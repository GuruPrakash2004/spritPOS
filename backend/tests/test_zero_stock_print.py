"""Print Zero Stock Items through the desktop's RptZBItem.rpt: the format list, and a clear refusal while the database or the file is missing."""
from app import rpt_bridge, rpt_zeroitems


async def test_zero_stock_formats_lists_only_the_rpt_that_is_in_the_folder(client, monkeypatch):
    monkeypatch.setattr(rpt_bridge, "rpt_files", lambda: ["RptDSR.rpt"])
    res = await client.get("/print/zero-stock-formats")
    assert res.status_code == 200
    assert res.json()["formats"] == []
    monkeypatch.setattr(rpt_bridge, "rpt_files", lambda: ["rptzbitem.rpt", "RptDSR.rpt"])
    assert (await client.get("/print/zero-stock-formats")).json()["formats"] == ["rptzbitem.rpt"]


async def test_zero_stock_print_needs_the_database(client):
    res = await client.post("/print/zero-stock", json={})
    assert res.status_code == 409   # the tests run with no database: Crystal would have nothing to read


async def test_zero_stock_job_is_the_rpt_with_no_selection(monkeypatch):
    seen = {}
    monkeypatch.setattr(rpt_zeroitems.sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(rpt_bridge, "rpt_files", lambda: ["RptZBItem.rpt"])
    monkeypatch.setattr(rpt_bridge, "render_pdf", lambda job: seen.setdefault("job", job) and b"%PDF")
    monkeypatch.setattr(rpt_bridge, "print_direct", lambda job: seen.setdefault("direct", job))
    assert rpt_zeroitems.render() == b"%PDF"
    assert seen["job"]["rpt"] == "RptZBItem.rpt" and seen["job"]["selection"] == "" and seen["job"]["action"] == "preview"
    assert rpt_zeroitems.render(printer="HP") is None
    assert seen["direct"]["printer"] == "HP" and seen["direct"]["action"] == "direct"


def test_print_settings_entry_for_the_zero_stock_report_gives_printer_and_copies():
    from app import rpt_zeroitems, store
    store.UTL_OPTIONS.append({"DocID": 999, "OptionOrder": 1, "OptionName": "Zero Stock", "CrptFileName": "RptZBItem.rpt",
                              "DirectPrinterName": "HP Office", "NoOfCopies": 2, "InActive": "N"})
    try:
        assert rpt_zeroitems.option() == {"name": "Zero Stock", "printer": "HP Office", "copies": 2}
        store.UTL_OPTIONS[-1]["InActive"] = "Y"
        assert rpt_zeroitems.option() is None
    finally:
        store.UTL_OPTIONS.pop()
