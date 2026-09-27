// Purchase Bill row type, empty-row factory and cell/amount helpers, plus prototype constants
// (supplier list, item codes) used until those lookups come fully from the API.

import { SAMPLE_STOCK_DATA } from '../../../data/stockData';

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

export { panel, field, iconButton } from '../../../lib/styles';
export const cell = 'w-full rounded-md border border-transparent bg-transparent px-1.5 py-1 text-xs tabular-nums outline-none hover:border-line-strong focus:border-accent focus:bg-surface focus:ring-2 focus:ring-accent/20';
export const infoBar = 'flex items-center gap-2 rounded-lg bg-subtle px-3 py-2 text-xs text-stone-600 dark:text-stone-300';
export const headCell = 'px-2 py-2.5 text-left';

// Copy the selected row under a fresh uid so React keys stay unique.
export const cloneRow = (r: Row): Row => ({ ...r, uid: nextUid++ });
