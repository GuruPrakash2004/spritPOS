"""Transaction screens (legacy frmTrnVch engine). Each posts a document and,
where the legacy transaction 'affects stock', moves item QOH.

Every POST needs the matching module in the caller's role plus its Add tick, and cancelling a bill its
Delete tick (require_right). Anything that takes
stock out (sale, held-bill release, stock deduction, purchase return) first checks the total
quantity per item across all lines, so stock can't go below zero.

Documents are saved to SQL Server's Web_ tables when the request succeeds (app/sqlstore.py); sales invoices
and purchase bills also in the desktop's format. Held bills are saved whole but not posted until released."""
from collections import defaultdict
from datetime import date, datetime

from fastapi import APIRouter, Depends, HTTPException

from .. import admin_audit, doc_settings, gsettings, security, store
from ..schemas import (
    CancelIn, ClosingStockIn, DenominationIn, ExpenseIn, GenerateInvoiceIn, Line, PurchaseBillIn, PurchaseIndentIn, SalesInvoiceIn,
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


def _price_lines(lines: list[Line], rate_field: str, with_gst: bool) -> tuple[list[dict], dict]:
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
        tot["qty"] += qty
        tot["litres"] = round(tot["litres"] + litres, 3)
        tot["basic"] = round(tot["basic"] + amt, 2)
        tot["cgst"] = round(tot["cgst"] + cgst, 2)
        tot["sgst"] = round(tot["sgst"] + sgst, 2)
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
    gsettings.check_not_frozen(body.date, "This purchase indent")
    doc_settings.check_lines("purchase_indent", body.lines, body.date)
    sup = store.find(store.SUPPLIERS, body.supplier)
    if not sup:
        raise HTTPException(422, f"Unknown supplier '{body.supplier}'")
    security.check_authorised("suppliers", sup["code"], "Supplier")
    body.supplier = sup["code"]
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    doc_no = store.next_doc_no("purchase_indent", doc_settings.prefix("purchase_indent"))
    store.DOCS["purchase_indent"][doc_no] = {"doc_no": doc_no, "date": body.date.isoformat(), "supplier": body.supplier,
                                             "lines": lines, "totals": tot, "remarks": body.remarks}
    admin_audit.record("Purchase Indent", "Added", doc_no, "", f"supplier {body.supplier}",
                       admin_audit.doc_items(store.DOCS["purchase_indent"][doc_no], "Added"))
    return store.DOCS["purchase_indent"][doc_no]


# ---------- Purchase Bill (adds stock) ----------
@router.post("/purchase-bill", status_code=201, tags=["Purchase Bill"], dependencies=[Depends(require_right("add", "Purchase Bill"))])
def create_purchase_bill(body: PurchaseBillIn):
    gsettings.check_not_frozen(body.date, "This purchase bill")
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
    lines, tot = _price_lines(src, "purchase_rate", with_gst=True)
    tot["tcs"], tot["cess"] = body.tcs, body.cess
    tot["net"] = round(tot["net"] + body.tcs + body.cess, 2)
    tot["balance_vs_supplier_value"] = round(body.supplier_inv_value - tot["net"], 2)
    doc_no = store.next_doc_no("purchase_bill", doc_settings.prefix("purchase_bill"))
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
    admin_audit.record("Purchase Bill", "Added", doc_no, "", f"{body.purchase_type}; supplier {sup['code']}; net {tot['net']}",
                       admin_audit.doc_items(doc, "Added"))
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
    gsettings.check_not_frozen(body.date, "This bill")
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
    lines, tot = _price_lines(body.lines, "mrp", with_gst=True)
    if not body.hold:
        _check_stock(lines)
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
    doc_no = f"HOLD-{next(store._seq['hold']):04d}" if body.hold else store.next_doc_no("sales_invoice", counter["prefix"])
    doc = body.model_dump(mode="json", exclude={"lines", "payment"}) | {
        "doc_no": doc_no, "status": "HOLD" if body.hold else "SAVED", "lines": lines, "totals": tot, "payment": payment,
        "time": datetime.now().strftime("%H:%M:%S"), "added_by": current_user["user"],
    }
    if not body.hold:
        _post_sale(doc, cust)
    store.DOCS["sales_invoice"][doc_no] = doc
    admin_audit.record("Sales Invoice", "Held" if body.hold else "Added", doc_no, "",
                       f"counter {doc['counter']}; customer {doc['customer']}; net {tot['net']}",
                       admin_audit.doc_items(doc, "Added"))
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


def _returned_qty(invoice_no: str) -> dict[str, float]:
    """Quantity per item already returned against an invoice."""
    done: dict[str, float] = defaultdict(float)
    for r in store.DOCS["sales_return"].values():
        if r.get("against_invoice") == invoice_no:
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
    gsettings.check_not_frozen(body.date, "This sales return")
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
        done = _returned_qty(body.against_invoice)
        asked: dict[str, float] = defaultdict(float)
        for l in lines:
            asked[l["item_code"]] += l["qty"]
        for code, qty in asked.items():
            left = sold.get(code, 0) - done.get(code, 0)
            if qty > left:
                raise HTTPException(422, f"Only {left:g} of {code} can be returned against {body.against_invoice}")
        body.customer = inv["customer"]
    doc_no = store.next_doc_no("sales_return", doc_settings.prefix("sales_return"))
    for l in lines:
        store.move_stock(l["item_code"], l["qty"], "sales-return", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["sales_return"][doc_no] = doc
    admin_audit.record("Sales Return", "Added", doc_no, "",
                       f"against {body.against_invoice}" if body.against_invoice else "without an invoice",
                       admin_audit.doc_items(doc, "Added"))
    return doc


# ---------- Expense Entry ----------
@router.post("/expense", status_code=201, tags=["Expense Entry"], dependencies=[Depends(require_right("add", "Expense Entry"))])
def create_expense(body: ExpenseIn):
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


# ---------- Stock Deduction (reduces stock) ----------
@router.post("/stock-deduction", status_code=201, tags=["Stock Deduction"], dependencies=[Depends(require_right("add", "Stock Deduction"))])
def create_stock_deduction(body: StockDeductionIn):
    gsettings.check_not_frozen(body.date, "This stock deduction")
    doc_settings.check_lines("stock_deduction", body.lines, body.date)
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    _check_stock(lines)
    doc_no = store.next_doc_no("stock_deduction", doc_settings.prefix("stock_deduction"))
    for l in lines:
        store.move_stock(l["item_code"], -l["qty"], "deduction", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["stock_deduction"][doc_no] = doc
    admin_audit.record("Stock Deduction", "Added", doc_no, "", "", admin_audit.doc_items(doc, "Added"))
    return doc


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


for _kind, _tag in [("purchase_indent", "Purchase Indent"), ("purchase_bill", "Purchase Bill"),
                    ("sales_invoice", "Sales Invoice"), ("sales_return", "Sales Return"), ("expense", "Expense Entry"),
                    ("stock_deduction", "Stock Deduction"), ("closing_stock", "Closing Stock Entry")]:
    _listing(_kind, _tag)
