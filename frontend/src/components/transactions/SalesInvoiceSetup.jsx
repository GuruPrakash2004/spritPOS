// Sales Invoice step 1: choose the counter (Sales Counter Master), payment type and customer (Customer Master).
// The invoice number shown is the backend's preview for that counter (GET /transactions/next-number); the real
// number is given when the bill is saved. App.jsx keeps the result as `invoiceDraft` and then opens SalesInvoice.
// The System Name is this PC's Windows name (fetched, not typed) and its counter is chosen; a PC with no counter is asked
// to create one. Fast from the keyboard: the customer starts as the
// walk-in CASH customer, and Enter in any field opens the bill.

import { useEffect, useMemo, useState } from 'react';
import { Monitor, Receipt, Save } from 'lucide-react';
import { field, label as labelBase, pageHeader, pageSub, pageTitle, panel, primaryButton, sectionTitle } from '../../lib/styles';
import { errorText, getAbout, getNextNumber, getSalesCounters, getSalesPersons } from '../../lib/api';
import { toastStore } from '../common/toastStore';
import { useScreenFields } from '../../hooks/useScreenFields';
import { applyDefinition } from '../../lib/designerFields';
import Kbd from '../common/Kbd';
import { useConfirm } from '../common/ConfirmDialog';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import {
  counterFromApi,
} from '../../data/salesCounters';

const LAST_COUNTER_KEY = 'vlqms_last_counter';   // this computer's till, a per-browser convenience
const readLastCounter = () => { try { return localStorage.getItem(LAST_COUNTER_KEY) || ''; } catch { return ''; } };
const WALK_IN = 'CASH';

const errorBorder = '!border-red-500 !ring-2 !ring-red-500/20 animate-shake';


// The three boxes of this step take the Doc Designer's caption for TrnHdrSINV's counter / payment / customer columns; they are
// never hidden here (a bill can't be opened without them).
const TYPED = [{ key: 'counter', label: 'Counter' }, { key: 'payment_type', label: 'Payment mode' }, { key: 'customer', label: 'Customer name' }];

const FIELD_IDS = { counter: 'setup-counter', customer: 'setup-customer' };

export default function SalesInvoiceSetup({ initial, onSubmit, onCreateCounter }) {
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [counters, setCounters] = useState(null);
  const [counterCode, setCounterCode] = useState(initial.counter.code || readLastCounter());
  const [invoiceNo, setInvoiceNo] = useState('');
  const [salesPersons, setSalesPersons] = useState([]);
  const [salesPerson, setSalesPerson] = useState(initial.salesPerson || '');
  const [errors, setErrors] = useState({});
  const live = useScreenFields('table:TrnHdrSINV');
  const caption = useMemo(() => Object.fromEntries(applyDefinition(TYPED, live.screen).map(f => [f.key, f.label])), [live.screen]);
  const [sysName, setSysName] = useState('');   // this PC's Windows name

  // The PC's own name (from Windows, found by the server from this browser's address) is the System Name; the counter set up
  // for this PC is chosen at once, and with none for it the desktop's "Sales Counter Not Defined" question is asked.
  useEffect(() => {
    Promise.all([getSalesCounters(), getAbout().then(a => String(a?.client_system_name || a?.system_name || '').toUpperCase()).catch(() => '')])
      .then(([list, name]) => {
        const active = list.map(counterFromApi).filter(c => !c.inactive);
        const forThisPc = active.filter(c => name && c.systemName && c.systemName.toUpperCase() === name);
        setSysName(name);
        setCounters(active);
        // The counter last used on this PC (remembered by the browser) counts as this PC's, so a counter with no System Name doesn't ask every time.
        const usedHere = active.some(c => c.code === readLastCounter());
        if (onCreateCounter && !forThisPc.length && !usedHere) confirm('Sales Counter Not Defined..., Do you want to create new?').then(yes => { if (yes) onCreateCounter(); });
        setCounterCode(code => {
          const last = readLastCounter();
          if (forThisPc.some(c => c.code === code)) return code;
          if (forThisPc.some(c => c.code === last)) return last;
          return forThisPc[0]?.code ?? (active.some(c => c.code === code) ? code : active[0]?.code ?? '');
        });
        setTimeout(() => document.getElementById('setup-counter')?.focus(), 0);   // enabled now: Enter opens the bill
      })
      .catch(e => { setCounters([]); toastStore.add('error', errorText(e)); });
    getSalesPersons()
      .then(list => { setSalesPersons(list); setSalesPerson(p => (list.includes(p) ? p : list[0] ?? '')); })
      .catch(e => toastStore.add('error', errorText(e)));
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
    setErrors(found);
    const first = (Object.keys(FIELD_IDS) ).find(k => found[k]);
    if (first) return document.getElementById(FIELD_IDS[first])?.focus();
    try { localStorage.setItem(LAST_COUNTER_KEY, counter.code); } catch { /* storage blocked */ }
    // Payment mode and customer are chosen on the bill itself (the desktop's setup asks only for the sales person).
    onSubmit({ counter: counter, invoiceNo, paymentType: 'CASH', customer: WALK_IN, salesPerson });
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
              <label htmlFor="setup-counter" className={labelBase}>{caption.counter}</label>
              {/* The till is fixed: it is this PC's counter and is not chosen here */}
              <input id="setup-counter" readOnly aria-readonly="true" value={counters === null ? 'Loading counters…' : counter ? `${counter.name}${counter.name !== counter.code ? ` (${counter.code})` : ''}` : (counters.length ? 'No counter set for this PC' : 'No active counters')}
                className={`${field} cursor-not-allowed bg-subtle ${errors.counter ? errorBorder : ''}`} />
              <InlineError message={errors.counter} />
            </div>
            <div>
              <label htmlFor="setup-sys" className={labelBase}>System name</label>
              <input id="setup-sys" value={sysName || counter?.systemName || ''} readOnly disabled className={`${field} font-mono`} />
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
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The next bill number and who is selling.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="setup-invoice-no" className={labelBase}>Next invoice no.</label>
              <input id="setup-invoice-no" value={invoiceNo} readOnly disabled placeholder="Given when the bill is saved" className={`${field} font-mono`} />
            </div>
            <div>
              <label htmlFor="setup-salesperson" className={labelBase}>Sales person</label>
              <select id="setup-salesperson" value={salesPerson} onChange={e => setSalesPerson(e.target.value)} className={field}>
                {!salesPersons.length && <option value="">No sales persons (Sales Person Master)</option>}
                {salesPersons.map(p => <option key={p}>{p}</option>)}
              </select>
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
