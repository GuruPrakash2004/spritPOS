"""One-time database setup from the command line (the same code as the sign-in page's Database setup).

Run from backend/, with the connection in backend/.env:
    .venv/bin/python tools/setup_db.py check      # what the database is; changes nothing
    .venv/bin/python tools/setup_db.py prepare    # create the web app's own tables (Web_App*, Web_ToolAudit)

Nothing is copied and no desktop table is changed: the app works on the shop's vLQMS database directly.
See app/setup_db.py.
"""
import argparse
import json
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from app import db, setup_db  # noqa: E402  (loads backend/.env)


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("action", choices=("check", "prepare"))
    ap.add_argument("--yes", action="store_true", help="don't ask for confirmation")
    args = ap.parse_args()
    if not db.init_db():
        sys.exit("Could not connect to SQL Server (check backend/.env).")
    info = setup_db.check()
    print(json.dumps(info, indent=1, default=str))
    if args.action == "check":
        return
    if not args.yes and input(f"Create the web app's tables in '{info['database']}'? Type the database name: ").strip() != info["database"]:
        sys.exit("Cancelled.")
    try:
        print(json.dumps(setup_db.prepare(), indent=1, default=str))
    except setup_db.SetupError as exc:
        sys.exit(f"Setup failed: {exc}")


if __name__ == "__main__":
    main()
