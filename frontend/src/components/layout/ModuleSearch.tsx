// Header search box (⌘/Ctrl+K or `/`). Matches the typed text against module names and
// MODULE_KEYWORDS, shows only modules the user can open, and opens the chosen one.

import { useEffect, useMemo, useRef, useState } from 'react';
import { Search, X } from 'lucide-react';
import { ADMIN_MASTER, MODULE_GROUPS, MODULE_KEYWORDS } from '../../lib/modules';

// The label users see for a module where it differs from the routing key.
const LABELS: Record<string, string> = { 'Reports Dashboard': 'Customised Reports' };

const ALL = [
  ...MODULE_GROUPS.flatMap(g => g.modules.map(name => ({ name, group: g.group }))),
  { name: ADMIN_MASTER, group: 'Administration' },
];

// Search box in the active-module bar: type part of a screen name or a legacy term, then click or Enter.
export default function ModuleSearch({ canOpen, onOpen }: { canOpen: (name: string) => boolean; onOpen: (name: string) => void }) {
  const [query, setQuery] = useState('');
  const [open, setOpen] = useState(false);
  const [highlight, setHighlight] = useState(0);
  const inputRef = useRef<HTMLInputElement>(null);

  const results = useMemo(() => {
    const words = query.toLowerCase().split(/\s+/).filter(Boolean);
    const allowed = ALL.filter(m => canOpen(m.name));
    if (!words.length) return allowed.slice(0, 8);
    return allowed
      .map(m => {
        const label = (LABELS[m.name] ?? m.name).toLowerCase();
        const hay = `${label} ${m.group.toLowerCase()} ${MODULE_KEYWORDS[m.name] ?? ''}`;
        if (!words.every(w => hay.includes(w))) return null;
        // Names that start with the query rank first, then name matches, then keyword-only matches.
        const score = label.startsWith(words[0]) ? 0 : words.every(w => label.includes(w)) ? 1 : 2;
        return { ...m, score };
      })
      .filter((m): m is NonNullable<typeof m> => m !== null)
      .sort((a, b) => a.score - b.score);
  }, [query, canOpen]);

  // Ctrl/⌘+K or "/" (outside a text field) jumps to the search box from anywhere.
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      const typing = e.target instanceof HTMLElement && e.target.closest('input, textarea, select, [contenteditable]');
      if ((e.key === 'k' && (e.ctrlKey || e.metaKey)) || (e.key === '/' && !typing)) {
        e.preventDefault();
        inputRef.current?.focus();
      }
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, []);

  const choose = (name: string) => {
    onOpen(name);
    setQuery('');
    setOpen(false);
    inputRef.current?.blur();
  };

  return (
    <div className="relative w-full min-w-0 print:hidden">
      <Search size={14} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
      <input
        ref={inputRef}
        value={query}
        onChange={e => { setQuery(e.target.value); setHighlight(0); setOpen(true); }}
        onFocus={() => setOpen(true)}
        // Delay so a click on a result lands before the list closes.
        onBlur={() => setTimeout(() => setOpen(false), 150)}
        onKeyDown={e => {
          if (e.key === 'ArrowDown') { e.preventDefault(); setHighlight(h => Math.min(h + 1, results.length - 1)); }
          if (e.key === 'ArrowUp') { e.preventDefault(); setHighlight(h => Math.max(h - 1, 0)); }
          if (e.key === 'Enter' && results[highlight]) choose(results[highlight].name);
          if (e.key === 'Escape') { setOpen(false); inputRef.current?.blur(); }
        }}
        placeholder="Search screens…"
        aria-label="Search screens"
        role="combobox"
        aria-expanded={open}
        aria-controls="module-search-list"
        className="h-9 w-full rounded-lg border border-line bg-subtle pl-8 pr-14 text-xs text-stone-800 outline-none transition-colors placeholder:text-stone-500 hover:border-line-strong focus:border-accent focus:bg-surface focus:ring-2 focus:ring-accent/20 dark:text-stone-100"
      />
      {query ? (
        <button onClick={() => { setQuery(''); inputRef.current?.focus(); }} className="absolute right-2.5 top-1/2 -translate-y-1/2 rounded text-stone-400 hover:text-stone-700" aria-label="Clear search"><X size={14} /></button>
      ) : (
        <kbd className="pointer-events-none absolute right-2 top-1/2 hidden -translate-y-1/2 rounded border border-line bg-surface px-1.5 py-px font-sans text-[10px] font-medium text-stone-500 sm:block">Ctrl K</kbd>
      )}
      {open && (
        <ul id="module-search-list" role="listbox" className="absolute right-0 top-full z-30 mt-1 max-h-80 w-full min-w-[260px] overflow-auto rounded-xl border border-line bg-surface p-1 text-xs shadow-pop">
          {results.map((m, i) => (
            <li key={m.name} role="option" aria-selected={i === highlight}>
              <button
                onMouseDown={e => e.preventDefault()}
                onClick={() => choose(m.name)}
                onMouseEnter={() => setHighlight(i)}
                className={`flex w-full items-center justify-between gap-3 rounded-lg px-3 py-2 text-left ${i === highlight ? 'bg-subtle text-stone-900 dark:text-stone-50' : 'text-stone-700 dark:text-stone-200'}`}
              >
                <span className="font-medium">{LABELS[m.name] ?? m.name}</span>
                <span className="text-[11px] text-stone-500">{m.group}</span>
              </button>
            </li>
          ))}
          {!results.length && <li className="px-3 py-4 text-center text-stone-500">No screen matches “{query}”.</li>}
        </ul>
      )}
    </div>
  );
}
