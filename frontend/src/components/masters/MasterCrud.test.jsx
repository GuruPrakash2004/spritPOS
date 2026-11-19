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

    await screen.findByRole('region', { name: 'Record details' })
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
    await screen.findByRole('region', { name: 'Record details' })
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

describe('Item Master New mode', () => {
  it('starts with Bottles per Case empty (the exe starts blank)', async () => {
    const user = userEvent.setup()
    setAuthRights(['Item Master'], { 'Item Master': { add: true, edit: true, delete: true } })
    renderItems()
    await user.click(await screen.findByRole('button', { name: /new/i }))
    expect(await screen.findByLabelText(/Bottles per Case/i)).toHaveValue(null)
  })
})

describe('Yes / No fields in view mode', () => {
  const ITEMS = [
    { code: 'A1', name: 'ITEM A1', group: 'G', mrp: 10, qoh: 0, active: true, non_stockable: false },
    { code: 'B2', name: 'ITEM B2', group: 'G', mrp: 10, qoh: 0, active: false, non_stockable: true },
  ]

  it('shows a ticked or empty check box instead of the words Yes / No', async () => {
    const user = userEvent.setup()
    vi.stubGlobal('fetch', vi.fn(async (url) => json(String(url).endsWith('/masters/items') ? ITEMS : [])))
    setAuthRights(['Item Master'])
    renderItems()
    const inactive = await screen.findByRole('checkbox', { name: 'Inactive' })
    expect(inactive).not.toBeChecked()   // an active record: Inactive is empty
    expect(inactive).toBeDisabled()
    expect(screen.getByRole('checkbox', { name: 'Non Stockable' })).not.toBeChecked()
    expect(screen.queryByText('Yes')).not.toBeInTheDocument()
    expect(screen.queryByText('No')).not.toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /next record/i }))
    expect(await screen.findByRole('checkbox', { name: 'Non Stockable' })).toBeChecked()
    expect(screen.getByRole('checkbox', { name: 'Inactive' })).toBeChecked()   // an inactive record: Inactive is ticked
  })
})

describe('Not Active Remarks in view mode', () => {
  const ROW = { code: 'A1', name: 'ITEM A1', group: 'G', mrp: 10, qoh: 0, active: false, extras: { InactiveRmks: 'discontinued by supplier' } }

  it('shows the remark under the Inactive box of an inactive record, and not at all for an active one', async () => {
    const user = userEvent.setup()
    const rows = [ROW, { ...ROW, code: 'B2', name: 'ITEM B2', active: true, extras: { InactiveRmks: '' } }]
    vi.stubGlobal('fetch', vi.fn(async (url) => json(String(url).endsWith('/masters/items') ? rows : [])))
    setAuthRights(['Item Master'])
    render(<MasterCrud title="Item Master" subtitle="Items" path="items" fields={[...ITEM_FIELDS, { key: 'InactiveRmks', label: 'Not Active Remarks', extra: true, type: 'text' }]} />)
    expect(await screen.findByText('discontinued by supplier')).toBeInTheDocument()
    expect(screen.getByText('Not Active Remarks')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /next record/i }))
    expect(await screen.findByRole('checkbox', { name: 'Inactive' })).not.toBeChecked()
    expect(screen.queryByText('Not Active Remarks')).not.toBeInTheDocument()
  })
})

describe('hideExtras (UOM Master)', () => {
  const FIELDS = [
    { key: 'code', label: 'UOM Code', required: true },
    { key: 'name', label: 'UOM Name' },
    { key: 'Gram', label: 'Weight in grams', extra: true, type: 'number', section: 'Other details' },
  ]
  const ROW = { code: 'BTL', name: 'Bottle', active: true, extras: { Gram: 750 } }

  it('draws the database columns beyond the typed fields, unless hideExtras is set', async () => {
    vi.stubGlobal('fetch', vi.fn(async (url) => json(String(url).endsWith('/masters/uoms') ? [ROW] : [])))
    setAuthRights(['UOM Master'])
    const { unmount } = render(<MasterCrud title="UOM Master" subtitle="Units" path="uoms" fields={FIELDS} />)
    expect(await screen.findByText('Weight in grams')).toBeInTheDocument()
    unmount()
    render(<MasterCrud title="UOM Master" subtitle="Units" path="uoms" fields={FIELDS} hideExtras />)
    expect(await screen.findByText('UOM Code')).toBeInTheDocument()
    expect(screen.queryByText('Weight in grams')).not.toBeInTheDocument()
    expect(screen.queryByText('Other details')).not.toBeInTheDocument()
  })
})

describe('hideKeys (UOM Master: no Decimals box)', () => {
  it('does not draw a hidden typed field', async () => {
    const { UOM_FIELDS } = await import('./masterConfigs')
    vi.stubGlobal('fetch', vi.fn(async () => json([])))
    setAuthRights(['UOM Master'])
    render(<MasterCrud title="UOM Master" subtitle="Units" path="uoms" fields={UOM_FIELDS} hideKeys={['decimals']} />)
    expect(await screen.findByText('UOM Code')).toBeInTheDocument()
    expect(screen.queryByText('Decimals')).not.toBeInTheDocument()
  })
})

describe('Package Master layout', () => {
  it('shows Code first and Litres per Bottle inside an "Other details" section below it', async () => {
    const { PACKAGE_FIELDS } = await import('./masterConfigs')
    const { arrangePackageFields } = await import('../../lib/packageLayout')
    vi.stubGlobal('fetch', vi.fn(async (url) => json(String(url).endsWith('/masters/packages') ? [{ code: '750ML', litres_per_bottle: 0.75, active: true }] : [])))
    setAuthRights(['Package Master'])
    render(<MasterCrud title="Package Master" subtitle="Packages" path="packages" fields={PACKAGE_FIELDS} arrange={arrangePackageFields} />)
    const code = await screen.findByText('Package Code')
    const heading = (await screen.findAllByText('Other details'))[0]
    const litres = await screen.findByText(/Litres per Bottle/)
    const follows = (a, b) => Boolean(a.compareDocumentPosition(b) & Node.DOCUMENT_POSITION_FOLLOWING)
    expect(follows(code, heading)).toBe(true)
    expect(follows(heading, litres)).toBe(true)
  })
})

describe('File Attachment button', () => {
  it('is a wide button under the fields, "File Attachment (0)", and needs a saved record', async () => {
    setAuthRights(['Item Master'])
    renderItems()

    const btn = await screen.findByRole('button', { name: /file attachment \(0\)/i })
    expect(btn).toBeDisabled()
  })
})

describe('Item Master Edit mode', () => {
  it('never lets the UOM be changed in Edit (it is changed from Options > Update Item Master Fields)', async () => {
    const ITEMS = [{ code: 'A1', name: 'ITEM A1', group: 'G', uom: 'BTL', mrp: 10, qoh: 0, active: true }]
    vi.stubGlobal('fetch', vi.fn(async (url) => json(String(url).includes('/masters/uoms') ? [{ code: 'BTL', name: 'Bottle' }, { code: 'CASE', name: 'Case' }]
      : String(url).endsWith('/masters/items') ? ITEMS : [])))
    setAuthRights(['Item Master'], { 'Item Master': { add: true, edit: true, delete: true } })
    const user = userEvent.setup()
    renderItems()
    await waitFor(() => expect(screen.getByText(/^1 \/ 1$/)).toBeInTheDocument())

    await user.click(screen.getByRole('button', { name: /^edit$/i }))
    const uom = await screen.findByLabelText(/^UOM/)
    expect(uom).toBeDisabled()                                    // Edit mode: not editable, even for an item with no bills
    expect(screen.getByText(/UOM can't be changed here/i)).toBeInTheDocument()
    expect(await screen.findByLabelText(/^Item Name/i)).not.toBeDisabled()   // the other boxes are
  })
})
