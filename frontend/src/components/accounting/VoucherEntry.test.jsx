// Voucher entry: debits must equal credits before it can be saved (shown inline, nothing sent), and a balanced voucher is posted.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { setAuthRights } from '../../lib/api'
import { toastStore } from '../common/toastStore'
import VoucherEntry from './VoucherEntry'

const json = (body, status = 200) => new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })
let posted
let sent
const VOUCHER = { id: 5, doc_no: '26//00005', date: '2026-10-30', narration: 'old', cheque_no: '', cheque_date: '', status: 'SAVED', total: 100,
  lines: [{ ledger_id: 2, ledger: 'Rent', side: 'Dr', amount: 100 }, { ledger_id: 1, ledger: 'Cash', side: 'Cr', amount: 100 }] }

beforeEach(() => {
  setAuthRights(['*'])
  posted = []
  sent = []
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  vi.stubGlobal('fetch', vi.fn(async (url, init) => {
    if (init?.method === 'PUT' || url.endsWith('/cancel')) { sent.push([init.method, url, init.body ? JSON.parse(init.body) : null]); return json({ ...VOUCHER, status: url.endsWith('/cancel') ? 'CANCELLED' : 'SAVED' }) }
    if (/\/vouchers\/payment\/5$/.test(url)) return json(VOUCHER)
    if (/\/vouchers\/payment\?/.test(url)) return json([{ id: 5, doc_no: '26//00005', date: '2026-10-30', narration: 'old', total: 100, status: 'SAVED', added_by: 'ADMIN' }])
    if (init?.method === 'POST') { posted.push(JSON.parse(init.body)); return json({ id: 1, doc_no: '26//00001', total: 100 }, 201) }
    if (url.includes('/accounting/ledgers')) return json([{ id: 1, name: 'Cash', group: 'CASH', type: 'Ledger' }, { id: 2, name: 'Rent', group: 'EXPENSES', type: 'Ledger' }])
    return json([])
  }))
})
afterEach(() => { vi.restoreAllMocks(); vi.unstubAllGlobals() })

async function fill(user, dr, cr) {
  await screen.findAllByRole('option', { name: /Cash/, hidden: true })
  await user.selectOptions(screen.getByLabelText('Ledger row 1'), '2'); await user.type(screen.getByLabelText('Amount row 1'), String(dr))
  await user.selectOptions(screen.getByLabelText('Ledger row 2'), '1'); await user.type(screen.getByLabelText('Amount row 2'), String(cr))
}

describe('VoucherEntry', () => {
  it('refuses unbalanced totals inline and sends nothing', async () => {
    const user = userEvent.setup()
    render(<VoucherEntry kind="payment" title="Payment Voucher" />)
    await fill(user, 100, 90)
    await user.click(screen.getByRole('button', { name: /^Save/ }))
    expect(await screen.findByText('Debit and Credit totals are NOT matching!')).toBeInTheDocument()
    expect(posted).toHaveLength(0)
  })

  it('posts a balanced voucher with its Dr and Cr lines', async () => {
    const user = userEvent.setup()
    render(<VoucherEntry kind="payment" title="Payment Voucher" />)
    await fill(user, 100, 100)
    await user.click(screen.getByRole('button', { name: /^Save/ }))
    await waitFor(() => expect(posted).toHaveLength(1))
    expect(posted[0].lines).toEqual([{ ledger_id: 2, side: 'Dr', amount: 100 }, { ledger_id: 1, side: 'Cr', amount: 100 }])
  })

  it('loads a saved voucher to edit and saves the changes to the same voucher', async () => {
    const user = userEvent.setup()
    render(<VoucherEntry kind="payment" title="Payment Voucher" />)
    await user.click(await screen.findByText('26//00005'))
    await user.click(await screen.findByRole('button', { name: /Edit/ }))
    expect(await screen.findByText(/Editing Payment Voucher 26\/\/00005/)).toBeInTheDocument()
    expect(screen.getByLabelText('Amount row 1')).toHaveValue(100)
    await user.clear(screen.getByLabelText('Amount row 1')); await user.type(screen.getByLabelText('Amount row 1'), '120')
    await user.clear(screen.getByLabelText('Amount row 2')); await user.type(screen.getByLabelText('Amount row 2'), '120')
    await user.click(screen.getByRole('button', { name: /^Save changes/ }))
    await waitFor(() => expect(sent).toHaveLength(1))
    expect(sent[0][0]).toBe('PUT'); expect(sent[0][1]).toMatch(/\/vouchers\/payment\/5$/); expect(sent[0][2].lines[0].amount).toBe(120)
  })

  it('cancels a voucher after the user agrees', async () => {
    const user = userEvent.setup()
    render(<VoucherEntry kind="payment" title="Payment Voucher" />)
    await user.click(await screen.findByText('26//00005'))
    await user.click(await screen.findByRole('button', { name: /Cancel voucher/i }))
    await user.click(await screen.findByRole('button', { name: /^(Yes|OK|Confirm)/ }))
    await waitFor(() => expect(sent.some(x => x[1].endsWith('/5/cancel'))).toBe(true))
  })
})
