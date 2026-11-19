"""Physical Stock Entry (legacy frmTrnPSE): a stock-take.

Load active items (by group / package / item) with their system stock, enter the counted stock and, if needed, new
purchase rate / MRP. Saving
  - updates the item's MRP and purchase rate where they changed,
  - posts the excess (counted > system) as a Purchase Bill from supplier KSBCL, remark "From Physical Stock Entry",
  - posts the shortage (counted < system) as a Stock Deduction, or as a Sales Invoice when "Show Reduced Qty as Sales" is
    ticked (needs a sales counter),
  - keeps the entry (the desktop's TrnHdrPSE / TrnDtlPSE) with the bill numbers it made.
Only rows that changed (quantity, purchase rate or MRP) are kept, like the desktop. Stock moves only through the
documents above, which use store.move_stock. The documents are web documents (GST on the purchase bill comes from the
item, where the desktop wrote none).
"""
from datetime import date

from fastapi import APIRouter, Depends, HTTPException, Query
from pydantic import BaseModel, Field

import logging

from .. import admin_audit, gsettings, security, sqlstore, store
from ..schemas import Line, PurchaseBillIn, SalesInvoiceIn, StockDeductionIn
from .auth import can_see_cost, get_current_user, has_right, require_right
from .masters import _check_margin
from .transactions import _build_purchase_bill, create_purchase_bill, create_sales_invoice, create_stock_deduction

logger = logging.getLogger(__name__)
router = APIRouter(prefix="/transactions/physical-stock", tags=["Physical Stock Entry"])
MODULE = "Physical Stock Entry"
SUPPLIER = "KSBCL"


class PseLine(BaseModel):
    item_code: str
    physical_qty: int = Field(ge=0, le=1_000_000)
    pur_rate: float | None = Field(None, ge=0, description="Omit (null) to keep the item's purchase rate; roles without cost access never send it")
    mrp: float = Field(ge=0)


class PseIn(BaseModel):
    lines: list[PseLine] = Field(min_length=1, max_length=5000)
    reduced_as_sales: bool = False
    counter: str | None = None
    salesperson: str | None = None


def _undo() -> None:
    """A step failed after another document was posted: throw the in-memory changes away and read the database again."""
    if not sqlstore._active:   # no database in use (tests, scripts): there is nothing to read back
        return
    try:
        sqlstore.reload()
    except Exception:   # noqa: BLE001 - the original error is the one to report
        logger.exception("Could not reload after a failed physical stock entry")


def _csv(value: str | None) -> set[str]:
    return {v.strip() for v in (value or "").split(",") if v.strip()}


@router.get("/load")
def load(groups: str | None = None, packages: str | None = None, items: str | None = None,
         current_user: dict = Depends(get_current_user)):
    """Active items with their system stock. At least one of groups / packages / items is needed (as on the desktop)."""
    g, p, i = _csv(groups), _csv(packages), _csv(items)
    if not (g or p or i):
        raise HTTPException(422, "Please Select Alteast One Item / ItemGroup / Package..")   # frmTrnPSE.btnView_Click 337
    show_cost = can_see_cost(current_user)
    rows = []
    for it in store.ITEMS.values():
        if not it["active"] or (g and it["group"] not in g) or (p and it["package"] not in p) or (i and it["code"] not in i):
            continue
        pkg = store.PACKAGES.get(it["package"], {})
        rows.append({"item_code": it["code"], "item_name": it["name"], "group": it["group"], "package": it["package"],
                     "litres_per_bottle": pkg.get("litres_per_bottle", 0), "bottles_per_case": it.get("bottles_per_case") or 0,
                     "pur_rate": it["purchase_rate"] if show_cost else None, "mrp": it["mrp"], "system_qty": it["qoh"]})
    # frmTrnPSE.fillData 412: Order by GroupOrderBy, ItemGroupName, (PkgItem,) PkgOrderBy, PurRate desc: the shelf order set in Arrange Slno.
    # The purchase rate is the item's own even when this role cannot see it, so the order is the same for everyone.
    rate = {i["code"]: i["purchase_rate"] for i in store.ITEMS.values()}
    rows.sort(key=lambda r: (store.ITEM_GROUPS.get(r["group"], {}).get("_order", 0), r["group"], store.PACKAGES.get(r["package"], {}).get("_order", 0), r["package"], -(rate.get(r["item_code"]) or 0)))
    return rows


@router.get("")
def history(current_user: dict = Depends(get_current_user)):
    """Earlier physical stock entries, newest first (View Previous Entered Records)."""
    docs = [{k: d[k] for k in ("doc_no", "date", "added_by", "sys_name")} | {"changed_items": len(d["lines"])}
            for d in store.DOCS["physical_stock"].values()]
    return sorted(docs, key=lambda d: d["doc_no"], reverse=True)


@router.get("/{doc_no:path}")
def get_entry(doc_no: str, current_user: dict = Depends(get_current_user)):
    doc = store.DOCS["physical_stock"].get(doc_no)
    if not doc:
        raise HTTPException(404, f"Physical stock entry '{doc_no}' not found")
    if not can_see_cost(current_user):
        doc = {**doc, "lines": [{**l, "pur_rate": None, "old_pur_rate": None} for l in doc["lines"]]}
    return doc


@router.post("", status_code=201, dependencies=[Depends(require_right("add", MODULE))])
def save(body: PseIn, current_user: dict = Depends(get_current_user)):
    today = date.today()
    gsettings.check_not_frozen(today, "This physical stock entry")
    if len({l.item_code for l in body.lines}) < len(body.lines):
        raise HTTPException(422, "Each item can appear only once in a physical stock entry")

    # 1. Work out what changed, and check everything before anything is posted.
    changed = []
    for l in body.lines:
        it = store.ITEMS.get(l.item_code)
        if not it or not it["active"]:
            raise HTTPException(422, f"Unknown or inactive item '{l.item_code}'")
        diff = l.physical_qty - it["qoh"]
        new_rate = it["purchase_rate"] if l.pur_rate is None else l.pur_rate
        rate_changed, mrp_changed = new_rate != it["purchase_rate"], l.mrp != it["mrp"]
        if not (diff or rate_changed or mrp_changed):
            continue
        if rate_changed and not can_see_cost(current_user):
            raise HTTPException(403, "You may not change the purchase rate (no cost access for your role)")
        for field, did in (("purchase_rate", rate_changed), ("mrp", mrp_changed)):
            if did and security.field_rights(current_user, "items").get(field):
                raise HTTPException(403, f"You may not change {field.replace('_', ' ')} (Field Level Rights for your role)")
        if rate_changed or mrp_changed:
            _check_margin({**it, "purchase_rate": new_rate, "mrp": l.mrp})
        changed.append({"item_code": it["code"], "old_pur_rate": it["purchase_rate"], "old_mrp": it["mrp"], "pur_rate": new_rate,
                        "mrp": l.mrp, "physical_qty": l.physical_qty, "system_qty": it["qoh"], "diff_qty": diff,
                        "pb_no": "", "stkded_no": ""})
    if not changed:
        raise HTTPException(422, "There are no Changes to Update!")
    excess = [c for c in changed if c["diff_qty"] > 0]
    short = [c for c in changed if c["diff_qty"] < 0]
    if excess and not store.find(store.SUPPLIERS, SUPPLIER):
        raise HTTPException(422, f"Supplier '{SUPPLIER}' is needed for the purchase bill of the excess stock but is not in Supplier Master")
    # The documents are posted for the user, so the user needs the right to add each of them (the router calls skip those checks).
    for needed, docs in (("Purchase Bill", excess), ("Sales Invoice" if body.reduced_as_sales else "Stock Deduction", short)):
        if docs and not has_right(current_user, "add", needed):
            raise HTTPException(403, f"Your role ({current_user.get('role')}) is not allowed to add {needed}, which this entry would post")
    if short and body.reduced_as_sales:
        counter = store.COUNTERS.get(body.counter or "")
        if not counter or counter["inactive"]:
            raise HTTPException(422, "Choose an active sales counter to post the reduced quantity as sales")
        if gsettings.yes("SPID") and not body.salesperson:
            raise HTTPException(422, "Salesperson is required (Global Settings)")

    # 2. Post the documents, the one most likely to be refused first (a sales invoice has the most rules), each with the
    # rate written on it, so the item's new rates can be applied last. If a later step still fails, the earlier ones are
    # undone by reloading from SQL Server (as a failed save does), so nothing half-done stays in memory.
    try:
        if short:
            lines = [Line(item_code=c["item_code"], bottles=-c["diff_qty"], rate=c["mrp"] if body.reduced_as_sales else c["pur_rate"]) for c in short]
            if body.reduced_as_sales:
                doc = create_sales_invoice(SalesInvoiceIn(date=today, counter=body.counter, salesperson=body.salesperson, lines=lines), current_user)
            else:
                doc = create_stock_deduction(StockDeductionIn(date=today, reason="Other", lines=lines, remarks="From Physical Stock Entry"))
            for c in short:
                c["stkded_no"] = doc["doc_no"]
        if excess:
            pb = create_purchase_bill(PurchaseBillIn(
                date=today, supplier=SUPPLIER, supplier_inv_no="PSE", supplier_inv_date=today, remarks="From Physical Stock Entry",
                lines=[Line(item_code=c["item_code"], bottles=c["diff_qty"], rate=c["pur_rate"]) for c in excess]))
            for c in excess:
                c["pb_no"] = pb["doc_no"]
    except Exception:
        _undo()
        raise

    # 3. Rates and MRP.
    for c in changed:
        it = store.ITEMS[c["item_code"]]
        for field, key in (("purchase_rate", "pur_rate"), ("mrp", "mrp")):
            if it[field] != c[key]:
                admin_audit.record(MODULE, "Edited", it["code"], f"{field}: {it[field]}", f"{field}: {c[key]}")
                it[field] = c[key]
                it["changed_by"] = current_user["user"]
                store.touch("ITEMS", it["code"])

    # 4. The entry itself.
    doc_no = store.next_doc_no("physical_stock", "PSE")
    doc = {"doc_no": doc_no, "date": today.isoformat(), "added_by": current_user["user"], "sys_name": "",
           "reduced_as_sales": body.reduced_as_sales, "lines": changed}
    store.DOCS["physical_stock"][doc_no] = doc
    admin_audit.record(MODULE, "Added", doc_no, "", f"{len(changed)} item(s) changed",
                       [{"code": c["item_code"], "name": store.ITEMS[c["item_code"]]["name"], "status": "Added",
                         "details": f"system {c['system_qty']}, counted {c['physical_qty']}, diff {c['diff_qty']}"} for c in changed])
    return doc


# ─── Update Item Stock (legacy frmUpdateItemStock): opening stock from a sheet ───────────────────────────────────────
# The desktop reads StockImport_Template.xlsx (ItemCode, ItemName, UOM, Stock, Rate), marks erroneous rows (item not
# found, negative stock), refuses to import until all rows are right, then posts the stock as one purchase bill from
# KSBCL with prefix OP: its own number series (26OP00001, sqlstore.next_opening_bill_no), dated today, no GST on the lines, the same
# item allowed twice. "Include Rate" uses the sheet's rate, else the item's purchase rate. The web does the same through the Purchase
# Bill (it shows in the Purchase Bill list under that OP number), so stock moves with store.move_stock.
stock_router = APIRouter(prefix="/utilities/update-item-stock", tags=["Update Item Stock"])
STOCK_MODULE = "Update Item Stock"


class StockRow(BaseModel):
    item_code: str = Field(max_length=60)
    stock: str = Field(max_length=30)
    rate: str = Field("", max_length=30)


class StockImportIn(BaseModel):
    rows: list[StockRow] = Field(min_length=1, max_length=5000)
    include_rate: bool = False
    dry_run: bool = True


def _number(text: str) -> float | None:
    try:
        return float(str(text).replace(",", "").strip() or 0)
    except ValueError:
        return None


@stock_router.post("", dependencies=[Depends(require_right("add", STOCK_MODULE))])
def update_item_stock(body: StockImportIn, current_user: dict = Depends(get_current_user)):
    """dry_run=true checks every row and returns {rows: [{row, error}], ok}; dry_run=false posts the purchase bill (all rows must pass)."""
    results, lines = [], []
    for n, r in enumerate(body.rows, start=1):
        errors = []
        code = r.item_code.strip().replace("'", "`")
        item = store.ITEMS.get(code)
        if not item:
            errors.append("Item not found")
        qty, rate = _number(r.stock), _number(r.rate)
        if qty is None or qty < 0 or qty != int(qty):
            errors.append("Invalid Stock")
        if body.include_rate and (rate is None or rate < 0):   # as the desktop, the sheet's rate is ignored (not checked) unless Include Rate is on
            errors.append("Invalid Rate")
        wrong = (["code"] if not item else []) + (["stock"] if qty is None or qty < 0 or qty != int(qty) else []) + (["rate"] if body.include_rate and (rate is None or rate < 0) else [])
        results.append({"row": n, "item_code": code, "error": "; ".join(errors), "fields": wrong})   # `fields`: the cells to mark in the sheet
        if not errors and qty > 0:
            lines.append(Line(item_code=item["code"], bottles=int(qty), rate=rate if body.include_rate and rate else None))
    ok = not any(r["error"] for r in results)
    if body.dry_run:
        return {"ok": ok, "rows": results, "importable": len(lines)}
    if not ok:
        raise HTTPException(422, "Importing of data is not allowed until all the data is correct")
    if not lines:
        raise HTTPException(422, "There is no stock above zero to import")
    if not store.find(store.SUPPLIERS, SUPPLIER):
        raise HTTPException(422, f"Supplier '{SUPPLIER}' is needed for the opening stock bill but is not in Supplier Master")
    today = date.today()
    pb = _build_purchase_bill(PurchaseBillIn(date=today, supplier=SUPPLIER, supplier_inv_no="", supplier_inv_date=today,
                                             remarks="Opening stock (Update Item Stock)", lines=lines),
                              doc_no=sqlstore.next_opening_bill_no(), opening=True)
    admin_audit.record(STOCK_MODULE, "Imported", pb["doc_no"], "", f"{len(lines)} item(s)")
    return {"ok": True, "doc_no": pb["doc_no"], "imported": len(lines), "skipped_zero": len(results) - len(lines)}
