// Sorting of the item pick list by a heading (ItemPickGrid): click once for ascending, again for descending, a third time for off.
// The owner of the list keeps { key, dir } and sorts with sortItems(), so the keyboard highlight and the grid walk the same order.

const SORT_BY = { shortName: i => i.shortName || i.name, clQty: i => i.clQty, rate: i => i.rate, code: i => i.code, name: i => i.name, group: i => i.group };

export function sortItems(items, sort) {
  const by = SORT_BY[sort?.key];
  if (!by) return items;
  return [...items].sort((a, b) => {
    const x = by(a) ?? '', y = by(b) ?? '';
    return (typeof x === 'number' && typeof y === 'number' ? x - y : String(x).localeCompare(String(y), undefined, { numeric: true, sensitivity: 'base' })) * sort.dir;
  });
}

export const nextSort = (s, key) => (s?.key === key ? (s.dir === 1 ? { key, dir: -1 } : { key: '', dir: 1 }) : { key, dir: 1 });
