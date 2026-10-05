// Header switch next to Shortcuts: Test data ON (the backend's sample data, nothing saved) or OFF (the shop's live
// SQL Server data). Only an Administrator can switch it (POST /utilities/test-data); everyone else sees a
// "Test data" badge while it is on, so nobody mistakes sample figures for the shop's. The page reloads after a
// switch so every screen fetches the data again. Shown at every screen width, so a counter tablet sees it too.
// While it is on, users, rights, settings and the company profile can't be changed (main.py KEPT_IN_TEST).

import { FlaskConical } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { api, errorText, type ApiAbout, type DataMode } from '../../lib/api';

export default function DataModeToggle({ mode, isAdmin }: { mode: DataMode | undefined; isAdmin: boolean }) {
  const on = mode === 'test';
  if (!isAdmin) {
    return on ? (
      <span className="inline-flex h-9 shrink-0 items-center gap-1.5 rounded-xl border border-amber-500/40 bg-amber-500/10 px-2.5 text-xs font-semibold text-amber-700 dark:text-amber-300"
        title="The screens show sample data for testing; nothing is saved">
        <FlaskConical size={15} />Test data
      </span>
    ) : null;
  }

  const toggle = async () => {
    const next = !on;
    const text = next
      ? 'Test data will be switched ON when this closes: the screens show sample data and nothing is saved.'
      : 'Test data will be switched OFF when this closes: the screens go back to the live SQL Server data.';
    if (!(await toastStore.defer('refresh', text, 'Cancelled. Nothing was switched.'))) return;
    try {
      await api<ApiAbout>('/utilities/test-data', { method: 'POST', body: { on: next } });
      window.location.reload();   // every screen loads its data again
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  return (
    <button onClick={toggle} aria-pressed={on}
      title={on ? 'Test data is ON: sample data, nothing is saved. Click to go back to the live data.' : 'Test data is OFF: live SQL Server data. Click to use sample data for testing.'}
      className={`inline-flex h-9 shrink-0 items-center gap-1.5 rounded-xl border px-2.5 text-xs font-medium transition-all active:scale-[0.98] ${on
        ? 'border-amber-500/50 bg-amber-500/15 text-amber-800 hover:bg-amber-500/25 dark:text-amber-300'
        : 'border-[#E3D9CD] bg-[#FFFCF7] text-[#70675F] hover:bg-[#F3EEE6] hover:text-[#29231F] dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-300 dark:hover:bg-[#1C1C1C]'}`}>
      <FlaskConical size={15} className={on ? '' : 'text-[#64283A] dark:text-[#D4A847]'} />
      <span className="hidden sm:inline">Test data</span> {on ? 'ON' : 'OFF'}
    </button>
  );
}
