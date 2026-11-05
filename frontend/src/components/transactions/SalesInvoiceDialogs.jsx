// Dialogs used by SalesInvoice:
// - ReprintDialog (Re-Print Bill): the bills of the chosen dates, to re-print in a chosen format or edit.
// - CancelInvoiceDialog: cancel with a mandatory remark (stock and points go back); lists the last 30 days' saved bills
//   to pick from and the cancelled bills with their reasons.
// - HeldBillsDialog: list held bills and recall one into the invoice (onRecall).
// Keyboard: Re-Print — type the number, Enter finds it, Enter again prints; Cancel — Enter moves to the reason,
// Enter there cancels the bill; Held bills — ↑/↓ choose, Enter recalls. Esc closes each.

import { useEffect, useMemo, useState } from 'react';
import { RefreshCw, Undo2, XCircle } from 'lucide-react';
import Modal from '../common/Modal';
import { fetchPrintOptions, savedFormat } from '../../lib/printing';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { cancelSalesInvoice, errorText, listSalesInvoices, todayIso } from '../../lib/api';
import { dangerButton, field, iconButton, label, money, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import { SkeletonTableRows } from '../common/Skeleton';


// Re-Print Bill (the desktop's window): a Search box and the bills of the chosen dates (today to start with; From / To and refresh are the only
// extras), a plain grid (Bill No, Bill Date, Bill Amt, Sales Person, Counter, Cancelled, Delete, Party; cancelled and deleted bills are listed with
// Y), then Print Format, Edit Bill, Re-Print Bill and Close. Keys: type to search, ↑/↓ choose a bill, Enter re-prints it, Esc closes.
const dmy = (iso) => (/^\d{4}-\d{2}-\d{2}/.test(iso ?? '') ? `${iso.slice(8, 10)}/${iso.slice(5, 7)}/${iso.slice(0, 4)}` : (iso ?? ''));
const billRow = (b) => ({
  no: b.doc_no, date: dmy(b.date), amount: Number(b.total ?? b.totals?.net ?? 0), person: b.salesperson ?? '', counter: b.counter ?? '',
  cancelled: (b.cancelled ?? b.status === 'CANCELLED') ? 'Y' : 'N', deleted: (b.deleted ?? b.status === 'DELETED') ? 'Y' : 'N',
  party: !b.customer || b.customer === 'CASH' ? '' : b.customer,
});

export function ReprintDialog({ onClose, onEdit, onPrint }) {
  const [from, setFrom] = useState(() => todayIso());
  const [to, setTo] = useState(() => todayIso());
  const [loaded, setLoaded] = useState({ key: '', list: [], error: '' });
  const [again, setAgain] = useState(0);   // the refresh button
  const [search, setSearch] = useState('');
  const [picked, setPicked] = useState('');
  const [formats, setFormats] = useState([]);   // the print formats of the Sales Invoice: [{ key, label }]
  const [format, setFormat] = useState('');
  const key = `${from}|${to}|${again}`;
  const loading = loaded.key !== key;

  useEffect(() => {
    let live = true;
    listSalesInvoices(undefined, from, to)
      .then(l => { if (live) setLoaded({ key, list: l.filter(b => b.status !== 'HOLD').sort(newestFirst), error: '' }); })
      .catch(e => { if (live) setLoaded({ key, list: [], error: errorText(e) }); });
    return () => { live = false; };
  }, [from, to, again, key]);

  useEffect(() => {
    fetchPrintOptions('sales_invoice').then(o => {
      const list = o.designed ? o.options.filter(x => x.available).map(x => ({ key: x.name, label: x.name })) : o.files.map(f => ({ key: f, label: f.replace(/\.rpt$/i, '') }));
      setFormats(list);
      setFormat(o.designed
        ? (list.find(x => x.key === o.preferred)?.key ?? list.find(x => x.key === o.default)?.key ?? list[0]?.key ?? '')
        : (list.find(x => x.key === savedFormat('sales_invoice'))?.key ?? list[0]?.key ?? ''));
    }).catch(() => undefined);   // the print window then offers what it has
  }, []);

  const shown = useMemo(() => {
    const words = search.trim().toLowerCase().split(/\s+/).filter(Boolean);
    return loaded.list.map(b => ({ b, r: billRow(b) })).filter(({ r }) => !words.length || words.every(w => Object.values(r).join(' ').toLowerCase().includes(w)));
  }, [loaded.list, search]);
  // The first bill is selected when the list opens, changes or is searched (as the desktop highlights its first row).
  const at = shown.findIndex(x => x.r.no === picked);
  const current = shown[at === -1 ? 0 : at];
  const isLive = (x) => x && x.r.cancelled === 'N' && x.r.deleted === 'N';

  const move = (d) => { const next = shown[Math.max(0, Math.min(shown.length - 1, (at === -1 ? 0 : at) + d))]; if (next) setPicked(next.r.no); };
  const print = () => { if (current) { onPrint?.(current.r.no, format); onClose(); } };
  const edit = () => { if (isLive(current)) { onEdit?.(current.b); onClose(); } };
  const dateBox = 'rounded-md border border-[#B7C7DA] bg-white px-2 py-1 text-xs';
  const th = 'sticky top-0 border border-[#C9D3DF] bg-[#F0F0F0] px-2 py-1 text-left text-xs font-normal text-[#1B3FD0]';

  return (
    <Modal title="Re-Print Bill" onClose={onClose} wide footer={
      <div className="flex w-full flex-wrap items-center gap-3">
        <label htmlFor="rp-format" className="text-sm">Print Format</label>
        <select id="rp-format" value={format} onChange={e => setFormat(e.target.value)} className={`${field} !w-48`}>
          {!formats.length && <option value="">—</option>}
          {formats.map(f => <option key={f.key} value={f.key}>{f.label}</option>)}
        </select>
        <div className="ml-auto flex gap-2">
          <button onClick={edit} disabled={!isLive(current)} className="rounded-md border border-[#F0A8A8] bg-[#FFC0C0] px-6 py-2 text-sm text-stone-900 hover:bg-[#FFB0B0] disabled:cursor-not-allowed disabled:opacity-50">Edit Bill</button>
          <button onClick={print} disabled={!current} className="rounded-md border border-stone-300 bg-white px-5 py-2 text-sm text-stone-900 hover:bg-stone-50 disabled:cursor-not-allowed disabled:opacity-50">Re-Print Bill</button>
          <button onClick={onClose} className="rounded-md border border-stone-300 bg-white px-6 py-2 text-sm text-stone-900 hover:bg-stone-50">Close</button>
        </div>
      </div>
    }>
      <div className="space-y-3" onKeyDown={e => {
        if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { if (e.target.tagName !== 'SELECT' && e.target.type !== 'date') { e.preventDefault(); move(e.key === 'ArrowDown' ? 1 : -1); } }
        else if (e.key === 'Enter' && e.target.tagName !== 'BUTTON' && e.target.tagName !== 'SELECT') { e.preventDefault(); print(); }
      }}>
        <div className="flex flex-wrap items-center gap-3">
          <label htmlFor="rp-search" className="text-sm">Search</label>
          <input id="rp-search" data-autofocus autoFocus value={search} onChange={e => { setSearch(e.target.value); setPicked(''); }} autoComplete="off" className={`${field} !w-72`} />
          <div className="ml-auto flex items-center gap-2 text-xs">
            <label htmlFor="rp-from" className="font-semibold">From</label>
            <input id="rp-from" type="date" value={from} max={to} onChange={e => { if (e.target.value) { setFrom(e.target.value); setPicked(''); } }} className={dateBox} />
            <label htmlFor="rp-to" className="font-semibold">To</label>
            <input id="rp-to" type="date" value={to} min={from} max={todayIso()} onChange={e => { if (e.target.value) { setTo(e.target.value); setPicked(''); } }} className={dateBox} />
            <button type="button" onClick={() => setAgain(n => n + 1)} className={`${iconButton} border border-line`} aria-label="Refresh" title="Refresh"><RefreshCw size={14} /></button>
          </div>
        </div>
        {loaded.error && <p className="text-red-600 dark:text-red-400">{loaded.error}</p>}
        <div className="h-[22rem] overflow-auto border border-[#7A7A7A] bg-[#ABABAB]">
          <table className="w-full border-collapse bg-white text-sm" aria-label="Bills">
            <thead>
              <tr><th className={th}>Bill No</th><th className={th}>Bill Date</th><th className={`${th} text-right`}>Bill Amt</th><th className={th}>Sales Person</th><th className={th}>Counter</th><th className={th}>Cancelled</th><th className={th}>Delete</th><th className={th}>Party</th></tr>
            </thead>
            <tbody>
              {shown.map((x) => {
                const on = x === current;
                return (
                  <tr key={x.r.no} aria-selected={on} onClick={() => setPicked(x.r.no)} onDoubleClick={() => { setPicked(x.r.no); onPrint?.(x.r.no, format); onClose(); }}
                    className={`cursor-pointer ${on ? 'bg-[#FF8000] text-white' : 'text-stone-900 hover:bg-[#FFE9D0]'}`}>
                    <td className="border border-[#C9D3DF] px-2 py-0.5 font-mono">{x.r.no}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5 tabular-nums">{x.r.date}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5 text-right tabular-nums">{x.r.amount.toFixed(2)}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5">{x.r.person}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5">{x.r.counter}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5">{x.r.cancelled}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5">{x.r.deleted}</td>
                    <td className="border border-[#C9D3DF] px-2 py-0.5">{x.r.party}</td>
                  </tr>
                );
              })}
              {loading && !shown.length && <SkeletonTableRows rows={3} cols={8} />}
            </tbody>
          </table>
        </div>
      </div>
    </Modal>
  );
}

// Cancel Bill (the desktop's window): a Search box and a grid of the saved bills of the last 30 days (Sel, Bill No, Bill Date, Bill Amt,
// Sales Person, Counter, Party); with one selected, Cancel Bill asks for the mandatory reason, then cancels it (stock and points go back);
// Edit Bill loads it onto the bill screen. Keys: type to search, ↑/↓ choose, Enter = Cancel Bill, Esc closes.
const LIST_DAYS = 30;
const newestFirst = (a, b) => `${b.date} ${b.time ?? ''}`.localeCompare(`${a.date} ${a.time ?? ''}`);

export function CancelInvoiceDialog({ initialNo, onClose, onDone, onEdit }) {
  const [search, setSearch] = useState('');
  const [picked, setPicked] = useState(initialNo || '');
  const [reasonFor, setReasonFor] = useState('');   // set once Cancel Bill is pressed: the reason box replaces the list
  const [remarks, setRemarks] = useState('');
  const [error, setError] = useState('');
  const [bills, setBills] = useState(null);
  const { confirm, dialog: confirmDialog } = useConfirm();

  useEffect(() => {
    const from = new Date(Date.now() - LIST_DAYS * 86400000).toISOString().slice(0, 10);
    listSalesInvoices('SAVED', from).then(l => setBills(l.sort(newestFirst))).catch(e => { setBills([]); setError(errorText(e)); });
  }, []);

  const shown = useMemo(() => {
    const words = search.trim().toLowerCase().split(/\s+/).filter(Boolean);
    return (bills ?? []).filter(b => !words.length || words.every(w => [b.doc_no, b.date, b.salesperson, b.counter, b.customer, money(b.totals?.net ?? 0)].join(' ').toLowerCase().includes(w)));
  }, [bills, search]);
  const at = shown.findIndex(b => b.doc_no === picked);
  const bill = shown[at];

  const move = (d) => { const next = shown[Math.max(0, Math.min(shown.length - 1, (at === -1 ? (d > 0 ? -1 : shown.length) : at) + d))]; if (next) setPicked(next.doc_no); };

  const askReason = () => {
    if (!bill) return setError('Select a bill to cancel.');
    setError('');
    setReasonFor(bill.doc_no);
    setTimeout(() => document.getElementById('ci-rmk')?.focus(), 0);
  };
  const submit = async () => {
    if (remarks.trim().length < 3) return setError('A reason for cancelling is required.');
    setError('');
    if (!(await confirm(`Are you sure you want to cancel invoice ${reasonFor}? Its stock goes back and its points are reversed.`))) return;
    if (!(await toastStore.defer('delete', `Bill ${reasonFor} will be cancelled when this closes.`, 'Bill not cancelled. Nothing was changed.'))) return;
    cancelSalesInvoice(reasonFor, remarks.trim())
      .then(inv => onDone(`Bill ${inv.doc_no} cancelled — stock returned.`))
      .catch(e => setError(errorText(e)));
  };

  const th = 'px-3 py-1.5 text-left font-medium text-[#1B5FA8]';
  return (
    <>
    {confirmDialog}
    <Modal
      title={reasonFor ? `Cancel Bill ${reasonFor}` : 'Cancel Bill'}
      onClose={onClose}
      wide
      footer={reasonFor ? <>
        <button onClick={() => { setReasonFor(''); setError(''); }} className={secondaryButton}>Back</button>
        <button onClick={submit} className={dangerButton}><XCircle size={14} />Cancel Bill</button>
      </> : <>
        {onEdit && <button onClick={() => bill && onEdit(bill)} disabled={!bill} className={secondaryButton}>Edit Bill</button>}
        <button onClick={askReason} disabled={!bill} className={secondaryButton}>Cancel Bill</button>
        <button onClick={onClose} className={secondaryButton}>Close</button>
      </>}
    >
      {reasonFor ? (
        <div className="space-y-3">
          <label htmlFor="ci-rmk" className={label}>Reason for cancelling<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <textarea id="ci-rmk" rows={3} value={remarks} onChange={e => setRemarks(e.target.value)} onKeyDown={e => { if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); void submit(); } }} className={`${field} w-full`} placeholder="e.g. customer returned the bottles before leaving" />
          <p className="text-[11px] text-stone-500">Enter cancels the bill · Shift+Enter for a new line</p>
          {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
        </div>
      ) : (
        <div className="space-y-3" onKeyDown={e => {
          if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); move(e.key === 'ArrowDown' ? 1 : -1); }
          else if (e.key === 'Enter' && e.target.tagName !== 'BUTTON') { e.preventDefault(); askReason(); }
        }}>
          <div className="flex items-center gap-3">
            <label htmlFor="ci-search" className="text-sm">Search</label>
            <input id="ci-search" data-autofocus autoFocus value={search} onChange={e => setSearch(e.target.value)} autoComplete="off" className={`${field} w-72`} />
          </div>
          {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
          <div className="h-80 overflow-auto border border-line bg-surface">
            <table className="w-full text-xs" aria-label="Saved bills">
              <thead className="sticky top-0 bg-[#F0F3F8] dark:bg-[#1E1E1E]">
                <tr><th className="w-12 px-3 py-1.5 text-left font-medium">Sel</th><th className={th}>Bill No</th><th className={th}>Bill Date</th><th className={`${th} text-right`}>Bill Amt</th><th className={th}>Sales Person</th><th className={th}>Counter</th><th className={th}>Party</th></tr>
              </thead>
              <tbody>
                {shown.map(b => (
                  <tr key={b.doc_no} aria-selected={b.doc_no === picked} onClick={() => setPicked(b.doc_no)} onDoubleClick={() => { setPicked(b.doc_no); setReasonFor(b.doc_no); }}
                    className={`cursor-pointer border-b border-line-soft ${b.doc_no === picked ? 'bg-[#FF8C1A] text-white' : 'hover:bg-subtle'}`}>
                    <td className="px-3 py-1"><input type="radio" tabIndex={-1} readOnly checked={b.doc_no === picked} aria-label={`Select ${b.doc_no}`} /></td>
                    <td className="px-3 py-1 font-mono">{b.doc_no}</td>
                    <td className="whitespace-nowrap px-3 py-1 tabular-nums">{dmy(b.date)}</td>
                    <td className="px-3 py-1 text-right tabular-nums">{money(b.totals?.net ?? b.total ?? 0)}</td>
                    <td className="px-3 py-1">{b.salesperson}</td>
                    <td className="px-3 py-1">{b.counter}</td>
                    <td className="px-3 py-1">{b.customer}</td>
                  </tr>
                ))}
                {bills && !shown.length && <tr><td colSpan={7} className="px-3 py-6 text-center text-stone-400">No saved bills found.</td></tr>}
                {!bills && <SkeletonTableRows rows={3} cols={7} />}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </Modal>
    </>
  );
}

// Held bills saved on the backend (legacy frmUtlHoldBill), recallable from any session.
export function HeldBillsDialog({ onClose, onRecall }) {
  const [bills, setBills] = useState(null);
  const [error, setError] = useState('');
  const [at, setAt] = useState(0);

  const load = () => listSalesInvoices('HOLD').then(list => { setBills(list); setAt(0); }).catch(e => setError(errorText(e)));
  useEffect(() => { void load(); }, []);

  const recall = async (inv) => {
    if (!(await toastStore.defer('ok', `Held bill ${inv.doc_no} will be recalled when this closes.`, 'Recall cancelled. The bill is still held.'))) return;
    // Held bills never moved stock. The screen holds the bill being made (if any), removes this parked copy and loads its lines.
    try { await onRecall(inv); } catch (e) { setError(errorText(e)); void load(); }
  };

  return (
    <Modal title="Held Bills" onClose={onClose} wide footer={<button onClick={onClose} className={secondaryButton}>Close</button>}>
      {error && <p className="mb-2 text-red-600 dark:text-red-400">{error}</p>}
      {!!bills?.length && <p className="mb-2 text-[11px] text-stone-500">↑↓ choose a bill · Enter recalls it · Esc closes</p>}
      <table className="w-full outline-none" tabIndex={0} data-autofocus aria-label="Held bills" onKeyDown={e => {
        if (!bills?.length) return;
        if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); setAt(a => Math.max(0, Math.min(bills.length - 1, a + (e.key === 'ArrowDown' ? 1 : -1)))); }
        if (e.key === 'Enter') { e.preventDefault(); void recall(bills[Math.min(at, bills.length - 1)]); }
      }}>
        <thead className={tableHead}>
          <tr><th className="px-3 py-2">Held No</th><th className="hidden px-3 py-2 sm:table-cell">Time</th><th className="px-3 py-2">Customer</th><th className="px-3 py-2 text-right">Items</th><th className="px-3 py-2 text-right">Value</th><th className="px-3 py-2" /></tr>
        </thead>
        <tbody className={tableBody}>
          {bills?.map((b, i) => (
            <tr key={b.doc_no} aria-selected={i === at} onClick={() => setAt(i)} className={i === at ? 'bg-accent-soft' : ''}>
              <td className="px-3 py-2 font-mono">{b.doc_no}</td>
              <td className="hidden px-3 py-2 font-mono sm:table-cell">{b.time ?? ''}</td>
              <td className="px-3 py-2">{b.customer}</td>
              <td className="px-3 py-2 text-right tabular-nums">{b.lines.length}</td>
              <td className="px-3 py-2 text-right tabular-nums">₹ {money(b.totals.net)}</td>
              <td className="px-3 py-2 text-right"><button onClick={() => recall(b)} tabIndex={-1} className={`${primaryButton} ml-auto py-1`}><Undo2 size={12} />Recall</button></td>
            </tr>
          ))}
          {bills && !bills.length && <tr><td colSpan={6} className="px-3 py-8 text-center text-stone-400">No held bills.</td></tr>}
          {!bills && !error && <SkeletonTableRows rows={4} cols={6} />}
        </tbody>
      </table>
    </Modal>
  );
}
