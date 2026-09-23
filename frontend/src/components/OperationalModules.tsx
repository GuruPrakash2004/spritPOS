import { useEffect, useState } from 'react';
import { Package, Receipt, Truck, TrendingUp, BarChart3 } from 'lucide-react';
import { api, errorText, qs, todayIso, type ApiFullDsr, type ApiIndent, type ApiItem, type ApiStockRow } from './api';
import { money } from './ui';

// --- DASHBOARD ---
export const Dashboard = () => {
  const [dsr, setDsr] = useState<ApiFullDsr | null>(null);
  const [indents, setIndents] = useState<ApiIndent[]>([]);

  // KPI tiles come from today's Daily Summary Report; the charts below are still illustrative.
  useEffect(() => {
    api<ApiFullDsr>(`/reports/dsr${qs({ on: todayIso() })}`).then(setDsr).catch(() => setDsr(null));
    api<ApiIndent[]>('/transactions/purchase-indent').then(setIndents).catch(() => setIndents([]));
  }, []);

  const counters = dsr ? Object.entries(dsr.counter_wise).map(([c, v]) => `${c}: ₹ ${money(v)}`).join(' | ') : '';
  const stats = [
    { label: "Today's Sales", value: dsr ? `₹ ${money(dsr.sales_incl_gst)}` : '—', sub: dsr ? `Profit ₹ ${money(dsr.profit_of_the_day)}` : 'Backend offline', icon: TrendingUp, color: 'text-emerald-500 bg-emerald-500/10' },
    { label: 'Bills Issued', value: dsr ? `${dsr.total_bills} Bills` : '—', sub: counters || 'No bills yet today', icon: Receipt, color: 'text-blue-500 bg-blue-500/10' },
    { label: 'Total Stock Value', value: dsr ? `₹ ${money(dsr.closing_stock.value)}` : '—', sub: dsr ? `${dsr.closing_stock.qty} bottles • ${dsr.closing_stock.litres} L` : '', icon: Package, color: 'text-purple-500 bg-purple-500/10' },
    { label: 'KSBCL Indents', value: `${indents.length} Indents`, sub: `${indents.reduce((acc, i) => acc + i.totals.qty, 0)} bottles ordered`, icon: Truck, color: 'text-amber-500 bg-amber-500/10' },
  ];

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      <div className="bg-linear-115 from-[#27233b] via-[#4b2840] via-48% to-[#8b422b] p-7 rounded-[22px] text-white shadow-[0_18px_55px_rgba(99,39,48,0.28)] flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <span className="px-2.5 py-1 text-xs font-semibold uppercase tracking-wider bg-emerald-500/20 text-emerald-400 rounded-full border border-emerald-500/30">
            Softvent vLQMS Net 8.0
          </span>
          <h2 className="text-2xl sm:text-3xl font-bold font-heading mt-2">THE LIQUOR HOUSE</h2>
          <p className="text-slate-300 text-sm">Byatarayanapura, Bangalore-92 • Karnataka Excise License CL-2</p>
        </div>
        <div className="text-right">
          <div className="text-xs text-slate-400">Financial Year</div>
          <div className="text-sm font-mono font-bold text-emerald-400">01/04/2026 - 31/03/2027</div>
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((s, i) => {
          const IconComp = s.icon;
          return (
            <div key={i} className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
              <div className="flex items-center justify-between">
                <span className="text-xs font-medium text-slate-500 uppercase">{s.label}</span>
                <div className={`p-2.5 rounded-xl ${s.color}`}><IconComp size={18} /></div>
              </div>
              <div className="mt-3 text-2xl font-bold font-heading text-slate-900 dark:text-white">{s.value}</div>
              <div className="mt-1 text-xs text-slate-500">{s.sub}</div>
            </div>
          );
        })}
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">
        <div className="xl:col-span-2 p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
          <div className="flex items-start justify-between mb-6">
            <div><h3 className="font-heading font-bold text-slate-900 dark:text-white">Sales performance</h3><p className="text-xs text-slate-500 mt-1">Bills value in thousands, last 7 days</p></div>
            <span className="text-xs font-semibold text-emerald-600 dark:text-emerald-400">+12.4%</span>
          </div>
          <div className="h-44 flex items-end gap-2 sm:gap-4 border-b border-slate-200 dark:border-slate-800 px-2">
            {[112, 145, 128, 168, 154, 192, 185].map((amount, index) => (
              <div key={amount + index} className="flex-1 h-full flex flex-col justify-end items-center gap-2">
                <span className="text-[10px] text-slate-400">₹{amount}k</span>
                <div className="w-full max-w-10 rounded-t-lg bg-linear-to-b from-[#ff9b5b] to-[#e8702a]" style={{ height: `${(amount / 192) * 78}%` }} />
                <span className="text-[10px] font-medium text-slate-500">{['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'][index]}</span>
              </div>
            ))}
          </div>
        </div>

        <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
          <h3 className="font-heading font-bold text-slate-900 dark:text-white">Inventory mix</h3><p className="text-xs text-slate-500 mt-1">Share of current stock value</p>
          <div className="flex items-center gap-5 mt-5"><div className="h-28 w-28 rounded-full" style={{ background: 'conic-gradient(#10b981 0 42%, #3b82f6 42% 68%, #f59e0b 68% 86%, #f43f5e 86% 100%)' }}><div className="h-16 w-16 rounded-full bg-[#fffaf5] dark:bg-slate-900 m-6 flex items-center justify-center text-xs font-bold text-slate-700 dark:text-slate-200">100%</div></div><div className="space-y-2 text-xs">{[['Whisky', '42%', 'bg-emerald-500'], ['Beer', '26%', 'bg-blue-500'], ['Rum & Vodka', '18%', 'bg-amber-500'], ['Scotch', '14%', 'bg-rose-500']].map(([label, value, color]) => <div key={label} className="flex items-center gap-2"><span className={`h-2 w-2 rounded-full ${color}`} /><span className="text-slate-500">{label}</span><strong className="text-slate-800 dark:text-slate-200">{value}</strong></div>)}</div></div>
        </div>
      </div>

      <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <div className="flex items-center justify-between mb-5"><div><h3 className="font-heading font-bold text-slate-900 dark:text-white">Stock movement</h3><p className="text-xs text-slate-500 mt-1">Today&apos;s inventory flow across the store</p></div><BarChart3 size={18} className="text-emerald-500" /></div>
        <div className="grid grid-cols-3 gap-4">{[['Opening stock', '1,248', '88%', 'bg-slate-400'], ['Received', '+168', '58%', 'bg-blue-500'], ['Issued', '-96', '34%', 'bg-rose-500']].map(([label, value, width, color]) => <div key={label} className="border-l-2 border-slate-200 dark:border-slate-700 pl-3"><p className="text-xs text-slate-500">{label}</p><strong className="text-xl font-heading text-slate-800 dark:text-slate-200">{value}</strong><div className="mt-2 h-1.5 rounded-full bg-slate-100 dark:bg-slate-800"><div className={`h-full rounded-full ${color}`} style={{ width }} /></div></div>)}</div>
      </div>
    </div>
  );
};

// --- STOCK BALANCE REPORT ---
type StockRow = { group: string; brand: string; name: string; pkg: string; opQty: number; recQty: number; issQty: number; clQty: number; rate: number; value: number };

export const StockBalanceReport = () => {
  const [rows, setRows] = useState<StockRow[] | null>(null);
  const [error, setError] = useState('');

  // Today's movement from the backend, so saved Purchase Bills / Sales Invoices show up here.
  useEffect(() => {
    const d = todayIso();
    Promise.all([api<ApiItem[]>('/masters/items'), api<ApiStockRow[]>(`/reports/stock-balance?from_date=${d}&to_date=${d}`)])
      .then(([items, balance]) => {
        const byCode = new Map(items.map(i => [i.code, i]));
        setRows(balance.map(r => ({
          group: r.group, brand: byCode.get(r.item_code)?.brand ?? '', name: r.item_name, pkg: r.package,
          opQty: r.op_qty, recQty: r.rec_qty, issQty: r.iss_qty, clQty: r.cl_qty, rate: byCode.get(r.item_code)?.mrp ?? 0, value: r.cl_value,
        })));
      })
      .catch(e => setError(errorText(e)));
  }, []);

  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex justify-between items-center">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-amber-500/10 text-amber-600 rounded-xl"><BarChart3 size={24} /></div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Stock Balance Report</h2>
            <p className="text-xs text-slate-500">Excise opening, receipt, issued & closing ledger balances.</p>
          </div>
        </div>
      </div>

      <div className="overflow-x-auto rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <table className="w-full text-xs text-left">
          <thead className="bg-slate-50 dark:bg-slate-800 text-slate-500 uppercase">
            <tr>
              <th className="px-3 py-3">Group</th>
              <th className="px-3 py-3">Brand</th>
              <th className="px-3 py-3">Item Description</th>
              <th className="px-3 py-3 text-center">Pkg</th>
              <th className="px-3 py-3 text-right">Opening</th>
              <th className="px-3 py-3 text-right">Receipt</th>
              <th className="px-3 py-3 text-right">Issued</th>
              <th className="px-3 py-3 text-right font-bold text-slate-900 dark:text-white">Closing</th>
              <th className="px-3 py-3 text-right">Rate (₹)</th>
              <th className="px-3 py-3 text-right font-bold text-emerald-600">Closing Value (₹)</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 dark:divide-slate-800 font-mono">
            {error && <tr><td colSpan={10} className="px-3 py-3 font-sans text-red-600 dark:text-red-400">{error}</td></tr>}
            {!error && !rows && <tr><td colSpan={10} className="px-3 py-3 font-sans text-slate-500">Loading stock from the backend…</td></tr>}
            {rows?.map((row, i) => (
              <tr key={i} className="hover:bg-slate-50/50">
                <td className="px-3 py-2.5 font-sans font-medium text-slate-600">{row.group}</td>
                <td className="px-3 py-2.5 font-sans text-slate-700">{row.brand}</td>
                <td className="px-3 py-2.5 font-sans font-medium text-slate-900 dark:text-white">{row.name}</td>
                <td className="px-3 py-2.5 text-center text-slate-500">{row.pkg}</td>
                <td className="px-3 py-2.5 text-right text-slate-500">{row.opQty}</td>
                <td className="px-3 py-2.5 text-right text-blue-600">+{row.recQty}</td>
                <td className="px-3 py-2.5 text-right text-rose-600">-{row.issQty}</td>
                <td className="px-3 py-2.5 text-right font-bold text-slate-900 dark:text-white">{row.clQty}</td>
                <td className="px-3 py-2.5 text-right">₹ {row.rate.toFixed(2)}</td>
                <td className="px-3 py-2.5 text-right font-bold text-emerald-600">₹ {row.value.toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
