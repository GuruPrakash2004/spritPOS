// Daily Summary Report / DSR (legacy frmRptDailySummary; fields follow Docs/WebDash.xlsx):
// KPI tiles and payment, counter, salesperson, group, expense and cancelled-bill tables from
// GET /reports/dsr. Printable in the DSR audit formats.

import { useCallback, useEffect, useState } from 'react';
import { Printer, RefreshCw } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { errorText, getDsr, getDsrFormats, renderDsrPrint, todayIso, type ApiFullDsr } from '../../lib/api';
import ReportPrintModal from '../print/ReportPrintModal';
import {  field, money, pageSub, pageTitle, panel, secondaryButton, pageHeader , printButton } from '../../lib/styles';

// Legacy frmRptDailySummary; fields follow Docs/WebDash.xlsx.
export default function DailySummaryReport() {
  const [on, setOn] = useState(todayIso());
  const [dsr, setDsr] = useState<ApiFullDsr | null>(null);
  const [loadedAt, setLoadedAt] = useState<Date | null>(null);
  const [refreshing, setRefreshing] = useState(false);
  const [formats, setFormats] = useState<string[]>([]);
  const [showPrint, setShowPrint] = useState(false);

  const load = useCallback(() => {
    return getDsr(on)
      .then(d => { setDsr(d); setLoadedAt(new Date()); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; });
  }, [on]);

  useEffect(() => { load(); }, [load]);

  // Stable between renders, so the modal's preview is asked for once per format.
  const renderOn = useCallback((rpt: string, printer?: string) => renderDsrPrint(on, rpt, printer), [on]);

  useEffect(() => {
    getDsrFormats().then(r => setFormats(r.formats)).catch(() => { /* printing says why */ });
  }, []);

  // Every press fetches the report again and says so. The popup goes to toastStore directly: a <Toast>
  // with the same text as last time would not show again, so a second Refresh looked like it did nothing.
  const handleRefresh = async () => {
    // The refresh waits for its popup; Cancel stops it.
    if (!(await toastStore.defer('refresh', `Daily Summary Report for ${on} will refresh when this closes.`, 'Refresh cancelled.'))) return;
    setRefreshing(true);
    try {
      if (await load()) toastStore.add('refresh', `Daily Summary Report for ${on} refreshed.`);
    } finally {
      setRefreshing(false);
    }
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
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Daily Summary Report</h1>
          <p className={pageSub}>
            Sales, stock, payments, purchases and cash for one day.
            {loadedAt && <span className="tabular-nums"> · Updated {loadedAt.toLocaleTimeString()}</span>}
          </p>
        </div>
        <div className="flex items-end gap-2 print:hidden">
          <input type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} tabular-nums`} aria-label="Report date" />
          <button onClick={handleRefresh} disabled={refreshing} className={secondaryButton}><RefreshCw size={14} className={refreshing ? "animate-spin" : ""} />{refreshing ? "Refreshing…" : "Refresh"}</button>
          <button onClick={() => setShowPrint(true)} disabled={!dsr?.total_bills} title={dsr?.total_bills ? undefined : 'No bills on this day: nothing to print'} className={printButton}><Printer size={14} />Print</button>
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

      {showPrint && (
        <ReportPrintModal
          title={`Daily Summary Report (${on})`}
          formats={formats}
          render={renderOn}
          onClose={() => setShowPrint(false)}
        />
      )}
    </div>
  );
}
