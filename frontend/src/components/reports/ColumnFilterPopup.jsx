// The column filter of the Stock Balance result grid: the exe's vsfView has AllowFiltering = True (C1FlexGrid), so every heading carries a filter
// button that opens a window with a condition (contains / equals / begins with ... or, for numbers, = > < ...) and a tick list of the column's
// distinct values (as the exe's: (Select All), the value list, then Text Filter / Apply / Clear / Cancel). Rows that fail any column's filter are hidden. The filter model (`passes`) is in lib/columnFilter.js; this file is the window.
//   filter   { op, text, values } for one column: `values` null = every value, else the ticked (displayed) values; `op`/`text` the condition
//   (a blank `text` means no condition)

import { useEffect, useMemo, useRef, useState } from 'react';
import { field } from '../../lib/styles';
import { NUMBER_OPS, TEXT_OPS } from '../../lib/columnFilter';

export default function ColumnFilterPopup({ label, isNumber, distinct, filter, left, top, onApply, onClose }) {
  const ops = isNumber ? NUMBER_OPS : TEXT_OPS;
  const [op, setOp] = useState(filter?.op ?? ops[0][0]);
  const [text, setText] = useState(filter?.text ?? '');
  const [showText, setShowText] = useState(!!filter?.text);   // the exe's "Text Filter" drop-down
  const [ticked, setTicked] = useState(() => new Set(filter?.values ?? []));   // nothing ticked = no list filter, as in the exe
  const [find, setFind] = useState('');
  const ref = useRef(null);
  const firstRef = useRef(null);

  useEffect(() => { firstRef.current?.focus(); }, []);
  useEffect(() => {
    const away = (e) => { if (ref.current && !ref.current.contains(e.target)) onClose(); };
    document.addEventListener('mousedown', away);
    return () => document.removeEventListener('mousedown', away);
  }, [onClose]);

  const listed = useMemo(() => distinct.filter(v => v.toLowerCase().includes(find.trim().toLowerCase())), [distinct, find]);
  const allOn = listed.length > 0 && listed.every(v => ticked.has(v));
  const toggleAll = () => setTicked(prev => { const n = new Set(prev); listed.forEach(v => (allOn ? n.delete(v) : n.add(v))); return n; });
  const toggle = (v) => setTicked(prev => { const n = new Set(prev); if (n.has(v)) n.delete(v); else n.add(v); return n; });
  const apply = () => {
    const values = ticked.size ? distinct.filter(v => ticked.has(v)) : null;
    const t = showText ? text.trim() : '';
    onApply(!values && !t ? null : { op, text: t, values });
  };
  const keys = (e) => {
    if (e.key === 'Escape') { e.stopPropagation(); e.preventDefault(); onClose(); }
    else if (e.key === 'Enter' && e.target.tagName !== 'BUTTON') { e.preventDefault(); apply(); }
  };
  const link = 'inline-flex items-center gap-1 rounded px-2 py-1 font-medium hover:bg-subtle';

  return (
    <div ref={ref} role="group" aria-label={`Filter ${label}`} onKeyDown={keys} style={{ left, top }}
      className="absolute z-[20] w-72 rounded-lg border border-line bg-surface p-2 text-xs text-[#29231F] shadow-2xl dark:text-stone-100">
      <input ref={firstRef} aria-label="Find a value" value={find} onChange={e => setFind(e.target.value)} placeholder="Search values" className={`${field} mb-1 !py-1 text-xs`} />
      <div className="max-h-52 overflow-auto rounded border border-line">
        <label className="flex cursor-pointer items-center gap-2 border-b border-line bg-subtle px-2 py-1">
          <input type="checkbox" checked={allOn} onChange={toggleAll} />(Select All)
        </label>
        {listed.slice(0, 300).map(v => (
          <label key={v} className="flex cursor-pointer items-center gap-2 px-2 py-1 hover:bg-[#1E66D0] hover:text-white">
            <input type="checkbox" checked={ticked.has(v)} onChange={() => toggle(v)} /><span className="truncate">{v === '' ? '(Blanks)' : v}</span>
          </label>
        ))}
        {listed.length > 300 && <p className="px-2 py-1 text-stone-500">Showing 300 of {listed.length}: search to narrow.</p>}
        {!listed.length && <p className="px-2 py-1 text-stone-500">No values match.</p>}
      </div>
      {showText && (
        <div className="mt-2 flex gap-1.5">
          <select aria-label="Condition" value={op} onChange={e => setOp(e.target.value)} className={`${field} !w-auto !py-1 text-xs`}>
            {ops.map(([k, n]) => <option key={k} value={k}>{n}</option>)}
          </select>
          <input aria-label="Condition value" value={text} onChange={e => setText(e.target.value)} placeholder="Value" className={`${field} !py-1 text-xs`} />
        </div>
      )}
      <div className="mt-2 flex items-center justify-between border-t border-line pt-2">
        <button type="button" className={link} aria-expanded={showText} onClick={() => setShowText(v => !v)}><b>B</b> Text Filter <span aria-hidden>{showText ? '▴' : '▾'}</span></button>
        <span className="flex gap-1">
          <button type="button" className={link} onClick={apply}>Apply</button>
          <button type="button" className={link} onClick={() => onApply(null)}>Clear</button>
          <button type="button" className={`${link} text-red-600`} onClick={onClose}>✕ Cancel</button>
        </span>
      </div>
    </div>
  );
}
