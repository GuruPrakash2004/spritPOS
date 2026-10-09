// Report rights tab of Admin Master (Security) (legacy frmUtlAssignRight, "Tick the checkbox against the user for
// whom the report should be accessible!"): per Customised Report, the users who may run it. No ticks = everyone
// who can open Customised Reports. Administrators can always run every report. The server enforces it.

import { useCallback, useEffect, useState } from 'react';
import { Save, Search } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { errorText, getReportRights, saveReportRights, } from '../../lib/api';
import { field, label, panel, primaryButton, sectionTitle } from '../../lib/styles';

export default function ReportRightsTab() {
  const [data, setData] = useState(null);
  const [report, setReport] = useState('');
  const [search, setSearch] = useState('');
  const [picked, setPicked] = useState(null);

  const load = useCallback(() => getReportRights()
    .then(d => { setData(d); setPicked(null); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  const current = report || data?.reports[0] || '';
  const selected = picked ?? new Set(data?.rights[current] ?? []);
  const toggle = (u) => { const n = new Set(selected); if (n.has(u)) n.delete(u); else n.add(u); setPicked(n); };

  const save = async () => {
    if (!current) return;
    const body = { users: [...selected] };
    if (!(await toastStore.defer('ok', `Rights for "${current}" will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveReportRights(current, body)
      .then(() => { toastStore.add('ok', 'Report rights assigned successfully!'); load(); })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  const reports = (data?.reports ?? []).filter(r => r.toLowerCase().includes(search.toLowerCase()));
  return (
    <div className="grid grid-cols-1 gap-4 lg:grid-cols-[1.3fr_1fr]">
      <section className={`${panel} overflow-hidden`}>
        <div className="relative border-b border-line p-3">
          <Search size={14} className="pointer-events-none absolute left-6 top-1/2 -translate-y-1/2 text-stone-400" />
          <input aria-label="Search reports" value={search} onChange={e => setSearch(e.target.value)} className={`${field} pl-8`} placeholder="Search reports" />
        </div>
        <ul className="max-h-[55vh] overflow-auto text-xs" role="listbox" aria-label="Customised Reports">
          {reports.map(r => (
            <li key={r}>
              <button role="option" aria-selected={r === current} onClick={() => { setReport(r); setPicked(null); }}
                className={`flex w-full items-center justify-between px-4 py-2 text-left ${r === current ? 'bg-accent-soft font-semibold' : 'hover:bg-[#F7F9FC] dark:hover:bg-subtle'}`}>
                <span>{r}</span>
                <span className="text-[11px] text-stone-500">{data?.rights[r]?.length ? `${data.rights[r].length} user(s)` : 'everyone'}</span>
              </button>
            </li>
          ))}
        </ul>
      </section>
      <section className={`${panel} self-start p-4`}>
        <h2 className={sectionTitle}>{current || 'Pick a report'}</h2>
        <p className="mb-3 text-[11px] text-stone-500">Tick the users who may run this report. No ticks: everyone who can open Customised Reports.</p>
        <span className={label}>Users</span>
        <div className="space-y-1.5">
          {(data?.users ?? []).map(u => (
            <label key={u} className="flex items-center gap-2 text-xs"><input type="checkbox" checked={selected.has(u)} onChange={() => toggle(u)} />{u}</label>
          ))}
          {!data?.users.length && <p className="text-xs text-stone-500">Only Administrators exist.</p>}
        </div>
        <div className="mt-4 flex justify-end"><button className={primaryButton} onClick={save} disabled={!current}><Save size={14} />Assign rights</button></div>
      </section>
    </div>
  );
}
