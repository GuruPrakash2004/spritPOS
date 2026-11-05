// Tests for the Purchase Bill's layout from the Doc Designer: the line grid and the header boxes take the designer's captions,
// hidden and mandatory flags (rows of TrnHdrPB), and without a definition the screen is as the web app coded it.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import PurchaseBill from './index'
import { STOCK_ITEMS } from '../../../data/stockData'
import { toastStore } from '../../common/toastStore'
import { setAuthRights } from '../../../lib/api'

const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

const row = (code, extra) => ({ code, caption: code, section: 'Detail', sub_section: '', order: 1, hidden: false, readonly: false, mandatory: false, modelled: true, web_key: '', ...extra })
const head = (code, web_key, extra) => row(code, { section: 'Header', web_key, ...extra })

let screenDef
beforeEach(() => {
  STOCK_ITEMS.length = 0
  screenDef = null
  vi.stubGlobal('fetch', vi.fn(async (url) => {
    if (url.startsWith('/api/designer/docs')) {
      return json(screenDef ? [{ doc_id: 7, name: 'Purchase Bill', type: 'Transaction', short_form: 'PB', header_table: 'TrnHdrPB', detail_table: 'TrnDtlPB', master_path: '' }] : [])
    }
    if (url.startsWith('/api/designer/screen/7')) return json({ doc_id: 7, name: 'Purchase Bill', type: 'Transaction', header_table: 'TrnHdrPB', fields: screenDef })
    if (url.startsWith('/api/transactions/next-number')) return json({ doc_no: '26/PB-0000001' })
    if (url.startsWith('/api/settings/values')) return json({})
    return json([])
  }))
})
afterEach(() => vi.unstubAllGlobals())

describe('PurchaseBill from the Doc Designer', () => {
  it('shows the web app\'s own grid and header with no definition', async () => {
    render(<PurchaseBill />)
    expect(await screen.findByRole('columnheader', { name: 'Item code' })).toBeInTheDocument()
    expect(screen.getByRole('columnheader', { name: 'Rate' })).toBeInTheDocument()
    expect(screen.getByRole('columnheader', { name: 'Comp. cess' })).toBeInTheDocument()
    expect(screen.getByRole('textbox', { name: 'Remarks' })).toBeInTheDocument()
  })

  it('takes captions and hidden columns from the designer, but never hides Qty or Rate', async () => {
    screenDef = [
      row('PBRATE', { caption: 'Purchase Rate', order: 2 }),
      row('PBQTY', { caption: 'Quantity', hidden: true, order: 1 }),   // a kept column: renamed, not hidden
      row('COMPCESS', { hidden: true, order: 3 }),
      head('RMKS', 'remarks', { caption: 'Narration', hidden: true, order: 1 }),
      head('PURCHASETYPE', 'purchase_type', { caption: 'Type of purchase', order: 2 }),
    ]
    render(<PurchaseBill />)
    expect(await screen.findByRole('columnheader', { name: 'Purchase Rate' })).toBeInTheDocument()
    expect(screen.getByRole('columnheader', { name: 'Quantity' })).toBeInTheDocument()
    expect(screen.queryByRole('columnheader', { name: 'Comp. cess' })).not.toBeInTheDocument()
    expect(screen.queryByRole('textbox', { name: /remarks|narration/i })).not.toBeInTheDocument()   // hidden by the designer
    await waitFor(() => expect(screen.getByLabelText('Type of purchase')).toBeInTheDocument())
  })
})

describe('PurchaseBill edit', () => {
  it('opens a saved bill from the List and saves the change as an update of the same bill', async () => {
    setAuthRights(['*'])
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
    const item = { code: 'BLND-750', name: 'BLENDERS PRIDE', hsn_code: '', group: 'WHISKY', brand: 'B', liqtype: 'IML', uom: 'BTL', package: '750ml', bottles_per_case: 12, mrp: 900, purchase_rate: 500,
      qoh: 5, short_name: '', cgst_per: 0, sgst_per: 0, rol: 0, roq: 0, active: true }
    const saved = { doc_no: '26/PB-0000001', date: '2026-10-30', supplier: 'KSBCL', supplier_inv_no: 'INV9', supplier_inv_date: '2026-10-29', supplier_inv_value: 1000, purchase_type: 'Purchase', remarks: '',
      lines: [{ item_code: 'BLND-750', item_name: 'BLENDERS PRIDE', qty: 2, rate: 500, amount: 1000 }], totals: { qty: 2, basic: 1000, tcs: 0, cess: 0, net: 1000 } }
    const puts = []
    fetch.mockImplementation(async (url, init) => {
      if (init?.method === 'PUT') { puts.push([url, JSON.parse(String(init.body))]); return json({ ...saved, totals: { qty: 2, net: 1000 } }) }
      if (url.startsWith('/api/transactions/purchase-bill') && (!init?.method || init.method === 'GET')) return json([saved])
      if (url.startsWith('/api/masters/items')) return json([item])
      if (url.startsWith('/api/masters/suppliers')) return json([{ code: 'KSBCL', name: 'KSBCL Depot', address: 'Bengaluru', gstin: '', city: '', active: true }])
      if (url.startsWith('/api/transactions/next-number')) return json({ doc_no: '26/PB-0000002' })
      if (url.startsWith('/api/settings/values')) return json({})
      return json([])
    })
    const user = userEvent.setup()
    render(<PurchaseBill />)
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))
    await user.click(screen.getByRole('button', { name: /^List/ }))
    await user.click(await screen.findByText('26/PB-0000001'))
    await user.click(screen.getByRole('button', { name: 'Edit Selected' }))
    expect(await screen.findByText(/Editing purchase bill 26\/PB-0000001/)).toBeInTheDocument()
    await user.keyboard('{Control>}s{/Control}')
    await waitFor(() => expect(puts).toHaveLength(1))
    expect(puts[0][0]).toBe('/api/transactions/purchase-bill/26/PB-0000001')
    expect(puts[0][1]).toMatchObject({ date: '2026-10-30', supplier: expect.stringContaining('KSBCL'), supplier_inv_no: 'INV9', purchase_type: 'Purchase', lines: [{ item_code: 'BLND-750', bottles: 2, rate: 500 }] })
    expect(fetch.mock.calls.some(([, init]) => init?.method === 'POST')).toBe(false)
  })
})

