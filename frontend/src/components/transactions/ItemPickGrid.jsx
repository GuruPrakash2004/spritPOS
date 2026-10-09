// A Windows-style item list: a header row (ItemCode, ShortName, HotKey, ItemName, ...), one row per item, the
// highlighted row filled, scrolling inside a fixed height. Used by the Sales Invoice search dropdown and the Purchase
// Bill "Add Item" window, so every item picker looks and behaves the same.
//   <ItemPickGrid id="x" items={list} at={2} onPick={item => ...} />
// The caller keeps the highlight (`at`) and moves it with the keyboard; a mouse click picks a row. The highlighted
// row is scrolled into view. `showStock` / `showRate` add the QOH and rate columns.

import { useEffect, useRef } from 'react';
import { money } from '../../lib/styles';

const cell = 'truncate px-2 py-1.5';

export default function ItemPickGrid({ id, items, at, onPick, showStock = false, showRate = false, className = 'max-h-72', empty = 'No items match.' }) {
  const box = useRef(null);
  useEffect(() => {
    box.current?.querySelector('[aria-selected="true"]')?.scrollIntoView({ block: 'nearest' });
  }, [at, items]);

  // On a phone the ShortName and HotKey columns are left out so the code, name, stock and rate stay readable.
  const narrow = typeof window !== 'undefined' && !!window.matchMedia?.('(max-width: 639.98px)').matches;
  // An inline style, not a Tailwind class built from pieces (Tailwind only generates class names it can read whole).
  const cols = { gridTemplateColumns: narrow
    ? `minmax(0,6rem) minmax(0,1fr)${showStock ? ' 3rem' : ''}${showRate ? ' 4.5rem' : ''}`
    : `minmax(0,1.1fr) minmax(0,1.3fr) minmax(0,0.6fr) minmax(0,2.6fr)${showStock ? ' 4.5rem' : ''}${showRate ? ' 5.5rem' : ''}` };
  return (
    <div id={id} role="listbox" aria-label="Items" ref={box} className={`overflow-auto border border-line bg-surface text-xs ${className}`}>
      <div role="presentation" style={cols} className="sticky top-0 z-10 grid border-b border-line bg-[#F0F3F8] font-semibold text-stone-700 dark:bg-[#1E1E1E] dark:text-stone-200">
        <span className={cell}>ItemCode</span>{!narrow && <span className={cell}>ShortName</span>}{!narrow && <span className={cell}>HotKey</span>}<span className={cell}>ItemName</span>
        {showStock && <span className={`${cell} text-right`}>QOH</span>}
        {showRate && <span className={`${cell} text-right`}>Rate</span>}
      </div>
      {items.map((item, i) => (
        <div key={item.code} id={`${id}-${i}`} role="option" aria-selected={i === at}
          onMouseDown={e => { e.preventDefault(); onPick(item); }}
          style={cols}
          className={`grid cursor-pointer border-b border-line-soft ${i === at ? 'bg-[#BBD7F0] text-stone-900 dark:bg-[#27405A] dark:text-stone-50' : 'text-stone-700 hover:bg-subtle dark:text-stone-200'}`}>
          <span className={`${cell} font-mono`}>{item.code}</span>
          {!narrow && <span className={cell}>{item.shortName || item.name}</span>}
          {!narrow && <span className={cell}>{item.hotKey}</span>}
          <span className={cell}>{item.name}</span>
          {showStock && <span className={`${cell} text-right tabular-nums`}>{item.clQty}</span>}
          {showRate && <span className={`${cell} text-right tabular-nums`}>{money(item.rate)}</span>}
        </div>
      ))}
      {!items.length && <p className="px-3 py-6 text-center text-stone-500">{empty}</p>}
    </div>
  );
}
