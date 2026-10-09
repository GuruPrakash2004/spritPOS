import { afterEach, describe, expect, it, vi } from 'vitest';
import { withViewTransition } from './viewTransition';

afterEach(() => { delete document.startViewTransition; });

describe('withViewTransition', () => {
  it('runs the update at once when the browser has no View Transitions (and in tests)', () => {
    const update = vi.fn();
    withViewTransition(update);
    expect(update).toHaveBeenCalledTimes(1);
  });

  it('runs the update at once when animation is switched off for the screen', () => {
    document.startViewTransition = vi.fn();
    const update = vi.fn();
    withViewTransition(update, false);
    expect(update).toHaveBeenCalledTimes(1);
    expect(document.startViewTransition).not.toHaveBeenCalled();
  });
});
