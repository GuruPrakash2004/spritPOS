// Utilities → Compare Stock (legacy frmUtlCompareStk, shortcut 714).
//
// Two stock sheets side by side, "Old" and "New": each is a file (Import from Excel: CSV, or the Excel XML
// that "Export to Excel" writes) or, added in the web app, the web app's current stock. The columns (Item
// Code, Item Name, Closing Qty, Value) are found from the headers and can be changed. Compare Stocks lists each
// item with both sides' Closing Qty and Value, Qty Diff and Value Diff, and the desktop statuses Item Not found,
// Qty Mismatch and Value Mismatch; matching items are hidden unless "Show matching items" is ticked. Export to
// Excel. Nothing is saved: this screen only reads.

import { useState, } from 'react';
import { FileSpreadsheet, GitCompare, Upload, Warehouse } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getCurrentStock } from '../../lib/api';
import { downloadXls } from '../../lib/xlsExport';
import { compareSheets, guessColumns, readSheet, sheetItems, } from '../../lib/stockSheet';
import { badge, exportButton, field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead } from '../../lib/styles';


const COLS = [['code', 'Item Code'], ['name', 'Item Name'], ['qty', 'Closing Qty'], ['value', 'Value']];
const qty = (n) => (n === null ? '—' : n.toLocaleString('en-IN', { maximumFractionDigits: 3 }));
const amt = (n) => (n === null ? '—' : n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
const TONE = {
  OK: 'success', 'Item Not found (Old)': 'danger', 'Item Not found (New)': 'danger', 'Qty Mismatch': 'danger', 'Value Mismatch': 'warning',
};

function SidePanel({ title, side, setSide, error, setError }) {
  const id = title.toLowerCase();
  const onFile = (e) => {
    const file = e.target.files?.[0];
    e.target.value = '';
    if (!file) return;
    file.text()
      .then(text => { const sheet = readSheet(file.name, text); setError(''); setSide({ sheet, cols: guessColumns(sheet), source: 'file' }); })
      .catch(err => setError(err instanceof Error ? err.message : 'Cannot Import From Excel.'));
  };
  const useWeb = async () => {
    if (!(await toastStore.defer('refresh', "The web app's current stock will be loaded when this closes.", 'Cancelled.'))) return;
    getCurrentStock().then(res => {
      const sheet = {
        name: `Web app stock now (value at ${res.basis})`,
        rows: [['Item Code', 'Item Name', 'Closing Qty', 'Value'], ...res.rows.map(r => [r.code, r.name, String(r.qty), String(r.value)])],
      };
      setError('');
      setSide({ sheet, cols: { header: 0, code: 0, name: 1, qty: 2, value: 3 }, source: 'web' });
    }).catch(err => toastStore.add('error', errorText(err)));
  };
  const header = side ? side.sheet.rows[side.cols.header] ?? [] : [];
  return (
    <section className={`${panel} flex-1 p-4`} aria-label={`${title} stock`}>
      <h2 className={sectionTitle}>{title}</h2>
      <p className="mb-3 truncate text-[11px] text-stone-500">{side ? side.sheet.name : 'Pick a stock sheet'}</p>
      <div className="flex flex-wrap gap-2">
        <label className={`${secondaryButton} cursor-pointer`}>
          <Upload size={14} />Import from Excel ({title})
          <input type="file" accept=".csv,.xls,.xml,.txt" className="sr-only" aria-label={`Import ${title} stock sheet`} onChange={onFile} />
        </label>
        <button className={secondaryButton} onClick={useWeb}><Warehouse size={14} />Web app stock now</button>
      </div>
      <InlineError message={error} />
      {side?.source === 'file' && (
        <div className="mt-3 grid grid-cols-2 gap-2 sm:grid-cols-4">
          {COLS.map(([k, name]) => (
            <div key={k}>
              <label htmlFor={`cs-${id}-${k}`} className={label}>{name}</label>
              <select id={`cs-${id}-${k}`} value={side.cols[k]} className={field}
                onChange={e => setSide({ ...side, cols: { ...side.cols, [k]: Number(e.target.value) } })}>
                <option value={-1}>(none)</option>
                {header.map((h, i) => <option key={i} value={i}>{h || `Column ${i + 1}`}</option>)}
              </select>
            </div>
          ))}
        </div>
      )}
    </section>
  );
}

export default function CompareStock() {
  const [older, setOlder] = useState(null);
  const [newer, setNewer] = useState(null);
  const [rows, setRows] = useState(null);
  const [showOk, setShowOk] = useState(false);
  const [errors, setErrors] = useState({ old: '', new: '', result: '' });

  const compare = async () => {
    const colsMissing = (s) => !!s && (s.cols.qty < 0 || (s.cols.code < 0 && s.cols.name < 0));
    const next = {
      old: !older ? 'Pick the Old stock sheet' : colsMissing(older) ? 'Choose its Closing Qty column and an Item Code or Item Name column' : '',
      new: !newer ? 'Pick the New stock sheet' : colsMissing(newer) ? 'Choose its Closing Qty column and an Item Code or Item Name column' : '',
      result: '',
    };
    setErrors(next);
    if (next.old || next.new || !older || !newer) return;
    if (!(await toastStore.defer('refresh', 'The two stock sheets will be compared when this closes.', 'Compare cancelled.'))) return;
    const result = compareSheets(sheetItems(older.sheet, older.cols), sheetItems(newer.sheet, newer.cols));
    setRows(result);
    const off = result.filter(r => r.status !== 'OK').length;
    toastStore.add(off ? 'error' : 'ok', off ? `${off} item(s) differ.` : 'Both stock sheets match.');
  };

  const shown = (rows ?? []).filter(r => showOk || r.status !== 'OK');

  const exportXls = async () => {
    if (!rows?.length) return setErrors(e => ({ ...e, result: 'Compare the stock sheets first, then export.' }));
    if (!(await toastStore.defer('ok', 'The comparison will be exported when this closes.', 'Export cancelled.'))) return;
    downloadXls('CompareStock', 'Compare Stock', `Old: ${older?.sheet.name ?? ''}  New: ${newer?.sheet.name ?? ''}`,
      [['code', 'Item Code', 'left'], ['name', 'Item Name', 'left'], ['oldQty', 'Old Closing Qty', 'right'], ['newQty', 'New Closing Qty', 'right'],
        ['qtyDiff', 'Qty Diff', 'right'], ['oldValue', 'Old Value', 'right'], ['newValue', 'New Value', 'right'], ['valueDiff', 'Value Diff', 'right'],
        ['status', 'Status', 'left']].map(([key, lbl, align]) => ({ key, label: lbl, align: align  })),
      shown.map(r => ({ ...r })));
    toastStore.add('ok', 'Comparison exported.');
  };

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Compare Stock</h1>
          <p className={pageSub}>Compare two stock sheets (for example yesterday's and today's closing stock, or the desktop app's and the web app's) item by item.</p>
        </div>
        <div className="flex gap-2">
          <button className={exportButton} onClick={exportXls}><FileSpreadsheet size={14} />Export to Excel</button>
          <button className={primaryButton} onClick={compare}><GitCompare size={14} />Compare Stocks</button>
        </div>
      </div>
      <div className="flex flex-col gap-4 lg:flex-row">
        <SidePanel title="Old" side={older} setSide={s => { setOlder(s); setRows(null); }} error={errors.old} setError={e => setErrors(x => ({ ...x, old: e }))} />
        <SidePanel title="New" side={newer} setSide={s => { setNewer(s); setRows(null); }} error={errors.new} setError={e => setErrors(x => ({ ...x, new: e }))} />
      </div>
      <InlineError message={errors.result} />
      {rows && (
        <section className={`${panel} overflow-hidden`}>
          <div className="flex items-center justify-between border-b border-[#DDE3EC] px-4 py-2 text-xs dark:border-line">
            <span>{rows.filter(r => r.status !== 'OK').length} of {rows.length} item(s) differ</span>
            <label className="flex items-center gap-2"><input type="checkbox" checked={showOk} onChange={e => setShowOk(e.target.checked)} />Show matching items</label>
          </div>
          <div className="max-h-[60vh] overflow-auto">
            <table className="min-w-full text-xs">
              <thead>
                <tr>
                  <th className={`${tableHead} px-3 py-2`}>Item</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Closing Qty (Old)</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Closing Qty (New)</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Qty Diff</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Value (Old)</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Value (New)</th>
                  <th className={`${tableHead} px-3 py-2 text-right`}>Value Diff</th>
                  <th className={`${tableHead} px-3 py-2`}>Status</th>
                </tr>
              </thead>
              <tbody className={tableBody}>
                {shown.map(r => (
                  <tr key={r.key}>
                    <td className="px-3 py-1.5"><span className="font-medium">{r.name}</span> <span className="font-mono text-[11px] text-stone-400">{r.code}</span></td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{qty(r.oldQty)}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{qty(r.newQty)}</td>
                    <td className={`px-3 py-1.5 text-right font-semibold tabular-nums ${r.qtyDiff ? 'text-[#9A4B45] dark:text-red-400' : ''}`}>{qty(r.qtyDiff)}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{amt(r.oldValue)}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{amt(r.newValue)}</td>
                    <td className={`px-3 py-1.5 text-right tabular-nums ${r.valueDiff ? 'text-amber-700 dark:text-amber-400' : ''}`}>{amt(r.valueDiff)}</td>
                    <td className="px-3 py-1.5"><span className={badge(TONE[r.status])}>{r.status}</span></td>
                  </tr>
                ))}
                {!shown.length && <tr><td colSpan={8} className="px-3 py-6 text-center text-stone-500">Every item matches.</td></tr>}
              </tbody>
            </table>
          </div>
        </section>
      )}
    </div>
  );
}
