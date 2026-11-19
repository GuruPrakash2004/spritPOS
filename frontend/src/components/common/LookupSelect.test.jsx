// A table drop-down (Doc Designer combo) keeps the server's order unless the screen asks for name or numeric order (Item Master: Brand, Pkg).
import { render, screen, waitFor } from '@testing-library/react'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import LookupSelect from './LookupSelect'

const json = (body) => new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
const ROWS = [{ value: 1, label: '1000ML' }, { value: 2, label: '180ML' }, { value: 3, label: '750ML' }, { value: 4, label: '2' }]

beforeEach(() => { vi.stubGlobal('fetch', vi.fn(async () => json(ROWS))) })
afterEach(() => vi.unstubAllGlobals())

const labels = () => screen.getAllByRole('option').map(o => o.textContent).filter(Boolean)

describe('LookupSelect order', () => {
  it('lists packages by their number when asked', async () => {
    render(<LookupSelect id="p" docId={1} code="PKGITEM" value="" onChange={() => {}} order="numeric" />)
    await waitFor(() => expect(labels()).toHaveLength(4))
    expect(labels()).toEqual(['2', '180ML', '750ML', '1000ML'])
  })

  it('keeps the server order without a sort', async () => {
    render(<LookupSelect id="p" docId={1} code="OTHER" value="" onChange={() => {}} />)
    await waitFor(() => expect(labels()).toHaveLength(4))
    expect(labels()).toEqual(['1000ML', '180ML', '750ML', '2'])
  })
})
