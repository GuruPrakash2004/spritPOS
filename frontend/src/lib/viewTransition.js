// Screen changes with the browser's View Transitions API: the old screen fades out and the new one rises in
// (the `screen` transition in index.css). Everything falls back to a plain, instant update when the browser has
// no support, when the user asked for reduced motion, or when `animate` is false (billing and data-entry screens:
// a cashier who opens one and starts typing is never held up by an animation).

import { flushSync } from 'react-dom';

const reducedMotion = () => typeof window !== 'undefined' && !!window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;

/** Run `update` (state changes that switch the screen), cross-fading when the browser can. */
export function withViewTransition(update, animate = true) {
  const start = typeof document !== 'undefined' ? document.startViewTransition : undefined;
  if (!animate || !start || reducedMotion() || import.meta.env.MODE === 'test') { update(); return; }
  try {
    document.startViewTransition(() => { flushSync(update); });
  } catch {
    update();
  }
}
