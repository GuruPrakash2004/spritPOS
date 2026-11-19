"""Sales invoice numbers from Invoice Settings (frmTrnInv.getInvNo), so the web and the Windows vLQMS number bills the same way.

The settings are UtlGSettings rows (SetGroup 'Invoice'): InvNoReset ('M' month wise, 'D' day wise, '' none), CounterWsRst ('Y': the running number
restarts per counter), DocNoFmt (the format, e.g. <<5>>/<<2>>-<<7>>) and PrefixSettings (12 month letters). The exe's running number:

    Select max(invCNo) + 1 from TrnHdrSInv where FinYear = <year> [and month(SInvdate) = m [and day(SInvdate) = d]] [and SCID = <counter>]

and its tokens: 1 the month letter (plus the day for 'D'), 2 the counter's prefix, 3 / 4 the two years (4 digits), 5 / 6 (2 digits), 7 the number
as 0000000, 8 the number as it is; then "//" becomes "/", "--" becomes "-" and commas go.

With the settings the web has always numbered by (no reset, no counter reset, the format <<5>>/<<2>>-<<7>>, or no row at all) nothing changes: the
old series (store.next_doc_no, kept in step with the database by sqlstore.sync_number) is used, so a shop that never opens Invoice Settings sees the
same numbers as before. Only a changed format or reset goes through the exe's query here.
"""
import logging
import re
import time
from datetime import date

from fastapi import HTTPException

from . import db, sqlstore, store

logger = logging.getLogger(__name__)
DEFAULT_FORMAT = "<<5>>/<<2>>-<<7>>"
DEFAULT_KEYS = {"f12": "S", "ctrl_f12": "S", "alt_f12": "S"}   # frmTrnInv: getGsettings("Invoice", "INVF12", "S")
NAMES = ("InvNoReset", "CounterWsRst", "DocNoFmt", "PrefixSettings")
_cache: tuple[float, dict] | None = None


def reset_cache() -> None:
    global _cache
    _cache = None


def settings(max_age: float = 5.0) -> dict:
    """{'reset': ''|'M'|'D', 'counter': bool, 'format': str, 'months': [12 letters]}; the defaults when the database is off or has no rows."""
    global _cache
    if _cache and time.monotonic() - _cache[0] < max_age:
        return _cache[1]
    st = {"reset": "", "counter": False, "format": DEFAULT_FORMAT, "months": [], "keys": dict(DEFAULT_KEYS)}
    if sqlstore.db_usable():
        try:
            rows = db.fetchall("SELECT SetName, SetValue FROM UtlGSettings WITH (NOLOCK) WHERE SetGroup = 'Invoice' AND SetName IN ('InvNoReset', 'CounterWsRst', 'DocNoFmt', 'PrefixSettings', 'INVF12', 'INVCTRLF12', 'INVALTF12')")
            got = {str(r["SetName"]).strip().lower(): str(r["SetValue"] or "").strip() for r in rows}
            st["reset"] = got.get("invnoreset", "").upper() if got.get("invnoreset", "").upper() in ("M", "D") else ""
            st["counter"] = got.get("counterwsrst", "").upper() == "Y"
            fmt = got.get("docnofmt") or DEFAULT_FORMAT
            # A format with no running number (the exe's own fallback <<5>>/<<6>>) would give every bill the same number: keep the web's own.
            st["format"] = fmt if ("<<7>>" in fmt or "<<8>>" in fmt) else DEFAULT_FORMAT
            st["months"] = got.get("prefixsettings", "").split(",")
            for key, name in (("f12", "invf12"), ("ctrl_f12", "invctrlf12"), ("alt_f12", "invaltf12")):
                if got.get(name, "").upper() in ("S", "SP", "SPP"):
                    st["keys"][key] = got[name].upper()
        except Exception:   # noqa: BLE001 - keep the defaults: a bill must still get its number
            pass
    _cache = (time.monotonic(), st)
    return st


def is_default(st: dict) -> bool:
    return not st["reset"] and not st["counter"] and st["format"] == DEFAULT_FORMAT


def _year(key: str) -> str:
    m = re.match(r"(\d{4})", str(store.COMPANY.get(key) or ""))
    return m.group(1) if m else ""


def build(st: dict, running: int, counter_prefix: str, on: date) -> str:
    """The number from the format, token by token (the exe's Replace calls, in its order)."""
    out = st["format"]
    month = ""
    if st["reset"] in ("M", "D"):
        months = st["months"]
        month = months[on.month - 1] if len(months) >= on.month else ""
        if st["reset"] == "D":
            month += str(on.day)
    start, end = _year("fy_start"), _year("fy_end")
    for token, value in (("<<1>>", month), ("<<2>>", counter_prefix), ("<<3>>", start), ("<<4>>", end), ("<<5>>", start[-2:]), ("<<6>>", end[-2:]),
                         ("<<7>>", f"{running:07d}"), ("<<8>>", str(running))):
        out = out.replace(token, value)
    return out.replace("//", "/").replace("--", "-").replace(",", "")


def _fin_year() -> int:
    return int(store.FY_PREFIX) if str(store.FY_PREFIX).isdigit() else 0


def _same(doc: dict, st: dict, counter: str, on: date) -> bool:
    d = str(doc.get("date") or "")
    if doc.get("status") == "HOLD" or not doc.get("inv_cno") or len(d) < 10:
        return False
    start, end = str(store.COMPANY.get("fy_start") or "")[:10], str(store.COMPANY.get("fy_end") or "")[:10]
    if start and end and not (start <= d[:10] <= end):   # the exe counts within the financial year (FinYear)
        return False
    if st["reset"] in ("M", "D") and int(d[5:7]) != on.month:
        return False
    if st["reset"] == "D" and int(d[8:10]) != on.day:
        return False
    return not (st["counter"] and doc.get("counter") != counter)


def running_number(st: dict, counter: dict, on: date) -> int:
    """getInvNo's `max(invCNo) + 1` (1 when there is none): the database's highest (other PCs write there too) or the highest of the bills made here."""
    best = 0
    if sqlstore.db_usable() and not db.use_web_tables():
        sql = "SELECT MAX(InvCNo) AS n FROM dbo.TrnHdrSINV WITH (NOLOCK) WHERE FinYear = ?"
        params: list = [_fin_year()]
        if st["reset"] in ("M", "D"):
            sql += " AND MONTH(SINVDate) = ?"
            params.append(on.month)
        if st["reset"] == "D":
            sql += " AND DAY(SINVDate) = ?"
            params.append(on.day)
        if st["counter"]:
            sql += " AND SCID = ?"
            params.append(int(counter.get("_scid") or 0))
        try:
            row = db.fetchone(sql, tuple(params))
            best = int(row["n"]) if row and row["n"] else 0
        except Exception:   # noqa: BLE001 - a database without InvCNo / FinYear, or a hiccup: count from the bills made here
            logger.warning("Highest invoice running number not read; using the bills made here", exc_info=True)
    mine = max((int(d["inv_cno"]) for d in store.DOCS["sales_invoice"].values() if _same(d, st, counter["code"], on)), default=0)
    return max(best, mine) + 1


def next_number(counter: dict, on: date) -> tuple[str, int]:
    """(the invoice number, its running number) for a new bill on this counter and date."""
    st = settings()
    if is_default(st):
        no = store.next_doc_no("sales_invoice", counter["prefix"])
        m = re.search(r"(\d+)$", no)
        return no, int(m.group(1)) if m else 0
    if st["counter"] and "<<2>>" not in st["format"]:   # two counters would make the same number
        raise HTTPException(422, "Counter wise reset is enabled in global settings! Please ensure counter number is a part of doc no. format!")
    n = running_number(st, counter, on)
    return build(st, n, counter["prefix"], on), n


def preview(counter: dict, on: date) -> tuple[str, int]:
    """The number the next bill will most likely get; nothing is taken."""
    st = settings()
    if is_default(st):
        return "", 0
    n = running_number(st, counter, on)
    return build(st, n, counter["prefix"], on), n


def print_keys() -> dict:
    """What F12, Ctrl+F12 and Alt+F12 do after saving a sales bill: 'S' save only, 'SP' save and print, 'SPP' save and preview."""
    return dict(settings()["keys"])
