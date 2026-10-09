// One-time Database setup on the sign-in page (backend routers/setup.py). Shown instead of the login form
// while the backend has no database set up. The client connects the app straight to their own Microsoft SQL
// Server database (the one their desktop vLQMS uses); nothing is copied, the app works on its tables directly:
//   1. Server: server (localhost, localhost\SQLEXPRESS), optional port, SQL login → Test
//      connection lists the databases;
//   2. Database: pick it → Connect saves the connection, creates the app's own few tables and loads the shop's
//      data; then the login form comes back.
// The password leaves the browser only encrypted (lib/secureField.js, purpose "db") and is never shown again.
// `change` mode (Global Settings → Database, and the sign-in page's Database settings while the database is offline):
// the form starts with the settings in use, a blank password keeps the saved one, and the title says so.

import { useEffect, useState, } from 'react';
import { AlertCircle, CheckCircle2, Database, Loader2, PlugZap, Search, Server } from 'lucide-react';
import { errorText, getSetupCurrent, getSetupInstances, setupConnect, setupScan, setupTest } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';



export default function DatabaseSetup({ inputClass, labelClass, onDone, change = false, plain = false, onCancel }

) {
  const [step, setStep] = useState('server');
  const [f, setF] = useState({ host: 'localhost', port: '', auth: 'sql' , user: 'sa', password: '' });
  const [databases, setDatabases] = useState([]);
  const [database, setDatabase] = useState('');
  const [errors, setErrors] = useState({});
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);
  const [hasPassword, setHasPassword] = useState(false);
  const [found, setFound] = useState(null);     // SQL Server instances of any version the app found: [{label, product, port, ...}]
  const [scanned, setScanned] = useState(null); // the login tried on every instance of the server: each with its databases

  // Every SQL Server on the network and this PC, whatever its version (2008 ... 2022); a typed server limits it to that one.
  const findServers = async () => {
    setBusy(true); setError('');
    try {
      const typed = f.host.trim();
      const r = await getSetupInstances(typed && !/^(localhost|\.|\(local\))$/i.test(typed) ? typed.split('\\')[0] : '');
      setFound(r.instances ?? []);
      if (!(r.instances ?? []).length) setError('No SQL Server answered. Type the server name or IP, or check that the SQL Server Browser service is running.');
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const chooseInstance = (e) => {
    setF(x => ({ ...x, host: e.address || e.server, port: e.port || '' }));
    setErrors({}); setError(''); setScanned(null);
  };

  // The same login on every instance of the server typed (never across the network): each instance's databases.
  const scanServer = async () => {
    if (!f.host.trim() || !f.user.trim()) { setError('Type the server and the SQL login first, then scan.'); return; }
    setBusy(true); setError('');
    try {
      const r = await setupScan({ host: f.host.trim().split('\\')[0], user: f.user.trim(), password: f.password ? await encryptSecret(f.password, 'db') : '' });
      setScanned(r.instances ?? []);
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const pickScanned = (inst, name) => {
    setF(x => ({ ...x, host: inst.port ? (inst.address || inst.server) : inst.host, port: inst.port || '' }));
    setDatabases(inst.databases); setDatabase(name); setStep('database'); setError('');
  };

  // Changing the settings: start from the ones in use (the password is never sent back).
  useEffect(() => {
    if (!change) return;
    getSetupCurrent().then(c => {
      setF({ host: c.host, port: c.port, auth: c.auth, user: c.user || 'sa', password: '' });
      setDatabase(c.database);
      setHasPassword(c.has_password);
    }).catch(e => setError(errorText(e)));
  }, [change]);

  const set = (k, v) => { setF(x => ({ ...x, [k]: v })); setErrors(e => ({ ...e, [k]: '' })); setError(''); };

  const server = async () => ({
    host: f.host.trim(), port: f.port.trim(), auth: f.auth,
    user: f.auth === 'sql' ? f.user.trim() : '', password: f.auth === 'sql' && f.password ? await encryptSecret(f.password, 'db') : '',
  });

  const test = async (e) => {
    e.preventDefault();
    const errs = {};
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
      setDatabase(d => ((r.databases ?? []).includes(d) ? d : (r.databases ?? []).find(n => /lqms/i.test(n)) || r.databases?.[0] || ''));
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

  const field = (k, caption, extra = {}, cls = '') => (
    <div>
      <label htmlFor={`db-${k}`} className={labelClass}>{caption}</label>
      <input id={`db-${k}`} value={f[k]} onChange={e => set(k, e.target.value)} aria-invalid={!!errors[k]}
        {...extra} className={`${inputClass} ${cls} ${errors[k] ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
      {errors[k] && <p className="mt-1 text-xs text-red-600 dark:text-red-400">{errors[k]}</p>}
    </div>
  );

  return (
    <div className={plain ? '' : 'glass-card-login animate-fadeInUp rounded-2xl border border-[#DDE3EC] p-7 shadow-[0_20px_50px_rgba(72,53,38,0.08)] dark:border-[#2A2A2A] dark:shadow-card'}>
      <div className="mb-5 flex items-center gap-2">
        <Database size={18} className="text-[#1B2B4B] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">{change ? 'Database settings' : 'Database setup'}</h2>
        <span className="ml-auto text-[11px] text-[#5F6B80] dark:text-stone-400">Step {step === 'server' ? 1 : 2} of 2</span>
      </div>

      {step === 'server' && (
        <form onSubmit={test} noValidate className="space-y-4">
          <p className="text-xs text-[#5F6B80] dark:text-stone-400">{change
            ? 'Change the server, login or database SpiritPOS uses. Test the connection, then pick the database. Back the database up first.'
            : "Connect SpiritPOS to this shop's SQL Server (the one the desktop vLQMS uses). This is done once."}</p>
          {field('host', 'Server', { placeholder: 'localhost or localhost\\SQLEXPRESS' })}
          {field('port', 'Port (optional)', { inputMode: 'numeric', placeholder: 'Blank for a named instance' })}
          {field('user', 'Login', { autoComplete: 'off' })}
          {field('password', 'Password', { type: 'password', autoComplete: 'new-password', ...(change && hasPassword ? { placeholder: 'Leave blank to keep the saved password' } : {}) })}

          <div className="space-y-2 rounded-xl border border-[#DDE3EC] p-3 dark:border-[#2A2A2A]">
            <p className="text-[11px] text-[#5F6B80] dark:text-stone-400">The server may run several SQL Server versions side by side. Find them all, or scan one server with the login above to see every database on every version.</p>
            <div className="flex flex-wrap gap-2">
              <button type="button" onClick={findServers} disabled={busy} className="inline-flex items-center gap-1.5 rounded-lg border border-[#D5DCE7] px-3 py-1.5 text-xs font-semibold dark:border-[#2A2A2A]"><Search size={13} />Find SQL Servers</button>
              <button type="button" onClick={scanServer} disabled={busy} className="inline-flex items-center gap-1.5 rounded-lg border border-[#D5DCE7] px-3 py-1.5 text-xs font-semibold dark:border-[#2A2A2A]"><Server size={13} />Scan all versions on this server</button>
            </div>
            {found && found.length > 0 && (
              <ul className="max-h-44 space-y-1 overflow-y-auto text-xs" aria-label="SQL Servers found">
                {found.map(e => (
                  <li key={`${e.server}|${e.instance}`}>
                    <button type="button" onClick={() => chooseInstance(e)} className="flex w-full items-center justify-between gap-2 rounded-lg px-2 py-1.5 text-left hover:bg-[#EDF0F6] dark:hover:bg-[#1C1C1C]">
                      <span className="min-w-0 truncate font-medium">{e.label}</span>
                      <span className="shrink-0 text-[11px] text-[#5F6B80] dark:text-stone-400">{e.product}{e.port ? ` · port ${e.port}` : ''}</span>
                    </button>
                  </li>
                ))}
              </ul>
            )}
            {scanned && (
              <div className="max-h-64 space-y-2 overflow-y-auto text-xs" aria-label="Databases by SQL Server version">
                {scanned.map(inst => (
                  <div key={`${inst.server}|${inst.instance}`} className="rounded-lg border border-[#DDE3EC] p-2 dark:border-[#2A2A2A]">
                    <div className="flex items-center justify-between gap-2"><span className="font-semibold">{inst.label}</span><span className="text-[11px] text-[#5F6B80] dark:text-stone-400">{inst.product}</span></div>
                    {inst.ok ? (
                      <div className="mt-1 flex flex-wrap gap-1">
                        {inst.databases.map(d => (
                          <button key={d} type="button" onClick={() => pickScanned(inst, d)} className={`rounded-md border px-2 py-0.5 ${/lqms/i.test(d) ? 'border-[#1B2B4B] font-semibold dark:border-[#D4A847]' : 'border-[#DDE3EC] dark:border-[#2A2A2A]'}`}>{d}</button>
                        ))}
                        {!inst.databases.length && <span className="text-[#5F6B80]">No databases.</span>}
                      </div>
                    ) : <p className="mt-1 text-[#9A4B45] dark:text-red-400">Could not sign in to this instance with that login.</p>}
                  </div>
                ))}
              </div>
            )}
          </div>
          {error && <ErrorLine text={error} />}
          <div className="flex gap-2">
            {onCancel && <button type="button" onClick={onCancel} className="rounded-xl border border-[#D5DCE7] px-4 text-xs dark:border-[#2A2A2A]">Cancel</button>}
            <button type="submit" disabled={busy} className={buttonClass}>
            {busy ? <Loader2 size={16} className="animate-spin" /> : <PlugZap size={16} />}{busy ? 'Testing…' : 'Test connection'}
            </button>
          </div>
        </form>
      )}

      {step === 'database' && (
        <div className="space-y-4">
          <p className="flex items-center gap-2 text-xs text-emerald-700 dark:text-emerald-400"><CheckCircle2 size={14} />Connected. Pick this shop's database.</p>
          <p className="text-xs text-[#5F6B80] dark:text-stone-400">The app works on this database directly, next to the desktop vLQMS. Back it up first.</p>
          <div>
            <label htmlFor="db-database" className={labelClass}>Database</label>
            <select id="db-database" value={database} onChange={e => { setDatabase(e.target.value); setErrors({}); }} className={inputClass} aria-invalid={!!errors.database}>
              {databases.map(d => <option key={d}>{d}</option>)}
            </select>
            {errors.database && <p className="mt-1 text-xs text-red-600 dark:text-red-400">{errors.database}</p>}
          </div>
          {error && <ErrorLine text={error} />}
          <div className="flex gap-2">
            <button type="button" onClick={() => { setStep('server'); setError(''); }} className="rounded-xl border border-[#D5DCE7] px-4 text-xs dark:border-[#2A2A2A]">Back</button>
            <button type="button" onClick={connect} disabled={busy} className={buttonClass}>
              {busy ? <Loader2 size={16} className="animate-spin" /> : <Database size={16} />}{busy ? 'Connecting and loading the data…' : 'Connect'}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

const buttonClass = `flex flex-1 w-full items-center justify-center gap-2 rounded-xl py-3 text-sm font-bold transition
  disabled:pointer-events-none disabled:opacity-45 bg-[#1B2B4B] text-white hover:bg-[#121D36]
  dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]`;

function ErrorLine({ text }) {
  return (
    <p role="alert" className="flex items-center gap-2 rounded-xl bg-[#9A4B45]/10 px-4 py-3 text-xs font-medium text-[#9A4B45] dark:bg-red-500/10 dark:text-red-400">
      <AlertCircle size={14} className="shrink-0" />{text}
    </p>
  );
}
