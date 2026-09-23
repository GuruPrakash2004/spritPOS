# vLQMS API (FastAPI)

The legacy vLQMS desktop screens as REST endpoints. There is no .NET source in `MAIN_Root/` (only the compiled exe), so these were rebuilt from `frontend/LEGACY_PARITY.md` and the legacy docs. Data is in memory, seeded with the same sample data the React app uses, and resets on restart.

```bash
python3 -m venv .venv && .venv/bin/pip install -r requirements.txt
.venv/bin/uvicorn app.main:app --reload --port 8000
```

Swagger UI: http://localhost:8000/docs · ReDoc: http://localhost:8000/redoc
