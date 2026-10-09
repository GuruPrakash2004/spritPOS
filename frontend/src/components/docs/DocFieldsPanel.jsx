// Draws the header / footer / total fields of a document from useDocHeader: one titled block per section of the Doc
// Designer (Header, Footer, Total), nothing when the database has no such fields. Worked-out columns (amount in words,
// totals ...) are read-only and follow the formulas as lines and amounts change.

import DefinedExtras from '../masters/DefinedExtras';

const TITLES = { header: '', footer: 'Notes', total: 'Totals' };

export default function DocFieldsPanel({ doc, idPrefix = 'doc' }) {
  if (!doc.ready || (!doc.fields.length && !doc.changed)) return null;
  const sections = ['header', 'footer', 'total'].map(s => ({
    s, fields: doc.fields.filter(f => String(f.designerSection || 'Header').toLowerCase() === s),
  })).filter(x => x.fields.length);
  return (
    <div className="space-y-4">
      {sections.map(({ s, fields }) => (
        <div key={s} className="space-y-2">
          {TITLES[s] && <div className="text-[11px] font-bold uppercase tracking-wide text-stone-500 dark:text-stone-400">{TITLES[s]}</div>}
          <DefinedExtras fields={fields} form={doc.form} onChange={doc.setForm} errors={doc.errors} idPrefix={idPrefix} />
        </div>
      ))}
      {doc.changed && <p role="status" className="text-[11px] text-stone-500 dark:text-stone-400">Layout updated from the database. It applies to your next document.</p>}
    </div>
  );
}
