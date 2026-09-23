"""Customised Reports Explorer: the legacy report list, built from the in-memory documents.
Each report returns {columns, rows} so the React side can render and export any of them the same way."""
from collections import defaultdict
from contextvars import ContextVar
from datetime import date

from fastapi import APIRouter, HTTPException, Query

from .. import store

router = APIRouter(prefix="/reports/custom", tags=["Customised Reports"])

L, R = "left", "right"

# Filter On values for the current request (ledger / supplier / group codes). A ContextVar keeps the
# report functions' (from, to) signature while still letting _docs narrow every report the same way.
_filters: ContextVar[dict[str, set[str]]] = ContextVar("filters", default={})


def _col(key: str, label: str, align: str = L) -> dict:
    return {"key": key, "label": label, "align": align}


def _narrow(kind: str, d: dict) -> dict | None:
    """Apply the Filter On values to one document: drop it, or return a copy holding only matching lines."""
    flt = _filters.get()
    if kind == "purchase_bill" and flt.get("supplier") and d["supplier"] not in flt["supplier"]:
        return None
    if kind == "expense" and flt.get("ledger"):
        lines = [l for l in d["lines"] if l["ledger"] in flt["ledger"]]
        return {**d, "lines": lines, "total": round(sum(l["amount"] for l in lines), 2)} if lines else None
    if kind != "expense" and flt.get("group"):
        lines = [l for l in d["lines"] if _item(l["item_code"]).get("group") in flt["group"]]
        if not lines:
            return None
        if len(lines) < len(d["lines"]):
            # Bill-level totals must match the kept lines; bill discounts and TCS can't be split per group.
            net = round(sum(l["amount"] + l["cgst"] + l["sgst"] for l in lines), 2)
            tot = {**d["totals"], "qty": sum(l["qty"] for l in lines),
                   "litres": round(sum(l["litres"] for l in lines), 3), "net": net}
            return {**d, "lines": lines, "totals": tot}
    return d


def _docs(kind: str, f: date, t: date, statuses: tuple[str, ...] | None = None) -> list[dict]:
    """Documents in the period, oldest first. Sales default to posted bills only (no held or cancelled)."""
    if statuses is None and kind == "sales_invoice":
        statuses = ("SAVED",)
    picked = (_narrow(kind, d) for d in store.DOCS[kind].values()
              if f.isoformat() <= d["date"] <= t.isoformat() and (statuses is None or d.get("status") in statuses))
    return sorted((d for d in picked if d), key=lambda d: (d["date"], d["doc_no"]))


def _lines(kind: str, f: date, t: date) -> list[tuple[dict, dict]]:
    return [(d, ln) for d in _docs(kind, f, t) for ln in d["lines"]]


def _item(code: str) -> dict:
    return store.ITEMS.get(code, {})


def _group(pairs, key, sums=("qty", "litres", "amount")) -> list[dict]:
    """Roll (doc, line) pairs up by `key(doc, line)` -> dict of label columns, summing line fields."""
    out: dict[tuple, dict] = {}
    for d, ln in pairs:
        labels = key(d, ln)
        row = out.setdefault(tuple(labels.values()), {**labels, **{s: 0 for s in sums}})
        for s in sums:
            row[s] = round(row[s] + ln.get(s, 0), 3)
    return list(out.values())


QTY = [_col("qty", "Qty (Btl)", R), _col("litres", "Litres", R), _col("amount", "Amount", R)]


def _month(d: dict) -> str:
    return d["date"][:7]


# ---------- Expense ----------
def expense_ledger(f, t):
    rows = [{"date": d["date"], "doc_no": d["doc_no"], "ledger": store.LEDGERS.get(l["ledger"], {}).get("name", l["ledger"]),
             "narration": l.get("narration", ""), "paid_from": d.get("paid_from", ""), "amount": l["amount"]}
            for d in _docs("expense", f, t) for l in d["lines"]]
    return [_col("date", "Date"), _col("doc_no", "Doc No"), _col("ledger", "Ledger"), _col("narration", "Narration"),
            _col("paid_from", "Paid From"), _col("amount", "Amount", R)], rows


def expense_summary(f, t):
    tot = defaultdict(float)
    for d in _docs("expense", f, t):
        for l in d["lines"]:
            tot[store.LEDGERS.get(l["ledger"], {}).get("name", l["ledger"])] += l["amount"]
    return [_col("ledger", "Ledger"), _col("amount", "Amount", R)], [{"ledger": k, "amount": round(v, 2)} for k, v in tot.items()]


# ---------- Purchase ----------
def grn_wise(f, t):
    rows = [{"date": d["date"], "doc_no": d["doc_no"], "supplier": d["supplier"], "inv_no": d["supplier_inv_no"],
             "type": d["purchase_type"], "qty": d["totals"]["qty"], "litres": d["totals"]["litres"], "amount": d["totals"]["net"]}
            for d in _docs("purchase_bill", f, t)]
    return [_col("date", "Date"), _col("doc_no", "GRN No"), _col("supplier", "Supplier"), _col("inv_no", "Supplier Inv No"),
            _col("type", "Type"), *QTY[:2], _col("amount", "Net Amount", R)], rows


def purchase_day_group(f, t):
    rows = _group(_lines("purchase_bill", f, t), lambda d, l: {"date": d["date"], "group": _item(l["item_code"]).get("group", "")})
    return [_col("date", "Date"), _col("group", "Item Group"), *QTY], rows


def supplier_bill_wise(f, t):
    rows = [{"supplier": d["supplier"], "inv_no": d["supplier_inv_no"], "inv_date": d["supplier_inv_date"], "doc_no": d["doc_no"],
             "inv_value": d.get("supplier_inv_value", 0), "amount": d["totals"]["net"]} for d in _docs("purchase_bill", f, t)]
    rows.sort(key=lambda r: r["supplier"])
    return [_col("supplier", "Supplier"), _col("inv_no", "Inv No"), _col("inv_date", "Inv Date"), _col("doc_no", "GRN No"),
            _col("inv_value", "Inv Value", R), _col("amount", "Net Amount", R)], rows


def _item_cols(*first):
    return [*first, _col("item_code", "Item Code"), _col("item_name", "Item Name"), *QTY]


def _by_item(kind, extra=lambda d: {}):
    def run(f, t):
        rows = _group(_lines(kind, f, t), lambda d, l: {**extra(d), "item_code": l["item_code"], "item_name": l["item_name"]})
        firsts = [_col(k, {"date": "Date", "doc_no": "Doc No", "month": "Month"}[k]) for k in extra({"date": "", "doc_no": ""})]
        return _item_cols(*firsts), rows
    return run


def mrp_rates(f, t):
    rows = [{"item_code": i["code"], "item_name": i["name"], "group": i["group"], "package": i["package"],
             "purchase_rate": i["purchase_rate"], "mrp": i["mrp"]} for i in store.ITEMS.values()
            if not _filters.get().get("group") or i["group"] in _filters.get()["group"]]
    return [_col("item_code", "Item Code"), _col("item_name", "Item Name"), _col("group", "Group"), _col("package", "Package"),
            _col("purchase_rate", "Purchase Rate", R), _col("mrp", "MRP", R)], rows


def _month_summary(kind):
    def run(f, t):
        tot: dict[str, dict] = {}
        for d in _docs(kind, f, t):
            r = tot.setdefault(_month(d), {"month": _month(d), "bills": 0, "qty": 0, "litres": 0.0, "amount": 0.0})
            r["bills"] += 1
            r["qty"] += d["totals"]["qty"]
            r["litres"] = round(r["litres"] + d["totals"]["litres"], 3)
            r["amount"] = round(r["amount"] + d["totals"]["net"], 2)
        return [_col("month", "Month"), _col("bills", "Bills", R), *QTY], list(tot.values())
    return run


# ---------- Sales ----------
def _bill_row(d):
    return {"date": d["date"], "time": d.get("time", ""), "doc_no": d["doc_no"], "counter": d["counter"], "customer": d["customer"],
            "payment": d["payment_type"], "qty": d["totals"]["qty"], "litres": d["totals"]["litres"], "amount": d["totals"]["net"]}


BILL_COLS = [_col("date", "Date"), _col("time", "Time"), _col("doc_no", "Invoice No"), _col("counter", "Counter"),
             _col("customer", "Customer"), _col("payment", "Payment"), *QTY[:2], _col("amount", "Net Amount", R)]


def sales_bill_wise(f, t):
    return BILL_COLS, [_bill_row(d) for d in _docs("sales_invoice", f, t)]


def cancelled_invoices(f, t):
    rows = [_bill_row(d) | {"remarks": d.get("cancel_remarks", "")} for d in _docs("sales_invoice", f, t, ("CANCELLED",))]
    return [*BILL_COLS, _col("remarks", "Cancel Remarks")], rows


def _sales_by(label: str, key):
    """Bill-level roll-up (counter, day, month, salesperson, hour)."""
    def run(f, t):
        tot: dict[str, dict] = {}
        for d in _docs("sales_invoice", f, t):
            k = key(d) or "-"
            r = tot.setdefault(k, {"key": k, "bills": 0, "qty": 0, "litres": 0.0, "amount": 0.0})
            r["bills"] += 1
            r["qty"] += d["totals"]["qty"]
            r["litres"] = round(r["litres"] + d["totals"]["litres"], 3)
            r["amount"] = round(r["amount"] + d["totals"]["net"], 2)
        return [_col("key", label), _col("bills", "Bills", R), *QTY], sorted(tot.values(), key=lambda r: r["key"])
    return run


def sales_group_wise(f, t):
    rows = _group(_lines("sales_invoice", f, t), lambda d, l: {"group": _item(l["item_code"]).get("group", "")})
    return [_col("group", "Item Group"), *QTY], rows


def sales_package_wise(f, t):
    rows = _group(_lines("sales_invoice", f, t), lambda d, l: {"package": _item(l["item_code"]).get("package", ""),
                                                               "item_code": l["item_code"], "item_name": l["item_name"]})
    return [_col("package", "Package"), *_item_cols()], rows


def sales_other_payment(f, t):
    rows = [{"date": d["date"], "doc_no": d["doc_no"], **{k: d["payment"].get(k, 0) for k in ("cash", "card", "upi", "credit")},
             "amount": d["totals"]["net"]} for d in _docs("sales_invoice", f, t)]
    return [_col("date", "Date"), _col("doc_no", "Invoice No"), _col("cash", "Cash", R), _col("card", "Card", R),
            _col("upi", "UPI", R), _col("credit", "Credit", R), _col("amount", "Net Amount", R)], rows


def sales_return_day(f, t):
    tot: dict[str, dict] = {}
    for d in _docs("sales_return", f, t):
        r = tot.setdefault(d["date"], {"date": d["date"], "returns": 0, "qty": 0, "litres": 0.0, "amount": 0.0})
        r["returns"] += 1
        r["qty"] += d["totals"]["qty"]
        r["litres"] = round(r["litres"] + d["totals"]["litres"], 3)
        r["amount"] = round(r["amount"] + d["totals"]["net"], 2)
    return [_col("date", "Date"), _col("returns", "Returns", R), *QTY], list(tot.values())


def sales_return_list(f, t):
    rows = [{"date": d["date"], "doc_no": d["doc_no"], "against": d.get("against_invoice") or "", "customer": d["customer"],
             "qty": d["totals"]["qty"], "litres": d["totals"]["litres"], "amount": d["totals"]["net"]} for d in _docs("sales_return", f, t)]
    return [_col("date", "Date"), _col("doc_no", "Return No"), _col("against", "Against Invoice"), _col("customer", "Customer"),
            *QTY[:2], _col("amount", "Net Amount", R)], rows


REPORTS = {
    "Expense Ledger Report": expense_ledger,
    "Expense Report": expense_summary,
    "1.GRN Wise Purchase Report": grn_wise,
    "2.Day Wise Group Wise Purchase Report": purchase_day_group,
    "3.Supplier Bill Wise Purchase Report": supplier_bill_wise,
    "Item Wise Purchase Report-Day Wise": _by_item("purchase_bill", lambda d: {"date": d["date"]}),
    "Item Wise Purchase Report-Bill Wise": _by_item("purchase_bill", lambda d: {"doc_no": d["doc_no"]}),
    "Item Wise Purchase Report-Month Wise": _by_item("purchase_bill", lambda d: {"month": _month(d)}),
    "ItemWise Purchase Report": _by_item("purchase_bill"),
    "MRP RATES": mrp_rates,
    "Purchase Report- Item Wise Summary": _by_item("purchase_bill"),
    "Purchase Report-MonthWise Summary": _month_summary("purchase_bill"),
    "2.Sales-Item Wise Report": _by_item("sales_invoice"),
    "3.Item wise Sales Report - Bill Wise": _by_item("sales_invoice", lambda d: {"doc_no": d["doc_no"]}),
    "Cancelled Invoice report": cancelled_invoices,
    "Item wise Sales Report - Day Wise": _by_item("sales_invoice", lambda d: {"date": d["date"]}),
    "Item wise Sales Report - Month Wise": _by_item("sales_invoice", lambda d: {"month": _month(d)}),
    "Sales - Bill Wise Report": sales_bill_wise,
    "Sales - Counter Wise Report": _sales_by("Counter", lambda d: d["counter"]),
    "Sales - Day Wise Report": _sales_by("Date", lambda d: d["date"]),
    "Sales - Group Wise Report": sales_group_wise,
    "Sales - Hourly Report": _sales_by("Hour", lambda d: f"{d.get('time', '')[:2]}:00"),
    "Sales - Item Wise Report": _by_item("sales_invoice"),
    "Sales - Month Wise Report": _sales_by("Month", _month),
    "Sales - PackageItem Wise Report": sales_package_wise,
    "Sales - Sales Person Wise Report": _sales_by("Sales Person", lambda d: d.get("salesperson")),
    "Sales Other Payment": sales_other_payment,
    "Sales Return - Day Wise Report": sales_return_day,
    "Sales Return Report": sales_return_list,
}


@router.get("")
def run_report(name: str, from_date: date, to_date: date, ledger: list[str] = Query([]),
               supplier: list[str] = Query([]), group: list[str] = Query([])):
    if from_date > to_date:
        raise HTTPException(422, "From Date cannot be after To Date")
    fn = REPORTS.get(name)
    if not fn:
        raise HTTPException(404, f"'{name}' is not available yet")
    token = _filters.set({"ledger": set(ledger), "supplier": set(supplier), "group": set(group)})
    try:
        columns, rows = fn(from_date, to_date)
    finally:
        _filters.reset(token)
    return {"name": name, "from_date": from_date, "to_date": to_date, "columns": columns, "rows": rows}
