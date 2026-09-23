import { SAMPLE_STOCK_DATA } from '../stockData';

export type Row = {
  uid: number; stock: number | null; code: string; name: string; qoh: number;
  qty: number; rate: number; cgstPct: number; sgstPct: number; cessPct: number; compCess: number;
};
export type Supplier = { name: string; address: string };

export const FY_PREFIX = '26';
export const PB_SERIES = ['', 'A', 'B'];
export const PURCHASE_TYPES = ['Purchase', 'Purchase Return', 'Stock Transfer In'];
export const ITEM_CODES = SAMPLE_STOCK_DATA.map(item => item.code);
export const INITIAL_SUPPLIERS: Supplier[] = [
  { name: 'R&D DISTRIBUTORS', address: 'No. 14, KSBCL Depot Road, Peenya Industrial Area, Bengaluru - 560058' },
  { name: 'KSBCL DEPOT - BANGALORE NORTH', address: 'KSBCL Depot, Yeshwanthpur, Bengaluru - 560022' },
  { name: 'UNITED SPIRITS LTD', address: 'UB Tower, Vittal Mallya Road, Bengaluru - 560001' },
];

export const today = () => new Date().toISOString().slice(0, 10);
export const money = (n: number) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

let nextUid = 1;
export const blankRow = (): Row => ({ uid: nextUid++, stock: null, code: '', name: '', qoh: 0, qty: 0, rate: 0, cgstPct: 0, sgstPct: 0, cessPct: 0, compCess: 0 });
export const stockRow = (stock: number, qty: number): Row => {
  const item = SAMPLE_STOCK_DATA[stock];
  return { ...blankRow(), stock, code: ITEM_CODES[stock], name: `${item.name} ${item.pkg}`, qoh: item.clQty, qty, rate: item.purchaseRate };
};

export const amt = (r: Row) => r.qty * r.rate;
export const pctOf = (r: Row, pct: number) => (amt(r) * pct) / 100;
export const isFilled = (r: Row) => r.stock !== null || r.name.trim() !== '';

export const panel = 'rounded-xl border border-[#e1c9b6] bg-[#fffaf5] shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]';
export const field = 'rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-2.5 py-1.5 text-xs text-stone-800 outline-none focus:border-[#e8702a] dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-100';
export const cell = 'w-full rounded border border-transparent bg-transparent px-1 py-0.5 font-mono text-xs outline-none hover:border-[#e1c9b6] focus:border-[#e8702a] focus:bg-[#fffaf5] dark:hover:border-[#414a6e] dark:focus:bg-[#20253a]';
export const iconButton = 'relative rounded-lg p-1.5 text-stone-600 hover:bg-[#e8702a]/10 hover:text-[#b84716] disabled:opacity-40 dark:text-stone-300 dark:hover:text-[#ffb07e]';
export const infoBar = 'flex items-center gap-2 rounded-lg bg-[#f1e3d8] px-3 py-2 text-xs text-stone-600 dark:bg-[#20253a] dark:text-stone-300';
export const headCell = 'px-2 py-2 text-left font-semibold';

// Copy the selected row under a fresh uid so React keys stay unique.
export const cloneRow = (r: Row): Row => ({ ...r, uid: nextUid++ });
