"""What the desktop's Doc Designer (vDocDesigner2020) stores in the database, read for the web app.

The Doc Designer and the desktop Utilities write the screen definitions into the `Utl*` tables; the desktop reads
them every time it opens a screen, so a change made in the designer shows at once. The web app follows the same
tables directly (loaded into memory):

- UtlTransactions  one row per document / master screen (DocID, DocShForm "SINV", header/detail table names, ...)
- UtlOption        the print formats of a document: .rpt file, selection formula, parameters, copies, which one is the
                   default, what a click / Ctrl+click does (preview, direct print, e-mail, multi print), the prompt
                   after saving, the printer and the e-mail text
- UtlOptionEmailDtls  who a print option is e-mailed to
- UtlDocDsgrFlds   the fields of every screen (caption, section, order, hidden, read-only, mandatory, ...)
- UtlMisc          global switches, e.g. InvPrintFromSP (print the invoice from the stored procedure)

`refresh()` copies those tables again from the legacy ones (a read of the legacy tables, as the Database setup does),
so a change the user just made in the Doc Designer arrives without a restart: Utilities → About → Refresh from Doc
Designer, and at every start. Without a database every dict stays empty and the callers fall back to the .rpt files
in the Rpts folder, as the desktop's invoice screen does.
"""
import logging

from . import db, store

logger = logging.getLogger("vlqms.designer")

# The desktop's own short forms (UtlTransactions.DocShForm) of the documents the web app prints.
SHORT_FORMS = {
    "sales_invoice": "SINV", "purchase_bill": "PB", "purchase_indent": "PI", "sales_return": "SR",
    "stock_deduction": "STKDED", "expense": "EXP",
}
CLICK_ACTIONS = {"P": "preview", "D": "direct", "E": "email", "M": "multi", "": ""}


def _rows(table: str) -> list[dict]:
    return db.fetchall(f"SELECT * FROM dbo.[{table}]")


def _clean(row: dict) -> dict:
    """A row with trimmed text (CHAR columns come back padded) and no secrets (SMTP passwords stay in the table)."""
    out = {}
    for k, v in row.items():
        if k.lower() in ("smtppwd", "smtpid"):
            continue
        out[k] = v.strip() if isinstance(v, str) else v
    return out


def load() -> None:
    """Fill store.UTL_* from the designer's tables. Missing tables or a down database leave what is there."""
    try:
        docs = [_clean(r) for r in _rows("UtlTransactions")]
        options = [_clean(r) for r in _rows("UtlOption")]
        emails = [_clean(r) for r in _rows("UtlOptionEmailDtls")]
        fields = [_clean(r) for r in _rows("UtlDocDsgrFlds")]
        misc = [_clean(r) for r in _rows("UtlMisc")]
    except Exception as e:   # no database, or it has no designer tables
        logger.warning("Doc Designer settings not loaded: %s", e)
        return
    store.UTL_DOCS.clear()
    store.UTL_DOCS.update({int(d["DocID"]): d for d in docs if d.get("DocID") is not None})
    store.UTL_OPTIONS.clear()
    store.UTL_OPTIONS.extend(sorted(options, key=lambda o: (int(o.get("DocID") or 0), int(o.get("OptionOrder") or 0))))
    store.UTL_OPTION_EMAILS.clear()
    store.UTL_OPTION_EMAILS.extend(emails)
    store.UTL_FIELDS.clear()
    store.UTL_FIELDS.extend(sorted(fields, key=lambda f: (int(f.get("DocID") or 0), int(f.get("FldOrderBy") or 0))))
    store.UTL_MISC.clear()
    store.UTL_MISC.update(misc[0] if misc else {})


def refresh() -> dict:
    """Read the designer's tables again. Returns the rows read per table."""
    load()
    return {"UtlTransactions": len(store.UTL_DOCS), "UtlOption": len(store.UTL_OPTIONS),
            "UtlOptionEmailDtls": len(store.UTL_OPTION_EMAILS), "UtlDocDsgrFlds": len(store.UTL_FIELDS),
            "UtlMisc": 1 if store.UTL_MISC else 0}


def doc_for(short_form: str) -> dict | None:
    """The UtlTransactions row whose DocShForm is `short_form` (case-insensitive), or None."""
    want = short_form.strip().upper()
    for d in store.UTL_DOCS.values():
        if str(d.get("DocShForm") or "").strip().upper() == want:
            return d
    return None


def options_for(doc_id: int) -> list[dict]:
    """The document's print options in the designer's order, without inactive ones."""
    return [o for o in store.UTL_OPTIONS
            if int(o.get("DocID") or 0) == int(doc_id) and str(o.get("InActive") or "N").upper() != "Y"]


def default_option(doc_id: int) -> dict | None:
    """The option a save prints with: the one marked DefaultPrint = 'Y', else the only option, else none
    (the desktop's strPrintOption logic in the transaction screens)."""
    opts = options_for(doc_id)
    for o in opts:
        if str(o.get("DefaultPrint") or "N").upper() == "Y":
            return o
    return opts[0] if len(opts) == 1 else None


def option_named(doc_id: int, name: str) -> dict | None:
    for o in options_for(doc_id):
        if str(o.get("OptionName") or "").strip().lower() == name.strip().lower():
            return o
    return None


def option_summary(o: dict, default_name: str | None) -> dict:
    """What the browser needs to list a print option (never the SMTP settings)."""
    name = str(o.get("OptionName") or "")
    return {
        "name": name,
        "rpt": str(o.get("CrptFileName") or ""),
        "copies": int(float(o.get("NoOfCopies") or 1)),
        "default": name == default_name,
        "click": CLICK_ACTIONS.get(str(o.get("ClickOperation") or "").strip(), "") or "preview",
        "ctrl_click": CLICK_ACTIONS.get(str(o.get("CtrlClickOperation") or "").strip(), ""),
        "ctrl_alt_click": CLICK_ACTIONS.get(str(o.get("CtrlAltClickOperation") or "").strip(), ""),
        "after_save": str(o.get("AfterSaveAction") or "").strip(),   # "" don't print, "P" ask first, "PP" print at once
        "printer": str(o.get("DirectPrinterName") or "").strip(),
        "multi_rpt": str(o.get("MultiRpt") or "N").upper() == "Y",
    }


def email_recipients(doc_id: int, option_order: int, kind: str) -> list[dict]:
    """UtlOptionEmailDtls rows of one print option for To ('T'), Cc ('C') or Bcc ('B'), in order."""
    rows = [e for e in store.UTL_OPTION_EMAILS
            if int(e.get("DocID") or 0) == int(doc_id) and int(e.get("OptionOrder") or 0) == int(option_order)
            and str(e.get("emailType") or e.get("EmailType") or "").upper() == kind]
    return sorted(rows, key=lambda e: int(e.get("Slno") or e.get("SlNo") or 0))


def fields_for(doc_id: int) -> list[dict]:
    """The screen's fields as the designer defined them, in display order."""
    return [f for f in store.UTL_FIELDS if int(f.get("DocID") or 0) == int(doc_id)]


def print_from_stored_proc() -> bool:
    """UtlMisc.InvPrintFromSP: the invoice prints from fmtSPInvPrint.rpt over the mSP_PrintInv procedure."""
    return str(store.UTL_MISC.get("InvPrintFromSP") or "N").upper() == "Y"
