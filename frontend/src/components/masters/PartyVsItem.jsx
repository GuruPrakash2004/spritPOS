// Customer Vs Item Master / Supplier Vs Item Master (the desktop's frmPartyItem, under Customer Related / Supplier Related): the items a party has its
// own rate for. Pick the party, Load, then add items (code or name), set each one's Rate, effective From / To dates and Remarks, and Update.
// Update replaces the party's rows in MstCUSTITEM / MstSUPPITEM, as the desktop does. Import from XL reads a CSV saved from Excel with the columns
// Item Code, Rate, From, To, Remarks (From and To as yyyy-mm-dd, optional). Cancel throws the changes away.
//
//   <PartyVsItem party="customers" />   <PartyVsItem party="suppliers" />

import { useEffect, useMemo, useRef, useState } from 'react';
import { Download, Plus, Save, Trash2, Upload, X } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { canDo, errorText, getCustomers, getItems, getPartyRecords, getSuppliers, savePartyRecords } from '../../lib/api';
import { parseCsv } from '../../lib/csv';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const WORDS = {
  customers: { title: 'Customer Vs Item Master', party: 'Customer', master: 'Customer Master', load: getCustomers },
  suppliers: { title: 'Supplier Vs Item Master', party: 'Supplier', master: 'Supplier Master', load: getSuppliers },
};
const blank = () => ({ item_code: '', item_name: '', rate: '', eff_from: '', eff_to: '', remarks: '' });

function PartyVsItem({ party }) {
  const w = WORDS[party];
  const [parties, setParties] = useState([]);
  const [items, setItems] = useState([]);
  const [code, setCode] = useState('');
  const [rows, setRows] = useState(null);       // null until Load
  const [dirty, setDirty] = useState(false);
  const [problem, setProblem] = useState('');
  const picker = useRef(null);
  const { confirm, dialog } = useConfirm();
  const canEdit = canDo(`${w.title}`, 'edit') || canDo(w.master, 'edit');

  useEffect(() => {
    w.load().then(l => setParties(l.filter(p => p.code !== 'CASH'))).catch(e => setProblem(errorText(e)));
    getItems().then(setItems).catch(() => undefined);
  }, [w]);

  const byCode = useMemo(() => new Map(items.map(i => [i.code.toLowerCase(), i])), [items]);
  const nameOf = (c) => byCode.get(String(c).trim().toLowerCase())?.name ?? '';

  const load = async () => {
    if (!code) return setProblem(`Please Select ${w.party}`);
    setProblem('');
    try {
      const r = await getPartyRecords(party, code, 'item-rates');
      setRows(r.rows.map(x => ({ ...blank(), ...x, item_name: nameOf(x.item_code) })));
      setDirty(false);
    } catch (e) { setProblem(errorText(e)); }
  };

  const edit = (i, patch) => { setRows(rs => rs.map((r, j) => (j === i ? { ...r, ...patch } : r))); setDirty(true); };
  const setItem = (i, text) => {
    const hit = byCode.get(text.trim().toLowerCase()) ?? items.find(x => x.name.toLowerCase() === text.trim().toLowerCase());
    edit(i, hit ? { item_code: hit.code, item_name: hit.name } : { item_code: text, item_name: '' });
  };
  const add = () => { setRows(rs => [...rs, blank()]); setDirty(true); };
  const remove = (i) => { setRows(rs => rs.filter((_, j) => j !== i)); setDirty(true); };

  const importCsv = async (e) => {
    const file = e.target.files?.[0];
    e.target.value = '';
    if (!file) return;
    const table = parseCsv(await file.text());
    const head = (table[0] ?? []).map(h => h.trim().toLowerCase());
    const col = (n) => head.indexOf(n);
    if (col('item code') < 0 || col('rate') < 0) return setProblem('Invalid file selected, please select the right file: the first row must have the columns Item Code and Rate.');
    const added = table.slice(1).map(r => ({ ...blank(), item_code: (r[col('item code')] ?? '').trim(), rate: (r[col('rate')] ?? '').trim(),
      eff_from: (r[col('from')] ?? '').trim(), eff_to: (r[col('to')] ?? '').trim(), remarks: (r[col('remarks')] ?? '').trim() }))
      .filter(r => r.item_code).map(r => ({ ...r, item_name: nameOf(r.item_code) }));
    const unknown = added.filter(r => !r.item_name).map(r => r.item_code);
    if (unknown.length) return setProblem(`Item not found: ${unknown.slice(0, 5).join(', ')}${unknown.length > 5 ? '…' : ''}. Nothing was imported.`);
    setProblem('');
    setRows(rs => {
      const have = new Map((rs ?? []).map(r => [r.item_code.toLowerCase(), r]));
      for (const r of added) have.set(r.item_code.toLowerCase(), r);   // an imported row replaces the same item's row
      return [...have.values()];
    });
    setDirty(true);
    toastStore.add('ok', `${added.length} row${added.length === 1 ? '' : 's'} imported. Click Update to save.`);
  };

  const validate = () => {
    const seen = new Set();
    for (const [n, r] of rows.entries()) {
      if (!r.item_code.trim()) return `Row ${n + 1}: select the item.`;
      if (!nameOf(r.item_code)) return `Row ${n + 1}: '${r.item_code}' is not an item.`;
      if (seen.has(r.item_code.toLowerCase())) return `Row ${n + 1}: '${r.item_code}' is on the list twice.`;
      seen.add(r.item_code.toLowerCase());
      if (r.rate === '' || Number(r.rate) < 0) return `Row ${n + 1}: enter the rate.`;
      if (r.eff_from && r.eff_to && r.eff_to < r.eff_from) return `Row ${n + 1}: the To date is before the From date.`;
    }
    return '';
  };

  const save = async () => {
    const why = validate();
    if (why) return setProblem(why);
    setProblem('');
    if (!(await toastStore.defer('ok', `The item rates of ${code} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    try {
      const r = await savePartyRecords(party, code, 'item-rates', rows.map(({ item_name, ...x }) => x));   // eslint-disable-line no-unused-vars
      setRows(r.rows.map(x => ({ ...blank(), ...x, item_name: nameOf(x.item_code) })));
      setDirty(false);
      toastStore.add('ok', 'Updated successfully.');
    } catch (e) { setProblem(errorText(e)); }
  };

  const cancel = async () => {
    if (dirty && !(await confirm('Throw away the changes?'))) return;
    setRows(null); setDirty(false); setProblem('');
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {dialog}
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>{w.title}</h1><p className={pageSub}>The items this {w.party.toLowerCase()} has its own rate for.</p></div>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} flex flex-wrap items-end gap-3 px-4 py-3`}>
        <div className="min-w-64">
          <label htmlFor="pv-party" className={label}>{w.party}</label>
          <select id="pv-party" value={code} onChange={e => { setCode(e.target.value); setRows(null); setDirty(false); }} disabled={dirty} className={`${field} w-full`}>
            <option value="">Select…</option>
            {parties.map(p => <option key={p.code} value={p.code}>{p.name} ({p.code})</option>)}
          </select>
        </div>
        <button onClick={() => void load()} disabled={dirty} className={secondaryButton}><Download size={14} />Load</button>
        {rows && canEdit && (
          <>
            <input ref={picker} type="file" accept=".csv,text/csv" hidden onChange={e => void importCsv(e)} />
            <button onClick={() => picker.current?.click()} className={secondaryButton}><Upload size={14} />Import from XL</button>
            <button onClick={add} className={secondaryButton}><Plus size={14} />Add item</button>
            <button onClick={() => void cancel()} className={`${secondaryButton} ml-auto`}><X size={14} />Cancel</button>
            <button onClick={() => void save()} className={primaryButton}><Save size={14} />Update{dirty ? ' *' : ''}</button>
          </>
        )}
      </div>

      {rows && (
        <div className={`${panel} max-h-[60vh] overflow-auto`}>
          <datalist id="pv-items">{items.slice(0, 3000).map(i => <option key={i.code} value={i.code}>{i.name}</option>)}</datalist>
          <table className="w-full text-sm">
            <thead className={`${tableHead} sticky top-0`}>
              <tr><th className="w-10 px-2 py-2">#</th><th className="px-2 py-2 text-left">Item Code</th><th className="px-2 py-2 text-left">Item Name</th>
                <th className="px-2 py-2 text-right">Rate</th><th className="px-2 py-2 text-left">From</th><th className="px-2 py-2 text-left">To</th><th className="px-2 py-2 text-left">Remarks</th><th className="w-10" /></tr>
            </thead>
            <tbody className={tableBody}>
              {rows.map((r, i) => (
                <tr key={i}>
                  <td className="px-2 py-1 text-center text-stone-500">{i + 1}</td>
                  <td className="px-2 py-1"><input list="pv-items" value={r.item_code} onChange={e => setItem(i, e.target.value)} disabled={!canEdit} aria-label={`Item code row ${i + 1}`} className={`${field} w-40 py-1`} /></td>
                  <td className="px-2 py-1 text-stone-700 dark:text-stone-200">{r.item_name}</td>
                  <td className="px-2 py-1 text-right"><input type="number" min={0} step="0.01" value={r.rate} onChange={e => edit(i, { rate: e.target.value })} disabled={!canEdit} aria-label={`Rate row ${i + 1}`} className={`${field} w-28 py-1 text-right tabular-nums`} placeholder={money(0)} /></td>
                  <td className="px-2 py-1"><input type="date" value={r.eff_from} onChange={e => edit(i, { eff_from: e.target.value })} disabled={!canEdit} aria-label={`From row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1"><input type="date" value={r.eff_to} onChange={e => edit(i, { eff_to: e.target.value })} disabled={!canEdit} aria-label={`To row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1"><input value={r.remarks} maxLength={250} onChange={e => edit(i, { remarks: e.target.value })} disabled={!canEdit} aria-label={`Remarks row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1">{canEdit && <button onClick={() => remove(i)} className="text-stone-400 hover:text-red-600" aria-label={`Remove row ${i + 1}`}><Trash2 size={14} /></button>}</td>
                </tr>
              ))}
              {!rows.length && <tr><td colSpan={8} className="px-3 py-8 text-center text-stone-500">No items yet. Add an item or import a sheet.</td></tr>}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export const CustomerVsItem = () => <PartyVsItem party="customers" />;
export const SupplierVsItem = () => <PartyVsItem party="suppliers" />;
export default PartyVsItem;
