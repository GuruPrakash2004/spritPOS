import { describe, expect, it } from 'vitest';
import { calculate } from './calculate';

describe('calculate', () => {
  it('follows the usual order of operations', () => {
    expect(calculate('1250*3-40')).toBe(3710);
    expect(calculate('2+3*4')).toBe(14);
    expect(calculate('(2+3)*4')).toBe(20);
    expect(calculate('-5+10/4')).toBe(-2.5);
    expect(calculate('10%3')).toBe(1);
  });
  it('is empty for nothing and refuses anything that is not arithmetic', () => {
    expect(calculate('  ')).toBeNull();
    expect(() => calculate('2+')).toThrow();
    expect(() => calculate('alert(1)')).toThrow();
    expect(() => calculate('1/0')).toThrow();
    expect(() => calculate('(1+2')).toThrow();
  });
});
