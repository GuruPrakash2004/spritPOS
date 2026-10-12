"""Reorder calculator: turns an item's demand forecast and stock into a draft purchase suggestion. Pure Python.

  reorder point = forecast demand over the lead time + safety stock (safety days of average demand)
  target stock  = forecast demand over lead time + cover days + safety stock
  suggested qty = target stock - stock on hand, never above MAXLEVEL, never below MINLEVEL (when set),
                  rounded up to whole cases

Pending purchase orders are ignored (the shop has no purchase-order table; owner's decision). Stock on hand
below zero is treated as zero. The forecast covers 14 days; beyond that its average is used.

Status:
  Reorder now            stock is at or under the reorder point (or out of stock with demand)
  Reorder soon           stock will reach the reorder point within SOON_DAYS
  Monitor                enough stock, or no demand
  Insufficient history   too little sales history for the model (forecast.fallback is set): "average" runs
                         the same sums on the existing average-sales rate, "rol_roq" suggests the item's ROQ
                         when stock is at or under its ROL
Rounding up can pass MAXLEVEL by part of a case; then the quantity is rounded down to the cases that fit.
"""
import math
import os
from dataclasses import dataclass

from .forecast import Forecast

NOW, SOON, MONITOR, INSUFFICIENT = "Reorder now", "Reorder soon", "Monitor", "Insufficient history"
STATUS_ORDER = {NOW: 0, SOON: 1, INSUFFICIENT: 2, MONITOR: 3}


def _env_float(key: str, default: float) -> float:
    try:
        return float(os.environ.get(key, "").split("#")[0].strip() or default)
    except ValueError:
        return default


@dataclass
class ReorderConfig:
    lead_time_days: float = 3.0     # AI_LEAD_TIME_DAYS: order placed to stock on the shelf
    safety_days: float = 2.0        # AI_SAFETY_DAYS: spare days of average demand
    cover_days: float = 14.0        # AI_COVER_DAYS: days of sales an order should cover after it arrives
    soon_days: float = 7.0          # AI_SOON_DAYS: "Reorder soon" window before the reorder point is reached

    @classmethod
    def from_env(cls) -> "ReorderConfig":
        d = cls()
        return cls(_env_float("AI_LEAD_TIME_DAYS", d.lead_time_days), _env_float("AI_SAFETY_DAYS", d.safety_days),
                   _env_float("AI_COVER_DAYS", d.cover_days), _env_float("AI_SOON_DAYS", d.soon_days))


@dataclass
class Recommendation:
    status: str
    qoh: float
    days_remaining: float | None      # days the stock lasts at the forecast rate; None when nothing is forecast to sell
    reorder_point: float
    target_stock: float
    suggested_bottles: int
    suggested_cases: int
    reason: str
    item_class: str = ""
    method: str = ""


def demand_over(daily: list[float], days: float) -> float:
    """Forecast demand over the next `days` days (may be fractional); past the forecast, its daily average."""
    total, left = 0.0, max(days, 0.0)
    for v in daily:
        take = min(1.0, left)
        total += v * take
        left -= take
        if left <= 0:
            return total
    return total + left * (sum(daily) / len(daily) if daily else 0.0)


def days_of_stock(daily: list[float], qoh: float) -> float | None:
    """How many days `qoh` bottles last at the forecast rate (None if nothing is forecast to sell)."""
    if qoh <= 0:
        return 0.0
    avg = sum(daily) / len(daily) if daily else 0.0
    if avg <= 0:
        return None
    used = 0.0
    for i, v in enumerate(daily):
        if v > 0 and used + v >= qoh:
            return i + (qoh - used) / v
        used += v
    return len(daily) + (qoh - used) / avg


def _cases(raw: float, qoh: float, bpc: int, max_level: float | None) -> int:
    """Whole cases for `raw` bottles, rounded up; rounded down instead if the round-up would pass MAXLEVEL."""
    cases = math.ceil(raw / bpc - 1e-9)
    if max_level and qoh + cases * bpc > max_level:
        cases = max(int((max_level - qoh) // bpc), 0)
    return cases


def recommend(fc: Forecast, qoh: float, bottles_per_case: int = 12, rol: float = 0, roq: float = 0,
              min_level: float | None = None, max_level: float | None = None,
              cfg: ReorderConfig | None = None) -> Recommendation:
    cfg = cfg or ReorderConfig()
    bpc = max(int(bottles_per_case or 1), 1)
    stock = max(qoh, 0.0)
    min_level = min_level or None            # 0 / missing means "not set"
    max_level = max_level or None
    daily = fc.daily
    avg = sum(daily) / len(daily) if daily else 0.0
    left = days_of_stock(daily, stock)

    def result(status, bottles, reason, rp=0.0, target=0.0):
        cases = bottles // bpc
        return Recommendation(status, qoh, None if left is None else round(left, 1), round(rp, 1), round(target, 1),
                              cases * bpc, cases, reason, fc.item_class, fc.method)

    if fc.fallback == "rol_roq":
        if stock <= rol and roq > 0:
            cases = _cases(roq, stock, bpc, max_level)
            return result(INSUFFICIENT, cases * bpc, f"Too little sales history; stock {stock:g} is at or under ROL {rol:g}, so ROQ {roq:g} is suggested")
        return result(INSUFFICIENT, 0, f"Too little sales history; stock {stock:g} is above ROL {rol:g}")

    safety = avg * cfg.safety_days
    rp = demand_over(daily, cfg.lead_time_days) + safety
    target = demand_over(daily, cfg.lead_time_days + cfg.cover_days) + safety
    basis = f"{avg:.1f}/day" + (" (average sales, little history)" if fc.fallback == "average" else "")

    if avg <= 0:
        status, why = MONITOR, f"No sales forecast; stock {stock:g}"
    elif stock <= rp:
        status = NOW
        why = f"Stock {stock:g} lasts {left:.1f} days at {basis}; at or under reorder point {rp:.0f} (lead {cfg.lead_time_days:g}d + safety {cfg.safety_days:g}d)"
    elif stock <= rp + cfg.soon_days * avg:
        status = SOON
        why = f"Stock {stock:g} lasts {left:.1f} days at {basis}; reaches reorder point {rp:.0f} within {cfg.soon_days:g} days"
    else:
        status, why = MONITOR, f"Stock {stock:g} lasts {left:.1f} days at {basis}; reorder point {rp:.0f}"

    bottles = 0
    if status in (NOW, SOON):
        raw = target - stock
        if max_level:
            raw = min(raw, max_level - stock)
        if min_level:
            raw = max(raw, min_level - stock)
        if raw > 0:
            bottles = _cases(raw, stock, bpc, max_level) * bpc
        if bottles == 0 and max_level and stock >= max_level - bpc:
            why += "; no case fits under MAXLEVEL"
    if fc.fallback == "average":
        why = "Too little sales history. " + why
        status = INSUFFICIENT
    return result(status, bottles, why, rp, target)


def rank(recs: list[tuple[str, Recommendation]]) -> list[tuple[str, Recommendation]]:
    """(item_code, recommendation) pairs: Reorder now first, then soon; fewest days of stock first inside a status."""
    return sorted(recs, key=lambda x: (STATUS_ORDER[x[1].status], x[1].days_remaining if x[1].days_remaining is not None else 1e9, x[0]))
