"""Print Settings: the desktop's Print Option Name checks (add / edit), the listing's e-mail and multi-print details, and
Remove Duplicate Invoice's exact invoice-number lookup. The SQL writes need the shop's database, so they are not run here."""
import pytest
from fastapi import HTTPException

from app import duplicate_invoices, print_settings, store


def _new(**over):
    base = {"name": "Tax Invoice", "rpt": "inv.rpt", "selection": "", "copies": 1, "default": False, "inactive": False, "click": "P",
            "ctrl_click": "", "ctrl_alt_click": "", "after_save": "DNP", "printer": "", "email_routing": "S", "email_subject": "", "email_body": "", "email_footer": "", "email_cond": "", "attach_copy": False,
            "allow_date_range": False, "allow_docno_range": False, "track_printed": False, "multi_query": ""}
    return {**base, **over}


@pytest.fixture
def options(monkeypatch):
    rows = [{"DocID": 5, "OptionOrder": 1, "OptionName": "Existing", "ClickOperation": "P", "NoOfCopies": 1, "EmailRouting": "S",
             "AllowDateRange": "Y", "MultiPrintQuery": "select 1"}]
    monkeypatch.setattr(store, "UTL_OPTIONS", rows)
    monkeypatch.setattr(store, "UTL_OPTION_EMAILS", [
        {"DocID": 5, "OptionOrder": 1, "Slno": 2, "FixedEmail": "N", "eMailType": "C", "EmailID": "", "TxnFldName": "CustEmail", "TxnCondition": "x"},
        {"DocID": 5, "OptionOrder": 1, "Slno": 1, "FixedEmail": "Y", "eMailType": "T", "EmailID": "a@b.com", "TxnFldName": "", "TxnCondition": ""}])
    return rows


def test_listing_carries_email_and_multi_print_details(options):
    opt = print_settings.listing()["documents"][0]["options"][0]
    assert opt["allow_date_range"] is True and opt["multi_query"] == "select 1"
    assert [(e["type"], e["fixed"]) for e in opt["emails"]] == [("T", True), ("C", False)]   # in Slno order


def test_main_click_is_required():
    with pytest.raises(HTTPException) as e:
        print_settings._check(_new(click=""), 5, None)
    assert e.value.detail == "Invalid Print Click...."


def test_duplicate_option_name_message(options):
    with pytest.raises(HTTPException) as e:
        print_settings._check(_new(name="existing"), 5, None)
    assert e.value.detail == "option Name Already Exist!"
    print_settings._check(_new(name="existing"), 5, options[0])   # the option itself may keep its name


def test_email_columns_only_kept_when_a_click_emails():
    plain = print_settings._values({**_new(), "email_subject": "S", "attach_copy": True, "allow_date_range": True, "allow_docno_range": False,
                                    "track_printed": False, "multi_query": "q"})
    assert plain[10] == "N" and plain[12] == "" and plain[16] == "N" and plain[19] == ""
    mail = print_settings._values({**_new(click="E"), "email_subject": "S", "attach_copy": True, "allow_date_range": False,
                                   "allow_docno_range": False, "track_printed": False, "multi_query": ""})
    assert mail[10] == "S" and mail[11] == "Y" and mail[12] == "S"


def test_duplicate_invoice_number_is_exact_and_ignores_dates(monkeypatch):
    from datetime import date
    doc = {"doc_no": "26/c1-0000001", "date": "2020-01-01", "status": "SAVED", "lines": [], "totals": {}}
    monkeypatch.setitem(store.DOCS, "sales_invoice", {doc["doc_no"]: doc})
    rows = duplicate_invoices.listing(date(2026, 1, 1), date(2026, 1, 2), "26/C1-0000001")
    assert [r["doc_no"] for r in rows] == [doc["doc_no"]]
    for part in ("0000001", "26/c1-000000"):
        with pytest.raises(HTTPException) as e:
            duplicate_invoices.listing(date(2026, 1, 1), date(2026, 1, 2), part)
        assert e.value.detail == "Invalid Invoice..."


def test_transaction_settings_follow_the_desktop_row(monkeypatch):
    from app import doc_settings
    row = {"DocID": 9, "DocName": "SALES INVOICE", "DocType": "Transaction", "BlnAllowDuplicateItems": "N", "AllowEditDocDate": "N",
           "blnFreezeDoc": "Y", "AllowEditDeleteForDays": 3, "AuthReq": "Y", "NoOfAuth": 2, "NoOfAuthQueryCond": "x"}
    monkeypatch.setitem(store.UTL_DOCS, 9, row)
    s = doc_settings.settings("sales_invoice")
    assert (s["allow_duplicate_items"], s["allow_edit_date"], s["edit_days"]) == (False, False, 3)
    row["blnFreezeDoc"] = "N"
    assert doc_settings.settings("sales_invoice")["edit_days"] == 0   # the days count only while the freeze flag is on
    cols = doc_settings.web_columns("sales_invoice", {**doc_settings.DEFAULTS, "edit_days": 5}, row)
    assert cols["blnFreezeDoc"] == "Y" and cols["AllowEditDeleteForDays"] == 5 and "AllowCopyFrom" not in cols


def test_desktop_switch_off_clears_its_details(monkeypatch):
    from app import doc_settings
    monkeypatch.setitem(store.UTL_DOCS, 9, {"DocID": 9, "DocName": "X", "AuthReq": "Y", "NoOfAuth": 2, "NoOfAuthQueryCond": "x"})
    assert doc_settings.desktop_updates(9, {"AuthReq": False}) == {"AuthReq": "N", "NoOfAuth": 0, "NoOfAuthQueryCond": ""}
    with pytest.raises(HTTPException):
        doc_settings.desktop_updates(9, {"Bogus": True})
