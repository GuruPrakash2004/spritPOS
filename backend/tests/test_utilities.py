"""Tests for the Utilities menu (app/routers/utilities.py): Global Settings rows and what they change,
the ADMIN Audit Log recording, and About."""
from datetime import date, timedelta

from app import sqlstore, store
from conftest import enc

TODAY = date.today().isoformat()


async def _sell(client, qty=1, day=TODAY, item="VIN-750"):
    return await client.post("/transactions/sales-invoice", json={
        "date": day, "counter": "counter10", "customer": "CASH", "salesperson": "GURU",
        "lines": [{"item_code": item, "bottles": qty}]})


# ─── Global Settings ─────────────────────────────────────────────────────────

async def test_settings_grid_lists_rows_with_their_effect_and_smtp_without_password(client, monkeypatch):
    monkeypatch.setenv("SMTP_HOST", "smtp.example.test")
    monkeypatch.setenv("SMTP_PASSWORD", "never-shown")
    rows = (await client.get("/utilities/settings")).json()
    by = {r["code"]: r for r in rows}
    assert by["SPID"]["value"] == "Y" and by["SPID"]["effect"]
    assert by["SMTPHostName"]["value"] == "smtp.example.test" and not by["SMTPHostName"]["readonly"]
    assert by["SMTPPassword"]["value"] == "" and by["SMTPPassword"]["saved"]
    assert "never-shown" not in str(rows)
    assert "PointsAlertAbove" not in by                     # hidden row
    assert "PointsAlertAbove" in {r["code"] for r in (await client.get("/utilities/settings?all=true")).json()}


async def test_settings_are_admin_only(cashier_client):
    assert (await cashier_client.get("/utilities/settings")).status_code == 403
    assert (await cashier_client.put("/utilities/settings/SPID", json={"caption": "x", "value": "N"})).status_code == 403
    assert (await cashier_client.get("/settings/values")).status_code == 200


async def test_spid_setting_decides_if_salesperson_is_required(client):
    body = {"date": TODAY, "counter": "counter10", "customer": "CASH",
            "lines": [{"item_code": "VIN-750", "bottles": 1}]}
    assert (await client.post("/transactions/sales-invoice", json=body)).status_code == 422
    res = await client.put("/utilities/settings/SPID", json={"caption": "Sales Person required in invoice", "value": "n"})
    assert res.status_code == 200 and res.json()["value"] == "N"
    assert (await client.post("/transactions/sales-invoice", json=body)).status_code == 201
    assert store.SETTINGS["invoice"]["salesperson_required"] is False     # older /settings view follows


async def test_setting_values_are_checked_by_type(client):
    assert (await client.put("/utilities/settings/SPID", json={"caption": "c", "value": "maybe"})).status_code == 422
    assert (await client.put("/utilities/settings/PointsForOneRs", json={"caption": "c", "value": "abc"})).status_code == 422
    assert (await client.put("/utilities/settings/FreezeDate", json={"caption": "c", "value": "31-12-2025"})).status_code == 422


async def test_freeze_date_blocks_documents_on_or_before_it(client):
    yesterday = (date.today() - timedelta(days=1)).isoformat()
    await client.put("/utilities/settings/FreezeDate", json={"caption": "Freeze Date", "value": yesterday})
    res = await _sell(client, day=yesterday)
    assert res.status_code == 409 and "freeze date" in res.json()["detail"]
    assert (await _sell(client)).status_code == 201


async def test_points_settings_change_points_earned(client):
    await client.put("/utilities/settings/MinBillForPoints", json={"caption": "c", "value": "0"})
    await client.put("/utilities/settings/PointsForOneRs", json={"caption": "c", "value": "0.1"})
    cust = next(c for c in store.CUSTOMERS if c != "CASH")
    res = await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "customer": cust, "salesperson": "GURU",
        "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    assert res.json()["totals"]["points_earned"] == round(res.json()["totals"]["net"] * 0.1, 2)


async def test_purchase_selling_variation_checks_item_mrp(client):
    await client.put("/utilities/settings/PurSellVarPer", json={"caption": "c", "value": "10"})
    item = dict(store.ITEMS["VIN-750"], purchase_rate=800, mrp=850)
    res = await client.put("/masters/items/VIN-750", json=item)
    assert res.status_code == 422 and "10%" in res.json()["detail"]
    assert (await client.put("/masters/items/VIN-750", json=dict(item, mrp=880))).status_code == 200


async def test_custom_setting_can_be_added_once(client):
    body = {"code": "MyNote", "group": "Custom", "caption": "Shop note", "value": "hello"}
    res = await client.post("/utilities/settings", json=body)
    assert res.status_code == 201 and res.json()["source"] == "custom"
    assert (await client.post("/utilities/settings", json=body)).status_code == 409
    assert (await client.post("/utilities/settings", json=dict(body, code="9bad"))).status_code == 422


async def test_old_settings_api_still_works_and_updates_rows(client):
    res = await client.put("/settings", json={"invoice": {"salesperson_required": False, "print_preview": True,
                                                           "barcode_scan_jumps_to_qty": True}})
    assert res.status_code == 200
    assert store.GSETTINGS["SPID"]["value"] == "N" and store.GSETTINGS["InvPreview"]["value"] == "Y"


# ─── ADMIN Audit Log ─────────────────────────────────────────────────────────

async def test_item_edit_invoice_and_settings_are_audited(client):
    item = dict(store.ITEMS["VIN-750"], mrp=store.ITEMS["VIN-750"]["mrp"] + 10)
    assert (await client.put("/masters/items/VIN-750", json=item)).status_code == 200
    doc = (await _sell(client, qty=2)).json()
    await client.put("/utilities/settings/SPID", json={"caption": "Sales Person required in invoice", "value": "N"})

    rows = (await client.get("/utilities/audit")).json()
    edit = next(r for r in rows if r["screen"] == "Item Master" and r["main_fld"] == "VIN-750")
    assert edit["action"] == "Edited" and edit["user_code"] == "ADMIN" and "mrp" in edit["changed"]
    bill = next(r for r in rows if r["screen"] == "Sales Invoice" and r["main_fld"] == doc["doc_no"])
    assert bill["items"][0]["code"] == "VIN-750" and bill["items"][0]["status"] == "Added"
    assert any(r["screen"] == "Global Settings" and r["main_fld"] == "SPID" for r in rows)
    only = (await client.get("/utilities/audit", params={"screen": "Sales Invoice"})).json()
    assert {r["screen"] for r in only} == {"Sales Invoice"}
    assert "Item Master" in (await client.get("/utilities/audit/screens")).json()


async def test_refused_change_is_not_audited(client):
    await client.put("/utilities/settings/SPID", json={"caption": "c", "value": "bad"})
    assert not [r for r in (await client.get("/utilities/audit")).json() if r["screen"] == "Global Settings"]


async def test_passwords_never_reach_the_audit(client):
    from conftest import enc
    await client.post("/admin/users", json={"user": "ANITHA", "password": enc("Counter#12", "user"), "role": "Cashier"})
    await client.put("/admin/users/ANITHA", json={"user": "ANITHA", "password": enc("Counter#34", "user"), "role": "Cashier"})
    rows = [r for r in (await client.get("/utilities/audit")).json() if r["screen"] == "Admin Master (Security)"]
    assert [r["action"] for r in rows] == ["User edited", "User added"]
    assert "Counter#" not in str(rows) and "hash" not in str(rows)


async def test_tools_runs_show_in_the_audit_log(client):
    sqlstore.audit_tool("ADMIN", "SQL Query Execution", "SELECT 1", True, 1)
    rows = (await client.get("/utilities/audit", params={"screen": "SQL Query Execution"})).json()
    assert rows and rows[0]["changed"].startswith("SELECT 1")


async def test_audit_log_is_admin_only(cashier_client):
    assert (await cashier_client.get("/utilities/audit")).status_code == 403


# ─── About ───────────────────────────────────────────────────────────────────

async def test_about(cashier_client):
    res = (await cashier_client.get("/utilities/about")).json()
    assert res["product"] == "e-vLQMS ERP" and res["software_version"] and "db_connected" in res
    assert res["client_system_name"]   # the PC's own name (the server's when the caller is local)


def test_client_system_name_uses_the_callers_pc(monkeypatch):
    from types import SimpleNamespace
    from app.routers import utilities
    monkeypatch.setattr(utilities.socket, "gethostbyaddr", lambda host: ("client-pc.lan", [], [host]))
    utilities._CLIENT_NAMES.clear()
    assert utilities.client_system_name(SimpleNamespace(client=SimpleNamespace(host="192.168.1.20"), headers={})) == "CLIENT-PC"
    # behind a proxy the browser's address is the first X-Forwarded-For entry
    proxied = SimpleNamespace(client=SimpleNamespace(host="127.0.0.1"), headers={"x-forwarded-for": "192.168.1.20, 10.0.0.1"})
    assert utilities.client_system_name(proxied) == "CLIENT-PC"
    monkeypatch.setattr(utilities.socket, "gethostbyaddr", lambda host: (_ for _ in ()).throw(OSError()))
    utilities._CLIENT_NAMES.clear()
    assert utilities.client_system_name(SimpleNamespace(client=SimpleNamespace(host="192.168.1.21"), headers={})) == utilities.platform.node().upper()


# ─── Remove Duplicate Invoice (711) ──────────────────────────────────────────

async def test_duplicate_bills_are_flagged(client):
    first = (await _sell(client, qty=2)).json()
    second = (await _sell(client, qty=2)).json()
    other = (await _sell(client, qty=1)).json()
    rows = {r["doc_no"]: r for r in (await client.get("/utilities/duplicate-invoices")).json()}
    assert rows[second["doc_no"]]["duplicate_of"] == first["doc_no"]
    assert rows[first["doc_no"]]["duplicate_of"] is None and rows[other["doc_no"]]["duplicate_of"] is None
    assert rows[first["doc_no"]]["added_by"] == "ADMIN" and rows[first["doc_no"]]["removable"]
    only = (await client.get("/utilities/duplicate-invoices", params={"only_duplicates": True})).json()
    assert [r["doc_no"] for r in only] == [second["doc_no"]]


async def test_removing_a_duplicate_puts_stock_back_and_rewrite_stays_clean(client):
    before = store.ITEMS["VIN-750"]["qoh"]
    doc = (await _sell(client, qty=2)).json()
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("admin", "admin")})
    assert res.status_code == 200 and res.json()["removed"] == [doc["doc_no"]]
    assert store.ITEMS["VIN-750"]["qoh"] == before
    assert doc["doc_no"] not in store.DOCS["sales_invoice"]                 # KeepDelData = N
    assert (await client.get("/tools/rewrite-stock")).json()["web"]["rewrite_required"] is False
    audit = [r for r in (await client.get("/utilities/audit")).json() if r["screen"] == "Remove Duplicate Invoice"]
    assert audit[0]["main_fld"] == doc["doc_no"] and audit[0]["items"][0]["status"] == "Removed"


async def test_keep_deleted_data_keeps_the_bill_out_of_reports(client):
    await client.put("/utilities/settings/KeepDelData", json={"caption": "Keep Deleted Data", "value": "Y"})
    before = (await client.get("/reports/dsr", params={"on": TODAY})).json()
    doc = (await _sell(client, qty=1)).json()
    assert (await client.get("/reports/dsr", params={"on": TODAY})).json() != before
    await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("admin", "admin")})
    assert store.DOCS["sales_invoice"][doc["doc_no"]]["status"] == "DELETED"
    after = (await client.get("/reports/dsr", params={"on": TODAY})).json()
    assert after == before and doc["doc_no"] not in str(after)
    assert (await client.get("/tools/rewrite-stock")).json()["web"]["rewrite_required"] is False
    listed = [r["doc_no"] for r in (await client.get("/utilities/duplicate-invoices")).json()]
    assert doc["doc_no"] not in listed


async def test_desktop_and_returned_bills_cannot_be_removed(client):
    doc = (await _sell(client, qty=2)).json()
    store.DOCS["sales_invoice"][doc["doc_no"]]["origin"] = "db"
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("admin", "admin")})
    assert res.status_code == 409 and "desktop" in res.json()["detail"]
    del store.DOCS["sales_invoice"][doc["doc_no"]]["origin"]
    await client.post("/transactions/sales-return", json={"date": TODAY, "against_invoice": doc["doc_no"],
                                                          "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("admin", "admin")})
    assert res.status_code == 409 and "returned" in res.json()["detail"]
    assert store.DOCS["sales_invoice"][doc["doc_no"]]["status"] == "SAVED"


async def test_remove_duplicate_needs_the_module(cashier_client):
    assert (await cashier_client.get("/utilities/duplicate-invoices")).status_code == 403


# ─── Merge Items (713) / Replace and Remove Duplicate items (712) ────────────

async def test_merge_moves_lines_stock_and_keeps_rewrite_clean(client):
    a, b = "RC-750", "VIN-750"
    qa, qb = store.ITEMS[a]["qoh"], store.ITEMS[b]["qoh"]
    bill = (await _sell(client, qty=1, item=a)).json()
    prev = (await client.get("/utilities/merge-items/merge/preview", params={"from_item": a, "to_item": b})).json()
    assert prev["lines"]["sales_invoice"] >= 1 and prev["qoh_after"] == qa - 1 + qb

    res = await client.post("/utilities/merge-items/merge", json={"from_item": a, "to_item": b, "admin_password": enc("admin", "admin")})
    assert res.status_code == 200 and res.json()["message"] == "Successfully Merged!"
    assert a not in store.ITEMS and store.ITEMS[b]["qoh"] == qa - 1 + qb
    assert store.DOCS["sales_invoice"][bill["doc_no"]]["lines"][0]["item_code"] == b
    assert not any(m["item"] == a for m in store.MOVEMENTS)
    assert store.MERGED_ITEMS[a] == b and store.merged_code(a) == b
    assert (await client.get("/tools/rewrite-stock")).json()["web"]["rewrite_required"] is False
    audit = [r for r in (await client.get("/utilities/audit")).json() if r["screen"] == "Merge Items"]
    assert audit and audit[0]["action"] == "Merged"


async def test_merge_refuses_invalid_choices(client):
    assert (await client.post("/utilities/merge-items/merge", json={"from_item": "NOPE", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})).status_code == 422
    assert (await client.post("/utilities/merge-items/merge", json={"from_item": "VIN-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})).status_code == 422
    store.ITEMS["RC-750"]["active"] = False
    res = await client.post("/utilities/merge-items/merge", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    assert res.status_code == 422 and "active" in res.json()["detail"]
    # Replace and Remove takes inactive items too.
    res = await client.post("/utilities/merge-items/replace", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    assert res.status_code == 200 and res.json()["message"] == "Item replaced and removed successfully"


async def test_merge_needs_its_module(cashier_client):
    res = await cashier_client.post("/utilities/merge-items/merge", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    assert res.status_code == 403


# ─── Compare Stock (714) ─────────────────────────────────────────────────────

async def test_compare_stock_current_sheet(client, cashier_client):
    res = (await client.get("/utilities/compare-stock/current")).json()
    vin = next(r for r in res["rows"] if r["code"] == "VIN-750")
    it = store.ITEMS["VIN-750"]
    assert res["basis"] == "purchase rate" and vin["value"] == round(it["qoh"] * it["purchase_rate"], 2)
    assert (await cashier_client.get("/utilities/compare-stock/current")).status_code == 403


# ─── Review fixes ────────────────────────────────────────────────────────────

async def test_closing_stock_generate_invoice_records_who_and_is_audited(client):
    qoh = store.ITEMS["VIN-750"]["qoh"]
    cse = (await client.post("/transactions/closing-stock", json={"as_on": TODAY, "lines": [
        {"item_code": "VIN-750", "closing_qty": qoh - 1}]})).json()
    res = await client.post(f"/transactions/closing-stock/{cse['doc_no']}/generate-invoice",
                            json={"counter": "counter10", "salesperson": "GURU"})
    assert res.status_code == 201 and res.json()["added_by"] == "ADMIN"
    screens = {r["screen"] for r in (await client.get("/utilities/audit")).json()}
    assert {"Closing Stock Entry", "Sales Invoice"} <= screens


async def test_number_settings_refuse_infinity_and_nan_and_respect_caps(client):
    for bad in ("Infinity", "nan", "1e400", "2"):
        res = await client.put("/utilities/settings/PointsForOneRs", json={"caption": "c", "value": bad})
        assert res.status_code == 422, bad
    res = await client.put("/settings", headers={"Content-Type": "application/json"}, content=(
        '{"loyalty": {"points_per_rupee": 0.01, "alert_when_points_above": 1e400, "min_bill_amount": 1000}}'))
    assert res.status_code == 422


async def test_margin_check_applies_to_roles_that_cannot_see_cost(client, cashier_client):
    await client.put("/utilities/settings/PurSellVarPer", json={"caption": "c", "value": "10"})
    item = dict(store.ITEMS["VIN-750"], purchase_rate=None, mrp=round(store.ITEMS["VIN-750"]["purchase_rate"] * 1.05, 2))
    res = await cashier_client.put("/masters/items/VIN-750", json=item)
    assert res.status_code == 422 and "10%" in res.json()["detail"]


async def test_hidden_compat_setting_stays_hidden_and_values_skip_smtp(client, monkeypatch):
    monkeypatch.setenv("SMTP_HOST", "smtp.example.test")
    res = await client.put("/utilities/settings/PointsAlertAbove", json={"caption": "c", "value": "10", "hidden": False})
    assert res.status_code == 409
    values = (await client.get("/settings/values")).json()
    assert "SMTPHostName" not in values and "smtp.example.test" not in str(values)


def test_saved_settings_restore_only_values_of_seeded_rows():
    from app import gsettings
    gsettings.restore({
        "SPID": {"value": "N", "caption": "Salesperson?", "effect": "old wording", "kind": "text", "hidden": False},
        "DroppedRow": {"value": "Y", "source": "web", "effect": "something removed"},
        "Mine": {"code": "Mine", "group": "Custom", "caption": "x", "value": "1", "kind": "text", "remarks": "",
                 "effect": None, "hidden": False, "source": "custom"},
        "PointsForOneRs": {"value": "inf"},
    })
    spid = store.GSETTINGS["SPID"]
    assert spid["value"] == "N" and spid["caption"] == "Salesperson?"
    assert spid["effect"] != "old wording" and spid["kind"] == "yn"
    assert "DroppedRow" not in store.GSETTINGS and "Mine" in store.GSETTINGS
    assert store.GSETTINGS["PointsForOneRs"]["value"] == "0.01"


async def test_held_bill_discard_and_other_documents_are_audited(client):
    held = (await client.post("/transactions/sales-invoice", json={
        "date": TODAY, "counter": "counter10", "customer": "CASH", "salesperson": "GURU", "hold": True,
        "lines": [{"item_code": "VIN-750", "bottles": 1}]})).json()
    assert (await client.delete(f"/transactions/sales-invoice/{held['doc_no']}")).status_code == 204
    await client.post("/transactions/stock-deduction", json={"date": TODAY, "lines": [{"item_code": "VIN-750", "bottles": 1}]})
    rows = (await client.get("/utilities/audit")).json()
    assert any(r["action"] == "Held bill discarded" and r["main_fld"] == held["doc_no"] for r in rows)
    assert any(r["screen"] == "Stock Deduction" for r in rows)


# ─── Phase 2 review fixes ────────────────────────────────────────────────────

async def test_a_removed_bill_cannot_be_cancelled(client):
    await client.put("/utilities/settings/KeepDelData", json={"caption": "Keep Deleted Data", "value": "Y"})
    before = store.ITEMS["VIN-750"]["qoh"]
    doc = (await _sell(client, qty=2)).json()
    await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("admin", "admin")})
    res = await client.post(f"/transactions/sales-invoice/{doc['doc_no']}/cancel", json={"remarks": "again"})
    assert res.status_code == 409 and "duplicate" in res.json()["detail"]
    assert store.ITEMS["VIN-750"]["qoh"] == before


async def test_merge_adds_up_closing_stock_lines_and_respects_the_freeze_date(client):
    qa, qb = store.ITEMS["RC-750"]["qoh"], store.ITEMS["VIN-750"]["qoh"]
    cse = (await client.post("/transactions/closing-stock", json={"as_on": TODAY, "lines": [
        {"item_code": "RC-750", "closing_qty": qa}, {"item_code": "VIN-750", "closing_qty": qb}]})).json()
    await client.post("/utilities/merge-items/merge", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    lines = store.DOCS["closing_stock"][cse["doc_no"]]["lines"]
    assert len(lines) == 1 and lines[0]["closing_qty"] == qa + qb

    yesterday = (date.today() - timedelta(days=1)).isoformat()
    await _sell(client, qty=1, item="OM-750", day=yesterday)
    await client.put("/utilities/settings/FreezeDate", json={"caption": "Freeze Date", "value": yesterday})
    res = await client.post("/utilities/merge-items/merge", json={"from_item": "OM-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    assert res.status_code == 409 and "freeze date" in res.json()["detail"]


# ─── Additional ADMIN password (Admin Master (Security)) ─────────────────────

async def test_destructive_utilities_need_the_admin_password(client):
    doc = (await _sell(client, qty=1)).json()
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]]})
    assert res.status_code == 403 and "your password" in res.json()["detail"]
    res = await client.post("/utilities/duplicate-invoices/remove", json={"doc_nos": [doc["doc_no"]], "admin_password": enc("wrong", "admin")})
    assert res.status_code == 403
    # Set an additional ADMIN password: from then on it, not the login password, is asked.
    res = await client.put("/admin/adnl-admin-password", json={"current_password": enc("admin", "current"), "new_password": enc("Extra#123", "new")})
    assert res.status_code == 200 and res.json()["adnl_admin_set"]
    res = await client.post("/utilities/merge-items/merge", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("admin", "admin")})
    assert res.status_code == 403 and "additional ADMIN" in res.json()["detail"]
    res = await client.post("/utilities/merge-items/merge", json={"from_item": "RC-750", "to_item": "VIN-750", "admin_password": enc("Extra#123", "admin")})
    assert res.status_code == 200
    assert "Extra#123" not in str((await client.get("/utilities/audit")).json())


async def test_smtp_rows_of_the_grid_are_edited_and_saved_to_env(client, monkeypatch, tmp_path):
    """The SMTP rows write backend/.env like the Email (SMTP) tab; the password is encrypted, sealed and never listed."""
    from app import envfile, mailer
    from tests.conftest import enc
    monkeypatch.setattr(envfile, "PATH", tmp_path / ".env")
    for key in ("SMTP_HOST", "SMTP_PORT", "SMTP_SECURITY", "SMTP_USER", "SMTP_FROM", "SMTP_PASSWORD", "SMTP_PASSWORD_ENC"):
        monkeypatch.delenv(key, raising=False)
    put = lambda code, value: client.put(f"/utilities/settings/{code}", json={"caption": "c", "value": value})
    assert (await put("SMTPHostName", "smtp.example.com")).json()["value"] == "smtp.example.com"
    assert (await put("SMTPServerPort", "465")).status_code == 200
    assert (await put("SMTPSecurity", "ssl")).status_code == 200
    assert (await put("SMTPMailID", "shop@example.com")).status_code == 200
    r = await put("SMTPPassword", enc("S3cret!pw", "smtp"))
    assert r.status_code == 200 and r.json()["value"] == "" and r.json()["saved"]
    assert mailer.password() == "S3cret!pw" and mailer.configured()
    text = (tmp_path / ".env").read_text()
    assert "SMTP_HOST=smtp.example.com" in text and "SMTP_PORT=465" in text and "S3cret" not in text
    rows = (await client.get("/utilities/settings")).json()
    assert {r["code"]: r["value"] for r in rows}["SMTPServerPort"] == "465" and "S3cret" not in str(rows)
    assert (await put("SMTPServerPort", "99999")).status_code == 422
    assert (await put("SMTPSecurity", "weird")).status_code == 422
    assert (await put("SMTPFromID", "not-an-email")).status_code == 422
    assert (await put("SMTPPassword", "")).status_code == 200 and mailer.password() == "S3cret!pw"   # blank keeps it


# ─── Refresh from Doc Designer (top bar button) ──────────────────────────────

async def test_reload_from_designer_is_for_the_administrator_and_needs_the_database(client, cashier_client):
    assert (await cashier_client.post("/designer/reload")).status_code == 403
    res = await client.post("/designer/reload")
    assert res.status_code == 409   # the tests run without a database: nothing to read again
