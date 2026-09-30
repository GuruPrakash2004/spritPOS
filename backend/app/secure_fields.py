"""Credentials typed in the web UI are always encrypted (owner's rule).

In transit: the browser fetches this server's RSA public key (GET /auth/transport-key, public) and encrypts
every password field with RSA-OAEP-SHA256 before sending it, as "enc:v1:<base64>". The plaintext inside is
JSON {"v": <secret>, "t": <epoch ms>, "f": <purpose>}:
- "t" is the SERVER's clock (the key endpoint returns it; the browser keeps the offset), so a counter PC with a
  wrong clock can still log in; values older than MAX_AGE_SECONDS are refused;
- each value is accepted once (a captured value can't be sent again);
- "f" ties it to one field (login, current, new, user, ...), so a captured current password can't be used as a
  login password.
The API refuses plain passwords (REQUIRE_ENCRYPTED; the tests encrypt with conftest.enc()). This complements
HTTPS; it doesn't replace it.

At rest: user passwords are never stored, only PBKDF2 hashes (app/passwords.py). Other secrets the app must
be able to read back (the SQL Server password saved from Utilities → Database Connectivity) are sealed with
AES-256-GCM: seal()/unseal(), "sealed:v1:<base64 nonce+ciphertext>".

Keys live in backend/data/ (gitignored, owner-only 0600): transport_key.pem and secret.key. When saving to
disk is off (VLQMS_STATE_DB empty, the tests) they are made in memory for this process only.
"""
import base64
import json
import os
import secrets
import hashlib
import threading
import time
from pathlib import Path

from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.asymmetric import padding, rsa
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
from fastapi import HTTPException

PREFIX = "enc:v1:"
SEALED = "sealed:v1:"
MAX_AGE_SECONDS = 300
REQUIRE_ENCRYPTED = True

KEY_DIR = Path(__file__).resolve().parents[1] / "data"
_lock = threading.Lock()
_private: rsa.RSAPrivateKey | None = None
_secret: bytes | None = None
_used: dict[str, float] = {}      # sha256 of accepted values -> when they stop being valid
_used_lock = threading.Lock()


def _on_disk() -> bool:
    return bool(os.environ.get("VLQMS_STATE_DB", "x"))


def _load_or_create(path: Path, make: "callable") -> bytes:
    """The key file's bytes, creating it (owner-only, 0600) with make() if missing. If another process created
    it first, that file wins, so every process uses the same key."""
    if not _on_disk():
        return make()
    if not path.exists():
        path.parent.mkdir(parents=True, exist_ok=True)
        data = make()
        try:
            fd = os.open(path, os.O_WRONLY | os.O_CREAT | os.O_EXCL, 0o600)
        except FileExistsError:
            pass
        else:
            with os.fdopen(fd, "wb") as f:
                f.write(data)
            return data
    return path.read_bytes()


def _private_key() -> rsa.RSAPrivateKey:
    global _private
    with _lock:
        if _private is None:
            pem = _load_or_create(KEY_DIR / "transport_key.pem", lambda: rsa.generate_private_key(
                public_exponent=65537, key_size=2048).private_bytes(serialization.Encoding.PEM,
                                                                     serialization.PrivateFormat.PKCS8,
                                                                     serialization.NoEncryption()))
            _private = serialization.load_pem_private_key(pem, password=None)
        return _private


def _secret_key() -> bytes:
    global _secret
    with _lock:
        if _secret is None:
            _secret = base64.b64decode(_load_or_create(KEY_DIR / "secret.key",
                                                       lambda: base64.b64encode(AESGCM.generate_key(bit_length=256))))
        return _secret


def public_key() -> dict:
    """What the browser needs: the SPKI public key (base64) and a short id so it can notice a new key."""
    spki = _private_key().public_key().public_bytes(serialization.Encoding.DER,
                                                    serialization.PublicFormat.SubjectPublicKeyInfo)
    return {"alg": "RSA-OAEP-256", "spki": base64.b64encode(spki).decode(),
            "kid": base64.urlsafe_b64encode(hashes_digest(spki)[:9]).decode(),
            "server_time": int(time.time() * 1000)}


def hashes_digest(data: bytes) -> bytes:
    h = hashes.Hash(hashes.SHA256())
    h.update(data)
    return h.finalize()


def encrypt_for_server(secret: str, purpose: str) -> str:
    """What the browser does (lib/secureField.ts); used by the tests."""
    payload = json.dumps({"v": secret, "t": int(time.time() * 1000), "f": purpose}).encode()
    ct = _private_key().public_key().encrypt(payload, padding.OAEP(mgf=padding.MGF1(hashes.SHA256()),
                                                                  algorithm=hashes.SHA256(), label=None))
    return PREFIX + base64.b64encode(ct).decode()


def decrypt(value: str | None, purpose: str, field: str = "Password") -> str | None:
    """The plain secret from an "enc:v1:" value made for `purpose`. None/"" stay as they are. A plain value is
    refused (400) while REQUIRE_ENCRYPTED is on; a bad, stale, re-used or other-field value is refused too.
    The secret is never logged."""
    if value is None or value == "":
        return value
    if not value.startswith(PREFIX):
        if REQUIRE_ENCRYPTED:
            raise HTTPException(400, f"{field} must be sent encrypted. Reload the page and try again.")
        return value
    try:
        raw = _private_key().decrypt(base64.b64decode(value[len(PREFIX):], validate=True),
                                     padding.OAEP(mgf=padding.MGF1(hashes.SHA256()), algorithm=hashes.SHA256(), label=None))
        data = json.loads(raw)
        secret, sent, made_for = str(data["v"]), int(data["t"]), data.get("f")
    except Exception:
        raise HTTPException(400, f"{field} could not be decrypted: the server's key changed. Reload the page and try again.")
    now = time.time()
    if abs(now * 1000 - sent) > MAX_AGE_SECONDS * 1000:
        raise HTTPException(400, f"{field} was encrypted too long ago. Try again.")
    if made_for != purpose:
        raise HTTPException(400, f"{field} was encrypted for a different field. Reload the page and try again.")
    digest = hashlib.sha256(value.encode()).hexdigest()
    with _used_lock:
        for k in [k for k, until in _used.items() if until < now]:
            del _used[k]
        if digest in _used:
            raise HTTPException(400, f"{field} was already sent once. Try again.")
        _used[digest] = now + 2 * MAX_AGE_SECONDS
    return secret


def seal(secret: str) -> str:
    """AES-256-GCM for a secret kept on the server (e.g. DB_PASSWORD in backend/.env)."""
    nonce = secrets.token_bytes(12)
    ct = AESGCM(_secret_key()).encrypt(nonce, secret.encode(), b"vlqms")
    return SEALED + base64.b64encode(nonce + ct).decode()


def unseal(value: str) -> str:
    """The secret from seal(); a value without the prefix is returned unchanged (hand-written .env)."""
    if not value.startswith(SEALED):
        return value
    blob = base64.b64decode(value[len(SEALED):])
    return AESGCM(_secret_key()).decrypt(blob[:12], blob[12:], b"vlqms").decode()
