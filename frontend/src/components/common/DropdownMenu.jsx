// Small pop-up menu (e.g. the "⋯ More options" menu) that always stays on screen.
//
// Rendered in a portal with fixed positioning, measured from the trigger button: it opens below the
// button, aligned to its right edge (align="end") or left edge (align="start"), then is shifted so it
// never leaves the viewport (8 px margin) and flips above the button when there is no room below.
// This matters on phones, where a wrapped header can put the trigger at the screen edge.
// Closes on an outside click, Esc, or window resize; the screen keeps the open state.
//
//   const moreRef = useRef<HTMLButtonElement>(null);
//   <button ref={moreRef} onClick={() => setOpen(!open)} aria-expanded={open}>…</button>
//   <DropdownMenu anchorRef={moreRef} open={open} onClose={() => setOpen(false)}>
//     <button className={menuItem} onClick={…}>Print List</button>
//   </DropdownMenu>

import { useEffect, useLayoutEffect, useRef, useState, } from 'react';
import { createPortal } from 'react-dom';
import { useExitGhost } from './useExitGhost';

const MARGIN = 8;   // minimum gap to the screen edge
const GAP = 4;      // gap between the button and the menu

// Class string for the buttons inside a menu.
export const menuItem = 'block w-full rounded-lg px-3 py-2 text-left text-stone-700 hover:bg-subtle focus:bg-subtle focus:outline-none dark:text-stone-200';










export default function DropdownMenu({ anchorRef, open, onClose, children, align = 'end', width = 176 }) {
  const menuRef = useRef(null);
  useExitGhost(menuRef, open);   // the menu fades out when it closes
  const [pos, setPos] = useState(null);

  // Measure after render (the menu's height is only known once it is in the DOM). A position left over
  // from the last time the menu was open is replaced here before the browser paints.
  useLayoutEffect(() => {
    if (!open) return;
    const place = () => {
      const anchor = anchorRef.current;
      if (!anchor) return;
      const r = anchor.getBoundingClientRect();
      const vw = window.innerWidth;
      const vh = window.innerHeight;
      const w = Math.min(width, vw - MARGIN * 2);
      const h = menuRef.current?.offsetHeight ?? 0;
      const preferredLeft = align === 'end' ? r.right - w : r.left;
      const left = Math.min(Math.max(preferredLeft, MARGIN), vw - w - MARGIN);
      const below = r.bottom + GAP;
      const above = r.top - GAP - h;
      const top = below + h > vh - MARGIN && above >= MARGIN ? above : Math.min(below, Math.max(MARGIN, vh - h - MARGIN));
      setPos({ top, left, width: w, up: top < r.top });
    };
    place();
    // Second pass once the menu has rendered and its height is known.
    const raf = requestAnimationFrame(place);
    window.addEventListener('scroll', place, true);
    return () => { cancelAnimationFrame(raf); window.removeEventListener('scroll', place, true); };
  }, [open, anchorRef, align, width]);

  useEffect(() => {
    if (!open) return;
    const onDown = (e) => {
      const t = e.target ;
      if (menuRef.current?.contains(t) || anchorRef.current?.contains(t)) return;
      onClose();
    };
    const onKey = (e) => { if (e.key === 'Escape') { e.stopPropagation(); onClose(); anchorRef.current?.focus(); } };
    document.addEventListener('mousedown', onDown);
    document.addEventListener('touchstart', onDown);
    document.addEventListener('keydown', onKey);
    window.addEventListener('resize', onClose);
    return () => {
      document.removeEventListener('mousedown', onDown);
      document.removeEventListener('touchstart', onDown);
      document.removeEventListener('keydown', onKey);
      window.removeEventListener('resize', onClose);
    };
  }, [open, onClose, anchorRef]);

  if (!open) return null;
  return createPortal(
    <div
      ref={menuRef}
      role="menu"
      style={{ position: 'fixed', top: pos?.top ?? -9999, left: pos?.left ?? -9999, width: pos?.width ?? width, visibility: pos ? 'visible' : 'hidden' }}
      className={`${pos?.up ? 'menu-pop-up' : 'menu-pop'} z-[900] overflow-hidden rounded-xl border border-line bg-surface p-1 text-xs shadow-pop dark:border-[#2A2A2A] dark:bg-[#141414]`}
      onClick={e => { if (e.target.closest('button')) onClose(); }}
    >
      {children}
    </div>,
    document.body,
  );
}
