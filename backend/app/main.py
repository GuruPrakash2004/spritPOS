from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from . import store
from .routers import auth, company_files, custom_reports, masters, reports, transactions

app = FastAPI(
    title="Softvent vLQMS API",
    version="0.1.0",
    description="FastAPI port of the legacy vLQMS desktop screens for THE LIQUOR HOUSE (CL-2). "
                "Data is in memory and resets on restart.",
)

# The Vite dev server runs on 5173.
app.add_middleware(CORSMiddleware, allow_origins=["http://localhost:5173"], allow_methods=["*"], allow_headers=["*"])

app.include_router(auth.router)
app.include_router(auth.admin)
app.include_router(company_files.router)
app.include_router(masters.router)
app.include_router(transactions.router)
app.include_router(reports.router)
app.include_router(custom_reports.router)


@app.get("/", tags=["Health"])
def health():
    return {"status": "ok", "docs": "/docs"}


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
