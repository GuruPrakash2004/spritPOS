"""Quotation endpoints under /transactions/quotation (app/quotation.py). Written straight into the shop's own tables."""
from datetime import date

from fastapi import APIRouter, Depends, HTTPException
from pydantic import BaseModel, Field

from .. import quotation
from .auth import can_use, get_current_user, has_right

router = APIRouter(prefix="/transactions/quotation", tags=["Quotation"])


class QuotationLine(BaseModel):
    item_code: str = Field(min_length=1, max_length=40)
    qty: float = Field(gt=0, le=1_000_000)
    rate: float | None = Field(None, ge=0, description="Defaults to the item's MRP")


class QuotationIn(BaseModel):
    date: date
    customer: str = Field("CASH", max_length=60)
    remarks: str = Field("", max_length=250)
    lines: list[QuotationLine] = Field(min_length=1, max_length=500)


def _use(user: dict) -> None:
    if not can_use(user, "Quotation"):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to use Quotation")


@router.get("")
def list_quotations(from_date: date, to_date: date, user: dict = Depends(get_current_user)):
    _use(user)
    if to_date < from_date:
        raise HTTPException(422, "The To date is before the From date")
    return quotation.list_quotations(from_date, to_date)


@router.get("/{qid}")
def get_quotation(qid: int, user: dict = Depends(get_current_user)):
    _use(user)
    return quotation.get_quotation(qid)


@router.post("", status_code=201)
def create_quotation(body: QuotationIn, user: dict = Depends(get_current_user)):
    if not has_right(user, "add", "Quotation"):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to add Quotation")
    return quotation.create_quotation(body, user["user"])


@router.put("/{qid}")
def update_quotation(qid: int, body: QuotationIn, user: dict = Depends(get_current_user)):
    if not has_right(user, "edit", "Quotation"):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to edit Quotation")
    return quotation.update_quotation(qid, body, user["user"])


@router.post("/{qid}/cancel")
def cancel_quotation(qid: int, user: dict = Depends(get_current_user)):
    if not has_right(user, "delete", "Quotation"):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to cancel Quotation")
    return quotation.cancel_quotation(qid, user["user"])
