// Tests for encryptSecret: the password is RSA-OAEP encrypted with the server's key (it can be decrypted
// back to the password, its field and a timestamp on the server's clock), an empty value stays empty, and the key is fetched once.
import { afterEach, describe, expect, it, vi } from 'vitest'
import { encryptSecret, resetTransportKey } from './secureField'

const b64 = (buf: ArrayBuffer) => btoa(String.fromCharCode(...new Uint8Array(buf)))

afterEach(() => { vi.unstubAllGlobals(); resetTransportKey() })

describe('encryptSecret', () => {
  it('encrypts with the server key so only the server can read it', async () => {
    const pair = await crypto.subtle.generateKey(
      { name: 'RSA-OAEP', modulusLength: 2048, publicExponent: new Uint8Array([1, 0, 1]), hash: 'SHA-256' }, true, ['encrypt', 'decrypt'])
    const spki = b64(await crypto.subtle.exportKey('spki', pair.publicKey))
    const fetchMock = vi.fn(async () => new Response(JSON.stringify({ alg: 'RSA-OAEP-256', spki, kid: 'k', server_time: Date.now() + 3_600_000 })))
    vi.stubGlobal('fetch', fetchMock)

    const sealed = await encryptSecret('Counter#12', 'user')
    await encryptSecret('again', 'login')

    expect(sealed.startsWith('enc:v1:')).toBe(true)
    expect(sealed).not.toContain('Counter')
    const raw = Uint8Array.from(atob(sealed.slice(7)), c => c.charCodeAt(0))
    const plain = JSON.parse(new TextDecoder().decode(await crypto.subtle.decrypt({ name: 'RSA-OAEP' }, pair.privateKey, raw)))
    expect(plain.v).toBe('Counter#12')
    expect(plain.f).toBe('user')
    // Stamped on the server's clock (here an hour ahead of this PC), not the PC's.
    expect(Math.abs(plain.t - (Date.now() + 3_600_000))).toBeLessThan(5000)
    expect(fetchMock).toHaveBeenCalledTimes(1)
    expect(fetchMock.mock.calls[0]).toEqual(['/api/auth/transport-key'])
  })

  it('leaves an empty password empty (keep the current one)', async () => {
    const fetchMock = vi.fn()
    vi.stubGlobal('fetch', fetchMock)
    expect(await encryptSecret('', 'user')).toBe('')
    expect(fetchMock).not.toHaveBeenCalled()
  })
})
