"""Rolling-origin backtest of the demand forecasts against what actually sold. Pure Python; no database here
(scripts/backtest.py reads ai.ItemDailySales, read-only, and calls this).

For every item and every origin day t (one every `step` days over the last `days` days):
  history = the item's daily net sales before t (from its first sale), actual = the next days
  every method forecasts the next 14 days from the history alone; nothing after t is ever seen.

Methods:  current (the existing formula: average of the last 15 days, as /reports/restock), dow_wma, tsb,
          croston, and chronos (the pretrained model) when it is installed.
Items with too little history are class "insufficient": only `current` is scored there (the others fall back to it).

Measures, per item class and method:
  wape        sum |forecast - actual| / sum actual over the 14 days (0 = perfect)
  bias_pct    (sum forecast - sum actual) / sum actual; positive = over-forecast
  stockout    share of item-origins with demand where the order-up-to level the method would set
              (reorder.py's target: demand over lead + cover days, plus safety stock) was below what actually
              sold in lead + cover days by at least one bottle: a missed stockout. shortfall_units = bottles short.
  excess_units  bottles of that order-up-to level that were not needed in lead + cover days.
This tests the stock level each method would aim for. It does not replay the shop's real stock history.
"""
import math
from collections import defaultdict
from datetime import date, timedelta
from typing import Callable

from . import forecast as fc
from . import reorder

HORIZON = fc.HORIZON_DAYS
CURRENT_WINDOW = 15      # days the existing restock formula averages

Method = Callable[[list[list[float]], list[bool]], list[list[float]]]


def current_formula(histories, _acc):
    out = []
    for h in histories:
        w = fc.demand(h)[-CURRENT_WINDOW:]
        out.append([sum(w) / len(w) if w else 0.0] * HORIZON)
    return out


def dow_method(histories, _acc):
    return [fc.dow_wma(h, date.today()) for h in histories]


def tsb_method(histories, _acc):
    return [[fc.tsb(h)] * HORIZON for h in histories]


def croston_method(histories, _acc):
    return [[fc.croston(h)] * HORIZON for h in histories]


def chronos_method(histories, _acc):
    from . import model_forecast
    return model_forecast._predict(histories, HORIZON)


def default_methods(use_model: bool = True) -> dict[str, Method]:
    m = {"current": current_formula, "dow_wma": dow_method, "tsb": tsb_method, "croston": croston_method}
    if use_model:
        m["chronos"] = chronos_method
    return m


def cover_days(cfg: reorder.ReorderConfig) -> int:
    return math.ceil(cfg.lead_time_days + cfg.cover_days)


def score(daily: list[float], actual: list[float], cfg: reorder.ReorderConfig) -> dict:
    """Errors of one forecast against the following days (len(actual) >= cover_days)."""
    a = fc.demand(actual)
    f14, a14 = daily[:HORIZON], a[:HORIZON]
    avg = sum(daily) / len(daily) if daily else 0.0
    target = reorder.demand_over(daily, cfg.lead_time_days + cfg.cover_days) + avg * cfg.safety_days
    needed = sum(a[:cover_days(cfg)])
    return {"abs_err": sum(abs(x - y) for x, y in zip(f14, a14)), "actual14": sum(a14), "forecast14": sum(f14),
            "needed": needed, "short": max(needed - target, 0.0), "excess": max(target - needed, 0.0)}


def origins(n: int, days: int, step: int, need: int) -> list[int]:
    """Indexes t into a series of length n: history is [:t], actual [t : t+need]. Latest first."""
    last, first = n - need, max(n - days, fc.MIN_HISTORY_DAYS)
    return list(range(last, first - 1, -max(step, 1))) if last >= first else []


def run(series: dict[str, list[float]], accessory: dict[str, bool], end: date, methods: dict[str, Method],
        days: int = 120, step: int = 7, cfg: reorder.ReorderConfig | None = None) -> list[dict]:
    """One row per item x origin x method. series: {item: daily net sales ending on `end`, oldest first}."""
    cfg = cfg or reorder.ReorderConfig()
    need = cover_days(cfg)
    rows: list[dict] = []
    longest = max((len(s) for s in series.values()), default=0)
    for t in origins(longest, days, step, need):
        eligible: dict[str, list[float]] = {}
        for item, s in series.items():
            if len(s) != longest or len(s) < t + need:
                continue
            first = next((i for i, x in enumerate(s[:t]) if x > 0), None)
            eligible[item] = s[first:t] if first is not None else []
        by_method: dict[str, list[str]] = defaultdict(list)
        classes = {item: fc.classify(h, accessory.get(item, False)) for item, h in eligible.items()}
        for item, cls in classes.items():
            for name in methods:
                if cls != fc.INSUFFICIENT or name == "current":
                    by_method[name].append(item)
        origin_day = end - timedelta(days=longest - t)    # the last day of the history
        for name, items in by_method.items():
            preds = methods[name]([eligible[i] for i in items], [accessory.get(i, False) for i in items])
            for item, daily in zip(items, preds):
                m = score(daily, series[item][t:t + need], cfg)
                rows.append({"item": item, "origin": origin_day.isoformat(), "class": classes[item], "method": name, **m})
    return rows


def aggregate(rows: list[dict]) -> list[dict]:
    groups: dict[tuple[str, str], list[dict]] = defaultdict(list)
    for r in rows:
        groups[(r["class"], r["method"])].append(r)
    out = []
    for (cls, method), rs in sorted(groups.items()):
        a14 = sum(r["actual14"] for r in rs)
        with_demand = [r for r in rs if r["needed"] > 0]
        out.append({
            "class": cls, "method": method, "item_origins": len(rs), "items": len({r["item"] for r in rs}),
            "wape": round(sum(r["abs_err"] for r in rs) / a14, 4) if a14 > 0 else None,
            "bias_pct": round((sum(r["forecast14"] for r in rs) - a14) / a14 * 100, 1) if a14 > 0 else None,
            "mae_per_day": round(sum(r["abs_err"] for r in rs) / (HORIZON * len(rs)), 3),
            "stockout_rate_pct": round(100 * sum(1 for r in with_demand if r["short"] >= 1) / len(with_demand), 1) if with_demand else None,
            "shortfall_units": round(sum(r["short"] for r in rs)),
            "excess_units": round(sum(r["excess"] for r in rs)),
        })
    return out


def winners(agg: list[dict]) -> dict[str, dict]:
    """Per class: the method with the lowest WAPE (most accurate) and the one with the fewest missed stockouts."""
    out = {}
    for cls in sorted({a["class"] for a in agg}):
        rows = [a for a in agg if a["class"] == cls]
        scored = [a for a in rows if a["wape"] is not None]
        stock = [a for a in rows if a["stockout_rate_pct"] is not None]
        out[cls] = {"most_accurate": min(scored, key=lambda a: a["wape"])["method"] if scored else None,
                    "fewest_stockouts": min(stock, key=lambda a: (a["stockout_rate_pct"], a["excess_units"]))["method"] if stock else None}
    return out


def summary(agg: list[dict], end: date, days: int, step: int) -> str:
    lines = [f"Backtest to {end.isoformat()}: origins every {step} days over the last {days} days, 14-day forecasts.", ""]
    for cls, w in winners(agg).items():
        lines.append(f"{cls}: most accurate = {w['most_accurate']}; fewest missed stockouts = {w['fewest_stockouts']}")
        for a in (x for x in agg if x["class"] == cls):
            lines.append(f"    {a['method']:<8} WAPE {a['wape']}  bias {a['bias_pct']}%  stockouts {a['stockout_rate_pct']}%  "
                         f"short {a['shortfall_units']}  excess {a['excess_units']}  ({a['item_origins']} item-origins)")
    return "\n".join(lines)
