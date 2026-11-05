"""Accounting endpoints under /accounting: the six vouchers, the ledger list and the ledger / outstanding / bill-wise reports.
Logic and the table rules are in app/accounting.py; every table is the shop's own (nothing of the web app's is stored)."""
from datetime import date
from typing import Literal

from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel, Field

from .. import accounting
from .auth import can_use, get_current_user, has_right

router = APIRouter(prefix="/accounting", tags=["Accounting"])


class VoucherLine(BaseModel):
    ledger_id: int = Field(gt=0)
    side: Literal["Dr", "Cr"]
    amount: float = Field(gt=0, le=1_000_000_000)


class VoucherIn(BaseModel):
    date: date
    narration: str = Field("", max_length=250)
    cheque_no: str = Field("", max_length=10)
    cheque_date: date | None = None
    lines: list[VoucherLine] = Field(min_length=2, max_length=200)


def _title(kind: str) -> str:
    return accounting._kind(kind)[1]


@router.get("/ledgers", dependencies=[Depends(get_current_user)])
def ledgers():
    return accounting.ledgers()


@router.get("/vouchers/{kind}")
def list_vouchers(kind: str, from_date: date, to_date: date, user: dict = Depends(get_current_user)):
    _require(user, _title(kind))
    if to_date < from_date:
        raise HTTPException(422, "The To date is before the From date")
    return accounting.list_vouchers(kind, from_date, to_date)


@router.get("/vouchers/{kind}/{voucher_id}")
def get_voucher(kind: str, voucher_id: int, user: dict = Depends(get_current_user)):
    _require(user, _title(kind))
    return accounting.get_voucher(kind, voucher_id)


@router.post("/vouchers/{kind}", status_code=201)
def create_voucher(kind: str, body: VoucherIn, user: dict = Depends(get_current_user)):
    title = _title(kind)
    if not has_right(user, "add", title):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to add {title}")
    return accounting.create_voucher(kind, body, user["user"])


@router.put("/vouchers/{kind}/{voucher_id}")
def update_voucher(kind: str, voucher_id: int, body: VoucherIn, user: dict = Depends(get_current_user)):
    title = _title(kind)
    if not has_right(user, "edit", title):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to edit {title}")
    return accounting.update_voucher(kind, voucher_id, body, user["user"])


@router.post("/vouchers/{kind}/{voucher_id}/cancel")
def cancel_voucher(kind: str, voucher_id: int, user: dict = Depends(get_current_user)):
    title = _title(kind)
    if not has_right(user, "delete", title):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to cancel {title}")
    return accounting.cancel_voucher(kind, voucher_id, user["user"])


def _require(user: dict, module: str) -> None:
    if not can_use(user, module):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to use {module}")


@router.get("/general-ledger")
def general_ledger(ledger_id: int, from_date: date, to_date: date, user: dict = Depends(get_current_user)):
    _require(user, "General Ledger")
    if to_date < from_date:
        raise HTTPException(422, "The To date is before the From date")
    return accounting.general_ledger(ledger_id, from_date, to_date)


@router.get("/outstanding")
def outstanding(party: Literal["C", "S"] = "C", user: dict = Depends(get_current_user)):
    _require(user, "Party wise OutStanding")
    return accounting.party_outstanding(party)


@router.get("/bill-wise")
def bill_wise(ac_type: Literal["", "R", "P"] = "", ledgers: str = "", user: dict = Depends(get_current_user)):
    _require(user, "Bill wise OutStanding")
    return accounting.bill_wise(ac_type, ledgers)


@router.get("/ageing")
def ageing(ac_type: Literal["", "R", "P"] = "", ledgers: str = "", as_on: date | None = None, user: dict = Depends(get_current_user)):
    _require(user, "Bill wise Ageing")
    return accounting.ageing(ac_type, ledgers, as_on)
