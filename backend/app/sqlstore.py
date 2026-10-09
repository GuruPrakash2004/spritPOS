"""Saves the web app's data in the shop's own SQL Server database (the desktop vLQMS database: no copy, no
SQLite), and loads it at startup.

After every successful POST/PUT/DELETE (write middleware, main.py) save() writes what the request changed, in
ONE SQL transaction; if that fails the request answers 503 and the in-memory store is reloaded from SQL
Server (reload()), so memory never keeps a change the database doesn't have.

What is written, compared with what was saved last (only rows that changed):
- master rows the app changed (store.touch): the desktop's own row (MstITEM, MstCUST, MstSUPP, MstITEMGROUP,
  MstBrand, MstUOM, MstPKG, MstUser, ...; LEGACY_MASTERS) and the whole web row (Web_AppRow: fields with no
  desktop column, e.g. password hashes; other masters live only there);
- documents made in the app (store.DOCS without "origin"): the whole document (Web_AppDoc), and saved
  sales invoices / purchase bills also as the desktop's own rows (TrnHdrSINV/TrnDtlSINV, TrnHdrPB/TrnDtlPB;
  cancel and remove set Cancelled / IsDelete);
- stock movements the app made: Web_AppMove (the web's ledger) and a UtlStockList row each, whose
  trigger keeps TrnQOH (a sale cancel deletes that bill's rows instead, as the desktop app does);
- settings and other web state (Global Settings, rights, number series, ...): Web_AppSetting;
- the ADMIN Audit Log: UtlAdmAudit / UtlAdmAuditItem (the desktop's own audit tables);
  Tools runs: Web_ToolAudit.
Only the five Web_ tables are the web app's own (migrations/web_app_tables.sql, created by the Database setup).

Without a database (tests, development away from the office) nothing is saved; the audit logs are kept in
memory for this process only.
"""
import copy
import hashlib
import json
import re
import logging
import threading
import time
from datetime import date, datetime
from decimal import Decimal

from . import db, gsettings, setup_db, store
from .master_specs import SPECS as _SPECS

from . import db_store, doc_engine  # noqa: E402  (db_store imports only db and store)

logger = logging.getLogger("vlqms.sqlstore")

# Tables whose rows the app can change (store.touch). COMPANY is a single dict (key "*").
TABLES = ("ITEMS", "ITEM_GROUPS", "UOMS", "PACKAGES", "BRANDS", "SUPPLIERS", "CUSTOMERS", "LEDGER_GROUPS",
          "LEDGERS", "COUNTERS", "SALES_PERSONS", "USERS", "ROLES", "ROLE_RIGHTS") + tuple(_s["store"] for _s in _SPECS)
# Never written: a desktop user's reversible password is re-read from MstUser instead.
SECRET_FIELDS = ("legacy_password", "default_password")
# Web state saved whole in Web_AppSetting (Section "state"), by name.
SECTIONS = ("settings", "gsettings", "denominations", "seq", "security", "field_rights", "master_approval",
            "report_rights", "doc_settings", "number_series", "menu_layout", "import_fields", "merged_items",
            "merged_item_ids", "company", "print_prefs")
MIRRORED_DOCS = ("sales_invoice", "purchase_bill", "purchase_indent", "sales_return", "stock_deduction")
MOVE_DOCTYPE = {"sale": "INV", "purchase": "PB", "opening": "OP", "sales-return": "SR", "deduction": "STKDED"}
LOCID = 1
WEB_ROW = "WEB:"          # Particulars prefix of the UtlStockList rows the web app wrote
# Codes and document numbers are never cut to fit a column (a cut number would point at another row).
NO_CUT = {"sinvno", "pbno", "pino", "srno", "stkdedno", "docno", "itemcode", "custcode", "suppcode", "itemgroupcode", "brandcode", "uomcode",
          "pkgname", "pkgcode", "usercode", "rowkey", "ledcode", "ledgroupcode", "sccode", "spcode", "scid"}
RETRY_SECONDS = 20        # while the database is offline, how often a write tries to reconnect

_lock = threading.RLock()
_active = False                        # True once load() ran on a ready database
_saved: dict[tuple, str] = {}          # ("row"|"doc"|"state", ...) -> hash of what SQL Server holds
_doc_status: dict[tuple, str] = {}     # (kind, doc_no) -> status last written in legacy format
_moves_saved = 0                       # how many of MOVEMENTS[APP_MOVEMENTS_FROM:] are in Web_AppMove
_movements_base = 0                    # len(MOVEMENTS) before the saved web moves were laid back
_cols: dict[str, dict] = {}            # table -> column metadata
_memory_audit: list[dict] = []         # without a database
_memory_tools: list[dict] = []
_rewrite_moves = False                 # Re Write Stock rebuilt the web's moves: write the whole ledger again
_defaults: dict | None = None          # the store before anything was loaded: empty tables + config (reload() starts from it)
_retry_at = 0.0


class SaveFailed(Exception):
    """SQL Server refused or lost the request's changes (the caller answers 503)."""


def enabled() -> bool:
    """The shop's data is loaded and every change is saved to SQL Server."""
    return _active and db.is_db_available()


def _now() -> str:
    return datetime.now().isoformat(timespec="seconds")


def _json(v) -> str:
    return json.dumps(v, default=str, sort_keys=True)


def _hash(text: str) -> str:
    return hashlib.sha256(text.encode()).hexdigest()


# ─── Column metadata and generic writers ─────────────────────────────────────

def _meta(table: str) -> dict:
    if table not in _cols:
        rows = db.fetchall(
            """SELECT c.name, TYPE_NAME(c.system_type_id) AS type, c.max_length, c.is_nullable, c.is_identity, c.is_computed,
                      CASE WHEN dc.object_id IS NULL THEN 0 ELSE 1 END AS has_default
               FROM sys.columns c LEFT JOIN sys.default_constraints dc ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE c.object_id = OBJECT_ID(?)""", (f"dbo.{table}",))
        _cols[table] = {r["name"].lower(): r for r in rows}
    return _cols[table]


def _fit(table: str, values: dict) -> dict:
    """Only columns the table has (exact names), strings cut to the column's length."""
    meta, out = _meta(table), {}
    for k, v in values.items():
        m = meta.get(k.lower())
        if not m or m["is_identity"] or m["is_computed"]:
            continue
        if isinstance(v, str) and m["type"] in ("varchar", "char", "nvarchar", "nchar") and m["max_length"] > 0:
            width = m["max_length"] // (2 if m["type"].startswith("n") else 1)
            if len(v) > width and m["name"].lower() in NO_CUT:
                raise ValueError(f"{table}.{m['name']}: '{v}' is longer than the column ({width})")
            v = v[:width]
        out[m["name"]] = v
    return out


def _blank(m: dict):
    t = m["type"]
    if t in ("varchar", "char", "nvarchar", "nchar", "text", "ntext"):
        return ""
    if t in ("datetime", "smalldatetime", "date", "datetime2"):
        return datetime.now()
    return 0


def _insert(cur, table: str, values: dict, identity_out: str | None = None):
    values = _fit(table, values)
    for m in _meta(table).values():   # NOT NULL columns without a default the caller didn't give
        if not m["is_nullable"] and not m["has_default"] and not m["is_identity"] and not m["is_computed"] and m["name"] not in values:
            values[m["name"]] = _blank(m)
    cols = ", ".join(f"[{c}]" for c in values)
    marks = ", ".join("?" for _ in values)
    if identity_out:
        cur.execute(f"INSERT INTO dbo.[{table}] ({cols}) OUTPUT INSERTED.[{identity_out}] VALUES ({marks})", tuple(values.values()))
        return cur.fetchone()[0]
    cur.execute(f"INSERT INTO dbo.[{table}] ({cols}) VALUES ({marks})", tuple(values.values()))
    return None


def _update(cur, table: str, values: dict, where: str, args: tuple) -> int:
    values = _fit(table, values)
    if not values:
        return 0
    sets = ", ".join(f"[{c}] = ?" for c in values)
    return cur.execute(f"UPDATE dbo.[{table}] SET {sets} WHERE {where}", (*values.values(), *args))


def _one(cur, sql: str, args: tuple):
    cur.execute(sql, args)
    r = cur.fetchone()
    return r[0] if r else None


def _yn(flag) -> str:
    return "Y" if flag else "N"


def _num(v) -> float:
    try:
        return float(v or 0)
    except (TypeError, ValueError):
        return 0.0


# ─── Masters in legacy format ────────────────────────────────────────────────

def _lookup(cur, table: str, id_col: str, value, *cols: str):
    """The ID of a master row by any of its code/name columns (0 when missing or blank)."""
    if not value:
        return 0
    where = " OR ".join(f"[{c}] = ?" for c in cols)
    return _one(cur, f"SELECT TOP 1 [{id_col}] FROM dbo.[{table}] WHERE {where}", tuple(value for _ in cols)) or 0


def _item_values(cur, r: dict) -> dict:
    return {
        "ITEMNAME": r.get("name", ""), "ShortName": r.get("short_name") or r.get("name", ""), "HSNCode": r.get("hsn_code") or "",
        "ITEMGroupID": _lookup(cur, "MstITEMGROUP", "ITEMGROUPID", r.get("group"), "ITEMGROUPCODE", "ITEMGROUPNAME"),
        "LiqType": r.get("liqtype") or "",
        "UOMID": _lookup(cur, "MstUOM", "UOMID", r.get("uom"), "UOMCODE", "UOMNAME"),
        "PKGID": _lookup(cur, "MstPKG", "PKGID", r.get("package"), "PKGCODE", "PKGNAME") or None,
        "BrandID": _lookup(cur, "MstBrand", "BRANDID", r.get("brand"), "BRANDCODE", "BRANDNAME"),
        "BotPerCases": int(_num(r.get("bottles_per_case")) or 12), "MRP": _num(r.get("mrp")), "PURRATE": _num(r.get("purchase_rate")),
        "CGSTPER": _num(r.get("cgst_per")), "SGSTPER": _num(r.get("sgst_per")), "ROL": _num(r.get("rol")), "ROQ": _num(r.get("roq")),
        "InActive": _yn(not r.get("active", True)),
    }


def _only(r: dict, spec: dict) -> dict:
    """Desktop columns for the keys this row actually carries: {column: (key, convert)}. A column whose key the
    loader never filled (db_store loads only some fields) is left alone, so a web edit can't blank the desktop's
    value (City, GSTNO, e-mail, ...) or switch an inactive row back on."""
    return {col: convert(r[key]) for col, (key, convert) in spec.items() if key in r}


def _text(v) -> str:
    return "" if v is None else str(v)


def _customer_values(cur, r: dict) -> dict:
    return _only(r, {"CUSTNAME": ("name", _text), "MOBNO": ("mobile", _text), "CARDNO": ("loyalty_card_no", _text),
                     "SENDSMS": ("send_sms", _yn), "InActive": ("active", lambda v: _yn(v is False)),
                     "GSTNO": ("gstin", _text), "City": ("city", _text)})


def _supplier_values(cur, r: dict) -> dict:
    return _only(r, {"SUPPNAME": ("name", _text), "SUPPAliasName": ("alias", _text), "BAdd1": ("address", _text),
                     "City": ("city", _text), "PinCode": ("pin", _text), "State": ("state", _text),
                     "Country": ("country", _text), "PHNO": ("phone", _text), "GSTNO": ("gstin", _text),
                     "InActive": ("active", lambda v: _yn(v is False))})


def _user_values(cur, r: dict) -> dict:
    # The desktop's LoginPwd is never touched: the desktop app still signs this user in with it. The web keeps
    # its own password only as a hash (Web_AppRow).
    v = _only(r, {"UserName": ("full_name", _text), "EmailID": ("email", _text), "Mobileno": ("mobile", _text),
                  "Department": ("department", _text), "InactiveRmks": ("inactive_reason", _text),
                  "Inactive": ("active", lambda x: _yn(not x)), "PwdResetDt": ("password_reset_on", lambda x: x or None)})
    if "dob" in r:
        v.update(DOB=r["dob"] or None, ChkDOB=_yn(r["dob"]))
    return v

# table: (desktop table, ID column, code column, values(cur, row))
LEGACY_MASTERS = {
    "ITEMS": ("MstITEM", "ITEMID", "ITEMCODE", _item_values),
    "CUSTOMERS": ("MstCUST", "CUSTID", "CUSTCODE", _customer_values),
    "SUPPLIERS": ("MstSUPP", "SUPPID", "SUPPCODE", _supplier_values),
    "ITEM_GROUPS": ("MstITEMGROUP", "ITEMGROUPID", "ITEMGROUPCODE",
                    lambda cur, r: {"ITEMGROUPNAME": r.get("name", ""), "LiqItem": _yn(r.get("liquor", True)),
                                    "LOYALTY": _yn(r.get("consider_for_loyalty"))}),
    "BRANDS": ("MstBrand", "BRANDID", "BRANDCODE", lambda cur, r: {"BRANDNAME": r.get("name", "")}),
    "UOMS": ("MstUOM", "UOMID", "UOMCODE", lambda cur, r: {"UOMNAME": r.get("name", "")}),
    "PACKAGES": ("MstPKG", "PKGID", "PKGNAME", lambda cur, r: {"PKGCODE": r.get("code", ""), "LTRPERBOT": _num(r.get("litres_per_bottle"))}),
    "USERS": ("MstUser", "UserID", "UserCode", _user_values),
    "LEDGER_GROUPS": ("MstLedgroup", "LEDGROUPID", "LEDGROUPCODE", lambda cur, r: {"LEDGROUPNAME": r.get("name", ""), "InActive": "N"}),
    "LEDGERS": ("MstLed", "LEDID", "LEDCODE",
                lambda cur, r: {"LEDNAME": r.get("name", ""), "InActive": "N",
                                "LEDGROUPID": _lookup(cur, "MstLedgroup", "LEDGROUPID", r.get("group"), "LEDGROUPCODE", "LEDGROUPNAME")}),
    "COUNTERS": ("MstSC", "SCID", "SCCODE",
                 lambda cur, r: {"SCNAME": r.get("name", ""), "SYSNAME": r.get("system_name") or "",
                                 "PREFIX": r.get("prefix") or "", "InActive": _yn(r.get("inactive"))}),
    "SALES_PERSONS": ("MstSP", "SPID", "SPCODE",
                      lambda cur, r: {"SPNAME": r.get("name", ""), "MOBNO": r.get("mobile") or "",
                                      "InActive": _yn(not r.get("active", True))}),
}


def _simple_values(sp: dict):
    """Column values of a simple master (master_specs.py) from its row."""
    def values(cur, r: dict) -> dict:
        v = {sp["name"]: r.get("name", "")}
        for f, col, kind in sp["extras"]:
            v[col] = _num(r.get(f)) if kind == "num" else _text(r.get(f))
        if not sp.get("plain"):
            v["InActive"] = _yn(not r.get("active", True))
        return v
    return values


for _sp in _SPECS:
    LEGACY_MASTERS[_sp["store"]] = (_sp["table"], _sp["id"], _sp["code"], _simple_values(_sp))
# The desktop ID a new row got, kept on the row (bills refer to items, counters and sales persons by ID).
NEW_ID_FIELD = {"ITEMS": "_itemid", "COUNTERS": "_scid", "SALES_PERSONS": "_spid"}


# Where a master's ID is used by the desktop's history: deleting the row would orphan those rows.
IN_USE_BY = {
    "ITEMS": [("TrnDtlSINV", "ITEMID"), ("TrnDtlPB", "ITEMID"), ("UtlStockList", "ITEMID"), ("TrnQOH", "ITEMID")],
    "CUSTOMERS": [("TrnHdrSINV", "CUSTID")], "SUPPLIERS": [("TrnHdrPB", "SUPPID")],
    "COUNTERS": [("TrnHdrSINV", "SCID")], "SALES_PERSONS": [("TrnHdrSINV", "SPID")],
    "ITEM_GROUPS": [("MstITEM", "ITEMGROUPID")], "BRANDS": [("MstITEM", "BRANDID")], "UOMS": [("MstITEM", "UOMID")],
    "PACKAGES": [("MstITEM", "PKGID")], "LEDGER_GROUPS": [("MstLed", "LEDGROUPID")],
}
for _sp in _SPECS:
    IN_USE_BY[_sp["store"]] = list(_sp["used_by"])


def _check_unused(cur, table: str, id_col: str, web_table: str, code_col: str, key: str) -> None:
    """Refuse to delete a desktop master row that old bills or stock still point at (the in-memory IN_USE check only
    sees the latest bills). Zero balances in TrnQOH don't count."""
    row_id = _one(cur, f"SELECT TOP 1 [{id_col}] FROM dbo.[{web_table}] WHERE [{code_col}] = ?", (key,))
    if row_id is None:
        return
    # The desktop's own deletion rules (UtlDeletion: DelTblName is referenced by TblName.FldName, shown as TblCaption):
    # vLQMS.exe refuses "Cannot Delete!!! Data : <value> of <table> exists in <caption>" for each of them (svtModule.blnDeleteOK).
    try:
        rules = db.fetchall("SELECT TblName, FldName, TblCaption FROM dbo.UtlDeletion WHERE UPPER(LTRIM(RTRIM(DelTblName))) = UPPER(?)", (web_table,))
    except Exception:   # a database without the table
        rules = []
    for rule in rules:
        ref_table, ref_col = str(rule["TblName"]).strip(), str(rule["FldName"]).strip()
        if not ref_table or not ref_col or ref_table.lower() == web_table.lower():
            continue
        try:
            found = _one(cur, f"SELECT TOP 1 1 FROM dbo.[{ref_table}] WHERE [{ref_col}] = ?", (row_id,))
        except Exception:   # a report table or column this database doesn't have
            continue
        if found:
            raise ValueError(f"Cannot delete {key}: data of {web_table} exists in {str(rule['TblCaption']).strip() or ref_table}")
    for ref_table, ref_col in IN_USE_BY.get(table, []):
        extra = " AND ISNULL([CLBal], 0) <> 0" if ref_table == "TrnQOH" else ""
        if _one(cur, f"SELECT TOP 1 1 FROM dbo.[{ref_table}] WHERE [{ref_col}] = ?{extra}", (row_id,)):
            raise ValueError(f"{key} is used by existing records in the database ({ref_table}) and can't be deleted: make it inactive instead")


class _Probe(dict):
    """A row that has every key, to learn which desktop columns a master's value function writes."""
    def __contains__(self, key):
        return True

    def __getitem__(self, key):
        return ""

    def get(self, key, default=None):
        return ""


# Columns the desktop fills or keys on: never carried as extras.
_NOT_EXTRA = {"addedby", "addedon", "changedby", "changedon", "authorised", "authorisedby", "authorisedon", "isdelete"}
_mapped_cache: dict[str, set] = {}


def _mapped(table: str) -> set:
    """Lower-case desktop columns the web app already models (typed fields, keys, audit columns) for a store table."""
    if table not in _mapped_cache:
        web_table, id_col, code_col, values = LEGACY_MASTERS[table]
        try:
            written = set(values(None, _Probe()))
        except Exception:   # a value function that needs a real row: carry no extras for it rather than guess
            written = None
        _mapped_cache[table] = None if written is None else (
            {c.lower() for c in written} | {id_col.lower(), code_col.lower()} | _NOT_EXTRA) - _unmodelled(table)
    return _mapped_cache[table]


def _unmodelled(table: str) -> set:
    """Columns the value function can write but the master's request model has no field for (a customer's GSTNO and
    City): the screen has no typed input for them, so they are carried as extras like any other column the designer
    lists, instead of being invisible."""
    from . import field_columns, schemas, security
    path = next((p for p, name in security.TABLES.items() if name == table), "")
    model = security._models(schemas).get(path)
    if not path or model is None:
        return set()
    typed = set(model.model_fields)
    return {col.lower() for key, col in field_columns._EXPLICIT.get(path, {}).items()
            if key not in typed and key not in ("code", "name")}


def _load_extras() -> None:
    """Attach each master row's other desktop columns as `extras` (columns added in the database or by the Doc
    Designer), so the screens can show them. Only the columns the web app does not already model are read, and a
    table that has none is not queried at all, so a normal shop pays for no extra SELECT. The database wins over
    what memory holds, except for a row with an unsaved edit."""
    from . import schema_meta
    for table, (web_table, id_col, code_col, values) in LEGACY_MASTERS.items():
        skip = _mapped(table)
        data = getattr(store, table, None)
        if skip is None or not data:
            continue
        wanted = [c["name"] for c in schema_meta.columns(web_table)
                  if c["name"].lower() not in skip and schema_meta.editable(c)]
        if not wanted:
            continue
        try:
            rows = db.fetchall(f"SELECT [{code_col}], " + ", ".join(f"[{c}]" for c in wanted) + f" FROM dbo.[{web_table}] WITH (NOLOCK)")
        except Exception as e:   # a table the shop's database does not have
            logger.warning("Extras of %s not read: %s", web_table, e)
            continue
        for r in rows:
            key = str(r.get(code_col) if code_col in r else "").strip()
            row = data.get(key)
            if row is None or key in store.EDITS.get(table, ()):   # a row with an unsaved edit keeps it
                continue
            row["extras"] = {k: (v.strip() if isinstance(v, str) else v) for k, v in r.items()
                             if k != code_col and isinstance(v, (str, int, float, bool, type(None), Decimal, datetime, date))}


def reload_extras() -> None:
    """Read the masters' extra columns again (a column was added or dropped in the database)."""
    with _lock:
        _mapped_cache.clear()
        if _active:
            _load_extras()
            if "*" not in store.EDITS.get("COMPANY", ()):   # not while an edit of it is waiting to be saved
                try:
                    db_store.load_company()
                except Exception as e:   # noqa: BLE001 - the company keeps what it had
                    logger.warning("Company not read again: %s", e)


def _write_company(cur, user: str | None) -> None:
    """Company Master edits go to the desktop's own MstComp row (so the desktop sees them), covering the typed fields and
    the extra columns (the address rows are Address1-3, as the desktop's Company Info grid has them). State, licence and map are web-only."""
    c = store.COMPANY
    vals = {"CompName": c.get("name", ""), "EMAIL": c.get("email", ""), "Address1": c.get("address1", ""), "Address2": c.get("address2", ""),
            "Address3": c.get("address3", ""), "TELNO": c.get("phone", ""), "FAXNO": c.get("fax", ""),
            "MOBNO": c.get("mobile", ""), "WEB": c.get("website", ""), "GSTNO": c.get("gstin", ""), "PANNO": c.get("pan", ""),
            "CINNO": c.get("cin", ""), "PHNO": c.get("excise_phone", ""), "RTLCODE": c.get("ksbcl_code", ""),
            **{k: v for k, v in (c.get("extras") or {}).items() if k.lower() not in db_store.COMPANY_COLUMNS}}
    _update(cur, "MstComp", {**vals, "ChangedBy": user or "", "ChangedOn": datetime.now()}, "CompID = ?", (1,))


def _write_master(cur, table: str, key: str, row: dict | None, user: str | None) -> None:
    web_table, id_col, code_col, values = LEGACY_MASTERS[table]
    if row is None:
        _check_unused(cur, table, id_col, web_table, code_col, key)
        cur.execute(f"DELETE FROM dbo.[{web_table}] WHERE [{code_col}] = ?", (key,))
        return
    vals = values(cur, row)
    skip = _mapped(table)
    if skip is not None and isinstance(row.get("extras"), dict):   # columns the database has beyond the web's own fields
        vals = {**{c: v for c, v in row["extras"].items() if c.lower() not in skip}, **vals}   # _fit drops unknown ones
    existing = _one(cur, f"SELECT TOP 1 [{id_col}] FROM dbo.[{web_table}] WHERE [{code_col}] = ?", (key,))
    before = None
    if existing is not None and table == "ITEMS":   # the rates before this save, for the rate-change log below
        cur.execute("SELECT MRP, PURRATE FROM dbo.MstITEM WHERE ITEMID = ?", (existing,))
        before = cur.fetchone()
    if existing is not None:
        _update(cur, web_table, {**vals, "ChangedBy": user or "", "ChangedOn": datetime.now()}, f"[{id_col}] = ?", (existing,))
        new_id = existing
    else:
        new_id = int(_one(cur, f"SELECT ISNULL(MAX([{id_col}]), 0) + 1 FROM dbo.[{web_table}] WITH (UPDLOCK, HOLDLOCK)", ()))
        _insert(cur, web_table, {id_col: new_id, code_col: key, **vals, "AddedBy": user or "", "AddedOn": datetime.now(),
                                 "Authorised": "Y"})
    if table in NEW_ID_FIELD:
        row[NEW_ID_FIELD[table]] = str(new_id)
    if table == "ITEMS":
        _write_item_extras(cur, int(new_id), key, row, vals, before, user)


def _write_item_extras(cur, item_id: int, code: str, row: dict, vals: dict, before, user: str | None) -> None:
    """What vLQMS.exe's item form saves beside MstITEM: the item's aliases (MstItemAlias: the old rows are replaced) and, when
    the MRP or the purchase rate changed, a line in the rate-change log (MstItemAudit: old and new rates, who and where)."""
    if isinstance(row.get("aliases"), list) and _meta("MstItemAlias"):
        cur.execute("DELETE FROM dbo.MstItemAlias WHERE ItemID = ?", (item_id,))
        seen = set()
        for alias in row["aliases"]:
            text = str(alias).strip()[:50]
            if text and text.lower() not in seen:
                seen.add(text.lower())
                cur.execute("INSERT INTO dbo.MstItemAlias (ItemID, ItemAlias) VALUES (?, ?)", (item_id, text))
    if before is not None and _meta("MstItemAudit"):
        old_mrp, old_rate = _num(before[0]), _num(before[1])
        new_mrp, new_rate = _num(vals.get("MRP", old_mrp)), _num(vals.get("PURRATE", old_rate))
        if (old_mrp, old_rate) != (new_mrp, new_rate):
            import socket
            cur.execute("INSERT INTO dbo.MstItemAudit (ItemID, ItemCode, ItemName, ISource, OldMRP, OldPurRate, NewMRP, NewPurRate, CSystemName, CUserName) "
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                        (item_id, code[:250], str(row.get("name") or "")[:250], "From Item Master", old_mrp, old_rate, new_mrp, new_rate,
                         socket.gethostname()[:50], (user or "")[:50]))


# ─── Documents in legacy format ──────────────────────────────────────────────

_short_no = db_store.short_no   # the desktop's varchar(12) header numbers (see db_store.short_no)


def _prefix(doc_no: str) -> str:
    part = doc_no.split("/", 1)[-1]
    return part.rsplit("-", 1)[0] if "-" in part else ""


def _item_ids(cur, code: str) -> tuple[int, int]:
    """(ITEMID, UOMID) of an item by code (0, 0 when it isn't in MstITEM)."""
    it = store.ITEMS.get(code) or {}
    if it.get("_itemid"):
        cur.execute("SELECT ITEMID, UOMID FROM dbo.MstITEM WHERE ITEMID = ?", (int(it["_itemid"]),))
    else:
        cur.execute("SELECT TOP 1 ITEMID, UOMID FROM dbo.MstITEM WHERE ITEMCODE = ?", (code,))
    r = cur.fetchone()
    return (int(r[0]), int(r[1] or 0)) if r else (0, 0)


# Header columns the writers below fill themselves: a designer-added column is anything else (`doc["extras"]`).
DOC_CORE = {
    "trnhdrsinv": {"sinvid", "sinvno", "prefix", "printsinvno", "sinvdate", "sinvdatetime", "custid", "partyname", "scid", "spid",
                   "locid", "addedby", "addedon", "totqty", "totbasicamt", "totcgstamt", "totsgstamt", "cgst", "sgst", "totdiscamt",
                   "disc", "totamt", "grndtot", "invvalue", "tot", "cashamt", "cardamt", "othpayamt", "rcvdamt", "paybackamt",
                   "billsystem", "saltyp", "authorised", "cancelled", "isdelete", "cancelrmks", "changedby", "changedon"},
    "trnhdrpi": {"piid", "pino", "prefix", "printpino", "pidate", "suppid", "partyname", "locid", "addedby", "addedon", "totamt", "othamt",
                 "grndtot", "totcgst", "totsgst", "totigst", "billsystem", "rmks", "amtinwrds", "cancelled", "authorised"},
    "trnhdrsr": {"srid", "srno", "prefix", "printsrno", "srdate", "locid", "addedby", "addedon", "totamt", "billsystem", "scid", "spid",
                 "cancelled", "authorised"},
    "trnhdrstkded": {"stkdedid", "stkdedno", "prefix", "printstkdedno", "stkdeddate", "locid", "addedby", "addedon", "totqty", "scid",
                     "spid", "billsystem", "cancelled", "authorised"},
    "trnhdrpb": {"pbid", "pbno", "prefix", "printpbno", "pbdate", "suppid", "partyname", "locid", "addedby", "addedon", "suppinvno",
                 "suppinvdate", "suppinvvalue", "purchasetype", "totamt", "tot", "totcgst", "totsgst", "tcsamt", "totcess", "grndtot",
                 "totnet", "rmks", "cancelled", "authorised"},
}


_DOC_BLOCK = re.compile(r"(^tot|id$|amt$|net$|disc|gst$|cess|tcs|round|qty$|value$|^cancel|^is)", re.I)


def doc_blocked(column: str) -> bool:
    """A bill header column that holds a total, a key or a flag: never offered or written as a free-form extra."""
    return bool(_DOC_BLOCK.search(column))


def _doc_extras(doc: dict) -> dict:
    """The designer-added header columns a bill carries (already checked when the bill was accepted)."""
    return dict(doc.get("extras") or {})


def _write_sales_invoice(cur, doc: dict, user: str | None) -> None:
    no, status = doc["doc_no"], doc.get("status", "SAVED")
    sinv_id = _one(cur, "SELECT SINVID FROM dbo.TrnHdrSINV WHERE SINVNO = ?", (no,))
    flags = {"Cancelled": _yn(status in ("CANCELLED", "DELETED")), "IsDelete": _yn(status == "DELETED"),
             "CancelRmks": doc.get("cancel_remarks") or "", "ChangedBy": user or "", "ChangedOn": datetime.now()}
    if sinv_id is not None:
        _update(cur, "TrnHdrSINV", flags, "SINVID = ?", (sinv_id,))
        return
    tot, pay = doc.get("totals", {}), doc.get("payment", {})
    cust = store.CUSTOMERS.get(doc.get("customer")) or {}
    when = f"{doc['date']} {doc.get('time') or '00:00:00'}"
    sinv_id = _insert(cur, "TrnHdrSINV", {
        **_doc_extras(doc), "SINVNO": no, "Prefix": _prefix(no), "PrintSINVNO": no, "SINVDate": doc["date"], "SINVDateTime": when,
        "CUSTID": _lookup(cur, "MstCUST", "CUSTID", doc.get("customer"), "CUSTCODE"), "PartyName": cust.get("name") or doc.get("customer") or "",
        "SCID": int((store.COUNTERS.get(doc.get("counter")) or {}).get("_scid") or 0),
        "SPID": _lookup(cur, "MstSP", "SPID", doc.get("salesperson"), "SPCODE", "SPNAME"), "LOCID": LOCID, "AddedBy": doc.get("added_by") or user or "", "AddedOn": datetime.now(),
        "TOTQTY": _num(tot.get("qty")), "TOTBASICAMT": _num(tot.get("basic")), "TOTCGSTAMT": _num(tot.get("cgst")),
        "TOTSGSTAMT": _num(tot.get("sgst")), "CGST": _num(tot.get("cgst")), "SGST": _num(tot.get("sgst")),
        "TOTDISCAMT": _num(tot.get("discount")), "DISC": _num(tot.get("discount")), "TOTAMT": _num(tot.get("net")),
        "GRNDTOT": _num(tot.get("net")), "INVVALUE": _num(tot.get("net")), "TOT": _num(tot.get("basic")),
        "CASHAMT": _num(pay.get("cash")), "CARDAMT": _num(pay.get("card")), "OTHPAYAMT": _num(pay.get("upi")) + _num(pay.get("credit")),
        "RCVDAMT": _num(tot.get("collected")), "PAYBACKAMT": _num(tot.get("payback")), "BILLSYSTEM": "WEB",
        "SALTYP": doc.get("payment_type") or "", "Authorised": "Y", **flags,
    }, identity_out="SINVID")
    _write_invoice_sidecars(cur, doc, sinv_id)
    for n, l in enumerate(doc.get("lines", []), start=1):
        item_id, uom_id = _item_ids(cur, l["item_code"])
        amount = _num(l.get("amount"))
        _insert(cur, "TrnDtlSINV", {
            "SINVID": sinv_id, "SlNo": n, "ItemSlNO": n, "ITEMID": item_id, "UOMID": uom_id, "SINVQty": _num(l.get("qty")),
            "STKQty": _num(l.get("qty")), "SINVRate": _num(l.get("rate")), "SINVAmt": amount, "MRP": _num(l.get("rate")),
            "CGSTAMT": _num(l.get("cgst")), "SGSTAMT": _num(l.get("sgst")), "SINVQtyInLtrs": _num(l.get("litres")),
            "ITEMAMT": amount, "NETTOT": round(amount + _num(l.get("cgst")) + _num(l.get("sgst")), 2),
        })


def _write_invoice_sidecars(cur, doc: dict, sinv_id: int) -> None:
    """The other rows vLQMS.exe writes with a sales invoice (frmTrnInv): the loyalty points earned (TrnPoints: bill date,
    customer, invoice id and number, bill amount, points) and each part of a split payment other than cash and card
    (TrnMultiAmt, under the designer field OTHPAYAMT). Cash denominations (TrnDtlSINVDenom) are not captured by the web screen."""
    tot, pay = doc.get("totals", {}), doc.get("payment", {})
    customer_id = _lookup(cur, "MstCUST", "CUSTID", doc.get("customer"), "CUSTCODE")
    earned = _num(tot.get("points_earned"))
    if earned and customer_id and doc.get("customer") != "CASH" and _meta("TrnPoints"):
        _insert(cur, "TrnPoints", {"DocDate": doc["date"], "CustID": customer_id, "SInvID": sinv_id, "sInvNo": doc["doc_no"],
                                   "sInvdate": doc["date"], "InvAmt": _num(tot.get("net")), "Points": round(earned), "Remarks": ""})
    doc_id = doc_engine.doc_id_of("Sales Invoice")
    slno = 0
    for part in ("upi", "credit"):
        if _num(pay.get(part)) > 0 and doc_id is not None and _meta("TrnMultiAmt"):
            slno += 1
            _insert(cur, "TrnMultiAmt", {"DocID": doc_id, "TrnID": sinv_id, "FldCode": "OTHPAYAMT", "Slno": slno,
                                         "Particulars": part.upper(), "Amount": _num(pay.get(part))})


def _write_purchase_bill(cur, doc: dict, user: str | None) -> None:
    no = doc["doc_no"]
    calc = _calc("purchase_bill", doc)
    if _one(cur, "SELECT PBID FROM dbo.TrnHdrPB WHERE PBNO = ?", (_short_no(no),)) is not None:
        return   # purchase bills are not changed after saving
    tot = doc.get("totals", {})
    supp = store.find(store.SUPPLIERS, doc.get("supplier") or "") or {}
    pb_id = _insert(cur, "TrnHdrPB", _with(calc["header"], {
        **_doc_extras(doc), "PBNO": _short_no(no), "Prefix": _prefix(no), "PrintPBNO": no, "PBDate": doc["date"],
        "SUPPID": _lookup(cur, "MstSUPP", "SUPPID", supp.get("code") or doc.get("supplier"), "SUPPCODE", "SUPPNAME"),
        "PartyName": supp.get("name") or doc.get("supplier") or "", "LOCID": LOCID, "AddedBy": user or "", "AddedOn": datetime.now(),
        "SUPPINVNO": doc.get("supplier_inv_no") or "", "SUPPINVDATE": doc.get("supplier_inv_date") or None,
        "SUPPINVVALUE": _num(doc.get("supplier_inv_value")), "PURCHASETYPE": doc.get("purchase_type") or "Purchase",
        "TOTAMT": _num(tot.get("basic")), "TOT": _num(tot.get("basic")), "TOTCGST": _num(tot.get("cgst")), "TOTSGST": _num(tot.get("sgst")),
        "TCSAMT": _num(tot.get("tcs")), "TOTCESS": _num(tot.get("cess")), "GRNDTOT": _num(tot.get("net")), "TOTNET": _num(tot.get("net")),
        "RMKS": doc.get("remarks") or "", "Cancelled": "N", "Authorised": "Y",
    }), identity_out="PBID")
    for n, l in enumerate(doc.get("lines", []), start=1):
        item_id, uom_id = _item_ids(cur, l["item_code"])
        _insert(cur, "TrnDtlPB", _with(calc["rows"][n - 1] if n - 1 < len(calc["rows"]) else {}, {
            "PBID": pb_id, "SlNo": n, "ItemSlNO": n, "ITEMID": item_id, "UOMID": uom_id, "PBQty": _num(l.get("qty")),
            "StkQty": _num(l.get("qty")), "PBRate": _num(l.get("rate")), "PBAmt": _num(l.get("amount")),
            "NOOFCASES": _num(l.get("cases")), "NOOFBOTTLES": _num(l.get("bottles")), "PBQtyInLtrs": _num(l.get("litres")),
            "CGSTAMT": _num(l.get("cgst")), "SGSTAMT": _num(l.get("sgst")),
            "NETAMT": round(_num(l.get("amount")) + _num(l.get("cgst")) + _num(l.get("sgst")), 2),
        }))


def _with(over: dict, base: dict) -> dict:
    """`base` with the designer's computed columns laid over it (the formulas decide what is stored)."""
    return {**base, **over}


def _calc(kind: str, doc: dict) -> dict:
    """The document's formula columns (app/doc_engine.py); empty when the designer or the database has none."""
    try:
        return doc_engine.compute(kind, doc)
    except Exception as e:   # noqa: BLE001 - a formula problem must never stop a bill being saved
        logger.warning("Formula columns of %s not worked out: %s", doc.get("doc_no"), e)
        return {"header": {}, "rows": [], "problems": [str(e)]}


def _write_purchase_indent(cur, doc: dict, user: str | None) -> None:
    no = doc["doc_no"]
    calc = _calc("purchase_indent", doc)
    if _one(cur, "SELECT PIID FROM dbo.TrnHdrPI WHERE PINO = ?", (_short_no(no),)) is not None:
        return
    tot = doc.get("totals", {})
    supp = store.find(store.SUPPLIERS, doc.get("supplier") or "") or {}
    pi_id = _insert(cur, "TrnHdrPI", _with(calc["header"], {
        **_doc_extras(doc), "PINO": _short_no(no), "Prefix": _prefix(no), "PrintPINO": no, "PIDate": doc["date"],
        "SUPPID": _lookup(cur, "MstSUPP", "SUPPID", supp.get("code") or doc.get("supplier"), "SUPPCODE", "SUPPNAME"),
        "PartyName": supp.get("name") or doc.get("supplier") or "", "LOCID": LOCID, "AddedBy": user or "", "AddedOn": datetime.now(),
        "TOTAMT": _num(tot.get("basic")), "GRNDTOT": _num(tot.get("net")), "RMKS": doc.get("remarks") or "",
        "BILLSYSTEM": "WEB", "Cancelled": "N", "Authorised": "Y",
    }), identity_out="PIID")
    for n, l in enumerate(doc.get("lines", []), start=1):
        item_id, uom_id = _item_ids(cur, l["item_code"])
        _insert(cur, "TrnDtlPI", _with(calc["rows"][n - 1] if n - 1 < len(calc["rows"]) else {}, {
            "PIID": pi_id, "SlNo": n, "ItemSlNO": n, "ITEMID": item_id, "UOMID": uom_id, "PIQty": _num(l.get("qty")),
            "StkQty": _num(l.get("qty")), "PIRate": _num(l.get("rate")), "PIAmt": _num(l.get("amount")),
            "NOOFCASES": _num(l.get("cases")), "NOOFBOTTLES": _num(l.get("bottles")), "NETAMT": _num(l.get("amount")),
        }))


def _write_sales_return(cur, doc: dict, user: str | None) -> None:
    no = doc["doc_no"]
    calc = _calc("sales_return", doc)
    if _one(cur, "SELECT SRID FROM dbo.TrnHdrSR WHERE SRNO = ?", (_short_no(no),)) is not None:
        return
    tot = doc.get("totals", {})
    sr_id = _insert(cur, "TrnHdrSR", _with(calc["header"], {
        **_doc_extras(doc), "SRNO": _short_no(no), "Prefix": _prefix(no), "PrintSRNO": no, "SRDate": doc["date"], "LOCID": LOCID,
        "AddedBy": user or "", "AddedOn": datetime.now(), "TOTAMT": _num(tot.get("net")), "BILLSYSTEM": "WEB",
        "Cancelled": "N", "Authorised": "Y",
    }), identity_out="SRID")
    for n, l in enumerate(doc.get("lines", []), start=1):
        item_id, uom_id = _item_ids(cur, l["item_code"])
        _insert(cur, "TrnDtlSR", _with(calc["rows"][n - 1] if n - 1 < len(calc["rows"]) else {}, {
            "SRID": sr_id, "SlNo": n, "ItemSlNO": n, "ITEMID": item_id, "UOMID": uom_id, "SRQty": _num(l.get("qty")),
            "StkQty": _num(l.get("qty")), "SRRate": _num(l.get("rate")), "SRAmt": _num(l.get("amount")), "SRQTYINLTRS": _num(l.get("litres")),
        }))


def _write_stock_deduction(cur, doc: dict, user: str | None) -> None:
    no = doc["doc_no"]
    calc = _calc("stock_deduction", doc)
    if _one(cur, "SELECT STKDEDID FROM dbo.TrnHdrSTKDED WHERE STKDEDNO = ?", (_short_no(no),)) is not None:
        return
    tot = doc.get("totals", {})
    sd_id = _insert(cur, "TrnHdrSTKDED", _with(calc["header"], {
        **_doc_extras(doc), "STKDEDNO": _short_no(no), "Prefix": _prefix(no), "PrintSTKDEDNO": no, "STKDEDDate": doc["date"], "LOCID": LOCID,
        "AddedBy": user or "", "AddedOn": datetime.now(), "TOTQTY": _num(tot.get("qty")), "BILLSYSTEM": "WEB",
        "Cancelled": "N", "Authorised": "Y",
    }), identity_out="STKDEDID")
    for n, l in enumerate(doc.get("lines", []), start=1):
        item_id, uom_id = _item_ids(cur, l["item_code"])
        _insert(cur, "TrnDtlSTKDED", _with(calc["rows"][n - 1] if n - 1 < len(calc["rows"]) else {}, {
            "STKDEDID": sd_id, "SlNo": n, "ItemSlNO": n, "ITEMID": item_id, "UOMID": uom_id, "STKDEDQty": _num(l.get("qty")),
            "StkQty": _num(l.get("qty")), "STKDEDQTYINLTRS": _num(l.get("litres")),
        }))


_MIRROR_WRITERS = {"sales_invoice": _write_sales_invoice, "purchase_bill": _write_purchase_bill,
                   "purchase_indent": _write_purchase_indent, "sales_return": _write_sales_return,
                   "stock_deduction": _write_stock_deduction}


def _write_move(cur, seq: int, m: dict) -> None:
    extra = {k: v for k, v in m.items() if k not in ("item", "qty", "kind", "doc_no", "date", "rate")}
    cur.execute("INSERT INTO dbo.Web_AppMove (Seq, ItemCode, Qty, Kind, DocNo, MoveDate, Rate, Extra) VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
                (seq, m["item"], m["qty"], m["kind"], m.get("doc_no"), m["date"], m.get("rate"), _json(extra) if extra else None))
    item_id, _ = _item_ids(cur, m["item"])
    if not item_id:
        return
    if m["kind"] == "sale-cancel":   # as the desktop does: the bill's stock rows go (the trigger puts the stock back)
        cur.execute("DELETE FROM dbo.UtlStockList WHERE DocType = 'INV' AND DocNo = ? AND ItemID = ? AND StkMultiplier = -1",
                    (m.get("doc_no"), item_id))
        return
    doc_type = MOVE_DOCTYPE.get(m["kind"], m["kind"].upper())
    doc_id = 0
    if doc_type == "INV":
        doc_id = _one(cur, "SELECT SINVID FROM dbo.TrnHdrSINV WHERE SINVNO = ?", (m.get("doc_no"),)) or 0
    elif doc_type == "PB":
        doc_id = _one(cur, "SELECT PBID FROM dbo.TrnHdrPB WHERE PBNO = ?", (_short_no(str(m.get("doc_no") or "")),)) or 0
    qty = abs(_num(m["qty"]))
    _insert(cur, "UtlStockList", {
        "ItemID": item_id, "StkMultiplier": 1 if m["qty"] >= 0 else -1, "DocType": doc_type, "DocDate": m["date"],
        "DocID": doc_id, "DocNo": m.get("doc_no") or "", "Particulars": f"{WEB_ROW}{m['kind']}", "Qty": qty,
        "Rate": _num(m.get("rate")), "DocValue": round(qty * _num(m.get("rate")), 2), "LOCID": LOCID,
    })


# ─── Item merge (Utilities → Merge Items) ────────────────────────────────────

_pending_merges: list[tuple[str, str]] = []


def merged(src: str, dst: str) -> None:
    """Merge Items moved src's documents, movements and stock to dst in memory: do the same in SQL Server on
    this request's save (every table with an ITEMID, the stock balance, the web ledger)."""
    _pending_merges.append((src, dst))


def _write_merge(cur, src: str, dst: str) -> None:
    src_id = _one(cur, "SELECT TOP 1 ITEMID FROM dbo.MstITEM WHERE ITEMCODE = ?", (src,))
    dst_id = _one(cur, "SELECT TOP 1 ITEMID FROM dbo.MstITEM WHERE ITEMCODE = ?", (dst,))
    cur.execute("UPDATE dbo.Web_AppMove SET ItemCode = ? WHERE ItemCode = ?", (dst, src))
    if src_id is None or dst_id is None:
        return
    cur.execute("""UPDATE d SET CLBal = d.CLBal + s.CLBal, OPQty = d.OPQty + s.OPQty, TrnQty = d.TrnQty + s.TrnQty
                   FROM dbo.TrnQOH d JOIN dbo.TrnQOH s ON s.LOCID = d.LOCID AND s.ITEMID = ? WHERE d.ITEMID = ?""", (src_id, dst_id))
    cur.execute("""UPDATE dbo.TrnQOH SET ITEMID = ? WHERE ITEMID = ?
                   AND NOT EXISTS (SELECT 1 FROM dbo.TrnQOH q WHERE q.ITEMID = ? AND q.LOCID = TrnQOH.LOCID)""", (dst_id, src_id, dst_id))
    cur.execute("DELETE FROM dbo.TrnQOH WHERE ITEMID = ?", (src_id,))
    tables = [r["t"] for r in db.fetchall(
        """SELECT t.name AS t FROM sys.tables t JOIN sys.columns c ON c.object_id = t.object_id
           WHERE t.name NOT LIKE 'Web[_]%' AND LOWER(c.name) = 'itemid' AND t.name NOT IN ('MstITEM', 'TrnQOH')""")]
    # An UPDATE trigger on UtlStockList (if the desktop has one) would move stock again for the re-pointed rows; the
    # balance was moved above. Only those triggers are switched off, by name, and only for this transaction. The
    # desktop's INSERT/DELETE triggers don't fire on an UPDATE.
    update_triggers = [r["n"] for r in db.fetchall("SELECT t.name AS n FROM sys.triggers t JOIN sys.trigger_events e ON e.object_id = t.object_id WHERE t.parent_id = OBJECT_ID('dbo.UtlStockList') AND e.type_desc = 'UPDATE'")]
    for name in update_triggers:
        cur.execute_raw(f"DISABLE TRIGGER [{name}] ON dbo.UtlStockList")
    for t in tables:
        col = next(m["name"] for m in _meta(t).values() if m["name"].lower() == "itemid")
        # Where ITEMID is part of a unique key (e.g. MstItemUOM (ItemID, UOMID)) and the kept item already has
        # the same row, the removed item's row goes, as the desktop drops the old item's dependent rows.
        for key in _unique_keys(t):
            if col.lower() in (k.lower() for k in key):
                rest = [k for k in key if k.lower() != col.lower()]
                same = " AND ".join(f"d.[{k}] = s.[{k}]" for k in rest) or "1 = 1"
                cur.execute(f"DELETE s FROM dbo.[{t}] s WHERE s.[{col}] = ? AND EXISTS "
                            f"(SELECT 1 FROM dbo.[{t}] d WHERE d.[{col}] = ? AND {same})", (src_id, dst_id))
        cur.execute(f"UPDATE dbo.[{t}] SET [{col}] = ? WHERE [{col}] = ?", (dst_id, src_id))
    for name in update_triggers:
        cur.execute_raw(f"ENABLE TRIGGER [{name}] ON dbo.UtlStockList")


def _unique_keys(table: str) -> list[list[str]]:
    rows = db.fetchall("""SELECT i.index_id, c.name FROM sys.indexes i
                          JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
                          JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                          WHERE i.object_id = OBJECT_ID(?) AND i.is_unique = 1 ORDER BY i.index_id, ic.key_ordinal""", (f"dbo.{table}",))
    keys: dict[int, list[str]] = {}
    for r in rows:
        keys.setdefault(r["index_id"], []).append(r["name"])
    return list(keys.values())


def moves_rewritten() -> None:
    """Re Write Stock replaced the web's stock moves: the next save writes the whole ledger again."""
    global _rewrite_moves
    _rewrite_moves = True


# ─── Can a write be saved now? ───────────────────────────────────────────────

def writes_blocked() -> str | None:
    """Why a write request must be refused now, or None. Without a database set up (development, the tests)
    writes are allowed and not saved. With one set up, a write is only allowed while the data is loaded
    from it; if the connection was lost, it tries to come back (at most every RETRY_SECONDS)."""
    global _retry_at
    if not db.configured() or enabled():
        return None
    if db.MANUAL_OFFLINE:
        return "Disconnected from the database for testing (Utilities → About → Reconnect). Nothing can be saved."
    if time.monotonic() >= _retry_at:
        _retry_at = time.monotonic() + RETRY_SECONDS
        try:
            if db.reconnect() and setup_db.is_ready():
                reload()
                if enabled():
                    return None
        except Exception:   # noqa: BLE001 - still offline
            logger.warning("The database is still unreachable")
    return "Database offline: the shop's SQL Server can't be reached, so nothing can be saved. Try again in a moment."


# ─── Save ────────────────────────────────────────────────────────────────────

def _row_value(row) -> str | None:
    if row is None:
        return None
    if isinstance(row, dict):
        row = {k: v for k, v in row.items() if k not in SECRET_FIELDS}
    return _json(row)


def _sections() -> dict:
    return {"settings": store.SETTINGS, "gsettings": store.GSETTINGS, "denominations": store.DENOMINATIONS,
            "seq": {k: s.value for k, s in store._seq.items()}, "security": store.SECURITY,
            "field_rights": store.FIELD_RIGHTS, "master_approval": store.MASTER_APPROVAL, "report_rights": store.REPORT_RIGHTS,
            "doc_settings": store.DOC_SETTINGS, "number_series": store.NUMBER_SERIES, "menu_layout": store.MENU_LAYOUT, "print_prefs": store.PRINT_PREFS,
            "import_fields": store.IMPORT_FIELDS, "merged_items": store.MERGED_ITEMS, "merged_item_ids": store.MERGED_ITEM_IDS,
            "company": store.COMPANY if "*" in store.EDITS.get("COMPANY", ()) else None}


def _upsert(cur, table: str, keys: dict, values: dict) -> None:
    where = " AND ".join(f"[{k}] = ?" for k in keys)
    if not _update(cur, table, values, where, tuple(keys.values())):
        _insert(cur, table, {**keys, **values})


def save(user: str | None = None) -> None:
    """Write the request's changes (see the module doc) in one transaction. Raises SaveFailed."""
    global _moves_saved, _rewrite_moves
    with _lock:
        audit = list(store.AUDIT_OUT)
        store.AUDIT_OUT.clear()
        if not enabled():
            if db.configured():   # set up but not in use (writes_blocked() should have refused it)
                _pending_merges.clear()
                raise SaveFailed("the database is offline")
            _memory_audit.extend(audit)
            _pending_merges.clear()
            return
        merges = list(_pending_merges)
        _pending_merges.clear()
        try:
            # What changed since the last save.
            for attempt in range(3):   # the background loader may be changing DOCS at the same moment
                try:
                    rows = [(t, k, _row_value(getattr(store, t).get(k))) for t in TABLES for k in store.EDITS.get(t, ())]
                    docs = {(kind, no): d for kind, ds in store.DOCS.items() for no, d in list(ds.items()) if "origin" not in d}
                    present = {(kind, no) for kind, ds in store.DOCS.items() for no in list(ds)}
                    flagged = [d for d in list(store.DOCS.get("sales_invoice", {}).values())
                               if "origin" in d and _doc_status.get(("sales_invoice", d["doc_no"]), "SAVED") != d.get("status", "SAVED")]
                    break
                except RuntimeError:
                    if attempt == 2:
                        raise
            rows = [(t, k, v) for t, k, v in rows if _saved.get(("row", t, k)) != (_hash(v) if v is not None else None)]
            doc_json = {key: _json(d) for key, d in docs.items()}
            new_docs = {key: v for key, v in doc_json.items() if _saved.get(("doc", *key)) != _hash(v)}
            # Documents that left the app (Remove Duplicate Invoice with KeepDelData = N). A number still in memory
            # under any origin is not gone.
            gone_docs = [(k[1], k[2]) for k in list(_saved) if k[0] == "doc" and (k[1], k[2]) not in present]
            sections = {n: _json(v) for n, v in _sections().items() if v is not None}
            new_sections = {n: v for n, v in sections.items() if _saved.get(("state", n)) != _hash(v)}
            app_moves = store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]
            rewrite = _rewrite_moves
            new_moves = app_moves if rewrite else app_moves[_moves_saved:]
            if not (rows or new_docs or gone_docs or flagged or new_sections or new_moves or merges or audit or rewrite):
                return
            for attempt in (1, 2):
                try:
                    with db.transaction() as cur:
                        for src, dst in merges:   # first: the removed item's MstITEM row goes with the rows below
                            _write_merge(cur, src, dst)
                        for table, key, value in rows:
                            if table in LEGACY_MASTERS:
                                _write_master(cur, table, key, getattr(store, table).get(key), user)
                                value = _row_value(getattr(store, table).get(key))   # may now carry the new _itemid
                            _upsert(cur, "Web_AppRow", {"Kind": table, "RowKey": key},
                                    {"Value": value, "ChangedAt": datetime.now(), "ChangedBy": user})
                        if "*" in store.EDITS.get("COMPANY", ()):
                            _write_company(cur, user)
                        for (kind, no), text in new_docs.items():
                            d = docs[(kind, no)]
                            _upsert(cur, "Web_AppDoc", {"Kind": kind, "DocNo": no},
                                    {"DocDate": d.get("date"), "Status": d.get("status"), "Value": text, "ChangedAt": datetime.now(), "ChangedBy": user})
                            status = d.get("status", "SAVED")
                            if kind in MIRRORED_DOCS and status != "HOLD" and _doc_status.get((kind, no)) != status:
                                _MIRROR_WRITERS[kind](cur, d, user)
                        for d in flagged:
                            _write_sales_invoice(cur, d, user)
                        for kind, no in gone_docs:
                            cur.execute("DELETE FROM dbo.Web_AppDoc WHERE Kind = ? AND DocNo = ?", (kind, no))
                            if kind == "sales_invoice":
                                cur.execute("UPDATE dbo.TrnHdrSINV SET Cancelled = 'Y', IsDelete = 'Y' WHERE SINVNO = ?", (no,))
                        for n, text in new_sections.items():
                            _upsert(cur, "Web_AppSetting", {"Section": "state", "Key": n}, {"Value": text, "ChangedAt": datetime.now(), "ChangedBy": user})
                        if rewrite:   # the web's own stock rows go (the trigger takes their stock off) and are written again
                            cur.execute("DELETE FROM dbo.UtlStockList WHERE Particulars LIKE ?", (WEB_ROW + "%",))
                            cur.execute_raw("DELETE FROM dbo.Web_AppMove")
                        for i, m in enumerate(new_moves, start=0 if rewrite else _moves_saved):
                            _write_move(cur, i, m)
                        for e in audit:
                            _write_audit(cur, e)
                    break
                except Exception as exc:
                    # "Connection is busy with results for another command": the shared connection was left with
                    # unread results; a fresh connection fixes it, so try once more before giving up.
                    if attempt == 1 and "busy with results" in str(exc):
                        logger.warning("Connection busy (last SQL: %s); reconnecting and retrying the save", db.last_sql())
                        db.reconnect()
                        continue
                    logger.error("Failed SQL: %s", db.last_sql())
                    raise
        except Exception as exc:
            logger.exception("Saving to SQL Server failed; reloading from it")
            raise SaveFailed(db.safe_error(exc)) from exc
        # Committed: remember what SQL Server now holds.
        for table, key, value in rows:
            v = _row_value(getattr(store, table).get(key))
            _saved[("row", table, key)] = _hash(v) if v is not None else None
        for (kind, no), text in new_docs.items():
            _saved[("doc", kind, no)] = _hash(text)
            if kind in MIRRORED_DOCS and docs[(kind, no)].get("status", "SAVED") != "HOLD":
                _doc_status[(kind, no)] = docs[(kind, no)].get("status", "SAVED")
        for d in flagged:
            _doc_status[("sales_invoice", d["doc_no"])] = d.get("status", "SAVED")
        for key in gone_docs:
            _saved.pop(("doc", *key), None)
        for n, text in new_sections.items():
            _saved[("state", n)] = _hash(text)
        _moves_saved = len(app_moves)
        _rewrite_moves = False


# ─── Load ────────────────────────────────────────────────────────────────────

def load() -> bool:
    """Lay the web app's saved data over the legacy-format rows just loaded (db_store). True if anything was
    restored. Call after db_store.load_masters(); items' QOH then already includes the web's stock moves
    (TrnQOH), so they are taken off first and replayed, as Re Write Stock expects."""
    global _active, _moves_saved, _movements_base, _rewrite_moves
    with _lock:
        _rewrite_moves = False
        _saved.clear()
        _doc_status.clear()
        _cols.clear()
        _active = False
        _pending_merges.clear()
        if not (db.is_db_available() and setup_db.is_ready()):
            store.mark_app_start()
            return False
        rows: dict[str, dict] = {}
        for r in db.fetchall("SELECT Kind, RowKey, Value FROM dbo.Web_AppRow WITH (NOLOCK)"):
            rows.setdefault(r["Kind"], {})[r["RowKey"]] = json.loads(r["Value"]) if r["Value"] is not None else None
            _saved[("row", r["Kind"], r["RowKey"])] = _hash(r["Value"]) if r["Value"] is not None else None
        docs: dict[str, dict] = {}
        for r in db.fetchall("SELECT Kind, DocNo, Status, Value FROM dbo.Web_AppDoc WITH (NOLOCK)"):
            docs.setdefault(r["Kind"], {})[r["DocNo"]] = json.loads(r["Value"])
            _saved[("doc", r["Kind"], r["DocNo"])] = _hash(r["Value"])
            if r["Kind"] in MIRRORED_DOCS and r["Status"] != "HOLD":
                _doc_status[(r["Kind"], r["DocNo"])] = r["Status"] or "SAVED"
        state = {}
        for r in db.fetchall("SELECT [Key] AS k, Value FROM dbo.Web_AppSetting WITH (NOLOCK) WHERE Section = 'state'"):
            state[r["k"]] = json.loads(r["Value"])
            _saved[("state", r["k"])] = _hash(r["Value"])
        moves = []
        for r in db.fetchall("SELECT ItemCode, Qty, Kind, DocNo, MoveDate, Rate, Extra FROM dbo.Web_AppMove WITH (NOLOCK) ORDER BY Seq"):
            m = {"item": r["ItemCode"], "qty": float(r["Qty"]), "kind": r["Kind"], "doc_no": r["DocNo"],
                 "date": str(r["MoveDate"])[:10], "rate": float(r["Rate"]) if r["Rate"] is not None else 0.0}
            if r["Extra"]:
                m.update(json.loads(r["Extra"]))
            moves.append(m)
        # TrnQOH already has the web's moves: take them off, so apply() can replay them onto the base.
        for m in moves:   # the same item apply() adds them back to
            item = store.ITEMS.get(m["item"])
            if item is not None:
                item["qoh"] -= m["qty"]
        _movements_base = len(store.MOVEMENTS)
        apply({"rows": rows, "docs": docs, "movements": moves, **state})
        _moves_saved = len(moves)
        # Invoice numbers go on after the highest one already in the table (desktop or web), so a new web bill
        # never takes a number the old data has (SINVNO is the key; a clash would refuse the save).
        # The parameter is cast to the column's type: a Unicode parameter against the varchar SINVNO made SQL Server read the
        # whole 700,000-row table (76,504 pages, 24-27 s on a cold cache) instead of the 946 pages of the year's index range.
        top = db.fetchall("SELECT MAX(TRY_CAST(SUBSTRING(SINVNO, CHARINDEX('-', SINVNO) + 1, 20) AS INT)) AS n "
                          "FROM dbo.TrnHdrSINV WITH (NOLOCK) WHERE SINVNO LIKE CAST(? AS VARCHAR(20))", (f"{store.FY_PREFIX}/%",))
        if top and top[0]["n"]:
            seq = store._seq.setdefault("sales_invoice", store.Seq(int(top[0]["n"]) + 1))
            seq.value = max(seq.value, int(top[0]["n"]) + 1)
        _active = True
        logger.info("Loaded web data from SQL Server: %d documents, %d stock moves, %d rows",
                    sum(len(d) for d in docs.values()), len(moves), sum(len(r) for r in rows.values()))
        return bool(rows or docs or moves or state)


def remember_defaults() -> None:
    """Keep the store as it is before anything is loaded (boot.py: empty tables and the built-in config), so
    reload() can start from it."""
    global _defaults
    _defaults = {"tables": copy.deepcopy({n: v for n, v in vars(store).items() if n.isupper() and isinstance(v, (dict, list))}),
             "seq": {k: s.value for k, s in store._seq.items()}}


def _restore_defaults() -> None:
    if _defaults is None:
        return
    for name, saved in copy.deepcopy(_defaults["tables"]).items():
        current = getattr(store, name)
        if isinstance(current, dict):   # in place: the routers hold references to these
            current.clear()
            current.update(saved)
        else:
            current[:] = saved
    for kind, value in _defaults["seq"].items():
        store._seq.setdefault(kind, store.Seq(value)).value = max(store._seq[kind].value, value)


def reload() -> None:
    """After a failed save (or when the database comes back): throw every in-memory change away and load
    everything again from SQL Server, starting from the empty store as at startup."""
    from . import db_store
    with _lock:
        if not db.is_db_available() and not db.MANUAL_OFFLINE:
            db.reconnect()
        _restore_defaults()
        store.AUDIT_OUT.clear()
        _pending_merges.clear()
        ready = db.is_db_available() and setup_db.is_ready()
        if ready:
            db_store.load_masters()
        load()
        if ready:
            threading.Thread(target=lambda: (reload_extras(), db_store.load_transactions()), daemon=True).start()


def apply(data: dict) -> None:
    """Lay saved web data over the freshly loaded store (the same rules the app always used)."""
    store.MERGED_ITEMS.update(data.get("merged_items") or {})
    store.MERGED_ITEM_IDS.update(data.get("merged_item_ids") or {})
    merged_stock = {old: store.ITEMS[old]["qoh"] for old in store.MERGED_ITEMS if old in store.ITEMS}
    # 1. Master rows, users and roles. Item QOH comes from the database, not from the saved row.
    for table, changed in data.get("rows", {}).items():
        target = getattr(store, table, None)
        if target is None:
            continue
        for key, row in changed.items():
            store.touch(table, key)
            if row is None:
                target.pop(key, None)
                continue
            existing = target.get(key)
            if isinstance(row, dict):
                if table == "USERS" and existing is not None and not row.get("role_set_in_web"):
                    # A desktop user's role follows their desktop role (db_store._role_for) unless Admin Master
                    # (or the Database setup) set one; a save made only for a password must not freeze the role.
                    row = {k: v for k, v in row.items() if k != "role"}
                merged = {**(existing or {}), **row}
                if table == "ITEMS":
                    merged["qoh"] = (existing or {}).get("qoh", 0)
                target[key] = merged
            else:
                target[key] = row
    for old, qty in merged_stock.items():
        target = store.ITEMS.get(store.merged_code(old))
        if target is not None and old not in store.ITEMS:
            target["qoh"] += qty
    if data.get("company"):
        saved = dict(data["company"])
        for key in ("name", "email", "address", "address1", "address2", "address3", "phone", "fax", "mobile", "website", "gstin", "pan", "cin", "excise_phone", "ksbcl_code", "extras"):
            if store.COMPANY.get("name"):   # MstComp was read: the desktop's row wins over an older copy saved here
                saved.pop(key, None)
        if store.FIN_YEARS:   # the financial year is the desktop's (MstCompFinYear), not a copy saved earlier
            saved.pop("fy_start", None)
            saved.pop("fy_end", None)
        store.COMPANY.update(saved)   # not touched: the desktop's MstComp row already wins, nothing is waiting to be written
    store.set_fin_year()
    for section, values in (data.get("settings") or {}).items():
        store.SETTINGS.setdefault(section, {}).update(values)
    if data.get("gsettings"):
        gsettings.restore(data["gsettings"])
    store.DENOMINATIONS.update(data.get("denominations") or {})
    store.SECURITY.update(data.get("security") or {})
    store.FIELD_RIGHTS.update(data.get("field_rights") or {})
    if data.get("master_approval"):
        store.MASTER_APPROVAL.update(data["master_approval"])
    store.REPORT_RIGHTS.update(data.get("report_rights") or {})
    store.DOC_SETTINGS.update(data.get("doc_settings") or {})
    store.NUMBER_SERIES.update(data.get("number_series") or {})
    store.MENU_LAYOUT.update(data.get("menu_layout") or {})
    store.PRINT_PREFS.update(data.get("print_prefs") or {})
    store.IMPORT_FIELDS.update(data.get("import_fields") or {})
    # 2. Documents and number series (never going backwards).
    for kind, docs in data.get("docs", {}).items():
        store.DOCS.setdefault(kind, {}).update(docs)
    for kind, value in (data.get("seq") or {}).items():
        seq = store._seq.setdefault(kind, store.Seq(value))
        seq.value = max(seq.value, value)
    # 3. Stock movements made in the app, replayed onto QOH.
    store.mark_app_start()
    for m in data.get("movements", []):
        item = store.ITEMS.get(m["item"])
        if item is not None:
            item["qoh"] += m["qty"]
        if m["kind"] == "purchase" and m["qty"] > 0:
            store.LAST_PURCHASE[m["item"]] = m["date"]
        store.MOVEMENTS.append(m)


# ─── ADMIN Audit Log (UtlAdmAudit / UtlAdmAuditItem) ─────────────────

def _write_audit(cur, e: dict) -> None:
    at = datetime.fromisoformat(e["at"]) if e.get("at") else datetime.now()
    doc_id = _insert(cur, "UtlAdmAudit", {
        "ScreenName": e["screen"], "DAction": e["action"], "DocDate": at.date(), "DocDateTime": at, "DocSysName": "WEB",
        "SysUserName": e.get("user") or "", "MainFld": str(e.get("main_fld") or ""), "EarlierData": e.get("earlier") or "",
        "ChangedData": e.get("changed") or "",
    }, identity_out="DocID")
    for i in e.get("items", []):
        _insert(cur, "UtlAdmAuditItem", {"DocID": doc_id, "ItemCode": i.get("code") or "", "Itemname": i.get("name") or "",
                                              "ActionStatus": i.get("status") or "", "ItemDetails": i.get("details") or ""})


def add_audit(entries: list[dict]) -> None:
    """Write audit entries now (actions outside the write middleware). Never raises."""
    if not enabled():
        _memory_audit.extend(entries)
        return
    try:
        with db.transaction() as cur:
            for e in entries:
                _write_audit(cur, e)
    except Exception:
        logger.exception("Could not write the admin audit rows")


def audit_rows(screen: str | None, from_at: str, to_at: str, limit: int = 5000) -> list[dict]:
    """Audit entries in [from_at, to_at] (ISO strings), newest first, each with its item rows."""
    if not enabled():
        rows = [dict(e, id=n + 1, user_code=e.get("user")) for n, e in enumerate(_memory_audit)
                if from_at <= e["at"] <= to_at and (not screen or e["screen"] == screen)]
        return [{k: r.get(k) for k in ("id", "at", "screen", "action", "main_fld", "user_code", "earlier", "changed")}
                | {"items": r.get("items", [])} for r in reversed(rows)][:limit]
    sql = (f"SELECT TOP {int(limit)} DocID, DocDateTime, ScreenName, DAction, MainFld, SysUserName, EarlierData, ChangedData "
           "FROM dbo.UtlAdmAudit WHERE DocDateTime BETWEEN ? AND ?")
    args = [from_at.replace("T", " ")[:19], to_at.replace("T", " ")[:19]]
    if screen:
        sql += " AND ScreenName = ?"
        args.append(screen)
    heads = db.fetchall(sql + " ORDER BY DocID DESC", tuple(args))
    items: dict[int, list[dict]] = {}
    ids = [h["DocID"] for h in heads]
    for i in range(0, len(ids), 500):
        chunk = ids[i:i + 500]
        for r in db.fetchall(f"SELECT DocID, ItemCode, Itemname, ActionStatus, ItemDetails FROM dbo.UtlAdmAuditItem "
                             f"WHERE DocID IN ({','.join('?' * len(chunk))})", tuple(chunk)):
            items.setdefault(r["DocID"], []).append({"code": r["ItemCode"], "name": r["Itemname"], "status": r["ActionStatus"],
                                                     "details": r["ItemDetails"]})
    return [{"id": h["DocID"], "at": h["DocDateTime"].isoformat(timespec="seconds") if h["DocDateTime"] else "",
             "screen": h["ScreenName"], "action": h["DAction"], "main_fld": h["MainFld"], "user_code": h["SysUserName"],
             "earlier": h["EarlierData"], "changed": h["ChangedData"], "items": items.get(h["DocID"], [])} for h in heads]


def audit_screens() -> list[str]:
    if not enabled():
        return sorted({e["screen"] for e in _memory_audit})
    return [r["ScreenName"] for r in db.fetchall("SELECT DISTINCT ScreenName FROM dbo.UtlAdmAudit ORDER BY ScreenName")]


# ─── Tools audit (Web_ToolAudit) ─────────────────────────────────────────────

def audit_tool(user_code: str, tool: str, detail: str, ok: bool, row_count: int | None = None,
               message: str | None = None) -> None:
    """Record one Tools action. Never raises: a failed audit write is logged, not shown to the user."""
    row = {"at": _now(), "user_code": user_code, "tool": tool, "detail": detail[:2000], "ok": ok, "row_count": row_count,
           "message": (message or "")[:500] or None}
    if not enabled():
        _memory_tools.append(row | {"id": len(_memory_tools) + 1})
        return
    try:
        with db.transaction() as cur:
            cur.execute("INSERT INTO dbo.Web_ToolAudit (At, UserCode, Tool, Detail, Ok, RowCnt, Error) VALUES (?, ?, ?, ?, ?, ?, ?)",
                        (datetime.now(), user_code, tool, row["detail"], 1 if ok else 0, row_count, row["message"]))
    except Exception:
        logger.exception("Could not write the Tools audit row")


def tool_audit_rows(tool: str | None = None, limit: int = 50) -> list[dict]:
    """Latest Tools audit rows, newest first."""
    if not enabled():
        return [r for r in reversed(_memory_tools) if not tool or r["tool"] == tool][:limit]
    sql = f"SELECT TOP {int(limit)} Id, At, UserCode, Tool, Detail, Ok, RowCnt, Error FROM dbo.Web_ToolAudit"
    rows = db.fetchall(sql + (" WHERE Tool = ?" if tool else "") + " ORDER BY Id DESC", (tool,) if tool else ())
    return [{"id": r["Id"], "at": r["At"].isoformat(timespec="seconds"), "user_code": r["UserCode"], "tool": r["Tool"],
             "detail": r["Detail"], "ok": bool(r["Ok"]), "row_count": r["RowCnt"], "message": r["Error"]} for r in rows]


def reset_memory() -> None:
    """Tests: forget the in-memory audit logs."""
    _memory_audit.clear()
    _memory_tools.clear()
