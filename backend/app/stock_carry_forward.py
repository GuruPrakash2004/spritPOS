"""Stock carry forward into a new financial year (legacy frmRptStockBal `btnStk` / frmMstCreateFinYear `StkCarryForward`).

The closing stock of the year before becomes the opening stock of the chosen year, written straight into the shop's own tables in ONE
transaction (`db.direct_writes()`, nothing of the web app's own is kept in the database):

  1. the closing quantity per item and location as at the previous year's last day, from UtlStockList (rows not marked deleted);
     only positive balances are carried, as the desktop does; the rate is the average rate of what came in;
  2. MstOpBalance rows of the chosen year (FinYear = last two digits of its start year) are replaced, one per item and location;
  3. UtlStockList 'OP' rows of that year (DocID = FinYear) are replaced, dated the year's first day;
  4. TrnQOH is rebuilt from the stock rows dated on or after the year's first day, so the live balance is opening + the new year's moves.

Running it again for the same year replaces what the last run wrote, so it can be repeated. Back the database up first.
"""
from datetime import date, timedelta

from fastapi import HTTPException

from . import accounting, admin_audit, db, db_store, gsettings, store

REMARK = "Stock Carry Forward From Previous Fin Year"
_TABLES = ("MstOpBalance", "UtlStockList", "TrnQOH")


def _years(start: str) -> tuple[dict, dict]:
    """(chosen year, the year before it) from the Company Master's financial years."""
    years = sorted(store.FIN_YEARS, key=lambda y: y["start"])
    target = next((y for y in years if y["start"] == start), None)
    if target is None:
        raise HTTPException(404, "Invalid year")
    earlier = [y for y in years if y["start"] < target["start"]]
    if not earlier:
        raise HTTPException(409, "There is no earlier financial year to carry the stock forward from")
    previous = earlier[-1]
    if date.fromisoformat(previous["end"]) + timedelta(days=1) != date.fromisoformat(target["start"]):
        raise HTTPException(409, "The earlier financial year does not end the day before this one starts, so stock dated in between "
                                 "would be lost. Carry forward between consecutive years only.")
    return target, previous


def _closing(previous_end: str) -> list[dict]:
    rows = db.fetchall(
        """SELECT ItemID, LocID, SUM(StkMultiplier * Qty) AS Qty,
                  SUM(CASE WHEN StkMultiplier > 0 THEN Qty * Rate ELSE 0 END) AS InValue,
                  SUM(CASE WHEN StkMultiplier > 0 THEN Qty ELSE 0 END) AS InQty
           FROM UtlStockList WITH (NOLOCK)
           WHERE DocDate < DATEADD(day, 1, CAST(? AS date)) AND ISNULL(DelFlag, 'N') <> 'Y'
           GROUP BY ItemID, LocID HAVING SUM(StkMultiplier * Qty) > 0
           ORDER BY LocID, ItemID""", (previous_end,))
    out = []
    for r in rows:
        in_qty = float(r["InQty"] or 0)
        out.append({"item_id": int(r["ItemID"]), "loc_id": int(r["LocID"] or 1), "qty": float(r["Qty"]),
                    "rate": round(float(r["InValue"] or 0) / in_qty, 4) if in_qty else 0.0})
    return out


def _check(start: str) -> tuple[dict, dict]:
    target, previous = _years(start)
    if not db.is_db_available():
        raise HTTPException(503, "The database is offline")
    accounting._need(*_TABLES)
    if "fldblnfinyrupdate" in accounting._columns("UtlMisc") and (db.fetchone(
            "SELECT TOP 1 FldBlnFinYrUpdate AS f FROM UtlMisc WITH (NOLOCK)") or {}).get("f") == "Y":
        raise HTTPException(409, "FinYear Activity is going on other system...")
    gsettings.check_not_frozen(target["start"], "The stock carry forward")
    return target, previous


def preview(start: str) -> dict:
    """What a carry forward would write, without writing anything."""
    target, previous = _check(start)
    rows = _closing(previous["end"])
    return {"from_year": previous, "to_year": target, "items": len({r["item_id"] for r in rows}), "lines": len(rows),
            "qty": round(sum(r["qty"] for r in rows), 3), "value": round(sum(r["qty"] * r["rate"] for r in rows), 2)}


def carry_forward(start: str, user: str) -> dict:
    target, previous = _check(start)
    fin = int(target["start"][:4]) % 100
    rows = _closing(previous["end"])
    opening = target["start"]
    op_cols, stk_cols = accounting._columns("MstOpBalance"), accounting._columns("UtlStockList")
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute("DELETE FROM MstOpBalance WHERE FinYear = ?", (fin,))
            cur.execute("DELETE FROM UtlStockList WHERE DocType = 'OP' AND DocID = ?", (fin,))
            for i, r in enumerate(rows, start=1):
                accounting._insert(cur, "MstOpBalance", {k: v for k, v in {
                    "FinYear": fin, "ItemID": r["item_id"], "LocID": r["loc_id"], "RcptDate": opening, "RcptRef": REMARK,
                    "OpQty": r["qty"], "OpRate": r["rate"], "Remarks": REMARK}.items() if k.lower() in op_cols})
                accounting._insert(cur, "UtlStockList", {k: v for k, v in {
                    "UTDocID": 0, "DocID": fin, "DocName": "Opening Balance", "DocType": "OP", "DocNo": str(i), "DocDate": opening,
                    "ItemID": r["item_id"], "ItemSlNo": i, "LocID": r["loc_id"], "StkMultiplier": 1, "Qty": r["qty"], "Rate": r["rate"],
                    "particulars": REMARK, "ValuationReq": "N", "StkRate": r["rate"],
                    # as the desktop's button writes them, so its stock reports and FIFO / Re Write Stock see the row
                    "DelFlag": "N", "ReWrite": "N", "FifoRate": r["rate"], "DocValue": round(r["qty"] * r["rate"], 2),
                    "FifoUsedQty": 0}.items() if k.lower() in stk_cols})
            cur.execute("DELETE FROM TrnQOH")
            cur.execute("""INSERT INTO TrnQOH (ItemID, LocID, CLBal)
                           SELECT ItemID, LocID, SUM(StkMultiplier * Qty) FROM UtlStockList
                           WHERE DocDate >= ? AND ISNULL(DelFlag, 'N') <> 'Y'
                           GROUP BY ItemID, LocID HAVING SUM(StkMultiplier * Qty) > 0""", (opening,))
    admin_audit.write_now(user, "Company Master", "Changed", "Stock carry forward", "",
                          f"{previous['start']} to {previous['end']} into {target['start']} to {target['end']}: {len(rows)} line(s)")
    db_store.reload_items()   # the item list and the bills' stock read QOH from memory
    return {"from_year": previous, "to_year": target, "lines": len(rows), "items": len({r["item_id"] for r in rows}),
            "qty": round(sum(r["qty"] for r in rows), 3)}
