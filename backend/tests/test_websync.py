"""Web Sync (app/websync.py, Utilities → Web Sync): the desktop's central tables built from the day's bills,
pushed to the central site with this shop's key, and what happens when the central site can't be reached."""
import json
from datetime import date

import httpx
import pytest

from app import store, websync

CENTRAL = "https://central.example.com/api"


@pytest.fixture
def sync_on(monkeypatch):
    monkeypatch.setenv("WEB_SYNC_URL", CENTRAL + "/")
    monkeypatch.setenv("WEB_SYNC_KEY", "shop-key-123")
    monkeypatch.setattr(websync, "STATUS", {k: None for k in websync.STATUS})
    monkeypatch.setattr(websync, "_live", lambda: True)   # as with the shop's own database loaded


@pytest.fixture(autouse=True)
def sync_off_by_default(monkeypatch):
    monkeypatch.delenv("WEB_SYNC_URL", raising=False)
    monkeypatch.delenv("WEB_SYNC_KEY", raising=False)


def _a_day_with_bills() -> date:
    return max(date.fromisoformat(i["date"]) for i in store.DOCS["sales_invoice"].values() if i["status"] == "SAVED")


async def test_build_has_the_desktop_tables_and_matches_the_dsr(client):
    day = _a_day_with_bills()
    payload = websync.build(day)
    assert payload["day"] == day.isoformat()
    assert set(payload["tables"]) == set(websync.DAY_TABLES)
    smry = payload["tables"]["RptDSRSmry"][0]
    dsr = (await client.get(f"/reports/dsr?on={day}")).json()
    assert smry["TotBills"] == dsr["total_bills"] > 0
    assert smry["TotSalesAmt"] == dsr["sales_incl_gst"]
    assert smry["DaySalesValue"] == dsr["sales_incl_gst"] <= smry["MnthSalesValue"] <= smry["YearSalesValue"]
    counters = payload["tables"]["RptDSRCtrSales"]
    assert sum(r["NoOfBills"] for r in counters) == dsr["total_bills"]
    assert round(sum(r["BillValue"] for r in counters), 2) == dsr["sales_incl_gst"]
    assert all(set(r) == {"slno", "CounterNo", "NoOfBills", "BillValue"} for r in counters)
    fast = payload["tables"]["RptFastMov"]
    assert 0 < len(fast) <= websync.TOP and fast == sorted(fast, key=lambda r: (-r["SoldQty"], r["ItemName"]))


def test_min_stock_lists_items_below_their_level():
    store.ITEMS["OM-750"].update(qoh=2, rol=12)
    rows = websync.build(date.today())["tables"]["RptMinStk"]
    om = next(r for r in rows if r["ItemName"] == store.ITEMS["OM-750"]["name"])
    assert om == {"ItemName": store.ITEMS["OM-750"]["name"], "QOH": 2, "MinLevel": 12, "ShortageQty": 10}


async def test_indents_of_the_day_are_sent(client):
    res = await client.post("/transactions/purchase-indent", json={
        "date": date.today().isoformat(), "supplier": "USL", "lines": [{"item_code": "RC-750", "cases": 2}]})
    assert res.status_code == 201, res.text
    tables = websync.build(date.today())["tables"]
    hdr = next(h for h in tables["TrnHdrPI"] if h["PINO"] == res.json()["doc_no"])
    assert hdr["SUPPName"] == store.SUPPLIERS["USL"]["name"]
    lines = [d for d in tables["TrnDtlPI"] if d["PIID"] == hdr["PIID"]]
    assert lines and lines[0]["ItemCode"] == "RC-750" and lines[0]["PIQty"] == 24


def test_stock_has_every_item():
    items = websync.build_stock()["items"]
    assert {i["ItemCode"] for i in items} == set(store.ITEMS)
    assert next(i for i in items if i["ItemCode"] == "KF-650")["QOH"] == store.ITEMS["KF-650"]["qoh"]


def test_push_sends_the_key_and_the_day(block_network, sync_on):
    day_route = block_network.post(CENTRAL + "/sync").mock(return_value=httpx.Response(200, json={"ok": True}))
    stock_route = block_network.post(CENTRAL + "/sync/stock").mock(return_value=httpx.Response(200, json={"ok": True}))
    day = _a_day_with_bills()
    status = websync.sync([day])
    assert day_route.call_count == 1 and stock_route.call_count == 1
    sent = day_route.calls.last.request
    assert sent.headers["authorization"] == "Bearer shop-key-123"
    assert json.loads(sent.read())["day"] == day.isoformat()
    assert status["last_ok"] and status["last_error"] is None and status["last_day"] == day.isoformat()


@pytest.mark.parametrize("reply", [httpx.Response(500, json={"detail": "boom"}), httpx.ConnectTimeout("slow")])
def test_a_failed_push_is_recorded_and_changes_nothing(block_network, sync_on, reply):
    route = block_network.post(CENTRAL + "/sync")
    route.mock(side_effect=reply) if isinstance(reply, Exception) else route.mock(return_value=reply)
    before = json.dumps(websync.build(date.today()), sort_keys=True)
    with pytest.raises(websync.SyncFailed):
        websync.sync([date.today()])
    assert websync.STATUS["last_error"] and websync.STATUS["last_ok"] is None
    assert json.dumps(websync.build(date.today()), sort_keys=True) == before
    assert "shop-key-123" not in websync.STATUS["last_error"]


def test_a_refused_key_says_so(block_network, sync_on):
    block_network.post(CENTRAL + "/sync").mock(return_value=httpx.Response(401))
    with pytest.raises(websync.SyncFailed, match="sync key"):
        websync.sync([date.today()])


def test_nothing_is_sent_when_sync_is_not_set_up():
    assert not websync.configured()
    with pytest.raises(websync.SyncFailed, match="not set up"):
        websync.sync([date.today()])
    websync.start()   # does nothing
    assert websync._thread is None or not websync._thread.is_alive()


async def test_screen_needs_the_right_and_never_shows_the_key(client, cashier_client, sync_on):
    assert (await cashier_client.get("/utilities/web-sync")).status_code == 403
    assert (await cashier_client.post("/utilities/web-sync", json={})).status_code == 403
    res = await client.get("/utilities/web-sync")
    assert res.status_code == 200
    body = res.json()
    assert body["configured"] is True and body["central"] == "central.example.com"
    assert "shop-key-123" not in res.text and "/api" not in body["central"]


async def test_sync_now_reports_a_central_failure(client, block_network, sync_on):
    block_network.post(CENTRAL + "/sync").mock(return_value=httpx.Response(503))
    res = await client.post("/utilities/web-sync", json={"day": date.today().isoformat()})
    assert res.status_code == 502 and "503" in res.json()["detail"]
    future = date.fromordinal(date.today().toordinal() + 1)
    assert (await client.post("/utilities/web-sync", json={"day": future.isoformat()})).status_code == 422


def test_sample_data_is_never_sent(block_network, sync_on, monkeypatch):
    monkeypatch.setattr(websync, "_live", lambda: False)
    route = block_network.post(CENTRAL + "/sync")
    with pytest.raises(websync.SyncFailed, match="database isn.t loaded"):
        websync.sync([date.today()])
    assert route.call_count == 0


@pytest.mark.parametrize("url,ok", [("http://central.example.com", False), ("ftp://x", False),
                                    ("http://localhost:8002", True), ("https://central.example.com/api", True)])
def test_the_key_only_travels_over_https(monkeypatch, url, ok):
    monkeypatch.setenv("WEB_SYNC_URL", url)
    assert (websync._url_problem() is None) == ok


def test_central_host_never_shows_credentials(monkeypatch):
    monkeypatch.setenv("WEB_SYNC_URL", "https://someone:secret@central.example.com:8443/api")
    assert websync.central_host() == "central.example.com:8443"


def test_days_to_send_catch_up_after_an_outage():
    today = date(2026, 10, 4)
    assert websync.days_to_send(today, None) == [date(2026, 10, 3), today]
    assert websync.days_to_send(today, today) == [today]
    assert websync.days_to_send(today, date(2026, 10, 1)) == [date(2026, 10, n) for n in range(1, 5)]
    assert len(websync.days_to_send(today, date(2026, 1, 1))) == websync.CATCH_UP_DAYS


def test_round_off_is_sent():
    day = _a_day_with_bills()
    bill = next(i for i in store.DOCS["sales_invoice"].values() if i["date"] == day.isoformat() and i["status"] == "SAVED")
    t = bill["totals"]
    expected = sum(
        (lambda d: d if abs(d) < 1 else 0)(i["totals"]["net"] - (i["totals"].get("basic", 0) + i["totals"].get("cgst", 0)
                                         + i["totals"].get("sgst", 0) - i["totals"].get("discount", 0)
                                         - i["totals"].get("points_redeemed", 0)))
        for i in store.DOCS["sales_invoice"].values() if i["date"] == day.isoformat() and i["status"] == "SAVED")
    assert t and websync.build(day)["tables"]["RptDSRSmry"][0]["TotRndOffAmt"] == round(expected, 2)
