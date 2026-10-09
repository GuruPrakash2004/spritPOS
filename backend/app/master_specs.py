"""The desktop's simple code/name masters that the web app adds from one description each (no code per master).

Each spec says which desktop table holds the master (its ID, code and name columns, plus any extra columns), which
store dict keeps its rows in memory, and the module name used for rights and the sidebar. From it:
  - store.py makes the dict, schemas.py the request model, masters.py the /masters/<path> CRUD,
  - db_store.py loads the rows from the desktop table, sqlstore.py saves changes back to it,
  - security.py knows its fields and the frontend lists them (components/masters/masterConfigs.js).
All tables are the desktop's own (MstSUPPGROUP, MstCUSTGROUP, MstSTATE, MstCUR, MstLOC, MstTAX, MstBANK, MstHoliday): the
web app creates no table for them. Pure data: this file imports nothing.
"""
# extras: (field, column, "text" | "num")
SPECS: list[dict] = [
    {"path": "supplier-groups", "title": "Supplier Group Master", "store": "SUPPLIER_GROUPS", "table": "MstSUPPGROUP",
     "id": "SUPPGROUPID", "code": "SUPPGROUPCODE", "name": "SUPPGROUPNAME", "extras": [],
     "used_by": [("MstSUPP", "SUPPGROUPID")]},
    {"path": "customer-groups", "title": "Customer Group Master", "store": "CUSTOMER_GROUPS", "table": "MstCUSTGROUP",
     "id": "CUSTGROUPID", "code": "CUSTGROUPCODE", "name": "CUSTGROUPNAME", "extras": [],
     "used_by": [("MstCUST", "CUSTGROUPID")]},
    {"path": "states", "title": "State Master", "store": "STATES", "table": "MstSTATE",
     "id": "STATEID", "code": "STATECODE", "name": "STATENAME", "extras": [], "used_by": []},
    {"path": "currencies", "title": "Currency Master", "store": "CURRENCIES", "table": "MstCUR",
     "id": "CURID", "code": "CURCODE", "name": "CURNAME",
     "extras": [("decimals", "CurDecimal", "text"), ("words_in_millions", "WORDSINMILLIONS", "text")],
     "used_by": [("MstSUPP", "CurID")]},
    {"path": "locations", "title": "Location Master", "store": "LOCATIONS", "table": "MstLOC",
     "id": "LOCID", "code": "LOCCODE", "name": "LOCNAME", "extras": [], "used_by": []},
    {"path": "taxes", "title": "Tax Master", "store": "TAXES", "table": "MstTAX",
     "id": "TAXID", "code": "TAXCODE", "name": "TAXNAME",
     "extras": [("tax_type", "TAXTYPE", "text"), ("cgst_per", "CGSTPER", "num"), ("sgst_per", "SGSTPER", "num"),
                ("igst_per", "IGSTPER", "num")], "used_by": []},
    {"path": "banks", "title": "Bank Master", "store": "BANKS", "table": "MstBANK",
     "id": "BANKID", "code": "BANKCODE", "name": "BANKNAME",
     "extras": [("display", "DISPLAY", "text"), ("account_name", "ACNAME", "text"), ("account_type", "ACTYPE", "text"),
                ("account_no", "ACNO", "text"), ("branch", "BRANCH", "text"), ("pan", "PANNO", "text"),
                ("micr", "MICRCOD", "text"), ("ifsc", "IFSCCOD", "text")], "used_by": []},
    # The holiday's date is its code (yyyy-mm-dd); the table has no audit columns.
    {"path": "holidays", "title": "Holiday Master", "store": "HOLIDAYS", "table": "MstHoliday",
     "id": "Id", "code": "Hdate", "name": "HolidayName", "extras": [], "used_by": [], "plain": True, "date_code": True},
]

BY_PATH = {s["path"]: s for s in SPECS}
