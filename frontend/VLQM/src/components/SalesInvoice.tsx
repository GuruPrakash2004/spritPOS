import { useEffect, useMemo, useRef, useState } from 'react';
import { FileStack, PauseCircle, Search, Trash2, Undo2, XCircle, Eraser, Save, DoorOpen, PanelLeftOpen, PanelLeftClose, X } from 'lucide-react';
import Toast from './Toast';
import { SAMPLE_STOCK_DATA } from './stockData';
import { numberToWords } from './formatters';
import { PARTIES, PAYMENT_TYPES, type InvoiceSetup, type PaymentType } from './salesCounters';

type Line = { id: number; name: string; qoh: number; qty: number; rate: number; discPct: number };
type HeldBill = { party: string; lines: Line[] };

const SALES_PERSONS = ['GURU', 'PRADEEP', 'RAVI', 'ADMIN'];
// Templates reference SAMPLE_STOCK_DATA by index.
const TEMPLATES: { name: string; items: { stock: number; qty: number }[] }[] = [
  { name: 'Weekend Party Pack', items: [{ stock: 3, qty: 12 }, { stock: 1, qty: 2 }, { stock: 6, qty: 1 }] },
  { name: 'Beer Crate (12)', items: [{ stock: 3, qty: 12 }] },
  { name: 'Premium Gift Box', items: [{ stock: 7, qty: 1 }, { stock: 5, qty: 1 }] },
];

const toLine = (stock: number, qty: number, discPct = 0): Line => {
  const item = SAMPLE_STOCK_DATA[stock];
  return { id: stock, name: `${item.name} ${item.pkg}`, qoh: item.clQty, qty, rate: item.rate, discPct };
};
// Sample items so the bill opens with data to work with; later bills start empty.
const DEMO_LINES: Line[] = [toLine(0, 2), toLine(3, 6, 5), toLine(2, 4), toLine(5, 1, 2.5), toLine(7, 1)];

const money = (n: number) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
const gross = (l: Line) => l.qty * l.rate;
const discAmt = (l: Line) => (gross(l) * l.discPct) / 100;
// Splits "26/c10-0083718" into "26/c10-" + 83718 so later bills keep the same prefix and zero padding.
const parseInvoiceNo = (invoiceNo: string) => {
  const match = invoiceNo.match(/^(.*?)(\d+)$/);
  return match ? { head: match[1], seq: Number(match[2]), width: match[2].length } : { head: `${invoiceNo}-`, seq: 1, width: 1 };
};

const panel = 'rounded-xl border border-[#e1c9b6] bg-[#fffaf5] shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]';
const field = 'rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-2.5 py-1.5 text-xs text-stone-800 outline-none focus:border-[#e8702a] dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-100';
const cellInput = 'w-16 rounded border border-transparent bg-transparent px-1 py-0.5 text-right font-mono hover:border-[#e1c9b6] focus:border-[#e8702a] focus:bg-[#fffaf5] focus:outline-none dark:focus:bg-[#20253a]';
const fnButton = 'flex items-center gap-1.5 rounded-lg px-3 py-2 text-xs font-bold text-white shadow-sm';

export default function SalesInvoice({ setup, onClose }: { setup: InvoiceSetup; onClose: () => void }) {
  const { counter } = setup;
  const [numbering] = useState(() => parseInvoiceNo(setup.invoiceNo));
  const [now, setNow] = useState(() => new Date());
  const [seq, setSeq] = useState(numbering.seq);
  const [prevBill, setPrevBill] = useState<{ no: string; time: string; amount: number } | null>(null);
  const [party, setParty] = useState(setup.customer);
  const [salesPerson, setSalesPerson] = useState(SALES_PERSONS[0]);
  const [lines, setLines] = useState<Line[]>(DEMO_LINES);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [barcode, setBarcode] = useState('');
  const [scanQty, setScanQty] = useState('1');
  const [quickFilter, setQuickFilter] = useState('');
  const [quickOpen, setQuickOpen] = useState(false);
  const [writeOff, setWriteOff] = useState('0');
  const [paymentType, setPaymentType] = useState<PaymentType>(setup.paymentType);
  const [otherPay, setOtherPay] = useState('0');
  const [cardInput, setCardInput] = useState('0');
  const [collected, setCollected] = useState('');
  const [held, setHeld] = useState<HeldBill[]>([]);
  const [templatesOpen, setTemplatesOpen] = useState(false);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const barcodeRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const timer = setInterval(() => setNow(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  const billNo = `${numbering.head}${String(seq).padStart(numbering.width, '0')}`;

  const totals = useMemo(() => {
    const basic = lines.reduce((acc, l) => acc + gross(l), 0);
    const disc = lines.reduce((acc, l) => acc + discAmt(l), 0);
    // Liquor is outside GST in Karnataka (excise is built into MRP), so GST/cess rows stay at zero for these items.
    const cgst = 0, sgst = 0, cess = 0, compCess = 0;
    const total = basic - disc + cgst + sgst + cess + compCess;
    const invoice = Math.max(0, total - (Number(writeOff) || 0));
    const card = paymentType === 'CARD' ? invoice : paymentType === 'SPLIT' ? Number(cardInput) || 0 : 0;
    const other = paymentType === 'UPI' ? invoice : paymentType === 'SPLIT' ? Number(otherPay) || 0 : 0;
    const cash = Math.max(0, invoice - card - other);
    const qty = lines.reduce((acc, l) => acc + l.qty, 0);
    return { basic, disc, cgst, sgst, cess, compCess, total, invoice, card, other, cash, qty };
  }, [lines, writeOff, paymentType, cardInput, otherPay]);

  const change = collected === '' ? 0 : (Number(collected) || 0) - totals.cash;

  const quickItems = SAMPLE_STOCK_DATA
    .map((item, stock) => ({ ...item, stock }))
    .filter(item => `${item.brand} ${item.name} ${item.ksbcl}`.toLowerCase().includes(quickFilter.toLowerCase()));

  const addItem = (stock: number, qty: number, current = lines) => {
    const item = SAMPLE_STOCK_DATA[stock];
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

  const scan = () => {
    const term = barcode.trim().toLowerCase();
    const qty = Math.floor(Number(scanQty));
    if (!term) return;
    if (!qty || qty < 1) return setToast({ kind: 'error', text: 'Enter a quantity of at least 1.' });
    const stock = SAMPLE_STOCK_DATA.findIndex(i => i.ksbcl.toLowerCase() === term || `${i.brand} ${i.name}`.toLowerCase().includes(term));
    if (stock === -1) return setToast({ kind: 'error', text: `No item found for "${barcode}".` });
    addItem(stock, qty);
    setBarcode('');
    setScanQty('1');
    barcodeRef.current?.focus();
  };

  const updateLine = (id: number, patch: Partial<Line>) => setLines(lines.map(l => {
    if (l.id !== id) return l;
    const next = { ...l, ...patch };
    next.qty = Math.max(0, Math.min(next.qoh, Math.floor(next.qty) || 0));
    next.rate = Math.max(0, next.rate || 0);
    next.discPct = Math.max(0, Math.min(100, next.discPct || 0));
    return next;
  }));

  const resetBill = () => {
    setLines([]);
    setSelectedId(null);
    setParty(setup.customer);
    setWriteOff('0');
    setPaymentType(setup.paymentType);
    setOtherPay('0');
    setCardInput('0');
    setCollected('');
    barcodeRef.current?.focus();
  };

  const clearItems = () => { setLines([]); setSelectedId(null); };

  const deleteItem = () => {
    const target = selectedId ?? lines.at(-1)?.id;
    if (target === undefined) return;
    setLines(lines.filter(l => l.id !== target));
    setSelectedId(null);
  };

  const loadTemplate = (index: number) => {
    let next = lines;
    TEMPLATES[index].items.forEach(({ stock, qty }) => { next = addItem(stock, qty, next); });
    setTemplatesOpen(false);
  };

  const holdOrNew = () => {
    if (lines.length) {
      setHeld([...held, { party, lines }]);
      setToast({ kind: 'ok', text: `Bill held (${lines.length} items). Starting a new bill.` });
    }
    resetBill();
  };

  const recall = (index: number) => {
    const bill = held[index];
    const remaining = held.filter((_, i) => i !== index);
    setHeld(lines.length ? [...remaining, { party, lines }] : remaining);
    setLines(bill.lines);
    setParty(bill.party);
  };

  const cancelBill = () => {
    if (lines.length && !window.confirm('Cancel this bill? All items will be removed.')) return;
    resetBill();
  };

  const saveBill = () => {
    if (!lines.length) return setToast({ kind: 'error', text: 'Add at least one item before saving the bill.' });
    if (lines.some(l => !l.qty || !l.rate)) return setToast({ kind: 'error', text: 'Every item needs a quantity and a rate.' });
    if (totals.card + totals.other > totals.invoice) return setToast({ kind: 'error', text: 'Card + other payment exceeds the invoice value.' });
    if (collected !== '' && change < 0) return setToast({ kind: 'error', text: `Collected amount is ₹ ${money(-change)} short of the cash amount.` });
    setPrevBill({ no: billNo, time: now.toLocaleTimeString('en-IN'), amount: totals.invoice });
    setToast({ kind: 'ok', text: `Bill ${billNo} saved — ₹ ${money(totals.invoice)}.` });
    setSeq(seq + 1);
    resetBill();
  };

  const totalRows: [string, number][] = [
    ['Total Basic Amt', totals.basic], ['Total Disc Amt', totals.disc], ['Total CGST Amt', totals.cgst],
    ['Total SGST Amt', totals.sgst], ['Total Cess Amt', totals.cess], ['Total Comp.Cess', totals.compCess], ['Total', totals.total],
  ];

  return (
    <div className="mx-auto flex max-w-[1400px] flex-col gap-3">
      {toast && <Toast kind={toast.kind} text={toast.text} />}

      {/* Header strip: date, bill no, previous bill, brand */}
      <div className={`${panel} flex flex-wrap items-center gap-x-5 gap-y-2 px-4 py-2.5`}>
        <div className="flex items-center gap-2 text-xs">
          <span className="font-bold text-stone-500">Date</span>
          <span className="rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-2.5 py-1 font-mono text-stone-800 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-100">
            {now.toLocaleDateString('en-GB')} {now.toLocaleTimeString('en-IN')}
          </span>
        </div>
        <span className="rounded-lg bg-[#e8702a] px-4 py-1.5 font-mono text-lg font-bold tracking-wide text-white shadow-sm">{billNo}</span>
        <div className="flex flex-wrap gap-4 text-xs text-stone-600 dark:text-stone-300">
          <span><b>Prev bill:</b> <span className="font-mono">{prevBill?.no ?? '—'}</span></span>
          <span><b>Time:</b> <span className="font-mono">{prevBill?.time ?? '—'}</span></span>
          <span><b>Amount:</b> <span className="font-mono">{prevBill ? `₹ ${money(prevBill.amount)}` : '—'}</span></span>
        </div>
        <div className="ml-auto text-right">
          <div className="text-sm font-black tracking-tight text-[#b84716] dark:text-[#ffb07e]">Softvent</div>
          <div className="font-mono text-[10px] text-stone-500">vLQMS • support@softvent.com</div>
        </div>
      </div>

      {/* Party / sales person / counter */}
      <div className={`${panel} flex flex-wrap items-center gap-x-6 gap-y-2 px-4 py-2.5 text-xs`}>
        <label className="flex items-center gap-2 font-bold text-stone-600 dark:text-stone-300">
          Party
          <input list="si-parties" value={party} onChange={e => setParty(e.target.value)} className={`${field} w-72`} />
          <datalist id="si-parties">{PARTIES.map(p => <option key={p} value={p} />)}</datalist>
        </label>
        <label className="ml-auto flex items-center gap-2 font-bold text-stone-600 dark:text-stone-300">
          Sales Person
          <select value={salesPerson} onChange={e => setSalesPerson(e.target.value)} className={`${field} font-bold text-[#b84716] dark:text-[#ffb07e]`}>
            {SALES_PERSONS.map(p => <option key={p}>{p}</option>)}
          </select>
        </label>
        <span className="flex items-center gap-2 font-bold text-stone-600 dark:text-stone-300">
          Counter
          <span className="rounded-lg bg-[#e8702a]/15 px-2.5 py-1.5 font-mono text-[#b84716] dark:text-[#ffb07e]">{counter.code}</span>
        </span>
      </div>

      {/* Scan row */}
      <div className={`${panel} flex flex-wrap items-end gap-3 px-4 py-2.5`}>
        <div>
          <label htmlFor="si-barcode" className="block text-sm font-bold text-stone-800 dark:text-stone-100">Scan Barcode</label>
          <input id="si-barcode" ref={barcodeRef} autoFocus list="si-items" value={barcode} onChange={e => setBarcode(e.target.value)} onKeyDown={e => e.key === 'Enter' && scan()} className={`${field} w-80 font-mono`} placeholder="KSBCL code or item name" />
          <datalist id="si-items">{SAMPLE_STOCK_DATA.map(i => <option key={i.ksbcl} value={i.ksbcl}>{i.brand} {i.pkg}</option>)}</datalist>
          <p className="mt-1 text-[10px] text-stone-400">[ Scan / Enter barcode / Press Down Arrow for list ]</p>
        </div>
        <div>
          <label htmlFor="si-qty" className="block text-sm font-bold text-stone-800 dark:text-stone-100">Qty</label>
          <input id="si-qty" type="number" min={1} value={scanQty} onChange={e => setScanQty(e.target.value)} onKeyDown={e => e.key === 'Enter' && scan()} className={`${field} w-20 text-right font-mono`} />
          <p className="mt-1 text-[10px] text-transparent">.</p>
        </div>
        <button onClick={scan} className="mb-5 rounded-lg bg-stone-700 px-4 py-1.5 text-xs font-bold text-white hover:bg-stone-800 dark:bg-stone-600">OK</button>
        <button
          onClick={() => setQuickOpen(!quickOpen)}
          aria-expanded={quickOpen}
          aria-controls="si-quick"
          className={`mb-5 flex items-center gap-1.5 rounded-lg border px-3 py-1.5 text-xs font-bold transition-colors ${quickOpen
            ? 'border-[#e8702a] bg-[#e8702a] text-white hover:bg-[#b84716]'
            : 'border-[#e1c9b6] bg-[#fffaf5] text-[#b84716] hover:bg-[#e8702a]/10 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-[#ffb07e]'}`}
        >
          {quickOpen ? <PanelLeftClose size={14} /> : <PanelLeftOpen size={14} />}Quick Selection
        </button>
        <div className="mb-5 ml-auto text-right">
          <span className="text-xs font-bold text-stone-500">Inv Val:</span>{' '}
          <span className="font-mono text-2xl font-bold text-[#b84716] dark:text-[#ffb07e]">₹ {money(totals.invoice)}</span>
        </div>
      </div>

      <div className={`grid grid-cols-1 gap-3 ${quickOpen ? 'lg:grid-cols-[230px_1fr]' : ''}`}>
        {/* Quick selection: docked leftmost while open */}
        {quickOpen && (
          <div id="si-quick" className={`${panel} flex max-h-[460px] flex-col overflow-hidden`}>
            <div className="flex items-center justify-between bg-stone-800 px-3 py-2 text-xs font-bold text-white dark:bg-[#20253a]">
              Quick Selection
              <button onClick={() => setQuickOpen(false)} className="rounded p-0.5 hover:bg-white/15" aria-label="Close Quick Selection"><X size={14} /></button>
            </div>
            <div className="relative border-b border-[#e1c9b6] p-2 dark:border-[#343b59]">
              <Search size={12} className="absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
              <input autoFocus value={quickFilter} onChange={e => setQuickFilter(e.target.value)} onKeyDown={e => e.key === 'Escape' && setQuickOpen(false)} placeholder="Filter…" className={`${field} w-full pl-7`} />
            </div>
            <ul className="flex-1 divide-y divide-[#f1e3d8] overflow-auto text-xs dark:divide-[#343b59]">
              {quickItems.map(item => (
                <li key={item.ksbcl}>
                  <button onClick={() => addItem(item.stock, 1)} className="w-full px-3 py-2 text-left hover:bg-[#e8702a]/10">
                    <span className="block font-semibold text-stone-800 dark:text-stone-100">{item.brand} <span className="text-stone-400">{item.pkg}</span></span>
                    <span className="flex justify-between font-mono text-[10px] text-stone-500"><span>QOH {item.clQty}</span><span>₹ {money(item.rate)}</span></span>
                  </button>
                </li>
              ))}
            </ul>
          </div>
        )}

        {/* Item grid */}
        <div className={`${panel} min-h-[300px] overflow-auto`}>
          <table className="w-full text-left text-xs">
            <thead className="sticky top-0 whitespace-nowrap bg-[#f1e3d8] text-stone-600 dark:bg-[#20253a] dark:text-stone-300">
              <tr>
                <th className="px-3 py-2">Sl No</th>
                <th className="px-3 py-2">Item</th>
                <th className="px-3 py-2 text-right">QOH</th>
                <th className="px-3 py-2 text-right">Qty</th>
                <th className="px-3 py-2 text-right">Rate</th>
                <th className="px-3 py-2 text-right">Disc.%</th>
                <th className="px-3 py-2 text-right">Disc.Amt</th>
                <th className="px-3 py-2 text-right">Amount</th>
                <th className="px-2 py-2" />
              </tr>
            </thead>
            <tbody className="divide-y divide-[#f1e3d8] font-mono dark:divide-[#343b59]">
              {lines.length === 0 && (
                <tr><td colSpan={9} className="px-3 py-16 text-center font-sans text-stone-400">Scan a barcode or pick from Quick Selection to start the bill.</td></tr>
              )}
              {lines.map((line, i) => (
                <tr key={line.id} onClick={() => setSelectedId(line.id)} className={`cursor-pointer ${selectedId === line.id ? 'bg-[#e8702a]/10' : 'hover:bg-stone-50 dark:hover:bg-[#1b2033]'}`}>
                  <td className="px-3 py-2 font-sans text-stone-400">{i + 1}</td>
                  <td className="px-3 py-2 font-sans font-medium text-stone-900 dark:text-stone-100">{line.name}</td>
                  <td className="px-3 py-2 text-right text-stone-500">{line.qoh}</td>
                  <td className="px-3 py-2 text-right"><input type="number" min={1} max={line.qoh} value={line.qty || ''} onChange={e => updateLine(line.id, { qty: Number(e.target.value) })} onBlur={() => line.qty || updateLine(line.id, { qty: 1 })} className={`${cellInput} font-bold`} aria-label={`Qty for ${line.name}`} /></td>
                  <td className="px-3 py-2 text-right"><input type="number" min={0} step="0.01" value={line.rate || ''} onChange={e => updateLine(line.id, { rate: Number(e.target.value) })} className={`${cellInput} w-20`} aria-label={`Rate for ${line.name}`} /></td>
                  <td className="px-3 py-2 text-right"><input type="number" min={0} max={100} value={line.discPct || ''} placeholder="0" onChange={e => updateLine(line.id, { discPct: Number(e.target.value) })} className={cellInput} aria-label={`Discount % for ${line.name}`} /></td>
                  <td className="px-3 py-2 text-right text-stone-500">{money(discAmt(line))}</td>
                  <td className="px-3 py-2 text-right font-bold text-stone-900 dark:text-stone-100">{money(gross(line) - discAmt(line))}</td>
                  <td className="px-2 py-2 text-center">
                    <button onClick={e => { e.stopPropagation(); setLines(lines.filter(l => l.id !== line.id)); }} className="text-stone-400 hover:text-red-500" aria-label={`Remove ${line.name}`}><Trash2 size={13} /></button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Totals & payment: below the item list so the grid gets the full width */}
      <div className="grid grid-cols-1 gap-3 md:grid-cols-2 xl:grid-cols-[1.4fr_1fr_1fr]">
        <div className={`${panel} overflow-hidden text-xs md:col-span-2 xl:col-span-1`}>
          <div className="border-b border-[#e1c9b6] bg-[#f1e3d8] px-3 py-1.5 font-bold text-stone-600 dark:border-[#343b59] dark:bg-[#20253a] dark:text-stone-300">Amounts</div>
          <dl className="grid grid-cols-1 sm:grid-cols-2">
            {totalRows.map(([label, value]) => (
              <div key={label} className={`flex justify-between gap-3 border-b border-[#f1e3d8] px-3 py-1.5 dark:border-[#343b59] sm:odd:border-r ${label === 'Total' ? 'bg-[#e8702a]/10 font-bold sm:col-span-2 sm:border-r-0' : ''}`}>
                <dt className="font-medium text-stone-600 dark:text-stone-300">{label}</dt>
                <dd className="font-mono">{money(value)}</dd>
              </div>
            ))}
          </dl>
        </div>

        <div className={`${panel} overflow-hidden text-xs`}>
          <table className="w-full">
            <tbody className="divide-y divide-[#f1e3d8] dark:divide-[#343b59]">
              <tr>
                <th className="px-3 py-1.5 text-left font-medium text-stone-600 dark:text-stone-300">Write Off Amt</th>
                <td className="px-3 py-1 text-right"><input type="number" min={0} value={writeOff} onChange={e => setWriteOff(e.target.value)} className={`${field} w-24 py-0.5 text-right font-mono`} aria-label="Write off amount" /></td>
              </tr>
              <tr>
                <th className="px-3 py-1.5 text-left font-medium text-stone-600 dark:text-stone-300">Payment Type</th>
                <td className="px-3 py-1 text-right">
                  <select value={paymentType} onChange={e => setPaymentType(e.target.value as PaymentType)} className={`${field} py-0.5 font-bold`} aria-label="Payment type">
                    {PAYMENT_TYPES.map(p => <option key={p}>{p}</option>)}
                  </select>
                </td>
              </tr>
              <tr>
                <th className="px-3 py-1.5 text-left font-medium text-stone-600 dark:text-stone-300">Other Pay Amt</th>
                <td className="px-3 py-1 text-right">
                  {paymentType === 'SPLIT'
                    ? <input type="number" min={0} value={otherPay} onChange={e => setOtherPay(e.target.value)} className={`${field} w-24 py-0.5 text-right font-mono`} aria-label="Other pay amount" />
                    : <span className="font-mono">{money(totals.other)}</span>}
                </td>
              </tr>
              <tr>
                <th className="px-3 py-1.5 text-left font-medium text-stone-600 dark:text-stone-300">Card Amount</th>
                <td className="px-3 py-1 text-right">
                  {paymentType === 'SPLIT'
                    ? <input type="number" min={0} value={cardInput} onChange={e => setCardInput(e.target.value)} className={`${field} w-24 py-0.5 text-right font-mono`} aria-label="Card amount" />
                    : <span className="font-mono">{money(totals.card)}</span>}
                </td>
              </tr>
              <tr>
                <th className="px-3 py-1.5 text-left font-medium text-stone-600 dark:text-stone-300">Cash Amt</th>
                <td className="px-3 py-1.5 text-right font-mono">{money(totals.cash)}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className={`${panel} flex flex-col justify-between gap-3 p-3 text-xs`}>
          <div className="rounded-lg bg-[#e8702a]/10 px-3 py-2">
            <div className="font-bold text-[#b84716] dark:text-[#ffb07e]">Invoice Value</div>
            <div className="text-right font-mono text-2xl font-bold text-[#b84716] dark:text-[#ffb07e]">₹ {money(totals.invoice)}</div>
          </div>
          <div>
            <label htmlFor="si-collected" className="mb-1.5 block font-bold text-stone-700 dark:text-stone-200">Collected Amount / Tender Change</label>
            <div className="flex items-center gap-2">
              <input id="si-collected" type="number" min={0} value={collected} onChange={e => setCollected(e.target.value)} onKeyDown={e => e.key === 'Enter' && saveBill()} className={`${field} w-28 text-right font-mono`} placeholder="0.00" />
              <span className={`ml-auto rounded-lg px-3 py-1.5 font-mono font-bold ${change < 0 ? 'bg-red-100 text-red-700 dark:bg-red-950/50 dark:text-red-300' : 'bg-emerald-100 text-emerald-700 dark:bg-emerald-950/50 dark:text-emerald-300'}`}>
                {change < 0 ? 'Short' : 'Change'} ₹ {money(Math.abs(change))}
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* Summary bar */}
      <div className={`${panel} flex flex-wrap items-center justify-between gap-3 px-5 py-2.5`}>
        <span className="text-sm font-bold text-stone-700 dark:text-stone-200"># Items: <span className="font-mono">{lines.length}</span></span>
        <span className="text-sm font-bold text-stone-700 dark:text-stone-200">Total Qty: <span className="font-mono">{totals.qty}</span></span>
        {held.length > 0 && (
          <span className="flex flex-wrap items-center gap-1">
            <span className="text-[10px] font-bold uppercase text-stone-500">Held</span>
            {held.map((bill, i) => (
              <button key={i} onClick={() => recall(i)} className="flex items-center gap-1 rounded-md bg-amber-100 px-2 py-1 text-[10px] font-semibold text-amber-800 dark:bg-amber-900/40 dark:text-amber-300">
                <Undo2 size={10} />#{i + 1} • {bill.lines.length} items
              </button>
            ))}
          </span>
        )}
        <span className="hidden text-[11px] italic text-stone-500 xl:inline">{numberToWords(totals.invoice)}</span>
        <span className="text-sm font-bold text-[#b84716] dark:text-[#ffb07e]">Invoice Value: <span className="font-mono text-lg">₹ {money(totals.invoice)}</span></span>
      </div>

      {/* Function keys */}
      <div className="flex flex-wrap justify-center gap-2">
        <button onClick={clearItems} className={`${fnButton} bg-stone-600 hover:bg-stone-700`}><Eraser size={14} />Clear Items</button>
        <div className="relative">
          <button onClick={() => setTemplatesOpen(!templatesOpen)} className={`${fnButton} bg-sky-600 hover:bg-sky-700`}><FileStack size={14} />Load Template</button>
          {templatesOpen && (
            <div className="absolute bottom-full left-0 z-10 mb-2 w-52 overflow-hidden rounded-xl border border-[#e1c9b6] bg-[#fffaf5] text-xs shadow-lg dark:border-[#414a6e] dark:bg-[#20253a]">
              {TEMPLATES.map((t, i) => (
                <button key={t.name} onClick={() => loadTemplate(i)} className="block w-full px-3 py-2 text-left text-stone-700 hover:bg-[#f6efe8] dark:text-stone-200 dark:hover:bg-[#343b59]">{t.name}</button>
              ))}
            </div>
          )}
        </div>
        <button onClick={holdOrNew} className={`${fnButton} bg-amber-600 hover:bg-amber-700`}><PauseCircle size={14} />Hold / New Bill</button>
        <button onClick={deleteItem} className={`${fnButton} bg-red-600 hover:bg-red-700`}><Trash2 size={14} />Delete Item</button>
        <button onClick={cancelBill} className={`${fnButton} bg-stone-400 hover:bg-stone-500`}><XCircle size={14} />Cancel</button>
        <button onClick={saveBill} className={`${fnButton} bg-[#e8702a] hover:bg-[#b84716]`}><Save size={14} />Save Bill</button>
        <button onClick={onClose} className={`${fnButton} bg-[#e8702a] hover:bg-[#b84716]`}><DoorOpen size={14} />Close</button>
      </div>
    </div>
  );
}
