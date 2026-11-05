// Shared Documents (legacy frmUtlSharedDocuments): the documents the shop lists as shared, by group, read from the database's
// UtlSharedDocs. The list is read-only here and the files themselves stay on the desktop's shared folder.

import { useEffect, useMemo, useState } from 'react';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getSharedDocuments } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, tableBody, tableHead } from '../../lib/styles';

export default function SharedDocuments() {
  const [rows, setRows] = useState(null);
  const [group, setGroup] = useState('');
  const [search, setSearch] = useState('');
  const [problem, setProblem] = useState('');

  useEffect(() => { getSharedDocuments().then(setRows).catch(e => { setRows([]); setProblem(errorText(e)); }); }, []);

  const groups = useMemo(() => [...new Set((rows || []).map(r => r.group).filter(Boolean))].sort(), [rows]);
  const text = search.trim().toLowerCase();
  const shown = (rows || []).filter(r => (!group || r.group === group) && (!text || r.file.toLowerCase().includes(text) || r.remarks.toLowerCase().includes(text)));

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Shared Documents</h1>
          <p className={pageSub}>The documents this shop shares, by group. The files are kept on the desktop's shared folder.</p>
        </div>
      </div>
      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div>
          <label htmlFor="sd-group" className={label}>Group</label>
          <select id="sd-group" value={group} onChange={e => setGroup(e.target.value)} className={`${field} min-w-44`}>
            <option value="">All groups</option>
            {groups.map(g => <option key={g} value={g}>{g}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="sd-search" className={label}>Search</label>
          <input id="sd-search" value={search} onChange={e => setSearch(e.target.value)} className={`${field} w-64`} />
        </div>
        <span className="ml-auto text-xs text-stone-500">{rows ? `${shown.length} of ${rows.length} document(s)` : ''}</span>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Group</th><th className="px-3 py-2">Document</th><th className="px-3 py-2">Remarks</th></tr></thead>
          <tbody className={tableBody}>
            {rows && !shown.length && <tr><td colSpan={3} className="px-4 py-14 text-center text-stone-500">{rows.length ? 'No document matches.' : 'This shop has no shared documents.'}</td></tr>}
            {shown.map(r => <tr key={r.id}><td className="px-3 py-1.5">{r.group}</td><td className="px-3 py-1.5">{r.file}</td><td className="px-3 py-1.5">{r.remarks}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}
