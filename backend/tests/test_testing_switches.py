"""Testing aids (Administrator only): Disconnect / Reconnect in About, and Dashboard panels hidden per role
through Field Level Rights."""
from app import db, sqlstore


async def test_disconnect_needs_an_admin_and_a_database(client, cashier_client):
    assert (await cashier_client.post("/utilities/database/disconnect")).status_code == 403
    assert (await client.post("/utilities/database/disconnect")).status_code == 409   # none set up in the tests


def test_a_manual_disconnect_is_not_undone_by_itself(monkeypatch):
    monkeypatch.setattr(db, "READ_ONLY", False)   # the older saving mode
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
