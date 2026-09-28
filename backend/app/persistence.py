"""Keeps the web app's own data across restarts, in a local SQLite file (backend/data/vlqms_state.sqlite3).

What is saved (after every successful POST/PUT/DELETE, see main.py) and restored at startup:
- documents made through the app (bills, returns, expenses, ...): those in store.DOCS with no "origin";
- stock movements made through the app, MOVEMENTS[APP_MOVEMENTS_FROM:], replayed onto item QOH;
- master rows, users (password hashes) and roles changed through the app (store.EDITS / store.touch),
  laid over the seed or SQL Server data, plus Global Settings, cash counts, company edits and the
  document number series.

Outbox (table web_outbox, same file): one row per web transaction to be copied to SQL Server's Web_*
tables (app/writeback.py). A request's rows are written in the SAME SQLite transaction as the state they
belong to, so a saved bill always has its row and a row never exists for an unsaved bill. Rows go
PENDING -> SYNCING -> SYNCED (or FAILED, retried later) and are never deleted: SYNCED is set only after SQL
Server confirmed the commit or already had the event. Rows are queued even while DB_WRITE_ENABLED is off,
so they can be sent once it is turned on.

The file holds password hashes and business data, so it is created owner-only (0600) and is gitignored.
VLQMS_STATE_DB (env) sets the file path; an empty value turns saving off (the tests do this), and the
outbox then lives in an in-memory SQLite database.
"""
import json
import logging
import os
import sqlite3
import tempfile
import threading
from contextlib import closing, contextmanager
from datetime import datetime, timedelta
from pathlib import Path
from typing import Iterator

from . import store

logger = logging.getLogger("vlqms.persistence")

DEFAULT_PATH = Path(__file__).resolve().parents[1] / "data" / "vlqms_state.sqlite3"
PATH = os.environ.get("VLQMS_STATE_DB", str(DEFAULT_PATH))

# Tables whose rows the app can change. COMPANY is a single dict (key "*").
TABLES = ("ITEMS", "ITEM_GROUPS", "UOMS", "PACKAGES", "BRANDS", "SUPPLIERS", "CUSTOMERS", "LEDGER_GROUPS",
          "LEDGERS", "COUNTERS", "USERS", "ROLES", "ROLE_RIGHTS")
# Never written to the file: a legacy DB user's reversible password is re-read from MstUser instead.
SECRET_FIELDS = ("legacy_password", "default_password")

_lock = threading.RLock()                  # every use of the file (state and outbox) in this process
_memory: sqlite3.Connection | None = None  # the outbox when saving is off

OUTBOX_SCHEMA = """
CREATE TABLE IF NOT EXISTS web_outbox (
    seq              INTEGER PRIMARY KEY AUTOINCREMENT,   -- send order
    id               TEXT NOT NULL UNIQUE,                -- event id = Web_WriteLog.EventId in SQL Server
    transaction_type TEXT NOT NULL,                       -- sales_invoice, purchase_bill, ...
    transaction_id   TEXT NOT NULL,                       -- document number
    action           TEXT NOT NULL,                       -- save, cancel, release, backfill, ...
    user_code        TEXT,
    payload          TEXT NOT NULL,                       -- the event as JSON (document + its stock moves)
    created_at       TEXT NOT NULL,
    sync_status      TEXT NOT NULL DEFAULT 'PENDING'
                     CHECK (sync_status IN ('PENDING', 'SYNCING', 'SYNCED', 'FAILED')),
    sync_attempts    INTEGER NOT NULL DEFAULT 0,
    last_attempt_at  TEXT,
    synced_at        TEXT,
    last_sync_error  TEXT
);
CREATE INDEX IF NOT EXISTS ix_web_outbox_status ON web_outbox (sync_status, seq);
CREATE INDEX IF NOT EXISTS ix_web_outbox_doc ON web_outbox (transaction_type, transaction_id);
"""


def enabled() -> bool:
    return bool(PATH)


def _connect() -> sqlite3.Connection:
    path = Path(PATH)
    path.parent.mkdir(parents=True, exist_ok=True)
    new = not path.exists()
    conn = sqlite3.connect(path, timeout=10)
    if new:
        os.chmod(path, 0o600)
    conn.execute("CREATE TABLE IF NOT EXISTS state (id INTEGER PRIMARY KEY CHECK (id = 1), saved_at TEXT, data TEXT)")
    conn.executescript(OUTBOX_SCHEMA)
    return conn


@contextmanager
def _db() -> Iterator[sqlite3.Connection]:
    """One SQLite transaction on the state file (or the in-memory outbox when saving is off):
    everything inside commits together, or rolls back together if anything raises."""
    global _memory
    with _lock:
        if enabled():
            with closing(_connect()) as conn, conn:
                yield conn
        else:
            if _memory is None:
                _memory = sqlite3.connect(":memory:", check_same_thread=False)
                _memory.executescript(OUTBOX_SCHEMA)
            with _memory:
                yield _memory


def snapshot() -> dict:
    """The app-owned part of the store, as plain JSON-able data."""
    rows: dict[str, dict] = {}
    for table in TABLES:
        data = getattr(store, table)
        out = {}
        for key in store.EDITS.get(table, ()):
            row = data.get(key)
            if isinstance(row, dict):
                row = {k: v for k, v in row.items() if k not in SECRET_FIELDS}
            out[key] = row          # None = deleted in the app
        if out:
            rows[table] = out
    return {
        "version": 1,
        "docs": {kind: {no: d for no, d in list(docs.items()) if "origin" not in d}
                 for kind, docs in store.DOCS.items()},
        "movements": store.MOVEMENTS[store.APP_MOVEMENTS_FROM:],
        "rows": rows,
        "company": store.COMPANY if "*" in store.EDITS.get("COMPANY", ()) else None,
        "settings": store.SETTINGS,
        "denominations": store.DENOMINATIONS,
        "seq": {k: s.value for k, s in store._seq.items()},
    }


def save() -> None:
    """Save the app state and the request's queued write-back events (store.OUTBOX) in ONE SQLite transaction."""
    with _lock:
        data = None
        if enabled():
            for attempt in range(3):   # the background DB loader may be changing DOCS at the same moment
                try:
                    data = json.dumps(snapshot(), default=str)
                    break
                except RuntimeError:
                    if attempt == 2:
                        raise
        with _db() as conn:
            if data is not None:
                conn.execute("INSERT OR REPLACE INTO state (id, saved_at, data) VALUES (1, ?, ?)", (_now(), data))
            _insert_events(conn, store.OUTBOX)
        store.OUTBOX.clear()   # only after the commit: if it failed, they go with the next successful save


def backup_bytes() -> bytes | None:
    """A consistent copy of the state file (state + outbox) as bytes, for the Company Master backup download.
    Taken with SQLite's online backup under the lock, so a save in progress can't leave it half-written.
    None when saving is off or nothing has been saved yet."""
    if not enabled() or not Path(PATH).exists():
        return None
    with tempfile.TemporaryDirectory() as tmp:
        target = Path(tmp) / "backup.sqlite3"
        with _lock, closing(sqlite3.connect(PATH, timeout=10)) as src, closing(sqlite3.connect(target)) as copy:
            src.backup(copy)
        return target.read_bytes()


def load() -> bool:
    """Lay the saved app data over the freshly seeded/loaded store. Returns True if anything was restored."""
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)
    if not enabled() or not Path(PATH).exists():
        return False
    with _db() as conn:
        row = conn.execute("SELECT data FROM state WHERE id = 1").fetchone()
    if not row:
        return False
    apply(json.loads(row[0]))
    return True


def apply(data: dict) -> None:
    # 1. Master rows, users and roles. Item QOH is not taken from the file: it is the seed/DB stock
    #    (0 for items made in the app) plus the app's own movements, replayed in step 3.
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
                merged = {**(existing or {}), **row}
                if table == "ITEMS":
                    merged["qoh"] = (existing or {}).get("qoh", 0)
                target[key] = merged
            else:
                target[key] = row
    if data.get("company"):
        store.COMPANY.update(data["company"])
        store.touch("COMPANY", "*")
    for section, values in data.get("settings", {}).items():
        store.SETTINGS.setdefault(section, {}).update(values)
    store.DENOMINATIONS.update(data.get("denominations", {}))
    if data.get("outbox"):
        # Files from before the outbox table kept the queue in the JSON: move it into the table (the next
        # save writes the JSON without it). INSERT OR IGNORE, so doing this twice changes nothing.
        moved = add_events(data["outbox"])
        logger.info("Moved %d queued write-back event(s) into the outbox table", moved)

    # 2. Documents and number series (never going backwards).
    for kind, docs in data.get("docs", {}).items():
        store.DOCS.setdefault(kind, {}).update(docs)
    for kind, value in data.get("seq", {}).items():
        seq = store._seq.setdefault(kind, store.Seq(value))
        seq.value = max(seq.value, value)

    # 3. Stock movements made in the app, replayed onto QOH.
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)
    for m in data.get("movements", []):
        item = store.ITEMS.get(m["item"])
        if item is not None:
            item["qoh"] += m["qty"]
        if m["kind"] == "purchase" and m["qty"] > 0:
            store.LAST_PURCHASE[m["item"]] = m["date"]
        store.MOVEMENTS.append(m)
    logger.info("Restored app data: %d documents, %d stock movements, %d changed rows",
                sum(len(d) for d in data.get("docs", {}).values()), len(data.get("movements", [])),
                sum(len(r) for r in data.get("rows", {}).values()))


# ─── Outbox (web_outbox) ─────────────────────────────────────────────────────

def _now() -> str:
    return datetime.now().isoformat(timespec="seconds")


def _insert_events(conn: sqlite3.Connection, events: list[dict]) -> int:
    """Add events as PENDING (an id already there is left alone). Returns how many were added."""
    added = 0
    for e in events:
        cur = conn.execute(
            "INSERT OR IGNORE INTO web_outbox (id, transaction_type, transaction_id, action, user_code, payload, "
            "created_at, sync_status) VALUES (?, ?, ?, ?, ?, ?, ?, 'PENDING')",
            (e["id"], e["kind"], str(e["doc_no"]), e["action"], e.get("user"), json.dumps(e, default=str),
             e.get("at") or _now()))
        added += cur.rowcount
    return added


def add_events(events: list[dict]) -> int:
    """Queue events outside a request (admin backfill, legacy queue). Returns how many were added."""
    with _db() as conn:
        return _insert_events(conn, events)


def pending_rows(include_failed: bool = True, limit: int | None = None) -> list[dict]:
    """Rows waiting to be sent, oldest first."""
    statuses = ("PENDING", "FAILED") if include_failed else ("PENDING",)
    sql = (f"SELECT id, transaction_type, transaction_id, payload, sync_attempts FROM web_outbox "
           f"WHERE sync_status IN ({','.join('?' * len(statuses))}) ORDER BY seq")
    params: tuple = statuses
    if limit:
        sql += " LIMIT ?"
        params += (limit,)
    with _db() as conn:
        rows = conn.execute(sql, params).fetchall()
    return [{"id": r[0], "transaction_type": r[1], "transaction_id": r[2], "event": json.loads(r[3]),
             "attempts": r[4]} for r in rows]


def claim(event_id: str) -> bool:
    """PENDING/FAILED -> SYNCING for one row. False if another worker already took it (one atomic UPDATE)."""
    with _db() as conn:
        cur = conn.execute("UPDATE web_outbox SET sync_status = 'SYNCING', sync_attempts = sync_attempts + 1, "
                           "last_attempt_at = ? WHERE id = ? AND sync_status IN ('PENDING', 'FAILED')",
                           (_now(), event_id))
        return cur.rowcount == 1


def mark_synced(event_id: str) -> None:
    """Call only after SQL Server committed the event or already had it."""
    with _db() as conn:
        conn.execute("UPDATE web_outbox SET sync_status = 'SYNCED', synced_at = ?, last_sync_error = NULL "
                     "WHERE id = ? AND sync_status = 'SYNCING'", (_now(), event_id))


def mark_failed(event_id: str, error: str) -> None:
    with _db() as conn:
        conn.execute("UPDATE web_outbox SET sync_status = 'FAILED', last_sync_error = ? "
                     "WHERE id = ? AND sync_status = 'SYNCING'", (error[:500], event_id))


def reset_stale(older_than_seconds: int | None = None) -> int:
    """SYNCING rows left behind by a crash -> PENDING. Safe: SQL Server skips an event it already has.
    Without an age it resets every SYNCING row (at startup, when nothing can be mid-send)."""
    sql, params = "UPDATE web_outbox SET sync_status = 'PENDING' WHERE sync_status = 'SYNCING'", ()
    if older_than_seconds is not None:
        cutoff = (datetime.now() - timedelta(seconds=older_than_seconds)).isoformat(timespec="seconds")
        sql, params = sql + " AND (last_attempt_at IS NULL OR last_attempt_at < ?)", (cutoff,)
    with _db() as conn:
        return conn.execute(sql, params).rowcount


def retry_failed(ids: list[str] | None = None) -> int:
    """FAILED -> PENDING, for all failed rows or only the given ids. Returns how many."""
    with _db() as conn:
        if ids:
            marks = ",".join("?" * len(ids))
            cur = conn.execute("UPDATE web_outbox SET sync_status = 'PENDING' WHERE sync_status = 'FAILED' "
                               f"AND id IN ({marks})", tuple(ids))
        else:
            cur = conn.execute("UPDATE web_outbox SET sync_status = 'PENDING' WHERE sync_status = 'FAILED'")
        return cur.rowcount


def outbox_counts() -> dict:
    with _db() as conn:
        counts = dict(conn.execute("SELECT sync_status, COUNT(*) FROM web_outbox GROUP BY sync_status").fetchall())
        oldest = conn.execute("SELECT MIN(created_at) FROM web_outbox "
                              "WHERE sync_status IN ('PENDING', 'FAILED')").fetchone()[0]
    return {"pending": counts.get("PENDING", 0), "syncing": counts.get("SYNCING", 0),
            "synced": counts.get("SYNCED", 0), "failed": counts.get("FAILED", 0), "oldest_pending_at": oldest}


def outbox_rows(status: str | None = None, limit: int = 50) -> list[dict]:
    """Rows for the admin, newest first, without the payload."""
    keys = ("id", "transaction_type", "transaction_id", "action", "user_code", "created_at", "sync_status",
            "sync_attempts", "last_attempt_at", "synced_at", "last_sync_error")
    sql, params = f"SELECT {', '.join(keys)} FROM web_outbox", ()
    if status:
        sql, params = sql + " WHERE sync_status = ?", (status,)
    with _db() as conn:
        rows = conn.execute(sql + " ORDER BY seq DESC LIMIT ?", params + (limit,)).fetchall()
    return [dict(zip(keys, r)) for r in rows]


def queued_documents() -> set[tuple[str, str]]:
    """(transaction_type, transaction_id) of every document that has any outbox row."""
    with _db() as conn:
        return set(conn.execute("SELECT DISTINCT transaction_type, transaction_id FROM web_outbox").fetchall())


def reset_memory_outbox() -> None:
    """Tests: forget the in-memory outbox used when saving is off."""
    global _memory
    with _lock:
        if _memory is not None:
            _memory.close()
        _memory = None
