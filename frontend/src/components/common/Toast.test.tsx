// The per-screen <Toast> forwards to the shared store; only App's one <SnackbarContainer> draws popups.
import { render, screen } from '@testing-library/react'
import { describe, expect, it } from 'vitest'
import Toast, { SnackbarContainer } from './Toast'

describe('Toast', () => {
  it('shows each message once, not once per container', () => {
    render(<><SnackbarContainer /><Toast kind="ok" text="Bill 26/c10-0083718 saved" /></>)

    expect(screen.getAllByText('Bill 26/c10-0083718 saved')).toHaveLength(1)
  })
})
