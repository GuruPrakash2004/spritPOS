// Expense Entry: post amounts against ledgers from Ledger Master, paid from Cash or Bank.
// Cash expenses reduce the DSR cash balance. POST /transactions/expense.
// Keyboard: Enter moves to the next field (Shift+Enter back), and on the last one adds a row; ⌘/Ctrl+S saves.

import { useEffect, useState } from 'react';
import { PlusCircle, Save, Trash2 } from 'lucide-react';
import Toast from '../common/Toast';
import Kbd from '../common/Kbd';
import { enterMovesNext, useShortcuts } from '../../lib/keys';
import { toastStore } from '../common/toastStore';
import { useConfirm } from '../common/ConfirmDialog';
import Modal from '../common/Modal';
import { errorText, getExpenseOpening, getLedgers, saveExpense, saveExpenseOpening, todayIso, updateDocument, } from '../../lib/api';
import WithHistory from './DocHistoryPanel';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader } from '../../lib/styles';


const blank = () => ({ ledger: '', amount: '', narration: '' });

// Expense Entry posts amounts against ledgers from Ledger Master; cash expenses reduce the DSR cash balance.
export default function ExpenseEntry() {
  const [date, setDate] = useState(todayIso());
  const [paidFrom, setPaidFrom] = useState('CASH');
  const [ledgers, setLedgers] = useState([]);
  const [lines, setLines] = useState([blank()]);
  const [toast, setToast] = useState(null);
  const [editing, setEditing] = useState(null);   // a saved entry opened from the List to change (PUT keeps its number): { no }
  const [opening, setOpening] = useState(null);   // Day's OP-Balance dialog: { amount } while open
  const { confirm, dialog: confirmDialog } = useConfirm();
  // A row with something on it asks first; an empty row goes without asking.
  const removeRow = async (i) => {
    const l = lines[i];
    if ((l.ledger || l.amount || l.narration) && !(await confirm(`Are you sure you want to delete row ${i + 1}${l.amount ? ` (₹ ${l.amount})` : ''}?`))) return;
    setLines(cur => (cur.length > 1 ? cur.filter((_, x) => x !== i) : [blank()]));
  };

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

  // List → Edit Selected: the saved entry goes onto the screen. Only its expense lines are editable (receipts into the petty cash fund stay as they were).
  const loadForEdit = async (saved) => {
    if (filled.length && !(await confirm(`Open ${saved.doc_no} for editing? What is on the screen is cleared.`))) return;
    const own = (saved.lines ?? []).filter(l => Number(l.amount) > 0);
    setDate(saved.date);
    setPaidFrom(saved.paid_from || 'CASH');
    setLines(own.length ? own.map(l => ({ ledger: l.ledger, amount: String(l.amount), narration: l.narration ?? '' })) : [blank()]);
    setEditing({ no: saved.doc_no });
    setToast({ kind: 'ok', text: `Editing expense ${saved.doc_no}. Save updates it.` });
  };
  const newEntry = () => { setEditing(null); setLines([blank()]); };
  const stopEditing = async () => {
    if (filled.length && !(await confirm(`Stop editing ${editing.no}? Your changes are thrown away.`))) return;
    newEntry();
  };

  const save = async () => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one expense line.' });
    if (filled.some(l => !(Number(l.amount) > 0))) return setToast({ kind: 'error', text: 'Every line needs an amount above zero.' });
    const body = { date, paid_from: paidFrom, lines: filled.map(l => ({ ledger: l.ledger, amount: Number(l.amount), narration: l.narration })) };
    if (editing) {   // a saved entry changed in place: same number
      if (!(await toastStore.defer('ok', `Expense ${editing.no} (₹ ${money(total)}) will be updated when this closes.`, 'Update cancelled. Nothing was changed.'))) return;
      updateDocument(`/transactions/expense/${editing.no}`, body).then(doc => {
        setToast({ kind: 'ok', text: `Expense ${doc.doc_no} updated — ₹ ${money(doc.total)}.` });
        newEntry();
      }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
      return;
    }
    if (!(await toastStore.defer('ok', `Expense of ₹ ${money(total)} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveExpense({ date, paid_from: paidFrom, lines: filled.map(l => ({ ledger: l.ledger, amount: Number(l.amount), narration: l.narration })) }).then(doc => {
      setToast({ kind: 'ok', text: `Expense ${doc.doc_no} saved — ₹ ${money(doc.total)}.` });
      setLines([blank()]);
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  // Day's OP-Balance (legacy frmUtlDaysExpOPBalance): the cash in hand at the start of the chosen date.
  const openOpening = () => getExpenseOpening(date).then(r => setOpening({ amount: String(r.amount ?? 0) })).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  const saveOpening = async () => {
    const amount = Number(opening.amount);
    if (!(amount >= 0)) return;
    if (!(await toastStore.defer('ok', `Opening balance ₹ ${money(amount)} for ${date} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveExpenseOpening({ date, amount }).then(r => {
      setOpening(null);
      setToast({ kind: 'ok', text: `Opening balance for ${r.date} saved — ₹ ${money(r.amount)}.` });
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  useShortcuts({ 'Mod+S': () => { void save(); } });

  return (
    <WithHistory kind="expense" title="expenses" onNew={newEntry} onEdit={saved => void loadForEdit(saved)}>
    <div className="mx-auto flex max-w-5xl flex-col gap-4" onKeyDown={e => enterMovesNext(e, () => setLines(ls => [...ls, blank()]))}>
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {confirmDialog}
      {editing && (
        <div role="status" className="flex flex-wrap items-center justify-between gap-2 rounded-xl border border-[#D99A2B]/60 bg-[#D99A2B]/10 px-4 py-2 text-xs font-semibold text-[#1B2B4B] dark:text-[#D4A847]">
          <span>Editing expense {editing.no}: Save updates it in place.</span>
          <button type="button" onClick={() => void stopEditing()} className={secondaryButton}>Stop editing</button>
        </div>
      )}
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
        <button type="button" onClick={() => void openOpening()} className={secondaryButton}>Day's OP-Balance</button>
      </div>
      {opening && (
        <Modal title="Opening Balance (Petty Cash / Expense)" onClose={() => setOpening(null)}
          footer={<><button onClick={() => setOpening(null)} className={secondaryButton}>Cancel</button><button onClick={() => void saveOpening()} className={primaryButton}>Save</button></>}>
          <label htmlFor="ex-op" className={label}>Cash opening balance as on {date}</label>
          <input id="ex-op" type="number" min={0} step="0.01" autoFocus value={opening.amount} onChange={e => setOpening({ amount: e.target.value })}
            onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); void saveOpening(); } }} className={`${field} w-full text-right tabular-nums`} />
        </Modal>
      )}

      <div className={`${panel} overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr><th className="px-3 py-2">Sl</th><th className="px-3 py-2">Ledger</th><th className="px-3 py-2 text-right">Amount (₹)</th><th className="px-3 py-2">Narration</th></tr>
          </thead>
          <tbody className={tableBody}>
            {lines.map((l, i) => (
              <tr key={i}>
                <td className="whitespace-nowrap px-3 py-1.5 font-sans text-stone-400"><span className="inline-flex items-center gap-1.5"><button onClick={() => void removeRow(i)} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button>{i + 1}</span></td>
                <td className="px-3 py-1.5">
                  <select value={l.ledger} onChange={e => update(i, { ledger: e.target.value })} className={`${field} w-full`} aria-label={`Ledger row ${i + 1}`}>
                    <option value="">— pick a ledger —</option>
                    {expenseLedgers.map(x => <option key={x.code} value={x.code}>{x.name}</option>)}
                  </select>
                </td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.amount} onChange={e => update(i, { amount: e.target.value })} className={`${field} w-28 text-right tabular-nums`} aria-label={`Amount row ${i + 1}`} /></td>
                <td className="px-3 py-1.5"><input value={l.narration} onChange={e => update(i, { narration: e.target.value })} className={`${field} w-full`} aria-label={`Narration row ${i + 1}`} /></td>
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
    </WithHistory>
  );
}
