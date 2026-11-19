"""Build backend/data/licence_keys.json (the Key Generator's Reg. No -> Key table) from the desktop source's Class/clsReg.vb.

    python tools/import_licence_keys.py [path/to/clsReg.vb]

The two lists in clsReg.vb (initregNos, initkeys) are paired by position, as clsReg.getKey does. The output is gitignored: keep it off
every shop's server, put it only where the vendor runs the Key Generator.
"""
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
src = Path(sys.argv[1]) if len(sys.argv) > 1 else ROOT / "source_code" / "Class" / "clsReg.vb"
text = src.read_text(encoding="utf-8-sig")


def block(name: str, add: str) -> list[str]:
    m = re.search(rf"Sub {name}\(\)(.*?)End Sub", text, re.S | re.I)
    if not m:
        sys.exit(f"{name} not found in {src}")
    return re.findall(rf'{add}\.Add\("([^"]*)"\)', m.group(1))


regs, keys = block("initregNos", "clRegNo"), block("initkeys", "clKey")
if len(regs) != len(keys):
    sys.exit(f"{len(regs)} Reg. Nos but {len(keys)} keys: the lists don't pair up")
table = {}
# clsReg.getKey loops `For i = 1 To clRegNo.Count - 1`, so the desktop never matches the last pair; the web does the same.
for r, k in list(zip(regs, keys))[:-1]:
    table.setdefault(r, k)   # the desktop takes the first match
out = ROOT / "backend" / "data" / "licence_keys.json"
out.parent.mkdir(exist_ok=True)
out.write_text(json.dumps(table, indent=0), encoding="utf-8")
print(f"{len(table)} keys written to {out}")
