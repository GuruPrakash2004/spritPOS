import { useEffect, useState } from 'react';
import { Download, FileOutput, Save } from 'lucide-react';
import Modal from './Modal';
import Toast from './Toast';
import { api, errorText, qs, todayIso, type ApiClosingLoad, type ApiCounter, type ApiDoc } from './api';
import { SALES_PERSONS } from './salesCounters';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from './ui';

type Row = ApiClosingLoad & { closing: string };

// Legacy frmTrnCloseStk: load liquor items with their opening balance for the As-on Date, enter the physical
// closing stock, and the difference becomes the day's sales (Sales Qty = OP − Closing).
export default function ClosingStockEntry() {
  const [asOn, setAsOn] = useState(todayIso());
  const [rows, setRows] = useState<Row[]>([]);
  const [savedDoc, setSavedDoc] = useState<string | null>(null);
  const [generateOpen, setGenerateOpen] = useState(false);
  const [counters, setCounters] = useState<ApiCounter[]>([]);
  const [counter, setCounter] = useState('');
  const [salesPerson, setSalesPerson] = useState(SALES_PERSONS[0]);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  useEffect(() => {
    api<ApiCounter[]>('/masters/sales-counters')
      .then(list => { const active = list.filter(c => !c.inactive); setCounters(active); setCounter(active[0]?.code ?? ''); })
      .catch(() => undefined);
  }, []);

  const load = () => {
    api<ApiClosingLoad[]>(`/transactions/closing-stock/load${qs({ as_on: asOn })}`)
      .then(list => { setRows(list.map(r => ({ ...r, closing: '' }))); setSavedDoc(null); })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const salesQty = (r: Row) => (r.closing === '' ? 0 : r.op_qty - Number(r.closing));
  const entered = rows.filter(r => r.closing !== '');
  const invalid = entered.some(r => salesQty(r) < 0 || Number(r.closing) < 0);
  const totalSales = entered.reduce((acc, r) => acc + salesQty(r) * r.mrp, 0);

  const save = () => {
    if (!entered.length) return setToast({ kind: 'error', text: 'Enter the closing stock for at least one item.' });
    if (invalid) return setToast({ kind: 'error', text: 'Closing stock cannot be more than the opening stock.' });
    api<{ doc_no: string }>('/transactions/closing-stock', {
      method: 'POST',
      body: { as_on: asOn, lines: entered.map(r => ({ item_code: r.item_code, closing_qty: Number(r.closing) })) },
    }).then(doc => {
      setSavedDoc(doc.doc_no);
      setToast({ kind: 'ok', text: `Closing Stock ${doc.doc_no} saved. You can now generate the sales invoice.` });
    }).catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const generate = () => {
    if (!savedDoc) return;
    api<ApiDoc>(`/transactions/closing-stock/${savedDoc}/generate-invoice`, { method: 'POST', body: { counter, salesperson: salesPerson } })
      .then(inv => {
        setGenerateOpen(false);
        setToast({ kind: 'ok', text: `Sales Invoice ${inv.doc_no} generated — ₹ ${money(inv.totals.net)}.` });
        load();
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={`${panel} flex flex-wrap items-end justify-between gap-3 px-5 py-3`}>
        <div>
          <h2 className={pageTitle}>Closing Stock Entry</h2>
          <p className={pageSub}>Enter the physical closing stock; the difference from opening is billed as the day's sales.</p>
        </div>
        <div className="flex flex-wrap items-end gap-2">
          <div>
            <label htmlFor="cse-date" className={`${label} !text-red-600 dark:!text-red-400`}>As-on Date</label>
            <input id="cse-date" type="date" value={asOn} onChange={e => setAsOn(e.target.value)} className={`${field} font-mono`} />
          </div>
          <button onClick={load} className={secondaryButton}><Download size={14} />Load Items</button>
          <button onClick={save} disabled={!rows.length || Boolean(savedDoc)} className={primaryButton}><Save size={14} />Save</button>
          <button onClick={() => setGenerateOpen(true)} disabled={!savedDoc} className={primaryButton}><FileOutput size={14} />Generate Sales Invoice</button>
        </div>
      </div>

      <div className={`${panel} max-h-[560px] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>
              <th className="px-3 py-2">Sl</th><th className="px-3 py-2">Item</th><th className="px-3 py-2">Pkg</th>
              <th className="px-3 py-2 text-right">OP Qty</th><th className="px-3 py-2 text-right">Closing Qty</th>
              <th className="px-3 py-2 text-right">Sales Qty</th><th className="px-3 py-2 text-right">MRP</th><th className="px-3 py-2 text-right">Sales Value</th>
            </tr>
          </thead>
          <tbody className={`${tableBody} font-mono`}>
            {!rows.length && <tr><td colSpan={8} className="px-3 py-12 text-center font-sans text-stone-400">Pick the As-on Date and press Load Items.</td></tr>}
            {rows.map((r, i) => {
              const sq = salesQty(r);
              return (
                <tr key={r.item_code}>
                  <td className="px-3 py-1.5 font-sans text-stone-400">{i + 1}</td>
                  <td className="px-3 py-1.5 font-sans font-medium text-stone-900 dark:text-stone-100">{r.item_name}</td>
                  <td className="px-3 py-1.5 text-stone-500">{r.package}</td>
                  <td className="px-3 py-1.5 text-right text-stone-500">{r.op_qty}</td>
                  <td className="px-3 py-1.5 text-right">
                    <input
                      type="number" min={0} max={r.op_qty} value={r.closing} disabled={Boolean(savedDoc)}
                      onChange={e => setRows(rows.map(x => (x.item_code === r.item_code ? { ...x, closing: e.target.value } : x)))}
                      className={`${field} w-20 py-0.5 text-right`} aria-label={`Closing qty for ${r.item_name}`}
                    />
                  </td>
                  <td className={`px-3 py-1.5 text-right font-bold ${sq < 0 ? 'text-red-600 dark:text-red-400' : 'text-stone-900 dark:text-stone-100'}`}>{r.closing === '' ? '—' : sq}</td>
                  <td className="px-3 py-1.5 text-right">{money(r.mrp)}</td>
                  <td className="px-3 py-1.5 text-right">{r.closing === '' ? '—' : money(sq * r.mrp)}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {rows.length > 0 && (
        <div className={`${panel} flex flex-wrap justify-between gap-3 px-5 py-3 text-sm font-bold text-stone-700 dark:text-stone-200`}>
          <span>{entered.length} of {rows.length} items entered{savedDoc && <span className="ml-3 font-mono text-emerald-600 dark:text-emerald-400">Saved as {savedDoc}</span>}</span>
          <span className="text-[#b84716] dark:text-[#ffb07e]">Sales Value: ₹ {money(totalSales)}</span>
        </div>
      )}

      {generateOpen && (
        <Modal
          title="Generate Sales Invoice"
          onClose={() => setGenerateOpen(false)}
          footer={<>
            <button onClick={() => setGenerateOpen(false)} className={secondaryButton}>Cancel</button>
            <button onClick={generate} disabled={!counter} className={primaryButton}><FileOutput size={14} />Generate</button>
          </>}
        >
          <p className="mb-3 text-stone-600 dark:text-stone-300">One sales invoice will be raised for the computed sales quantities of <span className="font-mono">{savedDoc}</span>.</p>
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label htmlFor="cse-counter" className={label}>Counter</label>
              <select id="cse-counter" value={counter} onChange={e => setCounter(e.target.value)} className={`${field} w-full`}>
                {counters.map(c => <option key={c.code} value={c.code}>{c.name}</option>)}
              </select>
            </div>
            <div>
              <label htmlFor="cse-sp" className={label}>Sales Person</label>
              <select id="cse-sp" value={salesPerson} onChange={e => setSalesPerson(e.target.value)} className={`${field} w-full`}>
                {SALES_PERSONS.map(p => <option key={p}>{p}</option>)}
              </select>
            </div>
          </div>
        </Modal>
      )}
    </div>
  );
}
