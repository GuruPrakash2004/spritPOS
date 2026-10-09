// Company Master: the store's profile (name, address, GSTIN, licence, FY) plus Images and Documents tabs.
// Loads GET /masters/company and saves with PUT (Administrator only). After a save it calls
// notifyCompanyProfileUpdated() so the header, login page and prints refresh.
// All data is in the shop's SQL Server (Web_ tables), which is backed up by SQL Server itself.

import { useMemo, useState, useEffect } from 'react';
import { FIELD_RULES, cleanInput, fieldProblem } from '../../lib/validation';
import { 
  Building2, ShieldCheck, Calendar, Image as ImageIcon,
  FolderArchive, Save, Plus,
} from 'lucide-react';
import { CompanyDocuments, CompanyImages } from './CompanyFiles';
import { TextField } from '../common/FormField';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import { createFinYear, errorText, getBackupStatus, getCompany, getNextFinYear, removeFinYear, saveCompany, startBackup, updateDashboardLink, } from '../../lib/api';
import Modal from '../common/Modal';
import DatabaseSetup from '../auth/DatabaseSetup';
import { notifyCompanyProfileUpdated } from '../../hooks/useCompanyProfile';
import { useScreenFields } from '../../hooks/useScreenFields';
import { applyDefinition, extraProblem, valueOf } from '../../lib/designerFields';
import DefinedExtras from './DefinedExtras';

import { cn } from '../../lib/utils';
import { badge, field, fieldTone, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, tableRow } from '../../lib/styles';

// The shop's map is a plain Google Maps frame searching the company name and registered premises address (no API, no key).
const mapSrc = (name, address) => (address.trim() ? `https://www.google.com/maps?q=${encodeURIComponent(`${name} ${address}`.trim())}&output=embed` : '');

const TABS = [
  { id: 'Company Info', icon: Building2 },
  { id: 'Additional Fields', icon: ShieldCheck },
  { id: 'Financial Year(s)', icon: Calendar },
  { id: 'Images', icon: ImageIcon },
  { id: 'Documents', icon: FolderArchive },
];

const EXTRA_FIELDS = [
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

// "2026-04-01" → "01/04/2026" for the financial-year table.
const dmy = (iso) => (iso ? iso.split('-').reverse().join('/') : '—');

// The last profile loaded, so the screen opens filled in at once and is refreshed from the server behind it.
let lastProfile = null;

const CompanyMaster = ({ canEditFiles }) => {
  const [tab, setTab] = useState('Company Info');
  const [companyProfile, setCompanyProfile] = useState(lastProfile);
  const [companyName, setCompanyName] = useState(lastProfile?.name ?? '');
  const [email, setEmail] = useState(lastProfile?.email ?? '');
  // The desktop's Company Info grid: Company Name, Address, ".", "..", Email ID = CompName, Address1, Address2, Address3, EMAIL.
  const lines = (p) => [p?.address1 || p?.address || '', p?.address2 ?? '', p?.address3 ?? ''];
  const [addr, setAddr] = useState(lines(lastProfile));
  const address = addr.filter(x => x.trim()).join(', ');
  // The shop's Google Maps frame: what the owner pastes (an <iframe> or its link); the server keeps only the link.
  const [mapEmbed, setMapEmbed] = useState(lastProfile?.map_embed ?? '');
  const [errors, setErrors] = useState({});
  const [toast, setToast] = useState(null);
  // The Additional Fields tab follows the Doc Designer's MstComp definition (caption, order, mandatory, read-only, hidden)
  // and shows every other MstComp column the database has (`extras`), saved back to MstComp.
  const live = useScreenFields('company');
  const extraFieldList = useMemo(() => applyDefinition(EXTRA_FIELDS.map(f => ({ ...f, required: false })), live.screen), [live.screen]);

  // The desktop's buttons under the grid: Backup (BACKUP DATABASE in the background), Companies... (switch database), Update Dashboard Link.
  const [backup, setBackup] = useState(null);
  const [companiesOpen, setCompaniesOpen] = useState(false);
  const [linkForm, setLinkForm] = useState(null);
  const runBackup = async () => {
    if (!(await toastStore.defer('backup', 'The database will be backed up when this closes.', 'Backup cancelled.'))) return;
    try {
      setBackup(await startBackup());
      const timer = setInterval(async () => {
        try {
          const state = await getBackupStatus();
          setBackup(state);
          if (state.state !== 'running') {
            clearInterval(timer);
            setToast({ kind: state.state === 'ok' ? 'ok' : 'error', text: state.message });
          }
        } catch { clearInterval(timer); }
      }, 3000);
    } catch (e) { setToast({ kind: 'error', text: errorText(e) }); }
  };
  const saveLink = async () => {
    if (!(await toastStore.defer('ok', 'The dashboard link will be saved when this closes.', 'Cancelled. The link was not changed.'))) return;
    try {
      setCompanyProfile(await updateDashboardLink(linkForm));
      setLinkForm(null);
      setToast({ kind: 'ok', text: 'Dashboard link updated.' });
    } catch (e) { setToast({ kind: 'error', text: errorText(e) }); }
  };
  const [fyForm, setFyForm] = useState(null);   // the "new financial year" form: {start, end} or null
  const startFinYear = () => getNextFinYear().then(r => setFyForm(r)).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  const saveFinYear = async () => {
    if (!fyForm?.start || !fyForm?.end) { setToast({ kind: 'error', text: 'Enter both dates.' }); return; }
    if (!(await toastStore.defer('ok', 'The financial year will be created when this closes.', 'Cancelled. No year was created.'))) return;
    try {
      const profile = await createFinYear(fyForm);
      setCompanyProfile(profile); setFyForm(null);
      setToast({ kind: 'ok', text: 'Financial year created.' });
    } catch (e) { setToast({ kind: 'error', text: errorText(e) }); }
  };
  const deleteFinYear = async (y) => {
    if (!(await toastStore.defer('delete', `The financial year ${dmy(y.start)} to ${dmy(y.end)} will be deleted when this closes.`, 'Delete cancelled. Nothing was removed.'))) return;
    try {
      const profile = await removeFinYear(y.start);
      setCompanyProfile(profile);
      setToast({ kind: 'ok', text: 'Financial year deleted.' });
    } catch (e) { setToast({ kind: 'error', text: errorText(e) }); }
  };

  const focusField = (key) => {
    const el = document.getElementById(key);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  useEffect(() => {
    getCompany()
      .then(profile => {
        lastProfile = profile;
        setCompanyProfile(profile);
        setCompanyName(profile.name);
        setEmail(profile.email);
        setAddr(lines(profile));
        setMapEmbed(profile.map_embed ?? '');
      })
      .catch(error => setToast({ kind: 'error', text: errorText(error) }));
  }, []);

  const handleSave = async () => {
    if (!canEditFiles || !companyProfile) return;
    const errs = {};
    if (!companyName.trim()) errs['co-name'] = 'Enter the company name';
    if (!addr[0].trim()) errs['co-address'] = 'Enter the address';
    if (!email.trim()) errs['co-email'] = 'Enter the email address';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) errs['co-email'] = 'Enter a valid email address';
    // Tel / mobile / excise phone: exactly 10 digits; GSTIN and PAN in their patterns (lib/validation.js, same as the server).
    for (const f of extraFieldList) {
      if (f.designerHidden) continue;
      const text = String(companyProfile[f.key] ?? '').trim();
      const wrong = f.required && !text ? `Enter the ${f.label}` : fieldProblem(f.key, companyProfile[f.key]);
      if (wrong) errs[f.id] = wrong;
    }
    for (const f of live.fields) {
      const wrong = extraProblem(f, companyProfile);
      if (wrong) errs[`co-x-${f.key}`] = wrong;
    }

    if (Object.keys(errs).length > 0) {
      setErrors(errs);
      const first = Object.keys(errs)[0];
      if (extraFieldList.some(f => f.id === first) || first.startsWith('co-x-')) { setTab('Additional Fields'); setTimeout(() => focusField(first), 60); } else focusField(first);
      return;
    }
    setErrors({});
    if (!(await toastStore.defer('ok', 'Company profile will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    try {
      const profile = await saveCompany({ ...companyProfile, extras: Object.fromEntries(live.fields.filter(f => !f.lockedAlways).map(f => [f.key, valueOf(companyProfile, f) ?? ''])), name: companyName.trim(), email: email.trim(), address: address.trim(), address1: addr[0].trim(), address2: addr[1].trim(), address3: addr[2].trim(), map_embed: mapEmbed.trim() });
      lastProfile = profile;
      setCompanyProfile(profile);
      setCompanyName(profile.name);
      setEmail(profile.email);
      setAddr(lines(profile));
      setMapEmbed(profile.map_embed ?? '');
      notifyCompanyProfileUpdated();
      setToast({ kind: 'ok', text: 'Company profile updated successfully.' });
    } catch (error) {
      setToast({ kind: 'error', text: errorText(error) });
    }
  };

  // Keyboard shortcut for Cmd/Ctrl+S
  useEffect(() => {
    const onKey = (e) => {
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
              <div className="md:col-span-2 grid gap-3">
                {['Address', 'Address line 2', 'Address line 3'].map((caption, i) => (
                  <div key={caption}>
                    <label htmlFor={i === 0 ? 'co-address' : `co-address${i + 1}`} className={cn(label, fieldTone(i === 0, false))}>{caption}{i === 0 && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}</label>
                    <input
                      id={i === 0 ? 'co-address' : `co-address${i + 1}`}
                      value={addr[i]}
                      readOnly={!canEditFiles || !companyProfile}
                      onChange={e => {
                        setAddr(prev => prev.map((x, j) => (j === i ? e.target.value : x)));
                        if (i === 0 && errors['co-address']) setErrors(prev => { const next = { ...prev }; delete next['co-address']; return next; });
                      }}
                      aria-invalid={i === 0 && !!errors['co-address']}
                      className={`${field} ${i === 0 && errors['co-address'] ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`}
                    />
                    {i === 0 && <InlineError id="co-address-err" message={errors['co-address']} />}
                  </div>
                ))}
              </div>
              <div className="md:col-span-2">
                <label className={label}>Shop location</label>
                {mapSrc(companyName, address) ? (
                  <iframe title="Shop location" src={mapSrc(companyName, address)} loading="lazy"
                    referrerPolicy="no-referrer-when-downgrade" className="mt-2 h-72 w-full rounded-xl border border-line" />
                ) : <p className="mt-2 text-[11px] text-stone-500">Fill in the address to see the shop on the map.</p>}
              </div>
            </div>
          </div>
        )}

        {tab === 'Additional Fields' && (
          <div className="space-y-6">
          <div className="grid gap-5 sm:grid-cols-2 md:grid-cols-3">
            {extraFieldList.filter(f => !f.designerHidden).map(f => (
              <TextField
                key={f.id}
                id={f.id}
                label={f.label}
                required={f.required}
                designerReadOnly={f.lockedAlways}
                value={companyProfile?.[f.key] ?? ''}
                placeholder={f.placeholder}
                className={f.mono ? 'font-mono' : ''}
                readOnly={!canEditFiles || !companyProfile || f.lockedAlways}
                hasError={!!errors[f.id]}
                errorMessage={errors[f.id]}
                inputMode={FIELD_RULES[f.key]?.digits ? 'numeric' : undefined}
                maxLength={FIELD_RULES[f.key]?.max}
                onChange={e => {
                  const value = cleanInput(f.key, e.target.value);   // digits only for phone numbers, capitals for GSTIN / PAN
                  setCompanyProfile(profile => profile ? { ...profile, [f.key]: value } : profile);
                  if (errors[f.id]) setErrors(prev => { const n = { ...prev }; delete n[f.id]; return n; });
                }}
              />
            ))}
          </div>
          {live.fields.length > 0 && companyProfile && (
            <DefinedExtras fields={live.fields} form={companyProfile} onChange={setCompanyProfile} readOnly={!canEditFiles} errors={Object.fromEntries(Object.entries(errors).filter(([k]) => k.startsWith('co-x-')).map(([k, v]) => [k.slice(5), v]))} idPrefix="co" />
          )}
          </div>
        )}

        {tab === 'Financial Year(s)' && (
          // As vLQMS.exe: years in date order; a new year starts a year after the last (it cannot start inside an existing one); only the
          // first or last year can be removed, never the active one, and one year always stays.
          <div className="space-y-4">
            <div className="flex flex-wrap items-center justify-between gap-3">
              <p className="text-xs text-stone-500 dark:text-stone-400">The active period determines all billing ledgers.</p>
              {canEditFiles && <button onClick={startFinYear} className={secondaryButton}><Plus size={14} />New financial year</button>}
            </div>
            {fyForm && (
              <div className="flex flex-wrap items-end gap-3 rounded-lg border border-line p-3">
                <div><label htmlFor="fy-from" className={label}>From date</label><input id="fy-from" type="date" value={fyForm.start} onChange={e => setFyForm(f => ({ ...f, start: e.target.value }))} className={`${field} font-mono`} /></div>
                <div><label htmlFor="fy-to" className={label}>To date</label><input id="fy-to" type="date" value={fyForm.end} onChange={e => setFyForm(f => ({ ...f, end: e.target.value }))} className={`${field} font-mono`} /></div>
                <button onClick={saveFinYear} className={primaryButton}>Create</button>
                <button onClick={() => setFyForm(null)} className={secondaryButton}>Cancel</button>
              </div>
            )}
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
                  {[...(companyProfile?.fin_years ?? [])].sort((x, y) => x.start.localeCompare(y.start)).map(y => (
                    <tr key={y.start} className={tableRow}>
                      <td className="px-4 py-2.5 tabular-nums text-stone-900 dark:text-stone-50">{dmy(y.start)}</td>
                      <td className="px-4 py-2.5 tabular-nums text-stone-900 dark:text-stone-50">{dmy(y.end)}</td>
                      <td className="px-4 py-2.5">
                        {y.active ? <span className={badge('success')}><span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />Active</span> : <span className="text-stone-500">—</span>}
                      </td>
                      <td className="px-4 py-2 text-right">
                        {canEditFiles && !y.active && <button onClick={() => deleteFinYear(y)} className="text-xs text-[#9A4B45] hover:underline dark:text-red-400">Remove</button>}
                      </td>
                    </tr>
                  ))}
                  {!(companyProfile?.fin_years ?? []).length && (
                    <tr className={tableRow}>
                      <td className="px-4 py-2.5 tabular-nums">{dmy(companyProfile?.fy_start)}</td>
                      <td className="px-4 py-2.5 tabular-nums">{dmy(companyProfile?.fy_end)}</td>
                      <td className="px-4 py-2.5"><span className={badge('success')}>Active</span></td>
                      <td />
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {tab === 'Images' && <CompanyImages canEdit={canEditFiles} />}
        {tab === 'Documents' && <CompanyDocuments canEdit={canEditFiles} />}
      </div>

      {canEditFiles && (
        <div className="mt-4 flex flex-wrap items-center gap-3">
          <button onClick={runBackup} disabled={backup?.state === 'running'} className={secondaryButton} title="Backup of the current database">
            {backup?.state === 'running' ? 'Backing up…' : 'Backup'}
          </button>
          <button onClick={() => setCompaniesOpen(true)} className={secondaryButton}>Companies…</button>
          <button onClick={() => setLinkForm(companyProfile?.extras?.DashBoardLink ?? '')} className="text-xs font-medium text-accent-ink underline">Update Dashboard Link</button>
          {backup?.state === 'running' && <span role="status" className="text-xs text-stone-500">Backing up {backup.file}…</span>}
        </div>
      )}
      {linkForm !== null && (
        <div className={`${panel} mt-3 flex flex-wrap items-end gap-3 p-4`}>
          <div className="min-w-0 flex-1"><label htmlFor="co-dash" className={label}>Dashboard link</label><input id="co-dash" value={linkForm} onChange={e => setLinkForm(e.target.value)} placeholder="https://" className={field} /></div>
          <button onClick={saveLink} className={primaryButton}>Save link</button>
          <button onClick={() => setLinkForm(null)} className={secondaryButton}>Cancel</button>
        </div>
      )}
      {companiesOpen && (
        <Modal title="Companies" onClose={() => setCompaniesOpen(false)} wide>
          <DatabaseSetup inputClass={field} labelClass={label} change plain onCancel={() => setCompaniesOpen(false)} onDone={() => window.location.reload()} />
        </Modal>
      )}
    </div>
  );
};

export default CompanyMaster;
