"""Package-wise sales/purchase, Loyalty and Excise Stock reports."""
from datetime import date

RANGE = {"from_date": "2020-01-01", "to_date": date.today().isoformat()}


async def test_package_wise_sales_and_purchase(client):
    for path in ("/reports/sales/package-wise", "/reports/purchase/package-wise"):
        res = await client.get(path, params=RANGE)
        assert res.status_code == 200
        for r in res.json():
            assert set(r) == {"package", "qty", "litres", "amount"}


async def test_loyalty_report_lists_customers_with_points(client):
    res = await client.get("/reports/loyalty", params=RANGE)
    assert res.status_code == 200
    assert all(set(r) >= {"customer", "earned", "redeemed", "balance"} for r in res.json())


async def test_excise_stock_groups_by_type_and_package(client):
    res = await client.get("/reports/excise-stock", params={"on": date.today().isoformat()})
    assert res.status_code == 200
    assert all(set(r) == {"liqtype", "package", "qty", "litres"} for r in res.json())


async def test_drops_reports_have_their_total_rows(client):
    on = {"on": date.today().isoformat()}
    sales = (await client.get("/reports/drops/sales-summary", params=on)).json()
    assert sales[0]["particulars"] == "LIQUOR" and sales[-1]["particulars"] == "Total Sales"
    assert any(r["particulars"] == "Round Off" for r in sales)
    purchase = (await client.get("/reports/drops/purchase", params=on)).json()
    assert purchase[-1]["group"] == "Total Purchase"
    petty = (await client.get("/reports/drops/petty-cash", params=on)).json()
    assert petty[0]["particulars"] == "OP Balance" and petty[-1]["particulars"] == "Closing Balance"


async def test_loyalty_detail_rows_are_per_bill(client):
    res = await client.get("/reports/loyalty/detail", params=RANGE)
    assert res.status_code == 200
    assert all(set(r) >= {"customer", "doc_date", "doc_ref", "added", "redeemed"} for r in res.json())


async def test_days_opening_balance_feeds_the_dsr_cash_balance(client):
    on = date.today().isoformat()
    before = (await client.get("/reports/dsr", params={"on": on})).json()["cash_balance"]
    res = await client.post("/transactions/expense/opening-balance", json={"date": on, "amount": 1500})
    assert res.status_code == 200
    assert (await client.get("/transactions/expense/opening-balance", params={"on": on})).json()["amount"] == 1500
    dsr = (await client.get("/reports/dsr", params={"on": on})).json()
    assert dsr["cash_opening"] == 1500 and dsr["cash_balance"] == round(before + 1500, 2)
    late = await client.post("/transactions/expense/opening-balance", json={"date": "2999-01-01", "amount": 1})
    assert late.status_code == 422


async def test_drops_consolidated_has_its_four_sections(client):
    res = await client.get("/reports/drops/consolidated", params=RANGE)
    assert res.status_code == 200
    heads = [r["a"] for r in res.json() if r.get("head") and r["b"] == "" and r["c"] == ""]
    assert heads == ["Sales Data", "Petty Cash Data", "Purchase Data", "Group Wise Summary Balance"]


async def test_petty_cash_uses_the_desktops_receipts_and_ignores_cancelled_entries(client):
    from app import store

    def doc(no, day, amount, receipt, status="SAVED"):
        return {"doc_no": no, "date": day, "paid_from": "CASH", "total": amount, "receipts": receipt, "status": status, "origin": "db",
                "lines": [{"ledger": "TEA", "ledger_name": "Tea / Coffee", "amount": amount, "receipt": receipt, "narration": ""}]}

    store.DOCS["expense"]["E1"] = doc("E1", "2026-01-01", 100, 1000)            # before the day: +900
    store.DOCS["expense"]["E2"] = doc("E2", "2026-01-02", 250, 0)              # on the day
    store.DOCS["expense"]["E3"] = doc("E3", "2026-01-02", 999, 999, "CANCELLED")   # cancelled: left out
    rows = (await client.get("/reports/drops/petty-cash", params={"on": "2026-01-02"})).json()
    assert rows[0] == {"particulars": "OP Balance", "expense": None, "receipt": 900.0}
    assert {"particulars": "Tea / Coffee", "expense": 250.0, "receipt": 0.0} in rows
    assert rows[-1]["receipt"] == 650.0                                         # 900 - 250


async def test_item_rate_history_reads_the_audit_rows_and_hides_costs_from_cashiers(client, cashier_client, monkeypatch):
    from app import db
    rows = [{"ItemCode": "A1", "ItemName": "Brandy", "ISource": "From Item Master", "OldMRP": 585, "newMRP": 580, "OldPurRate": 531, "NewPurRate": 522,
             "CUserName": "ADMIN", "CSystemName": "PC1", "cDateTime": "2026-05-23 14:02:45"}]
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): rows)
    res = await client.get("/reports/item-rate-history", params={"from_date": "2026-05-01", "to_date": "2026-05-31"})
    assert res.status_code == 200 and res.json()[0]["new_pur_rate"] == 522 and res.json()[0]["new_mrp"] == 580
    res = await cashier_client.get("/reports/item-rate-history", params={"from_date": "2026-05-01", "to_date": "2026-05-31"})
    assert res.json()[0]["new_pur_rate"] is None and res.json()[0]["new_mrp"] == 580
