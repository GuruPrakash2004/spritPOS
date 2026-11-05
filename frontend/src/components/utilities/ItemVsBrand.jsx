// Item Vs Brand (legacy frmUtlItemVsBrand): choose Linked or Unlinked items, Load them, set "Under Brand" row by row
// (Replace Rest copies the current row's brand to the rows below it that are shown), then Save.
// Keyboard: Alt+L load, Alt+R replace rest, Alt+S or ⌘/Ctrl+S save.

import { useEffect, useMemo, useState } from 'react';
import { Download, Replace, Save } from 'lucide-react';
import Kbd from '../common/Kbd';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { useShortcuts } from '../../lib/keys';
import { assignItemBrands, errorText, getBrands, getItems } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export default function ItemVsBrand() {
  const [mode, setMode] = useState('unlinked');
  const [brands, setBrands] = useState([]);
  const [rows, setRows] = useState(null);   // null until Load; { code, group, name, uom, brand, was }
  const [search, setSearch] = useState('');
  const [current, setCurrent] = useState(0);
  const [problem, setProblem] = useState('');

  useEffect(() => { getBrands().then(l => setBrands(l.map(b => b.code))).catch(e => setProblem(errorText(e))); }, []);

  const load = () => {
    setProblem('');
    getItems().then(items => {
      const list = items.filter(i => (mode === 'linked' ? !!i.brand : !i.brand))
        .sort((a, b) => (a.group || '').localeCompare(b.group || ''))
        .map(i => ({ code: i.code, group: i.group, name: i.name, uom: i.uom, brand: i.brand || '', was: i.brand || '' }));
      setRows(list); setSearch(''); setCurrent(0);
      if (!list.length) setProblem('No Records Found!');
    }).catch(e => setProblem(errorText(e)));
  };

  const text = search.trim().toLowerCase();
  const shown = useMemo(() => (rows || []).map((r, i) => ({ ...r, i })).filter(r => !text || r.code.toLowerCase().includes(text) || r.name.toLowerCase().includes(text)), [rows, text]);
  const setBrand = (i, brand) => setRows(rs => rs.map((r, x) => (x === i ? { ...r, brand } : r)));
  const replaceRest = () => {
    if (!rows?.[current]) return;
    const brand = rows[current].brand;
    const from = shown.findIndex(r => r.i === current);
    const ids = new Set(shown.slice(Math.max(from, 0)).map(r => r.i));
    setRows(rs => rs.map((r, x) => (ids.has(x) ? { ...r, brand } : r)));
  };
  const changed = (rows || []).filter(r => r.brand !== r.was);
  const save = async () => {
    if (!changed.length) return setProblem('There are no changes to save.');
    setProblem('');
    if (!(await toastStore.defer('ok', `${changed.length} item(s) will be linked to their brands when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    assignItemBrands(changed.map(r => ({ item_code: r.code, brand: r.brand })))
      .then(() => { toastStore.add('ok', 'Successfully Saved!'); load(); })
      .catch(e => setProblem(errorText(e)));
  };
  useShortcuts({ 'Alt+L': load, 'Alt+R': replaceRest, 'Alt+S': () => { void save(); }, 'Mod+S': () => { void save(); } });

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Item Vs Brand</h1>
          <p className={pageSub}>Link items to their brands, one by one or from a row downwards.</p>
        </div>
        <button onClick={() => void save()} disabled={!changed.length} className={primaryButton}><Save size={14} />Save<Kbd combo="Alt+S" /></button>
      </div>
      <div className={`${panel} flex flex-wrap items-end gap-4 px-5 py-3`}>
        <fieldset className="flex items-center gap-4 text-xs">
          <legend className={label}>Brand Link</legend>
          <label className="flex items-center gap-1.5"><input type="radio" name="ivb-mode" checked={mode === 'linked'} onChange={() => setMode('linked')} />Linked Items</label>
          <label className="flex items-center gap-1.5"><input type="radio" name="ivb-mode" checked={mode === 'unlinked'} onChange={() => setMode('unlinked')} />Unlinked Items</label>
        </fieldset>
        <button type="button" onClick={load} className={secondaryButton}><Download size={14} />Load<Kbd combo="Alt+L" /></button>
        <button type="button" onClick={replaceRest} disabled={!rows?.length} className={secondaryButton}><Replace size={14} />Replace Rest<Kbd combo="Alt+R" /></button>
        {rows?.length > 0 && (
          <div className="ml-auto">
            <label htmlFor="ivb-search" className={label}>Search Item</label>
            <input id="ivb-search" value={search} onChange={e => setSearch(e.target.value)} className={`${field} w-56`} />
          </div>
        )}
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} max-h-[62vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr><th className="px-3 py-2">SlNo</th><th className="px-3 py-2">ItemGroup</th><th className="px-3 py-2">ItemCode</th><th className="px-3 py-2">ItemName</th><th className="px-3 py-2">Uom</th><th className="px-3 py-2">Under Brand</th></tr>
          </thead>
          <tbody className={tableBody}>
            {!rows && <tr><td colSpan={6} className="px-4 py-14 text-center text-stone-500">Choose Linked or Unlinked items and press Load.</td></tr>}
            {shown.map((r, n) => (
              <tr key={r.code} className={r.brand !== r.was ? 'bg-amber-50/60 dark:bg-amber-500/5' : ''} onClick={() => setCurrent(r.i)} aria-selected={current === r.i}>
                <td className="px-3 py-1.5">{n + 1}</td><td className="px-3 py-1.5">{r.group}</td><td className="px-3 py-1.5">{r.code}</td><td className="px-3 py-1.5">{r.name}</td><td className="px-3 py-1.5">{r.uom}</td>
                <td className="px-3 py-1.5">
                  <select value={r.brand} onFocus={() => setCurrent(r.i)} onChange={e => setBrand(r.i, e.target.value)} className={`${field} w-44`} aria-label={`Brand for ${r.name}`}>
                    <option value="">— none —</option>
                    {brands.map(b => <option key={b} value={b}>{b}</option>)}
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {rows && <div className="text-xs text-stone-500">Total Records: {shown.length}/{rows.length}</div>}
    </div>
  );
}
