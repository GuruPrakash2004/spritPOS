"""The simple masters described in app/master_specs.py (groups, state, currency, location, tax, bank, holiday)."""
import pytest

from app.master_specs import SPECS

BODIES = {
    "supplier-groups": {"code": "KSBCL", "name": "KSBCL group"},
    "customer-groups": {"code": "VIP", "name": "VIP customers"},
    "states": {"code": "KA", "name": "Karnataka"},
    "currencies": {"code": "INR", "name": "Rupees", "decimals": "2"},
    "locations": {"code": "GODOWN", "name": "Godown"},
    "taxes": {"code": "GST18", "name": "GST 18", "tax_type": "LOCAL", "cgst_per": 9, "sgst_per": 9, "igst_per": 0},
    "banks": {"code": "SBI", "name": "State Bank", "display": "SBI", "ifsc": "SBIN0000001", "account_no": "123456789012"},
    "holidays": {"code": "2026-08-15", "name": "Independence Day"},
    "state-city-country": {"code": "Mysuru", "name": "Karnataka", "country": "INDIA"},   # the state must be in State Master
}


def test_every_spec_has_a_test_body():
    assert {s["path"] for s in SPECS} == set(BODIES)


@pytest.mark.parametrize("path", sorted(BODIES))
async def test_create_read_update_delete(client, path):
    body = BODIES[path]
    if path == "state-city-country":
        await client.post("/masters/states", json={"code": "KA", "name": "Karnataka"})
    assert (await client.post(f"/masters/{path}", json=body)).status_code == 201
    assert (await client.post(f"/masters/{path}", json=body)).status_code == 409   # same code twice
    got = await client.get(f"/masters/{path}/{body['code']}")
    assert got.status_code == 200 and got.json()["name"] == body["name"]
    new_name = "Karnataka" if path == "state-city-country" else "Changed"   # a city's "name" is its state, which must exist
    changed = await client.put(f"/masters/{path}/{body['code']}", json={**body, "name": new_name, "country": "Bharat"} if path == "state-city-country" else {**body, "name": new_name})
    assert changed.status_code == 200 and changed.json()["name"] == new_name
    assert (await client.delete(f"/masters/{path}/{body['code']}")).status_code == 204


async def test_holiday_needs_a_real_date_and_tax_a_sane_percentage(client):
    assert (await client.post("/masters/holidays", json={"code": "tomorrow", "name": "x"})).status_code == 422
    assert (await client.post("/masters/taxes", json={"code": "BAD", "name": "bad", "cgst_per": 140})).status_code == 422


async def test_cashier_cannot_add_a_state(cashier_client):
    assert (await cashier_client.post("/masters/states", json=BODIES["states"])).status_code == 403


# ─── frmMstHoliday / frmMstGeneral / frmMstUOM rules ───

async def test_holiday_rules_from_the_exe(client):
    out = await client.post("/masters/holidays", json={"code": "2020-01-01", "name": "Old"})
    assert out.status_code == 422 and "financial year" in out.json()["detail"]
    num = await client.post("/masters/holidays", json={"code": "2026-09-01", "name": "123"})
    assert num.status_code == 422 and "in words" in num.json()["detail"]
    assert (await client.post("/masters/holidays", json={"code": "2026-09-01", "name": "O'Day"})).json()["name"] == "O`Day"   # stringReplace
    moved = await client.put("/masters/holidays/2026-09-01", json={"code": "2026-09-02", "name": "O`Day"})
    assert moved.status_code == 422 and "cannot be changed" in moved.json()["detail"]


async def test_general_masters_turn_an_apostrophe_into_a_back_quote(client):
    res = await client.post("/masters/brands", json={"code": "B1", "name": "Mc'Dowell"})
    assert res.json()["name"] == "Mc`Dowell"


async def test_uom_decimals_and_inactive_flag_are_kept(client):
    res = await client.post("/masters/uoms", json={"code": "DZ", "name": "Dozen", "decimals": 2})
    assert res.status_code == 201 and res.json()["decimals"] == 2 and res.json()["active"] is True
    assert (await client.post("/masters/uoms", json={"code": "DZ2", "name": "x", "decimals": -1})).status_code == 422


async def test_a_brand_in_use_cannot_be_edited(client):
    from app import store
    used = next(c for c in store.BRANDS if any(i["brand"] == c for i in store.ITEMS.values()))
    res = await client.put(f"/masters/brands/{used}", json={"code": used, "name": "Renamed"})
    assert res.status_code == 409 and "Editing is not available" in res.json()["detail"]


async def test_arrange_slno_needs_the_database_and_a_known_master(client):
    assert (await client.get("/masters/item-groups/order")).status_code == 503
    assert (await client.post("/masters/packages/order", json={"ids": [1]})).status_code == 503
    assert (await client.get("/masters/uoms/order")).status_code == 404


async def test_currency_needs_a_decimal_name_and_keeps_its_exchange_rate(client):
    res = await client.post("/masters/currencies", json={"code": "USD", "name": "Dollar", "decimals": ""})
    assert res.status_code == 422 and "Decimal Name" in res.json()["detail"]
    ok = await client.post("/masters/currencies", json={"code": "USD", "name": "Dollar", "decimals": "Cents", "exchange_rate": 83.5})
    assert ok.status_code == 201 and ok.json()["exchange_rate"] == 83.5


# ─── frmMstParty / frmMstSCC ───

async def test_state_city_country_needs_a_known_state(client):
    bad = await client.post("/masters/state-city-country", json={"code": "Hubli", "name": "Nowhere", "country": "INDIA"})
    assert bad.status_code == 422 and "State Not found" in bad.json()["detail"]


async def _scc(client):
    await client.post("/masters/states", json={"code": "KA", "name": "Karnataka"})
    await client.post("/masters/state-city-country", json={"code": "Mysuru", "name": "Karnataka", "country": "INDIA"})


PARTY = {"code": "PX1", "name": "Party One", "gstin": "29ABCDE1234F1Z5", "currency": "Rupees", "extras": {}}


async def test_party_screen_rules_from_the_exe(client):
    await _scc(client)
    no_address = await client.post("/masters/customers", json=PARTY)
    assert no_address.status_code == 422 and "first line address" in no_address.json()["detail"]
    full = {"BAdd1": "1 Main Road", "City": "Mysuru", "State": "Karnataka", "Country": "INDIA"}
    bad_city = await client.post("/masters/customers", json={**PARTY, "extras": {**full, "City": "Atlantis"}})
    assert bad_city.status_code == 422 and "City Not found" in bad_city.json()["detail"]
    no_state = await client.post("/masters/customers", json={**PARTY, "extras": {**full, "State": ""}})
    assert no_state.status_code == 422 and "State" in no_state.json()["detail"]
    bad_group = await client.post("/masters/customers", json={**PARTY, "extras": {**full, "CustGroupID": "NOPE"}})
    assert bad_group.status_code == 422 and "Group Code" in bad_group.json()["detail"]
    blank_group = await client.post("/masters/customers", json={**PARTY, "extras": {**full, "CustGroupID": ""}})
    assert blank_group.status_code == 422 and "Group Code" in blank_group.json()["detail"]
    # the same name again is refused on Add
    ok = await client.post("/masters/customers", json={"code": "PX2", "name": "Party Two", "gstin": "-", "currency": "Rupees"})
    assert ok.status_code == 201
    dup = await client.post("/masters/customers", json={"code": "PX3", "name": "party two", "gstin": "-", "currency": "Rupees"})
    assert dup.status_code == 409 and "Name already exists" in dup.json()["detail"]


async def test_similar_codes_work_for_every_master(client):
    assert [r["code"] for r in (await client.get("/masters/customers/similar", params={"code": "c-a-s-h"})).json()] == ["CASH"]
    assert (await client.get("/masters/nope/similar", params={"code": "x"})).status_code == 404
