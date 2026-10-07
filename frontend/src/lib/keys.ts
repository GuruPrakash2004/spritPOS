// Keyboard helpers for the counter screens (billing is run from the keyboard; the mouse is the exception).
//
// - useShortcuts(map): screen-level shortcuts such as F10 or Ctrl+S, ignored while a dialog is open
//   (a dialog has its own keys) and while a key repeats. Keys are written "F10", "Mod+S" (⌘ on a Mac,
//   Ctrl elsewhere), "Alt+R" (matched on the physical key, so ⌥R on a Mac works too), "Shift+F6".
// - enterMovesNext: an onKeyDown for a form or grid wrapper. Enter in a field moves to the next field,
//   like the desktop vLQMS, and Shift+Enter moves back; on the last field of a filled grid row it calls onLast
//   (add a row). Fields marked data-enter="keep" handle Enter themselves.
// - focusField(id): focus and select a field by id.
// Waiting popups take Enter (do it now) and Esc (cancel) first: see components/common/Toast.tsx.

import { useEffect, useRef, type KeyboardEvent as ReactKeyboardEvent } from 'react';

export const isMac = typeof navigator !== 'undefined' && /Mac|iPod|iPhone|iPad/.test(navigator.platform);

// "Mod+S" written for this computer: "⌘S" on a Mac, "Ctrl+S" elsewhere.
export const keyLabel = (combo: string) => combo
  .replace('Mod+', isMac ? '⌘' : 'Ctrl+')
  .replace('Alt+', isMac ? '⌥' : 'Alt+');

export const isTypingTarget = (el: EventTarget | null) => {
  if (!(el instanceof HTMLElement)) return false;
  return el.isContentEditable || ['INPUT', 'TEXTAREA', 'SELECT'].includes(el.tagName);
};

export const dialogOpen = () => !!document.querySelector('[role="dialog"]');

// Whether a keydown matches "Mod+Shift+S", "F10", "Alt+R", "Delete", ...
export function matches(e: KeyboardEvent | ReactKeyboardEvent, combo: string) {
  const parts = combo.split('+');
  const key = parts.pop()!;
  const want = { mod: parts.includes('Mod'), alt: parts.includes('Alt'), shift: parts.includes('Shift') };
  const mod = isMac ? e.metaKey : e.ctrlKey;
  if (mod !== want.mod || e.altKey !== want.alt || e.shiftKey !== want.shift) return false;
  if (!isMac && e.metaKey) return false;
  if (key.length === 1 && /[A-Z0-9]/i.test(key)) {
    // The physical key: ⌥R on a Mac types "®", so e.key alone isn't enough.
    return e.code === (/\d/.test(key) ? `Digit${key}` : `Key${key.toUpperCase()}`);
  }
  return e.key.toLowerCase() === key.toLowerCase();
}

// Screen shortcuts. The handlers can change on every render; the listener is installed once.
export function useShortcuts(map: Record<string, (e: KeyboardEvent) => void>, enabled = true) {
  const ref = useRef(map);
  useEffect(() => { ref.current = map; });
  useEffect(() => {
    if (!enabled) return;
    const onKey = (e: KeyboardEvent) => {
      if (e.defaultPrevented || e.repeat || dialogOpen()) return;
      for (const [combo, run] of Object.entries(ref.current)) {
        if (matches(e, combo)) {
          e.preventDefault();
          run(e);
          return;
        }
      }
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [enabled]);
}

export function focusField(id: string) {
  const el = document.getElementById(id) as HTMLInputElement | null;
  el?.focus();
  if (el && typeof el.select === 'function' && el.type !== 'date') el.select();
}

const FOCUSABLE = 'input:not([disabled]):not([type="hidden"]):not([readonly]), select:not([disabled]), textarea:not([disabled])';

// Shown on screen (not inside display:none / a closed section). Browsers without checkVisibility count all.
export const visible = (el: HTMLElement) => (typeof el.checkVisibility === 'function'
  ? el.checkVisibility()
  : !el.closest('[hidden], .hidden'));

// The fields of a container in screen order (hidden ones skipped).
export const fieldsIn = (root: HTMLElement) =>
  Array.from(root.querySelectorAll<HTMLElement>(FOCUSABLE)).filter(el => visible(el) || el === document.activeElement);

// onKeyDown for a form/grid wrapper: Enter moves to the next field, Shift+Enter to the previous one.
// Returns false at the last field (the caller may then save or add a row).
export function enterMovesNext(e: ReactKeyboardEvent<HTMLElement>, onLast?: () => void) {
  if (e.key !== 'Enter' || e.ctrlKey || e.metaKey || e.altKey || e.defaultPrevented) return;
  const target = e.target as HTMLElement;
  if (!isTypingTarget(target) || target.tagName === 'TEXTAREA' || target.dataset.enter === 'keep') return;
  const list = fieldsIn(e.currentTarget);
  const at = list.indexOf(target);
  if (at === -1) return;
  e.preventDefault();
  const next = list[at + (e.shiftKey ? -1 : 1)];
  if (next) {
    next.focus();
    if (next instanceof HTMLInputElement && next.type !== 'date') next.select();
  } else if (!e.shiftKey && onLast) {
    // Only from a row with something in it, so holding Enter doesn't pile up blank rows.
    const row = target.closest('tr');
    const filled = !row || Array.from(row.querySelectorAll<HTMLInputElement>('input, select'))
      .some(f => f.type !== 'checkbox' && f.value !== '' && f.value !== '0');
    if (!filled) return;
    onLast();
    // A row was added: move into its first field once it is on screen.
    const root = e.currentTarget;
    setTimeout(() => {
      const after = fieldsIn(root);
      const first = after[after.indexOf(target) + 1];
      first?.focus();
      if (first instanceof HTMLInputElement && first.type !== 'date') first.select();
    }, 0);
  }
}

// Move to the next field of the same form/dialog/screen (after a pick, so the cashier never reaches for the mouse).
export function advanceFrom(el: HTMLElement) {
  const root = (el.closest('form, [role="dialog"], main, section') as HTMLElement | null) ?? document.body;
  const list = fieldsIn(root);
  const next = list[list.indexOf(el) + 1];
  if (!next) return;
  next.focus();
  if (next instanceof HTMLInputElement && next.type !== 'date') next.select();
}

// Picking an option in a drop-down (<select>) with the mouse, or Enter, moves straight to the next field.
// Arrow keys and typing letters change a closed drop-down one value at a time, so those never jump.
// A field can opt out with data-advance="off". Mounted once in App.tsx.
export function useAdvanceAfterSelect() {
  useEffect(() => {
    let lastKey = '';
    const onKey = (e: KeyboardEvent) => { lastKey = e.key; };
    const onPointer = () => { lastKey = ''; };
    const onChange = (e: Event) => {
      const el = e.target;
      if (!(el instanceof HTMLSelectElement) || el.multiple || el.dataset.advance === 'off') return;
      const stepping = lastKey.startsWith('Arrow') || (lastKey.length === 1 && lastKey !== ' ') || ['Home', 'End', 'PageUp', 'PageDown'].includes(lastKey);
      lastKey = '';
      if (stepping) return;
      setTimeout(() => advanceFrom(el), 0);
    };
    window.addEventListener('keydown', onKey, true);
    window.addEventListener('pointerdown', onPointer, true);
    document.addEventListener('change', onChange, true);
    return () => { window.removeEventListener('keydown', onKey, true); window.removeEventListener('pointerdown', onPointer, true); document.removeEventListener('change', onChange, true); };
  }, []);
}
