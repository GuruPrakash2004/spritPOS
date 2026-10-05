"""Manage the central site (app/central) from the command line, on the hosted server.

Run from backend/, with CENTRAL_DB_* in that server's backend/.env:
    .venv/bin/python tools/central.py init-db                      # create the central tables (safe to run again)
    .venv/bin/python tools/central.py add-client SHOP1 "Lakshmi Wines"   # prints the shop's sync key ONCE
    .venv/bin/python tools/central.py rotate-key SHOP1             # a new key (the old one stops working)
    .venv/bin/python tools/central.py disable SHOP1 | enable SHOP1
    .venv/bin/python tools/central.py clients
    .venv/bin/python tools/central.py add-user RAVI --role Owner   # prints a temporary password ONCE
    .venv/bin/python tools/central.py reset-password RAVI          # a new temporary password
    .venv/bin/python tools/central.py grant RAVI SHOP1 | revoke RAVI SHOP1
    .venv/bin/python tools/central.py users

The shop puts its key in its own backend/.env (WEB_SYNC_URL = this site's API address, WEB_SYNC_KEY = the
key). Only the key's SHA-256 is stored here, so a lost key can't be shown again: rotate it. Temporary
passwords must be changed at the first sign-in. Keys and passwords are printed, never written to a file.
"""
import argparse
import re
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from app import db, passwords, setup_db  # noqa: E402  (loads backend/.env)
from app.central import auth, config  # noqa: E402
from app.central.repo import Unavailable  # noqa: E402

SCHEMA = Path(__file__).resolve().parents[1] / "migrations" / "central" / "001_central.sql"
CODE_RE = re.compile(r"^[A-Z0-9][A-Z0-9_-]{1,19}$")


def _code(value: str, what: str = "Shop code") -> str:
    value = value.strip().upper()
    if not CODE_RE.match(value):
        sys.exit(f"{what}: 2-20 letters, digits, - or _")
    return value


def _client_id(code: str) -> int:
    found = next((c for c in config.REPO.clients() if c["code"] == code), None)
    if found is None:
        sys.exit(f"No shop '{code}'")
    return found["id"]


def init_db() -> None:
    with db.transaction() as cur:
        for batch in setup_db.batches(SCHEMA.read_text()):
            cur.execute_raw(batch)
    print("Central tables are ready.")


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("action", choices=("init-db", "add-client", "rotate-key", "disable", "enable", "clients",
                                       "add-user", "reset-password", "grant", "revoke", "users", "deactivate-user"))
    ap.add_argument("args", nargs="*")
    ap.add_argument("--role", choices=auth.ROLES, default=auth.OWNER)
    a = ap.parse_args()

    repo = config.connect()
    if repo.kind != "sql":
        sys.exit("Set CENTRAL_DB_HOST / CENTRAL_DB_NAME / CENTRAL_DB_USER / CENTRAL_DB_PASSWORD in backend/.env.")
    if not db.is_db_available():
        sys.exit("Could not connect to the central SQL Server: check CENTRAL_DB_* and that the server is reachable.")
    need = {"add-client": 2, "rotate-key": 1, "disable": 1, "enable": 1, "add-user": 1, "reset-password": 1,
            "grant": 2, "revoke": 2, "deactivate-user": 1}.get(a.action, 0)
    if len(a.args) != need:
        sys.exit(f"{a.action} needs {need} argument(s); see --help")

    try:
        _run(a, repo)
    except (KeyError, ValueError, Unavailable) as exc:
        sys.exit(str(exc).strip("'\""))


def _run(a, repo) -> None:
    if a.action == "init-db":
        init_db()
    elif a.action == "add-client":
        code, name = _code(a.args[0]), a.args[1].strip()[:100]
        key = auth.new_sync_key()
        repo.add_client(code, name, auth.key_hash(key))
        print(f"Shop {code} added. Its sync key (shown only now; put it in the shop's backend/.env):\n\n"
              f"  WEB_SYNC_KEY={key}\n")
    elif a.action == "rotate-key":
        code = _code(a.args[0])
        key = auth.new_sync_key()
        repo.set_key(code, auth.key_hash(key))
        print(f"New sync key for {code} (the old one no longer works):\n\n  WEB_SYNC_KEY={key}\n")
    elif a.action in ("disable", "enable"):
        repo.set_active(_code(a.args[0]), a.action == "enable")
        print(f"Shop {a.args[0].upper()} {a.action}d.")
    elif a.action == "clients":
        for c in repo.clients():
            print(f"{c['code']:<20} {'active' if c['active'] else 'DISABLED':<9} last sync {c['last_sync'] or '-':<20} "
                  f"day {c['last_day'] or '-':<11} {c['name']}")
    elif a.action in ("add-user", "reset-password"):
        name = _code(a.args[0], "User name")
        temp = auth.new_temporary_password()
        if a.action == "add-user":
            repo.add_user(name, passwords.hash_password(temp), a.role)
        else:
            if not repo.user(name):
                sys.exit(f"No user '{name}'")
            repo.set_password(name, passwords.hash_password(temp), must_change=True)
        print(f"Temporary password for {name} (shown only now; it must be changed at the first sign-in):\n\n  {temp}\n")
    elif a.action == "deactivate-user":
        repo.set_user_active(_code(a.args[0], "User name"), False)
        print("User deactivated (signed out everywhere).")
    elif a.action in ("grant", "revoke"):
        name, code = _code(a.args[0], "User name"), _code(a.args[1])
        if not repo.user(name):
            sys.exit(f"No user '{name}'")
        (repo.grant if a.action == "grant" else repo.revoke)(name, _client_id(code))
        print(f"{name} {'can now see' if a.action == 'grant' else 'no longer sees'} {code}.")
    elif a.action == "users":
        for u in repo.users():
            shops = ", ".join(c["code"] for c in repo.clients(repo.user_client_ids(u["user"]))) if u["role"] != auth.ADMIN else "all"
            print(f"{u['user']:<20} {u['role']:<6} {'active' if u['active'] else 'inactive':<9} shops: {shops}")


if __name__ == "__main__":
    main()
