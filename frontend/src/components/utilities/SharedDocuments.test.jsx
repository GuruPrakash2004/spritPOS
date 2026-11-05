// Shared Documents: lists the database's documents and filters them by group.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import SharedDocuments from './SharedDocuments'

const rows = [{ id: 1, group: 'Forms', file: 'form1.pdf', remarks: 'tax' }, { id: 2, group: 'Policies', file: 'policy.docx', remarks: '' }]
beforeEach(() => { vi.stubGlobal('fetch', vi.fn(async () => new Response(JSON.stringify(rows), { status: 200, headers: { 'Content-Type': 'application/json' } }))) })
afterEach(() => vi.unstubAllGlobals())

describe('SharedDocuments', () => {
  it('lists every document and narrows to a group', async () => {
    const user = userEvent.setup()
    render(<SharedDocuments />)
    expect(await screen.findByText('form1.pdf')).toBeInTheDocument()
    expect(screen.getByText('policy.docx')).toBeInTheDocument()
    await user.selectOptions(screen.getByLabelText('Group'), 'Policies')
    expect(screen.queryByText('form1.pdf')).not.toBeInTheDocument()
    expect(screen.getByText('policy.docx')).toBeInTheDocument()
  })
})
