// Tests for Customised Reports → Email: address mistakes stay inline (no request), a good request
// carries the report and filters, and the server's answer (e.g. no mail server) shows as a popup.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import ReportEmailDialog from './ReportEmailDialog'
import { toastStore } from '../common/toastStore'

const REQUEST = { name: 'Sales - Day Wise Report', from_date: '2026-09-01', to_date: '2026-09-28', ledger: [], supplier: ['KSBCL'], group: [] }
const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

let fetchMock
beforeEach(() => {
  fetchMock = vi.fn(async () => json({ sent_to: ['owner@example.com'], rows: 7 }))
  vi.stubGlobal('fetch', fetchMock)
})
afterEach(() => vi.unstubAllGlobals())

// Actions wait for their popup (toastStore.defer); here it closes at once so the request goes out.
beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.restoreAllMocks())

describe('ReportEmailDialog', () => {
  it('shows a bad address inline and sends nothing', async () => {
    const user = userEvent.setup()
    render(<ReportEmailDialog request={REQUEST} defaultTo="" onClose={() => {}} />)

    await user.type(screen.getByLabelText(/^To/), 'owner@example')
    await user.click(screen.getByRole('button', { name: 'Send' }))

    expect(screen.getByRole('alert')).toHaveTextContent('"owner@example" is not a valid email address')
    expect(screen.getByLabelText(/^To/)).toHaveFocus()
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('posts the report, its filters and every recipient', async () => {
    const user = userEvent.setup()
    const onClose = vi.fn()
    render(<ReportEmailDialog request={REQUEST} defaultTo="owner@example.com" onClose={onClose} />)

    await user.type(screen.getByLabelText(/^To/), ', accounts@example.com')
    await user.click(screen.getByRole('button', { name: 'Send' }))

    await waitFor(() => expect(onClose).toHaveBeenCalled())
    const [url, init] = fetchMock.mock.calls[0]
    expect(url).toBe('/api/reports/custom/email')
    expect(JSON.parse(String(init.body))).toEqual({ ...REQUEST, to: ['owner@example.com', 'accounts@example.com'], message: '' })
  })

  it('shows the server answer as a popup and stays open', async () => {
    fetchMock.mockResolvedValueOnce(json({ detail: "Email isn't set up: add the SMTP settings" }, 503))
    const add = vi.spyOn(toastStore, 'add')
    const user = userEvent.setup()
    const onClose = vi.fn()
    render(<ReportEmailDialog request={REQUEST} defaultTo="owner@example.com" onClose={onClose} />)

    await user.click(screen.getByRole('button', { name: 'Send' }))

    await waitFor(() => expect(add).toHaveBeenCalledWith('error', "Email isn't set up: add the SMTP settings"))
    expect(onClose).not.toHaveBeenCalled()
  })
})
