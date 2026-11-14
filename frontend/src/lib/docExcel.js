// Excel import for a stock document's lines (the exe's svtImport / svtTemplate on the party-document forms, shown when the Doc Designer's
// "Allow Import/Export" is on for the document): a sheet whose first row is "Item Code, Qty, Rate"; one line per row below it.
// Rows of an item already listed are skipped (the exe skips them); the screen decides what an unknown or inactive item means.

import { readTableFile } from './readTableFile';

export const TEMPLATE_HEAD = ['Item Code', 'Qty', 'Rate'];

const norm = (s) => String(s ?? '').replace(/[\s_]+/g, '').toUpperCase();

/** The template the exe offers ("PI_Template_<yyyyMMddHHmmss>.xlsx"): a sheet with only the three headings. */
export async function downloadDocTemplate(shortForm) {
  const XLSX = await import('@e965/xlsx');
  const book = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(book, XLSX.utils.aoa_to_sheet([TEMPLATE_HEAD]), 'Sheet1');
  const now = new Date();
  const stamp = [now.getFullYear(), now.getMonth() + 1, now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds()].map((n, i) => String(n).padStart(i ? 2 : 4, '0')).join('');
  XLSX.writeFile(book, `${shortForm}_Template_${stamp}.xlsx`);
}

/** Rows of a picked file as [{ code, qty, rate }]. Throws "Please Select the valid template" when the first row is not Item Code / Qty / Rate. */
export async function readDocLines(file) {
  const rows = await readTableFile(file);
  const head = (rows[0] ?? []).slice(0, 3).map(norm).join(',');
  if (head !== 'ITEMCODE,QTY,RATE') throw new Error('Please Select the valid template');
  const seen = new Set();
  const out = [];
  for (const r of rows.slice(1)) {
    const code = String(r[0] ?? '').trim().toUpperCase();
    if (!code || seen.has(code)) continue;
    seen.add(code);
    out.push({ code, qty: Number(String(r[1] ?? '').replace(/,/g, '')) || 0, rate: r[2] === '' || r[2] == null ? null : Number(String(r[2]).replace(/,/g, '')) });
  }
  return out;
}
