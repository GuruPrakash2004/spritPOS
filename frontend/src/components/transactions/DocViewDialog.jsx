// Shows one saved document read-only (what the action bar's List → Open Selected and the First / Previous / Next / Last buttons load):
// its header, its lines and totals, with Print and, for sales bills, Cancel bill. Money the role may not see (purchase cost) shows as —.

import { Ban, ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight, Pencil, Printer } from 'lucide-react';
import Modal from '../common/Modal';
import { badge, money, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const HEAD = [['date', 'Date'], ['time', 'Time'], ['customer', 'Customer'], ['supplier', 'Supplier'], ['counter', 'Counter'], ['payment_type', 'Payment'], ['salesperson', 'Sales person'],
  ['paid_from', 'Paid from'], ['reason', 'Reason'], ['supplier_inv_no', 'Supplier invoice'], ['supplier_inv_date', 'Invoice date'], ['purchase_type', 'Type'], ['sys_name', 'PC'], ['added_by', 'Added by']];
const COLS = [['item_code', 'Item Code'], ['item_name', 'Item'], ['ledger', 'Ledger'], ['narration', 'Narration'], ['cases', 'Cases'], ['bottles', 'Bottles'], ['qty', 'Qty'], ['system_qty', 'System Qty'], ['physical_qty', 'Physical Qty'], ['diff_qty', 'Difference'],
  ['rate', 'Rate'], ['pur_rate', 'Pur Rate'], ['mrp', 'MRP'], ['amount', 'Amount']];
const NUM = new Set(['cases', 'bottles', 'qty', 'system_qty', 'physical_qty', 'diff_qty', 'rate', 'pur_rate', 'mrp', 'amount']);
const MONEY = new Set(['rate', 'pur_rate', 'mrp', 'amount']);

// `nav` (the same object the action bar takes) adds First / Previous / Next / Last buttons, so the viewer keeps walking through the documents.
export default function DocViewDialog({ doc, kind, title, position, nav, onClose, onPrint, onCancelBill, onEdit }) {
  const cancelled = ['CANCELLED', 'DELETED'].includes(doc.status);
  const lines = doc.lines ?? [];
  const shownCols = COLS.filter(([k]) => lines.some(l => l[k] !== undefined && l[k] !== null && l[k] !== ''));
  const totals = doc.totals ?? {};
  const head = HEAD.filter(([k]) => doc[k] !== undefined && doc[k] !== null && doc[k] !== '');
  return (
    <Modal title={`${title} ${doc.doc_no}${position ? ` · ${position}` : ''}`} onClose={onClose} wide
      footer={<>
        {nav && (
          <span role="group" aria-label="Move between records" className="mr-auto flex items-center gap-1">
            <button type="button" onClick={nav.onFirst} disabled={nav.atStart} className={secondaryButton} aria-label="First record" title="First record"><ChevronsLeft size={14} /></button>
            <button type="button" onClick={nav.onPrev} disabled={nav.atStart} className={secondaryButton} aria-label="Previous record" title="Previous record"><ChevronLeft size={14} /></button>
            <button type="button" onClick={nav.onNext} disabled={nav.atEnd} className={secondaryButton} aria-label="Next record" title="Next record"><ChevronRight size={14} /></button>
            <button type="button" onClick={nav.onLast} disabled={nav.atEnd} className={secondaryButton} aria-label="Last record" title="Last record"><ChevronsRight size={14} /></button>
          </span>
        )}
        {onEdit && !cancelled && doc.status !== 'HOLD' && <button type="button" onClick={onEdit} className={secondaryButton}><Pencil size={14} />Edit</button>}
        {kind === 'sales_invoice' && !cancelled && onCancelBill && <button type="button" onClick={onCancelBill} className={secondaryButton}><Ban size={14} />Cancel bill</button>}
        {onPrint && <button type="button" onClick={onPrint} className={secondaryButton}><Printer size={14} />Print / preview</button>}
        <button type="button" onClick={onClose} className={secondaryButton}>Close</button>
      </>}>
      <div className="mb-3 flex flex-wrap items-center gap-2">
        <span className="font-mono text-sm font-semibold">{doc.doc_no}</span>
        {cancelled && <span className={badge('danger')}>Cancelled</span>}
        {doc.status === 'HOLD' && <span className={badge('warning')}>Held</span>}
      </div>
      <dl className="mb-4 grid grid-cols-2 gap-x-6 gap-y-1.5 sm:grid-cols-3">
        {head.map(([k, label]) => <div key={k}><dt className="text-[10px] font-semibold uppercase tracking-wide text-stone-500">{label}</dt><dd className="text-xs">{String(doc[k])}</dd></div>)}
      </dl>
      <div className="overflow-auto rounded-xl border border-[#E3D9CD] dark:border-[#2A2A2A]">
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">#</th>{shownCols.map(([k, label]) => <th key={k} className={`px-3 py-2 ${NUM.has(k) ? 'text-right' : ''}`}>{label}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {lines.map((l, i) => (
              <tr key={i}>
                <td className="px-3 py-1.5 text-stone-400">{i + 1}</td>
                {shownCols.map(([k]) => <td key={k} className={`px-3 py-1.5 ${NUM.has(k) ? 'text-right tabular-nums' : ''}`}>{MONEY.has(k) ? money(l[k]) : String(l[k] ?? '')}</td>)}
              </tr>
            ))}
            {!lines.length && <tr><td colSpan={shownCols.length + 1} className="px-3 py-6 text-center text-stone-500">No lines.</td></tr>}
          </tbody>
        </table>
      </div>
      {(doc.total !== undefined || totals.net !== undefined || totals.qty !== undefined) && (
        <p className="mt-3 text-right text-xs font-semibold tabular-nums">
          {totals.qty !== undefined && <span className="mr-4">Qty {totals.qty}</span>}
          Total ₹ {money(doc.total ?? totals.net ?? totals.basic)}
        </p>
      )}
    </Modal>
  );
}
