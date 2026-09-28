// Runs before every test file (see vitest.config.ts).
// Adds the jest-dom matchers (toBeInTheDocument, toHaveStyle, …) to Vitest's expect, and unmounts
// whatever a test rendered so the next test starts with an empty document.
// Also fills in browser APIs that jsdom doesn't implement.
import '@testing-library/jest-dom/vitest'
import { cleanup } from '@testing-library/react'
import { afterEach } from 'vitest'

afterEach(() => {
  cleanup()
})

// jsdom has no layout, so scrolling is a no-op (forms call this to jump to the first error).
if (!Element.prototype.scrollIntoView) {
  Element.prototype.scrollIntoView = () => {}
}

// jsdom has no media queries: report "no match" (light theme, motion allowed).
if (!window.matchMedia) {
  window.matchMedia = (query: string) => ({
    matches: false, media: query, onchange: null,
    addListener: () => {}, removeListener: () => {}, addEventListener: () => {}, removeEventListener: () => {},
    dispatchEvent: () => false,
  })
}
