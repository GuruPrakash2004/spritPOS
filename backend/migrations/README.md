# SQL Server write-back: runbook

The web app can copy the documents and stock changes it makes into SQL Server. It is **off by default**
(`DB_WRITE_ENABLED=false`). This page is the safety checklist for turning it on, marked with what the code
already does and what a person must do at the office.

## Approach (decided)

**Separate `Web_*` tables.** The web app writes only to `Web_DocHeader`, `Web_DocLine`, `Web_StockMove`,
`Web_StockBalance`, `Web_WriteLog` and `Web_SchemaVersion`. It never creates, alters or writes a legacy table
(`TrnHdrSINV`, `TrnDtlSINV`, `TrnQOH`, `MstItem`, …).

What that means:
- The desktop app keeps working exactly as before. It does **not** see web bills or web stock changes.
- `Web_StockBalance.QtyChange` is the net web stock change per item. To reconcile, compare
  `TrnQOH.CLBal + Web_StockBalance.QtyChange` with the web app's stock.
- Web bill numbers come from the web app's own series. Storage can't clash, since the numbers live in
  separate tables. If a web sales invoice number is already used by a desktop bill in `TrnHdrSINV`,
  `Web_DocHeader.LegacyNoClash` is set to 1 and a warning is logged. Give web counters their own prefix to avoid this.

Writing into the legacy tables (so the desktop app sees web bills) is a separate, riskier project. It needs
the desktop app's insert logic studied on real bills first, and the written acceptance of the risk.

## How it works

1. A transaction endpoint saves the document in memory and calls `writeback.changed()`.
2. If the request succeeds, the write middleware (`app/main.py`) queues an event in `store.OUTBOX`. The event
   holds a unique id, the user, the time, the document and that request's stock moves. The queue is saved in
   `data/vlqms_state.sqlite3` with the rest of the app state. A failed request queues nothing.
3. A background thread sends events in order. Each event is **one SQL transaction**: header, lines, stock
   moves, `Web_StockBalance` (`QtyChange = QtyChange + ?`) and the `Web_WriteLog` row commit together or roll
   back together. An event already in `Web_WriteLog` is skipped, so a retry never writes twice.
4. On any error (server down, network drop, missing tables) the event stays queued, the error is logged, and
   the thread retries every 15 s, reconnecting if the connection dropped. Check the state with
   `GET /admin/writeback` (Administrator). `POST /admin/writeback/flush` sends the queue now.
5. If the backend started **without** SQL Server (seed data), nothing is queued, because those bills use sample
   items. The SQLite state remains the app's own record either way.
6. Documents are sales invoices (save, release, cancel), purchase indents and bills, sales returns, expenses,
   stock deductions, closing stock entries, cash counts and the opening stock of items created on the web.
   Bills **loaded from the desktop database** are not written, even when cancelled on the web: the `Web_`
   tables hold web documents only, and the legacy bill itself stays as it is in `TrnHdrSINV`.

Double submits: Sales Invoice and Purchase Bill send an `Idempotency-Key` per draft, renewed for each new bill.
A repeated save with the same key and the same content gets the first reply back and makes no second bill.
The same key with *different* content (the bill was edited after a lost reply) gets 409 "already saved", so
the user checks the last bill instead of saving blind. Replays are only given to a token that is still valid.

## Checklist

### 1. Before any code
- [ ] **Office:** take a full backup of vLQMSLHOUSE and restore it onto a test server (for example `vLQMSLHOUSE_TEST`).
- [x] Approach chosen and written down: separate `Web_*` tables (above).
- [x] Legacy tables touched: **none written**. Read only: `TrnHdrSINV.SINVNO` (number clash check), plus the existing loaders' reads.
- [x] Desktop app `SELECT *` / column-less `INSERT` / stored procedures: not affected, because no legacy table gets a new column.
- [x] Same bill number at the same time: web numbers come from the web series under the write lock, and clashes with desktop numbers are flagged (`LegacyNoClash`).

### 2. Database access
- [ ] **DBA:** create a separate login from `000_web_login.template.sql` (not `sa`, not the desktop login): SELECT on dbo, and INSERT/UPDATE/DELETE only on the `Web_` tables.
- [ ] **DBA:** grant CREATE TABLE only while running the migration, then revoke it (steps 3 and 5 of the template). Or use a separate setup login through `MIGRATE_DB_USER`/`MIGRATE_DB_PASSWORD`.
- [x] Host, user and password only in `backend/.env`. The code has no defaults, and docs and templates hold placeholders.
- [~] Encrypted connection: pyodbc connects with `Encrypt=yes` (with `TrustServerCertificate=yes`, so the certificate isn't verified). With pymssql (always used on macOS), encryption depends on its FreeTDS settings. To be sure, **office:** turn on *Force Encryption* on the SQL Server.
- [ ] **Office:** keep port 1433 reachable only from the office LAN or VPN.

### 3. Creating tables
- [x] One versioned migration, `001_web_tables.sql`, applied only by `tools/migrate.py`. The app never creates tables; it only checks `Web_SchemaVersion`.
- [x] Safe to run twice (`IF NOT EXISTS` on every CREATE; a test checks this).
- [x] No new columns on legacy tables. Nothing renamed, dropped or retyped.
- [x] Rollback script: `001_web_tables.rollback.sql` (`tools/migrate.py down 1`).

### 4. Writing data
- [x] Parameterised SQL only (`?` values through `db.transaction()`; a test sends a SQL-injection string).
- [x] One transaction per document: header, lines and stock commit or roll back together (tested with a failure mid-save).
- [x] Shared connection under `_db_lock`.
- [x] Relative stock update, `QtyChange = QtyChange + ?`.
- [x] Server-side checks before anything is queued: item exists, qty > 0, stock, role rights (`require_right`).
- [x] Idempotent: `EventId` in `Web_WriteLog`, plus the `Idempotency-Key` for double-clicks.
- [x] Every write logged (who, when, document number). Passwords and tokens are never logged or stored in events.
- [x] SQL Server unreachable: the event stays queued (and is saved to SQLite), nothing is lost, and the status endpoint shows the error.

### 5. Testing
- [x] `tests/test_writeback.py` covers commit, rollback, retry, ordering, cancel, the seed-data guard, the migration checks and double submit. It runs against SQLite standing in for SQL Server.
- [ ] **Office:** run `tests/test_writeback_sqlserver.py` against the **test copy**. It refuses the live database name.
  ```bash
  TEST_DB_HOST=… TEST_DB_NAME=vLQMSLHOUSE_TEST TEST_DB_USER=… TEST_DB_PASSWORD=… \
    .venv/bin/python -m pytest tests/test_writeback_sqlserver.py -v
  ```
- [ ] **Office:** point `backend/.env` at the test copy with `DB_WRITE_ENABLED=true`. Bill from two browsers at once, pull the network cable during a save, and double-click Save. Then check `Web_DocHeader`, `Web_StockBalance` and `GET /admin/writeback`.

### 6. Going live
- [ ] Take a fresh backup the same day.
- [ ] Run `.venv/bin/python tools/migrate.py up` against live (it asks you to type the database name).
- [x] Feature flag `DB_WRITE_ENABLED`, off by default. Set it to `false` and restart to stop writing at once; queued events wait safely.
- [ ] Turn it on out of hours, with one counter first.
- [ ] For the first week, compare web totals with `Web_DocHeader` and `Web_StockBalance` daily.
- [x] Rollback script ready: `tools/migrate.py down 1` (drops only the `Web_` tables).

### 7. Ongoing
- [ ] Schedule automatic backups and test a restore now and then.
- [ ] Review the web login's rights, and revoke CREATE TABLE if it is still granted.
- [ ] Change the demo passwords (ADMIN/admin and the others) in Admin Master.
- [ ] Before each commit, check the diff for IP addresses and passwords.

## Commands

```bash
cd backend
.venv/bin/python tools/migrate.py status      # which migrations are applied
.venv/bin/python tools/migrate.py up          # create the Web_ tables (safe to repeat)
.venv/bin/python tools/migrate.py down 1      # remove them and their data
```
