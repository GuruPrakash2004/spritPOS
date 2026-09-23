import { useEffect, useState } from 'react';
import { Printer, Search, Undo2, XCircle } from 'lucide-react';
import Modal from './Modal';
import { api, errorText, qs, type ApiInvoice } from './api';
import { numberToWords } from './formatters';
import { dangerButton, field, label, money, primaryButton, secondaryButton, tableBody, tableHead } from './ui';

const path = (no: string) => `/transactions/sales-invoice/${no}`;

// Re-Print Bill: fetch a saved bill and show it in a printable layout (legacy fmtSInv).
export function ReprintDialog({ initialNo, onClose }: { initialNo: string; onClose: () => void }) {
  const [no, setNo] = useState(initialNo);
  const [inv, setInv] = useState<ApiInvoice | null>(null);
  const [error, setError] = useState('');

  const load = (billNo: string) => {
    if (!billNo.trim()) return;
    api<{ invoice: ApiInvoice }>(`${path(billNo.trim())}/reprint`)
      .then(r => { setInv(r.invoice); setError(''); })
      .catch(e => { setInv(null); setError(errorText(e)); });
  };

  // Load the previous bill straight away when there is one.
  useEffect(() => { if (initialNo) load(initialNo); }, [initialNo]);

  return (
    <Modal
      title="Re-Print Bill"
      onClose={onClose}
      footer={<>
        <button onClick={onClose} className={secondaryButton}>Close</button>
        <button onClick={() => window.print()} disabled={!inv} className={primaryButton}><Printer size={14} />Print</button>
      </>}
    >
      <div className="mb-3 flex items-end gap-2">
        <div className="flex-1">
          <label htmlFor="rp-no" className={label}>Bill No</label>
          <input id="rp-no" value={no} onChange={e => setNo(e.target.value)} onKeyDown={e => e.key === 'Enter' && load(no)} placeholder="26/c10-0083718" className={`${field} w-full font-mono`} />
        </div>
        <button onClick={() => load(no)} className={secondaryButton}><Search size={14} />Find</button>
      </div>
      {error && <p className="mb-2 text-red-600 dark:text-red-400">{error}</p>}
      {inv && (
        <div className="print-area rounded-lg border border-dashed border-stone-400 bg-white p-4 font-mono text-[11px] text-black">
          <div className="text-center">
            <div className="text-sm font-bold">THE LIQUOR HOUSE</div>
            <div>Byatarayanapura, Bangalore-92 • CL-2</div>
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
  );
}

// Cancel Invoice: a saved bill is cancelled with a mandatory remark and its stock is returned.
export function CancelInvoiceDialog({ initialNo, onClose, onDone }: { initialNo: string; onClose: () => void; onDone: (msg: string) => void }) {
  const [no, setNo] = useState(initialNo);
  const [remarks, setRemarks] = useState('');
  const [error, setError] = useState('');

  const submit = () => {
    if (!no.trim()) return setError('Enter the bill number.');
    if (remarks.trim().length < 3) return setError('A reason for cancelling is required.');
    api<ApiInvoice>(`${path(no.trim())}/cancel`, { method: 'POST', body: { remarks: remarks.trim() } })
      .then(inv => onDone(`Bill ${inv.doc_no} cancelled — stock returned.`))
      .catch(e => setError(errorText(e)));
  };

  return (
    <Modal
      title="Cancel Invoice"
      onClose={onClose}
      footer={<>
        <button onClick={onClose} className={secondaryButton}>Back</button>
        <button onClick={submit} className={dangerButton}><XCircle size={14} />Cancel Invoice</button>
      </>}
    >
      <div className="space-y-3">
        <div>
          <label htmlFor="ci-no" className={`${label} !text-red-600 dark:!text-red-400`}>Bill No</label>
          <input id="ci-no" value={no} onChange={e => setNo(e.target.value)} className={`${field} w-full font-mono`} />
        </div>
        <div>
          <label htmlFor="ci-rmk" className={`${label} !text-red-600 dark:!text-red-400`}>Reason for cancelling</label>
          <textarea id="ci-rmk" rows={3} value={remarks} onChange={e => setRemarks(e.target.value)} className={`${field} w-full`} placeholder="e.g. customer returned the bottles before leaving" />
        </div>
        {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
      </div>
    </Modal>
  );
}

// Held bills saved on the backend (legacy frmUtlHoldBill), recallable from any session.
export function HeldBillsDialog({ onClose, onRecall }: { onClose: () => void; onRecall: (inv: ApiInvoice) => void }) {
  const [bills, setBills] = useState<ApiInvoice[] | null>(null);
  const [error, setError] = useState('');

  useEffect(() => {
    api<ApiInvoice[]>(`/transactions/sales-invoice${qs({ status: 'HOLD' })}`).then(setBills).catch(e => setError(errorText(e)));
  }, []);

  const recall = (inv: ApiInvoice) => {
    // Held bills never moved stock, so recalling just removes the parked copy and loads its lines.
    api(path(inv.doc_no), { method: 'DELETE' }).then(() => onRecall(inv)).catch(e => setError(errorText(e)));
  };

  return (
    <Modal title="Held Bills" onClose={onClose} wide footer={<button onClick={onClose} className={secondaryButton}>Close</button>}>
      {error && <p className="mb-2 text-red-600 dark:text-red-400">{error}</p>}
      <table className="w-full">
        <thead className={tableHead}>
          <tr><th className="px-3 py-2">Held No</th><th className="px-3 py-2">Time</th><th className="px-3 py-2">Customer</th><th className="px-3 py-2 text-right">Items</th><th className="px-3 py-2 text-right">Value</th><th className="px-3 py-2" /></tr>
        </thead>
        <tbody className={tableBody}>
          {bills?.map(b => (
            <tr key={b.doc_no}>
              <td className="px-3 py-2 font-mono">{b.doc_no}</td>
              <td className="px-3 py-2 font-mono">{b.time ?? ''}</td>
              <td className="px-3 py-2">{b.customer}</td>
              <td className="px-3 py-2 text-right font-mono">{b.lines.length}</td>
              <td className="px-3 py-2 text-right font-mono">₹ {money(b.totals.net)}</td>
              <td className="px-3 py-2 text-right"><button onClick={() => recall(b)} className={`${primaryButton} ml-auto py-1`}><Undo2 size={12} />Recall</button></td>
            </tr>
          ))}
          {bills && !bills.length && <tr><td colSpan={6} className="px-3 py-8 text-center text-stone-400">No held bills.</td></tr>}
          {!bills && !error && <tr><td colSpan={6} className="px-3 py-8 text-center text-stone-400">Loading…</td></tr>}
        </tbody>
      </table>
    </Modal>
  );
}
