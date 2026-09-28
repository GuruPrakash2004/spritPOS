"""Write-back against a real SQL Server: the TEST COPY of the database (restored backup), never live.

Skipped unless these are set in the environment (not in backend/.env, so they can't be picked up by accident):
    TEST_DB_HOST, TEST_DB_PORT (default 1433), TEST_DB_NAME, TEST_DB_USER, TEST_DB_PASSWORD
It refuses to run when TEST_DB_NAME is the live database named by DB_NAME in backend/.env.

    TEST_DB_HOST=... TEST_DB_NAME=vLQMSLHOUSE_TEST TEST_DB_USER=... TEST_DB_PASSWORD=... \\
        .venv/bin/python -m pytest tests/test_writeback_sqlserver.py -v

It applies migrations/001 (twice, to prove it is safe to repeat), then: a bill with the flag off stays PENDING
in SQLite; turning the flag on syncs it; a second sync writes nothing; a crash between the SQL commit and the
SQLite update is detected on the next run without a duplicate. Finally it runs the rollback script, leaving
the test database as it found it.
"""
import os
from datetime import date
from pathlib import Path

import pytest

from app import db, persistence, writeback

HERE = Path(__file__).resolve().parents[1]
NEEDED = ("TEST_DB_HOST", "TEST_DB_NAME", "TEST_DB_USER", "TEST_DB_PASSWORD")


def _live_db_name() -> str:
    env = HERE / ".env"
    if env.exists():
        for line in env.read_text().splitlines():
            if line.strip().startswith("DB_NAME="):
                return line.split("=", 1)[1].strip().strip("'\"")
    return "vLQMSLHOUSE"


pytestmark = pytest.mark.skipif(any(not os.environ.get(k) for k in NEEDED),
                                reason="needs a TEST copy of the database: set TEST_DB_HOST/NAME/USER/PASSWORD")


@pytest.fixture(scope="module")
def test_db():
    name = os.environ["TEST_DB_NAME"]
    if name.lower() == _live_db_name().lower():
        pytest.fail(f"TEST_DB_NAME is the live database '{name}': point it at the restored test copy")
    saved = (db.DB_HOST, db.DB_PORT, db.DB_NAME, db.DB_USER, db.DB_PASSWORD)
    db.DB_HOST, db.DB_PORT = os.environ["TEST_DB_HOST"], os.environ.get("TEST_DB_PORT", "1433")
    db.DB_NAME, db.DB_USER, db.DB_PASSWORD = name, os.environ["TEST_DB_USER"], os.environ["TEST_DB_PASSWORD"]
    assert db.init_db(), "could not connect to the test database"
    from tools.migrate import MIGRATIONS, run
    run(MIGRATIONS / "001_web_tables.sql")
    run(MIGRATIONS / "001_web_tables.sql")          # safe to run twice
    yield
    run(MIGRATIONS / "001_web_tables.rollback.sql")
    run(MIGRATIONS / "001_web_tables.rollback.sql")  # rollback is safe to run twice too
    assert not db.fetchone("SELECT 1 AS x FROM sys.tables WHERE name LIKE 'Web[_]%'")
    db.DB_HOST, db.DB_PORT, db.DB_NAME, db.DB_USER, db.DB_PASSWORD = saved
    db._conn, db._db_available = None, False


@pytest.fixture
def connected(test_db, monkeypatch):
    """Masters 'from the DB', write-back OFF: bills are queued PENDING and not sent."""
    monkeypatch.setattr(writeback, "ENABLED", False)
    monkeypatch.setattr(writeback, "masters_from_db", True)


def _one(sql: str, params: tuple = ()):
    with db.transaction() as cur:
        cur.execute(sql, params)
        return cur.fetchone()


def _statuses() -> list[str]:
    return [r["sync_status"] for r in reversed(persistence.outbox_rows(limit=500))]


def _sale(bottles: int) -> dict:
    return {"date": date.today().isoformat(), "counter": "counter10", "salesperson": "GURU",
            "lines": [{"item_code": "VIN-750", "bottles": bottles}]}


async def test_pending_then_sync_retry_crash_and_cancel_on_sql_server(client, connected, monkeypatch):
    before = _one("SELECT ISNULL(SUM(QtyChange), 0) FROM dbo.Web_StockBalance WHERE ItemCode = ?", ("VIN-750",))[0]

    # 1. Flag off: saved in SQLite as PENDING, nothing in SQL Server.
    sale = (await client.post("/transactions/sales-invoice", json=_sale(2))).json()
    assert _statuses() == ["PENDING"]
    assert _one("SELECT COUNT(*) FROM dbo.Web_DocHeader WHERE DocNo = ?", (sale["doc_no"],))[0] == 0

    # 2. Flag on: sync sends it and marks it SYNCED.
    monkeypatch.setattr(writeback, "ENABLED", True)
    assert writeback.sync_pending_data()["sent"] == 1, writeback.state()
    assert _statuses() == ["SYNCED"]

    # 4. Syncing again writes nothing.
    assert writeback.sync_pending_data()["sent"] == 0

    # 5. Crash after the SQL commit, before SQLite was updated: detected, not duplicated.
    await client.post(f"/transactions/sales-invoice/{sale['doc_no']}/cancel", json={"remarks": "test"})
    real_mark = persistence.mark_synced
    monkeypatch.setattr(persistence, "mark_synced", lambda event_id: (_ for _ in ()).throw(OSError("crash")))
    writeback.sync_pending_data()
    monkeypatch.setattr(persistence, "mark_synced", real_mark)
    assert _statuses() == ["SYNCED", "SYNCING"]
    persistence.reset_stale()
    assert writeback.sync_pending_data()["already_there"] == 1
    assert _statuses() == ["SYNCED", "SYNCED"]

    assert _one("SELECT Status FROM dbo.Web_DocHeader WHERE DocKind = ? AND DocNo = ?",
                ("sales_invoice", sale["doc_no"]))[0] == "CANCELLED"
    assert _one("SELECT COUNT(*) FROM dbo.Web_WriteLog WHERE DocNo = ?", (sale["doc_no"],))[0] == 2
    assert _one("SELECT COUNT(*) FROM dbo.Web_StockMove WHERE DocNo = ?", (sale["doc_no"],))[0] == 2
    after = _one("SELECT ISNULL(SUM(QtyChange), 0) FROM dbo.Web_StockBalance WHERE ItemCode = ?", ("VIN-750",))[0]
    assert float(after) == float(before)            # -2 then +2
