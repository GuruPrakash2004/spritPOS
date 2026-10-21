"""Puts the AI pieces together for the API and the Dashboard's restock list: sales history (ai.ItemDailySales) + current
stock + forecast + reorder rules.

Read-only on the database. It learns from the past forecast.HISTORY_DAYS days of sales (a year by default) up to the day
the sales history was last refreshed, and refuses history more than STALE_DAYS old, because the missing days would read
as "no sales".

The forecasts (slow: the model runs for every item) are made once and kept for FORECAST_SECONDS; the recommendations
are worked out again on every call from the stock at that moment. The restock list never waits for the forecasts:
restock_overlay() starts building them in the background and returns None until they are ready, and the list uses its
old formula meanwhile.
"""
import logging
import threading
import time
from datetime import date, timedelta

from fastapi import HTTPException

from .. import db, db_store, store
from . import enabled
from . import forecast as fc
from . import model_forecast, reorder, sales_history

logger = logging.getLogger("vlqms.ai")

FORECAST_SECONDS = 3600
STALE_DAYS = 3
_state: dict = {"at": 0.0, "end": None, "forecasts": None, "error": None}
_state_lock = threading.Lock()
_build_lock = threading.Lock()          # one build at a time
_building = threading.Event()


def _id_to_code() -> dict[str, str]:
    ids = {str(it["_itemid"]): code for code, it in store.ITEMS.items() if it.get("_itemid")}
    ids.update({str(k): store.merged_code(v) for k, v in store.MERGED_ITEM_IDS.items()})   # merged-away items count for the item they became
    return ids


def _non_liquor_groups() -> set[str]:
    """Names of the groups whose items count as accessories. Empty when no group is flagged liquor at all: some shops have
    every group flagged "not liquor" (MstITEMGROUP.LiqItem), and then the flag says nothing about which items are accessories."""
    if not any(g.get("liquor") for g in store.ITEM_GROUPS.values()):
        return set()
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
    if sales_history.LOADED_THROUGH and (last is None or sales_history.LOADED_THROUGH > last):
        last = sales_history.LOADED_THROUGH      # refreshed up to here; the days after the last sale simply had none
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


def _build() -> None:
    """Make the forecasts of every active item and keep them. Raises HTTPException if the sales history is unusable."""
    end = _history_end()
    series = _series(end)
    non_liquor = _non_liquor_groups()
    forecasts = model_forecast.forecast_many(
        {code: (s, store.ITEMS[code].get("group") in non_liquor) for code, s in series.items()}, end)
    with _state_lock:
        _state.update(at=time.monotonic(), end=end, forecasts=forecasts, error=None)
    logger.info("AI forecasts ready for %d items (history to %s)", len(forecasts), end)


def _fresh() -> bool:
    with _state_lock:
        return _state["forecasts"] is not None and time.monotonic() - _state["at"] < FORECAST_SECONDS


def forecasts(refresh: bool = False) -> dict:
    """The forecasts, built now if none are fresh (waits). {"end": date, "forecasts": {code: Forecast}}."""
    with _build_lock:
        if refresh or not _fresh():
            _building.set()
            try:
                _build()
            finally:
                _building.clear()
    with _state_lock:
        return {"end": _state["end"], "forecasts": _state["forecasts"]}


def _background_build() -> None:
    try:
        forecasts()
    except Exception as exc:     # the restock list keeps its old formula; the reason is kept for the log
        detail = exc.detail if isinstance(exc, HTTPException) else f"{type(exc).__name__}: {exc}"
        with _state_lock:
            _state["error"] = str(detail)
        logger.warning("AI forecasts not built: %s", detail)


def warm() -> None:
    """Start building the forecasts in the background (at startup, and whenever the restock list finds them stale)."""
    if not _building.is_set() and not _fresh():
        threading.Thread(target=_background_build, name="ai-forecasts", daemon=True).start()


def _recommend(code: str, fcst: fc.Forecast, qoh: float, cfg: reorder.ReorderConfig) -> dict:
    it = store.ITEMS[code]
    rec = reorder.recommend(fcst, qoh, it.get("bottles_per_case", 12), it.get("rol", 0), it.get("roq", 0), cfg=cfg)
    return {"item_code": code, "item_name": it.get("name", code), "group": it.get("group", ""),
            "bottles_per_case": it.get("bottles_per_case", 12), "qoh": qoh, "status": rec.status,
            "avg_daily": round(fcst.avg_daily, 2), "days_remaining": rec.days_remaining,
            "reorder_point": rec.reorder_point, "target_stock": rec.target_stock,
            "suggested_cases": rec.suggested_cases, "suggested_bottles": rec.suggested_bottles, "reason": rec.reason,
            "item_class": rec.item_class, "method": rec.method}


def compute(refresh: bool = False) -> dict:
    """{"as_of", "data_through", "history_days", "items": [...]} for all active items, most urgent first."""
    st = forecasts(refresh)
    qoh = current_qoh()
    cfg = reorder.ReorderConfig.from_env()
    recs = [_recommend(code, f, qoh.get(code, store.ITEMS[code]["qoh"]), cfg)
            for code, f in st["forecasts"].items() if code in store.ITEMS]
    recs.sort(key=lambda r: (reorder.STATUS_ORDER[r["status"]], r["days_remaining"] if r["days_remaining"] is not None else 1e9, r["item_code"]))
    return {"as_of": date.today().isoformat(), "data_through": st["end"].isoformat(), "history_days": fc.HISTORY_DAYS,
            "lead_time_days": cfg.lead_time_days, "safety_days": cfg.safety_days, "cover_days": cfg.cover_days, "items": recs}


def restock_overlay(qoh_of: dict[str, float]) -> dict[str, dict] | None:
    """For /reports/restock: {item_code: recommendation} for the items the model could forecast, using the stock the list
    already worked out. None while the forecasts are not ready (they are being built in the background) or can't be made;
    the list then uses its old formula. Items with too little history are left out: they keep the old formula too."""
    if not enabled():
        return None
    if not _fresh():
        warm()
    with _state_lock:
        forecasts_, end = _state["forecasts"], _state["end"]
    if forecasts_ is None or end is None or end < date.today() - timedelta(days=STALE_DAYS + 1):
        return None
    cfg = reorder.ReorderConfig.from_env()
    out = {}
    for code, f in forecasts_.items():
        if code in store.ITEMS and f.fallback is None:
            out[code] = _recommend(code, f, qoh_of.get(code, store.ITEMS[code]["qoh"]), cfg)
    return out


def forecast_for(code: str) -> dict:
    """One item's 14-day forecast and its recommendation."""
    item = store.find(store.ITEMS, code)
    if not item:
        raise HTTPException(404, f"Item '{code}' not found")
    code = item["code"]
    st = forecasts()
    f = st["forecasts"].get(code)
    if f is None:
        raise HTTPException(404, f"Item '{code}' is not active")
    rec = _recommend(code, f, current_qoh().get(code, item["qoh"]), reorder.ReorderConfig.from_env())
    days = [{"date": (st["end"] + timedelta(days=i + 1)).isoformat(), "qty": round(v, 2)} for i, v in enumerate(f.daily)]
    return {"item_code": code, "item_name": item.get("name", code), "item_class": f.item_class, "method": f.method,
            "fallback": f.fallback, "notes": f.notes, "history_days": f.history_days, "data_through": st["end"].isoformat(),
            "total_14_days": round(f.total, 1), "avg_daily": round(f.avg_daily, 2), "forecast": days, "recommendation": rec}
