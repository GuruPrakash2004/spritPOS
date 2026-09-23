import { useEffect, useState } from 'react';
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
  const [isSidebarOpen, setSidebarOpen] = useState(false);
  const [activeTab, setActiveTab] = useState('Dashboard');
  const [darkMode, setDarkMode] = useState(false);

  useEffect(() => {
    const handlePointerMove = (event: PointerEvent) => {
      document.documentElement.style.setProperty('--cursor-x', `${event.clientX}px`);
      document.documentElement.style.setProperty('--cursor-y', `${event.clientY}px`);
    };

    window.addEventListener('pointermove', handlePointerMove);
    return () => window.removeEventListener('pointermove', handlePointerMove);
  }, []);

  const openModule = (moduleName: string) => {
    setActiveTab(moduleName);
    if (window.innerWidth < 768) setSidebarOpen(false);
  };

  const renderContent = () => {
    switch (activeTab) {
      case 'Dashboard': return <Dashboard />;
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
      case 'Purchase Bill': return <PurchaseBill />;
      case 'Sales Invoice': return <SalesInvoice />;
      case 'Expense Entry': return <ExpenseEntry />;
      case 'Stock Deduction': return <StockDeduction />;
      case 'Purchase Indent': return <PurchaseIndent />;
      case 'Sales Return': return <SalesReturn />;
      case 'Stock Balance Report': return <StockBalanceReport />;
      case 'Reports Dashboard':
      case 'Customised Reports': return <ReportsDashboard />;
      default: return (
        <div className="flex h-full flex-col items-center justify-center rounded-lg border border-dashed border-stone-300 bg-white p-8 text-stone-400 dark:border-stone-700 dark:bg-stone-900">
          <Settings size={48} className="mb-4 text-stone-300 dark:text-stone-600" />
          <h3 className="text-lg font-medium text-stone-600 dark:text-stone-300">Module "{activeTab}" is active.</h3>
          <p className="mt-2 text-sm text-stone-400">Softvent vLQMS Cloud ERP • License 827106637</p>
        </div>
      );
    }
  };

  return (
    <div className={`app-shell flex h-screen overflow-hidden font-sans transition-colors duration-300 ${darkMode ? 'dark bg-[#0d0e0c] text-stone-100' : 'bg-[#f4f1eb] text-stone-800'}`}>
      <Sidebar isOpen={isSidebarOpen} activeTab={activeTab} onClose={() => setSidebarOpen(false)} onOpenModule={openModule} />

      <div className="flex min-w-0 flex-1 flex-col overflow-hidden">
        <header className="z-20 flex shrink-0 items-center justify-between border-b border-[#ded8cd] bg-[#fbfaf7]/95 px-5 py-3 shadow-sm backdrop-blur dark:border-[#2b2c27] dark:bg-[#171815]/95">
          <div className="flex items-center gap-4">
            <button className="rounded-lg p-1 text-stone-600 hover:bg-stone-100 dark:text-stone-300 dark:hover:bg-stone-800 md:hidden" onClick={() => setSidebarOpen(true)} aria-label="Open navigation">
              <Menu size={22} />
            </button>
            <div>
              <h2 className="text-base font-bold leading-tight text-stone-900 dark:text-stone-100 sm:text-lg">THE LIQUOR HOUSE</h2>
              <span className="font-mono text-xs text-stone-500">FY: 01/04/2026 - 31/03/2027 • CL-2 License</span>
            </div>
          </div>
          <div className="flex items-center gap-3">
            <button onClick={() => setDarkMode(!darkMode)} className="rounded-xl p-2 text-stone-500 transition-colors hover:bg-stone-100 dark:hover:bg-stone-800" title="Toggle Dark / Light Theme" aria-label="Toggle Dark / Light Theme">
              {darkMode ? <Sun size={18} /> : <Moon size={18} />}
            </button>
            <div className="flex items-center gap-2 border-l border-[#ded8cd] pl-3 text-sm text-stone-700 dark:border-[#2b2c27] dark:text-stone-300">
              <div className="flex h-8 w-8 items-center justify-center rounded-xl bg-[#e8702a] text-xs font-bold text-white">AD</div>
              <span className="hidden text-xs font-medium sm:block">ADMIN</span>
            </div>
          </div>
        </header>

        <div className="flex h-10 shrink-0 items-center border-b border-[#ded8cd] bg-[#ebe8df]/90 px-4 dark:border-[#2b2c27] dark:bg-[#171815]/80">
          <div className="flex h-8 items-center gap-2 rounded-lg border border-[#ded8cd] bg-[#fbfaf7] px-4 text-xs font-bold text-[#c55419] shadow-sm dark:border-[#3a3b34] dark:bg-[#24251f] dark:text-[#f28a4e]">
            <span className="h-2 w-2 rounded-full bg-[#e8702a]" />
            <span>{activeTab}</span>
          </div>
        </div>

        <main className="workspace-backdrop flex-1 overflow-auto bg-[#f4f1eb] p-6 transition-colors duration-300 dark:bg-[#0d0e0c]">
          {renderContent()}
        </main>

        <footer className="flex shrink-0 items-center justify-between border-t border-[#ded8cd] bg-[#fbfaf7] px-4 py-2 font-mono text-[11px] text-stone-500 dark:border-[#2b2c27] dark:bg-[#171815]">
          <div className="flex items-center gap-3">
            <span className="flex items-center gap-1.5 font-semibold text-[#c55419]"><span className="h-1.5 w-1.5 rounded-full bg-[#e8702a]" />Ready</span>
            <span className="hidden sm:inline">DB: vLQMSLHOUSE</span>
            <span className="hidden md:inline">Login: ADMIN</span>
          </div>
          <div className="flex items-center gap-4">
            <span className="hidden font-semibold text-stone-700 dark:text-stone-300 sm:inline">Licensed Copy: 827106637</span>
            <span className="hidden md:inline">Total Bills: 142</span>
            <span className="font-bold text-[#c55419] dark:text-[#f28a4e]">Today's Sales: ₹ 1,84,650.00</span>
          </div>
        </footer>
      </div>
    </div>
  );
}
