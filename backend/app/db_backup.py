"""Backup of the current database, as vLQMS.exe's Company Master button does it (FrmMstComp.backupDB).

`BACKUP DATABASE <name> TO DISK = '<SQLBACKUPPATH>\db<name><date>_<time>.Bak' WITH FORMAT`, where SQLBACKUPPATH is the Global
Settings value (group Database) read from UtlGSettings; the path is on the SQL Server machine. The file the shop reaches over the
network is named from SQLNWBACKUPPATH. A backup of a large database takes minutes and SQL Server will not run BACKUP inside a
transaction, so it runs on its own autocommit connection in a background thread; `status()` tells how it is going.
"""
import logging
import threading
from datetime import datetime

from . import db

logger = logging.getLogger("vlqms.backup")
_lock = threading.Lock()
_state: dict = {"state": "idle", "file": "", "network_file": "", "message": "", "started": "", "finished": ""}


def status() -> dict:
    return dict(_state)


def _setting(name: str) -> str:
    rows = db.fetchall("SELECT SetValue FROM dbo.UtlGSettings WHERE UPPER(LTRIM(RTRIM(SetGroup))) = 'DATABASE' AND UPPER(LTRIM(RTRIM(SetName))) = ?", (name.upper(),))
    return str(rows[0]["SetValue"]).strip() if rows and rows[0]["SetValue"] else ""


def start() -> dict:
    """Begin a backup. Raises ValueError with the desktop's own wording when a path is not set or one is already running."""
    with _lock:
        if _state["state"] == "running":
            raise ValueError("A backup is already running")
        path, network = _setting("SQLBACKUPPATH"), _setting("SQLNWBACKUPPATH")
        if not path:
            raise ValueError("Backup path of SQL is not specified in Global Settings. (Under Database Group)")
        if not network:
            raise ValueError("Network Backup path of SQL is not specified in Global Settings. (Under Database Group)")
        name = db.settings()["database"]
        leaf = f"\db{name}{datetime.now():%d%b%Y_%H%M%S}.Bak"
        _state.update(state="running", file=path + leaf, network_file=network + leaf, message="", started=datetime.now().isoformat(timespec="seconds"), finished="")
        threading.Thread(target=_run, args=(name, path + leaf), daemon=True).start()
        return status()


def _run(name: str, target: str) -> None:
    conn = None
    try:
        s = db.settings()
        result = db._connect_with_driver(s["host"], s["port"], "master", s["user"], db._password(), s["auth"], login_timeout=15)
        if not result:
            raise RuntimeError("Could not connect to SQL Server to take the backup")
        conn, _driver = result
        try:
            conn.autocommit = True
        except Exception:   # pymssql: autocommit is a method
            conn.autocommit(True)
        cur = conn.cursor()
        quoted = target.replace("'", "''")
        cur.execute(f"BACKUP DATABASE [{name.replace(']', ']]')}] TO DISK = N'{quoted}' WITH FORMAT")
        while cur.nextset():   # BACKUP reports progress as extra result sets; read them so it finishes
            pass
        _state.update(state="ok", message=f"Backup of current database {name} successfully taken in file {_state['network_file']}")
    except Exception as e:   # noqa: BLE001 - shown to the person who clicked Backup
        logger.exception("Backup failed")
        _state.update(state="error", message=f"Error during taking backup: {db.safe_error(e)}")
    finally:
        _state["finished"] = datetime.now().isoformat(timespec="seconds")
        if conn is not None:
            try:
                conn.close()
            except Exception:
                pass
