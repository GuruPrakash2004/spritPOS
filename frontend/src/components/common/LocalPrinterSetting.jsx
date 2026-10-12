// The printer THIS PC uses for direct prints: the list comes from the PC the website is open on (every printer connected to it, USB or
// network, and its default), not from the server. A web page can't list printers, so the list is asked of the vLQMS Print Agent running
// on this PC (lib/localPrint.js, backend/tools/printagent). The choice is remembered in this browser and used by Save & Print and Print
// directly (lib/printing.js); blank = this PC's default printer. Without the agent the page says so and offers its one-time setup.

import { useEffect, useState } from 'react';
import { AlertCircle, CheckCircle2, Download, Monitor, Save } from 'lucide-react';
import { toastStore } from './toastStore';
import { downloadPrintAgent, errorText } from '../../lib/api';
import { detectAgent, PC_PREFIX, rememberPrinter, savedPrinter } from '../../lib/localPrint';
import { field as fieldClass, label as labelClass } from '../../lib/styles';

const savedName = () => { const s = savedPrinter(); return s.startsWith(PC_PREFIX) ? s.slice(PC_PREFIX.length) : ''; };

export default function LocalPrinterSetting({ inputClass = fieldClass, captionClass = labelClass, plain = false }) {
  const [agent, setAgent] = useState(null);
  const [looked, setLooked] = useState(false);
  const [value, setValue] = useState(savedName);
  const [kept, setKept] = useState(savedName);
  const [error, setError] = useState('');

  useEffect(() => { void detectAgent(true).then(a => { setAgent(a); setLooked(true); }); }, []);
  // Until the agent is found, look again every few seconds: right after its one-time setup the printers appear by themselves.
  useEffect(() => {
    if (agent) return undefined;
    const t = setInterval(() => { void detectAgent(true).then(a => { if (a) setAgent(a); }); }, 4000);
    return () => clearInterval(t);
  }, [agent]);

  const printers = agent?.printers ?? [];
  const save = async (e) => {
    e.preventDefault();
    const text = value ? `Direct prints from this PC will go to ${value} when this closes.` : "This PC's default printer will be used when this closes.";
    if (!(await toastStore.defer('ok', text, 'Cancelled. The printer is unchanged.'))) return;
    rememberPrinter(value ? `${PC_PREFIX}${value}` : '');
    setKept(value);
    toastStore.add('ok', value ? `Direct prints from this PC now go to ${value}.` : 'Direct prints from this PC use its default printer.');
  };

  const getAgentFile = async () => {
    setError('');
    try {
      const url = URL.createObjectURL(await downloadPrintAgent(window.location.origin));
      const a = document.createElement('a');
      a.href = url; a.download = 'vLQMS-Print-Agent-Setup.cmd';
      document.body.appendChild(a); a.click(); a.remove();
      setTimeout(() => URL.revokeObjectURL(url), 60_000);
      toastStore.add('ok', 'Open the downloaded Setup file once. This page finds the agent by itself.');
    } catch (err) { setError(errorText(err)); }
  };

  return (
    <form onSubmit={save} noValidate className={plain ? '' : 'rounded-2xl border border-[#DDE3EC] p-5 dark:border-[#2A2A2A]'}>
      <div className="mb-3 flex items-center gap-2">
        <Monitor size={16} className="text-[#1B2B4B] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Printer on this PC{agent?.host ? ` (${agent.host})` : ''} (direct print)</h2>
      </div>
      <label htmlFor="local-printer" className={captionClass}>Printer for direct prints from this PC</label>
      <select id="local-printer" value={value} onChange={e => setValue(e.target.value)} disabled={!agent} className={inputClass}>
        <option value="">{agent?.default ? `This PC's default printer (${agent.default})` : "This PC's default printer"}</option>
        {value && !printers.includes(value) && <option value={value}>{value}</option>}
        {printers.map(p => <option key={p} value={p}>{p}{p === agent?.default ? ' (default)' : ''}</option>)}
      </select>
      {agent && (
        <p className="mt-2 flex items-start gap-2 text-xs text-emerald-700 dark:text-emerald-400">
          <CheckCircle2 size={14} className="mt-0.5 shrink-0" />
          <span>{printers.length ? `Printers connected to this PC (USB and network): ${printers.join(', ')}` : 'The agent is running but Windows lists no printer on this PC.'}</span>
        </p>
      )}
      {looked && !agent && (
        <div className="mt-2 space-y-2 text-xs text-amber-700 dark:text-amber-400">
          <p className="flex items-start gap-2"><AlertCircle size={14} className="mt-0.5 shrink-0" />
            <span>A web page cannot see this PC's printers by itself. Install the vLQMS Print Agent on this PC once (no administrator needed): it lists every printer connected to this PC and prints to them without asking.</span></p>
          <button type="button" onClick={getAgentFile} className="inline-flex items-center gap-1.5 font-semibold text-accent-ink underline"><Download size={12} />Download Print Agent for this PC</button>
        </div>
      )}
      {error && <p role="alert" className="mt-2 flex items-center gap-2 text-xs text-red-600 dark:text-red-400"><AlertCircle size={14} className="shrink-0" />{error}</p>}
      <p className="mt-2 text-[11px] text-stone-500">Remembered in this browser, so each counter PC keeps its own printer. A printer chosen in the Print window overrides it for that print.</p>
      <button type="submit" disabled={!agent || value === kept}
        className="mt-3 flex items-center justify-center gap-2 rounded-xl bg-[#1B2B4B] px-4 py-2.5 text-xs font-bold text-white transition hover:bg-[#121D36] disabled:pointer-events-none disabled:opacity-45 dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]">
        <Save size={14} />Save printer
      </button>
    </form>
  );
}
