"""The Crystal bridge warm-up (app/bridge_launcher.py): the engine is started first and without a database logon, then the shop's
reports are opened and kept, most needed first and no more than the bridge keeps; a bridge that is already running is warmed too."""
from app import bridge_launcher as bl
from app import rpt_bridge, store


def _folder(tmp_path, names):
    for n in names:
        (tmp_path / n).write_bytes(b"x" * (len(n) * 10))   # distinct sizes: the smallest is the engine's report
    return tmp_path


def _setup(monkeypatch, tmp_path, names, options):
    monkeypatch.setattr(rpt_bridge, "folder", lambda: _folder(tmp_path, names))
    monkeypatch.setattr(store, "UTL_OPTIONS", options)
    monkeypatch.setattr(rpt_bridge, "_logon", lambda: {"server": "s", "database": "d", "user": "u", "password": "p"})
    monkeypatch.setattr(rpt_bridge, "company_parameters", lambda: {})
    monkeypatch.setattr(rpt_bridge, "label_values", lambda: {})
    monkeypatch.setattr(rpt_bridge, "address_lines", lambda: [])


def test_reports_to_keep_are_in_priority_order_and_capped(monkeypatch, tmp_path):
    names = ["fmtPB.rpt", "FMTSINV.rpt", "RptDSR.rpt", "RptDSR_S.rpt", "RptStkBal.rpt", "RptStkBal_Biggsize.rpt", "fmtPSE.rpt"] \
        + [f"extra{i}.rpt" for i in range(20)]
    _setup(monkeypatch, tmp_path, names, [{"CrptFileName": "fmtpb"}] + [{"CrptFileName": f"extra{i}"} for i in range(20)])

    files = bl._warm_files()

    assert files[0] == "fmtPB.rpt"   # a Doc Designer option's file (matched without case, ".rpt" added) comes first
    assert len(files) == bl.KEEP_REPORTS


def test_designer_files_then_dsr_and_stock_balance_formats(monkeypatch, tmp_path):
    names = ["fmtPB.rpt", "FMTSINV.rpt", "RptDSR.rpt", "RptDSR_S.rpt", "RptStkBal.rpt", "fmtPSE.rpt", "unrelated.rpt"]
    _setup(monkeypatch, tmp_path, names, [{"CrptFileName": "fmtPB"}])

    assert bl._warm_files() == ["fmtPB.rpt", "RptDSR.rpt", "RptDSR_S.rpt", "RptStkBal.rpt", "FMTSINV.rpt", "fmtPSE.rpt"]


def test_engine_is_warmed_first_without_a_logon_then_the_reports_with_one(monkeypatch, tmp_path):
    _setup(monkeypatch, tmp_path, ["RptDSR.rpt", "RptStkBal.rpt", "FMTSINV.rpt"], [{"CrptFileName": "FMTSINV"}])
    monkeypatch.setattr(bl, "_key_accepted", lambda base: True)
    monkeypatch.setattr(bl, "_ready", lambda base: True)
    posts = []

    class Reply:
        def json(self):
            return {"kept": 3, "ms": 1}

    monkeypatch.setattr(bl.httpx, "post", lambda url, json=None, **kw: posts.append((url, json)) or Reply())

    bl._warm_up(8765)

    # the engine start-up, then ONE report per call (a single slow call would pass the bridge's 55 s watchdog and kill it)
    assert [u for u, _ in posts] == ["http://127.0.0.1:8765/warm"] * 4
    engine, reports = posts[0][1], [p[1] for p in posts[1:]]
    assert "logon" not in engine and len(engine["files"]) == 1   # the engine start-up needs no database
    assert all(r["logon"]["database"] == "d" and len(r["files"]) == 1 for r in reports)
    assert reports[0]["files"][0] == "FMTSINV.rpt"


def test_layout_printer_is_a_local_driver_unless_turned_off(monkeypatch):
    monkeypatch.delenv("RPT_LAYOUT_PRINTER", raising=False)
    assert rpt_bridge.layout_printer() == "Microsoft Print to PDF"
    monkeypatch.setenv("RPT_LAYOUT_PRINTER", "Some Local Printer")
    assert rpt_bridge.layout_printer() == "Some Local Printer"
    for off in ("off", "OFF", "none", "default"):
        monkeypatch.setenv("RPT_LAYOUT_PRINTER", off)
        assert rpt_bridge.layout_printer() == ""


def test_only_previews_are_laid_out_on_the_layout_printer(monkeypatch):
    """/render carries layout_printer; a direct /print does not (it prints on its own printer)."""
    sent = []

    class Reply:
        status_code = 200
        headers = {}
        content = b"%PDF"

    class Client:
        def request(self, method, url, json=None, **kw):
            sent.append((url.rsplit("/", 1)[1], json))
            return Reply()

    monkeypatch.setattr(rpt_bridge, "_client", lambda: Client())
    monkeypatch.setattr(rpt_bridge, "using_mock", lambda: False)
    monkeypatch.setattr(rpt_bridge, "url", lambda: "http://127.0.0.1:8765")
    monkeypatch.setattr(rpt_bridge, "safe_rpt_name", lambda n: n)
    monkeypatch.setattr(rpt_bridge, "_logon", lambda: {})
    monkeypatch.setattr(rpt_bridge, "company_parameters", lambda: {})
    monkeypatch.setattr(rpt_bridge, "label_values", lambda: {})
    monkeypatch.setattr(rpt_bridge, "address_lines", lambda: [])
    monkeypatch.delenv("RPT_LAYOUT_PRINTER", raising=False)

    rpt_bridge._call("/render", {"rpt": "RptDSR.rpt"})
    rpt_bridge._call("/print", {"rpt": "RptDSR.rpt", "printer": "POS-80"})

    assert sent[0][0] == "render" and sent[0][1]["layout_printer"] == "Microsoft Print to PDF"
    assert sent[1][0] == "print" and sent[1][1]["layout_printer"] == ""


def test_a_bridge_that_is_already_running_is_warmed_too(monkeypatch):
    started = []

    class FakeThread:
        def __init__(self, target=None, args=(), **kw):
            started.append((target, args))

        def start(self):
            pass

    monkeypatch.setattr(bl, "should_start", lambda: 8765)
    monkeypatch.setattr(bl, "_answering", lambda base: True)
    monkeypatch.setattr(bl, "_key_accepted", lambda base: True)
    monkeypatch.setattr(bl.threading, "Thread", FakeThread)

    bl.start()

    assert started == [(bl._warm_up, (8765,))]
