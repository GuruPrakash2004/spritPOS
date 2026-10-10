// Column widths of a data grid that the user can drag, remembered per computer (localStorage, so each till/PC
// reopens its grid the way it was last left). `defaults` maps a column id to its starting width in px.
//   const { widths, startResize, reset } = useGridWidths('sales-invoice', { item: 260, qty: 70 });
// Put `style={{ width: widths.item }}` on the header cells (table-fixed) and a drag handle calling
// startResize('item') on mouse/touch down; reset() goes back to the defaults.

import { useCallback, useRef, useState } from 'react';

const MIN = 44, MAX = 600;
const storeKey = (key) => `vlqms_grid_${key}`;
const read = (key) => { try { return JSON.parse(localStorage.getItem(storeKey(key)) || '{}') || {}; } catch { return {}; } };
const write = (key, value) => { try { localStorage.setItem(storeKey(key), JSON.stringify(value)); } catch { /* storage blocked: not remembered */ } };

export function useGridWidths(key, defaults) {
  const [saved, setSaved] = useState(() => read(key));
  const live = useRef(saved);
  const widths = Object.fromEntries(Object.entries(defaults).map(([id, w]) => [id, Number(saved[id]) || w]));

  const startResize = useCallback((id) => (e) => {
    e.preventDefault();
    e.stopPropagation();
    const startX = e.touches ? e.touches[0].clientX : e.clientX;
    const startW = Number(live.current[id]) || defaults[id];
    const move = (ev) => {
      const x = ev.touches ? ev.touches[0].clientX : ev.clientX;
      const next = { ...live.current, [id]: Math.max(MIN, Math.min(MAX, Math.round(startW + x - startX))) };
      live.current = next;
      setSaved(next);
    };
    const up = () => {
      window.removeEventListener('mousemove', move); window.removeEventListener('mouseup', up);
      window.removeEventListener('touchmove', move); window.removeEventListener('touchend', up);
      write(key, live.current);
    };
    window.addEventListener('mousemove', move); window.addEventListener('mouseup', up);
    window.addEventListener('touchmove', move); window.addEventListener('touchend', up);
  }, [key, defaults]);

  const reset = useCallback(() => { live.current = {}; setSaved({}); write(key, {}); }, [key]);
  return { widths, startResize, reset };
}
