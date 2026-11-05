// Update Item Stock (legacy frmUpdateItemStock): load a sheet of ItemCode / ItemName / UOM / Stock / Rate, see the
// erroneous rows in red (item not found, bad stock), remove them, and import the rest as one purchase bill of
// opening stock. Nothing is imported until every row is right. "Include Rate" uses the sheet's rate, else the item's purchase rate.

import { useState } from 'react';
import { Download, FileSpreadsheet, Trash2, Upload } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, newIdempotencyKey, updateItemStock } from '../../lib/api';
import { downloadText, toCsv } from '../../lib/csv';
import { downloadXls } from '../../lib/xlsExport';
import { readTableFile } from '../../lib/readTableFile';
import { exportButton, field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const HEAD = ['ItemCode', 'ItemName', 'UOM', 'Stock', 'Rate'];
const norm = (s) => String(s).toLowerCase().replace(/[^a-z0-9]/g, '');

// Columns are found from the header row when there is one, else the template's own order.
function toRows(table) {
  const first = table[0].map(norm);
  const has = first.includes('itemcode') || first.includes('stock');
  const at = (name, fallback) => (has && first.indexOf(name) >= 0 ? first.indexOf(name) : fallback);
  const ix = { code: at('itemcode', 0), name: at('itemname', 1), uom: at('uom', 2), stock: at('stock', 3), rate: at('rate', 4) };
  return (has ? table.slice(1) : table).map(r => ({
    code: (r[ix.code] ?? '').trim(), name: r[ix.name] ?? '', uom: r[ix.uom] ?? '', stock: (r[ix.stock] ?? '').trim(), rate: (r[ix.rate] ?? '').trim(), error: '',
  }));
}

export default function UpdateItemStock() {
  const [rows, setRows] = useState([]);
  const [includeRate, setIncludeRate] = useState(false);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const [key, setKey] = useState(newIdempotencyKey());   // one key per loaded sheet, so a double click or a retry cannot import it twice

  const payload = (list, dryRun) => ({
    rows: list.map(r => ({ item_code: r.code, stock: r.stock || '0', rate: r.rate })), include_rate: includeRate, dry_run: dryRun,
  });
  const check = (list) => updateItemStock(payload(list, true))
    .then(res => setRows(list.map((r, i) => ({ ...r, error: res.rows[i]?.error ?? '' }))))
    .catch(e => setProblem(errorText(e)));

  const onFile = (e) => {
    const file = e.target.files?.[0];
    e.target.value = '';
    if (!file) return;
    setProblem('');
    readTableFile(file).then(table => {
      if (!table.length) return setProblem('The file has no rows.');
      const list = toRows(table);
      if (!list.length) return setProblem('The file has no data rows.');
      setRows(list);
      setKey(newIdempotencyKey());
      return check(list);
    }).catch(err => setProblem(err instanceof Error ? err.message : 'Cannot Import From Excel.'));
  };

  const errors = rows.filter(r => r.error);
  const removeErrors = async () => {
    if (!(await toastStore.defer('delete', `${errors.length} erroneous record(s) will be removed when this closes.`, 'Cancelled. The records are still listed.'))) return;
    setRows(cur => cur.filter(r => !r.error));
  };
  const doImport = async () => {
    if (!rows.length || errors.length) return setProblem('Importing of data is not allowed until all the data is correct.');
    setProblem('');
    if (!(await toastStore.defer('ok', `${rows.length} item stock record(s) will be imported when this closes.`, 'Import cancelled. Nothing was changed.'))) return;
    setBusy(true);
    updateItemStock(payload(rows, false), key)
      .then(res => { toastStore.add('ok', `Imported Data Successfully! Purchase bill ${res.doc_no} (${res.imported} items).`); setRows([]); setKey(newIdempotencyKey()); })
      .catch(e => setProblem(errorText(e)))
      .finally(() => setBusy(false));
  };
  const amount = (r) => (Number(r.stock) || 0) * (Number(r.rate) || 0);

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Update Item Stock</h1>
          <p className={pageSub}>Import opening stock from a sheet; it is posted as one purchase bill from KSBCL.</p>
        </div>
        <button onClick={() => void doImport()} disabled={busy || !rows.length || errors.length > 0} className={primaryButton}><Upload size={14} />Import data to database</button>
      </div>
      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <button type="button" className={secondaryButton} onClick={() => downloadText('StockImport_Template.csv', toCsv([HEAD]))}><Download size={14} />Template</button>
        <div>
          <label htmlFor="uis-file" className={label}>Select Excel File</label>
          <input id="uis-file" type="file" accept=".csv,.txt,.xls,.xlsx" onChange={onFile} className={field} />
        </div>
        <label className="flex items-center gap-2 text-xs">
          <input type="checkbox" checked={includeRate} onChange={e => setIncludeRate(e.target.checked)} />Include Rate
        </label>
        <button type="button" disabled={!errors.length} onClick={() => void removeErrors()} className={secondaryButton}><Trash2 size={14} />Remove erroneous records</button>
        <button type="button" disabled={!rows.length} className={exportButton}
          onClick={() => downloadXls('Item_Stock_Import', 'Update Item Stock', '', [...HEAD, 'Amount', 'Error'].map((l, i) => ({ key: String(i), label: l })),
            rows.map(r => [r.code, r.name, r.uom, r.stock, r.rate, amount(r), r.error]))}>
          <FileSpreadsheet size={14} />Export to excel
        </button>
        <span className="ml-auto text-xs text-stone-500">
          Total Number of Records Loaded : <b>{rows.length}</b>{errors.length > 0 && <span className="ml-2 text-red-600">{errors.length} with errors</span>}
        </span>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} max-h-[60vh] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr>
              <th className="px-3 py-2">Slno</th><th className="px-3 py-2">ItemCode</th><th className="px-3 py-2">ItemName</th><th className="px-3 py-2">UOM</th>
              <th className="px-3 py-2 text-right">Stock</th><th className="px-3 py-2 text-right">Rate</th><th className="px-3 py-2 text-right">Amount</th><th className="px-3 py-2">Error</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={8} className="px-4 py-14 text-center text-stone-500">Pick a file with ItemCode, ItemName, UOM, Stock and Rate columns.</td></tr>}
            {rows.map((r, i) => (
              <tr key={i} className={r.error ? 'text-red-600' : ''}>
                <td className="px-3 py-1.5">{i + 1}</td><td className="px-3 py-1.5">{r.code}</td><td className="px-3 py-1.5">{r.name}</td><td className="px-3 py-1.5">{r.uom}</td>
                <td className="px-3 py-1.5 text-right tabular-nums">{r.stock}</td><td className="px-3 py-1.5 text-right tabular-nums">{r.rate}</td>
                <td className="px-3 py-1.5 text-right tabular-nums">{money(amount(r))}</td><td className="px-3 py-1.5">{r.error}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
