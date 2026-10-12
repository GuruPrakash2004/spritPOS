// Removing a row that has something on it asks first (a popup with Cancel / Confirm); an empty row goes without asking.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import ExpenseEntry from './ExpenseEntry'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (url) => json(url.includes('/masters/ledgers') ? [{ code: 'TEA', name: 'TEA', group: 'EXPENSES' }] : [])))
})
afterEach(() => vi.unstubAllGlobals())

describe('ExpenseEntry removing a row', () => {
  it('asks before removing a row with an amount, and keeps it on Cancel', async () => {
    const user = userEvent.setup()
    render(<ExpenseEntry />)
    await user.type(screen.getByLabelText('Amount row 1'), '250')

    await user.click(screen.getByRole('button', { name: 'Remove row 1' }))
    expect(await screen.findByText(/Are you sure you want to delete row 1 \(₹ 250\)\?/)).toBeInTheDocument()

    await user.click(screen.getByRole('button', { name: 'Cancel' }))
    expect(screen.getByLabelText('Amount row 1')).toHaveValue(250)          // still there
  })

  it('removes the row after Confirm', async () => {
    const user = userEvent.setup()
    render(<ExpenseEntry />)
    await user.type(screen.getByLabelText('Amount row 1'), '250')
    await user.click(screen.getByRole('button', { name: 'Remove row 1' }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))

    expect(screen.getByLabelText('Amount row 1')).toHaveValue(null)         // the only row is blank again
  })

  it('does not ask for an empty row', async () => {
    const user = userEvent.setup()
    render(<ExpenseEntry />)
    await user.click(screen.getByRole('button', { name: 'Add Row' }))
    await user.click(screen.getByRole('button', { name: 'Remove row 2' }))

    expect(screen.queryByText(/Are you sure/)).not.toBeInTheDocument()
    expect(screen.queryByLabelText('Amount row 2')).not.toBeInTheDocument()
  })
})
