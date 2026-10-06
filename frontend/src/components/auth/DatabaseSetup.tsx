// One-time Database setup on the sign-in page (backend routers/setup.py). Shown instead of the login form
// while the backend has no database set up. The client connects the app straight to their own Microsoft SQL
// Server database (the one their desktop vLQMS uses); nothing is copied, the app works on its tables directly:
//   1. Server: server (localhost, localhost\SQLEXPRESS), optional port, SQL login or Windows login → Test
//      connection lists the databases;
//   2. Database: pick it → Connect saves the connection, creates the app's own few tables and loads the shop's
//      data; then the login form comes back.
// The password leaves the browser only encrypted (lib/secureField.ts, purpose "db") and is never shown again.

import { useState, type FormEvent } from 'react';
import { AlertCircle, CheckCircle2, Database, Loader2, PlugZap } from 'lucide-react';
import { errorText, setupConnect, setupTest } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';

type Step = 'server' | 'database';

export default function DatabaseSetup({ inputClass, labelClass, onDone }: { inputClass: string; labelClass: string; onDone: () => void }) {
  const [step, setStep] = useState<Step>('server');
  const [f, setF] = useState({ host: 'localhost', port: '', auth: 'sql' as 'sql' | 'windows', user: 'sa', password: '' });
  const [databases, setDatabases] = useState<string[]>([]);
  const [database, setDatabase] = useState('');
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  const set = (k: keyof typeof f, v: string) => { setF(x => ({ ...x, [k]: v })); setErrors(e => ({ ...e, [k]: '' })); setError(''); };

  const server = async () => ({
    host: f.host.trim(), port: f.port.trim(), auth: f.auth,
    user: f.auth === 'sql' ? f.user.trim() : '', password: f.auth === 'sql' && f.password ? await encryptSecret(f.password, 'db') : '',
  });

  const test = async (e: FormEvent) => {
    e.preventDefault();
    const errs: Record<string, string> = {};
    if (!f.host.trim()) errs.host = 'Enter the server, e.g. localhost or localhost\\SQLEXPRESS';
    if (f.port && !/^\d{1,5}$/.test(f.port)) errs.port = 'Digits only, or leave it blank';
    if (f.auth === 'sql' && !f.user.trim()) errs.user = 'Enter the SQL login';
    setErrors(errs);
    if (Object.keys(errs).length) { document.getElementById(`db-${Object.keys(errs)[0]}`)?.focus(); return; }
    setBusy(true);
    try {
      const r = await setupTest(await server());
      if (!r.ok) { setError(r.error || 'Could not connect.'); return; }
      setDatabases(r.databases ?? []);
      setDatabase(d => d || (r.databases ?? []).find(n => /lqms/i.test(n)) || r.databases?.[0] || '');
      setStep('database');
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const connect = async () => {
    if (!database) { setErrors({ database: 'Pick the database' }); return; }
    setBusy(true);
    try {
      await setupConnect({ ...(await server()), database });
      onDone();
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const field = (k: 'host' | 'port' | 'user' | 'password', caption: string, extra: Record<string, string> = {}, cls = '') => (
    <div>
      <label htmlFor={`db-${k}`} className={labelClass}>{caption}</label>
      <input id={`db-${k}`} value={f[k]} onChange={e => set(k, e.target.value)} aria-invalid={!!errors[k]}
        {...extra} className={`${inputClass} ${cls} ${errors[k] ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
      {errors[k] && <p className="mt-1 text-xs text-red-600 dark:text-red-400">{errors[k]}</p>}
    </div>
  );

  return (
    <div className="glass-card-login animate-fadeInUp rounded-2xl border border-[#E3D9CD] p-7 shadow-[0_20px_50px_rgba(72,53,38,0.08)] dark:border-[#2A2A2A] dark:shadow-card">
      <div className="mb-5 flex items-center gap-2">
        <Database size={18} className="text-[#64283A] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Database setup</h2>
        <span className="ml-auto text-[11px] text-[#70675F] dark:text-stone-400">Step {step === 'server' ? 1 : 2} of 2</span>
      </div>

      {step === 'server' && (
        <form onSubmit={test} noValidate className="space-y-4">
          <p className="text-xs text-[#70675F] dark:text-stone-400">Connect e-vLQMS ERP to this shop's SQL Server (the one the desktop vLQMS uses). This is done once.</p>
          {field('host', 'Server', { placeholder: 'localhost or localhost\\SQLEXPRESS' })}
          {field('port', 'Port (optional)', { inputMode: 'numeric', placeholder: 'Blank for a named instance' })}
          <div>
            <span className={labelClass}>Sign in to SQL Server with</span>
            <div className="flex gap-4 text-xs">
              <label className="flex items-center gap-2"><input type="radio" checked={f.auth === 'sql'} onChange={() => set('auth', 'sql')} />SQL login</label>
              <label className="flex items-center gap-2"><input type="radio" checked={f.auth === 'windows'} onChange={() => set('auth', 'windows')} />Windows login</label>
            </div>
          </div>
          {f.auth === 'sql' && <>
            {field('user', 'Login', { autoComplete: 'off' })}
            {field('password', 'Password', { type: 'password', autoComplete: 'new-password' })}
          </>}
          {error && <ErrorLine text={error} />}
          <button type="submit" disabled={busy} className={buttonClass}>
            {busy ? <Loader2 size={16} className="animate-spin" /> : <PlugZap size={16} />}{busy ? 'Testing…' : 'Test connection'}
          </button>
        </form>
      )}

      {step === 'database' && (
        <div className="space-y-4">
          <p className="flex items-center gap-2 text-xs text-emerald-700 dark:text-emerald-400"><CheckCircle2 size={14} />Connected. Pick this shop's database.</p>
          <p className="text-xs text-[#70675F] dark:text-stone-400">The app works on this database directly, next to the desktop vLQMS. Back it up first.</p>
          <div>
            <label htmlFor="db-database" className={labelClass}>Database</label>
            <select id="db-database" value={database} onChange={e => { setDatabase(e.target.value); setErrors({}); }} className={inputClass} aria-invalid={!!errors.database}>
              {databases.map(d => <option key={d}>{d}</option>)}
            </select>
            {errors.database && <p className="mt-1 text-xs text-red-600 dark:text-red-400">{errors.database}</p>}
          </div>
          {error && <ErrorLine text={error} />}
          <div className="flex gap-2">
            <button type="button" onClick={() => { setStep('server'); setError(''); }} className="rounded-xl border border-[#DCD1C5] px-4 text-xs dark:border-[#2A2A2A]">Back</button>
            <button type="button" onClick={connect} disabled={busy} className={buttonClass}>
              {busy ? <Loader2 size={16} className="animate-spin" /> : <Database size={16} />}{busy ? 'Connecting and loading the data…' : 'Connect'}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

const buttonClass = `flex flex-1 w-full items-center justify-center gap-2 rounded-xl py-3 text-sm font-bold transition-all
  disabled:pointer-events-none disabled:opacity-45 bg-[#64283A] text-white hover:bg-[#521E2E]
  dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]`;

function ErrorLine({ text }: { text: string }) {
  return (
    <p role="alert" className="flex items-center gap-2 rounded-xl bg-[#9A4B45]/10 px-4 py-3 text-xs font-medium text-[#9A4B45] dark:bg-red-500/10 dark:text-red-400">
      <AlertCircle size={14} className="shrink-0" />{text}
    </p>
  );
}
