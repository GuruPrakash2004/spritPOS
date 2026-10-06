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

# No defaults for host or credentials: they come from backend/.env (never committed), written by the sign-in
# page's one-time Database setup (app/envfile.py) or by hand. DB_HOST may name an instance (localhost\SQLEXPRESS);
# DB_PORT is optional (blank for a named instance). DB_AUTH is always "sql" (DB_USER + password; Windows
# login is not supported). The password is DB_PASSWORD_ENC, sealed (secure_fields.seal), or a
# hand-written DB_PASSWORD.
DB_HOST = _env("DB_HOST")
DB_PORT = _env("DB_PORT")
DB_NAME = _env("DB_NAME")
DB_USER = _env("DB_USER")
DB_AUTH = "sql"   # SQL login only
DB_PASSWORD = _env("DB_PASSWORD")
DB_PASSWORD_ENC = _env("DB_PASSWORD_ENC")
DB_DRIVER_OVERRIDE = _env("DB_DRIVER")
DB_ENGINE = _env("DB_ENGINE").lower()   # "pyodbc" or "pymssql" forces that driver; empty tries pyodbc, then pymssql


def _connect_with_driver(*args, **kw):
    """Connect with the driver DB_ENGINE names (empty: pyodbc first, then pymssql)."""
    if DB_ENGINE == "pymssql":
        return _try_pymssql(*args, **kw)
    if DB_ENGINE == "pyodbc":
        return _try_pyodbc(*args, **kw)
    return _try_pyodbc(*args, **kw) or _try_pymssql(*args, **kw)


def settings() -> dict:
    """The connection settings in use (the password is never returned)."""
    return {"host": DB_HOST, "port": DB_PORT, "database": DB_NAME, "user": DB_USER, "auth": DB_AUTH}


# The sealed password can't be read on this PC (its backend/data/secret.key isn't the one that sealed it, e.g. a
# .env copied from another machine): treated as not set up, so the sign-in page offers Database setup again.
_password_unreadable = False


def configured() -> bool:
    return bool(DB_HOST and DB_NAME and DB_USER) and not _password_unreadable


def _password() -> str:
    if DB_PASSWORD_ENC:
        from . import secure_fields   # here, not at the top: secure_fields needs cryptography, db.py is imported early
        return secure_fields.unseal(DB_PASSWORD_ENC)
    return DB_PASSWORD


def use_settings(host: str, port: str, database: str, user: str, auth: str, password_enc: str) -> None:
    """Switch to new settings (after the setup screen saved them); call reconnect() next."""
    global DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_AUTH, DB_PASSWORD, DB_PASSWORD_ENC, _password_unreadable
    DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_AUTH = host, port, database, user, auth
    DB_PASSWORD, DB_PASSWORD_ENC = "", password_enc
    _password_unreadable = False


def _odbc(value: str) -> str:
    """A value inside {…} in an ODBC connection string: a closing brace is doubled."""
    return value.replace("}", "}}")


def pyodbc_server(host: str, port: str) -> str:
    """SERVER= for ODBC: a named instance is found by the SQL Browser, so it gets no port."""
    return host if (not port or "\\" in host) else f"{host},{port}"


# ─── Connection ──────────────────────────────────────────────────────────────

_conn = None          # shared synchronous connection; every query goes through _db_lock
# pymssql/pyodbc connections can't run two queries at once, and requests (thread pool) plus the
# background loader share this one connection, so all cursor use is serialised.
_db_lock = threading.RLock()
_db_available = False
_driver_used: str = ""


def _try_pyodbc(host: str, port: str, database: str, user: str, password: str, auth: str,
                login_timeout: int = 8) -> Any | None:
    """Attempt connection via pyodbc (requires ODBC Driver 17/18)."""
    try:
        import importlib
        pyodbc = importlib.import_module("pyodbc")
    except ImportError:   # not installed, or its native library (unixODBC) is missing
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
                f"DRIVER={{{drv}}};SERVER={pyodbc_server(host, port)};DATABASE={{{_odbc(database)}}};"
                + f"UID={{{_odbc(user)}}};PWD={{{_odbc(password)}}};"
                + f"TrustServerCertificate=yes;Encrypt=yes;MARS_Connection=yes;Connection Timeout={login_timeout};"
            )
            conn = pyodbc.connect(conn_str, autocommit=False)
            return conn, drv
        except Exception as exc:
            logger.debug("pyodbc driver '%s' failed: %s", drv, exc)

    return None


def _try_pymssql(host: str, port: str, database: str, user: str, password: str, auth: str,
                 login_timeout: int = 8) -> Any | None:
    """Attempt connection via pymssql (pure-Python, no ODBC driver needed). SQL logins only."""
    try:
        import pymssql  # noqa: F401
    except ImportError:   # not installed, or its native library is missing
        return None

    for tds in ["7.4", "7.3", None]:
        try:
            kwargs = {
                "server": host,
                "database": database,
                "user": user,
                "password": password,
                "login_timeout": login_timeout,
                "timeout": 600,          # the one-time data copy runs big INSERT ... SELECTs
            }
            if port and "\\" not in host:
                kwargs["port"] = int(port)
            if tds:
                kwargs["tds_version"] = tds
            conn = pymssql.connect(**kwargs)
            return conn, f"pymssql (TDS {tds or 'default'})"
        except Exception as exc:
            logger.warning("pymssql connection attempt (TDS %s) failed: %s", tds, exc)

    return None


def init_db() -> bool:
    """Connect to SQL Server. Returns True if successful, False if falling back to in-memory store."""
    global _conn, _db_available, _driver_used, _password_unreadable

    if not configured():
        logger.warning("No database set up in backend/.env — no data until the Database setup has run.")
        _db_available = False
        return False

    try:
        password = _password()
    except Exception:
        logger.warning("The saved database password can't be read on this PC (backend/data/secret.key is not the "
                       "one that sealed it). Run the Database setup on the sign-in page again.")
        _password_unreadable = True
        _db_available = False
        return False
    args = (DB_HOST, DB_PORT, DB_NAME, DB_USER, password, DB_AUTH)
    result = _connect_with_driver(*args)
    if result:
        _conn, _driver_used = result
        _db_available = True
        logger.info("Connected to SQL Server database '%s' via %s", DB_NAME, _driver_used)
        return True

    logger.warning("Could not connect to SQL Server database '%s'.", DB_NAME)
    _db_available = False
    return False


def test_connection(host: str, port: str, database: str, user: str, password: str, auth: str) -> dict:
    """Try settings from the setup screen on a connection of their own (the app's connection is untouched).
    Returns {"ok", "driver", "databases"} or {"ok": False, "error"}; the error never names the host."""
    args = (host, port, database or "master", user, password, auth)
    result = _connect_with_driver(*args, login_timeout=6)
    if not result:
        why = "Could not connect. Check the server name (e.g. localhost\\SQLEXPRESS), that SQL Server is running and " \
            "accepts SQL logins and TCP/IP, and the login and password."
        return {"ok": False, "error": why}
    conn, driver = result
    try:
        cur = conn.cursor()
        cur.execute("SELECT name FROM sys.databases WHERE database_id > 4 AND state = 0 ORDER BY name")
        names = [r[0] for r in cur.fetchall()]
        return {"ok": True, "driver": driver, "databases": names}
    finally:
        try:
            conn.close()
        except Exception:
            pass


def get_conn():
    """Return the live DB connection, or None if not available."""
    return _conn if _db_available else None


def is_db_available() -> bool:
    return _db_available


def driver_name() -> str:
    return _driver_used


# ─── Query helpers ────────────────────────────────────────────────────────────

def fetchall(sql: str, params: tuple = ()) -> list[dict]:
    """Execute a SELECT and return list of dicts (column name → value). Parameters use ? placeholders (for
    both drivers, as in transaction()); without parameters the SQL runs exactly as written."""
    conn = get_conn()
    if conn is None:
        return []
    for attempt in (1, 2):
        with _db_lock:
            cur = conn.cursor()
            try:
                _user_statement(cur, sql, params, _driver_used.startswith("pymssql"))
                cols = [d[0] for d in cur.description]
                return [dict(zip(cols, row)) for row in cur.fetchall()]
            except Exception as exc:
                # The server dropped the connection (10053 / "Communication link failure"): reconnect, try once more.
                if attempt == 1 and ("08S01" in str(exc) or "communication link" in str(exc).lower()):
                    logger.warning("Connection lost during a read; reconnecting and retrying")
                    if reconnect():
                        conn = get_conn()
                        continue
                raise
            finally:
                try:
                    cur.close()
                except Exception:
                    pass


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


# ─── Transactions (app/sqlstore.py, app/setup_db.py) ─────────────────────────

_last_sql = ""


def last_sql() -> str:
    """The last statement a transaction ran (for the log when a save fails)."""
    return _last_sql[:400]


class TxCursor:
    """A cursor inside db.transaction(). SQL uses ? placeholders for both drivers (pymssql wants %s),
    and values always go in `params`, never into the SQL text."""

    def __init__(self, cur, pymssql: bool):
        self._cur = cur
        self._pymssql = pymssql

    def _drain(self) -> None:
        """Discard results the caller left unread; the driver refuses the next statement ("Connection is busy
        with results for another command") while rows of the last one are pending."""
        try:
            while True:
                if self._cur.description is not None:
                    self._cur.fetchall()
                if not self._cur.nextset():
                    break
        except Exception:   # nothing pending
            pass

    def execute(self, sql: str, params: tuple = ()) -> int:
        global _last_sql
        _last_sql = sql
        self._drain()
        self._cur.execute(sql.replace("?", "%s") if self._pymssql else sql, params)
        return self._cur.rowcount

    def fetchone(self):
        """The first row; the rest of the result is discarded now, so a nested db.fetchall() (a second cursor on
        this connection, e.g. a schema lookup) is not refused as "Connection is busy"."""
        row = self._cur.fetchone()
        self._drain()
        return row

    def execute_raw(self, sql: str) -> int:
        """Run SQL with no parameters, exactly as written (schema scripts: a '?' or '%' stays as is)."""
        self._drain()
        self._cur.execute(sql)
        return self._cur.rowcount


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
            tx = TxCursor(cur, _driver_used.startswith("pymssql"))
            yield tx
            tx._drain()
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


def abort_connection() -> None:
    """Close the shared connection from another thread, WITHOUT the lock, so a statement stuck on a dead
    network connection (the server dropped it and the driver waits for ever) fails and releases the lock.
    The caller then reconnect()s. Only the setup copy's watchdog uses this."""
    conn = _conn
    if conn is not None:
        try:
            conn.close()
        except Exception:
            pass


# Set by disconnect() (About → Disconnect, for testing): nothing reconnects by itself until reconnect() is called
# by an Administrator (About → Reconnect).
MANUAL_OFFLINE = False


def disconnect() -> None:
    """Close the connection on purpose, to test how the app behaves without SQL Server (settings are kept)."""
    global _conn, _db_available, MANUAL_OFFLINE
    with _db_lock:
        old, _conn, _db_available = _conn, None, False
        MANUAL_OFFLINE = True
    if old is not None:
        try:
            old.close()
        except Exception:
            pass
    logger.warning("Disconnected from SQL Server on purpose (About → Reconnect brings it back)")


def reconnect() -> bool:
    """Drop the shared connection and connect again (after a network drop, or after disconnect())."""
    global _conn, _db_available, MANUAL_OFFLINE
    MANUAL_OFFLINE = False
    with _db_lock:   # swap the old connection out; don't hold the lock through the login timeout
        old, _conn, _db_available = _conn, None, False
    if old is not None:
        try:
            old.close()
        except Exception:
            pass
    return init_db()
