"""Bulk Master Update: one Item Master field set on many items."""


async def _codes(client, n=2):
    return [i["code"] for i in (await client.get("/masters/items")).json()][:n]


async def test_sets_a_field_on_the_chosen_items(client):
    codes = await _codes(client)
    res = await client.post("/masters/items/bulk-update", json={"item_codes": codes, "field": "hsn_code", "value": "22083011"})
    assert res.status_code == 200 and res.json()["changed"] == len(codes)
    for c in codes:
        assert (await client.get(f"/masters/items/{c}")).json()["hsn_code"] == "22083011"


async def test_refuses_bad_values_and_changes_nothing(client):
    codes = await _codes(client)
    before = (await client.get(f"/masters/items/{codes[0]}")).json()["mrp"]
    assert (await client.post("/masters/items/bulk-update", json={"item_codes": codes, "field": "mrp", "value": -5})).status_code == 422
    assert (await client.post("/masters/items/bulk-update", json={"item_codes": codes, "field": "brand", "value": "NO-SUCH"})).status_code == 422
    assert (await client.post("/masters/items/bulk-update", json={"item_codes": ["NOPE"], "field": "roq", "value": 3})).status_code == 404
    assert (await client.get(f"/masters/items/{codes[0]}")).json()["mrp"] == before


async def test_cashier_cannot_bulk_update(cashier_client):
    res = await cashier_client.post("/masters/items/bulk-update", json={"item_codes": ["X"], "field": "roq", "value": 1})
    assert res.status_code == 403
