// Tests for Global Settings: rows load with what they change, a wrong value is shown under it and nothing is
// sent, a good value is saved only after its popup, and read-only SMTP rows can't be edited.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import GlobalSettings from './GlobalSettings'

const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

const ROWS = [
  { code: 'SPID', group: 'Invoice', caption: 'Sales Person required in invoice', value: 'Y', kind: 'yn', remarks: '', effect: 'Sales Invoice save refuses a bill without a salesperson.', hidden: false, source: 'web', readonly: false },
  { code: 'PointsForOneRs', group: 'Loyalty', caption: 'Number of points for 1 Rs', value: '0.01', kind: 'number', remarks: '', effect: 'Points earned on each saved bill.', hidden: false, source: 'web', readonly: false },
  { code: 'SMTPHostName', group: 'SMTP', caption: 'SMTP Host Name', value: 'smtp.example.test', kind: 'text', remarks: '', effect: 'Customised Reports → Email.', hidden: false, source: 'env', readonly: true },
]

let fetchMock
const puts = () => fetchMock.mock.calls.filter(([, init]) => init?.method === 'PUT')

beforeEach(() => {
  fetchMock = vi.fn(async (url, init) => {
    if (init?.method === 'PUT') return json({ ...ROWS[1], ...JSON.parse(String(init.body)) })
    if (url.includes('/utilities/settings')) return json(ROWS)
    return json({})
  })
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('GlobalSettings', () => {
  it('lists each setting with what it changes', async () => {
    render(<GlobalSettings />)
    expect(await screen.findByText('SPID')).toBeInTheDocument()
    expect(screen.getByText('Sales Invoice save refuses a bill without a salesperson.')).toBeInTheDocument()
    expect(screen.getByLabelText('Value of SMTPHostName')).toBeDisabled()
  })

  it('shows a wrong number under the value and sends nothing', async () => {
    const user = userEvent.setup()
    render(<GlobalSettings />)
    const box = await screen.findByLabelText('Value of PointsForOneRs')
    await user.clear(box)
    await user.type(box, 'abc')
    await user.click(screen.getByRole('button', { name: /save/i }))

    expect(await screen.findByText('Enter a number (0 or more)')).toBeInTheDocument()
    expect(puts()).toEqual([])
    expect(toastStore.defer).not.toHaveBeenCalled()
  })

  it('saves a changed value after its popup', async () => {
    const user = userEvent.setup()
    render(<GlobalSettings />)
    const box = await screen.findByLabelText('Value of PointsForOneRs')
    await user.clear(box)
    await user.type(box, '0.05')
    await user.click(screen.getByRole('button', { name: /save \(1\)/i }))

    await waitFor(() => expect(puts()).toHaveLength(1))
    expect(toastStore.defer).toHaveBeenCalledWith('ok', '1 setting(s) will be saved when this closes.', expect.any(String))
    const [url, init] = puts()[0]
    expect(url).toBe('/api/utilities/settings/PointsForOneRs')
    expect(JSON.parse(String(init.body))).toEqual({ caption: 'Number of points for 1 Rs', value: '0.05', hidden: false })
  })

  it('sends nothing when the popup is cancelled', async () => {
    vi.mocked(toastStore.defer).mockResolvedValue(false)
    const user = userEvent.setup()
    render(<GlobalSettings />)
    await user.selectOptions(await screen.findByLabelText('Value of SPID'), 'N')
    await user.click(screen.getByRole('button', { name: /save/i }))
    await waitFor(() => expect(toastStore.defer).toHaveBeenCalled())
    expect(puts()).toEqual([])
  })
})
