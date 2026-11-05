import { describe, expect, it } from 'vitest';
import { changedRate, round100, round50 } from './priceRound';

describe('price rounding (the desktop\'s ROUND50 / ROUND100)', () => {
  it('50p RoundOff goes up from a first decimal digit of 5', () => {
    expect(round50(12.5)).toBe(13);
    expect(round50(12.49)).toBe(12);
    expect(round50(12)).toBe(12);
  });
  it('1Rs RoundOff goes up for any decimal part', () => {
    expect(round100(12.01)).toBe(13);
    expect(round100(12)).toBe(12);
  });
  it('changes a rate by an amount or a percentage, either way', () => {
    expect(changedRate(100, { direction: 'inc', mode: 'amt', value: '5.5' })).toBe(105.5);
    expect(changedRate(100, { direction: 'dec', mode: 'amt', value: 10 })).toBe(90);
    expect(changedRate(250, { direction: 'inc', mode: 'per', value: 5 })).toBe(262.5);
    expect(changedRate(250, { direction: 'inc', mode: 'per', value: 5, round: '50' })).toBe(263);
    expect(changedRate(250, { direction: 'dec', mode: 'per', value: 5, round: '100' })).toBe(238);
  });
});
