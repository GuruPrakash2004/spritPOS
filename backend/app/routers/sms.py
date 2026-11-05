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
