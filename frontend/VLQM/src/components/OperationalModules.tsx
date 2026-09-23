import { useState } from 'react';
import {
  Package, Receipt, Truck, TrendingUp,
  ChevronLeft, ChevronRight, BarChart3,

} from 'lucide-react';
import { SAMPLE_STOCK_DATA } from './stockData';

// --- DASHBOARD ---
export const Dashboard = () => {
  const stats = [
    { label: "Today's Sales", value: '₹ 1,84,650.00', sub: '+12.4% vs yesterday', icon: TrendingUp, color: 'text-emerald-500 bg-emerald-500/10' },
    { label: 'Bills Issued', value: '142 Bills', sub: 'Counter 1: 118 | Delivery: 24', icon: Receipt, color: 'text-blue-500 bg-blue-500/10' },
    { label: 'Total Stock Value', value: '₹ 24,18,500.00', sub: '1,248 Items • 14 Categories', icon: Package, color: 'text-purple-500 bg-purple-500/10' },
    { label: 'Pending KSBCL Indents', value: '2 Indents', sub: 'Depot 12 • 48 Cases', icon: Truck, color: 'text-amber-500 bg-amber-500/10' },
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

// --- ITEM MASTER (Page 6) ---
export const ItemMaster = () => {
  const [idx, setIdx] = useState(0);
  const current = SAMPLE_STOCK_DATA[idx];

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <div>
          <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Item Master</h2>
          <p className="text-xs text-slate-500">Liquor catalogue, KSBCL barcode mappings, MRP & excise taxes (Page 6).</p>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={() => setIdx((idx - 1 + SAMPLE_STOCK_DATA.length) % SAMPLE_STOCK_DATA.length)} className="p-2 rounded-xl bg-slate-100 dark:bg-slate-800 hover:bg-slate-200">
            <ChevronLeft size={16} />
          </button>
          <span className="text-xs font-mono text-slate-500">{idx + 1} of {SAMPLE_STOCK_DATA.length}</span>
          <button onClick={() => setIdx((idx + 1) % SAMPLE_STOCK_DATA.length)} className="p-2 rounded-xl bg-slate-100 dark:bg-slate-800 hover:bg-slate-200">
            <ChevronRight size={16} />
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-4">
          <div>
            <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Item Code / Description</label>
            <input type="text" value={current.name} readOnly className="w-full px-3 py-2 text-xs font-mono font-medium rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800" />
          </div>
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">UOM</label>
              <input type="text" defaultValue="Bottle" className="w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">HSN Code</label>
              <input type="text" defaultValue="22083011" className="w-full px-3 py-2 text-xs font-mono rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Item Group</label>
              <input type="text" defaultValue={current.group} className="w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Category</label>
              <input type="text" defaultValue={current.cat} className="w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800 font-bold text-emerald-600" />
            </div>
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Bottles per Case</label>
            <input type="number" defaultValue="12" className="w-32 px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-[#fffaf5] dark:bg-slate-800" />
          </div>
        </div>

        <div className="p-6 rounded-2xl bg-slate-50 dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-3 text-xs">
          <h3 className="font-heading font-bold text-sm text-slate-900 dark:text-white border-b pb-2">Excise & Pricing</h3>
          <div className="flex justify-between py-1">
            <span className="text-slate-500">Item Purchase Rate:</span>
            <span className="font-mono font-bold">₹ {current.purchaseRate.toFixed(2)}</span>
          </div>
          <div className="flex justify-between py-1">
            <span className="text-slate-500">Maximum Retail Price (MRP):</span>
            <span className="font-mono font-bold text-emerald-600">₹ {current.rate.toFixed(2)}</span>
          </div>
          <div className="flex justify-between py-1">
            <span className="text-slate-500">Brand:</span>
            <span className="font-bold">{current.brand}</span>
          </div>
          <div className="flex justify-between py-1">
            <span className="text-slate-500">Package:</span>
            <span className="font-mono">{current.pkg}</span>
          </div>
          <div className="pt-2 border-t">
            <span className="text-slate-500 block mb-1">KSBCL Item Code:</span>
            <div className="p-2 font-mono font-bold bg-[#fffaf5] dark:bg-slate-800 rounded-lg border">{current.ksbcl}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

// --- STOCK BALANCE REPORT (Page 24) ---
export const StockBalanceReport = () => {
  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex justify-between items-center">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-amber-500/10 text-amber-600 rounded-xl"><BarChart3 size={24} /></div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Stock Balance Report</h2>
            <p className="text-xs text-slate-500">Excise opening, receipt, issued & closing ledger balances (Page 24).</p>
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
            {SAMPLE_STOCK_DATA.map((row, i) => (
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
                <td className="px-3 py-2.5 text-right font-bold text-emerald-600">₹ {(row.clQty * row.purchaseRate).toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

// --- EXPENSE ENTRY (Page 19) ---
export const ExpenseEntry = () => (
  <div className="max-w-5xl mx-auto p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Expense Entry (Page 19)</h2>
    <div className="overflow-x-auto rounded-xl border">
      <table className="w-full text-xs text-left">
        <thead className="bg-slate-50 uppercase">
          <tr><th className="p-3">Sl</th><th className="p-3">Particulars / Ledger</th><th className="p-3 text-right">Amount (₹)</th><th className="p-3">Remarks</th></tr>
        </thead>
        <tbody className="divide-y font-mono">
          <tr><td className="p-3">1</td><td className="p-3 font-sans font-semibold">Generator Diesel & Fuel</td><td className="p-3 text-right font-bold">₹ 2,200.00</td><td className="p-3 font-sans text-slate-500">Backup power generator fuel</td></tr>
          <tr><td className="p-3">2</td><td className="p-3 font-sans font-semibold">Shop Refreshments & Tea</td><td className="p-3 text-right font-bold">₹ 450.00</td><td className="p-3 font-sans text-slate-500">Weekly staff allowance</td></tr>
        </tbody>
      </table>
    </div>
  </div>
);

// --- STOCK DEDUCTION (Page 20) ---
export const StockDeduction = () => (
  <div className="max-w-5xl mx-auto p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Stock Deduction (Page 20)</h2>
    <p className="text-xs text-slate-500">Excise breakage, spillage, and damage adjustments.</p>
    <div className="p-4 rounded-xl bg-slate-50 font-mono text-xs">
      Item: VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY 750ML • Deducted Qty: 2 Bottles • Reason: Unloading crate breakage
    </div>
  </div>
);

// --- OTHER MASTERS ---
const GenericModule = ({ title, fields }: { title: string; fields: string[] }) => (
  <div className="max-w-4xl mx-auto p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">{title}</h2>
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 text-xs">
      {fields.map(f => (
        <div key={f}>
          <label className="block font-semibold text-slate-600 mb-1">{f}</label>
          <input className="w-full px-3 py-2 rounded-xl border bg-slate-50" placeholder={`Enter ${f}...`} />
        </div>
      ))}
    </div>
    <div className="flex justify-end pt-4"><button className="px-5 py-2 bg-[#e8702a] text-white rounded-xl text-xs font-bold">Save Master</button></div>
  </div>
);

export const ItemGroupMaster = () => <GenericModule title="Item Group Master (Page 7)" fields={['ITEMGROUP Code', 'ITEMGROUP Name', 'Consider for Loyalty', 'Liquor ItemGroup']} />;
export const UOMMaster = () => <GenericModule title="UOM Master (Page 8)" fields={['UOM Code', 'UOM Name', 'Decimal Places']} />;
export const CustomerMaster = () => <GenericModule title="Customer Master (Page 9)" fields={['Customer Code', 'Customer Name', 'Phone Number', 'GST Number', 'Address', 'City']} />;
export const LedgerMaster = () => <GenericModule title="Ledger Master (Page 13)" fields={['Ledger Code', 'Ledger Name', 'Ledger Group', 'Opening Balance']} />;
export const LedgerGroupMaster = () => <GenericModule title="Ledger Group Master (Page 14)" fields={['Group Code', 'Group Name', 'Nature of Ledger']} />;
export const PackageMaster = () => <GenericModule title="Package Master (Page 16)" fields={['Package Code', 'Package Name', 'Ltrs In Reports']} />;
export const BrandMaster = () => <GenericModule title="Brand Master (Page 17)" fields={['Brand Code', 'Brand Name', 'Manufacturer']} />;
export const PurchaseIndent = () => <GenericModule title="Purchase Indent (Page 21)" fields={['Indent Number', 'KSBCL Depot', 'Supplier', 'Required Delivery Date']} />;
export const SalesReturn = () => <GenericModule title="Sales Return (Page 22)" fields={['Return Bill No', 'Customer Name', 'Counter', 'Reason for Return']} />;
export const SalesInvoice = () => <GenericModule title="Sales Invoice POS" fields={['Invoice No', 'Counter 1', 'Payment Mode (Cash/UPI)', 'Customer Name']} />;