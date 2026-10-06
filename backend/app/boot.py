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
    sqlstore.remember_defaults()
    ok = db.init_db() and setup_db.is_ready()
    if ok:
        db_store.load_masters()
        designer.load()   # print options and screen fields from the Doc Designer's tables
    elif db.is_db_available():
        logger.warning("The database has no Web_ tables yet: no data until the Database setup has run")
    elif db.configured():
        logger.warning("The database can't be reached: no data and no sign-in until it comes back")
    else:
        logger.warning("No database set up: open the sign-in page's Database setup")
    sqlstore.load()   # the web's own rows, documents, stock moves and settings (Web_App* tables)
    if needs_admin():
        logger.warning("No active Administrator: create one on the sign-in page")
    if ok:
        # Heavy history loads in the background so the server starts at once; Web Sync (to the central
        # site, if WEB_SYNC_URL / WEB_SYNC_KEY are set) starts after it, so it never sends a half-loaded day.
        threading.Thread(target=_load_history_then_sync, daemon=True).start()


def _load_history_then_sync() -> None:
    try:
        designer.refresh()   # the Doc Designer may have changed the print options since the last start
    except Exception as e:
        logger.warning("Doc Designer settings not refreshed from the legacy tables: %s", e)
        designer.load()   # the refresh may have stopped after emptying a table: read what is there
    db_store.load_transactions()
    websync.start()


def reload_after_setup() -> None:
    """The database is connected and its web tables exist: load the shop's data (the connection is already open)."""
    sqlstore.reload()   # also loads the bill history
    designer.load()
    websync.start()
