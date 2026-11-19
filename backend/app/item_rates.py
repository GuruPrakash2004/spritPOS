"""Keeps every item's Item Selling Rate (MstITEM.ITEMSELL) equal to the Doc Designer's formula for the Item Master
(SVTROUND2((#MRP#-#COMPCESS#)/(100+#CGSTPER#+#SGSTPER#+#CESSPER#)*100): the desktop's AddnlMSTFldCalTotal).

The rate is worked out when an item is saved from the web (routers/masters.py). A change made in the database itself (MRP, GST or Cess edited
in SQL Server or another tool) leaves the stored rate behind, so every time the masters are read this runs once over all the items: the
rate in memory is the formula's, and a stored rate that differs is written back (only ITEMSELL, only for the rows that differ, never
when the app is read-only). The shop's database agreed with the formula for 5034 of 5035 items when this was written, so a normal run
writes nothing. (Owner's decision, 2026-11-19: the desktop's way, keeping the old rate until Options > Update Item Rates, has a bug, so
the web app keeps the rate current by itself.)
"""
import logging

from . import db, store

logger = logging.getLogger("vlqms.item_rates")


def _num(v) -> float:
    try:
        return float(v or 0)
    except (TypeError, ValueError):
        return 0.0


def sync() -> int:
    """Recalculate ITEMSELL of every item in memory; write the changed ones back. Returns how many were changed."""
    from .routers import masters
    if not store.ITEMS or not db.is_db_available():
        return 0
    context = masters._item_formula_context()
    if not context:
        return 0
    stale: list[tuple[str, int, float]] = []
    for code, row in list(store.ITEMS.items()):
        before = next((v for k, v in (row.get("extras") or {}).items() if k.lower() == "itemsell"), None)
        masters._apply_item_formulas_with(row, *context)
        after = next((v for k, v in (row.get("extras") or {}).items() if k.lower() == "itemsell"), None)
        if after is not None and round(_num(after), 2) != round(_num(before), 2) and str(row.get("_itemid") or "").isdigit():
            stale.append((code, int(row["_itemid"]), round(_num(after), 2)))
    if not stale:
        return 0
    logger.info("Item Selling Rate was out of date for %d item(s) (MRP / GST / Cess changed in the database): %s",
                len(stale), ", ".join(c for c, _, _ in stale[:10]))
    if db.no_writes() or db.READ_ONLY:
        return len(stale)   # shown right in memory; the database is left alone
    try:
        with db.direct_writes():
            with db.transaction() as cur:
                for _code, item_id, rate in stale:
                    cur.execute("UPDATE MstITEM SET ITEMSELL = ? WHERE ITEMID = ?", (rate, item_id))
        from . import master_watch
        master_watch.rebaseline()   # our own correction, not an outside change
    except Exception:   # noqa: BLE001 - memory is right; the next read tries the write again
        logger.warning("Could not write the corrected Item Selling Rate back", exc_info=True)
    return len(stale)
