// The calculator's expression reader (Alt+K): + - * / % and brackets over decimal numbers, no code is ever evaluated.

// Recursive-descent: expr = term (('+'|'-') term)*; term = factor (('*'|'/'|'%') factor)*; factor = ['-'|'+'] (number | '(' expr ')').
export function calculate(text) {
  const src = text.replace(/\s+/g, '');
  if (!src) return null;
  let i = 0;
  const fail = () => { throw new Error('bad'); };
  const factor = () => {
    if (src[i] === '-') { i += 1; return -factor(); }
    if (src[i] === '+') { i += 1; return factor(); }
    if (src[i] === '(') {
      i += 1;
      const v = expr();
      if (src[i] !== ')') fail();
      i += 1;
      return v;
    }
    const m = /^\d*\.?\d+|^\d+\./.exec(src.slice(i));
    if (!m) fail();
    i += m[0].length;
    return Number(m[0]);
  };
  const term = () => {
    let v = factor();
    while ('*/%'.includes(src[i] ?? '#')) {
      const op = src[i]; i += 1;
      const r = factor();
      if (op !== '*' && r === 0) fail();
      v = op === '*' ? v * r : op === '/' ? v / r : v % r;
    }
    return v;
  };
  const expr = () => {
    let v = term();
    while ('+-'.includes(src[i] ?? '#')) {
      const op = src[i]; i += 1;
      const r = term();
      v = op === '+' ? v + r : v - r;
    }
    return v;
  };
  const v = expr();
  if (i !== src.length || !Number.isFinite(v)) fail();
  return Math.round(v * 1e8) / 1e8;
}
