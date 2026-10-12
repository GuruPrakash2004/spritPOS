// Dialogs used by SalesInvoice:
// - ReprintDialog: fetch a saved bill (/reprint) and print it in a chosen .rpt format.
// - CancelInvoiceDialog: cancel with a mandatory remark (stock and points go back); lists the last 30 days' saved bills
//   to pick from and the cancelled bills with their reasons.
// - HeldBillsDialog: list held bills and recall one into the invoice (onRecall).
// Keyboard: Re-Print — type the number, Enter finds it, Enter again prints; Cancel — Enter moves to the reason,
// Enter there cancels the bill; Held bills — ↑/↓ choose, Enter recalls. Esc closes each.

import { useEffect, useState } from 'react';
import { Printer, Search, Undo2, XCircle } from 'lucide-react';
import Modal from '../common/Modal';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { cancelSalesInvoice, errorText, listSalesInvoices, reprintSalesInvoice, } from '../../lib/api';
import { numberToWords } from '../../lib/formatters';
import {  dangerButton, field, label, money, primaryButton, secondaryButton, tableBody, tableHead , printButton } from '../../lib/styles';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import PrintDialog from '../print/PrintDialog';
import { SkeletonTableRows } from '../common/Skeleton';


// Re-Print Bill: fetch a saved bill and show it in a printable layout with RPT model choices.
export function ReprintDialog({ initialNo, onClose }) {
  const { company } = useCompanyProfile();
  const [no, setNo] = useState(initialNo);
  const [inv, setInv] = useState(null);
  const [error, setError] = useState('');
  const [showPrintModal, setShowPrintModal] = useState(false);

  const load = (billNo) => {
    if (!billNo.trim()) return;
    reprintSalesInvoice(billNo.trim())
      .then(r => { setInv(r.invoice); setError(''); })
      .catch(e => { setInv(null); setError(errorText(e)); });
  };

  // Load the previous bill straight away when there is one.
  useEffect(() => { if (initialNo) load(initialNo); }, [initialNo]);

  return (
    <>
      <Modal
        title="Re-Print Bill"
        onClose={onClose}
        footer={<>
          <button onClick={onClose} className={secondaryButton}>Close</button>
          <button
            onClick={() => setShowPrintModal(true)}
            disabled={!inv}
            className={printButton}
          >
            <Printer size={14} />Select Format & Print
          </button>
        </>}
      >
      <div className="mb-3 flex items-end gap-2">
        <div className="flex-1">
          <label htmlFor="rp-no" className={label}>Bill No</label>
          <input id="rp-no" value={no} onChange={e => setNo(e.target.value)} onKeyDown={e => {
            if (e.key !== 'Enter') return;
            e.preventDefault();
            if (inv && inv.doc_no === no.trim()) setShowPrintModal(true);   // found already: Enter again prints
            else load(no);
          }} placeholder="Bill number" className={`${field} w-full font-mono`} />
        </div>
        <button onClick={() => load(no)} tabIndex={-1} className={secondaryButton}><Search size={14} />Find</button>
      </div>
      {error && <p className="mb-2 text-red-600 dark:text-red-400">{error}</p>}
      {inv && <p className="mb-2 text-[11px] text-stone-500">Press Enter to print this bill.</p>}
      {inv && (
        <div className="print-area rounded-lg border border-dashed border-stone-400 bg-white p-4 font-mono text-[11px] text-black">
          <div className="text-center">
            <div className="text-sm font-bold">{company?.name || 'Company profile unavailable'}</div>
            <div>{company?.address || 'Company address unavailable'} • {company?.licence_type || 'Licence not configured'}</div>
            <div className="mt-1 font-bold">DUPLICATE COPY</div>
          </div>
          <div className="my-2 flex justify-between border-y border-dashed border-stone-400 py-1">
            <span>Bill: {inv.doc_no}</span><span>{inv.date} {inv.time ?? ''}</span>
          </div>
          <div className="mb-1">Customer: {inv.customer} • Counter: {inv.counter} • By: {inv.salesperson ?? '-'}</div>
          {inv.status === 'CANCELLED' && <div className="mb-1 font-bold">*** CANCELLED: {inv.cancel_remarks} ***</div>}
          <table className="w-full">
            <thead><tr className="border-b border-dashed border-stone-400"><th className="text-left">Item</th><th className="text-right">Qty</th><th className="text-right">Rate</th><th className="text-right">Amt</th></tr></thead>
            <tbody>
              {inv.lines.map(l => (
                <tr key={l.item_code}><td>{l.item_name}</td><td className="text-right">{l.qty}</td><td className="text-right">{money(l.rate)}</td><td className="text-right">{money(l.amount)}</td></tr>
              ))}
            </tbody>
          </table>
          <div className="mt-2 space-y-0.5 border-t border-dashed border-stone-400 pt-1 text-right">
            <div>Qty {inv.totals.qty} • Litres {inv.totals.litres}</div>
            {inv.totals.discount > 0 && <div>Discount: -{money(inv.totals.discount)}</div>}
            {inv.totals.points_redeemed > 0 && <div>Points redeemed: -{money(inv.totals.points_redeemed)}</div>}
            <div className="text-sm font-bold">NET: ₹ {money(inv.totals.net)}</div>
            <div className="text-left italic">{numberToWords(inv.totals.net)}</div>
          </div>
        </div>
      )}
    </Modal>
    {showPrintModal && inv && <PrintDialog kind="sales_invoice" docNo={inv.doc_no} title={`Print Invoice ${inv.doc_no}`} onClose={() => setShowPrintModal(false)} />}
  </>
  );
}

// Cancel Invoice: a saved bill is cancelled with a mandatory remark and its stock is returned. Two lists help: the recent
// saved bills (click one, or ↑/↓ in Bill No, to fill the number) and the cancelled bills with their reasons, newest first.
const LIST_DAYS = 30;
const newestFirst = (a, b) => `${b.date} ${b.time ?? ''}`.localeCompare(`${a.date} ${a.time ?? ''}`);

export function CancelInvoiceDialog({ initialNo, onClose, onDone }) {
  const [no, setNo] = useState(initialNo);
  const [remarks, setRemarks] = useState('');
  const [error, setError] = useState('');
  const [view, setView] = useState('saved');   // 'saved' (bills that can be cancelled) | 'cancelled'
  const [bills, setBills] = useState({ saved: null, cancelled: null });
  const [at, setAt] = useState(-1);
  const { confirm, dialog: confirmDialog } = useConfirm();

  useEffect(() => {
    const from = new Date(Date.now() - LIST_DAYS * 86400000).toISOString().slice(0, 10);
    listSalesInvoices('SAVED', from).then(l => setBills(b => ({ ...b, saved: l.sort(newestFirst) }))).catch(() => setBills(b => ({ ...b, saved: [] })));
    listSalesInvoices('CANCELLED', from).then(l => setBills(b => ({ ...b, cancelled: l.sort(newestFirst) }))).catch(() => setBills(b => ({ ...b, cancelled: [] })));
  }, []);

  const choose = (i) => { const bill = bills.saved?.[i]; if (bill) { setAt(i); setNo(bill.doc_no); setError(''); } };

  const submit = async () => {
    if (!no.trim()) return setError('Enter the bill number.');
    if (remarks.trim().length < 3) return setError('A reason for cancelling is required.');
    setError('');
    if (!(await confirm(`Are you sure you want to cancel invoice ${no.trim()}? Its stock goes back and its points are reversed.`))) return;
    if (!(await toastStore.defer('delete', `Bill ${no.trim()} will be cancelled when this closes.`, 'Bill not cancelled. Nothing was changed.'))) return;
    cancelSalesInvoice(no.trim(), remarks.trim())
      .then(inv => onDone(`Bill ${inv.doc_no} cancelled — stock returned.`))
      .catch(e => setError(errorText(e)));
  };

  const list = bills[view];
  const cancelledView = view === 'cancelled';
  const tab = (id, text, count) => (
    <button type="button" role="tab" aria-selected={view === id} tabIndex={-1} onClick={() => setView(id)}
      className={`rounded-md px-3 py-1 text-xs font-semibold transition-colors ${view === id ? 'bg-surface text-stone-900 shadow-card dark:text-stone-50' : 'text-stone-600 hover:text-stone-900 dark:text-stone-300 dark:hover:text-stone-50'}`}>
      {text}{count != null ? ` (${count})` : ''}
    </button>
  );

  return (
    <>
    {confirmDialog}
    <Modal
      title="Cancel Invoice"
      onClose={onClose}
      wide
      footer={<>
        <button onClick={onClose} className={secondaryButton}>Back</button>
        <button onClick={submit} className={dangerButton}><XCircle size={14} />Cancel Invoice</button>
      </>}
    >
      <div className="space-y-3">
        <div>
          <label htmlFor="ci-no" className={label}>Bill No<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <input id="ci-no" value={no} onChange={e => { setNo(e.target.value); setAt(-1); }} onKeyDown={e => {
            if (e.key === 'Enter') { e.preventDefault(); document.getElementById('ci-rmk')?.focus(); }
            if ((e.key === 'ArrowDown' || e.key === 'ArrowUp') && bills.saved?.length) {
              e.preventDefault();
              setView('saved');
              choose(Math.max(0, Math.min(bills.saved.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1))));
            }
          }} className={`${field} w-full font-mono`} />
        </div>
        <div>
          <label htmlFor="ci-rmk" className={label}>Reason for cancelling<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <textarea id="ci-rmk" rows={3} value={remarks} onChange={e => setRemarks(e.target.value)} onKeyDown={e => { if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); void submit(); } }} className={`${field} w-full`} placeholder="e.g. customer returned the bottles before leaving" />
          <p className="mt-1 text-[11px] text-stone-500">Enter cancels the bill · Shift+Enter for a new line · ↑↓ in Bill No picks from the list</p>
        </div>
        {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
        <div>
          <div role="tablist" aria-label="Bill lists" className="mb-2 inline-flex gap-1 rounded-lg bg-muted p-1">
            {tab('saved', `Saved bills · last ${LIST_DAYS} days`, bills.saved?.length)}
            {tab('cancelled', 'Cancelled bills', bills.cancelled?.length)}
          </div>
          <div className="max-h-56 overflow-auto rounded-lg border border-line">
            <table className="w-full text-xs" aria-label={cancelledView ? 'Cancelled bills' : 'Saved bills'}>
              <thead className={tableHead}>
                <tr><th className="px-3 py-2">Bill No</th><th className="px-3 py-2">Date</th><th className="px-3 py-2">Customer</th><th className="px-3 py-2 text-right">Value</th>{cancelledView && <th className="px-3 py-2">Reason</th>}</tr>
              </thead>
              <tbody className={tableBody}>
                {list?.map((b, i) => (
                  <tr key={b.doc_no} aria-selected={!cancelledView && i === at} onClick={cancelledView ? undefined : () => { choose(i); document.getElementById('ci-rmk')?.focus(); }}
                    className={`${cancelledView ? '' : 'cursor-pointer hover:bg-subtle'} ${!cancelledView && i === at ? 'bg-accent-soft' : ''}`}>
                    <td className="px-3 py-1.5 font-mono">{b.doc_no}</td>
                    <td className="whitespace-nowrap px-3 py-1.5 tabular-nums">{b.date} {b.time ?? ''}</td>
                    <td className="px-3 py-1.5">{b.customer}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums">₹ {money(b.totals?.net ?? 0)}</td>
                    {cancelledView && <td className="px-3 py-1.5">{b.cancel_remarks ?? ''}</td>}
                  </tr>
                ))}
                {list && !list.length && <tr><td colSpan={cancelledView ? 5 : 4} className="px-3 py-6 text-center text-stone-400">{cancelledView ? 'No cancelled bills in this period.' : 'No saved bills in this period.'}</td></tr>}
                {!list && <SkeletonTableRows rows={3} cols={cancelledView ? 5 : 4} />}
              </tbody>
            </table>
          </div>
        </div>
      </div>
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
