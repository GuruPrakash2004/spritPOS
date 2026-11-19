import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import * as api from '../../lib/api'
import KeyGenerator from './KeyGenerator'

vi.mock('../../lib/api', async (orig) => ({ ...(await orig()), getKeyGeneratorAvailable: vi.fn(), getLicenceKey: vi.fn() }))

describe('KeyGenerator', () => {
  beforeEach(() => vi.clearAllMocks())

  it('shows the key for a Reg. No. on Enter and keeps only digits', async () => {
    api.getKeyGeneratorAvailable.mockResolvedValue({ available: true })
    api.getLicenceKey.mockResolvedValue({ reg_no: '59183', key: 'CWZMTD' })
    render(<KeyGenerator />)

    await userEvent.type(await screen.findByLabelText('Reg. No.'), '59a183{Enter}')

    await waitFor(() => expect(screen.getByLabelText('Key')).toHaveValue('CWZMTD'))
    expect(api.getLicenceKey).toHaveBeenCalledWith('59183')
  })

  it('says it is not set up when the server has no key file', async () => {
    api.getKeyGeneratorAvailable.mockResolvedValue({ available: false })
    render(<KeyGenerator />)

    expect(await screen.findByText(/not set up on this server/i)).toBeInTheDocument()
    expect(screen.queryByLabelText('Reg. No.')).toBeNull()
  })
})
