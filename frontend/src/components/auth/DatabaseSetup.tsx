// One-time Database setup on the sign-in page (backend routers/setup.py). Shown instead of the login form
// while the backend has no database set up. The client connects the app to their own local Microsoft SQL
// Server (the one their desktop vLQMS uses), and the app copies the old data into its Web_ tables:
//   1. Server: setup code (printed in the backend's window), server (localhost, localhost\SQLEXPRESS),
//      optional port, SQL login or Windows login → Test connection lists the databases;
//   2. Database: pick it → Connect saves the connection and shows what it holds (tables, rows, room);
//   3. Copy: creates the Web_ tables and copies the old data, with progress; then the login form comes back.
// The password leaves the browser only encrypted (lib/secureField.ts, purpose "db") and is never shown again.

import { useEffect, useRef, useState, type FormEvent } from 'react';
import { AlertCircle, CheckCircle2, Database, Loader2, PlugZap } from 'lucide-react';
import { api, errorText } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';

export type SetupCheck = {
  database: string; edition: string; version: string; can_create_tables: boolean; legacy_tables: number;
  legacy_rows: number; legacy_found: boolean; web_tables: number; schema_version: number; web_has_saved: boolean;
  ready: boolean; used_mb: number; copy_mb: number; after_copy_mb: number; limit_mb: number | null; fits: boolean;
};
type Progress = {
  state?: string; tables_total?: number; tables_done?: number; rows_total?: number; rows_done?: number; table?: string;
  error?: string;
};
type Step = 'server' | 'database' | 'copy';

const num = (n: number) => n.toLocaleString('en-IN');

export default function DatabaseSetup({ inputClass, labelClass, onDone }: { inputClass: string; labelClass: string; onDone: () => void }) {
  const [step, setStep] = useState<Step>('server');
  const [f, setF] = useState({ setup_code: '', host: 'localhost', port: '', auth: 'sql' as 'sql' | 'windows', user: 'sa', password: '' });
  const [databases, setDatabases] = useState<string[]>([]);
  const [database, setDatabase] = useState('');
  const [info, setInfo] = useState<SetupCheck | null>(null);
  const [progress, setProgress] = useState<Progress>({});
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);
  const timer = useRef<number | undefined>(undefined);
  useEffect(() => () => window.clearInterval(timer.current), []);

  const set = (k: keyof typeof f, v: string) => { setF(x => ({ ...x, [k]: v })); setErrors(e => ({ ...e, [k]: '' })); setError(''); };

  const server = async () => ({
    setup_code: f.setup_code.trim(), host: f.host.trim(), port: f.port.trim(), auth: f.auth,
    user: f.auth === 'sql' ? f.user.trim() : '', password: f.auth === 'sql' && f.password ? await encryptSecret(f.password, 'db') : '',
  });

  const test = async (e: FormEvent) => {
    e.preventDefault();
    const errs: Record<string, string> = {};
    if (!f.setup_code.trim()) errs.setup_code = 'Enter the setup code shown in the backend window';
    if (!f.host.trim()) errs.host = 'Enter the server, e.g. localhost or localhost\\SQLEXPRESS';
    if (f.port && !/^\d{1,5}$/.test(f.port)) errs.port = 'Digits only, or leave it blank';
    if (f.auth === 'sql' && !f.user.trim()) errs.user = 'Enter the SQL login';
    setErrors(errs);
    if (Object.keys(errs).length) { document.getElementById(`db-${Object.keys(errs)[0]}`)?.focus(); return; }
    setBusy(true);
    try {
      const r = await api<{ ok: boolean; databases?: string[]; error?: string }>('/setup/test', { method: 'POST', body: await server() });
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
      setInfo(await api<SetupCheck>('/setup/connect', { method: 'POST', body: { ...(await server()), database } }));
      setStep('copy');
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const poll = () => {
    window.clearInterval(timer.current);
    timer.current = window.setInterval(() => {
      api<Progress>('/setup/progress').then(p => {
        setProgress(p);
        if (p.state === 'done') { window.clearInterval(timer.current); window.setTimeout(onDone, 1200); }
        if (p.state === 'failed') { window.clearInterval(timer.current); setError(p.error || 'The copy failed.'); setBusy(false); }
      }).catch(() => undefined);
    }, 1500);
  };

  const run = async () => {
    setBusy(true); setError('');
    try {
      await api('/setup/run', { method: 'POST', body: { setup_code: f.setup_code.trim() } });
      setProgress({ state: 'starting' });
      poll();
    } catch (err) { setError(errorText(err)); setBusy(false); }
  };

  const field = (k: 'setup_code' | 'host' | 'port' | 'user' | 'password', caption: string, extra: Record<string, string> = {}, cls = '') => (
    <div>
      <label htmlFor={`db-${k}`} className={labelClass}>{caption}</label>
      <input id={`db-${k}`} value={f[k]} onChange={e => set(k, e.target.value)} aria-invalid={!!errors[k]}
        {...extra} className={`${inputClass} ${cls} ${errors[k] ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
      {errors[k] && <p className="mt-1 text-xs text-red-600 dark:text-red-400">{errors[k]}</p>}
    </div>
  );
  const pct = progress.rows_total ? Math.min(100, Math.round((progress.rows_done ?? 0) * 100 / progress.rows_total)) : 0;
  const copying = busy && step === 'copy';

  return (
    <div className="glass-card-login animate-fadeInUp rounded-2xl border border-[#E3D9CD] p-7 shadow-[0_20px_50px_rgba(72,53,38,0.08)] dark:border-[#2A2A2A] dark:shadow-card">
      <div className="mb-5 flex items-center gap-2">
        <Database size={18} className="text-[#64283A] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Database setup</h2>
        <span className="ml-auto text-[11px] text-[#70675F] dark:text-stone-400">Step {step === 'server' ? 1 : step === 'database' ? 2 : 3} of 3</span>
      </div>

      {step === 'server' && (
        <form onSubmit={test} noValidate className="space-y-4">
          <p className="text-xs text-[#70675F] dark:text-stone-400">Connect e-vLQMS ERP to this shop's SQL Server (the one the desktop vLQMS uses). This is done once.</p>
          {field('setup_code', 'Setup code', { autoComplete: 'off', placeholder: 'Shown in the backend window' }, 'uppercase tracking-widest')}
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
              {busy ? <Loader2 size={16} className="animate-spin" /> : <Database size={16} />}{busy ? 'Connecting…' : 'Connect'}
            </button>
          </div>
        </div>
      )}

      {step === 'copy' && info && (
        <div className="space-y-4 text-xs">
          <dl className="grid grid-cols-2 gap-x-3 gap-y-1.5">
            <dt className="text-[#70675F] dark:text-stone-400">Database</dt><dd className="font-medium">{info.database}</dd>
            <dt className="text-[#70675F] dark:text-stone-400">SQL Server</dt><dd>{info.edition}</dd>
            <dt className="text-[#70675F] dark:text-stone-400">Old data</dt>
            <dd>{info.legacy_found ? `${num(info.legacy_tables)} tables, ${num(info.legacy_rows)} rows` : 'None (a new shop)'}</dd>
            <dt className="text-[#70675F] dark:text-stone-400">Space after copy</dt>
            <dd>{num(Math.round(info.after_copy_mb))} MB{info.limit_mb ? ` of ${num(info.limit_mb)} MB` : ''}</dd>
          </dl>
          {info.web_has_saved && <ErrorLine text="The web app has already saved data here. Copying again would overwrite it, so it isn't offered here." />}
          {!info.fits && <ErrorLine text="There isn't enough room in this database for the copy." />}
          {!info.can_create_tables && <ErrorLine text="This login can't create tables. Use a login with db_owner rights." />}
          {progress.state && progress.state !== 'idle' && (
            <div aria-live="polite">
              <div className="h-2 overflow-hidden rounded-full bg-[#E3D9CD] dark:bg-[#2A2A2A]">
                <div className="h-full bg-[#64283A] transition-all dark:bg-[#D4A847]" style={{ width: `${progress.state === 'done' ? 100 : pct}%` }} />
              </div>
              <p className="mt-2 text-[#70675F] dark:text-stone-400">
                {progress.state === 'done' ? 'Done. Loading the sign-in…'
                  : progress.state === 'copying' ? `Copying ${progress.table} · table ${progress.tables_done ?? 0} of ${progress.tables_total} · ${num(progress.rows_done ?? 0)} of ${num(progress.rows_total ?? 0)} rows`
                    : `${progress.state[0].toUpperCase()}${progress.state.slice(1)}…`}
              </p>
            </div>
          )}
          {error && <ErrorLine text={error} />}
          <button type="button" onClick={run} disabled={copying || info.web_has_saved || !info.fits || !info.can_create_tables} className={buttonClass}>
            {copying ? <Loader2 size={16} className="animate-spin" /> : <Database size={16} />}
            {copying ? 'Copying… keep this page open' : info.legacy_found ? 'Create tables and copy the old data' : 'Create the tables'}
          </button>
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
