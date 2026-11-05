"""Held sales bills in direct mode.

The desktop keeps a held bill's lines in an XML file on its own PC and only a name row (UtlHoldBill) in the database, so no desktop table can
hold a web bill's lines. Direct mode therefore keeps them in a small JSON file next to the backend (backend/data/held_bills_<database>.json,
git-ignored), so held bills survive a restart of the backend. Nothing is written to the shop's database. The file is rewritten only when a
held bill is added, released or discarded, and it is read back at start-up (before that the bills sat in memory only and were lost).
"""
import hashlib
import json
import logging
import os
import re
from pathlib import Path

from . import db, store

logger = logging.getLogger("vlqms.held")
_last = ""   # hash of what the file holds now


def _path() -> Path:
    base = Path(os.getenv("HELD_BILLS_DIR") or Path(__file__).resolve().parent.parent / "data")
    name = re.sub(r"[^A-Za-z0-9_.-]", "_", db.DB_NAME or "shop")
    return base / f"held_bills_{name}.json"


def _held() -> dict:
    return {no: d for no, d in store.DOCS["sales_invoice"].items() if d.get("status") == "HOLD"}


def dump_if_changed() -> bool:
    """Write the held bills to the file when they differ from what it holds. Returns True when it wrote. Never raises: a bill is already saved."""
    global _last
    try:
        text = json.dumps(_held(), sort_keys=True, default=str)
        digest = hashlib.sha256(text.encode()).hexdigest()
        if digest == _last:
            return False
        path = _path()
        path.parent.mkdir(parents=True, exist_ok=True)
        tmp = path.with_suffix(".tmp")
        tmp.write_text(text, encoding="utf-8")
        tmp.replace(path)   # atomic: a crash never leaves half a file
        _last = digest
        return True
    except Exception:   # noqa: BLE001
        logger.exception("Held bills could not be written to their file")
        return False


def restore() -> int:
    """Read the file back into memory at start-up; returns how many held bills were restored. The HOLD number series continues after the highest."""
    global _last
    path = _path()
    if not path.exists():
        _last = hashlib.sha256(json.dumps({}, sort_keys=True).encode()).hexdigest()
        return 0
    try:
        held = json.loads(path.read_text(encoding="utf-8"))
    except Exception:   # noqa: BLE001
        logger.exception("Held bills file could not be read: %s", path)
        return 0
    n = 0
    for no, doc in held.items():
        if no not in store.DOCS["sales_invoice"]:
            store.DOCS["sales_invoice"][no] = doc
            n += 1
        try:
            seq = int(no.rsplit("-", 1)[1])
            store._seq["hold"].value = max(store._seq["hold"].value, seq + 1)
        except (IndexError, ValueError):
            pass
    _last = hashlib.sha256(json.dumps(_held(), sort_keys=True, default=str).encode()).hexdigest()
    logger.info("Restored %d held bill(s) from %s", n, path.name)
    return n
