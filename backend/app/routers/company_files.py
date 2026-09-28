"""Company Master → Images and Documents: files the administrator uploads for the company.

Unlike the rest of the demo data these are kept on disk (backend/uploads/company/) with a small JSON
index, so uploaded licences and photos survive a server restart. Uploads arrive as base64 JSON so the
API needs no multipart dependency; the size cap keeps that reasonable.
"""
import base64
import binascii
import json
import re
import uuid
from datetime import datetime
from pathlib import Path
from typing import Literal

from fastapi import APIRouter, Depends, HTTPException
from fastapi.responses import FileResponse
from pydantic import BaseModel, Field

from .auth import require_admin

router = APIRouter(prefix="/company/files", tags=["Company Master"])

STORE = Path(__file__).resolve().parents[2] / "uploads" / "company"
INDEX = STORE / "index.json"
MAX_BYTES = 8 * 1024 * 1024

# Allow-list by extension → the Content-Type we serve it with. SVG/HTML are left out on purpose:
# they can carry script and would run on this origin when opened.
IMAGE_TYPES = {".jpg": "image/jpeg", ".jpeg": "image/jpeg", ".png": "image/png", ".webp": "image/webp", ".gif": "image/gif"}
DOC_TYPES = {
    ".pdf": "application/pdf",
    ".xlsx": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    ".xls": "application/vnd.ms-excel",
    ".docx": "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    ".doc": "application/msword",
    ".csv": "text/csv",
    ".txt": "text/plain",
    **IMAGE_TYPES,
}
TYPE_LABELS = {".pdf": "PDF Document", ".xlsx": "Excel Spreadsheet", ".xls": "Excel Spreadsheet", ".docx": "Word Document",
               ".doc": "Word Document", ".csv": "CSV File", ".txt": "Text File"}


class FileIn(BaseModel):
    kind: Literal["image", "document"]
    file_name: str = Field(min_length=1, max_length=200)
    data: str = Field(description="File content, base64 encoded")
    title: str = Field("", max_length=80)
    group: str = Field("", max_length=40)
    remarks: str = Field("", max_length=200)


class FileUpdate(BaseModel):
    title: str = Field("", max_length=80)
    group: str = Field("", max_length=40)
    remarks: str = Field("", max_length=200)
    show_on_pos: bool = False


def _load() -> list[dict]:
    return json.loads(INDEX.read_text()) if INDEX.exists() else []


def _save(rows: list[dict]) -> None:
    STORE.mkdir(parents=True, exist_ok=True)
    INDEX.write_text(json.dumps(rows, indent=2))


def _find(rows: list[dict], file_id: str) -> dict:
    row = next((r for r in rows if r["id"] == file_id), None)
    if not row:
        raise HTTPException(404, "File not found")
    return row


@router.get("")
def list_files(kind: Literal["image", "document"] | None = None):
    return [r for r in _load() if not kind or r["kind"] == kind]


@router.post("", status_code=201)
def upload_file(body: FileIn, admin: dict = Depends(require_admin)):
    ext = Path(body.file_name).suffix.lower()
    allowed = IMAGE_TYPES if body.kind == "image" else DOC_TYPES
    if ext not in allowed:
        raise HTTPException(422, f"{ext or 'This file type'} is not allowed. Use: {', '.join(sorted(allowed))}")
    try:
        content = base64.b64decode(body.data, validate=True)
    except (binascii.Error, ValueError):
        raise HTTPException(422, "File data is not valid base64")
    if not content:
        raise HTTPException(422, "The file is empty")
    if len(content) > MAX_BYTES:
        raise HTTPException(413, f"File is larger than {MAX_BYTES // (1024 * 1024)} MB")

    file_id = uuid.uuid4().hex
    STORE.mkdir(parents=True, exist_ok=True)
    (STORE / f"{file_id}{ext}").write_bytes(content)  # stored under a generated name, never the user's
    # Keep a readable original name for display/download, stripped of path and odd characters.
    safe_name = re.sub(r"[^A-Za-z0-9._ -]+", "_", Path(body.file_name).name)[:120]
    row = {
        "id": file_id, "kind": body.kind, "file_name": safe_name, "ext": ext, "size": len(content),
        "type_label": "Image" if ext in IMAGE_TYPES else TYPE_LABELS.get(ext, ext.upper()),
        "title": body.title or Path(safe_name).stem, "group": body.group, "remarks": body.remarks,
        "show_on_pos": body.kind == "image",
        "uploaded_by": admin["user"], "uploaded_at": datetime.now().isoformat(timespec="seconds"),
    }
    rows = _load()
    rows.append(row)
    _save(rows)
    return row


@router.put("/{file_id}")
def update_file(file_id: str, body: FileUpdate, admin: dict = Depends(require_admin)):
    rows = _load()
    row = _find(rows, file_id)
    row.update(title=body.title or row["title"], group=body.group, remarks=body.remarks, show_on_pos=body.show_on_pos)
    _save(rows)
    return row


@router.delete("/{file_id}", status_code=204)
def delete_file(file_id: str, admin: dict = Depends(require_admin)):
    rows = _load()
    row = _find(rows, file_id)
    (STORE / f"{row['id']}{row['ext']}").unlink(missing_ok=True)
    _save([r for r in rows if r["id"] != file_id])


@router.get("/{file_id}/content")
def file_content(file_id: str, download: bool = False):
    row = _find(_load(), file_id)
    path = STORE / f"{row['id']}{row['ext']}"
    if not path.exists():
        raise HTTPException(404, "File is missing on the server")
    return FileResponse(
        path,
        media_type=DOC_TYPES[row["ext"]],
        filename=row["file_name"],
        content_disposition_type="attachment" if download else "inline",
        headers={"X-Content-Type-Options": "nosniff"},
    )
