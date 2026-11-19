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


# ─── Carry Over (the form's second tab: "Carry Over" of the chosen item groups) ───────────────────────────────

def _group_ids(codes: list[str]) -> list[int]:
    rows = db.fetchall(f"SELECT ItemGroupID FROM MstItemGroup WHERE ItemGroupID > 0 AND ItemGroupCode IN ({_marks(len(codes))})", tuple(codes)) if codes else []
    return [int(r["ItemGroupID"]) for r in rows]


def _item_ids_of_groups(group_ids: list[int]) -> list[int]:
    if not group_ids:
        return []
    return [int(r["ItemID"]) for r in db.fetchall(f"SELECT ItemID FROM MstItem WHERE ItemGroupID IN ({_marks(len(group_ids))})", tuple(group_ids))]


def _closing_balances(item_ids: list[int]) -> list[tuple[int, float]]:
    """The desktop's TmpGrpQOH: each item's stock now (sum of Qty x StkMultiplier of UtlStockList), only where it is above zero."""
    if not item_ids:
        return []
    rows = db.fetchall(f"SELECT ItemID, SUM(Qty * StkMultiplier) AS bal FROM UtlStockList WHERE ItemID IN ({_marks(len(item_ids))}) "
                       "GROUP BY ItemID HAVING SUM(Qty * StkMultiplier) > 0 ORDER BY ItemID", tuple(item_ids))
    return [(int(r["ItemID"]), float(r["bal"])) for r in rows]


def carry_groups() -> list[dict]:
    """Active item groups the Carry Over tab can pick from (the desktop: ItemGroupID > 0, InActive = 'N', by GroupOrderBy)."""
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    rows = db.fetchall("SELECT ItemGroupCode, ItemGroupName FROM MstItemGroup WHERE ItemGroupID > 0 AND ISNULL(InActive, 'N') = 'N' ORDER BY GroupOrderBy, ItemGroupCode")
    return [{"code": str(r["ItemGroupCode"]).strip(), "name": str(r["ItemGroupName"] or "").strip()} for r in rows]


def carry_preview(codes: list[str]) -> dict:
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    codes = list({c.strip().upper(): c.strip() for c in codes if c and c.strip()}.values())   # a code once, whatever its letter case
    if not codes:
        raise HTTPException(422, "No groups selected!")
    group_ids = _group_ids(codes)
    if len(group_ids) != len(codes):
        raise HTTPException(404, "An item group was not found")
    ids = _item_ids_of_groups(group_ids)
    blockers = []
    if not ids:
        blockers.append("The groups have no items")
    if db.use_web_tables():   # only the older mode keeps web documents outside the shop's tables
        web = _web_activity({i["code"] for i in store.ITEMS.values() if i.get("group") in codes})
        if web:
            blockers.append("Used by documents made in the web app (" + ", ".join(web[:3]) + ("…" if len(web) > 3 else "") + ")")
    counts = _counts(ids)
    carried = _closing_balances(ids)
    return {"groups": codes, "items": len(ids), "counts": counts, "carried_items": len(carried), "carried_qty": round(sum(b for _i, b in carried), 3),
            "blockers": blockers, "can_carry": not blockers}


def carry_over(codes: list[str], confirm: str, user: dict) -> dict:
    """Delete the transactions of the chosen item groups and carry each item's closing stock forward as one purchase bill (the desktop's btnCarryOver_Click
    and updatePB): sales and purchase lines of those items go (a bill left with no lines goes too, the others get their totals again), their stock rows
    and QOH go, then a new bill dated today (supplier 0, party '-', Authorised by ADMIN, remarks 'Stock') brings each item's old closing balance back as
    its opening stock at the item's purchase rate. The items and groups stay. Back the database up first."""
    wanted = ", ".join(sorted(c.strip() for c in codes if c.strip()))
    if confirm.strip().upper() != "CARRY OVER":
        raise HTTPException(422, "Type CARRY OVER to confirm")
    info = carry_preview(codes)
    if info["blockers"]:
        raise HTTPException(409, "; ".join(info["blockers"]))
    first = info["counts"]["first_sale"]
    if first:
        gsettings.check_not_frozen(date.fromisoformat(first), "A bill with these items")
    group_ids = _group_ids(info["groups"])
    ids = _item_ids_of_groups(group_ids)
    balances = _closing_balances(ids)   # read before anything is deleted
    today = date.today()
    pbno = store.next_doc_no("purchase_bill", "")
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
        if balances:
            cur.execute(
                "INSERT INTO TrnHdrPB (PBNO, Prefix, PrintPBNO, PBDate, SUPPID, PartyName, PartyAdd1, PartyAdd2, PartyAdd3, PartyAdd4, PartyCity, PartyState, "
                "PartyPinCode, PartyCountry, PartyCName, PartyCMob, PartyCEmail, PartyCDesig, Authorised, AuthBy, AuthOn, LOCID, ChangedBy, ChangedOn, "
                "Cancelled, AddedBy, AddedOn, AMTINWRDS, RMKS, SUPPINVNO, SUPPINVDATE, SUPPINVVALUE, TOTAMT, OTHAMT, GRNDTOT, TOTCGST, TOTSGST, TOTIGST, PURCHASETYPE) "
                "VALUES (?, '', ?, ?, 0, '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', 'Y', 'ADMIN', ?, 1, '', NULL, 'N', 'ADMIN', ?, "
                "'', 'Stock', '', NULL, 0, 0, 0, 0, 0, 0, 0, '')", (pbno, pbno, today, today, today))
            cur.execute("SELECT MAX(PBID) AS id FROM TrnHdrPB")
            pbid = int(cur.fetchone()[0])
            for n, (item_id, bal) in enumerate(balances, 1):
                cur.execute(
                    "INSERT INTO TrnDtlPB (PBID, SlNo, ITEMID, UOMID, ItemSlNO, StkQty, PBQty, PBRate, PBAmt, NOOFCASES, NOOFBOTTLES, CGSTPER, CGSTAMT, SGSTPER, SGSTAMT, "
                    "IGSTPER, IGSTAMT, NETAMT, MRP, PBQtyInLtrs) "
                    "SELECT ?, ?, i.ItemID, i.UOMID, ?, ?, ?, i.PurRate, i.PurRate * ?, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 FROM MstItem i WHERE i.ItemID = ?",
                    (pbid, n, n, bal, bal, bal, item_id))
            cur.execute(
                "INSERT INTO UtlStockList (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LocID) "
                "SELECT d.ItemID, 1, 'PB', h.PBDate, h.PBID, h.PBNO, h.PartyName, d.PBQty, d.PBQtyInLtrs, d.PBRate, h.LOCID "
                "FROM TrnDtlPB d JOIN TrnHdrPB h ON h.PBID = d.PBID WHERE d.PBID = ?", (pbid,))
        cur.execute(
            "UPDATE h SET TotQty = t.q, TotAmt = t.a, TotBasicAmt = t.a, Tot = t.a, InvValue = t.a, "
            "CashAmt = CASE WHEN ISNULL(h.CardAmt,0) + ISNULL(h.CreditAmt,0) + ISNULL(h.ChqAmt,0) = 0 THEN t.a ELSE h.CashAmt END "
            "FROM TrnHdrSINV h JOIN (SELECT SINVID, SUM(SINVQty) AS q, SUM(SINVAmt) AS a FROM TrnDtlSINV GROUP BY SINVID) t ON t.SINVID = h.SINVID "
            "WHERE h.SINVID IN (SELECT SINVID FROM #aff_sinv)")
    sqlstore.reload()   # everything in memory was read from the rows just deleted
    admin_audit.write_now(user["user"], "Delete Non-Liquor Items", "Carry Over", wanted, "",
                          f"{len(ids)} item(s), {info['counts']['sales_bills']} sales bill(s), {info['counts']['purchase_bills']} purchase bill(s) deleted; "
                          f"{len(balances)} item(s) carried over" + (f" on purchase bill {pbno}" if balances else ""))
    return {"groups": info["groups"], "items": len(ids), **info["counts"], "carried_items": len(balances), "purchase_bill": pbno if balances else None}
