// Quotation: an item that is not in the list is refused inline; a known item is priced from its MRP and sent.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { setAuthRights } from '../../lib/api'
import { toastStore } from '../common/toastStore'
import Quotation from './Quotation'

const json = (body, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
let posted
let sent
const QTN = { id: 4, doc_no: '26//00004', date: '2026-10-30', customer: 'CASH', remarks: 'old', status: 'SAVED', total: 600, qty: 2,
  lines: [{ item_code: 'WH1', item_name: 'Whisky 750', qty: 2, rate: 300, amount: 600 }] }

beforeEach(() => {
  setAuthRights(['*'])
  posted = []
  sent = []
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  vi.stubGlobal('fetch', vi.fn(async (url, init) => {
    if (init?.method === 'PUT' || url.endsWith('/cancel')) { sent.push([init.method, url, init.body ? JSON.parse(init.body) : null]); return json({ ...QTN, status: url.endsWith('/cancel') ? 'CANCELLED' : 'SAVED' }) }
    if (/quotation\/4$/.test(url)) return json(QTN)
    if (/quotation\?/.test(url)) return json([{ id: 4, doc_no: '26//00004', date: '2026-10-30', customer: 'CASH', qty: 2, total: 600, status: 'SAVED' }])
    if (init?.method === 'POST') { posted.push(JSON.parse(init.body)); return json({ id: 1, doc_no: '26//00001', total: 900 }, 201) }
    if (url.includes('/masters/items')) return json([{ code: 'WH1', name: 'Whisky 750', mrp: 300, active: true }])
    return json([])
  }))
})
afterEach(() => { vi.restoreAllMocks(); vi.unstubAllGlobals() })

describe('Quotation', () => {
  it('refuses an item that is not in the list, inline', async () => {
    const user = userEvent.setup()
    render(<Quotation />)
    await user.type(screen.getByLabelText('Item row 1'), 'nonsense'); await user.type(screen.getByLabelText('Quantity row 1'), '2')
    await user.click(screen.getByRole('button', { name: /^Save/ }))
    expect(await screen.findByText(/Row 1: pick an item from the list/)).toBeInTheDocument()
    expect(posted).toHaveLength(0)
  })

  it('prices from the MRP and sends the quotation', async () => {
    const user = userEvent.setup()
    render(<Quotation />)
    await user.type(await screen.findByLabelText('Item row 1'), 'WH1'); await user.type(screen.getByLabelText('Quantity row 1'), '3')
    await waitFor(() => expect(screen.getByText('900.00', { selector: 'td' })).toBeInTheDocument())
    await user.click(screen.getByRole('button', { name: /^Save/ }))
    await waitFor(() => expect(posted).toHaveLength(1))
    expect(posted[0].lines).toEqual([{ item_code: 'WH1', qty: 3 }])
  })

  it('loads a saved quotation to edit and sends the change to the same quotation', async () => {
    const user = userEvent.setup()
    render(<Quotation />)
    await user.click(await screen.findByText('26//00004'))
    await user.click(await screen.findByRole('button', { name: /Edit/ }))
    expect(await screen.findByText(/Editing Quotation 26\/\/00004/)).toBeInTheDocument()
    await user.clear(screen.getByLabelText('Quantity row 1')); await user.type(screen.getByLabelText('Quantity row 1'), '5')
    await user.click(screen.getByRole('button', { name: /^Save changes/ }))
    await waitFor(() => expect(sent).toHaveLength(1))
    expect(sent[0][0]).toBe('PUT'); expect(sent[0][1]).toMatch(/quotation\/4$/); expect(sent[0][2].lines[0].qty).toBe(5)
  })

  it('cancels a quotation after the user agrees', async () => {
    const user = userEvent.setup()
    render(<Quotation />)
    await user.click(await screen.findByText('26//00004'))
    await user.click(await screen.findByRole('button', { name: /Cancel quotation/i }))
    await user.click(await screen.findByRole('button', { name: /^Confirm/ }))
    await waitFor(() => expect(sent.some(x => x[1].endsWith('/4/cancel'))).toBe(true))
  })
})
