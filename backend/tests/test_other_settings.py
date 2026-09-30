"""Global Settings → Other Settings (app/doc_settings.py): Transaction Settings, document numbers, Menu Sequence
and Bulk Item Import Fields, each checked against what it changes."""
from datetime import date, timedelta

from app import store

TODAY = date.today().isoformat()
YESTERDAY = (date.today() - timedelta(days=1)).isoformat()


def _bill(day=TODAY, lines=None):
    return {"date": day, "counter": "counter10", "customer": "CASH", "salesperson": "GURU",
            "lines": lines or [{"item_code": "VIN-750", "bottles": 1}]}


async def _set(client, kind, **values):
    res = await client.put(f"/utilities/doc-settings/{kind}", json=values)
    assert res.status_code == 200, res.text
    return res.json()


async def test_duplicate_items_and_document_date(client):
    two = [{"item_code": "VIN-750", "bottles": 1}, {"item_code": "VIN-750", "bottles": 1}]
    assert (await client.post("/transactions/sales-invoice", json=_bill(lines=two))).status_code == 201
    await _set(client, "sales_invoice", allow_duplicate_items=False, allow_edit_date=False)
    res = await client.post("/transactions/sales-invoice", json=_bill(lines=two))
    assert res.status_code == 422 and "Duplicate Item" in res.json()["detail"]
    res = await client.post("/transactions/sales-invoice", json=_bill(day=YESTERDAY))
    assert res.status_code == 422 and "dated today" in res.json()["detail"]
    assert (await client.post("/transactions/sales-invoice", json=_bill())).status_code == 201


async def test_old_bills_cannot_be_cancelled_after_edit_days(client):
    old = (await client.post("/transactions/sales-invoice", json=_bill(day=(date.today() - timedelta(days=3)).isoformat()))).json()
    await _set(client, "sales_invoice", edit_days=2)
    res = await client.post(f"/transactions/sales-invoice/{old['doc_no']}/cancel", json={"remarks": "late"})
    assert res.status_code == 409 and "2 day" in res.json()["detail"]


async def test_purchase_bill_copy_from_and_rate_update(client):
    await _set(client, "purchase_bill", allow_copy_from=False, update_rate_from_pb=True)
    sup = next(iter(store.SUPPLIERS))
    body = {"date": TODAY, "supplier": sup, "supplier_inv_no": "X1", "supplier_inv_date": TODAY, "supplier_inv_value": 0,
            "lines": [{"item_code": "VIN-750", "bottles": 1, "rate": 777}]}
    res = await client.post("/transactions/purchase-bill", json={**body, "copy_from_indent": "26/PI-0000001", "lines": []})
    assert res.status_code == 422 and "Copy From" in res.json()["detail"]
    res = await client.post("/transactions/purchase-bill", json=body)
    assert res.status_code == 201, res.text
    assert store.ITEMS["VIN-750"]["purchase_rate"] == 777


async def test_document_number_prefix_and_reset(client):
    await client.put("/utilities/numbering/stock_deduction", json={"prefix": "BRK"})
    doc = (await client.post("/transactions/stock-deduction", json={"date": TODAY, "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()
    assert doc["doc_no"].startswith(f"{store.FY_PREFIX}/BRK-")
    used = int(doc["doc_no"].rsplit("-", 1)[1])
    res = await client.put("/utilities/numbering/stock_deduction", json={"next_number": used})
    assert res.status_code == 409 and "already used" in res.json()["detail"]
    res = await client.put("/utilities/numbering/stock_deduction", json={"next_number": used + 100})
    assert res.status_code == 200 and res.json()["next_number"] == used + 100
    doc = (await client.post("/transactions/stock-deduction", json={"date": TODAY, "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()
    assert doc["doc_no"].endswith(f"{used + 100:07d}")
    assert (await client.put("/utilities/numbering/sales_invoice", json={"prefix": "X"})).status_code == 409
    assert (await client.put("/utilities/numbering/stock_deduction", json={"prefix": "BR-K"})).status_code == 422


async def test_menu_sequence(client, cashier_client):
    res = await client.put("/utilities/menu", json={"hidden": ["Stock Ageing"], "codes": {"Sales Invoice": "101"}})
    assert res.status_code == 200
    assert (await cashier_client.get("/settings/menu")).json() == {"hidden": ["Stock Ageing"], "codes": {"Sales Invoice": "101"}}
    assert (await client.put("/utilities/menu", json={"hidden": ["Admin Master"]})).status_code == 422
    assert (await client.put("/utilities/menu", json={"codes": {"A": "1", "B": "1"}})).status_code == 422
    assert (await client.put("/utilities/menu", json={"codes": {"A": "x1"}})).status_code == 422


async def test_import_fields_limit_what_an_import_overwrites(client):
    await client.put("/utilities/import-fields", json={"fields": ["mrp"]})
    row = dict(store.ITEMS["VIN-750"], mrp=999, name="Changed name")
    res = await client.post("/masters/items/import?overwrite=true", json=[row])
    assert res.status_code == 200
    assert store.ITEMS["VIN-750"]["mrp"] == 999 and store.ITEMS["VIN-750"]["name"] != "Changed name"
    assert (await client.put("/utilities/import-fields", json={"fields": ["nope"]})).status_code == 422


async def test_other_settings_are_admin_only_but_readable_by_screens(cashier_client):
    assert (await cashier_client.put("/utilities/doc-settings/sales_invoice", json={})).status_code == 403
    docs = (await cashier_client.get("/settings/documents")).json()
    assert docs["sales_invoice"]["prints"] and not docs["purchase_bill"]["prints"]
