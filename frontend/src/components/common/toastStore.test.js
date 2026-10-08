// Popup timing: routine popups are quick (0.5 s on large screens, 0.8 s on smaller ones); errors and deletes keep 1.5 s / 2 s. Waiting popups included.
import { afterEach, describe, expect, it, vi } from 'vitest'
import { LARGE_SCREEN_QUERY, TOAST_DURATION_MS, TOAST_FAST_MS, toastDuration, toastStore } from './toastStore'

const screenIsLarge = (large) =>
  vi.spyOn(window, 'matchMedia').mockImplementation((query) =>
    ({ matches: large && query === LARGE_SCREEN_QUERY, media: query } ))

afterEach(() => vi.restoreAllMocks())

describe('toastDuration', () => {
  it('is 1.5 s on a large screen', () => {
    screenIsLarge(true)
    expect(toastDuration()).toBe(1500)
  })

  it('is 2 s on a medium or small screen', () => {
    screenIsLarge(false)
    expect(toastDuration()).toBe(2000)
  })

  it('is quick for routine popups and slow for errors and deletes', () => {
    screenIsLarge(true)
    expect(toastDuration('ok')).toBe(500)
    expect(toastDuration('refresh')).toBe(500)
    expect(toastDuration('error')).toBe(2000)   // every error popup: 2 s on any screen
    expect(toastDuration('delete')).toBe(1500)
    screenIsLarge(false)
    expect(toastDuration('backup')).toBe(800)
    expect(toastDuration('error')).toBe(2000)
  })
})

// Waiting popups: the action waits until the popup closes; Cancel stops it.
describe('toastStore.defer', () => {
  const noMotion = () =>
    vi.spyOn(window, 'matchMedia').mockImplementation((query) =>
      ({ matches: query.includes('reduced-motion'), media: query } ))
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
    expect(item.duration).toBe(TOAST_FAST_MS)   // a 'backup' popup is quick; noMotion() reports a small screen
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
    let done
    toastStore.defer('backup', 'Backup starts when this closes').then(v => { done = v })
    await vi.advanceTimersByTimeAsync(TOAST_FAST_MS - 300)
    expect(done).toBeUndefined()
    await vi.advanceTimersByTimeAsync(1000)
    expect(done).toBe(true)
    vi.useRealTimers()
  })

  it('keeps a delete popup at the slower time', () => {
    noMotion()
    toastStore.defer('delete', 'Item will be deleted when this closes')
    expect(toastStore.getSnapshot()[0].duration).toBe(TOAST_DURATION_MS)
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
