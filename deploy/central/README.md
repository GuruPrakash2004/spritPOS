# vLQMS Central: hosting and adding shops

vLQMS works the way the desktop app does:

- **Each shop bills on its own PC.** Its own backend runs there, next to its own SQL Server, so billing keeps working when the internet is down.
- **Each shop's backend sends its data to the central site** every few minutes over HTTPS (Utilities → Web Sync, `backend/app/websync.py`). It sends the day's summary (the desktop's DSR tables, fast-moving and minimum-stock lists, purchase indents) and its current stock.
- **The central site** (`backend/app/central`, page `frontend/central.html`) keeps every shop's data and shows the owner all shops: the day summary, stock across shops, indents and sync status.

```
 shop PC (each shop)                                     hosted server
 ┌────────────────────────────┐   HTTPS, sync key   ┌──────────────────────────────┐
 │ backend app.main :8001     │ ──────────────────► │ Caddy :443 → api :8002       │
 │  + frontend/dist (served)  │   POST /api/sync    │ central SQL Server           │
 │ local SQL Server (Web_*)   │                     │ owners: https://<domain>/    │
 └────────────────────────────┘                     └──────────────────────────────┘
```

Shops only make outbound HTTPS calls. No port needs to be opened at a shop, and no VPN is needed.

## 1. The hosted server (once)

You need:

- a Linux VM with Docker;
- a domain name pointing at the VM (for example `central.example.com`);
- ports 80 and 443 open.

1. Copy the repository to the server. In `deploy/central/`, run `cp central.env.example central.env` and fill in `central.env`. Never commit that file.
   - `CENTRAL_DOMAIN`: the domain.
   - `CENTRAL_JWT_SECRET`: a long random value.
   - `CENTRAL_DB_*`: the central SQL Server. Use Azure SQL or another SQL Server. Or use the bundled one: `--profile db`, `CENTRAL_DB_HOST=sqlserver`, and set `MSSQL_SA_PASSWORD`.
2. Create the database and a login for the central site on that SQL Server (as `sa`, in any SQL client):
   ```sql
   CREATE DATABASE vLQMSCentral;
   CREATE LOGIN vlqms_central WITH PASSWORD = '<a strong password>';
   USE vLQMSCentral; CREATE USER vlqms_central FOR LOGIN vlqms_central; ALTER ROLE db_owner ADD MEMBER vlqms_central;
   ```
   Put that login in `CENTRAL_DB_USER` and `CENTRAL_DB_PASSWORD`.
3. Start the central site:
   ```bash
   docker compose --env-file central.env up -d --build          # add --profile db for the bundled SQL Server
   ```
   Caddy gets the HTTPS certificate by itself.
4. Create the tables and the first administrator:
   ```bash
   docker compose --env-file central.env run --rm api python tools/central.py init-db
   docker compose --env-file central.env run --rm api python tools/central.py add-user ADMIN --role Admin
   ```
   The temporary password is printed once. Sign in at `https://<domain>/` and choose your own password.

**Back up:**

- the central database;
- the `central-keys` volume. It holds the key that browsers encrypt passwords with, and `secret.key`, which seals `CENTRAL_DB_PASSWORD_ENC` if you use it. If the volume is lost, people sign in again, and a sealed database password must be set again (a plain `CENTRAL_DB_PASSWORD` is unaffected).

## 2. Adding a shop

On the server:

```bash
docker compose --env-file central.env run --rm api python tools/central.py add-client SHOP1 "Lakshmi Wines"
```

It prints `WEB_SYNC_KEY=...` **once**. Only its hash is stored, so if the key is lost, run `rotate-key SHOP1` to make a new one.

On the shop's PC, in `backend/.env`:

```
WEB_SYNC_URL=https://central.example.com/api
WEB_SYNC_KEY=<the key>
WEB_SYNC_MINUTES=10
```

Then restart the shop's backend. Utilities → Web Sync shows when it last sent, and **Sync now** sends a chosen day.

**What Web Sync sends:**

- **Only the shop's own data.** It sends nothing while the backend runs on sample data.
- **Every round:** today, and every day since the last successful send (at most 31 days).
- **After an outage:** the missed days are sent at the next round.

**Owners:**

- `add-user RAVI` creates an owner and prints a temporary password.
- `grant RAVI SHOP1` lets that owner see a shop. The Admin role sees every shop.
- `clients` and `users` list what exists.
- `disable SHOP1` stops a shop's pushes.
- `deactivate-user RAVI` signs that owner out everywhere.

## 3. A shop's PC

The shop's backend can serve the app itself, so the PC needs no Node.js:

1. Build the frontend once (`cd frontend && npm ci && npm run build`), or copy a built `frontend/dist`.
2. Start the backend (`.venv/bin/python -m uvicorn app.main:app --host 0.0.0.0 --port 8001` from `backend/`).
3. Open `http://<that PC>:8001/` on the counters.

The API also answers under `/api`. Set `SERVE_FRONTEND=0` to turn the page serving off.

Signing in sends passwords encrypted in the browser, which needs HTTPS or `localhost`. Other counter PCs on the shop's network must reach the backend over HTTPS, for example through a local reverse proxy with a certificate.

## Trying it on one machine

```bash
cd backend && CENTRAL_DEMO=1 .venv/bin/python -m uvicorn app.central.main:app --port 8002   # in memory
cd frontend && npm run dev:central                                                         # page on :5180
```

With no `CENTRAL_DB_HOST`, `CENTRAL_DEMO=1` creates a shop `DEMO` and an administrator `ADMIN`, and prints that shop's sync key and the temporary password. Nothing is kept after a restart.

Point a shop backend at it by putting these in `backend/.env`, then restart that backend:

```
WEB_SYNC_URL=http://localhost:8002
WEB_SYNC_KEY=<the printed key>
```

Plain `http` is allowed only for `localhost`.
