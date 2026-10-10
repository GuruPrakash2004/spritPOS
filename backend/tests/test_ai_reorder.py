"""app/ai/reorder.py with hand-calculated numbers.
Flat forecast of 10 bottles/day, 12 per case, lead 3, safety 2, cover 14 days:
  reorder point = 3*10 + 2*10 = 50      target = 17*10 + 20 = 190      "soon" below 50 + 7*10 = 120"""
from datetime import date

import pytest

from app.ai import reorder as ro
from app.ai.forecast import Forecast

CFG = ro.ReorderConfig(lead_time_days=3, safety_days=2, cover_days=14, soon_days=7)


def flat(rate=10.0, cls="fast", fallback=None):
    return Forecast(cls, "dow_wma", [rate] * 14, date(2026, 10, 5), fallback=fallback)


def test_demand_over_whole_partial_and_beyond_the_forecast():
    assert ro.demand_over([10.0] * 14, 3) == 30
    assert ro.demand_over([10.0] * 14, 2.5) == 25
    assert ro.demand_over([10.0] * 14, 17) == 170      # 14 days + 3 more at the 10/day average
    assert ro.demand_over([1.0, 3.0] + [0.0] * 12, 2) == 4


def test_days_of_stock():
    d = [10.0] * 14
    assert ro.days_of_stock(d, 25) == pytest.approx(2.5)
    assert ro.days_of_stock(d, 150) == pytest.approx(15.0)    # 140 in the forecast, 10 more at 10/day
    assert ro.days_of_stock(d, 0) == 0
    assert ro.days_of_stock([0.0] * 14, 50) is None


def test_reorder_now_rounds_up_to_cases():
    r = ro.recommend(flat(), qoh=40, bottles_per_case=12, cfg=CFG)
    assert r.status == ro.NOW
    assert (r.reorder_point, r.target_stock, r.days_remaining) == (50, 190, 4.0)
    # 190 - 40 = 150 bottles -> 12.5 cases -> 13 cases = 156
    assert (r.suggested_cases, r.suggested_bottles) == (13, 156)
    assert "4.0 days" in r.reason and "reorder point 50" in r.reason


def test_reorder_soon():
    r = ro.recommend(flat(), qoh=100, bottles_per_case=12, cfg=CFG)
    assert r.status == ro.SOON
    assert (r.suggested_cases, r.suggested_bottles) == (8, 96)        # 90 bottles -> 7.5 -> 8 cases


def test_monitor_suggests_nothing():
    r = ro.recommend(flat(), qoh=200, bottles_per_case=12, cfg=CFG)
    assert (r.status, r.suggested_bottles) == (ro.MONITOR, 0)


def test_out_of_stock_with_demand_is_reorder_now():
    r = ro.recommend(flat(), qoh=0, bottles_per_case=12, cfg=CFG)
    assert (r.status, r.days_remaining, r.suggested_cases) == (ro.NOW, 0.0, 16)    # 190 -> 15.8 -> 16


def test_negative_stock_counts_as_zero():
    assert ro.recommend(flat(), qoh=-30, bottles_per_case=12, cfg=CFG).suggested_cases == 16


def test_no_demand_is_monitor_with_no_days_left():
    r = ro.recommend(flat(0.0), qoh=50, cfg=CFG)
    assert (r.status, r.days_remaining, r.suggested_bottles) == (ro.MONITOR, None, 0)


def test_maxlevel_caps_and_never_rounds_past_it():
    # raw = min(150, 120-40=80); 80 -> 7 cases = 84 would make 124 > 120, so 6 cases = 72
    r = ro.recommend(flat(), qoh=40, bottles_per_case=12, max_level=120, cfg=CFG)
    assert (r.suggested_cases, r.suggested_bottles) == (6, 72)


def test_minlevel_lifts_a_small_order():
    # 1/day: reorder point 5, target 19; qoh 4 -> raw 15; MINLEVEL 30 -> raw 26 -> 3 cases = 36
    r = ro.recommend(flat(1.0), qoh=4, bottles_per_case=12, min_level=30, cfg=CFG)
    assert r.status == ro.NOW and r.suggested_bottles == 36


def test_unset_levels_are_ignored():
    a = ro.recommend(flat(), qoh=40, min_level=0, max_level=0, cfg=CFG)
    assert a.suggested_bottles == 156


def test_no_history_uses_rol_and_roq():
    fc = Forecast("insufficient", "rol_roq", [0.0] * 14, fallback="rol_roq")
    low = ro.recommend(fc, qoh=10, bottles_per_case=12, rol=12, roq=48, cfg=CFG)
    assert (low.status, low.suggested_bottles, low.suggested_cases) == (ro.INSUFFICIENT, 48, 4)
    assert "ROL 12" in low.reason
    ok = ro.recommend(fc, qoh=20, bottles_per_case=12, rol=12, roq=48, cfg=CFG)
    assert (ok.status, ok.suggested_bottles) == (ro.INSUFFICIENT, 0)


def test_short_history_runs_the_sums_on_the_average_but_stays_insufficient():
    r = ro.recommend(flat(10.0, "insufficient", fallback="average"), qoh=40, bottles_per_case=12, cfg=CFG)
    assert r.status == ro.INSUFFICIENT and r.suggested_bottles == 156
    assert r.reason.startswith("Too little sales history.")


def test_cases_of_one_bottle_when_bpc_is_missing():
    assert ro.recommend(flat(), qoh=40, bottles_per_case=0, cfg=CFG).suggested_bottles == 150


def test_ranking_puts_the_most_urgent_first():
    items = [("A", ro.recommend(flat(), qoh=200, cfg=CFG)), ("B", ro.recommend(flat(), qoh=100, cfg=CFG)),
             ("C", ro.recommend(flat(), qoh=10, cfg=CFG)), ("D", ro.recommend(flat(), qoh=40, cfg=CFG))]
    assert [c for c, _ in ro.rank(items)] == ["C", "D", "B", "A"]


def test_config_from_env(monkeypatch):
    monkeypatch.setenv("AI_LEAD_TIME_DAYS", "5")
    monkeypatch.setenv("AI_SAFETY_DAYS", "oops")
    c = ro.ReorderConfig.from_env()
    assert (c.lead_time_days, c.safety_days, c.cover_days) == (5, 2, 14)
