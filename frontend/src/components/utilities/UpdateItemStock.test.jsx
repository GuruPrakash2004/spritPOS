// Update Item Stock: a loaded sheet shows its erroneous rows in red, they can be removed, and the import waits until every row is right.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import UpdateItemStock from './UpdateItemStock'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })

beforeEach(() => {
  vi.stubGlobal('fetch', vi.fn(async (_url, init) => {
    const body = JSON.parse(init.body)
    return json({ ok: false, importable: 1, rows: body.rows.map((r, i) => ({ row: i + 1, error: r.item_code === 'BAD' ? 'Item not found' : '' })) })
  }))
})
afterEach(() => vi.unstubAllGlobals())

describe('UpdateItemStock', () => {
  it('lists the loaded rows, marks the erroneous one, and keeps Import disabled', async () => {
    const user = userEvent.setup()
    render(<UpdateItemStock />)
    const csv = new File(['ItemCode,ItemName,UOM,Stock,Rate\nWH1,Whisky,BTL,5,100\nBAD,Unknown,BTL,2,50\n'], 'stock.csv', { type: 'text/csv' })
    await user.upload(screen.getByLabelText('Select Excel File'), csv)
    expect(await screen.findByText('Item not found')).toBeInTheDocument()
    expect(screen.getByText(/Total Number of Records Loaded/)).toHaveTextContent('2')
    expect(screen.getByRole('button', { name: /Import data to database/ })).toBeDisabled()
    await waitFor(() => expect(screen.getByRole('button', { name: /Remove erroneous records/ })).toBeEnabled())
  })
})
