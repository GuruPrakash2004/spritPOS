// Update Item Stock: the chosen file's name shows, Load from Excel lists the rows, a wrong cell gets a red box, Import waits until every row is
// right, and the import saves nothing: it opens Purchase Bill with the stock as an opening-stock draft.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import UpdateItemStock from './UpdateItemStock'
import { toRows } from '../../lib/stockImportSheet'
import { takeOpeningDraft } from '../../lib/billDraft'
import { OPEN_MODULE } from '../../lib/nav'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
let posted

beforeEach(() => {
  posted = []
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  vi.stubGlobal('fetch', vi.fn(async (url, init) => {
    if (String(url).includes('/masters/items')) return json([{ code: 'WH1', name: 'Whisky', active: true, uom: 'BTL', purchase_rate: 80, mrp: 120 }])
    const body = JSON.parse(init.body)
    if (body.dry_run === false) { posted.push(body); return json({ ok: true, doc_no: '26OP00001', imported: body.rows.length }) }
    return json({ ok: false, importable: 1, rows: body.rows.map((r, i) => (r.item_code === 'BAD' ? { row: i + 1, error: 'Item not found', fields: ['code'] } : { row: i + 1, error: '', fields: [] })) })
  }))
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

const sheet = (...lines) => new File([['hint,,,valid numbers only', 'ITEMCODE,ITEMNAME,UOMCode,Stock,Rate', ...lines].join('\n')], 'stock.csv', { type: 'text/csv' })

describe('toRows', () => {
  it('finds the heading row under the hint row, reads A..E by position and stops at the first empty code', () => {
    const rows = toRows([['', '', '', 'valid numbers only'], ['ITEMCODE', 'ITEMNAME', 'UOMCode', 'Stock', ''], ['A', 'Alpha', 'BTL', '5', '10'], ['', '', '', '', ''], ['Z', 'After', 'BTL', '1', '']])
    expect(rows.map(r => [r.code, r.stock, r.rate])).toEqual([['A', '5', '10']])
  })
  it('refuses a sheet whose headings are not the template\'s', () => {
    expect(() => toRows([['Code', 'Name', 'Unit', 'Qty']])).toThrow(/Invalid Template/)
  })
})

describe('UpdateItemStock', () => {
  it('shows the chosen file, boxes the wrong cell in red, keeps Import disabled, and imports after the bad row is removed', async () => {
    const user = userEvent.setup()
    render(<UpdateItemStock />)
    await user.upload(screen.getByTestId('uis-picker'), sheet('WH1,Whisky,BTL,5,100', 'BAD,Unknown,BTL,2,50'))
    expect(screen.getByLabelText('Select Excel File')).toHaveValue('stock.csv')          // the selected file's name stays visible
    await user.click(screen.getByRole('button', { name: /Load from Excel/ }))
    expect(await screen.findByText('Item not found', { selector: 'td' })).toBeInTheDocument()
    expect(screen.getByLabelText('Total Number of Records Loaded')).toHaveTextContent('2')
    expect(screen.getByText('BAD')).toHaveAttribute('aria-invalid', 'true')                // only the code cell is boxed
    expect(screen.getByText('WH1')).not.toHaveAttribute('aria-invalid')
    expect(screen.getByRole('button', { name: /Import data to database/ })).toBeDisabled()
    await user.click(screen.getByRole('button', { name: /Remove erroneous records/ }))
    await waitFor(() => expect(screen.queryByText('BAD')).not.toBeInTheDocument())
    const opened = vi.fn()
    window.addEventListener(OPEN_MODULE, opened)
    await user.click(screen.getByRole('button', { name: /Import data to database/ }))
    await waitFor(() => expect(opened).toHaveBeenCalled())
    window.removeEventListener(OPEN_MODULE, opened)
    expect(opened.mock.calls[0][0].detail).toBe('Purchase Bill')                          // the Purchase Bill screen opens ...
    expect(posted).toHaveLength(0)                                                         // ... and nothing was saved
    expect(takeOpeningDraft()).toMatchObject({ supplier: 'KSBCL', lines: [{ item_code: 'WH1', qty: 5, rate: 80 }] })   // Include Rate is off: the item's purchase rate
  })
})
