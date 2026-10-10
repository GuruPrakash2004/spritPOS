// Reads a spreadsheet the user picks for an import into rows of text (an array of arrays), whatever Excel saved:
// .xls (Excel 97-2003 or the Excel XML this app exports), .xlsx, or plain text such as .csv. The Excel reader is loaded
// only when needed. Everything is read in the browser; nothing is sent anywhere.

import { parseCsv } from './csv';

const isText = (name) => /\.(csv|txt)$/i.test(name);

/** Rows of the file's first sheet, every cell as text. Throws an Error with a message for the user when it can't be read. */
export async function readTableFile(file) {
  if (isText(file.name)) return parseCsv(await file.text());
  try {
    const XLSX = await import('@e965/xlsx');
    const book = XLSX.read(await file.arrayBuffer(), { type: 'array' });
    const sheet = book.Sheets[book.SheetNames[0]];
    if (!sheet) throw new Error('empty');
    return XLSX.utils.sheet_to_json(sheet, { header: 1, defval: '', raw: false, blankrows: false })
      .map(r => r.map(c => String(c ?? '')))
      .filter(r => r.some(c => c.trim() !== ''));
  } catch {
    throw new Error(`${file.name} could not be read as an Excel file. Save it as .xls or .xlsx and pick it again.`);
  }
}
