"""Load the data into the in-memory store: at startup, and again after the one-time Database setup.

- No database set up in backend/.env: no data at all and no sign-in; the sign-in page shows Database setup.
- Set up but the Web_ tables aren't ready (no finished copy): no data, and the sign-in page offers the
  Database setup.
- Set up but unreachable: no data and no sign-in; the sign-in page says the database is offline.
- Ready: the shop's data. If no active user is an Administrator, the sign-in page's Database setup can
  create one (routers/setup.py, POST /setup/admin).
"""
import logging
import threading
import time

from . import db, db_store, designer, setup_db, sqlstore, store, websync

logger = logging.getLogger("vlqms.boot")


def needs_setup() -> bool:
    if not db.configured():
        return True
    return db.is_db_available() and not setup_db.is_ready()


def needs_admin() -> bool:
    """The shop's database is in use but no active user is an Administrator: the sign-in page's Database
    setup then creates one (routers/setup.py, POST /setup/admin)."""
    return sqlstore.enabled() and not any(
        u.get("role") == store.ADMIN_ROLE and u.get("active", True) for u in store.USERS.values())


def load_data() -> None:
    """Connect (if set up) and load; start the background jobs."""
    t0 = time.perf_counter()
    sqlstore.remember_defaults()
    ok = db.init_db() and setup_db.is_ready()
    logger.info("Database connected and checked in %.1fs", time.perf_counter() - t0)
    if ok:
        db_store.load_masters()
        t1 = time.perf_counter()
        designer.load()   # print options and screen fields from the Doc Designer's tables
        logger.info("Designer settings loaded in %.1fs", time.perf_counter() - t1)
    elif db.is_db_available():
        logger.warning("The database has no Web_ tables yet: no data until the Database setup has run")
    elif db.configured():
        logger.warning("The database can't be reached: no data and no sign-in until it comes back")
    else:
        logger.warning("No database set up: open the sign-in page's Database setup")
    t2 = time.perf_counter()
    sqlstore.load()   # the web's own rows, documents, stock moves and settings (Web_App* tables)
    logger.info("Web data loaded in %.1fs (startup total %.1fs)", time.perf_counter() - t2, time.perf_counter() - t0)
    if needs_admin():
        logger.warning("No active Administrator: create one on the sign-in page")
    if ok:
        # Heavy history loads in the background so the server starts at once; Web Sync (to the central
        # site, if WEB_SYNC_URL / WEB_SYNC_KEY are set) starts after it, so it never sends a half-loaded day.
        threading.Thread(target=_load_history_then_sync, daemon=True).start()


def _load_history_then_sync() -> None:
    # The Doc Designer was read a moment ago (load_data); it is read again only on a change (/designer/version) or
    # from About → Refresh. The masters' extra database columns are read here, off the start-up path.
    try:
        sqlstore.reload_extras()
    except Exception as e:
        logger.warning("Extra master columns not read: %s", e)
    db_store.load_transactions()
    websync.start()


def reload_after_setup() -> None:
    """The database is connected and its web tables exist: load the shop's data (the connection is already open)."""
    sqlstore.reload()   # also loads the bill history
    designer.load()
    websync.start()
