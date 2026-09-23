"""Transaction screens (legacy frmTrnVch engine). Each posts a document and,
where the legacy transaction 'affects stock', moves item QOH."""
from datetime import date, datetime

from fastapi import APIRouter, HTTPException

from .. import store
from ..schemas import (
    CancelIn, ClosingStockIn, DenominationIn, ExpenseIn, GenerateInvoiceIn, Line, PurchaseBillIn, PurchaseIndentIn, SalesInvoiceIn,
    SalesReturnIn, StockDeductionIn,
)

router = APIRouter(prefix="/transactions")


def _item(code: str) -> dict:
    if code not in store.ITEMS:
        raise HTTPException(422, f"Unknown item '{code}'")
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


def _get(kind: str, doc_no: str) -> dict:
    doc = store.DOCS[kind].get(doc_no)
    if not doc:
        raise HTTPException(404, f"Document '{doc_no}' not found")
    return doc


def _listing(kind: str, tag: str):
    @router.get(f"/{kind.replace('_', '-')}", tags=[tag], name=f"List {tag}")
    def list_docs(from_date: date | None = None, to_date: date | None = None, status: str | None = None):
        return [d for d in store.DOCS[kind].values()
                if (not from_date or d["date"] >= from_date.isoformat()) and (not to_date or d["date"] <= to_date.isoformat())
                and (not status or d.get("status") == status)]

    @router.get(f"/{kind.replace('_', '-')}/{{doc_no:path}}", tags=[tag], name=f"Get {tag}")
    def get_doc(doc_no: str):
        return _get(kind, doc_no)


# ---------- Purchase Indent (does not affect stock) ----------
@router.post("/purchase-indent", status_code=201, tags=["Purchase Indent"])
def create_indent(body: PurchaseIndentIn):
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    doc_no = store.next_doc_no("purchase_indent", "PI")
    store.DOCS["purchase_indent"][doc_no] = {"doc_no": doc_no, "date": body.date.isoformat(), "supplier": body.supplier,
                                             "lines": lines, "totals": tot, "remarks": body.remarks}
    return store.DOCS["purchase_indent"][doc_no]


# ---------- Purchase Bill (adds stock) ----------
@router.post("/purchase-bill", status_code=201, tags=["Purchase Bill"])
def create_purchase_bill(body: PurchaseBillIn):
    src = body.lines
    if not src and body.copy_from_indent:
        src = [Line(item_code=l["item_code"], cases=l["cases"], bottles=l["bottles"], rate=l["rate"])
               for l in _get("purchase_indent", body.copy_from_indent)["lines"]]
    if not src:
        raise HTTPException(422, "Add lines or pass copy_from_indent")
    sup = store.find(store.SUPPLIERS, body.supplier)
    if not sup:
        raise HTTPException(422, f"Unknown supplier '{body.supplier}'")
    body.supplier = sup["code"]
    lines, tot = _price_lines(src, "purchase_rate", with_gst=True)
    tot["tcs"], tot["cess"] = body.tcs, body.cess
    tot["net"] = round(tot["net"] + body.tcs + body.cess, 2)
    tot["balance_vs_supplier_value"] = round(body.supplier_inv_value - tot["net"], 2)
    doc_no = store.next_doc_no("purchase_bill", "PB")
    sign = -1 if body.purchase_type == "Purchase Return" else 1
    for l in lines:
        store.move_stock(l["item_code"], sign * l["qty"], "purchase", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["purchase_bill"][doc_no] = doc
    return doc


# ---------- Sales Invoice (reduces stock) ----------
@router.post("/sales-invoice", status_code=201, tags=["Sales Invoice"])
def create_sales_invoice(body: SalesInvoiceIn):
    counter = store.COUNTERS.get(body.counter)
    if not counter or counter["inactive"]:
        raise HTTPException(422, "A sales invoice needs an active Sales Counter")
    if store.SETTINGS["invoice"]["salesperson_required"] and not body.salesperson:
        raise HTTPException(422, "Salesperson is required (Global Settings)")
    cust = store.find(store.CUSTOMERS, body.customer)
    if not cust:
        raise HTTPException(422, f"Unknown customer '{body.customer}'")
    body.customer = cust["code"]
    lines, tot = _price_lines(body.lines, "mrp", with_gst=True)
    for l in lines:
        if not body.hold and store.ITEMS[l["item_code"]]["qoh"] < l["qty"]:
            raise HTTPException(422, f"Insufficient stock for {l['item_code']} (QOH {store.ITEMS[l['item_code']]['qoh']})")
    if body.redeem_points > cust["points"]:
        raise HTTPException(422, f"Customer has only {cust['points']} points")
    tot["discount"] = body.discount
    tot["points_redeemed"] = body.redeem_points
    tot["net"] = round(tot["net"] - body.discount - body.redeem_points)  # bills are rounded to the rupee
    if body.collected is not None:
        diff = round(body.collected - tot["net"], 2)
        tot["collected"], tot["payback"], tot["shortage"] = body.collected, max(diff, 0), max(-diff, 0)
    status = "HOLD" if body.hold else "SAVED"
    doc_no = f"HOLD-{next(store._seq['hold']):04d}" if body.hold else store.next_doc_no("sales_invoice", counter["prefix"])
    points_earned = 0.0
    if not body.hold:
        for l in lines:
            store.move_stock(l["item_code"], -l["qty"], "sale", doc_no, body.date, l["rate"])
        loy = store.SETTINGS["loyalty"]
        if body.customer != "CASH" and tot["net"] > loy["min_bill_amount"]:
            points_earned = round(tot["net"] * loy["points_per_rupee"], 2)
        cust["points"] = round(cust["points"] - body.redeem_points + points_earned, 2)
    tot["points_earned"] = points_earned
    payment = body.payment.model_dump() if body.payment else {
        "cash": 0, "card": 0, "upi": 0, "credit": 0,
        {"CASH": "cash", "CARD": "card", "UPI": "upi", "CREDIT": "credit", "SPLIT": "cash"}[body.payment_type]: tot["net"],
    }
    doc = body.model_dump(mode="json", exclude={"lines", "payment"}) | {
        "doc_no": doc_no, "status": status, "lines": lines, "totals": tot, "payment": payment,
        "time": datetime.now().strftime("%H:%M:%S"),
    }
    store.DOCS["sales_invoice"][doc_no] = doc
    return doc


@router.post("/sales-invoice/{doc_no:path}/release", tags=["Sales Invoice"])
def release_held_bill(doc_no: str):
    """Recall a held bill and post it (moves stock)."""
    doc = _get("sales_invoice", doc_no)
    if doc["status"] != "HOLD":
        raise HTTPException(409, "Only held bills can be released")
    # Releasing turns the parked bill into a real invoice with the next number in its counter's series.
    new_no = store.next_doc_no("sales_invoice", store.COUNTERS[doc["counter"]]["prefix"])
    for l in doc["lines"]:
        store.move_stock(l["item_code"], -l["qty"], "sale", new_no, date.fromisoformat(doc["date"]), l["rate"])
    del store.DOCS["sales_invoice"][doc_no]
    doc.update(doc_no=new_no, status="SAVED")
    store.DOCS["sales_invoice"][new_no] = doc
    return doc


@router.delete("/sales-invoice/{doc_no:path}", status_code=204, tags=["Sales Invoice"])
def discard_held_bill(doc_no: str):
    """Remove a held bill once it has been recalled onto the counter (held bills never moved stock)."""
    if _get("sales_invoice", doc_no)["status"] != "HOLD":
        raise HTTPException(409, "Only held bills can be discarded; cancel saved bills instead")
    del store.DOCS["sales_invoice"][doc_no]


@router.post("/sales-invoice/{doc_no:path}/cancel", tags=["Sales Invoice"])
def cancel_invoice(doc_no: str, body: CancelIn):
    """Cancel a saved bill with a mandatory remark and put its stock back. Cancelled bills show in the DSR."""
    doc = _get("sales_invoice", doc_no)
    if doc["status"] == "CANCELLED":
        raise HTTPException(409, "Already cancelled")
    if doc["status"] == "SAVED":
        for l in doc["lines"]:
            store.move_stock(l["item_code"], l["qty"], "sale-cancel", doc_no, date.fromisoformat(doc["date"]), l["rate"])
        # Give back redeemed points and take back the ones this bill earned.
        cust = store.CUSTOMERS.get(doc["customer"])
        if cust:
            cust["points"] = round(cust["points"] + doc["totals"]["points_redeemed"] - doc["totals"]["points_earned"], 2)
    doc["status"] = "CANCELLED"
    doc["cancel_remarks"] = body.remarks
    return doc


@router.get("/sales-invoice/{doc_no:path}/reprint", tags=["Sales Invoice"])
def reprint_invoice(doc_no: str):
    """Re-Print Bill: returns the print payload (Crystal fmtSInv equivalent)."""
    doc = _get("sales_invoice", doc_no)
    return {"company": store.COMPANY, "invoice": doc, "copy": "DUPLICATE"}


# ---------- Sales Return (adds stock) ----------
@router.post("/sales-return", status_code=201, tags=["Sales Return"])
def create_sales_return(body: SalesReturnIn):
    if body.against_invoice:
        _get("sales_invoice", body.against_invoice)
    lines, tot = _price_lines(body.lines, "mrp", with_gst=True)
    doc_no = store.next_doc_no("sales_return", "SR")
    for l in lines:
        store.move_stock(l["item_code"], l["qty"], "sales-return", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["sales_return"][doc_no] = doc
    return doc


# ---------- Expense Entry ----------
@router.post("/expense", status_code=201, tags=["Expense Entry"])
def create_expense(body: ExpenseIn):
    for l in body.lines:
        if l.ledger not in store.LEDGERS:
            raise HTTPException(422, f"Unknown ledger '{l.ledger}'")
    doc_no = store.next_doc_no("expense", "EXP")
    doc = body.model_dump(mode="json") | {"doc_no": doc_no, "total": round(sum(l.amount for l in body.lines), 2)}
    store.DOCS["expense"][doc_no] = doc
    return doc


# ---------- Stock Deduction (reduces stock) ----------
@router.post("/stock-deduction", status_code=201, tags=["Stock Deduction"])
def create_stock_deduction(body: StockDeductionIn):
    lines, tot = _price_lines(body.lines, "purchase_rate", with_gst=False)
    doc_no = store.next_doc_no("stock_deduction", "SD")
    for l in lines:
        store.move_stock(l["item_code"], -l["qty"], "deduction", doc_no, body.date, l["rate"])
    doc = body.model_dump(mode="json", exclude={"lines"}) | {"doc_no": doc_no, "lines": lines, "totals": tot}
    store.DOCS["stock_deduction"][doc_no] = doc
    return doc


# ---------- Closing Stock Entry ----------
@router.get("/closing-stock/load", tags=["Closing Stock Entry"])
def load_closing_stock(as_on: date):
    """Pick As-on Date → liquor items with their opening balance (current QOH)."""
    return [{"item_code": i["code"], "item_name": i["name"], "package": i["package"], "op_qty": i["qoh"], "mrp": i["mrp"]}
            for i in store.ITEMS.values() if store.ITEM_GROUPS[i["group"]]["liquor"] and i["active"]]


@router.post("/closing-stock", status_code=201, tags=["Closing Stock Entry"])
def create_closing_stock(body: ClosingStockIn):
    """Physical closing stock per item; Sales Qty = OP − Closing."""
    lines = []
    for l in body.lines:
        it = _item(l.item_code)
        sales = it["qoh"] - l.closing_qty
        if sales < 0:
            raise HTTPException(422, f"Closing qty for {l.item_code} exceeds opening ({it['qoh']})")
        lines.append({"item_code": it["code"], "op_qty": it["qoh"], "closing_qty": l.closing_qty,
                      "sales_qty": sales, "mrp": it["mrp"], "amount": round(sales * it["mrp"], 2)})
    doc_no = store.next_doc_no("closing_stock", "CSE")
    doc = {"doc_no": doc_no, "date": body.as_on.isoformat(), "lines": lines, "invoice_no": None,
           "total_amount": round(sum(l["amount"] for l in lines), 2)}
    store.DOCS["closing_stock"][doc_no] = doc
    return doc


@router.post("/closing-stock/{doc_no:path}/generate-invoice", status_code=201, tags=["Closing Stock Entry"])
def generate_invoice_from_cse(doc_no: str, body: GenerateInvoiceIn):
    """Generate Sales Invoice from the computed sales quantities."""
    cse = _get("closing_stock", doc_no)
    if cse["invoice_no"]:
        raise HTTPException(409, f"Invoice {cse['invoice_no']} already generated")
    lines = [Line(item_code=l["item_code"], bottles=int(l["sales_qty"])) for l in cse["lines"] if l["sales_qty"] > 0]
    if not lines:
        raise HTTPException(422, "No sales quantity to invoice")
    inv = create_sales_invoice(SalesInvoiceIn(date=date.fromisoformat(cse["date"]), counter=body.counter,
                                              salesperson=body.salesperson, lines=lines))
    cse["invoice_no"] = inv["doc_no"]
    return inv


# ---------- Cash Denomination (day-end cash count) ----------
@router.get("/denomination", tags=["Cash Denomination"])
def get_denomination(on: date):
    return store.DENOMINATIONS.get(on.isoformat())


@router.post("/denomination", status_code=201, tags=["Cash Denomination"])
def save_denomination(body: DenominationIn):
    total = 0.0
    for key, n in body.counts.items():
        if n < 0:
            raise HTTPException(422, "Counts cannot be negative")
        # "coins" is entered as a rupee amount rather than a count.
        total += n if key == "coins" else float(key) * n
    doc = {"date": body.date.isoformat(), "counts": body.counts, "total": round(total, 2)}
    store.DENOMINATIONS[doc["date"]] = doc
    return doc


for _kind, _tag in [("purchase_indent", "Purchase Indent"), ("purchase_bill", "Purchase Bill"),
                    ("sales_invoice", "Sales Invoice"), ("sales_return", "Sales Return"), ("expense", "Expense Entry"),
                    ("stock_deduction", "Stock Deduction"), ("closing_stock", "Closing Stock Entry")]:
    _listing(_kind, _tag)
