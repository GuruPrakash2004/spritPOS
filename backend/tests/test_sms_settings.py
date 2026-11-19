"""SMS settings (frmUtlSendSMS provider settings, frmUtlInvSMS invoice SMS settings): the desktop's checks and messages.
The SQL writes need the shop's database, so only the validation and the Test SMS pre-checks run here."""
import pytest
from fastapi import HTTPException

from app import sms, store


@pytest.fixture
def providers(monkeypatch):
    rows = [{"id": 1, "name": "Gateway", "daily_limit": 100, "default": True, "url": "http://x/?m=<<MOBNO>>&t=<<MSG>>"}]
    monkeypatch.setattr(sms, "provider_rows", lambda: rows)
    return rows


@pytest.mark.parametrize("args,message", [
    (("", 10, "http://x", 0), "Please enter Provider!"),
    (("A", "", "http://x", 0), "Please enter Daily Limit of messages!"),
    (("A", 10, " ", 0), "Please enter SMS URL!"),
])
def test_provider_required_fields_in_the_desktops_order(providers, args, message):
    with pytest.raises(HTTPException) as e:
        sms.check_provider(*args)
    assert e.value.detail == message


def test_provider_name_must_be_unique_but_may_keep_its_own(providers):
    with pytest.raises(HTTPException) as e:
        sms.check_provider("gateway", 10, "http://x", 0)
    assert e.value.detail == "Provider already exists.."
    assert sms.check_provider("Gateway", "50", "http://y", 1) == ("Gateway", 50, "http://y")


def test_test_sms_prechecks(providers):
    assert sms.test_message(1, "") == (False, "Mobile No. is Empty....")
    assert sms.test_message(1, "12345") == (False, "Invalid Mobile No...")
    assert sms.test_message(1, "98765abcde") == (False, "Invalid Mobile No...")
    assert sms.test_message(9, "9876543210") == (False, "Invalid Provider...")


def test_invoice_settings_read_and_validate(monkeypatch):
    monkeypatch.setattr(store, "UTL_MISC", {"InvSMSblnSales": "Y", "InvSMSblnCancel": "N", "InvSMSblnDelete": "N", "InvMsgMobNo": "9876543210",
                                            "InvSMSSalesvalue": 5000, "InvSMSSalesMSg": "Sold <<SINVNO>>", "InvSMSCancelMsg": "", "InvSMSDeleteMsg": ""})
    s = sms.invoice_settings()
    assert s["on_sales"] is True and s["on_cancel"] is False and s["sales_value"] == 5000 and s["available"] is True
    with pytest.raises(HTTPException) as e:
        sms.save_invoice_settings({"mobiles": "12345"})
    assert "10 digit" in e.value.detail
    with pytest.raises(HTTPException) as e:
        sms.save_invoice_settings({"sales_msg": "x" * 251})
    assert "250" in e.value.detail
    monkeypatch.setattr(store, "UTL_MISC", {})
    with pytest.raises(HTTPException) as e:
        sms.save_invoice_settings({})
    assert e.value.status_code == 409


def test_merge_text_fills_known_columns_any_case():
    values = {"SINVNO": "26/c1-0000007", "CUSTNAME": "Ravi"}
    assert sms.merge_text("Bill <<SINVNO>> of <<custname>> <<UNKNOWN>>", values) == "Bill 26/c1-0000007 of Ravi <<UNKNOWN>>"


def _setup_event(monkeypatch, **over):
    misc = {"InvSMSblnSales": "Y", "InvSMSblnCancel": "Y", "InvSMSblnDelete": "N", "InvMsgMobNo": "9876543210, 12345,9123456780",
            "InvSMSSalesvalue": 0, "InvSMSSalesMSg": "sold", "InvSMSCancelMsg": "Cancelled <<SINVNO>>", "InvSMSDeleteMsg": "Deleted <<SINVNO>>", **over}
    monkeypatch.setattr(store, "UTL_MISC", misc)
    monkeypatch.setattr(sms, "enabled", lambda: True)
    monkeypatch.setattr(sms, "_merge_fields", lambda no: {"SINVNO": no})
    sent = []
    monkeypatch.setattr(sms, "send_later", lambda text, mobile, provider=None: sent.append((text, mobile)))
    return sent


def test_cancel_messages_each_valid_number_with_the_merged_text(monkeypatch):
    sent = _setup_event(monkeypatch)
    assert sms.invoice_event("cancel", "26/c1-0000007") == 2
    assert sent == [("Cancelled 26/c1-0000007", "9876543210"), ("Cancelled 26/c1-0000007", "9123456780")]   # the short number is skipped


def test_switch_off_or_no_text_sends_nothing_and_sales_is_never_sent(monkeypatch):
    sent = _setup_event(monkeypatch)
    assert sms.invoice_event("delete", "X") == 0          # the delete switch is off
    assert sms.invoice_event("sales", "X") == 0           # the exe's sales SMS is commented out, so it is never sent
    _setup_event(monkeypatch, InvSMSCancelMsg="  ")
    assert sms.invoice_event("cancel", "X") == 0 and not sent


def test_a_failure_never_breaks_the_caller(monkeypatch):
    _setup_event(monkeypatch)
    monkeypatch.setattr(sms, "_merge_fields", lambda no: (_ for _ in ()).throw(RuntimeError("db down")))
    assert sms.invoice_event("cancel", "X") == 0
