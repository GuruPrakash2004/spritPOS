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


# ─── GST from the Doc Designer's formulas ──────────────────────────────────────

def _designer_has_formulas(monkeypatch):
    """Pretend the designer gives the line amount / CGST / SGST a formula (the engine itself is stubbed per test)."""
    from app import doc_engine, schema_meta
    monkeypatch.setattr(doc_engine, "doc_id_of", lambda name: 1)
    monkeypatch.setattr(schema_meta, "screen", lambda doc_id: {"fields": [{"code": c, "formula": "1"} for c in ("SINVAMT", "CGSTAMT", "SGSTAMT")]})


async def test_bill_gst_follows_the_designers_formulas(client, monkeypatch):
    """A designer formula for a line's amount / CGST / SGST decides those figures and the totals; no formula = built-in maths."""
    from app import doc_engine
    plain = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    _designer_has_formulas(monkeypatch)
    monkeypatch.setattr(doc_engine, "compute", lambda kind, doc: {"rows": [{"SINVAMT": 100.0, "cgstamt": 9.0}], "header": {}, "problems": []})
    res = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    line = res["lines"][0]
    assert (line["amount"], line["cgst"]) == (100.0, 9.0)
    assert line["sgst"] == plain["lines"][0]["sgst"]          # no formula for SGST: the built-in figure stays
    assert res["totals"]["basic"] == 100.0 and res["totals"]["cgst"] == 9.0


async def test_a_formula_that_fails_leaves_the_built_in_maths(client, monkeypatch):
    from app import doc_engine

    def broken(kind, doc):
        raise RuntimeError("bad formula")
    plain = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    _designer_has_formulas(monkeypatch)
    monkeypatch.setattr(doc_engine, "compute", broken)
    res = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    assert res["lines"][0]["amount"] == plain["lines"][0]["amount"]


async def test_a_formula_that_works_out_to_zero_keeps_the_built_in_amount(client, monkeypatch):
    """A formula the web can't feed (it reads a column the bill has no value for) gives 0: the bill must not be billed at ₹0."""
    from app import doc_engine
    plain = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    _designer_has_formulas(monkeypatch)
    monkeypatch.setattr(doc_engine, "compute", lambda kind, doc: {"rows": [{"SINVAMT": 0.0, "CGSTAMT": 0.0, "SGSTAMT": 0.0}], "header": {}, "problems": []})
    res = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    assert res["lines"][0]["amount"] == plain["lines"][0]["amount"] > 0
    assert res["totals"]["net"] == plain["totals"]["net"]
