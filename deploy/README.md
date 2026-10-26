# One-click install / update (Windows)

1. **Make the package** (development PC): double-click `Make-Package.cmd`. It builds the frontend with no API address if needed, refuses a build with development URLs in it, and writes `deploy\out\vLQMS-<date>.zip` (no `.env`, `.venv`, `data\`, keys).
2. **Copy** the zip to the server and **extract** it to a *local* disk, for example `C:\vLQMS` (not a network or mapped drive).
3. **Run** `C:\vLQMS\deploy\Install-vLQMS.cmd`, accept the Windows prompt, choose **1 Install / Repair**. It installs what is missing (Python 3.12 for all users, NSSM, ODBC driver), builds the venv, writes `backend\.env`, creates the `vLQMS` service and firewall rule, starts it and checks it.
4. **Open** one of the addresses it prints. On the first visit finish **Database setup** on the sign-in page (back the shop database up first).

Update later: extract the new zip over the same folder (never overwrites `.env` or `data\`), run the installer, choose **2 Update**.
Also: **3 Status**, **4 Uninstall** (keeps files, `.env`, logs). Re-running Install repairs a broken service, venv, firewall rule or port.

Unattended: `Install-vLQMS.cmd -Mode Install -AppPath C:\vLQMS -Port 8001 -Unattended` (exit code 0 = healthy, 1 = failed; add `-DryRun` to only see what it would do; optional `-DbServer -DbName -DbUser` with `VLQMS_DB_PASSWORD` set checks the SQL login first; `-Offline` uses `deploy\installers` and `deploy\wheels`; `-WithAiModel` also installs torch/chronos). Logs: `backend\logs\install-*.log` and `service.log`.
