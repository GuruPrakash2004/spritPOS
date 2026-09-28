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
.venv/bin/python -m pip install -r requirements-dev.txt && .venv/bin/python -m pytest   # API tests
```

The port matters: `frontend/vite.config.ts` proxies `/api/*` to **:8001** (prefix stripped). uvicorn does not read `API_PORT` from `.env`, so pass `--port 8001` yourself. `GET /` reports whether the DB is connected.

Frontend, from `frontend/`:

```bash
npm run dev      # Vite on :5173 (proxies /api → :8001)
npm run build    # tsc -b (type-check) then vite build
npm run lint     # eslint (flat config: typescript-eslint, react-hooks, react-refresh)
npm test         # Vitest + React Testing Library in watch mode; npm run test:run runs once
```

The frontend has Vitest + React Testing Library (jsdom). Tests sit next to the component as `*.test.tsx` (example: `components/common/DropdownMenu.test.tsx`); `vitest.config.ts` reuses `vite.config.ts`, and `src/test/setup.ts` loads the jest-dom matchers. The backend has pytest tests in `backend/tests/` (settings in `pytest.ini`, pytest-asyncio auto mode): they call the app in-process with an httpx `AsyncClient` over `ASGITransport`, always on the seed data (the DB settings are blanked in `conftest.py`, and the startup event doesn't run), and `conftest.py` restores the in-memory store after every test and switches respx on so no test can make a real outgoing HTTP call. Doc numbers keep counting across tests, so assert on their pattern only. Check frontend changes with `npm run test:run`, `npm run build` and `npm run lint`, and backend changes with `pytest`, `/docs` or curl. When the backend isn't running, the frontend's API calls throw `ApiError` and the footer shows "API: offline".

Demo logins: `ADMIN`/`admin` (Administrator), `GURU`/`guru` (Manager), `PRADEEP`/`pradeep` and `RAVI`/`ravi` (Cashier). With a DB connected, `MstUser` users are added too (their legacy password or the lowercase user code, hashed on first login; role Cashier, or Manager for `OFFICE`/`MANAGER`). See `_verify_password` in `app/routers/auth.py`. Change the demo passwords in Admin Master before real use.

## Backend architecture

- **Data source: in-memory store, optionally loaded from SQL Server.** `app/store.py` holds every table as a module-level dict (`ITEMS`, `CUSTOMERS`, `USERS`, `ROLES`, `SETTINGS`, `DOCS`, …), seeded with sample data. On startup `main.py` calls `db.init_db()` (`app/db.py`: pyodbc with ODBC Driver 18/17, then pymssql; settings from `backend/.env`, loaded by a small built-in parser). If it connects to the legacy `vLQMSLHOUSE` database, `db_store.load_masters()` replaces the seed data with the live data (company, items, customers, suppliers, groups, brands, UOMs, users), and `load_transactions()` loads the latest 200 sales invoices and 100 purchase bills in a background thread. If it can't connect, the app runs on the seed data. The server is on the office LAN (host and credentials only in `backend/.env`), so away from the office the backend always uses seed data. Startup waits for the pymssql login timeouts (8 s per attempt) before it falls back. `pyodbc` is only installed off macOS (see `requirements.txt`), so on a Mac the connection always goes through pymssql. Loaded invoices and bills go into `DOCS` but add no `MOVEMENTS`, and item `qoh` comes straight from the DB, so the movement-based reports only show movements made since startup plus the seeded sales history.
- **One DB connection, serialised.** `db.py` shares a single connection between request threads and the background loader, so every query runs under `_db_lock` (pymssql fails with "Unknown error" on concurrent use). Keep new DB calls going through `db.fetchall`/`db.execute`, or `db.transaction()` for several statements that must commit together (it uses `?` placeholders for both drivers).
- **Writes go to local SQLite, and optionally to separate `Web_*` tables in SQL Server; the legacy tables are never written**, so the desktop app never sees web changes. `app/persistence.py` saves the app's own data to `backend/data/vlqms_state.sqlite3` (gitignored, 0600) after every successful POST/PUT/DELETE (middleware in `main.py`, which also runs writes one at a time) and lays it back over the seed/DB data at startup: documents without an `origin` (seed/DB documents carry `"origin"`), `MOVEMENTS[APP_MOVEMENTS_FROM:]` replayed onto QOH, rows marked with `store.touch(TABLE, key)` (masters, users, roles, rights, customer points), settings, cash counts, company edits and the number series (`store.Seq`). **A new write must call `store.touch()`** for any master/user/role row it changes, or the change is lost on restart. `VLQMS_STATE_DB` sets the file (empty = off, as in the tests). **SQL Server write-back** (`app/writeback.py`): a transaction endpoint calls `writeback.changed(kind, doc, action)`; when the request succeeds the write middleware turns it into an event (user, time, document, that request's stock moves) and `persistence.save()` writes it to the SQLite `web_outbox` table **in the same SQLite transaction as the state**, as PENDING, even while `DB_WRITE_ENABLED` is off. With the flag on, `sync_pending_data()` (startup, every 15 s, after writes, `POST /admin/writeback/sync`) claims rows (PENDING/FAILED → SYNCING), writes each as one SQL transaction into `Web_DocHeader`/`Web_DocLine`/`Web_StockMove`/`Web_StockBalance` (`QtyChange = QtyChange + ?`) and `Web_WriteLog`, and marks it SYNCED only after the commit; an `EventId` already in `Web_WriteLog` is marked SYNCED without writing again. Errors mark the row FAILED (retried; `POST /admin/writeback/retry`) and stop the run to keep the order; rows are never deleted. `GET /admin/writeback` shows counts and the last error; `/admin/writeback/backfill` queues chosen documents saved before the outbox existed (preview first; needs the DB connection to skip documents already in `Web_WriteLog`). Nothing is queued while the backend runs on seed data (such documents are stamped `seed_mode` and backfill refuses them), nor for bills loaded from the DB (they have `origin`). **A new document-saving endpoint must call `writeback.changed()`** (item opening stock does, with `moves_ref="OPENING"`). The tables come from `backend/migrations/` (versioned, `IF NOT EXISTS`-guarded, each with a rollback), applied only by `tools/migrate.py`, never by the app; the runbook is `backend/migrations/README.md`. Saves sent with an `Idempotency-Key` header (Sales Invoice and Purchase Bill send one per draft) are answered from `store.IDEMPOTENT` when repeated with the same body (a different body gets 409), so a double-click makes one bill. Uploaded company files are kept on disk in `backend/uploads/company/`.
- **Stock** is changed only through `store.move_stock()`. It updates `ITEMS[code]["qoh"]` and appends to `MOVEMENTS`, which the Stock Ledger, Stock Balance and DSR reports read. A new stock-affecting transaction must go through it. Anything that takes stock out checks the total per item across all lines first (`transactions._check_stock`), so stock can't go negative through the API. Item QOH is opening stock: set when an item is created (logged as an `opening` movement) and ignored on later edits. Masters in use (items on bills, groups with items, counters with bills, …) can't be deleted or re-coded (`masters.IN_USE`).
- **Documents** are kept in `store.DOCS[kind][doc_no]`. Numbers come from `next_doc_no(kind, prefix)` and look like `26/<prefix>-0000001`. Sales invoices start at 83718, and held bills have their own `hold` series. Doc numbers contain `/`, so routes use `{doc_no:path}`.
- **`store.find()`** looks a master up by code or by name, because some React screens send names.
- **Routers** (`app/routers/`): `masters.py` builds every master's CRUD at `/masters/<path>` from one `crud()` factory; `transactions.py` has a similar list/get factory plus one POST per document type; `reports.py` holds the report endpoints (`/reports/*`); `custom_reports.py` holds the Customised Reports (and `POST /reports/custom/email`, which builds the .xls on the server and sends it through `app/mailer.py`, at most 10 per user per hour; SMTP settings only in `.env`); `auth.py` has login and the admin-only users and roles; `company_files.py` handles uploads. Pydantic models are in `app/schemas.py`, and the frontend's `masterConfigs.ts` field lists mirror them. `/settings` and `/sales-persons` live directly in `main.py`. `/reports/stock-movement` and `/reports/restock` feed the Dashboard. Restock is real time: it ranks items by sales speed over the last 15 days up to today, and with a DB connected it reads sales (`TrnHdrSINV`/`TrnDtlSINV`) and stock (`TrnQOH.CLBal`) live via `db_store.live_sales_and_stock()` (cached 20 s), adding this app's own movements since startup (`store.DB_BASE_MOVEMENTS`). Only items that actually sell can need restock. and `/reports/rpt-files` lists the `.rpt` files in `backend/Rpts/` (gitignored; a copy of the legacy folder, the same as `rpts_files/Rpts/`).
- **Auth:** `POST /auth/login` returns a JWT (HS256, 24 h, secret from `JWT_SECRET`/`SECRET_KEY`). Every endpoint needs `Authorization: Bearer <token>`: `main.py` applies `auth.require_login` to the whole app, and only `auth.PUBLIC_ROUTES` are open (health `/`, login, `GET /masters/company` for the sign-in page, and company file content, which `<img>` tags load without a token). A new public endpoint must be added there. The `X-User` header is no longer trusted. For the current user or an admin-only endpoint, use the dependencies `Depends(get_current_user)` / `Depends(require_admin)`; never call them directly. Every write endpoint also checks the caller's role with `Depends(require_module("<module name>", ...))` (403 otherwise), using the same module names as `store.ROLES` and the sidebar; a new write endpoint needs one. Reads need only a login, except Profit Margin and Customised Reports. Passwords are salted PBKDF2-SHA256 hashes (`app/passwords.py`, 600k rounds; `PASSWORD_HASH_ITERATIONS` is lowered only in the tests); `MstUser` users are hashed on their first login. New passwords need 8+ characters mixing letters with numbers or symbols. Five wrong passwords lock a user name for 15 minutes (429). Tokens carry a password version (`pwv`), so a password change or deactivation signs the user out. `JWT_ALGORITHM` and `JWT_EXPIRATION_HOURS` in `.env` are not read.
- CORS allows `http://localhost:5173`, `5174` and `5175` (hard-coded in `main.py`; `CORS_ORIGINS` in `.env` is not read).

## Frontend architecture (`frontend/src`)

Folder layout. Every source file starts with a header comment that says what it does:

```
App.tsx, main.tsx     app shell + screen switch, entry point
lib/                  non-UI code: api.ts (HTTP client + response types), modules.ts (module registry, canOpen),
                      styles.ts (shared Tailwind classes, money()), csv.ts, xlsExport.ts, formatters.ts, utils.ts
hooks/                useCompanyProfile.ts
data/                 stockData.ts (live item catalogue for Sales Invoice / Purchase Bill), salesCounters.ts (invoice types + constants)
constants/images.ts   bundled images, productImage()
components/
  common/             Modal, ConfirmDialog (useConfirm), DropdownMenu (on-screen pop-up menus), Toast + toastStore, FormField, FormErrorSummary, KeyboardShortcutsModal
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
- **Auth.** `Login` returns an `ApiUser` with `role`, `modules` and a JWT. `canOpen()` gates the screens (`'*'` = Administrator = everything). `setAuthUser(user, role, token)` stores the token in `sessionStorage` (`vlqms_jwt_token`: survives a refresh, not closing the tab), and every request sends `Authorization: Bearer <token>`. On load `App.tsx` restores the session from `/auth/me` (and the last screen), re-checks it every minute and on window focus, and any 401 returns to the sign-in page with the reason. `setAuthRights(modules, rights)` feeds `canDo(module, 'add'|'edit'|'delete')` and `canSeeCost()`, which screens use to hide buttons and cost fields (the backend enforces the same: `require_right`, `hides_cost`). Users change their own password from the header key button (`POST /auth/change-password`).
- **Admin Master** (`AdminMaster.tsx`, `AdminUsersTab.tsx`, `PermissionMatrixTable.tsx`, screen list in `matrixDefs.ts`) shows Open/Add/Edit/Delete ticks per screen, but only the module names are saved (`PUT /admin/roles/{name}` with `{name, modules}`). The Add/Edit/Delete ticks are not stored. When you add a module, also add it to `MATRIX_SCREENS`.
- **API client.** `src/lib/api.ts` exports `api<T>(path, {method, body})`, `errorText`, and TypeScript shapes for backend responses. The shapes stay snake_case, as FastAPI returns them. Error messages come from FastAPI's `detail`.
- **Reusable screen engines** (these mirror the legacy generic forms):
  - `MasterCrud.tsx` with field lists in `masterConfigs.ts` → the simple masters at `/masters/<path>`. Field lists mirror `backend/app/schemas.py`, so change both together. `Masters.tsx` wires them together.
  - `reports/reportDefs.ts` (`REPORTS` map: endpoint, filters, columns) rendered by `reports/ApiReport.tsx` and `ReportShell.tsx`. `App.tsx` checks `REPORTS[activeTab]` before its switch, so a simple report only needs a new entry here.
  - `ItemLinesEntry.tsx` is the shared item-line grid used by the stock transactions (`StockTransactions.tsx`: Purchase Indent, Stock Deduction, Sales Return).
  - `print/`: `rptRegistry.ts` maps each document type to the legacy `.rpt` formats and printer types (thermal 80/58 mm, A4/A5 laser, dot-matrix). `PrintFormatSelectorModal.tsx` lets the user pick one, and `print/layouts/*` render the web version of each. **The layouts are not derived from the `.rpt` files** (`rpts_files/RPT_LAYOUT_AUDIT.md`): don't describe them as matching the legacy prints, and don't add content that isn't in the `.rpt`. `backend/tools/rpt_inspect.py` decodes the readable (older-format) `.rpt` files.
- **Sales Invoice has two steps.** `SalesInvoiceSetup` (counter, invoice no, payment type, customer) comes first, then `SalesInvoice`. `App.tsx` holds the draft state (`invoiceDraft`, `billOpen`). Saving a Sales Counter Master opens the setup form.
- **Dashboard** is `Dashboard` in `components/dashboard/Dashboard.tsx`. It reads `/reports/dsr`, `/reports/sales/daily` (last 7 days), `/reports/stock-movement` and the first company image. `ModuleSearch.tsx` (⌘K or `/`) searches module names and `MODULE_KEYWORDS`, and `KeyboardShortcutsModal.tsx` (`?`) lists the shortcuts.
- **Notifications and validation** follow `.claude/rules/popups-and-errors.md`: server results go to the snackbar (`Toast.tsx` / `toastStore.ts`), and client-side form mistakes are shown inline (`FormField.tsx`, and `InlineError`/`FormErrorSummary` in `FormErrorSummary.tsx`), never as toasts.
- Sales Invoice and Purchase Bill take items from `data/stockData.ts` (`useStockItems()`, loaded from `/masters/items`). A bill line holds the item's index in `STOCK_ITEMS`, so that array is only merged in place, never reordered. Call `loadStockItems()` after a save to refresh QOH. A few prototype constants remain (`PARTIES`/`DEFAULT_COUNTER` in `data/salesCounters.ts`, `print/samplePrintData.ts` for print previews).
- Styling notes (Tailwind v4, palette, shared class strings in `lib/styles.ts`) are in `frontend/CLAUDE.md`.

## Workflow

- **Never commit credentials.** DB host/IP, port, user, password, the legacy encrypted password, and JWT secrets go only in `backend/.env` (gitignored). Code gets no defaults for them, and `.env.example` and the docs get placeholders only. `SvtConn.ini` is gitignored for the same reason. Before a commit, grep the diff for IPs and passwords.

- After changing a screen, endpoint or report, run the `parity-reviewer` agent.
