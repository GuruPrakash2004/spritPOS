"""Admin Master (Security) beyond users and roles (app/security.py): legacy user fields, user-based rights,
Login Settings (password rules, expiry, remember me), Field Level Rights, Master Approvals and Customised
Report rights."""
from datetime import date, timedelta

from app import store
from conftest import _app_client, _log_in, enc

TODAY = date.today().isoformat()


def _user(**extra):
    return {"user": "ANITHA", "password": enc("Counter#12", "user"), "role": "Cashier", **extra}


async def test_user_fields_are_saved_and_checked(client):
    res = await client.post("/admin/users", json=_user(full_name="Anitha K", email="bad-email"))
    assert res.status_code == 422 and "EmailID" in res.json()["detail"]
    res = await client.post("/admin/users", json=_user(full_name="Anitha K", email="anitha@example.com", mobile="98450",
                                                       department="Billing", dob="1995-04-01"))
    assert res.status_code == 201
    u = res.json()
    assert u["full_name"] == "Anitha K" and u["email"] == "anitha@example.com" and u["dob"] == "1995-04-01"
    res = await client.put("/admin/users/ANITHA", json={"user": "ANITHA", "role": "Cashier", "active": False})
    assert res.status_code == 422 and "InActive" in res.json()["detail"]
    res = await client.put("/admin/users/ANITHA", json={"user": "ANITHA", "role": "Cashier", "active": False, "inactive_reason": "Left"})
    assert res.status_code == 200 and res.json()["inactive_reason"] == "Left"


async def test_rights_are_role_based_only(client):
    # User rights was the same grid as Roles & rights, so it was removed: a user gets their role's screens.
    assert (await client.get("/admin/users/RAVI/rights")).status_code in (404, 405)
    me = next(u for u in (await client.get("/admin/users")).json() if u["user"] == "RAVI")
    assert "own_rights" not in me


async def test_login_settings_rules_apply_to_new_passwords(client, anon_client):
    res = await client.put("/admin/login-settings", json={"min_length": 10, "require_upper": True, "require_special": True})
    assert res.status_code == 200
    public = (await anon_client.get("/auth/login-settings")).json()
    assert public["min_length"] == 10 and public["require_upper"] and "adnl_admin_hash" not in str(public)
    res = await client.post("/admin/users", json=_user(password=enc("counter#1234", "user")))
    assert res.status_code == 422 and "capital" in res.json()["detail"]
    res = await client.post("/admin/users", json=_user(password=enc("Counter#1234", "user")))
    assert res.status_code == 201


async def test_expired_password_must_be_changed_first(client):
    yesterday = (date.today() - timedelta(days=1)).isoformat()
    await client.put("/admin/users/RAVI", json={"user": "RAVI", "role": "Cashier", "password_reset_on": TODAY})
    store.USERS["RAVI"]["password_set_on"] = yesterday
    async with _app_client() as c:
        login = await c.post("/auth/login", json={"user": "RAVI", "password": enc("ravi")})
        assert login.json()["must_change_password"] is True
        c.headers["Authorization"] = f"Bearer {login.json()['access_token']}"
        res = await c.get("/masters/items")
        assert res.status_code == 403 and "expired" in res.json()["detail"]
        res = await c.post("/auth/change-password", json={"current_password": enc("ravi", "current"), "new_password": enc("Counter#77", "new")})
        assert res.status_code == 200
        c.headers["Authorization"] = f"Bearer {res.json()['access_token']}"
        assert (await c.get("/masters/items")).status_code == 200


async def test_field_rights_hide_and_lock_fields(client, cashier_client):
    res = await client.put("/admin/field-rights/Cashier", json={"masters": {"items": {"mrp": "readonly", "hsn_code": "hidden"}}})
    assert res.status_code == 200
    me = (await cashier_client.get("/auth/me")).json()
    assert me["field_rights"]["items"] == {"mrp": "readonly", "hsn_code": "hidden"}
    item = (await cashier_client.get("/masters/items/VIN-750")).json()
    assert item["hsn_code"] == "" and store.ITEMS["VIN-750"]["hsn_code"]      # hidden: the reply shows the default
    changed = dict(item, mrp=item["mrp"] + 5)
    res = await cashier_client.put("/masters/items/VIN-750", json=changed)
    assert res.status_code == 403 and "mrp" in res.json()["detail"]
    hsn = store.ITEMS["VIN-750"]["hsn_code"]
    res = await cashier_client.put("/masters/items/VIN-750", json=dict(item, name="Renamed"))
    assert res.status_code == 200 and store.ITEMS["VIN-750"]["hsn_code"] == hsn     # hidden field kept
    bad = await client.put("/admin/field-rights/Cashier", json={"masters": {"items": {"nope": "hidden"}}})
    assert bad.status_code == 422
    bad = await client.put("/admin/field-rights/Cashier", json={"masters": {"items": {"name": "hidden"}}})
    assert bad.status_code == 422 and "read-only" in bad.json()["detail"]


async def test_master_approval_blocks_unapproved_rows_until_approved(client, cashier_client):
    await client.put("/admin/master-approval", json={"masters": ["items", "customers"]})
    new = dict(store.ITEMS["VIN-750"], code="NEW-1", name="New item", qoh=5, purchase_rate=None)
    assert (await cashier_client.post("/masters/items", json=new)).status_code == 201
    assert store.ITEMS["NEW-1"]["authorised"] is False
    bill = {"date": TODAY, "counter": "counter10", "customer": "CASH", "salesperson": "GURU", "lines": [{"item_code": "NEW-1", "bottles": 1}]}
    res = await client.post("/transactions/sales-invoice", json=bill)
    assert res.status_code == 409 and "waiting for approval" in res.json()["detail"]
    pending = (await client.get("/admin/master-approval")).json()["pending"]
    assert [p["code"] for p in pending] == ["NEW-1"]
    assert (await client.post("/admin/master-approval/items/NEW-1")).status_code == 200
    assert (await client.post("/transactions/sales-invoice", json=bill)).status_code == 201
    assert (await cashier_client.get("/masters/items/NEW-1")).json()["authorised"] is True


async def test_report_rights_limit_who_runs_a_report(client, cashier_client):
    name = "Sales - Bill Wise Report"
    store.ROLES["Cashier"] = [*store.ROLES["Cashier"], "Reports Dashboard"]
    params = {"name": name, "from_date": TODAY, "to_date": TODAY}
    assert (await cashier_client.get("/reports/custom", params=params)).status_code == 200
    assert (await client.put(f"/admin/report-rights/{name}", json={"users": ["GURU"]})).status_code == 200
    res = await cashier_client.get("/reports/custom", params=params)
    assert res.status_code == 403
    allowed = (await cashier_client.get("/reports/custom/allowed")).json()
    assert name in allowed["known"] and name not in allowed["allowed"]
    assert (await client.get("/reports/custom", params=params)).status_code == 200


async def test_security_admin_endpoints_are_admin_only(cashier_client):
    for path in ("/admin/login-settings", "/admin/field-rights", "/admin/master-approval", "/admin/report-rights"):
        assert (await cashier_client.get(path)).status_code == 403, path


# ─── Review fixes ────────────────────────────────────────────────────────────

async def test_field_rights_follow_a_renamed_role_and_go_with_a_deleted_one(client):
    await client.put("/admin/field-rights/Cashier", json={"masters": {"items": {"hsn_code": "readonly"}}})
    role = next(r for r in (await client.get("/admin/roles")).json() if r["name"] == "Cashier")
    await client.put("/admin/roles/Cashier", json={"name": "Counter Staff", "modules": role["modules"], "rights": {}})
    assert "Cashier" not in store.FIELD_RIGHTS and store.FIELD_RIGHTS["Counter Staff"]["items"] == {"hsn_code": "readonly"}


async def test_hidden_fields_get_the_default_on_create_and_billing_fields_cannot_be_hidden(client, manager_client):
    res = await client.put("/admin/field-rights/Manager", json={"masters": {"items": {"mrp": "hidden"}}})
    assert res.status_code == 422 and "read-only" in res.json()["detail"]
    await client.put("/admin/field-rights/Manager", json={"masters": {"items": {"hsn_code": "hidden"}}})
    new = dict(store.ITEMS["VIN-750"], code="NEW-2", name="New", hsn_code="9999", qoh=0, purchase_rate=None)
    assert (await manager_client.post("/masters/items", json=new)).status_code == 201
    assert store.ITEMS["NEW-2"]["hsn_code"] == ""


async def test_price_list_follows_field_rights_and_approvals(client, manager_client):
    await client.put("/admin/field-rights/Manager", json={"masters": {"items": {"mrp": "readonly"}}})
    res = await manager_client.post("/masters/items/price-list", json={"item_codes": ["VIN-750"], "column": "mrp", "change_by": 10})
    assert res.status_code == 403
    await client.put("/admin/field-rights/Manager", json={"masters": {}})
    await client.put("/admin/master-approval", json={"masters": ["items"]})
    res = await manager_client.post("/masters/items/price-list", json={"item_codes": ["VIN-750"], "column": "mrp", "change_by": 10})
    assert res.status_code == 200 and store.ITEMS["VIN-750"]["authorised"] is False


async def test_admin_password_guesses_are_locked_out(client):
    for _ in range(5):
        res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": ["X"], "admin_password": enc("wrong", "admin")})
        assert res.status_code == 403
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": ["X"], "admin_password": enc("admin", "admin")})
    assert res.status_code == 429


async def test_seed_users_can_expire(client):
    await client.put("/admin/login-settings", json={"password_expiry_days": 1})
    store.USERS["GURU"]["password_set_on"] = (date.today() - timedelta(days=2)).isoformat()
    async with _app_client() as c:
        res = await c.post("/auth/login", json={"user": "GURU", "password": enc("guru")})
        assert res.json()["must_change_password"] is True
