import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import * as api from '../../lib/api'
import { toastStore } from '../common/toastStore'
import DeleteLinkage from './DeleteLinkage'

vi.mock('../../lib/api', async (orig) => ({
  ...(await orig()),
  getLinkageTables: vi.fn(), getLinkageColumns: vi.fn(), getLinkageGroups: vi.fn(), getLinkageGroup: vi.fn(),
  searchLinkage: vi.fn(), saveLinkageGroup: vi.fn(), deleteLinkageGroup: vi.fn(),
}))

const GROUP = { table: 'MstItem', doc_name: 'Item Master', doc_field: 'ItemID', rows: [{ table: 'TrnDtlSINV', column: 'ItemID', display: 'ItemCode', caption: 'Sales Invoice' }] }

beforeEach(() => {
  vi.clearAllMocks()
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  api.getLinkageTables.mockResolvedValue(['MstItem', 'TrnDtlPB', 'TrnDtlSINV'])
  api.getLinkageColumns.mockResolvedValue(['ItemID', 'ItemCode'])
  api.getLinkageGroups.mockResolvedValue([{ table: 'MstItem', name: 'Item Master' }])
  api.getLinkageGroup.mockResolvedValue(GROUP)
})

describe('DeleteLinkage', () => {
  it('lists the guarded tables and shows one when clicked, read-only until Edit', async () => {
    render(<DeleteLinkage />)

    await userEvent.click(await screen.findByRole('button', { name: 'Item Master' }))

    expect(await screen.findByDisplayValue('Item Master')).toBeDisabled()
    expect(screen.getByLabelText('Table 1')).toHaveValue('TrnDtlSINV')
    await userEvent.click(screen.getByRole('button', { name: 'Edit' }))
    expect(screen.getByDisplayValue('Item Master')).toBeEnabled()
  })

  it('Save follows the exe checks in order, then sends the rows', async () => {
    api.saveLinkageGroup.mockResolvedValue({ message: 'Saved Successfully!' })
    render(<DeleteLinkage />)
    await userEvent.click(await screen.findByRole('button', { name: 'Item Master' }))
    await userEvent.click(await screen.findByRole('button', { name: 'Edit' }))
    await userEvent.clear(screen.getByDisplayValue('Item Master'))

    await userEvent.click(screen.getByRole('button', { name: /^Save/ }))
    expect(await screen.findByText('Document name is Empty...')).toBeInTheDocument()
    expect(api.saveLinkageGroup).not.toHaveBeenCalled()

    await userEvent.type(screen.getByLabelText('Document Name'), 'Item Master')
    await userEvent.click(screen.getByRole('button', { name: /^Save/ }))
    await waitFor(() => expect(api.saveLinkageGroup).toHaveBeenCalledWith('MstItem', { doc_name: 'Item Master', doc_field: 'ItemID', rows: GROUP.rows }))
  })

  it('the magnifier needs a table, then lists the tables found', async () => {
    api.searchLinkage.mockResolvedValue([{ table: 'TrnDtlPB', column: 'ItemID', display: 'ItemCode', caption: 'Purchase Bill' }])
    render(<DeleteLinkage />)
    await userEvent.click(await screen.findByRole('button', { name: /^New/ }))

    await userEvent.click(screen.getByRole('button', { name: /find tables/i }))
    expect(await screen.findByText('Select Table name')).toBeInTheDocument()

    await userEvent.selectOptions(screen.getByLabelText('Table Name'), 'MstItem')
    await userEvent.selectOptions(await screen.findByLabelText('Field To be Checked for Deletion'), 'ItemID')
    await userEvent.click(screen.getByRole('button', { name: /find tables/i }))
    expect(await screen.findByText('Purchase Bill')).toBeInTheDocument()
    expect(api.searchLinkage).toHaveBeenCalledWith('MstItem', 'ItemID')
  })

  it('Delete asks first, then removes the whole group', async () => {
    api.deleteLinkageGroup.mockResolvedValue({ message: 'Deleted Successfully!' })
    render(<DeleteLinkage />)
    await userEvent.click(await screen.findByRole('button', { name: 'Item Master' }))

    await userEvent.click(await screen.findByRole('button', { name: 'Delete' }))
    expect(await screen.findByText('Are you sure you want to delete ?')).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Confirm' }))

    await waitFor(() => expect(api.deleteLinkageGroup).toHaveBeenCalledWith('MstItem'))
  })

  it('a save with no usable row still goes to the server (the exe deletes the group and writes nothing)', async () => {
    api.saveLinkageGroup.mockResolvedValue({ saved: 0 })
    render(<DeleteLinkage />)
    await userEvent.click(await screen.findByRole('button', { name: 'Item Master' }))
    await userEvent.click(await screen.findByRole('button', { name: 'Edit' }))
    await userEvent.selectOptions(screen.getByLabelText('Table 1'), '')

    await userEvent.click(screen.getByRole('button', { name: /^Save/ }))

    await waitFor(() => expect(api.saveLinkageGroup).toHaveBeenCalledWith('MstItem', { doc_name: 'Item Master', doc_field: 'ItemID', rows: [] }))
  })

  it('Insert, Copy, Paste and Remove work on the grid rows; Remove asks first', async () => {
    render(<DeleteLinkage />)
    await userEvent.click(await screen.findByRole('button', { name: 'Item Master' }))
    await userEvent.click(await screen.findByRole('button', { name: 'Edit' }))
    expect(screen.getAllByLabelText(/^Table \d/)).toHaveLength(2)   // the rule and the blank row

    await userEvent.click(screen.getByLabelText('Table 1'))
    await userEvent.click(screen.getByRole('button', { name: /^Copy/ }))
    await userEvent.click(screen.getByRole('button', { name: /^Paste/ }))
    expect(screen.getAllByLabelText(/^Table \d/)).toHaveLength(3)
    expect(screen.getByLabelText('Table 2')).toHaveValue('TrnDtlSINV')

    await userEvent.click(screen.getByRole('button', { name: /^Insert/ }))
    expect(screen.getAllByLabelText(/^Table \d/)).toHaveLength(4)

    await userEvent.click(screen.getByRole('button', { name: /^Remove/ }))
    expect(await screen.findByText('Are you sure you want to Remove ?')).toBeInTheDocument()
    expect(screen.getByText('Deletion Confirmation...')).toBeInTheDocument()
    await userEvent.click(screen.getByRole('button', { name: 'Confirm' }))
    await waitFor(() => expect(screen.getAllByLabelText(/^Table \d/)).toHaveLength(3))
  })

  it('Close is there when the screen is given one', async () => {
    const onClose = vi.fn()
    render(<DeleteLinkage onClose={onClose} />)

    await userEvent.click(await screen.findByRole('button', { name: 'Close' }))

    expect(onClose).toHaveBeenCalled()
  })

  it('shows the error while the database is unreachable, and loads the lists once Try now finds it', async () => {
    api.getLinkageTables.mockRejectedValueOnce(new Error('The database is not connected right now.'))
    render(<DeleteLinkage />)

    expect(await screen.findByText(/not connected right now/)).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Item Master' })).not.toBeInTheDocument()

    await userEvent.click(screen.getByRole('button', { name: 'Try now' }))

    expect(await screen.findByRole('button', { name: 'Item Master' })).toBeInTheDocument()
    expect(screen.queryByText(/not connected right now/)).not.toBeInTheDocument()
  })
})
