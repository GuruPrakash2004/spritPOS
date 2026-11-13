// The item alias grid follows the exe's: Tab / Enter on a filled last row adds a row, 30 characters, no apostrophe,
// removing asks first and always leaves one row.
import { useState } from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import AliasGrid from './AliasGrid'

function Harness({ start = [], confirmRemove }) {
  const [v, setV] = useState(start)
  return (<>
    <AliasGrid id="al" value={v} onChange={setV} locked={false} confirmRemove={confirmRemove} />
    <output data-testid="value">{JSON.stringify(v)}</output>
  </>)
}
const value = () => JSON.parse(screen.getByTestId('value').textContent)

describe('AliasGrid', () => {
  it('starts with one empty row', () => {
    render(<Harness />)
    expect(screen.getAllByRole('textbox')).toHaveLength(1)
  })

  it('Tab on a filled last row adds a row and moves into it; on an empty last row it does not', async () => {
    const user = userEvent.setup()
    render(<Harness />)
    await user.type(screen.getByLabelText('Alias 1'), 'RC')
    await user.keyboard('{Tab}')
    expect(screen.getAllByRole('textbox')).toHaveLength(2)
    expect(screen.getByLabelText('Alias 2')).toHaveFocus()
    await user.keyboard('{Tab}')   // the new row is empty: no third row
    expect(screen.getAllByRole('textbox')).toHaveLength(2)
  })

  it('Enter moves down the rows and adds a row after the last filled one', async () => {
    const user = userEvent.setup()
    render(<Harness start={['A', 'B']} />)
    screen.getByLabelText('Alias 1').focus()
    await user.keyboard('{Enter}')
    expect(screen.getByLabelText('Alias 2')).toHaveFocus()
    await user.keyboard('{Enter}')
    expect(screen.getAllByRole('textbox')).toHaveLength(3)
  })

  it('keeps 30 characters and drops apostrophes', async () => {
    const user = userEvent.setup()
    render(<Harness />)
    await user.type(screen.getByLabelText('Alias 1'), "it's " + 'x'.repeat(40))
    expect(value()[0]).toBe('its ' + 'x'.repeat(26))
    expect(value()[0]).toHaveLength(30)
  })

  it('asks before removing, and leaves one empty row when the last alias goes', async () => {
    const user = userEvent.setup()
    const confirmRemove = vi.fn().mockResolvedValueOnce(false).mockResolvedValue(true)
    render(<Harness start={['ONLY']} confirmRemove={confirmRemove} />)
    await user.click(screen.getByRole('button', { name: 'Remove alias' }))
    expect(confirmRemove).toHaveBeenCalledWith('ONLY')
    expect(value()).toEqual(['ONLY'])      // declined
    await user.click(screen.getByRole('button', { name: 'Remove alias' }))
    expect(value()).toEqual([''])          // accepted: one empty row stays
    expect(screen.getAllByRole('textbox')).toHaveLength(1)
  })
})

describe('AliasGrid inside a screen that saves on Enter', () => {
  it('Enter adds the typed alias as a row and does not reach the screen\'s save handler', async () => {
    const user = userEvent.setup()
    const save = vi.fn()
    const onKey = (e) => { if (e.key === 'Enter') save() }   // what MasterCrud's window handler does
    window.addEventListener('keydown', onKey)
    render(<Harness />)
    await user.type(screen.getByLabelText('Alias 1'), 'ABC{Enter}')
    await user.type(screen.getByLabelText('Alias 2'), 'DEF{Enter}')
    window.removeEventListener('keydown', onKey)
    expect(save).not.toHaveBeenCalled()
    expect(value()).toEqual(['ABC', 'DEF', ''])
  })
})
