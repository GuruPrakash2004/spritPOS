// Item Master Options menu dialogs of the exe's frmMstItem: "Items-DataLink Problem" (frmRPtInvalidItems: the database view of items
// whose links point at nothing, exportable) and "Update Item Master Fields" (frmUtlImportAnyFld: choose fields and items, get a template
// or load the values into a grid, edit, import a filled template, Import Data updates MstItem by ItemID).

import { useEffect, useState } from 'react';
import Modal from '../common/Modal';
import { toastStore } from '../common/toastStore';
import { downloadXls, downloadXlsRows } from '../../lib/xlsExport';
import { readTableFile } from '../../lib/readTableFile';
import { errorText, getBulkFieldDefs, getInvalidItems, getItems, importBulkFields, listMaster, loadBulkFields } from '../../lib/api';
import { field, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export function DataLinkProblemDialog({ onClose }) {
  const [data, setData] = useState(null);
  const [error, setError] = useState('');
  useEffect(() => { getInvalidItems().then(setData).catch(e => setError(errorText(e))); }, []);
  const columns = (data?.columns ?? []).map(c => ({ key: c, label: c }));
  return (
    <Modal title="Items-DataLink Problem" onClose={onClose} wide
      footer={<>
        <button className={secondaryButton} disabled={!data?.rows?.length} onClick={() => void downloadXls('Items_DataLink_Problem', 'Items-DataLink Problem', '', columns, data.rows)}>Export to Excel</button>
        <button className={primaryButton} onClick={onClose}>Close</button>
      </>}>
      {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
      {!data && !error && <p>Loading…</p>}
      {data && !data.rows.length && <p>{data.note || 'No items with a data-link problem.'}</p>}
      {!!data?.rows?.length && (
        <table className="w-full">
          <thead className={tableHead}><tr>{columns.map(c => <th key={c.key} className="px-2 py-1.5 text-left">{c.label}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {data.rows.map((r, i) => <tr key={i}>{columns.map(c => <td key={c.key} className="px-2 py-1.5">{String(r[c.key] ?? '')}</td>)}</tr>)}
          </tbody>
        </table>
      )}
    </Modal>
  );
}

const pick = (e) => Array.from(e.target.selectedOptions).map(o => o.value);

export function UpdateItemFieldsDialog({ onClose, onDone }) {
  const [defs, setDefs] = useState([]);
  const [chosen, setChosen] = useState([]);
  const [groups, setGroups] = useState([]);
  const [packages, setPackages] = useState([]);
  const [items, setItems] = useState([]);
  const [lists, setLists] = useState({ groups: [], packages: [], items: [] });
  const [grid, setGrid] = useState(null);   // { columns, rows }
  const [message, setMessage] = useState('');

  useEffect(() => {
    getBulkFieldDefs().then(setDefs).catch(e => setMessage(errorText(e)));
    Promise.all([listMaster('item-groups'), listMaster('packages'), getItems()])
      .then(([g, p, i]) => setLists({ groups: g.map(r => r.code), packages: p.map(r => r.code), items: i.filter(r => r.active !== false).map(r => r.code) }))
      .catch(() => undefined);
  }, []);

  const toggle = (code) => setChosen(c => (c.includes(code) ? c.filter(x => x !== code) : [...c, code]));
  const load = async () => {
    if (!chosen.length) { setMessage('Please Select atleast one field!'); return null; }
    setMessage('');
    try {
      const r = await loadBulkFields({ fields: chosen, groups, items, packages });
      setGrid(r);
      return r;
    } catch (e) { setMessage(errorText(e)); return null; }
  };
  const template = async () => {
    const r = await load();
    if (!r) return;
    const fields = r.columns.slice(3);
    downloadXlsRows(`${new Date().toISOString().replace(/\D/g, '').slice(0, 14)}_BulkImpFlds`,
      [['ItemID', 'Item Name', ...fields], ...r.rows.map(row => [row.ItemID, row.ItemName, ...fields.map(f => row[f] ?? '')])]);
    setMessage('Template Created');
  };
  const chooseFile = async (file) => {
    setMessage('');
    let rows;
    try { rows = await readTableFile(file); } catch (e) { return setMessage(e.message); }
    const head = rows[0] ?? [];
    if (`${head[0] ?? ''},${head[1] ?? ''}`.toUpperCase() !== 'ITEMID,ITEM NAME') return setMessage('Please Select a valid Template!');
    const columns = ['ItemID', 'ItemName', ...head.slice(2).filter(h => h.trim() !== '')];
    const body = rows.slice(1).filter(r => Number(r[0]) > 0).map(r => Object.fromEntries(columns.map((c, i) => [c, r[i] ?? ''])));
    setGrid({ columns: ['ItemID', 'ItemCode', ...columns.slice(1)], rows: body });
  };
  const edit = (i, col, v) => setGrid(g => ({ ...g, rows: g.rows.map((r, j) => (j === i ? { ...r, [col]: v } : r)) }));
  const importData = async () => {
    if (!grid?.rows?.length) return;
    if (!(await toastStore.defer('ok', `${grid.rows.length} items will be updated when this closes.`, 'Import cancelled. Nothing was changed.'))) return;
    try {
      const r = await importBulkFields(grid.rows);
      setMessage(`Data Imported Successfully! (${r.updated} items)`);
      onDone?.();
      onClose();
    } catch (e) { setMessage(errorText(e)); }
  };
  const shown = (grid?.columns ?? []).filter(c => c !== 'ItemID' && c !== 'ItemCode');

  return (
    <Modal title="Update Item Master Fields" onClose={onClose} wide
      footer={<>
        <button className={secondaryButton} disabled={!grid?.rows?.length} onClick={() => void downloadXls('Item_Master_Fields', 'Update Item Master Fields', '', shown.map(c => ({ key: c, label: c })), grid.rows)}>Export to Excel</button>
        <button className={secondaryButton} onClick={onClose}>Close</button>
        <button className={primaryButton} disabled={!grid?.rows?.length} onClick={() => void importData()}>Import Data</button>
      </>}>
      <div className="grid gap-4 md:grid-cols-2">
        <div>
          <div className="mb-1 font-bold">Select Fields to Import</div>
          <div className="max-h-40 overflow-auto rounded border border-line p-2">
            {defs.length === 0 && <p className="text-stone-500">No fields are set up for this shop (UtlItemBulkupdate).</p>}
            {defs.map(d => (
              <label key={d.code} className="flex items-center gap-2 py-0.5"><input type="checkbox" checked={chosen.includes(d.code)} onChange={() => toggle(d.code)} />{d.code}<span className="text-stone-400">{d.datatype}</span></label>
            ))}
          </div>
        </div>
        <div className="grid grid-cols-3 gap-2">
          {[['Item Groups', lists.groups, setGroups], ['Packing', lists.packages, setPackages], ['Items', lists.items, setItems]].map(([label, list, set]) => (
            <label key={label} className="block"><span className="mb-1 block font-bold">{label}</span>
              <select multiple className={`${field} h-36`} onChange={e => set(pick(e))}>{list.map(c => <option key={c}>{c}</option>)}</select>
            </label>
          ))}
        </div>
      </div>
      <div className="mt-3 flex flex-wrap items-center gap-2">
        <button className={secondaryButton} onClick={() => void template()}>Template</button>
        <button className={secondaryButton} onClick={() => void load()}>Load to Grid</button>
        <label className={`${secondaryButton} cursor-pointer`}>Select file<input type="file" hidden accept=".xls,.xlsx,.csv" onChange={e => { const f = e.target.files?.[0]; if (f) void chooseFile(f); e.target.value = ''; }} /></label>
        {message && <span role="status" className="text-accent-ink">{message}</span>}
      </div>
      {!!grid?.rows?.length && (
        <div className="mt-3 max-h-72 overflow-auto">
          <table className="w-full">
            <thead className={tableHead}><tr>{shown.map(c => <th key={c} className="px-2 py-1.5 text-left">{c}</th>)}</tr></thead>
            <tbody className={tableBody}>
              {grid.rows.map((r, i) => (
                <tr key={r.ItemID ?? i}>
                  {shown.map(c => c === 'ItemName'
                    ? <td key={c} className="px-2 py-1">{String(r[c] ?? '')}</td>
                    : <td key={c} className="px-1 py-0.5"><input className={field} value={String(r[c] ?? '')} onChange={e => edit(i, c, e.target.value)} /></td>)}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Modal>
  );
}
