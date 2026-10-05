# Installing vLQMS web at a shop: the one-time database setup

The web app uses **one database: the shop's own Microsoft SQL Server**, usually on the same PC as the backend
(`localhost` or `localhost\SQLEXPRESS`), the same database the desktop vLQMS uses. It keeps everything in
`Web_` tables: a copy of every legacy table with the same columns. At go-live the old data is copied into
them once. From then on the web app reads and writes only the `Web_` tables. The old desktop tables are only
read, by that copy, and the desktop app is not used after go-live.

## What is in this folder

| File | What it is |
|---|---|
| `002_web_legacy_tables.sql` | `CREATE TABLE Web_<Name>` for every legacy table (277 on the reference database): same columns, identity, defaults, computed columns, primary keys and indexes; the `RemoveNonAlphaCharacters` function the computed columns use; the stock triggers on `Web_UtlStockList` (they keep `Web_TrnQOH`, as the desktop's triggers keep `TrnQOH`); and the web-only tables `Web_AppRow`, `Web_AppDoc`, `Web_AppMove`, `Web_AppSetting`, `Web_ToolAudit`, `Web_Setup`, `Web_SchemaVersion`. Every CREATE is guarded, so running it twice changes nothing. **Generated** by `tools/gen_web_schema.py` from the reference database: regenerate it, don't hand-edit it. Schema only: no data, no server names, no credentials. |
| `002_web_legacy_tables.rollback.sql` | Drops every `Web_` table **and its data** (web bills included). Only for a test copy, or before go-live. |
| `000_web_login.template.sql` | Optional: a separate SQL login for the web app (placeholders only). |

## At the shop

1. **Back up the database** in SSMS first (right-click the database → Tasks → Back Up…).
2. **Start the backend** (`.venv/bin/uvicorn app.main:app --port 8001`, or the installed service). With no
   database in `backend/.env` it prints a box with a **Database setup code**.
3. Open the web app. The sign-in page shows **Database setup**:
   - **Setup code**: from the backend window. Five wrong codes lock it for 15 minutes.
   - **Server**: `localhost`, or `localhost\SQLEXPRESS` for a named instance. **Port**: leave it blank for
     a named instance.
   - **SQL login** (e.g. `sa`) and password, or **Windows login**. Windows login needs Windows with the
     ODBC Driver for SQL Server. The password is encrypted in the browser and stored sealed in
     `backend/.env`.
   - **Test connection** lists the server's databases; pick the vLQMS one and press **Connect**.
   - Check what it found: tables, rows, and the space after the copy. SQL Server Express allows 10 GB per
     database, and the copy roughly doubles the data.
   - **Create tables and copy the old data**. It takes a few minutes: about 4 minutes for 3.8 million rows
     on the reference database. Keep the page open. When it's done the login form comes back.
4. Sign in with a desktop user (their desktop password, or the lowercase user code) or `ADMIN`.

The same from the command line, in `backend/` with the connection already in `.env`:

```bash
.venv/bin/python tools/setup_db.py check        # what the database has; changes nothing
.venv/bin/python tools/setup_db.py run          # create the Web_ tables and copy the old data
```

## Trial copy, then the final copy

Copy once to try the web app, then copy again on go-live day so the latest desktop data is in:

- A copy is refused once the web app has saved anything into the `Web_` tables, because it would overwrite that data.
- `tools/setup_db.py run --force` copies anyway, and clears what the web app saved (`Web_AppRow`,
  `Web_AppDoc`, `Web_AppMove`, `Web_AppSetting`). Use it only to throw a trial away.
- A copy that stopped half way (power cut, dropped connection) continues where it stopped when run again.

## Checks after the copy

- `Web_Setup` has one row per table with the source and copied row counts. Every row should say `DONE`.
- In **Tools → SQL Query Execution**, compare a few tables:
  `SELECT COUNT(*) FROM TrnHdrSINV` and `SELECT COUNT(*) FROM Web_TrnHdrSINV`.
- `Web_TrnQOH` totals match `TrnQOH`.

## Notes

- Every web change is saved in the same request. If SQL Server refuses it or the connection drops, the
  screen says "Not saved…" and nothing changes.
- Views and stored procedures are not copied; the web app doesn't use them.
- Keep `backend/data/secret.key`: it seals the database password in `.env`. If it's lost, run the setup
  again (it only reconnects; the copy is refused once there is web data).
