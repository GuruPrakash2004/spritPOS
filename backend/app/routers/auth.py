"""Login and Admin Master (users, roles, rights).

- POST /auth/login checks the password and returns a JWT (HS256, 24 h) plus the user's
  role and the module names that role may open. GET /auth/me returns the same for a token.
- Passwords are stored only as salted PBKDF2 hashes (app/passwords.py). Users loaded from the
  legacy MstUser table get their password hashed on their first successful login.
- Brute force: 5 wrong passwords for a user name, or 20 from one address, within 15 minutes
  lock that name/address out for the rest of the window (HTTP 429).
- Tokens carry the user's password version ("pwv"): changing a password or deactivating a user
  makes their older tokens stop working.
- get_current_user / require_admin / require_module(...) are FastAPI dependencies: use
  Depends(...), never call them directly. They read "Authorization: Bearer <token>" only.
- require_login is applied to the whole app in main.py: every endpoint needs a valid token except
  the few in PUBLIC_ROUTES that the sign-in page (or an <img> tag, which can't send a token) uses.
- require_module("Sales Invoice", ...) guards the write endpoints: the caller's role must have one
  of those modules (the same names as the sidebar, store.ROLES and frontend lib/modules.ts).
- /admin/users and /admin/roles are Administrator-only. Users and roles live in
  store.USERS / store.ROLES, so changes are lost on restart.
- JWT_SECRET comes from backend/.env; never hard-code secrets or passwords here.
"""
import base64
import binascii
import os
import re
import secrets
import threading
import time
from collections import defaultdict, deque
from datetime import date, datetime, timedelta, timezone
from typing import Optional

import jwt
from fastapi import APIRouter, Depends, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer

from .. import admin_audit, db, secure_fields, security, sqlstore, store
from ..passwords import hash_password, verify_password
from ..schemas import (
    AdnlAdminPasswordIn, ChangePasswordIn, FieldRightsIn, LoginIn, LoginSettingsIn, MasterApprovalIn, ReportRightsIn, RoleIn,
    UserIn,
)

router = APIRouter(tags=["Login"])

# ─── JWT Settings ────────────────────────────────────────────────────────────
# Without JWT_SECRET/SECRET_KEY in backend/.env, a random per-process secret is used,
# so tokens stop working when the server restarts.
JWT_SECRET = os.environ.get("JWT_SECRET") or os.environ.get("SECRET_KEY") or secrets.token_urlsafe(64)
JWT_ALGORITHM = "HS256"
JWT_EXPIRATION_HOURS = 24

security_bearer = HTTPBearer(auto_error=False)


def create_access_token(data: dict, expires_delta: Optional[timedelta] = None) -> str:
    """Create a signed JWT access token."""
    to_encode = data.copy()
    now = datetime.now(timezone.utc)
    expire = now + (expires_delta or timedelta(hours=JWT_EXPIRATION_HOURS))
    to_encode.update({"exp": expire, "iat": now})
    return jwt.encode(to_encode, JWT_SECRET, algorithm=JWT_ALGORITHM)


def decode_access_token(token: str) -> dict:
    """Decode and validate a JWT access token."""
    try:
        return jwt.decode(token, JWT_SECRET, algorithms=[JWT_ALGORITHM])
    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=401, detail="Your session has expired. Please log in again.")
    except jwt.InvalidTokenError:
        raise HTTPException(status_code=401, detail="Invalid login token. Please log in again.")


def get_current_user(
    auth_header: Optional[HTTPAuthorizationCredentials] = Depends(security_bearer),
) -> dict:
    """The logged-in user from the JWT Bearer token (401 without a valid, current token)."""
    if not (auth_header and auth_header.credentials):
        raise HTTPException(status_code=401, detail="Not logged in. Please log in again.")
    payload = decode_access_token(auth_header.credentials)
    u = store.USERS.get(str(payload.get("sub", "")).upper())
    if not u:
        raise HTTPException(status_code=401, detail="This user no longer exists. Please log in again.")
    if "pwv" not in payload:   # issued before password versions existed
        raise HTTPException(status_code=401, detail="The app was updated. Please log in again.")
    if payload.get("pwv") != u.get("pwv", 1):
        raise HTTPException(status_code=401, detail="Your password or access was changed. Please log in again.")
    if not u.get("active", True):
        raise HTTPException(status_code=401, detail="This user is inactive. Ask the administrator.")
    return u


# Endpoints that work without logging in, as (method, route path).
PUBLIC_ROUTES = {
    ("GET", "/"),                                      # health check
    ("POST", "/auth/login"),
    ("GET", "/auth/transport-key"),                    # public key the sign-in page encrypts the password with
    ("GET", "/auth/login-settings"),                   # password rules and "Remember me" for the sign-in page
    ("GET", "/masters/company"),                       # company name on the sign-in page
    ("GET", "/company/files/{file_id}/content"),       # company images in <img> tags
    # One-time Database setup on the sign-in page: they check the setup code (or an Administrator's token
    # once the database is ready) themselves, in routers/setup.py.
    ("GET", "/setup/status"), ("GET", "/setup/progress"),
    ("POST", "/setup/test"), ("POST", "/setup/connect"), ("POST", "/setup/run"), ("POST", "/setup/admin"),
}


def require_login(
    request: Request,
    auth_header: Optional[HTTPAuthorizationCredentials] = Depends(security_bearer),
) -> None:
    """App-wide dependency (main.py): every route except PUBLIC_ROUTES needs a valid token."""
    route = request.scope.get("route")
    if (request.method, getattr(route, "path", request.url.path)) in PUBLIC_ROUTES:
        return
    user = get_current_user(auth_header)
    # Kept on the request so the write middleware (main.py) can log who made each change.
    request.state.user = user["user"]
    # An expired password (Login Settings expiry, or the user's Password Reset date) must be changed first.
    if security.password_expired(user) and (request.method, getattr(route, "path", "")) not in EXPIRED_ALLOWED:
        raise HTTPException(403, "Your password has expired. Change it to continue.")


# What a user whose password has expired may still do.
EXPIRED_ALLOWED = {("POST", "/auth/change-password"), ("GET", "/auth/me"), ("GET", "/settings/values"),
                   ("GET", "/utilities/about")}


def can_use(user: dict, *modules: str) -> bool:
    """True if the user's role may open any of these modules ("*" = every module)."""
    allowed = security.modules_for(user)
    return "*" in allowed or any(m in allowed for m in modules)


def require_module(*modules: str):
    """Dependency factory: the caller's role must include one of `modules`, else 403."""
    def check(current_user: dict = Depends(get_current_user)) -> dict:
        if not can_use(current_user, *modules):
            raise HTTPException(403, f"Your role ({current_user.get('role')}) is not allowed to use {' / '.join(modules)}")
        return current_user
    return check


ACTION_WORDS = {"add": "add in", "edit": "change", "delete": "delete in"}


def has_right(user: dict, action: str, *modules: str) -> bool:
    """True if the user's role may open one of `modules` AND has the Add/Edit/Delete tick for it there
    (store.ROLE_RIGHTS; a module with no saved ticks keeps full rights)."""
    role = user.get("role", "")
    if role == store.ADMIN_ROLE:
        return True
    rights = security.rights_for_user(user)
    return any(can_use(user, m) and rights.get(m, {}).get(action, True) for m in modules)


def require_right(action: str, *modules: str):
    """Dependency factory: like require_module, plus the role's Add/Edit/Delete tick for `action`."""
    def check(current_user: dict = Depends(get_current_user)) -> dict:
        if not can_use(current_user, *modules):
            raise HTTPException(403, f"Your role ({current_user.get('role')}) is not allowed to use {' / '.join(modules)}")
        if not has_right(current_user, action, *modules):
            raise HTTPException(403, f"Your role ({current_user.get('role')}) is not allowed to {ACTION_WORDS[action]} {' / '.join(modules)}")
        return current_user
    return check


# Roles with any of these modules may see purchase costs (purchase rate, stock value at cost, profit).
COST_MODULES = ("Purchase Bill", "Purchase Indent", "Profit Margin", "Price List Maintenance", "Stock Ageing",
                "Reports Dashboard")


def can_see_cost(user: dict) -> bool:
    return can_use(user, *COST_MODULES)


def hides_cost(user) -> bool:
    """True when a route was called by a user who must not see costs. Report functions are also called
    internally (the DSR calls stock_balance), where `user` is not a dict: nothing is hidden then."""
    return isinstance(user, dict) and not can_see_cost(user)


def rights_for(role: str) -> dict:
    """The role's saved Add/Edit/Delete ticks, for the frontend (Administrator: none, meaning all)."""
    return {} if role == store.ADMIN_ROLE else store.ROLE_RIGHTS.get(role, {})


USER_FIELDS = ("full_name", "email", "mobile", "department", "dob", "password_reset_on", "inactive_reason")


def _public(u: dict) -> dict:
    return {"user": u["user"], "role": u["role"], "active": u.get("active", True),
            **{k: u.get(k) or ("" if k not in ("dob", "password_reset_on") else None) for k in USER_FIELDS},
            "password_expired": security.password_expired(u)}


def _session(u: dict) -> dict:
    """What the screens need about the signed-in user: modules, Add/Edit/Delete rights, field rights."""
    role = u.get("role", "Cashier")
    return {"user": u["user"], "role": role, "modules": security.modules_for(u), "rights": security.rights_for_user(u),
            "field_rights": {} if role == store.ADMIN_ROLE else store.FIELD_RIGHTS.get(role, {}),
            "must_change_password": security.password_expired(u), "company": store.COMPANY,
            "data_mode": sqlstore.data_mode()}


def _set_password(u: dict, password: str) -> None:
    """Store a new password hash, forget any legacy password, and invalidate older tokens."""
    u["password_hash"] = hash_password(password)
    u["password_set_on"] = date.today().isoformat()
    u.pop("legacy_password", None)
    u.pop("default_password", None)
    u["pwv"] = u.get("pwv", 1) + 1


def _verify_password(u: dict, entered: str) -> bool:
    """Check a login password. Users from MstUser are checked against their legacy password (or the
    lowercase user code) once, then that password is hashed and the legacy value dropped."""
    if u.get("password_hash"):
        return verify_password(entered, u["password_hash"])
    legacy_ok = False
    if u.get("legacy_password"):
        try:
            legacy_ok = secrets.compare_digest(base64.b64decode(u["legacy_password"]), entered.encode())
        except (binascii.Error, ValueError):
            legacy_ok = False
    # The lowercase user code is never a password for an Administrator (it would be ADMIN/admin all over again).
    default_ok = (bool(u.get("default_password")) and u.get("role") != store.ADMIN_ROLE
                  and secrets.compare_digest(u["user"].lower().encode(), entered.encode()))
    if legacy_ok or default_ok:
        u["password_hash"] = hash_password(entered)   # upgrade: from now on only the hash is kept
        u["password_set_on"] = date.today().isoformat()
        u.pop("legacy_password", None)
        u.pop("default_password", None)
        store.touch("USERS", u["user"])
        return True
    return False


# ─── Login rate limiting ─────────────────────────────────────────────────────
LOCK_WINDOW_SECONDS = 15 * 60
MAX_FAILS_PER_USER = 5
MAX_FAILS_PER_ADDRESS = 20
_failures: dict[str, deque] = defaultdict(deque)
_failures_lock = threading.Lock()


def _recent(key: str, now: float) -> deque:
    q = _failures[key]
    while q and now - q[0] > LOCK_WINDOW_SECONDS:
        q.popleft()
    return q


def _check_not_locked(keys: list[tuple[str, int]]) -> None:
    now = time.monotonic()
    with _failures_lock:
        for key, limit in keys:
            q = _recent(key, now)
            if len(q) >= limit:
                wait = int(LOCK_WINDOW_SECONDS - (now - q[0])) // 60 + 1
                raise HTTPException(429, f"Too many failed logins. Try again in {wait} minute{'s' if wait != 1 else ''}.")


def _record_failure(keys: list[str]) -> None:
    now = time.monotonic()
    with _failures_lock:
        for key in keys:
            _recent(key, now).append(now)


def reset_login_failures() -> None:
    with _failures_lock:
        _failures.clear()


@router.get("/auth/transport-key")
def transport_key():
    """Public RSA key (SPKI, base64) that the browser encrypts every password field with (RSA-OAEP-256).
    Public: the sign-in page needs it before anyone is logged in."""
    return secure_fields.public_key()


@router.post("/auth/login")
def login(body: LoginIn, request: Request):
    """Authenticate user and issue a signed JWT token."""
    if db.configured() and not (sqlstore.enabled() or sqlstore.test_data()):
        raise HTTPException(503, "The database isn't set up yet, or it can't be reached. Sign in once it is back.")
    username = body.user.strip().upper()
    password = secure_fields.decrypt(body.password, "login")
    address = request.client.host if request.client else "unknown"
    user_key, address_key = f"user:{username}", f"addr:{address}"
    _check_not_locked([(user_key, MAX_FAILS_PER_USER), (address_key, MAX_FAILS_PER_ADDRESS)])

    u = store.USERS.get(username)
    if not u or not _verify_password(u, password):
        _record_failure([user_key, address_key])
        raise HTTPException(401, "Invalid user name or password")
    if not u.get("active", True):
        raise HTTPException(403, "This user is inactive. Ask the administrator.")
    with _failures_lock:
        _failures.pop(user_key, None)

    token = create_access_token({"sub": u["user"], "role": u.get("role", "Cashier"), "pwv": u.get("pwv", 1)})
    return {**_session(u), "access_token": token, "token": token, "token_type": "bearer"}


@router.post("/auth/change-password")
def change_password(body: ChangePasswordIn, request: Request, current_user: dict = Depends(get_current_user)):
    """Any logged-in user changes their own password. Needs the current password (and counts wrong
    ones like failed logins). Returns a new token: the user's other sessions are signed out."""
    name = current_user["user"]
    current = secure_fields.decrypt(body.current_password, "current", "Current password")
    new = secure_fields.decrypt(body.new_password, "new", "New password")
    address = request.client.host if request.client else "unknown"
    user_key, address_key = f"user:{name}", f"addr:{address}"
    _check_not_locked([(user_key, MAX_FAILS_PER_USER), (address_key, MAX_FAILS_PER_ADDRESS)])
    if not _verify_password(current_user, current):
        _record_failure([user_key, address_key])
        raise HTTPException(422, "Your current password is not correct")
    if new == current:
        raise HTTPException(422, "Choose a password different from the current one")
    _check_new_password(new, name)
    _set_password(current_user, new)
    store.touch("USERS", name)
    admin_audit.record("Admin Master (Security)", "Password changed", name, "", "own password changed")
    token = create_access_token({"sub": name, "role": current_user.get("role"), "pwv": current_user["pwv"]})
    return {"access_token": token, "token": token, "token_type": "bearer"}


@router.get("/auth/me")
def me(current_user: dict = Depends(get_current_user)):
    """Identity, modules, Add/Edit/Delete and field rights for the token holder (from their role)."""
    return _session(current_user)


# ---------- Admin Master (users, roles, rights) ----------
admin = APIRouter(prefix="/admin", tags=["Admin Master"])


def require_admin(current_user: dict = Depends(get_current_user)) -> dict:
    """The logged-in user, if they are an active Administrator (403 otherwise)."""
    if current_user.get("role") != store.ADMIN_ROLE:
        raise HTTPException(403, "Only an Administrator can perform this action")
    return current_user


def _admins_left(excluding: str) -> int:
    return sum(
        1 for u in store.USERS.values()
        if u["user"] != excluding and u.get("role") == store.ADMIN_ROLE and u.get("active", True)
    )


def _check_new_password(password: str, user: str) -> None:
    problem = security.password_problem(password, user)   # Login Settings rules
    if problem:
        raise HTTPException(422, problem)


@admin.get("/users")
def list_users(current_admin: dict = Depends(require_admin)):
    return [_public(u) for u in store.USERS.values()]


EMAIL_RE = re.compile(r"^[^@\s]+@[^@\s]+\.[^@\s]+$")


def _user_details(body: UserIn) -> dict:
    """The legacy user-master fields, checked (EmailID is not valid!!, Reason for InActive!)."""
    email = body.email.strip()
    if email and not EMAIL_RE.match(email):
        raise HTTPException(422, "EmailID is not valid!!")
    if not body.active and not body.inactive_reason.strip():
        raise HTTPException(422, "Reason for InActive! Enter why this user is inactive")
    if body.dob and body.dob > date.today():
        raise HTTPException(422, "Not a valid DOB")
    return {"full_name": body.full_name.strip(), "email": email, "mobile": body.mobile.strip(),
            "department": body.department.strip(), "dob": body.dob.isoformat() if body.dob else None,
            "password_reset_on": body.password_reset_on.isoformat() if body.password_reset_on else None,
            "inactive_reason": "" if body.active else body.inactive_reason.strip()}


@admin.post("/users", status_code=201)
def create_user(body: UserIn, current_admin: dict = Depends(require_admin)):
    name = body.user.strip().upper()
    if name in store.USERS:
        raise HTTPException(409, f"User '{name}' already exists")
    if body.role not in store.ROLES:
        raise HTTPException(422, f"Unknown role '{body.role}'")
    password = secure_fields.decrypt(body.password, "user")
    if not password:
        raise HTTPException(422, "Enter a password for the new user")
    _check_new_password(password, name)
    u = {"user": name, "role": body.role, "role_set_in_web": True, "active": body.active, "pwv": 0, **_user_details(body)}
    _set_password(u, password)
    store.USERS[name] = u
    store.touch("USERS", name)
    admin_audit.record("Admin Master (Security)", "User added", name, "", f"role: {body.role}; active: {'Yes' if body.active else 'No'}")
    return _public(u)


@admin.put("/users/{name}")
def update_user(name: str, body: UserIn, current_admin: dict = Depends(require_admin)):
    u = store.USERS.get(name.upper())
    if not u:
        raise HTTPException(404, f"User '{name}' not found")
    if body.role not in store.ROLES:
        raise HTTPException(422, f"Unknown role '{body.role}'")
    demoting = u.get("role") == store.ADMIN_ROLE and (body.role != store.ADMIN_ROLE or not body.active)
    if demoting and _admins_left(u["user"]) == 0:
        raise HTTPException(409, "At least one active Administrator must remain")
    details = _user_details(body)
    password = secure_fields.decrypt(body.password, "user")
    if password:
        _check_new_password(password, u["user"])
        _set_password(u, password)
    if u.get("active", True) and not body.active:
        u["pwv"] = u.get("pwv", 1) + 1   # sign the user out everywhere
    earlier, changed = admin_audit.diff({"role": u.get("role"), "active": u.get("active", True),
                                         **{k: u.get(k) for k in details}},
                                        {"role": body.role, "active": body.active, **details})
    if password:
        earlier, changed = (earlier + "; " if earlier else "") + "password: (old)", (changed + "; " if changed else "") + "password: (changed)"
    if changed:
        admin_audit.record("Admin Master (Security)", "User edited", u["user"], earlier, changed)
    u.update(role=body.role, role_set_in_web=True, active=body.active, **details)
    store.touch("USERS", u["user"])
    return _public(u)


@admin.delete("/users/{name}", status_code=204)
def delete_user(name: str, current_admin: dict = Depends(require_admin)):
    u = store.USERS.get(name.upper())
    if not u:
        raise HTTPException(404, f"User '{name}' not found")
    if u["user"] == current_admin["user"].upper():
        raise HTTPException(409, "You cannot delete the user you are logged in as")
    if u.get("role") == store.ADMIN_ROLE and _admins_left(u["user"]) == 0:
        raise HTTPException(409, "At least one active Administrator must remain")
    del store.USERS[u["user"]]
    for users in store.REPORT_RIGHTS.values():   # a later user with the same name gets no old report access
        if u["user"] in users:
            users.remove(u["user"])
    store.touch("USERS", u["user"])
    admin_audit.record("Admin Master (Security)", "User deleted", u["user"], f"role: {u.get('role')}", "")


@admin.get("/roles")
def list_roles(current_admin: dict = Depends(require_admin)):
    return [
        {"name": k, "modules": v, "rights": rights_for(k),
         "users": sum(1 for u in store.USERS.values() if u.get("role") == k)}
        for k, v in store.ROLES.items()
    ]


@admin.put("/roles/{name}")
def save_role(name: str, body: RoleIn, current_admin: dict = Depends(require_admin)):
    """Create or update a role's module rights (renaming it when body.name differs from `name`)."""
    new_name = body.name.strip()
    if store.ADMIN_ROLE in (name, new_name):
        raise HTTPException(409, "The Administrator role always has every module and cannot be changed")
    if new_name != name and new_name in store.ROLES:
        raise HTTPException(409, f"A role named '{new_name}' already exists")
    modules = sorted({m.strip() for m in body.modules if m.strip()})
    if not modules:
        raise HTTPException(422, "Give the role at least one screen")
    if "*" in modules:
        raise HTTPException(422, "Only the Administrator role can have every module")
    old_modules = store.ROLES.get(name, [])
    added, removed = sorted(set(modules) - set(old_modules)), sorted(set(old_modules) - set(modules))
    if added or removed or new_name != name:
        admin_audit.record("Admin Master (Security)", "Role saved", new_name if new_name == name else f"{name} → {new_name}",
                           f"removed: {', '.join(removed)}" if removed else "",
                           f"added: {', '.join(added)}" if added else "")
    store.ROLES[new_name] = modules
    store.touch("ROLES", new_name)
    # Keep ticks only for modules the role has, and only where something is switched off.
    store.ROLE_RIGHTS[new_name] = {m: r.model_dump() for m, r in body.rights.items()
                                   if m in modules and not all(r.model_dump().values())}
    store.touch("ROLE_RIGHTS", new_name)
    if new_name != name and name in store.ROLES:
        for u in store.USERS.values():
            if u.get("role") == name:
                u["role"] = new_name
                u["role_set_in_web"] = True
                store.touch("USERS", u["user"])
        del store.ROLES[name]
        store.touch("ROLES", name)
        store.ROLE_RIGHTS.pop(name, None)
        store.touch("ROLE_RIGHTS", name)
        if name in store.FIELD_RIGHTS:   # field rights follow the renamed role
            store.FIELD_RIGHTS[new_name] = store.FIELD_RIGHTS.pop(name)
    return {"name": new_name, "modules": store.ROLES[new_name], "rights": rights_for(new_name)}


@admin.delete("/roles/{name}", status_code=204)
def delete_role(name: str, current_admin: dict = Depends(require_admin)):
    if name == store.ADMIN_ROLE:
        raise HTTPException(409, "The Administrator role cannot be deleted")
    if name not in store.ROLES:
        raise HTTPException(404, f"Role '{name}' not found")
    if any(u.get("role") == name for u in store.USERS.values()):
        raise HTTPException(409, "Move this role's users to another role first")
    del store.ROLES[name]
    store.touch("ROLES", name)
    store.ROLE_RIGHTS.pop(name, None)
    store.touch("ROLE_RIGHTS", name)
    store.FIELD_RIGHTS.pop(name, None)
    admin_audit.record("Admin Master (Security)", "Role deleted", name, "", "")


# ─── Admin Master (Security): login settings, rights, field rights, approvals ───

@router.get("/auth/login-settings")
def public_login_settings():
    """Password rules and whether "Remember me" is offered (public: the sign-in page and the change-password
    dialog show them). No secrets."""
    s = security.login_settings()
    return {k: s[k] for k in ("min_length", "require_mix", "require_upper", "require_lower", "require_digit",
                              "require_special", "password_expiry_days", "show_remember_me")}


@admin.get("/login-settings")
def get_login_settings(current_admin: dict = Depends(require_admin)):
    return security.login_settings()


@admin.put("/login-settings")
def save_login_settings(body: LoginSettingsIn, current_admin: dict = Depends(require_admin)):
    before = security.login_settings()
    store.SECURITY["login"] = body.model_dump()
    earlier, changed = admin_audit.diff(before, store.SECURITY["login"])
    if changed:
        admin_audit.record("Admin Master (Security)", "Login Settings saved", "Login Settings", earlier, changed)
    return security.login_settings()


@admin.get("/field-rights")
def get_field_rights(current_admin: dict = Depends(require_admin)):
    return {"masters": security.master_fields(), "roles": store.FIELD_RIGHTS, "captions": {security.DASHBOARD: security.DASHBOARD_PANELS}}


@admin.put("/field-rights/{role}")
def save_field_rights(role: str, body: FieldRightsIn, current_admin: dict = Depends(require_admin)):
    if role not in store.ROLES:
        raise HTTPException(404, f"Role '{role}' not found")
    if role == store.ADMIN_ROLE:
        raise HTTPException(409, "Administrators see and change every field")
    fields = security.master_fields()
    clean: dict[str, dict[str, str]] = {}
    for path, modes in body.masters.items():
        if path not in fields:
            raise HTTPException(422, f"Unknown master '{path}'")
        bad = [f for f in modes if f not in fields[path]]
        if bad:
            raise HTTPException(422, f"Unknown field(s) for {path}: {', '.join(bad)}")
        must = [f for f, mode in modes.items() if mode == "hidden" and f in security.required_fields()[path]]
        if must:
            raise HTTPException(422, f"{', '.join(must)} can't be hidden ({path} can't be saved without it); make it read-only instead")
        if path == security.DASHBOARD and any(mode != "hidden" for mode in modes.values()):
            raise HTTPException(422, "Dashboard panels can only be hidden (there is nothing to edit on them)")
        if modes:
            clean[path] = dict(modes)
    before = store.FIELD_RIGHTS.get(role, {})
    store.FIELD_RIGHTS[role] = clean
    earlier, changed = admin_audit.diff({p: str(v) for p, v in before.items()}, {p: str(v) for p, v in clean.items()})
    if changed:
        admin_audit.record("Admin Master (Security)", "Field rights saved", role, earlier, changed)
    return {"role": role, "masters": clean}


@admin.get("/master-approval")
def get_master_approval(current_admin: dict = Depends(require_admin)):
    """Masters that can need approval, which ones do, and the rows waiting."""
    pending = []
    for path in store.MASTER_APPROVAL.get("masters", []):
        table = getattr(store, security.TABLES[path])
        pending += [{"master": path, "screen": security.APPROVABLE[path], "code": code, "name": row.get("name", ""),
                     "changed_by": row.get("changed_by")}
                    for code, row in table.items() if row.get("authorised") is False]
    return {"masters": security.APPROVABLE, "required": store.MASTER_APPROVAL.get("masters", []), "pending": pending}


@admin.put("/master-approval")
def save_master_approval(body: MasterApprovalIn, current_admin: dict = Depends(require_admin)):
    bad = [m for m in body.masters if m not in security.APPROVABLE]
    if bad:
        raise HTTPException(422, f"Unknown master(s): {', '.join(bad)}")
    before = store.MASTER_APPROVAL.get("masters", [])
    store.MASTER_APPROVAL["masters"] = sorted(set(body.masters))
    earlier, changed = admin_audit.diff({"masters": ", ".join(before)}, {"masters": ", ".join(store.MASTER_APPROVAL["masters"])})
    if changed:
        admin_audit.record("Admin Master (Security)", "Master approvals saved", "Master Approval", earlier, changed)
    return get_master_approval(current_admin)


@admin.post("/master-approval/{path}/{code}")
def approve_master_row(path: str, code: str, current_admin: dict = Depends(require_admin)):
    if path not in security.APPROVABLE:
        raise HTTPException(404, f"Unknown master '{path}'")
    table = getattr(store, security.TABLES[path])
    row = table.get(code)
    if row is None:
        raise HTTPException(404, f"'{code}' not found")
    row.update(authorised=True, auth_by=current_admin["user"], auth_on=date.today().isoformat())
    store.touch(security.TABLES[path], code)
    admin_audit.record(security.APPROVABLE[path], "Approved", code, "authorised: No", "authorised: Yes")
    return {"master": path, "code": code, "authorised": True}


@admin.get("/report-rights")
def get_report_rights(current_admin: dict = Depends(require_admin)):
    from .custom_reports import REPORTS
    return {"reports": sorted(REPORTS), "rights": store.REPORT_RIGHTS,
            "users": [u["user"] for u in store.USERS.values() if u.get("role") != store.ADMIN_ROLE]}


@admin.put("/report-rights/{name}")
def save_report_rights(name: str, body: ReportRightsIn, current_admin: dict = Depends(require_admin)):
    from .custom_reports import REPORTS
    if name not in REPORTS:
        raise HTTPException(404, f"Report '{name}' not found")
    users = sorted({u.strip().upper() for u in body.users if u.strip()})
    unknown = [u for u in users if u not in store.USERS]
    if unknown:
        raise HTTPException(422, f"Unknown user(s): {', '.join(unknown)}")
    before = store.REPORT_RIGHTS.get(name, [])
    if users:
        store.REPORT_RIGHTS[name] = users
    else:
        store.REPORT_RIGHTS.pop(name, None)
    admin_audit.record("Admin Master (Security)", "Report rights saved", name,
                       ", ".join(before) or "everyone", ", ".join(users) or "everyone")
    return {"name": name, "users": users}


@admin.get("/security")
def security_state(current_admin: dict = Depends(require_admin)):
    return {"adnl_admin_set": security.adnl_admin_set()}


@admin.put("/adnl-admin-password")
def set_adnl_admin_password(body: AdnlAdminPasswordIn, current_admin: dict = Depends(require_admin)):
    """Change Adnl. ADMIN Password: set (or, with an empty new password, remove) the additional password asked
    before destructive utilities. Needs your own login password."""
    current = secure_fields.decrypt(body.current_password, "current", "Your password")
    key = f"user:{current_admin['user']}"
    _check_not_locked([(key, MAX_FAILS_PER_USER)])
    if not _verify_password(current_admin, current or ""):
        _record_failure([key])
        raise HTTPException(422, "Your password is not correct")
    new = secure_fields.decrypt(body.new_password, "new", "Additional ADMIN password") if body.new_password else ""
    if new:
        problem = security.password_problem(new, current_admin["user"])
        if problem:
            raise HTTPException(422, problem)
        if new == current:
            raise HTTPException(422, "Use a different password from your login password")
    security.set_adnl_admin(new or None)
    admin_audit.record("Admin Master (Security)", "Adnl. ADMIN password " + ("changed" if new else "removed"),
                       "Adnl. ADMIN password", "", "(set)" if new else "(removed)")
    return {"adnl_admin_set": security.adnl_admin_set()}

