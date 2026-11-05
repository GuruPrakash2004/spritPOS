// Tests for the History column: it lists the latest documents, hides held and shows cancelled ones, opens a row to
// show its lines, finds by text, and can be hidden and shown again.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import WithHistory from './DocHistoryPanel'
import * as api from '../../lib/api'

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
    expect(api.listDocuments).toHaveBeenCalledWith('sales_invoice')
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
