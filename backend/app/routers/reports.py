"""Reports: Stock Balance, Stock Ledger, Profit Margin and the Daily Summary
Report (field list from Docs/WebDash.xlsx)."""
from collections import defaultdict
from datetime import date

from fastapi import APIRouter, HTTPException

from .. import store

router = APIRouter(prefix="/reports", tags=["Reports"])


def _litres(code: str, qty: float) -> float:
    return round(qty * store.PACKAGES.get(store.ITEMS[code]["package"], {}).get("litres_per_bottle", 0), 3)


def _moves(from_date: date, to_date: date, code: str | None = None):
    return [m for m in store.MOVEMENTS if from_date.isoformat() <= m["date"] <= to_date.isoformat()
            and (code is None or m["item"] == code)]


@router.get("/stock-balance")
def stock_balance(from_date: date, to_date: date, group: str | None = None):
    """Opening / receipt / issue / closing per item, with litre qty and value at purchase rate."""
    rows = []
    for it in store.ITEMS.values():
        if group and it["group"] != group:
            continue
        after_from = sum(m["qty"] for m in store.MOVEMENTS if m["item"] == it["code"] and m["date"] >= from_date.isoformat())
        after_to = sum(m["qty"] for m in store.MOVEMENTS if m["item"] == it["code"] and m["date"] > to_date.isoformat())
        period = _moves(from_date, to_date, it["code"])
        rec = sum(m["qty"] for m in period if m["qty"] > 0)
        iss = -sum(m["qty"] for m in period if m["qty"] < 0)
        closing = it["qoh"] - after_to
        rows.append({"item_code": it["code"], "item_name": it["name"], "group": it["group"], "package": it["package"],
                     "op_qty": it["qoh"] - after_from, "rec_qty": rec, "iss_qty": iss, "cl_qty": closing,
                     "cl_litres": _litres(it["code"], closing), "cl_value": round(closing * it["purchase_rate"], 2)})
    return rows


@router.get("/stock-ledger")
def stock_ledger(item_code: str, from_date: date, to_date: date):
    if item_code not in store.ITEMS:
        raise HTTPException(404, "Item not found")
    bal = store.ITEMS[item_code]["qoh"] - sum(
        m["qty"] for m in store.MOVEMENTS if m["item"] == item_code and m["date"] >= from_date.isoformat())
    rows = [{"date": from_date.isoformat(), "particulars": "Opening", "in": 0, "out": 0, "balance": bal}]
    for m in _moves(from_date, to_date, item_code):
        bal += m["qty"]
        rows.append({"date": m["date"], "particulars": f"{m['kind']} {m['doc_no']}",
                     "in": max(m["qty"], 0), "out": max(-m["qty"], 0), "balance": bal})
    return rows


@router.get("/profit-margin")
def profit_margin(from_date: date, to_date: date):
    agg = defaultdict(lambda: {"qty": 0, "sales_value": 0.0})
    for m in _moves(from_date, to_date):
        if m["kind"] in ("sale", "sale-cancel"):
            agg[m["item"]]["qty"] -= m["qty"]
            agg[m["item"]]["sales_value"] -= m["qty"] * m["rate"]
    rows = []
    for code, a in agg.items():
        it = store.ITEMS[code]
        pv = a["qty"] * it["purchase_rate"]
        rows.append({"item_code": code, "item_name": it["name"], "sales_qty": a["qty"], "purchase_rate": it["purchase_rate"],
                     "purchase_value": round(pv, 2), "sales_value": round(a["sales_value"], 2),
                     "profit": round(a["sales_value"] - pv, 2)})
    return rows


@router.get("/reorder")
def reorder_list():
    """Items at or below their reorder level (ROL), with suggested reorder qty (ROQ)."""
    return [{"item_code": i["code"], "item_name": i["name"], "qoh": i["qoh"], "rol": i["rol"], "roq": i["roq"]}
            for i in store.ITEMS.values() if i["qoh"] <= i["rol"]]


@router.get("/dsr")
def daily_summary_report(on: date):
    d = on.isoformat()
    invs = [i for i in store.DOCS["sales_invoice"].values() if i["date"] == d]
    saved = [i for i in invs if i["status"] == "SAVED"]
    liquor_basic = non_liquor_basic = profit = 0.0
    by_group, by_counter, by_person = defaultdict(float), defaultdict(float), defaultdict(float)
    pay = defaultdict(float)
    for inv in saved:
        by_counter[inv["counter"]] += inv["totals"]["net"]
        by_person[inv.get("salesperson") or "-"] += inv["totals"]["net"]
        for k, v in inv["payment"].items():
            pay[k] += v
        for l in inv["lines"]:
            it = store.ITEMS[l["item_code"]]
            if store.ITEM_GROUPS[it["group"]]["liquor"]:
                liquor_basic += l["amount"]
            else:
                non_liquor_basic += l["amount"]
            by_group[it["group"]] += l["amount"]
            profit += l["amount"] - l["qty"] * it["purchase_rate"]
    pbs = [p for p in store.DOCS["purchase_bill"].values() if p["date"] == d]
    returns = [r for r in store.DOCS["sales_return"].values() if r["date"] == d]
    exps = [e for e in store.DOCS["expense"].values() if e["date"] == d]
    exp_by_ledger = defaultdict(float)
    for e in exps:
        for l in e["lines"]:
            exp_by_ledger[l["ledger"]] += l["amount"]
    bal = stock_balance(on, on)
    return {
        "date": d,
        "total_bills": len(saved),
        "sales_incl_gst": round(sum(i["totals"]["net"] for i in saved), 2),
        "liquor_basic": round(liquor_basic, 2),
        "non_liquor_basic": round(non_liquor_basic, 2),
        "profit_of_the_day": round(profit, 2),
        "opening_stock": {"qty": sum(r["op_qty"] for r in bal),
                          "value": round(sum(r["op_qty"] * store.ITEMS[r["item_code"]]["purchase_rate"] for r in bal), 2)},
        "closing_stock": {"qty": sum(r["cl_qty"] for r in bal), "value": round(sum(r["cl_value"] for r in bal), 2),
                          "litres": round(sum(r["cl_litres"] for r in bal), 3)},
        "payment_modes": {k: round(v, 2) for k, v in pay.items()},
        "purchase": {"basic": round(sum(p["totals"]["basic"] for p in pbs), 2),
                     "incl_gst": round(sum(p["totals"]["net"] for p in pbs), 2)},
        "sales_returns": round(sum(r["totals"]["net"] for r in returns), 2),
        "expenses": round(sum(e["total"] for e in exps), 2),
        "cash_balance": round(pay["cash"] - sum(e["total"] for e in exps if e["paid_from"] == "CASH"), 2),
        "redemption": round(sum(i["totals"].get("points_redeemed", 0) for i in saved), 2),
        "counter_wise": dict(by_counter),
        "salesperson_wise": dict(by_person),
        "group_wise_sales": {k: round(v, 2) for k, v in by_group.items()},
        "expense_ledger_wise": dict(exp_by_ledger),
        "cancelled_invoices": [i["doc_no"] for i in invs if i["status"] == "CANCELLED"],
    }


@router.get("/stock-ageing")
def stock_ageing(as_on: date, months: int = 6):
    """Closing stock bucketed by months since the item was last received (legacy frmRptStockAgeing)."""
    rows = []
    for it in store.ITEMS.values():
        last = date.fromisoformat(store.LAST_PURCHASE.get(it["code"], store.COMPANY["fy_start"]))
        age_days = (as_on - last).days
        bucket = min(max(age_days, 0) // 30, months)
        rows.append({"item_code": it["code"], "item_name": it["name"], "qoh": it["qoh"], "last_purchase": last.isoformat(),
                     "age_days": age_days, "bucket": f"{months}+ months" if bucket >= months else f"{bucket}-{bucket + 1} months",
                     "value": round(it["qoh"] * it["purchase_rate"], 2)})
    return sorted(rows, key=lambda r: -r["age_days"])


def _saved_invoices(from_date: date, to_date: date):
    return [i for i in store.DOCS["sales_invoice"].values()
            if i["status"] == "SAVED" and from_date.isoformat() <= i["date"] <= to_date.isoformat()]


@router.get("/sales/hourly")
def hourly_sales(on: date):
    hours = {h: {"hour": f"{h:02d}:00", "bills": 0, "qty": 0, "amount": 0.0} for h in range(24)}
    for inv in _saved_invoices(on, on):
        h = hours[int(inv.get("time", "00")[:2])]
        h["bills"] += 1
        h["qty"] += inv["totals"]["qty"]
        h["amount"] = round(h["amount"] + inv["totals"]["net"], 2)
    return [h for h in hours.values() if h["bills"]]


@router.get("/sales/daily")
def daily_sales(from_date: date, to_date: date):
    days = defaultdict(lambda: {"bills": 0, "qty": 0, "litres": 0.0, "amount": 0.0})
    for inv in _saved_invoices(from_date, to_date):
        d = days[inv["date"]]
        d["bills"] += 1
        d["qty"] += inv["totals"]["qty"]
        d["litres"] = round(d["litres"] + inv["totals"]["litres"], 3)
        d["amount"] = round(d["amount"] + inv["totals"]["net"], 2)
    return [{"date": k, **v} for k, v in sorted(days.items())]


@router.get("/sales/brand-wise")
def brand_wise_sales(from_date: date, to_date: date):
    brands = defaultdict(lambda: {"qty": 0, "litres": 0.0, "amount": 0.0})
    for inv in _saved_invoices(from_date, to_date):
        for l in inv["lines"]:
            b = brands[store.ITEMS[l["item_code"]]["brand"]]
            b["qty"] += l["qty"]
            b["litres"] = round(b["litres"] + l["litres"], 3)
            b["amount"] = round(b["amount"] + l["amount"], 2)
    return sorted(({"brand": k, **v} for k, v in brands.items()), key=lambda r: -r["amount"])


@router.get("/litres/day")
def litres_sold_per_day(from_date: date, to_date: date):
    """Litres sold per day split by LIQTYPE (legacy frmRptDayLtr)."""
    days = defaultdict(lambda: defaultdict(float))
    for inv in _saved_invoices(from_date, to_date):
        for l in inv["lines"]:
            days[inv["date"]][store.ITEMS[l["item_code"]]["liqtype"]] += l["litres"]
    return [{"date": d, **{k: round(v, 3) for k, v in t.items()}, "total": round(sum(t.values()), 3)}
            for d, t in sorted(days.items())]


@router.get("/litres/stock")
def stock_in_litres(on: date):
    """Closing stock in litres by item (legacy frmRptStkLtr)."""
    return [{"item_code": r["item_code"], "item_name": r["item_name"], "package": r["package"],
             "liqtype": store.ITEMS[r["item_code"]]["liqtype"], "cl_qty": r["cl_qty"], "cl_litres": r["cl_litres"]}
            for r in stock_balance(on, on)]
