"""Health check, login, and who may call what."""
import pytest
from conftest import enc


async def test_health_is_public(anon_client):
    res = await anon_client.get("/")

    assert res.status_code == 200
    assert res.json()["status"] == "ok"
    assert res.json()["db"]["connected"] is False


async def test_login_returns_token_role_and_modules(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "admin", "password": enc("admin")})

    assert res.status_code == 200
    body = res.json()
    assert body["user"] == "ADMIN"
    assert body["role"] == "Administrator"
    assert body["modules"] == ["*"]
    assert body["token_type"] == "bearer"
    assert body["access_token"]


async def test_login_rejects_wrong_password(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": enc("wrong")})

    assert res.status_code == 401
    assert res.json()["detail"] == "Invalid user name or password"


async def test_changed_password_replaces_the_old_one(client, anon_client):
    res = await client.put("/admin/users/GURU", json={"user": "GURU", "password": enc("n3w-pass", "user"), "role": "Manager"})
    assert res.status_code == 200

    old = await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("guru")})
    new = await anon_client.post("/auth/login", json={"user": "GURU", "password": enc("n3w-pass")})

    assert old.status_code == 401
    assert new.status_code == 200


async def test_me_reads_the_bearer_token(cashier_client):
    res = await cashier_client.get("/auth/me")

    assert res.status_code == 200
    assert res.json()["user"] == "RAVI"
    assert res.json()["role"] == "Cashier"


async def test_bad_token_is_rejected(anon_client):
    res = await anon_client.get("/auth/me", headers={"Authorization": "Bearer not-a-token"})

    assert res.status_code == 401


# The sign-in page loads these before anyone logs in.
@pytest.mark.parametrize("path", ["/", "/masters/company"])
async def test_public_endpoints_work_without_login(anon_client, path):
    assert (await anon_client.get(path)).status_code == 200


@pytest.mark.parametrize(("method", "path"), [
    ("GET", "/masters/items"),
    ("DELETE", "/masters/items/VIN-750"),
    ("POST", "/transactions/sales-invoice"),
    ("GET", "/reports/restock"),
    ("PUT", "/settings"),
    ("GET", "/admin/users"),
    ("GET", "/company/files"),
])
async def test_everything_else_needs_login(anon_client, method, path):
    res = await anon_client.request(method, path, json={})

    assert res.status_code == 401


async def test_user_name_header_alone_is_not_a_login(anon_client):
    res = await anon_client.get("/admin/users", headers={"X-User": "ADMIN"})

    assert res.status_code == 401


async def test_admin_can_list_users(client):
    res = await client.get("/admin/users")

    assert res.status_code == 200
    assert "ADMIN" in {u["user"] for u in res.json()}


async def test_cashier_cannot_list_users(cashier_client):
    assert (await cashier_client.get("/admin/users")).status_code == 403


async def test_non_ascii_password_is_just_wrong(anon_client):
    res = await anon_client.post("/auth/login", json={"user": "ADMIN", "password": enc("pässwörd")})

    assert res.status_code == 401
