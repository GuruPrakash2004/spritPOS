# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A web version of **vLQMS**, a legacy Windows liquor-store ERP by Softvent (masters, purchase/sales transactions, stock, DSR and reports). It has two parts: a React SPA in `frontend/` and a FastAPI backend in `backend/`. The legacy .NET source isn't available, so both were rebuilt from the legacy docs. `frontend/LEGACY_PARITY.md` maps every legacy screen to its status here (Built / Partial / Stub / Missing) and lists the gaps. Read it before adding or extending a screen, and update it when you close a gap. `rpts_files/Rpts/` holds legacy artefacts (Crystal `.rpt` print formats, the item-import template) for reference.

## Commands

Backend, from `backend/`:

```bash
python3 -m venv .venv && .venv/bin/pip install -r requirements.txt
cp .env.example .env          # SQL Server + JWT settings; optional (see "Data source" below)
.venv/bin/uvicorn app.main:app --reload --port 8001   # Swagger UI at http://localhost:8001/docs
```

The port matters: `frontend/vite.config.ts` proxies `/api/*` to **:8001** (prefix stripped). uvicorn does not read `API_PORT` from `.env`, so pass `--port 8001` yourself. `GET /` reports whether the DB is connected.

Frontend, from `frontend/`:

```bash
npm run dev      # Vite on :5173 (proxies /api → :8001)
npm run build    # tsc -b (type-check) then vite build
npm run lint     # eslint (flat config: typescript-eslint, react-hooks, react-refresh)
```

There are no tests in either part. Check frontend changes with `npm run build` and `npm run lint`, and backend changes through `/docs`, curl, or FastAPI's `TestClient` (`.venv/bin/python -c "from fastapi.testclient import TestClient; from app.main import app; ..."`). When the backend isn't running, the frontend's API calls throw `ApiError` and the footer shows "API: offline".

Demo logins: `ADMIN`/`admin` (Administrator), `GURU`/`guru` (Manager), `PRADEEP`/`pradeep` and `RAVI`/`ravi` (Cashier). With a DB connected, `MstUser` users are added too (password = lowercase user code, role Cashier, or Manager for `OFFICE`/`MANAGER`). See `_verify_password` in `app/routers/auth.py`.

## Backend architecture

- **Data source: in-memory store, optionally loaded from SQL Server.** `app/store.py` holds every table as a module-level dict (`ITEMS`, `CUSTOMERS`, `USERS`, `ROLES`, `SETTINGS`, `DOCS`, …), seeded with sample data. On startup `main.py` calls `db.init_db()` (`app/db.py`: pyodbc with ODBC Driver 18/17, then pymssql; settings from `backend/.env`, loaded by a small built-in parser). If it connects to the legacy `vLQMSLHOUSE` database, `db_store.load_masters()` replaces the seed data with the live data (company, items, customers, suppliers, groups, brands, UOMs, users), and `load_transactions()` loads the latest 200 sales invoices and 100 purchase bills in a background thread. If it can't connect, the app runs on the seed data. The server is on the office LAN (host and credentials only in `backend/.env`), so away from the office the backend always uses seed data. Startup waits for the pymssql login timeouts (8 s per attempt) before it falls back. `pyodbc` is only installed off macOS (see `requirements.txt`), so on a Mac the connection always goes through pymssql. Loaded invoices and bills go into `DOCS` but add no `MOVEMENTS`, and item `qoh` comes straight from the DB, so the movement-based reports only show movements made since startup plus the seeded sales history.
- **Writes never reach SQL Server.** No router calls `db.execute()`. Every create/update/delete changes only the in-memory dicts and is lost on restart. To make a write persist, the router must also call `db.execute()` (see the `db_store.py` docstring). Uploaded company files are kept on disk in `backend/uploads/company/`.
- **Stock** is changed only through `store.move_stock()`. It updates `ITEMS[code]["qoh"]` and appends to `MOVEMENTS`, which the Stock Ledger, Stock Balance and DSR reports read. A new stock-affecting transaction must go through it.
- **Documents** are kept in `store.DOCS[kind][doc_no]`. Numbers come from `next_doc_no(kind, prefix)` and look like `26/<prefix>-0000001`. Sales invoices start at 83718, and held bills have their own `hold` series. Doc numbers contain `/`, so routes use `{doc_no:path}`.
- **`store.find()`** looks a master up by code or by name, because some React screens send names.
- **Routers** (`app/routers/`): `masters.py` builds every master's CRUD at `/masters/<path>` from one `crud()` factory; `transactions.py` has a similar list/get factory plus one POST per document type; `reports.py` holds the report endpoints (`/reports/*`); `custom_reports.py` holds the Customised Reports; `auth.py` has login and the admin-only users and roles; `company_files.py` handles uploads. Pydantic models are in `app/schemas.py`, and the frontend's `masterConfigs.ts` field lists mirror them. `/settings` and `/sales-persons` live directly in `main.py`. `/reports/stock-movement` feeds the Dashboard, and `/reports/rpt-files` lists the `.rpt` files in `backend/Rpts/` (gitignored; a copy of the legacy folder, the same as `rpts_files/Rpts/`).
- **Auth:** `POST /auth/login` returns a JWT (HS256, 24 h, secret from `JWT_SECRET`/`SECRET_KEY`). `get_current_user` and `require_admin` in `auth.py` accept `Authorization: Bearer <token>` or fall back to the legacy `X-User` header. They are FastAPI dependencies, so use `Depends(require_admin)`. Calling `require_admin(x_user)` directly (as `masters.py` and `company_files.py` still do) passes the string as the bearer credential and raises an error, so `PUT /masters/company` and the company file upload/edit/delete currently return 500. `JWT_ALGORITHM` and `JWT_EXPIRATION_HOURS` in `.env` are not read either. Only the admin endpoints check rights, plus the Cashier block on deleting items in `masters.py` (403). The other endpoints trust the caller.
- CORS allows `http://localhost:5173`, `5174` and `5175` (hard-coded in `main.py`; `CORS_ORIGINS` in `.env` is not read).

## Frontend architecture (`frontend/src`)

Folder layout. Every source file starts with a header comment that says what it does:

```
App.tsx, main.tsx     app shell + screen switch, entry point
lib/                  non-UI code: api.ts (HTTP client + response types), modules.ts (module registry, canOpen),
                      styles.ts (shared Tailwind classes, money()), csv.ts, xlsExport.ts, formatters.ts, utils.ts
hooks/                useCompanyProfile.ts
data/                 prototype data still in use: stockData.ts, salesCounters.ts (invoice types + constants)
constants/images.ts   bundled images, productImage()
components/
  common/             Modal, ConfirmDialog (useConfirm), Toast + toastStore, FormField, FormErrorSummary, KeyboardShortcutsModal
  layout/             Sidebar, ModuleSearch, VlqmsLogo
  auth/               Login
  dashboard/          Dashboard
  admin/              AdminMaster (+ AdminUsersTab, PermissionMatrixTable, matrixDefs)
  masters/            MasterCrud engine + masterConfigs, Masters (simple masters), Item/Company/Supplier/Sales Counter/Price List screens
  transactions/       SalesInvoice (+ Setup, Dialogs), PurchaseBill/, ItemLinesEntry + StockTransactions, Expense, Closing Stock, Cash Denomination
  reports/            reportDefs + ApiReport + ReportShell (table reports), DailySummaryReport, StockBalanceReport, ReportsDashboard (Customised Reports)
  print/              rptRegistry, PrintFormatSelectorModal, layouts/ (one per printer format)
  ui/                 third-party components (charts, loader); don't restyle app screens with these
```

Put a new screen in the folder for its sidebar group, and put shared non-UI code in `lib/`.

- **No router library.** `src/App.tsx` stores the active module name in `activeTab` state, and `renderContent()` switches on that string. The only URL route is `/adminMaster` (history pushState and popstate). After login the app always opens the Dashboard unless the URL is `/adminMaster`. To add a screen: add its name to `MODULE_GROUPS` in `src/lib/modules.ts` (this feeds the sidebar, the module search and role rights), add search words to `MODULE_KEYWORDS`, then add a `case` in `App.tsx`.
- **Module names are keys.** The same string is used for routing, in the sidebar, and in the per-role rights in `backend/app/store.py` `ROLES`. Renaming a module means changing both.
- **Auth.** `Login` returns an `ApiUser` with `role`, `modules` and a JWT. `canOpen()` gates the screens (`'*'` = Administrator = everything). `setAuthUser(user, role, token)` stores the token in `localStorage` (`vlqms_jwt_token`), and every request sends both `Authorization: Bearer` and `X-User`. The session itself is React state, so a page reload goes back to the login screen.
- **Admin Master** (`AdminMaster.tsx`, `AdminUsersTab.tsx`, `PermissionMatrixTable.tsx`, screen list in `matrixDefs.ts`) shows Open/Add/Edit/Delete ticks per screen, but only the module names are saved (`PUT /admin/roles/{name}` with `{name, modules}`). The Add/Edit/Delete ticks are not stored. When you add a module, also add it to `MATRIX_SCREENS`.
- **API client.** `src/lib/api.ts` exports `api<T>(path, {method, body})`, `errorText`, and TypeScript shapes for backend responses. The shapes stay snake_case, as FastAPI returns them. Error messages come from FastAPI's `detail`.
- **Reusable screen engines** (these mirror the legacy generic forms):
  - `MasterCrud.tsx` with field lists in `masterConfigs.ts` → the simple masters at `/masters/<path>`. Field lists mirror `backend/app/schemas.py`, so change both together. `Masters.tsx` wires them together.
  - `reports/reportDefs.ts` (`REPORTS` map: endpoint, filters, columns) rendered by `reports/ApiReport.tsx` and `ReportShell.tsx`. `App.tsx` checks `REPORTS[activeTab]` before its switch, so a simple report only needs a new entry here.
  - `ItemLinesEntry.tsx` is the shared item-line grid used by the stock transactions (`StockTransactions.tsx`: Purchase Indent, Stock Deduction, Sales Return).
  - `print/`: `rptRegistry.ts` maps each document type to the legacy `.rpt` formats and printer types (thermal 80/58 mm, A4/A5 laser, dot-matrix). `PrintFormatSelectorModal.tsx` lets the user pick one, and `print/layouts/*` render the web version of each.
- **Sales Invoice has two steps.** `SalesInvoiceSetup` (counter, invoice no, payment type, customer) comes first, then `SalesInvoice`. `App.tsx` holds the draft state (`invoiceDraft`, `billOpen`). Saving a Sales Counter Master opens the setup form.
- **Dashboard** is `Dashboard` in `components/dashboard/Dashboard.tsx`. It reads `/reports/dsr`, `/reports/sales/daily` (last 7 days), `/reports/stock-movement` and the first company image. `ModuleSearch.tsx` (⌘K or `/`) searches module names and `MODULE_KEYWORDS`, and `KeyboardShortcutsModal.tsx` (`?`) lists the shortcuts.
- **Notifications and validation** follow `.claude/rules/popups-and-errors.md`: server results go to the snackbar (`Toast.tsx` / `toastStore.ts`), and client-side form mistakes are shown inline (`FormField.tsx`, and `InlineError`/`FormErrorSummary` in `FormErrorSummary.tsx`), never as toasts.
- Some files still hold hard-coded sample data from the prototype (`data/stockData.ts`, `PARTIES`/`DEFAULT_COUNTER` in `data/salesCounters.ts`, `transactions/PurchaseBill/purchaseBillData.ts`, `print/samplePrintData.ts`). The newer screens use the API. **Supplier Master is not wired to the API**: its Save only changes local state.
- Styling notes (Tailwind v4, palette, shared class strings in `lib/styles.ts`) are in `frontend/CLAUDE.md`.

## Workflow

- **Never commit credentials.** DB host/IP, port, user, password, the legacy encrypted password, and JWT secrets go only in `backend/.env` (gitignored). Code gets no defaults for them, and `.env.example` and the docs get placeholders only. `SvtConn.ini` is gitignored for the same reason. Before a commit, grep the diff for IPs and passwords.

- After changing a screen, endpoint or report, run the `parity-reviewer` agent.
