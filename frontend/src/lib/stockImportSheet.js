// Reads the Update Item Stock sheet (the exe's StockImport template) into rows. Used by the Update Item Stock screen (Compare Stock has its own reader, stockSheet.js).

const norm = (s) => String(s ?? '').toUpperCase().replace(/[^A-Z0-9]/g, '');
// Rows of the sheet: the heading row holds ITEMCODE; its first three headings must be ITEMCODE, ITEMNAME, UOMCode (the exe: "Invalid Template...");
// the items start under it and end at the first empty ItemCode. Columns by position, as the exe: A code, B name, C UOM, D stock, E rate.
export function toRows(table) {
  const at = table.findIndex(r => norm(r[0]) === 'ITEMCODE');
  const head = at < 0 ? [] : table[at].slice(0, 3).map(norm);
  if (at < 0 || head[1] !== 'ITEMNAME' || !['UOMCODE', 'UOM'].includes(head[2])) {
    throw new Error('Invalid Template... Use the Template button: its heading row must start ITEMCODE, ITEMNAME, UOMCode, Stock, Rate.');
  }
  const rows = [];
  for (const r of table.slice(at + 1)) {
    const code = String(r[0] ?? '').trim();
    if (!code) break;
    rows.push({ code, name: String(r[1] ?? '').trim(), uom: String(r[2] ?? '').trim(), stock: String(r[3] ?? '').trim(), rate: String(r[4] ?? '').trim(), error: '', fields: [] });
  }
  return rows;
}

