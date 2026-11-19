// Delete Non-Liquor Items → Carry Over (the second tab of the desktop's frmUtlDeleteNonLiqItems), Administrator only. Pick item groups (any,
// not only non-liquor) and see what would go: their sales and purchase lines, stock rows, and what is carried forward. Carry Over then deletes
// those transactions and brings each item's closing stock back as ONE opening purchase bill dated today (supplier 0, remarks "Stock", at the
// item's purchase rate). Items and groups stay. This rewrites history in the shop's database: take a backup first. It needs the additional
// ADMIN password and CARRY OVER typed to confirm.

import { useEffect, useState } from 'react';
import { Eye, Replace } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { useConfirm } from '../common/ConfirmDialog';
import { useAdminPassword } from '../common/AdminPasswordPrompt';
import { encryptSecret } from '../../lib/secureField';
import { carryOverItems, errorText, getCarryGroups, previewCarryOver } from '../../lib/api';
import { dangerButton, field, label, panel, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export default function CarryOver() {
  const [groups, setGroups] = useState(null);
  const [picked, setPicked] = useState(new Set());
  const [info, setInfo] = useState(null);
  const [confirmText, setConfirmText] = useState('');
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const { confirm, dialog } = useConfirm();
  const { ask, dialog: passwordDialog } = useAdminPassword();

  useEffect(() => { getCarryGroups().then(setGroups).catch(e => { setGroups([]); setProblem(errorText(e)); }); }, []);

  const toggle = (code) => {
    setPicked(cur => { const n = new Set(cur); if (n.has(code)) n.delete(code); else n.add(code); return n; });
    setInfo(null); setConfirmText(''); setProblem('');
  };

  const show = () => {
    setProblem(''); setInfo(null); setConfirmText('');
    if (!picked.size) return setProblem('No groups selected!');
    previewCarryOver([...picked]).then(setInfo).catch(e => setProblem(errorText(e)));
  };

  const run = async () => {
    if (!picked.size) return setProblem('No groups selected!');
    if (confirmText.trim().toUpperCase() !== 'CARRY OVER') return setProblem('Type CARRY OVER to confirm.');
    setProblem('');
    const c = info.counts;
    if (!(await confirm(`Do you want to delete the transactions of listed item groups?\n\nThis deletes ${c.sales_lines} sales line(s) on ${c.sales_bills} bill(s), `
      + `${c.purchase_lines} purchase line(s) on ${c.purchase_bills} bill(s) and ${c.stock_rows} stock row(s) of ${info.items} item(s), and carries over the stock of `
      + `${info.carried_items} item(s) as one purchase bill. It cannot be undone without a backup. Are you sure?`))) return;
    const password = await ask();
    if (password === null) return;
    if (!(await toastStore.defer('delete', 'The transactions will be deleted and the stock carried over when this closes.', 'Cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      const admin_password = await encryptSecret(password, 'admin');
      const res = await carryOverItems({ groups: [...picked], confirm: confirmText.trim(), admin_password });
      toastStore.add('ok', `Successfully deleted the transactions and carried op balance.${res.purchase_bill ? ` Purchase bill ${res.purchase_bill}.` : ''}`);
      setInfo(null); setPicked(new Set()); setConfirmText('');
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="flex flex-col gap-4">
      {dialog}
      {passwordDialog}
      <p className="text-xs text-stone-600 dark:text-stone-300">
        Deletes the sales and purchase lines, stock rows and quantities of the chosen item groups and carries each item's closing stock forward as one
        purchase bill dated today. The items and groups stay. Take a database backup first.
      </p>
      <div className={`${panel} px-5 py-3`}>
        <p className={label}>Select Item Groups</p>
        {!groups && <p className="text-xs text-stone-500">Loading…</p>}
        <div role="group" aria-label="Item groups" className="mt-1 grid max-h-64 grid-cols-1 gap-1 overflow-auto sm:grid-cols-2 lg:grid-cols-3">
          {(groups ?? []).map(g => (
            <label key={g.code} className="flex items-center gap-2 text-xs">
              <input type="checkbox" checked={picked.has(g.code)} onChange={() => toggle(g.code)} />{g.name || g.code}
            </label>
          ))}
        </div>
        <button type="button" onClick={show} className={`${secondaryButton} mt-3`}><Eye size={14} />Show what will be deleted</button>
      </div>
      {problem && <InlineError message={problem} />}
      {info && (
        <div className={`${panel} overflow-auto`}>
          <table className="w-full text-xs">
            <thead className={tableHead}><tr><th className="px-3 py-2 text-left">What</th><th className="px-3 py-2 text-right">Count</th></tr></thead>
            <tbody className={tableBody}>
              <tr><td className="px-3 py-1.5">Items in the groups</td><td className="px-3 py-1.5 text-right tabular-nums">{info.items}</td></tr>
              <tr><td className="px-3 py-1.5">Sales lines deleted (bills touched)</td><td className="px-3 py-1.5 text-right tabular-nums">{info.counts.sales_lines} ({info.counts.sales_bills})</td></tr>
              <tr><td className="px-3 py-1.5">Purchase lines deleted (bills touched)</td><td className="px-3 py-1.5 text-right tabular-nums">{info.counts.purchase_lines} ({info.counts.purchase_bills})</td></tr>
              <tr><td className="px-3 py-1.5">Stock rows deleted</td><td className="px-3 py-1.5 text-right tabular-nums">{info.counts.stock_rows}</td></tr>
              <tr><td className="px-3 py-1.5">Items carried over (total quantity)</td><td className="px-3 py-1.5 text-right tabular-nums">{info.carried_items} ({info.carried_qty})</td></tr>
            </tbody>
          </table>
        </div>
      )}
      {info?.blockers.length > 0 && <InlineError message={info.blockers.join('. ') + '.'} />}
      {info?.can_carry && (
        <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
          <div>
            <label htmlFor="co-confirm" className={label}>Type CARRY OVER to confirm</label>
            <input id="co-confirm" value={confirmText} onChange={e => { setConfirmText(e.target.value); setProblem(''); }} className={`${field} w-48`} autoComplete="off" />
          </div>
          <button type="button" disabled={busy} onClick={() => void run()} className={dangerButton}><Replace size={14} />Delete the transactions and carry over the stock</button>
        </div>
      )}
    </div>
  );
}
