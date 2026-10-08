import { describe, expect, it, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import SmtpSetting from './SmtpSetting'
import * as api from '../../lib/api'
import { toastStore } from './toastStore'

vi.mock('../../lib/secureField', () => ({ encryptSecret: async (p, f) => (p ? `enc:${f}:${p}` : p) }))

const SAVED = { host: 'smtp.gmail.com', port: '587', security: 'starttls', user: 'shop@gmail.com', from_address: '', has_password: true, configured: true }

describe('SmtpSetting', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
    vi.spyOn(api, 'getSmtp').mockResolvedValue(SAVED)
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
  })

  it('shows the saved settings without the password', async () => {
    render(<SmtpSetting />)
    expect(await screen.findByDisplayValue('smtp.gmail.com')).toBeInTheDocument()
    expect(screen.getByLabelText(/^password/i)).toHaveValue('')
    expect(screen.getByPlaceholderText(/saved/i)).toBeInTheDocument()
  })

  it('tests the connection with the typed password encrypted', async () => {
    const test = vi.spyOn(api, 'testSmtp').mockResolvedValue({ ok: true, message: 'Connected to smtp.gmail.com:587 and signed in.' })
    const user = userEvent.setup()
    render(<SmtpSetting />)
    await screen.findByDisplayValue('smtp.gmail.com')
    await user.type(screen.getByLabelText(/^password/i), 'app pass')
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    expect(await screen.findByText(/Connected to smtp.gmail.com/)).toBeInTheDocument()
    expect(test).toHaveBeenCalledWith(expect.objectContaining({ host: 'smtp.gmail.com', password: 'enc:smtp:app pass' }))
  })

  it('shows why a connection failed', async () => {
    vi.spyOn(api, 'testSmtp').mockRejectedValue(new api.ApiError('Could not connect to the e-mail server: refused'))
    const user = userEvent.setup()
    render(<SmtpSetting />)
    await screen.findByDisplayValue('smtp.gmail.com')
    await user.click(screen.getByRole('button', { name: /test connection/i }))
    expect(await screen.findByRole('alert')).toHaveTextContent(/refused/)
  })

  it('checks the port and the from address before sending anything', async () => {
    const save = vi.spyOn(api, 'saveSmtp').mockResolvedValue(SAVED)
    const user = userEvent.setup()
    render(<SmtpSetting />)
    await screen.findByDisplayValue('smtp.gmail.com')
    await user.type(screen.getByLabelText(/from address/i), 'not-an-email')
    await user.click(screen.getByRole('button', { name: /save email settings/i }))
    expect(await screen.findByText(/not a valid e-mail address/i)).toBeInTheDocument()
    expect(save).not.toHaveBeenCalled()
  })

  it('saves the settings, sending the password encrypted', async () => {
    const save = vi.spyOn(api, 'saveSmtp').mockResolvedValue(SAVED)
    const user = userEvent.setup()
    render(<SmtpSetting />)
    await screen.findByDisplayValue('smtp.gmail.com')
    await user.type(screen.getByLabelText(/^password/i), 'new pass')
    await user.click(screen.getByRole('button', { name: /save email settings/i }))
    await waitFor(() => expect(save).toHaveBeenCalledWith(expect.objectContaining({ user: 'shop@gmail.com', password: 'enc:smtp:new pass' })))
  })
})
