// The order of the drop-downs on the Item Master: Brand in name order (A to Z); Package, Pkg Item and Pkg UOM in numeric order
// (2, 180ML, 275ML, 750ML, 1000ML: the number in the name, not the letters). Every other list keeps its own order.

const collator = new Intl.Collator(undefined, { numeric: true, sensitivity: 'base' });

/** 'name' | 'numeric' | null for a form field (typed or from the Doc Designer), by its key and caption. */
export function optionSortKind(f) {
  const text = `${f?.key ?? ''} ${f?.label ?? ''}`.toLowerCase();
  if (/brand/.test(text)) return 'name';
  if (/pkg|package/.test(text)) return 'numeric';
  return null;
}

/** A sorted copy. `label` reads the shown text of an option (a string by default). Without a kind the list is returned as given. */
export function sortOptions(list, kind, label = (x) => String(x)) {
  if (!kind || !list) return list;
  return [...list].sort((a, b) => collator.compare(label(a), label(b)));
}
