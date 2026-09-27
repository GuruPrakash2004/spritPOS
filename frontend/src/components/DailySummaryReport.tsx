import { useCallback, useEffect, useState } from 'react';
import { Printer, RefreshCw } from 'lucide-react';
import Toast from './Toast';
import { api, errorText, qs, todayIso, type ApiFullDsr } from './api';
import {  field, money, pageSub, pageTitle, panel, secondaryButton, pageHeader , printButton } from './ui';
import PrintFormatSelectorModal from './print/PrintFormatSelectorModal';

// Legacy frmRptDailySummary; fields follow Docs/WebDash.xlsx.
export default function DailySummaryReport() {
  const [on, setOn] = useState(todayIso());
  const [dsr, setDsr] = useState<ApiFullDsr | null>(null);
  const [error, setError] = useState('');
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [refreshing, setRefreshing] = useState(false);
  const [showPrintModal, setShowPrintModal] = useState(false);

  const load = useCallback(() => {
    api<ApiFullDsr>(`/reports/dsr${qs({ on })}`)
      .then(d => { setDsr(d); setError(''); })
      .catch(e => setError(errorText(e)));
  }, [on]);

  useEffect(() => { load(); }, [load]);

  const handleRefresh = () => {
    setRefreshing(true);
    api<ApiFullDsr>(`/reports/dsr${qs({ on })}`)
      .then(d => {
        setDsr(d);
        setError('');
        setToast({ kind: 'ok', text: `Daily Summary Report for ${on} refreshed.` });
      })
      .catch(e => {
        const msg = errorText(e);
        setError(msg);
        setToast({ kind: 'error', text: msg });
      })
      .finally(() => setRefreshing(false));
  };

  const kpis: [string, string][] = dsr ? [
    ['Total Bills', String(dsr.total_bills)],
    ['Sales incl. GST', `₹ ${money(dsr.sales_incl_gst)}`],
    ['Liquor Basic', `₹ ${money(dsr.liquor_basic)}`],
    ['Non-Liquor Basic', `₹ ${money(dsr.non_liquor_basic)}`],
    ['Profit of the Day', `₹ ${money(dsr.profit_of_the_day)}`],
    ['Opening Stock', `${dsr.opening_stock.qty} btl • ₹ ${money(dsr.opening_stock.value)}`],
    ['Closing Stock', `${dsr.closing_stock.qty} btl • ${dsr.closing_stock.litres} L • ₹ ${money(dsr.closing_stock.value)}`],
    ['Purchase (incl. GST)', `₹ ${money(dsr.purchase.incl_gst)}`],
    ['Sales Returns', `₹ ${money(dsr.sales_returns)}`],
    ['Expenses', `₹ ${money(dsr.expenses)}`],
    ['Cash Balance', `₹ ${money(dsr.cash_balance)}`],
    ['Points Redeemed', `₹ ${money(dsr.redemption)}`],
  ] : [];

  const tables: [string, Record<string, number>][] = dsr ? [
    ['Payment Modes', dsr.payment_modes],
    ['Counter-wise Sales', dsr.counter_wise],
    ['Salesperson-wise Sales', dsr.salesperson_wise],
    ['Group-wise Sales', dsr.group_wise_sales],
    ['Expenses Ledger-wise', dsr.expense_ledger_wise],
  ] : [];

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      {error && <Toast kind="error" text={error} />}
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Daily Summary Report</h1>
          <p className={pageSub}>Sales, stock, payments, purchases and cash for one day.</p>
        </div>
        <div className="flex items-end gap-2 print:hidden">
          <input type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} tabular-nums`} aria-label="Report date" />
          <button onClick={handleRefresh} disabled={refreshing} className={secondaryButton}><RefreshCw size={14} className={refreshing ? "animate-spin" : ""} />{refreshing ? "Refreshing…" : "Refresh"}</button>
          <button onClick={() => setShowPrintModal(true)} disabled={!dsr} className={printButton}><Printer size={14} />Print</button>
        </div>
      </div>

      <div className="grid grid-cols-2 gap-3 md:grid-cols-3 xl:grid-cols-4">
        {kpis.map(([k, v]) => (
          <div key={k} className={`${panel} px-4 py-3.5`}>
            <div className="text-xs font-medium text-stone-500 dark:text-stone-400">{k}</div>
            <div className="mt-1 text-lg font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">{v}</div>
          </div>
        ))}
      </div>

      <div className="grid grid-cols-1 gap-3 md:grid-cols-2 xl:grid-cols-3">
        {tables.map(([title, data]) => (
          <div key={title} className={`${panel} overflow-hidden text-xs`}>
            <div className="border-b border-line bg-subtle px-3 py-2 font-bold text-stone-600 dark:text-stone-300">{title}</div>
            <table className="w-full">
              <tbody className="divide-y divide-line-soft">
                {Object.entries(data).map(([k, v]) => (
                  <tr key={k}><td className="px-3 py-1.5 text-stone-700 dark:text-stone-200">{k}</td><td className="px-3 py-1.5 text-right tabular-nums">₹ {money(v)}</td></tr>
                ))}
                {!Object.keys(data).length && <tr><td className="px-3 py-3 text-center text-stone-400">Nothing for this day.</td></tr>}
              </tbody>
            </table>
          </div>
        ))}
        {dsr && (
          <div className={`${panel} overflow-hidden text-xs`}>
            <div className="border-b border-line bg-subtle px-3 py-2 font-bold text-stone-600 dark:text-stone-300">Cancelled Invoices</div>
            <ul className="divide-y divide-line-soft">
              {dsr.cancelled_invoices.map(no => <li key={no} className="px-3 py-1.5 font-mono text-red-600 dark:text-red-400">{no}</li>)}
              {!dsr.cancelled_invoices.length && <li className="px-3 py-3 text-center text-stone-400">None.</li>}
            </ul>
          </div>
        )}
      </div>

      {showPrintModal && dsr && (
        <PrintFormatSelectorModal
          category="DSR"
          data={dsr}
          title={`Daily Summary Report (${on})`}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
}
