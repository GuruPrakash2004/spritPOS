// Central site: sign-in mistakes are inline (nothing sent), and the day summary shows the totals and flags a
// shop that hasn't sent the day.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../components/common/toastStore'
import CentralLogin from './CentralLogin'
import ShopDashboard from './ShopDashboard'
import type { CentralDsr } from './types'

const dsr: CentralDsr = {
  day: '2026-10-01', client: null,
  summary: { TotBills: 4, TotSalesAmt: 1234.5, TotCashAmt: 1000 },
  shops: [
    { client: 'SHOP1', name: 'Lakshmi Wines', last_sync: new Date().toISOString(), synced: true, TotBills: 4, TotSalesAmt: 1234.5 },
    { client: 'SHOP2', name: 'Ganesh Wines', last_sync: null, synced: false },
  ],
  tables: { RptDSRGroupWiseSales: [{ GroupName: 'BEER', TotQty: 6, TotAmt: 1080 }] },
}

vi.mock('../lib/api', async importOriginal => ({
  ...(await importOriginal<typeof import('../lib/api')>()),
  api: vi.fn(async () => dsr),
}))

beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.clearAllMocks())

describe('CentralLogin', () => {
  it('shows missing fields inline and sends nothing', async () => {
    const { api } = await import('../lib/api')
    const user = userEvent.setup()
    render(<CentralLogin onSignedIn={() => {}} />)
    await user.click(screen.getByRole('button', { name: /sign in/i }))
    expect(screen.getByText('Enter your user name')).toBeInTheDocument()
    expect(screen.getByText('Enter your password')).toBeInTheDocument()
    expect(api).not.toHaveBeenCalled()
  })
})

describe('ShopDashboard', () => {
  it('shows the totals and flags a shop that has not sent the day', async () => {
    render(<ShopDashboard shops={[]} />)
    expect(await screen.findByText('₹ 1,234.50')).toBeInTheDocument()
    expect(screen.getByText("Hasn't sent this day")).toBeInTheDocument()
    expect(screen.getByText('Not syncing')).toBeInTheDocument()
    expect(screen.getByText('BEER')).toBeInTheDocument()
  })
})
