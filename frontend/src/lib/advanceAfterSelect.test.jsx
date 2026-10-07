// Picking an option in a drop-down moves to the next field; arrow keys stepping through it do not.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it } from 'vitest'
import { useAdvanceAfterSelect } from './keys'

function Form() {
  useAdvanceAfterSelect()
  return (
    <form>
      <select aria-label="Type"><option>A</option><option>B</option></select>
      <input aria-label="Next" />
    </form>
  )
}

describe('useAdvanceAfterSelect', () => {
  it('moves to the next field after a pick', async () => {
    render(<Form />)
    await userEvent.selectOptions(screen.getByLabelText('Type'), 'B')
    await new Promise(r => setTimeout(r, 10))
    expect(screen.getByLabelText('Next')).toHaveFocus()
  })

  it('stays put while arrow keys step through the options', async () => {
    render(<Form />)
    const sel = screen.getByLabelText('Type')
    sel.focus()
    await userEvent.keyboard('{ArrowDown}')
    await new Promise(r => setTimeout(r, 10))
    expect(sel).toHaveFocus()
  })
})
