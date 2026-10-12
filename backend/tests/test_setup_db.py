"""One-time Database setup (app/setup_db.py, app/envfile.py, routers/setup.py) without a real SQL Server:
the schema file, the .env writer, the connection strings, and the setup endpoints' guard (open while not
set up, an Administrator afterwards; the password only encrypted, and stored only sealed)."""
import re

import pytest

from app import boot, db, envfile, secure_fields, setup_db, sqlstore
from app.routers import auth, setup as setup_router

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
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(setup_router, "LOOPBACK", set())   # a request from another PC
    body = {**BODY, "password": enc("pw", "db")}
    assert (await anon_client.post("/setup/test", json=body)).status_code == 403
    assert (await cashier_client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 403
    assert (await client.post("/setup/test", json={**body, "password": enc("pw", "db")})).status_code == 200


async def test_while_a_set_up_database_is_offline_only_the_server_pc_may_change_it(anon_client, setup_mode, monkeypatch):
    # Set up but unreachable: nobody can sign in, so only a request from the server PC itself is accepted.
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(sqlstore, "enabled", lambda: False)
    monkeypatch.setattr(db, "is_db_available", lambda: False)
    monkeypatch.setattr(setup_router, "LOOPBACK", set())   # a request from another PC
    assert (await anon_client.post("/setup/test", json={**BODY, "password": enc("pw", "db")})).status_code == 403
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


async def test_the_server_pc_itself_may_open_the_settings_from_the_sign_in_page(anon_client, setup_mode, monkeypatch):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    assert (await anon_client.get("/setup/rpt-folder")).status_code == 200   # the test client counts as the server PC
    monkeypatch.setattr(setup_router, "LOOPBACK", set())
    assert (await anon_client.get("/setup/rpt-folder")).status_code == 403


async def test_the_rpt_folder_is_saved_checked_and_used_at_once(client, monkeypatch, tmp_path):
    from app import rpt_bridge
    monkeypatch.setattr(boot, "needs_setup", lambda: False)   # set up: an Administrator's token is checked
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    monkeypatch.delenv("RPT_FOLDER", raising=False)
    (tmp_path / "a.rpt").write_bytes(b"x")
    (tmp_path / "b.RPT").write_bytes(b"x")
    r = await client.post("/setup/rpt-folder", json={"folder": str(tmp_path)})
    assert r.status_code == 200 and r.json()["count"] == 2 and r.json()["exists"]
    assert rpt_bridge.folder() == tmp_path and "RPT_FOLDER=" in (tmp_path / ".env").read_text()
    assert (await client.post("/setup/rpt-folder", json={"folder": str(tmp_path / "nope")})).status_code == 422
    assert (await client.post("/setup/rpt-folder", json={"folder": "a;b"})).status_code == 422
    r = await client.post("/setup/rpt-folder", json={"folder": ""})   # blank = the default folder again
    assert r.status_code == 200 and rpt_bridge.folder() == rpt_bridge.DEFAULT_FOLDER


async def test_a_cashier_cannot_change_the_rpt_folder(cashier_client, monkeypatch):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(setup_router, "LOOPBACK", set())
    assert (await cashier_client.post("/setup/rpt-folder", json={"folder": ""})).status_code == 403


async def test_smtp_settings_are_saved_sealed_and_used_at_once(client, monkeypatch, tmp_path):
    from app import mailer, secure_fields
    from tests.conftest import enc
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    for key in ("SMTP_HOST", "SMTP_PORT", "SMTP_SECURITY", "SMTP_USER", "SMTP_FROM", "SMTP_PASSWORD", "SMTP_PASSWORD_ENC"):
        monkeypatch.delenv(key, raising=False)
    body = {"host": "smtp.example.com", "port": "465", "security": "ssl", "user": "shop@example.com",
            "from_address": "shop@example.com", "password": enc("S3cret!pw", "smtp")}
    r = await client.post("/setup/smtp", json=body)
    assert r.status_code == 200 and r.json()["configured"] and r.json()["has_password"] and "password" not in r.json()
    assert mailer.password() == "S3cret!pw" and mailer.configured()
    text = (tmp_path / ".env").read_text()
    assert "SMTP_HOST=smtp.example.com" in text and "S3cret" not in text and "SMTP_PASSWORD_ENC=" in text
    # A blank password keeps the saved one; a blank server turns e-mail off.
    r = await client.post("/setup/smtp", json=body | {"password": "", "port": "587"})
    assert r.json()["port"] == "587" and mailer.password() == "S3cret!pw"
    assert (await client.get("/setup/smtp")).json()["host"] == "smtp.example.com"
    r = await client.post("/setup/smtp", json={"host": ""})
    assert not r.json()["configured"] and not mailer.password()
    assert secure_fields  # sealed with the server's own key


async def test_smtp_settings_are_checked(client, monkeypatch, tmp_path):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    assert (await client.post("/setup/smtp", json={"host": "h", "port": "99999"})).status_code == 422
    assert (await client.post("/setup/smtp", json={"host": "h;x"})).status_code == 422
    assert (await client.post("/setup/smtp", json={"host": "h", "from_address": "not-an-email"})).status_code == 422
    assert (await client.post("/setup/smtp/test", json={"host": ""})).status_code == 422


async def test_smtp_test_reports_a_failed_connection_without_the_password(client, monkeypatch):
    from app import mailer
    from tests.conftest import enc
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    def refuse(*args):
        raise OSError("connection refused")
    monkeypatch.setattr(mailer, "check", refuse)
    r = await client.post("/setup/smtp/test", json={"host": "smtp.example.com", "user": "u", "password": enc("hunter2!!", "smtp")})
    assert r.status_code == 502 and "connection refused" in r.json()["detail"] and "hunter2" not in r.text
    monkeypatch.setattr(mailer, "check", lambda *a: None)
    r = await client.post("/setup/smtp/test", json={"host": "smtp.example.com"})
    assert r.status_code == 200 and r.json()["ok"]


async def test_a_cashier_cannot_change_the_smtp_settings(cashier_client, monkeypatch):
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(setup_router, "LOOPBACK", set())
    assert (await cashier_client.post("/setup/smtp", json={"host": ""})).status_code == 403


async def test_the_server_printer_is_saved_and_must_be_one_the_server_can_open(client, monkeypatch):
    from app import rpt_bridge
    have = ["EPSON TM-T82", "\\\\saturn2\\POS-80"]
    monkeypatch.setattr(boot, "needs_setup", lambda: False)
    monkeypatch.setattr(rpt_bridge, "using_mock", lambda: False)
    monkeypatch.setattr(rpt_bridge, "printer_report", lambda fresh=False: {"usable": have, "refused": [{"name": "POS-80 on saturn2 (redirected 4)", "why": "Windows error 1801"}]})
    monkeypatch.setattr(rpt_bridge, "installed_printers", lambda: have)
    monkeypatch.delenv("DIRECT_PRINTER", raising=False)
    r = await client.get("/setup/print-printer")
    assert r.status_code == 200 and r.json()["installed"] == have and r.json()["refused"][0]["why"] == "Windows error 1801"
    bad = await client.post("/setup/print-printer", json={"printer": "POS-80 on saturn2 (redirected 4)"})
    assert bad.status_code == 422 and "EPSON TM-T82" in bad.json()["detail"]
    ok = await client.post("/setup/print-printer", json={"printer": "\\\\saturn2\\POS-80"})
    assert ok.status_code == 200 and ok.json()["printer"] == "\\\\saturn2\\POS-80" and rpt_bridge._env("DIRECT_PRINTER") == "\\\\saturn2\\POS-80"
    assert (await client.post("/setup/print-printer", json={"printer": ""})).json()["printer"] == ""


def test_connecting_to_another_database_forgets_everything_of_the_old_one():
    """Hashes of what the old database held, its column lists, pending audit lines and replayed requests must not carry over, or a
    row that looks unchanged is never written to the new database and old values show up."""
    from app import admin_audit, boot, schema_meta, sqlstore, store
    sqlstore._saved[("row", "ITEMS", "A")] = "hash-from-the-old-database"
    sqlstore._doc_status[("sales_invoice", "26/S-1")] = "SAVED"
    sqlstore._cols["mstitem"] = {"x": 1}
    sqlstore._mapped_cache["items"] = {"code"}
    schema_meta._cols["MstITEM"] = [{"name": "ItemCode"}]
    admin_audit._pending.append({"screen": "Item Master"})
    store.IDEMPOTENT["key"] = {"doc_no": "x"}
    boot.forget_old_database()
    assert not sqlstore._saved and not sqlstore._doc_status and not sqlstore._cols and not sqlstore._mapped_cache
    assert not schema_meta._cols and not admin_audit._pending and not store.IDEMPOTENT
