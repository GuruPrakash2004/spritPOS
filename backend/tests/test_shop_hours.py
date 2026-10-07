"""The Dashboard's Open / Closed (app/shop_hours.py): Karnataka retail liquor hours, closed on Holiday Master dates, no
Google call. The map shown in Company Master is the iframe the owner pastes (Company.map_embed)."""
from datetime import datetime, timedelta, timezone

from app import shop_hours, store


def _utc(local: str) -> datetime:   # a time in India, as UTC
    return datetime.fromisoformat(local).replace(tzinfo=timezone.utc) - timedelta(minutes=330)


def test_open_between_ten_and_ten_every_day():
    for day in ("2026-10-05", "2026-10-04"):   # a Monday and a Sunday
        assert shop_hours.status(_utc(f"{day}T10:00"))["open_now"] is True
        assert shop_hours.status(_utc(f"{day}T21:59"))["open_now"] is True
        assert shop_hours.status(_utc(f"{day}T09:59"))["open_now"] is False
        assert shop_hours.status(_utc(f"{day}T22:30"))["open_now"] is False
    out = shop_hours.status(_utc("2026-10-05T12:00"))
    assert out["today"] == "10:00 AM – 10:30 PM" and out["configured"] is True and out["error"] is None


def test_closed_all_day_on_a_holiday(monkeypatch):
    monkeypatch.setitem(store.HOLIDAYS, "2026-10-05", {"code": "2026-10-05", "name": "Gandhi Jayanti"})
    out = shop_hours.status(_utc("2026-10-05T12:00"))
    assert out["open_now"] is False and out["holiday"] == "Gandhi Jayanti" and "Gandhi Jayanti" in out["today"]
    assert shop_hours.status(_utc("2026-10-06T12:00"))["open_now"] is True


def test_the_timings_can_be_changed_in_env(monkeypatch):
    monkeypatch.setenv("SHOP_OPEN_TIME", "09:00")
    monkeypatch.setenv("SHOP_CLOSE_TIME", "23:30")
    assert shop_hours.status(_utc("2026-10-05T09:00"))["open_now"] is True
    assert shop_hours.status(_utc("2026-10-05T23:29"))["open_now"] is True
    assert shop_hours.status(_utc("2026-10-05T23:30"))["open_now"] is False
    monkeypatch.setenv("SHOP_OPEN_TIME", "nonsense")
    assert shop_hours.hours()[0].hour == 10   # an unreadable value falls back to the default


async def test_company_hours_endpoint(client):
    res = await client.get("/company/hours")
    assert res.status_code == 200 and res.json()["open_now"] in (True, False)


EMBED = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3888"


async def test_the_map_is_kept_as_the_pasted_frame_link(client):
    body = {**(await client.get("/masters/company")).json()}
    body.pop("fin_years", None)
    iframe = f'<iframe src="{EMBED}" width="600" height="450" style="border:0;" loading="lazy"></iframe>'
    res = await client.put("/masters/company", json={**body, "map_embed": iframe})
    assert res.status_code == 200 and res.json()["map_embed"] == EMBED
    assert (await client.put("/masters/company", json={**body, "map_embed": EMBED})).json()["map_embed"] == EMBED
    other = await client.put("/masters/company", json={**body, "map_embed": "https://evil.example.com/maps/embed"})
    assert other.status_code == 422
    assert (await client.put("/masters/company", json={**body, "map_embed": ""})).json()["map_embed"] == ""
