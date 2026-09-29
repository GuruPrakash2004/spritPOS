// Sales Counter Master: POS counters with their bill prefix and system user
// (/masters/sales-counters). The legacy app needs a counter before any Sales Invoice, so saving a
// counter opens Sales Invoice setup (onSave).

import { useCallback, useEffect, useRef, useState } from 'react';
import {
  AlertTriangle, ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight, Edit3, FilePlus,
  MoreHorizontal, Paperclip, Plus, Save, Search, Trash2, X,
} from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import DropdownMenu, { menuItem } from '../common/DropdownMenu';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import { badge, field, ghostButton, iconButton, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle } from '../../lib/styles';
import { api, errorText, type ApiCounter } from '../../lib/api';
import { DEFAULT_COUNTER, type SalesCounter } from '../../data/salesCounters';
import { downloadText, toCsv } from '../../lib/csv';
import PrintFormatSelectorModal from '../print/PrintFormatSelectorModal';
import { SAMPLE_INVOICE_DATA } from '../print/samplePrintData';

const fromApi = (c: ApiCounter): SalesCounter => ({ code: c.code, name: c.name, systemName: c.system_name, prefix: c.prefix, inactive: c.inactive });
const toApi = (c: SalesCounter): ApiCounter => ({ code: c.code, name: c.name, system_name: c.systemName, prefix: c.prefix, inactive: c.inactive });

const EMPTY_COUNTER: SalesCounter = { code: '', name: '', systemName: '', prefix: '', inactive: false };

type Mode = 'View' | 'New' | 'Edit';

const inputClass = `${field} font-mono read-only:bg-subtle read-only:text-stone-600 dark:read-only:text-stone-300`;

export default function SalesCounterMaster({ onSave }: { onSave: (counter: SalesCounter) => void }) {
  const [records, setRecords] = useState<SalesCounter[]>([DEFAULT_COUNTER]);
  const [index, setIndex] = useState(0);
  const [mode, setMode] = useState<Mode>('View');
  const [form, setForm] = useState<SalesCounter>(DEFAULT_COUNTER);
  const [files, setFiles] = useState<string[]>([]);
  const [search, setSearch] = useState('');
  const [error, setError] = useState('');
  const [formErrors, setFormErrors] = useState<Record<string, string>>({});
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [optionsOpen, setOptionsOpen] = useState(false);
  const optionsRef = useRef<HTMLButtonElement>(null);
  const closeOptions = useCallback(() => setOptionsOpen(false), []);
  const [showPrintModal, setShowPrintModal] = useState(false);
  const fileInput = useRef<HTMLInputElement>(null);
  const { confirm, dialog } = useConfirm();

  const handlePrintList = () => {
    setOptionsOpen(false);
    setShowPrintModal(true);
  };

  const handleExportExcel = async () => {
    setOptionsOpen(false);
    if (!records.length) return setToast({ kind: 'error', text: 'No counter records to export.' });
    if (!(await toastStore.defer('ok', `${records.length} counter records will be exported to CSV when this closes.`, 'Export cancelled.'))) return;
    const header = ['Counter Code', 'Counter Name', 'System Name', 'Bill Prefix', 'Status'];
    const rows = records.map(r => [r.code, r.name, r.systemName, r.prefix, r.inactive ? 'Inactive' : 'Active']);
    downloadText('Sales_Counters.csv', toCsv([header, ...rows]));
    setToast({ kind: 'ok', text: `${records.length} counter records exported to CSV.` });
  };

  // No change history is recorded yet, so report what is known rather than inventing an audit trail.
  const handleAuditTrail = () => {
    setOptionsOpen(false);
    const current = records[index] || form;
    setToast({
      kind: 'error',
      text: `Counter ${current.code} is ${current.inactive ? 'inactive' : 'active'}. Change history (audit trail) isn't recorded in the web app yet.`,
    });
  };

  useEffect(() => {
    api<ApiCounter[]>('/masters/sales-counters')
      .then(rows => { const list = rows.map(fromApi); setRecords(list); setIndex(0); if (list[0]) setForm(list[0]); })
      .catch(e => setError(errorText(e)));
  }, []);

  const readOnly = mode === 'View';
  const update = <K extends keyof SalesCounter>(key: K, value: SalesCounter[K]) => {
    setForm(prev => ({ ...prev, [key]: value }));
    if (formErrors[key as string]) {
      setFormErrors(prev => {
        const next = { ...prev };
        delete next[key as string];
        return next;
      });
    }
  };

  const focusField = (key: string) => {
    const id = `sc-${key}`;
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  const goTo = (next: number) => {
    if (!records.length) return;
    const clamped = Math.max(0, Math.min(records.length - 1, next));
    setIndex(clamped);
    setForm(records[clamped]);
    setMode('View');
    setError('');
  };

  const handleNew = () => { setForm(EMPTY_COUNTER); setFiles([]); setMode('New'); setError(''); };
  const handleEdit = () => { setMode('Edit'); setError(''); };
  const handleClose = () => goTo(index);

  // Remove the counter from the list only once the server has deleted it (it refuses counters with bills).
  const handleDelete = async () => {
    const code = form.code;
    if (!(await confirm(`Delete sales counter "${code}"?`))) return;
    if (!(await toastStore.defer('delete', `Sales Counter "${code}" will be deleted when this closes.`, 'Delete cancelled. Nothing was removed.'))) return;
    try {
      await api(`/masters/sales-counters/${encodeURIComponent(code)}`, { method: 'DELETE' });
    } catch (e) {
      setToast({ kind: 'error', text: errorText(e) });
      return;
    }
    const next = records.filter(r => r.code !== code);
    setRecords(next);
    const newIndex = Math.max(0, Math.min(index, next.length - 1));
    setIndex(newIndex);
    setForm(next[newIndex] ?? EMPTY_COUNTER);
    setMode('View');
    setToast({ kind: 'ok', text: `Sales Counter "${code}" deleted.` });
  };

  const handleSearch = () => {
    const term = search.trim().toLowerCase();
    const found = records.findIndex(r => r.code.toLowerCase().includes(term) || r.name.toLowerCase().includes(term));
    if (found === -1) setError(`No sales counter matches "${search}".`);
    else goTo(found);
  };

  const handleSave = async () => {
    const errs: Record<string, string> = {};
    if (!form.code.trim()) errs.code = 'Enter the SC code';
    if (!form.name.trim()) errs.name = 'Enter the SC name';
    if (!form.prefix.trim()) errs.prefix = 'Enter the bill prefix';

    if (Object.keys(errs).length > 0) {
      setFormErrors(errs);
      const first = Object.keys(errs)[0];
      focusField(first);
      return;
    }
    setFormErrors({});

    const saved = { ...form, code: form.code.trim(), name: form.name.trim(), prefix: form.prefix.trim() };
    if (!(await toastStore.defer('ok', `Sales Counter "${saved.code}" will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    const path = mode === 'New' ? '/masters/sales-counters' : `/masters/sales-counters/${encodeURIComponent(records[index].code)}`;
    api(path, { method: mode === 'New' ? 'POST' : 'PUT', body: toApi(saved) })
      .then(() => {
        commit(saved);
        setToast({ kind: 'ok', text: `Sales Counter "${saved.code}" saved.` });
      })
      .catch(e => {
        setToast({ kind: 'error', text: errorText(e) });
      });
  };

  const commit = (saved: SalesCounter) => {
    if (mode === 'New') {
      setRecords([...records, saved]);
      setIndex(records.length);
    } else {
      setRecords(records.map((r, i) => (i === index ? saved : r)));
    }
    setMode('View');
    setError('');
    onSave(saved);
  };

  const lbl = `${label} flex items-center gap-0.5`;

  return (
    <div className="mx-auto max-w-6xl">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      <div className={`${pageHeader} mb-5`}>
        <div>
          <div className="flex items-center gap-2">
            <h1 className={pageTitle}>Sales Counter Master</h1>
            <span className={badge(mode === 'View' ? 'neutral' : 'accent')}>{mode === 'View' ? 'View' : mode === 'New' ? 'New record' : 'Editing'}</span>
          </div>
          <p className={pageSub}>Saving opens the Sales Invoice setup for this counter.</p>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <button ref={optionsRef} onClick={() => setOptionsOpen(!optionsOpen)} aria-expanded={optionsOpen} aria-haspopup="menu" aria-label="More options" className={iconButton}><MoreHorizontal size={16} /></button>
          <DropdownMenu anchorRef={optionsRef} open={optionsOpen} onClose={closeOptions}>
            <button type="button" role="menuitem" onClick={handlePrintList} className={menuItem}>Print List</button>
            <button type="button" role="menuitem" onClick={handleExportExcel} className={menuItem}>Export to Excel</button>
            <button type="button" role="menuitem" onClick={handleAuditTrail} className={menuItem}>Audit Trail</button>
          </DropdownMenu>
          {readOnly ? (
            <>
              <button onClick={handleDelete} disabled={!records.length} className={`${ghostButton} hover:!text-red-600 dark:hover:!text-red-400`}><Trash2 size={14} />Delete</button>
              <button onClick={handleEdit} disabled={!records.length} className={secondaryButton}><Edit3 size={14} />Edit</button>
              <button onClick={handleNew} className={secondaryButton}><FilePlus size={14} />New</button>
            </>
          ) : (
            <button onClick={handleClose} className={secondaryButton}><X size={14} />Cancel</button>
          )}
          <button onClick={handleSave} className={primaryButton}><Save size={14} />Save Master</button>
        </div>
      </div>

      {/* Record navigator + search */}
      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div className="flex items-center gap-1" role="group" aria-label="Record navigation">
          <button onClick={() => goTo(0)} disabled={!readOnly} className={iconButton} aria-label="First record"><ChevronsLeft size={16} /></button>
          <button onClick={() => goTo(index - 1)} disabled={!readOnly} className={iconButton} aria-label="Previous record"><ChevronLeft size={16} /></button>
          <span className="px-2 text-xs tabular-nums text-stone-600 dark:text-stone-300">Record <span className="font-semibold text-stone-900 dark:text-stone-50">{records.length ? index + 1 : 0}</span> of {records.length}</span>
          <button onClick={() => goTo(index + 1)} disabled={!readOnly} className={iconButton} aria-label="Next record"><ChevronRight size={16} /></button>
          <button onClick={() => goTo(records.length - 1)} disabled={!readOnly} className={iconButton} aria-label="Last record"><ChevronsRight size={16} /></button>
        </div>
        <div className="flex w-full gap-2 sm:w-72">
          <input id="sc-search" value={search} onChange={e => setSearch(e.target.value)} onKeyDown={e => e.key === 'Enter' && handleSearch()} placeholder="Find counter by code or name" aria-label="Search counters" className={field} />
          <button onClick={handleSearch} className={secondaryButton} aria-label="Search counters"><Search size={14} /></button>
        </div>
      </div>

      {error && (
        <div role="alert" className="mb-4 flex items-center gap-2 rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-2.5 text-xs font-medium text-red-700 dark:text-red-300">
          <AlertTriangle size={14} className="shrink-0" /> {error}
        </div>
      )}

      <div className={`${panel} divide-y divide-line p-6`}>
        {!readOnly && <FormErrorSummary errors={formErrors} onFocusField={focusField} />}
        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Counter</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The code can't be changed after the counter is created.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="sc-code" className={lbl}>SC code<span className="text-red-600" aria-hidden="true">*</span></label>
              <input id="sc-code" value={form.code} readOnly={readOnly || mode === 'Edit'} onChange={e => update('code', e.target.value)} aria-invalid={!!formErrors.code} className={`${inputClass} ${formErrors.code ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
              <InlineError message={formErrors.code} />
            </div>
            <div>
              <label htmlFor="sc-name" className={lbl}>SC name<span className="text-red-600" aria-hidden="true">*</span></label>
              <input id="sc-name" value={form.name} readOnly={readOnly} onChange={e => update('name', e.target.value)} aria-invalid={!!formErrors.name} className={`${inputClass} ${formErrors.name ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
              <InlineError message={formErrors.name} />
            </div>
            <label className="flex items-center gap-2 text-xs text-stone-700 sm:col-span-2 dark:text-stone-300">
              <input type="checkbox" checked={form.inactive} disabled={readOnly} onChange={e => update('inactive', e.target.checked)} className="h-4 w-4 accent-accent" />
              Counter is inactive
            </label>
          </div>
        </section>

        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Billing</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">The machine that runs this counter and its bill number prefix.</p>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <div>
              <label htmlFor="sc-system" className={label}>System name</label>
              <input id="sc-system" value={form.systemName} readOnly={readOnly} onChange={e => update('systemName', e.target.value.toUpperCase())} className={inputClass} />
            </div>
            <div>
              <label htmlFor="sc-prefix" className={lbl}>Bill prefix<span className="text-red-600" aria-hidden="true">*</span></label>
              <input id="sc-prefix" value={form.prefix} readOnly={readOnly} onChange={e => update('prefix', e.target.value)} aria-invalid={!!formErrors.prefix} className={`${inputClass} ${formErrors.prefix ? '!border-red-500 !ring-2 !ring-red-500/20 animate-shake' : ''}`} />
              <InlineError message={formErrors.prefix} />
            </div>
            <p className="text-xs text-stone-500 sm:col-span-2 dark:text-stone-400">
              Bills on this counter are numbered like <span className="rounded bg-subtle px-1.5 py-0.5 font-mono text-stone-800 dark:text-stone-200">26/{form.prefix || '<prefix>'}-0083718</span>
            </p>
          </div>
        </section>

        <section className="grid gap-6 p-6 md:grid-cols-[200px_minmax(0,1fr)]">
          <div>
            <h2 className={sectionTitle}>Attachments</h2>
            <p className="mt-1 text-xs text-stone-500 dark:text-stone-400">{files.length ? `${files.length} file${files.length > 1 ? 's' : ''}` : 'Optional supporting files.'}</p>
          </div>
          <div>
            <ul className="divide-y divide-line-soft rounded-lg border border-line text-xs">
              {files.length === 0 && <li className="px-3 py-5 text-center text-stone-500">No files attached</li>}
              {files.map((name, i) => (
                <li key={`${name}-${i}`} className="flex items-center justify-between gap-2 px-3 py-2">
                  <span className="flex min-w-0 items-center gap-2 text-stone-700 dark:text-stone-300"><Paperclip size={14} className="shrink-0 text-stone-400" /><span className="truncate">{name}</span></span>
                  <button onClick={() => setFiles(files.filter((_, j) => j !== i))} className={`${iconButton} h-7 w-7 hover:!text-red-600`} aria-label={`Remove ${name}`}><X size={14} /></button>
                </li>
              ))}
            </ul>
            <button onClick={() => fileInput.current?.click()} className={`${ghostButton} mt-2`}><Plus size={14} />Attach file</button>
            <input ref={fileInput} type="file" multiple hidden onChange={e => setFiles([...files, ...Array.from(e.target.files ?? []).map(f => f.name)])} />
          </div>
        </section>
      </div>

      {showPrintModal && (
        <PrintFormatSelectorModal
          category="SALES_INVOICE"
          data={{
            ...SAMPLE_INVOICE_DATA,
            counter: (records[index] || form).name || (records[index] || form).code || 'Counter 1',
            doc_no: `26/${(records[index] || form).prefix || 'c1'}-0083718`,
          }}
          title={`Print Bill Format (${(records[index] || form).name || (records[index] || form).code || 'Counter'})`}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
}
