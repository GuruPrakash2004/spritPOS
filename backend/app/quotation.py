"""Quotation (legacy document 1282, tables TrnHdrQTN / TrnDtlQTN): a priced offer to a customer. It does not change stock or post to the ledgers.
Only the clients whose menu shows Quotation have these tables (BW, NDrops, NDROPSBLDR here); the others answer 404. Read from and written to the shop's
own tables in one transaction, numbered like every desktop document (<year digits><00000>, printed 26//00001)."""
from datetime import date, datetime, timedelta

from fastapi import HTTPException

from . import accounting, db, formulas, gsettings, store

HDR, DTL = "TrnHdrQTN", "TrnDtlQTN"


def _need() -> None:
    if not (accounting._exists(HDR) and accounting._exists(DTL)):
        raise HTTPException(404, "This database does not have the quotation tables (TrnHdrQTN, TrnDtlQTN)")


def list_quotations(first: date, last: date) -> list[dict]:
    _need()
    rows = db.fetchall(f"""SELECT QTNID, PrintQTNNO, QTNDate, PartyName, TOTQTY, GRNDTOT, Cancelled, AddedBy FROM {HDR} WITH (NOLOCK)
                           WHERE QTNDate >= ? AND QTNDate < ? ORDER BY QTNID DESC""", (first.isoformat(), (last + timedelta(days=1)).isoformat()))
    return [{"id": int(r["QTNID"]), "doc_no": (r["PrintQTNNO"] or "").strip(), "date": str(r["QTNDate"])[:10], "customer": (r["PartyName"] or "").strip(),
             "qty": accounting.money(r["TOTQTY"]), "total": accounting.money(r["GRNDTOT"]), "status": "CANCELLED" if r.get("Cancelled") == "Y" else "SAVED",
             "added_by": (r["AddedBy"] or "").strip()} for r in rows]


def get_quotation(qid: int) -> dict:
    _need()
    h = db.fetchone(f"SELECT * FROM {HDR} WITH (NOLOCK) WHERE QTNID = ?", (qid,))
    if not h:
        raise HTTPException(404, f"Quotation {qid} not found")
    lines = db.fetchall(f"""SELECT d.SlNo, d.QTNQty, d.QTNRate, d.QTNAmt, i.ItemCode, i.ItemName FROM {DTL} d WITH (NOLOCK)
                            LEFT JOIN MstITEM i WITH (NOLOCK) ON i.ItemID = d.ITEMID WHERE d.QTNID = ? ORDER BY d.SlNo""", (qid,))
    return {"id": qid, "doc_no": (h.get("PrintQTNNO") or "").strip(), "date": str(h.get("QTNDate"))[:10], "customer": (h.get("PartyName") or "").strip(), "customer_id": int(h.get("CUSTID") or 0),
            "remarks": (h.get("RMKS") or "").strip(), "total": accounting.money(h.get("GRNDTOT")), "qty": accounting.money(h.get("TOTQTY")),
            "status": "CANCELLED" if h.get("Cancelled") == "Y" else "SAVED", "added_by": (h.get("AddedBy") or "").strip(),
            "lines": [{"item_code": (r["ItemCode"] or "").strip(), "item_name": (r["ItemName"] or "").strip(), "qty": accounting.money(r["QTNQty"]),
                       "rate": accounting.money(r["QTNRate"]), "amount": accounting.money(r["QTNAmt"])} for r in lines]}


def _prepare(body) -> tuple[dict | None, list[tuple], float, float]:
    """Check a quotation and work out its customer, lines and totals (shared by a new and an edited one)."""
    codes = [l.item_code for l in body.lines]
    unknown = [c for c in codes if c not in store.ITEMS]
    if unknown:
        raise HTTPException(422, f"Unknown item(s): {', '.join(unknown[:5])}")
    marks = ",".join("?" * len(codes))
    uom = {r["ItemCode"].strip(): int(r["UOMID"] or 0) for r in db.fetchall(f"SELECT ItemCode, UOMID FROM MstITEM WHERE ItemCode IN ({marks})", tuple(codes))}
    cust_code = (body.customer or "CASH").strip()
    cust = None if cust_code.upper() == "CASH" else db.fetchone("SELECT CUSTID, CUSTNAME FROM MstCUST WHERE CUSTCODE = ? OR CUSTNAME = ?", (cust_code, cust_code))
    if cust_code.upper() != "CASH" and not cust:
        raise HTTPException(422, f"Unknown customer '{cust_code}'")
    lines = []
    for l in body.lines:
        it = store.ITEMS[l.item_code]
        rate = round(float(l.rate if l.rate is not None else it.get("mrp", 0)), 2)
        lines.append((l.item_code, uom.get(l.item_code, 0), l.qty, rate, round(l.qty * rate, 2)))
    return cust, lines, sum(x[2] for x in lines), round(sum(x[4] for x in lines), 2)


def _content(body, cust, total_qty: float, total: float) -> dict:
    return {"QTNDate": body.date.isoformat(), "QTNDateYear": date(body.date.year, 1, 1).isoformat(),
            "QTNDateMonth": date(body.date.year, body.date.month, 1).isoformat(), "CUSTID": int(cust["CUSTID"]) if cust else 0,
            "PartyName": (cust["CUSTNAME"].strip() if cust else "CASH")[:150], "TOTQTY": total_qty,
            "AMTINWRDS": formulas.amount_in_words(total)[:250], "RMKS": body.remarks[:250], "TOTAL": total, "GRNDTOT": total}


def _insert_lines(cur, qid: int, lines: list[tuple]) -> None:
    cur.execute("SELECT COLUMNPROPERTY(OBJECT_ID('TrnDtlQTN'), 'QTNDtlID', 'IsIdentity')")
    ident = cur.fetchone()[0] == 1
    for i, (code, uom_id, qty, rate, amount) in enumerate(lines, start=1):
        row = {"QTNID": qid, "SlNo": i, "ITEMID": int(store.ITEMS[code].get("_itemid") or 0), "UOMID": uom_id, "ItemSlNO": i, "ShortClosed": "N",
               "ShortClosedBy": 0, "QTNQty": qty, "QTNRate": rate, "QTNAmt": amount}
        if not ident:
            cur.execute("SELECT ISNULL(MAX(QTNDtlID), 0) + 1 FROM TrnDtlQTN WITH (UPDLOCK, HOLDLOCK)")
            row["QTNDtlID"] = int(cur.fetchone()[0])
        accounting._insert(cur, DTL, row)


def create_quotation(body, user: str) -> dict:
    _need()
    gsettings.check_not_frozen(body.date, "This quotation")
    cust, lines, total_qty, total = _prepare(body)
    now, have = datetime.now(), accounting._columns(HDR)
    with db.direct_writes():
        with db.transaction() as cur:
            no, print_no, _ = accounting.next_number(cur, HDR, "QTNNO")
            dash = "-"
            hdr = {"QTNNO": no, "Prefix": "", "PrintQTNNO": print_no, **_content(body, cust, total_qty, total),
                   "PartyAdd1": dash, "PartyAdd2": dash, "PartyAdd3": dash, "PartyAdd4": dash,
                   "PartyCity": dash, "PartyState": dash, "PartyPinCode": dash, "PartyCountry": dash, "PartyCName": dash, "PartyCMob": dash,
                   "PartyCEmail": dash, "PartyCDesig": dash, "Authorised": "Y", "AuthBy": "", "ChangedBy": user, "Cancelled": "N", "AddedBy": user, "AddedOn": now}
            qid = accounting._insert(cur, HDR, {k: v for k, v in hdr.items() if k.lower() in have}, "QTNID")
            _insert_lines(cur, qid, lines)
    return get_quotation(qid)


def _open(qid: int, what: str) -> dict:
    """The quotation to edit or cancel: it must exist, not be cancelled, and not be dated on or before the freeze date."""
    _need()
    h = db.fetchone(f"SELECT QTNID, QTNDate, Cancelled FROM {HDR} WITH (NOLOCK) WHERE QTNID = ?", (qid,))
    if not h:
        raise HTTPException(404, f"Quotation {qid} not found")
    if h.get("Cancelled") == "Y":
        raise HTTPException(409, f"This quotation is cancelled; it can't be {what}")
    gsettings.check_not_frozen(str(h["QTNDate"])[:10], "This quotation")
    return h


def update_quotation(qid: int, body, user: str) -> dict:
    """Edit a quotation: the header is updated and its lines rewritten in one transaction. The number stays."""
    _open(qid, "edited")
    gsettings.check_not_frozen(body.date, "This quotation")
    cust, lines, total_qty, total = _prepare(body)
    have = accounting._columns(HDR)
    vals = {k: v for k, v in {**_content(body, cust, total_qty, total), "ChangedBy": user, "ChangedOn": datetime.now()}.items() if k.lower() in have}
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE {HDR} SET {', '.join(f'{k} = ?' for k in vals)} WHERE QTNID = ? AND Cancelled <> 'Y'", (*vals.values(), qid))
            cur.execute(f"DELETE FROM {DTL} WHERE QTNID = ?", (qid,))
            _insert_lines(cur, qid, lines)
    return get_quotation(qid)


def cancel_quotation(qid: int, user: str) -> dict:
    """Cancel a quotation: the header is flagged Cancelled = 'Y' (it posts nothing and moves no stock, so nothing else changes)."""
    _open(qid, "cancelled again")
    have = accounting._columns(HDR)
    sets = ["Cancelled = 'Y'"] + (["ChangedBy = ?", "ChangedOn = ?"] if {"changedby", "changedon"} <= have else [])
    args = (user, datetime.now()) if len(sets) > 1 else ()
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE {HDR} SET {', '.join(sets)} WHERE QTNID = ? AND Cancelled <> 'Y'", (*args, qid))
    return get_quotation(qid)
