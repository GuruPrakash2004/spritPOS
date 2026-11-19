// downloadXls / downloadXlsRows write a real .xlsx: read it back with the same Excel reader the imports use.
import { describe, expect, it, vi } from 'vitest';
import { downloadXls, downloadXlsRows, setExportCompany } from './xlsExport';

async function capture(run) {
  let blob, name;
  URL.createObjectURL = vi.fn(b => { blob = b; return 'blob:x'; });
  URL.revokeObjectURL = vi.fn();
  vi.spyOn(HTMLAnchorElement.prototype, 'click').mockImplementation(function () { name = this.download; });
  await run();
  return { blob, name };
}
const sheetRows = async (blob) => {
  const XLSX = await import('@e965/xlsx');
  const book = XLSX.read(new Uint8Array(await blob.arrayBuffer()), { type: 'array' });
  return XLSX.utils.sheet_to_json(book.Sheets[book.SheetNames[0]], { header: 1, defval: '' });
};

describe('Excel export', () => {
  it('a report is a .xlsx with the company, title, header and data', async () => {
    setExportCompany('Test Shop');
    const { blob, name } = await capture(() => downloadXls('Stock list', 'Stock', '01-01-2026', [{ key: 'a', label: 'Item' }, { key: 'b', label: 'Qty' }], [{ a: 'Rum <1>', b: 5 }]));
    expect(name).toBe('Stock_list.xlsx');
    expect(await sheetRows(blob)).toEqual([['Test Shop', ''], ['Stock (01-01-2026)', ''], ['Item', 'Qty'], ['Rum <1>', 5]]);
  });
  it('a template is a plain .xlsx of rows', async () => {
    const { blob, name } = await capture(() => downloadXlsRows('Tpl', [['ItemID', 'Name'], [1, 'A & B']]));
    expect(name).toBe('Tpl.xlsx');
    expect(await sheetRows(blob)).toEqual([['ItemID', 'Name'], [1, 'A & B']]);
  });
});
