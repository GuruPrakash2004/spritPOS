// Renders popups from toastStore at the bottom centre of the screen.
//
// - <SnackbarContainer />: mounted once in App.tsx; draws the stacked cards with a countdown bar.
// - <Toast kind text />: legacy per-screen API. Rendering it just forwards the message to toastStore,
//   so older screens that keep `toast` state still get the shared popup.

import { useEffect, useSyncExternalStore } from 'react';
import { createPortal } from 'react-dom';
import { CheckCircle2, AlertCircle, Trash2, X, RefreshCw, DatabaseBackup } from 'lucide-react';
import { toastStore, type ToastKind } from './toastStore';
import RunwayLoader from '../ui/runway-loader';

export function SnackbarContainer() {
  const items = useSyncExternalStore(toastStore.subscribe, toastStore.getSnapshot, toastStore.getSnapshot);

  if (!items.length) return null;

  return createPortal(
    <div
      role="region"
      aria-label="Notifications"
      className="fixed bottom-6 left-1/2 -translate-x-1/2 z-[1000] flex flex-col-reverse items-center gap-3 pointer-events-none w-full max-w-md px-4"
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

        // Colors: gold for refresh, metallic blue for backup, red for delete/error, green for save/success
        const loaderColor: 'green' | 'red' | 'blue' | 'gold' = isRefresh
          ? 'gold'
          : isBackup
          ? 'blue'
          : isDelete || isError
          ? 'red'
          : 'green';

        return (
          <div
            key={item.id}
            role={isError || isDelete ? 'alert' : 'status'}
            onMouseEnter={() => toastStore.setPaused(item.id, true)}
            onMouseLeave={() => toastStore.setPaused(item.id, false)}
            className={`group relative flex w-full flex-col overflow-hidden rounded-2xl border shadow-2xl p-3.5 text-xs pointer-events-auto transition-all duration-200 animate-slideUp ${
              isDelete || isError
                ? 'border-red-500/40 bg-[#FFF8F7] text-red-950 dark:border-red-500/50 dark:bg-[#1A0E0E] dark:text-red-100 shadow-[0_12px_32px_-4px_rgba(220,38,38,0.28)]'
                : isRefresh
                ? 'border-[#D4A847]/45 bg-[#FFFDF9] text-amber-950 dark:border-[#D4A847]/40 dark:bg-[#18140E] dark:text-amber-100 shadow-[0_12px_32px_-4px_rgba(212,168,71,0.28)]'
                : isBackup
                ? 'border-blue-500/40 bg-[#F8FAFF] text-blue-950 dark:border-blue-500/45 dark:bg-[#0E1526] dark:text-blue-100 shadow-[0_12px_32px_-4px_rgba(59,130,246,0.28)]'
                : 'border-[#E3D9CD] bg-[#FFFCF7] text-stone-800 dark:bg-[#141414] dark:border-[#2A2A2A] dark:text-stone-100'
            } ${item.shakeKey > 0 ? 'animate-shake' : ''}`}
            style={{
              boxShadow: isDelete || isError
                ? '0 12px 32px -4px rgba(220, 38, 38, 0.28), 0 4px 12px rgba(0, 0, 0, 0.1)'
                : isRefresh
                ? '0 12px 32px -4px rgba(212, 168, 71, 0.28), 0 4px 12px rgba(0, 0, 0, 0.1)'
                : isBackup
                ? '0 12px 32px -4px rgba(59, 130, 246, 0.28), 0 4px 12px rgba(0, 0, 0, 0.1)'
                : '0 12px 32px -4px rgba(0, 0, 0, 0.22), 0 4px 12px rgba(0, 0, 0, 0.08)',
            }}
          >
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
              </div>

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

            {/* Truck Runway Animation Track with Dynamic Theme Color (Green/Red/Gold/Blue) */}
            {item.duration > 0 && (
              <div className="mt-1.5 w-full px-1">
                <RunwayLoader
                  compact={true}
                  vehicle="truck"
                  color={loaderColor}
                  progress={elapsedPct}
                />
              </div>
            )}
          </div>
        );
      })}
    </div>,
    document.body
  );
}

export default function Toast({ kind = 'ok', text }: { kind?: ToastKind; text: string }) {
  useEffect(() => {
    if (text) {
      toastStore.add(kind, text);
    }
  }, [kind, text]);

  return <SnackbarContainer />;
}
