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
    "purchase_bill": {"name": "Purchase Bill",
                      "line": {"PBQTY": "qty", "PBRATE": "rate", "STKQTY": "qty", "NOOFCASES": "cases", "NOOFBOTTLES": "bottles",
                               "CGSTPER": "item.cgst_per", "SGSTPER": "item.sgst_per"},
                      "header": {"TCS": "totals.tcs", "OTHAMT": "totals.other"}},
}


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
