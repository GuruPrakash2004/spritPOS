// Tests for the Admin Master (Security) tabs: Login settings sends the additional ADMIN password only encrypted, with mistakes
// shown under the fields.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import LoginSettingsTab from './LoginSettingsTab'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p, f) => `enc:${f}:${p}` }))

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
let fetchMock
const sent = (method) => fetchMock.mock.calls.filter(([, init]) => init?.method === method)

beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('LoginSettingsTab', () => {
  const SETTINGS = { min_length: 8, require_mix: true, require_upper: false, require_lower: false, require_digit: false, require_special: false, password_expiry_days: 0, show_remember_me: false }
  beforeEach(() => {
    fetchMock = vi.fn(async (url) => json(url.includes('/admin/security') ? { adnl_admin_set: false }
      : url.includes('adnl-admin-password') ? { adnl_admin_set: true } : SETTINGS))
    vi.stubGlobal('fetch', fetchMock)
  })

  it('checks the new password inline and sends nothing', async () => {
    const user = userEvent.setup()
    render(<LoginSettingsTab currentUser="ADMIN" />)
    await user.click(await screen.findByRole('button', { name: /set password/i }))
    expect(screen.getByText('Enter your own login password')).toBeInTheDocument()
    expect(sent('PUT')).toEqual([])
  })

  it('sends the passwords encrypted', async () => {
    const user = userEvent.setup()
    render(<LoginSettingsTab currentUser="ADMIN" />)
    await user.type(await screen.findByLabelText('Your login password'), 'admin')
    await user.type(screen.getByLabelText('New additional ADMIN password'), 'Extra#123')
    await user.type(screen.getByLabelText('Confirm Password'), 'Extra#123')
    await user.click(screen.getByRole('button', { name: /set password/i }))
    await waitFor(() => expect(sent('PUT')).toHaveLength(1))
    expect(JSON.parse(String(sent('PUT')[0][1].body))).toEqual({ current_password: 'enc:current:admin', new_password: 'enc:new:Extra#123' })
    expect(await screen.findByText('Set')).toBeInTheDocument()
  })
})
