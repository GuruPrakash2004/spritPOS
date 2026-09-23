import { createPortal } from 'react-dom';
import { CheckCircle, XCircle } from 'lucide-react';

// Rendered into <body> rather than in place: inside <main>, the .workspace-backdrop stacking context
// caps any z-index below the z-20 header, which hid toasts behind it.
export default function Toast({ kind = 'ok', text }: { kind?: 'ok' | 'error'; text: string }) {
  return createPortal(
    <div
      role={kind === 'error' ? 'alert' : 'status'}
      className={`fixed right-6 top-[82px] z-[1000] flex max-w-md items-center gap-2 rounded-xl px-4 py-3 text-sm font-medium text-white shadow-2xl ring-1 ring-white/20 ${kind === 'ok' ? 'bg-emerald-600' : 'bg-red-600'}`}
    >
      {kind === 'ok' ? <CheckCircle size={18} className="shrink-0" /> : <XCircle size={18} className="shrink-0" />} {text}
    </div>,
    document.body,
  );
}
