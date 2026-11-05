// Item search for the bill: type a barcode/item code or part of the name and pick with the keyboard.
// Matches rank an exact code first, so a barcode scanner's "code + Enter" always adds the scanned item; then codes
// that start with the text, then names containing every typed word. Items with no stock are not listed.
// The list is a Windows-style grid (ItemPickGrid). Clicking the empty box, or ↓ when the bill has no lines
// (`listOnArrow`), or Alt+↓, opens the list of every item in stock; an empty box never opens it by itself, so a
// scanner's Enter on an empty box can't add the first item by mistake.
// ↑/↓ move through the list, Enter picks the highlighted one, Esc closes it. Keys the box doesn't use (↑/↓ with
// no list, +, -, Delete, ...) go to the screen's onKeyDown. Only the keyboard moves the highlight (a resting mouse
// pointer must never change what a scanner's Enter adds).
// While a save popup is waiting, Enter here with text still adds the item (Toast.jsx leaves it to the box).

import { useId, useMemo, useState, forwardRef, } from 'react';
import { ScanLine } from 'lucide-react';
import { field } from '../../lib/styles';
import { searchItems, } from '../../data/itemSearch';
import { stockVersion, useStockItems } from '../../data/stockData';
import ItemPickGrid from './ItemPickGrid';
import { nextSort, sortItems } from '../../data/itemSort';

const ItemSearchBox = forwardRef(function ItemSearchBox(
  { id, value, onChange, onPick, onEnterNoMatch, onKeyDown, placeholder, listOnArrow = false, onBrowse }, ref,
) {
  const listId = useId();
  const [highlight, setHighlight] = useState(0);
  const [closed, setClosed] = useState(false);
  const [sort, setSort] = useState({ key: '', dir: 1 });   // the heading the list is sorted by (typing starts again from the best match first)
  const [browse, setBrowse] = useState(false);   // the empty box's list was asked for
  useStockItems();   // re-render (and search again) when the catalogue loads or stock changes
  const version = stockVersion();
  const matches = useMemo(() => searchItems(value), [value, version]);   // eslint-disable-line react-hooks/exhaustive-deps
  const shown = useMemo(() => sortItems(matches, sort), [matches, sort]);
  const typed = value.trim() !== '';
  const open = !closed && matches.length > 0 && (typed || browse);
  const at = Math.min(highlight, shown.length - 1);

  const close = () => { setClosed(true); setBrowse(false); };
  const pick = (item) => { close(); onPick(item); };
  const show = () => {
    if (onBrowse) { onBrowse(); return; }   // the Sales Invoice opens the desktop's "Please Select Item" window instead of a dropdown
    setClosed(false); setBrowse(true); setHighlight(0);
  };

  const keyDown = (e) => {
    if (!open && !typed && e.key === 'ArrowDown' && (listOnArrow || e.altKey)) {
      e.preventDefault();
      show();
      return;
    }
    if (open && (e.key === 'ArrowDown' || e.key === 'ArrowUp')) {
      e.preventDefault();
      setHighlight(Math.max(0, Math.min(shown.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1))));
      return;
    }
    if (e.key === 'Enter' && !e.ctrlKey && !e.metaKey && !e.altKey) {
      e.preventDefault();
      if (open) pick(shown[at]);
      else if (typed && matches.length) pick(matches[0]);
      else if (typed) onEnterNoMatch();
      else onKeyDown?.(e);
      return;
    }
    if (e.key === 'Escape' && open) {
      e.preventDefault();
      e.stopPropagation();
      close();
      return;
    }
    if (e.key === 'Tab' && open) close();
    onKeyDown?.(e);
  };

  return (
    <div className="relative">
      <ScanLine size={16} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
      <input
        id={id} ref={ref} autoFocus value={value} autoComplete="off" spellCheck={false}
        role="combobox" aria-expanded={open} aria-controls={listId} aria-autocomplete="list"
        aria-activedescendant={open ? `${listId}-${at}` : undefined}
        onChange={e => { onChange(e.target.value); setHighlight(0); setClosed(false); setSort({ key: '', dir: 1 }); }}
        onBlur={close}
        onFocus={() => setClosed(false)}
        onClick={() => { if (!typed && !open && !onBrowse) show(); }}   /* with the picker window, only ↓ opens it (the desktop's "Press Down Arrow for list") */
        onKeyDown={keyDown}
        className={`${field} h-10 pl-9 font-mono text-sm`}
        placeholder={placeholder}
      />
      {open && (
        <div className="absolute left-0 top-full z-40 mt-1 w-[min(760px,calc(100vw-4rem))] shadow-2xl">
          <ItemPickGrid id={listId} items={shown} at={at} onPick={pick} showStock showRate windowsOrder sort={sort} onSort={k => { setSort(s => nextSort(s, k)); setHighlight(0); }} className="max-h-80" />
          <p aria-hidden="true" className="border border-t-0 border-line bg-surface px-2.5 py-1 text-[10px] text-stone-400">↑↓ choose · Enter add · Esc close · click a heading to sort · items with no stock are not listed</p>
        </div>
      )}
    </div>
  );
});

export default ItemSearchBox;
