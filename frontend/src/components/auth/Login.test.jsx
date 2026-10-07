// The sign-in page has no demo logins, takes the financial years from the shop's database, and has no
// sign-in form while the database is offline or has no administrator.
import { render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it, vi } from 'vitest'
import Login from './Login'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const COMPANY = { name: 'MY SHOP', address: '', fy_start: '2027-04-01', fy_end: '2028-03-31',
  fin_years: [{ start: '2027-04-01', end: '2028-03-31', active: true }, { start: '2026-04-01', end: '2027-03-31', active: false }] }

const serve = (status) => vi.stubGlobal('fetch', vi.fn(async (url) => {
  if (url.endsWith('/setup/status')) return json({ configured: true, connected: true, needs_setup: false, needs_admin: false, ...status })
  if (url.includes('/masters/company')) return json(COMPANY)
  return json({})
}))

afterEach(() => vi.unstubAllGlobals())

describe('Login', () => {
  it('shows no demo logins and the shop\'s own financial years', async () => {
    serve({})
    render(<Login onLogin={() => {}} />)
    expect(await screen.findByRole('option', { name: '01/04/2027 - 31/03/2028' })).toBeInTheDocument()
    expect(screen.getByRole('option', { name: '01/04/2026 - 31/03/2027' })).toBeInTheDocument()
    expect(screen.queryByText(/demo/i)).not.toBeInTheDocument()
    expect(screen.getByLabelText('User name')).toHaveValue('')
  })

  it('has no sign-in form while the database is offline', async () => {
    serve({ connected: false })
    render(<Login onLogin={() => {}} />)
    expect(await screen.findByText(/Database offline/)).toBeInTheDocument()
    expect(screen.queryByLabelText('Password')).not.toBeInTheDocument()
  })

  it('asks for the first administrator when there is none', async () => {
    serve({ needs_admin: true })
    render(<Login onLogin={() => {}} />)
    expect(await screen.findByRole('button', { name: /make administrator/i })).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /^sign in$/i })).not.toBeInTheDocument()
  })
})
