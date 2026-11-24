// Other Settings → File Attachment: one tick per master, saved as the master's Global Settings row (Y / N).
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import AttachmentSettings from './AttachmentSettings'
import { toastStore } from '../common/toastStore'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const ROWS = [
  { code: 'AttachFile_items', group: 'File Attachment', caption: 'Show File Attachment on Item Master', value: 'Y', kind: 'yn', hidden: false },
  { code: 'AttachFile_uoms', group: 'File Attachment', caption: 'Show File Attachment on UOM Master', value: 'N', kind: 'yn', hidden: false },
  { code: 'SPID', group: 'Invoice', caption: 'Sales Person required in invoice', value: 'Y', kind: 'yn', hidden: false },
]

let calls
beforeEach(() => {
  calls = []
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  vi.stubGlobal('fetch', vi.fn(async (url, init) => {
    calls.push([String(url), init?.method ?? 'GET', init?.body])
    return json(init?.method === 'PUT' ? {} : ROWS)
  }))
})
afterEach(() => { vi.restoreAllMocks(); vi.unstubAllGlobals() })

describe('AttachmentSettings', () => {
  it('lists only the masters, ticked when their row is not N', async () => {
    render(<AttachmentSettings />)

    expect(await screen.findByRole('checkbox', { name: /item master/i })).toBeChecked()
    expect(screen.getByRole('checkbox', { name: /uom master/i })).not.toBeChecked()
    expect(screen.queryByText(/sales person required/i)).not.toBeInTheDocument()
  })

  it('saves only the changed masters as Y / N', async () => {
    render(<AttachmentSettings />)
    await userEvent.click(await screen.findByRole('checkbox', { name: /item master/i }))
    await userEvent.click(screen.getByRole('button', { name: /save/i }))

    await waitFor(() => expect(calls.some(c => c[1] === 'PUT')).toBe(true))
    const puts = calls.filter(c => c[1] === 'PUT')
    expect(puts).toHaveLength(1)
    expect(puts[0][0]).toContain('/utilities/settings/AttachFile_items')
    expect(JSON.parse(puts[0][2]).value).toBe('N')
  })
})
