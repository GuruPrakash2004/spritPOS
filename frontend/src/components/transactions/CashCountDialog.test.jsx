// Tests for the Denomination window of the Sales Invoice: the desktop's fixed rows, double-click to add, Clear, OK total.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import CashCountDialog from './CashCountDialog'

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async () => new Response(JSON.stringify({ values: ['2000', '500', '200', '100', '50', '20', '10', 'coins'] }), { status: 200, headers: { 'Content-Type': 'application/json' } })))
})
afterEach(() => vi.unstubAllGlobals())

describe('CashCountDialog', () => {
  it('lists seven notes and coins, adds by double click, clears, and applies the total', async () => {
    const user = userEvent.setup()
    const onApply = vi.fn()
    render(<CashCountDialog counts={{}} onApply={onApply} onClose={() => {}} />)
    await waitFor(() => expect(screen.getAllByRole('spinbutton')).toHaveLength(8))
    expect(screen.queryByLabelText('Number of 1500 notes')).not.toBeInTheDocument()

    await user.type(screen.getByLabelText('Number of 2000 notes'), '2')
    await user.dblClick(screen.getByRole('img', { name: '₹500 note' }))
    await user.dblClick(screen.getByRole('img', { name: '₹500 note' }))
    expect(screen.getByLabelText('Number of 500 notes')).toHaveValue(2)
    await user.type(screen.getByLabelText('Coins amount'), '35')
    expect(screen.getByLabelText('Total')).toHaveTextContent('5,035')

    await user.click(screen.getByRole('button', { name: 'Clear' }))
    expect(screen.getByLabelText('Total')).toHaveTextContent('0')
    await user.type(screen.getByLabelText('Number of 100 notes'), '3')
    await user.click(screen.getByRole('button', { name: 'OK' }))
    expect(onApply).toHaveBeenCalledWith({ 100: 3 }, 300)
  })
})
