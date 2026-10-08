"""One-time Database setup on the sign-in page (and later Utilities → Database Connectivity).

The client's backend runs on their own PC/server, next to their local Microsoft SQL Server. The first time,
the sign-in page shows Database setup instead of the login form:
  GET  /setup/status    public: configured? connected? ready? (never where the server is)
  POST /setup/test      try server / login / password on a separate connection; lists the databases
  POST /setup/connect   save the connection in backend/.env (password sealed), connect to the shop's vLQMS
                        database, create the web app's own tables in it (nothing is copied: the app works on
                        the desktop's tables directly) and load the shop's data
  POST /setup/admin     once the database is in use but no active user is an Administrator: make one (a desktop
                        user or a new one) with a new password; only while there is none
  GET  /setup/current   the settings in use (no password), to fill the form when they are changed later
  GET/POST /setup/smtp, POST /setup/smtp/test   the e-mail (SMTP) settings used to send reports (SMTP_* in .env, the
                        password sealed as SMTP_PASSWORD_ENC); same access as the database settings
  GET/POST /setup/rpt-folder   where the .rpt print formats are (RPT_FOLDER in .env): shown and changed from the sign-in
                        page's settings and Print Settings; same access as the database settings
While the database isn't set up these are open to anyone who can reach the sign-in page. Once it is set up they need
an Administrator's token (Global Settings → Database). The one exception is a set-up database that is OFFLINE: then
nobody can sign in, so the sign-in page's "Database settings" works from the server PC itself (a request from this
machine), and from anywhere with an Administrator's token. The sign-in page offers the settings (database and report
files path) at any time: they open for a request from the server PC itself or an Administrator's token, never for
anyone else on the network. The password arrives encrypted (secure_fields, purpose
"db"), is never returned, and is stored only sealed; leaving it blank when changing keeps the saved one.
"""
import os
import re
from pathlib import Path
from typing import Literal

from fastapi import APIRouter, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials
from pydantic import BaseModel, Field

from .. import admin_audit, boot, db, envfile, mailer, rpt_bridge, secure_fields, security, setup_db, sqlstore, store
from . import auth

router = APIRouter(prefix="/setup", tags=["Database setup"])


class ServerIn(BaseModel):
    host: str = Field(min_length=1, max_length=200)
    port: str = Field("", max_length=5, pattern=r"^\d*$")
    auth: Literal["sql"] = "sql"
    user: str = Field("", max_length=128)
    password: str = Field("", max_length=1000)


class ConnectIn(ServerIn):
    database: str = Field(min_length=1, max_length=128)


class AdminIn(BaseModel):
    user: str = Field(min_length=1, max_length=50)
    password: str = Field(min_length=1, max_length=1000)   # encrypted, purpose "new"


LOOPBACK = {"127.0.0.1", "::1", "localhost"}


def _guard(request: Request) -> str:
    """Open while the database isn't set up (there are no users then); otherwise an Administrator's token. A set-up
    database that is offline also accepts a request from this PC itself (nobody can sign in to get a token then, and
    anyone on the network must not be able to repoint the app). Returns the signed-in user's name ('' when none)."""
    if boot.needs_setup():
        return ""
    header = request.headers.get("authorization", "")
    if header.lower().startswith("bearer "):
        user = auth.get_current_user(HTTPAuthorizationCredentials(scheme="Bearer", credentials=header[7:]))
        if user.get("role") != store.ADMIN_ROLE:
            raise HTTPException(403, "Only an Administrator can change the database")
        return str(user["user"])
    if request.client and request.client.host in LOOPBACK:   # the server PC itself (anyone on the network is refused)
        return ""
    raise HTTPException(403, "Only an Administrator can change these settings: sign in as an Administrator "
                             "(Global Settings), or open this page on the server PC itself.")


BAD_CHARS = set(";{}=[]\r\n'\"")


def _clean(value: str, what: str) -> str:
    value = value.strip()
    if any(c in BAD_CHARS for c in value):
        raise HTTPException(422, f"{what}: these characters can't be used: ; {{ }} = [ ] ' \"")
    return value


def _server(body: ServerIn) -> tuple[str, str, str, str, str]:
    host = _clean(body.host, "Server name")
    if not _clean(body.user, "Login"):
        raise HTTPException(422, "Enter the SQL login name")
    password = secure_fields.decrypt(body.password, "db") if body.password else ""
    if not password and db.configured():
        password = db._password()   # blank = keep the saved password (changing the settings later)
    return host, body.port.strip(), body.user.strip(), password or "", body.auth


@router.get("/status")
def status():
    if db.configured() and not sqlstore.enabled():
        sqlstore.writes_blocked()   # set up but offline: try to come back (at most every 20 s) and load the data
    return {"configured": db.configured(), "connected": db.is_db_available(), "needs_setup": boot.needs_setup(),
            "needs_admin": boot.needs_admin()}


@router.get("/current")
def current(request: Request):
    """The connection settings in use, to fill the form (never the password; `has_password` says one is saved)."""
    _guard(request)
    s = db.settings()
    return {"host": s["host"], "port": s["port"] or "", "database": s["database"], "auth": s["auth"], "user": s["user"],
            "has_password": bool(db.configured() and db._password()), "connected": db.is_db_available()}


@router.post("/test")
def test(body: ServerIn, request: Request):
    _guard(request)
    host, port, user, password, auth_kind = _server(body)
    return db.test_connection(host, port, "", user, password, auth_kind)


@router.post("/connect")
def connect(body: ConnectIn, request: Request):
    who = _guard(request)
    before = db.settings() if db.configured() else None
    host, port, user, password, auth_kind = _server(body)
    database = _clean(body.database, "Database")
    tried = db.test_connection(host, port, "", user, password, auth_kind)
    if not tried["ok"]:
        raise HTTPException(400, tried["error"])
    if database not in tried["databases"]:
        raise HTTPException(404, f"Database '{database}' isn't on that server")
    sealed = secure_fields.seal(password) if password else ""
    envfile.update({"DB_HOST": host, "DB_PORT": port, "DB_NAME": database, "DB_AUTH": auth_kind,
                    "DB_USER": user, "DB_PASSWORD_ENC": sealed})
    db.use_settings(host, port, database, user, auth_kind, sealed)
    if not db.reconnect():
        raise HTTPException(503, "Saved, but the app could not connect with these settings. Check them and try again.")
    try:
        info = setup_db.prepare()   # a vLQMS database? then the web app's own tables
    except setup_db.SetupError as exc:
        raise HTTPException(400, str(exc)) from exc
    boot.reload_after_setup()   # the shop's data, straight from this database
    if before is not None:   # a change of settings (not the first setup): written to the ADMIN Audit Log; never the password
        admin_audit.write_now(who or "SERVER PC", "Database settings", "Edited", database,
                              f"database {before['database']}",
                              f"database {database}")
    return {"database": info["database"], "edition": info["edition"], "ready": setup_db.is_ready()}


class RptFolderIn(BaseModel):
    folder: str = Field("", max_length=500)   # blank = the default (backend/Rpts)


def _rpt_folder_info() -> dict:
    folder = rpt_bridge.folder()
    files = rpt_bridge.rpt_files()
    return {"folder": rpt_bridge._env("RPT_FOLDER"), "default": str(rpt_bridge.DEFAULT_FOLDER), "effective": str(folder),
            "exists": folder.is_dir(), "count": len(files), "files": files[:30]}


@router.get("/rpt-folder")
def get_rpt_folder(request: Request):
    """Where the .rpt print formats are read from, and what is in that folder."""
    _guard(request)
    return _rpt_folder_info()


@router.post("/rpt-folder")
def set_rpt_folder(body: RptFolderIn, request: Request):
    """Save the .rpt folder (RPT_FOLDER in backend/.env) and use it at once. It must be an existing folder on this PC."""
    who = _guard(request)
    folder = _clean(body.folder, "Report files path")
    if folder:
        if not Path(folder).is_dir():
            raise HTTPException(422, f"'{folder}' is not a folder on this PC")
    before = rpt_bridge._env("RPT_FOLDER")
    envfile.update({"RPT_FOLDER": folder})
    if folder:
        os.environ["RPT_FOLDER"] = folder
    else:
        os.environ.pop("RPT_FOLDER", None)
    if before != folder:
        admin_audit.write_now(who or "SERVER PC", "Report files path", "Edited", "RPT_FOLDER", before or "(default)", folder or "(default)")
    return _rpt_folder_info()


def smtp_current() -> dict:
    """The saved SMTP settings as SmtpIn fields (no password)."""
    i = _smtp_info()
    return {k: i[k] for k in ("host", "port", "security", "user", "from_address")}


class SmtpIn(BaseModel):
    host: str = Field("", max_length=200)          # blank turns e-mail off
    port: str = Field("587", max_length=5, pattern=r"^\d*$")
    security: Literal["starttls", "ssl", "none"] = "starttls"
    user: str = Field("", max_length=200)
    from_address: str = Field("", max_length=200)
    password: str = Field("", max_length=1000)     # encrypted (purpose "smtp"); blank keeps the saved one


def _smtp_info() -> dict:
    return {"host": mailer._env("SMTP_HOST"), "port": mailer._env("SMTP_PORT", "587") or "587",
            "security": mailer._env("SMTP_SECURITY", "starttls").lower() or "starttls", "user": mailer._env("SMTP_USER"),
            "from_address": mailer._env("SMTP_FROM"), "has_password": bool(mailer.password()), "configured": mailer.configured()}


def _smtp_values(body: SmtpIn) -> tuple[str, int, str, str, str]:
    host = _clean(body.host, "SMTP server")
    user = _clean(body.user, "SMTP user")
    sender = _clean(body.from_address, "From address")
    if sender and not re.fullmatch(r"[^\s@]+@[^\s@]+\.[^\s@]+", sender):
        raise HTTPException(422, "From address is not a valid e-mail address")
    port = int(body.port or 587)
    if host and not 1 <= port <= 65535:
        raise HTTPException(422, "SMTP port must be between 1 and 65535")
    return host, port, body.security, user, sender


@router.get("/smtp")
def get_smtp(request: Request):
    """The e-mail (SMTP) settings in use; the password is never returned, only whether one is saved."""
    _guard(request)
    return _smtp_info()


@router.post("/smtp")
def set_smtp(body: SmtpIn, request: Request):
    """Save the SMTP settings in backend/.env and use them at once. The password arrives encrypted and is stored sealed;
    leaving it blank keeps the saved one. A blank server turns e-mail off."""
    return save_smtp(body, _guard(request))


def save_smtp(body: SmtpIn, who: str) -> dict:
    """Shared by this screen's endpoint and the Global Settings grid (routers/utilities.py)."""
    host, port, security_mode, user, sender = _smtp_values(body)
    before = _smtp_info()
    values = {"SMTP_HOST": host, "SMTP_PORT": str(port) if host else "", "SMTP_SECURITY": security_mode if host else "",
              "SMTP_USER": user, "SMTP_FROM": sender}
    password = secure_fields.decrypt(body.password, "smtp", "SMTP password")
    if not host:
        values["SMTP_PASSWORD_ENC"] = ""
    elif password:
        values["SMTP_PASSWORD_ENC"] = secure_fields.seal(password)
    envfile.update(values, remove=("SMTP_PASSWORD",) if (password or not host) else ())
    for key, value in values.items():
        if value:
            os.environ[key] = value
        else:
            os.environ.pop(key, None)
    if password or not host:
        os.environ.pop("SMTP_PASSWORD", None)
    after = _smtp_info()
    changed = [k for k in ("host", "port", "security", "user", "from_address") if before[k] != after[k]]
    if password:
        changed.append("password")
    if changed:
        admin_audit.write_now(who or "SERVER PC", "Email (SMTP) settings", "Edited", "SMTP", "", ", ".join(changed) + " changed")
    return after


@router.post("/smtp/test")
def test_smtp(body: SmtpIn, request: Request):
    """Connect to the SMTP server with these settings (the saved password when none is typed) and sign in; sends nothing."""
    _guard(request)
    host, port, security_mode, user, _sender = _smtp_values(body)
    if not host:
        raise HTTPException(422, "Enter the SMTP server")
    password = secure_fields.decrypt(body.password, "smtp", "SMTP password") or mailer.password()
    try:
        mailer.check(host, port, security_mode, user, password)
    except Exception as exc:   # smtplib / ssl / socket errors: say what went wrong, never the password
        raise HTTPException(502, f"Could not connect to the e-mail server: {exc}")
    return {"ok": True, "message": f"Connected to {host}:{port}" + (" and signed in." if user else ".")}


class PrinterIn(BaseModel):
    printer: str = Field("", max_length=200)   # blank = use each document's own saved printer


def _printer_info() -> dict:
    report = {"usable": [], "refused": []} if rpt_bridge.using_mock() else rpt_bridge.printer_report()
    import getpass
    try:
        runs_as = getpass.getuser()   # the Windows user this backend (and the bridge it starts) runs as; a name ending in $ is LocalSystem
    except Exception:   # noqa: BLE001
        runs_as = ""
    return {"printer": rpt_bridge._env("DIRECT_PRINTER"), "installed": report["usable"], "refused": report["refused"], "runs_as": runs_as}


@router.get("/print-printer")
def get_print_printer(request: Request):
    """The printer all direct prints use on this server (DIRECT_PRINTER in .env), and the printers this server can see."""
    _guard(request)
    return _printer_info()


@router.post("/print-printer")
def set_print_printer(body: PrinterIn, request: Request):
    """Save the server's printer for direct prints. It overrides the printer saved with each print option (which the desktop
    shares and which may be a name from a Remote Desktop session); it must be one this server can see."""
    who = _guard(request)
    printer = _clean(body.printer, "Printer")
    installed = [] if rpt_bridge.using_mock() else rpt_bridge.installed_printers()
    if printer and installed and printer.lower() not in {p.lower() for p in installed}:
        raise HTTPException(422, f"This server can't see a printer called '{printer}'. It sees: {', '.join(installed) or 'none'}.")
    before = rpt_bridge._env("DIRECT_PRINTER")
    envfile.update({"DIRECT_PRINTER": printer})
    if printer:
        os.environ["DIRECT_PRINTER"] = printer
    else:
        os.environ.pop("DIRECT_PRINTER", None)
    if before != printer:
        admin_audit.write_now(who or "SERVER PC", "Printer on this server", "Edited", "DIRECT_PRINTER", before or "(each document's own)", printer or "(each document's own)")
    return _printer_info()


@router.post("/admin", status_code=201)
def create_admin(body: AdminIn, request: Request):
    """The first Administrator, when the shop's users (MstUser) have none. A desktop user keeps their
    details and becomes Administrator with this password; any other name becomes a new user."""
    if not boot.needs_admin():
        raise HTTPException(409, "There is already an Administrator. Sign in, and manage users in Admin Master.")
    name = _clean(body.user, "User name").upper()
    if not name:
        raise HTTPException(422, "Enter the user name")
    password = secure_fields.decrypt(body.password, "new", "Password")
    problem = security.password_problem(password, name)
    if problem:
        raise HTTPException(422, problem)
    u = store.USERS.get(name)
    if u is None:
        u = store.USERS[name] = {"user": name, "active": True, "pwv": 1}
    earlier = u.get("role", "")
    u.update(role=store.ADMIN_ROLE, role_set_in_web=True, active=True)
    auth._set_password(u, password)
    store.touch("USERS", name)
    admin_audit.record("Database setup", "Administrator created", name, earlier, store.ADMIN_ROLE, user="SETUP")
    return {"user": name, "role": store.ADMIN_ROLE}
