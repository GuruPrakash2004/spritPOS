import { useMemo, useRef, useState } from 'react';
import { Search, X } from 'lucide-react';
import { ADMIN_MASTER, MODULE_GROUPS, MODULE_KEYWORDS } from './modules';

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

  const choose = (name: string) => {
    onOpen(name);
    setQuery('');
    setOpen(false);
    inputRef.current?.blur();
  };

  return (
    <div className="relative ml-auto w-full min-w-0 max-w-xs print:hidden">
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
        placeholder="Search screens… e.g. SINV, breakage, DSR"
        aria-label="Search screens"
        role="combobox"
        aria-expanded={open}
        aria-controls="module-search-list"
        className="h-8 w-full rounded-full border border-[#e1c9b6] bg-[#fffaf5] pl-8 pr-8 text-xs text-stone-800 outline-none placeholder:text-stone-400 focus:border-[#e8702a] dark:border-[#343b59] dark:bg-[#171b2b] dark:text-stone-100"
      />
      {query && (
        <button onClick={() => { setQuery(''); inputRef.current?.focus(); }} className="absolute right-2.5 top-1/2 -translate-y-1/2 text-stone-400 hover:text-[#b84716]" aria-label="Clear search"><X size={14} /></button>
      )}
      {open && (
        <ul id="module-search-list" role="listbox" className="absolute right-0 top-full z-30 mt-1.5 max-h-80 w-full min-w-[240px] overflow-auto rounded-xl border border-[#e1c9b6] bg-[#fffaf5] py-1 text-xs shadow-xl dark:border-[#343b59] dark:bg-[#171b2b]">
          {results.map((m, i) => (
            <li key={m.name} role="option" aria-selected={i === highlight}>
              <button
                onMouseDown={e => e.preventDefault()}
                onClick={() => choose(m.name)}
                onMouseEnter={() => setHighlight(i)}
                className={`flex w-full items-center justify-between gap-3 px-3 py-2 text-left ${i === highlight ? 'bg-[#e8702a]/15 text-[#b84716] dark:text-[#ffb07e]' : 'text-stone-700 dark:text-stone-200'}`}
              >
                <span className="font-semibold">{LABELS[m.name] ?? m.name}</span>
                <span className="text-[10px] uppercase tracking-wider text-stone-400">{m.group}</span>
              </button>
            </li>
          ))}
          {!results.length && <li className="px-3 py-3 text-center text-stone-400">No screen matches “{query}”.</li>}
        </ul>
      )}
    </div>
  );
}
