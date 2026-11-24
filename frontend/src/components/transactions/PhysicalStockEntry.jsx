// Physical Stock Entry (legacy frmTrnPSE): a stock-take. Pick groups / packages / an item, Load the active items with
// their system stock, enter the counted stock (and a new purchase rate / MRP if needed). Save posts the excess as a
// Purchase Bill (supplier KSBCL) and the shortage as a Stock Deduction, or as a Sales Invoice with "Show Reduced Qty
// as Sales". Only changed rows are saved. Below the grid, the Summary Report totals each group and package.
// Keyboard: Enter moves to the next field (Shift+Enter back); ⌘/Ctrl+S saves.

import { useEffect, useMemo, useState } from 'react';
import { Download, Eraser, FileSpreadsheet, Save } from 'lucide-react';
import { downloadXls } from '../../lib/xlsExport';
import Toast from '../common/Toast';
import Kbd from '../common/Kbd';
import MultiSelectDropdown from '../common/MultiSelectDropdown';
import WithHistory from './DocHistoryPanel';
import { InlineError } from '../common/FormErrorSummary';
import { enterMovesNext, useShortcuts } from '../../lib/keys';
import { toastStore } from '../common/toastStore';
import { useConfirm } from '../common/ConfirmDialog';
import { errorText, getItems, getPhysicalStockEntry, getSalesCounters, getSalesPersons, loadPhysicalStock, newIdempotencyKey, savePhysicalStock } from '../../lib/api';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader } from '../../lib/styles';
import { groupLabel } from '../../lib/formatters';

const num = (v) => (v === '' || v == null ? 0 : Number(v));
const uniq = (list, key) => [...new Set(list.map(i => i[key]).filter(Boolean))].sort();

export default function PhysicalStockEntry() {
  const [catalogue, setCatalogue] = useState([]);
  const [groups, setGroups] = useState([]);
  const [packages, setPackages] = useState([]);
  const [itemText, setItemText] = useState('');
  const [rows, setRows] = useState([]);
  const [asSales, setAsSales] = useState(false);
  const [counters, setCounters] = useState([]);
  const [counter, setCounter] = useState('');
  const [salesPersons, setSalesPersons] = useState([]);
  const [salesPerson, setSalesPerson] = useState('');
  const [toast, setToast] = useState(null);
  const [problem, setProblem] = useState('');   // a form mistake, shown inline (never as a popup)
  const [key, setKey] = useState(newIdempotencyKey());
  const { confirm, dialog: confirmDialog } = useConfirm();

  useEffect(() => { if (toast) { const t = setTimeout(() => setToast(null), 3000); return () => clearTimeout(t); } }, [toast]);
  useEffect(() => {
    getItems().then(setCatalogue).catch(e => setToast({ kind: 'error', text: errorText(e) }));
    getSalesCounters().then(l => { const a = l.filter(c => !c.inactive); setCounters(a); setCounter(a[0]?.code ?? ''); }).catch(() => {});
    getSalesPersons().then(l => { setSalesPersons(l); setSalesPerson(l[0] ?? ''); }).catch(() => {});
  }, []);

  const groupOptions = useMemo(() => uniq(catalogue, 'group').sort((a, b) => groupLabel(a).localeCompare(groupLabel(b))), [catalogue]);
  const packageOptions = useMemo(() => uniq(catalogue, 'package'), [catalogue]);

  const load = () => {
    const text = itemText.trim().toLowerCase();
    const items = text ? catalogue.filter(i => i.code.toLowerCase().includes(text) || i.name.toLowerCase().includes(text)).map(i => i.code) : [];
    if (!groups.length && !packages.length && !items.length) return setProblem('Please Select Alteast One Item / ItemGroup / Package..');
    setProblem('');
    loadPhysicalStock({ groups: groups.join(','), packages: packages.join(','), items: items.join(',') })
      .then(list => { setRows(list.map(r => ({ ...r, physical: String(r.system_qty), pur: r.pur_rate == null ? null : String(r.pur_rate), mrpNew: String(r.mrp) }))); setKey(newIdempotencyKey()); })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const update = (i, patch) => setRows(rs => rs.map((r, x) => (x === i ? { ...r, ...patch } : r)));
  const diff = (r) => num(r.physical) - r.system_qty;
  const changed = (r) => diff(r) !== 0 || (r.pur !== null && num(r.pur) !== r.pur_rate) || num(r.mrpNew) !== r.mrp;
  const dirty = rows.filter(changed);

  // Summary Report: per group and package, counted vs system bottles, litres and cases + loose bottles.
  const summary = useMemo(() => {
    const m = new Map();
    for (const r of rows) {
      const k = `${r.group}\u0000${r.package}`;
      const s = m.get(k) || { group: r.group, package: r.package, phys: 0, sys: 0, physL: 0, sysL: 0, physCases: 0, sysCases: 0, physLoose: 0, sysLoose: 0 };
      s.phys += num(r.physical); s.sys += r.system_qty;
      s.physL += num(r.physical) * r.litres_per_bottle; s.sysL += r.system_qty * r.litres_per_bottle;
      if (r.bottles_per_case > 0) {   // frmTrnPSE.btnShowSumm: whole cases and the loose bottles left over, per item
        const pc = Math.floor(num(r.physical) / r.bottles_per_case), sc = Math.floor(r.system_qty / r.bottles_per_case);
        s.physCases += pc; s.sysCases += sc; s.physLoose += num(r.physical) - pc * r.bottles_per_case; s.sysLoose += r.system_qty - sc * r.bottles_per_case;
      }
      m.set(k, s);
    }
    return [...m.values()];
  }, [rows]);
  const casesText = (cases, loose) => [cases ? `${cases} ${cases > 1 ? 'Cases' : 'Case'}` : '', loose ? `${loose} ${loose > 1 ? 'Bottles' : 'Bottle'}` : ''].filter(Boolean).join(',');
  const exportGrid = async () => {
    if (!rows.length) return;
    if (!(await toastStore.defer('ok', `${rows.length} items will be exported to Excel when this closes.`, 'Export cancelled.'))) return;
    const cols = ['Group', 'Item name', 'Package', 'Pur Rate', 'MRP Rate', 'Physical Stock', 'System Stock', 'Diff'].map(l => ({ key: l, label: l }));
    downloadXls('Physical_Stock_Entry', 'Physical Stock Entry', '', cols, rows.map(r => ({ Group: groupLabel(r.group), 'Item name': r.item_name, Package: r.package, 'Pur Rate': r.pur ?? '', 'MRP Rate': r.mrpNew, 'Physical Stock': num(r.physical), 'System Stock': r.system_qty, Diff: diff(r) })));
  };
  const exportSummary = async () => {
    if (!summary.length) return;
    if (!(await toastStore.defer('ok', 'The summary will be exported to Excel when this closes.', 'Export cancelled.'))) return;
    const cols = ['Group', 'Package', 'Physical Stock (Bottles)', 'System Stock (Bottles)', 'Difference', 'Physical Stock (Ltrs)', 'System Stock (Ltrs)', 'Physical Stock Cases', 'System Stock Cases'].map(l => ({ key: l, label: l }));
    downloadXls('Physical_Stock_Summary', 'Physical Stock Summary Report', `as on ${new Date().toLocaleDateString('en-GB')}`, cols, summary.map(s => ({ Group: groupLabel(s.group), Package: s.package, 'Physical Stock (Bottles)': s.phys, 'System Stock (Bottles)': s.sys, Difference: s.phys - s.sys, 'Physical Stock (Ltrs)': Math.round(s.physL * 1000) / 1000, 'System Stock (Ltrs)': Math.round(s.sysL * 1000) / 1000, 'Physical Stock Cases': casesText(s.physCases, s.physLoose), 'System Stock Cases': casesText(s.sysCases, s.sysLoose) })));
  };

  const save = async () => {
    setProblem('');
    if (!dirty.length) return setProblem('There are no Changes to Update!');
    if (dirty.some(r => num(r.physical) < 0 || !Number.isInteger(num(r.physical)))) return setProblem('Physical stock must be a whole number, zero or more.');
    if (asSales && dirty.some(r => diff(r) < 0) && !counter) return setProblem('Choose a sales counter for the reduced quantity.');
    const body = {
      reduced_as_sales: asSales, counter: asSales ? counter : null, salesperson: asSales ? salesPerson || null : null,
      lines: dirty.map(r => ({ item_code: r.item_code, physical_qty: num(r.physical), pur_rate: r.pur === null ? null : num(r.pur), mrp: num(r.mrpNew) })),
    };
    const more = dirty.filter(r => diff(r) > 0).length, less = dirty.filter(r => diff(r) < 0).length;
    if (!(await confirm(`${dirty.length} item(s) changed: ${more} with excess (purchase bill), ${less} with shortage (${asSales ? 'sales invoice' : 'stock deduction'}). Save the physical stock entry?`))) return;
    if (!(await toastStore.defer('ok', 'Physical stock entry will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    savePhysicalStock(body, key).then(doc => {
      const made = [...new Set(doc.lines.flatMap(l => [l.pb_no, l.stkded_no]).filter(Boolean))];
      setToast({ kind: 'ok', text: `Physical stock entry ${doc.doc_no} saved${made.length ? ` (${made.join(', ')})` : ''}.` });
      setRows([]); setKey(newIdempotencyKey());
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  useShortcuts({ 'Mod+S': () => { void save(); } });

  return (
    <WithHistory kind="physical_stock" title="physical stock entries" onNew={() => { setRows([]); setProblem(''); setKey(newIdempotencyKey()); }} fetchDoc={getPhysicalStockEntry}>
    <div className="mx-auto flex max-w-7xl flex-col gap-4" onKeyDown={e => enterMovesNext(e)}>
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {confirmDialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Physical Stock Entry</h1>
          <p className={pageSub}>Count the stock, enter what is on the shelf; the difference is posted as a purchase bill or stock deduction.</p>
        </div>
        <button onClick={save} className={primaryButton}><Save size={14} />Save<Kbd combo="Mod+S" /></button>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div>
          <label htmlFor="ps-groups" className={label}>Selected Group</label>
          <MultiSelectDropdown id="ps-groups" value={groups} options={groupOptions} labelOf={groupLabel} onChange={v => { setGroups(v); setProblem(''); }} placeholder="Select groups" className="w-48" />
        </div>
        <div>
          <label htmlFor="ps-pkgs" className={label}>Selected Package</label>
          <MultiSelectDropdown id="ps-pkgs" value={packages} options={packageOptions} onChange={v => { setPackages(v); setProblem(''); }} placeholder="Select packages" className="w-44" />
        </div>
        <div>
          <label htmlFor="ps-item" className={label}>Selected Item (code or name contains)</label>
          <input id="ps-item" value={itemText} onChange={e => { setItemText(e.target.value); setProblem(''); }} className={`${field} w-56`} />
        </div>
        <button type="button" onClick={load} className={secondaryButton}><Download size={14} />Load</button>
        <button type="button" onClick={() => setRows(rs => rs.map(r => ({ ...r, physical: '0' })))} disabled={!rows.length} className={secondaryButton}><Eraser size={14} />Clear Physical Stock</button>
        <button type="button" onClick={() => void exportGrid()} disabled={!rows.length} className={secondaryButton}><FileSpreadsheet size={14} />Export to Excel</button>
        <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={asSales} onChange={e => { setAsSales(e.target.checked); setProblem(''); }} />Show Reduced Qty as Sales</label>
        {asSales && (<>
          <select value={counter} onChange={e => setCounter(e.target.value)} className={field} aria-label="Sales counter">
            {counters.map(c => <option key={c.code} value={c.code}>{c.name || c.code}</option>)}
          </select>
          <select value={salesPerson} onChange={e => setSalesPerson(e.target.value)} className={field} aria-label="Sales person">
            <option value="">— sales person —</option>
            {salesPersons.map(p => <option key={p} value={p}>{p}</option>)}
          </select>
        </>)}
      </div>

      {problem && <InlineError message={problem} />}

      <div className={`${panel} max-h-[55vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr>
              <th className="px-3 py-2">Group</th><th className="px-3 py-2">Item name</th><th className="px-3 py-2">Package</th>
              <th className="px-3 py-2 text-right">Pur Rate</th><th className="px-3 py-2 text-right">MRP Rate</th>
              <th className="px-3 py-2 text-right">Physical Stock</th><th className="px-3 py-2 text-right">System Stock</th><th className="px-3 py-2 text-right">Diff</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={8} className="px-4 py-14 text-center text-stone-500">Choose a group, package or item and press Load.</td></tr>}
            {rows.map((r, i) => (
              <tr key={r.item_code} className={changed(r) ? 'bg-amber-50/60 dark:bg-amber-500/5' : ''}>
                <td className="px-3 py-1.5">{groupLabel(r.group)}</td><td className="px-3 py-1.5">{r.item_name}</td><td className="px-3 py-1.5">{r.package}</td>
                <td className="px-3 py-1.5 text-right">{r.pur === null ? '—' : <input type="number" min={0} step="0.01" value={r.pur} onChange={e => update(i, { pur: e.target.value })} className={`${field} w-24 text-right tabular-nums`} aria-label={`Purchase rate ${r.item_name}`} />}</td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={r.mrpNew} onChange={e => update(i, { mrpNew: e.target.value })} className={`${field} w-24 text-right tabular-nums`} aria-label={`MRP ${r.item_name}`} /></td>
                <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="1" value={r.physical} onChange={e => update(i, { physical: e.target.value })} className={`${field} w-24 text-right tabular-nums`} aria-label={`Physical stock ${r.item_name}`} /></td>
                <td className="px-3 py-1.5 text-right tabular-nums">{r.system_qty}</td>
                <td className={`px-3 py-1.5 text-right font-semibold tabular-nums ${diff(r) < 0 ? 'text-red-600' : diff(r) > 0 ? 'text-emerald-600' : ''}`}>{diff(r)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {summary.length > 0 && (
        <div className={`${panel} overflow-auto`}>
          <div className="flex items-center justify-between border-b border-line px-4 py-2 text-xs font-bold">Summary Report
            <button type="button" onClick={() => void exportSummary()} className={secondaryButton}><FileSpreadsheet size={14} />Export to Excel</button></div>
          <table className="w-full text-xs">
            <thead className={tableHead}>
              <tr>
                <th className="px-3 py-2">Group</th><th className="px-3 py-2">Package</th>
                <th className="px-3 py-2 text-right">Physical (Bottles)</th><th className="px-3 py-2 text-right">System (Bottles)</th><th className="px-3 py-2 text-right">Difference</th>
                <th className="px-3 py-2 text-right">Physical (Ltrs)</th><th className="px-3 py-2 text-right">System (Ltrs)</th>
                <th className="px-3 py-2 text-right">Physical Stock Cases</th><th className="px-3 py-2 text-right">System Stock Cases</th>
              </tr>
            </thead>
            <tbody className={tableBody}>
              {summary.map(s => (
                <tr key={`${s.group}-${s.package}`}>
                  <td className="px-3 py-1.5">{groupLabel(s.group)}</td><td className="px-3 py-1.5">{s.package}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{s.phys}</td><td className="px-3 py-1.5 text-right tabular-nums">{s.sys}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{s.phys - s.sys}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{money(s.physL)}</td><td className="px-3 py-1.5 text-right tabular-nums">{money(s.sysL)}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{casesText(s.physCases, s.physLoose)}</td><td className="px-3 py-1.5 text-right tabular-nums">{casesText(s.sysCases, s.sysLoose)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
    </WithHistory>
  );
}
