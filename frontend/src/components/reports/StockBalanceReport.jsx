// Stock Balance report: opening, receipts, issues, closing and value per item for a date
// (GET /reports/stock-balance). Printable in the stock register formats.

import { useCallback, useEffect, useState } from 'react';
import { FileDown, Play, Printer } from 'lucide-react';
import { errorText, getItemGroups, getItems, getStockBalance, getStockBalanceFormats, getStockBalanceView, renderStockBalancePrint, todayIso } from '../../lib/api';
import { downloadXls } from '../../lib/xlsExport';
import { field, label, primaryButton, exportButton } from '../../lib/styles';
import MultiSelectDropdown from './MultiSelectDropdown';
import { badge, money, pageHeader, pageSub, pageTitle, panel, tableBody, tableHead, tableRow, printButton } from '../../lib/styles';
import ReportPrintModal from '../print/ReportPrintModal';
import { SkeletonTableRows } from '../common/Skeleton';



export default function StockBalanceReport() {
  const [rows, setRows] = useState(null);
  const [error, setError] = useState('');
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [formats, setFormats] = useState([]);
  const day = todayIso();
  // The exe's view options (frmRptStockBal): which view, the period, filters and the valuation.
  const [view, setView] = useState('closing');
  const [from, setFrom] = useState(`${day.slice(0, 8)}01`);
  const [to, setTo] = useState(day);
  const [groupList, setGroupList] = useState([]);
  const [itemList, setItemList] = useState([]);
  const [groupSel, setGroupSel] = useState([]);
  const [itemSel, setItemSel] = useState([]);
  const [pkgSel, setPkgSel] = useState([]);
  const [opt, setOpt] = useState({ litres: false, values: false, zero_hide: false, group_total: false, with_tax: false, act_sales: false });
  const [rate, setRate] = useState('pur');
  const [result, setResult] = useState(null);
  const [viewError, setViewError] = useState('');
  const [running, setRunning] = useState(false);
  const flip = k => e => setOpt(o => ({ ...o, [k]: e.target.checked }));
  const runView = () => {
    setRunning(true);
    getStockBalanceView({ from_date: from, to_date: to, view, groups: groupSel.join(','), items: itemSel.join(','), packages: pkgSel.join(','), ...opt,
      values: view === 'package' ? false : opt.values, rate })
      .then(r => { setResult(r); setViewError(''); })
      .catch(e => { setResult(null); setViewError(errorText(e)); })
      .finally(() => setRunning(false));
  };
  const shownCols = result ? result.columns.filter(c => !c.brand || result.brand_required) : [];
  const cell = (c, v) => (v === undefined || v === null || v === '' ? '' : c.kind === 'money' ? money(Number(v)) : c.kind === 'number' ? Number(Number(v).toFixed(3)) : String(v));
  // Stable between renders, so the print window's preview is asked for once per format.
  const renderOn = useCallback((rpt, printer) => renderStockBalancePrint(day, day, '', rpt, printer), [day]);

  useEffect(() => {
    getItemGroups().then(setGroupList).catch(() => undefined);
    getItems().then(setItemList).catch(() => undefined);
  }, []);

  useEffect(() => {
    getStockBalanceFormats().then(f => setFormats(f.formats)).catch(() => { /* printing says why */ });
  }, []);

  useEffect(() => {
    const date = todayIso();
    Promise.all([getItems(), getStockBalance(date)])
      .then(([items, balance]) => {
        const byCode = new Map(items.map(item => [item.code, item]));
        setRows(balance.map(row => ({
          group: row.group, brand: byCode.get(row.item_code)?.brand ?? '', name: row.item_name, pkg: row.package,
          opQty: row.op_qty, recQty: row.rec_qty, issQty: row.iss_qty, clQty: row.cl_qty,
          rate: byCode.get(row.item_code)?.mrp ?? 0, value: row.cl_value,
        })));
      })
      .catch(error => setError(errorText(error)));
  }, []);

  const th = 'px-4 py-2.5';
  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>Stock balance</h1>
          <p className={pageSub}>Today's excise opening, receipt, issue and closing balances.</p>
        </div>
        <div className="flex items-center gap-3">
          {rows && <span className={badge('neutral')}>{rows.length} items</span>}
          <button
            onClick={() => setShowPrintModal(true)}
            disabled={!rows || !rows.length}
            className={printButton}
          >
            <Printer size={14} />Print Stock Sheet
          </button>
        </div>
      </div>
      <form onSubmit={e => { e.preventDefault(); runView(); }} aria-label="Stock balance options" className={`${panel} mb-4 flex flex-wrap items-end gap-3 p-4`}>
        <div className="w-40"><label htmlFor="sb-from" className={label}>From</label><input id="sb-from" type="date" value={from} onChange={e => setFrom(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div className="w-40"><label htmlFor="sb-to" className={label}>To</label><input id="sb-to" type="date" value={to} onChange={e => setTo(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div className="w-56"><label htmlFor="sb-view" className={label}>View</label>
          <select id="sb-view" value={view} onChange={e => setView(e.target.value)} className={field}>
            <option value="closing">Closing Balance Only</option><option value="detailed">Closing Balance With Detailed</option>
            <option value="package">Package wise Balance</option><option value="group">Group wise Summary Balance</option>
          </select>
        </div>
        {view !== 'group' && (<>
          <div className="w-44"><label htmlFor="sb-groups" className={label}>Selected Group</label>
            <MultiSelectDropdown id="sb-groups" allLabel="All groups" options={groupList.map(g => ({ value: g.code, label: g.name || g.code }))} value={groupSel} onChange={setGroupSel} /></div>
          <div className="w-44"><label htmlFor="sb-items" className={label}>Select Item</label>
            <MultiSelectDropdown id="sb-items" allLabel="All items" options={itemList.map(i => ({ value: i.code, label: i.name || i.code }))} value={itemSel} onChange={setItemSel} /></div>
          <div className="w-44"><label htmlFor="sb-pkgs" className={label}>Select Package</label>
            <MultiSelectDropdown id="sb-pkgs" allLabel="All packages" options={[...new Set(itemList.map(i => i.package).filter(Boolean))].sort().map(p => ({ value: p, label: p }))} value={pkgSel} onChange={setPkgSel} /></div>
        </>)}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.litres} onChange={flip('litres')} />Show qty in Litres</label>}
        {(view === 'closing' || view === 'detailed') && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.values} onChange={flip('values')} />Show Value</label>}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.zero_hide} onChange={flip('zero_hide')} />Do Not Show Items With Zero Balance</label>}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.group_total} onChange={flip('group_total')} />Group Wise Total Required</label>}
        {(view === 'group' || ((view === 'closing' || view === 'detailed') && opt.values)) && (
          <div className="w-48"><label htmlFor="sb-rate" className={label}>Value at</label>
            <select id="sb-rate" value={rate} onChange={e => setRate(e.target.value)} className={field}>
              <option value="pur">Last Purchase Rate</option><option value="mrp">Sales Rate (MRP)</option>
              {(view === 'detailed' || view === 'group') && <option value="fifo">FIFO</option>}
              {view === 'detailed' && <option value="war">Wt.Avg Rate</option>}
            </select>
          </div>)}
        {rate === 'pur' && opt.values && (view === 'closing' || view === 'detailed') && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.with_tax} onChange={flip('with_tax')} />With Tax</label>}
        {view === 'detailed' && opt.values && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.act_sales} onChange={flip('act_sales')} />Actual Sales Value</label>}
        <button type="submit" className={primaryButton} disabled={running}><Play size={14} />View</button>
        <button type="button" className={exportButton} disabled={!result} onClick={() => result && downloadXls('Stock_Balance', 'Stock Balance Report', `${from} to ${to}`, shownCols.map(c => ({ key: c.key, label: c.label })), result.rows)}><FileDown size={14} />Export Excel</button>
      </form>
      {(result || viewError || running) && (
        <div className={`${panel} mb-6 max-h-[560px] overflow-auto`}>
          <table className="w-full text-xs">
            <thead className={tableHead}><tr>{shownCols.map(c => <th key={c.key} className={`px-3 py-2 ${c.kind ? 'text-right' : 'text-left'}`}>{c.label}</th>)}</tr></thead>
            <tbody className={tableBody}>
              {viewError && <tr><td className="px-4 py-8 text-center text-red-600 dark:text-red-400">{viewError}</td></tr>}
              {running && <SkeletonTableRows rows={8} cols={Math.max(shownCols.length, 6)} />}
              {!running && result && !result.rows.length && <tr><td colSpan={shownCols.length} className="px-4 py-8 text-center text-stone-500">No data for these options.</td></tr>}
              {!running && result && result.rows.map((r, i) => (
                <tr key={i} className={`${tableRow} ${r.subtotal ? 'bg-subtle font-semibold' : ''}`}>
                  {shownCols.map(c => <td key={c.key} className={`px-3 py-1.5 ${c.kind ? 'text-right tabular-nums' : ''}`}>{cell(c, r[c.key])}</td>)}
                </tr>
              ))}
            </tbody>
          </table>
          {result && <div className="px-3 py-2 text-xs text-stone-500">Total Records: {result.rows.filter(r => !r.subtotal).length}</div>}
        </div>
      )}
      <div className={`${panel} overflow-x-auto`}>
        <table className="w-full text-left text-xs">
          <thead className={tableHead}>
            <tr>
              <th className={th}>Item</th><th className={th}>Group</th>
              <th className={`${th} text-center`}>Pkg</th>
              <th className={`${th} text-right`}>Opening</th><th className={`${th} text-right`}>Receipt</th>
              <th className={`${th} text-right`}>Issued</th><th className={`${th} text-right`}>Closing</th>
              <th className={`${th} text-right`}>Rate (₹)</th><th className={`${th} text-right`}>Closing value (₹)</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {error && <tr><td colSpan={9} className="px-4 py-8 text-center text-[#9A4B45] dark:text-red-400">{error}</td></tr>}
            {!error && !rows && <SkeletonTableRows rows={8} cols={9} />}
            {rows && !rows.length && <tr><td colSpan={9} className="px-4 py-8 text-center text-[#70675F]">No stock on record.</td></tr>}
            {rows?.map((row, index) => (
              <tr key={index} className={tableRow}>
                <td className="px-4 py-2.5">
                  <div className="font-medium text-[#29231F] dark:text-stone-50">{row.name}</div>
                  {row.brand && <div className="text-[11px] text-[#958B81] dark:text-stone-400">{row.brand}</div>}
                </td>
                <td className="px-4 py-2.5 text-[#70675F] dark:text-stone-300">{row.group}</td>
                <td className="px-4 py-2.5 text-center text-[#958B81]">{row.pkg}</td>
                <td className="px-4 py-2.5 text-right text-[#70675F] dark:text-stone-300">{row.opQty}</td>
                <td className="px-4 py-2.5 text-right text-[#55745A] dark:text-emerald-400">+{row.recQty}</td>
                <td className="px-4 py-2.5 text-right text-[#9A4B45] dark:text-red-400">−{row.issQty}</td>
                <td className="px-4 py-2.5 text-right font-semibold text-[#29231F] dark:text-stone-50">{row.clQty}</td>
                <td className="px-4 py-2.5 text-right text-[#70675F] dark:text-stone-300">{money(row.rate)}</td>
                <td className="px-4 py-2.5 text-right font-semibold text-[#29231F] dark:text-stone-50">{money(row.value)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showPrintModal && rows && (
        <ReportPrintModal
          title={`Stock Balance (${day})`}
          formats={formats}
          render={renderOn}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
}