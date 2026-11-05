"""The extra records of a customer or supplier that vLQMS.exe keeps beside the master (frmMstPartyContactPerson,
frmMstPartyAddress, frmMstPartyAlert, frmMstPartyActivities):

- contacts    MstPartyContacts      contact persons (name, designation, mobile, phone, fax, e-mail, department, remarks, default)
- addresses   MstPartyAddress       other / shipping addresses (category, address 1-4, city, pin, state, country, default shipping)
- alerts      MstPartyAlertMessage  messages shown when the party is picked on a bill
- activities  MstPartyActivity      dated notes with the user who wrote them (added, never edited)

The rows belong to the party through `PartyID` (its id in MstCUST / MstSUPP) and the party master's `DocID`. Reading is open to
anyone who may see the master; changing needs the master's Edit right. Contacts, addresses and alerts are replaced as a list
(the desktop adds, edits and deletes row by row; the result is the same); activities are only added.
"""
from datetime import datetime

from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel, Field

from .. import admin_audit, db, doc_engine, sqlstore, store
from .auth import get_current_user, require_right

router = APIRouter(prefix="/masters", tags=["Party records"])

PARTIES = {"customers": ("MstCUST", "CUSTID", "CUSTCODE", "Customer Master"),
           "suppliers": ("MstSUPP", "SUPPID", "SUPPCODE", "Supplier Master")}

# kind -> table, id column, the columns people edit (name, kind, width)
KINDS = {
    "contacts": ("MstPartyContacts", "PartyCID", [("CName", "text", 150), ("CDesig", "text", 150), ("CMobNo", "text", 100), ("CPhone", "text", 100),
                                                  ("Fax", "text", 50), ("Email", "text", 50), ("CDepartment", "text", 150), ("CRemarks", "text", 250),
                                                  ("SetDefault", "yn", 1)]),
    "addresses": ("MstPartyAddress", "PartyAddID", [("AddCat", "text", 100), ("Add1", "text", 100), ("Add2", "text", 100), ("Add3", "text", 100),
                                                    ("Add4", "text", 100), ("City", "text", 50), ("PinCode", "text", 6), ("State", "text", 50),
                                                    ("Country", "text", 50), ("DefShipAdd", "yn", 1)]),
    "alerts": ("MstPartyAlertMessage", None, [("Message", "text", 250)]),
    "activities": ("MstPartyActivity", "ActID", [("Notes", "text", 250)]),
}


class RowsIn(BaseModel):
    rows: list[dict] = Field(default_factory=list, max_length=200)


def _party(kind_of_party: str, code: str) -> tuple[int, int]:
    """(the party's id, the party master's DocID)."""
    if kind_of_party not in PARTIES:
        raise HTTPException(404, "Not a customer or supplier")
    table, id_col, code_col, master_name = PARTIES[kind_of_party]
    if not sqlstore.enabled():
        raise HTTPException(503, "The database is offline")
    rows = db.fetchall(f"SELECT TOP 1 [{id_col}] AS i FROM dbo.[{table}] WHERE [{code_col}] = ?", (code,))
    if not rows:
        raise HTTPException(404, f"'{code}' is not in the database yet")
    return int(rows[0]["i"]), int(doc_engine.doc_id_of(master_name) or 0)


def _clean(kind: str, row: dict) -> dict:
    out = {}
    for name, typ, width in KINDS[kind][2]:
        v = row.get(name)
        if typ == "yn":
            out[name] = "Y" if v in (True, "Y", "y", 1, "true") else "N"
        else:
            out[name] = ("" if v is None else str(v).strip())[:width]
    return out


def _check(kind: str, row: dict) -> None:
    need = {"contacts": ("CName", "Contact person name"), "addresses": ("AddCat", "Address category"), "alerts": ("Message", "Message"),
            "activities": ("Notes", "Notes")}[kind]
    if not row[need[0]]:
        raise HTTPException(422, f"Enter the {need[1]}")


@router.get("/{party}/{code}/{kind}")
def list_rows(party: str, code: str, kind: str, user: dict = Depends(get_current_user)):
    if kind == "item-rates":   # Customer vs Item / Supplier vs Item
        return _rates_view(party, code, user)
    if kind not in KINDS:
        raise HTTPException(404, "Unknown list")
    party_id, doc_id = _party(party, code)
    table, id_col, columns = KINDS[kind]
    cols = ", ".join(f"[{c}]" for c, _, _ in columns) + (", [ActDate], [UserId]" if kind == "activities" else "")
    key = "PartyId" if kind in ("alerts", "activities") else "PartyID"
    rows = db.fetchall(f"SELECT {cols} FROM dbo.[{table}] WITH (NOLOCK) WHERE [{key}] = ?" + (f" ORDER BY [{id_col}]" if id_col else ""), (party_id,))
    return {"columns": [{"key": c, "type": t, "width": w} for c, t, w in columns], "rows": [
        {**{c: (str(r[c]).strip() == "Y" if t == "yn" else (r[c] if r[c] is not None else "")) for c, t, _ in columns},
         **({"ActDate": str(r["ActDate"])[:16], "UserId": r["UserId"]} if kind == "activities" else {})} for r in rows]}


@router.put("/{party}/{code}/{kind}")
def replace_rows(party: str, code: str, kind: str, body: RowsIn, user: dict = Depends(get_current_user)):
    if kind == "item-rates":
        try:
            items = PartyItemsIn(rows=[PartyItem(**r) for r in body.rows])
        except ValueError as e:   # pydantic's ValidationError is a ValueError
            raise HTTPException(422, f"Check the item rows: {e.errors()[0]['loc'][-1]} {e.errors()[0]['msg']}" if hasattr(e, "errors") else "Check the item rows")
        save_party_items(party, code, items, user)
        return _rates_view(party, code, user)
    if kind not in ("contacts", "addresses", "alerts"):
        raise HTTPException(404, "Unknown list")
    master = PARTIES.get(party, ("", "", "", ""))[3]
    require_right("edit", master)(user)
    party_id, doc_id = _party(party, code)
    table, id_col, _ = KINDS[kind]
    rows = [_clean(kind, r) for r in body.rows]
    for r in rows:
        _check(kind, r)
    key = "PartyId" if kind == "alerts" else "PartyID"
    with db.transaction() as cur:
        cur.execute(f"DELETE FROM dbo.[{table}] WHERE [{key}] = ?", (party_id,))
        for r in rows:
            extra = {"MstDocID": doc_id, key: party_id}
            if kind == "contacts":
                extra["CDispName"] = r["CName"] + (f" - {r['CDesig']}" if r["CDesig"] else "")
            if kind == "alerts":
                extra["PartyDocId"] = doc_id
            sqlstore._insert(cur, table, {**r, **extra})
    admin_audit.record(master, "Edited", code, "", f"{kind}: {len(rows)}")
    return list_rows(party, code, kind, user)


@router.post("/{party}/{code}/activities", status_code=201)
def add_activity(party: str, code: str, body: RowsIn, user: dict = Depends(get_current_user)):
    """A dated note on the party, with the signed-in user (vLQMS.exe frmMstPartyActivities)."""
    master = PARTIES.get(party, ("", "", "", ""))[3]
    require_right("edit", master)(user)
    party_id, doc_id = _party(party, code)
    note = _clean("activities", body.rows[0] if body.rows else {})
    _check("activities", note)
    user_id = db.fetchall("SELECT TOP 1 UserID FROM dbo.MstUser WHERE UserCode = ?", (user.get("user"),))
    with db.transaction() as cur:
        sqlstore._insert(cur, "MstPartyActivity", {**note, "PartyId": party_id, "PartyDocId": doc_id,
                                                  "UserId": int(user_id[0]["UserID"]) if user_id else 0, "ActDate": datetime.now()})
    return list_rows(party, code, "activities", user)


# ---- Customer vs Item / Supplier vs Item (frmPartyItem): the items a party has its own rate for -----------------------------------------
VS_ITEM_MODULES = {"customers": "Customer Vs Item Master", "suppliers": "Supplier Vs Item Master"}
ITEM_TABLES = {"customers": ("MstCUSTITEM", "CUSTITEM", "CustID"), "suppliers": ("MstSUPPITEM", "SUPPITEM", "SuppID")}


class PartyItem(BaseModel):
    item_code: str
    rate: float = Field(0, ge=0)
    eff_from: str | None = None
    eff_to: str | None = None
    remarks: str = Field("", max_length=250)


class PartyItemsIn(BaseModel):
    rows: list[PartyItem] = Field(default_factory=list, max_length=2000)


def party_items(party: str, code: str, user: dict = Depends(get_current_user)):
    """[{item_code, item_name, rate, eff_from, eff_to, remarks}] for the party."""
    party_id, _doc = _party(party, code)
    table, stem, key = ITEM_TABLES[party]
    by_id = {str(i.get("_itemid")): c for c, i in store.ITEMS.items()}
    rows = db.fetchall(f"SELECT ItemID, Rate, EffDateFrom, EffDateTo, RMKS FROM dbo.[{table}] WITH (NOLOCK) WHERE [{key}] = ? ORDER BY [{stem}ID]", (party_id,))
    out = []
    for r in rows:
        c = by_id.get(str(r["ItemID"]))
        if c:
            out.append({"item_code": c, "item_name": store.ITEMS[c]["name"], "rate": float(r["Rate"] or 0),
                        "eff_from": str(r["EffDateFrom"])[:10] if r["EffDateFrom"] else None, "eff_to": str(r["EffDateTo"])[:10] if r["EffDateTo"] else None,
                        "remarks": (r["RMKS"] or "").strip()})
    return out


def save_party_items(party: str, code: str, body: PartyItemsIn, user: dict = Depends(get_current_user)):
    """Replace the party's item rates (the desktop deletes the party's rows and writes the list again)."""
    master = PARTIES.get(party, ("", "", "", ""))[3]
    require_right("edit", master, VS_ITEM_MODULES.get(party, master))(user)   # the party master's right, or the Customer / Supplier Vs Item Master screen's
    party_id, _doc = _party(party, code)
    table, stem, key = ITEM_TABLES[party]
    seen = set()
    for r in body.rows:
        item = store.ITEMS.get(r.item_code)
        if not item or not item.get("_itemid"):
            raise HTTPException(422, f"Unknown item '{r.item_code}'")
        if r.item_code in seen:
            raise HTTPException(422, f"Item '{r.item_code}' is on the list twice")
        seen.add(r.item_code)
        if r.eff_from and r.eff_to and r.eff_to < r.eff_from:
            raise HTTPException(422, f"'{r.item_code}': the To date is before the From date")
    with db.transaction() as cur:
        cur.execute(f"DELETE FROM dbo.[{table}] WHERE [{key}] = ?", (party_id,))
        for r in body.rows:
            item = store.ITEMS[r.item_code]
            uom = db.fetchall("SELECT TOP 1 UOMID FROM dbo.MstUOM WHERE UOMCODE = ?", (item.get("uom"),))
            sqlstore._insert(cur, table, {f"{stem}CODE": item["code"], f"{stem}NAME": item["name"], "ItemID": int(item["_itemid"]), key: party_id,
                                          "Rate": r.rate, "EffDateFrom": r.eff_from or None, "EffDateTo": r.eff_to or None,
                                          "UOMID": int(uom[0]["UOMID"]) if uom else None, "RMKS": r.remarks, "InActive": "N",
                                          "AddedBy": user.get("user") or "", "AddedOn": datetime.now()})
    admin_audit.record(master, "Edited", code, "", f"item rates: {len(body.rows)}")
    return party_items(party, code, user)


def _rates_view(party: str, code: str, user: dict) -> dict:
    """party_items in the {columns, rows} shape the Records window shows for every other list."""
    cols = [("item_code", "text", 50), ("rate", "text", 12), ("eff_from", "text", 10), ("eff_to", "text", 10), ("remarks", "text", 250)]
    return {"columns": [{"key": k, "type": t, "width": w} for k, t, w in cols],
            "rows": [{**{k: ("" if r.get(k) is None else r[k]) for k, _t, _w in cols}, "item_name": r.get("item_name", "")} for r in party_items(party, code, user)]}
