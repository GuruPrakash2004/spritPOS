"""Delete Linkage Updation (FrmUtlDeleteLinkageUpdation): the rules table UtlDeletion. The SQL needs the shop's database, so a fake one answers here."""
from contextlib import contextmanager

import pytest

from app import db, delete_linkage, sqlstore

SCHEMA = {"MstItem": ["ItemID", "ItemCode", "ItemName"], "TrnDtlSINV": ["SINVID", "ItemID"], "TrnDtlPB": ["PBID", "ItemID"], "MstCUST": ["CustID", "CustCode"]}


class FakeDb:
    def __init__(self):
        self.rules = [
            {"DelTblName": "MstItem", "TblName": "TrnDtlSINV", "FldName": "ItemID", "DispFldName": "ItemCode", "TblCaption": "Sales Invoice", "DelDocName": "Item Master", "DelDocFld": "ItemID"},
            {"DelTblName": "MstItem", "TblName": "TrnDtlPB", "FldName": "ItemID", "DispFldName": "ItemCode", "TblCaption": "Purchase Bill", "DelDocName": "Item Master", "DelDocFld": "ItemID"},
        ]
        self.statements = []

    def fetchall(self, sql, params=()):
        s = " ".join(sql.split()).lower()
        if "from sys.tables" in s:
            return [{"Object_Id": i, "Name": n} for i, n in enumerate(sorted(SCHEMA))]
        if "where table_name = ?" in s and "order by ordinal_position" in s:
            return [{"COLUMN_NAME": c} for c in SCHEMA.get(params[0], [])]
        if s.startswith("select delTblName".lower()) or "group by deltblname" in s:
            seen = {}
            for r in self.rules:
                seen[r["DelTblName"]] = r["DelDocName"] or r["DelTblName"]
            return [{"DelTblName": t, "DocName": n} for t, n in seen.items()]
        if "from utldeletion where deltblname = ?" in s:
            return [r for r in self.rules if r["DelTblName"] == params[0]]
        if "where column_name= ?" in s:
            return [{"TABLE_NAME": t, "COLUMN_NAME": params[0]} for t, cs in sorted(SCHEMA.items()) if params[0] in cs and t != params[1]]
        raise AssertionError(f"unexpected SQL: {sql}")

    def fetchone(self, sql, params=()):
        s = " ".join(sql.split()).lower()
        if "docshform" in s:
            return {"D": "ItemCode"} if params[0] == "MstItem" else None
        if "select docname" in s:
            return {"DocName": {"TrnDtlSINV": "Sales Invoice"}.get(params[0], "")} if params[0] == "TrnDtlSINV" else None
        raise AssertionError(f"unexpected SQL: {sql}")


@pytest.fixture
def fake(monkeypatch):
    f = FakeDb()

    class Cur:
        def execute(self, sql, params=()):
            f.statements.append(("execute", " ".join(sql.split()), params))
            if sql.lower().startswith("delete from utldeletion"):
                f.rules = [r for r in f.rules if r["DelTblName"] != params[0]]

        def executemany(self, sql, rows):
            f.statements.append(("executemany", " ".join(sql.split()), list(rows)))
            for t, tbl, col, disp, cap, dn, df in rows:
                f.rules.append({"DelTblName": t, "TblName": tbl, "FldName": col, "DispFldName": disp, "TblCaption": cap, "DelDocName": dn, "DelDocFld": df})

    @contextmanager
    def tx():
        f.statements.append(("begin",))
        yield Cur()
        f.statements.append(("commit",))

    monkeypatch.setattr(sqlstore, "db_usable", lambda: True)
    monkeypatch.setattr(db, "fetchall", f.fetchall)
    monkeypatch.setattr(db, "fetchone", f.fetchone)
    monkeypatch.setattr(db, "transaction", tx)
    return f


async def test_lists_tables_columns_and_guarded_tables(client, fake):
    assert (await client.get("/utilities/delete-linkage/tables")).json() == sorted(SCHEMA)
    assert (await client.get("/utilities/delete-linkage/columns", params={"table": "MstItem"})).json() == SCHEMA["MstItem"]
    assert (await client.get("/utilities/delete-linkage/groups")).json() == [{"table": "MstItem", "name": "Item Master"}]


async def test_loads_one_group(client, fake):
    g = (await client.get("/utilities/delete-linkage/group", params={"table": "MstItem"})).json()

    assert g["doc_name"] == "Item Master" and g["doc_field"] == "ItemID"
    assert [r["table"] for r in g["rows"]] == ["TrnDtlSINV", "TrnDtlPB"]
    assert (await client.get("/utilities/delete-linkage/group", params={"table": "MstNope"})).status_code == 404


async def test_magnifier_finds_other_tables_with_the_column(client, fake):
    res = (await client.post("/utilities/delete-linkage/search", json={"table": "MstItem", "field": "ItemID"})).json()

    assert [r["table"] for r in res] == ["TrnDtlPB", "TrnDtlSINV"]
    assert {r["display"] for r in res} == {"ItemCode"}
    assert {r["table"]: r["caption"] for r in res}["TrnDtlSINV"] == "Sales Invoice"


async def test_magnifier_needs_a_table_and_a_field_of_that_table(client, fake):
    no_table = await client.post("/utilities/delete-linkage/search", json={"table": "", "field": "ItemID"})
    no_field = await client.post("/utilities/delete-linkage/search", json={"table": "MstItem", "field": ""})
    other = await client.post("/utilities/delete-linkage/search", json={"table": "MstItem", "field": "PBID"})

    assert no_table.json()["detail"] == "Select Table name"
    assert no_field.json()["detail"] == "Select Field to be Checked for Deletion"
    assert other.status_code == 422


async def test_save_rewrites_the_group_in_one_transaction(client, fake):
    body = {"doc_name": "Item Master", "doc_field": "ItemID", "rows": [
        {"table": "TrnDtlSINV", "column": "ItemID", "display": "ItemCode", "caption": "Sales Invoice"}, {"table": "", "column": ""}]}

    res = await client.put("/utilities/delete-linkage/group", params={"table": "MstItem"}, json=body)

    assert res.status_code == 200 and res.json() == {"saved": 1, "message": "Saved Successfully!"}
    kinds = [s[0] for s in fake.statements]
    assert kinds[:4] == ["begin", "execute", "executemany", "commit"]   # then the audit entry
    assert [r["TblName"] for r in fake.rules] == ["TrnDtlSINV"]


@pytest.mark.parametrize(("body", "message"), [
    ({"doc_name": "X", "doc_field": "ItemID", "rows": [{"table": "TrnDtlPB", "column": "ItemID"}]}, None),
    ({"doc_name": "", "doc_field": "ItemID", "rows": [{"table": "TrnDtlPB", "column": "ItemID"}]}, "Document name is Empty..."),
    ({"doc_name": "X", "doc_field": "ItemID", "rows": []}, "Data is not available"),
    ({"doc_name": "X", "doc_field": "", "rows": [{"table": "TrnDtlPB", "column": "ItemID"}]}, "Column is Empty...."),
])
async def test_save_checks_in_the_exes_order(client, fake, body, message):
    res = await client.put("/utilities/delete-linkage/group", params={"table": "MstItem"}, json=body)

    if message is None:
        assert res.status_code == 200
    else:
        assert res.status_code == 422 and res.json()["detail"] == message


async def test_save_needs_a_table(client, fake):
    res = await client.put("/utilities/delete-linkage/group", params={"table": " "}, json={"doc_name": "X", "doc_field": "ItemID", "rows": [{"table": "TrnDtlPB", "column": "ItemID"}]})

    assert res.status_code == 422 and res.json()["detail"] == "Table name is Empty..."


async def test_blank_rows_are_dropped_and_if_none_is_left_the_group_is_just_deleted(client, fake):
    """As the exe: delete the table's rows, insert only rows with a Table and a Column (possibly none)."""
    res = await client.put("/utilities/delete-linkage/group", params={"table": "MstItem"}, json={"doc_name": "X", "doc_field": "ItemID", "rows": [{"table": "", "column": ""}]})

    assert res.status_code == 200 and res.json()["saved"] == 0
    assert fake.rules == []


async def test_save_takes_any_names_like_the_exe_but_only_as_parameters(client, fake):
    row = {"table": "TrnDtlPB]; drop table X;--", "column": "Nope", "display": "", "caption": "Whatever"}

    res = await client.put("/utilities/delete-linkage/group", params={"table": "MstItem"}, json={"doc_name": "X", "doc_field": "ItemID", "rows": [row]})

    assert res.status_code == 200
    assert all("drop table" not in s[1].lower() for s in fake.statements if s[0] in ("execute", "executemany"))


def test_the_delete_check_skips_a_rule_with_unsafe_names(monkeypatch):
    ran = []

    class Cur:
        def execute(self, sql, args=()):
            ran.append(sql)

        def fetchone(self):
            return (7,)

    rules = [{"TblName": "TrnDtlSINV]; drop table X;--", "FldName": "ItemID", "TblCaption": "Bad"}, {"TblName": "TrnDtlSINV", "FldName": "Item]ID", "TblCaption": "Bad"}]
    monkeypatch.setattr(db, "fetchall", lambda sql, p=(): rules)

    sqlstore._check_unused(Cur(), "MstItem", "ItemID", "MstItem", "ItemCode", "WH750")   # nothing found, nothing raised

    assert not any("drop table" in s.lower() or "Item]ID" in s for s in ran)


async def test_delete_removes_the_whole_group(client, fake):
    res = await client.delete("/utilities/delete-linkage/group", params={"table": "MstItem"})

    assert res.json() == {"message": "Deleted Successfully!"}
    assert fake.rules == []


async def test_only_an_administrator_and_needs_the_database(manager_client, client):
    assert (await manager_client.get("/utilities/delete-linkage/tables")).status_code == 403
    assert (await manager_client.put("/utilities/delete-linkage/group", params={"table": "MstItem"}, json={})).status_code == 403
    assert (await client.get("/utilities/delete-linkage/tables")).status_code == 503   # no database in the tests


def test_the_delete_check_uses_the_exes_wording(monkeypatch):
    class Cur:
        def execute(self, sql, args=()):
            self.sql = sql

        def fetchone(self):
            return (7,)

    monkeypatch.setattr(db, "fetchall", lambda sql, p=(): [{"TblName": "TrnDtlSINV", "FldName": "ItemID", "TblCaption": "Sales Invoice"}])

    with pytest.raises(ValueError) as e:
        sqlstore._check_unused(Cur(), "MstItem", "ItemID", "MstItem", "ItemCode", "WH750")

    assert str(e.value) == "Cannot Delete!!! \nData : WH750 of MstItem exists in Sales Invoice"
