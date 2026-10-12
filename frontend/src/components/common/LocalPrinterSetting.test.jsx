// The printer list in Print Settings comes from the PC the page is open on (the Print Agent), not from the server.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'

vi.mock('../../lib/localPrint', () => ({
  detectAgent: vi.fn(), PC_PREFIX: 'pc:', rememberPrinter: vi.fn(), savedPrinter: vi.fn(() => ''),
}))
vi.mock('../../lib/api', () => ({ downloadPrintAgent: vi.fn(), errorText: (e) => String(e?.message ?? e) }))
import { toastStore } from './toastStore'
import { detectAgent, rememberPrinter } from '../../lib/localPrint'
import LocalPrinterSetting from './LocalPrinterSetting'

beforeEach(() => { vi.spyOn(toastStore, 'defer').mockResolvedValue(true) })
afterEach(() => { vi.restoreAllMocks(); vi.clearAllMocks() })

describe('LocalPrinterSetting', () => {
  it('lists the printers of this PC (from the agent) and remembers the one picked', async () => {
    detectAgent.mockResolvedValue({ host: 'CLIENT-PC', default: 'EPSON TM-T82', printers: ['EPSON TM-T82', 'HP LaserJet (network)', 'Microsoft Print to PDF'] })
    const user = userEvent.setup()
    render(<LocalPrinterSetting />)

    expect(await screen.findByRole('option', { name: 'HP LaserJet (network)' })).toBeInTheDocument()
    expect(screen.getByRole('option', { name: 'EPSON TM-T82 (default)' })).toBeInTheDocument()
    expect(screen.getByText(/CLIENT-PC/)).toBeInTheDocument()

    await user.selectOptions(screen.getByLabelText(/printer for direct prints from this pc/i), 'HP LaserJet (network)')
    await user.click(screen.getByRole('button', { name: /save printer/i }))
    await waitFor(() => expect(rememberPrinter).toHaveBeenCalledWith('pc:HP LaserJet (network)'))
  })

  it('says the agent is needed when this PC has none, and offers its download', async () => {
    detectAgent.mockResolvedValue(null)
    render(<LocalPrinterSetting />)

    expect(await screen.findByRole('button', { name: /download print agent/i })).toBeInTheDocument()
    expect(screen.getByLabelText(/printer for direct prints from this pc/i)).toBeDisabled()
    expect(screen.getByRole('button', { name: /save printer/i })).toBeDisabled()
  })
})
