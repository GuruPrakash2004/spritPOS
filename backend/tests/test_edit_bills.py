"""Editing a saved sales invoice or purchase bill in place: same number, stock and loyalty points follow, bad edits change nothing."""
from datetime import date

ITEM = "VIN-750"


def bill(**over) -> dict:
    return {"date": date.today().isoformat(), "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": ITEM, "bottles": 2}], **over}


def purchase(**over) -> dict:
    return {"date": date.today().isoformat(), "supplier": "KSBCL", "supplier_inv_no": "INV-EDIT", "supplier_inv_date": date.today().isoformat(),
            "lines": [{"item_code": ITEM, "cases": 0, "bottles": 6}], **over}


async def qoh(client) -> float:
    return (await client.get(f"/masters/items/{ITEM}")).json()["qoh"]


async def test_editing_a_sales_invoice_keeps_the_number_and_follows_the_stock(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/sales-invoice", json=bill())).json()
    assert await qoh(client) == start - 2
    res = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill(lines=[{"item_code": ITEM, "bottles": 5}]))
    assert res.status_code == 200, res.text
    edited = res.json()
    assert edited["doc_no"] == doc["doc_no"] and edited["revision"] == 1 and edited["lines"][0]["qty"] == 5
    assert edited["time"] == doc["time"] and edited["added_by"] == doc["added_by"] and edited["edited_by"]
    assert await qoh(client) == start - 5                      # the old 2 went back, the new 5 came out
    assert (await client.get(f"/transactions/sales-invoice/{doc['doc_no']}")).json()["totals"]["qty"] == 5
    again = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill(lines=[{"item_code": ITEM, "bottles": 1}]))
    assert again.json()["revision"] == 2 and await qoh(client) == start - 1


async def test_a_sales_edit_that_needs_more_stock_than_there_is_changes_nothing(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/sales-invoice", json=bill())).json()
    res = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill(lines=[{"item_code": ITEM, "bottles": int(start) + 500}]))
    assert res.status_code == 422 and "Insufficient stock" in res.json()["detail"]
    assert await qoh(client) == start - 2                      # unchanged: still the original bill
    kept = (await client.get(f"/transactions/sales-invoice/{doc['doc_no']}")).json()
    assert kept["lines"][0]["qty"] == 2 and not kept.get("revision")


async def test_cancelled_held_and_returned_bills_cannot_be_edited(client):
    doc = (await client.post("/transactions/sales-invoice", json=bill())).json()
    await client.post(f"/transactions/sales-invoice/{doc['doc_no']}/cancel", json={"remarks": "test"})
    res = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill())
    assert res.status_code == 409 and "cancelled" in res.json()["detail"].lower()
    held = (await client.post("/transactions/sales-invoice", json=bill(hold=True))).json()
    assert (await client.put(f"/transactions/sales-invoice/{held['doc_no']}", json=bill())).status_code == 409
    saved = (await client.post("/transactions/sales-invoice", json=bill())).json()
    assert (await client.put(f"/transactions/sales-invoice/{saved['doc_no']}", json=bill(hold=True))).status_code == 422
    assert (await client.put("/transactions/sales-invoice/26%2FNOPE-1", json=bill())).status_code == 404


async def test_editing_needs_the_edit_right(cashier_client, client):
    doc = (await client.post("/transactions/sales-invoice", json=bill())).json()
    await client.put("/admin/roles/Cashier", json={"name": "Cashier", "modules": ["Sales Invoice"], "rights": {"Sales Invoice": {"add": True, "edit": False, "delete": False}}})
    assert (await cashier_client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill())).status_code == 403


async def test_editing_a_purchase_bill_moves_the_stock_by_the_difference(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/purchase-bill", json=purchase())).json()
    assert await qoh(client) == start + 6
    res = await client.put(f"/transactions/purchase-bill/{doc['doc_no']}", json=purchase(lines=[{"item_code": ITEM, "cases": 0, "bottles": 10}]))
    assert res.status_code == 200, res.text
    assert res.json()["doc_no"] == doc["doc_no"] and res.json()["revision"] == 1 and res.json()["lines"][0]["qty"] == 10
    assert await qoh(client) == start + 10


async def test_a_purchase_edit_is_refused_when_its_stock_was_sold_since(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/purchase-bill", json=purchase(lines=[{"item_code": ITEM, "cases": 0, "bottles": 4}]))).json()
    # sell everything there is: taking the 4 back out would leave the item below zero
    await client.post("/transactions/sales-invoice", json=bill(lines=[{"item_code": ITEM, "bottles": int(start + 4)}]))
    res = await client.put(f"/transactions/purchase-bill/{doc['doc_no']}", json=purchase(lines=[{"item_code": ITEM, "cases": 0, "bottles": 1}]))
    assert res.status_code in (409, 422)
    assert await qoh(client) == 0
    assert (await client.get(f"/transactions/purchase-bill/{doc['doc_no']}")).json()["lines"][0]["qty"] == 4


async def test_edits_respect_the_freeze_date(client):
    from app import gsettings
    doc = (await client.post("/transactions/sales-invoice", json=bill())).json()
    gsettings.seed()
    gsettings.store.GSETTINGS["FreezeDate"]["value"] = date.today().isoformat()
    res = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill())
    assert res.status_code == 409 and "freeze" in res.json()["detail"].lower()


def test_bills_read_from_the_database_carry_their_real_header_so_they_can_be_edited(monkeypatch):
    """The loaders keep the customer, discount, payment split and payment type of a sales invoice, and the supplier code, invoice number / date /
    value and purchase type of a purchase bill (they used to come back as CASH / Purchase / no invoice number). Columns a client lacks are NULL."""
    from app import db, db_store, store
    store.ITEMS["VIN-750"]["_itemid"] = "7"
    sales = {"SINVID": 1, "SINVNO": "26/c10-0000009", "SINVDate": "2026-10-30", "CUSTID": 3, "CUSTCODE": "ASHA", "TOTAMT": 500, "CASHAMT": 200, "CARDAMT": 0, "OTHPAYAMT": 300,
             "TOTQTY": 1, "SPID": 0, "SCID": 0, "AddedBy": "ADMIN", "Cancelled": "N", "SINVTime": "10:00:00", "TOTDISCAMT": 25, "GRNDTOT": 475, "SALTYP": "SPLIT",
             "SINVDtlID": 1, "ITEMID": 7, "SINVQty": 1, "SINVRate": 500, "SINVAmt": 500, "CGSTPER": 0, "CGSTAMT": 0, "SGSTPER": 0, "SGSTAMT": 0}
    buy = {"PBID": 1, "PBNO": "26PB0000009", "PBDate": "2026-10-30", "SUPPID": 2, "SUPPCODE": "KSBCL", "PartyName": "KSBCL Depot", "SUPPINVNO": "INV9", "SUPPINVDATE": "2026-10-29",
           "SUPPINVVALUE": 1000, "PURCHASETYPE": "Purchase Return", "TCSAMT": 10, "TOTCESS": None, "GRNDTOT": 1010, "RMKS": "r", "PBDtlID": 1, "ITEMID": 7, "PBQty": 2, "PBRate": 500,
           "PBAmt": 1000, "NOOFCASES": 0, "CGSTAMT": 0, "SGSTAMT": 0}
    monkeypatch.setattr(db_store, "_recent_sinv_numbers", lambda limit: None)
    monkeypatch.setattr(db, "fetchall", lambda sql, *a: [] if "sys.columns" in sql or "TrnMultiAmt" in sql else ([buy] if "TrnHdrPB" in sql else [sales]))
    db_store.load_sales_invoices(10)
    d = store.DOCS["sales_invoice"]["26/c10-0000009"]
    assert d["customer"] == "ASHA" and d["payment_type"] == "SPLIT" and d["totals"]["discount"] == 25 and d["totals"]["net"] == 475
    assert d["payment"] == {"cash": 200.0, "card": 0.0, "upi": 300.0, "credit": 0.0}   # no TrnMultiAmt rows: the other amount is UPI
    db_store.load_purchase_bills(10)
    p = store.DOCS["purchase_bill"]["26PB0000009"]
    assert p["supplier"] == "KSBCL" and p["supplier_inv_no"] == "INV9" and p["supplier_inv_date"] == "2026-10-29" and p["supplier_inv_value"] == 1000
    assert p["purchase_type"] == "Purchase Return" and p["tcs"] == 10 and p["totals"]["net"] == 1010


# ── the other documents ──────────────────────────────────────────────────────────

def _lines(n):
    return [{"item_code": ITEM, "cases": 0, "bottles": n}]


async def test_editing_a_purchase_indent_changes_it_in_place_and_moves_no_stock(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/purchase-indent", json={"date": date.today().isoformat(), "supplier": "KSBCL", "lines": _lines(3)})).json()
    res = await client.put(f"/transactions/purchase-indent/{doc['doc_no']}", json={"date": date.today().isoformat(), "supplier": "KSBCL", "lines": _lines(8), "remarks": "more"})
    assert res.status_code == 200, res.text
    assert res.json()["doc_no"] == doc["doc_no"] and res.json()["lines"][0]["qty"] == 8 and res.json()["revision"] == 1 and res.json()["remarks"] == "more"
    assert await qoh(client) == start


async def test_editing_a_sales_return_follows_the_stock_and_can_be_refused(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/sales-return", json={"date": date.today().isoformat(), "lines": _lines(2)})).json()
    assert await qoh(client) == start + 2
    res = await client.put(f"/transactions/sales-return/{doc['doc_no']}", json={"date": date.today().isoformat(), "lines": _lines(5)})
    assert res.status_code == 200 and res.json()["doc_no"] == doc["doc_no"] and await qoh(client) == start + 5
    # sell all of it: taking the returned stock back out would go below zero
    await client.post("/transactions/sales-invoice", json=bill(lines=[{"item_code": ITEM, "bottles": int(start + 5)}]))
    refused = await client.put(f"/transactions/sales-return/{doc['doc_no']}", json={"date": date.today().isoformat(), "lines": _lines(1)})
    assert refused.status_code == 409 and await qoh(client) == 0
    assert (await client.get(f"/transactions/sales-return/{doc['doc_no']}")).json()["lines"][0]["qty"] == 5


async def test_a_return_edited_against_its_invoice_is_not_counted_twice(client):
    inv = (await client.post("/transactions/sales-invoice", json=bill(lines=_lines(4)))).json()
    ret = (await client.post("/transactions/sales-return", json={"date": date.today().isoformat(), "against_invoice": inv["doc_no"], "lines": _lines(3)})).json()
    again = await client.put(f"/transactions/sales-return/{ret['doc_no']}", json={"date": date.today().isoformat(), "against_invoice": inv["doc_no"], "lines": _lines(4)})
    assert again.status_code == 200, again.text   # 4 of 4 may come back: the 3 it already holds are its own
    too_many = await client.put(f"/transactions/sales-return/{ret['doc_no']}", json={"date": date.today().isoformat(), "against_invoice": inv["doc_no"], "lines": _lines(5)})
    assert too_many.status_code == 422


async def test_editing_a_stock_deduction_follows_the_stock(client):
    start = await qoh(client)
    doc = (await client.post("/transactions/stock-deduction", json={"date": date.today().isoformat(), "reason": "Breakage", "lines": _lines(2)})).json()
    assert await qoh(client) == start - 2
    res = await client.put(f"/transactions/stock-deduction/{doc['doc_no']}", json={"date": date.today().isoformat(), "reason": "Leakage", "lines": _lines(6)})
    assert res.status_code == 200 and res.json()["doc_no"] == doc["doc_no"] and res.json()["reason"] == "Leakage" and await qoh(client) == start - 6
    too_much = await client.put(f"/transactions/stock-deduction/{doc['doc_no']}", json={"date": date.today().isoformat(), "reason": "Leakage", "lines": _lines(int(start) + 99)})
    assert too_much.status_code == 422 and await qoh(client) == start - 6


async def test_editing_an_expense_in_memory(client):
    led = next(iter(__import__("app.store", fromlist=["LEDGERS"]).LEDGERS))
    doc = (await client.post("/transactions/expense", json={"date": date.today().isoformat(), "paid_from": "CASH", "lines": [{"ledger": led, "amount": 50}]})).json()
    res = await client.put(f"/transactions/expense/{doc['doc_no']}", json={"date": date.today().isoformat(), "paid_from": "CASH", "lines": [{"ledger": led, "amount": 75}]})
    assert res.status_code == 200 and res.json()["total"] == 75 and res.json()["doc_no"] == doc["doc_no"]


async def test_editing_a_loyalty_bill_settles_the_points_again_from_the_start(client):
    """Points redeemed are given back and points earned taken back, then the changed bill redeems / earns its own; a failed edit leaves the points alone."""
    from app import gsettings, store
    gsettings.seed()
    store.GSETTINGS["PointsForOneRs"]["value"] = "0.1"
    store.GSETTINGS["MinBillForPoints"]["value"] = "0"
    code = next(c for c in store.CUSTOMERS if c != "CASH")
    store.CUSTOMERS[code]["points"] = 100.0
    first = (await client.post("/transactions/sales-invoice", json=bill(customer=code, redeem_points=20, lines=_lines(2)))).json()
    earned1 = first["totals"]["points_earned"]
    assert store.CUSTOMERS[code]["points"] == round(100 - 20 + earned1, 2)
    res = await client.put(f"/transactions/sales-invoice/{first['doc_no']}", json=bill(customer=code, redeem_points=10, lines=_lines(4)))
    assert res.status_code == 200, res.text
    earned2 = res.json()["totals"]["points_earned"]
    assert earned2 > earned1 and res.json()["totals"]["points_redeemed"] == 10
    assert store.CUSTOMERS[code]["points"] == round(100 - 10 + earned2, 2)
    before = store.CUSTOMERS[code]["points"]
    bad = await client.put(f"/transactions/sales-invoice/{first['doc_no']}", json=bill(customer=code, redeem_points=9999, lines=_lines(4)))   # more points than the customer has
    assert bad.status_code == 422 and store.CUSTOMERS[code]["points"] == before
    assert (await client.get(f"/transactions/sales-invoice/{first['doc_no']}")).json()["totals"]["points_redeemed"] == 10


async def test_a_loyalty_bill_moved_to_another_customer_settles_both(client):
    from app import gsettings, store
    gsettings.seed()
    store.GSETTINGS["PointsForOneRs"]["value"] = "0.1"
    store.GSETTINGS["MinBillForPoints"]["value"] = "0"
    names = [c for c in store.CUSTOMERS if c != "CASH"]
    if len(names) < 2:
        return
    a, b = names[:2]
    store.CUSTOMERS[a]["points"] = 40.0
    store.CUSTOMERS[b]["points"] = 5.0
    doc = (await client.post("/transactions/sales-invoice", json=bill(customer=a, lines=_lines(2)))).json()
    earned = doc["totals"]["points_earned"]
    assert store.CUSTOMERS[a]["points"] == round(40 + earned, 2)
    res = await client.put(f"/transactions/sales-invoice/{doc['doc_no']}", json=bill(customer=b, lines=_lines(2)))
    assert res.status_code == 200, res.text
    assert store.CUSTOMERS[a]["points"] == 40.0                         # the first customer is back where they were
    assert store.CUSTOMERS[b]["points"] == round(5 + res.json()["totals"]["points_earned"], 2)
