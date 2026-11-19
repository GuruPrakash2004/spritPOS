import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import * as api from '../../lib/api'
import { toastStore } from '../common/toastStore'
import InvoiceSettings from './InvoiceSettings'
import { STYLES, previewNumber } from '../../lib/invoiceFormat'

vi.mock('../../lib/api', async (orig) => ({ ...(await orig()), getInvoiceSettings: vi.fn(), saveInvoiceSettings: vi.fn() }))

const SAMPLE = { prefix: 'JAN', counter: 'C1', year_from: '2026', year_to: '2027', sh_year_from: '26', sh_year_to: '27', doc_no: '00001', sh_doc_no: '1' }
const CELLS = ['5', '/', '2', '-', '7', ...Array(10).fill('')]
const SETTINGS = {
  prefix: '', counter_reset: false, months: Array(12).fill(''), format: CELLS, f12: 'Save and Print', ctrl_f12: 'Save', alt_f12: 'Save and Preview',
  sms: { on_sales: true, on_cancel: true, on_delete: false, sales_value: 1000, sales_msg: 'Invoice <<SINVNO>>', cancel_msg: 'Cancelled', delete_msg: '' },
  update_rate: true, sms_available: true, sample: SAMPLE,
}

beforeEach(() => {
  vi.clearAllMocks()
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  api.getInvoiceSettings.mockResolvedValue(structuredClone(SETTINGS))
  api.saveInvoiceSettings.mockResolvedValue({ message: 'Saved Successfully.' })
})

describe('previewNumber (SetDocInvFmt)', () => {
  it('replaces the numbers 1 to 8 with the sample row and keeps other text', () => {
    expect(previewNumber(CELLS, SAMPLE)).toBe('26/C1-00001')
    expect(previewNumber(['1', '/', '3', '4', '8', 'x'], SAMPLE)).toBe('JAN/2026' + '2027' + '1x')
    expect(previewNumber(['9', '0', ''], SAMPLE)).toBe('90')
  })
})

describe('InvoiceSettings', () => {
  it('opens with the saved values and the number they make', async () => {
    render(<InvoiceSettings />)

    expect(await screen.findByLabelText('Document Number')).toHaveValue('26/C1-00001')
    expect(screen.getByLabelText('Cell 1')).toHaveValue('5')
    expect(screen.getByLabelText('F12')).toHaveValue('Save and Print')
    expect(screen.getByLabelText('Update Rate From PB to Item Master')).toBeChecked()
    expect(screen.queryByLabelText('Jan prefix')).not.toBeInTheDocument()
  })

  it('typing in a cell changes the Document Number at once', async () => {
    render(<InvoiceSettings />)
    await userEvent.clear(await screen.findByLabelText('Cell 6'))
    await userEvent.type(screen.getByLabelText('Cell 6'), '/')
    await userEvent.type(screen.getByLabelText('Cell 7'), '8')

    expect(screen.getByLabelText('Document Number')).toHaveValue('26/C1-00001/1')
  })

  it('Month wise shows the month prefixes with the three fill styles, one upper-case letter each', async () => {
    render(<InvoiceSettings />)
    await userEvent.click(await screen.findByLabelText('Month wise'))

    await userEvent.click(screen.getByRole('button', { name: 'Style-2' }))
    expect(screen.getByLabelText('Jan prefix')).toHaveValue('J')
    expect(screen.getByLabelText('May prefix')).toHaveValue('Y')
    await userEvent.clear(screen.getByLabelText('Dec prefix'))
    await userEvent.type(screen.getByLabelText('Dec prefix'), 'xy')
    expect(screen.getByLabelText('Dec prefix')).toHaveValue('X')
    expect(STYLES['Style-3']).toEqual('123456789OND'.split(''))
  })

  it('refuses with the exe messages and saves nothing', async () => {
    render(<InvoiceSettings />)
    await userEvent.click(await screen.findByLabelText('Day wise'))

    await userEvent.click(screen.getByRole('button', { name: 'Save' }))
    expect(await screen.findByText('Please fill the default Styles')).toBeInTheDocument()

    await userEvent.click(screen.getByRole('button', { name: 'Style-1' }))
    await userEvent.click(screen.getByRole('button', { name: 'Save' }))
    expect(await screen.findByText('Please select 1 as prefix if month or daywise is selected.')).toBeInTheDocument()

    await userEvent.type(screen.getByLabelText('Cell 6'), '1')
    await userEvent.selectOptions(screen.getByLabelText('Ctrl+F12'), 'Save and Print')
    await userEvent.click(screen.getByRole('button', { name: 'Save' }))
    expect(await screen.findByText('Please select distinct Invoice value')).toBeInTheDocument()
    expect(api.saveInvoiceSettings).not.toHaveBeenCalled()
  })

  it('unticking an SMS box clears its message, and the value takes digits only', async () => {
    render(<InvoiceSettings />)
    const value = await screen.findByLabelText('Value')

    await userEvent.clear(value)
    await userEvent.type(value, '12a5')
    expect(value).toHaveValue('125')

    await userEvent.click(screen.getByLabelText('For Cancellation'))
    await userEvent.click(screen.getByLabelText('For Cancellation'))
    expect(screen.getAllByRole('textbox', { name: 'Message' })[1]).toHaveValue('')
  })

  it('Save sends everything once it passes the checks', async () => {
    const onClose = vi.fn()
    render(<InvoiceSettings onClose={onClose} />)
    await userEvent.click(await screen.findByLabelText('Counter Wise Reset'))

    await userEvent.click(screen.getByRole('button', { name: 'Save' }))

    await waitFor(() => expect(api.saveInvoiceSettings).toHaveBeenCalled())
    const sent = api.saveInvoiceSettings.mock.calls[0][0]
    expect(sent).toMatchObject({ prefix: '', counter_reset: true, format: CELLS, f12: 'Save and Print', update_rate: true })
    expect(sent.sms).toMatchObject({ on_sales: true, sales_value: 1000, on_delete: false })
    await waitFor(() => expect(onClose).toHaveBeenCalled())
  })
})

describe('InvoiceSettings when the database is not reachable yet', () => {
  it('shows the error, then opens once Try now finds the database', async () => {
    api.getInvoiceSettings.mockRejectedValueOnce(new Error('The database is not connected right now.'))
    render(<InvoiceSettings />)

    expect(await screen.findByText(/not connected right now/)).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Try now' }))

    expect(await screen.findByLabelText('Document Number')).toHaveValue('26/C1-00001')
  })
})
