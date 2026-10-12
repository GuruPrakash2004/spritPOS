import { isSupported, operandValue } from './formulas.js';

// Turns a screen definition read from the database (GET /designer/screen/{id}, built from the Doc Designer's
// UtlDocDsgrFlds rows and the table's real columns) into the field configs the master screens draw, and evaluates the
// designer's conditional rules (hide / mandatory / read-only "when another field has a value").
//
// Only columns the web app has no typed field for become extra fields (`extra: true`); their values live in the row's
// `extras` object, which the server saves into the table's own columns. Nothing here is fixed: a column added in the
// database appears, one dropped disappears, and a changed caption, order, width or flag follows.

/** Compare a field's value with a rule's value the way the designer's operators do. */
export function ruleHolds(rule, values) {
  if (!rule) return false;
  // `compare` and `value` are operands: a field (#CODE#), a constant or an expression; an old-style rule has only `field`.
  const raw = rule.compare ? operandValue(rule.compare, values) : (rule.field ? values?.[rule.field] : undefined);
  const a = raw ?? '';
  const b = operandValue(rule.value ?? '', values) ?? '';
  const both = Number.isFinite(Number(a)) && Number.isFinite(Number(b)) && String(a).trim() !== '' && String(b).trim() !== '';
  const x = both ? Number(a) : String(a).trim().toLowerCase();
  const y = both ? Number(b) : String(b).trim().toLowerCase();
  switch (String(rule.op).trim()) {
    case '=': case '==': return x === y;
    case '<>': case '!=': return x !== y;
    case '>': return x > y;
    case '<': return x < y;
    case '>=': return x >= y;
    case '<=': return x <= y;
    default: return false;
  }
}

const KIND = { text: 'text', number: 'number', date: 'date', bool: 'bool', list: 'select' };

/** One database field -> the config MasterCrud draws. `width` (characters) decides how much room it asks for. */
export function toFieldConfig(f, docId = 0) {
  const lookup = f.combo && docId ? { docId, code: f.code } : undefined;   // a drop-down of the database's own rows
  const select = !lookup && (f.type === 'list' || (f.options && f.options.length > 0));
  return {
    key: f.code,
    label: f.caption || f.code,
    type: lookup ? 'lookup' : select ? 'select' : (KIND[f.type] ?? 'text'),
    lookup,
    options: select ? f.options : undefined,
    required: !!f.mandatory,
    section: f.sub_section ? '' : (f.standard ? 'Other details' : 'Additional details'),
    tab: f.sub_section || '',   // the designer's sub-section: a tab of its own (the first, nameless one is "Header Info")
    extra: true,                       // lives in row.extras, not in a typed field
    showInList: false,
    maxLength: f.width > 0 ? f.width : undefined,
    decimals: f.decimals,
    tooltip: f.tooltip || undefined,
    multiline: !!f.multiline || f.width > 120,
    wide: f.width > 60 || !!f.multiline,
    lockedAlways: !!f.readonly && !(f.formula && !isSupported(f.formula)),
    min: f.min ?? undefined,
    max: f.max ?? undefined,
    allowNegative: f.allow_negative,
    hideWhen: f.hide_when || undefined,
    mandatoryWhen: f.mandatory_when || undefined,
    readonlyWhen: f.readonly_when || undefined,
    default: f.use_default ? (f.default || undefined) : undefined,
    useDefault: !!f.use_default,
    defaultConditions: f.default_conditions?.length ? f.default_conditions : undefined,
    pickup: f.pickup || undefined,
    pickupAfter: f.pickup_after || undefined,
    unique: !!f.unique,
    column: f.code,
    order: f.order,
  };
}

/** The extra fields of a screen definition: designer or table columns the web app does not already model, not hidden. */
export function extraFields(screen) {
  if (!screen?.fields) return [];
  return screen.fields
    .filter(f => !f.modelled && !f.hidden && (f.section || 'Header').toLowerCase() !== 'detail')
    .sort((a, b) => a.order - b.order)
    .map(f => {
      const c = toFieldConfig(f, screen.doc_id);
      const colToKey = columnKeys(screen);
      return { ...c, hideWhen: rekey(c.hideWhen, colToKey), mandatoryWhen: rekey(c.mandatoryWhen, colToKey), readonlyWhen: rekey(c.readonlyWhen, colToKey) };
    });
}

// Fields every record needs to be created and found: the designer may rename or reorder them but never hide them.
const KEEP = new Set(['code', 'name']);

/**
 * (`hideUnlisted`: a typed field with no designer row is hidden too, for a screen the designer describes completely.)
 * The web app's own (typed) fields with the Doc Designer laid over them: its caption, mandatory / read-only flags,
 * hint, conditional rules and order win; a field the designer hides is flagged `designerHidden` (the screen does not
 * draw or check it, but still saves its value unchanged, so a required field can't make the save fail). The
 * pairing is the definition row's `web_key` (sent by the server, app/field_columns.py); a field the definition doesn't
 * know keeps the web app's own settings and place. Returns the same array while nothing changed.
 */
/** A rule whose `field` is a database column, re-pointed at the web key that holds that column's value. */
const rekey = (rule, colToKey) => {
  const key = rule && colToKey.get(String(rule.field).toLowerCase());
  return key ? { ...rule, field: key, compare: `#${key}#` } : rule;   // the rule reads the web's key for that column
};
const columnKeys = (screen) => new Map((screen?.fields ?? []).filter(d => d.web_key).map(d => [String(d.code).toLowerCase(), d.web_key]));

export function applyDefinition(typed, screen, { hideUnlisted = false } = {}) {
  if (!screen?.fields?.length) return typed;
  const byKey = new Map();
  for (const d of screen.fields) if (d.web_key && !byKey.has(d.web_key)) byKey.set(d.web_key, d);
  if (!byKey.size) return typed;
  const colToKey = columnKeys(screen);
  const over = typed.map(f => {
    const d = byKey.get(f.key);
    // With hideUnlisted (Company Master) a typed field the designer has no row for is not part of the desktop's screen: hidden.
    if (!d && hideUnlisted && !f.extra && !KEEP.has(f.key)) return { ...f, designerHidden: true };
    if (!d || f.extra || d.fk) return f;   // a key column's flags (hidden ID) don't describe the web's drop-down
    const keep = KEEP.has(f.key);
    return {
      ...f,
      label: d.caption || f.label,
      required: f.required || !!d.mandatory,
      section: d.sub_section || f.section,
      tooltip: d.tooltip || f.tooltip,
      lockedAlways: f.lockedAlways || (!keep && !!d.readonly),
      hideWhen: rekey(d.hide_when, colToKey) || f.hideWhen,
      mandatoryWhen: rekey(d.mandatory_when, colToKey) || f.mandatoryWhen,
      readonlyWhen: rekey(d.readonly_when, colToKey) || f.readonlyWhen,
      designerHidden: !keep && !!d.hidden,
      designerOrder: d.order,
    };
  });
  // Matched fields take the designer's order among themselves, in the places they already held.
  const slots = over.map((f, i) => (f.designerOrder != null ? i : -1)).filter(i => i >= 0);
  const sorted = slots.map(i => over[i]).sort((a, b) => a.designerOrder - b.designerOrder);
  slots.forEach((slot, n) => { over[slot] = sorted[n]; });
  return over;
}

/** The form value of a field, wherever it lives. */
export const valueOf = (form, f) => (f.extra ? form?.extras?.[f.key] : form?.[f.key]);

/** A form with the field set to a value. */
export const withValue = (form, f, v) => (f.extra ? { ...form, extras: { ...(form.extras ?? {}), [f.key]: v } } : { ...form, [f.key]: v });

/** The values a rule can look at: typed fields by key, extras by column, so a rule on any field works. */
export const ruleValues = (form) => ({ ...(form ?? {}), ...(form?.extras ?? {}) });

/** A problem with an extra field's value (or '' when fine): mandatory, number, range, length. */
export function extraProblem(f, form) {
  const v = valueOf(form, f);
  const text = String(v ?? '').trim();
  const values = ruleValues(form);
  if (f.hideWhen && ruleHolds(f.hideWhen, values)) return '';
  const required = f.required || (f.mandatoryWhen && ruleHolds(f.mandatoryWhen, values));
  if (required && text === '') return `Enter the ${f.label}`;
  if (text === '') return '';
  if (f.type === 'number') {
    const n = Number(text);
    if (!Number.isFinite(n)) return `${f.label} must be a number`;
    if (f.allowNegative === false && n < 0) return `${f.label} cannot be negative`;
    if (f.min != null && f.min !== '' && n < Number(f.min)) return `${f.label} must be at least ${f.min}`;
    if (f.max != null && f.max !== '' && n > Number(f.max)) return `${f.label} must be at most ${f.max}`;
  }
  if (f.type === 'text' && f.maxLength && text.length > f.maxLength) return `${f.label} can be at most ${f.maxLength} characters`;
  return '';
}

const FK = (f) => f.fk === true;

/** The typed field of a master, drawn the way the designer describes the column it is stored in. `w` is the master's
 *  request-model field (kind of value, required, options, hints) the server sends with the definition. */
function typedConfig(f, w, colToKey) {
  const options = w.options?.length ? w.options : (f.options?.length ? f.options : undefined);
  const master = f.combo?.master;
  const keep = KEEP.has(w.key);
  return {
    key: w.key,
    label: (FK(f) && String(f.caption).toLowerCase() === String(f.code).toLowerCase()) ? (w.caption || humanize(w.key)) : (f.caption || w.caption || w.key),
    type: w.kind === 'bool' ? 'bool' : w.kind === 'number' ? 'number' : w.kind === 'lines' ? 'lines' : (options && !master && !w.options_from ? 'select' : 'text'),
    options: master || w.options_from ? undefined : options,
    optionsFrom: w.options_from || master || undefined,
    required: !!w.required || !!f.mandatory,
    maxLength: w.kind === 'text' && f.width > 0 ? f.width : undefined,
    decimals: w.kind === 'number' ? f.decimals : undefined,
    tooltip: f.tooltip || undefined,
    multiline: !!f.multiline,
    lockedAlways: !keep && !FK(f) && !!f.readonly && !(f.formula && !isSupported(f.formula)),
    designerHidden: !keep && !FK(f) && !w.required && !!f.hidden,   // a field the record can't be saved without stays on screen
    hideWhen: rekey(f.hide_when, colToKey) || undefined,
    mandatoryWhen: rekey(f.mandatory_when, colToKey) || undefined,
    readonlyWhen: rekey(f.readonly_when, colToKey) || undefined,
    default: w.default ?? (f.use_default ? (f.default || undefined) : undefined),
    useDefault: !!f.use_default,
    defaultConditions: f.default_conditions?.length ? f.default_conditions : undefined,
    pickup: f.pickup || undefined,
    pickupAfter: f.pickup_after || undefined,
    unique: !!f.unique,
    column: f.code,
    costField: w.cost || undefined,
    newOnly: w.new_only || undefined,
    tab: f.sub_section || '',
    order: f.order,
  };
}

/** A typed field the designer has no row for (the opening stock, loyalty points): drawn after the others. */
function webOnlyConfig(w) {
  return {
    key: w.key, label: w.caption || humanize(w.key), type: w.kind === 'bool' ? 'bool' : w.kind === 'number' ? 'number' : w.kind === 'lines' ? 'lines' : (w.options?.length ? 'select' : 'text'),
    options: w.options?.length ? w.options : undefined, required: !!w.required, default: w.default ?? undefined,
    costField: w.cost || undefined, newOnly: w.new_only || undefined, order: w.key === 'code' ? -10000 : 10000,
  };
}

const humanize = (key) => key.replace(/_/g, ' ').replace(/(^|\s)\w/g, c => c.toUpperCase());

/**
 * The whole field list of a master, built from the database's definition and nothing else: the designer's fields in its
 * order (standard columns first, then the ones it added), each with its caption and flags, a column the web app writes as
 * a typed field keeping its key, any other column an `extra` (saved into the table's own column); then the typed fields the
 * designer has no row for. Hidden columns are not drawn (a hidden typed one is still sent as it is, so a required value
 * can't be lost); a hidden key column (the item group) is the web's drop-down, so it stays. null without a definition.
 */
export function fieldsFromDefinition(screen) {
  if (!screen?.fields?.length || !screen.web_fields) return null;
  const typed = new Map(screen.web_fields.map(w => [w.key, w]));
  const colToKey = columnKeys(screen);
  const used = new Set();
  const out = [];
  const rows = screen.fields.filter(f => String(f.section || 'Header').toLowerCase() !== 'detail').sort((a, b) => a.order - b.order);
  for (const f of rows) {
    const w = f.web_key ? typed.get(f.web_key) : null;
    if (w && !used.has(w.key)) {
      used.add(w.key);
      out.push(typedConfig(f, w, colToKey));
    } else if (!w && !f.modelled && !f.hidden) {
      const c = toFieldConfig(f, screen.doc_id);
      out.push({ ...c, hideWhen: rekey(c.hideWhen, colToKey), mandatoryWhen: rekey(c.mandatoryWhen, colToKey), readonlyWhen: rekey(c.readonlyWhen, colToKey) });
    }
  }
  for (const w of screen.web_fields) if (!used.has(w.key)) out.push(webOnlyConfig(w));
  return out.map((f, i) => ({ ...f, showInList: i < 4 && !f.designerHidden && f.type !== 'bool' ? undefined : false }));
}

const ISO = /^(\d{2})\/(\d{2})\/(\d{4})$/;

/**
 * A new record's starting value for a field (vLQMS.exe fillVsfHFTDefValues): the designer's default when the field uses one;
 * with "default against condition" the first matching condition's value (conditions over the other fields typed so far);
 * SVTCURDATE, SVTUSERCODE, #FIELD# ... are replaced as in a condition. A date comes back as yyyy-mm-dd for the date box.
 */
export function defaultFor(f, values = {}, system = {}) {
  let raw = f.default;
  for (const c of f.defaultConditions ?? []) {
    if (ruleHolds({ compare: c.compare, op: c.op, value: c.with }, values)) { raw = c.value; break; }
  }
  if (raw === undefined || raw === null || raw === '') return undefined;
  const v = operandValue(raw, values, system);
  const m = typeof v === 'string' ? ISO.exec(v) : null;
  return m ? `${m[3]}-${m[2]}-${m[1]}` : v;
}

/** The fields whose pick-up SQL must run now that `changed` was edited (their FldPickUPAfterFld is its column). */
export const pickupsAfter = (fields, changed) => fields.filter(f => f.pickup && f.pickupAfter
  && String(f.pickupAfter).replace(/#/g, '').toLowerCase() === String(changed.column ?? changed.key).toLowerCase());
