"""The desktop's Stock Ageing report (frmRptStockAgeing): closing stock per item split by the age of the receipts, in age ranges the user sets,
or the layer list "Show Ageing - Purchase Bill Wise". Read only; the stock list (UtlStockList) and the stock rates (UtlStockRate) are the sources.

Ranges (the exe's grid): rows of From / To days; the first starts at 0, each next From is the previous To + 1, the last row has no To (">= From").
Defaults 0-30, 31-60, 61-90, 91-180, 180-. A To lower than its From is "Invalid Range! Check Entry...".
Valuation: Standard / FIFO / Wt.Avg. / Last Purchase Rate = the column of UtlStockRate on the newest StkDate up to the as-on date (0 when the table is empty).
Like the exe, the age of each receipt (StkMultiplier = 1 row, any date) is counted in days up to the as-on date and its quantity is added to its range;
issues are not taken off the ranges (an exe quirk: the range totals can add up to more than the closing quantity).
"""
from datetime import date

from fastapi import HTTPException

from . import db

RATE_COLUMN = {"standard": "StkRate", "fifo": "FIFORate", "wtavg": "AvgRate", "lastpur": "LastPurRate"}
DEFAULT_RANGES = "0-30,31-60,61-90,91-180,180-"


def parse_ranges(raw: str) -> list[tuple[int, int | None]]:
    out: list[tuple[int, int | None]] = []
    for part in (raw or DEFAULT_RANGES).split(","):
        part = part.strip()
        if not part:
            continue
        a, _, b = part.partition("-")
        try:
            lo = int(a or 0)
            hi = int(b) if b.strip() else None
        except ValueError:
            raise HTTPException(422, "Invalid Range! Check Entry...") from None
        if hi is not None and hi < lo:
            raise HTTPException(422, "Invalid Range! Check Entry...")
        out.append((lo, hi))
    if not out:
        raise HTTPException(422, "Enter at least one age range")
    return out


def _in(col: str, raw: str, args: list) -> str:
    vals = [v.strip() for v in (raw or "").split(",") if v.strip()]
    if not vals:
        return ""
    args.extend(vals)
    return f" AND {col} IN ({', '.join('?' * len(vals))})"


def _closing(as_on: date, groups: str, packages: str) -> list[dict]:
    args: list = [as_on.isoformat()]
    where = _in("g.ItemGroupCode", groups, args) + _in("i.PKGItem", packages, args)
    return db.fetchall(
        "SELECT g.ItemGroupName, g.GroupOrderBy, s.ItemID, i.ItemName, SUM(s.Qty * s.StkMultiplier) cl FROM MstItemGroup g, MstItem i, UtlStockList s WITH (NOLOCK) "
        f"WHERE s.ItemID = i.ItemID AND i.ItemGroupID = g.ItemGroupID AND s.DocDate <= ? AND ISNULL(s.DelFlag, '') NOT IN ('Y', 'D'){where} "
        "GROUP BY g.ItemGroupName, g.GroupOrderBy, s.ItemID, i.ItemName HAVING SUM(s.Qty * s.StkMultiplier) > 0 ORDER BY g.GroupOrderBy", tuple(args))


def _rate(as_on: date, valuation: str) -> dict[int, float]:
    col = RATE_COLUMN.get(valuation, "StkRate")
    d = db.fetchall("SELECT MAX(StkDate) d FROM UtlStockRate WHERE StkDate <= ?", (as_on.isoformat(),))
    if not d or d[0]["d"] is None:
        return {}
    return {int(r["ItemID"]): float(r["rate"] or 0) for r in db.fetchall(f"SELECT ItemID, {col} rate FROM UtlStockRate WHERE StkDate = ?", (d[0]["d"],))}


def ranges_view(as_on: date, ranges_raw: str = "", groups: str = "", packages: str = "", valuation: str = "standard", hide_cost: bool = False) -> dict:
    ranges = parse_ranges(ranges_raw)
    rows = _closing(as_on, groups, packages)
    rates = {} if hide_cost else _rate(as_on, valuation)
    by_item = {int(r["ItemID"]): {"group": str(r["ItemGroupName"] or "").strip(), "item_name": str(r["ItemName"] or "").strip(), "cl": float(r["cl"] or 0),
                                  "b": [0.0] * len(ranges)} for r in rows}
    for r in db.fetchall("SELECT ItemID, DocDate, SUM(Qty) q FROM UtlStockList WITH (NOLOCK) WHERE StkMultiplier = 1 AND ISNULL(DelFlag, '') NOT IN ('Y', 'D') "
                         "GROUP BY ItemID, DocDate"):
        it = by_item.get(int(r["ItemID"]))
        if it is None:
            continue
        age = (as_on - r["DocDate"].date()).days
        col = None
        for i, (lo, hi) in enumerate(ranges):   # the exe's loop: the last matching range wins
            if i == 0:
                if age <= (hi if hi is not None else 10**9):
                    col = 0
            elif i == len(ranges) - 1:
                if age >= lo:
                    col = i
            elif age >= lo and (hi is None or age <= hi):
                col = i
        if col is not None:
            it["b"][col] += float(r["q"] or 0)
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "item_name", "label": "Item Name"}, {"key": "cl", "label": "Closing Quantity", "kind": "number", "total": True}]
    if not hide_cost:
        cols.append({"key": "cl_value", "label": "Closing Value", "kind": "money", "total": True})
    for i, (lo, hi) in enumerate(ranges):
        label = f"{lo}" if i == len(ranges) - 1 else f"{lo} - {hi}"
        cols.append({"key": f"r{i}", "label": label, "kind": "number", "total": True})
        cols.append({"key": f"r{i}_v", "label": "Value", "kind": "money"})   # the exe adds this column but never fills it
    out = []
    for iid, it in by_item.items():
        row = {"group": it["group"], "item_name": it["item_name"], "cl": it["cl"]}
        if not hide_cost:
            row["cl_value"] = round(it["cl"] * rates.get(iid, 0.0), 2)
        for i, q in enumerate(it["b"]):
            row[f"r{i}"] = q
        out.append(row)
    return {"columns": cols, "rows": out}


def pb_view(as_on: date, groups: str = "", packages: str = "") -> dict:
    """Show Ageing - Purchase Bill Wise: for each item still in stock, the newest receipts (any date, as the exe) that add up to the closing quantity."""
    rows = _closing(as_on, groups, packages)
    today = date.today()
    layers: dict[int, list[dict]] = {}
    for r in db.fetchall("SELECT RowID, ItemID, DocDate, DocNo, Particulars, Qty, Rate FROM UtlStockList WITH (NOLOCK) WHERE StkMultiplier = 1 "
                         "AND ISNULL(DelFlag, '') NOT IN ('Y', 'D') ORDER BY DocDate DESC, RowID DESC"):
        layers.setdefault(int(r["ItemID"]), []).append(r)
    out = []
    for c in rows:
        bal = float(c["cl"] or 0)
        first = True
        for l in layers.get(int(c["ItemID"]), []):
            if bal <= 0:
                break
            qty, rate = float(l["Qty"] or 0), float(l["Rate"] or 0)
            left = qty if bal > qty else bal
            out.append({"group": str(c["ItemGroupName"] or "").strip(), "item_name": str(c["ItemName"] or "").strip(), "cl": float(c["cl"] or 0) if first else None,
                        "pb_no": str(l["DocNo"] or "").strip(), "pb_date": str(l["DocDate"])[:10], "age_days": (today - l["DocDate"].date()).days,
                        "particulars": str(l["Particulars"] or "").strip(), "qty": qty, "left": left, "rate": rate, "amount": round(left * rate, 2)})
            first = False
            bal -= qty
        if first:   # no receipt on record: the item still shows with its closing quantity
            out.append({"group": str(c["ItemGroupName"] or "").strip(), "item_name": str(c["ItemName"] or "").strip(), "cl": float(c["cl"] or 0)})
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "item_name", "label": "Item Name"}, {"key": "cl", "label": "ClQty", "kind": "number"},
            {"key": "pb_no", "label": "PB No"}, {"key": "pb_date", "label": "PB Date"}, {"key": "age_days", "label": "Age Days", "kind": "number"},
            {"key": "particulars", "label": "Particulars"}, {"key": "qty", "label": "PB Qty", "kind": "number"},
            {"key": "left", "label": "Qty Left", "kind": "number", "total": True}, {"key": "rate", "label": "Rate", "kind": "money"},
            {"key": "amount", "label": "Amount", "kind": "money", "total": True}]
    return {"columns": cols, "rows": out}
