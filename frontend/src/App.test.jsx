// Page refresh: a tab that still holds its login token is signed back in; an expired one goes to the
// sign-in page with the reason.
import { render, screen, waitFor } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'

const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

const COMPANY = { name: 'THE LIQUOR HOUSE', email: 'shop@example.com', address: 'Bengaluru', fy_start: '2026-04-01', fy_end: '2027-03-31' }
const ME = { user: 'RAVI', role: 'Cashier', modules: ['Dashboard', 'Cash Denomination'], rights: {}, company: COMPANY }

beforeEach(() => {
  vi.resetModules()   // api.js reads the saved token when it is first imported
  sessionStorage.clear()
  localStorage.clear()
  window.history.replaceState(null, '', '/')
  vi.stubGlobal('BroadcastChannel', undefined)   // no other tabs to hand a token over (they would leak between tests)
})
afterEach(() => vi.unstubAllGlobals())

async function renderApp() {
  const { default: App } = await import('./App')
  render(<App />)
}

describe('page refresh', () => {
  it('signs the user back in and reopens the screen the address names', async () => {
    sessionStorage.setItem('vlqms_jwt_token', 'still-valid')
    window.history.replaceState(null, '', '/cash-denomination')   // each screen has its own address
    vi.stubGlobal('fetch', vi.fn(async (url) => {
      if (url.startsWith('/api/auth/me')) return json(ME)
      if (url.startsWith('/api/masters/company')) return json(COMPANY)
      if (url.startsWith('/api/transactions/denomination')) return json(null)
      if (url.startsWith('/api/reports/dsr')) return json({ total_bills: 3, sales_incl_gst: 2500, cash_balance: 2500 })
      return json([])
    }))

    await renderApp()

    expect((await screen.findAllByRole('heading', { name: /cash denomination/i })).length).toBeGreaterThan(0)
    expect(screen.queryByRole('button', { name: /sign in|log in/i })).not.toBeInTheDocument()
  })

  it('sends an expired session to the sign-in page with the reason', async () => {
    sessionStorage.setItem('vlqms_jwt_token', 'expired')
    vi.stubGlobal('fetch', vi.fn(async (url) =>
      url.startsWith('/api/auth/me')
        ? json({ detail: 'Your session has expired. Please log in again.' }, 401)
        : json(COMPANY)))

    await renderApp()

    expect(await screen.findByText('Your session has expired. Please log in again.')).toBeInTheDocument()
    expect(sessionStorage.getItem('vlqms_jwt_token')).toBeNull()
  })

  it('never keeps the token in localStorage', async () => {
    localStorage.setItem('vlqms_jwt_token', 'old-build-token')
    vi.stubGlobal('fetch', vi.fn(async () => json(COMPANY)))

    await renderApp()

    expect(localStorage.getItem('vlqms_jwt_token')).toBeNull()
  })

  it('notices a session ended elsewhere when the window regains focus', async () => {
    sessionStorage.setItem('vlqms_jwt_token', 'valid-then-changed')
    let meCalls = 0
    vi.stubGlobal('fetch', vi.fn(async (url) => {
      if (url.startsWith('/api/auth/me')) {
        meCalls += 1
        return meCalls === 1 ? json(ME) : json({ detail: 'Your password or access was changed. Please log in again.' }, 401)
      }
      if (url.startsWith('/api/reports/dsr')) return json({ total_bills: 0, sales_incl_gst: 0, cash_balance: 0 })
      if (url.startsWith('/api/masters/company')) return json(COMPANY)
      return json([])
    }))
    await renderApp()
    await screen.findAllByRole('heading', { name: /dashboard/i })

    // The focus listener is attached in an effect after the Dashboard renders, so keep focusing until
    // the check has run (a single early event could fire before the listener exists).
    await waitFor(() => {
      window.dispatchEvent(new Event('focus'))
      expect(meCalls).toBeGreaterThan(1)
    })

    expect(await screen.findByText('Your password or access was changed. Please log in again.')).toBeInTheDocument()
  })
})
