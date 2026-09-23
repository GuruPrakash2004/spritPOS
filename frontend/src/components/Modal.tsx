import type { ReactNode } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';

// Portalled to <body> for the same reason as Toast: .workspace-backdrop is a stacking context below the header.
export default function Modal({ title, onClose, children, footer, wide = false }: {
  title: string; onClose: () => void; children: ReactNode; footer?: ReactNode; wide?: boolean;
}) {
  return createPortal(
    <div className="fixed inset-0 z-[900] flex items-center justify-center bg-black/50 p-4" onClick={onClose}>
      <div
        role="dialog"
        aria-modal="true"
        aria-label={title}
        onClick={e => e.stopPropagation()}
        className={`flex max-h-[85vh] w-full flex-col overflow-hidden rounded-2xl border border-[#e1c9b6] bg-[#fffaf5] text-stone-800 shadow-2xl dark:border-[#343b59] dark:bg-[#171b2b] dark:text-stone-100 ${wide ? 'max-w-4xl' : 'max-w-lg'}`}
      >
        <div className="flex items-center justify-between border-b border-[#e1c9b6] bg-[#f1e3d8] px-4 py-3 dark:border-[#343b59] dark:bg-[#20253a]">
          <h3 className="text-sm font-bold">{title}</h3>
          <button onClick={onClose} className="rounded p-1 text-stone-500 hover:bg-[#e8702a]/10 hover:text-[#b84716] dark:hover:text-[#ffb07e]" aria-label="Close"><X size={16} /></button>
        </div>
        <div className="flex-1 overflow-auto p-4 text-xs">{children}</div>
        {footer && <div className="flex justify-end gap-2 border-t border-[#e1c9b6] px-4 py-3 dark:border-[#343b59]">{footer}</div>}
      </div>
    </div>,
    document.body,
  );
}
