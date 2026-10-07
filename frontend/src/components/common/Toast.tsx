// Renders popups from toastStore at the top right of the screen, stacked downwards.
//
// - <SnackbarContainer />: mounted once in App.tsx; draws the stacked cards with a countdown bar.
// - <Toast kind text />: legacy per-screen API. Rendering it just forwards the message to toastStore,
//   so older screens that keep `toast` state still get the shared popup.

import { useEffect, useSyncExternalStore } from 'react';
import { createPortal } from 'react-dom';
import { CheckCircle2, AlertCircle, Trash2, X, RefreshCw, DatabaseBackup } from 'lucide-react';
import { toastStore, type ToastKind } from './toastStore';

export function SnackbarContainer() {
  const items = useSyncExternalStore(toastStore.subscribe, toastStore.getSnapshot, toastStore.getSnapshot);

  // A waiting popup takes Enter (do it now) and Esc (cancel) before any screen or dialog sees the key.
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.repeat || e.ctrlKey || e.metaKey || e.altKey || (e.key !== 'Enter' && e.key !== 'Escape')) return;
      const waiting = toastStore.newestPending();
      if (!waiting) return;
      // Enter in a search box with text in it is a scan or a search, not an answer to the popup.
      const t = e.target as HTMLInputElement | null;
      if (e.key === 'Enter' && t?.getAttribute?.('role') === 'combobox' && t.value?.trim()) return;
      e.preventDefault();
      e.stopImmediatePropagation();
      if (e.key === 'Enter') toastStore.runNow(waiting.id);
      else toastStore.cancel(waiting.id);
    };
    window.addEventListener('keydown', onKey, true);
    return () => window.removeEventListener('keydown', onKey, true);
  }, []);

  if (!items.length) return null;

  return createPortal(
    <div
      role="region"
      aria-label="Notifications"
      className="fixed right-4 top-4 z-[1000] flex w-full max-w-sm flex-col items-stretch gap-2 pointer-events-none px-0"
    >
      {items.map(item => {
        const isDelete = item.kind === 'delete' || /\b(delete|deleted|deletion|removed)\b/i.test(item.text);
        const isError = item.kind === 'error' || /\b(already exists|error|failed|invalid)\b/i.test(item.text);
        const isRefresh = item.kind === 'refresh' || /\b(refresh|refreshed)\b/i.test(item.text);
        const isBackup = item.kind === 'backup' || /\b(backup|database backup)\b/i.test(item.text);

        // Elapsed progress from 0% to 100%
        const elapsedPct = item.duration > 0
          ? Math.min(100, Math.max(0, ((item.duration - item.remaining) / item.duration) * 100))
          : 100;

        // Thin countdown bar and left stripe: green for success, red for errors and deletes, amber refresh, blue backup.
        const stripe = isDelete || isError ? 'bg-red-500' : isRefresh ? 'bg-[#D4A847]' : isBackup ? 'bg-blue-500' : 'bg-emerald-500';

        return (
          <div
            key={item.id}
            role={isError || isDelete ? 'alert' : 'status'}
            onMouseEnter={() => toastStore.setPaused(item.id, true)}
            onMouseLeave={() => toastStore.setPaused(item.id, false)}
            className={`toast-card ${item.leaving ? 'toast-leaving' : 'toast-enter'} group relative flex w-full flex-col overflow-hidden rounded-xl border border-line bg-surface p-3.5 pl-5 text-xs shadow-2xl pointer-events-auto dark:border-[#2A2A2A] dark:bg-[#141414] ${item.shakeKey > 0 ? 'animate-shake' : ''}`}
          >
            <span aria-hidden className={`absolute inset-y-0 left-0 w-1.5 ${stripe}`} />
            {/* Top Row: Icon, Message, Close Button */}
            <div className="flex items-center gap-2.5">
              <div className="shrink-0">
                {isDelete ? (
                  <Trash2 size={16} className="text-red-600 dark:text-red-400" />
                ) : isError ? (
                  <AlertCircle size={16} className="text-red-600 dark:text-red-400" />
                ) : isRefresh ? (
                  <RefreshCw size={16} className="text-[#C5A059] dark:text-[#D4A847] animate-spin" style={{ animationDuration: '3s' }} />
                ) : isBackup ? (
                  <DatabaseBackup size={16} className="text-blue-600 dark:text-blue-400" />
                ) : (
                  <CheckCircle2 size={16} className="text-emerald-600 dark:text-emerald-400" />
                )}
              </div>

              {/* Message Text */}
              <div className={`flex-1 font-medium leading-normal break-words pr-1 text-[12px] ${
                isDelete || isError
                  ? 'text-red-950 dark:text-red-100 font-semibold'
                  : isRefresh
                  ? 'text-amber-950 dark:text-amber-100 font-semibold'
                  : isBackup
                  ? 'text-blue-950 dark:text-blue-100 font-semibold'
                  : ''
              }`}>
                {item.text}
                {item.pending && !item.leaving && (
                  <span className="mt-0.5 block text-[10.5px] font-normal opacity-70">Enter: do it now · Esc: cancel</span>
                )}
              </div>

              {/* Waiting popup: the action runs when it closes unless Cancel is pressed */}
              {item.pending && !item.leaving && (
                <button
                  type="button"
                  onClick={() => toastStore.cancel(item.id)}
                  title="Cancel (Esc)"
                  className="shrink-0 rounded-md border border-current/30 px-2 py-0.5 text-[11px] font-semibold hover:bg-black/5 dark:hover:bg-white/10"
                >
                  Cancel
                </button>
              )}

              {/* Close Button */}
              <button
                type="button"
                onClick={() => toastStore.remove(item.id)}
                className={`shrink-0 rounded-full p-1 transition-colors ${
                  isDelete || isError
                    ? 'text-red-400 hover:bg-red-200/60 hover:text-red-700 dark:text-red-400 dark:hover:bg-red-900/40 dark:hover:text-red-200'
                    : isRefresh
                    ? 'text-amber-500 hover:bg-amber-200/60 hover:text-amber-800 dark:text-amber-400 dark:hover:bg-amber-900/40 dark:hover:text-amber-200'
                    : isBackup
                    ? 'text-blue-400 hover:bg-blue-200/60 hover:text-blue-700 dark:text-blue-400 dark:hover:bg-blue-900/40 dark:hover:text-blue-200'
                    : 'text-stone-400 hover:bg-stone-200/60 hover:text-stone-700 dark:text-stone-500 dark:hover:bg-stone-800 dark:hover:text-stone-200'
                }`}
                aria-label="Close notification"
              >
                <X size={14} />
              </button>
            </div>

            {item.duration > 0 && (
              <div className="mt-2 h-0.5 w-full overflow-hidden rounded-full bg-black/10 dark:bg-white/10">
                <div className={`h-full ${stripe}`} style={{ width: `${elapsedPct}%` }} />
              </div>
            )}
          </div>
        );
      })}
    </div>,
    document.body
  );
}

// Renders nothing itself: App.tsx already mounts the one <SnackbarContainer />. (Rendering a second
// container here drew every popup twice while a screen's message was showing.)
export default function Toast({ kind = 'ok', text }: { kind?: ToastKind; text: string }) {
  useEffect(() => {
    if (text) {
      toastStore.add(kind, text);
    }
  }, [kind, text]);

  return null;
}
