"""Accounting, read from and written to the shop's own tables (nothing of the web app's own is kept in the database).

Vouchers (legacy frmTrnVch): Payment, Receipt, Credit Note, Debit Note, Contra, Journal. Each has a header TrnHdr<SH> and detail TrnDtl<SH> table
(identical in every client database); saving also posts the ledger entries the way the exe does: one TrnHdrAcc row (DocType = the short form, AccType
'V', TrnID = the voucher id) and a TrnDtlAcc row (AccID, AcLedID, DrAmt, CrAmt) per line. The exe refuses a voucher whose debit and credit totals
differ. Document numbers follow svtModule.TrnNoAutoGen / getFmtDocNo: the financial-year digits, the prefix, a five-digit running number
("2600115"), printed as 26/<prefix>/00115.

Reports: General Ledger (FrmRptAccGL), Party wise Outstanding (frmRptAccOutstanding), Bill wise Outstanding and Ageing (the database's own
procedure mSP_AcBillWisePendReport, which only builds session temp tables and selects).
Everything here writes inside `db.direct_writes()` in ONE transaction, so a failure leaves nothing half done.
"""
from datetime import date, datetime, timedelta

from fastapi import HTTPException

from . import db, formulas, gsettings, store

VOUCHERS = {
    "payment": ("PMTVCH", "Payment Voucher"), "receipt": ("RCPTVCH", "Receipt Voucher"), "credit-note": ("CRDNT", "Credit Note"),
    "debit-note": ("DBTNT", "Debit Note"), "contra": ("CNTVCH", "Contra Voucher"), "journal": ("JNRVCH", "Journal Voucher"),
}


def _kind(kind: str) -> tuple[str, str]:
    if kind not in VOUCHERS:
        raise HTTPException(404, f"Unknown voucher '{kind}'")
    return VOUCHERS[kind]


def _exists(table: str) -> bool:
    row = db.fetchone("SELECT OBJECT_ID(?, 'U') AS x", (f"dbo.{table}",))
    return bool(row and row.get("x"))


def _need(*tables: str) -> None:
    missing = [t for t in tables if not _exists(t)]
    if missing:
        raise HTTPException(404, "This database does not have the accounting tables: " + ", ".join(missing))


def _columns(table: str) -> set[str]:
    """The columns of a table that an INSERT may set: not computed (the year / month columns are in these databases) and not identity."""
    return {r["name"].lower() for r in db.fetchall(
        "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID(?) AND is_computed = 0 AND is_identity = 0 AND system_type_id <> 189", (f"dbo.{table}",))}


def money(v) -> float:
    return round(float(v or 0), 2)


# ─── Ledgers ───────────────────────────────────────────────────────────────────

def ledgers() -> list[dict]:
    """The account ledgers a voucher line can post to (MstAcLed: ledgers, suppliers and customers), with their group."""
    _need("MstAcLed")
    rows = db.fetchall("""SELECT a.AcLedID, a.AcLedName, a.LedType, ISNULL(g.LEDGROUPNAME, '') AS grp
                          FROM MstAcLed a WITH (NOLOCK) LEFT JOIN MstLedgroup g WITH (NOLOCK) ON g.LEDGROUPID = a.LedGroupId
                          WHERE a.AcLedID > 0 AND a.InActive = 'N' ORDER BY a.AcLedName""")
    kinds = {"L": "Ledger", "S": "Supplier", "C": "Customer", "J": "Other"}
    return [{"id": int(r["AcLedID"]), "name": (r["AcLedName"] or "").strip(), "type": kinds.get(r["LedType"], r["LedType"]), "group": (r["grp"] or "").strip()}
            for r in rows]


# ─── Vouchers ──────────────────────────────────────────────────────────────────

def _names(sh: str) -> dict:
    return {"hdr": f"TrnHdr{sh}", "dtl": f"TrnDtl{sh}", "id": f"{sh}ID", "no": f"{sh}NO", "print": f"Print{sh}NO", "date": f"{sh}Date", "amt": f"{sh}Amt"}


def list_vouchers(kind: str, first: date, last: date) -> list[dict]:
    sh, _ = _kind(kind)
    n = _names(sh)
    _need(n["hdr"], n["dtl"])
    rows = db.fetchall(f"""SELECT h.{n['id']} AS id, h.{n['print']} AS no, h.{n['date']} AS dt, h.NARR, h.TOTDR, h.Cancelled, h.AddedBy
                           FROM {n['hdr']} h WITH (NOLOCK)
                           WHERE h.{n['date']} >= ? AND h.{n['date']} < ? ORDER BY h.{n['id']} DESC""",
                       (first.isoformat(), (last + timedelta(days=1)).isoformat()))
    return [{"id": int(r["id"]), "doc_no": (r["no"] or "").strip(), "date": str(r["dt"])[:10], "narration": (r["NARR"] or "").strip(), "total": money(r["TOTDR"]),
             "status": "CANCELLED" if r.get("Cancelled") == "Y" else "SAVED", "added_by": (r["AddedBy"] or "").strip()} for r in rows]


def get_voucher(kind: str, voucher_id: int) -> dict:
    sh, title = _kind(kind)
    n = _names(sh)
    _need(n["hdr"], n["dtl"])
    h = db.fetchone(f"SELECT * FROM {n['hdr']} WITH (NOLOCK) WHERE {n['id']} = ?", (voucher_id,))
    if not h:
        raise HTTPException(404, f"{title} {voucher_id} not found")
    lines = db.fetchall(f"""SELECT d.SlNo, d.AcLedID, a.AcLedName, d.AmtType, d.DrAmt, d.CrAmt FROM {n['dtl']} d WITH (NOLOCK)
                            LEFT JOIN MstAcLed a WITH (NOLOCK) ON a.AcLedID = d.AcLedID WHERE d.{n['id']} = ? ORDER BY d.SlNo""", (voucher_id,))
    return {"id": voucher_id, "kind": kind, "title": title, "doc_no": (h.get(n["print"]) or "").strip(), "date": str(h.get(n["date"]))[:10],
            "narration": (h.get("NARR") or "").strip(), "cheque_no": (h.get("CHQNO") or "").strip(), "cheque_date": str(h.get("CHQDT"))[:10] if h.get("CHQDT") else "", "total": money(h.get("TOTDR")),
            "status": "CANCELLED" if h.get("Cancelled") == "Y" else "SAVED", "added_by": (h.get("AddedBy") or "").strip(),
            "lines": [{"ledger_id": int(r["AcLedID"] or 0), "ledger": (r["AcLedName"] or "").strip(), "side": (r["AmtType"] or "").strip(),
                       "amount": money(r["DrAmt"] if (r["AmtType"] or "").strip() == "Dr" else r["CrAmt"])} for r in lines]}


def next_number(cur, hdr: str, no_col: str, prefix: str = "") -> tuple[str, str, int]:
    """(stored number, printed number, sequence) for the next document of a series, as svtModule.TrnNoAutoGen: <year digits><prefix><00000>."""
    stem = f"{store.FY_PREFIX}{prefix}"
    cur.execute(f"SELECT ISNULL(MAX(CAST(SUBSTRING({no_col}, ?, 20) AS INT)), 0) FROM {hdr} WITH (UPDLOCK, HOLDLOCK) "
                f"WHERE {no_col} LIKE ? AND ISNUMERIC(SUBSTRING({no_col}, ?, 20)) = 1 AND LEN({no_col}) = ?",
                (len(stem) + 1, stem + "%", len(stem) + 1, len(stem) + 5))
    seq = int(cur.fetchone()[0]) + 1
    return f"{stem}{seq:05d}", f"{store.FY_PREFIX}/{prefix}/{seq:05d}", seq


def _insert(cur, table: str, values: dict, id_col: str | None = None) -> int | None:
    cols = list(values)
    sql = f"SET NOCOUNT ON; INSERT INTO {table} ({', '.join(cols)}) VALUES ({', '.join('?' * len(cols))})"
    if id_col:
        cur.execute(sql + "; SELECT CAST(SCOPE_IDENTITY() AS INT)", tuple(values.values()))
        return int(cur.fetchone()[0])
    cur.execute(sql, tuple(values.values()))
    return None


def _checked_totals(body) -> tuple[float, float]:
    """The rules of frmTrnVch's save, shared by a new and an edited voucher: known active ledgers, equal debit and credit, something entered."""
    if len(body.lines) < 2:
        raise HTTPException(422, "A voucher needs at least two lines (a debit and a credit)")
    ids = sorted({l.ledger_id for l in body.lines})
    found = {int(r["AcLedID"]) for r in db.fetchall(f"SELECT AcLedID FROM MstAcLed WHERE InActive = 'N' AND AcLedID IN ({','.join('?' * len(ids))})", tuple(ids))}
    if set(ids) - found:
        raise HTTPException(422, f"Unknown or inactive ledger id(s): {sorted(set(ids) - found)}")
    dr = round(sum(l.amount for l in body.lines if l.side == "Dr"), 2)
    cr = round(sum(l.amount for l in body.lines if l.side == "Cr"), 2)
    if dr != cr:
        raise HTTPException(422, "Debit and Credit totals are NOT matching!")
    if not dr:
        raise HTTPException(422, "Enter at least one debit and one credit")
    return dr, cr


def _header_values(sh: str, n: dict, body, dr: float, cr: float, have: set[str]) -> dict:
    """The header columns that change with the voucher's content (a new voucher adds its number, author and flags to these)."""
    hdr = {n["date"]: body.date.isoformat(), "NARR": body.narration[:250], "TOTDR": dr, "TOTCR": cr, "AMTINWORDS": formulas.amount_in_words(dr)[:250],
           "CHQNO": (body.cheque_no or "")[:10] if body.cheque_no else None, "CHQDT": body.cheque_date.isoformat() if body.cheque_no and body.cheque_date else None}
    if f"{sh}dateyear".lower() in have:
        hdr[f"{sh}DateYear"] = date(body.date.year, 1, 1).isoformat()
        hdr[f"{sh}DateMonth"] = date(body.date.year, body.date.month, 1).isoformat()
    return hdr


def _insert_lines(cur, n: dict, vid: int, body) -> None:
    for i, l in enumerate(body.lines, start=1):
        _insert(cur, n["dtl"], {n["id"]: vid, "SlNo": i, "AcLedID": l.ledger_id, n["amt"]: l.amount, "CrAmt": l.amount if l.side == "Cr" else 0,
                                "DrAmt": l.amount if l.side == "Dr" else 0, "AmtType": l.side, "AgRefAmt": 0, "NewRefAmt": 0})


def _insert_postings(cur, acc: int, body) -> None:
    for l in body.lines:
        _insert(cur, "TrnDtlAcc", {"AccID": acc, "AcLedID": l.ledger_id, "DrAmt": l.amount if l.side == "Dr" else 0, "CrAmt": l.amount if l.side == "Cr" else 0})


def create_voucher(kind: str, body, user: str) -> dict:
    """Save a voucher and post it to the ledgers, all or nothing. `body`: date, narration, cheque_no, cheque_date, lines [{ledger_id, side, amount}]."""
    sh, title = _kind(kind)
    n = _names(sh)
    _need(n["hdr"], n["dtl"], "TrnHdrAcc", "TrnDtlAcc", "MstAcLed")
    gsettings.check_not_frozen(body.date, f"This {title.lower()}")
    dr, cr = _checked_totals(body)
    now = datetime.now()
    have = _columns(n["hdr"])
    with db.direct_writes():
        with db.transaction() as cur:
            no, print_no, _ = next_number(cur, n["hdr"], n["no"])
            hdr = {n["no"]: no, "Prefix": "", n["print"]: print_no, "Authorised": "Y", "Cancelled": "N", "ChangedBy": user, "AddedBy": user, "AddedOn": now,
                   **{k: v for k, v in _header_values(sh, n, body, dr, cr, have).items() if v is not None}}
            vid = _insert(cur, n["hdr"], {k: v for k, v in hdr.items() if k.lower() in have}, n["id"])
            _insert_lines(cur, n, vid, body)
            acc = _insert(cur, "TrnHdrAcc", {"DocType": sh, "AccType": "V", "TrnID": vid, "TrnNo": print_no, "TrnDate": body.date.isoformat(),
                                             "DueDate": body.date.isoformat(), "Narrn": body.narration[:250]}, "AccID")
            _insert_postings(cur, acc, body)
    return get_voucher(kind, vid)


def _open_voucher(kind: str, voucher_id: int, what: str) -> tuple[str, str, dict, dict]:
    """The voucher to edit or cancel: it must exist and not be cancelled already; a date on or before the freeze date is refused (frmTrnVch)."""
    sh, title = _kind(kind)
    n = _names(sh)
    _need(n["hdr"], n["dtl"], "TrnHdrAcc", "TrnDtlAcc")
    h = db.fetchone(f"SELECT * FROM {n['hdr']} WITH (NOLOCK) WHERE {n['id']} = ?", (voucher_id,))
    if not h:
        raise HTTPException(404, f"{title} {voucher_id} not found")
    if h.get("Cancelled") == "Y":
        raise HTTPException(409, f"This {title.lower()} is cancelled; it can't be {what}")
    gsettings.check_not_frozen(str(h.get(n["date"]))[:10], f"This {title.lower()}")
    return sh, title, n, h


def update_voucher(kind: str, voucher_id: int, body, user: str) -> dict:
    """Edit a voucher: the header and lines are rewritten and its ledger postings replaced, in one transaction. The number stays."""
    sh, title, n, h = _open_voucher(kind, voucher_id, "edited")
    gsettings.check_not_frozen(body.date, f"This {title.lower()}")
    dr, cr = _checked_totals(body)
    have = _columns(n["hdr"])
    vals = {k: v for k, v in _header_values(sh, n, body, dr, cr, have).items() if k.lower() in have}
    if vals.get("CHQNO") is None and "chqno" in have:   # the cheque was cleared
        vals["CHQNO"] = ""
    if vals.get("CHQDT") is None:
        vals.pop("CHQDT", None)
    vals.update({"ChangedBy": user, "ChangedOn": datetime.now()})
    vals = {k: v for k, v in vals.items() if k.lower() in have}
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE {n['hdr']} SET {', '.join(f'{k} = ?' for k in vals)} WHERE {n['id']} = ? AND Cancelled <> 'Y'", (*vals.values(), voucher_id))
            cur.execute(f"DELETE FROM {n['dtl']} WHERE {n['id']} = ?", (voucher_id,))
            _insert_lines(cur, n, voucher_id, body)
            cur.execute("SELECT AccID FROM TrnHdrAcc WHERE TrnID = ? AND DocType = ? AND AccType = 'V'", (voucher_id, sh))
            row = cur.fetchone()
            if row:
                acc = int(row[0])
                cur.execute("UPDATE TrnHdrAcc SET TrnDate = ?, DueDate = ?, Narrn = ? WHERE AccID = ?", (body.date.isoformat(), body.date.isoformat(), body.narration[:250], acc))
                cur.execute("DELETE FROM TrnDtlAcc WHERE AccID = ?", (acc,))
            else:
                acc = _insert(cur, "TrnHdrAcc", {"DocType": sh, "AccType": "V", "TrnID": voucher_id, "TrnNo": (h.get(n["print"]) or "").strip(),
                                                 "TrnDate": body.date.isoformat(), "DueDate": body.date.isoformat(), "Narrn": body.narration[:250]}, "AccID")
            _insert_postings(cur, acc, body)
    return get_voucher(kind, voucher_id)


def cancel_voucher(kind: str, voucher_id: int, user: str) -> dict:
    """Cancel a voucher the way frmTrnVch does: the header is flagged Cancelled = 'Y'; nothing else is changed or removed."""
    sh, title, n, _ = _open_voucher(kind, voucher_id, "cancelled again")
    have = _columns(n["hdr"])
    sets = ["Cancelled = 'Y'"] + (["ChangedBy = ?", "ChangedOn = ?"] if {"changedby", "changedon"} <= have else [])
    args = (user, datetime.now()) if len(sets) > 1 else ()
    with db.direct_writes():
        with db.transaction() as cur:
            cur.execute(f"UPDATE {n['hdr']} SET {', '.join(sets)} WHERE {n['id']} = ? AND Cancelled <> 'Y'", (*args, voucher_id))
    return get_voucher(kind, voucher_id)


# ─── Reports ───────────────────────────────────────────────────────────────────

def general_ledger(ledger_id: int, first: date, last: date) -> dict:
    """FrmRptAccGL: the opening balance (everything posted before `first`), then each posting of the ledger up to `last`, with a running balance."""
    _need("TrnHdrAcc", "TrnDtlAcc", "MstAcLed")
    led = db.fetchone("SELECT AcLedName FROM MstAcLed WHERE AcLedID = ?", (ledger_id,))
    if not led or ledger_id <= 0:
        raise HTTPException(404, "Please select a valid Ledger!")
    op = db.fetchone("""SELECT SUM(d.DrAmt - d.CrAmt) AS net FROM TrnHdrAcc h WITH (NOLOCK) JOIN TrnDtlAcc d WITH (NOLOCK) ON d.AccID = h.AccID
                        WHERE d.AcLedID = ? AND h.TrnDate < ?""", (ledger_id, first.isoformat()))
    opening = money((op or {}).get("net"))
    rows = db.fetchall("""SELECT h.DocType, h.TrnDate, h.TrnNo, h.Narrn, d.DrAmt, d.CrAmt FROM TrnHdrAcc h WITH (NOLOCK)
                          JOIN TrnDtlAcc d WITH (NOLOCK) ON d.AccID = h.AccID
                          WHERE d.AcLedID = ? AND h.TrnDate >= ? AND h.TrnDate < ? ORDER BY h.TrnDate, h.AccID""",
                       (ledger_id, first.isoformat(), (last + timedelta(days=1)).isoformat()))
    out, bal = [{"doc_type": "OP", "date": first.isoformat(), "doc_no": "-", "narration": "Opening balance", "debit": opening if opening > 0 else 0.0, "credit": -opening if opening < 0 else 0.0,
                 "balance": opening}], opening
    for r in rows:
        bal = round(bal + money(r["DrAmt"]) - money(r["CrAmt"]), 2)
        out.append({"doc_type": (r["DocType"] or "").strip(), "date": str(r["TrnDate"])[:10], "doc_no": (r["TrnNo"] or "").strip(), "narration": (r["Narrn"] or "").strip(),
                    "debit": money(r["DrAmt"]), "credit": money(r["CrAmt"]), "balance": bal})
    return {"ledger": (led["AcLedName"] or "").strip(), "rows": out, "closing": bal}


def party_outstanding(party: str) -> list[dict]:
    """frmRptAccOutstanding: each customer (C) or supplier (S) whose postings do not net to zero, as a debit or a credit balance."""
    if party not in ("C", "S"):
        raise HTTPException(422, "party must be C (customers) or S (suppliers)")
    _need("TrnDtlAcc", "MstAcLed")
    sh = "Cust" if party == "C" else "Supp"
    rows = db.fetchall(f"""SELECT p.{sh}Code AS party, SUM(s.DrAmt - s.CrAmt) AS net FROM TrnDtlAcc s WITH (NOLOCK), MstAcLed a WITH (NOLOCK), Mst{sh} p WITH (NOLOCK)
                           WHERE s.AcLedID = a.AcLedID AND a.LedMstID = p.{sh}ID AND a.LedType = ? GROUP BY p.{sh}Code HAVING SUM(s.DrAmt - s.CrAmt) <> 0 ORDER BY 1""", (party,))
    return [{"party": (r["party"] or "").strip(), "debit": money(r["net"]) if r["net"] >= 0 else 0.0, "credit": -money(r["net"]) if r["net"] < 0 else 0.0} for r in rows]


def bill_wise(ac_type: str = "", ledger_ids: str = "") -> list[dict]:
    """Pending bills per party from the database's own mSP_AcBillWisePendReport. ac_type: R receivable (customers), P payable (suppliers), '' both."""
    if ac_type not in ("", "R", "P"):
        raise HTTPException(422, "ac_type must be R, P or empty")
    ids = [i for i in ledger_ids.split(",") if i.strip()]
    if not all(i.strip().isdigit() for i in ids):
        raise HTTPException(422, "ledgers must be a comma list of ledger ids")
    if not db.fetchone("SELECT OBJECT_ID('dbo.mSP_AcBillWisePendReport', 'P') AS x").get("x"):
        raise HTTPException(404, "This database does not have the bill-wise procedure (mSP_AcBillWisePendReport)")
    rows = db.fetchall("SET NOCOUNT ON; EXEC dbo.mSP_AcBillWisePendReport @AcType = ?, @AcLedgers = ?", (ac_type, ",".join(i.strip() for i in ids)))
    return [{"ledger": (r["AcLedName"] or "").strip(), "doc_type": (r["doctype"] or "").strip(), "doc_no": (r["trnno"] or "").strip(),
             "date": str(r["trndate"])[:10], "due_date": str(r["duedate"])[:10] if r.get("duedate") else "", "amount": money(r["docAmt"]),
             "adjusted": money(r["CumAdjamt"]), "balance": money(r["BalAmt"])} for r in rows]


BUCKETS = (("Not due", None, 0), ("1-30 days", 1, 30), ("31-60 days", 31, 60), ("61-90 days", 61, 90), ("Over 90 days", 91, None))


def ageing(ac_type: str = "", ledger_ids: str = "", as_on: date | None = None) -> list[dict]:
    """Bill wise Ageing: the pending bills of bill_wise() put in buckets by days past their due date (the bill date when there is none)."""
    as_on = as_on or date.today()
    per: dict[str, dict] = {}
    for b in bill_wise(ac_type, ledger_ids):
        due = date.fromisoformat((b["due_date"] or b["date"])[:10])
        days = (as_on - due).days
        row = per.setdefault(b["ledger"], {"ledger": b["ledger"], **{name: 0.0 for name, _, _ in BUCKETS}, "total": 0.0})
        for name, lo, hi in BUCKETS:
            if (lo is None or days >= lo) and (hi is None or days <= hi):
                row[name] = round(row[name] + b["balance"], 2)
                break
        row["total"] = round(row["total"] + b["balance"], 2)
    return sorted(per.values(), key=lambda r: r["ledger"])
