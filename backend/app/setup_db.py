"""One-time database setup: create the Web_ tables and copy the legacy vLQMS data into them.

The web app works only on Web_ tables (a Web_<Name> copy of every legacy table, same columns, from
migrations/002_web_legacy_tables.sql). At handover the client's old data is copied into them once:

- check(): what the connected database has (legacy tables and rows, Web_ tables, size, edition limit);
- create_schema(): runs 002 (every CREATE is guarded, so running it again changes nothing);
- copy_all(): for each legacy table, empties its Web_ copy and copies the rows (columns both tables have;
  identity values kept; the big tables in key ranges so the log and locks stay small), then checks the counts.
  Web_TrnQOH is copied as it is, so the Web_UtlStockList triggers are switched off during the copy.
  Refused once the web app has saved anything (a second copy would overwrite it), unless force=True, which
  also clears what the web app saved (Web_AppRow / Web_AppDoc / Web_AppMove / Web_AppSetting).

The legacy tables are only read here. Progress is kept in PROGRESS for the setup screen, and each table's
result in Web_Setup. Runs from the sign-in page's Database setup (routers/setup.py) or tools/setup_db.py.
"""
import json
import logging
import re
import threading
import time
from datetime import datetime
from pathlib import Path

from . import db

logger = logging.getLogger("vlqms.setup")

SCHEMA = Path(__file__).resolve().parents[1] / "migrations" / "002_web_legacy_tables.sql"
SCHEMA_VERSION = 2
BATCH_ROWS = 50_000
WEB_ONLY = {"SchemaVersion", "AppSetting", "AppRow", "AppDoc", "AppMove", "ToolAudit", "Setup"}
EXPRESS_LIMIT_MB = 10 * 1024            # SQL Server Express: 10 GB per database
FIRST_WEB_WRITE = ("setup", "first_web_write")   # Web_AppSetting key set by the first web save
COMPLETED = ("setup", "completed")                # Web_AppSetting key set when a copy finished and checked out

STALL_SECONDS = 120                     # no progress for this long: the connection is taken as dead
TABLE_ATTEMPTS = 3

PROGRESS: dict = {"state": "idle"}      # the setup screen polls this
RUNNING = ("starting", "checking", "creating tables", "copying")
_run_lock = threading.Lock()


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


def schema_tables() -> list[str]:
    """The legacy tables that have a Web_ copy in 002 (names without the Web_ prefix)."""
    names = re.findall(r"^CREATE TABLE dbo\.\[?Web_(\w+)\]? \(", SCHEMA.read_text(), re.M)
    return [n for n in names if n not in WEB_ONLY]


def _rows(sql: str, params: tuple = ()) -> list[dict]:
    return db.fetchall(sql, params)


def schema_version() -> int:
    """The highest applied Web_ schema version, 0 when there is none."""
    if not _rows("SELECT 1 AS x FROM sys.tables WHERE name = 'Web_SchemaVersion'"):
        return 0
    r = _rows("SELECT MAX(Version) AS v FROM dbo.Web_SchemaVersion")
    return int(r[0]["v"] or 0) if r else 0


def web_has_saved() -> bool:
    """True once the web app has saved something into the Web_ tables (see sqlstore)."""
    if not _rows("SELECT 1 AS x FROM sys.tables WHERE name = 'Web_AppSetting'"):
        return False
    return bool(_rows("SELECT 1 AS x FROM dbo.Web_AppSetting WHERE Section = ? AND [Key] = ?", FIRST_WEB_WRITE))


def is_ready() -> bool:
    """True when the Web_ tables exist and a copy has finished (the app can work on them)."""
    try:
        if schema_version() < SCHEMA_VERSION:
            return False
        return bool(_rows("SELECT 1 AS x FROM dbo.Web_AppSetting WHERE Section = ? AND [Key] = ?", COMPLETED))
    except Exception:   # noqa: BLE001 - no tables yet, or the connection dropped
        return False


def _mark_completed(run_id: int, tables: int, rows: int) -> None:
    value = json.dumps({"run_id": run_id, "tables": tables, "rows": rows, "at": datetime.now().isoformat(timespec="seconds")})
    with db.transaction() as cur:
        if not cur.execute("UPDATE dbo.Web_AppSetting SET Value = ?, ChangedAt = SYSUTCDATETIME() WHERE Section = ? AND [Key] = ?",
                           (value, *COMPLETED)):
            cur.execute("INSERT INTO dbo.Web_AppSetting (Section, [Key], Value) VALUES (?, ?, ?)", (*COMPLETED, value))


def check() -> dict:
    """What the connected database holds, for the setup screen before anything is changed."""
    wanted = set(schema_tables())
    tables = {r["name"]: r for r in _rows(
        """SELECT t.name, SUM(p.rows) AS row_count,
                  (SELECT SUM(a.used_pages) FROM sys.partitions p2 JOIN sys.allocation_units a ON a.container_id = p2.partition_id
                   WHERE p2.object_id = t.object_id) AS used_pages
           FROM sys.tables t JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
           WHERE t.schema_id = SCHEMA_ID('dbo') GROUP BY t.name, t.object_id""")}
    legacy = {n: r for n, r in tables.items() if not n.startswith("Web_")}
    found = sorted(n for n in legacy if n in wanted)
    copy_mb = sum(int(legacy[n]["used_pages"] or 0) for n in found) * 8 / 1024
    size = _rows("SELECT SUM(CAST(size AS BIGINT)) * 8 / 1024 AS mb FROM sys.database_files WHERE type = 0")
    edition = _rows("SELECT CAST(SERVERPROPERTY('Edition') AS NVARCHAR(128)) AS edition, "
                    "CAST(SERVERPROPERTY('EngineEdition') AS INT) AS engine, "
                    "CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(128)) AS version, DB_NAME() AS db")[0]
    used = _rows("SELECT SUM(CAST(FILEPROPERTY(name, 'SpaceUsed') AS BIGINT)) * 8 / 1024 AS mb FROM sys.database_files WHERE type = 0")
    used_mb = float(used[0]["mb"] or 0) if used else 0.0
    limit_mb = EXPRESS_LIMIT_MB if edition["engine"] == 4 else None
    perm = _rows("SELECT HAS_PERMS_BY_NAME(DB_NAME(), 'DATABASE', 'CREATE TABLE') AS p")[0]["p"]
    return {
        "database": edition["db"], "edition": edition["edition"], "version": edition["version"],
        "can_create_tables": bool(perm),
        "legacy_tables": len(found), "legacy_rows": sum(int(legacy[n]["row_count"] or 0) for n in found),
        "legacy_found": bool(found), "not_in_schema": sorted(n for n in legacy if n not in wanted and n not in ("sysdiagrams", "dtproperties")),
        "web_tables": sum(1 for n in tables if n.startswith("Web_")), "schema_version": schema_version(),
        "ready": is_ready(),
        "web_has_saved": web_has_saved(),
        "size_mb": round(float(size[0]["mb"] or 0), 1) if size else 0.0, "used_mb": round(used_mb, 1),
        "copy_mb": round(copy_mb, 1), "after_copy_mb": round(used_mb + copy_mb, 1), "limit_mb": limit_mb,
        "fits": limit_mb is None or used_mb + copy_mb < limit_mb * 0.95,
    }


def create_schema() -> None:
    for sql in batches(SCHEMA.read_text()):
        with db.transaction() as cur:
            cur.execute_raw(sql)


def _columns(table: str) -> list[dict]:
    return _rows("""SELECT c.name, c.is_identity, c.is_computed, TYPE_NAME(c.system_type_id) AS type
                    FROM sys.columns c WHERE c.object_id = OBJECT_ID(?) ORDER BY c.column_id""", (f"dbo.{table}",))


def _range_column(table: str, cols: list[dict]) -> str | None:
    """An integer column to copy a big table in key ranges: its identity, else the first primary key column."""
    ints = {c["name"] for c in cols if c["type"] in ("int", "bigint", "smallint", "tinyint")}
    ident = next((c["name"] for c in cols if c["is_identity"]), None)
    if ident in ints:
        return ident
    pk = _rows("""SELECT c.name FROM sys.indexes i JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                  JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                  WHERE i.object_id = OBJECT_ID(?) AND i.is_primary_key = 1 ORDER BY ic.key_ordinal""", (f"dbo.{table}",))
    return pk[0]["name"] if pk and pk[0]["name"] in ints else None


def _count(table: str) -> int:
    return int(_rows(f"SELECT COUNT_BIG(*) AS n FROM dbo.[{table}]")[0]["n"])


def copy_table(name: str) -> tuple[int, int]:
    """Copy one legacy table into its Web_ copy. Returns (source rows, copied rows)."""
    target = f"Web_{name}"
    src_cols, dst_cols = _columns(name), _columns(target)
    dst = {c["name"].lower(): c for c in dst_cols if not c["is_computed"]}
    common = [c["name"] for c in src_cols if not c["is_computed"] and c["name"].lower() in dst]
    if not common:
        raise SetupError(f"{name}: no columns in common with {target}")
    listed = ", ".join(f"[{c}]" for c in common)
    identity = any(dst[c.lower()]["is_identity"] for c in common)
    source = _count(name)
    with db.transaction() as cur:   # the Web_UtlStockList triggers are off during the copy (copy_all)
        cur.execute_raw(f"TRUNCATE TABLE dbo.[{target}]")
    key = _range_column(name, src_cols) if source > BATCH_ROWS else None
    ranges: list[tuple] = [()]
    if key:
        lo_hi = _rows(f"SELECT MIN([{key}]) AS lo, MAX([{key}]) AS hi FROM dbo.[{name}]")[0]
        lo, hi = int(lo_hi["lo"]), int(lo_hi["hi"])
        ranges = [(s, min(s + BATCH_ROWS - 1, hi)) for s in range(lo, hi + 1, BATCH_ROWS)]
    copied = 0
    for r in ranges:
        where = f" WHERE [{key}] BETWEEN ? AND ?" if r else ""
        with db.transaction() as cur:
            if identity:
                cur.execute(f"SET IDENTITY_INSERT dbo.[{target}] ON")
            try:
                n = cur.execute(f"INSERT INTO dbo.[{target}] ({listed}) SELECT {listed} FROM dbo.[{name}]{where}", r)
            finally:
                if identity:
                    cur.execute(f"SET IDENTITY_INSERT dbo.[{target}] OFF")
        copied += n
        PROGRESS["rows_done"] = PROGRESS.get("rows_done", 0) + n
        _beat()
    return source, _count(target)


def _beat() -> None:
    PROGRESS["_beat"] = time.monotonic()


def _watchdog(stop: threading.Event) -> None:
    """If the copy makes no progress for STALL_SECONDS, close the connection so the stuck statement fails
    (it is then retried on a new connection)."""
    while not stop.wait(5):
        if time.monotonic() - PROGRESS.get("_beat", time.monotonic()) > STALL_SECONDS:
            logger.warning("Setup copy made no progress for %d s: closing the connection to retry", STALL_SECONDS)
            _beat()
            db.abort_connection()


def _copy_with_retry(t: str) -> tuple[int, int]:
    """copy_table(), again on a new connection when the connection failed (each try starts the table afresh)."""
    for attempt in range(1, TABLE_ATTEMPTS + 1):
        rows_before = PROGRESS.get("rows_done", 0)
        try:
            return copy_table(t)
        except Exception as exc:   # noqa: BLE001
            if attempt == TABLE_ATTEMPTS:
                raise
            logger.warning("Copying %s failed (%s); reconnecting, try %d of %d", t, db.safe_error(exc), attempt + 1, TABLE_ATTEMPTS)
            PROGRESS["rows_done"] = rows_before
            PROGRESS["retries"] = PROGRESS.get("retries", 0) + 1
            if not db.reconnect():
                raise
    raise AssertionError("unreachable")


def _resumable_run() -> tuple[int, set[str]]:
    """The run to continue: the latest one if it never finished (a power cut, a dropped connection), with the
    tables it already copied; otherwise a new run."""
    last = int(_rows("SELECT ISNULL(MAX(RunId), 0) AS n FROM dbo.Web_Setup")[0]["n"])
    marker = _rows("SELECT Value FROM dbo.Web_AppSetting WHERE Section = ? AND [Key] = ?", COMPLETED)
    finished = int(json.loads(marker[0]["Value"])["run_id"]) if marker else 0
    if not last or last == finished:
        return last + 1, set()
    done = {r["TableName"] for r in _rows("SELECT TableName FROM dbo.Web_Setup WHERE RunId = ? AND Status = 'DONE'", (last,))}
    return last, done


def _clear_web_data() -> None:
    """Forget what the web app saved (its rows, documents, stock ledger, settings and the markers): a fresh copy
    of the old data would not match it."""
    with db.transaction() as cur:
        for t in ("Web_AppRow", "Web_AppDoc", "Web_AppMove"):
            cur.execute_raw(f"TRUNCATE TABLE dbo.{t}")
        cur.execute_raw("DELETE FROM dbo.Web_AppSetting")
    logger.warning("Setup: the web app's saved data was cleared before copying the old data again (force)")


def _log(run_id: int, table: str, **kw) -> None:
    cols = ", ".join(f"{k} = ?" for k in kw)
    with db.transaction() as cur:
        if not cur.execute(f"UPDATE dbo.Web_Setup SET {cols} WHERE RunId = ? AND TableName = ?", (*kw.values(), run_id, table)):
            cur.execute(f"INSERT INTO dbo.Web_Setup (RunId, TableName, {', '.join(kw)}) VALUES (?, ?, {', '.join('?' * len(kw))})",
                        (run_id, table, *kw.values()))


def copy_all(force: bool = False) -> dict:
    """Create the Web_ tables and copy every legacy table into them. Returns a summary; raises SetupError."""
    if not _run_lock.acquire(blocking=False):
        raise SetupError("A setup is already running")
    try:
        PROGRESS.clear()
        PROGRESS.update(state="checking", started_at=datetime.now().isoformat(timespec="seconds"))
        info = check()
        if not info["can_create_tables"]:
            raise SetupError("This SQL login can't create tables in the database. Use a login with db_owner (or CREATE TABLE) rights for the setup.")
        if not info["fits"]:
            raise SetupError(f"Not enough room: the copy needs about {info['copy_mb']:.0f} MB more, and this edition "
                             f"({info['edition']}) allows {info['limit_mb']} MB per database.")
        if info["web_has_saved"] and not force:
            raise SetupError("The web app has already saved data in the Web_ tables; copying the old data again would overwrite it.")
        PROGRESS.update(state="creating tables")
        create_schema()
        run_id, already = _resumable_run()   # before any clearing: it reads the "completed" marker
        if info["web_has_saved"]:   # force: a new copy starts the web app afresh from the old data
            _clear_web_data()
        tables = [t for t in schema_tables() if _rows("SELECT 1 AS x FROM sys.tables WHERE name = ?", (t,))]
        PROGRESS.update(state="copying", run_id=run_id, tables_total=len(tables), tables_done=0,
                        rows_total=info["legacy_rows"], rows_done=0, table="", failed=[], resumed=len(already))
        with db.transaction() as cur:
            cur.execute_raw("DISABLE TRIGGER ALL ON dbo.Web_UtlStockList")
        stop = threading.Event()
        _beat()
        threading.Thread(target=_watchdog, args=(stop,), name="vlqms-setup-watchdog", daemon=True).start()
        try:
            for t in tables:
                PROGRESS["table"] = t
                _beat()
                if t in already:   # copied by the unfinished run being continued: skip it if it still matches
                    n = _count(t)
                    if n == _count(f"Web_{t}"):
                        PROGRESS["rows_done"] += n
                        PROGRESS["tables_done"] += 1
                        continue
                _log(run_id, t, Status="COPYING", StartedAt=datetime.now())
                try:
                    source, copied = _copy_with_retry(t)
                except Exception as exc:   # noqa: BLE001 - reported per table, the run stops below
                    _log(run_id, t, Status="FAILED", Error=db.safe_error(exc)[:1000], DoneAt=datetime.now())
                    PROGRESS["failed"].append({"table": t, "error": db.safe_error(exc)})
                    raise SetupError(f"Copying {t} failed: {db.safe_error(exc)}") from exc
                ok = source == copied
                _log(run_id, t, SourceRows=source, CopiedRows=copied, Status="DONE" if ok else "MISMATCH", DoneAt=datetime.now())
                if not ok:
                    PROGRESS["failed"].append({"table": t, "error": f"{source} rows in {t}, {copied} copied"})
                    raise SetupError(f"{t}: {source} rows in the old table but {copied} copied")
                PROGRESS["tables_done"] += 1
        finally:
            stop.set()
            with db.transaction() as cur:
                cur.execute_raw("ENABLE TRIGGER ALL ON dbo.Web_UtlStockList")
        _mark_completed(run_id, len(tables), PROGRESS["rows_done"])
        PROGRESS.update(state="done", finished_at=datetime.now().isoformat(timespec="seconds"), table="")
        logger.info("Setup run %d: %d tables copied", run_id, len(tables))
        return {"run_id": run_id, "tables": len(tables), "rows": PROGRESS["rows_done"]}
    except SetupError as exc:
        PROGRESS.update(state="failed", error=str(exc))
        raise
    except Exception as exc:
        PROGRESS.update(state="failed", error=db.safe_error(exc))
        raise SetupError(db.safe_error(exc)) from exc
    finally:
        _run_lock.release()


def run_in_background(force: bool = False, on_done=None) -> None:
    """Start copy_all() in a thread (the setup screen polls PROGRESS)."""
    def work():
        try:
            copy_all(force)
            if on_done:
                on_done()
        except SetupError as exc:
            logger.warning("Setup failed: %s", exc)
    PROGRESS.update(state="starting")
    threading.Thread(target=work, name="vlqms-setup", daemon=True).start()
