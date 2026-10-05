"""The central site's settings and storage.

Settings come from the central server's backend/.env (loaded by app/db.py), with their own names so a shop's
DB_* settings can never point the central site at a shop database:
  CENTRAL_DB_HOST, CENTRAL_DB_PORT, CENTRAL_DB_NAME, CENTRAL_DB_USER, CENTRAL_DB_PASSWORD_ENC (sealed with
  secure_fields.seal) or CENTRAL_DB_PASSWORD: the central SQL Server. Blank host: the in-memory store
  (MemoryRepo), for tests and for trying it out; nothing is kept (CENTRAL_DEMO=1 adds a demo shop and admin).
  CENTRAL_DOCS=1: show the API's /docs (off by default on the hosted site).
  CENTRAL_JWT_SECRET: signs the owners' sign-in tokens (set it on the server; otherwise a random one is made
  at each start and everyone signs in again after a restart).
"""
import logging
import os
import secrets

from .. import db
from .repo import MemoryRepo, SqlRepo

logger = logging.getLogger("vlqms.central")


def _env(key: str) -> str:
    return os.environ.get(key, "").strip()


JWT_SECRET = _env("CENTRAL_JWT_SECRET") or secrets.token_urlsafe(48)
if not _env("CENTRAL_JWT_SECRET"):
    logger.warning("CENTRAL_JWT_SECRET is not set: sign-ins end when the central site restarts")

REPO: "MemoryRepo | SqlRepo"


def connect() -> "MemoryRepo | SqlRepo":
    """Point app/db.py at the central database (never at a shop's DB_* settings) and pick the store."""
    global REPO
    host = _env("CENTRAL_DB_HOST")
    sealed = _env("CENTRAL_DB_PASSWORD_ENC")
    if host and not sealed and _env("CENTRAL_DB_PASSWORD"):
        from .. import secure_fields
        sealed = secure_fields.seal(_env("CENTRAL_DB_PASSWORD"))
    db.use_settings(host, _env("CENTRAL_DB_PORT"), _env("CENTRAL_DB_NAME") if host else "",
                    _env("CENTRAL_DB_USER") if host else "", "sql", sealed if host else "")
    if host:
        # Never memory once a database is set: pushes would be accepted and lost. Until it can be reached,
        # every call answers 503 and the shops send the day again later.
        if db.reconnect():
            logger.info("Central site: SQL Server database '%s'", db.DB_NAME)
        else:
            logger.error("Central site: the central database can't be reached yet; answering 503 until it can")
        REPO = SqlRepo(db)
    else:
        REPO = MemoryRepo()
        if _env("CENTRAL_DEMO") == "1":
            _demo(REPO)
    return REPO


def _demo(repo: MemoryRepo) -> None:
    """CENTRAL_DEMO=1, in memory only: a shop DEMO and an administrator ADMIN, printed once, for trying the
    central site on one machine (never with a database)."""
    from .. import passwords
    from . import auth
    key, temp = auth.new_sync_key(), auth.new_temporary_password()
    repo.add_client("DEMO", "Demo shop", auth.key_hash(key))
    repo.add_user("ADMIN", passwords.hash_password(temp), auth.ADMIN)
    print(f"\nCentral demo (in memory): shop DEMO  WEB_SYNC_KEY={key}\n"
          f"                         sign in as ADMIN with the temporary password {temp}\n", flush=True)


REPO = MemoryRepo()
