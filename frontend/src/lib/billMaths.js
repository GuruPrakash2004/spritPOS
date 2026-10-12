// A bill's line maths from the Doc Designer's formulas: the line amount, CGST and SGST of Sales Invoice / Purchase Bill are
// worked out from the formula the designer gives that column (SINVAMT / PBAMT, CGSTAMT, SGSTAMT), with the same grammar and
// rounding as the server (lib/formulas.js, backend/app/formulas.py; the server prices the saved bill with the same
// formulas: backend/app/doc_engine.py LINE_OUT). A column with no formula, a formula this module can't read, or no
// definition at all returns nothing for that column, and the screen's own maths stands.
//
//   const amounts = formulaAmounts('sales_invoice', screen, [{ item_code, qty, rate, cases, bottles }], itemOf);
//   amounts[i] → { amount?, cgst?, sgst? }

import { compute, isSupported } from './formulas';

const LINE_OUT = {
  sales_invoice: { SINVAMT: 'amount', CGSTAMT: 'cgst', SGSTAMT: 'sgst' },
  purchase_bill: { PBAMT: 'amount', CGSTAMT: 'cgst', SGSTAMT: 'sgst' },
};

const none = (lines) => lines.map(() => ({}));
const round2 = (n) => Math.round((n + Number.EPSILON) * 100) / 100;

export function formulaAmounts(kind, screen, lines, itemOf) {
  const outCols = LINE_OUT[kind];
  const spec = screen?.inputs?.line;
  if (!outCols || !spec || !lines.length) return none(lines);
  // Only formulas this module can work out take part; the others are left as plain inputs.
  const fields = (screen.fields ?? []).map(f => (f.formula && !isSupported(f.formula) ? { ...f, formula: '' } : f));
  const wanted = Object.keys(outCols).map(c => c.toLowerCase());
  if (!fields.some(f => f.formula && String(f.section).toLowerCase() === 'detail' && wanted.includes(f.code.toLowerCase()))) return none(lines);
  const rows = lines.map(line => Object.fromEntries(Object.entries(spec).map(([col, src]) =>
    [col, src.startsWith('item.') ? itemOf?.(line.item_code)?.[src.slice(5)] : line[src]])));
  try {
    const worked = compute(fields, {}, rows, { SVTSYSNAME: 'WEB' });
    return worked.rows.map((row, i) => {
      const low = Object.fromEntries(Object.entries(row).map(([k, v]) => [k.toLowerCase(), v]));
      const own = Object.fromEntries(Object.entries(outCols)
        .filter(([col]) => typeof low[col.toLowerCase()] === 'number' && Number.isFinite(low[col.toLowerCase()]))
        .map(([col, key]) => [key, round2(low[col.toLowerCase()])]));
      // A priced line (qty and rate above zero) whose amount formula gives 0 is a formula this screen can't feed (it reads a
      // column the bill has no value for): keep the built-in figures rather than show ₹0 (the server does the same).
      if (Object.keys(own).length && !(own.amount > 0) && (Number(lines[i]?.qty) || 0) * (Number(lines[i]?.rate) || 0) > 0) return {};
      return own;
    });
  } catch { return none(lines); }
}
