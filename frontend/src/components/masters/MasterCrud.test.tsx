// Item Master follows the role: cost fields hidden without cost access, buttons follow the
// Add/Edit/Delete ticks saved in Admin Master.
import { render, screen } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import MasterCrud from './MasterCrud'
import { ITEM_FIELDS } from './masterConfigs'
import { setAuthRights } from '../../lib/api'
import { money } from '../../lib/styles'

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })

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
