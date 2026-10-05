"""The central site (app/central) on its in-memory store: shops push with their sync keys, owners sign in
with encrypted passwords and see only their shops, and a real Web Sync payload is accepted as built."""
import re
from datetime import date
from pathlib import Path

import httpx
import pytest

from app import passwords, store, websync
from app.central import auth as cauth, config, main as central, repo as crepo, tables as T

from conftest import enc

SCHEMA = Path(__file__).resolve().parents[1] / "migrations" / "central" / "001_central.sql"
DAY = "2026-10-01"


@pytest.fixture
def repo(monkeypatch):
    r = crepo.MemoryRepo()
    monkeypatch.setattr(config, "REPO", r)
    cauth.clear_failures()
    yield r
    cauth.clear_failures()


@pytest.fixture
def shops(repo):
    """Two shops with their keys, an owner who sees only SHOP1, and the central administrator."""
    keys = {}
    for code in ("SHOP1", "SHOP2"):
        keys[code] = cauth.new_sync_key()
        repo.add_client(code, f"{code} Wines", cauth.key_hash(keys[code]))
    for name, role in (("OWNER", cauth.OWNER), ("BOSS", cauth.ADMIN)):
        repo.add_user(name, passwords.hash_password("Start123"), role)
        repo.set_password(name, passwords.hash_password("Start123"), must_change=False)
    repo.grant("OWNER", repo.clients()[0]["id"])
    return keys


def _client() -> httpx.AsyncClient:
    return httpx.AsyncClient(transport=httpx.ASGITransport(app=central.app), base_url="http://central")


async def _signed_in(c: httpx.AsyncClient, user: str, password: str = "Start123") -> httpx.AsyncClient:
    res = await c.post("/auth/login", json={"user": user, "password": enc(password)})
    assert res.status_code == 200, res.text
    c.headers["Authorization"] = f"Bearer {res.json()['access_token']}"
    return c


def _day(sales: float, bills: int = 1, group_amt: float = 100.0) -> dict:
    return {"day": DAY, "tables": {
        "RptDSRSmry": [{"TotBills": bills, "TotSalesAmt": sales, "LastPurBillNo": "PB-1", "LastPurDate": DAY}],
        "RptDSRGroupWiseSales": [{"slno": 1, "GroupName": "BEER", "TotQty": 2, "TotAmt": group_amt}],
        "RptFastMov": [{"ItemName": "KINGFISHER", "SoldQty": 2}],
    }}


def test_schema_file_matches_the_table_list():
    sql = SCHEMA.read_text()
    for name, columns in {**T.DAY_TABLES, T.STOCK_TABLE: T.STOCK_COLUMNS}.items():
        block = re.search(rf"CREATE TABLE dbo\.{name} \((.*?)\n\);", sql, re.S)
        assert block, name
        found = re.findall(r"\[(\w+)\]", block.group(1))
        expected = (["ClientID", "DocDate", *columns] if name in T.DAY_TABLES else ["ClientID", *columns, "SyncedAt"])
        assert found == expected, name
    assert "INSERT" not in sql.upper().replace("INSERT_", "") and "CREATE LOGIN" not in sql.upper()


async def test_a_push_needs_a_valid_key(shops):
    async with _client() as c:
        assert (await c.post("/sync", json=_day(500))).status_code == 401
        assert (await c.post("/sync", json=_day(500), headers={"Authorization": "Bearer nope"})).status_code == 401
        config.REPO.set_active("SHOP2", False)
        res = await c.post("/sync", json=_day(500), headers={"Authorization": f"Bearer {shops['SHOP2']}"})
        assert res.status_code == 401


async def test_wrong_keys_are_rate_limited(shops):
    async with _client() as c:
        for _ in range(cauth.MAX_FAILS_PER_ADDRESS):
            await c.post("/sync", json=_day(1), headers={"Authorization": "Bearer wrong"})
        res = await c.post("/sync", json=_day(1), headers={"Authorization": f"Bearer {shops['SHOP1']}"})
        assert res.status_code == 429


async def test_a_push_replaces_only_that_shops_day(shops):
    async with _client() as c:
        h1 = {"Authorization": f"Bearer {shops['SHOP1']}"}
        h2 = {"Authorization": f"Bearer {shops['SHOP2']}"}
        assert (await c.post("/sync", json=_day(500), headers=h1)).status_code == 200
        assert (await c.post("/sync", json=_day(300), headers=h2)).status_code == 200
        assert (await c.post("/sync", json=_day(700, bills=3), headers=h1)).status_code == 200   # the same day again
    rows = config.REPO.day_rows([c["id"] for c in config.REPO.clients()], DAY)
    smry = sorted((r["ClientID"], r["TotSalesAmt"], r["TotBills"]) for r in rows["RptDSRSmry"])
    assert smry == [(1, 700, 3), (2, 300, 1)]
    assert len(rows["RptDSRGroupWiseSales"]) == 2
    assert config.REPO.clients()[0]["last_day"] == DAY


@pytest.mark.parametrize("bad", [
    {"day": DAY, "tables": {"RptNope": []}},
    {"day": DAY, "tables": {"RptDSRSmry": [{"TotBills": 1, "Hacked": 1}]}},
    {"day": DAY, "tables": {"RptDSRSmry": [{"TotBills": "lots"}]}},
    {"day": DAY, "tables": {"RptFastMov": [{"ItemName": "x" * 251, "SoldQty": 1}]}},
    {"day": "2999-01-01", "tables": {}},
])
async def test_a_bad_push_is_refused(shops, bad):
    async with _client() as c:
        res = await c.post("/sync", json=bad, headers={"Authorization": f"Bearer {shops['SHOP1']}"})
        assert res.status_code == 422


async def test_an_oversized_push_is_refused(shops):
    async with _client() as c:
        res = await c.post("/sync", content=b"{}", headers={"Authorization": f"Bearer {shops['SHOP1']}",
                                                              "Content-Type": "application/json",
                                                              "Content-Length": str(central.MAX_BODY_BYTES + 1)})
        assert res.status_code == 413


async def test_login_needs_an_encrypted_password_and_a_new_user_must_change_it(repo):
    repo.add_user("NEWBIE", passwords.hash_password("Temp1234"), cauth.OWNER)
    async with _client() as c:
        assert (await c.post("/auth/login", json={"user": "NEWBIE", "password": "Temp1234"})).status_code in (400, 422)
        assert (await c.post("/auth/login", json={"user": "NEWBIE", "password": enc("wrong")})).status_code == 401
        res = await c.post("/auth/login", json={"user": "newbie", "password": enc("Temp1234")})
        assert res.status_code == 200 and res.json()["must_change"] is True
        c.headers["Authorization"] = f"Bearer {res.json()['access_token']}"
        assert (await c.get("/clients")).status_code == 403
        bad = await c.post("/auth/change-password", json={"current_password": enc("Temp1234", "current"),
                                                          "new_password": enc("short", "new")})
        assert bad.status_code == 422
        res = await c.post("/auth/change-password", json={"current_password": enc("Temp1234", "current"),
                                                          "new_password": enc("Better#2026", "new")})
        assert res.status_code == 200
        assert (await c.get("/clients")).status_code == 401   # the old token is signed out
        c.headers["Authorization"] = f"Bearer {res.json()['access_token']}"
        assert (await c.get("/clients")).status_code == 200


async def test_five_wrong_passwords_lock_the_user(shops):
    async with _client() as c:
        for _ in range(cauth.MAX_FAILS_PER_USER):
            await c.post("/auth/login", json={"user": "OWNER", "password": enc("nope")})
        assert (await c.post("/auth/login", json={"user": "OWNER", "password": enc("Start123")})).status_code == 429


async def test_an_owner_sees_only_their_shops(shops):
    async with _client() as push:
        await push.post("/sync", json=_day(500), headers={"Authorization": f"Bearer {shops['SHOP1']}"})
        await push.post("/sync", json=_day(300), headers={"Authorization": f"Bearer {shops['SHOP2']}"})
    async with _client() as c:
        await _signed_in(c, "OWNER")
        assert [s["code"] for s in (await c.get("/clients")).json()] == ["SHOP1"]
        assert (await c.get(f"/dsr?day={DAY}&client=SHOP2")).status_code == 404
        all_shops = (await c.get(f"/dsr?day={DAY}")).json()
        assert all_shops["summary"]["TotSalesAmt"] == 500 and [s["client"] for s in all_shops["shops"]] == ["SHOP1"]


async def test_all_shops_add_up(shops):
    async with _client() as push:
        await push.post("/sync", json=_day(500, group_amt=100), headers={"Authorization": f"Bearer {shops['SHOP1']}"})
        await push.post("/sync", json=_day(300, bills=2, group_amt=40), headers={"Authorization": f"Bearer {shops['SHOP2']}"})
    async with _client() as c:
        await _signed_in(c, "BOSS")
        body = (await c.get(f"/dsr?day={DAY}")).json()
        assert body["summary"]["TotSalesAmt"] == 800 and body["summary"]["TotBills"] == 3
        assert body["tables"]["RptDSRGroupWiseSales"] == [{"GroupName": "BEER", "TotQty": 4, "TotAmt": 140}]
        assert body["tables"]["RptFastMov"] == [{"ItemName": "KINGFISHER", "SoldQty": 4}]
        one = (await c.get(f"/dsr?day={DAY}&client=shop2")).json()
        assert one["summary"]["TotSalesAmt"] == 300 and one["summary"]["LastPurBillNo"] == "PB-1"


async def test_stock_across_shops(shops):
    item = {"ItemCode": "KF-650", "ItemName": "KINGFISHER", "ItemGroupName": "BEER", "Brand": "Kingfisher",
            "Package": "650ml", "QOH": 10, "MinLevel": 12, "MRP": 180, "Active": True}
    async with _client() as push:
        await push.post("/sync/stock", json={"items": [item]}, headers={"Authorization": f"Bearer {shops['SHOP1']}"})
        await push.post("/sync/stock", json={"items": [{**item, "QOH": 30}]}, headers={"Authorization": f"Bearer {shops['SHOP2']}"})
        dup = await push.post("/sync/stock", json={"items": [item, item]}, headers={"Authorization": f"Bearer {shops['SHOP2']}"})
        assert dup.status_code == 422
    async with _client() as c:
        await _signed_in(c, "BOSS")
        assert (await c.get("/stock?q=k")).status_code == 422
        res = (await c.get("/stock?q=kingf")).json()
        kf = res["items"][0]
        assert kf["total_qoh"] == 40 and {s["client"]: s["short"] for s in kf["shops"]} == {"SHOP1": True, "SHOP2": False}


async def test_a_real_web_sync_payload_is_accepted(shops, client):
    """What a shop's websync.build() sends is what the central site accepts, with nothing dropped."""
    day = max(date.fromisoformat(i["date"]) for i in store.DOCS["sales_invoice"].values() if i["status"] == "SAVED")
    await client.post("/transactions/purchase-indent", json={"date": day.isoformat(), "supplier": "USL",
                                                             "lines": [{"item_code": "RC-750", "cases": 1}]})
    payload, stock = websync.build(day), websync.build_stock()
    async with _client() as c:
        h = {"Authorization": f"Bearer {shops['SHOP1']}"}
        res = await c.post("/sync", json=payload, headers=h)
        assert res.status_code == 200, res.text
        sent = {name: len(rows) for name, rows in payload["tables"].items() if rows}
        assert res.json()["rows"] == sent and sent["TrnHdrPI"] >= 1
        assert (await c.post("/sync/stock", json=stock, headers=h)).json()["items"] == len(store.ITEMS)
        await _signed_in(c, "OWNER")
        body = (await c.get(f"/dsr?day={day}&client=SHOP1")).json()
        assert body["summary"]["TotSalesAmt"] == payload["tables"]["RptDSRSmry"][0]["TotSalesAmt"]
        indents = (await c.get(f"/indents?day={day}&client=SHOP1")).json()
        assert indents and indents[0]["lines"][0]["ItemCode"] == "RC-750"


class _DownDb:
    """A central database that can't be reached."""
    DB_NAME = "x"

    def is_db_available(self):
        return False

    def reconnect(self):
        return False


async def test_an_unreachable_central_database_answers_503_and_never_keeps_pushes_in_memory(monkeypatch):
    monkeypatch.setattr(config, "REPO", crepo.SqlRepo(_DownDb()))
    cauth.clear_failures()
    async with _client() as c:
        res = await c.post("/sync", json=_day(1), headers={"Authorization": "Bearer anything"})
        assert res.status_code == 503   # the shop keeps the day and sends it again
        assert (await c.post("/auth/login", json={"user": "X", "password": enc("whatever1")})).status_code == 503


def test_times_carry_their_offset(repo):
    repo.add_client("SHOPX", "X", "h" * 64)
    repo.replace_day(1, DAY, {})
    assert repo.clients()[0]["last_sync"].endswith("+00:00")
    assert crepo._utc(__import__("datetime").datetime(2026, 10, 1, 5, 30)) == "2026-10-01T05:30:00+00:00"


async def test_blank_names_and_case_duplicates(shops):
    h = {"Authorization": f"Bearer {shops['SHOP1']}"}
    item = {"ItemCode": "kf-650", "ItemName": None, "QOH": 1}
    async with _client() as c:
        dup = await c.post("/sync/stock", json={"items": [item, {**item, "ItemCode": "KF-650 "}]}, headers=h)
        assert dup.status_code == 422
        assert (await c.post("/sync/stock", json={"items": [item]}, headers=h)).status_code == 200
        assert (await c.post("/sync", json={"day": DAY, "tables": {"TrnHdrPI": [{"PIID": 1, "PINO": None}],
                                                                   "RptFastMov": [{"ItemName": None, "SoldQty": 1}]}},
                             headers=h)).status_code == 200
        big = await c.post("/sync", json={"day": DAY, "tables": {"RptDSRSmry": [{"TotBills": 2**40}]}}, headers=h)
        assert big.status_code == 422
        await _signed_in(c, "BOSS")
        assert (await c.get("/stock?client=SHOP1")).status_code == 200
        assert (await c.get(f"/indents?day={DAY}")).status_code == 200
        assert (await c.get(f"/dsr?day={DAY}")).status_code == 200
