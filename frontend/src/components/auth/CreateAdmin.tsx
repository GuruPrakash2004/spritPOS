// The last step of Database setup on the sign-in page (backend POST /setup/admin): shown instead of the login form
// when the shop's database is in use but none of its users is an Administrator (the desktop's admin roles are
// mapped to Administrator when the users load). A user name (a desktop user keeps their details, any other name
// becomes a new user) and a new password, sent only encrypted (lib/secureField.ts, purpose "new").

import { useState, type FormEvent } from 'react';
import { AlertCircle, Loader2, ShieldCheck } from 'lucide-react';
import { createAdmin, errorText } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';

type Key = 'user' | 'password' | 'confirm';
const ORDER: Key[] = ['user', 'password', 'confirm'];

export default function CreateAdmin({ inputClass, labelClass, onDone }: { inputClass: string; labelClass: string; onDone: () => void }) {
  const [f, setF] = useState<Record<Key, string>>({ user: '', password: '', confirm: '' });
  const [errors, setErrors] = useState<Partial<Record<Key, string>>>({});
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  const set = (k: Key, v: string) => { setF(x => ({ ...x, [k]: v })); setErrors(e => ({ ...e, [k]: undefined })); setError(''); };

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    const errs: Partial<Record<Key, string>> = {};
    if (!f.user.trim()) errs.user = 'Enter the administrator\'s user name';
    if (f.password.length < 8) errs.password = 'Use at least 8 characters';
    else if (/^[a-z]+$/i.test(f.password) || /^\d+$/.test(f.password)) errs.password = 'Mix letters with numbers or symbols';
    if (!errs.password && f.confirm !== f.password) errs.confirm = 'The passwords don\'t match';
    setErrors(errs);
    const first = ORDER.find(k => errs[k]);
    if (first) { document.getElementById(`adm-${first}`)?.focus(); return; }
    const user = f.user.trim().toUpperCase();
    if (!(await toastStore.defer('ok', `${user} will be made the Administrator when this closes.`, 'Cancelled. No administrator was made.'))) return;
    setBusy(true);
    try {
      const body = { user, password: await encryptSecret(f.password, 'new') };
      await createAdmin(body);
      toastStore.add('ok', `${user} is now the Administrator. Sign in with the new password.`);
      onDone();
    } catch (err) {
      setError(errorText(err));
    } finally {
      setBusy(false);
    }
  };

  const input = (k: Key, label: string, props: Record<string, string> = {}) => (
    <div>
      <label htmlFor={`adm-${k}`} className={labelClass}>{label}</label>
      <input id={`adm-${k}`} value={f[k]} onChange={e => set(k, k === 'user' ? e.target.value.toUpperCase() : e.target.value)}
        aria-invalid={!!errors[k] || undefined} className={`${inputClass} ${errors[k] ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} {...props} />
      <InlineError message={errors[k]} />
    </div>
  );

  return (
    <form onSubmit={submit} noValidate className="space-y-4 rounded-2xl border border-[#E3D9CD] p-6 dark:border-[#2A2A2A]">
      <div>
        <h2 className="flex items-center gap-2 text-sm font-bold"><ShieldCheck size={16} />Create the administrator</h2>
        <p className="mt-1 text-xs text-[#70675F] dark:text-stone-400">
          None of this shop's users is an Administrator yet. Choose one and give it a password: a desktop user name makes that user the Administrator (and active) with this new password; any other name becomes a new user.
        </p>
      </div>
      {input('user', 'User name', { autoComplete: 'off' })}
      {input('password', 'New password', { type: 'password', autoComplete: 'new-password' })}
      {input('confirm', 'Confirm password', { type: 'password', autoComplete: 'new-password' })}
      {error && (
        <p role="alert" className="flex items-center gap-2 rounded-xl bg-[#9A4B45]/10 px-4 py-3 text-xs font-medium text-[#9A4B45] dark:bg-red-500/10 dark:text-red-400">
          <AlertCircle size={14} className="shrink-0" />{error}
        </p>
      )}
      <button type="submit" disabled={busy} className="flex w-full items-center justify-center gap-2 rounded-xl bg-[#64283A] py-3 text-sm font-bold text-white hover:bg-[#521E2E] disabled:opacity-45 dark:bg-[#D4A847] dark:text-[#181106]">
        {busy ? <Loader2 size={16} className="animate-spin" /> : <ShieldCheck size={16} />}{busy ? 'Saving…' : 'Make administrator'}
      </button>
    </form>
  );
}
