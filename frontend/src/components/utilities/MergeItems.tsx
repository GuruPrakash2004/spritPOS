// Utilities → Merge Items (legacy frmUtlMergeItems, 713) and Replace and Remove Duplicate items
// (frmTrnRemoveDuplicates, 712). One screen, two modes, as the desktop forms differ only in how items are
// picked: Merge Items offers active items by code ("Merge Item" → "To Item"); Replace and Remove offers every
// item by name ("Item to Remove" → "Item to Replace With").
//
// Once both are picked, the preview shows each item's QOH, the document lines that will move and the QOH
// after. Merge asks first, then waits for its popup: every bill line and stock movement moves to the kept
// item, the two QOHs are added, and the other item is removed (backend app/item_merge.py). The desktop
// database is not changed.

import { useEffect, useMemo, useState } from 'react';
import { ArrowRight, GitMerge } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import { useAdminPassword } from '../common/AdminPasswordPrompt';
import { encryptSecret } from '../../lib/secureField';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { api, errorText, type ApiItem, type ApiMergePreview } from '../../lib/api';
import { loadStockItems } from '../../data/stockData';
import { dangerButton, field, fieldError, label, pageHeader, pageSub, pageTitle, panel, sectionTitle } from '../../lib/styles';

type Mode = 'merge' | 'replace';
const TEXT: Record<Mode, { title: string; sub: string; from: string; to: string; button: string }> = {
  merge: { title: 'Merge Items', sub: 'Fold one item into another: its bills, stock movements and stock move to the item you keep, then it is removed.',
    from: 'Merge Item', to: 'To Item', button: 'Merge Item' },
  replace: { title: 'Replace and Remove Duplicate items', sub: 'Replace an item created twice with the right one: its bills and stock move to the item you keep, then it is removed.',
    from: 'Item to Remove', to: 'Item to Replace With', button: 'Merge and Remove First item' },
};
const DOC_NAMES: Record<string, string> = {
  sales_invoice: 'Sales invoices', purchase_bill: 'Purchase bills', purchase_indent: 'Purchase indents', sales_return: 'Sales returns',
  stock_deduction: 'Stock deductions', closing_stock: 'Closing stock entries', expense: 'Expenses',
};

export default function MergeItems({ mode }: { mode: Mode }) {
  const t = TEXT[mode];
  const [items, setItems] = useState<ApiItem[]>([]);
  const [fromText, setFromText] = useState('');
  const [toText, setToText] = useState('');
  const [preview, setPreview] = useState<ApiMergePreview | null>(null);
  const [errors, setErrors] = useState<{ from?: string; to?: string }>({});
  const [working, setWorking] = useState(false);
  const { confirm, dialog } = useConfirm();
  const { ask, dialog: passwordDialog } = useAdminPassword();

  const loadItems = () => api<ApiItem[]>('/masters/items').then(setItems).catch(e => toastStore.add('error', errorText(e)));
  useEffect(() => { loadItems(); }, []);

  const choices = useMemo(() => items.filter(i => mode === 'replace' || i.active !== false), [items, mode]);
  const labelOf = (i: ApiItem) => (mode === 'merge' ? `${i.code} — ${i.name}` : `${i.name} (${i.code})`);
  const pick = (text: string) => choices.find(i => labelOf(i) === text || i.code === text.trim())?.code;
  const fromCode = pick(fromText);
  const toCode = pick(toText);

  useEffect(() => {
    if (!fromCode || !toCode || fromCode === toCode) return;
    let live = true;
    api<ApiMergePreview>(`/utilities/merge-items/${mode}/preview?${new URLSearchParams({ from_item: fromCode, to_item: toCode })}`)
      .then(p => { if (live) setPreview(p); })
      .catch(e => { if (live) { setPreview(null); toastStore.add('error', errorText(e)); } });
    return () => { live = false; };
  }, [fromCode, toCode, mode]);
  const shown = preview && preview.from.code === fromCode && preview.to.code === toCode ? preview : null;

  const merge = async () => {
    const errs: typeof errors = {};
    if (!fromCode) errs.from = mode === 'merge' ? 'Select Merge Item...' : 'Please Select Item';
    if (!toCode) errs.to = mode === 'merge' ? 'Select To Item...' : 'Please Select Item';
    else if (fromCode === toCode) errs.to = 'Pick a different item from the one being removed';
    setErrors(errs);
    if (Object.keys(errs).length) {
      document.getElementById(errs.from ? 'mi-from' : 'mi-to')?.focus();
      return;
    }
    const body = { from_item: fromCode!, to_item: toCode! };
    if (!(await confirm(`${body.from_item} will be merged into ${body.to_item} and then removed. Its bills and stock move to ${body.to_item}. This can't be undone. Continue?`))) return;
    const password = await ask();
    if (password === null) return;
    if (!(await toastStore.defer('delete', `${body.from_item} will be merged into ${body.to_item} when this closes.`, 'Merge cancelled. Nothing was changed.'))) return;
    setWorking(true);
    try {
      const admin_password = await encryptSecret(password, 'admin');   // leaves the browser only encrypted
      const res = await api<ApiMergePreview>(`/utilities/merge-items/${mode}`, { method: 'POST', body: { ...body, admin_password } });
      toastStore.add('ok', `${res.message} ${body.to_item} now has ${res.qoh_after} in stock.`);
      setFromText('');
      setToText('');
      setPreview(null);
      loadItems();
      loadStockItems().catch(() => undefined);   // Sales Invoice / Purchase Bill stop offering the removed item
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setWorking(false);
    }
  };

  const side = (id: 'from' | 'to', text: string, set: (v: string) => void, caption: string) => (
    <div className="flex-1">
      <label htmlFor={`mi-${id}`} className={label}>{caption}</label>
      <input id={`mi-${id}`} list="mi-items" value={text} onChange={e => { set(e.target.value); setErrors(x => ({ ...x, [id]: undefined })); }}
        className={`${field} ${errors[id] ? fieldError : ''}`} aria-invalid={!!errors[id]}
        placeholder={mode === 'merge' ? 'Item code or name' : 'Item name'} />
      <InlineError message={errors[id]} />
    </div>
  );

  return (
    <div className="mx-auto flex max-w-4xl flex-col gap-4">
      {dialog}
      {passwordDialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>{t.title}</h1>
          <p className={pageSub}>{t.sub}</p>
        </div>
      </div>
      <section className={`${panel} p-5`}>
        <div className="flex flex-col gap-3 sm:flex-row sm:items-start">
          {side('from', fromText, setFromText, t.from)}
          <ArrowRight className="mx-auto mt-2 shrink-0 text-stone-400 sm:mt-8" size={18} />
          {side('to', toText, setToText, t.to)}
        </div>
        <datalist id="mi-items">{choices.map(i => <option key={i.code} value={labelOf(i)} />)}</datalist>

        {shown && (
          <div className="mt-4 rounded-xl border border-[#E3D9CD] p-4 text-xs dark:border-line" aria-label="Merge preview">
            <h2 className={sectionTitle}>What will happen</h2>
            <ul className="mt-2 space-y-1">
              <li><b>{shown.from.code}</b> {shown.from.name}: QOH {shown.from.qoh} — will be removed</li>
              <li><b>{shown.to.code}</b> {shown.to.name}: QOH {shown.to.qoh} → <b>{shown.qoh_after}</b></li>
              {Object.entries(shown.lines).map(([k, n]) => <li key={k}>{DOC_NAMES[k] ?? k}: {n} line(s) move to {shown.to.code}</li>)}
              {!Object.keys(shown.lines).length && <li>No bill lines to move.</li>}
              <li>{shown.movements} stock movement(s) move to {shown.to.code}</li>
            </ul>
          </div>
        )}
        <div className="mt-4 flex justify-end">
          <button className={dangerButton} onClick={merge} disabled={working}><GitMerge size={14} />{working ? 'Merging…' : t.button}</button>
        </div>
      </section>
    </div>
  );
}
