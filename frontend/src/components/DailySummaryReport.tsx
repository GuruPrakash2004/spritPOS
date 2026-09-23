import { useCallback, useEffect, useState } from 'react';
import { Printer, RefreshCw } from 'lucide-react';
import Toast from './Toast';
import { api, errorText, qs, todayIso, type ApiFullDsr } from './api';
import { field, money, pageSub, pageTitle, panel, secondaryButton } from './ui';

// Legacy frmRptDailySummary; fields follow Docs/WebDash.xlsx.
export default function DailySummaryReport() {
  const [on, setOn] = useState(todayIso());
  const [dsr, setDsr] = useState<ApiFullDsr | null>(null);
  const [error, setError] = useState('');

  const load = useCallback(() => {
    api<ApiFullDsr>(`/reports/dsr${qs({ on })}`).then(d => { setDsr(d); setError(''); }).catch(e => setError(errorText(e)));
  }, [on]);

  useEffect(() => { load(); }, [load]);

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
      <div className={`${panel} flex flex-wrap items-end justify-between gap-3 px-5 py-3`}>
        <div>
          <h2 className={pageTitle}>Daily Summary Report</h2>
          <p className={pageSub}>Sales, stock, payments, purchases and cash for one day.</p>
        </div>
        <div className="flex items-end gap-2 print:hidden">
          <input type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} font-mono`} aria-label="Report date" />
          <button onClick={load} className={secondaryButton}><RefreshCw size={14} />Refresh</button>
          <button onClick={() => window.print()} className={secondaryButton}><Printer size={14} />Print</button>
        </div>
      </div>

      <div className="grid grid-cols-2 gap-3 md:grid-cols-3 xl:grid-cols-4">
        {kpis.map(([k, v]) => (
          <div key={k} className={`${panel} px-4 py-3`}>
            <div className="text-[10px] font-bold uppercase tracking-wider text-stone-500">{k}</div>
            <div className="mt-1 font-mono text-sm font-bold text-stone-900 dark:text-stone-100">{v}</div>
          </div>
        ))}
      </div>

      <div className="grid grid-cols-1 gap-3 md:grid-cols-2 xl:grid-cols-3">
        {tables.map(([title, data]) => (
          <div key={title} className={`${panel} overflow-hidden text-xs`}>
            <div className="border-b border-[#e1c9b6] bg-[#f1e3d8] px-3 py-2 font-bold text-stone-600 dark:border-[#343b59] dark:bg-[#20253a] dark:text-stone-300">{title}</div>
            <table className="w-full">
              <tbody className="divide-y divide-[#f1e3d8] dark:divide-[#343b59]">
                {Object.entries(data).map(([k, v]) => (
                  <tr key={k}><td className="px-3 py-1.5 uppercase text-stone-700 dark:text-stone-200">{k}</td><td className="px-3 py-1.5 text-right font-mono">₹ {money(v)}</td></tr>
                ))}
                {!Object.keys(data).length && <tr><td className="px-3 py-3 text-center text-stone-400">Nothing for this day.</td></tr>}
              </tbody>
            </table>
          </div>
        ))}
        {dsr && (
          <div className={`${panel} overflow-hidden text-xs`}>
            <div className="border-b border-[#e1c9b6] bg-[#f1e3d8] px-3 py-2 font-bold text-stone-600 dark:border-[#343b59] dark:bg-[#20253a] dark:text-stone-300">Cancelled Invoices</div>
            <ul className="divide-y divide-[#f1e3d8] dark:divide-[#343b59]">
              {dsr.cancelled_invoices.map(no => <li key={no} className="px-3 py-1.5 font-mono text-red-600 dark:text-red-400">{no}</li>)}
              {!dsr.cancelled_invoices.length && <li className="px-3 py-3 text-center text-stone-400">None.</li>}
            </ul>
          </div>
        )}
      </div>
    </div>
  );
}
