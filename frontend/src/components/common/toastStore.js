// Popup (snackbar) state, shared by the whole app.
//
// Call showToast(kind, text) or toast.ok/error(...) from anywhere; <SnackbarContainer /> in App.jsx
// renders them. Rules (see .claude/rules/popups-and-errors.md):
// - every popup closes after toastDuration(): 1.5 s on large screens (lg, 1024 px wide and up), 2 s on
//   smaller ones; hovering pauses the countdown;
// - the same message again shakes the existing card and shows ×2, ×3 instead of stacking;
// - at most 3 cards are shown.
// Use popups for server results only; form mistakes are shown inline (FormField / InlineError).
//
// Waiting popups (toastStore.defer / toast.later): every user action (save, delete, refresh, hold, upload,
// export, email, backup, ...) waits for its popup. It shows a Cancel button and stays the same toastDuration()
// as any popup (hover pauses it) so the user has time to decide; the promise resolves true when the popup
// closes (countdown ends or ✕), false if Cancel was pressed. Pattern:
//   if (!(await toast.later('ok', 'Bill will be saved when this closes.'))) return;
//   ...then call the API.
// Keyboard (counter staff rarely use the mouse): while a waiting popup shows, Enter runs its action now and
// Esc cancels it (the newest one first); Toast.jsx listens for these keys before any screen does.

 

export const TOAST_DURATION_MS = 2000;          // phones, tablets and smaller laptops
export const TOAST_DURATION_LARGE_MS = 1500;    // large screens
export const LARGE_SCREEN_QUERY = '(min-width: 1024px)';   // Tailwind's lg breakpoint

// How long a new popup stays, by the screen width when it opens.
export const toastDuration = () =>
  typeof window !== 'undefined' && window.matchMedia?.(LARGE_SCREEN_QUERY).matches ? TOAST_DURATION_LARGE_MS : TOAST_DURATION_MS;

// How long a closing card animates out before it is removed (matches toastExit in index.css):
// a softer, slower exit below lg.
export const TOAST_EXIT_MS = 160;
export const TOAST_EXIT_SMALL_MS = 380;
const exitDuration = () =>
  typeof window !== 'undefined' && window.matchMedia?.(LARGE_SCREEN_QUERY).matches ? TOAST_EXIT_MS : TOAST_EXIT_SMALL_MS;














// The countdown runs on the browser's frame clock (requestAnimationFrame), so the bar moves every frame
// instead of in 50 ms steps. Falls back to ~60 fps timeouts where rAF is missing.
const nextFrame = (fn) =>
  typeof requestAnimationFrame === 'function' ? requestAnimationFrame(fn) : window.setTimeout(fn, 16);
const cancelFrame = (id) =>
  typeof cancelAnimationFrame === 'function' ? cancelAnimationFrame(id) : clearTimeout(id);

class ToastStore {
   items = [];
   listeners = new Set();
   frameId = null;
   lastTick = Date.now();
   waiting = new Map();   // defer() promises by toast id
   cancelled = new Map();                // defer() popup id → "… cancelled." message

   emit() {
    this.listeners.forEach(fn => fn());
  }

  subscribe = (listener) => {
    this.listeners.add(listener);
    return () => {
      this.listeners.delete(listener);
    };
  };

  getSnapshot = () => {
    return this.items;
  };

   tick = () => {
    this.frameId = null;
    const now = Date.now();
    const delta = now - this.lastTick;
    this.lastTick = now;

    let changed = false;
    const expired = [];
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

   startTimer() {
    if (this.frameId !== null) return;
    this.lastTick = Date.now();
    this.frameId = nextFrame(this.tick);
  }

   stopTimer() {
    if (this.frameId !== null) {
      cancelFrame(this.frameId);
      this.frameId = null;
    }
  }

  add(kind = 'ok', text) {
    if (!text || !text.trim()) return;
    const cleanText = text.trim();

    // Every popup (success or error) closes automatically after toastDuration().
    const isError = kind === 'error' || /\b(already exists|error|failed|invalid|required)\b/i.test(cleanText);
    const duration = toastDuration();

    // Check if an existing toast matches (one already closing counts as gone)
    const existingIndex = this.items.findIndex(i => i.text === cleanText && i.kind === kind && !i.leaving);
    if (existingIndex !== -1) {
      const existing = this.items[existingIndex];
      const updated = {
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
    const newItem = {
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

    this.push(newItem);
  }

  // Keep max 3 stacked. A waiting popup pushed out of the stack counts as closed: its action runs.
   push(item) {
    let nextList = [...this.items, item];
    if (nextList.length > 3) {
      nextList.slice(0, nextList.length - 3).forEach(old => this.settle(old.id, true));
      nextList = nextList.slice(nextList.length - 3);
    }
    this.items = nextList;
    this.emit();
    this.startTimer();
  }

  // A waiting popup: resolves true when it closes (countdown or ✕), false on Cancel (then `cancelledText`
  // is shown). The same message while one is already waiting just shakes it and resolves false, so a
  // double-click (or Ctrl+S twice) runs the action once.
  defer(kind, text, cancelledText = 'Cancelled. Nothing was changed.') {
    const cleanText = text.trim();
    const existing = this.items.find(i => i.pending && i.text === cleanText && !i.leaving);
    if (existing) {
      this.items = this.items.map(i => (i.id === existing.id ? { ...i, shakeKey: i.shakeKey + 1 } : i));
      this.emit();
      return Promise.resolve(false);
    }
    const duration = toastDuration();
    const item = {
      id: `toast-${Date.now()}-${Math.random().toString(36).slice(2, 6)}`,
      kind, text: cleanText, count: 1, shakeKey: 0,
      duration, remaining: duration,
      paused: false, leaving: false, pending: true,
    };
    const result = new Promise(resolve => this.waiting.set(item.id, resolve));
    this.cancelled.set(item.id, cancelledText);
    this.push(item);
    return result;
  }

  // The newest waiting popup still on screen, if any (Enter / Esc act on it).
  newestPending() {
    return [...this.items].reverse().find(i => i.pending && !i.leaving && this.waiting.has(i.id));
  }

  // Run a waiting popup's action now instead of at the end of the countdown.
  runNow(id) {
    this.settle(id, true);
    this.remove(id);
  }

  // Cancel a waiting popup: its action does not run.
  cancel(id) {
    const text = this.cancelled.get(id);
    const wasWaiting = this.waiting.has(id);
    this.settle(id, false);
    this.remove(id);
    if (wasWaiting && text) this.add('refresh', text);
  }

   settle(id, go) {
    const resolve = this.waiting.get(id);
    this.cancelled.delete(id);
    if (!resolve) return;
    this.waiting.delete(id);
    resolve(go);
  }

  // Close a card: it plays its exit animation first, then leaves the list.
  remove(id) {
    const item = this.items.find(i => i.id === id);
    if (!item || item.leaving) return;
    const reduceMotion = typeof window !== 'undefined' && window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
    if (reduceMotion) return this.drop(id);
    this.items = this.items.map(i => (i.id === id ? { ...i, leaving: true } : i));
    this.emit();
    window.setTimeout(() => this.drop(id), exitDuration());
  }

   drop(id) {
    this.settle(id, true);   // a waiting popup that closed without Cancel: go ahead
    this.items = this.items.filter(i => i.id !== id);
    this.emit();
    if (!this.items.some(i => i.duration > 0 && !i.paused && !i.leaving)) {
      this.stopTimer();
    }
  }

  setPaused(id, paused) {
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
export const showToast = (kind, text) => toastStore.add(kind, text);
export const toast = {
  ok: (text) => toastStore.add('ok', text),
  error: (text) => toastStore.add('error', text),
  delete: (text) => toastStore.add('delete', text),
  refresh: (text) => toastStore.add('refresh', text),
  backup: (text) => toastStore.add('backup', text),
  // `if (await toast.later('backup', 'Backup starts when this closes')) { ... }`: see defer().
  later: (kind, text, cancelledText) => toastStore.defer(kind, text, cancelledText),
};
