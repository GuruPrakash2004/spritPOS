// Supplier Master form (legacy layout), saved through /masters/suppliers.
// Pick a supplier to edit it (PUT /masters/suppliers/{code}) or "New supplier" to add one (POST).
// Suppliers live in the backend's in-memory store, so, like every master, edits are lost when the
// backend restarts (see the root CLAUDE.md: writes never reach SQL Server).

import { useCallback, useEffect, useState } from 'react';
import { Plus, Save } from 'lucide-react';
import { TextField } from '../common/FormField';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle } from '../../lib/styles';
import { toast } from '../common/toastStore';
import { FormErrorSummary } from '../common/FormErrorSummary';
import { createMaster, errorText, getSuppliers, updateMaster, } from '../../lib/api';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';

const BLANK = {
  code: '', name: '', alias: '', address: '', city: '', pin: '', state: '', country: 'INDIA',
  phone: '', gstin: '', currency: 'Rupees', active: true,
};

const SupplierMaster = () => {
  const { company } = useCompanyProfile();
  const [suppliers, setSuppliers] = useState([]);
  // Code of the supplier being edited; '' while entering a new one.
  const [editing, setEditing] = useState('');
  const [form, setForm] = useState(BLANK);
  const [errors, setErrors] = useState({});
  const [saving, setSaving] = useState(false);

  const load = useCallback((selectCode) => getSuppliers()
    .then(rows => {
      setSuppliers(rows);
      const pick = rows.find(r => r.code === selectCode) ?? (selectCode === undefined ? rows[0] : undefined);
      if (pick) { setEditing(pick.code); setForm(pick); }
    })
    .catch(e => toast.error(errorText(e))), []);

  useEffect(() => { load(); }, [load]);

  const choose = (code) => {
    setErrors({});
    const row = suppliers.find(r => r.code === code);
    setEditing(row ? row.code : '');
    setForm(row ?? { ...BLANK, state: company?.state ?? '' });   // a new supplier starts in the shop's own state
  };

  const handleFieldChange = (key, value) => {
    setForm(prev => ({ ...prev, [key]: value }));
    if (errors[key]) {
      setErrors(prev => {
        const next = { ...prev };
        delete next[key];
        return next;
      });
    }
  };

  const focusField = (key) => {
    const id = `sup-${key}`;
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  const handleSave = async () => {
    if (saving) return;
    const errs = {};
    const code = form.code.trim();
    if (!code) errs.code = 'Enter the Supplier code';
    else if (code !== editing && suppliers.some(r => r.code.toLowerCase() === code.toLowerCase())) errs.code = 'This supplier code is already used';
    if (!form.name.trim()) errs.name = 'Enter the Supplier name';

    if (Object.keys(errs).length > 0) {
      setErrors(errs);
      const first = Object.keys(errs)[0];
      focusField(first);
      return;
    }

    setErrors({});
    setSaving(true);
    const body = { ...form, code, name: form.name.trim() };
    // The save waits for its popup; Cancel stops it.
    if (!(await toast.later('ok', `Supplier "${code}" will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) {
      setSaving(false);
      return;
    }
    const request = editing
      ? updateMaster('suppliers', editing, body)
      : createMaster('suppliers', body);
    request
      .then(saved => { toast.ok(`Supplier "${saved.code}" saved`); return load(saved.code); })
      .catch(e => toast.error(errorText(e)))
      .finally(() => setSaving(false));
  };

  // ERP Keyboard shortcuts
  useEffect(() => {
    const onKey = (e) => {
      const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;
      const modKey = isMac ? e.metaKey : e.ctrlKey;
      if (modKey && e.key.toLowerCase() === 's') {
        e.preventDefault();
        handleSave();
      } else if (e.key === 'Enter') {
        const target = e.target ;
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
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>Supplier Master</h1>
          <p className={pageSub}>Distributor accounts, excise warehouse mappings and GST details.</p>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <label htmlFor="sup-pick" className="sr-only">Supplier to edit</label>
          <div className="w-56">
            <select id="sup-pick" value={editing} onChange={e => choose(e.target.value)} className={field}>
              {!editing && <option value="">New supplier</option>}
              {suppliers.map(r => <option key={r.code} value={r.code}>{r.name} ({r.code})</option>)}
            </select>
          </div>
          <button type="button" onClick={() => choose('')} className={secondaryButton}><Plus size={14} />New</button>
          <button onClick={handleSave} disabled={saving} className={primaryButton}><Save size={14} />{saving ? 'Saving…' : 'Save supplier'}</button>
        </div>
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
            <TextField id="sup-gst" label="GSTIN" value={form.gstin} onChange={e => handleFieldChange('gstin', e.target.value)} className="font-mono" />
            <TextField id="sup-currency" label="Currency" value={form.currency} onChange={e => handleFieldChange('currency', e.target.value)} />
            <label className="flex items-center gap-2 text-xs text-stone-700 dark:text-stone-300">
              <input type="checkbox" checked={form.active} onChange={e => handleFieldChange('active', e.target.checked)} />
              Active (shown in Purchase Bill and Purchase Indent)
            </label>
          </div>
        </section>
      </div>
    </div>
  );
};

export default SupplierMaster;
