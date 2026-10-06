"""A Customised Report printed through the Crystal file named in its definition (UtlReportsCRHdr.RptFileName).

The desktop prints these "UDR" reports the same way for every report: the rows go into the work table RptUDR (Fld1..Fld20
in the report's column order, all text), one heading row into RptUDRHeading (the column names, company, the period), and
the .rpt (UDRPurItem.rpt, ...) is run with the company name and address as parameters and a selection on the run's
RptName / UserName. The rows here belong to this web app only: UserName 'WEB-<user>', removed right after the report
is laid out, so the desktop's own rows are never touched. Reports whose definition has no file print from the grid
(Export XLS / the web print); files that read a database view directly instead of RptUDR are not offered.
"""
import threading
from datetime import date

from fastapi import HTTPException

from . import custom_engine, db, rpt_bridge, sqlstore, store

_lock = threading.Lock()
# The desktop files that read RptUDR / RptUDRHeading. NONE is offered yet: UDRPurItem.rpt and UDRSalesItemwise.rpt
# connect to the database in a way the bridge cannot log on to (Crystal waits for a login dialog and the report
# hangs; the bridge restarts itself after 55 s). Feeding them the rows as an in-memory data set instead of through the
# database would avoid the logon, but is not built. Until then these reports print with the web layout / Export XLS.
UDR_FILES: tuple[str, ...] = ()


def usable_file(defn: dict) -> str:
    """The definition's report file when it is one this app can lay out ('' otherwise)."""
    f = defn.get("rpt", "")
    return next((u for u in UDR_FILES if u.lower() == f.lower()), "")


def _text(v) -> str:
    if v is None:
        return ""
    if isinstance(v, float):
        return f"{v:.2f}"
    s = str(v)
    if len(s) == 10 and s[4] == "-" and s[7] == "-":
        return f"{s[8:10]}-{s[5:7]}-{s[0:4]}"
    return s[:250]


def render(name: str, from_date: date, to_date: date, flt: dict, user: dict, printer: str = "", copies: int = 1) -> bytes | None:
    """The report as a PDF laid out by Crystal; with a `printer` it is sent straight there (nothing returned)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "The database is offline, so Crystal Reports has nothing to read.")
    defn = custom_engine.definition(name)
    if not defn:
        raise HTTPException(404, f"'{name}' is not defined in the database.")
    file = usable_file(defn)
    if not file:
        raise HTTPException(409, f"'{name}' has no Crystal format this app can print yet; use Export XLS.")
    if len(defn["columns"]) > 20:
        raise HTTPException(409, "A UDR report holds at most 20 columns.")
    _, rows = custom_engine.run(defn, from_date, to_date, flt)
    if not rows:
        raise HTTPException(404, "Nothing in this period: nothing to print.")
    tag = ("WEB-" + str(user["user"]))[:50]
    keys = list(rows[0])
    blanks = {f"Fld{n}": "" for n in range(1, 21)}
    c = store.COMPANY
    addr = [str(c.get(k, "") or "") for k in ("address", "address2", "address3", "address4")]
    period = f"{from_date:%d-%m-%Y} to {to_date:%d-%m-%Y}"
    head = {**blanks, "RptName": name[:150], "UserName": tag, **{f"Fld{i}": k for i, k in enumerate(keys, 1)},
            "Compname": c.get("name", ""), "Address": addr[0], "HdrText": f"Report for the period: from {period}", "FilterText": ""}
    job = {"rpt": rpt_bridge.safe_rpt_name(file), "selection": f"{{RptUDR.RptName}} = '{name[:150]}' and {{RptUDR.UserName}} = '{tag}'",
           "parameters": {"compname": c.get("name", ""), "add1": addr[0], "add2": addr[1], "add3": addr[2], "add4": addr[3]},
           "copies": max(1, min(int(copies or 1), 20)), "printer": printer.strip(), "action": "direct" if printer.strip() else "preview",
           "option": "", "stored_proc": ""}
    if "'" in name:
        raise HTTPException(409, "This report's name can't be used as a print selection.")

    def insert(cur, row: dict, table: str) -> None:
        cols = list(row)
        cur.execute(f"INSERT INTO dbo.{table} ({', '.join(cols)}) VALUES ({', '.join('?' * len(cols))})", tuple(row.values()))

    def clear(cur) -> None:
        cur.execute("DELETE FROM dbo.RptUDR WHERE RptName = ? AND UserName = ?", (name[:150], tag))
        cur.execute("DELETE FROM dbo.RptUDRHeading WHERE RptName = ? AND UserName = ?", (name[:150], tag))

    with _lock:
        try:
            with db.transaction() as cur:
                clear(cur)
                for r in rows:
                    insert(cur, {**blanks, "RptName": name[:150], "UserName": tag, **{f"Fld{i}": _text(r[k]) for i, k in enumerate(keys, 1)}}, "RptUDR")
                insert(cur, head, "RptUDRHeading")
            if job["printer"]:
                rpt_bridge.print_direct(job)
                return None
            return rpt_bridge.render_pdf(job)
        finally:
            try:
                with db.transaction() as cur:
                    clear(cur)
            except Exception:   # noqa: BLE001 - the rows are this app's own; the next run deletes them first
                pass
