import { useEffect, useState, type ReactNode } from 'react';
import { Edit3, FilePlus, Save, Search, Trash2, X } from 'lucide-react';
import Toast from './Toast';
import { api, errorText } from './api';
import { field, label as labelClass, pageSub, pageTitle, panel, primaryButton, secondaryButton, dangerButton, tableBody, tableHead } from './ui';

// One screen for every simple master, the web twin of the legacy frmMstGeneral engine
// and of the backend's crud() factory (/masters/<path>).
export type MasterField = {
  key: string;
  label: string;
  type?: 'text' | 'number' | 'bool' | 'select';
  options?: string[];
  optionsFrom?: string; // another master's path, whose codes become the options
  required?: boolean;
  showInList?: boolean;
  default?: string | number | boolean; // value for a New record
};

type Row = Record<string, string | number | boolean>;
type Mode = 'View' | 'New' | 'Edit';

const blank = (fields: MasterField[]): Row =>
  Object.fromEntries(fields.map(f => [f.key, f.default ?? (f.type === 'bool' ? false : f.type === 'number' ? 0 : '')]));

export default function MasterCrud({ title, subtitle, path, fields, toolbar, reloadKey = 0 }: {
  title: string; subtitle: string; path: string; fields: MasterField[];
  toolbar?: ReactNode; // extra buttons, e.g. Item Master's import
  reloadKey?: number; // bump to re-fetch the list after an outside change
}) {
  const [rows, setRows] = useState<Row[]>([]);
  const [selected, setSelected] = useState<string | null>(null);
  const [form, setForm] = useState<Row>(() => blank(fields));
  const [mode, setMode] = useState<Mode>('View');
  const [filter, setFilter] = useState('');
  const [options, setOptions] = useState<Record<string, string[]>>({});
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => {
    api<Row[]>(`/masters/${path}`)
      .then(list => { setRows(list); if (list[0]) { setSelected(String(list[0].code)); setForm(list[0]); } })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
    fields.filter(f => f.optionsFrom).forEach(f => {
      api<Row[]>(`/masters/${f.optionsFrom}`)
        .then(list => setOptions(o => ({ ...o, [f.key]: list.map(r => String(r.code)) })))
        .catch(() => undefined);
    });
  }, [path, fields, reloadKey]);

  const readOnly = mode === 'View';
  const listFields = fields.filter(f => f.showInList !== false).slice(0, 4);
  const visible = rows.filter(r => `${r.code} ${r.name ?? ''}`.toLowerCase().includes(filter.toLowerCase()));

  const select = (row: Row) => { setSelected(String(row.code)); setForm(row); setMode('View'); };

  const save = () => {
    const missing = fields.find(f => f.required && String(form[f.key] ?? '').trim() === '');
    if (missing) return setToast({ kind: 'error', text: `${missing.label} is required.` });
    const body = Object.fromEntries(fields.map(f => [f.key, f.type === 'number' ? Number(form[f.key]) || 0 : form[f.key]]));
    const request = mode === 'New'
      ? api<Row>(`/masters/${path}`, { method: 'POST', body })
      : api<Row>(`/masters/${path}/${encodeURIComponent(selected ?? '')}`, { method: 'PUT', body });
    request.then(saved => {
      setRows(mode === 'New' ? [...rows, saved] : rows.map(r => (String(r.code) === selected ? saved : r)));
      select(saved);
      setToast({ kind: 'ok', text: `${title.replace(' Master', '')} "${saved.code}" saved.` });
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const remove = () => {
    if (!selected || !window.confirm(`Delete "${selected}"?`)) return;
    api(`/masters/${path}/${encodeURIComponent(selected)}`, { method: 'DELETE' })
      .then(() => {
        const next = rows.filter(r => String(r.code) !== selected);
        setRows(next);
        if (next[0]) select(next[0]); else { setSelected(null); setForm(blank(fields)); }
        setToast({ kind: 'ok', text: `"${selected}" deleted.` });
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const cancel = () => {
    const current = rows.find(r => String(r.code) === selected);
    if (current) select(current); else { setForm(blank(fields)); setMode('View'); }
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={`${panel} flex flex-wrap items-center justify-between gap-3 px-5 py-3`}>
        <div>
          <h2 className={pageTitle}>{title}</h2>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex flex-wrap gap-2">
          {readOnly && toolbar}
          {readOnly ? (
            <>
              <button onClick={() => { setForm(blank(fields)); setMode('New'); }} className={primaryButton}><FilePlus size={14} />New</button>
              <button onClick={() => setMode('Edit')} disabled={!selected} className={secondaryButton}><Edit3 size={14} />Edit</button>
              <button onClick={remove} disabled={!selected} className={dangerButton}><Trash2 size={14} />Delete</button>
            </>
          ) : (
            <>
              <button onClick={save} className={primaryButton}><Save size={14} />Save</button>
              <button onClick={cancel} className={secondaryButton}><X size={14} />Cancel</button>
            </>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 gap-4 lg:grid-cols-[1.2fr_1fr]">
        <div className={`${panel} flex max-h-[520px] flex-col overflow-hidden`}>
          <div className="relative border-b border-[#e1c9b6] p-2 dark:border-[#343b59]">
            <Search size={12} className="absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
            <input value={filter} onChange={e => setFilter(e.target.value)} placeholder="Search code or name…" className={`${field} w-full pl-7`} />
          </div>
          <div className="flex-1 overflow-auto">
            <table className="w-full text-xs">
              <thead className={tableHead}>
                <tr>{listFields.map(f => <th key={f.key} className="px-3 py-2">{f.label}</th>)}</tr>
              </thead>
              <tbody className={tableBody}>
                {visible.map(r => (
                  <tr key={String(r.code)} onClick={() => mode === 'View' && select(r)} className={`cursor-pointer ${String(r.code) === selected ? 'bg-[#e8702a]/10' : 'hover:bg-stone-50 dark:hover:bg-[#1b2033]'}`}>
                    {listFields.map(f => (
                      <td key={f.key} className="px-3 py-2 text-stone-800 dark:text-stone-200">
                        {f.type === 'bool' ? (r[f.key] ? 'Yes' : 'No') : String(r[f.key] ?? '')}
                      </td>
                    ))}
                  </tr>
                ))}
                {!visible.length && <tr><td colSpan={listFields.length} className="px-3 py-8 text-center text-stone-400">No records.</td></tr>}
              </tbody>
            </table>
          </div>
        </div>

        <div className={`${panel} grid content-start gap-3 p-5 ${fields.length > 8 ? 'sm:grid-cols-2' : ''}`}>
          <div className="flex items-center justify-between sm:col-span-full">
            <span className="text-xs font-bold uppercase tracking-wider text-stone-500">{mode === 'New' ? 'New record' : mode === 'Edit' ? 'Editing' : 'Details'}</span>
            <span className="text-[10px] text-stone-400"><span className="text-red-600 dark:text-red-400">Red</span> = required</span>
          </div>
          {fields.map(f => {
            // The code is the key, so it can only be typed when creating a record.
            const locked = readOnly || (f.key === 'code' && mode === 'Edit');
            const value = form[f.key];
            const opts = f.options ?? options[f.key] ?? [];
            return (
              <div key={f.key}>
                {f.type === 'bool' ? (
                  <label className="flex items-center gap-2 text-xs font-semibold text-stone-700 dark:text-stone-200">
                    <input type="checkbox" disabled={locked} checked={Boolean(value)} onChange={e => setForm({ ...form, [f.key]: e.target.checked })} className="accent-[#e8702a]" />
                    {f.label}
                  </label>
                ) : (
                  <>
                    <label className={`${labelClass} ${f.required ? '!text-red-600 dark:!text-red-400' : ''}`}>{f.label}</label>
                    {f.type === 'select' || f.optionsFrom ? (
                      <select disabled={locked} value={String(value ?? '')} onChange={e => setForm({ ...form, [f.key]: e.target.value })} className={`${field} w-full`}>
                        <option value="">— select —</option>
                        {opts.map(o => <option key={o}>{o}</option>)}
                      </select>
                    ) : (
                      <input
                        type={f.type === 'number' ? 'number' : 'text'}
                        readOnly={locked}
                        value={String(value ?? '')}
                        onChange={e => setForm({ ...form, [f.key]: e.target.value })}
                        className={`${field} w-full ${locked ? 'bg-stone-100 dark:bg-[#1b2033]' : ''}`}
                      />
                    )}
                  </>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
