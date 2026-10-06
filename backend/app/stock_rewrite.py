"""Tools → Re Write Stock (legacy frmStkRewrite + mSp_RewriteStkList), on the web app's own stock.

The desktop screen compares each document type's quantities with its stock list (Purchase, Sales Invoice,
Stock Deduction, Sales Return) and the stock list with QOH, then "Re-Write" rebuilds the stock list from
the documents and QOH from the stock list, refusing if any item would end up negative.

Here the stock list is store.MOVEMENTS[APP_MOVEMENTS_FROM:] (the app's own movements) and QOH is
ITEMS[code]["qoh"], which must equal BASE_QOH[code] + those movements. Only movements of documents made in
this app (no "origin") are rebuilt. Everything else is kept as it is and left out of the comparisons:
"opening" movements (item creation), movements of seed/DB documents (e.g. cancelling a bill loaded from SQL
Server) and movements whose document isn't loaded (DB bills load in the background, and only the latest
ones), because there is nothing to rebuild them from.

legacy_check() runs the desktop app's own five checks read-only against SQL Server. The web app never
writes the legacy tables, so a difference there is fixed with Re Write Stock in the desktop app.
"""
from collections import defaultdict

from . import db, sqlstore, store

# Tab key -> (caption, document kind, movement kinds on that tab)
TABS = {
    "purchase": ("Purchase", "purchase_bill", ("purchase",)),
    "sales_invoice": ("Sales Invoice", "sales_invoice", ("sale", "sale-cancel")),
    "stock_deduction": ("Stock Deduction", "stock_deduction", ("deduction",)),
    "sales_return": ("Sales Return", "sales_return", ("sales-return",)),
}
MOVE_TAB = {kind: tab for tab, (_, _, kinds) in TABS.items() for kind in kinds}
KIND_ORDER = {"purchase": 0, "sales-return": 1, "sale-cancel": 2, "opening": 3, "sale": 4, "deduction": 5}


def _web_docs(doc_kind: str) -> dict[str, dict]:
    return {no: d for no, d in store.DOCS.get(doc_kind, {}).items() if not d.get("origin")}


def _doc_moves(doc_kind: str, doc: dict) -> list[dict]:
    """The movements a web document makes, exactly as the transaction endpoints make them."""
    on, no = doc["date"], doc["doc_no"]
    out = []

    def add(code: str, qty: float, kind: str, rate: float) -> None:
        out.append({"item": code, "qty": qty, "kind": kind, "doc_no": no, "date": on, "rate": rate})

    for line in doc.get("lines", []):
        code, qty, rate = line["item_code"], line["qty"], line.get("rate", 0)
        if doc_kind == "purchase_bill":
            add(code, (-1 if doc.get("purchase_type") == "Purchase Return" else 1) * qty, "purchase", rate)
        elif doc_kind == "sales_invoice":
            # Held bills have no stock effect; cancelled bills and those removed as duplicates
            # (Utilities → Remove Duplicate Invoice) took the stock out and put it back.
            if doc.get("status") in ("SAVED", "CANCELLED", "DELETED"):
                add(code, -qty, "sale", rate)
            if doc.get("status") in ("CANCELLED", "DELETED"):
                add(code, qty, "sale-cancel", rate)
        elif doc_kind == "sales_return":
            add(code, qty, "sales-return", rate)
        elif doc_kind == "stock_deduction":
            add(code, -qty, "deduction", rate)
    return out


def _rebuildable(move: dict) -> bool:
    """True for a movement Re-Write regenerates: one made by a document of this app that is loaded."""
    tab = MOVE_TAB.get(move["kind"])
    if tab is None:          # "opening" and anything unknown: no document to rebuild it from
        return False
    doc = store.DOCS.get(TABS[tab][1], {}).get(move["doc_no"])
    return doc is not None and not doc.get("origin")


def _item_name(code: str) -> str:
    return (store.ITEMS.get(code) or {}).get("name", code)


def _rows(expected: dict[str, float], actual: dict[str, float]) -> list[dict]:
    rows = []
    for code in sorted(set(expected) | set(actual)):
        trn, sys = round(expected.get(code, 0), 3), round(actual.get(code, 0), 3)
        if trn != sys:
            rows.append({"item_code": code, "item_name": _item_name(code), "trn_stock": trn, "sys_stock": sys,
                         "diff": round(trn - sys, 3)})
    return rows


def check_stock() -> dict:
    """The five comparisons of the desktop screen, on the web data. Only mismatching items are listed."""
    app_moves = store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]
    tabs = []
    for tab, (caption, doc_kind, kinds) in TABS.items():
        expected: dict[str, float] = defaultdict(float)
        for doc in _web_docs(doc_kind).values():
            for m in _doc_moves(doc_kind, doc):
                expected[m["item"]] += m["qty"]
        actual: dict[str, float] = defaultdict(float)
        for m in app_moves:
            if m["kind"] in kinds and _rebuildable(m):
                actual[m["item"]] += m["qty"]
        # Stock-in shown as positive, as on the desktop screen (sales as quantities sold).
        sign = -1 if tab in ("sales_invoice", "stock_deduction") else 1
        tabs.append({"key": tab, "caption": caption,
                     "rows": _rows({k: sign * v for k, v in expected.items()}, {k: sign * v for k, v in actual.items()})})
    ledger: dict[str, float] = defaultdict(float)
    for m in app_moves:
        if m["item"] in store.ITEMS:
            ledger[m["item"]] += m["qty"]
    expected = {code: store.BASE_QOH.get(code, 0) + ledger.get(code, 0) for code in store.ITEMS}
    actual = {code: item["qoh"] for code, item in store.ITEMS.items()}
    ledger_rows = _rows(expected, actual)
    for r in ledger_rows:   # on this tab "Trn" is the ledger (base + movements) and "Sys" the item's QOH
        r["ledger_stock"], r["qoh"] = r.pop("trn_stock"), r.pop("sys_stock")
    tabs.append({"key": "ledger", "caption": "Ledger vs QOH", "rows": ledger_rows})
    return {"tabs": tabs, "rewrite_required": any(t["rows"] for t in tabs)}


class RewriteRefused(Exception):
    """Re-Write would leave items with negative stock; nothing was changed."""

    def __init__(self, items: list[dict]):
        self.items = items
        names = ", ".join(f"{i['item_name']} ({i['qoh']:g})" for i in items[:10])
        super().__init__(f"Re-Write stopped: these items would go negative, nothing was changed: {names}"
                         + (f" and {len(items) - 10} more" if len(items) > 10 else ""))


def rebuild_movements() -> dict:
    """Re-Write: regenerate the app movements from the web documents and set QOH = base + movements.
    Refuses (RewriteRefused, nothing changed) if the app's movements would take an item below zero, like
    mSp_RewriteStkList."""
    before = check_stock()
    if not before["rewrite_required"]:
        return {"rewritten": False, "message": "Re-Write not Required", "check": before}
    app_moves = store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]
    moves = [m for m in app_moves if not _rebuildable(m)]
    for tab, (_, doc_kind, _) in TABS.items():
        for doc in _web_docs(doc_kind).values():
            moves.extend(_doc_moves(doc_kind, doc))
    # Legacy order: document date, item, stock-in before stock-out, document type, document number.
    moves.sort(key=lambda m: (m["date"], m["item"], 0 if m["qty"] > 0 else 1, KIND_ORDER.get(m["kind"], 9), m["doc_no"]))
    qoh: dict[str, float] = {code: store.BASE_QOH.get(code, 0) for code in store.ITEMS}
    for m in moves:
        if m["item"] in qoh:
            qoh[m["item"]] += m["qty"]
    # Like the procedure, never write a result with negative stock, except where the item was already below
    # zero before the app's movements (seed/DB data): that is not something a rewrite can cause or fix.
    negative = [{"item_code": c, "item_name": _item_name(c), "qoh": round(q, 3)} for c, q in sorted(qoh.items())
                if q < -1e-9 and q < store.BASE_QOH.get(c, 0) - 1e-9]
    if negative:
        raise RewriteRefused(negative)
    changed = sum(1 for c, q in qoh.items() if round(store.ITEMS[c]["qoh"], 3) != round(q, 3))
    store.MOVEMENTS[store.APP_MOVEMENTS_FROM:] = moves
    for code, q in qoh.items():
        store.ITEMS[code]["qoh"] = round(q, 3)
    sqlstore.moves_rewritten()   # the save writes the web's whole stock ledger again (Web_AppMove, UtlStockList)
    return {"rewritten": True, "message": "Successfully re-Written Stock!", "items_changed": changed,
            "movements": len(moves), "check": check_stock()}


# The desktop screen's checks, as in vLQMS.exe (frmStkRewrite), run read-only on the legacy tables.
_LEGACY_CHECKS = [
    ("purchase", "Purchase", """
        SELECT i.ItemName AS item_name, i.ItemCode AS item_code, SUM(a.PQty) AS trn_stock, SUM(a.UslQty) AS sys_stock,
               SUM(a.PQty) - SUM(a.UslQty) AS diff
        FROM (SELECT ItemID, SUM(PBQty) AS PQty, 0 AS UslQty FROM TrnDtlPB GROUP BY ItemID
              UNION ALL
              SELECT ItemID, 0, SUM(Qty) FROM UtlStockList WHERE UPPER(DocType) IN ('PB','SA','ST') GROUP BY ItemID) a
        JOIN MstItem i ON i.ItemID = a.ItemID
        GROUP BY i.ItemID, i.ItemName, i.ItemCode HAVING SUM(a.PQty) <> SUM(a.UslQty)"""),
    ("sales_invoice", "Sales Invoice", """
        SELECT i.ItemName AS item_name, i.ItemCode AS item_code, SUM(a.PQty) AS trn_stock, SUM(a.UslQty) AS sys_stock,
               SUM(a.PQty) - SUM(a.UslQty) AS diff
        FROM (SELECT d.ItemID, SUM(d.SInvQty) AS PQty, 0 AS UslQty FROM TrnDtlSINV d
                JOIN TrnHdrSINV h ON h.SInvID = d.SInvID WHERE h.Cancelled = 'N' AND h.IsDelete = 'N' GROUP BY d.ItemID
              UNION ALL
              SELECT ItemID, 0, SUM(Qty) FROM UtlStockList WHERE UPPER(DocType) = 'INV' GROUP BY ItemID) a
        JOIN MstItem i ON i.ItemID = a.ItemID
        GROUP BY i.ItemID, i.ItemName, i.ItemCode HAVING SUM(a.PQty) <> SUM(a.UslQty)"""),
    ("stock_deduction", "Stock Deduction", """
        SELECT i.ItemName AS item_name, i.ItemCode AS item_code, SUM(a.PQty) AS trn_stock, SUM(a.UslQty) AS sys_stock,
               SUM(a.PQty) - SUM(a.UslQty) AS diff
        FROM (SELECT ItemID, SUM(StkDedQty) AS PQty, 0 AS UslQty FROM TrnDtlSTKDED GROUP BY ItemID
              UNION ALL
              SELECT ItemID, 0, SUM(Qty) FROM UtlStockList WHERE UPPER(DocType) = 'STKDED' GROUP BY ItemID) a
        JOIN MstItem i ON i.ItemID = a.ItemID
        GROUP BY i.ItemID, i.ItemName, i.ItemCode HAVING SUM(a.PQty) <> SUM(a.UslQty)"""),
    ("sales_return", "Sales Return", """
        SELECT i.ItemName AS item_name, i.ItemCode AS item_code, SUM(a.PQty) AS trn_stock, SUM(a.UslQty) AS sys_stock,
               SUM(a.PQty) - SUM(a.UslQty) AS diff
        FROM (SELECT ItemID, SUM(SRQty) AS PQty, 0 AS UslQty FROM TrnDtlSR GROUP BY ItemID
              UNION ALL
              SELECT ItemID, 0, SUM(Qty) FROM UtlStockList WHERE UPPER(DocType) = 'SR' GROUP BY ItemID) a
        JOIN MstItem i ON i.ItemID = a.ItemID
        GROUP BY i.ItemID, i.ItemName, i.ItemCode HAVING SUM(a.PQty) <> SUM(a.UslQty)"""),
    ("ledger", "Ledger vs QOH", """
        SELECT i.ItemName AS item_name, i.ItemCode AS item_code, SUM(a.PQty) AS ledger_stock, SUM(a.UslQty) AS qoh,
               SUM(a.PQty) - SUM(a.UslQty) AS diff
        FROM (SELECT ItemID, SUM(StkMultiplier * Qty) AS PQty, 0 AS UslQty FROM UtlStockList GROUP BY ItemID
              UNION ALL
              SELECT ItemID, 0, CLBal FROM TrnQOH) a
        JOIN MstItem i ON i.ItemID = a.ItemID
        GROUP BY i.ItemID, i.ItemName, i.ItemCode HAVING SUM(a.PQty) <> SUM(a.UslQty)"""),
]


def legacy_check() -> dict | None:
    """The desktop checks on SQL Server (read-only), or None without a connection. A tab whose table is
    missing in this database (Sales Return is optional in the desktop app) reports an empty list."""
    if not db.is_db_available():
        return None
    tabs = []
    for key, caption, sql in _LEGACY_CHECKS:
        # Read-only runner: a lock timeout instead of waiting behind a busy counter, and always rolled back.
        try:
            res = db.read_only(sql, max_rows=2000)
            rows = [dict(zip(res["columns"], r)) for r in res["rows"]]
            error = None
        except Exception as exc:   # e.g. TrnDtlSR not created in this database, or the connection dropped
            rows, error = [], db.safe_error(exc)[:300]
        tabs.append({"key": key, "caption": caption, "error": error,
                     "rows": [{k: (float(v) if isinstance(v, (int, float)) or hasattr(v, "as_tuple") else v)
                               for k, v in r.items()} for r in rows]})
    return {"tabs": tabs, "rewrite_required": any(t["rows"] for t in tabs)}
