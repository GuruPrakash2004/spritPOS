// "Select Columns to Display" window of the list windows (the exe's Select Cols): tick the columns to show, search them,
// and move the highlighted one Top / Up / Down / Bottom to change the order. Select All / Deselect All, Reset to Default,
// Apply Changes. Nothing changes until Apply; Close (or Esc) throws the edits away.
//
//   <ColumnPicker columns={[{ key, label }]} order={['id','code',…]} visible={['code','name']} defaults={['code','name']}
//                 onApply={({ order, visible }) => …} onClose={…} />
//
// `order` holds every column in its saved order; `visible` the ticked ones. Keyboard: ↑/↓ move the highlight, Space ticks it,
// Alt+↑ / Alt+↓ move the highlighted column up / down, Enter applies.

import { useId, useMemo, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { ArrowDown, ArrowDownToLine, ArrowUp, ArrowUpToLine, Check, Columns3, Search, X } from 'lucide-react';
import { field, iconButton, primaryButton, secondaryButton } from '../../lib/styles';
import { useDialogKeys } from './useDialogKeys';

const small = 'inline-flex items-center gap-1 rounded-full border border-[#E3D9CD] px-3 py-1 text-[11px] font-semibold text-[#70675F] transition hover:bg-[#F3EEE6] disabled:pointer-events-none disabled:opacity-40 dark:border-[#2A2A2A] dark:text-stone-300 dark:hover:bg-[#1C1C1C]';

export default function ColumnPicker({ columns, order, visible, defaults, reorder = true, onApply, onClose }) {   // reorder={false}: a fixed template, only ticking
  const titleId = useId();
  const ref = useRef(null);
  useDialogKeys(ref, onClose);
  const byKey = useMemo(() => new Map(columns.map(c => [c.key, c])), [columns]);
  const complete = (list) => [...list.filter(k => byKey.has(k)), ...columns.map(c => c.key).filter(k => !list.includes(k))];
  const [keys, setKeys] = useState(() => complete(order));
  const [ticked, setTicked] = useState(() => new Set(visible));
  const [current, setCurrent] = useState(keys[0] ?? '');
  const [find, setFind] = useState('');

  const move = (how) => setKeys(list => {
    const i = list.indexOf(current);
    if (i < 0) return list;
    const out = list.filter(k => k !== current);
    const at = how === 'top' ? 0 : how === 'bottom' ? out.length : how === 'up' ? Math.max(0, i - 1) : Math.min(out.length, i + 1);
    out.splice(at, 0, current);
    return out;
  });
  const toggle = (k) => setTicked(s => { const n = new Set(s); if (n.has(k)) n.delete(k); else n.add(k); return n; });
  const shown = keys.filter(k => (byKey.get(k)?.label ?? k).toLowerCase().includes(find.trim().toLowerCase()));
  const index = keys.indexOf(current);
  const apply = () => { if (ticked.size) onApply({ order: keys, visible: keys.filter(k => ticked.has(k)) }); };
  const reset = () => { const d = complete(defaults); setKeys([...d]); setTicked(new Set(defaults)); setCurrent(d[0] ?? ''); };

  const onKeyDown = (e) => {
    if (e.target.tagName === 'INPUT') { if (e.key === 'Enter') { e.preventDefault(); apply(); } return; }
    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
      e.preventDefault();
      if (e.altKey) { if (reorder) move(e.key === 'ArrowDown' ? 'down' : 'up'); return; }
      const at = shown.indexOf(current);
      setCurrent(shown[Math.max(0, Math.min(shown.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1)))] ?? current);
    } else if (e.key === ' ' && current) { e.preventDefault(); toggle(current); }
  };

  return createPortal(
    <div className="modal-backdrop fixed inset-0 z-[950] flex items-center justify-center bg-black/65 p-4 dark:bg-black/80" onClick={onClose}>
      <div ref={ref} role="dialog" aria-modal="true" aria-labelledby={titleId} onClick={e => e.stopPropagation()} onKeyDown={onKeyDown}
        className="animate-slideUp flex max-h-[88vh] w-full max-w-[500px] flex-col overflow-hidden rounded-3xl border border-[#E3D9CD] bg-white text-[#29231F] shadow-2xl dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100">
        <div className="flex items-center justify-between gap-3 px-6 pt-5">
          <h3 id={titleId} className="inline-flex items-center gap-2 text-base font-bold"><Columns3 size={18} className="text-[#64283A] dark:text-[#D4A847]" />Select Columns to Display</h3>
          <button type="button" onClick={onClose} tabIndex={-1} className={iconButton} aria-label="Close" title="Close (Esc)"><X size={16} /></button>
        </div>

        <div className="flex flex-wrap items-center justify-between gap-2 px-6 pt-4">
          <div className="flex gap-2">
            <button type="button" onClick={() => setTicked(new Set(keys))} className="rounded-full bg-[#64283A]/10 px-4 py-1.5 text-xs font-semibold text-[#64283A] hover:bg-[#64283A]/15 dark:bg-[#D4A847]/15 dark:text-[#D4A847]">Select All</button>
            <button type="button" onClick={() => setTicked(new Set())} className="rounded-full border border-[#E3D9CD] px-4 py-1.5 text-xs font-semibold hover:bg-[#F3EEE6] dark:border-[#2A2A2A] dark:hover:bg-[#1C1C1C]">Deselect All</button>
          </div>
          <div className="relative min-w-40 flex-1 sm:max-w-52">
            <Search size={13} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
            <input value={find} onChange={e => setFind(e.target.value)} placeholder="Search columns…" aria-label="Search columns" className={`${field} pl-8`} />
          </div>
        </div>

        {reorder && <div className="flex flex-wrap items-center gap-1.5 px-6 pt-3 text-[11px] text-stone-500">
          <span className="mr-1">Order:</span>
          <button type="button" onClick={() => move('top')} disabled={index <= 0} className={small}><ArrowUpToLine size={12} />Top</button>
          <button type="button" onClick={() => move('up')} disabled={index <= 0} className={small}><ArrowUp size={12} />Up</button>
          <button type="button" onClick={() => move('down')} disabled={index < 0 || index >= keys.length - 1} className={small}><ArrowDown size={12} />Down</button>
          <button type="button" onClick={() => move('bottom')} disabled={index < 0 || index >= keys.length - 1} className={small}><ArrowDownToLine size={12} />Bottom</button>
        </div>}
        <p className="px-6 pt-2 text-right text-[11px] text-stone-500">{reorder ? 'Click a column to select it' : 'Tick the columns to show'}</p>

        <ul role="listbox" aria-label="Columns" aria-multiselectable className="mx-6 mt-1 min-h-40 flex-1 overflow-y-auto rounded-xl border border-[#EFE9DF] dark:border-[#2A2A2A]">
          {shown.map(k => {
            const on = ticked.has(k);
            return (
              <li key={k} role="option" aria-selected={k === current} onClick={() => setCurrent(k)}
                className={`flex cursor-pointer items-center gap-3 border-b border-[#EFE9DF] px-4 py-2.5 text-sm last:border-0 dark:border-[#1F1F1F] ${k === current ? 'bg-[#64283A]/8 dark:bg-[#D4A847]/10' : 'hover:bg-[#FAF6EF] dark:hover:bg-[#1A1A1A]'}`}>
                <button type="button" role="checkbox" aria-checked={on} aria-label={byKey.get(k)?.label ?? k} onClick={e => { e.stopPropagation(); setCurrent(k); toggle(k); }}
                  className={`flex h-5 w-5 shrink-0 items-center justify-center rounded-full border ${on ? 'border-[#64283A] bg-[#64283A] text-white dark:border-[#D4A847] dark:bg-[#D4A847] dark:text-[#180E09]' : 'border-stone-300 dark:border-stone-600'}`}>
                  {on && <Check size={12} strokeWidth={3} />}
                </button>
                <span className="min-w-0 truncate">{byKey.get(k)?.label ?? k}</span>
              </li>
            );
          })}
          {!shown.length && <li className="px-4 py-6 text-center text-xs text-stone-500">No column matches.</li>}
        </ul>

        <div className="flex items-center justify-between gap-2 px-6 py-5">
          <button type="button" onClick={reset} className={secondaryButton}>Reset to Default</button>
          <div className="flex gap-2">
            <button type="button" onClick={apply} disabled={!ticked.size} className={primaryButton} title={ticked.size ? '' : 'Tick at least one column'}>Apply Changes</button>
            <button type="button" onClick={onClose} className={secondaryButton}>Close</button>
          </div>
        </div>
      </div>
    </div>,
    document.body,
  );
}
