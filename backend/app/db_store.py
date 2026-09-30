"""DB → In-memory Store loader.

Called at startup if the SQL Server connection is available.
Reads the real data from vLQMSLHOUSE and populates the in-memory
store dicts so all existing routers work with live data.

The store is the single source of truth for the running process.
For writes (create/update/delete) that need to persist, each router
should call db.execute() in addition to updating the store dict.
"""
import copy
import logging
import threading
import time
from datetime import date, timedelta
from decimal import Decimal

from . import db, store

logger = logging.getLogger("vlqms.db_store")


def _s(v) -> str:
    """Convert any value to a clean string."""
    if v is None:
        return ""
    return str(v).strip()


def _f(v, default=0.0) -> float:
    """Convert to float safely."""
    try:
        return float(v) if v is not None else default
    except (TypeError, ValueError):
        return default


def load_company() -> None:
    row = db.fetchone("""
        SELECT TOP 1
            CompName, Address1, Address2, CompLoc,
            TELNO, FAXNO, MOBNO, WEB, GSTNO, PANNO, CINNO,
            EMAIL, RTLCODE
        FROM MstComp
        WHERE CompID = 1
    """)
    if not row:
        row = db.fetchone("""
            SELECT TOP 1
                CompName, Address1, Address2, CompLoc,
                TELNO, FAXNO, MOBNO, WEB, GSTNO, PANNO, CINNO,
                EMAIL, RTLCODE
            FROM MstComp
        """)
    if row:
        addr_parts = [
            _s(row.get("Address1")),
            _s(row.get("Address2")),
            _s(row.get("CompLoc")),
        ]
        address = ", ".join(p for p in addr_parts if p)
        store.COMPANY.update({
            "name": _s(row.get("CompName")),
            "email": _s(row.get("EMAIL")),
            "address": address,
            "phone": _s(row.get("TELNO")),
            "fax": _s(row.get("FAXNO")),
            "mobile": _s(row.get("MOBNO")),
            "website": _s(row.get("WEB")),
            "gstin": _s(row.get("GSTNO")),
            "pan": _s(row.get("PANNO", "")),
            "cin": _s(row.get("CINNO", "")),
            "excise_phone": _s(row.get("MOBNO")),
            "ksbcl_code": _s(row.get("RTLCODE")),
            "state": "Karnataka",   # hardcoded as MstComp has no CompState column
        })
        logger.info("Loaded company: %s", store.COMPANY["name"])


def load_items() -> None:
    rows = db.fetchall("""
        SELECT
            i.ITEMID, i.ITEMCODE, i.ITEMNAME, i.ShortName,
            i.HSNCode, ig.ITEMGROUPNAME as GROUPNAME,
            b.BRANDNAME, p.PKGNAME, p.LTRPERBOT,
            i.LiqType,
            u.UOMCODE,
            i.MRP, i.PURRATE, i.CGSTPER, i.SGSTPER,
            i.ROL, i.ROQ,
            COALESCE(i.BotPerCases, 12) as BPC,
            COALESCE(q.CLBal, 0) as QOH,
            i.InActive
        FROM MstITEM i
        LEFT JOIN MstITEMGROUP ig ON ig.ITEMGROUPID = i.ITEMGroupID
        LEFT JOIN MstBrand b ON b.BRANDID = i.BrandID
        LEFT JOIN MstPKG p ON p.PKGID = i.PKGID
        LEFT JOIN MstUOM u ON u.UOMID = i.UOMID
        LEFT JOIN TrnQOH q ON q.ITEMID = i.ITEMID AND q.LOCID = 1
        WHERE i.ITEMID > 0 AND i.InActive = 'N'
    """)
    if not rows:
        logger.warning("No items loaded from DB")
        return

    store.ITEMS.clear()
    for r in rows:
        code = _s(r.get("ITEMCODE")) or str(r.get("ITEMID"))
        ltr_per_bot = _f(r.get("LTRPERBOT"), 0.75)
        store.ITEMS[code] = {
            "code": code,
            "_itemid": str(r.get("ITEMID") or ""),   # internal: for SINV/PB line lookups
            "name": _s(r.get("ITEMNAME")),
            "short_name": _s(r.get("ShortName")) or code,
            "hsn_code": _s(r.get("HSNCode")),
            "group": _s(r.get("GROUPNAME")),
            "brand": _s(r.get("BRANDNAME")),
            "liqtype": _s(r.get("LiqType")),
            "uom": _s(r.get("UOMCODE")) or "BTL",
            "package": _s(r.get("PKGNAME")),
            "litres_per_bottle": ltr_per_bot,
            "bottles_per_case": max(int(_f(r.get("BPC"), 12)), 1),
            "mrp": _f(r.get("MRP")),
            "purchase_rate": _f(r.get("PURRATE")),
            "cgst_per": _f(r.get("CGSTPER")),
            "sgst_per": _f(r.get("SGSTPER")),
            "rol": _f(r.get("ROL"), 12),
            "roq": _f(r.get("ROQ"), 48),
            "qoh": _f(r.get("QOH")),
            "active": _s(r.get("InActive")) != "Y",
        }
    logger.info("Loaded %d items from DB", len(store.ITEMS))




def load_customers() -> None:
    rows = db.fetchall("""
        SELECT CUSTID, CUSTCODE, CUSTNAME, MOBNO, CARDNO, SENDSMS, InActive
        FROM MstCUST
        WHERE CUSTID >= 0
    """)
    store.CUSTOMERS.clear()
    for r in rows:
        code = _s(r.get("CUSTCODE")) or str(r.get("CUSTID"))
        if not code or code == "-":
            code = "CASH"
        store.CUSTOMERS[code] = {
            "code": code,
            "name": _s(r.get("CUSTNAME")),
            "mobile": _s(r.get("MOBNO")),
            "loyalty_card_no": _s(r.get("CARDNO")),
            "send_sms": _s(r.get("SENDSMS")) == "Y",
            "points": 0.0,
        }
    logger.info("Loaded %d customers from DB", len(store.CUSTOMERS))


def load_suppliers() -> None:
    rows = db.fetchall("""
        SELECT SUPPID, SUPPCODE, SUPPNAME, GSTNO, City, InActive
        FROM MstSUPP
        WHERE SUPPID > 0 AND InActive = 'N'
    """)
    store.SUPPLIERS.clear()
    for r in rows:
        code = _s(r.get("SUPPCODE")) or str(r.get("SUPPID"))
        store.SUPPLIERS[code] = {
            "code": code,
            "name": _s(r.get("SUPPNAME")),
            "gstin": _s(r.get("GSTNO")),
            "city": _s(r.get("City")),
            "active": True,
        }
    logger.info("Loaded %d suppliers from DB", len(store.SUPPLIERS))


def load_item_groups() -> None:
    rows = db.fetchall("""
        SELECT ITEMGROUPID, ITEMGROUPCODE, ITEMGROUPNAME, LOYALTY, LiqItem, InActive
        FROM MstITEMGROUP
        WHERE ITEMGROUPID > 0 AND InActive = 'N'
    """)
    store.ITEM_GROUPS.clear()
    for r in rows:
        code = _s(r.get("ITEMGROUPCODE")) or str(r.get("ITEMGROUPID"))
        store.ITEM_GROUPS[code] = {
            "code": code,
            "name": _s(r.get("ITEMGROUPNAME")),
            "liquor": _s(r.get("LiqItem")) == "Y",
            "consider_for_loyalty": _s(r.get("LOYALTY")) == "Y",
        }
    logger.info("Loaded %d item groups from DB", len(store.ITEM_GROUPS))


def load_brands() -> None:
    rows = db.fetchall("""
        SELECT BRANDID, BRANDCODE, BRANDNAME, InActive
        FROM MstBrand
        WHERE BRANDID > 0 AND InActive = 'N'
    """)
    store.BRANDS.clear()
    for r in rows:
        code = _s(r.get("BRANDCODE")) or str(r.get("BRANDID"))
        store.BRANDS[code] = {"code": code, "name": _s(r.get("BRANDNAME"))}
    logger.info("Loaded %d brands from DB", len(store.BRANDS))


def load_uoms() -> None:
    rows = db.fetchall("""
        SELECT UOMID, UOMCODE, UOMNAME, InActive
        FROM MstUOM
        WHERE UOMID > 0 AND InActive = 'N'
    """)
    store.UOMS.clear()
    for r in rows:
        code = _s(r.get("UOMCODE")) or str(r.get("UOMID"))
        store.UOMS[code] = {"code": code, "name": _s(r.get("UOMNAME"))}
    logger.info("Loaded %d UOMs from DB", len(store.UOMS))


def load_users() -> None:
    rows = db.fetchall("""
        SELECT UserID, UserCode, LoginPwd, UserName, Inactive
        FROM MstUser
        WHERE UserID > 0
    """)
    # Start with the standard users (already hashed in store.SEED_USERS).
    store.USERS.clear()
    store.USERS.update(copy.deepcopy(store.SEED_USERS))

    for r in rows:
        uname = _s(r.get("UserCode")).upper()
        if not uname:
            continue
        pwd_raw = _s(r.get("LoginPwd"))
        # MstUser.LoginPwd is the legacy app's reversible (base64) password. It is kept only until the
        # user's first successful login, which replaces it with a PBKDF2 hash (auth._verify_password).
        # Hashing every DB user here would slow startup by ~0.2 s per user.
        if uname in store.USERS:
            store.USERS[uname]["legacy_password"] = pwd_raw
            store.USERS[uname]["active"] = _s(r.get("Inactive")) != "Y"
        else:
            store.USERS[uname] = {
                "user": uname,
                "password_hash": None,
                "default_password": True,   # until changed: the lowercase user code (e.g. office/office)
                "legacy_password": pwd_raw,
                "role": "Manager" if uname in ("OFFICE", "MANAGER") else "Cashier",
                "active": _s(r.get("Inactive")) != "Y",
                "pwv": 1,
            }
    logger.info("Loaded %d users into store", len(store.USERS))


def _drop_loaded(docs: dict) -> None:
    """Remove seed and previously loaded DB documents before a (re)load, keeping bills made in the app.
    (This used to clear everything, which silently deleted bills saved while the DB was loading.)"""
    for no in [no for no, d in docs.items() if d.get("origin") in ("seed", "db")]:
        del docs[no]


def load_sales_invoices(limit: int = 200) -> None:
    """Load the most recent sales invoices into DOCS['sales_invoice'] fast."""
    rows = db.fetchall(f"""
        WITH RecentHdr AS (
            SELECT TOP {limit}
                SINVID, SINVNO, SINVDate, CUSTID,
                TOTAMT, CASHAMT, CARDAMT, TOTQTY,
                SPID, SCID, AddedBy, Cancelled, SINVTime
            FROM TrnHdrSINV WITH (NOLOCK)
            WHERE Cancelled = 'N'
            ORDER BY SINVID DESC
        )
        SELECT
            h.SINVID, h.SINVNO, h.SINVDate, h.CUSTID,
            h.TOTAMT, h.CASHAMT, h.CARDAMT, h.TOTQTY,
            h.SPID, h.SCID, h.AddedBy,
            h.Cancelled, h.SINVTime,
            d.SINVDtlID, d.ITEMID, d.SINVQty, d.SINVRate, d.SINVAmt,
            d.CGSTPER, d.CGSTAMT, d.SGSTPER, d.SGSTAMT
        FROM RecentHdr h
        JOIN TrnDtlSINV d WITH (NOLOCK) ON d.SINVID = h.SINVID
        ORDER BY h.SINVID DESC
    """)

    # Build item code lookup from ITEMID → code
    item_by_id: dict[str, str] = {}
    for code, item in store.ITEMS.items():
        # We need ITEMID from DB — store it during item load
        item_by_id[item.get("_itemid", "")] = code

    docs = store.DOCS["sales_invoice"]
    _drop_loaded(docs)

    bills: dict = {}
    for r in rows:
        sinv_no = _s(r.get("SINVNO"))
        if sinv_no not in bills:
            t = _f(r.get("TOTAMT"))
            bills[sinv_no] = {
                "doc_no": sinv_no,
                "date": str(r.get("SINVDate", ""))[:10],
                "time": _s(r.get("SINVTime")) or "00:00:00",
                "counter": _s(r.get("SCID")) or "counter10",
                "customer": "CASH",
                "salesperson": _s(r.get("AddedBy")),
                "status": "SAVED",
                "payment_type": "CASH" if _f(r.get("CASHAMT")) >= t * 0.5 else "UPI",
                "lines": [],
                "totals": {
                    "qty": _f(r.get("TOTQTY")),
                    "litres": 0.0,
                    "basic": t,
                    "cgst": 0.0,
                    "sgst": 0.0,
                    "discount": 0.0,
                    "points_redeemed": 0.0,
                    "points_earned": 0.0,
                    "net": t,
                },
                "payment": {
                    "cash": _f(r.get("CASHAMT")),
                    "card": _f(r.get("CARDAMT")),
                    "upi": max(0.0, t - _f(r.get("CASHAMT")) - _f(r.get("CARDAMT"))),
                    "credit": 0.0,
                },
            }

        qty = _f(r.get("SINVQty"))
        rate = _f(r.get("SINVRate"))
        amt = _f(r.get("SINVAmt"))
        item_id = str(r.get("ITEMID") or "")
        # Items merged away in the web app (Utilities → Merge Items) land on the item they became.
        item_code = store.merged_code(store.MERGED_ITEM_IDS.get(str(item_id)) or item_by_id.get(item_id, item_id))
        item_obj = store.ITEMS.get(item_code, {})
        pkg_name = item_obj.get("package", "750ml")
        try:
            ltr_per_btl = float(pkg_name.replace("ml", "").strip()) / 1000.0
        except (ValueError, AttributeError):
            ltr_per_btl = 0.75

        line = {
            "item_code": item_code,
            "item_name": item_obj.get("name", item_code),
            "cases": int(qty) // max(int(item_obj.get("bottles_per_case", 12)), 1),
            "bottles": int(qty) % max(int(item_obj.get("bottles_per_case", 12)), 1),
            "qty": qty,
            "rate": rate,
            "amount": amt,
            "litres": round(qty * ltr_per_btl, 3),
        }
        bills[sinv_no]["lines"].append(line)
        bills[sinv_no]["totals"]["litres"] = round(
            bills[sinv_no]["totals"]["litres"] + line["litres"], 3
        )

    for b in bills.values():
        b["origin"] = "db"
    docs.update(bills)
    logger.info("Loaded %d sales invoices from DB", len(docs))


def load_purchase_bills(limit: int = 100) -> None:
    """Load the most recent purchase bills fast."""
    rows = db.fetchall(f"""
        WITH RecentPB AS (
            SELECT TOP {limit} PBID, PBNO, PBDate, SUPPID, PartyName
            FROM TrnHdrPB WITH (NOLOCK)
            ORDER BY PBID DESC
        )
        SELECT
            h.PBID, h.PBNO, h.PBDate, h.SUPPID, h.PartyName,
            d.PBDtlID, d.ITEMID, d.PBQty, d.PBRate, d.PBAmt, d.NOOFCASES
        FROM RecentPB h
        JOIN TrnDtlPB d WITH (NOLOCK) ON d.PBID = h.PBID
        ORDER BY h.PBID DESC
    """)

    item_by_id: dict[str, str] = {}
    for code, item in store.ITEMS.items():
        item_by_id[item.get("_itemid", "")] = code

    docs = store.DOCS["purchase_bill"]
    _drop_loaded(docs)
    bills: dict = {}
    for r in rows:
        pb_no = _s(r.get("PBNO"))
        if pb_no not in bills:
            bills[pb_no] = {
                "doc_no": pb_no,
                "date": str(r.get("PBDate", ""))[:10],
                "supplier": _s(r.get("PartyName")),
                "purchase_type": "Purchase",
                "lines": [],
                "totals": {"qty": 0, "basic": 0.0, "cgst": 0, "sgst": 0, "tcs": 0, "cess": 0, "net": 0.0},
            }
        qty = _f(r.get("PBQty"))
        rate = _f(r.get("PBRate"))
        amt = _f(r.get("PBAmt"))
        item_id = str(r.get("ITEMID") or "")
        # Items merged away in the web app (Utilities → Merge Items) land on the item they became.
        item_code = store.merged_code(store.MERGED_ITEM_IDS.get(str(item_id)) or item_by_id.get(item_id, item_id))
        item_obj = store.ITEMS.get(item_code, {})
        bills[pb_no]["lines"].append({
            "item_code": item_code,
            "item_name": item_obj.get("name", item_code),
            "cases": int(_f(r.get("NOOFCASES"))),
            "bottles": int(qty) % max(int(item_obj.get("bottles_per_case", 12)), 1),
            "qty": qty,
            "rate": rate,
            "amount": amt,
            "litres": round(qty * 0.75, 3),
        })
        bills[pb_no]["totals"]["qty"] += qty
        bills[pb_no]["totals"]["basic"] = round(bills[pb_no]["totals"]["basic"] + amt, 2)
        bills[pb_no]["totals"]["net"] = bills[pb_no]["totals"]["basic"]

    for b in bills.values():
        b["origin"] = "db"
    docs.update(bills)
    logger.info("Loaded %d purchase bills from DB", len(docs))


# ─── Live sales and stock (read on demand, for the Dashboard restock panel) ──────────────────────

LIVE_CACHE_SECONDS = 20          # how often the restock panel may re-query SQL Server
_live_lock = threading.Lock()
_live_cache: dict = {"at": 0.0, "key": None, "data": None}


def live_sales_and_stock(start: date, end: date) -> dict | None:
    """Current stock and sales per item straight from SQL Server, cached for LIVE_CACHE_SECONDS.

    The masters and bills loaded at startup are a snapshot; the shop keeps billing after that. This
    reads, read-only and WITH (NOLOCK):
      - sales: non-cancelled TrnHdrSINV/TrnDtlSINV lines dated start..end, summed per item;
      - stock: TrnQOH.CLBal for location 1 (the same column load_items() uses).
    Returns {"sold": {code: qty}, "last_sold": {code: iso}, "first_sale": iso | None, "qoh": {code: qty}},
    or None when the DB is unavailable or a query fails (the caller then falls back to memory).
    Dates are inlined from date objects (server-generated, never user input).
    """
    if not db.is_db_available():
        return None
    key = (start.isoformat(), end.isoformat())
    with _live_lock:
        if _live_cache["key"] == key and time.monotonic() - _live_cache["at"] < LIVE_CACHE_SECONDS:
            return _live_cache["data"]
        try:
            id_to_code = {str(it.get("_itemid")): code for code, it in store.ITEMS.items() if it.get("_itemid")}
            id_to_code.update({k: store.merged_code(v) for k, v in store.MERGED_ITEM_IDS.items()})
            sales = db.fetchall(f"""
                SELECT d.ITEMID, SUM(d.SINVQty) AS QTY, MIN(h.SINVDate) AS FIRSTDATE, MAX(h.SINVDate) AS LASTDATE
                FROM TrnHdrSINV h WITH (NOLOCK)
                JOIN TrnDtlSINV d WITH (NOLOCK) ON d.SINVID = h.SINVID
                WHERE h.Cancelled = 'N'
                  AND h.SINVDate >= '{start.isoformat()}' AND h.SINVDate < '{(end + timedelta(days=1)).isoformat()}'
                GROUP BY d.ITEMID
            """)
            stock = db.fetchall("SELECT ITEMID, CLBal FROM TrnQOH WITH (NOLOCK) WHERE LOCID = 1")
        except Exception as e:  # keep the panel working on the in-memory data
            logger.error("live_sales_and_stock failed: %s", e)
            return None

        sold: dict[str, float] = {}
        last_sold: dict[str, str] = {}
        first_sale = None
        for r in sales:
            code = id_to_code.get(str(r.get("ITEMID")))
            if not code:
                continue
            sold[code] = sold.get(code, 0.0) + _f(r.get("QTY"))
            last = str(r.get("LASTDATE") or "")[:10]
            first = str(r.get("FIRSTDATE") or "")[:10]
            if last:
                last_sold[code] = max(last_sold.get(code, ""), last)
            if first and (first_sale is None or first < first_sale):
                first_sale = first
        qoh: dict[str, float] = {}
        for r in stock:   # added up: a merged item's old ItemID maps to the item it became
            code = id_to_code.get(str(r.get("ITEMID")))
            if code:
                qoh[code] = qoh.get(code, 0.0) + _f(r.get("CLBal"))

        data = {"sold": sold, "last_sold": last_sold, "first_sale": first_sale, "qoh": qoh}
        _live_cache.update(at=time.monotonic(), key=key, data=data)
        return data


def load_masters() -> None:
    """Load all master tables from SQL Server (runs in < 0.2s)."""
    for fn in (load_company, load_item_groups, load_brands, load_uoms, load_items, load_customers, load_suppliers, load_users):
        try:
            fn()
        except Exception as e:
            logger.error("%s failed: %s", fn.__name__, e)
    logger.info("Master data loaded: %d items, %d users, %d suppliers", len(store.ITEMS), len(store.USERS), len(store.SUPPLIERS))
    # Movements before this index are the seeded demo history; later ones are bills made in this app.
    store.DB_BASE_MOVEMENTS = len(store.MOVEMENTS)
    store.mark_app_start()


def load_transactions() -> None:
    """Load recent sales invoices and purchase bills."""
    try:
        load_sales_invoices(limit=200)
    except Exception as e:
        logger.error("load_sales_invoices failed: %s", e)
    try:
        load_purchase_bills(limit=100)
    except Exception as e:
        logger.error("load_purchase_bills failed: %s", e)
    logger.info("Transaction data loaded: %d sales invoices, %d purchase bills", len(store.DOCS["sales_invoice"]), len(store.DOCS["purchase_bill"]))


def load_all() -> None:
    """Load all master + transaction data from SQL Server into the store."""
    if not db.is_db_available():
        logger.warning("DB not available — keeping in-memory seed data")
        return
    load_masters()
    load_transactions()
