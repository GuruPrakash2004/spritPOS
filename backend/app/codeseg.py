"""Codes in URL paths. A master's code may hold any character, including "/" (which a path cannot carry: the server reads %2F as a slash before it
chooses the route). The browser (frontend/src/lib/api.js `seg`) writes "~" as "~7E" and "/" as "~2F" before it URL-encodes the code; every route
that takes a code in its path gives it back here. Anything else (?, #, %, \, spaces) is plain URL-encoding, which the server already undoes."""
import re

_ESCAPE = re.compile(r"~(2F|7E)")


def dec(code: str) -> str:
    return _ESCAPE.sub(lambda m: "/" if m.group(1) == "2F" else "~", code)
