"""app/ai/model_forecast.py with the model faked: no torch, no download needed."""
from datetime import date

import pytest

from app.ai import forecast as fc
from app.ai import model_forecast as mf

END = date(2026, 10, 4)


@pytest.fixture
def items():
    return {"FAST": ([4.0] * 56, False), "NEW": ([0.0] * 5 + [6.0] * 5, False), "NONE": ([], False)}


def test_items_with_enough_history_use_the_model(monkeypatch, items):
    seen = {}

    def fake(series_list, horizon):
        seen["n"], seen["h"] = len(series_list), horizon
        return [[7.0] * horizon for _ in series_list]
    monkeypatch.setattr(mf, "_predict", fake)
    monkeypatch.setenv("AI_USE_MODEL", "1")
    out = mf.forecast_many(items, END)
    assert seen == {"n": 1, "h": 14}                       # only FAST goes to the model
    assert out["FAST"].method == "chronos-bolt-tiny" and out["FAST"].daily == [7.0] * 14
    assert out["FAST"].item_class == fc.FAST and out["FAST"].fallback is None
    assert out["NEW"].fallback == "average" and out["NONE"].fallback == "rol_roq"   # short history keeps the fallbacks


def test_a_failing_model_leaves_the_statistical_forecast(monkeypatch, items):
    def boom(series_list, horizon):
        raise RuntimeError("model not available")
    monkeypatch.setattr(mf, "_predict", boom)
    monkeypatch.setenv("AI_USE_MODEL", "1")
    out = mf.forecast_many(items, END)
    assert out["FAST"].method == "dow_wma" and out["FAST"].daily == pytest.approx([4.0] * 14)


def test_no_model_call_when_nothing_has_history(monkeypatch):
    monkeypatch.setattr(mf, "_predict", lambda *a: pytest.fail("model should not be called"))
    assert mf.forecast_many({"X": ([], False)}, END)["X"].fallback == "rol_roq"


def test_model_name_can_be_changed(monkeypatch):
    assert mf.model_name() == "amazon/chronos-bolt-tiny"
    monkeypatch.setenv("AI_FORECAST_MODEL", "amazon/chronos-bolt-mini")
    assert mf.model_name() == "amazon/chronos-bolt-mini"


def test_the_model_is_off_unless_asked_for(monkeypatch, items):
    monkeypatch.delenv("AI_USE_MODEL", raising=False)
    monkeypatch.setattr(mf, "_predict", lambda *a: pytest.fail("model should not be called"))
    assert mf.forecast_many(items, END)["FAST"].method == "dow_wma"
