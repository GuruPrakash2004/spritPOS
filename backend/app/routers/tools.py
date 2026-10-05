"""Tools menu of the desktop app (MstMenu type 6): SQL Query Execution and Re Write Stock.

SQL Query Execution (legacy frmUtlSQLQueryExec, shortcut 600), Administrator only, on the vLQMS SQL Server
database: SELECT anything; INSERT / UPDATE / DELETE only on the web app's own dbo.Web_* tables (not
Web_SchemaVersion, which the schema scripts own). The old desktop tables are never changed. Reads run through
db.read_only() (always rolled back), changes through db.write_statement() (committed). The web app keeps its
data in memory, loaded from the Web_ tables at startup: a change made here shows on the screens after a
restart, and a later edit on a screen can overwrite it. ("target" is kept in the API; "sqlserver" is the
only database now that the web app has no local SQLite file.)
Table changes (CREATE / ALTER / DROP) are never run here: the Web_* schema comes from backend/migrations.
One statement at a time. The object explorer, Select Top 1000, View Top 200 Rows, Script Table As and
view/procedure definitions mirror the desktop screen; object names are looked up before use, so user
text never becomes an identifier. Every run is recorded in Web_ToolAudit (app/sqlstore.py).

Re Write Stock (legacy frmStkRewrite, shortcut 604), module right "Re Write Stock": app/stock_rewrite.py.
"""
import datetime as dt
import decimal
import logging
import re
import time
import uuid
from dataclasses import dataclass
from typing import Any, Literal

from fastapi import APIRouter, Depends, HTTPException, Query
from pydantic import BaseModel, Field

from .. import db, sqlstore, stock_rewrite
from .auth import require_admin, require_module

logger = logging.getLogger("vlqms.tools")

router = APIRouter(prefix="/tools", tags=["Tools"])

SQL_TOOL = "SQL Query Execution"
REWRITE_TOOL = "Re Write Stock"
MAX_ROWS = 5000
Target = Literal["sqlserver"]

# Words never allowed outside the statement's own leading verb: they change schema, rights, sessions or the
# server, run code, reach other servers, or change data a second time. Matched as whole words after
# comments, strings and [bracketed] / "quoted" names are taken out.
_FORBIDDEN = (
    "INSERT", "UPDATE", "DELETE", "MERGE", "DROP", "ALTER", "CREATE", "TRUNCATE", "EXEC", "EXECUTE", "GRANT",
    "REVOKE", "DENY", "BACKUP", "RESTORE", "INTO", "OPENROWSET", "OPENQUERY", "OPENDATASOURCE", "OPENXML",
    "SHUTDOWN", "DBCC", "WAITFOR", "USE", "SET", "DECLARE", "KILL", "RECONFIGURE", "BULK", "COMMIT",
    "ROLLBACK", "BEGIN", "SAVE", "CHECKPOINT", "DISABLE", "ENABLE", "RAISERROR", "THROW", "NEXT", "OUTPUT",
    "ATTACH", "DETACH", "PRAGMA", "VACUUM", "REINDEX",
)
_FORBIDDEN_RE = re.compile(r"\b(" + "|".join(_FORBIDDEN) + r")\b", re.IGNORECASE)
_IDENT = r"(?:\w+|\x00\d+\x00)"                      # a plain name, or a [bracketed]/"quoted" one (placeholder)
_NAME = rf"(?:({_IDENT})\s*\.\s*)?({_IDENT})"         # [schema.]name
_FOUR_PART = re.compile(rf"{_IDENT}\s*\.\s*{_IDENT}\s*\.\s*{_IDENT}\s*\.\s*{_IDENT}")
_WRITE_HEADS = {
    "INSERT": re.compile(rf"^INSERT\s+(?:INTO\s+)?{_NAME}", re.IGNORECASE),
    "UPDATE": re.compile(rf"^UPDATE\s+{_NAME}\s+SET\b", re.IGNORECASE),
    "DELETE": re.compile(rf"^DELETE\s+(?:FROM\s+)?{_NAME}", re.IGNORECASE),
}


class SqlRefused(ValueError):
    """The SQL is not a statement this tool may run."""


@dataclass
class Statement:
    sql: str
    kind: Literal["read", "write"]
    table: str | None = None   # the table a write changes


def _tokenize(sql: str) -> tuple[str, list[str]]:
    """The SQL with comments removed (SQL Server nests /* */ comments, so nesting is counted), strings
    replaced by '' and [names] / "names" replaced by numbered placeholders. Returns (text, names).
    Raises SqlRefused for an unclosed comment, string or name."""
    out, names, i, n = [], [], 0, len(sql)
    while i < n:
        c, two = sql[i], sql[i:i + 2]
        if two == "--":
            j = sql.find("\n", i)
            i = n if j < 0 else j
            out.append(" ")
        elif two == "/*":
            depth, i = 1, i + 2
            while i < n and depth:
                if sql.startswith("/*", i):
                    depth, i = depth + 1, i + 2
                elif sql.startswith("*/", i):
                    depth, i = depth - 1, i + 2
                else:
                    i += 1
            if depth:
                raise SqlRefused("The query has an unclosed /* comment.")
            out.append(" ")
        elif c == "'":
            i += 1
            while True:
                j = sql.find("'", i)
                if j < 0:
                    raise SqlRefused("The query has an unclosed 'string'.")
                if sql.startswith("''", j):
                    i = j + 2
                    continue
                i = j + 1
                break
            out.append("''")
        elif c in "[\"":
            close = "]" if c == "[" else '"'
            i, name = i + 1, []
            while True:
                j = sql.find(close, i)
                if j < 0:
                    raise SqlRefused(f"The query has an unclosed {c}name{close}.")
                name.append(sql[i:j])
                if sql.startswith(close * 2, j):
                    name.append(close)
                    i = j + 2
                    continue
                i = j + 1
                break
            names.append("".join(name))
            out.append(f"\x00{len(names) - 1}\x00")
        else:
            out.append(c)
            i += 1
    return "".join(out), names


def _name(token: str | None, names: list[str]) -> str | None:
    if token is None:
        return None
    m = re.fullmatch(r"\x00(\d+)\x00", token)
    return names[int(m.group(1))] if m else token


def _top_level(text: str, word: str) -> bool:
    """True if `word` appears outside any brackets () in text (e.g. a FROM that is not in a sub-query)."""
    depth = 0
    for m in re.finditer(rf"[()]|\b{word}\b", text, re.IGNORECASE):
        if m.group(0) == "(":
            depth += 1
        elif m.group(0) == ")":
            depth -= 1
        elif depth == 0:
            return True
    return False


# The web app's own records and the audit trail: never changed by hand (roles and password hashes are in
# Web_AppRow; the logs must not be edited; the setup markers guard the old data's copy).
PROTECTED = {"web_schemaversion", "web_approw", "web_appdoc", "web_appmove", "web_appsetting", "web_setup",
             "web_toolaudit", "web_utladmaudit", "web_utladmaudititem"}


def _writable(target: Target, schema: str | None, table: str) -> bool:
    if table.lower() in PROTECTED:
        return False
    return (schema or "dbo").lower() == "dbo" and table.lower().startswith("web_") and table.lower() != "web_schemaversion"


def _check_sql(sql: str, target: Target = "sqlserver") -> Statement:
    """Classify one statement and refuse anything this tool may not run (message for the user)."""
    text = (sql or "").strip()
    if not text:
        raise SqlRefused("Type a query to execute.")
    if len(text) > 100_000:
        raise SqlRefused("The query is too long (100,000 characters at most).")
    bare, names = _tokenize(text)
    statements = [s for s in (part.strip() for part in bare.split(";")) if s]
    if len(statements) != 1:
        raise SqlRefused("Run one statement at a time.")
    body = statements[0]
    if _FOUR_PART.search(body):
        raise SqlRefused("Queries on other servers (four-part names) are not allowed.")
    first = body.lstrip("( \t\r\n").split(None, 1)[0].upper()
    where = "the web app's own tables"
    if first in ("SELECT", "WITH"):
        bad = _FORBIDDEN_RE.search(body)
        if bad:
            raise SqlRefused(f"'{bad.group(1).upper()}' is not allowed in a query. "
                             f"(A column with that name can be written in brackets, e.g. [{bad.group(1)}].)")
        return Statement(text, "read")
    if first in _WRITE_HEADS:
        head = _WRITE_HEADS[first].match(body)
        if not head:
            raise SqlRefused(f"Write the {first} as {first} {'INTO ' if first == 'INSERT' else 'FROM ' if first == 'DELETE' else ''}"
                             f"<table> ... so the table it changes is clear.")
        schema, table = _name(head.group(1), names), _name(head.group(2), names)
        if not _writable(target, schema, table):
            raise SqlRefused(f"{first} is allowed only on {where}; {table} can't be changed here. "
                             "The desktop app's tables are never changed by the web app.")
        rest = body[head.end():]
        if first in ("UPDATE", "DELETE") and _top_level(rest, "FROM"):
            raise SqlRefused(f"{first} with a joined FROM is not supported here: use WHERE ... IN (SELECT ...) instead.")
        bad = _FORBIDDEN_RE.search(rest)
        if bad:
            raise SqlRefused(f"'{bad.group(1).upper()}' is not allowed in this {first}.")
        return Statement(text, "write", f"{schema}.{table}" if schema else table)
    if first in ("CREATE", "ALTER", "DROP", "TRUNCATE"):
        raise SqlRefused("Table changes are not run from here: the web tables' structure comes from "
                         "backend/migrations (tools/migrate.py).")
    raise SqlRefused(f"Only SELECT, and INSERT / UPDATE / DELETE on {where}, can be run here.")


def _cell(v: Any) -> Any:
    """A result value as JSON: decimals as numbers, dates as ISO text, binary as 0x… hex."""
    if isinstance(v, decimal.Decimal):
        return float(v)
    if isinstance(v, (dt.datetime, dt.date, dt.time)):
        return v.isoformat()
    if isinstance(v, (bytes, bytearray, memoryview)):
        b = bytes(v)
        return "0x" + b[:64].hex().upper() + ("…" if len(b) > 64 else "")
    if isinstance(v, uuid.UUID):
        return str(v)
    return v


def _need(target: Target) -> None:
    if not db.is_db_available():
        raise HTTPException(409, "SQL Server is not connected: set up the database from the sign-in page first.")


def _read(target: Target, sql: str, params: tuple = (), max_rows: int = MAX_ROWS) -> dict:
    started = time.perf_counter()
    result = db.read_only(sql, params, max_rows=max_rows)
    result["rows"] = [[_cell(v) for v in row] for row in result["rows"]]
    result["row_count"] = len(result["rows"])
    result["elapsed_ms"] = round((time.perf_counter() - started) * 1000)
    return result


def _error(exc: Exception, target: Target) -> HTTPException:
    return HTTPException(400, f"SQL Server: {db.safe_error(exc)}")


# ─── SQL Query Execution ─────────────────────────────────────────────────────

class SqlIn(BaseModel):
    sql: str = Field(..., max_length=100_000)
    target: Target = "sqlserver"


@router.get("/sql/status")
def sql_status(admin: dict = Depends(require_admin)):
    """Which databases can be queried (never the server's address)."""
    name = None
    if db.is_db_available():
        try:
            name = _read("sqlserver", "SELECT DB_NAME() AS db")["rows"][0][0]
        except Exception:
            name = None
    return {"db_connected": db.is_db_available(), "database": name,
            "driver": db.driver_name() if db.is_db_available() else None}


@router.get("/sql/objects")
def sql_objects(target: Target = "sqlserver", search: str = Query("", max_length=128), admin: dict = Depends(require_admin)):
    """Object explorer: tables, views (and procedures on SQL Server), optionally filtered by name."""
    _need(target)
    term = search.strip().lower()
    sql = ("SELECT CASE o.type WHEN 'U' THEN 'Tables' WHEN 'V' THEN 'Views' ELSE 'Procedures' END AS kind, "
           "s.name AS schema_name, o.name AS name FROM sys.objects o JOIN sys.schemas s ON s.schema_id = o.schema_id "
           "WHERE o.type IN ('U', 'V', 'P') AND o.is_ms_shipped = 0")
    params: tuple = ()
    if term:
        sql += " AND o.name LIKE ?"
        params = ("%" + term.replace("[", "[[]").replace("%", "[%]").replace("_", "[_]") + "%",)
    try:
        rows = _read("sqlserver", sql + " ORDER BY kind, o.name", params, max_rows=20_000)["rows"]
    except Exception as exc:
        raise _error(exc, target)
    return [{"kind": k, "schema": s, "name": n} for k, s, n in rows]


def _q(part: str, target: Target) -> str:
    return "[" + part.replace("]", "]]") + "]"


def _object(target: Target, name: str, schema: str | None) -> dict:
    """The table / view / procedure with this name (404 if none): exact schema, name, type and quoted name."""
    _need(target)
    sql = ("SELECT TOP 1 s.name, o.name, o.type FROM sys.objects o JOIN sys.schemas s ON s.schema_id = o.schema_id "
           "WHERE o.type IN ('U', 'V', 'P') AND o.name = ?")
    params: tuple = (name,)
    if schema:
        sql += " AND s.name = ?"
        params += (schema,)
    try:
        rows = _read(target, sql + " ORDER BY CASE WHEN s.name = 'dbo' THEN 0 ELSE 1 END", params)["rows"]
    except Exception as exc:
        raise _error(exc, target)
    if not rows:
        raise HTTPException(404, f"No table, view or procedure named '{name}'")
    s, n, t = rows[0]
    return {"schema": s, "name": n, "type": t.strip(), "full": f"{_q(s, target)}.{_q(n, target)}"}


def _columns(target: Target, obj: dict) -> list[dict]:
    rows = _read(target,
        "SELECT c.COLUMN_NAME, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION, c.NUMERIC_SCALE, "
        "c.IS_NULLABLE, c.COLUMN_DEFAULT, COLUMNPROPERTY(OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') "
        "FROM INFORMATION_SCHEMA.COLUMNS c WHERE c.TABLE_SCHEMA = ? AND c.TABLE_NAME = ? ORDER BY c.ORDINAL_POSITION",
        (obj["schema"], obj["name"]))["rows"]
    return [{"name": r[0], "type": r[1], "max_length": r[2], "precision": r[3], "scale": r[4],
             "nullable": r[5] == "YES", "default": r[6], "identity": bool(r[7])} for r in rows]


def _public(obj: dict) -> dict:
    return {k: obj[k] for k in ("schema", "name", "type")}


@router.get("/sql/columns")
def sql_columns(name: str = Query(..., max_length=128), schema: str | None = Query(None, max_length=128),
                target: Target = "sqlserver", admin: dict = Depends(require_admin)):
    obj = _object(target, name, schema)
    try:
        return {"object": _public(obj), "columns": _columns(target, obj)}
    except Exception as exc:
        raise _error(exc, target)


def _type_text(c: dict) -> str:
    t = c["type"]
    if t in ("varchar", "nvarchar", "char", "nchar", "varbinary", "binary"):
        return f"[{t}]({'max' if c['max_length'] == -1 else c['max_length']})"
    if t in ("decimal", "numeric"):
        return f"[{t}]({c['precision']}, {c['scale']})"
    return f"[{t}]"


def _definition(target: Target, obj: dict) -> str:
    rows = _read(target, "SELECT OBJECT_DEFINITION(OBJECT_ID(?))", (f"{obj['schema']}.{obj['name']}",))["rows"]
    text = rows[0][0] if rows else None
    return text or "-- The definition of this object is encrypted or not available."


@router.get("/sql/script")
def sql_script(name: str = Query(..., max_length=128), kind: Literal["create", "drop", "select"] = "create",
               schema: str | None = Query(None, max_length=128), target: Target = "sqlserver",
               admin: dict = Depends(require_admin)):
    """Script Table As → CREATE / DROP, or the Select Top 1000 query. Scripts are text for the editor:
    running CREATE or DROP here is refused."""
    obj = _object(target, name, schema)
    try:
        cols = _columns(target, obj) if obj["type"] in ("U", "V") else []
        if kind == "select":
            if not cols:
                raise HTTPException(422, "Only tables and views can be selected from")
            col_list = ",\n       ".join(_q(c["name"], target) for c in cols)
            return {"script": f"SELECT TOP 1000 {col_list}\nFROM {obj['full']}"}
        if kind == "drop":
            word = {"U": "TABLE", "V": "VIEW", "P": "PROCEDURE"}[obj["type"]]
            return {"script": f"DROP {word} {obj['full']}"}
        if obj["type"] != "U":
            return {"script": _definition(target, obj)}
        pk = [r[0] for r in _read(target,
            "SELECT k.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS t JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE k "
            "ON k.CONSTRAINT_NAME = t.CONSTRAINT_NAME AND k.TABLE_SCHEMA = t.TABLE_SCHEMA AND k.TABLE_NAME = t.TABLE_NAME "
            "WHERE t.CONSTRAINT_TYPE = 'PRIMARY KEY' AND t.TABLE_SCHEMA = ? AND t.TABLE_NAME = ? ORDER BY k.ORDINAL_POSITION",
            (obj["schema"], obj["name"]))["rows"]]
    except HTTPException:
        raise
    except Exception as exc:
        raise _error(exc, target)
    lines = []
    for c in cols:
        line = f"\t[{c['name']}] {_type_text(c)}"
        if c["identity"]:
            line += " IDENTITY(1,1)"
        line += " NULL" if c["nullable"] else " NOT NULL"
        if c["default"] is not None:
            line += f" DEFAULT {c['default']}"
        lines.append(line)
    if pk:
        lines.append(f"\tCONSTRAINT [PK_{obj['name']}] PRIMARY KEY ({', '.join('[' + p + ']' for p in pk)})")
    return {"script": f"CREATE TABLE {obj['full']} (\n" + ",\n".join(lines) + "\n)"}


@router.get("/sql/definition")
def sql_definition(name: str = Query(..., max_length=128), schema: str | None = Query(None, max_length=128),
                   target: Target = "sqlserver", admin: dict = Depends(require_admin)):
    """A view's or procedure's source (the desktop's sp_helptext)."""
    obj = _object(target, name, schema)
    if obj["type"] == "U":
        raise HTTPException(422, f"{obj['name']} is a table: use Script Table As → Create")
    try:
        return {"object": _public(obj), "definition": _definition(target, obj)}
    except Exception as exc:
        raise _error(exc, target)


@router.get("/sql/top")
def sql_top(name: str = Query(..., max_length=128), n: int = Query(200, ge=1, le=1000),
            schema: str | None = Query(None, max_length=128), target: Target = "sqlserver",
            admin: dict = Depends(require_admin)):
    """View Top N Rows of a table or view (the desktop's Edit Top 200 Rows; change rows with an UPDATE)."""
    obj = _object(target, name, schema)
    if obj["type"] == "P":
        raise HTTPException(422, f"{obj['name']} is a procedure: only tables and views have rows")
    try:
        result = _read(target, f"SELECT TOP {int(n)} * FROM {obj['full']}")
        total = _read(target, f"SELECT COUNT_BIG(*) FROM {obj['full']}")["rows"][0][0]
    except Exception as exc:
        raise _error(exc, target)
    writable = _writable(target, obj["schema"], obj["name"]) and obj["type"] == "U"
    return {"object": _public(obj), **result, "total_rows": total, "writable": writable}


@router.post("/sql/execute")
def sql_execute(body: SqlIn, admin: dict = Depends(require_admin)):
    """Execute Query (F5): one statement. SELECT returns rows (at most 5,000); INSERT / UPDATE / DELETE on
    the web app's own tables is committed and returns the rows affected. Every run is audited."""
    where = ""
    try:
        stmt = _check_sql(body.sql, body.target)
    except SqlRefused as exc:
        sqlstore.audit_tool(admin["user"], SQL_TOOL, where + body.sql, False, message=str(exc))
        raise HTTPException(400, str(exc))
    _need(body.target)
    if stmt.kind != "read" and sqlstore.test_data():
        sqlstore.audit_tool(admin["user"], SQL_TOOL, where + body.sql, False, message="refused: Test data is on")
        raise HTTPException(409, "Test data is on: changes to the database are refused until it is switched off.")
    started = time.perf_counter()
    try:
        if stmt.kind == "read":
            result = _read(body.target, stmt.sql) | {"kind": "read"}
            count = result["row_count"]
        else:
            count = db.write_statement(stmt.sql)
            result = {"kind": "write", "table": stmt.table, "rows_affected": count, "columns": [], "rows": [],
                      "row_count": 0, "truncated": False,
                      "elapsed_ms": round((time.perf_counter() - started) * 1000)}
    except Exception as exc:
        error = _error(exc, body.target)
        sqlstore.audit_tool(admin["user"], SQL_TOOL, where + stmt.sql, False, message=error.detail)
        raise error
    sqlstore.audit_tool(admin["user"], SQL_TOOL, where + stmt.sql, True, row_count=count)
    logger.info("SQL Query Execution by %s on %s: %s, %d row(s)", admin["user"], body.target, stmt.kind, count)
    return result


@router.get("/audit")
def tools_audit(tool: Literal["SQL Query Execution", "Re Write Stock"] | None = None,
                limit: int = Query(50, ge=1, le=500), admin: dict = Depends(require_admin)):
    """Who ran which Tools action, newest first (Administrator only)."""
    return sqlstore.tool_audit_rows(tool, limit)


# ─── Re Write Stock ──────────────────────────────────────────────────────────

REWRITE = Depends(require_module(REWRITE_TOOL))


@router.get("/rewrite-stock")
def rewrite_stock_check(user: dict = REWRITE):
    """Differences between documents, stock movements and QOH (web data), plus the desktop database's
    own checks when SQL Server is connected (read-only)."""
    return {"web": stock_rewrite.check_stock(), "desktop": stock_rewrite.legacy_check(),
            "db_connected": db.is_db_available()}


@router.post("/rewrite-stock")
def rewrite_stock(user: dict = REWRITE):
    """Re-Write: rebuild the app's stock movements from its documents and QOH from them. Runs as a write
    (main.py serialises it), so no bill is saved half-way through. 409 if an item would go negative."""
    try:
        result = stock_rewrite.rebuild_movements()
    except stock_rewrite.RewriteRefused as exc:
        sqlstore.audit_tool(user["user"], REWRITE_TOOL, "Re-Write", False, message=str(exc))
        raise HTTPException(409, str(exc))
    sqlstore.audit_tool(user["user"], REWRITE_TOOL, "Re-Write", True, row_count=result.get("items_changed", 0),
                           message=result["message"])
    return result
