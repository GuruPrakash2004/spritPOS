import { Fragment } from 'react';
import { Check, Minus } from 'lucide-react';
import type { KeyboardEvent } from 'react';
import type { RoleDraft } from './matrixDefs';
import { GROUPS, MATRIX_SCREENS } from './matrixDefs';
import { tableHead } from './ui';

type Action = 'open' | 'add' | 'edit' | 'delete';

type Props = {
  roleDraft: RoleDraft;
  onToggleOpen: (screenId: string) => void;
  onToggleAction: (screenId: string, action: Exclude<Action, 'open'>) => void;
  onToggleGroup: (group: string, action: Action) => void;
  onTickKeyDown: (event: KeyboardEvent, rowIndex: number, columnIndex: number, screenId: string, action: Action) => void;
  registerCell: (key: string, element: HTMLButtonElement | null) => void;
};

const columns: { action: Action; title: string; width: string; capability: 'hasOpen' | 'hasAdd' | 'hasEdit' | 'hasDelete' }[] = [
  { action: 'open', title: 'OPEN', width: 'w-24', capability: 'hasOpen' },
  { action: 'add', title: 'ADD / SAVE', width: 'w-28', capability: 'hasAdd' },
  { action: 'edit', title: 'EDIT', width: 'w-24', capability: 'hasEdit' },
  { action: 'delete', title: 'DELETE / CANCEL', width: 'w-32', capability: 'hasDelete' },
];

export default function PermissionMatrixTable({ roleDraft, onToggleOpen, onToggleAction, onToggleGroup, onTickKeyDown, registerCell }: Props) {
  const toggle = (screenId: string, action: Action) => action === 'open' ? onToggleOpen(screenId) : onToggleAction(screenId, action);

  return (
    <div className="overflow-hidden rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:border-[#2A2A2A] dark:bg-[#141414] dark:shadow-card">
      <table className="w-full border-collapse text-left text-xs">
        <thead className={tableHead}><tr>
          <th className="px-4 py-3 font-semibold text-[11px] uppercase tracking-[0.04em]">SCREEN</th>
          {columns.map(column => <th key={column.action} className={`${column.width} px-2 py-3 text-center text-[11px] font-semibold uppercase tracking-[0.04em] ${column.action === 'delete' ? 'text-[#9A4B45] dark:text-red-400' : ''}`}>{column.title}</th>)}
        </tr></thead>
        <tbody>
          {GROUPS.map(groupName => {
            const groupScreens = MATRIX_SCREENS.filter(screen => screen.group === groupName);
            const openInGroup = groupScreens.filter(screen => roleDraft.modules.has(screen.id)).length;
            const allOpen = openInGroup === groupScreens.length;
            const someOpen = openInGroup > 0 && !allOpen;
            return <Fragment key={groupName}>
              <tr className="border-y border-[#E3D9CD] bg-[#F3EEE6] dark:border-[#2A2A2A] dark:bg-[#1C1C1C]">
                <td className="px-4 py-2.5">
                  <span className="text-xs font-bold uppercase tracking-wide text-[#64283A] dark:text-[#E8C36A]">{groupName}</span>
                  <span className="ml-2 text-[11px] text-[#70675F] dark:text-stone-400">{openInGroup} of {groupScreens.length} open</span>
                </td>
                {columns.map(column => {
                  const applicable = groupScreens.some(screen => screen[column.capability]);
                  return <td key={column.action} className="px-2 py-2.5 text-center">
                    {applicable ? <button type="button" onClick={() => onToggleGroup(groupName, column.action)}
                      className="inline-flex h-5 w-5 items-center justify-center rounded-[5px] bg-[#64283A] text-white shadow-xs transition-all hover:bg-[#542030] dark:bg-gradient-to-r dark:from-[#D4A847] dark:to-[#C8952E] dark:text-[#180E09]"
                      aria-label={`Toggle all ${column.action} in ${groupName}`}>
                      {column.action === 'open' ? allOpen ? <Check size={14} strokeWidth={3} /> : someOpen ? <Minus size={14} strokeWidth={3} /> : null : <Minus size={14} strokeWidth={3} />}
                    </button> : <span className="font-bold text-stone-300 dark:text-stone-600">—</span>}
                  </td>;
                })}
              </tr>
              {groupScreens.map(screen => {
                const globalIndex = MATRIX_SCREENS.findIndex(entry => entry.id === screen.id);
                return <tr key={screen.id} className="border-b border-[#EEE7DD] transition-colors hover:bg-[#FAF6EF] dark:border-[#1E1E1E] dark:hover:bg-[#1C1C1C]">
                  <td className="px-4 py-2.5">
                    <div className={`text-xs font-bold ${roleDraft.modules.has(screen.id) ? 'text-[#29231F] dark:text-stone-50' : 'text-stone-400 dark:text-stone-600'}`}>{screen.name}</div>
                    <div className={`mt-0.5 text-[11px] leading-tight ${roleDraft.modules.has(screen.id) ? 'text-[#70675F] dark:text-stone-400' : 'text-stone-300 dark:text-stone-700'}`}>{screen.sub}</div>
                  </td>
                  {columns.map((column, columnIndex) => {
                    const enabled = column.action === 'open'
                      ? roleDraft.modules.has(screen.id)
                      : roleDraft.actions.has(`${screen.id}:${column.action}`);
                    return <td key={column.action} className="px-2 py-2.5 text-center">
                      {screen[column.capability] ? <button type="button"
                        ref={element => registerCell(`${globalIndex}-${columnIndex}`, element)}
                        onClick={() => toggle(screen.id, column.action)}
                        onKeyDown={event => onTickKeyDown(event, globalIndex, columnIndex, screen.id, column.action)}
                        aria-label={`${column.action} ${screen.name}`} aria-checked={enabled} role="checkbox"
                        className={`inline-flex h-5 w-5 items-center justify-center rounded-[4px] transition-all focus:outline-none focus-visible:ring-2 focus-visible:ring-[#D9531E] ${enabled
                          ? column.action === 'delete' ? 'bg-[#9A4B45] text-white shadow-xs dark:bg-red-600' : 'bg-[#64283A] text-white shadow-xs dark:bg-gradient-to-r dark:from-[#D4A847] dark:to-[#C8952E] dark:text-[#180E09]'
                          : `border border-[#DCD1C5] bg-[#FFFCF7] dark:border-[#3A3A3A] dark:bg-[#1A1A1A] ${column.action === 'delete' ? 'hover:border-[#9A4B45] dark:hover:border-red-400' : 'hover:border-[#64283A] dark:hover:border-[#D4A847]'}`
                        }`}>
                        {enabled && <Check size={14} strokeWidth={3} />}
                      </button> : <span className="font-bold text-stone-400 dark:text-stone-600">—</span>}
                    </td>;
                  })}
                </tr>;
              })}
            </Fragment>;
          })}
        </tbody>
      </table>
    </div>
  );
}