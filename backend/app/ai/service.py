"""Puts the AI pieces together for the API: sales history (ai.ItemDailySales) + current stock + forecast + reorder rules.

Read-only on the database. Learns from the last forecast.HISTORY_DAYS days (120 by default) up to the day the sales
history was last refreshed; refuses to forecast from history that is more than STALE_DAYS old, because the
missing days would read as "no sales".
"""
import threading
import time
from datetime import date, timedelta

from fastapi import HTTPException

from .. import db, db_store, store
from . import forecast as fc
from . import model_forecast, reorder, sales_history

CACHE_SECONDS = 600
STALE_DAYS = 3
_cache: dict = {"at": 0.0, "data": None}
_lock = threading.Lock()


def _id_to_code() -> dict[str, str]:
    ids = {str(it["_itemid"]): code for code, it in store.ITEMS.items() if it.get("_itemid")}
    ids.update({str(k): store.merged_code(v) for k, v in store.MERGED_ITEM_IDS.items()})   # merged-away items count for the item they became
    return ids


def _non_liquor_groups() -> set[str]:
    return {g.get("name") for g in store.ITEM_GROUPS.values() if g.get("liquor") is False}


def current_qoh() -> dict[str, float]:
    """Stock now: live TrnQOH plus what this app has moved since it started (the same as /reports/restock)."""
    qoh = {code: it["qoh"] for code, it in store.ITEMS.items()}
    today = date.today()
    live = db_store.live_sales_and_stock(today, today)
    if live is not None:
        qoh.update(live["qoh"])
        for m in store.MOVEMENTS[store.DB_BASE_MOVEMENTS:]:
            qoh[m["item"]] = qoh.get(m["item"], 0) + m["qty"]
    return qoh


def _history_end() -> date:
    if not db.is_db_available():
        raise HTTPException(503, "No database connection")
    try:
        last = sales_history.last_loaded()
    except Exception as exc:
        raise HTTPException(503, f"ai.ItemDailySales cannot be read (was migrations/ai_001.sql run?): {db.safe_error(exc)[:200]}")
    if last is None:
        raise HTTPException(409, "The sales history is empty. Run: python -m app.ai.sales_history")
    if last < date.today() - timedelta(days=STALE_DAYS):
        raise HTTPException(409, f"The sales history stops at {last.isoformat()}. Run: python -m app.ai.sales_history")
    return last


def _series(end: date) -> dict[str, list[float]]:
    """Every active item's zero-filled daily net sales for the history window ending on `end`."""
    start = end - timedelta(days=fc.HISTORY_DAYS - 1)
    to_code = _id_to_code()
    rows: dict[str, list[dict]] = {}
    cur = start
    while cur <= end:                                           # a month at a time, gentle on a live database
        stop = min(cur + timedelta(days=29), end)
        for r in db.fetchall("SELECT ItemID, SaleDate AS day, NetQty AS net FROM ai.ItemDailySales WITH (NOLOCK) "
                             "WHERE SaleDate >= ? AND SaleDate <= ?", (cur.isoformat(), stop.isoformat())):
            code = to_code.get(str(r["ItemID"]))
            if code:
                rows.setdefault(code, []).append(r)
        cur = stop + timedelta(days=1)
    return {code: sales_history.zero_fill(rows.get(code, []), start, end)
            for code, it in store.ITEMS.items() if it.get("active") is not False}


def _recommend(code: str, fcst: fc.Forecast, qoh: float, cfg: reorder.ReorderConfig) -> dict:
    it = store.ITEMS[code]
    rec = reorder.recommend(fcst, qoh, it.get("bottles_per_case", 12), it.get("rol", 0), it.get("roq", 0), cfg=cfg)
    return {"item_code": code, "item_name": it.get("name", code), "group": it.get("group", ""),
            "bottles_per_case": it.get("bottles_per_case", 12), "qoh": qoh, "status": rec.status,
            "days_remaining": rec.days_remaining, "reorder_point": rec.reorder_point, "target_stock": rec.target_stock,
            "suggested_cases": rec.suggested_cases, "suggested_bottles": rec.suggested_bottles, "reason": rec.reason,
            "item_class": rec.item_class, "method": rec.method}


def compute(refresh: bool = False) -> dict:
    """{"as_of", "data_through", "history_days", "items": [...]} for all active items, most urgent first. Cached 10 minutes."""
    with _lock:
        if not refresh and _cache["data"] and time.monotonic() - _cache["at"] < CACHE_SECONDS:
            return _cache["data"]
        end = _history_end()
        series = _series(end)
        non_liquor = _non_liquor_groups()
        forecasts = model_forecast.forecast_many(
            {code: (s, store.ITEMS[code].get("group") in non_liquor) for code, s in series.items()}, end)
        qoh = current_qoh()
        cfg = reorder.ReorderConfig.from_env()
        recs = [_recommend(code, f, qoh.get(code, store.ITEMS[code]["qoh"]), cfg) for code, f in forecasts.items()]
        order = {c: i for i, (c, _) in enumerate(sorted(
            ((r["item_code"], r) for r in recs),
            key=lambda x: (reorder.STATUS_ORDER[x[1]["status"]], x[1]["days_remaining"] if x[1]["days_remaining"] is not None else 1e9, x[0])))}
        recs.sort(key=lambda r: order[r["item_code"]])
        data = {"as_of": date.today().isoformat(), "data_through": end.isoformat(), "history_days": fc.HISTORY_DAYS,
                "lead_time_days": cfg.lead_time_days, "safety_days": cfg.safety_days, "cover_days": cfg.cover_days, "items": recs}
        _cache.update(at=time.monotonic(), data=data)
        return data


def forecast_for(code: str) -> dict:
    """One item's 14-day forecast and its recommendation."""
    item = store.find(store.ITEMS, code)
    if not item:
        raise HTTPException(404, f"Item '{code}' not found")
    code = item["code"]
    end = _history_end()
    series = _series(end).get(code)
    if series is None:
        raise HTTPException(404, f"Item '{code}' is not active")
    acc = item.get("group") in _non_liquor_groups()
    f = model_forecast.forecast_many({code: (series, acc)}, end)[code]
    rec = _recommend(code, f, current_qoh().get(code, item["qoh"]), reorder.ReorderConfig.from_env())
    days = [{"date": (end + timedelta(days=i + 1)).isoformat(), "qty": round(v, 2)} for i, v in enumerate(f.daily)]
    return {"item_code": code, "item_name": item.get("name", code), "item_class": f.item_class, "method": f.method,
            "fallback": f.fallback, "notes": f.notes, "history_days": f.history_days, "data_through": end.isoformat(),
            "total_14_days": round(f.total, 1), "avg_daily": round(f.avg_daily, 2), "forecast": days, "recommendation": rec}
