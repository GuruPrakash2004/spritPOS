// The three stock transactions built on ItemLinesEntry:
// - PurchaseIndent: order to a supplier; no stock effect; later copied into a Purchase Bill.
// - SalesReturn: optionally against a saved invoice; adds stock.
// - StockDeduction: breakage/leakage/sample/other; reduces stock.

import { useEffect, useState } from 'react';
import ItemLinesEntry from './ItemLinesEntry';
import { api, todayIso, type ApiInvoice, type ApiSupplier } from '../../lib/api';
import { field, label } from '../../lib/styles';
import { useDocSettings } from '../../lib/docSettings';

// The three stock transactions built on ItemLinesEntry. Each only adds its own header fields.

const toLines = (lines: { item_code: string; cases: number; bottles: number; rate: number }[]) =>
  lines.map(l => ({ item_code: l.item_code, cases: l.cases, bottles: l.bottles, rate: l.rate }));

// Does not affect stock; its lines are copied into a Purchase Bill with Copy From.
export function PurchaseIndent() {
  const [date, setDate] = useState(todayIso());
  // Transaction Settings: when the date can't be changed, it is today's.
  const dateLocked = !useDocSettings('purchase_indent').allow_edit_date;
  const [supplier, setSupplier] = useState('KSBCL');
  const [suppliers, setSuppliers] = useState<ApiSupplier[]>([]);
  const [remarks, setRemarks] = useState('');

  useEffect(() => { api<ApiSupplier[]>('/masters/suppliers').then(setSuppliers).catch(() => undefined); }, []);

  return (
    <ItemLinesEntry
      title="Purchase Indent"
      subtitle="Order raised on KSBCL. It does not change stock; copy it into a Purchase Bill when the goods arrive."
      endpoint="/transactions/purchase-indent"
      rateField="purchase_rate"
      rateLabel="PB Rate"
      buildBody={lines => ({ date: dateLocked ? todayIso() : date, supplier, remarks, lines: toLines(lines) })}
      header={<>
        <div><label htmlFor="pi-date" className={label}>Indent Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><input id="pi-date" type="date" value={dateLocked ? todayIso() : date} disabled={dateLocked} onChange={e => setDate(e.target.value)} className={`${field} font-mono`} /></div>
        <div className="w-full min-w-0 sm:w-auto">
          <label htmlFor="pi-sup" className={label}>Supplier<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <select id="pi-sup" value={supplier} onChange={e => setSupplier(e.target.value)} className={`${field} w-full sm:w-80`}>
            {suppliers.map(s => <option key={s.code} value={s.code}>{s.name}</option>)}
          </select>
        </div>
        <div className="w-full min-w-0 sm:w-auto sm:flex-1"><label htmlFor="pi-rmk" className={label}>Remarks</label><input id="pi-rmk" value={remarks} onChange={e => setRemarks(e.target.value)} className={`${field} w-full`} /></div>
      </>}
    />
  );
}

// Adds stock back; optionally against the original invoice.
export function SalesReturn() {
  const [date, setDate] = useState(todayIso());
  // Transaction Settings: when the date can't be changed, it is today's.
  const dateLocked = !useDocSettings('sales_return').allow_edit_date;
  const [invoices, setInvoices] = useState<ApiInvoice[]>([]);
  const [against, setAgainst] = useState('');
  const [remarks, setRemarks] = useState('');

  useEffect(() => {
    api<ApiInvoice[]>('/transactions/sales-invoice?status=SAVED').then(setInvoices).catch(() => undefined);
  }, []);

  const invoice = invoices.find(i => i.doc_no === against);

  return (
    <ItemLinesEntry
      title="Sales Return"
      subtitle="Bottles returned by a customer go back into stock."
      endpoint="/transactions/sales-return"
      rateField="mrp"
      rateLabel="Rate"
      buildBody={lines => ({ date: dateLocked ? todayIso() : date, against_invoice: against || null, customer: invoice?.customer ?? 'CASH', remarks, lines: toLines(lines) })}
      header={<>
        <div><label htmlFor="sr-date" className={label}>Return Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><input id="sr-date" type="date" value={dateLocked ? todayIso() : date} disabled={dateLocked} onChange={e => setDate(e.target.value)} className={`${field} font-mono`} /></div>
        <div className="w-full min-w-0 sm:w-auto">
          <label htmlFor="sr-inv" className={label}>Against Invoice</label>
          <select id="sr-inv" value={against} onChange={e => setAgainst(e.target.value)} className={`${field} w-full font-mono sm:w-72`}>
            <option value="">— none —</option>
            {invoices.map(i => <option key={i.doc_no} value={i.doc_no}>{i.doc_no} • {i.customer}</option>)}
          </select>
        </div>
        {invoice && <p className="self-center text-xs text-stone-500">Bill had: {invoice.lines.map(l => `${l.item_code} × ${l.qty}`).join(', ')}</p>}
        <div className="w-full min-w-0 sm:w-auto sm:flex-1"><label htmlFor="sr-rmk" className={label}>Reason</label><input id="sr-rmk" value={remarks} onChange={e => setRemarks(e.target.value)} className={`${field} w-full`} /></div>
      </>}
    />
  );
}

const REASONS = ['Breakage', 'Leakage', 'Sample', 'Other'];

// Reduces stock for breakage, leakage and samples.
export function StockDeduction() {
  const [date, setDate] = useState(todayIso());
  // Transaction Settings: when the date can't be changed, it is today's.
  const dateLocked = !useDocSettings('stock_deduction').allow_edit_date;
  const [reason, setReason] = useState(REASONS[0]);
  const [remarks, setRemarks] = useState('');

  return (
    <ItemLinesEntry
      title="Stock Deduction"
      subtitle="Excise breakage, spillage and damage adjustments. Reduces stock."
      endpoint="/transactions/stock-deduction"
      rateField="purchase_rate"
      rateLabel="Purchase Rate"
      buildBody={lines => ({ date: dateLocked ? todayIso() : date, reason, remarks, lines: toLines(lines) })}
      header={<>
        <div><label htmlFor="sd-date" className={label}>Date<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label><input id="sd-date" type="date" value={dateLocked ? todayIso() : date} disabled={dateLocked} onChange={e => setDate(e.target.value)} className={`${field} font-mono`} /></div>
        <div>
          <label htmlFor="sd-reason" className={label}>Reason<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
          <select id="sd-reason" value={reason} onChange={e => setReason(e.target.value)} className={field}>{REASONS.map(r => <option key={r}>{r}</option>)}</select>
        </div>
        <div className="w-full min-w-0 sm:w-auto sm:flex-1"><label htmlFor="sd-rmk" className={label}>Remarks</label><input id="sd-rmk" value={remarks} onChange={e => setRemarks(e.target.value)} className={`${field} w-full`} placeholder="e.g. unloading crate breakage" /></div>
      </>}
    />
  );
}
