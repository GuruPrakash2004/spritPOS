import { describe, expect, it } from 'vitest'
import { passwordProblem } from './passwords'

describe('passwordProblem (same rules as the backend)', () => {
  it.each([
    ['short1', 'at least 8'],
    ['onlyletters', 'mix letters'],
    ['12345678', 'mix letters'],
    ['cashier01', 'same as the user name'],
  ])('refuses %j', (password, problem) => {
    expect(passwordProblem(password, 'CASHIER01')).toContain(problem)
  })

  it('accepts a password with letters and numbers', () => {
    expect(passwordProblem('Counter#12', 'RAVI')).toBeNull()
  })
})
