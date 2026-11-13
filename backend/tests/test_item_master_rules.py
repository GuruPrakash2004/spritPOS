"""Item Master save rules taken from the desktop's frmMstItem.ValidFields / Edit: what an item must have, the names it may not repeat,
the UOM that locks once the item is on a transaction, and the purchase / sale variation rule (not for the "Others" type)."""
import pytest
from fastapi import HTTPException

from app import store
from app.routers import masters
from app.schemas import Item


def _new(code="NEW-9", **changes):
    base = dict(store.ITEMS["VIN-750"], code=code, name="New item", short_name=code, aliases=[], qoh=0, purchase_rate=100.0, mrp=200.0, extras={})
    return {**base, **changes}


async def _post(client, **changes):
    return await client.post("/masters/items", json=_new(**changes))


async def test_group_type_and_uom_are_required(client):
    assert "Item Group" in (await _post(client, group="")).json()["detail"]
    assert "Item Type" in (await _post(client, liqtype="")).json()["detail"]
    res = await _post(client, liqtype="Spirits")
    assert res.status_code == 422 and "valid Item Type" in res.json()["detail"]
    assert "UOM" in (await _post(client, uom="")).json()["detail"]
    for kind in ("Wine", "beer", "IML", "OTHERS"):   # the exe's list, in any letter case
        assert (await _post(client, code=f"T-{kind}", liqtype=kind)).status_code == 201


async def test_aliases_may_not_repeat_or_equal_the_code(client):
    res = await _post(client, aliases=["X1", "x1"])
    assert res.status_code == 422 and "Duplicate alias" in res.json()["detail"]
    res = await _post(client, aliases=["new-9"])
    assert res.status_code == 422 and "cannot be the same" in res.json()["detail"]


async def test_an_alias_holds_at_most_30_characters(client):
    assert (await _post(client, aliases=["A" * 31])).status_code == 422
    assert (await _post(client, aliases=["A" * 30])).status_code == 201


async def test_code_alias_and_short_name_may_not_be_another_items(client):
    res = await _post(client, aliases=["VIN-750"])                       # another item's code
    assert res.status_code == 422 and "Duplicate item code" in res.json()["detail"]
    assert (await _post(client, aliases=["BARCODE-1"])).status_code == 201
    res = await _post(client, code="NEW-10", short_name="barcode-1")      # ... and an alias, whatever the case
    assert res.status_code == 422 and "alias" in res.json()["detail"]
    assert (await _post(client, code="NEW-11", short_name="SHORT-1")).status_code == 201
    res = await _post(client, code="NEW-12", short_name="short-1")
    assert res.status_code == 422 and "short name" in res.json()["detail"]


def test_hot_key_may_not_be_another_items():
    # called directly: without a database the API drops `extras`, which is where the desktop's HotKey column travels
    store.ITEMS["VIN-750"]["extras"] = {"HotKey": "K1"}
    with pytest.raises(HTTPException) as err:
        masters._check_item_names(Item(**_new(code="NEW-14", extras={"hotkey": "k1"})), None)
    assert "hot key" in err.value.detail
    masters._check_item_names(Item(**_new(code="NEW-15", extras={"hotkey": "K2"})), None)


async def test_editing_an_item_keeps_its_own_names(client):
    assert (await _post(client, aliases=["MINE"], short_name="MINE-S")).status_code == 201
    res = await client.put("/masters/items/NEW-9", json=_new(aliases=["MINE"], short_name="MINE-S", name="Renamed"))
    assert res.status_code == 200 and store.ITEMS["NEW-9"]["name"] == "Renamed"


async def test_uom_is_locked_once_the_item_is_on_a_transaction(client):
    other = next(c for c in store.UOMS if c != store.ITEMS["VIN-750"]["uom"])
    used = dict(store.ITEMS["VIN-750"], uom=other)
    res = await client.put("/masters/items/VIN-750", json=used)
    assert res.status_code == 409 and "UOM can't be changed" in res.json()["detail"]
    assert (await _post(client)).status_code == 201                      # an unused item can still change it
    res = await client.put("/masters/items/NEW-9", json=_new(uom=other))
    assert res.status_code == 200 and store.ITEMS["NEW-9"]["uom"] == other


async def test_purchase_sale_variation_is_not_checked_for_the_others_type(client):
    await client.put("/utilities/settings/PurSellVarPer", json={"caption": "c", "value": "10"})
    res = await _post(client, mrp=100.0, purchase_rate=100.0, liqtype="IML")
    assert res.status_code == 422 and "Variation" in res.json()["detail"]
    assert (await _post(client, code="NEW-OTH", mrp=100.0, purchase_rate=100.0, liqtype="Others")).status_code == 201


def test_the_selling_rate_is_what_the_desktop_compares(monkeypatch):
    monkeypatch.setattr(masters.gsettings, "number", lambda name: 10.0)
    row = {"code": "X", "mrp": 500.0, "purchase_rate": 100.0, "liqtype": "IML", "extras": {"ItemSell": "105"}}
    with pytest.raises(HTTPException) as err:
        masters._check_margin(row)
    assert "5%" in err.value.detail
    masters._check_margin({**row, "extras": {"ItemSell": "115"}})


def test_aliases_are_described_to_the_screen_as_a_list_of_lines():
    # list[str] was read as plain text, so the screen showed a single box instead of the alias grid
    from app import field_columns
    kinds = {f["key"]: f["kind"] for f in field_columns.web_fields("items")}
    assert kinds["aliases"] == "lines" and kinds["name"] == "text" and kinds["active"] == "bool"


def _remarks_body(active, remarks=None, sent=True):
    from app.schemas import Supplier
    fields = dict(code="S1", name="Sup", gstin="29ABCDE1234F1Z5")
    if sent:
        fields["extras"] = {"InActiveRmks": remarks} if remarks is not None else {}
    return Supplier(**fields, active=active)


def test_inactive_remarks_rule_clears_on_reactivation_and_needs_the_database_column(monkeypatch):
    # Every master with an Active flag: remarks are required while inactive (when the table has the column), cleared when active again.
    monkeypatch.setattr(masters.sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(masters.schema_meta, "columns", lambda table: [{"name": "InActiveRmks"}])
    with pytest.raises(HTTPException) as err:
        masters._check_inactive_remarks("suppliers", _remarks_body(False))
    assert "remarks" in err.value.detail
    masters._check_inactive_remarks("suppliers", _remarks_body(False, "closed shop"))      # given: fine
    body = _remarks_body(True, "old reason")
    masters._check_inactive_remarks("suppliers", body)
    assert body.extras["InActiveRmks"] == ""                                               # reactivated: cleared
    masters._check_inactive_remarks("suppliers", _remarks_body(False, sent=False))         # a client that sent no extras isn't asked
    monkeypatch.setattr(masters.schema_meta, "columns", lambda table: [{"name": "SomethingElse"}])
    masters._check_inactive_remarks("suppliers", _remarks_body(False))                     # a table without the column: no rule
