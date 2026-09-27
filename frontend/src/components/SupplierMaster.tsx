import { useState, useEffect } from 'react';
import { Save } from 'lucide-react';
import { TextField } from './FormField';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, sectionTitle } from './ui';
import Toast from './Toast';
import { FormErrorSummary } from './FormErrorSummary';

const SupplierMaster = () => {
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [form, setForm] = useState({
    code: 'R&D DISTRIBUTORS',
    name: 'R&D DISTRIBUTORS',
    alias: 'R&D DISTRIBUTOR',
    address: '460, 1ST MAIN ROAD, ALLASANDRA, YELAHANKA',
    city: 'Bangalore',
    pin: '560064',
    state: 'Karnataka',
    country: 'INDIA',
    phone: '9900196257',
    gst: '29AABCR1234F1Z1',
    currency: 'Rupees',
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleFieldChange = (key: string, value: string) => {
    setForm(prev => ({ ...prev, [key]: value }));
    if (errors[key]) {
      setErrors(prev => {
        const next = { ...prev };
        delete next[key];
        return next;
      });
    }
  };

  const focusField = (key: string) => {
    const id = `sup-${key}`;
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  const handleSave = () => {
    const errs: Record<string, string> = {};
    if (!form.code.trim()) errs.code = 'Enter the Supplier code';
    if (!form.name.trim()) errs.name = 'Enter the Supplier name';

    if (Object.keys(errs).length > 0) {
      setErrors(errs);
      const first = Object.keys(errs)[0];
      focusField(first);
      return;
    }

    setErrors({});
    setToast({ kind: 'ok', text: `Supplier "${form.code}" saved successfully!` });
  };

  // ERP Keyboard shortcuts
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;
      const modKey = isMac ? e.metaKey : e.ctrlKey;
      if (modKey && e.key.toLowerCase() === 's') {
        e.preventDefault();
        handleSave();
      } else if (e.key === 'Enter') {
        const target = e.target as HTMLElement;
        if (target && target.tagName === 'INPUT') {
          e.preventDefault();
          handleSave();
        }
      }
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  });

  return (
    <div className="mx-auto max-w-5xl">
      {toast && <Toast kind={toast.kind} text={toast.text} />}

      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>Supplier Master</h1>
          <p className={pageSub}>Distributor accounts, excise warehouse mappings and GST details.</p>
        </div>
        <button onClick={handleSave} className={primaryButton}><Save size={14} />Save supplier</button>
      </div>

      <div className={`${panel} divide-y divide-line p-6`}>
        {/* Amazon-style "There was a problem" alert summary */}
        <FormErrorSummary errors={errors} onFocusField={focusField} />

        <section className="grid gap-6 pb-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Supplier</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">Code, name and the alias used when exporting to Tally.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <TextField
              id="sup-code"
              label="Supplier code"
              value={form.code}
              required
              hasError={!!errors.code}
              errorMessage={errors.code}
              onChange={e => handleFieldChange('code', e.target.value)}
              className="font-mono"
            />
            <TextField
              id="sup-name"
              label="Supplier name"
              value={form.name}
              required
              hasError={!!errors.name}
              errorMessage={errors.name}
              onChange={e => handleFieldChange('name', e.target.value)}
            />
            <TextField
              id="sup-alias"
              label="Tally alias"
              value={form.alias}
              onChange={e => handleFieldChange('alias', e.target.value)}
              wide
            />
          </div>
        </section>

        <section className="grid gap-6 py-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Billing & depot address</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">Printed on purchase bills and indents.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div className="sm:col-span-2">
              <label htmlFor="sup-address" className={label}>Address</label>
              <textarea
                id="sup-address"
                rows={3}
                value={form.address}
                onChange={e => handleFieldChange('address', e.target.value)}
                className={field}
              />
            </div>
            <TextField id="sup-city" label="City" value={form.city} onChange={e => handleFieldChange('city', e.target.value)} />
            <TextField id="sup-pin" label="Pincode" value={form.pin} onChange={e => handleFieldChange('pin', e.target.value)} className="tabular-nums" />
            <TextField id="sup-state" label="State" value={form.state} onChange={e => handleFieldChange('state', e.target.value)} />
            <TextField id="sup-country" label="Country" value={form.country} onChange={e => handleFieldChange('country', e.target.value)} />
          </div>
        </section>

        <section className="grid gap-6 pt-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Contact & tax</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">GSTIN and the currency the supplier bills in.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <TextField id="sup-phone" label="Phone no." value={form.phone} onChange={e => handleFieldChange('phone', e.target.value)} className="tabular-nums" />
            <TextField id="sup-gst" label="GSTIN" value={form.gst} onChange={e => handleFieldChange('gst', e.target.value)} className="font-mono" />
            <TextField id="sup-currency" label="Currency" value={form.currency} onChange={e => handleFieldChange('currency', e.target.value)} />
          </div>
        </section>
      </div>
    </div>
  );
};

export default SupplierMaster;
