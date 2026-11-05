"""Admin Master → Desktop rights: edit the shop's own rights tables from the web (the desktop's frmMstRights).

Rights live in MstUserRights (UserId, MenuItemID, UserRights) when the shop allocates by user, or MstRoleRights (RoleID, MenuItemID, RoleRights)
when by role (UtlMisc.UserBased). UserRights is 12 flags: 1 Add, 2 Edit, 3 Delete, 4 Print, 5 View, 6 Auth, 7 Auth1, 8-12 Opt 1-5. Saving does
what the desktop does: all of the owner's rows are deleted and one row per menu screen is written, in one transaction. Administrator only; each
save goes in the ADMIN Audit Log and the shop's users get their new rights at once (app/desktop_rights.py). Nothing is created in the database."""
from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel, Field

from .. import admin_audit, db, desktop_rights
from .auth import require_admin

router = APIRouter(prefix="/admin/desktop-rights", tags=["Admin Master (Security)"])

TYPES = {1: "Masters", 2: "Transactions", 3: "Vouchers", 4: "Reports", 6: "Tools", 7: "Utilities"}
BLANK = "0" * 12
FLAGS = ("Add", "Edit", "Delete", "Print", "View", "Auth", "Auth1", "Opt-1", "Opt-2", "Opt-3", "Opt-4", "Opt-5")


class RightsRow(BaseModel):
    menu_id: int
    flags: str = Field(min_length=12, max_length=12, pattern=r"^[01]{12}$")


class RightsIn(BaseModel):
    rows: list[RightsRow] = Field(max_length=1000)


def _need_db() -> None:
    if not db.is_db_available():
        raise HTTPException(503, "The database is offline")


def _by_user() -> bool:
    misc = db.fetchone("SELECT TOP 1 UserBased FROM dbo.UtlMisc WITH (NOLOCK)") or {}
    return str(misc.get("UserBased") or "N").strip().upper() == "Y"


def _table() -> tuple[str, str, str]:
    """(table, owner column, flags column) of the shop's allocation."""
    return ("MstUserRights", "UserId", "UserRights") if _by_user() else ("MstRoleRights", "RoleID", "RoleRights")


@router.get("")
def overview(admin: dict = Depends(require_admin)):
    """Who the rights can be given to (users, or roles when the shop allocates by role) and the menu screens they are given for."""
    _need_db()
    by_user = _by_user()
    if by_user:
        owners = [{"id": int(r["UserID"]), "name": (r["UserCode"] or "").strip(), "detail": (r["UserName"] or "").strip()}
                  for r in db.fetchall("SELECT UserID, UserCode, UserName FROM dbo.MstUser WITH (NOLOCK) WHERE UserID > 0 ORDER BY UserCode")]
    else:
        owners = [{"id": int(r["ROLEID"]), "name": (r["ROLECODE"] or "").strip(), "detail": (r["ROLENAME"] or "").strip()}
                  for r in db.fetchall("SELECT ROLEID, ROLECODE, ROLENAME FROM dbo.MstROLE WITH (NOLOCK) WHERE ROLEID > 0 ORDER BY ROLECODE")]
    menu = [{"id": int(r["MenuItemID"]), "caption": (r["MenuItemCaption"] or "").strip(), "type": TYPES.get(int(r["MenuItemType"] or 0), "")}
            for r in db.fetchall("SELECT MenuItemID, MenuItemCaption, MenuItemType FROM dbo.MstMenu WITH (NOLOCK) "
                                 "WHERE MenuItemCaption <> '-' AND MenuShow = 'Y' ORDER BY MenuItemType, MenuItemCaption")]
    return {"by": "user" if by_user else "role", "owners": owners, "menu": menu, "flags": list(FLAGS)}


@router.get("/{owner_id}")
def rights_of(owner_id: int, admin: dict = Depends(require_admin)):
    """{menu_id: flags} the owner has now; a screen with no row is all zeros."""
    _need_db()
    table, owner, col = _table()
    rows = db.fetchall(f"SELECT MenuItemID, [{col}] AS F FROM dbo.[{table}] WITH (NOLOCK) WHERE [{owner}] = ?", (owner_id,))
    return {"rights": {str(int(r["MenuItemID"])): ((r["F"] or "").strip() + BLANK)[:12] for r in rows}}


@router.put("/{owner_id}")
def save_rights(owner_id: int, body: RightsIn, admin: dict = Depends(require_admin)):
    """Replace the owner's rights (the desktop's frmMstRights.saveDetails)."""
    _need_db()
    table, owner, col = _table()
    known = {int(r["MenuItemID"]) for r in db.fetchall("SELECT MenuItemID FROM dbo.MstMenu WITH (NOLOCK)")}
    seen: set[int] = set()
    for r in body.rows:
        if r.menu_id not in known:
            raise HTTPException(422, f"Unknown menu screen {r.menu_id}")
        if r.menu_id in seen:
            raise HTTPException(422, f"Menu screen {r.menu_id} is listed twice")
        seen.add(r.menu_id)
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"DELETE FROM dbo.[{table}] WHERE [{owner}] = ?", (owner_id,))
            for r in body.rows:
                cur.execute(f"INSERT INTO dbo.[{table}] ([{owner}], MenuItemID, [{col}]) VALUES (?, ?, ?)", (owner_id, r.menu_id, r.flags))
    admin_audit.record("Admin Master", "Edited", f"{table}:{owner_id}", "", f"rights of {owner_id}: {len(body.rows)} screens")
    desktop_rights.load()   # the new rights apply at once
    return rights_of(owner_id, admin)
