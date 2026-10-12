"""A saved document's columns worked out from the Doc Designer's formulas, the way the desktop fills them.

The designer says which header / detail / footer / total columns are formulas (UtlDocDsgrFlds.FldFormula: line amounts,
GST, totals, the amount in words ...). The web app's own code decides the few inputs a document is made of (quantity, rate,
cases, the item's GST %, TCS ...); this module hands those to the formula engine (app/formulas.py) under the designer's
column names and returns every computed column, ready to be written into the desktop's tables next to the typed ones. A
column the designer gives a formula therefore follows the designer: change the formula there and the next saved document
uses it. Each document's inputs are listed in DOCS below: data, not screen code.
"""
from decimal import Decimal

from . import designer, formulas, schema_meta, store

# kind -> the designer document name, the detail columns taken from a line (column: source) and the header columns taken
# from the document (column: source). A source is a key of the line / document, or "item.<key>" (the item's master row).
DOCS = {
    "purchase_indent": {"name": "Purchase Indent",
                        "line": {"PIQTY": "qty", "PIRATE": "rate", "STKQTY": "qty", "NOOFCASES": "cases", "NOOFBOTTLES": "bottles", "MRP": "item.mrp"},
                        "header": {}},
    "sales_return": {"name": "Sales Return",
                     "line": {"SRQTY": "qty", "SRRATE": "rate", "STKQTY": "qty", "LTRPERBOT": "item.litres_per_bottle"},
                     "header": {}},
    "stock_deduction": {"name": "Stock Deduction",
                        "line": {"STKDEDQTY": "qty", "STKQTY": "qty", "LTRPERBOT": "item.litres_per_bottle"},
                        "header": {}},
    "sales_invoice": {"name": "Sales Invoice",
                      "line": {"SINVQTY": "qty", "SINVRATE": "rate", "STKQTY": "qty", "NOOFCASES": "cases", "NOOFBOTTLES": "bottles",
                               "CGSTPER": "item.cgst_per", "SGSTPER": "item.sgst_per", "MRP": "item.mrp"},
                      "header": {}},
    "purchase_bill": {"name": "Purchase Bill",
                      "line": {"PBQTY": "qty", "PBRATE": "rate", "STKQTY": "qty", "NOOFCASES": "cases", "NOOFBOTTLES": "bottles",
                               "CGSTPER": "item.cgst_per", "SGSTPER": "item.sgst_per"},
                      "header": {"TCS": "totals.tcs", "OTHAMT": "totals.other"}},
}


# The columns of a priced line the designer's formulas decide (when the designer gives that column a formula): designer
# column -> the key of the priced line it fills. Used by the bills' pricing (routers/transactions._price_lines), so the
# GST on a bill follows the designer's formulas and the built-in maths is only the fallback.
LINE_OUT = {"sales_invoice": {"SINVAMT": "amount", "CGSTAMT": "cgst", "SGSTAMT": "sgst"},
            "purchase_bill": {"PBAMT": "amount", "CGSTAMT": "cgst", "SGSTAMT": "sgst"}}


def line_amounts(kind: str, lines: list[dict]) -> list[dict]:
    """For each priced line (qty, rate, cases, bottles, item_code ...): {"amount"|"cgst"|"sgst": value} for the columns
    the designer gives a formula, rounded to paise. A line (or a whole bill) the formulas can't work out gets {}, so the
    caller keeps its own figure."""
    out_cols = LINE_OUT.get(kind)
    if not out_cols or not lines:
        return [{} for _ in lines]
    try:
        doc_id = doc_id_of(DOCS[kind]["name"])
        wanted = {c.lower() for c in out_cols}
        # Nothing to work out unless the designer gives one of these columns a formula: skip the formula engine (every save asks).
        if doc_id is None or not any(f.get("formula") and str(f.get("code") or "").lower() in wanted
                                     for f in (schema_meta.screen(doc_id) or {}).get("fields", [])):
            return [{} for _ in lines]
        rows = compute(kind, {"lines": lines, "totals": {}})["rows"]
    except Exception:   # noqa: BLE001 - a formula problem must never stop a bill being priced
        return [{} for _ in lines]
    result = []
    for line, row in zip(lines, rows):
        low = {str(k).lower(): v for k, v in row.items()}
        own = {key: round(float(low[col.lower()]), 2) for col, key in out_cols.items()
               if isinstance(low.get(col.lower()), (int, float))}
        # A priced line (qty and rate above zero) whose amount formula gives nothing or 0 is a formula the web can't feed
        # (it reads a column the bill has no value for): keep the built-in figures rather than bill ₹0.
        if own and (own.get("amount") or 0) <= 0 and (line.get("qty") or 0) * (line.get("rate") or 0) > 0:
            own = {}
        result.append(own)
    return result + [{} for _ in range(len(lines) - len(result))]


def _source(spec: str, line: dict | None, doc: dict):
    if spec.startswith("item."):
        item = store.ITEMS.get((line or {}).get("item_code")) or {}
        return item.get(spec[5:])
    if spec.startswith("totals."):
        return (doc.get("totals") or {}).get(spec[7:])
    return (line or {}).get(spec)


def doc_id_of(name: str) -> int | None:
    return next((int(d["DocID"]) for d in store.UTL_DOCS.values() if str(d.get("DocName") or "").strip() == name), None)


def compute(kind: str, doc: dict) -> dict:
    """{"header": {column: value}, "rows": [{column: value}], "problems": [...]} for a saved document.

    Values are floats (numbers) or text; empty when the document kind has no designer rows, the database is offline or no
    column has a formula, so a caller can always merge the result into what it writes."""
    spec = DOCS.get(kind)
    doc_id = doc_id_of(spec["name"]) if spec else None
    if not spec or doc_id is None:
        return {"header": {}, "rows": [], "problems": []}
    fields = [f for f in (schema_meta.screen(doc_id) or {}).get("fields", []) if f.get("formula")]
    if not fields:
        return {"header": {}, "rows": [], "problems": []}
    every = (schema_meta.screen(doc_id) or {}).get("fields", [])
    lines = doc.get("lines") or []
    rows = [{col: _source(src, line, doc) for col, src in spec["line"].items()} for line in lines]
    header = {**(doc.get("extras") or {}), **{col: _source(src, None, doc) for col, src in spec["header"].items()}}   # typed inputs, then what the designer added
    result = formulas.compute(every, header, rows, {"SVTSYSNAME": "WEB", "SVTUSERCODE": str(doc.get("added_by") or "")})
    plain = lambda v: float(v) if isinstance(v, Decimal) else v   # noqa: E731
    return {"header": {k: plain(v) for k, v in result["values"].items() if v is not None},
            "rows": [{k: plain(v) for k, v in r.items() if v is not None} for r in result["rows"]],
            "problems": result["problems"]}
