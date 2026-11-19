"""Delete Linkage Updation (legacy FrmUtlDeleteLinkageUpdation): the rules that stop a master record being deleted while another table uses it.

The rules live in the desktop's own table UtlDeletion (the web creates nothing):
  DelTblName   the master table whose deletion is guarded (MstItem, MstCUST ...)
  DelDocName   a friendly name for it (the same on every row of the group)
  DelDocFld    the column of that master checked (the same on every row of the group), e.g. ItemID
  TblName / FldName   another table and the column in it that may hold the master's value
  DispFldName  "Display Column", used to word the refusal;  TblCaption  "Doc Name", the other table's friendly name
One group of rows per DelTblName. A delete is refused ("Cannot Delete!!! ...", sqlstore._check_unused, the exe's svtModule.blnDeleteOK)
when a row's table holds the record's value in its column.

Like the exe, Add and Edit are the same save: the group's rows are deleted and written again in one transaction; values are always parameters.
The delete check (sqlstore._check_unused) puts a rule's table and column into SQL as identifiers, so it skips a rule whose names hold anything but
letters, digits, "_" and spaces. Administrator only (routers/utilities.py). The exe's table list is Mst / Trn / Utl / Rpt tables only: the same here.
"""
import re

from fastapi import HTTPException

from . import admin_audit, db, sqlstore

SCREEN = "Delete Linkage Updation"
_IDENT = re.compile(r"^[A-Za-z0-9_ ]{1,128}$")   # a table name asked for the column list


def _need_db() -> None:
    if not sqlstore.db_usable():
        raise HTTPException(503, f"Delete Linkage Updation needs the shop's database. {sqlstore.db_problem()}")


def _s(v) -> str:
    return "" if v is None else str(v).strip()


def tables() -> list[str]:
    """frmUtlDeleteLinkageUpdation.FillPreview: the tables the screen offers."""
    _need_db()
    rows = db.fetchall("Select Object_Id,Name From sys.tables where LEFT(name,3) in ('Mst','Trn','Utl','Rpt') order by Name")
    return [_s(r["Name"]) for r in rows]


def columns(table: str) -> list[str]:
    """The columns of a table (the grid's column drop-downs)."""
    _need_db()
    table = _s(table)
    if not _IDENT.match(table):
        raise HTTPException(422, "Select Table name")
    rows = db.fetchall("Select COLUMN_NAME From Information_Schema.Columns Where TABLE_NAME = ? Order By ORDINAL_POSITION", (table,))
    return [_s(r["COLUMN_NAME"]) for r in rows]


def groups() -> list[dict]:
    """The right-hand list: one row per guarded table, named by its document name (or the table name when it has none)."""
    _need_db()
    rows = db.fetchall("select DelTblName,case DelDocName When '' then DelTblName else DelDocName end DocName from utldeletion Group by DelTblName, DelDocName")
    return [{"table": _s(r["DelTblName"]), "name": _s(r["DocName"]) or _s(r["DelTblName"])} for r in rows]


def group(table: str) -> dict:
    """LoadDelTable: the rows of one guarded table."""
    _need_db()
    table = _s(table)
    rows = db.fetchall("select * from UtlDeletion where DelTblName = ? order by DelTblName", (table,))
    if not rows:
        raise HTTPException(404, "Data Not found !!! ")
    return {
        "table": _s(rows[0]["DelTblName"]), "doc_name": _s(rows[0].get("DelDocName")), "doc_field": _s(rows[0].get("DelDocFld")),
        "rows": [{"table": _s(r.get("TblName")), "column": _s(r.get("FldName")), "display": _s(r.get("DispFldName")), "caption": _s(r.get("TblCaption"))} for r in rows],
    }


def search(table: str, field: str) -> list[dict]:
    """The magnifier: every other table (not views) that has a column named like the checked field, with its Doc Name and Display Column."""
    _need_db()
    table, field = _s(table), _s(field)
    if not table:
        raise HTTPException(422, "Select Table name")
    own = {c.lower() for c in columns(table)}
    if not field or field.lower() not in own:   # the exe blanks a field that is not a column of the selected table
        raise HTTPException(422, "Select Field to be Checked for Deletion")
    hits = db.fetchall("Select TABLE_NAME,COLUMN_NAME From Information_Schema.Columns Where COLUMN_NAME= ? AND TABLE_NAME <> ? And Table_Name Not Like '%vw%' Order By TABLE_NAME",
                       (field, table))
    # getDocNames: the Display Column comes from the selected table's document ("<DocShForm>Code"), else the checked field itself
    disp = db.fetchone("select DocShForm + 'Code' as D from utlTransactions where HdrTblName = ? or DtlTblName = ?", (table, table))
    display = _s((disp or {}).get("D")) or field
    out = []
    for h in hits:
        t = _s(h["TABLE_NAME"])
        doc = db.fetchone("select DocName from utlTransactions where HdrTblName = ? or DtlTblName = ?", (t, t))
        out.append({"table": t, "column": _s(h["COLUMN_NAME"]), "display": display, "caption": _s((doc or {}).get("DocName"))})
    return out


def save(table: str, doc_name: str, doc_field: str, rows: list[dict], user: dict) -> dict:
    """btnSav_Click: the checks in the exe's order and with its messages, then the group is rewritten in one transaction."""
    _need_db()
    table, doc_name, doc_field = _s(table), _s(doc_name), _s(doc_field)
    if not table:
        raise HTTPException(422, "Table name is Empty...")
    if not doc_name:
        raise HTTPException(422, "Document name is Empty...")
    if not rows:
        raise HTTPException(422, "Data is not available")
    if not doc_field:
        raise HTTPException(422, "Column is Empty....")
    # Rows with an empty Table or Column are dropped (the exe removes them before inserting). If none is left the group is simply deleted and
    # nothing is written, as in the exe. Values are parameters; the names are made safe where they are used as identifiers (sqlstore._check_unused).
    kept = [{"table": _s(r.get("table")), "column": _s(r.get("column")), "display": _s(r.get("display")), "caption": _s(r.get("caption"))}
            for r in rows if _s(r.get("table")) and _s(r.get("column"))]
    with db.direct_writes(), db.transaction() as cur:
        cur.execute("Delete from utlDeletion where DelTblName = ?", (table,))
        cur.executemany("Insert into utlDeletion (DelTblName, TblName, FldName, DispFldName, TblCaption, DelDocName, DelDocFld) values (?,?,?,?,?,?,?)",
                        [(table, r["table"], r["column"], r["display"], r["caption"], doc_name, doc_field) for r in kept])
    admin_audit.write_now(user["user"], SCREEN, "Saved", table, "", f"{len(kept)} table(s) checked before deleting")
    return {"saved": len(kept), "message": "Saved Successfully!"}


def delete(table: str, user: dict) -> dict:
    """btnDlt_Click: the whole group of rules for the table is removed."""
    _need_db()
    table = _s(table)
    if not table:
        raise HTTPException(422, "Table name is Empty...")
    with db.direct_writes(), db.transaction() as cur:
        cur.execute("Delete from utlDeletion where DelTblName = ?", (table,))
    admin_audit.write_now(user["user"], SCREEN, "Deleted", table)
    return {"message": "Deleted Successfully!"}
