// The "Listing of …" window opened by the List button of every master and document screen (the exe's ViewForm / frmUtlMstView / frmUtlTrnView).
//
//   <ListModal title="Item Master" subtitle="Listing of Item Master" columns={[{ key, label, kind?, value? }]} defaultKeys={['code','name']}
//              rows={rows} rowKey={r => r.code} onOpen={row => …} onRefresh={…} storageKey="item-master" exportName="Item Master" onClose={…} />
//
// - Global search over every column, a Refresh button, sortable headers and a filter box under each header ("Use column headers for
//   filtering and sorting"), "Total Records: shown / all".
// - Click a row to select it; double-click (or Enter, or Open Selected) loads it through `onOpen(row)`. The window closes after that unless
//   "Do NOT hide after loading the record" is ticked (remembered in this browser).
// - Select Columns (Alt+S) opens ColumnPicker; the chosen columns and their order are remembered per `storageKey`. Export to Excel (Alt+E)
//   writes the shown rows and columns. Freeze Columns (Alt+F) keeps the first column in view while scrolling sideways. Transpose (Alt+T) shows
//   the selected record as a field / value list. `renderDetail(row)` adds View Detail Grid (Alt+D) for documents (their lines).
// - `onEdit(row)` adds Edit Selected: the window closes and the record is handed to the screen to change (`canEditRow(row)` says which rows may be).
// - Column `kind`: text (default) | number | money | bool | date. `value(row)` overrides row[key].

import { useEffect, useId, useMemo, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { ArrowDown, ArrowUp, ArrowUpDown, Columns3, FileSpreadsheet, MousePointerClick, RefreshCw, Rows3, Search, Snowflake, X, Table2 } from 'lucide-react';
import ColumnPicker from './ColumnPicker';
import { useDialogKeys } from './useDialogKeys';
import { downloadXls } from '../../lib/xlsExport';
import { badge, money, primaryButton, secondaryButton } from '../../lib/styles';

const NUMERIC = new Set(['number', 'money']);
const pref = (key) => `vlqms_list_${key}`;
const read = (key, fallback) => { try { const v = JSON.parse(localStorage.getItem(pref(key)) ?? 'null'); return v ?? fallback; } catch { return fallback; } };
const write = (key, value) => { try { localStorage.setItem(pref(key), JSON.stringify(value)); } catch { /* not remembered */ } };

const text = (col, row) => {
  const v = col.value ? col.value(row) : row[col.key];
  if (v === null || v === undefined) return '';
  if (col.kind === 'bool') return v ? 'Yes' : 'No';
  if (col.kind === 'money') return v === '' ? '' : money(v);
  return String(v);
};
const raw = (col, row) => (col.value ? col.value(row) : row[col.key]);

export default function ListModal({ title, subtitle, columns, defaultKeys, rows, rowKey, onOpen, onRefresh, storageKey, exportName, onClose, loading = false, error = '', renderDetail, openLabel = 'Open Selected', editLabel = 'Edit Selected', onEdit, canEditRow, toolbar, extraActions = [] }) {
  const titleId = useId();
  const ref = useRef(null);
  const searchRef = useRef(null);
  const [pickerOpen, setPickerOpen] = useState(false);
  // Only the Close button in the footer and the X in the header's top-right corner close the list: not Esc, not a click outside (so a stray
  // key or click never throws away the search and sort in progress). useDialogKeys still moves focus in, keeps Tab inside and gives focus
  // back; the Select Columns window above it has its own Esc.
  useDialogKeys(ref, () => {});
  const known = useMemo(() => new Map(columns.map(c => [c.key, c])), [columns]);
  const base = useMemo(() => (defaultKeys?.length ? defaultKeys : columns.map(c => c.key)).filter(k => known.has(k)), [defaultKeys, columns, known]);

  const saved = useMemo(() => read(`${storageKey}_cols`, null), [storageKey]);
  const [order, setOrder] = useState(() => saved?.order ?? columns.map(c => c.key));
  const [visible, setVisible] = useState(() => (saved?.visible ?? base).filter(k => known.has(k)));
  const [search, setSearch] = useState('');
  const [filters, setFilters] = useState({});
  const [sort, setSort] = useState({ key: '', dir: 1 });
  const [selected, setSelected] = useState('');
  const [keepOpen, setKeepOpen] = useState(() => read('keep_open', false));
  const [frozen, setFrozen] = useState(() => read(`${storageKey}_frozen`, false));
  const [flip, setFlip] = useState(false);
  const [detail, setDetail] = useState(false);
  // Column widths in px, set by dragging the right edge of a header (double-click an edge to reset all); remembered per list.
  const [widths, setWidths] = useState(() => read(`${storageKey}_widths`, {}));
  const sized = Object.keys(widths).length > 0;
  const startResize = (e, key) => {
    e.preventDefault(); e.stopPropagation();
    const snap = {};
    e.currentTarget.closest('tr').querySelectorAll('th[data-col]').forEach(th => { snap[th.dataset.col] = th.offsetWidth; });
    const startX = e.clientX, startW = snap[key];
    let latest = snap;
    const move = (ev) => { latest = { ...snap, [key]: Math.max(40, startW + ev.clientX - startX) }; setWidths(latest); };
    const up = () => { window.removeEventListener('pointermove', move); window.removeEventListener('pointerup', up); write(`${storageKey}_widths`, latest); };
    window.addEventListener('pointermove', move);
    window.addEventListener('pointerup', up);
  };
  const resetWidths = () => { setWidths({}); write(`${storageKey}_widths`, {}); };

  const cols = useMemo(() => {
    const ordered = [...order.filter(k => known.has(k)), ...columns.map(c => c.key).filter(k => !order.includes(k))];
    const keys = ordered.filter(k => visible.includes(k));
    return (keys.length ? keys : base.length ? base : ordered.slice(0, 1)).map(k => known.get(k));
  }, [order, visible, columns, known, base]);

  const shown = useMemo(() => {
    const q = search.trim().toLowerCase();
    let list = rows.filter(r => {
      if (q && !columns.some(c => text(c, r).toLowerCase().includes(q))) return false;
      return cols.every(c => { const f = (filters[c.key] ?? '').trim().toLowerCase(); return !f || text(c, r).toLowerCase().includes(f); });
    });
    if (sort.key) {
      const c = known.get(sort.key);
      list = [...list].sort((a, b) => {
        const x = raw(c, a), y = raw(c, b);
        const bothNumbers = NUMERIC.has(c.kind) || (x !== '' && y !== '' && !isNaN(Number(x)) && !isNaN(Number(y)));
        return (bothNumbers ? Number(x ?? 0) - Number(y ?? 0) : String(x ?? '').localeCompare(String(y ?? ''), undefined, { numeric: true, sensitivity: 'base' })) * sort.dir;
      });
    }
    return list;
  }, [rows, search, filters, sort, cols, columns, known]);

  const current = shown.find(r => String(rowKey(r)) === selected) ?? null;
  useEffect(() => { searchRef.current?.focus(); }, []);

  const open = (row = current) => {
    if (!row) return;
    onOpen(row);
    if (!keepOpen) onClose();
  };
  const sortBy = (key) => setSort(s => (s.key === key ? (s.dir === 1 ? { key, dir: -1 } : { key: '', dir: 1 }) : { key, dir: 1 }));
  const applyColumns = ({ order: o, visible: v }) => { setOrder(o); setVisible(v); write(`${storageKey}_cols`, { order: o, visible: v }); setPickerOpen(false); };
  const toggleKeep = (on) => { setKeepOpen(on); write('keep_open', on); };
  const toggleFrozen = () => setFrozen(f => { write(`${storageKey}_frozen`, !f); return !f; });
  const exportXls = () => downloadXls(exportName || title, title, subtitle || `Listing of ${title}`, cols.map(c => ({ key: c.key, label: c.label })),
    shown.map(r => Object.fromEntries(cols.map(c => [c.key, NUMERIC.has(c.kind) && raw(c, r) !== '' && raw(c, r) != null && !isNaN(Number(raw(c, r))) ? Number(raw(c, r)) : text(c, r)]))));

  const step = (d) => {
    if (!shown.length) return;
    const at = current ? shown.indexOf(current) : -1;
    const next = shown[Math.max(0, Math.min(shown.length - 1, at + d))];
    setSelected(String(rowKey(next)));
    requestAnimationFrame(() => ref.current?.querySelector('[aria-selected="true"]')?.scrollIntoView?.({ block: 'nearest' }));
  };
  const onKeyDown = (e) => {
    if (pickerOpen) return;
    if (e.altKey) {
      const k = e.key.toLowerCase();
      const run = { s: () => setPickerOpen(true), e: exportXls, f: toggleFrozen, t: () => setFlip(v => !v), d: () => renderDetail && setDetail(v => !v) }[k];
      if (run) { e.preventDefault(); run(); }
      return;
    }
    if (e.key === 'ArrowDown') { e.preventDefault(); step(1); }
    else if (e.key === 'ArrowUp') { e.preventDefault(); step(-1); }
    else if (e.key === 'Enter' && current && e.target.tagName !== 'BUTTON') { e.preventDefault(); open(); }
  };

  const footerButton = 'inline-flex items-center gap-2 rounded-full border border-[#E3D9CD] bg-white px-3 py-1 text-xs font-semibold text-[#29231F] transition hover:bg-[#F3EEE6] aria-pressed:border-[#64283A] aria-pressed:bg-[#64283A]/8 dark:border-[#2A2A2A] dark:bg-transparent dark:text-stone-100 dark:hover:bg-[#1C1C1C] dark:aria-pressed:border-[#D4A847] dark:aria-pressed:bg-[#D4A847]/10';

  return createPortal(
    <div className="modal-backdrop fixed inset-0 z-[900] flex items-center justify-center bg-black/65 p-3 dark:bg-black/80" >
      <div ref={ref} role="dialog" aria-modal="true" aria-labelledby={titleId} onClick={e => e.stopPropagation()} onKeyDown={onKeyDown}
        className="animate-slideUp flex h-[min(92vh,860px)] w-full max-w-[1280px] flex-col overflow-hidden rounded-2xl border border-[#E3D9CD] bg-white text-[#29231F] shadow-2xl dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100">
        <header className="relative bg-[#64283A] px-12 py-2 text-center text-white dark:bg-[#0F0F0F] dark:text-[#F5D88A]">
          <h3 id={titleId} className="text-base font-bold uppercase tracking-wide">{title}</h3>
          <p className="text-xs opacity-80">{subtitle || `Listing of ${title}`}</p>
          <button type="button" onClick={onClose} tabIndex={-1} aria-label="Close list" title="Close"
            className="absolute right-3 top-3 inline-flex h-8 w-8 items-center justify-center rounded-lg text-white/80 transition hover:bg-white/15 hover:text-white active:scale-[0.96] dark:text-[#F5D88A]/80 dark:hover:bg-white/10 dark:hover:text-[#F5D88A]"><X size={18} /></button>
        </header>

        <div className="flex flex-wrap items-center gap-3 border-b border-[#EFE9DF] px-4 py-2 dark:border-[#2A2A2A]">
          <div className="relative min-w-60 max-w-[640px] flex-1">
            <Search size={15} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
            <input ref={searchRef} value={search} onChange={e => setSearch(e.target.value)} placeholder="Global search across all columns…" aria-label="Global search across all columns"
              className="w-full rounded-xl border border-[#64283A]/70 bg-white py-1.5 pl-9 pr-3 text-xs outline-none transition placeholder:text-stone-400 focus:ring-[3px] focus:ring-[#64283A]/10 dark:border-[#D4A847]/50 dark:bg-transparent dark:focus:ring-[#D4A847]/10" />
          </div>
          {toolbar}
          {onRefresh && <button type="button" onClick={onRefresh} className="inline-flex h-8 w-8 items-center justify-center rounded-full border border-[#E3D9CD] transition hover:bg-[#F3EEE6] dark:border-[#2A2A2A] dark:hover:bg-[#1C1C1C]" aria-label="Refresh" title="Refresh"><RefreshCw size={15} className={loading ? 'animate-spin' : ''} /></button>}
          <span className="ml-auto inline-flex items-center gap-1.5 text-xs text-stone-500"><MousePointerClick size={13} />Double-click row to load</span>
        </div>

        <div className="min-h-0 flex-1 overflow-auto">
          {error && <p role="alert" className="p-4 text-xs text-red-600 dark:text-red-400">{error}</p>}
          {flip ? (
            <div className="p-6">
              {!current ? <p className="text-sm text-stone-500">Select a record to see it as a list of fields.</p> : (
                <table className="w-full max-w-3xl text-xs"><tbody>
                  {columns.map(c => <tr key={c.key} className="border-b border-[#EFE9DF] dark:border-[#1F1F1F]"><th scope="row" className="w-1/3 py-2 pr-4 text-left font-semibold text-[#70675F] dark:text-stone-400">{c.label}</th><td className="py-2">{text(c, current) || <span className="text-stone-400">—</span>}</td></tr>)}
                </tbody></table>
              )}
            </div>
          ) : (
            <table className="text-xs" style={sized ? { tableLayout: 'fixed', width: cols.reduce((s, c) => s + (widths[c.key] ?? 150), 0) } : { minWidth: '60%' }}>
              {sized && <colgroup>{cols.map(c => <col key={c.key} style={{ width: widths[c.key] ?? 150 }} />)}</colgroup>}
              <thead className="sticky top-0 z-[2] bg-[#F7F4EC] dark:bg-[#181818]">
                <tr>
                  {cols.map((c, i) => {
                    const on = sort.key === c.key;
                    return (
                      <th key={c.key} data-col={c.key} scope="col" aria-sort={on ? (sort.dir === 1 ? 'ascending' : 'descending') : 'none'}
                        className={`${frozen && i === 0 ? '' : 'relative '}overflow-hidden border-b border-r border-[#E3DBCB] px-2.5 pt-1.5 text-left text-[10px] font-bold uppercase tracking-wide text-[#64283A] dark:border-[#2A2A2A] dark:text-[#D4A847] ${NUMERIC.has(c.kind) ? 'text-right' : ''} ${frozen && i === 0 ? 'sticky left-0 z-[3] bg-[#F7F4EC] dark:bg-[#181818]' : ''}`}>
                        <button type="button" onClick={() => sortBy(c.key)} className="flex w-full items-center justify-between gap-3 uppercase" title={`Sort by ${c.label}`} aria-label={`Sort by ${c.label}`}>
                          <span>{c.label}</span>
                          {on ? (sort.dir === 1 ? <ArrowUp size={12} /> : <ArrowDown size={12} />) : <ArrowUpDown size={12} className="text-stone-400" />}
                        </button>
                        <span role="separator" aria-orientation="vertical" aria-label={`Resize ${c.label} column`} title="Drag to resize (double-click to reset all)"
                          onPointerDown={e => startResize(e, c.key)} onDoubleClick={resetWidths}
                          className="absolute right-0 top-0 z-[4] h-full w-2 cursor-col-resize touch-none hover:bg-[#64283A]/30 dark:hover:bg-[#D4A847]/40" />
                      </th>
                    );
                  })}
                </tr>
                <tr>
                  {cols.map((c, i) => (
                    <th key={c.key} className={`border-b border-r border-[#E3DBCB] px-2 pb-1.5 pt-1 dark:border-[#2A2A2A] ${frozen && i === 0 ? 'sticky left-0 z-[3] bg-[#F7F4EC] dark:bg-[#181818]' : ''}`}>
                      <input value={filters[c.key] ?? ''} onChange={e => setFilters(f => ({ ...f, [c.key]: e.target.value }))} placeholder="Filter…" aria-label={`Filter ${c.label}`}
                        className={`w-full ${sized ? 'min-w-0' : 'min-w-24'} rounded-md border border-[#E3D9CD] bg-white px-2 py-1 text-[11px] font-normal normal-case outline-none focus:border-[#64283A] dark:border-[#2A2A2A] dark:bg-transparent dark:focus:border-[#D4A847]`} />
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {shown.map((r, n) => {
                  const k = String(rowKey(r));
                  const on = k === selected;
                  return (
                    <tr key={k} role="row" aria-selected={on} onClick={() => setSelected(k)} onDoubleClick={() => open(r)}
                      className={`cursor-pointer border-b border-[#EFE9DF] transition-colors dark:border-[#1F1F1F] ${on ? 'bg-[#EBC9D3] font-semibold text-[#3A1622] dark:bg-[#D4A847]/30 dark:text-white' : n % 2 ? 'bg-[#FBF8F2] hover:bg-[#F5EFE2] dark:bg-[#171717] dark:hover:bg-[#1C1C1C]' : 'hover:bg-[#F5EFE2] dark:hover:bg-[#1C1C1C]'}`}>
                      {cols.map((c, i) => (
                        <td key={c.key} className={`whitespace-nowrap ${sized ? 'overflow-hidden text-ellipsis' : ''} px-2.5 py-1 leading-tight${NUMERIC.has(c.kind) ? 'text-right tabular-nums' : ''} ${on && i === 0 ? 'shadow-[inset_4px_0_0_#64283A] dark:shadow-[inset_4px_0_0_#D4A847]' : ''} ${frozen && i === 0 ? `sticky left-0 ${on ? 'bg-[#EBC9D3] dark:bg-[#4A3D18]' : 'bg-white dark:bg-[#141414]'}` : ''}`}>
                          {c.kind === 'bool' ? <span className={badge(raw(c, r) ? 'success' : 'neutral')}>{text(c, r)}</span> : text(c, r)}
                        </td>
                      ))}
                    </tr>
                  );
                })}
                {!loading && !shown.length && <tr><td colSpan={cols.length} className="px-4 py-16 text-center text-stone-500">{rows.length ? 'No record matches the search or the filters.' : 'No records.'}</td></tr>}
                {loading && !rows.length && <tr><td colSpan={cols.length} className="px-4 py-16 text-center text-stone-500">Loading…</td></tr>}
              </tbody>
            </table>
          )}
          {detail && renderDetail && (
            <div className="border-t border-[#E3DBCB] bg-[#FBF8F2] p-4 text-xs dark:border-[#2A2A2A] dark:bg-[#171717]">
              {current ? renderDetail(current) : <p className="text-stone-500">Select a document to see its lines.</p>}
            </div>
          )}
        </div>

        <footer className="flex flex-wrap items-center gap-2 border-t border-[#EFE9DF] px-4 py-2 dark:border-[#2A2A2A]">
          <span className="rounded-xl border border-[#E3D9CD] px-3 py-1 text-xs font-bold dark:border-[#2A2A2A]" aria-live="polite">Total Records: {shown.length} / {rows.length}</span>
          <label className="inline-flex cursor-pointer items-center gap-2 text-xs">
            <input type="checkbox" checked={keepOpen} onChange={e => toggleKeep(e.target.checked)} className="h-4 w-4 accent-[#64283A]" />Do NOT hide after loading the record.
          </label>
          <div className="ml-auto flex flex-wrap items-center gap-2">
            <button type="button" onClick={() => setPickerOpen(true)} className={footerButton} title="Select Columns (Alt+S)"><Columns3 size={14} />Select Columns</button>
            <button type="button" onClick={toggleFrozen} aria-pressed={frozen} className={footerButton} title="Freeze Columns (Alt+F)"><Snowflake size={14} />{frozen ? 'UnFreeze Columns' : 'Freeze Columns'}</button>
            <button type="button" onClick={() => setFlip(v => !v)} aria-pressed={flip} className={footerButton} title="Transpose (Alt+T)"><Rows3 size={14} />Transpose</button>
            {renderDetail && <button type="button" onClick={() => setDetail(v => !v)} aria-pressed={detail} className={footerButton} title="View Detail Grid (Alt+D)"><Table2 size={14} />View Detail Grid</button>}
            <button type="button" onClick={exportXls} disabled={!shown.length} className={`${footerButton} disabled:opacity-45`} title="Export to Excel (Alt+E)"><FileSpreadsheet size={14} />Export to Excel</button>
            {extraActions.map(a => <button key={a.label} type="button" onClick={() => { if (current) { a.onClick(current); if (a.closes !== false) onClose(); } }} disabled={!current || !!a.disabled?.(current)} className={a.danger ? `${footerButton} !border-red-300 !text-red-700 disabled:opacity-45` : secondaryButton}>{a.label}</button>)}
            {onEdit && <button type="button" onClick={() => { if (current) { onEdit(current); onClose(); } }} disabled={!current || (canEditRow && !canEditRow(current))} className={secondaryButton} title={current && canEditRow && !canEditRow(current) ? 'This one can\'t be edited (cancelled, held or removed)' : 'Load the selected one into the screen to change it'}>{editLabel}</button>}
            <button type="button" onClick={() => open()} disabled={!current} className={primaryButton}>{openLabel}</button>
            <button type="button" onClick={onClose} className={secondaryButton}><X size={14} />Close</button>
          </div>
        </footer>
      </div>
      {pickerOpen && <ColumnPicker columns={columns} order={order} visible={visible} defaults={base} onApply={applyColumns} onClose={() => setPickerOpen(false)} />}
    </div>,
    document.body,
  );
}
