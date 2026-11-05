"""Sales Invoice's Quick Selection list (the desktop's Quick Selection panel): named sets of items kept in the desktop's own table
MstQuickSelItems (QSetName, ItemID). The panel on the bill shows a set's items; Edit adds or removes items and creates or deletes sets,
as the desktop does. Written straight into that table inside `db.direct_writes()` (main.DIRECT_WRITE_PATHS), nothing else is created."""
from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel, Field

from .. import db, store
from .auth import get_current_user, require_module



class QuickSet(BaseModel):
    codes: list[str] = Field(default_factory=list, max_length=2000)


def _by_id() -> dict[str, str]:
    return {str(i.get("_itemid")): code for code, i in store.ITEMS.items() if i.get("_itemid") not in (None, "")}


def make_router(prefix: str, table: str) -> APIRouter:
    """One router for a table of (QSetName, ItemID) sets: the Quick Selection panel (MstQuickSelItems) and Load Template (MstItemTemp)."""
    router = APIRouter(prefix=prefix, tags=["Sales Invoice"])
    T = table   # a fixed name from this file, never from a request

    @router.get("")
    def list_sets(current_user: dict = Depends(get_current_user)):
        """Every set with the item codes in it: {"sets": [{"name": "...", "codes": ["...", ...]}]}. Items no longer in Item Master are left out."""
        if not db.is_db_available():
            return {"sets": []}
        by_id = _by_id()
        sets: dict[str, list[str]] = {}
        try:
            rows = db.fetchall(f"SELECT QSetName, ItemID FROM dbo.{T} WITH (NOLOCK) ORDER BY QSetName")
        except Exception:   # noqa: BLE001 - a client database without the table
            return {"sets": []}
        for r in rows:
            name = str(r["QSetName"] or "").strip()
            if not name:
                continue
            codes = sets.setdefault(name, [])
            code = by_id.get(str(r["ItemID"]))
            if code and code not in codes:
                codes.append(code)
        return {"sets": [{"name": n, "codes": c} for n, c in sets.items()]}


    @router.put("/{name}")
    def save_set(name: str, body: QuickSet, current_user: dict = Depends(require_module("Sales Invoice"))):
        """Make the set `name` hold exactly these items (creates it when new)."""
        name = name.strip()
        if not name or len(name) > 50:
            raise HTTPException(422, "Enter a name for the quick selection (up to 50 characters)")
        ids = []
        for code in dict.fromkeys(body.codes):
            item = store.ITEMS.get(code)
            if not item or item.get("_itemid") in (None, ""):
                raise HTTPException(422, f"Unknown item '{code}'")
            ids.append(int(item["_itemid"]))
        if not db.is_db_available():
            raise HTTPException(503, "The database is offline")
        with db.direct_writes():
            with db.transaction() as cur:
                cur.execute(f"DELETE FROM dbo.{T} WHERE QSetName = ?", (name,))
                for item_id in ids:
                    cur.execute(f"INSERT INTO dbo.{T} (QSetName, ItemID) VALUES (?, ?)", (name, item_id))
        return {"name": name, "codes": list(dict.fromkeys(body.codes))}


    @router.delete("/{name}", status_code=204)
    def delete_set(name: str, current_user: dict = Depends(require_module("Sales Invoice"))):
        if not db.is_db_available():
            raise HTTPException(503, "The database is offline")
        with db.direct_writes():
            with db.transaction() as cur:
                cur.execute(f"DELETE FROM dbo.{T} WHERE QSetName = ?", (name.strip(),))

    return router


router = make_router("/quick-selection", "MstQuickSelItems")
templates_router = make_router("/item-templates", "MstItemTemp")
