// Shared item-line grid (cases + bottles → qty, rate, amount, litres) for the stock transactions:
// Purchase Indent, Sales Return, Stock Deduction. The screen supplies its own header fields and turns
// the lines into the request body.
// Keyboard: Enter moves to the next field (Shift+Enter back) and, on the last field, adds a row; Alt+A adds a
// row; ⌘/Ctrl+S saves; F10 saves and prints.

import { useEffect, useRef, useState, } from 'react';
import Kbd from '../common/Kbd';
import { enterMovesNext, useShortcuts } from '../../lib/keys';
import { PlusCircle, Save, Trash2, XCircle, Printer } from 'lucide-react';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { errorText, getItems, saveStockDocument, } from '../../lib/api';
import { field, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead, pageHeader, printButton } from '../../lib/styles';
import PrintDialog from '../print/PrintDialog';
import { afterSavePlan, } from '../../lib/printing';
import { useConfirm } from '../common/ConfirmDialog';
import { useDocSettings, } from '../../lib/docSettings';
import WithHistory from './DocHistoryPanel';
import DocFieldsPanel from '../docs/DocFieldsPanel';
import { useDocHeader } from '../docs/useDocHeader';
import { useColumnTone } from '../../hooks/useColumnTone';
import { GRID_OUTLINE, useResizableTable } from '../../hooks/useResizableTable';
import { cn } from '../../lib/utils';

// The desktop header table of each document, so the columns the database / Doc Designer added to it show here.
const HEADER_TABLE = { purchase_indent: 'TrnHdrPI', sales_return: 'TrnHdrSR', stock_deduction: 'TrnHdrSTKDED' };
// Header keys the screen draws itself (the document's own date / party), so the designer's panel leaves them out.
// The designer's Detail columns behind the grid's Qty / Rate / Amount headings (red = mandatory, blue = read-only).
const DETAIL = { purchase_indent: { qty: 'PIQTY', rate: 'PIRATE', amount: 'PIAMT' }, sales_return: { qty: 'SRQTY', rate: 'SRRATE', amount: 'SRAMT' },
  stock_deduction: { qty: 'STKDEDQTY', rate: '', amount: '' } };
const OWN = { purchase_indent: ['date', 'supplier'], sales_return: ['date'], stock_deduction: ['date'] };

 

const blankLine = () => ({ item_code: '', cases: 0, bottles: 0, rate: 0 });

// Shared cases + bottles grid for the stock transactions (Purchase Indent, Sales Return, Stock Deduction).
// The screen supplies its own header fields and turns the lines into the request body.
export default function ItemLinesEntry({ title, subtitle, endpoint, rateField, rateLabel, header, buildBody }







) {
  const [items, setItems] = useState([]);
  const [lines, setLines] = useState([blankLine()]);
  const [toast, setToast] = useState(null);
  const [printDocNo, setPrintDocNo] = useState(null);
  // Print Settings for this document: print after save (none / ask / preview) and the default format.
  const kind = endpoint.replace('/transactions/', '').replace(/-/g, '_') ;
  const docSettings = useDocSettings(kind);
  const { confirm, dialog: confirmDialog } = useConfirm();
  // A line with an item asks first; a blank line goes without asking.
  const removeLine = async (i) => {
    const l = lines[i];
    if ((l.item_code || l.cases || l.bottles) && !(await confirm(`Are you sure you want to delete the item '${items.find(x => x.code === l.item_code)?.name ?? l.item_code}'?`))) return;
    setLines(cur => (cur.length > 1 ? cur.filter((_, x) => x !== i) : [blankLine()]));
  };
  const gridRef = useRef(null);
  const focusRow = useRef(null);   // a row just added gets the cursor in its item box

  useEffect(() => {
    if (focusRow.current === null) return;
    gridRef.current?.querySelector(`[aria-label="Item row ${focusRow.current + 1}"]`)?.focus();
    focusRow.current = null;
  }, [lines.length]);

  const addRow = () => { focusRow.current = lines.length; setLines([...lines, blankLine()]); };

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => {
    getItems().then(list => setItems(list.filter(i => i.active))).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  }, []);

  const byCode = new Map(items.map(i => [i.code, i]));
  const qty = (l) => l.cases * (byCode.get(l.item_code)?.bottles_per_case ?? 0) + l.bottles;
  const filled = lines.filter(l => l.item_code);
  const docLines = filled.map(l => ({ item_code: l.item_code, qty: qty(l), rate: l.rate, cases: l.cases, bottles: l.bottles }));
  const { ref: tableRef, onMouseDown: gridDown, onDoubleClick: gridDouble } = useResizableTable(`lines-${kind}`);   // stretchable columns, remembered on this computer
  const tone = useColumnTone(HEADER_TABLE[kind] ?? '');
  const cols = DETAIL[kind] ?? {};
  const doc = useDocHeader(HEADER_TABLE[kind] ?? '', { skip: OWN[kind] ?? [], lines: docLines, itemOf: code => byCode.get(code), idPrefix: 'ile' });
  const total = filled.reduce((acc, l) => acc + qty(l) * l.rate, 0);

  const update = (index, patch) => setLines(lines.map((l, i) => {
    if (i !== index) return l;
    const next = { ...l, ...patch };
    // Picking an item fills in its default rate.
    if (patch.item_code !== undefined) next.rate = byCode.get(patch.item_code)?.[rateField] ?? 0;
    return next;
  }));

  const save = async (andPrint = false) => {
    if (!filled.length) return setToast({ kind: 'error', text: 'Add at least one item.' });
    if (filled.some(l => qty(l) <= 0)) return setToast({ kind: 'error', text: 'Every item needs cases or bottles.' });
    if (!doc.check()) return setToast({ kind: 'error', text: 'Fill the additional details marked in red.' });
    if (!(await toastStore.defer('ok', `${title} with ${filled.length} items will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveStockDocument(endpoint, { ...buildBody(filled), ...doc.body() })
      .then(saved => {
        setToast({ kind: 'ok', text: `${title} ${saved.doc_no} saved — ${saved.totals.qty} bottles.` });
        // The Doc Designer's default print option decides (Global Settings is the fallback).
        afterSavePlan(kind, docSettings.after_save, andPrint).then(({ plan }) => {
          if (plan === 'dialog' || plan === 'direct') setPrintDocNo(saved.doc_no);
          else if (plan === 'ask') confirm(`Do you want to print ${title} ${saved.doc_no}?`).then(ok => { if (ok) setPrintDocNo(saved.doc_no); });
        });
        setLines([blankLine()]);
        doc.refresh();
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  useShortcuts({
    'Mod+S': () => { void save(false); },
    F10: () => { void save(true); },
    'Alt+A': addRow,
  });

  return (
    <WithHistory kind={kind} title="documents">
    <div className="mx-auto flex max-w-6xl flex-col gap-4" onKeyDown={e => enterMovesNext(e, addRow)}>
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {confirmDialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>{title}</h1>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex flex-wrap gap-2">
          <button onClick={() => save(true)} className={printButton}><Printer size={14} />Save & Print<Kbd combo="F10" /></button>
          <button onClick={() => save(false)} className={primaryButton}><Save size={14} />Save<Kbd combo="Mod+S" /></button>
          <button onClick={() => setLines([blankLine()])} className={secondaryButton}><XCircle size={14} />Clear</button>
        </div>
      </div>

      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>{header}</div>
      {doc.ready && (doc.fields.length > 0 || doc.changed) && <div className={`${panel} px-5 py-3`}><DocFieldsPanel doc={doc} idPrefix="ile" /></div>}

      <div className={`${panel} overflow-hidden`}>
        <div className="overflow-x-auto">
        <table ref={tableRef} onMouseDown={gridDown} onDoubleClick={gridDouble} className={`${GRID_OUTLINE} w-full min-w-[720px] text-xs`}>
          <thead className={tableHead}>
            <tr>
              <th className="px-3 py-2">Sl</th><th className="px-3 py-2">Item</th><th className="px-3 py-2 text-right">QOH</th>
              <th className="px-3 py-2 text-right">Cases</th><th className="px-3 py-2 text-right">Bottles</th><th className={cn('px-3 py-2 text-right', tone(cols.qty))}>Qty</th>
              <th className={cn('px-3 py-2 text-right', tone(cols.rate))}>{rateLabel}</th><th className={cn('px-3 py-2 text-right', tone(cols.amount))}>Amount</th>
            </tr>
          </thead>
          <tbody ref={gridRef} className={`${tableBody} tabular-nums`}>
            {lines.map((l, i) => {
              const item = byCode.get(l.item_code);
              return (
                <tr key={i}>
                  <td className="whitespace-nowrap px-3 py-1.5 font-sans text-stone-400"><span className="inline-flex items-center gap-1.5"><button onClick={() => void removeLine(i)} tabIndex={-1} className="text-stone-400 hover:text-red-500" aria-label={`Remove row ${i + 1}`}><Trash2 size={13} /></button>{i + 1}</span></td>
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
                </tr>
              );
            })}
          </tbody>
        </table>
        </div>
        <div className="flex flex-wrap items-center justify-between gap-2 border-t border-line px-3 py-2">
          <button onClick={addRow} className={secondaryButton}><PlusCircle size={14} />Add Row<Kbd combo="Alt+A" /></button>
          <span className="text-sm font-bold text-accent-ink">{filled.length} items • ₹ {money(total)}</span>
        </div>
      </div>

      {printDocNo && <PrintDialog kind={kind} docNo={printDocNo} title={`Print ${title} (${printDocNo})`} onClose={() => setPrintDocNo(null)} />}
    </div>
    </WithHistory>
  );
}
