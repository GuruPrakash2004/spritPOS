"""The desktop's Customised Reports, run from their own definitions.

The desktop keeps every customised report as data in the shop's database, so a report added or changed in the
desktop app shows up here with no code change:
  UtlReportsCRHdr  one row per report: name, group (Sales Report / Purchase Report / ...), the header table or view,
                   a fixed condition (CondText) and the Crystal file it prints with (RptFileName, often empty)
  UtlReportsCRTbl  the tables the report may join, each with its link condition (e.g. MSTITEM.BRANDID = MSTBRAND.BRANDID)
  UtlReportsCRDtl  the columns: table, field, caption, order, data type (TEXT / NUMERIC / DATE), Sum / Count, total,
                   sub-total, hidden
The report is one SELECT built from them: the header table, the tables its columns need (joined through the link
conditions), the first DATE column limited to the chosen period, the fixed condition, and a GROUP BY when a column is
summed. It runs on the shop's own tables, so bills made on the web and in the desktop are all in it.
Table and field names come from those definition tables, never from the browser; the browser's values are parameters.
"""
import re
import time
from datetime import date, datetime
from decimal import Decimal

from fastapi import HTTPException

from . import db, sqlstore, store

IDENT = re.compile(r"^[A-Za-z_][A-Za-z0-9_]*$")
MAX_ROWS = 50_000
_CACHE: dict = {"at": 0.0, "defs": {}}
CACHE_SECONDS = 60


def _load() -> dict[str, dict]:
    """{report name: definition}, read again at most once a minute."""
    if time.monotonic() - _CACHE["at"] < CACHE_SECONDS and _CACHE["defs"]:
        return _CACHE["defs"]
    if not sqlstore.enabled():
        return {}
    try:
        hdrs = db.fetchall("SELECT CRID, CRName, HdrTblName, ReportGroup, RptFileName, CondText, IsSlnoReq FROM dbo.UtlReportsCRHdr")
        dtls = db.fetchall("SELECT CRID, TblName, FldCode, FldCaption, TotReq, FldAggregate, SubTotOn, FldOrderBy, FldDataType, FldHideCol, FilterReq, AddnlCol "
                           "FROM dbo.UtlReportsCRDtl ORDER BY CRID, FldOrderBy")
        tbls = db.fetchall("SELECT CRID, TblName, TblLinkCond FROM dbo.UtlReportsCRTbl")
    except Exception:   # noqa: BLE001 - no designer tables, or the database is away: use the built-in reports
        return {}
    defs: dict[str, dict] = {}
    for h in hdrs:
        crid = h["CRID"]
        name = str(h["CRName"] or "").strip()
        if not name:
            continue
        defs[name] = {
            "id": crid, "name": name, "group": str(h["ReportGroup"] or "").strip(), "header": str(h["HdrTblName"] or "").strip(),
            "rpt": str(h["RptFileName"] or "").strip(), "cond": str(h["CondText"] or "").strip(), "slno": str(h["IsSlnoReq"] or "N").upper() == "Y",
            "tables": {str(t["TblName"] or "").strip().upper(): str(t["TblLinkCond"] or "").strip() for t in tbls if t["CRID"] == crid},
            "columns": [{"table": str(d["TblName"] or "").strip(), "field": str(d["FldCode"] or "").strip(),
                         "caption": str(d["FldCaption"] or d["FldCode"] or "").strip(), "total": str(d["TotReq"] or "N").upper() == "Y",
                         "aggregate": str(d["FldAggregate"] or "").strip().lower(), "type": str(d["FldDataType"] or "TEXT").strip().upper(),
                         "hidden": str(d["FldHideCol"] or "N").upper() == "Y", "filter": str(d["FilterReq"] or "N").upper() == "Y",
                         "subtotal": str(d["SubTotOn"] or "N").upper() == "Y", "addnl": str(d["AddnlCol"] or "").strip()} for d in dtls if d["CRID"] == crid],
        }
    _CACHE.update(at=time.monotonic(), defs=defs)
    return defs


def reset() -> None:
    _CACHE.update(at=0.0, defs={})


def names() -> list[str]:
    return list(_load())


def definition(name: str) -> dict | None:
    return _load().get(name)


def _joins(d: dict) -> list[str]:
    """JOIN clauses for the tables the columns need, in an order where every link refers to a table already in."""
    hdr = d["header"].upper()
    need = {c["table"].upper() for c in d["columns"]} - {hdr}
    links = d["tables"]
    todo = set(need)
    while todo:   # a table's link condition may need other tables (MSTITEMGROUP -> MSTITEM -> TrnDtlSINV)
        t = todo.pop()
        for ref in re.findall(r"([A-Za-z_][A-Za-z0-9_]*)\.", links.get(t, "")):
            ref = ref.upper()
            if ref != hdr and ref != t and ref not in need:
                need.add(ref)
                todo.add(ref)
    have, out = {hdr}, []
    pending = set(need)
    while pending:
        ready = [t for t in sorted(pending) if all(r.upper() in have or r.upper() == t
                                                   for r in re.findall(r"([A-Za-z_][A-Za-z0-9_]*)\.", links.get(t, "")))]
        if not ready:
            raise HTTPException(500, f"The report's tables can't be joined: {', '.join(sorted(pending))} (UtlReportsCRTbl).")
        for t in ready:
            if not IDENT.match(t) or not links.get(t):
                raise HTTPException(500, f"The report has no link condition for {t} (UtlReportsCRTbl).")
            out.append(f"LEFT JOIN dbo.[{t}] ON {links[t]}")
            have.add(t)
            pending.discard(t)
    return out


def _plain(v):
    if isinstance(v, Decimal):
        return float(v)
    if isinstance(v, (datetime, date)):
        return v.date().isoformat() if isinstance(v, datetime) else v.isoformat()
    return v.strip() if isinstance(v, str) else v


def _filter_names(kind: str, codes: set[str]) -> list[str]:
    """The names behind the Filter On codes (the screens send codes; the report columns hold names)."""
    master = {"supplier": store.SUPPLIERS, "group": store.ITEM_GROUPS}.get(kind)
    out = []
    for c in codes:
        row = master.get(c) if master else None
        out.append(str((row or {}).get("name") or c))
    return out


AGGREGATES = {"sum": "SUM", "count": "COUNT", "avg": "AVG", "max": "MAX", "min": "MIN"}


def filter_fields(d: dict) -> list[dict]:
    """The fields the report offers as filters (UtlReportsCRDtl.FilterReq = 'Y'): a DATE field takes the From / To period, any other field a list of values."""
    return [{"table": c["table"], "field": c["field"], "caption": c["caption"], "type": c["type"]} for c in d["columns"] if c["filter"]]


def filter_values(d: dict, field: str) -> list[str]:
    """The values to pick from for a filter field: its distinct values in its own table (the exe's `Select distinct field from table order by 1`)."""
    col = next((c for c in d["columns"] if c["filter"] and c["type"] != "DATE" and c["field"].upper() == field.upper()), None)
    if col is None or not (IDENT.match(col["table"]) and IDENT.match(col["field"])):
        raise HTTPException(404, "That is not a filter field of this report")
    rows = db.fetchall(f"SELECT DISTINCT TOP 5000 [{col['field']}] v FROM dbo.[{col['table']}] WHERE [{col['field']}] IS NOT NULL ORDER BY 1")
    return [str(_plain(r["v"])) for r in rows if r["v"] is not None and str(r["v"]).strip() != ""]


def describe(d: dict, from_date: date, to_date: date, flt: dict[str, set[str]] | None) -> str:
    """The exe's print header: 'Report for the period: from .. to ..' (only when the report has a date filter) and 'Report Filtered by X and the values are (..)'."""
    parts = []
    if any(c["filter"] and c["type"] == "DATE" for c in d["columns"]):
        parts.append(f"Report for the period: from {from_date:%d/%m/%Y} to {to_date:%d/%m/%Y}")
    by: dict[str, list[str]] = {}
    for item in sorted((flt or {}).get("ff", set())):
        f, _, v = item.partition("|")
        by.setdefault(f, []).append(v)
    for f, vals in by.items():
        parts.append(f"Report Filtered by {f} and the values are ({', '.join(vals)})")
    return " ".join(parts)


def run(d: dict, from_date: date, to_date: date, flt: dict[str, set[str]] | None = None) -> tuple[list[dict], list[dict]]:
    """(columns, rows) of the report, built like the exe's getSTRSQL: the visible columns (a hidden column is not selected at all), a SUM / COUNT / AVG
    for aggregated ones with a GROUP BY on the rest, the tables' link conditions and the fixed condition, then the filters: each DATE filter field is
    limited to the period, each other filter field to the values picked (`field IN (..)`). A report with no date filter field ignores the period. Extra
    columns (`expression|ALIAS`), an automatic serial number (IsSlnoReq) and subtotal rows (SubTotOn) follow the definition. Output is ordered by every column."""
    cols = d["columns"]
    if not cols or not IDENT.match(d["header"]):
        raise HTTPException(500, f"'{d['name']}' has no columns or header table in the database.")
    for c in cols:
        if not (IDENT.match(c["table"]) and IDENT.match(c["field"])):
            raise HTTPException(500, f"'{d['name']}' has a column with an invalid name.")
    shown = [c for c in cols if not c["hidden"]]
    keys, used = [], set()
    for c in shown:   # one key per column even when two tables have a field of the same name
        k, n = c["field"], 2
        while k.upper() in used:
            k, n = f"{c['field']}_{n}", n + 1
        used.add(k.upper())
        keys.append(k)
    any_agg = any(c["aggregate"] in AGGREGATES for c in cols)
    select, group = [], []
    for c, k in zip(shown, keys):
        ref = f"[{c['table']}].[{c['field']}]"
        agg = AGGREGATES.get(c["aggregate"])
        select.append(f"{agg}({ref}) AS [{k}]" if agg else f"{ref} AS [{k}]")
        if not agg:
            group.append(ref)
    extra_keys = []
    for c in cols:   # AddnlCol = "expression|ALIAS": an extra column the report's author wrote (definition text, like CondText)
        if c["addnl"] and "|" in c["addnl"]:
            expr, _, alias = c["addnl"].rpartition("|")
            alias = alias.strip().upper()
            if IDENT.match(alias):
                select.append(f"{expr.strip()} AS [{alias}]")
                extra_keys.append((alias, c))
    sql = f"SELECT {', '.join(select)} FROM dbo.[{d['header']}] " + " ".join(_joins(d))
    where, params = ["1 = 1"], []
    for c in cols:
        if c["filter"] and c["type"] == "DATE":
            where.append(f"CAST([{c['table']}].[{c['field']}] AS date) BETWEEN ? AND ?")
            params += [from_date.isoformat(), to_date.isoformat()]
    picked: dict[str, list[str]] = {}
    for item in sorted((flt or {}).get("ff", set())):
        f, _, v = item.partition("|")
        picked.setdefault(f.upper(), []).append(v)
    for f, vals in picked.items():
        col = next((c for c in cols if c["filter"] and c["type"] != "DATE" and c["field"].upper() == f), None)
        if col is None:
            raise HTTPException(422, f"'{f}' is not a filter field of this report")
        where.append(f"[{col['table']}].[{col['field']}] IN ({', '.join('?' * len(vals))})")
        params += vals
    for kind, field_name in (("supplier", "SUPPNAME"), ("group", "ITEMGROUPNAME")):   # the web's own Filter On choices
        want = (flt or {}).get(kind)
        col = next((c for c in shown if c["field"].upper() == field_name), None)
        if want and col:
            names_ = _filter_names(kind, want)
            where.append(f"[{col['table']}].[{col['field']}] IN ({', '.join('?' * len(names_))})")
            params += names_
    cond = d["cond"]
    if cond and not re.match(r"(?i)\s*(and|or)\b", cond):
        cond = "AND " + cond
    sql += " WHERE " + " AND ".join(where) + (" " + cond if cond else "")
    if any_agg and group:
        sql += " GROUP BY " + ", ".join(group)
    sql += " ORDER BY " + ", ".join(str(i + 1) for i in range(len(select))) if select else ""
    rows_raw = db.fetchall(sql, tuple(params))
    if len(rows_raw) > MAX_ROWS:
        raise HTTPException(422, f"This report has more than {MAX_ROWS:,} rows. Choose a shorter period.")
    columns = [{"key": k, "label": c["caption"], "align": "right" if c["type"] == "NUMERIC" else "left", "total": c["total"], "hidden": False,
                "type": c["type"], "subtotal": c["subtotal"]} for c, k in zip(shown, keys)]
    for alias, c in extra_keys:
        columns.append({"key": alias, "label": alias, "align": "left", "total": False, "hidden": False, "type": "TEXT", "subtotal": False})
        keys.append(alias)
    rows = [{k: _plain(r.get(k)) for k in keys} for r in rows_raw]
    if d.get("slno"):
        columns.insert(0, {"key": "AutoSlno", "label": "Sl.No", "align": "right", "total": False, "hidden": False, "type": "NUMERIC", "subtotal": False})
        for i, r in enumerate(rows, 1):
            r["AutoSlno"] = i
    sub = next((c for c in columns if c.get("subtotal")), None)   # the exe puts a "<value> SubTotal" row after each run of this column's value
    if sub and rows:
        sums = [c["key"] for c in columns if c["total"]]
        out, run_rows = [], []

        def close():
            if run_rows:
                out.append({**{c["key"]: "" for c in columns}, sub["key"]: f"{run_rows[0][sub['key']]} SubTotal", "_subtotal": True,
                            **{k: round(sum(float(x.get(k) or 0) for x in run_rows), 3) for k in sums}})
        for r in rows:
            if run_rows and r[sub["key"]] != run_rows[0][sub["key"]]:
                close()
                run_rows = []
            out.append(r)
            run_rows.append(r)
        close()
        rows = out
    return columns, rows
