import { useState } from 'react';
import { Menu, Settings, Moon, Sun } from 'lucide-react';
import CompanyMaster from './components/CompanyMaster';
import ReportsDashboard from './components/ReportsDashboard';
import Sidebar from './components/Sidebar';
import SupplierMaster from './components/SupplierMaster';
import {
  BrandMaster, CustomerMaster, Dashboard, ItemGroupMaster, ItemMaster, LedgerGroupMaster,
  LedgerMaster, PackageMaster, PurchaseBill, PurchaseIndent, SalesCounterMaster, SalesInvoice,
  StockDeduction, UOMMaster, StockBalanceReport, ExpenseEntry, SalesReturn
} from './components/OperationalModules';

export default function App() {
  const [isSidebarOpen, setSidebarOpen] = useState<boolean>(false);
  const [activeTab, setActiveTab] = useState<string>('Dashboard');
  const [darkMode, setDarkMode] = useState<boolean>(false);

  const openModule = (moduleName: string) => {
    setActiveTab(moduleName);
    if (window.innerWidth < 768) setSidebarOpen(false);
  };

  const renderContent = () => {
    switch (activeTab) {
      case 'Dashboard': return <Dashboard />;
      
      // Masters
      case 'Company Master': return <CompanyMaster />;
      case 'Item Master': return <ItemMaster />;
      case 'Item Group Master': return <ItemGroupMaster />;
      case 'UOM Master': return <UOMMaster />;
      case 'Supplier Master': return <SupplierMaster />;
      case 'Customer Master': return <CustomerMaster />;
      case 'Ledger Master': return <LedgerMaster />;
      case 'Ledger Group Master': return <LedgerGroupMaster />;
      case 'Sales Counter Master': return <SalesCounterMaster />;
      case 'Package Master': return <PackageMaster />;
      case 'Brand Master': return <BrandMaster />;
      
      // Transactions
      case 'Purchase Bill': return <PurchaseBill />;
      case 'Sales Invoice': return <SalesInvoice />;
      case 'Expense Entry': return <ExpenseEntry />;
      case 'Stock Deduction': return <StockDeduction />;
      case 'Purchase Indent': return <PurchaseIndent />;
      case 'Sales Return': return <SalesReturn />;
      
      // Reports
      case 'Stock Balance Report': return <StockBalanceReport />;
      case 'Reports Dashboard':
      case 'Customised Reports': return <ReportsDashboard />;
      
      default: 
        return (
          <div className="flex flex-col items-center justify-center h-full text-gray-400 bg-white dark:bg-slate-900 rounded-lg border border-dashed border-gray-300 dark:border-slate-800 p-8">
            <Settings size={48} className="mb-4 text-gray-300 animate-spin" />
            <h3 className="text-lg font-medium text-gray-600 dark:text-gray-300">Module "{activeTab}" is active.</h3>
            <p className="text-sm mt-2 text-slate-400">Softvent vLQMS Cloud ERP • License 827106637</p>
          </div>
        );
    }
  };

  return (
    <div className={`flex h-screen ${darkMode ? 'dark bg-slate-950 text-slate-100' : 'bg-[#f7f8f6] text-slate-800'} font-sans overflow-hidden transition-colors duration-200`}>
      <Sidebar
        isOpen={isSidebarOpen}
        activeTab={activeTab}
        onClose={() => setSidebarOpen(false)}
        onOpenModule={openModule}
      />

      {/* Main Content Area */}
      <div className="flex-1 flex flex-col min-w-0 overflow-hidden">
        {/* Top Header */}
        <header className="bg-white/95 dark:bg-slate-900/95 border-b border-slate-200 dark:border-slate-800 px-5 py-3 flex items-center justify-between shadow-sm z-20 shrink-0 backdrop-blur">
          <div className="flex items-center gap-4">
            <button className="md:hidden text-gray-600 dark:text-gray-300 p-1 hover:bg-gray-100 dark:hover:bg-slate-800 rounded" onClick={() => setSidebarOpen(true)}>
              <Menu size={22} />
            </button>
            <div>
              <h2 className="text-base sm:text-lg font-bold text-slate-900 dark:text-white leading-tight">THE LIQUOR HOUSE</h2>
              <span className="text-xs text-slate-500 font-mono">
                FY: 01/04/2026 - 31/03/2027 • CL-2 License
              </span>
            </div>
          </div>
          
          <div className="flex items-center gap-3">
            <button
              onClick={() => setDarkMode(!darkMode)}
              className="p-2 rounded-xl text-slate-500 hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
              title="Toggle Dark / Light Theme"
            >
              {darkMode ? <Sun size={18} /> : <Moon size={18} />}
            </button>

            <div className="flex items-center gap-2 text-sm text-gray-700 dark:text-gray-300 border-l pl-3 dark:border-slate-800">
              <div className="w-8 h-8 bg-slate-900 dark:bg-emerald-500 text-white dark:text-slate-950 font-bold rounded-xl flex items-center justify-center text-xs">
                AD
              </div>
              <span className="hidden sm:block font-medium text-xs">ADMIN</span>
            </div>
          </div>
        </header>

        {/* Active Tab Indicator Bar (Only Active Tab) */}
        <div className="bg-[#eef3ef]/90 dark:bg-slate-900/80 border-b border-slate-200 dark:border-slate-800 px-4 flex items-center shrink-0 h-10">
          <div className="h-8 px-4 rounded-lg text-xs font-bold flex items-center gap-2 bg-white dark:bg-slate-800 text-emerald-600 dark:text-emerald-400 shadow-sm border border-slate-200/60 dark:border-slate-700/60">
            <span className="w-2 h-2 rounded-full bg-emerald-500"></span>
            <span>{activeTab}</span>
          </div>
        </div>

        {/* Main Workspace */}
        <main className="flex-1 overflow-auto p-6 bg-[#f7f8f6] dark:bg-slate-950 transition-colors duration-200">
          {renderContent()}
        </main>

        {/* Footer / Status Bar */}
        <footer className="bg-white dark:bg-slate-900 border-t border-slate-200 dark:border-slate-800 px-4 py-2 text-[11px] text-slate-500 flex justify-between items-center shrink-0 font-mono">
          <div className="flex items-center gap-3">
            <span className="flex items-center gap-1.5 text-emerald-600 font-semibold">
              <span className="w-1.5 h-1.5 rounded-full bg-emerald-500"></span>
              Ready
            </span>
            <span className="hidden sm:inline">DB: vLQMSLHOUSE</span>
            <span className="hidden md:inline">Login: ADMIN</span>
          </div>
          <div className="flex items-center gap-4">
            <span className="font-semibold text-slate-700 dark:text-slate-300">Licensed Copy: 827106637</span>
            <span className="hidden sm:inline">Total Bills: 142</span>
            <span className="text-emerald-600 dark:text-emerald-400 font-bold">Today's Sales: ₹ 1,84,650.00</span>
          </div>
        </footer>
      </div>
    </div>
  );
}
