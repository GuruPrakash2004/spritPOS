// Label Printing (legacy "Barcode Printing" / frmUtlBarCodePrint): pick items (one by one or with the "Select [Multiple] Items"
// window, whose Paste button takes a pasted list of item codes or names), then the grid shows what the exe's grid does (Sel,
// No of Labels, Item Code, Item Name, Barcode, UOM, Qty, MRP). The ticked rows are printed through the desktop's own RPTBC*.rpt
// label formats (Crystal Reports bridge, app/rpt_barcode.py): the print window shows the formats found in the report folder
// (a choice only when there is more than one), previews the labels and prints them.

import { useEffect, useMemo, useState } from 'react';
import { ListChecks, Plus, Printer, Trash2 } from 'lucide-react';
import MultiSelectDialog from '../common/MultiSelectDialog';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import ReportPrintModal from '../print/ReportPrintModal';
import { errorText, getBarcodeFormats, getItems, renderBarcodePrint } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import BackToMaster from '../common/BackToMaster';

export default function LabelPrinting() {
  const [items, setItems] = useState([]);
  const [q, setQ] = useState('');
  const [rows, setRows] = useState([]);
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [formats, setFormats] = useState([]);   // the RPTBC*.rpt files in the report folder
  const [picking, setPicking] = useState(false);
  const [printing, setPrinting] = useState(false);

  useEffect(() => { getItems().then(setItems).catch(e => toastStore.add('error', errorText(e)));
    getBarcodeFormats().then(f => setFormats(f.formats)).catch(() => { /* printing says why */ });
  }, []);
  const sortedItems = useMemo(() => [...items].sort((a, b) => a.code.localeCompare(b.code)), [items]);
  const byCode = useMemo(() => new Map(items.map(i => [i.code, i])), [items]);
  const matches = useMemo(() => {
    const t = q.trim().toLowerCase();
    return t ? items.filter(i => i.code.toLowerCase().includes(t) || i.name.toLowerCase().includes(t)).slice(0, 8) : [];
  }, [items, q]);

  // A row as the desktop's grid loads it (loadItems): ticked, 1 label, Qty 1, the item's UOM and MRP (the MRP can be changed here).
  const newRow = (code) => ({ code, sel: true, copies: 1, qty: 1, mrp: Number(byCode.get(code)?.mrp) || 0 });
  const patch = (code, change) => setRows(r => r.map(x => (x.code === code ? { ...x, ...change } : x)));
  const add = (code) => {
    setRows(r => (r.some(x => x.code === code) ? r : [...r, newRow(code)]));
    setQ('');
  };
  // OK in the picker: ticked items stay (keeping their edits), unticked ones go, new ones start at 1 label.
  const applyPicked = (codes) => {
    const want = new Set(codes);
    setRows(r => [...r.filter(x => want.has(x.code)), ...codes.filter(c => !r.some(x => x.code === c)).map(newRow)]);
    setPicking(false);
  };
  const setCopies = (code, copies) => patch(code, { copies: Math.max(1, Math.min(999, copies || 1)) });
  const ticked = rows.filter(r => r.sel);
  const allSel = rows.length > 0 && ticked.length === rows.length;

  const labelItems = ticked.map(r => ({ code: r.code, copies: r.copies, qty: r.qty, mrp: r.mrp, barcode: r.code }));
  const openPrint = () => {
    if (!ticked.length) return toastStore.add('error', rows.length ? 'Tick at least one item.' : 'Add at least one item.');
    if (!formats.length) return toastStore.add('error', 'No barcode format (RPTBC.rpt) is in the report folder.');
    setPrinting(true);
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {confirmDialog}
      {picking && (
        <MultiSelectDialog title="Select [Multiple] Items" rows={sortedItems} rowKey={i => i.code} chosen={rows.map(r => r.code)} onOk={applyPicked} onClose={() => setPicking(false)}
          columns={[{ key: 'code', label: 'ItemCode' }, { key: 'name', label: 'ItemName' }]}
          pasteBy={[{ label: 'ItemCode', value: i => i.code }, { label: 'ItemName', value: i => i.name }]} />
      )}
      {printing && <ReportPrintModal title={`Barcode labels (${ticked.reduce((n, r) => n + r.copies, 0)})`} formats={formats} render={(rpt, printer) => renderBarcodePrint(labelItems, rpt, printer)} onClose={() => setPrinting(false)} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Label Printing</h1>
          <p className={pageSub}>Print barcode labels through the shop's label formats (RPTBC.rpt): preview them first, then print.</p>
        </div>
        <div className="flex flex-wrap gap-2">
          <BackToMaster />
          <button onClick={openPrint} disabled={!ticked.length} className={primaryButton}><Printer size={14} />Preview &amp; print</button>
        </div>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div className="relative min-w-60 flex-1">
          <label htmlFor="lp-search" className={label}>Add an item (code or name)</label>
          <input id="lp-search" role="combobox" aria-expanded={matches.length > 0} aria-controls="lp-list" value={q} onChange={e => setQ(e.target.value)}
            onKeyDown={e => { if (e.key === 'Enter' && matches[0]) { e.preventDefault(); add(matches[0].code); } }} className={field} autoComplete="off" />
          {matches.length > 0 && (
            <ul id="lp-list" role="listbox" className="absolute z-10 mt-1 max-h-56 w-full overflow-auto rounded-lg border border-line bg-surface shadow-pop">
              {matches.map(i => (
                <li key={i.code} role="option" aria-selected="false">
                  <button type="button" onClick={() => add(i.code)} className="flex w-full items-center gap-2 px-3 py-1.5 text-left text-xs hover:bg-subtle">
                    <Plus size={12} /><span className="font-medium">{i.code}</span><span className="truncate text-stone-500">{i.name}</span>
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>
        <button type="button" onClick={() => setPicking(true)} className={secondaryButton}><ListChecks size={14} />Select multiple items</button>
      </div>

      <div className={`${panel} overflow-x-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>
              <th className="w-12 px-3 py-2.5"><input type="checkbox" className="accent-accent" aria-label="Select all rows" checked={allSel} onChange={e => setRows(r => r.map(x => ({ ...x, sel: e.target.checked })))} /> <span className="sr-only">Sel</span></th>
              <th className="px-3 py-2.5 text-right">No of Labels</th><th className="px-3 py-2.5">Item Code</th><th className="px-3 py-2.5">Item Name</th>
              <th className="px-3 py-2.5">Barcode</th><th className="px-3 py-2.5">UOM</th><th className="px-3 py-2.5 text-right">Qty</th><th className="px-3 py-2.5 text-right">MRP</th><th className="w-12" />
            </tr>
          </thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={9} className="px-4 py-10 text-center text-stone-500">Search for items above, or use Select multiple items.</td></tr>}
            {rows.map(r => (
              <tr key={r.code} className={r.sel ? '' : 'text-stone-400'}>
                <td className="px-3 py-1.5"><input type="checkbox" className="accent-accent" aria-label={`Print ${r.code}`} checked={r.sel} onChange={e => patch(r.code, { sel: e.target.checked })} /></td>
                <td className="px-3 py-1.5 text-right">
                  <input type="number" min={1} max={999} aria-label={`Labels for ${r.code}`} value={r.copies} onChange={e => setCopies(r.code, Number(e.target.value))}
                    className={`${field} ml-auto w-20 text-right tabular-nums`} />
                </td>
                <td className="px-3 py-1.5 font-medium">{r.code}</td>
                <td className="px-3 py-1.5">{byCode.get(r.code)?.name ?? ''}</td>
                <td className="px-3 py-1.5 font-mono">{r.code}</td>
                <td className="px-3 py-1.5">{byCode.get(r.code)?.uom ?? ''}</td>
                <td className="px-3 py-1.5 text-right">
                  <input type="number" min={0} aria-label={`Qty for ${r.code}`} value={r.qty} onChange={e => { const qty = Math.max(0, Number(e.target.value) || 0); patch(r.code, { qty, copies: Math.max(1, Math.min(999, qty)) }); }}   // a label for every piece: the count follows Qty (No of Labels can still be changed after)
                    className={`${field} ml-auto w-20 text-right tabular-nums`} />
                </td>
                <td className="px-3 py-1.5 text-right">
                  <input type="number" min={0} step="0.01" aria-label={`MRP for ${r.code}`} value={r.mrp} onChange={e => patch(r.code, { mrp: Math.max(0, Number(e.target.value) || 0) })}
                    className={`${field} ml-auto w-28 bg-yellow-50 text-right tabular-nums dark:bg-yellow-900/20`} />
                </td>
                <td className="px-2 py-1.5 text-right">
                  <button type="button" aria-label={`Remove ${r.code}`} onClick={async () => { if (await confirm(`Are you sure you want to remove '${r.code}' from the labels?`)) setRows(x => x.filter(y => y.code !== r.code)); }} className={`${secondaryButton} !px-2`}><Trash2 size={13} /></button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
