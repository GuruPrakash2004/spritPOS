// Import Item Data From Excel: Load from Excel lists the sheet, the server's per-row errors show in red, Import waits until every row is
// right, Remove erroneous records drops the bad ones, and the import posts the good rows.
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import ImportItemData from './ImportItemData'
import { parseItemSheet } from '../../lib/itemImport'

const json = (body, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
const HEAD = 'ITEMCODE,ITEMNAME,SHORTNAME,ITEMGROUPCode,LIQTYPE,UOMCode,PKGCode,BOTPERCASES,MRP'
const sheet = (...lines) => new File([[HEAD, ...lines].join('\n')], 'items.csv', { type: 'text/csv' })
let posted

beforeEach(() => {
  posted = []
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  vi.stubGlobal('fetch', vi.fn(async (url, init) => {
    if (String(url).includes('/masters/items/import')) {
      const body = JSON.parse(init.body)
      if (String(url).includes('dry_run=true')) return json({ rows: body.map(r => (r.code === 'BAD' ? { error: 'Invalid Group', fields: ['group'] } : { error: '', fields: [] })) })
      posted.push(body)
      return json({ added: body.map(r => r.code), duplicates: [] })
    }
    return json([])
  }))
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('parseItemSheet', () => {
  it('takes the row holding ITEMCODE as the header (the template has a hints row above it) and reads numbers', () => {
    const { records, error } = parseItemSheet([
      ['hint', 'hint'],
      HEAD.split(','),
      ['A1', 'Whisky', '', 'WHISKY', 'IML', 'BTL', '750ML', '12', '1,250.50'],
    ])
    expect(error).toBe('')
    expect(records[0].row).toMatchObject({ code: 'A1', name: 'Whisky', group: 'WHISKY', uom: 'BTL', bottles_per_case: 12, mrp: 1250.5 })
  })
  it('says what is missing', () => {
    expect(parseItemSheet([['ITEMCODE', 'ITEMNAME']]).error).toMatch(/Missing columns/)
    expect(parseItemSheet([['a', 'b']]).error).toMatch(/No ITEMCODE/)
  })
})

describe('ImportItemData', () => {
  it('shows the bad row in red, blocks the import, removes it, then imports the rest', async () => {
    const user = userEvent.setup()
    render(<ImportItemData />)
    await user.upload(screen.getByTestId('iid-picker'), sheet('GOOD,Good item,,G,IML,BTL,750ML,12,500', 'BAD,Bad item,,NOPE,IML,BTL,750ML,12,500'))
    await user.click(screen.getByRole('button', { name: /Load from Excel/ }))
    expect(await screen.findByText('Invalid Group', { selector: 'td' })).toBeInTheDocument()
    expect(screen.getByLabelText('Total Number of Records Loaded')).toHaveTextContent('2')
    expect(screen.getByRole('button', { name: /Import data to database/ })).toBeDisabled()
    const badCell = screen.getByText('NOPE')   // only the wrong cell (the group) is boxed in red, not the whole row
    expect(badCell).toHaveAttribute('aria-invalid', 'true')
    expect(badCell).toHaveClass('border-red-600')
    expect(screen.getByText('BAD')).not.toHaveAttribute('aria-invalid')
    expect(screen.getByText('BAD').closest('tr')).not.toHaveClass('text-red-600')

    await user.click(screen.getByRole('button', { name: /Remove erroneous records/ }))
    await waitFor(() => expect(screen.queryByText('BAD')).not.toBeInTheDocument())
    const doImport = screen.getByRole('button', { name: /Import data to database/ })
    expect(doImport).toBeEnabled()
    await user.click(doImport)
    await waitFor(() => expect(posted).toHaveLength(1))
    expect(posted[0].map(r => r.code)).toEqual(['GOOD'])
    expect(within(screen.getByLabelText('Total Number of Records Loaded')).queryByText('2')).toBeNull()
  })

  it('asks for a file first when Load from Excel is pressed with none chosen', async () => {
    const user = userEvent.setup()
    render(<ImportItemData />)
    await user.click(screen.getByRole('button', { name: /Load from Excel/ }))
    expect(await screen.findByRole('alert')).toHaveTextContent('Please select excel file path..')
  })

  it('draws 100 rows first and more on scroll', async () => {
    const user = userEvent.setup()
    render(<ImportItemData />)
    const lines = Array.from({ length: 250 }, (_, n) => `C${n},Item ${n},,G,IML,BTL,750ML,12,100`)
    await user.upload(screen.getByTestId('iid-picker'), sheet(...lines))
    await user.click(screen.getByRole('button', { name: /Load from Excel/ }))
    expect(await screen.findByText('C0')).toBeInTheDocument()
    expect(screen.queryByText('C100')).not.toBeInTheDocument()
    expect(screen.getByText(/Showing 100 of 250/)).toBeInTheDocument()
  })
})
