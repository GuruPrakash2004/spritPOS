"""The Stock Balance screen's four views, as the desktop's frmRptStockBal shows them (ViewClData, ViewClDetailed, ViewPkgData, ViewGrpSmryData).

Everything is read from the stock list (UtlStockList: one row per movement, StkMultiplier +1 / -1) joined to the item, group, package and brand
masters, for active items only (`Inactive = 'N'`), like the exe. Like the exe, the Wt.Avg option rewrites UtlStockList.DocValue (only the rows whose value changes) and the FIFO option runs the shop's
`mSp_UpdateFIFORate` before reading; both write inside `db.direct_writes()` and are refused when the app is set read-only (DB_READ_ONLY).

Rate used for values: Last Purchase Rate (the newest purchase bill's PBRate, or NetAmt / PBQty "With Tax", up to the To date) or MRP.
Result: {"columns": [{key, label, kind, total}], "rows": [...]} where a row with "subtotal": true is a group / grand total row.
"""
from datetime import date

from fastapi import HTTPException

from . import db

DETAIL_TYPES = {"OP": "op", "PB": "pb", "ST": "pb", "INV": "inv", "STKDED": "sd", "SR": "sr", "PR": "pr", "SA": "sa"}


def _in(col: str, raw: str, args: list) -> str:
    vals = [v.strip() for v in (raw or "").split(",") if v.strip()]
    if not vals:
        return ""
    args.extend(vals)
    return f" AND {col} IN ({', '.join('?' * len(vals))})"


def _filters(groups: str, items: str, packages: str, args: list) -> str:
    return _in("g.ItemGroupCode", groups, args) + _in("i.ItemCode", items, args) + _in("i.PKGItem", packages, args)


def brand_required() -> bool:
    try:
        rows = db.fetchall("SELECT SetValue FROM UtlGSettings WITH (NOLOCK) WHERE SetName = 'BRANDREQ'")
    except Exception:   # noqa: BLE001
        return False
    return bool(rows) and str(rows[0]["SetValue"] or "").strip().upper() == "Y"


def _last_purchase_rates(to_date: date, with_tax: bool) -> dict[int, float]:
    """ItemID -> the newest purchase bill's rate up to the date (vwPBItemLastPrice slot 1)."""
    col = "ROUND(NetAmt / NULLIF(PBQty, 0), 2)" if with_tax else "PBRate"
    return {int(r["ItemID"]): float(r["rate"] or 0) for r in db.fetchall(
        f"SELECT ItemID, {col} rate FROM vwPBItemLastPrice WHERE Slno = 1 AND PBDate <= ?", (to_date.isoformat(),))}


def _item_rows(from_date: date, to_date: date, groups: str, items: str, packages: str, upto: date | None = None, aroed: bool = False) -> list[dict]:
    """One row per item, movement bucket (before the From date or not), document type and direction."""
    args: list = []
    where = _filters(groups, items, packages, args)
    args += [(upto or to_date).isoformat()]
    ar = ", ISNULL(i.AROEDPER, 0) AROEDPER" if aroed else ""   # the AROED Value option: the item's AROED per bottle
    ar_group = ", ISNULL(i.AROEDPER, 0)" if aroed else ""   # GROUP BY takes the expression, never the SELECT alias (that was the 500)
    pre = f"CASE WHEN s.DocDate < '{from_date.isoformat()}' THEN 1 ELSE 0 END"   # an ISO date from a date object: safe to inline (a parameter would differ between SELECT and GROUP BY)
    return db.fetchall(
        "SELECT i.ItemID, i.ItemCode, i.ItemName, i.MRP, i.PURRATE, i.HSNCode, i.BotPerCases, i.PKGItem, g.ItemGroupName, g.GroupOrderBy, "
        f"p.PkgCode, p.PkgOrderBy, b.BrandName, {pre} pre, s.DocType, s.StkMultiplier m{ar}, "
        "SUM(s.Qty * s.StkMultiplier) q, SUM(s.LtrQty * s.StkMultiplier) l, SUM(s.DocValue * s.StkMultiplier) v, SUM(s.DocValue) vr "
        "FROM UtlStockList s WITH (NOLOCK) JOIN MstItem i ON i.ItemID = s.ItemID JOIN MstItemGroup g ON g.ItemGroupID = i.ItemGroupID "
        "JOIN MstPkg p ON p.PkgID = i.PkgID JOIN MstBrand b ON b.BrandID = i.BrandID "
        f"WHERE i.Inactive = 'N' AND ISNULL(s.DelFlag, '') NOT IN ('Y', 'D'){where} AND s.DocDate <= ? "
        "GROUP BY i.ItemID, i.ItemCode, i.ItemName, i.MRP, i.PURRATE, i.HSNCode, i.BotPerCases, i.PKGItem, g.ItemGroupName, g.GroupOrderBy, "
        f"p.PkgCode, p.PkgOrderBy, b.BrandName, {pre}, s.DocType, s.StkMultiplier{ar_group}",
        tuple(args))


def _per_item(rows: list[dict]) -> dict[int, dict]:
    out: dict[int, dict] = {}
    for r in rows:
        it = out.setdefault(int(r["ItemID"]), {"r": r, "b": []})
        it["b"].append(r)
    return out


def _order(it: dict) -> tuple:
    r = it["r"]
    return (int(r["GroupOrderBy"] or 0), str(r["PKGItem"] or ""), int(r["PkgOrderBy"] or 0))


def _totals(rows: list[dict], cols: list[dict], group_total: bool, group_key: str = "group") -> list[dict]:
    """Group total rows (when asked) and a Grand Total row, summing the columns marked total."""
    sums = [c["key"] for c in cols if c.get("total")]
    out: list[dict] = []
    gt = {k: 0.0 for k in sums}
    cur, gs = None, {k: 0.0 for k in sums}

    def flush():
        if group_total and cur is not None:
            out.append({group_key: f"Total- {cur}", "subtotal": True, **{k: round(v, 3) for k, v in gs.items()}})

    for r in rows:
        if r[group_key] != cur:
            flush()
            cur, gs = r[group_key], {k: 0.0 for k in sums}
        out.append(r)
        for k in sums:
            v = float(r.get(k) or 0)
            gs[k] += v
            gt[k] += v
    flush()
    if rows:
        out.append({group_key: "Grand Total", "subtotal": True, **{k: round(v, 3) for k, v in gt.items()}})
    return out


def _war_update(item_ids: set[int]) -> None:
    """Wt.Avg Rate, exactly as the exe (frmRptStockBal.valuateWAR): per item, in date order over the whole stock list, a purchase (PB) adds its value
    (quantity x rate) to the running value and quantity and stores it as the row's DocValue; any other row, while quantity and value are both above
    zero, is valued at the running average (value / quantity, 2 decimals) and that value (quantity x average, negative for issues) is stored as
    its DocValue. Only rows whose stored value changes are written, so a second run writes nothing."""
    rows = db.fetchall(
        "SELECT ItemID, DocType, StkMultiplier * Qty q, StkMultiplier * Qty * Rate dv, DocValue, RowID FROM UtlStockList WITH (NOLOCK) "
        "WHERE ISNULL(DelFlag, '') NOT IN ('Y', 'D') ORDER BY ItemID, DocDate, RowID")
    cur, qty, val = None, 0.0, 0.0
    changes: list[tuple] = []
    for r in rows:
        iid = int(r["ItemID"])
        if iid not in item_ids:
            continue
        if iid != cur:
            cur, qty, val = iid, 0.0, 0.0
        q, dv = float(r["q"] or 0), float(r["dv"] or 0)
        new = None
        if str(r["DocType"] or "").strip() == "PB":
            qty += q
            val += dv
            new = dv
        elif qty > 0 and val > 0:
            new = q * round(val / qty, 2)
            qty += q
            val += new
        if new is not None and abs(float(r["DocValue"] or 0) - new) > 0.005:
            changes.append((round(new, 4), int(r["RowID"])))
    if changes:
        with db.direct_writes():
            for i in range(0, len(changes), 5000):
                db.executemany("UPDATE UtlStockList SET DocValue = ? WHERE RowID = ?", changes[i:i + 5000])


def build(view: str, from_date: date, to_date: date, groups: str = "", items: str = "", packages: str = "", litres: bool = False,
          values: bool = False, rate: str = "pur", with_tax: bool = False, act_sales: bool = False, zero_hide: bool = False,
          group_total: bool = False, hide_cost: bool = False, aroed: bool = False) -> dict:
    if to_date < from_date:
        raise HTTPException(422, "Invalid Date Range!")
    show_values = values and not hide_cost
    if view == "closing":
        return _closing(from_date, to_date, groups, items, packages, litres, show_values, rate, with_tax, zero_hide, group_total)
    if view == "detailed":
        return _detailed(from_date, to_date, groups, items, packages, litres, show_values, rate, with_tax, act_sales, zero_hide, group_total, aroed)
    if view == "package":
        return _package(to_date, groups, items, packages, litres, zero_hide, group_total)
    if view == "group":
        return _group_summary(from_date, to_date, rate, hide_cost)
    raise HTTPException(422, "Unknown view")


def _rate_of(rate: str, it: dict, pur: dict[int, float]) -> float:
    r = it["r"]
    if rate == "mrp":
        return float(r["MRP"] or 0)
    return pur.get(int(r["ItemID"]), 0.0)


def _closing(from_date, to_date, groups, items, packages, litres, values, rate, with_tax, zero_hide, group_total) -> dict:
    """Closing Balance Only (ViewClData): opening, receipts (+ movements), issues (- movements) and closing per item, value at the rate."""
    per = _per_item(_item_rows(from_date, to_date, groups, items, packages))
    stored = values and not litres and rate in ("war", "fifo")   # Wt.Avg / FIFO: the closing value is the stored DocValues, as in the detailed view
    if stored and rate == "war":
        _war_update(set(per))
    elif stored:
        with db.direct_writes():
            db.execute("exec mSp_UpdateFIFORate")
    if stored:
        per = _per_item(_item_rows(from_date, to_date, groups, items, packages))
    pur = _last_purchase_rates(to_date, with_tax) if values and rate == "pur" else {}
    key = "l" if litres else "q"
    out = []
    # The exe: order by GroupOrderBy, ItemGroupName, PkgItem, PkgOrderBy, StkRate desc. Items equal on all of those (the same rate) come out in the
    # order the database stores them, which is ItemID, so that is the last key (Windows shows ABCD, 222, 132 ... the same way).
    def exe_order(x: dict) -> tuple:
        g, p, o = _order(x)
        return (g, str(x["r"]["ItemGroupName"] or "").strip(), p, o, -_rate_of(rate, x, pur), int(x["r"]["ItemID"]))
    for it in sorted(per.values(), key=exe_order):
        op = sum(float(b[key] or 0) for b in it["b"] if b["pre"])
        rc = sum(float(b[key] or 0) for b in it["b"] if not b["pre"] and b["m"] > 0)
        iss = -sum(float(b[key] or 0) for b in it["b"] if not b["pre"] and b["m"] < 0)
        cl = op + rc - iss
        qcl = sum(float(b["q"] or 0) for b in it["b"])
        rt = _rate_of(rate, it, pur)
        row = {"group": str(it["r"]["ItemGroupName"] or "").strip(), "brand": str(it["r"]["BrandName"] or "").strip(),
               "item_name": str(it["r"]["ItemName"] or "").strip(), "pkg": str(it["r"]["PkgCode"] or "").strip(),
               "op": op, "rcpt": rc, "iss": iss, "cl": cl}
        if stored:
            vcl = sum(float((b["vr"] if rate == "war" else b["v"]) or 0) for b in it["b"])
            row["cl_value"] = round(vcl, 2)
            row["rate"] = round(vcl / qcl, 2) if qcl else 0.0
        elif values:
            row["rate"] = round(rt, 2)
            row["cl_value"] = round(qcl * rt, 2)
        if zero_hide and not (op or rc or iss or cl or row.get("cl_value")):
            continue
        out.append(row)
    sfx = "-Ltr" if litres else ""
    sfx2 = " in Ltrs" if litres else ""
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "brand", "label": "Item Brand Name", "brand": True},
            {"key": "item_name", "label": "Item Name"}, {"key": "pkg", "label": "PKG"},
            {"key": "op", "label": "Opening Quantity" + sfx, "kind": "number", "total": True},
            {"key": "rcpt", "label": "Receipt Quantity" + sfx, "kind": "number", "total": True},
            {"key": "iss", "label": "Issued Quantity" + sfx, "kind": "number", "total": True},
            {"key": "cl", "label": "Closing Quantity" + sfx, "kind": "number", "total": True}]
    if values:
        cols += [{"key": "cl_value", "label": "Closing Value", "kind": "money", "total": True},
                 {"key": "rate", "label": {"mrp": "Rate [MRP]", "war": "Rate [Wt.Avg]", "fifo": "Rate [FIFO]"}.get(rate, "Rate [Last Purchase]"), "kind": "money"}]
    del sfx2
    return {"columns": cols, "rows": _totals(out, cols, group_total), "brand_required": brand_required()}


def _detailed(from_date, to_date, groups, items, packages, litres, values, rate, with_tax, act_sales, zero_hide, group_total, aroed=False) -> dict:
    """Closing Balance With Detailed (ViewClDetailed): OP, PB (+ stock transfers in), SR, Inv, Ded, PR, SA, closing quantities per item and,
    with Show Value, their values at the rate (Last Purchase / MRP), at the running weighted average (Wt.Avg) or at the stored DocValue (FIFO)."""
    per = _per_item(_item_rows(from_date, to_date, groups, items, packages, aroed=aroed))
    pur = _last_purchase_rates(to_date, with_tax) if values and rate in ("pur",) else {}
    if values and rate == "war" and not litres:   # as the exe: the stored DocValues are brought up to the weighted average first
        _war_update(set(per))
        per = _per_item(_item_rows(from_date, to_date, groups, items, packages, aroed=aroed))
    if values and rate == "fifo" and not litres:   # as the exe: its FIFO rates are refreshed by the shop's own procedure first
        with db.direct_writes():
            db.execute("exec mSp_UpdateFIFORate")
        per = _per_item(_item_rows(from_date, to_date, groups, items, packages, aroed=aroed))
    sales_amt: dict[int, float] = {}
    if values and act_sales:   # Actual Sales Value: the invoice lines' own ItemAmt
        for a in db.fetchall(
                "SELECT d.ItemID, SUM(d.ItemAmt) amt FROM TrnDtlSINV d WITH (NOLOCK) WHERE d.SINVID IN (SELECT DISTINCT DocID FROM UtlStockList WITH (NOLOCK) "
                "WHERE DocType = 'INV' AND DocDate >= ? AND DocDate <= ? AND ISNULL(DelFlag, '') NOT IN ('Y', 'D')) GROUP BY d.ItemID",
                (from_date.isoformat(), to_date.isoformat())):
            sales_amt[int(a["ItemID"])] = float(a["amt"] or 0)
    key = "l" if litres else "q"
    names = ["op", "pb", "sr", "inv", "sd", "pr", "sa"]
    out = []
    for it in sorted(per.values(), key=lambda x: _order(x) + (-_rate_of(rate, x, pur), str(x["r"]["ItemName"]))):
        r0 = it["r"]
        q = {n: 0.0 for n in names}
        v = {n: 0.0 for n in names}
        for b in it["b"]:
            n = "op" if b["pre"] else DETAIL_TYPES.get(str(b["DocType"] or "").strip().upper())
            if n is None:
                continue
            qq = float(b[key] or 0)
            q[n] += qq if n == "op" else abs(qq)
            v[n] += float(b["v"] or 0) if n == "op" else abs(float(b["v"] or 0))
        iid = int(r0["ItemID"])
        # closing = the signed sum of every counted movement (the exe adds each row's signed quantity)
        cl = sum(float(b[key] or 0) for b in it["b"] if b["pre"] or DETAIL_TYPES.get(str(b["DocType"] or "").strip().upper()))
        bpc = float(r0["BotPerCases"] or 0)
        row = {"group": str(r0["ItemGroupName"] or "").strip(), "item_code": str(r0["ItemCode"] or "").strip(), "item_name": str(r0["ItemName"] or "").strip(),
               "pkg": str(r0["PkgCode"] or "").strip(), "hsn": str(r0["HSNCode"] or "").strip(), "brand": str(r0["BrandName"] or "").strip(),
               "item_id": iid, "bpc": bpc, "op": q["op"], "pb": q["pb"], "sr": q["sr"], "inv": q["inv"], "sd": q["sd"], "pr": q["pr"], "sa": q["sa"], "cl": cl}
        if q["pb"] and bpc:
            row["in_case"] = f"{int(q['pb'] // bpc)} Cases {q['pb'] % bpc:g} Bottles" if not litres else ""
        if values and not litres:
            rt = float(r0["MRP"] or 0) if rate == "mrp" else pur.get(iid, 0.0) if rate == "pur" else 0.0
            if rate == "war":
                # stored DocValues: the OP column is their signed sum, the others their absolute sums, closing the signed total (exe :1610-1614)
                v = {n: 0.0 for n in names}
                for b in it["b"]:
                    n = "op" if b["pre"] else DETAIL_TYPES.get(str(b["DocType"] or "").strip().upper())
                    if n:
                        v[n] += float(b["vr"] or 0) if n == "op" else abs(float(b["vr"] or 0))
                v["cl"] = sum(float(b["vr"] or 0) for b in it["b"] if b["pre"] or DETAIL_TYPES.get(str(b["DocType"] or "").strip().upper()))
            elif rate == "fifo":
                v["cl"] = sum(float(b["v"] or 0) for b in it["b"] if b["pre"] or DETAIL_TYPES.get(str(b["DocType"] or "").strip().upper()))
            else:
                v = {n: round(q[n] * rt, 2) for n in names}
                v["cl"] = round(cl * rt, 2)
                if act_sales:
                    v["inv"] = round(sales_amt.get(iid, 0.0), 2)
            row["rate"] = round(rt if rate in ("pur", "mrp") else (v["cl"] / cl if cl else 0.0), 2)
            row["rate_case"] = round(bpc * float(r0["PURRATE"] or 0), 2)
            for n in names:
                row[n + "_value"] = round(v[n], 2)
            row["cl_value"] = round(v["cl"], 2)
            row["inv_rate"] = round(row["inv_value"] / q["inv"], 2) if q["inv"] > 0 else 0.0
            if aroed:   # AROED Value (ChkAROED): the quantity x the item's AROED per bottle, and Total Value = that + the value
                per_bottle = float(r0.get("AROEDPER") or 0)
                for n in names + ["cl"]:
                    qty_n = cl if n == "cl" else q[n]
                    row[n + "_aroed"] = round(qty_n * per_bottle, 2)
                    row[n + "_total"] = round(row[n + "_aroed"] + row[n + "_value"], 2)
        if zero_hide and not any(row[n] for n in names + ["cl"]):
            continue
        out.append(row)
    sfx = " in Ltrs" if litres else ""
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "brand", "label": "Item Brand Name", "brand": True},
            {"key": "item_code", "label": "Item Code"}, {"key": "item_name", "label": "Item Name"},
            {"key": "item_id", "label": "ItemID", "export_only": True}, {"key": "pkg", "label": "PKG"},
            {"key": "hsn", "label": "HSNCODE"}]
    if values and not litres:
        cols += [{"key": "rate", "label": {"mrp": "MRP", "pur": "Pur.Rate"}.get(rate, "Rate"), "kind": "money"},
                 {"key": "rate_case", "label": "Rate Per Case", "kind": "money"}]
    # the exe's column order and headings (an export of its grid): Inv Rate sits between Inv Qty and Inv Value, and the AROED headings read
    # OP / PB / SP / INV / SD / PR / SA / CL
    ar_lab = {"op": "OP", "pb": "PB", "sr": "SP", "inv": "INV", "sd": "SD", "pr": "PR", "sa": "SA", "cl": "CL"}
    for n, lab in (("op", "OP"), ("pb", "PB"), ("sr", "SR"), ("inv", "Inv"), ("sd", "Ded"), ("pr", "PR"), ("sa", "SA"), ("cl", "Cl")):
        cols.append({"key": n, "label": f"{lab} Qty{sfx}", "kind": "number", "total": True})
        if values and not litres:
            if n == "inv":
                cols.append({"key": "inv_rate", "label": "Inv Rate", "kind": "money"})
            cols.append({"key": n + "_value", "label": f"{lab} Value", "kind": "money", "total": True})
            if aroed:
                cols += [{"key": n + "_aroed", "label": f"{ar_lab[n]} AROED Value", "kind": "money", "total": True},
                         {"key": n + "_total", "label": f"{ar_lab[n]} Total Value", "kind": "money", "total": True}]
    if not litres:
        cols.append({"key": "bpc", "label": "Bottle PerCase", "kind": "number", "total": True, "export_only": True})   # hidden in the grid, in the exe's Excel
        cols.append({"key": "in_case", "label": "In Case"})
    return {"columns": cols, "rows": _totals(out, cols, group_total), "brand_required": brand_required()}


def _package(to_date, groups, items, packages, litres, zero_hide, group_total) -> dict:
    """Package wise Balance (ViewPkgData): one column per package size, one row per group / brand / main item, closing quantity up to the To date."""
    per = _per_item(_item_rows(to_date, to_date, groups, items, packages))
    key = "l" if litres else "q"
    pk_order: dict[str, int] = {}
    rows: dict[tuple, dict] = {}
    for it in per.values():
        r0 = it["r"]
        pk = str(r0["PkgCode"] or "").strip()
        pk_order[pk] = min(pk_order.get(pk, 10**9), int(r0["PkgOrderBy"] or 0))
        qty = sum(float(b[key] or 0) for b in it["b"])
        k = (int(r0["GroupOrderBy"] or 0), str(r0["ItemGroupName"] or "").strip(), str(r0["BrandName"] or "").strip(), str(r0["PKGItem"] or "").strip())
        row = rows.setdefault(k, {"group": k[1], "brand": k[2], "main_item": k[3], "total": 0.0})
        row["total"] += qty
        row[f"pk_{pk}"] = row.get(f"pk_{pk}", 0.0) + qty
    pks = sorted(pk_order, key=lambda p: (pk_order[p], p))
    out = [r for k, r in sorted(rows.items(), key=lambda kv: (kv[0][0], kv[0][3])) if not (zero_hide and not r["total"])]
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "brand", "label": "Item Brand Name", "brand": True},
            {"key": "main_item", "label": "Main Item"}, {"key": "total", "label": "Total Quantity(Ltr)" if litres else "Total Quantity", "kind": "number", "total": True}]
    cols += [{"key": f"pk_{p}", "label": p, "kind": "number", "total": True} for p in pks]
    return {"columns": cols, "rows": _totals(out, cols, group_total), "brand_required": brand_required()}


def _group_summary(from_date, to_date, rate, hide_cost) -> dict:
    """Group wise Summary Balance (ViewGrpSmryData): opening value (before the From date) and closing value per liquor item group and one
    line for non-liquor. The exe counts the closing value up to the From date as well, which is repeated here."""
    if hide_cost:
        raise HTTPException(403, "Values are hidden for your role")
    pur = _last_purchase_rates(to_date, False) if rate == "pur" else {}
    fifo = rate == "fifo"
    pre = f"CASE WHEN s.DocDate < '{from_date.isoformat()}' THEN 1 ELSE 0 END"
    rows = db.fetchall(
        f"SELECT i.ItemID, i.MRP, g.ItemGroupName, g.LiqItem, g.GroupOrderBy, {pre} pre, "
        + ("s.FifoRate fr, " if fifo else "0 fr, ") +
        "SUM(s.Qty * s.StkMultiplier) q FROM UtlStockList s WITH (NOLOCK) JOIN MstItem i ON i.ItemID = s.ItemID JOIN MstItemGroup g ON g.ItemGroupID = i.ItemGroupID "
        "WHERE i.Inactive = 'N' AND ISNULL(s.DelFlag, '') NOT IN ('Y', 'D') AND s.DocDate <= ? "
        f"GROUP BY i.ItemID, i.MRP, g.ItemGroupName, g.LiqItem, g.GroupOrderBy, {pre}" + (", s.FifoRate" if fifo else ""),
        (from_date.isoformat(),))
    agg: dict[tuple, dict] = {}
    for r in rows:
        liquor = str(r["LiqItem"] or "").strip().upper() == "Y"
        rt = float(r["fr"] or 0) if fifo else float(r["MRP"] or 0) if rate == "mrp" else pur.get(int(r["ItemID"]), 0.0)
        k = ("Liquor" if liquor else "Non-Liquor", str(r["ItemGroupName"] or "").strip() if liquor else "", int(r["GroupOrderBy"] or 0) if liquor else 0)
        a = agg.setdefault(k, {"category": k[0], "group": k[1], "op_value": 0.0, "cl_value": 0.0})
        q = float(r["q"] or 0)
        if r["pre"]:
            a["op_value"] += q * rt
        a["cl_value"] += q * rt   # up to and including the From date, as the exe
    out = [{**a, "op_value": round(a["op_value"], 2), "cl_value": round(a["cl_value"], 2)} for k, a in sorted(agg.items(), key=lambda kv: (kv[0][0], kv[0][1]))]
    cols = [{"key": "group", "label": "Item Group Name"}, {"key": "category", "label": "Category"},
            {"key": "op_value", "label": "Opening Value", "kind": "money", "total": True}, {"key": "cl_value", "label": "Closing Value", "kind": "money", "total": True}]
    return {"columns": cols, "rows": _totals(out, cols, False), "brand_required": False}
