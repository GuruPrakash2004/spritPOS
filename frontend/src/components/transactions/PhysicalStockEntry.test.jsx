// Physical Stock Entry: Load needs a filter, a counted quantity shows its difference, and Clear Physical Stock zeroes the counts.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import PhysicalStockEntry from './PhysicalStockEntry'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const ITEM = { code: 'WH1', name: 'Whisky 750', group: 'WHISKY', package: '750ML', active: true }
const ROW = { item_code: 'WH1', item_name: 'Whisky 750', group: 'WHISKY', package: '750ML', litres_per_bottle: 0.75, bottles_per_case: 12, pur_rate: 500, mrp: 700, system_qty: 10 }

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (url) => {
    if (url.includes('/physical-stock/load')) return json([ROW])
    if (url.includes('/masters/items')) return json([ITEM])
    return json([])
  }))
})
afterEach(() => vi.unstubAllGlobals())

describe('PhysicalStockEntry', () => {
  it('asks for a group, package or item before loading', async () => {
    const user = userEvent.setup()
    render(<PhysicalStockEntry />)
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    expect(await screen.findByText(/select at least one Item/i)).toBeInTheDocument()
  })

  it('shows the difference between the counted and the system stock, and clears the counts', async () => {
    const user = userEvent.setup()
    render(<PhysicalStockEntry />)
    await user.type(screen.getByLabelText(/Selected Item/), 'whisky')
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    const physical = await screen.findByLabelText('Physical stock Whisky 750')
    expect(physical).toHaveValue(10)                                         // starts at the system stock, so no difference
    await user.clear(physical)
    await user.type(physical, '7')
    expect(screen.getAllByText('-3').length).toBeGreaterThan(0)              // diff = 7 − 10

    await user.click(screen.getByRole('button', { name: /Clear Physical Stock/ }))
    expect(screen.getByLabelText('Physical stock Whisky 750')).toHaveValue(0)
  })
})
