// Admin Master rights grid <-> backend role: the Add/Edit/Delete ticks survive a save and reload.
import { describe, expect, it } from 'vitest'
import { draftRights, parseRoleToDraft } from './matrixDefs'

describe('rights ticks', () => {
  it('reads saved ticks: a switched-off action is unticked, others stay ticked', () => {
    const draft = parseRoleToDraft({
      name: 'Cashier', modules: ['Item Master', 'Sales Invoice'],
      rights: { 'Item Master': { add: true, edit: true, delete: false } },
    })

    expect(draft.actions.has('Item Master:add')).toBe(true)
    expect(draft.actions.has('Item Master:delete')).toBe(false)
    expect(draft.actions.has('Sales Invoice:delete')).toBe(true)
  })

  it('round-trips through save and reload', () => {
    const saved = { name: 'Clerk', modules: ['Customer Master', 'Dashboard'],
      rights: { 'Customer Master': { add: true, edit: false, delete: false } } }

    const rights = draftRights(parseRoleToDraft(saved))

    expect(rights['Customer Master']).toEqual({ add: true, edit: false, delete: false })
    expect(rights.Dashboard).toEqual({ add: true, edit: true, delete: true })   // no buttons = nothing to limit
    expect(rights['Item Master']).toBeUndefined()                              // not opened
  })
})
