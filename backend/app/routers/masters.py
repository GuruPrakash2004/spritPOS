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

from .. import admin_audit, gsettings, security, store
from ..master_specs import SPECS
from ..schemas import SIMPLE_MODELS
from ..schemas import (
    Brand, Company, CompanyOut, Customer, Item, ItemGroup, Ledger, LedgerGroup, Package, PriceUpdate, BulkItemUpdate, SalesCounter, SalesPerson, Supplier, Uom,
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
    **{sp["path"]: (lambda c: None) for sp in SPECS},   # the simple masters: sqlstore checks the desktop's tables on delete
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
    "sales-persons": lambda c: "sales bills name this sales person" if any(
        d.get("salesperson") in (c, (store.SALES_PERSONS.get(c) or {}).get("name"))
        for d in store.DOCS["sales_invoice"].values()) else None,
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


def _check_margin(row: dict) -> None:
    """Global Settings PurSellVarPer: MRP must exceed the purchase rate by at least this %. Checked on the row
    as it will be saved (with the stored purchase rate when the caller can't see or send it)."""
    pct = gsettings.number("PurSellVarPer")
    rate = row.get("purchase_rate") or 0
    if pct > 0 and rate and row.get("mrp", 0) < round(rate * (1 + pct / 100), 2):
        raise HTTPException(422, f"MRP of {row.get('code')} must be at least {pct:g}% above the purchase rate "
                                 f"(₹ {rate * (1 + pct / 100):.2f}; Global Settings → PurSellVarPer)")


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
    elif path == "holidays":
        try:
            date.fromisoformat(body.code)
        except ValueError:
            raise HTTPException(422, "The holiday's date must be a valid date (yyyy-mm-dd)")
    elif path == "taxes" and not all(0 <= v <= 100 for v in (body.cgst_per, body.sgst_per, body.igst_per)):
        raise HTTPException(422, "Tax percentages must be between 0 and 100")
    elif path == "packages" and body.litres_per_bottle <= 0:
        raise HTTPException(422, "Litres per bottle must be more than 0")
    return body


def _shown(path: str, row: dict, user: dict) -> dict:
    """A row as this user may see it: purchase rate hidden from roles without cost access, and fields hidden by
    the role's Field Level Rights (Admin Master (Security))."""
    row = {**row, "purchase_rate": None} if path == "items" and hides_cost(user) else row
    return security.hide_fields(user, path, row)


def _open_stock(code: str, qty: float) -> None:
    """Record a new item's opening stock as a movement so the stock ledger starts from it."""
    store.ITEMS[code]["qoh"] = 0
    if qty:
        store.move_stock(code, qty, "opening", "OPENING", date.today(), store.ITEMS[code]["purchase_rate"])


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
        row = security.apply_field_rights(current_user, path, body.model_dump(), None)
        if path == "items":
            _check_margin(row)
        row.pop("authorised", None)
        security.mark_approval(current_user, path, row)
        row["changed_by"] = current_user["user"]
        table[body.code] = row
        store.touch(store_name, body.code)
        if path == "items":
            if table[body.code]["purchase_rate"] is None:
                table[body.code]["purchase_rate"] = 0.0
            _open_stock(body.code, body.qoh)
        admin_audit.record(tag, "Added", body.code, "", admin_audit.diff({}, table[body.code])[1])
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
        row = security.apply_field_rights(current_user, path, body.model_dump(), table[code])
        row.pop("authorised", None)
        for k in ("authorised", "auth_by", "auth_on"):
            if k in table[code]:
                row[k] = table[code][k]
        if path == "items":
            row["qoh"] = table[code]["qoh"]   # stock changes only through transactions
            if row["purchase_rate"] is None or not can_see_cost(current_user):
                row["purchase_rate"] = table[code]["purchase_rate"]   # can't be changed by someone who can't see it
            _check_margin(row)
        security.mark_approval(current_user, path, row)
        row["changed_by"] = current_user["user"]
        earlier, changed = admin_audit.diff({k: v for k, v in table[code].items() if k != "changed_by"},
                                            {k: v for k, v in row.items() if k != "changed_by"})
        if changed:
            admin_audit.record(tag, "Edited", code if body.code == code else f"{code} → {body.code}", earlier, changed)
        table.pop(code)
        table[body.code] = row
        if path == "items" and body.code != code:
            store.rename_item_stock(code, body.code)
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
        admin_audit.record(tag, "Deleted", code, admin_audit.diff(table[code], {})[0], "")
        del table[code]
        if path == "items":
            store.rename_item_stock(code, None)
        store.touch(store_name, code)


def _company() -> dict:
    return {**store.COMPANY, "fin_years": store.FIN_YEARS}


@router.get("/company", response_model=CompanyOut, tags=["Company Master"])
def get_company():
    return _company()


@router.put("/company", response_model=CompanyOut, tags=["Company Master"])
def update_company(body: Company, admin: dict = Depends(require_admin)):
    earlier, changed = admin_audit.diff(store.COMPANY, {**store.COMPANY, **body.model_dump()})
    if changed:
        admin_audit.record("Company Master", "Edited", store.COMPANY.get("name") or "Company", earlier, changed)
    changes = body.model_dump()
    if store.FIN_YEARS:   # the financial year is the desktop's (MstCompFinYear), not edited here
        changes.update(fy_start=store.COMPANY["fy_start"], fy_end=store.COMPANY["fy_end"])
    store.COMPANY.update(changes)
    store.touch("COMPANY", "*")
    store.set_fin_year()
    return _company()


@router.post("/items/price-list", response_model=list[Item], tags=["Item Price List Maintenance"],
             dependencies=[Depends(require_right("edit", "Price List Maintenance"))])
def bulk_update_rates(body: PriceUpdate, current_user: dict = Depends(get_current_user)):
    """Select by item group or item codes, then shift a rate column by an amount (legacy Load → Apply).
    Follows Admin Master (Security): a column that is read-only for the role is refused, and changed items of a
    master that needs approval wait for it again."""
    if security.field_rights(current_user, "items").get(body.column):
        raise HTTPException(403, f"You may not change {body.column.replace('_', ' ')} (Field Level Rights for your role)")
    targets = [i for i in store.ITEMS.values()
               if (body.group and i["group"] == body.group) or (body.item_codes and i["code"] in body.item_codes)]
    if not targets:
        raise HTTPException(404, "No items match the selection")
    for i in targets:
        old = i[body.column]
        i[body.column] = round(max(0.0, i[body.column] + body.change_by), 2)
        security.mark_approval(current_user, "items", i)
        i["changed_by"] = current_user["user"]
        store.touch("ITEMS", i["code"])
        admin_audit.record("Price List Maintenance", "Edited", i["code"], f"{body.column}: {old}", f"{body.column}: {i[body.column]}")
    return [_shown("items", i, current_user) for i in targets]


# Fields Bulk Master Update can set (the desktop's UtlItemBulkUpdate lists these as HSNCode, BrandID, LiqType, BotPerCases, MRP,
# PURRATE, ROL, ROQ); its other entries (min/max level, KSBCL names, ...) have no field in the web Item Master.
BULK_FIELDS = {"hsn_code": str, "brand": str, "liqtype": str, "bottles_per_case": int, "mrp": float, "purchase_rate": float,
               "rol": int, "roq": int}


@router.post("/items/bulk-update", tags=["Bulk Master Update"], dependencies=[Depends(require_right("edit", "Bulk Master Update"))])
def bulk_update_items(body: BulkItemUpdate, current_user: dict = Depends(get_current_user)):
    """Set one field to one value on the chosen items. Follows Admin Master (Security): a field that is read-only for the
    role is refused, cost needs cost access, changed rows of a master that needs approval wait for it again, and each
    change goes to the ADMIN Audit Log. Nothing changes unless every chosen item passes."""
    kind = BULK_FIELDS[body.field]
    if security.field_rights(current_user, "items").get(body.field):
        raise HTTPException(403, f"You may not change {body.field.replace('_', ' ')} (Field Level Rights for your role)")
    if body.field == "purchase_rate" and not can_see_cost(current_user):
        raise HTTPException(403, "You may not change the purchase rate (no cost access for your role)")
    try:
        value = kind(float(body.value)) if kind in (int, float) else str(body.value).strip()
    except (TypeError, ValueError):
        raise HTTPException(422, f"'{body.value}' is not a valid value for {body.field.replace('_', ' ')}")
    if kind in (int, float) and value < 0:
        raise HTTPException(422, "The value cannot be negative")
    if body.field == "bottles_per_case" and value < 1:
        raise HTTPException(422, "Bottles per case must be at least 1")
    if body.field == "brand" and value not in store.BRANDS:
        raise HTTPException(422, f"Unknown brand '{value}'")
    if isinstance(value, str) and len(value) > 40:
        raise HTTPException(422, "The value must be 40 characters or fewer")
    missing = [c for c in dict.fromkeys(body.item_codes) if c not in store.ITEMS]
    if missing:
        raise HTTPException(404, f"Unknown item(s): {', '.join(missing[:5])}")
    targets = [store.ITEMS[c] for c in dict.fromkeys(body.item_codes)]
    if body.field in ("mrp", "purchase_rate"):   # the margin rule is checked on every row before any is changed
        for i in targets:
            _check_margin({**i, body.field: value})
    for i in targets:
        old = i.get(body.field)
        i[body.field] = value
        security.mark_approval(current_user, "items", i)
        i["changed_by"] = current_user["user"]
        store.touch("ITEMS", i["code"])
        admin_audit.record("Bulk Master Update", "Edited", i["code"], f"{body.field}: {old}", f"{body.field}: {value}")
    return {"changed": len(targets), "field": body.field, "value": value}


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
        existing = store.ITEMS.get(r.code)
        try:
            rate = r.purchase_rate if r.purchase_rate is not None and can_see_cost(current_user) else (existing or {}).get("purchase_rate")
            _check_margin({**r.model_dump(), "purchase_rate": rate})
        except HTTPException as e:
            problems.append(f"Row {n} ({r.code}): {e.detail}")
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
        row = security.apply_field_rights(current_user, "items", r.model_dump(), existing)
        allowed = store.IMPORT_FIELDS.get("fields") or []
        if existing and allowed:   # Global Settings → Bulk Item Import Fields: only these change on existing items
            row = {**existing, **{f: row[f] for f in allowed if f in row}}
        row.pop("authorised", None)
        security.mark_approval(current_user, "items", row)
        row["changed_by"] = current_user["user"]
        admin_audit.record("Item Master", "Imported" if existing else "Added", r.code,
                           *admin_audit.diff(existing, row | ({"qoh": existing["qoh"]} if existing else {})))
        store.ITEMS[r.code] = row
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
for _sp in SPECS:   # the simple masters described in master_specs.py
    crud(_sp["path"], getattr(store, _sp["store"]), SIMPLE_MODELS[_sp["path"]], _sp["title"], (_sp["title"],), _sp["store"])
crud("sales-persons", store.SALES_PERSONS, SalesPerson, "Sales Person Master", ("Sales Person Master",), "SALES_PERSONS")
