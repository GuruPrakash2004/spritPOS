// Package Master layout: Code and Name at the top, then an "Other details" section below them that holds Litres per Bottle and the
// database's other standard columns; the Active box stays last. The screen follows the database's definition (lib/designerFields.js), so this
// only re-orders and re-sections what it is given; nothing is added or dropped.

const isLitres = (f) => f.key === 'litres_per_bottle';
// the code (the web's `code`, stored in PKGNAME) and the name (PKGCODE / PKGNAME as the designer lists them)
const isCodeOrName = (f) => f.key === 'code' || /^pkg(code|name)$/i.test(String(f.column ?? f.key));

export function arrangePackageFields(fields) {
  const top = fields.filter(isCodeOrName).sort((a, b) => (a.key === 'code' ? 0 : 1) - (b.key === 'code' ? 0 : 1) || (a.order ?? 0) - (b.order ?? 0));
  const litres = fields.filter(isLitres).map(f => ({ ...f, section: 'Other details' }));
  const taken = new Set([...top, ...fields.filter(isLitres)]);
  const rest = fields.filter(f => !taken.has(f));
  const otherDetails = rest.filter(f => f.section === 'Other details');
  const others = rest.filter(f => f.section !== 'Other details');
  return [...top, ...litres, ...otherDetails, ...others];
}
