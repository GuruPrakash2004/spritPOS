import { useEffect, useState } from 'react';
import { PlusCircle, Save, Trash2 } from 'lucide-react';
import Toast from './Toast';
import { api, errorText, todayIso, type ApiLedger } from './api';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from './ui';

type Line = { ledger: string; amount: string; narration: string };
const blank = (): Line => ({ ledger: '', amount: '', narration: '' });

// Expense Entry posts amounts against ledgers from Ledger Master; cash expenses reduce the DSR cash balance.
export default function ExpenseEntry() {
  const [date, setDate] = useState(todayIso());
  const [paidFrom, setPaidFrom] = useState('CASH');
  const [ledgers, setLedgers] = useState<ApiLedger[]>([]);
  const [lines, setLines] = useState<Line[]>([blank()]);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => { api<ApiLedger[]>('/masters/ledgers').then(setLedgers).catch(e => setToast({ kind: 'error', text: errorText(e) })); }, []);

  const expenseLedgers = ledgers.filter(l => l.group !== 'Cash-in-hand' && l.group !== 'Bank Accounts');
  const payLedgers = ledgers.filter(l => l.group === 'Cash-in-hand' || l.group === 'Bank Accounts');
  const filled = lines.filter(l => l.ledger);
  const total = filled.reduce((acc, l) => acc + (Number(l.amount) || 0), 0);
  const update = (i: number, patch: Partial<Line>) => setLines(lines.map((l, x) => (x === i ? { ...l, ...patch } : l)));

  const save = () => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one expense line.' });
    if (filled.some(l => !(Number(l.amount) > 0))) return setToast({ kind: 'error', text: 'Every line needs an amount above zero.' });
    api<{ doc_no: string; total: number }>('/transactions/expense', {
      method: 'POST',
      body: { date, paid_from: paidFrom, lines: filled.map(l => ({ ledger: l.ledger, amount: Number(l.amount), narration: l.narration })) },
    }).then(doc => {
      setToast({ kind: 'ok', text: `Expense ${doc.doc_no} saved — ₹ ${money(doc.total)}.` });
      setLines([blank()]);
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={`${panel} flex flex-wrap items-center justify-between gap-3 px-5 py-3`}>
        <div>
          <h2 className={pageTitle}>Expense Entry</h2>
          <p className={pageSub}>Shop expenses posted against ledgers.</p>
        </div>
        <button onClick={save} className={primaryButton}><Save size={14} />Save</button>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div><label htmlFor="ex-date" className={`${label} !text-red-600 dark:!text-red-400`}>Date</label><input id="ex-date" type="date" value={date} onChange={e => setDate(e.target.value)} className={`${field} font-mono`} /></div>
        <div>
          <label htmlFor="ex-paid" className={`${label} !text-red-600 dark:!text-red-400`}>Paid From</label>
          <select id="ex-paid" value={paidFrom} onChange={e => setPaidFrom(e.target.value)} className={field}>
            {payLedgers.map(l => <option key={l.code} value={l.code}>{l.name}</option>)}
          </select>
        </div>
      </div>

      <div className={`${panel} overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr><th className="px-3 py-2">Sl</th><th className="px-3 py-2">Ledger</th><th className="px-3 py-2 text-right">Amount (₹)</th><th className="px-3 py-2">Narration</th><th className="px-3 py-2" /></tr>
          </thead>
          <tbody className={tableBody}>
            {lines.map((l, i) => (
              <tr key={i}>
                <td className="px-3 py-1.5 text-stone-400">{i + 1}</td>
                <td className="px-3 py-1.5">
                  <select value={l.ledger} onChange={e => update(i, { ledger: e.target.value })} className={`${field} w-full`} aria-label={`Ledger row ${i + 1}`}>
                    <option value="">— pick a ledger —</option>
                    {expenseLedgers.map(x => <option key={x.code} value={x.code}>{x.name}</option>)}
                  </select>
                </td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.amount} onChange={e => update(i, { amount: e.target.value })} className={`${field} w-28 text-right font-mono`} aria-label={`Amount row ${i + 1}`} /></td>
                <td className="px-3 py-1.5"><input value={l.narration} onChange={e => update(i, { narration: e.target.value })} className={`${field} w-full`} aria-label={`Narration row ${i + 1}`} /></td>
                <td className="px-3 py-1.5 text-center"><button onClick={() => setLines(lines.length > 1 ? lines.filter((_, x) => x !== i) : [blank()])} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button></td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="flex items-center justify-between border-t border-[#e1c9b6] px-3 py-2 dark:border-[#343b59]">
          <button onClick={() => setLines([...lines, blank()])} className={secondaryButton}><PlusCircle size={14} />Add Row</button>
          <span className="text-sm font-bold text-[#b84716] dark:text-[#ffb07e]">Total ₹ {money(total)}</span>
        </div>
      </div>
    </div>
  );
}
