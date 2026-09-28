"""The web app's own data survives a restart (app/persistence.py, saved after each successful write)."""
import copy
import os
import stat
from datetime import date

import pytest

from app import persistence, store

TODAY = date.today().isoformat()
TABLES = ("ITEMS", "USERS", "ROLES", "CUSTOMERS", "COUNTERS", "SETTINGS", "DENOMINATIONS", "COMPANY", "EDITS",
          "LAST_PURCHASE")


@pytest.fixture
def state_file(tmp_path, monkeypatch):
    path = tmp_path / "state.sqlite3"
    monkeypatch.setattr(persistence, "PATH", str(path))
    return path


def fresh_start(pristine: dict, seq: dict) -> None:
    """Put the store back to how a freshly started server has it (seed data only), then load the file."""
    for name, value in pristine.items():
        current = getattr(store, name)
        if isinstance(current, dict):
            current.clear()
            current.update(copy.deepcopy(value))
        elif isinstance(current, list):
            current[:] = copy.deepcopy(value)
    for kind, docs in store.DOCS.items():
        for no in [no for no, d in docs.items() if "origin" not in d]:
            del docs[no]
    for kind, value in seq.items():
        store._seq[kind].value = value
    assert persistence.load()


async def test_bills_stock_users_and_numbers_survive_a_restart(client, anon_client, state_file):
    pristine = {n: copy.deepcopy(getattr(store, n)) for n in TABLES + ("MOVEMENTS",)}
    seq = {k: s.value for k, s in store._seq.items()}
    stock_before = store.ITEMS["VIN-750"]["qoh"]

    sale = (await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": "VIN-750", "bottles": 3}]})).json()
    await client.post("/masters/items", json={"code": "NEW-1", "name": "NEW RUM 750", "group": "RUM", "package": "750ml",
                                              "mrp": 500, "purchase_rate": 400, "qoh": 12})
    await client.put("/admin/users/GURU", json={"user": "GURU", "password": "Manager#99", "role": "Manager"})
    await client.post("/admin/users", json={"user": "ANITHA", "password": "Counter#12", "role": "Cashier"})
    await client.delete("/masters/uoms/NOS")

    fresh_start(pristine, seq)

    assert store.DOCS["sales_invoice"][sale["doc_no"]]["totals"]["net"] == sale["totals"]["net"]
    assert store.ITEMS["VIN-750"]["qoh"] == stock_before - 3
    assert store.ITEMS["NEW-1"]["qoh"] == 12
    assert "NOS" not in store.UOMS
    assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": "Manager#99"})).status_code == 200
    assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": "guru"})).status_code == 401
    assert (await anon_client.post("/auth/login", json={"user": "ANITHA", "password": "Counter#12"})).status_code == 200
    # Numbering carries on instead of reusing the saved bill's number.
    nxt = (await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()
    assert nxt["doc_no"] > sale["doc_no"]


async def test_failed_requests_are_not_saved(client, state_file):
    await client.post("/transactions/sales-invoice", json={"date": TODAY, "counter": "counter10", "lines": []})

    assert not state_file.exists()


async def test_state_file_is_private_and_has_no_plain_passwords(client, state_file):
    await client.post("/admin/users", json={"user": "ANITHA", "password": "Counter#12", "role": "Cashier"})

    assert stat.S_IMODE(os.stat(state_file).st_mode) == 0o600
    raw = state_file.read_bytes()
    assert b"Counter#12" not in raw
    assert b"pbkdf2_sha256$" in raw


def test_loader_keeps_bills_made_in_the_app():
    from app.db_store import _drop_loaded
    docs = {"A": {"origin": "db"}, "B": {"origin": "seed"}, "C": {"doc_no": "C"}}

    _drop_loaded(docs)

    assert list(docs) == ["C"]
