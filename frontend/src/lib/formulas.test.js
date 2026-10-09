import { describe, expect, it } from 'vitest';
import vectors from '../../../backend/tests/formula_vectors.json';
import { amountInWords, compute, roundHalfUp } from './formulas';

describe('formulas (shared vectors with the backend)', () => {
  it.each(vectors.words)('amount in words %s', (amount, text) => {
    expect(amountInWords(amount)).toBe(text);
  });
  it.each(vectors.round50)('rounds %s to the rupee, half up', (amount, expected) => {
    expect(roundHalfUp(amount, 0)).toBe(Number(expected));
  });
  it('works a document out in dependency order, like the server', () => {
    const { fields, values, rows, expect: want } = vectors.doc;
    const res = compute(fields, values, rows);
    for (const [code, v] of Object.entries(want.values)) {
      const got = res.values[code];
      if (v === null) expect(got ?? null).toBeNull();
      else if (typeof got === 'number') expect(got).toBeCloseTo(Number(v), 2);
      else expect(got).toBe(v);
    }
    res.rows.forEach((r, i) => Object.entries(want.rows[i]).forEach(([c, v]) => expect(r[c]).toBeCloseTo(Number(v), 2)));
    const codes = (list) => new Set(list.map(p => p.split(':')[0]));
    expect(codes(res.problems)).toEqual(codes(want.problems));
  });
  it.each(vectors.functions.map(c => [c.formula, c]))('the designer function %s', (_, c) => {
    const fields = [{ code: 'R', section: 'Total', type: 'text', decimals: 2, formula: c.formula },
      { code: 'X', section: 'Detail', type: 'number', decimals: 2, formula: '' }];
    const got = compute(fields, c.values, c.rows).values.R;
    if (Number.isFinite(Number(c.expect)) && c.expect !== '') expect(Number(got)).toBeCloseTo(Number(c.expect), 5);
    else expect(String(got)).toBe(c.expect);
  });
});
