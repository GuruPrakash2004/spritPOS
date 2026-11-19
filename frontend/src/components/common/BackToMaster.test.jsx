// BackToMaster: the button every Item Master option page has, which returns to the Item Master.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import BackToMaster from './BackToMaster'
import { OPEN_MODULE } from '../../lib/nav'

describe('BackToMaster', () => {
  it('asks the app to open the Item Master', async () => {
    const opened = vi.fn()
    window.addEventListener(OPEN_MODULE, opened)
    render(<BackToMaster />)
    await userEvent.click(screen.getByRole('button', { name: /Back to Item Master/ }))
    window.removeEventListener(OPEN_MODULE, opened)
    expect(opened.mock.calls[0][0].detail).toBe('Item Master')
  })
})
