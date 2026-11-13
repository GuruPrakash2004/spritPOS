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

from .. import admin_audit, doc_engine, db_store, gsettings, schema_meta, security, sqlstore, store, validation
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
    """frmMstItem.ValidFields: an item's code, aliases, short name and hot key may not repeat among its own entries, and may not
    be another item's code, alias, short name or hot key (the find box and the barcode scan look all of them up)."""
    aliases = [a.strip() for a in body.aliases if a and a.strip()]
    for a in aliases:   # the exe's alias grid: 30 characters, no apostrophe
        if len(a) > 30:
            raise HTTPException(422, f"Alias '{a[:30]}...' is longer than 30 characters")
    seen: set[str] = set()
    for a in aliases:
        if a.casefold() in seen:
            raise HTTPException(422, f"Duplicate alias '{a}'")
        seen.add(a.casefold())
    if body.code.casefold() in seen:
        raise HTTPException(422, "Item code and alias cannot be the same")
    short, hot = (body.short_name or "").strip(), _extra(body.extras, "hotkey")
    own = {t.casefold() for t in [body.code, short, hot, *aliases] if t}
    for other_code, other in store.ITEMS.items():
        if other_code == (original or body.code):
            continue
        if other_code.casefold() in own:
            raise HTTPException(422, f"Duplicate item code: '{other_code}' is already an item's code")
        if own & {a.strip().casefold() for a in other.get("aliases", []) if a and a.strip()}:
            raise HTTPException(422, "Duplicate item alias: another item already uses it")
        if short and (other.get("short_name") or "").strip().casefold() == short.casefold() and other_code.casefold() != short.casefold():
            raise HTTPException(422, f"Duplicate short name '{short}' (item {other_code})")
        other_hot = _extra(other.get("extras"), "hotkey")
        if hot and other_hot and other_hot.casefold() == hot.casefold():
            raise HTTPException(422, f"Duplicate hot key '{hot}' (item {other_code})")


def _check_item(body: Item, original: str | None = None) -> None:
    if body.rol < 0 or body.roq < 0:
        raise HTTPException(422, "ROL and ROQ cannot be negative")
    if not (0 <= body.cgst_per <= 100 and 0 <= body.sgst_per <= 100):
        raise HTTPException(422, "CGST % and SGST % must be between 0 and 100")
    if not body.group.strip():
        raise HTTPException(422, "Please select an Item Group")
    if body.group not in store.ITEM_GROUPS:
        raise HTTPException(422, f"Unknown item group '{body.group}'")
    if store.ITEM_GROUPS[body.group].get("active") is False:
        raise HTTPException(422, f"Item group '{body.group}' is inactive")
    if not body.liqtype.strip():
        raise HTTPException(422, "Please select an Item Type")
    if body.liqtype.strip().casefold() not in {t.casefold() for t in ITEM_TYPES}:
        raise HTTPException(422, f"Not a valid Item Type (use {', '.join(ITEM_TYPES)})")
    if not body.uom.strip():
        raise HTTPException(422, "Please select a UOM")
    if body.uom not in store.UOMS:
        raise HTTPException(422, f"Unknown UOM '{body.uom}'")
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
    if not hasattr(body, "active") or "extras" not in body.model_fields_set:
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



def _clean(path: str, body: BaseModel, table: dict, original: str | None = None) -> BaseModel:
    """Request-side validation shared by create and update."""
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
            date.fromisoformat(body.code)
        except ValueError:
            raise HTTPException(422, "The holiday's date must be a valid date (yyyy-mm-dd)")
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
        body = _clean(path, body, table, original=code)
        if body.code != code:
            if body.code in table:
                raise HTTPException(409, f"{tag} '{body.code}' already exists")
            reason = IN_USE[path](code)
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
