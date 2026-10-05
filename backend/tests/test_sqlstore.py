"""app/sqlstore.py without a real SQL Server: a fake cursor records what each request would write.

The live behaviour (triggers, identity values, reload after a restart) is checked against a copy of the
database by hand; see backend/migrations/README.md."""
from contextlib import contextmanager
from datetime import date

import pytest

from app import db, sqlstore, store

TODAY = date.today().isoformat()


class FakeCursor:
    bills: set = set()   # SINVNOs "inserted" so far (a later SELECT SINVID finds them)

    def __init__(self, log, fail_on=None):
        self.log, self.fail_on, self.last, self.params = log, fail_on, "", ()

    def execute(self, sql, params=()):
        if self.fail_on and self.fail_on in sql:
            raise RuntimeError("simulated SQL Server error")
        self.log.append((sql, params))
        self.last, self.params = sql, params
        if sql.startswith("INSERT INTO dbo.[Web_TrnHdrSINV]"):
            self.bills.add(params[0])
        return 0   # no row updated: upserts insert

    def execute_raw(self, sql):
        return self.execute(sql)

    def fetchone(self):
        if "OUTPUT INSERTED" in self.last or "MAX(" in self.last:
            return (1,)
        if "ITEMID, UOMID FROM dbo.Web_MstITEM" in self.last:
            return (5, 1)
        if self.last.startswith("SELECT SINVID FROM dbo.Web_TrnHdrSINV") and self.params and self.params[0] in self.bills:
            return (1,)
        return None


class FakeMeta(dict):
    def get(self, key, default=None):
        return {"name": key, "type": "varchar", "max_length": -1, "is_identity": 0, "is_computed": 0,
                "is_nullable": 1, "has_default": 1}


@pytest.fixture
def sql(monkeypatch):
    """sqlstore switched on, with every transaction going to a FakeCursor. Returns the statement log."""
    log: list = []
    state = {"fail_on": None}

    @contextmanager
    def transaction():
        yield FakeCursor(log, state["fail_on"])
    monkeypatch.setattr(db, "transaction", transaction)
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(sqlstore, "_meta", lambda table: FakeMeta())
    monkeypatch.setattr(sqlstore, "_active", True)
    monkeypatch.setattr(sqlstore, "_saved", {})
    monkeypatch.setattr(sqlstore, "_doc_status", {})
    monkeypatch.setattr(sqlstore, "_moves_saved", len(store.MOVEMENTS) - store.APP_MOVEMENTS_FROM)
    return log, state


def _tables(log, verb="INSERT INTO"):
    return [s.split(verb, 1)[1].split("(", 1)[0].strip() for s, _ in log if s.lstrip().startswith(verb)]


async def _sell(client, qty=2):
    return await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "customer": "CASH", "payment_type": "CASH", "salesperson": "GURU",
        "lines": [{"item_code": "VIN-750", "cases": 0, "bottles": qty}]})


def test_prefix_of_a_document_number():
    assert sqlstore._prefix("26/c10-0083718") == "c10"
    assert sqlstore._prefix("26/PB-0000001") == "PB"
    assert sqlstore._prefix("2600025") == ""


def test_fit_cuts_strings_to_the_column_and_drops_unknown_columns(monkeypatch):
    monkeypatch.setitem(sqlstore._cols, "Web_X", {
        "name": {"name": "Name", "type": "varchar", "max_length": 5, "is_identity": 0, "is_computed": 0},
        "id": {"name": "ID", "type": "int", "max_length": 4, "is_identity": 1, "is_computed": 0},
        "code": {"name": "Code", "type": "nvarchar", "max_length": 6, "is_identity": 0, "is_computed": 0}})
    assert sqlstore._fit("Web_X", {"NAME": "abcdefgh", "id": 7, "code": "ABCDEFG", "nope": 1}) == {"Name": "abcde", "Code": "ABC"}


async def test_a_sale_is_written_in_legacy_format_with_its_stock_row(client, sql):
    log, _ = sql
    res = await _sell(client)
    assert res.status_code == 201, res.text
    inserted = _tables(log)
    for t in ("dbo.[Web_TrnHdrSINV]", "dbo.[Web_TrnDtlSINV]", "dbo.[Web_UtlStockList]", "dbo.[Web_AppDoc]", "dbo.Web_AppMove",
              "dbo.[Web_UtlAdmAudit]"):
        assert t in inserted, (t, inserted)
    header = next(p for s, p in log if "INSERT INTO dbo.[Web_TrnHdrSINV]" in s)
    assert res.json()["doc_no"] in header
    # The first save marks the database as used by the web app (a new copy of the old data is then refused).
    assert any("Web_AppSetting" in s and "first_web_write" in p for s, p in log if p)


async def test_only_what_changed_is_written_again(client, sql):
    log, _ = sql
    doc_no = (await _sell(client)).json()["doc_no"]
    log.clear()
    await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "wrong item"})
    assert "dbo.[Web_TrnHdrSINV]" not in _tables(log)            # the header is updated, not added again
    assert any(s.startswith("UPDATE dbo.[Web_TrnHdrSINV]") and "Y" in p for s, p in log)
    assert any("DELETE FROM dbo.Web_UtlStockList" in s for s, _ in log)   # the sale's stock rows go
    log.clear()
    await client.put("/masters/brands/" + next(iter(store.BRANDS)), json={**next(iter(store.BRANDS.values())), "name": "Renamed"})
    written = _tables(log) + _tables(log, "UPDATE")
    assert not any("SINV" in t or "AppDoc" in t for t in written), written


async def test_a_failed_save_answers_503_and_reloads(client, sql, monkeypatch):
    log, state = sql
    reloaded = []
    monkeypatch.setattr(sqlstore, "reload", lambda: reloaded.append(True))
    state["fail_on"] = "Web_TrnHdrSINV"
    res = await _sell(client)
    assert res.status_code == 503 and "Not saved" in res.json()["detail"]
    assert reloaded == [True]


async def test_without_a_database_nothing_is_written(client, monkeypatch):
    calls = []
    monkeypatch.setattr(db, "transaction", lambda: calls.append(1))
    assert (await _sell(client)).status_code == 201
    assert calls == []
