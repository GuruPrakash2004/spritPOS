import { useEffect, useMemo, useRef, useState } from 'react';
import {
  ArrowDownCircle, ArrowUpCircle, ChevronDown, ChevronsDown, ChevronsUp, Copy, FileInput,
  Info, Paperclip, PlusCircle, RefreshCw, Save, Search, Settings2, UserPlus, XCircle,
} from 'lucide-react';
import Toast from '../Toast';
import { api, errorText, type ApiDoc, type ApiIndent, type ApiSupplier } from '../api';
import { SAMPLE_STOCK_DATA } from '../stockData';
import { numberToWords } from '../formatters';
import {
  amt, blankRow, cell, cloneRow, field, FY_PREFIX, headCell, iconButton, infoBar, INITIAL_SUPPLIERS, isFilled,
  ITEM_CODES, money, panel, PB_SERIES, pctOf, PURCHASE_TYPES, stockRow, today, type Row, type Supplier,
} from './purchaseBillData';
import { CopyFromModal, ItemLookupModal } from './PurchaseBillModals';

export default function PurchaseBill() {
  const [pbSeries, setPbSeries] = useState('');
  const [pbNo, setPbNo] = useState(201);
  const [pbDate, setPbDate] = useState(today);
  const [suppliers, setSuppliers] = useState<Supplier[]>(INITIAL_SUPPLIERS);
  const [supplier, setSupplier] = useState(INITIAL_SUPPLIERS[0].name);
  const [billingAddress, setBillingAddress] = useState(INITIAL_SUPPLIERS[0].address);
  const [editingAddress, setEditingAddress] = useState(false);
  const [invNo, setInvNo] = useState('RD/2026/4471');
  const [invDate, setInvDate] = useState('2026-09-21');
  const [invValue, setInvValue] = useState('25920.00');
  const [purchaseType, setPurchaseType] = useState(PURCHASE_TYPES[0]);
  const [headerOpen, setHeaderOpen] = useState(true);
  const [rows, setRows] = useState<Row[]>(() => [stockRow(0, 24), stockRow(3, 48), blankRow()]);
  const [selectedUid, setSelectedUid] = useState<number>(() => rows[rows.length - 1].uid);
  const [showTaxColumns, setShowTaxColumns] = useState(true);
  const [addMenuOpen, setAddMenuOpen] = useState(false);
  const [lookupOpen, setLookupOpen] = useState(false);
  const [indents, setIndents] = useState<ApiIndent[] | null>(null);
  const [copyOpen, setCopyOpen] = useState(false);
  const [lookupFilter, setLookupFilter] = useState('');
  const [attachments, setAttachments] = useState<string[]>([]);
  const [tcsPct, setTcsPct] = useState('1');
  const [remarks, setRemarks] = useState('');
  const [notesOpen, setNotesOpen] = useState(true);
  const [totalsOpen, setTotalsOpen] = useState(true);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const fileInput = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  const pbDisplay = `${FY_PREFIX}/${pbSeries}/${String(pbNo).padStart(5, '0')}`;
  const selectedIndex = rows.findIndex(r => r.uid === selectedUid);
  const selectedRow = rows[selectedIndex];

  const totals = useMemo(() => {
    const filled = rows.filter(isFilled);
    const basic = filled.reduce((acc, r) => acc + amt(r), 0);
    const tcs = (basic * (Number(tcsPct) || 0)) / 100;
    const cgst = filled.reduce((acc, r) => acc + pctOf(r, r.cgstPct), 0);
    const sgst = filled.reduce((acc, r) => acc + pctOf(r, r.sgstPct), 0);
    const cess = filled.reduce((acc, r) => acc + pctOf(r, r.cessPct), 0);
    const compCess = filled.reduce((acc, r) => acc + r.compCess, 0);
    const beforeRound = basic + tcs + cgst + sgst + cess + compCess;
    const roundOff = Math.round(beforeRound) - beforeRound;
    const net = beforeRound + roundOff;
    const qty = filled.reduce((acc, r) => acc + r.qty, 0);
    return { basic, tcs, cgst, sgst, cess, compCess, roundOff, net, qty, items: filled.length };
  }, [rows, tcsPct]);

  const balance = (Number(invValue) || 0) - totals.net;
  const balanced = invValue !== '' && Math.abs(balance) < 0.005;

  const selectSupplier = (name: string) => {
    setSupplier(name);
    const match = suppliers.find(s => s.name.toLowerCase() === name.trim().toLowerCase());
    if (match) { setBillingAddress(match.address); setEditingAddress(false); }
  };

  // Suppliers come from Supplier Master; the sample addresses stay as a fallback when the API is down.
  useEffect(() => {
    api<ApiSupplier[]>('/masters/suppliers')
      .then(list => setSuppliers(list.filter(x => x.active).map(x => ({
        name: x.name, address: INITIAL_SUPPLIERS.find(i => i.name === x.name)?.address ?? x.city,
      }))))
      .catch(() => undefined);
  }, []);

  const addSupplier = () => {
    const name = window.prompt('New supplier name')?.trim().toUpperCase();
    if (!name) return;
    const address = window.prompt(`Billing address for ${name}`)?.trim() ?? '';
    const code = name.replace(/[^A-Z0-9]+/g, '-').slice(0, 20);
    api('/masters/suppliers', { method: 'POST', body: { code, name, city: address } })
      .then(() => {
        setSuppliers([...suppliers, { name, address }]);
        setSupplier(name);
        setBillingAddress(address);
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const updateRow = (uid: number, patch: Partial<Row>) => setRows(rows.map(r => (r.uid === uid ? { ...r, ...patch } : r)));

  // Typing an item code or name that matches stock fills in the rest of the row.
  const setItem = (uid: number, key: 'code' | 'name', value: string) => {
    const term = value.trim().toLowerCase();
    const stock = SAMPLE_STOCK_DATA.findIndex((item, i) => (key === 'code' ? ITEM_CODES[i].toLowerCase() === term : `${item.name} ${item.pkg}`.toLowerCase() === term));
    if (stock === -1) return updateRow(uid, { [key]: value, stock: null });
    const filled = stockRow(stock, 0);
    const current = rows.find(r => r.uid === uid);
    updateRow(uid, { stock, code: filled.code, name: filled.name, qoh: filled.qoh, rate: filled.rate, qty: current?.qty || 1 });
  };

  const numberCell = (row: Row, key: 'qty' | 'rate' | 'cgstPct' | 'sgstPct' | 'cessPct' | 'compCess', label: string, extra = '') => (
    <input
      type="number" min={0} step={key === 'qty' ? 1 : 0.01}
      value={row[key] || ''} placeholder="0"
      onChange={e => updateRow(row.uid, { [key]: Math.max(0, key === 'qty' ? Math.floor(Number(e.target.value)) || 0 : Number(e.target.value) || 0) })}
      className={`${cell} text-right ${extra}`} aria-label={`${label} row ${rows.indexOf(row) + 1}`}
    />
  );

  const addRows = (count: number) => {
    const added = Array.from({ length: count }, blankRow);
    setRows([...rows, ...added]);
    setSelectedUid(added[0].uid);
    setAddMenuOpen(false);
  };

  const deleteRow = () => {
    if (!selectedRow) return;
    const next = rows.filter(r => r.uid !== selectedUid);
    const safe = next.length ? next : [blankRow()];
    setRows(safe);
    setSelectedUid(safe[Math.min(selectedIndex, safe.length - 1)].uid);
  };

  const moveRow = (direction: -1 | 1) => {
    const target = selectedIndex + direction;
    if (selectedIndex === -1 || target < 0 || target >= rows.length) return;
    const next = [...rows];
    [next[selectedIndex], next[target]] = [next[target], next[selectedIndex]];
    setRows(next);
  };

  const copyRow = () => {
    if (!selectedRow) return;
    const copy = cloneRow(selectedRow);
    const next = [...rows];
    next.splice(selectedIndex + 1, 0, copy);
    setRows(next);
    setSelectedUid(copy.uid);
  };

  const refreshQoh = () => {
    setRows(rows.map(r => (r.stock === null ? r : { ...r, qoh: SAMPLE_STOCK_DATA[r.stock].clQty })));
    setToast({ kind: 'ok', text: 'QOH refreshed from current stock.' });
  };

  // Copy From: list saved Purchase Indents and load one's lines into this bill.
  const openCopyFrom = () => {
    setIndents(null);
    api<ApiIndent[]>('/transactions/purchase-indent')
      .then(setIndents)
      .catch(e => { setToast({ kind: 'error', text: errorText(e) }); setIndents([]); });
  };

  const copyFromIndent = (indent: ApiIndent) => {
    const kept = rows.filter(isFilled);
    const added = indent.lines
      .map(l => ({ stock: SAMPLE_STOCK_DATA.findIndex(i => i.code === l.item_code), qty: l.qty, rate: l.rate }))
      .filter(l => l.stock !== -1)
      .map(l => ({ ...stockRow(l.stock, l.qty), rate: l.rate }));
    if (!added.length) return setToast({ kind: 'error', text: `Indent ${indent.doc_no} has no items to copy.` });
    setRows([...kept, ...added]);
    setSelectedUid(added[0].uid);
    setCopyOpen(false);
    setToast({ kind: 'ok', text: `Copied ${added.length} items from indent ${indent.doc_no}.` });
  };

  const pickFromLookup = (stock: number) => {
    const row = stockRow(stock, 1);
    if (selectedRow && !isFilled(selectedRow)) {
      updateRow(selectedUid, { stock, code: row.code, name: row.name, qoh: row.qoh, rate: row.rate, qty: 1 });
    } else {
      setRows([...rows, row]);
      setSelectedUid(row.uid);
    }
    setLookupOpen(false);
    setLookupFilter('');
  };

  const resetBill = () => {
    const fresh = blankRow();
    setSupplier('');
    setBillingAddress('');
    setEditingAddress(false);
    setInvNo('');
    setInvDate(today());
    setInvValue('');
    setPurchaseType(PURCHASE_TYPES[0]);
    setRows([fresh]);
    setSelectedUid(fresh.uid);
    setAttachments([]);
    setRemarks('');
  };

  const saveBill = () => {
    const filled = rows.filter(isFilled);
    if (!supplier.trim()) return setToast({ kind: 'error', text: 'Select a supplier.' });
    if (!invNo.trim() || !invDate) return setToast({ kind: 'error', text: 'Enter the supplier Inv No and Inv Date.' });
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one item.' });
    if (filled.some(r => !r.qty || !r.rate)) return setToast({ kind: 'error', text: 'Every item needs a PB Qty and PB Rate.' });
    if (!balanced) return setToast({ kind: 'error', text: `PB value differs from Inv Value by ₹ ${money(Math.abs(balance))}.` });
    if (filled.some(r => r.stock === null)) return setToast({ kind: 'error', text: 'Pick every item with Add Item so it has an item code.' });
    api<ApiDoc>('/transactions/purchase-bill', {
      method: 'POST',
      body: {
        date: pbDate, supplier, supplier_inv_no: invNo, supplier_inv_date: invDate, supplier_inv_value: Number(invValue) || 0,
        purchase_type: purchaseType, remarks, tcs: totals.tcs, cess: totals.cess + totals.compCess,
        lines: filled.map(r => ({ item_code: r.code, bottles: r.qty, rate: r.rate })),
      },
    }).then(doc => {
      setToast({ kind: 'ok', text: `Purchase Bill ${doc.doc_no} saved — ${doc.totals.qty} bottles added to stock.` });
      setPbNo(pbNo + 1);
      resetBill();
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const cancelBill = () => {
    if (window.confirm('Cancel this purchase bill? Unsaved entries will be cleared.')) resetBill();
  };

  const totalRows: [string, number][] = [
    ['Total Basic Amount', totals.basic], ['TCS', totals.tcs], ['Total CGST Amount', totals.cgst], ['Total SGST Amount', totals.sgst],
    ['Total Cess Amount', totals.cess], ['Total Comp.Cess', totals.compCess], ['Round Off', totals.roundOff],
  ];

  return (
    <div className="mx-auto flex max-w-[1600px] flex-col gap-3">
      {toast && <Toast kind={toast.kind} text={toast.text} />}

      <div className="flex items-center justify-between rounded-xl bg-stone-800 px-4 py-2.5 text-white shadow-sm dark:bg-[#20253a]">
        <h2 className="text-lg font-bold">New - Purchase Bill</h2>
        <button onClick={() => setHeaderOpen(!headerOpen)} className="rounded-lg p-1 hover:bg-white/10" aria-label={headerOpen ? 'Collapse header' : 'Expand header'}>
          {headerOpen ? <ChevronsUp size={18} /> : <ChevronsDown size={18} />}
        </button>
      </div>

      {headerOpen && (
        <div className="grid grid-cols-1 gap-3 xl:grid-cols-[1fr_1fr]">
          {/* PB number, supplier, billing address, PB value */}
          <div className={`${panel} grid grid-cols-1 gap-4 p-4 md:grid-cols-[1fr_220px]`}>
            <div className="space-y-3 text-xs">
              <div className="flex flex-wrap items-center gap-2">
                <span className="w-24 font-bold text-[#b84716] dark:text-[#ffb07e]">PB No.</span>
                <select value={pbSeries} onChange={e => setPbSeries(e.target.value)} className={`${field} w-20`} aria-label="PB series">
                  {PB_SERIES.map(s => <option key={s} value={s}>{s || '—'}</option>)}
                </select>
                <input type="number" min={1} value={pbNo} onChange={e => setPbNo(Math.max(1, Math.floor(Number(e.target.value)) || 1))} className={`${field} w-20 text-center font-mono font-bold`} aria-label="PB number" />
                <label className="ml-2 font-bold text-[#b84716] dark:text-[#ffb07e]" htmlFor="pb-date">Date</label>
                <input id="pb-date" type="date" value={pbDate} onChange={e => setPbDate(e.target.value)} className={`${field} font-mono`} />
                <span className="ml-auto font-mono font-bold text-stone-600 dark:text-stone-300">{pbDisplay}</span>
              </div>
              <div className="flex items-center gap-2">
                <label htmlFor="pb-supplier" className="w-24 font-semibold text-red-700 dark:text-red-400">Supplier</label>
                <button onClick={addSupplier} className="rounded-lg border border-[#e1c9b6] bg-[#fffaf5] p-1.5 text-sky-700 hover:border-[#e8702a] dark:border-[#414a6e] dark:bg-[#20253a] dark:text-sky-400" title="Add new supplier" aria-label="Add new supplier"><UserPlus size={14} /></button>
                <input id="pb-supplier" list="pb-suppliers" value={supplier} onChange={e => selectSupplier(e.target.value)} placeholder="Select or type supplier" className={`${field} flex-1`} />
                <datalist id="pb-suppliers">{suppliers.map(s => <option key={s.name} value={s.name} />)}</datalist>
              </div>
              <div className="flex items-start gap-2">
                <span className="w-24 font-bold italic text-stone-700 dark:text-stone-200">Billing Address</span>
                <div className="flex-1">
                  {editingAddress
                    ? <textarea value={billingAddress} onChange={e => setBillingAddress(e.target.value)} rows={2} className={`${field} w-full`} aria-label="Billing address" />
                    : <p className="text-stone-600 dark:text-stone-400">{billingAddress || <span className="text-stone-400">—</span>}</p>}
                  <button onClick={() => setEditingAddress(!editingAddress)} className="mt-1 text-sky-700 italic underline dark:text-sky-400">{editingAddress ? 'Done' : 'Change'}</button>
                </div>
              </div>
            </div>
            <div className="flex flex-col overflow-hidden rounded-xl border border-[#e1c9b6] dark:border-[#414a6e]">
              <div className="bg-stone-600 px-2 py-1.5 text-center text-white">
                <div className="text-sm font-bold">PB Value</div>
                <div className="text-[10px]">[Balance to Enter]</div>
              </div>
              <div className={`flex flex-1 flex-col items-center justify-center p-3 ${balanced ? 'bg-emerald-500 text-white' : 'bg-amber-100 text-amber-800 dark:bg-amber-900/40 dark:text-amber-200'}`}>
                <span className="font-mono text-xl font-bold">₹ {money(totals.net)}</span>
                <span className="text-[11px] font-semibold">{balanced ? 'Matches Inv Value' : `Balance ₹ ${money(balance)}`}</span>
              </div>
            </div>
          </div>

          {/* Supplier invoice details */}
          <div className={`${panel} flex flex-col gap-3 p-4`}>
            <table className="w-full border border-[#e1c9b6] text-xs dark:border-[#414a6e]">
              <tbody className="divide-y divide-[#e1c9b6] dark:divide-[#414a6e]">
                <tr>
                  <th className="w-32 bg-[#f1e3d8] px-3 py-1.5 text-left font-medium dark:bg-[#20253a]">Inv No</th>
                  <td className="p-1"><input value={invNo} onChange={e => setInvNo(e.target.value)} className={`${field} w-full font-mono`} aria-label="Inv No" /></td>
                </tr>
                <tr>
                  <th className="bg-[#f1e3d8] px-3 py-1.5 text-left font-medium dark:bg-[#20253a]">Inv Date</th>
                  <td className="p-1"><input type="date" value={invDate} onChange={e => setInvDate(e.target.value)} className={`${field} w-full font-mono`} aria-label="Inv Date" /></td>
                </tr>
                <tr>
                  <th className="bg-[#f1e3d8] px-3 py-1.5 text-left font-medium dark:bg-[#20253a]">Inv Value</th>
                  <td className="p-1"><input type="number" min={0} step="0.01" value={invValue} onChange={e => setInvValue(e.target.value)} className={`${field} w-full font-mono`} aria-label="Inv Value" /></td>
                </tr>
                <tr>
                  <th className="bg-[#f1e3d8] px-3 py-1.5 text-left font-medium dark:bg-[#20253a]">Purchase Type</th>
                  <td className="p-1">
                    <select value={purchaseType} onChange={e => setPurchaseType(e.target.value)} className={`${field} w-full`} aria-label="Purchase Type">
                      {PURCHASE_TYPES.map(t => <option key={t}>{t}</option>)}
                    </select>
                  </td>
                </tr>
              </tbody>
            </table>
            <div className={infoBar}>
              <Info size={16} className="shrink-0 text-amber-500" />
              {balanced
                ? `Supplier invoice ${invNo || ''} matches the bill total.`
                : 'Enter the supplier invoice value; PB Value must match it before saving.'}
            </div>
          </div>
        </div>
      )}

      {/* Item grid */}
      <div className={`${panel} overflow-hidden`}>
        <div className="min-h-[280px] overflow-x-auto">
          <table className="w-full min-w-[1200px] text-xs">
            <thead className="whitespace-nowrap bg-[#f1e3d8] text-stone-600 dark:bg-[#20253a] dark:text-stone-300">
              <tr>
                <th className={`${headCell} w-12`}>Sl No</th>
                <th className={`${headCell} w-28`}>Item Code</th>
                <th className={`${headCell} min-w-72`}>Item Name</th>
                <th className={`${headCell} w-16 text-right`}>QOH</th>
                <th className={`${headCell} w-20 text-right text-[#b84716] dark:text-[#ffb07e]`}>PB Qty</th>
                <th className={`${headCell} w-24 text-right text-[#b84716] dark:text-[#ffb07e]`}>PB Rate</th>
                <th className={`${headCell} w-28 text-right`}>PB Amt</th>
                {showTaxColumns && <>
                  <th className={`${headCell} w-16 text-right`}>CGST %</th>
                  <th className={`${headCell} w-24 text-right`}>CGST Amt</th>
                  <th className={`${headCell} w-16 text-right`}>SGST %</th>
                  <th className={`${headCell} w-24 text-right`}>SGST Amt</th>
                  <th className={`${headCell} w-16 text-right`}>CESS %</th>
                  <th className={`${headCell} w-24 text-right`}>Cess Amount</th>
                  <th className={`${headCell} w-24 text-right`}>Comp.Cess</th>
                </>}
              </tr>
            </thead>
            <tbody className="divide-y divide-[#f1e3d8] dark:divide-[#343b59]">
              {rows.map((row, i) => (
                <tr key={row.uid} onClick={() => setSelectedUid(row.uid)} className={row.uid === selectedUid ? 'bg-[#e8702a]/20' : 'hover:bg-stone-50 dark:hover:bg-[#1b2033]'}>
                  <td className="px-2 py-1 text-stone-500">{i + 1}</td>
                  <td className="px-1 py-1"><input list="pb-codes" value={row.code} onChange={e => setItem(row.uid, 'code', e.target.value)} className={cell} aria-label={`Item code row ${i + 1}`} /></td>
                  <td className="px-1 py-1"><input list="pb-names" value={row.name} onChange={e => setItem(row.uid, 'name', e.target.value)} className={`${cell} font-sans`} aria-label={`Item name row ${i + 1}`} /></td>
                  <td className="px-2 py-1 text-right font-mono text-stone-500">{isFilled(row) ? row.qoh : ''}</td>
                  <td className="px-1 py-1">{numberCell(row, 'qty', 'PB Qty', 'font-bold')}</td>
                  <td className="px-1 py-1">{numberCell(row, 'rate', 'PB Rate')}</td>
                  <td className="px-2 py-1 text-right font-mono font-bold">{isFilled(row) ? money(amt(row)) : ''}</td>
                  {showTaxColumns && <>
                    <td className="px-1 py-1">{numberCell(row, 'cgstPct', 'CGST %')}</td>
                    <td className="px-2 py-1 text-right font-mono">{isFilled(row) ? money(pctOf(row, row.cgstPct)) : ''}</td>
                    <td className="px-1 py-1">{numberCell(row, 'sgstPct', 'SGST %')}</td>
                    <td className="px-2 py-1 text-right font-mono">{isFilled(row) ? money(pctOf(row, row.sgstPct)) : ''}</td>
                    <td className="px-1 py-1">{numberCell(row, 'cessPct', 'CESS %')}</td>
                    <td className="px-2 py-1 text-right font-mono">{isFilled(row) ? money(pctOf(row, row.cessPct)) : ''}</td>
                    <td className="px-1 py-1">{numberCell(row, 'compCess', 'Comp.Cess')}</td>
                  </>}
                </tr>
              ))}
            </tbody>
          </table>
          <datalist id="pb-codes">{ITEM_CODES.map((code, i) => <option key={code} value={code}>{SAMPLE_STOCK_DATA[i].name}</option>)}</datalist>
          <datalist id="pb-names">{SAMPLE_STOCK_DATA.map(item => <option key={item.ksbcl} value={`${item.name} ${item.pkg}`} />)}</datalist>
        </div>
      </div>

      {/* Grid toolbar */}
      <div className="flex flex-wrap items-center gap-1">
        <button onClick={deleteRow} className={iconButton} title="Delete selected row" aria-label="Delete selected row"><XCircle size={18} className="text-red-600" /></button>
        <span className="mx-1 h-5 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={() => moveRow(-1)} disabled={selectedIndex <= 0} className={iconButton} title="Move row up" aria-label="Move row up"><ArrowUpCircle size={18} /></button>
        <button onClick={() => moveRow(1)} disabled={selectedIndex === -1 || selectedIndex >= rows.length - 1} className={iconButton} title="Move row down" aria-label="Move row down"><ArrowDownCircle size={18} /></button>
        <span className="mx-1 h-5 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={() => setLookupOpen(true)} className={iconButton} title="Item lookup" aria-label="Item lookup"><Search size={18} className="text-sky-600" /></button>
        <button onClick={() => fileInput.current?.click()} className={iconButton} title="Attach supplier invoice" aria-label="Attach supplier invoice">
          <Paperclip size={18} className="text-sky-600" />
          {attachments.length > 0 && <span className="absolute -right-1 -top-1 rounded-full bg-[#e8702a] px-1 text-[9px] font-bold text-white">{attachments.length}</span>}
        </button>
        <input ref={fileInput} type="file" multiple hidden onChange={e => setAttachments([...attachments, ...Array.from(e.target.files ?? []).map(f => f.name)])} />
        <span className="mx-1 h-5 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={refreshQoh} className={iconButton} title="Refresh QOH" aria-label="Refresh QOH"><RefreshCw size={18} className="text-amber-600" /></button>
        <button onClick={() => setShowTaxColumns(!showTaxColumns)} className={iconButton} title={showTaxColumns ? 'Hide tax columns' : 'Show tax columns'} aria-label="Toggle tax columns"><Settings2 size={18} /></button>
        <span className="mx-1 h-5 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={() => { setCopyOpen(true); openCopyFrom(); }} className={`${iconButton} flex items-center gap-1 text-xs font-semibold`} title="Copy lines from a Purchase Indent"><FileInput size={18} className="text-[#b84716]" />Copy From</button>
        <span className="mx-1 h-5 w-px bg-[#e1c9b6] dark:bg-[#414a6e]" />
        <button onClick={copyRow} className={iconButton} title="Duplicate selected row" aria-label="Duplicate selected row"><Copy size={18} className="text-[#b84716]" /></button>
        <div className="relative flex items-center">
          <button onClick={() => addRows(1)} className={iconButton} title="Add single item" aria-label="Add single item"><PlusCircle size={18} /></button>
          <button onClick={() => setAddMenuOpen(!addMenuOpen)} className={`${iconButton} px-0.5`} aria-label="More add options"><ChevronDown size={14} /></button>
          {addMenuOpen && (
            <div className="absolute bottom-full left-0 z-10 mb-2 w-44 overflow-hidden rounded-xl border border-[#e1c9b6] bg-[#fffaf5] text-xs shadow-lg dark:border-[#414a6e] dark:bg-[#20253a]">
              <button onClick={() => addRows(1)} className="block w-full px-3 py-2 text-left hover:bg-[#f6efe8] dark:hover:bg-[#343b59]">Add single item</button>
              <button onClick={() => addRows(5)} className="block w-full px-3 py-2 text-left hover:bg-[#f6efe8] dark:hover:bg-[#343b59]">Add 5 blank rows</button>
              <button onClick={() => { const kept = rows.filter(isFilled); setRows(kept.length ? kept : [blankRow()]); setAddMenuOpen(false); }} className="block w-full px-3 py-2 text-left hover:bg-[#f6efe8] dark:hover:bg-[#343b59]">Remove blank rows</button>
            </div>
          )}
        </div>
        <button onClick={() => setLookupOpen(true)} className="ml-4 flex items-center gap-1.5 rounded-lg bg-[#e8702a] px-3 py-1.5 text-xs font-bold text-white shadow-sm hover:bg-[#b84716]"><PlusCircle size={14} />Add Item</button>
        <span className="ml-auto text-xs text-stone-500">{totals.items} items • Qty {totals.qty}</span>
      </div>

      {/* Footer: words / remarks and totals */}
      <div className="grid grid-cols-1 gap-3 lg:grid-cols-[1fr_380px]">
        <div className={`${panel} flex flex-col gap-2 p-3`}>
          {notesOpen && (
            <table className="w-full border border-[#e1c9b6] text-xs dark:border-[#414a6e]">
              <tbody className="divide-y divide-[#e1c9b6] dark:divide-[#414a6e]">
                <tr>
                  <th className="w-36 bg-[#f1e3d8] px-3 py-1.5 text-left font-medium text-sky-700 dark:bg-[#20253a] dark:text-sky-400">Amount In Words</th>
                  <td className="px-3 py-1.5 font-semibold text-emerald-700 dark:text-emerald-400">{numberToWords(totals.net)}</td>
                </tr>
                <tr>
                  <th className="bg-[#f1e3d8] px-3 py-1.5 text-left align-top font-medium dark:bg-[#20253a]">Remarks</th>
                  <td className="p-1"><textarea value={remarks} onChange={e => setRemarks(e.target.value)} rows={3} placeholder="Notes for this purchase bill" className={`${field} w-full`} aria-label="Remarks" /></td>
                </tr>
              </tbody>
            </table>
          )}
          <div className="flex items-center gap-2">
            <div className={`${infoBar} flex-1`}>
              <Info size={16} className="shrink-0 text-amber-500" />
              {selectedRow && isFilled(selectedRow)
                ? `Row ${selectedIndex + 1}: ${selectedRow.name} • QOH ${selectedRow.qoh} → ${selectedRow.qoh + selectedRow.qty} after this bill`
                : `Selected row is empty — pick an item code or name.${attachments.length ? ` Attached: ${attachments.join(', ')}` : ''}`}
            </div>
            <button onClick={() => setNotesOpen(!notesOpen)} className={iconButton} aria-label={notesOpen ? 'Collapse remarks' : 'Expand remarks'}>{notesOpen ? <ChevronsDown size={16} /> : <ChevronsUp size={16} />}</button>
          </div>
        </div>

        <div className={`${panel} flex flex-col gap-2 p-3`}>
          {totalsOpen && (
            <table className="w-full border border-[#e1c9b6] text-xs dark:border-[#414a6e]">
              <tbody className="divide-y divide-[#e1c9b6] dark:divide-[#414a6e]">
                {totalRows.map(([label, value]) => (
                  <tr key={label}>
                    <th className="bg-[#f1e3d8] px-3 py-1.5 text-left font-medium text-sky-700 dark:bg-[#20253a] dark:text-sky-400">
                      {label === 'TCS'
                        ? <span className="flex items-center gap-1 text-stone-700 dark:text-stone-200">TCS @ <input type="number" min={0} step="0.1" value={tcsPct} onChange={e => setTcsPct(e.target.value)} className={`${field} w-14 px-1 py-0.5 text-right`} aria-label="TCS percent" /> %</span>
                        : label}
                    </th>
                    <td className="px-3 py-1.5 text-right font-mono">{money(value)}</td>
                  </tr>
                ))}
                <tr className="bg-[#e8702a]/10 font-bold">
                  <th className="px-3 py-2 text-left text-[#b84716] dark:text-[#ffb07e]">Net Amount</th>
                  <td className="px-3 py-2 text-right font-mono text-[#b84716] dark:text-[#ffb07e]">₹ {money(totals.net)}</td>
                </tr>
              </tbody>
            </table>
          )}
          <div className="flex items-center gap-2">
            <div className={`${infoBar} flex-1`}>
              <Info size={16} className="shrink-0 text-amber-500" />
              TCS on liquor purchases u/s 206C(1).
            </div>
            <button onClick={() => setTotalsOpen(!totalsOpen)} className={iconButton} aria-label={totalsOpen ? 'Collapse totals' : 'Expand totals'}>{totalsOpen ? <ChevronsDown size={16} /> : <ChevronsUp size={16} />}</button>
          </div>
        </div>
      </div>

      <div className="flex justify-center gap-3">
        <button onClick={saveBill} className="flex items-center gap-2 rounded-lg bg-[#e8702a] px-6 py-2 text-sm font-bold text-white shadow-sm hover:bg-[#b84716]"><Save size={16} />Save</button>
        <button onClick={cancelBill} className="flex items-center gap-2 rounded-lg bg-red-600 px-6 py-2 text-sm font-bold text-white shadow-sm hover:bg-red-700"><XCircle size={16} />Cancel</button>
      </div>

      {lookupOpen && (
        <ItemLookupModal filter={lookupFilter} onFilter={setLookupFilter} onPick={pickFromLookup} onClose={() => setLookupOpen(false)} />
      )}
      {copyOpen && <CopyFromModal indents={indents} onCopy={copyFromIndent} onClose={() => setCopyOpen(false)} />}
    </div>
  );
}
