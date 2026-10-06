"""app/sqlstore.py without a real SQL Server: a fake cursor records what each request would write.

The live behaviour (triggers, identity values, reload after a restart) is checked against a backup restore of the
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
        if sql.startswith("INSERT INTO dbo.[TrnHdrSINV]"):
            self.bills.add(params[0])
        return 0   # no row updated: upserts insert

    def execute_raw(self, sql):
        return self.execute(sql)

    def fetchone(self):
        if "OUTPUT INSERTED" in self.last or "MAX(" in self.last:
            return (1,)
        if "ITEMID, UOMID FROM dbo.MstITEM" in self.last:
            return (5, 1)
        if self.last.startswith("SELECT SINVID FROM dbo.TrnHdrSINV") and self.params and self.params[0] in self.bills:
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
    monkeypatch.setitem(sqlstore._cols, "X", {
        "name": {"name": "Name", "type": "varchar", "max_length": 5, "is_identity": 0, "is_computed": 0},
        "id": {"name": "ID", "type": "int", "max_length": 4, "is_identity": 1, "is_computed": 0},
        "code": {"name": "Code", "type": "nvarchar", "max_length": 6, "is_identity": 0, "is_computed": 0}})
    assert sqlstore._fit("X", {"NAME": "abcdefgh", "id": 7, "code": "ABCDEFG", "nope": 1}) == {"Name": "abcde", "Code": "ABC"}


async def test_a_sale_is_written_in_legacy_format_with_its_stock_row(client, sql):
    log, _ = sql
    res = await _sell(client)
    assert res.status_code == 201, res.text
    inserted = _tables(log)
    for t in ("dbo.[TrnHdrSINV]", "dbo.[TrnDtlSINV]", "dbo.[UtlStockList]", "dbo.[Web_AppDoc]", "dbo.Web_AppMove",
              "dbo.[UtlAdmAudit]"):
        assert t in inserted, (t, inserted)
    header = next(p for s, p in log if "INSERT INTO dbo.[TrnHdrSINV]" in s)
    assert res.json()["doc_no"] in header
    # The desktop's own tables are written directly: no Web_ copy of them exists any more.
    assert not any(t.startswith("dbo.[Web_") and "App" not in t and "ToolAudit" not in t for t in inserted), inserted


async def test_only_what_changed_is_written_again(client, sql):
    log, _ = sql
    doc_no = (await _sell(client)).json()["doc_no"]
    log.clear()
    await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "wrong item"})
    assert "dbo.[TrnHdrSINV]" not in _tables(log)            # the header is updated, not added again
    assert any(s.startswith("UPDATE dbo.[TrnHdrSINV]") and "Y" in p for s, p in log)
    assert any("DELETE FROM dbo.UtlStockList" in s for s, _ in log)   # the sale's stock rows go
    log.clear()
    await client.put("/masters/brands/" + next(iter(store.BRANDS)), json={**next(iter(store.BRANDS.values())), "name": "Renamed"})
    written = _tables(log) + _tables(log, "UPDATE")
    assert not any("SINV" in t or "AppDoc" in t for t in written), written


async def test_a_failed_save_answers_503_and_reloads(client, sql, monkeypatch):
    log, state = sql
    reloaded = []
    monkeypatch.setattr(sqlstore, "reload", lambda: reloaded.append(True))
    state["fail_on"] = "TrnHdrSINV"
    res = await _sell(client)
    assert res.status_code == 503 and "Not saved" in res.json()["detail"]
    assert reloaded == [True]


async def test_without_a_database_nothing_is_written(client, monkeypatch):
    calls = []
    monkeypatch.setattr(db, "transaction", lambda: calls.append(1))
    assert (await _sell(client)).status_code == 201
    assert calls == []


# ─── Writing into the desktop's own tables: never blank what the web app didn't load, never orphan history ───

class _Cur:
    """A cursor that answers 'SELECT TOP 1 <id>' with a row id (or nothing) and records every statement."""
    def __init__(self, found=None, used_by=None):
        self.log, self.found, self.used_by, self.last = [], found, used_by or (), ""

    def execute(self, sql, params=()):
        self.log.append((sql, params))
        self.last = sql
        return 1

    def execute_raw(self, sql):
        return self.execute(sql)

    def fetchone(self):
        if any(k in self.last.upper() for k in ("TOP 1 [CUSTID]", "TOP 1 [ITEMID]", "TOP 1 [USERID]")):
            return (7,) if self.found else None
        if any(t in self.last for t in self.used_by):
            return (1,)
        return None


def _update_columns(cur):
    sql = next(s for s, _ in cur.log if s.startswith("UPDATE"))
    return sql.split("SET", 1)[1].split("WHERE", 1)[0].lower()   # FakeMeta lower-cases the column names


def test_editing_a_customer_leaves_the_columns_the_loader_never_read(monkeypatch):
    monkeypatch.setattr(sqlstore, "_meta", lambda table: FakeMeta())
    cur = _Cur(found=True)
    # db_store.load_customers fills name, mobile, loyalty_card_no, send_sms and points: nothing about City/GSTNO/InActive.
    sqlstore._write_master(cur, "CUSTOMERS", "C1", {"code": "C1", "name": "Ravi", "mobile": "99", "loyalty_card_no": "", "send_sms": True, "points": 0.0}, "ADMIN")
    cols = _update_columns(cur)
    assert "[custname]" in cols and "[mobno]" in cols
    assert "city" not in cols and "gstno" not in cols and "inactive" not in cols


def test_a_users_first_web_login_does_not_wipe_the_desktop_profile(monkeypatch):
    monkeypatch.setattr(sqlstore, "_meta", lambda table: FakeMeta())
    cur = _Cur(found=True)
    # db_store.load_users fills only full_name and active (plus the web's own fields, which have no column).
    sqlstore._write_master(cur, "USERS", "RAVI", {"user": "RAVI", "full_name": "Ravi K", "active": True, "role": "Cashier", "pwv": 2}, "RAVI")
    cols = _update_columns(cur)
    for untouched in ("EmailID", "Mobileno", "Department", "DOB", "ChkDOB", "InactiveRmks", "PwdResetDt", "LoginPwd"):
        assert untouched.lower() not in cols, untouched


def test_a_master_that_old_bills_use_cannot_be_deleted(monkeypatch):
    monkeypatch.setattr(sqlstore, "_meta", lambda table: FakeMeta())
    cur = _Cur(found=True, used_by=("TrnDtlSINV",))
    with pytest.raises(ValueError, match="TrnDtlSINV"):
        sqlstore._write_master(cur, "ITEMS", "VIN-750", None, "ADMIN")
    assert not any(s.startswith("DELETE") for s, _ in cur.log)
    unused = _Cur(found=True)
    sqlstore._write_master(unused, "ITEMS", "VIN-750", None, "ADMIN")
    assert any(s.startswith("DELETE FROM dbo.[MstITEM]") for s, _ in unused.log)
