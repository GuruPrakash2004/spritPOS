"""Which desktop column each typed web field of a master is stored in.

The Doc Designer describes columns (`MstITEM.MRP`), the web screens draw fields by key (`mrp`). To let the designer's
caption, order, mandatory / read-only / hidden flags override a typed field, the browser has to know that pairing;
`web_key_for(path, column)` gives it (and `GET /designer/screen/{id}` sends it as `web_key`). A field not listed here
simply keeps the web app's own presentation. Pure data plus two small lookups; add a line when a typed field is added.
"""
from .master_specs import SPECS

# master path -> {web field key: desktop column}. Not paired on purpose: the web's `active` checkbox (the designer's
# InActive column is "Not Active", the opposite sense, so its caption must not land on it).
_EXPLICIT: dict[str, dict[str, str]] = {
    "items": {
        "code": "ITEMCODE", "name": "ITEMNAME", "short_name": "ShortName", "hsn_code": "HSNCode", "group": "ITEMGroupID",
        "liqtype": "LiqType", "uom": "UOMID", "package": "PKGID", "brand": "BrandID", "bottles_per_case": "BotPerCases",
        "mrp": "MRP", "purchase_rate": "PURRATE", "cgst_per": "CGSTPER", "sgst_per": "SGSTPER", "rol": "ROL", "roq": "ROQ",
    },
    "customers": {"code": "CUSTCODE", "name": "CUSTNAME", "mobile": "MOBNO", "loyalty_card_no": "CARDNO",
                  "send_sms": "SENDSMS", "gstin": "GSTNO", "city": "City"},
    "suppliers": {"code": "SUPPCODE", "name": "SUPPNAME", "alias": "SUPPAliasName", "address": "BAdd1", "city": "City",
                  "pin": "PinCode", "state": "State", "country": "Country", "phone": "PHNO", "gstin": "GSTNO"},
    "item-groups": {"code": "ITEMGROUPCODE", "name": "ITEMGROUPNAME", "liquor": "LiqItem", "consider_for_loyalty": "LOYALTY"},
    "brands": {"code": "BRANDCODE", "name": "BRANDNAME"},
    "uoms": {"code": "UOMCODE", "name": "UOMNAME"},
    "packages": {"litres_per_bottle": "LTRPERBOT"},
    "ledger-groups": {"code": "LEDGROUPCODE", "name": "LEDGROUPNAME"},
    "ledgers": {"code": "LEDCODE", "name": "LEDNAME", "group": "LEDGROUPID"},
    "sales-counters": {"code": "SCCODE", "name": "SCNAME", "system_name": "SYSNAME", "prefix": "PREFIX", "inactive": "InActive"},
    "sales-persons": {"code": "SPCODE", "name": "SPNAME", "mobile": "MOBNO"},
}

# The two bills' own header fields (the web keys of SalesInvoiceIn / PurchaseBillIn), by document path.
# Company Master is the single MstComp row. The web's "excise phone" is the designer's PHNO ("Phone No"): it used to
# be filled from MOBNO, the same value as Mobile.
_EXPLICIT["company"] = {"name": "CompName", "email": "EMAIL", "address1": "Address1", "address2": "Address2", "address3": "Address3", "phone": "TELNO", "fax": "FAXNO", "mobile": "MOBNO", "website": "WEB",
                        "gstin": "GSTNO", "pan": "PANNO", "cin": "CINNO", "excise_phone": "PHNO", "ksbcl_code": "RTLCODE"}
_EXPLICIT["sales-invoice"] = {"date": "SINVDate", "counter": "SCID", "customer": "CUSTID", "salesperson": "SPID",
                              "payment_type": "SALTYP", "discount": "TOTDISCAMT"}
_EXPLICIT["purchase-bill"] = {"date": "PBDate", "supplier": "SUPPID", "supplier_inv_no": "SUPPINVNO", "supplier_inv_date": "SUPPINVDATE",
                              "supplier_inv_value": "SUPPINVVALUE", "purchase_type": "PURCHASETYPE", "remarks": "RMKS",
                              "tcs": "TCSAMT", "cess": "TOTCESS"}
_EXPLICIT["purchase-indent"] = {"date": "PIDate", "supplier": "SUPPID", "remarks": "RMKS"}
_EXPLICIT["sales-return"] = {"date": "SRDate"}
_EXPLICIT["stock-deduction"] = {"date": "STKDEDDate"}
DOC_PATHS = {"trnhdrsinv": "sales-invoice", "trnhdrpb": "purchase-bill", "trnhdrpi": "purchase-indent",
             "trnhdrsr": "sales-return", "trnhdrstkded": "stock-deduction"}   # header table (lower case) -> document path

for _s in SPECS:   # the simple masters are described once in master_specs.py
    _EXPLICIT[_s["path"]] = {"code": _s["code"], "name": _s["name"], **{f: c for f, c, _kind in _s["extras"]}}


# What the web app needs to know about a typed field that the Doc Designer can't say: a caption when no designer row pairs
# with it, and flags the screens act on (opening stock only for a new item, the purchase rate hidden from roles without cost
# access). Everything else about a field comes from the designer.
WEB_HINTS: dict[str, dict[str, dict]] = {
    "items": {"aliases": {"caption": "Item aliases (one per line)"}, "qoh": {"caption": "Opening stock (new items only)", "new_only": True}, "purchase_rate": {"cost": True},
              # the designer hides these key columns (the desktop draws its own drop-downs); the web's choices are the masters' codes
              "group": {"caption": "Item Group", "options_from": "item-groups", "required": True},
              "uom": {"caption": "UOM", "options_from": "uoms", "required": True},
              "brand": {"caption": "Brand", "options_from": "brands"}, "package": {"options_from": "packages", "required": True},
              "liqtype": {"options": ["IML", "Beer", "Wine", "Others"], "required": True}},
    "customers": {"points": {"caption": "Loyalty Points"}},
    "suppliers": {"gstin": {"required": True}},
    "ledgers": {"group": {"caption": "Ledger Group", "options_from": "ledger-groups", "required": True}},
}


def web_fields(path: str) -> list[dict]:
    """The typed fields of a master's request model, as the browser needs them to build the screen from the designer's
    definition: the key, the kind of value the web stores (bool / number / text / list), whether it is required, the
    desktop column it is stored in ('' when none), and the hints above. `code` and the server-only fields are left out."""
    import typing

    from . import schemas, security
    model = security._models(schemas).get(path)
    if model is None:
        return []
    columns = _EXPLICIT.get(path, {})
    out = []
    for key, info in model.model_fields.items():
        if key in ("extras", "authorised", "auth_by", "auth_on", "changed_by"):
            continue
        ann = info.annotation
        args = [a for a in typing.get_args(ann) if a is not type(None)]
        base = args[0] if args else ann
        options = []
        if typing.get_origin(base) is typing.Literal:
            options = [str(a) for a in typing.get_args(base)]
            kind = "list"
        elif typing.get_origin(base) is list:
            kind = "lines"   # a list of text lines (item aliases)
        elif base is bool:
            kind = "bool"
        elif base in (int, float):
            kind = "number"
        else:
            kind = "text"
        default = None if info.is_required() else info.get_default(call_default_factory=True)
        out.append({"key": key, "kind": kind, "required": info.is_required(), "column": columns.get(key, ""), "options": options,
                    "default": default, **WEB_HINTS.get(path, {}).get(key, {})})
    return out


def doc_fields(path: str) -> list[dict]:
    """The typed header keys of a document's request body (date, supplier, remarks ...) with the column each is stored in."""
    return [{"key": key, "column": col} for key, col in _EXPLICIT.get(path, {}).items()]


def web_key_for(path: str, column: str) -> str:
    """The typed web field stored in this desktop column of the master at `path` ('' when there is none)."""
    want = str(column or "").lower()
    for key, col in _EXPLICIT.get(path, {}).items():
        if col.lower() == want:
            return key
    return ""
