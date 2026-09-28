// Popup timing: 1.5 s on large screens (1024 px and up), 3 s on smaller ones.
import { afterEach, describe, expect, it, vi } from 'vitest'
import { LARGE_SCREEN_QUERY, toastDuration } from './toastStore'

const screenIsLarge = (large: boolean) =>
  vi.spyOn(window, 'matchMedia').mockImplementation((query: string) =>
    ({ matches: large && query === LARGE_SCREEN_QUERY, media: query } as MediaQueryList))

afterEach(() => vi.restoreAllMocks())

describe('toastDuration', () => {
  it('is 1.5 s on a large screen', () => {
    screenIsLarge(true)
    expect(toastDuration()).toBe(1500)
  })

  it('is 3 s on a medium or small screen', () => {
    screenIsLarge(false)
    expect(toastDuration()).toBe(3000)
  })
})
