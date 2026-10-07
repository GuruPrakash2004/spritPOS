// Tests for the live item catalogue: reloads must never move an item (open bills hold its index).
import { beforeEach, describe, expect, it } from 'vitest'
import { STOCK_ITEMS, litresPerBottle, mergeStockItems, sellable, stockIndex } from './stockData'


const item = (code, qoh, active = true) => ({
  code, name: `${code} NAME`, hsn_code: '', group: 'WHISKY', brand: 'B', liqtype: 'IML', uom: 'BTL', package: '750ml',
  bottles_per_case: 12, mrp: 100, purchase_rate: 80, qoh, short_name: '', cgst_per: 0, sgst_per: 0, rol: 0, roq: 0, active,
})

beforeEach(() => { STOCK_ITEMS.length = 0 })

describe('stock catalogue', () => {
  it('keeps every item at its index when reloaded in a different order', () => {
    mergeStockItems([item('A', 5), item('B', 7)])
    mergeStockItems([item('C', 1), item('B', 3), item('A', 4)])

    expect(STOCK_ITEMS.map(i => i.code)).toEqual(['A', 'B', 'C'])
    expect(STOCK_ITEMS[stockIndex('B')].clQty).toBe(3)
  })

  it('keeps items that disappear so open bills still resolve them', () => {
    mergeStockItems([item('A', 5), item('B', 7)])
    mergeStockItems([item('B', 7)])

    expect(stockIndex('A')).toBe(0)
  })

  it('hides inactive items from pickers but keeps their index', () => {
    mergeStockItems([item('A', 5, false), item('B', 7)])

    expect(sellable().map(i => [i.code, i.stock])).toEqual([['B', 1]])
  })

  it.each([
    ['750ml', 0.75], ['180 ML', 0.18], ['2 LTR', 2], ['1L', 1], ['650', 0.65], ['', 0],
  ])('litresPerBottle(%j) = %d', (pkg, litres) => {
    expect(litresPerBottle(pkg)).toBeCloseTo(litres)
  })
})
