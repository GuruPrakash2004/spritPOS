# vLQMS API (FastAPI)

The legacy vLQMS desktop screens as REST endpoints. The .NET source isn't available (only the compiled exe), so these were rebuilt from `frontend/LEGACY_PARITY.md` and the legacy docs.

## Run

```bash
python3 -m venv .venv && .venv/bin/pip install -r requirements.txt
cp .env.example .env      # optional: SQL Server + JWT settings
.venv/bin/uvicorn app.main:app --reload --port 8001
```

Use port **8001**, because the frontend's Vite proxy points there. uvicorn does not read `API_PORT` from `.env`. Swagger UI: http://localhost:8001/docs · ReDoc: http://localhost:8001/redoc · Health (shows whether the DB is connected): http://localhost:8001/

## Data

- All data lives in memory in `app/store.py`, seeded with sample data. Every write (masters, invoices, bills, …) changes only memory and is lost on restart. Uploaded company files are the only thing kept on disk (`uploads/company/`).
- At startup `app/db.py` tries to connect to the legacy SQL Server database `vLQMSLHOUSE` (pyodbc with ODBC Driver 18/17, then pymssql; settings from `.env`). If it connects, `app/db_store.py` replaces the seed masters with live data (company, items, groups, brands, UOMs, customers, suppliers, users) and loads the latest 200 sales invoices and 100 purchase bills in the background.
- The SQL Server is on the **office LAN**; put its host, port and credentials only in `.env` (gitignored), never in code or docs. Anywhere else the connection fails after its timeouts, and the API runs on the seed data. That's fine for development.
- On macOS `pyodbc` is not installed (see `requirements.txt`), so only pymssql is tried.

## Layout

| File | What it holds |
|---|---|
| `app/main.py` | App, CORS (localhost:5173–5175), startup DB load, `/`, `/settings`, `/sales-persons` |
| `app/store.py` | In-memory tables, `ROLES`, `DOCS`, `MOVEMENTS`, `move_stock()`, `next_doc_no()`, `find()` |
| `app/db.py`, `app/db_store.py` | SQL Server connection and the DB → store loaders |
| `app/schemas.py` | Pydantic request/response models |
| `app/routers/auth.py` | `POST /auth/login` (JWT), `GET /auth/me`, `/admin/users`, `/admin/roles` |
| `app/routers/masters.py` | `/masters/<name>` CRUD for every master, `/masters/company`, item price-list and import |
| `app/routers/transactions.py` | `/transactions/*`: purchase indent/bill, sales invoice (hold, release, cancel, reprint), sales return, expense, stock deduction, closing stock, denomination |
| `app/routers/reports.py` | `/reports/*`: stock balance/ledger/ageing, profit margin, reorder, DSR, hourly/daily/brand-wise sales, litres, stock movement, `.rpt` file list |
| `app/routers/custom_reports.py` | `GET /reports/custom`, the Customised Reports list |
| `app/routers/company_files.py` | `/company/files`: company images and documents (8 MB limit) |

`Rpts/` (gitignored) is a copy of the legacy report folder, and `/reports/rpt-files` lists its `.rpt` files. `app/random.py` only holds a comment with a one-liner for generating a `JWT_SECRET`.

## Auth

Demo users: `ADMIN`/`admin`, `GURU`/`guru` (Manager), `PRADEEP`/`pradeep`, `RAVI`/`ravi` (Cashier). When the DB is connected, each `MstUser` user can also log in with their lowercase user code as the password. Send `Authorization: Bearer <token>` from `/auth/login` (or the legacy `X-User: <user>` header). Only `/admin/*`, company edits and company file changes check rights, plus a 403 when a Cashier deletes an item.

Known issue: `PUT /masters/company` and the company file POST/PUT/DELETE call `require_admin(x_user)` directly instead of `Depends(require_admin)`, so they return 500.
