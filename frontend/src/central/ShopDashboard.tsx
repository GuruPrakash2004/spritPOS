// Central site → Day summary: one shop's day, or every shop together, as the shops pushed it (the desktop's
// central DSR tables). "All shops" adds the figures up and lists each shop on its own line, with a warning
// for a shop that hasn't sent this day or hasn't synced lately.

import { useCallback, useEffect, useState } from 'react';
import { AlertTriangle, Search } from 'lucide-react';
import { toastStore } from '../components/common/toastStore';
import { InlineError } from '../components/common/FormErrorSummary';
import { api, errorText, todayIso } from '../lib/api';
import { badge, field, fieldError, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow } from '../lib/styles';
import { isStale, when, type CentralDsr, type CentralShop, type DsrRow } from './types';

type Col = { key: string; label: string; money?: boolean };

function SmallTable({ title, rows, cols }: { title: string; rows: DsrRow[] | undefined; cols: Col[] }) {
  return (
    <section className={`${panel} overflow-hidden`}>
      <h3 className="border-b border-[#E3D9CD] px-3 py-2 text-xs font-semibold text-[#29231F] dark:border-line dark:text-stone-100">{title}</h3>
      {!rows?.length ? <p className="px-3 py-3 text-xs text-stone-500">Nothing for this day.</p> : (
        <div className="max-h-72 overflow-auto">
          <table className="min-w-full text-xs">
            <thead><tr>{cols.map(c => <th key={c.key} className={`${tableHead} px-3 py-1.5 ${c.money ? 'text-right' : ''}`}>{c.label}</th>)}</tr></thead>
            <tbody className={tableBody}>
              {rows.map((r, i) => (
                <tr key={i} className={tableRow}>
                  {cols.map(c => <td key={c.key} className={`px-3 py-1.5 ${c.money ? 'text-right tabular-nums' : ''}`}>{c.money ? money(Number(r[c.key] ?? 0)) : String(r[c.key] ?? '')}</td>)}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}

const TILES: [string, string][] = [
  ['TotBills', 'Bills'], ['TotSalesAmt', 'Sales (incl. GST)'], ['SalesAmtLiq', 'Liquor (basic)'], ['SalesAmtNonLiq', 'Non-liquor (basic)'],
  ['SalesProfitAmt', 'Profit'], ['TotCashAmt', 'Cash'], ['TotCardAmt', 'Card'], ['OtherAmt', 'UPI / other'], ['CreditAmt', 'Credit'],
  ['PurAmtInclGST', 'Purchase'], ['TotSalesRetnAmt', 'Sales returns'], ['ExpenseAmt', 'Expenses'], ['CashBalanceAmt', 'Cash balance'],
  ['ClStkQty', 'Closing stock (qty)'], ['CLStkValue', 'Closing stock value'], ['MnthSalesValue', 'Sales this month'], ['YearSalesValue', 'Sales this year'],
];
const COUNT_TILES = new Set(['TotBills', 'ClStkQty']);

export default function ShopDashboard({ shops }: { shops: CentralShop[] }) {
  const [client, setClient] = useState('');
  const [day, setDay] = useState(todayIso);
  const [dayError, setDayError] = useState('');
  const [data, setData] = useState<CentralDsr | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback((c: string, d: string) =>
    api<CentralDsr>(`/dsr?${new URLSearchParams({ day: d, ...(c ? { client: c } : {}) })}`)
      .then(r => { setData(r); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; })
      .finally(() => setLoading(false)), []);

  useEffect(() => { load('', todayIso()); }, [load]);

  const show = async () => {
    if (!day) {
      setDayError('Pick a day');
      document.getElementById('cd-day')?.focus();
      return;
    }
    setDayError('');
    if (!(await toastStore.defer('refresh', 'The day summary will be loaded when this closes.', 'Cancelled.'))) return;
    setLoading(true);
    if (await load(client, day)) toastStore.add('refresh', 'Day summary loaded.');
  };

  const t = data?.tables ?? {};
  const all = !data?.client;
  return (
    <div className="flex flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Day summary</h1>
          <p className={pageSub}>What each shop sent for the day (its Daily Summary Report). Shops send every few minutes while their backend runs.</p>
        </div>
      </div>

      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div className="min-w-[200px]">
          <label htmlFor="cd-shop" className={label}>Shop</label>
          <select id="cd-shop" value={client} onChange={e => setClient(e.target.value)} className={field}>
            <option value="">All shops</option>
            {shops.map(s => <option key={s.code} value={s.code}>{s.code} · {s.name}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="cd-day" className={label}>Day</label>
          <input id="cd-day" type="date" value={day} onChange={e => { setDay(e.target.value); setDayError(''); }}
            className={`${field} ${dayError ? fieldError : ''}`} aria-invalid={!!dayError} />
          <InlineError message={dayError} />
        </div>
        <button className={primaryButton} onClick={show} disabled={loading}><Search size={14} />Show</button>
      </section>

      {loading && !data ? <p className="text-xs text-stone-500">Loading…</p> : !data?.summary ? (
        <section className={`${panel} p-6 text-center text-sm text-stone-500`}>No shop has sent this day yet.</section>
      ) : (
        <>
          <section className="grid grid-cols-2 gap-3 sm:grid-cols-3 lg:grid-cols-6" aria-label="Totals">
            {TILES.map(([key, text]) => (
              <div key={key} className={`${panel} p-3`}>
                <p className="text-[11px] font-semibold uppercase tracking-wide text-[#70675F] dark:text-stone-400">{text}</p>
                <p className="mt-1 text-base font-bold tabular-nums text-[#29231F] dark:text-stone-50">
                  {COUNT_TILES.has(key) ? Number(data.summary![key] ?? 0).toLocaleString() : `₹ ${money(Number(data.summary![key] ?? 0))}`}
                </p>
              </div>
            ))}
          </section>

          {all && (
            <section className={`${panel} overflow-hidden`}>
              <h3 className="border-b border-[#E3D9CD] px-3 py-2 text-xs font-semibold dark:border-line">Shops</h3>
              <div className="overflow-auto">
                <table className="min-w-full text-xs">
                  <thead><tr>{['Shop', 'Bills', 'Sales', 'Profit', 'Cash', 'Expenses', 'Closing stock', 'Last sync'].map(h => <th key={h} className={`${tableHead} px-3 py-1.5`}>{h}</th>)}</tr></thead>
                  <tbody className={tableBody}>
                    {data.shops.map(s => (
                      <tr key={s.client} className={tableRow}>
                        <td className="px-3 py-1.5 font-medium">{s.client} <span className="text-stone-500">{s.name}</span></td>
                        {s.synced ? <>
                          <td className="px-3 py-1.5 tabular-nums">{s.TotBills}</td>
                          <td className="px-3 py-1.5 tabular-nums">{money(s.TotSalesAmt ?? 0)}</td>
                          <td className="px-3 py-1.5 tabular-nums">{money(s.SalesProfitAmt ?? 0)}</td>
                          <td className="px-3 py-1.5 tabular-nums">{money(s.TotCashAmt ?? 0)}</td>
                          <td className="px-3 py-1.5 tabular-nums">{money(s.ExpenseAmt ?? 0)}</td>
                          <td className="px-3 py-1.5 tabular-nums">{(s.ClStkQty ?? 0).toLocaleString()}</td>
                        </> : <td colSpan={6} className="px-3 py-1.5"><span className={badge('warning')}><AlertTriangle size={11} />Hasn't sent this day</span></td>}
                        <td className="px-3 py-1.5">{when(s.last_sync)} {isStale(s.last_sync) && <span className={badge('danger')}>Not syncing</span>}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </section>
          )}

          <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-3">
            <SmallTable title="Payments" rows={t.RptDSRPmt} cols={[{ key: 'Particulars', label: 'Mode' }, { key: 'PmtAmt', label: 'Amount', money: true }]} />
            <SmallTable title="Counter-wise sales" rows={t.RptDSRCtrSales} cols={[{ key: 'CounterNo', label: 'Counter' }, { key: 'NoOfBills', label: 'Bills' }, { key: 'BillValue', label: 'Value', money: true }]} />
            <SmallTable title="Salesperson-wise sales" rows={t.RptDSRSPWise} cols={[{ key: 'CounterNo', label: 'Salesperson' }, { key: 'NoOfBills', label: 'Bills' }, { key: 'BillValue', label: 'Value', money: true }]} />
            <SmallTable title="Group-wise sales" rows={t.RptDSRGroupWiseSales} cols={[{ key: 'GroupName', label: 'Group' }, { key: 'TotQty', label: 'Qty' }, { key: 'TotAmt', label: 'Amount', money: true }]} />
            <SmallTable title="Group-wise purchase (at MRP)" rows={t.RptDSRGroupWisePur} cols={[{ key: 'GroupName', label: 'Group' }, { key: 'TotQty', label: 'Qty' }, { key: 'TotAmt', label: 'Amount', money: true }]} />
            <SmallTable title="Expenses" rows={t.RptDSRExpLedWise} cols={[{ key: 'LedgerName', label: 'Ledger' }, { key: 'LedAmt', label: 'Amount', money: true }]} />
            <SmallTable title="GST" rows={t.RptDSR_Gst} cols={[{ key: 'GST', label: 'Rate' }, { key: 'GSTAmt', label: 'Amount', money: true }]} />
            <SmallTable title="Fast moving (top 20)" rows={t.RptFastMov} cols={[{ key: 'ItemName', label: 'Item' }, { key: 'SoldQty', label: 'Sold' }]} />
            <SmallTable title="Below minimum stock" rows={t.RptMinStk} cols={[{ key: 'ItemName', label: 'Item' }, { key: 'QOH', label: 'Stock' }, { key: 'MinLevel', label: 'Min' }, { key: 'ShortageQty', label: 'Short' }]} />
            <SmallTable title="Cancelled bills" rows={t.RptDSRInvCancelled} cols={[{ key: 'CounterNo', label: 'Counter' }, { key: 'NoOfBills', label: 'Bills' }, { key: 'BillValue', label: 'Value', money: true }]} />
          </div>
        </>
      )}
    </div>
  );
}
