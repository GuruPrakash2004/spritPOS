// Tests for Remove Duplicate Invoice: possible duplicates are flagged, desktop bills can't be ticked, and the
// ticked bills are removed only after the confirm and the waiting popup.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import RemoveDuplicateInvoice from './RemoveDuplicateInvoice'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p: string, f: string) => `enc:${f}:${p}` }))

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const row = (doc_no: string, extra = {}) => ({ doc_no, date: '2026-09-29', time: '10:00:00', added_by: 'ADMIN', sales_type: 'CASH',
  bill_system: 'Web', qty: 2, value: 1700, counter: 'counter10', removable: true, duplicate_of: null, why_not: null, ...extra })
const ROWS = [row('26/c10-2', { duplicate_of: '26/c10-1' }), row('26/c10-1'), row('D/1', { bill_system: 'Desktop', removable: false, why_not: 'Desktop bill: remove it in the desktop app' })]

let fetchMock: ReturnType<typeof vi.fn>
const posts = () => fetchMock.mock.calls.filter(([, init]) => init?.method === 'POST')
beforeEach(() => {
  fetchMock = vi.fn(async (_url: string, init?: RequestInit) => json(init?.method === 'POST' ? { removed: ['26/c10-2'], message: 'Data Removed successfully!' } : ROWS))
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('RemoveDuplicateInvoice', () => {
  it('flags duplicates and keeps desktop bills unticked', async () => {
    render(<RemoveDuplicateInvoice />)
    expect(await screen.findByText('Possible duplicate of 26/c10-1')).toBeInTheDocument()
    expect(screen.getByLabelText('Select D/1')).toBeDisabled()
  })

  it('removes the ticked bill after the confirm and the popup', async () => {
    const user = userEvent.setup()
    render(<RemoveDuplicateInvoice />)
    await user.click(await screen.findByLabelText('Select 26/c10-2'))
    await user.click(screen.getByRole('button', { name: /remove \(1\)/i }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    await user.type(await screen.findByLabelText('Password'), 'admin')
    await user.click(screen.getByRole('button', { name: 'Continue' }))
    await waitFor(() => expect(posts()).toHaveLength(1))
    expect(JSON.parse(String(posts()[0][1].body))).toEqual({ doc_nos: ['26/c10-2'], admin_password: 'enc:admin:admin' })
    expect(toastStore.defer).toHaveBeenCalledWith('delete', '1 invoice(s) will be removed when this closes.', expect.any(String))
  })

  it('removes nothing when the popup is cancelled', async () => {
    vi.mocked(toastStore.defer).mockResolvedValue(false)
    const user = userEvent.setup()
    render(<RemoveDuplicateInvoice />)
    await user.click(await screen.findByLabelText('Select 26/c10-2'))
    await user.click(screen.getByRole('button', { name: /remove \(1\)/i }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    await user.type(await screen.findByLabelText('Password'), 'admin')
    await user.click(screen.getByRole('button', { name: 'Continue' }))
    await waitFor(() => expect(toastStore.defer).toHaveBeenCalled())
    expect(posts()).toEqual([])
  })
})
