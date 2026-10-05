// Tests for the one-time Database setup on the sign-in page: mistakes are shown under the fields and nothing
// is sent, the password leaves the browser only encrypted for the "db" field, and the three steps (test the
// server, pick the database, copy the old data) run through to the login form.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import DatabaseSetup from './DatabaseSetup'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p: string, f: string) => `enc:${f}:${p}` }))

const json = (body: unknown, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
const CHECK = { database: 'vLQMSLHOUSE', edition: 'Express Edition', version: '16', can_create_tables: true, legacy_tables: 277,
  legacy_rows: 3796126, legacy_found: true, web_tables: 0, schema_version: 0, web_has_saved: false, ready: false, used_mb: 1432,
  copy_mb: 1411, after_copy_mb: 2843, limit_mb: 10240, fits: true }
let fetchMock: ReturnType<typeof vi.fn>
const sent = (path: string) => fetchMock.mock.calls.filter(([url]) => String(url).endsWith(path)).map(([, init]) => JSON.parse(String(init?.body)))

beforeEach(() => {
  let polls = 0
  fetchMock = vi.fn(async (url: string) => {
    if (url.endsWith('/setup/test')) return json({ ok: true, driver: 'fake', databases: ['master2', 'vLQMSLHOUSE'] })
    if (url.endsWith('/setup/connect')) return json(CHECK)
    if (url.endsWith('/setup/run')) return json({ started: true }, 202)
    if (url.endsWith('/setup/progress')) return json(++polls < 2 ? { state: 'copying', table: 'MstITEM', tables_done: 3, tables_total: 277, rows_done: 10, rows_total: 100 } : { state: 'done' })
    return json({})
  })
  vi.stubGlobal('fetch', fetchMock)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks(); vi.useRealTimers() })

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
    expect(screen.getByText('Enter the setup code shown in the backend window')).toBeInTheDocument()
    expect(screen.getByText(/Enter the server/)).toBeInTheDocument()
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('tests, connects and copies, sending the password only encrypted', async () => {
    const user = userEvent.setup()
    const onDone = setup()
    await user.type(screen.getByLabelText('Setup code'), 'ab12-cd34-ef56')
    await user.clear(screen.getByLabelText('Server'))
    await user.type(screen.getByLabelText('Server'), 'localhost\\SQLEXPRESS')
    await user.type(screen.getByLabelText('Password'), 'S3cret!')
    await user.click(screen.getByRole('button', { name: /test connection/i }))

    expect(sent('/setup/test')[0]).toEqual({ setup_code: 'ab12-cd34-ef56', host: 'localhost\\SQLEXPRESS', port: '', auth: 'sql', user: 'sa', password: 'enc:db:S3cret!' })
    // The vLQMS database is picked for them.
    expect(await screen.findByLabelText('Database')).toHaveValue('vLQMSLHOUSE')
    await user.click(screen.getByRole('button', { name: /^connect$/i }))
    expect(sent('/setup/connect')[0]).toMatchObject({ database: 'vLQMSLHOUSE', password: 'enc:db:S3cret!' })
    expect(await screen.findByText('277 tables, 37,96,126 rows')).toBeInTheDocument()

    await user.click(screen.getByRole('button', { name: /create tables and copy the old data/i }))
    expect(sent('/setup/run')[0]).toEqual({ setup_code: 'ab12-cd34-ef56' })
    expect(await screen.findByText(/Copying MstITEM/, {}, { timeout: 4000 })).toBeInTheDocument()
    await waitFor(() => expect(onDone).toHaveBeenCalled(), { timeout: 6000 })
    expect(JSON.stringify(fetchMock.mock.calls)).not.toContain('S3cret!"')   // never in plain text
  }, 15000)

  it('Windows login sends no login name or password', async () => {
    const user = userEvent.setup()
    setup()
    await user.type(screen.getByLabelText('Setup code'), 'X')
    await user.click(screen.getByRole('radio', { name: 'Windows login' }))
    expect(screen.queryByLabelText('Password')).toBeNull()
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    await waitFor(() => expect(sent('/setup/test')[0]).toMatchObject({ auth: 'windows', user: '', password: '' }))
  })

  it('does not offer the copy when the web app already saved data here', async () => {
    fetchMock.mockImplementation(async (url: string) => url.endsWith('/setup/connect') ? json({ ...CHECK, web_has_saved: true })
      : json({ ok: true, driver: 'fake', databases: ['vLQMSLHOUSE'] }))
    const user = userEvent.setup()
    setup()
    await user.type(screen.getByLabelText('Setup code'), 'X')
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    await user.click(await screen.findByRole('button', { name: /^connect$/i }))
    expect(await screen.findByText(/already saved data here/)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /copy the old data/i })).toBeDisabled()
  })
})
