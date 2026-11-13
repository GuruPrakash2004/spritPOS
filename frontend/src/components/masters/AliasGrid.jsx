// Item aliases as the exe's "Alias Name" grid (frmMstItem vsfAlias): a one-column list, one alias per row, always at least one row.
// - each alias holds up to 30 characters and no apostrophe (the exe's editor: MaxLength 30, ' is refused);
// - Tab (or Enter) on the last row, once it has text, adds a row below and moves into it (the name stays in the table); Enter never saves the record;
// - removing the current row (the red button at the bottom right, or Ctrl+Delete) asks "Do You want to Remove Alias Name --> X ?" first and keeps one empty row;
// - ↑ / ↓ move between rows. Empty rows are dropped when the record is saved (MasterCrud).
// Read-only (view mode or locked) it shows the aliases as plain lines.

import { useEffect, useRef, useState } from 'react';
import { ALIAS_MAX, cleanAlias } from '../../lib/aliases';

export default function AliasGrid({ id, value, onChange, locked, hasError, confirmRemove }) {
  const rows = Array.isArray(value) && value.length ? value : [''];
  const rowId = (i) => (i === 0 ? id : `${id}-${i}`);
  const [current, setCurrent] = useState(0);   // the row the cursor is in: the one the red button removes
  const focusRow = useRef(null);   // the row to focus after the next render (a row was added)

  useEffect(() => {
    if (focusRow.current !== null) {
      document.getElementById(rowId(focusRow.current))?.focus();
      focusRow.current = null;
    }
  });

  const set = (i, text) => onChange(rows.map((r, j) => (j === i ? cleanAlias(text) : r)));
  const addAfterLast = () => { focusRow.current = rows.length; onChange([...rows, '']); };
  const focus = (i) => document.getElementById(rowId(i))?.focus();

  const remove = async (i) => {
    if (confirmRemove && !(await confirmRemove(rows[i]))) return;
    const next = rows.filter((_, j) => j !== i);
    focusRow.current = Math.max(0, Math.min(i, next.length - 1));
    onChange(next.length ? next : ['']);
  };

  const onKeyDown = (e, i) => {
    const last = i === rows.length - 1;
    const filled = String(rows[i] ?? '').trim() !== '';
    // Enter belongs to the grid: the screen's own Enter handler saves the whole record, which must not happen while aliases are typed.
    if (e.key === 'Enter') {
      e.preventDefault(); e.stopPropagation();
      if (last && filled) addAfterLast(); else if (!last) focus(i + 1);
      return;
    }
    if (e.key === 'Tab' && !e.shiftKey && last && filled) { e.preventDefault(); addAfterLast(); return; }
    if (e.key === 'ArrowDown' && !last) { e.preventDefault(); e.stopPropagation(); focus(i + 1); return; }
    if (e.key === 'ArrowUp' && i > 0) { e.preventDefault(); e.stopPropagation(); focus(i - 1); return; }
    if (e.key === 'Delete' && (e.ctrlKey || e.metaKey)) { e.preventDefault(); void remove(i); }
  };

  if (locked) {
    const shown = rows.filter(r => String(r).trim());
    return <span className="whitespace-pre-line">{shown.join('\n') || '—'}</span>;
  }

  return (
    <div className={`relative overflow-hidden rounded-sm border ${hasError ? 'border-red-500' : 'border-stone-400 dark:border-[#3A3A3A]'}`}>
      <div className="border-b border-stone-400 bg-subtle px-3 py-1 text-center text-xs font-semibold text-stone-800 dark:border-[#3A3A3A] dark:text-stone-100">Alias Name</div>
      {/* like the exe's grid: a fixed-height area, the current row yellow, the unused part grey */}
      <div className="min-h-[8.75rem] bg-stone-300 dark:bg-[#1F1F1F]">
        {rows.map((text, i) => (
          <input key={i} id={rowId(i)} type="text" value={text} maxLength={ALIAS_MAX} autoComplete="off" aria-label={`Alias ${i + 1}`}
            onChange={e => set(i, e.target.value)} onKeyDown={e => onKeyDown(e, i)} onFocus={() => setCurrent(i)}
            data-enter="keep"
            className={`block h-7 w-full border-0 border-b border-stone-400 px-2 text-sm text-stone-900 outline-none dark:border-[#3A3A3A] dark:text-stone-50 ${current === i ? 'bg-[#FFF36B] dark:bg-[#D4A847]/35' : 'bg-white dark:bg-[#141414]'}`} />
        ))}
      </div>
      <button type="button" tabIndex={-1} onClick={() => void remove(Math.min(current, rows.length - 1))} title="Remove the current alias (Ctrl+Delete)" aria-label="Remove alias"
        className="absolute bottom-1.5 right-1.5 rounded-full text-red-600 transition hover:scale-110 hover:text-red-700 dark:text-red-400">
        <svg width="22" height="22" viewBox="0 0 22 22" aria-hidden="true"><circle cx="11" cy="11" r="10" fill="currentColor" /><path d="M7 7l8 8M15 7l-8 8" stroke="#fff" strokeWidth="2.4" strokeLinecap="round" /></svg>
      </button>
    </div>
  );
}
