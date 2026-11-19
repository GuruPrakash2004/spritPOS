// The exe's "Select [Multiple] ..." window (frmUtlMultiSelection): a Sel tick per row, a filter, Select All, Paste (a pasted list of codes or names
// gets ticked: the "Paste Data you want to Select" window with the column to paste) and Ok / Cancel. Ok hands back the keys of the ticked rows.
// Used by Label Printing (items) and Update Item Master Fields (item groups, packing, items).
//   rows        the records to list, in the order to show them
//   rowKey(r)   the unique key of a record (an item code: names repeat)
//   columns     [{ key, label, align? ('right') , render?(row) }] the columns to show next to Sel
//   pasteBy     [{ label, value(row) }] what a pasted line can be matched on (the first is the default); omit to hide Paste
//   chosen      keys that start ticked
//   wide        a large window (default) or a small popup (Import Item Master Fields uses small ones)

import { useMemo, useState } from 'react';
import Modal from './Modal';
import { field, label as labelClass, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const SHOWN = 500;   // rows drawn at a time; the filter narrows the rest

export default function MultiSelectDialog({ title, rows, rowKey, columns, pasteBy, chosen = [], onOk, onClose, wide = true }) {
  const [ticked, setTicked] = useState(() => new Set(chosen));
  const [filter, setFilter] = useState('');
  const [note, setNote] = useState('');
  const [pasteOpen, setPasteOpen] = useState(false);
  const [pasteColumn, setPasteColumn] = useState(0);
  const [pasteText, setPasteText] = useState('');

  const cell = (row, col) => (col.render ? col.render(row) : String(row[col.key] ?? ''));
  const list = useMemo(() => {
    const t = filter.trim().toLowerCase();
    return t ? rows.filter(r => columns.some(c => String(c.render ? c.render(r) : r[c.key] ?? '').toLowerCase().includes(t))) : rows;
  }, [rows, columns, filter]);
  const shown = list.slice(0, SHOWN);
  const allTicked = list.length > 0 && list.every(r => ticked.has(rowKey(r)));
  const toggle = (key) => setTicked(s => { const n = new Set(s); if (n.has(key)) n.delete(key); else n.add(key); return n; });
  const toggleAll = (on) => setTicked(s => { const n = new Set(s); list.forEach(r => (on ? n.add(rowKey(r)) : n.delete(rowKey(r)))); return n; });

  // "Paste Data you want to Select": one value per line (a spreadsheet column pastes that way); the matching rows get ticked.
  const applyPaste = () => {
    const by = pasteBy[pasteColumn];
    const known = new Map(rows.map(r => [String(by.value(r) ?? '').trim().toLowerCase(), rowKey(r)]));
    const found = [], missing = [];
    for (const w of pasteText.split(/[\r\n\t;]+/).map(x => x.trim()).filter(Boolean)) { const k = known.get(w.toLowerCase()); if (k !== undefined) found.push(k); else missing.push(w); }
    setTicked(s => new Set([...s, ...found]));
    setNote(`${found.length} ticked${missing.length ? `, ${missing.length} not found (${missing.slice(0, 3).join(', ')}${missing.length > 3 ? '…' : ''})` : ''}.`);
    setPasteOpen(false);
    setPasteText('');
  };

  return (
    <Modal title={title} onClose={onClose} wide={wide}
      footer={<>
        <label className="mr-auto flex items-center gap-2 text-xs"><input type="checkbox" className="accent-accent" checked={allTicked} onChange={e => toggleAll(e.target.checked)} />Select All</label>
        {pasteBy?.length > 0 && <button type="button" onClick={() => setPasteOpen(true)} className={secondaryButton}>Paste</button>}
        <button type="button" onClick={onClose} className={secondaryButton}>Cancel</button>
        <button type="button" onClick={() => onOk([...ticked])} className={primaryButton}>OK ({ticked.size})</button>
      </>}>
      <input autoFocus value={filter} onChange={e => setFilter(e.target.value)} placeholder="Type to filter" aria-label="Filter" className={`${field} mb-2`} />
      {note && <p role="status" className="mb-2 text-xs text-stone-500">{note}</p>}
      <div className="max-h-[50vh] overflow-auto">
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr><th className="w-12 px-3 py-2">Sel</th>{columns.map(c => <th key={c.key} className={`px-3 py-2 ${c.align === 'right' ? 'text-right' : 'text-left'}`}>{c.label}</th>)}</tr>
          </thead>
          <tbody className={tableBody}>
            {shown.map(r => {
              const key = rowKey(r);
              return (
                <tr key={key} onClick={() => toggle(key)} className={`cursor-pointer hover:bg-accent/10 ${ticked.has(key) ? 'bg-accent/10' : ''}`}>
                  <td className="px-3 py-1.5"><input type="checkbox" className="accent-accent" checked={ticked.has(key)} onChange={() => toggle(key)} onClick={e => e.stopPropagation()} aria-label={`Select ${key}`} /></td>
                  {columns.map(c => <td key={c.key} className={`px-3 py-1.5 ${c.align === 'right' ? 'text-right tabular-nums' : ''}`}>{cell(r, c)}</td>)}
                </tr>
              );
            })}
            {!list.length && <tr><td colSpan={columns.length + 1} className="px-3 py-6 text-center text-stone-500">Nothing matches.</td></tr>}
          </tbody>
        </table>
        {list.length > SHOWN && <p className="px-3 py-2 text-xs text-stone-500">Showing the first {SHOWN} of {list.length}: type in the filter to narrow the list.</p>}
      </div>
      {pasteOpen && (
        <Modal title="Paste Data you want to Select" onClose={() => setPasteOpen(false)}
          footer={<>
            <button type="button" onClick={() => setPasteOpen(false)} className={secondaryButton}>Cancel</button>
            <button type="button" onClick={applyPaste} className={primaryButton}>Ok</button>
          </>}>
          <label className="mb-3 block">
            <span className={labelClass}>Select Colum to Paste</span>
            <select value={pasteColumn} onChange={e => setPasteColumn(Number(e.target.value))} className={field}>
              {pasteBy.map((p, i) => <option key={p.label} value={i}>{p.label}</option>)}
            </select>
          </label>
          <textarea autoFocus value={pasteText} onChange={e => setPasteText(e.target.value)} rows={12} aria-label="Pasted values" placeholder="Paste one value per line" className={`${field} font-mono`} />
        </Modal>
      )}
    </Modal>
  );
}
