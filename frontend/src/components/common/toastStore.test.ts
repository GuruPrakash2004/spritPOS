// Popup timing: 2.5 s on large screens (1024 px and up), 3.2 s on smaller ones, waiting popups included.
import { afterEach, describe, expect, it, vi } from 'vitest'
import { LARGE_SCREEN_QUERY, TOAST_DURATION_MS, toastDuration, toastStore } from './toastStore'

const screenIsLarge = (large: boolean) =>
  vi.spyOn(window, 'matchMedia').mockImplementation((query: string) =>
    ({ matches: large && query === LARGE_SCREEN_QUERY, media: query } as MediaQueryList))

afterEach(() => vi.restoreAllMocks())

describe('toastDuration', () => {
  it('is 2.5 s on a large screen', () => {
    screenIsLarge(true)
    expect(toastDuration()).toBe(2500)
  })

  it('is 3.2 s on a medium or small screen', () => {
    screenIsLarge(false)
    expect(toastDuration()).toBe(3200)
  })
})

// Waiting popups: the action waits until the popup closes; Cancel stops it.
describe('toastStore.defer', () => {
  const noMotion = () =>
    vi.spyOn(window, 'matchMedia').mockImplementation((query: string) =>
      ({ matches: query.includes('reduced-motion'), media: query } as MediaQueryList))
  const clear = () => {
    noMotion()   // closes at once, so the loop ends
    while (toastStore.getSnapshot().length) toastStore.getSnapshot().forEach(i => toastStore.cancel(i.id))
  }
  afterEach(clear)

  it('goes ahead when the popup closes', async () => {
    noMotion()
    const go = toastStore.defer('backup', 'Backup starts when this closes')
    const [item] = toastStore.getSnapshot()
    expect(item.pending).toBe(true)
    expect(item.duration).toBe(TOAST_DURATION_MS)   // noMotion() reports a small screen
    toastStore.remove(item.id)
    await expect(go).resolves.toBe(true)
  })

  it('does not go ahead when Cancel is pressed', async () => {
    noMotion()
    const go = toastStore.defer('backup', 'Backup starts when this closes')
    toastStore.cancel(toastStore.getSnapshot()[0].id)
    await expect(go).resolves.toBe(false)
  })

  it('says what was cancelled', async () => {
    noMotion()
    const go = toastStore.defer('delete', 'Item will be deleted when this closes', 'Delete cancelled.')
    toastStore.cancel(toastStore.getSnapshot()[0].id)
    await expect(go).resolves.toBe(false)
    expect(toastStore.getSnapshot().map(i => [i.text, i.pending])).toEqual([['Delete cancelled.', undefined]])
  })

  it('waits for the countdown before going ahead', async () => {
    noMotion()
    vi.useFakeTimers()
    let done: boolean | undefined
    toastStore.defer('backup', 'Backup starts when this closes').then(v => { done = v })
    await vi.advanceTimersByTimeAsync(TOAST_DURATION_MS - 500)
    expect(done).toBeUndefined()
    await vi.advanceTimersByTimeAsync(1000)
    expect(done).toBe(true)
    vi.useRealTimers()
  })

  it('runs once on a double-click', async () => {
    noMotion()
    const first = toastStore.defer('backup', 'Backup starts when this closes')
    await expect(toastStore.defer('backup', 'Backup starts when this closes')).resolves.toBe(false)
    expect(toastStore.getSnapshot()).toHaveLength(1)
    toastStore.remove(toastStore.getSnapshot()[0].id)
    await expect(first).resolves.toBe(true)
  })
})
