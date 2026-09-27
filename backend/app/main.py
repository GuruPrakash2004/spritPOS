import logging

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from . import db, db_store, store
from .routers import auth, company_files, custom_reports, masters, reports, transactions

logging.basicConfig(level=logging.INFO, format="%(levelname)s %(name)s: %(message)s")
logger = logging.getLogger("vlqms.main")

app = FastAPI(
    title="Softvent vLQMS API",
    version="0.2.0",
    description=(
        "FastAPI port of the legacy vLQMS desktop screens for THE LIQUOR HOUSE (CL-2). "
        "Connected to SQL Server vLQMSLHOUSE."
    ),
)

# Vite dev server origins (CORS_ORIGINS in .env is not read).
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5174", "http://localhost:5173", "http://localhost:5175"],
    allow_methods=["*"],
    allow_headers=["*"],
)

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
        # Load heavy historical transactions in background thread so server starts in < 0.3s
        threading.Thread(target=db_store.load_transactions, daemon=True).start()
    else:
        logger.warning("Running with in-memory seed data (no DB connection)")


@app.get("/", tags=["Health"])
def health():
    return {
        "status": "ok",
        "docs": "/docs",
        "db": {
            "connected": db.is_db_available(),
            "driver": db.driver_name(),
            "host": db.DB_HOST,
            "port": db.DB_PORT,
            "database": db.DB_NAME,
        },
    }


@app.get("/settings", tags=["Global Settings"])
def get_settings():
    return store.SETTINGS


@app.put("/settings", tags=["Global Settings"])
def update_settings(body: dict):
    for section, values in body.items():
        store.SETTINGS.setdefault(section, {}).update(values)
    return store.SETTINGS


@app.get("/sales-persons", tags=["Sales Invoice"])
def sales_persons():
    return store.SALES_PERSONS
