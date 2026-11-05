// Item Vs Brand: Load lists the unlinked items, and Replace Rest copies the current row's brand to the rows below it.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import ItemVsBrand from './ItemVsBrand'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const item = (code, brand = '') => ({ code, name: `Item ${code}`, group: 'G', uom: 'BTL', brand })

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (url) => {
    if (url.includes('/masters/brands')) return json([{ code: 'B1' }, { code: 'B2' }])
    if (url.includes('/masters/items')) return json([item('A'), item('B'), item('C'), item('L', 'B1')])
    return json([])
  }))
})
afterEach(() => vi.unstubAllGlobals())

describe('ItemVsBrand', () => {
  it('lists only the unlinked items and replaces the rest from the current row', async () => {
    const user = userEvent.setup()
    render(<ItemVsBrand />)
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    expect(await screen.findByLabelText('Brand for Item A')).toBeInTheDocument()
    expect(screen.queryByLabelText('Brand for Item L')).not.toBeInTheDocument()

    await user.selectOptions(await screen.findByLabelText('Brand for Item B'), 'B2')   // current row = B
    await user.click(screen.getByRole('button', { name: /Replace Rest/ }))
    expect(screen.getByLabelText('Brand for Item B')).toHaveValue('B2')
    expect(screen.getByLabelText('Brand for Item C')).toHaveValue('B2')                // below it: changed
    expect(screen.getByLabelText('Brand for Item A')).toHaveValue('')                  // above it: untouched
  })
})
