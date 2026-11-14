"""Pre-flight check for a client PC: run it after installing (or any time printing or reports feel slow). It only READS: nothing is written to the
shop's database and nothing is printed to paper.

    cd backend
    .venv\\Scripts\\python.exe tools\\preflight.py            (add --no-print to skip the test prints)

It checks, in order: the settings in .env, the database (connection, the tables the app needs, the indexes that keep it fast), the Crystal Reports
bridge (runtime, files, printer, answering), the report folder (every .rpt the Doc Designer's print options name), and then really lays out the latest
sales invoice and purchase bill, timing them (a healthy PC: about 0.15-0.4 s each). Every line is OK / WARN / FAIL with what to do. Exit code 1 if
anything FAILs. Credentials are never printed.
"""
import os
import sys
import time
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))
os.chdir(ROOT)

results: list[tuple[str, str, str]] = []


def say(level: str, what: str, hint: str = "") -> None:
    results.append((level, what, hint))
    mark = {"OK": "  OK  ", "WARN": " WARN ", "FAIL": " FAIL ", "INFO": " info "}[level]
    print(f"[{mark}] {what}" + (f"\n         -> {hint}" if hint else ""), flush=True)


def timed(fn):
    t = time.perf_counter()
    out = fn()
    return out, time.perf_counter() - t


def main() -> int:
    no_print = "--no-print" in sys.argv
    print(f"vLQMS pre-flight on {os.environ.get('COMPUTERNAME', 'this PC')} (read only)\n")

    # ---- 1. settings
    env = ROOT / ".env"
    say("OK" if env.exists() else "FAIL", ".env found" if env.exists() else ".env missing", "" if env.exists() else "run the installer, or copy .env.example to .env")
    try:
        from app import db, designer, rpt_bridge, rpt_print, sqlstore  # noqa: F401
    except Exception as e:   # noqa: BLE001
        say("FAIL", f"the app does not import ({type(e).__name__}: {e})", "reinstall: .venv\\Scripts\\pip install -r requirements.txt")
        return 1

    # ---- 2. database
    try:
        db.init_db()
        sqlstore.enabled = lambda: True   # the check reads the database directly; the app's own start-up is not needed
        row, secs = timed(lambda: db.fetchall("SELECT @@VERSION v, DB_NAME() d"))
        say("OK", f"database '{row[0]['d']}' connected ({secs * 1000:.0f} ms)")
        if secs > 1:
            say("WARN", "the database answers slowly", "check the network to the server, or that SQL Server is not short of memory")
    except Exception as e:   # noqa: BLE001
        say("FAIL", f"cannot connect to the database ({type(e).__name__})", "Database setup on the sign-in page, or check SQL Server, the firewall and the login")
        return 1
    for t in ("TrnHdrSINV", "TrnDtlSINV", "TrnHdrPB", "TrnDtlPB", "UtlStockList", "MstItem", "MstComp", "UtlOption", "UtlTransactions", "RptStk", "RptUDR"):
        found = db.fetchall("SELECT OBJECT_ID(?) o", ("dbo." + t,))[0]["o"]
        if not found:
            say("FAIL" if t.startswith(("Trn", "Utl", "Mst")) else "WARN", f"table {t} is missing", "this is not a vLQMS database, or it is an older version")
    # indexes: the big header tables are keyed on the document number only; a date index keeps the footer, DSR and reports fast
    for table, col in (("TrnHdrSINV", "SINVDate"), ("TrnHdrPB", "PBDate")):
        n = db.fetchall(f"SELECT COUNT(*) n FROM dbo.[{table}] WITH (NOLOCK)")[0]["n"]
        have = db.fetchall("SELECT COUNT(*) n FROM sys.index_columns ic JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id "
                           "WHERE ic.object_id = OBJECT_ID(?) AND c.name = ? AND ic.key_ordinal = 1", ("dbo." + table, col))[0]["n"]
        if have:
            say("OK", f"{table}: {n:,} rows, date index present")
        elif n > 50000:
            say("WARN", f"{table}: {n:,} rows and no index on {col} (footer totals, DSR and reports scan the whole table)",
                f"run in SQL Server (back up first): CREATE INDEX IX_{table}_{col} ON dbo.{table} ({col}) INCLUDE (Cancelled);")
        else:
            say("OK", f"{table}: {n:,} rows (small, no date index needed)")
    probe, secs = timed(lambda: db.fetchall("SELECT COUNT(*) n FROM dbo.TrnHdrSINV WITH (NOLOCK) WHERE SINVDate >= CAST(GETDATE() AS DATE)"))
    say("OK" if secs < 1 else "WARN", f"today's bill count: {probe[0]['n']} in {secs * 1000:.0f} ms", "" if secs < 1 else "this is the footer's query: see the index advice above")

    # ---- 3. Crystal Reports bridge
    bridge_dir = ROOT / "tools" / "rptbridge" / "bin"
    exe = bridge_dir / "RptBridge.exe"
    private = None   # the check runs its OWN bridge (own port and key) so it never disturbs the one the running app uses
    if exe.exists() and not os.environ.get("RPT_BRIDGE_URL") and sys.platform == "win32":
        import secrets
        import subprocess
        os.environ["RPT_BRIDGE_URL"], os.environ["RPT_BRIDGE_KEY"] = "http://127.0.0.1:8798", secrets.token_hex(16)
        private = subprocess.Popen([str(exe), "8798"], cwd=str(bridge_dir), env=dict(os.environ), stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        for _ in range(40):
            time.sleep(0.5)
            try:
                if rpt_bridge.status().get("ok"):
                    break
            except Exception:   # noqa: BLE001
                pass
    say("OK" if exe.exists() else "FAIL", "RptBridge.exe present" if exe.exists() else "RptBridge.exe is missing", "" if exe.exists() else "copy backend\\tools\\rptbridge\\bin from the build PC")
    crystal = [Path(os.environ.get("WINDIR", r"C:\Windows")) / p / "GAC_MSIL" / "CrystalDecisions.CrystalReports.Engine" for p in ("Microsoft.NET\\assembly", "assembly")]
    if any(p.exists() for p in crystal) or list(bridge_dir.glob("CrystalDecisions.CrystalReports.Engine.dll")):
        say("OK", "Crystal Reports runtime found")
    else:
        say("FAIL", "Crystal Reports runtime (CrystalDecisions) not found", "install the SAP Crystal Reports runtime for .NET Framework 4, 32-bit (x86), the one the desktop vLQMS uses")
    folder = rpt_bridge.folder()
    files = rpt_bridge.rpt_files()
    say("OK" if files else "FAIL", f"report folder {folder} has {len(files)} .rpt files" if files else f"report folder {folder} has no .rpt files",
        "" if files else "set RPT_FOLDER in .env to the desktop's Rpts folder (the desktop's Reports -> RptFilePath)")
    lp = rpt_bridge.layout_printer()
    say("INFO", f"layout printer: {lp or 'Windows default'}", "Microsoft Print to PDF keeps previews fast; if it is not installed the Windows default printer is used (a network printer can add seconds)")
    st = rpt_bridge.status()
    if st.get("ok"):
        say("OK", f"Crystal bridge answering (version {st.get('version', '?')}, {len(st.get('printers') or [])} printers)")
        if lp and st.get("printers") and lp not in st["printers"]:
            say("WARN", f"layout printer '{lp}' is not installed on this PC", "enable 'Microsoft Print to PDF' in Windows Features, or set RPT_LAYOUT_PRINTER=<an installed local printer>")
    else:
        say("FAIL", f"Crystal bridge not answering: {st.get('message', '')}", "start the backend (it starts the bridge), or run tools\\rptbridge\\run_bridge.cmd; check RPT_BRIDGE_KEY matches")

    # ---- 4. every .rpt the print options name
    try:
        designer.load()
        need = {}
        for d in designer.store.UTL_DOCS.values() if hasattr(designer, "store") else []:
            for o in designer.options_for(int(d["DocID"])):
                f = str(o.get("CrptFileName") or "").strip()
                if f:
                    need.setdefault(f.lower(), f)
        on_disk = {f.lower() for f in files}
        missing = sorted(v for k, v in need.items() if k not in on_disk)
        if need:
            say("OK" if not missing else "WARN", f"{len(need) - len(missing)}/{len(need)} .rpt files named by the print options exist" + (f"; missing: {', '.join(missing[:8])}" if missing else ""),
                "" if not missing else "copy those files into the report folder (the 7 that embed the SQL logon stay out of git)")
    except Exception as e:   # noqa: BLE001
        say("WARN", f"could not read the print options ({type(e).__name__}: {e})")

    # ---- 5. real layouts
    if not no_print and st.get("ok") and private is not None:
        from app import bridge_launcher, store
        try:
            designer.load()
        except Exception:   # noqa: BLE001
            pass
        _, secs = timed(lambda: bridge_launcher._warm_up(8798))   # what the app does at start: start Crystal's engine and keep this shop's reports open
        say("INFO", f"Crystal start-up warm-up took {secs:.1f} s (the app does this in the background right after it starts; the first print is slow until it is done)")
    if not no_print and st.get("ok"):
        for kind, table, col in (("sales_invoice", "TrnHdrSINV", "SINV"), ("purchase_bill", "TrnHdrPB", "PB")):
            try:
                row = db.fetchall(f"SELECT TOP 1 [{col}NO] n FROM dbo.[{table}] WITH (NOLOCK) WHERE [{col}Date] >= DATEADD(day, -90, GETDATE()) ORDER BY [{col}Date] DESC")
                if not row:
                    row = db.fetchall(f"SELECT TOP 1 [{col}NO] n FROM dbo.[{table}] WITH (NOLOCK) ORDER BY [{col}NO] DESC")
                if not row:
                    say("INFO", f"{kind}: no document to test with")
                    continue
                no = str(row[0]["n"]).strip()
                times = []
                for _ in range(2):
                    job = rpt_print.build(kind, no, "", "", "")
                    pdf, secs = timed(lambda job=job: rpt_bridge.render_pdf(job))
                    times.append(secs)
                ok_pdf = pdf[:4] == b"%PDF"
                level = "OK" if ok_pdf and times[-1] < 1 else "WARN" if ok_pdf else "FAIL"
                say(level, f"{kind} {no}: {job['rpt']} laid out in {times[0]:.2f} s first, {times[-1]:.2f} s again ({len(pdf) // 1024} KB PDF)",
                    "" if level == "OK" else "slow: check the database speed above, that the layout printer exists, and that no other program is using the CPU")
            except Exception as e:   # noqa: BLE001
                say("FAIL", f"{kind}: {getattr(e, 'detail', e)}", "see the Crystal bridge lines above")

    if private is not None:
        private.terminate()
    bad = [r for r in results if r[0] == "FAIL"]
    warn = [r for r in results if r[0] == "WARN"]
    print(f"\n{len(bad)} failed, {len(warn)} warnings." + ("  Ready to use." if not bad and not warn else "  Fix the FAIL lines first." if bad else "  Works, but see the warnings."))
    return 1 if bad else 0


if __name__ == "__main__":
    code = main()
    sys.stdout.flush()
    os._exit(code)
