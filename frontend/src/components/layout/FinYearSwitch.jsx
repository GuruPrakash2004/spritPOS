// The header's financial-year chip. For an Administrator it is a drop-down of the shop's financial years (Company Master → Financial
// Year(s)); choosing one makes it the ACTIVE year for everyone (PUT /masters/company/fin-years/{start}/activate), after a confirmation,
// because the active year gives new documents their number prefix. Other roles, and a shop with one year, see the plain chip.

import { useRef, useState } from 'react';
import { ChevronDown } from 'lucide-react';
import DropdownMenu, { menuItem } from '../common/DropdownMenu';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { activateFinYear, errorText } from '../../lib/api';
import { notifyCompanyProfileUpdated } from '../../hooks/useCompanyProfile';
import { finYearLabel } from '../../lib/formatters';

const chip = 'hidden items-center gap-1 rounded-md border border-[#E3D9CD] px-2 py-1 text-[11px] font-medium tabular-nums text-[#64283A] lg:inline-flex dark:border-[#2A2A2A] dark:text-[#D4A847]';

export default function FinYearSwitch({ label, years = [], canChange = false, onChanged }) {
  const anchor = useRef(null);
  const [open, setOpen] = useState(false);
  const { confirm, dialog } = useConfirm();
  const list = [...years].filter(y => y.start && y.end).sort((a, b) => b.start.localeCompare(a.start));

  if (!canChange || list.length < 2) {
    return <span className={chip} title="Financial year">FY {label || '—'}</span>;
  }

  const choose = async (y) => {
    setOpen(false);
    const name = finYearLabel(y.start, y.end);
    if (y.active && name === label) return;
    if (!(await confirm(`Make ${name} the active financial year? New bills will be numbered in it, for every user.`))) return;
    if (!(await toastStore.defer('ok', `The financial year ${name} will be made active when this closes.`, 'Cancelled. The active year is unchanged.'))) return;
    try {
      await activateFinYear(y.start);
      notifyCompanyProfileUpdated();
      onChanged?.(name);
      toastStore.add('ok', `Financial year ${name} is now active.`);
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  return (
    <>
      {dialog}
      <button ref={anchor} type="button" onClick={() => setOpen(o => !o)} aria-haspopup="menu" aria-expanded={open} title="Change the active financial year"
        className={`${chip} cursor-pointer hover:bg-[#F3EEE6] dark:hover:bg-[#1C1C1C]`}>
        FY {label || '—'}<ChevronDown size={12} aria-hidden="true" />
      </button>
      <DropdownMenu anchorRef={anchor} open={open} onClose={() => setOpen(false)} width={230}>
        {list.map(y => {
          const name = finYearLabel(y.start, y.end);
          return (
            <button key={y.start} type="button" className={`${menuItem} flex items-center justify-between gap-2 text-xs tabular-nums`} onClick={() => void choose(y)}>
              <span>{name}</span>
              {y.active && <span className="rounded bg-[#F0E2E6] px-1.5 py-0.5 text-[10px] font-semibold text-[#64283A] dark:bg-[#D4A847]/15 dark:text-[#D4A847]">Active</span>}
            </button>
          );
        })}
      </DropdownMenu>
    </>
  );
}
