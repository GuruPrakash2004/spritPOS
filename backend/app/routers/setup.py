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
While the database isn't set up these are open to anyone who can reach the sign-in page. Once it is set up
(also while it is offline) they need an Administrator's token instead. The password arrives encrypted
(secure_fields, purpose "db"), is never returned, and is stored only sealed.
"""
from typing import Literal

from fastapi import APIRouter, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials
from pydantic import BaseModel, Field

from .. import admin_audit, boot, db, envfile, secure_fields, security, setup_db, sqlstore, store
from . import auth

router = APIRouter(prefix="/setup", tags=["Database setup"])


class ServerIn(BaseModel):
    host: str = Field(min_length=1, max_length=200)
    port: str = Field("", max_length=5, pattern=r"^\d*$")
    auth: Literal["sql", "windows"] = "sql"
    user: str = Field("", max_length=128)
    password: str = Field("", max_length=1000)


class ConnectIn(ServerIn):
    database: str = Field(min_length=1, max_length=128)


class AdminIn(BaseModel):
    user: str = Field(min_length=1, max_length=50)
    password: str = Field(min_length=1, max_length=1000)   # encrypted, purpose "new"


def _guard(request: Request) -> None:
    """Open while the database isn't set up (there are no users then); an Administrator's token otherwise, also
    while a set-up database is offline, so nobody can repoint the app during a network drop."""
    if boot.needs_setup():
        return
    header = request.headers.get("authorization", "")
    creds = HTTPAuthorizationCredentials(scheme="Bearer", credentials=header[7:]) if header.lower().startswith("bearer ") else None
    user = auth.get_current_user(creds)
    if user.get("role") != store.ADMIN_ROLE:
        raise HTTPException(403, "Only an Administrator can change the database")


BAD_CHARS = set(";{}=[]\r\n'\"")


def _clean(value: str, what: str) -> str:
    value = value.strip()
    if any(c in BAD_CHARS for c in value):
        raise HTTPException(422, f"{what}: these characters can't be used: ; {{ }} = [ ] ' \"")
    return value


def _server(body: ServerIn) -> tuple[str, str, str, str, str]:
    host = _clean(body.host, "Server name")
    if body.auth == "sql" and not _clean(body.user, "Login"):
        raise HTTPException(422, "Enter the SQL login name")
    password = secure_fields.decrypt(body.password, "db") if body.password else ""
    return host, body.port.strip(), body.user.strip() if body.auth == "sql" else "", password or "", body.auth


@router.get("/status")
def status():
    if db.configured() and not sqlstore.enabled():
        sqlstore.writes_blocked()   # set up but offline: try to come back (at most every 20 s) and load the data
    return {"configured": db.configured(), "connected": db.is_db_available(), "needs_setup": boot.needs_setup(),
            "needs_admin": boot.needs_admin()}


@router.post("/test")
def test(body: ServerIn, request: Request):
    _guard(request)
    host, port, user, password, auth_kind = _server(body)
    return db.test_connection(host, port, "", user, password, auth_kind)


@router.post("/connect")
def connect(body: ConnectIn, request: Request):
    _guard(request)
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
    return {"database": info["database"], "edition": info["edition"], "ready": setup_db.is_ready()}


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
