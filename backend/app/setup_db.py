"""One-time database setup: connect the web app straight to the shop's own vLQMS database (the one the desktop
app uses). Nothing is copied: the app reads and writes the desktop's tables themselves.

- check(): what the connected database is (edition, whether it is a vLQMS database, whether this login may
  create tables, whether the stock trigger the desktop relies on is there);
- create_schema(): creates the web app's own tables, `Web_AppSetting`, `Web_AppRow`, `Web_AppDoc`, `Web_AppMove`
  and `Web_ToolAudit` (migrations/web_app_tables.sql: they hold what the desktop tables have no column for:
  password hashes, roles and rights, whole documents, the web's stock ledger, the Tools audit). Every CREATE
  is guarded, so running it again changes nothing. No desktop table is created, altered or emptied.

Runs from the sign-in page's Database setup (routers/setup.py) or tools/setup_db.py.
"""
import logging
import re
from pathlib import Path

from . import db

logger = logging.getLogger("vlqms.setup")

SCHEMA = Path(__file__).resolve().parents[1] / "migrations" / "web_app_tables.sql"
WEB_TABLES = ("Web_AppSetting", "Web_AppRow", "Web_AppDoc", "Web_AppMove", "Web_ToolAudit")
# Desktop tables the web app reads and writes: a database without them is not a vLQMS database.
REQUIRED_TABLES = ("MstITEM", "MstCUST", "MstSUPP", "MstUser", "MstROLE", "MstComp", "MstCompFinYear", "TrnHdrSINV",
                   "TrnDtlSINV", "TrnHdrPB", "TrnDtlPB", "UtlStockList", "TrnQOH")
EXPRESS_LIMIT_MB = 10 * 1024            # SQL Server Express: 10 GB per database


class SetupError(Exception):
    """A setup step that can't go on, with a message for the user."""


def batches(sql: str) -> list[str]:
    """Split a script on lines that hold only GO (as SSMS/sqlcmd do)."""
    parts, current = [], []
    for line in sql.splitlines():
        if line.strip().upper() == "GO":
            parts.append("\n".join(current))
            current = []
        else:
            current.append(line)
    parts.append("\n".join(current))
    return [p for p in parts if re.sub(r"--[^\n]*", "", p).strip()]


def _rows(sql: str, params: tuple = ()) -> list[dict]:
    return db.fetchall(sql, params)


def _present(names: tuple[str, ...]) -> set[str]:
    have = {r["name"].lower() for r in _rows("SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo')")}
    return {n for n in names if n.lower() in have}


def is_ready() -> bool:
    """True when the connected database is a vLQMS database and the web app's own tables exist in it."""
    try:
        have = _present(REQUIRED_TABLES + WEB_TABLES)
        return len(have) == len(REQUIRED_TABLES) + len(WEB_TABLES)
    except Exception:   # noqa: BLE001 - the connection dropped
        return False


def check() -> dict:
    """What the connected database is, for the setup screen."""
    missing = [t for t in REQUIRED_TABLES if t not in _present(REQUIRED_TABLES)]
    edition = _rows("SELECT CAST(SERVERPROPERTY('Edition') AS NVARCHAR(128)) AS edition, "
                    "CAST(SERVERPROPERTY('EngineEdition') AS INT) AS engine, "
                    "CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(128)) AS version, DB_NAME() AS db")[0]
    perm = _rows("SELECT HAS_PERMS_BY_NAME(DB_NAME(), 'DATABASE', 'CREATE TABLE') AS p")[0]["p"]
    triggers = _rows("SELECT COUNT(*) AS n FROM sys.triggers t JOIN sys.sql_modules m ON m.object_id = t.object_id "
                     "WHERE t.parent_id = OBJECT_ID('dbo.UtlStockList') AND m.definition LIKE '%TrnQOH%'")[0]["n"]
    return {
        "database": edition["db"], "edition": edition["edition"], "version": edition["version"],
        "is_vlqms": not missing, "missing_tables": missing,
        "can_create_tables": bool(perm), "stock_trigger": bool(triggers),
        "web_tables": len(_present(WEB_TABLES)), "ready": is_ready(),
    }


def create_schema() -> None:
    """Create the web app's own tables (never touches a desktop table)."""
    for sql in batches(SCHEMA.read_text()):
        with db.transaction() as cur:
            cur.execute_raw(sql)


def prepare() -> dict:
    """Check the connected database and create the web app's tables in it. Raises SetupError (message for the
    user). Returns check()'s answer."""
    info = check()
    if not info["is_vlqms"]:
        raise SetupError("This isn't a vLQMS database: these tables are missing: " + ", ".join(info["missing_tables"]) + ".")
    if not info["stock_trigger"]:
        raise SetupError("UtlStockList has no trigger keeping the stock balance (TrnQOH), so stock would not update. "
                         "Use the database the desktop vLQMS works on.")
    if info["web_tables"] < len(WEB_TABLES):
        if not info["can_create_tables"]:
            raise SetupError("This SQL login can't create the web app's tables in the database. Use a login with "
                             "db_owner (or CREATE TABLE) rights for this one-time setup.")
        create_schema()
        logger.info("Created the web app's own tables in the database")
        info = check()
    return info
