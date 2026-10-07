// The per-screen <Toast> forwards to the shared store; only App's one <SnackbarContainer> draws popups.
// A waiting popup runs its action at once on Enter and cancels it on Esc (billing from the keyboard).
import { act, render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import Toast, { SnackbarContainer } from './Toast'
import { toastStore } from './toastStore'

describe('Toast', () => {
  it('shows each message once, not once per container', () => {
    render(<><SnackbarContainer /><Toast kind="ok" text="Bill 26/c10-0083718 saved" /></>)

    expect(screen.getAllByText('Bill 26/c10-0083718 saved')).toHaveLength(1)
  })

  it('runs a waiting action now on Enter', async () => {
    const user = userEvent.setup()
    render(<SnackbarContainer />)
    let go = Promise.resolve(false)
    act(() => { go = toastStore.defer('ok', 'Bill will be saved when this closes.') })
    expect(screen.getByText(/Enter: do it now/)).toBeInTheDocument()

    await user.keyboard('{Enter}')

    await expect(go).resolves.toBe(true)
  })

  it('cancels a waiting action on Esc', async () => {
    const user = userEvent.setup()
    render(<SnackbarContainer />)
    let go = Promise.resolve(true)
    act(() => { go = toastStore.defer('delete', 'Bill will be cancelled when this closes.', 'Not cancelled.') })

    await user.keyboard('{Escape}')

    await expect(go).resolves.toBe(false)
  })

  it('leaves a scan (Enter in a search box with text) alone while a popup waits', async () => {
    const user = userEvent.setup()
    const scanned = vi.fn()
    render(<><SnackbarContainer /><input role="combobox" aria-label="Search" onKeyDown={e => e.key === 'Enter' && scanned()} /></>)
    let settled = false
    act(() => { void toastStore.defer('ok', 'Held bill will be saved when this closes.').then(() => { settled = true }) })

    await user.type(screen.getByLabelText('Search'), 'BLND-750{Enter}')

    expect(scanned).toHaveBeenCalledTimes(1)
    await Promise.resolve()
    expect(settled).toBe(false)
  })
})
