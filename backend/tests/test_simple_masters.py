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
    "banks": {"code": "SBI", "name": "State Bank", "ifsc": "SBIN0000001", "account_no": "123"},
    "holidays": {"code": "2026-01-26", "name": "Republic Day"},
}


def test_every_spec_has_a_test_body():
    assert {s["path"] for s in SPECS} == set(BODIES)


@pytest.mark.parametrize("path", sorted(BODIES))
async def test_create_read_update_delete(client, path):
    body = BODIES[path]
    assert (await client.post(f"/masters/{path}", json=body)).status_code == 201
    assert (await client.post(f"/masters/{path}", json=body)).status_code == 409   # same code twice
    got = await client.get(f"/masters/{path}/{body['code']}")
    assert got.status_code == 200 and got.json()["name"] == body["name"]
    changed = await client.put(f"/masters/{path}/{body['code']}", json={**body, "name": "Changed"})
    assert changed.status_code == 200 and changed.json()["name"] == "Changed"
    assert (await client.delete(f"/masters/{path}/{body['code']}")).status_code == 204


async def test_holiday_needs_a_real_date_and_tax_a_sane_percentage(client):
    assert (await client.post("/masters/holidays", json={"code": "tomorrow", "name": "x"})).status_code == 422
    assert (await client.post("/masters/taxes", json={"code": "BAD", "name": "bad", "cgst_per": 140})).status_code == 422


async def test_cashier_cannot_add_a_state(cashier_client):
    assert (await cashier_client.post("/masters/states", json=BODIES["states"])).status_code == 403
