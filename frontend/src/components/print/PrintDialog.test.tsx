import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, describe, expect, it, vi } from 'vitest'
import PrintDialog from './PrintDialog'

const json = (body: unknown, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

const OPTIONS = {
  kind: 'sales_invoice', short_form: 'SINV', doc_id: 5, doc_name: 'Sales Invoice', designed: true, default: 'GST',
  files: [], stored_proc: false, bridge_configured: true,
  options: [
    { name: 'Thermal', rpt: 'fmtSINV80', copies: 1, default: false, click: 'preview', ctrl_click: '', ctrl_alt_click: '', after_save: '', printer: '', multi_rpt: false },
    { name: 'GST', rpt: 'FMTSINV_GST', copies: 2, default: true, click: 'preview', ctrl_click: '', ctrl_alt_click: '', after_save: 'P', printer: 'POS-1', multi_rpt: false },
  ],
}

afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('PrintDialog', () => {
  it('lists the designer\'s options with the default chosen and previews the PDF the server returns', async () => {
    const calls: { url: string; body?: unknown }[] = []
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      calls.push({ url, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      if (url.startsWith('/api/print/options')) return json(OPTIONS)
      if (url === '/api/print/render') return new Response(new Blob(['%PDF-1.4']), { status: 200, headers: { 'Content-Type': 'application/pdf' } })
      return json({})
    }))
    const tab = { document: { title: '' }, location: { href: '' }, close: vi.fn() } as unknown as Window
    vi.spyOn(window, 'open').mockReturnValue(tab)
    URL.createObjectURL = vi.fn(() => 'blob:x')
    URL.revokeObjectURL = vi.fn()

    render(<PrintDialog kind="sales_invoice" docNo="26/S-0000001" onClose={() => undefined} />)

    const gst = await screen.findByRole('radio', { name: /GST/ })
    expect(gst).toBeChecked()
    expect(screen.getByText('Default')).toBeInTheDocument()
    expect(screen.queryByText(/bridge is not set up/i)).not.toBeInTheDocument()

    await userEvent.click(screen.getByRole('radio', { name: /Thermal/ }))
    await userEvent.click(screen.getByRole('button', { name: /^preview$/i }))

    await waitFor(() => expect(tab.location.href).toBe('blob:x'))
    const sent = calls.find(c => c.url === '/api/print/render')?.body as Record<string, unknown>
    expect(sent).toMatchObject({ kind: 'sales_invoice', doc_no: '26/S-0000001', option: 'Thermal', action: 'preview' })
  })

  it('offers the FMTSINV files when the designer has no option, and warns when the bridge is missing', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => json({
      ...OPTIONS, designed: false, options: [], default: null, files: ['FMTSINV.rpt', 'FMTSINV_GST.rpt'], bridge_configured: false,
    })))
    render(<PrintDialog kind="sales_invoice" docNo="26/S-0000001" onClose={() => undefined} />)
    expect(await screen.findByRole('radio', { name: /FMTSINV_GST/ })).toBeInTheDocument()
    expect(screen.getByRole('alert')).toHaveTextContent(/bridge is not set up/i)
  })

  it('shows the server\'s message when the options can\'t be loaded', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => json({ detail: 'nope' }, 500)))
    render(<PrintDialog kind="sales_invoice" docNo="x" onClose={() => undefined} />)
    expect(await screen.findByText('nope')).toBeInTheDocument()
  })
})
