// Sales Invoice step 1: choose counter, invoice number, payment type and customer.
// App.tsx keeps the result as `invoiceDraft` and then opens SalesInvoice.

import { useState } from 'react';
import { AlertTriangle, Monitor, Receipt, Save } from 'lucide-react';
import { field, label as labelBase, pageHeader, pageSub, pageTitle, panel, primaryButton, sectionTitle } from '../../lib/styles';
import {
  FIRST_BILL_SEQ, PARTIES, PAYMENT_TYPES, billNumber,
  type InvoiceSetup, type PaymentType, type SalesCounter,
} from '../../data/salesCounters';

const inputClass = field;
const okBorder = '';
const errorBorder = '!border-red-500 focus:!ring-red-500/20';
const labelClass = labelBase;

type FieldKey = 'code' | 'name' | 'systemName' | 'prefix' | 'invoiceNo' | 'customer';

const FIELD_LABELS: Record<FieldKey, string> = {
  code: 'SC Code', name: 'SC Name', systemName: 'System Name', prefix: 'Prefix', invoiceNo: 'Invoice No', customer: 'Customer Name',
};

export default function SalesInvoiceSetup({ initial, onSubmit }: { initial: InvoiceSetup; onSubmit: (setup: InvoiceSetup) => void }) {
  const [counter, setCounter] = useState<SalesCounter>(initial.counter);
  const [invoiceNo, setInvoiceNo] = useState(initial.invoiceNo);
  // Until the user types their own Invoice No, it follows the counter prefix.
  const [invoiceNoEdited, setInvoiceNoEdited] = useState(initial.invoiceNo !== billNumber(initial.counter.prefix, FIRST_BILL_SEQ));
  const [paymentType, setPaymentType] = useState<PaymentType>(initial.paymentType);
  const [customer, setCustomer] = useState(initial.customer);
  const [missing, setMissing] = useState<FieldKey[]>([]);
  const [error, setError] = useState('');

  const effectiveInvoiceNo = invoiceNoEdited ? invoiceNo : counter.prefix.trim() ? billNumber(counter.prefix.trim(), FIRST_BILL_SEQ) : '';
  const values: Record<FieldKey, string> = { ...counter, invoiceNo: effectiveInvoiceNo, customer };

  const updateCounter = <K extends keyof SalesCounter>(key: K, value: SalesCounter[K]) => setCounter(prev => ({ ...prev, [key]: value }));
  const border = (key: FieldKey) => (missing.includes(key) && !values[key].trim() ? errorBorder : okBorder);

  const handleSave = () => {
    const empty = (Object.keys(FIELD_LABELS) as FieldKey[]).filter(key => !values[key].trim());
    setMissing(empty);
    if (empty.length) return setError(`Please fill in: ${empty.map(key => FIELD_LABELS[key]).join(', ')}.`);
    if (counter.inactive) return setError('This counter is marked Inactive. Untick Inactive to open a sales bill on it.');
    setError('');
    onSubmit({
      counter: { ...counter, code: counter.code.trim(), name: counter.name.trim(), systemName: counter.systemName.trim(), prefix: counter.prefix.trim() },
      invoiceNo: effectiveInvoiceNo.trim(),
      paymentType,
      customer: customer.trim(),
    });
  };

  const invalid = (key: FieldKey) => missing.includes(key) || undefined;

  return (
    <div className="mx-auto max-w-4xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>New sales bill</h1>
          <p className={pageSub}>Fill in the sales counter and invoice details, then Save Master to open the bill.</p>
        </div>
      </div>

      {error && (
        <div role="alert" className="mb-4 flex items-center gap-2 rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-2.5 text-xs font-medium text-red-700 dark:text-red-300">
          <AlertTriangle size={14} className="shrink-0" /> {error}
        </div>
      )}

      <div className={`${panel} divide-y divide-line`}>
        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={`${sectionTitle} flex items-center gap-2`}><Monitor size={16} className="text-stone-400" />Sales counter</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The till this bill is rung up on. The prefix sets the bill number series.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="setup-sc-code" className={labelClass}>SC code</label>
              <input id="setup-sc-code" aria-invalid={invalid('code')} value={counter.code} onChange={e => updateCounter('code', e.target.value)} placeholder="e.g. counter10" className={`${inputClass} ${border('code')} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-sc-name" className={labelClass}>SC name</label>
              <input id="setup-sc-name" aria-invalid={invalid('name')} value={counter.name} onChange={e => updateCounter('name', e.target.value)} placeholder="e.g. counter10" className={`${inputClass} ${border('name')}`} />
            </div>
            <div>
              <label htmlFor="setup-sys" className={labelClass}>System name</label>
              <input id="setup-sys" aria-invalid={invalid('systemName')} value={counter.systemName} onChange={e => updateCounter('systemName', e.target.value.toUpperCase())} placeholder="e.g. PRADEEP" className={`${inputClass} ${border('systemName')} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-prefix" className={labelClass}>Bill prefix</label>
              <input id="setup-prefix" aria-invalid={invalid('prefix')} value={counter.prefix} onChange={e => updateCounter('prefix', e.target.value)} placeholder="e.g. c10" className={`${inputClass} ${border('prefix')} font-mono`} />
            </div>
            <label className="flex items-center gap-2 text-xs text-stone-700 sm:col-span-2 dark:text-stone-300">
              <input type="checkbox" checked={counter.inactive} onChange={e => updateCounter('inactive', e.target.checked)} className="h-4 w-4 accent-accent" />
              Counter is inactive
            </label>
          </div>
        </section>

        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={`${sectionTitle} flex items-center gap-2`}><Receipt size={16} className="text-stone-400" />Invoice</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The starting bill number, payment mode and customer.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="setup-invoice-no" className={labelClass}>Invoice no.</label>
              <input id="setup-invoice-no" aria-invalid={invalid('invoiceNo')} value={effectiveInvoiceNo} onChange={e => { setInvoiceNoEdited(true); setInvoiceNo(e.target.value); }} placeholder="Fills in from the prefix" className={`${inputClass} ${border('invoiceNo')} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-counter" className={labelClass}>Counter</label>
              <input id="setup-counter" value={counter.code} readOnly disabled className={`${inputClass} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-payment" className={labelClass}>Payment mode</label>
              <select id="setup-payment" value={paymentType} onChange={e => setPaymentType(e.target.value as PaymentType)} className={inputClass}>
                {PAYMENT_TYPES.map(p => <option key={p}>{p}</option>)}
              </select>
            </div>
            <div>
              <label htmlFor="setup-customer" className={labelClass}>Customer name</label>
              <input id="setup-customer" aria-invalid={invalid('customer')} list="setup-parties" value={customer} onChange={e => setCustomer(e.target.value)} placeholder="Enter customer name…" className={`${inputClass} ${border('customer')}`} />
              <datalist id="setup-parties">{PARTIES.map(p => <option key={p} value={p} />)}</datalist>
            </div>
          </div>
        </section>

        <div className="flex justify-end bg-subtle/60 px-6 py-4">
          <button onClick={handleSave} className={primaryButton}><Save size={14} />Save Master</button>
        </div>
      </div>
    </div>
  );
}
