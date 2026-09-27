// useConfirm(): an async replacement for window.confirm.
//   const { confirm, dialog } = useConfirm();
//   if (!(await confirm('Delete X?'))) return;
// Render `{dialog}` once in the screen.

import { useCallback, useRef, useState } from 'react';
import { Trash2 } from 'lucide-react';
import Modal from './Modal';
import { dangerButton, secondaryButton } from '../../lib/styles';

// Drop-in for window.confirm: `if (!(await confirm('Delete X?'))) return;`. Render `dialog` once in the screen.
export function useConfirm() {
  const [message, setMessage] = useState<string | null>(null);
  const resolver = useRef<(ok: boolean) => void>(undefined);

  const confirm = useCallback((text: string) => new Promise<boolean>(resolve => {
    resolver.current = resolve;
    setMessage(text);
  }), []);

  const close = (ok: boolean) => {
    resolver.current?.(ok);
    setMessage(null);
  };

  const dialog = message === null ? null : (
    <Modal
      title="Please confirm"
      onClose={() => close(false)}
      footer={<>
        <button className={secondaryButton} onClick={() => close(false)}>Cancel</button>
        <button className={dangerButton} onClick={() => close(true)} autoFocus>Confirm</button>
      </>}
    >
      <div className="flex gap-3 items-center py-2">
        <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-red-500/15 text-red-600 dark:text-red-400">
          <Trash2 size={20} />
        </span>
        <p className="text-sm font-medium text-stone-800 dark:text-stone-100">{message}</p>
      </div>
    </Modal>
  );

  return { confirm, dialog };
}
