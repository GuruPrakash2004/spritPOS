import { describe, expect, it } from 'vitest';
import { cleanInput, fieldProblem, problems } from './validation';

describe('cleanInput', () => {
  it('keeps only digits in a phone number and stops at 10', () => {
    expect(cleanInput('mobile', '98a45-0 00011 99')).toBe('9845000011');
    expect(cleanInput('phone', '080-2679 3693')).toBe('0802679369');
  });
  it('upper-cases GSTIN and PAN and drops symbols', () => {
    expect(cleanInput('gstin', '29abcde1234f1z5-x')).toBe('29ABCDE1234F1Z5');
    expect(cleanInput('pan', 'abcde 1234f')).toBe('ABCDE1234F');
  });
  it('leaves other fields alone', () => {
    expect(cleanInput('name', 'Ravi & Sons')).toBe('Ravi & Sons');
  });
});

describe('fieldProblem', () => {
  it('needs exactly 10 digits, but allows blank', () => {
    expect(fieldProblem('mobile', '')).toBe('');
    expect(fieldProblem('mobile', '9845012345')).toBe('');
    expect(fieldProblem('mobile', '98450')).toMatch(/10 digits/);
    expect(fieldProblem('phone', '98450123456')).toMatch(/10 digits/);
  });
  it('checks PIN, GSTIN, PAN, HSN and e-mail', () => {
    expect(fieldProblem('pin', '5600')).toMatch(/6 digits/);
    expect(fieldProblem('pin', '560001')).toBe('');
    expect(fieldProblem('gstin', '29ABCDE1234F1Z5')).toBe('');
    expect(fieldProblem('gstin', '29ABCDE1234F1')).toMatch(/15 characters/);
    expect(fieldProblem('pan', 'ABCDE1234F')).toBe('');
    expect(fieldProblem('pan', 'ABCDE12345')).toMatch(/10 characters/);
    expect(fieldProblem('hsn_code', '2208')).toBe('');
    expect(fieldProblem('hsn_code', '22')).toMatch(/4 to 8 digits/);
    expect(fieldProblem('email', 'a@b')).toMatch(/e-mail/);
  });
  it('keeps tax percentages between 0 and 100', () => {
    expect(fieldProblem('cgst_per', 9)).toBe('');
    expect(fieldProblem('cgst_per', 120)).toMatch(/0 and 100/);
    expect(fieldProblem('sgst_per', -1)).toMatch(/0 and 100/);
  });
});

describe('problems', () => {
  it('lists every wrong field', () => {
    expect(Object.keys(problems({ mobile: '12', pin: '560001', gstin: 'X' }))).toEqual(['mobile', 'gstin']);
  });
});
