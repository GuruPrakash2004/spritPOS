"""Pydantic request/response models for every endpoint.

The master models (Company, Item, ItemGroup, ...) are also the field lists of the React
master screens: frontend/src/components/masters/masterConfigs.ts mirrors them, so change
both together. Models ending in "In" are request bodies for transactions and admin calls.
"""
from datetime import date as date_type
import re
from typing import Literal

from pydantic import BaseModel, Field, field_validator


# ---------- Masters ----------
class Company(BaseModel):
    name: str
    email: str
    address: str
    phone: str = ""
    fax: str = ""
    mobile: str = ""
    website: str = ""
    gstin: str = ""
    pan: str = ""
    cin: str = ""
    excise_phone: str = ""
    ksbcl_code: str = ""
    licence_type: str = "CL-2"
    state: str = "Karnataka"
    fy_start: str
    fy_end: str

    @field_validator("name", "address")
    @classmethod
    def required_company_text(cls, value: str) -> str:
        value = value.strip()
        if not value:
            raise ValueError("This company field cannot be blank")
        return value

    @field_validator("email")
    @classmethod
    def valid_company_email(cls, value: str) -> str:
        value = value.strip()
        if not re.fullmatch(r"[^\s@]+@[^\s@]+\.[^\s@]+", value):
            raise ValueError("Enter a valid company email address")
        return value


class Item(BaseModel):
    code: str
    name: str
    short_name: str = ""
    hsn_code: str = ""
    group: str = ""
    brand: str = ""
    liqtype: str = "IML"
    uom: str = "BTL"
    package: str = ""
    bottles_per_case: int = Field(12, ge=1, description="Must be ≥ 1")
    mrp: float = Field(0.0, ge=0)
    purchase_rate: float = Field(0.0, ge=0)
    cgst_per: float = 0
    sgst_per: float = 0
    rol: int = 0
    roq: int = 0
    qoh: float = 0
    active: bool = True


class ItemGroup(BaseModel):
    code: str
    name: str
    liquor: bool = True
    consider_for_loyalty: bool = True


class Uom(BaseModel):
    code: str
    name: str


class Package(BaseModel):
    code: str
    litres_per_bottle: float = Field(gt=0)


class Brand(BaseModel):
    code: str
    name: str


class Supplier(BaseModel):
    code: str
    name: str
    gstin: str = ""
    city: str = ""
    active: bool = True


class Customer(BaseModel):
    code: str
    name: str
    mobile: str = ""
    loyalty_card_no: str = ""
    send_sms: bool = False
    points: float = 0


class LedgerGroup(BaseModel):
    code: str
    name: str


class Ledger(BaseModel):
    code: str
    name: str
    group: str


class SalesCounter(BaseModel):
    code: str
    name: str
    system_name: str = ""
    prefix: str
    inactive: bool = False


class PriceUpdate(BaseModel):
    group: str | None = None
    item_codes: list[str] | None = None
    column: Literal["mrp", "purchase_rate"] = "mrp"
    change_by: float = Field(description="Amount to add (negative to reduce)")


# ---------- Transactions ----------
class Line(BaseModel):
    item_code: str
    cases: int = 0
    bottles: int = 0
    rate: float | None = Field(None, description="Defaults to MRP (sales) or purchase rate (purchase)")


class PurchaseIndentIn(BaseModel):
    date: date_type
    supplier: str = "KSBCL"
    lines: list[Line]
    remarks: str = ""


class PurchaseBillIn(BaseModel):
    date: date_type
    supplier: str = "KSBCL"
    supplier_inv_no: str
    supplier_inv_date: date_type
    supplier_inv_value: float = 0
    purchase_type: Literal["Purchase", "Purchase Return", "Stock Transfer In"] = "Purchase"
    copy_from_indent: str | None = Field(None, description="Indent doc no; its lines are used when `lines` is empty")
    lines: list[Line] = []
    tcs: float = 0
    cess: float = 0
    remarks: str = ""


class Payment(BaseModel):
    cash: float = 0
    card: float = 0
    upi: float = 0
    credit: float = 0


class SalesInvoiceIn(BaseModel):
    date: date_type
    counter: str
    customer: str = "CASH"
    salesperson: str | None = None
    payment_type: Literal["CASH", "CARD", "UPI", "SPLIT", "CREDIT"] = "CASH"
    lines: list[Line]
    discount: float = 0
    collected: float | None = Field(None, description="Cash tendered; used to work out payback/shortage")
    redeem_points: float = 0
    payment: Payment | None = None
    hold: bool = False


class SalesReturnIn(BaseModel):
    date: date_type
    against_invoice: str | None = None
    customer: str = "CASH"
    lines: list[Line]
    remarks: str = ""


class ExpenseLine(BaseModel):
    ledger: str
    amount: float = Field(gt=0)
    narration: str = ""


class ExpenseIn(BaseModel):
    date: date_type
    paid_from: str = "CASH"
    lines: list[ExpenseLine]


class StockDeductionIn(BaseModel):
    date: date_type
    reason: Literal["Breakage", "Leakage", "Sample", "Other"] = "Breakage"
    lines: list[Line]
    remarks: str = ""


class ClosingStockLine(BaseModel):
    item_code: str
    closing_qty: float = Field(ge=0)


class ClosingStockIn(BaseModel):
    as_on: date_type
    lines: list[ClosingStockLine]


class GenerateInvoiceIn(BaseModel):
    counter: str
    salesperson: str | None = None


class LoginIn(BaseModel):
    user: str
    password: str


class CancelIn(BaseModel):
    remarks: str = Field(min_length=3, description="Reason for cancelling (mandatory in legacy)")


class DenominationIn(BaseModel):
    date: date_type
    counts: dict[str, int] = Field(description='Note/coin value → count, e.g. {"500": 12, "coins": 35}')


class UserIn(BaseModel):
    user: str = Field(min_length=2, max_length=20)
    password: str | None = Field(None, description="Required for a new user; leave empty on update to keep it")
    role: str
    active: bool = True


class RoleIn(BaseModel):
    name: str = Field(min_length=2, max_length=30)
    modules: list[str]
