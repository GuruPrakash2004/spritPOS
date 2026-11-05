// Quotation (legacy document 1282): a priced offer to a customer. It changes no stock and posts nothing; it is kept in the shop's own
// quotation tables (POST /transactions/quotation). Type an item code or name on a line (suggestions appear), the quantity and, if it
// differs from the MRP, the rate. Below the form: the last 30 days' quotations.
// Keyboard: Enter moves to the next field (and on the last field adds a row); Alt+A adds a row; ⌘/Ctrl+S saves.

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
import { canDo, cancelQuotation, errorText, getCustomers, getItems, getQuotation, listQuotations, saveQuotation, todayIso, updateQuotation } from '../../lib/api';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const blank = () => ({ text: '', qty: '', rate: '' });
const days = (n) => { const d = new Date(); d.setDate(d.getDate() - n); return d.toISOString().slice(0, 10); };

export default function Quotation() {
  const [date, setDate] = useState(todayIso());
  const [customer, setCustomer] = useState('CASH');
  const [remarks, setRemarks] = useState('');
  const [lines, setLines] = useState([blank()]);
  const [items, setItems] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [recent, setRecent] = useState([]);
  const [open, setOpen] = useState(null);
  const [editing, setEditing] = useState(null);   // the quotation being edited (id and number), or null for a new one
  const [listOpen, setListOpen] = useState(false);   // the bar's List window
  const [all, setAll] = useState(null);   // the last year, for the List window
  const { confirm, dialog } = useConfirm();
  const [problem, setProblem] = useState('');
  const [toast, setToast] = useState(null);

  useEffect(() => { if (toast) { const t = setTimeout(() => setToast(null), 3000); return () => clearTimeout(t); } }, [toast]);
  useEffect(() => {
    getItems().then(l => setItems(l.filter(i => i.active !== false))).catch(e => setProblem(errorText(e)));
    getCustomers().then(setCustomers).catch(() => {});
  }, []);
  const reload = useCallback(() => listQuotations(days(30), todayIso()).then(setRecent).catch(e => setProblem(errorText(e))), []);
  useEffect(() => { void reload(); }, [reload]);

  // "CODE — NAME" labels for the suggestions; a line resolves by that label or by the bare code.
  const byLabel = useMemo(() => new Map(items.flatMap(i => [[`${i.code} — ${i.name}`.toLowerCase(), i], [String(i.code).toLowerCase(), i]])), [items]);
  const itemOf = (text) => byLabel.get(text.trim().toLowerCase());
  const rateOf = (l) => (l.rate !== '' ? Number(l.rate) : itemOf(l.text)?.mrp ?? 0);
  const amountOf = (l) => Math.round((Number(l.qty) || 0) * rateOf(l) * 100) / 100;
  const total = lines.reduce((a, l) => a + amountOf(l), 0);
  const update = (i, patch) => { setProblem(''); setLines(ls => ls.map((l, x) => (x === i ? { ...l, ...patch } : l))); };
  const addRow = () => setLines(ls => [...ls, blank()]);

  const save = async () => {
    const used = lines.filter(l => l.text.trim() || l.qty);
    if (!used.length) return setProblem('Add at least one item.');
    const bad = used.findIndex(l => !itemOf(l.text) || !(Number(l.qty) > 0));
    if (bad >= 0) return setProblem(`Row ${bad + 1}: pick an item from the list and enter a quantity above zero.`);
    setProblem('');
    const body = { date, customer, remarks, lines: used.map(l => ({ item_code: itemOf(l.text).code, qty: Number(l.qty), ...(l.rate !== '' ? { rate: Number(l.rate) } : {}) })) };
    if (!(await toastStore.defer('ok', `Quotation${editing ? ` ${editing.doc_no}` : ''} of ₹ ${money(total)} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    (editing ? updateQuotation(editing.id, body) : saveQuotation(body)).then(q => {
      setToast({ kind: 'ok', text: `Quotation ${q.doc_no} saved — ₹ ${money(q.total)}.` });
      resetForm(); setOpen(null);
      void reload();
    }).catch(e => setProblem(errorText(e)));
  };
  const resetForm = () => { setEditing(null); setDate(todayIso()); setCustomer('CASH'); setLines([blank()]); setRemarks(''); setProblem(''); };
  const startEdit = (q) => {   // load a saved quotation into the form; saving rewrites it (the number stays)
    const cust = customers.find(c => (c.name || c.code) === q.customer || c.code === q.customer);
    setEditing({ id: q.id, doc_no: q.doc_no }); setDate(q.date); setCustomer(cust ? cust.code : 'CASH'); setRemarks(q.remarks || '');
    setLines(q.lines.map(l => ({ text: `${l.item_code} — ${l.item_name}`, qty: String(l.qty), rate: String(l.rate) }))); setProblem('');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };
  const cancelDoc = async (q) => {
    if (!(await confirm(`Are you sure to CANCEL Quotation ${q.doc_no}?`))) return;
    if (!(await toastStore.defer('delete', `Quotation ${q.doc_no} will be cancelled when this closes.`, 'Cancel stopped. The quotation is unchanged.'))) return;
    cancelQuotation(q.id).then(done => {
      setToast({ kind: 'ok', text: `Quotation ${done.doc_no} cancelled.` });
      if (editing?.id === q.id) resetForm();
      setOpen(done); void reload();
    }).catch(e => setProblem(errorText(e)));
  };
  const loadAll = () => { setAll(null); listQuotations(days(365), todayIso()).then(setAll).catch(e => { setAll([]); setProblem(errorText(e)); }); };
  const walk = [...recent].reverse();   // oldest first, for First / Previous / Next / Last
  const at = open ? walk.findIndex(v => v.id === open.id) : -1;
  const goTo = (where) => {
    if (!walk.length) return;
    const to = where === 'first' ? 0 : where === 'last' ? walk.length - 1 : where === 'next' ? Math.min(walk.length - 1, at + 1) : Math.max(0, at < 0 ? walk.length - 1 : at - 1);
    getQuotation(walk[to].id).then(setOpen).catch(e => setProblem(errorText(e)));
  };
  const usable = !!open && open.status !== 'CANCELLED';
  const bar = (
    <ActionBar
      mode={editing ? 'Edit' : 'View'} onNew={canDo('Quotation', 'add') ? resetForm : undefined}
      onEdit={canDo('Quotation', 'edit') ? () => open && startEdit(open) : undefined} canEdit={usable} noEditReason="Open a saved one from the list first (a cancelled one can't be edited)"
      onDelete={canDo('Quotation', 'delete') ? () => open && void cancelDoc(open) : undefined} canDelete={usable} deleteLabel="Cancel Quotation" noDeleteReason="Open a saved one from the list first (it must not be cancelled already)"
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
        <ListModal title="Quotation" subtitle="Listing of the last 365 days" columns={[{ key: 'doc_no', label: 'Quotation No' }, { key: 'date', label: 'Date' }, { key: 'customer', label: 'Customer' }, { key: 'qty', label: 'Qty', kind: 'number' }, { key: 'total', label: 'Amount', kind: 'money' }, { key: 'status', label: 'Status' }, { key: 'added_by', label: 'Added By' }]} defaultKeys={['doc_no', 'date', 'customer', 'total', 'status']}
          rows={all ?? []} rowKey={r => r.id} loading={all === null} storageKey="quotation" exportName="Quotation" onRefresh={loadAll}
          onOpen={r => getQuotation(r.id).then(setOpen).catch(e => setProblem(errorText(e)))} onClose={() => setListOpen(false)} />
      )}
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>Quotation</h1><p className={pageSub}>A priced offer to a customer. It does not change stock.</p></div>
        <div className="flex gap-2">
          {editing && <button onClick={resetForm} className={secondaryButton}>Stop editing</button>}
          <button onClick={() => void save()} className={primaryButton}><Save size={14} />{editing ? 'Save changes' : 'Save'}<Kbd combo="Mod+S" /></button>
        </div>
      </div>
      {editing && <p className="rounded-md border border-line px-3 py-2 text-xs font-semibold text-accent-ink">Editing Quotation {editing.doc_no}: the number stays; saving replaces its lines.</p>}

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div><label htmlFor="q-date" className={label}>Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><input id="q-date" type="date" value={date} onChange={e => setDate(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div>
          <label htmlFor="q-cust" className={label}>Customer</label>
          <select id="q-cust" value={customer} onChange={e => setCustomer(e.target.value)} className={`${field} w-64`}>
            <option value="CASH">CASH</option>
            {customers.filter(c => c.code !== 'CASH').map(c => <option key={c.code} value={c.code}>{c.name || c.code}</option>)}
          </select>
        </div>
        <div className="min-w-60 flex-1"><label htmlFor="q-rem" className={label}>Remarks</label><input id="q-rem" value={remarks} maxLength={250} onChange={e => setRemarks(e.target.value)} className={`${field} w-full`} /></div>
      </div>

      <div className={`${panel} overflow-auto`}>
        <datalist id="q-items">{items.map(i => <option key={i.code} value={`${i.code} — ${i.name}`} />)}</datalist>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Sl</th><th className="px-3 py-2">Item</th><th className="px-3 py-2 text-right">Qty (bottles)</th><th className="px-3 py-2 text-right">Rate (₹)</th><th className="px-3 py-2 text-right">Amount (₹)</th></tr></thead>
          <tbody className={tableBody}>
            {lines.map((l, i) => (
              <tr key={i}>
                <td className="whitespace-nowrap px-3 py-1.5 text-stone-400"><span className="inline-flex items-center gap-1.5"><button onClick={() => setLines(ls => (ls.length > 1 ? ls.filter((_, x) => x !== i) : [blank()]))} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button>{i + 1}</span></td>
                <td className="px-3 py-1.5"><input list="q-items" value={l.text} onChange={e => update(i, { text: e.target.value })} className={`${field} w-full ${l.text && !itemOf(l.text) ? '!border-red-500' : ''}`} aria-label={`Item row ${i + 1}`} /></td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="1" value={l.qty} onChange={e => update(i, { qty: e.target.value })} className={`${field} w-24 text-right tabular-nums`} aria-label={`Quantity row ${i + 1}`} /></td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.rate} placeholder={itemOf(l.text) ? String(itemOf(l.text).mrp) : ''} onChange={e => update(i, { rate: e.target.value })} className={`${field} w-28 text-right tabular-nums`} aria-label={`Rate row ${i + 1}`} /></td>
                <td className="px-3 py-1.5 text-right tabular-nums">{money(amountOf(l))}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="flex items-center justify-between border-t border-line px-3 py-2">
          <button onClick={addRow} className={secondaryButton}><PlusCircle size={14} />Add Row<Kbd combo="Alt+A" /></button>
          <span className="text-sm font-bold text-accent-ink">Total ₹ {money(total)}</span>
        </div>
      </div>
      {problem && <InlineError message={problem} />}

      <div className={`${panel} overflow-auto`}>
        <div className="border-b border-line px-4 py-2 text-xs font-bold">Last 30 days</div>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Quotation No</th><th className="px-3 py-2">Date</th><th className="px-3 py-2">Customer</th><th className="px-3 py-2 text-right">Qty</th><th className="px-3 py-2 text-right">Amount (₹)</th></tr></thead>
          <tbody className={tableBody}>
            {!recent.length && <tr><td colSpan={5} className="px-4 py-8 text-center text-stone-500">No quotations in the last 30 days.</td></tr>}
            {recent.map(q => (
              <tr key={q.id} className="cursor-pointer hover:bg-stone-100/60 dark:hover:bg-white/5" onClick={() => getQuotation(q.id).then(setOpen).catch(e => setProblem(errorText(e)))}>
                <td className="px-3 py-1.5 font-semibold">{q.doc_no}{q.status === 'CANCELLED' && <span className="ml-2 text-red-600">cancelled</span>}</td><td className="px-3 py-1.5">{q.date}</td><td className="px-3 py-1.5">{q.customer}</td>
                <td className="px-3 py-1.5 text-right tabular-nums">{q.qty}</td><td className="px-3 py-1.5 text-right tabular-nums">{money(q.total)}</td>
              </tr>
            ))}
          </tbody>
        </table>
        {open && (
          <div className="border-t border-line px-4 py-3 text-xs">
            <div className="mb-1 font-bold">{open.doc_no} · {open.date} · {open.customer}</div>
            {open.lines.map((l, i) => <div key={i} className="tabular-nums">{l.item_code} — {l.item_name} · {l.qty} × ₹ {money(l.rate)} = ₹ {money(l.amount)}</div>)}
          </div>
        )}
      </div>
    </div>
    </ScreenFrame>
  );
}
