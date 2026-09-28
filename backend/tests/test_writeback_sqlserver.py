"""Write-back against a real SQL Server: the TEST COPY of the database (restored backup), never live.

Skipped unless these are set in the environment (not in backend/.env, so they can't be picked up by accident):
    TEST_DB_HOST, TEST_DB_PORT (default 1433), TEST_DB_NAME, TEST_DB_USER, TEST_DB_PASSWORD
It refuses to run when TEST_DB_NAME is the live database named by DB_NAME in backend/.env.

    TEST_DB_HOST=... TEST_DB_NAME=vLQMSLHOUSE_TEST TEST_DB_USER=... TEST_DB_PASSWORD=... \\
        .venv/bin/python -m pytest tests/test_writeback_sqlserver.py -v

It applies migrations/001 (twice, to prove it is safe to repeat), writes sales through the API, checks the
Web_ tables, retries, then runs the rollback script, leaving the test database as it found it.
"""
import os
from datetime import date
from pathlib import Path

import pytest

from app import db, store, writeback

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
def enabled(test_db, monkeypatch):
    monkeypatch.setattr(writeback, "ENABLED", True)
    monkeypatch.setattr(writeback, "masters_from_db", True)



def _one(sql: str, params: tuple = ()):
    with db.transaction() as cur:
        cur.execute(sql, params)
        return cur.fetchone()


async def test_sale_cancel_and_retry_on_sql_server(client, enabled):
    before = _one("SELECT ISNULL(SUM(QtyChange), 0) FROM dbo.Web_StockBalance WHERE ItemCode = ?", ("VIN-750",))[0]
    sale = (await client.post("/transactions/sales-invoice", json={
        "date": date.today().isoformat(), "counter": "counter10", "salesperson": "GURU",
        "lines": [{"item_code": "VIN-750", "bottles": 2}]})).json()
    event = dict(store.OUTBOX[0])

    assert writeback.flush() == 1, writeback.state()
    store.OUTBOX.append(event)
    assert writeback.flush() == 1                   # already written: skipped
    await client.post(f"/transactions/sales-invoice/{sale['doc_no']}/cancel", json={"remarks": "test"})
    assert writeback.flush() == 1

    assert _one("SELECT Status FROM dbo.Web_DocHeader WHERE DocKind = ? AND DocNo = ?",
                ("sales_invoice", sale["doc_no"]))[0] == "CANCELLED"
    after = _one("SELECT ISNULL(SUM(QtyChange), 0) FROM dbo.Web_StockBalance WHERE ItemCode = ?", ("VIN-750",))[0]
    assert float(after) == float(before)            # -2 then +2
    assert _one("SELECT COUNT(*) FROM dbo.Web_StockMove WHERE DocNo = ?", (sale["doc_no"],))[0] == 2
