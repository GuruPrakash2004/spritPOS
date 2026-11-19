// The sidebar follows the desktop's menu (INVENTORY > MASTERS ..., TRANSACTIONS, REPORTS, TOOLS, UTILITIES): same groups, same order.
import { describe, expect, it } from 'vitest'
import { MODULE_GROUPS, NAV_TREE } from './modules'

const find = (nodes, label) => {
  for (const n of nodes) {
    if (typeof n === 'string' || n.name) continue
    if (n.label === label) return n
    const hit = find(n.children ?? [], label)
    if (hit) return hit
  }
  return null
}
const names = (node) => node.children.map(c => (typeof c === 'string' ? c : c.name ?? c.label))
const leaves = (nodes) => nodes.flatMap(n => (typeof n === 'string' ? [n] : n.name ? [n.name] : leaves(n.children ?? [])))

describe('sidebar order (the desktop menu)', () => {
  it('starts with Dashboard and ends with About', () => {
    expect(NAV_TREE[0]).toBe('Dashboard')
    expect(NAV_TREE.at(-1)).toBe('About')
  })

  it('lists the masters as the exe does', () => {
    const masters = find(NAV_TREE, 'Masters')
    expect(masters.children.map(c => c.label)).toEqual(['Company Details', 'Item Related', 'Customer Related', 'Supplier Related', 'Others'])
    expect(names(find(NAV_TREE, 'Item Related')).slice(0, 3)).toEqual(['Item Master', 'Item Group Master', 'UOM Master'])
    expect(names(find(NAV_TREE, 'Others')).slice(0, 12)).toEqual(['State Master', 'Currency Master', 'Sales Person Master', 'Location Master', 'Ledger Master',
      'Ledger Group Master', 'State City Country', 'Tax Master', 'Bank Master', 'Sales Counter Master', 'Package Master', 'Brand Master'])
  })

  it('lists transactions, reports, tools and utilities as the exe does', () => {
    expect(names(find(NAV_TREE, 'Transactions')).slice(0, 8)).toEqual(['Sales Invoice', 'Purchase Bill', 'Expense Entry', 'Stock Deduction', 'Purchase Indent',
      'Sales Return', 'Quotation', 'Physical Stock Entry'])
    expect(names(find(NAV_TREE, 'Reports')).slice(0, 8)).toEqual(['Reports Dashboard', 'Stock Balance Report', 'Stock Ledger', 'Stock Ageing', 'Stock Level / Reorder',
      'Daily Summary Report', 'Brand-wise Sales', 'Profit Margin'])
    expect(names(find(NAV_TREE, 'Tools'))).toEqual(['SQL Query Execution', 'Re Write Stock'])
    expect(names(find(NAV_TREE, 'Utilities')).slice(0, 8)).toEqual(['Admin Master', 'Global Settings', 'Remove Duplicate Invoice', 'Replace and Remove Duplicate items',
      'Merge Items', 'Compare Stock', 'ADMIN Audit Log', 'Bulk Master Update'])
  })

  it('shows the exe captions for Security and the report names', () => {
    const utilities = find(NAV_TREE, 'Utilities').children
    expect(utilities[0]).toEqual({ name: 'Admin Master', label: 'Security' })
    expect(find(NAV_TREE, 'Reports').children[0]).toEqual({ name: 'Reports Dashboard', label: 'Customised Reports...' })
  })

  it('still places every screen of the registry (none is lost)', () => {
    const placed = new Set(leaves(NAV_TREE))
    const missing = MODULE_GROUPS.flatMap(g => g.modules).filter(n => !placed.has(n))
    expect(missing).toEqual([])
  })
})
