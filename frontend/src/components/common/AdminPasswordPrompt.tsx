// Asks for the admin password before a destructive utility (legacy UtlMisc.AdvancedADMLogin): the additional
// ADMIN password set in Admin Master (Security), or the user's own login password while none is set.
// useAdminPassword() gives `ask()` (resolves to the typed password, or null when cancelled) and the dialog to
// render. The caller encrypts it with encryptSecret(password, 'admin') just before sending (lib/secureField.ts).
import { useCallback, useRef, useState, type FormEvent } from 'react';
import { LockKeyhole } from 'lucide-react';
import Modal from './Modal';
import { InlineError } from './FormErrorSummary';
import { field, fieldError, label, primaryButton, secondaryButton } from '../../lib/styles';

export function useAdminPassword() {
  const [open, setOpen] = useState(false);
  const [value, setValue] = useState('');
  const [error, setError] = useState('');
  const resolver = useRef<(v: string | null) => void>(undefined);

  const ask = useCallback(() => new Promise<string | null>(resolve => {
    resolver.current = resolve;
    setValue('');
    setError('');
    setOpen(true);
  }), []);

  const close = (v: string | null) => {
    resolver.current?.(v);
    setOpen(false);
    setValue('');
  };

  const submit = (e?: FormEvent) => {
    e?.preventDefault();
    if (!value) {
      setError('Enter the password');
      document.getElementById('admin-pwd')?.focus();
      return;
    }
    close(value);
  };

  const dialog = !open ? null : (
    <Modal title="Admin password" onClose={() => close(null)} footer={<>
      <button className={secondaryButton} onClick={() => close(null)}>Cancel</button>
      <button className={primaryButton} onClick={() => submit()}>Continue</button>
    </>}>
      <form onSubmit={submit} className="flex flex-col gap-2">
        <p className="flex items-start gap-2 text-xs text-stone-600 dark:text-stone-300">
          <LockKeyhole size={16} className="mt-0.5 shrink-0" />
          Enter the additional ADMIN password (Admin Master (Security)). If none has been set, enter your own login password.
        </p>
        <label htmlFor="admin-pwd" className={label}>Password</label>
        <input id="admin-pwd" type="password" autoFocus autoComplete="off" value={value}
          onChange={e => { setValue(e.target.value); setError(''); }}
          className={`${field} ${error ? fieldError : ''}`} aria-invalid={!!error} />
        <InlineError message={error} />
      </form>
    </Modal>
  );

  return { ask, dialog };
}
