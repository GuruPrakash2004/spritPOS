"""app/ai/backtest.py on small invented histories with hand-calculated numbers."""
from datetime import date

import pytest

from app.ai import backtest as bt
from app.ai import reorder

CFG = reorder.ReorderConfig(lead_time_days=3, safety_days=2, cover_days=14, soon_days=7)   # cover window 17 days
END = date(2026, 10, 9)


def test_score_when_the_forecast_is_exactly_right():
    s = bt.score([10.0] * 14, [10.0] * 17, CFG)
    # target = 17*10 + 2*10 = 190, needed 170: nothing short, 20 spare
    assert (s["abs_err"], s["actual14"], s["forecast14"]) == (0, 140, 140)
    assert (s["needed"], s["short"], s["excess"]) == (170, 0, 20)


def test_score_when_demand_is_higher_than_forecast():
    s = bt.score([10.0] * 14, [12.0] * 17, CFG)
    assert s["abs_err"] == 28 and s["actual14"] == 168      # 14 days x 2 off
    assert (s["needed"], s["short"], s["excess"]) == (204, 14, 0)    # 204 - 190 short


def test_returns_only_days_do_not_count_as_negative_demand():
    assert bt.score([0.0] * 14, [-5.0] * 17, CFG)["needed"] == 0


def test_origins_stay_inside_the_series_and_step_back():
    o = bt.origins(200, 120, 7, 17)
    assert o[0] == 183 and o[-1] >= 80 and all(a - b == 7 for a, b in zip(o, o[1:]))
    assert bt.origins(40, 120, 7, 17) == []            # too short: history would be under 28 days


def const(value):
    return lambda histories, _acc: [[value] * 14 for _ in histories]


def test_the_exact_method_beats_a_bad_one_and_is_named_the_winner():
    series = {"A": [5.0] * 200}
    rows = bt.run(series, {}, END, {"current": bt.current_formula, "bad": const(50.0)}, days=120, step=7, cfg=CFG)
    agg = {(a["class"], a["method"]): a for a in bt.aggregate(rows)}
    good, bad = agg[("fast", "current")], agg[("fast", "bad")]
    assert good["wape"] == 0 and good["stockout_rate_pct"] == 0.0
    assert bad["wape"] == pytest.approx(9.0)              # forecasting 50 for 5: error 45 on 5
    assert bad["bias_pct"] == 900.0 and bad["excess_units"] > 0
    assert bt.winners(list(agg.values()))["fast"] == {"most_accurate": "current", "fewest_stockouts": "current"}


def test_an_under_forecasting_method_misses_stockouts():
    rows = bt.run({"A": [5.0] * 200}, {}, END, {"low": const(1.0)}, days=120, step=7, cfg=CFG)
    a = bt.aggregate(rows)[0]
    assert a["stockout_rate_pct"] == 100.0 and a["shortfall_units"] > 0 and a["bias_pct"] < 0


def test_items_without_history_are_scored_with_the_current_formula_only():
    series = {"NEW": [0.0] * 190 + [5.0] * 10}           # first sale 10 days ago
    rows = bt.run(series, {}, END, {"current": bt.current_formula, "other": const(3.0)}, days=120, step=7, cfg=CFG)
    insufficient = {r["method"] for r in rows if r["class"] == "insufficient"}
    assert insufficient == {"current"}


def test_the_accessory_flag_sets_the_class():
    rows = bt.run({"A": [5.0] * 200}, {"A": True}, END, {"current": bt.current_formula}, days=120, step=7, cfg=CFG)
    assert {r["class"] for r in rows} == {"accessory"}


def test_origin_dates_end_the_day_before_the_actual_window():
    rows = bt.run({"A": [5.0] * 200}, {}, END, {"current": bt.current_formula}, days=120, step=7, cfg=CFG)
    latest = max(r["origin"] for r in rows)
    # last origin t = 183: history ends on index 182; the series ends (index 199) on 2026-10-09 -> 17 days earlier
    assert latest == "2026-09-22"


def test_summary_names_each_class_winner():
    rows = bt.run({"A": [5.0] * 200}, {}, END, {"current": bt.current_formula, "bad": const(50.0)}, days=120, step=7, cfg=CFG)
    text = bt.summary(bt.aggregate(rows), END, 120, 7)
    assert "fast: most accurate = current" in text and "bad" in text
