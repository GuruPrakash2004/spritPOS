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

from . import gsettings, store

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

-- Tools audit trail (SQL Query Execution runs, Re Write Stock): who did what, when, and the outcome.
CREATE TABLE IF NOT EXISTS tool_audit (
    id         INTEGER PRIMARY KEY AUTOINCREMENT,
    at         TEXT NOT NULL,
    user_code  TEXT NOT NULL,
    tool       TEXT NOT NULL,                       -- "SQL Query Execution", "Re Write Stock"
    detail     TEXT NOT NULL,                       -- the SQL (first 2000 characters) or the action
    ok         INTEGER NOT NULL,                    -- 1 = succeeded, 0 = refused or failed
    row_count  INTEGER,
    message    TEXT                                 -- error / refusal text (first 500 characters)
);

-- ADMIN Audit Log (legacy UtlAdmAudit + UtlAdmAuditItem): who changed what on which screen, with the
-- earlier and changed data, and item-level rows for documents. Written with the state (app/admin_audit.py).
CREATE TABLE IF NOT EXISTS admin_audit (
    id         INTEGER PRIMARY KEY AUTOINCREMENT,
    at         TEXT NOT NULL,                       -- local date-time, seconds
    screen     TEXT NOT NULL,                       -- "Item Master", "Sales Invoice", "Merge Items", ...
    action     TEXT NOT NULL,                       -- Added, Edited, Deleted, Cancelled, ...
    main_fld   TEXT NOT NULL,                       -- document number or master code
    user_code  TEXT,
    earlier    TEXT,
    changed    TEXT
);
CREATE INDEX IF NOT EXISTS ix_admin_audit_at ON admin_audit (at);
CREATE TABLE IF NOT EXISTS admin_audit_item (
    audit_id   INTEGER NOT NULL REFERENCES admin_audit (id),
    item_code  TEXT,
    item_name  TEXT,
    status     TEXT,                                -- Added, Removed, Changed
    details    TEXT
);
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
        "gsettings": store.GSETTINGS,
        "denominations": store.DENOMINATIONS,
        "seq": {k: s.value for k, s in store._seq.items()},
        "security": {k: v for k, v in store.SECURITY.items()},
        "field_rights": store.FIELD_RIGHTS,
        "master_approval": store.MASTER_APPROVAL,
        "report_rights": store.REPORT_RIGHTS,
        "doc_settings": store.DOC_SETTINGS,
        "number_series": store.NUMBER_SERIES,
        "menu_layout": store.MENU_LAYOUT,
        "import_fields": store.IMPORT_FIELDS,
        "merged_items": store.MERGED_ITEMS,
        "merged_item_ids": store.MERGED_ITEM_IDS,
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
            _insert_audit(conn, store.AUDIT_OUT)
        store.OUTBOX.clear()   # only after the commit: if it failed, they go with the next successful save
        store.AUDIT_OUT.clear()


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
    store.mark_app_start()
    if not enabled() or not Path(PATH).exists():
        return False
    with _db() as conn:
        row = conn.execute("SELECT data FROM state WHERE id = 1").fetchone()
    if not row:
        return False
    apply(json.loads(row[0]))
    return True


def apply(data: dict) -> None:
    # 0. Items merged away in the app: note their seed/DB stock now, before step 1 removes their rows; it is
    #    added to the item they became once that item's row is back (it may have been made in the app).
    store.MERGED_ITEMS.update(data.get("merged_items", {}))
    store.MERGED_ITEM_IDS.update(data.get("merged_item_ids", {}))
    merged_stock = {old: store.ITEMS[old]["qoh"] for old in store.MERGED_ITEMS if old in store.ITEMS}
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
    for old, qty in merged_stock.items():
        target = store.ITEMS.get(store.merged_code(old))
        if target is not None and old not in store.ITEMS:
            target["qoh"] += qty
    if data.get("company"):
        store.COMPANY.update(data["company"])
        store.touch("COMPANY", "*")
    for section, values in data.get("settings", {}).items():
        store.SETTINGS.setdefault(section, {}).update(values)
    if data.get("gsettings"):
        gsettings.restore(data["gsettings"])
    else:   # a file from before Global Settings rows: take the older sections into the rows
        try:
            gsettings.set_from_legacy(data.get("settings", {}))
        except Exception:
            logger.warning("Ignored older saved settings that are out of range")
    store.DENOMINATIONS.update(data.get("denominations", {}))
    # Admin Master (Security): login settings, the additional ADMIN password hash, field/report rights, master
    # approvals. (A "user_rights" key saved by earlier builds is ignored: rights are per role only.)
    store.SECURITY.update(data.get("security", {}))
    store.FIELD_RIGHTS.update(data.get("field_rights", {}))
    if data.get("master_approval"):
        store.MASTER_APPROVAL.update(data["master_approval"])
    store.REPORT_RIGHTS.update(data.get("report_rights", {}))
    # Global Settings → Other Settings.
    store.DOC_SETTINGS.update(data.get("doc_settings", {}))
    store.NUMBER_SERIES.update(data.get("number_series", {}))
    store.MENU_LAYOUT.update(data.get("menu_layout", {}))
    store.IMPORT_FIELDS.update(data.get("import_fields", {}))
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
    store.mark_app_start()
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


def outbox_item_change(code: str) -> float:
    """The stock change for `code` in every queued write-back event (sent or not), i.e. what SQL Server's
    Web_StockBalance holds, or will hold, for it. Used by Merge Items."""
    total = 0.0
    with _db() as conn:
        for (payload,) in conn.execute("SELECT payload FROM web_outbox WHERE payload LIKE ?", (f'%{json.dumps(code)}%',)):
            total += sum(m["qty"] for m in json.loads(payload).get("moves", []) if m.get("item") == code)
    total += sum(m["qty"] for e in store.OUTBOX for m in e.get("moves", []) if m.get("item") == code)
    return round(total, 3)


def queued_documents() -> set[tuple[str, str]]:
    """(transaction_type, transaction_id) of every document that has any outbox row."""
    with _db() as conn:
        return set(conn.execute("SELECT DISTINCT transaction_type, transaction_id FROM web_outbox").fetchall())


# ─── Tools → SQL Query Execution on this local database ──────────────────────

# Tables a user's INSERT/UPDATE/DELETE may change. `state` is the app's data as one JSON row (the running
# app rewrites it after every change, so an edit there would be lost or break a restore) and `tool_audit`
# is the audit trail: both stay read-only.
LOCAL_WRITABLE = {"web_outbox"}


def _authorizer(write_table: str | None):
    """SQLite authorizer: reads anywhere; changes only to `write_table` (when given and writable);
    no PRAGMA, ATTACH, schema changes or transaction control."""
    import sqlite3 as s

    allowed_always = {s.SQLITE_SELECT, s.SQLITE_READ, s.SQLITE_FUNCTION, s.SQLITE_RECURSIVE}
    change = {s.SQLITE_INSERT, s.SQLITE_UPDATE, s.SQLITE_DELETE}

    def check(action, arg1, arg2, dbname, source):
        if action in allowed_always:
            return s.SQLITE_OK
        writing = bool(write_table) and write_table.lower() in LOCAL_WRITABLE
        if writing and action in change and arg1 and arg1.lower() in (write_table.lower(), "sqlite_sequence"):
            return s.SQLITE_OK   # sqlite_sequence: AUTOINCREMENT bookkeeping of the table being changed
        if writing and action == s.SQLITE_TRANSACTION:
            return s.SQLITE_OK   # the implicit BEGIN Python's sqlite3 issues before a change
        return s.SQLITE_DENY
    return check


def run_local_sql(sql: str, write_table: str | None = None, max_rows: int = 5000) -> dict:
    """Run one already-validated statement on this database. A read returns
    {"columns", "rows", "truncated"}; a change (write_table set) is committed and returns
    {"rows_affected"}. The SQLite authorizer is the second guard behind the caller's check."""
    with _db() as conn:
        conn.set_authorizer(_authorizer(write_table))
        try:
            cur = conn.execute(sql)
            if write_table:
                return {"rows_affected": cur.rowcount}
            if cur.description is None:
                return {"columns": [], "rows": [], "truncated": False}
            rows = [list(r) for r in cur.fetchmany(max_rows + 1)]
            return {"columns": [d[0] for d in cur.description], "rows": rows[:max_rows], "truncated": len(rows) > max_rows}
        finally:
            conn.set_authorizer(None)


def local_objects() -> list[dict]:
    """This database's tables and views (for the SQL Query Execution explorer)."""
    with _db() as conn:
        rows = conn.execute("SELECT type, name, sql FROM sqlite_master WHERE type IN ('table', 'view') "
                            "AND name NOT LIKE 'sqlite_%' ORDER BY type, name").fetchall()
    return [{"kind": "Tables" if t == "table" else "Views", "name": n, "sql": q or ""} for t, n, q in rows]


def local_columns(table: str) -> list[dict]:
    """Columns of a table or view that exists here (the name is checked against sqlite_master first)."""
    if table not in {o["name"] for o in local_objects()}:
        raise KeyError(table)
    with _db() as conn:
        info = conn.execute(f'PRAGMA table_info("{table}")').fetchall()
    return [{"name": r[1], "type": r[2] or "", "nullable": not r[3], "default": r[4], "identity": bool(r[5])} for r in info]


# ─── Tools audit (tool_audit) ─────────────────────────────────────────────────

def audit_tool(user_code: str, tool: str, detail: str, ok: bool, row_count: int | None = None,
               message: str | None = None) -> None:
    """Record one Tools action. Never raises: a failed audit write is logged, not shown to the user."""
    try:
        with _db() as conn:
            conn.execute("INSERT INTO tool_audit (at, user_code, tool, detail, ok, row_count, message) "
                         "VALUES (?, ?, ?, ?, ?, ?, ?)",
                         (_now(), user_code, tool, detail[:2000], 1 if ok else 0, row_count, (message or "")[:500] or None))
    except Exception:
        logger.exception("Could not write the Tools audit row")


def tool_audit_rows(tool: str | None = None, limit: int = 50) -> list[dict]:
    """Latest Tools audit rows, newest first."""
    with _db() as conn:
        sql, args = "SELECT id, at, user_code, tool, detail, ok, row_count, message FROM tool_audit", []
        if tool:
            sql, args = sql + " WHERE tool = ?", [tool]
        rows = conn.execute(sql + " ORDER BY id DESC LIMIT ?", [*args, limit]).fetchall()
    keys = ("id", "at", "user_code", "tool", "detail", "ok", "row_count", "message")
    return [dict(zip(keys, r)) | {"ok": bool(r[5])} for r in rows]


# ─── ADMIN Audit Log (admin_audit, admin_audit_item) ─────────────────────────

def _insert_audit(conn: sqlite3.Connection, entries: list[dict]) -> None:
    for e in entries:
        cur = conn.execute("INSERT INTO admin_audit (at, screen, action, main_fld, user_code, earlier, changed) "
                           "VALUES (?, ?, ?, ?, ?, ?, ?)",
                           (e["at"], e["screen"], e["action"], e["main_fld"], e.get("user"),
                            (e.get("earlier") or "")[:4000], (e.get("changed") or "")[:4000]))
        conn.executemany("INSERT INTO admin_audit_item (audit_id, item_code, item_name, status, details) "
                         "VALUES (?, ?, ?, ?, ?)",
                         [(cur.lastrowid, i.get("code"), i.get("name"), i.get("status"), (i.get("details") or "")[:1000])
                          for i in e.get("items", [])])


def add_audit(entries: list[dict]) -> None:
    """Write audit entries now (actions outside the write middleware). Never raises."""
    try:
        with _db() as conn:
            _insert_audit(conn, entries)
    except Exception:
        logger.exception("Could not write the admin audit rows")


def audit_rows(screen: str | None, from_at: str, to_at: str, limit: int = 5000) -> list[dict]:
    """Audit entries in [from_at, to_at] (ISO strings), newest first, each with its item rows."""
    sql = "SELECT id, at, screen, action, main_fld, user_code, earlier, changed FROM admin_audit WHERE at BETWEEN ? AND ?"
    args: list = [from_at, to_at]
    if screen:
        sql, args = sql + " AND screen = ?", [*args, screen]
    with _db() as conn:
        heads = conn.execute(sql + " ORDER BY id DESC LIMIT ?", [*args, limit]).fetchall()
        ids = [h[0] for h in heads]
        items: dict[int, list[dict]] = {}
        for i in range(0, len(ids), 500):
            chunk = ids[i:i + 500]
            for r in conn.execute(f"SELECT audit_id, item_code, item_name, status, details FROM admin_audit_item "
                                  f"WHERE audit_id IN ({','.join('?' * len(chunk))}) ORDER BY rowid", chunk):
                items.setdefault(r[0], []).append({"code": r[1], "name": r[2], "status": r[3], "details": r[4]})
    keys = ("id", "at", "screen", "action", "main_fld", "user_code", "earlier", "changed")
    return [dict(zip(keys, h)) | {"items": items.get(h[0], [])} for h in heads]


def audit_screens() -> list[str]:
    with _db() as conn:
        return [r[0] for r in conn.execute("SELECT DISTINCT screen FROM admin_audit ORDER BY screen")]


def reset_memory_outbox() -> None:
    """Tests: forget the in-memory outbox used when saving is off."""
    global _memory
    with _lock:
        if _memory is not None:
            _memory.close()
        _memory = None
