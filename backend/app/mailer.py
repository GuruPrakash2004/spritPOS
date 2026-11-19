"""Sends a report by email as an .xlsx attachment (Customised Reports → Email).

SMTP settings come only from backend/.env (never committed):
    SMTP_HOST, SMTP_PORT (default 587), SMTP_USER, SMTP_PASSWORD (or SMTP_PASSWORD_ENC, the same sealed: what the sign-in
    page's Email (SMTP) settings write, routers/setup.py),
    SMTP_FROM, SMTP_SECURITY = starttls (default) | ssl | none
The From address is the Company Master email (with the company name), then SMTP_FROM, then SMTP_USER.
Most providers (Gmail included) only send as the account you log in with or a verified "Send mail as"
alias of it, so SMTP_USER should be the company mailbox. Replies go to the company email either way.
Without SMTP_HOST, configured() is False and the endpoint says email isn't set up.
The workbook is a real .xlsx, the same format as the browser's Export to Excel (frontend/src/lib/xlsExport.js).
"""
import io
import os
import smtplib
import ssl
import zipfile
from email.message import EmailMessage
from email.utils import formataddr
from xml.sax.saxutils import escape

from . import db  # noqa: F401  (importing db loads backend/.env)
from . import secure_fields


def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


def password() -> str:
    """The SMTP password: the sealed one saved from the sign-in page's settings, else a hand-written SMTP_PASSWORD."""
    sealed = _env("SMTP_PASSWORD_ENC")
    if sealed:
        try:
            return secure_fields.unseal(sealed)
        except Exception:
            return ""
    return _env("SMTP_PASSWORD")


def check(host: str, port: int, security: str, user: str, pw: str) -> None:
    """Connect (and sign in when a user is given) without sending anything; raises on any SMTP / network error."""
    context = _tls_context()
    server = smtplib.SMTP_SSL(host, port, timeout=15, context=context) if security == "ssl" else smtplib.SMTP(host, port, timeout=15)
    with server:
        if security == "starttls":
            server.starttls(context=context)
        if user:
            server.login(user, pw)


def configured() -> bool:
    return bool(_env("SMTP_HOST"))


def _col(i: int) -> str:
    name, n = "", i + 1
    while n:
        n, r = divmod(n - 1, 26)
        name = chr(65 + r) + name
    return name


def _clean(v) -> str:
    """Text for XML: characters XML 1.0 can't carry are dropped."""
    text = "" if v is None else str(v)
    return escape("".join(ch for ch in text if ch >= " " or ch in (chr(9), chr(10), chr(13))), {chr(34): "&quot;"})


def _cell(v, r: int, c: int, style: int = 0) -> str:
    ref, s = f"{_col(c)}{r}", f' s="{style}"' if style else ""
    if v is None or v == "":
        return f'<c r="{ref}"{s}/>' if style else ""
    if isinstance(v, (int, float)) and not isinstance(v, bool):
        return f'<c r="{ref}"{s}><v>{v}</v></c>'
    return f'<c r="{ref}"{s} t="inlineStr"><is><t xml:space="preserve">{_clean(v)}</t></is></c>'


_XML = '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>' + chr(10)
_NS = "http://schemas.openxmlformats.org/spreadsheetml/2006/main"
# House style, as the browser's export (xlsExport.js). Style ids: 1 company, 2 subtitle, 3 header (white bold on burgundy)
_STYLES = (_XML + f'<styleSheet xmlns="{_NS}"><fonts count="4"><font><sz val="11"/><name val="Calibri"/></font>'
           '<font><b/><sz val="16"/><name val="Calibri"/></font><font><b/><sz val="12"/><name val="Calibri"/></font>'
           '<font><b/><sz val="11"/><color rgb="FFFFFFFF"/><name val="Calibri"/></font></fonts>'
           '<fills count="4"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill>'
           '<fill><patternFill patternType="solid"><fgColor rgb="FFF5E9C8"/><bgColor indexed="64"/></patternFill></fill>'
           '<fill><patternFill patternType="solid"><fgColor rgb="FF7B1E22"/><bgColor indexed="64"/></patternFill></fill></fills>'
           '<borders count="1"><border><left/><right/><top/><bottom/><diagonal/></border></borders>'
           '<cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>'
           '<cellXfs count="4"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>'
           '<xf numFmtId="0" fontId="1" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf>'
           '<xf numFmtId="0" fontId="2" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf>'
           '<xf numFmtId="0" fontId="3" fillId="3" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf></cellXfs>'
           '<cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles></styleSheet>')


def xlsx(title: str, subtitle: str, columns: list[dict], rows: list[dict]) -> bytes:
    """The report as a real .xlsx workbook in the same house style as the browser's Export to Excel: the company name and the
    title with the period on cream (merged across the width), a burgundy header row kept in view, then the data."""
    company, sep = title.split(" - ", 1) if " - " in title else ("", title)
    top = [[company], [f"{sep} ({subtitle.removeprefix('Period: ')})" if subtitle else sep]] if company else [[title]] + ([[subtitle]] if subtitle else [])
    lines = top + [[c["label"] for c in columns]] + [[r.get(c["key"]) for c in columns] for r in rows]
    styles = {i + 1: (1 if i == 0 else 2) for i in range(len(top))}
    head = len(top) + 1
    styles[head] = 3
    heights = {i: 24 for i in range(1, len(top) + 1)}
    heights[head] = 22
    across = max(1, len(columns))
    data = ""
    for i, vals in enumerate(lines, start=1):
        ht = f' ht="{heights[i]}" customHeight="1"' if i in heights else ""
        style = styles.get(i, 0)
        data += f'<row r="{i}"{ht}>' + "".join(_cell(v, i, c, style) for c, v in enumerate(vals)) + "</row>"
    widths = [min(max(len(str(c["label"])), 8, *(len(str(r.get(c["key"]) if r.get(c["key"]) is not None else "")) for r in rows[:200])), 50) + 2
              for c in columns]
    cols = "<cols>" + "".join(f'<col min="{i}" max="{i}" width="{w}" customWidth="1"/>' for i, w in enumerate(widths, start=1)) + "</cols>"
    merges = [f"A{i}:{_col(across - 1)}{i}" for i in range(1, len(top) + 1)] if across > 1 else []
    merge = f'<mergeCells count="{len(merges)}">' + "".join(f'<mergeCell ref="{m}"/>' for m in merges) + "</mergeCells>" if merges else ""
    pane = (f'<sheetViews><sheetView workbookViewId="0"><pane ySplit="{head}" topLeftCell="A{head + 1}" activePane="bottomLeft" state="frozen"/>'
            '<selection pane="bottomLeft"/></sheetView></sheetViews>')
    sheet = _XML + f'<worksheet xmlns="{_NS}">{pane}<sheetFormatPr defaultRowHeight="15"/>{cols}<sheetData>{data}</sheetData>{merge}</worksheet>'
    rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"
    parts = {
        "[Content_Types].xml": _XML + '<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">'
            '<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>'
            '<Default Extension="xml" ContentType="application/xml"/>'
            '<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>'
            '<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>'
            '<Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/></Types>',
        "_rels/.rels": _XML + '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'
            f'<Relationship Id="rId1" Type="{rel}/officeDocument" Target="xl/workbook.xml"/></Relationships>',
        "xl/workbook.xml": _XML + f'<workbook xmlns="{_NS}" xmlns:r="{rel}"><sheets><sheet name="Report" sheetId="1" r:id="rId1"/></sheets></workbook>',
        "xl/_rels/workbook.xml.rels": _XML + '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'
            f'<Relationship Id="rId1" Type="{rel}/worksheet" Target="worksheets/sheet1.xml"/>'
            f'<Relationship Id="rId2" Type="{rel}/styles" Target="styles.xml"/></Relationships>',
        "xl/styles.xml": _STYLES,
        "xl/worksheets/sheet1.xml": sheet,
    }
    buf = io.BytesIO()
    with zipfile.ZipFile(buf, "w", zipfile.ZIP_DEFLATED) as z:
        for name, text in parts.items():
            z.writestr(name, text)
    return buf.getvalue()


def _tls_context() -> ssl.SSLContext:
    """Verified TLS. Uses certifi's CA list when installed: the python.org macOS build ships without
    system CA certificates, so the default context fails with SSLCertVerificationError."""
    try:
        import certifi
        return ssl.create_default_context(cafile=certifi.where())
    except ImportError:
        return ssl.create_default_context()


def send(to: list[str], subject: str, text: str, file_name: str, attachment: bytes,
         company_email: str = "", company_name: str = "", subtype: str = "vnd.openxmlformats-officedocument.spreadsheetml.sheet") -> None:
    """Send one message; raises on any SMTP error (the caller turns it into a 502)."""
    address = company_email.strip() or _env("SMTP_FROM") or _env("SMTP_USER")
    msg = EmailMessage()
    msg["From"] = formataddr((company_name, address)) if company_name else address
    msg["To"], msg["Subject"] = ", ".join(to), subject
    if company_email.strip():
        msg["Reply-To"] = company_email.strip()
    msg.set_content(text)
    msg.add_attachment(attachment, maintype="application", subtype=subtype, filename=file_name)

    host, port = _env("SMTP_HOST"), int(_env("SMTP_PORT", "587") or 587)
    security = _env("SMTP_SECURITY", "starttls").lower()
    context = _tls_context()
    if security == "ssl":
        server = smtplib.SMTP_SSL(host, port, timeout=20, context=context)
    else:
        server = smtplib.SMTP(host, port, timeout=20)
    with server:
        if security == "starttls":
            server.starttls(context=context)
        if _env("SMTP_USER"):
            server.login(_env("SMTP_USER"), password())
        server.send_message(msg)
