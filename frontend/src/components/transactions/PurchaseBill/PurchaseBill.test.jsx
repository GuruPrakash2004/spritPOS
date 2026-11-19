// Tests for the Purchase Bill's layout from the Doc Designer: the line grid and the header boxes take the designer's captions,
// hidden and mandatory flags (rows of TrnHdrPB), and without a definition the screen is as the web app coded it.
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import PurchaseBill from './index'
import { STOCK_ITEMS } from '../../../data/stockData'
import { toastStore } from '../../common/toastStore'
import * as api from '../../../lib/api'
import { setAuthRights } from '../../../lib/api'
import { setOpeningDraft } from '../../../lib/billDraft'

const bar = () => within(screen.getByRole('toolbar', { name: 'Actions' }))   // the action bar (the supplier's own New button is not it)
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
    if (url.includes('kind=opening_purchase_bill')) return json({ doc_no: '26OP00001' })
    if (url.startsWith('/api/transactions/next-number')) return json({ doc_no: '26/PB-0000001' })
    if (url.startsWith('/api/masters/items')) return json([{ code: 'WH1', name: 'Whisky', package: '750ML', active: true, qoh: 5, purchase_rate: 80, mrp: 120, cgst_per: 9, sgst_per: 9, bottles_per_case: 12, liqtype: 'IML' },
      { code: 'GN0', name: 'Empty Gin', package: '750ML', active: true, qoh: 0, purchase_rate: 90, mrp: 140, cgst_per: 9, sgst_per: 9, bottles_per_case: 12, liqtype: 'IML' }])
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



describe('PurchaseBill opening-stock draft (from Update Item Stock)', () => {
  it("loads the stock sheet items as a GST-free bill with the OP number, and saves nothing by itself", async () => {
    setOpeningDraft({ supplier: 'KSBCL', remarks: 'Opening stock (Update Item Stock)', lines: [{ item_code: 'WH1', qty: 5, rate: 80 }] })
    render(<PurchaseBill />)
    expect(await screen.findByText(/Opening stock from Update Item Stock: nothing is saved yet/)).toBeInTheDocument()
    expect(await screen.findByDisplayValue('26OP00001')).toBeInTheDocument()           // the OP-series number
    expect(screen.getByPlaceholderText('Select or type supplier')).toHaveValue('KSBCL')
    expect(await screen.findByDisplayValue('Whisky 750ML')).toBeInTheDocument()       // the item is on the grid
    // As in the exe, the lines already on the bill can be changed before the invoice value is typed (only ADDING an item needs it)
    const qty = await screen.findByLabelText('PB Qty row 1')
    expect(qty).toBeEnabled()
    await userEvent.clear(qty)
    await userEvent.type(qty, '9')
    expect(await screen.findByText(/1 items · 9 bottles/)).toBeInTheDocument()
    const saved = vi.mocked(fetch).mock.calls.filter(([, init]) => init?.method === 'POST')
    expect(saved).toHaveLength(0)                                                      // nothing saved before the user enters the invoice value
  })
})


describe('PurchaseBill Add item', () => {
  it('opens the two-pane Add Line Item from the button and keeps the older lookup for F4', async () => {
    const user = userEvent.setup()
    render(<PurchaseBill />)
    await screen.findByRole('toolbar', { name: 'Actions' })
    await user.click(bar().getByRole('button', { name: /^New/ }))   // the screen opens in View mode; New starts a bill
    await user.type(await screen.findByPlaceholderText('Select or type supplier'), 'KSBCL')
    await user.type(screen.getByLabelText(/Supplier invoice no/), 'INV-1')
    await user.type(screen.getByLabelText(/Invoice value/), '1000')
    await user.click(screen.getByRole('button', { name: /^Add item/ }))
    expect(await screen.findByRole('dialog', { name: 'Add Line Item' })).toBeInTheDocument()
    expect(screen.getByText('Whisky 750ML')).toBeInTheDocument()            // an item with stock is listed
    expect(screen.getByText('Empty Gin 750ML')).toBeInTheDocument()         // so is an item with 0 stock (an arriving item has none yet)
    await user.keyboard('{Escape}')
    await waitFor(() => expect(screen.queryByRole('dialog', { name: 'Add Line Item' })).not.toBeInTheDocument())
    await user.keyboard('{F4}')
    expect(await screen.findByRole('dialog', { name: 'Add Item Into Purchase Bill' })).toBeInTheDocument()
  })
})


describe('PurchaseBill modes', () => {
  it('opens in View mode (read-only, the bar has New), New leaves only Save and Cancel, and Cancel goes back to View mode', async () => {
    const user = userEvent.setup()
    render(<PurchaseBill />)
    expect(await screen.findByText(/View mode\. Press New to enter a purchase bill/)).toBeInTheDocument()
    expect(screen.getByPlaceholderText('Select or type supplier')).toBeDisabled()           // nothing can be typed in View mode
    expect(screen.queryByRole('button', { name: /^Save/ })).not.toBeInTheDocument()
    expect(bar().getByRole('button', { name: /^Edit/ })).toBeDisabled()                     // no saved bill is on screen yet

    await user.click(bar().getByRole('button', { name: /^New/ }))
    expect(screen.getByPlaceholderText('Select or type supplier')).toBeEnabled()
    expect(screen.getAllByRole('button', { name: /^Save/ }).length).toBeGreaterThan(0)       // Save and Cancel on the bar
    expect(bar().queryByRole('button', { name: /^New/ })).not.toBeInTheDocument()           // New, Edit, List are gone while entering
    expect(bar().queryByRole('button', { name: /^List/ })).not.toBeInTheDocument()

    await user.click(screen.getAllByRole('button', { name: /^Cancel/ }).at(-1))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    expect(await screen.findByText(/View mode\. Press New/)).toBeInTheDocument()            // back in View mode
    expect(screen.getByPlaceholderText('Select or type supplier')).toBeDisabled()
    expect(bar().getByRole('button', { name: /^New/ })).toBeInTheDocument()
  })
})


describe('PurchaseBill Edit', () => {
  it('shows a saved bill in View mode; Edit opens that bill with only Save and Cancel; Cancel goes back to the bill in View mode', async () => {
    setAuthRights(['*'])
    const bill = { doc_no: '26PB00005', date: new Date().toLocaleDateString('en-CA'), time: '10:00:00', supplier: 'KSBCL', supplier_inv_no: 'INV-5', supplier_inv_date: '2026-11-19',
      supplier_inv_value: 400, purchase_type: 'Purchase', status: 'SAVED', totals: { basic: 400, net: 400, tcs: 0, qty: 5 }, lines: [{ item_code: 'WH1', qty: 5, rate: 80 }] }
    vi.spyOn(api, 'listDocuments').mockResolvedValue([bill])
    const user = userEvent.setup()
    render(<PurchaseBill />)
    await screen.findByRole('toolbar', { name: 'Actions' })
    await user.click(await screen.findByRole('button', { name: 'Last record' }))
    expect(await screen.findByDisplayValue('INV-5')).toBeDisabled()                          // the saved bill, read-only
    await user.click(bar().getByRole('button', { name: /^Edit/ }))
    expect(await screen.findByText(/Editing purchase bill 26PB00005/)).toBeInTheDocument()
    expect(screen.getByDisplayValue('INV-5')).toBeEnabled()                                  // now it can be changed
    expect(bar().getByRole('button', { name: /^Save(?! &)/ })).toBeInTheDocument()           // Save (and Save & print) and Cancel, as in New
    expect(bar().getByRole('button', { name: /^Save & print/ })).toBeInTheDocument()
    expect(bar().getByRole('button', { name: /^Cancel/ })).toBeInTheDocument()
    expect(bar().queryByRole('button', { name: /^Edit/ })).not.toBeInTheDocument()
    await user.click(bar().getByRole('button', { name: /^Cancel/ }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    expect(await screen.findByDisplayValue('INV-5')).toBeDisabled()                          // back to the bill in View mode
    expect(bar().getByRole('button', { name: /^Edit/ })).toBeEnabled()
  })
})
