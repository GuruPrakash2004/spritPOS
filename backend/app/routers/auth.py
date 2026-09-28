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
import secrets
import threading
import time
from collections import defaultdict, deque
from datetime import datetime, timedelta, timezone
from typing import Optional

import jwt
from fastapi import APIRouter, Depends, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer

from .. import store
from ..passwords import hash_password, password_problem, verify_password
from ..schemas import ChangePasswordIn, LoginIn, RoleIn, UserIn

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
    ("GET", "/masters/company"),                       # company name on the sign-in page
    ("GET", "/company/files/{file_id}/content"),       # company images in <img> tags
}


def require_login(
    request: Request,
    auth_header: Optional[HTTPAuthorizationCredentials] = Depends(security_bearer),
) -> None:
    """App-wide dependency (main.py): every route except PUBLIC_ROUTES needs a valid token."""
    route = request.scope.get("route")
    if (request.method, getattr(route, "path", request.url.path)) in PUBLIC_ROUTES:
        return
    # Kept on the request so the write middleware (main.py) can log who made each change.
    request.state.user = get_current_user(auth_header)["user"]


def can_use(user: dict, *modules: str) -> bool:
    """True if the user's role may open any of these modules ("*" = every module)."""
    allowed = store.ROLES.get(user.get("role", ""), [])
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
    rights = store.ROLE_RIGHTS.get(role, {})
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


def _public(u: dict) -> dict:
    return {"user": u["user"], "role": u["role"], "active": u.get("active", True)}


def _set_password(u: dict, password: str) -> None:
    """Store a new password hash, forget any legacy password, and invalidate older tokens."""
    u["password_hash"] = hash_password(password)
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
    default_ok = bool(u.get("default_password")) and secrets.compare_digest(u["user"].lower().encode(), entered.encode())
    if legacy_ok or default_ok:
        u["password_hash"] = hash_password(entered)   # upgrade: from now on only the hash is kept
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


@router.post("/auth/login")
def login(body: LoginIn, request: Request):
    """Authenticate user and issue a signed JWT token."""
    username = body.user.strip().upper()
    address = request.client.host if request.client else "unknown"
    user_key, address_key = f"user:{username}", f"addr:{address}"
    _check_not_locked([(user_key, MAX_FAILS_PER_USER), (address_key, MAX_FAILS_PER_ADDRESS)])

    u = store.USERS.get(username)
    if not u or not _verify_password(u, body.password):
        _record_failure([user_key, address_key])
        raise HTTPException(401, "Invalid user name or password")
    if not u.get("active", True):
        raise HTTPException(403, "This user is inactive. Ask the administrator.")
    with _failures_lock:
        _failures.pop(user_key, None)

    role = u.get("role", "Cashier")
    token = create_access_token({"sub": u["user"], "role": role, "pwv": u.get("pwv", 1)})
    return {
        "user": u["user"],
        "role": role,
        "modules": store.ROLES.get(role, []),
        "rights": rights_for(role),
        "company": store.COMPANY,
        "access_token": token,
        "token": token,
        "token_type": "bearer",
    }


@router.post("/auth/change-password")
def change_password(body: ChangePasswordIn, request: Request, current_user: dict = Depends(get_current_user)):
    """Any logged-in user changes their own password. Needs the current password (and counts wrong
    ones like failed logins). Returns a new token: the user's other sessions are signed out."""
    name = current_user["user"]
    address = request.client.host if request.client else "unknown"
    user_key, address_key = f"user:{name}", f"addr:{address}"
    _check_not_locked([(user_key, MAX_FAILS_PER_USER), (address_key, MAX_FAILS_PER_ADDRESS)])
    if not _verify_password(current_user, body.current_password):
        _record_failure([user_key, address_key])
        raise HTTPException(422, "Your current password is not correct")
    if body.new_password == body.current_password:
        raise HTTPException(422, "Choose a password different from the current one")
    _check_new_password(body.new_password, name)
    _set_password(current_user, body.new_password)
    store.touch("USERS", name)
    token = create_access_token({"sub": name, "role": current_user.get("role"), "pwv": current_user["pwv"]})
    return {"access_token": token, "token": token, "token_type": "bearer"}


@router.get("/auth/me")
def me(current_user: dict = Depends(get_current_user)):
    """Return identity and assigned modules for the token holder."""
    role = current_user.get("role", "Cashier")
    return {
        "user": current_user["user"],
        "role": role,
        "modules": store.ROLES.get(role, []),
        "rights": rights_for(role),
        "company": store.COMPANY,
    }


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
    problem = password_problem(password, user)
    if problem:
        raise HTTPException(422, problem)


@admin.get("/users")
def list_users(current_admin: dict = Depends(require_admin)):
    return [_public(u) for u in store.USERS.values()]


@admin.post("/users", status_code=201)
def create_user(body: UserIn, current_admin: dict = Depends(require_admin)):
    name = body.user.strip().upper()
    if name in store.USERS:
        raise HTTPException(409, f"User '{name}' already exists")
    if body.role not in store.ROLES:
        raise HTTPException(422, f"Unknown role '{body.role}'")
    if not body.password:
        raise HTTPException(422, "Enter a password for the new user")
    _check_new_password(body.password, name)
    u = {"user": name, "role": body.role, "active": body.active, "pwv": 0}
    _set_password(u, body.password)
    store.USERS[name] = u
    store.touch("USERS", name)
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
    if body.password:
        _check_new_password(body.password, u["user"])
        _set_password(u, body.password)
    if u.get("active", True) and not body.active:
        u["pwv"] = u.get("pwv", 1) + 1   # sign the user out everywhere
    u.update(role=body.role, active=body.active)
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
    store.touch("USERS", u["user"])


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
                store.touch("USERS", u["user"])
        del store.ROLES[name]
        store.touch("ROLES", name)
        store.ROLE_RIGHTS.pop(name, None)
        store.touch("ROLE_RIGHTS", name)
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
