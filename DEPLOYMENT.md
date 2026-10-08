# Deploying e-vLQMS ERP on a shop's local server

This is the full procedure for installing the web vLQMS on a shop's own PC or server (one PC, or a server PC with two or three
other PCs opening it in a browser). Nothing needs the internet except e-mail (and the map frame in Company Master, which Google serves).

**How it runs.** One program (the FastAPI backend) listens on one port (8001). It also serves the built frontend
(`frontend/dist`), and the frontend calls `/api` on the same address, so there is no separate web server and no backend
URL to edit. It works directly on the shop's own SQL Server database (the one the desktop vLQMS uses).

> **One thing that breaks a LAN install if you skip it: build the frontend with no API address.** `frontend/.env` has
> `VITE_API_URL=http://localhost:8001` for development. If a production build contains it, every PC except the server calls
> itself on port 8001 and nothing works (step A2).
>
> **HTTPS is optional.** Other PCs can open `http://<server-ip>:8001/` directly. Over plain `http://` the browser has no
> WebCrypto, so passwords travel as typed on the shop's LAN (the owner's decision; the backend accepts this,
> `secure_fields.REQUIRE_ENCRYPTED` is off). Over `https://` or `localhost` they are encrypted in the browser. Use HTTPS
> (step B6) only if the network is not trusted.

---

## A. Build the release (on the development PC, every release)

1. Run the checks:
   ```
   cd backend
   .venv\Scripts\python -m pytest -q
   cd ..\frontend
   npm run lint
   npm run test:run
   ```
2. Build the frontend with **no** API address (PowerShell, from `frontend`):
   ```
   $env:VITE_API_URL = ""; npm run build; Remove-Item Env:VITE_API_URL
   ```
   The empty variable overrides `frontend/.env`, so the build calls `/api` on whatever address the page was opened from.
3. Check nothing stale was baked in: search `frontend\dist\assets\*.js` for `localhost:8001`. It must find nothing.
4. Copy to the server, for example into `C:\vLQMS\`:
   ```
   C:\vLQMS\backend\    app, migrations, tools, Rpts, requirements.txt, .env.example
   C:\vLQMS\frontend\dist\
   ```
   Leave out `.venv`, `node_modules`, `.env`, `backend\data\` and `backend\uploads\` (the last three belong to each server).

## B. Prepare the server PC (once)

5. **Install:**
   - Python 3.12 (tick "Add python.exe to PATH").
   - Microsoft ODBC Driver 17 or 18 for SQL Server.
   - Only if you print through Crystal Reports: the Crystal Reports 13 runtime (32-bit).
   - SQL Server is already there (the shop's database).
6. **HTTPS (optional; skip it on a trusted shop LAN).** For a small shop, use `mkcert`:
   - On the server: `mkcert -install`, then `mkcert 192.168.1.10 vlqms-server localhost` (use the server's real IP and PC
     name). It creates a certificate file and a key file.
   - On each client PC: trust mkcert's root certificate (`rootCA.pem`, shown by `mkcert -CAROOT`) once, so the browser does not warn.
   - A single PC that only uses `http://localhost:8001` can skip this step.
7. **Install the backend:**
   ```
   cd C:\vLQMS\backend
   python -m venv .venv
   .venv\Scripts\python -m pip install -r requirements.txt
   copy .env.example .env
   ```
8. **Edit `C:\vLQMS\backend\.env`:**
   - `JWT_SECRET` and `SECRET_KEY`: long random values, not the placeholder
     (`python -c "import secrets; print(secrets.token_urlsafe(48))"`).
   - `SMTP_*`: only if reports are e-mailed.
   - Crystal printing needs nothing here: the backend starts the bridge itself (step 9). Only `RPT_FOLDER` (the `.rpt` path,
     default `backend\Rpts`) can be set, if the reports are kept elsewhere.
   - Shop timings default to the Karnataka 10:00 AM to 10:30 PM; set `SHOP_OPEN_TIME` / `SHOP_CLOSE_TIME` ("HH:MM") only if the
     licence differs. Closed days come from Holiday Master.
   - Do **not** type any `DB_*` lines. The Database setup screen writes them.
9. **Print files (Crystal printing only):**
   - Copy the 7 `.rpt` files that are not in git (`fmtPI`, `fmtQTN`, `fmtSInv_GST`, `fmtSPInvPrint`, `fmtSR`, `RptDSR_8.5`,
     `UDRSalesItemwise`) into `C:\vLQMS\backend\Rpts\`.
   - Copy the built bridge `backend\tools\rptbridge\bin\RptBridge.exe` (it is built on the dev PC, see
     `backend\tools\rptbridge\README.md`; it is not in git).
10. **Test run by hand:**
    ```
    cd C:\vLQMS\backend
    .venv\Scripts\python -m uvicorn app.main:app --host 0.0.0.0 --port 8001
    ```
    For HTTPS add `--ssl-certfile <certificate file> --ssl-keyfile <key file>`. Do not use `--reload` in production.
11. **Connect the database.** Open `http://<server>:8001/` (or `http://localhost:8001/` on the server). The sign-in page shows
    **Database setup**: enter the SQL Server name, a SQL login and its password (Windows login is not supported), press Test
    connection, pick the shop's vLQMS database and press Connect. **Back the database up first**: the web app writes to the
    live desktop tables. The password is stored sealed in `.env`.

## C. Run it as a Windows service (once)

12. Download NSSM (nssm.cc). In an Administrator Command Prompt:
    ```
    nssm install vLQMS "C:\vLQMS\backend\.venv\Scripts\python.exe" "-m uvicorn app.main:app --host 0.0.0.0 --port 8001"
    nssm set vLQMS AppDirectory C:\vLQMS\backend
    nssm set vLQMS Start SERVICE_AUTO_START
    nssm start vLQMS
    ```
    For HTTPS append `--ssl-certfile ... --ssl-keyfile ...` to the arguments.
    The backend starts and supervises the Crystal bridge itself, so there is no second service. The bridge runs as the same
    Windows user as this service. To print **directly** to a network printer (e.g. `\\saturn2\POS-80`), run the service as a
    user who has that printer added: `nssm set vLQMS ObjectName ".\<windows-user>" "<password>"`, then `nssm restart vLQMS`.
    (The default LocalSystem account cannot see per-user network printers; preview and PDF work either way.)
13. Open only the app port in the firewall:
    ```
    netsh advfirewall firewall add rule name="vLQMS" dir=in action=allow protocol=TCP localport=8001
    ```
    Do not open 1433 (SQL Server) or 8765 (the bridge) to the network.
14. Give the server a fixed address: a DHCP reservation in the router, or use its PC name.

## D. Client PCs

15. Install nothing. Open `http://<server-name-or-ip>:8001/` in Chrome or Edge (`https://` if you set up step B6 and trusted the certificate). Add a
    desktop shortcut, or use the browser's "Install app".
16. Sign in with the shop's own users (their desktop password, or the lowercase user code once). If no Administrator exists, the
    page asks you to create one.
17. Company Master: fill in the registered premises address (the Shop location map follows it). Add the
    shop's holidays and dry days in Holiday Master so the Dashboard shows Store Closed on them.

## E. Smoke test (about 10 minutes)

18. Sign in, then check:
    - Dashboard and Item Master open.
    - Utilities → About shows the database name and "SQL Server connected".
    - Save one sales invoice and print it (this tests the Crystal bridge).
    - Daily Summary Report and Stock Balance Report run.
19. Reboot the server and confirm the service started by itself and the app answers.

## F. Updating later

20. On the development PC repeat A1–A4. On the server:
    ```
    nssm stop vLQMS
    (replace backend\app, backend\migrations and frontend\dist)
    nssm start vLQMS
    ```
    Never overwrite `.env`, `backend\data\` or `backend\uploads\`.

## Keep safe

- `backend\data\` holds the encryption keys (`secret.key`, `transport_key.pem`). Back it up privately; never copy it to another
  PC or into git. Losing `secret.key` means running Database setup again.
- Schedule SQL Server backups of the shop's database.
- Never commit credentials: the database login and the JWT secrets live only in `backend\.env`.

## If something goes wrong

| Symptom | Cause and fix |
|---|---|
| Every PC but the server shows "API: offline" | The build contains `localhost:8001`: rebuild with an empty `VITE_API_URL` (A2, A3). |
| Sign-in says the password could not be decrypted | The server key changed: reload the page. If it persists, run Database setup again. |
| Sign-in page shows Database setup again | `.env` has no `DB_*` lines, or `backend\data\secret.key` was lost: run Database setup again. |
| "Crystal Reports bridge can't be reached" | `RptBridge.exe` is not in `backend\tools\rptbridge\bin\`, the Crystal runtime is not installed, or a bridge was started by hand with another key (stop it; the backend starts its own) (B9). |
| Page loads but a PC cannot reach it | Firewall rule or wrong address (C13, C14). |
| A save says the database refused the change | Read the message; nothing was changed. Check SQL Server is running and the login can write to the database. |
