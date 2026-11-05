// The sidebar follows each client's menu: renamed captions and reports (which have no document id) are matched too.
import { describe, expect, it } from 'vitest'
import { hiddenModules } from './menuAliases'

const row = (caption, show) => ({ caption, show })

describe('hiddenModules', () => {
  it('hides a screen whose menu row is not shown, by its own name or by its menu caption', () => {
    const h = hiddenModules([row('Label Printing', false), row('Stock Ledger Report', false), row('Brand Master', true), row('Item Price Maintenance Master', false)])
    expect(h.has('label printing')).toBe(true)
    expect(h.has('stock ledger')).toBe(true)                // the menu calls it "Stock Ledger Report"
    expect(h.has('price list maintenance')).toBe(true)
    expect(h.has('brand master')).toBe(false)
  })
  it('keeps a screen the menu shows or does not mention, and one menu row shows all four Drops reports', () => {
    const h = hiddenModules([row('Drops Reports', true), row('Customised Reports...', true)])
    for (const n of ['drops sales summary', 'drops purchase', 'drops petty cash', 'drops consolidated', 'reports dashboard', 'hourly sales']) expect(h.has(n)).toBe(false)
    expect(hiddenModules([row('Drops Reports', false)]).has('drops consolidated')).toBe(true)
  })
})
