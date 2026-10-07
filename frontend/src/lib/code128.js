// Code 128 (subset B) barcodes as SVG, for the item labels (Label Printing). Printable ASCII 32–126 only.
// Each pattern is the widths of bar, space, bar, space, bar, space (11 modules); the stop pattern has a seventh bar.

const PATTERNS = [
  '212222', '222122', '222221', '121223', '121322', '131222', '122213', '122312', '132212', '221213', '221312', '231212', '112232',
  '122132', '122231', '113222', '123122', '123221', '223211', '221132', '221231', '213212', '223112', '312131', '311222', '321122',
  '321221', '312212', '322112', '322211', '212123', '212321', '232121', '111323', '131123', '131321', '112313', '132113', '132311',
  '211313', '231113', '231311', '112133', '112331', '132131', '113123', '113321', '133121', '313121', '211331', '231131', '213113',
  '213311', '213131', '311123', '311321', '331121', '312113', '312311', '332111', '314111', '221411', '431111', '111224', '111422',
  '121124', '121421', '141122', '141221', '112214', '112412', '122114', '122411', '142112', '142211', '241211', '221114', '413111',
  '241112', '134111', '111242', '121142', '121241', '114212', '124112', '124211', '411212', '421112', '421211', '212141', '214121',
  '412121', '111143', '111341', '131141', '114113', '114311', '411113', '411311', '113141', '114131', '311141', '411131', '211412',
  '211214', '211232', '2331112',
];
const START_B = 104;
const STOP = 106;

export const code128Patterns = PATTERNS;

/** The pattern values of a text (start, characters, check digit, stop), or null when it has a character Code 128 B can't hold. */
export function code128Values(text) {
  const values = [START_B];
  for (const ch of text) {
    const c = ch.charCodeAt(0);
    if (c < 32 || c > 126) return null;
    values.push(c - 32);
  }
  const check = values.reduce((sum, v, i) => sum + v * (i === 0 ? 1 : i), 0) % 103;
  return [...values, check, STOP];
}

/** An SVG string of the barcode (black bars on white, with a quiet zone), or '' when the text can't be encoded. */
export function code128Svg(text, height = 40, module = 1.5) {
  const values = code128Values(text);
  if (!values) return '';
  const quiet = 10;
  let x = quiet * module;
  let bars = '';
  for (const v of values) {
    [...PATTERNS[v]].forEach((w, i) => {
      const width = Number(w) * module;
      if (i % 2 === 0) bars += `<rect x="${x}" y="0" width="${width}" height="${height}"/>`;
      x += width;
    });
  }
  const total = x + quiet * module;
  return `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${total} ${height}" width="100%" height="${height}" preserveAspectRatio="none" fill="#000">${bars}</svg>`;
}
