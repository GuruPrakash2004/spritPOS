import { describe, expect, it } from 'vitest'
import { seg } from './api'

describe('seg: a master code inside a URL path', () => {
  it('escapes / and ~ itself, then URL-encodes the rest', () => {
    expect(seg('ACCESSORIES 18% GST')).toBe('ACCESSORIES%2018%25%20GST')
    expect(seg('a/b')).toBe('a~2Fb')
    expect(seg('a~b')).toBe('a~7Eb')
    expect(seg('x?y#z')).toBe('x%3Fy%23z')
    expect(seg('~2F')).toBe('~7E2F')                 // a code that merely looks like the escape stays distinct
  })
})
