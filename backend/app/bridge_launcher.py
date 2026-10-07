"""Starts the Crystal Reports bridge together with the backend (Windows, bridge on this PC).

On startup, on Windows with the built tools/rptbridge/bin/RptBridge.exe, a small supervisor thread runs the bridge on
127.0.0.1:8765 (or the port of RPT_BRIDGE_URL when one is set) with a key made up at start (or RPT_BRIDGE_KEY),
and starts it again whenever it exits (it exits by itself when a report hangs, see tools/rptbridge/Program.cs). If a
bridge already answers on that port (started by hand or as a service) nothing is started. The bridge is stopped when
the backend stops. RPT_BRIDGE_AUTOSTART=0 turns this off (e.g. when the bridge runs as its own Windows service).
"""
import logging
import os
import subprocess
import sys
import threading
import time
from pathlib import Path
from urllib.parse import urlparse

import httpx

from . import rpt_bridge

logger = logging.getLogger("vlqms.bridge")
EXE = rpt_bridge.EXE

_stop = threading.Event()
_proc: subprocess.Popen | None = None


def _answering(base: str) -> bool:
    """Whether something already listens there (any HTTP answer counts; only a refused connection does not)."""
    try:
        httpx.get(base, timeout=2.0)
        return True
    except httpx.HTTPError:
        return False


def _port() -> int | None:
    u = urlparse(rpt_bridge.url())
    if (u.hostname or "") not in rpt_bridge.LOOPBACK:
        return None
    return u.port or 8765


def should_start() -> int | None:
    """The port to start the bridge on, or None when it should not be started from here."""
    if sys.platform != "win32" or os.environ.get("RPT_BRIDGE_AUTOSTART", "1") == "0":
        return None
    if not rpt_bridge.configured() or not EXE.exists():
        return None
    return _port()


def _supervise(port: int) -> None:
    global _proc
    env = {**os.environ, "RPT_BRIDGE_KEY": rpt_bridge.key()}
    flags = getattr(subprocess, "CREATE_NO_WINDOW", 0)
    while not _stop.is_set():
        try:
            _proc = subprocess.Popen([str(EXE), str(port)], cwd=str(EXE.parent), env=env, creationflags=flags,
                                     stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
            logger.info("Crystal Reports bridge started on port %s", port)
            _proc.wait()
        except OSError as exc:
            logger.warning("Crystal Reports bridge could not start: %s", exc)
        if not _stop.wait(3):   # it exited (a hung report); start it again
            logger.info("Crystal Reports bridge exited; restarting")


def start() -> None:
    port = should_start()
    if port is None:
        return
    if _answering(f"http://127.0.0.1:{port}"):
        logger.info("Crystal Reports bridge already running on port %s", port)
        return
    _stop.clear()
    threading.Thread(target=_supervise, args=(port,), name="rpt-bridge", daemon=True).start()


def stop() -> None:
    _stop.set()
    if _proc and _proc.poll() is None:
        _proc.terminate()
        try:
            _proc.wait(5)
        except subprocess.TimeoutExpired:
            _proc.kill()
