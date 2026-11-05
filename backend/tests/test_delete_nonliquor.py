"""Delete Non-Liquor Items (legacy frmUtlDeleteNonLiqItems): refusals, and the statements it runs, on a fake database."""
import pytest

from app import db, delete_nonliquor, sqlstore, store
from conftest import enc


class FakeCursor:
    def __init__(self):
        self.sql = []

    def execute(self, sql, params=()):
        self.sql.append((sql, params))
        return 1


@pytest.fixture
def fake_db(monkeypatch):
    cur = FakeCursor()
    reloads = []

    class Tx:
        def __enter__(self):
            return cur

        def __exit__(self, *a):
            return False

    monkeypatch.setattr(sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(sqlstore, "reload", lambda: reloads.append(1))
    monkeypatch.setattr(sqlstore, "save", lambda *a, **k: None)          # the middleware saves after each write; there is no SQL Server here
    monkeypatch.setattr(sqlstore, "add_audit", lambda *a, **k: None)
    monkeypatch.setattr(db, "transaction", lambda: Tx())
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [{"ItemID": 101}, {"ItemID": 102}])
    monkeypatch.setattr(db, "fetchone", lambda sql, params=(): {"b": 3, "l": 5, "n": 7, "first": None})
    store.ITEM_GROUPS["ZZNL"] = {"code": "ZZNL", "name": "ZZNL", "liquor": False}
    for n, t in ((1, "OTHERS"), (2, "")):
        store.ITEMS[f"ZZ{n}"] = {"code": f"ZZ{n}", "name": f"Snack {n}", "group": "ZZNL", "liqtype": t}
    yield cur, reloads


async def test_preview_lists_items_and_counts(client, fake_db):
    res = await client.get("/utilities/delete-non-liquor/preview", params={"group": "ZZNL"})
    assert res.status_code == 200
    body = res.json()
    assert body["can_delete"] and len(body["items"]) == 2 and body["counts"]["sales_bills"] == 3


async def test_liquor_in_the_group_blocks_the_delete(client, fake_db):
    store.ITEMS["ZZ2"]["liqtype"] = "IML"
    body = (await client.get("/utilities/delete-non-liquor/preview", params={"group": "ZZNL"})).json()
    assert not body["can_delete"] and "liquor" in body["blockers"][0]
    res = await client.post("/utilities/delete-non-liquor", json={"group": "ZZNL", "confirm": "ZZNL", "admin_password": enc("admin", "admin")})
    assert res.status_code == 409


async def test_needs_the_confirmation_and_the_admin_password(client, fake_db):
    cur, reloads = fake_db
    assert (await client.post("/utilities/delete-non-liquor", json={"group": "ZZNL", "confirm": "nope", "admin_password": enc("admin", "admin")})).status_code == 422
    assert (await client.post("/utilities/delete-non-liquor", json={"group": "ZZNL", "confirm": "ZZNL"})).status_code == 403
    assert cur.sql == [] and reloads == []


async def test_documents_made_in_the_web_app_block_the_delete(client, fake_db):
    store.DOCS["sales_invoice"]["T/1"] = {"doc_no": "T/1", "lines": [{"item_code": "ZZ1"}], "status": "SAVED", "date": "2026-01-01"}
    try:
        body = (await client.get("/utilities/delete-non-liquor/preview", params={"group": "ZZNL"})).json()
        assert not body["can_delete"] and "web app" in body["blockers"][0]
    finally:
        store.DOCS["sales_invoice"].pop("T/1", None)


async def test_delete_runs_its_statements_for_this_groups_items_and_reloads(client, fake_db):
    cur, reloads = fake_db
    res = await client.post("/utilities/delete-non-liquor", json={"group": "ZZNL", "confirm": "ZZNL", "admin_password": enc("admin", "admin")})
    assert res.status_code == 200, res.text
    assert res.json()["deleted_items"] == 2 and reloads == [1]
    text = " | ".join(s for s, _ in cur.sql)
    for part in ("DELETE FROM TrnDtlSINV WHERE ItemID IN", "DELETE FROM TrnDtlPB WHERE ItemID IN", "DELETE FROM UtlStockList", "DELETE FROM TrnQOH",
                 "DELETE FROM MstItem WHERE ItemID IN", "DELETE FROM MstItemGroup WHERE ItemGroupCode = ?"):
        assert part in text
    assert all(p == (101, 102) for s, p in cur.sql if "ItemID IN" in s)         # only this group's items, never "not in MstItem"
    assert text.index("UPDATE h SET") < text.index("DELETE FROM MstItem WHERE")  # totals are worked out before the items go
