"""Where the central site keeps its data: shops (MstClient), their pushes (the desktop's central tables and
RptWebStk) and the central users (CenUser, CenUserClient: which shops an owner sees).

SqlRepo uses the central SQL Server (the tables of migrations/central/001_central.sql) through app/db.py, one
transaction per push. MemoryRepo keeps everything in this process, for tests and for trying the central site
without a database (nothing survives a restart). Times are UTC, and are returned with their offset so a
browser in any time zone reads them right.

When the central database can't be reached SqlRepo raises Unavailable (the API answers 503, so a shop keeps
the day and sends it again) and connects again on the next call; it never falls back to memory.
"""
import copy
import functools
import logging
import threading
from datetime import datetime, timezone

from . import tables as T

logger = logging.getLogger("vlqms.central")
STOCK_BATCH = 150     # rows per INSERT: SQL Server takes at most 2100 parameters in one statement


class Unavailable(RuntimeError):
    """The central database can't be reached right now."""


def _now() -> str:
    return datetime.now(timezone.utc).isoformat(timespec="seconds")


def _utc(value):
    """A time read from SQL Server (stored as UTC, without an offset) as ISO text with its offset."""
    if value is None or not hasattr(value, "isoformat"):
        return value
    if getattr(value, "tzinfo", None) is None and hasattr(value, "hour"):
        value = value.replace(tzinfo=timezone.utc)
    return value.isoformat(timespec="seconds") if hasattr(value, "hour") else value.isoformat()


def _like(q: str) -> str:
    return "%" + q.replace("[", "[[]").replace("%", "[%]").replace("_", "[_]") + "%"


class MemoryRepo:
    kind = "memory"

    def __init__(self) -> None:
        self._lock = threading.RLock()
        self._clients: dict[int, dict] = {}
        self._days: dict[tuple[int, str], dict[str, list[dict]]] = {}
        self._stock: dict[int, list[dict]] = {}
        self._users: dict[str, dict] = {}
        self._grants: set[tuple[str, int]] = set()

    # Shops
    def add_client(self, code: str, name: str, key_hash: str) -> dict:
        with self._lock:
            if any(c["code"] == code for c in self._clients.values()):
                raise ValueError(f"Shop '{code}' already exists")
            cid = max(self._clients, default=0) + 1
            self._clients[cid] = {"id": cid, "code": code, "name": name, "active": True, "key_hash": key_hash,
                                  "last_sync": None, "last_stock_sync": None, "last_day": None}
            return self._public(self._clients[cid])

    def _by_code(self, code: str) -> dict:
        found = next((c for c in self._clients.values() if c["code"] == code), None)
        if found is None:
            raise KeyError(f"No shop '{code}'")
        return found

    def set_key(self, code: str, key_hash: str) -> None:
        with self._lock:
            self._by_code(code)["key_hash"] = key_hash

    def set_active(self, code: str, active: bool) -> None:
        with self._lock:
            self._by_code(code)["active"] = active

    def client_by_key(self, key_hash: str) -> dict | None:
        with self._lock:
            c = next((c for c in self._clients.values() if c["key_hash"] == key_hash), None)
            return self._public(c) if c else None

    def clients(self, ids: list[int] | None = None) -> list[dict]:
        with self._lock:
            return [self._public(c) for c in sorted(self._clients.values(), key=lambda c: c["code"])
                    if ids is None or c["id"] in ids]

    @staticmethod
    def _public(c: dict) -> dict:
        return {k: v for k, v in c.items() if k != "key_hash"}

    # Pushes
    def replace_day(self, client_id: int, day: str, tables: dict[str, list[dict]]) -> None:
        with self._lock:
            self._days[(client_id, day)] = copy.deepcopy(tables)
            c = self._clients[client_id]
            c["last_sync"], c["last_day"] = _now(), max(c["last_day"] or day, day)

    def replace_stock(self, client_id: int, rows: list[dict]) -> None:
        with self._lock:
            at = _now()
            self._stock[client_id] = [{**r, "SyncedAt": at} for r in rows]
            self._clients[client_id]["last_stock_sync"] = at

    def day_rows(self, client_ids: list[int], day: str) -> dict[str, list[dict]]:
        with self._lock:
            out: dict[str, list[dict]] = {name: [] for name in T.DAY_TABLES}
            for cid in client_ids:
                for name, rows in self._days.get((cid, day), {}).items():
                    out[name].extend({"ClientID": cid, **r} for r in rows)
            return out

    def stock(self, client_ids: list[int], q: str, limit: int) -> list[dict]:
        ql = q.lower()
        with self._lock:
            rows = [{"ClientID": cid, **r} for cid in client_ids for r in self._stock.get(cid, [])
                    if not ql or ql in (r["ItemName"] or "").lower() or ql in (r["ItemCode"] or "").lower()]
        return rows[:limit]

    # Users
    def user(self, name: str) -> dict | None:
        with self._lock:
            u = self._users.get(name)
            return dict(u) if u else None

    def users(self) -> list[dict]:
        with self._lock:
            return [dict(u) for u in sorted(self._users.values(), key=lambda u: u["user"])]

    def add_user(self, name: str, password_hash: str, role: str) -> None:
        with self._lock:
            if name in self._users:
                raise ValueError(f"User '{name}' already exists")
            self._users[name] = {"user": name, "password_hash": password_hash, "role": role, "pwv": 1,
                                 "active": True, "must_change": True}

    def set_password(self, name: str, password_hash: str, must_change: bool) -> int:
        with self._lock:
            u = self._users[name]
            u.update(password_hash=password_hash, must_change=must_change, pwv=u["pwv"] + 1)
            return u["pwv"]

    def set_user_active(self, name: str, active: bool) -> None:
        with self._lock:
            if name not in self._users:
                raise KeyError(f"No user '{name}'")
            u = self._users[name]
            u.update(active=active, pwv=u["pwv"] + 1)

    def grant(self, name: str, client_id: int) -> None:
        with self._lock:
            self._grants.add((name, client_id))

    def revoke(self, name: str, client_id: int) -> None:
        with self._lock:
            self._grants.discard((name, client_id))

    def user_client_ids(self, name: str) -> list[int]:
        with self._lock:
            return sorted(cid for user, cid in self._grants if user == name)


def _sql(fn):
    """Connect first if needed; a database failure becomes Unavailable (503) and a duplicate a ValueError (422)."""
    @functools.wraps(fn)
    def run(self, *args, **kwargs):
        self._ready()
        try:
            return fn(self, *args, **kwargs)
        except (KeyError, ValueError):
            raise
        except Exception as exc:
            if type(exc).__name__ == "IntegrityError":
                raise ValueError("Duplicate data (an item code or a name sent twice)") from None
            logger.warning("Central database call failed (%s); connecting again on the next call", type(exc).__name__)
            self._broken = True
            raise Unavailable("The central database is not reachable right now. Try again shortly.") from None
    return run


class SqlRepo:
    """The same, in the central SQL Server (app/db.py's connection)."""
    kind = "sql"

    def __init__(self, db) -> None:
        self.db = db
        self._broken = not db.is_db_available()
        self._reconnect_lock = threading.Lock()

    def _ready(self) -> None:
        if not self._broken and self.db.is_db_available():
            return
        with self._reconnect_lock:
            if self._broken or not self.db.is_db_available():
                if not self.db.reconnect():
                    raise Unavailable("The central database is not reachable right now. Try again shortly.")
                self._broken = False

    # Shops
    @staticmethod
    def _client(r: dict) -> dict:
        return {"id": r["ClientID"], "code": r["ClientCode"], "name": r["ClientName"], "active": bool(r["Active"]),
                "last_sync": _utc(r["LastSyncDateTime"]), "last_stock_sync": _utc(r["LastStockSyncDateTime"]),
                "last_day": r["LastSyncDay"].isoformat() if r["LastSyncDay"] else None}

    _CLIENT_COLS = "ClientID, ClientCode, ClientName, Active, LastSyncDateTime, LastStockSyncDateTime, LastSyncDay"

    @_sql
    def add_client(self, code: str, name: str, key_hash: str) -> dict:
        if self.db.fetchone("SELECT ClientID FROM MstClient WHERE ClientCode = ?", (code,)):
            raise ValueError(f"Shop '{code}' already exists")
        with self.db.transaction() as cur:
            cur.execute("INSERT INTO MstClient (ClientCode, ClientName, SyncKeyHash, Active) VALUES (?, ?, ?, 1)",
                        (code, name, key_hash))
        return self._client(self.db.fetchone(f"SELECT {self._CLIENT_COLS} FROM MstClient WHERE ClientCode = ?", (code,)))

    def _update_client(self, sql: str, params: tuple, code: str) -> None:
        with self.db.transaction() as cur:
            if cur.execute(sql, params) == 0:
                raise KeyError(f"No shop '{code}'")

    @_sql
    def set_key(self, code: str, key_hash: str) -> None:
        self._update_client("UPDATE MstClient SET SyncKeyHash = ? WHERE ClientCode = ?", (key_hash, code), code)

    @_sql
    def set_active(self, code: str, active: bool) -> None:
        self._update_client("UPDATE MstClient SET Active = ? WHERE ClientCode = ?", (1 if active else 0, code), code)

    @_sql
    def client_by_key(self, key_hash: str) -> dict | None:
        r = self.db.fetchone(f"SELECT {self._CLIENT_COLS} FROM MstClient WHERE SyncKeyHash = ?", (key_hash,))
        return self._client(r) if r else None

    @_sql
    def clients(self, ids: list[int] | None = None) -> list[dict]:
        rows = [self._client(r) for r in self.db.fetchall(f"SELECT {self._CLIENT_COLS} FROM MstClient ORDER BY ClientCode")]
        return rows if ids is None else [c for c in rows if c["id"] in ids]

    # Pushes
    @_sql
    def replace_day(self, client_id: int, day: str, tables: dict[str, list[dict]]) -> None:
        with self.db.transaction() as cur:
            for name, columns in T.DAY_TABLES.items():
                cur.execute(f"DELETE FROM {name} WHERE ClientID = ? AND DocDate = ?", (client_id, day))
                cols = ", ".join(["ClientID", "DocDate", *columns])
                marks = ", ".join("?" * (len(columns) + 2))
                for row in tables.get(name, []):
                    cur.execute(f"INSERT INTO {name} ({cols}) VALUES ({marks})",
                                (client_id, day, *(row[c] for c in columns)))
            cur.execute("UPDATE MstClient SET LastSyncDateTime = SYSUTCDATETIME(), LastSyncDay = CASE WHEN LastSyncDay IS "
                        "NULL OR LastSyncDay < ? THEN ? ELSE LastSyncDay END WHERE ClientID = ?", (day, day, client_id))

    @_sql
    def replace_stock(self, client_id: int, rows: list[dict]) -> None:
        cols = ", ".join(["ClientID", *T.STOCK_COLUMNS, "SyncedAt"])
        one = "(" + ", ".join("?" * (len(T.STOCK_COLUMNS) + 1)) + ", SYSUTCDATETIME())"
        values = lambda row: (client_id, *(1 if row[c] is True else 0 if row[c] is False else row[c] for c in T.STOCK_COLUMNS))
        with self.db.transaction() as cur:
            cur.execute(f"DELETE FROM {T.STOCK_TABLE} WHERE ClientID = ?", (client_id,))
            for start in range(0, len(rows), STOCK_BATCH):   # many rows per statement: one round trip each
                batch = rows[start:start + STOCK_BATCH]
                cur.execute(f"INSERT INTO {T.STOCK_TABLE} ({cols}) VALUES " + ", ".join([one] * len(batch)),
                            tuple(v for row in batch for v in values(row)))
            cur.execute("UPDATE MstClient SET LastStockSyncDateTime = SYSUTCDATETIME() WHERE ClientID = ?", (client_id,))

    @staticmethod
    def _ids(client_ids: list[int]) -> tuple[str, tuple]:
        return ", ".join("?" * len(client_ids)), tuple(client_ids)

    @staticmethod
    def _plain(row: dict) -> dict:
        out = {}
        for k, v in row.items():
            if hasattr(v, "isoformat"):
                v = _utc(v)
            elif v is not None and type(v).__name__ == "Decimal":
                v = float(v)
            out[k] = v
        return out

    @_sql
    def day_rows(self, client_ids: list[int], day: str) -> dict[str, list[dict]]:
        if not client_ids:
            return {name: [] for name in T.DAY_TABLES}
        marks, ids = self._ids(client_ids)
        out = {}
        for name, columns in T.DAY_TABLES.items():
            cols = ", ".join(["ClientID", *columns])
            out[name] = [self._plain(r) for r in self.db.fetchall(
                f"SELECT {cols} FROM {name} WHERE DocDate = ? AND ClientID IN ({marks})", (day, *ids))]
        return out

    @_sql
    def stock(self, client_ids: list[int], q: str, limit: int) -> list[dict]:
        if not client_ids:
            return []
        marks, ids = self._ids(client_ids)
        cols = ", ".join(["ClientID", *T.STOCK_COLUMNS, "SyncedAt"])
        where, params = f"ClientID IN ({marks})", ids
        if q:
            where += " AND (ItemName LIKE ? OR ItemCode LIKE ?)"
            params += (_like(q), _like(q))
        rows = self.db.fetchall(f"SELECT TOP {int(limit)} {cols} FROM {T.STOCK_TABLE} WHERE {where} ORDER BY ItemName", params)
        return [{**self._plain(r), "Active": bool(r["Active"])} for r in rows]

    # Users
    @staticmethod
    def _user(r: dict) -> dict:
        return {"user": r["UserName"], "password_hash": r["PasswordHash"], "role": r["Role"], "pwv": r["Pwv"],
                "active": bool(r["Active"]), "must_change": bool(r["MustChange"])}

    @_sql
    def user(self, name: str) -> dict | None:
        r = self.db.fetchone("SELECT * FROM CenUser WHERE UserName = ?", (name,))
        return self._user(r) if r else None

    @_sql
    def users(self) -> list[dict]:
        return [self._user(r) for r in self.db.fetchall("SELECT * FROM CenUser ORDER BY UserName")]

    @_sql
    def add_user(self, name: str, password_hash: str, role: str) -> None:
        if self.user(name):
            raise ValueError(f"User '{name}' already exists")
        with self.db.transaction() as cur:
            cur.execute("INSERT INTO CenUser (UserName, PasswordHash, Role, Pwv, Active, MustChange) VALUES (?, ?, ?, 1, 1, 1)",
                        (name, password_hash, role))

    @_sql
    def set_password(self, name: str, password_hash: str, must_change: bool) -> int:
        with self.db.transaction() as cur:
            cur.execute("UPDATE CenUser SET PasswordHash = ?, MustChange = ?, Pwv = Pwv + 1 WHERE UserName = ?",
                        (password_hash, 1 if must_change else 0, name))
        return self.user(name)["pwv"]

    @_sql
    def set_user_active(self, name: str, active: bool) -> None:
        with self.db.transaction() as cur:
            if cur.execute("UPDATE CenUser SET Active = ?, Pwv = Pwv + 1 WHERE UserName = ?", (1 if active else 0, name)) == 0:
                raise KeyError(f"No user '{name}'")

    @_sql
    def grant(self, name: str, client_id: int) -> None:
        with self.db.transaction() as cur:
            cur.execute("IF NOT EXISTS (SELECT 1 FROM CenUserClient WHERE UserName = ? AND ClientID = ?) "
                        "INSERT INTO CenUserClient (UserName, ClientID) VALUES (?, ?)", (name, client_id, name, client_id))

    @_sql
    def revoke(self, name: str, client_id: int) -> None:
        with self.db.transaction() as cur:
            cur.execute("DELETE FROM CenUserClient WHERE UserName = ? AND ClientID = ?", (name, client_id))

    @_sql
    def user_client_ids(self, name: str) -> list[int]:
        return sorted(r["ClientID"] for r in self.db.fetchall("SELECT ClientID FROM CenUserClient WHERE UserName = ?", (name,)))
