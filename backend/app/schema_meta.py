"""The shape of the shop's database, read live, and the screen definitions built from it.

The desktop vLQMS draws every screen from the database: `UtlTransactions` (one row per screen) and
`UtlDocDsgrFlds` (one row per field) say what to show and in which order, and the real tables say what each column
is. This module does the same for the web app. `screen()` merges the two into one list of fields that the browser
draws in a responsive, ordered grid (Header / Detail / Footer / Total sections, then sub-section, then order), so
nothing is fixed in code. `fingerprint()` is a cheap value that changes when a table or a designer row changes, so
open screens know to read the definition again.

`build_screen()` is pure (rows in, definition out) so it can be tested without a database.
"""
import logging
import re
import threading

from . import db, designer, field_rules, store

logger = logging.getLogger("vlqms.schema")

_lock = threading.Lock()
_cols: dict[str, list[dict]] = {}

# Columns the desktop fills itself: never offered for typing.
# Column types the web app can type into; rowversion, uniqueidentifier, xml, binary ... are never offered or written.
ALLOWED_TYPES = None   # set below, after the type groups
# Names that mean stock or cost: those change only through transactions / cost rights, never as a free-form extra.
GUARDED = re.compile(r"qoh|clbal|opbal|stock|stk|cost|purrate|pur_rate", re.I)
AUDIT_COLUMNS = {"addedby", "addedon", "changedby", "changedon", "authorised", "authorisedby", "authorisedon",
                 "isdelete", "deletedby", "deletedon"}
NUMBER_TYPES = {"int", "bigint", "smallint", "tinyint", "decimal", "numeric", "money", "smallmoney", "float", "real"}
DATE_TYPES = {"date", "datetime", "datetime2", "smalldatetime", "datetimeoffset"}
TEXT_TYPES = {"char", "varchar", "nchar", "nvarchar", "text", "ntext"}
ALLOWED_TYPES = NUMBER_TYPES | DATE_TYPES | TEXT_TYPES | {"bit"}


def editable(col: dict) -> bool:
    """A column the web app may write as a free-form extra."""
    return (str(col.get("type") or "").lower() in ALLOWED_TYPES and not col["is_identity"] and not col["is_computed"]
            and col["name"].lower() not in AUDIT_COLUMNS and not GUARDED.search(col["name"]))


def invalidate() -> None:
    """Forget the cached column lists (a column was added, dropped or changed)."""
    with _lock:
        _cols.clear()
    try:   # the writers keep their own copy
        from . import sqlstore
        with sqlstore._lock:
            sqlstore._cols.clear()
    except Exception:   # nothing to clear
        pass


def columns(table: str) -> list[dict]:
    """The table's columns in table order: name, type, length, precision, scale, nullable, identity, computed."""
    table = _safe(table)
    if not table:
        return []
    with _lock:
        if table in _cols:
            return _cols[table]
    try:
        rows = db.fetchall(
            """SELECT c.name, TYPE_NAME(c.system_type_id) AS type, c.max_length, c.precision, c.scale, c.is_nullable,
                      c.is_identity, c.is_computed, c.column_id
               FROM sys.columns c WHERE c.object_id = OBJECT_ID(?) ORDER BY c.column_id""", (f"dbo.{table}",))
    except Exception as e:   # no database
        logger.warning("Columns of %s not read: %s", table, e)
        return []
    with _lock:
        _cols[table] = rows
    return rows


def _safe(name) -> str:
    """A table name is only ever letters, digits and underscores (it goes into SQL text elsewhere)."""
    name = str(name or "").strip()
    return name if re.fullmatch(r"[A-Za-z0-9_]+", name) else ""


def _yes(v) -> bool:
    return str(v if v is not None else "").strip().upper() in ("Y", "YES", "TRUE", "1")


def _num(v, default=0):
    try:
        return int(float(v))
    except (TypeError, ValueError):
        return default


def _humanise(name: str) -> str:
    s = re.sub(r"[_\s]+", " ", name).strip()
    s = re.sub(r"(?<=[a-z])(?=[A-Z])", " ", s)
    return s.title() if s.isupper() or s.islower() else s


def _sql_kind(col: dict) -> str:
    t = str(col.get("type") or "").lower()
    if t in NUMBER_TYPES:
        return "number"
    if t in DATE_TYPES:
        return "date"
    if t == "bit":
        return "bool"
    return "text"


def _rule(compare, operator, value) -> dict | None:
    """A conditional rule from the designer's (compare, operator, with) triple, or None when it has no operator.

    The desktop (DsgrModule.vsfGenHideMndt / vsfBeforeEditCancel) treats the operator as the switch: a flag set to Y with an
    empty operator holds always; with an operator the flag holds only while `compare <operator> with` is true. `compare`
    and `with` may be a field (#CODE#), a constant, or an expression using #CODE#, SVTCURDATE, SVTUSERCODE, #PFX# ..."""
    operator = str(operator or "").strip()
    if not operator:
        return None
    compare = str(compare or "").strip()
    return {"field": compare.strip("#") if re.fullmatch(r"#[^#\s]+#", compare) else "", "compare": compare, "op": operator,
            "value": str(value).strip() if value is not None else ""}


def _default_conditions(row: dict) -> list[dict]:
    out = []
    for group in str(row.get("FldDefCondList") or "").split("°"):
        parts = group.split("ß")
        if len(parts) >= 4 and parts[1].strip():
            out.append({"compare": parts[0].strip(), "op": parts[1].strip(), "with": parts[2].strip(), "value": parts[3].strip()})
    return out


def _field(row: dict, col: dict | None, captions: dict) -> dict:
    code = str(row.get("FldCode") or "").strip()
    kind = {"text": "text", "numeric": "number", "date": "date", "list string": "list"}.get(
        str(row.get("FldDataType") or "").strip().lower(), _sql_kind(col) if col else "text")
    width = _num(row.get("FldDataWidth"))
    if not width and col and kind == "text":
        width = col["max_length"] // (2 if str(col["type"]).startswith("n") else 1) if col["max_length"] > 0 else 0
    options = [o.strip() for o in str(row.get("FldNumStringListContent") or "").split(",") if o.strip()] \
        if kind == "list" or _yes(row.get("FldBlnNumOrStringList")) else []
    combo = None
    if _yes(row.get("FldBlnComboData")) and _safe(row.get("FldCmbTblName")):
        combo = {"table": _safe(row.get("FldCmbTblName")), "store": str(row.get("FldCmbStorageFldName") or "").strip(),
                 "display": [c.strip() for c in str(row.get("FldCmbDisplayCols") or "").split(",") if c.strip()],
                 "where": str(row.get("FldCmbAddnlCond") or "").strip()}
    return {
        "code": col["name"] if col else code,   # the table's own spelling: extras are loaded and saved under it
        "caption": captions.get(code.lower()) or str(row.get("FldCaption") or "").strip() or _humanise(code),
        "section": str(row.get("FldSection") or "Header").strip() or "Header",
        "sub_section": str(row.get("SubSectionName") or "").strip(),
        "order": _num(row.get("FldOrderBy")),
        "type": kind,
        "width": width,
        "decimals": _num(row.get("FldNumDecimals"), 2) if kind == "number" else 0,
        "align": str(row.get("FldTxtAlignment") or "").strip().lower(),
        "multiline": _yes(row.get("FldTxtBlnMultiLine")),
        "readonly": (_yes(row.get("FldBlnReadOnly")) and _rule(row.get("FldROCompareVal"), row.get("FldROOperator"), row.get("FldROWithVal")) is None)
                    or _yes(row.get("FldBlnDispOnly")) or bool(col and (col["is_identity"] or col["is_computed"])),
        "hidden": _yes(row.get("FLdBlnHide")) and _rule(row.get("FldHideCompareVal"), row.get("FldHideOperator"), row.get("FldHideWithVal")) is None,
        "mandatory": _yes(row.get("FldBlnMandatory")) and _rule(row.get("FldMndtryCompareVal"), row.get("FldMndtryOperator"), row.get("FldMndtryWithVal")) is None,
        "unique": _yes(row.get("FldUnique")),
        "options": options,
        "combo": combo,
        "default": str(row.get("FldDefStdDefault") or "").strip(),
        "use_default": _yes(row.get("FldUseDefault")),
        # conditional defaults: FldDefCondList = groups split by "°", each "compare ß operator ß with ß value" (vLQMS.exe fillVsfHFTDefValues)
        "default_conditions": _default_conditions(row) if _yes(row.get("FldDefBlnAgnstCond")) else [],
        "unique_based_on": str(row.get("FldUniqueBasedOn") or "").strip(),
        "tooltip": str(row.get("FldToolTip") or "").strip(),
        "case": str(row.get("FldTxtCase") or "").strip().upper(),
        "min": row.get("FldNumRangeFromValue") if _yes(row.get("FldNumBlnRangeFromChk")) else None,
        "max": row.get("FldNumRangeToValue") if _yes(row.get("FldNumBlnRangeToChk")) else None,
        "allow_negative": _yes(row.get("FldNumBlnAllowNegative")),
        "total": _yes(row.get("BlnTotalReq")),
        "formula": str(row.get("FldFormula") or "").strip(),
        "pickup": str(row.get("FldPickUP") or "").strip(),
        "pickup_after": str(row.get("FldPickUPAfterFld") or "").strip().strip("#"),
        # a condition only counts while its flag is Y (the desktop ignores a condition on a flag that is N)
        "hide_when": _rule(row.get("FldHideCompareVal"), row.get("FldHideOperator"), row.get("FldHideWithVal")) if _yes(row.get("FLdBlnHide")) else None,
        "mandatory_when": _rule(row.get("FldMndtryCompareVal"), row.get("FldMndtryOperator"), row.get("FldMndtryWithVal")) if _yes(row.get("FldBlnMandatory")) else None,
        "readonly_when": _rule(row.get("FldROCompareVal"), row.get("FldROOperator"), row.get("FldROWithVal")) if _yes(row.get("FldBlnReadOnly")) else None,
        "standard": False,
    }


def _standard(col: dict, position: int, captions: dict) -> dict:
    """A table column the designer has no row for (the desktop's own, fixed fields)."""
    name = col["name"]
    kind = _sql_kind(col)
    audit = not editable(col) or (kind == "number" and name.lower().endswith("id"))   # keys, audit, odd types: not offered
    width = col["max_length"] // (2 if str(col["type"]).startswith("n") else 1) if kind == "text" and col["max_length"] > 0 else 0
    return {
        "code": name, "caption": captions.get(name.lower()) or _humanise(name), "section": "Header", "sub_section": "",
        "order": -1000 + position, "type": kind, "width": width, "decimals": col["scale"] if kind == "number" else 0,
        "align": "", "multiline": kind == "text" and (width > 120 or width < 0), "readonly": bool(col["is_identity"] or col["is_computed"] or audit),
        "hidden": audit, "mandatory": False,   # the table's own default / the writer fills a column the designer does not require
        "unique": False, "options": [], "combo": None, "default": "", "tooltip": "", "case": "",
        "min": None, "max": None, "allow_negative": True, "total": False, "formula": "", "pickup": "", "pickup_after": "",
        "hide_when": None, "mandatory_when": None, "readonly_when": None, "standard": True,
    }


def compiled_columns(doc: dict) -> set[str] | None:
    """The table columns vLQMS.exe's own form draws itself for this kind of master (decompiled frmMstGeneral / frmMstParty /
    frmMstItem / FrmMstComp), as lower-case names; every other column of the table is shown only when the Doc Designer has a row
    for it. None means "no rule known": show every column.

    The form is chosen by UtlTransactions.TrnDocType: General (code, name, Inactive + remarks), Party Variant (customer /
    supplier: also alias, billing address 1-4, city, state, pin, country, remarks, party group, ledger group), Item Variant
    (code, name, short name, hot key, HSN, group, liquor type, non-stockable, UOM, bottles per case), and the Company form's
    five rows (name, address 1-3, e-mail)."""
    table = str(doc.get("HdrTblName") or "").strip().lower()
    kind = str(doc.get("TrnDocType") or "").strip().lower()
    stem = table[3:] if table.startswith("mst") else table
    common = {f"{stem}code", f"{stem}name", "inactive", "inactivermks"}
    if str(doc.get("MstAutoGenCode") or "").strip().upper() == "Y":   # the form then shows Series and No, and builds the code from them
        common |= {f"{stem}series", f"{stem}no"}
    if table == "mstcomp":
        return {"compname", "address1", "address2", "address3", "email"}
    if str(doc.get("DocType") or "").strip().lower() != "master":
        return None
    if kind == "party variant":
        return common | {f"{stem}aliasname", "badd1", "badd2", "badd3", "badd4", "city", "state", "pincode", "country", "remarks",
                         f"{stem}groupid", "ledgroupid"}
    if kind == "item variant":
        return common | {"shortname", "hotkey", "hsncode", f"{stem}groupid", "liqtype", "nonstkable", "uomid", "botpercases"}
    if kind == "general" and table not in ("mstscc", "mstuom", "mstuser", "mstrole"):
        return common
    return None


def build_screen(doc: dict, field_rows: list[dict], hdr_cols: list[dict], dtl_cols: list[dict], captions: dict | None = None) -> dict:
    """The screen definition: designer fields first-class, table columns the designer does not list added after,
    columns the table no longer has dropped. Pure: rows in, definition out."""
    captions = captions or {}
    by_hdr = {c["name"].lower(): c for c in hdr_cols}
    by_dtl = {c["name"].lower(): c for c in dtl_cols}
    have_tables = bool(hdr_cols or dtl_cols)
    fields, seen = [], set()
    for row in sorted(field_rows, key=lambda r: (_num(r.get("FldOrderBy")), _num(r.get("FldID")))):
        code = str(row.get("FldCode") or "").strip()
        if not code or code.lower() in seen:
            continue
        detail = str(row.get("FldSection") or "").strip().lower() == "detail"
        col = (by_dtl if detail else by_hdr).get(code.lower())
        if have_tables and col is None and not str(row.get("FldFormula") or "").strip() and not _yes(row.get("FldBlnDispOnly")):
            continue   # the column was dropped from the table: the field disappears with it
        seen.add(code.lower())
        f = _field(row, col, captions)
        if col is not None and not editable(col):
            f["readonly"] = True   # a key, computed, stock or odd-typed column: shown, never written as an extra
        fields.append(f)
    compiled = compiled_columns(doc)
    for i, col in enumerate(hdr_cols):
        if compiled is not None and col["name"].lower() not in compiled:
            continue   # the desktop's own form doesn't draw this column and the designer has no row for it
        if col["name"].lower() not in seen and str(doc.get("DocType") or "").strip().lower() == "master":
            seen.add(col["name"].lower())
            fields.append(_standard(col, i, captions))
    return {
        "doc_id": _num(doc.get("DocID")),
        "name": str(doc.get("DocName") or "").strip(),
        "type": str(doc.get("DocType") or "").strip(),
        "short_form": str(doc.get("DocShForm") or "").strip(),
        "header_table": _safe(doc.get("HdrTblName")),
        "detail_table": _safe(doc.get("DtlTblName")),
        "fields": fields,
    }


def review(doc: dict, field_rows: list[dict], hdr_cols: list[dict], dtl_cols: list[dict], master_path: str = "") -> list[dict]:
    """Problems an Administrator should look at on one screen: [{level: "error"|"warn"|"info", field, text}].
    Pure like build_screen: it reads the same rows and says what the web app will do with them (and what it cannot)."""
    out: list[dict] = []

    def add(level, field, text):
        out.append({"level": level, "field": field, "text": text})

    by_hdr = {c["name"].lower(): c for c in hdr_cols}
    by_dtl = {c["name"].lower(): c for c in dtl_cols}
    is_master = str(doc.get("DocType") or "").strip().lower() == "master"
    if not hdr_cols:
        add("error", "", f"Header table '{doc.get('HdrTblName') or ''}' was not found in the database.")
    if is_master and not master_path:
        add("info", "", "The web app has no screen for this master yet; the fields are listed but cannot be edited here.")
    seen_codes, orders = {}, {}
    for row in field_rows:
        code = str(row.get("FldCode") or "").strip()
        section = str(row.get("FldSection") or "Header").strip() or "Header"
        if not code:
            add("warn", "", "A designer row has no field code and is ignored.")
            continue
        key = code.lower()
        if key in seen_codes:
            add("warn", code, f"'{code}' is defined twice in the designer; only the first is used.")
            continue
        seen_codes[key] = True
        orders.setdefault((section.lower(), _num(row.get("FldOrderBy"))), []).append(code)
        detail = section.lower() == "detail"
        col = (by_dtl if detail else by_hdr).get(key)
        formula = str(row.get("FldFormula") or "").strip()
        if col is None and hdr_cols and not formula and not _yes(row.get("FldBlnDispOnly")):
            add("warn", code, "The column is not in the table any more, so the field is left out.")
            continue
        mandatory = _yes(row.get("FldBlnMandatory"))
        if col is not None and not editable(col) and not detail:
            if mandatory:
                add("error", code, "Mandatory in the designer but the column is a key, computed, stock/cost or unsupported type: it is shown read-only and never written, so a new record cannot fill it.")
            else:
                add("info", code, "The column is a key, computed, stock/cost or unsupported type: shown read-only, never written from the web.")
        if mandatory and (_yes(row.get("FLdBlnHide")) or _yes(row.get("FldBlnReadOnly"))):
            add("warn", code, "Marked mandatory but also hidden or read-only: it is not asked for.")
        kind = str(row.get("FldDataType") or "").strip().lower()
        if kind == "list string" and not [o for o in str(row.get("FldNumStringListContent") or "").split(",") if o.strip()]:
            add("warn", code, "A list field with no list values.")
        width = _num(row.get("FldDataWidth"))
        if col is not None and kind == "text" and width and col["max_length"] > 0:
            real = col["max_length"] // (2 if str(col["type"]).startswith("n") else 1)
            if width > real:
                add("warn", code, f"The designer allows {width} characters but the column holds {real}; longer text is refused.")
        if col is not None and kind == "numeric" and _num(row.get("FldNumDecimals")) > 0 and str(col["type"]).lower() in ("int", "bigint", "smallint", "tinyint"):
            add("warn", code, "Decimals are allowed but the column is a whole-number type.")
        if _yes(row.get("FldBlnComboData")):
            table = _safe(row.get("FldCmbTblName"))
            if not table:
                add("warn", code, "A drop-down with no (or an unsafe) source table name.")
            elif not columns(table):
                add("warn", code, f"The drop-down's source table '{table}' was not found.")
            else:
                add("info", code, "Drop-down from a table: not drawn by the web app yet (shown as a plain box).")
        if formula:
            add("info", code, "Has a formula: the web app does not calculate it yet.")
        if str(row.get("FldPickUP") or "").strip():
            add("info", code, "Has a pick-up rule: the web app does not fill it yet.")
        if _yes(row.get("FldUnique")):
            add("info", code, "Marked unique: not checked by the web app.")
    for (section, order), codes in orders.items():
        if len(codes) > 1:
            add("info", ", ".join(codes), f"Same order number {order} in {section}: shown in the designer's row order.")
    extra = [c["name"] for c in hdr_cols if c["name"].lower() not in seen_codes and editable(c)
             and not (str(c["type"]).lower() in NUMBER_TYPES and c["name"].lower().endswith("id"))]
    if is_master and extra:
        shown = ", ".join(extra[:8]) + (f" and {len(extra) - 8} more" if len(extra) > 8 else "")
        add("info", "", f"{len(extra)} table column(s) the designer does not list are offered as extra fields: {shown}.")
    return out


def clean_extras(table: str, extras: dict, skip: set | None = None, context: dict | None = None, exclude: tuple | None = None) -> dict:
    """The columns a master row carries beyond the web app's own fields, checked against the table and the screen's
    designer rules. Dropped: unknown, key, computed, stock/cost, odd-typed and already-modelled columns, and fields the
    designer marks read-only or hidden. Values are converted to the column's type, text is cut-checked to the column's
    length, blanks in a column that can't be NULL are left out (the table's default fills them), and
    designer-mandatory fields must be filled. Raises ValueError with a message for the user."""
    cols = {c["name"].lower(): c for c in columns(table)}
    skip = skip or set()
    doc = next((d for d in store.UTL_DOCS.values() if str(d.get("HdrTblName") or "").lower() == table.lower()), None)
    rules = {str(f.get("FldCode") or "").strip().lower(): f for f in (designer.fields_for(int(doc["DocID"])) if doc else [])}
    out = {}
    # What a condition may look at: the record's other fields (the caller's typed values by column code) and the extras.
    values = {**{str(k).lower(): v for k, v in (context or {}).items()}, **{str(k).lower(): v for k, v in (extras or {}).items()}}
    for name, value in (extras or {}).items():
        key = str(name).lower()
        col = cols.get(key)
        if col is None or key in skip or not editable(col):
            continue
        rule = rules.get(key)
        # Read-only / hidden hold always without a condition, or while the condition is true (desktop: vsfBeforeEditCancel).
        if rule and (field_rules.is_readonly(rule, values) or field_rules.is_hidden(rule, values)):
            continue
        kind = _sql_kind(col)
        label = str(rule.get("FldCaption") or "").strip() if rule else ""
        label = label or _humanise(col["name"])
        blank = value is None or (isinstance(value, str) and value.strip() == "")
        if blank:
            if kind == "text":
                out[col["name"]] = "" if not col["is_nullable"] else None
            elif col["is_nullable"]:
                out[col["name"]] = None
            continue
        if rule:   # the designer's own rules for this field: number / date / text limits, case, percentage, mandatory zero
            message, value = field_rules.check_value(rule, value, values)
            if message:
                raise ValueError(message)
            if _yes(rule.get("FldUnique")) and str(value if value is not None else "").strip() != "" and _taken(table, col["name"], value, rule, values, cols, exclude):
                based = str(rule.get("FldUniqueBasedOn") or "").replace("#", "").strip()
                raise ValueError(f"{label} is to be unique" + (f" based on {based}" if based else ""))
        if kind == "number":
            try:
                out[col["name"]] = float(value) if col["type"] in ("float", "real", "decimal", "numeric", "money", "smallmoney") else int(str(value).strip().split(".")[0])
            except (TypeError, ValueError):
                raise ValueError(f"{label} must be a number")
        elif kind == "bool":
            out[col["name"]] = 1 if str(value).strip().lower() in ("1", "true", "y", "yes") else 0
        elif kind == "date":
            text = str(value).strip().replace("T", " ")
            out[col["name"]] = text[:10] if col["type"] == "date" else text   # a datetime keeps its time of day
        else:
            text = str(value)
            width = col["max_length"] // (2 if str(col["type"]).startswith("n") else 1) if col["max_length"] > 0 else 0
            if width and len(text) > width:
                raise ValueError(f"{label} can be at most {width} characters")
            out[col["name"]] = text
    for code, f in rules.items():
        if (field_rules.is_mandatory(f, values) and not field_rules.is_hidden(f, values) and not field_rules.is_readonly(f, values)
                and code in cols and code not in skip and editable(cols[code])):
            given = {str(k).lower(): v for k, v in (extras or {}).items()}
            if code in given and str(given[code] if given[code] is not None else "").strip() == "":
                raise ValueError(f"Enter the {str(f.get('FldCaption') or code).strip()}")
    return out


def _taken(table: str, column: str, value, rule: dict, values: dict, cols: dict, exclude: tuple | None) -> bool:
    """Does another record already hold this value (and, for "unique based on", the same value of those other fields)?
    vLQMS.exe fnExpUniqueEvaluate. `exclude` = (code column, this record's code), so a record doesn't clash with itself."""
    sql = f"SELECT TOP 1 1 FROM dbo.[{table}] WITH (NOLOCK) WHERE [{column}] = ?"
    args: list = [value]
    for part in [p for p in re.split(r"[,;]", str(rule.get("FldUniqueBasedOn") or "")) if p.strip()]:
        other = cols.get(part.replace("#", "").strip().lower())
        if other is not None and part.replace("#", "").strip().lower() in values:
            sql += f" AND [{other['name']}] = ?"
            args.append(values[part.replace("#", "").strip().lower()])
    if exclude and exclude[1] not in (None, ""):
        sql += f" AND [{exclude[0]}] <> ?"
        args.append(exclude[1])
    try:
        return bool(db.fetchall(sql, tuple(args)))
    except Exception:   # a column or table that can't be compared: don't block the save
        return False


def captions() -> dict:
    """UtlFldCaptions: global caption overrides, keyed by lower-case field code."""
    return {str(k).lower(): v for k, v in getattr(store, "UTL_CAPTIONS", {}).items()}


def screen(doc_id: int) -> dict | None:
    doc = store.UTL_DOCS.get(int(doc_id))
    if not doc:
        return None
    return build_screen(doc, designer.fields_for(doc_id), columns(doc.get("HdrTblName")), columns(doc.get("DtlTblName")), captions())


def fingerprint() -> str:
    """Changes when a table's columns or the designer's rows change; cheap enough to ask every half minute."""
    try:
        row = db.fetchall(
            """SELECT CONVERT(varchar(40), MAX(modify_date), 126) AS m, COUNT(*) AS n
               FROM sys.objects WHERE type = 'U' AND (name LIKE 'Mst%' OR name LIKE 'Trn%' OR name LIKE 'Utl%')""")
    except Exception as e:   # no database
        logger.warning("Schema fingerprint not read: %s", e)
        return ""
    # Each designer table is summed on its own, so one with a column type CHECKSUM can't take only falls back to its row
    # count and never hides a change in the others.
    parts = []
    watched = {"UtlDocDsgrFlds": "*", "UtlTransactions": "*", "UtlDocNoFmt": "*", "UtlSearchSettings": "*", "UtlFldCaptions": "*",
               "MstMenu": "MenuItemID, MenuItemCaption, MenuItemType, DocID, MenuShow, MenuGroupName, Menuindex, Slno, FrmName"}
    for table, cols in watched.items():
        try:
            value = db.fetchall(f"SELECT CHECKSUM_AGG(CHECKSUM({cols})) AS v, COUNT(*) AS n FROM dbo.[{table}]")[0]
            parts.append(f"{value['v']}.{value['n']}")
        except Exception:
            try:
                parts.append("n" + str(db.fetchall(f"SELECT COUNT(*) AS n FROM dbo.[{table}]")[0]["n"]))
            except Exception:   # the shop's database has no such table
                parts.append("-")
    r = row[0] if row else {}
    return "|".join([str(r.get("m")), str(r.get("n")), *parts])
