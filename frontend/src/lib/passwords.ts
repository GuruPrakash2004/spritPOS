// Password rules for new or changed passwords, shown inline in Admin Master (Security) and Change password.
// Same rules as backend/app/security.py password_problem(), from Admin Master (Security) → Login Settings
// (fetched once from the public GET /auth/login-settings by loadPasswordRules()); the server checks again.
import type { ApiLoginSettings } from './api';

const DEFAULT: ApiLoginSettings = {
  min_length: 8, require_mix: true, require_upper: false, require_lower: false, require_digit: false,
  require_special: false, password_expiry_days: 0, show_remember_me: false,
};
let rules: ApiLoginSettings = DEFAULT;
let loading: Promise<ApiLoginSettings> | null = null;

/** Load (once) and return the Login Settings; falls back to the default rules when the server can't be reached. */
export const loadPasswordRules = (): Promise<ApiLoginSettings> => {
  loading ??= fetch('/api/auth/login-settings')
    .then(res => (res.ok ? res.json() as Promise<ApiLoginSettings> : DEFAULT))
    .then(r => { rules = { ...DEFAULT, ...r }; return rules; })
    .catch(() => { loading = null; return rules; });
  return loading;
};
/** Use these rules now (Admin Master (Security) after saving Login Settings). */
export const setPasswordRules = (r: ApiLoginSettings) => { rules = { ...DEFAULT, ...r }; loading = Promise.resolve(rules); };

export const passwordProblem = (password: string, user: string): string | null => {
  const n = Math.max(8, rules.min_length);
  if (password.length < n) return `Password must be at least ${n} characters`;
  if (password.trim().toLowerCase() === user.trim().toLowerCase()) return 'Password must not be the same as the user name';
  if (rules.require_mix && (/^[a-z]+$/i.test(password) || /^\d+$/.test(password))) return 'Password must mix letters with numbers or symbols';
  if (rules.require_upper && !/[A-Z]/.test(password)) return 'Password must have a capital letter';
  if (rules.require_lower && !/[a-z]/.test(password)) return 'Password must have a small letter';
  if (rules.require_digit && !/\d/.test(password)) return 'Password must have a number';
  if (rules.require_special && !/[^A-Za-z0-9]/.test(password)) return 'Password must have a symbol such as # or @';
  return null;
};
