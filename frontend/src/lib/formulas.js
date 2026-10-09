// The Doc Designer's field formulas in the browser: the same grammar and rounding as backend/app/formulas.py (both are
// tested against backend/tests/formula_vectors.json). The server works the numbers out again when a document is saved
// and is the authority; this keeps the screen's totals live while typing.
//
//   #FIELD#  + - * / ( )  SVTCOLSUM(#FIELD#)  SVTROUND2(x)  SVTROUND50(x)  SVTAMTINWORDS(x)  SVTSYSNAME  SVTUSERCODE
//
// A field is worked out when something asks for it (formulas are not stored in dependency order), with a guard against
// loops. A formula that can't be read is reported and its field is left as typed.

const TOKEN = /\s*(?:(\d+(?:\.\d+)?)|#([^#\s]+)#|('[^']*'|`[^`]*`)|([A-Za-z_][A-Za-z_0-9]*)|([-+*/(),]))/y;

export class FormulaError extends Error {}

function tokens(text) {
  const out = [];
  const src = text.trim();
  TOKEN.lastIndex = 0;
  let pos = 0;
  while (pos < src.length) {
    TOKEN.lastIndex = pos;
    const m = TOKEN.exec(src);
    if (!m || TOKEN.lastIndex === pos) throw new FormulaError(`unexpected ${src.slice(pos, pos + 8)}`);
    pos = TOKEN.lastIndex;
    if (m[1] !== undefined) out.push(['num', m[1]]);
    else if (m[2] !== undefined) out.push(['ref', m[2]]);
    else if (m[3] !== undefined) out.push(['str', m[3].slice(1, -1)]);
    else if (m[4] !== undefined) out.push(['name', m[4]]);
    else out.push(['op', m[5]]);
  }
  return out;
}

const isOp = (t, ...ops) => t && t[0] === 'op' && ops.includes(t[1]);

function parseTokens(t) {
  let i = 0;
  const peek = () => t[i];
  const take = () => t[i++];
  const expr = () => {
    let node = term();
    while (isOp(peek(), '+', '-')) { const op = take()[1]; node = ['bin', op, node, term()]; }
    return node;
  };
  const term = () => {
    let node = unary();
    while (isOp(peek(), '*', '/')) { const op = take()[1]; node = ['bin', op, node, unary()]; }
    return node;
  };
  const unary = () => {
    if (isOp(peek(), '+', '-')) { const op = take()[1]; const inner = unary(); return op === '-' ? ['neg', inner] : inner; }
    return primary();
  };
  const primary = () => {
    const tok = take();
    if (!tok) throw new FormulaError('incomplete formula');
    const [kind, value] = tok;
    if (kind === 'num') return ['num', Number(value)];
    if (kind === 'ref') return ['ref', value];
    if (kind === 'str') return ['str', value];
    if (kind === 'name') {
      if (isOp(peek(), '(')) {
        take();
        const args = [];
        if (!isOp(peek(), ')')) {
          args.push(expr());
          while (isOp(peek(), ',')) { take(); args.push(expr()); }
        }
        if (!isOp(take(), ')')) throw new FormulaError('missing )');
        return ['call', value.toUpperCase(), args];
      }
      return ['sys', value.toUpperCase()];
    }
    if (kind === 'op' && value === '(') {
      const node = expr();
      if (!isOp(take(), ')')) throw new FormulaError('missing )');
      return node;
    }
    throw new FormulaError('incomplete formula');
  };
  const tree = expr();
  if (i !== t.length) throw new FormulaError('unexpected text after the formula');
  return tree;
}

const parsed = new Map();
/** The formula as a tree; throws FormulaError. Parsed once per text. */
export function parse(text) {
  if (!parsed.has(text)) parsed.set(text, parseTokens(tokens(text)));
  return parsed.get(text);
}

export const toNumber = (v) => {
  if (v === null || v === undefined || v === '' || typeof v === 'boolean') return 0;
  const n = Number(String(v).trim());
  return Number.isFinite(n) ? n : 0;
};

/** Round half away from zero to `places` decimals (the server's Decimal ROUND_HALF_UP). */
export function roundHalfUp(v, places = 0) {
  const f = 10 ** places;
  const scaled = Math.abs(v) * f;
  return (Math.sign(v) * Math.floor(scaled + 0.5 + 1e-9)) / f;
}

const ONES = ['', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine', 'Ten', 'Eleven', 'Twelve', 'Thirteen',
  'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
const TENS = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
const below100 = (n) => (n < 20 ? ONES[n] : TENS[Math.floor(n / 10)] + (n % 10 ? ` ${ONES[n % 10]}` : ''));

function words(n) {
  if (n === 0) return 'Zero';
  const parts = [];
  for (const [size, name] of [[10000000, 'Crore'], [100000, 'Lakh'], [1000, 'Thousand'], [100, 'Hundred']]) {
    if (n >= size) {
      const count = Math.floor(n / size);
      parts.push(`${words(count)} ${(name === 'Lakh' || name === 'Crore') && count > 1 ? `${name}s` : name}`);
      n %= size;
    }
  }
  if (n) parts.push(below100(n));
  return parts.join(' ');
}

/** "Rupees Eighty Thousand Five Hundred Seventy One And Fifty Four Paise Only" (the desktop's wording). */
export function amountInWords(amount) {
  const value = roundHalfUp(Math.abs(toNumber(amount)), 2);
  const rupees = Math.floor(value + 1e-9);
  const paise = Math.round((value - rupees) * 100);
  return `Rupees ${words(rupees)}${paise ? ` And ${words(paise)} Paise` : ''} Only`;
}

/** The field codes (lower case) a formula uses. */
export function refs(text) {
  const out = new Set();
  const walk = (n) => {
    if (n[0] === 'ref') out.add(n[1].toLowerCase());
    else if (n[0] === 'bin') { walk(n[2]); walk(n[3]); } else if (n[0] === 'neg') walk(n[1]);
    else if (n[0] === 'call') n[2].forEach(walk);
  };
  try { walk(parse(text)); } catch { /* an unreadable formula uses nothing */ }
  return out;
}

/**
 * Work out every formula of a document definition. `fields`: definition fields ({code, section, formula, decimals, type});
 * `values`: typed header / footer / total values by code; `rows`: detail rows by code; `system`: {SVTSYSNAME, SVTUSERCODE}.
 * Returns { values, rows, problems } with numbers rounded half-up to each field's decimals.
 */
export function compute(fields, values = {}, rows = [], system = {}) {
  const lower = (o) => Object.fromEntries(Object.entries(o ?? {}).map(([k, v]) => [k.toLowerCase(), v]));
  const formulas = {};
  const detailFormula = new Set();
  const decimals = {};
  const kinds = {};
  for (const f of fields) {
    const code = f.code.toLowerCase();
    decimals[code] = f.decimals ?? 2;
    kinds[code] = f.type;
    if (f.formula) {
      formulas[code] = f.formula;
      if (String(f.section).toLowerCase() === 'detail') detailFormula.add(code);
    }
  }
  const typed = lower(values);
  const data = rows.map(lower);
  const sys = Object.fromEntries(Object.entries(system).map(([k, v]) => [k.toUpperCase(), v]));
  const problems = new Set();
  const memo = new Map();
  const rowMemo = data.map(() => new Map());
  const busy = new Set();

  const rowValue = (i, code) => {
    if (rowMemo[i].has(code)) return rowMemo[i].get(code);
    const key = `r${i}.${code}`;
    if (busy.has(key)) { problems.add(`${code} refers to itself`); return 0; }
    if (detailFormula.has(code)) {
      busy.add(key);
      let v;
      try { v = evalNode(parse(formulas[code]), i); } catch (e) { problems.add(`${code}: ${e.message}`); v = toNumber(data[i][code]); }
      busy.delete(key);
      rowMemo[i].set(code, v);
      return v;
    }
    if (code in data[i]) return data[i][code];
    return value(code);
  };
  const value = (code) => {
    if (memo.has(code)) return memo.get(code);
    if (code in formulas && !detailFormula.has(code)) {
      if (busy.has(code)) { problems.add(`${code} refers to itself`); return 0; }
      busy.add(code);
      let v;
      try { v = evalNode(parse(formulas[code]), null); } catch (e) { problems.add(`${code}: ${e.message}`); v = typed[code]; }
      busy.delete(code);
      memo.set(code, v);
      return v;
    }
    return typed[code];
  };
  const evalNode = (n, row) => {
    switch (n[0]) {
      case 'num': return n[1];
      case 'str': return n[1];
      case 'ref': {
        const v = row !== null ? rowValue(row, n[1].toLowerCase()) : value(n[1].toLowerCase());
        return typeof v === 'string' && v.trim() !== '' && !Number.isFinite(Number(v)) ? v : toNumber(v);
      }
      case 'neg': return -evalNode(n[1], row);
      case 'bin': {
        const a = evalNode(n[2], row);
        const b = evalNode(n[3], row);
        if (n[1] === '+') return a + b;
        if (n[1] === '-') return a - b;
        if (n[1] === '*') return a * b;
        return b ? a / b : 0;
      }
      case 'sys': return sys[n[1]] ?? '';
      default: {
        const [, name, args] = n;
        if (name === 'SVTCOLSUM') {
          if (args.length !== 1 || args[0][0] !== 'ref') throw new FormulaError('SVTCOLSUM needs a field');
          return data.reduce((sum, _, i) => sum + toNumber(rowValue(i, args[0][1].toLowerCase())), 0);
        }
        if (name === 'SVTCOLAVG') {
          if (args.length !== 1 || args[0][0] !== 'ref' || !data.length) return 0;
          return data.reduce((sum, _, i) => sum + toNumber(rowValue(i, args[0][1].toLowerCase())), 0) / data.length;
        }
        if (name === 'SVTCOLMAX') {
          if (args.length !== 1 || args[0][0] !== 'ref' || !data.length) return 0;
          return Math.max(...data.map((_, i) => toNumber(rowValue(i, args[0][1].toLowerCase()))));
        }
        if (name === 'SVTROUND') {   // vLQMS.exe: floor(x * 10^n + 0.5) / 10^n
          const scale = 10 ** (args.length > 1 ? Math.trunc(toNumber(evalNode(args[1], row))) : 0);
          return Math.floor(toNumber(evalNode(args[0], row)) * scale + 0.5 + 1e-9) / scale;
        }
        if (name === 'SVTROUND100') {   // any fraction rounds up to the next whole number
          const x = toNumber(evalNode(args[0], row));
          return Number.isInteger(x) ? x : Math.trunc(x) + 1;
        }
        if (name === 'SVTROUNDOFFAMT') {
          const x = toNumber(evalNode(args[0], row));
          return x === 0 ? 0 : roundHalfUp(Math.floor(x + 0.5 + 1e-9) - x, 2);
        }
        if (name === 'SVTCALCTAXAMT') {
          const amount = roundHalfUp(toNumber(evalNode(args[0], row)) * toNumber(evalNode(args[1], row)) / 100, 2);
          const mode = args.length > 2 ? Math.trunc(toNumber(evalNode(args[2], row))) : -1;
          if (mode === 0) return Math.floor(amount + 0.5 + 1e-9);
          if (mode === 50) return roundHalfUp(amount, 0);
          if (mode === 100) return Number.isInteger(amount) ? amount : Math.trunc(amount) + 1;
          return amount;
        }
        if (name === 'SVTCONCATENATE') return args.map(a => String(evalNode(a, row) ?? '')).join('');
        if (name === 'SVTDATEDIFF') {   // (from, to, 'D' | 'M'): the difference plus one, both ends counted
          const unit = String(args.length > 2 ? evalNode(args[2], row) : 'D').trim().toUpperCase();
          const when = (v) => { const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v).trim()) || /^(\d{2})[/-](\d{2})[/-](\d{4})/.exec(String(v).trim()); if (!m) return null; return m[1].length === 4 ? new Date(Date.UTC(+m[1], +m[2] - 1, +m[3])) : new Date(Date.UTC(+m[3], +m[2] - 1, +m[1])); };
          const a = when(evalNode(args[0], row)); const b = when(evalNode(args[1], row));
          if (!a || !b || !['D', 'M'].includes(unit)) return '';
          return unit === 'D' ? Math.round((b - a) / 86400000) + 1 : (b.getUTCFullYear() - a.getUTCFullYear()) * 12 + b.getUTCMonth() - a.getUTCMonth() + 1;
        }
        if (name === 'SVTROUND2') return roundHalfUp(toNumber(evalNode(args[0], row)), 2);
        if (name === 'SVTROUND50') return roundHalfUp(toNumber(evalNode(args[0], row)), 0);
        if (name === 'SVTAMTINWORDS') return amountInWords(evalNode(args[0], row));
        throw new FormulaError(`unknown function ${name}`);
      }
    }
  };
  const fit = (code, v) => (typeof v === 'number' && kinds[code.toLowerCase()] === 'number' ? roundHalfUp(v, decimals[code.toLowerCase()] ?? 2) : v);

  const outRows = data.map((_, i) => Object.fromEntries(fields
    .filter(f => detailFormula.has(f.code.toLowerCase())).map(f => [f.code, fit(f.code, rowValue(i, f.code.toLowerCase()))])));
  const out = Object.fromEntries(fields
    .filter(f => f.formula && !detailFormula.has(f.code.toLowerCase())).map(f => [f.code, fit(f.code, value(f.code.toLowerCase()))]));
  for (const f of fields) { if (f.formula) { try { parse(f.formula); } catch (e) { problems.add(`${f.code.toLowerCase()}: ${e.message}`); } } }
  return { values: out, rows: outRows, problems: [...problems].sort() };
}

/** Can this module work the formula out? False for a formula it can't read, a bare system word (the desktop's SYSTEMNAME
 *  is the PC's name, which a browser doesn't have) or SVTCOLSUM(5). A field whose formula can't be worked out here stays
 *  editable even when the designer marks it read-only. */
export function isSupported(text) {
  try {
    const bad = (n) => (n[0] === 'sys' ? !['SVTSYSNAME', 'SVTUSERCODE'].includes(n[1])
      : n[0] === 'bin' ? bad(n[2]) || bad(n[3]) : n[0] === 'neg' ? bad(n[1])
      : n[0] === 'call' ? (n[1] === 'SVTCOLSUM' && !(n[2].length === 1 && n[2][0][0] === 'ref')) || n[2].some(bad) : false);
    return !bad(parse(text));
  } catch { return false; }
}

/**
 * A condition operand of the designer (the "compare" and "with" of Hide / Mandatory / Read-only rules): a constant, a field
 * (#CODE#) or an expression using fields. Returns a number when it is arithmetic over numbers, else the text with every
 * #CODE# replaced by that field's value (SVTCURDATE becomes today as dd/mm/yyyy, SVTUSERCODE the signed-in user).
 */
export function operandValue(text, values = {}, system = {}) {
  let t = String(text ?? '').trim();
  if (!t) return '';
  const lower = Object.fromEntries(Object.entries(values).map(([k, v]) => [k.toLowerCase(), v]));
  const today = new Date();
  const dd = (n) => String(n).padStart(2, '0');
  t = t.replace(/SVTCURDATE/g, `${dd(today.getDate())}/${dd(today.getMonth() + 1)}/${today.getFullYear()}`)
    .replace(/SVTUSERCODE/g, String(system.SVTUSERCODE ?? ''));
  const single = /^#([^#\s]+)#$/.exec(t);
  if (single) return lower[single[1].toLowerCase()] ?? '';
  // arithmetic only when it looks like arithmetic: a field in it, or digits and operators; never a date or a plain word
  const arithmetic = (t.includes('#') || /^[\d\s+\-*/().]+$/.test(t)) && !/^\d{1,4}[/-]\d{1,2}[/-]\d{1,4}$/.test(t);
  if (!arithmetic) return t.replace(/#([^#\s]+)#/g, (_, code) => String(lower[code.toLowerCase()] ?? ''));
  try {
    const walk = (n) => {
      if (n[0] === 'num') return n[1];
      if (n[0] === 'ref') return toNumber(lower[n[1].toLowerCase()]);
      if (n[0] === 'neg') return -walk(n[1]);
      if (n[0] === 'bin') {
        const a = walk(n[2]); const b = walk(n[3]);
        return n[1] === '+' ? a + b : n[1] === '-' ? a - b : n[1] === '*' ? a * b : (b ? a / b : 0);
      }
      throw new FormulaError('not arithmetic');
    };
    return walk(parse(t));
  } catch {
    return t.replace(/#([^#\s]+)#/g, (_, code) => String(lower[code.toLowerCase()] ?? ''));
  }
}
