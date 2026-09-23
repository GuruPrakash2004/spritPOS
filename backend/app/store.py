"""In-memory store seeded with the same sample data as the React prototype.

There is no database yet: everything resets when the server restarts. Stock
movements (purchase adds, sale reduces, return adds, deduction reduces) are
applied to ITEMS[code]["qoh"] so the reports stay consistent.
"""
from datetime import date
from itertools import count

COMPANY = {
    "name": "THE LIQUOR HOUSE",
    "licence_type": "CL-2",
    "state": "Karnataka",
    "fy_start": "2026-04-01",
    "fy_end": "2027-03-31",
}

ITEM_GROUPS = {
    "1.BEER": {"code": "1.BEER", "name": "BEER", "liquor": True, "consider_for_loyalty": True},
    "2.WHISKY": {"code": "2.WHISKY", "name": "WHISKY", "liquor": True, "consider_for_loyalty": True},
    "3.RUM": {"code": "3.RUM", "name": "RUM", "liquor": True, "consider_for_loyalty": True},
    "4.VODKA": {"code": "4.VODKA", "name": "VODKA", "liquor": True, "consider_for_loyalty": True},
    "5.SCOTCH": {"code": "5.SCOTCH", "name": "SCOTCH", "liquor": True, "consider_for_loyalty": True},
    "9.SOFT DRINKS": {"code": "9.SOFT DRINKS", "name": "SOFT DRINKS", "liquor": False, "consider_for_loyalty": False},
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
    _item("VIN-750", "VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY", "2.WHISKY", "ABD", "750ml", "IML", 835.00, 759.29, 56),
    _item("RC-750", "ROYAL CHALLENGE SELECT PREMIUM WHISKY", "2.WHISKY", "Royal Challenge", "750ml", "IML", 950.00, 860.00, 68),
    _item("MCD1-180", "MCDOWELLS NO.1 LUXURY PREMIUM WHISKY", "2.WHISKY", "McDowells No.1", "180ml", "IML", 220.00, 198.50, 132, bpc=48),
    _item("KF-650", "KINGFISHER PREMIUM LAGER BEER", "1.BEER", "Kingfisher", "650ml", "Beer", 180.00, 155.00, 168),
    _item("KFU-650", "KINGFISHER ULTRA MAX STRONG BEER", "1.BEER", "Kingfisher", "650ml", "Beer", 230.00, 198.00, 64),
    _item("OM-750", "OLD MONK THE LEGEND XXX RUM", "3.RUM", "Old Monk", "750ml", "IML", 850.00, 770.00, 42),
    _item("MM-750", "MAGIC MOMENTS REMIX GRAIN VODKA", "4.VODKA", "Magic Moments", "750ml", "IML", 750.00, 678.00, 25),
    _item("JWB-750", "JOHNNIE WALKER BLACK LABEL 12 YO", "5.SCOTCH", "Johnnie Walker", "750ml", "IML", 3850.00, 3450.00, 24, bpc=6),
    _item("TU-600", "THUMS UP 600ML", "9.SOFT DRINKS", "Thums Up", "600ml", "Others", 40.00, 32.00, 96, bpc=24, gst=14.0),
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

# Demo logins only: the prototype has no real security (plaintext demo passwords, user name as token).
USERS = {
    "ADMIN": {"user": "ADMIN", "password": "admin", "role": "Administrator", "active": True},
    "GURU": {"user": "GURU", "password": "guru", "role": "Manager", "active": True},
    "PRADEEP": {"user": "PRADEEP", "password": "pradeep", "role": "Cashier", "active": True},
    "RAVI": {"user": "RAVI", "password": "ravi", "role": "Cashier", "active": True},
}

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
    "Cashier": ["Dashboard", "Sales Invoice", "Sales Return", "Cash Denomination", "Closing Stock Entry", "Stock Balance Report"],
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

FY_PREFIX = "26"
_seq = {k: count(1) for k in DOCS}
_seq["sales_invoice"] = count(83718)
# Held bills get their own series so parking a bill never leaves a gap in the invoice numbers.
_seq["hold"] = count(1)


def find(table: dict, key: str) -> dict | None:
    """Look a master row up by code, or by name (the React screens send names)."""
    if key in table:
        return table[key]
    kl = key.strip().lower()
    return next((r for r in table.values() if r.get("name", "").lower() == kl), None)


def next_doc_no(kind: str, prefix: str) -> str:
    return f"{FY_PREFIX}/{prefix}-{next(_seq[kind]):07d}"


def move_stock(item_code: str, qty: float, kind: str, doc_no: str, on: date, rate: float) -> None:
    ITEMS[item_code]["qoh"] += qty
    if kind == "purchase" and qty > 0:
        LAST_PURCHASE[item_code] = on.isoformat()
    MOVEMENTS.append({"item": item_code, "qty": qty, "kind": kind, "doc_no": doc_no, "date": on.isoformat(), "rate": rate})
