// Tests for the Change password dialog: inline mistakes, and the new token replacing the old one.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import ChangePasswordDialog from './ChangePasswordDialog'
import { getAuthToken, setAuthUser } from '../../lib/api'

const json = (body: unknown, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

afterEach(() => { vi.unstubAllGlobals(); setAuthUser('', '', '') })

async function fill(current: string, next: string, again: string) {
  const user = userEvent.setup()
  if (current) await user.type(screen.getByLabelText(/^current password/i), current)
  if (next) await user.type(screen.getByLabelText(/^new password(?! again)/i), next)
  if (again) await user.type(screen.getByLabelText(/new password again/i), again)
  await user.click(screen.getByRole('button', { name: 'Change password' }))
}

// Actions wait for their popup (toastStore.defer); here it closes at once so the request goes out.
beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.restoreAllMocks())

describe('ChangePasswordDialog', () => {
  it('shows mistakes under the fields and sends nothing', async () => {
    const fetchMock = vi.fn()
    vi.stubGlobal('fetch', fetchMock)
    render(<ChangePasswordDialog user="RAVI" onClose={() => {}} />)

    await fill('ravi', 'short', 'other')

    expect(screen.getAllByText('Password must be at least 8 characters').length).toBeGreaterThan(0)
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('catches two new passwords that differ', async () => {
    vi.stubGlobal('fetch', vi.fn())
    render(<ChangePasswordDialog user="RAVI" onClose={() => {}} />)

    await fill('ravi', 'Counter#77', 'Counter#78')

    expect(screen.getAllByText("The two new passwords don't match").length).toBeGreaterThan(0)
  })

  it('saves and switches to the new login token', async () => {
    const fetchMock = vi.fn(async () => json({ access_token: 'new-token' }))
    vi.stubGlobal('fetch', fetchMock)
    setAuthUser('RAVI', 'Cashier', 'old-token')
    const onClose = vi.fn()
    render(<ChangePasswordDialog user="RAVI" onClose={onClose} />)

    await fill('ravi', 'Counter#77', 'Counter#77')

    await waitFor(() => expect(onClose).toHaveBeenCalled())
    const [url, init] = fetchMock.mock.calls[0] as unknown as [string, RequestInit]
    expect(url).toBe('/api/auth/change-password')
    expect(JSON.parse(String(init.body))).toEqual({ current_password: 'ravi', new_password: 'Counter#77' })
    expect(getAuthToken()).toBe('new-token')
  })

  it('shows a wrong current password under that field', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => json({ detail: 'Your current password is not correct' }, 422)))
    render(<ChangePasswordDialog user="RAVI" onClose={() => {}} />)

    await fill('nope', 'Counter#77', 'Counter#77')

    expect((await screen.findAllByText('Your current password is not correct')).length).toBeGreaterThan(0)
  })
})
