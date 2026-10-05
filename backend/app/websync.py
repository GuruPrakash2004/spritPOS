"""Web Sync (the desktop app's frmModWebSync: syncDataToWeb / deleteForTodaysData / pushData).

Billing stays on the shop's own PC and SQL Server. Web Sync sends a summary of each day, plus the current
stock, to the hosted central site (app/central/), where the owner sees every shop. The central site keeps
each shop's rows per day and replaces them on every push, as the desktop app does in its central database.

What is sent for a day (build(day)): the desktop's central tables, by name and with its column names (ClientID
and DocDate are added by the central site): RptDSRSmry, RptDSRCtrSales, RptDSRInvCancelled, RptDSRSPWise,
RptDSRGroupWiseSales, RptDSRGroupWisePur, RptDSRExpLedWise, RptDSR_Gst, RptDSRPmt, RptFastMov, RptMinStk and
the day's purchase indents (TrnHdrPI / TrnDtlPI). build_stock() gives RptWebStk, every item's current stock.

Settings are in backend/.env only (never in Global Settings): WEB_SYNC_URL (the central site's API),
WEB_SYNC_KEY (this shop's sync key, from `tools/central.py add-client` on the central server) and WEB_SYNC_MINUTES
(default 10). Without the URL and key, Web Sync is off and nothing is sent.

Only the shop's live data is ever sent: while the database isn't set up, loaded or reachable (the store is then
empty) nothing is sent, so a central day is never replaced by an empty one.
boot.py starts the background thread once the bill history has loaded. Each round sends every day from the
last day sent successfully up to today (at most CATCH_UP_DAYS; at the first round, yesterday and today) and
then the stock, every WEB_SYNC_MINUTES. A failure is kept in STATUS and tried again on the next round; it
never holds up billing.

Month and year totals and the last purchase bill come from SQL Server (Web_TrnHdrSINV / Web_TrnHdrPB, which
hold every bill, desktop and web) when it is connected; memory holds only the latest bills.
"""
import logging
import os
import threading
import time
from collections import defaultdict
from datetime import date, datetime, timedelta
from urllib.parse import urlsplit

import httpx

from . import db, store

logger = logging.getLogger("vlqms.websync")

DAY_TABLES = ("RptDSRSmry", "RptDSRCtrSales", "RptDSRInvCancelled", "RptDSRSPWise", "RptDSRGroupWiseSales",
              "RptDSRGroupWisePur", "RptDSRExpLedWise", "RptDSR_Gst", "RptDSRPmt", "RptFastMov", "RptMinStk",
              "TrnHdrPI", "TrnDtlPI")
TOP = 20                 # RptFastMov / RptMinStk keep the top 20, as the desktop app does
TIMEOUT = 20.0           # seconds for one push
CATCH_UP_DAYS = 31       # after a long outage, at most this many days are sent again
PAY_NAMES = {"cash": "Cash", "card": "Card", "upi": "UPI", "credit": "Credit"}

STATUS: dict = {"last_ok": None, "last_day": None, "last_stock_ok": None, "last_error": None, "last_error_at": None}
_sync_lock = threading.Lock()
_thread: threading.Thread | None = None


class SyncFailed(Exception):
    """The central site could not be reached, or refused the push."""


# ─── Settings ────────────────────────────────────────────────────────────────

def _url() -> str:
    return os.environ.get("WEB_SYNC_URL", "").strip().rstrip("/")


def _url_problem() -> str | None:
    """The key goes in a header: only over HTTPS (plain http only to this PC, for trying it out)."""
    parts = urlsplit(_url())
    if parts.scheme == "https" and parts.hostname:
        return None
    if parts.scheme == "http" and parts.hostname in ("localhost", "127.0.0.1", "::1"):
        return None
    return "WEB_SYNC_URL must be an https:// address (the sync key must not travel unencrypted)"


def _key() -> str:
    return os.environ.get("WEB_SYNC_KEY", "").strip()


def configured() -> bool:
    return bool(_url() and _key())


def minutes() -> int:
    try:
        return max(1, int(os.environ.get("WEB_SYNC_MINUTES", "10")))
    except ValueError:
        return 10


def central_host() -> str:
    """Where the pushes go, host only (never the key)."""
    parts = urlsplit(_url())
    if not parts.hostname:
        return ""
    return parts.hostname + (f":{parts.port}" if parts.port else "")


def _live() -> bool:
    """The shop's own data is loaded (the store isn't just empty)."""
    from . import sqlstore
    return sqlstore.enabled()


# ─── What is sent ────────────────────────────────────────────────────────────

def _r(v: float) -> float:
    return round(v or 0, 2)


def _fy_start(day: date) -> date:
    """The financial year's first day for `day` (April 1, or the month/day of the company's fy_start)."""
    try:
        start = date.fromisoformat(store.COMPANY.get("fy_start", ""))
        month, dom = start.month, start.day
    except ValueError:
        month, dom = 4, 1
    year = day.year if (day.month, day.day) >= (month, dom) else day.year - 1
    return date(year, month, dom)


def _rows(counts: dict, values: dict, name_col: str) -> list[dict]:
    return [{"slno": i, name_col: k, "NoOfBills": counts[k], "BillValue": _r(values[k])}
            for i, k in enumerate(sorted(values), start=1)]


def _doc_no_number(doc_no: str) -> int:
    try:
        return int(doc_no.rsplit("-", 1)[1])
    except (IndexError, ValueError):
        return 0


def build(day: date) -> dict:
    """The desktop's central tables for one day."""
    from .routers import reports   # here: routers import much of the app

    d = day.isoformat()
    dsr = reports.daily_summary_report(day, current_user=None)   # not a user: nothing is hidden
    invs = [i for i in store.DOCS["sales_invoice"].values() if i["date"] == d]
    saved = [i for i in invs if i["status"] == "SAVED"]
    cancelled = [i for i in invs if i["status"] == "CANCELLED"]
    pbs = [p for p in store.DOCS["purchase_bill"].values() if p["date"] == d]

    ctr_n, ctr_v, sp_n, sp_v = defaultdict(int), defaultdict(float), defaultdict(int), defaultdict(float)
    can_n, can_v = defaultdict(int), defaultdict(float)
    grp_q, grp_v, gst, sold = defaultdict(float), defaultdict(float), defaultdict(float), defaultdict(float)
    for inv in saved:
        ctr_n[inv["counter"]] += 1
        ctr_v[inv["counter"]] += inv["totals"]["net"]
        person = inv.get("salesperson") or "-"
        sp_n[person] += 1
        sp_v[person] += inv["totals"]["net"]
        for line in inv["lines"]:
            it = reports._item(line["item_code"])
            grp_q[it["group"]] += line["qty"]
            grp_v[it["group"]] += line["amount"]
            sold[it["name"]] += line["qty"]
            gst[f"CGST @{it.get('cgst_per', 0) or 0:g}"] += line.get("cgst", 0) or 0
            gst[f"SGST @{it.get('sgst_per', 0) or 0:g}"] += line.get("sgst", 0) or 0
    for inv in cancelled:
        can_n[inv["counter"]] += 1
        can_v[inv["counter"]] += inv["totals"]["net"]

    pur_q, pur_v = defaultdict(float), defaultdict(float)
    pur_at_mrp = 0.0
    for pb in pbs:
        for line in pb["lines"]:
            it = reports._item(line["item_code"])
            pur_q[it["group"]] += line["qty"]
            pur_v[it["group"]] += line["qty"] * (it.get("mrp") or 0)
            pur_at_mrp += line["qty"] * (it.get("mrp") or 0)

    # Month and year to date (financial year), and the last purchase bill up to this day.
    month_from, year_from = day.replace(day=1).isoformat(), _fy_start(day).isoformat()
    period = _period_from_sql(month_from, year_from, d)
    sales_mtd = sales_ytd = pur_mtd = pur_ytd = 0.0
    for inv in store.DOCS["sales_invoice"].values():
        if inv["status"] == "SAVED" and year_from <= inv["date"] <= d:
            sales_ytd += inv["totals"]["net"]
            if inv["date"] >= month_from:
                sales_mtd += inv["totals"]["net"]
    last_pb = None
    for pb in store.DOCS["purchase_bill"].values():
        if pb.get("status", "SAVED") in ("CANCELLED", "DELETED") or pb["date"] > d:
            continue
        if pb["date"] >= year_from:
            pur_ytd += pb["totals"]["net"]
            if pb["date"] >= month_from:
                pur_mtd += pb["totals"]["net"]
        if last_pb is None or (pb["date"], pb["doc_no"]) > (last_pb["date"], last_pb["doc_no"]):
            last_pb = pb

    if period is not None:
        sales_mtd, sales_ytd, pur_mtd, pur_ytd = period["sales_mtd"], period["sales_ytd"], period["pur_mtd"], period["pur_ytd"]
        last_pb = period["last_pb"]
    round_off = 0.0
    for inv in saved:
        t = inv["totals"]
        diff = t["net"] - (t.get("basic", 0) + t.get("cgst", 0) + t.get("sgst", 0) - t.get("discount", 0)
                           - t.get("points_redeemed", 0))
        round_off += diff if abs(diff) < 1 else 0   # bills are rounded to the rupee; anything bigger isn't round-off
    pay = dsr["payment_modes"]
    summary = {
        "TotBills": dsr["total_bills"], "TotSalesAmt": dsr["sales_incl_gst"], "TotRndOffAmt": _r(round_off),
        "SalesAmtLiq": dsr["liquor_basic"], "SalesAmtNonLiq": dsr["non_liquor_basic"],
        "SalesAmtCtrWise": _r(sum(ctr_v.values())), "SalesProfitAmt": dsr["profit_of_the_day"],
        "OPStkQty": dsr["opening_stock"]["qty"], "OPStkValue": dsr["opening_stock"]["value"],
        "ClStkQty": dsr["closing_stock"]["qty"], "CLStkValue": dsr["closing_stock"]["value"],
        "TotCashAmt": _r(pay.get("cash")), "TotCardAmt": _r(pay.get("card")), "OtherAmt": _r(pay.get("upi")),
        "CreditAmt": _r(pay.get("credit")), "WriteOffAmt": 0,
        "TotPurAmt_SalesRate": _r(pur_at_mrp), "PurAmtBasic": dsr["purchase"]["basic"],
        "PurAmtInclGST": dsr["purchase"]["incl_gst"], "TotSalesRetnAmt": dsr["sales_returns"], "CashOPAmt": 0,
        "ExpenseAmt": dsr["expenses"], "CashBalanceAmt": dsr["cash_balance"], "RedemAmount": dsr["redemption"],
        "LastPurBillNo": last_pb["doc_no"] if last_pb else "", "LastPurDate": last_pb["date"] if last_pb else None,
        "LastPurValue": _r(last_pb["totals"]["net"]) if last_pb else 0,
        "MnthPurValue": _r(pur_mtd), "YearPurValue": _r(pur_ytd),
        "DaySalesValue": dsr["sales_incl_gst"], "MnthSalesValue": _r(sales_mtd), "YearSalesValue": _r(sales_ytd),
    }

    short = [it for it in store.ITEMS.values()
             if it.get("active", True) and (it.get("rol") or 0) > 0 and it["qoh"] < it["rol"]]
    short.sort(key=lambda it: (it["qoh"] - it["rol"], it["name"]))

    indents = sorted((p for p in store.DOCS["purchase_indent"].values() if p["date"] == d), key=lambda p: p["doc_no"])
    hdr_pi, dtl_pi = [], []
    for p in indents:
        piid = _doc_no_number(p["doc_no"])
        sup = store.SUPPLIERS.get(p["supplier"], {})
        hdr_pi.append({"PIID": piid, "PINO": p["doc_no"], "Prefix": p["doc_no"].split("/")[-1].rsplit("-", 1)[0],
                       "PrintPINO": p["doc_no"], "PIDate": p["date"], "SUPPName": sup.get("name", p["supplier"]),
                       "LOCID": 1, "TOTAMT": _r(p["totals"]["basic"]), "OTHAMT": 0, "GRNDTOT": _r(p["totals"]["net"]),
                       "TOTCGST": _r(p["totals"].get("cgst")), "TOTSGST": _r(p["totals"].get("sgst")), "TOTIGST": 0})
        for n, line in enumerate(p["lines"], start=1):
            it = reports._item(line["item_code"])
            dtl_pi.append({"PIID": piid, "PIDtlID": n, "SlNo": n, "ItemCode": line["item_code"],
                           "ITEMName": line.get("item_name") or it["name"], "PIQty": line["qty"],
                           "PIRate": _r(line.get("rate")), "PIAmt": _r(line.get("amount")),
                           "NOOFCASES": line.get("cases", 0) or 0, "NOOFBOTTLES": line.get("bottles", 0) or 0,
                           "NETAMT": _r(line.get("amount")), "MRP": _r(it.get("mrp"))})

    ledgers = {code: row.get("name", code) for code, row in store.LEDGERS.items()}
    tables = {
        "RptDSRSmry": [summary],
        "RptDSRCtrSales": _rows(ctr_n, ctr_v, "CounterNo"),
        "RptDSRInvCancelled": _rows(can_n, can_v, "CounterNo"),
        "RptDSRSPWise": _rows(sp_n, sp_v, "CounterNo"),   # the desktop keeps the salesperson in CounterNo
        "RptDSRGroupWiseSales": [{"slno": i, "GroupName": g, "TotQty": grp_q[g], "TotAmt": _r(grp_v[g])}
                                 for i, g in enumerate(sorted(grp_v), start=1)],
        "RptDSRGroupWisePur": [{"slno": i, "GroupName": g, "TotQty": pur_q[g], "TotAmt": _r(pur_v[g])}
                               for i, g in enumerate(sorted(pur_q), start=1)],
        "RptDSRExpLedWise": [{"slno": i, "LedgerName": ledgers.get(k, k), "LedAmt": _r(v)}
                             for i, (k, v) in enumerate(sorted(dsr["expense_ledger_wise"].items()), start=1)],
        "RptDSR_Gst": [{"slno": i, "GST": k, "GSTAmt": _r(v)} for i, (k, v) in enumerate(sorted(gst.items()), start=1)],
        "RptDSRPmt": [{"SlNo": i, "Particulars": PAY_NAMES.get(k, k.title()), "PmtAmt": _r(v)}
                      for i, (k, v) in enumerate(sorted(pay.items()), start=1) if v],
        "RptFastMov": [{"ItemName": name, "SoldQty": qty}
                       for name, qty in sorted(sold.items(), key=lambda kv: (-kv[1], kv[0]))[:TOP]],
        "RptMinStk": [{"ItemName": it["name"], "QOH": it["qoh"], "MinLevel": it["rol"], "ShortageQty": it["rol"] - it["qoh"]}
                      for it in short[:TOP]],
        "TrnHdrPI": hdr_pi,
        "TrnDtlPI": dtl_pi,
    }
    return {"day": d, "tables": tables}


def _period_from_sql(month_from: str, year_from: str, to: str) -> dict | None:
    """Month/year sales and purchase totals and the last purchase bill from SQL Server, or None (no database,
    or the query failed: memory is used then)."""
    if not db.is_db_available():
        return None
    try:
        sales = db.fetchone(
            "SELECT SUM(CASE WHEN SINVDate >= ? THEN INVVALUE ELSE 0 END) AS mtd, SUM(INVVALUE) AS ytd "
            "FROM dbo.Web_TrnHdrSINV WHERE SINVDate >= ? AND SINVDate < DATEADD(day, 1, CAST(? AS date)) "
            "AND ISNULL(Cancelled, 'N') = 'N' AND ISNULL(IsDelete, 'N') = 'N'", (month_from, year_from, to)) or {}
        pur = db.fetchone(
            "SELECT SUM(CASE WHEN PBDate >= ? THEN GRNDTOT ELSE 0 END) AS mtd, SUM(GRNDTOT) AS ytd "
            "FROM dbo.Web_TrnHdrPB WHERE PBDate >= ? AND PBDate < DATEADD(day, 1, CAST(? AS date)) "
            "AND ISNULL(Cancelled, 'N') = 'N'", (month_from, year_from, to)) or {}
        last = db.fetchone(
            "SELECT TOP 1 PBNO, PBDate, GRNDTOT FROM dbo.Web_TrnHdrPB WHERE PBDate < DATEADD(day, 1, CAST(? AS date)) "
            "AND ISNULL(Cancelled, 'N') = 'N' ORDER BY PBDate DESC, PBID DESC", (to,))
    except Exception as exc:   # an older Web_ schema, or the connection dropped: fall back to memory
        logger.warning("Web Sync: month/year totals from memory (%s)", type(exc).__name__)
        return None
    num = lambda v: float(v or 0)
    return {"sales_mtd": num(sales.get("mtd")), "sales_ytd": num(sales.get("ytd")),
            "pur_mtd": num(pur.get("mtd")), "pur_ytd": num(pur.get("ytd")),
            "last_pb": {"doc_no": str(last["PBNO"]), "date": str(last["PBDate"])[:10], "totals": {"net": num(last["GRNDTOT"])}}
            if last else None}


def build_stock() -> dict:
    """RptWebStk: every item's current stock."""
    groups = {code: row.get("name", code) for code, row in store.ITEM_GROUPS.items()}
    items = [{"ItemCode": it["code"], "ItemName": it["name"], "ItemGroupName": groups.get(it.get("group"), it.get("group", "")),
              "Brand": it.get("brand", ""), "Package": it.get("package", ""), "QOH": it["qoh"],
              "MinLevel": it.get("rol", 0) or 0, "MRP": _r(it.get("mrp")), "Active": bool(it.get("active", True))}
             for it in store.ITEMS.values()]
    return {"as_of": datetime.now().isoformat(timespec="seconds"), "items": items}


def _snapshot(fn, *args):
    """Build while bills may be saved at the same time: a dict that changes mid-read is read again."""
    for attempt in range(5):
        try:
            return fn(*args)
        except RuntimeError:   # "dictionary changed size during iteration"
            time.sleep(0.05 * (attempt + 1))
    return fn(*args)


# ─── Sending ─────────────────────────────────────────────────────────────────

def _post(path: str, body: dict) -> None:
    try:
        with httpx.Client(timeout=TIMEOUT) as client:
            res = client.post(_url() + path, json=body, headers={"Authorization": f"Bearer {_key()}"})
    except httpx.HTTPError as exc:
        raise SyncFailed(f"Could not reach the central site ({type(exc).__name__})") from None
    if res.status_code == 401:
        raise SyncFailed("The central site refused this shop's sync key (WEB_SYNC_KEY)")
    if res.status_code >= 400:
        detail = ""
        try:
            detail = str(res.json().get("detail", ""))[:200]
        except (ValueError, AttributeError):
            pass
        raise SyncFailed(f"The central site answered {res.status_code}" + (f": {detail}" if detail else ""))


def push(day: date) -> None:
    _post("/sync", _snapshot(build, day))


def push_stock() -> None:
    _post("/sync/stock", _snapshot(build_stock))


def sync(days: list[date], stock: bool = True) -> dict:
    """Send these days (and the stock); record the result in STATUS. Raises SyncFailed."""
    if not configured():
        raise SyncFailed("Web Sync is not set up: WEB_SYNC_URL and WEB_SYNC_KEY in backend/.env")
    if _url_problem():
        raise SyncFailed(_url_problem())
    if not _live():
        raise SyncFailed("The shop's database isn't loaded, so nothing was sent")
    with _sync_lock:
        try:
            try:
                for day in days:
                    if not _live():   # e.g. Test data switched on meanwhile: never send sample figures
                        raise SyncFailed("The shop's database isn't loaded, so nothing more was sent")
                    push(day)
                    STATUS["last_day"] = day.isoformat()
                if stock and _live():
                    push_stock()
            except SyncFailed:
                raise
            except Exception as exc:   # e.g. the store being reloaded while it was read
                logger.exception("Web Sync failed")
                raise SyncFailed(f"Could not prepare the data to send ({type(exc).__name__}); it is tried again") from None
            STATUS["last_ok"] = datetime.now().isoformat(timespec="seconds")
            if stock:
                STATUS["last_stock_ok"] = STATUS["last_ok"]
            STATUS["last_error"] = None
        except SyncFailed as exc:
            STATUS["last_error"] = str(exc)
            STATUS["last_error_at"] = datetime.now().isoformat(timespec="seconds")
            raise
    return dict(STATUS)


def days_to_send(today: date, last_sent: date | None) -> list[date]:
    """From the last day sent successfully (sent again: it may have changed since) to today; at the first
    round yesterday and today; never more than CATCH_UP_DAYS."""
    first = last_sent if last_sent is not None else today - timedelta(days=1)
    first = max(min(first, today), today - timedelta(days=CATCH_UP_DAYS - 1))
    return [first + timedelta(days=n) for n in range((today - first).days + 1)]


def _loop() -> None:
    last_sent: date | None = None
    rounds = 0
    while True:
        today = date.today()
        try:
            sync(days_to_send(today, last_sent))
            rounds += 1
            # After the first round yesterday is sent once more, in case the bill history was still loading.
            last_sent = today if rounds > 1 else today - timedelta(days=1)
        except SyncFailed as exc:
            logger.warning("Web Sync failed: %s", exc)
        except Exception:
            logger.exception("Web Sync failed")
        time.sleep(minutes() * 60)


def start() -> None:
    """Start the background sync once, if it is set up and the shop's own data is loaded."""
    global _thread
    if not configured() or not _live() or (_thread is not None and _thread.is_alive()):
        return
    if _url_problem():
        logger.error("Web Sync off: %s", _url_problem())
        return
    _thread = threading.Thread(target=_loop, name="web-sync", daemon=True)
    _thread.start()
    logger.info("Web Sync on: every %s minutes to %s", minutes(), central_host())
