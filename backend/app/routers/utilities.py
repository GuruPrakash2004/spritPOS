"""The desktop UTILITIES menu (MstMenu.MenuItemType = 7) and About.

Built so far:
- Global Settings (702, FrmUtlGSettings): the settings grid (app/gsettings.py). Administrator only.
- Remove Duplicate Invoice (711): app/duplicate_invoices.py.
- Replace and Remove Duplicate items (712) and Merge Items (713): app/item_merge.py.
- Compare Stock (714): two stock sheets compared in the browser; this gives the web app's current stock.
- ADMIN Audit Log (frmRptAdminAuditLog): what was changed, by whom, when (app/admin_audit.py), plus the
  Tools runs (tool_audit). Administrator only.
- Web Sync (frmModWebSync): the day's summary and the stock sent to the central site (app/websync.py).
- About (frmUtlAbout).
The web app never writes the desktop tables; every utility acts on the web app's own data.
"""
import logging
import re
from datetime import date, datetime, timedelta
from typing import Literal

from fastapi import APIRouter, Depends, HTTPException, Query
from pydantic import BaseModel, Field

from .. import admin_audit, db, doc_settings, print_settings, duplicate_invoices, gsettings, item_merge, rpt_bridge, security, sqlstore, store, websync
from .auth import get_current_user, hides_cost, require_admin, require_module

router = APIRouter(prefix="/utilities", tags=["Utilities"])
logger = logging.getLogger("vlqms.utilities")
values_router = APIRouter(tags=["Global Settings"])

TOOLS_SCREENS = ("SQL Query Execution", "Re Write Stock")


# ─── Global Settings ─────────────────────────────────────────────────────────

class SettingUpdate(BaseModel):
    caption: str = Field(min_length=1, max_length=120)
    value: str = Field("", max_length=500)
    hidden: bool = False


class SettingNew(SettingUpdate):
    code: str = Field(min_length=2, max_length=40)
    group: str = Field(min_length=1, max_length=40)
    remarks: str = Field("", max_length=500)
    kind: str = "text"


@values_router.get("/settings/values")
def setting_values():
    """code -> value of every Global Setting row, for the screens (login only; no secrets and no server details)."""
    return gsettings.values()


@router.get("/settings")
def list_settings(all: bool = False, admin: dict = Depends(require_admin)):
    """The settings grid: group, code, caption, value, help text and what each changes in the web app.
    `all=true` also lists hidden rows (legacy Hide Settings)."""
    return gsettings.all_rows(include_hidden=all)


def _update_smtp_row(code: str, value: str, admin: dict) -> dict:
    """An SMTP row of the grid: written to backend/.env through the same path as the Email (SMTP) tab."""
    from pydantic import ValidationError
    from . import setup as setup_router
    fields = setup_router.smtp_current()
    password = ""
    if code == gsettings.SMTP_PASSWORD_CODE:
        password = value
    else:
        fields[gsettings.SMTP_FIELDS[code]] = value.strip()
    try:
        body = setup_router.SmtpIn(**fields, password=password)
    except ValidationError as exc:
        raise HTTPException(422, f"{exc.errors()[0]['loc'][-1]}: {exc.errors()[0]['msg']}")
    setup_router.save_smtp(body, str(admin["user"]))
    return next(r for r in gsettings.env_rows() if r["code"] == code)


@router.put("/settings/{code}")
def update_setting(code: str, body: SettingUpdate, admin: dict = Depends(require_admin)):
    row = store.GSETTINGS.get(code)
    if row is None:
        if code in gsettings.SMTP_FIELDS or code == gsettings.SMTP_PASSWORD_CODE:
            return _update_smtp_row(code, body.value, admin)
        raise HTTPException(404, f"Setting '{code}' not found")
    if code in gsettings._HIDDEN and not body.hidden:
        raise HTTPException(409, f"{code} is kept only for the older /settings API and stays hidden")
    new = {**row, "caption": body.caption.strip(), "value": gsettings.check_value(row["kind"], body.value, code),
           "hidden": body.hidden}
    earlier, changed = admin_audit.diff({k: row[k] for k in ("caption", "value", "hidden")},
                                        {k: new[k] for k in ("caption", "value", "hidden")})
    if changed:
        admin_audit.record("Global Settings", "Edited", code, earlier, changed)
    store.GSETTINGS[code] = new
    gsettings.sync_legacy()
    return dict(new, readonly=False)


@router.post("/settings", status_code=201)
def add_setting(body: SettingNew, admin: dict = Depends(require_admin)):
    """Add a setting of your own (legacy Fld Code / Fld Caption). The web app doesn't read these."""
    code = body.code.strip()
    if not gsettings.CODE_RE.match(code):
        raise HTTPException(422, "Fld Code: start with a letter; letters, digits and _ only")
    if code.lower() in {c.lower() for c in store.GSETTINGS} | {r["code"].lower() for r in gsettings.env_rows()}:
        raise HTTPException(409, "FldCode already exists...")
    if body.kind not in gsettings.KINDS:
        raise HTTPException(422, f"Type must be one of {', '.join(gsettings.KINDS)}")
    row = {"code": code, "group": body.group.strip(), "caption": body.caption.strip(),
           "value": gsettings.check_value(body.kind, body.value), "kind": body.kind, "remarks": body.remarks.strip(),
           "effect": None, "hidden": body.hidden, "source": "custom"}
    store.GSETTINGS[code] = row
    admin_audit.record("Global Settings", "Added", code, "", f"{row['group']}: {row['caption']} = {row['value']}")
    return dict(row, readonly=False)


# ─── ADMIN Audit Log ─────────────────────────────────────────────────────────

@router.get("/audit/screens")
def audit_screens(admin: dict = Depends(require_admin)):
    return sorted(set(sqlstore.audit_screens()) | set(TOOLS_SCREENS))


@router.get("/audit")
def audit_log(from_date: date = Query(default_factory=lambda: date.today() - timedelta(days=7)),
              to_date: date = Query(default_factory=date.today),
              screen: str | None = None, admin: dict = Depends(require_admin)):
    """Entries between the dates (inclusive), newest first, with item rows. Tools runs are included."""
    if to_date < from_date:
        raise HTTPException(422, "To date is before From date")
    lo, hi = f"{from_date.isoformat()}T00:00:00", f"{to_date.isoformat()}T23:59:59"
    rows = sqlstore.audit_rows(screen or None, lo, hi)
    if not screen or screen in TOOLS_SCREENS:
        for t in sqlstore.tool_audit_rows(screen or None, limit=5000):
            if lo <= t["at"] <= hi:
                rows.append({"id": f"T{t['id']}", "at": t["at"], "screen": t["tool"],
                             "action": "Run" if t["ok"] else "Refused / failed", "main_fld": "",
                             "user_code": t["user_code"], "earlier": "",
                             "changed": t["detail"] + (f"  [{t['message']}]" if t["message"] else ""), "items": []})
    rows.sort(key=lambda r: r["at"], reverse=True)
    return rows


# ─── About ───────────────────────────────────────────────────────────────────

def _connection_details() -> dict:
    """What an Administrator sees under About → Database: the settings in use (never the password) and, while
    connected, what SQL Server says about itself."""
    s = db.settings()
    out = {"server": s["host"], "port": s["port"] or "", "database": s["database"], "auth": "SQL login",
           "user": s["user"], "driver": db.driver_name(), "edition": "", "version": ""}
    if db.is_db_available():
        try:
            r = db.fetchone("SELECT CAST(SERVERPROPERTY('Edition') AS NVARCHAR(128)) AS edition, "
                            "CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(128)) AS version")
            out.update(edition=(r or {}).get("edition") or "", version=(r or {}).get("version") or "")
        except Exception:   # noqa: BLE001 - About must still open when the connection is on its way out
            pass
    return out


@router.get("/about")
def about(current_user: dict = Depends(get_current_user)):
    from ..main import app
    return {
        # The database name is shown to everyone who can open About; the server, port, login and driver only to an Administrator.
        "database": db.settings()["database"] if db.configured() else "",
        "connection": _connection_details() if current_user.get("role") == store.ADMIN_ROLE and db.configured() else None,
        "product": "e-vLQMS ERP",
        "software_version": gsettings.value("SWVersion"),
        "api_version": app.version,
        "company": store.COMPANY.get("name", ""),
        "db_connected": db.is_db_available(),
        "db_configured": db.configured(),
        "db_disconnected_for_testing": db.MANUAL_OFFLINE,
        # What the Doc Designer defined (print options, screens, fields) and whether Crystal printing is set up.
        "designer": {"screens": len(store.UTL_DOCS), "print_options": len(store.UTL_OPTIONS), "fields": len(store.UTL_FIELDS)},
        "rpt_bridge": rpt_bridge.configured(),
        "rpt_files": len(rpt_bridge.rpt_files()),
        "server_time": datetime.now().isoformat(timespec="seconds"),
    }


# ─── The database connection (Administrator only) ──────────────────────────────
# These change no shop data, so main.py runs them without the "database offline" check and saves nothing after.

@router.post("/database/disconnect")
def disconnect_database(admin: dict = Depends(require_admin)):
    """Close the SQL Server connection to test the app without it (the settings in .env are kept). Signed-in users
    stay signed in and see what is in memory; nothing can be saved and nobody new can sign in until Reconnect."""
    if not db.configured():
        raise HTTPException(409, "No database is set up")
    db.disconnect()
    logger.warning("Database disconnected for testing by %s", admin["user"])
    return about(admin)


@router.post("/database/reconnect")
def reconnect_database(admin: dict = Depends(require_admin)):
    """Connect again and load the shop's data."""
    if not db.configured():
        raise HTTPException(409, "No database is set up")
    if not db.reconnect():
        raise HTTPException(503, "Could not connect to the database. Check that SQL Server is running and try again.")
    sqlstore.reload()
    logger.warning("Database reconnected by %s", admin["user"])
    return about(admin)


# ─── Remove Duplicate Invoice (711) ──────────────────────────────────────────

class RemoveInvoicesIn(BaseModel):
    doc_nos: list[str] = Field(min_length=1, max_length=500)
    admin_password: str | None = Field(None, max_length=1000, description='Encrypted for "admin": the additional ADMIN password, or your own while none is set')


@router.get("/duplicate-invoices", dependencies=[Depends(require_module("Remove Duplicate Invoice"))])
def duplicate_invoice_list(from_date: date = Query(default_factory=date.today), to_date: date = Query(default_factory=date.today),
                           invoice_no: str | None = None, only_duplicates: bool = False):
    """Saved invoices (not cancelled or deleted), newest first; `duplicate_of` flags a possible duplicate."""
    if to_date < from_date:
        raise HTTPException(422, "To date is before From date")
    return duplicate_invoices.listing(from_date, to_date, invoice_no, only_duplicates)


@router.post("/duplicate-invoices/remove")
def remove_duplicate_invoices(body: RemoveInvoicesIn, user: dict = Depends(require_module("Remove Duplicate Invoice"))):
    security.check_admin_password(user, body.admin_password)
    return duplicate_invoices.remove(body.doc_nos)


# ─── Merge Items (713) / Replace and Remove Duplicate items (712) ─────────────

class MergeIn(BaseModel):
    from_item: str = Field(min_length=1, max_length=40)
    to_item: str = Field(min_length=1, max_length=40)
    admin_password: str | None = Field(None, max_length=1000, description='Encrypted for "admin" (see RemoveInvoicesIn)')


def _merge_mode(mode: str) -> str:
    if mode not in item_merge.SCREENS:
        raise HTTPException(404, "Unknown merge screen")
    return mode


def _can_merge(mode: str, user: dict) -> None:
    from .auth import can_use
    if not can_use(user, item_merge.SCREENS[mode]):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to use {item_merge.SCREENS[mode]}")


@router.get("/merge-items/{mode}/preview")
def merge_preview(mode: str, from_item: str, to_item: str,
                  user: dict = Depends(require_module(*item_merge.SCREENS.values()))):
    _can_merge(_merge_mode(mode), user)
    return item_merge.preview(from_item, to_item, mode)


@router.post("/merge-items/{mode}")
def merge_items(mode: str, body: MergeIn, user: dict = Depends(require_module(*item_merge.SCREENS.values()))):
    _can_merge(_merge_mode(mode), user)
    security.check_admin_password(user, body.admin_password)
    return item_merge.merge(body.from_item.strip(), body.to_item.strip(), mode)


# ─── Compare Stock (714) ─────────────────────────────────────────────────────

@router.get("/compare-stock/current")
def compare_stock_current(user: dict = Depends(require_module("Compare Stock"))):
    """The web app's stock now, as a stock sheet: code, name, closing qty and value (at purchase rate, or at
    MRP for roles that can't see costs)."""
    at_cost = not hides_cost(user)
    rows = [{"code": code, "name": it.get("name", ""), "qty": round(it.get("qoh", 0), 3),
             "value": round(it.get("qoh", 0) * ((it.get("purchase_rate") or 0) if at_cost else it.get("mrp", 0)), 2)}
            for code, it in sorted(store.ITEMS.items())]
    return {"basis": "purchase rate" if at_cost else "MRP", "rows": rows}


# ─── Global Settings → Other Settings (app/doc_settings.py) ───────────────────

class DocSettingsIn(BaseModel):
    after_save: Literal["none", "prompt", "preview"] = "none"
    default_format: str = Field("", max_length=60)
    allow_duplicate_items: bool = True
    allow_edit_date: bool = True
    edit_days: int = Field(0, ge=0, le=3650)
    allow_copy_from: bool = True
    update_rate_from_pb: bool = False


class NumberSeriesIn(BaseModel):
    prefix: str | None = Field(None, max_length=6)
    next_number: int | None = Field(None, ge=1, le=9_999_999)


class MenuLayoutIn(BaseModel):
    hidden: list[str] = Field(default_factory=list, max_length=200)
    codes: dict[str, str] = Field(default_factory=dict)


class ImportFieldsIn(BaseModel):
    fields: list[str] = Field(default_factory=list)


@values_router.get("/settings/documents")
def document_settings_for_screens():
    """Print and transaction settings per document, for the screens (login only)."""
    return doc_settings.all_settings()


@values_router.get("/settings/menu")
def menu_layout_for_screens():
    """Hidden modules and shortcut codes (Menu Sequence), for the sidebar and module search (login only)."""
    return store.MENU_LAYOUT


@router.get("/doc-settings")
def get_doc_settings(admin: dict = Depends(require_admin)):
    return doc_settings.all_settings()


@router.put("/doc-settings/{kind}")
def save_doc_settings(kind: str, body: DocSettingsIn, admin: dict = Depends(require_admin)):
    if kind not in doc_settings.KINDS:
        raise HTTPException(404, f"Unknown document '{kind}'")
    before = doc_settings.settings(kind)
    new = body.model_dump()
    if kind not in doc_settings.PRINTS:
        new.update(after_save="none", default_format="")
    if kind != "purchase_bill":
        new.update(allow_copy_from=True, update_rate_from_pb=False)
    if kind != "sales_invoice":
        new["edit_days"] = 0
    store.DOC_SETTINGS[kind] = new
    earlier, changed = admin_audit.diff(before, new)
    if changed:
        admin_audit.record("Global Settings", "Transaction Settings", doc_settings.KINDS[kind][0], earlier, changed)
    return doc_settings.all_settings()[kind]


@router.get("/numbering")
def get_numbering(admin: dict = Depends(require_admin)):
    return doc_settings.numbering()


@router.put("/numbering/{kind}")
def save_numbering(kind: str, body: NumberSeriesIn, admin: dict = Depends(require_admin)):
    before = next((r for r in doc_settings.numbering() if r["kind"] == kind), None)
    row = doc_settings.set_series(kind, body.prefix, body.next_number)
    admin_audit.record("Global Settings", "Document Number", row["screen"],
                       f"prefix {before['prefix']}, next {before['next_number']}" if before else "",
                       f"prefix {row['prefix']}, next {row['next_number']}")
    return row


@router.get("/menu")
def get_menu_layout(admin: dict = Depends(require_admin)):
    return store.MENU_LAYOUT


@router.put("/menu")
def save_menu_layout(body: MenuLayoutIn, admin: dict = Depends(require_admin)):
    if "Admin Master" in body.hidden:
        raise HTTPException(422, "Admin Master (Security) can't be hidden")
    codes = {m: c.strip() for m, c in body.codes.items() if c.strip()}
    bad = [c for c in codes.values() if not re.fullmatch(r"\d{1,6}", c)]
    if bad:
        raise HTTPException(422, f"Shortcut codes are 1 to 6 digits: {', '.join(bad)}")
    dup = sorted({c for c in codes.values() if list(codes.values()).count(c) > 1})
    if dup:
        raise HTTPException(422, f"Duplicate data for Shortcut Menu Name: {', '.join(dup)}")
    before = dict(store.MENU_LAYOUT)
    store.MENU_LAYOUT.update(hidden=sorted(set(body.hidden)), codes=codes)
    earlier, changed = admin_audit.diff({k: str(v) for k, v in before.items()}, {k: str(v) for k, v in store.MENU_LAYOUT.items()})
    if changed:
        admin_audit.record("Global Settings", "Menu Sequence", "Menu", earlier, changed)
    return store.MENU_LAYOUT


@router.get("/import-fields")
def get_import_fields(admin: dict = Depends(require_admin)):
    from ..schemas import Item
    return {"available": [f for f in Item.model_fields if f not in ("code", "qoh", "authorised")],
            "fields": store.IMPORT_FIELDS.get("fields", [])}


@router.put("/import-fields")
def save_import_fields(body: ImportFieldsIn, admin: dict = Depends(require_admin)):
    from ..schemas import Item
    available = [f for f in Item.model_fields if f not in ("code", "qoh", "authorised")]
    bad = [f for f in body.fields if f not in available]
    if bad:
        raise HTTPException(422, f"Unknown field(s): {', '.join(bad)}")
    before = store.IMPORT_FIELDS.get("fields", [])
    store.IMPORT_FIELDS["fields"] = sorted(set(body.fields))
    admin_audit.record("Global Settings", "Bulk Item Import Fields", "Item Master",
                       ", ".join(before) or "all fields", ", ".join(store.IMPORT_FIELDS["fields"]) or "all fields")
    return {"available": available, "fields": store.IMPORT_FIELDS["fields"]}



# ─── Print Settings (Other Settings → Print Settings) ───────────────────────────

class PrintOptionUpdate(BaseModel):
    name: str | None = Field(None, max_length=50)
    copies: int | None = None
    default: bool | None = None
    inactive: bool | None = None
    click: str | None = Field(None, max_length=1)
    ctrl_click: str | None = Field(None, max_length=1)
    ctrl_alt_click: str | None = Field(None, max_length=1)
    after_save: str | None = Field(None, max_length=3)
    printer: str | None = Field(None, max_length=50)


@router.get("/print-settings", dependencies=[Depends(require_admin)])
def print_settings_list():
    """Every document's print options: what a click does, the prompt after saving, printer, copies, default."""
    return print_settings.listing()


@router.put("/print-settings/{doc_id}/{order}")
def print_settings_save(doc_id: int, order: int, body: PrintOptionUpdate, admin: dict = Depends(require_admin)):
    return print_settings.update(doc_id, order, body.model_dump(exclude_unset=True), admin["user"])


# ─── Web Sync (frmModWebSync) ────────────────────────────────────────────────

class WebSyncIn(BaseModel):
    day: date | None = None   # blank: today


def _web_sync_status() -> dict:
    """Whether Web Sync is set up and how the last push went; the central site by host only, never the key."""
    return {"configured": websync.configured(), "central": websync.central_host(), "every_minutes": websync.minutes(),
            **websync.STATUS}


@router.get("/web-sync", dependencies=[Depends(require_module("Web Sync"))])
def web_sync_status():
    return _web_sync_status()


@router.post("/web-sync", dependencies=[Depends(require_module("Web Sync"))])
def web_sync_now(body: WebSyncIn):
    """Sync now: send that day's summary (today by default) and the current stock to the central site."""
    day = body.day or date.today()
    if day > date.today():
        raise HTTPException(422, "Choose today or an earlier day")
    try:
        websync.sync([day])
    except websync.SyncFailed as exc:
        raise HTTPException(502, str(exc)) from None
    return _web_sync_status()
