// Opening a saved Stock Deduction / Expense Entry from the List (Edit Selected) and saving the change as an update of the same document.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import { setAuthRights } from '../../lib/api'
import { StockDeduction } from './StockTransactions'
import ExpenseEntry from './ExpenseEntry'

const json = (body, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
const ITEM = { code: 'BLND-750', name: 'BLENDERS PRIDE', hsn_code: '', group: 'WHISKY', brand: 'B', liqtype: 'IML', uom: 'BTL', package: '750ml', bottles_per_case: 12, mrp: 900, purchase_rate: 500,
  qoh: 50, short_name: '', cgst_per: 0, sgst_per: 0, rol: 0, roq: 0, active: true }
let puts

beforeEach(() => {
  puts = []
  setAuthRights(['*'])
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.restoreAllMocks(); vi.unstubAllGlobals() })

const serve = (listUrl, saved, extra = {}) => vi.stubGlobal('fetch', vi.fn(async (url, init) => {
  if (init?.method === 'PUT') { puts.push([url, JSON.parse(String(init.body))]); return json({ ...saved, totals: { qty: 1 }, total: 10 }) }
  if (url.startsWith(listUrl) && (!init?.method || init.method === 'GET')) return json([saved])
  if (url.startsWith('/api/masters/items')) return json([ITEM])
  for (const [prefix, body] of Object.entries(extra)) if (url.startsWith(prefix)) return json(body)
  return json([])
}))

describe('editing saved documents', () => {
  it('a Stock Deduction opens from the List, with its reason, and is saved as an update of the same document', async () => {
    const saved = { doc_no: '26/SD-0000001', date: '2026-10-30', reason: 'Leakage', remarks: 'cracked', status: 'SAVED',
      lines: [{ item_code: 'BLND-750', item_name: 'BLENDERS PRIDE', cases: 0, bottles: 4, qty: 4, rate: 500, amount: 2000 }], totals: { qty: 4 } }
    serve('/api/transactions/stock-deduction', saved)
    const user = userEvent.setup()
    render(<StockDeduction />)
    await user.click(screen.getByRole('button', { name: /^List/ }))
    await user.click(await screen.findByText('26/SD-0000001'))
    await user.click(screen.getByRole('button', { name: 'Edit Selected' }))
    expect(await screen.findByText(/Editing Stock Deduction 26\/SD-0000001/)).toBeInTheDocument()
    expect(screen.getByLabelText(/^Reason/)).toHaveValue('Leakage')
    await user.keyboard('{Control>}s{/Control}')
    await waitFor(() => expect(puts).toHaveLength(1))
    expect(puts[0][0]).toBe('/api/transactions/stock-deduction/26/SD-0000001')
    expect(puts[0][1]).toMatchObject({ date: '2026-10-30', reason: 'Leakage', remarks: 'cracked', lines: [{ item_code: 'BLND-750', bottles: 4 }] })
  })

  it('an Expense Entry opens from the List and is saved as an update of the same entry (receipt-only lines are left out)', async () => {
    const saved = { doc_no: '26//00007', date: '2026-10-30', paid_from: 'CASH', status: 'SAVED', total: 50,
      lines: [{ ledger: 'TEA', amount: 50, narration: 'tea' }, { ledger: 'FUND', amount: 0, receipt: 500, narration: 'top up' }] }
    serve('/api/transactions/expense', saved, { '/api/masters/ledgers': [{ code: 'TEA', name: 'Tea', group: 'EXPENSES' }] })
    const user = userEvent.setup()
    render(<ExpenseEntry />)
    await user.click(screen.getByRole('button', { name: /^List/ }))
    await user.click(await screen.findByText('26//00007'))
    await user.click(screen.getByRole('button', { name: 'Edit Selected' }))
    expect(await screen.findByText(/Editing expense 26\/\/00007/)).toBeInTheDocument()
    await user.keyboard('{Control>}s{/Control}')
    await waitFor(() => expect(puts).toHaveLength(1))
    expect(puts[0][0]).toBe('/api/transactions/expense/26//00007')
    expect(puts[0][1]).toMatchObject({ date: '2026-10-30', paid_from: 'CASH', lines: [{ ledger: 'TEA', amount: 50, narration: 'tea' }] })
    expect(puts[0][1].lines).toHaveLength(1)
  })
})
