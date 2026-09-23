import { useEffect, useState } from 'react';
import { PieChart, Mail, Download, Eye, ChevronRight, X, Loader2 } from 'lucide-react';
import Toast from './Toast';
import { api, errorText } from './api';
import { downloadXls, type ReportColumn, type ReportRow } from './xlsExport';

const toDmy = (iso: string) => iso.split('-').reverse().join('/');

type ReportResult = { name: string; from_date: string; to_date: string; columns: ReportColumn[]; rows: ReportRow[]; filterKey?: string };
type Master = { code: string; name: string };
type ActiveFilter = { on: string; val: string; param?: MasterParam; code?: string };

// Filter On choices that pick a master record; the date choices use From / To instead.
const MASTER_FILTERS = {
  'Ledger Name': { param: 'ledger', path: '/masters/ledgers' },
  'Supplier Name': { param: 'supplier', path: '/masters/suppliers' },
  'Item Group': { param: 'group', path: '/masters/item-groups' },
} as const;
type MasterParam = (typeof MASTER_FILTERS)[keyof typeof MASTER_FILTERS]['param'];
const masterFilter = (on: string) => MASTER_FILTERS[on as keyof typeof MASTER_FILTERS];

const fmtCell = (v: ReportRow[string], col: ReportColumn) => {
  if (typeof v === 'number') return v.toLocaleString('en-IN', { maximumFractionDigits: col.key === 'litres' ? 3 : 2 });
  if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(v)) return toDmy(v);
  return v ?? '';
};

const ReportsDashboard = () => {
  const [selectedReport, setSelectedReport] = useState<string>('Expense Ledger Report');
  const [filterOn, setFilterOn] = useState<string>('Exp Date');
  const [activeFilters, setActiveFilters] = useState<ActiveFilter[]>([
    { on: 'Date Range', val: '01/09/2026 to 21/09/2026' }
  ]);

  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [filterValue, setFilterValue] = useState('');
  const [masters, setMasters] = useState<Record<string, Master[]>>({});

  // Load the value list the first time a master-based Filter On is picked.
  useEffect(() => {
    const mf = masterFilter(filterOn);
    if (!mf || masters[filterOn]) return;
    api<Master[]>(mf.path)
      .then(list => setMasters(m => ({ ...m, [filterOn]: list })))
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  }, [filterOn, masters]);

  const [fromDate, setFromDate] = useState('2026-09-01');
  const [toDate, setToDate] = useState('2026-09-21');
  const [result, setResult] = useState<ReportResult | null>(null);
  const [busy, setBusy] = useState(false);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  const reportCategories = [
    {
      cat: 'Expense Report',
      reports: ['Expense Ledger Report', 'Expense Report']
    },
    {
      cat: 'Purchase Report',
      reports: [
        '1.GRN Wise Purchase Report',
        '2.Day Wise Group Wise Purchase Report',
        '3.Supplier Bill Wise Purchase Report',
        'Item Wise Purchase Report-Day Wise',
        'Item Wise Purchase Report-Bill Wise',
        'Item Wise Purchase Report-Month Wise',
        'ItemWise Purchase Report',
        'MRP RATES',
        'Purchase Report- Item Wise Summary',
        'Purchase Report-MonthWise Summary'
      ]
    },
    {
      cat: 'Sales Report',
      reports: [
        '1.Deleted Invoice Report',
        '2.Sales-Item Wise Report',
        '3.Item wise Sales Report - Bill Wise',
        'Cancelled Invoice report',
        'Item wise Sales Report - Day Wise',
        'Item wise Sales Report - Month Wise',
        'Sales - Bill Wise Report',
        'Sales - Counter Wise Report',
        'Sales - Day Wise Report',
        'Sales - Group Wise Report',
        'Sales - Hourly Report',
        'Sales - Item Wise Report',
        'Sales - Month Wise Report',
        'Sales - PackageItem Wise Report',
        'Sales - Sales Person Wise Report',
        'Sales Other Payment',
        'Sales Return - Day Wise Report',
        'Sales Return Report'
      ]
    }
  ];

  const addFilter = () => {
    if (!fromDate || !toDate) return setToast({ kind: 'error', text: 'Pick both From and To dates.' });
    if (fromDate > toDate) return setToast({ kind: 'error', text: 'From Date cannot be after To Date.' });
    const mf = masterFilter(filterOn);
    if (mf && !filterValue) return setToast({ kind: 'error', text: `Pick a ${filterOn} to filter on.` });
    const val = mf
      ? masters[filterOn]?.find(m => m.code === filterValue)?.name ?? filterValue
      : `${toDmy(fromDate)} to ${toDmy(toDate)}`;
    if (activeFilters.some(f => f.on === filterOn && f.val === val)) return setToast({ kind: 'error', text: 'That filter is already applied.' });
    setActiveFilters([...activeFilters, mf ? { on: filterOn, val, param: mf.param, code: filterValue } : { on: filterOn, val }]);
  };

  // Email is still a stub: there is no mail server configured.
  const emailStub = () => setToast({ kind: 'ok', text: `${selectedReport} emailed` });

  const fetchReport = async (): Promise<ReportResult | null> => {
    if (!fromDate || !toDate) { setToast({ kind: 'error', text: 'Pick both From and To dates.' }); return null; }
    if (fromDate > toDate) { setToast({ kind: 'error', text: 'From Date cannot be after To Date.' }); return null; }
    // Reuse what is on screen when nothing has changed since Generate.
    const params = new URLSearchParams({ name: selectedReport, from_date: fromDate, to_date: toDate });
    activeFilters.forEach(f => { if (f.param && f.code) params.append(f.param, f.code); });
    const filterKey = params.toString();
    if (result?.filterKey === filterKey) return result;
    setBusy(true);
    try {
      const res = { ...(await api<ReportResult>(`/reports/custom?${filterKey}`)), filterKey };
      setResult(res);
      return res;
    } catch (e) {
      setToast({ kind: 'error', text: errorText(e) });
      return null;
    } finally {
      setBusy(false);
    }
  };

  const generate = async () => {
    const res = await fetchReport();
    if (res) setToast({ kind: 'ok', text: `${res.rows.length} row${res.rows.length === 1 ? '' : 's'} found` });
  };

  const exportXls = async () => {
    const res = await fetchReport();
    if (!res) return;
    if (!res.rows.length) return setToast({ kind: 'error', text: 'Nothing to export for this period.' });
    downloadXls(`${res.name} ${res.from_date} to ${res.to_date}`, `THE LIQUOR HOUSE - ${res.name}`,
      `Period: ${toDmy(res.from_date)} to ${toDmy(res.to_date)}`, res.columns, res.rows);
    setToast({ kind: 'ok', text: `${res.name} exported to XLS` });
  };

  const removeFilter = (idx: number) => {
    setActiveFilters(activeFilters.filter((_, i) => i !== idx));
  };

  return (
    <div className="max-w-7xl mx-auto space-y-6">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4">
        <div className="flex items-center gap-3 min-w-0">
          <div className="p-3 bg-[#e8702a]/15 text-[#b84716] dark:text-[#ffb07e] rounded-xl">
            <PieChart size={24} />
          </div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Customised Reports Explorer</h2>
            <p className="text-xs text-slate-500">Comprehensive audit & MIS reporting across 30+ liquor modules.</p>
          </div>
        </div>
        <div className="grid grid-cols-2 sm:flex sm:items-center gap-2 shrink-0">
          <button onClick={emailStub} className="px-3.5 py-2 bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-300 rounded-xl text-xs font-medium flex items-center justify-center gap-1.5 whitespace-nowrap">
            <Mail size={14} /> Email Report
          </button>
          <button onClick={exportXls} disabled={busy} className="disabled:opacity-60 px-3.5 py-2 bg-[#e8702a] text-white rounded-xl text-xs font-semibold flex items-center justify-center gap-1.5 whitespace-nowrap hover:bg-[#b84716] shadow-sm">
            <Download size={14} /> Export XLS
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
        {/* Left Side: Report Tree */}
        <div className="lg:col-span-4 p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-4">
          <div className="max-h-[600px] overflow-y-auto space-y-4 pr-1">
            {reportCategories.map((group, idx) => (
              <div key={idx} className="space-y-1">
                <div className="text-[11px] font-bold uppercase tracking-wider text-slate-400 px-2 py-1">
                  {group.cat}
                </div>
                <div className="space-y-0.5">
                  {group.reports.map((rep) => (
                    <button
                      key={rep}
                      onClick={() => setSelectedReport(rep)}
                      className={`w-full text-left px-3 py-2 rounded-xl text-xs transition-colors flex items-center justify-between ${
                        selectedReport === rep
                          ? 'bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 font-bold border-l-4 border-l-emerald-500'
                          : 'text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800'
                      }`}
                    >
                      <span className="truncate">{rep}</span>
                      {selectedReport === rep && <ChevronRight size={12} />}
                    </button>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Right Side: Filters */}
        <div className="lg:col-span-8 space-y-6">
          <div className="p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-5">
            <div className="border-b pb-3 border-slate-100 dark:border-slate-800">
              <span className="text-xs text-slate-400">Report to be Viewed</span>
              <h3 className="font-heading font-bold text-lg text-slate-900 dark:text-white mt-0.5">{selectedReport}</h3>
            </div>

            <div className={`grid grid-cols-1 gap-4 text-xs ${masterFilter(filterOn) ? 'sm:grid-cols-2 lg:grid-cols-4' : 'sm:grid-cols-3'}`}>
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">Filter On</label>
                <select value={filterOn} onChange={(e) => { setFilterOn(e.target.value); setFilterValue(''); }} className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800">
                  <option>Exp Date</option>
                  <option>Bill Date</option>
                  <option>Ledger Name</option>
                  <option>Supplier Name</option>
                  <option>Item Group</option>
                </select>
              </div>
              {masterFilter(filterOn) && (
                <div>
                  <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">{filterOn}</label>
                  <select value={filterValue} onChange={(e) => setFilterValue(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800">
                    <option value="">{masters[filterOn] ? `Select ${filterOn}…` : 'Loading…'}</option>
                    {masters[filterOn]?.map(m => <option key={m.code} value={m.code}>{m.name}</option>)}
                  </select>
                </div>
              )}
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">From Date</label>
                <input type="date" value={fromDate} onChange={(e) => setFromDate(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800 font-mono" />
              </div>
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">To Date</label>
                <input type="date" value={toDate} onChange={(e) => setToDate(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800 font-mono" />
              </div>
            </div>

            <div className="flex justify-between items-center pt-2">
              <button onClick={addFilter} className="px-3 py-1.5 text-xs font-semibold bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 rounded-lg text-slate-700 dark:text-slate-300">
                + Apply Selected Filter
              </button>
              <button onClick={() => setActiveFilters([])} className="text-xs text-rose-500 hover:underline">
                Clear All Filters
              </button>
            </div>

            <div className="rounded-xl border border-slate-200 dark:border-slate-800 overflow-hidden">
              <table className="w-full text-xs text-left">
                <thead className="bg-slate-50 dark:bg-slate-800/60 text-slate-500 uppercase tracking-wider">
                  <tr>
                    <th className="px-4 py-2">Filtered On</th>
                    <th className="px-4 py-2">Filtered Value</th>
                    <th className="px-4 py-2 text-right">Action</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
                  {activeFilters.map((f, i) => (
                    <tr key={i}>
                      <td className="px-4 py-2 font-semibold text-slate-700 dark:text-slate-300">{f.on}</td>
                      <td className="px-4 py-2 font-mono text-slate-600 dark:text-slate-400">{f.val}</td>
                      <td className="px-4 py-2 text-right">
                        <button onClick={() => removeFilter(i)} className="text-slate-400 hover:text-rose-500">
                          <X size={14} />
                        </button>
                      </td>
                    </tr>
                  ))}
                  {activeFilters.length === 0 && (
                    <tr>
                      <td colSpan={3} className="px-4 py-4 text-center text-slate-400 italic">No custom filters active (showing full period data)</td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>

            <div className="flex justify-end pt-2">
              <button onClick={generate} disabled={busy} className="disabled:opacity-60 px-6 py-2.5 bg-[#e8702a] hover:bg-[#b84716] text-white font-bold rounded-xl text-xs shadow-md transition-all flex items-center gap-2">
                {busy ? <Loader2 size={14} className="animate-spin" /> : <Eye size={14} />} Generate & View Report
              </button>
            </div>
          </div>
        </div>
      </div>

      {result && (
        <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 space-y-3">
          <div className="flex flex-col sm:flex-row sm:items-baseline sm:justify-between gap-1">
            <h3 className="font-heading font-bold text-slate-900 dark:text-white">{result.name}</h3>
            <span className="text-xs text-slate-500">
              {toDmy(result.from_date)} to {toDmy(result.to_date)} · {result.rows.length} row{result.rows.length === 1 ? '' : 's'}
            </span>
          </div>
          {/* Wide reports scroll sideways inside the card so the page itself never scrolls horizontally on mobile. */}
          <div className="rounded-xl border border-slate-200 dark:border-slate-800 overflow-x-auto max-h-[520px] overflow-y-auto">
            <table className="w-full text-xs text-left whitespace-nowrap">
              <thead className="sticky top-0 bg-[#f1e3d8] dark:bg-[#20253a] text-slate-600 dark:text-slate-300 uppercase tracking-wider">
                <tr>
                  {result.columns.map(c => (
                    <th key={c.key} className={`px-3 py-2 ${c.align === 'right' ? 'text-right' : ''}`}>{c.label}</th>
                  ))}
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
                {result.rows.map((row, i) => (
                  <tr key={i} className="text-slate-700 dark:text-slate-300">
                    {result.columns.map(c => (
                      <td key={c.key} className={`px-3 py-2 ${c.align === 'right' ? 'text-right font-mono' : ''}`}>{fmtCell(row[c.key], c)}</td>
                    ))}
                  </tr>
                ))}
                {result.rows.length === 0 && (
                  <tr>
                    <td colSpan={result.columns.length} className="px-4 py-6 text-center text-slate-400 italic">
                      No transactions in this period.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default ReportsDashboard;