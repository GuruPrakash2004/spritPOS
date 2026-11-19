// Global Settings → Other Settings → Bulk Item Import Fields (the desktop's "UtlItemBulkUpdate" window, frmUtlBulkImpFlds): "Add Fields To Import in
// item Master", a grid of Field Name From Item Master (a drop-down of the Item Master's columns) and Data Type (Text / Integer / Date / Numeric).
// Save replaces the whole list in the shop's UtlItemBulkUpdate table; that list is the fields Options → Update Item Master Fields offers.
// Administrator only (backend: /utilities/bulk-import-fields). The save waits for its popup; mistakes are shown inline.

import { useCallback, useEffect, useState } from 'react';
import { Plus, Save, Trash2 } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getBulkImportFields, saveBulkImportFields } from '../../lib/api';
import { iconButton, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const select = 'w-full rounded-md border border-[#E3D9CD] bg-white px-2 py-1 text-xs dark:border-[#2A2A2A] dark:bg-[#141414]';
const sameType = (a, b) => String(a).toUpperCase() === String(b).toUpperCase();
const asRows = (fields) => fields.map(f => ({ name: f.name, datatype: f.datatype }));

export default function BulkImportFields() {
  const [data, setData] = useState(null);      // { fields, columns, types } as the server has it
  const [rows, setRows] = useState([]);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  const load = useCallback(() => getBulkImportFields()
    .then(d => { setData(d); setRows(asRows(d.fields)); setError(''); })
    .catch(e => setError(errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  if (!data) return error ? <InlineError message={error} /> : <p className="text-xs text-stone-500">Loading…</p>;

  const dirty = JSON.stringify(rows.filter(r => r.name)) !== JSON.stringify(asRows(data.fields));
  const edit = (i, patch) => { setRows(rs => rs.map((r, j) => (j === i ? { ...r, ...patch } : r))); setError(''); };
  // The drop-down lists the types in the exe's spelling; a stored type in another case (INTEGER, Numeric) still shows as chosen.
  const typeOf = (t) => data.types.find(x => sameType(x, t)) ?? '';

  const save = async () => {
    const used = rows.filter(r => r.name);
    const seen = new Set();
    for (const r of used) {
      if (seen.has(r.name.toUpperCase())) { setError(`Duplicate field '${r.name}'`); return; }
      seen.add(r.name.toUpperCase());
      if (!typeOf(r.datatype)) { setError(`Select the Data Type of ${r.name}`); return; }
    }
    if (!(await toastStore.defer('ok', 'The import fields will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setSaving(true);
    try {
      const res = await saveBulkImportFields(used.map(r => ({ name: r.name, datatype: typeOf(r.datatype) })));
      toastStore.add('ok', res.message);
      await load();
    } catch (e) {
      setError(errorText(e));
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className={`${panel} mx-auto max-w-xl space-y-3 p-4`} aria-label="Bulk Item Import Fields">
      <div className="flex items-center justify-between gap-2">
        <div>
          <h2 className="text-sm font-semibold">Add Fields To Import in item Master</h2>
          <p className="text-[11px] text-stone-500">These are the fields Item Master → Options → Update Item Master Fields offers for a bulk change.</p>
        </div>
        <div className="flex gap-2">
          <button onClick={load} disabled={!dirty} className={secondaryButton}>Undo</button>
          <button onClick={save} disabled={!dirty || saving} className={primaryButton}><Save size={14} />Save</button>
        </div>
      </div>
      <div className="max-h-[calc(100vh-20rem)] overflow-auto rounded-lg border border-[#E3D9CD] dark:border-[#2A2A2A]">
        <table className="w-full text-xs">
          <thead className={`${tableHead} sticky top-0`}>
            <tr><th className="px-3 py-2 text-left">Field Name From Item Master</th><th className="w-40 px-3 py-2 text-left">Data Type</th><th className="w-10" /></tr>
          </thead>
          <tbody className={tableBody}>
            {rows.map((r, i) => (
              <tr key={i}>
                <td className="px-3 py-1">
                  <select aria-label={`Field name ${i + 1}`} value={r.name} onChange={e => edit(i, { name: e.target.value })} className={select}>
                    <option value="" />
                    {[...new Set([...data.columns, ...(r.name ? [r.name] : [])])].map(c => <option key={c} value={c}>{c}</option>)}
                  </select>
                </td>
                <td className="px-3 py-1">
                  <select aria-label={`Data type ${i + 1}`} value={typeOf(r.datatype)} onChange={e => edit(i, { datatype: e.target.value })} className={select}>
                    <option value="" />
                    {data.types.map(t => <option key={t} value={t}>{t}</option>)}
                  </select>
                </td>
                <td className="px-1"><button type="button" aria-label={`Remove row ${i + 1}`} title="Remove row" onClick={() => { setRows(rs => rs.filter((_, j) => j !== i)); setError(''); }} className={iconButton}><Trash2 size={14} /></button></td>
              </tr>
            ))}
            {!rows.length && <tr><td colSpan={3} className="px-3 py-4 text-center text-stone-500">No fields yet. Add one below.</td></tr>}
          </tbody>
        </table>
      </div>
      <button type="button" onClick={() => setRows(rs => [...rs, { name: '', datatype: '' }])} className={secondaryButton}><Plus size={14} />Add field</button>
      {error && <InlineError message={error} />}
    </section>
  );
}
