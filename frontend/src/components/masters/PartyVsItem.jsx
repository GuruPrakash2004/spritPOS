// Customer Vs Item Master / Supplier Vs Item Master (the desktop's frmPartyItem, under Customer Related / Supplier Related): the items a party has its
// own rate for. Pick the party, Load, then add items (type a code or name: the list shows each item with its units, like the exe's pick list), set
// each row's Rate (and, as extras this app keeps, From / To dates and Remarks), then Save. Save replaces the party's rows in MstCUSTITEM / MstSUPPITEM,
// as the desktop does, after the exe's checks: the list is not empty, no item + unit twice, an active item, a rate above zero.
// Keys: Alt+A add row, Alt+R remove the current row, Alt+E export to Excel, Alt+S or Ctrl+S save, in the item box ↑ ↓ Enter pick, Esc close the list.
// "Import from XL" is the exe's Import Item Details window (columns ItemCode, Rate; Append or Delete previous data; red rows are errors).
//
//   <PartyVsItem party="customers" />   <PartyVsItem party="suppliers" />

import { useEffect, useMemo, useRef, useState } from 'react';
import { Download, FileDown, Plus, Save, Trash2, Upload, X } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import Modal from '../common/Modal';
import Kbd from '../common/Kbd';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { canDo, errorText, getCustomers, getItems, getPartyItemCounts, getPartyRecords, getSuppliers, savePartyRecords, searchPartyItems } from '../../lib/api';
import { useShortcuts } from '../../lib/keys';
import { readTableFile } from '../../lib/readTableFile';
import { downloadXls, downloadXlsRows } from '../../lib/xlsExport';
import { field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const WORDS = {
  customers: { title: 'Customer Vs Item Master', party: 'Customer', master: 'Customer Master', load: getCustomers },
  suppliers: { title: 'Supplier Vs Item Master', party: 'Supplier', master: 'Supplier Master', load: getSuppliers },
};
const blank = () => ({ item_code: '', item_name: '', uom: '', rate: '', eff_from: '', eff_to: '', remarks: '' });
const isEmpty = (r) => !r.item_code.trim();

// The exe's Import Item Details window: file with ItemCode, Rate; every row is checked (code empty / does not exist / listed twice) and errors show in red.
function ImportDialog({ items, existing, onClose, onTransfer }) {
  const [rows, setRows] = useState(null);
  const [mode, setMode] = useState('append');
  const [problem, setProblem] = useState('');
  const picker = useRef(null);
  const { confirm, dialog } = useConfirm();
  const byCode = useMemo(() => new Map(items.map(i => [i.code.toUpperCase(), i])), [items]);

  const checked = (list) => {
    const seen = new Set();
    return list.map(r => {
      const code = r.item_code.trim().toUpperCase().replace(/'/g, '`');
      const it = byCode.get(code);
      let err = '';
      if (!code) err = 'Item Code is Empty';
      else if (!it) err = 'Item Code Does not Exst';
      else if (seen.has(code)) err = 'Item Code already exists';
      seen.add(code);
      return { ...r, item_code: code, item_name: it?.name ?? '', uom: it?.uom ?? '', err };
    });
  };

  const load = async (e) => {
    const file = e.target.files?.[0];
    e.target.value = '';
    if (!file) return;
    setProblem('');
    try {
      const table = await readTableFile(file);
      const head = (table[0] ?? []).slice(0, 2).map(h => h.trim().toUpperCase()).join(',');
      if (head !== 'ITEMCODE,RATE') { setRows(null); return setProblem('Please select the valid template'); }
      const col = (n) => (table[0] ?? []).findIndex(h => h.trim().toLowerCase() === n);
      const list = table.slice(1).filter(r => `${r[0] ?? ''}${r[1] ?? ''}`.trim() !== '').map(r => ({
        ...blank(), item_code: r[0] ?? '', rate: String(r[1] ?? '').trim(), eff_from: (r[col('from')] ?? '').trim(), eff_to: (r[col('to')] ?? '').trim(), remarks: (r[col('remarks')] ?? '').trim() }));
      if (!list.length) { setRows(null); return setProblem('There is no data exists for importing'); }
      setRows(checked(list));
    } catch (err) { setProblem(errorText(err)); }
  };

  const errors = (rows ?? []).filter(r => r.err);
  const removeErrors = async () => {
    if (!(await confirm('Do you want to Remove ALL Erroneous Records ?'))) return;
    const bad = rows.filter(r => r.err);
    setRows(checked(rows.filter(r => !r.err)));
    if (bad.length && (await confirm('Do you want to Export ALL Erroneous Records to XL?'))) {
      downloadXls('Errorneous_Records', 'Errorneous Records', '', [{ key: 'item_code', label: 'ItemCode' }, { key: 'rate', label: 'Rate' }, { key: 'err', label: 'Error Record' }], bad);
    }
  };
  const transfer = () => {
    if (!rows?.length) return setProblem('No records found in the selected excel file!');
    if (errors.length) return setProblem('Please Remove Errorneous Data!!');
    onTransfer(rows.map(({ err, ...r }) => r), mode === 'append');   // eslint-disable-line no-unused-vars
    toastStore.add('ok', ' Item Details Imported Successfully!');
    onClose();
  };
  const template = () => downloadXlsRows('Template', [['ItemCode', 'Rate']]);
  void existing;

  return (
    <Modal title="Import Item Details" onClose={onClose} wide footer={(
      <>
        <button type="button" onClick={template} className={secondaryButton}><FileDown size={14} />Template</button>
        <button type="button" onClick={() => picker.current?.click()} className={secondaryButton}><Upload size={14} />Load from Excel</button>
        <button type="button" onClick={() => void removeErrors()} disabled={!errors.length} className={secondaryButton}>Remove erroneous records</button>
        <button type="button" onClick={onClose} className={secondaryButton}>Close</button>
        <button type="button" onClick={transfer} disabled={!rows?.length} className={primaryButton}>Transfer to Main Page</button>
      </>
    )}>
      {dialog}
      <input ref={picker} type="file" accept=".xls,.xlsx,.csv,.txt" hidden onChange={e => void load(e)} />
      <p className="mb-2 text-xs text-stone-500">Please select a excel file containing Item Details (columns ItemCode, Rate). Importing of data is not allowed until all the data is correct.</p>
      <fieldset className="mb-3 flex items-center gap-4 text-xs">
        <label className="flex items-center gap-1.5"><input type="radio" name="pvi-mode" checked={mode === 'append'} onChange={() => setMode('append')} />Append Previous Data</label>
        <label className="flex items-center gap-1.5"><input type="radio" name="pvi-mode" checked={mode === 'delete'} onChange={() => setMode('delete')} />Delete Previous Data</label>
        {rows && <span className="ml-auto text-stone-500">Total Number of Records Loaded : {rows.length}</span>}
      </fieldset>
      {problem && <InlineError message={problem} />}
      {rows && (
        <div className="max-h-[45vh] overflow-auto rounded border border-line">
          <table className="w-full text-xs">
            <thead className={`${tableHead} sticky top-0`}><tr><th className="px-2 py-1.5">Sl No</th><th className="px-2 py-1.5 text-left">Item Code</th><th className="px-2 py-1.5 text-left">Item Name</th><th className="px-2 py-1.5 text-left">UOM</th><th className="px-2 py-1.5 text-right">Rate</th><th className="px-2 py-1.5 text-left">Error Record</th></tr></thead>
            <tbody>
              {rows.map((r, i) => (
                <tr key={i} className={r.err ? 'text-red-600' : ''}>
                  <td className="px-2 py-1 text-center">{i + 1}</td><td className="px-2 py-1">{r.item_code}</td><td className="px-2 py-1">{r.item_name}</td><td className="px-2 py-1">{r.uom}</td><td className="px-2 py-1 text-right tabular-nums">{r.rate}</td><td className="px-2 py-1">{r.err}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Modal>
  );
}

function PartyVsItem({ party }) {
  const w = WORDS[party];
  const [parties, setParties] = useState([]);
  const [counts, setCounts] = useState({});
  const [items, setItems] = useState([]);
  const [code, setCode] = useState('');
  const [rows, setRows] = useState(null);       // null until Load
  const [dirty, setDirty] = useState(false);
  const [problem, setProblem] = useState('');
  const [cur, setCur] = useState(0);            // the row the cursor is in
  const [pick, setPick] = useState(null);       // { row, q, list, idx }: the item pick list under the grid
  const [importing, setImporting] = useState(false);
  const { confirm, dialog } = useConfirm();
  const canEdit = canDo(`${w.title}`, 'edit') || canDo(w.master, 'edit');

  useEffect(() => {
    w.load().then(l => setParties(l.filter(p => p.code !== 'CASH'))).catch(e => setProblem(errorText(e)));
    getItems().then(setItems).catch(() => undefined);
    getPartyItemCounts(party).then(setCounts).catch(() => undefined);
  }, [w, party]);

  const byCode = useMemo(() => new Map(items.map(i => [i.code.toLowerCase(), i])), [items]);
  const nameOf = (c) => byCode.get(String(c).trim().toLowerCase())?.name ?? '';

  // the pick list: ask the server, as the exe's list does, a moment after typing stops
  useEffect(() => {
    if (!pick?.q?.trim()) return undefined;
    const t = setTimeout(() => {
      searchPartyItems(party, pick.q).then(list => setPick(p => (p && p.q === pick.q ? { ...p, list, idx: 0 } : p))).catch(() => undefined);
    }, 200);
    return () => clearTimeout(t);
  }, [pick?.q, party]);

  const load = async () => {
    if (!code) return setProblem(`Please Select ${w.party}`);
    setProblem('');
    try {
      const r = await getPartyRecords(party, code, 'item-rates');
      setRows(r.rows.map(x => ({ ...blank(), ...x, item_name: x.item_name || nameOf(x.item_code) })));
      setDirty(false); setPick(null); setCur(0);
    } catch (e) { setProblem(errorText(e)); }
  };

  const edit = (i, patch) => { setRows(rs => rs.map((r, j) => (j === i ? { ...r, ...patch } : r))); setDirty(true); };
  const typeItem = (i, text) => {
    edit(i, { item_code: text, item_name: '', uom: '' });
    setPick({ row: i, q: text, list: [], idx: 0 });
  };
  const choose = (i, it) => {
    edit(i, { item_code: it.item_code, item_name: it.item_name, uom: it.uom });
    setPick(null);
    setTimeout(() => document.getElementById(`pv-rate-${i}`)?.focus(), 0);
  };
  const itemKey = (i, e) => {
    if (pick?.row === i && pick.list.length) {
      if (e.key === 'ArrowDown') { e.preventDefault(); setPick(p => ({ ...p, idx: Math.min(p.idx + 1, p.list.length - 1) })); return; }
      if (e.key === 'ArrowUp') { e.preventDefault(); setPick(p => ({ ...p, idx: Math.max(p.idx - 1, 0) })); return; }
      if (e.key === 'Enter') { e.preventDefault(); choose(i, pick.list[pick.idx]); return; }
    }
    if (e.key === 'Escape') { setPick(null); return; }
    if (e.key === 'Enter') {   // no list: the typed code must be an item (the exe: "code ... does not exist!")
      e.preventDefault();
      const it = byCode.get(e.currentTarget.value.trim().toLowerCase());
      if (!it) return setProblem(`Item code '${e.currentTarget.value.trim()}' does not exist!`);
      setProblem(''); choose(i, { item_code: it.code, item_name: it.name, uom: it.uom });
    }
  };
  const add = () => { setRows(rs => [...(rs ?? []), blank()]); setDirty(true); setTimeout(() => document.getElementById(`pv-item-${(rows?.length ?? 0)}`)?.focus(), 0); };
  const removeCurrent = async () => {
    const r = rows?.[cur];
    if (!r) return;
    if (!r.item_name && !r.item_code) return setProblem('Item Code Empty....');
    if (!(await confirm('Do you want to delete the Item Code ...?'))) return;
    setRows(rs => rs.filter((_, j) => j !== cur)); setDirty(true);
    toastStore.add('ok', 'Click on save to update new item list');
  };

  const exportXl = () => {
    if (!rows?.some(r => r.item_code.trim())) return;
    downloadXls(`${w.title.replace(/\s+/g, '_')}_${code}`, w.title, code, [{ key: 'item_code', label: 'Item Code' }, { key: 'item_name', label: 'Item Name' }, { key: 'uom', label: 'UOM' },
      { key: 'rate', label: 'Rate' }, { key: 'eff_from', label: 'From' }, { key: 'eff_to', label: 'To' }, { key: 'remarks', label: 'Remarks' }], rows);
  };

  const validate = (list) => {
    if (!code) return `${w.party} Name should not be blank!!`;
    if (!list.length) return 'Details section should not be Empty....';
    const seen = new Set();
    for (const [n, r] of list.entries()) {
      if (!nameOf(r.item_code)) return `Row ${n + 1}: item code '${r.item_code}' does not exist!`;
      const k = `${r.item_code.toLowerCase()}|${r.uom.toLowerCase()}`;
      if (seen.has(k)) return `Duplicate Data present (in row ${n + 1}) for combination of columns Item and UOM`;
      seen.add(k);
      if (!(Number(r.rate) > 0)) return `Row ${n + 1}: Rate Empty....`;
      if (r.eff_from && r.eff_to && r.eff_to < r.eff_from) return `Row ${n + 1}: the To date is before the From date.`;
    }
    return '';
  };

  const save = async () => {
    if (!rows || !canEdit) return;
    const list = rows.filter(r => !isEmpty(r));   // the exe drops the empty rows first
    const why = validate(list);
    if (why) return setProblem(why);
    setProblem('');
    if (!(await toastStore.defer('ok', `The item rates of ${code} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    try {
      const r = await savePartyRecords(party, code, 'item-rates', list.map(({ item_name, ...x }) => x));   // eslint-disable-line no-unused-vars
      setRows(r.rows.map(x => ({ ...blank(), ...x, item_name: x.item_name || nameOf(x.item_code) })));
      setDirty(false);
      toastStore.add('ok', 'Saved successfully.');
      getPartyItemCounts(party).then(setCounts).catch(() => undefined);
    } catch (e) { setProblem(errorText(e)); }
  };

  const cancel = async () => {
    if (dirty && !(await confirm('Throw away the changes?'))) return;
    setRows(null); setDirty(false); setProblem(''); setPick(null);
  };

  const transfer = (list, append) => {
    setRows(rs => {
      const keep = append ? (rs ?? []).filter(r => !isEmpty(r)) : [];
      const have = new Map(keep.map(r => [r.item_code.toLowerCase(), r]));
      for (const r of list) have.set(r.item_code.toLowerCase(), { ...blank(), ...r });   // an imported row replaces the same item's row
      return [...have.values()];
    });
    setDirty(true);
  };

  useShortcuts({ 'Alt+A': add, 'Alt+R': () => { void removeCurrent(); }, 'Alt+E': exportXl, 'Alt+S': () => { void save(); }, 'Mod+S': () => { void save(); } }, !!rows && canEdit);

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {dialog}
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>{w.title}</h1><p className={pageSub}>The items this {w.party.toLowerCase()} has its own rate for.</p></div>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} flex flex-wrap items-end gap-3 px-4 py-3`}>
        <div className="min-w-64">
          <label htmlFor="pv-party" className={label}>Select {w.party}</label>
          <select id="pv-party" value={code} onChange={e => { setCode(e.target.value); setRows(null); setDirty(false); setProblem(''); }} disabled={dirty} className={`${field} w-full`}>
            <option value="">Select…</option>
            {parties.map(p => <option key={p.code} value={p.code}>{p.name} ({p.code}){counts[p.code] ? ` · ${counts[p.code]} items` : ''}</option>)}
          </select>
        </div>
        <button onClick={() => void load()} disabled={dirty} className={secondaryButton}><Download size={14} />Load</button>
        {rows && canEdit && (
          <>
            <button onClick={() => setImporting(true)} className={secondaryButton}><Upload size={14} />Import from XL</button>
            <button onClick={add} className={secondaryButton}><Plus size={14} />Add item<Kbd combo="Alt+A" /></button>
            <button onClick={exportXl} className={secondaryButton}><FileDown size={14} />Export<Kbd combo="Alt+E" /></button>
            <button onClick={() => void cancel()} className={`${secondaryButton} ml-auto`}><X size={14} />Cancel</button>
            <button onClick={() => void save()} className={primaryButton}><Save size={14} />Save{dirty ? ' *' : ''}<Kbd combo="Alt+S" /></button>
          </>
        )}
      </div>

      {rows && (
        <div className={`${panel} max-h-[55vh] overflow-auto`}>
          <table className="w-full text-sm">
            <thead className={`${tableHead} sticky top-0`}>
              <tr><th className="w-10 px-2 py-2">Sl No</th><th className="px-2 py-2 text-left">Item Code</th><th className="px-2 py-2 text-left">Item Name</th><th className="px-2 py-2 text-left">UOM</th>
                <th className="px-2 py-2 text-right">Rate</th><th className="px-2 py-2 text-left">From</th><th className="px-2 py-2 text-left">To</th><th className="px-2 py-2 text-left">Remarks</th><th className="w-10" /></tr>
            </thead>
            <tbody className={tableBody}>
              {rows.map((r, i) => (
                <tr key={i} onFocus={() => setCur(i)} aria-selected={cur === i} className={cur === i ? 'bg-subtle/60' : ''}>
                  <td className="px-2 py-1 text-center text-stone-500">{i + 1}</td>
                  <td className="px-2 py-1"><input id={`pv-item-${i}`} value={r.item_code} onChange={e => typeItem(i, e.target.value)} onKeyDown={e => itemKey(i, e)} disabled={!canEdit} autoComplete="off"
                    aria-label={`Item code row ${i + 1}`} className={`${field} w-40 py-1`} /></td>
                  <td className="px-2 py-1 text-stone-700 dark:text-stone-200">{r.item_name}</td>
                  <td className="px-2 py-1 text-stone-700 dark:text-stone-200">{r.uom}</td>
                  <td className="px-2 py-1 text-right"><input id={`pv-rate-${i}`} type="number" min={0} step="0.01" value={r.rate} onChange={e => edit(i, { rate: e.target.value })} disabled={!canEdit} aria-label={`Rate row ${i + 1}`} className={`${field} w-28 py-1 text-right tabular-nums`} placeholder={money(0)} /></td>
                  <td className="px-2 py-1"><input type="date" value={r.eff_from} onChange={e => edit(i, { eff_from: e.target.value })} disabled={!canEdit} aria-label={`From row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1"><input type="date" value={r.eff_to} onChange={e => edit(i, { eff_to: e.target.value })} disabled={!canEdit} aria-label={`To row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1"><input value={r.remarks} maxLength={250} onChange={e => edit(i, { remarks: e.target.value })} disabled={!canEdit} aria-label={`Remarks row ${i + 1}`} className={`${field} py-1`} /></td>
                  <td className="px-2 py-1">{canEdit && <button onClick={() => { setCur(i); setRows(rs => rs.filter((_, j) => j !== i)); setDirty(true); }} className="text-stone-400 hover:text-red-600" aria-label={`Remove row ${i + 1}`}><Trash2 size={14} /></button>}</td>
                </tr>
              ))}
              {!rows.length && <tr><td colSpan={9} className="px-3 py-8 text-center text-stone-500">No items yet. Add an item or import a sheet.</td></tr>}
            </tbody>
          </table>
        </div>
      )}

      {pick && pick.list.length > 0 && (
        <div className={`${panel} max-h-52 overflow-auto`} role="listbox" aria-label="Select item">
          <table className="w-full text-xs">
            <thead className={`${tableHead} sticky top-0`}><tr><th className="px-3 py-1.5 text-left">Item Code</th><th className="px-3 py-1.5 text-left">UOM</th><th className="px-3 py-1.5 text-left">Item Name</th></tr></thead>
            <tbody>
              {pick.list.map((it, n) => (
                <tr key={`${it.item_code}|${it.uom}`} role="option" aria-selected={n === pick.idx} onMouseDown={e => { e.preventDefault(); choose(pick.row, it); }}
                  className={`cursor-pointer ${n === pick.idx ? 'bg-accent-soft' : 'hover:bg-subtle'}`}>
                  <td className="px-3 py-1">{it.item_code}</td><td className="px-3 py-1">{it.uom}</td><td className="px-3 py-1">{it.item_name}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
      {importing && <ImportDialog items={items} existing={rows} onClose={() => setImporting(false)} onTransfer={transfer} />}
    </div>
  );
}

export const CustomerVsItem = () => <PartyVsItem party="customers" />;
export const SupplierVsItem = () => <PartyVsItem party="suppliers" />;
export default PartyVsItem;
