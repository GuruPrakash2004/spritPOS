// Utilities → Global Settings (legacy FrmUtlGSettings, table UtlGSettings, shortcut 702 / Ctrl+G).
//
// The settings grid: Set Group filter (or all groups), Fld Code, Fld Caption, Value, and what the setting
// changes in the web app. Values are edited in the grid and saved together (Save waits for its popup);
// mistakes (Y/N, numbers, dates) are shown under the value. "Show hidden settings" lists rows hidden with the
// Hide tick. Add setting creates a row of your own (Fld Code + Fld Caption), as in the desktop screen.
// SMTP rows come from backend/.env on the server and are read-only; the SMTP password is never shown.
// Administrator only (backend: /utilities/settings).

import { useCallback, useEffect, useMemo, useState } from 'react';
import { Plus, RefreshCw, Save, Search } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { api, errorText, type ApiSetting, type ApiSettingKind } from '../../lib/api';
import { badge, field, fieldError, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead, tableRow } from '../../lib/styles';

type Draft = { caption: string; value: string; hidden: boolean };

/** The value's problem, or '' when it is fine (same rules as the backend). */
function settingProblem(kind: ApiSettingKind, value: string): string {
  const v = value.trim();
  if (kind === 'yn') return /^[YyNn]$/.test(v) ? '' : 'Enter Y or N';
  if (kind === 'number') return v === '' || (Number.isFinite(Number(v)) && Number(v) >= 0) ? '' : 'Enter a number (0 or more)';
  if (kind === 'date') return v === '' || /^\d{4}-\d{2}-\d{2}$/.test(v) ? '' : 'Pick a date, or leave it empty';
  return v.length > 500 ? 'At most 500 characters' : '';
}

const EMPTY_NEW = { code: '', group: '', caption: '', value: '', kind: 'text' as ApiSettingKind };

export default function GlobalSettings() {
  const [rows, setRows] = useState<ApiSetting[]>([]);
  const [loading, setLoading] = useState(true);
  const [showHidden, setShowHidden] = useState(false);
  const [group, setGroup] = useState('');
  const [search, setSearch] = useState('');
  const [drafts, setDrafts] = useState<Record<string, Draft>>({});
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [saving, setSaving] = useState(false);
  const [adding, setAdding] = useState(false);
  const [newRow, setNewRow] = useState(EMPTY_NEW);
  const [newErrors, setNewErrors] = useState<Record<string, string>>({});

  const load = useCallback((all: boolean) => api<ApiSetting[]>(`/utilities/settings${all ? '?all=true' : ''}`)
    .then(r => { setRows(r); setDrafts({}); setErrors({}); return true; })
    .catch(e => { toastStore.add('error', errorText(e)); return false; })
    .finally(() => setLoading(false)), []);

  useEffect(() => { load(showHidden); }, [load, showHidden]);

  const groups = useMemo(() => [...new Set(rows.map(r => r.group))].sort(), [rows]);
  const shown = rows.filter(r => (!group || r.group === group)
    && (!search || `${r.code} ${r.caption} ${r.value}`.toLowerCase().includes(search.toLowerCase())));
  const changed = Object.entries(drafts).filter(([code, d]) => {
    const r = rows.find(x => x.code === code);
    return r && (d.caption !== r.caption || d.value !== r.value || d.hidden !== r.hidden);
  });

  const draftOf = (r: ApiSetting): Draft => drafts[r.code] ?? { caption: r.caption, value: r.value, hidden: r.hidden };
  const edit = (r: ApiSetting, patch: Partial<Draft>) => {
    setDrafts(d => ({ ...d, [r.code]: { ...draftOf(r), ...patch } }));
    setErrors(e => { const n = { ...e }; delete n[r.code]; return n; });
  };

  const save = async () => {
    const errs: Record<string, string> = {};
    for (const [code, d] of changed) {
      const r = rows.find(x => x.code === code)!;
      const p = settingProblem(r.kind, d.value) || (d.caption.trim() ? '' : 'Enter a caption');
      if (p) errs[code] = p;
    }
    setErrors(errs);
    if (Object.keys(errs).length) {
      document.getElementById(`gs-${Object.keys(errs)[0]}`)?.focus();
      return;
    }
    if (!changed.length) return toastStore.add('ok', 'Nothing to save.');
    const bodies = changed.map(([code, d]) => [code, { caption: d.caption.trim(), value: d.value.trim(), hidden: d.hidden }] as const);
    if (!(await toastStore.defer('ok', `${bodies.length} setting(s) will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setSaving(true);
    let saved = 0;
    try {
      for (const [code, body] of bodies) {
        await api(`/utilities/settings/${encodeURIComponent(code)}`, { method: 'PUT', body });
        saved++;
      }
      toastStore.add('ok', `Data Saved Successfully! (${saved} setting${saved === 1 ? '' : 's'})`);
    } catch (e) {
      toastStore.add('error', `${saved ? `${saved} saved, then: ` : ''}${errorText(e)}`);
    } finally {
      setSaving(false);
      load(showHidden);
    }
  };

  const refresh = async () => {
    if (!(await toastStore.defer('refresh', 'Settings will be reloaded when this closes.', 'Reload cancelled.'))) return;
    setLoading(true);
    if (await load(showHidden)) toastStore.add('refresh', 'Settings reloaded.');
  };

  const addSetting = async () => {
    const errs: Record<string, string> = {};
    if (!/^[A-Za-z][A-Za-z0-9_]{1,39}$/.test(newRow.code.trim())) errs.code = 'Start with a letter; letters, digits and _ only (2–40)';
    else if (rows.some(r => r.code.toLowerCase() === newRow.code.trim().toLowerCase())) errs.code = 'FldCode already exists';
    if (!newRow.caption.trim()) errs.caption = 'FldCaption should not be blank';
    if (!newRow.group.trim()) errs.group = 'Enter a group';
    const vp = settingProblem(newRow.kind, newRow.value);
    if (vp) errs.value = vp;
    setNewErrors(errs);
    if (Object.keys(errs).length) {
      document.getElementById(`gs-new-${Object.keys(errs)[0]}`)?.focus();
      return;
    }
    const body = { ...newRow, code: newRow.code.trim(), caption: newRow.caption.trim(), group: newRow.group.trim(), value: newRow.value.trim() };
    if (!(await toastStore.defer('ok', `Setting ${body.code} will be added when this closes.`, 'Add cancelled.'))) return;
    api<ApiSetting>('/utilities/settings', { method: 'POST', body })
      .then(() => { toastStore.add('ok', `Setting ${body.code} added.`); setNewRow(EMPTY_NEW); setAdding(false); load(showHidden); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  const valueInput = (r: ApiSetting) => {
    const d = draftOf(r);
    const cls = `${field} ${errors[r.code] ? fieldError : ''}`;
    const common = { id: `gs-${r.code}`, 'aria-label': `Value of ${r.code}`, disabled: r.readonly, 'aria-invalid': !!errors[r.code] };
    if (r.kind === 'yn') {
      return (
        <select {...common} value={d.value.toUpperCase()} onChange={e => edit(r, { value: e.target.value })} className={cls}>
          <option value="Y">Y</option><option value="N">N</option>
        </select>
      );
    }
    return <input {...common} type={r.kind === 'date' ? 'date' : 'text'} inputMode={r.kind === 'number' ? 'decimal' : undefined}
      value={d.value} onChange={e => edit(r, { value: e.target.value })} className={cls} />;
  };

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Global Settings</h1>
          <p className={pageSub}>Each setting says what it changes in the web app. Edit values in the grid, then Save.</p>
        </div>
        <div className="flex flex-wrap gap-2">
          <button className={secondaryButton} onClick={() => setAdding(a => !a)}><Plus size={14} />Add setting</button>
          <button className={secondaryButton} onClick={refresh} disabled={loading || saving}><RefreshCw size={14} className={loading ? 'animate-spin' : ''} />Reload</button>
          <button className={primaryButton} onClick={save} disabled={saving || loading}><Save size={14} />{saving ? 'Saving…' : `Save${changed.length ? ` (${changed.length})` : ''}`}</button>
        </div>
      </div>

      {adding && (
        <section className={`${panel} p-4`} aria-label="Add setting">
          <h2 className={sectionTitle}>Add setting</h2>
          <p className="mb-3 text-[11px] text-stone-500">A setting of your own, kept with the others. The web app doesn't read it.</p>
          <div className="grid gap-3 sm:grid-cols-5">
            {([['code', 'Fld Code'], ['caption', 'Fld Caption'], ['group', 'Set Group'], ['value', 'Value']] as const).map(([k, l]) => (
              <div key={k}>
                <label htmlFor={`gs-new-${k}`} className={label}>{l}</label>
                <input id={`gs-new-${k}`} value={newRow[k]} list={k === 'group' ? 'gs-groups' : undefined}
                  onChange={e => { setNewRow(n => ({ ...n, [k]: e.target.value })); setNewErrors(x => ({ ...x, [k]: '' })); }}
                  className={`${field} ${newErrors[k] ? fieldError : ''}`} aria-invalid={!!newErrors[k]} />
                <InlineError message={newErrors[k]} />
              </div>
            ))}
            <div>
              <label htmlFor="gs-new-kind" className={label}>Type</label>
              <select id="gs-new-kind" value={newRow.kind} onChange={e => setNewRow(n => ({ ...n, kind: e.target.value as ApiSettingKind }))} className={field}>
                <option value="text">Text</option><option value="yn">Y / N</option><option value="number">Number</option><option value="date">Date</option>
              </select>
            </div>
          </div>
          <datalist id="gs-groups">{groups.map(g => <option key={g} value={g} />)}</datalist>
          <div className="mt-3 flex justify-end gap-2">
            <button className={secondaryButton} onClick={() => { setAdding(false); setNewErrors({}); }}>Cancel</button>
            <button className={primaryButton} onClick={addSetting}><Plus size={14} />Add</button>
          </div>
        </section>
      )}

      <section className={`${panel} overflow-hidden`}>
        <div className="flex flex-wrap items-end gap-3 border-b border-[#E3D9CD] p-4 dark:border-line">
          <div>
            <label htmlFor="gs-group" className={label}>Set Group</label>
            <select id="gs-group" value={group} onChange={e => setGroup(e.target.value)} className={field}>
              <option value="">Show all groups</option>
              {groups.map(g => <option key={g} value={g}>{g}</option>)}
            </select>
          </div>
          <div className="relative min-w-[220px] flex-1">
            <label htmlFor="gs-search" className={label}>Search</label>
            <Search size={14} className="pointer-events-none absolute bottom-2.5 left-3 text-stone-400" />
            <input id="gs-search" value={search} onChange={e => setSearch(e.target.value)} className={`${field} pl-8`} placeholder="Code, caption or value" />
          </div>
          <label className="flex items-center gap-2 pb-2 text-xs text-stone-700 dark:text-stone-200">
            <input type="checkbox" checked={showHidden} onChange={e => { setLoading(true); setShowHidden(e.target.checked); }} />Show hidden settings
          </label>
        </div>
        <div className="max-h-[65vh] overflow-auto">
          <table className="min-w-full text-xs">
            <thead>
              <tr>
                {['Set Group', 'Fld Code', 'Fld Caption', 'Value', 'What it changes', 'Hide'].map(h => <th key={h} className={`${tableHead} px-3 py-2`}>{h}</th>)}
              </tr>
            </thead>
            <tbody className={tableBody}>
              {shown.map(r => {
                const d = draftOf(r);
                return (
                  <tr key={r.code} className={`${tableRow} align-top`}>
                    <td className="px-3 py-2 text-stone-500">{r.group}</td>
                    <td className="px-3 py-2 font-mono">{r.code}</td>
                    <td className="px-3 py-2">
                      <input aria-label={`Caption of ${r.code}`} value={d.caption} disabled={r.readonly} onChange={e => edit(r, { caption: e.target.value })} className={field} />
                      {r.remarks && <p className="mt-1 text-[11px] text-stone-500">{r.remarks}</p>}
                    </td>
                    <td className="w-48 px-3 py-2">{valueInput(r)}<InlineError message={errors[r.code]} /></td>
                    <td className="px-3 py-2 text-[11px] text-stone-600 dark:text-stone-300">
                      {r.source === 'custom' ? <span className={badge('neutral')}>Your own setting</span>
                        : r.readonly ? <span className={badge('neutral')}>Set in backend/.env</span> : r.effect}
                      {r.readonly && r.effect && <div className="mt-1">{r.effect}</div>}
                    </td>
                    <td className="px-3 py-2 text-center">
                      <input type="checkbox" aria-label={`Hide ${r.code}`} checked={d.hidden} disabled={r.readonly} onChange={e => edit(r, { hidden: e.target.checked })} />
                    </td>
                  </tr>
                );
              })}
              {!shown.length && <tr><td colSpan={6} className="px-3 py-6 text-center text-stone-500">{loading ? 'Loading settings…' : 'No settings match.'}</td></tr>}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}
