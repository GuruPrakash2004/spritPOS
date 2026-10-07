// About shows the connection state (there is no Disconnect button).
import { render, screen } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import { setAuthUser } from '../../lib/api'
import About from './About'

const ABOUT = { product: 'e-vLQMS ERP', software_version: '1', api_version: '0.2.0', company: 'MY SHOP', db_connected: true,
  db_configured: true, db_disconnected_for_testing: false, server_time: '2026-10-05T10:00:00' }
const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
let fetchMock: ReturnType<typeof vi.fn>

beforeEach(() => {
  fetchMock = vi.fn(async (url: string) => json(String(url).endsWith('/disconnect')
    ? { ...ABOUT, db_connected: false, db_disconnected_for_testing: true } : ABOUT))
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks(); setAuthUser('', '', '') })

describe('About', () => {
  it('shows the connection, and has no disconnect button even for an Administrator', async () => {
    setAuthUser('ADMIN', 'Administrator', 't')
    render(<About />)
    expect(await screen.findByText('SQL Server connected')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /disconnect|reconnect/i })).not.toBeInTheDocument()
  })

  it('has no connection buttons for other roles', async () => {
    setAuthUser('RAVI', 'Cashier', 't')
    render(<About />)
    await screen.findByText('SQL Server connected')
    expect(screen.queryByRole('button', { name: /disconnect/i })).not.toBeInTheDocument()
  })

  it('has no test-data switch or label any more', async () => {
    setAuthUser('ADMIN', 'Administrator', 't')
    render(<About />)
    await screen.findByText('SQL Server connected')
    expect(screen.queryByText(/test \(dummy\) data|test data/i)).not.toBeInTheDocument()
  })
})
