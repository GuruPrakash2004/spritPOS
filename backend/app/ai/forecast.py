"""Demand forecast per item for the next days, in bottles. Plain Python, no packages, no database.

Input is one item's daily net sales (app/ai/sales_history.load_series: oldest first, zero-filled, ending on the
last complete day). Negative days (only returns) count as 0 demand.

Classes (classify()):
  insufficient  fewer than MIN_HISTORY_DAYS days, or fewer than MIN_DEMAND_DAYS days with a sale
  accessory     the item's group is not liquor (the caller says so)
  fast          sold on at least FAST_DEMAND_RATIO of the last CLASS_WINDOW days
  slow          everything else

Methods:
  fast          day-of-week weighted moving average: each weekday is the average of that weekday's last
                DOW_WEEKS values, the newest weighted most (weights 1..n, oldest to newest)
  slow, accessory  TSB (Teunter-Syntetos-Babai): smoothed demand size x smoothed probability of a sale per day.
                Unlike Croston it decays when sales stop. croston() is kept for comparison (the Phase 5 backtest).
  insufficient  no model: the existing formula (average daily sales over the days we have, as /reports/restock
                does) when there is any history of sales, else the item's ROL/ROQ is the only guide
                (`fallback = "rol_roq"`, forecast 0) and app/ai/reorder.py applies them.
"""
import os
from dataclasses import dataclass, field
from datetime import date, timedelta

HISTORY_DAYS = int(os.environ.get("AI_HISTORY_DAYS", "").strip() or 120)   # the most recent days every method learns from
HORIZON_DAYS = 14
MIN_HISTORY_DAYS = 28
MIN_DEMAND_DAYS = 3
CLASS_WINDOW = 90
FAST_DEMAND_RATIO = 0.5
DOW_WEEKS = 8
ALPHA = 0.1          # smoothing of the demand size (Croston, TSB)
BETA = 0.1           # smoothing of the probability of a sale (TSB)

INSUFFICIENT, ACCESSORY, FAST, SLOW = "insufficient", "accessory", "fast", "slow"


@dataclass
class Forecast:
    item_class: str
    method: str                       # "dow_wma", "tsb" or "average" / "rol_roq" for the fallbacks
    daily: list[float]                # HORIZON_DAYS values, the first is the day after the last history day
    start: date | None = None
    fallback: str | None = None       # "average" or "rol_roq" when no model was used
    history_days: int = 0
    notes: list[str] = field(default_factory=list)

    @property
    def total(self) -> float:
        return sum(self.daily)

    @property
    def avg_daily(self) -> float:
        return self.total / len(self.daily) if self.daily else 0.0


def demand(series: list[float]) -> list[float]:
    """Net sales with returns-only days as 0."""
    return [max(float(x), 0.0) for x in series]


def classify(series: list[float], is_accessory: bool = False) -> str:
    d = demand(series)
    if len(d) < MIN_HISTORY_DAYS or sum(1 for x in d if x > 0) < MIN_DEMAND_DAYS:
        return INSUFFICIENT
    if is_accessory:
        return ACCESSORY
    recent = d[-CLASS_WINDOW:]
    return FAST if sum(1 for x in recent if x > 0) / len(recent) >= FAST_DEMAND_RATIO else SLOW


def dow_wma(series: list[float], end: date, horizon: int = HORIZON_DAYS, weeks: int = DOW_WEEKS) -> list[float]:
    """Day-of-week weighted moving average. `end` is the date of the last value in `series`."""
    d = demand(series)
    n = len(d)
    out = []
    for h in range(1, horizon + 1):
        # the same weekday as forecast day h, already in the past: index n-1+h-7k for k = ceil(h/7), +1, ...
        first = -(-h // 7)
        idx = [n - 1 + h - 7 * k for k in range(first, first + weeks)]
        obs = [d[i] for i in reversed(idx) if 0 <= i < n]          # oldest first
        weights = range(1, len(obs) + 1)                           # newest gets the largest weight
        out.append(sum(w * v for w, v in zip(weights, obs)) / sum(weights) if obs else 0.0)
    return out


def croston(series: list[float], alpha: float = ALPHA) -> float:
    """Croston's daily rate: smoothed demand size / smoothed interval between sales. 0 if nothing sold."""
    z = p = None
    last = -1
    for i, x in enumerate(demand(series)):
        if x <= 0:
            continue
        if z is None:
            z, p = x, float(i + 1)             # first sale: interval counted from the start of the history
        else:
            z += alpha * (x - z)
            p += alpha * ((i - last) - p)
        last = i
    return 0.0 if z is None else z / p


def tsb(series: list[float], alpha: float = ALPHA, beta: float = BETA) -> float:
    """TSB daily rate: smoothed probability of a sale x smoothed size of a sale. 0 if nothing sold."""
    z = prob = None
    for i, x in enumerate(demand(series)):
        if z is None:
            if x <= 0:
                continue
            z, prob = x, 1.0 / (i + 1)         # first sale: probability = 1 sale in the days so far
            continue
        prob += beta * ((1.0 if x > 0 else 0.0) - prob)
        if x > 0:
            z += alpha * (x - z)
    return 0.0 if z is None else prob * z


def forecast_item(series: list[float], end: date, is_accessory: bool = False, horizon: int = HORIZON_DAYS,
                  fallback_window: int | None = None) -> Forecast:
    """The next `horizon` days after `end` (the date of the last value in `series`). Only the last HISTORY_DAYS
    days (default 120) are used; the fallback average covers the same days unless fallback_window says otherwise."""
    series = series[-HISTORY_DAYS:]
    fallback_window = fallback_window or HISTORY_DAYS
    cls = classify(series, is_accessory)
    d = demand(series)
    start = end + timedelta(days=1)
    if cls == FAST:
        return Forecast(cls, "dow_wma", dow_wma(series, end, horizon), start, history_days=len(d))
    if cls in (SLOW, ACCESSORY):
        return Forecast(cls, "tsb", [tsb(series)] * horizon, start, history_days=len(d))
    # Not enough history: the existing formula if anything sold, else leave it to ROL / ROQ.
    window = d[-fallback_window:]
    if sum(window) > 0:
        rate = sum(window) / len(window)
        return Forecast(cls, "average", [rate] * horizon, start, fallback="average", history_days=len(d),
                        notes=["too little history for a model; average daily sales of the last %d days" % len(window)])
    return Forecast(cls, "rol_roq", [0.0] * horizon, start, fallback="rol_roq", history_days=len(d),
                    notes=["no sales to learn from; use the item's ROL / ROQ"])
