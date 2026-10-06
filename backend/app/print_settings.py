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
    }


def listing() -> dict:
    """Every document that has print options, with its options (inactive ones too)."""
    by_doc: dict[int, list[dict]] = {}
    for o in store.UTL_OPTIONS:
        by_doc.setdefault(int(o["DocID"]), []).append(_option(o))
    docs = []
    for doc_id, options in sorted(by_doc.items(), key=lambda kv: str((store.UTL_DOCS.get(kv[0]) or {}).get("DocName") or kv[0])):
        d = store.UTL_DOCS.get(doc_id) or {}
        docs.append({"doc_id": doc_id, "name": str(d.get("DocName") or f"Document {doc_id}"),
                     "short_form": str(d.get("DocShForm") or ""), "options": sorted(options, key=lambda x: x["order"])})
    return {"documents": docs, "clicks": CLICKS, "after_save": AFTER_SAVE}


def update(doc_id: int, order: int, body: dict, user: str) -> dict:
    """Change one print option. `body` has the fields to change (name, copies, default, click, ctrl_click,
    ctrl_alt_click, after_save, printer, inactive)."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    row = next((o for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id and int(o.get("OptionOrder") or 0) == order), None)
    if row is None:
        raise HTTPException(404, "That print option does not exist.")
    now = _option(row)
    new = {**now, **{k: v for k, v in body.items() if v is not None}}
    name = str(new["name"]).strip()
    if not name or len(name) > 50:
        raise HTTPException(422, "The option name is required (50 characters at most).")
    if any(o is not row and int(o["DocID"]) == doc_id and str(o.get("OptionName") or "").strip().lower() == name.lower()
           for o in store.UTL_OPTIONS):
        raise HTTPException(422, f"This document already has an option called '{name}'.")
    if not 1 <= int(new["copies"]) <= 20:
        raise HTTPException(422, "Copies must be between 1 and 20.")
    for key in ("click", "ctrl_click", "ctrl_alt_click"):
        if new[key] not in CLICKS:
            raise HTTPException(422, f"'{new[key]}' is not a valid action.")
    if new["after_save"] not in AFTER_SAVE:
        raise HTTPException(422, "After save must be: Do not print, Ask first or Print at once.")
    printer = str(new["printer"]).strip()
    if len(printer) > 50:
        raise HTTPException(422, "The printer name is at most 50 characters.")
    if new["default"] and new["inactive"]:
        raise HTTPException(422, "The default option must be in use.")
    one = lambda c: (c or " ")[:1]   # noqa: E731 - CHAR(1) columns hold a space for "nothing"
    with db.transaction() as cur:
        if new["default"]:   # one default per document
            cur.execute("UPDATE dbo.UtlOption SET DefaultPrint = 'N' WHERE DocID = ? AND OptionOrder <> ?", (doc_id, order))
        cur.execute(
            "UPDATE dbo.UtlOption SET OptionName = ?, NoOfCopies = ?, DefaultPrint = ?, ClickOperation = ?, "
            "CtrlClickOperation = ?, CtrlAltClickOperation = ?, AfterSaveAction = ?, DirectPrinterName = ?, InActive = ? "
            "WHERE DocID = ? AND OptionOrder = ?",
            (name, int(new["copies"]), "Y" if new["default"] else "N", one(new["click"]), one(new["ctrl_click"]),
             one(new["ctrl_alt_click"]), new["after_save"], printer, "Y" if new["inactive"] else "N", doc_id, order))
    designer.load()
    changed = [f"{k}: {now[k]} -> {new[k]}" for k in ("name", "copies", "default", "click", "ctrl_click", "ctrl_alt_click",
                                                      "after_save", "printer", "inactive") if now[k] != new[k]]
    doc = str((store.UTL_DOCS.get(doc_id) or {}).get("DocName") or doc_id)
    admin_audit.record("Print Settings", "Edited", f"{doc} / {now['name']}", "", "; ".join(changed))
    return next(_option(o) for o in store.UTL_OPTIONS if int(o["DocID"]) == doc_id and int(o.get("OptionOrder") or 0) == order)
