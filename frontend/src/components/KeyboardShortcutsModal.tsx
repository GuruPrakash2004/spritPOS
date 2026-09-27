import { Keyboard, Search, FileText, CheckSquare, X } from 'lucide-react';
import Modal from './Modal';
import { secondaryButton } from './ui';

interface ShortcutGroup {
  category: string;
  icon: typeof Keyboard;
  shortcuts: { keys: string[]; description: string }[];
}

const SHORTCUT_GROUPS: ShortcutGroup[] = [
  {
    category: 'Global & Navigation',
    icon: Search,
    shortcuts: [
      { keys: ['⌘', 'K'], description: 'Focus screen search from anywhere' },
      { keys: ['/'], description: 'Quick jump to search (when not typing)' },
      { keys: ['?'], description: 'Show keyboard shortcuts' },
      { keys: ['Esc'], description: 'Close modals, search dropdowns, or cancel' },
    ],
  },
  {
    category: 'Forms & Masters',
    icon: FileText,
    shortcuts: [
      { keys: ['⌘', 'S'], description: 'Quick save active record or master form' },
      { keys: ['Enter'], description: 'Submit form / run report / select search item' },
      { keys: ['Tab'], description: 'Move to next input field' },
      { keys: ['Shift', 'Tab'], description: 'Move to previous input field' },
    ],
  },
  {
    category: 'Lists & Permissions Matrix',
    icon: CheckSquare,
    shortcuts: [
      { keys: ['↑', '↓'], description: 'Navigate through records & search results' },
      { keys: ['←', '→'], description: 'Navigate permission columns in Admin Master' },
      { keys: ['Space'], description: 'Toggle checkbox or permission tick' },
    ],
  },
];

export default function KeyboardShortcutsModal({ onClose }: { onClose: () => void }) {
  const isMac = typeof navigator !== 'undefined' && /Mac|iPod|iPhone|iPad/.test(navigator.platform);

  return (
    <Modal
      title="Keyboard Shortcuts"
      onClose={onClose}
      wide={true}
      footer={
        <button onClick={onClose} className={secondaryButton}>
          <X size={14} />Close
        </button>
      }
    >
      <div className="space-y-6 py-1">
        <p className="text-xs text-[#70675F] dark:text-stone-400">
          Press any of these key combinations to quickly navigate and operate vLQMS.
        </p>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-3">
          {SHORTCUT_GROUPS.map(group => {
            const Icon = group.icon;
            return (
              <div
                key={group.category}
                className="flex flex-col rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-4 shadow-2xs dark:border-[#2A2A2A] dark:bg-[#141414]"
              >
                <div className="flex items-center gap-2 border-b border-[#E3D9CD] pb-2.5 dark:border-[#2A2A2A]">
                  <Icon size={16} className="text-[#64283A] dark:text-[#D4A847]" />
                  <h3 className="text-xs font-bold text-[#29231F] dark:text-stone-50">
                    {group.category}
                  </h3>
                </div>

                <div className="mt-3 space-y-3">
                  {group.shortcuts.map(sc => (
                    <div key={sc.description} className="flex items-center justify-between gap-2">
                      <span className="text-[11px] text-[#70675F] dark:text-stone-300 leading-tight">
                        {sc.description}
                      </span>
                      <div className="flex shrink-0 items-center gap-1">
                        {sc.keys.map((k, ki) => (
                          <span key={ki} className="flex items-center gap-1">
                            <kbd className="inline-flex min-w-[20px] items-center justify-center rounded-md border border-[#DCD1C5] bg-[#F3EEE6] px-1.5 py-0.5 font-mono text-[10px] font-bold text-[#29231F] shadow-xs dark:border-[#3A3A3A] dark:bg-[#1E1E1E] dark:text-stone-100">
                              {k === '⌘' && !isMac ? 'Ctrl' : k}
                            </kbd>
                            {ki < sc.keys.length - 1 && (
                              <span className="text-[10px] text-stone-400">+</span>
                            )}
                          </span>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </Modal>
  );
}
