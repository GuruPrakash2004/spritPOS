// A bill's line grid laid out by the Doc Designer: the Detail rows of the document's header table (UtlDocDsgrFlds) give each
// column's caption, order, hidden and read-only flags, as the desktop's grid shows them.
//
//   const cols = applyGridDefinition(GRID_COLS, screen, new Set(['sl', 'item', 'qty', 'rate']));
//
// `base` lists the web's own columns as [id, caption, designerColumnCode | null, startingWidth]. A column with a designer
// row takes its caption, is dropped when the designer hides it (never one in `keep`: the bill can't work without it) and
// is read-only when the designer says so; the coded columns take the designer's order among the places they already hold,
// the rest (Sl No, Item, QOH, Total Item Amt: no designer row) stay where the web put them. With no definition (offline, no
// designer rows) the columns are exactly `base`.

export function applyGridDefinition(base, screen, keep = new Set()) {
  const detail = new Map((screen?.fields ?? [])
    .filter(f => String(f.section).toLowerCase() === 'detail')
    .map(f => [String(f.code).toLowerCase(), f]));
  const cols = base.map(([id, caption, code, width]) => {
    const d = code ? detail.get(code.toLowerCase()) : undefined;
    return { id, caption: d?.caption || caption, code, width, locked: !!d?.readonly && !keep.has('lock:' + id), hidden: !!d?.hidden && !keep.has(id), order: d?.order, mandatory: !!d?.mandatory };
  });
  const slots = cols.map((c, i) => (c.order != null ? i : -1)).filter(i => i >= 0);
  const sorted = slots.map(i => cols[i]).sort((a, b) => a.order - b.order);
  slots.forEach((slot, n) => { cols[slot] = sorted[n]; });
  return cols.filter(c => !c.hidden);
}
