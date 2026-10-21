"""Report endpoints under /reports.

Stock Balance, Stock Ledger, Stock Ageing, Profit Margin, Reorder (Stock Level), the Daily
Summary Report (field list from Docs/WebDash.xlsx), hourly/daily/brand-wise sales, litre
reports, the Dashboard's stock-movement summary and restock priority list (sales velocity from
saved invoices), and the list of legacy .rpt files in
backend/Rpts/. Stock figures are computed from store.MOVEMENTS, so they only include
movements made through store.move_stock().

The frontend renders most of these through REPORTS in
frontend/src/components/reports/reportDefs.js.
"""
import math
from collections import defaultdict
from datetime import date, timedelta
from pathlib import Path

from fastapi import APIRouter, Depends, HTTPException, Query

from .. import db_store, security, sqlstore, store
from ..ai import service as ai_service
from .auth import get_current_user, hides_cost, require_module

router = APIRouter(prefix="/reports", tags=["Reports"])
RPTS_DIR = Path(__file__).resolve().parent.parent.parent / "Rpts"


MAX_RANGE_DAYS = 3660   # ten years; longer ranges are refused so one request can't tie the server up


def _item(code: str) -> dict:
    """The item, or a placeholder when a bill refers to an item that is no longer in Item Master."""
    return store.ITEMS.get(code) or {"code": code, "name": code, "group": "", "brand": "", "package": "",
                                     "liqtype": "", "purchase_rate": 0, "qoh": 0}


def _is_liquor(item: dict) -> bool:
    return store.ITEM_GROUPS.get(item.get("group"), {}).get("liquor", True)


def _check_range(from_date: date, to_date: date) -> None:
    if to_date < from_date:
        raise HTTPException(422, "The To date is before the From date")
    if (to_date - from_date).days > MAX_RANGE_DAYS:
        raise HTTPException(422, "Choose a period of ten years or less")


def _litres(code: str, qty: float) -> float:
    return round(qty * store.PACKAGES.get(_item(code)["package"], {}).get("litres_per_bottle", 0), 3)


def _moves(from_date: date, to_date: date, code: str | None = None):
    return [m for m in store.MOVEMENTS if from_date.isoformat() <= m["date"] <= to_date.isoformat()
            and (code is None or m["item"] == code)]


@router.get("/stock-balance")
def stock_balance(from_date: date, to_date: date, group: str | None = None, current_user: dict = Depends(get_current_user)):
    """Opening / receipt / issue / closing per item, with litre qty and value at purchase rate."""
    _check_range(from_date, to_date)
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
    if hides_cost(current_user):
        for r in rows:
            r["cl_value"] = None
    return rows


@router.get("/stock-ledger")
def stock_ledger(item_code: str, from_date: date, to_date: date):
    if item_code not in store.ITEMS:
        raise HTTPException(404, "Item not found")
    _check_range(from_date, to_date)
    bal = store.ITEMS[item_code]["qoh"] - sum(
        m["qty"] for m in store.MOVEMENTS if m["item"] == item_code and m["date"] >= from_date.isoformat())
    rows = [{"date": from_date.isoformat(), "particulars": "Opening", "in": 0, "out": 0, "balance": bal}]
    for m in _moves(from_date, to_date, item_code):
        bal += m["qty"]
        rows.append({"date": m["date"], "particulars": f"{m['kind']} {m['doc_no']}",
                     "in": max(m["qty"], 0), "out": max(-m["qty"], 0), "balance": bal})
    return rows


@router.get("/profit-margin", dependencies=[Depends(require_module("Profit Margin"))])
def profit_margin(from_date: date, to_date: date):
    _check_range(from_date, to_date)
    agg = defaultdict(lambda: {"qty": 0, "sales_value": 0.0})
    for m in _moves(from_date, to_date):
        if m["kind"] in ("sale", "sale-cancel"):
            agg[m["item"]]["qty"] -= m["qty"]
            agg[m["item"]]["sales_value"] -= m["qty"] * m["rate"]
    rows = []
    for code, a in agg.items():
        it = _item(code)
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


# Restock analysis thresholds (days of stock left at the current sales rate).
RESTOCK_CRITICAL_DAYS = 3    # runs out within 3 days -> CRITICAL
RESTOCK_LOW_DAYS = 7         # runs out within a week (or qoh <= ROL) -> LOW
RESTOCK_MAX_DAYS_LEFT = 365  # a longer cover means "no real demand" (a shop with no recent billing); shown as 365+
RESTOCK_COVER_DAYS = 14      # suggested order covers two weeks of sales
_STATUS_ORDER = {"OUT_OF_STOCK": 0, "CRITICAL": 1, "LOW": 2, "OK": 3}


def _dashboard_panel(user: dict, panel: str) -> None:
    if security.dashboard_hidden(user, panel):
        raise HTTPException(403, "This Dashboard panel is hidden for your role (Admin Master → Field Level Rights)")


@router.get("/restock")
def restock(days: int = Query(15, ge=1, le=365), limit: int | None = Query(None, ge=1),
            current_user: dict = Depends(get_current_user)):
    """Items to restock, fast sellers that are running out first (feeds the Dashboard panel; 403 for a role whose
    Field Level Rights hide that panel).

    Real time: the window is always the last `days` days up to today.
    - With SQL Server connected, sales and stock are read live from the DB (db_store.live_sales_and_stock,
      cached 20 s) and bills made in this app since startup (store.MOVEMENTS after DB_BASE_MOVEMENTS)
      are added on top, since the app's own writes don't reach the DB.
    - Without a DB, the in-memory bills (DOCS) and stock (ITEMS qoh) are used.
    Held and cancelled bills never count. priority = avg_daily / (days_left + 1): it grows with how fast
    an item sells and how close it is to running out, and is 0 for items that didn't sell. Only items
    that sell can need restock; those come first (out of stock, CRITICAL or LOW).
    """
    _dashboard_panel(current_user, "restock")
    end = date.today()
    start = end - timedelta(days=max(days, 1) - 1)
    sold: dict[str, float] = defaultdict(float)
    last_sold: dict[str, str] = {}
    qoh_of: dict[str, float] = {code: it["qoh"] for code, it in store.ITEMS.items()}
    first_sale: str | None = None

    live = db_store.live_sales_and_stock(start, end)
    if live is not None:
        source = "live"
        sold.update(live["sold"])
        last_sold.update(live["last_sold"])
        first_sale = live["first_sale"]
        qoh_of.update(live["qoh"])
        # Add this app's own activity since the DB snapshot (bills, returns, purchases made here).
        for m in store.MOVEMENTS[store.DB_BASE_MOVEMENTS:]:
            qoh_of[m["item"]] = qoh_of.get(m["item"], 0) + m["qty"]
            if m["kind"] in ("sale", "sale-cancel") and start.isoformat() <= m["date"] <= end.isoformat():
                sold[m["item"]] -= m["qty"]  # a sale is a negative movement; a cancel gives it back
                if m["kind"] == "sale":
                    last_sold[m["item"]] = max(last_sold.get(m["item"], ""), m["date"])
                    first_sale = min(first_sale or m["date"], m["date"])
    else:
        source = "memory"
        for inv in _saved_invoices(start, end):
            for line in inv["lines"]:
                code = line["item_code"]
                sold[code] += line["qty"]
                last_sold[code] = max(last_sold.get(code, ""), inv["date"])
            first_sale = min(first_sale or inv["date"], inv["date"])

    # A shop that started billing less than `days` days ago: divide by the days it has actually been
    # selling, so the daily rate isn't underestimated.
    window_days = max(1, min(max(days, 1), (end - date.fromisoformat(first_sale)).days + 1)) if first_sale else max(days, 1)

    # AI forecast (AI_FORECAST_ENABLED): items the model could forecast use its demand, days left, status and order quantity;
    # the rest, and every item while the forecasts are not ready, keep the formula below.
    ai_recs = ai_service.restock_overlay(qoh_of)

    rows = []
    for it in store.ITEMS.values():
        if it.get("active") is False:
            continue
        qoh, rol, roq = qoh_of.get(it["code"], it["qoh"]), it.get("rol", 0), it.get("roq", 0)
        ai = (ai_recs or {}).get(it["code"])
        avg_daily = ai["avg_daily"] if ai else round(sold.get(it["code"], 0) / window_days, 2)
        if qoh <= 0:
            days_left = 0.0
        elif ai:
            days_left = None if ai["days_remaining"] is None else min(ai["days_remaining"], RESTOCK_MAX_DAYS_LEFT)
        elif avg_daily > 0:
            days_left = min(round(qoh / avg_daily, 1), RESTOCK_MAX_DAYS_LEFT)
        else:
            days_left = None

        if qoh <= 0:
            status = "OUT_OF_STOCK"
        elif ai:
            status = {"Reorder now": "CRITICAL", "Reorder soon": "LOW"}.get(ai["status"], "OK")
        elif days_left is not None and days_left <= RESTOCK_CRITICAL_DAYS:
            status = "CRITICAL"
        elif qoh <= rol or (days_left is not None and days_left <= RESTOCK_LOW_DAYS):
            status = "LOW"
        else:
            status = "OK"

        priority = round(avg_daily / (max(days_left, 0) + 1), 3) if avg_daily > 0 and days_left is not None else 0.0
        rows.append({
            "item_code": it["code"], "item_name": it["name"], "group": it.get("group", ""), "brand": it.get("brand", ""),
            "mrp": it.get("mrp", 0), "qoh": qoh, "rol": rol, "roq": roq,
            "sold_qty": sold.get(it["code"], 0), "avg_daily": avg_daily, "days_left": days_left,
            "last_sold": last_sold.get(it["code"]), "status": status,
            "needs_restock": status != "OK" and avg_daily > 0, "priority": priority,
            "sales_from": start.isoformat(), "sales_to": end.isoformat(), "window_days": window_days, "source": source,
            "suggested_qty": ai["suggested_bottles"] if ai and ai["suggested_bottles"] > 0
                             else max(roq, math.ceil(avg_daily * RESTOCK_COVER_DAYS - max(qoh, 0))),
            "forecast": "ai" if ai else "formula", "ai_reason": ai["reason"] if ai else None,
        })

    # How fast each item sells compared with the shop's other sellers (so it suits any shop): the top fifth of the items
    # that sold is "Fast", the next 30% "Medium", the rest "Slow". Priority says how soon to order.
    rates = sorted((r["avg_daily"] for r in rows if r["avg_daily"] > 0), reverse=True)
    fast_at = rates[max(int(len(rates) * 0.2) - 1, 0)] if rates else 0
    medium_at = rates[max(int(len(rates) * 0.5) - 1, 0)] if rates else 0
    for r in rows:
        a = r["avg_daily"]
        r["speed"] = None if a <= 0 else "Fast" if a >= fast_at else "Medium" if a >= medium_at else "Slow"
        r["priority_level"] = ("Urgent" if r["status"] in ("OUT_OF_STOCK", "CRITICAL") else "High" if r["status"] == "LOW"
                               else "Normal") if r["needs_restock"] else None

    # Items that need restock: by priority (fast sellers running out first). The rest (for filling the
    # Dashboard's top 10): fewest days of stock left first, items that didn't sell last.
    def sort_key(r):
        if r["needs_restock"]:
            return (0, -r["priority"], -r["avg_daily"], _STATUS_ORDER[r["status"]], r["qoh"])
        if r["avg_daily"] <= 0:
            return (2, 0, 0, 0, r["qoh"])  # never sold in the window: last, even when out of stock
        return (1, r["days_left"], -r["avg_daily"], 0, r["qoh"])
    rows.sort(key=sort_key)
    return rows[:limit] if limit else rows


@router.get("/dsr")
def daily_summary_report(on: date, current_user: dict = Depends(get_current_user)):
    d = on.isoformat()
    db_store.ensure_days(on, on)                   # a date older than the bills loaded at start-up is read from SQL Server once
    db_store.ensure_days(on, on, purchases=True)
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
            it = _item(l["item_code"])
            if _is_liquor(it):
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
    result = {
        "date": d,
        "total_bills": len(saved),
        "sales_incl_gst": round(sum(i["totals"]["net"] for i in saved), 2),
        "liquor_basic": round(liquor_basic, 2),
        "non_liquor_basic": round(non_liquor_basic, 2),
        "profit_of_the_day": round(profit, 2),
        "opening_stock": {"qty": sum(r["op_qty"] for r in bal),
                          "value": round(sum(r["op_qty"] * _item(r["item_code"])["purchase_rate"] for r in bal), 2)},
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
    if hides_cost(current_user):   # profit and stock/purchase value at cost
        result["profit_of_the_day"] = None
        result["opening_stock"]["value"] = None
        result["closing_stock"]["value"] = None
        result["purchase"] = {"basic": None, "incl_gst": None}
    return result


@router.get("/stock-ageing")
def stock_ageing(as_on: date, months: int = Query(6, ge=1, le=120), current_user: dict = Depends(get_current_user)):
    """Closing stock bucketed by months since the item was last received (legacy frmRptStockAgeing)."""
    rows = []
    for it in store.ITEMS.values():
        last = date.fromisoformat(store.LAST_PURCHASE.get(it["code"], store.COMPANY["fy_start"]))
        age_days = (as_on - last).days
        bucket = min(max(age_days, 0) // 30, months)
        rows.append({"item_code": it["code"], "item_name": it["name"], "qoh": it["qoh"], "last_purchase": last.isoformat(),
                     "age_days": age_days, "bucket": f"{months}+ months" if bucket >= months else f"{bucket}-{bucket + 1} months",
                     "value": None if hides_cost(current_user) else round(it["qoh"] * it["purchase_rate"], 2)})
    return sorted(rows, key=lambda r: -r["age_days"])


def _saved_invoices(from_date: date, to_date: date):
    db_store.ensure_days(from_date, to_date)
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
    _check_range(from_date, to_date)
    days = defaultdict(lambda: {"bills": 0, "qty": 0, "litres": 0.0, "amount": 0.0})
    for inv in _saved_invoices(from_date, to_date):
        d = days[inv["date"]]
        d["bills"] += 1
        d["qty"] += inv["totals"]["qty"]
        d["litres"] = round(d["litres"] + inv["totals"]["litres"], 3)
        d["amount"] = round(d["amount"] + inv["totals"]["net"], 2)
    cur = from_date
    result = []
    while cur <= to_date:
        ds = cur.isoformat()
        v = days[ds]
        result.append({
            "date": ds,
            "day": cur.strftime("%a"),
            "bills": v["bills"],
            "qty": v["qty"],
            "litres": v["litres"],
            "amount": v["amount"],
        })
        cur += timedelta(days=1)
    return result


@router.get("/stock-movement")
def stock_movement(on: date | None = None, current_user: dict = Depends(get_current_user)):
    """The Dashboard's Stock movement panel (403 for a role whose Field Level Rights hide it)."""
    _dashboard_panel(current_user, "stock_movement")
    target_date = on or date.today()
    bal = stock_balance(target_date, target_date)
    op = sum(r["op_qty"] for r in bal)
    rec = sum(r["rec_qty"] for r in bal)
    iss = sum(r["iss_qty"] for r in bal)
    cl = sum(r["cl_qty"] for r in bal)
    max_val = max(op, rec, iss, cl, 1)

    return {
        "date": target_date.isoformat(),
        "opening_stock": int(op),
        "received": int(rec),
        "issued": int(iss),
        "closing_stock": int(cl),
        "flow": [
            {"label": "Opening stock", "value": f"{int(op):,}", "pct": max(15, min(100, round((op / max_val) * 100))), "color": "bg-[#958B81] dark:bg-stone-400"},
            {"label": "Received", "value": f"+{int(rec):,}", "pct": max(15, min(100, round((rec / max_val) * 100))), "color": "bg-[#55745A]"},
            {"label": "Issued", "value": f"−{int(iss):,}", "pct": max(15, min(100, round((iss / max_val) * 100))), "color": "bg-[#64283A] dark:bg-accent"},
        ],
    }


@router.get("/sales/brand-wise")
def brand_wise_sales(from_date: date, to_date: date):
    _check_range(from_date, to_date)
    brands = defaultdict(lambda: {"qty": 0, "litres": 0.0, "amount": 0.0})
    for inv in _saved_invoices(from_date, to_date):
        for l in inv["lines"]:
            b = brands[_item(l["item_code"])["brand"]]
            b["qty"] += l["qty"]
            b["litres"] = round(b["litres"] + l["litres"], 3)
            b["amount"] = round(b["amount"] + l["amount"], 2)
    return sorted(({"brand": k, **v} for k, v in brands.items()), key=lambda r: -r["amount"])


@router.get("/litres/day")
def litres_sold_per_day(from_date: date, to_date: date):
    """Litres sold per day split by LIQTYPE (legacy frmRptDayLtr)."""
    _check_range(from_date, to_date)
    days = defaultdict(lambda: defaultdict(float))
    for inv in _saved_invoices(from_date, to_date):
        for l in inv["lines"]:
            days[inv["date"]][_item(l["item_code"])["liqtype"] or "Other"] += l["litres"]
    return [{"date": d, **{k: round(v, 3) for k, v in t.items()}, "total": round(sum(t.values()), 3)}
            for d, t in sorted(days.items())]


@router.get("/litres/stock")
def stock_in_litres(on: date):
    """Closing stock in litres by item (legacy frmRptStkLtr)."""
    return [{"item_code": r["item_code"], "item_name": r["item_name"], "package": r["package"],
             "liqtype": _item(r["item_code"])["liqtype"], "cl_qty": r["cl_qty"], "cl_litres": r["cl_litres"]}
            for r in stock_balance(on, on)]


@router.get("/rpt-files")
def list_rpt_files(category: str | None = None):
    """List available legacy Crystal Reports (.rpt) template files from backend/Rpts."""
    files = []
    if RPTS_DIR.exists():
        for p in sorted(RPTS_DIR.glob("*.rpt")):
            fname = p.name
            cat = "GENERIC_REPORT"
            printer = "LASER_A4_PORTRAIT"
            paper = "A4 Portrait"
            badge = "A4"
            desc = "Crystal Reports document template"

            if fname.startswith("fmtSPInvPrint") or fname in ("fmtSInv.rpt", "fmtSInv_GST.rpt", "fmtPSE.rpt", "fmtQTN.rpt", "fmtSR.rpt"):
                cat = "SALES_INVOICE"
                if "SPInvPrint" in fname:
                    printer = "THERMAL_80MM"
                    paper = "80mm Roll (Thermal POS)"
                    badge = "80mm"
                    desc = "Standard 80mm roll POS cash bill format"
                elif fname == "fmtSInv.rpt":
                    printer = "LASER_A5_LANDSCAPE"
                    paper = "A5 Landscape (148 x 210 mm)"
                    badge = "A5"
                    desc = "Half-page compact retail cash memo"
                elif fname == "fmtSInv_GST.rpt":
                    printer = "LASER_A4_PORTRAIT"
                    paper = "A4 Portrait (210 x 297 mm)"
                    badge = "A4 GST"
                    desc = "Full A4 GST tax invoice with statutory details"
                else:
                    printer = "THERMAL_80MM"
                    paper = "80mm Roll"
                    badge = "Thermal"
                    desc = "POS receipt format"
            elif fname.startswith("RptDSR") or fname == "RptDropsDSR.rpt":
                cat = "DSR"
                if fname == "RptDSR_S.rpt":
                    printer = "THERMAL_80MM"
                    paper = "80mm Roll"
                    badge = "Thermal"
                    desc = "Compact thermal audit slip"
                elif fname == "RptDSR_8.5.rpt":
                    printer = "DOT_MATRIX_80"
                    paper = '8.5" Continuous Roll'
                    badge = "DMP 80"
                    desc = "Dot-matrix audit continuous stationery"
                else:
                    printer = "LASER_A4_PORTRAIT"
                    paper = "A4 Portrait"
                    badge = "A4"
                    desc = "Full daily summary audit sheet"
            elif fname.startswith("RptStk"):
                cat = "STOCK_BALANCE"
                printer = "LASER_A4_LANDSCAPE"
                paper = "A4 Landscape"
                badge = "A4 L"
                desc = "Stock balance and excise audit register"
            elif fname.startswith("fmtPB"):
                cat = "PURCHASE_BILL"
                printer = "LASER_A4_PORTRAIT" if "A4" in fname else "THERMAL_80MM"
                paper = "A4 Portrait" if "A4" in fname else "80mm Roll"
                badge = "A4" if "A4" in fname else "80mm"
                desc = "Goods inward receipt voucher"
            elif fname.startswith("fmtPI"):
                cat = "PURCHASE_INDENT"
                printer = "LASER_A4_PORTRAIT"
                paper = "A4 Portrait"
                badge = "A4"
                desc = "KSBCL indent requisition order"
            elif fname.startswith("fmtSTKDED"):
                cat = "STOCK_DEDUCTION"
                printer = "LASER_A4_PORTRAIT" if "A4" in fname else "THERMAL_80MM"
                paper = "A4 Portrait" if "A4" in fname else "80mm Roll"
                badge = "A4" if "A4" in fname else "80mm"
                desc = "Breakage and deduction voucher"
            else:
                cat = "GENERIC_REPORT"
                printer = "LASER_A4_LANDSCAPE" if any(w in fname for w in ("Sales", "Item", "Pur", "Dis")) else "LASER_A4_PORTRAIT"
                paper = "A4 Landscape" if printer == "LASER_A4_LANDSCAPE" else "A4 Portrait"
                badge = "Landscape" if printer == "LASER_A4_LANDSCAPE" else "Portrait"
                desc = "MIS Analytical and transactional register"

            if category and cat != category:
                continue

            files.append({
                "id": fname.replace(".rpt", "").replace(" ", "_"),
                "filename": fname,
                "rptFileName": fname,
                "name": fname.replace(".rpt", "").replace("_", " "),
                "category": cat,
                "printer": printer,
                "paperName": paper,
                "badge": badge,
                "description": desc,
                "size_bytes": p.stat().st_size
            })
    return files



# ─── Package-wise sales / purchase, Loyalty and Excise Stock (legacy frmRptSalesPkgwise, frmRptSPurPkgwise, frmRptLoyalty,
# frmRptExciseStk) ───────────────────────────────────────────────────────────────

@router.get("/sales/package-wise")
def package_wise_sales(from_date: date, to_date: date):
    """Sales by bottle size (package): bottles, litres and amount."""
    _check_range(from_date, to_date)
    pk = defaultdict(lambda: {"qty": 0, "litres": 0.0, "amount": 0.0})
    for inv in _saved_invoices(from_date, to_date):
        for l in inv["lines"]:
            p = pk[_item(l["item_code"])["package"] or "-"]
            p["qty"] += l["qty"]
            p["litres"] = round(p["litres"] + l["litres"], 3)
            p["amount"] = round(p["amount"] + l["amount"], 2)
    return sorted(({"package": k, **v} for k, v in pk.items()), key=lambda r: -r["litres"])


@router.get("/purchase/package-wise")
def package_wise_purchase(from_date: date, to_date: date, current_user: dict = Depends(get_current_user)):
    """Purchases (net of purchase returns) by bottle size. The amount is at cost, so it is hidden from roles without cost access."""
    _check_range(from_date, to_date)
    pk = defaultdict(lambda: {"qty": 0, "litres": 0.0, "amount": 0.0})
    db_store.ensure_days(from_date, to_date, purchases=True)
    for bill in store.DOCS["purchase_bill"].values():
        if not from_date.isoformat() <= bill["date"] <= to_date.isoformat():
            continue
        sign = -1 if bill.get("purchase_type") == "Purchase Return" else 1
        for l in bill["lines"]:
            p = pk[_item(l["item_code"])["package"] or "-"]
            p["qty"] += sign * l["qty"]
            p["litres"] = round(p["litres"] + sign * l["litres"], 3)
            p["amount"] = round(p["amount"] + sign * l["amount"], 2)
    hide = hides_cost(current_user)
    return sorted(({"package": k, **v, **({"amount": None} if hide else {})} for k, v in pk.items()), key=lambda r: -r["litres"])


@router.get("/loyalty")
def loyalty_report(from_date: date, to_date: date):
    """Loyalty points per customer: balance now, and what the saved bills of the period earned and redeemed."""
    _check_range(from_date, to_date)
    earned, redeemed = defaultdict(float), defaultdict(float)
    for inv in _saved_invoices(from_date, to_date):
        earned[inv["customer"]] += inv["totals"].get("points_earned", 0)
        redeemed[inv["customer"]] += inv["totals"].get("points_redeemed", 0)
    rows = []
    for c in store.CUSTOMERS.values():
        if c["code"] == "CASH" or not (c.get("points") or earned[c["code"]] or redeemed[c["code"]]):
            continue
        rows.append({"customer": c["code"], "name": c["name"], "mobile": c.get("mobile", ""), "card_no": c.get("loyalty_card_no", ""),
                     "earned": round(earned[c["code"]], 2), "redeemed": round(redeemed[c["code"]], 2), "balance": round(c.get("points") or 0, 2)})
    return sorted(rows, key=lambda r: -r["balance"])


@router.get("/excise-stock")
def excise_stock(on: date):
    """Closing stock in bottles and litres by liquor type and bottle size (the excise stock statement)."""
    grid = defaultdict(lambda: {"qty": 0, "litres": 0.0})
    for r in stock_balance(on, on):
        it = _item(r["item_code"])
        g = grid[(it["liqtype"] or "Other", it["package"] or "-")]
        g["qty"] += r["cl_qty"]
        g["litres"] = round(g["litres"] + r["cl_litres"], 3)
    return [{"liqtype": t, "package": p, **v} for (t, p), v in sorted(grid.items()) if v["qty"]]
