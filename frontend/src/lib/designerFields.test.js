import { describe, expect, it } from 'vitest';
import { applyDefinition } from './designerFields';

const typed = [
  { key: 'code', label: 'Code', required: true },
  { key: 'name', label: 'Name', required: true },
  { key: 'mrp', label: 'MRP', type: 'number' },
  { key: 'hsn_code', label: 'HSN' },
];
const def = (fields) => ({ fields });

describe('applyDefinition', () => {
  it('returns the same array without a definition or pairing', () => {
    expect(applyDefinition(typed, null)).toBe(typed);
    expect(applyDefinition(typed, def([{ code: 'X', caption: 'x' }]))).toBe(typed);
  });
  it('takes caption, mandatory and read-only from the designer', () => {
    const out = applyDefinition(typed, def([{ web_key: 'mrp', caption: 'Max Retail Price', mandatory: true, readonly: true, order: 1 }]));
    const f = out.find(x => x.key === 'mrp');
    expect(f).toMatchObject({ label: 'Max Retail Price', required: true, lockedAlways: true });
  });
  it('re-points a rule on a database column at the typed field', () => {
    const out = applyDefinition(typed, def([{ web_key: 'mrp', code: 'MRP', order: 1, hide_when: { field: 'HSNCode', op: '=', value: 'x' } },
      { web_key: 'hsn_code', code: 'HSNCode', order: 2 }]));
    expect(out.find(f => f.key === 'mrp').hideWhen.field).toBe('hsn_code');
  });
  it('flags a hidden field but never code or name', () => {
    const out = applyDefinition(typed, def([
      { web_key: 'hsn_code', hidden: true, order: 1 }, { web_key: 'code', hidden: true, readonly: true, order: 0 }]));
    expect(out.filter(f => !f.designerHidden).map(f => f.key)).toEqual(['code', 'name', 'mrp']);
    expect(out[0].lockedAlways).toBeFalsy();
  });
  it('reorders matched fields among their own places', () => {
    const out = applyDefinition(typed, def([{ web_key: 'mrp', order: 9 }, { web_key: 'hsn_code', order: 2 }]));
    expect(out.map(f => f.key)).toEqual(['code', 'name', 'hsn_code', 'mrp']);
  });
});
