"""ai.IndentSuggestion: the draft purchase-indent suggestions (migrations/ai_002.sql).

A suggestion is only a draft: draft -> approved (the router then makes the indent with the existing indent code)
or rejected. At most one open draft per item (a unique index); generating again replaces it.
"""
from .. import db, store

COLUMNS = ("SuggestionID, ItemID, ItemCode, ItemName, Cases, Bottles, QOH, DaysRemaining, ReorderPoint, TargetStock, "
           "RecStatus, ItemClass, Method, Reason, Status, CreatedOn, CreatedBy, DecidedOn, DecidedBy, IndentDocNo")
_KEYS = {"SuggestionID": "id", "ItemID": "item_id", "ItemCode": "item_code", "ItemName": "item_name", "Cases": "cases",
         "Bottles": "bottles", "QOH": "qoh", "DaysRemaining": "days_remaining", "ReorderPoint": "reorder_point",
         "TargetStock": "target_stock", "RecStatus": "rec_status", "ItemClass": "item_class", "Method": "method",
         "Reason": "reason", "Status": "status", "CreatedOn": "created_on", "CreatedBy": "created_by",
         "DecidedOn": "decided_on", "DecidedBy": "decided_by", "IndentDocNo": "indent_doc_no"}


def _row(r: dict) -> dict:
    out = {}
    for col, key in _KEYS.items():
        v = r.get(col)
        out[key] = float(v) if hasattr(v, "as_tuple") else (v.isoformat(sep=" ", timespec="seconds") if hasattr(v, "isoformat") else v)
    return out


def create_drafts(recs: list[dict], user: str) -> list[int]:
    """Save a draft for each recommendation that asks for a quantity, replacing the item's earlier draft. Returns the ids."""
    ids = []
    with db.transaction() as cur:
        for r in recs:
            it = store.ITEMS.get(r["item_code"]) or {}
            item_id = int(it.get("_itemid") or 0)
            if not item_id or r["suggested_cases"] <= 0:
                continue
            cur.execute("DELETE FROM ai.IndentSuggestion WHERE ItemID = ? AND Status = 'draft'", (item_id,))
            cur.execute("INSERT INTO ai.IndentSuggestion (ItemID, ItemCode, ItemName, Cases, Bottles, QOH, DaysRemaining, "
                        "ReorderPoint, TargetStock, RecStatus, ItemClass, Method, Reason, CreatedBy) "
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                        (item_id, r["item_code"], r["item_name"][:200], r["suggested_cases"], r["suggested_bottles"], r["qoh"],
                         r["days_remaining"], r["reorder_point"], r["target_stock"], r["status"], r["item_class"],
                         r["method"][:60], r["reason"][:500], user[:50]))
            cur.execute("SELECT SCOPE_IDENTITY()")
            ids.append(int(cur.fetchone()[0]))
    return ids


def list_(status: str | None = None, limit: int = 500) -> list[dict]:
    sql = f"SELECT TOP {int(limit)} {COLUMNS} FROM ai.IndentSuggestion WITH (NOLOCK)"
    params: tuple = ()
    if status:
        sql, params = sql + " WHERE Status = ?", (status,)
    return [_row(r) for r in db.fetchall(sql + " ORDER BY SuggestionID DESC", params)]


def get(suggestion_id: int) -> dict | None:
    rows = db.fetchall(f"SELECT {COLUMNS} FROM ai.IndentSuggestion WHERE SuggestionID = ?", (suggestion_id,))
    return _row(rows[0]) if rows else None


def _decide(suggestion_id: int, status: str, user: str, doc_no: str | None) -> bool:
    """draft -> approved/rejected. False if it was no longer a draft."""
    with db.transaction() as cur:
        n = cur.execute("UPDATE ai.IndentSuggestion SET Status = ?, DecidedOn = GETDATE(), DecidedBy = ?, IndentDocNo = ? "
                        "WHERE SuggestionID = ? AND Status = 'draft'", (status, user[:50], doc_no, suggestion_id))
    return bool(n)


def mark_approved(suggestion_id: int, user: str, doc_no: str) -> bool:
    return _decide(suggestion_id, "approved", user, doc_no)


def mark_rejected(suggestion_id: int, user: str) -> bool:
    return _decide(suggestion_id, "rejected", user, None)


def reopen(suggestion_id: int) -> None:
    """An approved suggestion whose indent never reached the database goes back to draft."""
    with db.transaction() as cur:
        cur.execute("UPDATE ai.IndentSuggestion SET Status = 'draft', DecidedOn = NULL, DecidedBy = NULL, IndentDocNo = NULL "
                    "WHERE SuggestionID = ? AND Status = 'approved'", (suggestion_id,))
