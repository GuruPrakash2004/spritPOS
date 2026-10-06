"""Utilities → Merge Items (713, frmUtlMergeItems) and Replace and Remove Duplicate items (712,
frmTrnRemoveDuplicates): one item is folded into another and then removed.

The desktop app re-points every transaction line, the stock list and QOH from the item being removed to the
item kept (adding the two QOHs), then deletes the old item and its dependent master rows. The web app does
the same in memory, and sqlstore.merged() does it in SQL Server's Web_ tables on the request's save (every
Web_ table with an ITEMID, TrnQOH added together, the web's ledger Web_AppMove):
- every document line (web and loaded desktop bills) and every stock movement moves to the kept item;
- base QOH (store.BASE_QOH) and QOH are added together, so Tools → Re Write Stock still balances;
- the last purchase date keeps the later one;
- the old item is deleted (store.touch), and store.MERGED_ITEMS remembers old -> kept so bills loaded later
  from SQL Server, and a restart, land on the kept item;
- the ADMIN Audit Log gets the merge with each document line count.
Merge Items needs both items active (as the desktop screen's list only offers active items); Replace and
Remove takes any item, picked by name.
"""
from collections import Counter
from datetime import date

from fastapi import HTTPException

from . import admin_audit, gsettings, sqlstore, store

SCREENS = {"merge": "Merge Items", "replace": "Replace and Remove Duplicate items"}


def _check(src: str, dst: str, mode: str) -> tuple[dict, dict]:
    a, b = store.ITEMS.get(src), store.ITEMS.get(dst)
    if a is None:
        raise HTTPException(422, "Merge Item is invalid" if mode == "merge" else "Please Select Item (to remove)")
    if b is None:
        raise HTTPException(422, "To Merge Item is invalid" if mode == "merge" else "Please Select Item (to replace with)")
    if src == dst:
        raise HTTPException(422, "Pick two different items")
    if mode == "merge" and not (a.get("active", True) and b.get("active", True)):
        raise HTTPException(422, "Both items must be active to merge them (mark them active first)")
    return a, b


def _lines(src: str) -> Counter:
    out: Counter = Counter()
    for kind, docs in store.DOCS.items():
        for d in docs.values():
            out[kind] += sum(1 for ln in d.get("lines", []) if ln.get("item_code") == src)
    return +out


def _check_frozen(src: str) -> None:
    """Merging rewrites old documents: refuse when any document with the item is on or before the Freeze Date."""
    frozen = gsettings.freeze_date()
    if frozen is None:
        return
    for docs in store.DOCS.values():
        for d in docs.values():
            if str(d.get("date", "9999"))[:10] <= frozen.isoformat() and any(
                    ln.get("item_code") == src for ln in d.get("lines", [])):
                gsettings.check_not_frozen(d["date"], f"{d.get('doc_no', 'A document')} (it has {src})")


def _merge_closing_lines(d: dict, dst: str) -> None:
    """A closing stock entry lists each item once: add the two items' lines together."""
    lines = [ln for ln in d.get("lines", []) if ln.get("item_code") == dst]
    if len(lines) < 2:
        return
    keep = lines[0]
    for ln in lines[1:]:
        for k in ("op_qty", "closing_qty", "sales_qty", "amount"):
            if k in ln:
                keep[k] = round(keep.get(k, 0) + ln[k], 3)
        d["lines"].remove(ln)


def preview(src: str, dst: str, mode: str) -> dict:
    a, b = _check(src, dst, mode)
    return {
        "from": {"code": src, "name": a.get("name", ""), "qoh": a.get("qoh", 0), "active": a.get("active", True)},
        "to": {"code": dst, "name": b.get("name", ""), "qoh": b.get("qoh", 0), "active": b.get("active", True)},
        "lines": dict(_lines(src)),
        "movements": sum(1 for m in store.MOVEMENTS if m["item"] == src),
        "qoh_after": round(a.get("qoh", 0) + b.get("qoh", 0), 3),
    }


def merge(src: str, dst: str, mode: str) -> dict:
    """Fold `src` into `dst` and delete `src`. Runs inside one write request (the middleware saves it)."""
    info = preview(src, dst, mode)
    _check_frozen(src)

    for kind, docs in store.DOCS.items():
        for d in docs.values():
            hit = False
            for ln in d.get("lines", []):
                if ln.get("item_code") == src:
                    hit = True
                    ln["item_code"] = dst
                    if "item_name" in ln:
                        ln["item_name"] = store.ITEMS[dst].get("name", dst)
            if hit and kind == "closing_stock":
                _merge_closing_lines(d, dst)
    for i, m in enumerate(store.MOVEMENTS):
        if m["item"] == src:
            store.MOVEMENTS[i] = {**m, "item": dst, "merged_from": m.get("merged_from", src)}
    store.BASE_QOH[dst] = store.BASE_QOH.get(dst, 0) + store.BASE_QOH.pop(src, 0)
    old = store.ITEMS.pop(src)
    store.ITEMS[dst]["qoh"] = round(store.ITEMS[dst]["qoh"] + old.get("qoh", 0), 3)
    last = [d for d in (store.LAST_PURCHASE.pop(src, None), store.LAST_PURCHASE.get(dst)) if d]
    if last:
        store.LAST_PURCHASE[dst] = max(last)
    for k, v in list(store.MERGED_ITEMS.items()):
        if v == src:
            store.MERGED_ITEMS[k] = dst
    store.MERGED_ITEMS[src] = dst
    if old.get("_itemid"):
        store.MERGED_ITEM_IDS[str(old["_itemid"])] = dst
    store.touch("ITEMS", src)
    store.touch("ITEMS", dst)

    sqlstore.merged(src, dst)
    counts = ", ".join(f"{k.replace('_', ' ')}: {n}" for k, n in info["lines"].items()) or "no document lines"
    admin_audit.record(SCREENS[mode], "Merged", f"{src} → {dst}",
                       f"{src} {old.get('name', '')}: QOH {info['from']['qoh']}; {dst}: QOH {info['to']['qoh']}",
                       f"{src} removed; {dst} QOH {store.ITEMS[dst]['qoh']}; lines moved ({counts})",
                       [{"code": src, "name": old.get("name", ""), "status": "Removed", "details": f"merged into {dst}"},
                        {"code": dst, "name": store.ITEMS[dst].get("name", ""), "status": "Changed",
                         "details": f"QOH {info['to']['qoh']} → {store.ITEMS[dst]['qoh']}"}])
    message = "Successfully Merged!" if mode == "merge" else "Item replaced and removed successfully"
    return {**info, "message": message, "qoh_after": store.ITEMS[dst]["qoh"]}
