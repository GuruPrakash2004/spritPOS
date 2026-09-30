"""Tools menu: SQL Query Execution (read-only, Administrator only) and Re Write Stock (app/stock_rewrite.py).

The SQL tests never reach SQL Server: db.read_only is replaced by a fake that records the SQL it was given
and answers from a small table, and db.is_db_available is switched on.
"""
from datetime import date

import pytest

from app import db, persistence, store
from app.routers import tools

TODAY = date.today().isoformat()


def bill(lines, **extra):
    return {"date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": lines} | extra


# ---------- SQL Query Execution: which statements may run ----------

@pytest.mark.parametrize("sql", [
    "SELECT * FROM MstItem",
    "  select top 10 ItemCode from MstItem where ItemName like '%delete%'",
    "WITH a AS (SELECT 1 AS x) SELECT x FROM a",
    "SELECT [Update], [Set] FROM T -- DELETE FROM T",
    "SELECT 1 /* DROP TABLE x */;",
    "SELECT REPLACE(ItemName, 'A', 'B') FROM MstItem",
    "(SELECT 1)",
])
def test_reads_are_accepted(sql):
    assert tools._check_sql(sql).kind == "read"


@pytest.mark.parametrize("sql, table", [
    ("UPDATE Web_DocHeader SET Status = 'CANCELLED' WHERE DocNo = '26/PB-1'", "Web_DocHeader"),
    ("DELETE FROM dbo.Web_DocLine WHERE DocNo IN (SELECT DocNo FROM Web_DocHeader WHERE Status = 'X')", "dbo.Web_DocLine"),
    ("INSERT INTO [dbo].[Web_StockBalance] (ItemCode, QtyChange) VALUES ('VIN-750', 1)", "dbo.Web_StockBalance"),
    ("insert web_writelog (EventId) select 'x'", "web_writelog"),
])
def test_changes_to_the_web_tables_are_accepted(sql, table):
    stmt = tools._check_sql(sql, "sqlserver")
    assert stmt.kind == "write" and stmt.table == table


def test_local_changes_only_to_the_outbox():
    assert tools._check_sql("UPDATE web_outbox SET sync_status = 'PENDING' WHERE sync_status = 'FAILED'", "local").table == "web_outbox"
    for sql in ("UPDATE state SET data = ''", "DELETE FROM tool_audit", "UPDATE Web_DocHeader SET Status = 'X'"):
        with pytest.raises(tools.SqlRefused):
            tools._check_sql(sql, "local")


@pytest.mark.parametrize("sql", [
    "", "   ",
    "UPDATE MstItem SET MRP = 0",                          # desktop tables are never changed
    "DELETE FROM TrnHdrSINV",
    "INSERT INTO MstItem (ItemCode) VALUES ('X')",
    "INSERT INTO Web_SchemaVersion (Version) VALUES (9)",  # owned by tools/migrate.py
    "UPDATE other.Web_DocHeader SET Status = 'X'",
    "UPDATE w SET Status = 'X' FROM Web_DocHeader w",       # alias: the changed table isn't clear
    "UPDATE Web_DocHeader SET Status = 'X' FROM Web_DocHeader h JOIN MstItem i ON 1 = 1",
    "DELETE Web_DocLine FROM Web_DocLine JOIN MstItem ON 1 = 1",
    "UPDATE Web_DocHeader SET Status = 'X' DELETE FROM MstItem",
    "UPDATE Web_DocHeader SET Status = 'X' OUTPUT deleted.* INTO MstItem",
    "DROP TABLE Web_DocHeader",
    "ALTER TABLE Web_DocHeader ADD x int",
    "EXEC sp_who",
    "SELECT * INTO Copy FROM MstItem",
    "SELECT 1; DELETE FROM MstItem",
    "SELECT 1 DELETE FROM MstItem",
    "WITH a AS (SELECT 1 AS x) DELETE FROM MstItem",
    # SQL Server nests /* */ comments: this hides an UPDATE ... COMMIT from a flat comment scanner.
    "SELECT 1 /* /* */ ' */ UPDATE MstItem SET ItemName = 'x' COMMIT --'",
    "SELECT 1 /* unclosed",
    "SELECT 'unclosed",
    "SELECT [unclosed FROM MstItem",
    "SELECT * FROM OPENROWSET('SQLNCLI', 'x', 'y')",
    "SELECT * FROM remote.vLQMS.dbo.MstItem",
    "SELECT NEXT VALUE FOR dbo.SomeSequence",
    "TRUNCATE TABLE UtlStockList",
    "select 1 waitfor delay '00:00:10'",
])
def test_everything_else_is_refused(sql):
    with pytest.raises(tools.SqlRefused):
        tools._check_sql(sql, "sqlserver")


@pytest.fixture
def sqlserver(monkeypatch):
    """A connected 'SQL Server': read_only() records the SQL and answers from canned results,
    write_statement() records the change and reports 3 rows."""
    calls = []

    def fake_read_only(sql, params=(), max_rows=5000, lock_timeout_ms=10000):
        calls.append((sql, params))
        if "FROM sys.objects" in sql and "TOP 1" in sql:
            name = params[0]
            return {"columns": ["s", "n", "t"], "rows": [["dbo", name, "U "]] if name in ("MstItem", "Web_DocHeader") else [],
                    "truncated": False}
        if "FROM sys.objects" in sql:
            return {"columns": ["kind", "schema_name", "name"],
                    "rows": [["Tables", "dbo", "MstItem"], ["Views", "dbo", "vwDC"]], "truncated": False}
        if "INFORMATION_SCHEMA.COLUMNS" in sql:
            return {"columns": [], "rows": [["ItemID", "int", None, 10, 0, "NO", None, 1],
                                            ["ItemName", "varchar", 100, None, None, "YES", None, 0]], "truncated": False}
        if "KEY_COLUMN_USAGE" in sql:
            return {"columns": ["c"], "rows": [["ItemID"]], "truncated": False}
        if "COUNT_BIG" in sql:
            return {"columns": [""], "rows": [[7]], "truncated": False}
        if "BOOM" in sql:
            raise Exception(207, b"Invalid column name 'BOOM'.")
        rows = [[i, f"ITEM {i}"] for i in range(max_rows + 5)]
        return {"columns": ["ItemID", "ItemName"], "rows": rows[:max_rows], "truncated": len(rows) > max_rows}

    def fake_write(sql, lock_timeout_ms=10000):
        calls.append(("WRITE", sql))
        return 3

    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(db, "read_only", fake_read_only)
    monkeypatch.setattr(db, "write_statement", fake_write)
    return calls


async def test_execute_returns_rows_capped_at_5000_and_audits(client, sqlserver):
    res = await client.post("/tools/sql/execute", json={"sql": "SELECT ItemID, ItemName FROM MstItem"})

    assert res.status_code == 200
    body = res.json()
    assert body["kind"] == "read" and body["columns"] == ["ItemID", "ItemName"]
    assert body["row_count"] == 5000 and body["truncated"] is True
    audit = persistence.tool_audit_rows("SQL Query Execution")
    assert audit[0]["ok"] and audit[0]["row_count"] == 5000 and audit[0]["user_code"] == "ADMIN"


async def test_changes_to_web_tables_are_committed_and_audited(client, sqlserver):
    sql = "UPDATE Web_DocHeader SET Status = 'CANCELLED' WHERE DocNo = '26/PB-1'"

    res = await client.post("/tools/sql/execute", json={"sql": sql})

    assert res.status_code == 200
    assert res.json() | {"elapsed_ms": 0} == {"kind": "write", "table": "Web_DocHeader", "rows_affected": 3, "columns": [],
                                              "rows": [], "row_count": 0, "truncated": False, "elapsed_ms": 0}
    assert ("WRITE", sql) in sqlserver
    assert persistence.tool_audit_rows("SQL Query Execution")[0]["row_count"] == 3


async def test_desktop_tables_are_never_changed(client, sqlserver):
    res = await client.post("/tools/sql/execute", json={"sql": "DELETE FROM MstItem"})

    assert res.status_code == 400
    assert "never changed by the web app" in res.json()["detail"]
    assert sqlserver == []
    assert persistence.tool_audit_rows("SQL Query Execution")[0]["ok"] is False


async def test_execute_shows_the_sql_server_error_but_not_the_server(client, sqlserver, monkeypatch):
    res = await client.post("/tools/sql/execute", json={"sql": "SELECT BOOM FROM MstItem"})
    assert res.status_code == 400
    assert res.json()["detail"] == "SQL Server: Invalid column name 'BOOM'."

    class OperationalError(Exception):
        pass

    def dropped(*a, **k):
        raise OperationalError(20009, b"DB-Lib error message 20009: Unable to connect: Adaptive Server is unavailable or does not exist (10.1.2.3)")
    monkeypatch.setattr(db, "read_only", dropped)
    res = await client.post("/tools/sql/execute", json={"sql": "SELECT 1"})
    assert "10.1.2.3" not in res.json()["detail"]
    assert res.json()["detail"] == "SQL Server: The connection to SQL Server failed. Check the network and try again."


async def test_sql_server_needs_a_connection_but_the_local_database_does_not(client):
    res = await client.post("/tools/sql/execute", json={"sql": "SELECT 1"})
    assert res.status_code == 409
    status = (await client.get("/tools/sql/status")).json()
    assert status["db_connected"] is False and status["database"] is None and status["local_writable"] == ["web_outbox"]

    res = await client.post("/tools/sql/execute", json={"sql": "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name",
                                                        "target": "local"})
    assert res.status_code == 200
    assert ["web_outbox"] in res.json()["rows"] and ["tool_audit"] in res.json()["rows"]


async def test_local_outbox_rows_can_be_changed_but_not_the_saved_state(client):
    persistence.add_events([{"id": "e1", "kind": "sales_invoice", "doc_no": "26/c10-1", "action": "save", "user": "ADMIN",
                             "at": "2026-09-29T10:00:00", "doc": {}, "moves": []}])
    persistence.claim("e1")
    persistence.mark_failed("e1", "boom")

    res = await client.post("/tools/sql/execute", json={
        "sql": "UPDATE web_outbox SET sync_status = 'PENDING', last_sync_error = NULL WHERE sync_status = 'FAILED'", "target": "local"})

    assert res.status_code == 200 and res.json()["rows_affected"] == 1
    assert persistence.outbox_counts()["pending"] == 1
    assert (await client.post("/tools/sql/execute", json={"sql": "DELETE FROM state", "target": "local"})).status_code == 400


def test_the_sqlite_authorizer_stops_changes_the_check_would_miss():
    import sqlite3
    for sql, table in (("DELETE FROM tool_audit", "web_outbox"), ("UPDATE state SET data = ''", "state"),
                       ("ATTACH DATABASE ':memory:' AS x", None), ("PRAGMA writable_schema = 1", None),
                       ("CREATE TABLE x (a)", "web_outbox")):
        with pytest.raises(sqlite3.DatabaseError):
            persistence.run_local_sql(sql, write_table=table)


async def test_sql_tools_are_administrator_only(manager_client, cashier_client, sqlserver):
    for c in (manager_client, cashier_client):
        assert (await c.post("/tools/sql/execute", json={"sql": "SELECT 1"})).status_code == 403
        assert (await c.post("/tools/sql/execute", json={"sql": "SELECT 1", "target": "local"})).status_code == 403
        assert (await c.get("/tools/sql/objects")).status_code == 403
        assert (await c.get("/tools/audit")).status_code == 403


async def test_object_explorer_scripts_and_top_rows(client, sqlserver):
    objects = (await client.get("/tools/sql/objects", params={"search": "Item"})).json()
    assert {"kind": "Tables", "schema": "dbo", "name": "MstItem"} in objects
    assert sqlserver[-1][1] == ("%item%",)   # the search is a parameter, never pasted into the SQL

    create = (await client.get("/tools/sql/script", params={"name": "MstItem"})).json()["script"]
    assert create.startswith("CREATE TABLE [dbo].[MstItem] (")
    assert "[ItemID] [int] IDENTITY(1,1) NOT NULL" in create and "PRIMARY KEY ([ItemID])" in create
    select = (await client.get("/tools/sql/script", params={"name": "MstItem", "kind": "select"})).json()["script"]
    assert select.startswith("SELECT TOP 1000 [ItemID]") and select.endswith("FROM [dbo].[MstItem]")

    top = (await client.get("/tools/sql/top", params={"name": "MstItem", "n": 200})).json()
    assert top["total_rows"] == 7 and top["writable"] is False
    assert any(sql == "SELECT TOP 200 * FROM [dbo].[MstItem]" for sql, _ in sqlserver)
    assert (await client.get("/tools/sql/top", params={"name": "Web_DocHeader"})).json()["writable"] is True


async def test_local_explorer_lists_the_sqlite_tables(client):
    objects = (await client.get("/tools/sql/objects", params={"target": "local"})).json()
    assert {"kind": "Tables", "schema": "main", "name": "web_outbox"} in objects
    script = (await client.get("/tools/sql/script", params={"name": "web_outbox", "target": "local"})).json()["script"]
    assert "CREATE TABLE web_outbox" in script
    top = (await client.get("/tools/sql/top", params={"name": "web_outbox", "target": "local"})).json()
    assert top["writable"] is True and top["total_rows"] == 0


async def test_unknown_objects_are_404_and_names_never_reach_the_sql(client, sqlserver):
    res = await client.get("/tools/sql/top", params={"name": "x]; DROP TABLE MstItem;--"})

    assert res.status_code == 404
    assert not any("DROP" in str(sql) for sql, _ in sqlserver)


# ---------- Re Write Stock ----------

async def test_clean_stock_needs_no_rewrite(client):
    await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))

    body = (await client.get("/tools/rewrite-stock")).json()

    assert body["web"]["rewrite_required"] is False
    assert [t["key"] for t in body["web"]["tabs"]] == ["purchase", "sales_invoice", "stock_deduction", "sales_return", "ledger"]
    assert body["desktop"] is None and body["db_connected"] is False
    assert (await client.post("/tools/rewrite-stock")).json()["message"] == "Re-Write not Required"


async def test_rewrite_repairs_a_lost_movement_and_a_wrong_qoh(client):
    stock = store.ITEMS["VIN-750"]["qoh"]
    rc = store.ITEMS["RC-750"]["qoh"]
    sale = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))).json()
    await client.post("/masters/items", json={"code": "NEW-1", "name": "NEW RUM 750", "group": "RUM", "package": "750ml",
                                              "mrp": 500, "purchase_rate": 400, "qoh": 12})
    # Damage: the sale's movement is lost and RC-750's QOH drifts.
    store.MOVEMENTS[:] = [m for m in store.MOVEMENTS if m["doc_no"] != sale["doc_no"]]
    store.ITEMS["VIN-750"]["qoh"] = stock
    store.ITEMS["RC-750"]["qoh"] = rc + 5

    check = (await client.get("/tools/rewrite-stock")).json()["web"]
    tabs = {t["key"]: t["rows"] for t in check["tabs"]}
    assert tabs["sales_invoice"] == [{"item_code": "VIN-750", "item_name": store.ITEMS["VIN-750"]["name"],
                                      "trn_stock": 2, "sys_stock": 0, "diff": 2}]
    assert [r["item_code"] for r in tabs["ledger"]] == ["RC-750"]

    res = await client.post("/tools/rewrite-stock")

    assert res.status_code == 200 and res.json()["rewritten"] is True
    assert store.ITEMS["VIN-750"]["qoh"] == stock - 2
    assert store.ITEMS["RC-750"]["qoh"] == rc
    assert store.ITEMS["NEW-1"]["qoh"] == 12   # the opening movement is kept
    assert res.json()["check"]["rewrite_required"] is False
    assert persistence.tool_audit_rows("Re Write Stock")[0]["ok"] is True


async def test_rewrite_follows_cancellations_returns_and_deductions(client):
    stock = store.ITEMS["VIN-750"]["qoh"]
    cancelled = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 3}]))).json()
    await client.post(f"/transactions/sales-invoice/{cancelled['doc_no']}/cancel", json={"remarks": "wrong item"})
    sold = (await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))).json()
    await client.post("/transactions/sales-return", json={"date": TODAY, "against_invoice": sold["doc_no"],
                                                          "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    await client.post("/transactions/stock-deduction", json={"date": TODAY, "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 1}], hold=True))
    store.MOVEMENTS[store.APP_MOVEMENTS_FROM:] = []   # lose every app movement
    store.ITEMS["VIN-750"]["qoh"] = stock

    await client.post("/tools/rewrite-stock")

    assert store.ITEMS["VIN-750"]["qoh"] == stock - 2 + 1 - 1
    assert (await client.get("/tools/rewrite-stock")).json()["web"]["rewrite_required"] is False


async def test_rewrite_refuses_when_an_item_would_go_negative(client):
    stock = store.ITEMS["RC-750"]["qoh"]
    await client.post("/transactions/sales-invoice", json=bill([{"item_code": "RC-750", "bottles": int(stock)}]))
    store.BASE_QOH["RC-750"] = 0          # as if the opening stock had never been there
    before = list(store.MOVEMENTS), store.ITEMS["RC-750"]["qoh"]
    store.ITEMS["RC-750"]["qoh"] = 3      # make a rewrite necessary

    res = await client.post("/tools/rewrite-stock")

    assert res.status_code == 409
    assert "would go negative" in res.json()["detail"]
    assert store.ITEMS["RC-750"]["name"] in res.json()["detail"]
    assert store.MOVEMENTS == before[0] and store.ITEMS["RC-750"]["qoh"] == 3


async def test_rewritten_stock_survives_a_restart(client, tmp_path, monkeypatch):
    monkeypatch.setattr(persistence, "PATH", str(tmp_path / "state.sqlite3"))
    stock = store.ITEMS["VIN-750"]["qoh"]
    await client.post("/transactions/sales-invoice", json=bill([{"item_code": "VIN-750", "bottles": 2}]))
    store.ITEMS["VIN-750"]["qoh"] = stock + 40
    await client.post("/tools/rewrite-stock")
    saved_moves = len(store.MOVEMENTS)

    # Restart: seed QOH, no app movements, then load the file.
    store.ITEMS["VIN-750"]["qoh"] = stock
    del store.MOVEMENTS[store.APP_MOVEMENTS_FROM:]
    assert persistence.load()

    assert store.ITEMS["VIN-750"]["qoh"] == stock - 2
    assert len(store.MOVEMENTS) == saved_moves


async def test_rewrite_stock_needs_the_module_right(manager_client, cashier_client):
    for c in (manager_client, cashier_client):
        assert (await c.get("/tools/rewrite-stock")).status_code == 403
        assert (await c.post("/tools/rewrite-stock")).status_code == 403


async def test_movements_of_bills_not_loaded_are_kept(client):
    stock = store.ITEMS["VIN-750"]["qoh"]
    # A sale-cancel made in the app for a DB bill that isn't loaded (yet) after a restart.
    store.move_stock("VIN-750", 2, "sale-cancel", "26/DB-0000001", date.today(), 100)
    store.ITEMS["RC-750"]["qoh"] += 1   # something else needs a rewrite

    await client.post("/tools/rewrite-stock")

    assert store.ITEMS["VIN-750"]["qoh"] == stock + 2
    assert any(m["doc_no"] == "26/DB-0000001" for m in store.MOVEMENTS)


async def test_deleting_and_recreating_an_item_starts_its_stock_again(client):
    item = {"code": "NEW-1", "name": "NEW RUM 750", "group": "RUM", "package": "750ml", "mrp": 500, "purchase_rate": 400}
    await client.post("/masters/items", json=item | {"qoh": 12})
    assert (await client.delete("/masters/items/NEW-1")).status_code == 204
    await client.post("/masters/items", json=item | {"qoh": 5})

    check = (await client.get("/tools/rewrite-stock")).json()["web"]

    assert check["rewrite_required"] is False
    assert store.ITEMS["NEW-1"]["qoh"] == 5
