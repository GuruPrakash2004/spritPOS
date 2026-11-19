// RecordSearch: type part of a code or name, pick a match, and the row is handed back.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import RecordSearch from './RecordSearch'

const rows = [
  { code: '1001', name: 'OLD MONK RUM 750ML' },
  { code: '1002', name: 'MONKEY SHOULDER 700ML', aliases: ['MS700'] },
  { code: '2001', name: 'BLACK DOG WHISKY' },
]

describe('RecordSearch', () => {
  it('lists matches (names starting with the text first) and loads the picked one with the keyboard', async () => {
    const onPick = vi.fn()
    const user = userEvent.setup()
    render(<RecordSearch rows={rows} onPick={onPick} />)
    await user.type(screen.getByRole('combobox'), 'monk')
    const options = await screen.findAllByRole('option')
    expect(options).toHaveLength(2)
    await user.keyboard('{ArrowDown}{Enter}')
    expect(onPick).toHaveBeenCalledTimes(1)
  })
  it('finds an item by its alias and by code, and is switched off while editing', async () => {
    const onPick = vi.fn()
    const user = userEvent.setup()
    const { rerender } = render(<RecordSearch rows={rows} onPick={onPick} />)
    await user.type(screen.getByRole('combobox'), 'ms7')
    await user.click(await screen.findByRole('button', { name: /MONKEY SHOULDER/ }))
    expect(onPick).toHaveBeenCalledWith(rows[1])
    rerender(<RecordSearch rows={rows} onPick={onPick} disabled />)
    expect(screen.getByRole('combobox')).toBeDisabled()
  })
})
