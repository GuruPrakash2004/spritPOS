"""SQLite outbox -> SQL Server write-back (app/writeback.py, app/persistence.py web_outbox).

SQL Server is played by an in-memory SQLite database with the Web_ tables: it gives real transactions,
rowcounts and primary keys, so commit/rollback, idempotency and the relative stock update are really
exercised. The T-SQL-only bits (dbo., SYSUTCDATETIME(), WITH (NOLOCK), TOP) are translated. The same flow
against a real SQL Server copy is in test_writeback_sqlserver.py.

The outbox itself is the real code: an in-memory SQLite database (saving is off in the tests), or a temp
state file where a test needs the file.
"""
import json
import re
import sqlite3
import threading
from datetime import date
from pathlib import Path

import pytest

from app import db, persistence, store, writeback

TODAY = date.today().isoformat()
MIGRATIONS = Path(__file__).resolve().parents[1] / "migrations"

SCHEMA = """
CREATE TABLE Web_SchemaVersion (Version INT PRIMARY KEY, AppliedAt TEXT DEFAULT CURRENT_TIMESTAMP);
CREATE TABLE Web_DocHeader (DocKind TEXT NOT NULL, DocNo TEXT NOT NULL, DocDate TEXT NOT NULL, Status TEXT,
    PartyCode TEXT, CounterCode TEXT, NetAmount REAL, LegacyNoClash INT NOT NULL DEFAULT 0, DocJson TEXT NOT NULL,
    CreatedBy TEXT, CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP, UpdatedAt TEXT DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (DocKind, DocNo));
CREATE TABLE Web_DocLine (DocKind TEXT, DocNo TEXT, LineNum INT, ItemCode TEXT NOT NULL, Qty REAL NOT NULL,
    Rate REAL, Amount REAL, PRIMARY KEY (DocKind, DocNo, LineNum));
CREATE TABLE Web_StockMove (MoveId INTEGER PRIMARY KEY AUTOINCREMENT, EventId TEXT NOT NULL, ItemCode TEXT NOT NULL,
    MoveDate TEXT NOT NULL, Qty REAL NOT NULL, MoveKind TEXT NOT NULL, DocNo TEXT NOT NULL, Rate REAL,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP);
CREATE TABLE Web_StockBalance (ItemCode TEXT PRIMARY KEY, QtyChange REAL NOT NULL DEFAULT 0, UpdatedAt TEXT);
CREATE TABLE Web_WriteLog (EventId TEXT PRIMARY KEY, DocKind TEXT NOT NULL, DocNo TEXT NOT NULL, Action TEXT NOT NULL,
    UserCode TEXT, RequestedAt TEXT NOT NULL, AppliedAt TEXT DEFAULT CURRENT_TIMESTAMP);
CREATE TABLE TrnHdrSINV (SINVNO TEXT);
INSERT INTO Web_SchemaVersion (Version) VALUES (1);
"""


def _tsql_to_sqlite(sql: str) -> str:
    sql = sql.replace("dbo.", "").replace("SYSUTCDATETIME()", "CURRENT_TIMESTAMP").replace(" WITH (NOLOCK)", "")
    return sql.replace("SELECT TOP 1 1", "SELECT 1").replace("sys.tables", "sqlite_master")


class FakeCursor:
    def __init__(self, conn):
        self._cur = conn.sqlite.cursor()
        self.conn = conn

    def execute(self, sql, params=()):
        fail = self.conn.fail_on
        if fail and (fail in sql or any(fail == str(p) for p in params)):
            # Worded so it isn't taken for a lost connection (that path reconnects; see test_3b).
            raise RuntimeError("simulated failure in the middle of the save")
        self._cur.execute(_tsql_to_sqlite(sql), params)

    def __getattr__(self, name):
        return getattr(self._cur, name)


class FakeConn:
    """Stands in for the shared pymssql/pyodbc connection. fail_on: SQL text or a parameter value that
    makes a statement fail (a network drop in the middle of a save)."""

    def __init__(self):
        self.sqlite = sqlite3.connect(":memory:", check_same_thread=False)
        self.sqlite.executescript(SCHEMA)
        self.fail_on = None

    def cursor(self):
        return FakeCursor(self)

    def commit(self):
        self.sqlite.commit()

    def rollback(self):
        self.sqlite.rollback()

    def rows(self, sql, params=()):
        return self.sqlite.execute(sql, params).fetchall()

    def count(self, table):
        return self.rows(f"SELECT COUNT(*) FROM {table}")[0][0]


@pytest.fixture
def sqlserver(monkeypatch):
    """The fake SQL Server connected, masters 'loaded from the DB', write-back switched ON."""
    conn = FakeConn()
    monkeypatch.setattr(db, "_conn", conn)
    monkeypatch.setattr(db, "_db_available", True)
    monkeypatch.setattr(db, "_driver_used", "test")
    monkeypatch.setattr(writeback, "ENABLED", True)
    monkeypatch.setattr(writeback, "masters_from_db", True)
    monkeypatch.setattr(writeback, "status", {"last_error": None, "last_error_at": None, "last_sync_at": None, "written": 0})
    return conn


@pytest.fixture
def flag_off(sqlserver, monkeypatch):
    """Same, but DB_WRITE_ENABLED=false."""
    monkeypatch.setattr(writeback, "ENABLED", False)
    return sqlserver


@pytest.fixture
def state_file(tmp_path, monkeypatch):
    path = tmp_path / "state.sqlite3"
    monkeypatch.setattr(persistence, "PATH", str(path))
    return path


def sale(item="VIN-750", bottles=2, **extra):
    return {"date": TODAY, "counter": "counter10", "salesperson": "GURU",
            "lines": [{"item_code": item, "bottles": bottles}], **extra}


def outbox(status=None):
    """Outbox rows, oldest first."""
    return list(reversed(persistence.outbox_rows(status, limit=500)))


def statuses():
    return [r["sync_status"] for r in outbox()]


# ─── The seven required scenarios ─────────────────────────────────────────────

async def test_1_flag_off_bill_is_saved_in_sqlite_and_pending(client, flag_off, state_file):
    doc = (await client.post("/transactions/sales-invoice", json=sale(bottles=3))).json()

    with sqlite3.connect(state_file) as conn:
        state = json.loads(conn.execute("SELECT data FROM state").fetchone()[0])
        row = conn.execute("SELECT transaction_type, transaction_id, sync_status, sync_attempts FROM web_outbox").fetchall()
    assert doc["doc_no"] in state["docs"]["sales_invoice"]
    assert row == [("sales_invoice", doc["doc_no"], "PENDING", 0)]
    assert flag_off.count("Web_DocHeader") == 0
    assert writeback.sync_pending_data()["skipped"].startswith("Write-back is off")
    assert statuses() == ["PENDING"]


async def test_2_turning_the_flag_on_sends_what_was_pending(client, flag_off, monkeypatch):
    doc = (await client.post("/transactions/sales-invoice", json=sale(bottles=3))).json()
    monkeypatch.setattr(writeback, "ENABLED", True)

    result = writeback.sync_pending_data()

    assert (result["sent"], result["failed"]) == (1, 0)
    assert statuses() == ["SYNCED"]
    assert outbox()[0]["synced_at"] and outbox()[0]["last_sync_error"] is None
    assert flag_off.rows("SELECT DocKind, DocNo, Status, CreatedBy FROM Web_DocHeader") == [
        ("sales_invoice", doc["doc_no"], "SAVED", "ADMIN")]
    assert flag_off.rows("SELECT ItemCode, Qty FROM Web_DocLine") == [("VIN-750", 3)]
    assert flag_off.rows("SELECT QtyChange FROM Web_StockBalance WHERE ItemCode = 'VIN-750'") == [(-3,)]
    assert flag_off.rows("SELECT Action, UserCode FROM Web_WriteLog") == [("save", "ADMIN")]


async def test_3_sql_server_failure_keeps_the_data_and_marks_failed(client, sqlserver, state_file):
    doc = (await client.post("/transactions/sales-invoice", json=sale(bottles=1))).json()
    sqlserver.fail_on = "INSERT INTO dbo.Web_WriteLog"   # last statement of the SQL transaction

    result = writeback.sync_pending_data()

    assert result["failed"] == 1
    row = outbox()[0]
    assert (row["sync_status"], row["sync_attempts"]) == ("FAILED", 1)
    assert "simulated failure" in row["last_sync_error"]
    for table in ("Web_DocHeader", "Web_DocLine", "Web_StockMove", "Web_StockBalance", "Web_WriteLog"):
        assert sqlserver.count(table) == 0, table   # rolled back, nothing half-written
    with sqlite3.connect(state_file) as conn:        # the bill itself is still in SQLite
        assert doc["doc_no"] in json.loads(conn.execute("SELECT data FROM state").fetchone()[0])["docs"]["sales_invoice"]

    sqlserver.fail_on = None
    assert writeback.sync_pending_data()["sent"] == 1
    assert (outbox()[0]["sync_status"], outbox()[0]["sync_attempts"]) == ("SYNCED", 2)


async def test_3b_server_unreachable_changes_no_rows(client, sqlserver, monkeypatch):
    await client.post("/transactions/sales-invoice", json=sale())
    monkeypatch.setattr(db, "_db_available", False)
    monkeypatch.setattr(db, "init_db", lambda: False)

    result = writeback.sync_pending_data()

    assert "not reachable" in result["skipped"]
    assert statuses() == ["PENDING"]
    assert outbox()[0]["sync_attempts"] == 0
    assert "not reachable" in writeback.state()["last_error"]


async def test_4_syncing_twice_writes_once(client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale(bottles=4))

    first, second = writeback.sync_pending_data(), writeback.sync_pending_data()

    assert (first["sent"], second["sent"], second["already_there"]) == (1, 0, 0)
    assert sqlserver.count("Web_DocHeader") == sqlserver.count("Web_WriteLog") == 1
    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance") == [(-4,)]


async def test_5_crash_after_sql_commit_is_detected_not_duplicated(client, sqlserver, monkeypatch):
    await client.post("/transactions/sales-invoice", json=sale(bottles=2))
    real_mark = persistence.mark_synced

    def crash(event_id):
        raise OSError("backend died before SQLite was updated")
    monkeypatch.setattr(persistence, "mark_synced", crash)

    writeback.sync_pending_data()

    assert sqlserver.count("Web_DocHeader") == 1   # SQL Server committed ...
    assert statuses() == ["SYNCING"]               # ... but SQLite never heard

    monkeypatch.setattr(persistence, "mark_synced", real_mark)
    assert persistence.reset_stale() == 1          # what start() does after a restart
    result = writeback.sync_pending_data()

    assert (result["sent"], result["already_there"]) == (0, 1)
    assert statuses() == ["SYNCED"]
    assert sqlserver.count("Web_DocHeader") == sqlserver.count("Web_WriteLog") == sqlserver.count("Web_StockMove") == 1
    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance") == [(-2,)]


async def test_6_many_pending_all_sync_in_order(client, flag_off, monkeypatch):
    docs = [(await client.post("/transactions/sales-invoice", json=sale(bottles=n))).json()["doc_no"] for n in (1, 2, 3)]
    await client.post(f"/transactions/sales-invoice/{docs[0]}/cancel", json={"remarks": "wrong item"})
    assert statuses() == ["PENDING"] * 4
    monkeypatch.setattr(writeback, "ENABLED", True)

    assert writeback.sync_pending_data()["sent"] == 4

    assert statuses() == ["SYNCED"] * 4
    assert flag_off.rows("SELECT QtyChange FROM Web_StockBalance WHERE ItemCode = 'VIN-750'") == [(-5,)]
    assert flag_off.rows("SELECT Status FROM Web_DocHeader WHERE DocNo = ?", (docs[0],)) == [("CANCELLED",)]


async def test_6b_a_failure_stops_later_rows_to_keep_the_order(client, sqlserver):
    docs = [(await client.post("/transactions/sales-invoice", json=sale(bottles=1))).json()["doc_no"] for _ in range(3)]
    sqlserver.fail_on = docs[1]   # any statement for the second bill fails

    result = writeback.sync_pending_data()

    assert (result["sent"], result["failed"]) == (1, 1)
    assert statuses() == ["SYNCED", "FAILED", "PENDING"]
    sqlserver.fail_on = None
    assert writeback.sync_pending_data()["sent"] == 2
    assert statuses() == ["SYNCED"] * 3


class _NoLock:
    """Stands in for the per-process sync lock, so the threads act like separate processes and only the
    outbox claim (one atomic UPDATE) and Web_WriteLog's primary key keep them apart."""
    def acquire(self, blocking=True):
        return True

    def release(self):
        pass


async def test_7_concurrent_syncs_write_each_bill_once(client, flag_off, monkeypatch):
    for n in range(1, 6):
        await client.post("/transactions/sales-invoice", json=sale(bottles=1))
    monkeypatch.setattr(writeback, "ENABLED", True)
    monkeypatch.setattr(writeback, "_flush_lock", _NoLock())
    results = []
    threads = [threading.Thread(target=lambda: results.append(writeback.sync_pending_data())) for _ in range(4)]
    for t in threads:
        t.start()
    for t in threads:
        t.join()

    assert sum(r["sent"] for r in results) == 5
    assert sum(r["failed"] for r in results) == 0
    assert statuses() == ["SYNCED"] * 5
    assert flag_off.count("Web_DocHeader") == flag_off.count("Web_WriteLog") == flag_off.count("Web_StockMove") == 5
    assert flag_off.rows("SELECT QtyChange FROM Web_StockBalance") == [(-5,)]


async def test_7b_only_one_worker_can_claim_a_row(client, flag_off):
    await client.post("/transactions/sales-invoice", json=sale())
    event_id = outbox()[0]["id"]

    claims = [persistence.claim(event_id), persistence.claim(event_id)]

    assert claims == [True, False]
    assert outbox()[0]["sync_attempts"] == 1


# ─── SQLite atomicity, upgrade, seed mode, backfill ─────────────────────────

async def test_bill_and_outbox_row_are_saved_together(client, flag_off, state_file, monkeypatch):
    real_insert = persistence._insert_events

    def broken(conn, events):
        if events:
            raise sqlite3.OperationalError("disk full")
        return 0
    monkeypatch.setattr(persistence, "_insert_events", broken)
    doc = (await client.post("/transactions/sales-invoice", json=sale())).json()

    with sqlite3.connect(state_file) as conn:   # neither the bill nor its row reached the file
        assert conn.execute("SELECT COUNT(*) FROM state").fetchone()[0] == 0
        assert conn.execute("SELECT COUNT(*) FROM web_outbox").fetchone()[0] == 0
    assert len(store.OUTBOX) == 1               # still buffered for the next save

    monkeypatch.setattr(persistence, "_insert_events", real_insert)
    await client.post("/transactions/sales-invoice", json=sale())

    with sqlite3.connect(state_file) as conn:
        assert doc["doc_no"] in json.loads(conn.execute("SELECT data FROM state").fetchone()[0])["docs"]["sales_invoice"]
        assert conn.execute("SELECT COUNT(*) FROM web_outbox WHERE sync_status = 'PENDING'").fetchone()[0] == 2
    assert store.OUTBOX == []


def test_a_queue_from_an_older_state_file_moves_into_the_table(flag_off):
    event = {"id": "a" * 32, "kind": "sales_invoice", "doc_no": "26/c10-1", "action": "save", "user": "ADMIN",
             "at": TODAY, "doc": {"date": TODAY}, "moves": []}

    persistence.apply({"outbox": [event]})
    persistence.apply({"outbox": [event]})   # twice: still one row

    assert [(r["id"], r["sync_status"]) for r in outbox()] == [("a" * 32, "PENDING")]


async def test_nothing_is_queued_while_running_on_seed_data(client, sqlserver, monkeypatch):
    monkeypatch.setattr(writeback, "masters_from_db", False)

    assert (await client.post("/transactions/sales-invoice", json=sale())).status_code == 201

    assert outbox() == []
    assert "seed data" in writeback.sync_pending_data()["skipped"]


async def test_the_process_lock_turns_a_second_sync_away(client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale())
    writeback._flush_lock.acquire()
    try:
        assert writeback.sync_pending_data()["busy"] is True
    finally:
        writeback._flush_lock.release()
    assert statuses() == ["PENDING"]


async def test_bills_loaded_from_the_desktop_db_are_not_queued(client, sqlserver):
    no = next(n for n, d in store.DOCS["sales_invoice"].items() if "origin" in d and d["status"] == "SAVED")

    assert (await client.post(f"/transactions/sales-invoice/{no}/cancel", json={"remarks": "test"})).status_code == 200

    assert outbox() == []


def _old_bill(no="26/c10-0000042", item="VIN-750", **extra):
    """A bill saved in SQLite before the outbox existed (so it has no outbox row)."""
    doc = {"doc_no": no, "date": TODAY, "status": "SAVED", "counter": "counter10", "customer": "CASH",
           "lines": [{"item_code": item, "qty": 1, "rate": 835, "amount": 835}], "totals": {"net": 835}, **extra}
    store.DOCS["sales_invoice"][no] = doc
    return doc


async def test_backfill_queues_chosen_older_documents_once_after_a_preview(client, cashier_client, flag_off):
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)    # as startup sets it: sample movements come before
    old = _old_bill()
    await client.post("/transactions/sales-invoice", json=sale())   # has a row already

    preview = (await client.get("/admin/writeback/backfill")).json()
    assert [(d["doc_no"], d["blocked"]) for d in preview["documents"]] == [(old["doc_no"], None)]
    assert preview["queued"] == 0 and len(outbox()) == 1

    pick = {"documents": [{"kind": "sales_invoice", "doc_no": old["doc_no"]}]}
    applied = await client.post("/admin/writeback/backfill", json=pick)
    again = await client.post("/admin/writeback/backfill", json=pick)

    assert applied.json()["queued"] == 1
    assert again.status_code == 409 and "Not in the backfill list" in again.json()["detail"]
    assert [(r["transaction_id"], r["action"], r["sync_status"]) for r in outbox()][1] == (old["doc_no"], "backfill", "PENDING")
    assert (await cashier_client.get("/admin/writeback/backfill")).status_code == 403
    assert (await cashier_client.post("/admin/writeback/backfill", json=pick)).status_code == 403


async def test_backfill_skips_documents_already_in_sql_server(client, flag_off):
    """The previous version sent bills without keeping an outbox row: sending them again under a new event id
    would count their stock twice."""
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)
    sent_before, never_sent = _old_bill("26/c10-0000050"), _old_bill("26/c10-0000051")
    flag_off.sqlite.execute("INSERT INTO Web_WriteLog (EventId, DocKind, DocNo, Action, RequestedAt) "
                            "VALUES ('old-event', 'sales_invoice', ?, 'save', ?)", (sent_before["doc_no"], TODAY))

    preview = (await client.get("/admin/writeback/backfill")).json()

    assert [d["doc_no"] for d in preview["documents"]] == [never_sent["doc_no"]]
    res = await client.post("/admin/writeback/backfill",
                            json={"documents": [{"kind": "sales_invoice", "doc_no": sent_before["doc_no"]}]})
    assert res.status_code == 409 and outbox() == []


async def test_backfill_refuses_seed_data_documents(client, flag_off, monkeypatch):
    store.APP_MOVEMENTS_FROM = len(store.MOVEMENTS)
    monkeypatch.setattr(writeback, "masters_from_db", False)          # a session on sample data ...
    seed_doc = (await client.post("/transactions/sales-invoice", json=sale())).json()
    assert seed_doc["seed_mode"] is True and outbox() == []
    monkeypatch.setattr(writeback, "masters_from_db", True)           # ... then a restart that connects
    unknown = _old_bill("26/c10-0000060", item="SAMPLE-ONLY")

    preview = {d["doc_no"]: d["blocked"] for d in (await client.get("/admin/writeback/backfill")).json()["documents"]}

    assert "seed data" in preview[seed_doc["doc_no"]]
    assert "SAMPLE-ONLY" in preview[unknown["doc_no"]]
    for no in (seed_doc["doc_no"], unknown["doc_no"]):
        res = await client.post("/admin/writeback/backfill", json={"documents": [{"kind": "sales_invoice", "doc_no": no}]})
        assert res.status_code == 409 and "must not be sent" in res.json()["detail"]
    assert outbox() == []


async def test_backfill_needs_sql_server(client, flag_off, monkeypatch):
    monkeypatch.setattr(db, "_db_available", False)

    res = await client.get("/admin/writeback/backfill")

    assert res.status_code == 409 and "Connect to SQL Server" in res.json()["detail"]


# ─── Admin endpoints ─────────────────────────────────────────────────────────

async def test_status_sync_and_retry_endpoints(client, cashier_client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale())
    await client.post("/transactions/sales-invoice", json=sale())
    sqlserver.fail_on = "INSERT INTO dbo.Web_WriteLog"
    await client.post("/admin/writeback/sync")

    state = (await client.get("/admin/writeback")).json()
    assert (state["enabled"], state["db_connected"], state["pending"], state["failed"], state["synced"]) == (True, True, 1, 1, 0)
    assert "simulated failure" in state["last_error"]
    failed = (await client.get("/admin/writeback/outbox", params={"status": "FAILED"})).json()
    assert len(failed) == 1 and failed[0]["sync_attempts"] == 1 and "payload" not in failed[0]

    sqlserver.fail_on = None
    retried = (await client.post("/admin/writeback/retry", json={"ids": [failed[0]["id"]]})).json()

    assert (retried["reset"], retried["sent"]) == (1, 2)
    assert retried["status"]["synced"] == 2 and retried["status"]["last_error"] is None
    for path in ("/admin/writeback/sync", "/admin/writeback/retry"):
        assert (await cashier_client.post(path)).status_code == 403
    assert (await cashier_client.get("/admin/writeback")).status_code == 403


async def test_sync_endpoint_says_when_the_flag_is_off(client, flag_off):
    res = await client.post("/admin/writeback/sync")

    assert res.status_code == 409 and "DB_WRITE_ENABLED" in res.json()["detail"]


# ─── What gets written ───────────────────────────────────────────────────────

async def test_other_documents_are_written_too(client, sqlserver):
    await client.post("/transactions/purchase-bill", json={
        "date": TODAY, "supplier": next(iter(store.SUPPLIERS)),
        "supplier_inv_no": "S-1", "supplier_inv_date": TODAY, "supplier_inv_value": 0, "lines": [{"item_code": "KF-650", "cases": 1}]})
    await client.post("/transactions/expense", json={"date": TODAY, "paid_from": "CASH",
                                                     "lines": [{"ledger": next(iter(store.LEDGERS)), "amount": 150}]})
    await client.post("/transactions/denomination", json={"date": TODAY, "counts": {"500": 2}})
    assert [r["transaction_type"] for r in outbox()] == ["purchase_bill", "expense", "denomination"]

    assert writeback.sync_pending_data()["sent"] == 3

    kinds = {k for (k,) in sqlserver.rows("SELECT DocKind FROM Web_DocHeader")}
    assert kinds == {"purchase_bill", "expense", "denomination"}
    assert sqlserver.rows("SELECT COUNT(*) FROM Web_DocLine WHERE DocKind = 'expense'") == [(0,)]
    assert sqlserver.rows("SELECT QtyChange > 0 FROM Web_StockBalance WHERE ItemCode = 'KF-650'") == [(1,)]


async def test_opening_stock_of_a_new_item_is_written(client, sqlserver):
    for code in ("NEW-1", "NEW-2"):
        await client.post("/masters/items", json={"code": code, "name": f"{code} RUM 750", "group": "RUM",
                                                  "package": "750ml", "mrp": 500, "purchase_rate": 400, "qoh": 12})
    assert [(r["transaction_type"], r["transaction_id"]) for r in outbox()] == [
        ("opening_stock", "OPENING/NEW-1"), ("opening_stock", "OPENING/NEW-2")]

    assert writeback.sync_pending_data()["sent"] == 2

    assert sqlserver.rows("SELECT ItemCode, QtyChange FROM Web_StockBalance ORDER BY ItemCode") == [("NEW-1", 12), ("NEW-2", 12)]


async def test_failed_requests_and_held_bills_queue_nothing(client, sqlserver):
    too_many = await client.post("/transactions/sales-invoice", json=sale(bottles=10_000))
    held = await client.post("/transactions/sales-invoice", json=sale(hold=True))
    assert too_many.status_code == 422 and held.status_code == 201
    assert outbox() == []

    released = (await client.post(f"/transactions/sales-invoice/{held.json()['doc_no']}/release")).json()

    assert [(r["action"], r["transaction_id"]) for r in outbox()] == [("release", released["doc_no"])]


async def test_values_are_parameters_not_sql(client, sqlserver):
    remarks = "x'); DROP TABLE Web_DocHeader; --"
    await client.post("/transactions/purchase-indent", json={
        "date": TODAY, "supplier": next(iter(store.SUPPLIERS)), "remarks": remarks,
        "lines": [{"item_code": "KF-650", "bottles": 1}]})

    writeback.sync_pending_data()

    (body,) = sqlserver.rows("SELECT DocJson FROM Web_DocHeader")[0]
    assert remarks in body


async def test_a_number_already_used_by_a_desktop_bill_is_flagged(client, sqlserver):
    doc = (await client.post("/transactions/sales-invoice", json=sale())).json()
    sqlserver.sqlite.execute("INSERT INTO TrnHdrSINV VALUES (?)", (doc["doc_no"],))

    writeback.sync_pending_data()

    assert sqlserver.rows("SELECT LegacyNoClash FROM Web_DocHeader") == [(1,)]


async def test_refuses_without_the_migration(client, sqlserver):
    sqlserver.sqlite.execute("DELETE FROM Web_SchemaVersion")
    await client.post("/transactions/sales-invoice", json=sale())

    assert "tools/migrate.py up" in writeback.sync_pending_data()["skipped"]
    assert statuses() == ["PENDING"]


# ─── Double submit (Idempotency-Key) ─────────────────────────────────────────

async def test_same_idempotency_key_makes_one_bill(client):
    stock = store.ITEMS["VIN-750"]["qoh"]
    headers = {"Idempotency-Key": "bill-draft-123"}

    first = await client.post("/transactions/sales-invoice", json=sale(bottles=2), headers=headers)
    again = await client.post("/transactions/sales-invoice", json=sale(bottles=2), headers=headers)

    assert first.status_code == again.status_code == 201
    assert again.json()["doc_no"] == first.json()["doc_no"]
    assert again.headers["Idempotent-Replay"] == "true"
    assert store.ITEMS["VIN-750"]["qoh"] == stock - 2
    other = await client.post("/transactions/sales-invoice", json=sale(bottles=2), headers={"Idempotency-Key": "next"})
    assert other.json()["doc_no"] != first.json()["doc_no"]


async def test_a_refused_save_can_be_retried_with_the_same_key(client):
    headers = {"Idempotency-Key": "k1"}
    assert (await client.post("/transactions/sales-invoice", json=sale(bottles=99_999), headers=headers)).status_code == 422
    assert (await client.post("/transactions/sales-invoice", json=sale(bottles=1), headers=headers)).status_code == 201


async def test_keys_are_per_user(client, cashier_client):
    headers = {"Idempotency-Key": "same"}
    a = (await client.post("/transactions/sales-invoice", json=sale(), headers=headers)).json()
    b = (await cashier_client.post("/transactions/sales-invoice", json=sale(), headers=headers)).json()

    assert a["doc_no"] != b["doc_no"]


async def test_same_key_with_a_different_bill_is_refused_not_replayed(client):
    headers = {"Idempotency-Key": "draft-9"}
    first = await client.post("/transactions/sales-invoice", json=sale(bottles=1), headers=headers)
    count = len(store.DOCS["sales_invoice"])

    other = await client.post("/transactions/sales-invoice", json=sale(bottles=3), headers=headers)

    assert first.status_code == 201
    assert other.status_code == 409 and "already saved" in other.json()["detail"]
    assert len(store.DOCS["sales_invoice"]) == count


async def test_replayed_reply_has_the_security_headers(client):
    headers = {"Idempotency-Key": "h1"}
    await client.post("/transactions/sales-invoice", json=sale(), headers=headers)

    again = await client.post("/transactions/sales-invoice", json=sale(), headers=headers)

    assert again.headers["Idempotent-Replay"] == "true"
    assert again.headers["X-Content-Type-Options"] == "nosniff"
    assert again.headers["X-Frame-Options"] == "DENY"


async def test_no_replay_for_a_signed_out_token(client):
    headers = {"Idempotency-Key": "h2"}
    await client.post("/transactions/sales-invoice", json=sale(), headers=headers)
    store.USERS["ADMIN"]["pwv"] += 1   # as a password change does: the old token is no longer valid

    again = await client.post("/transactions/sales-invoice", json=sale(), headers=headers)

    assert again.status_code == 401
    assert "Idempotent-Replay" not in again.headers


# ─── Migration scripts ───────────────────────────────────────────────────────

def _migrations():
    return sorted(p for p in MIGRATIONS.glob("[0-9][0-9][0-9]_*.sql") if re.fullmatch(r"\d{3}_[\w-]+\.sql", p.name))


def test_every_migration_has_a_rollback_and_is_safe_to_run_twice():
    from tools.migrate import batches
    scripts = _migrations()
    assert [p.name for p in scripts] == ["001_web_tables.sql"]
    for path in scripts:
        assert path.with_name(path.name.replace(".sql", ".rollback.sql")).exists()
        for batch in batches(path.read_text()):
            code = re.sub(r"--[^\n]*", "", batch).strip()
            if re.search(r"\bCREATE\s+(TABLE|INDEX)\b", code, re.I):
                assert re.match(r"IF NOT EXISTS", code, re.I), code[:80]
            assert not re.search(r"\b(ALTER|DROP)\s+TABLE\b", code, re.I), "001 must not alter or drop anything"


def test_migration_touches_only_web_tables():
    sql = (MIGRATIONS / "001_web_tables.sql").read_text() + (MIGRATIONS / "001_web_tables.rollback.sql").read_text()
    code = re.sub(r"--[^\n]*", "", sql)
    for name in re.findall(r"\bdbo\.(\w+)", code):
        assert name.startswith("Web_"), name


def test_batches_split_on_go_lines_only():
    from tools.migrate import batches
    assert batches("SELECT 1\nGO\n-- note\nGO\nSELECT 'GOOD'\n go \n") == ["SELECT 1", "SELECT 'GOOD'"]
