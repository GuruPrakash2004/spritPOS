// Dialogs from the keyboard: focus moves into the dialog, Tab stays inside, and Esc closes only the top one.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import Modal from './Modal'

describe('Modal', () => {
  it('puts the cursor in the first field and keeps Tab inside', async () => {
    const user = userEvent.setup()
    render(<>
      <input aria-label="Behind" />
      <Modal title="Re-Print Bill" onClose={() => {}} footer={<button>Print</button>}><input aria-label="Bill No" /></Modal>
    </>)

    expect(screen.getByLabelText('Bill No')).toHaveFocus()
    await user.tab()
    expect(screen.getByRole('button', { name: 'Print' })).toHaveFocus()
    await user.tab()
    expect(screen.getByLabelText('Bill No')).toHaveFocus()
  })

  it('closes only the top dialog on Esc', async () => {
    const user = userEvent.setup()
    const outer = vi.fn()
    const inner = vi.fn()
    render(<>
      <Modal title="Outer" onClose={outer}><p>outer</p></Modal>
      <Modal title="Inner" onClose={inner}><p>inner</p></Modal>
    </>)

    await user.keyboard('{Escape}')

    expect(inner).toHaveBeenCalledTimes(1)
    expect(outer).not.toHaveBeenCalled()
  })
})
