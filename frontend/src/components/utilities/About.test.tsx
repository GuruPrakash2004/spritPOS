// About shows the connection state, and an Administrator can disconnect the database for testing.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
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
  it('shows the connection, and disconnects the database for an Administrator', async () => {
    setAuthUser('ADMIN', 'Administrator', 't')
    render(<About />)
    expect(await screen.findByText('SQL Server connected')).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: /disconnect database/i }))
    expect(await screen.findByText('SQL Server disconnected (for testing)')).toBeInTheDocument()
    await waitFor(() => expect(screen.getByRole('button', { name: /reconnect database/i })).toBeInTheDocument())
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
