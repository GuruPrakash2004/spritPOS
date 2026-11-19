"""Sales invoice numbers from Invoice Settings (frmTrnInv.getInvNo): the tokens, the month / day / counter restarts, and "no change by default"."""
from datetime import date

import pytest

from app import invoice_numbers as inv
from app import store

ST = {"reset": "", "counter": False, "format": inv.DEFAULT_FORMAT, "months": list("ABCDEFGHIJKL")}


def bill(counter="counter10", on=None, item="VIN-750"):
    return {"date": (on or date.today()).isoformat(), "counter": counter, "salesperson": "GURU", "lines": [{"item_code": item, "bottles": 1}]}


@pytest.fixture
def use(monkeypatch):
    def _use(**kw):
        monkeypatch.setattr(inv, "settings", lambda max_age=5.0: ST | kw)
    return _use


@pytest.fixture(autouse=True)
def company_year():
    store.COMPANY.update({"fy_start": "2026-04-01", "fy_end": "2027-03-31"})


# ---------- the number maker ----------
@pytest.mark.parametrize(("fmt", "reset", "n", "expect"), [
    ("<<5>>/<<2>>-<<7>>", "", 1, "26/c10-0000001"),
    ("<<1>>/<<5>>-<<7>>", "M", 12, "B/26-0000012"),          # month letter of Feb 2027
    ("<<1>>-<<8>>", "D", 7, "B14-7"),                         # day wise: letter then the day
    ("<<3>>/<<4>>/<<2>>//<<7>>", "", 3, "2026/2027/c10/0000003"),
    ("<<6>>,<<5>>", "", 1, "2726"),
    ("INV--<<8>>", "", 42, "INV-42"),
])
def test_tokens_and_cleanups(fmt, reset, n, expect):
    assert inv.build(ST | {"format": fmt, "reset": reset}, n, "c10", date(2027, 2, 14)) == expect


def test_a_month_without_a_letter_gives_an_empty_prefix():
    assert inv.build(ST | {"format": "<<1>>x<<8>>", "reset": "M", "months": []}, 5, "c", date(2026, 6, 1)) == "x5"


def test_default_settings_are_recognised():
    assert inv.is_default(ST)
    assert not inv.is_default(ST | {"counter": True})
    assert not inv.is_default(ST | {"reset": "M"})
    assert not inv.is_default(ST | {"format": "<<1>><<7>>"})


# ---------- bills in the API ----------
async def test_default_settings_number_bills_as_before(client, use):
    use()

    a = (await client.post("/transactions/sales-invoice", json=bill())).json()
    b = (await client.post("/transactions/sales-invoice", json=bill())).json()

    assert a["doc_no"].startswith(f"{store.FY_PREFIX}/c10-") and int(b["doc_no"][-7:]) == int(a["doc_no"][-7:]) + 1


async def test_a_custom_format_numbers_bills_and_remembers_the_running_number(client, use):
    use(format="<<1>><<5>>-<<2>>-<<7>>", reset="M")

    a = (await client.post("/transactions/sales-invoice", json=bill(on=date(date.today().year, date.today().month, 1)))).json()
    b = (await client.post("/transactions/sales-invoice", json=bill())).json()

    letter = "ABCDEFGHIJKL"[date.today().month - 1]
    assert a["doc_no"] == f"{letter}26-c10-0000001" and b["doc_no"] == f"{letter}26-c10-0000002"
    assert (a["inv_cno"], b["inv_cno"]) == (1, 2)


async def test_month_wise_restarts_each_month_and_counter_wise_each_counter(client, use):
    use(format="<<1>>-<<2>>-<<7>>", reset="M", counter=True)
    d1, d2 = date(2026, 5, 10), date(2026, 6, 10)

    nos = []
    for c, d in (("counter10", d1), ("counter10", d1), ("counter11", d1), ("counter10", d2)):
        nos.append((await client.post("/transactions/sales-invoice", json=bill(c, d))).json()["doc_no"])

    assert nos == ["E-c10-0000001", "E-c10-0000002", "E-c11-0000001", "F-c10-0000001"]


async def test_day_wise_prefix_carries_the_day(client, use):
    use(format="<<1>>/<<7>>", reset="D")

    res = (await client.post("/transactions/sales-invoice", json=bill(on=date(2026, 7, 9)))).json()

    assert res["doc_no"] == "G9/0000001"


async def test_counter_wise_reset_needs_the_counter_in_the_format(client, use):
    use(format="<<5>>-<<7>>", counter=True)

    res = await client.post("/transactions/sales-invoice", json=bill())

    assert res.status_code == 422 and res.json()["detail"].startswith("Counter wise reset is enabled in global settings!")


async def test_the_preview_follows_the_format(client, use):
    use(format="<<2>>/<<8>>")

    res = (await client.get("/transactions/next-number", params={"kind": "sales_invoice", "counter": "counter11"})).json()

    assert res["doc_no"] == "c11/1" and res["number"] == 1
    await client.post("/transactions/sales-invoice", json=bill("counter11"))
    assert (await client.get("/transactions/next-number", params={"kind": "sales_invoice", "counter": "counter11"})).json()["doc_no"] == "c11/2"


async def test_a_released_held_bill_takes_the_next_number_in_the_format(client, use):
    use(format="<<2>>/<<8>>")
    held = (await client.post("/transactions/sales-invoice", json=bill("counter10") | {"hold": True})).json()

    done = (await client.post(f"/transactions/sales-invoice/{held['doc_no']}/release")).json()

    assert done["doc_no"] == "c10/1" and done["inv_cno"] == 1


def test_a_database_running_number_is_the_exes_query(monkeypatch):
    seen = {}
    monkeypatch.setattr(inv.sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(inv.db, "use_web_tables", lambda: False)
    monkeypatch.setattr(inv.db, "fetchone", lambda sql, params=(): seen.update(sql=sql, params=params) or {"n": 41})

    n = inv.running_number(ST | {"reset": "D", "counter": True}, {"code": "counter10", "_scid": "4"}, date(2026, 8, 3))

    assert n == 42
    assert seen["sql"] == "SELECT MAX(InvCNo) AS n FROM dbo.TrnHdrSINV WITH (NOLOCK) WHERE FinYear = ? AND MONTH(SINVDate) = ? AND DAY(SINVDate) = ? AND SCID = ?"
    assert seen["params"] == (int(store.FY_PREFIX), 8, 3, 4)


def test_the_header_takes_the_running_number_from_the_bill():
    from app.sqlstore import _finyear_and_running_no
    assert _finyear_and_running_no("E-c10-xyz", 7) == {"FINYEAR": int(store.FY_PREFIX), "InvCNo": 7}


def test_a_stored_format_without_a_running_number_is_not_used(monkeypatch):
    monkeypatch.setattr(inv.sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(inv.db, "fetchall", lambda sql, p=(): [{"SetName": "DocNoFmt", "SetValue": "<<5>>/<<6>>"}])
    inv.reset_cache()

    assert inv.settings(0)["format"] == inv.DEFAULT_FORMAT
    inv.reset_cache()


def test_a_failing_number_query_falls_back_to_the_bills_made_here(monkeypatch):
    monkeypatch.setattr(inv.sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(inv.db, "use_web_tables", lambda: False)
    monkeypatch.setattr(inv.db, "fetchone", lambda sql, params=(): (_ for _ in ()).throw(RuntimeError("no InvCNo column")))

    assert inv.running_number(ST, {"code": "counter10", "_scid": "1"}, date(2026, 8, 3)) >= 1


def test_the_running_number_counts_within_the_financial_year():
    store.DOCS["sales_invoice"]["old"] = {"date": "2025-12-01", "status": "SAVED", "inv_cno": 500, "counter": "counter10"}
    store.DOCS["sales_invoice"]["now"] = {"date": "2026-12-01", "status": "SAVED", "inv_cno": 7, "counter": "counter10"}

    assert inv.running_number(ST, {"code": "counter10", "_scid": "1"}, date(2026, 12, 2)) == 8
