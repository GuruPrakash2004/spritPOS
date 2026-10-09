// The header, footer and total fields of a document (Purchase Indent, Sales Return, Stock Deduction ...), drawn from the
// Doc Designer's definition of its header table instead of from screen code.
//
// What the hook gives a screen:
//   fields      the field configs to draw (lib/designerFields.js `toFieldConfig`), in the designer's section and order;
//               a column the designer gives a formula is drawn read-only and shows its worked-out value
//   form        {extras: values by column code} for <DefinedExtras> (the stored values plus the worked-out ones)
//   setForm     what <DefinedExtras> calls when something is typed
//   computed    the worked-out header values; rowsComputed the worked-out detail columns of each line (formula engine,
//               the same inputs and grammar as the server: lib/formulas.js, backend/app/formulas.py)
//   body()      what to add to the request: typed keys (remarks ...) and `extras` (every other typed column)
//   check()     mandatory-field check with inline errors and the cursor on the first mistake
//   changed     the database's definition moved while this document is open: it keeps its fields until refresh()
// The definition is taken when the document opens and again after each save (refresh), so a bill in progress is never
// redrawn under the cashier; `changed` lets the screen say so.

import { useMemo, useState } from 'react';
import { useScreenFields } from '../../hooks/useScreenFields';
import { compute, isSupported } from '../../lib/formulas';
import { extraProblem, toFieldConfig } from '../../lib/designerFields';

const SECTION_RANK = { header: 0, footer: 1, total: 2 };

/** The value a detail column takes from a line: a key of the line, or `item.<key>` of the item's master row. */
const lineSource = (source, line, itemOf) => (source.startsWith('item.') ? itemOf?.(line.item_code)?.[source.slice(5)] : line[source]);

export function useDocHeader(table, { skip = [], lines = [], itemOf, idPrefix = 'doc' } = {}) {
  const live = useScreenFields(table ? `table:${table}` : 'table:');
  const [taken, setTaken] = useState(null);   // the definition this document opened with
  if (taken === null && live.screen) setTaken(live.screen);
  const screen = taken;
  const [values, setValues] = useState({});
  const [errors, setErrors] = useState({});

  const skipKey = skip.join('|');
  const headerFields = useMemo(() => (screen?.fields ?? [])
    .filter(f => String(f.section || 'Header').toLowerCase() !== 'detail' && !f.hidden && !skipKey.split('|').includes(f.web_key))
    .sort((a, b) => (SECTION_RANK[String(a.section).toLowerCase()] ?? 0) - (SECTION_RANK[String(b.section).toLowerCase()] ?? 0) || a.order - b.order),
  [screen, skipKey]);

  const rows = useMemo(() => {
    const spec = screen?.inputs?.line ?? {};
    return lines.map(line => Object.fromEntries(Object.entries(spec).map(([col, src]) => [col, lineSource(src, line, itemOf)])));
  }, [screen, lines, itemOf]);

  const worked = useMemo(() => {
    if (!screen) return { values: {}, rows: [], problems: [] };
    const inputs = {};
    for (const f of screen.fields) inputs[f.code] = values[f.code] ?? (f.default || undefined);
    return compute(screen.fields, inputs, rows, { SVTSYSNAME: 'WEB' });
  }, [screen, values, rows]);

  const fields = useMemo(() => headerFields.map(f => {
    const computed = !!f.formula && isSupported(f.formula);
    const base = toFieldConfig(f, screen?.doc_id);
    return { ...base, key: f.code, designerSection: f.section, lockedAlways: computed || base.lockedAlways, computed, decimals: f.decimals };
  }), [headerFields, screen]);

  const shown = (f) => {
    if (!f.computed) return values[f.key] ?? f.default ?? (f.type === 'bool' ? false : '');
    const v = worked.values[f.key];
    return typeof v === 'number' ? v.toFixed(f.decimals ?? 2) : (v ?? '');
  };
  const form = { extras: Object.fromEntries(fields.map(f => [f.key, shown(f)])) };
  const setForm = (next) => {
    setValues(prev => {
      const out = { ...prev };
      for (const f of fields) if (!f.computed && next.extras && f.key in next.extras) out[f.key] = next.extras[f.key];
      return out;
    });
    setErrors({});
  };

  const changed = !!taken && !!live.screen && JSON.stringify(live.screen.fields) !== JSON.stringify(taken.fields);

  const check = () => {
    const out = {};
    for (const f of fields) {
      if (f.computed || f.lockedAlways) continue;
      const wrong = extraProblem(f, form);
      if (wrong) out[f.key] = wrong;
    }
    setErrors(out);
    const first = Object.keys(out)[0];
    if (first) document.getElementById(`${idPrefix}-x-${first}`)?.focus();
    return !first;
  };

  /** Typed keys (remarks ...) and the other typed columns as `extras`, ready to merge into the request body. */
  const body = () => {
    const out = { extras: {} };
    for (const f of headerFields) {
      const cfg = fields.find(x => x.key === f.code);
      if (!cfg || cfg.computed || cfg.lockedAlways) continue;
      const v = values[f.code] ?? (f.default || '');
      if (f.web_key) out[f.web_key] = v;
      else if (String(v).trim() !== '') out.extras[f.code] = v;
    }
    return out;
  };

  const refresh = () => { setTaken(live.screen ?? taken); setValues({}); setErrors({}); };

  return { ready: !!screen, fields, form, setForm, errors, computed: worked.values, rowsComputed: worked.rows, problems: worked.problems,
    body, check, changed, refresh, screen };
}
