"""Master CRUD (the shared masters.crud() factory), shown with the UOM Master."""
async def test_uom_create_read_update_delete(client):
    created = await client.post("/masters/uoms", json={"code": "CS", "name": "Case"})
    assert created.status_code == 201

    got = await client.get("/masters/uoms/CS")
    assert got.status_code == 200
    assert got.json()["name"] == "Case"

    updated = await client.put("/masters/uoms/CS", json={"code": "CS", "name": "Carton"})
    assert updated.status_code == 200
    assert updated.json()["name"] == "Carton"

    deleted = await client.delete("/masters/uoms/CS")
    assert deleted.status_code == 204
    assert (await client.get("/masters/uoms/CS")).status_code == 404


async def test_store_is_reset_between_tests(client):
    # The UOM created in the test above must not exist here (see reset_store in conftest.py).
    assert (await client.get("/masters/uoms/CS")).status_code == 404


async def test_cashier_cannot_delete_an_item(cashier_client):
    res = await cashier_client.delete("/masters/items/VIN-750")

    assert res.status_code == 403
    assert (await cashier_client.get("/masters/items/VIN-750")).status_code == 200


NEW_ITEM = {"code": "TEST-1", "name": "TEST RUM 750", "group": "RUM", "brand": "Old Monk", "package": "750ml",
            "bottles_per_case": 12, "mrp": 500, "purchase_rate": 400, "qoh": 10}


async def test_admin_can_delete_an_unused_item(client):
    assert (await client.post("/masters/items", json=NEW_ITEM)).status_code == 201

    assert (await client.delete("/masters/items/TEST-1")).status_code == 204


async def test_item_on_bills_cannot_be_deleted(client):
    res = await client.delete("/masters/items/VIN-750")

    assert res.status_code == 409
    assert "inactive" in res.json()["detail"]


async def test_admin_can_update_company(client):
    company = (await client.get("/masters/company")).json()

    res = await client.put("/masters/company", json=company | {"email": "shop@example.com"})

    assert res.status_code == 200
    assert res.json()["email"] == "shop@example.com"


async def test_cashier_cannot_update_company(cashier_client):
    company = (await cashier_client.get("/masters/company")).json()

    assert (await cashier_client.put("/masters/company", json=company)).status_code == 403


async def test_supplier_keeps_address_and_contact_fields(client):
    supplier = {"code": "ABC", "name": "ABC TRADERS", "alias": "ABC", "address": "12 MG Road",
                "city": "Bengaluru", "pin": "560001", "state": "Karnataka", "phone": "9900000000",
                "gstin": "29ABCDE1234F1Z5"}
    assert (await client.post("/masters/suppliers", json=supplier)).status_code == 201

    await client.put("/masters/suppliers/ABC", json=supplier | {"phone": "9811111111"})
    saved = (await client.get("/masters/suppliers/ABC")).json()

    assert saved["address"] == "12 MG Road"
    assert saved["phone"] == "9811111111"
    assert saved["currency"] == "Rupees"


async def test_seed_suppliers_get_defaults_for_new_fields(client):
    rnd = (await client.get("/masters/suppliers/RND")).json()

    assert rnd["name"] == "R&D DISTRIBUTORS"
    assert rnd["address"] == ""
    assert rnd["country"] == "INDIA"


async def test_supplier_list_survives_odd_db_rows(client):
    # Rows loaded from MstSUPP can have an empty or very long name; the list must still load.
    from app import store
    store.SUPPLIERS["X1"] = {"code": "X1", "name": "", "gstin": "", "city": "", "active": True}
    store.SUPPLIERS["X2"] = {"code": "X2", "name": "N" * 300, "gstin": "", "city": "", "active": True}

    res = await client.get("/masters/suppliers")

    assert res.status_code == 200
    assert {"X1", "X2"} <= {s["code"] for s in res.json()}


async def test_package_with_zero_litres_from_the_desktop_still_lists(client):
    # Desktop MstPKG rows can hold LTRPERBOT 0; listing must not fail, but saving one with 0 is refused.
    from app import store
    store.PACKAGES["NA"] = {"code": "NA", "litres_per_bottle": 0.0}
    res = await client.get("/masters/packages")
    assert res.status_code == 200 and any(p["code"] == "NA" for p in res.json())
    res = await client.post("/masters/packages", json={"code": "ZERO", "litres_per_bottle": 0})
    assert res.status_code == 422
