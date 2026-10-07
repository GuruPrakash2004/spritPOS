// Without a browser key the picker explains how to set one up and loads nothing from Google.
import { render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it, vi } from 'vitest'

afterEach(() => { vi.unstubAllEnvs(); vi.resetModules() })

describe('ShopPlacePicker', () => {
  it('asks for the browser key and adds no Google script without one', async () => {
    vi.stubEnv('VITE_GOOGLE_MAPS_BROWSER_KEY', '')   // the key is read when the module loads, and a real one may be in .env
    vi.resetModules()
    const { default: ShopPlacePicker } = await import('./ShopPlacePicker')
    render(<ShopPlacePicker placeId="" readOnly={false} onPick={() => {}} />)
    expect(screen.getByText(/VITE_GOOGLE_MAPS_BROWSER_KEY/)).toBeInTheDocument()
    expect(document.querySelector('script[src*="googleapis"]')).toBeNull()
  })
})
