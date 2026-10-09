// The extras block of a bill (see useBillExtras.js): nothing when the database added no header column.

import DefinedExtras from '../masters/DefinedExtras';

export default function BillExtras({ bill, idPrefix, title = 'Additional details' }) {
  if (!bill.fields.length && !bill.changed) return null;
  return (
    <div className="col-span-full space-y-2">
      {bill.fields.length > 0 && (
        <>
          <div className="text-[11px] font-bold uppercase tracking-wide text-stone-500 dark:text-stone-400">{title}</div>
          <DefinedExtras fields={bill.fields} form={bill.form} onChange={bill.setForm} errors={bill.errors} idPrefix={idPrefix} />
        </>
      )}
      {bill.changed && <p role="status" className="text-[11px] text-stone-500 dark:text-stone-400">Layout updated from the database. It applies to your next bill.</p>}
    </div>
  );
}
