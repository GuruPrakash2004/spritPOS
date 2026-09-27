---
name: run-app
description: Start the vLQMS FastAPI backend (:8001) and Vite frontend (:5173) and check that they are talking to each other. Use when asked to run, start, or check the app in a browser.
---

# Run vLQMS locally

1. **Backend** (from `backend/`). Create the venv once if `.venv/` is missing: `python3 -m venv .venv && .venv/bin/pip install -r requirements.txt`. Then start `.venv/bin/uvicorn app.main:app --reload --port 8001` in the background.
2. **Frontend** (from `frontend/`). The dev server is often already running, so run `curl -s http://localhost:5173/` first. If nothing answers, start `npm run dev` in the background. Restart it after changing `vite.config.ts` or moving folders.
3. **Check the connection:** `curl -s http://localhost:5173/api/` should return `{"status":"ok",...}`, which means the proxy reaches the backend. Swagger UI is at http://localhost:8001/docs.
4. **Log in** as `ADMIN` / `admin`. Every module is visible to that user. To test role gating, use `GURU`/`guru` (Manager) or `PRADEEP`/`pradeep` (Cashier).

The SQL Server is on the office LAN (settings in `backend/.env`, never committed). Away from the office, startup waits for the connection timeouts and then logs "running with in-memory store". That's expected, so don't try to fix it.

The backend keeps its data in memory. At startup it loads live data from SQL Server if `backend/.env` points at a reachable server, and otherwise uses the seed data in `backend/app/store.py`. `curl -s http://localhost:8001/` shows `db.connected`. Writes are never saved to the DB, so restarting uvicorn (including `--reload` after a Python edit) wipes any documents you created.
