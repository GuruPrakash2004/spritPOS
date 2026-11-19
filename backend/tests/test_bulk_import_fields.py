"""Bulk Item Import Fields (frmUtlBulkImpFlds): the grid's rows, the drop-downs and the replace-all save, on a fake database."""
from contextlib import contextmanager

import pytest

from app import admin_audit, db, sqlstore

COLUMNS = ["ItemID", "ItemCode", "ItemName", "MRP", "HSNCode", "BotPerCases", "ROL"]


@pytest.fixture
def fake(monkeypatch):
    state = {"rows": [{"FieldName": "MRP", "DataType": "NUMERIC"}, {"FieldName": "HSNCode", "DataType": "TEXT"}], "sql": [], "audit": []}

    def fetchall(sql, params=()):
        s = " ".join(sql.split()).lower()
        if "information_schema.columns" in s:
            return [{"column_name": c} for c in COLUMNS]
        assert "from utlitembulkupdate" in s, sql
        if state.get("missing"):
            raise RuntimeError("Invalid object name")
        return list(state["rows"])

    class Cur:
        def execute(self, sql, params=()):
            state["sql"].append((" ".join(sql.split()), params))
            if sql.lower().startswith("delete"):
                state["rows"] = []
            else:
                state["rows"].append({"FieldName": params[0], "DataType": params[1]})
            return 1

    @contextmanager
    def tx():
        yield Cur()

    monkeypatch.setattr(sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(db, "fetchall", fetchall)
    monkeypatch.setattr(db, "transaction", tx)
    monkeypatch.setattr(admin_audit, "write_now", lambda *a, **k: state["audit"].append(a))
    return state


async def test_the_screen_lists_the_table_and_the_item_columns(client, fake):
    res = (await client.get("/utilities/bulk-import-fields")).json()
    assert res["fields"] == [{"name": "MRP", "datatype": "NUMERIC"}, {"name": "HSNCode", "datatype": "TEXT"}]
    assert "ItemID" not in res["columns"] and "ROL" in res["columns"]
    assert res["types"] == ["Text", "Integer", "Date", "Numeric"]


async def test_save_replaces_the_whole_table_and_skips_blank_rows(client, fake):
    rows = [{"name": "rol", "datatype": "numeric"}, {"name": "", "datatype": ""}, {"name": "BotPerCases", "datatype": "Integer"}]
    res = await client.put("/utilities/bulk-import-fields", json={"fields": rows})
    assert res.status_code == 200 and res.json()["message"] == "Saved successfully."
    assert fake["sql"][0][0].lower() == "delete from utlitembulkupdate"
    assert fake["rows"] == [{"FieldName": "ROL", "DataType": "Numeric"}, {"FieldName": "BotPerCases", "DataType": "Integer"}]
    assert fake["audit"]   # the change is in the audit log


@pytest.mark.parametrize("fields,text", [
    ([{"name": "Nope", "datatype": "Text"}], "not a field of the Item Master"),
    ([{"name": "ItemID", "datatype": "Integer"}], "not a field of the Item Master"),
    ([{"name": "MRP", "datatype": "Numeric"}, {"name": "mrp", "datatype": "Numeric"}], "Duplicate field"),
    ([{"name": "MRP", "datatype": ""}], "Select the Data Type"),
])
async def test_mistakes_are_refused_and_nothing_is_changed(client, fake, fields, text):
    res = await client.put("/utilities/bulk-import-fields", json={"fields": fields})
    assert res.status_code == 422 and text in res.json()["detail"]
    assert fake["sql"] == []


async def test_without_the_table_the_screen_says_so(client, fake):
    fake["missing"] = True
    res = await client.get("/utilities/bulk-import-fields")
    assert res.status_code == 503 and "UtlItemBulkUpdate" in res.json()["detail"]


async def test_only_an_administrator_may_change_it(cashier_client, fake):
    assert (await cashier_client.get("/utilities/bulk-import-fields")).status_code == 403
    assert (await cashier_client.put("/utilities/bulk-import-fields", json={"fields": []})).status_code == 403
