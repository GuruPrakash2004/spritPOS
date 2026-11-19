// Update Item Stock (legacy frmUpdateItemStock): pick the stock sheet with "...", press Load from Excel, and each row is checked (item not
// found, negative stock). A wrong cell gets a red box (and is painted red in Export to excel), the reason is in the Error column, "Remove
// erroneous records" drops the bad rows, and "Import data to database" is allowed only when every row is right. The import does NOT save anything:
// it opens Purchase Bill with the stock as the lines of one bill from KSBCL (its own number series, prefix OP: 26OP00001, 26OP00002 ..., no GST); the
// user enters the supplier invoice no., date and value there and the bill saves when its total equals the invoice value, as every purchase bill.
// "Include Rate" uses the sheet's rate, else the item's purchase rate.
// The sheet is the exe's StockImport template: a hint row, then the heading row ITEMCODE, ITEMNAME, UOMCode, Stock, Rate, and the items below.

import { useRef, useState } from 'react';
import { Download, FileSpreadsheet, Trash2, Upload, X } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getItems, updateItemStock } from '../../lib/api';
import { goHome, openModule } from '../../lib/nav';
import { setOpeningDraft } from '../../lib/billDraft';
import { downloadXls, downloadXlsRows } from '../../lib/xlsExport';
import { readTableFile } from '../../lib/readTableFile';
import { loadStockItems } from '../../data/stockData';
import { toRows } from '../../lib/stockImportSheet';
import { exportButton, field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import BackToMaster from '../common/BackToMaster';

// The grid: [heading, row key, the server's name for a wrong cell, align].
const COLUMNS = [['ItemCode', 'code', 'code'], ['ItemName', 'name'], ['UOM', 'uom'], ['Stock', 'stock', 'stock', 'right'], ['Rate', 'rate', 'rate', 'right']];

export default function UpdateItemStock() {
  const [file, setFile] = useState(null);
  const [rows, setRows] = useState([]);
  const [includeRate, setIncludeRate] = useState(false);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const picker = useRef(null);

  const payload = (list, dryRun, rate = includeRate) => ({
    rows: list.map(r => ({ item_code: r.code, stock: r.stock || '0', rate: r.rate })), include_rate: rate, dry_run: dryRun,
  });
  const check = (list, rate = includeRate) => updateItemStock(payload(list, true, rate))
    .then(res => setRows(list.map((r, i) => ({ ...r, error: res.rows[i]?.error ?? '', fields: res.rows[i]?.fields ?? [] }))))
    .catch(e => setProblem(errorText(e)));

  const pick = (e) => {
    const f = e.target.files?.[0];
    e.target.value = '';
    if (f) { setFile(f); setProblem(''); }
  };

  const load = async () => {
    if (!file) { setProblem('Please select excel file path..'); picker.current?.click(); return; }
    setProblem(''); setBusy(true);
    try {
      const list = toRows(await readTableFile(file));   // throws "Invalid Template..." when the heading row is wrong
      if (!list.length) { setRows([]); return setProblem('There is no data for importing'); }
      setRows(list);
      await check(list);
    } catch (err) {
      setRows([]); setProblem(err instanceof Error ? err.message : 'Cannot Import From Excel.');
    } finally { setBusy(false); }
  };

  // The exe's Template button fills StockImport_Template.xlsx with every item (code, name, UOM): only Stock and Rate are typed.
  const template = async () => {
    try {
      const items = await getItems();
      downloadXlsRows('StockImport_Template', [
        ['', '', '', 'valid numbers only', 'valid numbers only'],
        ['ITEMCODE', 'ITEMNAME', 'UOMCode', 'Stock', 'Rate'],
        ...items.filter(i => i.code).map(i => [i.code, i.name ?? '', i.uom ?? '', '', '']),
      ], 1);   // the heading row is the second, under the hint row
    } catch (e) {
      setProblem(errorText(e));
    }
  };

  const errors = rows.filter(r => r.error);
  const removeErrors = async () => {
    if (!(await toastStore.defer('delete', `${errors.length} erroneous record(s) will be removed when this closes.`, 'Cancelled. The records are still listed.'))) return;
    setRows(cur => cur.filter(r => !r.error));
  };
  // "Import data to database": the rows (stock above zero) go onto a Purchase Bill from KSBCL, numbered in the OP series, and the Purchase Bill screen
  // opens with them. NOTHING IS SAVED HERE: the user enters the supplier invoice no., date and value there, and the bill saves when its total equals the
  // invoice value. The rate is the sheet's when Include Rate is ticked and it is above 0, else the item's purchase rate.
  const doImport = async () => {
    if (!rows.length) return setProblem('There is no data to import. Load the Excel file first.');
    if (errors.length) return setProblem('Importing of data is not allowed until all the data is correct.');
    const lines = rows.filter(r => Number(r.stock) > 0).map(r => {
      const sheetRate = Number(r.rate) || 0;
      return { item_code: r.code, qty: Math.round(Number(r.stock)), rate: includeRate && sheetRate > 0 ? sheetRate : null };
    });
    if (!lines.length) return setProblem('There is no stock above zero to import.');
    setProblem(''); setBusy(true);
    try {
      const items = await loadStockItems();   // the purchase rate of an item whose sheet rate is not used
      const rateOf = (code) => items.find(i => i.code === code)?.purchaseRate ?? 0;
      setOpeningDraft({ supplier: 'KSBCL', remarks: 'Opening stock (Update Item Stock)', lines: lines.map(l => ({ ...l, rate: l.rate ?? rateOf(l.item_code) })) });
      setRows([]); setFile(null);
      openModule('Purchase Bill');
    } catch (e) {
      setProblem(errorText(e));
    } finally { setBusy(false); }
  };
  const amount = (r) => (Number(r.stock) || 0) * (Number(r.rate) || 0);

  const exportGrid = () => downloadXls('Item_Stock_Import', 'Update Item Stock', file?.name ?? '',
    ['Slno', ...COLUMNS.map(c => c[0]), 'Amount', 'Error'].map((l, i) => ({ key: String(i), label: l })),
    rows.map((r, i) => [i + 1, ...COLUMNS.map(c => r[c[1]]), amount(r), r.error]),
    rows.map(r => COLUMNS.flatMap((c, n) => (c[2] && r.fields.includes(c[2]) ? [n + 1] : []))));   // the refused cells are painted red in the sheet

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Update Item Stock</h1>
          <p className={pageSub}>Import opening stock from a sheet; it goes onto a purchase bill from KSBCL (OP number series) that you save in Purchase Bill once the invoice value is entered.</p>
        </div>
        <BackToMaster />
      </div>
      <div className={`${panel} flex flex-col gap-3 px-5 py-4`}>
        <div className="flex flex-wrap items-end gap-3">
          <div className="min-w-64 flex-1">
            <label htmlFor="uis-file" className={label}>Select Excel File</label>
            <div className="flex gap-2">
              <input id="uis-file" readOnly value={file?.name ?? ''} placeholder="No file chosen" className={`${field} flex-1`} />
              <button type="button" onClick={() => picker.current?.click()} className={secondaryButton} aria-label="Browse for the Excel file" title="Browse">...</button>
            </div>
            <input ref={picker} type="file" accept=".csv,.txt,.xls,.xlsx" onChange={pick} hidden data-testid="uis-picker" />
          </div>
          <button type="button" onClick={() => void load()} disabled={busy} className={secondaryButton}><FileSpreadsheet size={14} />Load from Excel</button>
          <button type="button" className={secondaryButton} onClick={() => void template()}><Download size={14} />Template</button>
          <label className="flex items-center gap-2 pb-2 text-xs">
            <input type="checkbox" checked={includeRate} onChange={e => { setIncludeRate(e.target.checked); if (rows.length) void check(rows, e.target.checked); }} />Include Rate
          </label>
        </div>
        <p className="text-xs font-semibold text-blue-700 dark:text-blue-300">Please select a excel file containing Item Stock Details</p>
        <div className="flex items-center gap-2 text-xs">
          <span>Total Number of Records Loaded :</span>
          <output className="inline-block min-w-16 rounded border border-line px-2 py-1 text-right font-semibold" aria-label="Total Number of Records Loaded">{rows.length || ''}</output>
          {errors.length > 0 && <span className="font-semibold text-red-600">{errors.length} erroneous</span>}
        </div>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} max-h-[56vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr>
              <th className="px-3 py-2 text-left">Slno</th>
              {COLUMNS.map(c => <th key={c[0]} className={`px-3 py-2 ${c[3] === 'right' ? 'text-right' : 'text-left'}`}>{c[0]}</th>)}
              <th className="px-3 py-2 text-right">Amount</th><th className="px-3 py-2 text-left">Error</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={COLUMNS.length + 3} className="px-4 py-14 text-center text-stone-500">Select the Excel file and press Load from Excel.</td></tr>}
            {rows.map((r, i) => (
              <tr key={i} title={r.error || undefined}>
                <td className="px-3 py-1.5">{i + 1}</td>
                {COLUMNS.map(c => {
                  const wrong = !!c[2] && r.fields.includes(c[2]);
                  return (
                    <td key={c[0]} className="px-1.5 py-1">
                      <span className={`block rounded border px-1.5 py-0.5 ${c[3] === 'right' ? 'text-right tabular-nums' : ''} ${wrong ? 'border-red-600 bg-red-50 text-red-700 ring-1 ring-red-600 dark:border-red-400 dark:bg-red-500/10 dark:text-red-300' : 'border-transparent'}`}
                        {...(wrong ? { 'aria-invalid': true, title: r.error } : {})}>{r[c[1]]}</span>
                    </td>
                  );
                })}
                <td className="px-3 py-1.5 text-right tabular-nums">{money(amount(r))}</td>
                <td className="whitespace-nowrap px-3 py-1.5 text-red-600 dark:text-red-400">{r.error}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="flex flex-wrap items-end justify-between gap-3">
        <div className="text-xs text-stone-600 dark:text-stone-300">
          <p>Cells marked with a <b className="rounded border border-red-600 px-1 text-red-600">red box</b> are erroneous.</p>
          <p className="mt-1">Importing data is not allowed until all the data is correct.</p>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <button type="button" onClick={() => void doImport()} disabled={busy || !rows.length || errors.length > 0} className={primaryButton}><Upload size={14} />Import data to database</button>
          <button type="button" disabled={!errors.length} onClick={() => void removeErrors()} className={secondaryButton}><Trash2 size={14} />Remove erroneous records</button>
          <button type="button" disabled={!rows.length} onClick={exportGrid} className={exportButton}><FileSpreadsheet size={14} />Export to excel</button>
          <button type="button" onClick={goHome} className={secondaryButton}><X size={14} />Close</button>
        </div>
      </div>
    </div>
  );
}
