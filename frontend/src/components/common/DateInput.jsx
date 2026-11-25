// The app's date box: replaces the browser's own <input type="date"> everywhere, so every screen shows the same calendar (burgundy / gold theme, Monday first,
// today ringed, today's date written out under the days, Clear / Today links). It is a drop-in: same props, and `onChange` gets `{ target: { value, id, name } }`
// with the date as YYYY-MM-DD ('' when cleared), so a screen only swaps `<input type="date"` for `<DateInput`.
//   value            YYYY-MM-DD or ''            min / max   YYYY-MM-DD limits (days outside are greyed and can't be picked)
//   disabled, readOnly, required, autoFocus, id, name, className (the text box's classes), aria-label, onBlur
// The box shows dd/mm/yyyy and can be typed into (digits; "/" is added as you go, committed on Enter or leaving the box). The calendar button, F4 or Alt+Down open
// the calendar; in it arrows move by day / week, PageUp / PageDown by month, Enter picks, Esc closes (only the calendar, never the dialog behind it), Tab closes.

import { useEffect, useLayoutEffect, useMemo, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { CalendarDays, ChevronLeft, ChevronRight } from 'lucide-react';

const MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
const DAYS = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
const pad = (n) => String(n).padStart(2, '0');
const iso = (y, m, d) => `${y}-${pad(m + 1)}-${pad(d)}`;   // m is 0-based
const todayIso = () => { const t = new Date(); return iso(t.getFullYear(), t.getMonth(), t.getDate()); };
const parts = (s) => { const m = /^(\d{4})-(\d{2})-(\d{2})$/.exec(s || ''); return m ? { y: +m[1], m: +m[2] - 1, d: +m[3] } : null; };
const show = (s) => { const p = parts(s); return p ? `${pad(p.d)}/${pad(p.m + 1)}/${p.y}` : ''; };
const addDays = (s, n) => { const p = parts(s); const t = new Date(p.y, p.m, p.d + n); return iso(t.getFullYear(), t.getMonth(), t.getDate()); };
const addMonths = (s, n) => {
  const p = parts(s); const t = new Date(p.y, p.m + n, 1);
  const last = new Date(t.getFullYear(), t.getMonth() + 1, 0).getDate();
  return iso(t.getFullYear(), t.getMonth(), Math.min(p.d, last));
};
// dd/mm/yyyy (or d/m/yy, or 8 digits) typed by hand -> YYYY-MM-DD, '' when it is not a real date
function parseTyped(text) {
  const t = String(text || '').trim();
  let m = /^(\d{1,2})[/\-.](\d{1,2})[/\-.](\d{2}|\d{4})$/.exec(t);
  if (!m && /^\d{8}$/.test(t)) m = [t, t.slice(0, 2), t.slice(2, 4), t.slice(4)];
  if (!m) return '';
  const d = +m[1], mo = +m[2] - 1, y = m[3].length === 2 ? 2000 + +m[3] : +m[3];
  const dt = new Date(y, mo, d);
  return dt.getFullYear() === y && dt.getMonth() === mo && dt.getDate() === d ? iso(y, mo, d) : '';
}
const longToday = () => new Date().toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' });

export default function DateInput({ value = '', onChange, onBlur, min, max, disabled, readOnly, className = '', id, name, ...rest }) {
  const [draft, setDraft] = useState(null);   // what is being typed; null = show the value
  const [open, setOpen] = useState(false);
  const [view, setView] = useState(value || todayIso());   // a day inside the month on show
  const [focusDay, setFocusDay] = useState(value || todayIso());
  const [pos, setPos] = useState(null);
  const boxRef = useRef(null);
  const inputRef = useRef(null);
  const popRef = useRef(null);
  const locked = disabled || readOnly;

  const inRange = (s) => (!min || s >= min) && (!max || s <= max);
  const emit = (v) => onChange?.({ target: { value: v, id, name } });
  const pick = (s) => { if (!inRange(s)) return; emit(s); setDraft(null); setOpen(false); inputRef.current?.focus(); };
  const commit = () => {
    if (draft === null) return;
    const text = draft.trim();
    if (!text) { if (value) emit(''); } else { const v = parseTyped(text); if (v && inRange(v) && v !== value) emit(v); }
    setDraft(null);
  };
  const openCal = () => {
    if (locked) return;
    const start = value || todayIso();
    setView(start); setFocusDay(start); setOpen(true);
  };

  // place the calendar under the box (above when there is no room), inside the screen
  useLayoutEffect(() => {
    if (!open) return undefined;
    const place = () => {
      const r = boxRef.current?.getBoundingClientRect();
      if (!r) return;
      const w = 288, h = popRef.current?.offsetHeight ?? 360;
      const left = Math.min(Math.max(8, r.left), window.innerWidth - w - 8);
      const below = r.bottom + 4;
      const top = below + h > window.innerHeight - 8 && r.top - h - 4 >= 8 ? r.top - h - 4 : below;
      setPos({ left, top });
    };
    place();
    const raf = requestAnimationFrame(place);
    window.addEventListener('scroll', place, true);
    window.addEventListener('resize', place);
    return () => { cancelAnimationFrame(raf); window.removeEventListener('scroll', place, true); window.removeEventListener('resize', place); };
  }, [open, view]);

  // outside click closes; Esc / Tab inside the calendar are handled before the dialog behind it sees them
  useEffect(() => {
    if (!open) return undefined;
    const down = (e) => { if (!popRef.current?.contains(e.target) && !boxRef.current?.contains(e.target)) setOpen(false); };
    const key = (e) => {
      if (e.key === 'Escape') { e.stopPropagation(); e.preventDefault(); setOpen(false); inputRef.current?.focus(); }
      else if (e.key === 'Tab') { setOpen(false); inputRef.current?.focus(); e.preventDefault(); e.stopPropagation(); }
    };
    document.addEventListener('mousedown', down);
    window.addEventListener('keydown', key, true);
    return () => { document.removeEventListener('mousedown', down); window.removeEventListener('keydown', key, true); };
  }, [open]);

  // keyboard focus follows the highlighted day
  useEffect(() => {
    if (open && pos) popRef.current?.querySelector(`[data-day="${focusDay}"]`)?.focus();
  }, [open, pos, focusDay]);

  const go = (s) => { setFocusDay(s); setView(s); };
  const gridKey = (e) => {
    const moves = { ArrowLeft: () => addDays(focusDay, -1), ArrowRight: () => addDays(focusDay, 1), ArrowUp: () => addDays(focusDay, -7), ArrowDown: () => addDays(focusDay, 7),
      PageUp: () => addMonths(focusDay, -1), PageDown: () => addMonths(focusDay, 1) };
    if (moves[e.key]) { e.preventDefault(); go(moves[e.key]()); }
  };

  const p = parts(view) ?? parts(todayIso());
  const cells = useMemo(() => {
    const first = new Date(p.y, p.m, 1);
    const lead = (first.getDay() + 6) % 7;   // Monday first
    return Array.from({ length: 42 }, (_, i) => { const t = new Date(p.y, p.m, 1 - lead + i); return { s: iso(t.getFullYear(), t.getMonth(), t.getDate()), d: t.getDate(), out: t.getMonth() !== p.m }; });
  }, [p.y, p.m]);
  const today = todayIso();
  const years = useMemo(() => { const lo = Math.min(p.y, 2000), hi = Math.max(p.y, new Date().getFullYear() + 10); return Array.from({ length: hi - lo + 1 }, (_, i) => lo + i); }, [p.y]);
  const step = (n) => setView(addMonths(view, n));
  const nav = 'inline-flex h-8 w-8 items-center justify-center rounded-lg text-[#64283A] hover:bg-[#64283A]/10 dark:text-[#D4A847] dark:hover:bg-[#D4A847]/15';
  const sel = 'rounded-md border-0 bg-transparent py-1 text-sm font-semibold text-[#29231F] outline-none hover:bg-[#64283A]/10 dark:text-stone-100 dark:hover:bg-[#D4A847]/15';

  return (
    <div ref={boxRef} className="relative">
      <input ref={inputRef} id={id} name={name} type="text" inputMode="numeric" autoComplete="off" placeholder="dd/mm/yyyy" maxLength={10}
        value={draft ?? show(value)} disabled={disabled} readOnly={readOnly} className={`${className} !pr-9`}
        onChange={e => setDraft(e.target.value.replace(/[^\d/.-]/g, ''))}
        onBlur={e => { commit(); onBlur?.(e); }}
        onKeyDown={e => {
          if (e.key === 'Enter') commit();
          else if (e.key === 'F4' || (e.altKey && e.key === 'ArrowDown')) { e.preventDefault(); commit(); openCal(); }
        }}
        {...rest} />
      <button type="button" tabIndex={-1} disabled={locked} aria-label="Open calendar" aria-expanded={open} onClick={() => (open ? setOpen(false) : openCal())}
        className="absolute right-2 top-1/2 inline-flex -translate-y-1/2 rounded p-1 text-[#64283A] hover:bg-[#64283A]/10 disabled:opacity-40 dark:text-[#D4A847] dark:hover:bg-[#D4A847]/15">
        <CalendarDays size={15} aria-hidden="true" />
      </button>
      {open && createPortal(
        <div ref={popRef} role="application" aria-label="Calendar" style={{ position: 'fixed', left: pos?.left ?? -9999, top: pos?.top ?? -9999, width: 288, visibility: pos ? 'visible' : 'hidden' }}
          className="menu-pop z-[1100] rounded-2xl border border-line bg-surface p-3 text-[#29231F] shadow-pop dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100">
          <div className="mb-2 flex items-center gap-1">
            <select aria-label="Month" value={p.m} onChange={e => setView(iso(p.y, +e.target.value, 1))} className={sel}>
              {MONTHS.map((n, i) => <option key={n} value={i}>{n}</option>)}
            </select>
            <select aria-label="Year" value={p.y} onChange={e => setView(iso(+e.target.value, p.m, 1))} className={sel}>
              {years.map(y => <option key={y} value={y}>{y}</option>)}
            </select>
            <span className="ml-auto flex">
              <button type="button" aria-label="Previous month" onClick={() => step(-1)} className={nav}><ChevronLeft size={18} /></button>
              <button type="button" aria-label="Next month" onClick={() => step(1)} className={nav}><ChevronRight size={18} /></button>
            </span>
          </div>
          <div className="grid grid-cols-7 text-center text-[11px] font-semibold uppercase tracking-wide text-[#8A7F74]">
            {DAYS.map(d => <span key={d} className="py-1.5">{d}</span>)}
          </div>
          <div className="grid grid-cols-7 gap-y-0.5 text-center text-sm" onKeyDown={gridKey}>
            {cells.map(c => {
              const chosen = c.s === value, isToday = c.s === today, off = !inRange(c.s);
              return (
                <button key={c.s} type="button" data-day={c.s} tabIndex={c.s === focusDay ? 0 : -1} disabled={off} aria-label={c.s} aria-pressed={chosen}
                  onClick={() => pick(c.s)}
                  className={`mx-auto flex h-9 w-9 items-center justify-center rounded-full tabular-nums outline-none transition-colors focus-visible:ring-2 focus-visible:ring-[#B08A45] disabled:cursor-not-allowed disabled:opacity-30
                    ${chosen ? 'bg-[#64283A] font-semibold text-white dark:bg-[#D4A847] dark:text-[#141414]'
                      : `${c.out ? 'text-[#A69D91] dark:text-stone-500' : ''} ${isToday ? 'font-semibold ring-1 ring-inset ring-[#B08A45] text-[#64283A] dark:text-[#D4A847]' : ''} hover:bg-[#64283A]/10 dark:hover:bg-[#D4A847]/15`}`}>
                  {c.d}
                </button>
              );
            })}
          </div>
          <div className="mt-2 flex items-center justify-between border-t border-line pt-2 text-sm font-medium text-[#64283A] dark:text-[#D4A847]">
            <button type="button" disabled={!value || rest.required} onClick={() => { emit(''); setDraft(null); setOpen(false); inputRef.current?.focus(); }} className="rounded px-2 py-1 hover:bg-[#64283A]/10 disabled:opacity-30 dark:hover:bg-[#D4A847]/15">Clear</button>
            <button type="button" disabled={!inRange(today)} onClick={() => pick(today)} className="rounded px-2 py-1 hover:bg-[#64283A]/10 disabled:opacity-30 dark:hover:bg-[#D4A847]/15">Today</button>
          </div>
          <p className="mt-1 text-center text-xs text-[#70675F] dark:text-stone-400">Today: <b className="font-semibold text-[#29231F] dark:text-stone-200">{longToday()}</b></p>
        </div>,
        document.body)}
    </div>
  );
}
