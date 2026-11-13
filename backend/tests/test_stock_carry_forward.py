"""Company Master → Financial Year(s) → Carry forward stock (app/stock_carry_forward.py).

The tests run without a database, so they check what is decided before anything is written: who may ask, which years are valid, and that an
offline database writes nothing. The SQL itself is checked against a real SQL Server by hand (back it up first)."""
from app import store
from conftest import enc

YEARS = [{"start": "2025-04-01", "end": "2026-03-31", "active": False}, {"start": "2026-04-01", "end": "2027-03-31", "active": True}]


def _years(_monkeypatch=None, years=YEARS):
    store.FIN_YEARS[:] = [dict(y) for y in years]   # in place: the routers keep the list; conftest restores it after the test


async def test_preview_needs_an_administrator(manager_client, cashier_client, monkeypatch):
    _years(monkeypatch)
    for c in (manager_client, cashier_client):
        assert (await c.get("/masters/company/fin-years/2026-04-01/carry-forward")).status_code == 403
        assert (await c.post("/masters/company/fin-years/2026-04-01/carry-forward", json={})).status_code == 403


async def test_unknown_year_is_refused(client, monkeypatch):
    _years(monkeypatch)
    assert (await client.get("/masters/company/fin-years/2030-04-01/carry-forward")).status_code == 404


async def test_first_year_has_nothing_to_carry_from(client, monkeypatch):
    _years(monkeypatch)
    res = await client.get("/masters/company/fin-years/2025-04-01/carry-forward")
    assert res.status_code == 409 and "earlier financial year" in res.json()["detail"]


async def test_years_with_a_gap_are_refused(client, monkeypatch):
    _years(monkeypatch, [{"start": "2024-04-01", "end": "2025-03-31", "active": False}, {"start": "2026-04-01", "end": "2027-03-31", "active": True}])
    res = await client.get("/masters/company/fin-years/2026-04-01/carry-forward")
    assert res.status_code == 409 and "consecutive" in res.json()["detail"]


async def test_wrong_or_missing_password_is_refused(client, monkeypatch):
    _years(monkeypatch)
    url = "/masters/company/fin-years/2026-04-01/carry-forward"
    assert (await client.post(url, json={})).status_code == 403
    assert (await client.post(url, json={"admin_password": enc("not-the-password", "admin")})).status_code == 403


async def test_offline_database_writes_nothing(client, monkeypatch):
    _years(monkeypatch)
    assert (await client.get("/masters/company/fin-years/2026-04-01/carry-forward")).status_code == 503
    res = await client.post("/masters/company/fin-years/2026-04-01/carry-forward", json={"admin_password": enc("admin", "admin")})
    assert res.status_code == 503
