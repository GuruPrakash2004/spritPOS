"""Global Settings → Other Settings (legacy FrmUtlPrintSettings / UtlOption, FrmUtlTrnSettings / UtlTransactions,
frmUtlDocNumber / frmUtlInvNumberFmt, frmUtlMenuSequence, frmUtlBulkImpFlds), limited to what the web app acts on:

- store.DOC_SETTINGS[kind]: per web document
  - after_save: "none" | "prompt" | "preview" (print after a save; the screens read it), default_format (print
    format id), for the documents that print;
  - allow_duplicate_items, allow_edit_date (the date must be today when off), edit_days (a sales invoice older
    than this can't be cancelled; 0 = no limit), allow_copy_from (Purchase Bill: Copy From indent),
    update_rate_from_pb (Purchase Bill: the item's purchase rate follows the bill).
- store.NUMBER_SERIES[kind]: the document number prefix, and Reset Numbers (never below a number already used).
  Sales invoices take their prefix from the Sales Counter Master, as in the desktop app.
- store.MENU_LAYOUT: modules hidden from the sidebar and module search, and shortcut codes (legacy SCMenuName).
- store.IMPORT_FIELDS["fields"]: Item Master fields an import may overwrite on existing items (empty = all).
Desktop-only options (direct printing to a named printer, Crystal formulas, e-mail routing per print option,
Tally ledgers, grid colours, SMS) are listed in frontend/LEGACY_PARITY.md instead.
"""
import re
from datetime import date

from fastapi import HTTPException

from . import store

# Web document kinds, their screens and default number prefixes.
KINDS = {
    "sales_invoice": ("Sales Invoice", None), "purchase_bill": ("Purchase Bill", "PB"),
    "purchase_indent": ("Purchase Indent", "PI"), "sales_return": ("Sales Return", "SR"),
    "stock_deduction": ("Stock Deduction", "SD"), "expense": ("Expense Entry", "EXP"),
    "closing_stock": ("Closing Stock Entry", "CSE"),
}
PRINTS = {"sales_invoice", "purchase_indent", "sales_return", "stock_deduction"}   # screens with a Print button
AFFECTS_STOCK = {"sales_invoice", "purchase_bill", "sales_return", "stock_deduction"}
DEFAULTS = {"after_save": "none", "default_format": "", "allow_duplicate_items": True, "allow_edit_date": True,
            "edit_days": 0, "allow_copy_from": True, "update_rate_from_pb": False}
PREFIX_RE = re.compile(r"^[A-Za-z0-9]{1,6}$")


def settings(kind: str) -> dict:
    return {**DEFAULTS, **store.DOC_SETTINGS.get(kind, {})}


def all_settings() -> dict:
    return {k: {**settings(k), "screen": KINDS[k][0], "prints": k in PRINTS, "affects_stock": k in AFFECTS_STOCK}
            for k in KINDS}


def check_lines(kind: str, lines: list, on: date | str) -> None:
    """Transaction Settings on a new document: duplicate items and the document date."""
    s = settings(kind)
    if not s["allow_duplicate_items"]:
        codes = [getattr(l, "item_code", None) if not isinstance(l, dict) else l.get("item_code") for l in lines]
        codes = [c for c in codes if c]
        dup = sorted({c for c in codes if codes.count(c) > 1})
        if dup:
            raise HTTPException(422, f"Duplicate Item! {', '.join(dup)} appear more than once "
                                     f"(Global Settings → Transaction Settings for {KINDS[kind][0]})")
    if not s["allow_edit_date"]:
        d = on if isinstance(on, date) else date.fromisoformat(str(on)[:10])
        if d != date.today():
            raise HTTPException(422, f"{KINDS[kind][0]} must be dated today (the document date can't be changed: "
                                     "Global Settings → Transaction Settings)")


def check_edit_days(kind: str, on: str, what: str) -> None:
    days = int(settings(kind)["edit_days"] or 0)
    if days and (date.today() - date.fromisoformat(str(on)[:10])).days > days:
        raise HTTPException(409, f"{what} is more than {days} day(s) old and can't be changed any more "
                                 "(Global Settings → Transaction Settings)")


# ─── Document numbers ─────────────────────────────────────────────────────────

def prefix(kind: str) -> str:
    return store.NUMBER_SERIES.get(kind, {}).get("prefix") or KINDS[kind][1]


def highest_used(kind: str) -> int:
    """The highest number on any document of this kind (all its prefixes share one counter)."""
    nums = [int(m.group(1)) for no in store.DOCS.get(kind, {}) if (m := re.search(r"-(\d+)$", no))]
    return max(nums, default=0)


def numbering() -> list[dict]:
    out = []
    for kind, (screen, default) in KINDS.items():
        seq = store._seq.get(kind)
        out.append({"kind": kind, "screen": screen, "prefix": prefix(kind) if default else None,
                    "default_prefix": default, "next_number": seq.value if seq else 1,
                    "example": f"{store.FY_PREFIX}/{prefix(kind) if default else '<counter prefix>'}-{(seq.value if seq else 1):07d}",
                    "from_counter": default is None})
    return out


def set_series(kind: str, new_prefix: str | None, next_number: int | None) -> dict:
    if kind not in KINDS:
        raise HTTPException(404, f"Unknown document '{kind}'")
    if new_prefix is not None:
        if KINDS[kind][1] is None:
            raise HTTPException(409, "Sales invoices take their prefix from the Sales Counter Master")
        if not PREFIX_RE.match(new_prefix):
            raise HTTPException(422, "Prefix: 1 to 6 letters or digits")
        store.NUMBER_SERIES.setdefault(kind, {})["prefix"] = new_prefix.upper()
    if next_number is not None:
        used = highest_used(kind)
        if next_number <= used:
            raise HTTPException(409, f"Number {next_number} is already used (highest is {used}); start from {used + 1} or later")
        store._seq.setdefault(kind, store.Seq(next_number)).value = next_number
    return next(r for r in numbering() if r["kind"] == kind)
