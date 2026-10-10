"""app/ai/sales_history.py: window planning, netting of returns, zero-fill and the incremental refresh.
The database is faked: nothing here touches SQL Server."""
from contextlib import contextmanager
from datetime import date, datetime

import pytest

from app.ai import sales_history as sh

D = date


def test_first_run_backfills_and_stops_at_through():
    assert sh.plan_window(None, D(2026, 10, 9), backfill_days=10) == (D(2026, 9, 30), D(2026, 10, 9))


def test_later_run_rereads_the_last_stored_days():
    # last stored day 10-05, lookback 3: re-read 10-03..10-09
    assert sh.plan_window(D(2026, 10, 5), D(2026, 10, 9), lookback_days=3) == (D(2026, 10, 3), D(2026, 10, 9))


def test_nothing_new_when_stored_data_is_ahead_of_through():
    assert sh.plan_window(D(2026, 10, 20), D(2026, 10, 9), lookback_days=3) is None


def test_chunks_cover_the_range_without_gaps():
    chunks = sh.chunk_dates(D(2026, 1, 1), D(2026, 1, 10), size=4)
    assert chunks == [(D(2026, 1, 1), D(2026, 1, 4)), (D(2026, 1, 5), D(2026, 1, 8)), (D(2026, 1, 9), D(2026, 1, 10))]


def test_returns_are_netted_off_the_sales_of_the_same_day():
    rows = sh.combine({(1, D(2026, 10, 1)): 10, (2, D(2026, 10, 1)): 4}, {(1, D(2026, 10, 1)): 3})
    by = {r["item_id"]: r for r in rows}
    assert (by[1]["sold"], by[1]["returned"], by[1]["net"]) == (10, 3, 7)
    assert by[2]["net"] == 4


def test_a_day_with_only_a_return_is_negative():
    rows = sh.combine({}, {(5, D(2026, 10, 2)): 2})
    assert rows == [{"item_id": 5, "day": D(2026, 10, 2), "sold": 0.0, "returned": 2.0, "net": -2.0}]


def test_zero_fill_adds_the_empty_days():
    rows = [{"day": D(2026, 10, 2), "net": 5}, {"day": datetime(2026, 10, 4, 0, 0), "net": 2}]
    assert sh.zero_fill(rows, D(2026, 10, 1), D(2026, 10, 5)) == [0, 5, 0, 2, 0]


class FakeDb:
    def __init__(self, last=None, sales=None, returns=None, returns_fail=False):
        self.last, self.sales, self.returns, self.returns_fail = last, sales or [], returns or [], returns_fail
        self.statements: list[tuple[str, tuple]] = []
        self.transactions = 0

    def is_db_available(self):
        return True

    def safe_error(self, exc):
        return str(exc)

    def fetchone(self, sql, params=()):
        return {"d": self.last}

    def fetchall(self, sql, params=()):
        if "TrnHdrSR" in sql:
            if self.returns_fail:
                raise RuntimeError("Invalid object name 'TrnDtlSR'")
            return self.returns
        return self.sales

    @contextmanager
    def transaction(self):
        self.transactions += 1
        db = self

        class Cur:
            def execute(self, sql, params=()):
                db.statements.append((sql, params))
        yield Cur()


@pytest.fixture
def on(monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")


def install(monkeypatch, fake):
    monkeypatch.setattr(sh, "db", fake)
    return fake


def test_refresh_does_nothing_while_the_flag_is_off(monkeypatch):
    monkeypatch.delenv("AI_FORECAST_ENABLED", raising=False)
    fake = install(monkeypatch, FakeDb())
    assert sh.refresh(through=D(2026, 10, 9))["status"] == "skipped"
    assert fake.statements == [] and fake.transactions == 0


def test_refresh_reads_only_the_new_days_and_replaces_them(on, monkeypatch):
    fake = install(monkeypatch, FakeDb(last=D(2026, 10, 7),
                                       sales=[{"item_id": 1, "day": D(2026, 10, 8), "qty": 6}],
                                       returns=[{"item_id": 1, "day": D(2026, 10, 8), "qty": 1}]))
    res = sh.refresh(through=D(2026, 10, 9))
    assert (res["status"], res["from"], res["to"]) == ("ok", "2026-10-05", "2026-10-09")
    delete, insert = fake.statements[0], fake.statements[1]
    assert delete[0].startswith("DELETE") and delete[1] == ("2026-10-05", "2026-10-09")
    assert insert[1] == (1, "2026-10-08", 6.0, 1.0, 5.0)


def test_refresh_is_up_to_date_when_nothing_is_new(on, monkeypatch):
    fake = install(monkeypatch, FakeDb(last=D(2026, 10, 30)))
    assert sh.refresh(through=D(2026, 10, 9))["status"] == "up-to-date"
    assert fake.transactions == 0


def test_a_shop_without_a_returns_table_still_loads_sales(on, monkeypatch):
    fake = install(monkeypatch, FakeDb(last=D(2026, 10, 8), returns_fail=True,
                                       sales=[{"item_id": 3, "day": D(2026, 10, 9), "qty": 2}]))
    res = sh.refresh(through=D(2026, 10, 9))
    assert res["status"] == "ok" and res["rows"] == 1
    assert fake.statements[1][1] == (3, "2026-10-09", 2.0, 0.0, 2.0)


def test_first_run_saves_one_transaction_per_chunk(on, monkeypatch):
    fake = install(monkeypatch, FakeDb(last=None))
    res = sh.refresh(through=D(2026, 10, 9), backfill_days=30)
    assert res["days"] == 30 and fake.transactions == 3   # 14 + 14 + 2 days


def test_the_queries_exclude_cancelled_and_deleted_bills_and_are_date_limited():
    assert "Cancelled = 'N'" in sh._SALES_SQL and "IsDelete = 'N'" in sh._SALES_SQL
    assert "h.SINVDate >= ?" in sh._SALES_SQL and "h.SINVDate < ?" in sh._SALES_SQL
    assert "Cancelled = 'N'" in sh._RETURNS_SQL and "h.SRDate >= ?" in sh._RETURNS_SQL
