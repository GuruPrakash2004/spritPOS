import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it } from 'vitest'
import Pager from './Pager'
import { usePage } from './usePage'

const rows = Array.from({ length: 250 }, (_, i) => i)
function List() {
  const p = usePage(rows, 100)
  return <><ul>{p.slice.map(r => <li key={r}>{r}</li>)}</ul><Pager p={p} /></>
}

describe('Pager', () => {
  it('shows one page at a time', async () => {
    render(<List />)
    expect(screen.getAllByRole('listitem')).toHaveLength(100)
    await userEvent.click(screen.getByRole('button', { name: 'Next page' }))
    expect(screen.getByText('Page 2 / 3')).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Next page' }))
    expect(screen.getAllByRole('listitem')).toHaveLength(50)
  })
})
