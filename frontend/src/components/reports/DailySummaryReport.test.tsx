// Refresh fetches the report again and shows a popup on every press, not only the first.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import DailySummaryReport from './DailySummaryReport'

const dsr = {
  total_bills: 0, sales_incl_gst: 0, liquor_basic: 0, non_liquor_basic: 0, profit_of_the_day: 0,
  opening_stock: { qty: 0, value: 0 }, closing_stock: { qty: 0, litres: 0, value: 0 }, purchase: { incl_gst: 0 },
  sales_returns: 0, expenses: 0, cash_balance: 0, redemption: 0,
  cancelled_invoices: [], payment_modes: {}, counter_wise: {}, salesperson_wise: {}, group_wise_sales: {}, expense_ledger_wise: {},
}

vi.mock('../../lib/api', async importOriginal => ({
  ...(await importOriginal<typeof import('../../lib/api')>()),
  api: vi.fn(async () => dsr),
}))

afterEach(() => vi.restoreAllMocks())

describe('DailySummaryReport', () => {
  it('refreshes and says so on every press', async () => {
    const { api } = await import('../../lib/api')
    const popup = vi.spyOn(toastStore, 'add').mockImplementation(() => {})
    render(<DailySummaryReport />)
    await waitFor(() => expect(api).toHaveBeenCalledTimes(1))

    const refresh = screen.getByRole('button', { name: /refresh/i })
    await userEvent.click(refresh)
    await waitFor(() => expect(refresh).toBeEnabled())
    await userEvent.click(refresh)

    await waitFor(() => expect(popup).toHaveBeenCalledTimes(2))
    expect(api).toHaveBeenCalledTimes(3)
    expect(popup).toHaveBeenLastCalledWith('refresh', expect.stringMatching(/refreshed/))
    expect(screen.getByText(/Updated/)).toBeInTheDocument()
  })
})
