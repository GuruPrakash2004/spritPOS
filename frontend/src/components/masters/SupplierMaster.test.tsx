// Tests for Supplier Master: it loads suppliers from the API and saves edits (PUT) and new ones (POST).
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import SupplierMaster from './SupplierMaster'
import type { ApiSupplier } from '../../lib/api'

const RND: ApiSupplier = {
  code: 'RND', name: 'R&D DISTRIBUTORS', alias: '', address: '', city: 'Bangalore', pin: '', state: 'Karnataka',
  country: 'INDIA', phone: '9900196257', gstin: '29AABCR1234F1Z1', currency: 'Rupees', active: true,
}

const json = (body: unknown, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

let fetchMock: ReturnType<typeof vi.fn>

beforeEach(() => {
  // GET returns the supplier list; PUT/POST echo the body back, as the backend does.
  fetchMock = vi.fn(async (_url: string, init?: RequestInit) =>
    init?.method && init.method !== 'GET' ? json(JSON.parse(String(init.body)), init.method === 'POST' ? 201 : 200) : json([RND]))
  vi.stubGlobal('fetch', fetchMock)
})

afterEach(() => {
  vi.unstubAllGlobals()
})

const writes = () => fetchMock.mock.calls.filter(([, init]) => init?.method && init.method !== 'GET')

// Actions wait for their popup (toastStore.defer); here it closes at once so the request goes out.
beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.restoreAllMocks())

describe('SupplierMaster', () => {
  it('sends nothing when the waiting popup is cancelled', async () => {
    vi.mocked(toastStore.defer).mockResolvedValue(false)
    const user = userEvent.setup()
    render(<SupplierMaster />)
    const phone = await screen.findByDisplayValue('9900196257')

    await user.clear(phone)
    await user.type(phone, '9811111111')
    await user.click(screen.getByRole('button', { name: /save supplier/i }))

    await waitFor(() => expect(toastStore.defer).toHaveBeenCalledWith('ok', 'Supplier "RND" will be saved when this closes.', expect.any(String)))
    expect(writes()).toHaveLength(0)
    expect(screen.getByRole('button', { name: /save supplier/i })).toBeEnabled()
  })

  it('loads the first supplier and saves an edit with PUT', async () => {
    const user = userEvent.setup()
    render(<SupplierMaster />)
    const phone = await screen.findByDisplayValue('9900196257')

    await user.clear(phone)
    await user.type(phone, '9811111111')
    await user.click(screen.getByRole('button', { name: /save supplier/i }))

    await waitFor(() => expect(writes()).toHaveLength(1))
    const [url, init] = writes()[0]
    expect(url).toBe('/api/masters/suppliers/RND')
    expect(init.method).toBe('PUT')
    expect(JSON.parse(init.body)).toMatchObject({ code: 'RND', phone: '9811111111' })
  })

  it('adds a new supplier with POST', async () => {
    const user = userEvent.setup()
    render(<SupplierMaster />)
    await screen.findByDisplayValue('9900196257')

    await user.click(screen.getByRole('button', { name: 'New' }))
    await user.type(screen.getByLabelText(/supplier code/i), 'ABC')
    await user.type(screen.getByLabelText(/supplier name/i), 'ABC TRADERS')
    await user.click(screen.getByRole('button', { name: /save supplier/i }))

    await waitFor(() => expect(writes()).toHaveLength(1))
    const [url, init] = writes()[0]
    expect(url).toBe('/api/masters/suppliers')
    expect(init.method).toBe('POST')
    expect(JSON.parse(init.body)).toMatchObject({ code: 'ABC', name: 'ABC TRADERS', country: 'INDIA' })
  })

  it('shows inline errors and sends nothing when code or name is missing', async () => {
    const user = userEvent.setup()
    render(<SupplierMaster />)
    await screen.findByDisplayValue('9900196257')

    await user.click(screen.getByRole('button', { name: 'New' }))
    await user.click(screen.getByRole('button', { name: /save supplier/i }))

    expect(screen.getAllByText('Enter the Supplier code').length).toBeGreaterThan(0)
    expect(screen.getAllByText('Enter the Supplier name').length).toBeGreaterThan(0)
    expect(writes()).toHaveLength(0)
  })

  it('refuses a code that another supplier already uses', async () => {
    const user = userEvent.setup()
    render(<SupplierMaster />)
    await screen.findByDisplayValue('9900196257')

    await user.click(screen.getByRole('button', { name: 'New' }))
    await user.type(screen.getByLabelText(/supplier code/i), 'rnd')
    await user.type(screen.getByLabelText(/supplier name/i), 'Copy')
    await user.click(screen.getByRole('button', { name: /save supplier/i }))

    expect(screen.getAllByText('This supplier code is already used').length).toBeGreaterThan(0)
    expect(writes()).toHaveLength(0)
  })
})
