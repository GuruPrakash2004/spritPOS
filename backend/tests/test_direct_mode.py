"""Direct mode (no Web_ tables): a bill is written into the desktop's own tables and nothing else; what has no desktop table is refused and undone."""
import pytest

from app import db, gsettings, sqlstore, store
from test_sqlstore import FakeCursor, FakeMeta, _sell, _tables, sql  # noqa: F401  (the fake cursor and its fixture)


@pytest.fixture(autouse=True)
def direct(monkeypatch):
    monkeypatch.setattr(db, "USE_WEB_TABLES", False)


def _no_web_table(log):
    touched = [s for s, _ in log if "Web_" in s]
    assert not touched, touched


async def test_a_sale_goes_to_the_desktops_tables_and_to_no_web_table(client, sql):
    log, _ = sql
    res = await _sell(client)
    assert res.status_code == 201, res.text
    inserted = _tables(log)
    for t in ("dbo.[TrnHdrSINV]", "dbo.[TrnDtlSINV]", "dbo.[UtlStockList]"):
        assert t in inserted, (t, inserted)
    _no_web_table(log)


async def test_cancelling_it_updates_the_header_and_takes_its_stock_rows_out(client, sql):
    log, _ = sql
    doc_no = (await _sell(client)).json()["doc_no"]
    log.clear()
    assert (await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "wrong item"})).status_code == 200
    assert any(s.startswith("UPDATE dbo.[TrnHdrSINV]") for s, _ in log)
    assert any("DELETE FROM dbo.UtlStockList" in s for s, _ in log)
    _no_web_table(log)


async def test_a_purchase_bill_goes_to_the_desktops_tables_and_to_no_web_table(client, sql):
    log, _ = sql
    res = await client.post("/transactions/purchase-bill", json={
        "date": "2026-10-31", "supplier": next(iter(store.SUPPLIERS)), "supplier_inv_no": "X1", "supplier_inv_date": "2026-10-31",
        "supplier_inv_value": 0, "lines": [{"item_code": "VIN-750", "cases": 0, "bottles": 6}]})
    assert res.status_code == 201, res.text
    inserted = _tables(log)
    for t in ("dbo.[TrnHdrPB]", "dbo.[TrnDtlPB]", "dbo.[UtlStockList]"):
        assert t in inserted, (t, inserted)
    _no_web_table(log)


async def test_a_held_bill_is_kept_in_memory_and_writes_nothing(client, sql):
    log, _ = sql
    res = await client.post("/transactions/sales-invoice", json={
        "date": "2026-10-31", "counter": "counter10", "customer": "CASH", "payment_type": "CASH", "salesperson": "GURU", "hold": True,
        "lines": [{"item_code": "VIN-750", "cases": 0, "bottles": 1}]})
    assert res.status_code == 201 and res.json()["doc_no"].startswith("HOLD-")
    assert [t for t in _tables(log) if "SINV" in t] == []
    _no_web_table(log)


async def test_a_setting_has_no_desktop_table_so_the_change_is_refused_and_undone(client, sql, monkeypatch):
    log, _ = sql
    reloaded = []
    monkeypatch.setattr(sqlstore, "reload", lambda: reloaded.append(True))
    res = await client.put("/settings", json={"loyalty": {"points_per_rupee": 0.05, "alert_when_points_above": 500, "min_bill_amount": 1000}})
    assert res.status_code == 503 and "no desktop table" in res.json()["detail"], res.text
    assert reloaded == [True]
    _no_web_table(log)


def test_the_desktops_global_settings_are_read(monkeypatch):
    rows = [{"SetName": "FreezeDate", "SetValue": "30 Jun 2026"}, {"SetName": "SPID", "SetValue": "y"}, {"SetName": "Colour", "SetValue": "blue"}]
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): rows)
    assert gsettings.load_from_db() == 3   # a setting only the desktop has is read too
    assert gsettings.value("FreezeDate") == "2026-06-30" and gsettings.yes("SPID")


async def test_a_global_setting_edit_is_written_to_the_desktop_table(client, monkeypatch):
    """Direct mode: the edit goes to UtlGSettings (update, else insert), dates in the desktop's '30 Jun 2026' form."""
    from app import gsettings
    sent = []

    class Cur:
        def execute(self, sql, params=()):
            sent.append((sql.split()[0], params))
            return 1

    class Tx:
        def __enter__(self): return Cur()
        def __exit__(self, *a): return False

    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "transaction", lambda: Tx())
    gsettings.seed()
    row = gsettings.store.GSETTINGS["FreezeDate"]
    gsettings.store.GSETTINGS["FreezeDate"] = {**row, "kind": "date", "value": "2026-06-30"}
    gsettings.save_to_db("FreezeDate")
    assert sent[0][0] == "UPDATE" and sent[0][1][0] == "30 Jun 2026" and sent[0][1][-1] == "FreezeDate"


async def test_a_customer_needs_a_gstin_and_a_currency_except_the_cash_customer(client):
    """The desktop's Customer Master marks GSTNO and CURID mandatory (Doc Designer); the walk-in CASH customer is exempt."""
    base = {"code": "C8", "name": "Asha Traders"}
    r = await client.post("/masters/customers", json=base)
    assert r.status_code == 422 and "GSTIN is mandatory" in r.json()["detail"]
    r = await client.post("/masters/customers", json={**base, "gstin": "29ABCDE1234F1Z5"})
    assert r.status_code == 422 and "Currency is mandatory" in r.json()["detail"]
    r = await client.post("/masters/customers", json={**base, "gstin": "29ABCDE1234F1Z5", "currency": "Rupees"})
    assert r.status_code == 201 and r.json()["gstin"] == "29ABCDE1234F1Z5" and r.json()["currency"] == "Rupees"
    assert (await client.post("/masters/customers", json={**base, "gstin": "bad", "currency": "Rupees", "code": "C9"})).status_code == 422


async def test_the_desktops_dash_counts_as_a_customer_gstin(client):
    r = await client.post("/masters/customers", json={"code": "C10", "name": "Old Customer", "gstin": "-", "currency": "Rupees"})
    assert r.status_code == 201 and r.json()["gstin"] == "-"


def test_a_password_change_is_written_to_the_desktop_user_table(monkeypatch):
    """Direct mode: change-password writes MstUser.LoginPwd in the exe's cipher (so the desktop and a restart agree), and refuses what the column can't hold."""
    import pytest
    from fastapi import HTTPException
    from app import legacy_crypto
    from app.routers import auth
    sent = []

    class Cur:
        def execute(self, sql, params=()):
            sent.append((sql.split()[0], params))
            return 1

    class Tx:
        def __enter__(self): return Cur()
        def __exit__(self, *a): return False

    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "transaction", lambda: Tx())
    auth._write_desktop_password("ADMIN", "New@Pass123")
    assert sent[0][0] == "UPDATE" and legacy_crypto.decrypt(sent[0][1][0]) == "New@Pass123" and sent[0][1][2] == "ADMIN"
    with pytest.raises(HTTPException):
        auth._write_desktop_password("ADMIN", "x" * 32)
    with pytest.raises(HTTPException):
        auth._write_desktop_password("ADMIN", "Pässword1!")


async def test_held_bills_are_kept_in_a_file_and_come_back(client, monkeypatch, tmp_path):
    """Direct mode: a held bill is written to a file beside the backend (never to the database) and restored after a restart."""
    from app import held_bills
    monkeypatch.setenv("HELD_BILLS_DIR", str(tmp_path))
    monkeypatch.setattr(held_bills, "_last", "")
    store.DOCS["sales_invoice"]["HOLD-0007"] = {"doc_no": "HOLD-0007", "status": "HOLD", "date": "2026-10-31", "lines": [{"item_code": "X", "qty": 1}], "customer": "CASH"}
    assert held_bills.dump_if_changed() is True and held_bills.dump_if_changed() is False   # written once, then unchanged
    files = list(tmp_path.glob("held_bills_*.json"))
    assert len(files) == 1 and "HOLD-0007" in files[0].read_text()
    del store.DOCS["sales_invoice"]["HOLD-0007"]
    store._seq["hold"].value = 1
    assert held_bills.restore() == 1 and store.DOCS["sales_invoice"]["HOLD-0007"]["lines"][0]["item_code"] == "X"
    assert store._seq["hold"].value == 8   # numbering goes on after the highest held bill
    del store.DOCS["sales_invoice"]["HOLD-0007"]   # released: the file follows
    assert held_bills.dump_if_changed() is True and "HOLD-0007" not in files[0].read_text()
