"""The desktop vLQMS's reversible password cipher (svtCrypto in vLQMS.exe), so MstUser.LoginPwd can be checked.

LoginPwd is Base64( TripleDES-ECB( ASCII password ) ) with PKCS7 padding and the key MD5("svt$$12@#") (16 bytes, which .NET
expands to K1,K2,K1). Only used to compare a typed password with the desktop's stored one at sign-in; after a successful
sign-in the web keeps a salted PBKDF2 hash in memory instead. The exe's other ways in (a password made from the day's date)
are deliberately not ported.
"""
import base64
import hashlib

from cryptography.hazmat.primitives import padding
from cryptography.hazmat.primitives.ciphers import Cipher, modes

try:   # cryptography 43+ keeps legacy ciphers here
    from cryptography.hazmat.decrepit.ciphers.algorithms import TripleDES
except ImportError:   # older versions
    from cryptography.hazmat.primitives.ciphers.algorithms import TripleDES

_KEY = hashlib.md5(b"svt$$12@#").digest()
_KEY = _KEY + _KEY[:8]   # .NET expands a 16-byte TripleDES key to K1,K2,K1; the library wants that spelled out


def _cipher() -> Cipher:
    return Cipher(TripleDES(_KEY), modes.ECB())


def decrypt(stored: str) -> str | None:
    """The password behind a stored LoginPwd, or None when it is empty or not in this format."""
    if not stored:
        return None
    try:
        d = _cipher().decryptor()
        raw = d.update(base64.b64decode(stored, validate=True)) + d.finalize()
        u = padding.PKCS7(64).unpadder()
        return (u.update(raw) + u.finalize()).decode("ascii")
    except Exception:   # noqa: BLE001 - not this cipher (the older plain base64 form is tried by the caller)
        return None


def encrypt(password: str) -> str:
    """The desktop's form of a password (used by tests; the web never writes LoginPwd)."""
    p = padding.PKCS7(64).padder()
    e = _cipher().encryptor()
    return base64.b64encode(e.update(p.update(password.encode("ascii")) + p.finalize()) + e.finalize()).decode()
