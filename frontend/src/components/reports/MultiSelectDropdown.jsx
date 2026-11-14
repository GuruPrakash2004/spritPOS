// A drop-down that lets several options be ticked (Brand Wise Report's "Select Brand" / "Select ItemGroup", the exe's multi-selection window).
// Shows "All" when nothing is ticked, the name when one is, "N selected" otherwise; the list opens in the shared DropdownMenu (portal, stays on screen).

import { useRef, useState } from 'react';
import { ChevronDown } from 'lucide-react';
import DropdownMenu from '../common/DropdownMenu';
import { field } from '../../lib/styles';

export default function MultiSelectDropdown({ id, options, value, onChange, allLabel = 'All' }) {
  const [open, setOpen] = useState(false);
  const ref = useRef(null);
  const names = Object.fromEntries(options.map(o => [o.value, o.label]));
  const text = !value.length ? allLabel : value.length === 1 ? names[value[0]] || value[0] : `${value.length} selected`;
  const toggle = v => onChange(value.includes(v) ? value.filter(x => x !== v) : [...value, v]);
  return (
    <>
      <button type="button" id={id} ref={ref} onClick={() => setOpen(o => !o)} aria-expanded={open} aria-haspopup="listbox"
        className={`${field} flex items-center justify-between gap-2 text-left`}>
        <span className="truncate">{text}</span><ChevronDown size={14} className="shrink-0" />
      </button>
      <DropdownMenu anchorRef={ref} open={open} onClose={() => setOpen(false)} align="start" width={260}>
        <div role="listbox" aria-multiselectable="true" className="max-h-72 overflow-auto">
          <button type="button" className="block w-full px-3 py-2 text-left text-xs font-medium text-accent hover:bg-subtle" onClick={() => onChange([])}>{allLabel} (clear)</button>
          {options.map(o => (
            <label key={o.value} className="flex cursor-pointer items-center gap-2 px-3 py-1.5 text-xs hover:bg-subtle">
              <input type="checkbox" checked={value.includes(o.value)} onChange={() => toggle(o.value)} />
              <span className="truncate">{o.label}</span>
            </label>
          ))}
        </div>
      </DropdownMenu>
    </>
  );
}
