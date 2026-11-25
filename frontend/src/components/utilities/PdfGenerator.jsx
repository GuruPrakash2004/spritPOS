// PDF Generator (the desktop's frmUtlBulkPDFGenerator): choose a document type and dates, Load the documents, tick the ones you want and Generate:
// each is laid out by the desktop's own Crystal print format (the same as Print) and they come as one ZIP of PDFs. At most 100 at a time. A
// document that cannot be drawn is listed in errors.txt inside the zip.

import { useEffect, useMemo, useState } from 'react';
import { Download, FileDown } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { downloadBulkPdf, errorText, getPrintOptions, listPurchaseBills, listSalesInvoices, getPurchaseIndents, todayIso } from '../../lib/api';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';
import DateInput from '../common/DateInput';

const KINDS = [
  { id: 'sales_invoice', name: 'Sales Invoice', load: (from, to) => listSalesInvoices('SAVED', from, to) },
  { id: 'purchase_bill', name: 'Purchase Bill', load: (from, to) => listPurchaseBills(from, to) },
  { id: 'purchase_indent', name: 'Purchase Indent', load: (from, to) => getPurchaseIndents(from, to) },
];
const MAX = 100;

export default function PdfGenerator() {
  const [kind, setKind] = useState(KINDS[0].id);
  const [from, setFrom] = useState(todayIso);
  const [to, setTo] = useState(todayIso);
  const [docs, setDocs] = useState(null);
  const [ticked, setTicked] = useState(new Set());
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);
  const [find, setFind] = useState('');
  const [printTypes, setPrintTypes] = useState([]);   // the document's print options (UtlOption), as the desktop's Print Type list
  const [printType, setPrintType] = useState('');

  // The desktop lists the document's print options and needs one chosen: the default is pre-selected.
  useEffect(() => {
    let live = true;
    getPrintOptions(kind).then(o => {
      if (!live) return;
      const usable = (o.options ?? []).filter(x => x.available !== false);
      setPrintTypes(usable);
      setPrintType((usable.find(x => x.default) ?? usable[0])?.name ?? '');
    }).catch(() => { if (live) { setPrintTypes([]); setPrintType(''); } });
    return () => { live = false; };
  }, [kind]);

  const load = async () => {
    setProblem(''); setDocs(null);
    try {
      const list = await KINDS.find(k => k.id === kind).load(from, to);
      setDocs(list); setTicked(new Set());
      if (!list.length) setProblem('No Records Found!');
    } catch (e) { setProblem(errorText(e)); }
  };

  const q = find.trim().toLowerCase();
  const shown = useMemo(() => (docs ?? []).filter(d => !q || d.doc_no.toLowerCase().includes(q) || String(d.customer ?? d.supplier ?? '').toLowerCase().includes(q)), [docs, q]);
  const toggle = (no) => setTicked(cur => { const n = new Set(cur); if (n.has(no)) n.delete(no); else n.add(no); return n; });

  const generate = async () => {
    if (printTypes.length && !printType) return setProblem('Please select Print Type');
    if (!ticked.size) return setProblem('Please select at least one document.');
    if (ticked.size > MAX) return setProblem(`At most ${MAX} documents at a time.`);
    setProblem('');
    if (!(await toastStore.defer('ok', `${ticked.size} PDF${ticked.size === 1 ? '' : 's'} will be generated when this closes.`, 'Cancelled. Nothing was generated.'))) return;
    setBusy(true);
    try {
      const blob = await downloadBulkPdf(kind, [...ticked], printType);
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url; a.download = `${kind}_${from}_${to}.zip`; a.click();
      setTimeout(() => URL.revokeObjectURL(url), 60_000);
      toastStore.add('ok', 'PDFs generated.');
    } catch (e) { setProblem(errorText(e)); } finally { setBusy(false); }
  };

  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-4">
      <div className={pageHeader}>
        <div><h1 className={pageTitle}>PDF Generator</h1><p className={pageSub}>Make PDFs of many documents at once, in the desktop's own print formats.</p></div>
      </div>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} flex flex-wrap items-end gap-3 px-4 py-3`}>
        <div>
          <label htmlFor="pg-kind" className={label}>Document</label>
          <select id="pg-kind" value={kind} onChange={e => { setKind(e.target.value); setDocs(null); }} className={field}>
            {KINDS.map(k => <option key={k.id} value={k.id}>{k.name}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="pg-type" className={label}>Print Type</label>
          <select id="pg-type" value={printType} onChange={e => setPrintType(e.target.value)} className={field}>
            {!printTypes.length && <option value="">Default format</option>}
            {printTypes.map(t => <option key={t.name} value={t.name}>{t.name}</option>)}
          </select>
        </div>
        <div><label htmlFor="pg-from" className={label}>From</label><DateInput id="pg-from" value={from} max={to} onChange={e => e.target.value && setFrom(e.target.value)} className={field} /></div>
        <div><label htmlFor="pg-to" className={label}>To</label><DateInput id="pg-to" value={to} min={from} max={todayIso()} onChange={e => e.target.value && setTo(e.target.value)} className={field} /></div>
        <button onClick={() => void load()} className={secondaryButton}><Download size={14} />Load</button>
        <button onClick={() => void generate()} disabled={busy || !ticked.size} className={`${primaryButton} ml-auto`}><FileDown size={14} />{busy ? 'Generating…' : `Generate PDF (${ticked.size})`}</button>
      </div>
      {docs && (
        <div className={`${panel} p-3`}>
          <div className="mb-2 flex flex-wrap items-center gap-3 text-sm">
            <input value={find} onChange={e => setFind(e.target.value)} placeholder="Find a document" aria-label="Find a document" className={`${field} w-56`} />
            <label className="flex items-center gap-1.5"><input type="checkbox" checked={shown.length > 0 && shown.every(d => ticked.has(d.doc_no))} onChange={e => setTicked(e.target.checked ? new Set(shown.map(d => d.doc_no)) : new Set())} />Select All</label>
            <span className="ml-auto text-stone-500">{ticked.size} selected out of {docs.length}</span>
          </div>
          <div className="max-h-[55vh] overflow-auto">
            <table className="w-full text-sm">
              <thead className={tableHead}><tr><th className="w-10 px-3 py-2" /><th className="px-3 py-2 text-left">Doc No</th><th className="px-3 py-2 text-left">Date</th><th className="px-3 py-2 text-left">Party</th><th className="px-3 py-2 text-right">Amount</th></tr></thead>
              <tbody className={tableBody}>
                {shown.map(d => (
                  <tr key={d.doc_no}>
                    <td className="px-3 py-1.5"><input type="checkbox" checked={ticked.has(d.doc_no)} onChange={() => toggle(d.doc_no)} aria-label={`Select ${d.doc_no}`} /></td>
                    <td className="px-3 py-1.5 tabular-nums">{d.doc_no}</td><td className="px-3 py-1.5 tabular-nums">{d.date}</td>
                    <td className="px-3 py-1.5">{d.customer ?? d.supplier ?? ''}</td><td className="px-3 py-1.5 text-right tabular-nums">{d.total ?? d.totals?.net ?? ''}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
