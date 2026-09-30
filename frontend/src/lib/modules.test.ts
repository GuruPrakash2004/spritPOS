// Tests for the module gate: SQL Query Execution, Admin Master and the admin Utilities are Administrator-only
// whatever the role's module list says; Re Write Stock follows the role's rights; About is open to everyone.
import { describe, expect, it } from 'vitest'
import { canOpen } from './modules'

describe('canOpen', () => {
  it('keeps SQL Query Execution for Administrators only', () => {
    expect(canOpen(['*'], 'Administrator', 'SQL Query Execution')).toBe(true)
    expect(canOpen(['*', 'SQL Query Execution'], 'Manager', 'SQL Query Execution')).toBe(false)
  })

  it('lets a role open Re Write Stock once it is granted', () => {
    expect(canOpen(['Dashboard'], 'Manager', 'Re Write Stock')).toBe(false)
    expect(canOpen(['Re Write Stock'], 'Manager', 'Re Write Stock')).toBe(true)
  })

  it('keeps Global Settings and the ADMIN Audit Log for Administrators only', () => {
    for (const name of ['Global Settings', 'ADMIN Audit Log']) {
      expect(canOpen(['*'], 'Administrator', name)).toBe(true)
      expect(canOpen([name], 'Manager', name)).toBe(false)
    }
  })

  it('lets every user open About', () => {
    expect(canOpen([], 'Cashier', 'About')).toBe(true)
  })
})
