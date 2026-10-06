"""Client of the Crystal Reports bridge: the way the web app prints the desktop's .rpt files.

Why a bridge: a .rpt file is a Crystal Reports document. Only the Crystal Reports runtime can lay one out (the layout
is stored in a proprietary, partly encrypted format), so the web app does not draw its own copy of a format. The
desktop vLQMS loads the .rpt with the Crystal runtime, logs it on to SQL Server, sets the record selection formula and
the parameters, and previews, prints or exports it. The bridge (tools/rptbridge/, a small Windows program that uses
the same CrystalDecisions DLLs as the desktop exe) does exactly that for the web app and hands back a PDF.

Settings in backend/.env (never committed):
    RPT_BRIDGE_URL        e.g. http://127.0.0.1:8765  (the bridge runs on the shop PC next to this backend)
    RPT_BRIDGE_KEY        shared secret the bridge expects in X-Bridge-Key
    RPT_FOLDER            where the .rpt files are (default backend/Rpts; the desktop's Reports → RptFilePath)
    RPT_TABLE_PREFIX      prefix of the table names the reports read (default none: the desktop's own tables,
                          which hold the bills made on the web too)
    RPT_BRIDGE_ALLOW_REMOTE=1   allow a bridge that is not on this PC (the database logon is sent to it)
"""
import os
import re
from pathlib import Path
from urllib.parse import urlparse

import httpx
from fastapi import HTTPException

from . import db

DEFAULT_FOLDER = Path(__file__).resolve().parents[1] / "Rpts"
LOOPBACK = {"127.0.0.1", "localhost", "::1"}
TIMEOUT = 60.0   # Crystal opens the report, runs its queries and lays out the pages


def _env(key: str, default: str = "") -> str:
    return os.environ.get(key, default).strip()


def url() -> str:
    return _env("RPT_BRIDGE_URL").rstrip("/")


def configured() -> bool:
    return bool(url())


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
    if not base:
        raise HTTPException(503, "Printing needs the Crystal Reports bridge (RPT_BRIDGE_URL in backend/.env). "
                                 "See backend/tools/rptbridge/README.md.")
    host = urlparse(base).hostname or ""
    if host not in LOOPBACK and _env("RPT_BRIDGE_ALLOW_REMOTE") != "1":
        raise HTTPException(503, "RPT_BRIDGE_URL must point at this PC (the database logon is sent to the bridge). "
                                 "Set RPT_BRIDGE_ALLOW_REMOTE=1 to allow another PC.")
    return base


def _call(path: str, job: dict | None, method: str = "POST") -> httpx.Response:
    base = _check_target()
    body = None
    if job is not None:
        body = {**job, "rpt": safe_rpt_name(job["rpt"]), "folder": str(folder()), "table_prefix": table_prefix(),
                "logon": _logon()}
    try:
        r = httpx.request(method, base + path, json=body, headers={"X-Bridge-Key": _env("RPT_BRIDGE_KEY")}, timeout=TIMEOUT)
    except httpx.HTTPError as e:
        raise HTTPException(503, f"The Crystal Reports bridge can't be reached ({e.__class__.__name__}). Is it running?")
    if r.status_code >= 400:
        try:
            detail = r.json().get("error") or r.text
        except ValueError:
            detail = r.text
        raise HTTPException(502, f"Crystal Reports: {detail[:400]}")
    return r


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
            for r in [pymupdf.Rect(b[:4]) for b in page.get_text("blocks")] + [d["rect"] for d in page.get_drawings()] \
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
        for page, box in zip(doc, boxes):
            full = page.rect
            y0, y1 = (max(0, box.y0 - pad), box.y1 + pad) if len(doc) == 1 and box is not None else (full.y0, full.y1)
            page.set_cropbox(pymupdf.Rect(x0, y0, min(x1, full.x1), min(y1, full.y1)))
        first = doc[0].cropbox
        return doc.tobytes(), f"{first.width:.0f},{first.height:.0f}"
    except Exception:   # noqa: BLE001 - a preview that can't be trimmed is still a preview
        return pdf, ""


def print_direct(job: dict) -> None:
    """Send the report to the named printer (UtlOption.DirectPrinterName) from the PC the bridge runs on."""
    _call("/print", job)


def status() -> dict:
    """For the Print dialog and About: is the bridge set up and answering?"""
    if not configured():
        return {"configured": False, "ok": False, "message": "RPT_BRIDGE_URL is not set in backend/.env"}
    try:
        r = _call("/health", None, "GET")
        return {"configured": True, "ok": True, "message": "", **{k: v for k, v in r.json().items() if k in ("version", "printers")}}
    except HTTPException as e:
        return {"configured": True, "ok": False, "message": e.detail}
