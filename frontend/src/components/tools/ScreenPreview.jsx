// Tools → Screen Preview (Administrator only). Lists every screen the Doc Designer defines, what the server builds
// from it together with the table's real columns (GET /designer/preview, backend/app/schema_meta.py), and what
// looks wrong: a column gone from the table, a mandatory field the web cannot write, a width longer than the
// column, a formula or drop-down the web app does not run yet. Read only; nothing here changes the database.

import { useEffect, useMemo, useState } from 'react';
import { ChevronDown, ChevronRight, RefreshCw, Search } from 'lucide-react';
import { errorText, getScreenPreview } from '../../lib/api';
import { badge, field, pageHeader, pageSub, pageTitle, panel, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const TONE = { error: 'danger', warn: 'warning', info: 'info' };
const LEVELS = ['error', 'warn', 'info'];

// The worst problem level of a screen ('' when clean).
const worst = (problems) => LEVELS.find(l => problems.some(p => p.level === l)) ?? '';

const flags = (f) => [f.mandatory && 'mandatory', f.readonly && 'read-only', f.hidden && 'hidden', f.unique && 'unique',
  f.formula && 'formula', f.combo && 'drop-down', f.standard && 'from table'].filter(Boolean);

export default function ScreenPreview() {
  const [data, setData] = useState(null);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);
  const [filter, setFilter] = useState('');
  const [only, setOnly] = useState('all');   // all | problems
  const [open, setOpen] = useState(null);

  // "Read again": the database may have changed since the screen opened.
  const load = () => {
    setBusy(true);
    setError('');
    getScreenPreview().then(setData).catch(e => setError(errorText(e))).finally(() => setBusy(false));
  };
  useEffect(() => { getScreenPreview().then(setData).catch(e => setError(errorText(e))); }, []);

  const screens = useMemo(() => {
    const q = filter.trim().toLowerCase();
    return (data?.screens ?? []).filter(s =>
      (only === 'all' || worst(s.problems) === 'error' || worst(s.problems) === 'warn')
      && (!q || `${s.name} ${s.short_form} ${s.header_table} ${s.type}`.toLowerCase().includes(q)));
  }, [data, filter, only]);

  const totals = useMemo(() => {
    const t = { error: 0, warn: 0, info: 0 };
    for (const s of data?.screens ?? []) for (const p of s.problems) t[p.level] += 1;
    return t;
  }, [data]);

  return (
    <div className="mx-auto max-w-6xl">
      <div className={`${pageHeader} mb-4`}>
        <div>
          <h1 className={pageTitle}>Screen Preview</h1>
          <p className={pageSub}>What the web app builds from the Doc Designer and the database, and what looks wrong. Read only.</p>
        </div>
        <button onClick={load} disabled={busy} className={secondaryButton}><RefreshCw size={14} className={busy ? 'animate-spin' : ''} />Read again</button>
      </div>

      {error && <p role="alert" className="mb-3 text-xs text-[#9A4B45] dark:text-red-400">{error}</p>}

      <div className="mb-3 flex flex-wrap items-center gap-2">
        <div className="relative min-w-0 flex-1 basis-56">
          <Search size={14} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
          <input value={filter} onChange={e => setFilter(e.target.value)} placeholder="Search screen or table…" aria-label="Search screens" className={`${field} pl-8`} />
        </div>
        <select value={only} onChange={e => setOnly(e.target.value)} aria-label="Show" className={`${field} !w-auto`}>
          <option value="all">All screens</option>
          <option value="problems">Only with errors or warnings</option>
        </select>
        <span className={badge('danger')}>{totals.error} errors</span>
        <span className={badge('warning')}>{totals.warn} warnings</span>
        <span className={badge('info')}>{totals.info} notes</span>
      </div>

      <section className={`${panel} overflow-hidden`} aria-label="Screens">
        {!data && !error && <p className="p-8 text-center text-xs text-stone-500">Reading the database…</p>}
        {data && !screens.length && <p className="p-8 text-center text-xs text-stone-500">{data.screens.length ? 'No screen matches.' : 'The Doc Designer has no screens in this database.'}</p>}
        <ul className="divide-y divide-[#E8ECF3] dark:divide-line-soft">
          {screens.map(s => {
            const isOpen = open === s.doc_id;
            const level = worst(s.problems);
            return (
              <li key={s.doc_id}>
                <button onClick={() => setOpen(isOpen ? null : s.doc_id)} aria-expanded={isOpen}
                  className="flex w-full min-w-0 flex-wrap items-center gap-x-3 gap-y-1 px-4 py-3 text-left hover:bg-subtle/70 focus-visible:bg-subtle">
                  {isOpen ? <ChevronDown size={14} className="shrink-0" /> : <ChevronRight size={14} className="shrink-0" />}
                  <span className="min-w-0 flex-1 basis-48 truncate text-sm font-medium text-stone-900 dark:text-stone-50">{s.name || `Document ${s.doc_id}`}</span>
                  <span className="text-[11px] text-stone-500">{s.type} · {s.header_table || 'no table'}{s.detail_table ? ` + ${s.detail_table}` : ''}</span>
                  <span className="text-[11px] tabular-nums text-stone-500">{s.shown_fields} shown / {s.designer_fields} designed</span>
                  {s.master_path ? <span className={badge('success')}>web screen</span> : s.type === 'Master' ? <span className={badge('neutral')}>no web screen</span> : null}
                  {level ? <span className={badge(TONE[level])}>{s.problems.filter(p => p.level === level).length} {level === 'error' ? 'errors' : level === 'warn' ? 'warnings' : 'notes'}</span> : <span className={badge('success')}>clean</span>}
                </button>

                {isOpen && (
                  <div className="space-y-4 border-t border-line bg-subtle/30 p-4">
                    {s.problems.length > 0 && (
                      <ul className="space-y-1.5 text-xs" aria-label="Problems">
                        {[...s.problems].sort((a, b) => LEVELS.indexOf(a.level) - LEVELS.indexOf(b.level)).map((p, i) => (
                          <li key={i} className="flex min-w-0 items-start gap-2">
                            <span className={`${badge(TONE[p.level])} shrink-0`}>{p.level}</span>
                            <span className="min-w-0 break-words">{p.field && <b className="mr-1 font-semibold">{p.field}</b>}{p.text}</span>
                          </li>
                        ))}
                      </ul>
                    )}
                    <div className="max-h-[420px] overflow-auto rounded-xl border border-line">
                      <table className="w-full text-xs">
                        <thead className={tableHead}>
                          <tr>{['#', 'Section', 'Field', 'Caption', 'Type', 'Width', 'Flags'].map(h => <th key={h} className="px-3 py-2">{h}</th>)}</tr>
                        </thead>
                        <tbody className={tableBody}>
                          {s.fields.map((f, i) => (
                            <tr key={f.code} className={f.hidden ? 'opacity-60' : ''}>
                              <td className="px-3 py-1.5 tabular-nums text-stone-500">{i + 1}</td>
                              <td className="px-3 py-1.5">{f.section}{f.sub_section ? ` › ${f.sub_section}` : ''}</td>
                              <td className="px-3 py-1.5 font-medium">{f.code}</td>
                              <td className="max-w-[16rem] truncate px-3 py-1.5" title={f.caption}>{f.caption}</td>
                              <td className="px-3 py-1.5">{f.type}</td>
                              <td className="px-3 py-1.5 tabular-nums">{f.width || '—'}</td>
                              <td className="px-3 py-1.5">{flags(f).join(', ') || '—'}</td>
                            </tr>
                          ))}
                          {!s.fields.length && <tr><td colSpan={7} className="px-3 py-6 text-center text-stone-500">No fields.</td></tr>}
                        </tbody>
                      </table>
                    </div>
                  </div>
                )}
              </li>
            );
          })}
        </ul>
      </section>
    </div>
  );
}
