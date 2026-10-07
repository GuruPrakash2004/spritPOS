// Keyboard behaviour shared by every dialog (Modal, the print dialog):
// - on open, focus goes into the dialog: an element with autoFocus / data-autofocus, else the first field,
//   else the dialog's main (last) footer button, so Enter does the obvious thing;
// - Tab and Shift+Tab stay inside the dialog;
// - Esc closes only the top dialog (a print dialog opened from Re-Print closes first);
// - on close, focus goes back to where it was (e.g. the bill's item search).

import { useEffect, useRef, } from 'react';
import { visible } from '../../lib/keys';

const TABBABLE = 'a[href], button:not([disabled]), input:not([disabled]):not([type="hidden"]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])';
const FIELDS = 'input:not([disabled]):not([readonly]):not([type="hidden"]):not([type="checkbox"]), select:not([disabled]), textarea:not([disabled])';


export const isTopDialog = (el) => {
  const all = document.querySelectorAll('[role="dialog"]');
  return !!el && all[all.length - 1] === el;
};

export function useDialogKeys(ref, onClose) {
  const close = useRef(onClose);
  useEffect(() => { close.current = onClose; });

  useEffect(() => {
    const root = ref.current;
    if (!root) return;
    const before = document.activeElement ;

    if (!root.contains(document.activeElement)) {
      const first = root.querySelector('[data-autofocus]')
        ?? Array.from((root.querySelector('[data-dialog-body]') ?? root).querySelectorAll(FIELDS)).find(visible)
        ?? Array.from(root.querySelectorAll('[data-dialog-footer] button:not([disabled])')).filter(visible).at(-1)
        ?? Array.from(root.querySelectorAll(TABBABLE)).find(el => el.tabIndex >= 0 && visible(el));
      first?.focus();
      if (first instanceof HTMLInputElement) first.select();
    }

    const onKey = (e) => {
      if (!isTopDialog(root)) return;
      if (e.key === 'Escape') {
        e.preventDefault();
        close.current();
        return;
      }
      if (e.key !== 'Tab') return;
      const list = Array.from(root.querySelectorAll(TABBABLE)).filter(el => el.tabIndex >= 0 && visible(el));
      if (!list.length) return;
      const at = list.indexOf(document.activeElement );
      if (e.shiftKey && at <= 0) { e.preventDefault(); list.at(-1).focus(); }
      else if (!e.shiftKey && (at === -1 || at === list.length - 1)) { e.preventDefault(); list[0].focus(); }
    };
    window.addEventListener('keydown', onKey);
    return () => {
      window.removeEventListener('keydown', onKey);
      // Give focus back once the dialog has gone (a field the next dialog autofocuses keeps it).
      setTimeout(() => {
        if (before && document.contains(before) && (document.activeElement === document.body || !document.activeElement)) before.focus();
      }, 0);
    };
  }, [ref]);
}
