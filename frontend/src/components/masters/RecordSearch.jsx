// A small "search the record to load" box for a master's detail header (Item Master): type part of the code, name, short name or an alias,
// pick a match with the mouse or ↑ ↓ Enter, and `onPick(row)` loads it. Matches that start with the text come first; 8 are shown.
// Disabled while a record is being added or edited, so a pick never throws away what is being typed.

import { useId, useMemo, useRef, useState } from 'react';
import { Search } from 'lucide-react';

const SHOWN = 8;

export default function RecordSearch({ rows, onPick, disabled = false, placeholder = 'Search item to load…' }) {
  const listId = useId();
  const box = useRef(null);
  const [text, setText] = useState('');
  const [open, setOpen] = useState(false);
  const [at, setAt] = useState(0);

  const matches = useMemo(() => {
    const q = text.trim().toLowerCase();
    if (!q) return [];
    const hits = [];
    for (const r of rows) {
      const keys = [r.code, r.name, r.short_name, ...(Array.isArray(r.aliases) ? r.aliases : [])].map(v => String(v ?? '').toLowerCase());
      const rank = keys.some(k => k.startsWith(q)) ? 0 : keys.some(k => k.includes(q)) ? 1 : -1;
      if (rank >= 0) hits.push({ r, rank });
    }
    return hits.sort((a, b) => a.rank - b.rank).slice(0, SHOWN).map(h => h.r);
  }, [rows, text]);

  const pick = (row) => {
    if (!row) return;
    onPick(row);
    setText(''); setOpen(false); setAt(0);
  };
  const onKeyDown = (e) => {
    if (e.key === 'ArrowDown') { e.preventDefault(); setOpen(true); setAt(i => Math.min(matches.length - 1, i + 1)); }
    else if (e.key === 'ArrowUp') { e.preventDefault(); setAt(i => Math.max(0, i - 1)); }
    else if (e.key === 'Enter' && open && matches.length) { e.preventDefault(); e.stopPropagation(); pick(matches[at] ?? matches[0]); }
    else if (e.key === 'Escape' && (open || text)) { e.stopPropagation(); setText(''); setOpen(false); }
  };

  return (
    <div className="relative w-64 max-w-full">
      <Search size={13} className="pointer-events-none absolute left-2.5 top-1/2 -translate-y-1/2 text-stone-400" aria-hidden="true" />
      <input ref={box} type="search" role="combobox" aria-expanded={open && matches.length > 0} aria-controls={listId} aria-autocomplete="list" aria-label="Search item to load"
        value={text} disabled={disabled} placeholder={placeholder} autoComplete="off"
        onChange={e => { setText(e.target.value); setOpen(true); setAt(0); }} onFocus={() => setOpen(true)} onBlur={() => setTimeout(() => setOpen(false), 120)} onKeyDown={onKeyDown}
        className="w-full rounded-lg border border-line bg-surface py-1.5 pl-8 pr-2 text-xs outline-none transition focus:border-[#64283A] focus:ring-2 focus:ring-[#64283A]/15 disabled:opacity-50 dark:focus:border-[#D4A847] dark:focus:ring-[#D4A847]/15" />
      {open && text.trim() && (
        <ul id={listId} role="listbox" className="absolute left-0 right-0 top-full z-30 mt-1 max-h-72 overflow-auto rounded-lg border border-line bg-surface py-1 text-xs shadow-xl">
          {matches.length === 0 && <li className="px-3 py-2 text-stone-500">No item matches.</li>}
          {matches.map((r, i) => (
            <li key={String(r.code)} role="option" aria-selected={i === at}>
              <button type="button" onMouseDown={e => e.preventDefault()} onClick={() => pick(r)}
                className={`flex w-full items-center gap-2 px-3 py-1.5 text-left ${i === at ? 'bg-[#64283A]/10 dark:bg-[#D4A847]/15' : 'hover:bg-[#64283A]/5'}`}>
                <span className="font-mono">{String(r.code)}</span><span className="truncate">{String(r.name ?? '')}</span>
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
