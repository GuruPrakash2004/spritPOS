// Reads the Item Master import sheet (ItemImport_Template.xlsx, saved as .xlsx / .xls / .csv) into item records. Shared by the
// "Import Item Data From Excel" screen. The sheet has a row of hints on top and the column names under it: the row holding ITEMCODE is the
// header. Returns { records: [{ row, extras }], error } where `row` uses the API's field names and `extras` the database's other item columns.

import { ITEM_IMPORT_ALIASES, ITEM_IMPORT_COLUMNS, ITEM_IMPORT_EXTRAS, ITEM_IMPORT_NUMERIC } from '../components/masters/masterConfigs';

export function parseItemSheet(table) {
  if (!table.length) return { records: [], error: 'The file is empty.' };
  const at = table.findIndex(r => r.some(c => String(c).trim().toUpperCase() === 'ITEMCODE'));
  if (at === -1) return { records: [], error: 'No ITEMCODE column found. Use the Template button.' };
  const index = new Map(table[at].map((h, i) => [ITEM_IMPORT_ALIASES[String(h).trim().toUpperCase()] ?? String(h).trim().toUpperCase(), i]));
  const missing = ITEM_IMPORT_COLUMNS.filter(([col, , required]) => required && !index.has(col.toUpperCase())).map(([col]) => col);
  if (missing.length) return { records: [], error: `Missing columns: ${missing.join(', ')}. Use the Template button.` };
  const cell = (cells, col) => (index.has(col) ? String(cells[index.get(col)] ?? '').trim() : '');
  const records = table.slice(at + 1)
    .filter(cells => cell(cells, 'ITEMCODE') !== '' || cell(cells, 'ITEMNAME') !== '')
    .map(cells => {
      const row = {};
      ITEM_IMPORT_COLUMNS.forEach(([col, key]) => {
        if (!index.has(col.toUpperCase())) return;   // an optional column the file doesn't have
        const raw = cell(cells, col.toUpperCase());
        row[key] = ITEM_IMPORT_NUMERIC.has(key) ? Number(raw.replace(/,/g, '')) || 0 : raw;
      });
      const extras = {};
      for (const col of ITEM_IMPORT_EXTRAS) {
        const raw = cell(cells, col);
        if (raw !== '') extras[col] = col === 'NONSTKABLE' ? raw : raw.replace(/,/g, '');
      }
      // The exe fills these in when the sheet leaves them empty (frmImportItemXLData.ValidateItemEntry): no package "-", no brand "-", Non-Stockable "N".
      if (index.has('PKGCODE') && row.package === '') row.package = '-';
      if (index.has('BRANDCODE') && row.brand === '') row.brand = '-';
      if (extras.NONSTKABLE === undefined) extras.NONSTKABLE = 'N';
      return { row, extras };
    });
  return { records, error: '' };
}

/** The request body for one record (what POST /masters/items/import takes). The short name stays as typed (the exe leaves it empty too). */
export const importBody = ({ row, extras }) => ({
  ...row, qoh: 0, active: true, ...(Object.keys(extras).length ? { extras } : {}),
});
