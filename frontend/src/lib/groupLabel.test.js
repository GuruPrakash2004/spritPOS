// groupLabel(): item-group names are shown exactly as the database holds them (numbers included).
import { describe, expect, it } from 'vitest'
import { groupLabel } from './formatters'

describe('groupLabel', () => {
  it('keeps the desktop group names as they are', () => {
    expect(groupLabel('1 BRANDY')).toBe('1 BRANDY')
    expect(groupLabel('10 BEER')).toBe('10 BEER')
    expect(groupLabel(' 4 LAB(BREEZER) ')).toBe('4 LAB(BREEZER)')
    expect(groupLabel('BEVERAGES 5% GST')).toBe('BEVERAGES 5% GST')
    expect(groupLabel(undefined)).toBe('')
  })
})
