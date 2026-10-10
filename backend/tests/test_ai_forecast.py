"""app/ai/forecast.py with hand-calculated examples. 2026-09-21 is a Monday, 2026-10-04 a Sunday."""
from datetime import date, timedelta

import pytest

from app.ai import forecast as fc

SUNDAY = date(2026, 10, 4)


def test_dow_wma_weights_the_newest_same_weekday_most():
    # 14 days Mon 09-21 .. Sun 10-04. Mondays are index 0 and 7 = 3 and 9; every other day 0.
    s = [0.0] * 14
    s[0], s[7] = 3, 9
    out = fc.dow_wma(s, SUNDAY, horizon=7)
    # Monday = (3*1 + 9*2) / (1+2) = 7
    assert out[0] == pytest.approx(7.0)
    assert out[1:] == [0.0] * 6


def test_dow_wma_for_the_second_week_uses_only_past_weeks():
    s = [0.0] * 14
    s[0], s[7] = 3, 9
    out = fc.dow_wma(s, SUNDAY, horizon=14)
    assert out[7] == pytest.approx(7.0)      # the Monday after next: same two Mondays, same weights


def test_dow_wma_ignores_weeks_beyond_the_window():
    s = [0.0] * 28
    s[0], s[7], s[14], s[21] = 100, 1, 1, 1   # four Mondays, the oldest is huge
    out = fc.dow_wma(s, date(2026, 10, 18), horizon=1, weeks=3)
    assert out[0] == pytest.approx(1.0)       # only the last three count: all 1


def test_croston_by_hand():
    # [0,4,0,0,2], alpha .5: first sale at index 1 -> z=4, p=2; second at index 4 -> interval 3,
    # z = 4 + .5(2-4) = 3, p = 2 + .5(3-2) = 2.5; rate = 3/2.5 = 1.2
    assert fc.croston([0, 4, 0, 0, 2], alpha=0.5) == pytest.approx(1.2)


def test_tsb_by_hand():
    # [0,4,0,0,2], alpha=beta=.5: z=4, prob=1/2; idx2: .25; idx3: .125; idx4: .125+.5(1-.125)=.5625, z=3 -> 1.6875
    assert fc.tsb([0, 4, 0, 0, 2], alpha=0.5, beta=0.5) == pytest.approx(1.6875)


def test_tsb_decays_when_sales_stop_but_croston_does_not():
    s = [5] + [0] * 60
    assert fc.tsb(s) < 0.2 and fc.croston(s) == pytest.approx(5.0)


def test_nothing_sold_is_zero():
    assert fc.tsb([0] * 40) == 0 and fc.croston([0] * 40) == 0


def test_returns_only_days_count_as_zero_demand():
    assert fc.demand([-3, 2, 0]) == [0.0, 2.0, 0.0]


def test_classification():
    daily = [3.0] * 60                                   # sells every day
    sparse = ([2.0] + [0.0] * 9) * 6                     # one day in ten
    assert fc.classify(daily) == fc.FAST
    assert fc.classify(sparse) == fc.SLOW
    assert fc.classify(daily, is_accessory=True) == fc.ACCESSORY
    assert fc.classify([3.0] * 10) == fc.INSUFFICIENT                     # under 28 days
    assert fc.classify([0.0] * 60) == fc.INSUFFICIENT                     # never sold
    assert fc.classify([1.0, 1.0] + [0.0] * 58) == fc.INSUFFICIENT        # only two days with a sale


def test_fast_item_forecast_has_14_days_starting_after_history():
    f = fc.forecast_item([4.0] * 56, SUNDAY)
    assert (f.item_class, f.method, len(f.daily), f.start) == (fc.FAST, "dow_wma", 14, date(2026, 10, 5))
    assert f.daily == pytest.approx([4.0] * 14)
    assert f.total == pytest.approx(56) and f.avg_daily == pytest.approx(4)


def test_slow_item_gets_a_flat_tsb_forecast():
    f = fc.forecast_item(([2.0] + [0.0] * 9) * 6, SUNDAY)
    assert f.item_class == fc.SLOW and f.method == "tsb"
    assert len(set(f.daily)) == 1 and 0 < f.daily[0] < 2


def test_short_history_with_sales_uses_the_existing_average():
    f = fc.forecast_item([0.0] * 5 + [6.0] * 5, SUNDAY)      # 10 days: last 10 average 3
    assert (f.item_class, f.fallback) == (fc.INSUFFICIENT, "average")
    assert f.daily == pytest.approx([3.0] * 14)


def test_no_history_hands_over_to_rol_roq():
    f = fc.forecast_item([], SUNDAY)
    assert (f.fallback, f.daily, f.history_days) == ("rol_roq", [0.0] * 14, 0)


def test_horizon_is_configurable():
    assert len(fc.forecast_item([1.0] * 40, SUNDAY, horizon=21).daily) == 21
