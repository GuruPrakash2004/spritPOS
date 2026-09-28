"""Master screens. Legacy vLQMS drives most masters through one generic engine
(frmMstGeneral), so here one CRUD factory serves every master table.

Rules every master write follows:
- The caller's role needs the master's module (require_module), e.g. "Item Master".
- Codes are trimmed and must be non-empty, at most 40 characters, and free of / ? # % \\ (they go in URLs).
- A record's code can't be changed to another record's code, and a record that other data uses
  (an item on a bill, a group with items, a counter with bills, ...) can't be deleted or re-coded.
- Item QOH is opening stock: it is set when the item is created (logged as an "opening" movement)
  and afterwards changes only through transactions, never by editing the item.
The checks run on requests only; response models stay loose so odd rows loaded from the DB still list.
"""
import re
from datetime import date
from typing import Callable

from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel

from .. import store, writeback
from ..schemas import (
    Brand, Company, Customer, Item, ItemGroup, Ledger, LedgerGroup, Package, PriceUpdate, SalesCounter, Supplier, Uom,
)
from .auth import can_see_cost, get_current_user, hides_cost, require_admin, require_right

router = APIRouter(prefix="/masters")

BAD_CODE = re.compile(r"[/?#%\\]")


def _docs_lines():
    for kind, docs in store.DOCS.items():
        for d in docs.values():
            for ln in d.get("lines", []):
                yield kind, d, ln


def _item_in_use(code: str) -> bool:
    return any(m["item"] == code for m in store.MOVEMENTS if m["kind"] != "opening") or any(
        ln.get("item_code") == code for _, _, ln in _docs_lines())


# master path -> function(code) returning why the record is in use, or None.
IN_USE: dict[str, Callable[[str], str | None]] = {
    "items": lambda c: "it is on bills or has stock movements; mark it inactive instead" if _item_in_use(c) else None,
    "item-groups": lambda c: "items belong to this group" if any(i["group"] == c for i in store.ITEMS.values()) else None,
    "uoms": lambda c: "items use this UOM" if any(i["uom"] == c for i in store.ITEMS.values()) else None,
    "packages": lambda c: "items use this package" if any(i["package"] == c for i in store.ITEMS.values()) else None,
    "brands": lambda c: "items use this brand" if any(i["brand"] == c for i in store.ITEMS.values()) else None,
    "suppliers": lambda c: "purchase bills or indents use this supplier" if any(
        d.get("supplier") == c for k in ("purchase_bill", "purchase_indent") for d in store.DOCS[k].values()) else None,
    "customers": lambda c: "it is the walk-in cash customer" if c == "CASH" else (
        "sales bills use this customer" if any(d.get("customer") == c for k in ("sales_invoice", "sales_return")
                                               for d in store.DOCS[k].values()) else None),
    "ledger-groups": lambda c: "ledgers belong to this group" if any(l["group"] == c for l in store.LEDGERS.values()) else None,
    "ledgers": lambda c: "expense entries use this ledger" if any(
        ln.get("ledger") == c for d in store.DOCS["expense"].values() for ln in d["lines"]) else None,
    "sales-counters": lambda c: "sales bills were made on this counter" if any(
        d.get("counter") == c for d in store.DOCS["sales_invoice"].values()) else None,
}


def _check_code(code: str) -> str:
    code = code.strip()
    if not code:
        raise HTTPException(422, "Code cannot be blank")
    if len(code) > 40:
        raise HTTPException(422, "Code must be 40 characters or fewer")
    if BAD_CODE.search(code):
        raise HTTPException(422, "Code cannot contain / ? # % or \\")
    return code


def _check_item(body: Item) -> None:
    if body.rol < 0 or body.roq < 0:
        raise HTTPException(422, "ROL and ROQ cannot be negative")
    if not (0 <= body.cgst_per <= 100 and 0 <= body.sgst_per <= 100):
        raise HTTPException(422, "CGST % and SGST % must be between 0 and 100")
    if body.group not in store.ITEM_GROUPS:
        raise HTTPException(422, f"Unknown item group '{body.group}'")
    if body.package and body.package not in store.PACKAGES:
        raise HTTPException(422, f"Unknown package '{body.package}'")
    if body.uom and body.uom not in store.UOMS:
        raise HTTPException(422, f"Unknown UOM '{body.uom}'")
    if body.brand and body.brand not in store.BRANDS:
        raise HTTPException(422, f"Unknown brand '{body.brand}'")


def _clean(path: str, body: BaseModel, table: dict) -> BaseModel:
    """Request-side validation shared by create and update."""
    body.code = _check_code(body.code)
    if hasattr(body, "name"):
        body.name = body.name.strip()
        if not body.name:
            raise HTTPException(422, "Name cannot be blank")
    if path == "items":
        _check_item(body)
    elif path == "ledgers" and body.group not in store.LEDGER_GROUPS:
        raise HTTPException(422, f"Unknown ledger group '{body.group}'")
    elif path == "sales-counters" and not body.prefix.strip():
        raise HTTPException(422, "Bill prefix cannot be blank")
    elif path == "customers" and body.points < 0:
        raise HTTPException(422, "Points cannot be negative")
    return body


def _shown(path: str, row: dict, user: dict) -> dict:
    """An item row as this user may see it (purchase rate hidden from roles without cost access)."""
    return {**row, "purchase_rate": None} if path == "items" and hides_cost(user) else row


def _open_stock(code: str, qty: float) -> None:
    """Record a new item's opening stock as a movement so the stock ledger starts from it."""
    store.ITEMS[code]["qoh"] = 0
    if qty:
        store.move_stock(code, qty, "opening", "OPENING", date.today(), store.ITEMS[code]["purchase_rate"])
        writeback.changed("opening_stock", {"doc_no": f"OPENING/{code}", "date": date.today().isoformat(),
                                            "lines": [{"item_code": code, "qty": qty,
                                                       "rate": store.ITEMS[code]["purchase_rate"]}]},
                          moves_ref="OPENING", item=code)


def crud(path: str, table: dict, model: type[BaseModel], tag: str, modules: tuple[str, ...], store_name: str) -> None:
    can_add = [Depends(require_right("add", *modules))]
    can_edit = [Depends(require_right("edit", *modules))]
    can_delete = [Depends(require_right("delete", *modules))]

    @router.get(f"/{path}", response_model=list[model], tags=[tag], name=f"List {tag}")
    def list_all(q: str | None = None, current_user: dict = Depends(get_current_user)):
        rows = table.values()
        if q:
            ql = q.lower()
            rows = [r for r in rows if ql in r["code"].lower() or ql in r.get("name", "").lower()]
        return [_shown(path, r, current_user) for r in rows]

    @router.get(f"/{path}/{{code}}", response_model=model, tags=[tag], name=f"Get {tag}")
    def get_one(code: str, current_user: dict = Depends(get_current_user)):
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        return _shown(path, table[code], current_user)

    @router.post(f"/{path}", response_model=model, status_code=201, tags=[tag], name=f"Create {tag}", dependencies=can_add)
    def create(body: model, current_user: dict = Depends(get_current_user)):
        body = _clean(path, body, table)
        if body.code in table:
            raise HTTPException(409, f"{tag} '{body.code}' already exists")
        table[body.code] = body.model_dump()
        store.touch(store_name, body.code)
        if path == "items":
            if table[body.code]["purchase_rate"] is None:
                table[body.code]["purchase_rate"] = 0.0
            _open_stock(body.code, body.qoh)
        return _shown(path, table[body.code], current_user)

    @router.put(f"/{path}/{{code}}", response_model=model, tags=[tag], name=f"Update {tag}", dependencies=can_edit)
    def update(code: str, body: model, current_user: dict = Depends(get_current_user)):
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        body = _clean(path, body, table)
        if body.code != code:
            if body.code in table:
                raise HTTPException(409, f"{tag} '{body.code}' already exists")
            reason = IN_USE[path](code)
            if reason:
                raise HTTPException(409, f"Can't change the code of {tag} '{code}': {reason}")
        row = body.model_dump()
        if path == "items":
            row["qoh"] = table[code]["qoh"]   # stock changes only through transactions
            if row["purchase_rate"] is None or not can_see_cost(current_user):
                row["purchase_rate"] = table[code]["purchase_rate"]   # can't be changed by someone who can't see it
        table.pop(code)
        table[body.code] = row
        store.touch(store_name, code)
        store.touch(store_name, body.code)
        return _shown(path, row, current_user)

    @router.delete(f"/{path}/{{code}}", status_code=204, tags=[tag], name=f"Delete {tag}", dependencies=can_delete)
    def delete(code: str, current_user: dict = Depends(get_current_user)):
        if path == "items" and current_user.get("role") == "Cashier":
            raise HTTPException(403, "Cashiers are not permitted to delete items. Manager or Administrator approval required.")
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        reason = IN_USE[path](code)
        if reason:
            raise HTTPException(409, f"Can't delete {tag} '{code}': {reason}")
        del table[code]
        store.touch(store_name, code)


@router.get("/company", response_model=Company, tags=["Company Master"])
def get_company():
    return store.COMPANY


@router.put("/company", response_model=Company, tags=["Company Master"])
def update_company(body: Company, admin: dict = Depends(require_admin)):
    store.COMPANY.update(body.model_dump())
    store.touch("COMPANY", "*")
    return store.COMPANY


@router.post("/items/price-list", response_model=list[Item], tags=["Item Price List Maintenance"],
             dependencies=[Depends(require_right("edit", "Price List Maintenance"))])
def bulk_update_rates(body: PriceUpdate):
    """Select by item group or item codes, then shift a rate column by an amount (legacy Load → Apply)."""
    targets = [i for i in store.ITEMS.values()
               if (body.group and i["group"] == body.group) or (body.item_codes and i["code"] in body.item_codes)]
    if not targets:
        raise HTTPException(404, "No items match the selection")
    for i in targets:
        i[body.column] = round(max(0.0, i[body.column] + body.change_by), 2)
        store.touch("ITEMS", i["code"])
    return targets


@router.post("/items/import", tags=["Item Master"], dependencies=[Depends(require_right("add", "Item Master"))])
def import_items(rows: list[Item], overwrite: bool = False, current_user: dict = Depends(get_current_user)):
    """Import Item from Excel: existing codes are skipped (reported as duplicates) unless overwrite=true.
    Every row is checked first; nothing is imported if any row is invalid. Overwriting keeps the item's stock."""
    if len(rows) > 5000:
        raise HTTPException(413, "Import at most 5000 items at a time")
    problems = []
    seen: set[str] = set()
    for n, r in enumerate(rows, start=1):
        try:
            _clean("items", r, store.ITEMS)
        except HTTPException as e:
            problems.append(f"Row {n} ({r.code or 'no code'}): {e.detail}")
            continue
        if r.code in seen:
            problems.append(f"Row {n} ({r.code}): the same code appears twice in the file")
        seen.add(r.code)
    if problems:
        raise HTTPException(422, "; ".join(problems[:10]) + (f" … and {len(problems) - 10} more" if len(problems) > 10 else ""))

    added, duplicates = [], []
    for r in rows:
        existing = store.ITEMS.get(r.code)
        if existing and not overwrite:
            duplicates.append(r.code)
            continue
        store.ITEMS[r.code] = r.model_dump()
        store.touch("ITEMS", r.code)
        if store.ITEMS[r.code]["purchase_rate"] is None:
            store.ITEMS[r.code]["purchase_rate"] = existing["purchase_rate"] if existing else 0.0
        if existing:
            store.ITEMS[r.code]["qoh"] = existing["qoh"]
            if not can_see_cost(current_user):
                store.ITEMS[r.code]["purchase_rate"] = existing["purchase_rate"]
        else:
            _open_stock(r.code, r.qoh)
        added.append(r.code)
    return {"added": added, "duplicates": duplicates}


crud("items", store.ITEMS, Item, "Item Master", ("Item Master",), "ITEMS")
crud("item-groups", store.ITEM_GROUPS, ItemGroup, "Item Group Master", ("Item Group Master",), "ITEM_GROUPS")
crud("uoms", store.UOMS, Uom, "UOM Master", ("UOM Master",), "UOMS")
crud("packages", store.PACKAGES, Package, "Package Master", ("Package Master",), "PACKAGES")
crud("brands", store.BRANDS, Brand, "Brand Master", ("Brand Master",), "BRANDS")
crud("suppliers", store.SUPPLIERS, Supplier, "Supplier Master", ("Supplier Master", "Purchase Bill"), "SUPPLIERS")
crud("customers", store.CUSTOMERS, Customer, "Customer Master", ("Customer Master",), "CUSTOMERS")
crud("ledger-groups", store.LEDGER_GROUPS, LedgerGroup, "Ledger Group Master", ("Ledger Group Master",), "LEDGER_GROUPS")
crud("ledgers", store.LEDGERS, Ledger, "Ledger Master", ("Ledger Master",), "LEDGERS")
crud("sales-counters", store.COUNTERS, SalesCounter, "Sales Counter Master", ("Sales Counter Master",), "COUNTERS")
