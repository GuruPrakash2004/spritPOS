// Central site: change your password. A new user's first password is temporary, so this is all they can do
// until it is changed. Mistakes are inline; the change waits for its popup, then signs in with the new token.

import { useState, } from 'react';
import { KeyRound } from 'lucide-react';
import { TextField } from '../components/common/FormField';
import { FormErrorSummary } from '../components/common/FormErrorSummary';
import { toastStore } from '../components/common/toastStore';
import { centralChangePassword, errorText } from '../lib/api';
import { encryptSecret } from '../lib/secureField';
import { panel, primaryButton, secondaryButton } from '../lib/styles';


const FIELDS = { current: 'cp-current', next: 'cp-new', again: 'cp-again' } ;

export default function ChangePassword({ user, temporary, onChanged, onCancel }

) {
  const [values, setValues] = useState({ current: '', next: '', again: '' });
  const [errors, setErrors] = useState({});
  const [busy, setBusy] = useState(false);
  const set = (k) => (e) => {
    setValues(v => ({ ...v, [k]: e.target.value }));
    setErrors(x => { const n = { ...x }; delete n[k]; return n; });
  };

  const submit = async (e) => {
    e.preventDefault();
    const found = {};
    if (!values.current) found.current = 'Enter your current password';
    if (values.next.length < 8) found.next = 'Use at least 8 characters';
    else if (/^[A-Za-z]+$/.test(values.next) || /^\d+$/.test(values.next)) found.next = 'Mix letters with numbers or symbols';
    if (values.again !== values.next) found.again = 'The two new passwords are not the same';
    setErrors(found);
    const first = Object.keys(found)[0] ;
    if (first) {
      document.getElementById(FIELDS[first])?.focus();
      return;
    }
    if (!(await toastStore.defer('ok', 'Your password will be changed when this closes.', 'Password not changed.'))) return;
    setBusy(true);
    try {
      const body = { current_password: await encryptSecret(values.current, 'current'), new_password: await encryptSecret(values.next, 'new') };
      const session = await centralChangePassword(body);
      toastStore.add('ok', 'Password changed.');
      onChanged(session);
    } catch (err) {
      toastStore.add('error', errorText(err));
    } finally {
      setBusy(false);
    }
  };

  return (
    <form onSubmit={submit} noValidate className={`${panel} mx-auto w-full max-w-md space-y-4 p-6`}>
      <div>
        <h1 className="text-lg font-bold text-[#1A2236] dark:text-stone-50">Change password</h1>
        <p className="text-xs text-[#5F6B80] dark:text-stone-400">
          {temporary ? `Welcome, ${user}. Your first password is temporary: choose your own to continue.` : `Signed in as ${user}.`}
        </p>
      </div>
      <FormErrorSummary errors={errors} onFocusField={k => document.getElementById(FIELDS[k ])?.focus()} />
      <TextField id={FIELDS.current} label="Current password" type="password" value={values.current} onChange={set('current')}
        hasError={!!errors.current} errorMessage={errors.current} required />
      <TextField id={FIELDS.next} label="New password" type="password" value={values.next} onChange={set('next')}
        hasError={!!errors.next} errorMessage={errors.next} required />
      <TextField id={FIELDS.again} label="New password again" type="password" value={values.again} onChange={set('again')}
        hasError={!!errors.again} errorMessage={errors.again} required />
      <div className="flex justify-end gap-2">
        <button type="button" className={secondaryButton} onClick={onCancel}>{temporary ? 'Sign out' : 'Cancel'}</button>
        <button type="submit" className={primaryButton} disabled={busy}><KeyRound size={14} />{busy ? 'Saving…' : 'Change password'}</button>
      </div>
    </form>
  );
}
