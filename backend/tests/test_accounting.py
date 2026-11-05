"""Vouchers, quotation and expense written straight into the shop's own tables (fake database: no SQL Server here)."""
import pytest

from app import accounting, db, quotation, store
from conftest import enc


class Cursor:
    """Records the statements of one transaction; SCOPE_IDENTITY / MAX / COLUMNPROPERTY answers are scripted."""
    def __init__(self):
        self.sql, self.n = [], 0
        self._row = (0,)

    def execute(self, sql, params=()):
        self.sql.append((" ".join(sql.split()), params))
        s = sql.upper()
        if "SCOPE_IDENTITY" in s:
            self.n += 1
            self._row = (self.n,)
        elif "MAX(CAST" in s or "COLUMNPROPERTY" in s:
            self._row = (0,)
        return 1

    def fetchone(self):
        return self._row


@pytest.fixture
def fake(monkeypatch):
    cur = Cursor()

    class Tx:
        def __enter__(self):
            return cur

        def __exit__(self, *a):
            return False

    monkeypatch.setattr(db, "transaction", lambda: Tx())
    monkeypatch.setattr(accounting, "_exists", lambda t: True)
    monkeypatch.setattr(accounting, "_columns", lambda t: {"pmtvchno", "prefix", "printpmtvchno", "pmtvchdate", "authorised", "cancelled", "changedby", "addedby",
                                                           "addedon", "narr", "totdr", "totcr", "amtinwords", "chqno", "chqdt"})
    monkeypatch.setattr(accounting, "ledgers", lambda: [])
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [{"AcLedID": i} for i in params])
    monkeypatch.setattr(accounting, "get_voucher", lambda kind, vid: {"id": vid, "doc_no": "26//00001", "total": 100.0})
    store.FY_PREFIX = "26"
    return cur


def body(dr=100, cr=100):
    return {"date": "2026-10-31", "narration": "test", "lines": [{"ledger_id": 1, "side": "Dr", "amount": dr}, {"ledger_id": 2, "side": "Cr", "amount": cr}]}


async def test_unbalanced_voucher_is_refused_and_writes_nothing(client, fake):
    res = await client.post("/accounting/vouchers/payment", json=body(100, 90))
    assert res.status_code == 422 and "NOT matching" in res.json()["detail"]
    assert fake.sql == []


async def test_voucher_writes_header_lines_and_ledger_postings_in_one_transaction(client, fake):
    res = await client.post("/accounting/vouchers/payment", json=body())
    assert res.status_code == 201, res.text
    text = [s for s, _ in fake.sql]
    tables = [next(t for t in ("TrnHdrPMTVCH", "TrnDtlPMTVCH", "TrnHdrAcc", "TrnDtlAcc") if f"INSERT INTO {t} " in s) for s in text if "INSERT INTO" in s]
    assert tables == ["TrnHdrPMTVCH", "TrnDtlPMTVCH", "TrnDtlPMTVCH", "TrnHdrAcc", "TrnDtlAcc", "TrnDtlAcc"]
    hdr = next(p for s, p in fake.sql if "INSERT INTO TrnHdrPMTVCH" in s)
    assert "2600001" in hdr and "26//00001" in hdr and 100.0 in hdr          # stored number, printed number, totals
    acc = next(p for s, p in fake.sql if "INSERT INTO TrnHdrAcc" in s)
    assert "PMTVCH" in acc and "V" in acc


async def test_unknown_voucher_kind_and_missing_rights(client, cashier_client, fake):
    assert (await client.post("/accounting/vouchers/nope", json=body())).status_code == 404
    assert (await cashier_client.post("/accounting/vouchers/payment", json=body())).status_code == 403


def test_next_number_continues_the_year_series():
    cur = Cursor()
    cur._row = (115,)
    store.FY_PREFIX = "26"
    cur.execute = lambda sql, params=(): cur.sql.append((sql, params)) or 1
    assert accounting.next_number(cur, "TrnHdrEXP", "EXPNO") == ("2600116", "26//00116", 116)
    assert accounting.next_number(cur, "TrnHdrEXP", "EXPNO", "A")[:2] == ("26A00116", "26/A/00116")


async def test_general_ledger_keeps_a_running_balance(client, monkeypatch):
    monkeypatch.setattr(accounting, "_need", lambda *t: None)
    answers = iter([{"AcLedName": "Cash"}, {"net": 50}])
    monkeypatch.setattr(db, "fetchone", lambda sql, params=(): next(answers))
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [
        {"DocType": "PMTVCH", "TrnDate": "2026-10-02", "TrnNo": "26//00001", "Narrn": "x", "DrAmt": 20, "CrAmt": 0},
        {"DocType": "RCPTVCH", "TrnDate": "2026-10-03", "TrnNo": "26//00002", "Narrn": "y", "DrAmt": 0, "CrAmt": 70}])
    res = (await client.get("/accounting/general-ledger", params={"ledger_id": 1, "from_date": "2026-10-01", "to_date": "2026-10-31"})).json()
    assert [r["balance"] for r in res["rows"]] == [50, 70, 0] and res["closing"] == 0 and res["ledger"] == "Cash"


async def test_quotation_prices_from_the_mrp_and_writes_header_and_lines(client, monkeypatch, fake):
    monkeypatch.setattr(quotation.accounting, "_columns", lambda t: {"qtnno", "prefix", "printqtnno", "qtndate", "custid", "partyname", "authorised", "cancelled", "changedby", "addedby", "addedon", "totqty", "total", "grndtot"})
    item = next(c for c, i in store.ITEMS.items() if i.get("mrp"))
    store.ITEMS[item]["_itemid"] = "7"
    mrp = store.ITEMS[item]["mrp"]
    monkeypatch.setattr(quotation, "get_quotation", lambda qid: {"id": qid, "total": 0})
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [{"ItemCode": item, "UOMID": 2}])
    res = await client.post("/transactions/quotation", json={"date": "2026-10-31", "customer": "CASH", "lines": [{"item_code": item, "qty": 3}]})
    assert res.status_code == 201, res.text
    det = next(p for s, p in fake.sql if "INSERT INTO TrnDtlQTN" in s)
    assert 3 in det and round(3 * mrp, 2) in det


def _saved(monkeypatch, cancelled="N", day="2026-10-31"):
    """A voucher / quotation that exists, as the header read finds it."""
    monkeypatch.setattr(db, "fetchone", lambda sql, params=(): {"Cancelled": cancelled, "PMTVCHDate": day, "QTNDate": day, "QTNID": 1, "PrintPMTVCHNO": "26//00001"})


async def test_voucher_edit_rewrites_header_lines_and_postings_and_keeps_the_number(client, fake, monkeypatch):
    _saved(monkeypatch)
    res = await client.put("/accounting/vouchers/payment/7", json=body(250, 250))
    assert res.status_code == 200, res.text
    text = [s for s, _ in fake.sql]
    assert any(s.startswith("UPDATE TrnHdrPMTVCH SET") for s in text) and any(s.startswith("DELETE FROM TrnDtlPMTVCH") for s in text)
    assert any(s.startswith("DELETE FROM TrnDtlAcc") for s in text) and sum("INSERT INTO TrnDtlAcc" in s for s in text) == 2
    assert not any("INSERT INTO TrnHdrPMTVCH" in s for s in text)   # no new number


async def test_voucher_edit_checks_the_same_rules_as_a_new_one(client, fake, monkeypatch):
    _saved(monkeypatch)
    assert (await client.put("/accounting/vouchers/payment/7", json=body(5, 6))).status_code == 422
    assert fake.sql == []
    _saved(monkeypatch, cancelled="Y")
    res = await client.put("/accounting/vouchers/payment/7", json=body())
    assert res.status_code == 409 and "cancelled" in res.json()["detail"] and fake.sql == []


async def test_voucher_cancel_only_flags_the_header(client, fake, monkeypatch):
    _saved(monkeypatch)
    res = await client.post("/accounting/vouchers/payment/7/cancel")
    assert res.status_code == 200
    assert len(fake.sql) == 1 and fake.sql[0][0].startswith("UPDATE TrnHdrPMTVCH SET Cancelled = 'Y'")   # postings stay, as in the exe
    _saved(monkeypatch, cancelled="Y")
    assert (await client.post("/accounting/vouchers/payment/7/cancel")).status_code == 409


async def test_a_cashier_cannot_edit_or_cancel_and_the_freeze_date_holds(client, cashier_client, fake, monkeypatch):
    assert (await cashier_client.put("/accounting/vouchers/payment/7", json=body())).status_code == 403
    assert (await cashier_client.post("/accounting/vouchers/payment/7/cancel")).status_code == 403
    _saved(monkeypatch, day="2021-04-11")
    from app import gsettings
    gsettings.seed()
    gsettings.store.GSETTINGS["FreezeDate"]["value"] = "2021-04-11"
    assert (await client.post("/accounting/vouchers/payment/7/cancel")).status_code == 409
    assert fake.sql == []


async def test_quotation_edit_and_cancel(client, fake, monkeypatch):
    _saved(monkeypatch)
    item = next(iter(store.ITEMS))
    monkeypatch.setattr(quotation.accounting, "_columns", lambda t: {"qtndate", "custid", "partyname", "totqty", "total", "grndtot", "rmks", "changedby", "changedon", "cancelled"})
    monkeypatch.setattr(quotation, "get_quotation", lambda qid: {"id": qid})
    monkeypatch.setattr(db, "fetchall", lambda sql, params=(): [{"ItemCode": item, "UOMID": 2}])
    res = await client.put("/transactions/quotation/1", json={"date": "2026-10-31", "customer": "CASH", "lines": [{"item_code": item, "qty": 3, "rate": 10}]})
    assert res.status_code == 200, res.text
    text = [s for s, _ in fake.sql]
    assert text[0].startswith("UPDATE TrnHdrQTN SET") and any(s.startswith("DELETE FROM TrnDtlQTN") for s in text) and any("INSERT INTO TrnDtlQTN" in s for s in text)
    fake.sql.clear()
    assert (await client.post("/transactions/quotation/1/cancel")).status_code == 200
    assert len(fake.sql) == 1 and "Cancelled = 'Y'" in fake.sql[0][0]
    _saved(monkeypatch, cancelled="Y")
    assert (await client.post("/transactions/quotation/1/cancel")).status_code == 409
