// Central site sign-in: user name and password (encrypted in the browser before it is sent, as on the shop
// app). Mistakes are shown inline; the server's answer (wrong password, locked) under the form.

import { useState, type FormEvent } from 'react';
import { LogIn } from 'lucide-react';
import { TextField } from '../components/common/FormField';
import { InlineError } from '../components/common/FormErrorSummary';
import VlqmsLogo from '../components/layout/VlqmsLogo';
import { api, errorText } from '../lib/api';
import { encryptSecret } from '../lib/secureField';
import { panel, primaryButton } from '../lib/styles';
import type { CentralSession } from './types';

export default function CentralLogin({ onSignedIn, reason }: { onSignedIn: (s: CentralSession) => void; reason?: string }) {
  const [user, setUser] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [serverError, setServerError] = useState(reason ?? '');
  const [busy, setBusy] = useState(false);

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    const found: Record<string, string> = {};
    if (!user.trim()) found.user = 'Enter your user name';
    if (!password) found.password = 'Enter your password';
    setErrors(found);
    if (Object.keys(found).length) {
      document.getElementById(found.user ? 'c-user' : 'c-password')?.focus();
      return;
    }
    setBusy(true);
    setServerError('');
    try {
      const enc = await encryptSecret(password, 'login');
      onSignedIn(await api<CentralSession>('/auth/login', { method: 'POST', body: { user: user.trim(), password: enc } }));
    } catch (err) {
      setServerError(errorText(err));
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-[#F5F1E9] p-4 dark:bg-[#0A0A0A]">
      <form onSubmit={submit} noValidate className={`${panel} w-full max-w-sm space-y-4 p-6`}>
        <div className="flex flex-col items-center gap-2 text-center">
          <VlqmsLogo size="md" />
          <h1 className="text-lg font-bold text-[#29231F] dark:text-stone-50">e-vLQMS ERP Central</h1>
          <p className="text-xs text-[#70675F] dark:text-stone-400">Every shop's day and stock in one place.</p>
        </div>
        <TextField id="c-user" label="User name" value={user} required hasError={!!errors.user} errorMessage={errors.user}
          onChange={e => { setUser(e.target.value); setErrors(x => ({ ...x, user: '' })); }} />
        <TextField id="c-password" label="Password" type="password" value={password} required hasError={!!errors.password}
          errorMessage={errors.password} onChange={e => { setPassword(e.target.value); setErrors(x => ({ ...x, password: '' })); }} />
        <InlineError message={serverError} />
        <button type="submit" className={`${primaryButton} w-full justify-center`} disabled={busy}><LogIn size={14} />{busy ? 'Signing in…' : 'Sign in'}</button>
      </form>
    </div>
  );
}
