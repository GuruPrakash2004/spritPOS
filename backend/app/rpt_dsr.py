"""The Daily Summary Report printed through the desktop's own RptDSR.rpt / RptDSR_S.rpt (Crystal Reports bridge).

The desktop (frmRptDailySummary) fills the report like this, and so does this module:
- the day's totals go in as report parameters (CompName, Add1..4, phno, gstin, mNoOfInv, mCashSales, mCardSales,
  mTotInvValue, mSalesRet, mTotExp, mNetAmt, mCGSTAMT, mSGSTAMT, mCESSAMT, mopqty, mopval, mclqty, mclval, mupiamt);
- the group-wise sales go in as rows of the work table RptUDR (Fld1 item group, Fld2 qty, Fld3 amount, Fld4 the
  heading the group is printed under), next to one row of RptUDRHeading, both tagged with RptName / UserName.
The report reads those rows, so they are written just before the report is laid out and removed right after. They
belong to this web app alone (RptName 'Web DSR'), so the desktop's own rows in the same tables are never touched.
"""
import threading
from collections import defaultdict
from datetime import date

from fastapi import HTTPException

from . import db, rpt_bridge, sqlstore, store
from .routers import reports

RPT_NAME = "Web DSR"
FILES = ("RptDSR.rpt", "RptDSR_S.rpt")   # RptDSR_8.5.rpt hangs the bridge: not offered
_lock = threading.Lock()   # one DSR at a time per backend: the rows are tagged by user, not by request


def _money(v) -> str:
    return f"{float(v or 0):.2f}"


def _qty(v) -> str:
    return f"{float(v or 0):g}"


def _day(on: date) -> dict:
    d = on.isoformat()
    saved = [i for i in store.DOCS["sales_invoice"].values() if i["date"] == d and i["status"] == "SAVED"]
    qty_by_group: dict[str, float] = defaultdict(float)
    cgst = sgst = cess = 0.0
    for inv in saved:
        t = inv["totals"]
        cgst += t.get("cgst", 0)
        sgst += t.get("sgst", 0)
        cess += t.get("cess", 0)
        for l in inv["lines"]:
            it = store.ITEMS.get(l["item_code"]) or {}
            qty_by_group[it.get("group") or "-"] += l["qty"]
    return {"bills": len(saved), "qty_by_group": qty_by_group, "cgst": cgst, "sgst": sgst, "cess": cess}


def parameters(on: date, user: dict) -> tuple[dict, dict]:
    """(report parameters, the day's DSR numbers)."""
    dsr = reports.daily_summary_report(on, user)
    extra = _day(on)
    pay = dsr["payment_modes"]
    inv_value = dsr["sales_incl_gst"]
    net = inv_value - dsr["sales_returns"] - dsr["expenses"]
    c = store.COMPANY
    addr = [str(c.get(k, "") or "") for k in ("address", "address2", "address3", "address4")]
    params = {
        "CompName": c.get("name", ""), "Add1": addr[0], "Add2": addr[1], "Add3": addr[2], "Add4": addr[3],
        "phno": c.get("phone", ""), "gstin": c.get("gstin", ""),
        "mNoOfInv": str(dsr["total_bills"]), "mCashSales": _money(pay.get("cash")), "mCardSales": _money(pay.get("card")),
        "mupiamt": _money(pay.get("upi")), "mTotInvValue": _money(inv_value), "mSalesRet": _money(dsr["sales_returns"]),
        "mTotExp": _money(dsr["expenses"]), "mNetAmt": _money(net),
        "mCGSTAMT": round(extra["cgst"], 2), "mSGSTAMT": _money(extra["sgst"]), "mCESSAMT": round(extra["cess"], 2),
        "mopqty": _qty(dsr["opening_stock"]["qty"]), "mopval": _money(dsr["opening_stock"]["value"]),
        "mclqty": _qty(dsr["closing_stock"]["qty"]), "mclval": _money(dsr["closing_stock"]["value"]),
    }
    return params, {"dsr": dsr, **extra}


def _write_rows(cur, user_code: str, on: date, groups: dict, qty: dict) -> None:
    blanks = {f"Fld{n}": "" for n in range(1, 21)}
    for name, amount in sorted(groups.items()):
        row = {**blanks, "RptName": RPT_NAME, "UserName": user_code, "Fld1": name, "Fld2": _qty(qty.get(name)),
               "Fld3": _money(amount), "Fld4": "Sales"}
        cols = list(row)
        cur.execute(f"INSERT INTO dbo.RptUDR ({', '.join(cols)}) VALUES ({', '.join('?' * len(cols))})", tuple(row.values()))
    head = {**blanks, "RptName": RPT_NAME, "UserName": user_code, "Fld1": "ITEMGROUPNAME", "Fld2": "QTY", "Fld3": "AMOUNT",
            "Compname": store.COMPANY.get("name", ""), "Address": store.COMPANY.get("address", ""),
            "HdrText": f"Daily Summary Report for {on:%d-%m-%Y}", "FilterText": ""}
    cols = list(head)
    cur.execute(f"INSERT INTO dbo.RptUDRHeading ({', '.join(cols)}) VALUES ({', '.join('?' * len(cols))})", tuple(head.values()))


def _clear(cur, user_code: str) -> None:
    cur.execute("DELETE FROM dbo.RptUDR WHERE RptName = ? AND UserName = ?", (RPT_NAME, user_code))
    cur.execute("DELETE FROM dbo.RptUDRHeading WHERE RptName = ? AND UserName = ?", (RPT_NAME, user_code))


def render(on: date, user: dict, rpt: str = "", printer: str = "", copies: int = 1) -> bytes | None:
    """The DSR of `on` as a PDF laid out by Crystal; with a `printer` it is sent straight to that printer instead
    (and nothing is returned)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "The database is offline, so Crystal Reports has nothing to read.")
    name = rpt_bridge.safe_rpt_name(rpt or FILES[0])
    if name.lower() not in {f.lower() for f in FILES}:
        raise HTTPException(422, f"'{name}' is not a Daily Summary Report format")
    params, info = parameters(on, user)
    code = str(user["user"])[:50]
    job = {"rpt": name, "selection": f"{{RptUDR.RptName}} = '{RPT_NAME}' and {{RptUDR.UserName}} = '{code}'",
           "parameters": params, "copies": max(1, min(int(copies or 1), 20)), "printer": printer.strip(),
           "action": "direct" if printer.strip() else "preview", "option": "", "stored_proc": ""}
    with _lock:
        try:
            with db.transaction() as cur:
                _clear(cur, code)
                _write_rows(cur, code, on, info["dsr"]["group_wise_sales"], info["qty_by_group"])
            if job["printer"]:
                rpt_bridge.print_direct(job)
                return None
            return rpt_bridge.render_pdf(job)
        finally:
            try:
                with db.transaction() as cur:
                    _clear(cur, code)
            except Exception:   # noqa: BLE001 - the rows are this app's own; the next run deletes them first
                pass


def formats() -> list[str]:
    on_disk = {f.lower(): f for f in rpt_bridge.rpt_files()}
    return [on_disk[f.lower()] for f in FILES if f.lower() in on_disk]
