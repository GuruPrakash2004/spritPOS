"""In-memory store: every table the API serves, as module-level dicts.

Empty until the shop's SQL Server is loaded: at startup db_store.load_masters() fills the masters (ITEMS,
CUSTOMERS, SUPPLIERS, USERS, COUNTERS, LEDGERS, ...) from the Web_ tables, sqlstore.load() lays the web app's
own saved rows over them, and load_transactions() fills DOCS with recent invoices and bills. There is no
sample data (the tests install theirs from tests/seed.py). Every change is saved by app/sqlstore.py.

Key rules:
- Change stock only through move_stock(): it updates ITEMS[code]["qoh"] and logs a
  MOVEMENTS row, which the Stock Ledger, Stock Balance and DSR reports read.
- Documents live in DOCS[kind][doc_no]; numbers come from next_doc_no(). DB-loaded documents
  carry "origin" ("db"); documents made through the app have none, and are the ones
  app/sqlstore.py saves to SQL Server.
- Call touch(TABLE, key) after changing a master row, user or role, so the change is saved.
- BASE_QOH is each item's QOH where the app's own movements start (MOVEMENTS[APP_MOVEMENTS_FROM:]), so
  QOH must always equal BASE_QOH + those movements; Tools → Re Write Stock checks and restores that.
  Set both with mark_app_start(), never one without the other.
- ROLES maps a role to the module names it may open (the React sidebar names; "*" = all).
"""

from datetime import date

# The shop's company profile (Web_MstComp, Web_MstCompFinYear and Company Master edits). Blank until loaded.
COMPANY = {
    "name": "", "email": "", "address": "", "phone": "", "fax": "", "mobile": "", "website": "", "gstin": "",
    "pan": "", "cin": "", "excise_phone": "", "ksbcl_code": "", "licence_type": "", "state": "",
    "fy_start": "", "fy_end": "",
}

# Financial years (Web_MstCompFinYear): [{"start", "end", "active"}], newest first.
FIN_YEARS: list[dict] = []

# Masters, code -> row. All empty until db_store.load_masters() fills them from the Web_ tables.
ITEM_GROUPS: dict[str, dict] = {}
UOMS: dict[str, dict] = {}
PACKAGES: dict[str, dict] = {}
BRANDS: dict[str, dict] = {}
ITEMS: dict[str, dict] = {}
SUPPLIERS: dict[str, dict] = {}
CUSTOMERS: dict[str, dict] = {}
LEDGER_GROUPS: dict[str, dict] = {}
LEDGERS: dict[str, dict] = {}
COUNTERS: dict[str, dict] = {}
SALES_PERSONS: dict[str, dict] = {}

# App users (Web_MstUser, plus what the web app saved). Passwords are kept only as salted PBKDF2 hashes
# (app/passwords.py), never in plain text. "pwv" (password version) goes up on every password change, which
# invalidates older login tokens.
USERS: dict[str, dict] = {}

# Role → module names it may open (the React sidebar names). "*" means every module.
# Legacy equivalent: frmMstRole + frmUtlAssignRights.
ADMIN_ROLE = "Administrator"
ROLES = {
    ADMIN_ROLE: ["*"],
    "Manager": [
        "Dashboard", "Item Master", "Price List Maintenance", "Item Group Master", "UOM Master", "Supplier Master",
        "Customer Master", "Ledger Master", "Ledger Group Master", "Sales Person Master", "Package Master", "Brand Master",
        "Purchase Bill", "Sales Invoice", "Expense Entry", "Stock Deduction", "Purchase Indent", "Sales Return",
        "Closing Stock Entry", "Cash Denomination", "Daily Summary Report", "Stock Balance Report", "Stock Ledger",
        "Stock Ageing", "Profit Margin", "Hourly Sales", "Daily Sales", "Brand-wise Sales", "Day-wise Litres",
        "Stock in Litres", "Stock Level / Reorder", "Reports Dashboard",
    ],
    "Cashier": ["Dashboard", "Item Master", "Sales Invoice", "Sales Return", "Cash Denomination", "Closing Stock Entry", "Stock Balance Report"],
}

# Day-end cash counts, keyed by ISO date.
DENOMINATIONS: dict[str, dict] = {}

# When each item was last received (Web_TrnHdrPB/Web_TrnDtlPB), for Stock Ageing; purchases update it.
LAST_PURCHASE: dict[str, str] = {}

SETTINGS = {
    "loyalty": {"points_per_rupee": 0.01, "alert_when_points_above": 500, "min_bill_amount": 1000},
    "invoice": {"salesperson_required": True, "print_preview": False, "barcode_scan_jumps_to_qty": True},
}
# Global Settings rows (legacy UtlGSettings), code -> row; seeded and managed by app/gsettings.py, which also
# keeps SETTINGS above (the older /settings shape) in step with them.
GSETTINGS: dict[str, dict] = {}

# Transactions: doc type -> {doc_no: doc}
DOCS: dict[str, dict[str, dict]] = {k: {} for k in [
    "purchase_indent", "purchase_bill", "sales_invoice", "sales_return", "expense", "stock_deduction", "closing_stock",
]}

# Every stock-affecting line is logged here to drive Stock Ledger / Balance / DSR.
MOVEMENTS: list[dict] = []
# Index into MOVEMENTS where in-app activity starts once live DB data is loaded (set by db_store).
DB_BASE_MOVEMENTS = 0
# Index into MOVEMENTS where movements made through this app start (after the DB
# load). app/sqlstore.py saves MOVEMENTS[APP_MOVEMENTS_FROM:] (Web_AppMove) and replays them after a restart.
APP_MOVEMENTS_FROM = 0
# Each item's QOH at that index (items created later in the app start from 0). See mark_app_start().
BASE_QOH: dict[str, float] = {}

# Master rows changed through the app, per table name ("ITEMS", "USERS", "ROLES", "COMPANY", ...).
# app/sqlstore.py saves these rows (or their deletion) and lays them back over the DB data on start.
EDITS: dict[str, set[str]] = {}

# ADMIN Audit Log entries of the request that just succeeded, written with its changes by sqlstore.save()
# (app/admin_audit.py fills it).
AUDIT_OUT: list[dict] = []

# Replies to recent bill saves, per (user, path, Idempotency-Key header): a double-click or a retry of
# the same save gets the first reply back instead of creating a second bill (main.py).
IDEMPOTENT: dict[str, dict] = {}


# Add/Edit/Delete rights per role and module: ROLE_RIGHTS[role][module] = {"add", "edit", "delete"}.
# A module missing here means full rights on it (the role's module list still decides access).
ROLE_RIGHTS: dict[str, dict[str, dict[str, bool]]] = {
    "Cashier": {"Item Master": {"add": True, "edit": True, "delete": False}},
}


def touch(table: str, key: str) -> None:
    """Mark a row as changed by the app so it is saved (see app/sqlstore.py)."""
    EDITS.setdefault(table, set()).add(key)


class Seq:
    """A document number series: next(seq) gives the next number. Unlike itertools.count its current
    value can be read and restored, so numbering continues after a restart."""
    def __init__(self, start: int) -> None:
        self.value = start

    def __next__(self) -> int:
        n = self.value
        self.value += 1
        return n


# Year prefix of document numbers ("26" for the financial year starting in 2026), from COMPANY["fy_start"].
FY_PREFIX = ""
_seq = {k: Seq(1) for k in DOCS}
# Held bills get their own series so parking a bill never leaves a gap in the invoice numbers.
_seq["hold"] = Seq(1)


def set_fin_year() -> None:
    """Set FY_PREFIX from the company's financial year, or from today's (April to March) when it isn't known."""
    global FY_PREFIX
    start = COMPANY.get("fy_start") or ""
    if len(start) >= 4 and start[:4].isdigit():
        year = int(start[:4])
    else:
        today = date.today()
        year = today.year if today.month >= 4 else today.year - 1
    FY_PREFIX = f"{year % 100:02d}"


set_fin_year()


def find(table: dict, key: str) -> dict | None:
    """Look a master row up by code, or by name (the React screens send names)."""
    if key in table:
        return table[key]
    kl = key.strip().lower()
    return next((r for r in table.values() if r.get("name", "").lower() == kl), None)


def next_doc_no(kind: str, prefix: str) -> str:
    """Next number in a document series, e.g. next_doc_no("sales_invoice", "c10") -> "26/c10-0000001"."""
    return f"{FY_PREFIX}/{prefix}-{next(_seq[kind]):07d}"


def mark_app_start() -> None:
    """Record where the app's own stock movements begin: later movements are the app's, and QOH at this
    moment is each item's base (Tools → Re Write Stock rebuilds QOH as base + app movements)."""
    global APP_MOVEMENTS_FROM
    APP_MOVEMENTS_FROM = len(MOVEMENTS)
    BASE_QOH.clear()
    BASE_QOH.update({code: item["qoh"] for code, item in ITEMS.items()})


def rename_item_stock(old: str, new: str | None) -> None:
    """An item was re-coded (new) or deleted (None): move or drop its base QOH and its app "opening"
    movements, so Re Write Stock doesn't count them for a later item with the old code. Only called for
    items that are in no document, so opening movements are the only ones it can have."""
    base = BASE_QOH.pop(old, None)
    if new is not None and base is not None:
        BASE_QOH[new] = base
    kept = []
    for m in MOVEMENTS[APP_MOVEMENTS_FROM:]:
        if m["item"] == old and m["kind"] == "opening":
            if new is None:
                continue
            m = {**m, "item": new}
        kept.append(m)
    MOVEMENTS[APP_MOVEMENTS_FROM:] = kept


# Admin Master (Security), see app/security.py. SECURITY: {"login": Login Settings, "adnl_admin_hash": ...};
# FIELD_RIGHTS[role][master path][field] = "hidden" | "readonly"; MASTER_APPROVAL["masters"] = master
# paths whose rows need approval; REPORT_RIGHTS[report name] = users who may run that Customised Report.
SECURITY: dict = {}
FIELD_RIGHTS: dict[str, dict] = {}
MASTER_APPROVAL: dict[str, list] = {"masters": []}
REPORT_RIGHTS: dict[str, list] = {}

# Global Settings → Other Settings (app/doc_settings.py): per-document print and transaction settings, number
# prefixes, the sidebar's hidden modules and shortcut codes, and the fields an Item import may overwrite.
DOC_SETTINGS: dict[str, dict] = {}
NUMBER_SERIES: dict[str, dict] = {}
MENU_LAYOUT: dict = {"hidden": [], "codes": {}}
IMPORT_FIELDS: dict[str, list] = {"fields": []}

# Items merged away (Utilities → Merge Items / Replace and Remove Duplicate items): old code -> the item it
# was merged into, and the old item's desktop ItemID -> that item. Saved by sqlstore.py; db_store maps
# bills loaded later from SQL Server through them.
MERGED_ITEMS: dict[str, str] = {}
MERGED_ITEM_IDS: dict[str, str] = {}


def merged_code(code: str) -> str:
    """The item a (possibly merged-away) code now lives on."""
    seen = set()
    while code in MERGED_ITEMS and code not in seen:
        seen.add(code)
        code = MERGED_ITEMS[code]
    return code


def move_stock(item_code: str, qty: float, kind: str, doc_no: str, on: date, rate: float) -> None:
    """Add (+qty) or remove (-qty) stock and log the movement for the reports.

    kind is "opening", "purchase", "sale", "sale-cancel", "sales-return" or "deduction".
    """
    ITEMS[item_code]["qoh"] += qty
    if kind == "purchase" and qty > 0:
        LAST_PURCHASE[item_code] = on.isoformat()
    MOVEMENTS.append({"item": item_code, "qty": qty, "kind": kind, "doc_no": doc_no, "date": on.isoformat(), "rate": rate})
