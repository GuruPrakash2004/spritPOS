"""SQL Server write-back (app/writeback.py) against an in-memory SQLite stand-in for SQL Server.

SQLite gives real transactions, rowcounts and primary keys, so commit/rollback, idempotency and the
relative stock update are really exercised. The T-SQL-only bits (dbo., SYSUTCDATETIME(), WITH (NOLOCK),
TOP) are translated. The same flow against a real SQL Server copy is in test_writeback_sqlserver.py.
"""
import json
import re
import sqlite3
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
        self.fail_on = conn.fail_on

    def execute(self, sql, params=()):
        if self.fail_on and self.fail_on in sql:
            raise RuntimeError("network drop in the middle of the save")
        self._cur.execute(_tsql_to_sqlite(sql), params)

    def __getattr__(self, name):
        return getattr(self._cur, name)


class FakeConn:
    """Stands in for the shared pymssql/pyodbc connection."""

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


@pytest.fixture
def sqlserver(monkeypatch):
    """Write-back switched on, with the fake SQL Server connected and masters 'loaded from the DB'."""
    conn = FakeConn()
    monkeypatch.setattr(db, "_conn", conn)
    monkeypatch.setattr(db, "_db_available", True)
    monkeypatch.setattr(db, "_driver_used", "test")
    monkeypatch.setattr(writeback, "ENABLED", True)
    monkeypatch.setattr(writeback, "masters_from_db", True)
    monkeypatch.setattr(writeback, "status", {"last_error": None, "last_error_at": None, "last_sync_at": None, "written": 0})
    return conn


def sale(item="VIN-750", bottles=2, **extra):
    return {"date": TODAY, "counter": "counter10", "salesperson": "GURU",
            "lines": [{"item_code": item, "bottles": bottles}], **extra}


async def test_off_by_default_nothing_is_queued(client):
    assert writeback.ENABLED is False   # DB_WRITE_ENABLED is not set in the tests

    assert (await client.post("/transactions/sales-invoice", json=sale())).status_code == 201

    assert store.OUTBOX == []


async def test_a_sale_is_written_in_one_transaction_with_its_stock(client, sqlserver):
    doc = (await client.post("/transactions/sales-invoice", json=sale(bottles=3))).json()
    assert len(store.OUTBOX) == 1
    event = store.OUTBOX[0]
    assert (event["kind"], event["doc_no"], event["user"]) == ("sales_invoice", doc["doc_no"], "ADMIN")
    assert [(m["item"], m["qty"]) for m in event["moves"]] == [("VIN-750", -3)]

    assert writeback.flush() == 1

    assert store.OUTBOX == []
    assert sqlserver.rows("SELECT DocKind, DocNo, Status, NetAmount, CreatedBy FROM Web_DocHeader") == [
        ("sales_invoice", doc["doc_no"], "SAVED", doc["totals"]["net"], "ADMIN")]
    assert sqlserver.rows("SELECT ItemCode, Qty FROM Web_DocLine") == [("VIN-750", 3)]
    assert sqlserver.rows("SELECT ItemCode, Qty, MoveKind FROM Web_StockMove") == [("VIN-750", -3, "sale")]
    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance WHERE ItemCode = 'VIN-750'") == [(-3,)]
    assert sqlserver.rows("SELECT Action, UserCode FROM Web_WriteLog") == [("save", "ADMIN")]


async def test_stock_balance_adds_up_across_saves_and_cancel(client, sqlserver):
    first = (await client.post("/transactions/sales-invoice", json=sale(bottles=2))).json()
    await client.post("/transactions/sales-invoice", json=sale(bottles=5))
    await client.post(f"/transactions/sales-invoice/{first['doc_no']}/cancel", json={"remarks": "wrong item"})

    assert writeback.flush() == 3

    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance WHERE ItemCode = 'VIN-750'") == [(-5,)]
    assert sqlserver.rows("SELECT Status FROM Web_DocHeader WHERE DocNo = ?", (first["doc_no"],)) == [("CANCELLED",)]
    assert sqlserver.rows("SELECT COUNT(*) FROM Web_DocHeader") == [(2,)]


async def test_retrying_an_event_never_writes_it_twice(client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale(bottles=4))
    event = dict(store.OUTBOX[0])
    writeback.flush()

    store.OUTBOX.append(event)   # e.g. the app restarted before the queue removal was saved
    assert writeback.flush() == 1

    assert sqlserver.rows("SELECT COUNT(*) FROM Web_StockMove") == [(1,)]
    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance") == [(-4,)]


async def test_a_failure_mid_save_rolls_everything_back_and_keeps_it_queued(client, sqlserver, monkeypatch):
    def network_back():   # the reconnect after the drop finds the server again
        db._conn, db._db_available = sqlserver, True
        return True
    monkeypatch.setattr(db, "init_db", network_back)
    await client.post("/transactions/sales-invoice", json=sale(bottles=1))
    sqlserver.fail_on = "INSERT INTO dbo.Web_WriteLog"   # last statement of the transaction

    assert writeback.flush() == 0

    assert len(store.OUTBOX) == 1
    assert "network drop" in writeback.state()["last_error"]
    for table in ("Web_DocHeader", "Web_DocLine", "Web_StockMove", "Web_StockBalance", "Web_WriteLog"):
        assert sqlserver.rows(f"SELECT COUNT(*) FROM {table}") == [(0,)], table

    sqlserver.fail_on = None
    assert writeback.flush() == 1   # reconnects, then sends
    assert writeback.state()["last_error"] is None
    assert sqlserver.rows("SELECT QtyChange FROM Web_StockBalance") == [(-1,)]


async def test_events_are_sent_in_order_and_stop_at_the_first_failure(client, sqlserver):
    for n in (1, 2, 3):
        await client.post("/transactions/sales-invoice", json=sale(bottles=n))
    sqlserver.fail_on = "UPDATE dbo.Web_StockBalance"
    await client.post("/transactions/sales-invoice", json=sale(item="RC-750", bottles=1))

    assert writeback.flush() == 0   # the first event already touches Web_StockBalance
    assert len(store.OUTBOX) == 4


async def test_failed_requests_and_held_bills_queue_nothing(client, sqlserver):
    too_many = await client.post("/transactions/sales-invoice", json=sale(bottles=10_000))
    held = await client.post("/transactions/sales-invoice", json=sale(hold=True))
    assert too_many.status_code == 422 and held.status_code == 201
    assert store.OUTBOX == []

    released = (await client.post(f"/transactions/sales-invoice/{held.json()['doc_no']}/release")).json()

    assert [(e["action"], e["doc_no"]) for e in store.OUTBOX] == [("release", released["doc_no"])]


async def test_other_documents_are_written_too(client, sqlserver):
    await client.post("/transactions/purchase-bill", json={
        "date": TODAY, "supplier": next(iter(store.SUPPLIERS)),
        "supplier_inv_no": "S-1", "supplier_inv_date": TODAY, "supplier_inv_value": 0, "lines": [{"item_code": "KF-650", "cases": 1}]})
    await client.post("/transactions/expense", json={"date": TODAY, "paid_from": "CASH",
                                                     "lines": [{"ledger": next(iter(store.LEDGERS)), "amount": 150}]})
    await client.post("/transactions/denomination", json={"date": TODAY, "counts": {"500": 2}})
    assert [e["kind"] for e in store.OUTBOX] == ["purchase_bill", "expense", "denomination"]

    assert writeback.flush() == 3

    kinds = {k for (k,) in sqlserver.rows("SELECT DocKind FROM Web_DocHeader")}
    assert kinds == {"purchase_bill", "expense", "denomination"}
    assert sqlserver.rows("SELECT COUNT(*) FROM Web_DocLine WHERE DocKind = 'expense'") == [(0,)]
    assert sqlserver.rows("SELECT QtyChange > 0 FROM Web_StockBalance WHERE ItemCode = 'KF-650'") == [(1,)]


async def test_values_are_parameters_not_sql(client, sqlserver):
    remarks = "x'); DROP TABLE Web_DocHeader; --"
    await client.post("/transactions/purchase-indent", json={
        "date": TODAY, "supplier": next(iter(store.SUPPLIERS)), "remarks": remarks,
        "lines": [{"item_code": "KF-650", "bottles": 1}]})

    writeback.flush()

    (body,) = sqlserver.rows("SELECT DocJson FROM Web_DocHeader")[0]
    assert remarks in body


async def test_a_number_already_used_by_a_desktop_bill_is_flagged(client, sqlserver):
    doc = (await client.post("/transactions/sales-invoice", json=sale())).json()
    sqlserver.sqlite.execute("INSERT INTO TrnHdrSINV VALUES (?)", (doc["doc_no"],))

    writeback.flush()

    assert sqlserver.rows("SELECT LegacyNoClash FROM Web_DocHeader") == [(1,)]


async def test_refuses_without_the_migration(client, sqlserver):
    sqlserver.sqlite.execute("DELETE FROM Web_SchemaVersion")
    await client.post("/transactions/sales-invoice", json=sale())

    assert writeback.flush() == 0
    assert "tools/migrate.py up" in writeback.state()["last_error"]
    assert len(store.OUTBOX) == 1


async def test_nothing_is_queued_or_sent_while_running_on_seed_data(client, sqlserver, monkeypatch):
    monkeypatch.setattr(writeback, "masters_from_db", False)
    assert (await client.post("/transactions/sales-invoice", json=sale())).status_code == 201
    assert store.OUTBOX == []

    store.OUTBOX.append({"id": "x" * 32, "kind": "sales_invoice", "doc_no": "N", "action": "save", "user": None,
                         "at": TODAY, "doc": {"date": TODAY}, "moves": []})
    assert writeback.flush() == 0
    assert "seed data" in writeback.state()["last_error"]
    assert sqlserver.rows("SELECT COUNT(*) FROM Web_DocHeader") == [(0,)]


async def test_queue_survives_a_restart(client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale())
    saved = json.loads(json.dumps(persistence.snapshot(), default=str))   # as written to the file
    store.OUTBOX.clear()

    persistence.apply(saved)

    assert len(store.OUTBOX) == 1


async def test_status_endpoint_is_admin_only(client, cashier_client, sqlserver):
    await client.post("/transactions/sales-invoice", json=sale())

    res = await client.get("/admin/writeback")
    assert res.status_code == 200
    assert res.json()["enabled"] is True and res.json()["queued"] == 1
    assert (await cashier_client.get("/admin/writeback")).status_code == 403
    assert (await client.post("/admin/writeback/flush")).json()["queued"] == 0


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


async def test_opening_stock_of_a_new_item_is_written(client, sqlserver):
    for code in ("NEW-1", "NEW-2"):
        await client.post("/masters/items", json={"code": code, "name": f"{code} RUM 750", "group": "RUM",
                                                  "package": "750ml", "mrp": 500, "purchase_rate": 400, "qoh": 12})
    assert [(e["kind"], e["doc_no"], [m["item"] for m in e["moves"]]) for e in store.OUTBOX] == [
        ("opening_stock", "OPENING/NEW-1", ["NEW-1"]), ("opening_stock", "OPENING/NEW-2", ["NEW-2"])]

    assert writeback.flush() == 2

    assert sqlserver.rows("SELECT ItemCode, QtyChange FROM Web_StockBalance ORDER BY ItemCode") == [("NEW-1", 12), ("NEW-2", 12)]


async def test_bills_loaded_from_the_desktop_db_are_not_written(client, sqlserver):
    no, doc = next((n, d) for n, d in store.DOCS["sales_invoice"].items() if "origin" in d and d["status"] == "SAVED")

    res = await client.post(f"/transactions/sales-invoice/{no}/cancel", json={"remarks": "test"})

    assert res.status_code == 200
    assert store.OUTBOX == []
