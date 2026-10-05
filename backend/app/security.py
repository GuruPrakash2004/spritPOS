"""Admin Master (Security): the parts of the desktop Security screens beyond users and roles.

Legacy sources: frmMstUSer / frmMstRights (user master, Assign Rights, Copy From), frmUtlLoginSettings (rights
allocation, password rules and expiry, "Remember me"), frmMstFldRights (Master Field Rights), frmUtlMstApprovals
(Master Approval), frmUtlAssignRight (Customised Report rights) and UtlMisc.AdvancedADMLogin (Change Adnl.
ADMIN Password). Everything is enforced on the server; the screens only show it.

- modules_for(user) / rights_for_user(user): the user's role's screens and Add/Edit/Delete rights (role based
  allocation only; a user who needs different rights gets a role of their own).
- store.SECURITY["login"]: password rules (password_problem), expiry (password_expired) and show_remember_me.
- store.FIELD_RIGHTS[role][master path][field] = "hidden" | "readonly": hidden fields are left out of that
  role's master replies and kept as they are on save; changing a read-only field is refused.
- store.MASTER_APPROVAL["masters"]: masters whose new or changed rows wait for an Administrator's approval
  (row["authorised"] False); items, customers and suppliers waiting for approval can't be used on documents.
- store.REPORT_RIGHTS[report name] = [users]: who may run a Customised Report (empty/missing = everyone who can
  open Customised Reports).
- store.SECURITY["adnl_admin_hash"]: the additional ADMIN password asked before destructive utilities
  (check_admin_password); until it is set, the caller's own password is asked instead.
"""
from datetime import date, timedelta

from fastapi import HTTPException

from . import secure_fields, store
from .passwords import hash_password, verify_password

DEFAULT_LOGIN = {
    "min_length": 8, "require_mix": True, "require_upper": False, "require_lower": False, "require_digit": False,
    "require_special": False, "password_expiry_days": 0, "show_remember_me": False,
}

# Dashboard panels a role can be kept from seeing (Field Level Rights, document "dashboard"; only Hidden applies).
# The keys are the frontend's (Dashboard.tsx); restock and stock_movement are also refused by their endpoints.
DASHBOARD = "dashboard"
DASHBOARD_PANELS = {
    "hero": "Welcome banner", "kpis": "Key figures (today's sales, bills, stock)", "restock": "Restock immediately",
    "company_banner": "Company banner", "sales_chart": "Sales, last 7 days", "inventory_mix": "Inventory mix",
    "stock_movement": "Stock movement", "store_banner": "Store photo banner",
}


def dashboard_hidden(user: dict, panel: str) -> bool:
    return field_rights(user, DASHBOARD).get(panel) == "hidden"


def required_fields() -> dict[str, list[str]]:
    """Fields that may be made read-only but never hidden: those a master can't be saved without, and
    NOT_HIDEABLE."""
    from . import schemas
    return {path: sorted({f for f, info in model.model_fields.items() if info.is_required() and f != "code"}
                         | NOT_HIDEABLE.get(path, set()))
            for path, model in _models(schemas).items()} | {DASHBOARD: []}


def _default(path: str, f: str):
    from . import schemas
    info = _models(schemas)[path].model_fields.get(f)
    return None if info is None or info.is_required() else info.get_default(call_default_factory=True)


def _models(schemas) -> dict:
    return {"items": schemas.Item, "item-groups": schemas.ItemGroup, "uoms": schemas.Uom, "packages": schemas.Package,
            "brands": schemas.Brand, "suppliers": schemas.Supplier, "customers": schemas.Customer,
            "ledger-groups": schemas.LedgerGroup, "ledgers": schemas.Ledger, "sales-counters": schemas.SalesCounter,
            "sales-persons": schemas.SalesPerson}


def master_fields() -> dict[str, list[str]]:
    """Fields per master path (from the request models), for Field Level Rights. The code is never listed."""
    from . import schemas
    models = {"items": schemas.Item, "item-groups": schemas.ItemGroup, "uoms": schemas.Uom, "packages": schemas.Package,
              "brands": schemas.Brand, "suppliers": schemas.Supplier, "customers": schemas.Customer,
              "ledger-groups": schemas.LedgerGroup, "ledgers": schemas.Ledger, "sales-counters": schemas.SalesCounter,
              "sales-persons": schemas.SalesPerson}
    out = {path: [f for f in m.model_fields if f not in ("code", "authorised")] for path, m in models.items()}
    return out | {DASHBOARD: list(DASHBOARD_PANELS)}


# Master path -> store table name.
TABLES = {"items": "ITEMS", "customers": "CUSTOMERS", "suppliers": "SUPPLIERS", "item-groups": "ITEM_GROUPS",
          "brands": "BRANDS", "uoms": "UOMS", "packages": "PACKAGES", "ledgers": "LEDGERS",
          "ledger-groups": "LEDGER_GROUPS", "sales-counters": "COUNTERS", "sales-persons": "SALES_PERSONS"}

# Masters whose rows can need approval, and the documents that refuse unapproved rows.
# Only these are offered: rows waiting for approval can't be used on documents (items, customers, suppliers);
# for other masters an approval would change nothing.
APPROVABLE = {"items": "Item Master", "customers": "Customer Master", "suppliers": "Supplier Master"}

# Fields that may be read-only but never hidden: billing, stock and lookups need their real values on screen
# (a hidden MRP or GST would make the invoice screen price the bill differently from the server).
NOT_HIDEABLE = {
    "items": {"name", "group", "brand", "liqtype", "uom", "package", "bottles_per_case", "mrp", "purchase_rate",
              "cgst_per", "sgst_per", "qoh", "active"},
    "customers": {"name", "points"}, "suppliers": {"name"}, "sales-counters": {"name", "prefix", "inactive"},
    "ledgers": {"name", "group"},
}


def login_settings() -> dict:
    return {**DEFAULT_LOGIN, **store.SECURITY.get("login", {})}


# ─── Rights (role based) ──────────────────────────────────────────────────────

def modules_for(user: dict) -> list[str]:
    if user.get("role") == store.ADMIN_ROLE:
        return store.ROLES.get(store.ADMIN_ROLE, ["*"])
    return store.ROLES.get(user.get("role", ""), [])


def rights_for_user(user: dict) -> dict:
    if user.get("role") == store.ADMIN_ROLE:
        return {}
    return store.ROLE_RIGHTS.get(user.get("role", ""), {})


# ─── Passwords: rules, expiry, additional ADMIN password ──────────────────────

def password_problem(password: str, user: str) -> str | None:
    """Why a new password is not acceptable under Login Settings, or None when it is fine."""
    s = login_settings()
    n = max(8, int(s["min_length"]))
    if len(password) < n:
        return f"Password must be at least {n} characters"
    if password.strip().lower() == user.strip().lower():
        return "Password must not be the same as the user name"
    if s["require_mix"] and (password.isalpha() or password.isdigit()):
        return "Password must mix letters with numbers or symbols"
    if s["require_upper"] and not any(c.isupper() for c in password):
        return "Password must have a capital letter"
    if s["require_lower"] and not any(c.islower() for c in password):
        return "Password must have a small letter"
    if s["require_digit"] and not any(c.isdigit() for c in password):
        return "Password must have a number"
    if s["require_special"] and all(c.isalnum() for c in password):
        return "Password must have a symbol such as # or @"
    return None


def password_expired(user: dict, today: date | None = None) -> bool:
    """True when the user must change their password before doing anything else: the Admin Master "Password
    Reset" date has come and the password is older than it, or it is older than the expiry days."""
    today = today or date.today()
    set_on = user.get("password_set_on")
    set_day = date.fromisoformat(set_on[:10]) if set_on else None
    reset = user.get("password_reset_on")
    if reset and date.fromisoformat(reset) <= today and (set_day is None or set_day < date.fromisoformat(reset)):
        return True
    days = int(login_settings()["password_expiry_days"] or 0)
    return bool(days and set_day and set_day + timedelta(days=days) <= today)


def adnl_admin_set() -> bool:
    return bool(store.SECURITY.get("adnl_admin_hash"))


def check_admin_password(user: dict, sealed: str | None) -> None:
    """403 unless `sealed` (encrypted for "admin") is the additional ADMIN password, or, while none is set,
    the caller's own password. Asked before destructive utilities (merge, remove invoices, archive, ...)."""
    from .routers.auth import _verify_password   # the login check (handles users not yet hashed)
    plain = secure_fields.decrypt(sealed, "admin", "Admin password") if sealed else None
    if not plain:
        raise HTTPException(403, "Enter the " + ("additional ADMIN password" if adnl_admin_set() else "your password")
                            + " to confirm this action.")
    from .routers.auth import MAX_FAILS_PER_USER, _check_not_locked, _record_failure
    key = f"admin-password:{user['user']}"   # wrong tries lock it for 15 minutes, as logins do
    _check_not_locked([(key, MAX_FAILS_PER_USER)])
    ok = verify_password(plain, store.SECURITY["adnl_admin_hash"]) if adnl_admin_set() else _verify_password(user, plain)
    if not ok:
        _record_failure([key])
        raise HTTPException(403, "The " + ("additional ADMIN password" if adnl_admin_set() else "password")
                            + " is not correct.")


def set_adnl_admin(password: str | None) -> None:
    if password:
        store.SECURITY["adnl_admin_hash"] = hash_password(password)
    else:
        store.SECURITY.pop("adnl_admin_hash", None)


# ─── Field Level Rights ───────────────────────────────────────────────────────

def field_rights(user: dict, path: str) -> dict[str, str]:
    if user.get("role") == store.ADMIN_ROLE:
        return {}
    return store.FIELD_RIGHTS.get(user.get("role", ""), {}).get(path, {})


def hide_fields(user: dict, path: str, row: dict) -> dict:
    hidden = [f for f, mode in field_rights(user, path).items() if mode == "hidden"]
    return {k: v for k, v in row.items() if k not in hidden} if hidden else row   # the reply shows the default


def apply_field_rights(user: dict, path: str, new: dict, old: dict | None) -> dict:
    """The row to save: hidden fields keep their old value (or the default for a new row); a read-only field
    may not change (403)."""
    rights = field_rights(user, path)
    if not rights:
        return new
    out = dict(new)
    for f, mode in rights.items():
        if f == "code":
            continue
        before = (old or {}).get(f)
        if mode == "hidden":
            out[f] = before if old is not None else _default(path, f)   # a new row gets the default
        elif mode == "readonly" and old is not None and f in new and new[f] != before:
            raise HTTPException(403, f"You may not change {f.replace('_', ' ')} (Field Level Rights for your role)")
    return out


# ─── Master Approvals ─────────────────────────────────────────────────────────

def needs_approval(path: str) -> bool:
    return path in store.MASTER_APPROVAL.get("masters", [])


def mark_approval(user: dict, path: str, row: dict) -> None:
    """A master row just saved: when its master needs approval, an Administrator's save approves it and anyone
    else's leaves it waiting (authorised False)."""
    if not needs_approval(path):
        return
    if user.get("role") == store.ADMIN_ROLE:
        row.update(authorised=True, auth_by=user["user"], auth_on=date.today().isoformat())
    else:
        row.update(authorised=False, auth_by=None, auth_on=None)


def check_authorised(path: str, code: str, what: str) -> None:
    """409 if a row of a master that needs approval is still waiting for it (rows saved before are fine)."""
    row = getattr(store, TABLES[path]).get(code)
    if needs_approval(path) and row is not None and row.get("authorised") is False:
        raise HTTPException(409, f"{what} {code} is waiting for approval (Admin Master (Security) → Master Approvals)")


# ─── Customised Report rights ─────────────────────────────────────────────────

def can_run_report(user: dict, name: str) -> bool:
    allowed = store.REPORT_RIGHTS.get(name)
    return user.get("role") == store.ADMIN_ROLE or not allowed or user.get("user") in allowed
