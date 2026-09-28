"""In-memory store: every table the API serves, as module-level dicts.

Seeded with sample data. At startup, if SQL Server is reachable, db_store.load_masters()
replaces the masters (ITEMS, CUSTOMERS, SUPPLIERS, USERS, ...) with live rows and
load_transactions() fills DOCS with recent invoices and bills. All writes change only
these dicts, so they are lost when the server restarts.

Key rules:
- Change stock only through move_stock(): it updates ITEMS[code]["qoh"] and logs a
  MOVEMENTS row, which the Stock Ledger, Stock Balance and DSR reports read.
- Documents live in DOCS[kind][doc_no]; numbers come from next_doc_no(). Seed and DB-loaded documents
  carry "origin" ("seed" / "db"); documents made through the app have none, and are the ones
  app/persistence.py saves.
- Call touch(TABLE, key) after changing a master row, user or role, so the change is saved.
- ROLES maps a role to the module names it may open (the React sidebar names; "*" = all).
"""
import copy
from datetime import date

from .passwords import hash_password

COMPANY = {
    "name": "THE LIQUOR HOUSE",
    "email": "theliquorhouse28@gmail.com",
    "address": "Byatarayanapura, Bangalore-92, Karnataka",
    "phone": "080-26793693",
    "fax": "",
    "mobile": "9480697887",
    "website": "https://theliquorhouse.in",
    "gstin": "29AEGFP9916P1ZD",
    "pan": "AEGFP9916P",
    "cin": "U51909KA2020PTC139882",
    "excise_phone": "9900196276",
    "ksbcl_code": "41818",
    "licence_type": "CL-2",
    "state": "Karnataka",
    "fy_start": "2026-04-01",
    "fy_end": "2027-03-31",
}

ITEM_GROUPS = {
    "BEER": {"code": "BEER", "name": "BEER", "liquor": True, "consider_for_loyalty": True},
    "WHISKY": {"code": "WHISKY", "name": "WHISKY", "liquor": True, "consider_for_loyalty": True},
    "RUM": {"code": "RUM", "name": "RUM", "liquor": True, "consider_for_loyalty": True},
    "VODKA": {"code": "VODKA", "name": "VODKA", "liquor": True, "consider_for_loyalty": True},
    "SCOTCH": {"code": "SCOTCH", "name": "SCOTCH", "liquor": True, "consider_for_loyalty": True},
    "SOFT DRINKS": {"code": "SOFT DRINKS", "name": "SOFT DRINKS", "liquor": False, "consider_for_loyalty": False},
}

UOMS = {"BTL": {"code": "BTL", "name": "Bottle"}, "NOS": {"code": "NOS", "name": "Numbers"}}

PACKAGES = {
    "180ml": {"code": "180ml", "litres_per_bottle": 0.18},
    "650ml": {"code": "650ml", "litres_per_bottle": 0.65},
    "750ml": {"code": "750ml", "litres_per_bottle": 0.75},
    "600ml": {"code": "600ml", "litres_per_bottle": 0.6},
}

BRANDS = {b: {"code": b, "name": b} for b in [
    "ABD", "Royal Challenge", "McDowells No.1", "Kingfisher", "Old Monk", "Magic Moments", "Johnnie Walker", "Thums Up",
]}


def _item(code, name, group, brand, pkg, liqtype, mrp, pur, qoh, bpc=12, gst=0.0):
    return {
        "code": code, "name": name, "short_name": code, "hsn_code": "2208", "group": group, "brand": brand,
        "liqtype": liqtype, "uom": "BTL", "package": pkg, "bottles_per_case": bpc, "mrp": mrp, "purchase_rate": pur,
        "cgst_per": gst, "sgst_per": gst, "rol": 12, "roq": 48, "qoh": qoh, "active": True,
    }


# Liquor carries zero GST (excise is built into MRP); non-liquor items carry GST.
ITEMS = {i["code"]: i for i in [
    _item("VIN-750", "VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY", "WHISKY", "ABD", "750ml", "IML", 835.00, 759.29, 56),
    _item("RC-750", "ROYAL CHALLENGE SELECT PREMIUM WHISKY", "WHISKY", "Royal Challenge", "750ml", "IML", 950.00, 860.00, 68),
    _item("MCD1-180", "MCDOWELLS NO.1 LUXURY PREMIUM WHISKY", "WHISKY", "McDowells No.1", "180ml", "IML", 220.00, 198.50, 132, bpc=48),
    _item("KF-650", "KINGFISHER PREMIUM LAGER BEER", "BEER", "Kingfisher", "650ml", "Beer", 180.00, 155.00, 168),
    _item("KFU-650", "KINGFISHER ULTRA MAX STRONG BEER", "BEER", "Kingfisher", "650ml", "Beer", 230.00, 198.00, 64),
    _item("OM-750", "OLD MONK THE LEGEND XXX RUM", "RUM", "Old Monk", "750ml", "IML", 850.00, 770.00, 42),
    _item("MM-750", "MAGIC MOMENTS REMIX GRAIN VODKA", "VODKA", "Magic Moments", "750ml", "IML", 750.00, 678.00, 25),
    _item("JWB-750", "JOHNNIE WALKER BLACK LABEL 12 YO", "SCOTCH", "Johnnie Walker", "750ml", "IML", 3850.00, 3450.00, 24, bpc=6),
    _item("TU-600", "THUMS UP 600ML", "SOFT DRINKS", "Thums Up", "600ml", "Others", 40.00, 32.00, 96, bpc=24, gst=14.0),
]}

SUPPLIERS = {
    "RND": {"code": "RND", "name": "R&D DISTRIBUTORS", "gstin": "29AABCR1234F1Z1", "city": "Bangalore", "active": True},
    "KSBCL-BN": {"code": "KSBCL-BN", "name": "KSBCL DEPOT - BANGALORE NORTH", "gstin": "", "city": "Bengaluru", "active": True},
    "USL": {"code": "USL", "name": "UNITED SPIRITS LTD", "gstin": "", "city": "Bengaluru", "active": True},
    "KSBCL": {"code": "KSBCL", "name": "KARNATAKA STATE BEVERAGES CORPORATION LTD", "gstin": "", "city": "Bengaluru", "active": True},
    "HCCB": {"code": "HCCB", "name": "HINDUSTAN COCA-COLA BEVERAGES", "gstin": "29AAACH0000A1Z5", "city": "Bengaluru", "active": True},
}

CUSTOMERS = {
    "CASH": {"code": "CASH", "name": "CASH CUSTOMER", "mobile": "", "loyalty_card_no": "", "send_sms": False, "points": 0.0},
    "SRIRAM": {"code": "SRIRAM", "name": "SRI RAM BAR & RESTAURANT", "mobile": "9845000001", "loyalty_card_no": "LC0001", "send_sms": True, "points": 120.0},
    "AMBASSADOR": {"code": "AMBASSADOR", "name": "HOTEL AMBASSADOR", "mobile": "9845000002", "loyalty_card_no": "LC0002", "send_sms": False, "points": 0.0},
    "RAJU": {"code": "RAJU", "name": "RAJU (CREDIT)", "mobile": "9845000003", "loyalty_card_no": "", "send_sms": False, "points": 0.0},
}

LEDGER_GROUPS = {g: {"code": g, "name": g} for g in ["Indirect Expenses", "Cash-in-hand", "Bank Accounts", "Sundry Creditors"]}

LEDGERS = {
    "ELECTRICITY": {"code": "ELECTRICITY", "name": "Electricity Charges", "group": "Indirect Expenses"},
    "SALARY": {"code": "SALARY", "name": "Staff Salary", "group": "Indirect Expenses"},
    "TRANSPORT": {"code": "TRANSPORT", "name": "Transport / Hamali", "group": "Indirect Expenses"},
    "CASH": {"code": "CASH", "name": "Cash", "group": "Cash-in-hand"},
}

COUNTERS = {
    "counter10": {"code": "counter10", "name": "counter10", "system_name": "PRADEEP", "prefix": "c10", "inactive": False},
    "counter11": {"code": "counter11", "name": "counter11", "system_name": "GURU", "prefix": "c11", "inactive": False},
    "counter12": {"code": "counter12", "name": "counter12 (Beer Bay)", "system_name": "RAVI", "prefix": "c12", "inactive": True},
}

SALES_PERSONS = ["GURU", "PRADEEP", "RAVI", "ADMIN"]

# App users. Passwords are kept only as salted PBKDF2 hashes (app/passwords.py), never in plain text.
# "pwv" (password version) goes up on every password change, which invalidates older login tokens.
# The demo logins (ADMIN/admin, GURU/guru, PRADEEP/pradeep, RAVI/ravi) are for development:
# change them in Admin Master before real use.
def _user(name: str, password: str, role: str) -> dict:
    return {"user": name, "password_hash": hash_password(password), "role": role, "active": True, "pwv": 1}


SEED_USERS = {u["user"]: u for u in [
    _user("ADMIN", "admin", "Administrator"),
    _user("GURU", "guru", "Manager"),
    _user("PRADEEP", "pradeep", "Cashier"),
    _user("RAVI", "ravi", "Cashier"),
]}
USERS = copy.deepcopy(SEED_USERS)

# Role → module names it may open (the React sidebar names). "*" means every module.
# Legacy equivalent: frmMstRole + frmUtlAssignRights.
ADMIN_ROLE = "Administrator"
ROLES = {
    ADMIN_ROLE: ["*"],
    "Manager": [
        "Dashboard", "Item Master", "Price List Maintenance", "Item Group Master", "UOM Master", "Supplier Master",
        "Customer Master", "Ledger Master", "Ledger Group Master", "Package Master", "Brand Master",
        "Purchase Bill", "Sales Invoice", "Expense Entry", "Stock Deduction", "Purchase Indent", "Sales Return",
        "Closing Stock Entry", "Cash Denomination", "Daily Summary Report", "Stock Balance Report", "Stock Ledger",
        "Stock Ageing", "Profit Margin", "Hourly Sales", "Daily Sales", "Brand-wise Sales", "Day-wise Litres",
        "Stock in Litres", "Stock Level / Reorder", "Reports Dashboard",
    ],
    "Cashier": ["Dashboard", "Item Master", "Sales Invoice", "Sales Return", "Cash Denomination", "Closing Stock Entry", "Stock Balance Report"],
}

# Day-end cash counts, keyed by ISO date.
DENOMINATIONS: dict[str, dict] = {}

# When each item was last received, for Stock Ageing; purchases update it.
LAST_PURCHASE = {
    "VIN-750": "2026-09-10", "RC-750": "2026-08-28", "MCD1-180": "2026-09-18", "KF-650": "2026-09-20",
    "KFU-650": "2026-07-15", "OM-750": "2026-06-02", "MM-750": "2026-04-22", "JWB-750": "2026-03-05", "TU-600": "2026-09-01",
}

SETTINGS = {
    "loyalty": {"points_per_rupee": 0.01, "alert_when_points_above": 500, "min_bill_amount": 1000},
    "invoice": {"salesperson_required": True, "print_preview": False, "barcode_scan_jumps_to_qty": True},
}

# Transactions: doc type -> {doc_no: doc}
DOCS: dict[str, dict[str, dict]] = {k: {} for k in [
    "purchase_indent", "purchase_bill", "sales_invoice", "sales_return", "expense", "stock_deduction", "closing_stock",
]}

# Every stock-affecting line is logged here to drive Stock Ledger / Balance / DSR.
MOVEMENTS: list[dict] = []
# Index into MOVEMENTS where in-app activity starts once live DB data is loaded (set by db_store).
DB_BASE_MOVEMENTS = 0
# Index into MOVEMENTS where movements made through this app start (after the seed history and the DB
# load). app/persistence.py saves MOVEMENTS[APP_MOVEMENTS_FROM:] and replays them after a restart.
APP_MOVEMENTS_FROM = 0

# Master rows changed through the app, per table name ("ITEMS", "USERS", "ROLES", "COMPANY", ...).
# app/persistence.py saves these rows (or their deletion) and lays them back over the seed/DB data on start.
EDITS: dict[str, set[str]] = {}

# Writes queued for SQL Server (app/writeback.py, only with DB_WRITE_ENABLED). Saved with the rest of the
# app state (app/persistence.py) so nothing queued is lost on a restart.
OUTBOX: list[dict] = []

# Replies to recent bill saves, per (user, path, Idempotency-Key header): a double-click or a retry of
# the same save gets the first reply back instead of creating a second bill (main.py).
IDEMPOTENT: dict[str, dict] = {}


# Add/Edit/Delete rights per role and module: ROLE_RIGHTS[role][module] = {"add", "edit", "delete"}.
# A module missing here means full rights on it (the role's module list still decides access).
ROLE_RIGHTS: dict[str, dict[str, dict[str, bool]]] = {
    "Cashier": {"Item Master": {"add": True, "edit": True, "delete": False}},
}


def touch(table: str, key: str) -> None:
    """Mark a row as changed by the app so it is saved (see app/persistence.py)."""
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


FY_PREFIX = "26"
_seq = {k: Seq(1) for k in DOCS}
_seq["sales_invoice"] = Seq(83718)
# Held bills get their own series so parking a bill never leaves a gap in the invoice numbers.
_seq["hold"] = Seq(1)


def find(table: dict, key: str) -> dict | None:
    """Look a master row up by code, or by name (the React screens send names)."""
    if key in table:
        return table[key]
    kl = key.strip().lower()
    return next((r for r in table.values() if r.get("name", "").lower() == kl), None)


def next_doc_no(kind: str, prefix: str) -> str:
    """Next number in a document series, e.g. next_doc_no("sales_invoice", "c10") -> "26/c10-0083718"."""
    return f"{FY_PREFIX}/{prefix}-{next(_seq[kind]):07d}"


def move_stock(item_code: str, qty: float, kind: str, doc_no: str, on: date, rate: float) -> None:
    """Add (+qty) or remove (-qty) stock and log the movement for the reports.

    kind is "opening", "purchase", "sale", "sale-cancel", "sales-return" or "deduction".
    """
    ITEMS[item_code]["qoh"] += qty
    if kind == "purchase" and qty > 0:
        LAST_PURCHASE[item_code] = on.isoformat()
    MOVEMENTS.append({"item": item_code, "qty": qty, "kind": kind, "doc_no": doc_no, "date": on.isoformat(), "rate": rate})


def _seed_sales_history():
    from datetime import timedelta
    import random
    rng = random.Random(42)  # Deterministic seed
    today = date.today()

    # Seed baseline purchase bills and stock movements
    # Day -6 to Day 0
    sample_items = [
        ("RC-750", 950.0, 860.0, 0.75),
        ("VIN-750", 835.0, 759.29, 0.75),
        ("KF-650", 180.0, 155.0, 0.65),
        ("MCD1-180", 220.0, 198.5, 0.18),
        ("OM-750", 850.0, 770.0, 0.75),
        ("KFU-650", 230.0, 198.0, 0.65),
        ("MM-750", 750.0, 678.0, 0.75),
        ("JWB-750", 3850.0, 3450.0, 0.75),
        ("TU-600", 40.0, 32.0, 0.60),
    ]
    counters = ["counter10", "counter11"]
    salespersons = ["GURU", "PRADEEP", "RAVI", "ADMIN"]

    # 1. Seed purchase bills
    for i in range(7):
        p_date = today - timedelta(days=6 - i)
        pb_no = f"{FY_PREFIX}/PB-{next(_seq['purchase_bill']):07d}"
        pb_lines = []
        pb_tot_qty = 0
        pb_tot_basic = 0.0
        for code, mrp, pur_rate, l_per_btl in sample_items[:5]:
            qty = rng.choice([24, 36, 48, 60])
            amt = qty * pur_rate
            litres = round(qty * l_per_btl, 3)
            move_stock(code, qty, "purchase", pb_no, p_date, pur_rate)
            pb_lines.append({
                "item_code": code,
                "item_name": ITEMS[code]["name"],
                "cases": qty // 12,
                "bottles": qty % 12,
                "qty": qty,
                "rate": pur_rate,
                "amount": amt,
                "litres": litres,
            })
            pb_tot_qty += qty
            pb_tot_basic += amt
        DOCS["purchase_bill"][pb_no] = {
            "origin": "seed",
            "doc_no": pb_no,
            "date": p_date.isoformat(),
            "supplier": "KSBCL-BN",
            "purchase_type": "Purchase",
            "lines": pb_lines,
            "totals": {
                "qty": pb_tot_qty,
                "basic": round(pb_tot_basic, 2),
                "cgst": 0,
                "sgst": 0,
                "tcs": 0,
                "cess": 0,
                "net": round(pb_tot_basic, 2),
            },
        }

    # 2. Seed 7 days sales invoices and sales movements
    targets = [112000, 145000, 128000, 168000, 154000, 192000, 185000]
    for day_idx in range(7):
        target_amt = targets[day_idx]
        current_date = today - timedelta(days=6 - day_idx)
        d_str = current_date.isoformat()
        accum = 0.0
        bill_idx = 1

        while accum < target_amt:
            doc_no = f"{FY_PREFIX}/c10-{next(_seq['sales_invoice']):07d}"
            num_lines = rng.choice([1, 2, 2, 3, 4])
            lines = []
            bill_qty = 0
            bill_litres = 0.0
            bill_net = 0.0

            chosen = rng.sample(sample_items, min(num_lines, len(sample_items)))
            for code, mrp, pur_rate, l_per_btl in chosen:
                qty = rng.choice([1, 1, 2, 2, 3, 4, 6])
                amt = qty * mrp
                litres = round(qty * l_per_btl, 3)
                item_obj = ITEMS.get(code, {})
                bpc = item_obj.get("bottles_per_case", 12)
                lines.append({
                    "item_code": code,
                    "item_name": item_obj.get("name", code),
                    "cases": qty // bpc,
                    "bottles": qty % bpc,
                    "qty": qty,
                    "rate": mrp,
                    "amount": amt,
                    "litres": litres,
                })
                bill_qty += qty
                bill_litres += litres
                bill_net += amt
                move_stock(code, -qty, "sale", doc_no, current_date, mrp)

            bill_net = round(bill_net)
            accum += bill_net

            pay_type = rng.choice(["CASH", "UPI", "CARD", "CASH", "UPI"])
            pay = {"cash": 0, "card": 0, "upi": 0, "credit": 0}
            pay[{"CASH": "cash", "UPI": "upi", "CARD": "card"}[pay_type]] = bill_net

            h = 10 + (bill_idx % 12)
            m = (bill_idx * 7) % 60
            s = (bill_idx * 13) % 60

            DOCS["sales_invoice"][doc_no] = {
                "origin": "seed",
                "doc_no": doc_no,
                "date": d_str,
                "time": f"{h:02d}:{m:02d}:{s:02d}",
                "counter": rng.choice(counters),
                "customer": "CASH",
                "salesperson": rng.choice(salespersons),
                "status": "SAVED",
                "payment_type": pay_type,
                "lines": lines,
                "totals": {
                    "qty": bill_qty,
                    "litres": round(bill_litres, 3),
                    "basic": bill_net,
                    "cgst": 0,
                    "sgst": 0,
                    "discount": 0,
                    "points_redeemed": 0,
                    "points_earned": 0,
                    "net": bill_net,
                },
                "payment": pay,
            }
            bill_idx += 1


_seed_sales_history()
APP_MOVEMENTS_FROM = len(MOVEMENTS)
