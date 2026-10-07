// Item Master: MasterCrud for /masters/items plus the legacy "Import Item from Excel" flow
// (pick the template saved as CSV → preview with duplicates in red → POST /masters/items/import).

import { useRef, useState } from 'react';
import { FileDown, FileUp, Upload } from 'lucide-react';
import MasterCrud from './MasterCrud';
import Modal from '../common/Modal';
import { toastStore } from '../common/toastStore';
import { errorText, getItems, importItems } from '../../lib/api';
import { downloadText, parseCsv, toCsv } from '../../lib/csv';
import { ITEM_FIELDS, ITEM_IMPORT_COLUMNS, ITEM_IMPORT_NUMERIC } from './masterConfigs';
import { primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';



// Item Master = the generic master screen plus the legacy "Import Item from Excel" flow:
// download template → fill in Excel → save as CSV → load → duplicates shown in red → import.
export default function ItemMaster() {
  const [reloadKey, setReloadKey] = useState(0);
  const [preview, setPreview] = useState(null);
  const [message, setMessage] = useState('');
  const fileInput = useRef(null);

  const downloadTemplate = () => {
    const header = ITEM_IMPORT_COLUMNS.map(([col]) => col);
    downloadText('ItemImport_Template.csv', toCsv([header]));
  };

  const loadFile = async (file) => {
    setMessage('');
    const [header, ...body] = parseCsv(await file.text());
    if (!header) return setMessage('The file is empty.');
    const index = new Map(header.map((h, i) => [h.trim().toUpperCase(), i]));
    const missing = ITEM_IMPORT_COLUMNS.filter(([col]) => !index.has(col.toUpperCase())).map(([col]) => col);
    if (missing.length) return setMessage(`Missing columns: ${missing.join(', ')}. Use Download Template.`);
    const existing = new Set((await getItems()).map(i => i.code));
    const seen = new Set();
    setPreview(body.map(cells => {
      const row = {};
      ITEM_IMPORT_COLUMNS.forEach(([col, key]) => {
        const raw = (cells[index.get(col.toUpperCase())] ?? '').trim();
        row[key] = ITEM_IMPORT_NUMERIC.has(key) ? Number(raw) || 0 : raw;
      });
      const code = String(row.code);
      const duplicate = existing.has(code) || seen.has(code);
      seen.add(code);
      const problem = !code || !row.name ? 'Code and name are required'
        : Number(row.bottles_per_case) < 1 ? 'Bottles/Case must be ≥ 1'
        : !row.group || !row.package ? 'Item group and package are required' : '';
      return { row, duplicate, problem };
    }));
  };

  const importRows = async () => {
    const good = preview?.filter(p => !p.duplicate && !p.problem) ?? [];
    if (!good.length) return setMessage('Nothing to import: every row is a duplicate or has a problem.');
    const body = good.map(p => ({ ...p.row, short_name: p.row.short_name || p.row.code, qoh: 0, active: true }));
    if (!(await toastStore.defer('ok', `${good.length} items will be imported when this closes.`, 'Import cancelled. No items were added.'))) return;
    importItems(body)
      .then(r => {
        setPreview(null);
        setReloadKey(k => k + 1);
        setMessage(`Imported ${r.added.length} items${r.duplicates.length ? `, skipped ${r.duplicates.length} duplicates` : ''}.`);
      })
      .catch(e => setMessage(errorText(e)));
  };

  return (
    <>
      <MasterCrud
        title="Item Master"
        subtitle="Liquor and non-liquor items, packages, MRP, purchase rate and reorder levels."
        path="items"
        fields={ITEM_FIELDS}
        reloadKey={reloadKey}
        toolbar={<>
          <button onClick={downloadTemplate} className={secondaryButton}><FileDown size={14} />Download Template</button>
          <button onClick={() => fileInput.current?.click()} className={secondaryButton}><FileUp size={14} />Import Items</button>
          <input ref={fileInput} type="file" accept=".csv,text/csv" hidden onChange={e => { const f = e.target.files?.[0]; if (f) void loadFile(f); e.target.value = ''; }} />
        </>}
      />
      {message && !preview && (
        <p role="status" className="mx-auto mt-3 max-w-6xl rounded-lg bg-accent/10 px-4 py-2 text-xs font-medium text-accent-ink">{message}</p>
      )}
      {preview && (
        <Modal
          title={`Import Items — ${preview.length} rows`}
          onClose={() => setPreview(null)}
          wide
          footer={<>
            <button onClick={() => setPreview(null)} className={secondaryButton}>Cancel</button>
            <button onClick={importRows} className={primaryButton}><Upload size={14} />Import {preview.filter(p => !p.duplicate && !p.problem).length} items</button>
          </>}
        >
          <p className="mb-2 text-stone-500">Rows in <span className="font-bold text-red-600 dark:text-red-400">red</span> are duplicates or have a problem and will be skipped.</p>
          {message && <p className="mb-2 text-red-600 dark:text-red-400">{message}</p>}
          <table className="w-full">
            <thead className={tableHead}>
              <tr><th className="px-2 py-1.5">Code</th><th className="px-2 py-1.5">Name</th><th className="px-2 py-1.5">Group</th><th className="px-2 py-1.5">Pkg</th><th className="px-2 py-1.5 text-right">MRP</th><th className="px-2 py-1.5">Status</th></tr>
            </thead>
            <tbody className={tableBody}>
              {preview.map((p, i) => (
                <tr key={i} className={p.duplicate || p.problem ? 'text-red-600 dark:text-red-400' : ''}>
                  <td className="px-2 py-1.5 font-mono">{p.row.code}</td>
                  <td className="px-2 py-1.5">{p.row.name}</td>
                  <td className="px-2 py-1.5">{p.row.group}</td>
                  <td className="px-2 py-1.5">{p.row.package}</td>
                  <td className="px-2 py-1.5 text-right font-mono">{p.row.mrp}</td>
                  <td className="px-2 py-1.5">{p.duplicate ? 'Duplicate' : p.problem || 'OK'}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </Modal>
      )}
    </>
  );
}
