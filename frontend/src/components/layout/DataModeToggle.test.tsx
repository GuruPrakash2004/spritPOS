// The header's Test data switch: an Administrator switches it (after its popup), others only see a badge.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import DataModeToggle from './DataModeToggle'

const json = (body: unknown) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
let fetchMock: ReturnType<typeof vi.fn>
const reload = vi.fn()

beforeEach(() => {
  fetchMock = vi.fn(async () => json({ data_mode: 'test' }))
  vi.stubGlobal('fetch', fetchMock)
  vi.stubGlobal('location', { ...window.location, reload })
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})
afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks(); reload.mockClear() })

describe('DataModeToggle', () => {
  it('lets an Administrator switch test data on', async () => {
    render(<DataModeToggle mode="live" isAdmin />)
    await userEvent.click(screen.getByRole('button', { name: /test data off/i }))
    await waitFor(() => expect(reload).toHaveBeenCalled())
    const [url, init] = fetchMock.mock.calls[0]
    expect(String(url)).toMatch(/\/utilities\/test-data$/)
    expect(JSON.parse(String(init.body))).toEqual({ on: true })
  })

  it('shows other users only a badge, and only while test data is on', () => {
    const { rerender } = render(<DataModeToggle mode="test" isAdmin={false} />)
    expect(screen.getByText('Test data')).toBeInTheDocument()
    expect(screen.queryByRole('button')).not.toBeInTheDocument()
    rerender(<DataModeToggle mode="live" isAdmin={false} />)
    expect(screen.queryByText('Test data')).not.toBeInTheDocument()
  })
})
