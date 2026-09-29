// Price List Maintenance (legacy frmMstPriceMaintenance): choose an item group, Load, change MRP or
// purchase rate by an amount, preview, then Apply via POST /masters/items/price-list.

import { useEffect, useState } from 'react';
import { CheckCircle2, Download } from 'lucide-react';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { api, errorText, type ApiItemFull } from '../../lib/api';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader } from '../../lib/styles';

type Column = 'mrp' | 'purchase_rate';

// Legacy frmMstPriceMaintenance: select by item group or items, Load, change a rate column by an amount, Apply.
export default function PriceListMaintenance() {
  const [groups, setGroups] = useState<string[]>([]);
  const [group, setGroup] = useState('');
  const [column, setColumn] = useState<Column>('mrp');
  const [items, setItems] = useState<ApiItemFull[] | null>(null);
  const [picked, setPicked] = useState<Set<string>>(new Set());
  const [change, setChange] = useState('');
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => {
    api<{ code: string }[]>('/masters/item-groups').then(g => setGroups(g.map(x => x.code))).catch(() => undefined);
  }, []);

  const load = () => {
    api<ApiItemFull[]>('/masters/items')
      .then(all => {
        const list = group ? all.filter(i => i.group === group) : all;
        setItems(list);
        setPicked(new Set(list.map(i => i.code)));
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const delta = Number(change) || 0;
  const toggle = (code: string) => {
    const next = new Set(picked);
    if (next.has(code)) next.delete(code); else next.add(code);
    setPicked(next);
  };

  const apply = async () => {
    if (!picked.size) return setToast({ kind: 'error', text: 'Select at least one item.' });
    if (!delta) return setToast({ kind: 'error', text: 'Enter the amount to change the rate by.' });
    if (!(await toastStore.defer('ok', `${column === 'mrp' ? 'MRP' : 'Purchase rate'} for ${picked.size} items will be updated when this closes.`, 'Rate update cancelled. Nothing was changed.'))) return;
    api<ApiItemFull[]>('/masters/items/price-list', { method: 'POST', body: { item_codes: [...picked], column, change_by: delta } })
      .then(updated => {
        setToast({ kind: 'ok', text: `Updated ${column === 'mrp' ? 'MRP' : 'purchase rate'} for ${updated.length} items.` });
        setChange('');
        load();
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Item Price List Maintenance</h1>
          <p className={pageSub}>Change MRP or purchase rate for many items at once.</p>
        </div>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>
        <div>
          <label htmlFor="pl-group" className={label}>Item Group</label>
          <select id="pl-group" value={group} onChange={e => setGroup(e.target.value)} className={field}>
            <option value="">All groups</option>
            {groups.map(g => <option key={g}>{g}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="pl-col" className={label}>Rate Column</label>
          <select id="pl-col" value={column} onChange={e => setColumn(e.target.value as Column)} className={field}>
            <option value="mrp">MRP</option>
            <option value="purchase_rate">Purchase Rate</option>
          </select>
        </div>
        <button onClick={load} className={secondaryButton}><Download size={14} />Load</button>
        <div className="ml-auto">
          <label htmlFor="pl-change" className={label}>Change by ₹ (negative to reduce)</label>
          <input id="pl-change" type="number" step="0.01" value={change} onChange={e => setChange(e.target.value)} className={`${field} w-36 text-right tabular-nums`} />
        </div>
        <button onClick={apply} disabled={!items} className={primaryButton}><CheckCircle2 size={14} />Apply</button>
      </div>

      <div className={`${panel} max-h-[520px] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>
              <th className="px-3 py-2">
                <input type="checkbox" aria-label="Select all" className="accent-accent" checked={Boolean(items?.length) && picked.size === items?.length}
                  onChange={e => setPicked(new Set(e.target.checked ? items?.map(i => i.code) : []))} />
              </th>
              <th className="px-3 py-2">Code</th><th className="px-3 py-2">Item</th><th className="px-3 py-2">Group</th>
              <th className="px-3 py-2 text-right">Current</th><th className="px-3 py-2 text-right">New</th>
            </tr>
          </thead>
          <tbody className={`${tableBody} tabular-nums`}>
            {!items && <tr><td colSpan={6} className="px-3 py-10 text-center font-sans text-stone-400">Choose a group and press Load.</td></tr>}
            {items?.map(i => {
              const on = picked.has(i.code);
              const current = i[column] ?? 0;
              return (
                <tr key={i.code} className={on ? '' : 'opacity-50'}>
                  <td className="px-3 py-1.5"><input type="checkbox" checked={on} onChange={() => toggle(i.code)} className="accent-accent" aria-label={`Select ${i.name}`} /></td>
                  <td className="px-3 py-1.5">{i.code}</td>
                  <td className="px-3 py-1.5 font-sans text-stone-900 dark:text-stone-100">{i.name}</td>
                  <td className="px-3 py-1.5 font-sans">{i.group}</td>
                  <td className="px-3 py-1.5 text-right">{money(current)}</td>
                  <td className={`px-3 py-1.5 text-right font-bold ${on && delta ? 'text-accent-ink' : ''}`}>{money(on ? Math.max(0, current + delta) : current)}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}
