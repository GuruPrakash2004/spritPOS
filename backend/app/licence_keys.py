"""Key Generator (the desktop's frmKeyReg / clsReg.getKey): the vendor's tool that gives the Key for a customer's Reg. No.

The desktop keeps a fixed table of Reg. No -> Key pairs inside clsReg.vb; the window looks the number up and shows "-" when it isn't there.
The web does the same from a server-side file that is NOT in the repository: backend/data/licence_keys.json (gitignored, or the path in
LICENCE_KEYS_FILE), a JSON object {"59183": "...", ...}. Build it once from the desktop source with `python tools/import_licence_keys.py`.
No file = the tool is switched off on this server (the exe likewise shows its menu item only on the vendor's own machines). The table never
goes to the browser: only the one key asked for does.
"""
import json
import os
from pathlib import Path

PATH = Path(os.getenv("LICENCE_KEYS_FILE") or Path(__file__).resolve().parents[1] / "data" / "licence_keys.json")
_cache: tuple[float, dict[str, str]] | None = None


def _table() -> dict[str, str]:
    """The Reg. No -> Key table, re-read when the file changes; {} when there is no file."""
    global _cache
    try:
        mtime = PATH.stat().st_mtime
    except OSError:
        _cache = None
        return {}
    if _cache is None or _cache[0] != mtime:
        try:
            raw = json.loads(PATH.read_text(encoding="utf-8"))
        except (OSError, ValueError):
            return {}
        _cache = (mtime, {str(k).strip(): str(v) for k, v in raw.items()} if isinstance(raw, dict) else {})
    return _cache[1]


def available() -> bool:
    return bool(_table())


def get_key(reg_no: str) -> str:
    """clsReg.getKey: the Key for this Reg. No., or "-" when it has none."""
    return _table().get(str(reg_no).strip(), "-")
