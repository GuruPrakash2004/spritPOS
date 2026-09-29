// Shared item-line grid (cases + bottles → qty, rate, amount, litres) for the stock transactions:
// Purchase Indent, Sales Return, Stock Deduction. The screen supplies its own header fields and turns
// the lines into the request body.

import { useEffect, useState, type ReactNode } from 'react';
import { PlusCircle, Save, Trash2, XCircle, Printer } from 'lucide-react';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { api, errorText, type ApiDoc, type ApiItemFull, type ApiInvoice } from '../../lib/api';
import { field, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader, printButton } from '../../lib/styles';
import PrintFormatSelectorModal from '../print/PrintFormatSelectorModal';
import { type DocumentCategory } from '../print/rptRegistry';

export type EntryLine = { item_code: string; cases: number; bottles: number; rate: number };

const blankLine = (): EntryLine => ({ item_code: '', cases: 0, bottles: 0, rate: 0 });

// Shared cases + bottles grid for the stock transactions (Purchase Indent, Sales Return, Stock Deduction).
// The screen supplies its own header fields and turns the lines into the request body.
export default function ItemLinesEntry({ title, subtitle, endpoint, rateField, rateLabel, header, buildBody }: {
  title: string;
  subtitle: string;
  endpoint: string;
  rateField: 'mrp' | 'purchase_rate';
  rateLabel: string;
  header: ReactNode;
  buildBody: (lines: EntryLine[]) => object;
}) {
  const [items, setItems] = useState<ApiItemFull[]>([]);
  const [lines, setLines] = useState<EntryLine[]>([blankLine()]);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [printDoc, setPrintDoc] = useState<ApiInvoice | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => {
    api<ApiItemFull[]>('/masters/items').then(list => setItems(list.filter(i => i.active))).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  }, []);

  const byCode = new Map(items.map(i => [i.code, i]));
  const qty = (l: EntryLine) => l.cases * (byCode.get(l.item_code)?.bottles_per_case ?? 0) + l.bottles;
  const filled = lines.filter(l => l.item_code);
  const total = filled.reduce((acc, l) => acc + qty(l) * l.rate, 0);

  const update = (index: number, patch: Partial<EntryLine>) => setLines(lines.map((l, i) => {
    if (i !== index) return l;
    const next = { ...l, ...patch };
    // Picking an item fills in its default rate.
    if (patch.item_code !== undefined) next.rate = byCode.get(patch.item_code)?.[rateField] ?? 0;
    return next;
  }));

  const category: DocumentCategory = title.includes('Indent')
    ? 'PURCHASE_INDENT'
    : title.includes('Return')
    ? 'SALES_RETURN'
    : title.includes('Deduction')
    ? 'STOCK_DEDUCTION'
    : 'PURCHASE_BILL';

  const makeDoc = (docNo: string): ApiInvoice => ({
    doc_no: docNo,
    date: new Date().toISOString().slice(0, 10),
    time: new Date().toLocaleTimeString('en-IN'),
    counter: 'Main Store',
    customer: title,
    salesperson: 'Store Manager',
    status: 'SAVED',
    lines: filled.map(l => {
      const it = byCode.get(l.item_code);
      const q = qty(l);
      return {
        item_code: l.item_code,
        item_name: it?.name || l.item_code,
        cases: l.cases,
        bottles: l.bottles,
        qty: q,
        rate: l.rate,
        amount: q * l.rate,
        litres: q * 0.75,
      };
    }),
    totals: {
      qty: filled.reduce((acc, l) => acc + qty(l), 0),
      litres: 0,
      basic: total,
      cgst: 0,
      sgst: 0,
      discount: 0,
      points_redeemed: 0,
      points_earned: 0,
      net: total,
    },
    payment: { cash: total, card: 0, upi: 0, credit: 0 },
  });

  const previewPrint = () => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one item to preview print.' });
    setPrintDoc(makeDoc('DRAFT-01'));
  };

  const save = async (andPrint = false) => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one item.' });
    if (filled.some(l => qty(l) <= 0)) return setToast({ kind: 'error', text: 'Every item needs cases or bottles.' });
    if (!(await toastStore.defer('ok', `${title} with ${filled.length} items will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    api<ApiDoc>(endpoint, { method: 'POST', body: buildBody(filled) })
      .then(doc => {
        setToast({ kind: 'ok', text: `${title} ${doc.doc_no} saved — ${doc.totals.qty} bottles.` });
        if (andPrint) {
          setPrintDoc(makeDoc(doc.doc_no));
        }
        setLines([blankLine()]);
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>{title}</h1>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex flex-wrap gap-2">
          <button onClick={() => save(true)} className={printButton}><Printer size={14} />Save & Print</button>
          <button onClick={() => save(false)} className={primaryButton}><Save size={14} />Save</button>
          <button onClick={previewPrint} disabled={!filled.length} className={secondaryButton}><Printer size={14} />Print Preview</button>
          <button onClick={() => setLines([blankLine()])} className={secondaryButton}><XCircle size={14} />Clear</button>
        </div>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>{header}</div>

      <div className={`${panel} overflow-hidden`}>
        <div className="overflow-x-auto">
        <table className="w-full min-w-[720px] text-xs">
          <thead className={tableHead}>
            <tr>
              <th className="px-3 py-2">Sl</th><th className="px-3 py-2">Item</th><th className="px-3 py-2 text-right">QOH</th>
              <th className="px-3 py-2 text-right">Cases</th><th className="px-3 py-2 text-right">Bottles</th><th className="px-3 py-2 text-right">Qty</th>
              <th className="px-3 py-2 text-right">{rateLabel}</th><th className="px-3 py-2 text-right">Amount</th><th className="px-3 py-2" />
            </tr>
          </thead>
          <tbody className={`${tableBody} tabular-nums`}>
            {lines.map((l, i) => {
              const item = byCode.get(l.item_code);
              return (
                <tr key={i}>
                  <td className="px-3 py-1.5 font-sans text-stone-400">{i + 1}</td>
                  <td className="px-3 py-1.5">
                    <select value={l.item_code} onChange={e => update(i, { item_code: e.target.value })} className={`${field} w-full max-w-sm font-sans`} aria-label={`Item row ${i + 1}`}>
                      <option value="">— pick an item —</option>
                      {items.map(it => <option key={it.code} value={it.code}>{it.code} • {it.name} {it.package}</option>)}
                    </select>
                  </td>
                  <td className="px-3 py-1.5 text-right text-stone-500">{item?.qoh ?? '—'}</td>
                  <td className="px-3 py-1.5 text-right"><input type="number" min={0} value={l.cases || ''} placeholder="0" onChange={e => update(i, { cases: Math.max(0, Math.floor(Number(e.target.value)) || 0) })} className={`${field} w-16 py-0.5 text-right`} aria-label={`Cases row ${i + 1}`} /></td>
                  <td className="px-3 py-1.5 text-right"><input type="number" min={0} value={l.bottles || ''} placeholder="0" onChange={e => update(i, { bottles: Math.max(0, Math.floor(Number(e.target.value)) || 0) })} className={`${field} w-16 py-0.5 text-right`} aria-label={`Bottles row ${i + 1}`} /></td>
                  <td className="px-3 py-1.5 text-right font-bold">{l.item_code ? qty(l) : '—'}</td>
                  <td className="px-3 py-1.5 text-right"><input type="number" min={0} step="0.01" value={l.rate || ''} onChange={e => update(i, { rate: Math.max(0, Number(e.target.value) || 0) })} className={`${field} w-24 py-0.5 text-right`} aria-label={`Rate row ${i + 1}`} /></td>
                  <td className="px-3 py-1.5 text-right">{money(qty(l) * l.rate)}</td>
                  <td className="px-3 py-1.5 text-center">
                    <button onClick={() => setLines(lines.length > 1 ? lines.filter((_, x) => x !== i) : [blankLine()])} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
        </div>
        <div className="flex flex-wrap items-center justify-between gap-2 border-t border-line px-3 py-2">
          <button onClick={() => setLines([...lines, blankLine()])} className={secondaryButton}><PlusCircle size={14} />Add Row</button>
          <span className="text-sm font-bold text-accent-ink">{filled.length} items • ₹ {money(total)}</span>
        </div>
      </div>

      {printDoc && (
        <PrintFormatSelectorModal
          category={category}
          data={printDoc}
          title={`Print ${title} (${printDoc.doc_no})`}
          onClose={() => setPrintDoc(null)}
        />
      )}
    </div>
  );
}
