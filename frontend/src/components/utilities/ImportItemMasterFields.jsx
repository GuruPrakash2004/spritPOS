// Import Item Master Fields (the exe's frmUtlImportAnyFld, opened from Item Master > Options > Update Item Master Fields), as a page of its own:
//   left   three tick boxes (Select Item Group / Select Item Packing / Select Item) that open small "Select [Multiple] ..." popups, the table
//          "Select Fields to Import | Field Names" (the fields of UtlItemBulkupdate, plus UOMID), Generate Excel Template and Load to Grid;
//   right  "Select Excel File" (loads the grid as soon as a file is chosen) and the grid, which has the whole width of the page: ItemID and
//          ItemName fixed, the chosen fields editable, Export to Excel.
//   top    Close (back to Item Master) and Import Data (updates MstItem by ItemID, then returns to Item Master, as the exe closes its tab).
// A field ending in ID shows its master's code (BrandID -> the brand code, UOMID -> the UOM code). UOM is changed only here: a UOM change is
// refused by the server for an item that is used in bills or stock.

import { useEffect, useState } from 'react';
import { toastStore } from '../common/toastStore';
import MultiSelectDialog from '../common/MultiSelectDialog';
import { downloadXls, downloadXlsRows } from '../../lib/xlsExport';
import { readTableFile } from '../../lib/readTableFile';
import { openModule } from '../../lib/nav';
import { errorText, getBulkFieldDefs, getItems, importBulkFields, listMaster, loadBulkFields } from '../../lib/api';
import { field, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import BackToMaster from '../common/BackToMaster';

const FILTERS = [['groups', 'Select Item Group'], ['packages', 'Select Item Packing'], ['items', 'Select Item']];

export default function ImportItemMasterFields() {
  const [defs, setDefs] = useState([]);
  const [chosen, setChosen] = useState([]);
  const [filters, setFilters] = useState({ groups: [], packages: [], items: [] });   // codes picked in the three popups
  const [picking, setPicking] = useState('');                                         // 'groups' | 'packages' | 'items' while its popup is open
  const [lists, setLists] = useState({ groups: [], packages: [], items: [], uoms: [] });
  const [grid, setGrid] = useState(null);   // { columns, rows }
  const [fileName, setFileName] = useState('');
  const [message, setMessage] = useState('');

  useEffect(() => {
    getBulkFieldDefs().then(setDefs).catch(e => setMessage(errorText(e)));
    Promise.all([listMaster('item-groups'), listMaster('packages'), getItems(), listMaster('uoms')])
      .then(([g, p, i, u]) => setLists({
        groups: g.filter(r => r.active !== false).sort((a, b) => String(a.code).localeCompare(String(b.code))),
        packages: [...new Map(p.filter(r => r.active !== false).map(r => [r.code, r])).values()].sort((a, b) => Number(a.litres_per_bottle) - Number(b.litres_per_bottle)),
        items: i.filter(r => r.active !== false).sort((a, b) => String(a.name).localeCompare(String(b.name))),
        uoms: u.filter(r => r.active !== false && String(r.code).trim() !== '-').map(r => r.code),
      }))
      .catch(() => undefined);
  }, []);

  const toggle = (code) => setChosen(c => (c.includes(code) ? c.filter(x => x !== code) : [...c, code]));
  // The exe's tick boxes: ticking opens the popup; leaving it with nothing picked unticks the box again; unticking forgets the pick.
  const tick = (which, on) => { if (on) setPicking(which); else setFilters(f => ({ ...f, [which]: [] })); };
  const picked = (which, keys) => { setFilters(f => ({ ...f, [which]: keys })); setPicking(''); };

  // The rows for the chosen fields and filters (the exe runs the same query for the template and for the grid); null with a message when there is nothing.
  const fetchRows = async () => {
    if (!chosen.length) { setMessage('Please Select atleast one field!'); return null; }
    setMessage('');
    try { return await loadBulkFields({ fields: chosen, ...filters }); } catch (e) { setMessage(errorText(e)); return null; }
  };
  // Load to Grid: the rows go into the grid.
  const load = async () => {
    const r = await fetchRows();
    if (r) setGrid(r);
  };
  // Generate Excel Template: only the Excel file is made (and the notice shown); the grid is left alone.
  const template = async () => {
    const r = await fetchRows();
    if (!r) return;
    const fields = r.columns.slice(3);
    downloadXlsRows(`${new Date().toISOString().replace(/\D/g, '').slice(0, 14)}_BulkImpFlds`,
      [['ItemID', 'Item Name', ...fields], ...r.rows.map(row => [row.ItemID, row.ItemName, ...fields.map(f => row[f] ?? '')])]);
    toastStore.add('ok', 'Template Created');
  };
  const chooseFile = async (file) => {
    setMessage('');
    let rows;
    try { rows = await readTableFile(file); } catch (e) { return setMessage(e.message); }
    const head = rows[0] ?? [];
    if (`${head[0] ?? ''},${head[1] ?? ''}`.toUpperCase() !== 'ITEMID,ITEM NAME') return setMessage('Please Select a valid Template!');
    setFileName(file.name);
    const columns = ['ItemID', 'ItemName', ...head.slice(2).filter(h => h.trim() !== '')];
    const body = rows.slice(1).filter(r => Number(r[0]) > 0).map(r => Object.fromEntries(columns.map((c, i) => [c, r[i] ?? ''])));
    setGrid({ columns: ['ItemID', 'ItemCode', ...columns.slice(1)], rows: body });
  };
  const edit = (i, col, v) => setGrid(g => ({ ...g, rows: g.rows.map((r, j) => (j === i ? { ...r, [col]: v } : r)) }));
  const importData = async () => {
    if (!grid?.rows?.length) return;
    if (!(await toastStore.defer('ok', `${grid.rows.length} items will be updated when this closes.`, 'Import cancelled. Nothing was changed.'))) return;
    try {
      await importBulkFields(grid.rows);
      toastStore.add('ok', 'Data Imported Successfully!');
      openModule('Item Master');   // the exe closes the form after a successful import
    } catch (e) { setMessage(`Error During Import! ${errorText(e)}`); }
  };
  const shown = (grid?.columns ?? []).filter(c => c !== 'ItemID' && c !== 'ItemCode');

  return (
    <div className="mx-auto flex max-w-[1680px] flex-col gap-3 text-xs">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Import Item Master Fields</h1>
          <p className={pageSub}>Change item fields in bulk: pick items and fields, load them into the grid (or a filled Excel template), edit, then Import Data. UOM is changed here only.</p>
        </div>
        <div className="flex gap-2">
          <BackToMaster />
          <button type="button" className={primaryButton} disabled={!grid?.rows?.length} onClick={() => void importData()}>Import Data</button>
        </div>
      </div>

      <div className="grid items-start gap-4 lg:grid-cols-[minmax(280px,340px)_minmax(0,1fr)]">
        <section className={`${panel} p-3`} aria-label="Filters and fields">
          {FILTERS.map(([which, text]) => (
            <label key={which} className="flex items-center gap-2 py-0.5 text-xs font-medium">
              <input type="checkbox" className="accent-accent" checked={filters[which].length > 0} onChange={e => tick(which, e.target.checked)} />{text}
              {filters[which].length > 0 && <span className="text-xs font-normal text-stone-500">({filters[which].length})</span>}
            </label>
          ))}
          <div className="mb-1 mt-3 text-xs font-bold">Select Fields to import</div>
          <div className="max-h-[calc(100vh-24rem)] min-h-40 overflow-auto rounded border border-line">
            <table className="w-full">
              <thead className={tableHead}><tr><th className="w-24 px-2 py-1.5 text-left">Select Fields to Import</th><th className="px-2 py-1.5 text-left">Field Names</th></tr></thead>
              <tbody className={tableBody}>
                {defs.length === 0 && <tr><td colSpan={2} className="px-2 py-3 text-stone-500">No fields are set up for this shop (UtlItemBulkupdate).</td></tr>}
                {defs.map(d => (
                  <tr key={d.code} onClick={() => toggle(d.code)} className={`cursor-pointer ${chosen.includes(d.code) ? 'bg-accent/10' : ''}`}>
                    <td className="px-2 py-1 text-center"><input type="checkbox" className="accent-accent" aria-label={`Import ${d.code}`} checked={chosen.includes(d.code)} onChange={() => toggle(d.code)} onClick={e => e.stopPropagation()} /></td>
                    <td className="px-2 py-1">{d.code}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="mt-3 flex flex-col gap-2">
            <button className={secondaryButton} onClick={() => void template()}>Generate Excel Template</button>
            <button className={secondaryButton} onClick={() => void load()}>Load to Grid</button>
          </div>
        </section>

        <section className={`${panel} min-w-0 p-3`} aria-label="Grid">
          <div className="mb-3 flex items-center gap-2">
            <span className="whitespace-nowrap text-xs font-medium">Select Excel File</span>
            <input readOnly value={fileName} aria-label="Selected Excel file" className={`${field} flex-1 !py-1.5`} />
            <label className={`${secondaryButton} cursor-pointer`} title="Choose the filled template">...<input type="file" hidden accept=".xls,.xlsx,.csv" onChange={e => { const f = e.target.files?.[0]; if (f) void chooseFile(f); e.target.value = ''; }} /></label>
          </div>
          {message && <p role="status" className="mb-2 text-accent-ink">{message}</p>}
          {grid?.rows?.length ? (
            <>
              <div className="max-h-[calc(100vh-17rem)] overflow-auto rounded border border-line">
                <table className="w-full">
                  <thead className={`${tableHead} sticky top-0`}><tr><th className="px-2 py-1.5 text-left">ItemID</th>{shown.map(c => <th key={c} className="px-2 py-1.5 text-left">{c}</th>)}</tr></thead>
                  <tbody className={tableBody}>
                    {grid.rows.map((r, i) => (
                      <tr key={r.ItemID ?? i}>
                        <td className="px-2 py-1 font-mono">{String(r.ItemID ?? '')}</td>
                        {shown.map(c => c === 'ItemName'
                          ? <td key={c} className="px-2 py-1">{String(r[c] ?? '')}</td>
                          : c.toUpperCase() === 'UOMID'
                            ? (
                              <td key={c} className="px-1 py-0.5">
                                <select className={`${field} !py-1`} aria-label={`UOM of ${r.ItemName ?? r.ItemID}`} value={String(r[c] ?? '')} onChange={e => edit(i, c, e.target.value)}>
                                  {!lists.uoms.includes(String(r[c] ?? '')) && <option value={String(r[c] ?? '')}>{String(r[c] ?? '')}</option>}
                                  {lists.uoms.map(u => <option key={u} value={u}>{u}</option>)}
                                </select>
                              </td>
                            )
                            : <td key={c} className="px-1 py-0.5"><input className={`${field} !py-1`} aria-label={`${c} of ${r.ItemName ?? r.ItemID}`} value={String(r[c] ?? '')} onChange={e => edit(i, c, e.target.value)} /></td>)}
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <button className={`${secondaryButton} mt-2`} onClick={() => void downloadXls('Item_Master_Fields', 'Update Item Master Fields', '', shown.map(c => ({ key: c, label: c })), grid.rows)}>Export to Excel</button>
            </>
          ) : <p className="rounded border border-dashed border-line px-3 py-16 text-center text-stone-500">Choose the fields, then Load to Grid (or pick a filled Excel template).</p>}
        </section>
      </div>

      {picking === 'groups' && (
        <MultiSelectDialog wide={false} title="Select [Multiple] Item Groups" rows={lists.groups} rowKey={r => r.code} chosen={filters.groups}
          columns={[{ key: 'code', label: 'ItemGroupCode' }]} pasteBy={[{ label: 'ItemGroupCode', value: r => r.code }]}
          onOk={keys => picked('groups', keys)} onClose={() => picked('groups', filters.groups)} />
      )}
      {picking === 'packages' && (
        <MultiSelectDialog wide={false} title="Select [Multiple] Item Packaging" rows={lists.packages} rowKey={r => r.code} chosen={filters.packages}
          columns={[{ key: 'code', label: 'PKGCode' }, { key: 'litres_per_bottle', label: 'LTRPERBOT', align: 'right', render: r => Number(r.litres_per_bottle ?? 0).toFixed(3) }]}
          pasteBy={[{ label: 'PKGCode', value: r => r.code }]}
          onOk={keys => picked('packages', keys)} onClose={() => picked('packages', filters.packages)} />
      )}
      {picking === 'items' && (
        <MultiSelectDialog wide={false} title="Select [Multiple] Item" rows={lists.items} rowKey={r => r.code} chosen={filters.items}
          columns={[{ key: 'name', label: 'ItemName' }]} pasteBy={[{ label: 'ItemName', value: r => r.name }, { label: 'ItemCode', value: r => r.code }]}
          onOk={keys => picked('items', keys)} onClose={() => picked('items', filters.items)} />
      )}
    </div>
  );
}
