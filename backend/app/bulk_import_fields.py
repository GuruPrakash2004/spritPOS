"""Bulk Item Import Fields (legacy frmUtlBulkImpFlds, Global Settings → Other Settings → Bulk Item Import Fields, window "UtlItemBulkUpdate").

Add Fields To Import in item Master: a two column grid, Field Name From Item Master (a drop-down of the MstItem columns) and Data Type (Text, Integer,
Date, Numeric). It lists UtlItemBulkUpdate (FieldName, DataType; FieldName is the primary key) and Save replaces the whole table with the grid's rows
(Delete from UtlItemBulkUpdate, then one INSERT per row with a field name; "Saved successfully."). The table is what Options → Update Item Master Fields
(frmUtlImportAnyFld, here components/utilities/ImportItemMasterFields.jsx) offers as its fields, so a field added here can be changed in bulk there.
The exe creates the table (with ten fields) at its own start-up; the web never creates anything, so without the table the screen says so.
Differences from the exe: a duplicate field and a row with no data type are refused with a message (the exe lets the database's key error through, or saves
a blank type), and ItemID is not offered (it is the key a bulk update matches on).
"""
import re

from fastapi import HTTPException

from . import admin_audit, db, sqlstore

SCREEN = "Bulk Item Import Fields"
TYPES = ("Text", "Integer", "Date", "Numeric")
_NAME = re.compile(r"^[A-Za-z_][A-Za-z0-9_]*$")


def _need_db() -> None:
    if not sqlstore.db_usable():
        raise HTTPException(503, f"Bulk Item Import Fields needs the shop's database. {sqlstore.db_problem()}")


def _s(v) -> str:
    return "" if v is None else str(v).strip()


def item_columns() -> list[str]:
    """The drop-down: `SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MstItem'` (ItemID left out)."""
    rows = db.fetchall("SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MstItem' ORDER BY ORDINAL_POSITION")
    return [_s(r["column_name"]) for r in rows if _s(r["column_name"]).upper() != "ITEMID"]


def rows() -> list[dict]:
    """loadFlds: `select FieldName, DataType from UtlItemBulkupdate` (in the table's own order)."""
    try:
        data = db.fetchall("select FieldName, DataType from UtlItemBulkupdate")
    except Exception:
        raise HTTPException(503, "The table UtlItemBulkUpdate is not in this database. Start the desktop vLQMS once: it creates it.") from None
    return [{"name": _s(r["FieldName"]), "datatype": _s(r["DataType"])} for r in data]


def get() -> dict:
    _need_db()
    return {"fields": rows(), "columns": item_columns(), "types": list(TYPES)}


def check(fields: list[dict], columns: list[str]) -> list[dict]:
    """The rows to save: blank rows dropped, every field a real MstItem column, once, with a data type."""
    known = {c.upper(): c for c in columns}
    out, seen = [], set()
    for f in fields:
        name = _s(f.get("name"))
        if not name:
            continue
        real = known.get(name.upper())
        if not real or not _NAME.match(real):
            raise HTTPException(422, f"'{name}' is not a field of the Item Master")
        if real.upper() in seen:
            raise HTTPException(422, f"Duplicate field '{real}'")
        kind = next((t for t in TYPES if t.upper() == _s(f.get("datatype")).upper()), "")
        if not kind:
            raise HTTPException(422, f"Select the Data Type of {real}")
        seen.add(real.upper())
        out.append({"name": real, "datatype": kind})
    return out


def save(fields: list[dict], user: dict) -> dict:
    """btnSave_Click: Delete from UtlItemBulkUpdate, then one insert per row, in one transaction."""
    _need_db()
    before = rows()
    clean = check(fields, item_columns())
    with db.direct_writes(), db.transaction() as cur:
        cur.execute("Delete from UtlItemBulkUpdate")
        for f in clean:
            cur.execute("Insert into UtlItemBulkUpdate(FieldName,DataType) values (?,?)", (f["name"], f["datatype"]))
    show = lambda rs: ", ".join(f"{r['name']} ({r['datatype']})" for r in rs) or "none"  # noqa: E731
    if show(before) != show(clean):
        admin_audit.write_now(user["user"], "Global Settings", SCREEN, "Item Master", show(before), show(clean))
    return {"message": "Saved successfully.", "fields": clean}
