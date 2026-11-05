"""Sales Invoice's Quick Selection (MstQuickSelItems). The tests have no database: reading gives no sets, writing is refused, and the
sets are kept only in the shop's table, never in memory."""


async def test_no_database_means_no_sets(client):
    res = await client.get("/quick-selection")
    assert res.status_code == 200
    assert res.json() == {"sets": []}


async def test_saving_needs_the_database(client):
    codes = list(__import__("app.store", fromlist=["ITEMS"]).ITEMS)[:1]
    res = await client.put("/quick-selection/FAST", json={"codes": codes})
    assert res.status_code in (422, 503)   # no item has a database id here / the database is offline


async def test_delete_needs_the_database(client):
    assert (await client.delete("/quick-selection/FAST")).status_code == 503


async def test_a_quick_selection_needs_a_login(anon_client):
    assert (await anon_client.get("/quick-selection")).status_code == 401


async def test_item_templates_behave_like_quick_selections(client, anon_client):
    assert (await client.get("/item-templates")).json() == {"sets": []}
    assert (await client.delete("/item-templates/FAST")).status_code == 503
    assert (await anon_client.get("/item-templates")).status_code == 401
