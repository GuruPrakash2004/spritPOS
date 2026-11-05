// Delete Non-Liquor Items (legacy frmUtlDeleteNonLiqItems), Administrator only. Pick a non-liquor item group, see its items
// and what would go (bills, lines, stock), then delete the items, the group, their bill lines and their stock.
// This rewrites history in the shop's database: take a backup first. Needs the additional ADMIN password and the group
// code typed again. Liquor items, or items used by documents made in the web app, block it (Merge Items instead).

import { useEffect, useState } from 'react';
import { Eye, Trash2 } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { useConfirm } from '../common/ConfirmDialog';
import { useAdminPassword } from '../common/AdminPasswordPrompt';
import { encryptSecret } from '../../lib/secureField';
import { deleteNonLiquorItems, errorText, getItemGroups, previewDeleteNonLiquor } from '../../lib/api';
import { dangerButton, field, label, pageHeader, pageSub, pageTitle, panel, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export default function DeleteNonLiquorItems() {
  const [groups, setGroups] = useState([]);
  const [group, setGroup] = useState('');
  const [info, setInfo] = useState(null);
  const [confirmText, setConfirmText] = useState('');
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const { confirm, dialog } = useConfirm();
  const { ask, dialog: passwordDialog } = useAdminPassword();

  useEffect(() => { getItemGroups().then(l => setGroups(l.filter(g => !g.liquor).map(g => g.code))).catch(e => setProblem(errorText(e))); }, []);

  const preview = () => {
    setProblem(''); setInfo(null); setConfirmText('');
    if (!group) return setProblem('Select an item group.');
    previewDeleteNonLiquor(group).then(setInfo).catch(e => setProblem(errorText(e)));
  };

  const remove = async () => {
    if (confirmText.trim() !== group) return setProblem(`Type the item group code (${group}) to confirm.`);
    setProblem('');
    const c = info.counts;
    if (!(await confirm(`Do you want to delete the listed items?\n\nThis deletes ${info.items.length} item(s), the group ${group}, ${c.sales_lines} sales line(s) on ${c.sales_bills} bill(s), `
      + `${c.purchase_lines} purchase line(s) on ${c.purchase_bills} bill(s) and ${c.stock_rows} stock row(s). It cannot be undone without a backup. Are you sure?`))) return;
    const password = await ask();
    if (password === null) return;
    if (!(await toastStore.defer('delete', `Group ${group} and its items will be deleted when this closes.`, 'Delete cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      const admin_password = await encryptSecret(password, 'admin');
      const res = await deleteNonLiquorItems({ group, confirm: confirmText.trim(), admin_password });
      toastStore.add('ok', `Successfully deleted ${res.deleted_items} item(s) of ${group}.`);
      setInfo(null); setGroup(''); setConfirmText('');
      setGroups(gs => gs.filter(g => g !== group));
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="mx-auto flex max-w-4xl flex-col gap-4">
      {dialog}
      {passwordDialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Delete Non-Liquor Items</h1>
          <p className={pageSub}>Delete a whole non-liquor item group with its stock and transactions. Take a database backup first.</p>
        </div>
      </div>
      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div>
          <label htmlFor="dnl-group" className={label}>Select ItemGroup</label>
          <select id="dnl-group" value={group} onChange={e => { setGroup(e.target.value); setInfo(null); setProblem(''); }} className={`${field} min-w-48`}>
            <option value="">— pick a group —</option>
            {groups.map(g => <option key={g} value={g}>{g}</option>)}
          </select>
        </div>
        <button type="button" onClick={preview} className={secondaryButton}><Eye size={14} />Show items</button>
      </div>
      {problem && <InlineError message={problem} />}
      {info && (
        <>
          <div className={`${panel} overflow-auto`}>
            <table className="w-full text-xs">
              <thead className={tableHead}><tr><th className="px-3 py-2">Item Name</th><th className="px-3 py-2">Liquor Type</th><th className="px-3 py-2">Problem</th></tr></thead>
              <tbody className={tableBody}>
                {info.items.map(r => (
                  <tr key={r.item_code} className={r.problem ? 'text-red-600' : ''}>
                    <td className="px-3 py-1.5">{r.item_name}</td><td className="px-3 py-1.5">{r.liqtype}</td><td className="px-3 py-1.5">{r.problem}</td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="border-t border-line px-4 py-2 text-xs text-stone-600 dark:text-stone-300">
              No. of Items : {info.items.length} · Sales: {info.counts.sales_lines} line(s) on {info.counts.sales_bills} bill(s) · Purchase: {info.counts.purchase_lines} line(s) on {info.counts.purchase_bills} bill(s) · Stock rows: {info.counts.stock_rows}
            </div>
          </div>
          {info.blockers.length > 0 && <InlineError message={info.blockers.join('. ') + '.'} />}
          {info.can_delete && (
            <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
              <div>
                <label htmlFor="dnl-confirm" className={label}>Type the group code ({group}) to confirm</label>
                <input id="dnl-confirm" value={confirmText} onChange={e => { setConfirmText(e.target.value); setProblem(''); }} className={`${field} w-48`} autoComplete="off" />
              </div>
              <button type="button" disabled={busy} onClick={() => void remove()} className={dangerButton}><Trash2 size={14} />Delete the above listed items along with stock and transactions</button>
            </div>
          )}
        </>
      )}
    </div>
  );
}
