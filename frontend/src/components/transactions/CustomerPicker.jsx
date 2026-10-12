// Customer box of the Sales Invoice with a visible pick list (Code / Name / Mobile / Points). Clicking the box, F4 or ↓ opens
// the list of every customer; typing narrows it (a browser <datalist> only showed names matching the text, so with "CASH" in
// the box nobody else was offered). ↑/↓ move, Enter picks the highlighted one (or keeps the typed name when nothing matches),
// Esc closes. A typed name that isn't in Customer Master is still accepted, as before.

import { useId, useMemo, useState } from 'react';
import { ChevronDown } from 'lucide-react';
import { field } from '../../lib/styles';

const MAX = 200;

export default function CustomerPicker({ id, value, onChange, customers, disabled, title, onDone, className = '' }) {
  const listId = useId();
  const [open, setOpen] = useState(false);
  const [all, setAll] = useState(true);   // opened without typing: list everyone
  const [at, setAt] = useState(0);
  const [moved, setMoved] = useState(false);   // an arrow key or a click opened/moved the list: only then does Enter take the highlighted row
  const term = value.trim().toLowerCase();
  const matches = useMemo(() => {
    const list = all || !term ? customers : customers.filter(c => `${c.code} ${c.name} ${c.mobile ?? ''}`.toLowerCase().includes(term));
    return list.slice(0, MAX);
  }, [customers, term, all]);
  const shown = open && matches.length > 0;
  const current = Math.min(at, matches.length - 1);

  const show = () => { setMoved(false); setAll(true); setAt(Math.max(0, customers.findIndex(c => c.name.toLowerCase() === term))); setOpen(true); };
  const pick = (c) => { onChange(c.name); setOpen(false); onDone?.(); };

  const keyDown = (e) => {
    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
      e.preventDefault();
      if (!shown) return show();
      setMoved(true);
      setAt(Math.max(0, Math.min(matches.length - 1, current + (e.key === 'ArrowDown' ? 1 : -1))));
    } else if (e.key === 'Enter') {
      e.preventDefault();
      // Typing narrows the list but Enter keeps what was typed (a new customer name that starts like an existing one), unless the
      // text is an exact name / code or a row was chosen with the arrows.
      const exact = matches.find(c => c.name.toLowerCase() === term || c.code.toLowerCase() === term);
      if (shown && (moved || all || exact)) pick(exact && !moved ? exact : matches[current]); else { setOpen(false); onDone?.(); }
    } else if (e.key === 'Escape') {
      e.preventDefault();
      if (shown) { e.stopPropagation(); setOpen(false); } else onDone?.();
    }
  };

  return (
    <div className="relative">
      <input id={id} role="combobox" aria-expanded={shown} aria-controls={listId} aria-autocomplete="list" autoComplete="off"
        value={value} disabled={disabled} title={title} placeholder="Walk-in customer"
        onChange={e => { onChange(e.target.value); setAll(false); setAt(0); setOpen(true); }}
        onClick={() => { if (!open) show(); }}
        onFocus={e => { e.target.select(); show(); }}
        onBlur={() => setOpen(false)}
        onKeyDown={keyDown}
        className={`${field} h-8 py-1 pr-7 text-xs ${className}`} />
      <ChevronDown size={14} className="pointer-events-none absolute right-2 top-1/2 -translate-y-1/2 text-stone-400" />
      {shown && (
        <div id={listId} role="listbox" aria-label="Customers" className="absolute left-0 top-full z-40 mt-1 max-h-64 w-[min(420px,calc(100vw-3rem))] overflow-auto border border-line bg-surface text-xs shadow-2xl">
          <div role="presentation" className="sticky top-0 grid grid-cols-[5rem_minmax(0,1fr)_6.5rem_3.5rem] border-b border-line bg-[#F0F3F8] font-semibold text-stone-700 dark:bg-[#1E1E1E] dark:text-stone-200">
            <span className="px-2 py-1.5">Code</span><span className="px-2 py-1.5">Name</span><span className="px-2 py-1.5">Mobile</span><span className="px-2 py-1.5 text-right">Points</span>
          </div>
          {matches.map((c, i) => (
            <div key={c.code} role="option" aria-selected={i === current} ref={i === current ? el => el?.scrollIntoView({ block: 'nearest' }) : undefined}
              onMouseDown={e => { e.preventDefault(); pick(c); }}
              className={`grid cursor-pointer grid-cols-[5rem_minmax(0,1fr)_6.5rem_3.5rem] border-b border-line-soft ${i === current ? 'bg-[#BBD7F0] text-stone-900 dark:bg-[#27405A] dark:text-stone-50' : 'text-stone-700 hover:bg-subtle dark:text-stone-200'}`}>
              <span className="truncate px-2 py-1.5 font-mono">{c.code}</span><span className="truncate px-2 py-1.5">{c.name}</span>
              <span className="truncate px-2 py-1.5">{c.mobile}</span><span className="px-2 py-1.5 text-right tabular-nums">{c.points || ''}</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
