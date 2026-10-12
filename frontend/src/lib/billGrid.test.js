import { describe, expect, it } from 'vitest';
import { applyGridDefinition } from './billGrid';

const BASE = [['sl', 'Sl No', null, 78], ['item', 'Item', null, 260], ['qty', 'Qty', 'SINVQTY', 78], ['rate', 'MRP', 'SINVRATE', 88], ['discPct', 'Disc.%', 'DISCPER', 72], ['amount', 'Amount', 'SINVAMT', 100]];
const KEEP = new Set(['sl', 'item', 'qty', 'rate']);
const detail = (code, extra) => ({ code, section: 'Detail', caption: code, order: 1, hidden: false, readonly: false, mandatory: false, ...extra });

describe('applyGridDefinition', () => {
  it('is the web grid when there is no definition', () => {
    expect(applyGridDefinition(BASE, null, KEEP).map(c => c.id)).toEqual(['sl', 'item', 'qty', 'rate', 'discPct', 'amount']);
  });

  it('takes the designer caption, drops a hidden column and never drops a kept one', () => {
    const screen = { fields: [detail('SINVRATE', { caption: 'Rate', hidden: true, order: 4 }), detail('DISCPER', { caption: 'Disc %', hidden: true, order: 5 }), detail('SINVAMT', { caption: 'Line Amount', order: 6 })] };
    const cols = applyGridDefinition(BASE, screen, KEEP);
    expect(cols.map(c => c.id)).toEqual(['sl', 'item', 'qty', 'rate', 'amount']);   // discPct hidden; rate kept
    expect(cols.find(c => c.id === 'rate').caption).toBe('Rate');
    expect(cols.find(c => c.id === 'amount').caption).toBe('Line Amount');
  });

  it('puts the coded columns in the designer order and marks read-only ones', () => {
    const screen = { fields: [detail('SINVAMT', { order: 1 }), detail('DISCPER', { order: 2, readonly: true }), detail('SINVRATE', { order: 3 }), detail('SINVQTY', { order: 4 })] };
    const cols = applyGridDefinition(BASE, screen, KEEP);
    expect(cols.map(c => c.id)).toEqual(['sl', 'item', 'amount', 'discPct', 'rate', 'qty']);
    expect(cols.find(c => c.id === 'discPct').locked).toBe(true);
  });

  it('ignores header rows', () => {
    const screen = { fields: [{ code: 'SINVQTY', section: 'Header', caption: 'X', order: 1, hidden: true }] };
    expect(applyGridDefinition(BASE, screen, KEEP)).toHaveLength(6);
  });
});
