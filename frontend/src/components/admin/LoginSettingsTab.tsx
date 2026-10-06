// Login settings tab of Admin Master (Security) (legacy frmUtlLoginSettings and "Change Adnl. ADMIN Password"):
// - password rules (minimum length, mix, capital / small letter, number, symbol) for every new password;
// - password expiry (never, or every N days: the user must change it at sign-in);
// - whether the sign-in page offers "Remember me" (keeps that device signed in until Sign out);
// - the additional ADMIN password asked before destructive utilities (Merge Items, Remove Duplicate Invoice…).
// Passwords typed here leave the browser only encrypted (lib/secureField.ts).

import { useEffect, useState, type FormEvent } from 'react';
import { KeyRound, Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getLoginSettings, getSecurity, saveLoginSettings, setAdditionalAdminPassword, type ApiLoginSettings } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';
import { passwordProblem, setPasswordRules } from '../../lib/passwords';
import { badge, field, fieldError, label, panel, primaryButton, sectionTitle } from '../../lib/styles';

const CHECKS: [keyof ApiLoginSettings, string][] = [
  ['require_mix', 'Mix letters with numbers or symbols'], ['require_upper', 'At least one capital letter'],
  ['require_lower', 'At least one small letter'], ['require_digit', 'At least one number'], ['require_special', 'At least one symbol (# @ ! …)'],
];

export default function LoginSettingsTab({ currentUser }: { currentUser: string }) {
  const [s, setS] = useState<ApiLoginSettings | null>(null);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [adnlSet, setAdnlSet] = useState(false);
  const [pw, setPw] = useState({ current: '', next: '', again: '' });
  const [pwErrors, setPwErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    getLoginSettings().then(setS).catch(e => toastStore.add('error', errorText(e)));
    getSecurity().then(r => setAdnlSet(r.adnl_admin_set)).catch(() => undefined);
  }, []);

  const save = async () => {
    if (!s) return;
    const errs: Record<string, string> = {};
    if (!Number.isInteger(s.min_length) || s.min_length < 8 || s.min_length > 64) errs.min_length = 'Between 8 and 64';
    if (!Number.isInteger(s.password_expiry_days) || s.password_expiry_days < 0 || s.password_expiry_days > 3650) errs.password_expiry_days = '0 (never) to 3650 days';
    setErrors(errs);
    if (Object.keys(errs).length) { document.getElementById(`ls-${Object.keys(errs)[0]}`)?.focus(); return; }
    const body = { ...s };
    if (!(await toastStore.defer('ok', 'Login settings will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    saveLoginSettings(body)
      .then(r => { setS(r); setPasswordRules(r); toastStore.add('ok', 'Login settings saved.'); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  const changeAdnl = async (e: FormEvent, remove = false) => {
    e.preventDefault();
    const errs: Record<string, string> = {};
    if (!pw.current) errs.current = 'Enter your own login password';
    if (!remove) {
      const p = pw.next ? passwordProblem(pw.next, currentUser) : 'Enter the new additional ADMIN password';
      if (p) errs.next = p;
      else if (pw.again !== pw.next) errs.again = 'Confirm Password should be same as password!!';
    }
    setPwErrors(errs);
    if (Object.keys(errs).length) { document.getElementById(`ap-${Object.keys(errs)[0]}`)?.focus(); return; }
    const { current, next } = pw;
    if (!(await toastStore.defer('ok', remove ? 'The additional ADMIN password will be removed when this closes.' : 'The additional ADMIN password will be set when this closes.', 'Cancelled. Nothing was changed.'))) return;
    try {
      const body = { current_password: await encryptSecret(current, 'current'), new_password: remove ? '' : await encryptSecret(next, 'new') };
      const r = await setAdditionalAdminPassword(body);
      setAdnlSet(r.adnl_admin_set);
      setPw({ current: '', next: '', again: '' });
      toastStore.add('ok', r.adnl_admin_set ? 'Additional ADMIN password set.' : 'Additional ADMIN password removed: your own password is asked instead.');
    } catch (err) {
      const text = errorText(err);
      if (/your password/i.test(text)) setPwErrors({ current: text });
      else toastStore.add('error', text);
    }
  };

  if (!s) return <p className={`${panel} p-6 text-xs text-stone-500`}>Loading…</p>;
  const num = (k: 'min_length' | 'password_expiry_days', caption: string, hint: string) => (
    <div>
      <label htmlFor={`ls-${k}`} className={label}>{caption}</label>
      <input id={`ls-${k}`} type="number" value={s[k]} onChange={e => { setS({ ...s, [k]: Number(e.target.value) }); setErrors(x => ({ ...x, [k]: '' })); }}
        className={`${field} ${errors[k] ? fieldError : ''}`} aria-invalid={!!errors[k]} />
      <InlineError message={errors[k]} />
      <p className="mt-1 text-[11px] text-stone-500">{hint}</p>
    </div>
  );
  const pwField = (k: 'current' | 'next' | 'again', caption: string) => (
    <div>
      <label htmlFor={`ap-${k}`} className={label}>{caption}</label>
      <input id={`ap-${k}`} type="password" autoComplete={k === 'current' ? 'current-password' : 'new-password'} value={pw[k]}
        onChange={e => { setPw({ ...pw, [k]: e.target.value }); setPwErrors(x => ({ ...x, [k]: '' })); }}
        className={`${field} ${pwErrors[k] ? fieldError : ''}`} aria-invalid={!!pwErrors[k]} />
      <InlineError message={pwErrors[k]} />
    </div>
  );
  return (
    <div className="grid grid-cols-1 gap-4 lg:grid-cols-2">
      <section className={`${panel} space-y-4 p-5`}>
        <h2 className={sectionTitle}>Password rules</h2>
        {num('min_length', 'Minimum length', 'At least 8.')}
        <div className="space-y-1.5">
          {CHECKS.map(([k, caption]) => (
            <label key={k} className="flex items-center gap-2 text-xs"><input type="checkbox" checked={!!s[k]} onChange={e => setS({ ...s, [k]: e.target.checked })} />{caption}</label>
          ))}
        </div>
        {num('password_expiry_days', 'Password expires every (days)', '0 = never expires. When it expires the user must change it at sign-in.')}
        <label className="flex items-center gap-2 text-xs">
          <input type="checkbox" checked={s.show_remember_me} onChange={e => setS({ ...s, show_remember_me: e.target.checked })} />
          Offer "Remember me" on the sign-in page (keeps that device signed in until Sign out)
        </label>
        <div className="flex justify-end"><button className={primaryButton} onClick={save}><Save size={14} />Save login settings</button></div>
      </section>
      <section className={`${panel} space-y-4 p-5`}>
        <div className="flex items-center justify-between">
          <h2 className={sectionTitle}>Change Adnl. ADMIN Password</h2>
          <span className={badge(adnlSet ? 'success' : 'neutral')}>{adnlSet ? 'Set' : 'Not set'}</span>
        </div>
        <p className="text-[11px] text-stone-500">Asked before Merge Items, Replace and Remove Duplicate items and Remove Duplicate Invoice. While it isn't set, the user's own password is asked instead.</p>
        <form onSubmit={e => changeAdnl(e)} className="space-y-3">
          {pwField('current', 'Your login password')}
          {pwField('next', 'New additional ADMIN password')}
          {pwField('again', 'Confirm Password')}
          <div className="flex justify-end gap-2">
            {adnlSet && <button type="button" className="text-xs font-medium text-[#9A4B45] underline-offset-2 hover:underline dark:text-red-400" onClick={e => changeAdnl(e, true)}>Remove it</button>}
            <button type="submit" className={primaryButton}><KeyRound size={14} />Set password</button>
          </div>
        </form>
      </section>
    </div>
  );
}
