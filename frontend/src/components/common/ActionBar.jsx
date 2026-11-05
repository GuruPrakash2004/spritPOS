// The floating action bar at the bottom of every master and document screen (the exe's btnCtrl bar):
//
//   [ New ] [ Edit ] [ Delete ] [ Print ] [ List ] [ Close ] [ Options ▾ ]                    ⏮ ◀ 🔍 ▶ ⏭
//
// While a record is being added or edited ('New' / 'Edit') the bar shows Save and Cancel instead, as the exe does. A button the
// screen has no action for is simply not drawn; one the user's rights forbid is drawn disabled with the reason as its tooltip.
// `options` fills the Options menu with the screen's extra actions ([{ label, onClick, disabled? }]); the menu is hidden when empty.
// `nav` = { onFirst, onPrev, onNext, onLast, onFind, atStart, atEnd, label } adds the record-moving buttons on the right.
// Keys (not while typing in a dialog): Alt+N New, Alt+E Edit, Alt+D Delete, Alt+P Print, Alt+L List, Alt+O Close, Alt+S Save,
// Alt+C Cancel (as the exe's Alt-letters), Alt+Home / Alt+PageUp / Alt+PageDown / Alt+End move between records, Alt+F Find.
//
// Put the bar last inside <ScreenFrame> so it floats at the bottom of the screen.

import { useRef, useState } from 'react';
import { ChevronDown, ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight, FilePlus, ListTree, Pencil, Printer, Save, Search, Trash2, X } from 'lucide-react';
import DropdownMenu, { menuItem } from './DropdownMenu';
import Kbd from './Kbd';
import { useShortcuts } from '../../lib/keys';

const base = 'inline-flex items-center justify-center gap-2 rounded-full px-5 py-2.5 text-sm font-semibold whitespace-nowrap transition duration-150 active:scale-[0.98] disabled:pointer-events-none disabled:opacity-40';
const filled = `${base} bg-[#1B2B4B] text-white shadow-[0_6px_16px_rgba(27,43,75,0.25)] hover:bg-[#121D36] dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F5D88A] dark:to-[#C8952E] dark:text-[#180E09] dark:shadow-[0_0_18px_rgba(212,168,71,0.35)] dark:hover:brightness-110`;
const soft = `${base} bg-[#EEE9E3] text-[#1B2B4B] hover:bg-[#E5DED6] dark:bg-[#1F1F1F] dark:text-[#D4A847] dark:hover:bg-[#262626]`;
const danger = `${base} bg-[#FCE9E7] text-[#B3392F] hover:bg-[#F9D9D5] dark:bg-red-950/40 dark:text-red-300 dark:hover:bg-red-950/60`;
const outline = `${base} border border-[#DDE3EC] bg-white text-[#1A2236] hover:bg-[#F0F3F8] dark:border-[#2A2A2A] dark:bg-transparent dark:text-stone-100 dark:hover:bg-[#1C1C1C]`;
const round = 'inline-flex h-10 w-10 items-center justify-center rounded-full border border-[#DDE3EC] bg-white text-[#1A2236] transition hover:bg-[#F0F3F8] disabled:pointer-events-none disabled:opacity-35 dark:border-[#2A2A2A] dark:bg-transparent dark:text-stone-100 dark:hover:bg-[#1C1C1C]';

/** Wraps a screen so its action bar floats at the bottom of the window even when the screen is short. */
export function ScreenFrame({ children, bar }) {
  return (
    <div className="flex min-h-[calc(100dvh-8.5rem)] flex-col">
      <div className="min-w-0 flex-1">{children}</div>
      {bar}
    </div>
  );
}

export default function ActionBar({
  mode = 'View', onNew, onEdit, onDelete, onPrint, onList, onClose, onSave, onCancel,
  canNew = true, canEdit = true, canDelete = true, deleteLabel = 'Delete', newLabel = 'New', saveLabel = 'Save', listLabel = 'List',
  noEditReason, noDeleteReason, noNewReason, saving = false, options = [], nav,
}) {
  const [menu, setMenu] = useState(false);
  const optionsRef = useRef(null);
  const editing = mode !== 'View';
  const run = (fn, allowed = true) => () => { if (fn && allowed) fn(); };
  const menuItems = options.filter(Boolean);

  useShortcuts({
    'Alt+N': run(onNew, !editing && canNew && !!onNew), 'Alt+E': run(onEdit, !editing && canEdit && !!onEdit), 'Alt+D': run(onDelete, !editing && canDelete && !!onDelete),
    'Alt+P': run(onPrint, !editing && !!onPrint), 'Alt+L': run(onList, !!onList), 'Alt+O': run(onClose, !editing && !!onClose),
    'Alt+S': run(onSave, editing && !!onSave), 'Alt+C': run(onCancel, editing && !!onCancel),
    'Alt+Home': run(nav?.onFirst, !editing && !!nav && !nav.atStart), 'Alt+PageUp': run(nav?.onPrev, !editing && !!nav && !nav.atStart),
    'Alt+PageDown': run(nav?.onNext, !editing && !!nav && !nav.atEnd), 'Alt+End': run(nav?.onLast, !editing && !!nav && !nav.atEnd),
    'Alt+F': run(nav?.onFind, !editing && !!nav?.onFind),
  });

  const noEdit = canEdit ? undefined : (noEditReason ?? 'Your role is not allowed to edit here');
  const noDel = canDelete ? undefined : (noDeleteReason ?? 'Your role is not allowed to delete here');

  return (
    <div role="toolbar" aria-label="Actions" className="sticky bottom-3 z-20 mt-6">
      <div className="flex flex-wrap items-center gap-2 rounded-[28px] border border-[#DDE3EC] bg-white/95 px-4 py-3 shadow-[0_10px_40px_rgba(27,43,75,0.14)] backdrop-blur dark:border-[#2A2A2A] dark:bg-[#141414]/95 dark:shadow-[0_10px_40px_rgba(0,0,0,0.5)]">
        {editing ? (
          <>
            <button type="button" onClick={onSave} disabled={saving} className={filled} title="Save (Alt+S)"><Save size={16} />{saving ? 'Saving…' : saveLabel}<Kbd combo="Alt+S" /></button>
            <button type="button" onClick={onCancel} className={outline} title="Cancel (Alt+C)"><X size={16} />Cancel<Kbd combo="Alt+C" /></button>
          </>
        ) : (
          <>
            {onNew && <button type="button" onClick={onNew} disabled={!canNew} className={filled} title={canNew ? `${newLabel} (Alt+N)` : (noNewReason ?? 'Your role is not allowed to add here')}><FilePlus size={16} />{newLabel}</button>}
            {onEdit && <button type="button" onClick={onEdit} disabled={!canEdit} className={soft} title={noEdit ?? 'Edit (Alt+E)'}><Pencil size={15} />Edit</button>}
            {onDelete && <button type="button" onClick={onDelete} disabled={!canDelete} className={danger} title={noDel ?? `${deleteLabel} (Alt+D)`}><Trash2 size={15} />{deleteLabel}</button>}
            {onPrint && <button type="button" onClick={onPrint} className={outline} title="Print (Alt+P)"><Printer size={15} />Print</button>}
            {onList && <button type="button" onClick={onList} className={outline} title={`${listLabel} (Alt+L)`}><ListTree size={15} />{listLabel}</button>}
            {onClose && <button type="button" onClick={onClose} className={outline} title="Close (Alt+O)"><X size={15} />Close</button>}
            {menuItems.length > 0 && (
              <>
                <button ref={optionsRef} type="button" onClick={() => setMenu(o => !o)} aria-expanded={menu} aria-haspopup="menu" className={soft}>Options<ChevronDown size={15} className={`transition-transform ${menu ? 'rotate-180' : ''}`} /></button>
                <DropdownMenu anchorRef={optionsRef} open={menu} onClose={() => setMenu(false)} align="start">
                  <div role="menu" className="min-w-56 p-1 text-xs">
                    {menuItems.map(o => (
                      <button key={o.label} type="button" role="menuitem" disabled={o.disabled} onClick={() => { setMenu(false); o.onClick?.(); }} className={`${menuItem} disabled:pointer-events-none disabled:opacity-40`}>{o.label}</button>
                    ))}
                  </div>
                </DropdownMenu>
              </>
            )}
          </>
        )}

        {nav && !editing && (
          <div role="group" aria-label="Move between records" className="ml-auto flex items-center gap-2">
            {nav.label && <span className="mr-1 text-xs tabular-nums text-stone-500" aria-live="polite">{nav.label}</span>}
            {nav.onFirst && <button type="button" onClick={nav.onFirst} disabled={nav.atStart} className={round} title="First record (Alt+Home)" aria-label="First record"><ChevronsLeft size={16} /></button>}
            {nav.onPrev && <button type="button" onClick={nav.onPrev} disabled={nav.atStart} className={round} title="Previous record (Alt+Page Up)" aria-label="Previous record"><ChevronLeft size={16} /></button>}
            {nav.onFind && <button type="button" onClick={nav.onFind} className={round} title="Search (Alt+F)" aria-label="Search"><Search size={16} /></button>}
            {nav.onNext && <button type="button" onClick={nav.onNext} disabled={nav.atEnd} className={round} title="Next record (Alt+Page Down)" aria-label="Next record"><ChevronRight size={16} /></button>}
            {nav.onLast && <button type="button" onClick={nav.onLast} disabled={nav.atEnd} className={round} title="Last record (Alt+End)" aria-label="Last record"><ChevronsRight size={16} /></button>}
          </div>
        )}
      </div>
    </div>
  );
}
