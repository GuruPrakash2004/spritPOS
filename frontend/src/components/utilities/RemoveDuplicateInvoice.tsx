// Utilities → Remove Duplicate Invoice (legacy frmUtlDelDupInvoice, shortcut 711).
//
// Lists saved invoices (not cancelled or deleted) for a date range and an optional invoice number, with the
// desktop columns (Invoice No, Date, Time, Added By, Sales Type, Bill System, Tot Qty, Inv Value, Sales
// Counter). A bill that repeats an earlier one (same counter, amount and lines within a few minutes) is
// flagged "Possible duplicate of …"; "Only possible duplicates" narrows the list to them. Tick the bills to
// remove, then Remove: it asks "Selected Invoice Will Delete.... Do you want to Continue...?", then waits for
// its popup. Their stock goes back and their points are reversed (backend app/duplicate_invoices.py). Desktop
// bills can't be ticked: the web app never changes the desktop database.

import { useCallback, useEffect, useState } from 'react';
import { AlertTriangle, Search, Trash2 } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import { useAdminPassword } from '../common/AdminPasswordPrompt';
import { encryptSecret } from '../../lib/secureField';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { api, errorText, type ApiDupInvoice } from '../../lib/api';
import { badge, dangerButton, field, fieldError, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow } from '../../lib/styles';

const today = () => new Date().toISOString().slice(0, 10);

export default function RemoveDuplicateInvoice() {
  const [from, setFrom] = useState(today);
  const [to, setTo] = useState(today);
  const [invoiceNo, setInvoiceNo] = useState('');
  const [onlyDup, setOnlyDup] = useState(false);
  const [rows, setRows] = useState<ApiDupInvoice[]>([]);
  const [picked, setPicked] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(true);
  const [removing, setRemoving] = useState(false);
  const [dateError, setDateError] = useState('');
  const { confirm, dialog } = useConfirm();
  const { ask, dialog: passwordDialog } = useAdminPassword();

  const load = useCallback((f: string, t: string, no: string, dup: boolean) =>
    api<ApiDupInvoice[]>(`/utilities/duplicate-invoices?${new URLSearchParams({
      from_date: f, to_date: t, ...(no.trim() ? { invoice_no: no.trim() } : {}), ...(dup ? { only_duplicates: 'true' } : {}),
    })}`)
      .then(r => { setRows(r); setPicked(new Set()); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; })
      .finally(() => setLoading(false)), []);

  useEffect(() => { load(today(), today(), '', false); }, [load]);

  const show = async () => {
    if (!from || !to || to < from) {
      setDateError(!from || !to ? 'Pick both dates' : 'To date is before From date');
      document.getElementById('rdi-to')?.focus();
      return;
    }
    setDateError('');
    if (!(await toastStore.defer('refresh', 'Invoices will be loaded when this closes.', 'Cancelled.'))) return;
    setLoading(true);
    if (await load(from, to, invoiceNo, onlyDup)) toastStore.add('refresh', 'Invoices loaded.');
  };

  const toggle = (no: string) => setPicked(p => { const n = new Set(p); if (n.has(no)) n.delete(no); else n.add(no); return n; });
  const removable = rows.filter(r => r.removable);
  const allPicked = removable.length > 0 && removable.every(r => picked.has(r.doc_no));

  const remove = async () => {
    const doc_nos = [...picked];
    if (!doc_nos.length) return toastStore.add('error', 'Please select atleast one invoice...');
    const total = rows.filter(r => picked.has(r.doc_no)).reduce((s, r) => s + r.value, 0);
    if (!(await confirm(`Selected Invoice Will Delete.... Do you want to Continue...?\n\n${doc_nos.length} bill(s), ₹ ${money(total)}. `
      + 'Their stock goes back and their loyalty points are reversed.'))) return;
    const password = await ask();
    if (password === null) return;
    if (!(await toastStore.defer('delete', `${doc_nos.length} invoice(s) will be removed when this closes.`, 'Remove cancelled. Nothing was changed.'))) return;
    setRemoving(true);
    try {
      const admin_password = await encryptSecret(password, 'admin');   // leaves the browser only encrypted
      const res = await api<{ removed: string[]; message: string }>('/utilities/duplicate-invoices/remove', { method: 'POST', body: { doc_nos, admin_password } });
      toastStore.add('ok', `${res.message} (${res.removed.length} invoice${res.removed.length === 1 ? '' : 's'})`);
      setLoading(true);
      await load(from, to, invoiceNo, onlyDup);
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setRemoving(false);
    }
  };

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      {dialog}
      {passwordDialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Remove Duplicate Invoice</h1>
          <p className={pageSub}>Remove bills saved twice by mistake. Their stock goes back and their points are reversed; the ADMIN Audit Log keeps a copy.</p>
        </div>
        <button className={dangerButton} onClick={remove} disabled={removing || !picked.size}><Trash2 size={14} />{removing ? 'Removing…' : `Remove${picked.size ? ` (${picked.size})` : ''}`}</button>
      </div>

      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div>
          <label htmlFor="rdi-from" className={label}>From</label>
          <input id="rdi-from" type="date" value={from} onChange={e => { setFrom(e.target.value); setDateError(''); }} className={field} />
        </div>
        <div>
          <label htmlFor="rdi-to" className={label}>To</label>
          <input id="rdi-to" type="date" value={to} onChange={e => { setTo(e.target.value); setDateError(''); }}
            className={`${field} ${dateError ? fieldError : ''}`} aria-invalid={!!dateError} />
          <InlineError message={dateError} />
        </div>
        <div className="min-w-[180px]">
          <label htmlFor="rdi-no" className={label}>Invoice No.</label>
          <input id="rdi-no" value={invoiceNo} onChange={e => setInvoiceNo(e.target.value)} onKeyDown={e => e.key === 'Enter' && show()} className={field} placeholder="Part of the number" />
        </div>
        <label className="flex items-center gap-2 pb-2 text-xs text-stone-700 dark:text-stone-200">
          <input type="checkbox" checked={onlyDup} onChange={e => setOnlyDup(e.target.checked)} />Only possible duplicates
        </label>
        <button className={primaryButton} onClick={show} disabled={loading}><Search size={14} />Show</button>
      </section>

      <section className={`${panel} overflow-hidden`}>
        <div className="max-h-[60vh] overflow-auto">
          <table className="min-w-full text-xs">
            <thead>
              <tr>
                <th className={`${tableHead} px-3 py-2`}>
                  <input type="checkbox" aria-label="Select all removable invoices" checked={allPicked} disabled={!removable.length}
                    onChange={() => setPicked(allPicked ? new Set() : new Set(removable.map(r => r.doc_no)))} />
                </th>
                {['Invoice No.', 'Inv Date', 'Time', 'Added By', 'Sales Type', 'Bill System', 'Tot Qty', 'Inv Value', 'Sales Counter', ''].map((h, i) => (
                  <th key={i} className={`${tableHead} px-3 py-2 ${h === 'Tot Qty' || h === 'Inv Value' ? 'text-right' : ''}`}>{h}</th>
                ))}
              </tr>
            </thead>
            <tbody className={tableBody}>
              {rows.map(r => (
                <tr key={r.doc_no} className={`${tableRow} ${r.duplicate_of ? 'bg-amber-50/70 dark:bg-amber-900/10' : ''}`}>
                  <td className="px-3 py-1.5">
                    <input type="checkbox" aria-label={`Select ${r.doc_no}`} checked={picked.has(r.doc_no)} disabled={!r.removable} onChange={() => toggle(r.doc_no)} />
                  </td>
                  <td className="px-3 py-1.5 font-mono">{r.doc_no}</td>
                  <td className="px-3 py-1.5 tabular-nums">{r.date.split('-').reverse().join('-')}</td>
                  <td className="px-3 py-1.5 tabular-nums">{r.time}</td>
                  <td className="px-3 py-1.5">{r.added_by}</td>
                  <td className="px-3 py-1.5">{r.sales_type}</td>
                  <td className="px-3 py-1.5">{r.bill_system}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{r.qty}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{money(r.value)}</td>
                  <td className="px-3 py-1.5">{r.counter}</td>
                  <td className="px-3 py-1.5">
                    {r.duplicate_of && <span className={badge('warning')}><AlertTriangle size={12} />Possible duplicate of {r.duplicate_of}</span>}
                    {r.why_not && <span className="text-[11px] text-stone-500">{r.why_not}</span>}
                  </td>
                </tr>
              ))}
              {!rows.length && <tr><td colSpan={11} className="px-3 py-6 text-center text-stone-500">{loading ? 'Loading…' : 'No Records Found...'}</td></tr>}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}
