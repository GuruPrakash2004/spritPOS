"""Write-back of the web app's documents and stock changes to SQL Server, into separate Web_* tables.

The legacy tables (TrnHdrSINV, TrnDtlSINV, TrnQOH, MstItem, ...) are never written: see
migrations/001_web_tables.sql for the Web_ tables and tools/migrate.py to create them.

How a save reaches SQL Server:
1. A transaction endpoint calls changed(kind, doc, action) after it has checked and saved the document
   in the in-memory store (routers/transactions.py, masters.py for opening stock). Nothing is queued yet.
2. When the request succeeds, the write middleware in main.py calls commit_request(): each change becomes
   an event (unique id, user, time, the document and the stock moves the request made for it) in the
   store.OUTBOX buffer, and persistence.save() writes the app state AND those events into the SQLite
   outbox table (web_outbox) in one SQLite transaction, as PENDING. This happens whether or not
   DB_WRITE_ENABLED is on, so bills made with it off can be sent later. A failed request queues nothing.
3. sync_pending_data() (background thread every RETRY_SECONDS when DB_WRITE_ENABLED is on, at startup,
   and POST /admin/writeback/sync) sends PENDING/FAILED rows in order. Per row:
   claim (PENDING/FAILED -> SYNCING, atomic) -> ONE SQL Server transaction (header, lines, stock moves,
   Web_StockBalance QtyChange = QtyChange + ?, Web_WriteLog row) -> mark SYNCED only after the commit
   returned. An event whose id is already in Web_WriteLog is not written again but marked SYNCED, which
   covers a crash between the SQL commit and the SQLite update. On an error the SQL transaction rolls back,
   the row becomes FAILED with the error, and sending stops (later documents may depend on it) until the
   next try. Rows are never deleted.

Not queued at all: bills made while the backend runs on seed data (masters_from_db False), and documents
loaded from SQL Server (they carry "origin"). Documents saved before the outbox existed can be queued by an
Administrator with backfill() (GET /admin/writeback/backfill previews, POST queues).

All SQL uses ? parameters (db.TxCursor), never string formatting. Logs carry event ids, document numbers and
users, never payloads, passwords, tokens or connection settings.
"""
import json
import logging
import os
import threading
import time
import uuid
from datetime import datetime

from . import db, persistence, store

logger = logging.getLogger("vlqms.writeback")

ENABLED = os.environ.get("DB_WRITE_ENABLED", "").strip().lower() in ("1", "true", "yes", "on")
SCHEMA_VERSION = 1          # the migration this code needs (migrations/001_web_tables.sql)
RETRY_SECONDS = 15
STALE_SECONDS = 300         # a row SYNCING for this long was left by a crashed worker: send it again

# Legacy table holding each document kind's numbers, to warn when a web number is already used there.
LEGACY_NUMBERS = {"sales_invoice": ("TrnHdrSINV", "SINVNO")}

_pending: list[dict] = []   # changes made by the request in progress (writes run one at a time)
_wake = threading.Event()
_flush_lock = threading.Lock()   # one sync run at a time in this process (claims guard across processes)
_thread: threading.Thread | None = None
# Set by main.py when the masters were loaded from SQL Server at startup. Without that the app runs on
# seed data, whose sample items and bills must never reach the real database.
masters_from_db = False
status = {"last_error": None, "last_error_at": None, "last_sync_at": None, "written": 0}


def changed(kind: str, doc: dict, action: str = "save", moves_ref: str | None = None, item: str | None = None,
            moves: list[dict] | None = None) -> None:
    """Note that `doc` was created or changed by this request. Queued only if the request succeeds.
    `moves_ref` is the doc_no this request's stock moves carry, when it differs from the document's
    (an item's opening stock is logged as "OPENING"); `item` keeps only that item's moves. `moves` gives the
    event's stock moves explicitly instead (Utilities → Merge Items moves an item's web stock change to
    another item without adding to store.MOVEMENTS).

    Queued whether or not DB_WRITE_ENABLED is on. Not queued:
    - while the backend runs on seed data: those bills use sample items and numbers and must never reach
      the real database, even after a later restart that connects;
    - for bills loaded from SQL Server (they carry "origin"), e.g. a desktop bill cancelled on the web:
      the Web_ tables hold web documents only, and the legacy bill itself is not changed."""
    if not masters_from_db:
        doc["seed_mode"] = True   # made on sample data: backfill will never queue it
        return
    if "origin" in doc:
        logger.warning("Not queued for SQL Server: %s %s was loaded from the desktop database (%s)",
                       kind, doc.get("doc_no"), action)
        return
    doc_no = doc.get("doc_no") or doc.get("date")
    _pending.append({"kind": kind, "doc_no": doc_no, "doc": doc, "action": action, "moves_ref": moves_ref or doc_no,
                     "item": item, "moves": moves})


def begin_request() -> int:
    """Start of a write request: forget leftovers and return where this request's stock moves start."""
    _pending.clear()
    return len(store.MOVEMENTS)


def _event(kind: str, doc_no: str, action: str, user: str | None, doc: dict, moves: list[dict]) -> dict:
    return {"id": uuid.uuid4().hex, "kind": kind, "doc_no": doc_no, "action": action, "user": user,
            "at": datetime.now().isoformat(timespec="seconds"), "doc": json.loads(json.dumps(doc, default=str)),
            "moves": [dict(m) for m in moves]}


def commit_request(moves_from: int, user: str | None) -> None:
    """The request succeeded: buffer its changes, each with the stock moves it made for that document.
    persistence.save(), called next by the middleware, writes them to the outbox table with the state."""
    if not _pending:
        return
    moves = store.MOVEMENTS[moves_from:]
    for p in _pending:
        own = p.get("moves")
        store.OUTBOX.append(_event(p["kind"], p["doc_no"], p["action"], user, p["doc"],
                                   own if own is not None else
                                   [m for m in moves if m["doc_no"] == p["moves_ref"]
                                    and (p["item"] is None or m["item"] == p["item"])]))
        logger.info("Queued %s %s %s by %s", p["action"], p["kind"], p["doc_no"], user or "?")
    _pending.clear()
    if ENABLED:
        _wake.set()


def discard_request() -> None:
    _pending.clear()


# ─── Writing one event to SQL Server ─────────────────────────────────────────

def _num(v):
    return None if v is None else round(float(v), 3)


def _header(event: dict) -> tuple:
    doc = event["doc"]
    totals = doc.get("totals") or {}
    net = totals.get("net", doc.get("total", doc.get("total_amount")))
    party = doc.get("customer") or doc.get("supplier")
    return (str(doc.get("date") or event["at"])[:10], doc.get("status"), party, doc.get("counter"), _num(net))


def _legacy_clash(cur, event: dict) -> bool:
    legacy = LEGACY_NUMBERS.get(event["kind"])
    if not legacy:
        return False
    table, column = legacy   # constants above, never user input
    cur.execute(f"SELECT TOP 1 1 FROM dbo.{table} WITH (NOLOCK) WHERE {column} = ?", (event["doc_no"],))
    return cur.fetchone() is not None


def apply_event(cur, event: dict) -> bool:
    """Write one event inside an open transaction. Returns False if it was already written (its EventId is
    in Web_WriteLog, the idempotency key), in which case nothing is written."""
    cur.execute("SELECT 1 FROM dbo.Web_WriteLog WHERE EventId = ?", (event["id"],))
    if cur.fetchone() is not None:
        return False
    kind, doc_no, doc = event["kind"], event["doc_no"], event["doc"]
    doc_date, doc_status, party, counter, net = _header(event)
    clash = _legacy_clash(cur, event)
    if clash:
        logger.warning("%s %s: the same number is already used by a desktop bill", kind, doc_no)
    body = json.dumps(doc, default=str)
    updated = cur.execute(
        "UPDATE dbo.Web_DocHeader SET DocDate = ?, Status = ?, PartyCode = ?, CounterCode = ?, NetAmount = ?, "
        "LegacyNoClash = ?, DocJson = ?, UpdatedAt = SYSUTCDATETIME() WHERE DocKind = ? AND DocNo = ?",
        (doc_date, doc_status, party, counter, net, int(clash), body, kind, doc_no))
    if not updated:
        cur.execute(
            "INSERT INTO dbo.Web_DocHeader (DocKind, DocNo, DocDate, Status, PartyCode, CounterCode, NetAmount, "
            "LegacyNoClash, DocJson, CreatedBy) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            (kind, doc_no, doc_date, doc_status, party, counter, net, int(clash), body, event["user"]))
    cur.execute("DELETE FROM dbo.Web_DocLine WHERE DocKind = ? AND DocNo = ?", (kind, doc_no))
    for n, line in enumerate(doc.get("lines") or [], start=1):
        if line.get("item_code") is None:
            continue   # expense lines are ledgers, not items; they stay in DocJson
        qty = line.get("qty", line.get("closing_qty", 0))
        cur.execute("INSERT INTO dbo.Web_DocLine (DocKind, DocNo, LineNum, ItemCode, Qty, Rate, Amount) "
                    "VALUES (?, ?, ?, ?, ?, ?, ?)",
                    (kind, doc_no, n, line["item_code"], _num(qty), _num(line.get("rate", line.get("mrp"))),
                     _num(line.get("amount"))))
    for m in event["moves"]:
        cur.execute("INSERT INTO dbo.Web_StockMove (EventId, ItemCode, MoveDate, Qty, MoveKind, DocNo, Rate) "
                    "VALUES (?, ?, ?, ?, ?, ?, ?)",
                    (event["id"], m["item"], m["date"], _num(m["qty"]), m["kind"], m["doc_no"], _num(m.get("rate"))))
        # Relative update: stays right if two writers change the same item at once.
        if not cur.execute("UPDATE dbo.Web_StockBalance SET QtyChange = QtyChange + ?, UpdatedAt = SYSUTCDATETIME() "
                           "WHERE ItemCode = ?", (_num(m["qty"]), m["item"])):
            cur.execute("INSERT INTO dbo.Web_StockBalance (ItemCode, QtyChange) VALUES (?, ?)", (m["item"], _num(m["qty"])))
    cur.execute("INSERT INTO dbo.Web_WriteLog (EventId, DocKind, DocNo, Action, UserCode, RequestedAt) "
                "VALUES (?, ?, ?, ?, ?, ?)", (event["id"], kind, doc_no, event["action"], event["user"], event["at"]))
    return True


def _check_schema() -> None:
    row = db.fetchone("SELECT MAX(Version) AS v FROM dbo.Web_SchemaVersion") if db.fetchone(
        "SELECT 1 AS x FROM sys.tables WHERE name = 'Web_SchemaVersion'") else None
    if not row or (row["v"] or 0) < SCHEMA_VERSION:
        raise RuntimeError(f"The Web_ tables are missing or old: run tools/migrate.py up (needs version {SCHEMA_VERSION})")


# ─── Sync ─────────────────────────────────────────────────────────────────────

def _safe_error(exc: Exception) -> str:
    """Error text for the outbox, status and logs. Driver messages don't carry the connection settings
    (they live only in db.py), and the payload is never included."""
    return f"{type(exc).__name__}: {str(exc)[:200]}"


def _record_error(exc: Exception) -> None:
    status.update(last_error=_safe_error(exc), last_error_at=datetime.now().isoformat(timespec="seconds"))


def _looks_disconnected(exc: Exception) -> bool:
    text = str(exc).lower()
    return any(w in text for w in ("connection", "network", "timeout", "timed out", "broken pipe", "20047", "08s01"))


def sync_pending_data(include_failed: bool = True) -> dict:
    """Send PENDING (and FAILED) outbox rows to SQL Server, oldest first. A row is marked SYNCED only after
    SQL Server committed it or already had it. Returns what happened in this run."""
    result = {"sent": 0, "already_there": 0, "failed": 0, "busy": False, "skipped": None}
    if not ENABLED:
        result["skipped"] = "Write-back is off (DB_WRITE_ENABLED=false): transactions stay PENDING in SQLite"
        return result
    if not _flush_lock.acquire(blocking=False):
        result["busy"] = True   # another sync is running in this process
        return result
    try:
        try:
            if not masters_from_db:
                raise RuntimeError("The backend started without SQL Server (seed data): restart it while connected")
            if not db.is_db_available() and not db.reconnect():
                raise RuntimeError("SQL Server is not reachable")
            _check_schema()
        except Exception as exc:   # nothing claimed: rows stay PENDING/FAILED
            _record_error(exc)
            result["skipped"] = status["last_error"]
            logger.error("Write-back sync not started: %s", status["last_error"])
            return result

        persistence.reset_stale(STALE_SECONDS)
        try:
            for row in persistence.pending_rows(include_failed):
                if not persistence.claim(row["id"]):
                    continue   # another worker has it
                event, attempt, started = row["event"], row["attempts"] + 1, time.monotonic()
                try:
                    with db.transaction() as cur:
                        written = apply_event(cur, event)
                except Exception as exc:   # rolled back in SQL Server; keep the row for a retry
                    persistence.mark_failed(row["id"], _safe_error(exc))
                    result["failed"] += 1
                    _record_error(exc)
                    logger.error("Sync FAILED %s %s %s (event %s, attempt %d, %.0f ms): %s", event["action"],
                                 event["kind"], event["doc_no"], event["id"], attempt,
                                 (time.monotonic() - started) * 1000, status["last_error"])
                    if db.is_db_available() and _looks_disconnected(exc):
                        db.reconnect()
                    break   # keep the order: later documents may depend on this one
                persistence.mark_synced(row["id"])   # only now: SQL Server has committed (or already had) it
                result["sent" if written else "already_there"] += 1
                status["written"] += 1
                logger.info("Synced %s %s %s (event %s, attempt %d, %.0f ms)%s", event["action"], event["kind"],
                            event["doc_no"], event["id"], attempt, (time.monotonic() - started) * 1000,
                            "" if written else ": already in SQL Server")
            else:
                status.update(last_error=None, last_error_at=None)
        except Exception as exc:   # e.g. the SQLite update failed after a commit: the row stays SYNCING
            _record_error(exc)       # and is picked up again later, when SQL Server reports it as written
            logger.error("Write-back sync stopped: %s", status["last_error"])
        status["last_sync_at"] = datetime.now().isoformat(timespec="seconds")
        return result
    finally:
        _flush_lock.release()


def flush() -> int:
    """Older name: run a sync and return how many rows became SYNCED."""
    r = sync_pending_data()
    return r["sent"] + r["already_there"]


def retry(ids: list[str] | None = None) -> dict:
    """FAILED -> PENDING (all, or the given ids), then sync. Safe to repeat: written events are skipped."""
    reset = persistence.retry_failed(ids)
    return {"reset": reset, **sync_pending_data()}


# ─── Backfill (documents saved before the outbox existed) ─────────────────────

class BackfillRefused(Exception):
    """Backfill can't run now; the message says why (shown to the Administrator as 409)."""


def _written_documents() -> set[tuple[str, str]]:
    """(DocKind, DocNo) already in SQL Server's Web_WriteLog. Needs the connection: documents the previous
    version sent left no outbox row, and sending them again under a new event id would count their stock twice."""
    if not db.is_db_available():
        raise BackfillRefused("Connect to SQL Server first: backfill must skip documents already written there")
    if not db.fetchone("SELECT 1 AS x FROM sys.tables WHERE name = 'Web_WriteLog'"):
        return set()   # tables not created yet: nothing can have been written
    return {(r["DocKind"], r["DocNo"]) for r in db.fetchall("SELECT DISTINCT DocKind, DocNo FROM dbo.Web_WriteLog")}


def backfill_candidates() -> list[dict]:
    """Web documents in SQLite with no outbox row and not already in SQL Server: bills, returns, expenses,
    closing stock, cash counts and opening stock made in the app. Each as the event that would be queued,
    oldest first, with a `blocked` reason when it must not be sent (made on seed data, unknown items)."""
    if not masters_from_db:
        raise BackfillRefused("The backend started without SQL Server (seed data): backfill needs the real masters")
    queued = persistence.queued_documents() | _written_documents()
    app_moves = store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]
    found: list[tuple[str, str, dict, list[dict]]] = []
    for kind, docs in store.DOCS.items():
        for no, doc in list(docs.items()):
            if "origin" in doc or doc.get("status") == "HOLD" or (kind, no) in queued:
                continue
            found.append((kind, no, doc, [m for m in app_moves if m["doc_no"] == no]))
    for day, doc in store.DENOMINATIONS.items():
        if ("denomination", day) not in queued:
            found.append(("denomination", day, doc, []))
    for m in app_moves:
        # Numbered by the item it was opened for, even after that item was merged into another.
        no = f"OPENING/{m.get('merged_from', m['item'])}"
        if m["kind"] == "opening" and ("opening_stock", no) not in queued:
            found.append(("opening_stock", no, {"doc_no": no, "date": m["date"],
                                                "lines": [{"item_code": m["item"], "qty": m["qty"], "rate": m.get("rate")}]},
                          [m]))
    found.sort(key=lambda f: (str(f[2].get("date", "")), str(f[2].get("time", "")), f[1]))
    out = []
    for kind, no, doc, moves in found:
        unknown = sorted({l["item_code"] for l in doc.get("lines") or []
                          if l.get("item_code") and l["item_code"] not in store.ITEMS})
        blocked = ("made while the backend ran on seed data" if doc.get("seed_mode")
                   else f"items not in the loaded masters: {', '.join(unknown)}" if unknown else None)
        out.append({"kind": kind, "doc_no": no, "doc": doc, "moves": moves, "blocked": blocked})
    return out


def backfill(user: str, documents: list[tuple[str, str]] | None = None) -> dict:
    """Without `documents`: a preview of what could be queued. With them ((kind, doc_no) pairs picked from the
    preview): queue exactly those as PENDING. A blocked document (seed data, unknown items) is refused."""
    candidates = backfill_candidates()
    preview = [{"kind": c["kind"], "doc_no": c["doc_no"], "date": c["doc"].get("date"),
                "status": c["doc"].get("status"), "moves": len(c["moves"]), "blocked": c["blocked"]}
               for c in candidates]
    queued = 0
    if documents is not None:
        by_key = {(c["kind"], c["doc_no"]): c for c in candidates}
        missing = [f"{k} {n}" for k, n in documents if (k, n) not in by_key]
        if missing:
            raise BackfillRefused(f"Not in the backfill list (already queued or written?): {', '.join(missing)}")
        blocked = [f"{k} {n} ({by_key[(k, n)]['blocked']})" for k, n in documents if by_key[(k, n)]["blocked"]]
        if blocked:
            raise BackfillRefused(f"These must not be sent to SQL Server: {'; '.join(blocked)}")
        picked = [by_key[key] for key in dict.fromkeys(documents)]
        queued = persistence.add_events([_event(c["kind"], c["doc_no"], "backfill", user, c["doc"], c["moves"])
                                         for c in picked])
        logger.info("Backfill by %s: queued %d document(s) as PENDING", user, queued)
        if ENABLED:
            _wake.set()
    return {"documents": preview, "count": len(preview), "queued": queued}


# ─── Background sender and status ─────────────────────────────────────────────

def _run() -> None:
    while True:
        try:
            sync_pending_data()
        except Exception:
            logger.exception("Write-back thread error")
        _wake.wait(RETRY_SECONDS)
        _wake.clear()
        time.sleep(0.2)


def start() -> None:
    """At startup (main.py, after persistence.load()): put back rows a crash left SYNCING, then, when
    DB_WRITE_ENABLED is on, start the background sender, which syncs straight away and then every
    RETRY_SECONDS or whenever a write is queued."""
    global _thread
    try:
        stale = persistence.reset_stale()
        if stale:
            logger.warning("%d outbox row(s) were left SYNCING by a previous run: back to PENDING", stale)
        counts = persistence.outbox_counts()
    except Exception:
        logger.exception("Could not read the outbox")
        counts = {"pending": 0, "failed": 0}
    if not ENABLED:
        logger.info("SQL Server write-back is off (DB_WRITE_ENABLED): %d transaction(s) PENDING in SQLite",
                    counts["pending"] + counts["failed"])
        return
    if not masters_from_db:
        status["last_error"] = "Not writing: the backend started without SQL Server (seed data). Restart it while connected."
        logger.warning(status["last_error"])
    if _thread is None:
        _thread = threading.Thread(target=_run, name="writeback", daemon=True)
        _thread.start()
        logger.info("SQL Server write-back is ON: %d transaction(s) to send", counts["pending"] + counts["failed"])


def state() -> dict:
    return {"enabled": ENABLED, "db_connected": db.is_db_available(), "masters_from_db": masters_from_db,
            **persistence.outbox_counts(), **status}
