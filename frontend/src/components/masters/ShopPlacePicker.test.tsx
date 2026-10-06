// Without a browser key the picker explains how to set one up and loads nothing from Google.
import { render, screen } from '@testing-library/react'
import { describe, expect, it } from 'vitest'
import ShopPlacePicker from './ShopPlacePicker'

describe('ShopPlacePicker', () => {
  it('asks for the browser key and adds no Google script without one', () => {
    render(<ShopPlacePicker placeId="" readOnly={false} onPick={() => {}} />)
    expect(screen.getByText(/VITE_GOOGLE_MAPS_BROWSER_KEY/)).toBeInTheDocument()
    expect(document.querySelector('script[src*="googleapis"]')).toBeNull()
  })
})
