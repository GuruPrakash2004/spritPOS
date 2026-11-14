// Excel import of document lines: the first row must be Item Code / Qty / Rate; duplicate items are skipped.
import { describe, expect, it } from 'vitest';
import { readDocLines } from './docExcel';

const file = (text, name = 'lines.csv') => new File([text], name, { type: 'text/csv' });

describe('readDocLines', () => {
  it('reads item, qty and rate and skips a repeated item', async () => {
    const rows = await readDocLines(file('Item Code,Qty,Rate\nvin-750,12,640\nVIN-750,3,640\nRC-750,6,\n'));
    expect(rows).toEqual([{ code: 'VIN-750', qty: 12, rate: 640 }, { code: 'RC-750', qty: 6, rate: null }]);
  });
  it('accepts ItemCode as one word, as the exe\'s own template writes it', async () => {
    expect((await readDocLines(file('ItemCode,Qty,Rate\nA1,2,5\n')))[0]).toMatchObject({ code: 'A1', qty: 2 });
  });
  it('refuses any other first row with the exe\'s message', async () => {
    await expect(readDocLines(file('Code,Quantity,Price\nA1,2,5\n'))).rejects.toThrow('Please Select the valid template');
  });
});
