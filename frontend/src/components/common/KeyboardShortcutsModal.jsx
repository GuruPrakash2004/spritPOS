// The keyboard-shortcut cheat sheet, opened with `?` or the keyboard button in the header.
// Shows ⌘ on macOS and Ctrl elsewhere.

import { Search, FileText, CheckSquare, X, Receipt, ListOrdered, MessageSquare } from 'lucide-react';
import Modal from './Modal';
import { secondaryButton } from '../../lib/styles';







const SHORTCUT_GROUPS = [
  {
    category: 'Sales bill',
    icon: Receipt,
    shortcuts: [
      { keys: ['Alt', 'B'], description: 'New sales bill from any screen' },
      { keys: ['Enter'], description: 'Bill setup: open the bill' },
      { keys: ['F2'], description: 'Item search (barcode, code or name)' },
      { keys: ['↑', '↓', 'Enter'], description: 'Choose a suggested item and add it' },
      { keys: ['3', '*'], description: 'Quantity 3 for the next item' },
      { keys: ['F3'], description: 'Quick pick list' },
      { keys: ['F4'], description: 'Customer' },
      { keys: ['F6'], description: 'Next payment type (or Alt+P; Shift+F6 back)' },
      { keys: ['F9'], description: 'Cash tendered (Enter saves)' },
      { keys: ['F10'], description: 'Save & print' },
      { keys: ['⌘', 'S'], description: 'Save only (no print)' },
      { keys: ['F7'], description: 'Hold this bill / start a new one' },
      { keys: ['F8'], description: 'Held bills (↑↓, Enter recalls)' },
      { keys: ['Alt', 'R'], description: 'Re-print a saved bill' },
      { keys: ['Alt', 'C'], description: 'Cancel a saved invoice' },
      { keys: ['Alt', 'X'], description: 'Cancel this bill (asks first)' },
    ],
  },
  {
    category: 'Bill lines',
    icon: ListOrdered,
    shortcuts: [
      { keys: ['↑', '↓'], description: 'In the empty search: choose a line' },
      { keys: ['+', '−'], description: 'One more / one less of that line' },
      { keys: ['Delete'], description: 'Remove that line' },
      { keys: ['Enter'], description: 'Edit its quantity' },
      { keys: ['↑', '↓'], description: 'In a quantity / rate box: next line' },
      { keys: ['Enter'], description: 'In a quantity / rate box: back to search' },
      { keys: ['Esc'], description: 'Back to the item search' },
    ],
  },
  {
    category: 'Popups & dialogs',
    icon: MessageSquare,
    shortcuts: [
      { keys: ['Enter'], description: 'Waiting popup: do it now' },
      { keys: ['Esc'], description: 'Waiting popup: cancel' },
      { keys: ['Enter'], description: 'Print dialog: print' },
      { keys: ['↑', '↓'], description: 'Print dialog: choose the format' },
      { keys: ['Tab'], description: 'Next button (stays in the dialog)' },
      { keys: ['Esc'], description: 'Close the dialog' },
    ],
  },
  {
    category: 'Global & Navigation',
    icon: Search,
    shortcuts: [
      { keys: ['⌘', 'K'], description: 'Focus screen search from anywhere' },
      { keys: ['/'], description: 'Quick jump to search (when not typing)' },
      { keys: ['⌘/Ctrl', 'B'], description: 'Collapse or expand the left navigation' },
      { keys: ['?'], description: 'Show keyboard shortcuts' },
      { keys: ['Esc'], description: 'Close modals, search dropdowns, or cancel' },
    ],
  },
  {
    category: 'Forms & Masters',
    icon: FileText,
    shortcuts: [
      { keys: ['⌘', 'S'], description: 'Save the record, entry or bill' },
      { keys: ['F10'], description: 'Save & print (stock entries and purchase bill)' },
      { keys: ['F4'], description: 'Purchase bill: add item (liquor list; F12 adds)' },
      { keys: ['Alt', 'X'], description: 'Purchase bill: cancel the bill' },
      { keys: ['Enter'], description: 'Next field (entries) / save (masters)' },
      { keys: ['Shift', 'Enter'], description: 'Previous field (entries)' },
      { keys: ['Alt', 'A'], description: 'Add a row (purchase bill, stock entries)' },
      { keys: ['Alt', 'H'], description: 'Show or hide the History column (bills, entries)' },
      { keys: ['Tab'], description: 'Move to next input field' },
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

export default function KeyboardShortcutsModal({ onClose }) {
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
          Press any of these key combinations to quickly navigate and operate e-vLQMS ERP.
        </p>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
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
                              {k === '⌘' && !isMac ? 'Ctrl' : k === 'Alt' && isMac ? '⌥' : k}
                            </kbd>
                            {ki < sc.keys.length - 1 && ['⌘', 'Alt', 'Shift'].includes(sc.keys[0]) && (
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
