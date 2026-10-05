"""One-time database setup from the command line (the same code as the sign-in page's Database setup).

Run from backend/, with the connection in backend/.env:
    .venv/bin/python tools/setup_db.py check          # what the database has; changes nothing
    .venv/bin/python tools/setup_db.py run            # create the Web_ tables and copy the old data into them
    .venv/bin/python tools/setup_db.py run --force    # copy again although the web app has saved data (overwrites it)

The legacy tables are only read. See app/setup_db.py.
"""
import argparse
import json
import sys
import threading
import time
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from app import db, setup_db  # noqa: E402  (loads backend/.env)


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("action", choices=("check", "run"))
    ap.add_argument("--force", action="store_true", help="copy again even though the web app has saved data")
    ap.add_argument("--yes", action="store_true", help="don't ask for confirmation")
    args = ap.parse_args()
    if not db.init_db():
        sys.exit("Could not connect to SQL Server (check backend/.env).")
    info = setup_db.check()
    print(json.dumps(info, indent=1, default=str))
    if args.action == "check":
        return
    if not args.yes and input(f"Copy the old data into the Web_ tables of '{info['database']}'? Type the database name: ").strip() != info["database"]:
        sys.exit("Cancelled.")
    done = threading.Event()
    result: dict = {}

    def work():
        try:
            result["ok"] = setup_db.copy_all(force=args.force)
        except setup_db.SetupError as exc:
            result["error"] = str(exc)
        done.set()

    threading.Thread(target=work, daemon=True).start()
    while not done.wait(5):
        p = setup_db.PROGRESS
        print(f"  {p.get('state')}: table {p.get('tables_done', 0)}/{p.get('tables_total', '?')} {p.get('table', '')} "
              f"- {p.get('rows_done', 0):,}/{p.get('rows_total', 0):,} rows", flush=True)
    if "error" in result:
        sys.exit(f"Setup failed: {result['error']}")
    print(f"Done: {result['ok']}")


if __name__ == "__main__":
    t0 = time.time()
    main()
    print(f"({time.time() - t0:.0f} s)")
