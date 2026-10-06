# Installing vLQMS web at a shop: the one-time database setup

The web app uses **one database: the shop's own Microsoft SQL Server**, usually on the same PC as the backend
(`localhost` or `localhost\SQLEXPRESS`), the same database the desktop vLQMS uses. It works on the desktop's
own tables directly (`MstITEM`, `TrnHdrSINV`, `UtlStockList`, ...): **nothing is copied**, so the desktop app
and the web app always see the same data. The setup only creates five small tables of the web app's own.

## What is in this folder

| File | What it is |
|---|---|
| `web_app_tables.sql` | `CREATE TABLE` for the web app's own tables: `Web_AppRow`, `Web_AppDoc`, `Web_AppMove`, `Web_AppSetting`, `Web_ToolAudit` (password hashes, roles and rights, whole documents, the web's stock ledger, the Tools audit: what the desktop tables have no column for). Every CREATE is guarded, so running it twice changes nothing. No desktop table is created, altered or emptied. Holds no data, server names or credentials. |
| `000_web_login.template.sql` | Optional: a separate SQL login for the web app (placeholders only). |

## At the shop

1. **Back up the database** in SSMS first (right-click the database → Tasks → Back Up…). The web app writes
   to the live desktop tables, so a backup is the way back.
2. **Start the backend** (`.venv/bin/uvicorn app.main:app --port 8001`, or the installed service).
3. Open the web app. With no database in `backend/.env`, the sign-in page shows **Database setup**:
   - **Server**: `localhost`, or `localhost\SQLEXPRESS` for a named instance. **Port**: leave it blank for
     a named instance.
   - **SQL login** (e.g. `sa`) and password. The password is encrypted in the browser and stored sealed in
     `backend/.env`.
   - **Test connection** lists the server's databases; pick the vLQMS one and press **Connect**. The app
     checks it is a vLQMS database, creates its five tables (this login needs the right to create tables
     for that one moment) and loads the shop's data. The login form comes back at once.
4. Sign in with a desktop user (their desktop password, or the lowercase user code) or `ADMIN`.

The same from the command line, in `backend/` with the connection already in `.env`:

```bash
.venv/bin/python tools/setup_db.py check        # what the database is; changes nothing
.venv/bin/python tools/setup_db.py prepare      # create the web app's own tables
```

## Notes

- Every web change is saved in the same request. If SQL Server refuses it or the connection drops, the
  screen says "Not saved…" and nothing changes.
- The web app never changes a desktop user's `LoginPwd`: the desktop keeps signing its users in. A password
  set in the web app is kept as a hash in `Web_AppRow`.
- Stock: a bill's stock rows go into `UtlStockList`, and the desktop's own triggers keep `TrnQOH`. The setup
  refuses a database without that trigger.
- Keep `backend/data/secret.key`: it seals the database password in `.env`. If it's lost, run the setup
  again (it only reconnects).
