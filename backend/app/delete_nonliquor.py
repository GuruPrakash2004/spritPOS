"""Delete Non-Liquor Items (legacy frmUtlDeleteNonLiqItems, tab "Single Group - Delete All - Items/Trn/Stock").

Deletes every item of one non-liquor item group, the group itself, the items' lines on sales invoices and purchase
bills (a bill left with no lines is deleted, the others get their totals worked out again), their stock rows and QOH.
This rewrites history in the shop's SQL Server, so it is guarded:
  - Administrator only, and the additional ADMIN password (see routers/utilities.py);
  - the desktop's own rule: an item whose liquor type is not blank / OTHERS is liquor, and then nothing is deleted;
  - refused when a document or stock movement made in this web app uses one of the items (those live in the Web_ tables,
    which this does not rewrite; use Merge Items instead);
  - refused when a bill that would change is on or before the Freeze Date;
  - one transaction: any failure rolls everything back; afterwards the web app reads the database again.
Unlike the desktop, which deletes every orphaned row "not in MstItem", only rows of THIS group's items are touched, and a
bill paid partly by card, credit or cheque keeps its payment split (the desktop sets CashAmt to the new total).
Back the database up first.
"""
from datetime import date

from fastapi import HTTPException

from . import admin_audit, db, gsettings, sqlstore, store

NOT_LIQUOR = {"", "OTHERS"}


def _items_of(group: str) -> list[dict]:
    return [i for i in store.ITEMS.values() if i.get("group") == group]


def _marks(n: int) -> str:
    return ",".join("?" * n)


def _ids(group: str) -> list[int]:
    rows = db.fetchall("SELECT i.ItemID FROM MstItem i JOIN MstItemGroup g ON g.ItemGroupID = i.ItemGroupID WHERE g.ItemGroupCode = ?", (group,))
    return [int(r["ItemID"]) for r in rows]


def _web_activity(codes: set[str]) -> list[str]:
    """Documents and stock movements made in this web app that use these items."""
    found = []
    for kind, docs in store.DOCS.items():
        for no, d in docs.items():
            if "origin" in d:
                continue
            if any(l.get("item_code") in codes for l in d.get("lines", [])):
                found.append(f"{kind.replace('_', ' ')} {no}")
    if any(m.get("item") in codes for m in store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]):
        found.append("stock movements made in the web app")
    return found


def _counts(ids: list[int]) -> dict:
    if not ids:
        return {"sales_bills": 0, "sales_lines": 0, "purchase_bills": 0, "purchase_lines": 0, "stock_rows": 0, "first_sale": None}
    m = _marks(len(ids))
    one = lambda sql: db.fetchone(sql, tuple(ids)) or {}   # noqa: E731
    s = one(f"SELECT COUNT(DISTINCT d.SINVID) AS b, COUNT(*) AS l, MIN(h.SINVDATE) AS first FROM TrnDtlSINV d JOIN TrnHdrSINV h ON h.SINVID = d.SINVID WHERE d.ItemID IN ({m})")
    p = one(f"SELECT COUNT(DISTINCT d.PBID) AS b, COUNT(*) AS l, MIN(h.PBDATE) AS first FROM TrnDtlPB d JOIN TrnHdrPB h ON h.PBID = d.PBID WHERE d.ItemID IN ({m})")
    k = one(f"SELECT COUNT(*) AS n FROM UtlStockList WHERE ItemID IN ({m})")
    firsts = [d for d in (s.get("first"), p.get("first")) if d]
    return {"sales_bills": int(s.get("b") or 0), "sales_lines": int(s.get("l") or 0), "purchase_bills": int(p.get("b") or 0),
            "purchase_lines": int(p.get("l") or 0), "stock_rows": int(k.get("n") or 0),
            "first_sale": min(firsts).isoformat()[:10] if firsts else None}


def preview(group: str) -> dict:
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    if group not in store.ITEM_GROUPS:
        raise HTTPException(404, f"Item group '{group}' not found")
    items = _items_of(group)
    rows = [{"item_code": i["code"], "item_name": i["name"], "liqtype": i.get("liqtype") or "",
             "problem": "" if (i.get("liqtype") or "").upper() in NOT_LIQUOR else "Type is of LIQUOUR, cannot be deleted!"} for i in sorted(items, key=lambda i: i["name"])]
    blockers = []
    if not items:
        blockers.append("The group has no items")
    if any(r["problem"] for r in rows):
        blockers.append("The group has liquor items")
    web = _web_activity({i["code"] for i in items})
    if web:
        blockers.append("Used by documents made in the web app (" + ", ".join(web[:3]) + ("…" if len(web) > 3 else "") + "): use Merge Items instead")
    ids = _ids(group)
    counts = _counts(ids)
    return {"group": group, "items": rows, "counts": counts, "blockers": blockers, "can_delete": not blockers}


def delete(group: str, confirm: str, user: dict) -> dict:
    if confirm.strip() != group:
        raise HTTPException(422, f"Type the item group code ({group}) to confirm")
    info = preview(group)
    if info["blockers"]:
        raise HTTPException(409, "; ".join(info["blockers"]))
    first = info["counts"]["first_sale"]
    if first:
        gsettings.check_not_frozen(date.fromisoformat(first), "A bill with these items")
    ids = _ids(group)
    if not ids:
        raise HTTPException(409, "The group's items were not found in the database")
    m, args = _marks(len(ids)), tuple(ids)
    with db.transaction() as cur:
        cur.execute(f"SELECT DISTINCT SINVID INTO #aff_sinv FROM TrnDtlSINV WHERE ItemID IN ({m})", args)
        cur.execute(f"SELECT DISTINCT PBID INTO #aff_pb FROM TrnDtlPB WHERE ItemID IN ({m})", args)
        cur.execute(f"DELETE FROM TrnDtlSINV WHERE ItemID IN ({m})", args)
        cur.execute("DELETE FROM TrnHdrSINV WHERE SINVID IN (SELECT SINVID FROM #aff_sinv) AND SINVID NOT IN (SELECT SINVID FROM TrnDtlSINV)")
        cur.execute(f"DELETE FROM TrnDtlPB WHERE ItemID IN ({m})", args)
        cur.execute("DELETE FROM TrnHdrPB WHERE PBID IN (SELECT PBID FROM #aff_pb) AND PBID NOT IN (SELECT PBID FROM TrnDtlPB)")
        cur.execute(f"UPDATE UtlStockList SET Qty = 0 WHERE ItemID IN ({m})", args)
        cur.execute(f"DELETE FROM UtlStockList WHERE ItemID IN ({m})", args)
        cur.execute(f"DELETE FROM TrnQOH WHERE ItemID IN ({m})", args)
        # The bills that lost lines: totals again from the lines left. Payment split kept when the bill was not all cash.
        cur.execute(
            "UPDATE h SET TotQty = t.q, TotAmt = t.a, TotBasicAmt = t.a, Tot = t.a, InvValue = t.a, "
            "CashAmt = CASE WHEN ISNULL(h.CardAmt,0) + ISNULL(h.CreditAmt,0) + ISNULL(h.ChqAmt,0) = 0 THEN t.a ELSE h.CashAmt END "
            "FROM TrnHdrSINV h JOIN (SELECT SINVID, SUM(SINVQty) AS q, SUM(SINVAmt) AS a FROM TrnDtlSINV GROUP BY SINVID) t ON t.SINVID = h.SINVID "
            "WHERE h.SINVID IN (SELECT SINVID FROM #aff_sinv)")
        cur.execute(f"DELETE FROM MstItem WHERE ItemID IN ({m})", args)
        cur.execute("DELETE FROM MstItemGroup WHERE ItemGroupCode = ?", (group,))
    sqlstore.reload()   # everything in memory was read from the rows just deleted
    admin_audit.write_now(user["user"], "Delete Non-Liquor Items", "Deleted", group, "",
                          f"{len(ids)} item(s), {info['counts']['sales_bills']} sales bill(s), {info['counts']['purchase_bills']} purchase bill(s)",
                          [{"code": r["item_code"], "name": r["item_name"], "status": "Deleted", "details": ""} for r in info["items"]])
    return {"deleted_items": len(ids), **info["counts"]}
