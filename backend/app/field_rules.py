"""The Doc Designer's per-field rules, checked the way vLQMS.exe checks them (decompiled DsgrModule: vsfBeforeEditCancel,
vsfGenHideMndt, vsfValidateEdit, validOthFLds, fnExpEvaluate, ReplSVTExpWithConstantOrValues).

What the desktop does, and this module repeats on the server (the browser mirrors it in lib/designerFields.js):

- **Conditions.** Hide, Mandatory and Read-only are flags (Y/N). A flag with no operator holds always; with an operator it
  holds only while `compare <operator> with` is true. A condition on a flag that is N is ignored. `compare` and `with`
  may be a constant, a field (#CODE#) or an expression over fields; SVTCURDATE, SVTUSERCODE, #D# (document date), #FYF# /
  #FYT# (financial year start / end) stand for their values. The comparison is numeric when both sides are numbers, a date
  comparison for dates, else a case-insensitive text comparison.
- **Mandatory.** A mandatory field that is blank (or, for a number, zero: "cannot be blank or less than zero" when negatives
  are not allowed) is refused.
- **Numbers.** Percentage fields run 0-100 (1-100 when mandatory); a range gives a lower and / or upper bound with its operator.
- **Dates.** Within the financial year; a range with operators; "first / last day of the month" fields are snapped.
- **Text.** String type (ALPHABET, ALPHANUMBER, NUMBER), characters that are not allowed, case (UPPER, LOWER, TITLE), width.
- **Combo fields.** The value must exist in the combo's table.

`check_value` returns a message ('' when fine) and the value to store (snapped / cased).
"""
import re
from datetime import date, datetime
from decimal import Decimal, InvalidOperation

from . import formulas, store


def _t(v) -> str:
    return "" if v is None else str(v).strip()


def _yes(v) -> bool:
    return _t(v).upper() in ("Y", "YES", "TRUE", "1")


def _num(v):
    try:
        d = Decimal(_t(v))
    except InvalidOperation:
        return None
    return d if d.is_finite() else None


def _date(v):
    text = _t(v)[:10]
    for fmt in ("%Y-%m-%d", "%d/%m/%Y", "%d-%m-%Y"):
        try:
            return datetime.strptime(text, fmt).date()
        except ValueError:
            continue
    return None


def operand(text, values: dict | None = None, *, doc_date=None, user: str = ""):
    """A condition operand as a value: a Decimal for arithmetic over numbers, else text (fields and tokens replaced)."""
    t = _t(text)
    if not t:
        return ""
    lower = {str(k).lower(): v for k, v in (values or {}).items()}
    fy = getattr(store, "FY", None) or {}
    t = (t.replace("SVTCURDATE", date.today().strftime("%d/%m/%Y")).replace("SVTUSERCODE", user)
         .replace("#D#", (doc_date or date.today()).strftime("%d/%m/%Y")))
    t = t.replace("#FYF#", _t(store.COMPANY.get("fy_start")) or _t(fy.get("start"))).replace("#FYT#", _t(store.COMPANY.get("fy_end")) or _t(fy.get("end")))
    single = re.fullmatch(r"#([^#\s]+)#", t)
    if single:
        return lower.get(single.group(1).lower(), "")
    # arithmetic only when it looks like arithmetic: a field in it, or digits and operators; never a date or a plain word
    if not (("#" in t or re.fullmatch(r"[\d\s+\-*/().]+", t)) and not re.fullmatch(r"\d{1,4}[/-]\d{1,2}[/-]\d{1,4}", t)):
        return re.sub(r"#([^#\s]+)#", lambda m: _t(lower.get(m.group(1).lower(), "")), t)
    try:
        tree = formulas.parse(t)
        scope = formulas.Scope({}, {k: v for k, v in lower.items()}, [], {"SVTUSERCODE": user})
        value = scope._eval(tree, None)
        return value if isinstance(value, Decimal) else _t(value)
    except (formulas.FormulaError, TypeError, ArithmeticError):
        return re.sub(r"#([^#\s]+)#", lambda m: _t(lower.get(m.group(1).lower(), "")), t)


def holds(compare, op, with_, values: dict | None = None, *, doc_date=None, user: str = "", kind: str = "") -> bool:
    """`compare <op> with` as the desktop's fnExpEvaluate decides it. An empty operator is true."""
    op = _t(op)
    if not op:
        return True
    a = operand(compare, values, doc_date=doc_date, user=user)
    b = operand(with_, values, doc_date=doc_date, user=user)
    da, db_ = _date(a), _date(b)
    if kind.lower() == "date" or (da and db_ and not isinstance(a, Decimal)):
        x, y = da, db_
    else:
        na, nb = _num(a), _num(b)
        x, y = (na, nb) if na is not None and nb is not None and _t(a) != "" and _t(b) != "" else (_t(a).lower(), _t(b).lower())
    if x is None or y is None:
        return False
    return {"=": x == y, "==": x == y, "<>": x != y, "!=": x != y, ">": x > y, "<": x < y, ">=": x >= y, "<=": x <= y}.get(op, False)


def flag_now(row: dict, flag: str, prefix: str, values: dict | None = None, **ctx) -> bool:
    """Does the flag hold now? `flag` is the Y/N column, `prefix` the Hide / Mndtry / RO column family."""
    if not _yes(row.get(flag)):
        return False
    return holds(row.get(f"Fld{prefix}CompareVal"), row.get(f"Fld{prefix}Operator"), row.get(f"Fld{prefix}WithVal"), values, **ctx)


def is_hidden(row, values=None, **ctx) -> bool:
    return flag_now(row, "FLdBlnHide", "Hide", values, **ctx)


def is_mandatory(row, values=None, **ctx) -> bool:
    return flag_now(row, "FldBlnMandatory", "Mndtry", values, **ctx)


def is_readonly(row, values=None, **ctx) -> bool:
    return flag_now(row, "FldBlnReadOnly", "RO", values, **ctx) or _yes(row.get("FldBlnDispOnly"))


def _title(text: str) -> str:
    return " ".join(w[:1].upper() + w[1:].lower() for w in text.split(" "))


def check_value(row: dict, value, values: dict | None = None, *, doc_date=None, user: str = "") -> tuple[str, object]:
    """Check one value against the designer row. Returns (message, value to store); a message means refuse it."""
    caption = _t(row.get("FldCaption")) or _t(row.get("FldCode"))
    kind = _t(row.get("FldDataType")).lower()
    text = _t(value)
    ctx = {"doc_date": doc_date, "user": user}
    mandatory = is_mandatory(row, values, **ctx)
    if kind == "numeric" or kind == "list numeric":
        n = _num(text)
        if mandatory and (text == "" or (n is not None and n == 0 and not _yes(row.get("FldBlnComboData")))):
            return f"Mandatory field: '{caption}' cannot be {'blank' if text == '' else 'zero'}", value
        if mandatory and n is not None and _yes(row.get("FldNumBlnAnyNumber")) and not _yes(row.get("FldNumBlnAllowNegative")) and n <= 0:
            return f"Field {caption} cannot be blank or less than zero", value
        if text == "":
            return "", value
        if n is None:
            return f"{caption} must be a number", value
        if n < 0 and not _yes(row.get("FldNumBlnAllowNegative")) and _yes(row.get("FldNumBlnAnyNumber")):
            return f"{caption} cannot be negative", value
        if _yes(row.get("FldNumBlnPercentage")):
            low = 1 if mandatory else 0
            if n < low or n > 100:
                return f"Invalid percentage should range {low}-100", value
        for chk, op, bound in (("FldNumBlnRangeFromChk", "FldNumRangeFromOperator", "FldNumRangeFromValue"),
                               ("FldNumBlnRangeToChk", "FldNumRangeToOperator", "FldNumRangeToValue")):
            if _yes(row.get(chk)) and not holds(text, row.get(op), row.get(bound), values, kind="numeric", **ctx):
                return f"{caption} --- Entered number not {_t(row.get(op))} {_t(row.get(bound))}", value
        return "", value
    if kind == "date":
        if mandatory and text in ("", "/  /", "__/__/____"):
            return f"Mandatory field: '{caption}' cannot be blank", value
        if text in ("", "/  /", "__/__/____"):
            return "", value
        d = _date(text)
        if d is None:
            return "Invalid date", value
        if _yes(row.get("FldDtBlnFirstDOM")):
            d = d.replace(day=1)
        if _yes(row.get("FldDtBlnLastDOM")):
            nxt = date(d.year + (d.month == 12), d.month % 12 + 1, 1)
            d = date.fromordinal(nxt.toordinal() - 1)
        if _yes(row.get("FldDtBlnWithinFinYear")):
            start, end = _date(store.COMPANY.get("fy_start")), _date(store.COMPANY.get("fy_end"))
            if start and end and not (start <= d <= end):
                return f"{caption} --- Date should be within Financial Year {start:%d %b %Y} to {end:%d %b %Y}", value
        for chk, op, bound in (("FldDtBlnRangeFrom", "FldDtRangeFromOperator", "FldDtRangeFromValue"),
                               ("FldDtBlnRangeTo", "FldDtRangeToOperator", "FldDtRangeToValue")):
            if _yes(row.get(chk)) and not holds(d.strftime("%d/%m/%Y"), row.get(op), row.get(bound), values, kind="date", **ctx):
                return f"{caption} --- Entered Date not {_t(row.get(op))} {_t(row.get(bound))}", value
        return "", d.isoformat()
    # text and list strings
    if mandatory and text == "":
        return f"Mandatory field: '{caption}' cannot be blank", value
    if text == "":
        return "", value
    stype = _t(row.get("FldTxtStringType")).upper()
    banned = _t(row.get("FldTxtDoNotAllowChars")) if _yes(row.get("FldTxtBlnDoNotAllowChk")) else ""
    allowed = {"ALPHANUMBER": r"[A-Za-z0-9 ]", "ALPHABET": r"[A-Za-z ]", "NUMBER": r"[0-9.]"}.get(stype)
    for ch in text:
        if allowed and not re.fullmatch(allowed, ch):
            return f"{caption}: '{ch}' is not allowed ({stype.lower()} only)", value
        if banned and ch in banned:
            return f"{caption}: '{ch}' is not allowed", value
    case = _t(row.get("FldTxtCase")).upper()
    out = text.upper() if case == "UPPER" else text.lower() if case == "LOWER" else _title(text) if case == "TITLE" else value
    width = int(_num(row.get("FldDataWidth")) or 0)
    if width and len(text) > width:
        return f"{caption} can be at most {width} characters", value
    return "", out
