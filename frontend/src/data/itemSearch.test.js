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

  it('does not find an item with no stock by name or code', () => {
    expect(searchItems('beta')).toEqual([]);
    expect(searchItems('B2')).toEqual([]);
  });

  it('ranks an exact code first and matches the short name', () => {
    expect(searchItems('c3')[0].code).toBe('C3');
    expect(searchItems('gamm').map(i => i.code)).toEqual(['C3']);
  });
});
