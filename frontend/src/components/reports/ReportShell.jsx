// Shared frame for the table-driven reports: filter row → Run → table with totals, Print and Export Excel.
// Filters map to query params: on → `on`, range → `from_date`/`to_date`, item → `item_code`, months → `months`.

import Pager from '../common/Pager';
import { usePage } from '../common/usePage';
import { useEffect, useState } from 'react';
import { FileDown, Play } from 'lucide-react';
import { errorText, getBrands, getItemGroups, getItems, getReport, todayIso, } from '../../lib/api';
import { downloadXls } from '../../lib/xlsExport';
import { badge, field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow, exportButton } from '../../lib/styles';
import { groupLabel } from '../../lib/formatters';
import { SkeletonTableRows } from '../common/Skeleton';
import MultiSelectDropdown from './MultiSelectDropdown';
import DateInput from '../common/DateInput';

 



const monthStart = () => `${todayIso().slice(0, 8)}01`;

// Shared frame for the API reports: filter row → Run → table with totals, Print and Export Excel.
export default function ReportShell({ title, subtitle, endpoint, filters, columns }

) {
  const [on, setOn] = useState(todayIso());
  const [from, setFrom] = useState(monthStart());
  const [to, setTo] = useState(todayIso());
  const [item, setItem] = useState('');
  const [months, setMonths] = useState('6');
  const [level, setLevel] = useState('rol');
  const [items, setItems] = useState([]);
  const [kind, setKind] = useState('pur');
  const [withValue, setWithValue] = useState(false);
  const [brandList, setBrandList] = useState([]);
  const [groupList, setGroupList] = useState([]);
  const [brandSel, setBrandSel] = useState([]);
  const [groupSel, setGroupSel] = useState([]);
  const [itemSel, setItemSel] = useState([]);
  const [pkgSel, setPkgSel] = useState([]);
  const [litres, setLitres] = useState(false);
  const [ageMode, setAgeMode] = useState('months');
  const [ageRanges, setAgeRanges] = useState('0-30,31-60,61-90,91-180,180-');
  const [valuation, setValuation] = useState('standard');
  const [dyn, setDyn] = useState(null);
  const [rows, setRows] = useState(null);
  const [error, setError] = useState('');
  const [running, setRunning] = useState(false);

  useEffect(() => {
    if (filters.includes('item')) {
      getItems().then(list => { setItems(list); setItem(i => i || list[0]?.code || ''); }).catch(() => undefined);
    }
    if (filters.includes('ageing')) {
      getItemGroups().then(setGroupList).catch(() => undefined);
      getItems().then(setItems).catch(() => undefined);
    }
    if (filters.includes('stocklevel')) {
      getItemGroups().then(setGroupList).catch(() => undefined);
      getItems().then(setItems).catch(() => undefined);
    }
    if (filters.includes('brandwise')) {
      getBrands().then(setBrandList).catch(() => undefined);
      getItemGroups().then(setGroupList).catch(() => undefined);
    }
  }, [filters]);

  const run = () => {
    const params = {};
    if (filters.includes('on')) params[endpoint.includes('ageing') ? 'as_on' : 'on'] = on;
    if (filters.includes('range')) { params.from_date = from; params.to_date = to; }
    if (filters.includes('item')) params.item_code = item;
    if (filters.includes('months') && ageMode === 'months') params.months = months;
    if (filters.includes('ageing') && ageMode !== 'months') { params.view = ageMode; params.ranges = ageRanges; params.groups = groupSel.join(','); params.packages = pkgSel.join(','); params.valuation = valuation; }
    if (filters.includes('level')) params.level = level;
    if (filters.includes('stocklevel')) { params.groups = groupSel.join(','); params.items = itemSel.join(','); params.packages = pkgSel.join(','); }
    if (filters.includes('brandwise')) { params.kind = kind; params.brands = brandSel.join(','); params.groups = groupSel.join(','); }
    setRunning(true);
    const ep = filters.includes('ageing') && ageMode !== 'months' ? `${endpoint}/desktop` : endpoint;
    getReport(ep, params)
      .then(r => { if (Array.isArray(r)) { setDyn(null); setRows(r); } else { setDyn(r.columns); setRows(r.rows); } setError(''); })
      .catch(e => { setRows(null); setError(errorText(e)); })
      .finally(() => setRunning(false));
  };

  const format = (c, v) =>
    v === undefined || v === '' ? '—' : c.kind === 'money' ? money(Number(v)) : c.kind === 'number' ? String(Number(Number(v).toFixed(3))) : c.key === 'group' ? groupLabel(v) : String(v);

  const pager = usePage(rows ?? []);

  const exportCsv = () => {
    if (!rows) return;
    downloadXls(title.replace(/\s+/g, '_'), title, '', shown.map(c => ({ key: c.key, label: c.label })), rows);
  };

  // Brand Wise (frmRptBrandWise): the exe's "With Value" and Purchase / Sale / Purchase Vs Sale choices decide which columns show.
  const shown = (dyn || columns).filter(c => {
    if (filters.includes('stocklevel')) return !(c.ltr !== undefined && c.ltr !== litres);
    if (!filters.includes('brandwise')) return true;
    if (c.side === 'pur' && kind === 'sales') return false;
    if (c.side === 'sal' && kind === 'pur') return false;
    if (c.valued && !withValue) return false;
    if (c.cases && kind === 'both') return false;
    return true;
  });
  const totals = shown.map(c => (c.total && rows && !filters.includes('brandwise') ? rows.reduce((acc, r) => acc + (Number(r[c.key]) || 0), 0) : null));
  const num = (c) => c.kind && c.kind !== 'text';

  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>{title}</h1>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex gap-2 print:hidden">
          <button onClick={exportCsv} disabled={!rows} className={exportButton}><FileDown size={14} />Export Excel</button>
        </div>
      </div>

      <form
        onSubmit={e => { e.preventDefault(); run(); }}
        aria-label="Report filters"
        className={`${panel} mb-4 flex flex-wrap items-end gap-3 p-4 print:hidden`}
      >
        {filters.includes('item') && (
          <div className="w-full min-w-0 sm:w-72">
            <label htmlFor="rp-item" className={label}>Item</label>
            <select id="rp-item" value={item} onChange={e => setItem(e.target.value)} className={field}>
              {items.map(i => <option key={i.code} value={i.code}>{i.code} · {i.name}</option>)}
            </select>
          </div>
        )}
        {filters.includes('on') && (
          <div className="w-40"><label htmlFor="rp-on" className={label}>{endpoint.includes('ageing') ? 'As-on date' : 'Date'}</label><DateInput id="rp-on" value={on} onChange={e => setOn(e.target.value)} className={`${field} tabular-nums`} /></div>
        )}
        {filters.includes('level') && (
          <div className="w-56"><label htmlFor="rp-level" className={label}>Show</label>
            <select id="rp-level" value={level} onChange={e => setLevel(e.target.value)} className={field}>
              <option value="min">Below Min Level</option>
              <option value="rol">Below Re-Order Level</option>
              <option value="max">Above Max Level</option>
            </select>
          </div>
        )}
        {filters.includes('stocklevel') && (<>
          <div className="w-44"><label htmlFor="rp-sl-groups" className={label}>Selected Group</label>
            <MultiSelectDropdown id="rp-sl-groups" allLabel="All groups" options={groupList.map(g => ({ value: g.code, label: groupLabel(g.name || g.code) }))} value={groupSel} onChange={setGroupSel} />
          </div>
          <div className="w-44"><label htmlFor="rp-sl-items" className={label}>Select Item</label>
            <MultiSelectDropdown id="rp-sl-items" allLabel="All items" options={items.map(i => ({ value: i.code, label: i.name || i.code }))} value={itemSel} onChange={setItemSel} />
          </div>
          <div className="w-44"><label htmlFor="rp-sl-pkgs" className={label}>Select Package</label>
            <MultiSelectDropdown id="rp-sl-pkgs" allLabel="All packages" options={[...new Set(items.map(i => i.package).filter(Boolean))].sort().map(p => ({ value: p, label: p }))} value={pkgSel} onChange={setPkgSel} />
          </div>
          <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={litres} onChange={e => setLitres(e.target.checked)} />Show qty in Litres</label>
        </>)}
        {filters.includes('range') && (<>
          <div className="w-40"><label htmlFor="rp-from" className={label}>From</label><DateInput id="rp-from" value={from} onChange={e => setFrom(e.target.value)} className={`${field} tabular-nums`} /></div>
          <div className="w-40"><label htmlFor="rp-to" className={label}>To</label><DateInput id="rp-to" value={to} onChange={e => setTo(e.target.value)} className={`${field} tabular-nums`} /></div>
        </>)}
        {filters.includes('brandwise') && (<>
          <div className="w-48"><label htmlFor="rp-kind" className={label}>Show</label>
            <select id="rp-kind" value={kind} onChange={e => setKind(e.target.value)} className={field}>
              <option value="pur">Purchase</option><option value="sales">Sale</option><option value="both">Purchase Vs Sale</option>
            </select>
          </div>
          <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={withValue} onChange={e => setWithValue(e.target.checked)} />With Value</label>
          <div className="w-48"><label htmlFor="rp-brands" className={label}>Select Brand</label>
            <MultiSelectDropdown id="rp-brands" allLabel="All brands" options={brandList.map(b => ({ value: b.code, label: b.name || b.code }))} value={brandSel} onChange={setBrandSel} />
          </div>
          <div className="w-48"><label htmlFor="rp-groups" className={label}>Select ItemGroup</label>
            <MultiSelectDropdown id="rp-groups" allLabel="All groups" options={groupList.map(g => ({ value: g.code, label: groupLabel(g.name || g.code) }))} value={groupSel} onChange={setGroupSel} />
          </div>
        </>)}
        {filters.includes('ageing') && (<>
          <div className="w-56"><label htmlFor="rp-agemode" className={label}>Ageing</label>
            <select id="rp-agemode" value={ageMode} onChange={e => setAgeMode(e.target.value)} className={field}>
              <option value="months">By months since last receipt</option><option value="ranges">By age ranges (days)</option><option value="pb">Purchase Bill wise</option>
            </select>
          </div>
          {ageMode !== 'months' && (<>
            {ageMode === 'ranges' && <div className="w-64"><label htmlFor="rp-ranges" className={label}>Age ranges (From-To days, last open)</label>
              <input id="rp-ranges" value={ageRanges} onChange={e => setAgeRanges(e.target.value)} className={`${field} tabular-nums`} /></div>}
            <div className="w-44"><label htmlFor="rp-ag-groups" className={label}>Selected Group</label>
              <MultiSelectDropdown id="rp-ag-groups" allLabel="All groups" options={groupList.map(g => ({ value: g.code, label: groupLabel(g.name || g.code) }))} value={groupSel} onChange={setGroupSel} /></div>
            <div className="w-44"><label htmlFor="rp-ag-pkgs" className={label}>Select Package</label>
              <MultiSelectDropdown id="rp-ag-pkgs" allLabel="All packages" options={[...new Set(items.map(i => i.package).filter(Boolean))].sort().map(p => ({ value: p, label: p }))} value={pkgSel} onChange={setPkgSel} /></div>
            {ageMode === 'ranges' && <div className="w-44"><label htmlFor="rp-valuation" className={label}>Valuation</label>
              <select id="rp-valuation" value={valuation} onChange={e => setValuation(e.target.value)} className={field}>
                <option value="standard">Standard Rate</option><option value="fifo">FIFO Rate</option><option value="wtavg">Wt.Avg. Rate</option><option value="lastpur">Last Purchase Rate</option>
              </select></div>}
          </>)}
        </>)}
        {filters.includes('months') && ageMode === 'months' && (
          <div className="w-24"><label htmlFor="rp-months" className={label}>Months</label><input id="rp-months" type="number" min={1} max={24} value={months} onChange={e => setMonths(e.target.value)} className={`${field} text-right tabular-nums`} /></div>
        )}
        <button type="submit" className={primaryButton}><Play size={14} />Run report</button>
        {rows && <span className={`${badge('neutral')} ml-auto`}>{rows.length} rows</span>}
      </form>

      <div className={`${panel} max-h-[640px] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>{shown.map(c => <th key={c.key} className={`px-4 py-2.5 ${num(c) ? 'text-right' : ''}`}>{c.label}</th>)}</tr>
          </thead>
          <tbody className={tableBody}>
            {error && <tr><td colSpan={shown.length} className="px-4 py-10 text-center text-red-600 dark:text-red-400">{error}</td></tr>}
            {running && <SkeletonTableRows rows={8} cols={shown.length} />}
            {!running && !rows && !error && <tr><td colSpan={shown.length} className="px-4 py-14 text-center text-stone-500">Choose the filters above and run the report.</td></tr>}
            {!running && rows && !rows.length && <tr><td colSpan={shown.length} className="px-4 py-14 text-center text-stone-500">No data for these filters.</td></tr>}
            {!running && rows && pager.slice.map((r, i) => (
              <tr key={pager.from + i} className={tableRow}>
                {shown.map((c, ci) => (
                  <td key={c.key} className={`${r.subtotal ? 'font-semibold ' : ''}px-4 py-2 ${num(c) ? 'text-right tabular-nums text-stone-700 dark:text-stone-200' : ci === 0 ? 'font-medium text-stone-900 dark:text-stone-50' : 'text-stone-600 dark:text-stone-300'}`}>{format(c, r[c.key])}</td>
                ))}
              </tr>
            ))}
          </tbody>
          {rows && rows.length > 0 && totals.some(t => t !== null) && (
            <tfoot className="sticky bottom-0 border-t border-line-strong bg-subtle font-semibold text-stone-900 dark:text-stone-50">
              <tr>
                {shown.map((c, i) => (
                  <td key={c.key} className={`px-4 py-2.5 ${num(c) ? 'text-right tabular-nums' : ''}`}>
                    {i === 0 ? 'Total' : totals[i] === null ? '' : c.kind === 'money' ? money(totals[i]) : Number(totals[i].toFixed(3))}
                  </td>
                ))}
              </tr>
            </tfoot>
          )}
        </table>
        <Pager p={pager} />
      </div>
    </div>
  );
}
