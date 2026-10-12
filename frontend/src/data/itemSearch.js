// Item search for the bill's search box (components/transactions/ItemSearchBox.jsx): an exact code first, so
// a barcode scanner's "code + Enter" always finds the scanned item; then codes that start with the text, then
// names that start with it, then names containing every typed word. Only items in stock are offered (a bill can't
// sell an item with none); an empty text lists them all, which is the dropdown the clerk opens with a click.

import { sellable } from './stockData';

const MAX = 300;   // rows listed

export function searchItems(text, limit = MAX) {
  const term = text.trim().toLowerCase();
  const words = term.split(/\s+/).filter(Boolean);
  const ranked = [];
  for (const item of sellable()) {
    if (!(item.clQty > 0)) continue;
    const code = item.code.toLowerCase();
    const name = `${item.brand} ${item.name} ${item.pkg} ${item.shortName ?? ''} ${item.hotKey ?? ''} ${(item.aliases ?? []).join(' ')}`.toLowerCase();   // the item's aliases (MstItemAlias) find it too
    const rank = !term ? 0
      : code === term ? 0
      : code.startsWith(term) ? 1
      : name.startsWith(term) || item.name.toLowerCase().startsWith(term) ? 2
      : words.every(w => name.includes(w) || code.includes(w)) ? 3
      : -1;
    if (rank >= 0) ranked.push({ item, rank });
  }
  // Within a rank the list is in the desktop's order: by ShortName (blank first), then item name.
  const key = (item) => (item.shortName || '').toUpperCase();
  const order = (a, b) => (a < b ? -1 : a > b ? 1 : 0);
  return ranked.sort((a, b) => a.rank - b.rank || order(key(a.item), key(b.item)) || order(a.item.name.toUpperCase(), b.item.name.toUpperCase())).slice(0, limit).map(r => r.item);
}
