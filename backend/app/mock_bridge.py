"""Built-in stand-in for the Crystal Reports bridge: same job in, PDF out, but laid out from HTML templates.

The backend never opens a .rpt. It builds a job ({rpt, selection, parameters, copies, printer, ...}, rpt_print.py /
rpt_dsr.py / rpt_stock.py / rpt_custom.py) and sends it to a print service over `POST /render` and `POST /print`
(rpt_bridge.py). The real service is tools/rptbridge (Crystal runtime, Windows). When RPT_BRIDGE_URL is not set,
rpt_bridge calls this module instead, with the very same job, so setting RPT_BRIDGE_URL later swaps in the real one and
nothing else changes.

This module reads the .rpt file's NAME only (to pick a template) and the data the report would read: the saved
document by the ID in the selection formula, the work-table rows (RptUDR / RptStk) the backend wrote for the run, and
the job's parameters. Layout is an approximation of the desktop's, not a copy: the templates below are ours.
HTML becomes PDF with PyMuPDF's Story (no GTK / WeasyPrint needed on Windows).
"""
import io
import json
import re
from pathlib import Path
from datetime import date, datetime

import pymupdf
from fastapi import HTTPException
from jinja2 import Environment

from . import db, store

_env = Environment(autoescape=True)
UPLOADS = Path(__file__).resolve().parents[1] / "uploads" / "company"
THERMAL_WIDTH_PT = 226   # 80 mm roll
CSS = """
body { font-family: sans-serif; font-size: 9pt; }
h1 { font-size: 13pt; text-align: center; margin: 0; }
.c { text-align: center; } .r { text-align: right; }
.small { font-size: 8pt; color: #333333; }
.serif { font-family: serif; } .big { font-size: 13pt; font-weight: bold; } .big2 { font-size: 15pt; } .tiny { font-size: 6pt; }
.grey { background-color: #c0c0c0; } .box { border: 1px solid #000000; }
table { width: 100%; border-collapse: collapse; }
th { border-top: 1px solid #000000; border-bottom: 1px solid #000000; text-align: left; padding: 2px; }
td { padding: 2px; }
.tot td { border-top: 1px solid #000000; font-weight: bold; }
"""

_HEAD = """<h1>{{ co.name }}</h1>
<p class="c small">{% for a in co.address %}{{ a }}<br>{% endfor %}PH NO: {{ co.phone }}<br>Email ID: {{ co.email }}<br>GSTIN No: {{ co.gstin }}</p>"""

# The desktop's receipt (fmtSInv, 3-inch roll): logo, shop details, grey boxed headings, one block per item.
SALES_INVOICE = """
{% if logo %}<p class="c"><img src="{{ logo }}" width="150"></p>{% else %}<h1 class="serif">{{ co.name }}</h1>{% endif %}
<p class="c serif">PH NO: {{ co.phone }}<br>Email ID: {{ co.email }}<br>GSTIN No: {{ co.gstin }}</p>
<table class="box grey"><tr><td class="c big serif">{{ (h.SALTYP or 'CASH')|upper }} BILL</td></tr></table>
<table class="serif"><tr><td><b>Bill No.</b> &nbsp;{{ h.SINVNO }}</td><td class="r"><b>Bill Dt:</b> &nbsp;{{ dmy(h.SINVDate) }}</td></tr>
<tr><td>Sales Type: {{ h.SALTYP }}</td><td class="r">Time: &nbsp;{{ clock(h.SINVDateTime or h.SINVTime) }}</td></tr></table>
<table class="grey serif"><tr><td><b>#</b></td><td class="c"><b>Item Name</b></td><td class="r"><b>Qty</b></td><td class="r"><b>Rate</b></td><td class="r"><b>Amount</b></td></tr></table>
{% for l in lines %}
<table class="serif small"><tr><td width="6%">{{ loop.index }}</td><td width="50%"><b>{{ l.ITEMNAME }}</b></td>
<td class="r" width="12%">{{ num(l.SINVQty) }}</td><td class="r" width="18%">{{ m(l.MRP if l.MRP else l.SINVRate) }}</td><td></td></tr>
</table>
<table class="serif small"><tr><td width="40%">HSN: {{ l.HSNCode or 0 }}</td><td width="60%" class="r">MRP: {{ m(l.MRP if l.MRP else l.SINVRate) }}</td></tr>
<tr><td width="50%"><b>Item Total Amt :</b></td><td width="50%" class="r"><b>{{ m(l.NETTOT if l.NETTOT is not none else l.SINVAmt) }}</b></td></tr></table>
{% endfor %}
<table class="box grey serif small"><tr><td>Item Sold :</td><td class="r">{{ num(h.TOTQTY) }}</td></tr></table>
<table class="box grey"><tr><td class="serif big2"><b>Grand Total :</b></td><td class="r serif big2"><b>{{ m(h.GRNDTOT) }}</b></td></tr></table>
{% if h.Cancelled == 'Y' %}<p class="c"><b>** CANCELLED **</b></p>{% endif %}
<p class="c serif"><b><u>GST EXEMPTED ON LIQUOR</u></b></p>
<p class="c serif small"><b>NOTE :</b> No Return, No Exchange,No Claims on Breakage</p>
<p class="c serif"><b>Thank You For Shopping</b><br><span class="tiny">vLQMS-2020 by Softvent.com</span></p>"""

PURCHASE_BILL = _HEAD + """
<p class="c"><b>PURCHASE BILL</b></p>
<p>Bill No: <b>{{ h.PBNO }}</b> &nbsp; Date: {{ date(h.PBDate) }}<br>Supplier: {{ h.PartyName }}
{% if h.SUPPINVNO %}<br>Supplier invoice: {{ h.SUPPINVNO }} {{ date(h.SUPPINVDATE) }}{% endif %}</p>
<table><tr><th>Item</th><th class="r">Qty</th><th class="r">Rate</th><th class="r">Amount</th><th class="r">Net</th></tr>
{% for l in lines %}<tr><td>{{ l.ITEMNAME }}</td><td class="r">{{ num(l.PBQty) }}</td><td class="r">{{ m(l.PBRate) }}</td>
<td class="r">{{ m(l.PBAmt) }}</td><td class="r">{{ m(l.NETAMT) }}</td></tr>{% endfor %}
<tr class="tot"><td>Total</td><td></td><td></td><td class="r">{{ m(h.TOTAMT) }}</td><td class="r">{{ m(h.GRNDTOT) }}</td></tr></table>
<p class="small">CGST {{ m(h.TOTCGST) }} &nbsp; SGST {{ m(h.TOTSGST) }} &nbsp; TCS {{ m(h.TCSAMT) }} &nbsp; Cess {{ m(h.TOTCESS) }}</p>
{% if h.RMKS %}<p class="small">{{ h.RMKS }}</p>{% endif %}"""

DSR = _HEAD + """
<p class="c"><b>Daily Summary Report</b>{% if title %}<br>{{ title }}{% endif %}</p>
<table>
{% for label, key in rows %}<tr><td>{{ label }}</td><td class="r">{{ p.get(key, '') }}</td></tr>{% endfor %}
</table>
{% if groups %}<p><b>Group-wise sales</b></p>
<table><tr><th>Item group</th><th class="r">Qty</th><th class="r">Amount</th></tr>
{% for g in groups %}<tr><td>{{ g.Fld1 }}</td><td class="r">{{ g.Fld2 }}</td><td class="r">{{ g.Fld3 }}</td></tr>{% endfor %}</table>{% endif %}"""
DSR_ROWS = [("No. of invoices", "mNoOfInv"), ("Cash sales", "mCashSales"), ("Card sales", "mCardSales"), ("UPI", "mupiamt"),
            ("Total invoice value", "mTotInvValue"), ("Sales returns", "mSalesRet"), ("Expenses", "mTotExp"),
            ("Net amount", "mNetAmt"), ("CGST", "mCGSTAMT"), ("SGST", "mSGSTAMT"), ("Cess", "mCESSAMT"),
            ("Opening stock qty", "mopqty"), ("Opening stock value", "mopval"),
            ("Closing stock qty", "mclqty"), ("Closing stock value", "mclval")]

STOCK = _HEAD + """
<p class="c"><b>Stock Balance Report</b><br>{{ p.get('daterange', '') }}</p>
<table><tr><th>Item</th><th class="r">Open</th><th class="r">Purchase</th><th class="r">S.Ret</th><th class="r">Sales</th>
<th class="r">Deduct</th><th class="r">Close</th>{% if values %}<th class="r">Close value</th>{% endif %}</tr>
{% for r in rows %}<tr><td>{{ r.ITEMNAME }}</td><td class="r">{{ num(r.OPQty) }}</td><td class="r">{{ num(r.PBQty) }}</td>
<td class="r">{{ num(r.SRQty) }}</td><td class="r">{{ num(r.InvQty) }}</td><td class="r">{{ num(r.SDQty) }}</td>
<td class="r">{{ num(r.ClQty) }}</td>{% if values %}<td class="r">{{ m(r.ClValue) }}</td>{% endif %}</tr>{% endfor %}
<tr class="tot"><td>Total</td><td class="r">{{ num(t.OPQty) }}</td><td class="r">{{ num(t.PBQty) }}</td><td class="r">{{ num(t.SRQty) }}</td>
<td class="r">{{ num(t.InvQty) }}</td><td class="r">{{ num(t.SDQty) }}</td><td class="r">{{ num(t.ClQty) }}</td>
{% if values %}<td class="r">{{ m(t.ClValue) }}</td>{% endif %}</tr></table>"""

TABLE = _HEAD + """
<p class="c"><b>{{ head.get('HdrText') or name }}</b></p>
<table><tr>{% for c in cols %}<th>{{ c }}</th>{% endfor %}</tr>
{% for r in rows %}<tr>{% for c in keys %}<td>{{ r.get(c, '') }}</td>{% endfor %}</tr>{% endfor %}</table>"""


def _num(v) -> str:
    try:
        return f"{float(v or 0):g}"
    except (TypeError, ValueError):
        return str(v or "")


def _money(v) -> str:
    try:
        return f"{float(v or 0):.2f}"
    except (TypeError, ValueError):
        return str(v or "")


def _date(v) -> str:
    if isinstance(v, (date, datetime)):
        return f"{v:%d-%m-%Y}"
    return str(v or "")[:10]


def _dmy(v) -> str:
    """8-10-2026 (the desktop's day-month-year without leading zeros)."""
    if isinstance(v, (date, datetime)):
        return f"{v.day}-{v.month}-{v.year}"
    t = str(v or "")[:10].split("-")
    return f"{int(t[2])}-{int(t[1])}-{t[0]}" if len(t) == 3 and all(x.isdigit() for x in t) else str(v or "")


def _clock(v) -> str:
    """10:39 AM from a datetime, a time or 'YYYY-MM-DD HH:MM:SS'."""
    if isinstance(v, datetime):
        return v.strftime("%I:%M %p").lstrip("0")
    t = str(v or "").strip()[-8:]
    try:
        return datetime.strptime(t, "%H:%M:%S").strftime("%I:%M %p").lstrip("0")
    except ValueError:
        return str(v or "")


def _logo() -> tuple[str, str]:
    """(file name, folder) of the shop's logo: the first image uploaded in Company Master (Images, shown on the POS first)."""
    try:
        rows = json.loads((UPLOADS / "index.json").read_text())
    except (OSError, ValueError):
        return "", ""
    pics = sorted((r for r in rows if r.get("kind") == "image"), key=lambda r: not r.get("show_on_pos"))
    return (f"{pics[0]['id']}{pics[0]['ext']}", str(UPLOADS)) if pics and (UPLOADS / f"{pics[0]['id']}{pics[0]['ext']}").exists() else ("", "")


def _company(params: dict) -> dict:
    """The shop's header: the job's parameters (any spelling a print option uses), else Company Master."""
    p = {str(k).lower().lstrip("@"): str(v or "").strip() for k, v in (params or {}).items()}
    c = store.COMPANY

    def pick(names: tuple, fallback: str) -> str:
        return next((p[n] for n in names if p.get(n)), "") or str(c.get(fallback) or "").strip()

    addr = [pick((f"add{i}", f"address{i}"), key) for i, key in enumerate(("address", "address2", "address3", "address4"), 1)]
    return {"name": pick(("compname", "companyname"), "name"), "address": [a for a in addr if a],
            "gstin": pick(("gstin", "gstno", "gstinno"), "gstin"),
            "phone": pick(("phno", "phone", "telno", "phoneno"), "phone"),
            "email": pick(("email", "emailid", "email_id", "mailid", "emailaddress"), "email")}


def _pdf(html: str, thermal: bool, images: str = "") -> bytes:
    """HTML to PDF: A4 pages, or one tall page 80 mm wide for a receipt format."""
    def story():
        return pymupdf.Story(html=html, user_css=CSS, archive=pymupdf.Archive(images) if images else None)

    buf = io.BytesIO()
    writer = pymupdf.DocumentWriter(buf)
    if thermal:
        w, margin = THERMAL_WIDTH_PT, 8
        _, filled = story().place(pymupdf.Rect(0, 0, w - 2 * margin, 20000))
        media = pymupdf.Rect(0, 0, w, pymupdf.Rect(filled).y1 + 2 * margin)
        s = story()
        s.place(media + (margin, margin, -margin, -margin))
        s.draw(writer.begin_page(media))
        writer.end_page()
    else:
        media = pymupdf.paper_rect("a4")
        where = media + (36, 36, -36, -36)
        s, more = story(), True
        while more:
            dev = writer.begin_page(media)
            more, _ = s.place(where)
            s.draw(dev)
            writer.end_page()
    writer.close()
    return buf.getvalue()


def _html(template: str, **ctx) -> str:
    return _env.from_string(template).render(num=_num, m=_money, date=_date, dmy=_dmy, clock=_clock, **ctx)


def _doc_id(selection: str) -> int:
    m = re.search(r"=\s*(\d+)\s*$", selection or "")
    if not m:
        raise HTTPException(422, "The selection formula has no document ID to print.")
    return int(m.group(1))


def _tag(selection: str, field: str) -> str:
    m = re.search(rf"\.{field}\}}\s*=\s*'((?:[^']|'')*)'", selection or "", re.I)
    return m.group(1).replace("''", "'") if m else ""


def _one(sql: str, params: tuple) -> dict:
    rows = db.fetchall(sql, params)
    if not rows:
        raise HTTPException(404, "The document is not in the database.")
    return rows[0]


def _sales_invoice(job: dict) -> bytes:
    sid = _doc_id(job["selection"])
    h = _one("SELECT * FROM dbo.TrnHdrSINV WHERE SINVID = ?", (sid,))
    lines = db.fetchall("SELECT d.*, i.ITEMNAME, i.HSNCode FROM dbo.TrnDtlSINV d LEFT JOIN dbo.MstITEM i ON i.ITEMID = d.ITEMID "
                        "WHERE d.SINVID = ? ORDER BY d.SlNo", (sid,))
    logo, folder = _logo()
    return _pdf(_html(SALES_INVOICE, co=_company(job["parameters"]), h=h, lines=lines, logo=logo), True, folder)


def _purchase_bill(job: dict) -> bytes:
    pid = _doc_id(job["selection"])
    h = _one("SELECT * FROM dbo.TrnHdrPB WHERE PBID = ?", (pid,))
    lines = db.fetchall("SELECT d.*, i.ITEMNAME FROM dbo.TrnDtlPB d LEFT JOIN dbo.MstITEM i ON i.ITEMID = d.ITEMID "
                        "WHERE d.PBID = ? ORDER BY d.SlNo", (pid,))
    return _pdf(_html(PURCHASE_BILL, co=_company(job["parameters"]), h=h, lines=lines), False)


def _udr(job: dict) -> tuple[dict, list[dict]]:
    name, user = _tag(job["selection"], "RptName"), _tag(job["selection"], "UserName")
    head = db.fetchall("SELECT * FROM dbo.RptUDRHeading WHERE RptName = ? AND UserName = ?", (name, user))
    rows = db.fetchall("SELECT * FROM dbo.RptUDR WHERE RptName = ? AND UserName = ?", (name, user))
    return (head[0] if head else {}), rows


def _dsr(job: dict) -> bytes:
    head, rows = _udr(job)
    return _pdf(_html(DSR, co=_company(job["parameters"]), p={str(k): v for k, v in job["parameters"].items()},
                      rows=DSR_ROWS, groups=rows, title=head.get("HdrText", "")), False)


def _custom(job: dict) -> bytes:
    head, rows = _udr(job)
    keys = [f"Fld{n}" for n in range(1, 21) if str(head.get(f"Fld{n}") or "").strip()]
    return _pdf(_html(TABLE, co=_company(job["parameters"]), head=head, name=_tag(job["selection"], "RptName"),
                      cols=[head[k] for k in keys], keys=keys, rows=rows), len(keys) > 8)


def _stock(job: dict) -> bytes:
    user = _tag(job["selection"], "UserName")
    rows = db.fetchall("SELECT s.*, i.ITEMNAME FROM dbo.RptStk s LEFT JOIN dbo.MstITEM i ON i.ITEMID = s.itemID "
                       "WHERE s.UserName = ? ORDER BY i.ITEMNAME", (user,))
    fields = ("OPQty", "PBQty", "SRQty", "InvQty", "SDQty", "ClQty", "ClValue")
    total = {f: sum(float(r.get(f) or 0) for r in rows) for f in fields}
    return _pdf(_html(STOCK, co=_company(job["parameters"]), p=job["parameters"], rows=rows, t=total,
                      values="without clvalue" not in job["rpt"].lower()), False)


def render(job: dict) -> bytes:
    name = str(job.get("rpt") or "").lower()
    if name.startswith(("fmtsinv", "fmtspinv")):
        return _sales_invoice(job)
    if name.startswith("fmtpb"):
        return _purchase_bill(job)
    if name.startswith("rptdsr"):
        return _dsr(job)
    if name.startswith("rptstkbal"):
        return _stock(job)
    if "RptUDR" in str(job.get("selection") or ""):
        return _custom(job)
    raise HTTPException(501, f"The built-in print has no layout for {job.get('rpt')} yet "
                             "(only the real Crystal Reports bridge, RPT_BRIDGE_URL, can print this format).")


def print_direct(job: dict) -> None:
    raise HTTPException(409, "Printing straight to a printer needs the real Crystal Reports bridge (RPT_BRIDGE_URL). "
                             "Use Preview and print from the browser.")
