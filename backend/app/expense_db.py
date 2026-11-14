"""Expense Entry and Day's OP-Balance, written straight into the shop's own tables (legacy frmTrnRDM / frmUtlDaysExpOPBalance).

An expense entry is TrnHdrEXP (EXPNO, date, TOTAMT ...) with TrnDtlEXP lines (LEDID, AMT = the expense, RAMT = a receipt into the petty cash, REMARKS);
numbered like every desktop document (<year digits><00000>, printed 26//00115). Day's OP-Balance is TrnExpOPBal (ExpDate, OpBalAmt): saving a date
replaces its row, as the exe does. One transaction each, nothing of the web app's own is kept in the database."""
from datetime import date, datetime

from fastapi import HTTPException

from . import accounting, db, db_store, formulas, gsettings, store


def create_expense(body, user: str) -> dict:
    accounting._need("TrnHdrEXP", "TrnDtlEXP", "MstLed")
    gsettings.check_not_frozen(body.date, "This expense entry")
    codes = sorted({l.ledger for l in body.lines})
    rows = db.fetchall(f"SELECT LEDID, LEDCODE FROM MstLed WHERE LEDCODE IN ({','.join('?' * len(codes))})", tuple(codes))
    ids = {r["LEDCODE"].strip(): int(r["LEDID"]) for r in rows}
    if set(codes) - set(ids):
        raise HTTPException(422, f"Unknown ledger(s): {sorted(set(codes) - set(ids))}")
    total = round(sum(l.amount for l in body.lines), 2)
    have, now = accounting._columns("TrnHdrEXP"), datetime.now()
    with db.direct_writes():
        with db.transaction() as cur:
            no, print_no, _ = accounting.next_number(cur, "TrnHdrEXP", "EXPNO")
            hdr = {"EXPNO": no, "Prefix": "", "PrintEXPNO": print_no, "EXPDate": body.date.isoformat(), "EXPDateYear": date(body.date.year, 1, 1).isoformat(),
                   "EXPDateMonth": date(body.date.year, body.date.month, 1).isoformat(), "Authorised": "Y", "AuthBy": "", "ChangedBy": user, "Cancelled": "N",
                   "AddedBy": user, "AddedOn": now, "RMKS": " ", "AMTINWORDS": formulas.amount_in_words(total)[:250], "TOTAMT": total}
            eid = accounting._insert(cur, "TrnHdrEXP", {k: v for k, v in hdr.items() if k.lower() in have}, "EXPID")
            cur.execute("SELECT COLUMNPROPERTY(OBJECT_ID('TrnDtlEXP'), 'EXPDtlID', 'IsIdentity')")
            ident = cur.fetchone()[0] == 1
            for i, l in enumerate(body.lines, start=1):
                row = {"EXPID": eid, "SlNo": i, "ShortClosed": "N", "ShortClosedBy": 0, "ItemSlNO": i, "LEDID": ids[l.ledger], "AMT": l.amount,
                       "REMARKS": (l.narration or " ")[:250], "RAMT": 0}
                if not ident:
                    cur.execute("SELECT ISNULL(MAX(EXPDtlID), 0) + 1 FROM TrnDtlEXP WITH (UPDLOCK, HOLDLOCK)")
                    row["EXPDtlID"] = int(cur.fetchone()[0])
                accounting._insert(cur, "TrnDtlEXP", row)
    db_store.load_expenses()   # the list, the DSR and the petty cash report read it from memory
    return {"doc_no": print_no, "date": body.date.isoformat(), "paid_from": body.paid_from, "total": total,
            "lines": [{"ledger": l.ledger, "amount": l.amount, "narration": l.narration} for l in body.lines]}


def update_expense(doc_no: str, body, user: str) -> dict:
    """Edit a saved expense entry in place: its header (date, total, words) is updated and its lines rewritten, in one transaction. The receipt
    amounts (RAMT, money put into the petty cash fund) of the old lines are kept as they were. A cancelled entry or a date on or before the freeze date is refused."""
    accounting._need("TrnHdrEXP", "TrnDtlEXP", "MstLed")
    h = db.fetchone("SELECT EXPID, EXPDate, Cancelled FROM TrnHdrEXP WITH (NOLOCK) WHERE PrintEXPNO = ? OR EXPNO = ?", (doc_no, doc_no))
    if not h:
        raise HTTPException(404, f"Expense entry '{doc_no}' not found")
    if h.get("Cancelled") == "Y":
        raise HTTPException(409, "Current record is cancelled. Cannot edit!")   # frmTrnNonPartyNonItem 160
    gsettings.check_not_frozen(str(h["EXPDate"])[:10], "This expense entry")
    gsettings.check_not_frozen(body.date, "This expense entry")
    codes = sorted({l.ledger for l in body.lines})
    ids = {r["LEDCODE"].strip(): int(r["LEDID"]) for r in db.fetchall(f"SELECT LEDID, LEDCODE FROM MstLed WHERE LEDCODE IN ({','.join('?' * len(codes))})", tuple(codes))}
    if set(codes) - set(ids):
        raise HTTPException(422, f"Unknown ledger(s): {sorted(set(codes) - set(ids))}")
    eid = int(h["EXPID"])
    kept = [(int(r["LEDID"]), float(r["RAMT"] or 0), (r["REMARKS"] or "")) for r in db.fetchall("SELECT LEDID, RAMT, REMARKS FROM TrnDtlEXP WITH (NOLOCK) WHERE EXPID = ? AND RAMT > 0", (eid,))]
    total = round(sum(l.amount for l in body.lines), 2)
    have = accounting._columns("TrnHdrEXP")
    vals = {k: v for k, v in {"EXPDate": body.date.isoformat(), "EXPDateYear": date(body.date.year, 1, 1).isoformat(), "EXPDateMonth": date(body.date.year, body.date.month, 1).isoformat(),
                              "AMTINWORDS": formulas.amount_in_words(total)[:250], "TOTAMT": total, "ChangedBy": user, "ChangedOn": datetime.now()}.items() if k.lower() in have}
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE TrnHdrEXP SET {', '.join(f'{k} = ?' for k in vals)} WHERE EXPID = ? AND Cancelled <> 'Y'", (*vals.values(), eid))
            cur.execute("DELETE FROM TrnDtlEXP WHERE EXPID = ?", (eid,))
            cur.execute("SELECT COLUMNPROPERTY(OBJECT_ID('TrnDtlEXP'), 'EXPDtlID', 'IsIdentity')")
            ident = cur.fetchone()[0] == 1
            rows = [{"LEDID": ids[l.ledger], "AMT": l.amount, "REMARKS": (l.narration or " ")[:250], "RAMT": 0} for l in body.lines] + \
                   [{"LEDID": led, "AMT": 0, "REMARKS": (rem or " ")[:250], "RAMT": ramt} for led, ramt, rem in kept]
            for i, r in enumerate(rows, start=1):
                row = {"EXPID": eid, "SlNo": i, "ShortClosed": "N", "ShortClosedBy": 0, "ItemSlNO": i, **r}
                if not ident:
                    cur.execute("SELECT ISNULL(MAX(EXPDtlID), 0) + 1 FROM TrnDtlEXP WITH (UPDLOCK, HOLDLOCK)")
                    row["EXPDtlID"] = int(cur.fetchone()[0])
                accounting._insert(cur, "TrnDtlEXP", row)
    db_store.load_expenses()
    return {"doc_no": doc_no, "date": body.date.isoformat(), "paid_from": body.paid_from, "total": total,
            "lines": [{"ledger": l.ledger, "amount": l.amount, "narration": l.narration} for l in body.lines]}


def cancel_expense(doc_no: str, user: str) -> dict:
    """frmTrnNonPartyNonItem.deleteDoc with the CANCELLED choice: the header is marked Cancelled = 'Y' (an expense moves no stock). Refused while a
    stock re-write runs, when already cancelled ("Current record is cancelled. Deletion not possible!") and on or before the Freeze Date."""
    accounting._need("TrnHdrEXP", "TrnDtlEXP")
    if gsettings.rewrite_in_progress():
        raise HTTPException(409, "re-Writing Stocks.. Try after few mins.")
    h = db.fetchone("SELECT EXPID, EXPDate, Cancelled FROM TrnHdrEXP WITH (NOLOCK) WHERE PrintEXPNO = ? OR EXPNO = ?", (doc_no, doc_no))
    if not h:
        raise HTTPException(404, f"Expense entry '{doc_no}' not found")
    if h.get("Cancelled") == "Y":
        raise HTTPException(409, "Current record is cancelled. Deletion not possible!")
    gsettings.check_not_frozen(str(h["EXPDate"])[:10], "This expense entry")
    have = accounting._columns("TrnHdrEXP")
    vals = {k: v for k, v in {"Cancelled": "Y", "ChangedBy": user, "ChangedOn": datetime.now()}.items() if k.lower() in have}
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE TrnHdrEXP SET {', '.join(f'{k} = ?' for k in vals)} WHERE EXPID = ? AND Cancelled <> 'Y'", (*vals.values(), int(h["EXPID"])))
    db_store.load_expenses()
    return {"doc_no": doc_no, "status": "CANCELLED"}


def load_opening() -> None:
    """Day's OP-Balance of each date from TrnExpOPBal into memory (the DSR's cash opening)."""
    store.EXP_OPENING.clear()
    try:
        for r in db.fetchall("SELECT ExpDate, OpBalAmt FROM TrnExpOPBal WITH (NOLOCK)"):
            store.EXP_OPENING[str(r["ExpDate"])[:10]] = round(float(r["OpBalAmt"] or 0), 2)
    except Exception:  # noqa: BLE001 - a database without the table
        pass


def save_opening(day: date, amount: float) -> float:
    """Replace the date's row of TrnExpOPBal, as the exe does (delete, then insert)."""
    accounting._need("TrnExpOPBal")
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute("DELETE FROM TrnExpOPBal WHERE ExpDate = ?", (day.isoformat(),))
            cur.execute("INSERT INTO TrnExpOPBal (ExpDate, OpBalAmt) VALUES (?, ?)", (day.isoformat(), amount))
    store.EXP_OPENING[day.isoformat()] = round(amount, 2)
    return round(amount, 2)
