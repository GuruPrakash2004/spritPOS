"""AI stock forecasting and purchase-indent suggestions. Everything here is behind AI_FORECAST_ENABLED."""
import os


def enabled() -> bool:
    """AI_FORECAST_ENABLED in backend/.env (1/true/yes/on). Off by default; the existing stock formulas stay in use."""
    return os.environ.get("AI_FORECAST_ENABLED", "").strip().lower() in ("1", "true", "yes", "on")
