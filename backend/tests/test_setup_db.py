"""One-time Database setup (app/setup_db.py, app/envfile.py, routers/setup.py) without a real SQL Server:
the schema file, the .env writer, the connection strings, and the setup endpoints' guard (open while not
set up, an Administrator afterwards; the password only encrypted, and stored only sealed)."""
import re

import pytest

from app import boot, db, envfile, secure_fields, setup_db, sqlstore
from app.routers import auth

from conftest import enc

SCHEMA = setup_db.SCHEMA.read_text()
CODE = re.sub(r"--[^\n]*", "", SCHEMA)


def test_schema_creates_only_the_web_apps_own_tables():
    names = re.findall(r"CREATE TABLE dbo\.\[?(\w+)", CODE)
    assert sorted(names) == sorted(setup_db.WEB_TABLES)
    for batch in setup_db.batches(SCHEMA):
        code = re.sub(r"--[^\n]*", "", batch).strip()
        if re.search(r"\bCREATE\s+(TABLE|INDEX)\b", code, re.I):
            assert re.match(r"IF (OBJECT_ID|NOT EXISTS)", code, re.I), code[:80]   # guarded: safe to run twice
        assert not re.search(r"\b(ALTER|DROP|TRUNCATE)\s+TABLE\b", code, re.I), code[:80]
    # It never touches a desktop table, and holds no data, server names or logins.
    assert not re.search(r"\b(INSERT INTO|UPDATE|DELETE FROM)\b", CODE, re.I)
    assert not re.search(r"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", SCHEMA)
    assert "CREATE LOGIN" not in SCHEMA.upper() and "CREATE USER" not in SCHEMA.upper()


def test_a_database_that_is_not_vlqms_is_refused(monkeypatch):
    info = {"is_vlqms": False, "missing_tables": ["MstITEM"], "stock_trigger": True, "web_tables": 0, "can_create_tables": True}
    monkeypatch.setattr(setup_db, "check", lambda: info)
    monkeypatch.setattr(setup_db, "create_schema", lambda: pytest.fail("must not create anything in a foreign database"))
    with pytest.raises(setup_db.SetupError, match="MstITEM"):
        setup_db.prepare()


def test_a_database_without_the_stock_trigger_is_refused(monkeypatch):
    info = {"is_vlqms": True, "missing_tables": [], "stock_trigger": False, "web_tables": 0, "can_create_tables": True}
    monkeypatch.setattr(setup_db, "check", lambda: info)
    with pytest.raises(setup_db.SetupError, match="trigger"):
        setup_db.prepare()


def test_prepare_creates_the_web_tables_once_and_needs_the_right_to(monkeypatch):
    state = {"web_tables": 0, "can_create_tables": False, "created": 0}
    monkeypatch.setattr(setup_db, "check", lambda: {"is_vlqms": True, "missing_tables": [], "stock_trigger": True, **state})
    monkeypatch.setattr(setup_db, "create_schema", lambda: state.update(created=state["created"] + 1, web_tables=5))
    with pytest.raises(setup_db.SetupError, match="can't create"):
        setup_db.prepare()
    state["can_create_tables"] = True
    setup_db.prepare()
    setup_db.prepare()   # the tables are there now: nothing is created again
    assert state["created"] == 1


def test_a_sealed_password_this_pc_cannot_read_means_not_set_up(monkeypatch):
    monkeypatch.setattr(db, "DB_HOST", "server")
    monkeypatch.setattr(db, "DB_NAME", "vLQMS")
    monkeypatch.setattr(db, "DB_USER", "sa")
    monkeypatch.setattr(db, "DB_PASSWORD_ENC", "sealed:v1:from-another-pc")
    monkeypatch.setattr(db, "_password_unreadable", False)

    def unreadable():
        raise ValueError("InvalidTag")
    monkeypatch.setattr(db, "_password", unreadable)
    assert db.configured() is True
    assert db.init_db() is False   # no crash at startup
    assert db.configured() is False and boot.needs_setup() is True   # the sign-in page offers Database setup again

def test_batches_split_on_go_lines_only():
    assert setup_db.batches("SELECT 'GO'\nGO\n-- only a comment\nGO\nSELECT 2") == ["SELECT 'GO'", "SELECT 2"]


def test_odbc_server_name():
    assert db.pyodbc_server("localhost\\SQLEXPRESS", "1433") == "localhost\\SQLEXPRESS"   # instance: no port
    assert db.pyodbc_server("localhost", "") == "localhost"
    assert db.pyodbc_server("pos-pc", "1433") == "pos-pc,1433"


def test_envfile_replaces_only_db_keys(tmp_path):
    path = tmp_path / ".env"
    path.write_text("# comment\nJWT_SECRET=keep-me\nDB_HOST=old\nDB_PASSWORD=plain\nSMTP_HOST=smtp\n")
    envfile.update({"DB_HOST": "localhost\\SQLEXPRESS", "DB_NAME": "vLQMS", "DB_PASSWORD_ENC": "sealed:v1:x"}, path=path)
    text = path.read_text()
    assert "JWT_SECRET=keep-me" in text and "SMTP_HOST=smtp" in text and "# comment" in text
    assert "DB_HOST=localhost\\SQLEXPRESS" in text and "DB_NAME=vLQMS" in text and "DB_PASSWORD_ENC=sealed:v1:x" in text
    assert "DB_PASSWORD=plain" not in text and "DB_HOST=old" not in text
    with pytest.raises(ValueError):
        envfile.update({"DB_HOST": "x\nJWT_SECRET=evil"}, path=path)


# ─── Endpoints ───────────────────────────────────────────────────────────────

@pytest.fixture
def setup_mode(monkeypatch, tmp_path):
    """No database set up; .env writes go to a temp file; no real connections."""
    auth.reset_login_failures()
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    tried = []

    def fake_test(host, port, database, user, password, auth_kind):
        tried.append((host, port, database, user, password, auth_kind))
        return {"ok": True, "driver": "fake", "databases": ["vLQMSLHOUSE", "Other"]}
    monkeypatch.setattr(db, "test_connection", fake_test)
    monkeypatch.setattr(db, "reconnect", lambda: True)
    monkeypatch.setattr(setup_db, "prepare", lambda: {"database": "vLQMSLHOUSE", "edition": "Express Edition"})
    monkeypatch.setattr(setup_db, "is_ready", lambda: True)
    loaded = []
    monkeypatch.setattr(boot, "reload_after_setup", lambda: loaded.append(True))
    monkeypatch.setattr(db, "_loaded_after_setup", loaded, raising=False)
    for k in ("DB_HOST", "DB_PORT", "DB_NAME", "DB_USER", "DB_AUTH", "DB_PASSWORD", "DB_PASSWORD_ENC"):
        monkeypatch.setattr(db, k, getattr(db, k))
    yield tried
    auth.reset_login_failures()


BODY = {"host": "localhost\\SQLEXPRESS", "port": "", "auth": "sql", "user": "sa"}


async def test_status_is_public_and_says_setup_is_needed(anon_client):
    r = (await anon_client.get("/setup/status")).json()
    assert r["needs_setup"] is True and r["configured"] is False
    assert "host" not in r


async def test_setup_is_open_while_not_set_up(anon_client, setup_mode):
    r = await anon_client.post("/setup/test", json={**BODY, "password": enc("pw", "db")})
    assert r.status_code == 200
    assert len(setup_mode) == 1


async def test_password_must_be_encrypted_for_the_db_field(anon_client, setup_mode):
    r = await anon_client.post("/setup/test", json={**BODY, "password": "plain"})
    assert r.status_code in (400, 422)
    r = await anon_client.post("/setup/test", json={**BODY, "password": enc("pw", "login")})
    assert r.status_code in (400, 422)
    r = await anon_client.post("/setup/test", json={**BODY, "password": enc("pw", "db")})
    assert r.status_code == 200 and r.json()["databases"] == ["vLQMSLHOUSE", "Other"]
    assert setup_mode[-1] == ("localhost\\SQLEXPRESS", "", "", "sa", "pw", "sql")


async def test_connect_saves_the_password_only_sealed(anon_client, setup_mode):
    envfile.PATH.write_text("JWT_SECRET=keep\n")
    r = await anon_client.post("/setup/connect", json={**BODY, "database": "vLQMSLHOUSE",
                                                       "password": enc("S3cret!pw", "db")})
    assert r.status_code == 200 and r.json() == {"database": "vLQMSLHOUSE", "edition": "Express Edition", "ready": True}
    assert db._loaded_after_setup == [True]   # the shop's data is loaded at once
    text = envfile.PATH.read_text()
    assert "S3cret!pw" not in text and "JWT_SECRET=keep" in text and "DB_NAME=vLQMSLHOUSE" in text
    sealed = re.search(r"^DB_PASSWORD_ENC=(.+)$", text, re.M).group(1)
    assert secure_fields.unseal(sealed) == "S3cret!pw"
    assert db.DB_NAME == "vLQMSLHOUSE" and db._password() == "S3cret!pw"


async def test_connect_to_a_foreign_database_is_refused_and_loads_nothing(anon_client, setup_mode, monkeypatch):
    def refuse():
        raise setup_db.SetupError("This isn't a vLQMS database: these tables are missing: MstITEM.")
    monkeypatch.setattr(setup_db, "prepare", refuse)
    r = await anon_client.post("/setup/connect", json={**BODY, "database": "vLQMSLHOUSE", "password": enc("pw", "db")})
    assert r.status_code == 400 and "isn't a vLQMS database" in r.json()["detail"]
    assert db._loaded_after_setup == []


async def test_connect_refuses_a_database_the_server_does_not_have(anon_client, setup_mode):
    r = await anon_client.post("/setup/connect", json={**BODY, "database": "Nope",
                                                       "password": enc("pw", "db")})
    assert r.status_code == 404
    assert not envfile.PATH.exists()


async def test_bad_server_names_are_refused(anon_client, setup_mode):
    r = await anon_client.post("/setup/test", json={**BODY, "host": "x;Trusted_Connection=yes"})
    assert r.status_code == 422


async def test_once_set_up_only_an_administrator_can_change_it(anon_client, client, cashier_client, setup_mode, monkeypatch):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)   # the database is in use
    body = {**BODY, "password": enc("pw", "db")}
    assert (await anon_client.post("/setup/test", json=body)).status_code == 401
    assert (await cashier_client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 403
    assert (await client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 200


async def test_while_a_set_up_database_is_offline_setup_stays_closed(anon_client, setup_mode, monkeypatch):
    # Set up but unreachable (a network drop): nobody may repoint the app at another server meanwhile.
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(sqlstore, "enabled", lambda: False)
    assert (await anon_client.post("/setup/test", json={**BODY, "password": enc("pw", "db")})).status_code == 401
    assert setup_mode == []


async def test_odbc_values_are_escaped_and_bad_names_refused(anon_client, setup_mode):
    assert db._odbc("a}b") == "a}}b"
    for field in ("user", "host"):
        r = await anon_client.post("/setup/test", json={**BODY, field: "sa};Trusted_Connection=yes"})
        assert r.status_code == 422, field
    r = await anon_client.post("/setup/connect", json={**BODY, "database": "x];DROP",
                                                       "password": enc("pw", "db")})
    assert r.status_code == 422


async def test_writes_are_refused_while_a_set_up_database_is_offline(client, monkeypatch):
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "reconnect", lambda: False)
    res = await client.post("/masters/brands", json={"code": "ZZ", "name": "Zed"})
    assert res.status_code == 503 and "offline" in res.json()["detail"]
