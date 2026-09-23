import { useState } from 'react';
import { 
  Building2, ShieldCheck, Calendar, Image as ImageIcon, 
  FolderArchive, Save, Plus 
} from 'lucide-react';
import { CompanyDocuments, CompanyImages } from './CompanyFiles';
import Toast from './Toast';

interface FinYear {
  from: string;
  to: string;
  active: boolean;
}

const INITIAL_FIN_YEARS: FinYear[] = [
  { from: '01/04/2021', to: '31/03/2022', active: false },
  { from: '01/04/2022', to: '31/03/2023', active: false },
  { from: '01/04/2023', to: '31/03/2024', active: false },
  { from: '01/04/2024', to: '31/03/2025', active: false },
  { from: '01/04/2025', to: '31/03/2026', active: false },
  { from: '01/04/2026', to: '31/03/2027', active: true },
];

// canEditFiles: only an Administrator may add, edit or delete company images and documents.
const CompanyMaster = ({ canEditFiles }: { canEditFiles: boolean }) => {
  const [tab, setTab] = useState<string>('Company Info');
  const [finYears, setFinYears] = useState<FinYear[]>(INITIAL_FIN_YEARS);
  const [showToast, setShowToast] = useState<boolean>(false);

  const triggerToast = () => {
    setShowToast(true);
    setTimeout(() => setShowToast(false), 2500);
  };

  const handleSetActiveFY = (index: number) => {
    setFinYears(finYears.map((fy, i) => ({ ...fy, active: i === index })));
    triggerToast();
  };

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      {/* Toast Notification */}
      {showToast && <Toast text="Company credentials updated successfully!" />}

      {/* Top Header Card */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 p-5 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 rounded-xl">
            <Building2 size={24} />
          </div>
          <div>
            <div className="flex items-center gap-2">
              <h2 className="text-xl font-bold font-heading text-slate-900 dark:text-white">Company Master</h2>
              <span className="px-2 py-0.5 text-xs font-semibold bg-emerald-100 text-emerald-700 dark:bg-emerald-900/40 dark:text-emerald-400 rounded-md">
                Licensed Entity (CL-2)
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-0.5">Maintain firm credentials, excise licenses, retailer IDs, and financial periods.</p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={triggerToast} className="px-4 py-2 bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 dark:hover:bg-slate-700 text-slate-700 dark:text-slate-300 text-xs font-medium rounded-xl transition-all">
            Backup Database
          </button>
          <button onClick={triggerToast} className="px-4 py-2 bg-[#e8702a] hover:bg-[#b84716] text-white text-xs font-semibold rounded-xl shadow-sm transition-all flex items-center gap-1.5">
            <Save size={14} /> Save Master
          </button>
        </div>
      </div>

      {/* Tab Bar */}
      <div className="flex gap-1 border-b border-slate-200 dark:border-slate-800 overflow-x-auto">
        {[
          { id: 'Company Info', icon: Building2 },
          { id: 'Additional Fields', icon: ShieldCheck },
          { id: 'Financial Year(s)', icon: Calendar },
          { id: 'Images', icon: ImageIcon },
          { id: 'Documents', icon: FolderArchive }
        ].map((t) => {
          const IconComp = t.icon;
          return (
            <button
              key={t.id}
              onClick={() => setTab(t.id)}
              className={`px-4 py-2.5 text-xs font-semibold rounded-t-xl transition-all flex items-center gap-2 whitespace-nowrap ${
                tab === t.id 
                  ? 'bg-[#fffaf5] dark:bg-slate-900 border-t-2 border-x border-slate-200 dark:border-slate-800 border-t-emerald-500 text-emerald-600 dark:text-emerald-400 shadow-sm' 
                  : 'text-slate-500 hover:text-slate-800 dark:hover:text-slate-200'
              }`}
            >
              <IconComp size={14} />
              <span>{t.id}</span>
            </button>
          );
        })}
      </div>

      {/* Tab Panels */}
      <div className="p-6 rounded-2xl bg-[#fffaf5] dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm">
        {/* Tab 1: Company Info */}
        {tab === 'Company Info' && (
          <div className="space-y-5">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              <div>
                <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Company Name (License Holder)</label>
                <input type="text" defaultValue="THE LIQUOR HOUSE" className="w-full px-3.5 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 focus:ring-2 focus:ring-emerald-500 outline-none" />
              </div>
              <div>
                <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Email Address</label>
                <input type="email" defaultValue="theliquorhouse28@gmail.com" className="w-full px-3.5 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 focus:ring-2 focus:ring-emerald-500 outline-none" />
              </div>
              <div className="md:col-span-2">
                <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Registered Premises Address</label>
                <textarea rows={3} defaultValue="Byatarayanapura, Bangalore-92, Karnataka" className="w-full px-3.5 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 focus:ring-2 focus:ring-emerald-500 outline-none" />
              </div>
            </div>
          </div>
        )}

        {/* Tab 2: Additional Fields */}
        {tab === 'Additional Fields' && (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Tel No</label>
              <input type="text" defaultValue="080-26793693" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Fax No</label>
              <input type="text" placeholder="Optional Fax" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Mobile No</label>
              <input type="text" defaultValue="9480697887" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Website</label>
              <input type="text" defaultValue="https://theliquorhouse.in" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">GST No (GSTIN)</label>
              <input type="text" defaultValue="29AEGFP9916P1ZD" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono font-bold text-emerald-600 dark:text-emerald-400" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">PAN No</label>
              <input type="text" defaultValue="AEGFP9916P" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">CIN No</label>
              <input type="text" defaultValue="U51909KA2020PTC139882" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">Excise Ph No</label>
              <input type="text" defaultValue="9900196276" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono" />
            </div>
            <div>
              <label className="block text-xs font-semibold text-slate-600 dark:text-slate-300 mb-1.5">KSBCL Retailer Code</label>
              <input type="text" defaultValue="41818" className="w-full px-3 py-2 text-sm rounded-xl border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 font-mono font-bold text-amber-600 dark:text-amber-400" />
            </div>
          </div>
        )}

        {/* Tab 3: Financial Years */}
        {tab === 'Financial Year(s)' && (
          <div className="space-y-4">
            <div className="flex items-center justify-between mb-2">
              <span className="text-xs text-slate-500">Configure financial periods. Current active period determines all billing ledgers.</span>
              <button onClick={triggerToast} className="px-3 py-1.5 text-xs font-semibold bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 rounded-lg hover:bg-emerald-500/20 flex items-center gap-1">
                <Plus size={12} /> Create Financial Year
              </button>
            </div>
            <div className="overflow-x-auto rounded-xl border border-slate-200 dark:border-slate-800">
              <table className="w-full text-sm text-left">
                <thead className="bg-slate-50 dark:bg-slate-800/60 text-xs uppercase text-slate-500 tracking-wider">
                  <tr>
                    <th className="px-4 py-3">From Date</th>
                    <th className="px-4 py-3">To Date</th>
                    <th className="px-4 py-3 text-center">Status</th>
                    <th className="px-4 py-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
                  {finYears.map((fy, i) => (
                    <tr key={i} className={`hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors ${fy.active ? 'bg-emerald-50/50 dark:bg-emerald-950/20' : ''}`}>
                      <td className="px-4 py-3 font-mono">{fy.from}</td>
                      <td className="px-4 py-3 font-mono">{fy.to}</td>
                      <td className="px-4 py-3 text-center">
                        {fy.active ? (
                          <span className="inline-flex items-center gap-1 px-2.5 py-0.5 text-xs font-bold rounded-full bg-emerald-100 text-emerald-800 dark:bg-emerald-900/50 dark:text-emerald-300">
                            <span className="w-1.5 h-1.5 rounded-full bg-emerald-500"></span> Active (Y)
                          </span>
                        ) : (
                          <span className="text-xs text-slate-400 font-medium">Inactive (N)</span>
                        )}
                      </td>
                      <td className="px-4 py-3 text-right">
                        {!fy.active ? (
                          <button
                            onClick={() => handleSetActiveFY(i)}
                            className="px-3 py-1 text-xs font-medium text-emerald-600 dark:text-emerald-400 hover:bg-emerald-50 dark:hover:bg-emerald-950/40 rounded-lg transition-colors"
                          >
                            Set Active
                          </button>
                        ) : (
                          <span className="text-xs text-slate-400 font-semibold">Current</span>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* Tabs 4-5: files the administrator uploads (stored by the backend) */}
        {tab === 'Images' && <CompanyImages canEdit={canEditFiles} />}
        {tab === 'Documents' && <CompanyDocuments canEdit={canEditFiles} />}
      </div>
    </div>
  );
};

export default CompanyMaster;