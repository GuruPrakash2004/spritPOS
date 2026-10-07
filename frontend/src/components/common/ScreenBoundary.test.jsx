import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import ScreenBoundary from './ScreenBoundary'

let broken = true
function Screen() {
  if (broken) throw new Error('map exploded')
  return <p>screen is fine</p>
}

describe('ScreenBoundary', () => {
  it('shows the error instead of a blank page and recovers on retry', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {})
    render(<ScreenBoundary onHome={() => {}}><Screen /></ScreenBoundary>)
    expect(screen.getByRole('alert')).toHaveTextContent('map exploded')
    broken = false
    await userEvent.click(screen.getByRole('button', { name: 'Try again' }))
    expect(screen.getByText('screen is fine')).toBeInTheDocument()
  })
})
