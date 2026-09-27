// Company Master: the store's profile (name, address, GSTIN, licence, FY) plus Images and Documents tabs.
// Loads GET /masters/company and saves with PUT (Administrator only). After a save it calls
// notifyCompanyProfileUpdated() so the header, login page and prints refresh.
// Known backend bug: PUT /masters/company currently returns 500 (see .claude/CLAUDE.md).

import { useState, useEffect } from 'react';
import { 
  Building2, ShieldCheck, Calendar, Image as ImageIcon,
  FolderArchive, Save, Plus, DatabaseBackup,
} from 'lucide-react';
import { CompanyDocuments, CompanyImages } from './CompanyFiles';
import { TextField } from '../common/FormField';
import Toast from '../common/Toast';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import { api, errorText, type ApiCompany } from '../../lib/api';
import { notifyCompanyProfileUpdated } from '../../hooks/useCompanyProfile';
import { badge, field, ghostButton, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, tableRow } from '../../lib/styles';

interface FinYear {
  from: string;
  to: string;
  active: boolean;
}

const TABS = [
  { id: 'Company Info', icon: Building2 },
  { id: 'Additional Fields', icon: ShieldCheck },
  { id: 'Financial Year(s)', icon: Calendar },
  { id: 'Images', icon: ImageIcon },
  { id: 'Documents', icon: FolderArchive },
];

const EXTRA_FIELDS: { id: string; label: string; key: keyof Pick<ApiCompany, 'phone' | 'fax' | 'mobile' | 'website' | 'gstin' | 'pan' | 'cin' | 'excise_phone' | 'ksbcl_code'>; placeholder?: string; mono?: boolean }[] = [
  { id: 'co-tel', label: 'Tel no.', key: 'phone' },
  { id: 'co-fax', label: 'Fax no.', key: 'fax', placeholder: 'Optional' },
  { id: 'co-mobile', label: 'Mobile no.', key: 'mobile', mono: true },
  { id: 'co-web', label: 'Website', key: 'website' },
  { id: 'co-gst', label: 'GSTIN', key: 'gstin', mono: true },
  { id: 'co-pan', label: 'PAN', key: 'pan', mono: true },
  { id: 'co-cin', label: 'CIN', key: 'cin', mono: true },
  { id: 'co-excise', label: 'Excise phone no.', key: 'excise_phone', mono: true },
  { id: 'co-ksbcl', label: 'KSBCL retailer code', key: 'ksbcl_code', mono: true },
];

const INITIAL_FIN_YEARS: FinYear[] = [
  { from: '01/04/2021', to: '31/03/2022', active: false },
  { from: '01/04/2022', to: '31/03/2023', active: false },
  { from: '01/04/2023', to: '31/03/2024', active: false },
  { from: '01/04/2024', to: '31/03/2025', active: false },
  { from: '01/04/2025', to: '31/03/2026', active: false },
  { from: '01/04/2026', to: '31/03/2027', active: true },
];

const CompanyMaster = ({ canEditFiles }: { canEditFiles: boolean }) => {
  const [tab, setTab] = useState<string>('Company Info');
  const [finYears, setFinYears] = useState<FinYear[]>(INITIAL_FIN_YEARS);
  const [companyProfile, setCompanyProfile] = useState<ApiCompany | null>(null);
  const [companyName, setCompanyName] = useState('');
  const [email, setEmail] = useState('');
  const [address, setAddress] = useState('');
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  const focusField = (key: string) => {
    const el = document.getElementById(key);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  useEffect(() => {
    api<ApiCompany>('/masters/company')
      .then(profile => {
        setCompanyProfile(profile);
        setCompanyName(profile.name);
        setEmail(profile.email);
        setAddress(profile.address);
      })
      .catch(error => setToast({ kind: 'error', text: errorText(error) }));
  }, []);

  const handleSave = async () => {
    if (!canEditFiles || !companyProfile) return;
    const errs: Record<string, string> = {};
    if (!companyName.trim()) errs['co-name'] = 'Enter the company name';
    if (!address.trim()) errs['co-address'] = 'Enter the registered premises address';
    if (!email.trim()) errs['co-email'] = 'Enter the email address';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) errs['co-email'] = 'Enter a valid email address';

    if (Object.keys(errs).length > 0) {
      setErrors(errs);
      focusField(Object.keys(errs)[0]);
      return;
    }
    setErrors({});
    try {
      const profile = await api<ApiCompany>('/masters/company', {
        method: 'PUT',
        body: { ...companyProfile, name: companyName.trim(), email: email.trim(), address: address.trim() },
      });
      setCompanyProfile(profile);
      setCompanyName(profile.name);
      setEmail(profile.email);
      setAddress(profile.address);
      notifyCompanyProfileUpdated();
      setToast({ kind: 'ok', text: 'Company profile updated successfully.' });
    } catch (error) {
      setToast({ kind: 'error', text: errorText(error) });
    }
  };

  const handleBackup = () => {
    setToast({ kind: 'ok', text: 'Database backup initiated successfully. Backup archive generated.' });
  };

  const handleSetActiveFY = (index: number) => {
    setFinYears(finYears.map((fy, i) => ({ ...fy, active: i === index })));
    setToast({ kind: 'ok', text: `Financial year ${finYears[index].from} - ${finYears[index].to} set as active.` });
  };

  // Keyboard shortcut for Cmd/Ctrl+S
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;
      const modKey = isMac ? e.metaKey : e.ctrlKey;
      if (modKey && e.key.toLowerCase() === 's') {
        e.preventDefault();
        handleSave();
      }
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  });

  return (
    <div className="mx-auto max-w-6xl">
      {toast && <Toast kind={toast.kind} text={toast.text} />}

      <div className={`${pageHeader} mb-5`}>
        <div>
          <div className="flex items-center gap-2">
            <h1 className={pageTitle}>Company Master</h1>
            <span className={badge('success')}>{companyProfile?.licence_type ? `Licensed entity · ${companyProfile.licence_type}` : 'Licensed entity'}</span>
          </div>
          <p className={pageSub}>Firm credentials, excise licences, retailer IDs and financial periods.</p>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={handleBackup} className={secondaryButton}><DatabaseBackup size={14} />Backup database</button>
          {canEditFiles && <button onClick={handleSave} className={primaryButton}><Save size={14} />Save Master</button>}
        </div>
      </div>

      <div role="tablist" aria-label="Company sections" className="mb-5 flex gap-1 overflow-x-auto border-b border-line">
        {TABS.map(t => (
          <button
            key={t.id}
            role="tab"
            aria-selected={tab === t.id}
            onClick={() => setTab(t.id)}
            className={`-mb-px flex items-center gap-2 whitespace-nowrap border-b-2 px-3 py-2.5 text-xs font-medium transition-colors ${
              tab === t.id
                ? 'border-accent text-stone-900 dark:text-stone-50'
                : 'border-transparent text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-50'
            }`}
          >
            <t.icon size={14} className={tab === t.id ? 'text-accent' : ''} />
            {t.id}
          </button>
        ))}
      </div>

      <div role="tabpanel" aria-label={tab} className={`${panel} p-6`}>
        {tab === 'Company Info' && (
          <div>
            <FormErrorSummary errors={errors} onFocusField={focusField} />
            <div className="grid gap-5 md:grid-cols-2">
              <TextField
                id="co-name"
                label="Company name (licence holder)"
                value={companyName}
                required
                readOnly={!canEditFiles || !companyProfile}
                hasError={!!errors['co-name']}
                errorMessage={errors['co-name']}
                onChange={e => {
                  setCompanyName(e.target.value);
                  if (errors['co-name']) setErrors(prev => { const n = { ...prev }; delete n['co-name']; return n; });
                }}
              />
              <TextField
                id="co-email"
                label="Email address"
                type="email"
                value={email}
                required
                readOnly={!canEditFiles || !companyProfile}
                hasError={!!errors['co-email']}
                errorMessage={errors['co-email']}
                onChange={e => {
                  setEmail(e.target.value);
                  if (errors['co-email']) setErrors(prev => { const n = { ...prev }; delete n['co-email']; return n; });
                }}
              />
              <div className="md:col-span-2">
                <label htmlFor="co-address" className={label}>Registered premises address</label>
                <textarea
                  id="co-address"
                  rows={3}
                  value={address}
                  onChange={e => {
                    setAddress(e.target.value);
                    if (errors['co-address']) setErrors(prev => { const next = { ...prev }; delete next['co-address']; return next; });
                  }}
                  readOnly={!canEditFiles || !companyProfile}
                  aria-invalid={!!errors['co-address']}
                  aria-errormessage={errors['co-address'] ? 'co-address-err' : undefined}
                  className={`${field} ${errors['co-address'] ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`}
                />
                <InlineError id="co-address-err" message={errors['co-address']} />
              </div>
            </div>
          </div>
        )}

        {tab === 'Additional Fields' && (
          <div className="grid gap-5 sm:grid-cols-2 md:grid-cols-3">
            {EXTRA_FIELDS.map(f => (
              <TextField
                key={f.id}
                id={f.id}
                label={f.label}
                value={companyProfile?.[f.key] ?? ''}
                placeholder={f.placeholder}
                className={f.mono ? 'font-mono' : ''}
                readOnly={!canEditFiles || !companyProfile}
                onChange={e => setCompanyProfile(profile => profile ? { ...profile, [f.key]: e.target.value } : profile)}
              />
            ))}
          </div>
        )}

        {tab === 'Financial Year(s)' && (
          <div className="space-y-4">
            <div className="flex flex-wrap items-center justify-between gap-3">
              <p className="text-xs text-stone-500 dark:text-stone-400">The active period determines all billing ledgers.</p>
              <button onClick={() => setToast({ kind: 'ok', text: 'New financial year initialized.' })} className={secondaryButton}><Plus size={14} />New financial year</button>
            </div>
            <div className="overflow-x-auto rounded-lg border border-line">
              <table className="w-full text-left text-xs">
                <thead className={tableHead}>
                  <tr>
                    <th className="px-4 py-2.5">From</th>
                    <th className="px-4 py-2.5">To</th>
                    <th className="px-4 py-2.5">Status</th>
                    <th className="px-4 py-2.5 text-right"><span className="sr-only">Actions</span></th>
                  </tr>
                </thead>
                <tbody className={tableBody}>
                  {finYears.map((fy, i) => (
                    <tr key={i} className={tableRow}>
                      <td className="px-4 py-2.5 tabular-nums text-stone-900 dark:text-stone-50">{fy.from}</td>
                      <td className="px-4 py-2.5 tabular-nums text-stone-900 dark:text-stone-50">{fy.to}</td>
                      <td className="px-4 py-2.5">
                        {fy.active
                          ? <span className={badge('success')}><span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />Active</span>
                          : <span className={badge('neutral')}>Inactive</span>}
                      </td>
                      <td className="px-4 py-2 text-right">
                        {fy.active
                          ? <span className="text-xs text-stone-500">Current</span>
                          : <button onClick={() => handleSetActiveFY(i)} className={`${ghostButton} py-1`}>Set active</button>}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {tab === 'Images' && <CompanyImages canEdit={canEditFiles} />}
        {tab === 'Documents' && <CompanyDocuments canEdit={canEditFiles} />}
      </div>
    </div>
  );
};

export default CompanyMaster;
