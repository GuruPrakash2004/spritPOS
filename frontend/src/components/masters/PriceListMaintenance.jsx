// Item Price List Maintenance (the desktop's frmMstPriceMaintenance), in its two steps:
//  1. Selection: choose Item Groups (several) or Items, and the rate columns to change (MRP, Purchase Rate); Load.
//  2. The items with each rate's Current and New value. Tick the rows to change, then Increase / Decrease by an amount or a percentage (a percentage
//     can be rounded: 50p RoundOff or 1Rs RoundOff) and Apply: the New column fills in. A New rate can also be typed row by row, and Replace for rest
//     of the list copies the typed rate to the ticked rows below. Update to DB writes the New rates (it asks first: the old ones are overwritten).
// A rate may not go negative and a new MRP must be above zero. Export gives the grid as an Excel sheet. Back returns to the selection.

import { useEffect, useMemo, useState } from 'react';
import { ArrowLeft, CheckCircle2, Download, FileSpreadsheet, Replace, Save } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getItemGroups, getItems, getRateColumns, setItemRates } from '../../lib/api';
import { changedRate, ROUNDINGS } from '../../lib/priceRound';
import { downloadXlsRows } from '../../lib/xlsExport';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

// A rate column is a typed Item Master field (item.mrp) or one of the item's extra columns (item.extras.MINLEVEL); both read the same way.
const rateOf = (item, key) => Number(item[key] ?? item.extras?.[key]) || 0;

export default function PriceListMaintenance() {
  const { confirm, dialog } = useConfirm();
  const [all, setAll] = useState([]);
  const [groups, setGroups] = useState([]);
  const [by, setBy] = useState('group');                  // 'group' | 'item'
  const [pickedGroups, setPickedGroups] = useState(new Set());
  const [pickedItems, setPickedItems] = useState(new Set());
  const [find, setFind] = useState('');
  const [columns, setColumns] = useState([]);          // the Item Rate Columns the shop's Item Master has: [{ key, caption }]
  const [cols, setCols] = useState([]);
  const [problem, setProblem] = useState('');
  const [rows, setRows] = useState(null);                 // step 2: the loaded items; null = step 1
  const [ticked, setTicked] = useState(new Set());
  const [edits, setEdits] = useState({});                 // { code: { mrp: '12', purchase_rate: '10' } } the New rates
  const [last, setLast] = useState(null);                 // { code, col } the cell typed in last, for Replace for rest
  const [direction, setDirection] = useState('inc');
  const [mode, setMode] = useState('per');
  const [amount, setAmount] = useState('');
  const [round, setRound] = useState('');

  useEffect(() => {
    getItems().then(setAll).catch(e => setProblem(errorText(e)));
    getItemGroups().then(g => setGroups(g.map(x => x.code))).catch(() => undefined);
    getRateColumns().then(setColumns).catch(e => setProblem(errorText(e)));
  }, []);

  const caption = (key) => columns.find(c => c.key === key)?.caption ?? key;
  const toggleSet = (set, setter, value) => { const n = new Set(set); if (n.has(value)) n.delete(value); else n.add(value); setter(n); };
  const text = find.trim().toLowerCase();
  const matches = useMemo(() => all.filter(i => !text || i.code.toLowerCase().includes(text) || i.name.toLowerCase().includes(text)).slice(0, 200), [all, text]);

  const load = () => {
    if (!cols.length) return setProblem('Please select at least one Rate column to update.');
    const list = by === 'group' ? all.filter(i => pickedGroups.has(i.group)) : all.filter(i => pickedItems.has(i.code));
    if (!list.length) return setProblem(by === 'group' ? 'Please select at least one Item Group.' : 'Please select at least one Item.');
    setProblem('');
    setRows(list);
    setTicked(new Set(list.map(i => i.code)));
    setEdits({});
    setLast(null);
  };

  const newOf = (item, col) => (edits[item.code]?.[col] !== undefined && edits[item.code][col] !== '' ? Number(edits[item.code][col]) : rateOf(item, col));
  const setNew = (code, col, value) => setEdits(e => ({ ...e, [code]: { ...e[code], [col]: value } }));

  const apply = () => {
    if (!rows?.length) return setProblem('No Items present. Cannot continue.');
    const chosen = rows.filter(i => ticked.has(i.code));
    if (!chosen.length) return setProblem('Please select at least one Item to update Rate to new values.');
    const next = {};
    for (const item of chosen) {
      next[item.code] = {};
      for (const col of cols) {
        const n = changedRate(rateOf(item, col), { direction, mode, value: amount, round });
        if (n < 0) return setProblem('Rates cannot go Negative.');
        next[item.code][col] = String(n);
      }
    }
    setProblem('');
    setEdits(e => ({ ...e, ...next }));
    toastStore.add('ok', "Price alterations applied. Click 'Update to DB' to save the new rates.");
  };

  const replaceRest = () => {
    if (!last) return setProblem('Type a new rate in a row first.');
    const at = rows.findIndex(i => i.code === last.code);
    if (!ticked.has(last.code)) return setProblem(`Please select ${rows[at]?.name} (the current row) to give its new rate to the rest of the selected items.`);
    const rate = newOf(rows[at], last.col);
    const below = rows.slice(at + 1).filter(i => ticked.has(i.code));
    setEdits(e => ({ ...e, ...Object.fromEntries(below.map(i => [i.code, { ...e[i.code], [last.col]: String(rate) }])) }));
    setProblem('');
    toastStore.add('ok', below.length ? `Rate ${rate} given to the ${below.length} selected items below.` : 'No item is selected below the current row.');
  };

  const update = async () => {
    const chosen = rows.filter(i => ticked.has(i.code));
    if (!chosen.length) return setProblem('Please select at least one Item to update Rate to new values.');
    const payload = chosen.map(i => ({ item_code: i.code, values: Object.fromEntries(cols.map(c => [c, newOf(i, c)])) }));
    if (cols.includes('mrp') && payload.some(p => !(p.values.mrp > 0))) return setProblem('Price cannot be negative or zero. Please enter a valid Price.');
    if (payload.some(p => cols.some(c => p.values[c] < 0))) return setProblem('Rates cannot go Negative.');
    setProblem('');
    if (!(await confirm(`You are about to update rate for ${chosen.length} Item(s).\n\nIf you click Yes all existing rates will be over-written and cannot be reverted back!!!\n\nAre you sure you want to continue?`))) return;
    if (!(await toastStore.defer('ok', `The new rates of ${chosen.length} items will be saved when this closes.`, 'Rate update cancelled. Nothing was changed.'))) return;
    try {
      const r = await setItemRates(payload);
      toastStore.add('ok', `${r.changed} rate${r.changed === 1 ? '' : 's'} updated on ${r.items} items.`);
      const fresh = await getItems();
      setAll(fresh);
      setRows(rows.map(i => fresh.find(f => f.code === i.code) ?? i));
      setEdits({});
    } catch (e) { setProblem(errorText(e)); }
  };

  const exportGrid = () => downloadXlsRows('PriceList', [
    ['Code', 'Item', 'Group', ...cols.flatMap(c => [caption(c), `${caption(c)}-New`])],
    ...rows.map(i => [i.code, i.name, i.group, ...cols.flatMap(c => [rateOf(i, c), newOf(i, c)])]),
  ]);

  const radio = 'flex items-center gap-1.5 text-sm';

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {dialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Item Price List Maintenance</h1>
          <p className={pageSub}>{rows ? 'Update Rate' : 'Selection'}</p>
        </div>
      </div>
      {problem && <InlineError message={problem} />}

      {!rows ? (
        <div className="grid gap-4 lg:grid-cols-[1fr_260px]">
          <div className={`${panel} p-4`}>
            <div className="mb-3 flex gap-5" role="radiogroup" aria-label="Select by">
              <label className={radio}><input type="radio" name="pl-by" checked={by === 'group'} onChange={() => setBy('group')} />Item Group</label>
              <label className={radio}><input type="radio" name="pl-by" checked={by === 'item'} onChange={() => setBy('item')} />Item</label>
            </div>
            {by === 'group' ? (
              <>
                <div className="mb-2 flex gap-2">
                  <button onClick={() => setPickedGroups(new Set(groups))} className={`${secondaryButton} !px-3 !py-1 text-xs`}>Select all</button>
                  <button onClick={() => setPickedGroups(new Set())} className={`${secondaryButton} !px-3 !py-1 text-xs`}>Clear</button>
                </div>
                <div className="grid max-h-80 grid-cols-2 gap-1 overflow-auto rounded-lg border border-line p-2 sm:grid-cols-3">
                  {groups.map(g => <label key={g} className={radio}><input type="checkbox" checked={pickedGroups.has(g)} onChange={() => toggleSet(pickedGroups, setPickedGroups, g)} />{g}</label>)}
                </div>
              </>
            ) : (
              <>
                <input value={find} onChange={e => setFind(e.target.value)} placeholder="Search an item by code or name" aria-label="Search items" className={`${field} mb-2`} />
                <div className="max-h-80 overflow-auto rounded-lg border border-line">
                  {matches.map(i => (
                    <label key={i.code} className="flex cursor-pointer items-center gap-2 border-b border-line px-3 py-1 text-sm last:border-0 hover:bg-subtle">
                      <input type="checkbox" checked={pickedItems.has(i.code)} onChange={() => toggleSet(pickedItems, setPickedItems, i.code)} />
                      <span className="tabular-nums text-stone-500">{i.code}</span><span className="flex-1 truncate">{i.name}</span>
                    </label>
                  ))}
                </div>
                <p className="mt-1 text-xs text-stone-500">{pickedItems.size} item{pickedItems.size === 1 ? '' : 's'} chosen</p>
              </>
            )}
          </div>
          <div className={`${panel} flex flex-col gap-3 p-4`}>
            <p className={label}>Rate columns to update</p>
            {columns.map(c => <label key={c.key} className={radio}><input type="checkbox" checked={cols.includes(c.key)} onChange={() => setCols(cur => (cur.includes(c.key) ? cur.filter(x => x !== c.key) : [...cur, c.key]))} />{c.caption}</label>)}
            <p className="mt-auto text-sm font-semibold text-accent-ink">Records selected : {by === 'group' ? pickedGroups.size : pickedItems.size}/{by === 'group' ? groups.length : all.length}</p>
            <button onClick={load} className={primaryButton}><Download size={14} />Load</button>
          </div>
        </div>
      ) : (
        <>
          <p className="text-sm text-red-700 dark:text-red-400">On click of Load, Items are loaded ready for price alteration. Bulk Rate updation can also be done for selected Items by entering the Update rate by Amount (given below).</p>
          <div className="flex justify-end"><button onClick={() => setRows(null)} className={secondaryButton}><ArrowLeft size={14} />Back</button></div>
          <div className={`${panel} max-h-[52vh] overflow-auto`}>
            <table className="w-full text-xs">
              <thead className={tableHead}>
                <tr>
                  <th className="px-3 py-2 text-left">Group Name</th><th className="px-3 py-2">Sel</th><th className="px-3 py-2 text-left">Item Code</th><th className="px-3 py-2 text-left">Item Name</th>
                  {cols.map(c => { const cap = caption(c); return [<th key={c} className="px-3 py-2 text-right">{cap}</th>, <th key={`${c}n`} className="px-3 py-2 text-right">{cap}-New</th>]; })}
                </tr>
              </thead>
              <tbody className={`${tableBody} tabular-nums`}>
                {rows.map(i => {
                  const on = ticked.has(i.code);
                  return (
                    <tr key={i.code} className={on ? '' : 'opacity-50'}>
                      <td className="px-3 py-1.5 font-sans">{i.group}</td>
                      <td className="px-3 py-1.5"><input type="checkbox" checked={on} onChange={() => toggleSet(ticked, setTicked, i.code)} className="accent-accent" aria-label={`Select ${i.name}`} /></td>
                      <td className="px-3 py-1.5">{i.code}</td>
                      <td className="px-3 py-1.5 font-sans text-stone-900 dark:text-stone-100">{i.name}</td>
                      {cols.map(c => [
                        <td key={c} className="px-3 py-1.5 text-right">{money(rateOf(i, c))}</td>,
                        <td key={`${c}n`} className="px-3 py-1 text-right">
                          {on ? <input type="number" min={0} step="0.01" aria-label={`New ${c.replace('_', ' ')} for ${i.name}`} value={edits[i.code]?.[c] ?? ''} placeholder={money(rateOf(i, c))}
                            onFocus={() => setLast({ code: i.code, col: c })} onChange={e => { setLast({ code: i.code, col: c }); setNew(i.code, c, e.target.value); }}
                            className={`${field} w-28 py-1 text-right font-bold tabular-nums`} /> : '—'}
                        </td>,
                      ])}
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>

          <div className="flex items-center justify-between">
            <p className="text-sm font-semibold text-accent-ink">{ticked.size} Item(s) Selected out of {rows.length}</p>
            <button onClick={exportGrid} className={secondaryButton} aria-label="Export to Excel"><FileSpreadsheet size={14} />Export</button>
          </div>
          <div className={`${panel} flex flex-wrap items-end gap-x-5 gap-y-3 px-4 py-3`}>
            <fieldset className="flex flex-col gap-1.5">
              <legend className={label}>Update Rate by</legend>
              <div className="flex flex-wrap gap-4">
                <div role="radiogroup" aria-label="Direction" className="flex gap-3">
                  <label className={radio}><input type="radio" name="pl-dir" checked={direction === 'inc'} onChange={() => setDirection('inc')} />Increase</label>
                  <label className={radio}><input type="radio" name="pl-dir" checked={direction === 'dec'} onChange={() => setDirection('dec')} />Decrease</label>
                </div>
                <div role="radiogroup" aria-label="By" className="flex gap-3">
                  <label className={radio}><input type="radio" name="pl-mode" checked={mode === 'per'} onChange={() => setMode('per')} />%age</label>
                  <label className={radio}><input type="radio" name="pl-mode" checked={mode === 'amt'} onChange={() => setMode('amt')} />Amount</label>
                </div>
              </div>
              <div className="flex items-center gap-2">
                <label htmlFor="pl-amt" className="text-sm text-stone-600">By {mode === 'per' ? '%age' : 'Amount'}</label>
                <input id="pl-amt" type="number" min={0} step="0.01" value={amount} onChange={e => setAmount(e.target.value)} className={`${field} w-28 text-right tabular-nums`} />
                <select id="pl-round" aria-label="Rounding" value={round} onChange={e => setRound(e.target.value)} disabled={mode !== 'per'} className={field}>
                  {ROUNDINGS.map(r => <option key={r.id} value={r.id}>{r.label}</option>)}
                </select>
              </div>
            </fieldset>
            <button onClick={apply} className={secondaryButton}><CheckCircle2 size={14} />Apply</button>
            <label className={radio}><input type="checkbox" checked={ticked.size === rows.length} onChange={e => setTicked(new Set(e.target.checked ? rows.map(i => i.code) : []))} />Select All</label>
            <button onClick={replaceRest} className={secondaryButton} title="Give the typed rate to the ticked rows below it"><Replace size={14} />Replace for rest of the list</button>
            <button onClick={() => void update()} className={`${primaryButton} ml-auto`}><Save size={14} />Update to DB</button>
          </div>
        </>
      )}
    </div>
  );
}
