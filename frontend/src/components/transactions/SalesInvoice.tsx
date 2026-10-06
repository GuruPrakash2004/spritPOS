// Sales Invoice (POS billing, legacy frmTrnSINV), step 2 after SalesInvoiceSetup.
//
// Barcode/item entry, qty +/-, collected amount → payback/shortage, loyalty points, total litres,
// Hold / New Bill / Cancel / Re-Print. Saves with POST /transactions/sales-invoice, which reduces
// stock on the backend (store.move_stock). Held bills get their own HOLD- series.
// Items come from the live catalogue (data/stockData.ts; a line's `id` is its STOCK_ITEMS index), and
// the bill total is rounded to the rupee exactly as the backend rounds it.
// Global Settings used here: QTYSELINV (Y: Enter after a scan moves to Qty; N: the item is added at once)
// and InvPreview (Y: the print preview opens after every saved bill). Transaction/Print Settings for Sales Invoice
// (lib/docSettings.ts): after save none / ask "Do you want to print the invoice..?" / preview, and the default
// print format.
//
// Keyboard first (the counter rarely uses a mouse; the full list is in the Shortcuts sheet, `?`):
//   F2 item search · F3 quick pick · F4 customer · F6 or Alt+P next payment type · F7 hold / new · F8 held bills
//   F9 cash tendered · F10 save & print · ⌘/Ctrl+S save only
//   Alt+R re-print · Alt+C cancel a saved invoice · Alt+X cancel this bill
//   In the empty item search: ↑/↓ choose a line, + / − change its qty, Delete removes it, Enter edits its qty,
//   and "3*" sets the qty for the next item. In a line: ↑/↓ move between lines, Enter or Esc back to the search.
//   Waiting popups: Enter saves now, Esc cancels.

import { useEffect, useMemo, useRef, useState, type KeyboardEvent as ReactKeyboardEvent } from 'react';
import { PauseCircle, Search, Trash2, Undo2, XCircle, Eraser, Save, DoorOpen, PanelLeftOpen, PanelLeftClose, X, Printer, Ban, Gift, ScanLine, ChevronDown } from 'lucide-react';
import Kbd from '../common/Kbd';
import ItemSearchBox, { type ItemMatch } from './ItemSearchBox';
import { focusField, useShortcuts } from '../../lib/keys';
import { useConfirm } from '../common/ConfirmDialog';
import {  badge, field as baseField, ghostButton, iconButton, label as labelClass, panel, primaryButton, secondaryButton, tableHead , printButton } from '../../lib/styles';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { api, errorText, newIdempotencyKey, todayIso, type ApiCustomer, type ApiInvoice } from '../../lib/api';
import { CancelInvoiceDialog, HeldBillsDialog, ReprintDialog } from './SalesInvoiceDialogs';
import { STOCK_ITEMS, litresPerBottle, loadStockItems, sellable, stockIndex, useStockItems } from '../../data/stockData';
import { numberToWords } from '../../lib/formatters';
import { PAYMENT_TYPES, type InvoiceSetup, type NextNumber, type PaymentType } from '../../data/salesCounters';
import PrintDialog from '../print/PrintDialog';
import { afterSavePlan, runPrint } from '../../lib/printing';
import { useDocSettings } from '../../lib/docSettings';

type Line = { id: number; name: string; qoh: number; qty: number; rate: number; discPct: number };

const toLine = (stock: number, qty: number, discPct = 0): Line => {
  const item = STOCK_ITEMS[stock];
  return { id: stock, name: `${item.name} ${item.pkg}`, qoh: item.clQty, qty, rate: item.rate, discPct };
};

const money = (n: number) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
const gross = (l: Line) => l.qty * l.rate;
const discAmt = (l: Line) => (gross(l) * l.discPct) / 100;
// Splits "26/c10-0001234" into "26/c10-" + 1234 so later bills keep the same prefix and zero padding.
const parseInvoiceNo = (invoiceNo: string) => {
  const match = invoiceNo.match(/^(.*?)(\d+)$/);
  return match ? { head: match[1], seq: Number(match[2]), width: match[2].length } : { head: `${invoiceNo}-`, seq: 1, width: 1 };
};

const field = baseField;
const cellInput = 'w-16 rounded-md border border-transparent bg-transparent px-1.5 py-1 text-right tabular-nums hover:border-line-strong focus:border-accent focus:bg-surface focus:outline-none focus:ring-2 focus:ring-accent/20';

export default function SalesInvoice({ setup, onClose }: { setup: InvoiceSetup; onClose: () => void }) {
  const { counter } = setup;
  // The bill number preview (the backend gives the real number on save); null until it is known.
  const [numbering, setNumbering] = useState(() => (setup.invoiceNo ? parseInvoiceNo(setup.invoiceNo) : null));
  const [now, setNow] = useState(() => new Date());
  const [seq, setSeq] = useState(numbering?.seq ?? 1);
  const [prevBill, setPrevBill] = useState<{ no: string; time: string; amount: number } | null>(null);
  const [party, setParty] = useState(setup.customer);
  useStockItems();
  const [salesPersons, setSalesPersons] = useState<string[]>([]);
  const [salesPerson, setSalesPerson] = useState('');
  const [lines, setLines] = useState<Line[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [barcode, setBarcode] = useState('');
  const [scanQty, setScanQty] = useState('1');
  const [quickFilter, setQuickFilter] = useState('');
  const [quickOpen, setQuickOpen] = useState(false);
  const [quickAt, setQuickAt] = useState(0);
  const [writeOff, setWriteOff] = useState('0');
  const [paymentType, setPaymentType] = useState<PaymentType>(setup.paymentType);
  const [otherPay, setOtherPay] = useState('0');
  const [cardInput, setCardInput] = useState('0');
  const [collected, setCollected] = useState('');
  const [customers, setCustomers] = useState<ApiCustomer[]>([]);
  const [redeem, setRedeem] = useState('');
  const [dialog, setDialog] = useState<'reprint' | 'cancel' | 'held' | null>(null);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [printDocNo, setPrintDocNo] = useState<string | null>(null);
  const barcodeRef = useRef<HTMLInputElement>(null);
  // From the moment a save/hold goes to the server until it answers, the bill can't change or be sent again,
  // so a bottle scanned straight after F10 + Enter never lands on the bill being saved.
  const sending = useRef(false);
  const [busy, setBusy] = useState(false);
  const locked = () => {
    if (sending.current) setToast({ kind: 'error', text: 'Saving the last bill. Scan again in a moment.' });
    return sending.current;
  };
  const send = (body: object, key: string) => {
    sending.current = true;
    setBusy(true);
    return api<ApiInvoice>('/transactions/sales-invoice', { method: 'POST', body, idempotencyKey: key })
      .finally(() => { sending.current = false; setBusy(false); });
  };
  // One key per bill: pressing Save twice (or a retry) can't make two bills. Renewed for each new bill.
  const saveKey = useRef(newIdempotencyKey());
  const { confirm, dialog: confirmDialog } = useConfirm();
  // Global Settings values (GET /settings/values); until they load, a scan adds the item at once.
  const [gsettings, setGsettings] = useState<Record<string, string>>({});
  const qtyAfterScan = gsettings.QTYSELINV?.toUpperCase() === 'Y';
  const docSettings = useDocSettings('sales_invoice');

  useEffect(() => {
    const timer = setInterval(() => setNow(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // Customers come from Customer Master so loyalty points are current; salespeople from the backend.
  useEffect(() => {
    api<ApiCustomer[]>('/masters/customers').then(setCustomers).catch(e => toastStore.add('error', errorText(e)));
    api<Record<string, string>>('/settings/values').then(v => { if (v && typeof v === 'object') setGsettings(v); }).catch(() => undefined);
    api<string[]>('/sales-persons')
      .then(list => { setSalesPersons(list); setSalesPerson(p => (list.includes(p) ? p : list[0] ?? '')); })
      .catch(e => toastStore.add('error', errorText(e)));
  }, []);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  // Opened without a previewed number (e.g. straight from the sidebar): ask the backend for this counter's next one.
  useEffect(() => {
    if (setup.invoiceNo || !counter.code) return;
    api<NextNumber>(`/transactions/next-number?${new URLSearchParams({ kind: 'sales_invoice', counter: counter.code })}`)
      .then(n => { const p = parseInvoiceNo(n.doc_no); setNumbering(p); setSeq(p.seq); })
      .catch(() => undefined);
  }, [setup.invoiceNo, counter.code]);

  const billNo = numbering ? `${numbering.head}${String(seq).padStart(numbering.width, '0')}` : '—';

  const totals = useMemo(() => {
    const basic = lines.reduce((acc, l) => acc + gross(l), 0);
    const disc = lines.reduce((acc, l) => acc + discAmt(l), 0);
    // Liquor is outside GST in Karnataka (excise is built into MRP), so GST/cess rows stay at zero for these items.
    const cgst = 0, sgst = 0, cess = 0, compCess = 0;
    const total = basic - disc + cgst + sgst + cess + compCess;
    const points = Math.max(0, Number(redeem) || 0);
    // Rounded to the rupee, as the backend rounds the saved bill.
    const invoice = Math.max(0, Math.round(total - (Number(writeOff) || 0) - points));
    const card = paymentType === 'CARD' ? invoice : paymentType === 'SPLIT' ? Number(cardInput) || 0 : 0;
    const other = paymentType === 'UPI' ? invoice : paymentType === 'SPLIT' ? Number(otherPay) || 0 : 0;
    const cash = Math.max(0, invoice - card - other);
    const qty = lines.reduce((acc, l) => acc + l.qty, 0);
    const litres = lines.reduce((acc, l) => acc + l.qty * litresPerBottle(STOCK_ITEMS[l.id].pkg), 0);
    return { basic, disc, cgst, sgst, cess, compCess, total, points, invoice, card, other, cash, qty, litres };
  }, [lines, writeOff, redeem, paymentType, cardInput, otherPay]);

  const customer = customers.find(c => c.name.toLowerCase() === party.trim().toLowerCase() || c.code.toLowerCase() === party.trim().toLowerCase());

  const change = collected === '' ? 0 : (Number(collected) || 0) - totals.cash;

  const quickItems = sellable()
    .filter(item => `${item.brand} ${item.name} ${item.code}`.toLowerCase().includes(quickFilter.toLowerCase()));

  const addItem = (stock: number, qty: number, current = lines) => {
    if (locked()) return current;
    const item = STOCK_ITEMS[stock];
    const existing = current.find(l => l.id === stock);
    const nextQty = (existing?.qty ?? 0) + qty;
    if (nextQty > item.clQty) {
      setToast({ kind: 'error', text: `Only ${item.clQty} in stock for ${item.brand} ${item.pkg}.` });
      return current;
    }
    const next = existing
      ? current.map(l => (l.id === stock ? { ...l, qty: nextQty } : l))
      : [...current, toLine(stock, qty)];
    setLines(next);
    setSelectedId(stock);
    return next;
  };

  const scan = (code?: string) => {
    if (locked()) return;
    const term = (code ?? barcode).trim().toLowerCase();
    const qty = Math.floor(Number(scanQty));
    if (!term) return;
    if (!qty || qty < 1) return setToast({ kind: 'error', text: 'Enter a quantity of at least 1.' });
    // Exact item code first (barcode scanners send the code), then a brand/name match.
    const exact = STOCK_ITEMS.findIndex(i => i.active && i.code.toLowerCase() === term);
    const stock = exact !== -1 ? exact : STOCK_ITEMS.findIndex(i => i.active && `${i.brand} ${i.name}`.toLowerCase().includes(term));
    if (stock === -1) return setToast({ kind: 'error', text: `No item found for "${code ?? barcode}".` });
    addItem(stock, qty);
    setBarcode('');
    setScanQty('1');
    barcodeRef.current?.focus();
  };

  const updateLine = (id: number, patch: Partial<Line>) => {
    if (sending.current) return;
    setLines(lines.map(l => {
    if (l.id !== id) return l;
    const next = { ...l, ...patch };
    next.qty = Math.max(0, Math.min(next.qoh, Math.floor(next.qty) || 0));
    next.rate = Math.max(0, next.rate || 0);
    next.discPct = Math.max(0, Math.min(100, next.discPct || 0));
    return next;
  }));
  };

  // A picked suggestion: straight onto the bill, or first to Qty when QTYSELINV is Y.
  const pickItem = (item: ItemMatch) => {
    if (qtyAfterScan) {
      setBarcode(item.code);
      focusField('si-qty');
    } else {
      scan(item.code);
    }
  };

  const selectedIndex = lines.findIndex(l => l.id === selectedId);
  const focusSearch = () => focusField('si-barcode');
  const focusLineCell = (row: number, col: string) => {
    const el = document.querySelector<HTMLInputElement>(`[data-si-row="${row}"][data-si-col="${col}"]`);
    el?.focus();
    el?.select();
  };

  // Keys in the empty item search work on the selected line (or the last one).
  const searchKeys = (e: ReactKeyboardEvent<HTMLInputElement>) => {
    if (e.key === '*' && /^\d+$/.test(barcode.trim())) {   // "3*" then the item: sell 3
      e.preventDefault();
      setScanQty(barcode.trim());
      setBarcode('');
      return;
    }
    if (barcode || !lines.length) return;
    if (sending.current && ['+', '-', 'Delete'].includes(e.key)) { e.preventDefault(); locked(); return; }
    const at = selectedIndex === -1 ? lines.length - 1 : selectedIndex;
    const line = lines[at];
    if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
      e.preventDefault();
      const next = selectedIndex === -1 ? lines.length - 1 : Math.max(0, Math.min(lines.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1)));
      setSelectedId(lines[next].id);
    } else if (e.key === '+' || e.key === '-') {
      e.preventDefault();
      if (e.key === '+' && line.qty + 1 > line.qoh) return setToast({ kind: 'error', text: `Only ${line.qoh} in stock for ${line.name}.` });
      if (e.key === '-' && line.qty <= 1) return;
      updateLine(line.id, { qty: line.qty + (e.key === '+' ? 1 : -1) });
      setSelectedId(line.id);
    } else if (e.key === 'Delete') {
      e.preventDefault();
      setLines(lines.filter(l => l.id !== line.id));
      setSelectedId(lines[at + 1]?.id ?? lines[at - 1]?.id ?? null);
    } else if (e.key === 'Enter') {
      focusLineCell(at, 'qty');
    }
  };

  // Keys inside a line's Qty / Rate / Disc % box.
  const cellKeys = (row: number, col: string) => (e: ReactKeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' || e.key === 'Escape') {
      e.preventDefault();
      e.stopPropagation();
      if (!lines[row].qty) updateLine(lines[row].id, { qty: 1 });
      focusSearch();
    } else if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
      e.preventDefault();
      const next = row + (e.key === 'ArrowDown' ? 1 : -1);
      if (next >= 0 && next < lines.length) { setSelectedId(lines[next].id); focusLineCell(next, col); }
    }
  };

  const nextPayment = (step: number) =>
    setPaymentType(p => PAYMENT_TYPES[(PAYMENT_TYPES.indexOf(p) + step + PAYMENT_TYPES.length) % PAYMENT_TYPES.length]);

  const toggleQuick = () => {
    if (quickOpen) { setQuickOpen(false); focusSearch(); } else { setQuickOpen(true); setQuickAt(0); }
  };

  const resetBill = () => {
    saveKey.current = newIdempotencyKey();
    setLines([]);
    setSelectedId(null);
    setParty(setup.customer);
    setWriteOff('0');
    setPaymentType(setup.paymentType);
    setOtherPay('0');
    setCardInput('0');
    setCollected('');
    setRedeem('');
    barcodeRef.current?.focus();
  };

  const clearItems = () => { setLines([]); setSelectedId(null); };

  const deleteItem = () => {
    const target = selectedId ?? lines.at(-1)?.id;
    if (target === undefined) return;
    setLines(lines.filter(l => l.id !== target));
    setSelectedId(null);
  };

  const billBody = (hold: boolean) => ({
    date: todayIso(), counter: setup.counter.code, customer: party || 'CASH', salesperson: salesPerson,
    payment_type: paymentType,
    lines: lines.map(l => ({ item_code: STOCK_ITEMS[l.id].code, bottles: l.qty, rate: l.rate })),
    discount: totals.disc + (Number(writeOff) || 0),
    redeem_points: totals.points,
    payment: { cash: totals.cash, card: totals.card, upi: totals.other, credit: 0 },
    hold,
  });

  // Held bills are parked on the backend so any counter session can recall them.
  const holdOrNew = async () => {
    if (locked()) return;
    if (!lines.length) return resetBill();
    // Waits for its popup (Cancel keeps the bill open). The bill is taken as it was when Hold was pressed.
    const body = billBody(true);
    const key = saveKey.current;
    if (!(await toastStore.defer('ok', `Bill will be held (${lines.length} items) when this closes.`, 'Hold cancelled. The bill is still open.'))) return;
    if (locked()) return;
    send(body, key)
      .then(doc => {
        setToast({ kind: 'ok', text: `Bill held as ${doc.doc_no} (${lines.length} items). Starting a new bill.` });
        resetBill();
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const recall = (inv: ApiInvoice) => {
    const found = inv.lines.map(l => ({ stock: stockIndex(l.item_code), qty: l.qty, rate: l.rate }));
    const missing = found.filter(l => l.stock === -1).length;
    const recalled = found.filter(l => l.stock !== -1).map(l => ({ ...toLine(l.stock, l.qty), rate: l.rate }));
    saveKey.current = newIdempotencyKey();   // a recalled bill is a different bill
    setLines(recalled);
    setParty(customers.find(c => c.code === inv.customer)?.name ?? inv.customer);
    setDialog(null);
    setToast(missing
      ? { kind: 'error', text: `Recalled ${inv.doc_no}, but ${missing} item${missing > 1 ? 's are' : ' is'} no longer in Item Master and was left out.` }
      : { kind: 'ok', text: `Recalled held bill ${inv.doc_no}.` });
  };

  const cancelBill = async () => {
    if (locked()) return;
    if (lines.length && !(await confirm('Cancel this bill? All items will be removed.'))) return;
    resetBill();
  };

  const saveBill = async (andPrint = false) => {
    if (locked()) return;
    if (!lines.length) return setToast({ kind: 'error', text: 'Add at least one item before saving the bill.' });
    if (lines.some(l => !l.qty || !l.rate)) return setToast({ kind: 'error', text: 'Every item needs a quantity and a rate.' });
    if (totals.card + totals.other > totals.invoice) return setToast({ kind: 'error', text: 'Card + other payment exceeds the invoice value.' });
    if (collected !== '' && change < 0) return setToast({ kind: 'error', text: `Collected amount is ₹ ${money(-change)} short of the cash amount.` });
    if (totals.points > (customer?.points ?? 0)) return setToast({ kind: 'error', text: `Only ${customer?.points ?? 0} points available to redeem.` });
    // Waits for its popup (Cancel keeps the bill open). The bill is taken as it was when Save was pressed.
    const body = billBody(false);
    const key = saveKey.current;
    if (!(await toastStore.defer('ok', `Bill of ₹ ${money(totals.invoice)} will be saved when this closes.`, 'Save cancelled. The bill is still open.'))) return;
    if (locked()) return;   // a second F10 whose popup closed while the first save was still going
    // The backend numbers the bill and moves stock; show its doc no rather than the local preview.
    send(body, key).then(doc => {
      // Refresh points and stock so the next bill sees what this one earned, redeemed and sold.
      api<ApiCustomer[]>('/masters/customers').then(setCustomers).catch(() => undefined);
      loadStockItems().catch(() => undefined);
      setPrevBill({ no: doc.doc_no, time: now.toLocaleTimeString('en-IN'), amount: doc.totals.net });
      setToast({ kind: 'ok', text: `Bill ${doc.doc_no} saved — ₹ ${money(doc.totals.net)}.` });
      // Print the bill exactly as the server saved it (number, totals, points).
      // What printing follows a save comes from the Doc Designer's default print option (Global Settings is the fallback).
      const fallback = gsettings.InvPreview?.toUpperCase() === 'Y' ? 'preview' : docSettings.after_save;
      afterSavePlan('sales_invoice', fallback, andPrint).then(({ plan, option }) => {
        if (plan === 'dialog') setPrintDocNo(doc.doc_no);
        else if (plan === 'ask') confirm('Do you want to print the invoice..?').then(ok => { if (ok) setPrintDocNo(doc.doc_no); });
        else if (plan === 'direct' && option) {
          runPrint({ kind: 'sales_invoice', doc_no: doc.doc_no, option: option.name, action: 'direct' })
            .then(r => setToast({ kind: 'ok', text: r.text }))
            .catch(e => setToast({ kind: 'error', text: errorText(e) }));
        }
      });
      const given = parseInvoiceNo(doc.doc_no);   // the next preview follows the number the server gave
      setNumbering(given);
      setSeq(given.seq + 1);
      resetBill();
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  useShortcuts({
    F2: focusSearch,
    F3: toggleQuick,
    F4: () => focusField('si-party'),
    F6: () => nextPayment(1),
    'Shift+F6': () => nextPayment(-1),
    'Alt+P': () => nextPayment(1),   // F6 is kept by some browsers for the address bar
    F7: () => { void holdOrNew(); },
    F8: () => setDialog('held'),
    F9: () => focusField('si-collected'),
    F10: () => { void saveBill(true); },
    'Mod+S': () => { void saveBill(false); },
    'Alt+R': () => setDialog('reprint'),
    'Alt+C': () => setDialog('cancel'),
    'Alt+X': () => { void cancelBill(); },
    Escape: focusSearch,
  });

  const totalRows: [string, number][] = [
    ['Basic amount', totals.basic], ['Discount', totals.disc], ['CGST', totals.cgst],
    ['SGST', totals.sgst], ['Cess', totals.cess], ['Comp. cess', totals.compCess], ['Points redeemed', totals.points],
  ];
  const payRow = 'flex items-center justify-between gap-3 py-1.5';

  return (
    <div className="mx-auto flex max-w-[1440px] flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {confirmDialog}

      {/* Document header: bill no, time, counter, previous bill */}
      <div className="flex flex-wrap items-center gap-x-6 gap-y-3">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-xl font-semibold tracking-tight text-stone-900 dark:text-stone-50">Sales invoice</h1>
            <span className={badge('accent')}>{busy ? 'Saving…' : 'Draft'}</span>
          </div>
          <p className="mt-0.5 text-xs tabular-nums text-stone-500 dark:text-stone-400">
            <span className="font-mono font-medium text-stone-800 dark:text-stone-200">{billNo}</span> · {now.toLocaleDateString('en-GB')} {now.toLocaleTimeString('en-IN')} · Counter <span className="font-mono">{counter.code}</span>
          </p>
        </div>
        <div className="ml-auto flex flex-wrap items-center gap-2">
          <span className="hidden text-xs text-stone-500 lg:inline dark:text-stone-400">
            Last bill {prevBill ? <><span className="font-mono text-stone-700 dark:text-stone-300">{prevBill.no}</span> · {prevBill.time} · <span className="tabular-nums font-medium text-stone-800 dark:text-stone-200">₹ {money(prevBill.amount)}</span></> : '—'}
          </span>
          <button onClick={() => setDialog('held')} className={secondaryButton}><Undo2 size={14} />Held bills<Kbd combo="F8" /></button>
          <button onClick={() => setDialog('reprint')} className={printButton}><Printer size={14} />Re-print<Kbd combo="Alt+R" /></button>
          <button onClick={() => setDialog('cancel')} className={`${ghostButton} hover:!text-red-600 dark:hover:!text-red-400`}><Ban size={14} />Cancel invoice<Kbd combo="Alt+C" /></button>
          <button onClick={onClose} className={ghostButton}><DoorOpen size={14} />Close</button>
        </div>
      </div>

      <div className="grid grid-cols-1 items-start gap-4 xl:grid-cols-[minmax(0,1fr)_340px]">
        {/* LEFT: scan + lines */}
        <div className="flex min-w-0 flex-col gap-4">
          <div className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
            <div className="min-w-0 flex-1 basis-64">
              <label htmlFor="si-barcode" className={labelClass}>Scan or search item <span className="font-normal text-stone-400">(F2)</span></label>
              <ItemSearchBox id="si-barcode" ref={barcodeRef} value={barcode} onChange={setBarcode} onPick={pickItem}
                onEnterNoMatch={() => scan()} onKeyDown={searchKeys}
                placeholder="Barcode, item code or name, then Enter" />
            </div>
            <div className="w-20">
              <label htmlFor="si-qty" className={labelClass}>Qty</label>
              <input id="si-qty" type="number" min={1} value={scanQty} onChange={e => setScanQty(e.target.value)} onFocus={e => e.target.select()} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); if (barcode.trim()) scan(); else focusSearch(); } if (e.key === 'Escape') focusSearch(); }} className={`${field} h-10 text-right text-sm tabular-nums`} />
            </div>
            <button onClick={() => scan()} tabIndex={-1} className={`${primaryButton} h-10 px-4`}>Add</button>
            <button
              onClick={toggleQuick}
              tabIndex={-1}
              aria-expanded={quickOpen}
              aria-controls="si-quick"
              className={`${secondaryButton} h-10 ${quickOpen ? '!border-accent !bg-accent-soft !text-accent-ink' : ''}`}
            >
              {quickOpen ? <PanelLeftClose size={14} /> : <PanelLeftOpen size={14} />}Quick pick<Kbd combo="F3" />
            </button>
          </div>

          <div className={`grid grid-cols-1 gap-4 ${quickOpen ? 'lg:grid-cols-[240px_minmax(0,1fr)]' : ''}`}>
            {quickOpen && (
              <div id="si-quick" className={`${panel} flex max-h-[480px] flex-col overflow-hidden`}>
                <div className="flex items-center justify-between border-b border-line px-3 py-2.5">
                  <span className="text-xs font-semibold text-stone-900 dark:text-stone-50">Quick pick</span>
                  <button onClick={() => setQuickOpen(false)} className={`${iconButton} h-7 w-7`} aria-label="Close quick pick"><X size={14} /></button>
                </div>
                <div className="relative border-b border-line p-2">
                  <Search size={14} className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
                  <input autoFocus value={quickFilter} onChange={e => { setQuickFilter(e.target.value); setQuickAt(0); }} onKeyDown={e => {
                    if (e.key === 'Escape') { e.preventDefault(); setQuickOpen(false); focusSearch(); }
                    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); setQuickAt(a => Math.max(0, Math.min(quickItems.length - 1, a + (e.key === 'ArrowDown' ? 1 : -1)))); }
                    if (e.key === 'Enter' && quickItems[quickAt]) { e.preventDefault(); addItem(quickItems[quickAt].stock, 1); }
                  }} placeholder="Filter, ↑↓, Enter adds" aria-label="Filter items" className={`${field} pl-8`} />
                </div>
                <ul className="flex-1 overflow-auto p-1 text-xs">
                  {quickItems.map((item, qi) => (
                    <li key={item.code}>
                      <button tabIndex={-1} onClick={() => addItem(item.stock, 1)} ref={qi === quickAt ? el => el?.scrollIntoView({ block: 'nearest' }) : undefined}
                        className={`w-full rounded-lg px-2.5 py-2 text-left transition-colors hover:bg-subtle ${qi === quickAt ? 'bg-accent-soft' : ''}`}>
                        <span className="block truncate font-medium text-stone-900 dark:text-stone-50">{item.brand}</span>
                        <span className="mt-0.5 flex justify-between text-[11px] tabular-nums text-stone-500 dark:text-stone-400">
                          <span>{item.pkg} · {item.clQty} in stock</span><span className="font-medium text-stone-700 dark:text-stone-300">₹ {money(item.rate)}</span>
                        </span>
                      </button>
                    </li>
                  ))}
                  {!quickItems.length && <li className="px-3 py-6 text-center text-stone-500">No items match.</li>}
                </ul>
              </div>
            )}

            <div className={`${panel} flex min-h-[320px] flex-col overflow-hidden`}>
              <div className="flex-1 overflow-auto">
                <table className="w-full text-left text-xs">
                  <thead className={tableHead}>
                    <tr>
                      <th className="w-10 px-4 py-2.5">#</th>
                      <th className="min-w-[220px] px-3 py-2.5">Item</th>
                      <th className="px-3 py-2.5 text-right">In stock</th>
                      <th className="px-3 py-2.5 text-right">Qty</th>
                      <th className="px-3 py-2.5 text-right">Rate</th>
                      <th className="px-3 py-2.5 text-right">Disc %</th>
                      <th className="px-3 py-2.5 text-right">Disc amt</th>
                      <th className="px-3 py-2.5 text-right">Amount</th>
                      <th className="w-10 px-2 py-2.5"><span className="sr-only">Remove</span></th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-line-soft tabular-nums">
                    {lines.length === 0 && (
                      <tr><td colSpan={9} className="px-4 py-20 text-center">
                        <ScanLine size={28} className="mx-auto mb-2 text-stone-300 dark:text-stone-600" />
                        <p className="text-sm font-medium text-stone-700 dark:text-stone-200">No items on this bill</p>
                        <p className="mt-0.5 text-xs text-stone-500">Scan a barcode or type an item name and press Enter. Quick pick: F3.</p>
                      </td></tr>
                    )}
                    {lines.map((line, i) => {
                      const low = line.qty >= line.qoh;
                      return (
                        <tr key={line.id} onClick={() => setSelectedId(line.id)} aria-selected={selectedId === line.id} className={`cursor-pointer transition-colors ${selectedId === line.id ? 'bg-accent-soft' : 'hover:bg-subtle/70'}`}>
                          <td className="px-4 py-2 text-stone-400">{i + 1}</td>
                          <td className="px-3 py-2 font-medium text-stone-900 dark:text-stone-50">{line.name}</td>
                          <td className={`px-3 py-2 text-right ${low ? 'text-amber-700 dark:text-amber-400' : 'text-stone-500'}`}>{line.qoh}</td>
                          <td className="px-3 py-1.5 text-right"><input type="number" min={1} max={line.qoh} value={line.qty || ''} data-si-row={i} data-si-col="qty" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'qty')} onChange={e => updateLine(line.id, { qty: Number(e.target.value) })} onBlur={() => line.qty || updateLine(line.id, { qty: 1 })} className={`${cellInput} font-semibold`} aria-label={`Qty for ${line.name}`} /></td>
                          <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={line.rate || ''} data-si-row={i} data-si-col="rate" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'rate')} onChange={e => updateLine(line.id, { rate: Number(e.target.value) })} className={`${cellInput} w-20`} aria-label={`Rate for ${line.name}`} /></td>
                          <td className="px-3 py-1.5 text-right"><input type="number" min={0} max={100} value={line.discPct || ''} placeholder="0" data-si-row={i} data-si-col="disc" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'disc')} onChange={e => updateLine(line.id, { discPct: Number(e.target.value) })} className={cellInput} aria-label={`Discount % for ${line.name}`} /></td>
                          <td className="px-3 py-2 text-right text-stone-500">{money(discAmt(line))}</td>
                          <td className="px-3 py-2 text-right font-semibold text-stone-900 dark:text-stone-50">{money(gross(line) - discAmt(line))}</td>
                          <td className="px-2 py-2 text-center">
                            <button tabIndex={-1} onClick={e => { e.stopPropagation(); setLines(lines.filter(l => l.id !== line.id)); }} className={`${iconButton} h-7 w-7 hover:!text-red-600`} aria-label={`Remove ${line.name}`}><Trash2 size={14} /></button>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>

              {/* Line summary + line actions */}
              <div className="flex flex-wrap items-center gap-x-5 gap-y-2 border-t border-line bg-subtle/60 px-4 py-2.5 text-xs text-stone-600 dark:text-stone-300">
                <span>Items <span className="font-semibold tabular-nums text-stone-900 dark:text-stone-50">{lines.length}</span></span>
                <span>Qty <span className="font-semibold tabular-nums text-stone-900 dark:text-stone-50">{totals.qty}</span></span>
                <span>Litres <span className="font-semibold tabular-nums text-stone-900 dark:text-stone-50">{totals.litres.toFixed(3)}</span></span>
                <div className="ml-auto flex flex-wrap gap-1">
                  <span className="hidden text-[11px] text-stone-400 xl:inline">↑↓ line · + − qty · Del remove · 3* qty · F9 cash · F10 save</span>
                  <button onClick={deleteItem} disabled={!lines.length} tabIndex={-1} className={ghostButton}><Trash2 size={14} />Delete item</button>
                  <button onClick={clearItems} disabled={!lines.length} tabIndex={-1} className={ghostButton}><Eraser size={14} />Clear</button>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* RIGHT: customer, totals, payment, actions */}
        <aside aria-label="Bill summary" className="flex flex-col gap-4 xl:sticky xl:top-0">
          <div className={`${panel} grid grid-cols-2 gap-3 p-4`}>
            <div className="col-span-2 sm:col-span-1">
              <label htmlFor="si-party" className={labelClass}>Customer <span className="font-normal text-stone-400">(F4)</span></label>
              <input id="si-party" list="si-parties" value={party} onChange={e => setParty(e.target.value)} onKeyDown={e => { if (e.key === 'Enter' || e.key === 'Escape') { e.preventDefault(); focusSearch(); } }} className={field} placeholder="Walk-in customer" />
              <datalist id="si-parties">{customers.map(c => <option key={c.code} value={c.name} />)}</datalist>
            </div>
            {customer && customer.points > 0 && (
              <div className="col-span-2 flex items-center gap-2 rounded-lg bg-accent-soft px-3 py-2 text-xs text-accent-ink">
                <Gift size={14} className="shrink-0" />
                <span className="flex-1"><span className="font-semibold tabular-nums">{customer.points}</span> loyalty points</span>
                <input type="number" min={0} max={customer.points} value={redeem} onChange={e => setRedeem(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusField('si-collected'); } }} placeholder="Redeem" className={`${field} w-24 py-1 text-right tabular-nums`} aria-label="Points to redeem" />
              </div>
            )}
            <div className="col-span-2 sm:col-span-1">
              <label htmlFor="si-salesperson" className={labelClass}>Sales person</label>
              <select id="si-salesperson" value={salesPerson} onChange={e => setSalesPerson(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusSearch(); } }} className={field}>
                {!salesPersons.length && <option value="">No sales persons (Sales Person Master)</option>}
                {salesPersons.map(p => <option key={p}>{p}</option>)}
              </select>
            </div>
          </div>

          <div className={`${panel} space-y-3 p-4 text-xs`}>
            <div className="rounded-lg bg-subtle px-3 py-3">
              <div className="flex items-baseline justify-between gap-2">
                <span className="text-xs font-medium text-stone-500 dark:text-stone-400">Invoice value</span>
                <span className="text-[11px] tabular-nums text-stone-500 dark:text-stone-400">{lines.length} items · {totals.qty} qty</span>
              </div>
              <div className="mt-0.5 text-3xl font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">₹ {money(totals.invoice)}</div>
              <div className="mt-1 text-[11px] italic leading-snug text-stone-500 dark:text-stone-400">{numberToWords(totals.invoice)}</div>
            </div>
            <details className="group rounded-lg border border-line">
              <summary className="flex cursor-pointer list-none items-center justify-between gap-3 px-3 py-2 text-stone-600 hover:bg-subtle/70 dark:text-stone-300 [&::-webkit-details-marker]:hidden">
                <span>Amount breakdown</span>
                <span className="flex items-center gap-1.5 tabular-nums text-stone-900 dark:text-stone-50">{money(totals.total)}<ChevronDown size={14} className="text-stone-400 transition-transform group-open:rotate-180" /></span>
              </summary>
              <dl className="space-y-1.5 border-t border-line px-3 py-2.5 tabular-nums">
                {totalRows.map(([label, value]) => (
                  <div key={label} className="flex justify-between gap-3">
                    <dt className="text-stone-500 dark:text-stone-400">{label}</dt>
                    <dd className={value ? 'text-stone-800 dark:text-stone-200' : 'text-stone-400'}>{money(value)}</dd>
                  </div>
                ))}
                <div className="flex justify-between gap-3 border-t border-line pt-1.5 font-medium">
                  <dt className="text-stone-700 dark:text-stone-200">Total</dt>
                  <dd className="text-stone-900 dark:text-stone-50">{money(totals.total)}</dd>
                </div>
              </dl>
            </details>
            <div className={payRow}>
              <label htmlFor="si-writeoff" className="whitespace-nowrap text-stone-500 dark:text-stone-400">Write-off</label>
              <input id="si-writeoff" type="number" min={0} value={writeOff} onFocus={e => e.target.select()} onChange={e => setWriteOff(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusField('si-collected'); } }} className={`${field} w-28 py-1 text-right tabular-nums`} />
            </div>
            <div role="radiogroup" aria-label="Payment type (F6 or Alt+P)" onKeyDown={e => {
              if (['ArrowRight', 'ArrowDown', 'ArrowLeft', 'ArrowUp'].includes(e.key)) {
                e.preventDefault();
                const next = PAYMENT_TYPES[(PAYMENT_TYPES.indexOf(paymentType) + (e.key === 'ArrowRight' || e.key === 'ArrowDown' ? 1 : -1) + PAYMENT_TYPES.length) % PAYMENT_TYPES.length];
                setPaymentType(next);
                document.getElementById(`si-pay-${next}`)?.focus();
              }
            }} className="grid grid-cols-2 gap-1 rounded-lg bg-muted p-1 sm:grid-cols-4 xl:grid-cols-2 2xl:grid-cols-4">
              {PAYMENT_TYPES.map(p => (
                <button
                  key={p}
                  id={`si-pay-${p}`}
                  role="radio"
                  tabIndex={paymentType === p ? 0 : -1}
                  aria-checked={paymentType === p}
                  onClick={() => setPaymentType(p)}
                  className={`rounded-md px-2 py-1.5 text-[11px] font-semibold transition-colors ${paymentType === p ? 'bg-surface text-stone-900 shadow-card dark:text-stone-50' : 'text-stone-600 hover:text-stone-900 dark:text-stone-300 dark:hover:text-stone-50'}`}
                >{p}</button>
              ))}
            </div>
            <dl className="tabular-nums">
              <div className={payRow}>
                <dt className="text-stone-500 dark:text-stone-400">Card</dt>
                <dd>{paymentType === 'SPLIT'
                  ? <input type="number" min={0} value={cardInput} onChange={e => setCardInput(e.target.value)} className={`${field} w-28 py-1 text-right tabular-nums`} aria-label="Card amount" />
                  : <span className="text-stone-800 dark:text-stone-200">{money(totals.card)}</span>}</dd>
              </div>
              <div className={payRow}>
                <dt className="text-stone-500 dark:text-stone-400">UPI / other</dt>
                <dd>{paymentType === 'SPLIT'
                  ? <input type="number" min={0} value={otherPay} onChange={e => setOtherPay(e.target.value)} className={`${field} w-28 py-1 text-right tabular-nums`} aria-label="Other pay amount" />
                  : <span className="text-stone-800 dark:text-stone-200">{money(totals.other)}</span>}</dd>
              </div>
              <div className={payRow}>
                <dt className="font-medium text-stone-700 dark:text-stone-200">Cash</dt>
                <dd className="font-semibold text-stone-900 dark:text-stone-50">{money(totals.cash)}</dd>
              </div>
            </dl>
            <div>
              <label htmlFor="si-collected" className={labelClass}>Cash tendered <span className="font-normal text-stone-400">(F9 · Enter saves)</span></label>
              <div className="flex items-center gap-2">
                <input id="si-collected" type="number" min={0} value={collected} onFocus={e => e.target.select()} onChange={e => setCollected(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); void saveBill(); } if (e.key === 'Escape') focusSearch(); }} className={`${field} flex-1 text-right text-sm tabular-nums`} placeholder="0.00" />
                <span className={`${badge(change < 0 ? 'danger' : 'success')} px-2 py-1.5 text-xs tabular-nums`}>
                  {change < 0 ? 'Short' : 'Change'} ₹ {money(Math.abs(change))}
                </span>
              </div>
            </div>
            <div className="space-y-2">
              <button
                onClick={() => saveBill(true)}
                className={`${printButton} h-11 w-full text-sm font-bold shadow-md hover:shadow-lg`}
              >
                <Printer size={16} />Save & Print · ₹ {money(totals.invoice)}<Kbd combo="F10" />
              </button>
              <button
                onClick={() => saveBill(false)}
                className={`${secondaryButton} h-9 w-full text-xs font-semibold`}
              >
                <Save size={14} />Save only (No print)<Kbd combo="Mod+S" />
              </button>
            </div>
            <div className="grid grid-cols-2 gap-2">
              <button onClick={holdOrNew} className={secondaryButton}><PauseCircle size={14} />Hold / new<Kbd combo="F7" /></button>
              <button onClick={cancelBill} className={`${secondaryButton} hover:!text-red-600 dark:hover:!text-red-400`}><XCircle size={14} />Cancel bill<Kbd combo="Alt+X" /></button>
            </div>
          </div>
        </aside>
      </div>

      {dialog === 'reprint' && <ReprintDialog initialNo={prevBill?.no ?? ''} onClose={() => setDialog(null)} />}
      {dialog === 'cancel' && (
        <CancelInvoiceDialog
          initialNo={prevBill?.no ?? ''}
          onClose={() => setDialog(null)}
          onDone={msg => { setDialog(null); setToast({ kind: 'ok', text: msg }); }}
        />
      )}
      {dialog === 'held' && <HeldBillsDialog onClose={() => setDialog(null)} onRecall={recall} />}

      {printDocNo && <PrintDialog kind="sales_invoice" docNo={printDocNo} title={`Print Sales Invoice (${printDocNo})`} onClose={() => setPrintDocNo(null)} />}
    </div>
  );
}
