"""Utilities → Remove Duplicate Invoice (711, frmUtlDelDupInvoice).

The desktop screen lists saved invoices (not cancelled, not deleted) with Inv ID, Invoice No, Date, Added
By, Sales Type, Bill System, Tot Qty, Inv Value and Sales Counter, filtered by invoice number and dates;
the user ticks the ones to remove, confirms "Selected Invoice Will Delete.... Do you want to Continue...?",
and they are deleted (or, with KeepDelData, marked IsDelete = 'Y').

Here, on the web app's own bills only (bills loaded from the desktop database are listed but can't be
removed: the desktop tables are never written):
- each bill's stock goes back ("sale-cancel" movements, as a cancel) and its loyalty points are reversed;
- Global Settings KeepDelData = Y keeps it with status DELETED (out of every report and list of saved
  bills); N removes it from the web app's bills. Either way the ADMIN Audit Log keeps the bill and its
  lines, and write-back sends the DELETED status to SQL Server's Web_ tables;
- a bill with goods already returned, or dated on or before the Freeze Date, is refused;
- possible duplicates are flagged: same date, counter, amount and lines as an earlier bill within
  DUPLICATE_MINUTES.
"""
from collections import defaultdict
from datetime import date, datetime

from fastapi import HTTPException

from . import admin_audit, gsettings, store

DUPLICATE_MINUTES = 10


def _returned(doc_no: str) -> bool:
    return any(r.get("against_invoice") == doc_no for r in store.DOCS["sales_return"].values())


def _row(d: dict) -> dict:
    tot = d.get("totals") or {}
    return {
        "doc_no": d["doc_no"], "date": d["date"], "time": d.get("time", ""),
        "added_by": d.get("added_by") or d.get("salesperson") or "",
        "sales_type": d.get("payment_type", ""), "bill_system": "Desktop" if d.get("origin") else "Web",
        "qty": tot.get("qty", sum(l.get("qty", 0) for l in d.get("lines", []))),
        "value": tot.get("net", 0), "counter": d.get("counter", ""),
        "removable": not d.get("origin"), "duplicate_of": None,
        "why_not": "Desktop bill: remove it in the desktop app" if d.get("origin") else None,
    }


def _signature(d: dict) -> tuple:
    return (d["date"], d.get("counter"), round(float((d.get("totals") or {}).get("net", 0)), 2),
            tuple(sorted((l.get("item_code"), l.get("qty")) for l in d.get("lines", []))))


def _minutes(t: str) -> float | None:
    try:
        h, m, *s = (int(x) for x in t.split(":"))
        return h * 60 + m + (s[0] / 60 if s else 0)
    except (ValueError, AttributeError):
        return None


def listing(from_date: date, to_date: date, invoice_no: str | None = None, only_duplicates: bool = False) -> list[dict]:
    docs = [d for d in store.DOCS["sales_invoice"].values()
            if d.get("status") == "SAVED" and from_date.isoformat() <= d["date"] <= to_date.isoformat()]
    if invoice_no:
        q = invoice_no.strip().lower()
        docs = [d for d in docs if q in d["doc_no"].lower()]
    rows = {d["doc_no"]: _row(d) for d in docs}
    groups: dict[tuple, list[dict]] = defaultdict(list)
    for d in sorted(docs, key=lambda d: (d["date"], d.get("time", ""), d["doc_no"])):
        groups[_signature(d)].append(d)
    for same in groups.values():
        for earlier, later in zip(same, same[1:]):
            a, b = _minutes(earlier.get("time", "")), _minutes(later.get("time", ""))
            if a is not None and b is not None and 0 <= b - a <= DUPLICATE_MINUTES:
                rows[later["doc_no"]]["duplicate_of"] = earlier["doc_no"]
    out = sorted(rows.values(), key=lambda r: (r["date"], r["time"], r["doc_no"]), reverse=True)
    return [r for r in out if r["duplicate_of"]] if only_duplicates else out


def remove(doc_nos: list[str]) -> dict:
    """Remove the given web bills (all checked first: nothing is removed if one is refused)."""
    if not doc_nos:
        raise HTTPException(422, "Please select atleast one invoice...")
    problems, docs = [], []
    for no in dict.fromkeys(doc_nos):
        d = store.DOCS["sales_invoice"].get(no)
        if d is None:
            problems.append(f"{no}: Invalid Invoice...")
        elif d.get("origin"):
            problems.append(f"{no}: a desktop bill; remove it in the desktop app")
        elif d.get("status") != "SAVED":
            problems.append(f"{no}: it is {d.get('status', '').lower()}, not a saved bill")
        elif _returned(no):
            problems.append(f"{no}: goods from it were already returned (Sales Return)")
        else:
            try:
                gsettings.check_not_frozen(d["date"], f"Bill {no}")
            except HTTPException as e:
                problems.append(e.detail)
                continue
            docs.append(d)
    if problems:
        raise HTTPException(409, "; ".join(problems))

    keep = gsettings.yes("KeepDelData")
    for d in docs:
        no = d["doc_no"]
        for l in d["lines"]:
            store.move_stock(l["item_code"], l["qty"], "sale-cancel", no, date.fromisoformat(d["date"]), l["rate"])
        cust = store.CUSTOMERS.get(d.get("customer"))
        tot = d.get("totals") or {}
        if cust:
            cust["points"] = round(cust["points"] + tot.get("points_redeemed", 0) - tot.get("points_earned", 0), 2)
            store.touch("CUSTOMERS", cust["code"])
        d["status"] = "DELETED"
        d["deleted_at"] = datetime.now().isoformat(timespec="seconds")
        admin_audit.record("Remove Duplicate Invoice", "Deleted", no,
                           f"status: SAVED; date {d['date']} {d.get('time', '')}; counter {d.get('counter', '')}; net {tot.get('net', 0)}",
                           "status: DELETED" + ("" if keep else " (removed from the web app's bills)"),
                           admin_audit.doc_items(d, "Removed"))
        if not keep:
            del store.DOCS["sales_invoice"][no]
    return {"removed": [d["doc_no"] for d in docs], "kept_as_deleted": keep, "message": "Data Removed successfully!"}
