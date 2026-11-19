// Item Vs Brand: Load lists the unlinked items, and Replace Rest copies the current row's brand to the rows below it.
import { fireEvent, render, screen } from '@testing-library/react'
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

describe('ItemVsBrand paging and counts', () => {
  it('shows the Total / Linked / Unlinked counts and draws 20 rows more on every scroll to the end', async () => {
    const many = Array.from({ length: 45 }, (_, n) => item(`U${String(n).padStart(2, '0')}`))
    vi.stubGlobal('fetch', vi.fn(async (url) => {
      if (url.includes('/masters/brands')) return json([{ code: 'B1' }])
      if (url.includes('/masters/items')) return json([...many, item('L1', 'B1'), item('L2', 'B1')])
      return json([])
    }))
    const user = userEvent.setup()
    render(<ItemVsBrand />)
    const counts = await screen.findByLabelText('Item counts')
    expect(counts).toHaveTextContent('Total Items: 47')
    expect(counts).toHaveTextContent('Linked: 2')
    expect(counts).toHaveTextContent('Unlinked: 45')
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    expect(await screen.findAllByRole('combobox')).toHaveLength(20)
    expect(screen.getByText(/Total Records: 45\/45 unlinked/)).toBeInTheDocument()
    const grid = screen.getAllByRole('combobox')[0].closest('div.overflow-auto')
    Object.defineProperty(grid, 'scrollHeight', { configurable: true, value: 1000 })
    Object.defineProperty(grid, 'clientHeight', { configurable: true, value: 400 })
    grid.scrollTop = 600
    fireEvent.scroll(grid)
    expect(await screen.findAllByRole('combobox')).toHaveLength(40)
    fireEvent.scroll(grid)
    expect(await screen.findAllByRole('combobox')).toHaveLength(45)
    expect(screen.getByText(/showing all 45/)).toBeInTheDocument()
  })
})

describe('ItemVsBrand "no brand" rows', () => {
  it("counts an item holding the desktop's '-' brand as unlinked", async () => {
    vi.stubGlobal('fetch', vi.fn(async (url) => {
      if (url.includes('/masters/brands')) return json([{ code: 'B1' }])
      if (url.includes('/masters/items')) return json([item('A', '-'), item('B', '-'), item('L', 'B1')])
      return json([])
    }))
    const user = userEvent.setup()
    render(<ItemVsBrand />)
    const counts = await screen.findByLabelText('Item counts')
    expect(counts).toHaveTextContent('Linked: 1')
    expect(counts).toHaveTextContent('Unlinked: 2')
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    expect(await screen.findByLabelText('Brand for Item A')).toHaveValue('')
    expect(screen.queryByLabelText('Brand for Item L')).not.toBeInTheDocument()
    expect(screen.getByText(/Total Records: 2\/2 unlinked/)).toBeInTheDocument()
  })
})

describe('ItemVsBrand Save / Cancel', () => {
  it('keeps Save and Cancel beside the list and Cancel drops the unsaved choices', async () => {
    const user = userEvent.setup()
    render(<ItemVsBrand />)
    await user.click(screen.getByRole('button', { name: /^Load/ }))
    const save = screen.getByRole('button', { name: /^Save/ })
    const cancel = screen.getByRole('button', { name: /^Cancel/ })
    expect(save).toBeDisabled()
    expect(cancel).toBeDisabled()
    await user.selectOptions(await screen.findByLabelText('Brand for Item A'), 'B1')
    expect(save).toBeEnabled()
    expect(screen.getByText(/1 unsaved change/)).toBeInTheDocument()
    await user.click(cancel)
    expect(screen.getByLabelText('Brand for Item A')).toHaveValue('')
    expect(save).toBeDisabled()
  })
})
