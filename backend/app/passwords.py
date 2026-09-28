"""Password hashing for app users (salted PBKDF2-HMAC-SHA256, standard library only).

Passwords are never stored or compared in plain text:
- hash_password("secret") -> "pbkdf2_sha256$600000$<salt>$<hash>" (a new random salt each time).
- verify_password("secret", stored) checks in constant time, using the iteration count stored in
  the hash, so old hashes keep working if PASSWORD_HASH_ITERATIONS changes.

PASSWORD_HASH_ITERATIONS (env) defaults to 600,000, the OWASP 2023 figure for PBKDF2-SHA256.
The test suite lowers it so tests stay fast; never lower it in backend/.env.
"""
import base64
import hashlib
import hmac
import os
import secrets

ALGORITHM = "pbkdf2_sha256"
ITERATIONS = int(os.environ.get("PASSWORD_HASH_ITERATIONS", "600000"))
MIN_LENGTH = 8


def hash_password(password: str) -> str:
    salt = secrets.token_bytes(16)
    digest = hashlib.pbkdf2_hmac("sha256", password.encode(), salt, ITERATIONS)
    return f"{ALGORITHM}${ITERATIONS}${_b64(salt)}${_b64(digest)}"


def verify_password(password: str, stored: str | None) -> bool:
    if not stored:
        return False
    try:
        algorithm, iterations, salt, digest = stored.split("$")
        if algorithm != ALGORITHM:
            return False
        expected = base64.b64decode(digest)
        actual = hashlib.pbkdf2_hmac("sha256", password.encode(), base64.b64decode(salt), int(iterations))
    except (ValueError, TypeError):
        return False
    return hmac.compare_digest(actual, expected)


def password_problem(password: str, user: str) -> str | None:
    """Why a new password is not acceptable, or None when it is fine."""
    if len(password) < MIN_LENGTH:
        return f"Password must be at least {MIN_LENGTH} characters"
    if password.strip().lower() == user.strip().lower():
        return "Password must not be the same as the user name"
    if password.isalpha() or password.isdigit():
        return "Password must mix letters with numbers or symbols"
    return None


def _b64(raw: bytes) -> str:
    return base64.b64encode(raw).decode()
