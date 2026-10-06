"""Sends a report by email as an .xls attachment (Customised Reports → Email).

SMTP settings come only from backend/.env (never committed):
    SMTP_HOST, SMTP_PORT (default 587), SMTP_USER, SMTP_PASSWORD,
    SMTP_FROM, SMTP_SECURITY = starttls (default) | ssl | none
The From address is the Company Master email (with the company name), then SMTP_FROM, then SMTP_USER.
Most providers (Gmail included) only send as the account you log in with or a verified "Send mail as"
alias of it, so SMTP_USER should be the company mailbox. Replies go to the company email either way.
Without SMTP_HOST, configured() is False and the endpoint says email isn't set up.
The workbook is SpreadsheetML 2003, the same format as the browser's Export XLS (frontend/src/lib/xlsExport.ts).
"""
import os
import smtplib
import ssl
from email.message import EmailMessage
from email.utils import formataddr
from xml.sax.saxutils import escape

from . import db  # noqa: F401  (importing db loads backend/.env)


def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


def configured() -> bool:
    return bool(_env("SMTP_HOST"))


def _cell(v, style: str = "") -> str:
    s = f' ss:StyleID="{style}"' if style else ""
    if isinstance(v, (int, float)) and not isinstance(v, bool):
        return f'<Cell{s}><Data ss:Type="Number">{v}</Data></Cell>'
    return f'<Cell{s}><Data ss:Type="String">{escape("" if v is None else str(v), {chr(34): "&quot;"})}</Data></Cell>'


def xls(title: str, subtitle: str, columns: list[dict], rows: list[dict]) -> bytes:
    body = "\n".join(f"<Row>{''.join(_cell(r.get(c['key'])) for c in columns)}</Row>" for r in rows)
    return f"""<?xml version="1.0"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
<Styles><Style ss:ID="h"><Font ss:Bold="1"/></Style><Style ss:ID="t"><Font ss:Bold="1" ss:Size="14"/></Style></Styles>
<Worksheet ss:Name="Report"><Table>
<Row>{_cell(title, 't')}</Row>
<Row>{_cell(subtitle)}</Row>
<Row/>
<Row>{''.join(_cell(c['label'], 'h') for c in columns)}</Row>
{body}
</Table></Worksheet></Workbook>""".encode()


def _tls_context() -> ssl.SSLContext:
    """Verified TLS. Uses certifi's CA list when installed: the python.org macOS build ships without
    system CA certificates, so the default context fails with SSLCertVerificationError."""
    try:
        import certifi
        return ssl.create_default_context(cafile=certifi.where())
    except ImportError:
        return ssl.create_default_context()


def send(to: list[str], subject: str, text: str, file_name: str, attachment: bytes,
         company_email: str = "", company_name: str = "", subtype: str = "vnd.ms-excel") -> None:
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
            server.login(_env("SMTP_USER"), _env("SMTP_PASSWORD"))
        server.send_message(msg)
