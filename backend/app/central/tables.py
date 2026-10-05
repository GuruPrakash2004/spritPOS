"""The central tables a shop's Web Sync fills, with the desktop app's names and columns (recovered from
frmModWebSync in vLQMS.exe), plus RptWebStk for the live stock. One list here drives the validation of a push,
the SQL inserts (app/central/repo.py) and is checked against migrations/central/001_central.sql by the tests.

Column kinds: n = amount or quantity, i = whole number, s = text (with its maximum length), d = date or blank.
Every day table also has ClientID and DocDate, which the central site fills in (a shop never sends them).
"""
from datetime import date

N, I, D = ("n", 0), ("i", 0), ("d", 0)


def S(length: int) -> tuple[str, int]:
    return ("s", length)


_LINE = {"slno": I, "CounterNo": S(100), "NoOfBills": I, "BillValue": N}
_GROUP = {"slno": I, "GroupName": S(250), "TotQty": N, "TotAmt": N}

DAY_TABLES: dict[str, dict[str, tuple[str, int]]] = {
    "RptDSRSmry": {
        "TotBills": I, "TotSalesAmt": N, "TotRndOffAmt": N, "SalesAmtLiq": N, "SalesAmtNonLiq": N,
        "SalesAmtCtrWise": N, "SalesProfitAmt": N, "OPStkQty": N, "OPStkValue": N, "ClStkQty": N, "CLStkValue": N,
        "TotCashAmt": N, "TotCardAmt": N, "OtherAmt": N, "CreditAmt": N, "WriteOffAmt": N,
        "TotPurAmt_SalesRate": N, "PurAmtBasic": N, "PurAmtInclGST": N, "TotSalesRetnAmt": N, "CashOPAmt": N,
        "ExpenseAmt": N, "CashBalanceAmt": N, "RedemAmount": N, "LastPurBillNo": S(50), "LastPurDate": D,
        "LastPurValue": N, "MnthPurValue": N, "YearPurValue": N, "DaySalesValue": N, "MnthSalesValue": N,
        "YearSalesValue": N,
    },
    "RptDSRCtrSales": _LINE,
    "RptDSRInvCancelled": _LINE,
    "RptDSRSPWise": _LINE,
    "RptDSRGroupWiseSales": _GROUP,
    "RptDSRGroupWisePur": _GROUP,
    "RptDSRExpLedWise": {"slno": I, "LedgerName": S(250), "LedAmt": N},
    "RptDSR_Gst": {"slno": I, "GST": S(30), "GSTAmt": N},
    "RptDSRPmt": {"SlNo": I, "Particulars": S(100), "PmtAmt": N},
    "RptFastMov": {"ItemName": S(250), "SoldQty": N},
    "RptMinStk": {"ItemName": S(250), "QOH": N, "MinLevel": N, "ShortageQty": N},
    "TrnHdrPI": {
        "PIID": I, "PINO": S(50), "Prefix": S(30), "PrintPINO": S(50), "PIDate": D, "SUPPName": S(250), "LOCID": I,
        "TOTAMT": N, "OTHAMT": N, "GRNDTOT": N, "TOTCGST": N, "TOTSGST": N, "TOTIGST": N,
    },
    "TrnDtlPI": {
        "PIID": I, "PIDtlID": I, "SlNo": I, "ItemCode": S(50), "ITEMName": S(250), "PIQty": N, "PIRate": N,
        "PIAmt": N, "NOOFCASES": I, "NOOFBOTTLES": I, "NETAMT": N, "MRP": N,
    },
}

STOCK_TABLE = "RptWebStk"
STOCK_COLUMNS: dict[str, tuple[str, int]] = {
    "ItemCode": S(50), "ItemName": S(250), "ItemGroupName": S(250), "Brand": S(250), "Package": S(50), "QOH": N,
    "MinLevel": N, "MRP": N, "Active": ("b", 0),
}

MAX_ROWS = 5000          # rows per table in one day's push
MAX_STOCK_ROWS = 100_000


class BadPush(ValueError):
    """The push isn't in the expected shape (the central site answers 422)."""


def _value(kind: tuple[str, int], value, where: str):
    k, length = kind
    if value is None:
        return None if k == "d" else "" if k == "s" else 0
    try:
        if k == "n":
            v = float(value)
            if v != v or abs(v) >= 1e14:   # DECIMAL(18,3) holds less than 10^15
                raise ValueError
            return round(v, 3)
        if k == "i":
            v = int(value)
            if abs(v) > 2_147_483_647:   # SQL Server INT
                raise ValueError
            return v
        if k == "b":
            return bool(value)
        if k == "d":
            return date.fromisoformat(str(value)[:10]).isoformat() if value != "" else None
        text = str(value)
        if len(text) > length:
            raise BadPush(f"{where}: longer than {length} characters")
        return text
    except (TypeError, ValueError) as exc:
        if isinstance(exc, BadPush):
            raise
        raise BadPush(f"{where}: not a valid value") from None


def clean_rows(table: str, columns: dict, rows, limit: int) -> list[dict]:
    """Rows with exactly the table's columns, each converted to its kind; unknown columns are refused."""
    if not isinstance(rows, list):
        raise BadPush(f"{table}: expected a list of rows")
    if len(rows) > limit:
        raise BadPush(f"{table}: more than {limit} rows")
    out = []
    for n, row in enumerate(rows, start=1):
        if not isinstance(row, dict):
            raise BadPush(f"{table} row {n}: expected an object")
        extra = set(row) - set(columns)
        if extra:
            raise BadPush(f"{table} row {n}: unknown column(s) {', '.join(sorted(extra))}")
        out.append({col: _value(kind, row.get(col), f"{table} row {n} {col}") for col, kind in columns.items()})
    return out


def clean_day(tables) -> dict[str, list[dict]]:
    if not isinstance(tables, dict):
        raise BadPush("tables: expected an object")
    unknown = set(tables) - set(DAY_TABLES)
    if unknown:
        raise BadPush(f"Unknown table(s): {', '.join(sorted(unknown))}")
    return {name: clean_rows(name, cols, tables.get(name, []), MAX_ROWS) for name, cols in DAY_TABLES.items()}


def clean_stock(items) -> list[dict]:
    rows = clean_rows(STOCK_TABLE, STOCK_COLUMNS, items, MAX_STOCK_ROWS)
    codes = [r["ItemCode"].strip().upper() for r in rows]   # as SQL Server compares them
    if len(codes) != len(set(codes)) or any(not c for c in codes):
        raise BadPush(f"{STOCK_TABLE}: every item needs its own ItemCode")
    return rows
