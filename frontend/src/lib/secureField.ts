// Credentials typed in the UI are always encrypted before they leave the browser (owner's rule).
// encryptSecret(password, purpose) encrypts a password with the server's RSA public key (RSA-OAEP-256,
// WebCrypto) as "enc:v1:<base64>", with the field it is for and a timestamp on the SERVER's clock (so a PC
// with a wrong clock still works). The backend (app/secure_fields.py) refuses plain, old, re-used and
// other-field values. Use it for every password field sent to the API. It complements HTTPS.
import { ApiError } from './api';

type TransportKey = { alg: string; spki: string; kid: string; server_time: number };
/** Which field a password is for; the server accepts it only there. */
export type SecretPurpose = 'login' | 'user' | 'current' | 'new' | 'admin' | 'db';

let cached: Promise<{ key: CryptoKey; offset: number }> | null = null;

function fromBase64(b64: string): ArrayBuffer {
  const bin = atob(b64);
  const out = new Uint8Array(bin.length);
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i);
  return out.buffer;
}

function toBase64(buf: ArrayBuffer): string {
  const bytes = new Uint8Array(buf);
  let bin = '';
  for (let i = 0; i < bytes.length; i++) bin += String.fromCharCode(bytes[i]);
  return btoa(bin);
}

async function serverKey(): Promise<{ key: CryptoKey; offset: number }> {
  if (!globalThis.crypto?.subtle) {
    throw new ApiError('This page can only send passwords securely over HTTPS (or on localhost). Open the app with an https:// address.');
  }
  cached ??= fetch('/api/auth/transport-key')
    .then(res => {
      if (!res.ok) throw new ApiError('Could not get the server key for sending the password securely.');
      return res.json() as Promise<TransportKey>;
    })
    .then(async k => ({
      key: await crypto.subtle.importKey('spki', fromBase64(k.spki), { name: 'RSA-OAEP', hash: 'SHA-256' }, false, ['encrypt']),
      offset: typeof k.server_time === 'number' ? k.server_time - Date.now() : 0,   // server clock minus this PC's clock
    }))
    .catch(e => {
      cached = null;   // try again next time (server restarting, network blip)
      throw e instanceof ApiError ? e : new ApiError('Backend is not reachable. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload --port 8001');
    });
  return cached;
}

/** The password as the API expects it: "enc:v1:..." (empty stays empty, e.g. "keep the current password"). */
export async function encryptSecret(plain: string, purpose: SecretPurpose): Promise<string> {
  if (!plain) return plain;
  const { key, offset } = await serverKey();
  const payload = new TextEncoder().encode(JSON.stringify({ v: plain, t: Date.now() + offset, f: purpose }));
  return `enc:v1:${toBase64(await crypto.subtle.encrypt({ name: 'RSA-OAEP' }, key, payload))}`;
}

/** Forget the cached key (tests; or after the server said it couldn't decrypt). */
export function resetTransportKey(): void {
  cached = null;
}
