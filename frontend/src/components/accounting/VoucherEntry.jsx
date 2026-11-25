// Voucher entry (legacy frmTrnVch): Payment, Receipt, Credit Note, Debit Note, Contra and Journal Voucher share this screen.
// Pick a ledger and a Dr or Cr amount on each line; the voucher can only be saved when the debit and credit totals match. Saving writes the
// voucher and posts it to the ledgers in the shop's own tables (POST /accounting/vouchers/<kind>). Below the form: the last 30 days' vouchers.
// Keyboard: Enter moves to the next field (Shift+Enter back, and on the last field adds a row); Alt+A adds a row; ⌘/Ctrl+S saves.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { PlusCircle, Save, Trash2 } from 'lucide-react';
import ActionBar, { ScreenFrame } from '../common/ActionBar';
import ListModal from '../common/ListModal';
import Kbd from '../common/Kbd';
import Toast from '../common/Toast';
import { InlineError } from '../common/FormErrorSummary';
import { toastStore } from '../common/toastStore';
import { useConfirm } from '../common/ConfirmDialog';
import { enterMovesNext, useShortcuts } from '../../lib/keys';
import { goHome } from '../../lib/nav';
import { canDo, cancelVoucher, errorText, getAccountingLedgers, getVoucher, listVouchers, saveVoucher, todayIso, updateVoucher } from '../../lib/api';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import DateInput from '../common/DateInput';

const blank = (side = 'Dr') => ({ ledger_id: '', side, amount: '' });
const days = (n) => { const d = new Date(); d.setDate(d.getDate() - n); return d.toISOString().slice(0, 10); };
const HAS_CHEQUE = new Set(['payment', 'receipt']);
const HINT = {
  payment: 'Debit the party or expense you paid, credit the cash or bank account it was paid from.',
  receipt: 'Debit the cash or bank account that received the money, credit the party it came from.',
  'credit-note': 'A credit note raised on a party.', 'debit-note': 'A debit note raised on a party.',
  contra: 'Move money between cash and bank accounts.', journal: 'Adjust accounts without cash: debits must equal credits.',
};

export default function VoucherEntry({ kind, title }) {
  const [date, setDate] = useState(todayIso());
  const [narration, setNarration] = useState('');
  const [chequeNo, setChequeNo] = useState('');
  const [chequeDate, setChequeDate] = useState('');
  const [lines, setLines] = useState([blank('Dr'), blank('Cr')]);
  const [ledgers, setLedgers] = useState([]);
  const [recent, setRecent] = useState([]);
  const [open, setOpen] = useState(null);       // a recent voucher shown in full
  const [editing, setEditing] = useState(null); // the voucher being edited (its id and number), or null for a new one
  const [problem, setProblem] = useState('');
  const [toast, setToast] = useState(null);
  const [listOpen, setListOpen] = useState(false);   // the bar's List window
  const [all, setAll] = useState(null);   // the last year, for the List window
  const { confirm, dialog } = useConfirm();

  useEffect(() => { if (toast) { const t = setTimeout(() => setToast(null), 3000); return () => clearTimeout(t); } }, [toast]);
  useEffect(() => { getAccountingLedgers().then(setLedgers).catch(e => setProblem(errorText(e))); }, []);
  const reload = useCallback(() => listVouchers(kind, days(30), todayIso()).then(setRecent).catch(e => setProblem(errorText(e))), [kind]);
  useEffect(() => { void reload(); }, [reload]);

  const totals = useMemo(() => {
    const sum = (side) => Math.round(lines.filter(l => l.side === side).reduce((a, l) => a + (Number(l.amount) || 0), 0) * 100) / 100;
    return { dr: sum('Dr'), cr: sum('Cr') };
  }, [lines]);
  const update = (i, patch) => { setProblem(''); setLines(ls => ls.map((l, x) => (x === i ? { ...l, ...patch } : l))); };
  const addRow = () => setLines(ls => [...ls, blank(totals.dr > totals.cr ? 'Cr' : 'Dr')]);
  const removeRow = async (i) => {
    const l = lines[i];
    if ((l.ledger_id || l.amount) && !(await confirm(`Are you sure you want to delete row ${i + 1}?`))) return;
    setLines(ls => (ls.length > 2 ? ls.filter((_, x) => x !== i) : [blank(ls[i].side)]));
  };

  const save = async () => {
    const filled = lines.filter(l => l.ledger_id && Number(l.amount) > 0);
    if (lines.some(l => (l.ledger_id && !(Number(l.amount) > 0)) || (!l.ledger_id && Number(l.amount) > 0))) return setProblem('Every line needs a ledger and an amount above zero.');
    if (filled.length < 2 || !totals.dr || !totals.cr) return setProblem('Enter at least one debit and one credit.');
    if (totals.dr !== totals.cr) return setProblem('Debit and Credit totals are NOT matching!');
    setProblem('');
    const body = {
      date, narration, lines: filled.map(l => ({ ledger_id: Number(l.ledger_id), side: l.side, amount: Number(l.amount) })),
      ...(HAS_CHEQUE.has(kind) && chequeNo ? { cheque_no: chequeNo, cheque_date: chequeDate || null } : {}),
    };
    if (!(await toastStore.defer('ok', `${title}${editing ? ` ${editing.doc_no}` : ''} of ₹ ${money(totals.dr)} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    (editing ? updateVoucher(kind, editing.id, body) : saveVoucher(kind, body)).then(v => {
      setToast({ kind: 'ok', text: `${title} ${v.doc_no} saved — ₹ ${money(v.total)}.` });
      resetForm(); setOpen(null);
      void reload();
    }).catch(e => setProblem(errorText(e)));
  };
  const resetForm = () => { setEditing(null); setDate(todayIso()); setLines([blank('Dr'), blank('Cr')]); setNarration(''); setChequeNo(''); setChequeDate(''); setProblem(''); };
  const startEdit = (v) => {   // load a saved voucher into the form; saving then rewrites it (the number stays)
    setEditing({ id: v.id, doc_no: v.doc_no }); setDate(v.date); setNarration(v.narration || ''); setChequeNo(v.cheque_no || ''); setChequeDate(v.cheque_date || '');
    setLines(v.lines.map(l => ({ ledger_id: String(l.ledger_id), side: l.side, amount: String(l.amount) }))); setProblem('');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };
  const cancelDoc = async (v) => {
    if (!(await confirm(`Are you sure to CANCEL ${title} ${v.doc_no}?`))) return;
    if (!(await toastStore.defer('delete', `${title} ${v.doc_no} will be cancelled when this closes.`, 'Cancel stopped. The voucher is unchanged.'))) return;
    cancelVoucher(kind, v.id).then(done => {
      setToast({ kind: 'ok', text: `${title} ${done.doc_no} cancelled.` });
      if (editing?.id === v.id) resetForm();
      setOpen(done); void reload();
    }).catch(e => setProblem(errorText(e)));
  };
  const loadAll = () => { setAll(null); ((a, b) => listVouchers(kind, a, b))(days(365), todayIso()).then(setAll).catch(e => { setAll([]); setProblem(errorText(e)); }); };
  const walk = [...recent].reverse();   // oldest first, for First / Previous / Next / Last
  const at = open ? walk.findIndex(v => v.id === open.id) : -1;
  const goTo = (where) => {
    if (!walk.length) return;
    const to = where === 'first' ? 0 : where === 'last' ? walk.length - 1 : where === 'next' ? Math.min(walk.length - 1, at + 1) : Math.max(0, at < 0 ? walk.length - 1 : at - 1);
    ((id) => getVoucher(kind, id))(walk[to].id).then(setOpen).catch(e => setProblem(errorText(e)));
  };
  const usable = !!open && open.status !== 'CANCELLED';
  const bar = (
    <ActionBar
      mode={editing ? 'Edit' : 'View'} onNew={canDo(title, 'add') ? resetForm : undefined}
      onEdit={canDo(title, 'edit') ? () => open && startEdit(open) : undefined} canEdit={usable} noEditReason="Open a saved one from the list first (a cancelled one can't be edited)"
      onDelete={canDo(title, 'delete') ? () => open && void cancelDoc(open) : undefined} canDelete={usable} deleteLabel="Cancel Voucher" noDeleteReason="Open a saved one from the list first (it must not be cancelled already)"
      onSave={() => void save()} onCancel={resetForm} onList={() => { setListOpen(true); loadAll(); }} onClose={goHome}
      nav={{ onFirst: () => goTo('first'), onPrev: () => goTo('prev'), onNext: () => goTo('next'), onLast: () => goTo('last'), onFind: () => { setListOpen(true); loadAll(); },
        atStart: false, atEnd: false, label: at >= 0 ? `${at + 1} / ${walk.length}` : '' }}
    />
  );
  useShortcuts({ 'Mod+S': () => { void save(); }, 'Alt+A': addRow });

  return (
    <ScreenFrame bar={bar}>
    <div className="mx-auto flex max-w-5xl flex-col gap-4" onKeyDown={e => enterMovesNext(e, addRow)}>
      {listOpen && (
        <ListModal title={title} subtitle="Listing of the last 365 days" columns={[{ key: 'doc_no', label: 'Voucher No' }, { key: 'date', label: 'Date' }, { key: 'narration', label: 'Narration' }, { key: 'total', label: 'Amount', kind: 'money' }, { key: 'status', label: 'Status' }, { key: 'added_by', label: 'Added By' }]} defaultKeys={['doc_no', 'date', 'narration', 'total', 'status']}
          rows={all ?? []} rowKey={r => r.id} loading={all === null} storageKey={`voucher-${kind}`} exportName={title} onRefresh={loadAll}
          onOpen={r => ((id) => getVoucher(kind, id))(r.id).then(setOpen).catch(e => setProblem(errorText(e)))} onClose={() => setListOpen(false)} />
      )}
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>{title}</h1><p className={pageSub}>{HINT[kind]}</p></div>
        <div className="flex gap-2">
          {editing && <button onClick={resetForm} className={secondaryButton}>Stop editing</button>}
          <button onClick={() => void save()} className={primaryButton}><Save size={14} />{editing ? 'Save changes' : 'Save'}<Kbd combo="Mod+S" /></button>
        </div>
      </div>
      {editing && <p className="rounded-md border border-line px-3 py-2 text-xs font-semibold text-accent-ink">Editing {title} {editing.doc_no}: the number stays; saving replaces its lines and its ledger postings.</p>}

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div><label htmlFor="v-date" className={label}>Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><DateInput id="v-date" value={date} onChange={e => setDate(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div className="min-w-60 flex-1"><label htmlFor="v-narr" className={label}>Narration</label><input id="v-narr" value={narration} maxLength={250} onChange={e => setNarration(e.target.value)} className={`${field} w-full`} /></div>
        {HAS_CHEQUE.has(kind) && (<>
          <div><label htmlFor="v-chq" className={label}>Cheque No</label><input id="v-chq" value={chequeNo} maxLength={10} onChange={e => setChequeNo(e.target.value)} className={`${field} w-32`} /></div>
          <div><label htmlFor="v-chqd" className={label}>Cheque Date</label><DateInput id="v-chqd" value={chequeDate} onChange={e => setChequeDate(e.target.value)} className={`${field} tabular-nums`} /></div>
        </>)}
      </div>

      <div className={`${panel} overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Sl</th><th className="px-3 py-2">Ledger</th><th className="px-3 py-2">Dr / Cr</th><th className="px-3 py-2 text-right">Amount (₹)</th></tr></thead>
          <tbody className={tableBody}>
            {lines.map((l, i) => (
              <tr key={i}>
                <td className="whitespace-nowrap px-3 py-1.5 text-stone-400"><span className="inline-flex items-center gap-1.5"><button onClick={() => void removeRow(i)} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button>{i + 1}</span></td>
                <td className="px-3 py-1.5">
                  <select value={l.ledger_id} onChange={e => update(i, { ledger_id: e.target.value })} className={`${field} w-full`} aria-label={`Ledger row ${i + 1}`}>
                    <option value="">— pick a ledger —</option>
                    {ledgers.map(x => <option key={x.id} value={x.id}>{x.name}{x.group ? ` — ${x.group}` : ''}</option>)}
                  </select>
                </td>
                <td className="px-3 py-1.5">
                  <select value={l.side} onChange={e => update(i, { side: e.target.value })} className={field} aria-label={`Dr or Cr row ${i + 1}`}><option>Dr</option><option>Cr</option></select>
                </td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.amount} onChange={e => update(i, { amount: e.target.value })} className={`${field} w-32 text-right tabular-nums`} aria-label={`Amount row ${i + 1}`} /></td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="flex flex-wrap items-center justify-between gap-3 border-t border-line px-3 py-2">
          <button onClick={addRow} className={secondaryButton}><PlusCircle size={14} />Add Row<Kbd combo="Alt+A" /></button>
          <span className="text-sm font-bold text-accent-ink">Dr ₹ {money(totals.dr)} · Cr ₹ {money(totals.cr)}{totals.dr !== totals.cr && <span className="ml-2 text-red-600">difference ₹ {money(Math.abs(totals.dr - totals.cr))}</span>}</span>
        </div>
      </div>
      {problem && <InlineError message={problem} />}

      <div className={`${panel} overflow-auto`}>
        <div className="border-b border-line px-4 py-2 text-xs font-bold">Last 30 days</div>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Voucher No</th><th className="px-3 py-2">Date</th><th className="px-3 py-2">Narration</th><th className="px-3 py-2 text-right">Amount (₹)</th><th className="px-3 py-2">By</th></tr></thead>
          <tbody className={tableBody}>
            {!recent.length && <tr><td colSpan={5} className="px-4 py-8 text-center text-stone-500">No {title.toLowerCase()} in the last 30 days.</td></tr>}
            {recent.map(v => (
              <tr key={v.id} className="cursor-pointer hover:bg-stone-100/60 dark:hover:bg-white/5" onClick={() => getVoucher(kind, v.id).then(setOpen).catch(e => setProblem(errorText(e)))}>
                <td className="px-3 py-1.5 font-semibold">{v.doc_no}{v.status === 'CANCELLED' && <span className="ml-2 text-red-600">cancelled</span>}</td><td className="px-3 py-1.5">{v.date}</td><td className="px-3 py-1.5">{v.narration}</td>
                <td className="px-3 py-1.5 text-right tabular-nums">{money(v.total)}</td><td className="px-3 py-1.5">{v.added_by}</td>
              </tr>
            ))}
          </tbody>
        </table>
        {open && (
          <div className="border-t border-line px-4 py-3 text-xs">
            <div className="mb-1 font-bold">{open.doc_no} · {open.date} {open.status === 'CANCELLED' && <span className="text-red-600">(cancelled)</span>}</div>
            {open.lines.map((l, i) => <div key={i} className="tabular-nums">{l.side} · {l.ledger} · ₹ {money(l.amount)}</div>)}
          </div>
        )}
      </div>
    </div>
    </ScreenFrame>
  );
}
