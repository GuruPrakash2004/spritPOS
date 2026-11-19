"""Invoice Settings → "Update Rate From PB to Item Master" (UtlMisc.RateFld): what a Purchase Bill save does to the item master."""
from datetime import date

import pytest

from app import db, sqlstore, store

TODAY = date.today().isoformat()


def pb(rate=777, **extra):
    sup = next(iter(store.SUPPLIERS))
    return {"date": TODAY, "supplier": sup, "supplier_inv_no": "R1", "supplier_inv_date": TODAY, "supplier_inv_value": 0,
            "lines": [{"item_code": "VIN-750", "bottles": 1, "rate": rate}], **extra}


# ---------- the web's own item ----------
async def test_switch_off_leaves_the_item_rate(client):
    before = store.ITEMS["VIN-750"]["purchase_rate"]
    store.UTL_MISC["RateFld"] = "N"

    assert (await client.post("/transactions/purchase-bill", json=pb())).status_code == 201

    assert store.ITEMS["VIN-750"]["purchase_rate"] == before


async def test_switch_on_item_follows_the_bill_even_for_a_return(client):
    store.UTL_MISC["RateFld"] = "Y"

    await client.post("/transactions/purchase-bill", json=pb(777))
    assert store.ITEMS["VIN-750"]["purchase_rate"] == 777

    res = await client.post("/transactions/purchase-bill", json=pb(650, purchase_type="Purchase Return"))   # the exe makes no exception for a return
    assert res.status_code == 201 and store.ITEMS["VIN-750"]["purchase_rate"] == 650


# ---------- the database statement ----------
class Cur:
    def __init__(self):
        self.sql = []

    def execute(self, sql, params=()):
        self.sql.append((" ".join(sql.split()), params))


def meta(item_cols, dtl_cols):
    return lambda table: {c: {} for c in (item_cols if table == "MstITEM" else dtl_cols)}


BASE_ITEM = ["purrate", "mrp", "itemsell", "aroedper", "cgstper", "sgstper", "liqtype"]
BASE_DTL = ["pbrate", "mrp", "aroedperbot"]


def run(monkeypatch, item_cols, dtl_cols, doc_no="26/PB-0000007", database="SHOP"):
    monkeypatch.setattr(sqlstore, "_meta", meta(item_cols, dtl_cols))
    monkeypatch.setattr(db, "settings", lambda: {"database": database})
    store.UTL_MISC["RateFld"] = "Y"
    cur = Cur()
    sqlstore._rate_from_pb(cur, 5, {"doc_no": doc_no})
    return cur.sql


def test_the_exes_update_without_cess(monkeypatch):
    (sql, params), = run(monkeypatch, BASE_ITEM, BASE_DTL)

    assert params == (5,)
    assert sql == ("UPDATE dbo.MstITEM SET PurRate = d.PBRate, AROEDPER = ISNULL(d.AROEDPERBOT, MstITEM.AROEDPER), "
                   "ItemSell = (Case When MstITEM.LiqType = 'Others' Then CAST(((ISNULL(d.MRP, MstITEM.MRP)) / (100 + MstITEM.CGSTPER + MstITEM.SGSTPER) * 100) As Numeric(12,2)) "
                   "Else ISNULL(d.MRP, MstITEM.MRP) - ISNULL(d.AROEDPERBOT, 0) End), MRP = ISNULL(d.MRP, MstITEM.MRP) "
                   "FROM dbo.TrnDtlPB d WHERE d.PBID = ? AND d.ITEMID = MstITEM.ITEMID")


def test_cess_and_comp_cess_columns_change_the_item_sell_formula(monkeypatch):
    (cess, _), = run(monkeypatch, BASE_ITEM + ["cessper"], BASE_DTL)
    (comp, _), = run(monkeypatch, BASE_ITEM + ["cessper", "compcess"], BASE_DTL)

    assert "MstITEM.SGSTPER + MstITEM.Cessper)" in cess and "Compcess" not in cess
    assert "(ISNULL(d.MRP, MstITEM.MRP) - MstITEM.Compcess) / (100 + MstITEM.CGSTPER + MstITEM.SGSTPER + MstITEM.Cessper)" in comp


def test_pm_pur_rate_is_only_for_the_vlqmsclh_database(monkeypatch):
    other = run(monkeypatch, BASE_ITEM + ["pmpurrate"], BASE_DTL)[0][0]
    clh = run(monkeypatch, BASE_ITEM + ["pmpurrate"], BASE_DTL, database="vlqmsclh")[0][0]

    assert "PMPurRate" not in other and clh.startswith("UPDATE dbo.MstITEM SET PMPurRate = d.PBRate, PurRate = d.PBRate")


@pytest.mark.parametrize("doc_no", ["26OP00005", "26/op-0000001"])
def test_an_op_bill_does_not_update_the_item(monkeypatch, doc_no):
    assert run(monkeypatch, BASE_ITEM, BASE_DTL, doc_no=doc_no) == []


def test_nothing_runs_with_the_switch_off_or_without_the_columns(monkeypatch):
    monkeypatch.setattr(sqlstore, "_meta", meta(BASE_ITEM, BASE_DTL))
    store.UTL_MISC["RateFld"] = "N"
    cur = Cur()
    sqlstore._rate_from_pb(cur, 5, {"doc_no": "26/PB-0000007"})
    assert cur.sql == []

    assert run(monkeypatch, ["purrate", "mrp"], BASE_DTL) == []   # no ItemSell column: the table is not the desktop's
