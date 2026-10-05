"""One-time Database setup (app/setup_db.py, app/envfile.py, routers/setup.py) without a real SQL Server:
the schema file, the .env writer, the connection strings, and the setup endpoints' guard (setup code while
not set up, an Administrator afterwards; the password only encrypted, and stored only sealed)."""
import re

import pytest

from app import boot, db, envfile, secure_fields, setup_db, sqlstore
from app.routers import auth

from conftest import enc

SCHEMA = setup_db.SCHEMA.read_text()
CODE = re.sub(r"--[^\n]*", "", SCHEMA)


def test_schema_copies_every_table_with_web_names_only():
    tables = setup_db.schema_tables()
    assert len(tables) > 250 and "MstITEM" in tables and "TrnHdrSINV" in tables and "UtlStockList" in tables
    assert not any(t.startswith("Web_") for t in tables)
    for batch in setup_db.batches(SCHEMA):
        code = re.sub(r"--[^\n]*", "", batch).strip()
        if re.search(r"\bCREATE\s+(TABLE|INDEX|UNIQUE|NONCLUSTERED|CLUSTERED)\b", code, re.I):
            assert re.match(r"IF (OBJECT_ID|NOT EXISTS)", code, re.I), code[:80]
        if re.match(r"IF COL_LENGTH\('dbo\.Web_\w+', '\w+'\) < \d+", code):   # widening a Web_ number column
            assert not re.search(r"\b(DROP|TRUNCATE)\s+TABLE\b", code, re.I), code[:80]
            assert all(t.startswith("Web_") for t in re.findall(r"ALTER TABLE dbo\.\[(\w+)\]", code)), code[:80]
            continue
        assert not re.search(r"\b(ALTER|DROP|TRUNCATE)\s+TABLE\b", code, re.I), code[:80]
    # Every table it creates, and every table its triggers write, is a Web_ table.
    for name in re.findall(r"CREATE TABLE dbo\.\[?(\w+)", CODE):
        assert name.startswith("Web_"), name
    for name in re.findall(r"\b(?:INSERT INTO|UPDATE|DELETE FROM)\s+(?:dbo\.)?\[?(\w+)", CODE, re.I):
        assert name.startswith("Web_") or name in ("t",), name


def test_schema_holds_no_data_or_secrets():
    inserts = re.findall(r"INSERT INTO\s+(?:dbo\.)?\[?(\w+)", CODE, re.I)
    assert set(inserts) <= {"Web_SchemaVersion", "Web_TrnQOH"}   # the version row; the trigger's own insert
    assert not re.search(r"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", SCHEMA)
    assert "CREATE LOGIN" not in SCHEMA.upper() and "CREATE USER" not in SCHEMA.upper()


def test_rollback_drops_only_web_tables():
    rollback = setup_db.SCHEMA.with_name("002_web_legacy_tables.rollback.sql").read_text()
    for name in re.findall(r"DROP TABLE dbo\.\[?(\w+)", rollback):
        assert name.startswith("Web_"), name


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
    """No database set up; a known setup code; .env writes go to a temp file; no real connections."""
    auth.reset_login_failures()
    monkeypatch.setitem(boot.SETUP_CODE, "code", "AB12-CD34-EF56")
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    tried = []

    def fake_test(host, port, database, user, password, auth_kind):
        tried.append((host, port, database, user, password, auth_kind))
        return {"ok": True, "driver": "fake", "databases": ["vLQMSLHOUSE", "Other"]}
    monkeypatch.setattr(db, "test_connection", fake_test)
    monkeypatch.setattr(db, "reconnect", lambda: True)
    monkeypatch.setattr(setup_db, "check", lambda: {"database": "vLQMSLHOUSE", "legacy_tables": 277})
    for k in ("DB_HOST", "DB_PORT", "DB_NAME", "DB_USER", "DB_AUTH", "DB_PASSWORD", "DB_PASSWORD_ENC"):
        monkeypatch.setattr(db, k, getattr(db, k))
    yield tried
    auth.reset_login_failures()


BODY = {"host": "localhost\\SQLEXPRESS", "port": "", "auth": "sql", "user": "sa"}


async def test_status_is_public_and_says_setup_is_needed(anon_client):
    r = (await anon_client.get("/setup/status")).json()
    assert r["needs_setup"] is True and r["configured"] is False
    assert "host" not in r


async def test_setup_code_is_required_and_locks_after_five_wrong(anon_client, setup_mode):
    for _ in range(5):
        r = await anon_client.post("/setup/test", json={**BODY, "setup_code": "WRONG", "password": enc("pw", "db")})
        assert r.status_code == 403
    r = await anon_client.post("/setup/test", json={**BODY, "setup_code": "AB12-CD34-EF56", "password": enc("pw", "db")})
    assert r.status_code == 429
    assert setup_mode == []


async def test_password_must_be_encrypted_for_the_db_field(anon_client, setup_mode):
    r = await anon_client.post("/setup/test", json={**BODY, "setup_code": "ab12-cd34-ef56", "password": "plain"})
    assert r.status_code in (400, 422)
    r = await anon_client.post("/setup/test", json={**BODY, "setup_code": "ab12-cd34-ef56", "password": enc("pw", "login")})
    assert r.status_code in (400, 422)
    r = await anon_client.post("/setup/test", json={**BODY, "setup_code": "ab12-cd34-ef56", "password": enc("pw", "db")})
    assert r.status_code == 200 and r.json()["databases"] == ["vLQMSLHOUSE", "Other"]
    assert setup_mode[-1] == ("localhost\\SQLEXPRESS", "", "", "sa", "pw", "sql")


async def test_connect_saves_the_password_only_sealed(anon_client, setup_mode):
    envfile.PATH.write_text("JWT_SECRET=keep\n")
    r = await anon_client.post("/setup/connect", json={**BODY, "setup_code": "AB12-CD34-EF56", "database": "vLQMSLHOUSE",
                                                       "password": enc("S3cret!pw", "db")})
    assert r.status_code == 200 and r.json()["legacy_tables"] == 277
    text = envfile.PATH.read_text()
    assert "S3cret!pw" not in text and "JWT_SECRET=keep" in text and "DB_NAME=vLQMSLHOUSE" in text
    sealed = re.search(r"^DB_PASSWORD_ENC=(.+)$", text, re.M).group(1)
    assert secure_fields.unseal(sealed) == "S3cret!pw"
    assert db.DB_NAME == "vLQMSLHOUSE" and db._password() == "S3cret!pw"


async def test_connect_refuses_a_database_the_server_does_not_have(anon_client, setup_mode):
    r = await anon_client.post("/setup/connect", json={**BODY, "setup_code": "AB12-CD34-EF56", "database": "Nope",
                                                       "password": enc("pw", "db")})
    assert r.status_code == 404
    assert not envfile.PATH.exists()


async def test_bad_server_names_are_refused(anon_client, setup_mode):
    r = await anon_client.post("/setup/test", json={**BODY, "host": "x;Trusted_Connection=yes", "setup_code": "AB12-CD34-EF56"})
    assert r.status_code == 422


async def test_once_set_up_only_an_administrator_can_change_it(anon_client, client, cashier_client, setup_mode, monkeypatch):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)   # the database is in use
    body = {**BODY, "setup_code": "AB12-CD34-EF56", "password": enc("pw", "db")}
    assert (await anon_client.post("/setup/test", json=body)).status_code == 401
    assert (await cashier_client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 403
    assert (await client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 200


async def test_while_the_database_is_offline_the_setup_code_is_needed_again(anon_client, client, setup_mode, monkeypatch):
    # Set up but unreachable: the app runs on sample data, so a demo Administrator must not repoint it.
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    body = {**BODY, "setup_code": "WRONG", "password": enc("pw", "db")}
    assert (await client.post("/setup/test", json=body)).status_code == 403
    assert (await client.post("/setup/test", json={**body, "setup_code": "AB12-CD34-EF56",
                                                   "password": enc("pw", "db")})).status_code == 200


async def test_odbc_values_are_escaped_and_bad_names_refused(anon_client, setup_mode):
    assert db._odbc("a}b") == "a}}b"
    for field in ("user", "host"):
        r = await anon_client.post("/setup/test", json={**BODY, field: "sa};Trusted_Connection=yes", "setup_code": "AB12-CD34-EF56"})
        assert r.status_code == 422, field
    r = await anon_client.post("/setup/connect", json={**BODY, "setup_code": "AB12-CD34-EF56", "database": "x];DROP",
                                                       "password": enc("pw", "db")})
    assert r.status_code == 422


async def test_writes_are_refused_while_a_set_up_database_is_offline(client, monkeypatch):
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "reconnect", lambda: False)
    res = await client.post("/masters/brands", json={"code": "ZZ", "name": "Zed"})
    assert res.status_code == 503 and "offline" in res.json()["detail"]


async def test_progress_shows_no_private_keys(anon_client, monkeypatch):
    monkeypatch.setattr(setup_db, "PROGRESS", {"state": "copying", "table": "MstITEM", "_beat": 1.0})
    assert (await anon_client.get("/setup/progress")).json() == {"state": "copying", "table": "MstITEM"}
