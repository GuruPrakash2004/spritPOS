// The printer every direct print uses ON THIS SERVER (backend DIRECT_PRINTER in .env, routers/setup.py /setup/print-printer).
// The printer saved with each print option (Print Settings below; the desktop shares it) can be a name that only exists in
// another session, e.g. "POS-80 on saturn2 (redirected 4)" saved from a Remote Desktop session, which the server never has.
// Pick here one of the printers the server can really see and every direct print goes to it; blank = each document's own
// printer (the server also matches a saved name that differs only by " on host (redirected N)" or \\host\ in front).
// Only an Administrator (or a request from the server PC itself) can change it.

import { useEffect, useState } from 'react';
import { AlertCircle, CheckCircle2, Loader2, Printer, Save } from 'lucide-react';
import { toastStore } from './toastStore';
import { errorText, getPrintPrinter, savePrintPrinter } from '../../lib/api';
import { field as fieldClass, label as labelClass } from '../../lib/styles';

export default function ServerPrinterSetting({ inputClass = fieldClass, captionClass = labelClass, plain = false }) {
  const [info, setInfo] = useState(null);
  const [value, setValue] = useState('');
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    getPrintPrinter().then(i => { setInfo(i); setValue(i.printer); }).catch(e => setError(errorText(e)));
  }, []);

  const save = async (e) => {
    e.preventDefault();
    const printer = value.trim();
    const text = printer ? `Direct prints will go to ${printer} when this closes.` : 'Each document will use its own saved printer when this closes.';
    if (!(await toastStore.defer('ok', text, 'Cancelled. The printer is unchanged.'))) return;
    setBusy(true); setError('');
    try {
      const i = await savePrintPrinter(printer);
      setInfo(i); setValue(i.printer);
      toastStore.add('ok', i.printer ? `Direct prints now go to ${i.printer}.` : 'Each document uses its own saved printer.');
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const changed = info != null && value.trim() !== info.printer;
  const installed = info?.installed ?? [];
  const refused = info?.refused ?? [];
  return (
    <form onSubmit={save} noValidate className={plain ? '' : 'rounded-2xl border border-[#DDE3EC] p-5 dark:border-[#2A2A2A]'}>
      <div className="mb-3 flex items-center gap-2">
        <Printer size={16} className="text-[#1B2B4B] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Printer on this server (direct print)</h2>
      </div>
      <label htmlFor="server-printer" className={captionClass}>Printer for every direct print</label>
      <input id="server-printer" list="server-printers" value={value} onChange={e => { setValue(e.target.value); setError(''); }} spellCheck={false}
        placeholder="Blank = each document's own printer" className={inputClass} aria-invalid={!!error} />
      <datalist id="server-printers">{installed.map(p => <option key={p} value={p} />)}</datalist>
      {info && !error && (
        <p className={`mt-2 flex items-start gap-2 text-xs ${installed.length ? 'text-emerald-700 dark:text-emerald-400' : 'text-amber-700 dark:text-amber-400'}`}>
          {installed.length ? <CheckCircle2 size={14} className="mt-0.5 shrink-0" /> : <AlertCircle size={14} className="mt-0.5 shrink-0" />}
          <span>{installed.length ? `Printers this service can print to: ${installed.join(', ')}` : 'This service can print to no printer (or the Crystal bridge is not running). Add the printer for the Windows user the service runs as.'}</span>
        </p>
      )}
      {info && !error && (
        <div className="mt-2 space-y-1 text-[11px] text-stone-600 dark:text-stone-300">
          <p>The service runs as Windows user <span className="font-mono font-semibold">{info.runs_as || '?'}</span>{info.runs_as?.endsWith('$') ? ' (LocalSystem: it has no printers of its own; run the service as a real user)' : ''}.</p>
          {refused.length > 0 && (
            <div>
              <p className="font-semibold text-red-700 dark:text-red-400">Listed by Windows but this service cannot open them:</p>
              <ul className="mt-0.5 list-disc pl-5">{refused.map(r => <li key={r.name}><span className="font-mono">{r.name}</span> — {r.why}</li>)}</ul>
            </div>
          )}
        </div>
      )}
      {error && <p role="alert" className="mt-2 flex items-center gap-2 text-xs text-red-600 dark:text-red-400"><AlertCircle size={14} className="shrink-0" />{error}</p>}
      <button type="submit" disabled={busy || !changed}
        className="mt-3 flex items-center justify-center gap-2 rounded-xl bg-[#1B2B4B] px-4 py-2.5 text-xs font-bold text-white transition hover:bg-[#121D36] disabled:pointer-events-none disabled:opacity-45 dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]">
        {busy ? <Loader2 size={14} className="animate-spin" /> : <Save size={14} />}Save printer
      </button>
    </form>
  );
}
