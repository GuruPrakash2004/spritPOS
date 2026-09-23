import { useState } from 'react';
import { 
  Building2, ShieldCheck, Calendar, Image as ImageIcon, 
  FolderArchive, Save, Upload, Eye, Download, Plus 
} from 'lucide-react';
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

const CompanyMaster = () => {
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

        {/* Tab 4: Images */}
        {tab === 'Images' && (
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <h4 className="text-sm font-semibold text-slate-800 dark:text-slate-200">Store Showcase & Beverage Visuals</h4>
              <button onClick={triggerToast} className="px-3 py-1.5 text-xs font-medium bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 rounded-lg text-slate-700 dark:text-slate-300 flex items-center gap-1.5">
                <Upload size={14} /> Change Brand Display
              </button>
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
              {[
                { title: 'vLQMS 2020 Display', subtitle: 'Main POS Brand Wall', bg: 'from-amber-600 to-amber-900' },
                { title: 'Single Malt Gallery', subtitle: 'Glenfiddich, Amrut, Macallan', bg: 'from-emerald-700 to-slate-900' },
                { title: 'Craft Beer Section', subtitle: 'Kingfisher, Bira, Corona', bg: 'from-blue-600 to-indigo-950' }
              ].map((card, idx) => (
                <div key={idx} className={`h-48 rounded-xl bg-linear-to-br ${card.bg} p-5 text-white flex flex-col justify-between shadow-lg relative overflow-hidden group`}>
                  <div className="relative z-10">
                    <span className="px-2 py-0.5 text-[10px] font-mono bg-white/20 rounded-md">Banner #{idx + 1}</span>
                    <h5 className="font-heading font-bold text-lg mt-2">{card.title}</h5>
                    <p className="text-xs text-white/80">{card.subtitle}</p>
                  </div>
                  <div className="relative z-10 flex items-center justify-between text-xs text-white/90">
                    <span>Active on POS Screen</span>
                    <Eye size={14} />
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Tab 5: Documents */}
        {tab === 'Documents' && (
          <div className="space-y-4">
            <div className="overflow-x-auto rounded-xl border border-slate-200 dark:border-slate-800">
              <table className="w-full text-xs text-left">
                <thead className="bg-slate-50 dark:bg-slate-800/50 text-slate-500 uppercase tracking-wider">
                  <tr>
                    <th className="px-4 py-3">Group</th>
                    <th className="px-4 py-3">File Type</th>
                    <th className="px-4 py-3">File Name</th>
                    <th className="px-4 py-3">Remarks</th>
                    <th className="px-4 py-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
                  {[
                    { group: 'Excise', type: 'PDF Document', name: 'Karnataka_Excise_CL2_2026_27.pdf', remarks: 'Annual License Renewal Certificate' },
                    { group: 'Taxation', type: 'PDF Document', name: 'GST_Reg_Certificate_29AEGFP9916P1ZD.pdf', remarks: 'Department of Commercial Taxes' },
                    { group: 'KSBCL', type: 'Excel Spreadsheet', name: 'KSBCL_Depot12_Indent_Permit.xlsx', remarks: 'Authorized Indent Quota Card' },
                  ].map((doc, idx) => (
                    <tr key={idx} className="hover:bg-slate-50 dark:hover:bg-slate-800/40">
                      <td className="px-4 py-3 font-semibold text-slate-700 dark:text-slate-300">{doc.group}</td>
                      <td className="px-4 py-3 text-slate-500">{doc.type}</td>
                      <td className="px-4 py-3 font-mono text-emerald-600 dark:text-emerald-400 font-medium cursor-pointer hover:underline">{doc.name}</td>
                      <td className="px-4 py-3 text-slate-600 dark:text-slate-400">{doc.remarks}</td>
                      <td className="px-4 py-3 text-right">
                        <button onClick={triggerToast} className="text-slate-500 hover:text-emerald-600 p-1">
                          <Download size={14} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="flex justify-end gap-2 pt-2">
              <button onClick={triggerToast} className="px-3 py-1.5 text-xs font-medium bg-slate-100 dark:bg-slate-800 rounded-lg text-slate-700 dark:text-slate-300">Add Document</button>
              <button onClick={triggerToast} className="px-3 py-1.5 text-xs font-medium bg-[#e8702a] text-white rounded-lg">Export To Excel</button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default CompanyMaster;