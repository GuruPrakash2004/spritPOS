// Header user menu: the avatar + name opens a card (burgundy header with initials, role badge and user name, the database in use, then
// Tools: Keyboard shortcuts, Refresh from Doc Designer (Administrator), Dark theme, Change my password, and Sign out).
// Replaces the separate password / log-out icon buttons. Pop-up is common/DropdownMenu (portal, stays on screen, Esc closes).

import { useEffect, useRef, useState } from 'react';
import { ChevronDown, Database, Keyboard, KeyRound, LogOut, Moon, RefreshCw, Sun } from 'lucide-react';
import DropdownMenu from '../common/DropdownMenu';
import { getSetupCurrent } from '../../lib/api';
import { runDesignerRefresh } from './designerRefresh';

const item = 'flex w-full items-center gap-3 rounded-lg px-3 py-2.5 text-left text-[13px] text-[#29231F] hover:bg-[#F3EEE6] focus:bg-[#F3EEE6] focus:outline-none dark:text-stone-200 dark:hover:bg-[#1C1C1C] dark:focus:bg-[#1C1C1C]';

export default function UserMenu({ user, role, isAdmin, darkMode, onToggleDark, onShortcuts, onChangePassword, onSignOut }) {
  const [open, setOpen] = useState(false);
  const [database, setDatabase] = useState('');
  const anchorRef = useRef(null);
  const initials = String(user || '').slice(0, 2).toUpperCase();

  // The database name is only offered to the Administrator (the same people who can see Database settings).
  useEffect(() => {
    if (!isAdmin) return;
    let live = true;
    getSetupCurrent().then(s => { if (live) setDatabase(s?.database || ''); }).catch(() => {});
    return () => { live = false; };
  }, [isAdmin]);

  return (
    <>
      <button ref={anchorRef} onClick={() => setOpen(o => !o)} aria-expanded={open} aria-haspopup="menu" aria-label="User menu"
        className="ml-1 flex items-center gap-2 rounded-full border-l py-0.5 pl-3 pr-1 transition hover:bg-[#F3EEE6] dark:hover:bg-[#1C1C1C] border-[#E3D9CD] dark:border-[#2A2A2A] print:hidden">
        <span className="flex h-8 w-8 items-center justify-center rounded-full text-[11px] font-semibold bg-[#64283A]/10 text-[#64283A] dark:bg-[#D4A847]/20 dark:text-[#D4A847]" aria-hidden="true">{initials}</span>
        <span className="hidden text-left leading-tight sm:block">
          <span className="block text-xs font-medium text-[#29231F] dark:text-stone-50">{user}</span>
          <span className="block text-[11px] text-[#70675F] dark:text-stone-400">{role}</span>
        </span>
        <ChevronDown size={14} className="hidden text-[#70675F] sm:block dark:text-stone-400" />
      </button>

      <DropdownMenu anchorRef={anchorRef} open={open} onClose={() => setOpen(false)} width={300}>
        <div className="-m-1 mb-1 flex items-center gap-3 bg-linear-to-br from-[#7A2E44] to-[#4E1E2D] px-4 py-4">
          <span className="flex h-12 w-12 shrink-0 items-center justify-center rounded-full border-2 border-[#D4A847] bg-[#B8923A] text-base font-bold text-white">{initials}</span>
          <div className="min-w-0">
            <div className="truncate text-base font-bold uppercase tracking-wide text-[#E9C46A]">{user}</div>
            <div className="mt-1 flex items-center gap-2">
              <span className="rounded bg-[#B8923A] px-1.5 py-0.5 text-[10px] font-bold uppercase tracking-wide text-[#4E1E2D]">{role}</span>
              <span className="truncate text-xs uppercase text-[#E9C46A]/80">{user}</span>
            </div>
          </div>
        </div>

        {database && (
          <div className="-mx-1 flex items-center gap-2 border-b border-[#E3D9CD] bg-[#FBF3E4] px-4 py-2 text-xs text-[#4A4038] dark:border-[#2A2A2A] dark:bg-[#1A1A1A] dark:text-stone-300">
            <Database size={14} className="shrink-0" /> Database: <b className="truncate text-[#64283A] dark:text-[#D4A847]">{database}</b>
          </div>
        )}

        <div className="px-3 pb-1 pt-3 text-[11px] font-semibold uppercase tracking-wider text-[#64283A] dark:text-[#D4A847]">Tools</div>
        <button role="menuitem" className={item} onClick={onShortcuts}>
          <Keyboard size={16} /> <span className="flex-1">Keyboard shortcuts</span> <span className="text-xs text-[#70675F] dark:text-stone-500">?</span>
        </button>
        {isAdmin && (
          <button role="menuitem" className={item} onClick={() => runDesignerRefresh()}>
            <RefreshCw size={16} /> Refresh from Doc Designer
          </button>
        )}
        <button role="menuitem" className={item} onClick={onToggleDark}>
          {darkMode ? <Sun size={16} /> : <Moon size={16} />} {darkMode ? 'Light theme' : 'Dark theme'}
        </button>
        <button role="menuitem" className={item} onClick={onChangePassword}>
          <KeyRound size={16} /> Change my password
        </button>
        <div className="my-1 border-t border-[#E3D9CD] dark:border-[#2A2A2A]" />
        <button role="menuitem" className={`${item} text-[#9A4B45] dark:text-red-400`} onClick={onSignOut}>
          <LogOut size={16} /> Sign out
        </button>
      </DropdownMenu>
    </>
  );
}
