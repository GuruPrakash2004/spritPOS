// Sales Invoice (POS billing, legacy frmTrnSINV), step 2 after SalesInvoiceSetup.
//
// Barcode/item entry, qty +/-, collected amount → payback/shortage, loyalty points, total litres,
// Hold / New Bill / Cancel / Re-Print. Saves with POST /transactions/sales-invoice, which reduces
// stock on the backend (store.move_stock). Held bills get their own HOLD- series.
// Items come from the live catalogue (data/stockData.js; a line's `id` is its STOCK_ITEMS index), and
// the bill total is rounded to the rupee exactly as the backend rounds it.
// Global Settings used here: QTYSELINV (Y: Enter after a scan moves to Qty; N: the item is added at once)
// and InvPreview (Y: the print preview opens after every saved bill). Transaction/Print Settings for Sales Invoice
// (lib/docSettings.js): after save none / ask "Do you want to print the invoice..?" / preview, and the default
// print format.
//
// Keyboard first (the counter rarely uses a mouse; the full list is in the Shortcuts sheet, `?`):
//   F2 item search · F3 quick pick · Alt+T load template · F4 customer · F6 or Alt+P next payment type · F7 hold / new · F8 held bills
//   F9 cash tendered · F10 save & print · ⌘/Ctrl+S save only
//   Alt+R re-print · Alt+C cancel a saved invoice
//   In the empty item search: ↑/↓ choose a line, + / − change its qty, Delete removes it, Enter edits its qty,
//   and "3*" sets the qty for the next item. In a line: ↑/↓ move between lines, Enter or Esc back to the search.
//   Waiting popups: Enter saves now, Esc cancels.

import { Fragment, useEffect, useMemo, useRef, useState, } from 'react';
import ColumnPicker from '../common/ColumnPicker';
import QuickSelectionPanel from './QuickSelectionPanel';
import CashCountDialog from './CashCountDialog';
import { ChevronDown, ChevronUp, Columns3, Trash2, Undo2, Eraser, DoorOpen, PanelLeftOpen, PanelLeftClose, Printer, Ban, Gift, ScanLine } from 'lucide-react';
import Kbd from '../common/Kbd';
import { useColumnTone } from '../../hooks/useColumnTone';
import { useGridWidths } from '../../hooks/useGridWidths';
import { GRID_OUTLINE } from '../../hooks/useResizableTable';
import { cn } from '../../lib/utils';
import ItemSearchBox, { } from './ItemSearchBox';
import ItemPickerModal from './ItemPickerModal';
import { focusField, useShortcuts } from '../../lib/keys';
import { useConfirm } from '../common/ConfirmDialog';
import {  badge, field as baseField, fieldTone, ghostButton, iconButton, label as labelClass, panel, primaryButton, secondaryButton, tableHead , printButton } from '../../lib/styles';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import BillExtras from './BillExtras';
import CustomerPicker from './CustomerPicker';
import { useBillExtras } from './useBillExtras';
import { deleteSalesInvoice, errorText, getCustomers, getNextNumber, getPartyRecords, getSalesPersons, getSettingValues, listSalesInvoices, newIdempotencyKey, saveSalesInvoice, todayIso, updateSalesInvoice, } from '../../lib/api';
import { CancelInvoiceDialog, HeldBillsDialog, ReprintDialog } from './SalesInvoiceDialogs';
import { STOCK_ITEMS, lineName, litresPerBottle, loadStockItems, stockIndex, useStockItems } from '../../data/stockData';
import { PAYMENT_TYPES, } from '../../data/salesCounters';
import PrintDialog from '../print/PrintDialog';
import WithHistory from './DocHistoryPanel';
import { afterSavePlan, runPrint } from '../../lib/printing';
import { useDocSettings } from '../../lib/docSettings';
import { useScreenFields } from '../../hooks/useScreenFields';
import { applyGridDefinition } from '../../lib/billGrid';
import { formulaAmounts } from '../../lib/billMaths';
import { applyDefinition, ruleHolds, ruleValues } from '../../lib/designerFields';



const toLine = (stock, qty, discPct = 0) => {
  const item = STOCK_ITEMS[stock];
  return { id: stock, name: lineName(item), qoh: item.clQty, qty, rate: item.rate, discPct, cgstPct: item.cgstPct ?? 0, sgstPct: item.sgstPct ?? 0 };
};

const money = (n) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
// A line's amount / CGST / SGST come from the designer's formulas when it has them (`fAmount`, `fCgst`, `fSgst`, set by `priced` below), else from qty × MRP and the item's GST %.
const gross = (l) => l.fAmount ?? l.qty * l.rate;
const discAmt = (l) => (gross(l) * l.discPct) / 100;
// GST per line is worked out on the line's amount as the backend does (Item Master's CGST % / SGST %); the item has no Cess %
// in Item Master, so the Cess columns of the legacy grid show 0.00.
const round2 = (n) => Math.round(n * 100) / 100;
const cgstAmt = (l) => l.fCgst ?? round2(round2(gross(l)) * (l.cgstPct ?? 0) / 100);
const sgstAmt = (l) => l.fSgst ?? round2(round2(gross(l)) * (l.sgstPct ?? 0) / 100);
const itemAmt = (l) => gross(l) - discAmt(l) + cgstAmt(l) + sgstAmt(l);
// Splits "26/c10-0001234" into "26/c10-" + 1234 so later bills keep the same prefix and zero padding.
const parseInvoiceNo = (invoiceNo) => {
  const match = invoiceNo.match(/^(.*?)(\d+)$/);
  return match ? { head: match[1], seq: Number(match[2]), width: match[2].length } : { head: `${invoiceNo}-`, seq: 1, width: 1 };
};

const field = baseField;
const payRow = 'flex items-center justify-between gap-3 py-1.5';
const cellInput = 'w-full min-w-0 rounded-md border border-transparent bg-transparent px-1.5 py-1 text-right tabular-nums hover:border-line-strong focus:border-accent focus:bg-surface focus:outline-none focus:ring-2 focus:ring-accent/20';

// The bill grid's columns in the desktop's order (id, caption, field-tone code, starting width in px). The user can drag
// a header's right edge to resize; the widths are remembered on this computer (hooks/useGridWidths.js).
const GRID_COLS = [
  ['sl', 'Sl No', null, 64], ['item', 'Item', null, 240], ['qoh', 'QOH', null, 58], ['qty', 'Qty', 'SINVQTY', 70],
  ['rate', 'MRP', 'SINVRATE', 84], ['discPct', 'Disc.%', 'DISCPER', 66], ['disc', 'Disc.Amt', 'DISCAMT', 80],
  ['amount', 'Amount', 'SINVAMT', 92], ['total', 'Total Item Amt', null, 112], ['cgstPct', 'CGST%', 'CGSTPER', 70],
  ['cgst', 'CGST Amt', 'CGSTAMT', 86], ['sgstPct', 'SGST%', 'SGSTPER', 70], ['sgst', 'SGST Amt', 'SGSTAMT', 86],
  ['cessPct', 'Cess%', 'CESSPER', 66], ['cess', 'Cess Amt', 'CESSAMT', 80], ['compCess', 'Comp.Cess', 'COMPCESSAMT', 86],
  ['itemAmt', 'Item Amt', 'ITEMAMT', 104],
];
const GRID_DEFAULTS = Object.fromEntries(GRID_COLS.map(([id, , , w]) => [id, w]));
// Columns the bill can't work without (the designer may rename or reorder them, never hide them); Qty can't be locked either.
const GRID_KEEP = new Set(['sl', 'item', 'qty', 'rate', 'lock:qty']);
// What a column sorts by (the line's own figure); Sl No sorts as the order the items were added.
const SORT_VALUE = {
  item: l => l.name, qoh: l => l.qoh, qty: l => l.qty, rate: l => l.rate, discPct: l => l.discPct, disc: discAmt, amount: gross,
  total: l => gross(l) - discAmt(l), cgstPct: l => l.cgstPct ?? 0, cgst: cgstAmt, sgstPct: l => l.sgstPct ?? 0, sgst: sgstAmt, itemAmt,
};
const COLS_PREF = 'vlqms_si_cols';   // the clerk's own column choice for the bill grid, kept on this computer
const readColsPref = () => { try { return JSON.parse(localStorage.getItem(COLS_PREF) ?? 'null'); } catch { return null; } };
// The header boxes drawn here (the setup step has counter, payment mode and customer); the designer's captions, mandatory,
// read-only and hidden flags are laid over them (Doc Designer rows of TrnHdrSINV).
// Header columns the desktop's bill screen does not show (Amount In Words, Remarks).
const NOT_ON_BILL = /amount\s*_?in\s*_?words|amtinwords|remarks?|rmks/i;
const HEADER_TYPED = [{ key: 'customer', label: 'Customer' }, { key: 'salesperson', label: 'Sales person' }];

export default function SalesInvoice({ setup, onClose }) {
  const { counter } = setup;
  // The bill number preview (the backend gives the real number on save); null until it is known.
  const [numbering, setNumbering] = useState(() => (setup.invoiceNo ? parseInvoiceNo(setup.invoiceNo) : null));
  const [now, setNow] = useState(() => new Date());
  const [seq, setSeq] = useState(numbering?.seq ?? 1);
  const [prevBill, setPrevBill] = useState(null);
  // The previous bill comes from the database: the latest saved bill of this counter in the last week (any counter's when this one has none), so
  // a bill made on another PC or in the Windows vLQMS shows too. A bill saved on this screen replaces it (below).
  useEffect(() => {
    let live = true;
    const from = new Date(Date.now() - 7 * 86400000 - new Date().getTimezoneOffset() * 60000).toISOString().slice(0, 10);
    listSalesInvoices('SAVED', from, todayIso()).then(list => {
      if (!live || !list.length) return;
      const mine = list.filter(b => b.counter === setup.counter.code);
      const latest = (mine.length ? mine : list).reduce((a, b) => (parseInvoiceNo(b.doc_no).seq > parseInvoiceNo(a.doc_no).seq ? b : a));
      setPrevBill(cur => cur ?? { no: latest.doc_no, time: latest.time ?? '', amount: latest.totals?.net ?? latest.total ?? 0 });
    }).catch(() => undefined);
    return () => { live = false; };
  }, [setup.counter.code]);
  const [party, setParty] = useState(setup.customer);
  useStockItems();
  const [salesPersons, setSalesPersons] = useState([]);
  const [salesPerson, setSalesPerson] = useState(setup.salesPerson ?? '');
  const [lines, setLines] = useState([]);
  // Draft auto-save (the desktop's AutoSaveGrid / RestoreData): the unsaved bill is kept on this PC as items are added, and offered back
  // after a refresh or a crash. A saved, cancelled or cleared bill removes it.
  const draftKey = `vlqms_si_draft_${setup.counter?.code ?? ''}`;
  const [draft, setDraft] = useState(() => { try { return JSON.parse(localStorage.getItem(draftKey) ?? 'null'); } catch { return null; } });
  const [selectedId, setSelectedId] = useState(null);
  const [barcode, setBarcode] = useState('');
  const [scanQty, setScanQty] = useState('1');
  const [pickerOpen, setPickerOpen] = useState(false);   // the "Please Select Item" window
  const pickFromList = (item) => {   // Ok in that window: the code goes into the Scan Barcode box and the cursor to Qty (desktop)
    setPickerOpen(false);
    setBarcode(item.code);
    setScanQty('1');
    focusField('si-qty');
  };
  // The customer / sales person box can be folded away (remembered on this computer); F4 opens it again.
  const [partyOpen, setPartyOpen] = useState(() => { try { return localStorage.getItem('vlqms_si_party_open') !== '0'; } catch { return true; } });
  const togglePartyBox = () => setPartyOpen(o => { try { localStorage.setItem('vlqms_si_party_open', o ? '0' : '1'); } catch { /* not remembered */ } return !o; });
  const [findLine, setFindLine] = useState('');   // narrows the bill grid to lines matching this text
  const { widths, startResize, reset: resetWidths } = useGridWidths('sales-invoice-v2', GRID_DEFAULTS);
  const [quickOpen, setQuickOpen] = useState(false);
  const [tplOpen, setTplOpen] = useState(false);   // the desktop's F6 Load Template (MstItemTemp), Alt+T here because F6 changes the payment type
  const [writeOff, setWriteOff] = useState('0');
  const [paymentType, setPaymentType] = useState(setup.paymentType);
  const [otherPay, setOtherPay] = useState('0');
  const [cardInput, setCardInput] = useState('0');
  const [collected, setCollected] = useState('');
  const [denoms, setDenoms] = useState({});   // the cash count { '500': 2 } saved with the bill
  const [countOpen, setCountOpen] = useState(false);
  const [customers, setCustomers] = useState([]);
  const [redeem, setRedeem] = useState('');
  const [dialog, setDialog] = useState(null);
  const [printFormat, setPrintFormat] = useState('');   // the Print Format picked in Re-Print Bill
  // A saved bill opened from the List to change (PUT keeps its number): its number, date and counter, and how many of each item it already
  // holds (that stock is the bill's own, so it counts as available while the bill is edited).
  const [editing, setEditing] = useState(null);
  const [editQty, setEditQty] = useState({});
  const [toast, setToast] = useState(null);
  const [printDocNo, setPrintDocNo] = useState(null);
  const barcodeRef = useRef(null);
  // From the moment a save/hold goes to the server until it answers, the bill can't change or be sent again,
  // so a bottle scanned straight after F10 + Enter never lands on the bill being saved.
  const sending = useRef(false);
  const [busy, setBusy] = useState(false);
  const locked = () => {
    if (sending.current) setToast({ kind: 'error', text: 'Saving the last bill. Scan again in a moment.' });
    return sending.current;
  };
  const send = (body, key) => {
    sending.current = true;
    setBusy(true);
    return saveSalesInvoice(body, key)
      .finally(() => { sending.current = false; setBusy(false); });
  };
  // One key per bill: pressing Save twice (or a retry) can't make two bills. Renewed for each new bill.
  const saveKey = useRef(newIdempotencyKey());
  const { confirm, dialog: confirmDialog } = useConfirm();
  const extrasBill = useBillExtras('TrnHdrSINV', 'si', NOT_ON_BILL);
  const tone = useColumnTone('TrnHdrSINV');   // red = mandatory, blue = read-only, from the Doc Designer
  const live = useScreenFields('table:TrnHdrSINV');
  const cols = useMemo(() => applyGridDefinition(GRID_COLS, live.screen, GRID_KEEP), [live.screen]);   // the grid as the designer lays it out
  const colOf = (id) => cols.find(c => c.id === id);
  // Select Columns (as the desktop's Column Extend): the clerk's own order and choice laid over the designer's grid; the columns the bill
  // can't work without always stay. Sort: click a header (ascending, descending, off); it only changes how the lines are shown.
  const [colPref, setColPref] = useState(readColsPref);
  const [pickCols, setPickCols] = useState(false);
  const [sort, setSort] = useState({ id: '', dir: 1 });
  const shownCols = useMemo(() => {
    if (!colPref) return cols;
    const byId = new Map(cols.map(c => [c.id, c]));
    const ordered = [...(colPref.order ?? []).filter(id => byId.has(id)), ...cols.map(c => c.id).filter(id => !(colPref.order ?? []).includes(id))];
    return ordered.filter(id => (colPref.visible ?? []).includes(id) || GRID_KEEP.has(id)).map(id => byId.get(id));
  }, [cols, colPref]);
  const applyCols = ({ order, visible }) => {
    const next = { order, visible: [...new Set([...visible, ...order.filter(id => GRID_KEEP.has(id))])] };
    setColPref(next);
    try { localStorage.setItem(COLS_PREF, JSON.stringify(next)); } catch { /* not remembered */ }
    setPickCols(false);
  };
  // Hold the left mouse button on the grid and drag to move it sideways (and up / down); boxes, buttons and the resize edges keep their own mouse use.
  const panGrid = (e) => {
    if (e.button !== 0 || e.target.closest('input, button, select, textarea, [role="separator"]')) return;
    const box = e.currentTarget;
    const from = { x: e.clientX, y: e.clientY, left: box.scrollLeft, top: box.scrollTop };
    const move = (m) => { box.scrollLeft = from.left - (m.clientX - from.x); box.scrollTop = from.top - (m.clientY - from.y); };
    const up = () => { window.removeEventListener('mousemove', move); window.removeEventListener('mouseup', up); };
    window.addEventListener('mousemove', move);
    window.addEventListener('mouseup', up);
  };
  const sortBy = (id) => setSort(s => (s.id === id ? (s.dir === 1 ? { id, dir: -1 } : { id: '', dir: 1 }) : { id, dir: 1 }));
  const head = useMemo(() => applyDefinition(HEADER_TYPED, live.screen), [live.screen]);
  // The lines with the designer's formula figures (line amount, CGST, SGST) laid over them; the screen's totals and cells read these.
  const priced = useMemo(() => {
    const asked = lines.map(l => ({ item_code: STOCK_ITEMS[l.id].code, qty: l.qty, rate: l.rate, cases: 0, bottles: l.qty }));
    const itemOf = (code) => { const it = STOCK_ITEMS[stockIndex(code)]; return it && { cgst_per: it.cgstPct, sgst_per: it.sgstPct, mrp: it.rate, bottles_per_case: it.bottlesPerCase }; };
    return formulaAmounts('sales_invoice', live.screen, asked, itemOf).map((f, i) => ({ ...lines[i], fAmount: f.amount, fCgst: f.cgst, fSgst: f.sgst }));
  }, [lines, live.screen]);
  // The lines in the order the grid shows them: the "Find in bill" filter, then the chosen sort. Each entry keeps the line's own index.
  const displayed = useMemo(() => {
    const q = findLine.trim().toLowerCase();
    let list = priced.map((line, i) => ({ line, i })).filter(({ line }) => !q || `${line.name} ${STOCK_ITEMS[line.id].code}`.toLowerCase().includes(q));
    const by = SORT_VALUE[sort.id];
    if (by) list = [...list].sort((a, b) => {
      const x = by(a.line), y = by(b.line);
      return (typeof x === 'string' ? x.localeCompare(y, undefined, { sensitivity: 'base' }) : (x ?? 0) - (y ?? 0)) * sort.dir;
    });
    else if (sort.id === 'sl' && sort.dir === -1) list = [...list].reverse();
    return list;
  }, [priced, findLine, sort]);
  const headDef = (key) => {
    const f = head.find(h => h.key === key) ?? { label: key };
    const values = ruleValues({ customer: party, salesperson: salesPerson });
    return { label: f.label, required: !!f.required || !!(f.mandatoryWhen && ruleHolds(f.mandatoryWhen, values)), hidden: !!f.designerHidden || !!(f.hideWhen && ruleHolds(f.hideWhen, values)), locked: !!f.lockedAlways || !!(f.readonlyWhen && ruleHolds(f.readonlyWhen, values)) };
  };
  // Global Settings values (GET /settings/values); until they load, a scan adds the item at once.
  const [gsettings, setGsettings] = useState({});
  // AllowNegative = Y (Global Settings, the desktop's setting): bills may take an item below zero stock, so the stock caps are off.
  const negativeOk = String(gsettings.AllowNegative ?? '').toUpperCase() === 'Y';
  const qtyAfterScan = gsettings.QTYSELINV?.toUpperCase() === 'Y';
  const docSettings = useDocSettings('sales_invoice');

  useEffect(() => {
    const timer = setInterval(() => setNow(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  // Customers come from Customer Master so loyalty points are current; salespeople from the backend.
  useEffect(() => {
    getCustomers().then(setCustomers).catch(e => toastStore.add('error', errorText(e)));
    // The Global Settings are read again from the database when the window gets the focus back, so a change made in the Windows vLQMS applies.
    const loadSettings = () => getSettingValues().then(v => { if (v && typeof v === 'object') setGsettings(v); }).catch(() => undefined);
    loadSettings();
    window.addEventListener('focus', loadSettings);
    getSalesPersons()
      .then(list => { setSalesPersons(list); setSalesPerson(p => (list.includes(p) ? p : list[0] ?? '')); })
      .catch(e => toastStore.add('error', errorText(e)));
    return () => window.removeEventListener('focus', loadSettings);
  }, []);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  // Opened without a previewed number (e.g. straight from the sidebar): ask the backend for this counter's next one.
  useEffect(() => {
    if (setup.invoiceNo || !counter.code) return;
    getNextNumber('sales_invoice', counter.code)
      .then(n => { const p = parseInvoiceNo(n.doc_no); setNumbering(p); setSeq(p.seq); })
      .catch(() => undefined);
  }, [setup.invoiceNo, counter.code]);

  const billNo = numbering ? `${numbering.head}${String(seq).padStart(numbering.width, '0')}` : '—';

  const totals = useMemo(() => {
    const basic = priced.reduce((acc, l) => acc + gross(l), 0);
    const disc = priced.reduce((acc, l) => acc + discAmt(l), 0);
    // GST comes from each item's CGST % / SGST % in Item Master (zero for liquor, whose excise is built into the MRP), as
    // the backend prices the bill; Item Master has no cess.
    const cgst = priced.reduce((acc, l) => acc + cgstAmt(l), 0);
    const sgst = priced.reduce((acc, l) => acc + sgstAmt(l), 0);
    const cess = 0, compCess = 0;
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
  }, [lines, priced, writeOff, redeem, paymentType, cardInput, otherPay]);

  // Beer / Wine / IML litres on the bill against the shop's per-bill limits (Global Settings AllowBeerInLtr, AllowWineInLtr, AllowIMLinLtr);
  // split billing (SPLITINV = Y) lifts them, as in the desktop. A limit the shop has no setting for is not applied.
  const liqLimits = useMemo(() => {
    const sums = { BEER: 0, WINE: 0, IML: 0 };
    for (const l of lines) {
      const it = STOCK_ITEMS[l.id];
      const kind = String(it?.cat ?? '').trim().toUpperCase();
      if (kind in sums) sums[kind] += l.qty * litresPerBottle(it.pkg);
    }
    const split = String(gsettings.SPLITINV ?? '').toUpperCase() === 'Y';
    return [['BEER', 'Beer', 'AllowBeerInLtr'], ['WINE', 'Wine', 'AllowWineInLtr'], ['IML', 'IML', 'AllowIMLinLtr']]
      .filter(([k]) => sums[k] > 0)
      .map(([k, label, code]) => {
        const limit = gsettings[code] === undefined ? null : Number(gsettings[code]);
        return { label, litres: Math.round(sums[k] * 1000) / 1000, limit, over: !split && limit !== null && sums[k] > limit };
      });
  }, [lines, gsettings]);

  const customer = customers.find(c => c.name.toLowerCase() === party.trim().toLowerCase() || c.code.toLowerCase() === party.trim().toLowerCase());
  // The alert messages kept on the customer (MstPartyAlertMessage) are shown when the customer is picked, as vLQMS.exe does.
  const alertFor = useRef('');
  useEffect(() => {
    const code = customer?.code;
    if (!code || code === 'CASH' || alertFor.current === code) return;
    alertFor.current = code;
    getPartyRecords('customers', code, 'alerts').then(r => {
      const text = r.rows.map(x => x.Message).filter(Boolean).join(' / ');
      if (text) setToast({ kind: 'error', text: `Alert for ${customer.name}: ${text}` });
    }).catch(() => undefined);
  }, [customer?.code]);   // eslint-disable-line react-hooks/exhaustive-deps

  // What the cash tendered is measured against: the cash part of the bill, or the whole bill when none of it is cash.
  const due = totals.cash > 0 ? totals.cash : totals.invoice;
  const change = collected === '' ? 0 : (Number(collected) || 0) - due;

  // setQty: the quantity typed replaces the one already on the bill's line (desktop behaviour) instead of being added to it.
  const addItem = (stock, qty, current = lines, setQty = false) => {
    if (locked()) return current;
    const item = STOCK_ITEMS[stock];
    const existing = current.find(l => l.id === stock);
    const nextQty = setQty ? qty : (existing?.qty ?? 0) + qty;
    const available = item.clQty + (editQty[stock] ?? 0);
    if (!negativeOk && nextQty > available) {
      setToast({ kind: 'error', text: `Only ${available} in stock for ${item.brand} ${item.pkg}.` });
      return current;
    }
    const next = existing
      ? current.map(l => (l.id === stock ? { ...l, qty: nextQty } : l))
      : [...current, toLine(stock, qty)];
    setLines(next);
    setSelectedId(stock);
    return next;
  };

  // The item the scan box holds right now (its code is typed, scanned or picked from the list): its name shows beside OK until it is added.
  const pickedItem = barcode.trim() ? STOCK_ITEMS.find(i => i.active && i.code.toLowerCase() === barcode.trim().toLowerCase()) : null;
  const pickedName = pickedItem ? `${pickedItem.brand} ${pickedItem.name}`.trim() : '';

  const scan = (code) => {
    if (locked()) return;
    const term = (code ?? barcode).trim().toLowerCase();
    const qty = Math.floor(Number(scanQty));
    if (!term) {
      // Nothing in the scan box: the quantity typed in Qty is the new quantity of the selected line (click a line, type the quantity, Enter).
      const line = lines.find(l => l.id === selectedId);
      if (!line) return;
      if (!qty || qty < 1) return setToast({ kind: 'error', text: 'Enter a quantity of at least 1.' });
      if (!negativeOk && qty > line.qoh) return setToast({ kind: 'error', text: `Only ${line.qoh} in stock for ${line.name}.` });
      updateLine(line.id, { qty });
      setScanQty('1');
      focusSearch();
      return;
    }
    if (!qty || qty < 1) return setToast({ kind: 'error', text: 'Enter a quantity of at least 1.' });
    // A QR code that carries several items, "CODE,QTY;CODE,QTY" (the desktop's MultiItemsFromQRCode): each goes onto the bill.
    if (/;/.test(term) || /^[^,;\s]+,\s*\d+$/.test(term)) {
      let cur = lines, added = 0;
      for (const part of (code ?? barcode).split(';')) {
        const [c, q] = part.split(',').map(x => x.trim());
        if (!c) continue;
        const at = STOCK_ITEMS.findIndex(i => i.active && (i.code.toLowerCase() === c.toLowerCase() || (i.aliases ?? []).some(a => String(a).toLowerCase() === c.toLowerCase())));
        if (at === -1) return setToast({ kind: 'error', text: `${c} Item not found!!` });
        cur = addItem(at, Math.max(1, Math.floor(Number(q)) || 1), cur);
        added += 1;
      }
      setBarcode('');
      if (added) setToast({ kind: 'ok', text: `${added} item${added === 1 ? '' : 's'} added from the code.` });
      return;
    }
    // Exact item code first (barcode scanners send the code), then a brand/name match.
    const exact = STOCK_ITEMS.findIndex(i => i.active && (i.code.toLowerCase() === term || (i.hotKey && i.hotKey.toLowerCase() === term) || (i.shortName && i.shortName.toLowerCase() === term) || (i.aliases ?? []).some(a => String(a).toLowerCase() === term)));
    const stock = exact !== -1 ? exact : STOCK_ITEMS.findIndex(i => i.active && `${i.brand} ${i.name}`.toLowerCase().includes(term));
    if (stock === -1) return setToast({ kind: 'error', text: `No item found for "${code ?? barcode}".` });
    // A quantity typed on purpose (not the default 1) for an item already on the bill sets that line's quantity.
    addItem(stock, qty, lines, qty !== 1 && lines.some(l => l.id === stock));
    setBarcode('');
    setScanQty('1');
    barcodeRef.current?.focus();
  };

  const updateLine = (id, patch) => {
    if (sending.current) return;
    setLines(cur => cur.map(l => {
    if (l.id !== id) return l;
    const next = { ...l, ...patch };
    next.qty = Math.max(0, Math.min(negativeOk ? 1e6 : next.qoh, Math.floor(next.qty) || 0));
    next.rate = Math.max(0, next.rate || 0);
    next.discPct = Math.max(0, Math.min(100, next.discPct || 0));
    return next;
  }));
  };

  // A picked suggestion: straight onto the bill, or first to Qty when QTYSELINV is Y.
  const pickItem = (item) => {
    if (qtyAfterScan) {
      setBarcode(item.code);
      focusField('si-qty');
    } else {
      scan(item.code);
    }
  };

  useEffect(() => {
    if (!lines.length || editing) return;
    try { localStorage.setItem(draftKey, JSON.stringify(lines.map(l => ({ code: STOCK_ITEMS[l.id]?.code, qty: l.qty, rate: l.rate })))); } catch { /* not kept */ }
  }, [lines, editing, draftKey]);
  const restoreDraft = () => {
    let cur = lines;
    for (const d of draft ?? []) {
      const stock = stockIndex(d.code);
      if (stock !== -1) cur = addItem(stock, d.qty, cur);
    }
    setLines(cur.map(l => { const d = draft.find(x => x.code === STOCK_ITEMS[l.id]?.code); return d ? { ...l, rate: d.rate } : l; }));
    setDraft(null);
    focusSearch();
  };
  const discardDraft = () => { try { localStorage.removeItem(draftKey); } catch { /* not kept */ } setDraft(null); };

  const selectedIndex = lines.findIndex(l => l.id === selectedId);
  const focusSearch = () => focusField('si-barcode');
  const focusLineCell = (row, col) => {
    const el = document.querySelector(`[data-si-row="${row}"][data-si-col="${col}"]`);
    el?.focus();
    el?.select();
  };

  // Keys in the empty item search work on the selected line (or the last one).
  const searchKeys = (e) => {
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
      if (e.key === '+' && !negativeOk && line.qty + 1 > line.qoh) return setToast({ kind: 'error', text: `Only ${line.qoh} in stock for ${line.name}.` });
      if (e.key === '-' && line.qty <= 1) return;
      updateLine(line.id, { qty: line.qty + (e.key === '+' ? 1 : -1) });
      setSelectedId(line.id);
    } else if (e.key === 'Delete') {
      e.preventDefault();
      void removeLine(line.id);
    } else if (e.key === 'Enter') {
      focusLineCell(at, 'qty');
    }
  };

  // Keys inside a line's Qty / MRP / Disc % box.
  const cellKeys = (row, col) => (e) => {
    if (e.key === 'Enter' || e.key === 'Escape') {
      e.preventDefault();
      e.stopPropagation();
      if (!lines[row].qty) updateLine(lines[row].id, { qty: 1 });
      focusSearch();
    } else if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
      e.preventDefault();
      const at = displayed.findIndex(d => d.i === row);   // up / down follow the lines as shown (sorted or filtered)
      const next = displayed[at + (e.key === 'ArrowDown' ? 1 : -1)]?.i;
      if (next !== undefined) { setSelectedId(lines[next].id); focusLineCell(next, col); }
    } else if (col === 'qty' && (e.key === '+' || e.key === '-')) {   // same + / − as in the search box
      e.preventDefault();
      const line = lines[row];
      if (e.key === '+' && !negativeOk && line.qty + 1 > line.qoh) return setToast({ kind: 'error', text: `Only ${line.qoh} in stock for ${line.name}.` });
      if (e.key === '-' && line.qty <= 1) return;
      updateLine(line.id, { qty: line.qty + (e.key === '+' ? 1 : -1) });
    }
  };

  // The payment mode (and what hangs on it) opens up only once items are on the bill; the amounts then map to the mode chosen.
  const nextPayment = (step) => lines.length &&
    setPaymentType(p => PAYMENT_TYPES[(PAYMENT_TYPES.indexOf(p) + step + PAYMENT_TYPES.length) % PAYMENT_TYPES.length]);

  const toggleQuick = () => {
    if (quickOpen) { setQuickOpen(false); focusSearch(); } else { setTplOpen(false); setQuickOpen(true); }
  };
  const toggleTemplate = () => {
    if (tplOpen) { setTplOpen(false); focusSearch(); } else { setQuickOpen(false); setTplOpen(true); }
  };

  const resetBill = () => {
    try { localStorage.removeItem(draftKey); } catch { /* not kept */ }
    setDraft(null);
    saveKey.current = newIdempotencyKey();
    extrasBill.refresh();
    setEditing(null);
    setEditQty({});
    setLines([]);
    setSelectedId(null);
    setParty(setup.customer);
    setWriteOff('0');
    setPaymentType(setup.paymentType);
    setOtherPay('0');
    setCardInput('0');
    setCollected('');
    setDenoms({});
    setRedeem('');
    setBarcode('');
    setScanQty('1');
    setFindLine('');
    setQuickOpen(false);
    alertFor.current = '';
    barcodeRef.current?.focus();
  };

  const clearItems = async () => {
    if (locked() || !lines.length) return;
    if (!(await confirm(`Are you sure you want to remove all ${lines.length} item${lines.length > 1 ? 's' : ''} from this bill?`))) return focusSearch();
    setLines([]);
    setSelectedId(null);
    focusSearch();
  };

  // Removing an item asks first, as the desktop's "Item deletion..." box does (No keeps the item).
  const removeLine = async (id) => {
    if (locked()) return;
    const at = lines.findIndex(l => l.id === id);
    if (at === -1) return;
    const next = lines[at + 1]?.id ?? lines[at - 1]?.id ?? null;
    const ok = await confirm(`Are you sure you want to delete the item '${lines[at].name}'?`);
    if (ok && !sending.current) {
      setLines(cur => cur.filter(l => l.id !== id));
      setSelectedId(cur => (cur === id ? next : cur));
    }
    focusSearch();
  };

  const deleteItem = () => {
    const target = selectedId ?? lines.at(-1)?.id;
    if (target !== undefined) void removeLine(target);
  };

  const billBody = (hold) => ({
    date: editing?.date ?? todayIso(), counter: editing?.counter ?? setup.counter.code, customer: party || 'CASH', salesperson: salesPerson,
    payment_type: paymentType,
    lines: lines.map(l => ({ item_code: STOCK_ITEMS[l.id].code, bottles: l.qty, rate: l.rate })),
    discount: totals.disc + (Number(writeOff) || 0),
    redeem_points: totals.points,
    payment: { cash: totals.cash, card: totals.card, upi: totals.other, credit: 0 },
    collected: !hold && collected !== '' ? Number(collected) || 0 : undefined,
    ...(!hold && Object.keys(denoms).length ? { denominations: Object.entries(denoms).map(([cur, nos]) => ({ cur, nos })) } : {}),
    hold,
    ...(extrasBill.fields.length ? { extras: extrasBill.body(hold) } : {}),
  });

  // Held bills are parked on the backend so any counter session can recall them.
  const holdOrNew = async () => {
    if (locked()) return;
    if (editing) return setToast({ kind: 'error', text: `Bill ${editing.no} is a saved bill: finish editing it (Save), or Stop editing.` });
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

  // Recalls a held bill. A bill already on the screen is held first (a swap), so it shows in Held bills and is never lost;
  // the recalled bill's parked copy is removed only once it is on the screen. Throws when something fails, so the dialog shows why.
  const recall = async (inv) => {
    if (sending.current) throw new Error('Saving the last bill. Try again in a moment.');
    if (lines.length) {
      const parked = await send(billBody(true), saveKey.current);
      setToast({ kind: 'ok', text: `The bill you were making was held as ${parked.doc_no} (${lines.length} items).` });
    }
    await deleteSalesInvoice(inv.doc_no);
    const found = inv.lines.map(l => ({ stock: stockIndex(l.item_code), qty: l.qty, rate: l.rate }));
    const missing = found.filter(l => l.stock === -1).length;
    const recalled = found.filter(l => l.stock !== -1).map(l => ({ ...toLine(l.stock, l.qty), rate: l.rate }));
    saveKey.current = newIdempotencyKey();   // a recalled bill is a different bill
    setLines(recalled);
    extrasBill.load(inv.extras);
    setParty(customers.find(c => c.code === inv.customer)?.name ?? inv.customer);
    setDialog(null);
    setToast(missing
      ? { kind: 'error', text: `Recalled ${inv.doc_no}, but ${missing} item${missing > 1 ? 's are' : ' is'} no longer in Item Master and was left out.` }
      : { kind: 'ok', text: `Recalled held bill ${inv.doc_no}.` });
  };

  // List → Edit Selected: the saved bill goes onto the screen to be changed. Items and rates are the bill's own; the bill's discount is kept as one
  // amount (Write-off) and its payment as it was. Saving sends PUT, which puts the old stock and points back and posts the new bill.
  const loadForEdit = async (doc) => {
    if (sending.current) return setToast({ kind: 'error', text: 'Saving the last bill. Try again in a moment.' });
    if (lines.length && !(await confirm(`Open ${doc.doc_no} for editing? The bill on the screen is cleared.`))) return;
    const merged = new Map();   // one line per item, as the screen keeps them
    for (const l of doc.lines ?? []) {
      const stock = stockIndex(l.item_code);
      if (stock === -1) return setToast({ kind: 'error', text: `${doc.doc_no} has an item (${l.item_code}) that is no longer in Item Master, so it can't be edited here.` });
      const was = merged.get(stock);
      merged.set(stock, { stock, qty: (was?.qty ?? 0) + l.qty, rate: was?.rate ?? l.rate });
    }
    const bonus = Object.fromEntries([...merged.values()].map(m => [m.stock, m.qty]));
    saveKey.current = newIdempotencyKey();
    setLines([...merged.values()].map(m => ({ ...toLine(m.stock, m.qty), rate: m.rate, qoh: STOCK_ITEMS[m.stock].clQty + m.qty })));
    setSelectedId(null);
    setEditQty(bonus);
    setEditing({ no: doc.doc_no, date: doc.date, counter: doc.counter && !/^\d+$/.test(String(doc.counter)) ? doc.counter : setup.counter.code });
    extrasBill.load(doc.extras);
    setParty(customers.find(c => c.code === doc.customer)?.name ?? doc.customer ?? 'CASH');
    if (doc.salesperson && salesPersons.includes(doc.salesperson)) setSalesPerson(doc.salesperson);
    const pay = doc.payment ?? {};
    setPaymentType(PAYMENT_TYPES.includes(doc.payment_type) ? doc.payment_type : 'CASH');
    setCardInput(String(pay.card ?? 0));
    setOtherPay(String((pay.upi ?? 0) + (pay.credit ?? 0)));
    setWriteOff(String(doc.totals?.discount ?? 0));
    setDenoms(Object.fromEntries((doc.denominations ?? []).map(d => [d.cur, d.nos])));
    setRedeem(doc.totals?.points_redeemed ? String(doc.totals.points_redeemed) : '');
    setCollected('');
    setToast({ kind: 'ok', text: `Editing bill ${doc.doc_no}. Save changes updates it.` });
    barcodeRef.current?.focus();
  };

  const stopEditing = async () => {
    if (lines.length && !(await confirm(`Stop editing ${editing.no}? Your changes are thrown away.`))) return;
    resetBill();
  };

  const saveBill = async (andPrint = false) => {
    if (locked()) return;
    if (!lines.length) return setToast({ kind: 'error', text: 'Add at least one item before saving the bill.' });
    if (lines.some(l => !l.qty || !l.rate)) return setToast({ kind: 'error', text: 'Every item needs a quantity and a rate.' });
    if (totals.card + totals.other > totals.invoice) return setToast({ kind: 'error', text: 'Card + other payment exceeds the invoice value.' });
    if (collected !== '' && change < 0) return setToast({ kind: 'error', text: `Collected amount is ₹ ${money(-change)} short of the cash amount.` });
    if (totals.points > (customer?.points ?? 0)) return setToast({ kind: 'error', text: `Only ${customer?.points ?? 0} points available to redeem.` });
    if (headDef('customer').required && !party.trim()) return setToast({ kind: 'error', text: `Enter the ${headDef('customer').label}.` });
    if (headDef('salesperson').required && !salesPerson) return setToast({ kind: 'error', text: `Choose the ${headDef('salesperson').label}.` });
    const overLimit = liqLimits.find(x => x.over);
    if (overLimit) return setToast({ kind: 'error', text: `${overLimit.label} quantity exceeds the limit of ${overLimit.limit} Ltr.` });
    if (!extrasBill.check()) return setToast({ kind: 'error', text: 'Fill the additional details marked in red.' });
    // Waits for its popup (Cancel keeps the bill open). The bill is taken as it was when Save was pressed.
    const body = billBody(false);
    const key = saveKey.current;
    if (editing) {   // a saved bill changed in place: same number, stock and points follow
      const no = editing.no;
      if (!(await toastStore.defer('ok', `Bill ${no} (₹ ${money(totals.invoice)}) will be updated when this closes.`, 'Update cancelled. The bill is still open.'))) return;
      if (locked()) return;
      sending.current = true;
      setBusy(true);
      updateSalesInvoice(no, body).then(doc => {
        getCustomers().then(setCustomers).catch(() => undefined);
        loadStockItems().catch(() => undefined);
        setToast({ kind: 'ok', text: `Bill ${doc.doc_no} updated — ₹ ${money(doc.totals.net)}.` });
        if (andPrint) setPrintDocNo(doc.doc_no);
        resetBill();
      }).catch(e => setToast({ kind: 'error', text: errorText(e) })).finally(() => { sending.current = false; setBusy(false); });
      return;
    }
    if (!(await toastStore.defer('ok', `Bill of ₹ ${money(totals.invoice)} will be saved when this closes.`, 'Save cancelled. The bill is still open.'))) return;
    if (locked()) return;   // a second F10 whose popup closed while the first save was still going
    // The backend numbers the bill and moves stock; show its doc no rather than the local preview.
    send(body, key).then(doc => {
      // Refresh points and stock so the next bill sees what this one earned, redeemed and sold.
      getCustomers().then(setCustomers).catch(() => undefined);
      loadStockItems().catch(() => undefined);
      setPrevBill({ no: doc.doc_no, time: now.toLocaleTimeString('en-IN'), amount: doc.totals.net });
      setToast({ kind: 'ok', text: doc.split_bills?.length > 1 ? `Over the litre limit: saved as ${doc.split_bills.length} bills (${doc.split_bills.join(', ')}).` : `Bill ${doc.doc_no} saved — ₹ ${money(doc.totals.net)}.` });
      // Print the bill exactly as the server saved it (number, totals, points).
      // What printing follows a save comes from the Doc Designer's default print option (Global Settings is the fallback).
      const fallback = gsettings.InvPreview?.toUpperCase() === 'Y' ? 'preview' : docSettings.after_save;
      afterSavePlan('sales_invoice', fallback, andPrint).then(({ plan, option }) => {
        if (plan === 'dialog') setPrintDocNo(doc.doc_no);
        else if (plan === 'ask') confirm('Do you want to print the invoice..?').then(ok => { if (ok) setPrintDocNo(doc.doc_no); else setTimeout(focusSearch, 60); });
        else if (plan === 'direct' && option) {
          runPrint({ kind: 'sales_invoice', doc_no: doc.doc_no, option: option.name, action: 'direct' })
            .then(r => setToast({ kind: 'ok', text: r.text }))
            .catch(e => setToast({ kind: 'error', text: errorText(e) }))
            .finally(() => setTimeout(focusSearch, 60));
        } else setTimeout(focusSearch, 60);
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
    'Alt+T': toggleTemplate,
    F4: () => { if (!partyOpen) togglePartyBox(); setTimeout(() => focusField('si-party'), 0); },
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
    Escape: focusSearch,
  });

  const totalRows = [
    ['Basic amount', totals.basic], ['Discount', totals.disc], ['CGST', totals.cgst],
    ['SGST', totals.sgst], ['Cess', totals.cess], ['Comp. cess', totals.compCess], ['Points redeemed', totals.points],
  ];

  return (
    <WithHistory kind="sales_invoice" title="bills" onNew={resetBill} onEdit={doc => void loadForEdit(doc)}>
    <div className="mx-auto flex max-w-[1760px] flex-col gap-3 xl:h-[max(calc(100dvh-9.5rem),33.5rem)]">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {confirmDialog}
      {pickCols && (
        <ColumnPicker columns={cols.map(c => ({ key: c.id, label: c.caption }))} order={colPref?.order ?? cols.map(c => c.id)}
          visible={shownCols.map(c => c.id)} defaults={cols.map(c => c.id)}
          onApply={applyCols} onClose={() => { setPickCols(false); }} />
      )}
      {editing && (
        <div role="status" className="flex shrink-0 flex-wrap items-center justify-between gap-2 rounded-xl border border-[#B08A45]/60 bg-[#B08A45]/10 px-4 py-2 text-xs font-semibold text-[#64283A] dark:text-[#D4A847]">
          <span>Editing bill {editing.no}: Save updates it in place (its stock and points are put back first, then the changed bill is posted).</span>
          <button type="button" onClick={() => void stopEditing()} className={secondaryButton}>Stop editing</button>
        </div>
      )}

      {/* Document header: bill no, time and counter on the left; last bill and the actions on the right, one tidy row */}
      <div className="flex shrink-0 flex-wrap items-center gap-x-4 gap-y-2">
        <div className="min-w-0">
          <div className="flex items-center gap-2">
            <h1 className="text-xl font-semibold tracking-tight text-stone-900 dark:text-stone-50">Sales invoice</h1>
            <span className={badge('accent')}>{busy ? 'Saving…' : 'Draft'}</span>
          </div>
          <p className="mt-0.5 text-xs tabular-nums text-stone-500 dark:text-stone-400">
            <span className="font-mono font-medium text-stone-800 dark:text-stone-200">{billNo}</span> · {now.toLocaleDateString('en-GB')} {now.toLocaleTimeString('en-IN')} · Counter <span className="font-mono">{counter.code}</span>
          </p>
          <p className="mt-0.5 hidden text-xs text-stone-500 lg:block dark:text-stone-400">
            Last bill {prevBill ? <><span className="font-mono text-stone-700 dark:text-stone-300">{prevBill.no}</span> · {prevBill.time} · <span className="font-medium tabular-nums text-stone-800 dark:text-stone-200">₹ {money(prevBill.amount)}</span></> : '—'}
          </p>
        </div>
        <div className="ml-auto flex flex-wrap items-center justify-end gap-2">
          <button onClick={() => setDialog('held')} className={`${secondaryButton} h-9`}><Undo2 size={14} /><span className="max-xl:sr-only">Held bills</span><Kbd combo="F8" /></button>
          <button onClick={() => setDialog('reprint')} className={`${printButton} h-9`}><Printer size={14} /><span className="max-xl:sr-only">Re-print / Edit</span><Kbd combo="Alt+R" /></button>
          <button onClick={() => setDialog('cancel')} className={`${ghostButton} h-9 hover:!text-red-600 dark:hover:!text-red-400`}><Ban size={14} /><span className="max-xl:sr-only">Cancel invoice</span><Kbd combo="Alt+C" /></button>
          <button onClick={onClose} className={`${ghostButton} h-9`}><DoorOpen size={14} /><span className="max-xl:sr-only">Close</span></button>
        </div>
      </div>

      <div className="grid grid-cols-1 items-start gap-3 xl:min-h-0 xl:flex-1 xl:grid-cols-[minmax(0,1fr)_360px] xl:grid-rows-[minmax(0,1fr)] xl:items-stretch">
        {/* LEFT: scan + lines */}
        <div className="flex min-w-0 flex-col gap-3 xl:min-h-0">
          {draft?.length > 0 && !lines.length && !editing && (
            <div role="status" className="flex shrink-0 flex-wrap items-center gap-3 rounded-xl border border-amber-300 bg-amber-50 px-4 py-2.5 text-sm text-amber-900 dark:border-amber-700 dark:bg-amber-950/40 dark:text-amber-200">
              <span className="flex-1">An unsaved bill with {draft.length} item{draft.length === 1 ? '' : 's'} was found on this computer.</span>
              <button onClick={restoreDraft} className={`${primaryButton} !px-3 !py-1 text-xs`}>Restore</button>
              <button onClick={discardDraft} className={`${secondaryButton} !px-3 !py-1 text-xs`}>Discard</button>
            </div>
          )}
          <div className={`${panel} relative shrink-0 px-4 pb-3 pt-3`}>
            {/* The picked item: one small line at the top right, above the buttons */}
            {pickedItem && (
              <p className="absolute right-4 top-2 hidden max-w-[45%] truncate text-[11px] text-stone-500 lg:block dark:text-stone-400" aria-live="polite" title={pickedName}>
                <span className="font-semibold text-[#64283A] dark:text-[#D4A847]">{pickedName}</span>
              </p>
            )}
            <div className="flex flex-wrap items-end gap-2.5">
              <div className="min-w-[14rem] max-w-md flex-1">
                <label htmlFor="si-barcode" className={labelClass}>Scan or search item <span className="font-normal text-stone-400">(F2)</span></label>
                <ItemSearchBox id="si-barcode" ref={barcodeRef} value={barcode} onChange={setBarcode} onPick={pickItem} listOnArrow onBrowse={() => setPickerOpen(true)}
                  onEnterNoMatch={() => scan()} onKeyDown={searchKeys}
                  placeholder="Barcode, item code or name, then Enter" />
              </div>
              <div className="w-20 shrink-0">
                <label htmlFor="si-qty" className={labelClass}>Qty</label>
                <input id="si-qty" type="number" min={1} value={scanQty} onChange={e => setScanQty(e.target.value)} onFocus={e => e.target.select()} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); scan(); if (!barcode.trim()) focusSearch(); } if (e.key === 'Escape') focusSearch(); }} className={`${field} h-10 text-right text-sm tabular-nums`} />
              </div>
              <button onClick={() => scan()} tabIndex={-1} className={`${primaryButton} h-10 shrink-0 px-5`}>OK</button>
              <button onClick={toggleQuick} tabIndex={-1} aria-expanded={quickOpen} aria-controls="si-quick" className={`${secondaryButton} h-10 text-xs ${quickOpen ? '!border-accent !bg-accent-soft !text-accent-ink' : ''}`}>
                {quickOpen ? <PanelLeftClose size={14} /> : <PanelLeftOpen size={14} />}Quick pick<Kbd combo="F3" />
              </button>
              <button onClick={toggleTemplate} tabIndex={-1} aria-expanded={tplOpen} className={`${secondaryButton} h-10 text-xs ${tplOpen ? '!border-accent !bg-accent-soft !text-accent-ink' : ''}`}>
                Template<Kbd combo="Alt+T" />
              </button>
              <button onClick={() => setPickCols(true)} tabIndex={-1} className={`${ghostButton} h-10`} title="Choose and order the grid's columns"><Columns3 size={14} />Columns</button>
            </div>
            {pickerOpen && <ItemPickerModal onPick={pickFromList} onClose={() => { setPickerOpen(false); focusSearch(); }} />}
          </div>

          <div className={`grid grid-cols-1 gap-3 xl:min-h-0 xl:flex-1 xl:grid-rows-[minmax(0,1fr)] ${quickOpen || tplOpen ? 'lg:grid-cols-[240px_minmax(0,1fr)]' : ''}`}>
            {tplOpen && <QuickSelectionPanel kind="template" onAdd={stock => addItem(stock, 1)} onAddAll={stocks => stocks.reduce((cur, stock) => addItem(stock, 1, cur), lines)} onClose={() => { setTplOpen(false); focusSearch(); }} />}
            {quickOpen && <QuickSelectionPanel onAdd={stock => addItem(stock, 1)} onClose={() => { setQuickOpen(false); focusSearch(); }} />}

            <div className={`${panel} flex min-h-[22rem] flex-col overflow-hidden xl:h-full xl:min-h-[22rem]`}>
              <div className="flex min-h-0 flex-1 cursor-grab flex-col overflow-auto active:cursor-grabbing" onMouseDown={panGrid}>
                <table className={`${GRID_OUTLINE} table-fixed text-left text-xs`} style={{ width: shownCols.reduce((n, { id }) => n + widths[id], 0), minWidth: '100%' }}>
                  <thead className={tableHead}>
                    <tr>
                      {shownCols.map(({ id, caption, code }) => (
                        <th key={id} style={{ width: widths[id] }} aria-sort={sort.id === id ? (sort.dir === 1 ? 'ascending' : 'descending') : undefined}
                          className={cn('relative overflow-hidden whitespace-nowrap px-3 py-2.5', !['sl', 'item'].includes(id) && 'text-right', code && tone(code))}>
                          <button type="button" tabIndex={-1} onClick={() => sortBy(id)} title={`Sort by ${caption}`} className="inline-flex items-center gap-1 font-[inherit] hover:underline">
                            {caption}
                            {sort.id === id && (sort.dir === 1 ? <ChevronUp size={12} aria-hidden="true" /> : <ChevronDown size={12} aria-hidden="true" />)}
                          </button>
                          <span role="separator" aria-orientation="vertical" aria-label={`Resize ${caption}`} title="Drag to resize · double-click to reset all"
                            onMouseDown={startResize(id)} onTouchStart={startResize(id)} onDoubleClick={resetWidths}
                            className="absolute inset-y-0 right-0 w-1.5 cursor-col-resize hover:bg-accent/40" />
                        </th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-line-soft tabular-nums">
                    {displayed.map(({ line, i }) => {
                      const low = line.qty >= line.qoh;
                      const num = (v, cls) => <td className={`px-3 py-2 text-right ${cls}`}>{money(v)}</td>;
                      // One cell per column, in the order the grid shows them (cols).
                      const cell = {
                        sl: <td className="whitespace-nowrap px-2 py-1 text-stone-400"><span className="inline-flex items-center gap-1"><button tabIndex={-1} onClick={e => { e.stopPropagation(); void removeLine(line.id); }} className={`${iconButton} h-7 w-7 hover:!text-red-600`} aria-label={`Remove ${line.name}`}><Trash2 size={14} /></button>{i + 1}</span></td>,
                        item: <td className="truncate px-3 py-2 font-medium text-stone-900 dark:text-stone-50" title={line.name}>{line.name}</td>,
                        qoh: <td className={`px-3 py-2 text-right ${low ? 'text-amber-700 dark:text-amber-400' : 'text-stone-500'}`}>{line.qoh}</td>,
                        qty: <td className="px-3 py-1.5 text-right"><input type="number" min={1} max={line.qoh} value={line.qty || ''} data-si-row={i} data-si-col="qty" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'qty')} onChange={e => updateLine(line.id, { qty: Number(e.target.value) })} onBlur={() => line.qty || updateLine(line.id, { qty: 1 })} className={`${cellInput} font-semibold`} aria-label={`Qty for ${line.name}`} /></td>,
                        rate: <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={line.rate || ''} readOnly={colOf('rate')?.locked} data-si-row={i} data-si-col="rate" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'rate')} onChange={e => updateLine(line.id, { rate: Number(e.target.value) })} className={`${cellInput} w-20`} aria-label={`${colOf('rate')?.caption ?? 'MRP'} for ${line.name}`} /></td>,
                        discPct: <td className="px-3 py-1.5 text-right"><input type="number" min={0} max={100} value={line.discPct || ''} placeholder="0" readOnly={colOf('discPct')?.locked} data-si-row={i} data-si-col="disc" onFocus={e => { e.target.select(); setSelectedId(line.id); }} onKeyDown={cellKeys(i, 'disc')} onChange={e => updateLine(line.id, { discPct: Number(e.target.value) })} className={cellInput} aria-label={`Discount % for ${line.name}`} /></td>,
                        disc: num(discAmt(line), 'text-stone-500'),
                        amount: num(gross(line), 'text-stone-700 dark:text-stone-200'),
                        total: num(gross(line) - discAmt(line), 'font-semibold text-stone-900 dark:text-stone-50'),
                        cgstPct: num(line.cgstPct ?? 0, 'text-stone-500'),
                        cgst: num(cgstAmt(line), 'text-stone-500'),
                        sgstPct: num(line.sgstPct ?? 0, 'text-stone-500'),
                        sgst: num(sgstAmt(line), 'text-stone-500'),
                        cessPct: num(0, 'text-stone-400'),
                        cess: num(0, 'text-stone-400'),
                        compCess: num(0, 'text-stone-400'),
                        itemAmt: num(itemAmt(line), 'font-semibold text-stone-900 dark:text-stone-50'),
                      };
                      return (
                        <tr key={line.id} onClick={e => { setSelectedId(line.id); if (!e.target.closest('input,button,select')) focusSearch(); }} aria-selected={selectedId === line.id} className={`cursor-pointer transition-colors ${selectedId === line.id ? 'bg-accent-soft' : 'hover:bg-subtle/70'}`}>
                          {shownCols.map(({ id }) => <Fragment key={id}>{cell[id]}</Fragment>)}
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
                {lines.length === 0 && (
                  <div className="flex flex-1 flex-col items-center justify-center px-4 py-12 text-center">
                    <ScanLine size={28} className="mx-auto mb-2 text-stone-300 dark:text-stone-600" />
                    <p className="text-sm font-medium text-stone-700 dark:text-stone-200">No items on this bill</p>
                    <p className="mt-0.5 text-xs text-stone-500">Scan a barcode or type an item name and press Enter. Quick pick: F3.</p>
                  </div>
                )}
              </div>

              {/* Line summary + line actions */}
              <div className="flex flex-wrap items-center gap-x-6 gap-y-2 border-t border-line bg-subtle/60 px-4 py-2 text-xs text-stone-600 dark:text-stone-300">
                <span className="flex items-baseline gap-1.5"># Items<span className="text-sm font-semibold tabular-nums text-stone-900 dark:text-stone-50">{lines.length}</span></span>
                <span className="flex items-baseline gap-1.5">Total Qty<span className="text-sm font-semibold tabular-nums text-stone-900 dark:text-stone-50">{totals.qty}</span></span>
                <span className="flex items-baseline gap-1.5">Invoice Value<span className="text-sm font-semibold tabular-nums text-stone-900 dark:text-stone-50">₹ {money(totals.invoice)}</span></span>
                <span className="flex items-baseline gap-1.5">Litres<span className="text-sm font-semibold tabular-nums text-stone-900 dark:text-stone-50">{totals.litres.toFixed(3)}</span></span>
                {liqLimits.map(x => <span key={x.label} className={x.over ? 'font-semibold text-red-600' : ''} title={x.limit === null ? undefined : `Limit ${x.limit} Ltr per bill`}>{x.label} : {x.litres} Ltr</span>)}
                <div className="ml-auto flex flex-wrap items-center gap-1">
                  <input value={findLine} onChange={e => setFindLine(e.target.value)} placeholder="Find in bill…" aria-label="Find in bill" tabIndex={-1} className={`${field} h-8 !w-40 py-1 text-xs`} />
                  <span className="hidden text-[11px] text-stone-400 min-[1900px]:inline">↑↓ line · + − qty · Del remove · F9 cash · F10 save</span>
                  <button onClick={deleteItem} disabled={!lines.length} tabIndex={-1} className={ghostButton}><Trash2 size={14} />Delete item</button>
                  <button onClick={clearItems} disabled={!lines.length} tabIndex={-1} className={ghostButton}><Eraser size={14} />Clear</button>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* RIGHT: customer, totals, payment, actions */}
        <aside aria-label="Bill summary" className="flex flex-col gap-2.5 xl:sticky xl:top-0 xl:h-full xl:max-h-[calc(100dvh-9rem)] xl:min-h-0">
          {/* The customer, totals and payment scroll if the window is short; the save buttons below never do */}
          <div className="flex min-h-0 flex-1 flex-col gap-2.5 xl:overflow-y-auto">
          <div className={`${panel} p-2`}>
            <button type="button" onClick={togglePartyBox} aria-expanded={partyOpen} aria-controls="si-party-box" tabIndex={-1}
              className="flex w-full items-center justify-between gap-2 rounded-md px-1 py-0.5 text-left text-[11px] font-semibold uppercase tracking-wide text-stone-500 hover:text-stone-800 dark:text-stone-400 dark:hover:text-stone-100">
              <span className="min-w-0 truncate">{partyOpen ? 'Customer & details' : `${party || 'Walk-in'} · ${salesPerson || 'No sales person'}`}</span>
              {partyOpen ? <ChevronUp size={14} /> : <ChevronDown size={14} />}
            </button>
            <div id="si-party-box" className={`grid grid-cols-2 gap-x-2 gap-y-1.5 px-1 pb-1 pt-1.5 text-xs ${partyOpen ? '' : 'hidden'}`}>
            {!headDef('customer').hidden && <div className="min-w-0">
              <label htmlFor="si-party" className={cn(labelClass, fieldTone(headDef('customer').required, headDef('customer').locked))}>{headDef('customer').label}{headDef('customer').required && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>} <span className="font-normal text-stone-400">(F4)</span></label>
              <CustomerPicker id="si-party" value={party} onChange={v => { if (v !== party) setRedeem(''); setParty(v); }} customers={customers} disabled={headDef('customer').locked} onDone={focusSearch} />
            </div>}
            <BillExtras bill={extrasBill} idPrefix="si" />
            {customer && customer.points > 0 && (
              <div className="col-span-2 flex items-center gap-2 rounded-lg bg-accent-soft px-3 py-2 text-xs text-accent-ink">
                <Gift size={14} className="shrink-0" />
                <span className="flex-1"><span className="font-semibold tabular-nums">{customer.points}</span> loyalty points</span>
                <input type="number" min={0} max={customer.points} value={redeem} onChange={e => setRedeem(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusField('si-collected'); } }} placeholder="Redeem" className={`${field} w-24 py-1 text-right tabular-nums`} aria-label="Points to redeem" />
              </div>
            )}
            {!headDef('salesperson').hidden && <div className="min-w-0">
              <label htmlFor="si-salesperson" className={cn(labelClass, fieldTone(headDef('salesperson').required, headDef('salesperson').locked))}>{headDef('salesperson').label}{headDef('salesperson').required && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}</label>
              <select id="si-salesperson" value={salesPerson} disabled={headDef('salesperson').locked} onChange={e => setSalesPerson(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusSearch(); } }} className={`${field} h-8 py-1 text-xs`}>
                {!salesPersons.length && <option value="">No sales persons (Sales Person Master)</option>}
                {salesPersons.map(p => <option key={p}>{p}</option>)}
              </select>
            </div>}
          </div>
          </div>

          <div className={`${panel} space-y-2.5 p-3 text-xs`}>
            <div className="flex items-center justify-between gap-3 rounded-lg bg-subtle px-3 py-2.5">
              <div className="min-w-0">
                <div className="text-xs font-medium text-stone-500 dark:text-stone-400">Invoice value</div>
                <div className="text-[11px] tabular-nums text-stone-500 dark:text-stone-400">{lines.length} items · {totals.qty} qty</div>
              </div>
              <div className="text-3xl font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">₹ {money(totals.invoice)}</div>
            </div>
            <div role="radiogroup" aria-label="Payment type (F6 or Alt+P)" onKeyDown={e => {
              if (lines.length && ['ArrowRight', 'ArrowDown', 'ArrowLeft', 'ArrowUp'].includes(e.key)) {
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
                  disabled={!lines.length}
                  tabIndex={paymentType === p ? 0 : -1}
                  aria-checked={paymentType === p}
                  onClick={() => setPaymentType(p)}
                  className={`rounded-md px-2 py-1.5 text-[11px] font-semibold transition-colors disabled:cursor-not-allowed disabled:opacity-50 ${paymentType === p ? 'bg-surface text-stone-900 shadow-card dark:text-stone-50' : 'text-stone-600 hover:text-stone-900 dark:text-stone-300 dark:hover:text-stone-50'}`}
                >{p}</button>
              ))}
            </div>
            {!lines.length && <p className="-mt-1 text-[11px] text-stone-500 dark:text-stone-400">Add items to choose the payment mode.</p>}
            <div>
              <label htmlFor="si-collected" className={labelClass}>Cash tendered <span className="font-normal text-stone-400">(F9 · Enter saves)</span></label>
              <div className="flex items-center gap-2">
                <input id="si-collected" type="number" min={0} disabled={!lines.length} value={collected} onFocus={e => e.target.select()} onChange={e => setCollected(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); void saveBill(); } if (e.key === 'Escape') focusSearch(); }} className={`${field} flex-1 text-right text-sm tabular-nums`} placeholder="0.00" />
                <button type="button" onClick={() => setCountOpen(true)} disabled={!lines.length} className={`${secondaryButton} h-9 shrink-0 px-3 text-xs`} title="Count the notes and coins">Count</button>
              </div>
              {countOpen && <CashCountDialog counts={denoms} onClose={() => { setCountOpen(false); focusField('si-collected'); }} onApply={(c, total) => { setDenoms(c); setCollected(total ? String(total) : ''); setCountOpen(false); focusField('si-collected'); }} />}
              {collected !== '' && (
                <div role="status" aria-live="polite" className={`mt-2 flex items-baseline justify-between gap-3 rounded-lg px-3 py-1.5 tabular-nums ${change < 0
                  ? 'bg-red-100 text-red-800 ring-2 ring-red-500/40 dark:bg-red-950/60 dark:text-red-200'
                  : 'bg-emerald-100 text-emerald-900 ring-2 ring-emerald-500/40 dark:bg-emerald-950/60 dark:text-emerald-200'}`}>
                  <span className="text-xs font-semibold">{change < 0 ? 'Short by' : change === 0 ? 'Exact amount' : 'Return change'}</span>
                  <span className="text-xl font-bold tracking-tight">₹ {money(Math.abs(change))}</span>
                </div>
              )}
            </div>
            <dl className="grid grid-cols-3 gap-2 tabular-nums">
              <div className="rounded-lg border border-line px-2 py-1.5">
                <dt className="text-[11px] text-stone-500 dark:text-stone-400">Card</dt>
                <dd className="mt-0.5">{paymentType === 'SPLIT'
                  ? <input type="number" min={0} value={cardInput} onChange={e => setCardInput(e.target.value)} className={`${field} h-7 w-full py-0.5 text-right tabular-nums`} aria-label="Card amount" />
                  : <span className="font-medium text-stone-800 dark:text-stone-200">{money(totals.card)}</span>}</dd>
              </div>
              <div className="rounded-lg border border-line px-2 py-1.5">
                <dt className="text-[11px] text-stone-500 dark:text-stone-400">UPI / other</dt>
                <dd className="mt-0.5">{paymentType === 'SPLIT'
                  ? <input type="number" min={0} value={otherPay} onChange={e => setOtherPay(e.target.value)} className={`${field} h-7 w-full py-0.5 text-right tabular-nums`} aria-label="Other pay amount" />
                  : <span className="font-medium text-stone-800 dark:text-stone-200">{money(totals.other)}</span>}</dd>
              </div>
              <div className="rounded-lg border border-line bg-subtle px-2 py-1.5">
                <dt className="text-[11px] font-medium text-stone-700 dark:text-stone-200">Cash</dt>
                <dd className="mt-0.5 font-semibold text-stone-900 dark:text-stone-50">{money(totals.cash)}</dd>
              </div>
            </dl>
            {/* The desktop's totals grid, always open so the amounts are seen without scrolling */}
            <dl aria-label="Amount breakdown" className="grid grid-cols-2 gap-x-4 gap-y-1 rounded-lg border border-line px-3 py-2 tabular-nums">
              {[...totalRows, ['Total', totals.total]].map(([label, value]) => (
                <div key={label} className="flex justify-between gap-2">
                  <dt className={label === 'Total' ? 'font-medium text-stone-700 dark:text-stone-200' : 'text-stone-500 dark:text-stone-400'}>{label}</dt>
                  <dd className={label === 'Total' ? 'font-semibold text-stone-900 dark:text-stone-50' : value ? 'text-stone-800 dark:text-stone-200' : 'text-stone-400'}>{money(value)}</dd>
                </div>
              ))}
            </dl>
            <div className={payRow}>
              <label htmlFor="si-writeoff" className="whitespace-nowrap text-stone-500 dark:text-stone-400">Write-off</label>
              <input id="si-writeoff" type="number" min={0} disabled={!lines.length} value={writeOff} onFocus={e => e.target.select()} onChange={e => setWriteOff(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); focusField('si-collected'); } }} className={`${field} w-28 py-1 text-right tabular-nums`} />
            </div>
          </div>
          </div>
          <div className={`${panel} shrink-0 space-y-2 p-3`}>
            <div className="space-y-2">
              <button
                onClick={() => saveBill(true)}
                className={`${printButton} h-10 w-full text-sm font-bold shadow-md hover:shadow-lg`}
              >
                <Printer size={16} />Save & Print · ₹ {money(totals.invoice)}<Kbd combo="F10" />
              </button>
            </div>
            <div className="grid grid-cols-2 gap-1.5">
              <button onClick={() => saveBill(false)} title="Save without printing" className={`${secondaryButton} h-9 !px-2 text-xs font-semibold`}>Save only<Kbd combo="Mod+S" /></button>
              <button onClick={holdOrNew} title="Hold this bill and start a new one" className={`${secondaryButton} h-9 !px-2 text-xs`}>Hold<Kbd combo="F7" /></button>
            </div>
          </div>
        </aside>
      </div>

      {dialog === 'reprint' && <ReprintDialog onClose={() => setDialog(d => (d === 'reprint' ? null : d))} onEdit={doc => void loadForEdit(doc)} onPrint={(no, format) => { setPrintDocNo(no); setPrintFormat(format ?? ''); }} />}
      {dialog === 'cancel' && (
        <CancelInvoiceDialog
          onClose={() => setDialog(null)}
          onEdit={doc => { setDialog(null); void loadForEdit(doc); }}
          onDone={msg => { setDialog(null); setToast({ kind: 'ok', text: msg }); }}
        />
      )}
      {dialog === 'held' && <HeldBillsDialog onClose={() => setDialog(null)} onRecall={recall} />}

      {printDocNo && <PrintDialog kind="sales_invoice" docNo={printDocNo} initialFormat={printFormat} title={`Print Sales Invoice (${printDocNo})`} onClose={() => { setPrintDocNo(null); setTimeout(focusSearch, 60); }} />}
    </div>
    </WithHistory>
  );
}
