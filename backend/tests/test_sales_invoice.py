"""Sales invoice: validation, stock movement on save, hold and cancel."""
from datetime import date

import pytest

ITEM = "VIN-750"


def bill(**overrides) -> dict:
    """A valid cash bill for 2 bottles of ITEM on counter10, dated today."""
    return {
        "date": date.today().isoformat(),
        "counter": "counter10",
        "salesperson": "GURU",
        "lines": [{"item_code": ITEM, "bottles": 2}],
        **overrides,
    }


async def qoh(client, code: str = ITEM) -> float:
    res = await client.get(f"/masters/items/{code}")
    assert res.status_code == 200
    return res.json()["qoh"]


async def test_saving_a_bill_takes_the_stock(client):
    before = await qoh(client)

    res = await client.post("/transactions/sales-invoice", json=bill())

    assert res.status_code == 201
    doc = res.json()
    assert doc["status"] == "SAVED"
    assert doc["doc_no"].startswith("26/c10-")
    assert doc["lines"][0]["qty"] == 2
    assert await qoh(client) == before - 2


async def test_held_bill_does_not_move_stock(client):
    before = await qoh(client)

    res = await client.post("/transactions/sales-invoice", json=bill(hold=True))

    assert res.status_code == 201
    assert res.json()["status"] == "HOLD"
    assert res.json()["doc_no"].startswith("HOLD-")
    assert await qoh(client) == before


async def test_cancelling_a_bill_puts_the_stock_back(client):
    before = await qoh(client)
    doc_no = (await client.post("/transactions/sales-invoice", json=bill())).json()["doc_no"]

    res = await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "Wrong item"})

    assert res.status_code == 200
    assert res.json()["status"] == "CANCELLED"
    assert await qoh(client) == before


async def test_cannot_cancel_twice(client):
    doc_no = (await client.post("/transactions/sales-invoice", json=bill())).json()["doc_no"]
    await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "Wrong item"})

    res = await client.post(f"/transactions/sales-invoice/{doc_no}/cancel", json={"remarks": "Again"})

    assert res.status_code == 409


@pytest.mark.parametrize(("overrides", "message"), [
    ({"salesperson": None}, "Salesperson is required"),
    ({"counter": "counter12"}, "active Sales Counter"),          # counter12 is inactive in the seed data
    ({"customer": "NOBODY"}, "Unknown customer"),
    ({"lines": [{"item_code": ITEM, "bottles": 100_000}]}, "Insufficient stock"),
])
async def test_invalid_bills_are_rejected(client, overrides, message):
    before = await qoh(client)

    res = await client.post("/transactions/sales-invoice", json=bill(**overrides))

    assert res.status_code == 422
    assert message in res.json()["detail"]
    assert await qoh(client) == before
