"""Central site sign-in: owners and the central administrator (CenUser), and the shops' sync keys.

Passwords arrive encrypted from the browser (secure_fields, purposes "login", "current" and "new"), are kept
only as salted hashes (app/passwords.py), and five wrong ones lock a user name for 15 minutes. A new user's
first password is temporary: until it is changed, the token is good only for changing it. Shops send their
sync key (Bearer) with every push; only its SHA-256 is stored, and wrong keys count against the address.
"""
import hashlib
import secrets
import threading
import time
from collections import defaultdict, deque
from datetime import datetime, timedelta, timezone

import jwt
from fastapi import Depends, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer

from . import config

ADMIN, OWNER = "Admin", "Owner"
ROLES = (ADMIN, OWNER)
TOKEN_HOURS = 12
LOCK_WINDOW_SECONDS = 15 * 60
MAX_FAILS_PER_USER = 5
MAX_FAILS_PER_ADDRESS = 20

bearer = HTTPBearer(auto_error=False)
_failures: dict[str, deque] = defaultdict(deque)
_failures_lock = threading.Lock()


def key_hash(key: str) -> str:
    return hashlib.sha256(key.encode()).hexdigest()


def new_sync_key() -> str:
    return "vlqms_" + secrets.token_urlsafe(32)


def new_temporary_password() -> str:
    return secrets.token_urlsafe(9) + "7"   # letters and digits, so it passes password_problem


# ─── Lockouts ────────────────────────────────────────────────────────────────

def _recent(key: str, now: float) -> deque:
    q = _failures[key]
    while q and now - q[0] > LOCK_WINDOW_SECONDS:
        q.popleft()
    return q


def check_not_locked(keys: list[tuple[str, int]], what: str = "failed sign-ins") -> None:
    now = time.monotonic()
    with _failures_lock:
        for key, limit in keys:
            q = _recent(key, now)
            if len(q) >= limit:
                wait = int(LOCK_WINDOW_SECONDS - (now - q[0])) // 60 + 1
                raise HTTPException(429, f"Too many {what}. Try again in {wait} minute{'s' if wait != 1 else ''}.")


def record_failure(keys: list[str]) -> None:
    now = time.monotonic()
    with _failures_lock:
        for key in keys:
            _recent(key, now).append(now)


def clear_failures(key: str | None = None) -> None:
    with _failures_lock:
        if key is None:
            _failures.clear()
        else:
            _failures.pop(key, None)


def address(request: Request) -> str:
    return request.client.host if request.client else "unknown"


# ─── Tokens ──────────────────────────────────────────────────────────────────

def token_for(user: dict) -> str:
    now = datetime.now(timezone.utc)
    claims = {"sub": user["user"], "role": user["role"], "pwv": user["pwv"], "iat": now,
              "exp": now + timedelta(hours=TOKEN_HOURS), "aud": "vlqms-central"}
    return jwt.encode(claims, config.JWT_SECRET, algorithm="HS256")


def session(user: dict) -> dict:
    token = token_for(user)
    return {"user": user["user"], "role": user["role"], "must_change": user["must_change"],
            "access_token": token, "token_type": "bearer"}


def signed_in_user(creds: HTTPAuthorizationCredentials | None = Depends(bearer)) -> dict:
    """The token's user, still active and with the same password version; else 401."""
    if creds is None or creds.scheme.lower() != "bearer":
        raise HTTPException(401, "Sign in first")
    try:
        claims = jwt.decode(creds.credentials, config.JWT_SECRET, algorithms=["HS256"], audience="vlqms-central")
    except jwt.ExpiredSignatureError:
        raise HTTPException(401, "Your session has ended. Sign in again.") from None
    except jwt.InvalidTokenError:
        raise HTTPException(401, "Sign in first") from None
    user = config.REPO.user(claims.get("sub", ""))
    if not user or not user["active"] or user["pwv"] != claims.get("pwv"):
        raise HTTPException(401, "Your session has ended. Sign in again.")
    return user


def current_user(user: dict = Depends(signed_in_user)) -> dict:
    """A signed-in user who has replaced their temporary password."""
    if user["must_change"]:
        raise HTTPException(403, "Change your temporary password first")
    return user


def require_admin(user: dict = Depends(current_user)) -> dict:
    if user["role"] != ADMIN:
        raise HTTPException(403, "Only the central administrator can do this")
    return user


def visible_client_ids(user: dict) -> list[int]:
    """The shops this user may see: every shop for the administrator, the granted ones for an owner."""
    if user["role"] == ADMIN:
        return [c["id"] for c in config.REPO.clients()]
    return config.REPO.user_client_ids(user["user"])


def pushing_client(request: Request, creds: HTTPAuthorizationCredentials | None = Depends(bearer)) -> dict:
    """The shop whose sync key came with this push; 401 for a missing, wrong or disabled key."""
    addr = f"sync-addr:{address(request)}"
    check_not_locked([(addr, MAX_FAILS_PER_ADDRESS)], "wrong sync keys")
    client = config.REPO.client_by_key(key_hash(creds.credentials)) if creds and creds.scheme.lower() == "bearer" else None
    if client is None or not client["active"]:
        record_failure([addr])
        raise HTTPException(401, "Unknown or disabled sync key")
    return client
