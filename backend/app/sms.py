"""SMS, the desktop's way (svtModule.SendSMS): the shop's providers are rows of UtlMsgSettings (Provider, DailyLimit, SMSUrl with <<MOBNO>> and
<<MSG>> in it, Def = Y for the default); a message is one HTTP GET to that URL and counts as sent when the answer says DONE; each sent message is
logged in TrnSMS (TrnDate, ProviderID, MobNo, MsgText, TrnDateTime) and the provider's daily limit counts those rows. The feature is on only when
UtlMisc.SMSFeature = 'Y'. Nothing is created in the database; a client without these tables simply has no SMS."""
from __future__ import annotations

import logging
import threading
from datetime import date, datetime
from urllib.parse import quote

import httpx

from . import db, store

logger = logging.getLogger("vlqms.sms")


def enabled() -> bool:
    return str(store.UTL_MISC.get("SMSFeature") or "").upper() == "Y" and db.is_db_available()


def providers() -> list[dict]:
    """[{id, name, daily_limit, default}] from UtlMsgSettings; [] when the table is missing."""
    if not db.is_db_available():
        return []
    try:
        rows = db.fetchall("SELECT RowID, Provider, DailyLimit, Def FROM dbo.UtlMsgSettings WITH (NOLOCK) ORDER BY RowID")
    except Exception:   # noqa: BLE001 - a client database without the table
        return []
    return [{"id": int(r["RowID"]), "name": (r["Provider"] or "").strip(), "daily_limit": int(r["DailyLimit"] or 0),
             "default": (r.get("Def") or "").strip().upper() == "Y"} for r in rows]


def _provider(name: str | None) -> tuple[dict, str] | None:
    """(provider, url) for the named provider, else the default one, else the first."""
    rows = providers()
    chosen = next((p for p in rows if name and p["name"].lower() == name.lower()), None) or next((p for p in rows if p["default"]), None) or (rows[0] if rows else None)
    if not chosen:
        return None
    r = db.fetchone("SELECT SMSUrl FROM dbo.UtlMsgSettings WITH (NOLOCK) WHERE RowID = ?", (chosen["id"],))
    return (chosen, (r or {}).get("SMSUrl") or "")


def send(message: str, mobile: str, provider: str | None = None) -> tuple[bool, str]:
    """Send one message. Returns (sent, reason): the reason says why not (feature off, no provider, daily limit, gateway answer)."""
    if not enabled():
        return False, "SMS is not switched on for this shop"
    mobile = "".join(ch for ch in (mobile or "") if ch.isdigit())[-10:]
    if len(mobile) != 10:
        return False, "Enter a 10 digit mobile number"
    found = _provider(provider)
    if not found or not found[1]:
        return False, "No SMS provider is set up"
    prov, url = found
    sent_today = db.fetchone("SELECT COUNT(RowID) AS n FROM dbo.TrnSMS WITH (NOLOCK) WHERE ProviderID = ? AND TrnDate = ?", (prov["id"], date.today()))
    if sent_today and int(sent_today["n"] or 0) > prov["daily_limit"]:
        return False, f"Daily SMS limit ({prov['daily_limit']}) has exceeded for Provider {prov['name']}. Cannot Send SMS."
    try:
        res = httpx.get(url.replace("<<MOBNO>>", mobile).replace("<<MSG>>", quote(message)), timeout=15)
        ok = "DONE" in res.text.upper()
    except Exception as e:   # noqa: BLE001 - the gateway is outside the shop's control
        logger.warning("SMS gateway failed: %s", e)
        return False, "The SMS gateway could not be reached"
    if not ok:
        return False, "The SMS gateway did not accept the message"
    with db.direct_writes():
        db.execute("INSERT INTO dbo.TrnSMS (TrnDate, ProviderID, MobNo, MsgText, TrnDateTime) VALUES (?, ?, ?, ?, ?)",
                   (date.today(), prov["id"], mobile, message[:250], datetime.now()))
    return True, ""


def send_later(message: str, mobile: str, provider: str | None = None) -> None:
    """Send in the background: billing never waits for a gateway."""
    if not enabled() or not mobile:
        return
    threading.Thread(target=lambda: send(message, mobile, provider), daemon=True).start()


def history(start: date, end: date) -> list[dict]:
    """The messages sent between two dates (TrnSMS joined to its provider), newest first."""
    if not db.is_db_available():
        return []
    try:
        rows = db.fetchall(
            "SELECT s.TrnDateTime, s.MobNo, s.MsgText, m.Provider FROM dbo.TrnSMS s WITH (NOLOCK) LEFT JOIN dbo.UtlMsgSettings m WITH (NOLOCK) ON m.RowID = s.ProviderID "
            "WHERE s.TrnDate BETWEEN ? AND ? ORDER BY s.TrnDateTime DESC", (start, end))
    except Exception:   # noqa: BLE001
        return []
    return [{"at": r["TrnDateTime"].isoformat(sep=" ", timespec="seconds") if r["TrnDateTime"] else "", "mobile": (r["MobNo"] or "").strip(),
             "message": r["MsgText"] or "", "provider": (r["Provider"] or "").strip()} for r in rows]


def loyalty_message(points: float, amount: float, redeemed: float = 0, balance: float = 0) -> list[str]:
    """The texts the desktop sends a loyalty customer after a bill (LoyaltySENDMAIL), with the company's name."""
    shop = store.COMPANY.get("name") or "us"
    out = []
    if points:
        out.append(f"You have accrued {int(points)} points for shopping at {shop}. Your total bill amount is INR Rs.{amount:g} and we thank you for your patronage.")
    if redeemed:
        out.append(f"Congratulations! You have redeemed {int(redeemed)} points, your balance is {int(balance)}. Thank you for your patronage. Regards, {shop}.")
    return out


# ─── Settings screens: frmUtlSendSMS (providers) and frmUtlInvSMS (invoice SMS) ───────────────────────────────

def provider_rows() -> list[dict]:
    """Every UtlMsgSettings row with its URL (the settings screen's grid): [{id, name, daily_limit, default, url}]."""
    if not db.is_db_available():
        return []
    try:
        rows = db.fetchall("SELECT RowID, Provider, DailyLimit, SMSUrl, Def FROM dbo.UtlMsgSettings WITH (NOLOCK) ORDER BY RowID")
    except Exception:   # noqa: BLE001 - a client database without the table
        return []
    return [{"id": int(r["RowID"]), "name": (r["Provider"] or "").strip(), "daily_limit": int(r["DailyLimit"] or 0),
             "default": (r.get("Def") or "").strip().upper() == "Y", "url": (r["SMSUrl"] or "").strip()} for r in rows]


def check_provider(name: str, daily_limit, url: str, row_id: int) -> tuple[str, int, str]:
    """The desktop's validFlds, in its order and with its messages."""
    from fastapi import HTTPException
    name, url = (name or "").strip(), (url or "").strip()
    if not name:
        raise HTTPException(422, "Please enter Provider!")
    if str(daily_limit if daily_limit is not None else "").strip() == "":
        raise HTTPException(422, "Please enter Daily Limit of messages!")
    if not url:
        raise HTTPException(422, "Please enter SMS URL!")
    if any(p["name"].lower() == name.lower() and p["id"] != row_id for p in provider_rows()):
        raise HTTPException(422, "Provider already exists..")
    try:
        limit = int(float(daily_limit))
    except (TypeError, ValueError):
        raise HTTPException(422, "Daily Limit of messages must be a number")
    if limit < 0 or limit > 1_000_000:
        raise HTTPException(422, "Daily Limit of messages must be from 0 to 1000000")
    return name, limit, url


def save_provider(row_id: int, name: str, daily_limit, url: str, default: bool) -> dict:
    """Add (row_id 0) or change a provider; a default one clears the others' flag (as the desktop's second UPDATE)."""
    name, limit, url = check_provider(name, daily_limit, url, row_id)
    with db.transaction() as cur:
        if row_id <= 0:
            row_id = int((db.fetchone("SELECT ISNULL(MAX(RowID), 0) + 1 AS n FROM dbo.UtlMsgSettings") or {"n": 1})["n"])
            cur.execute("INSERT INTO dbo.UtlMsgSettings (RowID, Provider, DailyLimit, SMSUrl, Def) VALUES (?, ?, ?, ?, ?)",
                        (row_id, name, limit, url, "Y" if default else "N"))
        else:
            cur.execute("UPDATE dbo.UtlMsgSettings SET Provider = ?, DailyLimit = ?, SMSUrl = ?, Def = ? WHERE RowID = ?",
                        (name, limit, url, "Y" if default else "N", row_id))
        if default:
            cur.execute("UPDATE dbo.UtlMsgSettings SET Def = 'N' WHERE RowID <> ?", (row_id,))
    return next(p for p in provider_rows() if p["id"] == row_id)


def delete_provider(row_id: int) -> None:
    with db.transaction() as cur:
        cur.execute("DELETE FROM dbo.UtlMsgSettings WHERE RowID = ?", (row_id,))


def test_message(row_id: int, mobile: str) -> tuple[bool, str]:
    """Test SMS panel (btnTestSMS_Click): the desktop's checks and message text, sent through that provider."""
    mobile = (mobile or "").strip()
    if not mobile:
        return False, "Mobile No. is Empty...."
    if not mobile.isdigit() or len(mobile) != 10:
        return False, "Invalid Mobile No..."
    prov = next((p for p in provider_rows() if p["id"] == row_id), None)
    if prov is None:
        return False, "Invalid Provider..."
    text = f"Test SMS\nFrom Provider: {prov['name']}\nSending Time:{datetime.now().strftime('%d-%b-%Y %I:%M:%S %p')}"
    ok, why = send(text, mobile, prov["name"])
    return ok, "Test Message Sent...." if ok else (why or "Failed to Sent Message...")


def invoice_settings() -> dict:
    """UtlMisc's invoice SMS columns (frmUtlInvSMS): when to message the owner, which numbers, the value limit and the three texts."""
    m = store.UTL_MISC

    def flag(col: str) -> bool:
        return str(m.get(col) or "").strip().upper() == "Y"

    return {"on_sales": flag("InvSMSblnSales"), "on_cancel": flag("InvSMSblnCancel"), "on_delete": flag("InvSMSblnDelete"),
            "mobiles": str(m.get("InvMsgMobNo") or "").strip(), "sales_value": float(m.get("InvSMSSalesvalue") or 0),
            "sales_msg": str(m.get("InvSMSSalesMSg") or ""), "cancel_msg": str(m.get("InvSMSCancelMsg") or ""),
            "delete_msg": str(m.get("InvSMSDeleteMsg") or ""), "available": "InvSMSblnSales" in m}


def save_invoice_settings(body: dict) -> dict:
    from fastapi import HTTPException
    if "InvSMSblnSales" not in store.UTL_MISC:
        raise HTTPException(409, "This database has no invoice SMS settings (UtlMisc.InvSMS*).")
    value = float(body.get("sales_value") or 0)
    if value < 0 or value > 1e9:
        raise HTTPException(422, "The sales value must be from 0 to 1000000000")
    mobiles = ",".join(x for x in (p.strip() for p in str(body.get("mobiles") or "").replace(";", ",").split(",")) if x)
    if any(not (x.isdigit() and len(x) == 10) for x in mobiles.split(",") if x):
        raise HTTPException(422, "Enter 10 digit mobile numbers, separated by commas")
    texts = {k: str(body.get(k) or "").replace("'", "`").strip() for k in ("sales_msg", "cancel_msg", "delete_msg")}
    if any(len(t) > 250 for t in texts.values()):
        raise HTTPException(422, "Each message is at most 250 characters")
    with db.transaction() as cur:
        cur.execute("UPDATE dbo.UtlMisc SET InvSMSblnSales = ?, InvSMSblnCancel = ?, InvSMSblnDelete = ?, InvMsgMobNo = ?, InvSMSSalesvalue = ?, "
                    "InvSMSSalesMSg = ?, InvSMSCancelMsg = ?, InvSMSDeleteMsg = ?",
                    ("Y" if body.get("on_sales") else "N", "Y" if body.get("on_cancel") else "N", "Y" if body.get("on_delete") else "N",
                     mobiles, value, texts["sales_msg"], texts["cancel_msg"], texts["delete_msg"]))
    from . import designer
    designer.load()   # re-read UtlMisc so the screens and the sender see the new values
    return invoice_settings()


# ─── Invoice SMS on cancel / delete (frmTrnInv / frmTrnPosInv getSMS + SendSMS) ───────────────────────────────

import re  # noqa: E402

_IDENT = re.compile(r"^[A-Za-z][A-Za-z0-9_]*$")


def _merge_fields(doc_no: str) -> dict[str, str]:
    """`<<COLUMN>>` values of a saved sales invoice, as the desktop's getFieldsFromTbl builds them: the TrnHdrSINV row, then the row of each master
    its ID columns point to (MstCust for CUSTID, ...), first value of a repeated column name wins. Empty without the database."""
    try:
        head = db.fetchone("SELECT * FROM dbo.TrnHdrSINV WITH (NOLOCK) WHERE SINVNO = ?", (doc_no,))
    except Exception:   # noqa: BLE001 - no such table / no database
        return {}
    if not head:
        return {}
    values: dict[str, str] = {}

    def add(row: dict) -> None:
        for col, v in row.items():
            if col.upper() in ("RGUID", "IMGPRINT"):
                continue
            values.setdefault(col.upper(), "" if v is None else str(v).strip())

    add(head)
    for col, v in list(head.items()):
        stem = col.upper()[:-2] if col.upper().endswith("ID") else ""
        if not stem or stem in ("SINV", "") or not v or not _IDENT.match(stem):
            continue
        try:
            master = db.fetchone(f"SELECT * FROM dbo.[Mst{stem}] WITH (NOLOCK) WHERE [{stem}ID] = ?", (v,))
        except Exception:   # noqa: BLE001 - not a master table
            continue
        if master:
            add(master)
    return values


def merge_text(template: str, values: dict[str, str]) -> str:
    """Replace each `<<COLUMN>>` (any case) with its value; unknown names stay as typed."""
    return re.sub(r"<<([A-Za-z0-9_]+)>>", lambda m: values.get(m.group(1).upper(), m.group(0)), template or "")


def invoice_event(kind: str, doc_no: str) -> int:
    """Never raises: a message that cannot be made or sent must not undo the cancel / delete it reports."""
    try:
        return _invoice_event(kind, doc_no)
    except Exception:   # noqa: BLE001
        logger.exception("Invoice SMS for %s %s failed", kind, doc_no)
        return 0


def _invoice_event(kind: str, doc_no: str) -> int:
    """Message the shop's own numbers about a cancelled ('cancel') or deleted ('delete') invoice, if Invoice SMS settings say so.
    The text is made now (the bill is still as the database holds it) and sent in the background. Returns how many numbers were messaged.
    Like the exe, a *sales* message is never sent: that code is commented out in the desktop's invoice forms."""
    if kind not in ("cancel", "delete") or not enabled():
        return 0
    s = invoice_settings()
    if not s["available"] or not s["on_cancel" if kind == "cancel" else "on_delete"]:
        return 0
    template = s["cancel_msg" if kind == "cancel" else "delete_msg"]
    text = merge_text(template, _merge_fields(doc_no)).strip() if template.strip() else ""
    if not text:
        return 0
    numbers = [m.strip() for m in s["mobiles"].split(",") if m.strip().isdigit() and len(m.strip()) == 10]
    for mobile in dict.fromkeys(numbers):
        send_later(text[:250], mobile)
    return len(set(numbers))
