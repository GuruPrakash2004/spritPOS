// Tests for the API client: token header, error text, and ending the session on a 401.
import { afterEach, describe, expect, it, vi } from 'vitest'
import { api, ApiError, qs, setAuthUser, setSessionEndedHandler } from './api'

const reply = (status: number, body: unknown) =>
  vi.fn(async () => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } }))

afterEach(() => {
  vi.unstubAllGlobals()
  setSessionEndedHandler(null)
  setAuthUser('', '', '')
})

describe('api()', () => {
  it('sends the login token and no X-User header', async () => {
    const fetchMock = reply(200, [])
    vi.stubGlobal('fetch', fetchMock)
    setAuthUser('RAVI', 'Cashier', 'tok-123')

    await api('/masters/items')

    const headers = (fetchMock.mock.calls[0] as unknown as [string, RequestInit])[1].headers as Record<string, string>
    expect(headers.Authorization).toBe('Bearer tok-123')
    expect(headers['X-User']).toBeUndefined()
  })

  it('turns FastAPI validation errors into one readable message', async () => {
    vi.stubGlobal('fetch', reply(422, { detail: [{ msg: 'Counts cannot be negative' }, { msg: 'Unknown note' }] }))

    await expect(api('/transactions/denomination')).rejects.toThrow('Counts cannot be negative; Unknown note')
  })

  it('ends the session on a 401 once logged in', async () => {
    vi.stubGlobal('fetch', reply(401, { detail: 'Your session has expired. Please log in again.' }))
    const ended = vi.fn()
    setSessionEndedHandler(ended)
    setAuthUser('RAVI', 'Cashier', 'tok-123')

    await expect(api('/masters/items')).rejects.toBeInstanceOf(ApiError)
    expect(ended).toHaveBeenCalledWith('Your session has expired. Please log in again.')
  })

  it('does not end a session for a wrong password at login', async () => {
    vi.stubGlobal('fetch', reply(401, { detail: 'Invalid user name or password' }))
    const ended = vi.fn()
    setSessionEndedHandler(ended)

    await expect(api('/auth/login', { method: 'POST', body: {} })).rejects.toThrow('Invalid user name or password')
    expect(ended).not.toHaveBeenCalled()
  })

  it('qs() drops empty values', () => {
    expect(qs({ a: 1, b: '', c: undefined, d: 'x y' })).toBe('?a=1&d=x+y')
  })
})
