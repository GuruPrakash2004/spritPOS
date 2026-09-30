// Tests for the ADMIN Audit Log: entries load with their item lines, and a To date before the From date is
// shown under the field without asking the server.
import { render, screen, fireEvent } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import AdminAuditLog from './AdminAuditLog'

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })

const ROWS = [{
  id: 1, at: '2026-09-29T10:15:00', screen: 'Sales Invoice', action: 'Added', main_fld: '26/c10-0083718', user_code: 'ADMIN',
  earlier: '', changed: 'net 1700', items: [
    { code: 'VIN-750', name: 'Vintage 750', status: 'Added', details: 'Qty 2 @ 850' },
    { code: 'RC-750', name: 'Royal 750', status: 'Added', details: 'Qty 1 @ 900' }],
}]

let fetchMock: ReturnType<typeof vi.fn>
beforeEach(() => {
  fetchMock = vi.fn(async (url: string) => json(url.includes('/audit/screens') ? ['Item Master', 'Sales Invoice'] : ROWS))
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('AdminAuditLog', () => {
  it('shows each entry with its item lines', async () => {
    render(<AdminAuditLog />)
    expect(await screen.findByText('26/c10-0083718')).toBeInTheDocument()
    expect(screen.getByText('Qty 2 @ 850')).toBeInTheDocument()
    expect(screen.getByText('Royal 750')).toBeInTheDocument()
    expect(screen.getByText('29-09-2026')).toBeInTheDocument()
  })

  it('catches a To date before the From date', async () => {
    const user = userEvent.setup()
    render(<AdminAuditLog />)
    await screen.findByText('26/c10-0083718')
    const calls = fetchMock.mock.calls.length
    fireEvent.change(screen.getByLabelText('From'), { target: { value: '2026-09-20' } })
    fireEvent.change(screen.getByLabelText('To'), { target: { value: '2026-09-10' } })
    await user.click(screen.getByRole('button', { name: /view/i }))
    expect(screen.getAllByText('To date is before From date').length).toBeGreaterThan(0)
    expect(fetchMock.mock.calls.length).toBe(calls)
  })
})
