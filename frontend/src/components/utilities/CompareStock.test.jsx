// Tests for Compare Stock: comparing without both sheets is shown inline (no popup), and two CSV sheets are
// compared after the waiting popup with the desktop statuses.
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import CompareStock from './CompareStock'

beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => vi.restoreAllMocks())

const csv = (name, text) => new File([text], name, { type: 'text/csv' })

describe('CompareStock', () => {
  it('asks for both sheets inline', async () => {
    const user = userEvent.setup()
    render(<CompareStock />)
    await user.click(screen.getByRole('button', { name: /compare stocks/i }))
    expect(screen.getByText('Pick the Old stock sheet')).toBeInTheDocument()
    expect(screen.getByText('Pick the New stock sheet')).toBeInTheDocument()
    expect(toastStore.defer).not.toHaveBeenCalled()
  })

  it('compares two sheets', async () => {
    const user = userEvent.setup()
    render(<CompareStock />)
    fireEvent.change(screen.getByLabelText('Import Old stock sheet'), { target: { files: [csv('old.csv', 'Item Code,Item Name,Closing Qty,Value\nA,Alpha,5,50\nB,Beta,2,20\n')] } })
    fireEvent.change(screen.getByLabelText('Import New stock sheet'), { target: { files: [csv('new.csv', 'Item Code,Item Name,Closing Qty,Value\nA,Alpha,5,50\nB,Beta,3,30\nC,Gamma,1,1\n')] } })
    await screen.findByText('new.csv')
    await user.click(screen.getByRole('button', { name: /compare stocks/i }))
    expect(await screen.findByText('Qty Mismatch')).toBeInTheDocument()
    expect(screen.getByText('Item Not found (Old)')).toBeInTheDocument()
    await waitFor(() => expect(screen.queryByText('Alpha')).not.toBeInTheDocument())   // matching items hidden
  })
})
