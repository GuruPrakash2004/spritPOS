"""Global Settings → Other Settings (legacy FrmUtlPrintSettings / UtlOption, FrmUtlTrnSettings / UtlTransactions,
frmUtlDocNumber / frmUtlInvNumberFmt, frmUtlMenuSequence, frmUtlBulkImpFlds), limited to what the web app acts on:

- store.DOC_SETTINGS[kind]: per web document
  - after_save: "none" | "prompt" | "preview" (print after a save; the screens read it), default_format (print
    format id), for the documents that print;
  - allow_duplicate_items, allow_edit_date (the date must be today when off), edit_days (a sales invoice older
    than this can't be cancelled; 0 = no limit), allow_copy_from (Purchase Bill: Copy From indent),
    update_rate_from_pb (Purchase Bill: the item's purchase rate follows the bill).
- store.NUMBER_SERIES[kind]: the document number prefix, and Reset Numbers (never below a number already used).
  Sales invoices take their prefix from the Sales Counter Master, as in the desktop app.
- store.MENU_LAYOUT: modules hidden from the sidebar and module search, and shortcut codes (legacy SCMenuName).
- store.IMPORT_FIELDS["fields"]: Item Master fields an import may overwrite on existing items (empty = all).
Desktop-only options (direct printing to a named printer, Crystal formulas, e-mail routing per print option,
Tally ledgers, grid colours, SMS) are listed in frontend/LEGACY_PARITY.md instead.
"""
import re
from datetime import date

from fastapi import HTTPException

from . import store

# Web document kinds, their screens and default number prefixes.
KINDS = {
    "sales_invoice": ("Sales Invoice", None), "purchase_bill": ("Purchase Bill", "PB"),
    "purchase_indent": ("Purchase Indent", "PI"), "sales_return": ("Sales Return", "SR"),
    "stock_deduction": ("Stock Deduction", "SD"), "expense": ("Expense Entry", "EXP"),
    "closing_stock": ("Closing Stock Entry", "CSE"),
}
PRINTS = {"sales_invoice", "purchase_indent", "sales_return", "stock_deduction"}   # screens with a Print button
AFFECTS_STOCK = {"sales_invoice", "purchase_bill", "sales_return", "stock_deduction"}
DEFAULTS = {"after_save": "none", "default_format": "", "allow_duplicate_items": True, "allow_edit_date": True,
            "edit_days": 0, "allow_copy_from": True, "update_rate_from_pb": False}
PREFIX_RE = re.compile(r"^[A-Za-z0-9]{1,6}$")


def desktop_row(kind: str) -> dict | None:
    """The document's UtlTransactions row (matched on DocName, any case), or None without a database or when the shop has no such row."""
    want = KINDS[kind][0].strip().lower()
    return next((d for d in store.UTL_DOCS.values() if str(d.get("DocName") or "").strip().lower() == want), None)


def _flag(row: dict, col: str) -> bool | None:
    """A Y/N column as a bool, or None when this database has no such column."""
    if col not in row:
        return None
    return str(row[col] or "").strip().upper() == "Y"


def from_desktop(row: dict) -> dict:
    """The settings the web acts on, as the desktop's Transaction Settings window (FrmUtlTrnSettings) holds them in UtlTransactions."""
    out = {}
    for key, col in (("allow_duplicate_items", "BlnAllowDuplicateItems"), ("allow_edit_date", "AllowEditDocDate"), ("allow_copy_from", "AllowCopyFrom")):
        v = _flag(row, col)
        if v is not None:
            out[key] = v
    if "blnFreezeDoc" in row or "AllowEditDeleteForDays" in row:   # the days count only while the freeze flag is on
        out["edit_days"] = int(float(row.get("AllowEditDeleteForDays") or 0)) if _flag(row, "blnFreezeDoc") else 0
    if "RateFldMstItem" in row:
        out["update_rate_from_pb"] = bool(str(row.get("RateFldMstItem") or "").strip())
    return out


def settings(kind: str) -> dict:
    row = desktop_row(kind) if kind in KINDS else None
    return {**DEFAULTS, **store.DOC_SETTINGS.get(kind, {}), **(from_desktop(row) if row else {})}


# Every other UtlTransactions setting of the desktop's Transaction Settings window: (column, caption, kind). The web does not act on these;
# they are shown and saved so the desktop and the web read the same row. kind: flag (Y/N), text, number.
DESKTOP_FIELDS = [
    ("AffectsStock", "Affects stock (Add / Reduce)", "text"),
    ("LinkedPendingParty", "Linked pending: by party", "flag"), ("LinkedPendingSingleDoc", "Linked pending: single document", "flag"),
    ("LinkedPendingDocNo", "Linked pending: by document number", "flag"), ("ShowGraceQty", "Show grace quantity", "flag"),
    ("AuthReq", "Authorisation required", "flag"), ("NoOfAuth", "Number of authorisations", "number"),
    ("NoOfAuthQueryCond", "Authorisation query condition", "text"),
    ("AllowLoadFromBOM", "Allow load from BOM", "flag"), ("AmmReq", "Amendment required", "flag"), ("MultiDocGen", "Multi document generation", "flag"),
    ("AllowTenderChange", "Allow tender change", "flag"), ("TenderChangeFld", "Tender change field", "text"),
    ("OffLineCalc", "Offline calculation", "flag"), ("AllowImpExp", "Allow import / export", "flag"), ("blnEnvPrint", "Envelope print", "flag"),
    ("FileAttachReq", "File attachment required", "flag"), ("PDFPreview", "PDF preview", "flag"),
    ("PartyFilterCond", "Party filter condition", "text"), ("FldBlnEnforcePartyItemLink", "Enforce party-item link", "flag"),
    ("RateFldPartyItem", "Rate field (party item)", "text"), ("RateFldMstItem", "Rate field (item master)", "text"),
    ("ItemFilterCond", "Item filter condition", "text"),
    ("PendingQryFlds", "Pending query: fields", "text"), ("PendingQryTbls", "Pending query: tables", "text"), ("PendingQryCond", "Pending query: condition", "text"),
    ("PickFldsFromSrc", "Pick fields from source", "text"), ("PickFldsFromSrcDocQry", "Pick fields from source document query", "text"),
    ("AccTrn", "Accounts transaction", "flag"), ("AccMstType", "Accounts master type", "text"), ("AcLedFld", "Ledger field", "text"),
    ("AccValueFld", "Voucher value field", "text"),
    ("BalanceColReq", "Balance column required", "flag"), ("BillWiseFeatureReq", "Bill-wise feature", "flag"), ("ChqPrintReq", "Cheque printing", "flag"),
    ("DefFirstRowAmtType", "Default first row amount type", "text"),
]
# What the desktop clears when its switch is off (FrmUtlTrnSettings.btnSave_Click).
_RESETS = {"AuthReq": {"NoOfAuth": 0, "NoOfAuthQueryCond": ""}, "AllowTenderChange": {"TenderChangeFld": ""},
           "FldBlnEnforcePartyItemLink": {"RateFldPartyItem": ""}, "AccTrn": {"AccMstType": "", "AcLedFld": "", "AccValueFld": ""}}
_MAX_TEXT = 2000


def desktop_list() -> dict:
    """Every Transaction / Voucher document with its columns, for the desktop-settings panel."""
    docs = []
    for d in sorted(store.UTL_DOCS.values(), key=lambda d: str(d.get("DocName") or "").lower()):
        if str(d.get("DocType") or "") not in ("Transaction", "Voucher"):
            continue
        values = {}
        for col, _cap, kind in DESKTOP_FIELDS:
            if col in d:
                values[col] = (str(d[col] or "").strip().upper() == "Y") if kind == "flag" else (int(float(d[col] or 0)) if kind == "number" else str(d[col] or ""))
        docs.append({"doc_id": int(d["DocID"]), "name": str(d.get("DocName") or ""), "type": str(d.get("DocType") or ""),
                     "kind": next((k for k in KINDS if KINDS[k][0].strip().lower() == str(d.get("DocName") or "").strip().lower()), None),
                     "values": values})
    return {"docs": docs, "fields": [{"column": c, "caption": cap, "kind": k} for c, cap, k in DESKTOP_FIELDS]}


def desktop_updates(doc_id: int, values: dict) -> dict:
    """Validate `values` (column -> value) for one document and return the columns to write, with the desktop's dependent resets."""
    row = store.UTL_DOCS.get(doc_id)
    if row is None:
        raise HTTPException(404, "Selected Document Not Found in Transaction List!")
    spec = {c: k for c, _cap, k in DESKTOP_FIELDS}
    out: dict = {}
    for col, v in values.items():
        if col not in spec:
            raise HTTPException(422, f"'{col}' is not a Transaction Settings field")
        if col not in row:
            continue   # this database has no such column: nothing to save
        kind = spec[col]
        if kind == "flag":
            out[col] = "Y" if v in (True, "Y", "y") else "N"
        elif kind == "number":
            try:
                n = int(float(v or 0))
            except (TypeError, ValueError):
                raise HTTPException(422, f"{col}: a whole number is needed")
            if not 0 <= n <= 99:
                raise HTTPException(422, f"{col}: from 0 to 99")
            out[col] = n
        else:
            text = str(v or "").strip()
            if len(text) > _MAX_TEXT:
                raise HTTPException(422, f"{col}: {_MAX_TEXT} characters at most")
            out[col] = text
    for switch, cleared in _RESETS.items():
        if out.get(switch) == "N":
            out.update({c: z for c, z in cleared.items() if c in row})
    return out


def all_settings() -> dict:
    return {k: {**settings(k), "screen": KINDS[k][0], "prints": k in PRINTS, "affects_stock": k in AFFECTS_STOCK}
            for k in KINDS}


def check_lines(kind: str, lines: list, on: date | str) -> None:
    """Transaction Settings on a new document: duplicate items and the document date."""
    s = settings(kind)
    if not s["allow_duplicate_items"]:
        codes = [getattr(l, "item_code", None) if not isinstance(l, dict) else l.get("item_code") for l in lines]
        codes = [c for c in codes if c]
        dup = sorted({c for c in codes if codes.count(c) > 1})
        if dup:
            raise HTTPException(422, f"Duplicate Item! {', '.join(dup)} appear more than once "
                                     f"(Global Settings → Transaction Settings for {KINDS[kind][0]})")
    if not s["allow_edit_date"]:
        d = on if isinstance(on, date) else date.fromisoformat(str(on)[:10])
        if d != date.today():
            raise HTTPException(422, f"{KINDS[kind][0]} must be dated today (the document date can't be changed: "
                                     "Global Settings → Transaction Settings)")


def check_edit_days(kind: str, on: str, what: str) -> None:
    days = int(settings(kind)["edit_days"] or 0)
    if days and (date.today() - date.fromisoformat(str(on)[:10])).days > days:
        raise HTTPException(409, f"Edit limit days crossed... Cannot edit! ({what} is more than {days} day(s) old) "
                                 "(Global Settings → Transaction Settings)")


# ─── Document numbers ─────────────────────────────────────────────────────────

def prefix(kind: str) -> str:
    if kind in store.EXE_NUMBERED:   # blank unless the shop chose one (the exe's prefix box)
        return store.NUMBER_SERIES.get(kind, {}).get("prefix") or store.EXE_DEFAULT_PREFIX.get(kind, "")
    return store.NUMBER_SERIES.get(kind, {}).get("prefix") or KINDS[kind][1]


def highest_used(kind: str) -> int:
    """The highest number on any document of this kind (all its prefixes share one counter)."""
    pattern = r"(\d{5})$" if kind in store.EXE_NUMBERED else r"-(\d+)$"
    nums = [int(m.group(1)) for no in store.DOCS.get(kind, {}) if (m := re.search(pattern, no))]
    return max(nums, default=0)


def numbering() -> list[dict]:
    out = []
    for kind, (screen, default) in KINDS.items():
        seq = store._seq.get(kind)
        out.append({"kind": kind, "screen": screen, "prefix": prefix(kind) if default else None,
                    "default_prefix": default, "next_number": seq.value if seq else 1,
                    "example": (f"{store.FY_PREFIX}{prefix(kind)}{(seq.value if seq else 1):05d}" if kind in store.EXE_NUMBERED
                                else f"{store.FY_PREFIX}/{prefix(kind) if default else '<counter prefix>'}-{(seq.value if seq else 1):07d}"),
                    "from_counter": default is None})
    return out


def set_series(kind: str, new_prefix: str | None, next_number: int | None) -> dict:
    if kind not in KINDS:
        raise HTTPException(404, f"Unknown document '{kind}'")
    if new_prefix is not None:
        if KINDS[kind][1] is None:
            raise HTTPException(409, "Sales invoices take their prefix from the Sales Counter Master")
        if not PREFIX_RE.match(new_prefix) and not (kind in store.EXE_NUMBERED and new_prefix == ""):
            raise HTTPException(422, "Prefix: 1 to 6 letters or digits" + (" (or blank)" if kind in store.EXE_NUMBERED else ""))
        store.NUMBER_SERIES.setdefault(kind, {})["prefix"] = new_prefix.upper()   # "" = no prefix (exe-numbered documents)
    if next_number is not None:
        used = highest_used(kind)
        if next_number <= used:
            raise HTTPException(409, f"Number {next_number} is already used (highest is {used}); start from {used + 1} or later")
        store._seq.setdefault(kind, store.Seq(next_number)).value = next_number
    return next(r for r in numbering() if r["kind"] == kind)


def web_columns(kind: str, new: dict, row: dict) -> dict:
    """The UtlTransactions columns that hold the settings the web acts on (only those this database has)."""
    cols = {"BlnAllowDuplicateItems": "Y" if new["allow_duplicate_items"] else "N", "AllowEditDocDate": "Y" if new["allow_edit_date"] else "N"}
    if kind == "purchase_bill":
        cols["AllowCopyFrom"] = "Y" if new["allow_copy_from"] else "N"
        current = str(row.get("RateFldMstItem") or "").strip()
        cols["RateFldMstItem"] = (current or "PURRATE") if new["update_rate_from_pb"] else ""
    if kind == "sales_invoice":
        days = int(new["edit_days"] or 0)
        cols["blnFreezeDoc"] = "Y" if days else "N"   # the desktop keeps the days only while this is on
        cols["AllowEditDeleteForDays"] = days
    return {c: v for c, v in cols.items() if c in row}


def write_desktop(doc_id: int, columns: dict) -> None:
    """UPDATE the document's UtlTransactions row (column names come from the fixed lists above, never from the caller) and re-read the designer."""
    if not columns:
        return
    from . import db, designer
    names = {c for c, _cap, _k in DESKTOP_FIELDS} | {"BlnAllowDuplicateItems", "AllowEditDocDate", "AllowCopyFrom", "blnFreezeDoc", "AllowEditDeleteForDays"}
    bad = [c for c in columns if c not in names]
    if bad:
        raise HTTPException(422, f"'{bad[0]}' is not a Transaction Settings field")
    with db.transaction() as cur:
        cur.execute(f"UPDATE dbo.UtlTransactions SET {', '.join(f'[{c}] = ?' for c in columns)} WHERE DocID = ?", (*columns.values(), doc_id))
    designer.load()
