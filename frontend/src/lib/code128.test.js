import { describe, expect, it } from 'vitest'
import { code128Patterns, code128Svg, code128Values } from './code128'

describe('code128', () => {
  it('has 107 patterns, each 11 modules wide (the stop pattern 13)', () => {
    expect(code128Patterns).toHaveLength(107)
    code128Patterns.forEach((p, i) => {
      expect([...p].reduce((s, d) => s + Number(d), 0)).toBe(i === 106 ? 13 : 11)
    })
  })

  it('starts with Start B, ends with Stop, and holds one value per character plus a check digit', () => {
    const v = code128Values('PJJ123C')
    expect(v[0]).toBe(104)
    expect(v.at(-1)).toBe(106)
    expect(v).toHaveLength(7 + 3)
  })

  it('refuses characters Code 128 B cannot hold and draws bars otherwise', () => {
    expect(code128Values('é')).toBeNull()
    expect(code128Svg('é')).toBe('')
    expect(code128Svg('ITEM-1')).toContain('<rect')
  })
})
