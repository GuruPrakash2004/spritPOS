// Master approvals tab of Admin Master (Security) (legacy frmUtlMstApprovals, "Check the selected Masters which
// needs approval"): tick the masters whose new or changed rows must be approved by an Administrator. Rows saved
// by other roles in those masters wait (items, customers and suppliers waiting can't be used on documents);
// the list below shows them with Approve. An Administrator's own saves are approved at once.

import { useCallback, useEffect, useState } from 'react';
import { CheckCircle2, Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { api, errorText, type ApiMasterApproval } from '../../lib/api';
import { panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead } from '../../lib/styles';

export default function ApprovalsTab() {
  const [data, setData] = useState<ApiMasterApproval | null>(null);
  const [picked, setPicked] = useState<Set<string> | null>(null);

  const load = useCallback(() => api<ApiMasterApproval>('/admin/master-approval')
    .then(d => { setData(d); setPicked(null); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  const selected = picked ?? new Set(data?.required ?? []);
  const toggle = (m: string) => { const n = new Set(selected); if (n.has(m)) n.delete(m); else n.add(m); setPicked(n); };

  const save = async () => {
    const body = { masters: [...selected] };
    if (!(await toastStore.defer('ok', 'Master approvals will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    api<ApiMasterApproval>('/admin/master-approval', { method: 'PUT', body })
      .then(d => { setData(d); setPicked(null); toastStore.add('ok', 'Master approvals saved.'); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  const approve = async (master: string, code: string) => {
    if (!(await toastStore.defer('ok', `${code} will be approved when this closes.`, 'Not approved.'))) return;
    api(`/admin/master-approval/${encodeURIComponent(master)}/${encodeURIComponent(code)}`, { method: 'POST' })
      .then(() => { toastStore.add('ok', `${code} approved.`); load(); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  return (
    <div className="grid grid-cols-1 gap-4 lg:grid-cols-[1fr_1.4fr]">
      <section className={`${panel} p-4`}>
        <h2 className={sectionTitle}>Masters that need approval</h2>
        <p className="mb-3 text-[11px] text-stone-500">New or changed rows saved by non-Administrators wait for approval.</p>
        <div className="space-y-1.5">
          {Object.entries(data?.masters ?? {}).map(([path, name]) => (
            <label key={path} className="flex items-center gap-2 text-xs">
              <input type="checkbox" checked={selected.has(path)} onChange={() => toggle(path)} />{name}
            </label>
          ))}
        </div>
        <div className="mt-4 flex justify-end gap-2">
          <button className={secondaryButton} onClick={() => setPicked(null)}>Cancel</button>
          <button className={primaryButton} onClick={save}><Save size={14} />Save</button>
        </div>
      </section>
      <section className={`${panel} overflow-hidden`}>
        <h2 className={`${sectionTitle} border-b border-line px-4 py-3`}>Waiting for approval <span className="font-normal text-stone-500">{data?.pending.length ?? 0}</span></h2>
        <table className="min-w-full text-xs">
          <thead><tr>{['Master', 'Code', 'Name', 'Saved by', ''].map((h, i) => <th key={i} className={`${tableHead} px-4 py-2`}>{h}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {data?.pending.map(p => (
              <tr key={`${p.master}/${p.code}`}>
                <td className="px-4 py-1.5">{p.screen}</td>
                <td className="px-4 py-1.5 font-mono">{p.code}</td>
                <td className="px-4 py-1.5">{p.name}</td>
                <td className="px-4 py-1.5">{p.changed_by ?? ''}</td>
                <td className="px-4 py-1.5 text-right"><button className={secondaryButton} onClick={() => approve(p.master, p.code)}><CheckCircle2 size={14} />Approve</button></td>
              </tr>
            ))}
            {!data?.pending.length && <tr><td colSpan={5} className="px-4 py-6 text-center text-stone-500">Nothing is waiting.</td></tr>}
          </tbody>
        </table>
      </section>
    </div>
  );
}
