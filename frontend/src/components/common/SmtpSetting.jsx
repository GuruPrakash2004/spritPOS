// The e-mail (SMTP) settings used to e-mail reports (backend SMTP_* in .env, routers/setup.py /setup/smtp). Shown on the
// sign-in page's settings and in Print Settings, so the server, user and password can be typed here instead of editing
// .env by hand. The password is encrypted in the browser, kept on the server only sealed, and never shown again: leave
// it blank to keep the saved one. "Test connection" connects and signs in without sending anything. A blank server turns
// e-mail off. Only an Administrator (or a request from the server PC itself) can change it.

import { useEffect, useState } from 'react';
import { AlertCircle, CheckCircle2, Loader2, Mail, Save, Plug } from 'lucide-react';
import { toastStore } from './toastStore';
import { errorText, getSmtp, saveSmtp, testSmtp } from '../../lib/api';
import { encryptSecret } from '../../lib/secureField';
import { field as fieldClass, label as labelClass } from '../../lib/styles';

const BLANK = { host: '', port: '587', security: 'starttls', user: '', from_address: '', password: '' };
const PORTS = { starttls: '587', ssl: '465', none: '25' };

export default function SmtpSetting({ inputClass = fieldClass, captionClass = labelClass, plain = false }) {
  const [saved, setSaved] = useState(null);
  const [form, setForm] = useState(BLANK);
  const [busy, setBusy] = useState('');
  const [error, setError] = useState('');
  const [errors, setErrors] = useState({});
  const [result, setResult] = useState('');

  const load = (info) => {
    setSaved(info);
    setForm({ host: info.host, port: info.port, security: info.security, user: info.user, from_address: info.from_address, password: '' });
  };
  useEffect(() => { getSmtp().then(load).catch(e => setError(errorText(e))); }, []);

  const set = (key, value) => {
    setForm(f => ({ ...f, [key]: value }));
    setError(''); setResult('');
    if (errors[key]) setErrors(prev => { const n = { ...prev }; delete n[key]; return n; });
  };
  const chooseSecurity = (security) => setForm(f => ({ ...f, security, port: !f.port || Object.values(PORTS).includes(f.port) ? PORTS[security] : f.port }));

  // Mistakes are shown beside the box (not as popups).
  const check = () => {
    const found = {};
    if (form.host.trim()) {
      const port = Number(form.port);
      if (!/^\d+$/.test(form.port) || port < 1 || port > 65535) found.port = 'Port must be a number from 1 to 65535.';
      if (form.from_address.trim() && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.from_address.trim())) found.from_address = 'From address is not a valid e-mail address.';
    }
    setErrors(found);
    return Object.keys(found).length === 0;
  };

  const body = async () => ({
    host: form.host.trim(), port: form.port.trim() || '587', security: form.security, user: form.user.trim(),
    from_address: form.from_address.trim(), password: await encryptSecret(form.password, 'smtp'),
  });

  const test = async () => {
    if (!form.host.trim()) { setErrors({ host: 'Enter the SMTP server.' }); return; }
    if (!check()) return;
    setBusy('test'); setError(''); setResult('');
    try { setResult((await testSmtp(await body())).message); } catch (e) { setError(errorText(e)); } finally { setBusy(''); }
  };

  const save = async (e) => {
    e.preventDefault();
    if (!check()) return;
    const text = form.host.trim() ? 'The e-mail settings will be saved when this closes.' : 'E-mail will be turned off when this closes.';
    if (!(await toastStore.defer('ok', text, 'Cancelled. The e-mail settings are unchanged.'))) return;
    setBusy('save'); setError(''); setResult('');
    try {
      load(await saveSmtp(await body()));
      toastStore.add('ok', form.host.trim() ? 'E-mail settings saved.' : 'E-mail turned off.');
    } catch (err) { setError(errorText(err)); } finally { setBusy(''); }
  };

  const changed = saved != null && (form.password !== '' || ['host', 'port', 'security', 'user', 'from_address'].some(k => form[k] !== saved[k]));
  const bad = (k) => (errors[k] ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : '');
  const err = (k) => errors[k] && <p id={`smtp-${k}-err`} className="mt-1 flex items-center gap-1.5 text-xs text-red-600 dark:text-red-400"><AlertCircle size={12} className="shrink-0" />{errors[k]}</p>;
  return (
    <form onSubmit={save} noValidate className={plain ? '' : 'rounded-2xl border border-[#E3D9CD] p-5 dark:border-[#2A2A2A]'}>
      <div className="mb-3 flex items-center gap-2">
        <Mail size={16} className="text-[#64283A] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Email (SMTP) settings</h2>
        {saved && <span className={`ml-auto text-[11px] font-semibold ${saved.configured ? 'text-emerald-700 dark:text-emerald-400' : 'text-stone-500'}`}>{saved.configured ? 'On' : 'Off'}</span>}
      </div>
      <div className="grid gap-3 sm:grid-cols-2">
        <div className="sm:col-span-2">
          <label htmlFor="smtp-host" className={captionClass}>SMTP server</label>
          <input id="smtp-host" value={form.host} onChange={e => set('host', e.target.value)} spellCheck={false} autoComplete="off"
            placeholder="e.g. smtp.gmail.com (blank = e-mail off)" className={`${inputClass} ${bad('host')}`} aria-invalid={!!errors.host} />
          {err('host')}
        </div>
        <div>
          <label htmlFor="smtp-security" className={captionClass}>Security</label>
          <select id="smtp-security" value={form.security} onChange={e => chooseSecurity(e.target.value)} className={inputClass}>
            <option value="starttls">STARTTLS (587)</option>
            <option value="ssl">SSL / TLS (465)</option>
            <option value="none">None (25)</option>
          </select>
        </div>
        <div>
          <label htmlFor="smtp-port" className={captionClass}>Port</label>
          <input id="smtp-port" value={form.port} onChange={e => set('port', e.target.value.replace(/\D/g, '').slice(0, 5))} inputMode="numeric"
            className={`${inputClass} ${bad('port')}`} aria-invalid={!!errors.port} />
          {err('port')}
        </div>
        <div>
          <label htmlFor="smtp-user" className={captionClass}>User name</label>
          <input id="smtp-user" value={form.user} onChange={e => set('user', e.target.value)} spellCheck={false} autoComplete="off"
            placeholder="the mailbox, e.g. shop@gmail.com" className={inputClass} />
        </div>
        <div>
          <label htmlFor="smtp-password" className={captionClass}>Password</label>
          <input id="smtp-password" type="password" value={form.password} onChange={e => set('password', e.target.value)} autoComplete="new-password"
            placeholder={saved?.has_password ? 'saved · leave blank to keep' : 'app password'} className={inputClass} />
        </div>
        <div className="sm:col-span-2">
          <label htmlFor="smtp-from" className={captionClass}>From address (optional)</label>
          <input id="smtp-from" value={form.from_address} onChange={e => set('from_address', e.target.value)} spellCheck={false} autoComplete="off"
            placeholder="Blank = the Company Master e-mail" className={`${inputClass} ${bad('from_address')}`} aria-invalid={!!errors.from_address} />
          {err('from_address')}
        </div>
      </div>
      {result && !error && <p role="status" className="mt-3 flex items-center gap-2 text-xs text-emerald-700 dark:text-emerald-400"><CheckCircle2 size={14} className="shrink-0" />{result}</p>}
      {error && <p role="alert" className="mt-3 flex items-center gap-2 text-xs text-red-600 dark:text-red-400"><AlertCircle size={14} className="shrink-0" />{error}</p>}
      <div className="mt-3 flex flex-wrap gap-2">
        <button type="button" onClick={test} disabled={!!busy}
          className="flex items-center justify-center gap-2 rounded-xl border border-[#DCD1C5] px-4 py-2.5 text-xs font-bold transition-all hover:bg-[#F3EEE6] disabled:pointer-events-none disabled:opacity-45 dark:border-[#2A2A2A] dark:hover:bg-[#1C1C1C]">
          {busy === 'test' ? <Loader2 size={14} className="animate-spin" /> : <Plug size={14} />}Test connection
        </button>
        <button type="submit" disabled={!!busy || !changed}
          className="flex items-center justify-center gap-2 rounded-xl bg-[#64283A] px-4 py-2.5 text-xs font-bold text-white transition-all hover:bg-[#521E2E] disabled:pointer-events-none disabled:opacity-45 dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]">
          {busy === 'save' ? <Loader2 size={14} className="animate-spin" /> : <Save size={14} />}Save email settings
        </button>
      </div>
    </form>
  );
}
