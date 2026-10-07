// Item search for the bill: type a barcode/item code or part of the name and pick with the keyboard.
// Suggestions (at most 8) rank an exact code first, so a barcode scanner's "code + Enter" always adds the
// scanned item; then codes that start with the text, then names containing every typed word.
// ↑/↓ move through the suggestions, Enter picks the highlighted one, Esc closes the list. Keys the box
// doesn't use (↑/↓ with no list, +, -, Delete, ...) go to the screen's onKeyDown. Only the keyboard moves the
// highlight (a resting mouse pointer must never change what a scanner's Enter adds).
// While a save popup is waiting, Enter here with text still adds the item (Toast.jsx leaves it to the box).

import { useId, useMemo, useState, forwardRef, } from 'react';
import { ScanLine } from 'lucide-react';
import { field, money } from '../../lib/styles';
import { searchItems, } from '../../data/itemSearch';

;












const ItemSearchBox = forwardRef(function ItemSearchBox(
  { id, value, onChange, onPick, onEnterNoMatch, onKeyDown, placeholder }, ref,
) {
  const listId = useId();
  const [highlight, setHighlight] = useState(0);
  const [closed, setClosed] = useState(false);
  const matches = useMemo(() => searchItems(value), [value]);
  const open = !closed && matches.length > 0;
  const at = Math.min(highlight, matches.length - 1);

  const pick = (item) => { setClosed(true); onPick(item); };

  const keyDown = (e) => {
    if (open && (e.key === 'ArrowDown' || e.key === 'ArrowUp')) {
      e.preventDefault();
      setHighlight(Math.max(0, Math.min(matches.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1))));
      return;
    }
    if (e.key === 'Enter' && !e.ctrlKey && !e.metaKey && !e.altKey) {
      e.preventDefault();
      if (open) pick(matches[at]);
      else if (value.trim() && matches.length) pick(matches[0]);
      else if (value.trim()) onEnterNoMatch();
      else onKeyDown?.(e);
      return;
    }
    if (e.key === 'Escape' && open) {
      e.preventDefault();
      e.stopPropagation();
      setClosed(true);
      return;
    }
    if (e.key === 'Tab' && open) setClosed(true);
    onKeyDown?.(e);
  };

  return (
    <div className="relative">
      <ScanLine size={16} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
      <input
        id={id} ref={ref} autoFocus value={value} autoComplete="off" spellCheck={false}
        role="combobox" aria-expanded={open} aria-controls={listId} aria-autocomplete="list"
        aria-activedescendant={open ? `${listId}-${at}` : undefined}
        onChange={e => { onChange(e.target.value); setHighlight(0); setClosed(false); }}
        onBlur={() => setClosed(true)}
        onFocus={() => setClosed(false)}
        onKeyDown={keyDown}
        className={`${field} h-10 pl-9 font-mono text-sm`}
        placeholder={placeholder}
      />
      {open && (
        <ul id={listId} role="listbox" aria-label="Matching items"
          className="absolute left-0 right-0 top-full z-30 mt-1 max-h-80 overflow-auto rounded-xl border border-line bg-surface p-1 text-xs shadow-2xl">
          {matches.map((item, i) => (
            <li key={item.code} id={`${listId}-${i}`} role="option" aria-selected={i === at}
              onMouseDown={e => { e.preventDefault(); pick(item); }}
              className={`flex cursor-pointer items-center justify-between gap-3 rounded-lg px-2.5 py-1.5 ${i === at ? 'bg-accent-soft text-accent-ink' : 'text-stone-700 hover:bg-subtle dark:text-stone-200'}`}>
              <span className="min-w-0">
                <span className="block truncate font-medium text-stone-900 dark:text-stone-50">{item.brand || item.name} <span className="font-normal text-stone-500">{item.pkg}</span></span>
                <span className="block truncate font-mono text-[10.5px] text-stone-500">{item.code}{item.brand && item.name !== item.brand ? ` · ${item.name}` : ''}</span>
              </span>
              <span className="shrink-0 text-right tabular-nums">
                <span className="block font-semibold">₹ {money(item.rate)}</span>
                <span className={`block text-[10.5px] ${item.clQty > 0 ? 'text-stone-500' : 'text-red-600 dark:text-red-400'}`}>{item.clQty} in stock</span>
              </span>
            </li>
          ))}
          <li aria-hidden="true" className="px-2.5 pt-1 text-[10px] text-stone-400">↑↓ choose · Enter add · Esc close</li>
        </ul>
      )}
    </div>
  );
});

export default ItemSearchBox;
