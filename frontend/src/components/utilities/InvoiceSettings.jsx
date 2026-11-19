// Global Settings → Other Settings → Invoice Settings (legacy frmUtlInvNumberFmt, Administrator only). Not the SMS screen: this is the exe's own
// "Invoice Settings" window, with its SMS box, saved in the desktop's UtlGSettings ('Invoice' rows) and UtlMisc so the desktop follows it too.
//
// - Prefix: None / Month wise / Day wise (then a month prefix letter for each month, with Fill Defaults styles 1 to 3), Counter Wise Reset.
// - The document number: 15 cells, each a token 1 to 8 (the help table above shows what each stands for) or any text such as "/" or "-";
//   the pink box shows the number they make (SetDocInvFmt).
// - SMS: for sales above a value, for a cancelled and for a deleted invoice, each with its message (<<COLUMN>> merge fields).
// - Invoice Print Settings: what F12, Ctrl+F12 and Alt+F12 do in the Sales Invoice (they must differ).
// - Update Rate From PB to Item Master.
// The exe's checks and messages are kept; the server repeats them.

import { useEffect, useState } from 'react';
import { Save } from 'lucide-react';
import GroupBox from '../common/GroupBox';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getInvoiceSettings, saveInvoiceSettings } from '../../lib/api';
import { CHOICES, HEAD, MONTHS, SAMPLE_KEYS, STYLES, previewNumber } from '../../lib/invoiceFormat';
import { field, label, panel, primaryButton, secondaryButton, tableHead } from '../../lib/styles';

const checkBox = (checked, on, caption) => (
  <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={checked} onChange={e => on(e.target.checked)} />{caption}</label>
);

export default function InvoiceSettings({ onClose }) {
  const [s, setS] = useState(null);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);

  // If the server can't reach the database when the tab opens, try again every few seconds (it reconnects by itself).
  const [tries, setTries] = useState(0);
  useEffect(() => {
    let live = true;
    let timer;
    getInvoiceSettings()
      .then(r => { if (live) { setS(r); setProblem(''); } })
      .catch(e => { if (live) { setProblem(errorText(e)); if (tries < 30) timer = setTimeout(() => setTries(n => n + 1), 8000); } });
    return () => { live = false; clearTimeout(timer); };
  }, [tries]);
  if (!s) return problem ? <div className="flex flex-wrap items-center gap-3"><InlineError message={`${problem} Trying again...`} /><button type="button" className={secondaryButton} onClick={() => setTries(n => n + 1)}>Try now</button></div> : null;
  const set = (patch) => { setS({ ...s, ...patch }); setProblem(''); };
  const setSms = (patch) => set({ sms: { ...s.sms, ...patch } });
  const wise = s.prefix === 'M' || s.prefix === 'D';

  // ValidFields, in the exe's order
  const mistake = () => {
    if (wise) {
      const seen = new Set();
      for (const m of s.months) {
        if (m && seen.has(m)) return `Duplicate Prefix '${m}'!!`;
        seen.add(m);
      }
      if (s.months.some(m => !m)) return 'Please fill the default Styles ';
      if (!s.format.includes('1')) return 'Please select 1 as prefix if month or daywise is selected.';
    }
    if (!s.format.some(c => ['7', '8'].includes(c.trim()))) return 'Please select 7 or 8 (the running number) in the document number.';
    if (s.f12 === s.ctrl_f12 || s.f12 === s.alt_f12 || s.ctrl_f12 === s.alt_f12) return 'Please select  distinct Invoice value';
    return '';
  };

  const save = async () => {
    const m = mistake();
    if (m) return setProblem(m);
    if (!(await toastStore.defer('ok', 'Invoice settings will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      await saveInvoiceSettings({ ...s, sms: { ...s.sms, sales_value: Number(s.sms.sales_value) || 0 } });
      toastStore.add('ok', 'Saved Successfully.');
      onClose?.();
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const sms = s.sms;
  const msgBox = (key, enabled, caption) => (
    <label className="block"><span className={label}>{caption}</span>
      <textarea rows={4} value={sms[key]} disabled={!enabled} onChange={e => setSms({ [key]: e.target.value.replaceAll("'", '') })} className={`${field} resize-none`} /></label>
  );
  const printSelect = (key, caption) => (
    <label className="grid grid-cols-[5rem_1fr] items-center gap-2 text-xs"><span className={key === 'f12' ? 'font-semibold text-red-600' : ''}>{caption}</span>
      <select value={s[key]} onChange={e => set({ [key]: e.target.value })} className={field}><option value="" />{CHOICES.map(c => <option key={c}>{c}</option>)}</select></label>
  );

  return (
    <section className={`${panel} space-y-4 p-4`} aria-label="Invoice settings">
      <div className="grid gap-4 lg:grid-cols-[220px_minmax(0,1fr)]">
        <div className="flex flex-col gap-4">
          <GroupBox title="Prefix">
            <div className="space-y-2" role="radiogroup" aria-label="Prefix">
              {[['', 'None'], ['M', 'Month wise'], ['D', 'Day wise']].map(([v, c]) => (
                <label key={c} className="flex items-center gap-2 text-xs"><input type="radio" name="inv-prefix" checked={s.prefix === v} onChange={() => set({ prefix: v })} />{c}</label>
              ))}
              {checkBox(s.counter_reset, v => set({ counter_reset: v }), 'Counter Wise Reset')}
            </div>
          </GroupBox>
          {wise && (
            <GroupBox title="Month prefix">
              <div className="mb-2 flex flex-wrap gap-1" aria-label="Fill defaults">
                {Object.keys(STYLES).map(n => <button key={n} type="button" className={`${secondaryButton} !px-2 !py-1 text-[11px]`} onClick={() => set({ months: [...STYLES[n]] })}>{n}</button>)}
              </div>
              <table className="w-full text-xs"><thead><tr><th className={tableHead}>Month</th><th className={tableHead}>Prefix</th></tr></thead>
                <tbody>{MONTHS.map((m, i) => (
                  <tr key={m}><td className="px-2 py-0.5">{m}</td>
                    <td className="p-0.5"><input aria-label={`${m} prefix`} maxLength={1} value={s.months[i]} className={`${field} text-center uppercase`}
                      onChange={e => set({ months: s.months.map((x, j) => (j === i ? e.target.value.toUpperCase().slice(0, 1) : x)) })} /></td></tr>
                ))}</tbody></table>
            </GroupBox>
          )}
          <GroupBox title="Invoice Print Settings" className="lg:mt-auto">
            <div className="space-y-2">{printSelect('f12', 'F12')}{printSelect('ctrl_f12', 'Ctrl+F12')}{printSelect('alt_f12', 'Alt+F12')}</div>
          </GroupBox>
        </div>

        <div className="min-w-0 space-y-3">
          <div className="overflow-x-auto"><table className="text-xs" aria-label="What each number stands for">
            <thead><tr>{HEAD.map((h, i) => <th key={h} className={tableHead} title={`Number ${i + 1}`}>{i + 1}<br />{h}</th>)}</tr></thead>
            <tbody><tr>{SAMPLE_KEYS.map(k => <td key={k} className="border border-[#E3D9CD] px-3 py-1 text-center dark:border-[#2A2A2A]">{s.sample?.[k]}</td>)}</tr></tbody>
          </table></div>
          <div className="space-y-0.5 text-xs text-stone-600 dark:text-stone-300">
            <p>Please enter the no [ 1- 8 ] with separator to form the document number</p>
            <p>If we select from 1 to 8, then above prefix will be replaced and other than that, it is same prefix as used in Document Number.</p>
            <p>Eg: 1/JAN/0001</p>
          </div>
          <div className="flex flex-wrap gap-1" role="group" aria-label="Document number format">
            {s.format.map((c, i) => (
              <input key={i} aria-label={`Cell ${i + 1}`} value={c} maxLength={5} onChange={e => set({ format: s.format.map((x, j) => (j === i ? e.target.value : x)) })}
                className={`${field} !w-12 text-center font-mono`} />
            ))}
          </div>
          <label className="block"><span className={label}>Document Number</span>
            <input readOnly aria-label="Document Number" value={previewNumber(s.format, s.sample)} className={`${field} bg-[#FBD5FB] font-mono text-lg font-bold dark:!bg-[#3a2a3a]`} /></label>
        </div>
      </div>

      <GroupBox title="SMS">
        {!s.sms_available && <p className="mb-2 text-[11px] text-stone-500">This database has no invoice SMS columns (UtlMisc.InvSMS*); they are not saved.</p>}
        <div className="grid gap-4 md:grid-cols-3">
          <div className="space-y-2">
            {checkBox(sms.on_sales, v => setSms(v ? { on_sales: true } : { on_sales: false, sales_msg: '', sales_value: '' }), 'For Sales Values')}
            <label className="grid grid-cols-[4rem_1fr] items-center gap-2 text-xs"><span>Value &gt;</span>
              <input aria-label="Value" inputMode="numeric" value={sms.sales_value} disabled={!sms.on_sales} onChange={e => setSms({ sales_value: e.target.value.replace(/\D/g, '') })} className={`${field} text-right`} /></label>
            {msgBox('sales_msg', sms.on_sales, 'Message')}
          </div>
          <div className="space-y-2">
            {checkBox(sms.on_cancel, v => setSms(v ? { on_cancel: true } : { on_cancel: false, cancel_msg: '' }), 'For Cancellation')}
            {msgBox('cancel_msg', sms.on_cancel, 'Message')}
          </div>
          <div className="space-y-2">
            {checkBox(sms.on_delete, v => setSms(v ? { on_delete: true } : { on_delete: false, delete_msg: '' }), 'For Deletion')}
            {msgBox('delete_msg', sms.on_delete, 'Message')}
          </div>
        </div>
      </GroupBox>

      {checkBox(s.update_rate, v => set({ update_rate: v }), 'Update Rate From PB to Item Master')}
      {problem && <InlineError message={problem} />}
      <div className="flex flex-wrap gap-2">
        <button className={primaryButton} disabled={busy} onClick={() => void save()}><Save size={14} />Save</button>
        {onClose && <button className={secondaryButton} onClick={onClose}>Close</button>}
      </div>
    </section>
  );
}
