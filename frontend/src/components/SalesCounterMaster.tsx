import { useEffect, useRef, useState } from 'react';
import {
  AlertTriangle, ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight, Edit3, FilePlus,
  Monitor, Paperclip, Plus, Save, Search, Settings2, Trash2, X,
} from 'lucide-react';
import { api, errorText, type ApiCounter } from './api';
import { DEFAULT_COUNTER, type SalesCounter } from './salesCounters';

const fromApi = (c: ApiCounter): SalesCounter => ({ code: c.code, name: c.name, systemName: c.system_name, prefix: c.prefix, inactive: c.inactive });
const toApi = (c: SalesCounter): ApiCounter => ({ code: c.code, name: c.name, system_name: c.systemName, prefix: c.prefix, inactive: c.inactive });

const EMPTY_COUNTER: SalesCounter = { code: '', name: '', systemName: '', prefix: '', inactive: false };

type Mode = 'View' | 'New' | 'Edit';

const inputClass = 'w-full rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-3 py-1.5 font-mono text-xs text-stone-800 outline-none focus:border-[#e8702a] read-only:bg-stone-100 read-only:text-stone-500 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-100 dark:read-only:bg-[#1b2033] dark:read-only:text-stone-400';
const toolButton = 'flex items-center gap-1.5 rounded-lg px-3 py-2 text-xs font-semibold text-white shadow-sm disabled:cursor-not-allowed disabled:opacity-40';
const navButton = 'rounded-lg border border-[#e1c9b6] bg-[#fffaf5] p-2 text-stone-600 hover:border-[#e8702a] hover:text-[#b84716] dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-300';

export default function SalesCounterMaster({ onSave }: { onSave: (counter: SalesCounter) => void }) {
  const [records, setRecords] = useState<SalesCounter[]>([DEFAULT_COUNTER]);
  const [index, setIndex] = useState(0);
  const [mode, setMode] = useState<Mode>('View');
  const [form, setForm] = useState<SalesCounter>(DEFAULT_COUNTER);
  const [files, setFiles] = useState<string[]>([]);
  const [search, setSearch] = useState('');
  const [error, setError] = useState('');
  const [optionsOpen, setOptionsOpen] = useState(false);
  const fileInput = useRef<HTMLInputElement>(null);

  useEffect(() => {
    api<ApiCounter[]>('/masters/sales-counters')
      .then(rows => { const list = rows.map(fromApi); setRecords(list); setIndex(0); if (list[0]) setForm(list[0]); })
      .catch(e => setError(errorText(e)));
  }, []);

  const readOnly = mode === 'View';
  const update = <K extends keyof SalesCounter>(key: K, value: SalesCounter[K]) => setForm(prev => ({ ...prev, [key]: value }));

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

  const handleDelete = () => {
    if (!window.confirm(`Delete sales counter "${form.code}"?`)) return;
    api(`/masters/sales-counters/${encodeURIComponent(form.code)}`, { method: 'DELETE' }).catch(e => setError(errorText(e)));
    const next = records.filter((_, i) => i !== index);
    setRecords(next);
    const newIndex = Math.min(index, next.length - 1);
    setIndex(Math.max(newIndex, 0));
    setForm(next[newIndex] ?? EMPTY_COUNTER);
    setMode('View');
  };

  const handleSearch = () => {
    const term = search.trim().toLowerCase();
    const found = records.findIndex(r => r.code.toLowerCase().includes(term) || r.name.toLowerCase().includes(term));
    if (found === -1) setError(`No sales counter matches "${search}".`);
    else goTo(found);
  };

  const handleSave = () => {
    if (!form.code.trim() || !form.name.trim()) return setError('SC Code and SC Name are required.');
    if (!form.prefix.trim()) return setError('Prefix is required — it is used in every bill number for this counter.');
    if (form.inactive) return setError('This counter is marked Inactive. Untick Inactive to open a sales bill on it.');
    const duplicate = records.findIndex(r => r.code.toLowerCase() === form.code.trim().toLowerCase());
    if (mode === 'New' && duplicate !== -1) return setError(`SC Code "${form.code}" already exists.`);

    const saved = { ...form, code: form.code.trim(), name: form.name.trim(), prefix: form.prefix.trim() };
    const path = mode === 'New' ? '/masters/sales-counters' : `/masters/sales-counters/${encodeURIComponent(records[index].code)}`;
    api(path, { method: mode === 'New' ? 'POST' : 'PUT', body: toApi(saved) })
      .then(() => commit(saved))
      .catch(e => setError(errorText(e)));
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

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className="flex flex-wrap items-center justify-between gap-3 rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] px-5 py-3 shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]">
        <div className="flex items-center gap-3">
          <div className="rounded-xl bg-[#e8702a]/10 p-2.5 text-[#b84716] dark:text-[#ffb07e]"><Monitor size={20} /></div>
          <div>
            <h2 className="text-lg font-bold text-stone-900 dark:text-stone-100">{mode}: Sales Counter Master</h2>
            <p className="text-xs text-stone-500">Record {records.length ? index + 1 : 0} of {records.length} • Saving opens the Sales Invoice setup for this counter.</p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          <label htmlFor="sc-search" className="text-xs font-bold text-stone-600 dark:text-stone-300">Search</label>
          <input id="sc-search" value={search} onChange={e => setSearch(e.target.value)} onKeyDown={e => e.key === 'Enter' && handleSearch()} placeholder="Code / name" className={`${inputClass} w-44`} />
          <button onClick={handleSearch} className={navButton} aria-label="Search counters"><Search size={14} /></button>
        </div>
      </div>

      {error && (
        <div className="flex items-center gap-2 rounded-xl border border-red-200 bg-red-50 px-4 py-2 text-xs font-medium text-red-700 dark:border-red-900 dark:bg-red-950/40 dark:text-red-300">
          <AlertTriangle size={14} /> {error}
        </div>
      )}

      <div className="grid grid-cols-1 gap-4 lg:grid-cols-2">
        <div className="flex flex-col gap-4 rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] p-5 shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]">
          <div className="grid grid-cols-[90px_1fr] items-center gap-x-4 gap-y-3 text-xs">
            <label htmlFor="sc-code" className="font-semibold text-[#b84716] dark:text-[#ffb07e]">SC Code *</label>
            <input id="sc-code" value={form.code} readOnly={readOnly || mode === 'Edit'} onChange={e => update('code', e.target.value)} className={inputClass} />
            <label htmlFor="sc-name" className="font-semibold text-[#b84716] dark:text-[#ffb07e]">SC Name *</label>
            <input id="sc-name" value={form.name} readOnly={readOnly} onChange={e => update('name', e.target.value)} className={inputClass} />
            <span />
            <label className="flex items-center gap-2 text-stone-600 dark:text-stone-300">
              <input type="checkbox" checked={form.inactive} disabled={readOnly} onChange={e => update('inactive', e.target.checked)} className="accent-[#e8702a]" />
              Inactive
            </label>
          </div>

          <div className="mt-auto overflow-hidden rounded-xl border border-[#e1c9b6] dark:border-[#414a6e]">
            <div className="flex items-center justify-between bg-[#e8702a] px-3 py-2 text-xs font-bold text-white">
              <span>File Attachment ({files.length})</span>
              <button onClick={() => fileInput.current?.click()} aria-label="Attach file" className="rounded p-0.5 hover:bg-white/20"><Plus size={14} /></button>
              <input ref={fileInput} type="file" multiple hidden onChange={e => setFiles([...files, ...Array.from(e.target.files ?? []).map(f => f.name)])} />
            </div>
            <ul className="min-h-20 divide-y divide-[#f1e3d8] bg-[#fffaf5] text-xs dark:divide-[#343b59] dark:bg-[#1b2033]">
              {files.length === 0 && <li className="px-3 py-6 text-center text-stone-400">No files attached</li>}
              {files.map((name, i) => (
                <li key={`${name}-${i}`} className="flex items-center justify-between px-3 py-1.5">
                  <span className="flex items-center gap-1.5 text-stone-700 dark:text-stone-300"><Paperclip size={12} />{name}</span>
                  <button onClick={() => setFiles(files.filter((_, j) => j !== i))} className="text-stone-400 hover:text-red-500" aria-label={`Remove ${name}`}><X size={12} /></button>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] p-5 shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]">
          <span className="inline-block rounded-t-lg border border-b-0 border-[#e1c9b6] bg-[#fffaf5] px-3 py-1 text-xs font-bold text-stone-700 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-200">Header Info</span>
          <table className="w-full border border-[#e1c9b6] text-xs dark:border-[#414a6e]">
            <tbody>
              <tr className="border-b border-[#e1c9b6] dark:border-[#414a6e]">
                <th className="w-32 bg-[#f1e3d8] px-3 py-2 text-left font-semibold text-stone-600 dark:bg-[#20253a] dark:text-stone-300">System Name</th>
                <td className="p-1"><input value={form.systemName} readOnly={readOnly} onChange={e => update('systemName', e.target.value.toUpperCase())} className={inputClass} aria-label="System Name" /></td>
              </tr>
              <tr>
                <th className="bg-[#f1e3d8] px-3 py-2 text-left font-semibold text-stone-600 dark:bg-[#20253a] dark:text-stone-300">Prefix *</th>
                <td className="bg-[#e8702a]/15 p-1"><input value={form.prefix} readOnly={readOnly} onChange={e => update('prefix', e.target.value)} className={`${inputClass} font-bold text-[#b84716]`} aria-label="Prefix" /></td>
              </tr>
            </tbody>
          </table>
          <p className="mt-3 text-xs text-stone-500">
            Bill numbers on this counter look like <span className="font-mono font-bold text-[#b84716] dark:text-[#ffb07e]">26/{form.prefix || '<prefix>'}-0083718</span>.
          </p>
        </div>
      </div>

      <div className="flex flex-wrap items-center justify-center gap-2 rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] px-4 py-3 shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]">
        <button onClick={handleNew} disabled={!readOnly} className={`${toolButton} bg-sky-600 hover:bg-sky-700`}><FilePlus size={14} />New</button>
        <button onClick={handleEdit} disabled={!readOnly || !records.length} className={`${toolButton} bg-stone-600 hover:bg-stone-700`}><Edit3 size={14} />Edit</button>
        <button onClick={handleDelete} disabled={!readOnly || !records.length} className={`${toolButton} bg-red-600 hover:bg-red-700`}><Trash2 size={14} />Delete</button>
        <button onClick={handleClose} disabled={readOnly} className={`${toolButton} bg-stone-400 hover:bg-stone-500`}><X size={14} />Close</button>
        <button onClick={handleSave} className={`${toolButton} bg-[#e8702a] hover:bg-[#b84716]`}><Save size={14} />Save Master</button>
        <div className="relative">
          <button onClick={() => setOptionsOpen(!optionsOpen)} className={`${toolButton} bg-[#e8702a] hover:bg-[#b84716]`}><Settings2 size={14} />Options</button>
          {optionsOpen && (
            <div className="absolute bottom-full left-0 z-10 mb-2 w-40 overflow-hidden rounded-xl border border-[#e1c9b6] bg-[#fffaf5] text-xs shadow-lg dark:border-[#414a6e] dark:bg-[#20253a]">
              {['Print List', 'Export to Excel', 'Audit Trail'].map(option => (
                <button key={option} onClick={() => { setOptionsOpen(false); alert(`${option} — not available in the prototype.`); }} className="block w-full px-3 py-2 text-left text-stone-700 hover:bg-[#f6efe8] dark:text-stone-200 dark:hover:bg-[#343b59]">{option}</button>
              ))}
            </div>
          )}
        </div>
        <span className="mx-1 h-6 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={() => goTo(0)} disabled={!readOnly} className={navButton} aria-label="First record"><ChevronsLeft size={14} /></button>
        <button onClick={() => goTo(index - 1)} disabled={!readOnly} className={navButton} aria-label="Previous record"><ChevronLeft size={14} /></button>
        <button onClick={() => goTo(index + 1)} disabled={!readOnly} className={navButton} aria-label="Next record"><ChevronRight size={14} /></button>
        <button onClick={() => goTo(records.length - 1)} disabled={!readOnly} className={navButton} aria-label="Last record"><ChevronsRight size={14} /></button>
      </div>
    </div>
  );
}
