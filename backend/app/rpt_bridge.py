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
"""
import os
import re
import secrets
import sys
from pathlib import Path
from urllib.parse import urlparse

import httpx
from fastapi import HTTPException

from . import db, mock_bridge, store

DEFAULT_FOLDER = Path(__file__).resolve().parents[1] / "Rpts"
LOOPBACK = {"127.0.0.1", "localhost", "::1"}
THERMAL_PREVIEW_PT = 216   # 3 inches in points: the width of the thermal paper roll
TIMEOUT = 60.0  # Crystal opens the report, runs its queries and lays out the pages


def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


EXE = Path(__file__).resolve().parents[1] / "tools" / "rptbridge" / "bin" / "RptBridge.exe"
DEFAULT_URL = "http://127.0.0.1:8765"
_AUTO_KEY = secrets.token_urlsafe(32)   # made up at every start; the launcher gives it to the bridge it starts


def local_bridge() -> bool:
    """Whether this PC can run its own bridge (Windows, with the built exe): the backend then starts it by itself
    (bridge_launcher.py) and nothing needs configuring."""
    return sys.platform == "win32" and EXE.exists()


def url() -> str:
    return (_env("RPT_BRIDGE_URL") or (DEFAULT_URL if local_bridge() else "")).rstrip("/")


def key() -> str:
    return _env("RPT_BRIDGE_KEY") or _AUTO_KEY


def using_mock() -> bool:
    """No bridge at all (none set, none built on this PC): the built-in print (mock_bridge.py) lays the pages out."""
    return not url()


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


def _call(path: str, job: dict | None, method: str = "POST") -> httpx.Response:
    if using_mock():
        return _call_mock(path, job)
    base = _check_target()
    body = None
    if job is not None:
        body = {**job, "parameters": {**company_parameters(), **(job.get("parameters") or {})},
                "rpt": safe_rpt_name(job["rpt"]), "folder": str(folder()), "table_prefix": table_prefix(),
                "logon": _logon(), "label_values": label_values(), "address_lines": address_lines()}
    try:
        r = httpx.request(method, base + path, json=body, headers={"X-Bridge-Key": key()}, timeout=TIMEOUT)
    except httpx.HTTPError as e:
        raise HTTPException(503, f"The Crystal Reports bridge can't be reached ({e.__class__.__name__}). Is it running?")
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


def address_lines() -> list[str]:
    """The shop's address lines (MstComp Address1..4, as the desktop's print takes them; else Company Master's address) for
    a format's blank address placeholder (FillLabels in the bridge)."""
    try:
        row = db.fetchall("SELECT TOP 1 Address1, Address2, Address3, Address4 FROM dbo.MstComp")[0]
        lines = [str(row.get(k) or "").strip() for k in ("Address1", "Address2", "Address3", "Address4")]
    except Exception:   # noqa: BLE001 - offline, or a database with other columns: Company Master's address will do
        lines = []
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


def printer_report() -> dict:
    """The printers the bridge lists, split into those this service can really open and those Windows refuses (with why)."""
    try:
        listed = list(_call("/health", None, "GET").json().get("printers") or [])
    except Exception:   # noqa: BLE001 - only a hint
        return {"usable": [], "refused": []}
    usable, refused = [], []
    for name in listed:
        ok, why = can_open(name)
        (usable if ok else refused).append(name if ok else {"name": name, "why": why})
    return {"usable": usable, "refused": refused}


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
    try:
        r = _call("/health", None, "GET")
        return {"configured": True, "ok": True, "message": "", **{k: v for k, v in r.json().items() if k in ("version", "printers")}}
    except HTTPException as e:
        return {"configured": True, "ok": False, "message": e.detail}
