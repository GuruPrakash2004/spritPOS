// The vLQMS.exe colours for a document's grid columns: a mandatory column's heading is red, a read-only one blue
// (read-only wins when both). The flags come from the Doc Designer's Detail rows of the document's header table, read live
// (hooks/useScreenFields.js), so they follow the database. `tone('PBQTY')` gives the text-colour classes for that column's
// heading, '' when the designer has no such column or it is neither.

import { useCallback, useMemo } from 'react';
import { useScreenFields } from './useScreenFields';
import { fieldTone } from '../lib/styles';

export function useColumnTone(table) {
  const { screen } = useScreenFields(`table:${table}`);
  const byCode = useMemo(() => new Map((screen?.fields ?? [])
    .filter(f => String(f.section).toLowerCase() === 'detail')
    .map(f => [f.code.toLowerCase(), f])), [screen]);
  return useCallback((code) => {
    const f = byCode.get(String(code).toLowerCase());
    return f ? fieldTone(f.mandatory && !f.hidden, f.readonly) : '';
  }, [byCode]);
}
