// The extra database columns of a hand-built master or a bill header, as a responsive grid of inputs (one cell wide, or
// the whole row for long text). Used with useDefinedForm.js / useBillExtras.js.

import LookupSelect from '../common/LookupSelect';
import { InlineError } from '../common/FormErrorSummary';
import { field as fieldClass, fieldTone, label as labelClass } from '../../lib/styles';
import { cn } from '../../lib/utils';
import { ruleHolds, ruleValues, valueOf, withValue } from '../../lib/designerFields';

export default function DefinedExtras({ fields, form, onChange, readOnly, errors = {}, idPrefix }) {
  const values = ruleValues(form);
  const shown = fields.filter(f => !(f.hideWhen && ruleHolds(f.hideWhen, values)));
  if (!shown.length) return null;
  return (
    <div className="grid gap-4 [grid-template-columns:repeat(auto-fit,minmax(min(100%,14rem),1fr))]">
      {shown.map(f => {
        const locked = readOnly || f.lockedAlways || (f.readonlyWhen && ruleHolds(f.readonlyWhen, values));
        const required = f.required || (f.mandatoryWhen && ruleHolds(f.mandatoryWhen, values));
        const id = `${idPrefix}-x-${f.key}`;
        const value = valueOf(form, f) ?? (f.type === 'bool' ? false : '');
        const set = (v) => onChange(withValue(form, f, v));
        const bad = errors[f.key];
        const tone = fieldTone(required, locked && !readOnly);   // red = mandatory, blue = read-only worked-out / locked by the designer
        const cls = cn(fieldClass, locked && !readOnly && 'text-blue-700 dark:text-blue-300', bad && '!border-red-500 !ring-2 !ring-red-500/20 animate-shake');
        return (
          <div key={f.key} className={`min-w-0 ${f.wide ? 'col-span-full' : ''}`}>
            {f.type === 'bool' ? (
              <label className="flex items-center gap-2 text-xs text-stone-700 dark:text-stone-300" title={f.tooltip}>
                <input id={id} type="checkbox" checked={!!value} disabled={locked} onChange={e => set(e.target.checked)} className="h-4 w-4 accent-accent" />
                {f.label}
              </label>
            ) : (
              <>
                <label htmlFor={id} className={cn(labelClass, tone)} title={f.tooltip}>{f.label}{required && <span className="text-red-600" aria-hidden="true">*</span>}</label>
                {f.type === 'lookup' ? (
                  <LookupSelect id={id} docId={f.lookup.docId} code={f.lookup.code} value={value} disabled={locked} onChange={set} invalid={!!bad} className={cls} />
                ) : f.type === 'select' ? (
                  <select id={id} value={value} disabled={locked} onChange={e => set(e.target.value)} className={cls}>
                    <option value="" />
                    {(f.options ?? []).map(o => <option key={o} value={o}>{o}</option>)}
                  </select>
                ) : f.multiline ? (
                  <textarea id={id} rows={3} value={value} readOnly={locked} maxLength={f.maxLength} onChange={e => set(e.target.value)} className={cls} />
                ) : (
                  <input id={id} type={f.type === 'number' ? 'number' : f.type === 'date' ? 'date' : 'text'}
                    value={f.type === 'date' ? String(value).slice(0, 10) : value} readOnly={locked}
                    maxLength={f.type === 'text' ? f.maxLength : undefined}
                    step={f.type === 'number' ? (f.decimals > 0 ? 10 ** -f.decimals : 1) : undefined}
                    onChange={e => set(e.target.value)} aria-invalid={!!bad} className={cls} />
                )}
                <InlineError message={bad} />
              </>
            )}
          </div>
        );
      })}
    </div>
  );
}
