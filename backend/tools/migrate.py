"""Apply or roll back the Web_ schema (backend/migrations/). Usually not needed: the one-time Database setup
(app/setup_db.py, tools/setup_db.py) applies 002 itself.

Run from backend/, against the TEST copy of the database first:
    .venv/bin/python tools/migrate.py status
    .venv/bin/python tools/migrate.py up            # applies every NNN_*.sql not yet applied (safe to repeat)
    .venv/bin/python tools/migrate.py down 1        # runs 001_*.rollback.sql (drops the Web_ tables and their data)

Connection settings come from backend/.env (DB_HOST, DB_NAME, ...). If MIGRATE_DB_USER / MIGRATE_DB_PASSWORD
are set, they are used instead of DB_USER / DB_PASSWORD, so the app's own login never needs CREATE TABLE.
The app only creates the schema through the Database setup, and checks its version (setup_db.is_ready).
Every script is guarded with IF NOT EXISTS / IF OBJECT_ID, so running it twice changes nothing.
"""
import argparse
import os
import re
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from app import db  # noqa: E402  (loads backend/.env)
from app.setup_db import batches  # noqa: E402

MIGRATIONS = Path(__file__).resolve().parents[1] / "migrations"
SCRIPT = re.compile(r"^(\d{3})_[\w-]+\.sql$")


def scripts() -> dict[int, Path]:
    return {int(m.group(1)): p for p in sorted(MIGRATIONS.iterdir()) if (m := SCRIPT.match(p.name))}


def applied() -> set[int]:
    if not db.fetchone("SELECT 1 AS x FROM sys.tables WHERE name = 'Web_SchemaVersion'"):
        return set()
    return {int(r["Version"]) for r in db.fetchall("SELECT Version FROM dbo.Web_SchemaVersion")}


def run(path: Path) -> None:
    for sql in batches(path.read_text()):
        with db.transaction() as cur:
            cur.execute_raw(sql)
    print(f"  ran {path.name}")


def confirm(action: str, assume_yes: bool) -> None:
    print(f"{action} on database '{db.DB_NAME}' (server from backend/.env).")
    if assume_yes:
        return
    typed = input(f"Type the database name ({db.DB_NAME}) to continue: ").strip()
    if typed != db.DB_NAME:
        sys.exit("Cancelled.")


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("action", choices=("status", "up", "down"))
    ap.add_argument("version", nargs="?", type=int, help="for down: the migration number to roll back")
    ap.add_argument("--yes", action="store_true", help="don't ask for confirmation")
    args = ap.parse_args()

    if os.environ.get("MIGRATE_DB_USER"):
        db.DB_USER = os.environ["MIGRATE_DB_USER"]
        db.DB_PASSWORD = os.environ.get("MIGRATE_DB_PASSWORD", "")
    if not db.init_db():
        sys.exit("Could not connect to SQL Server (check backend/.env).")

    done, known = applied(), scripts()
    if args.action == "status":
        for v, p in known.items():
            print(f"  {'applied' if v in done else 'pending'}  {p.name}")
    elif args.action == "up":
        todo = [v for v in known if v not in done]
        if not todo:
            print("Nothing to apply.")
            return
        confirm(f"Apply {', '.join(known[v].name for v in todo)}", args.yes)
        for v in todo:
            run(known[v])
    else:
        if args.version is None:
            sys.exit("Say which migration to roll back, e.g.: tools/migrate.py down 1")
        path = MIGRATIONS / known[args.version].name.replace(".sql", ".rollback.sql")
        if not path.exists():
            sys.exit(f"No rollback script {path.name}")
        confirm(f"ROLL BACK {path.name} (drops its tables and their data)", args.yes)
        run(path)


if __name__ == "__main__":
    main()
