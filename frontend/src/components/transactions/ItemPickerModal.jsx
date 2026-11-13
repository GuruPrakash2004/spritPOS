// The desktop's "Please Select Item" window for the Sales Invoice (F2 or ↓ in the empty Scan Barcode box): a search box,
// a grid of ShortName / QOH / MRP / ItemCode / ItemName / ItemGroupName with drag-resizable column widths (drag the edge of
// a heading; the widths are remembered on this computer), "Total Records : shown / all", and Ok / Cancel.
// Keys: type to search, ↑/↓ move the highlight, Enter or Ok picks it, Esc closes. A double click on a row picks it.

import { useEffect, useMemo, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';
import { money, primaryButton, secondaryButton, field, iconButton } from '../../lib/styles';
import { searchItems } from '../../data/itemSearch';
import { stockVersion, useStockItems } from '../../data/stockData';
import { useDialogKeys } from '../common/useDialogKeys';

const COLUMNS = [
  { key: 'shortName', label: 'ShortName', width: 280, get: i => i.shortName || i.name },
  { key: 'clQty', label: 'QOH', width: 70, right: true, get: i => i.clQty },
  { key: 'rate', label: 'MRP', width: 90, right: true, get: i => money(i.rate) },
  { key: 'code', label: 'ItemCode', width: 150, mono: true, get: i => i.code },
  { key: 'name', label: 'ItemName', width: 380, get: i => i.name },
  { key: 'group', label: 'ItemGroupName', width: 200, get: i => i.group },
];
const WIDTHS_KEY = 'vlqms_item_picker_widths';
const MIN_WIDTH = 40;

function readWidths() {
  try {
    const saved = JSON.parse(localStorage.getItem(WIDTHS_KEY) || '{}');
    return Object.fromEntries(COLUMNS.map(c => [c.key, Number(saved[c.key]) >= MIN_WIDTH ? Number(saved[c.key]) : c.width]));
  } catch { return Object.fromEntries(COLUMNS.map(c => [c.key, c.width])); }
}

export default function ItemPickerModal({ onPick, onClose }) {
  useStockItems();
  const version = stockVersion();
  const [text, setText] = useState('');
  const [at, setAt] = useState(0);
  const [widths, setWidths] = useState(readWidths);
  const ref = useRef(null);
  const listRef = useRef(null);
  useDialogKeys(ref, onClose);

  const total = useMemo(() => searchItems('', 1e6).length, [version]);   // eslint-disable-line react-hooks/exhaustive-deps
  const items = useMemo(() => searchItems(text, 1e6), [text, version]);   // eslint-disable-line react-hooks/exhaustive-deps
  const row = Math.min(at, items.length - 1);

  useEffect(() => {
    listRef.current?.querySelector('[aria-selected="true"]')?.scrollIntoView({ block: 'nearest' });
  }, [row, items]);
  useEffect(() => { try { localStorage.setItem(WIDTHS_KEY, JSON.stringify(widths)); } catch { /* not kept */ } }, [widths]);

  const startDrag = (key) => (e) => {
    e.preventDefault();
    e.stopPropagation();
    const x0 = e.clientX, w0 = widths[key];
    const move = (ev) => setWidths(w => ({ ...w, [key]: Math.max(MIN_WIDTH, w0 + ev.clientX - x0) }));
    const up = () => { window.removeEventListener('mousemove', move); window.removeEventListener('mouseup', up); document.getElementById('pick-search')?.focus(); };   // typing and Enter keep working after a drag
    window.addEventListener('mousemove', move);
    window.addEventListener('mouseup', up);
  };

  const keyDown = (e) => {
    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
      e.preventDefault();
      setAt(Math.max(0, Math.min(items.length - 1, row + (e.key === 'ArrowDown' ? 1 : -1))));
    } else if (e.key === 'PageDown' || e.key === 'PageUp') {
      e.preventDefault();
      setAt(Math.max(0, Math.min(items.length - 1, row + (e.key === 'PageDown' ? 10 : -10))));
    } else if (e.key === 'Enter') {
      e.preventDefault();
      if (items[row]) onPick(items[row]);
    }
  };

  const grid = { gridTemplateColumns: COLUMNS.map(c => `${widths[c.key]}px`).join(' ') };
  const cell = 'truncate whitespace-nowrap px-2 py-1.5';
  return createPortal(
    <div className="modal-backdrop fixed inset-0 z-[900] flex items-center justify-center bg-black/65 p-4 dark:bg-black/80" onClick={onClose}>
      <div ref={ref} role="dialog" aria-modal="true" aria-label="Please Select Item" onClick={e => e.stopPropagation()} onKeyDown={keyDown}
        className="flex max-h-[90vh] w-full max-w-5xl flex-col overflow-hidden rounded-xl border border-[#E3D9CD] bg-white text-[#29231F] shadow-2xl animate-slideUp dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100">
        <div className="flex items-center justify-between border-b border-[#E3D9CD] px-4 py-2.5 dark:border-[#2A2A2A]">
          <h3 className="text-sm font-semibold">Please Select Item</h3>
          <button onClick={onClose} tabIndex={-1} className={iconButton} aria-label="Close" title="Close (Esc)"><X size={16} /></button>
        </div>
        <div className="flex items-center gap-3 px-4 py-3">
          <label htmlFor="pick-search" className="shrink-0 text-xs font-medium">Enter your search string here</label>
          <input id="pick-search" data-autofocus autoFocus value={text} autoComplete="off" spellCheck={false}
            onChange={e => { setText(e.target.value); setAt(0); }} className={`${field} h-9 flex-1 text-sm`} />
        </div>
        <div ref={listRef} role="listbox" aria-label="Items" className="mx-4 h-[26rem] overflow-auto border border-line bg-surface text-xs">
          <div role="presentation" style={grid} className="sticky top-0 z-10 grid w-max min-w-full border-b border-line bg-[#F3EEE6] font-semibold dark:bg-[#1E1E1E]">
            {COLUMNS.map(c => (
              <span key={c.key} className={`relative ${cell} ${c.right ? 'text-right' : ''}`}>
                {c.label}
                <span role="separator" aria-orientation="vertical" aria-label={`Resize ${c.label}`} onMouseDown={startDrag(c.key)}
                  className="absolute right-0 top-0 h-full w-1.5 cursor-col-resize hover:bg-[#64283A]/40" />
              </span>
            ))}
          </div>
          {items.map((item, i) => (
            <div key={item.code} role="option" aria-selected={i === row} style={grid} onClick={() => setAt(i)} onDoubleClick={() => onPick(item)}
              className={`grid w-max min-w-full cursor-pointer border-b border-line-soft ${i === row ? 'bg-[#FF8C1A] text-white' : 'hover:bg-subtle'}`}>
              {COLUMNS.map(c => <span key={c.key} className={`${cell} ${c.right ? 'text-right tabular-nums' : ''} ${c.mono ? 'font-mono' : ''}`}>{c.get(item)}</span>)}
            </div>
          ))}
          {!items.length && <p className="px-3 py-6 text-center text-stone-500">No items match.</p>}
        </div>
        <div className="flex items-center justify-between px-4 py-3">
          <span className="text-xs text-[#64283A] dark:text-[#D4A847]">Total Records : {items.length} / {total}</span>
          <div data-dialog-footer className="flex gap-2">
            <button type="button" onClick={onClose} className={secondaryButton}>Cancel</button>
            <button type="button" onClick={() => items[row] && onPick(items[row])} className={primaryButton}>Ok</button>
          </div>
        </div>
      </div>
    </div>,
    document.body,
  );
}
