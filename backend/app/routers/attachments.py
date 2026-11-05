"""File Attachment on masters (the desktop's "File Attachment (n)" button on Item, Customer, Supplier, UOM, ... : frmUtlFileControl).

The desktop copies each file into a shared TrnDocFiles folder and lists it in UtlAttachFiles. The web server has no access to that folder, so
files are kept here on disk (backend/uploads/attachments/<master>/, one JSON index per master) and are not written to UtlAttachFiles: a file
attached in the web is seen in the web. Uploads arrive as base64 JSON (no multipart dependency), 8 MB at most, from an allow-list of types.
Seeing the list needs the master's module; adding, changing or removing a file needs its Edit right."""
import base64
import binascii
import json
import re
import uuid
from datetime import datetime
from pathlib import Path

from fastapi import APIRouter, Depends, HTTPException, Query
from fastapi.responses import FileResponse
from pydantic import BaseModel, Field

from ..master_specs import SPECS
from .auth import can_use, get_current_user, has_right

router = APIRouter(prefix="/attachments", tags=["Attachments"])

STORE = Path(__file__).resolve().parents[2] / "uploads" / "attachments"
MAX_BYTES = 8 * 1024 * 1024
TYPES = {
    ".pdf": "application/pdf", ".xlsx": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xls": "application/vnd.ms-excel",
    ".docx": "application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".doc": "application/msword", ".csv": "text/csv",
    ".txt": "text/plain", ".jpg": "image/jpeg", ".jpeg": "image/jpeg", ".png": "image/png", ".webp": "image/webp", ".gif": "image/gif",
}   # SVG / HTML are left out on purpose: they can carry script

# master path -> module name (the same names the rights use)
MODULES = {
    "items": "Item Master", "item-groups": "Item Group Master", "uoms": "UOM Master", "packages": "Package Master", "brands": "Brand Master",
    "suppliers": "Supplier Master", "customers": "Customer Master", "ledger-groups": "Ledger Group Master", "ledgers": "Ledger Master",
    "sales-counters": "Sales Counter Master", "sales-persons": "Sales Person Master", **{s["path"]: s["title"] for s in SPECS},
}


class FileIn(BaseModel):
    file_name: str = Field(min_length=1, max_length=200)
    data: str = Field(description="File content, base64 encoded")
    remarks: str = Field("", max_length=200)


def _module(master: str) -> str:
    if master not in MODULES:
        raise HTTPException(404, "Unknown master")
    return MODULES[master]


def _index(master: str) -> Path:
    return STORE / master / "index.json"


def _load(master: str) -> list[dict]:
    p = _index(master)
    return json.loads(p.read_text(encoding="utf-8")) if p.exists() else []


def _save(master: str, rows: list[dict]) -> None:
    p = _index(master)
    p.parent.mkdir(parents=True, exist_ok=True)
    p.write_text(json.dumps(rows, indent=1), encoding="utf-8")


def _need(user: dict, module: str, action: str | None) -> None:
    if not can_use(user, module) or (action and not has_right(user, action, module)):
        raise HTTPException(403, f"Your role ({user.get('role')}) is not allowed to {action or 'open'} {module}")


@router.get("/{master}")
def list_files(master: str, code: str = Query(min_length=1), user: dict = Depends(get_current_user)):
    _need(user, _module(master), None)
    return [r for r in _load(master) if r["code"] == code]


@router.get("/{master}/counts")
def counts(master: str, user: dict = Depends(get_current_user)):
    _need(user, _module(master), None)
    out: dict[str, int] = {}
    for r in _load(master):
        out[r["code"]] = out.get(r["code"], 0) + 1
    return out


@router.post("/{master}", status_code=201)
def upload(master: str, body: FileIn, code: str = Query(min_length=1), user: dict = Depends(get_current_user)):
    module = _module(master)
    _need(user, module, "edit")
    ext = Path(body.file_name).suffix.lower()
    if ext not in TYPES:
        raise HTTPException(422, f"{ext or 'This file type'} is not allowed. Use: {', '.join(sorted(TYPES))}")
    try:
        content = base64.b64decode(body.data, validate=True)
    except (binascii.Error, ValueError):
        raise HTTPException(422, "File data is not valid base64")
    if not content:
        raise HTTPException(422, "The file is empty")
    if len(content) > MAX_BYTES:
        raise HTTPException(413, f"File is larger than {MAX_BYTES // (1024 * 1024)} MB")
    file_id = uuid.uuid4().hex
    (STORE / master).mkdir(parents=True, exist_ok=True)
    (STORE / master / f"{file_id}{ext}").write_bytes(content)   # a generated name on disk, never the user's
    row = {"id": file_id, "code": code, "file_name": re.sub(r"[^A-Za-z0-9._ -]+", "_", Path(body.file_name).name)[:120], "ext": ext,
           "size": len(content), "remarks": body.remarks, "added_by": user["user"], "added_on": datetime.now().isoformat(timespec="seconds")}
    rows = _load(master)
    rows.append(row)
    _save(master, rows)
    return row


@router.delete("/{master}/{file_id}", status_code=204)
def delete(master: str, file_id: str, user: dict = Depends(get_current_user)):
    _need(user, _module(master), "edit")
    rows = _load(master)
    row = next((r for r in rows if r["id"] == file_id), None)
    if not row:
        raise HTTPException(404, "File not found")
    (STORE / master / f"{row['id']}{row['ext']}").unlink(missing_ok=True)
    _save(master, [r for r in rows if r["id"] != file_id])


@router.get("/{master}/{file_id}/content")
def content(master: str, file_id: str, download: bool = False, user: dict = Depends(get_current_user)):
    _need(user, _module(master), None)
    row = next((r for r in _load(master) if r["id"] == file_id), None)
    path = STORE / master / f"{row['id']}{row['ext']}" if row else None
    if not row or not path.exists():
        raise HTTPException(404, "File not found")
    return FileResponse(path, media_type=TYPES[row["ext"]], filename=row["file_name"], content_disposition_type="attachment" if download else "inline",
                        headers={"X-Content-Type-Options": "nosniff"})
