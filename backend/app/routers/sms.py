"""SMS: the shop's providers and sent messages (the desktop's UtlMsgSettings / TrnSMS), and sending a message by hand (frmTrnSMS / frmUtlSendSMS).
See app/sms.py. The SMS screen's module is needed to send or to read the history."""
from datetime import date

from fastapi import APIRouter, Depends, HTTPException, Query
from pydantic import BaseModel, Field

from .. import sms
from .auth import get_current_user, require_module

router = APIRouter(prefix="/sms", tags=["SMS"])


class SmsIn(BaseModel):
    mobiles: list[str] = Field(min_length=1, max_length=200)
    message: str = Field(min_length=1, max_length=250)   # the desktop's message box takes 250 characters
    provider: str | None = None


@router.get("/providers")
def providers(current_user: dict = Depends(require_module("SMS"))):
    return {"enabled": sms.enabled(), "providers": sms.providers()}


@router.get("/history")
def history(start: date = Query(alias="from"), end: date = Query(alias="to"), current_user: dict = Depends(require_module("SMS"))):
    if end < start:
        raise HTTPException(422, "The To date is before the From date")
    return sms.history(start, end)


@router.post("/send")
def send(body: SmsIn, current_user: dict = Depends(require_module("SMS"))):
    """Send one message to each mobile; the answer says how many went and why the others did not."""
    sent, failed = 0, []
    for mobile in dict.fromkeys(body.mobiles):
        ok, why = sms.send(body.message, mobile, body.provider)
        if ok:
            sent += 1
        else:
            failed.append({"mobile": mobile, "reason": why})
    return {"sent": sent, "failed": failed}


# ─── Settings (frmUtlSendSMS provider settings, frmUtlInvSMS invoice SMS settings): Administrator only, the URLs hold gateway keys ───

from .. import admin_audit   # noqa: E402
from .auth import require_admin   # noqa: E402


class ProviderIn(BaseModel):
    name: str = Field("", max_length=100)
    daily_limit: int | str | None = None
    url: str = Field("", max_length=1000)
    default: bool = False


class TestIn(BaseModel):
    mobile: str = Field("", max_length=20)


class InvoiceSmsIn(BaseModel):
    on_sales: bool = False
    on_cancel: bool = False
    on_delete: bool = False
    mobiles: str = Field("", max_length=300)
    sales_value: float = 0
    sales_msg: str = Field("", max_length=250)
    cancel_msg: str = Field("", max_length=250)
    delete_msg: str = Field("", max_length=250)


@router.get("/settings/providers")
def settings_providers(admin: dict = Depends(require_admin)):
    return {"enabled": sms.enabled(), "providers": sms.provider_rows()}


@router.post("/settings/providers")
def settings_provider_add(body: ProviderIn, admin: dict = Depends(require_admin)):
    row = sms.save_provider(0, body.name, body.daily_limit, body.url, body.default)
    admin_audit.record("SMS", "Added", row["name"], "", f"daily limit {row['daily_limit']}" + ("; default" if row["default"] else ""))
    return row


@router.put("/settings/providers/{row_id}")
def settings_provider_save(row_id: int, body: ProviderIn, admin: dict = Depends(require_admin)):
    if not any(p["id"] == row_id for p in sms.provider_rows()):
        raise HTTPException(404, "That provider does not exist.")
    row = sms.save_provider(row_id, body.name, body.daily_limit, body.url, body.default)
    admin_audit.record("SMS", "Edited", row["name"], "", f"daily limit {row['daily_limit']}" + ("; default" if row["default"] else ""))
    return row


@router.delete("/settings/providers/{row_id}")
def settings_provider_delete(row_id: int, admin: dict = Depends(require_admin)):
    old = next((p for p in sms.provider_rows() if p["id"] == row_id), None)
    if old is None:
        raise HTTPException(404, "That provider does not exist.")
    sms.delete_provider(row_id)
    admin_audit.record("SMS", "Deleted", old["name"], "", "")
    return {"message": "Deleted"}


@router.post("/settings/providers/{row_id}/test")
def settings_provider_test(row_id: int, body: TestIn, admin: dict = Depends(require_admin)):
    ok, message = sms.test_message(row_id, body.mobile)
    return {"sent": ok, "message": message}


@router.get("/settings/invoice")
def settings_invoice(admin: dict = Depends(require_admin)):
    return sms.invoice_settings()


@router.put("/settings/invoice")
def settings_invoice_save(body: InvoiceSmsIn, admin: dict = Depends(require_admin)):
    before = sms.invoice_settings()
    now = sms.save_invoice_settings(body.model_dump())
    earlier, changed = admin_audit.diff({k: str(v) for k, v in before.items()}, {k: str(v) for k, v in now.items()})
    if changed:
        admin_audit.record("SMS", "Invoice SMS settings", "UtlMisc", earlier, changed)
    return now
