// A key hint on a button, e.g. <Kbd combo="F10" /> or <Kbd combo="Mod+S" /> (⌘S on a Mac, Ctrl+S elsewhere).
// Hidden on small screens, where there is usually no keyboard.

import { keyLabel } from '../../lib/keys';

export default function Kbd({ combo, className = '' }) {
  return (
    <kbd aria-hidden="true"
      className={`ml-1 hidden rounded border border-current/25 px-1 py-px font-mono text-[10px] font-semibold leading-none opacity-70 lg:inline ${className}`}>
      {keyLabel(combo)}
    </kbd>
  );
}
