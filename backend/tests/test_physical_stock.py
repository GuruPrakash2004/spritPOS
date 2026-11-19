"""Physical Stock Entry (legacy frmTrnPSE): excess -> purchase bill, shortage -> stock deduction, changed rows only."""


async def _items(client, n=2):
    res = await client.get("/masters/items")
    items = [i for i in res.json() if i["active"] and i["qoh"] >= 5]
    return items[:n]


async def test_needs_a_filter_to_load(client):
    assert (await client.get("/transactions/physical-stock/load")).status_code == 422


async def test_excess_and_shortage_post_documents_and_move_stock(client):
    up, down = await _items(client)
    rows = (await client.get("/transactions/physical-stock/load", params={"items": f"{up['code']},{down['code']}"})).json()
    assert {r["item_code"] for r in rows} == {up["code"], down["code"]}
    body = {"lines": [
        {"item_code": up["code"], "physical_qty": up["qoh"] + 3, "pur_rate": up["purchase_rate"], "mrp": up["mrp"]},
        {"item_code": down["code"], "physical_qty": down["qoh"] - 2, "pur_rate": down["purchase_rate"], "mrp": down["mrp"]},
    ]}
    res = await client.post("/transactions/physical-stock", json=body)
    assert res.status_code == 201, res.text
    doc = res.json()
    by = {l["item_code"]: l for l in doc["lines"]}
    assert by[up["code"]]["diff_qty"] == 3 and by[up["code"]]["pb_no"]
    assert by[down["code"]]["diff_qty"] == -2 and by[down["code"]]["stkded_no"]
    after = {i["code"]: i["qoh"] for i in (await client.get("/masters/items")).json()}
    assert after[up["code"]] == up["qoh"] + 3 and after[down["code"]] == down["qoh"] - 2
    assert (await client.get(f"/transactions/physical-stock/{doc['doc_no']}")).json()["doc_no"] == doc["doc_no"]


async def test_unchanged_rows_are_refused_and_rate_change_alone_is_saved(client):
    (it,) = await _items(client, 1)
    same = {"item_code": it["code"], "physical_qty": it["qoh"], "pur_rate": it["purchase_rate"], "mrp": it["mrp"]}
    assert (await client.post("/transactions/physical-stock", json={"lines": [same]})).status_code == 422
    res = await client.post("/transactions/physical-stock", json={"lines": [{**same, "mrp": it["mrp"] + 5}]})
    assert res.status_code == 201
    line = res.json()["lines"][0]
    assert line["diff_qty"] == 0 and line["old_mrp"] == it["mrp"] and line["mrp"] == it["mrp"] + 5
    now = next(i for i in (await client.get("/masters/items")).json() if i["code"] == it["code"])
    assert now["mrp"] == it["mrp"] + 5


async def test_reduced_quantity_as_sales_needs_a_counter(client):
    (it,) = await _items(client, 1)
    line = {"item_code": it["code"], "physical_qty": it["qoh"] - 1, "pur_rate": it["purchase_rate"], "mrp": it["mrp"]}
    assert (await client.post("/transactions/physical-stock", json={"lines": [line], "reduced_as_sales": True})).status_code == 422


async def test_update_item_stock_validates_then_posts_one_purchase_bill(client):
    (it,) = await _items(client, 1)
    rows = [{"item_code": it["code"], "stock": "4", "rate": ""}, {"item_code": "NOPE", "stock": "1"}, {"item_code": it["code"], "stock": "-2"}]
    check = (await client.post("/utilities/update-item-stock", json={"rows": rows, "dry_run": True})).json()
    assert check["ok"] is False
    assert [r["error"] for r in check["rows"]] == ["", "Item not found", "Invalid Stock"]
    refused = await client.post("/utilities/update-item-stock", json={"rows": rows, "dry_run": False})
    assert refused.status_code == 422                       # not allowed until every row is right
    res = await client.post("/utilities/update-item-stock", json={"rows": rows[:1], "dry_run": False})
    assert res.status_code == 200 and res.json()["imported"] == 1
    now = next(i for i in (await client.get("/masters/items")).json() if i["code"] == it["code"])
    assert now["qoh"] == it["qoh"] + 4


async def test_item_vs_brand_links_and_unlinks(client):
    (it,) = await _items(client, 1)
    brands = (await client.get("/masters/brands")).json()
    assert brands
    code = brands[0]["code"]
    res = await client.post("/masters/items/brands", json={"assignments": [{"item_code": it["code"], "brand": code}]})
    assert res.status_code == 200
    items = (await client.get("/masters/items")).json()
    assert next(i for i in items if i["code"] == it["code"])["brand"] == code
    assert (await client.post("/masters/items/brands", json={"assignments": [{"item_code": it["code"], "brand": "NOPE"}]})).status_code == 422
    assert (await client.post("/masters/items/brands", json={"assignments": [{"item_code": it["code"], "brand": ""}]})).json()["changed"] == 1


async def test_tools_audit_stays_in_memory_when_the_database_is_read_only(client, monkeypatch):
    """No Web_ tables: a Tools run is remembered in memory and the audit list never queries the database."""
    from app import db, sqlstore
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(sqlstore, "_active", True)
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(db, "fetchall", lambda *a, **k: (_ for _ in ()).throw(AssertionError("the database must not be queried")))
    sqlstore.audit_tool("ADMIN", "SQL Query Execution", "select 1", True, 1)
    assert sqlstore.tool_audit_rows()[0]["tool"] == "SQL Query Execution"


async def test_shared_documents_are_read_from_the_database(client, monkeypatch):
    from app import db, sqlstore
    monkeypatch.setattr(sqlstore, "_active", True)
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [{"DocID": 1, "GroupName": " Forms ", "DocFileName": "a.pdf", "Remarks": None}])
    res = await client.get("/utilities/shared-documents")
    assert res.status_code == 200 and res.json() == [{"id": 1, "group": "Forms", "file": "a.pdf", "remarks": ""}]


async def test_update_item_stock_marks_the_wrong_cells(client):
    (it,) = await _items(client, 1)
    rows = [{"item_code": it["code"], "stock": "4"}, {"item_code": "NOPE", "stock": "1"}, {"item_code": it["code"], "stock": "-2"}]
    check = (await client.post("/utilities/update-item-stock", json={"rows": rows, "dry_run": True})).json()
    assert [r["fields"] for r in check["rows"]] == [[], ["code"], ["stock"]]


async def test_update_item_stock_bills_use_their_own_op_number_series_without_gst(client):
    """frmUpdateItemStock posts the opening stock as a purchase bill with prefix OP (26OP00001, 26OP00002 ...): its own series, no GST."""
    import re
    (it,) = await _items(client, 1)
    first = (await client.post("/utilities/update-item-stock", json={"rows": [{"item_code": it["code"], "stock": "3"}], "dry_run": False})).json()
    second = (await client.post("/utilities/update-item-stock", json={"rows": [{"item_code": it["code"], "stock": "2"}], "dry_run": False})).json()
    assert re.fullmatch(r"\d{2}OP00001", first["doc_no"]), first
    assert re.fullmatch(r"\d{2}OP00002", second["doc_no"]), second
    bill = (await client.get(f"/transactions/purchase-bill/{first['doc_no']}")).json()
    assert bill["totals"]["cgst"] == 0 and bill["totals"]["sgst"] == 0
    assert bill["totals"]["net"] == bill["totals"]["basic"]
    ordinary = await client.post("/transactions/purchase-bill", json={"date": "2026-11-19", "supplier": "KSBCL", "supplier_inv_no": "X1", "supplier_inv_date": "2026-11-19",
                                                                         "lines": [{"item_code": it["code"], "bottles": 1}]})
    assert ordinary.status_code == 201, ordinary.text
    assert "OP" not in ordinary.json()["doc_no"]            # the ordinary series is not touched


async def test_an_opening_purchase_bill_is_numbered_in_the_op_series_when_the_user_saves_it(client):
    """Update Item Stock hands its lines to the Purchase Bill screen: nothing is saved until the bill is saved there with opening=true."""
    import re
    (it,) = await _items(client, 1)
    preview = (await client.get("/transactions/next-number", params={"kind": "opening_purchase_bill"})).json()
    assert re.fullmatch(r"\d{2}OP\d{5}", preview["doc_no"]), preview
    body = {"date": "2026-11-19", "supplier": "KSBCL", "supplier_inv_no": "INV-9", "supplier_inv_date": "2026-11-19", "opening": True,
            "lines": [{"item_code": it["code"], "bottles": 3, "rate": 100}]}
    res = await client.post("/transactions/purchase-bill", json=body)
    assert res.status_code == 201, res.text
    bill = res.json()
    assert bill["doc_no"] == preview["doc_no"]
    assert bill["totals"]["cgst"] == 0 and bill["totals"]["net"] == bill["totals"]["basic"] == 300
    assert "opening" not in bill
    assert (await client.get("/transactions/next-number", params={"kind": "opening_purchase_bill"})).json()["doc_no"] != preview["doc_no"]
    edited = await client.put(f"/transactions/purchase-bill/{bill['doc_no']}", json=body | {"lines": [{"item_code": it["code"], "bottles": 4, "rate": 100}]})
    assert edited.status_code == 200 and edited.json()["totals"]["cgst"] == 0   # still GST-free when edited
