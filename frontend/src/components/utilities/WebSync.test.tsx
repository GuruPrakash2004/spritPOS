// Web Sync: shows "Not set up" without a central site, sends the chosen day after its waiting popup, and
// shows a future day as an inline mistake (no request, no popup).
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import WebSync from './WebSync'

const status = {
  configured: true, central: 'central.example.com', every_minutes: 10, last_ok: null, last_day: null,
  last_stock_ok: null, last_error: null, last_error_at: null,
}

vi.mock('../../lib/api', async importOriginal => ({
  ...(await importOriginal<typeof import('../../lib/api')>()),
  api: vi.fn(async () => status),
}))

beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.clearAllMocks())

describe('WebSync', () => {
  it('sends the chosen day after the popup', async () => {
    const { api } = await import('../../lib/api')
    vi.spyOn(toastStore, 'add').mockImplementation(() => {})
    const user = userEvent.setup()
    render(<WebSync />)
    expect(await screen.findByText(/central\.example\.com/)).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /sync now/i }))
    await waitFor(() => expect(api).toHaveBeenCalledWith('/utilities/web-sync', expect.objectContaining({ method: 'POST' })))
    expect(toastStore.defer).toHaveBeenCalledTimes(1)
  })

  it('shows a future day inline and sends nothing', async () => {
    const { api } = await import('../../lib/api')
    const user = userEvent.setup()
    render(<WebSync />)
    await screen.findByText(/central\.example\.com/)
    const input = screen.getByLabelText('Day')
    await user.clear(input)
    await user.type(input, '2999-01-01')
    await user.click(screen.getByRole('button', { name: /sync now/i }))
    expect(screen.getByText('Choose today or an earlier day')).toBeInTheDocument()
    expect(toastStore.defer).not.toHaveBeenCalled()
    expect(api).not.toHaveBeenCalledWith('/utilities/web-sync', expect.objectContaining({ method: 'POST' }))
  })

  it('says when it is not set up', async () => {
    const { api } = await import('../../lib/api')
    vi.mocked(api).mockResolvedValueOnce({ ...status, configured: false, central: '' })
    render(<WebSync />)
    expect(await screen.findByText('Not set up')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /sync now/i })).toBeDisabled()
  })
})
