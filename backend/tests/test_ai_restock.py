"""/reports/restock with the AI forecast switched on. The forecast service is faked; the old formula is unchanged when the
flag is off or the forecasts are not ready."""
import pytest

from app import store
from app.ai import service


def ai_rec(code, status, avg_daily, days, bottles, reason="by the model"):
    return {"item_code": code, "status": status, "avg_daily": avg_daily, "days_remaining": days,
            "suggested_bottles": bottles, "reason": reason}


async def rows(client):
    return {r["item_code"]: r for r in (await client.get("/reports/restock", params={"days": 15})).json()}


async def test_flag_off_keeps_the_formula(client):
    r = next(iter((await rows(client)).values()))
    assert r["forecast"] == "formula" and r["ai_reason"] is None


async def test_ai_overrides_the_items_it_forecast(client, monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    code = next(c for c, it in store.ITEMS.items() if it["qoh"] > 0)
    monkeypatch.setattr(service, "restock_overlay",
                        lambda qoh_of: {code: ai_rec(code, "Reorder now", 7.5, 2.0, 96, "lasts 2 days")})
    got = await rows(client)
    r = got[code]
    assert (r["forecast"], r["status"], r["avg_daily"], r["days_left"], r["suggested_qty"]) == ("ai", "CRITICAL", 7.5, 2.0, 96)
    assert r["needs_restock"] is True and r["ai_reason"] == "lasts 2 days"
    other = next(v for c, v in got.items() if c != code)
    assert other["forecast"] == "formula"          # items the model did not forecast keep the old formula


async def test_ai_monitor_means_ok_and_soon_means_low(client, monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    a, b = [c for c, it in store.ITEMS.items() if it["qoh"] > 0][:2]
    monkeypatch.setattr(service, "restock_overlay", lambda q: {a: ai_rec(a, "Monitor", 1.0, 40.0, 0),
                                                                b: ai_rec(b, "Reorder soon", 5.0, 9.0, 48)})
    got = await rows(client)
    assert got[a]["status"] == "OK" and got[a]["needs_restock"] is False
    assert got[b]["status"] == "LOW" and got[b]["suggested_qty"] == 48


async def test_out_of_stock_stays_out_of_stock(client, monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    code = next(iter(store.ITEMS))
    store.ITEMS[code]["qoh"] = 0
    monkeypatch.setattr(service, "restock_overlay", lambda q: {code: ai_rec(code, "Reorder now", 4.0, 0.0, 60)})
    r = (await rows(client))[code]
    assert r["status"] == "OUT_OF_STOCK" and r["days_left"] == 0.0 and r["suggested_qty"] == 60


async def test_no_overlay_means_the_old_formula(client, monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    monkeypatch.setattr(service, "restock_overlay", lambda q: None)
    assert {r["forecast"] for r in (await rows(client)).values()} == {"formula"}


def test_overlay_is_none_when_the_flag_is_off(monkeypatch):
    monkeypatch.delenv("AI_FORECAST_ENABLED", raising=False)
    assert service.restock_overlay({}) is None


def test_overlay_does_not_wait_for_the_forecasts(monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    started = []
    monkeypatch.setattr(service, "_fresh", lambda: False)
    monkeypatch.setattr(service, "warm", lambda: started.append(1))
    monkeypatch.setitem(service._state, "forecasts", None)
    assert service.restock_overlay({}) is None and started == [1]


def test_accessories_are_only_named_when_some_group_is_liquor(monkeypatch):
    monkeypatch.setattr(store, "ITEM_GROUPS", {"A": {"name": "WHISKY", "liquor": True}, "B": {"name": "SODA", "liquor": False}})
    assert service._non_liquor_groups() == {"SODA"}
    monkeypatch.setattr(store, "ITEM_GROUPS", {"A": {"name": "WHISKY", "liquor": False}, "B": {"name": "SODA", "liquor": False}})
    assert service._non_liquor_groups() == set()      # every group flagged non-liquor: the flag tells nothing
