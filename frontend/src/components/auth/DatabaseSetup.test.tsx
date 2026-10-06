// Tests for the one-time Database setup on the sign-in page: mistakes are shown under the fields and nothing
// is sent, the password leaves the browser only encrypted for the "db" field, and the two steps (test the
// server, pick the database and connect) run through to the login form. Nothing is copied.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import DatabaseSetup from './DatabaseSetup'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p: string, f: string) => `enc:${f}:${p}` }))

const json = (body: unknown, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
let fetchMock: ReturnType<typeof vi.fn>
const sent = (path: string) => fetchMock.mock.calls.filter(([url]) => String(url).endsWith(path)).map(([, init]) => JSON.parse(String(init?.body)))

beforeEach(() => {
  fetchMock = vi.fn(async (url: string) => {
    if (url.endsWith('/setup/test')) return json({ ok: true, driver: 'fake', databases: ['master2', 'vLQMSLHOUSE'] })
    if (url.endsWith('/setup/connect')) return json({ database: 'vLQMSLHOUSE', edition: 'Express Edition', ready: true })
    return json({})
  })
  vi.stubGlobal('fetch', fetchMock)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

const setup = (onDone = vi.fn()) => {
  render(<DatabaseSetup inputClass="" labelClass="" onDone={onDone} />)
  return onDone
}

describe('DatabaseSetup', () => {
  it('shows mistakes under the fields and sends nothing', async () => {
    const user = userEvent.setup()
    setup()
    await user.clear(screen.getByLabelText('Server'))
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    expect(screen.getByText(/Enter the server/)).toBeInTheDocument()
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('tests and connects, sending the password only encrypted, then goes back to the sign-in', async () => {
    const user = userEvent.setup()
    const onDone = setup()
    await user.clear(screen.getByLabelText('Server'))
    await user.type(screen.getByLabelText('Server'), 'localhost\\SQLEXPRESS')
    await user.type(screen.getByLabelText('Password'), 'S3cret!')
    await user.click(screen.getByRole('button', { name: /test connection/i }))

    expect(sent('/setup/test')[0]).toEqual({ host: 'localhost\\SQLEXPRESS', port: '', auth: 'sql', user: 'sa', password: 'enc:db:S3cret!' })
    // The vLQMS database is picked for them.
    expect(await screen.findByLabelText('Database')).toHaveValue('vLQMSLHOUSE')
    await user.click(screen.getByRole('button', { name: /^connect$/i }))
    await waitFor(() => expect(onDone).toHaveBeenCalled())
    expect(sent('/setup/connect')[0]).toMatchObject({ database: 'vLQMSLHOUSE', password: 'enc:db:S3cret!' })
    expect(fetchMock.mock.calls.some(([url]) => String(url).endsWith('/setup/run'))).toBe(false)   // nothing is copied
    expect(JSON.stringify(fetchMock.mock.calls)).not.toContain('S3cret!"')   // never in plain text
  })

  it('shows why a database was refused and stays on the step', async () => {
    fetchMock.mockImplementation(async (url: string) => url.endsWith('/setup/connect')
      ? json({ detail: "This isn't a vLQMS database: these tables are missing: MstITEM." }, 400)
      : json({ ok: true, driver: 'fake', databases: ['vLQMSLHOUSE'] }))
    const user = userEvent.setup()
    const onDone = setup()
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    await user.click(await screen.findByRole('button', { name: /^connect$/i }))
    expect(await screen.findByText(/isn't a vLQMS database/)).toBeInTheDocument()
    expect(onDone).not.toHaveBeenCalled()
  })
})
