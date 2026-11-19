// The list window: global search, column filters, sorting, the total, double-click to load, "do not hide", Select Columns, Export.
import { render, screen, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import ListModal from './ListModal'
import * as xls from '../../lib/xlsExport'

const COLUMNS = [{ key: 'code', label: 'Code' }, { key: 'name', label: 'Name' }, { key: 'city', label: 'City' }, { key: 'qty', label: 'Qty', kind: 'number' }]
const ROWS = [
  { code: 'NMM', name: 'Nava Mantralya', city: 'Mysuru', qty: 10 },
  { code: 'RYA', name: 'Rayara Aradhana', city: 'Raichur', qty: 2 },
  { code: 'SLV', name: 'Sri Lakshmi', city: 'Tirupati', qty: 30 },
]
const setup = (props = {}) => {
  const onOpen = vi.fn(), onClose = vi.fn()
  render(<ListModal title="Sannidhi Master" columns={COLUMNS} defaultKeys={['code', 'name']} rows={ROWS} rowKey={r => r.code} storageKey="t" onOpen={onOpen} onClose={onClose} {...props} />)
  return { onOpen, onClose }
}
const bodyCodes = () => screen.getAllByRole('row').slice(2).map(r => within(r).getAllByRole('cell')[0].textContent)

beforeEach(() => { localStorage.clear() })
afterEach(() => vi.restoreAllMocks())

describe('ListModal', () => {
  it('shows only the default columns, with the total', () => {
    setup()
    expect(screen.getByRole('columnheader', { name: /code/i })).toBeInTheDocument()
    expect(screen.queryByRole('columnheader', { name: /city/i })).not.toBeInTheDocument()
    expect(screen.getByText('Total Records: 3 / 3')).toBeInTheDocument()
  })

  it('searches every column, even ones that are not shown', async () => {
    const user = userEvent.setup()
    setup()
    await user.type(screen.getByLabelText('Global search across all columns'), 'raichur')   // City is not a shown column
    expect(bodyCodes()).toEqual(['RYA'])
    expect(screen.getByText('Total Records: 1 / 3')).toBeInTheDocument()
  })

  it('filters under a header and sorts by clicking it', async () => {
    const user = userEvent.setup()
    setup()
    await user.type(screen.getByLabelText('Filter Name'), 'sri')
    expect(bodyCodes()).toEqual(['SLV'])
    await user.clear(screen.getByLabelText('Filter Name'))
    await user.click(screen.getByRole('button', { name: /sort by code/i }))
    await user.click(screen.getByRole('button', { name: /sort by code/i }))
    expect(bodyCodes()).toEqual(['SLV', 'RYA', 'NMM'])   // the second click sorts descending
  })

  it('loads the double-clicked record and closes', async () => {
    const user = userEvent.setup()
    const { onOpen, onClose } = setup()
    await user.dblClick(screen.getByText('RYA'))
    expect(onOpen).toHaveBeenCalledWith(ROWS[1])
    expect(onClose).toHaveBeenCalledTimes(1)
  })

  it('stays open after loading when "Do NOT hide" is ticked, and opens the selected row with the button', async () => {
    const user = userEvent.setup()
    const { onOpen, onClose } = setup()
    await user.click(screen.getByLabelText(/Do NOT hide after loading/))
    expect(screen.getByRole('button', { name: 'Open Selected' })).toBeDisabled()
    await user.click(screen.getByText('SLV'))
    await user.click(screen.getByRole('button', { name: 'Open Selected' }))
    expect(onOpen).toHaveBeenCalledWith(ROWS[2])
    expect(onClose).not.toHaveBeenCalled()
  })

  it('lets the user pick columns, remembers them, and offers Reset to Default', async () => {
    const user = userEvent.setup()
    setup()
    await user.click(screen.getByRole('button', { name: /select columns/i }))
    await user.click(screen.getByRole('checkbox', { name: 'City' }))
    await user.click(screen.getByRole('button', { name: 'Apply Changes' }))
    expect(screen.getByRole('columnheader', { name: /city/i })).toBeInTheDocument()
    expect(JSON.parse(localStorage.getItem('vlqms_list_t_cols')).visible).toContain('city')
    await user.click(screen.getByRole('button', { name: /select columns/i }))
    await user.click(screen.getByRole('button', { name: 'Reset to Default' }))
    await user.click(screen.getByRole('button', { name: 'Apply Changes' }))
    expect(screen.queryByRole('columnheader', { name: /city/i })).not.toBeInTheDocument()
  })

  it('exports the rows and columns that are shown', async () => {
    const user = userEvent.setup()
    const spy = vi.spyOn(xls, 'downloadXls').mockImplementation(() => {})
    setup()
    await user.type(screen.getByLabelText('Global search across all columns'), 'nmm')
    await user.click(screen.getByRole('button', { name: /export to excel/i }))
    const [, , , columns, rows] = spy.mock.calls[0]
    expect(columns.map(c => c.key)).toEqual(['code', 'name'])
    expect(rows).toEqual([{ code: 'NMM', name: 'Nava Mantralya' }])
  })

  it('shows the selected record as a field list (Transpose)', async () => {
    const user = userEvent.setup()
    setup()
    await user.click(screen.getByText('NMM'))
    await user.click(screen.getByRole('button', { name: /transpose/i }))
    expect(screen.getByRole('rowheader', { name: 'City' })).toBeInTheDocument()
    expect(screen.getByText('Mysuru')).toBeInTheDocument()
  })
})

describe('ListModal closing', () => {
  it('closes with the Close button or the X in the top-right corner, not by Esc or a click outside', async () => {
    const user = userEvent.setup()
    const { onClose } = setup()
    await user.keyboard('{Escape}')
    await user.click(document.querySelector('.modal-backdrop'))
    expect(onClose).not.toHaveBeenCalled()
    await user.click(screen.getByRole('button', { name: 'Close' }))   // the footer button
    expect(onClose).toHaveBeenCalledTimes(1)
    await user.click(screen.getByRole('button', { name: 'Close list' }))   // the X in the header's corner
    expect(onClose).toHaveBeenCalledTimes(2)
  })
})
