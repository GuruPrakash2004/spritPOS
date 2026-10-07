"""Printing the desktop's .rpt formats: the print options come from the Doc Designer's tables (store.UTL_*), the job
is built the way frmUtlReportViewer builds it, and Crystal Reports is reached through the bridge (mocked here)."""
import json

import httpx
import pytest
import respx

from app import rpt_bridge, rpt_print, store

BRIDGE = "http://127.0.0.1:8765"


@pytest.fixture
def rpts(tmp_path, monkeypatch):
    for n in ("FMTSINV.rpt", "FMTSINV_GST.rpt", "fmtSINV80.rpt", "fmtPB.rpt"):
        (tmp_path / n).write_bytes(b"rpt")
    monkeypatch.setenv("RPT_FOLDER", str(tmp_path))
    monkeypatch.setenv("RPT_BRIDGE_URL", BRIDGE)
    monkeypatch.setenv("RPT_BRIDGE_KEY", "k")
    monkeypatch.setattr(rpt_print, "document_id", lambda kind, doc_no: 7)
    store.COMPANY.update({"name": "THE LIQUOR HOUSE", "address": "MG Road", "gstin": "29AAAAA0000A1Z5", "phone": "98450"})


def design(options):
    store.UTL_DOCS[5] = {"DocID": 5, "DocName": "Sales Invoice", "DocShForm": "SINV"}
    store.UTL_OPTIONS.extend({"DocID": 5, "OptionOrder": i + 1, "InActive": "N", **o} for i, o in enumerate(options))


async def test_options_follow_the_designer_and_mark_the_default(client, rpts):
    design([{"OptionName": "Thermal", "CrptFileName": "fmtSINV80", "NoOfCopies": 2, "DefaultPrint": "N", "ClickOperation": "P"},
            {"OptionName": "GST", "CrptFileName": "FMTSINV_GST", "DefaultPrint": "Y", "ClickOperation": "D",
             "CtrlClickOperation": "P", "AfterSaveAction": "PP", "DirectPrinterName": "POS-1"}])
    r = (await client.get("/print/options", params={"kind": "sales_invoice"})).json()
    assert r["designed"] and r["default"] == "GST" and r["doc_id"] == 5
    gst = next(o for o in r["options"] if o["name"] == "GST")
    assert (gst["click"], gst["ctrl_click"], gst["after_save"], gst["printer"]) == ("direct", "preview", "PP", "POS-1")
    assert next(o for o in r["options"] if o["name"] == "Thermal")["copies"] == 2


async def test_the_only_option_is_the_default(client, rpts):
    design([{"OptionName": "Only", "CrptFileName": "fmtSINV80"}])
    assert (await client.get("/print/options", params={"kind": "sales_invoice"})).json()["default"] == "Only"


async def test_render_builds_the_desktop_job_and_returns_the_pdf(client, rpts):
    design([{"OptionName": "Std", "CrptFileName": "FMTSINV_GST", "DefaultPrint": "Y", "SelFormula": "{TrnHdrSINV.SINVID} =",
             "FormulasFld": "compname;gstin", "FormulasVal": "MstComp.CompName;MstComp.GSTNO"}])
    with respx.mock(assert_all_called=True) as m:
        route = m.post(BRIDGE + "/render").mock(return_value=httpx.Response(200, content=b"%PDF-1.4 x"))
        r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "26/S-0000001"})
    assert r.status_code == 200 and r.headers["content-type"] == "application/pdf" and r.content.startswith(b"%PDF")
    sent = json.loads(route.calls[0].request.content)
    assert route.calls[0].request.headers["X-Bridge-Key"] == "k"
    assert sent["rpt"] == "FMTSINV_GST.rpt" and sent["selection"] == "{TrnHdrSINV.SINVID} = 7"
    assert sent["parameters"] == {"compname": "THE LIQUOR HOUSE", "gstin": "29AAAAA0000A1Z5"}
    assert sent["table_prefix"] == "" and "logon" in sent


async def test_a_document_with_no_print_option_uses_the_desktops_file_mode(client, rpts):
    with respx.mock() as m:
        route = m.post(BRIDGE + "/render").mock(return_value=httpx.Response(200, content=b"%PDF"))
        r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "26/S-0000001", "rpt": "fmtsinv80"})
    assert r.status_code == 200
    sent = json.loads(route.calls[0].request.content)
    assert sent["rpt"] == "fmtSINV80.rpt" and sent["selection"] == "{TRNHDRSInv.SInvID} = 7"
    assert sent["parameters"]["compname"] == "THE LIQUOR HOUSE" and sent["parameters"]["gstin"] == "29AAAAA0000A1Z5"
    listed = (await client.get("/print/options", params={"kind": "sales_invoice"})).json()
    assert not listed["designed"] and {"FMTSINV.rpt", "FMTSINV_GST.rpt", "fmtSINV80.rpt"} <= set(listed["files"])


async def test_direct_print_goes_to_the_options_printer(client, rpts):
    design([{"OptionName": "Std", "CrptFileName": "FMTSINV", "DefaultPrint": "Y", "ClickOperation": "D",
             "DirectPrinterName": "POS-1", "NoOfCopies": 2}])
    with respx.mock() as m:
        route = m.post(BRIDGE + "/print").mock(return_value=httpx.Response(200, json={"printed": True}))
        r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "26/S-0000001"})
    assert r.json() == {"printed": True, "printer": "POS-1", "copies": 2, "rpt": "FMTSINV"}
    assert json.loads(route.calls[0].request.content)["printer"] == "POS-1"


async def test_ctrl_click_uses_the_ctrl_click_operation(client, rpts):
    design([{"OptionName": "Std", "CrptFileName": "FMTSINV", "DefaultPrint": "Y", "ClickOperation": "D",
             "CtrlClickOperation": "P", "DirectPrinterName": "POS-1"}])
    with respx.mock() as m:
        m.post(BRIDGE + "/render").mock(return_value=httpx.Response(200, content=b"%PDF"))
        r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x", "click": "C"})
    assert r.headers["content-type"] == "application/pdf"


async def test_missing_rpt_and_missing_bridge_are_clear_errors(client, rpts, monkeypatch):
    design([{"OptionName": "Std", "CrptFileName": "NoSuchFile", "DefaultPrint": "Y"}])
    r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x"})
    assert r.status_code == 404 and "NoSuchFile.rpt" in r.json()["detail"]
    monkeypatch.delenv("RPT_BRIDGE_URL")
    monkeypatch.setattr(rpt_bridge, "local_bridge", lambda: False)   # no bridge built on this PC
    r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x", "option": "Std"})
    assert r.status_code == 503 and "bridge" in r.json()["detail"].lower()


async def test_a_bridge_on_another_pc_is_refused_unless_allowed(client, rpts, monkeypatch):
    monkeypatch.setenv("RPT_BRIDGE_URL", "http://10.1.2.3:8765")
    r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x"})
    assert r.status_code == 503 and "this PC" in r.json()["detail"]


async def test_a_bridge_error_is_reported_not_swallowed(client, rpts):
    with respx.mock() as m:
        m.post(BRIDGE + "/render").mock(return_value=httpx.Response(500, json={"error": "Logon failed"}))
        r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x"})
    assert r.status_code == 502 and "Logon failed" in r.json()["detail"]


async def test_rpt_names_cannot_leave_the_folder(client, rpts):
    r = await client.post("/print/render", json={"kind": "sales_invoice", "doc_no": "x", "rpt": "../secret"})
    assert r.status_code == 422


async def test_multi_report_option_picks_the_file_by_the_saved_document(rpts, monkeypatch):
    design([{"OptionName": "M", "MultiRpt": "Y", "CrptFileName": "x",
             "MultiRptString1": "1åfmtPBå#h.PayTypeå=åCASHå\x00".replace("\x00", "") + "ç"}])
    monkeypatch.setattr(rpt_print.db, "fetchall", lambda sql, params=(): [{"x": 1}])
    assert rpt_print.multi_rpt_name(store.UTL_OPTIONS[0], "SINV", 7) == "fmtPB"


async def test_only_admin_can_refresh_designer_settings(cashier_client):
    assert (await cashier_client.post("/designer/refresh")).status_code == 403


async def test_printing_needs_a_login(anon_client):
    assert (await anon_client.get("/print/options", params={"kind": "sales_invoice"})).status_code == 401
