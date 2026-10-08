"""Starts the Crystal Reports bridge together with the backend (Windows, bridge on this PC), so a server needs one service.

On startup, on Windows with the built tools/rptbridge/bin/RptBridge.exe, a small supervisor thread runs the bridge on
127.0.0.1:8765 (or the port of RPT_BRIDGE_URL when one is set) with the key RPT_BRIDGE_KEY, or one made up at start, and
starts it again whenever it exits (it exits by itself when a report hangs, see tools/rptbridge/Program.cs). If a bridge
already answers on that port and accepts our key (started by hand or as a service) nothing is started; one that refuses
a made-up key is a leftover of an earlier run and is replaced (never when RPT_BRIDGE_KEY is set: that one is not ours to
kill). The bridge is stopped when the backend stops, and runs as the same Windows user as the backend: for a direct print
to a network printer, run the backend service as a user who has that printer added.
RPT_BRIDGE_AUTOSTART=0 turns this off (e.g. when the bridge runs as its own Windows service on this PC).
"""
import logging
import os
import subprocess
import sys
import threading
import time
from urllib.parse import urlparse

import httpx

from . import rpt_bridge

logger = logging.getLogger("vlqms.bridge")
EXE = rpt_bridge.EXE

_stop = threading.Event()
_proc: subprocess.Popen | None = None
_NO_WINDOW = getattr(subprocess, "CREATE_NO_WINDOW", 0)


def _answering(base: str) -> bool:
    """Whether something already listens there (any HTTP answer counts; only a refused connection does not)."""
    try:
        httpx.get(base, timeout=2.0)
        return True
    except httpx.HTTPError:
        return False


def _key_accepted(base: str) -> bool | None:
    """Whether the bridge answering there accepts our key (GET /health): True, False (wrong key), None (no answer)."""
    try:
        r = httpx.get(base + "/health", headers={"X-Bridge-Key": rpt_bridge.key()}, timeout=2.0)
    except httpx.HTTPError:
        return None
    return r.status_code != 401


def _port() -> int | None:
    u = urlparse(rpt_bridge.url())
    if (u.hostname or "") not in rpt_bridge.LOOPBACK:
        return None
    return u.port or 8765


def _kill_stale(port: int) -> bool:
    """Stop leftover RptBridge.exe processes (a bridge of an earlier backend run holds the port with a key this run
    doesn't know). Returns True once the port is free."""
    subprocess.run(["taskkill", "/IM", EXE.name, "/F"], capture_output=True, creationflags=_NO_WINDOW)
    for _ in range(10):
        if not _answering(f"http://127.0.0.1:{port}"):
            return True
        time.sleep(0.3)
    return False


def should_start() -> int | None:
    """The port to start the bridge on, or None when it should not be started from here."""
    if sys.platform != "win32" or os.environ.get("RPT_BRIDGE_AUTOSTART", "1") == "0":
        return None
    if not EXE.exists():
        return None
    return _port()


def _supervise(port: int) -> None:
    global _proc
    env = {**os.environ, "RPT_BRIDGE_KEY": rpt_bridge.key()}
    while not _stop.is_set():
        try:
            _proc = subprocess.Popen([str(EXE), str(port)], cwd=str(EXE.parent), env=env, creationflags=_NO_WINDOW,
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
    base = f"http://127.0.0.1:{port}"
    if _answering(base):
        if _key_accepted(base) is not False:
            logger.info("Crystal Reports bridge already running on port %s", port)
            return
        if os.environ.get("RPT_BRIDGE_KEY", "").strip():   # run by hand with the key from .env: not ours to kill
            logger.warning("A bridge on port %s refuses RPT_BRIDGE_KEY: prints will fail until the keys match", port)
            return
        logger.info("A bridge from an earlier run holds port %s with another key; replacing it", port)
        if not _kill_stale(port):
            logger.warning("Could not stop the old bridge on port %s: prints will fail until it is closed", port)
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
