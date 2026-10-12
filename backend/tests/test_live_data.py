"""No sample data in the app: the store starts empty, every master comes from the shop's SQL Server (the loaders
are fed fake rows here), sign-in needs the database, and the first Administrator comes from the desktop roles or
from Database setup (POST /setup/admin)."""
import subprocess
import sys
from pathlib import Path

import pytest
from conftest import enc

from app import boot, db, db_store, sqlstore, store

BACKEND = Path(__file__).resolve().parents[1]


def test_a_fresh_store_has_no_data():
    code = ("import app.store as s; "
            "assert not (s.ITEMS or s.CUSTOMERS or s.SUPPLIERS or s.USERS or s.COUNTERS or s.LEDGERS or s.SALES_PERSONS), 'masters'; "
            "assert not s.MOVEMENTS and not any(s.DOCS.values()), 'documents'; "
            "assert not s.COMPANY['name'] and not s.COMPANY['state'], 'company'")
    run = subprocess.run([sys.executable, "-c", code], cwd=BACKEND, capture_output=True, text=True,
                         env={"PATH": "", "VLQMS_KEYS_IN_MEMORY": "1"})
    assert run.returncode == 0, run.stderr


async def test_sign_in_needs_the_database(anon_client, monkeypatch):
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(sqlstore, "enabled", lambda: False)
    monkeypatch.setattr(sqlstore, "writes_blocked", lambda: None)
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": enc("admin")})
    assert res.status_code == 503


def _rows(monkeypatch, by_table: dict):
    def fetchall(sql, *args):
        return next((rows for table, rows in by_table.items() if table in sql), [])
    monkeypatch.setattr(db, "fetchall", fetchall)


def test_ledgers_counters_and_sales_persons_come_from_sql(monkeypatch):
    _rows(monkeypatch, {
        "FROM MstLed l": [{"LEDID": 7, "LEDCODE": "EB", "LEDNAME": "Electricity", "LEDGROUPCODE": "IE", "LEDGROUPNAME": "Indirect Exp"}],
        "FROM MstLedgroup": [{"LEDGROUPID": 3, "LEDGROUPCODE": "IE", "LEDGROUPNAME": "Indirect Exp"}],
        "MstSC": [{"SCID": 4, "SCCODE": "C1", "SCNAME": "Front", "SYSNAME": "PC1", "PREFIX": "F", "InActive": "N"}],
        "MstSP": [{"SPID": 9, "SPCODE": "S9", "SPNAME": "Mani", "MOBNO": "", "InActive": "N"}],
    })
    db_store.load_ledger_groups()
    db_store.load_ledgers()
    db_store.load_counters()
    db_store.load_sales_persons()
    assert store.LEDGER_GROUPS == {"IE": {"code": "IE", "name": "Indirect Exp"}}
    assert store.LEDGERS == {"EB": {"code": "EB", "name": "Electricity", "group": "IE"}}
    assert store.COUNTERS["C1"] | {} == {"code": "C1", "_scid": "4", "name": "Front", "system_name": "PC1", "prefix": "F", "inactive": False}
    assert list(store.COUNTERS) == ["C1"] and list(store.SALES_PERSONS) == ["S9"]
    assert store.SALES_PERSONS["S9"]["name"] == "Mani"


def test_the_financial_year_sets_the_number_prefix(monkeypatch):
    _rows(monkeypatch, {"MstCompFinYear": [
        {"yrFdate": "2027-04-01 00:00:00", "yrTdate": "2028-03-31 00:00:00", "Active": "Y"},
        {"yrFdate": "2026-04-01 00:00:00", "yrTdate": "2027-03-31 00:00:00", "Active": "N"}]})
    db_store.load_fin_year()
    assert store.COMPANY["fy_start"] == "2027-04-01" and store.FY_PREFIX == "27"
    assert len(store.FIN_YEARS) == 2


def test_empty_tables_stay_empty(monkeypatch):
    _rows(monkeypatch, {})
    db_store.load_items()
    db_store.load_packages()
    assert store.ITEMS == {} and store.PACKAGES == {}


def test_desktop_roles_decide_the_web_role(monkeypatch):
    _rows(monkeypatch, {"MstUser": [
        {"UserID": 1, "UserCode": "boss", "LoginPwd": "", "UserName": "Boss", "Inactive": "N", "ROLECODE": "ADM", "ROLENAME": "ADMINISTRATOR"},
        {"UserID": 2, "UserCode": "OFFICE", "LoginPwd": "", "UserName": "", "Inactive": "N", "ROLECODE": "", "ROLENAME": ""},
        {"UserID": 3, "UserCode": "KUMAR", "LoginPwd": "", "UserName": "", "Inactive": "Y", "ROLECODE": "C", "ROLENAME": "COUNTER"}]})
    db_store.load_users()
    assert {u: r["role"] for u, r in store.USERS.items()} == {"BOSS": "Administrator", "OFFICE": "Manager", "KUMAR": "Cashier"}
    assert not store.USERS["KUMAR"]["active"]
    assert "GURU" not in store.USERS   # no demo users


@pytest.fixture
def no_admin(monkeypatch):
    """A live database (faked: nothing is written) whose users have no Administrator."""
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(sqlstore, "writes_blocked", lambda: None)
    monkeypatch.setattr(sqlstore, "save", lambda user=None: None)
    for u in store.USERS.values():
        if u["role"] == store.ADMIN_ROLE:
            u["role"] = "Manager"
    yield


async def test_setup_creates_the_first_administrator(anon_client, no_admin):
    assert (await anon_client.get("/setup/status")).json()["needs_admin"] is True
    body = {"user": "owner", "password": enc("Shop#2026", "new")}
    assert (await anon_client.post("/setup/admin", json={**body, "password": "Shop#2026"})).status_code in (400, 422)
    assert (await anon_client.post("/setup/admin", json={**body, "password": enc("short", "new")})).status_code == 422
    res = await anon_client.post("/setup/admin", json=body)
    assert res.status_code == 201, res.text
    assert store.USERS["OWNER"]["role"] == store.ADMIN_ROLE and store.USERS["OWNER"]["password_hash"]
    login = await anon_client.post("/auth/login", json={"user": "OWNER", "password": enc("Shop#2026")})
    assert login.status_code == 200 and login.json()["role"] == store.ADMIN_ROLE
    # Only while there is none
    again = await anon_client.post("/setup/admin", json={**body, "password": enc("Shop#2026", "new")})
    assert again.status_code == 409


async def test_setup_can_make_a_desktop_user_the_administrator(anon_client, no_admin):
    res = await anon_client.post("/setup/admin", json={"user": "GURU",
                                                         "password": enc("Owner#2026", "new")})
    assert res.status_code == 201
    assert store.USERS["GURU"]["role"] == store.ADMIN_ROLE
    assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("guru")})).status_code == 401


async def test_next_number_is_only_a_preview(client):
    first = (await client.get("/transactions/next-number", params={"kind": "sales_invoice", "counter": "counter10"})).json()
    second = (await client.get("/transactions/next-number", params={"kind": "sales_invoice", "counter": "counter10"})).json()
    assert first == second and first["doc_no"].startswith(f"{store.FY_PREFIX}/c10-")
    assert (await client.get("/transactions/next-number", params={"kind": "sales_invoice", "counter": "nope"})).status_code == 404
    assert (await client.get("/transactions/next-number", params={"kind": "purchase_bill"})).json()["prefix"]


async def test_sales_person_master(client):
    res = await client.post("/masters/sales-persons", json={"code": "S1", "name": "Mani"})
    assert res.status_code == 201, res.text
    assert "Mani" in (await client.get("/sales-persons")).json()
    await client.put("/masters/sales-persons/S1", json={"code": "S1", "name": "Mani", "active": False})
    assert "Mani" not in (await client.get("/sales-persons")).json()


async def test_company_is_served_blank_and_with_fin_years(anon_client):
    blank = {k: "" for k in store.COMPANY}
    saved = dict(store.COMPANY)
    store.COMPANY.update(blank)
    try:
        res = await anon_client.get("/masters/company")
        assert res.status_code == 200 and res.json()["name"] == "" and res.json()["fin_years"] == []
    finally:
        store.COMPANY.update(saved)


def test_role_names_match_whole_words():
    assert db_store._role_for("X", "", "NON ADMIN") == "Cashier"
    assert db_store._role_for("X", "ADMIN", "") == store.ADMIN_ROLE
    assert db_store._role_for("X", "", "Store Manager") == "Manager"


async def test_the_user_code_is_never_an_administrators_password(anon_client):
    store.USERS["BOSS"] = {"user": "BOSS", "password_hash": None, "default_password": True, "legacy_password": "",
                           "role": store.ADMIN_ROLE, "active": True, "pwv": 1}
    store.USERS["KUMAR"] = {**store.USERS["BOSS"], "user": "KUMAR", "role": "Cashier"}
    assert (await anon_client.post("/auth/login", json={"user": "BOSS", "password": enc("boss")})).status_code == 401
    assert (await anon_client.post("/auth/login", json={"user": "KUMAR", "password": enc("kumar")})).status_code == 200


def test_a_saved_row_keeps_the_desktop_role_unless_set_in_admin_master():
    store.USERS["KUMAR"] = {"user": "KUMAR", "role": "Manager", "active": True, "pwv": 1}
    store.USERS["MANI"] = {"user": "MANI", "role": "Cashier", "active": True, "pwv": 1}
    sqlstore.apply({"rows": {"USERS": {"KUMAR": {"user": "KUMAR", "role": "Cashier", "password_hash": "h"},
                                       "MANI": {"user": "MANI", "role": "Manager", "role_set_in_web": True}}}})
    assert store.USERS["KUMAR"]["role"] == "Manager" and store.USERS["KUMAR"]["password_hash"] == "h"
    assert store.USERS["MANI"]["role"] == "Manager"


async def test_a_walk_in_bill_needs_no_cash_customer_row(client):
    store.CUSTOMERS.pop("CASH", None)
    res = await client.post("/transactions/sales-invoice", json={
        "date": str(__import__("datetime").date.today()), "counter": "counter10", "salesperson": "GURU",
        "customer": "CASH", "payment_type": "CASH", "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    assert res.status_code == 201, res.text
    assert res.json()["customer"] == "CASH"


def test_how_many_recent_bills_are_kept_is_configurable(monkeypatch):
    """The Dashboard's 7-day chart reads the bills in memory: 200 left most days empty, so the default is 3000 and .env can change it."""
    from app import db_store
    monkeypatch.delenv("RECENT_INVOICES", raising=False)
    assert db_store._count("RECENT_INVOICES", 3000) == 3000
    monkeypatch.setenv("RECENT_INVOICES", "5000")
    assert db_store._count("RECENT_INVOICES", 3000) == 5000
    for bad in ("0", "-4", "many", "999999999"):
        monkeypatch.setenv("RECENT_INVOICES", bad)
        assert db_store._count("RECENT_INVOICES", 3000) == 3000
