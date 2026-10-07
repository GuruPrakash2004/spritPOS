// Expense Entry: post amounts against ledgers from Ledger Master, paid from Cash or Bank.
// Cash expenses reduce the DSR cash balance. POST /transactions/expense.
// Keyboard: Enter moves to the next field (Shift+Enter back), and on the last one adds a row; ⌘/Ctrl+S saves.

import { useEffect, useState } from 'react';
import { PlusCircle, Save, Trash2 } from 'lucide-react';
import Toast from '../common/Toast';
import Kbd from '../common/Kbd';
import { enterMovesNext, useShortcuts } from '../../lib/keys';
import { toastStore } from '../common/toastStore';
import { errorText, getLedgers, saveExpense, todayIso, } from '../../lib/api';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader } from '../../lib/styles';


const blank = () => ({ ledger: '', amount: '', narration: '' });

// Expense Entry posts amounts against ledgers from Ledger Master; cash expenses reduce the DSR cash balance.
export default function ExpenseEntry() {
  const [date, setDate] = useState(todayIso());
  const [paidFrom, setPaidFrom] = useState('CASH');
  const [ledgers, setLedgers] = useState([]);
  const [lines, setLines] = useState([blank()]);
  const [toast, setToast] = useState(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => { getLedgers().then(setLedgers).catch(e => setToast({ kind: 'error', text: errorText(e) })); }, []);

  // Cash and bank ledgers are where money is paid from, not expense heads (group names come from the shop's Ledger Group Master).
  const expenseLedgers = ledgers.filter(l => !/cash|bank/i.test(l.group));
  const filled = lines.filter(l => l.ledger);
  const total = filled.reduce((acc, l) => acc + (Number(l.amount) || 0), 0);
  const update = (i, patch) => setLines(lines.map((l, x) => (x === i ? { ...l, ...patch } : l)));

  const save = async () => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one expense line.' });
    if (filled.some(l => !(Number(l.amount) > 0))) return setToast({ kind: 'error', text: 'Every line needs an amount above zero.' });
    if (!(await toastStore.defer('ok', `Expense of ₹ ${money(total)} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveExpense({ date, paid_from: paidFrom, lines: filled.map(l => ({ ledger: l.ledger, amount: Number(l.amount), narration: l.narration })) }).then(doc => {
      setToast({ kind: 'ok', text: `Expense ${doc.doc_no} saved — ₹ ${money(doc.total)}.` });
      setLines([blank()]);
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  useShortcuts({ 'Mod+S': () => { void save(); } });

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4" onKeyDown={e => enterMovesNext(e, () => setLines(ls => [...ls, blank()]))}>
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Expense Entry</h1>
          <p className={pageSub}>Shop expenses posted against ledgers.</p>
        </div>
        <button onClick={save} className={primaryButton}><Save size={14} />Save<Kbd combo="Mod+S" /></button>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div><label htmlFor="ex-date" className={label}>Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><input id="ex-date" type="date" value={date} onChange={e => setDate(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div>
          <label htmlFor="ex-paid" className={label}>Paid From<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <select id="ex-paid" value={paidFrom} onChange={e => setPaidFrom(e.target.value )} className={field}>
            <option value="CASH">Cash</option>
            <option value="BANK">Bank</option>
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
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.amount} onChange={e => update(i, { amount: e.target.value })} className={`${field} w-28 text-right tabular-nums`} aria-label={`Amount row ${i + 1}`} /></td>
                <td className="px-3 py-1.5"><input value={l.narration} onChange={e => update(i, { narration: e.target.value })} className={`${field} w-full`} aria-label={`Narration row ${i + 1}`} /></td>
                <td className="px-3 py-1.5 text-center"><button onClick={() => setLines(lines.length > 1 ? lines.filter((_, x) => x !== i) : [blank()])} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button></td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="flex items-center justify-between border-t border-line px-3 py-2">
          <button onClick={() => setLines([...lines, blank()])} className={secondaryButton}><PlusCircle size={14} />Add Row</button>
          <span className="text-sm font-bold text-accent-ink">Total ₹ {money(total)}</span>
        </div>
      </div>
    </div>
  );
}
