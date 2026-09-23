import { useState } from 'react';
import { AlertTriangle, Monitor, Receipt, Save } from 'lucide-react';
import {
  FIRST_BILL_SEQ, PARTIES, PAYMENT_TYPES, billNumber,
  type InvoiceSetup, type PaymentType, type SalesCounter,
} from './salesCounters';

const inputClass = 'w-full rounded-xl border bg-[#fffaf5] px-3 py-2 text-xs text-stone-800 outline-none focus:border-[#e8702a] dark:bg-[#20253a] dark:text-stone-100';
const okBorder = 'border-[#e1c9b6] dark:border-[#414a6e]';
const errorBorder = 'border-red-400 dark:border-red-700';
const labelClass = 'mb-1 block text-xs font-semibold text-stone-600 dark:text-stone-300';

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

  return (
    <div className="mx-auto max-w-5xl space-y-5 rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] p-6 shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]">
      <div>
        <h2 className="text-xl font-bold text-stone-900 dark:text-stone-100">Sales Invoice POS</h2>
        <p className="text-xs text-stone-500">Fill in the sales counter and invoice details, then Save Master to open the bill.</p>
      </div>

      {error && (
        <div className="flex items-center gap-2 rounded-xl border border-red-200 bg-red-50 px-4 py-2 text-xs font-medium text-red-700 dark:border-red-900 dark:bg-red-950/40 dark:text-red-300">
          <AlertTriangle size={14} /> {error}
        </div>
      )}

      <section className="space-y-3">
        <h3 className="flex items-center gap-2 text-sm font-bold text-[#b84716] dark:text-[#ffb07e]"><Monitor size={16} />Sales Counter</h3>
        <div className="grid grid-cols-1 gap-4 lg:grid-cols-2">
          <div className="space-y-3 rounded-xl border border-[#e1c9b6] bg-[#f6efe8]/60 p-4 dark:border-[#343b59] dark:bg-[#1b2033]">
            <div>
              <label htmlFor="setup-sc-code" className={labelClass}>SC Code</label>
              <input id="setup-sc-code" value={counter.code} onChange={e => updateCounter('code', e.target.value)} placeholder="e.g. counter10" className={`${inputClass} ${border('code')} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-sc-name" className={labelClass}>SC Name</label>
              <input id="setup-sc-name" value={counter.name} onChange={e => updateCounter('name', e.target.value)} placeholder="e.g. counter10" className={`${inputClass} ${border('name')}`} />
            </div>
            <label className="flex items-center gap-2 text-xs text-stone-600 dark:text-stone-300">
              <input type="checkbox" checked={counter.inactive} onChange={e => updateCounter('inactive', e.target.checked)} className="accent-[#e8702a]" />
              Inactive
            </label>
          </div>

          <div className="rounded-xl border border-[#e1c9b6] bg-[#f6efe8]/60 p-4 dark:border-[#343b59] dark:bg-[#1b2033]">
            <span className="inline-block rounded-t-lg border border-b-0 border-[#e1c9b6] bg-[#fffaf5] px-3 py-1 text-xs font-bold text-stone-700 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-200">Header Info</span>
            <table className="w-full border border-[#e1c9b6] text-xs dark:border-[#414a6e]">
              <tbody>
                <tr className="border-b border-[#e1c9b6] dark:border-[#414a6e]">
                  <th className="w-32 bg-[#f1e3d8] px-3 py-2 text-left font-semibold text-stone-600 dark:bg-[#20253a] dark:text-stone-300">System Name</th>
                  <td className="p-1.5"><input value={counter.systemName} onChange={e => updateCounter('systemName', e.target.value.toUpperCase())} placeholder="e.g. PRADEEP" aria-label="System Name" className={`${inputClass} ${border('systemName')} font-mono`} /></td>
                </tr>
                <tr>
                  <th className="bg-[#f1e3d8] px-3 py-2 text-left font-semibold text-stone-600 dark:bg-[#20253a] dark:text-stone-300">Prefix</th>
                  <td className="bg-[#e8702a]/15 p-1.5"><input value={counter.prefix} onChange={e => updateCounter('prefix', e.target.value)} placeholder="e.g. c10" aria-label="Prefix" className={`${inputClass} ${border('prefix')} font-mono font-bold text-[#b84716]`} /></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </section>

      <section className="space-y-3">
        <h3 className="flex items-center gap-2 text-sm font-bold text-[#b84716] dark:text-[#ffb07e]"><Receipt size={16} />Invoice</h3>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <label htmlFor="setup-invoice-no" className={labelClass}>Invoice No</label>
            <input id="setup-invoice-no" value={effectiveInvoiceNo} onChange={e => { setInvoiceNoEdited(true); setInvoiceNo(e.target.value); }} placeholder="Fills in from the prefix" className={`${inputClass} ${border('invoiceNo')} font-mono`} />
          </div>
          <div>
            <label htmlFor="setup-counter" className={labelClass}>Counter</label>
            <input id="setup-counter" value={counter.code} readOnly className={`${inputClass} ${okBorder} bg-stone-100 font-mono text-stone-500 dark:bg-[#1b2033] dark:text-stone-400`} />
          </div>
          <div>
            <label htmlFor="setup-payment" className={labelClass}>Payment Mode</label>
            <select id="setup-payment" value={paymentType} onChange={e => setPaymentType(e.target.value as PaymentType)} className={`${inputClass} ${okBorder} font-bold`}>
              {PAYMENT_TYPES.map(p => <option key={p}>{p}</option>)}
            </select>
          </div>
          <div>
            <label htmlFor="setup-customer" className={labelClass}>Customer Name</label>
            <input id="setup-customer" list="setup-parties" value={customer} onChange={e => setCustomer(e.target.value)} placeholder="Enter Customer Name..." className={`${inputClass} ${border('customer')}`} />
            <datalist id="setup-parties">{PARTIES.map(p => <option key={p} value={p} />)}</datalist>
          </div>
        </div>
      </section>

      <div className="flex justify-end">
        <button onClick={handleSave} className="flex items-center gap-1.5 rounded-xl bg-[#e8702a] px-5 py-2 text-xs font-bold text-white shadow-sm hover:bg-[#b84716]"><Save size={14} />Save Master</button>
      </div>
    </div>
  );
}
