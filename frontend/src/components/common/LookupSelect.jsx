// A drop-down whose choices are rows of a database table, as the Doc Designer describes it (a field's combo: table,
// storage column, display columns, extra condition). The server reads the rows (GET /designer/lookup/{doc}/{field});
// the value kept is the storage column (an id), the label the display columns. The list is read again when the
// database's structure changes.

import { useEffect, useState, useSyncExternalStore } from 'react';
import { getDesignerLookup } from '../../lib/api';
import { getDesignerSnapshot, subscribeDesigner } from '../../lib/designerStore';

export default function LookupSelect({ id, docId, code, value, onChange, disabled, className = '', invalid, ...rest }) {
  const { changes } = useSyncExternalStore(subscribeDesigner, getDesignerSnapshot);
  const [choices, setChoices] = useState(null);

  useEffect(() => {
    let live = true;
    getDesignerLookup(docId, code).then(rows => { if (live) setChoices(rows); }).catch(() => { if (live) setChoices([]); });
    return () => { live = false; };
  }, [docId, code, changes]);

  const current = value === null || value === undefined ? '' : String(value);
  const known = (choices ?? []).some(c => String(c.value) === current);
  return (
    <select id={id} value={current} disabled={disabled || choices === null} onChange={e => onChange(e.target.value)}
      aria-invalid={invalid || undefined} className={className} {...rest}>
      <option value="" />
      {current !== '' && !known && <option value={current}>{current}</option>}
      {(choices ?? []).map(c => <option key={String(c.value)} value={String(c.value)}>{c.label}</option>)}
    </select>
  );
}
