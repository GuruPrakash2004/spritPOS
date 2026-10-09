// Generic dialog. Portalled to <body> so it sits above the header and sidebar.
// Keyboard (useDialogKeys): focus moves in on open (autoFocus, else the first field, else the main footer
// button), Tab stays inside, Esc closes the top dialog, and focus returns afterwards. A backdrop click closes it.
// Pass `footer` for the button row (put the main action last) and `wide` for large content.

import { useId, useRef, } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';
import { iconButton } from '../../lib/styles';
import { useDialogKeys } from './useDialogKeys';
import { useExitGhost } from './useExitGhost';

export default function Modal({ title, onClose, children, footer, wide = false }

) {
  const titleId = useId();
  const ref = useRef(null);
  const backdropRef = useRef(null);
  useDialogKeys(ref, onClose);
  useExitGhost(backdropRef);   // the dialog fades out when it closes

  return createPortal(
    <div ref={backdropRef} className="modal-backdrop fixed inset-0 z-[900] flex items-center justify-center bg-black/65 p-4 dark:bg-black/80" onClick={onClose}>
      <div
        ref={ref}
        role="dialog"
        aria-modal="true"
        aria-labelledby={titleId}
        onClick={e => e.stopPropagation()}
        className={`flex max-h-[85vh] w-full flex-col overflow-hidden rounded-2xl border border-[#DDE3EC] bg-[#FFFFFF] text-[#1A2236] shadow-2xl dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-100 animate-slideUp ${wide ? 'max-w-4xl' : 'max-w-lg'}`}
      >
        <div className="flex items-center justify-between gap-4 border-b border-[#DDE3EC] bg-[#F7F9FC] px-5 py-4 dark:border-[#2A2A2A] dark:bg-[#181818]">
          <h3 id={titleId} className="text-sm font-bold text-[#1A2236] dark:text-stone-50">{title}</h3>
          <button onClick={onClose} tabIndex={-1} className={`${iconButton} -mr-2 text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-100`} aria-label="Close" title="Close (Esc)"><X size={16} /></button>
        </div>
        <div data-dialog-body className="flex-1 overflow-auto p-5 text-xs text-stone-700 dark:text-stone-200">{children}</div>
        {footer && <div data-dialog-footer className="flex justify-end gap-2 border-t border-[#DDE3EC] bg-[#F4F6FA] px-5 py-3 dark:border-[#2A2A2A] dark:bg-[#101010]">{footer}</div>}
      </div>
    </div>,
    document.body,
  );
}
