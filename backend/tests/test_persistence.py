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


async def test_backup_downloads_a_working_copy_of_the_state_file(client, state_file, tmp_path):
    import sqlite3
    sale = (await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()

    r = await client.get("/admin/backup")

    assert r.status_code == 200
    assert r.headers["content-disposition"].startswith('attachment; filename="vlqms_backup_')
    copy_path = tmp_path / "copy.sqlite3"
    copy_path.write_bytes(r.content)
    with sqlite3.connect(copy_path) as conn:
        assert sale["doc_no"] in conn.execute("SELECT data FROM state WHERE id = 1").fetchone()[0]
        conn.execute("SELECT COUNT(*) FROM web_outbox").fetchone()


async def test_backup_is_administrator_only(manager_client, cashier_client, state_file):
    assert (await manager_client.get("/admin/backup")).status_code == 403
    assert (await cashier_client.get("/admin/backup")).status_code == 403


async def test_backup_says_so_when_nothing_is_saved(client, state_file):
    r = await client.get("/admin/backup")

    assert r.status_code == 404
    assert "Nothing to back up" in r.json()["detail"]


async def test_backup_refused_when_saving_is_off(client, monkeypatch):
    monkeypatch.setattr(persistence, "PATH", "")

    r = await client.get("/admin/backup")

    assert r.status_code == 409
    assert "turned off" in r.json()["detail"]
