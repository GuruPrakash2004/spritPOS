"""The rest of what the desktop's Doc Designer tables say about a document, besides its fields (schema_meta.py):

- the document's behaviour flags (UtlTransactions): does it affect stock, allow Copy From, need authorisation ...
- its number format (UtlDocNoFmt) and its find fields (UtlSearchSettings, "<doc name>VIEW")
- the menu (MstMenu): which screens exist, in which group and order, which are shown
- the rows behind a drop-down field (FldCmbTblName / FldCmbStorageFldName / FldCmbDisplayCols / FldCmbAddnlCond)

Everything is read from the tables loaded by designer.load() (store.UTL_*), so a change in the database arrives with the
next refresh. Pure lookups except `lookup()`, which runs one parameterised SELECT.
"""
import re

from . import db, schema_meta, store

# UtlTransactions column -> (name in the definition, kind). Only flags the web engine acts on or shows.
FLAGS = {
    "AffectsStock": ("affects_stock", "text"), "AllowCopyFrom": ("allow_copy_from", "bool"), "AuthReq": ("auth_req", "bool"),
    "AllowEditDocDate": ("allow_edit_date", "bool"), "AllowTenderChange": ("allow_tender_change", "bool"),
    "TenderChangeFld": ("tender_change_field", "text"), "BlnAllowDuplicateItems": ("allow_duplicate_items", "bool"),
    "PrintAfterSave": ("print_after_save", "bool"), "AccTrn": ("acc_trn", "bool"), "TrnDrCr": ("dr_cr", "text"),
    "Voucher": ("voucher", "bool"), "DetSecPresent": ("has_detail", "bool"), "ItemFilterCond": ("item_filter", "text"),
    "PartyFilterCond": ("party_filter", "text"), "NonItemDoc": ("non_item", "bool"), "AllowImpExp": ("allow_import_export", "bool"),
    "FileAttachReq": ("file_attach", "bool"), "FldSMS": ("sms_field", "text"), "FldEmail": ("email_field", "text"),
    "GeneratePDF": ("generate_pdf", "bool"), "PDFPreview": ("pdf_preview", "bool"), "AuditLogTrack": ("audit_log", "bool"),
    "AllowLoadFromBOM": ("allow_load_from_bom", "bool"), "LinkedFROMDoc": ("linked_from", "text"), "LinkedTODoc": ("linked_to", "text"),
    "MstAutoGenCode": ("auto_code", "bool"), "MstInventory": ("inventory_master", "bool"), "ContDocNoGroup": ("doc_no_group", "text"),
    "AllowEditDeleteForDays": ("edit_delete_days", "text"), "blnFreezeDoc": ("freeze_doc", "bool"),
}


def _text(v) -> str:
    return "" if v is None else str(v).strip()


def _flag(v) -> bool:
    return _text(v).upper() in ("Y", "YES", "TRUE", "1")


def doc_flags(doc: dict) -> dict:
    """The behaviour flags of a UtlTransactions row, normalised (bools for Y/N columns, trimmed text otherwise)."""
    out = {}
    for column, (name, kind) in FLAGS.items():
        if column in doc:
            out[name] = _flag(doc[column]) if kind == "bool" else _text(doc[column])
    return out


def input_columns(doc_id: int) -> set[str]:
    """Lower-case codes of the document's header / footer / total fields a person types into: shown, not read-only, not
    hidden and without a formula (a formula column is worked out, never accepted from a request)."""
    out = set()
    for r in store.UTL_FIELDS:
        if int(r.get("DocID") or 0) != int(doc_id) or _text(r.get("FldSection")).lower() == "detail":
            continue
        if _flag(r.get("FldBlnReadOnly")) or _flag(r.get("FLdBlnHide")) or _flag(r.get("FldBlnDispOnly")) or _text(r.get("FldFormula")):
            continue
        out.add(_text(r.get("FldCode")).lower())
    return out


def numbering(doc_id: int) -> dict:
    """The number format of the document (UtlDocNoFmt): the F1..F14 parts, whether the series restarts per prefix."""
    row = next((r for r in store.UTL_NUMFMT if int(r.get("DocID") or 0) == int(doc_id)), None)
    if not row:
        return {}
    parts = [_text(row.get(f"F{i}")) for i in range(1, 15)]
    while parts and not parts[-1]:
        parts.pop()
    return {"parts": parts, "prefix_wise_reset": _flag(row.get("PfxWiseResetNo")), "number_editable": _flag(row.get("DocNoChange")),
            "auth_required": _flag(row.get("AuthReq"))}


def find_fields(doc_name: str) -> list[str]:
    """The fields the document's Find box searches (UtlSearchSettings, keyed "<doc name>VIEW"), in the designer's order."""
    key = f"{doc_name}VIEW".strip().lower()
    rows = [r for r in store.UTL_SEARCH if _text(r.get("DocName")).lower() == key]
    seen, out = set(), []
    for r in sorted(rows, key=lambda r: int(r.get("Slno") or 0)):
        f = _text(r.get("SearchFld"))
        if f and f.lower() not in seen:
            seen.add(f.lower())
            out.append(f)
    return out


def menu() -> list[dict]:
    """The desktop's menu (MstMenu): every entry with its group, order and whether the desktop shows it."""
    out = []
    for r in store.UTL_MENU:
        out.append({"id": int(r.get("MenuItemID") or 0), "caption": _text(r.get("MenuItemCaption")), "name": _text(r.get("MenuItemName")),
                    "index": int(r.get("Menuindex") or 0), "type": int(r.get("MenuItemType") or 0), "doc_id": int(r.get("DocID") or 0),
                    "group": _text(r.get("MenuGroupName")), "show": _flag(r.get("MenuShow")), "form": _text(r.get("FrmName")),
                    "icon": _text(r.get("IconName")), "view": _text(r.get("MenuView")),
                    "slno": int(r["Slno"]) if r.get("Slno") is not None else None})
    return sorted(out, key=lambda m: (m["slno"] if m["slno"] is not None else 10_000, m["index"], m["id"]))


# ─── Drop-down rows ──────────────────────────────────────────────────────────

_IDENT = re.compile(r"^[A-Za-z_][A-Za-z0-9_]*$")
_WHERE = re.compile(r"^[A-Za-z0-9_ =<>!'.,()%\-]+$")


def _where(cond: str) -> str:
    """The designer's extra condition ("Inactive=`N`" — backticks stand for quotes), or '' when it isn't plainly safe."""
    text = _text(cond).replace("`", "'")
    if not text or ";" in text or "--" in text or "/*" in text or not _WHERE.match(text):
        return ""
    return text


def combo_of(doc_id: int, code: str) -> dict | None:
    """The drop-down description of a designer field, or None when the field has none."""
    for r in store.UTL_FIELDS:
        if int(r.get("DocID") or 0) == int(doc_id) and _text(r.get("FldCode")).lower() == code.lower():
            if _flag(r.get("FldBlnComboData")) and _text(r.get("FldCmbTblName")):
                return {"table": _text(r.get("FldCmbTblName")), "store": _text(r.get("FldCmbStorageFldName")),
                        "display": [c.strip() for c in _text(r.get("FldCmbDisplayCols")).split(",") if c.strip()],
                        "where": _text(r.get("FldCmbAddnlCond"))}
            return None
    return None


def lookup(doc_id: int, code: str, q: str = "", limit: int = 200) -> list[dict]:
    """[{value, label}] for a drop-down field: the storage column as the value, the display columns joined as the label.
    Table and column names are checked against the real columns, the condition against a plain pattern, and the search
    text is a parameter."""
    combo = combo_of(doc_id, code)
    if not combo:
        return []
    cols = {c["name"].lower(): c["name"] for c in schema_meta.columns(combo["table"])}
    value = cols.get(combo["store"].lower())
    shown = [cols[c.lower()] for c in combo["display"] if c.lower() in cols]
    known = {_text(d.get("HdrTblName")).lower() for d in store.UTL_DOCS.values()} | {_text(d.get("DtlTblName")).lower() for d in store.UTL_DOCS.values()}
    if not value or not shown or not _IDENT.match(combo["table"]) or combo["table"].lower() not in known:   # only tables the designer describes
        return []
    sql = f"SELECT TOP {max(1, min(int(limit), 1000))} [{value}] AS v, " + ", ".join(f"[{c}] AS d{i}" for i, c in enumerate(shown)) \
        + f" FROM dbo.[{combo['table']}] WITH (NOLOCK)"
    where, args = [], []
    cond = _where(combo["where"])
    if cond:
        where.append(f"({cond})")
    if q.strip():
        where.append(" (" + " OR ".join(f"CAST([{c}] AS varchar(200)) LIKE ?" for c in shown) + ")")
        args += [f"%{q.strip()}%"] * len(shown)
    if where:
        sql += " WHERE " + " AND ".join(where)
    sql += f" ORDER BY [{shown[0]}]"
    try:
        rows = db.fetchall(sql, tuple(args))
    except Exception:   # a table or condition the database doesn't accept: no choices rather than an error page
        return []
    return [{"value": r["v"], "label": " • ".join(_text(r[f"d{i}"]) for i in range(len(shown)) if _text(r[f"d{i}"]))} for r in rows]


# ─── Pick-up SQL ─────────────────────────────────────────────────────────────

_FORBIDDEN = re.compile(r"(insert|update|delete|drop|alter|create|truncate|exec|execute|merge|grant|revoke|xp_\w+)", re.I)


def pickup(doc_id: int, code: str, values: dict) -> str:
    """A field's pick-up value (UtlDocDsgrFlds.FldPickUP, vLQMS.exe vsfValidateEdit): the designer's SELECT run with
    #CODE# standing for the value of that field (#SVTPARTY# the party's id, #SVTITEM# the item code). Each token becomes a
    parameter, never text pasted into the SQL, and only a single read-only SELECT runs. Returns '' when it finds nothing."""
    row = next((r for r in store.UTL_FIELDS if int(r.get("DocID") or 0) == int(doc_id) and _text(r.get("FldCode")).lower() == code.lower()), None)
    sql = _text(row.get("FldPickUP")) if row else ""
    if not sql:
        return ""
    sql = sql.replace("`", "'")
    if ";" in sql or not sql.lower().startswith("select") or _FORBIDDEN.search(sql) or "--" in sql or "/*" in sql:
        return ""
    lower = {str(k).lower(): v for k, v in (values or {}).items()}
    params: list = []

    def token(m):
        value = lower.get(m.group(2).lower(), "")
        params.append(f"{m.group(1)}{value}{m.group(3)}" if "'" in m.group(0) else value)   # '%#X#%' keeps its wildcards
        return "?"
    sql = re.sub(r"'?(%?)#([A-Za-z0-9_]+)#(%?)'?", token, sql)
    try:
        result = db.read_only(sql, tuple(params), max_rows=1)
    except Exception:   # a query this database doesn't accept: no value rather than an error page
        return ""
    return _text(result["rows"][0][0]) if result["rows"] and result["rows"][0] else ""


# ─── Auto-generated codes (MstAutoGenCode) ───────────────────────────────────

def next_code(doc_id: int, series: str) -> dict:
    """The next code of a series, as vLQMS.exe's FillAutoGenItemNo makes it: the highest <stem>No of that <stem>Series plus one
    (1 for a new series), and the code = the series followed by that number as six digits."""
    doc = store.UTL_DOCS.get(int(doc_id)) or {}
    table = _text(doc.get("HdrTblName"))
    stem = table[3:] if table.lower().startswith("mst") else table
    cols = {c["name"].lower(): c["name"] for c in schema_meta.columns(table)}
    s_col, n_col = cols.get(f"{stem}series".lower()), cols.get(f"{stem}no".lower())
    if not (_IDENT.match(table) and s_col and n_col) or not _text(series):
        return {"no": 0, "code": ""}
    rows = db.fetchall(f"SELECT ISNULL(MAX(ISNULL([{n_col}], 0)), 0) + 1 AS n FROM dbo.[{table}] WITH (NOLOCK) WHERE [{s_col}] = ?", (_text(series),))
    number = int(rows[0]["n"]) if rows else 1
    return {"no": number, "code": f"{_text(series)}{number:06d}"}


def series_list(doc_id: int) -> list[str]:
    """The series already used in the table (<stem>Series, not blank or "-")."""
    doc = store.UTL_DOCS.get(int(doc_id)) or {}
    table = _text(doc.get("HdrTblName"))
    stem = table[3:] if table.lower().startswith("mst") else table
    s_col = {c["name"].lower(): c["name"] for c in schema_meta.columns(table)}.get(f"{stem}series".lower())
    if not (_IDENT.match(table) and s_col):
        return []
    rows = db.fetchall(f"SELECT DISTINCT [{s_col}] AS s FROM dbo.[{table}] WITH (NOLOCK) WHERE [{s_col}] IS NOT NULL AND [{s_col}] <> '-' ORDER BY 1")
    return [_text(r["s"]) for r in rows if _text(r["s"])]
