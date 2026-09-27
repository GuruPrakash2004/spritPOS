"""Master screens. Legacy vLQMS drives most masters through one generic engine
(frmMstGeneral), so here one CRUD factory serves every master table."""
from fastapi import APIRouter, Header, HTTPException
from pydantic import BaseModel

from .. import store
from ..schemas import (
    Brand, Company, Customer, Item, ItemGroup, Ledger, LedgerGroup, Package, PriceUpdate, SalesCounter, Supplier, Uom,
)
from .auth import require_admin

router = APIRouter(prefix="/masters")


def crud(path: str, table: dict, model: type[BaseModel], tag: str) -> None:
    @router.get(f"/{path}", response_model=list[model], tags=[tag], name=f"List {tag}")
    def list_all(q: str | None = None):
        rows = table.values()
        if q:
            ql = q.lower()
            rows = [r for r in rows if ql in r["code"].lower() or ql in r.get("name", "").lower()]
        return list(rows)

    @router.get(f"/{path}/{{code}}", response_model=model, tags=[tag], name=f"Get {tag}")
    def get_one(code: str):
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        return table[code]

    @router.post(f"/{path}", response_model=model, status_code=201, tags=[tag], name=f"Create {tag}")
    def create(body: model):
        if body.code in table:
            raise HTTPException(409, f"{tag} '{body.code}' already exists")
        table[body.code] = body.model_dump()
        return table[body.code]

    @router.put(f"/{path}/{{code}}", response_model=model, tags=[tag], name=f"Update {tag}")
    def update(code: str, body: model):
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        table.pop(code)
        table[body.code] = body.model_dump()
        return table[body.code]

    @router.delete(f"/{path}/{{code}}", status_code=204, tags=[tag], name=f"Delete {tag}")
    def delete(code: str, x_user: str | None = Header(None)):
        if path == "items" and x_user:
            u = store.USERS.get(x_user.strip().upper())
            if u and u.get("role") == "Cashier":
                raise HTTPException(403, "Cashiers are not permitted to delete items. Manager or Administrator approval required.")
        if table.pop(code, None) is None:
            raise HTTPException(404, f"{tag} '{code}' not found")


@router.get("/company", response_model=Company, tags=["Company Master"])
def get_company():
    return store.COMPANY


@router.put("/company", response_model=Company, tags=["Company Master"])
def update_company(body: Company, x_user: str | None = Header(None)):
    require_admin(x_user)
    store.COMPANY.update(body.model_dump())
    return store.COMPANY


@router.post("/items/price-list", response_model=list[Item], tags=["Item Price List Maintenance"])
def bulk_update_rates(body: PriceUpdate):
    """Select by item group or item codes, then shift a rate column by an amount (legacy Load → Apply)."""
    targets = [i for i in store.ITEMS.values()
               if (body.group and i["group"] == body.group) or (body.item_codes and i["code"] in body.item_codes)]
    if not targets:
        raise HTTPException(404, "No items match the selection")
    for i in targets:
        i[body.column] = round(max(0.0, i[body.column] + body.change_by), 2)
    return targets


@router.post("/items/import", tags=["Item Master"])
def import_items(rows: list[Item], overwrite: bool = False):
    """Import Item from Excel: existing codes are skipped (reported as duplicates) unless overwrite=true."""
    added, duplicates = [], []
    for r in rows:
        if r.code in store.ITEMS and not overwrite:
            duplicates.append(r.code)
            continue
        store.ITEMS[r.code] = r.model_dump()
        added.append(r.code)
    return {"added": added, "duplicates": duplicates}


crud("items", store.ITEMS, Item, "Item Master")
crud("item-groups", store.ITEM_GROUPS, ItemGroup, "Item Group Master")
crud("uoms", store.UOMS, Uom, "UOM Master")
crud("packages", store.PACKAGES, Package, "Package Master")
crud("brands", store.BRANDS, Brand, "Brand Master")
crud("suppliers", store.SUPPLIERS, Supplier, "Supplier Master")
crud("customers", store.CUSTOMERS, Customer, "Customer Master")
crud("ledger-groups", store.LEDGER_GROUPS, LedgerGroup, "Ledger Group Master")
crud("ledgers", store.LEDGERS, Ledger, "Ledger Master")
crud("sales-counters", store.COUNTERS, SalesCounter, "Sales Counter Master")
