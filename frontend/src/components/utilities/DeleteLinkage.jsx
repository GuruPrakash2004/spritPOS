// Global Settings → Other Settings → Delete Linkage Updation (legacy FrmUtlDeleteLinkageUpdation, Administrator only).
//
// The rules that stop a master record being deleted while another table uses it (the desktop's UtlDeletion table): pick the master table, name
// the document, pick the field checked (e.g. ItemID), press the magnifier to list every other table with a column of that name, keep the
// rows that matter and Save. Right: the tables that already have rules; click one to see it. Add / Edit / Delete / Save / Cancel work as in
// the exe's View / Add / Modify modes. A delete of a master record is then refused while one of the tables holds its value.

import { useCallback, useEffect, useState } from 'react';
import { ClipboardPaste, Copy, Download, Plus, Search, Trash2 } from 'lucide-react';
import ActionBar, { ScreenFrame } from '../common/ActionBar';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { useShortcuts } from '../../lib/keys';
import { downloadXlsRows } from '../../lib/xlsExport';
import { deleteLinkageGroup, errorText, getLinkageColumns, getLinkageGroup, getLinkageGroups, getLinkageTables, saveLinkageGroup, searchLinkage } from '../../lib/api';
import { field, label, panel, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const blankRow = () => ({ table: '', column: '', display: '', caption: '' });

export default function DeleteLinkage({ onClose }) {
  const [mode, setMode] = useState('V');   // V view, A add, M modify
  const [tables, setTables] = useState([]);
  const [groups, setGroups] = useState([]);
  const [cols, setCols] = useState({});   // table → its columns
  const [table, setTable] = useState('');
  const [docName, setDocName] = useState('');
  const [docField, setDocField] = useState('');
  const [rows, setRows] = useState([]);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const [current, setCurrent] = useState(-1);   // the grid row last used
  const [clip, setClip] = useState(null);   // the copied row
  const { confirm, dialog } = useConfirm();
  const editing = mode !== 'V';

  const loadGroups = useCallback(() => getLinkageGroups().then(setGroups).catch(e => setProblem(errorText(e))), []);
  // The lists are read when the tab opens. If the server can't reach the database at that moment, try again every few seconds (it reconnects by
  // itself) instead of leaving an empty screen with an old error.
  const [loadError, setLoadError] = useState('');
  const [tries, setTries] = useState(0);
  useEffect(() => {
    let live = true;
    let timer;
    Promise.all([getLinkageTables(), getLinkageGroups()])
      .then(([t, g]) => { if (live) { setTables(t); setGroups(g); setLoadError(''); } })
      .catch(e => { if (live) { setLoadError(errorText(e)); if (tries < 30) timer = setTimeout(() => setTries(n => n + 1), 8000); } });
    return () => { live = false; clearTimeout(timer); };
  }, [tries]);

  // The column lists of the tables on screen are read once each.
  useEffect(() => {
    const wanted = [...new Set([table, ...rows.map(r => r.table)].filter(t => t && !cols[t]))];
    wanted.forEach(t => {
      getLinkageColumns(t).then(c => setCols(prev => ({ ...prev, [t]: c }))).catch(() => setCols(prev => ({ ...prev, [t]: [] })));
    });
  }, [table, rows, cols]);

  const clear = () => { setMode('V'); setTable(''); setDocName(''); setDocField(''); setRows([]); setProblem(''); };
  const open = async (t) => {
    try {
      const g = await getLinkageGroup(t);
      setMode('V'); setTable(g.table); setDocName(g.doc_name); setDocField(g.doc_field); setRows([...g.rows, blankRow()]); setProblem('');
    } catch (e) { setProblem(errorText(e)); }
  };
  const setRow = (i, patch) => setRows(rs => rs.map((r, j) => (j === i ? { ...r, ...patch } : r)));

  const magnifier = async () => {
    if (!table) return setProblem('Select Table name');
    if (!docField) return setProblem('Select Field to be Checked for Deletion');
    try { setRows([...(await searchLinkage(table, docField)), blankRow()]); setProblem(''); } catch (e) { setProblem(errorText(e)); }
  };

  const save = async () => {
    // The exe's checks, in its order (the server repeats them).
    if (!table) return setProblem('Table name is Empty...');
    if (!docName.trim()) return setProblem('Document name is Empty...');
    const real = rows.filter(r => r.table && r.column);
    if (!rows.length) return setProblem('Data is not available');
    if (!docField) return setProblem('Column is Empty....');
    if (!(await toastStore.defer('ok', 'Delete linkage will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      await saveLinkageGroup(table, { doc_name: docName.trim(), doc_field: docField, rows: real });
      toastStore.add('ok', 'Saved Successfully!');
      await loadGroups();
      await open(table);
    } catch (e) { setProblem(errorText(e)); } finally { setBusy(false); }
  };

  const remove = async () => {
    if (!table) return;
    if (!(await confirm('Are you sure you want to delete ?'))) return;
    if (!(await toastStore.defer('delete', 'Delete linkage will be deleted when this closes.', 'Delete cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      await deleteLinkageGroup(table);
      toastStore.add('delete', 'Deleted Successfully!');
      clear();
      await loadGroups();
    } catch (e) { setProblem(errorText(e)); } finally { setBusy(false); }
  };

  // The exe's grid tools (ucBtnTools, Ctrl+Shift+I / C / P / E / R): insert, copy, paste, export and remove a row.
  const at = current >= 0 && current < rows.length ? current : rows.length - 1;
  const insertRow = () => { if (editing) setRows(rs => [...rs.slice(0, at + 1), blankRow(), ...rs.slice(at + 1)]); };
  const copyRow = () => { if (rows[at]) setClip({ ...rows[at] }); };
  const pasteRow = () => { if (editing && clip) setRows(rs => [...rs.slice(0, at + 1), { ...clip }, ...rs.slice(at + 1)]); };
  const exportRows = () => downloadXlsRows('Delete Linkage', [['Table', 'Column', 'Display Column', 'Doc Name'], ...rows.map(r => [r.table, r.column, r.display, r.caption])]);
  const removeRow = async () => {
    if (!rows.length || !table) return setProblem('No data to Remove');
    if (current < 0 || !rows[current]) return setProblem('Click the row you want to remove first.');
    if (!(await confirm('Are you sure you want to Remove ?', 'Deletion Confirmation...'))) return;
    // Removing a row changes the rules, so a saved group goes into Modify mode: Save then writes it.
    if (!editing) setMode('M');
    setRows(rs => rs.filter((_, j) => j !== current));
    setCurrent(-1);
    setProblem('');
  };
  useShortcuts({ 'Mod+Shift+I': insertRow, 'Mod+Shift+C': copyRow, 'Mod+Shift+P': pasteRow, 'Mod+Shift+E': exportRows, 'Mod+Shift+R': () => { void removeRow(); } });
  // Tab past the last row's Display Column, with its Table and Column filled, adds a new row.
  const tabAdds = (e, i) => { if (e.key === 'Tab' && !e.shiftKey && i === rows.length - 1 && rows[i].table && rows[i].column) setRows(rs => [...rs, blankRow()]); };

  const pick = (list, current) => (
    <>
      <option value="" />
      {current && !list.includes(current) && <option value={current}>{current}</option>}
      {list.map(x => <option key={x} value={x}>{x}</option>)}
    </>
  );

  const startNew = () => { clear(); setMode('A'); };
  const bar = (
    <ActionBar mode={{ V: 'View', A: 'New', M: 'Edit' }[mode]} onNew={startNew} onEdit={() => { setMode('M'); setProblem(''); }} canEdit={!!table}
      onDelete={() => void remove()} canDelete={!!table && !busy} onSave={() => void save()} onCancel={() => (table ? void open(table) : clear())} saving={busy} onClose={onClose} />
  );

  return (
    <ScreenFrame bar={bar}>
    <section className={`${panel} space-y-4 p-4`} aria-label="Delete linkage updation">
      {dialog}
      <p className="text-xs font-semibold text-stone-500" aria-live="polite">{{ V: 'View mode', A: 'New mode', M: 'Edit mode' }[mode]}</p>
      <div className="grid gap-5 lg:grid-cols-[minmax(0,1fr)_minmax(220px,300px)]">
        <div className="space-y-3">
          <div className="grid gap-3 sm:grid-cols-2">
            <label><span className={label}>Table Name</span>
              <select value={table} disabled={!editing} onChange={e => { setTable(e.target.value); setDocField(''); setProblem(''); }} className={field}>{pick(tables, table)}</select></label>
            <label><span className={label}>Document Name</span>
              <input value={docName} disabled={!editing} maxLength={200} onChange={e => { setDocName(e.target.value); setProblem(''); }} className={field} /></label>
          </div>
          <div className="flex items-end gap-3">
            <label className="min-w-0 flex-1"><span className={label}>Field To be Checked for Deletion</span>
              <select value={docField} disabled={!editing || !table} onChange={e => { setDocField(e.target.value); setProblem(''); }} className={field}>{pick(cols[table] ?? [], docField)}</select></label>
            <button type="button" className={secondaryButton} disabled={!editing} onClick={() => void magnifier()} aria-label="Find tables with this column"><Search size={14} /></button>
          </div>
          <div className="overflow-x-auto rounded-xl border border-[#E3D9CD] dark:border-[#2A2A2A]">
            <table className="w-full text-xs">
              <thead><tr><th className={tableHead}>Table</th><th className={tableHead}>Column</th><th className={tableHead}>Display Column</th><th className={tableHead}>Doc Name</th></tr></thead>
              <tbody className={tableBody}>
                {rows.map((r, i) => (
                  <tr key={i} aria-selected={i === current} className={`cursor-pointer transition-colors ${i === current ? 'bg-[#EBC9D3] shadow-[inset_4px_0_0_#64283A] dark:bg-[#D4A847]/25 dark:shadow-[inset_4px_0_0_#D4A847]' : 'hover:bg-[#FAF6EF] dark:hover:bg-subtle/70'}`} onClick={() => setCurrent(i)} onFocusCapture={() => setCurrent(i)}>
                    <td className="p-1"><select aria-label={`Table ${i + 1}`} value={r.table} disabled={!editing} onChange={e => setRow(i, { table: e.target.value, column: '', display: '' })} className={field}>{pick(tables, r.table)}</select></td>
                    <td className="p-1"><select aria-label={`Column ${i + 1}`} value={r.column} disabled={!editing || !r.table} onChange={e => setRow(i, { column: e.target.value })} className={field}>{pick(cols[r.table] ?? [], r.column)}</select></td>
                    <td className="p-1"><select aria-label={`Display column ${i + 1}`} value={r.display} disabled={!editing || !r.table} onChange={e => setRow(i, { display: e.target.value })} onKeyDown={e => tabAdds(e, i)} className={field}>{pick(cols[r.table] ?? [], r.display)}</select></td>
                    <td className="px-2 py-1 text-stone-600 dark:text-stone-300">{r.caption}</td>
                  </tr>
                ))}
                {!rows.length && <tr><td colSpan={4} className="px-3 py-6 text-center text-stone-500">Pick the table and field, then press the magnifier.</td></tr>}
              </tbody>
            </table>
          </div>
          <div className="flex flex-wrap gap-2" role="toolbar" aria-label="Grid tools">
            {editing && <button type="button" className={secondaryButton} onClick={insertRow} title="Insert row (Ctrl+Shift+I)"><Plus size={14} />Insert</button>}
            <button type="button" className={secondaryButton} onClick={exportRows} title="Export (Ctrl+Shift+E)"><Download size={14} />Export</button>
            <button type="button" className={secondaryButton} onClick={copyRow} disabled={!rows.length} title="Copy row (Ctrl+Shift+C)"><Copy size={14} />Copy</button>
            {editing && <button type="button" className={secondaryButton} onClick={pasteRow} disabled={!clip} title="Paste row (Ctrl+Shift+P)"><ClipboardPaste size={14} />Paste</button>}
            <button type="button" className={secondaryButton} onClick={() => void removeRow()} disabled={!rows.length} title="Remove the selected row (Ctrl+Shift+R)"><Trash2 size={14} />Remove</button>
          </div>
        </div>
        <div>
          <div className="overflow-hidden rounded-xl border border-[#E3D9CD] dark:border-[#2A2A2A]">
            <div className={`${tableHead} px-3 py-2`}>Document Name</div>
            <ul className="max-h-96 overflow-auto text-xs">
              {groups.map(g => (
                <li key={g.table}><button type="button" onClick={() => void open(g.table)} disabled={editing}
                  className={`block w-full px-3 py-1.5 text-left ${g.table === table ? 'bg-[#64283A] font-semibold text-white shadow-[inset_4px_0_0_#D4A847] dark:bg-[#D4A847] dark:text-[#180E09] dark:shadow-[inset_4px_0_0_#64283A]' : 'hover:bg-[#F8EDDA] dark:hover:bg-[#D4A847]/10'}`}>{g.name}</button></li>
              ))}
              {!groups.length && <li className="px-3 py-4 text-stone-500">No rules yet.</li>}
            </ul>
          </div>
        </div>
      </div>
      {loadError && (
        <div className="flex flex-wrap items-center gap-3">
          <InlineError message={`${loadError} Trying again...`} />
          <button type="button" className={secondaryButton} onClick={() => setTries(n => n + 1)}>Try now</button>
        </div>
      )}
      {problem && <InlineError message={problem} />}
    </section>
    </ScreenFrame>
  );
}
