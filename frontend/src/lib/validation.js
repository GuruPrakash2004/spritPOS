// Field rules for what a person types into a master, as the desktop vLQMS forms enforce them. The backend checks the same
// rules (backend/app/validation.py), so change both together.
//   mobile / phone / excise phone: exactly 10 digits, nothing else    PIN code: 6 digits
//   GSTIN: 15 characters like 29ABCDE1234F1Z5   PAN: 10 characters like ABCDE1234F   HSN: 4 to 8 digits
//   e-mail: looks like one   CGST % / SGST %: 0 to 100
// cleanInput(key, value) is what a box keeps while the person types (digits only, capitals, a length limit), so a wrong
// character never gets in; fieldProblem(key, value) is the message to show when the field is wrong ('' when fine). A blank
// value is allowed (these fields are optional): a required field is checked on its own.

const TEN_DIGITS = { label: 'Mobile No', max: 10, digits: true, pattern: /^\d{10}$/, wrong: 'must be exactly 10 digits (numbers only, no spaces, letters or symbols)' };
export const FIELD_RULES = {
  mobile: { ...TEN_DIGITS, label: 'Mobile No' },
  phone: { ...TEN_DIGITS, label: 'Phone No' },
  excise_phone: { ...TEN_DIGITS, label: 'Excise phone' },
  pin: { label: 'PIN code', max: 6, digits: true, pattern: /^\d{6}$/, wrong: 'must be exactly 6 digits' },
  gstin: { label: 'GSTIN', max: 15, upper: true, alnum: true, pattern: /^\d{2}[A-Z]{5}\d{4}[A-Z][1-9A-Z]Z[0-9A-Z]$/, wrong: 'must be 15 characters like 29ABCDE1234F1Z5' },
  pan: { label: 'PAN', max: 10, upper: true, alnum: true, pattern: /^[A-Z]{5}\d{4}[A-Z]$/, wrong: 'must be 10 characters like ABCDE1234F' },
  hsn_code: { label: 'HSN code', max: 8, digits: true, pattern: /^\d{4,8}$/, wrong: 'must be 4 to 8 digits (numbers only)' },
  ifsc: { label: 'IFSC code', max: 11, upper: true, alnum: true, pattern: /^[A-Z]{4}0[A-Z0-9]{6}$/, wrong: 'must be 11 characters like SBIN0001234' },
  micr: { label: 'MICR code', max: 9, digits: true, pattern: /^\d{9}$/, wrong: 'must be exactly 9 digits' },
  account_no: { label: 'Account no.', max: 18, digits: true, pattern: /^\d{9,18}$/, wrong: 'must be 9 to 18 digits (numbers only)' },
  email: { label: 'Email', pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, wrong: 'is not a valid e-mail address' },
};
const PERCENT_FIELDS = { cgst_per: 'CGST %', sgst_per: 'SGST %' };

export const hasRule = (key) => key in FIELD_RULES;

/** What the box keeps while typing: digits only / capitals and letters+digits only, cut to the field's length. */
export function cleanInput(key, value) {
  const rule = FIELD_RULES[key];
  if (!rule || typeof value !== 'string') return value;
  let text = value;
  if (rule.digits) text = text.replace(/\D/g, '');
  if (rule.alnum) text = text.replace(/[^A-Za-z0-9]/g, '');
  if (rule.upper) text = text.toUpperCase();
  return rule.max ? text.slice(0, rule.max) : text;
}

/** The message for a wrong value, or '' when it is fine (or blank). */
export function fieldProblem(key, value) {
  const text = String(value ?? '').trim();
  const rule = FIELD_RULES[key];
  if (rule && text) {
    const kept = rule.upper ? text.toUpperCase() : text;
    if (!rule.pattern.test(kept)) return `${rule.label} ${rule.wrong}.`;
  }
  if (key in PERCENT_FIELDS && text !== '') {
    const n = Number(text);
    if (!Number.isFinite(n) || n < 0 || n > 100) return `${PERCENT_FIELDS[key]} must be between 0 and 100.`;
  }
  return '';
}

/** {key: message} for every checked field of `values` that is wrong. */
export function problems(values) {
  const found = {};
  for (const key of [...Object.keys(FIELD_RULES), ...Object.keys(PERCENT_FIELDS)]) {
    if (key in values) {
      const message = fieldProblem(key, values[key]);
      if (message) found[key] = message;
    }
  }
  return found;
}
