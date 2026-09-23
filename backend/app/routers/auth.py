from fastapi import APIRouter, Header, HTTPException

from .. import store
from ..schemas import LoginIn, RoleIn, UserIn

router = APIRouter(tags=["Login"])


def _public(u: dict) -> dict:
    return {"user": u["user"], "role": u["role"], "active": u["active"]}


@router.post("/auth/login")
def login(body: LoginIn):
    """Demo login against sample users (ADMIN/admin, GURU/guru, PRADEEP/pradeep, RAVI/ravi).
    Returns the modules the user's role may open."""
    u = store.USERS.get(body.user.strip().upper())
    if not u or u["password"] != body.password:
        raise HTTPException(401, "Invalid user name or password")
    if not u["active"]:
        raise HTTPException(403, "This user is inactive. Ask the administrator.")
    return {"user": u["user"], "role": u["role"], "modules": store.ROLES.get(u["role"], []), "company": store.COMPANY}


# ---------- Admin Master (users, roles, rights) ----------
admin = APIRouter(prefix="/admin", tags=["Admin Master"])


def require_admin(x_user: str | None = Header(None)) -> None:
    # The prototype passes the logged-in user name as X-User; real auth would use a session token.
    u = store.USERS.get((x_user or "").upper())
    if not u or u["role"] != store.ADMIN_ROLE or not u["active"]:
        raise HTTPException(403, "Only an Administrator can open Admin Master")


def _admins_left(excluding: str) -> int:
    return sum(1 for u in store.USERS.values() if u["user"] != excluding and u["role"] == store.ADMIN_ROLE and u["active"])


@admin.get("/users")
def list_users(x_user: str | None = Header(None)):
    require_admin(x_user)
    return [_public(u) for u in store.USERS.values()]


@admin.post("/users", status_code=201)
def create_user(body: UserIn, x_user: str | None = Header(None)):
    require_admin(x_user)
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
def update_user(name: str, body: UserIn, x_user: str | None = Header(None)):
    require_admin(x_user)
    u = store.USERS.get(name.upper())
    if not u:
        raise HTTPException(404, f"User '{name}' not found")
    if body.role not in store.ROLES:
        raise HTTPException(422, f"Unknown role '{body.role}'")
    demoting = u["role"] == store.ADMIN_ROLE and (body.role != store.ADMIN_ROLE or not body.active)
    if demoting and _admins_left(u["user"]) == 0:
        raise HTTPException(409, "At least one active Administrator must remain")
    if body.password:
        if len(body.password) < 4:
            raise HTTPException(422, "Password must be at least 4 characters")
        u["password"] = body.password
    u.update(role=body.role, active=body.active)
    return _public(u)


@admin.delete("/users/{name}", status_code=204)
def delete_user(name: str, x_user: str | None = Header(None)):
    require_admin(x_user)
    u = store.USERS.get(name.upper())
    if not u:
        raise HTTPException(404, f"User '{name}' not found")
    if u["user"] == (x_user or "").upper():
        raise HTTPException(409, "You cannot delete the user you are logged in as")
    if u["role"] == store.ADMIN_ROLE and _admins_left(u["user"]) == 0:
        raise HTTPException(409, "At least one active Administrator must remain")
    del store.USERS[u["user"]]


@admin.get("/roles")
def list_roles(x_user: str | None = Header(None)):
    require_admin(x_user)
    return [{"name": k, "modules": v, "users": sum(1 for u in store.USERS.values() if u["role"] == k)} for k, v in store.ROLES.items()]


@admin.put("/roles/{name}")
def save_role(name: str, body: RoleIn, x_user: str | None = Header(None)):
    """Create or update a role's module rights (legacy Assign Rights)."""
    require_admin(x_user)
    if name == store.ADMIN_ROLE:
        raise HTTPException(409, "The Administrator role always has every module")
    store.ROLES[body.name] = sorted(set(body.modules))
    if body.name != name and name in store.ROLES:
        # Renamed: move users across, then drop the old name.
        for u in store.USERS.values():
            if u["role"] == name:
                u["role"] = body.name
        del store.ROLES[name]
    return {"name": body.name, "modules": store.ROLES[body.name]}


@admin.delete("/roles/{name}", status_code=204)
def delete_role(name: str, x_user: str | None = Header(None)):
    require_admin(x_user)
    if name == store.ADMIN_ROLE:
        raise HTTPException(409, "The Administrator role cannot be deleted")
    if name not in store.ROLES:
        raise HTTPException(404, f"Role '{name}' not found")
    if any(u["role"] == name for u in store.USERS.values()):
        raise HTTPException(409, "Move this role's users to another role first")
    del store.ROLES[name]
