// Label Printing (legacy "Label Printing" / frmUtlBarCodePrint with the ItemBC layout: SELECT ITEMCODE, SHORTNAME FROM
// MSTITEM): pick items and a number of labels each, then print Code 128 item-code labels through the browser's print
// dialog. The desktop sends a .prn file to a label printer; here the label is drawn at the chosen size on a page, so any
// printer the PC has can print it (a label printer takes the size from its driver).

import { useEffect, useMemo, useState } from 'react';
import { Plus, Printer, Trash2 } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { errorText, getItems, } from '../../lib/api';
import { code128Svg } from '../../lib/code128';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const SIZES = [
  { id: '50x25', label: '50 × 25 mm', w: 50, h: 25 }, { id: '38x25', label: '38 × 25 mm', w: 38, h: 25 },
  { id: '75x38', label: '75 × 38 mm', w: 75, h: 38 }, { id: '100x50', label: '100 × 50 mm', w: 100, h: 50 },
];
const esc = (t) => t.replace(/[&<>"]/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[c]);

export default function LabelPrinting() {
  const [items, setItems] = useState([]);
  const [q, setQ] = useState('');
  const [rows, setRows] = useState([]);
  const [size, setSize] = useState('50x25');
  const [withName, setWithName] = useState(true);
  const [withMrp, setWithMrp] = useState(false);

  useEffect(() => { getItems().then(setItems).catch(e => toastStore.add('error', errorText(e))); }, []);
  const byCode = useMemo(() => new Map(items.map(i => [i.code, i])), [items]);
  const matches = useMemo(() => {
    const t = q.trim().toLowerCase();
    return t ? items.filter(i => i.code.toLowerCase().includes(t) || i.name.toLowerCase().includes(t)).slice(0, 8) : [];
  }, [items, q]);

  const add = (code) => {
    setRows(r => (r.some(x => x.code === code) ? r : [...r, { code, copies: 1 }]));
    setQ('');
  };
  const setCopies = (code, copies) => setRows(r => r.map(x => (x.code === code ? { ...x, copies: Math.max(1, Math.min(999, copies || 1)) } : x)));

  const print = async () => {
    if (!rows.length) return toastStore.add('error', 'Add at least one item.');
    const bad = rows.filter(r => !code128Svg(r.code)).map(r => r.code);
    if (bad.length) return toastStore.add('error', `These item codes can't be drawn as a barcode: ${bad.join(', ')}`);
    if (!(await toastStore.defer('ok', `${rows.reduce((n, r) => n + r.copies, 0)} labels will be sent to print when this closes.`, 'Printing cancelled.'))) return;
    const s = SIZES.find(x => x.id === size);
    const labels = rows.flatMap(r => {
      const it = byCode.get(r.code);
      const html = `<div class="l">${withName ? `<div class="n">${esc(it?.short_name || it?.name || r.code)}</div>` : ''}
        <div class="b">${code128Svg(r.code, 40)}</div><div class="c">${esc(r.code)}${withMrp && it ? ` &nbsp; MRP ${money(it.mrp)}` : ''}</div></div>`;
      return Array.from({ length: r.copies }, () => html);
    });
    const w = window.open('', '_blank');
    if (!w) return toastStore.add('error', 'The browser blocked the print window. Allow pop-ups for this site and try again.');
    w.document.write(`<!doctype html><html><head><title>Labels</title><style>
      @page { size: ${s.w}mm ${s.h}mm; margin: 0 }
      body { margin: 0; font-family: Arial, sans-serif }
      .l { width: ${s.w}mm; height: ${s.h}mm; box-sizing: border-box; padding: 1.5mm 2mm; page-break-after: always; display: flex; flex-direction: column; justify-content: center; overflow: hidden }
      .n { font-size: ${s.h >= 38 ? 12 : 8}pt; font-weight: 700; white-space: nowrap; overflow: hidden; text-overflow: ellipsis }
      .b { height: ${Math.round(s.h * 0.45)}mm } .b svg { width: 100%; height: 100% }
      .c { font-size: ${s.h >= 38 ? 10 : 7}pt; text-align: center }
    </style></head><body>${labels.join('')}</body></html>`);
    w.document.close();
    w.focus();
    w.onload = () => w.print();
    setTimeout(() => { try { w.print(); } catch { /* the window was closed */ } }, 500);
  };

  return (
    <div className="mx-auto flex max-w-4xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Label Printing</h1>
          <p className={pageSub}>Print item-code barcode labels (Code 128) with the browser's print dialog.</p>
        </div>
        <button onClick={print} disabled={!rows.length} className={primaryButton}><Printer size={14} />Print labels</button>
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
        <div>
          <label htmlFor="lp-size" className={label}>Label size</label>
          <select id="lp-size" value={size} onChange={e => setSize(e.target.value)} className={field}>
            {SIZES.map(s => <option key={s.id} value={s.id}>{s.label}</option>)}
          </select>
        </div>
        <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" className="accent-accent" checked={withName} onChange={e => setWithName(e.target.checked)} />Item name</label>
        <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" className="accent-accent" checked={withMrp} onChange={e => setWithMrp(e.target.checked)} />MRP</label>
      </div>

      <div className={`${panel} overflow-hidden`}>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-4 py-2.5">Code</th><th className="px-4 py-2.5">Item</th><th className="px-4 py-2.5 text-right">Labels</th><th className="w-12" /></tr></thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={4} className="px-4 py-10 text-center text-stone-500">Search for items above to add them.</td></tr>}
            {rows.map(r => (
              <tr key={r.code}>
                <td className="px-4 py-2 font-medium">{r.code}</td>
                <td className="px-4 py-2">{byCode.get(r.code)?.name ?? ''}</td>
                <td className="px-4 py-2 text-right">
                  <input type="number" min={1} max={999} aria-label={`Labels for ${r.code}`} value={r.copies} onChange={e => setCopies(r.code, Number(e.target.value))}
                    className={`${field} ml-auto w-24 text-right tabular-nums`} />
                </td>
                <td className="px-2 py-2 text-right">
                  <button type="button" aria-label={`Remove ${r.code}`} onClick={() => setRows(x => x.filter(y => y.code !== r.code))} className={`${secondaryButton} !px-2`}><Trash2 size={13} /></button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
