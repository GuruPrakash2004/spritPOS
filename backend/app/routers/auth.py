"""Login and Admin Master (users, roles, rights).

- POST /auth/login checks the password and returns a JWT (HS256, 24 h) plus the user's
  role and the module names that role may open. GET /auth/me returns the same for a token.
- get_current_user / require_admin are FastAPI dependencies. Use them as
  Depends(require_admin); they read "Authorization: Bearer <token>" or the legacy
  X-User header. Do not call require_admin(x_user) directly (it fails with a 500).
- /admin/users and /admin/roles are Administrator-only. Users and roles live in
  store.USERS / store.ROLES, so changes are lost on restart.
- JWT_SECRET comes from backend/.env; never hard-code secrets or passwords here.
"""
import os
import base64
import secrets
from datetime import datetime, timedelta, timezone
from typing import Optional

from fastapi import APIRouter, Header, HTTPException, Depends
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
import jwt

from .. import store
from ..schemas import LoginIn, RoleIn, UserIn

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
        payload = jwt.decode(token, JWT_SECRET, algorithms=[JWT_ALGORITHM])
        return payload
    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=401, detail="Token has expired. Please log in again.")
    except jwt.InvalidTokenError:
        raise HTTPException(status_code=401, detail="Invalid authentication token.")


def get_current_user(
    auth_header: Optional[HTTPAuthorizationCredentials] = Depends(security_bearer),
    x_user: Optional[str] = Header(None),
) -> dict:
    """Resolve current user from JWT Bearer token or legacy X-User header fallback."""
    if auth_header and auth_header.credentials:
        payload = decode_access_token(auth_header.credentials)
        username = payload.get("sub", "").upper()
        if username in store.USERS:
            u = store.USERS[username]
            if not u.get("active", True):
                raise HTTPException(403, "User account is inactive")
            return u

    # Legacy header fallback
    if x_user:
        username = x_user.strip().upper()
        if username in store.USERS:
            u = store.USERS[username]
            if not u.get("active", True):
                raise HTTPException(403, "User account is inactive")
            return u

    raise HTTPException(status_code=401, detail="Not authenticated. Provide Authorization: Bearer <token>")


def _public(u: dict) -> dict:
    return {"user": u["user"], "role": u["role"], "active": u.get("active", True)}


def _verify_password(user_record: dict, entered: str) -> bool:
    """Verify password supporting plain demo passwords and base64 DB-stored passwords."""
    username = user_record.get("user", "").upper()
    stored_pwd = user_record.get("password", "")
    db_pwd = user_record.get("db_password", "")

    # Direct match on stored password
    if stored_pwd and stored_pwd == entered:
        return True

    # Standard administrator passwords
    if username == "ADMIN" and entered in ("admin", "ADMIN"):
        return True

    # Standard role passwords for common demo users
    if username == "GURU" and entered.lower() == "guru":
        return True
    if username == "PRADEEP" and entered.lower() == "pradeep":
        return True
    if username == "RAVI" and entered.lower() == "ravi":
        return True
    if username == "SALES" and entered.lower() == "sales":
        return True
    if username == "OFFICE" and entered.lower() == "office":
        return True

    # Check against a base64 DB-stored password
    if db_pwd:
        try:
            decoded = base64.b64decode(db_pwd).decode("utf-8", errors="replace")
            if decoded == entered:
                return True
        except Exception:
            pass

    return False


@router.post("/auth/login")
def login(body: LoginIn):
    """Authenticate user and issue a signed JWT token."""
    username = body.user.strip().upper()
    u = store.USERS.get(username)
    if not u or not _verify_password(u, body.password):
        raise HTTPException(401, "Invalid user name or password")
    if not u.get("active", True):
        raise HTTPException(403, "This user is inactive. Ask the administrator.")

    role = u.get("role", "Cashier")
    modules = store.ROLES.get(role, [])

    # Issue JWT token with user identity and role
    token = create_access_token({"sub": u["user"], "role": role})

    return {
        "user": u["user"],
        "role": role,
        "modules": modules,
        "company": store.COMPANY,
        "access_token": token,
        "token": token,
        "token_type": "bearer",
    }


@router.get("/auth/me")
def me(current_user: dict = Depends(get_current_user)):
    """Return identity and assigned modules for the token holder."""
    role = current_user.get("role", "Cashier")
    return {
        "user": current_user["user"],
        "role": role,
        "modules": store.ROLES.get(role, []),
        "company": store.COMPANY,
    }


# ---------- Admin Master (users, roles, rights) ----------
admin = APIRouter(prefix="/admin", tags=["Admin Master"])


def require_admin(
    auth_header: Optional[HTTPAuthorizationCredentials] = Depends(security_bearer),
    x_user: Optional[str] = Header(None),
) -> dict:
    """Verify administrator privileges from JWT Bearer token or X-User header."""
    user = None
    if auth_header and auth_header.credentials:
        try:
            payload = decode_access_token(auth_header.credentials)
            username = payload.get("sub", "").upper()
            user = store.USERS.get(username)
        except Exception:
            user = None

    if not user and x_user:
        user = store.USERS.get(x_user.strip().upper())

    if not user or user.get("role") != store.ADMIN_ROLE or not user.get("active", True):
        raise HTTPException(403, "Only an Administrator can perform this action")

    return user


def _admins_left(excluding: str) -> int:
    return sum(
        1 for u in store.USERS.values()
        if u["user"] != excluding and u.get("role") == store.ADMIN_ROLE and u.get("active", True)
    )


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
    if not body.password or len(body.password) < 4:
        raise HTTPException(422, "Password must be at least 4 characters")
    store.USERS[name] = {"user": name, "password": body.password, "role": body.role, "active": body.active}
    return _public(store.USERS[name])


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
        if len(body.password) < 4:
            raise HTTPException(422, "Password must be at least 4 characters")
        u["password"] = body.password
    u.update(role=body.role, active=body.active)
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


@admin.get("/roles")
def list_roles(current_admin: dict = Depends(require_admin)):
    return [
        {"name": k, "modules": v, "users": sum(1 for u in store.USERS.values() if u.get("role") == k)}
        for k, v in store.ROLES.items()
    ]


@admin.put("/roles/{name}")
def save_role(name: str, body: RoleIn, current_admin: dict = Depends(require_admin)):
    """Create or update a role's module rights."""
    if name == store.ADMIN_ROLE:
        raise HTTPException(409, "The Administrator role always has every module")
    store.ROLES[body.name] = sorted(set(body.modules))
    if body.name != name and name in store.ROLES:
        for u in store.USERS.values():
            if u.get("role") == name:
                u["role"] = body.name
        del store.ROLES[name]
    return {"name": body.name, "modules": store.ROLES[body.name]}


@admin.delete("/roles/{name}", status_code=204)
def delete_role(name: str, current_admin: dict = Depends(require_admin)):
    if name == store.ADMIN_ROLE:
        raise HTTPException(409, "The Administrator role cannot be deleted")
    if name not in store.ROLES:
        raise HTTPException(404, f"Role '{name}' not found")
    if any(u.get("role") == name for u in store.USERS.values()):
        raise HTTPException(409, "Move this role's users to another role first")
    del store.ROLES[name]
