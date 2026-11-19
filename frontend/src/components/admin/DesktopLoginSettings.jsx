// Admin Master → Login Settings, the desktop's own settings (frmUtlLoginSettings, saved in UtlMisc so the desktop app follows them too):
// login required, rights by user or by role, and the password policy: never expires / expires after N days, and the rules (minimum special
// characters, numbers, letters, length, lower and upper case). The desktop's checks and messages are kept. Active Directory sign-in is not offered.

import { useEffect, useState } from 'react';
import { Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getDesktopLoginSettings, saveDesktopLoginSettings } from '../../lib/api';
import { field, label, panel, primaryButton, sectionTitle } from '../../lib/styles';

const NUMS = [['min_special', 'Min. special characters'], ['min_numeric', 'Min. numbers'], ['min_alpha', 'Min. alphabets'], ['min_length', 'Min. password length'],
  ['min_lower', 'Min. lower case'], ['min_upper', 'Min. upper case']];

export default function DesktopLoginSettings() {
  const [s, setS] = useState(null);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);

  useEffect(() => { getDesktopLoginSettings().then(setS).catch(e => setProblem(errorText(e))); }, []);
  if (!s) return problem ? <InlineError message={problem} /> : null;
  if (!s.available) return null;   // this database has no UtlMisc password columns
  const set = (patch) => { setS({ ...s, ...patch }); setProblem(''); };

  const save = async () => {
    // The desktop's checks, in its order (the server repeats them).
    if (s.policy && s.rules) {
      if (!s.never_expires && s.expiry_days <= 0) return setProblem('Please enter the Password Expiry Days!!');
      if (s.min_special + s.min_numeric + s.min_alpha > s.min_length) return setProblem('Min. no of Special Character + Alphabets + Numeric should not be less than the Password length!!');
      if (s.min_alpha !== s.min_lower + s.min_upper) return setProblem('Mni. No of LowerCase + UpperCase should be equal to Alphabets Length!!');
    } else if (s.policy && !s.never_expires && s.expiry_days <= 0) {
      return setProblem('Please enter the Password Expiry Days!!');
    }
    if (!(await toastStore.defer('ok', 'Login settings will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      setS(await saveDesktopLoginSettings({ ...s }));
      toastStore.add('ok', 'Login settings saved for the desktop app too.');
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const num = (k, caption) => (
    <label key={k}><span className={label}>{caption}</span>
      <input type="number" min={0} max={99} value={s[k]} disabled={!s.policy || !s.rules} onChange={e => set({ [k]: Math.trunc(Number(e.target.value)) })} className={field} /></label>
  );

  return (
    <section className={`${panel} mb-5 space-y-3 p-4`} aria-label="Desktop login settings">
      <div>
        <h2 className={sectionTitle}>Desktop login settings</h2>
        <p className="text-[11px] text-stone-500">Saved in the shop's database: the desktop app follows the same password policy.</p>
      </div>
      <div className="flex flex-wrap gap-6 text-xs">
        <label className="flex items-center gap-2"><input type="checkbox" checked={s.login_required} onChange={e => set({ login_required: e.target.checked })} />Login required</label>
        <label className="flex items-center gap-2"><input type="checkbox" checked={s.user_based} onChange={e => set({ user_based: e.target.checked })} />Rights by user (off: by role)</label>
        <label className="flex items-center gap-2"><input type="checkbox" checked={s.policy} onChange={e => set({ policy: e.target.checked })} />Password policy</label>
      </div>
      <fieldset disabled={!s.policy} className="space-y-3 disabled:opacity-50">
        <div className="flex flex-wrap items-center gap-4 text-xs">
          <label className="flex items-center gap-2"><input type="radio" name="dl-exp" checked={s.never_expires} onChange={() => set({ never_expires: true, expiry_days: 0 })} />Never expires</label>
          <label className="flex items-center gap-2"><input type="radio" name="dl-exp" checked={!s.never_expires} onChange={() => set({ never_expires: false })} />Expires after</label>
          <input aria-label="Expiry days" type="number" min={0} max={3650} value={s.expiry_days} disabled={s.never_expires} onChange={e => set({ expiry_days: Math.trunc(Number(e.target.value)) })} className={`${field} w-24`} />
          <span className="text-xs">days</span>
          <label className="ml-4 flex items-center gap-2"><input type="checkbox" checked={s.rules} onChange={e => set({ rules: e.target.checked })} />Password rules</label>
        </div>
        <div className="grid gap-3 sm:grid-cols-3">{NUMS.map(([k, c]) => num(k, c))}</div>
      </fieldset>
      {problem && <InlineError message={problem} />}
      <button className={primaryButton} disabled={busy} onClick={() => void save()}><Save size={14} />Save</button>
    </section>
  );
}
