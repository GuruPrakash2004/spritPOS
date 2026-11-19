"""DB → In-memory Store loader.

Called at startup (app/boot.py) once the database is set up. Reads the desktop's own tables (MstITEM,
TrnHdrSINV, ...) in the shop's vLQMS database and fills the in-memory store dicts, so every router works
with live data. There is no copy: the desktop app and the web app share these tables.
"""
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


# MstComp columns the web's own Company fields cover (CompName ... RTLCODE, PHNO; CompLoc is not one of them); every other editable column of the
# table is carried in the profile's `extras`, so a column added to MstComp or listed in the Doc Designer shows up.
COMPANY_COLUMNS = {"compid", "compname", "address1", "address2", "address3", "telno", "faxno", "mobno", "web", "gstno",
                   "panno", "cinno", "email", "rtlcode", "phno", "jpgfile", "dashimg"}


def load_company() -> None:
    row = db.fetchone("SELECT TOP 1 * FROM MstComp WHERE CompID = 1") or db.fetchone("SELECT TOP 1 * FROM MstComp")
    if row:
        from . import schema_meta
        row = {str(k): v for k, v in row.items()}
        low = {k.lower(): v for k, v in row.items()}
        # vLQMS.exe's Company Info grid: Company Name, Address, ".", "..", Email ID = CompName, Address1, Address2, Address3, EMAIL
        lines = [_s(low.get("address1")), _s(low.get("address2")), _s(low.get("address3"))]
        address = ", ".join(p for p in lines if p)
        editable = {c["name"].lower() for c in schema_meta.columns("MstComp") if schema_meta.editable(c)}
        extras = {k: (v.strip() if isinstance(v, str) else v) for k, v in row.items()
                  if k.lower() in editable and k.lower() not in COMPANY_COLUMNS
                  and isinstance(v, (str, int, float, bool, type(None), Decimal))}
        store.COMPANY.update({
            "name": _s(low.get("compname")),
            "email": _s(low.get("email")),
            "address": address,
            "address1": lines[0], "address2": lines[1], "address3": lines[2],
            "phone": _s(low.get("telno")),
            "fax": _s(low.get("faxno")),
            "mobile": _s(low.get("mobno")),
            "website": _s(low.get("web")),
            "gstin": _s(low.get("gstno")),
            "pan": _s(low.get("panno")),
            "cin": _s(low.get("cinno")),
            "excise_phone": _s(low.get("phno")),
            "ksbcl_code": _s(low.get("rtlcode")),
            "extras": extras,
            # MstComp has no state or licence column: those stay as saved from Company Master (sqlstore.apply)
        })
        logger.info("Loaded company: %s", store.COMPANY["name"])


def reload_items() -> None:
    """Read the items again after a bulk change made in the database (Update Item Rates, Update Item Master Fields, year-end carry forward). `load_items`
    alone gives rows without their `extras` (Item Selling Rate, Cess, Comp.Cess, HotKey ... : the database's other columns), so the screens would show
    them blank and the next save of an item could write the blanks back; the extras are read again here."""
    load_items()
    from . import sqlstore
    sqlstore.reload_extras()


def load_items() -> None:
    _nonstk = _optional("MstITEM", ("NonStkable",), "i.")   # a client without the column reads NULL
    rows = db.fetchall(f"""
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
            i.InActive{_nonstk}
        FROM MstITEM i
        LEFT JOIN MstITEMGROUP ig ON ig.ITEMGROUPID = i.ITEMGroupID
        LEFT JOIN MstBrand b ON b.BRANDID = i.BrandID
        LEFT JOIN MstPKG p ON p.PKGID = i.PKGID
        LEFT JOIN MstUOM u ON u.UOMID = i.UOMID
        LEFT JOIN TrnQOH q ON q.ITEMID = i.ITEMID AND q.LOCID = 1
        WHERE i.ITEMID > 0
    """)
    if not rows:
        logger.warning("No items in MstITEM")
    store.ITEMS.clear()
    aliases: dict[int, list[str]] | None = {}   # MstItemAlias: other names an item is found by (the desktop's item form has a grid for them)
    try:
        for a in db.fetchall("SELECT ItemID, ItemAlias FROM MstItemAlias"):
            text = _s(a.get("ItemAlias"))
            if text and text not in aliases.setdefault(int(a["ItemID"]), []):
                aliases[int(a["ItemID"])].append(text)
    except Exception as e:   # a database without the table: the items carry no `aliases` at all, so a save never clears them
        logger.warning("Item aliases not read: %s", e)
        aliases = None
    for r in rows:
        code = _s(r.get("ITEMCODE")) or str(r.get("ITEMID"))
        ltr_per_bot = _f(r.get("LTRPERBOT"), 0.75)
        store.ITEMS[code] = {
            "code": code,
            "_itemid": str(r.get("ITEMID") or ""),   # internal: for SINV/PB line lookups
            "name": _s(r.get("ITEMNAME")),
            "short_name": _s(r.get("ShortName")),
            "hsn_code": _s(r.get("HSNCode")),
            "group": _s(r.get("GROUPNAME")),
            "brand": _s(r.get("BRANDNAME")),
            "liqtype": _s(r.get("LiqType")),
            "uom": _s(r.get("UOMCODE")) or "BTL",
            "package": _s(r.get("PKGNAME")),
            "litres_per_bottle": ltr_per_bot,
            "bottles_per_case": max(int(_f(r.get("BPC"), 12)), 0),
            "mrp": _f(r.get("MRP")),
            "purchase_rate": _f(r.get("PURRATE")),
            "cgst_per": _f(r.get("CGSTPER")),
            "sgst_per": _f(r.get("SGSTPER")),
            "rol": _f(r.get("ROL"), 12),
            "roq": _f(r.get("ROQ"), 48),
            "qoh": _f(r.get("QOH")),
            "active": _s(r.get("InActive")) != "Y",
            "non_stockable": _s(r.get("NonStkable")) == "Y",   # the Item Master's "Non Stockable" tick
            **({"aliases": aliases.get(int(r.get("ITEMID") or 0), [])} if aliases is not None else {}),
        }
    logger.info("Loaded %d items from DB", len(store.ITEMS))




def load_packages() -> None:
    """Pack sizes (MstPKG), keyed by name as items refer to them (e.g. "750ML")."""
    rows = db.fetchall(f"""
        SELECT PKGID, PKGCODE, PKGNAME, LTRPERBOT, InActive{_optional("MstPKG", ("PKGOrderBy",))}
        FROM MstPKG
        WHERE PKGID > 0
    """)
    store.PACKAGES.clear()
    for r in rows:
        code = _s(r.get("PKGNAME")) or _s(r.get("PKGCODE")) or str(r.get("PKGID"))
        store.PACKAGES[code] = {"code": code, "litres_per_bottle": _f(r.get("LTRPERBOT")), "active": _s(r.get("InActive")).upper() != "Y",
                                "_order": int(_f(r.get("PKGOrderBy")))}
    logger.info("Loaded %d packages from DB", len(store.PACKAGES))


def load_customers() -> None:
    rows = db.fetchall("""
        SELECT c.CUSTID, c.CUSTCODE, c.CUSTNAME, c.MOBNO, c.CARDNO, c.SENDSMS, c.InActive, c.GSTNO, u.CURCODE
        FROM MstCUST c LEFT JOIN MstCUR u ON u.CURID = c.CURID
        WHERE c.CUSTID >= 0
    """)
    store.CUSTOMERS.clear()
    for r in rows:
        code = _s(r.get("CUSTCODE")) or str(r.get("CUSTID"))
        if not code or code == "-" or r.get("CUSTID") == 0:   # CUSTID 0 is the desktop's walk-in cash customer
            code = "CASH"
        store.CUSTOMERS[code] = {
            "code": code,
            "name": _s(r.get("CUSTNAME")),
            "mobile": _s(r.get("MOBNO")),
            "loyalty_card_no": _s(r.get("CARDNO")),
            "send_sms": _s(r.get("SENDSMS")) == "Y",
            "points": 0.0,
            "gstin": _s(r.get("GSTNO")),
            "currency": _s(r.get("CURCODE")),
            "active": _s(r.get("InActive")).upper() != "Y",
        }
    logger.info("Loaded %d customers from DB", len(store.CUSTOMERS))


def load_suppliers() -> None:
    rows = db.fetchall("""
        SELECT SUPPID, SUPPCODE, SUPPNAME, GSTNO, City, InActive
        FROM MstSUPP
        WHERE SUPPID > 0
    """)
    store.SUPPLIERS.clear()
    for r in rows:
        code = _s(r.get("SUPPCODE")) or str(r.get("SUPPID"))
        store.SUPPLIERS[code] = {
            "code": code,
            "name": _s(r.get("SUPPNAME")),
            "gstin": _s(r.get("GSTNO")),
            "city": _s(r.get("City")),
            "active": _s(r.get("InActive")).upper() != "Y",
        }
    logger.info("Loaded %d suppliers from DB", len(store.SUPPLIERS))


def load_item_groups() -> None:
    rows = db.fetchall(f"""
        SELECT ITEMGROUPID, ITEMGROUPCODE, ITEMGROUPNAME, LOYALTY, LiqItem, InActive{_optional("MstITEMGROUP", ("Authorised", "GroupOrderBy"))}
        FROM MstITEMGROUP
        WHERE ITEMGROUPID > 0
    """)
    store.ITEM_GROUPS.clear()
    for r in rows:
        code = _s(r.get("ITEMGROUPCODE")) or str(r.get("ITEMGROUPID"))
        store.ITEM_GROUPS[code] = {
            "code": code,
            "name": _s(r.get("ITEMGROUPNAME")),
            "liquor": _s(r.get("LiqItem")) == "Y",
            "consider_for_loyalty": _s(r.get("LOYALTY")) == "Y",
            **({"active": False} if _s(r.get("InActive")).upper() == "Y" else {}),
            **({"authorised": False} if _s(r.get("Authorised")).upper() == "N" else {}),
            "_order": int(_f(r.get("GroupOrderBy"))),   # Item Group "Arrange Slno": the order stock lists (Physical Stock Entry) follow
        }
    logger.info("Loaded %d item groups from DB", len(store.ITEM_GROUPS))


def load_brands() -> None:
    rows = db.fetchall("""
        SELECT BRANDID, BRANDCODE, BRANDNAME, InActive
        FROM MstBrand
        WHERE BRANDID > 0
    """)
    store.BRANDS.clear()
    for r in rows:
        code = _s(r.get("BRANDCODE")) or str(r.get("BRANDID"))
        store.BRANDS[code] = {"code": code, "name": _s(r.get("BRANDNAME")), "active": _s(r.get("InActive")).upper() != "Y"}
    logger.info("Loaded %d brands from DB", len(store.BRANDS))


def load_simple_masters() -> None:
    """The simple masters of master_specs.py (groups, states, currencies, locations, taxes, banks, holidays)."""
    from .master_specs import SPECS
    for sp in SPECS:
        cols = [sp["id"], sp["code"], sp["name"]] + [c for _f, c, _k in sp["extras"]] + ([] if sp.get("plain") else ["InActive"])
        rows = db.fetchall(f"SELECT {', '.join(cols)} FROM {sp['table']} WHERE {sp['id']} > 0")
        target = getattr(store, sp["store"])
        target.clear()
        for r in rows:
            raw = r.get(sp["code"])
            code = raw.date().isoformat() if sp.get("date_code") and hasattr(raw, "date") else _s(raw)
            if not code or code == "-":
                continue
            row = {"code": code, "name": _s(r.get(sp["name"]))}
            for f, c, kind in sp["extras"]:
                row[f] = float(r.get(c) or 0) if kind == "num" else _s(r.get(c))
            if not sp.get("plain"):
                row["active"] = _s(r.get("InActive")).upper() != "Y"
            target[code] = row
        logger.info("Loaded %d rows of %s", len(target), sp["table"])


def load_uoms() -> None:
    rows = db.fetchall(f"""
        SELECT UOMID, UOMCODE, UOMNAME, InActive{_optional("MstUOM", ("Decimals",))}
        FROM MstUOM
        WHERE UOMID > 0
    """)
    store.UOMS.clear()
    for r in rows:
        code = _s(r.get("UOMCODE")) or str(r.get("UOMID"))
        store.UOMS[code] = {"code": code, "name": _s(r.get("UOMNAME")), "decimals": int(_f(r.get("Decimals"))),
                             "active": _s(r.get("InActive")).upper() != "Y"}
    logger.info("Loaded %d UOMs from DB", len(store.UOMS))


def load_fin_year() -> None:
    """The active financial year (MstCompFinYear), which also gives document numbers their year prefix."""
    rows = db.fetchall("SELECT yrFdate, yrTdate, Active FROM MstCompFinYear ORDER BY yrFdate DESC")
    years = [{"start": str(r.get("yrFdate") or "")[:10], "end": str(r.get("yrTdate") or "")[:10],
              "active": _s(r.get("Active")).upper() in ("Y", "1", "TRUE")} for r in rows if r.get("yrFdate")]
    store.FIN_YEARS[:] = years
    current = next((y for y in years if y["active"]), years[0] if years else None)
    if current:
        store.COMPANY.update({"fy_start": current["start"], "fy_end": current["end"]})
    store.set_fin_year()
    logger.info("Financial year: %s to %s", store.COMPANY.get("fy_start"), store.COMPANY.get("fy_end"))


def load_ledger_groups() -> None:
    rows = db.fetchall("""
        SELECT LEDGROUPID, LEDGROUPCODE, LEDGROUPNAME
        FROM MstLedgroup
        WHERE LEDGROUPID > 0 AND InActive = 'N'
    """)
    store.LEDGER_GROUPS.clear()
    for r in rows:
        code = _s(r.get("LEDGROUPCODE")) or _s(r.get("LEDGROUPNAME")) or str(r.get("LEDGROUPID"))
        store.LEDGER_GROUPS[code] = {"code": code, "name": _s(r.get("LEDGROUPNAME")) or code}
    logger.info("Loaded %d ledger groups from DB", len(store.LEDGER_GROUPS))


def load_ledgers() -> None:
    rows = db.fetchall("""
        SELECT l.LEDID, l.LEDCODE, l.LEDNAME, g.LEDGROUPCODE, g.LEDGROUPNAME
        FROM MstLed l
        LEFT JOIN MstLedgroup g ON g.LEDGROUPID = l.LEDGROUPID
        WHERE l.LEDID > 0 AND l.InActive = 'N'
    """)
    store.LEDGERS.clear()
    for r in rows:
        code = _s(r.get("LEDCODE")) or _s(r.get("LEDNAME")) or str(r.get("LEDID"))
        store.LEDGERS[code] = {"code": code, "name": _s(r.get("LEDNAME")) or code,
                               "group": _s(r.get("LEDGROUPCODE")) or _s(r.get("LEDGROUPNAME"))}
    logger.info("Loaded %d ledgers from DB", len(store.LEDGERS))


def load_counters() -> None:
    """Sales counters (MstSC). "_scid" is the desktop's SCID, which bills store (TrnHdrSINV.SCID)."""
    rows = db.fetchall("SELECT SCID, SCCODE, SCNAME, SYSNAME, PREFIX, InActive FROM MstSC WHERE SCID > 0")
    store.COUNTERS.clear()
    for r in rows:
        code = _s(r.get("SCCODE")) or str(r.get("SCID"))
        store.COUNTERS[code] = {"code": code, "_scid": str(r.get("SCID") or ""), "name": _s(r.get("SCNAME")) or code,
                                "system_name": _s(r.get("SYSNAME")), "prefix": _s(r.get("PREFIX")) or code,
                                "inactive": _s(r.get("InActive")) == "Y"}
    logger.info("Loaded %d sales counters from DB", len(store.COUNTERS))


def load_sales_persons() -> None:
    """Sales persons (MstSP). "_spid" is the desktop's SPID, which bills store (TrnHdrSINV.SPID)."""
    rows = db.fetchall("SELECT SPID, SPCODE, SPNAME, MOBNO, InActive FROM MstSP WHERE SPID > 0")
    store.SALES_PERSONS.clear()
    for r in rows:
        code = _s(r.get("SPCODE")) or str(r.get("SPID"))
        store.SALES_PERSONS[code] = {"code": code, "_spid": str(r.get("SPID") or ""), "name": _s(r.get("SPNAME")) or code,
                                     "mobile": _s(r.get("MOBNO")), "active": _s(r.get("InActive")) != "Y"}
    logger.info("Loaded %d sales persons from DB", len(store.SALES_PERSONS))


def load_last_purchase() -> None:
    """When each item was last received (for Stock Ageing), from the purchase bills."""
    rows = db.fetchall("""
        SELECT d.ITEMID, MAX(h.PBDate) AS LASTDATE
        FROM TrnHdrPB h WITH (NOLOCK)
        JOIN TrnDtlPB d WITH (NOLOCK) ON d.PBID = h.PBID
        WHERE ISNULL(h.Cancelled, 'N') <> 'Y'
        GROUP BY d.ITEMID
    """)
    by_id = {it.get("_itemid"): code for code, it in store.ITEMS.items() if it.get("_itemid")}
    store.LAST_PURCHASE.clear()
    for r in rows:
        code = by_id.get(str(r.get("ITEMID") or ""))
        if code and r.get("LASTDATE"):
            store.LAST_PURCHASE[code] = str(r["LASTDATE"])[:10]
    logger.info("Loaded the last purchase date of %d items", len(store.LAST_PURCHASE))


# Desktop role codes/names (MstROLE) taken as the web roles: matched whole, so "NON ADMIN" is not an admin.
ADMIN_ROLE_NAMES = {"ADMIN", "ADMINISTRATOR", "ADMINISTRATORS", "SUPER ADMIN", "SUPERADMIN"}
MANAGER_ROLE_NAMES = {"MANAGER", "MANAGERS", "STORE MANAGER", "SHOP MANAGER"}


def _role_for(user_code: str, role_code: str, role_name: str) -> str:
    """The web role of a desktop user, from their desktop role (MstROLE)."""
    names = {role_code.strip().upper(), role_name.strip().upper()} - {""}
    if user_code.strip().upper() == "ADMIN" or names & ADMIN_ROLE_NAMES:   # the exe gives the user code ADMIN every right (svtModule, frmMain)
        return store.ADMIN_ROLE
    if names & MANAGER_ROLE_NAMES or user_code in ("OFFICE", "MANAGER"):
        return "Manager"
    return "Cashier"


def load_users() -> None:
    rows = db.fetchall("""
        SELECT u.UserID, u.UserCode, u.LoginPwd, u.UserName, u.Inactive, u.Authorised, r.ROLECODE, r.ROLENAME
        FROM MstUser u
        LEFT JOIN MstROLE r ON r.ROLEID = u.RoleID
        WHERE u.UserID > 0
    """)
    # Only the shop's own users; a role or password changed in the web app is laid over them by sqlstore.apply().
    store.USERS.clear()
    for r in rows:
        uname = _s(r.get("UserCode")).upper()
        if not uname:
            continue
        pwd_raw = _s(r.get("LoginPwd"))
        # MstUser.LoginPwd is the legacy app's reversible (base64) password. It is kept only until the
        # user's first successful login, which replaces it with a PBKDF2 hash (auth._verify_password).
        # Hashing every DB user here would slow startup by ~0.2 s per user.
        store.USERS[uname] = {
            "user": uname,
            "full_name": _s(r.get("UserName")),
            "password_hash": None,
            "default_password": True,   # until changed: the lowercase user code (e.g. office/office)
            "legacy_password": pwd_raw,
            "role": _role_for(uname, _s(r.get("ROLECODE")), _s(r.get("ROLENAME"))),
            "active": _s(r.get("Inactive")) != "Y",
            # The exe signs in only users it has authorised ("Not a valid User!"); ADMIN is exempt. A row with no value counts as authorised.
            "authorised": _s(r.get("Authorised")).upper() != "N",
            "pwv": 1,
        }
    logger.info("Loaded %d users into store", len(store.USERS))


def _drop_loaded(docs: dict) -> None:
    """Remove previously loaded DB documents before a (re)load, keeping bills made in the app.
    (This used to clear everything, which silently deleted bills saved while the DB was loading.)"""
    for no in [no for no, d in docs.items() if d.get("origin") == "db"]:
        del docs[no]


def _doc_key(printed: str, short: str) -> str:
    """The number a saved document is known by: the exe-style number ("2600202", the header's own number column; its printed form is "26//00202"),
    or, for the web's older "26/SR-0000001" numbers (which don't fit the varchar(12) column), the printed one."""
    return printed if "-" in printed or not short else short


def short_no(doc_no: str) -> str:
    """A document number as the desktop's varchar(12) header columns hold it (PBNO, PINO, SRNO, STKDEDNO): the web's
    "26/PB-0000001" is written as "26PB0000001"; a number that already fits is unchanged."""
    return doc_no if len(doc_no) <= 12 else doc_no.replace("/", "").replace("-", "")


def _add_loaded(docs: dict, bills: dict) -> None:
    """Add bills read from the legacy-format tables, never over a document the web app made (the web writes
    its own bills there too; the whole document comes from Web_AppDoc). A web bill is also found under the short
    number its header row carries, so it is not listed twice."""
    web_short = {short_no(no) for no, d in docs.items() if "origin" not in d}
    for no, b in bills.items():
        if (no in docs and "origin" not in docs[no]) or no in web_short:
            continue
        docs[no] = b


def _recent_sinv_numbers(limit: int) -> list[str] | None:
    """The numbers of the latest `limit` desktop-made bills of this financial year, found through the SINVNO index.
    SINVNO is the table's clustered key and SINVID has no index, so `TOP n ... ORDER BY SINVID DESC` reads the whole
    700,000-row table (557 MB, 25 s on a cold cache) and, with the one shared connection, holds up every request
    meanwhile. Here each bill series ('26/SR-', '26/C1-', ...) of the year gives its last `limit` numbers by index seek,
    and the newest `limit` by SINVID among them are kept. None when that can't be done (the caller reads the old way)."""
    prefix = f"{store.FY_PREFIX}/"
    if not store.FY_PREFIX:
        return None
    try:
        series = [r["pre"] for r in db.fetchall(
            "SELECT LEFT(SINVNO, CHARINDEX('-', SINVNO)) AS pre FROM TrnHdrSINV WITH (NOLOCK) "
            "WHERE SINVNO LIKE CAST(? AS VARCHAR(20)) AND CHARINDEX('-', SINVNO) > 0 GROUP BY LEFT(SINVNO, CHARINDEX('-', SINVNO))",
            (prefix + "%",)) if r["pre"]]
        found: list[tuple[int, str]] = []
        for pre in series:
            for r in db.fetchall(
                    f"SELECT TOP {int(limit)} SINVID, SINVNO FROM TrnHdrSINV WITH (NOLOCK) WHERE SINVNO LIKE CAST(? AS VARCHAR(30)) "
                    "" + _not_web() + " ORDER BY SINVNO DESC", (pre + "%",)):
                found.append((int(r["SINVID"]), _s(r["SINVNO"])))
        found.sort(reverse=True)
        return [no for _, no in found[:limit]] or None
    except Exception as e:   # noqa: BLE001 - the slow query below still gives the same bills
        logger.warning("Latest bills by index failed (%s); reading them the slow way", e)
        return None


def _not_web() -> str:
    """Older mode only: bills made by the web app came whole from Web_AppDoc, so the desktop-table read left them out. In direct mode (no Web_
    tables) they are rows of the desktop's own tables like any other bill and are read here."""
    from . import db as _db
    return "" if not _db.use_web_tables() else " AND ISNULL(BILLSYSTEM, '') <> 'WEB'"


_col_cache: dict[tuple, set] = {}


def _has(table: str) -> set:
    """The lower-case column names of a table, read once per connection (client databases differ in their optional columns)."""
    key = (db.DB_NAME, table)   # per database: the app can be pointed at another one
    if key not in _col_cache:
        try:
            _col_cache[key] = {r["name"].lower() for r in db.fetchall("SELECT name FROM sys.columns WHERE object_id = OBJECT_ID(?)", (f"dbo.{table}",))}
        except Exception:   # noqa: BLE001
            return set()
    return _col_cache[key]


def _optional(table: str, names: tuple, prefix: str = "") -> str:
    """', h.A, NULL AS B ...': each optional column the table has, and NULL in its place for one it lacks, so a query never names a missing column."""
    have = _has(table)
    return "".join(f", {prefix}{n}" if n.lower() in have else f", NULL AS {n}" for n in names)


def _other_payments(sinv_ids: set) -> dict[int, dict]:
    """The split of a bill's OTHPAYAMT into UPI and credit, from TrnMultiAmt (the rows the exe and the web write for it); {} when the table or
    the designer document is not there (the whole amount is then shown as UPI)."""
    out: dict[int, dict] = {}
    if not sinv_ids:
        return out
    try:
        from . import doc_engine
        doc_id = doc_engine.doc_id_of("Sales Invoice")
        if doc_id is None:
            return out
        ids = sorted(sinv_ids)
        for i in range(0, len(ids), 500):
            chunk = ids[i:i + 500]
            for r in db.fetchall(f"SELECT TrnID, Particulars, Amount FROM TrnMultiAmt WITH (NOLOCK) WHERE DocID = ? AND FldCode = 'OTHPAYAMT' AND TrnID IN ({','.join('?' * len(chunk))})",
                                 (doc_id, *chunk)):
                p = out.setdefault(int(r["TrnID"]), {"upi": 0.0, "credit": 0.0})
                p["credit" if "CREDIT" in _s(r.get("Particulars")).upper() else "upi"] += _f(r.get("Amount"))
    except Exception as e:   # noqa: BLE001 - no such table: the other payments stay one amount
        logger.warning("TrnMultiAmt not read: %s", e)
    return out


def load_sales_invoices(limit: int = 200, days: tuple[date, date] | None = None) -> None:
    """Load the most recent sales invoices into DOCS['sales_invoice'] fast. `days` (first, last) loads the bills of those dates instead
    and keeps what is already loaded (used by `ensure_days` for dates older than the latest bills)."""
    numbers = None if days else _recent_sinv_numbers(limit)
    opt = _optional("TrnHdrSINV", ("TOTDISCAMT", "GRNDTOT", "OTHPAYAMT", "SALTYP", "IsDelete", "CancelRmks"))
    if days:
        recent = (f"SELECT SINVID, SINVNO, SINVDate, CUSTID, TOTAMT, CASHAMT, CARDAMT, TOTQTY, SPID, SCID, AddedBy, Cancelled, SINVTime{opt} "
                  f"FROM TrnHdrSINV WITH (NOLOCK) WHERE SINVDate >= '{days[0].isoformat()}' AND SINVDate < '{(days[1] + timedelta(days=1)).isoformat()}'"
                  + _not_web())   # cancelled and deleted bills are read too: the Re-Print / Edit list shows them (status CANCELLED / DELETED)
    elif numbers:
        in_list = ", ".join("'" + n.replace("'", "''") + "'" for n in numbers)   # literals: the index is used (see above)
        recent = f"SELECT SINVID, SINVNO, SINVDate, CUSTID, TOTAMT, CASHAMT, CARDAMT, TOTQTY, SPID, SCID, AddedBy, Cancelled, SINVTime{opt} FROM TrnHdrSINV WITH (NOLOCK) WHERE SINVNO IN ({in_list})"
    else:
        recent = f"""SELECT TOP {limit}
                SINVID, SINVNO, SINVDate, CUSTID,
                TOTAMT, CASHAMT, CARDAMT, TOTQTY,
                SPID, SCID, AddedBy, Cancelled, SINVTime{opt}
            FROM TrnHdrSINV WITH (NOLOCK)
            WHERE 1 = 1{_not_web()}
            ORDER BY SINVID DESC"""
    rows = db.fetchall(f"""
        WITH RecentHdr AS ({recent})
        SELECT
            h.SINVID, h.SINVNO, h.SINVDate, h.CUSTID,
            h.TOTAMT, h.CASHAMT, h.CARDAMT, h.TOTQTY,
            h.SPID, h.SCID, h.AddedBy,
            h.Cancelled, h.IsDelete, h.CancelRmks, h.SINVTime, h.TOTDISCAMT, h.GRNDTOT, h.OTHPAYAMT, h.SALTYP, cu.CUSTCODE,
            d.SINVDtlID, d.ITEMID, d.SINVQty, d.SINVRate, d.SINVAmt,
            d.CGSTPER, d.CGSTAMT, d.SGSTPER, d.SGSTAMT
        FROM RecentHdr h
        JOIN TrnDtlSINV d WITH (NOLOCK) ON d.SINVID = h.SINVID
        LEFT JOIN MstCUST cu WITH (NOLOCK) ON cu.CUSTID = h.CUSTID
        ORDER BY h.SINVID DESC
    """)

    # Build item code lookup from ITEMID → code
    item_by_id: dict[str, str] = {}
    for code, item in store.ITEMS.items():
        # We need ITEMID from DB — store it during item load
        item_by_id[item.get("_itemid", "")] = code

    counter_by_id = {c.get("_scid"): code for code, c in store.COUNTERS.items() if c.get("_scid")}
    person_by_id = {p.get("_spid"): p.get("name") or code for code, p in store.SALES_PERSONS.items() if p.get("_spid")}

    docs = store.DOCS["sales_invoice"]
    if not days:
        _drop_loaded(docs)

    parts = _other_payments({int(r["SINVID"]) for r in rows})   # SINVID -> {'upi': .., 'credit': ..} (the split of OTHPAYAMT)
    bills: dict = {}
    for r in rows:
        sinv_no = _s(r.get("SINVNO"))
        if sinv_no not in bills:
            t = _f(r.get("TOTAMT"))
            cash, card, other = _f(r.get("CASHAMT")), _f(r.get("CARDAMT")), _f(r.get("OTHPAYAMT"))
            split = parts.get(int(r["SINVID"])) or ({"upi": other, "credit": 0.0} if other else {"upi": 0.0, "credit": 0.0})
            paid = {"cash": cash, "card": card, "upi": split["upi"], "credit": split["credit"]}
            used = [k for k, v in paid.items() if v > 0]
            saltyp = _s(r.get("SALTYP")).upper()
            cust_code = _s(r.get("CUSTCODE"))
            bills[sinv_no] = {
                "doc_no": sinv_no,
                "date": str(r.get("SINVDate", ""))[:10],
                "time": _s(r.get("SINVTime")) or "00:00:00",
                "counter": counter_by_id.get(_s(r.get("SCID")), _s(r.get("SCID"))),
                "customer": cust_code if cust_code and cust_code != "-" and int(r.get("CUSTID") or 0) > 0 else "CASH",
                "salesperson": person_by_id.get(_s(r.get("SPID"))) or _s(r.get("AddedBy")),
                "status": "DELETED" if _s(r.get("IsDelete")).upper() == "Y" else "CANCELLED" if _s(r.get("Cancelled")).upper() == "Y" else "SAVED",
                "added_by": _s(r.get("AddedBy")),
                "cancel_remarks": _s(r.get("CancelRmks")).strip(),
                "cancelled": _s(r.get("Cancelled")).upper() == "Y",   # the two columns as the desktop's Re-Print list shows them (a deleted bill can have Cancelled N)
                "deleted": _s(r.get("IsDelete")).upper() == "Y",
                "payment_type": saltyp if saltyp in ("CASH", "CARD", "UPI", "CREDIT", "SPLIT") else (used[0].upper() if len(used) == 1 else "SPLIT" if used else "CASH"),
                "lines": [],
                "totals": {
                    "qty": _f(r.get("TOTQTY")),
                    "litres": 0.0,
                    "basic": t,
                    "cgst": 0.0,
                    "sgst": 0.0,
                    "discount": _f(r.get("TOTDISCAMT")),
                    "points_redeemed": 0.0,
                    "points_earned": 0.0,
                    "net": _f(r.get("GRNDTOT")) or t,
                },
                "payment": paid,
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
            "cgst": _f(r.get("CGSTAMT")),
            "sgst": _f(r.get("SGSTAMT")),
            "litres": round(qty * ltr_per_btl, 3),
        }
        bills[sinv_no]["lines"].append(line)
        bills[sinv_no]["totals"]["litres"] = round(
            bills[sinv_no]["totals"]["litres"] + line["litres"], 3
        )

    for b in bills.values():
        b["origin"] = "db"
    _add_loaded(docs, bills)
    # The status a desktop bill already has in SQL Server counts as written: without this, every cancelled / deleted bill read above would
    # look changed and be re-written (remark blanked, "changed by" stamped) on the first save.
    from . import sqlstore
    for no, b in bills.items():
        if docs.get(no) is b:
            sqlstore._doc_status[("sales_invoice", no)] = b["status"]
    logger.info("Loaded %d sales invoices from DB", len(docs))


def load_purchase_bills(limit: int = 100, days: tuple[date, date] | None = None) -> None:
    """Load the most recent purchase bills fast (or, with `days`, those of that first..last date, keeping what is loaded)."""
    pick = (f"TOP {limit}" if not days else "")
    names = ("SUPPINVNO", "SUPPINVDATE", "SUPPINVVALUE", "PURCHASETYPE", "TCSAMT", "TOTCESS", "GRNDTOT", "RMKS", "Cancelled")
    opt, hopt, dopt = _optional("TrnHdrPB", names), _optional("TrnHdrPB", names, "h."), _optional("TrnDtlPB", ("CGSTAMT", "SGSTAMT"), "d.")
    where = (f"WHERE PBDate >= '{days[0].isoformat()}' AND PBDate < '{(days[1] + timedelta(days=1)).isoformat()}'" if days else "")
    rows = db.fetchall(f"""
        WITH RecentPB AS (
            SELECT {pick} PBID, PBNO, PBDate, SUPPID, PartyName{opt}
            FROM TrnHdrPB WITH (NOLOCK)
            {where}
            ORDER BY PBID DESC
        )
        SELECT
            h.PBID, h.PBNO, h.PBDate, h.SUPPID, h.PartyName{hopt},
            su.SUPPCODE, d.PBDtlID, d.ITEMID, d.PBQty, d.PBRate, d.PBAmt, d.NOOFCASES{dopt}
        FROM RecentPB h
        JOIN TrnDtlPB d WITH (NOLOCK) ON d.PBID = h.PBID
        LEFT JOIN MstSUPP su WITH (NOLOCK) ON su.SUPPID = h.SUPPID
        ORDER BY h.PBID DESC
    """)

    item_by_id: dict[str, str] = {}
    for code, item in store.ITEMS.items():
        item_by_id[item.get("_itemid", "")] = code

    docs = store.DOCS["purchase_bill"]
    if not days:
        _drop_loaded(docs)
    bills: dict = {}
    for r in rows:
        pb_no = _s(r.get("PBNO"))
        if pb_no not in bills:
            bills[pb_no] = {
                "doc_no": pb_no,
                "date": str(r.get("PBDate", ""))[:10],
                "supplier": _s(r.get("SUPPCODE")) or _s(r.get("PartyName")),
                "supplier_inv_no": _s(r.get("SUPPINVNO")),
                "supplier_inv_date": str(r.get("SUPPINVDATE") or "")[:10] or str(r.get("PBDate", ""))[:10],
                "supplier_inv_value": _f(r.get("SUPPINVVALUE")),
                "purchase_type": _s(r.get("PURCHASETYPE")) or "Purchase",
                "remarks": _s(r.get("RMKS")),
                "status": "CANCELLED" if _s(r.get("Cancelled")).upper() == "Y" else "SAVED",
                "tcs": _f(r.get("TCSAMT")), "cess": _f(r.get("TOTCESS")),
                "lines": [],
                "totals": {"qty": 0, "basic": 0.0, "cgst": 0, "sgst": 0, "tcs": _f(r.get("TCSAMT")), "cess": _f(r.get("TOTCESS")), "net": 0.0},
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
            "cgst": _f(r.get("CGSTAMT")),
            "sgst": _f(r.get("SGSTAMT")),
            "litres": round(qty * 0.75, 3),
        })
        bills[pb_no]["totals"]["cgst"] = round(bills[pb_no]["totals"]["cgst"] + _f(r.get("CGSTAMT")), 2)
        bills[pb_no]["totals"]["sgst"] = round(bills[pb_no]["totals"]["sgst"] + _f(r.get("SGSTAMT")), 2)
        bills[pb_no]["totals"]["qty"] += qty
        bills[pb_no]["totals"]["basic"] = round(bills[pb_no]["totals"]["basic"] + amt, 2)
        bills[pb_no]["totals"]["net"] = bills[pb_no]["totals"]["basic"]
        if _f(r.get("GRNDTOT")):
            bills[pb_no]["totals"]["net"] = _f(r.get("GRNDTOT"))

    for b in bills.values():
        b["origin"] = "db"
    _add_loaded(docs, bills)
    logger.info("Loaded %d purchase bills from DB", len(docs))


def load_closing_stock(limit: int = 2000) -> None:
    """Closing Stock Entries made in the desktop (TrnHdrCSE / TrnDtlCSE, the Drops client's frmTrnCloseStk), newest `limit`; a database without those
    tables has none. Only the rows with a closing quantity or a sales quantity are kept (the exe stores a row for every item)."""
    if not _has("TrnHdrCSE") or not _has("TrnDtlCSE"):
        return
    rows = db.fetchall(f"""
        WITH R AS (SELECT TOP {int(limit)} CSEID, CSENO, PrintCSENO, CSEDate, ASONDT, Cancelled, AddedBy, INVNO FROM TrnHdrCSE WITH (NOLOCK) ORDER BY CSEID DESC)
        SELECT h.CSEID, h.CSENO, h.PrintCSENO, h.CSEDate, h.ASONDT, h.Cancelled, h.AddedBy, h.INVNO, d.ITEMID, d.OPQTY, d.CLQTY, d.INVQTY
        FROM R h JOIN TrnDtlCSE d WITH (NOLOCK) ON d.CSEID = h.CSEID
        WHERE d.CLQTY > 0 OR d.INVQTY > 0
        ORDER BY h.CSEID DESC, d.SlNo
    """)
    item_by_id = {str(i.get("_itemid", "")): c for c, i in store.ITEMS.items()}
    docs: dict = {}
    for r in rows:
        no = _doc_key(_s(r.get("PrintCSENO")), _s(r.get("CSENO")))
        doc = docs.setdefault(no, {"doc_no": no, "date": str(r.get("ASONDT") or r.get("CSEDate") or "")[:10], "lines": [], "invoice_no": _s(r.get("INVNO")) or None,
                                   "total_amount": 0.0, "added_by": _s(r.get("AddedBy")),
                                   "status": "CANCELLED" if _s(r.get("Cancelled")).upper() == "Y" else "SAVED", "origin": "db"})
        code = store.merged_code(store.MERGED_ITEM_IDS.get(str(r.get("ITEMID"))) or item_by_id.get(str(r.get("ITEMID")), str(r.get("ITEMID"))))
        item = store.ITEMS.get(code, {})
        sales = _f(r.get("INVQTY"))
        amount = round(sales * float(item.get("mrp") or 0), 2)
        doc["lines"].append({"item_code": code, "op_qty": _f(r.get("OPQTY")), "closing_qty": _f(r.get("CLQTY")), "sales_qty": sales, "mrp": item.get("mrp", 0), "amount": amount})
        doc["total_amount"] = round(doc["total_amount"] + amount, 2)
    _add_loaded(store.DOCS["closing_stock"], docs)
    logger.info("Loaded %d closing stock entries from DB", len(docs))


def load_expenses(limit: int = 5000) -> None:
    """Expense entries made in the desktop (TrnHdrEXP / TrnDtlEXP), newest `limit`. A line's AMT is an expense and its RAMT a
    receipt into the petty cash fund. They are read-only here (origin "db"); entries made in the web app live in Web_AppDoc."""
    rows = db.fetchall(f"""
        WITH Recent AS (
            SELECT TOP {int(limit)} EXPID, EXPNO, PrintEXPNO, EXPDate, Cancelled, AddedBy, RMKS
            FROM TrnHdrEXP WITH (NOLOCK) ORDER BY EXPID DESC
        )
        SELECT h.EXPID, h.EXPNO, h.PrintEXPNO, h.EXPDate, h.Cancelled, h.AddedBy, h.RMKS,
               d.AMT, d.RAMT, d.REMARKS, l.LEDCODE, l.LEDNAME
        FROM Recent h
        JOIN TrnDtlEXP d WITH (NOLOCK) ON d.EXPID = h.EXPID
        LEFT JOIN MstLed l WITH (NOLOCK) ON l.LEDID = d.LEDID
        ORDER BY h.EXPID DESC, d.SlNo
    """)
    docs: dict = {}
    for r in rows:
        no = _s(r.get("PrintEXPNO")) or _s(r.get("EXPNO"))
        doc = docs.setdefault(no, {"doc_no": no, "date": str(r.get("EXPDate", ""))[:10], "paid_from": "CASH", "lines": [], "total": 0.0,
                                   "receipts": 0.0, "added_by": _s(r.get("AddedBy")), "remarks": _s(r.get("RMKS")).strip(),
                                   "status": "CANCELLED" if _s(r.get("Cancelled")) == "Y" else "SAVED", "origin": "db"})
        amt, rcpt = _f(r.get("AMT")), _f(r.get("RAMT"))
        name = _s(r.get("LEDNAME"))
        doc["lines"].append({"ledger": _s(r.get("LEDCODE")) or name, "ledger_name": name, "amount": amt, "receipt": rcpt,
                             "narration": _s(r.get("REMARKS")).strip()})
        doc["total"] = round(doc["total"] + amt, 2)
        doc["receipts"] = round(doc["receipts"] + rcpt, 2)
    mine = store.DOCS["expense"]
    for no in [no for no, d in mine.items() if d.get("origin") == "db"]:
        del mine[no]
    mine.update(docs)
    logger.info("Loaded %d desktop expense entries", len(docs))


# ─── Other documents the desktop keeps (read-only): sales returns, stock deductions, purchase indents, closing stock, physical stock ───

def _doc_line(item_by_id: dict, item_id, qty: float, rate: float, amount: float | None = None, litres: float | None = None) -> dict:
    """One document line in the shape the web screens and reports use, from a desktop detail row."""
    code = store.merged_code(store.MERGED_ITEM_IDS.get(str(item_id)) or item_by_id.get(str(item_id), str(item_id)))
    item = store.ITEMS.get(code, {})
    per_case = max(int(item.get("bottles_per_case", 12)), 1)
    return {"item_code": code, "item_name": item.get("name", code), "cases": int(qty) // per_case, "bottles": int(qty) % per_case, "qty": qty,
            "rate": rate, "amount": round(qty * rate, 2) if amount is None else amount,
            "litres": round(qty * item.get("litres_per_bottle", 0), 3) if litres is None else litres}


def _totals(lines: list[dict], net: float | None = None) -> dict:
    basic = round(sum(l["amount"] for l in lines), 2)
    return {"qty": sum(l["qty"] for l in lines), "basic": basic, "cgst": 0, "sgst": 0, "net": basic if net is None else net,
            "litres": round(sum(l["litres"] for l in lines), 3)}


def _replace_loaded(kind: str, docs: dict) -> None:
    """Swap the documents read from the database for a fresh set; documents made in the web app are never touched."""
    mine = store.DOCS[kind]
    for no in [no for no, d in mine.items() if d.get("origin") == "db"]:
        del mine[no]
    for no, d in docs.items():
        if no not in mine:
            d["origin"] = "db"
            mine[no] = d


def load_desktop_documents(limit: int = 5000) -> None:
    """Sales returns, stock deductions, purchase indents, closing stock and physical stock entries made in the desktop (newest `limit` of
    each). Each kind is read on its own, so a client database without one of the tables still loads the rest."""
    item_by_id = {str(it.get("_itemid", "")): code for code, it in store.ITEMS.items()}
    rate_of = lambda code: store.ITEMS.get(code, {}).get("purchase_rate", 0.0)   # noqa: E731

    def run(name: str, fn) -> None:
        try:
            fn()
        except Exception as e:  # noqa: BLE001 - this client's database lacks the table, or has it differently
            logger.warning("%s not loaded: %s", name, str(e)[:160])

    def sales_returns():
        rows = db.fetchall(f"""
            WITH R AS (SELECT TOP {int(limit)} SRID, SRNO, PrintSRNO, SRDate, Cancelled, AddedBy, TOTAMT FROM TrnHdrSR WITH (NOLOCK) ORDER BY SRID DESC)
            SELECT h.SRID, h.SRNO, h.PrintSRNO, h.SRDate, h.Cancelled, h.AddedBy, h.TOTAMT, d.ITEMID, d.SRQty, d.SRRate, d.SRAmt, d.SRQTYINLTRS
            FROM R h JOIN TrnDtlSR d WITH (NOLOCK) ON d.SRID = h.SRID ORDER BY h.SRID DESC, d.SlNo""")
        docs: dict = {}
        for r in rows:
            no = _doc_key(_s(r.get("PrintSRNO")), _s(r.get("SRNO")))
            d = docs.setdefault(no, {"doc_no": no, "date": str(r.get("SRDate", ""))[:10], "customer": "CASH", "against_invoice": None, "lines": [],
                                     "remarks": "", "added_by": _s(r.get("AddedBy")), "head_total": _f(r.get("TOTAMT")),
                                     "status": "CANCELLED" if _s(r.get("Cancelled")) == "Y" else "SAVED"})
            d["lines"].append(_doc_line(item_by_id, r.get("ITEMID"), _f(r.get("SRQty")), _f(r.get("SRRate")), _f(r.get("SRAmt")), _f(r.get("SRQTYINLTRS"))))
        for d in docs.values():
            d["totals"] = _totals(d["lines"], d.pop("head_total"))
        _replace_loaded("sales_return", docs)
        logger.info("Loaded %d desktop sales returns", len(docs))

    def stock_deductions():
        rows = db.fetchall(f"""
            WITH R AS (SELECT TOP {int(limit)} STKDEDID, STKDEDNO, PrintSTKDEDNO, STKDEDDate, Cancelled, AddedBy FROM TrnHdrSTKDED WITH (NOLOCK) ORDER BY STKDEDID DESC)
            SELECT h.STKDEDID, h.STKDEDNO, h.PrintSTKDEDNO, h.STKDEDDate, h.Cancelled, h.AddedBy, d.ITEMID, d.STKDEDQty, d.STKDEDQTYINLTRS
            FROM R h JOIN TrnDtlSTKDED d WITH (NOLOCK) ON d.STKDEDID = h.STKDEDID ORDER BY h.STKDEDID DESC, d.SlNo""")
        docs: dict = {}
        for r in rows:
            no = _doc_key(_s(r.get("PrintSTKDEDNO")), _s(r.get("STKDEDNO")))
            d = docs.setdefault(no, {"doc_no": no, "date": str(r.get("STKDEDDate", ""))[:10], "reason": "Other", "lines": [], "remarks": "",
                                     "added_by": _s(r.get("AddedBy")), "status": "CANCELLED" if _s(r.get("Cancelled")) == "Y" else "SAVED"})
            item_code = store.merged_code(store.MERGED_ITEM_IDS.get(str(r.get("ITEMID"))) or item_by_id.get(str(r.get("ITEMID")), str(r.get("ITEMID"))))
            d["lines"].append(_doc_line(item_by_id, r.get("ITEMID"), _f(r.get("STKDEDQty")), rate_of(item_code), litres=_f(r.get("STKDEDQTYINLTRS"))))
        for d in docs.values():
            d["totals"] = _totals(d["lines"])
        _replace_loaded("stock_deduction", docs)
        logger.info("Loaded %d desktop stock deductions", len(docs))

    def purchase_indents():
        rows = db.fetchall(f"""
            WITH R AS (SELECT TOP {int(limit)} PIID, PINO, PrintPINO, PIDate, SUPPID, PartyName, Cancelled, AddedBy, RMKS, GRNDTOT FROM TrnHdrPI WITH (NOLOCK) ORDER BY PIID DESC)
            SELECT h.PIID, h.PINO, h.PrintPINO, h.PIDate, h.PartyName, su.SUPPCODE, h.Cancelled, h.AddedBy, h.RMKS, h.GRNDTOT, d.ITEMID, d.PIQty, d.PIRate, d.PIAmt
            FROM R h JOIN TrnDtlPI d WITH (NOLOCK) ON d.PIID = h.PIID LEFT JOIN MstSUPP su WITH (NOLOCK) ON su.SUPPID = h.SUPPID ORDER BY h.PIID DESC, d.SlNo""")
        docs: dict = {}
        for r in rows:
            no = _doc_key(_s(r.get("PrintPINO")), _s(r.get("PINO")))
            d = docs.setdefault(no, {"doc_no": no, "date": str(r.get("PIDate", ""))[:10], "supplier": _s(r.get("SUPPCODE")) or _s(r.get("PartyName")), "lines": [],
                                     "remarks": _s(r.get("RMKS")).strip(), "added_by": _s(r.get("AddedBy")), "head_total": _f(r.get("GRNDTOT")),
                                     "status": "CANCELLED" if _s(r.get("Cancelled")) == "Y" else "SAVED"})
            d["lines"].append(_doc_line(item_by_id, r.get("ITEMID"), _f(r.get("PIQty")), _f(r.get("PIRate")), _f(r.get("PIAmt"))))
        for d in docs.values():
            d["totals"] = _totals(d["lines"], d.pop("head_total"))
        _replace_loaded("purchase_indent", docs)
        logger.info("Loaded %d desktop purchase indents", len(docs))

    def closing_stock():
        rows = db.fetchall(f"""
            WITH R AS (SELECT TOP {int(limit)} CSEID, CSENO, PrintCSENO, CSEDate, ASONDT, INVNO FROM TrnHdrCSE WITH (NOLOCK) ORDER BY CSEID DESC)
            SELECT h.CSEID, h.CSENO, h.PrintCSENO, h.CSEDate, h.ASONDT, h.INVNO, d.ITEMID, d.OPQTY, d.CLQTY, d.INVQTY
            FROM R h JOIN TrnDtlCSE d WITH (NOLOCK) ON d.CSEID = h.CSEID ORDER BY h.CSEID DESC, d.SlNo""")
        docs: dict = {}
        for r in rows:
            no = _s(r.get("PrintCSENO")) or _s(r.get("CSENO"))
            d = docs.setdefault(no, {"doc_no": no, "date": str(r.get("ASONDT") or r.get("CSEDate") or "")[:10], "lines": [],
                                     "invoice_no": _s(r.get("INVNO")) or None, "total_amount": 0.0})
            code = store.merged_code(store.MERGED_ITEM_IDS.get(str(r.get("ITEMID"))) or item_by_id.get(str(r.get("ITEMID")), str(r.get("ITEMID"))))
            mrp = store.ITEMS.get(code, {}).get("mrp", 0.0)
            sold = _f(r.get("INVQTY"))
            d["lines"].append({"item_code": code, "op_qty": _f(r.get("OPQTY")), "closing_qty": _f(r.get("CLQTY")), "sales_qty": sold,
                               "mrp": mrp, "amount": round(sold * mrp, 2)})
            d["total_amount"] = round(d["total_amount"] + sold * mrp, 2)
        _replace_loaded("closing_stock", docs)
        logger.info("Loaded %d desktop closing stock entries", len(docs))

    def physical_stock():
        rows = db.fetchall(f"""
            WITH R AS (SELECT TOP {int(limit)} PSEID, PSEDATE, SYSNAME, USERNAME FROM TrnHdrPSE WITH (NOLOCK) ORDER BY PSEID DESC)
            SELECT h.PSEID, h.PSEDATE, h.SYSNAME, h.USERNAME, d.ItemID, d.PRate, d.MRate, d.ClQty, d.QOH, d.DiffQty, d.PBNO, d.StkDedNo
            FROM R h JOIN TrnDtlPSE d WITH (NOLOCK) ON d.PSEID = h.PSEID ORDER BY h.PSEID DESC, d.Slno""")
        docs: dict = {}
        for r in rows:
            no = f"PSE-{r.get('PSEID')}"
            d = docs.setdefault(no, {"doc_no": no, "date": str(r.get("PSEDATE", ""))[:10], "added_by": _s(r.get("USERNAME")),
                                     "sys_name": _s(r.get("SYSNAME")), "reduced_as_sales": False, "lines": []})
            code = store.merged_code(store.MERGED_ITEM_IDS.get(str(r.get("ItemID"))) or item_by_id.get(str(r.get("ItemID")), str(r.get("ItemID"))))
            d["lines"].append({"item_code": code, "old_pur_rate": None, "old_mrp": None, "pur_rate": _f(r.get("PRate")), "mrp": _f(r.get("MRate")),
                               "physical_qty": _f(r.get("ClQty")), "system_qty": _f(r.get("QOH")), "diff_qty": _f(r.get("DiffQty")),
                               "pb_no": _s(r.get("PBNO")), "stkded_no": _s(r.get("StkDedNo"))})
        _replace_loaded("physical_stock", docs)
        logger.info("Loaded %d desktop physical stock entries", len(docs))

    for name, fn in (("Sales returns", sales_returns), ("Stock deductions", stock_deductions), ("Purchase indents", purchase_indents),
                     ("Closing stock entries", closing_stock), ("Physical stock entries", physical_stock)):
        run(name, fn)


def expense_balance_before(on: date) -> float | None:
    """The petty cash balance before a date from the desktop's tables (sum of receipts minus expenses), as its Drops Petty Cash
    report works it out; None when the database is not available or has no such tables."""
    if not db.is_db_available():
        return None
    try:
        row = db.fetchone(f"""SELECT SUM(ISNULL(d.RAMT,0) - ISNULL(d.AMT,0)) AS b FROM TrnHdrEXP h WITH (NOLOCK)
                              JOIN TrnDtlEXP d WITH (NOLOCK) ON d.EXPID = h.EXPID WHERE h.EXPDate < '{on.isoformat()}'""")
    except Exception as e:  # noqa: BLE001 - a database without the expense tables
        logger.warning("Petty cash balance not read: %s", e)
        return None
    return round(_f((row or {}).get("b")), 2)


def expense_by_ledger(first: date, last: date) -> list[dict] | None:
    """Desktop expense and receipt totals per ledger name between two dates (None when the database is not available)."""
    if not db.is_db_available():
        return None
    try:
        rows = db.fetchall(f"""SELECT l.LEDNAME, SUM(d.AMT) AS e, SUM(d.RAMT) AS r FROM TrnHdrEXP h WITH (NOLOCK)
                               JOIN TrnDtlEXP d WITH (NOLOCK) ON d.EXPID = h.EXPID JOIN MstLed l WITH (NOLOCK) ON l.LEDID = d.LEDID
                               WHERE h.EXPDate >= '{first.isoformat()}' AND h.EXPDate <= '{last.isoformat()}' GROUP BY l.LEDNAME ORDER BY 1""")
    except Exception as e:  # noqa: BLE001
        logger.warning("Petty cash ledger totals not read: %s", e)
        return None
    return [{"name": _s(r.get("LEDNAME")), "expense": round(_f(r.get("e")), 2), "receipt": round(_f(r.get("r")), 2)} for r in rows]


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


def load_desktop_rights() -> None:
    """Each user's Add / Edit / Delete and screen rights from the desktop's MstUserRights / MstRoleRights (app/desktop_rights.py)."""
    from . import desktop_rights
    desktop_rights.load()


def load_masters() -> None:
    """Load all master tables from SQL Server (runs in < 0.2s)."""
    t_all = time.perf_counter()
    for fn in (load_company, load_fin_year, load_item_groups, load_brands, load_uoms, load_packages, load_items,
               load_customers, load_suppliers, load_ledger_groups, load_ledgers, load_counters, load_sales_persons,
               load_simple_masters, load_last_purchase, load_users, load_desktop_rights):
        t0 = time.perf_counter()
        try:
            fn()
        except Exception as e:
            logger.error("%s failed: %s", fn.__name__, e)
        if time.perf_counter() - t0 > 0.5:
            logger.info("  %s took %.1fs", fn.__name__, time.perf_counter() - t0)
    logger.info("Master data loaded in %.1fs: %d items, %d users, %d suppliers", time.perf_counter() - t_all, len(store.ITEMS), len(store.USERS), len(store.SUPPLIERS))
    # Movements before this index were there before the load (none: the store starts empty); later ones are the app's.
    store.DB_BASE_MOVEMENTS = len(store.MOVEMENTS)
    store.mark_app_start()


def _count(key: str, default: int) -> int:
    """A positive whole number from the environment (backend/.env), else the default."""
    import os
    try:
        n = int((os.environ.get(key) or "").strip() or default)
    except ValueError:
        return default
    return n if 0 < n <= 100000 else default


def load_transactions() -> None:
    """Load recent sales invoices and purchase bills."""
    # How many of the latest bills are kept in memory: the Dashboard's last 7 days, the daily / hourly sales and the other reports that read
    # the bills in memory only see these. 200 covered about a day for a busy shop (the other days of the chart came out empty), so the
    # default is 3000 sales invoices; RECENT_INVOICES / RECENT_PURCHASE_BILLS in backend/.env change it. Read through the bill-number
    # index, in the background thread that loads the history, so it never holds up sign-in.
    sales, purchases = _count("RECENT_INVOICES", 3000), _count("RECENT_PURCHASE_BILLS", 300)
    t0 = time.perf_counter()
    try:
        load_sales_invoices(limit=sales)
    except Exception as e:
        logger.error("load_sales_invoices failed: %s", e)
    try:
        load_purchase_bills(limit=purchases)
    except Exception as e:
        logger.error("load_purchase_bills failed: %s", e)
    logger.info("Bill history read in %.1fs (latest %d sales invoices, %d purchase bills asked for)", time.perf_counter() - t0, sales, purchases)
    try:
        load_expenses(_count("RECENT_EXPENSES", 5000))
    except Exception as e:  # noqa: BLE001 - a database without the expense tables keeps the web's own entries only
        logger.warning("load_expenses failed: %s", e)
    load_desktop_documents(_count("RECENT_DOCUMENTS", 5000))
    try:
        load_closing_stock()
    except Exception as e:  # noqa: BLE001 - a database without the closing stock tables keeps none
        logger.warning("load_closing_stock failed: %s", e)
    from . import expense_db
    expense_db.load_opening()   # Day's OP-Balance (TrnExpOPBal)
    _remember_oldest()
    logger.info("Transaction data loaded: %d sales invoices, %d purchase bills", len(store.DOCS["sales_invoice"]), len(store.DOCS["purchase_bill"]))


_old_lock = threading.Lock()
_oldest: dict[str, str | None] = {}     # kind -> date of the oldest bill the start-up load brought in
_covered: dict[str, set] = {}           # kind -> older dates already read from SQL Server
MAX_OLD_BILLS = 30000                   # older bills kept in memory at once; past this they are dropped and read again when asked


def _remember_oldest() -> None:
    """After the start-up load: note how far back the bills in memory go, and forget the older dates read on demand."""
    _covered.clear()
    for kind in ("sales_invoice", "purchase_bill"):
        dates = [d["date"] for d in store.DOCS[kind].values() if d.get("origin") == "db" and d.get("date")]
        _oldest[kind] = min(dates) if dates else None


def ensure_days(first: date, last: date, purchases: bool = False) -> None:
    """Make sure the bills of first..last are in memory for a report. The latest bills are loaded at start-up; for a date older than
    those, the missing dates are read from SQL Server once (never on a request for recent dates, so billing and the Dashboard
    are not slowed) and kept. Does nothing when the database is off or the start-up load found no bill."""
    kind = "purchase_bill" if purchases else "sales_invoice"
    oldest = _oldest.get(kind)
    if not oldest or first.isoformat() >= oldest or not db.is_db_available():
        return
    stop = min(last, date.fromisoformat(oldest) - timedelta(days=1))
    with _old_lock:
        done = _covered.setdefault(kind, set())
        missing = [first + timedelta(days=n) for n in range((stop - first).days + 1) if (first + timedelta(days=n)).isoformat() not in done]
        if not missing:
            return
        docs = store.DOCS[kind]
        if sum(1 for d in docs.values() if d.get("origin") == "db") > _count("RECENT_INVOICES", 3000) + MAX_OLD_BILLS:
            _drop_loaded(docs)            # too many old bills kept: start again (the latest ones are read back below)
            done.clear()
            (load_purchase_bills(_count("RECENT_PURCHASE_BILLS", 300)) if purchases else load_sales_invoices(_count("RECENT_INVOICES", 3000)))
            missing = [first + timedelta(days=n) for n in range((stop - first).days + 1)]
        t0 = time.perf_counter()
        try:
            span = (missing[0], missing[-1])
            (load_purchase_bills(days=span) if purchases else load_sales_invoices(days=span))
        except Exception as e:  # noqa: BLE001 - the report then shows what is in memory
            logger.error("Reading older %s bills failed: %s", kind, e)
            return
        done.update(d.isoformat() for d in missing)
        logger.info("Read older %s bills for %s to %s in %.1fs", kind, span[0], span[1], time.perf_counter() - t0)


_recent_lock = threading.Lock()
_recent_at = 0.0


def refresh_recent_sales(max_age: float = 5.0) -> None:
    """Bring in the bills other PCs made since start-up (the Windows vLQMS, another web counter): the bills of yesterday and today are read again,
    at most once every `max_age` seconds, so a bill saved elsewhere shows in the history list and the List window. Bills this app made itself
    are never replaced (see _add_loaded). Does nothing when the database is off."""
    global _recent_at
    if not db.is_db_available() or time.monotonic() - _recent_at < max_age:
        return
    if not _recent_lock.acquire(blocking=False):   # another request is reading them right now
        return
    try:
        _recent_at = time.monotonic()
        today = date.today()
        load_sales_invoices(days=(today - timedelta(days=1), today))
    except Exception as e:  # noqa: BLE001 - the list shows what is in memory
        logger.warning("Refreshing the latest bills failed: %s", e)
    finally:
        _recent_lock.release()


def load_all() -> None:
    """Load all master + transaction data from SQL Server into the store."""
    if not db.is_db_available():
        logger.warning("DB not available — nothing loaded")
        return
    load_masters()
    load_transactions()
