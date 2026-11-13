// Multi-select dropdown: a field-looking button that shows what is picked ("All", the one name, or "3 selected") and opens a
// checkbox list (with a filter box and Select all / Clear) in common/DropdownMenu, so it stays on screen and Esc closes it.
//   <MultiSelectDropdown id="ps-groups" value={groups} options={groupOptions} onChange={setGroups} placeholder="Select groups" />

import { useMemo, useRef, useState } from 'react';
import { ChevronDown } from 'lucide-react';
import DropdownMenu from './DropdownMenu';
import { field } from '../../lib/styles';

export default function MultiSelectDropdown({ id, value, options, onChange, placeholder = 'Select', className = '', width = 240 }) {
  const [open, setOpen] = useState(false);
  const [q, setQ] = useState('');
  const anchorRef = useRef(null);
  const shown = useMemo(() => options.filter(o => String(o).toLowerCase().includes(q.trim().toLowerCase())), [options, q]);
  const summary = value.length === 0 ? placeholder : value.length === 1 ? value[0] : `${value.length} selected`;
  const toggle = (o) => onChange(value.includes(o) ? value.filter(v => v !== o) : [...value, o]);

  return (
    <>
      <button ref={anchorRef} id={id} type="button" onClick={() => setOpen(o => !o)} aria-expanded={open} aria-haspopup="listbox"
        className={`${field} flex items-center justify-between gap-2 text-left ${className}`}>
        <span className={`truncate ${value.length ? '' : 'text-stone-400'}`}>{summary}</span>
        <ChevronDown size={14} className="shrink-0 text-stone-500" />
      </button>
      <DropdownMenu anchorRef={anchorRef} open={open} onClose={() => { setOpen(false); setQ(''); }} align="start" width={width}>
        <div className="p-1">
          <input autoFocus value={q} onChange={e => setQ(e.target.value)} placeholder="Search…" aria-label="Filter options" className={`${field} mb-1 w-full`} />
          <div className="mb-1 flex justify-between px-1 text-[11px] font-medium text-[#64283A] dark:text-[#D4A847]">
            <span role="button" tabIndex={0} onClick={() => onChange([...options])} onKeyDown={e => { if (e.key === 'Enter') onChange([...options]); }} className="cursor-pointer hover:underline">Select all</span>
            <span role="button" tabIndex={0} onClick={() => onChange([])} onKeyDown={e => { if (e.key === 'Enter') onChange([]); }} className="cursor-pointer hover:underline">Clear</span>
          </div>
          <div role="listbox" aria-multiselectable="true" className="max-h-56 overflow-auto">
            {shown.length === 0 && <div className="px-2 py-3 text-center text-stone-500">No match</div>}
            {shown.map(o => (
              <label key={o} className="flex cursor-pointer items-center gap-2 rounded-lg px-2 py-1.5 hover:bg-subtle">
                <input type="checkbox" checked={value.includes(o)} onChange={() => toggle(o)} />
                <span className="truncate">{o}</span>
              </label>
            ))}
          </div>
        </div>
      </DropdownMenu>
    </>
  );
}
