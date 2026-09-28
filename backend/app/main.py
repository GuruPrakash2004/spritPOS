"""FastAPI application: CORS, routers, startup DB load, and a few small endpoints.

Run from backend/:  .venv/bin/uvicorn app.main:app --reload --port 8001
On startup it connects to SQL Server (settings in backend/.env) and loads live data into
the in-memory store; without a connection it runs on the seed data in store.py.
"""
import asyncio
import hashlib
import logging

from fastapi import Depends, FastAPI, HTTPException, Request, Response
from fastapi.concurrency import run_in_threadpool
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
from fastapi.security import HTTPAuthorizationCredentials

from . import db, db_store, persistence, store, writeback
from .routers import auth, company_files, custom_reports, masters, reports, transactions
from .schemas import SettingsIn

logging.basicConfig(level=logging.INFO, format="%(levelname)s %(name)s: %(message)s")
logger = logging.getLogger("vlqms.main")

app = FastAPI(
    title="Softvent vLQMS API",
    version="0.2.0",
    description=(
        "FastAPI port of the legacy vLQMS desktop screens for THE LIQUOR HOUSE (CL-2). "
        "Connected to SQL Server vLQMSLHOUSE."
    ),
    # Every endpoint needs a login token except auth.PUBLIC_ROUTES (health, login, sign-in page data).
    dependencies=[Depends(auth.require_login)],
)

# Writes run one at a time: two counters can't both sell the last bottle or take the same bill number,
# and the saved state (app/persistence.py) is written after each successful change. A successful
# request's documents are also queued for SQL Server when write-back is on (app/writeback.py).
_write_lock = asyncio.Lock()
WRITE_METHODS = {"POST", "PUT", "DELETE"}
IDEMPOTENT_KEEP = 500   # replies remembered for Idempotency-Key replays
# POSTs that change no app data and may be slow (SMTP, SQL Server reconnect): they must not hold up bills.
NOT_WRITES = {"/reports/custom/email", "/admin/writeback/flush"}


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
        moves_from = writeback.begin_request()
        response = await call_next(request)
        if response.status_code >= 400:
            writeback.discard_request()
            return response
        writeback.commit_request(moves_from, getattr(request.state, "user", None))
        if key:
            body = b"".join([chunk async for chunk in response.body_iterator])
            store.IDEMPOTENT[key] = {"status": response.status_code, "body": body.decode(), "request": body_hash}
            for old in list(store.IDEMPOTENT)[:-IDEMPOTENT_KEEP]:
                del store.IDEMPOTENT[old]
            response = Response(body, status_code=response.status_code, headers=dict(response.headers))
        try:
            await run_in_threadpool(persistence.save)
        except Exception:   # never fail the user's request because the local save failed
            logger.exception("Could not save app data to %s", persistence.PATH)
        return response


# Registered after the write middleware so they wrap it: replayed replies get the same headers.
# Vite dev server origins (CORS_ORIGINS in .env is not read).
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5174", "http://localhost:5173", "http://localhost:5175"],
    allow_methods=["GET", "POST", "PUT", "DELETE"],
    allow_headers=["Authorization", "Content-Type", "Idempotency-Key"],
)


@app.middleware("http")
async def security_headers(request: Request, call_next):
    """Browser hardening headers on every response (no sniffing, no framing, no referrer leaks)."""
    response = await call_next(request)
    response.headers.setdefault("X-Content-Type-Options", "nosniff")
    response.headers.setdefault("X-Frame-Options", "DENY")
    response.headers.setdefault("Referrer-Policy", "no-referrer")
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


@app.on_event("startup")
def startup_event():
    """Connect to SQL Server and load live data into the in-memory store."""
    import threading
    ok = db.init_db()
    if ok:
        db_store.load_masters()
    else:
        logger.warning("Running with in-memory seed data (no DB connection)")
    # Lay the web app's own saved data (bills, stock movements, users, edits) over the seed/DB data.
    if persistence.load():
        logger.info("Restored saved app data from %s", persistence.PATH)
    writeback.masters_from_db = ok
    writeback.start()
    if ok:
        # Load heavy historical transactions in background thread so server starts in < 0.3s
        threading.Thread(target=db_store.load_transactions, daemon=True).start()


@app.get("/", tags=["Health"])
def health():
    # Public (no login), so it says only whether the DB is connected, never where it is.
    return {"status": "ok", "docs": "/docs", "db": {"connected": db.is_db_available()}}


@app.get("/settings", tags=["Global Settings"])
def get_settings():
    return store.SETTINGS


@app.put("/settings", tags=["Global Settings"])
def update_settings(body: SettingsIn, admin: dict = Depends(auth.require_admin)):
    for section, values in body.model_dump(exclude_none=True).items():
        store.SETTINGS[section].update(values)
    return store.SETTINGS


@app.get("/sales-persons", tags=["Sales Invoice"])
def sales_persons():
    return store.SALES_PERSONS


@app.get("/admin/writeback", tags=["Admin Master"])
def writeback_status(admin: dict = Depends(auth.require_admin)):
    """SQL Server write-back: on/off, events waiting, last error and last successful sync."""
    return writeback.state()


@app.post("/admin/writeback/flush", tags=["Admin Master"])
def writeback_flush(admin: dict = Depends(auth.require_admin)):
    """Send the queued writes now instead of waiting for the next retry."""
    if not writeback.ENABLED:
        raise HTTPException(409, "Write-back is off (set DB_WRITE_ENABLED=true in backend/.env and restart)")
    sent = writeback.flush()
    return {"sent": sent, **writeback.state()}
