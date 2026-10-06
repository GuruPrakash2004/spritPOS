"""FastAPI application: CORS, routers, startup DB load, and a few small endpoints.

Run from backend/:  .venv/bin/uvicorn app.main:app --reload --port 8001
On a shop PC the same process also serves the built frontend (frontend/dist, `npm run build`), so the shop
needs no Node.js: open http://<this PC>:8001/ and the API answers under /api (serve_frontend below).
On startup it connects to SQL Server (settings in backend/.env, written by the one-time Database setup) and
loads the Web_ tables into the in-memory store (app/boot.py); without a database it has no data and nobody
can sign in (the sign-in page offers the Database setup). Every successful change is saved to SQL Server by app/sqlstore.py.
"""
import asyncio
import hashlib
import logging
import os
from pathlib import Path

from fastapi import Depends, FastAPI, HTTPException, Request, Response
from fastapi.concurrency import run_in_threadpool
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import FileResponse, JSONResponse
from fastapi.security import HTTPAuthorizationCredentials

from . import admin_audit, boot, db, gsettings, shop_hours, sqlstore, store
from .routers import auth, company_files, custom_reports, masters, printing, reports, setup, tools, transactions, utilities
from .schemas import SettingsIn

logging.basicConfig(level=logging.INFO, format="%(levelname)s %(name)s: %(message)s")
logger = logging.getLogger("vlqms.main")

app = FastAPI(
    title="Softvent vLQMS API",
    version="0.2.0",
    description=(
        "FastAPI port of the legacy vLQMS desktop screens, on the shop's own SQL Server (the Web_ tables)."
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
# The Database setup endpoints save nothing into the store (setup runs its own copy).
# Web Sync only sends a summary to the central site (it can wait on the network for a while).
# Testing switches (Utilities → About, the header's Test data): run one at a time like writes, but never refused
# for a database that is offline (they are how it is reconnected) and nothing is saved after them.
CONTROL_PATHS = {"/utilities/test-data", "/utilities/database/disconnect", "/utilities/database/reconnect"}
# Writes refused while Test data is on (sqlstore.KEEP_IN_TEST: they would not be saved and would be undone).
KEPT_IN_TEST = ("/admin/", "/auth/change-password", "/settings", "/masters/company", "/utilities/settings",
                "/utilities/doc-settings", "/utilities/numbering", "/utilities/menu", "/utilities/import-fields")
NOT_WRITES = {"/print/render", "/print/email", "/designer/refresh", "/reports/custom/email", "/tools/sql/execute", "/setup/test", "/setup/connect", "/setup/run",
              "/utilities/web-sync"}


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
    if request.url.path in CONTROL_PATHS:
        async with _write_lock:
            return await call_next(request)
    if sqlstore.test_data() and request.url.path.startswith(KEPT_IN_TEST):
        # Users, roles, rights and settings stay the shop's own in test mode and aren't saved; a change would be
        # undone (even a password change or a deactivation) when Test data is switched off.
        return JSONResponse({"detail": "Test data is on: users, rights, settings and the company profile can't be changed "
                                       "now (the change would be lost). Switch Test data off first."}, status_code=409)
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
        if blocked:   # the database is set up but offline, or the setup is copying: nothing may change
            return JSONResponse({"detail": blocked}, status_code=503)
        admin_audit.begin()
        response = await call_next(request)
        if response.status_code >= 400:
            admin_audit.discard()
            return response
        admin_audit.commit(getattr(request.state, "user", None))
        try:
            await run_in_threadpool(sqlstore.save, getattr(request.state, "user", None))
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


@app.middleware("http")
async def serve_frontend(request: Request, call_next):
    path = request.scope["path"]
    if path == "/api" or path.startswith("/api/"):
        stripped = path[4:] or "/"
        request.scope["path"] = stripped
        request.scope["raw_path"] = stripped.encode()
        return await call_next(request)
    if (request.method in ("GET", "HEAD") and os.environ.get("SERVE_FRONTEND", "1") != "0"
            and (FRONTEND_DIST / "index.html").is_file()):
        file = _dist_file(path)
        if file is not None:
            return FileResponse(file)
        if "text/html" in request.headers.get("accept", "") and path not in API_PAGES:   # a page load: the app
            return FileResponse(FRONTEND_DIST / "index.html", headers={"Cache-Control": "no-cache"})
    return await call_next(request)


# Registered after the write middleware so they wrap it: replayed replies get the same headers.
# CORS_ORIGINS in .env (comma-separated) for a frontend served from another address; by default the Vite dev
# server's. Not needed when this backend serves the frontend itself (same origin).
CORS_ORIGINS = [o.strip() for o in os.environ.get("CORS_ORIGINS", "").split(",") if o.strip()] or [
    "http://localhost:5174", "http://localhost:5173", "http://localhost:5175"]
app.add_middleware(
    CORSMiddleware,
    allow_origins=CORS_ORIGINS,
    allow_methods=["GET", "POST", "PUT", "DELETE"],
    allow_headers=["Authorization", "Content-Type", "Idempotency-Key"],
)


@app.middleware("http")
async def security_headers(request: Request, call_next):
    """Browser hardening headers on every response (no sniffing, no framing, no referrer leaks). The referrer is
    the site's origin only (never a path): a Google Maps browser key restricted to this site needs it."""
    response = await call_next(request)
    response.headers.setdefault("X-Content-Type-Options", "nosniff")
    response.headers.setdefault("X-Frame-Options", "DENY")
    response.headers.setdefault("Referrer-Policy", "strict-origin")
    response.headers.setdefault("Cross-Origin-Resource-Policy", "same-origin")
    if request.url.path.startswith(("/auth", "/admin")):
        response.headers.setdefault("Cache-Control", "no-store")
    return response


app.include_router(auth.router)
app.include_router(auth.admin)
app.include_router(company_files.router)
app.include_router(masters.router)
app.include_router(transactions.router)
app.include_router(reports.router)
app.include_router(custom_reports.router)
app.include_router(tools.router)
app.include_router(utilities.router)
app.include_router(utilities.values_router)
app.include_router(printing.router)
app.include_router(setup.router)


@app.on_event("startup")
def startup_event():
    """Connect to SQL Server (if set up) and load the data into the in-memory store (app/boot.py)."""
    boot.load_data()


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
    """Whether the shop is open now, from its opening hours on Google Maps (app/shop_hours.py; the Dashboard's store banner)."""
    return shop_hours.status()


@app.get("/sales-persons", tags=["Sales Invoice"])
def sales_persons():
    """Names of the active sales persons (Sales Person Master, the desktop's MstSP)."""
    return [p["name"] for p in store.SALES_PERSONS.values() if p.get("active", True)]


