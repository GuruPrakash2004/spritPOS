// Item Master follows the role: cost fields hidden without cost access, buttons follow the
// Add/Edit/Delete ticks saved in Admin Master.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import MasterCrud from './MasterCrud'
import { ITEM_FIELDS } from './masterConfigs'
import { setAuthRights } from '../../lib/api'
import { money } from '../../lib/styles'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async () => json([])))
})
afterEach(() => { vi.unstubAllGlobals(); setAuthRights([], {}) })

const renderItems = () => render(<MasterCrud title="Item Master" subtitle="Items" path="items" fields={ITEM_FIELDS} />)

describe('Item Master by role', () => {
  it('hides Purchase Rate and Delete from a Cashier', async () => {
    setAuthRights(['Item Master', 'Sales Invoice'], { 'Item Master': { add: true, edit: true, delete: false } })
    renderItems()

    expect(await screen.findByRole('button', { name: /new/i })).toBeInTheDocument()
    expect(screen.queryByText('Purchase Rate')).not.toBeInTheDocument()
    expect(screen.getByRole('button', { name: /delete/i })).toBeDisabled()
  })

  it('shows Purchase Rate to a Manager', async () => {
    setAuthRights(['Item Master', 'Purchase Bill'])
    renderItems()

    expect((await screen.findAllByText('Purchase Rate')).length).toBeGreaterThan(0)
  })

  it('hides New and Edit when those ticks are off', async () => {
    setAuthRights(['Item Master'], { 'Item Master': { add: false, edit: false, delete: false } })
    renderItems()

    await screen.findByRole('heading', { name: 'Item Master' })
    expect(screen.queryByRole('button', { name: /^new$/i })).not.toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /^edit$/i })).not.toBeInTheDocument()
  })
})

describe('money()', () => {
  it('shows — for an amount the server hid', () => {
    expect(money(null)).toBe('—')
    expect(money(123456.7)).toBe('1,23,456.70')
  })
})

describe('Item Master loading', () => {
  it('loads the list and each option list once, not in a loop', async () => {
    const fetchMock = vi.fn(async (url) => json(url.includes('/masters/uoms') ? [{ code: 'BTL', name: 'Bottle' }] : []))
    vi.stubGlobal('fetch', fetchMock)
    setAuthRights(['Item Master'])
    renderItems()
    await screen.findByRole('heading', { name: 'Item Master' })
    await new Promise(r => setTimeout(r, 200))   // a loop would keep fetching during this wait
    const uomCalls = fetchMock.mock.calls.filter(([url]) => String(url).includes('/masters/uoms')).length
    expect(uomCalls).toBe(1)
  })
})

describe('Item Master record navigation', () => {
  const ITEMS = ['A1', 'B2', 'C3', 'D4'].map(code => ({ code, name: `ITEM ${code}`, group: 'G', mrp: 10, qoh: 0, active: true }))

  it('moves first / next / previous / last through the records', async () => {
    vi.stubGlobal('fetch', vi.fn(async (url) => json(url.includes('/masters/items') && !url.includes('/masters/items/') ? ITEMS : [])))
    setAuthRights(['Item Master'])
    const user = userEvent.setup()
    renderItems()

    const counter = async (text) => waitFor(() => expect(screen.getByText(text)).toBeInTheDocument())
    await counter(/^\d+ \/ 4$/)
    await user.click(screen.getByRole('button', { name: 'First record' }))
    await counter('1 / 4')
    expect(screen.getByRole('button', { name: 'First record' })).toBeDisabled()
    await user.click(screen.getByRole('button', { name: 'Next record' }))
    await counter('2 / 4')
    await user.click(screen.getByRole('button', { name: 'Last record' }))
    await counter('4 / 4')
    expect(screen.getByRole('button', { name: 'Next record' })).toBeDisabled()
    await user.click(screen.getByRole('button', { name: 'Previous record' }))
    await counter('3 / 4')
  })
})
