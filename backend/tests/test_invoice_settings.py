"""Invoice Settings (frmUtlInvNumberFmt): the number-format strip, the exe's checks and the save. A fake database answers."""
from contextlib import contextmanager

import pytest

from app import db, designer, gsettings, invoice_settings as inv, sqlstore, store

CELLS = ["5", "/", "2", "-", "7"] + [""] * 10


# ---------- the strip ----------
def test_the_default_strip_is_saved_the_way_the_exe_writes_it():
    assert inv.encode_format(CELLS) == "<<5>>/,<<2>>-,<<7>>" + ",,,,,,,,,,"


@pytest.mark.parametrize("stored", ["<<5>>/,<<2>>-,<<7>>" + ",,,,,,,,,,", "<<5>>/<<2>>-<<7>>"])
def test_the_strip_reads_back_from_either_form(stored):
    assert inv.decode_format(stored) == CELLS


def test_a_blank_or_tiny_saved_format_gives_empty_cells():
    assert inv.decode_format("") == [""] * 15
    assert inv.decode_format("x") == [""] * 15


def test_token_eight_is_saved_as_a_token():
    """The exe's `Val < 8` writes it as the text "8" (its number maker does understand <<8>>); the web saves <<8>>."""
    assert inv.encode_format(["8"]) .startswith("<<8>>")
    assert inv.token_of("9") == 0 and inv.token_of("0") == 0 and inv.token_of("A") == 0 and inv.token_of("5/") == 5


def test_a_strip_longer_than_15_cells_keeps_the_first_15():
    assert inv.decode_format("<<" + ">>,<<".join(str(i % 7 + 1) for i in range(20)) + ">>") == [str(i % 7 + 1) for i in range(15)]


# ---------- the checks ----------
def body(**extra):
    base = {"prefix": "", "counter_reset": False, "months": [""] * 12, "format": CELLS, "f12": "Save and Print", "ctrl_f12": "Save", "alt_f12": "Save and Preview",
            "sms": {}, "update_rate": False}
    return base | extra


def test_month_wise_needs_a_filled_unique_prefix_and_a_token_1():
    letters = list("ABCDEFGHIJKL")
    for bad, msg in ((letters[:11] + ["A"], "Duplicate Prefix 'A'!!"), (letters[:11] + [""], "Please fill the default Styles ")):
        with pytest.raises(Exception) as e:
            inv.check(body(prefix="M", months=bad, format=["1"] + [""] * 14))
        assert e.value.detail == msg
    with pytest.raises(Exception) as e:
        inv.check(body(prefix="D", months=letters))
    assert e.value.detail == "Please select 1 as prefix if month or daywise is selected."
    inv.check(body(prefix="M", months=letters, format=["1"] + CELLS[:14]))   # fine


def test_the_three_print_choices_must_differ():
    for kw in ({"ctrl_f12": "Save and Print"}, {"alt_f12": "Save and Print"}, {"alt_f12": "Save"}, {"f12": "", "ctrl_f12": "", "alt_f12": "Save"}):
        with pytest.raises(Exception) as e:
            inv.check(body(**kw))
        assert e.value.detail == "Please select  distinct Invoice value"


# ---------- the screen's endpoints on a fake database ----------
class Fake:
    def __init__(self):
        self.g = {"InvNoReset": "M", "CounterWsRst": "Y", "DocNoFmt": "<<5>>/<<2>>-<<7>>", "PrefixSettings": "A,B,C,D,E,F,G,H,I,J,K,L",
                  "INVF12": "SP", "INVCTRLF12": "S", "INVALTF12": "SPP"}
        self.misc = {"InvSMSblnSales": "Y", "InvSMSblnCancel": "N", "InvSMSblnDelete": "Y", "InvSMSSalesvalue": 1000.0, "InvSMSSalesMSg": "S", "InvSMSCancelMsg": "C",
                     "InvSMSDeleteMsg": "D", "RateFld": "Y"}
        self.sql = []

    def fetchone(self, sql, params=()):
        s = " ".join(sql.split()).lower()
        if s.startswith("select * from utlmisc"):
            return dict(self.misc)
        assert "from utlgsettings" in s, sql
        return {"SetValue": self.g[params[0]]} if params[0] in self.g else None


@pytest.fixture
def fake(monkeypatch):
    f = Fake()

    class Cur:
        def __init__(self):
            self.updates = 0

        def execute(self, sql, params=()):
            f.sql.append((" ".join(sql.split()), params))
            s = sql.lower()
            if s.startswith("update utlgsettings"):
                if params[1] in f.g:
                    f.g[params[1]] = params[0]
                    return 1
                return 0
            if s.startswith("insert into utlgsettings"):
                f.g[params[0]] = params[1]
            if s.startswith("update utlmisc"):
                f.misc.update(dict(zip(["InvSMSblnSales", "InvSMSblnCancel", "InvSMSblnDelete", "InvSMSSalesvalue", "InvSMSSalesMSg", "InvSMSCancelMsg", "InvSMSDeleteMsg", "RateFld"], params)))
            return 1

    @contextmanager
    def tx():
        yield Cur()

    monkeypatch.setattr(sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(db, "fetchone", f.fetchone)
    monkeypatch.setattr(db, "transaction", tx)
    monkeypatch.setattr(designer, "load", lambda: None)
    monkeypatch.setattr(gsettings, "refresh_from_db", lambda *a, **k: None)
    store.COMPANY.update({"fy_start": "2026-04-01", "fy_end": "2027-03-31"})
    return f


async def test_the_screen_opens_with_the_saved_values(client, fake):
    r = (await client.get("/utilities/invoice-settings")).json()

    assert r["prefix"] == "M" and r["counter_reset"] is True and r["months"] == list("ABCDEFGHIJKL")
    assert r["format"] == CELLS
    assert (r["f12"], r["ctrl_f12"], r["alt_f12"]) == ("Save and Print", "Save", "Save and Preview")
    assert r["sms"]["on_sales"] is True and r["sms"]["sales_value"] == 1000 and r["update_rate"] is True
    assert r["sample"] == {"prefix": "JAN", "counter": "C1", "year_from": "2026", "year_to": "2027", "sh_year_from": "26", "sh_year_to": "27", "doc_no": "00001", "sh_doc_no": "1"}


async def test_save_writes_every_setting_in_one_transaction(client, fake):
    res = await client.put("/utilities/invoice-settings", json=body(prefix="D", months=list("JFMAYULGSOND"), counter_reset=True, format=["1", "5", "/", "2", "-", "7"],
                                                                   sms={"on_sales": True, "sales_value": 500, "sales_msg": "Hi", "on_cancel": False, "on_delete": True, "delete_msg": "Bye"},
                                                                   update_rate=True))

    assert res.status_code == 200 and res.json() == {"message": "Saved Successfully."}
    assert fake.g["InvNoReset"] == "D" and fake.g["CounterWsRst"] == "Y"
    assert fake.g["DocNoFmt"] == "<<1>><<5>>/,<<2>>-,<<7>>" + ",,,,,,,,,"
    assert fake.g["PrefixSettings"] == "J,F,M,A,Y,U,L,G,S,O,N,D"
    assert (fake.g["INVF12"], fake.g["INVCTRLF12"], fake.g["INVALTF12"]) == ("SP", "S", "SPP")
    assert fake.misc["RateFld"] == "Y" and fake.misc["InvSMSSalesvalue"] == 500 and fake.misc["InvSMSblnCancel"] == "N" and fake.misc["InvSMSDeleteMsg"] == "Bye"


async def test_no_prefix_clears_the_month_letters(client, fake):
    await client.put("/utilities/invoice-settings", json=body(prefix="", months=list("ABCDEFGHIJKL")))

    assert fake.g["InvNoReset"] == "" and fake.g["PrefixSettings"] == ""


async def test_a_missing_row_is_added(client, fake):
    del fake.g["INVALTF12"]

    await client.put("/utilities/invoice-settings", json=body())

    assert fake.g["INVALTF12"] == "SPP"


async def test_a_blank_print_choice_is_left_as_it_is(client, fake):
    res = await client.put("/utilities/invoice-settings", json=body(f12="Save and Print", ctrl_f12="", alt_f12=""))

    assert res.status_code == 422   # two blanks are equal, as in the exe
    res = await client.put("/utilities/invoice-settings", json=body(f12="Save and Print", ctrl_f12="Save", alt_f12=""))
    assert res.status_code == 200 and fake.g["INVALTF12"] == "SPP"


async def test_checks_run_before_anything_is_written(client, fake):
    res = await client.put("/utilities/invoice-settings", json=body(prefix="M", months=["A"] * 12, format=["1"] + [""] * 14))

    assert res.status_code == 422 and res.json()["detail"] == "Duplicate Prefix 'A'!!"
    assert fake.sql == []


async def test_a_message_cannot_hold_an_apostrophe(client, fake):
    res = await client.put("/utilities/invoice-settings", json=body(sms={"sales_msg": "it's"}))

    assert res.status_code == 422 and fake.sql == []


async def test_only_an_administrator_and_needs_the_database(manager_client, client):
    assert (await manager_client.get("/utilities/invoice-settings")).status_code == 403
    assert (await manager_client.put("/utilities/invoice-settings", json=body())).status_code == 403
    assert (await client.get("/utilities/invoice-settings")).status_code == 503


async def test_a_format_without_the_running_number_is_refused(client, fake):
    res = await client.put("/utilities/invoice-settings", json=body(format=["5", "/", "6"] + [""] * 12))

    assert res.status_code == 422 and res.json()["detail"] == "Please select 7 or 8 (the running number) in the document number."
    assert fake.sql == []
