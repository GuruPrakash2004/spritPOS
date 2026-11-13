// The header's financial-year chip: a drop-down for an Administrator (choosing a year makes it active for everyone, after a confirmation),
// a plain chip for everyone else.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import FinYearSwitch from './FinYearSwitch'
import { toastStore } from '../common/toastStore'
import * as api from '../../lib/api'

const YEARS = [{ start: '2025-04-01', end: '2026-03-31', active: false }, { start: '2026-04-01', end: '2027-03-31', active: true }]

describe('FinYearSwitch', () => {
  beforeEach(() => { vi.restoreAllMocks() })

  it('is a plain chip for a role that may not change the year, or with a single year', () => {
    const { rerender } = render(<FinYearSwitch label="01/04/2026 - 31/03/2027" years={YEARS} canChange={false} />)
    expect(screen.queryByRole('button')).not.toBeInTheDocument()
    rerender(<FinYearSwitch label="x" years={[YEARS[1]]} canChange />)
    expect(screen.queryByRole('button')).not.toBeInTheDocument()
  })

  it('lists the years, and choosing another asks first and then makes it the active year', async () => {
    const user = userEvent.setup()
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
    vi.spyOn(toastStore, 'add').mockImplementation(() => undefined)
    const activate = vi.spyOn(api, 'activateFinYear').mockResolvedValue({})
    const changed = vi.fn()
    render(<FinYearSwitch label="01/04/2026 - 31/03/2027" years={YEARS} canChange onChanged={changed} />)
    await user.click(screen.getByRole('button', { name: /FY 01\/04\/2026/ }))
    expect(screen.getByText('Active')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /01\/04\/2025 - 31\/03\/2026/ }))
    await user.click(await screen.findByRole('button', { name: /^(yes|ok|confirm|delete)/i }))
    await vi.waitFor(() => expect(activate).toHaveBeenCalledWith('2025-04-01'))
    expect(changed).toHaveBeenCalledWith('01/04/2025 - 31/03/2026')
  })
})
