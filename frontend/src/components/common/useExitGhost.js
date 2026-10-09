// Gives a pop-up (dialog, menu) an exit animation although its owner simply stops rendering it: when the element
// goes away, a non-interactive copy of it stays on screen for a moment and fades out (`.modal-ghost` in index.css).
// The real element is gone at once, so focus, keys and state behave exactly as before; the copy is aria-hidden,
// inert and has no ids. Skipped for reduced motion and in tests.

import { useEffect } from 'react';

const GHOST_MS = 180;

export function useExitGhost(ref, active = true) {
  useEffect(() => {
    const el = ref.current;
    if (!el || !active || import.meta.env.MODE === 'test') return undefined;
    return () => {
      if (window.matchMedia?.('(prefers-reduced-motion: reduce)').matches) return;
      const ghost = el.cloneNode(true);
      ghost.removeAttribute('id');
      ghost.querySelectorAll('[id]').forEach(n => n.removeAttribute('id'));
      ghost.setAttribute('aria-hidden', 'true');
      ghost.inert = true;
      ghost.classList.remove('modal-backdrop', 'menu-pop', 'menu-pop-up');
      ghost.classList.add('modal-ghost');
      document.body.appendChild(ghost);
      setTimeout(() => ghost.remove(), GHOST_MS + 40);
    };
  }, [ref, active]);
}
