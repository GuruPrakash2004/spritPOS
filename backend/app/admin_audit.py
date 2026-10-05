"""ADMIN Audit Log (legacy UtlAdmAudit / UtlAdmAuditItem, screen frmRptAdminAuditLog).

Endpoints call record() while they change data. The write middleware (main.py) calls begin() before the
request, then commit(user) when it succeeded (the entries move to store.AUDIT_OUT and sqlstore.save()
writes them in the same SQL Server transaction as the request's changes, into Web_UtlAdmAudit) or discard() when it failed, so a refused change
never appears in the log. Actions that run outside the middleware use write_now().

What is recorded, as in the desktop app: master rows added / edited (with each changed field, from and to) /
deleted; sales invoices saved, released, cancelled and removed, with their item lines; purchase bills; and
every Utilities action and Admin Master (Security) changes. The Tools audit (tool_audit) is shown in the same log screen.
Never recorded: passwords or hashes (FIELDS_NEVER_SHOWN), even as "changed".
"""
from datetime import datetime

from . import sqlstore, store

_pending: list[dict] = []
FIELDS_NEVER_SHOWN = {"password", "password_hash", "salt", "pwv", "legacy_password", "default_password", "iterations"}


def _now() -> str:
    return datetime.now().isoformat(timespec="seconds")


def begin() -> None:
    _pending.clear()


def discard() -> None:
    _pending.clear()


def commit(user: str | None) -> None:
    for e in _pending:
        e.setdefault("user", user)
        store.AUDIT_OUT.append(e)
    _pending.clear()


def record(screen: str, action: str, main_fld: str, earlier: str = "", changed: str = "",
           items: list[dict] | None = None, user: str | None = None) -> None:
    """One audit entry. items: [{"code", "name", "status": Added|Removed|Changed, "details"}]."""
    e = {"at": _now(), "screen": screen, "action": action, "main_fld": str(main_fld), "earlier": earlier,
         "changed": changed, "items": items or []}
    if user:
        e["user"] = user
    _pending.append(e)


def write_now(user: str, screen: str, action: str, main_fld: str, earlier: str = "", changed: str = "",
              items: list[dict] | None = None) -> None:
    sqlstore.add_audit([{"at": _now(), "screen": screen, "action": action, "main_fld": str(main_fld),
                            "earlier": earlier, "changed": changed, "items": items or [], "user": user}])


def _fmt(v) -> str:
    if isinstance(v, bool):
        return "Yes" if v else "No"
    if isinstance(v, float) and v.is_integer():
        return str(int(v))
    return "" if v is None else str(v)


def diff(before: dict | None, after: dict | None) -> tuple[str, str]:
    """(earlier, changed) text for the fields that differ, e.g. ("mrp: 850; name: Old", "mrp: 900; name: New")."""
    before, after = before or {}, after or {}
    keys = [k for k in dict.fromkeys([*before, *after]) if k not in FIELDS_NEVER_SHOWN
            and _fmt(before.get(k)) != _fmt(after.get(k))]
    return ("; ".join(f"{k}: {_fmt(before.get(k))}" for k in keys),
            "; ".join(f"{k}: {_fmt(after.get(k))}" for k in keys))


def doc_items(doc: dict, status: str) -> list[dict]:
    """Item rows for a document's lines (legacy UtlAdmAuditItem)."""
    out = []
    for ln in doc.get("lines", []):
        code = ln.get("item_code")
        if code is None:
            continue
        item = store.ITEMS.get(code, {})
        details = f"Qty {_fmt(ln.get('qty'))}"
        if ln.get("rate") is not None:
            details += f" @ {_fmt(ln.get('rate'))}"
        out.append({"code": code, "name": item.get("name", ""), "status": status, "details": details})
    return out
