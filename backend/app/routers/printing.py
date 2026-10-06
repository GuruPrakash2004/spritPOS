"""Printing: the desktop's Crystal .rpt formats, driven by the Doc Designer's print options (app/designer.py).

- GET  /print/options?kind=   the document's print options (UtlOption) and which one is the default
- PUT  /print/preference      remember this user's last-picked option of a document
- POST /print/dsr             the Daily Summary Report of a day through the desktop's RptDSR.rpt (+ GET /print/dsr-formats)
- POST /print/stock-balance   the Stock Balance report of a period through the desktop's RptStkBal*.rpt (+ GET /print/stock-balance-formats)
- GET  /print/bridge          is the Crystal Reports bridge set up and answering
- POST /print/render          the report as a PDF for the preview tab, or sent to the option's printer (direct print)
- POST /print/email           the report as a PDF, e-mailed with the option's subject, text and recipients
- POST /designer/refresh      read the Doc Designer's latest settings again (Administrator)
Nothing here changes the shop's data, so these POSTs are in main.NOT_WRITES.
"""
import re
import time
from datetime import date

from fastapi import APIRouter, Depends, HTTPException
from fastapi.concurrency import run_in_threadpool
from fastapi.responses import Response
from pydantic import BaseModel

from .. import designer, mailer, rpt_bridge, rpt_dsr, rpt_print, rpt_stock, sqlstore, store
from .auth import get_current_user, require_admin

router = APIRouter(tags=["Printing"])
EMAIL_PER_HOUR = 10   # per user, like Customised Reports → Email
_sent: dict[str, list[float]] = {}
EMAIL = re.compile(r"^[^@\s;,]+@[^@\s;,]+\.[^@\s;,]+$")


class PrintIn(BaseModel):
    kind: str
    doc_no: str
    option: str = ""
    click: str = "N"        # N plain click / Print button, C Ctrl+click, A Ctrl+Alt+click
    rpt: str = ""           # only when the document has no print option: the FMTSINV*.rpt file the user picked
    action: str = ""        # "preview" | "direct": overrides the option's click action (the Print dialog's buttons)
    printer: str = ""       # direct print: this printer instead of the option's own


class EmailIn(PrintIn):
    to: list[str] = []


def _doc_info(kind: str) -> dict:
    sh = designer.SHORT_FORMS.get(kind)
    if not sh:
        raise HTTPException(404, f"'{kind}' has no print format")
    doc = designer.doc_for(sh)
    return {"kind": kind, "short_form": sh, "doc": doc}


@router.get("/print/options")
def print_options(kind: str, user: dict = Depends(get_current_user)):
    info = _doc_info(kind)
    doc = info["doc"]
    options, default = [], None
    if doc:
        d = designer.default_option(int(doc["DocID"]))
        default = str(d["OptionName"]).strip() if d else None
        options = [designer.option_summary(o, default) for o in designer.options_for(int(doc["DocID"]))]
    on_disk = {f.lower() for f in rpt_bridge.rpt_files()}
    mine = store.PRINT_PREFS.get(user["user"], {}).get(kind)
    for o in options:   # a format whose .rpt file isn't in the report folder is listed but can't be used
        o["available"] = (o["rpt"] if o["rpt"].lower().endswith(".rpt") else o["rpt"] + ".rpt").lower() in on_disk
        o["preferred"] = o["name"] == mine
    files = [f for f in rpt_bridge.rpt_files() if kind == "sales_invoice" and f.lower().startswith("fmtsinv")]
    return {
        "kind": kind, "short_form": info["short_form"], "doc_id": int(doc["DocID"]) if doc else None,
        "doc_name": str(doc.get("DocName") or "") if doc else "",
        "designed": bool(options),            # False: the desktop's file mode (FMTSINV*.rpt files)
        "options": options, "default": default, "preferred": mine if any(o["name"] == mine for o in options) else None,
        "files": files,
        "stored_proc": kind == "sales_invoice" and designer.print_from_stored_proc(),
        "bridge_configured": rpt_bridge.configured(),
    }


class PreferenceIn(BaseModel):
    kind: str
    option: str


@router.put("/print/preference")
def set_preference(body: PreferenceIn, user: dict = Depends(get_current_user)):
    """Remember this user's own last-picked print option for a document (it comes before the shared default)."""
    info = _doc_info(body.kind)
    names = [str(o.get("OptionName") or "").strip() for o in designer.options_for(int(info["doc"]["DocID"]))] if info["doc"] else []
    if body.option not in names:
        raise HTTPException(404, f"'{body.option}' is not a print option of this document")
    store.PRINT_PREFS.setdefault(user["user"], {})[body.kind] = body.option
    return {"kind": body.kind, "option": body.option}


class ReportPrintIn(BaseModel):
    on: date
    rpt: str = ""       # one of /print/dsr-formats; empty = RptDSR.rpt
    printer: str = ""   # a printer name: the report goes straight to it (Print) instead of coming back as a PDF (Preview)
    copies: int = 1


@router.get("/print/dsr-formats")
def dsr_formats(user: dict = Depends(get_current_user)):
    """The Daily Summary Report's .rpt formats that are in the report folder."""
    return {"formats": rpt_dsr.formats(), "bridge_configured": rpt_bridge.configured()}


@router.post("/print/dsr")
async def print_dsr(body: ReportPrintIn, user: dict = Depends(get_current_user)):
    """The Daily Summary Report of a day as a PDF, laid out by the desktop's RptDSR.rpt; with `printer` it is printed
    directly on that printer and the reply is JSON."""
    pdf = await run_in_threadpool(rpt_dsr.render, body.on, user, body.rpt, body.printer, body.copies)
    if pdf is None:
        return {"printed": True, "printer": body.printer, "copies": body.copies, "rpt": body.rpt or rpt_dsr.FILES[0]}
    pdf, size = await run_in_threadpool(rpt_bridge.preview_pdf, pdf)
    return Response(pdf, media_type="application/pdf", headers={
        "Content-Disposition": f'inline; filename="DSR_{body.on.isoformat()}.pdf"', "X-Page-Size": size})


class StockPrintIn(BaseModel):
    from_date: date
    to_date: date
    group: str | None = None
    rpt: str = ""
    printer: str = ""
    copies: int = 1


@router.get("/print/stock-balance-formats")
def stock_balance_formats(user: dict = Depends(get_current_user)):
    """The Stock Balance .rpt formats that are in the report folder."""
    return {"formats": rpt_stock.formats(), "bridge_configured": rpt_bridge.configured()}


@router.post("/print/stock-balance")
async def print_stock_balance(body: StockPrintIn, user: dict = Depends(get_current_user)):
    """The Stock Balance report of a period as a PDF, laid out by the desktop's RptStkBal*.rpt; with `printer` it is
    printed directly on that printer and the reply is JSON."""
    pdf = await run_in_threadpool(rpt_stock.render, body.from_date, body.to_date, body.group, user, body.rpt, body.printer, body.copies)
    if pdf is None:
        return {"printed": True, "printer": body.printer, "copies": body.copies, "rpt": body.rpt or rpt_stock.FILES[0]}
    pdf, size = await run_in_threadpool(rpt_bridge.preview_pdf, pdf)
    return Response(pdf, media_type="application/pdf", headers={
        "Content-Disposition": f'inline; filename="StockBalance_{body.from_date.isoformat()}_{body.to_date.isoformat()}.pdf"',
        "X-Page-Size": size})


@router.get("/print/bridge")
async def bridge_status(user: dict = Depends(get_current_user)):
    return await run_in_threadpool(rpt_bridge.status)


def _job(body: PrintIn) -> dict:
    job = rpt_print.build(body.kind, body.doc_no, body.option, body.click, body.rpt)
    if body.action in ("preview", "direct"):
        job["action"] = body.action
    if body.printer.strip():
        job["printer"] = body.printer.strip()
    return job


@router.post("/print/render")
async def render(body: PrintIn, user: dict = Depends(get_current_user)):
    """PDF bytes for a preview; for a direct print the report goes to the option's printer and the reply is JSON."""
    job = await run_in_threadpool(_job, body)
    if job["action"] == "direct":
        await run_in_threadpool(rpt_bridge.print_direct, job)
        return {"printed": True, "printer": job["printer"], "copies": job["copies"], "rpt": job["rpt"]}
    if job["action"] == "email":
        raise HTTPException(409, "This print option e-mails the document: use the Email button.")
    pdf = await run_in_threadpool(rpt_bridge.render_pdf, job)
    name = re.sub(r"[^A-Za-z0-9._-]+", "_", body.doc_no) + ".pdf"
    pdf, size = await run_in_threadpool(rpt_bridge.preview_pdf, pdf)
    return Response(pdf, media_type="application/pdf", headers={
        "Content-Disposition": f'inline; filename="{name}"', "X-Print-Rpt": job["rpt"], "X-Print-Option": job["option"],
        "X-Page-Size": size})


def _mail_text(text: str, sh: str, doc_no: str) -> str:
    """The designer's <<SINVNO>>-style placeholders: the document number (the other tokens stay as written)."""
    return (text or "").replace(f"<<{sh}NO>>", doc_no)


@router.post("/print/email")
async def email(body: EmailIn, user: dict = Depends(get_current_user)):
    if not mailer.configured():
        raise HTTPException(409, "Email isn't set up on this server (SMTP_HOST in backend/.env).")
    now = time.time()
    recent = [t for t in _sent.get(user["user"], []) if now - t < 3600]
    if len(recent) >= EMAIL_PER_HOUR:
        raise HTTPException(429, f"At most {EMAIL_PER_HOUR} documents can be e-mailed per hour.")
    _sent[user["user"]] = recent
    info = _doc_info(body.kind)
    doc = info["doc"]
    option = designer.option_named(int(doc["DocID"]), body.option) if doc and body.option else (
        designer.default_option(int(doc["DocID"])) if doc else None)
    to = [t.strip() for t in body.to if t.strip()]
    if option:
        for row in designer.email_recipients(int(doc["DocID"]), int(option.get("OptionOrder") or 0), "T"):
            if str(row.get("FixedEmail") or "").upper() == "Y" and row.get("EmailID"):
                to.append(str(row["EmailID"]).strip())
    bad = [t for t in to if not EMAIL.match(t)]
    if bad or not to:
        raise HTTPException(422, "Enter the e-mail address to send to." if not to else f"Not a valid e-mail address: {bad[0]}")
    job = await run_in_threadpool(_job, body)
    pdf = await run_in_threadpool(rpt_bridge.render_pdf, job)
    sh = info["short_form"]
    subject = _mail_text(str(option.get("EmailSubject") or "") if option else "", sh, body.doc_no) or f"{doc['DocName'] if doc else 'Document'} {body.doc_no}"
    text = "\n".join(x for x in (_mail_text(str(option.get("EmailBody") or ""), sh, body.doc_no) if option else "",
                                 _mail_text(str(option.get("EmailFooter") or ""), sh, body.doc_no) if option else "") if x)
    name = re.sub(r"[^A-Za-z0-9._-]+", "_", body.doc_no) + ".pdf"
    try:
        await run_in_threadpool(mailer.send, list(dict.fromkeys(to)), subject, text or subject, name, pdf,
                                store.COMPANY.get("email", ""), store.COMPANY.get("name", ""), "pdf")
    except Exception as e:
        raise HTTPException(502, f"Could not send the e-mail: {e.__class__.__name__}")
    _sent[user["user"]].append(now)
    return {"sent": True, "to": to}


@router.post("/designer/refresh")
async def refresh_designer(admin: dict = Depends(require_admin)):
    """Read the Doc Designer's tables again."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    try:
        copied = await run_in_threadpool(designer.refresh)
    except Exception as e:
        raise HTTPException(503, f"Could not read the Doc Designer settings: {e}")
    return {"tables": copied}
