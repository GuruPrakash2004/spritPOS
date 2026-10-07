// Stock sheets for Utilities → Compare Stock (legacy frmUtlCompareStk): read a file, find its columns, compare.
//
// readSheet() takes CSV or an Excel 2003 XML workbook (what this app's and the desktop's "Export to Excel"
// write). Real .xlsx files are zip archives and are not read: save them as CSV first.
// guessColumns() finds the header row and the Item Code / Item Name / Closing Qty / Value columns; the screen
// lets the user change them. compareSheets() gives one row per item with each side's qty and value, the
// differences and the desktop's statuses: Item Not found, Qty Mismatch, Value Mismatch (or OK).
import { parseCsv } from './csv';

 









function readXml(text) {
  const doc = new DOMParser().parseFromString(text, 'application/xml');
  if (doc.getElementsByTagName('parsererror').length) throw new Error('The file is not a readable Excel XML workbook.');
  const out = [];
  for (const row of Array.from(doc.getElementsByTagNameNS('*', 'Row'))) {
    const cells = [];
    for (const c of Array.from(row.getElementsByTagNameNS('*', 'Cell'))) {
      const idx = c.getAttribute('ss:Index') ?? c.getAttributeNS('urn:schemas-microsoft-com:office:spreadsheet', 'Index');
      if (idx) while (cells.length < Number(idx) - 1) cells.push('');
      cells.push(c.getElementsByTagNameNS('*', 'Data')[0]?.textContent ?? '');
    }
    out.push(cells);
  }
  return out;
}

/** The sheet's rows. Throws an Error with a message for the user when the file can't be read. */
export function readSheet(fileName, text) {
  const head = text.slice(0, 400).trimStart();
  if (head.startsWith('PK')) throw new Error(`${fileName} is an .xlsx file. Save it as CSV (or use Export to Excel from the app) and pick it again.`);
  const rows = head.startsWith('<') ? readXml(text) : parseCsv(text);
  if (!rows.length) throw new Error(`No records found in ${fileName}.`);
  return { name: fileName, rows };
}

const find = (cells, re, not) =>
  cells.findIndex(c => re.test(c.trim()) && !(not && not.test(c.trim())));

/** Best guess of the header row and columns (-1 when a column isn't found). */
export function guessColumns(sheet) {
  for (let h = 0; h < Math.min(sheet.rows.length, 15); h++) {
    const cells = sheet.rows[h].map(c => c.toLowerCase());
    const qty = [/^(cl|closing)[ ._-]*(qty|bal|balance|stock)/, /closing/, /^qoh$/, /cl[ ._-]?bal/, /\bqty\b/, /stock/]
      .map(re => find(cells, re, /litre|ltr|value|amt|amount/)).find(i => i !== -1) ?? -1;
    if (qty === -1) continue;
    const code = find(cells, /code/);
    const name = find(cells, /name|item|description|particular/, /code|group/);
    if (code === -1 && name === -1) continue;   // a title row such as "Stock Balance", not the header
    return {
      header: h, qty, code, name,
      value: [/^(cl|closing)[ ._-]*(value|amt|amount)/, /value/, /amount|amt/].map(re => find(cells, re)).find(i => i !== -1) ?? -1,
    };
  }
  return { header: 0, code: 0, name: 1, qty: 2, value: 3 };
}

const num = (s) => {
  const n = Number(String(s ?? '').replace(/[₹,\s]/g, ''));
  return Number.isFinite(n) ? n : 0;
};

/** Items of a sheet (rows after the header; rows without a code or name, like totals, are skipped). */
export function sheetItems(sheet, cols) {
  const byKey = new Map();
  for (const r of sheet.rows.slice(cols.header + 1)) {
    const code = cols.code >= 0 ? (r[cols.code] ?? '').trim() : '';
    const name = cols.name >= 0 ? (r[cols.name] ?? '').trim() : '';
    if (!code && !name) continue;
    if (/^(grand )?total/i.test(code || name)) continue;
    const key = (code || name).toLowerCase();
    const prev = byKey.get(key);
    const qty = num(r[cols.qty]);
    const value = cols.value >= 0 ? num(r[cols.value]) : 0;
    byKey.set(key, prev ? { ...prev, qty: prev.qty + qty, value: prev.value + value } : { key, code, name, qty, value });
  }
  return [...byKey.values()];
}

const round = (n, d = 3) => Math.round(n * 10 ** d) / 10 ** d;

/** One row per item in either sheet, with the desktop screen's statuses. */
export function compareSheets(oldItems, newItems) {
  const older = new Map(oldItems.map(i => [i.key, i]));
  const newer = new Map(newItems.map(i => [i.key, i]));
  const keys = [...new Set([...older.keys(), ...newer.keys()])];
  return keys.map(key => {
    const o = older.get(key);
    const n = newer.get(key);
    const qtyDiff = round((n?.qty ?? 0) - (o?.qty ?? 0));
    const valueDiff = round((n?.value ?? 0) - (o?.value ?? 0), 2);
    const status = !o ? 'Item Not found (Old)' : !n ? 'Item Not found (New)'
      : qtyDiff !== 0 ? 'Qty Mismatch' : valueDiff !== 0 ? 'Value Mismatch' : 'OK';
    return {
      key, code: o?.code || n?.code || '', name: o?.name || n?.name || '',
      oldQty: o ? round(o.qty) : null, newQty: n ? round(n.qty) : null,
      oldValue: o ? round(o.value, 2) : null, newValue: n ? round(n.value, 2) : null,
      qtyDiff, valueDiff, status,
    };
  }).sort((a, b) => (a.status === 'OK' ? 1 : 0) - (b.status === 'OK' ? 1 : 0) || a.code.localeCompare(b.code) || a.name.localeCompare(b.name));
}
