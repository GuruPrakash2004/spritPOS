"""Field checks for what a person types into a master, as the desktop vLQMS forms enforce them: a mobile or phone number is
exactly 10 digits (no letters, spaces or symbols), a PIN code 6 digits, a GSTIN 15 characters in the GST pattern, a PAN 10
characters in the PAN pattern, an e-mail address looks like one, a tax percentage is 0 to 100.

Used on requests only (master create/update, Company Master, users), never when listing: a row already in the shop's database
with an older value still lists, and the rule applies the next time it is saved. A blank value is allowed (these fields are
optional); a field that is required says so on its own.
The same rules are in frontend/src/lib/validation.js (shown beside the field before the request is sent), so change both.
"""
import re

from fastapi import HTTPException

PHONE = (re.compile(r"\d{10}"), "must be exactly 10 digits (numbers only, no spaces, letters or symbols)")
RULES = {
    # field name -> (pattern, what is wrong, upper-case it first)
    "mobile": ("Mobile No", *PHONE, False),
    "phone": ("Phone No", *PHONE, False),
    "excise_phone": ("Excise phone", *PHONE, False),
    "pin": ("PIN code", re.compile(r"\d{6}"), "must be exactly 6 digits", False),
    "gstin": ("GSTIN", re.compile(r"\d{2}[A-Z]{5}\d{4}[A-Z][1-9A-Z]Z[0-9A-Z]"), "must be 15 characters like 29ABCDE1234F1Z5", True),
    "pan": ("PAN", re.compile(r"[A-Z]{5}\d{4}[A-Z]"), "must be 10 characters like ABCDE1234F", True),
    "hsn_code": ("HSN code", re.compile(r"\d{4,8}"), "must be 4 to 8 digits (numbers only)", False),
    "ifsc": ("IFSC code", re.compile(r"[A-Z]{4}0[A-Z0-9]{6}"), "must be 11 characters like SBIN0001234", True),
    "micr": ("MICR code", re.compile(r"\d{9}"), "must be exactly 9 digits", False),
    "account_no": ("Account no.", re.compile(r"\d{9,18}"), "must be 9 to 18 digits (numbers only)", False),
    "email": ("Email", re.compile(r"[^\s@]+@[^\s@]+\.[^\s@]+"), "is not a valid e-mail address", False),
}
PERCENT_FIELDS = {"cgst_per": "CGST %", "sgst_per": "SGST %"}


def problems(data: dict, skip: frozenset | set = frozenset()) -> dict[str, str]:
    """{field: message} for every field of `data` that breaks a rule (the fields in `skip` are not checked)."""
    found = {}
    for key, (label, pattern, wrong, _upper) in RULES.items():
        if key in skip:
            continue
        value = data.get(key)
        if value in (None, ""):
            continue
        text = str(value).strip()
        if _upper:
            text = text.upper()
        if text and not pattern.fullmatch(text):
            found[key] = f"{label} {wrong}."
    for key, label in PERCENT_FIELDS.items():
        value = data.get(key)
        if isinstance(value, (int, float)) and not 0 <= value <= 100:
            found[key] = f"{label} must be between 0 and 100."
    return found


def check(data: dict, skip: frozenset | set = frozenset()) -> dict:
    """Raises 422 naming the first wrong field; returns the values to keep (trimmed, GSTIN and PAN in capitals).
    `skip`: fields whose rule doesn't apply on this screen (Company Master's phone numbers are free text)."""
    wrong = problems(data, skip)
    if wrong:
        raise HTTPException(422, next(iter(wrong.values())))
    cleaned = {}
    for key, (_label, _pattern, _wrong, upper) in RULES.items():
        value = data.get(key)
        if isinstance(value, str):
            text = value.strip()
            cleaned[key] = text.upper() if upper else text
    return cleaned
