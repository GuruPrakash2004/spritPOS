// Carry Over tab: nothing runs without groups and the typed confirmation; the run button appears only when nothing blocks it.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import DeleteNonLiquorItems from './DeleteNonLiquorItems'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const counts = { sales_bills: 4, sales_lines: 9, purchase_bills: 2, purchase_lines: 3, stock_rows: 14, first_sale: null }
let preview

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (url) => {
    if (url.includes('/delete-non-liquor/carry-groups')) return json([{ code: 'BEER', name: 'Beer' }, { code: 'WHISKY', name: 'Whisky' }])
    if (url.includes('/delete-non-liquor/carry-preview')) return json(preview)
    return json([])
  }))
})
afterEach(() => vi.unstubAllGlobals())

const open = async () => {
  const user = userEvent.setup()
  render(<DeleteNonLiquorItems />)
  await user.click(screen.getByRole('tab', { name: 'Carry Over' }))
  await screen.findByLabelText('Beer')
  return user
}

describe('Carry Over', () => {
  it('asks for groups first', async () => {
    const user = await open()
    await user.click(screen.getByRole('button', { name: /Show what will be deleted/ }))
    expect(await screen.findByText('No groups selected!')).toBeInTheDocument()
  })

  it('shows what goes and the run button only when nothing blocks it', async () => {
    preview = { groups: ['BEER'], items: 3, counts, carried_items: 2, carried_qty: 12.5, blockers: [], can_carry: true }
    const user = await open()
    await user.click(screen.getByLabelText('Beer'))
    await user.click(screen.getByRole('button', { name: /Show what will be deleted/ }))
    expect(await screen.findByText('2 (12.5)')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /Delete the transactions and carry over the stock/ })).toBeInTheDocument()
  })

  it('shows the blocker and no run button', async () => {
    preview = { groups: ['BEER'], items: 0, counts, carried_items: 0, carried_qty: 0, blockers: ['The groups have no items'], can_carry: false }
    const user = await open()
    await user.click(screen.getByLabelText('Beer'))
    await user.click(screen.getByRole('button', { name: /Show what will be deleted/ }))
    expect(await screen.findByText(/groups have no items/)).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /Delete the transactions and carry over the stock/ })).not.toBeInTheDocument()
  })

  it('refuses to run without CARRY OVER typed', async () => {
    preview = { groups: ['BEER'], items: 3, counts, carried_items: 2, carried_qty: 12.5, blockers: [], can_carry: true }
    const user = await open()
    await user.click(screen.getByLabelText('Beer'))
    await user.click(screen.getByRole('button', { name: /Show what will be deleted/ }))
    await user.click(await screen.findByRole('button', { name: /Delete the transactions and carry over the stock/ }))
    expect(await screen.findByText('Type CARRY OVER to confirm.')).toBeInTheDocument()
  })
})
