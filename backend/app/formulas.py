"""The Doc Designer's field formulas (UtlDocDsgrFlds.FldFormula), evaluated without `eval`.

The language, as found in the shop's designer rows:
    #FIELD#                  the value of another field (a detail column inside a row, else a header / total field)
    + - * / ( )              arithmetic, a leading + or - allowed ("+ #A# + + #B#" occurs)
    SVTCOLSUM(#FIELD#)       the sum of a detail column over every row
    SVTROUND2(x)             round to 2 decimals;  SVTROUND50(x)  round to the whole rupee, 50 paise and over goes up
                             (checked against 3,000 saved desktop bills: INVVALUE and CASHAMT match every time)
    SVTAMTINWORDS(x)         "Rupees Two Lakhs Forty Nine Thousand Twenty Nine And Forty Four Paise Only"
    SVTSYSNAME, SVTUSERCODE  the billing machine's name and the signed-in user's code
Fields do not have to be listed in dependency order (PB's TOT uses TOTNET, listed after it), so a field is worked out
when something asks for it, with a guard against loops. A formula this module can't read (a stray "\\", SVTCOLSUM(5))
is reported and its field is left as typed.

Same grammar and rounding as frontend/src/lib/formulas.js; both are tested against shared vectors.
"""
import re
from decimal import ROUND_HALF_UP, Decimal, InvalidOperation

_TOKEN = re.compile(r"\s*(?:(?P<num>\d+(?:\.\d+)?)|#(?P<ref>[^#\s]+)#|(?P<str>'[^']*'|`[^`]*`)|(?P<name>[A-Za-z_][A-Za-z_0-9]*)|(?P<op>[-+*/(),]))")


class FormulaError(ValueError):
    """The formula can't be read."""


def _tokens(text: str) -> list[tuple[str, str]]:
    out, pos, text = [], 0, text.strip()
    while pos < len(text):
        m = _TOKEN.match(text, pos)
        if not m or m.end() == pos:
            raise FormulaError(f"unexpected {text[pos:pos + 8]!r}")
        pos = m.end()
        kind = m.lastgroup
        out.append((kind, m.group(kind)))
    return out


class _Parser:
    def __init__(self, tokens):
        self.t, self.i = tokens, 0

    def peek(self):
        return self.t[self.i] if self.i < len(self.t) else (None, None)

    def take(self):
        tok = self.peek()
        self.i += 1
        return tok

    def expr(self):
        node = self.term()
        while self.peek() in (("op", "+"), ("op", "-")):
            op = self.take()[1]
            node = ("bin", op, node, self.term())
        return node

    def term(self):
        node = self.unary()
        while self.peek() in (("op", "*"), ("op", "/")):
            op = self.take()[1]
            node = ("bin", op, node, self.unary())
        return node

    def unary(self):
        if self.peek() in (("op", "+"), ("op", "-")):
            op = self.take()[1]
            inner = self.unary()
            return ("neg", inner) if op == "-" else inner
        return self.primary()

    def primary(self):
        kind, value = self.take()
        if kind == "num":
            return ("num", Decimal(value))
        if kind == "ref":
            return ("ref", value)
        if kind == "str":
            return ("str", value[1:-1])
        if kind == "name":
            if self.peek() == ("op", "("):
                self.take()
                args = []
                if self.peek() != ("op", ")"):
                    args.append(self.expr())
                    while self.peek() == ("op", ","):
                        self.take()
                        args.append(self.expr())
                if self.take() != ("op", ")"):
                    raise FormulaError("missing )")
                return ("call", value.upper(), args)
            return ("sys", value.upper())
        if (kind, value) == ("op", "("):
            node = self.expr()
            if self.take() != ("op", ")"):
                raise FormulaError("missing )")
            return node
        raise FormulaError("incomplete formula")


_parsed: dict[str, tuple] = {}


def parse(text: str) -> tuple:
    """The formula as a tree; raises FormulaError. Parsed once per text."""
    if text not in _parsed:
        p = _Parser(_tokens(text))
        tree = p.expr()
        if p.i != len(p.t):
            raise FormulaError("unexpected text after the formula")
        _parsed[text] = tree
    return _parsed[text]


def refs(text: str) -> set[str]:
    """The field codes (lower case) a formula uses."""
    try:
        tree = parse(text)
    except FormulaError:
        return set()
    out: set[str] = set()

    def walk(n):
        if n[0] == "ref":
            out.add(n[1].lower())
        elif n[0] == "bin":
            walk(n[2]); walk(n[3])
        elif n[0] == "neg":
            walk(n[1])
        elif n[0] == "call":
            for a in n[2]:
                walk(a)
    walk(tree)
    return out


# ─── Numbers ─────────────────────────────────────────────────────────────────

def _is_number(v) -> bool:
    try:
        Decimal(str(v).strip())
        return str(v).strip() != ""
    except InvalidOperation:
        return False


def _text_of(v) -> str:
    return "" if v is None else (f"{v.normalize():f}" if isinstance(v, Decimal) else str(v))


def _as_date(v):
    from datetime import datetime
    text = str(v).strip()[:10]
    for fmt in ("%Y-%m-%d", "%d/%m/%Y", "%d-%m-%Y"):
        try:
            return datetime.strptime(text, fmt).date()
        except ValueError:
            continue
    return None


def to_decimal(v) -> Decimal:
    if isinstance(v, Decimal):
        return v
    if v is None or v == "" or isinstance(v, bool):
        return Decimal(0)
    try:
        return Decimal(str(v).strip())
    except InvalidOperation:
        return Decimal(0)


def round_half_up(v: Decimal, places: int = 0) -> Decimal:
    return v.quantize(Decimal(1).scaleb(-places), rounding=ROUND_HALF_UP)


_ONES = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen",
         "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"]
_TENS = ["", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"]


def _below_100(n: int) -> str:
    return _ONES[n] if n < 20 else (_TENS[n // 10] + (" " + _ONES[n % 10] if n % 10 else ""))


def _words(n: int) -> str:
    """An Indian-system number in words (Crores, Lakhs, Thousand, Hundred; Lakh and Crore take an s above one)."""
    if n == 0:
        return "Zero"
    parts = []
    for size, name in ((10_000_000, "Crore"), (100_000, "Lakh"), (1000, "Thousand"), (100, "Hundred")):
        if n >= size:
            count = n // size
            parts.append(_words(count) + " " + (name + "s" if name in ("Lakh", "Crore") and count > 1 else name))
            n %= size
    if n:
        parts.append(_below_100(n))
    return " ".join(parts)


def amount_in_words(amount) -> str:
    """"Rupees Eighty Thousand Five Hundred Seventy One And Fifty Four Paise Only" (the desktop's wording)."""
    value = round_half_up(to_decimal(amount), 2)
    rupees, paise = int(abs(value)), int((abs(value) * 100) % 100)
    text = "Rupees " + _words(rupees)
    if paise:
        text += " And " + _words(paise) + " Paise"
    return text + " Only"


# ─── Evaluation ──────────────────────────────────────────────────────────────

class Scope:
    """What a formula can see. `row` is the detail row being worked out (None for a header / total field), `rows` every
    detail row, `values` the typed header / footer / total values, `system` SVTSYSNAME / SVTUSERCODE."""

    def __init__(self, formulas: dict[str, str], values: dict, rows: list[dict], system: dict | None = None):
        self.formulas = {k.lower(): v for k, v in formulas.items()}      # field code -> formula text
        self.detail = {k.lower() for k in (rows[0].keys() if rows else [])}
        self.values = {k.lower(): v for k, v in values.items()}
        self.rows = [{k.lower(): v for k, v in r.items()} for r in rows]
        self.system = {k.upper(): v for k, v in (system or {}).items()}
        self.detail_formulas: set[str] = set()   # set by the caller: codes of Detail-section fields that have a formula
        self._row_memo: list[dict] = [{} for _ in self.rows]
        self._memo: dict = {}
        self._busy: set = set()
        self.problems: list[str] = []

    def row_value(self, i: int, code: str):
        code = code.lower()
        memo = self._row_memo[i]
        if code in memo:
            return memo[code]
        key = ("row", i, code)
        if key in self._busy:
            self.problems.append(f"{code} refers to itself")
            return Decimal(0)
        if code in self.detail_formulas and code in self.formulas:
            self._busy.add(key)
            try:
                memo[code] = self._eval(parse(self.formulas[code]), i)
            except (FormulaError, TypeError, ArithmeticError, IndexError) as e:
                self.problems.append(f"{code}: {e}")
                memo[code] = to_decimal(self.rows[i].get(code))
            finally:
                self._busy.discard(key)
            return memo[code]
        if code in self.rows[i]:
            return self.rows[i][code]
        return self.value(code)

    def value(self, code: str):
        code = code.lower()
        if code in self._memo:
            return self._memo[code]
        if code in self.formulas and code not in self.detail_formulas:
            if code in self._busy:
                self.problems.append(f"{code} refers to itself")
                return Decimal(0)
            self._busy.add(code)
            try:
                self._memo[code] = self._eval(parse(self.formulas[code]), None)
            except (FormulaError, TypeError, ArithmeticError, IndexError) as e:
                self.problems.append(f"{code}: {e}")
                self._memo[code] = self.values.get(code)
            finally:
                self._busy.discard(code)
            return self._memo[code]
        return self.values.get(code)

    def _eval(self, n, row: int | None):
        kind = n[0]
        if kind == "num":
            return n[1]
        if kind == "str":
            return n[1]
        if kind == "ref":
            v = self.row_value(row, n[1]) if row is not None else self.value(n[1])
            return v if isinstance(v, str) and not _is_number(v) else to_decimal(v)
        if kind == "neg":
            return -self._eval(n[1], row)
        if kind == "bin":
            a, b = self._eval(n[2], row), self._eval(n[3], row)
            if n[1] == "+":
                return a + b
            if n[1] == "-":
                return a - b
            if n[1] == "*":
                return a * b
            return a / b if b else Decimal(0)
        if kind == "sys":
            return self.system.get(n[1], "")
        name, args = n[1], n[2]
        if name == "SVTCOLSUM":
            if len(args) != 1 or args[0][0] != "ref":
                raise FormulaError("SVTCOLSUM needs a field")
            return sum((to_decimal(self.row_value(i, args[0][1])) for i in range(len(self.rows))), Decimal(0))
        if name == "SVTCOLAVG":
            if len(args) != 1 or args[0][0] != "ref" or not self.rows:
                return Decimal(0)
            total = sum((to_decimal(self.row_value(i, args[0][1])) for i in range(len(self.rows))), Decimal(0))
            return total / len(self.rows)
        if name == "SVTCOLMAX":
            if len(args) != 1 or args[0][0] != "ref" or not self.rows:
                return Decimal(0)
            return max(to_decimal(self.row_value(i, args[0][1])) for i in range(len(self.rows)))
        if name == "SVTROUND":   # vLQMS.exe: floor(x * 10^n + 0.5) / 10^n
            places = int(to_decimal(self._eval(args[1], row))) if len(args) > 1 else 0
            scale = Decimal(10) ** places
            return (to_decimal(self._eval(args[0], row)) * scale + Decimal("0.5")).to_integral_value(rounding="ROUND_FLOOR") / scale
        if name == "SVTROUND100":   # any fraction rounds up to the next whole number
            x = to_decimal(self._eval(args[0], row))
            return x if x == x.to_integral_value() else x.to_integral_value(rounding="ROUND_DOWN") + 1
        if name == "SVTROUNDOFFAMT":   # the rounding amount: rounded to the rupee minus the original
            x = to_decimal(self._eval(args[0], row))
            return Decimal(0) if x == 0 else round_half_up((x + Decimal("0.5")).to_integral_value(rounding="ROUND_FLOOR") - x, 2)
        if name == "SVTCALCTAXAMT":   # (amount, percent, mode): the tax rounded to 2 places, then to the rupee (0), 50 paise rule (50) or up (100)
            amount = round_half_up(to_decimal(self._eval(args[0], row)) * to_decimal(self._eval(args[1], row)) / 100, 2)
            mode = int(to_decimal(self._eval(args[2], row))) if len(args) > 2 else -1
            if mode == 0:
                return (amount + Decimal("0.5")).to_integral_value(rounding="ROUND_FLOOR")
            if mode == 50:
                return round_half_up(amount, 0)
            if mode == 100:
                return amount if amount == amount.to_integral_value() else amount.to_integral_value(rounding="ROUND_DOWN") + 1
            return amount
        if name == "SVTCONCATENATE":
            return "".join(_text_of(self._eval(a, row)) for a in args)
        if name == "SVTDATEDIFF":   # (from, to, 'D' | 'M'): the difference plus one, as the desktop counts both ends
            unit = str(self._eval(args[2], row)).strip().upper() if len(args) > 2 else "D"
            a, b = _as_date(self._eval(args[0], row)), _as_date(self._eval(args[1], row))
            if a is None or b is None or unit not in ("D", "M"):
                return ""
            return Decimal((b - a).days + 1) if unit == "D" else Decimal((b.year - a.year) * 12 + b.month - a.month + 1)
        if name == "SVTROUND2":
            return round_half_up(to_decimal(self._eval(args[0], row)), 2)
        if name == "SVTROUND50":
            return round_half_up(to_decimal(self._eval(args[0], row)), 0)
        if name == "SVTAMTINWORDS":
            return amount_in_words(self._eval(args[0], row))
        raise FormulaError(f"unknown function {name}")


def compute(fields: list[dict], values: dict, rows: list[dict], system: dict | None = None) -> dict:
    """Work out every formula of a document definition.

    `fields` are definition fields (schema_meta: code, section, formula, decimals, type). Returns
    {"values": {code: value} for header / footer / total fields with a formula, "rows": [{code: value}] for detail
    columns with a formula, "problems": [...]}. Numbers are Decimals rounded half-up to the field's decimals."""
    formulas = {f["code"]: f["formula"] for f in fields if f.get("formula")}
    scope = Scope(formulas, values, rows, system)
    scope.detail_formulas = {f["code"].lower() for f in fields if f.get("formula") and str(f.get("section")).lower() == "detail"}
    decimals = {f["code"].lower(): f.get("decimals", 2) for f in fields}
    kinds = {f["code"].lower(): f.get("type") for f in fields}

    def fit(code, v):
        if isinstance(v, Decimal) and kinds.get(code.lower()) == "number":
            return round_half_up(v, int(decimals.get(code.lower(), 2) or 0))
        return v

    out_rows = [{f["code"]: fit(f["code"], scope.row_value(i, f["code"])) for f in fields
                 if f["code"].lower() in scope.detail_formulas} for i in range(len(rows))]
    out = {f["code"]: fit(f["code"], scope.value(f["code"])) for f in fields
           if f.get("formula") and f["code"].lower() not in scope.detail_formulas}
    for f in fields:   # a formula whose text can't be read is reported once
        if f.get("formula"):
            try:
                parse(f["formula"])
            except FormulaError as e:
                scope.problems.append(f"{f['code'].lower()}: {e}")
    return {"values": out, "rows": out_rows, "problems": sorted(set(scope.problems))}
