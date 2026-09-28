"""Customised Reports → Email (POST /reports/custom/email, app/mailer.py). No real mail is sent: smtplib is faked."""
from datetime import date, timedelta

import pytest

from app import mailer, store
from app.routers import custom_reports

TODAY = date.today()
BODY = {"name": "Sales - Day Wise Report", "from_date": (TODAY - timedelta(days=6)).isoformat(),
        "to_date": TODAY.isoformat(), "to": ["owner@example.com"]}


class FakeSMTP:
    sent: list = []

    def __init__(self, host, port, timeout=None, context=None):
        self.host, self.port = host, port
        self.calls = []

    def __enter__(self):
        return self

    def __exit__(self, *exc):
        return False

    def starttls(self, context=None):
        self.calls.append("starttls")

    def login(self, user, password):
        self.calls.append(("login", user))

    def send_message(self, msg):
        FakeSMTP.sent.append((self, msg))


@pytest.fixture
def smtp(monkeypatch):
    FakeSMTP.sent = []
    monkeypatch.setattr(custom_reports, "_email_log", custom_reports.defaultdict(list))
    monkeypatch.setenv("SMTP_HOST", "mail.example.com")
    monkeypatch.setenv("SMTP_USER", "reports@example.com")
    monkeypatch.setenv("SMTP_PASSWORD", "not-a-real-password")
    monkeypatch.setattr(mailer.smtplib, "SMTP", FakeSMTP)
    return FakeSMTP


async def test_without_smtp_settings_it_says_so(client, monkeypatch):
    monkeypatch.delenv("SMTP_HOST", raising=False)

    res = await client.post("/reports/custom/email", json=BODY)

    assert res.status_code == 503
    assert "SMTP" in res.json()["detail"]


async def test_sends_the_report_as_xls(client, smtp):
    res = await client.post("/reports/custom/email", json={**BODY, "message": "Weekly figures"})

    assert res.status_code == 200, res.text
    assert res.json()["sent_to"] == ["owner@example.com"] and res.json()["rows"] > 0
    server, msg = smtp.sent[0]
    assert (server.host, server.port) == ("mail.example.com", 587)
    assert server.calls == ["starttls", ("login", "reports@example.com")]
    assert msg["To"] == "owner@example.com" and "Sales - Day Wise Report" in msg["Subject"]
    company = store.COMPANY
    assert msg["From"] == f'{company["name"]} <{company["email"]}>'   # the Company Master email
    assert msg["Reply-To"] == company["email"]
    (att,) = list(msg.iter_attachments())
    assert att.get_filename().endswith(".xls")
    xml = att.get_content()
    assert b"<Workbook" in xml and b"Sales - Day Wise Report" in xml
    assert "Weekly figures" in msg.get_body().get_content()


@pytest.mark.parametrize("to", [[], ["not-an-email"], ["a@b.com\r\nBcc: x@y.com"], ["a@b.com"] * 6])
async def test_bad_recipients_are_refused(client, smtp, to):
    res = await client.post("/reports/custom/email", json={**BODY, "to": to})

    assert res.status_code == 422
    assert smtp.sent == []


async def test_empty_period_is_not_sent(client, smtp):
    res = await client.post("/reports/custom/email", json={**BODY, "from_date": "2001-01-01", "to_date": "2001-01-02"})

    assert res.status_code == 422
    assert smtp.sent == []


async def test_mail_server_error_is_reported(client, smtp, monkeypatch):
    def refuse(self, msg):
        raise OSError("connection refused")
    monkeypatch.setattr(FakeSMTP, "send_message", refuse)

    res = await client.post("/reports/custom/email", json=BODY)

    assert res.status_code == 502
    assert "not-a-real-password" not in res.text


async def test_needs_the_reports_module(cashier_client, smtp):
    res = await cashier_client.post("/reports/custom/email", json=BODY)

    assert res.status_code == 403


async def test_at_most_a_few_reports_an_hour(client, smtp, monkeypatch):
    monkeypatch.setattr(custom_reports, "EMAIL_LIMIT", 2)

    codes = [(await client.post("/reports/custom/email", json=BODY)).status_code for _ in range(3)]

    assert codes == [200, 200, 429]
    assert len(smtp.sent) == 2


async def test_from_falls_back_to_smtp_from_without_a_company_email(client, smtp, monkeypatch):
    monkeypatch.setitem(store.COMPANY, "email", "")
    monkeypatch.setenv("SMTP_FROM", "reports@example.com")

    await client.post("/reports/custom/email", json=BODY)

    msg = smtp.sent[0][1]
    assert msg["From"].endswith("<reports@example.com>") and msg["Reply-To"] is None
