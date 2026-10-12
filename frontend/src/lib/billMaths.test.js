import { describe, expect, it } from 'vitest';
import { formulaAmounts } from './billMaths';

const field = (code, formula, section = 'Detail') => ({ code, section, formula, decimals: 2, type: 'number' });
const screen = (fields) => ({ fields, inputs: { line: { SINVQTY: 'qty', SINVRATE: 'rate', CGSTPER: 'item.cgst_per', SGSTPER: 'item.sgst_per' } } });
const item = () => ({ cgst_per: 9, sgst_per: 9 });
const lines = [{ item_code: 'A', qty: 2, rate: 100 }];

describe('formulaAmounts', () => {
  it('works the line amount and GST out from the designer formulas', () => {
    const s = screen([field('SINVQTY'), field('SINVRATE'), field('CGSTPER'), field('SGSTPER'),
      field('SINVAMT', '#SINVQTY#*#SINVRATE#'), field('CGSTAMT', '#SINVAMT#*#CGSTPER#/100'), field('SGSTAMT', '#SINVAMT#*#SGSTPER#/100')]);
    expect(formulaAmounts('sales_invoice', s, lines, item)).toEqual([{ amount: 200, cgst: 18, sgst: 18 }]);
  });

  it('returns nothing for a column the designer gives no formula, or without a definition', () => {
    const s = screen([field('SINVAMT', '#SINVQTY#*#SINVRATE#'), field('CGSTAMT', '')]);
    expect(formulaAmounts('sales_invoice', s, lines, item)).toEqual([{ amount: 200 }]);
    expect(formulaAmounts('sales_invoice', null, lines, item)).toEqual([{}]);
    expect(formulaAmounts('sales_invoice', screen([]), lines, item)).toEqual([{}]);
    expect(formulaAmounts('sales_return', s, lines, item)).toEqual([{}]);   // not a bill kind
  });

  it('keeps the built-in figures when the amount formula gives 0 for a priced line', () => {
    // the formula reads a column (#DISCAMT2#) the bill has no value for, so it works out to nothing
    const s = screen([field('SINVAMT', '#SINVQTY#*#SINVRATE#*#NOSUCH#'), field('CGSTAMT', '#SINVAMT#*#CGSTPER#/100')]);
    expect(formulaAmounts('sales_invoice', s, lines, item)).toEqual([{}]);
  });

  it('ignores a formula it cannot read', () => {
    const s = screen([field('SINVAMT', '#SINVQTY# ***')]);
    expect(formulaAmounts('sales_invoice', s, lines, item)).toEqual([{}]);
  });
});
