// Popup (snackbar) state, shared by the whole app.
//
// Call showToast(kind, text) or toast.ok/error(...) from anywhere; <SnackbarContainer /> in App.tsx
// renders them. Rules (see .claude/rules/popups-and-errors.md):
// - every popup closes after TOAST_DURATION_MS (3 s); hovering pauses the countdown;
// - the same message again shakes the existing card and shows ×2, ×3 instead of stacking;
// - at most 3 cards are shown.
// Use popups for server results only; form mistakes are shown inline (FormField / InlineError).

export type ToastKind = 'ok' | 'error' | 'delete' | 'refresh' | 'backup';

export const TOAST_DURATION_MS = 3000;

export interface ToastItem {
  id: string;
  kind: ToastKind;
  text: string;
  count: number;
  shakeKey: number;
  duration: number; // TOAST_DURATION_MS (3000) for every popup
  remaining: number;
  paused: boolean;
}

class ToastStore {
  private items: ToastItem[] = [];
  private listeners = new Set<() => void>();
  private intervalId: number | null = null;
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

  private startTimer() {
    if (this.intervalId !== null) return;
    this.lastTick = Date.now();
    this.intervalId = window.setInterval(() => {
      const now = Date.now();
      const delta = now - this.lastTick;
      this.lastTick = now;

      let changed = false;
      const nextItems: ToastItem[] = [];

      for (const item of this.items) {
        if (item.duration > 0 && !item.paused) {
          const nextRemaining = Math.max(0, item.remaining - delta);
          if (nextRemaining <= 0) {
            changed = true;
            continue; // expired and closes automatically
          }
          if (Math.abs(nextRemaining - item.remaining) > 20) {
            changed = true;
            nextItems.push({ ...item, remaining: nextRemaining });
            continue;
          }
        }
        nextItems.push(item);
      }

      if (changed) {
        this.items = nextItems;
        this.emit();
      }

      if (this.items.length === 0 || this.items.every(i => i.duration === 0 || i.paused)) {
        this.stopTimer();
      }
    }, 50);
  }

  private stopTimer() {
    if (this.intervalId !== null) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  add(kind: ToastKind = 'ok', text: string) {
    if (!text || !text.trim()) return;
    const cleanText = text.trim();

    // Every popup (success or error) closes automatically after 3 seconds.
    const isError = kind === 'error' || /\b(already exists|error|failed|invalid|required)\b/i.test(cleanText);
    const duration = TOAST_DURATION_MS;

    // Check if an existing toast matches
    const existingIndex = this.items.findIndex(i => i.text === cleanText && i.kind === kind);
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

  remove(id: string) {
    this.items = this.items.filter(i => i.id !== id);
    this.emit();
    if (this.items.length === 0 || this.items.every(i => i.duration === 0)) {
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
