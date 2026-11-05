// The action bar: New / Edit / Delete / List / Close / Options in view mode, Save / Cancel while editing, record-moving buttons, rights.
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import ActionBar from './ActionBar'

describe('ActionBar', () => {
  it('shows the view buttons and runs their actions', async () => {
    const user = userEvent.setup()
    const a = { onNew: vi.fn(), onEdit: vi.fn(), onDelete: vi.fn(), onList: vi.fn(), onClose: vi.fn() }
    render(<ActionBar {...a} />)
    for (const [name, fn] of [['New', a.onNew], ['Edit', a.onEdit], ['Delete', a.onDelete], ['List', a.onList], ['Close', a.onClose]]) {
      await user.click(screen.getByRole('button', { name: new RegExp(`^${name}`) }))
      expect(fn).toHaveBeenCalledTimes(1)
    }
    expect(screen.queryByRole('button', { name: /options/i })).not.toBeInTheDocument()   // no extras: no Options menu
    expect(screen.queryByRole('button', { name: /^save/i })).not.toBeInTheDocument()
  })

  it('shows Save and Cancel instead while a record is being added or edited', async () => {
    const user = userEvent.setup()
    const onSave = vi.fn(), onCancel = vi.fn()
    render(<ActionBar mode="Edit" onNew={vi.fn()} onEdit={vi.fn()} onList={vi.fn()} onSave={onSave} onCancel={onCancel} nav={{ onFirst: vi.fn() }} />)
    expect(screen.queryByRole('button', { name: /^new/i })).not.toBeInTheDocument()
    expect(screen.queryByRole('group', { name: /move between records/i })).not.toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /^save/i }))
    await user.click(screen.getByRole('button', { name: /^cancel/i }))
    expect(onSave).toHaveBeenCalledTimes(1)
    expect(onCancel).toHaveBeenCalledTimes(1)
  })

  it('draws a forbidden Delete disabled, and leaves out an action the screen does not have', () => {
    render(<ActionBar onNew={vi.fn()} onDelete={vi.fn()} canDelete={false} />)
    expect(screen.getByRole('button', { name: /^delete/i })).toBeDisabled()
    expect(screen.queryByRole('button', { name: /^edit/i })).not.toBeInTheDocument()
  })

  it('opens the Options menu with the extra actions of the screen', async () => {
    const user = userEvent.setup()
    const run = vi.fn()
    render(<ActionBar onNew={vi.fn()} options={[{ label: 'Import Item from Excel', onClick: run }]} />)
    await user.click(screen.getByRole('button', { name: /options/i }))
    await user.click(await screen.findByRole('menuitem', { name: 'Import Item from Excel' }))
    expect(run).toHaveBeenCalledTimes(1)
  })

  it('moves between records and stops at the ends', async () => {
    const user = userEvent.setup()
    const nav = { onFirst: vi.fn(), onPrev: vi.fn(), onNext: vi.fn(), onLast: vi.fn(), onFind: vi.fn(), atStart: true, atEnd: false, label: '1 / 5' }
    render(<ActionBar onNew={vi.fn()} nav={nav} />)
    expect(screen.getByRole('button', { name: 'First record' })).toBeDisabled()
    await user.click(screen.getByRole('button', { name: 'Next record' }))
    await user.click(screen.getByRole('button', { name: 'Last record' }))
    await user.click(screen.getByRole('button', { name: 'Search' }))
    expect(nav.onNext).toHaveBeenCalled()
    expect(nav.onLast).toHaveBeenCalled()
    expect(nav.onFind).toHaveBeenCalled()
    expect(screen.getByText('1 / 5')).toBeInTheDocument()
  })

  it('answers the Alt keys of the exe', async () => {
    const user = userEvent.setup()
    const onNew = vi.fn(), onList = vi.fn()
    render(<ActionBar onNew={onNew} onList={onList} />)
    await user.keyboard('{Alt>}n{/Alt}')
    await user.keyboard('{Alt>}l{/Alt}')
    expect(onNew).toHaveBeenCalledTimes(1)
    expect(onList).toHaveBeenCalledTimes(1)
  })
})
