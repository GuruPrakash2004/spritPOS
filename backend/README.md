# vLQMS API (FastAPI)

The legacy vLQMS desktop screens as REST endpoints. The .NET source isn't available (only the compiled exe), so these were rebuilt from `frontend/LEGACY_PARITY.md` and the legacy docs.

## Run

```bash
python3 -m venv .venv && .venv/bin/pip install -r requirements.txt
cp .env.example .env      # optional: SQL Server + JWT settings
.venv/bin/uvicorn app.main:app --reload --port 8001
```

Use port **8001**, because the frontend's Vite proxy points there. uvicorn does not read `API_PORT` from `.env`. Swagger UI: http://localhost:8001/docs · ReDoc: http://localhost:8001/redoc · Health (shows whether the DB is connected): http://localhost:8001/

## Saved data

The web app's own changes (bills, stock movements, users and password hashes, roles and rights, master edits, settings) are saved after every successful write to `data/vlqms_state.sqlite3` (gitignored, owner-only) and restored at startup, over the seed or SQL Server data. They are **not** written to SQL Server. Delete that file to start fresh. `VLQMS_STATE_DB` changes its path; an empty value turns saving off.

## Tests

```bash
.venv/bin/python -m pip install -r requirements-dev.txt   # pytest, pytest-asyncio, httpx, respx
.venv/bin/python -m pytest
```

`tests/` holds API tests that call the app in-process (httpx `AsyncClient` + `ASGITransport`) on the seed data; they never connect to SQL Server. `conftest.py` has the shared fixtures: `client`, `admin_headers`/`cashier_headers`, `reset_store` (puts the in-memory store back after each test) and `block_network` (respx; mock any outside HTTP service as shown in `test_respx_example.py`). `test_admin_can_update_company` is marked `xfail` for the known `require_admin` bug; remove the marker once it is fixed.

## Data

- All data lives in memory in `app/store.py`, seeded with sample data. What the web app changes (bills, stock movements, masters, users, settings) is saved after every write to `data/vlqms_state.sqlite3` (`app/persistence.py`) and restored at startup. Uploaded company files are kept in `uploads/company/`.
- **SQL Server write-back** is off unless `DB_WRITE_ENABLED=true`. When on, web documents and stock changes are copied into separate `Web_*` tables (never the legacy tables, so the desktop app doesn't see them). Create the tables with `tools/migrate.py`; see [`migrations/README.md`](migrations/README.md) for the full runbook and safety checklist.
- At startup `app/db.py` tries to connect to the legacy SQL Server database `vLQMSLHOUSE` (pyodbc with ODBC Driver 18/17, then pymssql; settings from `.env`). If it connects, `app/db_store.py` replaces the seed masters with live data (company, items, groups, brands, UOMs, customers, suppliers, users) and loads the latest 200 sales invoices and 100 purchase bills in the background.
- The SQL Server is on the **office LAN**; put its host, port and credentials only in `.env` (gitignored), never in code or docs. Anywhere else the connection fails after its timeouts, and the API runs on the seed data. That's fine for development.
- On macOS `pyodbc` is not installed (see `requirements.txt`), so only pymssql is tried.

## Layout

| File | What it holds |
|---|---|
| `app/main.py` | App, CORS (localhost:5173–5175), startup DB load, `/`, `/settings`, `/sales-persons` |
| `app/store.py` | In-memory tables, `ROLES`, `DOCS`, `MOVEMENTS`, `move_stock()`, `next_doc_no()`, `find()` |
| `app/db.py`, `app/db_store.py` | SQL Server connection (`db.transaction()` for multi-statement writes) and the DB → store loaders |
| `app/persistence.py` | Saves and restores the web app's own data (local SQLite) |
| `app/writeback.py` | Queues web documents and sends them to the `Web_*` tables; `GET /admin/writeback`, `POST /admin/writeback/flush` |
| `app/mailer.py` | SMTP email with an .xls attachment (Customised Reports → Email) |
| `migrations/`, `tools/migrate.py` | Versioned SQL Server schema for the write-back, with rollbacks |
| `app/schemas.py` | Pydantic request/response models |
| `app/routers/auth.py` | `POST /auth/login` (JWT), `GET /auth/me`, `/admin/users`, `/admin/roles` |
| `app/routers/masters.py` | `/masters/<name>` CRUD for every master, `/masters/company`, item price-list and import |
| `app/routers/transactions.py` | `/transactions/*`: purchase indent/bill, sales invoice (hold, release, cancel, reprint), sales return, expense, stock deduction, closing stock, denomination |
| `app/routers/reports.py` | `/reports/*`: stock balance/ledger/ageing, profit margin, reorder, DSR, hourly/daily/brand-wise sales, litres, stock movement, `.rpt` file list |
| `app/routers/custom_reports.py` | `GET /reports/custom`, the Customised Reports list; `POST /reports/custom/email` |
| `app/routers/company_files.py` | `/company/files`: company images and documents (8 MB limit) |

`Rpts/` (gitignored) is a copy of the legacy report folder, and `/reports/rpt-files` lists its `.rpt` files. `app/random.py` only holds a comment with a one-liner for generating a `JWT_SECRET`.

## Auth

Demo users: `ADMIN`/`admin`, `GURU`/`guru` (Manager), `PRADEEP`/`pradeep`, `RAVI`/`ravi` (Cashier). When the DB is connected, each `MstUser` user can also log in with their legacy password or lowercase user code, which is then stored as a hash. Passwords are kept only as salted PBKDF2-SHA256 hashes (`app/passwords.py`); five wrong passwords lock a user name for 15 minutes. Every endpoint needs `Authorization: Bearer <token>` from `/auth/login`, except those in `PUBLIC_ROUTES` in `app/routers/auth.py` (health, login, `GET /masters/company` and company file content). The old `X-User` header is ignored. Write endpoints also check the caller's role (`require_module`), and `/admin/*`, `PUT /settings`, company edits and company files are Administrator-only. Tokens are signed with `JWT_SECRET` from `.env`; without it a random secret is used and everyone must log in again after a restart.
