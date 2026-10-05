# vLQMS API (FastAPI)

The legacy vLQMS desktop screens as REST endpoints. The .NET source isn't available (only the compiled exe), so these were rebuilt from `frontend/LEGACY_PARITY.md` and the legacy docs.

## Run

```bash
python3 -m venv .venv && .venv/bin/pip install -r requirements.txt
cp .env.example .env      # JWT/SMTP settings; the DB_* keys are written by the Database setup
.venv/bin/uvicorn app.main:app --reload --port 8001
```

Use port **8001**, because the frontend's Vite proxy points there. uvicorn does not read `API_PORT` from `.env`. Swagger UI: http://localhost:8001/docs · ReDoc: http://localhost:8001/redoc · Health (shows whether the DB is connected): http://localhost:8001/

## Database (one-time setup)

The web app keeps everything in the shop's own **Microsoft SQL Server** (the one the desktop vLQMS uses, usually on the same PC: `localhost` or `localhost\SQLEXPRESS`), in `Web_` tables: a copy of every legacy table with the same columns (`migrations/002_web_legacy_tables.sql`). There is no SQLite file.

The first time the backend starts without `DB_*` settings it prints a **setup code** in its window, and the sign-in page shows **Database setup**: enter the code, the server, SQL login (or Windows login) and password, pick the database, and press **Create tables and copy the old data**. The old tables are only read. The same can be done from the command line: `.venv/bin/python tools/setup_db.py check` / `run`. The runbook is [`migrations/README.md`](migrations/README.md).

## Many shops: Web Sync and the central site

As with the desktop app, every shop bills on its own PC against its own SQL Server, and sends a summary of each day plus its stock to a hosted **central site** every few minutes.

- **Shop side:** Utilities → Web Sync (`app/websync.py`). Set `WEB_SYNC_URL` and `WEB_SYNC_KEY` in `.env`; the key comes from the central site's administrator.
- **Central site:** `app/central/`, run with `uvicorn app.central.main:app --port 8002`. `tools/central.py` adds shops and owners.
- **How to set it up:** [`../deploy/central/README.md`](../deploy/central/README.md).

This backend also serves the built frontend (`frontend/dist`), so a shop PC needs only Python: open `http://<PC>:8001/`.

## Tests

```bash
.venv/bin/python -m pip install -r requirements-dev.txt   # pytest, pytest-asyncio, httpx, respx
.venv/bin/python -m pytest
```

`tests/` holds API tests that call the app in-process (httpx `AsyncClient` + `ASGITransport`) on the sample data in `app/sample_data.py`, installed by `tests/seed.py` (the app only uses it when an Administrator switches Test data on); they never connect to SQL Server. `conftest.py` has the shared fixtures: `client`, `admin_headers`/`cashier_headers`, `reset_store` (puts the in-memory store back after each test) and `block_network` (respx; mock any outside HTTP service as shown in `test_respx_example.py`). `test_admin_can_update_company` is marked `xfail` for the known `require_admin` bug; remove the marker once it is fixed.

## Data

- The running app keeps its data in memory (`app/store.py`), loaded at startup from the `Web_` tables (`app/boot.py`, `app/db_store.py`, then `app/sqlstore.load()` for the web's own rows, documents, stock moves and settings). Every successful change is written to SQL Server in the same request (`app/sqlstore.py`, one transaction; if it fails the request answers 503 and memory is reloaded). Uploaded company files are kept in `uploads/company/`.
- Without `DB_*` settings the app has no data and nobody can sign in (the sign-in page shows the Database setup); the tests install `app/sample_data.py`.
- The server name, login and the sealed password live only in `.env` (gitignored), never in code or docs.
- On macOS `pyodbc` is not installed (see `requirements.txt`), so only pymssql is tried (SQL logins only).

## Layout

| File | What it holds |
|---|---|
| `app/main.py` | App, CORS (localhost:5173–5175), the write middleware (saves each change), `/`, `/settings`, `/sales-persons` |
| `app/boot.py`, `app/setup_db.py`, `app/envfile.py`, `app/routers/setup.py` | Startup load; the one-time Database setup (`/setup/*`: status, test, connect, run, progress), the copy of the old data, the `.env` writer |
| `app/sqlstore.py` | Saves every change to the `Web_` tables (legacy format for masters, invoices, bills, stock moves; `Web_App*` for the rest) and loads it at startup |
| `app/store.py` | In-memory tables, `ROLES`, `DOCS`, `MOVEMENTS`, `move_stock()`, `next_doc_no()`, `find()` |
| `app/db.py`, `app/db_store.py` | SQL Server connection (named instance, optional port, SQL or Windows login, sealed password; `db.transaction()` for multi-statement writes) and the `Web_` table → store loaders |
| `app/mailer.py` | SMTP email with an .xls attachment (Customised Reports → Email) |
| `migrations/`, `tools/gen_web_schema.py`, `tools/setup_db.py`, `tools/migrate.py` | The generated `Web_` schema (002, with rollback), its generator, the command-line setup, and the migration runner |
| `app/schemas.py` | Pydantic request/response models |
| `app/routers/auth.py` | `POST /auth/login` (JWT), `GET /auth/me`, `/admin/users`, `/admin/roles` |
| `app/routers/masters.py` | `/masters/<name>` CRUD for every master, `/masters/company`, item price-list and import |
| `app/routers/transactions.py` | `/transactions/*`: purchase indent/bill, sales invoice (hold, release, cancel, reprint), sales return, expense, stock deduction, closing stock, denomination |
| `app/routers/reports.py` | `/reports/*`: stock balance/ledger/ageing, profit margin, reorder, DSR, hourly/daily/brand-wise sales, litres, stock movement, `.rpt` file list |
| `app/routers/custom_reports.py` | `GET /reports/custom`, the Customised Reports list; `POST /reports/custom/email` |
| `app/routers/company_files.py` | `/company/files`: company images and documents (8 MB limit) |
| `app/routers/tools.py`, `app/stock_rewrite.py` | The desktop TOOLS menu. SQL Query Execution (Administrator only; `GET /tools/sql/status`, `/objects`, `/columns`, `/script`, `/definition`, `/top` and `POST /tools/sql/execute`: one statement, SELECT anywhere, INSERT/UPDATE/DELETE only on `dbo.Web_*`; the old desktop tables are never written). Re Write Stock (`GET /tools/rewrite-stock` check, `POST /tools/rewrite-stock` rebuild; module right "Re Write Stock"). `GET /tools/audit` lists who ran what (`Web_ToolAudit`). |
| `app/routers/utilities.py`, `app/gsettings.py`, `app/admin_audit.py`, `app/duplicate_invoices.py`, `app/item_merge.py` | The desktop UTILITIES menu, built phase by phase: Global Settings (`/utilities/settings`, Administrator only; `GET /settings/values` for any user), Remove Duplicate Invoice (`/utilities/duplicate-invoices`, `/remove`), Merge Items / Replace and Remove (`/utilities/merge-items/{merge|replace}` and `/preview`), Compare Stock (`/utilities/compare-stock/current`), ADMIN Audit Log (`/utilities/audit`, `/utilities/audit/screens`) and About (`/utilities/about`). |
| `app/secure_fields.py` | Passwords typed in the UI arrive RSA-OAEP encrypted (`GET /auth/transport-key`, public) and are decrypted here; plain passwords are refused. `seal()`/`unseal()` keep server-side secrets AES-GCM encrypted. Keys in `data/` (0600). |

`Rpts/` holds the 24 legacy `.rpt` print formats the app uses (and `RPT_LAYOUT_AUDIT.md`), and `/reports/rpt-files` lists them. They are committed through the allow-list in `.gitignore`. Seven used formats embed the desktop app's SQL Server connection (server, database, user), so they are not committed: copy `fmtPI`, `fmtQTN`, `fmtSInv_GST`, `fmtSPInvPrint`, `fmtSR`, `RptDSR_8.5` and `UDRSalesItemwise` `.rpt` into `Rpts/` on the host by hand. `app/random.py` only holds a comment with a one-liner for generating a `JWT_SECRET`.

## Auth

There are no demo users: the users are the shop's `MstUser` users, who log in with their legacy password or lowercase user code (then stored as a hash). Their role follows the desktop role (`MstROLE`: ADMIN → Administrator, MANAGER or users OFFICE/MANAGER → Manager, else Cashier). When none is an Administrator, the sign-in page asks for one with the console setup code (`POST /setup/admin`). Passwords are kept only as salted PBKDF2-SHA256 hashes (`app/passwords.py`); five wrong passwords lock a user name for 15 minutes. Every endpoint needs `Authorization: Bearer <token>` from `/auth/login`, except those in `PUBLIC_ROUTES` in `app/routers/auth.py` (health, login, `GET /masters/company` and company file content). The old `X-User` header is ignored. Write endpoints also check the caller's role (`require_module`), and `/admin/*`, `PUT /settings`, company edits and company files are Administrator-only. Tokens are signed with `JWT_SECRET` from `.env`; without it a random secret is used and everyone must log in again after a restart.
