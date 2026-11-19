"""Invoice Settings (legacy frmUtlInvNumberFmt, Global Settings → Other Settings → Invoice Settings).

Everything lives in the desktop's own tables (the web creates nothing):
  UtlGSettings, SetGroup 'Invoice':  InvNoReset ('M' month wise, 'D' day wise, '' none)  ·  CounterWsRst ('Y'/'N')  ·  DocNoFmt (the 15-cell
  strip, saved as the exe does)  ·  PrefixSettings (12 comma separated letters, Jan to Dec; '' when no prefix)  ·  INVF12 / INVCTRLF12 / INVALTF12
  ('S' Save, 'SP' Save and Print, 'SPP' Save and Preview)
  UtlMisc (one row):  InvSMSblnSales / Cancel / Delete, InvSMSSalesvalue, InvSMSSalesMSg / CancelMsg / DeleteMsg, RateFld ('Y' = Update Rate From PB
  to Item Master).  The mobile numbers (InvMsgMobNo) are the SMS screen's (frmUtlInvSMS), not this form's.
The exe only UPDATEs the UtlGSettings rows; a row the database lacks is added here so a new database works.

The document number strip: each of the 15 cells holds a token 1 to 8 (1 prefix, 2 counter, 3 / 4 the two years, 5 / 6 their last two digits, 7 the
running number as 0000000, 8 the running number as it is) or any text (a separator). One difference from the exe: its save tests `Val < 8`, so token 8
is written as the plain text "8" although its number maker understands <<8>>; here 8 is saved as <<8>>.
"""
import re

from fastapi import HTTPException

from . import admin_audit, db, sqlstore, store

SCREEN = "Invoice Settings"
CELLS = 15
MONTHS = 12
CHOICES = {"S": "Save", "SP": "Save and Print", "SPP": "Save and Preview"}
CODE_OF = {v: k for k, v in CHOICES.items()}
F12_NAMES = (("f12", "INVF12"), ("ctrl_f12", "INVCTRLF12"), ("alt_f12", "INVALTF12"))


def _need_db() -> None:
    if not sqlstore.db_usable():
        raise HTTPException(503, f"Invoice Settings needs the shop's database. {sqlstore.db_problem()}")


def _s(v) -> str:
    return "" if v is None else str(v).strip()


def _val(text: str) -> float:
    """VB's Val(): the number at the start of the text, 0 when there is none."""
    m = re.match(r"\s*([+-]?(?:\d+\.?\d*|\.\d+))", text or "")
    return float(m.group(1)) if m else 0.0


def token_of(cell: str) -> int:
    """The token (1 to 8) a strip cell stands for, or 0 when it is plain text (SetDocInvFmt: Select Case Val(...) 1 to 8)."""
    v = _val(cell)
    return int(v) if v in (1, 2, 3, 4, 5, 6, 7, 8) else 0


def decode_format(stored: str) -> list[str]:
    """fillDocNoPfx: the strip's cells from the saved DocNoFmt."""
    s = (stored or "").replace("<<", ",").replace(">>", ",")
    s = s[1:len(s) - 1] if len(s) >= 2 else ""   # Mid(s, 2, Len(s) - 2)
    cells = [p for p in s.split(",") if p != ""][:CELLS]
    return cells + [""] * (CELLS - len(cells))


def encode_format(cells: list[str]) -> str:
    """saveDetails: a token cell as <<n>>, any other cell as its text and a comma."""
    out = ""
    for c in (cells + [""] * CELLS)[:CELLS]:
        t = token_of(c)
        out += f"<<{t}>>" if t else f"{c},"
    return out


def _sample() -> dict:
    """The help table's second row (initScreen): sample values, the years from the company's financial year."""
    def year(key: str) -> str:
        m = re.match(r"(\d{4})", _s(store.COMPANY.get(key)))
        return m.group(1) if m else ""
    yf, yt = year("fy_start"), year("fy_end")
    return {"prefix": "JAN", "counter": "C1", "year_from": yf, "year_to": yt, "sh_year_from": yf[-2:], "sh_year_to": yt[-2:], "doc_no": "00001", "sh_doc_no": "1"}


def get() -> dict:
    """The form as it opens: fillInvNoFmt, loadCboValue and loadContent."""
    _need_db()

    def one(sql: str, params: tuple = ()) -> str:
        r = db.fetchone(sql, params)
        return _s(next(iter(r.values()), "")) if r else ""

    base = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = ?"
    reset = one(base, ("InvNoReset",)).upper()
    months = (one(base, ("PrefixSettings",)).split(","))[:MONTHS]
    months += [""] * (MONTHS - len(months))
    keys = {}
    for key, name in F12_NAMES:
        keys[key] = CHOICES.get(one("Select SetValue from utlGSettings where setName = ?", (name,)).upper(), "")   # the exe asks without the group
    misc = db.fetchone("Select * from UtlMisc") or {}

    def flag(col: str) -> bool:
        return _s(misc.get(col)).upper() == "Y"

    return {
        "prefix": reset if reset in ("M", "D") else "", "counter_reset": one(base, ("CounterWsRst",)).upper() == "Y",
        "months": [m.strip().upper()[:1] for m in months], "format": decode_format(one(base, ("DocNoFmt",))), **keys,
        "sms": {"on_sales": flag("InvSMSblnSales"), "on_cancel": flag("InvSMSblnCancel"), "on_delete": flag("InvSMSblnDelete"),
                "sales_value": misc.get("InvSMSSalesvalue") if misc.get("InvSMSSalesvalue") is not None else 0,
                "sales_msg": _s(misc.get("InvSMSSalesMSg")), "cancel_msg": _s(misc.get("InvSMSCancelMsg")), "delete_msg": _s(misc.get("InvSMSDeleteMsg"))},
        "update_rate": flag("RateFld"), "sms_available": "InvSMSblnSales" in misc, "sample": _sample(),
    }


def check(body: dict) -> None:
    """ValidFields: the exe's checks, in its order and with its messages."""
    prefix = _s(body.get("prefix")).upper()
    months = [_s(m).upper() for m in (body.get("months") or [])]
    months += [""] * (MONTHS - len(months))
    cells = [_s(c) for c in (body.get("format") or [])]
    if prefix in ("M", "D"):
        seen = set()
        for m in months[:MONTHS]:
            if m and m in seen:
                raise HTTPException(422, f"Duplicate Prefix '{m}'!!")
            seen.add(m)
        if any(m == "" for m in months[:MONTHS]):
            raise HTTPException(422, "Please fill the default Styles ")
        if "1" not in cells:
            raise HTTPException(422, "Please select 1 as prefix if month or daywise is selected.")
    if not any(token_of(c) in (7, 8) for c in cells):   # not in the exe, which would then give every bill the same number
        raise HTTPException(422, "Please select 7 or 8 (the running number) in the document number.")
    f12 = [_s(body.get(k)) for k, _ in F12_NAMES]
    if f12[0] == f12[1] or f12[0] == f12[2] or f12[1] == f12[2]:
        raise HTTPException(422, "Please select  distinct Invoice value")


def _set(cur, name: str, value: str) -> None:
    """UpdateInv: Update UtlGSettings set SetValue ... where SetGroup = 'Invoice' and SetName = ... (a missing row is added)."""
    changed = cur.execute("Update UtlGSettings set SetValue = ? where SetGroup = 'Invoice' and SetName = ?", (value[:250], name))
    if not changed:
        cur.execute("INSERT INTO UtlGSettings (SetGroup, SetName, SetValue, SetCaption, Remarks, HideSetting, DropDownList, AddnlInterface, "
                    "Mandatory, DefaultValue, MaxWidth, DataType) VALUES ('Invoice', ?, ?, ?, '', 'N', 'N', 'N', 'N', '', 0, '')", (name, value[:250], name))


def save(body: dict, user: dict) -> dict:
    """saveDetails: everything in one transaction; any failure rolls it all back."""
    _need_db()
    check(body)
    prefix = _s(body.get("prefix")).upper()
    prefix = prefix if prefix in ("M", "D") else ""
    months = [_s(m).upper()[:1] for m in (body.get("months") or [])]
    months = (months + [""] * MONTHS)[:MONTHS]
    cells = [_s(c) for c in (body.get("format") or [])]
    sms = body.get("sms") or {}
    texts = {k: str(sms.get(k) or "") for k in ("sales_msg", "cancel_msg", "delete_msg")}
    if any("'" in t for t in texts.values()):
        raise HTTPException(422, "A message can't hold the ' character")
    try:
        value = float(sms.get("sales_value") or 0)
    except (TypeError, ValueError):
        raise HTTPException(422, "Value must be a number") from None
    if value < 0 or value > 1e12:
        raise HTTPException(422, "Value must be a number")
    flag = lambda v: "Y" if v else "N"  # noqa: E731
    misc = db.fetchone("Select * from UtlMisc") or {}
    has_misc = "InvSMSblnSales" in misc and "RateFld" in misc   # a database without these columns keeps its UtlMisc as it is
    with db.direct_writes(), db.transaction() as cur:
        _set(cur, "InvNoReset", prefix)
        _set(cur, "CounterWsRst", flag(body.get("counter_reset")))
        _set(cur, "DocNoFmt", encode_format(cells))
        _set(cur, "PrefixSettings", ",".join(months) if prefix else "")
        for key, name in F12_NAMES:
            if _s(body.get(key)):   # a blank choice is left as it is
                code = CODE_OF.get(_s(body.get(key)))
                if not code:
                    raise HTTPException(422, "Choose Save, Save and Print or Save and Preview")
                _set(cur, name, code)
        if has_misc:
            cur.execute("update UtlMisc set InvSMSblnSales = ?, InvSMSblnCancel = ?, InvSMSblnDelete = ?, InvSMSSalesvalue = ?, InvSMSSalesMSg = ?, "
                        "InvSMSCancelMsg = ?, InvSMSDeleteMsg = ?, RateFld = ?",
                        (flag(sms.get("on_sales")), flag(sms.get("on_cancel")), flag(sms.get("on_delete")), value, texts["sales_msg"], texts["cancel_msg"],
                         texts["delete_msg"], flag(body.get("update_rate"))))
    from . import designer, gsettings, invoice_numbers
    invoice_numbers.reset_cache()   # the next bill follows the new format at once
    designer.load()   # UtlMisc again: the SMS sender and the rate switch read it
    gsettings.refresh_from_db(0)
    admin_audit.write_now(user["user"], SCREEN, "Saved", "Invoice Settings", "", f"number format {encode_format(cells)}")
    return {"message": "Saved Successfully."}
