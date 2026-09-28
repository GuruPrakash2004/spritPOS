// Popup (snackbar) state, shared by the whole app.
//
// Call showToast(kind, text) or toast.ok/error(...) from anywhere; <SnackbarContainer /> in App.tsx
// renders them. Rules (see .claude/rules/popups-and-errors.md):
// - every popup closes after toastDuration(): 1.5 s on large screens (lg, 1024 px wide and up), 3 s on
//   smaller ones; hovering pauses the countdown;
// - the same message again shakes the existing card and shows ×2, ×3 instead of stacking;
// - at most 3 cards are shown.
// Use popups for server results only; form mistakes are shown inline (FormField / InlineError).

export type ToastKind = 'ok' | 'error' | 'delete' | 'refresh' | 'backup';

export const TOAST_DURATION_MS = 3000;          // phones, tablets and smaller laptops
export const TOAST_DURATION_LARGE_MS = 1500;    // large screens
export const LARGE_SCREEN_QUERY = '(min-width: 1024px)';   // Tailwind's lg breakpoint

// How long a new popup stays, by the screen width when it opens.
export const toastDuration = () =>
  typeof window !== 'undefined' && window.matchMedia?.(LARGE_SCREEN_QUERY).matches ? TOAST_DURATION_LARGE_MS : TOAST_DURATION_MS;

// How long a closing card animates out before it is removed (matches toastExit in index.css):
// a softer, slower exit below lg.
export const TOAST_EXIT_MS = 220;
export const TOAST_EXIT_SMALL_MS = 380;
const exitDuration = () =>
  typeof window !== 'undefined' && window.matchMedia?.(LARGE_SCREEN_QUERY).matches ? TOAST_EXIT_MS : TOAST_EXIT_SMALL_MS;

export interface ToastItem {
  id: string;
  kind: ToastKind;
  text: string;
  count: number;
  shakeKey: number;
  duration: number; // toastDuration(): 1500 on large screens, 3000 below
  remaining: number;
  paused: boolean;
  leaving: boolean; // playing its exit animation; removed when it ends
}

// The countdown runs on the browser's frame clock (requestAnimationFrame), so the bar moves every frame
// instead of in 50 ms steps. Falls back to ~60 fps timeouts where rAF is missing.
const nextFrame = (fn: () => void) =>
  typeof requestAnimationFrame === 'function' ? requestAnimationFrame(fn) : window.setTimeout(fn, 16);
const cancelFrame = (id: number) =>
  typeof cancelAnimationFrame === 'function' ? cancelAnimationFrame(id) : clearTimeout(id);

class ToastStore {
  private items: ToastItem[] = [];
  private listeners = new Set<() => void>();
  private frameId: number | null = null;
  private lastTick = Date.now();

  private emit() {
    this.listeners.forEach(fn => fn());
  }

  subscribe = (listener: () => void) => {
    this.listeners.add(listener);
    return () => {
      this.listeners.delete(listener);
    };
  };

  getSnapshot = (): ToastItem[] => {
    return this.items;
  };

  private tick = () => {
    this.frameId = null;
    const now = Date.now();
    const delta = now - this.lastTick;
    this.lastTick = now;

    let changed = false;
    const expired: string[] = [];
    const nextItems = this.items.map(item => {
      if (item.duration <= 0 || item.paused || item.leaving) return item;
      changed = true;
      const remaining = Math.max(0, item.remaining - delta);
      if (remaining <= 0) expired.push(item.id);
      return { ...item, remaining };
    });

    if (changed) {
      this.items = nextItems;
      this.emit();
    }
    expired.forEach(id => this.remove(id));

    if (this.items.some(i => i.duration > 0 && !i.paused && !i.leaving)) {
      this.frameId = nextFrame(this.tick);
    }
  };

  private startTimer() {
    if (this.frameId !== null) return;
    this.lastTick = Date.now();
    this.frameId = nextFrame(this.tick);
  }

  private stopTimer() {
    if (this.frameId !== null) {
      cancelFrame(this.frameId);
      this.frameId = null;
    }
  }

  add(kind: ToastKind = 'ok', text: string) {
    if (!text || !text.trim()) return;
    const cleanText = text.trim();

    // Every popup (success or error) closes automatically after toastDuration().
    const isError = kind === 'error' || /\b(already exists|error|failed|invalid|required)\b/i.test(cleanText);
    const duration = toastDuration();

    // Check if an existing toast matches (one already closing counts as gone)
    const existingIndex = this.items.findIndex(i => i.text === cleanText && i.kind === kind && !i.leaving);
    if (existingIndex !== -1) {
      const existing = this.items[existingIndex];
      const updated: ToastItem = {
        ...existing,
        count: existing.count + 1,
        shakeKey: existing.shakeKey + 1,
        remaining: duration, // reset countdown
        duration,
      };
      const copy = [...this.items];
      copy[existingIndex] = updated;
      this.items = copy;
      this.emit();
      this.startTimer();
      return;
    }

    // New toast item
    const newItem: ToastItem = {
      id: `toast-${Date.now()}-${Math.random().toString(36).slice(2, 6)}`,
      kind: isError ? 'error' : kind,
      text: cleanText,
      count: 1,
      shakeKey: 0,
      duration,
      remaining: duration,
      paused: false,
      leaving: false,
    };

    // Keep max 3 stacked
    let nextList = [...this.items, newItem];
    if (nextList.length > 3) {
      nextList = nextList.slice(nextList.length - 3);
    }
    this.items = nextList;
    this.emit();
    this.startTimer();
  }

  // Close a card: it plays its exit animation first, then leaves the list.
  remove(id: string) {
    const item = this.items.find(i => i.id === id);
    if (!item || item.leaving) return;
    const reduceMotion = typeof window !== 'undefined' && window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
    if (reduceMotion) return this.drop(id);
    this.items = this.items.map(i => (i.id === id ? { ...i, leaving: true } : i));
    this.emit();
    window.setTimeout(() => this.drop(id), exitDuration());
  }

  private drop(id: string) {
    this.items = this.items.filter(i => i.id !== id);
    this.emit();
    if (!this.items.some(i => i.duration > 0 && !i.paused && !i.leaving)) {
      this.stopTimer();
    }
  }

  setPaused(id: string, paused: boolean) {
    let changed = false;
    this.items = this.items.map(item => {
      if (item.id === id && item.paused !== paused) {
        changed = true;
        return { ...item, paused };
      }
      return item;
    });
    if (changed) {
      this.emit();
      if (!paused) {
        this.startTimer();
      }
    }
  }
}

export const toastStore = new ToastStore();

// Show a popup from anywhere, e.g. `toast.ok('Item saved')` or `toast.error(errorText(e))`.
export const showToast = (kind: ToastKind, text: string) => toastStore.add(kind, text);
export const toast = {
  ok: (text: string) => toastStore.add('ok', text),
  error: (text: string) => toastStore.add('error', text),
  delete: (text: string) => toastStore.add('delete', text),
  refresh: (text: string) => toastStore.add('refresh', text),
  backup: (text: string) => toastStore.add('backup', text),
};
