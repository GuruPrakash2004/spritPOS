// The Stock Balance report's result window: pressing View on the screen opens it over the page, big, with the grid and its buttons (frmRptStockBal's
// grid and Col Select / Col Freez / Export / Print). One grid: the Item Group Name is merged down a group's rows as in the exe. Column widths are
// adjustable: drag the edge of a heading to shrink or open a column, double-click the edge to put them all back.
//   result      { columns, rows, brand_required } from GET /reports/stock-balance/view (null while loading or after an error)
//   running     the report is being read
//   error       the server's message when it failed
//   filters     every heading has the exe's filter button (vsfView.AllowFiltering): a value tick list and a text/number condition (ColumnFilterPopup)
//   rate        the Rate radio button chosen on the screen: 'pur' | 'fifo' | 'mrp' | 'war' (shown beside the view name)
//   view        'closing' | 'detailed' | 'package' | 'group'
//   layoutKey   where the chosen columns are remembered: the exe keeps them per user for each view / rate / value-or-litres combination
//               (UtlSelCols, FormTag StkBalViewCPV ...), here the browser keeps them per combination
//   title, subtitle   the window title and the period line
//   onPrintZero  opens Print Zero Stock Items (the active items whose stock is zero); no button when missing
//   formats, renderPrint   Print opens the Crystal print window (the shop's RptStkBal*.rpt formats):
//               `formats` are the .rpt names, `renderPrint(rpt, printer)` asks the server for the PDF / the print

import { useLayoutEffect, useRef, useState } from 'react';
import { Columns3, FileDown, Filter, Printer, Snowflake } from 'lucide-react';
import Modal from '../common/Modal';
import ColumnPicker from '../common/ColumnPicker';
import ColumnFilterPopup from './ColumnFilterPopup';
import { passes } from '../../lib/columnFilter';
import ReportPrintModal from '../print/ReportPrintModal';
import { InlineError } from '../common/FormErrorSummary';
import { SkeletonTableRows } from '../common/Skeleton';
import { downloadXls } from '../../lib/xlsExport';
import { groupLabel } from '../../lib/formatters';
import { exportButton, money, secondaryButton, tableBody, tableHead, tableRow } from '../../lib/styles';

const VIEW_NAMES = { closing: 'Closing Balance Only', detailed: 'Closing Balance With Detailed', package: 'Package wise Balance', group: 'Group wise Summary Balance' };
const RATE_NAMES = { pur: 'Last Purchase Rate', fifo: 'FIFO', mrp: 'Sales Rate', war: 'Wt.Avg Rate' };
const MIN_WIDTH = 48;
const isGrand = (r) => !!r.subtotal && Object.values(r).includes('Grand Total');   // the report's closing total row
const readLayout = (key) => { try { return JSON.parse(localStorage.getItem(key) || 'null'); } catch { return null; } };
const writeLayout = (key, value) => { try { if (value) localStorage.setItem(key, JSON.stringify(value)); else localStorage.removeItem(key); } catch { /* the layout just is not remembered */ } };
const cell = (c, v) => (v === undefined || v === null || v === '' ? '' : c.kind === 'money' ? money(Number(v)) : c.kind === 'number' ? Number(Number(v).toFixed(3)) : String(v));

export default function StockBalanceResult({ result, running, error, view, rate, layoutKey, title, subtitle, formats, renderPrint, onPrintZero, onClose }) {
  const [layout, setLayout] = useState(() => readLayout(layoutKey));   // { order, visible } or null (every column, as served)
  const [pickingCols, setPickingCols] = useState(false);
  const [printing, setPrinting] = useState(false);
  const [note, setNote] = useState('');
  const [sel, setSel] = useState(null);   // the clicked cell { r, c }: Col Freeze keeps the columns up to it in view
  const [freezeTo, setFreezeTo] = useState(-1);   // columns 0..freezeTo stay at the left while scrolling sideways; -1 = not frozen
  const [lefts, setLefts] = useState([]);
  const [widths, setWidths] = useState({});   // column key -> px once a column has been dragged; {} = the browser sizes them
  const headRef = useRef(null);
  const wrapRef = useRef(null);
  const [filters, setFilters] = useState({});   // column key -> { op, text, values } (ColumnFilterPopup); a filtered grid hides the total rows
  const [filtering, setFiltering] = useState(null);   // { key, left, top }: the open filter window
  const [seen, setSeen] = useState(result);
  if (seen !== result) { setSeen(result); setFilters({}); setFiltering(null); setSel(null); }   // a new View starts unfiltered

  const allCols = result ? result.columns.filter(c => !c.export_only && (!c.brand || result.brand_required)) : [];
  // The column order is the exe's template (the server's); Col Select only ticks which columns show.
  const shownCols = (() => {
    if (!layout?.visible) return allCols;
    return allCols.filter(c => layout.visible.includes(c.key) || !(layout.order ?? []).includes(c.key));
  })();
  const baseRows = result?.rows ?? [];
  const shownText = (c, r) => (c.key === 'group' ? groupLabel(r.group) : String(cell(c, r[c.key])));
  const filterKeys = Object.keys(filters);
  const rows = filterKeys.length
    ? baseRows.filter(r => !r.subtotal && filterKeys.every(k => { const c = allCols.find(x => x.key === k); return !c || passes(filters[k], !!c.kind, shownText(c, r)); }))
    : baseRows;
  const openFilter = (e, c) => {
    e.stopPropagation();
    const b = e.currentTarget.getBoundingClientRect(), w = wrapRef.current.getBoundingClientRect();
    setFiltering({ key: c.key, left: Math.max(0, Math.min(b.left - w.left - 200, w.width - 300)), top: b.bottom - w.top + 2 });
  };
  const filterCol = filtering && allCols.find(c => c.key === filtering.key);
  // The exe merges the Item Group Name cells down a group's rows: the name is drawn once, spanning the group's items (a total row ends the span).
  const spans = rows.map((r, i, all) => {
    if (r.subtotal || !r.group) return 1;
    if (i > 0 && !all[i - 1].subtotal && all[i - 1].group === r.group) return 0;
    let n = 1;
    while (all[i + n] && !all[i + n].subtotal && all[i + n].group === r.group) n += 1;
    return n;
  });

  // The frozen columns stick at the left: each one's offset is the width of the columns before it.
  useLayoutEffect(() => {
    if (freezeTo < 0 || !headRef.current) { setLefts([]); return; }
    let x = 0;
    setLefts([...headRef.current.children].map(th => { const left = x; x += th.offsetWidth; return left; }));
  }, [freezeTo, result, layout, running, widths]);

  // Drag the edge of a heading: the first drag fixes every column at its current width, then only the dragged one changes.
  const startResize = (e, key) => {
    e.preventDefault();
    e.stopPropagation();
    const measured = [...headRef.current.children].map(th => th.offsetWidth);
    const start = Object.keys(widths).length ? widths : Object.fromEntries(shownCols.map((c, i) => [c.key, measured[i]]));
    const x0 = e.clientX;
    const w0 = start[key];
    const move = (ev) => setWidths({ ...start, [key]: Math.max(MIN_WIDTH, w0 + ev.clientX - x0) });
    const up = () => { window.removeEventListener('mousemove', move); window.removeEventListener('mouseup', up); };
    window.addEventListener('mousemove', move);
    window.addEventListener('mouseup', up);
  };
  const sized = Object.keys(widths).length > 0;
  const tableWidth = sized ? shownCols.reduce((n, c) => n + (widths[c.key] ?? 120), 0) : undefined;

  const colSelect = () => {
    if (!rows.length) { setNote('Load the report with View first, then select columns.'); return; }
    setNote('');
    setPickingCols(true);
  };
  const colFreeze = () => setFreezeTo(f => (f >= 0 ? -1 : Math.max(0, sel?.c ?? 0)));
  // The exe's Excel file carries the Item Brand Name column and the hidden ItemID and Bottle PerCase columns even when the grid does not show them.
  const exportCols = (result?.columns ?? []).filter(c => c.export_only || c.brand || shownCols.some(s => s.key === c.key))
    .map(c => ({ key: c.key, label: c.label, hidden: !!c.export_only }));
  const exportXls = () => downloadXls('Stock_Balance', `Stock Balance Report - ${viewName}${rateName ? ` - ${rateName}` : ''}`, subtitle, exportCols, rows, [],
    { rowStyles: rows.map(r => (isGrand(r) ? 7 : r.subtotal ? 6 : 0)) });

  const disabled = !rows.length;
  const viewName = VIEW_NAMES[view] || '';
  const rateName = RATE_NAMES[rate] || '';
  return (
    <Modal title={`${title}${viewName ? ` - ${viewName}` : ''}  ${subtitle}`} onClose={onClose} wide="xl"
      footer={<>
        <span className="mr-auto self-center text-xs text-stone-500">Total Records: {rows.filter(r => !r.subtotal).length}</span>
        <button type="button" onClick={onClose} className={secondaryButton}>Close</button>
      </>}>
      <div className="mb-3 flex flex-wrap items-center gap-2">
        {viewName && <span className="rounded-lg bg-[#64283A] px-3 py-1.5 text-xs font-semibold text-white dark:bg-[#D4A847] dark:text-[#141414]" title="The view chosen on the Stock Balance screen">View: {viewName}</span>}
        {rateName && <span className="rounded-lg bg-[#B08A45] px-3 py-1.5 text-xs font-semibold text-[#1F0A00] dark:bg-[#4A3D18] dark:text-[#F5E1B0]" title="The rate chosen on the Stock Balance screen">Rate: {rateName}</span>}
        <button type="button" className={exportButton} disabled={disabled} onClick={exportXls}><FileDown size={14} />Export Excel</button>
        <button type="button" className={exportButton} disabled={disabled} onClick={colSelect} title="Choose the columns to show"><Columns3 size={14} />Col Select</button>
        <button type="button" className={exportButton} disabled={disabled} onClick={colFreeze} aria-pressed={freezeTo >= 0} title="Keep the columns up to the clicked cell in view while scrolling sideways"><Snowflake size={14} />{freezeTo >= 0 ? 'Col UnFreeze' : 'Col Freeze'}</button>
        <button type="button" className={exportButton} disabled={!formats?.length} onClick={() => setPrinting(true)} title="Print through the shop's Stock Balance .rpt formats"><Printer size={14} />Print</button>
        {onPrintZero && <button type="button" className={exportButton} onClick={onPrintZero} title="Print the active items whose stock is zero"><Printer size={14} />Print Zero Stock Items</button>}
      </div>
      {note && <div className="mb-2"><InlineError message={note} /></div>}
      <div ref={wrapRef} className="relative">
      <div className="max-h-[calc(92vh-13rem)] overflow-auto rounded-lg border border-line">
        <table className="text-[13px] leading-tight text-[#1F0A00] dark:text-white" style={sized ? { tableLayout: 'fixed', width: tableWidth } : { width: '100%' }}>
          <thead className={`${tableHead} !bg-[#64283A] !text-white dark:!bg-[#D4A847] dark:!text-[#141414]`}><tr ref={headRef}>{shownCols.map((c, i) => (
            <th key={c.key} onClick={() => setSel(s => ({ r: s?.r ?? 0, c: i }))} style={{ ...(i <= freezeTo ? { left: lefts[i] ?? 0 } : {}), ...(sized ? { width: widths[c.key] } : {}) }}
              className={`sticky top-0 bg-[#64283A] text-white dark:bg-[#D4A847] dark:text-[#141414] px-3 py-1 text-[12px] font-normal ${c.kind ? 'text-right' : 'text-left'} ${i <= freezeTo ? 'z-[4]' : 'z-[3]'}`}>
              {c.label}
              <button type="button" aria-label={`Filter ${c.label}`} title="Filter this column" onClick={e => openFilter(e, c)}
                className={`ml-1.5 inline-flex rounded p-0.5 align-middle hover:bg-white/20 ${filters[c.key] ? 'bg-[#FF8C1A] text-[#1F0A00]' : 'opacity-80'}`}><Filter size={12} /></button>
              <span role="separator" aria-orientation="vertical" aria-label={`Resize ${c.label}`} title="Drag to resize, double-click to reset" onMouseDown={e => startResize(e, c.key)}
                onDoubleClick={() => setWidths({})} onClick={e => e.stopPropagation()}
                className="absolute right-0 top-0 h-full w-2 cursor-col-resize select-none border-r border-line hover:bg-accent/30" />
            </th>))}</tr></thead>
          <tbody className={tableBody}>
            {error && <tr><td className="px-4 py-8 text-center text-red-600 dark:text-red-400">{error}</td></tr>}
            {running && <SkeletonTableRows rows={10} cols={Math.max(shownCols.length, 6)} />}
            {!running && !error && result && !rows.length && <tr><td colSpan={Math.max(shownCols.length, 1)} className="px-4 py-8 text-center text-stone-500">No data for these options.</td></tr>}
            {!running && rows.map((r, i) => (
              <tr key={i} aria-selected={sel?.r === i} className={`${sel?.r === i || r.subtotal ? '' : tableRow} ${sel?.r === i ? 'bg-[#FF8C1A] text-[#1F0A00] hover:bg-[#FF8C1A] dark:hover:bg-[#FF8C1A]' : isGrand(r) ? 'bg-[#64283A] font-bold text-white hover:bg-[#64283A] dark:bg-[#D4A847] dark:text-[#141414] dark:hover:bg-[#D4A847]' : r.subtotal ? 'bg-[#F5E1B0] font-semibold text-[#3A2A00] hover:bg-[#F5E1B0] dark:bg-[#4A3D18] dark:text-[#F5E1B0] dark:hover:bg-[#4A3D18]' : ''}`}>
                {shownCols.map((c, ci) => {
                  const frozen = ci <= freezeTo;
                  const on = sel?.r === i;
                  const pick = () => setSel({ r: i, c: ci });
                  const look = `${sized ? 'truncate' : ''} ${frozen ? `sticky z-[2] ${on ? 'bg-[#FF8C1A]' : isGrand(r) ? 'bg-[#64283A] dark:bg-[#D4A847]' : r.subtotal ? 'bg-[#F5E1B0] dark:bg-[#4A3D18]' : 'bg-surface'}` : ''}`;
                  const style = frozen ? { left: lefts[ci] ?? 0 } : undefined;
                  if (c.key === 'group' && spans[i] !== 1) {
                    return spans[i] === 0 ? null
                      : <td key={c.key} rowSpan={spans[i]} onClick={pick} style={style} className={`border-r border-line bg-subtle px-3 py-0.5 align-middle font-medium ${look}`}>{groupLabel(r.group)}</td>;
                  }
                  return <td key={c.key} onClick={pick} style={style} title={sized ? String(cell(c, r[c.key])) : undefined} className={`px-3 ${isGrand(r) ? 'py-3 text-sm' : 'py-0.5'} ${c.kind ? 'text-right tabular-nums' : ''} ${look}`}>{cell(c, r[c.key])}</td>;
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {filterCol && (
        <ColumnFilterPopup key={filterCol.key} label={filterCol.label} isNumber={!!filterCol.kind}
          distinct={[...new Set(baseRows.filter(r => !r.subtotal).map(r => shownText(filterCol, r)))].sort((a, b) => a.localeCompare(b, undefined, { numeric: true }))}
          filter={filters[filterCol.key]} left={filtering.left} top={filtering.top} onClose={() => setFiltering(null)}
          onApply={f => { setFilters(prev => { const n = { ...prev }; if (f) n[filterCol.key] = f; else delete n[filterCol.key]; return n; }); setSel(null); setFiltering(null); }} />
      )}
      </div>
      {pickingCols && (
        <ColumnPicker reorder={false} columns={allCols.map(c => ({ key: c.key, label: c.label }))} order={layout?.order ?? allCols.map(c => c.key)}
          visible={layout?.visible ?? allCols.map(c => c.key)} defaults={allCols.map(c => c.key)}
          onApply={({ order, visible }) => {
            const asServed = visible.length === allCols.length;
            writeLayout(layoutKey, asServed ? null : { order, visible });
            setLayout(asServed ? null : { order, visible });
            setPickingCols(false);
            setFreezeTo(-1);
            setWidths({});
          }}
          onClose={() => setPickingCols(false)} />
      )}
      {printing && <ReportPrintModal title={`${title} ${subtitle}`} formats={formats} render={renderPrint} directOnly onClose={() => setPrinting(false)} />}
    </Modal>
  );
}
