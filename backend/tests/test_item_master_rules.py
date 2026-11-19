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
    assert res.status_code == 422 and "cannot be same" in res.json()["detail"]


async def test_an_alias_holds_at_most_30_characters(client):
    assert (await _post(client, aliases=["A" * 31])).status_code == 422
    assert (await _post(client, aliases=["A" * 30])).status_code == 201


async def test_code_and_aliases_may_not_be_another_items_code_alias_short_name_or_hot_key(client):
    # frmMstItem.ValidFields: the list searched holds the item's code and aliases only (short name / hot key never match)
    res = await _post(client, aliases=["VIN-750"])                       # another item's code
    assert res.status_code == 422 and "Duplicate Item Code" in res.json()["detail"]
    assert (await _post(client, aliases=["BARCODE-1"])).status_code == 201
    res = await _post(client, code="barcode-1")                          # another item's alias, whatever the case
    assert res.status_code == 422
    assert (await _post(client, code="NEW-11", short_name="SHORT-1")).status_code == 201
    assert (await _post(client, code="NEW-12", short_name="short-1")).status_code == 201   # two items may share a short name
    res = await _post(client, code="SHORT-1")                            # ... but a code may not equal one
    assert res.status_code == 422 and "ShortName" in res.json()["detail"]


async def test_bottles_per_case_may_be_zero_and_short_name_saved_as_typed(client):
    res = await _post(client, code="NEW-BPC", bottles_per_case=0, short_name="")
    assert res.status_code == 201 and res.json()["bottles_per_case"] == 0 and res.json()["short_name"] == ""


async def test_an_item_can_be_recoded_after_it_is_on_bills(client):
    old = "VIN-750"
    res = await client.put(f"/masters/items/{old}", json={**store.ITEMS[old], "code": "VIN-750-R"})
    assert res.status_code == 200 and "VIN-750-R" in store.ITEMS and old not in store.ITEMS
    assert not any(m["item"] == old for m in store.MOVEMENTS)


async def test_an_inactive_item_needs_remarks(client):
    res = await _post(client, code="NEW-INA", active=False)
    assert res.status_code == 422 and "remarks" in res.json()["detail"]


def test_hot_key_may_not_be_another_items():
    # called directly: without a database the API drops `extras`, which is where the desktop's HotKey column travels
    store.ITEMS["VIN-750"]["extras"] = {"HotKey": "K1"}
    with pytest.raises(HTTPException) as err:
        masters._check_item_names(Item(**_new(code="k1")), None)          # the code is looked up among the others' hot keys
    assert "HotKey" in err.value.detail
    masters._check_item_names(Item(**_new(code="NEW-15")), None)


async def test_editing_an_item_keeps_its_own_names(client):
    assert (await _post(client, aliases=["MINE"], short_name="MINE-S")).status_code == 201
    res = await client.put("/masters/items/NEW-9", json=_new(aliases=["MINE"], short_name="MINE-S", name="Renamed"))
    assert res.status_code == 200 and store.ITEMS["NEW-9"]["name"] == "Renamed"


async def test_uom_is_never_changed_from_the_item_form(client):
    other = next(c for c in store.UOMS if c != store.ITEMS["VIN-750"]["uom"])
    res = await client.put("/masters/items/VIN-750", json=dict(store.ITEMS["VIN-750"], uom=other))   # on bills
    assert res.status_code == 409 and "Update Item Master Fields" in res.json()["detail"]
    assert (await _post(client)).status_code == 201
    res = await client.put("/masters/items/NEW-9", json=_new(uom=other))                              # not on any bill: still refused
    assert res.status_code == 409 and "UOM can't be changed" in res.json()["detail"]
    assert (await client.put("/masters/items/NEW-9", json=_new(name="Renamed"))).status_code == 200     # the same UOM saves as before


def test_update_item_master_fields_always_offers_uomid(monkeypatch):
    monkeypatch.setattr(masters.db, "is_db_available", lambda: True)
    monkeypatch.setattr(masters.db, "fetchall", lambda sql, params=(): [{"FieldName": "MRP", "Datatype": "NUMERIC"}, {"FieldName": "BotPerCases", "Datatype": "INTEGER"}])
    assert [f["code"] for f in masters._bulk_field_defs()] == ["BotPerCases", "MRP", "UOMID"]   # sorted by name, UOMID added
    monkeypatch.setattr(masters.db, "fetchall", lambda sql, params=(): [{"FieldName": "UOMID", "Datatype": "Numeric"}])
    assert [f["code"] for f in masters._bulk_field_defs()] == ["UOMID"]                           # the shop's own row is kept, not doubled


def test_uom_written_by_the_bulk_screen_is_checked(monkeypatch):
    monkeypatch.setattr(masters.db, "fetchone", lambda sql, params=(): {"v": 7})           # the item's UOMID now
    monkeypatch.setattr(masters, "_item_in_use", lambda code: code == "VIN-750")                    # bills / stock the app knows of
    monkeypatch.setattr(masters, "_item_id_on_trn_tables", lambda item_id, strict=False: item_id == 777)   # the shop's TrnDtl* tables
    other_code = next(c for c in store.ITEMS if c != "VIN-750")
    monkeypatch.setitem(store.ITEMS["VIN-750"], "_itemid", 501)
    monkeypatch.setitem(store.ITEMS[other_code], "_itemid", 502)
    item_id = 501
    masters._check_uom_change(1, item_id, "BTL", 7)                                         # unchanged: passes even for a used item
    for blank in ("", "-"):
        with pytest.raises(masters.HTTPException) as e:
            masters._check_uom_change(1, item_id, blank, 0)
        assert e.value.status_code == 422
    with pytest.raises(masters.HTTPException) as e:                                          # changed, on bills: refused
        masters._check_uom_change(2, item_id, "CASE", 9)
    assert e.value.status_code == 409 and "used in bills or stock" in e.value.detail
    masters._check_uom_change(3, 502, "CASE", 9)                                            # changed, unused: passes


def test_uom_guard_also_checks_the_database_when_the_item_is_not_in_memory(monkeypatch):
    monkeypatch.setattr(masters.db, "fetchone", lambda sql, params=(): {"v": 7})
    monkeypatch.setattr(masters, "_item_id_on_trn_tables", lambda item_id, strict=False: item_id == 777)
    with pytest.raises(masters.HTTPException) as e:                  # ItemID 777 is in no in-memory item, but it is on the shop's bills
        masters._check_uom_change(1, 777, "CASE", 9)
    assert e.value.status_code == 409 and "ItemID 777" in e.value.detail
    masters._check_uom_change(2, 778, "CASE", 9)                    # another unknown ItemID with no bills: allowed


def test_uom_guard_does_not_guess_when_the_database_cannot_answer(monkeypatch):
    monkeypatch.setattr(masters.db, "is_db_available", lambda: True)
    def broken(*a, **k):
        raise RuntimeError("down")
    monkeypatch.setattr(masters.db, "fetchall", broken)
    assert masters._item_id_on_trn_tables(5) is False               # the item form's old rule: don't block on a guess
    with pytest.raises(masters.HTTPException) as e:
        masters._item_id_on_trn_tables(5, strict=True)              # a UOM change: refuse
    assert e.value.status_code == 503


def test_uomid_is_always_an_integer_id_field(monkeypatch):
    monkeypatch.setattr(masters.db, "is_db_available", lambda: True)
    monkeypatch.setattr(masters.db, "fetchall", lambda sql, params=(): [{"FieldName": "UOMID", "Datatype": "TEXT"}])
    assert masters._bulk_field_defs() == [{"code": "UOMID", "datatype": "INTEGER"}]               # a TEXT row in the shop's table can't skip the guard


async def test_import_from_excel_cannot_change_the_uom_of_an_existing_item(client):
    other = next(c for c in store.UOMS if c != store.ITEMS["VIN-750"]["uom"])
    res = await client.post("/masters/items/import", params={"overwrite": "true"}, json=[dict(store.ITEMS["VIN-750"], uom=other)])
    assert res.status_code == 422 and "UOM of an existing item" in res.json()["detail"]
    assert store.ITEMS["VIN-750"]["uom"] != other
    ok = await client.post("/masters/items/import", params={"overwrite": "true"}, json=[dict(store.ITEMS["VIN-750"], name="Renamed by import")])
    assert ok.status_code == 200                                                                   # the same UOM still imports


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


# ─── Item Master options (frmMstItem Options menu, similar-code prompt, UOM lock) ───

async def test_similar_codes_are_found_ignoring_punctuation_and_case(client):
    res = await client.get("/masters/items/similar", params={"code": "vin 750"})
    assert res.status_code == 200 and [r["code"] for r in res.json()] == ["VIN-750"]
    assert (await client.get("/masters/items/similar", params={"code": "vin 750", "exclude": "VIN-750"})).json() == []


async def test_options_that_need_the_database_say_so_without_one(client):
    invalid = (await client.get("/masters/items/invalid")).json()
    assert invalid["rows"] == [] and "database" in invalid["note"].lower()
    assert (await client.get("/masters/items/bulk-fields")).json() == []
    assert (await client.post("/masters/items/bulk-fields/load", json={"fields": ["HSNCode"]})).status_code == 422
    assert (await client.post("/masters/items/bulk-fields/import", json={"rows": [{"ItemID": 1}]})).status_code == 503
    assert (await client.post("/masters/items/recalculate-rates")).status_code == 503


async def test_item_find_looks_in_short_name_and_aliases(client):
    assert (await _post(client, code="FIND-1", short_name="zzshort", aliases=["zzalias"])).status_code == 201
    for q in ("zzshort", "ZZALIAS", "find-1"):
        assert [r["code"] for r in (await client.get("/masters/items", params={"q": q})).json()] == ["FIND-1"]


async def test_an_items_aliases_come_back_in_the_item_list_the_bills_search(client):
    """The Sales Invoice finds an item by its alias from /masters/items: a saved item must list its aliases at once."""
    res = await _post(client, code="ALIAS-22", aliases=["0000023", "0000024"])
    assert res.status_code == 201
    listed = {i["code"]: i for i in (await client.get("/masters/items")).json()}
    assert listed["ALIAS-22"]["aliases"] == ["0000023", "0000024"]
    res = await client.put("/masters/items/ALIAS-22", json=_new(code="ALIAS-22", aliases=["0000025"]))
    assert res.status_code == 200
    listed = {i["code"]: i for i in (await client.get("/masters/items")).json()}
    assert listed["ALIAS-22"]["aliases"] == ["0000025"]


async def test_a_new_item_needs_an_mrp_but_not_a_package_or_hsn_code(client):
    """MRP must be above 0 (2026-11-19). The Package box is the designer's Pkg Item column (mandatory there); the typed package and HSN stay optional."""
    res = await client.post("/masters/items", json=_new("NOPKG-1", package="", hsn_code=""))
    assert res.status_code == 201, res.text
    res = await client.post("/masters/items", json=_new("NOMRP-1", mrp=0))
    assert res.status_code == 422 and "MRP" in res.text
    res = await client.post("/masters/items", json=_new("NOHSN-1", package="-", hsn_code=""))
    assert res.status_code == 201, res.text


def test_the_item_masters_header_formulas_are_worked_out_on_save(monkeypatch):
    """Item Selling Rate (read-only, formula of the Doc Designer) is always recalculated; an editable formula field keeps what the user typed."""
    from app import designer, doc_engine
    from app import schema_meta
    sell = "SVTROUND2((#MRP#-#COMPCESS#)/(100+#CGSTPER#+#SGSTPER#+#CESSPER#)*100)"
    fields = [{"FldCode": "ITEMSELL", "FldFormula": sell, "FldBlnReadOnly": "Y", "FldNumDecimals": 2},
              {"FldCode": "MAXLEVEL", "FldFormula": "#ROL#*2", "FldBlnReadOnly": "N", "FldNumDecimals": 2}]
    monkeypatch.setattr(doc_engine, "doc_id_of", lambda name: 1114)
    monkeypatch.setattr(designer, "fields_for", lambda doc_id: fields)
    monkeypatch.setattr(schema_meta, "columns", lambda table: [{"name": n} for n in ("ITEMSELL", "MAXLEVEL", "MRP", "ROL")])
    row = {"mrp": 395.0, "cgst_per": 9, "sgst_per": 9, "rol": 5, "extras": {"CESSPER": "10", "COMPCESS": "100", "ITEMSELL": "395"}}
    masters._apply_item_formulas(row)
    assert row["extras"]["ITEMSELL"] == 230.47          # (395 - 100) / (100 + 9 + 9 + 10) * 100, whatever was there
    assert row["extras"]["MAXLEVEL"] == 10               # empty: calculated
    typed = {"mrp": 100.0, "cgst_per": 0, "sgst_per": 0, "rol": 5, "extras": {"MAXLEVEL": "7"}}
    masters._apply_item_formulas(typed)
    assert typed["extras"]["MAXLEVEL"] == "7"            # editable and filled: kept
    assert typed["extras"]["ITEMSELL"] == 100.0
    monkeypatch.setattr(designer, "fields_for", lambda doc_id: [])
    plain = {"mrp": 100.0, "extras": {}}
    masters._apply_item_formulas(plain)
    assert plain["extras"] == {}                         # no formulas in the database: nothing added


async def test_an_item_with_the_dash_package_or_brand_can_be_saved_again(client):
    """The desktop's "-" row (ID 0) means none: an item loaded with it is saved back as it is."""
    res = await client.post("/masters/items", json=_new("DASH-1", package="-", brand="-"))
    assert res.status_code == 201, res.text
    res = await client.put("/masters/items/DASH-1", json=_new("DASH-1", package="-", brand="-"))
    assert res.status_code == 200, res.text


async def test_hsn_code_is_optional_and_zero_means_none(client):
    for code, hsn in (("HSN-A", ""), ("HSN-B", "0"), ("HSN-C", "22083011")):
        assert (await client.post("/masters/items", json=_new(code, hsn_code=hsn))).status_code == 201
    assert (await client.post("/masters/items", json=_new("HSN-D", hsn_code="22"))).status_code == 422


async def test_import_dry_run_reports_each_bad_cell_in_the_exes_words_and_adds_nothing(client):
    """Import Item Data From Excel checks the sheet with dry_run=true: one {"error", "fields"} per row (the exe's checks and wording), nothing is imported."""
    await client.post("/masters/items", json=_new("DRY-OLD"))
    rows = [_new("DRY-OK"), _new("DRY-BAD", group="NO SUCH GROUP", brand="NO BRAND", liqtype="Spirits", mrp=0), _new("DRY-OK"), _new("DRY-OLD")]
    res = await client.post("/masters/items/import?dry_run=true", json=rows)
    assert res.status_code == 200, res.text
    out = res.json()["rows"]
    assert out[0] == {"error": "", "fields": []}
    assert out[1]["fields"] == ["brand", "group", "liqtype", "mrp"]
    assert "Invalid Group" in out[1]["error"] and "Invalid Brand" in out[1]["error"] and "Invalid Liq.Type" in out[1]["error"] and "Invalid MRP" in out[1]["error"]
    assert out[2] == {"error": "Duplicate Data", "fields": ["code"]}
    assert out[3] == {"error": "Item Already Present", "fields": ["code"]}
    assert "DRY-OK" not in store.ITEMS and "DRY-BAD" not in store.ITEMS


async def test_import_applies_only_the_exes_checks_not_the_item_forms_other_rules(client):
    """A row the desktop import accepts is accepted: a short name another item already holds, a blank package / brand ("-"), a blank liquor type."""
    other = store.ITEMS["VIN-750"]
    row = _new("IMP-222", short_name=other.get("short_name") or "SHARED", package="", brand="", liqtype="", purchase_rate=500.0, mrp=200.0)
    res = await client.post("/masters/items/import?dry_run=true", json=[row])
    assert res.status_code == 200, res.text
    assert res.json()["rows"][0] == {"error": "", "fields": []}
    res = await client.post("/masters/items/import", json=[row])
    assert res.status_code == 200, res.text
    assert store.ITEMS["IMP-222"]["package"] == "-" and store.ITEMS["IMP-222"]["brand"] == "-"


async def test_import_works_out_the_item_selling_rate(client, monkeypatch):
    """The exe's import stores ItemSell with the item; the web does it with the designer's formula (MRP less Comp.Cess over the taxes)."""
    fields = [{"FldCode": "ITEMSELL", "FldFormula": "SVTROUND2((#MRP#-#COMPCESS#)/(100+#CGSTPER#+#SGSTPER#+#CESSPER#)*100)", "FldBlnReadOnly": "Y", "FldNumDecimals": 2}]
    monkeypatch.setattr(masters, "_item_formula_context", lambda: (fields, {"ITEMSELL": "ITEMSELL", "MRP": "MRP"}))
    row = _new("SELL-1", mrp=395.0, cgst_per=6.0, sgst_per=6.0)   # the test database has no cess columns: MRP over the two GST rates, as the exe works it out
    res = await client.post("/masters/items/import", json=[row])
    assert res.status_code == 200, res.text
    assert store.ITEMS["SELL-1"]["extras"]["ITEMSELL"] == 352.68   # 395 / (100 + 6 + 6) * 100
