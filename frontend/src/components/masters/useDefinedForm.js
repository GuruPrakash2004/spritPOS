// Lets a hand-built master screen (Supplier, Sales Counter) follow the database like MasterCrud does.
//
// The screen lists its own typed fields once (`typed`: key, label, required); `useDefinedForm(path, typed)` lays the
// Doc Designer's definition over them (lib/designerFields.js `applyDefinition`) and reads the extra columns the web app
// has no field for. The screen then asks `def(key)` for each field's caption / hidden / mandatory / read-only state,
// draws `<DefinedExtras>` for the extra columns, and calls `problems(form)` before saving. Layout stays the screen's own;
// the order of its sections is not changed by the designer. Without a definition (offline, no designer rows) everything
// is exactly as the screen coded it.

import { useMemo } from 'react';
import { useScreenFields } from '../../hooks/useScreenFields';
import { applyDefinition, extraProblem, ruleHolds, ruleValues, valueOf } from '../../lib/designerFields';

export function useDefinedForm(path, typed) {
  const live = useScreenFields(path);
  const applied = useMemo(() => applyDefinition(typed, live.screen), [typed, live.screen]);
  const byKey = useMemo(() => new Map(applied.map(f => [f.key, f])), [applied]);

  /** How the database wants this typed field drawn: { label, required, hidden, locked, tooltip }. */
  const def = (key, form) => {
    const f = byKey.get(key);
    if (!f) return { label: '', required: false, hidden: false, locked: false };
    const values = ruleValues(form);
    return {
      label: f.label,
      required: !!f.required || !!(f.mandatoryWhen && ruleHolds(f.mandatoryWhen, values)),
      hidden: !!f.designerHidden || !!(f.hideWhen && ruleHolds(f.hideWhen, values)),
      locked: !!f.lockedAlways || !!(f.readonlyWhen && ruleHolds(f.readonlyWhen, values)),
      tooltip: f.tooltip,
    };
  };

  /** Mistakes in what the database adds: a mandatory typed field left empty, and every extra column's rules. */
  const problems = (form, skip = []) => {
    const out = {};
    for (const f of applied) {
      if (skip.includes(f.key) || f.extra) continue;
      const d = def(f.key, form);
      if (d.hidden || !d.required) continue;
      if (String(form?.[f.key] ?? '').trim() === '') out[f.key] = `Enter the ${d.label || f.label}`;
    }
    for (const f of live.fields) {
      const wrong = extraProblem(f, form);
      if (wrong) out[f.key] = wrong;
    }
    return out;
  };

  /** What to send along with the record: the extra columns still in the definition (a dropped one is left out). */
  const extrasOf = (form) => Object.fromEntries(live.fields
    .filter(f => !f.lockedAlways && !(f.hideWhen && ruleHolds(f.hideWhen, ruleValues(form))))
    .map(f => [f.key, valueOf(form, f) ?? '']));

  return { def, problems, extrasOf, extras: live.fields, version: live.version };
}
