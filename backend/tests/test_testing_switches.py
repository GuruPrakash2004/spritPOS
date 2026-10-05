"""Testing aids (Administrator only): the header's Test data switch, Disconnect / Reconnect in About, and
Dashboard panels hidden per role through Field Level Rights."""
from app import db, sqlstore, store


async def test_test_data_switch_is_admin_only_and_shows_in_about(client, cashier_client):
    assert (await cashier_client.post("/utilities/test-data", json={"on": True})).status_code == 403
    res = await client.post("/utilities/test-data", json={"on": True})
    assert res.status_code == 200 and res.json()["data_mode"] == "test"
    assert (await client.get("/auth/me")).json()["data_mode"] == "test"
    assert "VIN-750" in store.ITEMS and "ADMIN" in store.USERS   # sample business data, the same users
    res = await client.post("/utilities/test-data", json={"on": False})
    assert res.json()["data_mode"] == "none"   # no database in the tests: nothing live to go back to
    assert not sqlstore.test_data()


async def test_bills_made_with_test_data_dont_use_up_live_numbers(client):
    before = store._seq["sales_invoice"].value
    await client.post("/utilities/test-data", json={"on": True})
    res = await client.post("/transactions/sales-invoice", json={
        "date": str(__import__("datetime").date.today()), "counter": "counter10", "salesperson": "GURU",
        "customer": "CASH", "payment_type": "CASH", "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    assert res.status_code == 201, res.text
    await client.post("/utilities/test-data", json={"on": False})
    assert store._seq["sales_invoice"].value == before


async def test_test_data_keeps_saving_off(client, monkeypatch):
    saved = []
    monkeypatch.setattr(sqlstore, "_active", True)
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(sqlstore, "reload", lambda: saved.append("reload"))
    await client.post("/utilities/test-data", json={"on": True})
    assert not sqlstore.enabled() and sqlstore.writes_blocked() is None
    sqlstore.save("ADMIN")   # nothing is written (would raise SaveFailed without a real database)
    await client.post("/utilities/test-data", json={"on": False})
    assert saved == ["reload"]   # back to the live data


async def test_disconnect_needs_an_admin_and_a_database(client, cashier_client):
    assert (await cashier_client.post("/utilities/database/disconnect")).status_code == 403
    assert (await client.post("/utilities/database/disconnect")).status_code == 409   # none set up in the tests


def test_a_manual_disconnect_is_not_undone_by_itself(monkeypatch):
    monkeypatch.setattr(db, "configured", lambda: True)
    tried = []
    monkeypatch.setattr(db, "reconnect", lambda: tried.append(1) or False)
    db.disconnect()
    try:
        assert "Disconnected" in sqlstore.writes_blocked()
        assert not tried
    finally:
        db.MANUAL_OFFLINE = False


async def test_dashboard_panels_can_be_hidden_per_role(client, cashier_client):
    fr = (await client.get("/admin/field-rights")).json()
    assert "restock" in fr["masters"]["dashboard"]
    bad = await client.put("/admin/field-rights/Cashier", json={"masters": {"dashboard": {"restock": "readonly"}}})
    assert bad.status_code == 422
    ok = await client.put("/admin/field-rights/Cashier", json={"masters": {"dashboard": {"restock": "hidden", "stock_movement": "hidden"}}})
    assert ok.status_code == 200, ok.text
    assert (await cashier_client.get("/auth/me")).json()["field_rights"]["dashboard"]["restock"] == "hidden"
    assert (await cashier_client.get("/reports/restock")).status_code == 403
    assert (await cashier_client.get("/reports/stock-movement")).status_code == 403
    assert (await client.get("/reports/restock")).status_code == 200   # the Administrator sees everything


async def test_in_test_mode_users_and_settings_cant_be_changed(client):
    await client.post("/utilities/test-data", json={"on": True})
    res = await client.put("/admin/field-rights/Cashier", json={"masters": {}})
    assert res.status_code == 409 and "Test data" in res.json()["detail"]
    await client.post("/utilities/test-data", json={"on": False})
    assert (await client.put("/admin/field-rights/Cashier", json={"masters": {}})).status_code == 200


async def test_test_data_cant_be_switched_off_without_the_database(client, monkeypatch):
    await client.post("/utilities/test-data", json={"on": True})
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "is_db_available", lambda: False)
    res = await client.post("/utilities/test-data", json={"on": False})
    assert res.status_code == 409 and sqlstore.test_data()   # nobody is locked out by an empty reload


def test_web_sync_never_sends_once_test_data_is_on(monkeypatch):
    from app import websync
    import datetime
    monkeypatch.setattr(websync, "configured", lambda: True)
    monkeypatch.setattr(websync, "_url_problem", lambda: None)
    calls = iter([True, False])
    monkeypatch.setattr(websync, "_live", lambda: next(calls, False))
    sent = []
    monkeypatch.setattr(websync, "push", lambda day: sent.append(day))
    import pytest
    with pytest.raises(websync.SyncFailed):
        websync.sync([datetime.date.today()])
    assert sent == []
