import { describe, expect, it } from 'vitest'
import { arrangePackageFields } from './packageLayout'

const keys = (list) => list.map(f => f.key)

describe('arrangePackageFields', () => {
  it('puts Code and Name first, then Litres per Bottle with the other details, then the rest', () => {
    const fields = [
      { key: 'litres_per_bottle', label: 'Litres per Bottle' },
      { key: 'Remarks', label: 'Remarks', extra: true, section: 'Additional details' },
      { key: 'PKGCODE', column: 'PKGCODE', label: 'Package Name', extra: true, section: 'Other details' },
      { key: 'code', label: 'Package Code' },
      { key: 'Excise', column: 'Excise', label: 'Excise', extra: true, section: 'Other details' },
      { key: 'active', label: 'Active', type: 'bool' },
    ]
    const out = arrangePackageFields(fields)
    expect(keys(out)).toEqual(['code', 'PKGCODE', 'litres_per_bottle', 'Excise', 'Remarks', 'active'])
    expect(out.find(f => f.key === 'litres_per_bottle').section).toBe('Other details')
  })

  it('keeps every field and works with just the offline fallback list', () => {
    const fields = [{ key: 'code' }, { key: 'litres_per_bottle' }, { key: 'active' }]
    expect(keys(arrangePackageFields(fields))).toEqual(['code', 'litres_per_bottle', 'active'])
    expect(arrangePackageFields(fields)).toHaveLength(3)
  })
})
