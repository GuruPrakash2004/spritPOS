// The "Create the administrator" step of Database setup: mistakes show under the fields and nothing is sent; the
// password leaves the browser only encrypted for the "new" field.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import CreateAdmin from './CreateAdmin'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p: string, f: string) => `enc:${f}:${p}` }))

const json = (body: unknown, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
let fetchMock: ReturnType<typeof vi.fn>

beforeEach(() => {
  fetchMock = vi.fn(async () => json({ user: 'OWNER', role: 'Administrator' }, 201))
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('CreateAdmin', () => {
  it('shows mistakes under the fields and sends nothing', async () => {
    const user = userEvent.setup()
    render(<CreateAdmin inputClass="" labelClass="" onDone={() => {}} />)
    await user.type(screen.getByLabelText('New password'), 'short')
    await user.click(screen.getByRole('button', { name: /make administrator/i }))
    expect(screen.getByText('Enter the setup code shown in the backend window')).toBeInTheDocument()
    expect(screen.getByText('Use at least 8 characters')).toBeInTheDocument()
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('sends the password only encrypted, then goes back to the sign-in', async () => {
    const user = userEvent.setup()
    const onDone = vi.fn()
    render(<CreateAdmin inputClass="" labelClass="" onDone={onDone} />)
    await user.type(screen.getByLabelText('Setup code'), 'ab12-cd34-ef56')
    await user.type(screen.getByLabelText('User name'), 'owner')
    await user.type(screen.getByLabelText('New password'), 'Shop#2026')
    await user.type(screen.getByLabelText('Confirm password'), 'Shop#2026')
    await user.click(screen.getByRole('button', { name: /make administrator/i }))
    await waitFor(() => expect(onDone).toHaveBeenCalled())
    const [url, init] = fetchMock.mock.calls[0]
    expect(String(url)).toMatch(/\/setup\/admin$/)
    expect(JSON.parse(String(init.body))).toEqual({ setup_code: 'AB12-CD34-EF56', user: 'OWNER', password: 'enc:new:Shop#2026' })
  })
})
