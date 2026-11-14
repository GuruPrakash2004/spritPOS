

async def test_dsr_carries_the_desktops_summary_rows(client):
    """frmRptDailySummary's grid: its section headings and the rows the tiles do not show (stock at sales rate, cash / card / other, group wise purchase)."""
    from datetime import date
    today = date.today().isoformat()
    await client.post("/transactions/sales-invoice", json={"date": today, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": "VIN-750", "bottles": 2}]})
    rows = (await client.get("/reports/dsr", params={"on": today})).json()["exe_summary"]
    captions = [r["caption"] for r in rows]
    for want in ("Total Bills", "Total Sales Amount (Incl.GST)", "Sales Amount - Liquor (BasicAmt)", "Opening Stock Value", "Closing Stock Value", "Cash Amount",
                 "Card Amount", "Other Pay Amount", "Total Purchase Amount (Sales rate)", "Counter Wise Sales", "Sales Person Wise Sales", "Group Wise Sales",
                 "Group Wise Purchase", "Expense Ledger Wise"):
        assert want in captions, want
    assert next(r for r in rows if r["caption"] == "Total Bills")["value"] >= 1
    counter = next(r for r in rows if r["caption"] == "c10" or r["caption"] == "counter10")
    assert counter["value"] >= 1 and counter["value2"] > 0   # No of Bills and Value


async def test_profit_margin_is_read_from_the_bills_of_the_period(client):
    """frmRptProMrg: the saved sales lines of the period per item (cancelled bills left out), not the web's own stock movements."""
    from datetime import date
    today = date.today().isoformat()
    before = {r["item_code"]: r for r in (await client.get("/reports/profit-margin", params={"from_date": today, "to_date": today})).json()}
    inv = (await client.post("/transactions/sales-invoice", json={"date": today, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": "VIN-750", "bottles": 2}]})).json()
    rows = {r["item_code"]: r for r in (await client.get("/reports/profit-margin", params={"from_date": today, "to_date": today})).json()}
    assert rows["VIN-750"]["sales_qty"] == before.get("VIN-750", {}).get("sales_qty", 0) + 2 and "group" in rows["VIN-750"]
    await client.post(f"/transactions/sales-invoice/{inv['doc_no']}/cancel", json={"remarks": "test"})
    after = {r["item_code"]: r for r in (await client.get("/reports/profit-margin", params={"from_date": today, "to_date": today})).json()}
    assert after.get("VIN-750", {}).get("sales_qty", 0) == before.get("VIN-750", {}).get("sales_qty", 0)
