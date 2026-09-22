import { useState } from 'react';
import { Truck, Save, CheckCircle } from 'lucide-react';

const SupplierMaster = () => {
  const [showToast, setShowToast] = useState(false);

  const handleSave = () => {
    setShowToast(true);
    setTimeout(() => setShowToast(false), 2500);
  };

  return (
    <div className="max-w-5xl mx-auto p-6 rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm space-y-6">
      {showToast && (
        <div className="fixed top-6 right-6 z-50 bg-emerald-600 text-white px-4 py-3 rounded-xl shadow-2xl flex items-center gap-2 text-sm font-medium animate-bounce">
          <CheckCircle size={18} /> Supplier record saved successfully!
        </div>
      )}

      <div className="flex justify-between items-center border-b pb-4 border-slate-100 dark:border-slate-800">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-blue-500/10 text-blue-600 dark:text-blue-400 rounded-xl">
            <Truck size={24} />
          </div>
          <div>
            <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Supplier Master</h2>
            <p className="text-xs text-slate-500">Distributor accounts, excise warehouse mappings & GST details.</p>
          </div>
        </div>
        <button onClick={handleSave} className="px-4 py-2 bg-emerald-600 text-white rounded-xl text-xs font-semibold flex items-center gap-1.5 shadow-sm">
          <Save size={14} /> Save Supplier
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Supp Code</label>
              <input type="text" defaultValue="R&D DISTRIBUTORS" className="w-full px-3 py-2 text-xs font-mono font-bold rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Supp Name</label>
              <input type="text" defaultValue="R&D DISTRIBUTORS" className="w-full px-3 py-2 text-sm font-semibold rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800" />
            </div>
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Tally Alias</label>
            <input type="text" defaultValue="R&D DISTRIBUTOR" className="w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800" />
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1">Billing & Depot Address</label>
            <textarea rows={3} defaultValue="460, 1ST MAIN ROAD, ALLASANDRA, YELAHANKA" className="w-full px-3 py-2 text-xs rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800" />
          </div>
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
            <div><label className="block text-xs text-slate-500 mb-1">City</label><input type="text" defaultValue="Bangalore" className="w-full px-2 py-1.5 text-xs rounded-lg border border-slate-200 dark:border-slate-700" /></div>
            <div><label className="block text-xs text-slate-500 mb-1">Pincode</label><input type="text" defaultValue="560064" className="w-full px-2 py-1.5 text-xs rounded-lg border border-slate-200 dark:border-slate-700" /></div>
            <div><label className="block text-xs text-slate-500 mb-1">State</label><input type="text" defaultValue="Karnataka" className="w-full px-2 py-1.5 text-xs rounded-lg border border-slate-200 dark:border-slate-700" /></div>
            <div><label className="block text-xs text-slate-500 mb-1">Country</label><input type="text" defaultValue="INDIA" className="w-full px-2 py-1.5 text-xs rounded-lg border border-slate-200 dark:border-slate-700" /></div>
          </div>
        </div>

        <div className="p-4 rounded-xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200 dark:border-slate-700 space-y-3 text-xs">
          <h4 className="font-bold text-slate-700 dark:text-slate-300 uppercase tracking-wider text-[11px] border-b pb-1 border-slate-200 dark:border-slate-700">Header Info</h4>
          <div>
            <label className="block text-slate-500 mb-1">Phone No</label>
            <input type="text" defaultValue="9900196257" className="w-full px-2 py-1 rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 font-mono" />
          </div>
          <div>
            <label className="block text-slate-500 mb-1">GSTNO</label>
            <input type="text" defaultValue="29AABCR1234F1Z1" className="w-full px-2 py-1 rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 font-mono font-bold text-emerald-600" />
          </div>
          <div>
            <label className="block text-slate-500 mb-1">Currency [Mst]</label>
            <input type="text" defaultValue="Rupees" className="w-full px-2 py-1 rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800" />
          </div>
        </div>
      </div>
    </div>
  );
};

export default SupplierMaster;