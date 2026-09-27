// Generic dialog. Portalled to <body> so it sits above the header and sidebar;
// Esc and a backdrop click close it. Pass `footer` for the button row and `wide` for large content.

import { useEffect, useId, type ReactNode } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';
import { iconButton } from '../../lib/styles';

// Portalled to <body> so it always sits above the header and sidebar. Esc and a backdrop click close it.
export default function Modal({ title, onClose, children, footer, wide = false }: {
  title: string; onClose: () => void; children: ReactNode; footer?: ReactNode; wide?: boolean;
}) {
  const titleId = useId();
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => { if (e.key === 'Escape') onClose(); };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [onClose]);

  return createPortal(
    <div className="fixed inset-0 z-[900] flex items-center justify-center bg-black/60 dark:bg-black/80 backdrop-blur-sm p-4 transition-all" onClick={onClose}>
      <div
        role="dialog"
        aria-modal="true"
        aria-labelledby={titleId}
        onClick={e => e.stopPropagation()}
        className={`flex max-h-[85vh] w-full flex-col overflow-hidden rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] text-[#29231F] shadow-2xl dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100 animate-slideUp ${wide ? 'max-w-4xl' : 'max-w-lg'}`}
      >
        <div className="flex items-center justify-between gap-4 border-b border-[#E3D9CD] bg-[#FAF6EF] px-5 py-4 dark:border-[#2A2A2A] dark:bg-[#181818]">
          <h3 id={titleId} className="text-sm font-bold text-[#29231F] dark:text-stone-50">{title}</h3>
          <button onClick={onClose} className={`${iconButton} -mr-2 text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-100`} aria-label="Close"><X size={16} /></button>
        </div>
        <div className="flex-1 overflow-auto p-5 text-xs text-stone-700 dark:text-stone-200">{children}</div>
        {footer && <div className="flex justify-end gap-2 border-t border-[#E3D9CD] bg-[#F5F1E9] px-5 py-3 dark:border-[#2A2A2A] dark:bg-[#101010]">{footer}</div>}
      </div>
    </div>,
    document.body,
  );
}
