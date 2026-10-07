// Tests for Re Write Stock: the differences are listed per tab, and Re-Write runs only after the
// confirmation and the waiting popup, then shows the fresh check.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import ReWriteStock from './ReWriteStock'

const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

const tabs = (salesRows) => [
  { key: 'purchase', caption: 'Purchase', rows: [] },
  { key: 'sales_invoice', caption: 'Sales Invoice', rows: salesRows },
  { key: 'stock_deduction', caption: 'Stock Deduction', rows: [] },
  { key: 'sales_return', caption: 'Sales Return', rows: [] },
  { key: 'ledger', caption: 'Ledger vs QOH', rows: [] },
]
const MISMATCH = { item_code: 'VIN-750', item_name: 'VINTAGE WHISKY', trn_stock: 2, sys_stock: 0, diff: 2 }

let fetchMock
const posts = () => fetchMock.mock.calls.filter(([, init]) => init?.method === 'POST')

beforeEach(() => {
  fetchMock = vi.fn(async (_url, init) => init?.method === 'POST'
    ? json({ rewritten: true, message: 'Successfully re-Written Stock!', items_changed: 1, check: { tabs: tabs([]), rewrite_required: false } })
    : json({ web: { tabs: tabs([MISMATCH]), rewrite_required: true }, desktop: null, db_connected: false }))
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})

afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('ReWriteStock', () => {
  it('lists the mismatching items of the tab that has them', async () => {
    render(<ReWriteStock />)

    expect(await screen.findByText('VINTAGE WHISKY')).toBeInTheDocument()
    expect(screen.getByRole('tab', { name: /Sales Invoice/ })).toHaveAttribute('aria-selected', 'true')
    expect(screen.getByText('Re-Write required')).toBeInTheDocument()
    expect(screen.getByText(/desktop database's own check isn't shown/)).toBeInTheDocument()
  })

  it('re-writes after the confirmation and the waiting popup', async () => {
    const user = userEvent.setup()
    render(<ReWriteStock />)
    await screen.findByText('VINTAGE WHISKY')

    await user.click(screen.getByRole('button', { name: /^Re-Write$/ }))
    await user.click(await screen.findByRole('button', { name: /confirm|yes|ok|continue/i }))

    await waitFor(() => expect(posts()).toHaveLength(1))
    expect(toastStore.defer).toHaveBeenCalledWith('refresh', 'Stock will be re-written when this closes.', expect.any(String))
    expect(await screen.findByText('Re-Write not Required')).toBeInTheDocument()
  })

  it('changes nothing when the waiting popup is cancelled', async () => {
    vi.mocked(toastStore.defer).mockResolvedValue(false)
    const user = userEvent.setup()
    render(<ReWriteStock />)
    await screen.findByText('VINTAGE WHISKY')

    await user.click(screen.getByRole('button', { name: /^Re-Write$/ }))
    await user.click(await screen.findByRole('button', { name: /confirm|yes|ok|continue/i }))

    await waitFor(() => expect(toastStore.defer).toHaveBeenCalled())
    expect(posts()).toHaveLength(0)
  })
})
