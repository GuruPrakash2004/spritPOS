// Shared Documents (legacy frmUtlSharedDocuments): the documents the shop lists as shared, by group, read from the database's
// UtlSharedDocs. The list is read-only here and the files themselves stay on the desktop's shared folder.

import { useEffect, useMemo, useState } from 'react';
import { FileSpreadsheet } from 'lucide-react';
import { InlineError } from '../common/FormErrorSummary';
import { toastStore } from '../common/toastStore';
import { errorText, getSharedDocuments } from '../../lib/api';
import { downloadXls } from '../../lib/xlsExport';
import { useShortcuts } from '../../lib/keys';
import { exportButton, field, label, pageHeader, pageSub, pageTitle, panel, tableBody, tableHead } from '../../lib/styles';

// The desktop's File Type column: the extension of the file name, upper-cased ("PDF", "XLSX"), blank when it has none.
const fileType = (name) => { const m = /.([A-Za-z0-9]{1,6})$/.exec(name || ''); return m ? m[1].toUpperCase() : ''; };

export default function SharedDocuments() {
  const [rows, setRows] = useState(null);
  const [group, setGroup] = useState('');
  const [search, setSearch] = useState('');
  const [problem, setProblem] = useState('');

  useEffect(() => { getSharedDocuments().then(setRows).catch(e => { setRows([]); setProblem(errorText(e)); }); }, []);

  const groups = useMemo(() => [...new Set((rows || []).map(r => r.group).filter(Boolean))].sort(), [rows]);
  const text = search.trim().toLowerCase();
  const shown = (rows || []).filter(r => (!group || r.group === group) && (!text || r.file.toLowerCase().includes(text) || r.remarks.toLowerCase().includes(text)));

  // Export to Excel (legacy btnExportToXL, Alt+X): the shown rows with the desktop's columns.
  const exportList = async () => {
    if (!shown.length) return toastStore.add('error', 'Nothing to export: no documents are listed.');
    if (!(await toastStore.defer('ok', `${shown.length} document(s) will be exported to Excel when this closes.`, 'Export cancelled.'))) return;
    const cols = [['group', 'Group'], ['type', 'File Type'], ['file', 'File Name'], ['remarks', 'Remarks']].map(([key, labelText]) => ({ key, label: labelText, align: 'left' }));
    await downloadXls('Shared_Documents', 'Shared Documents', '', cols, shown.map(r => ({ group: r.group, type: fileType(r.file), file: r.file, remarks: r.remarks })));
    toastStore.add('ok', `${shown.length} document(s) exported to Excel.`);
  };
  useShortcuts({ 'Alt+X': () => { void exportList(); } });

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
        <button type="button" className={exportButton} disabled={!shown.length} onClick={() => void exportList()} title="Export to Excel (Alt+X)"><FileSpreadsheet size={14} />Export to Excel</button>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="px-3 py-2">Group</th><th className="px-3 py-2">File Type</th><th className="px-3 py-2">File Name</th><th className="px-3 py-2">Remarks</th></tr></thead>
          <tbody className={tableBody}>
            {rows && !shown.length && <tr><td colSpan={4} className="px-4 py-14 text-center text-stone-500">{rows.length ? 'No document matches.' : 'This shop has no shared documents.'}</td></tr>}
            {shown.map(r => <tr key={r.id}><td className="px-3 py-1.5">{r.group}</td><td className="px-3 py-1.5">{fileType(r.file)}</td><td className="px-3 py-1.5">{r.file}</td><td className="px-3 py-1.5">{r.remarks}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}
