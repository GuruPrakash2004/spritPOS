"""Screen definitions read live from the database, so the web screens follow the desktop's Doc Designer.

- GET /designer/docs          the screens the designer defines (DocID, name, type, tables)
- GET /designer/screen/{id}   one screen: its fields in display order with type, width, rules (app/schema_meta.py)
- GET /designer/version       a value that changes when a table or a designer row changes; open screens poll it
                              and read their definition again when it moves

Reading only. Any signed-in user may read a definition (it holds captions and rules, no data).
"""
import asyncio
import json
import time

from fastapi import APIRouter, Depends, HTTPException, Request
from fastapi.responses import StreamingResponse
from fastapi.concurrency import run_in_threadpool

from .. import db_store, designer, doc_engine, doc_meta, field_columns, schema_meta, security, sqlstore, store
from .auth import get_current_user, require_admin

router = APIRouter(tags=["Designer"])
_last = {"version": None}
_shared = {"at": 0.0, "version": ""}   # the latest fingerprint, shared by every page that asks within a few seconds
_refresh_lock = asyncio.Lock()


def _master_paths() -> dict[str, str]:
    """Desktop header table (lower case) -> the web master path that edits it ('items' for MstITEM, ...)."""
    out = {}
    for path, store_name in security.TABLES.items():
        if store_name in sqlstore.LEGACY_MASTERS:
            out[sqlstore.LEGACY_MASTERS[store_name][0].lower()] = path
    out["mstcomp"] = "company"   # the single-row Company Master
    return out


@router.get("/designer/docs")
async def docs(user: dict = Depends(get_current_user)):
    paths = _master_paths()
    return [{"doc_id": int(d["DocID"]), "name": str(d.get("DocName") or ""), "type": str(d.get("DocType") or ""),
             "short_form": str(d.get("DocShForm") or ""), "header_table": str(d.get("HdrTblName") or ""),
             "detail_table": str(d.get("DtlTblName") or ""),
             "master_path": paths.get(str(d.get("HdrTblName") or "").lower(), "") if str(d.get("DocType") or "") == "Master" else ""}
            for d in sorted(store.UTL_DOCS.values(), key=lambda d: int(d["DocID"]))]


@router.get("/designer/screen/{doc_id}")
async def screen(doc_id: int, user: dict = Depends(get_current_user)):
    result = await run_in_threadpool(schema_meta.screen, doc_id)
    if result is None:
        raise HTTPException(404, "No such screen in the Doc Designer.")
    result["version"] = _last["version"] or ""
    doc = store.UTL_DOCS.get(int(doc_id)) or {}
    result["flags"] = doc_meta.doc_flags(doc)   # what the document does (stock, copy from, authorisation, ...)
    result["numbering"] = doc_meta.numbering(int(doc_id))
    result["find_fields"] = doc_meta.find_fields(result["name"])
    result["master_path"] = _master_paths().get(result["header_table"].lower(), "") if result["type"] == "Master" else ""
    store_name = security.TABLES.get(result["master_path"], "")
    mapped = sqlstore._mapped(store_name) if store_name in sqlstore.LEGACY_MASTERS else None
    if result["master_path"] == "company":
        mapped = db_store.COMPANY_COLUMNS | sqlstore._NOT_EXTRA
    doc_path = field_columns.DOC_PATHS.get(result["header_table"].lower(), "")
    if doc_path:   # a bill: the writers fill the core header columns; any other designer header column is an extra
        result["doc_path"] = doc_path
        # Which line / header values feed the document's formulas (doc_engine.DOCS): the browser works the totals out live
        # from the same inputs the server uses when it saves.
        kind = doc_path.replace("-", "_")
        if kind in doc_engine.DOCS:
            result["inputs"] = {"line": doc_engine.DOCS[kind]["line"], "header": doc_engine.DOCS[kind]["header"]}
        result["web_fields"] = [w for w in field_columns.doc_fields(doc_path)]
        core = sqlstore.DOC_CORE[result["header_table"].lower()] | sqlstore._NOT_EXTRA
        for f in result["fields"]:
            f["modelled"] = f["code"].lower() in core or sqlstore.doc_blocked(f["code"])
            f["web_key"] = field_columns.web_key_for(doc_path, f["code"])
        return result
    if result["master_path"] in security.TABLES:   # the typed fields of the master's request model (the screen is built from both)
        result["web_fields"] = field_columns.web_fields(result["master_path"])
    inverse = {t: p for t, p in _master_paths().items()}
    for f in result["fields"]:   # the web master a drop-down's table belongs to, so its choices are the web's codes
        if f.get("combo"):
            f["combo"] = {**f["combo"], "master": inverse.get(str(f["combo"]["table"]).lower(), "")}
    for f in result["fields"]:   # modelled: the web app already has a typed field for this column
        f["modelled"] = mapped is None or f["code"].lower() in mapped
        # web_key: that typed field's key, so the browser lets the designer's caption/order/flags override it
        f["web_key"] = field_columns.web_key_for(result["master_path"], f["code"])
        # A key column (ITEMGroupID, UOMID ...): the designer hides it because the desktop shows a drop-down of
        # codes instead, which is the web's typed field. Only the column's name matches, so its flags must not apply.
        f["fk"] = bool(f["web_key"]) and f["code"].lower().endswith("id")
    return result


def _build_id() -> str:
    """Identifies the frontend build this backend serves (the time of its index.html), so an open page can tell it is old."""
    try:
        from ..main import FRONTEND_DIST
        return str(int((FRONTEND_DIST / "index.html").stat().st_mtime))
    except Exception:   # not served from here (dev server) or not built
        return ""


@router.post("/designer/reload")
async def reload_from_designer(admin: dict = Depends(require_admin)):
    """The top bar's "Refresh from Doc Designer" button (Administrator): read the designer's tables, the tables' column lists and the
    masters' extra columns again, now, and answer the new structure version. Nothing is read again on its own when a page loads."""
    if not sqlstore.enabled():
        raise HTTPException(409, "Not available while the database is offline.")
    async with _refresh_lock:
        try:
            await run_in_threadpool(designer.refresh)
            await run_in_threadpool(schema_meta.invalidate)
            await run_in_threadpool(sqlstore.reload_extras)
            current = await run_in_threadpool(schema_meta.fingerprint)
        except Exception as e:   # noqa: BLE001 - say why instead of a bare 500
            raise HTTPException(503, f"Could not read the Doc Designer settings: {e}")
        if current:
            _last["version"] = current
            _shared.update(at=time.monotonic(), version=current)
    return {"version": current, "build": _build_id()}


@router.get("/designer/version")
async def version(user: dict = Depends(get_current_user)):
    """The structure fingerprint. When it moved since the last call the designer's rows and the column lists are read
    again first, so the answer a browser gets next is already the new definition."""
    return await _check()


@router.get("/designer/preview")
async def preview(admin: dict = Depends(require_admin)):
    """For an Administrator: every screen the designer defines, what the server builds from it and what looks wrong."""
    def build():
        paths = _master_paths()
        out = []
        for d in sorted(store.UTL_DOCS.values(), key=lambda d: int(d["DocID"])):
            doc_id = int(d["DocID"])
            path = paths.get(str(d.get("HdrTblName") or "").lower(), "") if str(d.get("DocType") or "") == "Master" else ""
            rows = designer.fields_for(doc_id)
            hdr, dtl = schema_meta.columns(d.get("HdrTblName")), schema_meta.columns(d.get("DtlTblName"))
            screen = schema_meta.build_screen(d, rows, hdr, dtl, schema_meta.captions())
            problems = schema_meta.review(d, rows, hdr, dtl, path)
            out.append({"doc_id": doc_id, "name": screen["name"], "type": screen["type"], "short_form": screen["short_form"],
                        "header_table": screen["header_table"], "detail_table": screen["detail_table"], "master_path": path,
                        "designer_fields": len(rows), "shown_fields": len([f for f in screen["fields"] if not f["hidden"]]),
                        "problems": problems, "fields": screen["fields"]})
        return out
    return {"version": _last["version"] or "", "screens": await run_in_threadpool(build)}


@router.get("/designer/menu")
async def menu(user: dict = Depends(get_current_user)):
    """The desktop's menu (MstMenu) with each entry's document, group and order."""
    return doc_meta.menu()


@router.get("/designer/pickup/{doc_id}/{code}")
async def pickup(doc_id: int, code: str, values: str = "{}", user: dict = Depends(get_current_user)):
    """The value a field picks up after another field changed (the designer's FldPickUP SQL). `values` is a JSON object of
    the other fields' values by code (and SVTPARTY / SVTITEM). A read, so it never goes through the write queue."""
    try:
        given = json.loads(values)
    except ValueError:
        given = {}
    return {"value": await run_in_threadpool(doc_meta.pickup, doc_id, code, given if isinstance(given, dict) else {})}


@router.get("/designer/series/{doc_id}")
async def series(doc_id: int, user: dict = Depends(get_current_user)):
    """The code series in use for a document whose codes are generated (UtlTransactions.MstAutoGenCode)."""
    return {"series": await run_in_threadpool(doc_meta.series_list, doc_id)}


@router.get("/designer/next-code/{doc_id}")
async def next_code(doc_id: int, series: str = "", user: dict = Depends(get_current_user)):
    """The next number and code of a series: the highest number of that series plus one, code = series + six digits."""
    return await run_in_threadpool(doc_meta.next_code, doc_id, series)


@router.get("/designer/lookup/{doc_id}/{code}")
async def lookup(doc_id: int, code: str, q: str = "", user: dict = Depends(get_current_user)):
    """The choices of a drop-down field of a designer screen: [{value, label}]."""
    return await run_in_threadpool(doc_meta.lookup, doc_id, code, q)


async def _check() -> dict:
    """The current structure version (reading the designer's rows again when it moved) and the served front-end build."""
    if not sqlstore.enabled():
        return {"version": "", "build": _build_id()}
    now = time.monotonic()
    if _shared["at"] and now - _shared["at"] < FINGERPRINT_SECONDS and _shared["version"]:
        return {"version": _shared["version"], "build": _build_id()}
    current = await run_in_threadpool(schema_meta.fingerprint)
    if current:
        _shared.update(at=now, version=current)
    async with _refresh_lock:
        if current and _last["version"] is not None and current != _last["version"]:
            await run_in_threadpool(designer.refresh)
            await run_in_threadpool(sqlstore.reload_extras)
        if current:
            _last["version"] = current
    return {"version": current, "build": _build_id()}


EVENT_SECONDS = 3      # how often a stream asks for the shared answer below
FINGERPRINT_SECONDS = 5   # how long one database check serves every open page
HEARTBEAT_SECONDS = 15


@router.get("/designer/events")
async def events(request: Request, user: dict = Depends(get_current_user)):
    """Server-sent events: `data: {"version", "build"}` once at the start and again the moment the database's structure
    (a table's columns, a Doc Designer row, the menu ...) changes, so open pages redraw at once instead of waiting for the
    next poll. A comment line is sent every few seconds to keep proxies from closing the stream. The page falls back to
    GET /designer/version when the stream is not available. Needs the Bearer token, so the browser reads it with fetch
    rather than EventSource."""
    async def stream():
        sent, quiet = None, 0.0
        yield "retry: 5000\n\n"
        while not await request.is_disconnected():
            state = await _check()
            key = (state["version"], state["build"])
            if not state["version"] and sent is not None:   # a failed check is not a change
                key = sent
            if key != sent:
                sent, quiet = key, 0.0
                yield f"data: {json.dumps(state)}\n\n"
            elif quiet >= HEARTBEAT_SECONDS:
                quiet = 0.0
                yield ": keep-alive\n\n"
            await asyncio.sleep(EVENT_SECONDS)
            quiet += EVENT_SECONDS
    return StreamingResponse(stream(), media_type="text/event-stream",
                             headers={"Cache-Control": "no-cache", "X-Accel-Buffering": "no"})
