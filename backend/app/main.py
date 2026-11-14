"""FastAPI application: CORS, routers, startup DB load, and a few small endpoints.

Run from backend/:  .venv/bin/uvicorn app.main:app --reload --port 8001
On a shop PC the same process also serves the built frontend (frontend/dist, `npm run build`), so the shop
needs no Node.js: open http://<this PC>:8001/ and the API answers under /api (serve_frontend below).
On startup it connects to SQL Server (settings in backend/.env, written by the one-time Database setup) and
loads the shop's tables into the in-memory store (app/boot.py); without a database it has no data and nobody
can sign in (the sign-in page offers the Database setup). Every successful change is saved to SQL Server by app/sqlstore.py.
"""
import asyncio
import hashlib
import logging
import mimetypes
import os
import threading
from pathlib import Path

from fastapi import Depends, FastAPI, HTTPException, Request, Response
from fastapi.concurrency import run_in_threadpool
from fastapi.middleware.cors import CORSMiddleware
from fastapi.middleware.gzip import GZipMiddleware
from fastapi.responses import FileResponse, JSONResponse
from fastapi.security import HTTPAuthorizationCredentials

from . import admin_audit, boot, bridge_launcher, db, gsettings, held_bills, shop_hours, sqlstore, store
from .ai import bootstrap as ai_bootstrap, enabled as ai_enabled
from .routers import accounting as accounting_router, ai as ai_router, auth, quotation as quotation_router, quick_selection, sms as sms_router, attachments, rights_admin, company_files, custom_reports, designer as designer_router, masters, party_info, printing, physical_stock, reports, setup, tools, transactions, utilities
from .schemas import SettingsIn

logging.basicConfig(level=logging.INFO, format="%(levelname)s %(name)s: %(message)s")
logger = logging.getLogger("vlqms.main")

app = FastAPI(
    title="Softvent vLQMS API",
    version="0.2.0",
    description=(
        "FastAPI port of the legacy vLQMS desktop screens, directly on the shop's own SQL Server database."
    ),
    # Every endpoint needs a login token except auth.PUBLIC_ROUTES (health, login, sign-in page data).
    dependencies=[Depends(auth.require_login)],
)

# Writes run one at a time: two counters can't both sell the last bottle or take the same bill number,
# and each successful change is saved to SQL Server (app/sqlstore.py) before its reply goes out.
_write_lock = asyncio.Lock()
WRITE_METHODS = {"POST", "PUT", "DELETE"}
IDEMPOTENT_KEEP = 500   # replies remembered for Idempotency-Key replays
# POSTs that change no app data and may be slow (SMTP, SQL Server reconnect): they must not hold up bills.
# The Database setup endpoints save nothing into the store (they only connect and load).
# Connection switches (Utilities → About → Disconnect / Reconnect database): run one at a time like writes, but
# never refused for a database that is offline (they are how it is reconnected) and nothing is saved after them.
CONTROL_PATHS = {"/utilities/database/disconnect", "/utilities/database/reconnect"}
# Features that write straight into the shop's own tables (db.direct_writes) and never use the generic save (no Web_ tables).
DIRECT_WRITE_PATHS = ("/accounting/", "/transactions/quotation", "/transactions/expense", "/quick-selection", "/item-templates", "/sms/", "/admin/desktop-rights", "/masters/items/bulk-fields/import", "/masters/items/recalculate-rates", "/masters/item-groups/order", "/masters/packages/order")
NOT_WRITES = {"/auth/login", "/print/bulk-pdf", "/print/render", "/print/pages", "/setup/scan", "/utilities/backup", "/print/dsr", "/print/custom-report", "/print/stock-balance", "/print/email", "/designer/refresh", "/designer/reload", "/reports/custom/email", "/tools/sql/execute", "/setup/test", "/setup/connect", "/setup/rpt-folder", "/setup/smtp", "/setup/smtp/test", "/setup/print-printer", "/setup/print-printer/add",
              # AI suggestions: generating may take a while (the forecast) and rejecting changes only the ai.* tables.
              "/ai/suggestions/generate"}


def _idempotency_key(request: Request) -> str | None:
    """Replay key: the caller's token (hashed, so tokens are never kept), the path and the header."""
    key = request.headers.get("Idempotency-Key", "").strip()
    if not key or len(key) > 100:
        return None
    who = hashlib.sha256(request.headers.get("Authorization", "").encode()).hexdigest()[:32]
    return f"{who}|{request.url.path}|{key}"


def _still_signed_in(request: Request) -> bool:
    """A replay is only for a token that is still valid (not expired, not signed out by a password change)."""
    scheme, _, token = request.headers.get("Authorization", "").partition(" ")
    try:
        auth.get_current_user(HTTPAuthorizationCredentials(scheme=scheme, credentials=token))
        return scheme.lower() == "bearer"
    except HTTPException:
        return False


@app.middleware("http")
async def serialise_and_save_writes(request: Request, call_next):
    if request.method not in WRITE_METHODS or request.url.path in NOT_WRITES:
        return await call_next(request)
    if request.url.path in CONTROL_PATHS or request.url.path.startswith(DIRECT_WRITE_PATHS):
        async with _write_lock:
            return await call_next(request)
    async with _write_lock:
        key = _idempotency_key(request)
        body_hash = hashlib.sha256(await request.body()).hexdigest() if key else ""
        if key and key in store.IDEMPOTENT and _still_signed_in(request):
            saved = store.IDEMPOTENT[key]
            if saved["request"] != body_hash:
                # Same key, different bill: never answer it with the first bill's reply, and never save it
                # blindly either, because the first save may have gone through without the screen knowing.
                return JSONResponse({"detail": "This bill was already saved (the reply was lost). Check the last "
                                               "bill before saving again, or start a new bill."}, status_code=409)
            # Same save sent again (double-click, retry after a dropped reply): send the first reply back
            # and don't run the endpoint, so no second bill. Only successful replies are kept.
            return Response(saved["body"], status_code=saved["status"], media_type="application/json",
                            headers={"Idempotent-Replay": "true"})
        blocked = await run_in_threadpool(sqlstore.writes_blocked)
        if blocked:   # the database is set up but offline: nothing may change
            return JSONResponse({"detail": blocked}, status_code=503)
        admin_audit.begin()
        sqlstore.begin_request()
        response = await call_next(request)
        if response.status_code >= 400:
            admin_audit.discard()
            return response
        admin_audit.commit(getattr(request.state, "user", None))
        try:
            await run_in_threadpool(sqlstore.save, getattr(request.state, "user", None))
            if not db.use_web_tables():
                await run_in_threadpool(held_bills.dump_if_changed)   # held bills are kept in a file beside the backend (direct mode)
        except sqlstore.SaveFailed as exc:
            # SQL Server didn't take the change: undo it in memory too, and say so (nothing was saved).
            await run_in_threadpool(sqlstore.reload)
            return JSONResponse({"detail": f"Not saved: the database refused or lost the change ({exc}). Nothing was "
                                           "changed; try again."}, status_code=503)
        if key:
            body = b"".join([chunk async for chunk in response.body_iterator])
            store.IDEMPOTENT[key] = {"status": response.status_code, "body": body.decode(), "request": body_hash}
            for old in list(store.IDEMPOTENT)[:-IDEMPOTENT_KEEP]:
                del store.IDEMPOTENT[old]
            response = Response(body, status_code=response.status_code, headers=dict(response.headers))
        return response


# The built frontend, served by this backend on a shop PC (SERVE_FRONTEND=0 turns it off).
# Registered before CORS and the security headers, so the pages get the same headers as the API. Its API calls go
# to /api/...: the prefix is taken off here, as the Vite dev server's proxy does.
FRONTEND_DIST = Path(__file__).resolve().parents[2] / "frontend" / "dist"
# Python reads MIME types from the Windows registry, where .js can be text/plain, and browsers refuse module scripts
# served that way. Pin the types the built frontend needs.
for _ext, _type in ((".js", "text/javascript"), (".mjs", "text/javascript"), (".css", "text/css"), (".svg", "image/svg+xml")):
    mimetypes.add_type(_type, _ext)
API_PAGES = {"/docs", "/redoc", "/openapi.json", "/docs/oauth2-redirect"}   # the API's own pages stay the API's


def _dist_file(path: str) -> Path | None:
    """A file of the built frontend for this URL path, never outside frontend/dist."""
    if not path or path == "/":
        return None
    try:
        target = (FRONTEND_DIST / path.lstrip("/")).resolve()
    except (ValueError, OSError):   # e.g. a NUL byte in the path
        return None
    if FRONTEND_DIST.resolve() not in target.parents or not target.is_file():
        return None
    return target


# Registered after the write middleware so they wrap it: replayed replies get the same headers.
# Cross-origin requests are allowed from anywhere (any origin, method and header): the frontend is opened from
# another address than the backend's (its VITE_API_URL). The API uses a Bearer token, never cookies, so no
# credentials are involved.
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
    expose_headers=["Content-Disposition", "Idempotent-Replay", "X-Page-Size"],
)


class FrontendAndHeaders:
    """Pure ASGI (no BaseHTTPMiddleware, which costs several task hops per request): takes the /api prefix off,
    serves the built frontend on a page load, and adds the browser hardening headers to every response (no
    sniffing, no framing). Outermost layer, so every reply gets the headers."""

    def __init__(self, app):   # Starlette builds middleware as cls(app=...), so the name must be `app`
        self.inner = app

    async def __call__(self, scope, receive, send):
        if scope["type"] != "http":
            return await self.inner(scope, receive, send)
        path = scope["path"]
        method = scope["method"]
        no_store = path.startswith(("/auth", "/admin"))

        async def send_with_headers(message):
            if message["type"] == "http.response.start":
                headers = message.setdefault("headers", [])
                have = {k.lower() for k, _ in headers}
                extra = [(b"x-content-type-options", b"nosniff"), (b"x-frame-options", b"DENY"),
                         (b"cross-origin-resource-policy", b"cross-origin")]   # company images load from the frontend's page
                if no_store:
                    extra.append((b"cache-control", b"no-store"))
                headers.extend(h for h in extra if h[0] not in have)
            await send(message)

        if path == "/api" or path.startswith("/api/"):
            stripped = path[4:] or "/"
            scope = dict(scope, path=stripped, raw_path=stripped.encode())
            return await self.inner(scope, receive, send_with_headers)
        if method in ("GET", "HEAD") and os.environ.get("SERVE_FRONTEND", "1") != "0" and (FRONTEND_DIST / "index.html").is_file():
            file = _dist_file(path)
            if file is not None:
                return await FileResponse(file)(scope, receive, send_with_headers)
            accept = dict(scope["headers"]).get(b"accept", b"").decode("latin-1")
            if "text/html" in accept and path not in API_PAGES:   # a page load: the app
                return await FileResponse(FRONTEND_DIST / "index.html", headers={"Cache-Control": "no-cache"})(scope, receive, send_with_headers)
        return await self.inner(scope, receive, send_with_headers)


# Big lists (items, bills, reports) shrink a lot: less to send when many counters ask at once.
app.add_middleware(GZipMiddleware, minimum_size=1024, compresslevel=4)
app.add_middleware(FrontendAndHeaders)


app.include_router(auth.router)
app.include_router(auth.admin)
app.include_router(company_files.router)
app.include_router(masters.router)
app.include_router(party_info.router)
app.include_router(transactions.router)
app.include_router(accounting_router.router)
app.include_router(quotation_router.router)
app.include_router(quick_selection.router)
app.include_router(quick_selection.templates_router)
app.include_router(sms_router.router)
app.include_router(attachments.router)
app.include_router(rights_admin.router)
app.include_router(physical_stock.router)
app.include_router(physical_stock.stock_router)
app.include_router(reports.router)
app.include_router(custom_reports.router)
app.include_router(tools.router)
app.include_router(utilities.router)
app.include_router(utilities.values_router)
app.include_router(printing.router)
app.include_router(designer_router.router)
app.include_router(setup.router)
app.include_router(ai_router.router)


def _prepare_print_agent() -> None:
    """Build the Print Agent that counter PCs download (tools/printagent) when it isn't built yet; never stops the backend."""
    try:
        printing._agent_exe()
    except Exception as exc:   # noqa: BLE001 - a missing agent only means the download link says so
        logger.info("Print Agent not prepared: %s", getattr(exc, "detail", exc))


@app.on_event("startup")
def startup_event():
    """Connect to SQL Server (if set up) and load the data into the in-memory store (app/boot.py)."""
    bridge_launcher.start()   # the Crystal Reports bridge runs whenever the backend does (Windows, bridge on this PC); started first so it warms up while the data loads
    threading.Thread(target=_prepare_print_agent, daemon=True).start()   # the counter PCs' Print Agent is ready to download
    boot.load_data()
    if ai_enabled():   # tables, sales history and the forecasts are made in the background (app/ai/bootstrap.py)
        ai_bootstrap.start()


@app.on_event("shutdown")
def shutdown_event():
    bridge_launcher.stop()


@app.exception_handler(db.ReadOnlyDatabase)
async def read_only_database_handler(request, exc):
    return JSONResponse({"detail": str(exc)}, status_code=503)


@app.get("/", tags=["Health"])
def health():
    # Public (no login), so it says only whether the DB is connected, never where it is.
    return {"status": "ok", "docs": "/docs", "db": {"connected": db.is_db_available()}}


@app.get("/settings", tags=["Global Settings"])
def get_settings():
    return store.SETTINGS


@app.put("/settings", tags=["Global Settings"])
def update_settings(body: SettingsIn, admin: dict = Depends(auth.require_admin)):
    before = {k: dict(v) for k, v in store.SETTINGS.items()}
    gsettings.set_from_legacy(body.model_dump(exclude_none=True))
    for section in store.SETTINGS:
        earlier, changed = admin_audit.diff(before.get(section), store.SETTINGS[section])
        if changed:
            admin_audit.record("Global Settings", "Edited", section, earlier, changed)
    return store.SETTINGS


@app.get("/company/hours", tags=["Company Master"])
def company_hours():
    """Whether the shop is open now: Karnataka liquor-shop hours, closed on Holiday Master dates (app/shop_hours.py; the Dashboard's store banner)."""
    return shop_hours.status()


@app.get("/sales-persons", tags=["Sales Invoice"])
def sales_persons():
    """Names of the active sales persons (Sales Person Master, the desktop's MstSP)."""
    return [p["name"] for p in store.SALES_PERSONS.values() if p.get("active", True)]


