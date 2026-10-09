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

from .. import admin_audit, db, doc_engine, sqlstore
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
