"""The Stock Balance report printed through the desktop's own RptStkBal*.rpt files (Crystal Reports bridge).

Like the desktop, the report reads the work table RptStk (one row per item: opening, purchase, sales return, sales,
stock deduction, closing, with values) joined to MstITEM / MstITEMGROUP / MstPKG / MstComp. The rows are written just
before the report is laid out, tagged with a UserName of this web app's own ('WEB-<user>'), and removed right after,
so the desktop's own rows in the same table are never touched.
"""
import threading
from datetime import date

from fastapi import HTTPException

from . import db, rpt_bridge, sqlstore, store
from .routers import reports

FILES = ("RptStkBal.rpt", "RptStkBal_Biggsize.rpt", "RptStkBal_Without CLvalue.rpt", "RptStkBal_with Mrp and inv rate.rpt")
_lock = threading.Lock()


def _tag(user: dict) -> str:
    return ("WEB-" + str(user["user"]))[:50]


def _picked(raw: str) -> set[str]:
    return {v.strip() for v in (raw or "").split(",") if v.strip()}


def _rows(from_date: date, to_date: date, group: str | None, user: dict, groups: str = "", items: str = "", packages: str = "") -> list[dict]:
    out = []
    pick_groups, pick_items, pick_pkgs = _picked(groups), _picked(items), _picked(packages)
    pkg_items: set[str] | None = None   # the item codes whose PKGItem was picked (the exe filters on i.PKGItem)
    if pick_pkgs:
        marks = ", ".join("?" * len(pick_pkgs))
        pkg_items = {str(r["c"]).strip() for r in db.fetchall(f"SELECT ItemCode c FROM MstItem WITH (NOLOCK) WHERE PKGItem IN ({marks})", tuple(pick_pkgs))}
    by_type = reports.stock_list_by_doc_type(from_date, to_date)   # the stock list: the desktop's documents count too, not only the web's movements
    sales_value: dict[str, float] = {}
    if by_type is not None:
        for inv in reports._saved_invoices(from_date, to_date):
            for l in inv["lines"]:
                sales_value[l["item_code"]] = sales_value.get(l["item_code"], 0.0) + l["amount"]
    for r in reports.stock_balance(from_date, to_date, group, user):
        if (pick_groups and r["group"] not in pick_groups) or (pick_items and r["item_code"] not in pick_items) or (pkg_items is not None and r["item_code"] not in pkg_items):
            continue
        it = store.ITEMS.get(r["item_code"]) or {}
        item_id = str(it.get("_itemid") or "")
        if not item_id.isdigit():
            continue
        rate = float(it.get("purchase_rate") or 0)
        pb = sr = inv = sd = inv_value = 0.0
        if by_type is not None:
            t = by_type.get(item_id, {})
            pb, sr, inv, sd = t.get("PB", 0.0), t.get("SR", 0.0), t.get("INV", 0.0), t.get("STKDED", 0.0)
            inv_value = sales_value.get(r["item_code"], 0.0)
        else:
            for m in reports._moves(from_date, to_date, r["item_code"]):
                q = m["qty"]
                if m["kind"] in ("purchase", "opening"):
                    pb += q
                elif m["kind"] == "sales-return":
                    sr += q
                elif m["kind"] in ("sale", "sale-cancel"):
                    inv -= q
                    inv_value -= q * m["rate"]
                elif m["kind"] == "deduction":
                    sd -= q
        op, cl = float(r["op_qty"]), float(r["cl_qty"])
        if not (op or cl or pb or sr or inv or sd):
            continue
        out.append({
            "itemID": int(item_id), "Rate": rate, "RatePerCase": rate, "OPQty": op, "OPValue": round(op * rate, 2),
            "PBQty": pb, "PBValue": round(pb * rate, 2), "SRQty": sr, "SRValue": round(sr * rate, 2),
            "InvQty": inv, "InvRate": round(inv_value / inv, 2) if inv else 0.0, "InvValue": round(inv_value, 2),
            "SDQty": sd, "SDValue": round(sd * rate, 2), "PRQty": 0.0, "PRValue": 0.0,
            "ClQty": cl, "ClValue": round(cl * rate, 2),
        })
    return out


def _clear(cur, tag: str) -> None:
    cur.execute("DELETE FROM dbo.RptStk WHERE UserName = ?", (tag,))


def render(from_date: date, to_date: date, group: str | None, user: dict, rpt: str = "",
           printer: str = "", copies: int = 1, groups: str = "", items: str = "", packages: str = "") -> bytes | None:
    """The report as a PDF laid out by Crystal; with a `printer` it is sent straight there (nothing returned)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "The database is offline, so Crystal Reports has nothing to read.")
    name = rpt_bridge.safe_rpt_name(rpt or FILES[0])
    if name.lower() not in {f.lower() for f in FILES}:
        raise HTTPException(422, f"'{name}' is not a Stock Balance format")
    tag = _tag(user)
    rows = _rows(from_date, to_date, group, user, groups, items, packages)
    if not rows:
        raise HTTPException(404, "No stock movement or balance in this period: nothing to print.")
    header = f"Stock Balance Report From {from_date:%d-%m-%Y} To {to_date:%d-%m-%Y}" + (f" ({group})" if group else "")
    job = {"rpt": name, "selection": f"{{RptStk.UserName}} = '{tag}'",
           "parameters": {"daterange": f"{from_date:%d-%m-%Y} to {to_date:%d-%m-%Y}"},
           "copies": max(1, min(int(copies or 1), 20)), "printer": printer.strip(),
           "action": "direct" if printer.strip() else "preview", "option": "", "stored_proc": ""}
    cols = ["UserName", "hdrName"] + list(rows[0])
    sql = f"INSERT INTO dbo.RptStk ({', '.join(cols)}) VALUES ({', '.join('?' * len(cols))})"
    with _lock:
        try:
            with db.transaction() as cur:
                _clear(cur, tag)
                cur.executemany(sql, [(tag, header, *r.values()) for r in rows])
            if job["printer"]:
                rpt_bridge.print_direct(job)
                return None
            return rpt_bridge.render_pdf(job)
        finally:
            try:
                with db.transaction() as cur:
                    _clear(cur, tag)
            except Exception:   # noqa: BLE001 - the rows are this app's own; the next run deletes them first
                pass


def formats() -> list[str]:
    on_disk = {f.lower(): f for f in rpt_bridge.rpt_files()}
    return [on_disk[f.lower()] for f in FILES if f.lower() in on_disk]
