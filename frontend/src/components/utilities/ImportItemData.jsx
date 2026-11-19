// Import Item Data From Excel (legacy frmImportItemXLData): pick the item sheet with "...", press Load from Excel, and every row is checked
// by the server (the same rules as the Item Master form: group, UOM, package, brand, liquor category, MRP, duplicates ...). Rows that are
// wrong show a red box round the wrong cell (and the reason in the Error column); "Remove erroneous records" drops them, and "Import data to database" is allowed only when no
// wrong row is left. Template gives the blank sheet, Export to excel saves the grid (with the reasons), Close goes back to the main window.
// Rules and columns are the exe's (frmImportItemXLData.ValidateItemEntry / SAveItemMaster): an item code already present, a group, UOM, package or brand
// that is not in its master, a liquor type other than Beer / Wine / IML / Others, an MRP of 0 and a code twice in the sheet are errors; an empty package
// or brand becomes "-" and an empty Non-Stockable "N". The grid draws 100 rows first and 100 more each time it is scrolled to its end, so a sheet of thousands of items stays quick.

import { useEffect, useRef, useState } from 'react';
import { Download, FileSpreadsheet, Trash2, Upload, X } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, importItems, newIdempotencyKey } from '../../lib/api';
import { goHome } from '../../lib/nav';
import { importBody, parseItemSheet } from '../../lib/itemImport';
import { downloadXls } from '../../lib/xlsExport';
import { readTableFile } from '../../lib/readTableFile';
import { loadStockItems } from '../../data/stockData';
import { exportButton, field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import BackToMaster from '../common/BackToMaster';

const PAGE = 100;
// The grid's columns, in the exe's order: [heading, where the value is, key, align]. "x" = an extra column of the item table (the sheet's other columns).
const COLUMNS = [
  ['ItemCode', 'row', 'code'], ['ItemName', 'row', 'name'], ['Short Name', 'row', 'short_name'], ['ItemGroup', 'row', 'group'], ['Brand', 'row', 'brand'],
  ['Liq Category', 'row', 'liqtype'], ['UOM', 'row', 'uom'], ['Pkg.ItemName', 'x', 'PKGITEM'], ['Pkg.Type', 'row', 'package'], ['Bots.PerCase', 'row', 'bottles_per_case', 'right'],
  ['MRP', 'row', 'mrp', 'right'], ['Purchase Rate', 'row', 'purchase_rate', 'right'], ['Cgst%', 'row', 'cgst_per', 'right'], ['Sgst%', 'row', 'sgst_per', 'right'],
  ['ROL', 'row', 'rol', 'right'], ['ROQ', 'row', 'roq', 'right'], ['MaxLevel', 'x', 'MAXLEVEL', 'right'], ['MinLevel', 'x', 'MINLEVEL', 'right'], ['HSNcode', 'row', 'hsn_code'],
  ['Non-Stockable', 'x', 'NONSTKABLE'], ['CessPer', 'x', 'CESSPER', 'right'], ['CompCess', 'x', 'COMPCESS', 'right'],
];
// The server names a wrong cell by the item field; the grid's extra columns are keyed by their database column.
const FIELD_OF = { PKGITEM: 'pkg_item', NONSTKABLE: 'non_stockable' };
// Is this cell one the server's check refused?
const isWrong = (r, c) => r.fields.includes(c[2]) || r.fields.includes(FIELD_OF[c[2]]);
const valueOf = (record, [, where, key]) => String((where === 'x' ? record.extras[key] : record.row[key]) ?? '');

export default function ImportItemData() {
  const [file, setFile] = useState(null);
  const [rows, setRows] = useState([]);       // [{ record: { row, extras }, error }]
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const [limit, setLimit] = useState(PAGE);
  const [key, setKey] = useState(newIdempotencyKey());   // one per loaded sheet: a double click or a retry cannot import it twice
  const picker = useRef(null);
  const grid = useRef(null);

  const errors = rows.filter(r => r.error);

  const pick = (e) => {
    const f = e.target.files?.[0];
    e.target.value = '';
    if (f) { setFile(f); setProblem(''); }
  };

  const load = async () => {
    if (!file) { setProblem('Please select excel file path..'); picker.current?.click(); return; }
    setProblem(''); setBusy(true);
    try {
      const { records, error } = parseItemSheet(await readTableFile(file));
      if (error) { setRows([]); return setProblem(error); }
      if (!records.length) { setRows([]); return setProblem('There is no data for importing'); }
      if (records.length > 5000) { setRows([]); return setProblem('Import at most 5000 items at a time. Split the sheet.'); }
      const checked = await importItems(records.map(importBody), { dryRun: true });
      setRows(records.map((record, i) => ({ record, error: checked.rows?.[i]?.error ?? '', fields: checked.rows?.[i]?.fields ?? [] })));
      setLimit(PAGE); setKey(newIdempotencyKey());
      grid.current?.scrollTo?.({ top: 0 });
    } catch (e) {
      setRows([]); setProblem(e instanceof Error ? errorText(e) : 'Cannot Import From Excel.');
    } finally { setBusy(false); }
  };

  const template = () => {
    const link = document.createElement('a');
    link.href = `${import.meta.env.BASE_URL}ItemImport_Template.xlsx`;
    link.download = 'ItemImport_Template.xlsx';
    document.body.appendChild(link); link.click(); link.remove();
  };

  const removeErrors = async () => {
    if (!(await toastStore.defer('delete', `${errors.length} erroneous record(s) will be removed when this closes.`, 'Cancelled. The records are still listed.'))) return;
    setRows(cur => cur.filter(r => !r.error));
  };

  const doImport = async () => {
    if (!rows.length) return setProblem('There is no data to import. Load the Excel file first.');
    if (errors.length) return setProblem('Importing of data is not allowed until all the data is correct.');
    setProblem('');
    const body = rows.map(r => importBody(r.record));
    if (!(await toastStore.defer('ok', `${rows.length} item(s) will be imported when this closes.`, 'Import cancelled. Nothing was added.'))) return;
    setBusy(true);
    importItems(body, { key })
      .then(res => {
        toastStore.add('ok', `Imported Data Successfully! ${res.added.length} item(s) added${res.duplicates.length ? `, ${res.duplicates.length} skipped as duplicates` : ''}.`);
        setRows([]); setFile(null); setKey(newIdempotencyKey());
        void loadStockItems().catch(() => undefined);   // the bills' item list follows the Item Master at once
        goHome();   // the exe closes the form once the data is imported
      })
      .catch(e => setProblem(errorText(e)))
      .finally(() => setBusy(false));
  };

  const exportGrid = () => downloadXls('Item_Import', 'Import Item Data From Excel', file?.name ?? '',
    ['Slno', ...COLUMNS.map(c => c[0]), 'Error'].map((l, i) => ({ key: String(i), label: l })),
    rows.map((r, i) => [i + 1, ...COLUMNS.map(c => valueOf(r.record, c)), r.error]),
    rows.map(r => COLUMNS.flatMap((c, n) => (isWrong(r, c) ? [n + 1] : []))));   // the refused cells are painted red in the sheet (column 0 is Slno)

  const onScroll = (e) => { const el = e.currentTarget; if (el.scrollTop + el.clientHeight >= el.scrollHeight - 80) setLimit(l => (l < rows.length ? l + PAGE : l)); };
  useEffect(() => {   // a tall window can hold PAGE rows without a scrollbar: keep adding until it fills or the sheet ends
    const el = grid.current;
    if (el && el.clientHeight > 0 && el.scrollHeight <= el.clientHeight && limit < rows.length) setLimit(l => l + PAGE);
  }, [limit, rows.length]);

  const shown = rows.slice(0, limit);
  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Import Item Data From Excel</h1>
          <p className={pageSub}>Add many items at once from the item sheet. Every row is checked before anything is imported.</p>
        </div>
        <BackToMaster />
      </div>
      <div className={`${panel} flex flex-col gap-3 px-5 py-4`}>
        <div className="flex flex-wrap items-end gap-3">
          <div className="min-w-64 flex-1">
            <label htmlFor="iid-file" className={label}>Select Excel File</label>
            <div className="flex gap-2">
              <input id="iid-file" readOnly value={file?.name ?? ''} placeholder="No file chosen" className={`${field} flex-1`} />
              <button type="button" onClick={() => picker.current?.click()} className={secondaryButton} aria-label="Browse for the Excel file" title="Browse">...</button>
            </div>
            <input ref={picker} type="file" accept=".csv,.txt,.xls,.xlsx" onChange={pick} hidden data-testid="iid-picker" />
          </div>
          <button type="button" onClick={() => void load()} disabled={busy} className={secondaryButton}><FileSpreadsheet size={14} />Load from Excel</button>
          <button type="button" onClick={template} className={secondaryButton}><Download size={14} />Template</button>
        </div>
        <p className="text-xs font-semibold text-blue-700 dark:text-blue-300">Please select a excel file containing Item Details</p>
        <div className="flex items-center gap-2 text-xs">
          <span>Total Number of Records Loaded :</span>
          <output className="inline-block min-w-16 rounded border border-line px-2 py-1 text-right font-semibold" aria-label="Total Number of Records Loaded">{rows.length || ''}</output>
          {errors.length > 0 && <span className="font-semibold text-red-600">{errors.length} erroneous</span>}
        </div>
      </div>
      {problem && <InlineError message={problem} />}
      <div ref={grid} onScroll={onScroll} className={`${panel} max-h-[56vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr>
              <th className="px-3 py-2 text-left">Slno</th>
              {COLUMNS.map(([name, , , align]) => <th key={name} className={`whitespace-nowrap px-3 py-2 ${align === 'right' ? 'text-right' : 'text-left'}`}>{name}</th>)}
              <th className="px-3 py-2 text-left">Error</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={COLUMNS.length + 2} className="px-4 py-14 text-center text-stone-500">Select the Excel file and press Load from Excel.</td></tr>}
            {shown.map((r, i) => (
              <tr key={i} title={r.error || undefined}>
                <td className="px-3 py-1.5">{i + 1}</td>
                {COLUMNS.map(c => {
                  const wrong = isWrong(r, c);   // this very cell is what the check refused
                  return (
                    <td key={c[0]} className="px-1.5 py-1">
                      <span className={`block whitespace-nowrap rounded border px-1.5 py-0.5 ${c[3] === 'right' ? 'text-right tabular-nums' : ''} ${wrong ? 'border-red-600 bg-red-50 text-red-700 ring-1 ring-red-600 dark:border-red-400 dark:bg-red-500/10 dark:text-red-300' : 'border-transparent'}`}
                        {...(wrong ? { 'aria-invalid': true, title: r.error } : {})}>{valueOf(r.record, c)}</span>
                    </td>
                  );
                })}
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
          {shown.length < rows.length && <p className="mt-1 text-stone-500">Showing {shown.length} of {rows.length}; scroll down for more.</p>}
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
