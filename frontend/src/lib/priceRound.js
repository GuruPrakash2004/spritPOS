// The two roundings of the desktop's Price List Maintenance (DsgrModule.ROUND50 / ROUND100), applied to a percentage change.
//   "50p RoundOff": the rupee part, plus 1 when the first decimal digit is 5 or more (so 12.5 → 13, 12.4 → 12; the name is the desktop's).
//   "1Rs RoundOff": the rupee part, plus 1 when there is any decimal part at all (12.01 → 13, 12 → 12).

const parts = (x) => {
  const [whole, dec = ''] = Math.abs(x).toFixed(2).split('.');
  return { whole: Number(whole), dec, sign: x < 0 ? -1 : 1 };
};

export function round50(x) {
  if (!x) return x;
  const { whole, dec, sign } = parts(x);
  return sign * (Number(dec[0]) >= 5 ? whole + 1 : whole);
}

export function round100(x) {
  if (!x) return x;
  const { whole, dec, sign } = parts(x);
  return sign * (Number(dec) > 0 ? whole + 1 : whole);
}

export const ROUNDINGS = [{ id: '', label: 'No RoundOff' }, { id: '50', label: '50p RoundOff' }, { id: '100', label: '1Rs RoundOff' }];

/** The new rate: `current` increased or decreased by an amount or a percentage (rounded only for a percentage, as the desktop does). */
export function changedRate(current, { direction, mode, value, round }) {
  const v = Number(value) || 0;
  const sign = direction === 'dec' ? -1 : 1;
  const base = Number(current) || 0;
  if (mode === 'amt') return Math.round((base + sign * v) * 100) / 100;
  let n = Math.round((base + sign * (base * v) / 100) * 100) / 100;
  if (round === '50') n = round50(n);
  if (round === '100') n = round100(n);
  return n;
}
