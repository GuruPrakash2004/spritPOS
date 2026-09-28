"""/reports/restock: the Dashboard's "Restock immediately" ranking (in-memory mode, no DB)."""
from datetime import date


async def restock(client, **params) -> list[dict]:
    res = await client.get("/reports/restock", params=params)
    assert res.status_code == 200, res.text
    return res.json()


async def test_rows_have_the_fields_the_dashboard_reads(client):
    rows = await restock(client, days=15)

    assert rows
    expected = {"item_code", "item_name", "qoh", "avg_daily", "days_left", "status",
                "needs_restock", "priority", "suggested_qty", "source"}
    assert expected <= rows[0].keys()
    assert {r["source"] for r in rows} == {"memory"}


async def test_limit_cuts_the_list(client):
    assert len(await restock(client, days=15, limit=3)) == 3


async def test_days_must_be_in_range(client):
    res = await client.get("/reports/restock", params={"days": 0})

    assert res.status_code == 422


async def test_selling_out_an_item_makes_it_urgent(client):
    code = "KF-650"   # the seed item with the most stock left
    rank_before = [r["item_code"] for r in await restock(client, days=15)].index(code)
    item = (await client.get(f"/masters/items/{code}")).json()
    sale = {
        "date": date.today().isoformat(), "counter": "counter10", "salesperson": "GURU",
        "lines": [{"item_code": code, "bottles": int(item["qoh"])}],
    }
    assert (await client.post("/transactions/sales-invoice", json=sale)).status_code == 201

    rows = await restock(client, days=15)
    row = next(r for r in rows if r["item_code"] == code)

    assert row["qoh"] == 0
    assert row["status"] == "OUT_OF_STOCK"
    assert row["needs_restock"] is True
    assert row["days_left"] == 0
    assert row["suggested_qty"] >= item["roq"]
    assert rows.index(row) < rank_before


async def test_items_needing_restock_come_first_by_priority(client):
    rows = await restock(client, days=15)
    urgent = [r for r in rows if r["needs_restock"]]

    assert urgent, "the seed data has items below zero stock"
    assert rows[:len(urgent)] == urgent
    assert [r["priority"] for r in urgent] == sorted((r["priority"] for r in urgent), reverse=True)


async def test_items_that_never_sell_do_not_need_restock(client):
    rows = await restock(client, days=15)

    for r in rows:
        if r["avg_daily"] == 0:
            assert r["needs_restock"] is False
            assert r["days_left"] in (None, 0)
