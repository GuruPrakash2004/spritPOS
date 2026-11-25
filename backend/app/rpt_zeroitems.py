"""Print Zero Stock Items through the desktop's own RptZBItem.rpt (Crystal Reports bridge).

The desktop (frmRptStockBal.btnPrintZeroStk_Click) opens RptZBItem.rpt with NO selection formula and only the shop's
details as parameters (compname, add1..add4, phno, gstin), so the report reads its zero-stock items straight from the
database itself. Nothing is written to the database here: the job is just the file and the parameters
(rpt_bridge.company_parameters() already supplies them for every job).
"""
from fastapi import HTTPException

from . import rpt_bridge, sqlstore, store

FILES = ("RptZBItem.rpt",)


def formats() -> list[str]:
    """The zero-stock format that is in the report folder (none until RptZBItem.rpt is copied there)."""
    on_disk = {f.lower(): f for f in rpt_bridge.rpt_files()}
    return [on_disk[f.lower()] for f in FILES if f.lower() in on_disk]


def option() -> dict | None:
    """The Print Settings entry (a UtlOption row, Global Settings -> Print Settings) whose report file is RptZBItem.rpt: its printer and
    number of copies are this print's settings. The desktop has no such row (its button opens the report directly), so one is added by
    Print Settings -> Add option with the report file RptZBItem.rpt; an inactive one is ignored."""
    for o in store.UTL_OPTIONS:
        if str(o.get("CrptFileName") or "").strip().lower() in {f.lower() for f in FILES} and str(o.get("InActive") or "N").upper() != "Y":
            return {"name": str(o.get("OptionName") or ""), "printer": str(o.get("DirectPrinterName") or "").strip(),
                    "copies": max(1, int(float(o.get("NoOfCopies") or 1)))}
    return None


def render(rpt: str = "", printer: str = "", copies: int = 1) -> bytes | None:
    """The list as a PDF laid out by Crystal; with a `printer` it is sent straight there (nothing returned)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "The database is offline, so Crystal Reports has nothing to read.")
    have = formats()
    if not have:
        raise HTTPException(404, "RptZBItem.rpt is not in the report folder. Copy it from the desktop's report folder into backend/Rpts.")
    name = rpt_bridge.safe_rpt_name(rpt or have[0])
    if name.lower() not in {f.lower() for f in FILES}:
        raise HTTPException(422, f"'{name}' is not a Zero Stock Items format")
    opt = option()
    n = int(copies or 1)
    if n <= 1 and opt:   # the Print Settings entry's copies, unless the caller asked for more
        n = opt["copies"]
    job = {"rpt": name, "selection": "", "parameters": {}, "copies": max(1, min(n, 20)), "printer": printer.strip(),
           "action": "direct" if printer.strip() else "preview", "option": "", "stored_proc": ""}
    if job["printer"]:
        rpt_bridge.print_direct(job)
        return None
    return rpt_bridge.render_pdf(job)
