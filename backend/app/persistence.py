"""Keeps the web app's own data across restarts, in a local SQLite file (backend/data/vlqms_state.sqlite3).

What is saved (after every successful POST/PUT/DELETE, see main.py) and restored at startup:
- documents made through the app (bills, returns, expenses, ...): those in store.DOCS with no "origin";
- stock movements made through the app, MOVEMENTS[APP_MOVEMENTS_FROM:], replayed onto item QOH;
- master rows, users (password hashes) and roles changed through the app (store.EDITS / store.touch),
  laid over the seed or SQL Server data, plus Global Settings, cash counts, company edits and the
  document number series.

- the queue of writes waiting to go to SQL Server (store.OUTBOX, app/writeback.py).

This file is the app's own record either way. The optional SQL Server write-back (DB_WRITE_ENABLED)
copies documents into separate Web_* tables; the desktop app does not see them.
The file holds password hashes and business data, so it is created owner-only (0600) and is gitignored.
VLQMS_STATE_DB (env) sets the file path; an empty value turns saving off (the tests do this).
"""
import json
import logging
import os
import sqlite3
import threading
from datetime import datetime
from pathlib import Path

from . import store

logger = logging.getLogger("vlqms.persistence")

DEFAULT_PATH = Path(__file__).resolve().parents[1] / "data" / "vlqms_state.sqlite3"
PATH = os.environ.get("VLQMS_STATE_DB", str(DEFAULT_PATH))

# Tables whose rows the app can change. COMPANY is a single dict (key "*").
TABLES = ("ITEMS", "ITEM_GROUPS", "UOMS", "PACKAGES", "BRANDS", "SUPPLIERS", "CUSTOMERS", "LEDGER_GROUPS",
          "LEDGERS", "COUNTERS", "USERS", "ROLES", "ROLE_RIGHTS")
# Never written to the file: a legacy DB user's reversible password is re-read from MstUser instead.
SECRET_FIELDS = ("legacy_password", "default_password")

_lock = threading.Lock()


def enabled() -> bool:
    return bool(PATH)


def _connect() -> sqlite3.Connection:
    path = Path(PATH)
    path.parent.mkdir(parents=True, exist_ok=True)
    new = not path.exists()
    conn = sqlite3.connect(path)
    if new:
        os.chmod(path, 0o600)
    conn.execute("CREATE TABLE IF NOT EXISTS state (id INTEGER PRIMARY KEY CHECK (id = 1), saved_at TEXT, data TEXT)")
    return conn


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
        "outbox": store.OUTBOX,
    }


def save() -> None:
    if not enabled():
        return
    with _lock:
        for attempt in range(3):   # the background DB loader may be changing DOCS at the same moment
            try:
                data = json.dumps(snapshot(), default=str)
                break
            except RuntimeError:
                if attempt == 2:
                    raise
        with _connect() as conn:
            conn.execute("INSERT OR REPLACE INTO state (id, saved_at, data) VALUES (1, ?, ?)",
                         (datetime.now().isoformat(timespec="seconds"), data))


def load() -> bool:
    """Lay the saved app data over the freshly seeded/loaded store. Returns True if anything was restored."""
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)
    if not enabled() or not Path(PATH).exists():
        return False
    with _lock, _connect() as conn:
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
    store.OUTBOX[:] = data.get("outbox", [])

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
