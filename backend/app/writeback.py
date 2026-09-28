"""Write-back of the web app's documents and stock changes to SQL Server, into separate Web_* tables.

Off unless DB_WRITE_ENABLED=true in backend/.env. The legacy tables (TrnHdrSINV, TrnQOH, MstItem, ...)
are never written: see migrations/001_web_tables.sql for the tables and tools/migrate.py to create them.

How a save reaches SQL Server:
1. A transaction endpoint calls changed(kind, doc, action) after it has checked and saved the document
   in the in-memory store (routers/transactions.py). Nothing is sent yet.
2. When the request succeeds, the write middleware in main.py calls commit_request(): each change
   becomes an event in store.OUTBOX with a unique id, the user, the time and the stock movements that
   request made for that document. The outbox is saved in the local SQLite state with everything else
   (app/persistence.py), so a queued write survives a restart. A failed request queues nothing.
3. A background thread (start()) sends the outbox in order. Each event is ONE SQL transaction: header,
   lines, stock moves, the stock balance (QtyChange = QtyChange + ?) and the Web_WriteLog row commit
   together or roll back together. An event whose id is already in Web_WriteLog is skipped, so a retry
   after a network drop never writes twice. On any error the event stays queued, the error is logged and
   shown on GET /admin/writeback, and the thread retries later (reconnecting if the connection dropped).

All SQL uses ? parameters (db.TxCursor), never string formatting.
"""
import json
import logging
import os
import threading
import time
import uuid
from datetime import datetime

from . import db, store

logger = logging.getLogger("vlqms.writeback")

ENABLED = os.environ.get("DB_WRITE_ENABLED", "").strip().lower() in ("1", "true", "yes", "on")
SCHEMA_VERSION = 1          # the migration this code needs (migrations/001_web_tables.sql)
RETRY_SECONDS = 15

# Legacy table holding each document kind's numbers, to warn when a web number is already used there.
LEGACY_NUMBERS = {"sales_invoice": ("TrnHdrSINV", "SINVNO")}

_pending: list[dict] = []   # changes made by the request in progress (writes run one at a time)
_wake = threading.Event()
_flush_lock = threading.Lock()
_thread: threading.Thread | None = None
# Set by main.py when the masters were loaded from SQL Server at startup. Without that the app runs on
# seed data, whose sample items and bills must never reach the real database.
masters_from_db = False
status = {"last_error": None, "last_error_at": None, "last_sync_at": None, "written": 0}


def changed(kind: str, doc: dict, action: str = "save", moves_ref: str | None = None, item: str | None = None) -> None:
    """Note that `doc` was created or changed by this request. Sent only if the request succeeds.
    `moves_ref` is the doc_no this request's stock moves carry, when it differs from the document's
    (an item's opening stock is logged as "OPENING"); `item` keeps only that item's moves.

    Not queued:
    - while the backend runs on seed data: those bills use sample items and numbers and must never reach
      the real database, even after a later restart that connects;
    - for bills loaded from SQL Server (they carry "origin"), e.g. a desktop bill cancelled on the web:
      the Web_ tables hold web documents only, and the legacy bill itself is not changed."""
    if not (ENABLED and masters_from_db):
        return
    if "origin" in doc:
        logger.warning("Not written to SQL Server: %s %s was loaded from the desktop database (%s)",
                       kind, doc.get("doc_no"), action)
        return
    doc_no = doc.get("doc_no") or doc.get("date")
    _pending.append({"kind": kind, "doc_no": doc_no, "doc": doc, "action": action, "moves_ref": moves_ref or doc_no,
                     "item": item})


def begin_request() -> int:
    """Start of a write request: forget leftovers and return where this request's stock moves start."""
    _pending.clear()
    return len(store.MOVEMENTS)


def commit_request(moves_from: int, user: str | None) -> None:
    """The request succeeded: queue its changes, each with the stock moves it made for that document."""
    if not _pending:
        return
    moves = store.MOVEMENTS[moves_from:]
    now = datetime.now().isoformat(timespec="seconds")
    for p in _pending:
        store.OUTBOX.append({
            "id": uuid.uuid4().hex, "kind": p["kind"], "doc_no": p["doc_no"], "action": p["action"],
            "user": user, "at": now, "doc": json.loads(json.dumps(p["doc"], default=str)),
            "moves": [dict(m) for m in moves
                      if m["doc_no"] == p["moves_ref"] and (p["item"] is None or m["item"] == p["item"])],
        })
        logger.info("Queued %s %s %s by %s", p["action"], p["kind"], p["doc_no"], user or "?")
    _pending.clear()
    _wake.set()


def discard_request() -> None:
    _pending.clear()


# ─── Sending ────────────────────────────────────────────────────────────────

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
    """Write one event inside an open transaction. Returns False if it was already written."""
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


def flush() -> int:
    """Send queued events in order until the queue is empty or one fails. Returns how many were sent."""
    sent = 0
    with _flush_lock:
        if not store.OUTBOX:
            return 0
        try:
            if not masters_from_db:
                raise RuntimeError("The backend started without SQL Server (seed data): restart it while connected")
            if not db.is_db_available() and not db.reconnect():
                raise RuntimeError("SQL Server is not reachable")
            _check_schema()
            while store.OUTBOX:
                event = store.OUTBOX[0]
                with db.transaction() as cur:
                    written = apply_event(cur, event)
                store.OUTBOX.pop(0)
                sent += 1
                status["written"] += 1
                logger.info("%s %s %s %s by %s", "Wrote" if written else "Already written:", event["action"],
                            event["kind"], event["doc_no"], event["user"] or "?")
            status.update(last_error=None, last_error_at=None, last_sync_at=datetime.now().isoformat(timespec="seconds"))
        except Exception as exc:   # keep the event queued; never lose it
            status.update(last_error=str(exc)[:300], last_error_at=datetime.now().isoformat(timespec="seconds"))
            logger.error("Write-back paused, %d event(s) queued: %s", len(store.OUTBOX), exc)
            if db.is_db_available() and _looks_disconnected(exc):
                db.reconnect()
    return sent


def _looks_disconnected(exc: Exception) -> bool:
    text = str(exc).lower()
    return any(w in text for w in ("connection", "network", "timeout", "timed out", "broken pipe", "20047", "08s01"))


def _run() -> None:
    while True:
        _wake.wait(RETRY_SECONDS)
        _wake.clear()
        try:
            flush()
        except Exception:
            logger.exception("Write-back thread error")
        time.sleep(0.2)


def start() -> None:
    """Start the background sender (main.py startup). Does nothing when the flag is off."""
    global _thread
    if not ENABLED:
        logger.info("SQL Server write-back is off (DB_WRITE_ENABLED)")
        return
    if not masters_from_db:
        status["last_error"] = "Not writing: the backend started without SQL Server (seed data). Restart it while connected."
        logger.warning(status["last_error"])
    if _thread is None:
        _thread = threading.Thread(target=_run, name="writeback", daemon=True)
        _thread.start()
        logger.info("SQL Server write-back is ON: %d event(s) queued", len(store.OUTBOX))
        _wake.set()


def state() -> dict:
    return {"enabled": ENABLED, "db_connected": db.is_db_available(), "queued": len(store.OUTBOX),
            "oldest_queued_at": store.OUTBOX[0]["at"] if store.OUTBOX else None, **status}
