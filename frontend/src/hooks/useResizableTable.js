// Makes an existing <table> stretchable: every header cell gets a drag handle on its right edge, the table shows its
// outline (cell borders), and the widths are remembered on this computer (localStorage, never the database), so the
// grid opens next time as it was last left. Double-click a handle to go back to the automatic widths.
//
//   const grid = useResizableTable('purchase-bill');
//   <table ref={grid.ref} onMouseDown={grid.onMouseDown} className={`${grid.outline} ...`}> ... </table>
//
// Columns are told apart by their header text, so a grid that shows or hides columns keeps the right widths.
// (For a grid whose columns are known up front, hooks/useGridWidths.js does the same the React way.)

import { useLayoutEffect, useRef } from 'react';

/** Cell borders that draw the table's outline; add to the table's className. */
export const GRID_OUTLINE = 'border-collapse [&_th]:border [&_th]:border-line [&_td]:border [&_td]:border-line-soft';

const MIN = 40, MAX = 700;
const storeKey = (key) => `vlqms_grid_${key}`;
const read = (key) => { try { return JSON.parse(localStorage.getItem(storeKey(key)) || '{}') || {}; } catch { return {}; } };
const write = (key, value) => { try { localStorage.setItem(storeKey(key), JSON.stringify(value)); } catch { /* not remembered */ } };
const idOf = (th) => (th.getAttribute('data-col') || th.textContent || '').trim();

export function useResizableTable(key) {
  const ref = useRef(null);

  // After every render: apply the saved widths and make sure each header cell has its handle.
  useLayoutEffect(() => {
    const table = ref.current;
    if (!table) return;
    const saved = read(key);
    const heads = [...table.querySelectorAll('thead th')];
    heads.forEach(th => {
      th.style.position = 'relative';
      if (saved[idOf(th)]) th.style.width = `${saved[idOf(th)]}px`;
      if (!th.querySelector('[data-grip]')) {
        const grip = document.createElement('span');
        grip.setAttribute('data-grip', '1');
        grip.setAttribute('role', 'separator');
        grip.title = 'Drag to resize · double-click to reset';
        grip.style.cssText = 'position:absolute;top:0;right:0;bottom:0;width:6px;cursor:col-resize;user-select:none;';
        th.appendChild(grip);
      }
    });
    if (heads.some(th => saved[idOf(th)])) {
      table.style.tableLayout = 'fixed';
      table.style.minWidth = '0';
      table.style.width = `${heads.reduce((n, th) => n + (saved[idOf(th)] || th.offsetWidth), 0)}px`;
    }
  });

  const onMouseDown = (e) => {
    const grip = e.target instanceof Element ? e.target.closest('[data-grip]') : null;
    const table = ref.current;
    if (!grip || !table) return;
    e.preventDefault();
    const th = grip.parentElement;
    const heads = [...table.querySelectorAll('thead th')];
    // Freeze every column at its present width so only the dragged one changes.
    const widths = Object.fromEntries(heads.map(h => [idOf(h), h.offsetWidth]));
    heads.forEach(h => { h.style.width = `${widths[idOf(h)]}px`; });
    table.style.tableLayout = 'fixed';
    table.style.minWidth = '0';
    const id = idOf(th), startX = e.clientX, startW = widths[id];
    const total = () => Object.values(widths).reduce((n, w) => n + w, 0);
    table.style.width = `${total()}px`;
    const move = (ev) => {
      widths[id] = Math.max(MIN, Math.min(MAX, Math.round(startW + ev.clientX - startX)));
      th.style.width = `${widths[id]}px`;
      table.style.width = `${total()}px`;
    };
    const up = () => {
      window.removeEventListener('mousemove', move);
      window.removeEventListener('mouseup', up);
      write(key, widths);
    };
    window.addEventListener('mousemove', move);
    window.addEventListener('mouseup', up);
  };

  const onDoubleClick = (e) => {
    if (!(e.target instanceof Element) || !e.target.closest('[data-grip]')) return;
    const table = ref.current;
    write(key, {});
    table?.querySelectorAll('thead th').forEach(h => { h.style.width = ''; });
    if (table) { table.style.tableLayout = ''; table.style.minWidth = ''; table.style.width = ''; }
  };

  return { ref, onMouseDown, onDoubleClick, outline: GRID_OUTLINE };
}
