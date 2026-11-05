"""Transaction screens (legacy frmTrnVch engine). Each posts a document and,
where the legacy transaction 'affects stock', moves item QOH.

Every POST needs the matching module in the caller's role plus its Add tick, and cancelling a bill its
Delete tick (require_right). Anything that takes
stock out (sale, held-bill release, stock deduction, purchase return) first checks the total
quantity per item across all lines, so stock can't go below zero.

Documents are saved to SQL Server's Web_ tables when the request succeeds (app/sqlstore.py); sales invoices
and purchase bills also in the desktop's format. Held bills are saved whole but not posted until released."""
from collections import defaultdict
from datetime import date, datetime, timedelta

from fastapi import APIRouter, Depends, HTTPException

from .. import admin_audit, sms, db, db_store, doc_engine, expense_db, doc_meta, doc_settings, gsettings, schema_meta, security, sqlstore, store
from ..schemas import (
    CancelIn, ClosingStockIn, DenominationIn, ExpenseIn, ExpenseOpeningIn, GenerateInvoiceIn, Line, PurchaseBillIn, PurchaseIndentIn, SalesInvoiceIn,
    SalesReturnIn, StockDeductionIn,
)
from .auth import get_current_user, hides_cost, require_right

router = APIRouter(prefix="/transactions")

# A bill with no customer is the walk-in cash customer's ("CASH", the desktop's CUSTID 0); no points.
WALK_IN = {"code": "CASH", "name": "CASH", "points": 0.0}


@router.get("/next-number", tags=["Sales Invoice"])
def next_number(kind: str, counter: str = "", current_user: dict = Depends(get_current_user)):
    """The number the next document of a kind will most likely get (a sales invoice: on that counter). Only a
    preview, nothing is taken: the number is given when the document is saved, and another counter may save first."""
    if kind not in store.DOCS:
        raise HTTPException(404, f"Unknown document kind '{kind}'")
    if kind == "sales_invoice":
        row = store.COUNTERS.get(counter)
        if row is None:
            raise HTTPException(404, f"Sales counter '{counter}' not found")
        prefix = row["prefix"]
    else:
        prefix = doc_settings.prefix(kind)
    if store.NUMBER_SYNC:
        store.NUMBER_SYNC(kind)   # other PCs may have saved since start-up
    seq = store._seq.setdefault(kind, store.Seq(1))
    return {"doc_no": f"{store.FY_PREFIX}/{prefix}-{seq.value:07d}", "fy_prefix": store.FY_PREFIX, "prefix": prefix,
            "number": seq.value}

SALES = Depends(require_right("add", "Sales Invoice"))
SALES_CANCEL = Depends(require_right("delete", "Sales Invoice"))


def _item(code: str) -> dict:
    if code not in store.ITEMS:
        raise HTTPException(422, f"Unknown item '{code}'")
    security.check_authorised("items", code, "Item")   # Master Approvals
    return store.ITEMS[code]


def _price_lines(lines: list[Line], rate_field: str, with_gst: bool, kind: str | None = None) -> tuple[list[dict], dict]:
    """Price the lines. `kind` (a bill that has designer formulas for its line amount / CGST / SGST) lets those formulas
    decide those three figures; without a formula, or when it can't be worked out, the built-in maths stands."""
    out, tot = [], {"qty": 0, "litres": 0.0, "basic": 0.0, "cgst": 0.0, "sgst": 0.0}
    for ln in lines:
        it = _item(ln.item_code)
        qty = ln.cases * it["bottles_per_case"] + ln.bottles
        if qty <= 0:
            raise HTTPException(422, f"Quantity for '{ln.item_code}' must be > 0")
        rate = ln.rate if ln.rate is not None else it[rate_field]
        amt = round(qty * rate, 2)
        cgst = round(amt * it["cgst_per"] / 100, 2) if with_gst else 0.0
        sgst = round(amt * it["sgst_per"] / 100, 2) if with_gst else 0.0
        litres = qty * store.PACKAGES.get(it["package"], {}).get("litres_per_bottle", 0)
        out.append({"item_code": it["code"], "item_name": it["name"], "cases": ln.cases, "bottles": ln.bottles,
                    "qty": qty, "rate": rate, "amount": amt, "cgst": cgst, "sgst": sgst, "litres": round(litres, 3)})
    if kind and with_gst:
        for line, own in zip(out, doc_engine.line_amounts(kind, out)):
            line.update(own)   # the designer's formula for that column, where there is one
    for line in out:
        tot["qty"] += line["qty"]
        tot["litres"] = round(tot["litres"] + line["litres"], 3)
        tot["basic"] = round(tot["basic"] + line["amount"], 2)
        tot["cgst"] = round(tot["cgst"] + line["cgst"], 2)
        tot["sgst"] = round(tot["sgst"] + line["sgst"], 2)
    tot["net"] = round(tot["basic"] + tot["cgst"] + tot["sgst"], 2)
    return out, tot


def _check_stock(lines: list[dict]) -> None:
    """422 if taking these lines out would leave any item below zero (same item on several lines adds up)."""
    need: dict[str, float] = defaultdict(float)
    for l in lines:
        need[l["item_code"]] += l["qty"]
    for code, qty in need.items():
        qoh = store.ITEMS[code]["qoh"]
        if qty > qoh:
            raise HTTPException(422, f"Insufficient stock for {code}: {qoh:g} in stock, {qty:g} needed")


def _check_liquor_limits(lines: list[dict]) -> None:
    """The desktop's per-bill litre limits (Global Settings AllowBeerInLtr / AllowWineInLtr / AllowIMLinLtr, group Invoice): a bill with more
    Beer, Wine or IML litres than the limit is refused unless split billing is on (SPLITINV = Y). A limit the shop has no row for is not applied."""
    if gsettings.yes("SPLITINV"):
        return
    litres: dict[str, float] = defaultdict(float)
    for l in lines:
        kind = (store.ITEMS.get(l["item_code"], {}).get("liqtype") or "").strip().upper()
        if kind in ("BEER", "WINE", "IML"):
            litres[kind] += float(l.get("litres") or 0)
    for kind, code in (("BEER", "AllowBeerInLtr"), ("WINE", "AllowWineInLtr"), ("IML", "AllowIMLinLtr")):
        if code in store.GSETTINGS and litres[kind] > gsettings.number(code):
            raise HTTPException(422, f"{kind.title() if kind != 'IML' else 'IML'} quantity exceeds the limit of {gsettings.number(code):g} Ltr ({litres[kind]:g} Ltr on this bill)")


def _is_liquor(item: dict) -> bool:
    return store.ITEM_GROUPS.get(item.get("group"), {}).get("liquor", True)


def _get(kind: str, doc_no: str) -> dict:
    doc = store.DOCS[kind].get(doc_no)
    if not doc:
        raise HTTPException(404, f"Document '{doc_no}' not found")
    return doc


# Documents priced at purchase cost: their money figures are hidden from roles without cost access
# (quantities stay, e.g. the Dashboard counts indent bottles).
COST_DOCS = {"purchase_indent", "purchase_bill", "stock_deduction"}
LINE_MONEY = ("rate", "amount", "cgst", "sgst")
TOTAL_MONEY = ("basic", "cgst", "sgst", "net", "tcs", "cess", "balance_vs_supplier_value")


def _shown(kind: str, doc: dict, user: dict) -> dict:
    if kind not in COST_DOCS or not hides_cost(user):
        return doc
    return {
        **doc,
        "supplier_inv_value": None if "supplier_inv_value" in doc else doc.get("supplier_inv_value"),
        "lines": [{**l, **{k: None for k in LINE_MONEY if k in l}} for l in doc.get("lines", [])],
        "totals": {**doc.get("totals", {}), **{k: None for k in TOTAL_MONEY if k in doc.get("totals", {})}},
    }


def _listing(kind: str, tag: str):
    @router.get(f"/{kind.replace('_', '-')}", tags=[tag], name=f"List {tag}")
    def list_docs(from_date: date | None = None, to_date: date | None = None, status: str | None = None,
                  current_user: dict = Depends(get_current_user)):
        if kind == "sales_invoice":
            db_store.refresh_recent_sales()   # bills made on other PCs since start-up
        return [_shown(kind, d, current_user) for d in store.DOCS[kind].values()
                if (not from_date or d["date"] >= from_date.isoformat()) and (not to_date or d["date"] <= to_date.isoformat())
                and (not status or d.get("status") == status)]

    @router.get(f"/{kind.replace('_', '-')}/{{doc_no:path}}", tags=[tag], name=f"Get {tag}")
    def get_doc(doc_no: str, current_user: dict = Depends(get_current_user)):
        return _shown(kind, _get(kind, doc_no), current_user)


# ---------- Purchase Indent (does not affect stock) ----------
@router.post("/purchase-indent", status_code=201, tags=["Purchase Indent"],
             dependencies=[Depends(require_right("add", "Purchase Indent", "Purchase Bill"))])
def create_indent(body: PurchaseIndentIn):
    return create_indent_doc(body)


def create_indent_doc(body: PurchaseIndentIn, doc_no: str | None = None, base: dict | None = None) -> dict:
    """The indent logic itself (also used by app/routers/ai.py when a suggestion is approved). With `doc_no` / `base` it edits that saved indent."""
    gsettings.check_not_frozen(body.date, "This purchase indent")
    _doc_extras("TrnHdrPI", body)
    doc_settings.check_lines("purchase_indent", body.lines, body.date)
    sup = store.find(store.SUPPLIERS, body.supplier)
    if not sup:
        raise HTTPException(422, f"Unknown supplier '{body.supplier}'")
    security.check_authorised("suppliers", sup["code"], "Supplier")
    body.supplier = sup["code"]
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    doc_no = doc_no or store.next_doc_no("purchase_indent", doc_settings.prefix("purchase_indent"))
    store.DOCS["purchase_indent"][doc_no] = {"doc_no": doc_no, "date": body.date.isoformat(), "supplier": body.supplier,
                                             "lines": lines, "totals": tot, "remarks": body.remarks, "extras": body.extras}
    admin_audit.record("Purchase Indent", "Edited" if base else "Added", doc_no, "", f"supplier {body.supplier}",
                       admin_audit.doc_items(store.DOCS["purchase_indent"][doc_no], "Added"))
    return store.DOCS["purchase_indent"][doc_no]


def _editable(kind: str, doc_no: str, label: str) -> dict:
    """The saved document to edit: it must exist, not be cancelled / removed, and not be dated on or before the freeze date (or past the edit-days limit)."""
    old = _get(kind, doc_no)
    gsettings.check_not_frozen(old["date"], f"{label} {doc_no}")
    doc_settings.check_edit_days(kind, old["date"], f"{label} {doc_no}")
    if old.get("status") in ("CANCELLED", "DELETED"):
        raise HTTPException(409, f"{label} {doc_no} is cancelled; it can't be edited")
    return old


@router.put("/purchase-indent/{doc_no:path}", tags=["Purchase Indent"], dependencies=[Depends(require_right("edit", "Purchase Indent", "Purchase Bill"))])
def edit_indent(doc_no: str, body: PurchaseIndentIn):
    """Change a saved purchase indent in place (it moves no stock): same number, its rows rewritten."""
    old = _editable("purchase_indent", doc_no, "Purchase indent")
    doc = create_indent_doc(body, doc_no=doc_no, base=old)
    doc["revision"] = int(old.get("revision", 0)) + 1
    return doc


# ---------- Purchase Bill (adds stock) ----------
def _doc_extras(table: str, body, hold: bool = False) -> None:
    """Check the header columns the database added (Doc Designer) against the real table; 422 with the reason."""
    if not body.extras:
        return
    if not sqlstore.enabled():
        body.extras = {}
        return
    try:
        doc_id = doc_engine.doc_id_of(next((d["DocName"] for d in store.UTL_DOCS.values()
                                            if str(d.get("HdrTblName") or "").lower() == table.lower()), ""))
        typed_in = doc_meta.input_columns(doc_id) if doc_id is not None else set()   # a column the designer lets people type into
        allowed = {k: v for k, v in body.extras.items() if str(k).lower() in typed_in or not sqlstore.doc_blocked(str(k))}
        # A held bill is unfinished: blank mandatory extras are only enforced when it is released or saved.
        body.extras = schema_meta.clean_extras(table, {k: v for k, v in allowed.items() if not (hold and v in ("", None))},
                                               sqlstore.DOC_CORE[table.lower()] | sqlstore._NOT_EXTRA)
    except ValueError as e:
        raise HTTPException(422, str(e))


@router.post("/purchase-bill", status_code=201, tags=["Purchase Bill"], dependencies=[Depends(require_right("add", "Purchase Bill"))])
def create_purchase_bill(body: PurchaseBillIn):
    return _build_purchase_bill(body)


def _build_purchase_bill(body: PurchaseBillIn, doc_no: str | None = None, base: dict | None = None) -> dict:
    """Check, price and post a purchase bill. With `doc_no` / `base` it is an edit of that saved bill: same number, same place in the list."""
    gsettings.check_not_frozen(body.date, "This purchase bill")
    _doc_extras("TrnHdrPB", body)
    if body.copy_from_indent and not doc_settings.settings("purchase_bill")["allow_copy_from"]:
        raise HTTPException(422, "Copy From is switched off for Purchase Bill (Global Settings → Transaction Settings)")
    src = body.lines
    if not src and body.copy_from_indent:
        src = [Line(item_code=l["item_code"], cases=l["cases"], bottles=l["bottles"], rate=l["rate"])
               for l in _get("purchase_indent", body.copy_from_indent)["lines"]]
    if not src:
        raise HTTPException(422, "Add lines or pass copy_from_indent")
    sup = store.find(store.SUPPLIERS, body.supplier)
    if not sup:
        raise HTTPException(422, f"Unknown supplier '{body.supplier}'")
    security.check_authorised("suppliers", sup["code"], "Supplier")
    body.supplier = sup["code"]
    doc_settings.check_lines("purchase_bill", src, body.date)
    lines, tot = _price_lines(src, "purchase_rate", with_gst=True, kind="purchase_bill")
    tot["tcs"], tot["cess"] = body.tcs, body.cess
    tot["net"] = round(tot["net"] + body.tcs + body.cess, 2)
    tot["balance_vs_supplier_value"] = round(body.supplier_inv_value - tot["net"], 2)
    doc_no = doc_no or store.next_doc_no("purchase_bill", doc_settings.prefix("purchase_bill"))
    sign = -1 if body.purchase_type == "Purchase Return" else 1
    if sign < 0:
        _check_stock(lines)
    for l in lines:
        store.move_stock(l["item_code"], sign * l["qty"], "purchase", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["purchase_bill"][doc_no] = doc
    if doc_settings.settings("purchase_bill")["update_rate_from_pb"] and sign > 0:
        # Invoice Settings "Update Rate From PB to Item Master": the item's purchase rate follows this bill.
        for l in lines:
            item = store.ITEMS[l["item_code"]]
            if l["rate"] and item.get("purchase_rate") != l["rate"]:
                admin_audit.record("Item Master", "Rate from purchase bill", l["item_code"],
                                   f"purchase_rate: {item.get('purchase_rate')}", f"purchase_rate: {l['rate']} ({doc_no})")
                item["purchase_rate"] = l["rate"]
                store.touch("ITEMS", l["item_code"])
    admin_audit.record("Purchase Bill", "Edited" if base else "Added", doc_no, f"net {base['totals']['net']}" if base else "",
                       f"{body.purchase_type}; supplier {sup['code']}; net {tot['net']}", admin_audit.doc_items(doc, "Added"))
    return doc


def _undo_edit(snap: dict, kind: str, doc_no: str, old: dict) -> None:
    """An edit failed after the old bill's effects were taken back: put stock, points, rates and the bill itself back as they were."""
    for code, v in snap["qoh"].items():
        store.ITEMS[code]["qoh"] = v
    for code, v in snap.get("rate", {}).items():
        store.ITEMS[code]["purchase_rate"] = v
    del store.MOVEMENTS[snap["moves"]:]
    for code, v in snap.get("custs", {}).items():
        if code in store.CUSTOMERS:
            store.CUSTOMERS[code]["points"] = v
    store.DOCS[kind][doc_no] = old


def _snapshot(codes, custs=()) -> dict:
    return {"qoh": {c: store.ITEMS[c]["qoh"] for c in codes if c in store.ITEMS}, "rate": {c: store.ITEMS[c].get("purchase_rate") for c in codes if c in store.ITEMS},
            "moves": len(store.MOVEMENTS), "custs": {c["code"]: c["points"] for c in custs if c}}


@router.put("/purchase-bill/{doc_no:path}", tags=["Purchase Bill"], dependencies=[Depends(require_right("edit", "Purchase Bill"))])
def edit_purchase_bill(doc_no: str, body: PurchaseBillIn):
    """Change a saved purchase bill in place (same number): the old bill's stock goes out, the new lines come in, and the bill's rows are rewritten.
    Refused when the old or new date is on or before the freeze date, or when the stock the old bill added has been sold since."""
    old = _get("purchase_bill", doc_no)
    gsettings.check_not_frozen(old["date"], f"Purchase bill {doc_no}")
    doc_settings.check_edit_days("purchase_bill", old["date"], f"Purchase bill {doc_no}")
    if body.copy_from_indent:
        raise HTTPException(422, "Copy From is for a new bill")
    codes = {l["item_code"] for l in old["lines"]} | {l.item_code for l in body.lines}
    snap = _snapshot(codes)
    old_sign = -1 if old.get("purchase_type") == "Purchase Return" else 1
    for l in old["lines"]:   # take the old bill's stock effect back (its rows in the stock list go, as in the desktop's edit)
        store.move_stock(l["item_code"], -old_sign * l["qty"], "purchase", doc_no, date.fromisoformat(old["date"]), l["rate"])
        store.MOVEMENTS[-1]["reverses"] = True
    try:
        doc = _build_purchase_bill(body, doc_no=doc_no, base=old)
        short = [c for c in codes if c in store.ITEMS and store.ITEMS[c]["qoh"] < 0]
        if short:
            raise HTTPException(409, f"This change would leave {', '.join(sorted(short)[:3])} below zero stock (the stock the old bill added was sold since)")
    except Exception:
        _undo_edit(snap, "purchase_bill", doc_no, old)
        raise
    doc["revision"] = int(old.get("revision", 0)) + 1
    return doc


# ---------- Sales Invoice (reduces stock) ----------
def _post_sale(doc: dict, cust: dict) -> None:
    """Take a saved bill's stock out and settle its loyalty points (the bill must already be checked)."""
    on = date.fromisoformat(doc["date"])
    for l in doc["lines"]:
        store.move_stock(l["item_code"], -l["qty"], "sale", doc["doc_no"], on, l["rate"])
    tot = doc["totals"]
    earned = 0.0   # Global Settings: MinBillForPoints, PointsForOneRs
    if doc["customer"] != "CASH" and tot["net"] > gsettings.number("MinBillForPoints"):
        earned = round(tot["net"] * gsettings.number("PointsForOneRs"), 2)
    tot["points_earned"] = earned
    cust["points"] = round(cust["points"] - tot["points_redeemed"] + earned, 2)
    store.touch("CUSTOMERS", cust["code"])


@router.post("/sales-invoice", status_code=201, tags=["Sales Invoice"], dependencies=[SALES])
def create_sales_invoice(body: SalesInvoiceIn, current_user: dict = Depends(get_current_user)):
    if not body.hold and gsettings.yes("SPLITINV"):
        parts = _split_parts(body)
        if len(parts) > 1:
            return _create_split_bills(body, parts, current_user)
    doc = _build_sales_invoice(body, current_user)
    _loyalty_sms(doc)
    return doc


def _loyalty_sms(doc: dict) -> None:
    """The desktop's loyalty SMS (LoyaltySENDMAIL): a customer marked "Send SMS" with a mobile is told the points the bill earned and, when
    points were redeemed, the balance. Sent in the background, only when the shop has SMS on; a failure never touches the bill."""
    if doc.get("status") != "SAVED":
        return
    cust = store.CUSTOMERS.get(doc.get("customer") or "") or {}
    tot = doc.get("totals", {})
    if cust.get("code") in (None, "CASH") or not cust.get("send_sms") or not cust.get("mobile"):
        return
    for text in sms.loyalty_message(tot.get("points_earned", 0), tot.get("net", 0), tot.get("points_redeemed", 0), cust.get("points", 0)):
        sms.send_later(text, cust["mobile"])


_LIMIT_CODES = {"BEER": "AllowBeerInLtr", "WINE": "AllowWineInLtr", "IML": "AllowIMLinLtr"}


def _split_parts(body: SalesInvoiceIn) -> list[list[tuple[str, int, float]]]:
    """The desktop's split billing (frmTrnPosInv.saveSplitBills): when split billing is on (SPLITINV = Y) and the bill has more Beer, Wine or IML
    litres than the per-bill limit, it is saved as several bills, each within the limits. Every bottle is taken in the order of the lines and put
    in the current bill if its type still fits, else it waits for the next one; other items go in the first bill. Returns the parts as
    [(item_code, qty, rate), ...] each; one part when no split is needed."""
    lines, _tot = _price_lines(body.lines, "mrp", with_gst=True, kind="sales_invoice")
    units = []   # one entry per bottle: (item_code, rate, litres, type)
    for l in lines:
        kind = (store.ITEMS.get(l["item_code"], {}).get("liqtype") or "").strip().upper()
        each = float(l["litres"]) / l["qty"] if l["qty"] else 0.0
        units += [(l["item_code"], l["rate"], each, kind)] * int(l["qty"])
    limits = {k: gsettings.number(c) for k, c in _LIMIT_CODES.items() if c in store.GSETTINGS}
    parts, waiting = [], units
    while waiting:
        used: dict[str, float] = defaultdict(float)
        part, rest = [], []
        for u in waiting:
            kind = u[3]
            if kind in limits:
                add = round(u[2], 3)
                if used[kind] + add <= limits[kind]:
                    used[kind] += add
                    part.append(u)
                else:
                    rest.append(u)
            else:
                part.append(u)
        if not part:   # one bottle alone is over the limit: it can never fit, so save the rest as it is (the normal check then refuses it)
            return [[(u[0], 1, u[1]) for u in units]]
        merged: dict[str, list] = {}
        for code, rate, _lt, _k in part:
            merged.setdefault(code, [code, 0, rate])[1] += 1
        parts.append([tuple(m) for m in merged.values()])
        waiting = rest
    return parts


def _create_split_bills(body: SalesInvoiceIn, parts: list, current_user: dict) -> dict:
    """Save each part of a split bill as its own bill (own number). The discount, redeemed points and cash count go on the first bill."""
    if body.payment is not None or body.payment_type == "SPLIT":
        raise HTTPException(422, "This bill is over the litre limit and must be saved as several bills: choose one payment type (Cash, Card or UPI), not a split payment")
    if not gsettings.yes("AllowNegative"):
        _check_stock(_price_lines(body.lines, "mrp", with_gst=True, kind="sales_invoice")[0])
    docs = []
    try:
        for n, part in enumerate(parts):
            sub = body.model_copy(update={
                "lines": [Line(item_code=c, bottles=q, rate=r) for c, q, r in part],
                "discount": body.discount if n == 0 else 0, "redeem_points": body.redeem_points if n == 0 else 0,
                "denominations": body.denominations if n == 0 else [], "collected": body.collected if n == 0 and len(parts) == 1 else None,
            })
            docs.append(_build_sales_invoice(sub, current_user))
    except Exception:
        if docs:   # an earlier part is already posted in memory: put the store back to what the database holds, as a failed save does
            sqlstore.reload()
        raise
    for d in docs:
        _loyalty_sms(d)
    return docs[0] | {"split_bills": [d["doc_no"] for d in docs]}


def _build_sales_invoice(body: SalesInvoiceIn, current_user: dict, doc_no: str | None = None, base: dict | None = None) -> dict:
    """Check, price and post a bill. With `doc_no` / `base` it is an edit of that saved bill: same number, time and author."""
    gsettings.check_not_frozen(body.date, "This bill")
    _doc_extras("TrnHdrSINV", body, body.hold)
    doc_settings.check_lines("sales_invoice", body.lines, body.date)
    counter = store.COUNTERS.get(body.counter)
    if not counter or counter["inactive"]:
        raise HTTPException(422, "A sales invoice needs an active Sales Counter")
    if gsettings.yes("SPID") and not body.salesperson:
        raise HTTPException(422, "Salesperson is required (Global Settings)")
    cust = store.find(store.CUSTOMERS, body.customer)
    if not cust and body.customer.strip().upper() in ("", "CASH"):
        cust = WALK_IN   # the walk-in cash customer, also when the shop's customer list has no CASH row
    if not cust:
        raise HTTPException(422, f"Unknown customer '{body.customer}'")
    security.check_authorised("customers", cust["code"], "Customer")
    body.customer = cust["code"]
    lines, tot = _price_lines(body.lines, "mrp", with_gst=True, kind="sales_invoice")
    if not body.hold:
        if not gsettings.yes("AllowNegative"):   # the desktop's AllowNegative setting: Y sells below zero stock
            _check_stock(lines)
        _check_liquor_limits(lines)
    if body.redeem_points > cust["points"]:
        raise HTTPException(422, f"Customer has only {cust['points']} points")
    if body.redeem_points and cust["code"] == "CASH":
        raise HTTPException(422, "Points can only be redeemed on a customer's bill")
    if body.discount + body.redeem_points > tot["net"]:
        raise HTTPException(422, "Discount and redeemed points can't be more than the bill amount")
    tot["discount"] = body.discount
    tot["points_redeemed"] = body.redeem_points
    tot["net"] = round(tot["net"] - body.discount - body.redeem_points)  # bills are rounded to the rupee
    if body.collected is not None:
        diff = round(body.collected - tot["net"], 2)
        tot["collected"], tot["payback"], tot["shortage"] = body.collected, max(diff, 0), max(-diff, 0)
    if body.payment:
        payment = body.payment.model_dump()
        paid = round(sum(payment.values()), 2)
        # The screen shows paise; the bill is rounded to the rupee, so allow up to ₹1 of rounding.
        if abs(paid - tot["net"]) > 1:
            raise HTTPException(422, f"Payment total ₹{paid:,.2f} does not match the bill amount ₹{tot['net']:,.2f}")
        biggest = max(payment, key=lambda k: payment[k])
        payment[biggest] = round(payment[biggest] + tot["net"] - paid, 2)
    else:
        payment = {"cash": 0, "card": 0, "upi": 0, "credit": 0}
        payment[{"CASH": "cash", "CARD": "card", "UPI": "upi", "CREDIT": "credit", "SPLIT": "cash"}[body.payment_type]] = tot["net"]
    tot["points_earned"] = 0.0
    doc_no = doc_no or (f"HOLD-{next(store._seq['hold']):04d}" if body.hold else store.next_doc_no("sales_invoice", counter["prefix"]))
    doc = body.model_dump(mode="json", exclude={"lines", "payment"}) | {
        "doc_no": doc_no, "status": "HOLD" if body.hold else "SAVED", "lines": lines, "totals": tot, "payment": payment,
        "time": (base or {}).get("time") or datetime.now().strftime("%H:%M:%S"), "added_by": (base or {}).get("added_by") or current_user["user"],
    }
    if not body.hold:
        _post_sale(doc, cust)
    store.DOCS["sales_invoice"][doc_no] = doc
    admin_audit.record("Sales Invoice", "Edited" if base else ("Held" if body.hold else "Added"), doc_no, f"net {base['totals']['net']}" if base else "",
                       f"counter {doc['counter']}; customer {doc['customer']}; net {tot['net']}",
                       admin_audit.doc_items(doc, "Added"))
    return doc


@router.put("/sales-invoice/{doc_no:path}", tags=["Sales Invoice"], dependencies=[Depends(require_right("edit", "Sales Invoice"))])
def edit_sales_invoice(doc_no: str, body: SalesInvoiceIn, current_user: dict = Depends(get_current_user)):
    """Change a saved bill in place (same number): its stock and loyalty points are taken back, the new bill is checked and posted as a new one
    would be, and the bill's rows are rewritten. A bill that was cancelled, removed, held or already returned against can't be edited."""
    old = _get("sales_invoice", doc_no)
    gsettings.check_not_frozen(old["date"], f"Bill {doc_no}")
    doc_settings.check_edit_days("sales_invoice", old["date"], f"Bill {doc_no}")
    if old["status"] != "SAVED":
        raise HTTPException(409, {"HOLD": "Held bills are changed on the counter: recall them", "CANCELLED": "A cancelled bill can't be edited",
                                  "DELETED": "This bill was removed as a duplicate"}.get(old["status"], "Only a saved bill can be edited"))
    if body.hold:
        raise HTTPException(422, "A saved bill can't be put on hold")
    if _returned_qty(doc_no):
        raise HTTPException(409, "Goods from this bill were already returned (Sales Return); it can't be edited")
    old_cust = store.CUSTOMERS.get(old["customer"])
    new_cust = store.find(store.CUSTOMERS, body.customer)
    codes = {l["item_code"] for l in old["lines"]} | {l.item_code for l in body.lines}
    snap = _snapshot(codes, (old_cust, new_cust))
    for l in old["lines"]:   # put the old bill's stock back (its stock-list rows go, as a cancel does) ...
        store.move_stock(l["item_code"], l["qty"], "sale-cancel", doc_no, date.fromisoformat(old["date"]), l["rate"])
    if old_cust:   # ... and give back the points it redeemed, take back the ones it earned
        old_cust["points"] = round(old_cust["points"] + old["totals"]["points_redeemed"] - old["totals"]["points_earned"], 2)
    try:
        doc = _build_sales_invoice(body, current_user, doc_no=doc_no, base=old)
    except Exception:
        _undo_edit(snap, "sales_invoice", doc_no, old)
        raise
    doc["revision"] = int(old.get("revision", 0)) + 1
    doc["edited_by"] = current_user["user"]
    return doc


@router.post("/sales-invoice/{doc_no:path}/release", tags=["Sales Invoice"], dependencies=[SALES])
def release_held_bill(doc_no: str):
    """Recall a held bill and post it: checks stock and points again, moves stock, earns points."""
    doc = _get("sales_invoice", doc_no)
    gsettings.check_not_frozen(doc["date"], f"Bill {doc_no}")
    if doc["status"] != "HOLD":
        raise HTTPException(409, "Only held bills can be released")
    counter = store.COUNTERS.get(doc["counter"])
    if not counter or counter["inactive"]:
        raise HTTPException(422, "The bill's Sales Counter is missing or inactive")
    _check_stock(doc["lines"])
    for l in doc["lines"]:
        security.check_authorised("items", l["item_code"], "Item")      # changed after the bill was held
    security.check_authorised("customers", doc["customer"], "Customer")
    cust = store.CUSTOMERS.get(doc["customer"]) or WALK_IN
    if doc["totals"]["points_redeemed"] > cust["points"]:
        raise HTTPException(422, f"Customer has only {cust['points']} points")
    # Releasing turns the parked bill into a real invoice with the next number in its counter's series.
    new_no = store.next_doc_no("sales_invoice", counter["prefix"])
    del store.DOCS["sales_invoice"][doc_no]
    doc.update(doc_no=new_no, status="SAVED")
    admin_audit.record("Sales Invoice", "Released", new_no, f"held bill {doc_no}", f"net {doc['totals']['net']}",
                       admin_audit.doc_items(doc, "Added"))
    _post_sale(doc, cust)
    store.DOCS["sales_invoice"][new_no] = doc
    return doc


@router.delete("/sales-invoice/{doc_no:path}", status_code=204, tags=["Sales Invoice"], dependencies=[SALES])
def discard_held_bill(doc_no: str):
    """Remove a held bill once it has been recalled onto the counter (held bills never moved stock)."""
    if _get("sales_invoice", doc_no)["status"] != "HOLD":
        raise HTTPException(409, "Only held bills can be discarded; cancel saved bills instead")
    admin_audit.record("Sales Invoice", "Held bill discarded", doc_no, "status: HOLD", "discarded",
                       admin_audit.doc_items(store.DOCS["sales_invoice"][doc_no], "Removed"))
    del store.DOCS["sales_invoice"][doc_no]


@router.post("/sales-invoice/{doc_no:path}/cancel", tags=["Sales Invoice"], dependencies=[SALES_CANCEL])
def cancel_invoice(doc_no: str, body: CancelIn):
    """Cancel a saved bill with a mandatory remark and put its stock back. Cancelled bills show in the DSR."""
    doc = _get("sales_invoice", doc_no)
    gsettings.check_not_frozen(doc["date"], f"Bill {doc_no}")
    doc_settings.check_edit_days("sales_invoice", doc["date"], f"Bill {doc_no}")
    if doc["status"] == "CANCELLED":
        raise HTTPException(409, "Already cancelled")
    if doc["status"] == "DELETED":
        raise HTTPException(409, "This bill was removed as a duplicate (Utilities → Remove Duplicate Invoice); its stock is already back")
    if doc["status"] == "HOLD":
        raise HTTPException(409, "Held bills are not posted yet: recall and discard them instead of cancelling")
    if _returned_qty(doc_no):
        raise HTTPException(409, "Goods from this bill were already returned (Sales Return); it can't be cancelled")
    for l in doc["lines"]:
        store.move_stock(l["item_code"], l["qty"], "sale-cancel", doc_no, date.fromisoformat(doc["date"]), l["rate"])
    # Give back redeemed points and take back the ones this bill earned.
    cust = store.CUSTOMERS.get(doc["customer"])
    if cust:
        cust["points"] = round(cust["points"] + doc["totals"]["points_redeemed"] - doc["totals"]["points_earned"], 2)
        store.touch("CUSTOMERS", cust["code"])
    doc["status"] = "CANCELLED"
    doc["cancel_remarks"] = body.remarks
    admin_audit.record("Sales Invoice", "Cancelled", doc_no, "status: SAVED", f"status: CANCELLED; reason: {body.remarks}",
                       admin_audit.doc_items(doc, "Removed"))
    return doc


def _returned_qty(invoice_no: str, skip: str | None = None) -> dict[str, float]:
    """Quantity per item already returned against an invoice (`skip`: a return being edited, which is not counted against itself)."""
    done: dict[str, float] = defaultdict(float)
    for r in store.DOCS["sales_return"].values():
        if r.get("against_invoice") == invoice_no and r.get("doc_no") != skip:
            for l in r["lines"]:
                done[l["item_code"]] += l["qty"]
    return done


@router.get("/sales-invoice/{doc_no:path}/reprint", tags=["Sales Invoice"])
def reprint_invoice(doc_no: str):
    """Re-Print Bill: returns the print payload (Crystal fmtSInv equivalent)."""
    doc = _get("sales_invoice", doc_no)
    return {"company": store.COMPANY, "invoice": doc, "copy": "DUPLICATE"}


# ---------- Sales Return (adds stock) ----------
@router.post("/sales-return", status_code=201, tags=["Sales Return"], dependencies=[Depends(require_right("add", "Sales Return"))])
def create_sales_return(body: SalesReturnIn):
    return _build_sales_return(body)


def _build_sales_return(body: SalesReturnIn, doc_no: str | None = None, base: dict | None = None) -> dict:
    """Check and post a sales return; with `doc_no` / `base` it edits that saved return."""
    gsettings.check_not_frozen(body.date, "This sales return")
    _doc_extras("TrnHdrSR", body)
    doc_settings.check_lines("sales_return", body.lines, body.date)
    if body.customer:
        security.check_authorised("customers", body.customer, "Customer")
    lines, tot = _price_lines(body.lines, "mrp", with_gst=True)
    if body.against_invoice:
        inv = _get("sales_invoice", body.against_invoice)
        if inv["status"] != "SAVED":
            raise HTTPException(422, f"Invoice {body.against_invoice} is {inv['status'].lower()}; only saved bills can be returned")
        sold: dict[str, float] = defaultdict(float)
        for l in inv["lines"]:
            sold[l["item_code"]] += l["qty"]
        done = _returned_qty(body.against_invoice, skip=doc_no)
        asked: dict[str, float] = defaultdict(float)
        for l in lines:
            asked[l["item_code"]] += l["qty"]
        for code, qty in asked.items():
            left = sold.get(code, 0) - done.get(code, 0)
            if qty > left:
                raise HTTPException(422, f"Only {left:g} of {code} can be returned against {body.against_invoice}")
        body.customer = inv["customer"]
    doc_no = doc_no or store.next_doc_no("sales_return", doc_settings.prefix("sales_return"))
    for l in lines:
        store.move_stock(l["item_code"], l["qty"], "sales-return", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["sales_return"][doc_no] = doc
    admin_audit.record("Sales Return", "Edited" if base else "Added", doc_no, "",
                       f"against {body.against_invoice}" if body.against_invoice else "without an invoice",
                       admin_audit.doc_items(doc, "Added"))
    return doc


# ---------- Expense Entry ----------
@router.post("/expense", status_code=201, tags=["Expense Entry"], dependencies=[Depends(require_right("add", "Expense Entry"))])
def create_expense(body: ExpenseIn, current_user: dict = Depends(get_current_user)):
    if db.configured():   # a real database: straight into the shop's own TrnHdrEXP / TrnDtlEXP (no Web_ tables)
        return expense_db.create_expense(body, current_user["user"])
    gsettings.check_not_frozen(body.date, "This expense entry")
    doc_settings.check_lines("expense", [], body.date)
    for l in body.lines:
        if l.ledger not in store.LEDGERS:
            raise HTTPException(422, f"Unknown ledger '{l.ledger}'")
    doc_no = store.next_doc_no("expense", doc_settings.prefix("expense"))
    doc = body.model_dump(mode="json") | {"doc_no": doc_no, "total": round(sum(l.amount for l in body.lines), 2)}
    store.DOCS["expense"][doc_no] = doc
    admin_audit.record("Expense Entry", "Added", doc_no, "", f"total {doc['total']}",
                       [{"code": l.ledger, "name": store.LEDGERS.get(l.ledger, {}).get("name", ""), "status": "Added",
                         "details": f"{l.amount}"} for l in body.lines])
    return doc


@router.put("/expense/{doc_no:path}", tags=["Expense Entry"], dependencies=[Depends(require_right("edit", "Expense Entry"))])
def edit_expense(doc_no: str, body: ExpenseIn, current_user: dict = Depends(get_current_user)):
    """Change a saved expense entry in place (same number): its date and lines are rewritten. Receipts into the petty cash fund are kept."""
    if db.configured():
        return expense_db.update_expense(doc_no, body, current_user["user"])
    old = _editable("expense", doc_no, "Expense entry")
    for l in body.lines:
        if l.ledger not in store.LEDGERS:
            raise HTTPException(422, f"Unknown ledger '{l.ledger}'")
    doc = body.model_dump(mode="json") | {"doc_no": doc_no, "total": round(sum(l.amount for l in body.lines), 2), "revision": int(old.get("revision", 0)) + 1}
    store.DOCS["expense"][doc_no] = doc
    admin_audit.record("Expense Entry", "Edited", doc_no, f"total {old.get('total')}", f"total {doc['total']}", [])
    return doc


# ---------- Stock Deduction (reduces stock) ----------
@router.post("/stock-deduction", status_code=201, tags=["Stock Deduction"], dependencies=[Depends(require_right("add", "Stock Deduction"))])
def create_stock_deduction(body: StockDeductionIn):
    return _build_stock_deduction(body)


def _build_stock_deduction(body: StockDeductionIn, doc_no: str | None = None, base: dict | None = None) -> dict:
    """Check and post a stock deduction; with `doc_no` / `base` it edits that saved one."""
    gsettings.check_not_frozen(body.date, "This stock deduction")
    _doc_extras("TrnHdrSTKDED", body)
    doc_settings.check_lines("stock_deduction", body.lines, body.date)
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    _check_stock(lines)
    doc_no = doc_no or store.next_doc_no("stock_deduction", doc_settings.prefix("stock_deduction"))
    for l in lines:
        store.move_stock(l["item_code"], -l["qty"], "deduction", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["stock_deduction"][doc_no] = doc
    admin_audit.record("Stock Deduction", "Edited" if base else "Added", doc_no, "", "", admin_audit.doc_items(doc, "Added"))
    return doc


def _edit_stock_doc(kind: str, doc_no: str, label: str, body, build, move_kind: str, sign: int) -> dict:
    """Edit a saved sales return (+stock) or stock deduction (-stock): the old document's stock effect is taken back (its stock-list rows go), the new one is
    checked and posted, and nothing changes when it fails or would leave an item below zero."""
    old = _editable(kind, doc_no, label)
    codes = {l["item_code"] for l in old["lines"]} | {l.item_code for l in body.lines}
    snap = _snapshot(codes)
    for l in old["lines"]:
        store.move_stock(l["item_code"], -sign * l["qty"], move_kind, doc_no, date.fromisoformat(old["date"]), l["rate"])
        store.MOVEMENTS[-1]["reverses"] = True
    try:
        doc = build(body, doc_no=doc_no, base=old)
        short = sorted(c for c in codes if c in store.ITEMS and store.ITEMS[c]["qoh"] < 0)
        if short:
            raise HTTPException(409, f"This change would leave {', '.join(short[:3])} below zero stock (the stock this {label.lower()} added was sold since)")
    except Exception:
        _undo_edit(snap, kind, doc_no, old)
        raise
    doc["revision"] = int(old.get("revision", 0)) + 1
    return doc


@router.put("/sales-return/{doc_no:path}", tags=["Sales Return"], dependencies=[Depends(require_right("edit", "Sales Return"))])
def edit_sales_return(doc_no: str, body: SalesReturnIn):
    return _edit_stock_doc("sales_return", doc_no, "Sales return", body, _build_sales_return, "sales-return", +1)


@router.put("/stock-deduction/{doc_no:path}", tags=["Stock Deduction"], dependencies=[Depends(require_right("edit", "Stock Deduction"))])
def edit_stock_deduction(doc_no: str, body: StockDeductionIn):
    return _edit_stock_doc("stock_deduction", doc_no, "Stock deduction", body, _build_stock_deduction, "deduction", -1)


# ---------- Closing Stock Entry ----------
@router.get("/closing-stock/load", tags=["Closing Stock Entry"])
def load_closing_stock(as_on: date):
    """Pick As-on Date → liquor items with their opening balance (current QOH)."""
    return [{"item_code": i["code"], "item_name": i["name"], "package": i["package"], "op_qty": i["qoh"], "mrp": i["mrp"]}
            for i in store.ITEMS.values() if _is_liquor(i) and i["active"]]


CLOSING = Depends(require_right("add", "Closing Stock Entry"))


@router.post("/closing-stock", status_code=201, tags=["Closing Stock Entry"], dependencies=[CLOSING])
def create_closing_stock(body: ClosingStockIn):
    """Physical closing stock per item; Sales Qty = OP − Closing."""
    gsettings.check_not_frozen(body.as_on, "This closing stock entry")
    doc_settings.check_lines("closing_stock", [], body.as_on)
    lines = []
    if len({l.item_code for l in body.lines}) < len(body.lines):
        raise HTTPException(422, "Each item can appear only once in a closing stock entry")
    for l in body.lines:
        it = _item(l.item_code)
        sales = it["qoh"] - l.closing_qty
        if sales < 0:
            raise HTTPException(422, f"Closing qty for {l.item_code} exceeds opening ({it['qoh']})")
        lines.append({"item_code": it["code"], "op_qty": it["qoh"], "closing_qty": l.closing_qty,
                      "sales_qty": sales, "mrp": it["mrp"], "amount": round(sales * it["mrp"], 2)})
    doc_no = store.next_doc_no("closing_stock", doc_settings.prefix("closing_stock"))
    doc = {"doc_no": doc_no, "date": body.as_on.isoformat(), "lines": lines, "invoice_no": None,
           "total_amount": round(sum(l["amount"] for l in lines), 2)}
    store.DOCS["closing_stock"][doc_no] = doc
    admin_audit.record("Closing Stock Entry", "Added", doc_no, "", f"total {doc['total_amount']}",
                       [{"code": l["item_code"], "name": store.ITEMS.get(l["item_code"], {}).get("name", ""), "status": "Added",
                         "details": f"OP {l['op_qty']}, closing {l['closing_qty']}, sold {l['sales_qty']}"} for l in lines])
    return doc


@router.post("/closing-stock/{doc_no:path}/generate-invoice", status_code=201, tags=["Closing Stock Entry"], dependencies=[CLOSING])
def generate_invoice_from_cse(doc_no: str, body: GenerateInvoiceIn, current_user: dict = Depends(get_current_user)):
    """Generate Sales Invoice from the computed sales quantities."""
    cse = _get("closing_stock", doc_no)
    gsettings.check_not_frozen(cse["date"], f"Closing stock {doc_no}")
    if cse["invoice_no"]:
        raise HTTPException(409, f"Invoice {cse['invoice_no']} already generated")
    lines = [Line(item_code=l["item_code"], bottles=int(l["sales_qty"])) for l in cse["lines"] if l["sales_qty"] > 0]
    if not lines:
        raise HTTPException(422, "No sales quantity to invoice")
    inv = create_sales_invoice(SalesInvoiceIn(date=date.fromisoformat(cse["date"]), counter=body.counter,
                                              salesperson=body.salesperson, lines=lines), current_user)
    cse["invoice_no"] = inv["doc_no"]
    admin_audit.record("Closing Stock Entry", "Invoice generated", doc_no, "", f"invoice {inv['doc_no']}")
    return inv


@router.get("/today-totals", tags=["Sales Invoice"])
def today_totals(on: date | None = None):
    """The footer's "Total Bills / Sales" as the desktop's status bar shows them: today's bills in SQL Server's TrnHdrSINV, cancelled ones
    included (as the desktop counts them) but not deleted ones, on every counter and PC (so a bill made in the Windows vLQMS shows at once). Read straight from the database;
    with it off, from the bills in memory (the same rule)."""
    day = on or date.today()
    if db.is_db_available():
        try:
            from ..db_store import _has
            net = "ISNULL(NULLIF(GRNDTOT, 0), TOTAMT)" if "grndtot" in _has("TrnHdrSINV") else "TOTAMT"
            gone = " AND ISNULL(IsDelete, 'N') <> 'Y'" if "isdelete" in _has("TrnHdrSINV") else ""
            row = db.fetchall(
                f"SELECT COUNT(*) AS n, ISNULL(SUM({net}), 0) AS amt FROM dbo.TrnHdrSINV WITH (NOLOCK) "
                f"WHERE SINVDate >= '{day.isoformat()}' AND SINVDate < '{(day + timedelta(days=1)).isoformat()}' {gone}")[0]
            return {"date": day.isoformat(), "total_bills": int(row["n"]), "sales": round(float(row["amt"]), 2), "source": "db"}
        except Exception:   # noqa: BLE001 - fall back to the bills in memory
            pass
    saved = [d for d in store.DOCS["sales_invoice"].values() if d.get("date") == day.isoformat() and d.get("status") in ("SAVED", "CANCELLED")]
    return {"date": day.isoformat(), "total_bills": len(saved), "sales": round(sum(d["totals"]["net"] for d in saved), 2), "source": "memory"}


@router.get("/denominations/config", tags=["Sales Invoice"])
def denomination_config():
    """The notes and coin row of the desktop's Denomination window (frmUtlDenom's grid): a fixed set, as the exe draws it with a picture per
    note. The shop's UtlDenom table is not read: it is a settings list some shops fill with other numbers (1500, 3000, ...)."""
    return {"values": ["2000", "500", "200", "100", "50", "20", "10", "coins"]}


# ---------- Cash Denomination (day-end cash count) ----------
@router.get("/denomination", tags=["Cash Denomination"])
def get_denomination(on: date):
    return store.DENOMINATIONS.get(on.isoformat())


@router.post("/denomination", status_code=201, tags=["Cash Denomination"], dependencies=[Depends(require_right("add", "Cash Denomination"))])
def save_denomination(body: DenominationIn):
    gsettings.check_not_frozen(body.date, "This cash count")
    total = 0.0
    for key, n in body.counts.items():
        # "coins" is entered as a rupee amount rather than a count.
        total += n if key == "coins" else float(key) * n
    doc = {"date": body.date.isoformat(), "counts": body.counts, "total": round(total, 2)}
    old = store.DENOMINATIONS.get(doc["date"])
    admin_audit.record("Cash Denomination", "Edited" if old else "Added", doc["date"],
                       f"total {old['total']}" if old else "", f"total {doc['total']}")
    store.DENOMINATIONS[doc["date"]] = doc
    return doc


# ---------- Expense Entry: Day's OP-Balance (legacy frmUtlDaysExpOPBalance, table TrnExpOPBal) ----------
@router.get("/expense/opening-balance", tags=["Expense Entry"])
def get_expense_opening(on: date):
    return {"date": on.isoformat(), "amount": store.EXP_OPENING.get(on.isoformat(), 0.0)}


@router.post("/expense/opening-balance", tags=["Expense Entry"], dependencies=[Depends(require_right("add", "Expense Entry"))])
def save_expense_opening(body: ExpenseOpeningIn):
    """The cash in hand at the start of a day. Like the desktop, a date later than tomorrow is refused; saving again replaces it."""
    if body.date > date.today() + timedelta(days=1):
        raise HTTPException(422, "Date cannot be greater than tomorrow!")
    gsettings.check_not_frozen(body.date, "This opening balance")
    if db.configured():   # TrnExpOPBal, the desktop's own table
        return {"date": body.date.isoformat(), "amount": expense_db.save_opening(body.date, body.amount)}
    old = store.EXP_OPENING.get(body.date.isoformat())
    admin_audit.record("Expense Entry", "Edited" if old is not None else "Added", body.date.isoformat(),
                       f"opening {old}" if old is not None else "", f"opening {body.amount}")
    store.EXP_OPENING[body.date.isoformat()] = round(body.amount, 2)
    return {"date": body.date.isoformat(), "amount": round(body.amount, 2)}


for _kind, _tag in [("purchase_indent", "Purchase Indent"), ("purchase_bill", "Purchase Bill"),
                    ("sales_invoice", "Sales Invoice"), ("sales_return", "Sales Return"), ("expense", "Expense Entry"),
                    ("stock_deduction", "Stock Deduction"), ("closing_stock", "Closing Stock Entry")]:
    _listing(_kind, _tag)
