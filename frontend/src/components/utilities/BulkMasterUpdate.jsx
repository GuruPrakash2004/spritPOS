// Bulk Master Update (legacy frmUtlBulkUpdate): load items (all, or one group), tick the ones to change, pick a field,
// type the new value and Apply: POST /masters/items/bulk-update sets that one field on every ticked item.
// Only fields the web Item Master has are offered (the desktop's own list also names min/max level and KSBCL names).

import { useEffect, useState } from 'react';
import { CheckCircle2, Download } from 'lucide-react';
import Pager from '../common/Pager';
import { usePage } from '../common/usePage';
import { toastStore } from '../common/toastStore';
import { bulkUpdateItems, errorText, getBrands, getItemGroups, getItems, } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';


const FIELDS = [
  { key: 'hsn_code', label: 'HSN code' }, { key: 'brand', label: 'Brand' }, { key: 'liqtype', label: 'Liquor type' },
  { key: 'bottles_per_case', label: 'Bottles per case', numeric: true }, { key: 'mrp', label: 'MRP', numeric: true },
  { key: 'purchase_rate', label: 'Purchase rate', numeric: true }, { key: 'rol', label: 'Re-order level (ROL)', numeric: true },
  { key: 'roq', label: 'Re-order quantity (ROQ)', numeric: true },
];

export default function BulkMasterUpdate() {
  const [groups, setGroups] = useState([]);
  const [brands, setBrands] = useState([]);
  const [group, setGroup] = useState('');
  const [fieldKey, setFieldKey] = useState('hsn_code');
  const [value, setValue] = useState('');
  const [items, setItems] = useState(null);
  const [picked, setPicked] = useState(new Set());
  const [error, setError] = useState('');
  const spec = FIELDS.find(f => f.key === fieldKey);
  const pager = usePage(items ?? []);

  useEffect(() => {
    getItemGroups().then(g => setGroups(g.map(x => x.code))).catch(() => undefined);
    getBrands().then(b => setBrands(b.map(x => x.code))).catch(() => undefined);
  }, []);

  const load = () => {
    getItems()
      .then(all => {
        const list = group ? all.filter(i => i.group === group) : all;
        setItems(list);
        setPicked(new Set());
        setError('');
      })
      .catch(e => toastStore.add('error', errorText(e)));
  };

  const toggle = (code) => setPicked(p => { const n = new Set(p); if (n.has(code)) n.delete(code); else n.add(code); return n; });

  const apply = async () => {
    if (!picked.size) return setError('Tick at least one item.');
    if (value.trim() === '') return setError(`Enter the new ${spec.label.toLowerCase()}.`);
    if (spec.numeric && Number.isNaN(Number(value))) return setError(`${spec.label} must be a number.`);
    setError('');
    const body = { item_codes: [...picked], field: fieldKey, value: spec.numeric ? Number(value) : value.trim() };
    if (!(await toastStore.defer('ok', `${spec.label} of ${picked.size} items will be set to ${value} when this closes.`, 'Update cancelled. Nothing was changed.'))) return;
    try {
      const res = await bulkUpdateItems(body);
      toastStore.add('ok', `${spec.label} updated on ${res.changed} items.`);
      setValue('');
      load();
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Bulk Master Update</h1>
          <p className={pageSub}>Set one Item Master field to one value on many items at once.</p>
        </div>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div>
          <label htmlFor="bm-group" className={label}>Item Group</label>
          <select id="bm-group" value={group} onChange={e => setGroup(e.target.value)} className={field}>
            <option value="">All groups</option>
            {groups.map(g => <option key={g}>{g}</option>)}
          </select>
        </div>
        <button onClick={load} className={secondaryButton}><Download size={14} />Load</button>
        <div className="ml-auto">
          <label htmlFor="bm-field" className={label}>Field to change</label>
          <select id="bm-field" value={fieldKey} onChange={e => { setFieldKey(e.target.value ); setValue(''); }} className={field}>
            {FIELDS.map(f => <option key={f.key} value={f.key}>{f.label}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="bm-value" className={label}>New value</label>
          {fieldKey === 'brand' ? (
            <select id="bm-value" value={value} onChange={e => setValue(e.target.value)} className={field}>
              <option value="">Select…</option>
              {brands.map(b => <option key={b}>{b}</option>)}
            </select>
          ) : (
            <input id="bm-value" type={spec.numeric ? 'number' : 'text'} value={value} onChange={e => setValue(e.target.value)}
              className={`${field} w-40 ${spec.numeric ? 'text-right tabular-nums' : ''}`} />
          )}
        </div>
        <button onClick={apply} disabled={!items} className={primaryButton}><CheckCircle2 size={14} />Apply to {picked.size || ''} ticked</button>
      </div>
      {error && <p role="alert" className="text-xs text-red-600 dark:text-red-400">{error}</p>}

      <div className={`${panel} overflow-hidden`}>
        <div className="max-h-[520px] overflow-auto">
          <table className="w-full text-xs">
            <thead className={tableHead}>
              <tr>
                <th className="px-3 py-2">
                  <input type="checkbox" aria-label="Select all" className="accent-accent" checked={Boolean(items?.length) && picked.size === items?.length}
                    onChange={e => setPicked(new Set(e.target.checked ? items?.map(i => i.code) : []))} />
                </th>
                <th className="px-3 py-2">Code</th><th className="px-3 py-2">Item</th><th className="px-3 py-2">Group</th>
                <th className="px-3 py-2">Now: {spec.label}</th>
              </tr>
            </thead>
            <tbody className={`${tableBody} tabular-nums`}>
              {!items && <tr><td colSpan={5} className="px-3 py-10 text-center font-sans text-stone-500">Choose a group and press Load.</td></tr>}
              {pager.slice.map(i => (
                <tr key={i.code}>
                  <td className="px-3 py-1.5"><input type="checkbox" className="accent-accent" aria-label={`Select ${i.code}`} checked={picked.has(i.code)} onChange={() => toggle(i.code)} /></td>
                  <td className="px-3 py-1.5 font-medium">{i.code}</td>
                  <td className="px-3 py-1.5 font-sans">{i.name}</td>
                  <td className="px-3 py-1.5 font-sans">{i.group}</td>
                  <td className="px-3 py-1.5">{String(i[fieldKey] ?? '')}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <Pager p={pager} />
      </div>
    </div>
  );
}
