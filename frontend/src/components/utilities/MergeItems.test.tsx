// Tests for Merge Items / Replace and Remove: picking both items shows the preview, an empty pick is shown
// under the field, and the merge is sent only after the confirm and the waiting popup.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import MergeItems from './MergeItems'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p: string, f: string) => `enc:${f}:${p}` }))

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const item = (code: string, name: string, active = true) => ({ code, name, qoh: 5, active, mrp: 1, purchase_rate: 1, group: '', brand: '', liqtype: '', uom: '', package: '', hsn_code: '', bottles_per_case: 12 })
const PREVIEW = { from: { code: 'RC-750', name: 'Royal', qoh: 5, active: true }, to: { code: 'VIN-750', name: 'Vintage', qoh: 5, active: true },
  lines: { sales_invoice: 3 }, movements: 4, qoh_after: 10 }

let fetchMock: ReturnType<typeof vi.fn>
const posts = () => fetchMock.mock.calls.filter(([, init]) => init?.method === 'POST')
beforeEach(() => {
  fetchMock = vi.fn(async (url: string, init?: RequestInit) => {
    if (url.includes('/masters/items')) return json([item('RC-750', 'Royal'), item('VIN-750', 'Vintage'), item('OLD-1', 'Old one', false)])
    if (init?.method === 'POST') return json({ ...PREVIEW, message: 'Successfully Merged!' })
    return json(PREVIEW)
  })
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('MergeItems', () => {
  it('previews and merges after the confirm and the popup', async () => {
    const user = userEvent.setup()
    render(<MergeItems mode="merge" />)
    await waitFor(() => expect(fetchMock).toHaveBeenCalled())
    await user.type(screen.getByLabelText('Merge Item'), 'RC-750')
    await user.type(screen.getByLabelText('To Item'), 'VIN-750')
    expect(await screen.findByText('Sales invoices: 3 line(s) move to VIN-750')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Merge Item' }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))
    await user.type(await screen.findByLabelText('Password'), 'Extra#123')
    await user.click(screen.getByRole('button', { name: 'Continue' }))
    await waitFor(() => expect(posts()).toHaveLength(1))
    expect(posts()[0][0]).toBe('/api/utilities/merge-items/merge')
    expect(JSON.parse(String(posts()[0][1].body))).toEqual({ from_item: 'RC-750', to_item: 'VIN-750', admin_password: 'enc:admin:Extra#123' })
  })

  it('shows a missing pick under the field and sends nothing', async () => {
    const user = userEvent.setup()
    render(<MergeItems mode="replace" />)
    await user.click(await screen.findByRole('button', { name: 'Merge and Remove First item' }))
    expect(screen.getAllByText('Please Select Item').length).toBeGreaterThan(0)
    expect(posts()).toEqual([])
  })

  it('offers only active items for Merge Items', async () => {
    const { container } = render(<MergeItems mode="merge" />)
    await waitFor(() => expect(container.querySelectorAll('#mi-items option').length).toBe(2))
  })
})
