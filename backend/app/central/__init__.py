"""The hosted central site (the desktop app's central database for frmModWebSync), a separate FastAPI app.

Each shop runs its own backend (app.main) next to its own SQL Server, and pushes a summary of each day and its
current stock here (app/websync.py). The central site keeps them per shop (ClientID) and shows the owner every
shop. Run it on the hosted server with `uvicorn app.central.main:app --port 8002`; see app/central/main.py.
"""
