"""Stock, money and master-data rules (the bugs fixed in the full audit)."""
from datetime import date

import pytest

from app import store

TODAY = date.today().isoformat()


def bill(lines, **extra):
    return {"date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": lines, **extra}


async def qoh(client, code):
    return (await client.get(f"/masters/items/{code}")).json()["qoh"]


# ---------- Quantities and money can't go negative ----------
@pytest.mark.parametrize("line", [
    {"item_code": "VIN-750", "bottles": -2},
    {"item_code": "VIN-750", "cases": -1, "bottles": 20},
    {"item_code": "VIN-750", "bottles": 1, "rate": -100},
])
async def test_negative_quantities_and_rates_are_refused(client, line):
    before = await qoh(client, "VIN-750")

    res = await client.post("/transactions/sales-invoice", json=bill([line]))

    assert res.status_code == 422
    assert await qoh(client, "VIN-750") == before


async def test_empty_bill_is_refused(client):
    assert (await client.post("/transactions/sales-invoice", json=bill([]))).status_code == 422


async def test_negative_points_cannot_mint_loyalty_points(client):
    store.CUSTOMERS["C1"] = {"code": "C1", "name": "Ravi Kumar", "mobile": "", "loyalty_card_no": "", "send_sms": False, "points": 10}

    res = await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 1}], customer="C1",
                                                                     redeem_points=-5000))

    assert res.status_code == 422
    assert store.CUSTOMERS["C1"]["points"] == 10


async def test_discount_cannot_exceed_the_bill(client):
    res = await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 1}], discount=5000))

    assert res.status_code == 422
    assert "more than the bill" in res.json()["detail"]


async def test_payment_must_add_up_to_the_bill(client):
    res = await client.post("/transactions/sales-invoice", json=bill(
        [{"item_code": "VIN-750", "bottles": 1}], payment={"cash": 100, "card": 0, "upi": 0, "credit": 0}))

    assert res.status_code == 422
    assert "does not match" in res.json()["detail"]


async def test_paise_rounding_goes_into_the_payment(client):
    # 835 MRP with a 0.40 discount: the screen shows 834.60, the bill rounds to 835.
    res = await client.post("/transactions/sales-invoice", json=bill(
        [{"item_code": "VIN-750", "bottles": 1}], discount=0.4, payment={"cash": 834.6, "card": 0, "upi": 0, "credit": 0}))

    assert res.status_code == 201
    doc = res.json()
    assert doc["totals"]["net"] == 835
    assert sum(doc["payment"].values()) == 835


# ---------- Stock can't be oversold ----------
async def test_same_item_on_two_lines_cannot_oversell(client):
    stock = int(await qoh(client, "VIN-750"))
    half = stock // 2 + 1   # each line alone fits, both together don't

    res = await client.post("/transactions/sales-invoice", json=bill(
        [{"item_code": "VIN-750", "bottles": half}, {"item_code": "VIN-750", "bottles": half}]))

    assert res.status_code == 422
    assert await qoh(client, "VIN-750") == stock


async def test_stock_deduction_cannot_go_below_zero(client):
    stock = await qoh(client, "RC-750")

    res = await client.post("/transactions/stock-deduction", json={
        "date": TODAY, "lines": [{"item_code": "RC-750", "bottles": int(stock) + 1}]})

    assert res.status_code == 422
    assert await qoh(client, "RC-750") == stock


async def test_purchase_return_cannot_go_below_zero(client):
    res = await client.post("/transactions/purchase-bill", json={
        "date": TODAY, "supplier": "KSBCL", "supplier_inv_no": "R1", "supplier_inv_date": TODAY,
        "purchase_type": "Purchase Return", "lines": [{"item_code": "RC-750", "bottles": 100_000}]})

    assert res.status_code == 422


async def test_releasing_a_held_bill_checks_stock_again(client):
    stock = int(await qoh(client, "OM-750"))
    held = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "OM-750", "bottles": stock}], hold=True))).json()
    # Meanwhile the stock is sold on another bill.
    assert (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "OM-750", "bottles": stock}]))).status_code == 201

    res = await client.post(f"/transactions/sales-invoice/{held['doc_no']}/release")

    assert res.status_code == 422
    assert await qoh(client, "OM-750") == 0


async def test_released_bill_earns_loyalty_points(client):
    store.CUSTOMERS["C1"] = {"code": "C1", "name": "Ravi Kumar", "mobile": "", "loyalty_card_no": "", "send_sms": False, "points": 0}
    held = (await client.post("/transactions/sales-invoice", json=bill(
        [{"item_code": "VIN-750", "bottles": 2}], customer="C1", hold=True))).json()

    released = (await client.post(f"/transactions/sales-invoice/{held['doc_no']}/release")).json()

    assert released["status"] == "SAVED"
    assert released["totals"]["points_earned"] > 0
    assert store.CUSTOMERS["C1"]["points"] == released["totals"]["points_earned"]


async def test_held_bill_cannot_be_cancelled(client):
    held = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 1}], hold=True))).json()

    res = await client.post(f"/transactions/sales-invoice/{held['doc_no']}/cancel", json={"remarks": "mistake"})

    assert res.status_code == 409


# ---------- Sales returns ----------
async def test_return_cannot_exceed_what_the_invoice_sold(client):
    inv = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))).json()
    ret = {"date": TODAY, "against_invoice": inv["doc_no"]}

    first = await client.post("/transactions/sales-return", json=ret | {"lines": [{"item_code": "VIN-750", "bottles": 1}]})
    too_many = await client.post("/transactions/sales-return", json=ret | {"lines": [{"item_code": "VIN-750", "bottles": 2}]})
    other_item = await client.post("/transactions/sales-return", json=ret | {"lines": [{"item_code": "RC-750", "bottles": 1}]})

    assert first.status_code == 201
    assert too_many.status_code == 422
    assert other_item.status_code == 422


async def test_returned_bill_cannot_then_be_cancelled(client):
    inv = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))).json()
    await client.post("/transactions/sales-return", json={"date": TODAY, "against_invoice": inv["doc_no"],
                                                          "lines": [{"item_code": "VIN-750", "bottles": 1}]})

    res = await client.post(f"/transactions/sales-invoice/{inv['doc_no']}/cancel", json={"remarks": "mistake"})

    assert res.status_code == 409


async def test_cannot_return_against_a_cancelled_bill(client):
    inv = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 1}]))).json()
    await client.post(f"/transactions/sales-invoice/{inv['doc_no']}/cancel", json={"remarks": "mistake"})

    res = await client.post("/transactions/sales-return", json={"date": TODAY, "against_invoice": inv["doc_no"],
                                                                "lines": [{"item_code": "VIN-750", "bottles": 1}]})

    assert res.status_code == 422


# ---------- Other transactions ----------
async def test_indent_needs_a_known_supplier(client):
    res = await client.post("/transactions/purchase-indent", json={
        "date": TODAY, "supplier": "NOBODY", "lines": [{"item_code": "VIN-750", "bottles": 12}]})

    assert res.status_code == 422


async def test_cash_count_rejects_unknown_notes(client):
    bad = await client.post("/transactions/denomination", json={"date": TODAY, "counts": {"abc": 1}})
    good = await client.post("/transactions/denomination", json={"date": TODAY, "counts": {"500": 2, "coins": 35}})

    assert bad.status_code == 422
    assert good.json()["total"] == 1035


async def test_closing_stock_rejects_duplicate_items(client):
    res = await client.post("/transactions/closing-stock", json={"as_on": TODAY, "lines": [
        {"item_code": "VIN-750", "closing_qty": 1}, {"item_code": "VIN-750", "closing_qty": 2}]})

    assert res.status_code == 422


# ---------- Masters ----------
async def test_master_code_cannot_be_renamed_onto_another(client):
    res = await client.put("/masters/uoms/NOS", json={"code": "BTL", "name": "Numbers"})

    assert res.status_code == 409
    assert store.UOMS["BTL"]["name"] == "Bottle"


@pytest.mark.parametrize("code", ["", "   ", "A/B", "50%", "X?Y"])
async def test_bad_master_codes_are_refused(client, code):
    assert (await client.post("/masters/brands", json={"code": code, "name": "Test"})).status_code == 422


@pytest.mark.parametrize(("path", "code"), [
    ("item-groups", "WHISKY"), ("packages", "750ml"), ("brands", "Kingfisher"),
    ("sales-counters", "counter10"), ("customers", "CASH"), ("suppliers", "KSBCL-BN"),
])
async def test_masters_in_use_cannot_be_deleted(client, path, code):
    res = await client.delete(f"/masters/{path}/{code}")

    assert res.status_code == 409


async def test_editing_an_item_does_not_change_its_stock(client):
    item = (await client.get("/masters/items/VIN-750")).json()

    res = await client.put("/masters/items/VIN-750", json=item | {"qoh": 99999, "mrp": 840})

    assert res.json()["qoh"] == item["qoh"]
    assert res.json()["mrp"] == 840


async def test_new_item_opening_stock_is_in_the_stock_ledger(client):
    new = {"code": "TEST-2", "name": "TEST GIN 750", "group": "WHISKY", "package": "750ml", "mrp": 600,
           "purchase_rate": 500, "qoh": 24}
    assert (await client.post("/masters/items", json=new)).status_code == 201

    ledger = (await client.get("/reports/stock-ledger", params={"item_code": "TEST-2", "from_date": TODAY,
                                                                "to_date": TODAY})).json()

    assert ledger[-1]["in"] == 24
    assert ledger[-1]["balance"] == 24


async def test_item_needs_a_known_group(client):
    res = await client.post("/masters/items", json={"code": "TEST-3", "name": "X", "group": "NOPE", "mrp": 1, "purchase_rate": 1})

    assert res.status_code == 422


async def test_import_is_all_or_nothing(client):
    rows = [
        {"code": "IMP-1", "name": "IMPORTED ONE", "group": "BEER", "package": "650ml", "mrp": 200, "purchase_rate": 150},
        {"code": "IMP-2", "name": "", "group": "BEER", "package": "650ml", "mrp": 200, "purchase_rate": 150},
    ]

    res = await client.post("/masters/items/import", json=rows)

    assert res.status_code == 422
    assert "Row 2" in res.json()["detail"]
    assert "IMP-1" not in store.ITEMS


# ---------- Reports ----------
async def test_reports_refuse_backwards_and_huge_ranges(client):
    backwards = await client.get("/reports/sales/daily", params={"from_date": "2026-02-01", "to_date": "2026-01-01"})
    huge = await client.get("/reports/sales/daily", params={"from_date": "1900-01-01", "to_date": "2100-01-01"})

    assert backwards.status_code == 422
    assert huge.status_code == 422


async def test_dsr_survives_a_sold_item_missing_from_item_master(client):
    inv = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "RC-750", "bottles": 1}]))).json()
    store.ITEMS.pop("RC-750")   # e.g. an item dropped from the DB load

    res = await client.get("/reports/dsr", params={"on": inv["date"]})

    assert res.status_code == 200
