"""Pretrained forecast model from Hugging Face (default amazon/chronos-bolt-tiny, ~9M parameters, zero-shot).

Replaces the statistical methods of forecast.py for every item with enough history. Items with too little
history keep forecast.py's fallbacks (average sales, then ROL/ROQ), and so does everything if the model can't
be used (packages not installed, weights not downloaded, any error): the shop never loses its forecast.

Needs requirements-ai.txt (torch, chronos-forecasting). The weights download from Hugging Face on first use
and are cached; after that it runs offline on the CPU. Set AI_FORECAST_MODEL to use another Chronos-Bolt model.
The forecast is the average of the model's quantile forecasts (10% to 90%). The model's own "mean" output is only its
median, which collapses to about 0 for items that sell on some days only.
"""
import logging
import os
import threading
from datetime import date

from . import forecast as fc

logger = logging.getLogger("vlqms.ai")

DEFAULT_MODEL = "amazon/chronos-bolt-tiny"
CONTEXT_DAYS = fc.HISTORY_DAYS      # the days of past sales given to the model (a year by default, AI_HISTORY_DAYS)
BATCH = 64              # items per model call
QUANTILES = [0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9]

_pipeline = None
_failed: str | None = None
_lock = threading.Lock()


def use_model() -> bool:
    """AI_USE_MODEL in backend/.env (1/true/yes/on). Off by default: forecasts come from the statistical methods
    (day-of-week average for fast sellers and accessories, TSB for slow sellers, the old formula for items with little history)."""
    return os.environ.get("AI_USE_MODEL", "").split("#")[0].strip().lower() in ("1", "true", "yes", "on")


def model_name() -> str:
    return os.environ.get("AI_FORECAST_MODEL", "").strip() or DEFAULT_MODEL


def _load():
    global _pipeline, _failed
    with _lock:
        if _pipeline is None and _failed is None:
            try:
                import torch
                from chronos import BaseChronosPipeline
                _pipeline = BaseChronosPipeline.from_pretrained(model_name(), device_map="cpu", torch_dtype=torch.float32)
                logger.info("Forecast model %s loaded", model_name())
            except Exception as exc:   # not installed, offline on first use, ...: the statistical methods take over
                _failed = f"{type(exc).__name__}: {exc}"[:300]
                logger.warning("Forecast model not available (%s); using the statistical methods", _failed)
        return _pipeline


def available() -> bool:
    return _load() is not None


def _predict(series_list: list[list[float]], horizon: int) -> list[list[float]]:
    """The model's mean forecast for each series (not below 0). Replaced by tests."""
    import torch
    pipe = _load()
    if pipe is None:
        raise RuntimeError(_failed or "model not available")
    out: list[list[float]] = []
    for i in range(0, len(series_list), BATCH):
        ctx = [torch.tensor(fc.demand(s)[-CONTEXT_DAYS:], dtype=torch.float32) for s in series_list[i:i + BATCH]]
        quantiles, _ = pipe.predict_quantiles(ctx, prediction_length=horizon, quantile_levels=QUANTILES)
        mean = quantiles.mean(dim=-1)                                  # (items, horizon)
        out.extend([max(float(v), 0.0) for v in row] for row in mean)
    return out


def forecast_many(items: dict[str, tuple[list[float], bool]], end: date, horizon: int = fc.HORIZON_DAYS) -> dict[str, fc.Forecast]:
    """Forecast every item: {code: (daily net sales oldest first ending on `end`, is_accessory)} -> {code: Forecast}.
    Items with enough history go through the model in batches; the rest, or all of them if the model fails,
    use forecast.forecast_item."""
    result = {code: fc.forecast_item(s, end, acc, horizon) for code, (s, acc) in items.items()}
    if not use_model():     # the shop's own backtest: the statistical methods beat the model on stockouts and bias
        return result
    codes =[c for c, f in result.items() if f.fallback is None]      # the ones with enough history
    if not codes:
        return result
    try:
        predicted = _predict([items[c][0] for c in codes], horizon)
    except Exception as exc:
        logger.warning("Forecast model failed (%s); statistical methods kept", exc)
        return result
    name = model_name().split("/")[-1]
    for code, daily in zip(codes, predicted):
        old = result[code]
        result[code] = fc.Forecast(old.item_class, name, daily, old.start, history_days=old.history_days)
    return result
