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
    licence_type: str = ""
    state: str = ""
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


class MasterRow(BaseModel):
    """Base of the master models. `authorised` is set by the server only (Master Approvals in Admin Master
    (Security)): False while a row waits for approval; whatever a client sends is ignored."""
    authorised: bool | None = None


class Item(MasterRow):
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
    # None when hidden from the caller (roles without cost access) or not sent: an edit then keeps the cost.
    purchase_rate: float | None = Field(None, ge=0)
    cgst_per: float = 0
    sgst_per: float = 0
    rol: int = 0
    roq: int = 0
    qoh: float = 0
    active: bool = True


class ItemGroup(MasterRow):
    code: str
    name: str
    liquor: bool = True
    consider_for_loyalty: bool = True


class Uom(MasterRow):
    code: str
    name: str


class Package(MasterRow):
    code: str
    litres_per_bottle: float   # above 0 on save (masters._clean); desktop rows may hold 0


class Brand(MasterRow):
    code: str
    name: str


class Supplier(MasterRow):
    code: str
    name: str
    alias: str = Field("", description="Name used when exporting to Tally")
    address: str = ""
    city: str = ""
    pin: str = ""
    state: str = ""
    country: str = "INDIA"
    phone: str = ""
    gstin: str = ""
    currency: str = "Rupees"
    active: bool = True


class Customer(MasterRow):
    code: str
    name: str
    mobile: str = ""
    loyalty_card_no: str = ""
    send_sms: bool = False
    points: float = 0


class FinYear(BaseModel):
    start: str
    end: str
    active: bool = False


class CompanyOut(BaseModel):
    """The company profile as served: blank until the shop's database is loaded (no validation on the way out),
    plus the financial years from Web_MstCompFinYear."""
    name: str = ""
    email: str = ""
    address: str = ""
    phone: str = ""
    fax: str = ""
    mobile: str = ""
    website: str = ""
    gstin: str = ""
    pan: str = ""
    cin: str = ""
    excise_phone: str = ""
    ksbcl_code: str = ""
    licence_type: str = ""
    state: str = ""
    fy_start: str = ""
    fy_end: str = ""
    fin_years: list[FinYear] = []


class LedgerGroup(MasterRow):
    code: str
    name: str


class Ledger(MasterRow):
    code: str
    name: str
    group: str


class SalesCounter(MasterRow):
    code: str
    name: str
    system_name: str = ""
    prefix: str
    inactive: bool = False


class SalesPerson(MasterRow):
    code: str
    name: str
    mobile: str = ""
    active: bool = True


class PriceUpdate(BaseModel):
    group: str | None = None
    item_codes: list[str] | None = None
    column: Literal["mrp", "purchase_rate"] = "mrp"
    change_by: float = Field(description="Amount to add (negative to reduce)")


# ---------- Transactions ----------
# Request bodies only (never response models), so their limits can't break a list of existing data.
MAX_LINES = 500


class Line(BaseModel):
    item_code: str
    cases: int = Field(0, ge=0, le=100_000)
    bottles: int = Field(0, ge=0, le=1_000_000)
    rate: float | None = Field(None, ge=0, description="Defaults to MRP (sales) or purchase rate (purchase)")


class PurchaseIndentIn(BaseModel):
    date: date_type
    supplier: str = "KSBCL"
    lines: list[Line] = Field(min_length=1, max_length=MAX_LINES)
    remarks: str = Field("", max_length=500)


class PurchaseBillIn(BaseModel):
    date: date_type
    supplier: str = "KSBCL"
    supplier_inv_no: str
    supplier_inv_date: date_type
    supplier_inv_value: float = Field(0, ge=0)
    purchase_type: Literal["Purchase", "Purchase Return", "Stock Transfer In"] = "Purchase"
    copy_from_indent: str | None = Field(None, description="Indent doc no; its lines are used when `lines` is empty")
    lines: list[Line] = Field([], max_length=MAX_LINES)
    tcs: float = Field(0, ge=0)
    cess: float = Field(0, ge=0)
    remarks: str = Field("", max_length=500)


class Payment(BaseModel):
    cash: float = Field(0, ge=0)
    card: float = Field(0, ge=0)
    upi: float = Field(0, ge=0)
    credit: float = Field(0, ge=0)


class SalesInvoiceIn(BaseModel):
    date: date_type
    counter: str
    customer: str = "CASH"
    salesperson: str | None = None
    payment_type: Literal["CASH", "CARD", "UPI", "SPLIT", "CREDIT"] = "CASH"
    lines: list[Line] = Field(min_length=1, max_length=MAX_LINES)
    discount: float = Field(0, ge=0)
    collected: float | None = Field(None, ge=0, description="Cash tendered; used to work out payback/shortage")
    redeem_points: float = Field(0, ge=0)
    payment: Payment | None = None
    hold: bool = False


class SalesReturnIn(BaseModel):
    date: date_type
    against_invoice: str | None = None
    customer: str = "CASH"
    lines: list[Line] = Field(min_length=1, max_length=MAX_LINES)
    remarks: str = Field("", max_length=500)


class ExpenseLine(BaseModel):
    ledger: str
    amount: float = Field(gt=0)
    narration: str = ""


class ExpenseIn(BaseModel):
    date: date_type
    paid_from: Literal["CASH", "BANK"] = "CASH"
    lines: list[ExpenseLine] = Field(min_length=1, max_length=MAX_LINES)


class StockDeductionIn(BaseModel):
    date: date_type
    reason: Literal["Breakage", "Leakage", "Sample", "Other"] = "Breakage"
    lines: list[Line] = Field(min_length=1, max_length=MAX_LINES)
    remarks: str = Field("", max_length=500)


class ClosingStockLine(BaseModel):
    item_code: str
    closing_qty: float = Field(ge=0)


class ClosingStockIn(BaseModel):
    as_on: date_type
    lines: list[ClosingStockLine] = Field(min_length=1, max_length=5000)


class GenerateInvoiceIn(BaseModel):
    counter: str
    salesperson: str | None = None


# Password fields arrive encrypted from the browser ("enc:v1:...", app/secure_fields.py), hence the length.
class LoginIn(BaseModel):
    user: str = Field(max_length=50)
    password: str = Field(max_length=1000)


class ChangePasswordIn(BaseModel):
    current_password: str = Field(max_length=1000)
    new_password: str = Field(max_length=1000)


class CancelIn(BaseModel):
    remarks: str = Field(min_length=3, description="Reason for cancelling (mandatory in legacy)")


DENOMINATIONS = ("2000", "500", "200", "100", "50", "20", "10", "coins")


class DenominationIn(BaseModel):
    date: date_type
    counts: dict[str, int] = Field(description='Note/coin value → count, e.g. {"500": 12, "coins": 35}')

    @field_validator("counts")
    @classmethod
    def known_notes(cls, value: dict[str, int]) -> dict[str, int]:
        unknown = sorted(set(value) - set(DENOMINATIONS))
        if unknown:
            raise ValueError(f"Unknown note or coin value: {', '.join(unknown)}. Use {', '.join(DENOMINATIONS)}")
        if any(n < 0 for n in value.values()):
            raise ValueError("Counts cannot be negative")
        return value


class LoyaltySettings(BaseModel):
    points_per_rupee: float = Field(ge=0, le=1)
    alert_when_points_above: float = Field(ge=0)
    min_bill_amount: float = Field(ge=0)


class InvoiceSettings(BaseModel):
    salesperson_required: bool
    print_preview: bool
    barcode_scan_jumps_to_qty: bool


class SettingsIn(BaseModel):
    """Global Settings. Send only the sections you change; unknown sections are rejected."""
    model_config = {"extra": "forbid"}
    loyalty: LoyaltySettings | None = None
    invoice: InvoiceSettings | None = None


class UserIn(BaseModel):
    user: str = Field(min_length=2, max_length=20, pattern=r"^[A-Za-z0-9_.-]+$")
    password: str | None = Field(None, max_length=1000, description="Encrypted (enc:v1:...). Required for a new user; leave empty on update to keep it")
    role: str
    active: bool = True
    # Legacy frmMstUSer fields (all optional).
    full_name: str = Field("", max_length=80)
    email: str = Field("", max_length=120)
    mobile: str = Field("", max_length=20)
    department: str = Field("", max_length=60)
    dob: date_type | None = None
    password_reset_on: date_type | None = Field(None, description="From this date the user must change their password")
    inactive_reason: str = Field("", max_length=200, description="Required when the user is marked inactive")


class LoginSettingsIn(BaseModel):
    """Admin Master (Security) → Login Settings (legacy frmUtlLoginSettings)."""
    min_length: int = Field(8, ge=8, le=64)
    require_mix: bool = True
    require_upper: bool = False
    require_lower: bool = False
    require_digit: bool = False
    require_special: bool = False
    password_expiry_days: int = Field(0, ge=0, le=3650, description="0 = never expires")
    show_remember_me: bool = False


class FieldRightsIn(BaseModel):
    """master path -> field -> "hidden" | "readonly" (fields not listed are normal)."""
    masters: dict[str, dict[str, Literal["hidden", "readonly"]]]


class MasterApprovalIn(BaseModel):
    masters: list[str]


class ReportRightsIn(BaseModel):
    users: list[str] = Field(default_factory=list, description="Empty = everyone who can open Customised Reports")


class AdnlAdminPasswordIn(BaseModel):
    """Change Adnl. ADMIN Password: your own login password (encrypted for "current") and the new additional
    ADMIN password (encrypted for "new"; empty removes it, so your own password is asked instead)."""
    current_password: str = Field(max_length=1000)
    new_password: str = Field("", max_length=1000)


class ActionRights(BaseModel):
    add: bool = True
    edit: bool = True
    delete: bool = True


class RoleIn(BaseModel):
    name: str = Field(min_length=2, max_length=30)
    modules: list[str]
    # Add/Edit/Delete ticks per module; a module left out keeps full rights on it.
    rights: dict[str, ActionRights] = {}


class ReportEmailIn(BaseModel):
    """Customised Reports → Email: which report to build, and who gets it."""
    name: str
    from_date: date_type
    to_date: date_type
    ledger: list[str] = []
    supplier: list[str] = []
    group: list[str] = []
    to: list[str] = Field(min_length=1, max_length=5)
    message: str = Field("", max_length=1000)

    @field_validator("to")
    @classmethod
    def valid_recipients(cls, value: list[str]) -> list[str]:
        out = [v.strip() for v in value if v.strip()]
        for v in out:
            if len(v) > 254 or not re.fullmatch(r"[^\s@,;<>]+@[^\s@,;<>]+\.[^\s@,;<>]+", v):
                raise ValueError(f"'{v}' is not a valid email address")
        if not out:
            raise ValueError("Enter at least one email address")
        return out


