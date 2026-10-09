"""Finding the SQL Server instances a shop has, whatever their version, so the sign-in page's Database setup can offer every one
of them (SQL Server 2008 R2, 2012, 2014 ... 2022, Express or full) and list each instance's databases.

Three sources, merged:
- the **SQL Server Browser** (UDP 1434). A broadcast finds the servers on the local network; a question to one host lists every
  instance on it with its TCP port and version. This is how the desktop's own connection screen finds servers.
- the **Windows registry** of this PC (`HKLM\\SOFTWARE\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL`): instances installed
  here even when the Browser service is stopped, with their version and TCP port.
- the server the app is set up with (so it is always in the list).

Nothing here logs in or reads data; `routers/setup.py` tries a login against each instance when asked to scan.
"""
import logging
import socket
import sys
import time

logger = logging.getLogger("vlqms.discovery")

BROWSER_PORT = 1434
# major version -> product name (the "Version" the Browser and the registry report is major.minor.build)
PRODUCTS = {8: "SQL Server 2000", 9: "SQL Server 2005", 10: "SQL Server 2008 / 2008 R2", 11: "SQL Server 2012", 12: "SQL Server 2014",
            13: "SQL Server 2016", 14: "SQL Server 2017", 15: "SQL Server 2019", 16: "SQL Server 2022", 17: "SQL Server 2025"}


def product_name(version: str) -> str:
    try:
        return PRODUCTS.get(int(str(version).split(".")[0]), f"SQL Server {version}")
    except ValueError:
        return f"SQL Server {version}" if version else "SQL Server"


def _entry(server: str, instance: str, version: str, port: str, source: str, edition: str = "", address: str = "") -> dict:
    return {"server": server, "instance": instance or "MSSQLSERVER", "version": version, "product": product_name(version),
            "edition": edition, "port": str(port or ""), "source": source, "address": address,
            "label": f"{server}\\{instance}" if instance and instance.upper() != "MSSQLSERVER" else server}


def parse_browser(payload: bytes, address: str = "") -> list[dict]:
    """The Browser's answer: records "ServerName;X;InstanceName;Y;IsClustered;No;Version;15.0.x;tcp;1433;np;...;;" joined by ";;"."""
    text = payload[3:].decode("ascii", errors="ignore") if len(payload) > 3 else ""
    out = []
    for record in [r for r in text.split(";;") if r.strip()]:
        parts = record.split(";")
        info = {parts[i].lower(): parts[i + 1] for i in range(0, len(parts) - 1, 2)}
        if "servername" in info:
            out.append(_entry(info["servername"], info.get("instancename", ""), info.get("version", ""), info.get("tcp", ""), "network", address=address))
    return out


def browse(host: str | None = None, timeout: float = 2.0) -> list[dict]:
    """Ask the SQL Server Browser: one host (unicast) or the whole local network (broadcast). Empty when nobody answers."""
    found: list[dict] = []
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    try:
        sock.settimeout(0.4)
        if host:
            target = (host, BROWSER_PORT)
            message = b"\x03"   # CLNT_UCAST_EX: every instance of that host
        else:
            sock.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
            target = ("255.255.255.255", BROWSER_PORT)
            message = b"\x02"   # CLNT_BCAST_EX
        try:
            sock.sendto(message, target)
        except OSError as e:
            logger.info("SQL Browser not asked: %s", e)
            return []
        try:   # only the host asked may answer (a stray reply would send the login to someone else)
            asked = socket.gethostbyname(host) if host else None
        except OSError:
            asked = None
        end = time.monotonic() + timeout
        while time.monotonic() < end:
            try:
                data, sender = sock.recvfrom(65535)
            except socket.timeout:
                continue
            except OSError:
                break
            if host and asked and sender[0] != asked:
                continue
            found += parse_browser(data, sender[0])
    finally:
        sock.close()
    return found


def local_instances() -> list[dict]:
    """Instances installed on this PC, from the registry (Windows only)."""
    if sys.platform != "win32":
        return []
    try:
        import winreg
    except ImportError:
        return []
    host = socket.gethostname()
    out = []
    for view in (winreg.KEY_WOW64_64KEY, winreg.KEY_WOW64_32KEY):
        try:
            names = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, r"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", 0,
                                   winreg.KEY_READ | view)
        except OSError:
            continue
        with names:
            index = 0
            while True:
                try:
                    instance, folder, _ = winreg.EnumValue(names, index)
                except OSError:
                    break
                index += 1
                base = rf"SOFTWARE\Microsoft\Microsoft SQL Server\{folder}"
                version, edition, port = "", "", ""
                try:
                    with winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, base + r"\Setup", 0, winreg.KEY_READ | view) as k:
                        version = str(winreg.QueryValueEx(k, "Version")[0])
                        try:
                            edition = str(winreg.QueryValueEx(k, "Edition")[0])
                        except OSError:
                            pass
                except OSError:
                    pass
                try:
                    with winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, base + r"\MSSQLServer\SuperSocketNetLib\Tcp\IPAll", 0, winreg.KEY_READ | view) as k:
                        for name in ("TcpPort", "TcpDynamicPorts"):
                            try:
                                value = str(winreg.QueryValueEx(k, name)[0]).split(",")[0].strip()
                            except OSError:
                                value = ""
                            if value:
                                port = value
                                break
                except OSError:
                    pass
                if instance.upper() == "MSSQLSERVER" and not port:
                    port = "1433"
                out.append(_entry(host, instance, version, port, "this PC", edition))
    return out


def discover(host: str | None = None, configured: dict | None = None) -> list[dict]:
    """Every instance we can find, one entry per server + instance (the network's answer wins: it carries the port the
    server really listens on). `host` limits the search to that server; `configured` is the app's current connection."""
    entries = browse(host) if host else browse(None) + local_instances()
    if host:
        entries += [e for e in local_instances() if e["server"].lower() == host.lower()]
    if configured and configured.get("host"):
        entries.append(_entry(configured["host"].split("\\")[0], configured["host"].split("\\")[1] if "\\" in configured["host"] else "",
                              "", configured.get("port", ""), "in use"))
    merged: dict[tuple, dict] = {}
    for e in entries:
        key = (e["server"].lower(), e["instance"].upper())
        if key not in merged:
            merged[key] = e
        else:   # keep the richest answer: a version and a port beat a bare one
            for k in ("version", "port", "edition", "address"):
                if not merged[key][k] and e[k]:
                    merged[key][k] = e[k]
            merged[key]["product"] = product_name(merged[key]["version"])
    return sorted(merged.values(), key=lambda e: (e["server"].lower(), e["instance"].upper()))
