// Purchase Bill row type, empty-row factory and cell/amount helpers. Items come from the live
// catalogue in data/stockData.js (a row's `stock` is its STOCK_ITEMS index).

import { STOCK_ITEMS } from '../../../data/stockData';

 





export const PURCHASE_TYPES = ['Purchase', 'Purchase Return', 'Stock Transfer In'];

export const today = () => new Date().toISOString().slice(0, 10);
export const money = (n) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

let nextUid = 1;
export const blankRow = () => ({ uid: nextUid++, stock: null, code: '', name: '', qoh: 0, qty: 0, rate: 0, cgstPct: 0, sgstPct: 0, cessPct: 0, compCess: 0 });
export const stockRow = (stock, qty) => {
  const item = STOCK_ITEMS[stock];
  return {
    ...blankRow(), stock, code: item.code, name: `${item.name} ${item.pkg}`, qoh: item.clQty, qty, rate: item.purchaseRate,
    cgstPct: item.cgstPct, sgstPct: item.sgstPct,
  };
};

export const amt = (r) => r.qty * r.rate;
export const pctOf = (r, pct) => (amt(r) * pct) / 100;
export const isFilled = (r) => r.stock !== null || r.name.trim() !== '';

export { panel, field, iconButton } from '../../../lib/styles';
export const cell = 'w-full rounded-md border border-transparent bg-transparent px-1.5 py-1 text-xs tabular-nums outline-none hover:border-line-strong focus:border-accent focus:bg-surface focus:ring-2 focus:ring-accent/20';
export const infoBar = 'flex items-center gap-2 rounded-lg bg-subtle px-3 py-2 text-xs text-stone-600 dark:text-stone-300';
export const headCell = 'px-2 py-2.5 text-left';

// Copy the selected row under a fresh uid so React keys stay unique.
export const cloneRow = (r) => ({ ...r, uid: nextUid++ });
