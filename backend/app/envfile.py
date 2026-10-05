"""Write the DB_* settings into backend/.env (the one-time Database setup, Utilities → Database Connectivity).

Only the keys given are replaced (or added); every other line (JWT secret, SMTP, comments) is kept as it is.
The file is written to a temporary file next to it and renamed over it, so a crash never leaves half a file,
and it is owner-only (0600). The DB password is written only sealed (DB_PASSWORD_ENC, secure_fields.seal);
a plain DB_PASSWORD line is removed.
"""
import os
import re
import tempfile
from pathlib import Path

PATH = Path(__file__).resolve().parents[1] / ".env"
DB_KEYS = ("DB_HOST", "DB_PORT", "DB_NAME", "DB_AUTH", "DB_USER", "DB_PASSWORD_ENC")
_LINE = re.compile(r"^\s*([A-Z0-9_]+)\s*=")


def _clean(value: str) -> str:
    if "\n" in value or "\r" in value:
        raise ValueError("A setting can't contain a line break")
    return value.strip()


def update(values: dict[str, str], remove: tuple[str, ...] = ("DB_PASSWORD",), path: Path | None = None) -> None:
    path = path or PATH
    values = {k: _clean(v) for k, v in values.items()}
    lines = path.read_text().splitlines() if path.exists() else []
    out, seen = [], set()
    for line in lines:
        m = _LINE.match(line)
        key = m.group(1) if m else None
        if key in remove and key not in values:
            continue
        if key in values:
            if key in seen:
                continue
            out.append(f"{key}={values[key]}")
            seen.add(key)
        else:
            out.append(line)
    missing = [k for k in values if k not in seen]
    if missing:
        out += ["", "# Database (written by the one-time Database setup)"] + [f"{k}={values[k]}" for k in missing]
    fd, tmp = tempfile.mkstemp(dir=path.parent, prefix=".env.", text=True)
    try:
        if hasattr(os, "fchmod"):   # owner-only on Unix; on Windows the folder's permissions apply
            os.fchmod(fd, 0o600)
        with os.fdopen(fd, "w") as f:
            f.write("\n".join(out) + "\n")
        os.replace(tmp, path)
    except BaseException:
        try:
            os.unlink(tmp)
        except FileNotFoundError:
            pass
        raise
