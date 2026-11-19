"""The shop's own rights, as the desktop vLQMS gives them (frmMstRights / frmMain.EnableDisableMenus / svtModule.chkUserRights).

The desktop keeps, for every menu screen (MstMenu), a 12-character string of 0/1 flags per user (MstUserRights.UserRights) or per role
(MstRoleRights.RoleRights), whichever the shop chose in Login Settings (UtlMisc.UserBased = 'Y' means by user). Position 1 Add, 2 Edit, 3 Delete,
4 Print, 5 View, 6 Auth, 7 Auth1, 8-12 Opt 1-5. A screen is open to the user when it has a row that is not all zeros (a screen with no row is
greyed out); the user code ADMIN has every right. Here that decides, for each web user, which modules open and which of Add / Edit / Delete /
Print they have. A web screen with no menu row of its own (Cash Denomination, SMS, ...) keeps what the user's web role gives. A role the
Administrator has set in the web's Admin Master replaces all of this (security.modules_for).

`compute` is pure (rows in, rights out) so it can be tested; `load` reads the tables and fills each store.USERS entry's "desktop" key."""
from __future__ import annotations

import logging

from . import db, store

logger = logging.getLogger("vlqms.rights")

# The caption a web module has in MstMenu when it is not the module's own name (the same table as frontend/src/lib/menuAliases.js).
MENU_CAPTIONS = {
    "Price List Maintenance": "Item Price Maintenance Master", "Stock Ledger": "Stock Ledger Report", "Stock Ageing": "Stock Ageing Report",
    "Profit Margin": "Profit Margin Report", "Brand-wise Sales": "Brand Wise Report", "Stock Level / Reorder": "Stock Level Report",
    "Package-wise Sales": "Package Wise Sales Report", "Package-wise Purchase": "Package Wise Purchase Report",
    "Drops Sales Summary": "Drops Reports", "Drops Purchase": "Drops Reports", "Drops Petty Cash": "Drops Reports", "Drops Consolidated": "Drops Reports",
    "Reports Dashboard": "Customised Reports...", "Global Settings": "Global Settings...",
}
ALWAYS = ("Dashboard", "About")
ADD, EDIT, DELETE, PRINT, VIEW = 1, 2, 3, 4, 5


def _flag(rights: str, pos: int) -> bool:
    return len(rights) >= pos and rights[pos - 1] == "1"


def _universe() -> dict[str, str]:
    """lower-case web module name -> its spelling, from every role's module list and the caption table."""
    names = {m for mods in store.ROLES.values() for m in mods if m != "*"} | set(MENU_CAPTIONS)
    return {n.lower(): n for n in names}


def modules_of_caption(caption: str, universe: dict[str, str] | None = None) -> set[str]:
    """The web modules a menu caption stands for."""
    universe = universe if universe is not None else _universe()
    cap = (caption or "").strip()
    out = {m for m, c in MENU_CAPTIONS.items() if c.lower() == cap.lower()}
    out.add(universe.get(cap.lower(), cap))
    return out


def compute(menu: dict[int, str], rights: dict[int, str], fallback: list[str]) -> dict:
    """{"modules": [...], "rights": {module: {add, edit, delete, print}}} for one user.

    `menu` is MenuItemID -> caption, `rights` the user's (or role's) MenuItemID -> flags, `fallback` the modules the web role gives (kept only for a
    web screen that has no menu row at all)."""
    universe = _universe()
    open_modules: set[str] = set(ALWAYS)
    flags: dict[str, dict[str, bool]] = {}
    in_menu: set[str] = set()
    for menu_id, caption in menu.items():
        mods = modules_of_caption(caption, universe)
        in_menu |= mods
        row = rights.get(menu_id)
        if not row or "1" not in row:   # no row, or all zeros: the desktop greys the screen out
            continue
        for m in mods:
            open_modules.add(m)
            cur = flags.setdefault(m, {"add": False, "edit": False, "delete": False, "print": False})
            for key, pos in (("add", ADD), ("edit", EDIT), ("delete", DELETE), ("print", PRINT)):
                cur[key] = cur[key] or _flag(row, pos)
    open_modules |= {m for m in fallback if m not in in_menu and m != "*"}
    return {"modules": sorted(open_modules), "rights": flags}


def load() -> None:
    """Read the menu and the rights tables and give every non-admin web user their desktop rights (store.USERS[...]["desktop"])."""
    if not db.is_db_available():
        return
    try:
        menu = {int(r["MenuItemID"]): (r["MenuItemCaption"] or "").strip() for r in db.fetchall("SELECT MenuItemID, MenuItemCaption FROM dbo.MstMenu WITH (NOLOCK)")
                if (r["MenuItemCaption"] or "").strip() not in ("", "-")}
        misc = db.fetchone("SELECT TOP 1 UserBased FROM dbo.UtlMisc WITH (NOLOCK)") or {}
        by_user = str(misc.get("UserBased") or "N").strip().upper() == "Y"
        ids = {r["UserCode"].strip().upper(): (int(r["UserID"]), int(r["RoleID"] or 0)) for r in db.fetchall("SELECT UserID, UserCode, RoleID FROM dbo.MstUser WITH (NOLOCK)") if r["UserCode"]}
        if by_user:
            rows = db.fetchall("SELECT UserId AS Owner, MenuItemID, UserRights AS Rights FROM dbo.MstUserRights WITH (NOLOCK)")
        else:
            rows = db.fetchall("SELECT RoleID AS Owner, MenuItemID, RoleRights AS Rights FROM dbo.MstRoleRights WITH (NOLOCK)")
    except Exception as e:   # noqa: BLE001 - a client database without these tables keeps the web roles
        logger.warning("Desktop rights not read: %s", e)
        return
    owned: dict[int, dict[int, str]] = {}
    for r in rows:
        owned.setdefault(int(r["Owner"]), {})[int(r["MenuItemID"])] = (r["Rights"] or "").strip()
    n = 0
    for code, u in store.USERS.items():
        if u.get("role") == store.ADMIN_ROLE or code not in ids:
            u.pop("desktop", None)
            continue
        user_id, role_id = ids[code]
        mine = owned.get(user_id if by_user else role_id, {})
        # No rows at all for this user / role: the shop doesn't use these tables for them, so the web role stays rather than locking them out.
        # Rows that exist but are all zero are a decision made on the desktop (every screen greyed out): that is honoured, as the exe does.
        if not mine:
            u.pop("desktop", None)
            logger.warning("%s has no rights rows (%s): the web role %s applies", code, "by user" if by_user else "by role", u.get("role"))
            continue
        u["desktop"] = compute(menu, mine, store.ROLES.get(u.get("role", ""), []))
        n += 1
    logger.info("Desktop rights applied to %d users (%s)", n, "by user" if by_user else "by role")
