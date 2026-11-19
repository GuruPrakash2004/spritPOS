// Item Vs Brand (legacy frmUtlItemVsBrand): choose Linked or Unlinked items, Load them, set "Under Brand" row by row
// (Replace Rest copies the current row's brand to the rows below it that are shown), then Save.
// Keyboard: Alt+L load, Alt+R replace rest, Alt+S or ⌘/Ctrl+S save, Alt+C cancel (drops the unsaved choices).
// The top of the screen counts every item (Total / Linked / Unlinked, read from the Item Master). The list draws PAGE rows first and PAGE more each
// time the grid is scrolled to its end, so thousands of items (4,699 unlinked) never become thousands of drop-downs at once; Total Records
// at the bottom is "matching the search / loaded", and Replace Rest still reaches every loaded row below the current one.

import { useEffect, useMemo, useRef, useState } from 'react';
import { Download, Replace, Save, X } from 'lucide-react';
import Kbd from '../common/Kbd';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { useShortcuts } from '../../lib/keys';
import { assignItemBrands, errorText, getBrands, getItems } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import BackToMaster from '../common/BackToMaster';

const PAGE = 20;   // rows drawn first, and added on every scroll to the end
// The desktop's "no brand" row (BrandID 0) is named "-": an item holding it is not linked to any brand.
const brandOf = (i) => (i.brand && String(i.brand).trim() !== '-' ? i.brand : '');

export default function ItemVsBrand() {
  const [mode, setMode] = useState('unlinked');
  const [brands, setBrands] = useState([]);
  const [rows, setRows] = useState(null);   // null until Load; { code, group, name, uom, brand, was }
  const [search, setSearch] = useState('');
  const [current, setCurrent] = useState(0);
  const [problem, setProblem] = useState('');
  const [counts, setCounts] = useState(null);   // { total, linked, unlinked } of the whole Item Master
  const [limit, setLimit] = useState(PAGE);      // rows drawn so far
  const gridRef = useRef(null);

  const countOf = (items) => { const linked = items.filter(i => !!brandOf(i)).length; return { total: items.length, linked, unlinked: items.length - linked }; };
  useEffect(() => { getItems().then(items => setCounts(countOf(items))).catch(() => undefined); }, []);
  useEffect(() => { getBrands().then(l => setBrands(l.map(b => b.code))).catch(e => setProblem(errorText(e))); }, []);

  const load = () => {
    setProblem('');
    getItems().then(items => {
      const list = items.filter(i => (mode === 'linked' ? !!brandOf(i) : !brandOf(i)))
        .sort((a, b) => (a.group || '').localeCompare(b.group || ''))
        .map(i => ({ code: i.code, group: i.group, name: i.name, uom: i.uom, brand: brandOf(i), was: brandOf(i) }));
      setRows(list); setSearch(''); setCurrent(0); setLimit(PAGE); setCounts(countOf(items));
      if (!list.length) setProblem('No Records Found!');
    }).catch(e => setProblem(errorText(e)));
  };

  const text = search.trim().toLowerCase();
  const shown = useMemo(() => (rows || []).map((r, i) => ({ ...r, i })).filter(r => !text || r.code.toLowerCase().includes(text) || r.name.toLowerCase().includes(text)), [rows, text]);
  const visible = useMemo(() => shown.slice(0, limit), [shown, limit]);
  const typeSearch = (v) => { setSearch(v); setLimit(PAGE); gridRef.current?.scrollTo?.({ top: 0 }); };   // a new search starts from the top
  const more = () => setLimit(l => (l < shown.length ? l + PAGE : l));
  const onScroll = (e) => { const el = e.currentTarget; if (el.scrollTop + el.clientHeight >= el.scrollHeight - 60) more(); };
  useEffect(() => {   // a tall window can show all PAGE rows without a scrollbar: keep adding until it fills or the list ends
    const el = gridRef.current;
    if (el && el.clientHeight > 0 && el.scrollHeight <= el.clientHeight && limit < shown.length) setLimit(l => l + PAGE);
  }, [limit, shown.length]);
  const setBrand = (i, brand) => setRows(rs => rs.map((r, x) => (x === i ? { ...r, brand } : r)));
  const replaceRest = () => {
    if (!rows?.[current]) return;
    const brand = rows[current].brand;
    const from = shown.findIndex(r => r.i === current);
    const ids = new Set(shown.slice(Math.max(from, 0)).map(r => r.i));
    setRows(rs => rs.map((r, x) => (ids.has(x) ? { ...r, brand } : r)));
  };
  const changed = (rows || []).filter(r => r.brand !== r.was);
  const cancel = () => { setRows(rs => (rs ? rs.map(r => ({ ...r, brand: r.was })) : rs)); setProblem(''); };   // drop the unsaved brand choices
  const save = async () => {
    if (!changed.length) return setProblem('There are no changes to save.');
    setProblem('');
    if (!(await toastStore.defer('ok', `${changed.length} item(s) will be linked to their brands when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    assignItemBrands(changed.map(r => ({ item_code: r.code, brand: r.brand })))
      .then(() => { toastStore.add('ok', 'Successfully Saved!'); load(); })
      .catch(e => setProblem(errorText(e)));
  };
  useShortcuts({ 'Alt+L': load, 'Alt+R': replaceRest, 'Alt+S': () => { void save(); }, 'Alt+C': cancel, 'Mod+S': () => { void save(); } });

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Item Vs Brand</h1>
          <p className={pageSub}>Link items to their brands, one by one or from a row downwards.</p>
        </div>
        <BackToMaster />
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
            <input id="ivb-search" value={search} onChange={e => typeSearch(e.target.value)} className={`${field} w-56`} />
          </div>
        )}
      </div>
      {/* Save and Cancel sit right above the list they apply to, beside the counts. */}
      <div className="flex flex-wrap items-center gap-2 text-xs">
        {counts && (
          <div className="flex flex-wrap gap-2" aria-label="Item counts">
            <span className={`${panel} px-3 py-1.5`}>Total Items: <b>{counts.total}</b></span>
            <span className={`${panel} px-3 py-1.5`}>Linked: <b>{counts.linked}</b></span>
            <span className={`${panel} px-3 py-1.5`}>Unlinked: <b>{counts.unlinked}</b></span>
          </div>
        )}
        <div className="ml-auto flex items-center gap-2">
          {changed.length > 0 && <span className="text-amber-700 dark:text-amber-300">{changed.length} unsaved change{changed.length === 1 ? '' : 's'}</span>}
          <button type="button" onClick={cancel} disabled={!changed.length} className={secondaryButton}><X size={14} />Cancel<Kbd combo="Alt+C" /></button>
          <button type="button" onClick={() => void save()} disabled={!changed.length} className={primaryButton}><Save size={14} />Save<Kbd combo="Alt+S" /></button>
        </div>
      </div>
      {problem && <InlineError message={problem} />}
      <div ref={gridRef} onScroll={onScroll} className={`${panel} max-h-[62vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr><th className="px-3 py-2">SlNo</th><th className="px-3 py-2">ItemGroup</th><th className="px-3 py-2">ItemCode</th><th className="px-3 py-2">ItemName</th><th className="px-3 py-2">Uom</th><th className="px-3 py-2">Under Brand</th></tr>
          </thead>
          <tbody className={tableBody}>
            {!rows && <tr><td colSpan={6} className="px-4 py-14 text-center text-stone-500">Choose Linked or Unlinked items and press Load.</td></tr>}
            {visible.map((r, n) => (
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
      {rows && (
        <div className="text-xs text-stone-500">
          Total Records: {shown.length}/{rows.length} {mode === 'linked' ? 'linked' : 'unlinked'}
          {visible.length < shown.length ? ` · showing ${visible.length}, scroll down for ${Math.min(PAGE, shown.length - visible.length)} more` : ` · showing all ${visible.length}`}
        </div>
      )}
    </div>
  );
}
