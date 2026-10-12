// Tests for the Purchase Bill's layout from the Doc Designer: the line grid and the header boxes take the designer's captions,
// hidden and mandatory flags (rows of TrnHdrPB), and without a definition the screen is as the web app coded it.
import { render, screen, waitFor } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import PurchaseBill from './index'
import { STOCK_ITEMS } from '../../../data/stockData'

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
