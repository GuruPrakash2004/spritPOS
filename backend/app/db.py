"""Database connection layer for SQL Server.

Tries ODBC Driver 18 → 17 → pymssql in order.
Falls back to the in-memory store if no DB is reachable so the app still
runs during development without a live SQL Server.
"""
import os
import logging
import threading
from contextlib import contextmanager
from typing import Any, Iterator

logger = logging.getLogger("vlqms.db")

# ─── Config ──────────────────────────────────────────────────────────────────

def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


def _load_dotenv() -> None:
    """Minimal .env loader — avoids a python-dotenv dependency."""
    env_path = os.path.join(os.path.dirname(__file__), "..", ".env")
    env_path = os.path.normpath(env_path)
    if not os.path.exists(env_path):
        return
    with open(env_path) as f:
        for raw in f:
            line = raw.strip()
            if not line or line.startswith("#") or "=" not in line:
                continue
            key, _, val = line.partition("=")
            key = key.strip()
            val = val.strip().strip('"').strip("'")
            os.environ.setdefault(key, val)


_load_dotenv()

# No defaults for host or credentials: they come from backend/.env (never committed).
DB_HOST = _env("DB_HOST")
DB_PORT = _env("DB_PORT", "1433")
DB_NAME = _env("DB_NAME", "vLQMSLHOUSE")
DB_USER = _env("DB_USER")
DB_PASSWORD = _env("DB_PASSWORD")
DB_DRIVER_OVERRIDE = _env("DB_DRIVER")

# ─── Connection ──────────────────────────────────────────────────────────────

_conn = None          # shared synchronous connection; every query goes through _db_lock
# pymssql/pyodbc connections can't run two queries at once, and requests (thread pool) plus the
# background loader share this one connection, so all cursor use is serialised.
_db_lock = threading.RLock()
_db_available = False
_driver_used: str = ""


def _try_pyodbc() -> Any | None:
    """Attempt connection via pyodbc (requires ODBC Driver 17/18)."""
    try:
        import importlib
        pyodbc = importlib.import_module("pyodbc")
    except ModuleNotFoundError:
        return None

    drivers = (
        [DB_DRIVER_OVERRIDE] if DB_DRIVER_OVERRIDE
        else [
            "ODBC Driver 18 for SQL Server",
            "ODBC Driver 17 for SQL Server",
        ]
    )

    for drv in drivers:
        try:
            conn_str = (
                f"DRIVER={{{drv}}};"
                f"SERVER={DB_HOST},{DB_PORT};"
                f"DATABASE={DB_NAME};"
                f"UID={DB_USER};"
                f"PWD={DB_PASSWORD};"
                "TrustServerCertificate=yes;"
                "Encrypt=yes;"
                "Connection Timeout=8;"
            )
            conn = pyodbc.connect(conn_str, autocommit=False)
            return conn, drv
        except Exception as exc:
            logger.debug("pyodbc driver '%s' failed: %s", drv, exc)

    return None


def _try_pymssql() -> Any | None:
    """Attempt connection via pymssql (pure-Python, no ODBC driver needed)."""
    try:
        import pymssql  # noqa: F401
    except ModuleNotFoundError:
        return None

    for tds in ["7.4", "7.3", None]:
        try:
            kwargs = {
                "server": DB_HOST,
                "port": int(DB_PORT),
                "database": DB_NAME,
                "user": DB_USER,
                "password": DB_PASSWORD,
                "login_timeout": 8,
                "timeout": 30,
            }
            if tds:
                kwargs["tds_version"] = tds
            conn = pymssql.connect(**kwargs)
            return conn, f"pymssql (TDS {tds or 'default'})"
        except Exception as exc:
            logger.warning("pymssql connection attempt (TDS %s) failed: %s", tds, exc)

    return None


def init_db() -> bool:
    """Connect to SQL Server. Returns True if successful, False if falling back to in-memory store."""
    global _conn, _db_available, _driver_used

    if not (DB_HOST and DB_USER):
        logger.warning("DB_HOST / DB_USER not set in backend/.env — running with in-memory store.")
        _db_available = False
        return False

    result = _try_pyodbc() or _try_pymssql()
    if result:
        _conn, _driver_used = result
        _db_available = True
        logger.info("✅ Connected to SQL Server '%s' on %s:%s via %s", DB_NAME, DB_HOST, DB_PORT, _driver_used)
        return True

    logger.warning(
        "⚠️  Could not connect to SQL Server at %s:%s/%s — running with in-memory store.",
        DB_HOST, DB_PORT, DB_NAME,
    )
    _db_available = False
    return False


def get_conn():
    """Return the live DB connection, or None if not available."""
    return _conn if _db_available else None


def is_db_available() -> bool:
    return _db_available


def driver_name() -> str:
    return _driver_used


# ─── Query helpers ────────────────────────────────────────────────────────────

def fetchall(sql: str, params: tuple = ()) -> list[dict]:
    """Execute a SELECT and return list of dicts (column name → value)."""
    conn = get_conn()
    if conn is None:
        return []
    with _db_lock:
        cur = conn.cursor()
        try:
            cur.execute(sql, params)
            cols = [d[0] for d in cur.description]
            return [dict(zip(cols, row)) for row in cur.fetchall()]
        finally:
            cur.close()


def fetchone(sql: str, params: tuple = ()) -> dict | None:
    rows = fetchall(sql, params)
    return rows[0] if rows else None


def execute(sql: str, params: tuple = ()) -> int:
    """Execute INSERT/UPDATE/DELETE; returns rowcount. Commits immediately."""
    conn = get_conn()
    if conn is None:
        raise RuntimeError("No database connection")
    with _db_lock:
        cur = conn.cursor()
        try:
            cur.execute(sql, params)
            conn.commit()
            return cur.rowcount
        except Exception:
            conn.rollback()
            raise
        finally:
            cur.close()


def executemany(sql: str, rows: list[tuple]) -> int:
    """Bulk insert/update; returns total rowcount."""
    conn = get_conn()
    if conn is None:
        raise RuntimeError("No database connection")
    with _db_lock:
        cur = conn.cursor()
        try:
            cur.executemany(sql, rows)
            conn.commit()
            return cur.rowcount
        except Exception:
            conn.rollback()
            raise
        finally:
            cur.close()


# ─── Transactions (write-back, app/writeback.py) ─────────────────────────────

class TxCursor:
    """A cursor inside db.transaction(). SQL uses ? placeholders for both drivers (pymssql wants %s),
    and values always go in `params`, never into the SQL text."""

    def __init__(self, cur, pymssql: bool):
        self._cur = cur
        self._pymssql = pymssql

    def execute(self, sql: str, params: tuple = ()) -> int:
        self._cur.execute(sql.replace("?", "%s") if self._pymssql else sql, params)
        return self._cur.rowcount

    def fetchone(self):
        return self._cur.fetchone()


@contextmanager
def transaction() -> Iterator[TxCursor]:
    """Run several statements as one unit: all commit together, or all roll back if any fails.
    Holds _db_lock throughout, so nothing else uses the shared connection in the middle."""
    conn = get_conn()
    if conn is None:
        raise RuntimeError("No database connection")
    with _db_lock:
        cur = conn.cursor()
        try:
            yield TxCursor(cur, _driver_used.startswith("pymssql"))
            conn.commit()
        except Exception:
            try:
                conn.rollback()
            except Exception:   # the connection itself may be gone; the server rolls back on disconnect
                logger.warning("Rollback failed (connection lost?)")
            raise
        finally:
            try:
                cur.close()
            except Exception:
                pass


def _user_statement(cur, sql: str, params: tuple, pymssql: bool) -> None:
    """Run SQL typed by a user: never through the driver's parameter substitution when there are no
    parameters, so a '?' or '%' inside the text stays as typed."""
    if params:
        cur.execute(sql.replace("?", "%s") if pymssql else sql, params)
    else:
        cur.execute(sql)


def read_only(sql: str, params: tuple = (), max_rows: int = 5000, lock_timeout_ms: int = 10000) -> dict:
    """Run one query for Tools → SQL Query Execution (and other read-only tool queries) and ALWAYS roll
    back, so a statement that got past the caller's check still can't change anything. Returns
    {"columns", "rows", "truncated"}; rows stop at max_rows. Waits at most lock_timeout_ms for locked rows
    instead of hanging the shared connection. The caller validates the SQL first (routers/tools.py)."""
    conn = get_conn()
    if conn is None:
        raise RuntimeError("No database connection")
    pymssql = _driver_used.startswith("pymssql")
    with _db_lock:
        cur = conn.cursor()
        try:
            cur.execute(f"SET LOCK_TIMEOUT {int(lock_timeout_ms)}")
            _user_statement(cur, sql, params, pymssql)
            if cur.description is None:
                return {"columns": [], "rows": [], "truncated": False}
            columns = [d[0] or f"(column {i + 1})" for i, d in enumerate(cur.description)]
            rows = [list(r) for r in cur.fetchmany(max_rows + 1)]
            return {"columns": columns, "rows": rows[:max_rows], "truncated": len(rows) > max_rows}
        finally:
            _finish(conn, cur)


def write_statement(sql: str, lock_timeout_ms: int = 10000) -> int:
    """Run ONE already-validated INSERT/UPDATE/DELETE (Tools → SQL Query Execution, Web_* tables only) and
    commit it; roll back if it fails. Returns the rows affected."""
    conn = get_conn()
    if conn is None:
        raise RuntimeError("No database connection")
    with _db_lock:
        cur = conn.cursor()
        try:
            cur.execute(f"SET LOCK_TIMEOUT {int(lock_timeout_ms)}")
            cur.execute(sql)
            count = cur.rowcount
            conn.commit()
            return count
        except Exception:
            try:
                conn.rollback()
            except Exception:
                logger.warning("Rollback failed (connection lost?)")
            raise
        finally:
            _finish(conn, cur, rollback=False)


def _finish(conn, cur, rollback: bool = True) -> None:
    """End a tool statement: roll back (reads), put the shared connection's lock timeout back to
    'wait for ever' (SET is not undone by a rollback) and close the cursor. Never raises."""
    try:
        if rollback:
            conn.rollback()
        cur.execute("SET LOCK_TIMEOUT -1")
        conn.commit()   # ends the tiny transaction the SET may have opened; nothing else is pending
    except Exception:
        logger.warning("Could not reset the connection after a tool query (connection lost?)")
    try:
        cur.close()
    except Exception:
        pass


def safe_error(exc: Exception) -> str:
    """An error for the user: SQL Server's own message for SQL mistakes, but a plain sentence for
    connection failures, whose driver text can include the server's address."""
    kind = type(exc).__name__
    raw = exc.args[1] if len(exc.args) > 1 and isinstance(exc.args[1], (str, bytes)) else (exc.args[0] if exc.args else exc)
    if isinstance(raw, bytes):
        raw = raw.decode("utf-8", "replace")
    text = str(raw)
    lowered = text.lower()
    if kind in ("OperationalError", "InterfaceError") and any(w in lowered for w in (
            "unavailable", "does not exist", "connect", "network", "timeout", "login", "closed", "adaptive server")):
        return "The connection to SQL Server failed. Check the network and try again."
    if "adaptive server is unavailable" in lowered or "tcp provider" in lowered:
        return "The connection to SQL Server failed. Check the network and try again."
    return text[:1000]


def reconnect() -> bool:
    """Drop the shared connection and connect again (after a network drop)."""
    global _conn, _db_available
    with _db_lock:   # swap the old connection out; don't hold the lock through the login timeout
        old, _conn, _db_available = _conn, None, False
    if old is not None:
        try:
            old.close()
        except Exception:
            pass
    return init_db()
