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
from pydantic import BaseModel, Field

from .. import admin_audit, db, doc_engine, db_store, gsettings, schema_meta, security, sqlstore, store, validation
from ..master_specs import SPECS
from ..schemas import SIMPLE_MODELS
from ..schemas import (
    Brand, Company, CompanyOut, Customer, Item, ItemGroup, Ledger, LedgerGroup, Package, PriceUpdate, PriceSet, BulkItemUpdate, ItemBrandIn, SalesCounter, SalesPerson, Supplier, Uom,
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


ITEM_TYPES = ("Wine", "Beer", "IML", "Others")   # frmMstItem's Item Type list (cboType)


def _extra(extras: dict | None, column: str) -> str:
    """A database column the web has no typed field for (HotKey, InActiveRmks, ItemSell ...), from `extras`, any letter case."""
    for k, v in (extras or {}).items():
        if k.lower() == column:
            return str(v if v is not None else "").strip()
    return ""


def _item_used_in_trn(code: str) -> bool:
    """frmMstItem.CheckforItemUsedInTrn: the item is on any TrnDtl* table (bills, indents, returns ...) or has stock movements."""
    if _item_in_use(code):
        return True
    item_id = (store.ITEMS.get(code) or {}).get("_itemid")
    from .. import db
    if not item_id or not db.is_db_available():
        return False
    try:
        for t in db.fetchall("SELECT TABLE_NAME AS t FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ItemID' AND TABLE_NAME LIKE 'TrnDtl%'"):
            if db.fetchone(f"SELECT TOP 1 1 AS x FROM [{t['t']}] WITH (NOLOCK) WHERE ItemID = ?", (item_id,)):
                return True
    except Exception:   # a database that can't say: don't block the edit on a guess
        return False
    return False


def _check_item_names(body: Item, original: str | None) -> None:
    """frmMstItem.ValidFields (578-616): the item's own entries (alias grid) may not repeat or equal its code, and its code and aliases
    may not be another item's code, alias, short name or hot key. The exe builds the list it searches with only the code and the
    aliases (the short name and hot key are glued on without a comma, so they never match), and so does this check."""
    aliases = [a.strip() for a in body.aliases if a and a.strip()]
    for a in aliases:   # the exe's alias grid: 30 characters, no apostrophe
        if len(a) > 30:
            raise HTTPException(422, f"Alias '{a[:30]}...' is longer than 30 characters")
        if "'" in a:
            raise HTTPException(422, "An alias cannot contain an apostrophe")
    seen: set[str] = set()
    for a in aliases:
        if a.casefold() in seen:
            raise HTTPException(422, f"Duplicate alias '{a}'")
        seen.add(a.casefold())
    if body.code.strip().casefold() in seen:
        raise HTTPException(422, "ItemCode and Alias cannot be same")
    mine = {t.casefold() for t in [body.code.strip(), *aliases] if t}
    for other_code, other in store.ITEMS.items():
        if other_code == (original or body.code):
            continue
        if other_code.casefold() in mine:
            raise HTTPException(422, f"Duplicate Item Code: '{other_code}' is already an item's code")
        if mine & {a.strip().casefold() for a in other.get("aliases", []) if a and a.strip()}:
            raise HTTPException(422, "Duplicate Item Aliase")
        if (other.get("short_name") or "").strip().casefold() in mine:
            raise HTTPException(422, f"Duplicate ShortName (item {other_code})")
        other_hot = _extra(other.get("extras"), "hotkey")
        if other_hot and other_hot.casefold() in mine:
            raise HTTPException(422, f"Duplicate HotKey (item {other_code})")


def _check_item(body: Item, original: str | None = None) -> None:
    if body.rol < 0 or body.roq < 0:
        raise HTTPException(422, "ROL and ROQ cannot be negative")
    if not (0 <= body.cgst_per <= 100 and 0 <= body.sgst_per <= 100):
        raise HTTPException(422, "CGST % and SGST % must be between 0 and 100")
    if not body.group.strip():
        raise HTTPException(422, "Please select an Item Group")
    if body.group not in store.ITEM_GROUPS:
        raise HTTPException(422, f"Unknown item group '{body.group}'")
    if store.ITEM_GROUPS[body.group].get("active") is False or store.ITEM_GROUPS[body.group].get("authorised") is False:
        raise HTTPException(422, "Selected ItemGroup not found in master!")   # the exe's list holds active, authorised groups only
    if not body.liqtype.strip():
        raise HTTPException(422, "Please select an Item Type")
    if body.liqtype.strip().casefold() not in {t.casefold() for t in ITEM_TYPES}:
        raise HTTPException(422, f"Not a valid Item Type (use {', '.join(ITEM_TYPES)})")
    if not body.uom.strip():
        raise HTTPException(422, "Please select a UOM")
    if body.uom not in store.UOMS or body.uom.strip() == "-":
        raise HTTPException(422, "Selected UOMCode not found in the master !")
    if store.UOMS[body.uom].get("active") is False:
        raise HTTPException(422, f"UOM '{body.uom}' is inactive")
    if body.package and body.package not in store.PACKAGES:
        raise HTTPException(422, f"Unknown package '{body.package}'")
    if body.brand and body.brand not in store.BRANDS:
        raise HTTPException(422, f"Unknown brand '{body.brand}'")
    _check_item_names(body, original)
    if original and body.uom != (store.ITEMS.get(original) or {}).get("uom", body.uom) and _item_used_in_trn(original):
        raise HTTPException(409, "The UOM can't be changed: this item is used in transactions")


def _check_margin(row: dict) -> None:
    """Global Settings PurSellVarPer: MRP must exceed the purchase rate by at least this %. Checked on the row
    as it will be saved (with the stored purchase rate when the caller can't see or send it)."""
    pct = gsettings.number("PurSellVarPer")
    rate = row.get("purchase_rate") or 0
    if str(row.get("liqtype") or "").strip().upper() == "OTHERS":   # frmMstItem: the variation is not checked for the "Others" type
        return
    try:   # the desktop compares the item's selling rate (ItemSell) with the purchase rate; without one, the MRP
        sell = float(_extra(row.get("extras"), "itemsell") or 0) or row.get("mrp", 0)
    except ValueError:
        sell = row.get("mrp", 0)
    if pct > 0 and rate and sell < round(rate * (1 + pct / 100), 2):
        raise HTTPException(422, f"Purchase and Sales Variation ({round(sell / rate * 100 - 100, 2):g}%) of {row.get('code')} must be at least "
                                 f"{pct:g}% (selling rate ₹ {rate * (1 + pct / 100):.2f} or more; Global Settings → PurSellVarPer)")


def _check_inactive_remarks(path: str, body: BaseModel) -> None:
    """vLQMS.exe, every master with an InActive flag: a record made inactive needs remarks for why (the Not Active Remarks column, shown only
    while it is inactive); made active again, they are cleared. Only checked when the screen sent the database's extra columns (a client that
    does not know them can't be asked for one) and the master's desktop table really has the column."""
    if not hasattr(body, "active"):
        return
    if path == "items":   # frmMstItem.ValidFields (562): an inactive item always needs its remarks
        column = next((k for k in (body.extras or {}) if k.lower() == "inactivermks"), None)
        if body.active:
            if column:
                body.extras[column] = ""
        elif not (column and str(body.extras[column] or "").strip()):
            raise HTTPException(422, "Please enter remarks for why the item is inactive!")
        return
    if "extras" not in body.model_fields_set:
        return
    column = next((k for k in (body.extras or {}) if k.lower() == "inactivermks"), None)
    if body.active:
        if column:
            body.extras[column] = ""
        return
    if column and str(body.extras[column] or "").strip():
        return
    store_name = security.TABLES.get(path)
    if not sqlstore.enabled() or store_name not in sqlstore.LEGACY_MASTERS:
        return
    if "inactivermks" in {c["name"].lower() for c in schema_meta.columns(sqlstore.LEGACY_MASTERS[store_name][0])}:
        raise HTTPException(422, "Please enter remarks for why the record is inactive")



# The masters the exe draws with frmMstGeneral (and Holiday): their text boxes turn an apostrophe into a back-quote on save (stringReplace).
_BACKQUOTE = {"item-groups", "brands", "packages", "uoms", "holidays", "customers", "suppliers", *(sp["path"] for sp in SPECS)}


def _backquote(path: str, body: BaseModel) -> None:
    if path not in _BACKQUOTE:
        return
    for key, value in list(body.__dict__.items()):
        if isinstance(value, str) and "'" in value:
            setattr(body, key, value.replace("'", "`"))
    for k, v in list((body.extras or {}).items()):   # in place: assigning would mark `extras` as sent by the screen
        if isinstance(v, str) and "'" in v:
            body.extras[k] = v.replace("'", "`")


def _within_financial_year(day: date) -> bool:
    """svtModule.validDate: the date lies between the company's financial year start and end (true when the year is not known)."""
    start, end = store.COMPANY.get("fy_start") or "", store.COMPANY.get("fy_end") or ""
    return not (start and day.isoformat() < start[:10]) and not (end and day.isoformat() > end[:10])


def _is_numeric(text: str) -> bool:
    try:
        float(text.replace(",", ""))
        return True
    except ValueError:
        return False


def _master_used_elsewhere(code: str, path: str) -> bool:
    """frmMstGeneral.Editable: the master's ID appears in another table (same ID column name). In memory: the web's own in-use rule."""
    if IN_USE[path](code):
        return True
    return False


def _party_value(body: BaseModel, typed: str, column: str) -> str:
    """A party field the exe draws as a box: the web's typed field when it has one, else the database column carried in `extras`."""
    value = getattr(body, typed, "") if typed else ""
    return str(value or _extra(body.extras, column.lower()) or "").strip()


def _check_party(path: str, body: BaseModel, original: str | None) -> None:
    """frmMstParty.ValidFields. Checked when the screen sent the database's extra columns (it always does; a bare API call that carries only
    code, name and GSTIN is not asked for an address): first address line, a group from the group master (blank is refused, "-" allowed),
    and City, Country and State each present in State City Country."""
    kind = "Customer" if path == "customers" else "Supplier"
    if "extras" not in body.model_fields_set:
        return
    if not _party_value(body, "address", "badd1"):
        raise HTTPException(422, "Please enter atleast first line address!")
    group_col = "custgroupid" if path == "customers" else "suppgroupid"
    given = {k.lower(): v for k, v in (body.extras or {}).items()}
    if group_col in given:
        group = str(given[group_col] if given[group_col] is not None else "").strip()
        groups = store.CUSTOMER_GROUPS if path == "customers" else store.SUPPLIER_GROUPS
        known = group.isdigit() and int(group) > 0 or group == "-" or (group in groups and groups[group].get("active") is not False)
        if not known:
            raise HTTPException(422, f"{kind} Group Code  NOT found in the master !")
    scc = store.STATE_CITY_COUNTRY
    for label, typed, column, cities in (("City", "city", "city", {c.casefold() for c in scc}),
                                         ("Country", "country", "country", {str(r.get("country", "")).strip().casefold() for r in scc.values()}),
                                         ("State", "state", "state", {str(r.get("name", "")).strip().casefold() for r in scc.values()})):
        value = _party_value(body, typed, column)
        if not value:
            raise HTTPException(422, f"Please Enter The {label}!")
        if value.casefold() not in cities:
            raise HTTPException(422, f"{label} Not found in Master!")


def _clean(path: str, body: BaseModel, table: dict, original: str | None = None) -> BaseModel:
    """Request-side validation shared by create and update."""
    _backquote(path, body)
    if path in ("customers", "suppliers"):   # on the extras exactly as the screen sent them (before they are checked against the table)
        _check_party(path, body, original)
    body.code = _check_code(body.code)
    if hasattr(body, "name"):
        body.name = body.name.strip()
        if not body.name:
            raise HTTPException(422, "Name cannot be blank")
    if body.extras:   # columns added in the database / Doc Designer: checked against the real table
        store_name = security.TABLES.get(path)
        if store_name in sqlstore.LEGACY_MASTERS:
            try:
                body.extras = schema_meta.clean_extras(sqlstore.LEGACY_MASTERS[store_name][0], body.extras, sqlstore._mapped(store_name),
                                                       exclude=(sqlstore.LEGACY_MASTERS[store_name][2], original))
            except ValueError as e:
                raise HTTPException(422, str(e))
        else:
            body.extras = {}
    _check_inactive_remarks(path, body)
    skip = {"gstin"} if path == "customers" and body.gstin.strip() == "-" else set()   # '-' is the desktop's own "none" in a customer's GSTNO
    for key, value in validation.check(body.model_dump(), skip=skip).items():   # mobile / phone / PIN / GSTIN / PAN / HSN / e-mail / tax %
        if hasattr(body, key):
            setattr(body, key, value)
    if path == "items":
        _check_item(body, original)
    elif path == "state-city-country":   # frmMstSCC.ValidFields: the state must be in State Master (by name)
        if not any(str(s.get("name", "")).strip().casefold() == body.name.strip().casefold() for s in store.STATES.values()):
            raise HTTPException(422, "State Not found in Master!")
    elif path == "ledgers" and body.group not in store.LEDGER_GROUPS:
        raise HTTPException(422, f"Unknown ledger group '{body.group}'")
    elif path == "sales-counters" and not body.prefix.strip():
        raise HTTPException(422, "Bill prefix cannot be blank")
    elif path == "customers" and body.points < 0:
        raise HTTPException(422, "Points cannot be negative")
    elif path == "customers" and body.code != "CASH" and not body.gstin.strip():
        raise HTTPException(422, "GSTIN is mandatory for a customer")   # the desktop Customer Master marks GSTNO mandatory
    elif path == "customers" and body.code != "CASH" and not body.currency.strip():
        raise HTTPException(422, "Currency is mandatory for a customer")   # ... and CURID
    elif path == "holidays":
        try:
            day = date.fromisoformat(body.code)
        except ValueError:
            raise HTTPException(422, "The holiday's date must be a valid date (yyyy-mm-dd)")
        if not _within_financial_year(day):   # frmMstHoliday.ValidFields
            raise HTTPException(422, "Holiday Date Should be within the financial year")
        if _is_numeric(body.name):
            raise HTTPException(422, "Please Enter holiday in words")
    elif path == "currencies" and not body.decimals.strip():
        raise HTTPException(422, "Please enter the Decimal Name!")   # frmMstCurr.ValidFields (the exchange rate is required on the screen)
    elif path == "taxes" and not all(0 <= v <= 100 for v in (body.cgst_per, body.sgst_per, body.igst_per)):
        raise HTTPException(422, "Tax percentages must be between 0 and 100")
    elif path == "suppliers" and not body.gstin.strip():
        raise HTTPException(422, "GSTIN is mandatory for a supplier")   # the desktop Supplier Master marks GSTNO mandatory
    elif path == "taxes" and not body.tax_type.strip():
        raise HTTPException(422, "Tax Type is mandatory")
    elif path == "banks" and not body.display.strip():
        raise HTTPException(422, "Display Name in Invoice is mandatory")
    elif path == "packages" and body.litres_per_bottle <= 0:
        raise HTTPException(422, "Litres per bottle must be more than 0")
    return body


def _shown(path: str, row: dict, user: dict) -> dict:
    """A row as this user may see it: purchase rate hidden from roles without cost access, and fields hidden by
    the role's Field Level Rights (Admin Master (Security))."""
    row = {**row, "purchase_rate": None} if path == "items" and hides_cost(user) else row
    if path == "items" and hides_cost(user) and row.get("extras"):   # unknown columns may hold costs
        row = {**row, "extras": {}}
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
            def hit(r):   # the item find box (frmMstItem.txtSearch_KeyUp) also looks in ShortName and HotKey
                names = [r["code"], r.get("name", "")]
                if path == "items":
                    names += [r.get("short_name", ""), _extra(r.get("extras"), "hotkey"), *r.get("aliases", [])]
                return any(ql in str(n or "").lower() for n in names)
            rows = [r for r in rows if hit(r)]
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
        if path in ("customers", "suppliers") and any(str(r.get("name", "")).strip().casefold() == body.name.strip().casefold() for r in table.values()):
            raise HTTPException(409, f"{path[:-1].capitalize()} Name already exists..")
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
        if path == "brands" and _master_used_elsewhere(code, path):   # frmMstGeneral: Edit is refused for a brand that is in use
            raise HTTPException(409, "Editing is not available for this masters")
        if path == "holidays" and body.code != code:
            raise HTTPException(422, "A holiday's date cannot be changed: delete it and add the new date")   # frmMstHoliday: the date box is locked
        body = _clean(path, body, table, original=code)
        if body.code != code:
            if body.code in table:
                raise HTTPException(409, f"{tag} '{body.code}' already exists")
            reason = None if path == "items" else IN_USE[path](code)   # the exe lets an item be re-coded (bills point at its ID)
            if reason:
                raise HTTPException(409, f"Can't change the code of {tag} '{code}': {reason}")
        row = security.apply_field_rights(current_user, path, body.model_dump(), table[code])
        row.pop("authorised", None)
        if "extras" not in body.model_fields_set:   # a screen that doesn't know the database's extra columns keeps them
            row["extras"] = table[code].get("extras", {})
        if "aliases" in row and "aliases" not in body.model_fields_set:   # nor does it blank the item's aliases
            row["aliases"] = table[code].get("aliases", [])
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
        if body.code != code:
            store.RENAMED[(store_name, body.code)] = store.RENAMED.get((store_name, code), code)
        if path == "items" and body.code != code:
            store.recode_item(code, body.code)
        store.touch(store_name, code)
        store.touch(store_name, body.code)
        return _shown(path, row, current_user)

    @router.delete(f"/{path}/{{code}}", status_code=204, tags=[tag], name=f"Delete {tag}", dependencies=can_delete)
    def delete(code: str, current_user: dict = Depends(get_current_user)):
        if path == "items" and current_user.get("role") == "Cashier":
            raise HTTPException(403, "Cashiers are not permitted to delete items. Manager or Administrator approval required.")
        if code not in table:
            raise HTTPException(404, f"{tag} '{code}' not found")
        if path == "holidays":
            try:
                if not _within_financial_year(date.fromisoformat(code)):
                    raise HTTPException(422, "Holiday Date Should be with the financial year")
            except ValueError:
                pass
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


COMPANY_FREE_TEXT_COLUMNS = {"phone": ("telno", "Tel no."), "mobile": ("mobno", "Mobile no."), "excise_phone": ("phno", "Excise phone no.")}
COMPANY_FREE_TEXT = frozenset(COMPANY_FREE_TEXT_COLUMNS)


@router.put("/company", response_model=CompanyOut, tags=["Company Master"])
def update_company(body: Company, admin: dict = Depends(require_admin)):
    # Tel / mobile / excise phone are free text here (several numbers may be entered, as the desktop allows); only their length is held
    # to the column's, so nothing is silently cut when it is saved.
    for key, value in validation.check(body.model_dump(), skip=COMPANY_FREE_TEXT).items():
        setattr(body, key, value)
    if sqlstore.enabled():
        widths = {c["name"].lower(): c["max_length"] // (2 if str(c["type"]).startswith("n") else 1) for c in schema_meta.columns("MstComp") if c["max_length"] > 0}
        for key, (column, label) in COMPANY_FREE_TEXT_COLUMNS.items():
            if widths.get(column) and len(getattr(body, key) or "") > widths[column]:
                raise HTTPException(422, f"{label} can be at most {widths[column]} characters")
    if body.extras:   # MstComp columns beyond the web's own fields, checked against the table and the designer's rules
        try:
            body.extras = (schema_meta.clean_extras("MstComp", body.extras, db_store.COMPANY_COLUMNS | sqlstore._NOT_EXTRA)
                           if sqlstore.enabled() else {})
        except ValueError as e:
            raise HTTPException(422, str(e))
    for key in ("name", "email", "address", "address1", "address2", "address3"):   # frmMstComp.btnSave_Click: an apostrophe becomes a back-quote
        setattr(body, key, (getattr(body, key) or "").replace("'", "`"))
    body.extras = {k: (v.replace("'", "`") if isinstance(v, str) else v) for k, v in (body.extras or {}).items()}
    earlier, changed = admin_audit.diff(store.COMPANY, {**store.COMPANY, **body.model_dump()})
    if changed:
        admin_audit.record("Company Master", "Edited", store.COMPANY.get("name") or "Company", earlier, changed)
    changes = body.model_dump()
    if any(changes.get(k) for k in ("address1", "address2", "address3")):   # the three address rows are the truth; `address` is them joined
        changes["address"] = ", ".join(changes[k].strip() for k in ("address1", "address2", "address3") if changes.get(k, "").strip())
    if store.FIN_YEARS:   # the financial year is the desktop's (MstCompFinYear), not edited here
        changes.update(fy_start=store.COMPANY["fy_start"], fy_end=store.COMPANY["fy_end"])
    changes["extras"] = {**(store.COMPANY.get("extras") or {}), **(changes.get("extras") or {})}
    store.COMPANY.update(changes)
    store.touch("COMPANY", "*")
    store.set_fin_year()
    return _company()


class FinYearIn(BaseModel):
    start: date
    end: date


class DashboardLinkIn(BaseModel):
    link: str = Field("", max_length=500)


@router.put("/company/dashboard-link", tags=["Company Master"])
def update_dashboard_link(body: DashboardLinkIn, admin: dict = Depends(require_admin)):
    """Company Master's "Update Dashboard Link": MstComp.DashBoardLink (the page the desktop's dashboard shows)."""
    link = body.link.strip()
    if link and not re.match(r"^https?://", link, re.I):
        raise HTTPException(422, "Enter a web address starting with http:// or https://")
    if "dashboardlink" not in {c["name"].lower() for c in schema_meta.columns("MstComp")}:
        raise HTTPException(409, "This database's company table has no DashBoardLink column")
    store.COMPANY.setdefault("extras", {})["DashBoardLink"] = link
    store.touch("COMPANY", "*")
    admin_audit.record("Company Master", "Edited", "Dashboard link", "", link)
    return _company()


@router.get("/company/fin-years/next", tags=["Company Master"])
def next_fin_year():
    """The dates the Company Master offers for a new financial year: a year after the last one (else 1 April this year)."""
    years = sorted(store.FIN_YEARS, key=lambda y: y["start"])
    if years:
        last = years[-1]
        s0, e0 = date.fromisoformat(last["start"]), date.fromisoformat(last["end"])
        def a_year_on(d):
            try:
                return date(d.year + 1, d.month, d.day)
            except ValueError:   # 29 February
                return date(d.year + 1, d.month, 28)
        return {"start": a_year_on(s0).isoformat(), "end": a_year_on(e0).isoformat()}
    return {"start": date(date.today().year, 4, 1).isoformat(), "end": date(date.today().year + 1, 3, 31).isoformat()}


@router.post("/company/fin-years", status_code=201, tags=["Company Master"])
def create_fin_year(body: FinYearIn, admin: dict = Depends(require_admin)):
    """vLQMS.exe btnCreate_Click: refused when the start date already falls inside an existing financial year; created inactive."""
    if body.end <= body.start:
        raise HTTPException(422, "The financial year must end after it starts")
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    for y in store.FIN_YEARS:
        if date.fromisoformat(y["start"]) <= body.start <= date.fromisoformat(y["end"]):
            raise HTTPException(409, "Financial Year already exists!")
    from .. import db
    with db.transaction() as cur:
        cur.execute("INSERT INTO dbo.MstCompFinYear (yrFdate, yrTdate, Active) VALUES (?, ?, 'N')", (body.start.isoformat(), body.end.isoformat()))
    admin_audit.write_now(admin["user"], "Company Master", "Added", "Financial year", "", f"{body.start} to {body.end}")
    db_store.load_fin_year()
    return _company()


@router.delete("/company/fin-years/{start}", status_code=200, tags=["Company Master"])
def remove_fin_year(start: date, admin: dict = Depends(require_admin)):
    """vLQMS.exe btnRemoveFinYr_Click: at least one year must remain, only the first or the last can go, never the active one."""
    years = sorted(store.FIN_YEARS, key=lambda y: y["start"])
    target = next((y for y in years if y["start"] == start.isoformat()), None)
    if target is None:
        raise HTTPException(404, "Invalid year")
    if len(years) <= 1:
        raise HTTPException(409, "Atleast 1 active financial year must exist!")
    if target not in (years[0], years[-1]):
        raise HTTPException(409, "Only first and last row can be deleted!")
    if target["active"]:
        raise HTTPException(409, "Unable to remove as this financial year is currently in use!")
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    from .. import db
    with db.transaction() as cur:
        cur.execute("DELETE FROM dbo.MstCompFinYear WHERE yrFdate = ? AND yrTdate = ?", (target["start"], target["end"]))
    admin_audit.write_now(admin["user"], "Company Master", "Deleted", "Financial year", f"{target['start']} to {target['end']}", "")
    db_store.load_fin_year()
    return _company()


@router.put("/company/fin-years/{start}/activate", status_code=200, tags=["Company Master"])
def activate_fin_year(start: date, admin: dict = Depends(require_admin)):
    """Make a financial year (e.g. the previous one) the active one: exactly one year is Active in MstCompFinYear, and the
    active year gives document numbers their prefix. It is the same for every user until another year is made active."""
    target = next((y for y in store.FIN_YEARS if y["start"] == start.isoformat()), None)
    if target is None:
        raise HTTPException(404, "Invalid year")
    if target["active"]:
        return _company()
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    from .. import db
    was = next((y for y in store.FIN_YEARS if y["active"]), None)
    with db.transaction() as cur:
        cur.execute("UPDATE dbo.MstCompFinYear SET Active = CASE WHEN yrFdate = ? AND yrTdate = ? THEN 'Y' ELSE 'N' END",
                    (target["start"], target["end"]))
    admin_audit.write_now(admin["user"], "Company Master", "Changed", "Active financial year",
                          f"{was['start']} to {was['end']}" if was else "", f"{target['start']} to {target['end']}")
    db_store.load_fin_year()
    return _company()


class CarryForwardIn(BaseModel):
    admin_password: str | None = Field(None, max_length=1000, description='Encrypted for "admin": the additional ADMIN password, or your own while none is set')


@router.get("/company/fin-years/{start}/carry-forward", tags=["Company Master"])
def carry_forward_preview(start: date, admin: dict = Depends(require_admin)):
    """What carrying the previous year's closing stock into this year would write (nothing is written)."""
    from .. import stock_carry_forward
    return stock_carry_forward.preview(start.isoformat())


@router.post("/company/fin-years/{start}/carry-forward", tags=["Company Master"])
def carry_forward_stock(start: date, body: CarryForwardIn, admin: dict = Depends(require_admin)):
    """Stock Balance's "Stock Carry Forward": the previous year's closing stock becomes this year's opening stock (MstOpBalance, the
    'OP' stock rows) and the live balance (TrnQOH) is rebuilt. Needs the ADMIN password; back the database up first."""
    from .. import stock_carry_forward
    security.check_admin_password(admin, body.admin_password)
    return stock_carry_forward.carry_forward(start.isoformat(), admin["user"])


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


# Typed Item Master fields the rate columns can name (key -> desktop column); the rest of the numeric designer fields are the item's extras.
_RATE_TYPED = {"mrp": "MRP", "purchase_rate": "PURRATE", "cgst_per": "CGSTPER", "sgst_per": "SGSTPER", "rol": "ROL", "roq": "ROQ"}
_RATE_FALLBACK = [("mrp", "MRP"), ("purchase_rate", "Item Purchase Rate"), ("cgst_per", "CGST %"), ("sgst_per", "SGST %"), ("rol", "Reorder Level"), ("roq", "Reorder Qty")]


def _rate_columns(user: dict) -> list[dict]:
    """The Item Rate Columns list of Price List Maintenance (frmMstPriceMaintenance.LoadSelItemCol): the Item Master's numeric fields from the Doc
    Designer (UtlDocDsgrFlds, FldDataType Numeric, not an ID), as {key, caption, typed}. `key` is the web field for a typed column, else the
    column's own name (it lives in the item's extras). The purchase rate is left out for a role with no cost access."""
    from .. import designer
    doc_id = doc_engine.doc_id_of("Item Master")
    wanted = [f for f in (designer.fields_for(doc_id) if doc_id else []) if str(f.get("FldDataType") or "").lower() == "numeric" and not str(f.get("FldCode") or "").upper().endswith("ID")]
    by_col = {v: k for k, v in _RATE_TYPED.items()}
    out = []
    for f in wanted:
        col = str(f["FldCode"]).strip()
        typed = by_col.get(col.upper())
        out.append({"key": typed or col, "caption": str(f.get("FldCaption") or col).strip(), "typed": bool(typed)})
    if not out:
        out = [{"key": k, "caption": c, "typed": True} for k, c in _RATE_FALLBACK]
    if not can_see_cost(user):
        out = [c for c in out if c["key"] != "purchase_rate"]
    return out


@router.get("/items/rate-columns", tags=["Item Price List Maintenance"])
def rate_columns(current_user: dict = Depends(get_current_user)):
    return _rate_columns(current_user)


@router.post("/items/price-set", tags=["Item Price List Maintenance"], dependencies=[Depends(require_right("edit", "Price List Maintenance"))])
def set_rates(body: PriceSet, current_user: dict = Depends(get_current_user)):
    """Update to DB (frmMstPriceMaintenance.btnUpdtToDB): set the chosen rate columns of each item. Every row is checked first and nothing changes
    unless all pass: the item and the column must exist, a rate may not be negative, a new MRP must be above zero, a column that is read-only for
    the role (Admin Master field rights) or the cost (no cost access) is refused. Changed items of a master that needs approval wait for it
    again; each change is audited."""
    allowed = {c["key"]: c for c in _rate_columns(current_user)}
    rights = security.field_rights(current_user, "items")
    plan = []
    for r in body.rows:
        item = store.ITEMS.get(r.item_code)
        if not item:
            raise HTTPException(404, f"Unknown item '{r.item_code}'")
        for key, new in r.values.items():
            if key not in allowed:
                raise HTTPException(422, f"'{key}' is not a rate column you can change")
            if new < 0:
                raise HTTPException(422, f"Rates cannot go Negative ({r.item_code})")
            if key == "mrp" and new <= 0:
                raise HTTPException(422, f"Price cannot be negative or zero. Please enter a valid Price ({r.item_code})")
            if allowed[key]["typed"] and rights.get(key):
                raise HTTPException(403, f"You may not change {key.replace('_', ' ')} (Field Level Rights for your role)")
            plan.append((item, key, round(new, 2), allowed[key]["typed"]))
    changed, touched = 0, {}
    for item, key, new, typed in plan:
        holder = item if typed else item.setdefault("extras", {})
        old = holder.get(key)
        if old is not None and round(float(old or 0), 2) == new:
            continue
        holder[key] = new
        admin_audit.record("Price List Maintenance", "Edited", item["code"], f"{key}: {old}", f"{key}: {new}")
        changed += 1
        touched[item["code"]] = item
    for item in touched.values():
        security.mark_approval(current_user, "items", item)
        item["changed_by"] = current_user["user"]
        store.touch("ITEMS", item["code"])
    return {"items": len(body.rows), "changed": changed}


# Fields Bulk Master Update can set (the desktop's UtlItemBulkUpdate lists these as HSNCode, BrandID, LiqType, BotPerCases, MRP,
# PURRATE, ROL, ROQ); its other entries (min/max level, KSBCL names, ...) have no field in the web Item Master.
BULK_FIELDS = {"hsn_code": str, "brand": str, "liqtype": str, "bottles_per_case": int, "mrp": float, "purchase_rate": float,
               "rol": float, "roq": float}


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


@router.post("/items/brands", tags=["Item Vs Brand"], dependencies=[Depends(require_right("edit", "Item Vs Brand"))])
def assign_item_brands(body: ItemBrandIn, current_user: dict = Depends(get_current_user)):
    """Link items to brands (an empty brand unlinks). Like Bulk Master Update, Field Level Rights for the brand apply, a master that
    needs approval waits again, and each change goes to the ADMIN Audit Log. Nothing changes unless every row passes."""
    if security.field_rights(current_user, "items").get("brand"):
        raise HTTPException(403, "You may not change brand (Field Level Rights for your role)")
    changes = {}
    for r in body.assignments:
        item, brand = store.ITEMS.get(r.item_code), r.brand.strip()
        if not item:
            raise HTTPException(404, f"Unknown item '{r.item_code}'")
        if brand and brand not in store.BRANDS:
            raise HTTPException(422, f"Unknown brand '{brand}'")
        changes[item["code"]] = brand
    done = 0
    for code, brand in changes.items():
        item = store.ITEMS[code]
        if (item.get("brand") or "") == brand:
            continue
        old = item.get("brand") or ""
        item["brand"] = brand
        security.mark_approval(current_user, "items", item)
        item["changed_by"] = current_user["user"]
        store.touch("ITEMS", code)
        admin_audit.record("Item Vs Brand", "Edited", code, f"brand: {old}", f"brand: {brand}")
        done += 1
    return {"changed": done}


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


# ─── Arrange Group Slno / Arrange Pkg Slno (frmUtlGrpOrderBy / frmUtlPkgOrderBy, Options of the Item Group and Package masters) ───

_ORDERS = {   # path -> (table, id column, order column, code column, extra select)
    "item-groups": ("MstItemGroup", "ItemGroupID", "GroupOrderBy", "ItemGroupCode", ""),
    "packages": ("MstPkg", "PkgID", "PkgOrderBy", "PkgCode", ", LtrPerBot AS litres"),
}


class OrderSave(BaseModel):
    ids: list[int] = Field(min_length=1, max_length=5000, description="The master IDs in their new order (Slno 1, 2, 3 ...)")


def _order_rows(path: str, sort: str = "") -> list[dict]:
    table, id_col, order_col, code_col, extra = _ORDERS[path]
    by = {"asc": "LtrPerBot", "dsc": "LtrPerBot DESC"}.get(sort) if path == "packages" else None
    if not db.is_db_available():
        raise HTTPException(503, "This needs the shop's database")
    rows = db.fetchall(f"SELECT {id_col} AS id, {order_col} AS slno, {code_col} AS code{extra} FROM {table} WHERE {id_col} > 0 ORDER BY {by or order_col}")
    return [{"id": r["id"], "slno": i, "code": str(r["code"]).strip(), **({"litres": float(r["litres"] or 0)} if "litres" in r else {})} for i, r in enumerate(rows, 1)]


@router.get("/{path}/order", tags=["Item Master"])
def get_order(path: str, sort: str = "", current_user: dict = Depends(get_current_user)):
    """The Slno grid: current order (sort empty), or a package list by litres ascending ("asc") / descending ("dsc")."""
    if path not in _ORDERS:
        raise HTTPException(404, "Not found")
    return _order_rows(path, sort)


@router.post("/{path}/order", tags=["Item Master"], dependencies=[Depends(require_right("edit", "Item Group Master", "Package Master"))])
def save_order(path: str, body: OrderSave, current_user: dict = Depends(get_current_user)):
    """Save: `Update <table> Set <order column> = Slno Where ID = ...` for every row of the grid, in one transaction (the exe updates one by one)."""
    if path not in _ORDERS:
        raise HTTPException(404, "Not found")
    table, id_col, order_col, _code, _extra = _ORDERS[path]
    known = {r["id"] for r in _order_rows(path)}
    if set(body.ids) - known:
        raise HTTPException(422, "The list contains an entry that is not in the master")
    with db.direct_writes():
        with db.transaction() as cur:
            for slno, row_id in enumerate(body.ids, 1):
                cur.execute(f"UPDATE {table} SET {order_col} = ? WHERE {id_col} = ?", (slno, row_id))
    admin_audit.record("Item Group Master" if path == "item-groups" else "Package Master", "Edited", "Arrange Slno", "", f"{len(body.ids)} rows")
    return {"saved": len(body.ids)}


# ─── Item Master options and helpers (frmMstItem) ─────────────────────────────────────────────────────────────────────

_IDENT = re.compile(r"^[A-Za-z_][A-Za-z0-9_]*$")


def _rows_json(rows: list[dict]) -> list[dict]:
    out = []
    for r in rows:
        out.append({k: (v.isoformat() if hasattr(v, "isoformat") else (float(v) if hasattr(v, "as_tuple") else v)) for k, v in r.items()})
    return out


@router.get("/items/invalid", tags=["Item Master"])
def invalid_items(current_user: dict = Depends(get_current_user)):
    """Options → Items-DataLink Problem (frmRPtInvalidItems): `select * from vwInvalidItems order by 1,2`, the database view of items whose
    links (group, UOM, package, brand ...) point at nothing. Empty when there is no database or the view is not in it."""
    if not db.is_db_available():
        return {"columns": [], "rows": [], "note": "No database is connected."}
    try:
        rows = _rows_json(db.fetchall("SELECT * FROM vwInvalidItems ORDER BY 1, 2"))
    except Exception:
        return {"columns": [], "rows": [], "note": "This database has no vwInvalidItems view."}
    return {"columns": list(rows[0].keys()) if rows else [], "rows": rows, "note": ""}


@router.get("/{path}/similar", tags=["Item Master"])
def similar_codes(path: str, code: str, exclude: str = "", current_user: dict = Depends(get_current_user)):
    """The "Similar <code> already exists. Do you want to view them?" list of the exe's masters: rows whose <Code>Clean column (the code without
    non-alphanumeric characters, dbo.RemoveNonAlphaCharacters) equals the typed code's, other than the record being edited. Without the
    database, the same comparison on the loaded rows."""
    store_name = security.TABLES.get(path)
    if not store_name or store_name not in sqlstore.LEGACY_MASTERS:
        raise HTTPException(404, "Not found")
    clean = re.sub(r"[^A-Za-z0-9]", "", code or "").casefold()
    if not clean:
        return []
    web_table, id_col, code_col, _values = sqlstore.LEGACY_MASTERS[store_name]
    if db.is_db_available() and _IDENT.match(web_table) and _IDENT.match(code_col):
        name_col = code_col[:-4] + "NAME" if code_col.upper().endswith("CODE") else code_col
        for cols in (f"{code_col} AS code, {name_col} AS name", f"{code_col} AS code, {code_col} AS name"):
            try:
                rows = db.fetchall(f"SELECT {cols} FROM {web_table} WITH (NOLOCK) WHERE {code_col}Clean = dbo.RemoveNonAlphaCharacters(?) "
                                   f"AND {code_col} <> ? AND {id_col} > 0", (code.strip(), exclude.strip()))
                return [{"code": str(r["code"]).strip(), "name": str(r["name"] or "").strip()} for r in rows]
            except Exception:   # no Clean column, or no name column: try the next form, then compare here
                continue
    table = getattr(store, store_name)
    return [{"code": c, "name": r.get("name", "")} for c, r in table.items()
            if c != exclude and re.sub(r"[^A-Za-z0-9]", "", str(c)).casefold() == clean]


@router.get("/items/{code}/locks", tags=["Item Master"])
def item_locks(code: str, current_user: dict = Depends(get_current_user)):
    """Edit mode of the exe: the UOM box is disabled when the item is on any TrnDtl* table (CheckforItemUsedInTrn)."""
    if code not in store.ITEMS:
        raise HTTPException(404, f"Item '{code}' not found")
    return {"uom_locked": _item_used_in_trn(code)}


def _bulk_field_defs() -> list[dict]:
    """Options → Update Item Master Fields, field list: `select Distinct FieldName, Datatype from UtlItemBulkupdate order by FieldName`."""
    if not db.is_db_available():
        return []
    try:
        rows = db.fetchall("SELECT DISTINCT FieldName, Datatype FROM UtlItemBulkupdate ORDER BY FieldName")
    except Exception:
        return []
    return [{"code": str(r["FieldName"]).strip(), "datatype": str(r["Datatype"] or "").strip().upper()} for r in rows if str(r["FieldName"] or "").strip()]


@router.get("/items/bulk-fields", tags=["Item Master"])
def bulk_fields(current_user: dict = Depends(get_current_user)):
    return _bulk_field_defs()


class BulkFieldsLoad(BaseModel):
    fields: list[str] = Field(min_length=1)
    groups: list[str] = Field(default_factory=list)      # item group codes
    items: list[str] = Field(default_factory=list)       # item codes
    packages: list[str] = Field(default_factory=list)    # package codes


def _id_master(col: str) -> tuple[str, str]:
    """The exe's rule for a field whose name holds "ID": the master is MST + the name without "ID", its code column that + "Code"."""
    stem = col.replace("ID", "")
    if not _IDENT.match(stem):
        raise HTTPException(422, f"Field {col} cannot be handled")
    return f"Mst{stem}", f"{stem}Code"


@router.post("/items/bulk-fields/load", tags=["Item Master"], dependencies=[Depends(require_right("edit", "Item Master"))])
def bulk_fields_load(body: BulkFieldsLoad, current_user: dict = Depends(get_current_user)):
    """Template / Load to grid (frmUtlImportAnyFld.btnTemplate_Click / btnLoadtoGrd_Click): ItemID, ItemCode, ItemName and the chosen fields for the
    chosen items (a field ending in ID shows its master's code), ordered by ItemCode. 404 when nothing matches ("No Records Found!")."""
    allowed = {f["code"]: f for f in _bulk_field_defs()}
    for f in body.fields:
        if f not in allowed:
            raise HTTPException(422, f"'{f}' is not an updatable Item Master field")
    if "PURRATE" in {f.upper() for f in body.fields} and not can_see_cost(current_user):
        raise HTTPException(403, "You may not see the purchase rate (no cost access for your role)")
    cols = []
    for f in body.fields:
        if "ID" in f:
            tbl, code_col = _id_master(f)
            cols.append(f"(SELECT {code_col} FROM {tbl} WHERE {f} = MstItem.{f}) AS {f}")
        elif _IDENT.match(f):
            cols.append(f"MstItem.{f} AS {f}")
        else:
            raise HTTPException(422, f"Field {f} cannot be handled")
    sql, params = f"SELECT ItemID, ItemCode, ItemName, {', '.join(cols)} FROM MstItem WHERE ItemID <> 0", []
    for names, column, table, code_col in ((body.groups, "ItemGroupID", "MstItemGroup", "ItemGroupCode"), (body.items, "ItemID", "MstItem", "ItemCode"),
                                           (body.packages, "PKGID", "MstPKG", "PKGCode")):
        if names:
            sql += f" AND MstItem.{column} IN (SELECT {column} FROM {table} WHERE {code_col} IN ({', '.join('?' * len(names))}))"
            params += names
    rows = _rows_json(db.fetchall(sql + " ORDER BY ItemCode", tuple(params)))
    if not rows:
        raise HTTPException(404, "No Records Found!")
    return {"columns": ["ItemID", "ItemCode", "ItemName", *body.fields], "rows": rows}


class BulkFieldsImport(BaseModel):
    rows: list[dict] = Field(min_length=1, max_length=20000, description="{ItemID, <field>: value, ...}")


@router.post("/items/bulk-fields/import", tags=["Item Master"], dependencies=[Depends(require_right("edit", "Item Master"))])
def bulk_fields_import(body: BulkFieldsImport, current_user: dict = Depends(get_current_user)):
    """Import Data (frmUtlImportAnyFld.btnImportData_Click): for each row, update MstItem's chosen columns where ItemID matches: TEXT as typed,
    INTEGER / NUMERIC as numbers (a field ending in ID is looked up by its master's code), DATE as a date. The exe runs the updates one by one
    with no transaction (it is commented out); here they run in one, so a failure leaves nothing half changed."""
    if not db.is_db_available():
        raise HTTPException(503, "This needs the shop's database")
    defs = {f["code"].upper(): f for f in _bulk_field_defs()}
    updates = []
    for n, row in enumerate(body.rows, 1):
        try:
            item_id = int(float(row.get("ItemID") or 0))
        except (TypeError, ValueError):
            item_id = 0
        if item_id <= 0:
            raise HTTPException(422, f"Row {n}: ItemID is missing")
        sets, args = [], []
        for col, value in row.items():
            if col in ("ItemID", "ItemCode", "ItemName") or col.upper() not in defs:
                continue
            if not _IDENT.match(col):
                raise HTTPException(422, f"Row {n}: {col} cannot be handled")
            kind = defs[col.upper()]["datatype"]
            if col.upper() == "PURRATE" and not can_see_cost(current_user):
                raise HTTPException(403, "You may not change the purchase rate (no cost access for your role)")
            text = "" if value is None else str(value).strip()
            if kind == "TEXT":
                sets.append(f"{col} = ?")
                args.append(text)
            elif kind in ("INTEGER", "NUMERIC"):
                if "ID" in col.upper():
                    if text == "":
                        found = 0
                    else:
                        tbl, code_col = _id_master(col)
                        r = db.fetchone(f"SELECT {col} AS v FROM {tbl} WHERE {code_col} = ?", (text,))
                        if not r:
                            raise HTTPException(422, f"Row {n}: '{text}' is not a {col.replace('ID', '')} code")
                        found = r["v"]
                    sets.append(f"{col} = ?")
                    args.append(found)
                else:
                    try:
                        num = float(text.replace(",", "") or 0)
                    except ValueError:
                        num = 0.0
                    sets.append(f"{col} = ?")
                    args.append(int(num) if kind == "INTEGER" else num)
            elif kind == "DATE":
                sets.append(f"{col} = ?")
                args.append(text or None)
        if sets:
            updates.append((item_id, sets, args))
    if not updates:
        raise HTTPException(422, "There is nothing to import")
    with db.direct_writes():
        with db.transaction() as cur:
            for item_id, sets, args in updates:
                cur.execute(f"UPDATE MstItem SET {', '.join(sets)} WHERE ItemID = ?", (*args, item_id))
    admin_audit.record("Update Item Master Fields", "Edited", f"{len(updates)} items", "", ", ".join(sorted({x.split(' =')[0] for _, ss, _a in updates for x in ss})))
    db_store.load_items()
    return {"updated": len(updates)}


@router.post("/items/recalculate-rates", tags=["Item Master"], dependencies=[Depends(require_right("edit", "Item Master"))])
def recalculate_rates(current_user: dict = Depends(get_current_user)):
    """Options → Update Item Rates (frmMstItem.updateItemRates): every Item Master designer field that has a formula (UtlDocDsgrFlds.FldFormula) is
    recalculated for all items, as `Update MstItem set <field> = <formula>`: '#' removed, SVTROUND2( ) / SVTROUND4( ) become ROUND( , 2 / 4 ).
    The exe's own statement ends `where  and ItemID>0`, which SQL Server rejects, so it never ran; this does what it meant. Items whose UITCCGST /
    UITCSGST are NULL get 0 first (the exe's ALTER TABLE for those columns is not done: the web app never changes the shop's table design)."""
    if not db.is_db_available():
        raise HTTPException(503, "This needs the shop's database")
    doc_id = doc_engine.doc_id_of("Item Master")
    if not doc_id:
        raise HTTPException(422, "Invalid Item Master DocID!")
    from .. import designer
    have = {c["name"].upper() for c in schema_meta.columns("MstItem")}
    todo = []
    for f in designer.fields_for(doc_id):
        formula = str(f.get("FldFormula") or "").strip()
        col = str(f.get("FldCode") or "").strip()
        if not formula or not col:
            continue
        if col.upper() not in have or not _IDENT.match(col):
            raise HTTPException(422, f"The formula field {col} is not a column of MstItem")
        formula = formula.replace("#", "")
        head = formula[:9]
        if head in ("SVTROUND2", "SVTROUND4"):
            formula = formula.replace(head, "ROUND")
            formula = formula[:-1] + f",{head[-1]})"
        words = set(re.findall(r"[A-Za-z_][A-Za-z0-9_]*", formula))
        if not re.fullmatch(r"[\w\s.+\-*/(),]+", formula) or any(w.upper() not in have and w.upper() not in ("ROUND", "ISNULL", "ABS", "CEILING", "FLOOR") for w in words):
            raise HTTPException(422, f"The formula of {col} cannot be run safely: {formula}")
        todo.append((col, formula))
    if not todo:
        return {"fields": 0, "message": "No Item Master field has a formula."}
    with db.direct_writes():
        with db.transaction() as cur:
            for col in ("UITCCGST", "UITCSGST"):
                if col in have:
                    cur.execute(f"UPDATE MstItem SET {col} = 0 WHERE {col} IS NULL")
            for col, formula in todo:
                cur.execute(f"UPDATE MstItem SET {col} = {formula} WHERE ItemID > 0")
    admin_audit.record("Item Master", "Edited", "Update Item Rates", "", ", ".join(c for c, _ in todo))
    db_store.load_items()
    return {"fields": len(todo), "message": "Item Rates Updated/Recalculated!"}


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
