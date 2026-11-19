"""Barcode labels printed through the desktop's own RPTBC*.rpt files (Crystal Reports bridge).

The desktop's Barcode Printing screen (frmUtlBarCodePrint.btnPrint_Click) empties the work table RptBC, writes one row per
label (Slno, USERNAME, BCNO, ItemCode, ItemName, Qty, UOM, MRP: a row is repeated "No of Labels" times for every ticked
item) and opens RPTBC.rpt with the selection `{rptbc.UserName} = '<user>'`. This does the same, except that the rows carry
this web app's own tag ('WEB-<user>') and are removed right after, so the desktop's rows in the table are never touched.
Every RPTBC*.rpt in the report folder is a format the user can pick, so a shop with several label layouts gets them all.
"""
import threading

from fastapi import HTTPException

from . import db, rpt_bridge, sqlstore, store

MAX_LABELS = 2000    # each label is a row for Crystal; more than this would hold the request for minutes
_lock = threading.Lock()


def _tag(user: dict) -> str:
    return ("WEB-" + str(user["user"]))[:50]


def formats() -> list[str]:
    """The label formats in the report folder: every RPTBC*.rpt, by name."""
    return sorted((f for f in rpt_bridge.rpt_files() if f.lower().startswith("rptbc") and f.lower().endswith(".rpt")), key=str.lower)


def _rows(items: list[dict]) -> list[tuple]:
    """(Slno, BCNO, ItemCode, ItemName, Qty, UOM, MRP) for every label, as the desktop writes them."""
    out: list[tuple] = []
    for it in items:
        code = str(it.get("code") or "").strip()
        master = store.ITEMS.get(code)
        if not master:
            raise HTTPException(404, f"Item '{code}' is not in the Item Master.")
        copies = max(1, min(int(it.get("copies") or 1), 999))
        barcode = str(it.get("barcode") or code).strip() or code
        for _ in range(copies):
            out.append((len(out) + 1, barcode, code, str(master.get("name") or "")[:100], float(it.get("qty") or 0),
                        str(master.get("uom") or ""), round(float(it.get("mrp") or 0), 2)))
        if len(out) > MAX_LABELS:
            raise HTTPException(422, f"At most {MAX_LABELS} labels at a time.")
    return out


def render(items: list[dict], user: dict, rpt: str = "", printer: str = "", copies: int = 1) -> bytes | None:
    """The labels as a PDF laid out by Crystal; with a `printer` they are sent straight there (nothing returned)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "The database is offline, so Crystal Reports has nothing to read.")
    have = formats()
    if not have:
        raise HTTPException(404, "No barcode format (RPTBC.rpt) is in the report folder.")
    name = rpt_bridge.safe_rpt_name(rpt or have[0])
    if name.lower() not in {f.lower() for f in have}:
        raise HTTPException(422, f"'{name}' is not a barcode format")
    rows = _rows(items)
    if not rows:
        raise HTTPException(422, "Tick at least one item.")
    tag = _tag(user)
    # "columns": 1: the label formats are multi-column sheets (labels run left to right); one column makes the labels stack top to bottom.
    job = {"rpt": name, "selection": f"{{RptBC.UserName}} = '{tag}'", "parameters": {}, "columns": 1,
           "copies": max(1, min(int(copies or 1), 20)), "printer": printer.strip(),
           "action": "direct" if printer.strip() else "preview", "option": "", "stored_proc": ""}
    sql = "INSERT INTO dbo.RptBC (Slno, UserName, BCNO, ItemCode, ItemName, Qty, UOM, MRP) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"
    with _lock:
        try:
            with db.transaction() as cur:
                cur.execute("DELETE FROM dbo.RptBC WHERE UserName = ?", (tag,))
                # PK_RptBC is on Slno alone, and the desktop's own rows (any user) may still be in the table: number ours after the highest.
                cur.execute("SELECT ISNULL(MAX(Slno), 0) FROM dbo.RptBC")
                start = int(cur.fetchone()[0] or 0)
                cur.executemany(sql, [(start + r[0], tag, *r[1:]) for r in rows])
            if job["printer"]:
                rpt_bridge.print_direct(job)
                return None
            return rpt_bridge.render_pdf(job)
        finally:
            try:
                with db.transaction() as cur:
                    cur.execute("DELETE FROM dbo.RptBC WHERE UserName = ?", (tag,))
            except Exception:   # noqa: BLE001 - the rows are this app's own; the next run deletes them first
                pass
