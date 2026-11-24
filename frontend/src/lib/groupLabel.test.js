// groupLabel(): the desktop's numbered item-group names ("1 BRANDY") shown without the number.
import { describe, expect, it } from 'vitest'
import { groupLabel } from './formatters'

describe('groupLabel', () => {
  it('drops the leading number the desktop puts on a group', () => {
    expect(groupLabel('1 BRANDY')).toBe('BRANDY')
    expect(groupLabel('10 BEER')).toBe('BEER')
    expect(groupLabel('4 LAB(BREEZER)')).toBe('LAB(BREEZER)')
    expect(groupLabel(' 5 VODKA ')).toBe('VODKA')
  })

  it('leaves other names alone', () => {
    expect(groupLabel('ITC CIGARETTES & CIGAR')).toBe('ITC CIGARETTES & CIGAR')
    expect(groupLabel('BEVERAGES 5% GST')).toBe('BEVERAGES 5% GST')
    expect(groupLabel('')).toBe('')
    expect(groupLabel(undefined)).toBe('')
  })

  it('never returns an empty name for a group that is only a number', () => {
    expect(groupLabel('7')).toBe('7')
  })
})
