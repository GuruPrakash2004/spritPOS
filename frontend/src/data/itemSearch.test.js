import { describe, expect, it } from 'vitest';
import { mergeStockItems } from './stockData';
import { searchItems } from './itemSearch';

const item = (code, name, qoh) => ({
  code, name, group: 'WHISKY', brand: 'B', package: '750ml', liqtype: 'IML', qoh, mrp: 100, purchase_rate: 80, active: true, short_name: name.slice(0, 4),
});

describe('searchItems', () => {
  mergeStockItems([item('A1', 'Alpha Whisky', 5), item('B2', 'Beta Rum', 0), item('C3', 'Gamma Gin', 2)]);

  it('lists every item in stock for an empty box, and never an item with no stock', () => {
    expect(searchItems('').map(i => i.code)).toEqual(['A1', 'C3']);
  });

  it('does not list an item with no stock by name, but an exact code is still found (the bill then says how much is in stock)', () => {
    expect(searchItems('beta')).toEqual([]);
    expect(searchItems('B2').map(i => i.code)).toEqual(['B2']);
  });

  it('ranks an exact code first and matches the short name', () => {
    expect(searchItems('c3')[0].code).toBe('C3');
    expect(searchItems('gamm').map(i => i.code)).toEqual(['C3']);
  });

  it('finds an item by its alias, typed or scanned, and ranks an exact alias with an exact code', () => {
    mergeStockItems([{ ...item('0000022', 'Chips', 12), aliases: ['0000023', '0000024'] }, item('0000230', 'Other', 4)]);
    expect(searchItems('0000023').map(i => i.code)).toEqual(['0000022']);          // the exact alias first; 0000230 only starts with another code
    expect(searchItems('0000024')[0].code).toBe('0000022');
    expect(searchItems('000002')[0].code).toBe('0000022');                         // part of an alias still finds it
  });

  it('finds an exact alias even when the item has no stock', () => {
    mergeStockItems([{ ...item('Z9', 'Zero Stock', 0), aliases: ['ZZ-ALIAS'] }]);
    expect(searchItems('zz-alias').map(i => i.code)).toEqual(['Z9']);
    expect(searchItems('zz-ali')).toEqual([]);                                     // only an exact match gets past the stock filter
  });
});
