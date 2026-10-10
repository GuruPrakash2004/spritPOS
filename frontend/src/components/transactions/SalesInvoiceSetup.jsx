// Sales Invoice step 1: choose the counter (Sales Counter Master), payment type and customer (Customer Master).
// The invoice number shown is the backend's preview for that counter (GET /transactions/next-number); the real
// number is given when the bill is saved. App.jsx keeps the result as `invoiceDraft` and then opens SalesInvoice.
// Fast from the keyboard: the counter used last on this computer is chosen again, the customer starts as the
// walk-in CASH customer, and Enter in any field opens the bill.

import { useEffect, useState } from 'react';
import { Monitor, Receipt, Save } from 'lucide-react';
import { field, label as labelBase, pageHeader, pageSub, pageTitle, panel, primaryButton, sectionTitle } from '../../lib/styles';
import { errorText, getAbout, getCustomers, getNextNumber, getSalesCounters, } from '../../lib/api';
import { toastStore } from '../common/toastStore';
import Kbd from '../common/Kbd';
import { useConfirm } from '../common/ConfirmDialog';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import {
  PAYMENT_TYPES, counterFromApi,

} from '../../data/salesCounters';

const LAST_COUNTER_KEY = 'vlqms_last_counter';   // this computer's till, a per-browser convenience
const readLastCounter = () => { try { return localStorage.getItem(LAST_COUNTER_KEY) || ''; } catch { return ''; } };
const WALK_IN = 'CASH';

const errorBorder = '!border-red-500 !ring-2 !ring-red-500/20 animate-shake';


const FIELD_IDS = { counter: 'setup-counter', customer: 'setup-customer' };

export default function SalesInvoiceSetup({ initial, onSubmit, onCreateCounter }) {
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [counters, setCounters] = useState(null);
  const [customers, setCustomers] = useState([]);
  const [counterCode, setCounterCode] = useState(initial.counter.code || readLastCounter());
  const [invoiceNo, setInvoiceNo] = useState('');
  const [paymentType, setPaymentType] = useState(initial.paymentType);
  const [customer, setCustomer] = useState(initial.customer || WALK_IN);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    getSalesCounters()
      .then(list => {
        const active = list.map(counterFromApi).filter(c => !c.inactive);
        setCounters(active);
        // Like the desktop: no counter yet → ask, and Yes opens Sales Counter Master to create one.
        // "Not defined" = none at all, or none for this system (the counter last used here counts as this system's).
        if (onCreateCounter) {
          const mine = active.some(c => c.code === readLastCounter());
          (mine ? Promise.resolve(true) : active.length ? getAbout().then(a => active.some(c => c.systemName && c.systemName.toUpperCase() === String(a?.system_name ?? '').toUpperCase())).catch(() => true) : Promise.resolve(false))
            .then(defined => defined || confirm('Sales Counter Not Defined..., Do you want to create new?').then(yes => { if (yes) onCreateCounter(); }));
        }
        setCounterCode(code => (active.some(c => c.code === code) ? code : active[0]?.code ?? ''));
        setTimeout(() => document.getElementById('setup-counter')?.focus(), 0);   // enabled now: Enter opens the bill
      })
      .catch(e => { setCounters([]); toastStore.add('error', errorText(e)); });
    getCustomers().then(setCustomers).catch(e => toastStore.add('error', errorText(e)));
  }, []);   // eslint-disable-line react-hooks/exhaustive-deps

  const counter = counters?.find(c => c.code === counterCode);

  useEffect(() => {
    if (!counterCode) return;
    let live = true;
    getNextNumber('sales_invoice', counterCode)
      .then(n => { if (live) setInvoiceNo(n.doc_no); })
      .catch(() => { if (live) setInvoiceNo(''); });
    return () => { live = false; };
  }, [counterCode]);

  const handleSave = () => {
    const found = {};
    if (!counter) found.counter = counters?.length ? 'Choose the sales counter' : 'Add a sales counter in Sales Counter Master first';
    if (!customer.trim()) found.customer = 'Enter the customer name';
    setErrors(found);
    const first = (Object.keys(FIELD_IDS) ).find(k => found[k]);
    if (first) return document.getElementById(FIELD_IDS[first])?.focus();
    try { localStorage.setItem(LAST_COUNTER_KEY, counter.code); } catch { /* storage blocked */ }
    onSubmit({ counter: counter, invoiceNo, paymentType, customer: customer.trim() });
  };

  return (
    <div className="mx-auto max-w-4xl">
      {confirmDialog}
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>New sales bill</h1>
          <p className={pageSub}>Choose the sales counter and the invoice details, then press Enter to open the bill.</p>
        </div>
      </div>

      <FormErrorSummary errors={Object.fromEntries(Object.entries(errors).filter(([, m]) => m)) }
        onFocusField={k => document.getElementById(FIELD_IDS[k ])?.focus()} />

      <div className={`${panel} divide-y divide-line`} onKeyDown={e => {
        if (e.key === 'Enter' && !(e.target instanceof HTMLButtonElement)) { e.preventDefault(); handleSave(); }
      }}>
        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={`${sectionTitle} flex items-center gap-2`}><Monitor size={16} className="text-stone-400" />Sales counter</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The till this bill is rung up on. Its prefix sets the bill number series.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div className="sm:col-span-2">
              <label htmlFor="setup-counter" className={labelBase}>Counter</label>
              <select id="setup-counter" aria-invalid={!!errors.counter || undefined} value={counterCode} disabled={counters === null}
                onChange={e => { setCounterCode(e.target.value); setErrors(x => ({ ...x, counter: undefined })); }}
                className={`${field} ${errors.counter ? errorBorder : ''}`}>
                {counters === null && <option value="">Loading counters…</option>}
                {counters?.length === 0 && <option value="">No active counters</option>}
                {counters?.map(c => <option key={c.code} value={c.code}>{c.name}{c.name !== c.code ? ` (${c.code})` : ''}</option>)}
              </select>
              <InlineError message={errors.counter} />
            </div>
            <div>
              <label htmlFor="setup-sys" className={labelBase}>System name</label>
              <input id="setup-sys" value={counter?.systemName ?? ''} readOnly disabled className={`${field} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-prefix" className={labelBase}>Bill prefix</label>
              <input id="setup-prefix" value={counter?.prefix ?? ''} readOnly disabled className={`${field} font-mono`} />
            </div>
          </div>
        </section>

        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={`${sectionTitle} flex items-center gap-2`}><Receipt size={16} className="text-stone-400" />Invoice</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The next bill number, payment mode and customer.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="setup-invoice-no" className={labelBase}>Next invoice no.</label>
              <input id="setup-invoice-no" value={invoiceNo} readOnly disabled placeholder="Given when the bill is saved" className={`${field} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-payment" className={labelBase}>Payment mode</label>
              <select id="setup-payment" value={paymentType} onChange={e => setPaymentType(e.target.value )} className={field}>
                {PAYMENT_TYPES.map(p => <option key={p}>{p}</option>)}
              </select>
            </div>
            <div className="sm:col-span-2">
              <label htmlFor="setup-customer" className={labelBase}>Customer name</label>
              <input id="setup-customer" aria-invalid={!!errors.customer || undefined} list="setup-parties" value={customer}
                onChange={e => { setCustomer(e.target.value); setErrors(x => ({ ...x, customer: undefined })); }}
                placeholder="Enter customer name…" className={`${field} ${errors.customer ? errorBorder : ''}`} />
              <datalist id="setup-parties">{customers.map(c => <option key={c.code} value={c.name} />)}</datalist>
              <InlineError message={errors.customer} />
            </div>
          </div>
        </section>

        <div className="flex justify-end bg-subtle/60 px-6 py-4">
          <button onClick={handleSave} className={primaryButton}><Save size={14} />Open bill<Kbd combo="Enter" /></button>
        </div>
      </div>
    </div>
  );
}
