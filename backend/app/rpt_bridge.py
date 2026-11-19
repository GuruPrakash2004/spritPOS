"""Client of the print service: the way the web app prints the desktop's .rpt formats.

The backend never opens a .rpt. It builds a job (which file, selection formula, parameters, printer; rpt_print.py and
friends) and sends it over HTTP to a print service: POST /render returns a PDF, POST /print sends it to a printer.
Two interchangeable services speak that contract:
  - the real one, tools/rptbridge (Windows, the Crystal Reports runtime): lays out the .rpt file itself. When
    tools/rptbridge/bin/RptBridge.exe exists on this Windows PC the backend starts and supervises it by itself
    (app/bridge_launcher.py, on 127.0.0.1:8765, with a key made up at start or RPT_BRIDGE_KEY); nothing has to be set.
    RPT_BRIDGE_URL / RPT_BRIDGE_KEY are for a bridge you run yourself, elsewhere (RPT_BRIDGE_AUTOSTART=0 stops the
    backend starting one).
  - the built-in one, app/mock_bridge.py: used when there is no bridge at all (no exe, no RPT_BRIDGE_URL). It reads the
    .rpt file's name only, picks an HTML template and renders it to PDF; the layout approximates the desktop's.
Settings:
    RPT_FOLDER            where the .rpt files are (default backend/Rpts; the desktop's Reports -> RptFilePath)
    RPT_BRIDGE_URL        a real bridge you run yourself, e.g. http://127.0.0.1:8765 (empty = the one the backend starts, else built-in)
    RPT_BRIDGE_KEY        the key the bridge uses (empty = made up at each start)
    RPT_BRIDGE_AUTOSTART  0 = the backend does not start the bridge
    RPT_TABLE_PREFIX      prefix of the table names the reports read (default none: the desktop's own tables,
                          which hold the bills made on the web too)
    RPT_BRIDGE_ALLOW_REMOTE=1   allow a real bridge that is not on this PC (the database logon is sent to it)
    RPT_LAYOUT_PRINTER    the local printer previews/PDFs are laid out on (default "Microsoft Print to PDF" when installed; "off" =
                          the Windows default printer). A network default printer made the first preview after ~60 s idle take ~8.5 s.
"""
import logging
import os
import re
import secrets
import sys
import time
from pathlib import Path
from urllib.parse import urlparse

import httpx
from fastapi import HTTPException

from . import db, mock_bridge, store

logger = logging.getLogger(__name__)
SLOW_SECONDS = 5.0   # a print slower than this is logged as a warning with the usual causes
DEFAULT_FOLDER = Path(__file__).resolve().parents[1] / "Rpts"
LOOPBACK = {"127.0.0.1", "localhost", "::1"}
THERMAL_PREVIEW_PT = 216   # 3 inches in points: the width of the thermal paper roll
TIMEOUT = 60.0  # Crystal opens the report, runs its queries and lays out the pages


def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


EXE = Path(__file__).resolve().parents[1] / "tools" / "rptbridge" / "bin" / "RptBridge.exe"
DEFAULT_URL = "http://127.0.0.1:8765"
def _auto_key() -> str:
    """The key the launcher gives the bridge it starts. Kept in backend/data/bridge.key so a bridge that is still running when the backend
    restarts (a --reload, a service restart) is recognised and reused instead of being replaced: a new bridge pays Windows' slow first
    printer call (up to a minute) again. In memory only under VLQMS_KEYS_IN_MEMORY (the tests) or when the file can't be written."""
    fresh = secrets.token_urlsafe(32)
    if os.environ.get("VLQMS_KEYS_IN_MEMORY"):
        return fresh
    path = Path(__file__).resolve().parents[1] / "data" / "bridge.key"
    try:
        if path.exists():
            saved = path.read_text(encoding="ascii").strip()
            if len(saved) >= 20:
                return saved
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(fresh, encoding="ascii")
    except OSError:
        pass
    return fresh


_AUTO_KEY = _auto_key()


def local_bridge() -> bool:
    """Whether this PC can run its own bridge (Windows, with the built exe): the backend then starts it by itself
    (bridge_launcher.py) and nothing needs configuring."""
    return sys.platform == "win32" and EXE.exists()


def url() -> str:
    return (_env("RPT_BRIDGE_URL") or (DEFAULT_URL if local_bridge() and _env("RPT_BUILTIN_PRINT") != "1" else "")).rstrip("/")


def key() -> str:
    return _env("RPT_BRIDGE_KEY") or _AUTO_KEY


def using_mock() -> bool:
    """The built-in print (mock_bridge.py, our own approximate layout) is used ONLY when RPT_BUILTIN_PRINT=1 (the test suite).
    A shop always prints through the original .rpt files run by the Crystal bridge, never a redrawn copy."""
    return _env("RPT_BUILTIN_PRINT") == "1" and not _env("RPT_BRIDGE_URL")


def _no_bridge() -> HTTPException:
    return HTTPException(503, "The Crystal Reports bridge is not available on this server, so nothing can be printed. "
                              "Build backend/tools/rptbridge (see its README) or set RPT_BRIDGE_URL / RPT_BRIDGE_KEY.")


LAYOUT_PRINTER = "Microsoft Print to PDF"


def layout_printer() -> str:
    """The printer Crystal lays a PREVIEW / PDF out on. Crystal measures text against a printer driver and re-contacts the Windows
    default printer about every 60 s; when that is a network (WSD) printer the next preview then takes ~8.5 s instead of ~0.3 s.
    A local driver has no such delay. The bridge uses it only if this PC has it installed. RPT_LAYOUT_PRINTER=<name> picks another
    local printer, RPT_LAYOUT_PRINTER=off keeps the Windows default. Direct prints are not affected."""
    v = _env("RPT_LAYOUT_PRINTER", LAYOUT_PRINTER)
    return "" if v.lower() in ("off", "none", "default") else v


def configured() -> bool:
    """Printing is always possible: the real bridge when set, else the built-in one."""
    return True


def folder() -> Path:
    return Path(_env("RPT_FOLDER") or DEFAULT_FOLDER)


def table_prefix() -> str:
    return os.environ.get("RPT_TABLE_PREFIX", "").strip()


def rpt_files() -> list[str]:
    """The .rpt files in the report folder (the desktop lists them the same way, top level only)."""
    try:
        return sorted(p.name for p in folder().glob("*.rpt") if p.is_file())
    except OSError:
        return []


def safe_rpt_name(name: str) -> str:
    """A bare .rpt file name that exists in the report folder; adds '.rpt' as the desktop does."""
    n = (name or "").strip()
    if n and not n.lower().endswith(".rpt"):
        n += ".rpt"
    if not n or re.search(r"[\\/:*?\"<>|]", n) or n.startswith("."):
        raise HTTPException(422, f"'{name}' is not a report file name")
    # Case-insensitive match, because the Windows folder the formats came from is.
    for f in rpt_files():
        if f.lower() == n.lower():
            return f
    raise HTTPException(404, f"Please ensure {n} exists in the report folder ({folder()}).")


def _logon() -> dict:
    s = db.settings()
    return {"server": s["host"] + (f",{s['port']}" if s.get("port") else ""), "database": s["database"],
            "user": s["user"], "password": db._password()}


def _check_target() -> str:
    base = url()
    host = urlparse(base).hostname or ""
    if host not in LOOPBACK and _env("RPT_BRIDGE_ALLOW_REMOTE") != "1":
        raise HTTPException(503, "The bridge must run on this PC (the database logon is sent to it). "
                                 "Set RPT_BRIDGE_ALLOW_REMOTE=1 to allow another PC.")
    return base


def friendly_error(detail: str, job: dict | None) -> str:
    """Crystal's and Windows' own wording is cryptic when a printer is the problem; say what is wrong and what to do."""
    printer = str((job or {}).get("printer") or "").strip()
    low = detail.lower()
    where = f"The printer {printer}" if printer else "The Windows default printer"
    if "rpc server is unavailable" in low:
        return (f"{where} can't be reached: it is switched off, or the PC that shares it is offline or off the network. "
                "Switch it on, or choose a printer that is online (Global Settings → Print Settings → Printer), "
                "or set an online printer as the Windows default." if printer else
                f"{where} can't be reached (its PC is offline or off the network), and Crystal Reports needs a printer to lay out "
                "the page. Switch it on, or set a printer that is online as the Windows default printer.")
    if "is not installed on this pc" in low:
        have = (job or {}).get("installed_printers") or []
        seen = (" Printers this service can use: " + ", ".join(have) + ".") if have else " This service can open none of the printers Windows lists (see Global Settings → Print Settings → Printer on this server)."
        return (f"{detail.split('Crystal Reports:')[-1].strip()}{seen} Pick one of them in Global Settings → Print Settings → "
                "Printer on this server (it applies to every document), or add the printer for the Windows user the service runs as.")
    return f"Crystal Reports: {detail[:400]}"


def _call_mock(path: str, job: dict | None) -> httpx.Response:
    """The built-in service behind the same /render, /print and /health calls (as an httpx.Response, so callers don't differ)."""
    if path == "/health":
        return httpx.Response(200, json={"version": "built-in", "printers": []})
    job = {**job, "rpt": safe_rpt_name(job["rpt"])}
    if path == "/print":
        mock_bridge.print_direct(job)
        return httpx.Response(200, json={"printed": True})
    return httpx.Response(200, content=mock_bridge.render(job))


_CLIENT: httpx.Client | None = None


def _client() -> httpx.Client:
    """One client for every call to the bridge. httpx.request()/httpx.get() build a new client each time, and building it loads
    the SSL certificate store: ~290 ms on this PC for every print, and the bridge is plain http on this PC, so no TLS is
    needed. Idle connections expire before the bridge's own 120 s idle limit, so a kept connection is never a dead one."""
    global _CLIENT
    if _CLIENT is None:
        _CLIENT = httpx.Client(verify=False, trust_env=False, limits=httpx.Limits(max_keepalive_connections=4, keepalive_expiry=30))
    return _CLIENT


# Crystal's engine inside the bridge can run out of its own memory after many reports ("Memory full. Failed to export the report. Not enough
# memory for operation."). A new bridge process starts clean, so such an error restarts the bridge (when this backend started it) and the
# print is tried once more; and the bridge is recycled after RECYCLE_AFTER reports so it never gets that far.
MEMORY_ERRORS = ("memory full", "not enough memory", "out of memory", "insufficient memory")
RECYCLE_AFTER = 300
_reports_since_start = 0


def _is_memory_error(detail: str) -> bool:
    low = (detail or "").lower()
    return any(m in low for m in MEMORY_ERRORS)


def _restart_bridge(reason: str) -> bool:
    global _reports_since_start
    from . import bridge_launcher   # imported here: bridge_launcher imports this module
    ok = bridge_launcher.restart(reason)
    if ok:
        _reports_since_start = 0
    return ok


def _call(path: str, job: dict | None, method: str = "POST") -> httpx.Response:
    global _reports_since_start
    if using_mock():
        return _call_mock(path, job)
    if not url():
        raise _no_bridge()
    if job is not None and path in ("/render", "/print"):
        from . import bridge_launcher   # imported here: bridge_launcher imports this module
        bridge_launcher.wait_ready(200)   # a bridge that has just started is still getting past Windows' slow first printer call; returns at once when ready or not ours
        if _reports_since_start >= RECYCLE_AFTER:
            _restart_bridge(f"recycled after {_reports_since_start} reports")
        _reports_since_start += 1
    try:
        return _call_once(path, job, method)
    except HTTPException as e:
        if e.status_code == 502 and _is_memory_error(str(e.detail)) and _restart_bridge("Crystal ran out of memory"):
            return _call_once(path, job, method)   # one more try on the fresh bridge
        raise


def _call_once(path: str, job: dict | None, method: str = "POST") -> httpx.Response:
    base = _check_target()
    body = None
    if job is not None:
        body = {**job, "parameters": {**company_parameters(), **(job.get("parameters") or {})},
                "rpt": safe_rpt_name(job["rpt"]), "folder": str(folder()), "table_prefix": table_prefix(),
                "logon": _logon(), "label_values": label_values(), "address_lines": address_lines(),
                # previews/exports are laid out on a local driver (see layout_printer()); a direct print uses its own printer
                "layout_printer": layout_printer() if path == "/render" else ""}
    started = time.perf_counter()
    try:
        try:
            r = _client().request(method, base + path, json=body, headers={"X-Bridge-Key": key()}, timeout=TIMEOUT)
        except httpx.ConnectError:
            # The bridge is down for a moment (it restarts itself after a hung report): wait for the one this backend runs, then go again.
            from . import bridge_launcher   # imported here: bridge_launcher imports this module
            if not bridge_launcher.wait_ready(60):
                raise
            r = _client().request(method, base + path, json=body, headers={"X-Bridge-Key": key()}, timeout=TIMEOUT)
    except httpx.HTTPError as e:
        logger.warning("Crystal bridge %s %s failed after %.1fs: %s", path, (job or {}).get("rpt", ""), time.perf_counter() - started, e.__class__.__name__)
        raise HTTPException(503, f"The Crystal Reports bridge can't be reached ({e.__class__.__name__}). Is it running?")
    took = time.perf_counter() - started
    # The whole Crystal run is timed here: it opens the .rpt, logs on to SQL Server for every table, lays the pages out against the
    # printer and writes the PDF. Slow usually means the database logon (SQL Server on another PC), a printer that is off or
    # unreachable (Crystal needs one to lay the page out), or a heavy format.
    (logger.warning if took > SLOW_SECONDS else logger.info)(
        "Crystal bridge %s %s took %.1fs (HTTP %s) [%s]%s", path, (job or {}).get("rpt", ""), took, r.status_code, r.headers.get("X-Bridge-Timing", "no timing"),
        " - slow: check the database server's speed and that the Windows default / chosen printer is online" if took > SLOW_SECONDS else "")
    if r.status_code >= 400:
        try:
            detail = r.json().get("error") or r.text
        except ValueError:
            detail = r.text
        raise HTTPException(502, friendly_error(detail, job))
    return r


# Some formats (fmtSInv.rpt) print "PH NO:", "Email ID:", "GSTIN No:" as fixed text with no field behind them, so Crystal
# prints them empty. Every job carries the shop's values under the labels formats use; the bridge (FillLabels in
# tools/rptbridge/Program.cs) adds the value to a text object that holds only such a label, for any .rpt and for a
# preview, export or direct print alike. Label keys are lower case, without the colon.
LABEL_FIELDS = {
    "ph no": "phone", "phone": "phone", "phone no": "phone", "tel no": "phone", "tel": "phone", "telephone": "phone",
    "email id": "email", "email": "email", "e-mail": "email", "e-mail id": "email", "mail id": "email",
    "address": "address", "add": "address", "addr": "address",
    "gstin no": "gstin", "gstin": "gstin", "gst no": "gstin", "gst in": "gstin", "gstin number": "gstin",
}


_COMP = {"at": 0.0, "row": None}
_COMP_TTL = 30.0   # the shop's own row changes rarely; every print used to read it once per parameter (8-20 round trips)


def comp_row() -> dict:
    """MstComp's first row, read once and kept for 30 seconds (empty when the database can't be read)."""
    now = time.monotonic()
    if _COMP["row"] is not None and now - _COMP["at"] < _COMP_TTL:
        return _COMP["row"]
    try:
        row = dict(db.fetchall(f"SELECT TOP 1 * FROM dbo.[{table_prefix()}MstComp]")[0])
    except Exception:   # noqa: BLE001 - offline, or no such table: callers fall back to Company Master
        row = {}
    _COMP.update(at=now, row=row)
    return row


def address_lines() -> list[str]:
    """The shop's address lines (MstComp Address1..4, as the desktop's print takes them; else Company Master's address) for
    a format's blank address placeholder (FillLabels in the bridge)."""
    row = comp_row()
    lines = [str(row.get(k) or "").strip() for k in ("Address1", "Address2", "Address3", "Address4")]
    lines = [x for x in lines if x]
    return lines or ([str(store.COMPANY.get("address") or "").strip()] if str(store.COMPANY.get("address") or "").strip() else [])


# Parameter names formats use for the shop's details (compared without case). A job's own parameters win; the bridge skips
# a name the report doesn't define, so every report gets the ones it asks for (fmtPB: CompName, Phone, Add1..Add3;
# RptDSR: CompName, Add1..Add4, phno, gstin; ...) even when its print option maps none.
def company_parameters() -> dict[str, str]:
    c = store.COMPANY
    lines = address_lines()
    out = {n: str(c.get(k) or "").strip() for names, k in (
        (("compname", "companyname", "company", "shopname", "name"), "name"),
        (("phone", "phno", "telno", "phoneno", "telephone", "tel", "mobile", "mobno"), "phone"),
        (("email", "emailid", "mailid", "email_id"), "email"),
        (("gstin", "gstno", "gstinno", "gst"), "gstin")) for n in names}
    for i in range(1, 5):
        line = lines[i - 1] if len(lines) >= i else ""
        out[f"add{i}"] = out[f"address{i}"] = line
    out["address"] = ", ".join(lines)
    return out


def label_values() -> dict[str, str]:
    c = store.COMPANY
    return {label: str(c.get(key) or "").strip() for label, key in LABEL_FIELDS.items() if str(c.get(key) or "").strip()}


def render_pdf(job: dict) -> bytes:
    """The report as a PDF, laid out by Crystal exactly as the desktop's preview/export would."""
    return _call("/render", job).content


def preview_pdf(pdf: bytes) -> tuple[bytes, str]:
    """The PDF trimmed to what the report draws, for the on-screen preview, and its page size in points ("w,h").
    The reports are saved on a Letter page (a 3-inch receipt sits in a narrow column of it), so the preview shows
    the drawn area as the sheet; the paper layout is untouched (printing uses the bridge's own PDF). One page is
    trimmed all round; several pages keep their height and share one width. Without PyMuPDF the PDF is returned as is."""
    try:
        import pymupdf
    except ImportError:
        return pdf, ""
    try:
        doc = pymupdf.open(stream=pdf, filetype="pdf")
        boxes = []
        for page in doc:
            box = None
            # Only what is really drawn counts: words with text (not empty or blank text boxes), lines/fills that show
            # (not invisible white ones) and pictures. An invisible page-wide object would otherwise keep the page wide.
            shown = [d["rect"] for d in page.get_drawings()
                     if d.get("color") is not None or (d.get("fill") is not None and tuple(d["fill"]) != (1.0, 1.0, 1.0))]
            for r in [pymupdf.Rect(w[:4]) for w in page.get_text("words") if str(w[4]).strip()] + shown \
                    + [pymupdf.Rect(i["bbox"]) for i in page.get_image_info()]:
                if r.is_empty or r.is_infinite:
                    continue
                box = pymupdf.Rect(r) if box is None else box | r
            boxes.append(box)
        found = [b for b in boxes if b is not None]
        if not found:
            return pdf, ""
        pad = 10
        x0 = max(0, min(b.x0 for b in found) - pad)
        x1 = max(b.x1 for b in found) + pad
        if x1 - x0 <= THERMAL_PREVIEW_PT + 2 * pad:   # a receipt report: show it on the 3-inch (80 mm) roll, not a wide page
            x0 = max(0, min(b.x0 for b in found) - 4)
            x1 = x0 + THERMAL_PREVIEW_PT
        if x1 - x0 <= THERMAL_PREVIEW_PT + 2 * pad and len(doc) > 1:
            # A long receipt (many items) is laid out by Crystal on several Letter pages, but a 3-inch roll is one
            # continuous strip: stack the drawn part of every page into a single tall page so nothing is split.
            width = min(x1, doc[0].rect.x1) - x0
            slices = [(page, pymupdf.Rect(x0, max(0, b.y0 - 2), min(x1, page.rect.x1), min(page.rect.y1, b.y1 + 2)))
                      for page, b in zip(doc, boxes) if b is not None]
            # Crystal's Page Header (Bill No, Supplier, column heads) repeats on every page; on the strip it belongs only once.
            # On pages after the first, start just under the column-head rule (the line below the "Slno" row).
            if slices and any(str(w[4]).strip().lower() == "slno" for w in slices[0][0].get_text("words")):
                trimmed = [slices[0]]
                for page, clip in slices[1:]:
                    heads = [pymupdf.Rect(w[:4]) for w in page.get_text("words") if str(w[4]).strip().lower() == "slno"]
                    if heads:
                        bottom = heads[0].y1
                        rules = [d["rect"].y1 for d in page.get_drawings()
                                 if d["rect"].height < 3 and d["rect"].width > 50 and bottom - 2 <= d["rect"].y0 <= bottom + 12]
                        clip = pymupdf.Rect(clip.x0, max([bottom] + rules) + 1, clip.x1, clip.y1)
                    if not clip.is_empty:
                        trimmed.append((page, clip))
                slices = trimmed
            if slices:
                out = pymupdf.open()
                sheet = out.new_page(width=width, height=sum(c.height for _, c in slices) + 2 * pad)
                top = float(pad)
                for page, clip in slices:
                    sheet.show_pdf_page(pymupdf.Rect(0, top, width, top + clip.height), doc, page.number, clip=clip)
                    top += clip.height
                return out.tobytes(), f"{width:.0f},{sheet.rect.height:.0f}"
        for page, box in zip(doc, boxes):
            full = page.rect
            y0, y1 = (max(0, box.y0 - pad), box.y1 + pad) if len(doc) == 1 and box is not None else (full.y0, full.y1)
            page.set_cropbox(pymupdf.Rect(x0, y0, min(x1, full.x1), min(y1, full.y1)))
        first = doc[0].cropbox
        return doc.tobytes(), f"{first.width:.0f},{first.height:.0f}"
    except Exception:   # noqa: BLE001 - a preview that can't be trimmed is still a preview
        return pdf, ""


_REDIRECTED = re.compile(r"\s+on\s+\S+(\s+\(redirected\s+\d+\))?\s*$|\s*\(redirected\s+\d+\)\s*$", re.I)


def _plain(name: str) -> str:
    """A printer name without the parts that differ between PCs: \\\\host\\ in front, " on host (redirected 4)" behind."""
    name = name.strip()
    if name.startswith("\\\\") and "\\" in name[2:]:
        name = name[2:].split("\\", 1)[1]
    return _REDIRECTED.sub("", name).strip().lower()


def resolve_printer(wanted: str, installed: list[str]) -> str:
    """The printer to use on THIS PC. The option's saved name (UtlOption.DirectPrinterName, shared with the desktop) is often a
    name that only exists in another session, e.g. "POS-80 on saturn2 (redirected 4)" saved from a Remote Desktop session.
    In order: the web's own DIRECT_PRINTER setting (Print Settings → Printer on this server), the saved name when this PC
    has it, an installed printer with the same name once " on host (redirected N)" / \\\\host\\ are ignored, else the saved
    name as it is (the bridge then says it is not installed and the message lists what this PC has)."""
    own = _env("DIRECT_PRINTER")
    lower = {p.lower(): p for p in installed}
    if own and (not installed or own.lower() in lower):
        return lower.get(own.lower(), own)
    if wanted.lower() in lower:
        return lower[wanted.lower()]
    plain = _plain(wanted)
    if plain:
        for p in installed:
            if _plain(p) == plain:
                return p
    return wanted


def can_open(name: str) -> tuple[bool, str]:
    """Whether THIS process (the one that runs the bridge: the service's Windows user and session) can open the printer, which is
    what printing needs. A name Windows lists is not always usable: a Remote Desktop printer or another user's printer is listed
    but refused. Returns (ok, the Windows reason)."""
    if sys.platform != "win32":
        return True, ""
    try:
        import ctypes
        from ctypes import wintypes
        handle = wintypes.HANDLE()
        winspool = ctypes.WinDLL("winspool.drv", use_last_error=True)
        if winspool.OpenPrinterW(name, ctypes.byref(handle), None):
            winspool.ClosePrinter(handle)
            return True, ""
        code = ctypes.get_last_error()
        return False, f"Windows error {code}: {ctypes.FormatError(code).strip()}"
    except Exception as exc:   # noqa: BLE001 - a diagnostic must never break printing
        return True, f"not checked ({exc})"


_PRINTERS = {"at": 0.0, "report": None}
_PRINTERS_TTL = 120.0   # the PC's printers rarely change; asking Windows to open every one of them is what takes the time


def printer_report(fresh: bool = False) -> dict:
    """The printers the bridge lists, split into those this service can really open and those Windows refuses (with why).
    Opening a printer asks Windows to reach it, and a network or Remote Desktop printer that is off can take many seconds, so the
    checks run side by side (the slowest one, not the sum, is the wait) and the answer is kept for two minutes; a print then
    no longer repeats it. A good answer is kept only when the list was read (`fresh` forces a new look)."""
    now = time.monotonic()
    if not fresh and _PRINTERS["report"] is not None and now - _PRINTERS["at"] < _PRINTERS_TTL:
        return _PRINTERS["report"]
    try:
        listed = list(_call("/health", None, "GET").json().get("printers") or [])
    except Exception:   # noqa: BLE001 - only a hint
        return {"usable": [], "refused": []}
    from concurrent.futures import ThreadPoolExecutor
    with ThreadPoolExecutor(max_workers=min(8, max(1, len(listed)))) as pool:
        checked = list(pool.map(can_open, listed))
    usable, refused = [], []
    for name, (ok, why) in zip(listed, checked):
        (usable if ok else refused).append(name if ok else {"name": name, "why": why})
    report = {"usable": usable, "refused": refused}
    _PRINTERS.update(at=time.monotonic(), report=report)
    logger.info("Printers checked in %.1fs: %d usable, %d refused", time.monotonic() - now, len(usable), len(refused))
    return report


def installed_printers() -> list[str]:
    """The printers this service can really print to (listed AND openable); the rest is in printer_report()."""
    return printer_report()["usable"]


def print_direct(job: dict) -> None:
    """Send the report to the printer from the PC the bridge runs on (see resolve_printer for which one)."""
    installed = [] if using_mock() else installed_printers()
    job = {**job, "printer": resolve_printer(str(job.get("printer") or "").strip(), installed), "installed_printers": installed}
    _call("/print", job)


def status() -> dict:
    """For the Print dialog and About: is the bridge set up and answering?"""
    if using_mock():
        return {"configured": True, "ok": True, "message": "Built-in print (approximate layout; set RPT_BRIDGE_URL for Crystal Reports)",
                "version": "built-in", "printers": []}
    if not url():
        return {"configured": False, "ok": False, "message": _no_bridge().detail}
    try:
        r = _call("/health", None, "GET")
        return {"configured": True, "ok": True, "message": "", **{k: v for k, v in r.json().items() if k in ("version", "printers")}}
    except HTTPException as e:
        return {"configured": True, "ok": False, "message": e.detail}
