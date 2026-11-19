// Tests for the History column: it lists the latest documents, hides held and shows cancelled ones, opens a row to
// show its lines, finds by text, and can be hidden and shown again.
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import WithHistory from './DocHistoryPanel'
import * as api from '../../lib/api'
import { toastStore } from '../common/toastStore'

const TODAY = new Date().toLocaleDateString('en-CA')   // the panel lists only recent days, so the fixtures must be recent
const DOCS = [
  { doc_no: '26/S-0000001', date: TODAY, time: '10:05:00', customer: 'CASH', status: 'SAVED', totals: { net: 185 }, lines: [{ item_code: 'KF500', qty: 1 }] },
  { doc_no: '26/S-0000002', date: TODAY, time: '11:20:00', customer: 'RAMESH', status: 'CANCELLED', totals: { net: 740 }, lines: [] },
  { doc_no: '26/H-0000001', date: TODAY, time: '12:00:00', customer: 'CASH', status: 'HOLD', totals: { net: 50 }, lines: [] },
]

describe('WithHistory', () => {
  beforeEach(() => {
    localStorage.clear()
    api.setAuthRights(['*'])
    vi.spyOn(api, 'listDocuments').mockResolvedValue(DOCS)
  })

  it('is closed on a small screen and opens from the tab on the right', async () => {
    render(<WithHistory kind="sales_invoice"><p>the bill</p></WithHistory>)
    expect(screen.getByText('the bill')).toBeInTheDocument()
    expect(screen.queryByRole('complementary', { name: 'History' })).not.toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Show history' }))
    expect(await screen.findByText('26/S-0000002')).toBeInTheDocument()
    expect(api.listDocuments).toHaveBeenCalledWith('sales_invoice', { from_date: expect.stringMatching(/^\d{4}-\d{2}-\d{2}$/) })   // only recent documents
  })

  it('lists newest first, leaves out held bills, and shows lines when a row is opened', async () => {
    render(<WithHistory kind="sales_invoice"><p>x</p></WithHistory>)
    await userEvent.click(screen.getByRole('button', { name: 'Show history' }))
    const numbers = (await screen.findAllByText(/^26\/S-/)).map(n => n.textContent)
    expect(numbers).toEqual(['26/S-0000002', '26/S-0000001'])
    expect(screen.queryByText('26/H-0000001')).not.toBeInTheDocument()
    expect(screen.getByText('Cancelled')).toBeInTheDocument()
    await userEvent.click(screen.getByText('26/S-0000001'))
    expect(screen.getByText(/KF500 × 1/)).toBeInTheDocument()
  })

  it('finds by text and can be hidden again', async () => {
    render(<WithHistory kind="sales_invoice"><p>x</p></WithHistory>)
    await userEvent.click(screen.getByRole('button', { name: 'Show history' }))
    await screen.findByText('26/S-0000001')
    await userEvent.type(screen.getByLabelText('Find in history'), 'ramesh')
    await waitFor(() => expect(screen.queryByText('26/S-0000001')).not.toBeInTheDocument())
    expect(screen.getByText('26/S-0000002')).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Hide history' }))
    expect(screen.getByRole('button', { name: 'Show history' })).toBeInTheDocument()
  })

  it('refreshes when a document is saved anywhere', async () => {
    render(<WithHistory kind="purchase_bill"><p>x</p></WithHistory>)
    await userEvent.click(screen.getByRole('button', { name: 'Show history' }))
    await screen.findByText('26/S-0000001')
    const calls = api.listDocuments.mock.calls.length
    window.dispatchEvent(new Event(api.DOCS_CHANGED))
    await waitFor(() => expect(api.listDocuments.mock.calls.length).toBe(calls + 1))
  })

  it('has the action bar: New runs the own new-document action of the screen', async () => {
    api.setAuthRights(['*'])
    const onNew = vi.fn()
    render(<WithHistory kind="purchase_bill" onNew={onNew}><p>x</p></WithHistory>)
    await userEvent.click(screen.getByRole('button', { name: /^New/ }))
    expect(onNew).toHaveBeenCalledTimes(1)
  })

  it('opens the List window with the documents and shows the selected one read-only', async () => {
    render(<WithHistory kind="purchase_bill" title="bills"><p>x</p></WithHistory>)
    await userEvent.click(screen.getByRole('button', { name: /^List/ }))
    expect(await screen.findByText('Total Records: 2 / 2')).toBeInTheDocument()   // the held bill is not listed
    await userEvent.click(screen.getByText('26/S-0000001'))
    await userEvent.click(screen.getByRole('button', { name: 'Open Selected' }))
    expect(await screen.findByText('KF500')).toBeInTheDocument()   // the document's line
    expect(screen.getByRole('button', { name: /Print/ })).toBeInTheDocument()
  })

  it('has no First / Previous / Search / Next / Last record buttons on any document screen', () => {
    for (const kind of ['sales_invoice', 'purchase_bill', 'sales_return']) {
      const { unmount } = render(<WithHistory kind={kind} title="docs"><p>x</p></WithHistory>)
      for (const name of ['First record', 'Previous record', 'Next record', 'Last record', 'Search'])
        expect(screen.queryByRole('button', { name })).toBeNull()
      unmount()
    }
  })
})

describe('WithHistory bar', () => {
  it('has no First / Previous / Next / Last / Search record buttons (the bill screens go through the List window)', () => {
    api.setAuthRights(['*'])
    vi.spyOn(api, 'listDocuments').mockResolvedValue([])
    render(<WithHistory kind="purchase_bill"><p>x</p></WithHistory>)
    for (const name of ['First record', 'Previous record', 'Next record', 'Last record', 'Search']) expect(screen.queryByRole('button', { name })).not.toBeInTheDocument()
    expect(screen.getByRole('button', { name: /^List/ })).toBeInTheDocument()
  })
})

describe('Sales Invoice', () => {
  it('has no action bar: saved bills are found, edited and cancelled in its Re-print / Edit window', () => {
    api.setAuthRights(['*'])
    vi.spyOn(api, 'listDocuments').mockResolvedValue([])
    render(<WithHistory kind="sales_invoice" onNew={() => undefined} onEdit={() => undefined}><p>x</p></WithHistory>)
    expect(screen.queryByRole('toolbar')).not.toBeInTheDocument()
  })
})


describe('WithHistory record buttons and Save / Cancel', () => {
  const BILLS = ['A1', 'A2', 'A3'].map((n, i) => ({ doc_no: `26PB0000${n.slice(1)}`, date: TODAY, time: `1${i}:00:00`, supplier: 'KSBCL', status: 'SAVED', totals: { net: 100 + i }, lines: [] }))
  beforeEach(() => {
    localStorage.clear()
    api.setAuthRights(['*'])
    vi.spyOn(api, 'listDocuments').mockResolvedValue(BILLS)
  })

  it('walks through the saved bills with First / Previous / Next / Last and shows each one', async () => {
    const user = userEvent.setup()
    render(<WithHistory kind="purchase_bill" title="purchase bills" navigate><p>the bill</p></WithHistory>)
    expect(await screen.findByText('3 bills')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'First record' }))
    expect(await screen.findByText(/Purchase bills 26PB00001 · 1 \/ 3/)).toBeInTheDocument()
    await user.click(screen.getAllByRole('button', { name: 'Next record' }).at(-1))
    expect(await screen.findByText(/26PB00002 · 2 \/ 3/)).toBeInTheDocument()
    await user.click(screen.getAllByRole('button', { name: 'Last record' }).at(-1))
    expect(await screen.findByText(/26PB00003 · 3 \/ 3/)).toBeInTheDocument()
    expect(screen.getAllByRole('button', { name: 'Next record' }).at(-1)).toBeDisabled()
    await user.click(screen.getAllByRole('button', { name: 'Previous record' }).at(-1))
    expect(await screen.findByText(/26PB00002 · 2 \/ 3/)).toBeInTheDocument()
  })

  it('shows Save and Cancel instead of the record buttons while a bill is being entered', async () => {
    const onSave = vi.fn(); const onCancel = vi.fn()
    const user = userEvent.setup()
    const onSavePrint = vi.fn()
    render(<WithHistory kind="purchase_bill" title="purchase bills" navigate mode="Edit" onSave={onSave} onSavePrint={onSavePrint} onCancel={onCancel}><p>the bill</p></WithHistory>)
    await user.click(screen.getByRole('button', { name: /^Save & print/ }))
    expect(onSavePrint).toHaveBeenCalledTimes(1)
    await user.click(screen.getByRole('button', { name: /^Save(?! &)/ }))
    await user.click(screen.getByRole('button', { name: /^Cancel/ }))
    expect(onSave).toHaveBeenCalledTimes(1)
    expect(onCancel).toHaveBeenCalledTimes(1)
    expect(screen.queryByRole('button', { name: 'First record' })).not.toBeInTheDocument()
  })
})


describe('History panel: Cancel Bill', () => {
  it('offers Cancel Bill on an open row of a purchase bill and cancels that bill after the confirmation', async () => {
    api.setAuthRights(['*'])
    vi.spyOn(api, 'listDocuments').mockResolvedValue([{ doc_no: '26OP00001', date: TODAY, time: '10:00:00', supplier: 'KSBCL', status: 'SAVED', totals: { net: 100 }, lines: [] },
      { doc_no: '26OP00002', date: TODAY, time: '11:00:00', supplier: 'KSBCL', status: 'CANCELLED', totals: { net: 50 }, lines: [] }])
    const cancel = vi.spyOn(api, 'cancelSavedDocument').mockResolvedValue({})
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
    const user = userEvent.setup()
    render(<WithHistory kind="purchase_bill" title="purchase bills"><p>x</p></WithHistory>)
    await user.click(screen.getByRole('button', { name: 'Show history' }))
    await user.click(await screen.findByText('26OP00002'))
    const panel = within(screen.getByRole('complementary', { name: 'History' }))   // (the action bar has a Cancel Bill button of its own)
    expect(panel.queryByRole('button', { name: 'Cancel Bill' })).not.toBeInTheDocument()   // already cancelled: nothing to cancel
    await user.click(screen.getByText('26OP00001'))
    await user.click(panel.getByRole('button', { name: 'Cancel Bill' }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    await waitFor(() => expect(cancel).toHaveBeenCalledWith('purchase_bill', '26OP00001'))
  })
})


describe('WithHistory with a screen that shows the bill itself', () => {
  it('hands the bill to onView (no dialog) when a record button is pressed', async () => {
    api.setAuthRights(['*'])
    const bills = ['1', '2', '3'].map((n, i) => ({ doc_no: `26PB0000${n}`, date: TODAY, time: `1${i}:00:00`, supplier: 'KSBCL', status: 'SAVED', totals: { net: 100 }, lines: [] }))
    vi.spyOn(api, 'listDocuments').mockResolvedValue(bills)
    const onView = vi.fn()
    const user = userEvent.setup()
    render(<WithHistory kind="purchase_bill" title="purchase bills" navigate onView={onView} focusDoc=""><p>x</p></WithHistory>)
    await screen.findByText('3 bills')
    await user.click(screen.getByRole('button', { name: 'Last record' }))
    expect(onView).toHaveBeenCalledWith(expect.objectContaining({ doc_no: '26PB00003' }))
    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()       // the screen shows it itself, not a dialog
  })
})
