// SMS (the desktop's frmTrnSMS / frmUtlSendSMS and the SMS History report): Send writes one message to typed mobile numbers or to the customers
// who have a mobile number (up to 250 characters, through the shop's SMS provider); History lists the messages sent between two dates.
// Nothing here is stored by the web app: providers and sent messages are the desktop's own UtlMsgSettings / TrnSMS rows.

import { useEffect, useMemo, useState } from 'react';
import { Send } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getCustomers, getSmsHistory, getSmsProviders, sendSms, todayIso } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const MAX = 250;

function SendTab({ info }) {
  const [customers, setCustomers] = useState([]);
  const [numbers, setNumbers] = useState('');
  const [chosen, setChosen] = useState([]);   // customer codes
  const [message, setMessage] = useState('');
  const [provider, setProvider] = useState('');
  const [problem, setProblem] = useState('');
  const [find, setFind] = useState('');

  useEffect(() => { getCustomers().then(l => setCustomers(l.filter(c => c.mobile && c.code !== 'CASH'))).catch(() => {}); }, []);
  const shown = useMemo(() => {
    const t = find.trim().toLowerCase();
    return customers.filter(c => !t || c.name.toLowerCase().includes(t) || c.mobile.includes(t)).slice(0, 200);
  }, [customers, find]);

  const send = async () => {
    const typed = numbers.split(/[\s,;]+/).filter(Boolean);
    const fromCustomers = customers.filter(c => chosen.includes(c.code)).map(c => c.mobile);
    const mobiles = [...new Set([...typed, ...fromCustomers])];
    if (!mobiles.length) return setProblem('Please enter mobile number....');
    if (!message.trim()) return setProblem('Message is Empty....');
    setProblem('');
    if (!(await toastStore.defer('ok', `${mobiles.length} message${mobiles.length === 1 ? '' : 's'} will be sent when this closes.`, 'Cancelled. Nothing was sent.'))) return;
    try {
      const r = await sendSms({ mobiles, message: message.trim(), provider: provider || undefined });
      if (r.failed.length) toastStore.add('error', `${r.sent} sent, ${r.failed.length} not sent: ${r.failed[0].reason}`);
      else toastStore.add('ok', `${r.sent} message${r.sent === 1 ? '' : 's'} sent.`);
    } catch (e) { setProblem(errorText(e)); }
  };

  return (
    <div className="grid gap-4 lg:grid-cols-2">
      <div className={`${panel} space-y-3 p-4`}>
        <div>
          <label htmlFor="sms-to" className={label}>To (mobile numbers, separated by space or comma)</label>
          <textarea id="sms-to" rows={3} value={numbers} onChange={e => setNumbers(e.target.value)} className={field} />
        </div>
        {info.providers.length > 1 && (
          <div>
            <label htmlFor="sms-prov" className={label}>Provider</label>
            <select id="sms-prov" value={provider} onChange={e => setProvider(e.target.value)} className={field}>
              <option value="">Default</option>
              {info.providers.map(p => <option key={p.id}>{p.name}</option>)}
            </select>
          </div>
        )}
        <div>
          <label htmlFor="sms-msg" className={label}>Message <span className="font-normal text-stone-400">({message.length}/{MAX})</span></label>
          <textarea id="sms-msg" rows={5} maxLength={MAX} value={message} onChange={e => setMessage(e.target.value)} className={field} />
        </div>
        {problem && <InlineError message={problem} />}
        <button onClick={() => void send()} className={primaryButton}><Send size={14} />Send</button>
      </div>
      <div className={`${panel} p-4`}>
        <label htmlFor="sms-find" className={label}>Or pick customers ({chosen.length} chosen)</label>
        <input id="sms-find" value={find} onChange={e => setFind(e.target.value)} placeholder="Search name or mobile" className={`${field} mb-2`} />
        <div className="max-h-80 overflow-auto rounded-lg border border-line">
          {shown.map(c => (
            <label key={c.code} className="flex cursor-pointer items-center gap-2 border-b border-line px-3 py-1.5 text-sm last:border-0 hover:bg-subtle">
              <input type="checkbox" checked={chosen.includes(c.code)} onChange={e => setChosen(cur => (e.target.checked ? [...cur, c.code] : cur.filter(x => x !== c.code)))} />
              <span className="flex-1 truncate">{c.name}</span><span className="tabular-nums text-stone-500">{c.mobile}</span>
            </label>
          ))}
          {!shown.length && <p className="p-4 text-center text-sm text-stone-500">No customer with a mobile number.</p>}
        </div>
        <div className="mt-2 flex gap-2">
          <button onClick={() => setChosen(shown.map(c => c.code))} className={`${secondaryButton} !px-3 !py-1 text-xs`}>Select shown</button>
          <button onClick={() => setChosen([])} className={`${secondaryButton} !px-3 !py-1 text-xs`}>Clear</button>
        </div>
      </div>
    </div>
  );
}

function HistoryTab() {
  const [from, setFrom] = useState(todayIso);
  const [to, setTo] = useState(todayIso);
  const [state, setState] = useState({ key: '', rows: [], error: '' });
  const key = `${from}|${to}`;
  useEffect(() => {
    let live = true;
    getSmsHistory(from, to).then(rows => live && setState({ key, rows, error: '' })).catch(e => live && setState({ key, rows: [], error: errorText(e) }));
    return () => { live = false; };
  }, [from, to, key]);
  const loading = state.key !== key;
  return (
    <div className={`${panel} p-4`}>
      <div className="mb-3 flex flex-wrap items-center gap-2 text-sm">
        <label htmlFor="sh-from" className="font-semibold">From</label>
        <input id="sh-from" type="date" value={from} max={to} onChange={e => e.target.value && setFrom(e.target.value)} className={`${field} w-auto`} />
        <label htmlFor="sh-to" className="font-semibold">To</label>
        <input id="sh-to" type="date" value={to} min={from} max={todayIso()} onChange={e => e.target.value && setTo(e.target.value)} className={`${field} w-auto`} />
        <span className="ml-auto text-xs text-stone-500">{loading ? 'Loading…' : `${state.rows.length} message${state.rows.length === 1 ? '' : 's'}`}</span>
      </div>
      {state.error && <InlineError message={state.error} />}
      <div className="max-h-[60vh] overflow-auto">
        <table className="w-full text-sm">
          <thead className={tableHead}><tr><th className="px-3 py-2 text-left">Sent at</th><th className="px-3 py-2 text-left">Mobile</th><th className="px-3 py-2 text-left">Provider</th><th className="px-3 py-2 text-left">Message</th></tr></thead>
          <tbody className={tableBody}>
            {state.rows.map((r, i) => <tr key={i}><td className="px-3 py-1.5 tabular-nums">{r.at}</td><td className="px-3 py-1.5 tabular-nums">{r.mobile}</td><td className="px-3 py-1.5">{r.provider}</td><td className="px-3 py-1.5">{r.message}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default function SmsScreen() {
  const [tab, setTab] = useState('send');
  const [info, setInfo] = useState({ enabled: true, providers: [] });
  useEffect(() => { getSmsProviders().then(setInfo).catch(() => {}); }, []);
  const tabBtn = (id, text) => (
    <button role="tab" aria-selected={tab === id} onClick={() => setTab(id)}
      className={`rounded-md px-4 py-1.5 text-sm font-semibold ${tab === id ? 'bg-surface shadow-card' : 'text-stone-600 hover:text-stone-900 dark:text-stone-300'}`}>{text}</button>
  );
  return (
    <div className="space-y-4">
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>SMS</h1><p className={pageSub}>Send messages through the shop's SMS provider, and see what was sent.</p></div>
      </div>
      {!info.enabled && <p role="alert" className="rounded-lg border border-amber-300 bg-amber-50 px-4 py-2 text-sm text-amber-900">SMS is not switched on for this shop (UtlMisc.SMSFeature), or no provider is set up, so messages will not be sent.</p>}
      <div role="tablist" className="inline-flex gap-1 rounded-lg bg-subtle p-1">{tabBtn('send', 'Send')}{tabBtn('history', 'History')}</div>
      {tab === 'send' ? <SendTab info={info} /> : <HistoryTab />}
    </div>
  );
}
