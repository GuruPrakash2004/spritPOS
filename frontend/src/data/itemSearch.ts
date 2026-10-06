// Item search for the bill's search box (components/transactions/ItemSearchBox.tsx): an exact code first, so
// a barcode scanner's "code + Enter" always finds the scanned item; then codes that start with the text, then
// names that start with it, then names containing every typed word.

import { sellable } from './stockData';

const MAX = 8;   // suggestions shown

export type ItemMatch = ReturnType<typeof sellable>[number];

export function searchItems(text: string, limit = MAX): ItemMatch[] {
  const term = text.trim().toLowerCase();
  if (!term) return [];
  const words = term.split(/\s+/);
  const ranked: { item: ItemMatch; rank: number }[] = [];
  for (const item of sellable()) {
    const code = item.code.toLowerCase();
    const name = `${item.brand} ${item.name} ${item.pkg}`.toLowerCase();
    const rank = code === term ? 0
      : code.startsWith(term) ? 1
      : name.startsWith(term) || item.name.toLowerCase().startsWith(term) ? 2
      : words.every(w => name.includes(w) || code.includes(w)) ? 3
      : -1;
    if (rank >= 0) ranked.push({ item, rank });
  }
  return ranked.sort((a, b) => a.rank - b.rank || a.item.name.localeCompare(b.item.name)).slice(0, limit).map(r => r.item);
}
