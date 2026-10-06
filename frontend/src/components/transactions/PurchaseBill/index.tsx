// Purchase Bill (legacy frmTrnPB): receive stock from a supplier.
//
// Supplier invoice no/date/value, cases/bottles grid, GST/Cess/TCS, amount in words, Copy From a
// Purchase Indent, and Add Supplier. Saves with POST /transactions/purchase-bill, which adds stock on
// the backend (store.move_stock). Items come from the live catalogue (data/stockData.ts); CGST/SGST
// are the Item Master rates (the backend applies those), so they are shown read-only.
// Keyboard: Enter moves to the next field (Shift+Enter back); on the grid's last field it adds a row.
// F2 item lookup (↑↓, Enter picks; the cursor goes to its PB Qty) · Alt+A add row · ⌘/Ctrl+S or F10 save (no print here) ·
// Alt+X cancel the bill.

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import {
  ArrowDownCircle, ArrowUpCircle, ChevronDown, ChevronsDown, ChevronsUp, Copy, FileInput,
  Info, Paperclip, PlusCircle, RefreshCw, Save, Search, Settings2, UserPlus, XCircle, CheckCircle2, AlertTriangle,
} from 'lucide-react';
import { useConfirm } from '../../common/ConfirmDialog';
import { badge, ghostButton, label as labelClass, pageTitle, primaryButton, secondaryButton, sectionTitle, tableHead } from '../../../lib/styles';
import Toast from '../../common/Toast';
import { toastStore } from '../../common/toastStore';
import DropdownMenu, { menuItem } from '../../common/DropdownMenu';
import { api, errorText, newIdempotencyKey, type ApiDoc, type ApiIndent, type ApiSupplier } from '../../../lib/api';
import { loadStockItems, sellable, STOCK_ITEMS, useStockItems } from '../../../data/stockData';
import { numberToWords } from '../../../lib/formatters';
import type { NextNumber } from '../../../data/salesCounters';
import {
  amt, blankRow, cell, cloneRow, field, headCell, iconButton, infoBar, isFilled,
  money, panel, pctOf, PURCHASE_TYPES, stockRow, today, type Row, type Supplier,
} from './purchaseBillData';
import { AddSupplierModal, CopyFromModal, ItemLookupModal } from './PurchaseBillModals';
import { useDocSettings } from '../../../lib/docSettings';
import Kbd from '../../common/Kbd';
import { enterMovesNext, useShortcuts } from '../../../lib/keys';

export default function PurchaseBill() {
  // The backend gives the PB number when the bill is saved; this is its preview (GET /transactions/next-number).
  const [pbDisplay, setPbDisplay] = useState('—');
  const loadNextNo = useCallback(() => {
    api<NextNumber>('/transactions/next-number?kind=purchase_bill').then(n => setPbDisplay(n.doc_no)).catch(() => setPbDisplay('—'));
  }, []);
  useEffect(() => { loadNextNo(); }, [loadNextNo]);
  const [pbDate, setPbDate] = useState(today);
  // Transaction Settings for Purchase Bill: Copy From allowed, and whether the bill date can be changed.
  const docSettings = useDocSettings('purchase_bill');
  useStockItems();
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [supplier, setSupplier] = useState('');
  const [billingAddress, setBillingAddress] = useState('');
  const [editingAddress, setEditingAddress] = useState(false);
  const [invNo, setInvNo] = useState('');
  const [invDate, setInvDate] = useState(today);
  const [invValue, setInvValue] = useState('');
  const [purchaseType, setPurchaseType] = useState(PURCHASE_TYPES[0]);
  const [headerOpen, setHeaderOpen] = useState(true);
  const [rows, setRows] = useState<Row[]>(() => [blankRow()]);
  const [selectedUid, setSelectedUid] = useState<number>(() => rows[rows.length - 1].uid);
  const [showTaxColumns, setShowTaxColumns] = useState(true);
  const [addMenuOpen, setAddMenuOpen] = useState(false);
  const addMenuRef = useRef<HTMLButtonElement>(null);
  const closeAddMenu = useCallback(() => setAddMenuOpen(false), []);
  const [lookupOpen, setLookupOpen] = useState(false);
  const [indents, setIndents] = useState<ApiIndent[] | null>(null);
  const [copyOpen, setCopyOpen] = useState(false);
  const [lookupFilter, setLookupFilter] = useState('');
  const [attachments, setAttachments] = useState<string[]>([]);
  const [tcsPct, setTcsPct] = useState('1');
  const [remarks, setRemarks] = useState('');
  const [notesOpen, setNotesOpen] = useState(true);
  const { confirm, dialog } = useConfirm();
  const [totalsOpen, setTotalsOpen] = useState(true);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const fileInput = useRef<HTMLInputElement>(null);
  // One key per bill, so a double-click on Save can't add the stock twice. Renewed for each new bill.
  const saveKey = useRef(newIdempotencyKey());

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

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

  // Suppliers come from Supplier Master (address, else city).
  useEffect(() => {
    api<ApiSupplier[]>('/masters/suppliers')
      .then(list => setSuppliers(list.filter(x => x.active).map(x => ({
        name: x.name, address: [x.address, x.city].filter(Boolean).join(', '),
      }))))
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  }, []);

  const [supplierOpen, setSupplierOpen] = useState(false);
  const [supplierSaving, setSupplierSaving] = useState(false);
  const addSupplier = async (name: string, address: string) => {
    // Codes can't hold / ? # % (they go in URLs); upper-case first so lower-case names aren't lost.
    const code = name.toUpperCase().replace(/[^A-Z0-9]+/g, '-').replace(/^-+|-+$/g, '').slice(0, 20);
    if (!code) return setToast({ kind: 'error', text: 'Supplier name needs at least one letter or number.' });
    setSupplierSaving(true);
    if (!(await toastStore.defer('ok', `Supplier "${name}" will be added when this closes.`, 'Supplier not added.'))) {
      setSupplierSaving(false);
      return;
    }
    api('/masters/suppliers', { method: 'POST', body: { code, name, address } })
      .then(() => {
        setSuppliers([...suppliers, { name, address }]);
        setSupplier(name);
        setBillingAddress(address);
        setSupplierOpen(false);
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }))
      .finally(() => setSupplierSaving(false));
  };

  const updateRow = (uid: number, patch: Partial<Row>) => setRows(rows.map(r => (r.uid === uid ? { ...r, ...patch } : r)));

  // Typing an item code or name that matches stock fills in the rest of the row.
  const setItem = (uid: number, key: 'code' | 'name', value: string) => {
    const term = value.trim().toLowerCase();
    const stock = STOCK_ITEMS.findIndex(item => item.active && (key === 'code' ? item.code.toLowerCase() === term : `${item.name} ${item.pkg}`.toLowerCase() === term));
    if (stock === -1) return updateRow(uid, { [key]: value, stock: null });
    const filled = stockRow(stock, 0);
    const current = rows.find(r => r.uid === uid);
    updateRow(uid, {
      stock, code: filled.code, name: filled.name, qoh: filled.qoh, rate: filled.rate, cgstPct: filled.cgstPct, sgstPct: filled.sgstPct,
      qty: current?.qty || 1,
    });
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

  // Reload stock from the server, then copy each row's current QOH (and Item Master GST) into the grid.
  const refreshQoh = async () => {
    if (!(await toastStore.defer('refresh', 'QOH will be refreshed from current stock when this closes.', 'Refresh cancelled.'))) return;
    loadStockItems()
      .then(items => {
        setRows(current => current.map(r => (r.stock === null ? r : {
          ...r, qoh: items[r.stock].clQty, cgstPct: items[r.stock].cgstPct, sgstPct: items[r.stock].sgstPct,
        })));
        setToast({ kind: 'ok', text: 'QOH refreshed from current stock.' });
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
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
      .map(l => ({ stock: STOCK_ITEMS.findIndex(i => i.code === l.item_code), qty: l.qty, rate: l.rate }))
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
    let at = rows.length;
    if (selectedRow && !isFilled(selectedRow)) {
      at = selectedIndex;
      updateRow(selectedUid, { stock, code: row.code, name: row.name, qoh: row.qoh, rate: row.rate, qty: 1 });
    } else {
      setRows([...rows, row]);
      setSelectedUid(row.uid);
    }
    setLookupOpen(false);
    setLookupFilter('');
    // Straight on to the quantity of the picked item.
    setTimeout(() => {
      const qty = document.querySelector<HTMLInputElement>(`[aria-label="PB Qty row ${at + 1}"]`);
      qty?.focus();
      qty?.select();
    }, 0);
  };

  const resetBill = () => {
    saveKey.current = newIdempotencyKey();
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

  const saveBill = async () => {
    const filled = rows.filter(isFilled);
    if (!supplier.trim()) return setToast({ kind: 'error', text: 'Select a supplier.' });
    if (!invNo.trim() || !invDate) return setToast({ kind: 'error', text: 'Enter the supplier Inv No and Inv Date.' });
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one item.' });
    if (filled.some(r => !r.qty || !r.rate)) return setToast({ kind: 'error', text: 'Every item needs a PB Qty and PB Rate.' });
    if (!balanced) return setToast({ kind: 'error', text: `PB value differs from Inv Value by ₹ ${money(Math.abs(balance))}.` });
    if (filled.some(r => r.stock === null)) return setToast({ kind: 'error', text: 'Pick every item with Add Item so it has an item code.' });
    // Waits for its popup (Cancel keeps the bill open). The bill is taken as it was when Save was pressed.
    const key = saveKey.current;
    const body = {
      date: docSettings.allow_edit_date ? pbDate : today(), supplier, supplier_inv_no: invNo, supplier_inv_date: invDate, supplier_inv_value: Number(invValue) || 0,
      purchase_type: purchaseType, remarks, tcs: totals.tcs, cess: totals.cess + totals.compCess,
      lines: filled.map(r => ({ item_code: r.code, bottles: r.qty, rate: r.rate })),
    };
    if (!(await toastStore.defer('ok', `Purchase Bill for ${supplier} (${filled.length} items) will be saved when this closes.`, 'Save cancelled. The bill is still open.'))) return;
    api<ApiDoc>('/transactions/purchase-bill', { method: 'POST', idempotencyKey: key, body }).then(doc => {
      const moved = purchaseType === 'Purchase Return' ? 'returned to the supplier' : 'added to stock';
      setToast({ kind: 'ok', text: `Purchase Bill ${doc.doc_no} saved — ${doc.totals.qty} bottles ${moved}.` });
      loadStockItems().catch(() => undefined);
      loadNextNo();
      resetBill();
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const cancelBill = async () => {
    if (await confirm('Cancel this purchase bill? Unsaved entries will be cleared.')) resetBill();
  };

  const totalRows: [string, number][] = [
    ['Basic amount', totals.basic], ['TCS', totals.tcs], ['CGST', totals.cgst], ['SGST', totals.sgst],
    ['Cess', totals.cess], ['Comp. cess', totals.compCess], ['Round off', totals.roundOff],
  ];

  useShortcuts({
    'Mod+S': () => { void saveBill(); },
    F10: () => { void saveBill(); },
    F2: () => setLookupOpen(true),
    'Alt+A': () => addRows(1),
    'Alt+X': () => { void cancelBill(); },
  });

  const sep = <span className="mx-1 h-5 w-px bg-line" aria-hidden="true" />;

  return (
    <div className="mx-auto flex max-w-[1600px] flex-col gap-4" onKeyDown={e => enterMovesNext(e)}>
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}

      {/* Document header */}
      <div className="flex flex-wrap items-center gap-x-4 gap-y-3">
        <div>
          <div className="flex items-center gap-2">
            <h1 className={pageTitle}>Purchase bill</h1>
            <span className={badge('accent')}>New</span>
          </div>
          <p className="mt-0.5 text-xs text-stone-500 dark:text-stone-400">PB <span className="font-mono font-medium text-stone-800 dark:text-stone-200">{pbDisplay}</span> · {totals.items} items · {totals.qty} bottles</p>
        </div>
        <div className="ml-auto flex items-center gap-2">
          <button onClick={() => setHeaderOpen(!headerOpen)} className={ghostButton} aria-expanded={headerOpen}>
            {headerOpen ? <ChevronsUp size={14} /> : <ChevronsDown size={14} />}{headerOpen ? 'Hide details' : 'Show details'}
          </button>
          <button onClick={cancelBill} className={secondaryButton}><XCircle size={14} />Cancel<Kbd combo="Alt+X" /></button>
          <button onClick={saveBill} className={primaryButton}><Save size={14} />Save bill<Kbd combo="Mod+S" /></button>
        </div>
      </div>

      {headerOpen && (
        <div className="grid grid-cols-1 gap-4 xl:grid-cols-[minmax(0,1.2fr)_minmax(0,1fr)]">
          {/* PB number, supplier, billing address */}
          <section aria-label="Bill details" className={`${panel} grid gap-4 p-5 sm:grid-cols-2`}>
            <div>
              <label className={labelClass} htmlFor="pb-no">Next PB no.</label>
              <input id="pb-no" value={pbDisplay} readOnly disabled title="Given when the bill is saved" className={`${field} font-mono`} />
            </div>
            <div>
              <label className={labelClass} htmlFor="pb-date">Bill date</label>
              <input id="pb-date" type="date" value={docSettings.allow_edit_date ? pbDate : today()} disabled={!docSettings.allow_edit_date} onChange={e => setPbDate(e.target.value)} className={`${field} tabular-nums`} />
            </div>
            <div className="sm:col-span-2">
              <label htmlFor="pb-supplier" className={labelClass}>Supplier<span className="ml-0.5 text-red-600" aria-hidden="true">*</span></label>
              <div className="flex gap-2">
                <input id="pb-supplier" list="pb-suppliers" value={supplier} onChange={e => selectSupplier(e.target.value)} placeholder="Select or type supplier" className={field} />
                <button onClick={() => setSupplierOpen(true)} className={secondaryButton} title="Add new supplier"><UserPlus size={14} /><span className="hidden sm:inline">New</span></button>
              </div>
              <datalist id="pb-suppliers">{suppliers.map(s => <option key={s.name} value={s.name} />)}</datalist>
            </div>
            <div className="sm:col-span-2">
              <div className="mb-1.5 flex items-center justify-between">
                <span className={`${labelClass} mb-0`}>Billing address</span>
                <button onClick={() => setEditingAddress(!editingAddress)} className="text-xs font-medium text-accent-ink hover:underline">{editingAddress ? 'Done' : 'Change'}</button>
              </div>
              {editingAddress
                ? <textarea value={billingAddress} onChange={e => setBillingAddress(e.target.value)} rows={2} className={field} aria-label="Billing address" />
                : <p className="text-xs leading-relaxed text-stone-700 dark:text-stone-300">{billingAddress || <span className="text-stone-400">—</span>}</p>}
            </div>
          </section>

          {/* Supplier invoice + match status */}
          <section aria-label="Supplier invoice" className={`${panel} flex flex-col gap-4 p-5`}>
            <div className="grid gap-4 sm:grid-cols-2">
              <div>
                <label htmlFor="pb-inv-no" className={labelClass}>Supplier invoice no.</label>
                <input id="pb-inv-no" value={invNo} onChange={e => setInvNo(e.target.value)} className={`${field} font-mono`} />
              </div>
              <div>
                <label htmlFor="pb-inv-date" className={labelClass}>Invoice date</label>
                <input id="pb-inv-date" type="date" value={invDate} onChange={e => setInvDate(e.target.value)} className={`${field} tabular-nums`} />
              </div>
              <div>
                <label htmlFor="pb-inv-val" className={labelClass}>Invoice value (₹)</label>
                <input id="pb-inv-val" type="number" min={0} step="0.01" value={invValue} onChange={e => setInvValue(e.target.value)} className={`${field} text-right tabular-nums`} />
              </div>
              <div>
                <label htmlFor="pb-type" className={labelClass}>Purchase type</label>
                <select id="pb-type" value={purchaseType} onChange={e => setPurchaseType(e.target.value)} className={field}>
                  {PURCHASE_TYPES.map(t => <option key={t}>{t}</option>)}
                </select>
              </div>
            </div>
            <div role="status" className={`mt-auto flex items-center gap-3 rounded-lg px-4 py-3 ${balanced ? 'bg-emerald-500/10 text-emerald-800 dark:text-emerald-300' : 'bg-amber-500/12 text-amber-800 dark:text-amber-300'}`}>
              {balanced ? <CheckCircle2 size={18} className="shrink-0" /> : <AlertTriangle size={18} className="shrink-0" />}
              <div className="min-w-0 flex-1 text-xs">
                <div className="font-semibold">{balanced ? 'Matches supplier invoice' : `₹ ${money(balance)} left to enter`}</div>
                <div className="opacity-80">{balanced ? `Invoice ${invNo || ''} agrees with the bill total.` : 'PB value must match the invoice value before saving.'}</div>
              </div>
              <div className="text-right">
                <div className="text-[11px] opacity-80">PB value</div>
                <div className="text-lg font-semibold tabular-nums">₹ {money(totals.net)}</div>
              </div>
            </div>
          </section>
        </div>
      )}

      {/* Line items */}
      <section aria-label="Line items" className={`${panel} overflow-hidden`} onKeyDown={e => enterMovesNext(e, () => addRows(1))}>
        <div className="flex flex-wrap items-center gap-1 border-b border-line px-3 py-2">
          <button onClick={() => setLookupOpen(true)} className={`${primaryButton} mr-2`}><PlusCircle size={14} />Add item<Kbd combo="F2" /></button>
          <div className="relative flex items-center">
            <button onClick={() => addRows(1)} className={iconButton} title="Add blank row (Alt+A)" aria-label="Add blank row"><PlusCircle size={16} /></button>
            <button ref={addMenuRef} onClick={() => setAddMenuOpen(!addMenuOpen)} className={`${iconButton} w-6`} aria-label="More add options" aria-expanded={addMenuOpen} aria-haspopup="menu"><ChevronDown size={14} /></button>
            <DropdownMenu anchorRef={addMenuRef} open={addMenuOpen} onClose={closeAddMenu} align="start" width={192}>
              <button role="menuitem" onClick={() => addRows(1)} className={menuItem}>Add single item</button>
              <button role="menuitem" onClick={() => addRows(5)} className={menuItem}>Add 5 blank rows</button>
              <button role="menuitem" onClick={() => { const kept = rows.filter(isFilled); setRows(kept.length ? kept : [blankRow()]); setAddMenuOpen(false); }} className={menuItem}>Remove blank rows</button>
            </DropdownMenu>
          </div>
          <button onClick={copyRow} className={iconButton} title="Duplicate selected row" aria-label="Duplicate selected row"><Copy size={16} /></button>
          <button onClick={deleteRow} className={`${iconButton} hover:!text-red-600`} title="Delete selected row" aria-label="Delete selected row"><XCircle size={16} /></button>
          {sep}
          <button onClick={() => moveRow(-1)} disabled={selectedIndex <= 0} className={iconButton} title="Move row up" aria-label="Move row up"><ArrowUpCircle size={16} /></button>
          <button onClick={() => moveRow(1)} disabled={selectedIndex === -1 || selectedIndex >= rows.length - 1} className={iconButton} title="Move row down" aria-label="Move row down"><ArrowDownCircle size={16} /></button>
          {sep}
          <button onClick={() => setLookupOpen(true)} className={iconButton} title="Item lookup" aria-label="Item lookup"><Search size={16} /></button>
          <button onClick={refreshQoh} className={iconButton} title="Refresh stock on hand" aria-label="Refresh stock on hand"><RefreshCw size={16} /></button>
          <button onClick={() => setShowTaxColumns(!showTaxColumns)} className={`${iconButton} ${showTaxColumns ? '' : '!text-accent'}`} title={showTaxColumns ? 'Hide tax columns' : 'Show tax columns'} aria-label="Toggle tax columns" aria-pressed={!showTaxColumns}><Settings2 size={16} /></button>
          <button onClick={() => fileInput.current?.click()} className={`${iconButton} relative`} title="Attach supplier invoice" aria-label="Attach supplier invoice">
            <Paperclip size={16} />
            {attachments.length > 0 && <span className="absolute -right-0.5 -top-0.5 rounded-full bg-accent px-1 text-[9px] font-semibold text-white">{attachments.length}</span>}
          </button>
          <input ref={fileInput} type="file" multiple hidden onChange={e => setAttachments([...attachments, ...Array.from(e.target.files ?? []).map(f => f.name)])} />
          {sep}
          {docSettings.allow_copy_from && <button onClick={() => { setCopyOpen(true); openCopyFrom(); }} className={ghostButton} title="Copy lines from a Purchase Indent"><FileInput size={14} />Copy from indent</button>}
          <span className="ml-auto pr-1 text-xs tabular-nums text-stone-500">{totals.items} items · {totals.qty} qty</span>
        </div>
        <div className="min-h-[260px] overflow-x-auto">
          <table className="w-full min-w-[1200px] text-xs">
            <thead className={tableHead}>
              <tr>
                <th className={`${headCell} w-12 pl-4`}>#</th>
                <th className={`${headCell} w-28`}>Item code</th>
                <th className={`${headCell} min-w-72`}>Item name</th>
                <th className={`${headCell} w-16 text-right`}>In stock</th>
                <th className={`${headCell} w-20 text-right`}>Qty</th>
                <th className={`${headCell} w-24 text-right`}>Rate</th>
                <th className={`${headCell} w-28 text-right`}>Amount</th>
                {showTaxColumns && <>
                  <th className={`${headCell} w-16 text-right`}>CGST %</th>
                  <th className={`${headCell} w-24 text-right`}>CGST amt</th>
                  <th className={`${headCell} w-16 text-right`}>SGST %</th>
                  <th className={`${headCell} w-24 text-right`}>SGST amt</th>
                  <th className={`${headCell} w-16 text-right`}>Cess %</th>
                  <th className={`${headCell} w-24 text-right`}>Cess amt</th>
                  <th className={`${headCell} w-24 pr-4 text-right`}>Comp. cess</th>
                </>}
              </tr>
            </thead>
            <tbody className="divide-y divide-line-soft tabular-nums">
              {rows.map((row, i) => (
                <tr key={row.uid} onClick={() => setSelectedUid(row.uid)} onFocus={() => setSelectedUid(row.uid)} aria-selected={row.uid === selectedUid} className={`transition-colors ${row.uid === selectedUid ? 'bg-accent-soft' : 'hover:bg-subtle/70'}`}>
                  <td className="py-1 pl-4 pr-2 text-stone-400">{i + 1}</td>
                  <td className="px-1 py-1"><input list="pb-codes" value={row.code} onChange={e => setItem(row.uid, 'code', e.target.value)} className={`${cell} font-mono`} aria-label={`Item code row ${i + 1}`} /></td>
                  <td className="px-1 py-1"><input list="pb-names" value={row.name} onChange={e => setItem(row.uid, 'name', e.target.value)} className={`${cell} font-medium text-stone-900 dark:text-stone-50`} aria-label={`Item name row ${i + 1}`} /></td>
                  <td className="px-2 py-1 text-right text-stone-500">{isFilled(row) ? row.qoh : ''}</td>
                  <td className="px-1 py-1">{numberCell(row, 'qty', 'PB Qty', 'font-semibold')}</td>
                  <td className="px-1 py-1">{numberCell(row, 'rate', 'PB Rate')}</td>
                  <td className="px-2 py-1 text-right font-semibold text-stone-900 dark:text-stone-50">{isFilled(row) ? money(amt(row)) : ''}</td>
                  {showTaxColumns && <>
                    <td className="px-2 py-1 text-right tabular-nums text-stone-600 dark:text-stone-300" title="From Item Master">{isFilled(row) ? row.cgstPct : ''}</td>
                    <td className="px-2 py-1 text-right text-stone-600 dark:text-stone-300">{isFilled(row) ? money(pctOf(row, row.cgstPct)) : ''}</td>
                    <td className="px-2 py-1 text-right tabular-nums text-stone-600 dark:text-stone-300" title="From Item Master">{isFilled(row) ? row.sgstPct : ''}</td>
                    <td className="px-2 py-1 text-right text-stone-600 dark:text-stone-300">{isFilled(row) ? money(pctOf(row, row.sgstPct)) : ''}</td>
                    <td className="px-1 py-1">{numberCell(row, 'cessPct', 'CESS %')}</td>
                    <td className="px-2 py-1 text-right text-stone-600 dark:text-stone-300">{isFilled(row) ? money(pctOf(row, row.cessPct)) : ''}</td>
                    <td className="py-1 pl-1 pr-3">{numberCell(row, 'compCess', 'Comp.Cess')}</td>
                  </>}
                </tr>
              ))}
            </tbody>
          </table>
          <datalist id="pb-codes">{sellable().map(item => <option key={item.code} value={item.code}>{item.name}</option>)}</datalist>
          <datalist id="pb-names">{sellable().map(item => <option key={item.code} value={`${item.name} ${item.pkg}`} />)}</datalist>
        </div>
        <div className={`${infoBar} rounded-none border-t border-line`}>
          <Info size={14} className="shrink-0 text-stone-400" />
          {selectedRow && isFilled(selectedRow)
            ? `Row ${selectedIndex + 1}: ${selectedRow.name} · stock ${selectedRow.qoh} → ${selectedRow.qoh + selectedRow.qty} after this bill`
            : `Selected row is empty. Pick an item code or name.${attachments.length ? ` Attached: ${attachments.join(', ')}` : ''}`}
        </div>
      </section>

      {/* Remarks + totals */}
      <div className="grid grid-cols-1 items-start gap-4 lg:grid-cols-[minmax(0,1fr)_380px]">
        <section aria-label="Remarks" className={`${panel} p-5`}>
          <div className="mb-3 flex items-center justify-between">
            <h2 className={sectionTitle}>Notes</h2>
            <button onClick={() => setNotesOpen(!notesOpen)} className={iconButton} aria-expanded={notesOpen} aria-label={notesOpen ? 'Collapse notes' : 'Expand notes'}>{notesOpen ? <ChevronsUp size={16} /> : <ChevronsDown size={16} />}</button>
          </div>
          {notesOpen && (
            <div className="space-y-4">
              <div>
                <div className={labelClass}>Amount in words</div>
                <p className="text-xs italic text-stone-700 dark:text-stone-300">{numberToWords(totals.net)}</p>
              </div>
              <div>
                <label htmlFor="pb-remarks" className={labelClass}>Remarks</label>
                <textarea id="pb-remarks" value={remarks} onChange={e => setRemarks(e.target.value)} rows={3} placeholder="Notes for this purchase bill" className={field} />
              </div>
            </div>
          )}
        </section>

        <section aria-label="Totals" className={`${panel} p-5 text-xs`}>
          <div className="mb-3 flex items-center justify-between">
            <h2 className={sectionTitle}>Totals</h2>
            <button onClick={() => setTotalsOpen(!totalsOpen)} className={iconButton} aria-expanded={totalsOpen} aria-label={totalsOpen ? 'Collapse totals' : 'Expand totals'}>{totalsOpen ? <ChevronsUp size={16} /> : <ChevronsDown size={16} />}</button>
          </div>
          {totalsOpen && (
            <dl className="space-y-2 tabular-nums">
              {totalRows.map(([label, value]) => (
                <div key={label} className="flex items-center justify-between gap-3">
                  <dt className="text-stone-500 dark:text-stone-400">
                    {label === 'TCS'
                      ? <span className="flex items-center gap-1.5">TCS @ <input type="number" min={0} step="0.1" value={tcsPct} onChange={e => setTcsPct(e.target.value)} className={`${field} w-16 px-2 py-1 text-right`} aria-label="TCS percent" /> %</span>
                      : label}
                  </dt>
                  <dd className={value ? 'text-stone-800 dark:text-stone-200' : 'text-stone-400'}>{money(value)}</dd>
                </div>
              ))}
            </dl>
          )}
          <div className="mt-3 flex items-baseline justify-between gap-3 border-t border-line pt-3">
            <span className="text-sm font-medium text-stone-700 dark:text-stone-200">Net amount</span>
            <span className="text-2xl font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">₹ {money(totals.net)}</span>
          </div>
          <p className="mt-2 text-[11px] text-stone-500">TCS on liquor purchases u/s 206C(1).</p>
        </section>
      </div>

      {lookupOpen && (
        <ItemLookupModal filter={lookupFilter} onFilter={setLookupFilter} onPick={pickFromLookup} onClose={() => setLookupOpen(false)} />
      )}
      {supplierOpen && <AddSupplierModal saving={supplierSaving} onSave={addSupplier} onClose={() => setSupplierOpen(false)} />}
      {copyOpen && <CopyFromModal indents={indents} onCopy={copyFromIndent} onClose={() => setCopyOpen(false)} />}
    </div>
  );
}
