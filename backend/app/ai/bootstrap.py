"""Starts the AI restock model by itself when the backend starts, so a new shop needs no manual steps.

In a background thread (the server never waits for it, and any failure is only logged):
  1. creates the ai.* tables if they are missing (migrations/ai_001.sql, ai_002.sql: guarded, touch no desktop table),
  2. loads the sales history into ai.ItemDailySales (first run: the last 365 days, in small chunks; after that only new days),
  3. builds the forecasts so the first Dashboard / restock load is fast,
  and then repeats steps 2 and 3 every REFRESH_HOURS so the history never goes stale.
Needs AI_FORECAST_ENABLED=1 and a login on the shop's database that may create tables.
"""
import logging
import threading
import time
from pathlib import Path

from .. import db, setup_db
from . import enabled, sales_history, service

logger = logging.getLogger("vlqms.ai")

MIGRATIONS = Path(__file__).resolve().parents[2] / "migrations"
SCRIPTS = ("ai_001.sql", "ai_002.sql")
REFRESH_HOURS = 6
_started = threading.Event()


def ensure_schema() -> None:
    """Run the AI migrations (every CREATE is guarded, so running them again changes nothing)."""
    for name in SCRIPTS:
        for batch in setup_db.batches((MIGRATIONS / name).read_text(encoding="utf-8")):
            db.execute(batch)


def run_once() -> None:
    ensure_schema()
    result = sales_history.refresh()
    logger.info("AI sales history: %s", result)
    service.warm()


def _loop() -> None:
    while True:
        try:
            if not db.is_db_available():     # the data is still loading, or the database is not set up yet
                time.sleep(30)
                continue
            run_once()
        except Exception as exc:   # noqa: BLE001 - the restock list keeps its old formula; try again next round
            logger.warning("AI set-up/refresh failed: %s", db.safe_error(exc)[:300])
        time.sleep(REFRESH_HOURS * 3600)


def start() -> None:
    """Called once at startup; does nothing when AI_FORECAST_ENABLED is off."""
    if enabled() and not _started.is_set():
        _started.set()
        threading.Thread(target=_loop, name="ai-bootstrap", daemon=True).start()
