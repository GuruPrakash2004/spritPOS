"""The footer's Open / Closed from Google Maps (app/shop_hours.py): the shop is searched by company name + address,
and whether it is open is worked out from its weekly hours in the shop's time zone. No real call is made (respx)."""
from datetime import datetime, timezone

import httpx
import pytest

from app import shop_hours, store

# Mon-Sat 10:00-22:30, Sunday closed; India (UTC+5:30).
PERIODS = [{"open": {"day": d, "hour": 10, "minute": 0}, "close": {"day": d, "hour": 22, "minute": 30}} for d in range(1, 7)]
PLACE = {"places": [{"id": "x", "displayName": {"text": "My Shop"}, "googleMapsUri": "https://maps.google.com/?cid=1",
                     "utcOffsetMinutes": 330, "businessStatus": "OPERATIONAL",
                     "regularOpeningHours": {"periods": PERIODS, "weekdayDescriptions": [
                         f"{d}: 10:00 AM – 10:30 PM" for d in ("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")]
                         + ["Sunday: Closed"]}}]}


@pytest.fixture(autouse=True)
def _fresh(monkeypatch):
    shop_hours.reset()
    monkeypatch.setenv("GOOGLE_MAPS_API_KEY", "test-key")
    yield
    shop_hours.reset()


def _utc(local: str) -> datetime:   # a time in India, as UTC
    return datetime.fromisoformat(local).replace(tzinfo=timezone.utc) - __import__("datetime").timedelta(minutes=330)


def test_open_and_closed_from_the_weekly_hours(block_network):
    route = block_network.post(shop_hours.SEARCH_URL).mock(return_value=httpx.Response(200, json=PLACE))
    assert shop_hours.status(_utc("2026-10-05T11:00"))["open_now"] is True      # Monday 11:00
    late = shop_hours.status(_utc("2026-10-05T22:45"))                           # Monday 22:45
    assert late["open_now"] is False and late["today"] == "10:00 AM – 10:30 PM"
    assert shop_hours.status(_utc("2026-10-04T12:00"))["open_now"] is False     # Sunday
    assert route.call_count == 1   # looked up once, then worked out from the cached hours
    sent = route.calls[0].request
    assert sent.headers["X-Goog-Api-Key"] == "test-key"
    assert store.COMPANY["address"] in sent.content.decode()


def test_hours_past_midnight():
    periods = [{"open": {"day": 5, "hour": 18, "minute": 0}, "close": {"day": 6, "hour": 2, "minute": 0}}]   # Fri 18:00-Sat 02:00
    assert shop_hours.open_now(periods, datetime(2026, 10, 10, 1, 0))      # Saturday 01:00
    assert not shop_hours.open_now(periods, datetime(2026, 10, 10, 3, 0))
    assert shop_hours.open_now([{"open": {"day": 0, "hour": 0, "minute": 0}}], datetime(2026, 10, 10, 3, 0))   # 24 hours


def test_without_a_key_nothing_is_asked(block_network, monkeypatch):
    monkeypatch.delenv("GOOGLE_MAPS_API_KEY")
    out = shop_hours.status()
    assert out["configured"] is False and out["open_now"] is None


def test_a_refused_key_is_reported_not_raised(block_network):
    block_network.post(shop_hours.SEARCH_URL).mock(return_value=httpx.Response(403, json={"error": {"message": "API key not valid"}}))
    out = shop_hours.status()
    assert out["open_now"] is None and "API key not valid" in out["error"]


async def test_endpoint_needs_a_login_and_hides_the_key(anon_client, client, block_network):
    block_network.post(shop_hours.SEARCH_URL).mock(return_value=httpx.Response(200, json=PLACE))
    assert (await anon_client.get("/company/hours")).status_code == 401
    res = await client.get("/company/hours")
    assert res.status_code == 200 and "test-key" not in res.text and res.json()["place"] == "My Shop"


def test_the_place_picked_on_the_map_is_used(block_network, monkeypatch):
    monkeypatch.setitem(store.COMPANY, "place_id", "ChIJabc-123")
    route = block_network.get(shop_hours.DETAILS_URL + "ChIJabc-123").mock(return_value=httpx.Response(200, json=PLACE["places"][0]))
    out = shop_hours.status(_utc("2026-10-05T11:00"))
    assert out["open_now"] is True and out["place"] == "My Shop"
    assert route.called and "places." not in route.calls[0].request.headers["X-Goog-FieldMask"]


async def test_company_saves_the_picked_place(client):
    body = {**(await client.get("/masters/company")).json(), "place_id": "ChIJabc-123"}
    body.pop("fin_years")
    res = await client.put("/masters/company", json=body)
    assert res.status_code == 200 and res.json()["place_id"] == "ChIJabc-123"
    bad = await client.put("/masters/company", json={**body, "place_id": "x/../y"})
    assert bad.status_code == 422
