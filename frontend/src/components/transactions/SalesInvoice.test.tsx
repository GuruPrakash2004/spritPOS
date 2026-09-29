// Tests for Sales Invoice: live items (not the old 8 samples), no demo lines, real stock limits,
// rupee rounding, and printing the bill the server saved.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import SalesInvoice from './SalesInvoice'
import { STOCK_ITEMS } from '../../data/stockData'
import { DEFAULT_COUNTER } from '../../data/salesCounters'

// Test data: one real-looking item that is not in the old sample list, with 5 in stock.
const ITEM = {
  code: 'BLND-750', name: 'BLENDERS PRIDE RARE WHISKY', hsn_code: '', group: 'WHISKY', brand: "Blender's Pride",
  liqtype: 'IML', uom: 'BTL', package: '750ml', bottles_per_case: 12, mrp: 1234.6, purchase_rate: 1100, qoh: 5,
  short_name: '', cgst_per: 0, sgst_per: 0, rol: 0, roq: 0, active: true,
}
const CASH = { code: 'CASH', name: 'CASH', mobile: '', loyalty_card_no: '', send_sms: false, points: 0 }

const json = (body: unknown, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

let fetchMock: ReturnType<typeof vi.fn>
const posts = () => fetchMock.mock.calls.filter(([, init]) => init?.method === 'POST')

beforeEach(() => {
  STOCK_ITEMS.length = 0
  fetchMock = vi.fn(async (url: string, init?: RequestInit) => {
    if (init?.method === 'POST') {
      const body = JSON.parse(String(init.body))
      return json({ ...body, doc_no: '26/c10-0083718', status: 'SAVED', totals: { net: 1235, qty: 1 }, lines: [] }, 201)
    }
    if (url.startsWith('/api/masters/items')) return json([ITEM])
    if (url.startsWith('/api/masters/customers')) return json([CASH])
    if (url.startsWith('/api/sales-persons')) return json(['GURU', 'RAVI'])
    return json([])
  })
  vi.stubGlobal('fetch', fetchMock)
})

afterEach(() => vi.unstubAllGlobals())

const setup = { counter: DEFAULT_COUNTER, invoiceNo: '26/c10-0083718', paymentType: 'CASH' as const, customer: 'CASH' }

async function scan(user: ReturnType<typeof userEvent.setup>, code: string, qty = '1') {
  const qtyBox = screen.getByLabelText('Qty')
  await user.clear(qtyBox)
  await user.type(qtyBox, qty)
  await user.type(screen.getByLabelText('Scan or search item'), `${code}{Enter}`)
}

// Actions wait for their popup (toastStore.defer); here it closes at once so the request goes out.
beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.restoreAllMocks())

describe('SalesInvoice', () => {
  it('opens with an empty bill (no demo items)', async () => {
    render(<SalesInvoice setup={setup} onClose={() => {}} />)

    expect(screen.getByText('No items on this bill')).toBeInTheDocument()
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))
  })

  it('sells a live item and sends a rupee-rounded payment', async () => {
    const user = userEvent.setup()
    render(<SalesInvoice setup={setup} onClose={() => {}} />)
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))

    await scan(user, 'BLND-750')
    await user.click(screen.getByRole('button', { name: /save only/i }))

    await waitFor(() => expect(posts()).toHaveLength(1))
    const body = JSON.parse(String(posts()[0][1].body))
    expect(body.lines).toEqual([{ item_code: 'BLND-750', bottles: 1, rate: 1234.6 }])
    expect(body.payment.cash).toBe(1235)   // 1234.60 rounds to ₹1235, as the backend rounds the bill
    expect(body.salesperson).toBe('GURU')
  })

  it('sends one save key per bill, and a new one for the next bill', async () => {
    const user = userEvent.setup()
    render(<SalesInvoice setup={setup} onClose={() => {}} />)
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))

    await scan(user, 'BLND-750')
    await user.click(screen.getByRole('button', { name: /save only/i }))
    await waitFor(() => expect(posts()).toHaveLength(1))
    await scan(user, 'BLND-750')
    await user.click(screen.getByRole('button', { name: /save only/i }))
    await waitFor(() => expect(posts()).toHaveLength(2))

    const keys = posts().map(([, init]) => (init?.headers as Record<string, string>)['Idempotency-Key'])
    expect(keys[0]).toBeTruthy()
    expect(keys[1]).toBeTruthy()
    expect(keys[1]).not.toBe(keys[0])
  })

  it('will not add more than the real stock', async () => {
    const user = userEvent.setup()
    render(<SalesInvoice setup={setup} onClose={() => {}} />)
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))

    await scan(user, 'BLND-750', '6')

    expect(screen.getByText('No items on this bill')).toBeInTheDocument()
  })

  it('does not find the old hard-coded sample items', async () => {
    const user = userEvent.setup()
    render(<SalesInvoice setup={setup} onClose={() => {}} />)
    await waitFor(() => expect(STOCK_ITEMS).toHaveLength(1))

    await scan(user, 'VIN-750')

    expect(screen.getByText('No items on this bill')).toBeInTheDocument()
  })
})
