// Add Line Item (the Purchase Bill's Add item button): the item list and its search stay on the left, the picked item's quantity and rate go on the
// right with the amount, "Add & next" keeps the window open and "Add & close" closes it.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import { AddLineItemModal } from './PurchaseBillModals'

const ITEMS = [
  { stock: 0, code: 'WH1', name: 'Old Monk Rum', pkg: '750ML', shortName: '', hotKey: '', brand: '', clQty: 12, bottlesPerCase: 12, purchaseRate: 400, rate: 520, rol: 5 },
  { stock: 1, code: 'VK1', name: 'Magic Moments Vodka', pkg: '750ML', shortName: '', hotKey: '', brand: '', clQty: 3, bottlesPerCase: 12, purchaseRate: 300, rate: 390, rol: 5 },
]

describe('AddLineItemModal', () => {
  it('lists the items beside the details, filters by the search, and fills the amount for the picked item', async () => {
    const user = userEvent.setup()
    render(<AddLineItemModal items={ITEMS} onAdd={vi.fn()} onClose={vi.fn()} />)
    expect(screen.getByText('Select item from master')).toBeInTheDocument()
    expect(screen.getByText('Magic Moments Vodka 750ML')).toBeInTheDocument()
    await user.type(screen.getByLabelText('Search items'), 'monk')
    expect(screen.queryByText('Magic Moments Vodka 750ML')).not.toBeInTheDocument()
    await user.click(screen.getByText('Old Monk Rum 750ML'))
    expect(screen.getByLabelText(/Purchase rate/)).toHaveValue(400)           // the item's purchase rate comes in
    await user.type(screen.getByLabelText('No. of cases'), '2')
    await user.type(screen.getByLabelText('Loose bottles'), '3')
    expect(screen.getByLabelText('Total bottles')).toHaveValue(27)            // 2 x 12 + 3
    expect(screen.getByLabelText('Amount')).toHaveTextContent('10,800.00')    // 27 x 400
  })

  it('Add & next adds the line and stays open; Add & close adds and closes', async () => {
    const onAdd = vi.fn()
    const user = userEvent.setup()
    render(<AddLineItemModal items={ITEMS} onAdd={onAdd} onClose={vi.fn()} />)
    await user.click(screen.getByText('Old Monk Rum 750ML'))
    await user.type(screen.getByLabelText('Total bottles'), '5')
    await user.click(screen.getByRole('button', { name: /Add & next/ }))
    expect(onAdd).toHaveBeenLastCalledWith({ stock: 0, qty: 5, rate: 400 }, true)
    expect(screen.getByText(/1 item added/)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /Add & next/ })).toBeDisabled()   // nothing picked again yet
    await user.click(screen.getByText('Magic Moments Vodka 750ML'))
    await user.type(screen.getByLabelText('Total bottles'), '2')
    await user.click(screen.getByRole('button', { name: /Add & close/ }))
    expect(onAdd).toHaveBeenLastCalledWith({ stock: 1, qty: 2, rate: 300 }, false)
  })

  it('asks for the quantity when it is missing', async () => {
    const onAdd = vi.fn()
    const user = userEvent.setup()
    render(<AddLineItemModal items={ITEMS} onAdd={onAdd} onClose={vi.fn()} />)
    await user.click(screen.getByText('Old Monk Rum 750ML'))
    await user.click(screen.getByRole('button', { name: /Add & close/ }))
    expect(await screen.findByRole('alert')).toHaveTextContent('Enter the cases or loose bottles.')
    expect(onAdd).not.toHaveBeenCalled()
  })
})
