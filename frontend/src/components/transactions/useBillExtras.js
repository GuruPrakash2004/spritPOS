// The header columns the database / Doc Designer added to a bill (Sales Invoice: TrnHdrSINV, Purchase Bill: TrnHdrPB).
//
// Billing is never disturbed: the fields are fixed when the bill opens (and again after each save or new bill). If the
// database changes meanwhile, the bill in progress keeps what it has and `changed` turns on, so the screen can say
// "Layout updated, applies to your next bill". Values ride in `extras` and go to the server with the bill.

import { useCallback, useEffect, useRef, useState } from 'react';
import { useScreenFields } from '../../hooks/useScreenFields';
import { extraProblem, ruleHolds, ruleValues, valueOf } from '../../lib/designerFields';

export function useBillExtras(table, idPrefix) {
  const live = useScreenFields(`table:${table}`);
  const [taken, setTaken] = useState(null);   // null until the first definition arrives
  const frozen = taken ?? [];
  const ready = taken !== null;
  const [form, setForm] = useState({ extras: {} });
  const [errors, setErrors] = useState({});

  // Take the first definition that arrives; later changes wait for the next bill.
  if (taken === null && live.screen) setTaken(live.fields);

  const latest = useRef(live.fields);   // refresh is called from async save callbacks: it must see the newest definition
  useEffect(() => { latest.current = live.fields; }, [live.fields]);
  const refresh = useCallback(() => { setTaken(latest.current); setForm({ extras: {} }); setErrors({}); }, []);
  const changed = ready && live.fields !== frozen && JSON.stringify(live.fields) !== JSON.stringify(frozen);

  /** Check the extras (mandatory / number / length); true when fine. Sets the inline errors otherwise. */
  const check = () => {
    const out = {};
    for (const f of frozen) { const wrong = extraProblem(f, form); if (wrong) out[f.key] = wrong; }
    setErrors(out);
    const first = Object.keys(out)[0];
    if (first) document.getElementById(`${idPrefix}-x-${first}`)?.focus();   // the cursor goes to the first mistake
    return !first;
  };
  // A held bill sends only what has been filled in (it is not finished); a saved one sends every field.
  const body = (hold = false) => Object.fromEntries(frozen
    .filter(f => !f.lockedAlways && !(f.hideWhen && ruleHolds(f.hideWhen, ruleValues(form))))
    .map(f => [f.key, valueOf(form, f) ?? ''])
    .filter(([, v]) => !hold || String(v).trim() !== ''));

  /** Put a recalled held bill's extras back (the server names columns as the table spells them). */
  const load = (saved) => {
    const lower = Object.fromEntries(Object.entries(saved ?? {}).map(([k, v]) => [k.toLowerCase(), v]));
    setForm({ extras: Object.fromEntries(frozen.filter(f => f.key.toLowerCase() in lower).map(f => [f.key, lower[f.key.toLowerCase()] ?? ''])) });
  };

  return { fields: frozen, form, setForm, errors, changed, check, body, load, refresh };
}
