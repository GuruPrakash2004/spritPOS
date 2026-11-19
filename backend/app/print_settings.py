"""Global Settings → Print Settings (the desktop's Other Settings → Print Settings): how each document's print formats behave.

The desktop edits the Doc Designer's UtlOption rows from this screen, and the web app prints from the same rows
(app/designer.py), so a change made here shows in the desktop too, and the other way round. Per print option:
  - what a plain click on Print, a Ctrl+click and a Ctrl+Alt+click do (Preview / Direct print / E-mail / Multi print),
  - what happens after the document is saved (Do not print / Ask first / Print at once),
  - the printer for a direct print, the number of copies, which option is the default, and whether it is in use.
The report file (CrptFileName), selection formula and parameters belong to the Doc Designer and are shown, not edited.
Administrator only. Nothing else in the database is touched.
"""
from fastapi import HTTPException

from . import admin_audit, db, designer, sqlstore, store

CLICKS = {"P": "Preview", "D": "Direct print", "E": "E-mail", "M": "Multi print", "": "Nothing"}
AFTER_SAVE = {"DNP": "Do not print", "P": "Ask first", "PP": "Print at once"}


def _after(code: str) -> str:
    c = (code or "").strip().upper()
    return c if c in AFTER_SAVE else ("DNP" if not c else c)


def _option(o: dict) -> dict:
    return {
        "doc_id": int(o["DocID"]), "order": int(o.get("OptionOrder") or 0),
        "name": str(o.get("OptionName") or ""), "rpt": str(o.get("CrptFileName") or ""),
        "selection": str(o.get("SelFormula") or ""), "copies": int(float(o.get("NoOfCopies") or 1)),
        "default": str(o.get("DefaultPrint") or "N").upper() == "Y",
        "inactive": str(o.get("InActive") or "N").upper() == "Y",
        "click": str(o.get("ClickOperation") or "").strip().upper(),
        "ctrl_click": str(o.get("CtrlClickOperation") or "").strip().upper(),
        "ctrl_alt_click": str(o.get("CtrlAltClickOperation") or "").strip().upper(),
        "after_save": _after(str(o.get("AfterSaveAction") or "")),
        "printer": str(o.get("DirectPrinterName") or "").strip(),
        "multi_rpt": str(o.get("MultiRpt") or "N").upper() == "Y",
        # the desktop's Print Option Name window (FrmUtlPrintOptionname): e-mail and multi-print details
        "email_routing": str(o.get("EmailRouting") or "").strip().upper(),
        "attach_copy": str(o.get("DoNotAttach") or "").strip().upper() == "Y",
        "email_subject": str(o.get("EmailSubject") or ""), "email_body": str(o.get("EmailBody") or ""),
        "email_footer": str(o.get("EmailFooter") or ""), "email_cond": str(o.get("EmailCond") or ""),
        "allow_date_range": str(o.get("AllowDateRange") or "N").upper() == "Y",
        "allow_docno_range": str(o.get("AllowDocNoRange") or "N").upper() == "Y",
        "track_printed": str(o.get("TrackPrinted") or "N").upper() == "Y",
        "multi_query": str(o.get("MultiPrintQuery") or ""),
        "emails": email_rows(int(o["DocID"]), int(o.get("OptionOrder") or 0)),
    }


def email_rows(doc_id: int, order: int) -> list[dict]:
    """UtlOptionEmailDtls of one option: Fixed (Y: a typed address; N: a field of the document with a condition), type To/Cc/Bcc."""
    rows = [e for e in store.UTL_OPTION_EMAILS if int(e.get("DocID") or 0) == doc_id and int(e.get("OptionOrder") or 0) == order]
    return [{"fixed": str(e.get("FixedEmail") or "N").strip().upper() == "Y", "type": str(e.get("eMailType") or "").strip().upper(),
             "email": str(e.get("EmailID") or ""), "field": str(e.get("TxnFldName") or ""), "cond": str(e.get("TxnCondition") or "")}
            for e in sorted(rows, key=lambda e: int(e.get("Slno") or 0))]


def listing() -> dict:
    """Every document that has print options, with its options (inactive ones too)."""
    designer.ensure_loaded()
    by_doc: dict[int, list[dict]] = {}
    for o in store.UTL_OPTIONS:
        by_doc.setdefault(int(o["DocID"]), []).append(_option(o))
    docs = []
    for doc_id, options in sorted(by_doc.items(), key=lambda kv: str((store.UTL_DOCS.get(kv[0]) or {}).get("DocName") or kv[0])):
        d = store.UTL_DOCS.get(doc_id) or {}
        docs.append({"doc_id": doc_id, "name": str(d.get("DocName") or f"Document {doc_id}"),
                     "short_form": str(d.get("DocShForm") or ""), "options": sorted(options, key=lambda x: x["order"])})
    return {"documents": docs, "clicks": CLICKS, "after_save": AFTER_SAVE}


def _find(doc_id: int, order: int) -> dict:
    row = next((o for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id and int(o.get("OptionOrder") or 0) == order), None)
    if row is None:
        raise HTTPException(404, "That print option does not exist.")
    return row


FIELDS = ("name", "rpt", "selection", "copies", "default", "inactive", "click", "ctrl_click", "ctrl_alt_click", "after_save", "printer",
          "email_routing", "attach_copy", "email_subject", "email_body", "email_footer", "email_cond",
          "allow_date_range", "allow_docno_range", "track_printed", "multi_query")


def _check(new: dict, doc_id: int, skip: dict | None) -> None:
    """The desktop's checks (FrmUtlPrintOptionname.btnSave_Click) plus the web's own limits."""
    name = str(new["name"]).strip()
    if not name or len(name) > 50:
        raise HTTPException(422, "The option name is required (50 characters at most).")
    if any(o is not skip and int(o["DocID"]) == doc_id and str(o.get("OptionName") or "").strip().lower() == name.lower()
           for o in store.UTL_OPTIONS):
        raise HTTPException(422, "option Name Already Exist!")
    if not new["click"]:
        raise HTTPException(422, "Invalid Print Click....")
    if not 1 <= int(new["copies"]) <= 20:
        raise HTTPException(422, "Copies must be between 1 and 20.")
    for key in ("click", "ctrl_click", "ctrl_alt_click"):
        if new[key] not in CLICKS:
            raise HTTPException(422, f"'{new[key]}' is not a valid action.")
    if new["after_save"] not in AFTER_SAVE:
        raise HTTPException(422, "After save must be: Do not print, Ask first or Print at once.")
    if len(str(new["printer"]).strip()) > 50:
        raise HTTPException(422, "The printer name is at most 50 characters.")
    if len(str(new["rpt"]).strip()) > 100:
        raise HTTPException(422, "The report file name is at most 100 characters.")
    if new["default"] and new["inactive"]:
        raise HTTPException(422, "The default option must be in use.")
    if new["email_routing"] not in ("", "O", "S", "N"):
        raise HTTPException(422, "E-mail routing must be Outlook or SMTP.")


def _kinds(new: dict) -> tuple[bool, bool]:
    """(some click e-mails, some click is multi print): the desktop keeps the e-mail / multi-print columns only then."""
    clicks = {new["click"], new["ctrl_click"], new["ctrl_alt_click"]}
    return "E" in clicks, "M" in clicks


def _write_emails(cur, doc_id: int, order: int, rows: list[dict]) -> None:
    cur.execute("DELETE FROM dbo.UtlOptionEmailDtls WHERE DocID = ? AND OptionOrder = ?", (doc_id, order))
    n = 0
    for r in rows:
        kind = str(r.get("type") or "").strip().upper()
        if not kind:
            continue   # the desktop drops rows with no To / Cc / Bcc type
        n += 1
        fixed = bool(r.get("fixed"))
        cur.execute("INSERT INTO dbo.UtlOptionEmailDtls (DocID, OptionOrder, Slno, FixedEmail, eMailType, EmailID, TxnFldName, TxnCondition) "
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                    (doc_id, order, n, "Y" if fixed else "N", kind, str(r.get("email") or "") if fixed else "",
                     "" if fixed else str(r.get("field") or ""), "" if fixed else str(r.get("cond") or "")))


def _values(new: dict) -> list:
    has_email, has_multi = _kinds(new)
    one = lambda c: (c or " ")[:1]   # noqa: E731 - CHAR(1) columns hold a space for "nothing"
    return [str(new["name"]).strip(), str(new["rpt"]).strip(), str(new["selection"]).strip(), int(new["copies"]),
            "Y" if new["default"] else "N", str(new["printer"]).strip(), new["after_save"],
            one(new["click"]), one(new["ctrl_click"]), one(new["ctrl_alt_click"]),
            (new["email_routing"] or "S") if has_email else "N", ("Y" if new["attach_copy"] else "N") if has_email else "",
            new["email_subject"] if has_email else "", new["email_body"] if has_email else "",
            new["email_footer"] if has_email else "", new["email_cond"] if has_email else "",
            "Y" if has_multi and new["allow_date_range"] else "N", "Y" if has_multi and new["allow_docno_range"] else "N",
            "Y" if has_multi and new["track_printed"] else "N", str(new["multi_query"]).strip() if has_multi else "",
            "Y" if new["inactive"] else "N"]


def update(doc_id: int, order: int, body: dict, user: str) -> dict:
    """Change one print option (any of FIELDS, plus `emails`, its UtlOptionEmailDtls rows)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    row = _find(doc_id, order)
    now = _option(row)
    new = {**now, **{k: v for k, v in body.items() if v is not None and k in FIELDS}}
    emails = body.get("emails")
    _check(new, doc_id, row)
    v = _values(new)
    with db.transaction() as cur:
        if new["default"]:   # one default per document
            cur.execute("UPDATE dbo.UtlOption SET DefaultPrint = 'N' WHERE DocID = ? AND OptionOrder <> ?", (doc_id, order))
        cur.execute(
            "UPDATE dbo.UtlOption SET OptionName = ?, CrptFileName = ?, SelFormula = ?, NoOfCopies = ?, DefaultPrint = ?, DirectPrinterName = ?, "
            "AfterSaveAction = ?, ClickOperation = ?, CtrlClickOperation = ?, CtrlAltClickOperation = ?, EmailRouting = ?, DoNotAttach = ?, "
            "EmailSubject = ?, EmailBody = ?, EmailFooter = ?, EmailCond = ?, AllowDateRange = ?, AllowDocNoRange = ?, TrackPrinted = ?, "
            "MultiPrintQuery = ?, InActive = ? WHERE DocID = ? AND OptionOrder = ?", (*v, doc_id, order))
        if not _kinds(new)[0]:
            _write_emails(cur, doc_id, order, [])      # no click e-mails any more: the desktop clears the recipient rows
        elif emails is not None:
            _write_emails(cur, doc_id, order, emails)
    designer.load()
    changed = [f"{k}: {now[k]} -> {new[k]}" for k in FIELDS if now[k] != new[k]]
    doc = str((store.UTL_DOCS.get(doc_id) or {}).get("DocName") or doc_id)
    admin_audit.record("Print Settings", "Edited", f"{doc} / {now['name']}", "", "; ".join(changed))
    return next(_option(o) for o in store.UTL_OPTIONS
                if int(o["DocID"]) == doc_id and str(o.get("OptionName") or "").strip() == str(new["name"]).strip())


def create(doc_id: int, body: dict, user: str) -> dict:
    """Add a print option to a document (the desktop's Add): OptionOrder = the highest + 1; the selection formula starts as {TrnHdr<sh>.<sh>ID}=."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    doc = store.UTL_DOCS.get(doc_id)
    if doc is None:
        raise HTTPException(404, "Selected Document Not Found in Transaction List!")
    sh = str(doc.get("DocShForm") or "").strip()
    base = {"name": "", "rpt": "", "selection": f"{{TrnHdr{sh}.{sh}ID}}=" if sh else "", "copies": 1, "default": False, "inactive": False,
            "click": "", "ctrl_click": "", "ctrl_alt_click": "", "after_save": "DNP", "printer": "", "email_routing": "S", "attach_copy": False,
            "email_subject": "", "email_body": "", "email_footer": "", "email_cond": "", "allow_date_range": False, "allow_docno_range": False,
            "track_printed": False, "multi_query": ""}
    new = {**base, **{k: v for k, v in body.items() if v is not None and k in FIELDS}}
    _check(new, doc_id, None)
    v = _values(new)
    order = max([int(o.get("OptionOrder") or 0) for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id] or [0]) + 1
    with db.transaction() as cur:
        if new["default"]:
            cur.execute("UPDATE dbo.UtlOption SET DefaultPrint = 'N' WHERE DocID = ?", (doc_id,))
        cur.execute(
            "INSERT INTO dbo.UtlOption (DocID, OptionOrder, OptionName, CrptFileName, SelFormula, FormulasFld, FormulasVal, NoOfCopies, DefaultPrint, "
            "DirectPrinterName, AfterSaveAction, ClickOperation, CtrlClickOperation, CtrlAltClickOperation, EmailRouting, DoNotAttach, EmailSubject, "
            "EmailBody, EmailFooter, EmailCond, AllowDateRange, AllowDocNoRange, TrackPrinted, MultiPrintQuery, InActive) "
            "VALUES (?, ?, ?, ?, ?, '', '', ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", (doc_id, order, *v))
        if _kinds(new)[0]:
            _write_emails(cur, doc_id, order, body.get("emails") or [])
    designer.load()
    admin_audit.record("Print Settings", "Added", f"{doc.get('DocName') or doc_id} / {new['name']}", "", f"click {new['click']}, copies {new['copies']}")
    return next(_option(o) for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id and int(o.get("OptionOrder") or 0) == order)


def delete(doc_id: int, order: int, user: str) -> None:
    """Delete a print option ("Deleted Successfully!"). Its recipient rows go too, so a later option that takes the same order doesn't inherit them."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    row = _find(doc_id, order)
    name = str(row.get("OptionName") or "")
    with db.transaction() as cur:
        cur.execute("DELETE FROM dbo.UtlOptionEmailDtls WHERE DocID = ? AND OptionOrder = ?", (doc_id, order))
        cur.execute("DELETE FROM dbo.UtlOption WHERE DocID = ? AND OptionOrder = ?", (doc_id, order))
    designer.load()
    admin_audit.record("Print Settings", "Deleted", f"{(store.UTL_DOCS.get(doc_id) or {}).get('DocName') or doc_id} / {name}", name, "")


def reorder(doc_id: int, names: list[str], user: str) -> None:
    """Alter Order: the document's options in the new order (every option named once), numbered 1..n ("Saved Successfully!")."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    have = [o for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id]
    if sorted(n.strip().lower() for n in names) != sorted(str(o.get("OptionName") or "").strip().lower() for o in have):
        raise HTTPException(422, "The order must list every option of the document once.")
    old = {str(o.get("OptionName") or "").strip().lower(): int(o.get("OptionOrder") or 0) for o in have}
    with db.transaction() as cur:
        # recipient rows follow their option: park them (and the options) on a temporary order first, then number 1..n
        for i, n in enumerate(names, 1):
            cur.execute("UPDATE dbo.UtlOption SET OptionOrder = ? WHERE DocID = ? AND OptionOrder = ?", (1000 + i, doc_id, old[n.strip().lower()]))
            cur.execute("UPDATE dbo.UtlOptionEmailDtls SET OptionOrder = ? WHERE DocID = ? AND OptionOrder = ?", (1000 + i, doc_id, old[n.strip().lower()]))
        for i in range(1, len(names) + 1):
            cur.execute("UPDATE dbo.UtlOption SET OptionOrder = ? WHERE DocID = ? AND OptionOrder = ?", (i, doc_id, 1000 + i))
            cur.execute("UPDATE dbo.UtlOptionEmailDtls SET OptionOrder = ? WHERE DocID = ? AND OptionOrder = ?", (i, doc_id, 1000 + i))
    designer.load()
    admin_audit.record("Print Settings", "Order changed", str((store.UTL_DOCS.get(doc_id) or {}).get("DocName") or doc_id), "", ", ".join(names))
