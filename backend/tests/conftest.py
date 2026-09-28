"""Shared pytest fixtures for the backend API tests.

- The app always runs on the seed data in app/store.py. The DB settings are blanked before the app is
  imported, and httpx's ASGITransport does not run the startup event, so tests never touch SQL Server.
- The in-memory store is put back after every test, so a test that sells stock or adds a master
  doesn't leak into the next one. (Document number counters are not reset: assert on the pattern
  of a doc number, never on its exact value.)
- `client` calls the FastAPI app in-process through httpx, with no server running, logged in as the
  seeded Administrator. `cashier_client` is logged in as a Cashier and `anon_client` not at all.
- respx is switched on for every test: any real outgoing HTTP call made with httpx fails unless the
  test mocks that route (see test_respx_example.py).
"""
import copy
import os

# Must run before `app` is imported: db.py reads these at import time and its .env loader only fills
# variables that are not already set.
os.environ["DB_HOST"] = ""
os.environ["DB_USER"] = ""
os.environ["DB_PASSWORD"] = ""
# Real password hashing, but with few rounds so the ~100 logins in the suite stay fast.
os.environ["PASSWORD_HASH_ITERATIONS"] = "1000"
# Don't write the real saved-state file; test_persistence.py points it at a temp file itself.
os.environ["VLQMS_STATE_DB"] = ""

import httpx  # noqa: E402
import pytest  # noqa: E402
import respx  # noqa: E402

from app import persistence, store  # noqa: E402
from app.main import app  # noqa: E402
from app.routers import auth  # noqa: E402


def _store_tables() -> dict:
    """The store's module-level tables (ITEMS, DOCS, MOVEMENTS, …) and plain values (DB_BASE_MOVEMENTS)."""
    return {
        name: value for name, value in vars(store).items()
        if name.isupper() and isinstance(value, (dict, list, int, float, str))
    }


@pytest.fixture(autouse=True)
def reset_store():
    """Snapshot the store before each test and restore it afterwards.

    Restored in place (clear + refill) because the routers keep references to these dicts,
    e.g. masters.crud("items", store.ITEMS, ...).
    """
    snapshot = copy.deepcopy(_store_tables())
    auth.reset_login_failures()
    persistence.reset_memory_outbox()   # the write-back outbox (in memory while saving is off)
    yield
    persistence.reset_memory_outbox()
    for name, saved in snapshot.items():
        current = getattr(store, name)
        if isinstance(current, dict):
            current.clear()
            current.update(saved)
        elif isinstance(current, list):
            current[:] = saved
        else:
            setattr(store, name, saved)


@pytest.fixture(autouse=True)
def block_network():
    """Fail any real outgoing httpx request that a test hasn't mocked. Calls to the app itself go
    through ASGITransport, which respx does not intercept."""
    with respx.mock(assert_all_called=False) as router:
        yield router


def _app_client() -> httpx.AsyncClient:
    """httpx client wired straight to the FastAPI app (no login)."""
    return httpx.AsyncClient(transport=httpx.ASGITransport(app=app), base_url="http://test")


async def _log_in(c: httpx.AsyncClient, user: str, password: str) -> httpx.AsyncClient:
    """Log `c` in: later requests send the user's Bearer token."""
    res = await c.post("/auth/login", json={"user": user, "password": password})
    assert res.status_code == 200, res.text
    c.headers["Authorization"] = f"Bearer {res.json()['access_token']}"
    return c


@pytest.fixture
async def anon_client():
    """Not logged in."""
    async with _app_client() as c:
        yield c


@pytest.fixture
async def client():
    """Logged in as the seeded Administrator (ADMIN/admin)."""
    async with _app_client() as c:
        yield await _log_in(c, "ADMIN", "admin")


@pytest.fixture
async def manager_client():
    """Logged in as the seeded Manager (GURU/guru)."""
    async with _app_client() as c:
        yield await _log_in(c, "GURU", "guru")


@pytest.fixture
async def cashier_client():
    """Logged in as a seeded Cashier (RAVI/ravi)."""
    async with _app_client() as c:
        yield await _log_in(c, "RAVI", "ravi")
