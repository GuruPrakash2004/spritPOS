// Dashboard panels a role's Field Level Rights hide are not shown, and their data isn't asked for.
import { render, screen, waitFor } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { setFieldRights } from '../../lib/api'
import { Dashboard } from './Dashboard'

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
let fetchMock: ReturnType<typeof vi.fn>

beforeEach(() => {
  // Lists come back empty; the single-object reports fail (the panels then show "unavailable").
  fetchMock = vi.fn(async (url: string) => /\/reports\/(dsr|stock-movement)|\/masters\/company/.test(String(url))
    ? new Response('{"detail":"x"}', { status: 500, headers: { 'Content-Type': 'application/json' } }) : json([]))
  vi.stubGlobal('fetch', fetchMock)
})
afterEach(() => { vi.unstubAllGlobals(); setFieldRights({}) })

const asked = (part: string) => fetchMock.mock.calls.some(([url]) => String(url).includes(part))

describe('Dashboard field rights', () => {
  it('leaves out hidden panels and does not fetch their data', async () => {
    setFieldRights({ dashboard: { stock_movement: 'hidden', restock: 'hidden', sales_chart: 'hidden' } })
    render(<Dashboard />)
    await waitFor(() => expect(asked('/reports/dsr')).toBe(true))
    expect(screen.queryByText('Stock movement')).not.toBeInTheDocument()
    expect(screen.queryByText('Sales, last 7 days')).not.toBeInTheDocument()
    expect(asked('/reports/stock-movement')).toBe(false)
    expect(asked('/reports/restock')).toBe(false)
    expect(screen.getByLabelText('Key figures')).toBeInTheDocument()
  })

  it('shows every panel without field rights', async () => {
    render(<Dashboard />)
    expect(await screen.findByText('Stock movement')).toBeInTheDocument()
    expect(screen.getByText('Sales, last 7 days')).toBeInTheDocument()
  })
})

describe('Dashboard store banner', () => {
  it('shows Store Open with today\'s hours next to the Live chip', async () => {
    vi.stubGlobal('fetch', vi.fn(async (url: string) => String(url).includes('/company/hours')
      ? json({ configured: true, open_now: true, today: '10:00 AM – 10:30 PM', place: 'My Shop', maps_url: 'https://maps.google.com/?cid=1', error: null })
      : new Response('{"detail":"x"}', { status: 500, headers: { 'Content-Type': 'application/json' } })))
    render(<Dashboard />)
    expect(await screen.findByText('Store Open')).toBeInTheDocument()
    expect(screen.getByText('· 10:00 AM – 10:30 PM')).toBeInTheDocument()
  })
})
