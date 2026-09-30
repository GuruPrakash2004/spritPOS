"""Global Settings (legacy FrmUtlGSettings / table UtlGSettings): one row per setting, with a group, a code,
a caption, a value and help text. Utilities → Global Settings edits them.

Only settings the web app acts on are seeded here, each with `effect` saying what it changes (the rule for
this screen: no switch that does nothing). The desktop's other rows (colours, tooltips, exe path, SMS URL, ...)
are listed in frontend/LEGACY_PARITY.md instead. An Administrator may add rows of their own ("custom"), as in
the desktop screen; the web app doesn't read those.

Secrets are never kept here: the SMTP rows are read-only views of backend/.env (host, port, mailbox), and the
SMTP password is never shown. The older store.SETTINGS sections (loyalty, invoice), still served by
GET/PUT /settings, are kept in step with these rows by sync_legacy().
"""
import math
import os
import re
from datetime import date

from fastapi import HTTPException

from . import store

KINDS = ("yn", "number", "text", "date")

# code, group, caption, default value, kind, remarks (help text), effect in the web app.
_SEED = [
    ("SPID", "Invoice", "Sales Person required in invoice", "Y", "yn",
     "Y: a sales invoice can't be saved without a salesperson.",
     "Sales Invoice save refuses a bill without a salesperson."),
    ("QTYSELINV", "Invoice", "Qty: Manual Entry in Invoice", "Y", "yn",
     "Y: after scanning a barcode the cursor moves to Qty. N: the item is added at once with Qty 1.",
     "Sales Invoice scan box."),
    ("InvPreview", "Invoice", "Invoice Print Preview", "N", "yn",
     "Y: the print preview opens after every saved bill.",
     "Sales Invoice after Save."),
    ("KeepDelData", "Invoice", "Keep Deleted Data", "N", "yn",
     "Y: a bill removed with Remove Duplicate Invoice is kept with status DELETED. N: it is taken out of the "
     "web app's bills (the ADMIN Audit Log keeps it either way).",
     "Utilities → Remove Duplicate Invoice."),
    ("PointsForOneRs", "Loyalty", "Number of points for 1 Rs", "0.01", "number",
     "Loyalty points earned for each rupee of a customer's bill.",
     "Points earned on each saved bill."),
    ("MinBillForPoints", "Loyalty", "Minimum bill amount for points", "1000", "number",
     "A bill earns points only above this amount.",
     "Points earned on each saved bill."),
    ("FreezeDate", "Transaction", "Freeze Date", "", "date",
     "Documents dated on or before this date can't be added, released, cancelled or returned. Empty = no freeze.",
     "Every transaction save and cancel."),
    ("PurSellVarPer", "Company", "Purchase-Selling Rate variation %",
     "0", "number", "Enter % to which Sales Rate (MRP) has to exceed Purchase Rate. 0 = no check.",
     "Item Master save."),
    ("SWVersion", "Reports", "Software Version", "vLQMS Web", "text",
     "Shown on the About screen.", "About screen."),
    # Kept only so the older /settings API keeps its shape; hidden like a desktop HideSetting = 'Y' row.
    ("PointsAlertAbove", "Loyalty", "Alert when points above", "500", "number",
     "Older /settings field (loyalty.alert_when_points_above).", None),
]
_HIDDEN = {"PointsAlertAbove"}
# Highest value a number setting may take (the older /settings API caps points_per_rupee at 1 too).
MAX = {"PointsForOneRs": 1, "MinBillForPoints": 10_000_000, "PurSellVarPer": 1000, "PointsAlertAbove": 10_000_000}

# SMTP: read-only, from backend/.env (app/mailer.py). The password is never shown.
_ENV_ROWS = [
    ("SMTPHostName", "SMTP Host Name", "SMTP_HOST"),
    ("SMTPServerPort", "SMTP Server Port", "SMTP_PORT"),
    ("SMTPMailID", "SMTP Mail ID", "SMTP_USER"),
]

# Older store.SETTINGS[section][field] <-> row code, with the value type.
LEGACY_MAP = {
    ("loyalty", "points_per_rupee"): ("PointsForOneRs", float),
    ("loyalty", "alert_when_points_above"): ("PointsAlertAbove", float),
    ("loyalty", "min_bill_amount"): ("MinBillForPoints", float),
    ("invoice", "salesperson_required"): ("SPID", bool),
    ("invoice", "print_preview"): ("InvPreview", bool),
    ("invoice", "barcode_scan_jumps_to_qty"): ("QTYSELINV", bool),
}

CODE_RE = re.compile(r"^[A-Za-z][A-Za-z0-9_]{1,39}$")


def _row(code, group, caption, value, kind, remarks, effect, source="web") -> dict:
    return {"code": code, "group": group, "caption": caption, "value": value, "kind": kind, "remarks": remarks,
            "effect": effect, "hidden": code in _HIDDEN, "source": source}


def restore(saved: dict) -> None:
    """Lay saved rows over the seeded ones. For seeded rows only the value, caption and hidden flag are taken
    (what a row means and does comes from this file, so a changed or dropped seeded row isn't brought back);
    custom rows come back whole. Saved values that no longer pass the checks are ignored."""
    seeded = {s[0] for s in _SEED}
    for code, row in saved.items():
        if row.get("source") == "custom":
            if code not in store.GSETTINGS:
                store.GSETTINGS[code] = row
        elif code in seeded:
            cur = store.GSETTINGS[code]
            try:
                cur["value"] = check_value(cur["kind"], str(row.get("value", cur["value"])), code)
            except HTTPException:
                pass
            cur["caption"] = str(row.get("caption") or cur["caption"])
            cur["hidden"] = bool(row.get("hidden", cur["hidden"])) or code in _HIDDEN
    sync_legacy()


def seed() -> None:
    """Fill store.GSETTINGS with the seeded rows that aren't there yet (values already set are kept)."""
    for s in _SEED:
        store.GSETTINGS.setdefault(s[0], _row(*s))
    sync_legacy()


def _bool(v: str) -> bool:
    return str(v).strip().upper() == "Y"


def value(code: str, default: str = "") -> str:
    row = store.GSETTINGS.get(code)
    return default if row is None else str(row["value"])


def yes(code: str) -> bool:
    return _bool(value(code))


def number(code: str, default: float = 0.0) -> float:
    try:
        return float(value(code) or default)
    except ValueError:
        return default


def freeze_date() -> date | None:
    v = value("FreezeDate").strip()
    try:
        return date.fromisoformat(v) if v else None
    except ValueError:
        return None


def check_not_frozen(on: date | str, what: str = "This document") -> None:
    """409 if `on` is on or before the FreezeDate setting (legacy UtlGSettings FreezeDate)."""
    frozen = freeze_date()
    if frozen is None:
        return
    d = date.fromisoformat(str(on)[:10]) if not isinstance(on, date) else on
    if d <= frozen:
        raise HTTPException(409, f"{what} is dated {d:%d-%m-%Y}, on or before the freeze date {frozen:%d-%m-%Y} "
                                 "(Utilities → Global Settings → FreezeDate), so it can't be saved or changed.")


def check_value(kind: str, v: str, code: str | None = None) -> str:
    """The value, cleaned, or 422 with what is wrong."""
    v = (v or "").strip()
    if kind == "yn":
        if v.upper() not in ("Y", "N"):
            raise HTTPException(422, "Enter Y or N")
        return v.upper()
    if kind == "number":
        try:
            n = float(v or 0)
        except ValueError:
            raise HTTPException(422, "Enter a number")
        if not math.isfinite(n):
            raise HTTPException(422, "Enter a number")
        if n < 0:
            raise HTTPException(422, "The number can't be negative")
        if code in MAX and n > MAX[code]:
            raise HTTPException(422, f"At most {MAX[code]:g}")
        return v or "0"
    if kind == "date":
        if not v:
            return ""
        try:
            date.fromisoformat(v)
        except ValueError:
            raise HTTPException(422, "Enter a date as YYYY-MM-DD, or leave it empty")
        return v
    if len(v) > 500:
        raise HTTPException(422, "At most 500 characters")
    return v


def env_rows() -> list[dict]:
    rows = []
    for code, caption, key in _ENV_ROWS:
        v = os.environ.get(key, "").strip()
        if key == "SMTP_PORT" and not v and os.environ.get("SMTP_HOST"):
            v = "587"
        rows.append({"code": code, "group": "SMTP", "caption": caption, "value": v, "kind": "text",
                     "remarks": "Set in backend/.env on the server (SMTP password is never shown here).",
                     "effect": "Customised Reports → Email.", "hidden": False, "source": "env", "readonly": True})
    return rows


def all_rows(include_hidden: bool = False) -> list[dict]:
    rows = [dict(r, readonly=False) for r in store.GSETTINGS.values() if include_hidden or not r["hidden"]]
    rows += env_rows()
    return sorted(rows, key=lambda r: (r["group"].lower(), r["code"].lower()))


def values() -> dict[str, str]:
    """code -> value for the screens (any logged-in user): the settings rows only, not the server's SMTP
    details from .env."""
    return {code: r["value"] for code, r in store.GSETTINGS.items()}


def sync_legacy() -> None:
    """Refresh store.SETTINGS (the older /settings shape) from the rows."""
    for (section, field), (code, typ) in LEGACY_MAP.items():
        if code in store.GSETTINGS:
            v = store.GSETTINGS[code]["value"]
            store.SETTINGS.setdefault(section, {})[field] = _bool(v) if typ is bool else number(code)


def set_from_legacy(sections: dict) -> None:
    """PUT /settings or an older saved file: write store.SETTINGS-shaped values into the rows (numbers go
    through check_value, so 422 for a non-finite or too large value)."""
    for section, fields in sections.items():
        for field, v in (fields or {}).items():
            code_typ = LEGACY_MAP.get((section, field))
            if not code_typ or code_typ[0] not in store.GSETTINGS:
                continue
            code, typ = code_typ
            store.GSETTINGS[code]["value"] = ("Y" if v else "N") if typ is bool else check_value("number", str(v), code)
    sync_legacy()


seed()
