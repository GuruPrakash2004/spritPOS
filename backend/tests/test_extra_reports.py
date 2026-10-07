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
