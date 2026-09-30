"""Security: password hashing, brute-force lockout, token invalidation, role rights, headers."""
import base64
from datetime import date

import pytest

from app import store
from app.passwords import hash_password, password_problem, verify_password
from app.routers import auth
from conftest import enc


# ---------- Password hashing ----------
def test_passwords_are_stored_only_as_hashes():
    for u in store.USERS.values():
        assert "password" not in u
        assert u["password_hash"].startswith("pbkdf2_sha256$")


def test_hash_is_salted_and_verifies():
    a, b = hash_password("Shop@2026"), hash_password("Shop@2026")

    assert a != b                      # a new random salt each time
    assert verify_password("Shop@2026", a)
    assert not verify_password("shop@2026", a)
    assert not verify_password("anything", "not-a-hash")
    assert not verify_password("anything", None)


@pytest.mark.parametrize(("password", "problem"), [
    ("short1", "at least 8"),
    ("onlyletters", "mix letters"),
    ("12345678", "mix letters"),
    ("guru2026", None),
])
def test_new_password_rules(password, problem):
    result = password_problem(password, "GURU")
    assert (result is None) if problem is None else (problem in result)


def test_password_same_as_user_name_is_refused():
    assert "same as the user name" in password_problem("cashier01", "CASHIER01")


async def test_new_user_password_is_hashed_and_works(client, anon_client):
    res = await client.post("/admin/users", json={"user": "ANITHA", "password": enc("Counter#12", "user"), "role": "Cashier"})
    assert res.status_code == 201
    assert "password" not in res.json() and "password_hash" not in res.json()
    assert store.USERS["ANITHA"]["password_hash"].startswith("pbkdf2_sha256$")

    login = await anon_client.post("/auth/login", json={"user": "anitha", "password": enc("Counter#12")})
    assert login.status_code == 200


async def test_weak_password_is_refused_for_a_new_user(client):
    res = await client.post("/admin/users", json={"user": "ANITHA", "password": enc("abcd", "user"), "role": "Cashier"})

    assert res.status_code == 422
    assert "ANITHA" not in store.USERS


async def test_db_user_legacy_password_is_upgraded_to_a_hash(anon_client):
    # A user loaded from MstUser: legacy base64 password, no hash yet.
    store.USERS["OFFICE"] = {"user": "OFFICE", "password_hash": None, "default_password": True,
                             "legacy_password": base64.b64encode(b"Legacy#1").decode(), "role": "Manager",
                             "active": True, "pwv": 1}

    res = await anon_client.post("/auth/login", json={"user": "OFFICE", "password": enc("Legacy#1")})

    assert res.status_code == 200
    u = store.USERS["OFFICE"]
    assert u["password_hash"].startswith("pbkdf2_sha256$")
    assert "legacy_password" not in u and "default_password" not in u
    # The default (user code) password stops working once the real one is hashed.
    assert (await anon_client.post("/auth/login", json={"user": "OFFICE", "password": enc("office")})).status_code == 401


# ---------- Brute force ----------
async def test_five_wrong_passwords_lock_the_user_out(anon_client):
    for _ in range(auth.MAX_FAILS_PER_USER):
        assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("wrong")})).status_code == 401

    locked = await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("guru")})

    assert locked.status_code == 429
    assert "Too many failed logins" in locked.json()["detail"]


async def test_a_good_login_clears_earlier_failures(anon_client):
    for _ in range(auth.MAX_FAILS_PER_USER - 1):
        await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("wrong")})
    assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("guru")})).status_code == 200

    assert (await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("wrong")})).status_code == 401


# ---------- Tokens ----------
async def test_password_change_signs_the_user_out(client, anon_client):
    login = await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("guru")})
    old_token = {"Authorization": f"Bearer {login.json()['access_token']}"}
    assert (await anon_client.get("/auth/me", headers=old_token)).status_code == 200

    await client.put("/admin/users/GURU", json={"user": "GURU", "password": enc("Manager#99", "user"), "role": "Manager"})

    res = await anon_client.get("/auth/me", headers=old_token)
    assert res.status_code == 401
    assert "log in again" in res.json()["detail"]


async def test_deactivated_user_is_signed_out(client, cashier_client):
    await client.put("/admin/users/RAVI", json={"user": "RAVI", "role": "Cashier", "active": False, "inactive_reason": "Left the shop"})

    assert (await cashier_client.get("/auth/me")).status_code == 401


async def test_forged_token_is_rejected(anon_client):
    forged = auth.jwt.encode({"sub": "ADMIN", "pwv": 1}, "not-the-secret", algorithm="HS256")

    res = await anon_client.get("/admin/users", headers={"Authorization": f"Bearer {forged}"})

    assert res.status_code == 401


# ---------- Role rights ----------
BILL = {"date": "2026-01-10", "supplier": "KSBCL", "supplier_inv_no": "INV-1", "supplier_inv_date": "2026-01-10",
        "lines": [{"item_code": "VIN-750", "bottles": 12}]}


@pytest.mark.parametrize(("method", "path", "body"), [
    ("POST", "/transactions/purchase-bill", BILL),
    ("POST", "/transactions/stock-deduction", {"date": "2026-01-10", "lines": [{"item_code": "VIN-750", "bottles": 1}]}),
    ("POST", "/transactions/expense", {"date": "2026-01-10", "lines": [{"ledger": "RENT", "amount": 10}]}),
    ("POST", "/masters/items/price-list", {"item_codes": ["VIN-750"], "change_by": -800}),
    ("POST", "/masters/customers", {"code": "C9", "name": "Test"}),
    ("PUT", "/masters/sales-counters/counter10", {"code": "counter10", "name": "x", "prefix": "c10"}),
    ("GET", "/reports/profit-margin?from_date=2026-01-01&to_date=2026-01-31", None),
    ("GET", "/reports/custom?name=Sales%20Bill%20Wise&from_date=2026-01-01&to_date=2026-01-31", None),
    ("PUT", "/settings", {"loyalty": {"points_per_rupee": 1, "alert_when_points_above": 0, "min_bill_amount": 0}}),
])
async def test_cashier_is_refused_outside_their_role(cashier_client, method, path, body):
    res = await cashier_client.request(method, path, json=body)

    assert res.status_code == 403


async def test_cashier_can_still_bill(cashier_client):
    res = await cashier_client.post("/transactions/sales-invoice", json={
        "date": "2026-01-10", "counter": "counter10", "salesperson": "RAVI",
        "lines": [{"item_code": "VIN-750", "bottles": 1}]})

    assert res.status_code == 201


async def test_manager_can_post_a_purchase_bill(manager_client):
    assert (await manager_client.post("/transactions/purchase-bill", json=BILL)).status_code == 201


# ---------- Roles can't be used to take over Administrator ----------
async def test_role_cannot_be_renamed_to_administrator(client):
    res = await client.put("/admin/roles/Cashier", json={"name": "Administrator", "modules": ["Dashboard"]})

    assert res.status_code == 409
    assert store.ROLES["Administrator"] == ["*"]


async def test_role_cannot_overwrite_another_role(client):
    res = await client.put("/admin/roles/Cashier", json={"name": "Manager", "modules": ["Dashboard"]})

    assert res.status_code == 409
    assert "Item Master" in store.ROLES["Manager"]


async def test_role_cannot_get_every_module(client):
    res = await client.put("/admin/roles/Cashier", json={"name": "Cashier", "modules": ["*"]})

    assert res.status_code == 422


# ---------- Settings, health, headers ----------
async def test_settings_are_validated(client):
    bad = await client.put("/settings", json={"loyalty": {"points_per_rupee": -1, "alert_when_points_above": 0, "min_bill_amount": 0}})
    unknown = await client.put("/settings", json={"database": {"backup": "/"}})
    good = await client.put("/settings", json={"invoice": {"salesperson_required": False, "print_preview": True,
                                                           "barcode_scan_jumps_to_qty": True}})

    assert bad.status_code == 422
    assert unknown.status_code == 422
    assert good.status_code == 200
    assert good.json()["invoice"]["salesperson_required"] is False


async def test_health_does_not_reveal_the_database_address(anon_client):
    db = (await anon_client.get("/")).json()["db"]

    assert set(db) == {"connected"}


async def test_security_headers_are_sent(anon_client):
    res = await anon_client.get("/")

    assert res.headers["X-Content-Type-Options"] == "nosniff"
    assert res.headers["X-Frame-Options"] == "DENY"
    assert res.headers["Referrer-Policy"] == "no-referrer"


async def test_user_names_with_odd_characters_are_refused(client):
    res = await client.post("/admin/users", json={"user": "<script>", "password": enc("Counter#12", "user"), "role": "Cashier"})

    assert res.status_code == 422


# ---------- Change my own password ----------
async def test_user_changes_own_password(cashier_client, anon_client):
    res = await cashier_client.post("/auth/change-password", json={"current_password": enc("ravi", "current"), "new_password": enc("Counter#77", "new")})

    assert res.status_code == 200
    # The new token works; the session it replaced does not.
    new_headers = {"Authorization": f"Bearer {res.json()['access_token']}"}
    assert (await anon_client.get("/auth/me", headers=new_headers)).status_code == 200
    assert (await cashier_client.get("/auth/me")).status_code == 401
    assert (await anon_client.post("/auth/login", json={"user": "RAVI", "password": enc("Counter#77")})).status_code == 200
    assert (await anon_client.post("/auth/login", json={"user": "RAVI", "password": enc("ravi")})).status_code == 401


@pytest.mark.parametrize(("current", "new", "problem"), [
    ("wrong", "Counter#77", "current password is not correct"),
    ("ravi", "short1", "at least 8"),
    ("ravi", "ravi", "different from the current"),
])
async def test_change_password_rules(cashier_client, current, new, problem):
    res = await cashier_client.post("/auth/change-password", json={"current_password": enc(current, "current"), "new_password": enc(new, "new")})

    assert res.status_code == 422
    assert problem in res.json()["detail"]


async def test_guessing_the_current_password_is_locked_out(cashier_client):
    for _ in range(auth.MAX_FAILS_PER_USER):
        await cashier_client.post("/auth/change-password", json={"current_password": enc("guess", "current"), "new_password": enc("Counter#77", "new")})

    res = await cashier_client.post("/auth/change-password", json={"current_password": enc("ravi", "current"), "new_password": enc("Counter#77", "new")})

    assert res.status_code == 429


# ---------- Add / Edit / Delete ticks (Admin Master) ----------
async def test_rights_ticks_are_saved_and_returned(client):
    res = await client.put("/admin/roles/Manager", json={
        "name": "Manager", "modules": store.ROLES["Manager"],
        "rights": {"Customer Master": {"add": True, "edit": False, "delete": False}}})

    assert res.status_code == 200
    manager = next(r for r in (await client.get("/admin/roles")).json() if r["name"] == "Manager")
    assert manager["rights"]["Customer Master"] == {"add": True, "edit": False, "delete": False}


async def test_rights_ticks_are_enforced(client, manager_client):
    await client.put("/admin/roles/Manager", json={
        "name": "Manager", "modules": store.ROLES["Manager"],
        "rights": {"Customer Master": {"add": True, "edit": False, "delete": False},
                   "Sales Invoice": {"add": True, "edit": True, "delete": False}}})
    customer = {"code": "C7", "name": "Ravi Kumar"}

    assert (await manager_client.post("/masters/customers", json=customer)).status_code == 201
    assert (await manager_client.put("/masters/customers/C7", json=customer | {"name": "Ravi K"})).status_code == 403
    assert (await manager_client.delete("/masters/customers/C7")).status_code == 403
    bill = (await manager_client.post("/transactions/sales-invoice", json={
        "date": "2026-01-10", "counter": "counter10", "salesperson": "GURU",
        "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()
    cancel = await manager_client.post(f"/transactions/sales-invoice/{bill['doc_no']}/cancel", json={"remarks": "mistake"})
    assert cancel.status_code == 403
    assert "not allowed to delete in Sales Invoice" in cancel.json()["detail"]


async def test_login_tells_the_screen_its_rights(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "RAVI", "password": enc("ravi")})

    assert res.json()["rights"]["Item Master"]["delete"] is False


async def test_modules_without_ticks_keep_full_rights(manager_client):
    assert (await manager_client.post("/masters/brands", json={"code": "NEWB", "name": "New Brand"})).status_code == 201
    assert (await manager_client.delete("/masters/brands/NEWB")).status_code == 204


# ---------- Purchase costs hidden from roles without cost access ----------
async def test_cashier_does_not_see_purchase_rates(cashier_client, client):
    items = (await cashier_client.get("/masters/items")).json()
    one = (await cashier_client.get("/masters/items/VIN-750")).json()

    assert all(i["purchase_rate"] is None for i in items)
    assert one["purchase_rate"] is None
    assert (await client.get("/masters/items/VIN-750")).json()["purchase_rate"] == 759.29


async def test_cashier_edit_keeps_the_hidden_purchase_rate(cashier_client):
    item = (await cashier_client.get("/masters/items/VIN-750")).json()

    await cashier_client.put("/masters/items/VIN-750", json=item | {"purchase_rate": 1, "mrp": 845})

    assert store.ITEMS["VIN-750"]["purchase_rate"] == 759.29
    assert store.ITEMS["VIN-750"]["mrp"] == 845


async def test_cashier_reports_hide_cost_and_profit(cashier_client, client):
    today = date.today().isoformat()
    dsr = (await cashier_client.get("/reports/dsr", params={"on": today})).json()
    balance = (await cashier_client.get("/reports/stock-balance", params={"from_date": today, "to_date": today})).json()

    assert dsr["profit_of_the_day"] is None
    assert dsr["closing_stock"]["value"] is None and dsr["closing_stock"]["qty"] is not None
    assert dsr["purchase"]["incl_gst"] is None
    assert all(r["cl_value"] is None for r in balance)
    assert (await client.get("/reports/dsr", params={"on": today})).json()["profit_of_the_day"] is not None


async def test_cashier_sees_purchase_quantities_but_not_money(cashier_client):
    bills = (await cashier_client.get("/transactions/purchase-bill")).json()

    assert bills
    assert all(b["totals"]["net"] is None and b["totals"]["qty"] > 0 for b in bills)
    assert all(l["rate"] is None and l["qty"] > 0 for b in bills for l in b["lines"])


async def test_manager_sees_costs(manager_client):
    assert (await manager_client.get("/masters/items/VIN-750")).json()["purchase_rate"] == 759.29


async def test_token_from_before_the_update_gets_a_clear_message(anon_client):
    old = auth.create_access_token({"sub": "RAVI", "role": "Cashier"})   # no "pwv", like tokens issued before

    res = await anon_client.get("/auth/me", headers={"Authorization": f"Bearer {old}"})

    assert res.status_code == 401
    assert res.json()["detail"] == "The app was updated. Please log in again."


# ─── Credentials are always encrypted from the UI (app/secure_fields.py) ─────────

async def test_plain_password_is_refused(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": "admin"})
    assert res.status_code == 400
    assert "encrypted" in res.json()["detail"]


async def test_transport_key_is_public_and_login_works_with_it(anon_client):
    import base64, json as _json, time
    from cryptography.hazmat.primitives import hashes, serialization
    from cryptography.hazmat.primitives.asymmetric import padding
    key = (await anon_client.get("/auth/transport-key")).json()
    assert key["alg"] == "RSA-OAEP-256"
    pub = serialization.load_der_public_key(base64.b64decode(key["spki"]))
    ct = pub.encrypt(_json.dumps({"v": "admin", "t": key["server_time"], "f": "login"}).encode(),
                     padding.OAEP(mgf=padding.MGF1(hashes.SHA256()), algorithm=hashes.SHA256(), label=None))
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": "enc:v1:" + base64.b64encode(ct).decode()})
    assert res.status_code == 200


async def test_stale_encrypted_password_is_refused(anon_client, monkeypatch):
    from app import secure_fields
    old = enc("admin")
    monkeypatch.setattr(secure_fields.time, "time", lambda: 10**10)
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": old})
    assert res.status_code == 400 and "too long ago" in res.json()["detail"]


def test_sealed_secret_round_trips_and_hides_the_value():
    from app import secure_fields
    sealed = secure_fields.seal("S3cret!")
    assert sealed.startswith("sealed:v1:") and "S3cret" not in sealed
    assert secure_fields.unseal(sealed) == "S3cret!"
    assert secure_fields.unseal("plain-from-hand-written-env") == "plain-from-hand-written-env"


async def test_an_encrypted_password_works_only_once(anon_client):
    once = enc("admin")
    assert (await anon_client.post("/auth/login", json={"user": "ADMIN", "password": once})).status_code == 200
    again = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": once})
    assert again.status_code == 400 and "already sent" in again.json()["detail"]


async def test_an_encrypted_value_only_works_for_its_field(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": enc("admin", "current")})
    assert res.status_code == 400 and "different field" in res.json()["detail"]


async def test_transport_key_gives_server_time_for_pcs_with_a_wrong_clock(anon_client):
    import time
    key = (await anon_client.get("/auth/transport-key")).json()
    assert abs(key["server_time"] - time.time() * 1000) < 5000

