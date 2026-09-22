import { useState, useMemo } from 'react';
import { 
  Package, Receipt, Truck, TrendingUp, Trash2,
  ChevronLeft, ChevronRight, BarChart3
} from 'lucide-react';
import { numberToWords } from './formatters';
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
      <div className="p-6 rounded-2xl bg-linear-to-r from-slate-900 via-slate-800 to-emerald-950 text-white border border-slate-700/50 shadow-xl flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
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
            <div key={i} className="p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
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
    </div>
  );
};

// --- ITEM MASTER ---
const ItemField = ({ label, value, readOnly = false, type = 'text', className = '' }: {
  label: string;
  value: string | number;
  readOnly?: boolean;
  type?: string;
  className?: string;
}) => (
  <div>
    <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">{label}</label>
    <input
      type={type}
      defaultValue={value}
      readOnly={readOnly}
      className={`w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 ${className}`}
    />
  </div>
);

export const ItemMaster = () => {
  const [idx, setIdx] = useState(0);
  const current = SAMPLE_STOCK_DATA[idx];

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <div>
          <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Item Master</h2>
          <p className="text-xs text-slate-500">Liquor catalogue, KSBCL barcode mappings, MRP & excise taxes.</p>
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
        <div className="lg:col-span-2 p-6 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-4">
          <ItemField label="Item Code / Description" value={current.name} readOnly className="font-mono font-medium bg-slate-50 dark:bg-slate-800" />
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
            <ItemField label="UOM" value="Bottle" />
            <ItemField label="HSN Code" value="22083011" className="font-mono" />
            <ItemField label="Item Group" value={current.group} />
            <ItemField label="Category" value={current.cat} className="font-bold text-emerald-600" />
          </div>
          <ItemField label="Bottles per Case" value="12" type="number" className="w-32" />
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
            <div className="p-2 font-mono font-bold bg-white dark:bg-slate-800 rounded-lg border">{current.ksbcl}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

// --- PURCHASE BILL ---
export const PurchaseBill = () => {
  const [items, setItems] = useState([
    { id: 1, code: 'VIN-750', name: 'VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY 750ML', qoh: 48, qty: 24, rate: 759.29 },
    { id: 2, code: 'KF-650', name: 'KINGFISHER PREMIUM LAGER BEER 650ML', qoh: 144, qty: 48, rate: 155.00 },
  ]);

  const basicTotal = useMemo(() => items.reduce((acc, it) => acc + (it.qty * it.rate), 0), [items]);

  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex justify-between items-center">
        <div>
          <div className="flex items-center gap-2">
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">New - Purchase Bill</h2>
            <span className="px-2 py-0.5 text-xs font-mono font-bold bg-purple-100 text-purple-700 rounded-md">PB No: 201</span>
          </div>
          <p className="text-xs text-slate-500">Consignment procurement from R&D Distributors / KSBCL.</p>
        </div>
        <div className="text-xs font-mono">Date: <span className="font-bold">21/09/2026</span></div>
      </div>

      <div className="overflow-x-auto rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <table className="w-full text-xs text-left">
          <thead className="bg-slate-50 dark:bg-slate-800 text-slate-500 uppercase">
            <tr>
              <th className="px-4 py-3">Sl</th>
              <th className="px-4 py-3">Item Code</th>
              <th className="px-4 py-3">Item Name</th>
              <th className="px-4 py-3 text-right">QOH</th>
              <th className="px-4 py-3 text-right">PB Qty</th>
              <th className="px-4 py-3 text-right">Rate (₹)</th>
              <th className="px-4 py-3 text-right">Amount (₹)</th>
              <th className="px-4 py-3 text-center">Action</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 dark:divide-slate-800 font-mono">
            {items.map((row, i) => (
              <tr key={row.id}>
                <td className="px-4 py-3 font-sans text-slate-400">{i + 1}</td>
                <td className="px-4 py-3 font-medium">{row.code}</td>
                <td className="px-4 py-3 font-sans font-medium text-slate-900 dark:text-white">{row.name}</td>
                <td className="px-4 py-3 text-right text-slate-500">{row.qoh}</td>
                <td className="px-4 py-3 text-right font-bold">{row.qty}</td>
                <td className="px-4 py-3 text-right">₹ {row.rate.toFixed(2)}</td>
                <td className="px-4 py-3 text-right font-bold text-slate-900 dark:text-white">₹ {(row.qty * row.rate).toFixed(2)}</td>
                <td className="px-4 py-3 text-center">
                  <button onClick={() => setItems(items.filter(it => it.id !== row.id))} className="text-slate-400 hover:text-red-500">
                    <Trash2 size={14} />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 items-center">
        <div className="p-4 rounded-xl bg-slate-50 dark:bg-slate-800 text-xs font-semibold text-emerald-700 dark:text-emerald-400 font-mono">
          {numberToWords(basicTotal)}
        </div>
        <div className="p-4 rounded-xl bg-white dark:bg-slate-900 border text-right">
          <span className="text-xs text-slate-500 block">Total Net Amount</span>
          <span className="text-xl font-bold font-mono text-emerald-600">₹ {basicTotal.toFixed(2)}</span>
        </div>
      </div>
    </div>
  );
};

// --- STOCK BALANCE REPORT ---
export const StockBalanceReport = () => {
  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="p-5 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm flex justify-between items-center">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-amber-500/10 text-amber-600 rounded-xl"><BarChart3 size={24} /></div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Stock Balance Report</h2>
            <p className="text-xs text-slate-500">Excise opening, receipt, issued & closing ledger balances.</p>
          </div>
        </div>
      </div>

      <div className="overflow-x-auto rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
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

// --- EXPENSE ENTRY ---
export const ExpenseEntry = () => (
  <div className="max-w-5xl mx-auto p-6 rounded-2xl bg-white dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Expense Entry</h2>
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

// --- STOCK DEDUCTION ---
export const StockDeduction = () => (
  <div className="max-w-5xl mx-auto p-6 rounded-2xl bg-white dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Stock Deduction</h2>
    <p className="text-xs text-slate-500">Excise breakage, spillage, and damage adjustments.</p>
    <div className="p-4 rounded-xl bg-slate-50 font-mono text-xs">
      Item: VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY 750ML • Deducted Qty: 2 Bottles • Reason: Unloading crate breakage
    </div>
  </div>
);

// --- OTHER MASTERS ---
const GenericModule = ({ title, fields }: { title: string; fields: string[] }) => (
  <div className="max-w-4xl mx-auto p-6 rounded-2xl bg-white dark:bg-slate-900 border shadow-sm space-y-4">
    <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">{title}</h2>
    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 text-xs">
      {fields.map(f => (
        <div key={f}>
          <label className="block font-semibold text-slate-600 mb-1">{f}</label>
          <input className="w-full px-3 py-2 rounded-xl border bg-slate-50" placeholder={`Enter ${f}...`} />
        </div>
      ))}
    </div>
    <div className="flex justify-end pt-4"><button className="px-5 py-2 bg-emerald-600 text-white rounded-xl text-xs font-bold">Save Master</button></div>
  </div>
);

export const ItemGroupMaster = () => <GenericModule title="Item Group Master" fields={['ITEMGROUP Code', 'ITEMGROUP Name', 'Consider for Loyalty', 'Liquor ItemGroup']} />;
export const UOMMaster = () => <GenericModule title="UOM Master" fields={['UOM Code', 'UOM Name', 'Decimal Places']} />;
export const CustomerMaster = () => <GenericModule title="Customer Master" fields={['Customer Code', 'Customer Name', 'Phone Number', 'GST Number', 'Address', 'City']} />;
export const LedgerMaster = () => <GenericModule title="Ledger Master" fields={['Ledger Code', 'Ledger Name', 'Ledger Group', 'Opening Balance']} />;
export const LedgerGroupMaster = () => <GenericModule title="Ledger Group Master" fields={['Group Code', 'Group Name', 'Nature of Ledger']} />;
export const SalesCounterMaster = () => <GenericModule title="Sales Counter Master" fields={['Counter Code', 'Counter Name', 'System Name', 'Prefix']} />;
export const PackageMaster = () => <GenericModule title="Package Master" fields={['Package Code', 'Package Name', 'Ltrs In Reports']} />;
export const BrandMaster = () => <GenericModule title="Brand Master" fields={['Brand Code', 'Brand Name', 'Manufacturer']} />;
export const PurchaseIndent = () => <GenericModule title="Purchase Indent" fields={['Indent Number', 'KSBCL Depot', 'Supplier', 'Required Delivery Date']} />;
export const SalesReturn = () => <GenericModule title="Sales Return" fields={['Return Bill No', 'Customer Name', 'Counter', 'Reason for Return']} />;
export const SalesInvoice = () => <GenericModule title="Sales Invoice POS" fields={['Invoice No', 'Counter 1', 'Payment Mode (Cash/UPI)', 'Customer Name']} />;