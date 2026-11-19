import { describe, expect, it } from 'vitest'
import { optionSortKind, sortOptions } from './optionOrder'

describe('option order', () => {
  it('picks the order by field', () => {
    expect(optionSortKind({ key: 'BRANDID', label: 'Brand' })).toBe('name')
    expect(optionSortKind({ key: 'brand', label: 'Brand' })).toBe('name')
    expect(optionSortKind({ key: 'PKGITEM', label: 'Pkg Item' })).toBe('numeric')
    expect(optionSortKind({ key: 'PKGUOM', label: 'Pkg UOM' })).toBe('numeric')
    expect(optionSortKind({ key: 'package', label: 'Package' })).toBe('numeric')
    expect(optionSortKind({ key: 'uom', label: 'UOM' })).toBeNull()
    expect(optionSortKind({ key: 'group', label: 'Item Group' })).toBeNull()
  })

  it('sorts packages by their number, not their letters', () => {
    const list = ['750ML', '1000ML', '2', '180ML', '275ML', '-']
    expect(sortOptions(list, 'numeric')).toEqual(['-', '2', '180ML', '275ML', '750ML', '1000ML'])
  })

  it('sorts brands A to Z whatever the case', () => {
    expect(sortOptions(['USL', 'bagpiper', 'Royal Challenge', 'AB InBev'], 'name')).toEqual(['AB InBev', 'bagpiper', 'Royal Challenge', 'USL'])
  })

  it('sorts objects by their label and leaves other lists alone', () => {
    const rows = [{ value: 1, label: '1000ML' }, { value: 2, label: '180ML' }]
    expect(sortOptions(rows, 'numeric', r => r.label).map(r => r.value)).toEqual([2, 1])
    const same = ['b', 'a']
    expect(sortOptions(same, null)).toBe(same)
  })
})
