// Credentials typed in the UI are encrypted before they leave the browser whenever WebCrypto exists (https:// or
// localhost); over plain http:// from another PC they are sent as typed and the backend accepts them.
// encryptSecret(password, purpose) encrypts a password with the server's RSA public key (RSA-OAEP-256,
// WebCrypto) as "enc:v1:<base64>", with the field it is for and a timestamp on the SERVER's clock (so a PC
// with a wrong clock still works). The backend (app/secure_fields.py) refuses plain, old, re-used and
// other-field values. Use it for every password field sent to the API. It complements HTTPS.
import { ApiError, getTransportKey } from './api';

/** Which field a password is for; the server accepts it only there. */
 

let cached = null;

function fromBase64(b64) {
  const bin = atob(b64);
  const out = new Uint8Array(bin.length);
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i);
  return out.buffer;
}

function toBase64(buf) {
  const bytes = new Uint8Array(buf);
  let bin = '';
  for (let i = 0; i < bytes.length; i++) bin += String.fromCharCode(bytes[i]);
  return btoa(bin);
}

async function serverKey() {
  if (!globalThis.crypto?.subtle) {
    throw new ApiError('This page can only send passwords securely over HTTPS (or on localhost). Open the app with an https:// address.');
  }
  cached ??= getTransportKey()
    .then(async k => ({
      key: await crypto.subtle.importKey('spki', fromBase64(k.spki), { name: 'RSA-OAEP', hash: 'SHA-256' }, false, ['encrypt']),
      offset: typeof k.server_time === 'number' ? k.server_time - Date.now() : 0,   // server clock minus this PC's clock
    }))
    .catch(e => {
      cached = null;   // try again next time (server restarting, network blip)
      throw e instanceof ApiError ? e : new ApiError('Could not get the server key for sending the password securely.');
    });
  return cached;
}

/** The password as the API expects it: "enc:v1:..." (empty stays empty, e.g. "keep the current password"). */
export async function encryptSecret(plain, purpose) {
  if (!plain) return plain;
  if (!globalThis.crypto?.subtle) return plain;   // plain http:// from another PC: no WebCrypto, send as typed
  const { key, offset } = await serverKey();
  const payload = new TextEncoder().encode(JSON.stringify({ v: plain, t: Date.now() + offset, f: purpose }));
  return `enc:v1:${toBase64(await crypto.subtle.encrypt({ name: 'RSA-OAEP' }, key, payload))}`;
}

/** Forget the cached key (tests; or after the server said it couldn't decrypt). */
export function resetTransportKey() {
  cached = null;
}
