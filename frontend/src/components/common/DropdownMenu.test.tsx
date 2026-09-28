// Tests for DropdownMenu: opening and closing, and that the menu stays inside a narrow (phone) screen.
import { useRef, useState } from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, describe, expect, it, vi } from 'vitest'
import DropdownMenu, { menuItem } from './DropdownMenu'

// A trigger button plus the menu, wired the way SalesCounterMaster uses it.
function Harness({ onPrint = () => {} }: { onPrint?: () => void }) {
  const ref = useRef<HTMLButtonElement>(null)
  const [open, setOpen] = useState(false)
  return (
    <>
      <button ref={ref} onClick={() => setOpen(o => !o)} aria-expanded={open}>More options</button>
      <DropdownMenu anchorRef={ref} open={open} onClose={() => setOpen(false)}>
        <button className={menuItem} onClick={onPrint}>Print List</button>
      </DropdownMenu>
      <p>Outside</p>
    </>
  )
}

const originalWidth = window.innerWidth

afterEach(() => {
  Object.defineProperty(window, 'innerWidth', { configurable: true, value: originalWidth })
  vi.restoreAllMocks()
})

describe('DropdownMenu', () => {
  it('is hidden until the trigger is clicked', async () => {
    const user = userEvent.setup()
    render(<Harness />)

    expect(screen.queryByRole('menu')).not.toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'More options' }))
    expect(screen.getByRole('menu')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Print List' })).toBeInTheDocument()
  })

  it('runs the item action and closes when an item is clicked', async () => {
    const user = userEvent.setup()
    const onPrint = vi.fn()
    render(<Harness onPrint={onPrint} />)

    await user.click(screen.getByRole('button', { name: 'More options' }))
    await user.click(screen.getByRole('button', { name: 'Print List' }))

    expect(onPrint).toHaveBeenCalledOnce()
    expect(screen.queryByRole('menu')).not.toBeInTheDocument()
  })

  it('closes on Escape and gives focus back to the trigger', async () => {
    const user = userEvent.setup()
    render(<Harness />)
    const trigger = screen.getByRole('button', { name: 'More options' })

    await user.click(trigger)
    await user.keyboard('{Escape}')

    expect(screen.queryByRole('menu')).not.toBeInTheDocument()
    expect(trigger).toHaveFocus()
  })

  it('closes on a click outside', async () => {
    const user = userEvent.setup()
    render(<Harness />)

    await user.click(screen.getByRole('button', { name: 'More options' }))
    await user.click(screen.getByText('Outside'))

    expect(screen.queryByRole('menu')).not.toBeInTheDocument()
  })

  it('stays on screen when the trigger is at the right edge of a phone screen', async () => {
    const user = userEvent.setup()
    Object.defineProperty(window, 'innerWidth', { configurable: true, value: 360 })
    render(<Harness />)
    const trigger = screen.getByRole('button', { name: 'More options' })
    // Trigger hugging the right edge: right-aligning the 176px menu to it would overflow by 3px.
    vi.spyOn(trigger, 'getBoundingClientRect').mockReturnValue(
      { top: 10, bottom: 40, left: 325, right: 355, width: 30, height: 30, x: 325, y: 10, toJSON: () => ({}) },
    )

    await user.click(trigger)

    // 360 (screen) - 176 (menu) - 8 (margin) = 176
    expect(screen.getByRole('menu')).toHaveStyle({ left: '176px', top: '44px', width: '176px' })
  })
})
