"""Builds the print job of a document the way the desktop does, then hands it to the Crystal bridge.

The desktop's frmUtlReportViewer.ShowRptFileFromUtlOptions / PrintInvCrpt (read from the vLQMS source) do this:
1. Find the document's print option in UtlOption (by name, else the DefaultPrint = 'Y' one, else the only one).
2. The report file is the option's CrptFileName (".Rpt" added when missing); a "multi report" option picks the
   file by testing the saved document (MultiRptString1..3, rows split by 'ç', columns by 'å').
3. The record selection formula is the option's SelFormula (back-ticks are quotes) followed by the document's ID,
   e.g. `{TrnHdrSINV.SINVID} = 1234`.
4. The parameters come from FormulasFld / FormulasVal: parallel ';' lists; each value is `Table.Field`, and the
   parameter gets `Select Field from Table` (the company's name, address lines, GSTIN, ...).
5. Preview ('P'), direct print ('D'), e-mail as PDF ('E') or multi print ('M') follow ClickOperation (plain click),
   CtrlClickOperation and CtrlAltClickOperation.
A document with no print option in the designer falls back to PrintInvCrpt: FMTSINV.rpt (or the .rpt the user picked
from the FMTSINV*.rpt files), `{TRNHDRSInv.SInvID} = <id>` and the company parameters.

Documents made through the web app are in the desktop's own tables, so the reports read them as they are
(rpt_bridge.table_prefix is empty unless RPT_TABLE_PREFIX says otherwise); the selection formula and the report's
own fields are used unchanged.
"""
import logging
import re

from fastapi import HTTPException

from . import db, designer, rpt_bridge, sqlstore, store

log = logging.getLogger("uvicorn.error")
IDENT = re.compile(r"^[A-Za-z_][A-Za-z0-9_]*$")
OPERATORS = {"=", "<>", ">", "<", ">=", "<=", "like", "LIKE"}
# PrintInvCrpt's parameters, from MstComp: parameter name -> MstComp column.
COMPANY_PARAMS = {"compname": "CompName", "add1": "Address1", "add2": "Address2", "add3": "Address3",
                  "add4": "Address4", "gstin": "GSTNO", "phno": "TELNO", "email": "EMAIL"}
# MstComp column (any spelling a print option may use) -> Company Master value, used when the column can't be read
# or is blank. The phone is TELNO in MstComp (db_store.load_masters), not PHNO.
COMPANY_FALLBACK = {"CompName": "name", "Address1": "address", "GSTNO": "gstin", "GSTIN": "gstin",
                    "TELNO": "phone", "PHNO": "phone", "PHONE": "phone", "MOBNO": "mobile",
                    "EMAIL": "email", "EMAILID": "email", "WEB": "website", "PANNO": "pan", "CINNO": "cin"}
_COMPANY_FALLBACK_UPPER = {k.upper(): v for k, v in COMPANY_FALLBACK.items()}
DEFAULT_INVOICE_RPT = "FMTSINV.rpt"


def company_values() -> dict[str, str]:
    """The shop's details under every parameter name an invoice format may use (PrintInvCrpt's names first). The bridge
    skips a name the report doesn't define, so extra spellings cost nothing."""
    base = {n: scalar("MstComp", c) for n, c in COMPANY_PARAMS.items()}
    for alias, src in (("emailid", "email"), ("email_id", "email"), ("mailid", "email"), ("emailaddress", "email"),
                       ("phone", "phno"), ("telno", "phno"), ("phoneno", "phno"), ("gstno", "gstin"),
                       ("gstinno", "gstin"), ("companyname", "compname"), ("address1", "add1"), ("address2", "add2"),
                       ("address3", "add3"), ("address4", "add4")):
        base[alias] = base.get(src, "")
    return base


def header_table(short_form: str) -> str:
    return f"TrnHdr{short_form}"


def selection_for(short_form: str) -> str:
    """The desktop's own fallback selection formula (PrintInvCrpt writes {TRNHDRSInv.SInvID} =)."""
    if short_form == "SINV":
        return "{TRNHDRSInv.SInvID} ="
    return f"{{TrnHdr{short_form}.{short_form}ID}} ="


def document_id(kind: str, doc_no: str) -> int:
    """The saved document's ID in TrnHdr<short form> (the report selects by it)."""
    sh = designer.SHORT_FORMS.get(kind)
    if not sh:
        raise HTTPException(422, f"'{kind}' has no print format")
    if not sqlstore.enabled():
        raise HTTPException(409, "Nothing is saved to the database right now (it is offline), so there is no saved "
                                 "document for Crystal Reports to print.")
    rows = db.fetchall(f"SELECT [{sh}ID] AS id FROM dbo.[TrnHdr{sh}] WHERE [{sh}NO] = ?", (doc_no,))
    if not rows:
        raise HTTPException(404, f"{doc_no} is not in the database yet.")
    return int(rows[0]["id"])


def scalar(table: str, field: str) -> str:
    """`Select field from table` on the web's copy of a legacy table (first row), as the desktop's formulas do."""
    if not IDENT.match(table) or not IDENT.match(field):
        return ""
    company = table.lower() == "mstcomp"
    key = _COMPANY_FALLBACK_UPPER.get(field.upper()) if company else None
    from_company = str(store.COMPANY.get(key, "") or "").strip() if key else ""
    if not sqlstore.enabled():
        return from_company
    try:
        rows = db.fetchall(f"SELECT TOP 1 [{field}] AS v FROM dbo.[{rpt_bridge.table_prefix()}{table}]")
    except Exception:   # noqa: BLE001 - a column this database doesn't have (e.g. PHNO for TELNO) must not blank the print
        return from_company
    value = "" if not rows or rows[0]["v"] is None else str(rows[0]["v"]).strip()
    return value or from_company


def parameters(fields: str, values: str) -> dict[str, str]:
    """FormulasFld / FormulasVal ('a;b' and 'Table.Field;Table.Field') -> {parameter: value}."""
    out: dict[str, str] = {}
    names = [f.strip().replace("`", "'") for f in (fields or "").split(";")] if (fields or "").strip() else []
    sources = [v.strip() for v in (values or "").split(";")]
    for i, name in enumerate(names):
        name = name.rstrip("=").strip()
        src = sources[i] if i < len(sources) else ""
        table, dot, field = src.partition(".")
        out[name] = scalar(table, field) if dot else src
    return out


def multi_rpt_name(option: dict, short_form: str, doc_id: int) -> str:
    """The report file a "multi report" option picks for this document (the desktop's getRptName)."""
    text = "".join(str(option.get(f"MultiRptString{n}") or "") for n in (1, 2, 3))
    rows = [r for r in text.split("ç")][:-1] if "ç" in text else []
    for row in rows:
        cols = row.split("å")
        if len(cols) < 5:
            continue
        rpt, fld, op, val = cols[1], cols[2], cols[3].strip(), cols[4]
        table = f"TrnHdr{short_form}" if "#h." in fld else f"TrnDtl{short_form}"
        fld = fld.replace("#", "").replace("h.", "").replace("d.", "")
        if op not in OPERATORS or not IDENT.match(fld):
            continue
        hit = db.fetchall(f"SELECT 1 AS x FROM dbo.[{rpt_bridge.table_prefix()}{table}] "
                          f"WHERE [{short_form}ID] = ? AND [{fld}] {op} ?", (doc_id, val))
        if hit:
            return rpt
    raise HTTPException(409, "Combination not found, cannot open report")


def build(kind: str, doc_no: str, option_name: str = "", click: str = "N", fallback_rpt: str = "") -> dict:
    """The job for the bridge: {rpt, selection, parameters, copies, printer, action, option}.
    `click` is the way the user asked: 'N' plain click / Print button, 'C' Ctrl+click, 'A' Ctrl+Alt+click."""
    sh = designer.SHORT_FORMS.get(kind)
    if not sh:
        raise HTTPException(422, f"'{kind}' has no print format")
    doc_id = document_id(kind, doc_no)
    doc = designer.doc_for(sh)
    option = None
    if doc:
        option = designer.option_named(int(doc["DocID"]), option_name) if option_name else designer.default_option(int(doc["DocID"]))
        if option_name and option is None:
            raise HTTPException(404, f"The print option '{option_name}' is not set up for this document in the Doc Designer.")

    if option is None:   # nothing designed: PrintInvCrpt's file mode
        if designer.print_from_stored_proc() and kind == "sales_invoice":
            return {"rpt": "fmtSPInvPrint.rpt", "selection": "", "parameters": {"@mInvID": doc_id}, "copies": 1,
                    "printer": "", "action": "preview", "option": "", "stored_proc": "mSP_PrintInv"}
        rpt = fallback_rpt or (DEFAULT_INVOICE_RPT if kind == "sales_invoice" else "")
        if not rpt:
            raise HTTPException(404, "No print format is set up for this document (Doc Designer → Print options).")
        params = {n: scalar("MstComp", c) for n, c in COMPANY_PARAMS.items()} if kind == "sales_invoice" else {}
        return {"rpt": rpt, "selection": f"{selection_for(sh)} {doc_id}",
                "parameters": params, "copies": 1, "printer": "", "action": "preview", "option": "", "stored_proc": ""}

    rpt = str(option.get("CrptFileName") or "").strip()
    if str(option.get("MultiRpt") or "N").upper() == "Y":
        rpt = multi_rpt_name(option, sh, doc_id)
    sel = str(option.get("SelFormula") or "").replace("`", "'").strip()
    action_code = {"N": option.get("ClickOperation"), "C": option.get("CtrlClickOperation"),
                   "A": option.get("CtrlAltClickOperation")}.get(click, "")
    action = designer.CLICK_ACTIONS.get(str(action_code or "").strip(), "") or "preview"
    params = parameters(option.get("FormulasFld"), option.get("FormulasVal"))
    if kind == "sales_invoice":   # the print option may map too little (or blank): fill in the shop's details the way PrintInvCrpt does
        have = {k.lower().lstrip("@"): v for k, v in params.items()}
        for name, value in company_values().items():
            if not str(have.get(name, "")).strip() and value:
                params[name] = value
    log.info("print %s %s: %s %s", kind, doc_no, rpt, {k: (v if len(str(v)) < 40 else str(v)[:40] + "…") for k, v in params.items()})
    return {
        "rpt": rpt,
        "selection": f"{sel} {doc_id}" if sel else "",
        "parameters": params,
        "copies": max(1, int(float(option.get("NoOfCopies") or 1))),
        "printer": str(option.get("DirectPrinterName") or "").strip(),
        "action": action, "option": str(option.get("OptionName") or ""), "stored_proc": "",
    }
