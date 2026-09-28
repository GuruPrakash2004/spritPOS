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

SQLite (`data/vlqms_state.sqlite3`) is the durable record of unsynchronised web transactions. The `Web_*`
tables in SQL Server are where they are copied to.

```
web bill ──► SQLite: state + web_outbox row (PENDING), one SQLite transaction
                         │   (queued even with DB_WRITE_ENABLED=false)
                         ▼   sync_pending_data()  (flag on: at startup, every 15 s, after each bill,
                         │                          POST /admin/writeback/sync)
              claim: PENDING/FAILED ──► SYNCING  (one atomic UPDATE: only one worker wins)
                         ▼
              SQL Server: ONE transaction = Web_DocHeader + Web_DocLine + Web_StockMove
                          + Web_StockBalance (QtyChange = QtyChange + ?) + Web_WriteLog
                         │
              ┌──────────┴───────────┐
          committed, or EventId      error: SQL rolls back
          already in Web_WriteLog        │
              ▼                          ▼
           SYNCED                  FAILED + last_sync_error, attempts+1
                                   (sending stops here to keep the order; retried later)
```

1. A transaction endpoint saves the document in memory and calls `writeback.changed()`.
2. If the request succeeds, the write middleware (`app/main.py`) turns it into an event: a unique id, the
   user, the time, the document and that request's stock moves. `persistence.save()` writes the app state
   **and** the event's `web_outbox` row in **one SQLite transaction**, so a saved bill always has its row.
   A failed request queues nothing.
3. The row is **PENDING**. This happens whether or not `DB_WRITE_ENABLED` is on.
4. When the flag is on, `sync_pending_data()` sends PENDING and FAILED rows oldest first. Each row is claimed
   (PENDING/FAILED to SYNCING), written as **one SQL transaction**, and marked **SYNCED only after the commit
   returns**. The event id is the key in `Web_WriteLog`: an event already there is not written again, just
   marked SYNCED. That covers a crash after the SQL commit but before SQLite was updated.
5. On an error the SQL transaction rolls back, the row becomes **FAILED** with the error and its attempt
   count, and sending stops, because later documents may depend on it (a cancel after its sale). It is
   retried on the next run, or at once with `POST /admin/writeback/retry`. Rows are never deleted.
6. SQL Server unreachable when a run starts, or `Web_` tables missing: no row is touched, and the error shows
   on `GET /admin/writeback`. If the connection drops in the middle of a row, that row is rolled back in SQL
   Server and marked FAILED (attempts + 1). It is sent again on the next run.
7. Rows a crash left SYNCING go back to PENDING at startup, or after 5 minutes. That is safe because of point 4.
8. Not queued: bills made while the backend runs on **seed data** (it started without SQL Server), and bills
   **loaded from the desktop database**, even when cancelled on the web. The `Web_` tables hold web
   documents only, and the legacy bill stays as it is in `TrnHdrSINV`.
9. Queued documents: sales invoices (save, release, cancel), purchase indents and bills, sales returns,
   expenses, stock deductions, closing stock entries, cash counts, and the opening stock of items created on
   the web. Not queued: masters, users, settings and other SQLite-only data.

**Bills saved before the outbox existed** have no row. An Administrator backfills them:
1. `GET /admin/writeback/backfill` lists them. This needs the SQL Server connection: documents already in
   `Web_WriteLog`, sent by the earlier version that kept no outbox row, are left out, because sending them
   again would count their stock twice. A document has `blocked` set when it must not be sent: it was made
   while the backend ran on seed data (those carry `seed_mode`), or its items aren't in the loaded masters.
2. `POST /admin/writeback/backfill` with `{"documents": [{"kind": "sales_invoice", "doc_no": "…"}, …]}`
   queues exactly the documents you picked from that list. It refuses blocked documents and documents not in
   the list, including ones already queued.

**No new SQL Server migration** is needed for this: `Web_WriteLog.EventId` (primary key) is the
idempotency key, and the outbox table lives in SQLite, created by the app itself (`CREATE TABLE IF NOT EXISTS`).

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
- [x] SQL Server unreachable: nothing is lost. Rows stay PENDING (FAILED if the connection dropped during a row), and the status endpoint shows the error.
- [x] Concurrent syncs: one run at a time per process, and each row is claimed with one atomic UPDATE, so two workers can't send the same row.
- [x] Crash after the SQL commit: the row is left SYNCING, goes back to PENDING, and the next run finds its EventId and marks it SYNCED without writing it again.

### 5. Testing
- [x] `tests/test_writeback.py` runs against SQLite standing in for SQL Server. It covers:
  - flag off leaves bills PENDING, and turning it on syncs them;
  - a failure rolls back and marks the row FAILED, and an unreachable server changes no rows;
  - a double sync, a crash after the commit, many rows in order, and concurrent syncs and claims;
  - bill and row saved atomically, the upgrade of an old queue, seed mode and backfill;
  - the admin endpoints, the migration checks and double submit.
- [ ] **Office:** run `tests/test_writeback_sqlserver.py` against the **test copy**. It refuses the live database name.
  ```bash
  TEST_DB_HOST=… TEST_DB_NAME=vLQMSLHOUSE_TEST TEST_DB_USER=… TEST_DB_PASSWORD=… \
    .venv/bin/python -m pytest tests/test_writeback_sqlserver.py -v
  ```
- [ ] **Office:** point `backend/.env` at the test copy with `DB_WRITE_ENABLED=false`, make two bills, and check that `GET /admin/writeback` shows `pending: 2`. Set the flag to `true` and restart: the startup sync should show `synced: 2`. Then, with the flag on, bill from two browsers at once, pull the network cable during a save, and double-click Save. Check `Web_DocHeader`, `Web_StockBalance` and `GET /admin/writeback` (see "Checking the data" below).

### 6. Going live
- [ ] Take a fresh backup the same day.
- [ ] Run `.venv/bin/python tools/migrate.py up` against live (it asks you to type the database name).
- [x] Feature flag `DB_WRITE_ENABLED`, off by default. Set it to `false` and restart to stop writing at once; new bills keep being queued as PENDING in SQLite and are sent when it is turned back on.
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

`down 1` deletes what was written to SQL Server, but the SQLite rows still say SYNCED. To send everything
again after re-creating the tables, set them back (with the backend stopped):
`sqlite3 data/vlqms_state.sqlite3 "UPDATE web_outbox SET sync_status='PENDING' WHERE sync_status='SYNCED'"`.

### Syncing (Administrator token from `POST /auth/login`)

```bash
TOKEN=...   # {"access_token": ...} from logging in as an Administrator
curl -s localhost:8001/admin/writeback -H "Authorization: Bearer $TOKEN"                 # counts, last error
curl -s localhost:8001/admin/writeback/outbox?status=FAILED -H "Authorization: Bearer $TOKEN"
curl -s -X POST localhost:8001/admin/writeback/sync -H "Authorization: Bearer $TOKEN"    # send now
curl -s -X POST localhost:8001/admin/writeback/retry -H "Authorization: Bearer $TOKEN" \
     -H 'Content-Type: application/json' -d '{}'                                          # all FAILED again
curl -s localhost:8001/admin/writeback/backfill -H "Authorization: Bearer $TOKEN"        # preview old bills
curl -s -X POST localhost:8001/admin/writeback/backfill -H "Authorization: Bearer $TOKEN" \
     -H 'Content-Type: application/json' -d '{"documents":[{"kind":"sales_invoice","doc_no":"26/c10-0083718"}]}'  # queue the ones you picked
```

The same endpoints are in Swagger at `http://localhost:8001/docs` (Admin Master).

### Checking the data

In SQLite (local status):
```bash
sqlite3 data/vlqms_state.sqlite3 "SELECT sync_status, COUNT(*) FROM web_outbox GROUP BY 1"
sqlite3 data/vlqms_state.sqlite3 "SELECT transaction_type, transaction_id, sync_status, sync_attempts, last_sync_error FROM web_outbox ORDER BY seq DESC LIMIT 20"
```

In SQL Server (SSMS, on the test copy first):
```sql
SELECT DocKind, DocNo, Status, NetAmount, CreatedBy, CreatedAt FROM dbo.Web_DocHeader ORDER BY CreatedAt DESC;
SELECT EventId, DocKind, DocNo, Action, UserCode, AppliedAt FROM dbo.Web_WriteLog ORDER BY AppliedAt DESC;
SELECT ItemCode, QtyChange FROM dbo.Web_StockBalance ORDER BY ItemCode;
-- no document saved twice (must return no rows; a cash count re-saved for the same day is the one legitimate repeat,
-- so denominations are left out):
SELECT DocKind, DocNo, COUNT(*) AS times
FROM dbo.Web_WriteLog
WHERE Action IN ('save', 'release', 'backfill') AND DocKind <> 'denomination'
GROUP BY DocKind, DocNo HAVING COUNT(*) > 1;
```
Every SYNCED row's `id` in SQLite is an `EventId` in `Web_WriteLog`.
