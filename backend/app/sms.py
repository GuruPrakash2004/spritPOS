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
        return False, f"Daily SMS limit ({prov['daily_limit']}) has exceeded for {prov['name']}"
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
                   (date.today(), prov["id"], mobile, message[:500], datetime.now()))
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
