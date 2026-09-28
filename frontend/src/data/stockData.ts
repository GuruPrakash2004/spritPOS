// Live item catalogue for the billing screens (Sales Invoice, Purchase Bill), loaded from /masters/items.
//
// Those screens refer to an item by its position in STOCK_ITEMS (a bill line stores that index), so
// the array is only ever updated in place: a reload updates existing items where they are and appends
// new ones, and never removes or reorders, so indexes held by an open bill stay valid.
// Components call useStockItems() to load it and re-render when it changes; call loadStockItems()
// after a save to pick up the new stock (QOH). Inactive items stay in the array but are hidden from
// pickers (see sellable()).

import { useEffect, useSyncExternalStore } from 'react';
import { api, type ApiItemFull } from '../lib/api';

export type StockItem = {
  code: string; name: string; group: string; brand: string; pkg: string; cat: string;
  clQty: number; rate: number; purchaseRate: number; cgstPct: number; sgstPct: number; active: boolean;
};

export const STOCK_ITEMS: StockItem[] = [];

let version = 0;
const listeners = new Set<() => void>();
let inFlight: Promise<StockItem[]> | null = null;

const toStockItem = (i: ApiItemFull): StockItem => ({
  code: i.code, name: i.name, group: i.group, brand: i.brand, pkg: i.package, cat: i.liqtype,
  clQty: i.qoh, rate: i.mrp, purchaseRate: i.purchase_rate ?? 0, cgstPct: i.cgst_per ?? 0, sgstPct: i.sgst_per ?? 0,
  active: i.active ?? true,
});

// Merge a fresh item list into STOCK_ITEMS without moving existing entries.
export function mergeStockItems(rows: ApiItemFull[]): void {
  const at = new Map(STOCK_ITEMS.map((item, i) => [item.code, i]));
  for (const row of rows) {
    const next = toStockItem(row);
    const i = at.get(row.code);
    if (i === undefined) STOCK_ITEMS.push(next);
    else Object.assign(STOCK_ITEMS[i], next);
  }
  version++;
  listeners.forEach(l => l());
}

export function loadStockItems(): Promise<StockItem[]> {
  inFlight ??= api<ApiItemFull[]>('/masters/items')
    .then(rows => { mergeStockItems(rows); return STOCK_ITEMS; })
    .finally(() => { inFlight = null; });
  return inFlight;
}

// Index of the item with this code, or -1.
export const stockIndex = (code: string) => STOCK_ITEMS.findIndex(i => i.code === code);

// Items a picker should offer, with their STOCK_ITEMS index.
export const sellable = () => STOCK_ITEMS.map((item, stock) => ({ ...item, stock })).filter(i => i.active);

export function useStockItems(): StockItem[] {
  useSyncExternalStore(
    cb => { listeners.add(cb); return () => listeners.delete(cb); },
    () => version,
  );
  useEffect(() => { loadStockItems().catch(() => undefined); }, []);
  return STOCK_ITEMS;
}

// Litres in one bottle, from the package code: "750ml" → 0.75, "2 LTR" / "1L" → 2 / 1, a bare "650" → 0.65.
// Display only; the backend works litres out from Package Master when a bill is saved.
export const litresPerBottle = (pkg: string) => {
  const n = Number.parseFloat(pkg) || 0;
  if (/ml/i.test(pkg)) return n / 1000;
  if (/l/i.test(pkg)) return n;
  return n >= 50 ? n / 1000 : n;
};
