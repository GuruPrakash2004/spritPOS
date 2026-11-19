import { describe, expect, it } from 'vitest';
import { applyDefinition, fieldsFromDefinition } from './designerFields';

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

describe('Bottles per Case on a new item', () => {
  const screen = (extra = {}) => ({
    doc_id: 1,
    fields: [{ web_key: 'bottles_per_case', code: 'BotPerCases', caption: 'BotPerCases', section: 'Header', order: 1, use_default: true, default: '12', ...extra }],
    web_fields: [{ key: 'bottles_per_case', kind: 'number', default: 12, caption: 'Bottles per Case' }],
  });

  it('starts empty: not the model default of 12, not a designer default', () => {
    const [f] = fieldsFromDefinition(screen());
    expect(f.key).toBe('bottles_per_case');
    expect(f.default).toBe('');
    expect(f.useDefault).toBe(false);
  });

  it('starts empty even when the designer has no row for it', () => {
    const out = fieldsFromDefinition({ doc_id: 1, fields: [{ code: 'X', caption: 'X', section: 'Header', order: 1 }], web_fields: [{ key: 'bottles_per_case', kind: 'number', default: 12 }] });
    expect(out.find(f => f.key === 'bottles_per_case').default).toBe('');
  });
});

describe('Item Master: Liquor Category and QOH', () => {
  const def = {
    doc_id: 1,
    fields: [{ web_key: 'liqtype', code: 'LIQTYPE', caption: 'LIQTYPE', section: 'Header', order: 1, mandatory: true, use_default: true, default: 'IML' }],
    web_fields: [
      { key: 'liqtype', kind: 'text', default: 'IML', options: ['IML', 'Beer', 'Wine', 'Others'], required: true },
      { key: 'qoh', kind: 'number', default: 0, caption: 'Opening stock (new items only)', new_only: true },
    ],
  };

  it('Liquor Category starts with nothing chosen', () => {
    const f = fieldsFromDefinition(def).find(x => x.key === 'liqtype');
    expect(f.default).toBe('');
    expect(f.useDefault).toBe(false);
    expect(f.required).toBe(true);
  });

  it('the opening stock is a small read-only QOH box', () => {
    const f = fieldsFromDefinition(def).find(x => x.key === 'qoh');
    expect(f.label).toBe('QOH');
    expect(f.lockedAlways).toBe(true);
    expect(f.small).toBe(true);
    expect(f.newOnly).toBeUndefined();
  });
});
