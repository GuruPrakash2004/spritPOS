// Delete Non-Liquor Items: the delete button appears only when nothing blocks it; a blocker is shown instead.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import DeleteNonLiquorItems from './DeleteNonLiquorItems'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const counts = { sales_bills: 1, sales_lines: 2, purchase_bills: 1, purchase_lines: 1, stock_rows: 3, first_sale: null }
let preview

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (url) => {
    if (url.includes('/masters/item-groups')) return json([{ code: 'SNACKS', liquor: false }, { code: 'WHISKY', liquor: true }])
    if (url.includes('/delete-non-liquor/preview')) return json(preview)
    return json([])
  }))
})
afterEach(() => vi.unstubAllGlobals())

const open = async () => {
  const user = userEvent.setup()
  render(<DeleteNonLiquorItems />)
  await user.selectOptions(await screen.findByLabelText('Select ItemGroup'), 'SNACKS')   // liquor groups are not offered
  await user.click(screen.getByRole('button', { name: /Show items/ }))
  return user
}

describe('DeleteNonLiquorItems', () => {
  it('offers only non-liquor groups and shows the delete button when nothing blocks it', async () => {
    preview = { group: 'SNACKS', items: [{ item_code: 'S1', item_name: 'Chips', liqtype: 'OTHERS', problem: '' }], counts, blockers: [], can_delete: true }
    await open()
    expect(screen.queryByRole('option', { name: 'WHISKY' })).not.toBeInTheDocument()
    expect(await screen.findByRole('button', { name: /Delete the above listed items/ })).toBeInTheDocument()
  })

  it('shows the blocker and no delete button when the group has liquor', async () => {
    preview = { group: 'SNACKS', items: [{ item_code: 'S1', item_name: 'Rum', liqtype: 'IML', problem: 'Type is of LIQUOUR, cannot be deleted!' }], counts, blockers: ['The group has liquor items'], can_delete: false }
    await open()
    expect(await screen.findByText(/group has liquor items/)).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /Delete the above listed items/ })).not.toBeInTheDocument()
  })
})
