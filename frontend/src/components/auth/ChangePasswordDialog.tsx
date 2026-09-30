// "Change password" dialog for the logged-in user (header key button in App.tsx).
// Posts to /auth/change-password with the current password; the backend applies the same rules as
// lib/passwords.ts and returns a new token, which replaces the old one (other sessions are signed out).
// Mistakes show inline under the field; the server's answer shows as a popup.

import { useEffect, useState, type FormEvent } from 'react';
import Modal from '../common/Modal';
import { TextField } from '../common/FormField';
import { toast } from '../common/toastStore';
import { api, errorText, setAuthToken } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';
import { loadPasswordRules, passwordProblem } from '../../lib/passwords';
import { primaryButton, secondaryButton } from '../../lib/styles';

type Errors = Partial<Record<'current' | 'next' | 'confirm', string>>;

// forced: the password has expired (Login Settings / Password reset date), so it must be changed before anything
// else; there is no Cancel.
export default function ChangePasswordDialog({ user, onClose, forced = false }: { user: string; onClose: () => void; forced?: boolean }) {
  useEffect(() => { loadPasswordRules(); }, []);
  const [current, setCurrent] = useState('');
  const [next, setNext] = useState('');
  const [confirm, setConfirm] = useState('');
  const [errors, setErrors] = useState<Errors>({});
  const [saving, setSaving] = useState(false);

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    const errs: Errors = {};
    if (!current) errs.current = 'Enter your current password';
    const problem = passwordProblem(next, user);
    if (!next) errs.next = 'Enter a new password';
    else if (problem) errs.next = problem;
    else if (next === current) errs.next = 'Choose a password different from the current one';
    if (next && confirm !== next) errs.confirm = 'The two new passwords don\'t match';
    setErrors(errs);
    if (Object.keys(errs).length) {
      document.getElementById(`cp-${Object.keys(errs)[0]}`)?.focus();
      return;
    }
    setSaving(true);
    if (!(await toast.later('ok', 'Your password will be changed when this closes.', 'Password not changed.'))) {
      setSaving(false);
      return;
    }
    // Both passwords leave the browser only encrypted (lib/secureField.ts).
    Promise.all([encryptSecret(current, 'current'), encryptSecret(next, 'new')])
      .then(([cur, nxt]) => api<{ access_token: string }>('/auth/change-password', { method: 'POST', body: { current_password: cur, new_password: nxt } }))
      .then(res => {
        setAuthToken(res.access_token);
        toast.ok('Password changed. Other devices signed in as you have been signed out.');
        onClose();
      })
      .catch(err => {
        const text = errorText(err);
        if (/current password/i.test(text)) setErrors({ current: text });
        else toast.error(text);
      })
      .finally(() => setSaving(false));
  };

  const clear = (key: keyof Errors) => setErrors(prev => ({ ...prev, [key]: undefined }));

  return (
    <Modal
      title={forced ? `Your password has expired · ${user}` : `Change password · ${user}`}
      onClose={forced ? () => undefined : onClose}
      footer={<>
        {!forced && <button type="button" onClick={onClose} className={secondaryButton}>Cancel</button>}
        <button type="submit" form="change-password" disabled={saving} className={primaryButton}>{saving ? 'Saving…' : 'Change password'}</button>
      </>}
    >
      <form id="change-password" onSubmit={submit} className="space-y-4" noValidate>
        {forced && <p className="text-xs text-stone-600 dark:text-stone-300">Choose a new password to continue.</p>}
        <TextField id="cp-current" label="Current password" type="password" value={current} required
          hasError={!!errors.current} errorMessage={errors.current}
          onChange={e => { setCurrent(e.target.value); clear('current'); }} />
        <TextField id="cp-next" label="New password" type="password" value={next} required
          hasError={!!errors.next} errorMessage={errors.next}
          onChange={e => { setNext(e.target.value); clear('next'); }} />
        <TextField id="cp-confirm" label="New password again" type="password" value={confirm} required
          hasError={!!errors.confirm} errorMessage={errors.confirm}
          onChange={e => { setConfirm(e.target.value); clear('confirm'); }} />
        <p className="text-xs text-stone-500 dark:text-stone-400">At least 8 characters, mixing letters with numbers or symbols.</p>
      </form>
    </Modal>
  );
}
