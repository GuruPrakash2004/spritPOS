// Field rights tab of Admin Master (Security) (legacy frmMstFldRights, "Master Field Rights"): for a role and a
// master, each field is Normal, Hidden (not shown to that role, kept as it is when they save) or Read-only
// (shown, but they can't change it). The server enforces it; MasterCrud shows it. Administrators see everything.
// "Dashboard" lists the Dashboard's panels instead of fields: each is Normal or Hidden for the role.

import { useCallback, useEffect, useState } from 'react';
import { Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { errorText, getFieldRights, saveFieldRights, type ApiFieldRights, type ApiFieldRightsAdmin, type ApiRole } from '../../lib/api';
import { ADMIN_ROLE } from '../../lib/modules';
import { field, label, panel, primaryButton, tableBody, tableHead } from '../../lib/styles';

const MASTER_NAMES: Record<string, string> = {
  items: 'Item Master', 'item-groups': 'Item Group Master', uoms: 'UOM Master', packages: 'Package Master', brands: 'Brand Master',
  suppliers: 'Supplier Master', customers: 'Customer Master', 'ledger-groups': 'Ledger Group Master', ledgers: 'Ledger Master',
  'sales-counters': 'Sales Counter Master', 'sales-persons': 'Sales Person Master', dashboard: 'Dashboard',
};
const caption = (f: string) => f.replace(/_/g, ' ').replace(/^./, c => c.toUpperCase());

export default function FieldRightsTab({ roles }: { roles: ApiRole[] }) {
  const choices = roles.filter(r => r.name !== ADMIN_ROLE);
  const [data, setData] = useState<ApiFieldRightsAdmin | null>(null);
  const [picked, setRole] = useState('');
  const role = picked || choices[0]?.name || '';
  const [master, setMaster] = useState('items');
  const [draft, setDraft] = useState<ApiFieldRights | null>(null);

  const load = useCallback(() => getFieldRights()
    .then(d => { setData(d); setDraft(null); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  const current: ApiFieldRights = draft ?? data?.roles[role] ?? {};
  const setMode = (f: string, mode: string) => {
    const next: ApiFieldRights = { ...current, [master]: { ...(current[master] ?? {}) } };
    if (mode === 'normal') delete next[master][f];
    else next[master][f] = mode as 'hidden' | 'readonly';
    setDraft(next);
  };

  const save = async () => {
    if (!role) return;
    const body = { masters: current };
    if (!(await toastStore.defer('ok', `Field rights for ${role} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveFieldRights(role, body)
      .then(() => { toastStore.add('ok', `Field rights for ${role} saved. They apply the next time its users open a master.`); load(); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  if (!choices.length) return <p className={`${panel} p-6 text-xs text-stone-500`}>Create a role other than Administrator first.</p>;
  return (
    <div className={`${panel} overflow-hidden`}>
      <div className="flex flex-wrap items-end gap-3 border-b border-line p-4">
        <div>
          <label htmlFor="fr-role" className={label}>Role</label>
          <select id="fr-role" value={role} onChange={e => { setRole(e.target.value); setDraft(null); }} className={field}>
            {choices.map(r => <option key={r.name}>{r.name}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="fr-master" className={label}>Select Document</label>
          <select id="fr-master" value={master} onChange={e => setMaster(e.target.value)} className={field}>
            {Object.keys(data?.masters ?? MASTER_NAMES).map(m => <option key={m} value={m}>{MASTER_NAMES[m] ?? m}</option>)}
          </select>
        </div>
        <button className={`${primaryButton} ml-auto`} onClick={save}><Save size={14} />Save field rights</button>
      </div>
      <div className="max-h-[55vh] overflow-auto">
        <table className="min-w-full text-xs">
          <thead><tr><th className={`${tableHead} px-4 py-2`}>Field Caption</th><th className={`${tableHead} px-4 py-2`}>Field Code</th><th className={`${tableHead} px-4 py-2`}>Rights</th></tr></thead>
          <tbody className={tableBody}>
            {(data?.masters[master] ?? []).map(f => (
              <tr key={f}>
                <td className="px-4 py-1.5">{data?.captions?.[master]?.[f] ?? caption(f)}</td>
                <td className="px-4 py-1.5 font-mono text-stone-500">{f}</td>
                <td className="px-4 py-1.5">
                  <select aria-label={`Rights for ${f}`} value={current[master]?.[f] ?? 'normal'} onChange={e => setMode(f, e.target.value)} className={field}>
                    <option value="normal">Normal</option>
                    {master !== 'dashboard' && <option value="readonly">Read-only</option>}
                    <option value="hidden">Hidden</option>
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
