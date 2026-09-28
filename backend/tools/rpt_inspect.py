"""Developer tool (not used by the app). Usage from backend/:
    .venv/bin/python -m pip install olefile   # or: -r requirements-dev.txt
    .venv/bin/python tools/rpt_inspect.py md Rpts/fmtSPInvPrint.rpt      # readable outline
    .venv/bin/python tools/rpt_inspect.py json Rpts/*.rpt                # machine-readable

Read the layout content of a Crystal Reports .rpt (without Crystal):
areas and sections in order (with section heights), and the objects in each section
(text objects with their exact text and embedded fields, field objects with their database field,
lines and boxes), each with its width x height in twips (1440 twips = 1 inch).
Left/top positions are NOT recoverable this way.

Works for the older Crystal format ('Contents' starts f8 64 07). Newer files (Contents starts
fc 00 ff ff) are encrypted and come back empty.
"""
import json
import re
import struct
import sys

import olefile

NAME = re.compile(rb'((?:Text|Field|Line|Box|Section|Area|Picture|Subreport|CrossTab|Chart|OleObject)\d{1,3})\x00')
REF = re.compile(r"[?@]?[A-Za-z_][\w ]*(?:;\d+)?(?:\.[A-Za-z_]\w*)?|Print (?:Date|Time)|Page ?Number|Total ?Page ?Count")


def length_prefixed(c: bytes, key: int):
    """(position, string) for every big-endian-length-prefixed, NUL-terminated printable string."""
    x = bytes(b ^ key for b in c)
    for m in re.finditer(rb'\x00\x00\x00', x):
        p = m.start() + 3
        n = x[p]
        t = x[p + 1:p + n]
        if 2 <= n < 250 and x[p + n:p + n + 1] == b'\x00' and all(32 <= ch < 127 or ch in (9, 10, 13) for ch in t):
            yield p, t.decode('latin1')


def inspect(path: str) -> dict:
    ole = olefile.OleFileIO(path)
    c = ole.openstream('Contents').read()
    encrypted = c[:4] != b'\xf8\x64\x07\x00'
    events = []
    if not encrypted:
        for k in range(256):
            x = bytes(b ^ k for b in c)
            for m in NAME.finditer(x):
                name, p = m.group(1).decode(), m.start()
                if name.startswith(('Section', 'Area')):
                    h, n = struct.unpack('>II', x[p - 12:p - 4])
                    events.append((p, 'area' if name.startswith('Area') else 'section', name, {'height': h, 'count': n}))
                else:
                    w, h = struct.unpack('>II', x[p - 20:p - 12])
                    events.append((p, 'object', name, {'width': w, 'height': h}))
        for p, v in length_prefixed(c, 0xC2):
            events.append((p, 'text', v, {}))
        for key in (0xC4, 0x9F):
            for p, v in length_prefixed(c, key):
                if REF.fullmatch(v):
                    events.append((p, 'field', v, {}))
    events = sorted(set((p, kind, v, json.dumps(e, sort_keys=True)) for p, kind, v, e in events))

    areas, area, section, obj = [], None, None, None
    for p, kind, v, extra in events:
        extra = json.loads(extra)
        if kind == 'area':
            area = {'area': v, **extra, 'sections': []}
            areas.append(area)
            section = obj = None
        elif kind == 'section' and area is not None:
            section = {'section': v, **extra, 'objects': []}
            area['sections'].append(section)
            obj = None
        elif kind == 'object' and section is not None:
            obj = {'object': v, **extra, 'content': []}
            section['objects'].append(obj)
        elif kind in ('text', 'field') and obj is not None:
            item = [kind, v]
            if not obj['content'] or obj['content'][-1] != item:
                obj['content'].append(item)
    return {'file': path.split('/')[-1], 'encrypted': encrypted, 'areas': areas}


AREA_NAMES = {'Area1': 'Report Header', 'Area2': 'Page Header', 'Area3': 'Details', 'Area4': 'Report Footer',
              'Area5': 'Page Footer'}


def to_markdown(spec: dict) -> str:
    out = [f"### {spec['file']}", '']
    if spec['encrypted']:
        return '\n'.join(out + ['Newer Crystal format (encrypted): layout not readable without Crystal Reports.', ''])
    for a in spec['areas']:
        out.append(f"- **{AREA_NAMES.get(a['area'], a['area'])}** ({a['area']})")
        for s in a['sections']:
            h = s['height']
            out.append(f"  - {s['section']}: height {h} twips ({h / 1440 * 25.4:.1f} mm){'' if s['objects'] else ', empty'}")
            for o in s['objects']:
                kind = o['object'].rstrip('0123456789')
                what = ' '.join(f'"{v}"' if k == 'text' else f'{{{v}}}' for k, v in o['content'])
                size = f"{o['width']}x{o['height']}" if o['height'] else f"{o['width']} wide"
                out.append(f"    - {kind} {o['object']} ({size} twips): {what or ('line/box' if kind in ('Line', 'Box') else '(not decoded)')}")
    return '\n'.join(out) + '\n'


if __name__ == '__main__':
    fmt = sys.argv[1]
    for path in sys.argv[2:]:
        spec = inspect(path)
        print(json.dumps(spec) if fmt == 'json' else to_markdown(spec))
