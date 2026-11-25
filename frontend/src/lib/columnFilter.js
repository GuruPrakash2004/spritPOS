// The Stock Balance grid's column filter model (the exe's vsfView.AllowFiltering): a condition on the shown text / number and a ticked value list.
//   filter   { op, text, values } for one column: `values` null = every value, else the ticked (displayed) values; a blank `text` = no condition

export const TEXT_OPS = [['contains', 'Contains'], ['equals', 'Equals'], ['begins', 'Begins with'], ['ends', 'Ends with'], ['not', 'Does not contain']];
export const NUMBER_OPS = [['eq', '= Equals'], ['ne', '≠ Not equal'], ['gt', '> Greater than'], ['ge', '≥ Greater or equal'], ['lt', '< Less than'], ['le', '≤ Less or equal']];

/** Does a displayed cell value (a string) pass this column's filter? */
export function passes(filter, isNumber, shown) {
  if (!filter) return true;
  if (filter.values && !filter.values.includes(shown)) return false;
  const t = String(filter.text ?? '').trim();
  if (!t) return true;
  if (isNumber) {
    const a = Number(shown), b = Number(t);
    if (Number.isNaN(b)) return false;
    return { eq: a === b, ne: a !== b, gt: a > b, ge: a >= b, lt: a < b, le: a <= b }[filter.op] ?? true;
  }
  const v = shown.toLowerCase(), q = t.toLowerCase();
  return { contains: v.includes(q), equals: v === q, begins: v.startsWith(q), ends: v.endsWith(q), not: !v.includes(q) }[filter.op] ?? true;
}

