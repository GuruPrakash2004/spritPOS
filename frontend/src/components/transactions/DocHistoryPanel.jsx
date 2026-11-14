// A collapsible "History" column on the right of the document entry screens (Sales Invoice, Purchase Bill, Purchase
// Indent, Sales Return, Stock Deduction, Expense Entry): the documents made before, newest first, so the clerk can
// check the last bills without leaving the screen.
//
//   <WithHistory kind="sales_invoice" title="Sales invoice"> …the screen… </WithHistory>
//
// - Open or closed with its button, the tab on the right edge, or Alt+H; the choice is remembered (localStorage) and,
//   on screens 1536 px wide and up, an open panel sits beside the screen. On narrower screens it slides over the
//   screen as a drawer and starts closed, so it never squeezes a billing screen.
// - Lists the latest documents of the kind (GET /transactions/<kind>), with a search box. A row opens to show its lines
//   and a Print button (the same print window as after a save). Cancelled documents are struck through; held bills are
//   not listed (Held bills has its own list). Money the role may not see (purchase cost) shows as —.
// - Refreshes itself when a document is saved or cancelled anywhere in the app (lib/api.js sends 'vlqms-docs-changed').

import { useCallback, useEffect, useMemo, useState } from 'react';
import { Ban, ChevronDown, History, Printer, RefreshCw, Search, X } from 'lucide-react';
import PrintDialog from '../print/PrintDialog';
import ActionBar, { ScreenFrame } from '../common/ActionBar';
import ListModal from '../common/ListModal';
import DocViewDialog from './DocViewDialog';
import { goHome } from '../../lib/nav';
import { badge, iconButton, money, panel } from '../../lib/styles';
import { DOCS_CHANGED, canDo, cancelSavedDocument, errorText, listDocuments } from '../../lib/api';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { getPurchaseDays, setPurchaseDays } from '../../lib/localPrefs';
import { toast } from '../common/toastStore';
import { CancelInvoiceDialog } from './SalesInvoiceDialogs';
import { useShortcuts } from '../../lib/keys';
import { useScreenFields } from '../../hooks/useScreenFields';
import AttachmentsDialog from '../masters/AttachmentsDialog';
import Kbd from '../common/Kbd';

const KEY = 'vlqms_history_open';
const SHOWN = 60;   // the latest documents listed
const wide = () => (typeof window !== 'undefined' && window.matchMedia?.('(min-width: 1536px)').matches) || false;
const initiallyOpen = () => {
  if (!wide()) return false;
  // With no choice remembered it opens by itself only on a really wide window (1760px+); below that the bill gets the whole width and the
  // right-edge History tab / Alt+H opens it.
  const roomy = window.matchMedia?.('(min-width: 1760px)').matches ?? false;
  try { const saved = localStorage.getItem(KEY); return saved === null ? roomy : saved === '1'; } catch { return roomy; }
};

// The screen (module) each kind of document is entered on, for the action bar's Add / Delete rights.
const MODULES = { sales_invoice: 'Sales Invoice', purchase_bill: 'Purchase Bill', purchase_indent: 'Purchase Indent', sales_return: 'Sales Return',
  stock_deduction: 'Stock Deduction', expense: 'Expense Entry', closing_stock: 'Closing Stock Entry', physical_stock: 'Physical Stock Entry' };
// Columns of the List window; the party column is the customer, supplier, reason or paid-from of the kind of document.
const LIST_COLUMNS = [
  { key: 'doc_no', label: 'Doc No' }, { key: 'date', label: 'Date' }, { key: 'time', label: 'Time' }, { key: 'party', label: 'Party' }, { key: 'status', label: 'Status' },
  { key: 'qty', label: 'Qty', kind: 'number' }, { key: 'amount', label: 'Amount', kind: 'money' }, { key: 'added_by', label: 'Added By' },
];
const listRow = (d) => ({ doc_no: d.doc_no, date: d.date ?? '', time: d.time ? String(d.time).slice(0, 5) : '', party: d.customer || d.supplier || d.reason || d.paid_from || '',
  status: d.status ?? '', qty: d.totals?.qty ?? '', amount: d.total ?? d.totals?.net ?? d.totals?.basic ?? '', added_by: d.added_by ?? '' });

// The bar's Delete button, in the exe's CANCELLED sense, named for what it cancels.
const CANCEL_LABELS = { purchase_bill: 'Cancel Bill', purchase_indent: 'Cancel Indent', sales_return: 'Cancel Return', stock_deduction: 'Cancel Deduction', expense: 'Cancel Entry', closing_stock: 'Cancel Entry' };
// A document's header table, to read its Doc Designer flags (File Attachment, ...).
const HEADER_TABLES = { sales_invoice: 'TrnHdrSINV', purchase_bill: 'TrnHdrPB', purchase_indent: 'TrnHdrPI', sales_return: 'TrnHdrSR', stock_deduction: 'TrnHdrSTKDED', expense: 'TrnHdrEXP', closing_stock: 'TrnHdrCSE' };
const TITLES = { sales_invoice: 'Previous bills', purchase_bill: 'Previous purchase bills', purchase_indent: 'Previous indents',
  sales_return: 'Previous returns', stock_deduction: 'Previous deductions', expense: 'Previous expenses', closing_stock: 'Previous entries' };

/** What a row shows for any kind of document: who, how much, which date and time. */
function describe(doc) {
  const t = doc.totals ?? {};
  return {
    no: doc.doc_no,
    party: doc.customer || doc.supplier || doc.reason || doc.paid_from || '',
    amount: doc.total ?? t.net ?? t.basic ?? null,
    when: [doc.date ? String(doc.date).split('-').reverse().join('-') : '', doc.time ? String(doc.time).slice(0, 5) : ''].filter(Boolean).join(' '),
    sort: `${doc.date ?? ''} ${doc.time ?? ''} ${doc.doc_no}`,
    cancelled: ['CANCELLED', 'DELETED'].includes(doc.status),
    lines: (doc.lines ?? []).map(l => `${l.item_code ?? l.ledger ?? ''} × ${l.qty ?? l.amount ?? ''}`.trim()),
  };
}

/** `onNew` starts a blank document on the screen (the action bar's New); `options` are the screen's extra actions for the bar's Options menu;
 *  `fetchDoc(no)` loads the whole document when the list only carries a summary (Physical Stock); `onEdit(doc)` loads a saved document into the
 *  screen to change it (Sales Invoice, Purchase Bill): List → Edit Selected, the viewer's Edit and the bar's Edit all call it */
export default function WithHistory({ kind, title, children, onNew, options = [], fetchDoc, onEdit }) {
  const [open, setOpen] = useState(initiallyOpen);
  const [docs, setDocs] = useState(null);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);
  const [find, setFind] = useState('');
  const [opened, setOpened] = useState('');
  // Only the last N days of bills (kept in this browser only, edited in the panel's own box); 0 = the latest SHOWN
  const [days, setDays] = useState(getPurchaseDays);
  const changeDays = (v) => { const n = Math.max(0, Math.min(3650, Math.floor(Number(v)) || 0)); setDays(n); setPurchaseDays(n); };
  const [printNo, setPrintNo] = useState('');
  const [cancelNo, setCancelNo] = useState(null);   // Sales Invoice: the bill whose "reason" window is open (null = closed, '' = no bill picked yet)
  const [listOpen, setListOpen] = useState(false);   // the action bar's List window
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [viewNo, setViewNo] = useState('');
  const [attachOpen, setAttachOpen] = useState(false);
  const attachOn = Boolean(useScreenFields(`table:${HEADER_TABLES[kind] ?? ''}`).screen?.flags?.file_attach);   // UtlTransactions.FileAttachReq = 'Y'
  const [full, setFull] = useState(null);   // the whole document behind viewNo, when `fetchDoc` is given   // the document shown read-only (List → Open Selected, First / Previous / Next / Last)

  const toggle = useCallback(() => setOpen(o => {
    if (wide()) { try { localStorage.setItem(KEY, o ? '0' : '1'); } catch { /* the choice just isn't remembered */ } }
    return !o;
  }), []);
  useShortcuts({ 'Alt+H': toggle });


  const fetchDocs = useCallback(() => listDocuments(kind)
    .then(list => { setDocs(list); setError(''); })
    .catch(e => setError(errorText(e))), [kind]);
  const refresh = async () => { setBusy(true); await fetchDocs(); setBusy(false); };

  // Loaded when the panel or the List window opens, and again whenever a document is saved or cancelled anywhere in the app.
  useEffect(() => {
    if (!open && !listOpen) return undefined;
    void fetchDocs();
    const onChange = () => { void fetchDocs(); };
    window.addEventListener(DOCS_CHANGED, onChange);
    return () => window.removeEventListener(DOCS_CHANGED, onChange);
  }, [open, listOpen, fetchDocs]);

  const rows = useMemo(() => {
    const q = find.trim().toLowerCase();
    const from = days ? new Date(new Date().getTime() - days * 86_400_000) : null;
    const since = from ? `${from.getFullYear()}-${String(from.getMonth() + 1).padStart(2, '0')}-${String(from.getDate()).padStart(2, '0')}` : '';
    const list = (docs ?? []).filter(d => d.status !== 'HOLD' && (!since || String(d.date ?? '') >= since)).map(d => ({ doc: d, ...describe(d) }))
      .filter(r => !q || `${r.no} ${r.party} ${r.when}`.toLowerCase().includes(q))
      .sort((a, b) => (a.sort < b.sort ? 1 : -1));
    return days ? list : list.slice(0, SHOWN);
  }, [docs, find, days]);

  // The documents the bar's List and record buttons walk through: oldest first, held bills left out (they have their own list).
  const walk = useMemo(() => (docs ?? []).filter(d => d.status !== 'HOLD').sort((a, b) => (`${a.date ?? ''} ${a.time ?? ''} ${a.doc_no}` < `${b.date ?? ''} ${b.time ?? ''} ${b.doc_no}` ? -1 : 1)), [docs]);
  const at = walk.findIndex(d => d.doc_no === viewNo);
  const showDoc = (no) => {
    setViewNo(no);
    setFull(null);
    if (fetchDoc && no) fetchDoc(no).then(setFull).catch(() => undefined);
  };
  const moduleName = MODULES[kind] ?? title;
  const found = walk.find(d => d.doc_no === viewNo);
  const viewDoc = found && full?.doc_no === viewNo ? { ...found, ...full } : found;
  // Sales Invoice has no action bar: its saved bills are found, re-printed, edited and cancelled in its Re-print / Edit window (SalesInvoiceDialogs.jsx).
  const sales = kind === 'sales_invoice';
  // Purchase Bill, Purchase Indent, Sales Return, Stock Deduction: the exe's Delete button with its CANCELLED choice (deleteDoc): the opened
  // document's stock effect is taken back and it is marked cancelled.
  const cancelable = ['closing_stock', 'expense', 'purchase_bill', 'purchase_indent', 'sales_return', 'stock_deduction'].includes(kind);
  const cancelDocument = async () => {
    if (!viewNo) { toast.error('Open a saved document from the List first.'); return; }
    if (!(await confirm('Are you sure to CANCEL this Document ?'))) return;
    if (!(await toastStore.defer('delete', `${title ?? 'Document'} ${viewNo} will be cancelled when this closes.`, 'Not cancelled. Nothing was changed.'))) return;
    cancelSavedDocument(kind, viewNo).then(() => toast.ok(`Cancelled ${title ?? 'document'} ${viewNo} Successfully!`)).catch(e => toast.error(errorText(e)));
  };
  const bar = sales ? null : (
    <ActionBar
      onNew={sales ? undefined : onNew} canNew={canDo(moduleName, 'add')}
      listLabel={sales ? 'Edit' : 'List'}
      onEdit={!sales && onEdit && canDo(moduleName, 'edit') ? () => viewDoc && onEdit(viewDoc) : undefined} canEdit={!!viewDoc && !['CANCELLED', 'DELETED', 'HOLD'].includes(viewDoc.status)}
      noEditReason="Open a saved one from the List first (a cancelled one can't be edited)"
      onDelete={kind === 'sales_invoice' ? () => setCancelNo(viewNo || '') : cancelable ? () => void cancelDocument() : undefined} deleteLabel={CANCEL_LABELS[kind] ?? 'Cancel Bill'} canDelete={canDo(moduleName, 'delete')}
      onList={() => setListOpen(true)} onClose={goHome}
      options={[...options, ...(attachOn ? [{ label: 'File Attachment', onClick: () => setAttachOpen(true), disabled: !viewNo }] : []), { label: open ? 'Hide history' : 'Show history', onClick: toggle }]}
    />
  );

  return (
    <ScreenFrame bar={bar}>
    {confirmDialog}
    {attachOpen && viewNo && <AttachmentsDialog master={kind} code={viewNo} name={viewNo} canEdit={canDo(moduleName, 'edit')} onClose={() => setAttachOpen(false)} />}
    <div className="flex items-start gap-4">
      <div className="min-w-0 flex-1">{children}</div>

      {!open && (
        <button type="button" onClick={toggle} aria-label="Show history" title="Show history (Alt+H)"
          className="fixed right-0 top-1/2 z-30 flex -translate-y-1/2 flex-col items-center gap-2 rounded-l-xl border border-r-0 border-[#E3D9CD] bg-[#FFFCF7] px-1.5 py-3 text-[#64283A] shadow-md hover:bg-[#F3EEE6] dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-[#D4A847] dark:hover:bg-[#1C1C1C]">
          <History size={16} />
          <span className="text-[11px] font-semibold [writing-mode:vertical-rl]">History</span>
        </button>
      )}

      {open && <div className="fixed inset-0 z-30 bg-black/30 2xl:hidden" onClick={toggle} aria-hidden="true" />}
      {open && (
        <aside aria-label="History"
          className={`${panel} z-40 flex flex-col overflow-hidden max-2xl:fixed max-2xl:inset-y-0 max-2xl:right-0 max-2xl:w-[min(92vw,340px)] max-2xl:rounded-r-none 2xl:sticky 2xl:top-4 2xl:max-h-[calc(100vh-2rem)] 2xl:w-80 2xl:shrink-0`}>
          <div className="flex items-center gap-1 border-b border-[#E3D9CD] px-3 py-2.5 dark:border-[#2A2A2A]">
            <History size={15} className="text-[#64283A] dark:text-[#D4A847]" />
            <h2 className="min-w-0 flex-1 truncate text-sm font-semibold">{TITLES[kind] ?? `Previous ${title ?? 'documents'}`}</h2>
            <button type="button" onClick={() => void refresh()} className={`${iconButton} h-7 w-7`} aria-label="Refresh history" title="Refresh">
              <RefreshCw size={13} className={busy ? 'animate-spin' : ''} />
            </button>
            <button type="button" onClick={toggle} className={`${iconButton} h-7 w-7`} aria-label="Hide history" title="Hide (Alt+H)"><X size={14} /></button>
          </div>

          <div className="flex items-center gap-2 border-b border-[#E3D9CD] px-3 py-1.5 text-[11px] text-stone-600 dark:border-[#2A2A2A] dark:text-stone-300">
            <label htmlFor="hist-days">Show last</label>
            <input id="hist-days" type="number" min={0} max={3650} value={days} onChange={e => changeDays(e.target.value)}
              title="Days of bills listed here (0 = the latest 60). Kept on this computer only."
              className="w-16 rounded-md border border-[#DCD1C5] bg-transparent px-1.5 py-0.5 text-right tabular-nums outline-none focus:border-[#64283A] dark:border-[#2A2A2A] dark:focus:border-[#D4A847]" />
            <span>days</span>
          </div>

          <div className="relative border-b border-[#E3D9CD] p-2 dark:border-[#2A2A2A]">
            <Search size={13} className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
            <input value={find} onChange={e => setFind(e.target.value)} placeholder="Find number, name or date"
              aria-label="Find in history"
              className="w-full rounded-lg border border-[#DCD1C5] bg-transparent py-1.5 pl-8 pr-2 text-xs outline-none focus:border-[#64283A] dark:border-[#2A2A2A] dark:focus:border-[#D4A847]" />
          </div>

          <div className="min-h-0 flex-1 overflow-y-auto">
            {error && <p role="alert" className="p-3 text-xs text-red-600 dark:text-red-400">{error}</p>}
            {!error && docs === null && <p className="p-3 text-xs text-stone-500">Loading…</p>}
            {!error && docs !== null && !rows.length && <p className="p-3 text-xs text-stone-500">{find ? 'Nothing matches.' : 'Nothing made yet.'}</p>}
            <ul>
              {rows.map(r => (
                <li key={r.no} className="border-b border-[#EFE9DF] last:border-0 dark:border-[#1F1F1F]">
                  <button type="button" onClick={() => setOpened(o => (o === r.no ? '' : r.no))} aria-expanded={opened === r.no}
                    className="flex w-full items-start gap-2 px-3 py-2 text-left hover:bg-[#F3EEE6] dark:hover:bg-[#1A1A1A]">
                    <span className="min-w-0 flex-1">
                      <span className={`block truncate font-mono text-xs font-medium ${r.cancelled ? 'text-stone-400 line-through' : ''}`}>{r.no}</span>
                      <span className="block truncate text-[11px] text-stone-500 dark:text-stone-400">{[r.when, r.party].filter(Boolean).join(' · ')}</span>
                    </span>
                    <span className="shrink-0 text-right">
                      <span className={`block text-xs font-semibold tabular-nums ${r.cancelled ? 'text-stone-400 line-through' : ''}`}>{money(r.amount)}</span>
                      {r.cancelled && <span className={badge('danger')}>Cancelled</span>}
                    </span>
                    <ChevronDown size={13} className={`mt-1 shrink-0 text-stone-400 transition-transform ${opened === r.no ? 'rotate-180' : ''}`} />
                  </button>
                  {opened === r.no && (
                    <div className="bg-[#FBF8F2] px-3 pb-2.5 pt-1 text-[11px] dark:bg-[#171717]">
                      {r.lines.length ? <p className="leading-5 text-stone-600 dark:text-stone-300">{r.lines.join(' · ')}</p> : <p className="text-stone-500">No lines.</p>}
                      <button type="button" onClick={() => setPrintNo(r.no)}
                        className="mt-2 inline-flex items-center gap-1.5 rounded-lg border border-[#B08A45] px-2.5 py-1 font-semibold text-[#64283A] hover:bg-[#B08A45]/10 dark:border-[#D4A847]/40 dark:text-[#D4A847]">
                        <Printer size={12} />Print / preview
                      </button>
                      {kind === 'sales_invoice' && !r.cancelled && (
                        <button type="button" onClick={() => setCancelNo(r.no)}
                          className="ml-2 mt-2 inline-flex items-center gap-1.5 rounded-lg border border-red-300 px-2.5 py-1 font-semibold text-red-700 hover:bg-red-50 dark:border-red-900 dark:text-red-400 dark:hover:bg-red-950/40">
                          <Ban size={12} />Cancel bill
                        </button>
                      )}
                    </div>
                  )}
                </li>
              ))}
            </ul>
          </div>
          <p className="border-t border-[#E3D9CD] px-3 py-1.5 text-[10px] text-stone-500 dark:border-[#2A2A2A]">
            {days ? `Last ${days} days` : `Latest ${SHOWN}`} · {rows.length} listed ·<Kbd combo="Alt+H" /> hides this
          </p>
        </aside>
      )}

      {listOpen && (
        <ListModal title={title ? `${title[0].toUpperCase()}${title.slice(1)}` : 'Documents'} subtitle={`Listing of ${title ?? 'documents'}`} columns={LIST_COLUMNS} defaultKeys={['doc_no', 'date', 'party', 'status', 'amount']}
          rows={walk.map(listRow).reverse()} rowKey={r => r.doc_no} loading={docs === null} error={error} storageKey={`doc-${kind}`} exportName={title ?? kind}
          onRefresh={() => void refresh()} onOpen={r => showDoc(r.doc_no)} onClose={() => setListOpen(false)} openLabel="Open Selected"
          onEdit={onEdit && canDo(moduleName, 'edit') ? r => { const d = walk.find(x => x.doc_no === r.doc_no); if (d) onEdit(d); } : undefined}
          canEditRow={r => !['CANCELLED', 'DELETED', 'HOLD'].includes(r.status)}
          renderDetail={r => { const d = walk.find(x => x.doc_no === r.doc_no); return d?.lines?.length ? <p className="leading-5">{d.lines.map(l => `${l.item_code ?? l.ledger ?? ''} × ${l.qty ?? l.amount ?? ''}`).join(' · ')}</p> : <p className="text-stone-500">No lines.</p>; }} />
      )}
      {viewDoc && (
        <DocViewDialog doc={viewDoc} kind={kind} title={title ? `${title[0].toUpperCase()}${title.slice(1)}` : 'Document'} position={at >= 0 ? `${at + 1} / ${walk.length}` : ''}
          onClose={() => showDoc('')} onPrint={() => setPrintNo(viewDoc.doc_no)} onCancelBill={() => setCancelNo(viewDoc.doc_no)}
          onEdit={onEdit && canDo(moduleName, 'edit') ? () => { const d = viewDoc; showDoc(''); onEdit(d); } : undefined} />
      )}
      {cancelNo !== null && <CancelInvoiceDialog initialNo={cancelNo} onClose={() => setCancelNo(null)}
        onDone={(msg) => { setCancelNo(null); toast.ok(msg); void fetchDocs(); }} />}
      {printNo && <PrintDialog kind={kind} docNo={printNo} title={`Print (${printNo})`} onClose={() => setPrintNo('')} />}
    </div>
    </ScreenFrame>
  );
}
