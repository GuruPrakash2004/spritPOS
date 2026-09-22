import { useState } from 'react';
import { PieChart, Mail, Download, Eye, ChevronRight, X } from 'lucide-react';

const ReportsDashboard = () => {
  const [selectedReport, setSelectedReport] = useState<string>('Expense Ledger Report');
  const [filterOn, setFilterOn] = useState<string>('Exp Date');
  const [activeFilters, setActiveFilters] = useState<{ on: string; val: string }[]>([
    { on: 'Date Range', val: '01/09/2026 to 21/09/2026' }
  ]);

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
    setActiveFilters([...activeFilters, { on: filterOn, val: 'Selected Period' }]);
  };

  const removeFilter = (idx: number) => {
    setActiveFilters(activeFilters.filter((_, i) => i !== idx));
  };

  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex justify-between items-center">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-teal-500/10 text-teal-600 dark:text-teal-400 rounded-xl">
            <PieChart size={24} />
          </div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Customised Reports Explorer</h2>
            <p className="text-xs text-slate-500">Comprehensive audit & MIS reporting across 30+ liquor modules.</p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          <button className="px-3.5 py-2 bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-300 rounded-xl text-xs font-medium flex items-center gap-1.5">
            <Mail size={14} /> Email Report
          </button>
          <button className="px-3.5 py-2 bg-emerald-600 text-white rounded-xl text-xs font-semibold flex items-center gap-1.5 shadow-sm">
            <Download size={14} /> Export XLS
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
        {/* Left Side: Report Tree */}
        <div className="lg:col-span-4 p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-4">
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
          <div className="p-6 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-5">
            <div className="border-b pb-3 border-slate-100 dark:border-slate-800">
              <span className="text-xs text-slate-400">Report to be Viewed</span>
              <h3 className="font-heading font-bold text-lg text-slate-900 dark:text-white mt-0.5">{selectedReport}</h3>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 text-xs">
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">Filter On</label>
                <select value={filterOn} onChange={(e) => setFilterOn(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800">
                  <option>Exp Date</option>
                  <option>Bill Date</option>
                  <option>Ledger Name</option>
                  <option>Supplier Name</option>
                  <option>Item Group</option>
                </select>
              </div>
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">From Date</label>
                <input type="date" defaultValue="2026-09-01" className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 font-mono" />
              </div>
              <div>
                <label className="block font-semibold text-slate-600 dark:text-slate-300 mb-1">To Date</label>
                <input type="date" defaultValue="2026-09-21" className="w-full px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 font-mono" />
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
              <button className="px-6 py-2.5 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-xl text-xs shadow-md transition-all flex items-center gap-2">
                <Eye size={14} /> Generate & View Report
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ReportsDashboard;