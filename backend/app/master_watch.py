"""Notices when someone changes a master table outside this app (the Windows vLQMS, SQL Server Management Studio, another tool) and
reloads the masters, so the web screens show the new data without a restart.

The masters live in memory (app/store.py), read from SQL Server once at start-up. Every `INTERVAL` seconds, while the app is being used
(a request calls `poke()`), a background thread reads a small fingerprint of each master table (row count + CHECKSUM_AGG of the rows,
NOLOCK, a few milliseconds in all). A different fingerprint means the data changed: `sqlstore.reload()` reads everything again and
`VERSION` goes up. The server sends VERSION in the `X-Data-Version` header of every reply; the browser (lib/api.js) sees it change and
tells the open screens to read again.

The app's own saves change these tables too: `own_write()` (called by the write middleware after a save that touched a master) makes
the next check just take a new baseline, so a save never causes a reload of its own. Bills, stock and vouchers are not watched
(`db_store.refresh_recent_sales` already brings in bills made elsewhere, and a busy counter would otherwise reload all the time).
"""
import logging
import threading
import time

from . import db

logger = logging.getLogger("vlqms.master_watch")

INTERVAL = 10.0          # seconds between two checks
VERSION = 1              # goes up every time the masters were reloaded because of an outside change

# Tables with no entry in sqlstore.LEGACY_MASTERS / master_specs.SPECS that the master screens still show.
EXTRA_TABLES = ("MstComp", "MstCompFinYear", "MstItemAlias", "MstSC", "MstSP", "MstLed", "MstLedgroup")

# A reload throws the in-memory rows away and reads them again, so it must never run while a write is between "the endpoint changed
# memory" and "the save reached SQL Server" (that change would be lost). main.py counts the writes in flight and holds new ones
# while RELOADING.
IN_FLIGHT = 0
RELOADING = False

_lock = threading.Lock()
_base: dict | None = None
_rebase = False
_at = 0.0


def tables() -> list[str]:
    """Every master table the app keeps in memory."""
    from . import sqlstore
    from .master_specs import SPECS
    names = [spec[0] for spec in sqlstore.LEGACY_MASTERS.values()] + [sp["table"] for sp in SPECS] + list(EXTRA_TABLES)
    return sorted(dict.fromkeys(names))


def _fingerprint() -> dict:
    """{table: (rows, checksum)}; a table the database lacks (or that can't be read) is None."""
    out: dict = {}
    for t in tables():
        try:
            r = db.fetchone(f"SELECT COUNT_BIG(*) AS n, CHECKSUM_AGG(BINARY_CHECKSUM(*)) AS c FROM {t} WITH (NOLOCK)")
            out[t] = (int(r["n"]), r["c"]) if r else None
        except Exception:   # noqa: BLE001 - a client database without this table
            out[t] = None
    return out


def own_write() -> None:
    """This app just saved a master: the next check only takes a new baseline (it is not an outside change)."""
    global _rebase
    _rebase = True


def rebaseline() -> None:
    """Take the tables' fingerprints now as the new baseline: used right after the app itself changed a master table (a save, or the Item
    Selling Rate correction), so only changes made by someone else count from here on. The window in which an outside edit could be
    absorbed is the few milliseconds this query takes."""
    global _base
    try:
        _base = _fingerprint()
    except Exception:   # noqa: BLE001 - the next check takes the baseline
        _base = None


def forget() -> None:
    """Start again from the next check (a different database, or everything was just reloaded)."""
    global _base
    _base = None


def _run() -> None:
    global _base, _rebase, VERSION
    from . import setup_db, sqlstore
    if not _lock.acquire(blocking=False):
        return
    try:
        if not db.is_db_available() or not setup_db.is_ready():
            return
        rebase, _rebase = _rebase, False
        now = _fingerprint()
        if _base is None or rebase:
            _base = now
            return
        changed = sorted(t for t in now if now[t] != _base.get(t))
        if not changed:
            return
        global RELOADING
        RELOADING = True
        try:
            deadline = time.monotonic() + 10
            while IN_FLIGHT > 0 and time.monotonic() < deadline:
                time.sleep(0.05)
            if IN_FLIGHT > 0:   # a save is taking long: try again at the next check (the baseline is kept)
                return
            logger.info("Master data changed outside this app (%s): reloading", ", ".join(changed))
            _base = now
            sqlstore.reload()
            VERSION += 1
        finally:
            RELOADING = False
    except Exception:   # noqa: BLE001 - never disturb a request; the next check tries again
        logger.warning("Master change check failed", exc_info=True)
    finally:
        _lock.release()


_started = False


def start() -> None:
    """Check on a timer of its own, so a change made in the database is noticed (and the Item Selling Rates brought up to date, see
    item_rates.py) even when nobody is using the web app at that moment. Started once, at start-up."""
    global _started
    if _started:
        return
    _started = True

    def loop() -> None:
        while True:
            time.sleep(INTERVAL)
            _run()   # never raises

    threading.Thread(target=loop, daemon=True, name="master-watch-timer").start()


def poke() -> None:
    """Called for every API request: starts a check in the background when the last one is older than INTERVAL."""
    global _at
    now = time.monotonic()
    if now - _at < INTERVAL:
        return
    _at = now
    threading.Thread(target=_run, daemon=True, name="master-watch").start()
