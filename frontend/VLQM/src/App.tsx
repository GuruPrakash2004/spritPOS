import { useEffect, useState } from 'react';
import { Menu, Settings, Moon, Sun } from 'lucide-react';
import CompanyMaster from './components/CompanyMaster';
import PurchaseBill from './components/PurchaseBill';
import ReportsDashboard from './components/ReportsDashboard';
import SalesCounterMaster from './components/SalesCounterMaster';
import { DEFAULT_COUNTER, FIRST_BILL_SEQ, billNumber, type InvoiceSetup, type SalesCounter } from './components/salesCounters';
import SalesInvoice from './components/SalesInvoice';
import SalesInvoiceSetup from './components/SalesInvoiceSetup';
import Sidebar from './components/Sidebar';
import SupplierMaster from './components/SupplierMaster';
import {
  BrandMaster, CustomerMaster, Dashboard, ItemGroupMaster, ItemMaster, LedgerGroupMaster,
  LedgerMaster, PackageMaster, PurchaseIndent,
  StockDeduction, UOMMaster, StockBalanceReport, ExpenseEntry, SalesReturn
} from './components/OperationalModules';

export default function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);
  const [activeTab, setActiveTab] = useState('Dashboard');
  const [darkMode, setDarkMode] = useState(false);
  const [invoiceDraft, setInvoiceDraft] = useState<InvoiceSetup>({
    counter: DEFAULT_COUNTER, invoiceNo: billNumber(DEFAULT_COUNTER.prefix, FIRST_BILL_SEQ), paymentType: 'CASH', customer: '',
  });
  const [billOpen, setBillOpen] = useState(false);

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
    setBillOpen(false);
    if (window.innerWidth < 768) setSidebarOpen(false);
  };

  // Sales Invoice is two steps: the setup form collects counter + invoice details, then the bill screen opens.
  const openInvoiceSetup = (counter: SalesCounter) => {
    setInvoiceDraft(draft => ({ ...draft, counter, invoiceNo: billNumber(counter.prefix, FIRST_BILL_SEQ) }));
    setBillOpen(false);
    setActiveTab('Sales Invoice');
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
      case 'Sales Counter Master': return <SalesCounterMaster onSave={openInvoiceSetup} />;
      case 'Package Master': return <PackageMaster />;
      case 'Brand Master': return <BrandMaster />;
      case 'Purchase Bill': return <PurchaseBill />;
      case 'Sales Invoice': return billOpen
        ? <SalesInvoice setup={invoiceDraft} onClose={() => setBillOpen(false)} />
        : <SalesInvoiceSetup initial={invoiceDraft} onSubmit={setup => { setInvoiceDraft(setup); setBillOpen(true); }} />;
      case 'Expense Entry': return <ExpenseEntry />;
      case 'Stock Deduction': return <StockDeduction />;
      case 'Purchase Indent': return <PurchaseIndent />;
      case 'Sales Return': return <SalesReturn />;
      case 'Stock Balance Report': return <StockBalanceReport />;
      case 'Reports Dashboard':
      case 'Customised Reports': return <ReportsDashboard />;
      default: return (
        <div className="flex h-full flex-col items-center justify-center rounded-lg border border-dashed border-stone-300 bg-[#fffaf5] p-8 text-stone-400 dark:border-stone-700 dark:bg-stone-900">
          <Settings size={48} className="mb-4 text-stone-300 dark:text-stone-600" />
          <h3 className="text-lg font-medium text-stone-600 dark:text-stone-300">Module "{activeTab}" is active.</h3>
          <p className="mt-2 text-sm text-stone-400">Softvent vLQMS Cloud ERP • License 827106637</p>
        </div>
      );
    }
  };

  return (
    <div className={`app-shell flex h-screen overflow-hidden font-sans transition-colors duration-300 ${darkMode ? 'dark bg-[#0e101b] text-stone-100' : 'bg-[#f6efe8] text-stone-800'}`}>
      <Sidebar isOpen={isSidebarOpen} activeTab={activeTab} onClose={() => setSidebarOpen(false)} onOpenModule={openModule} />

      <div className="flex min-w-0 flex-1 flex-col overflow-hidden">
        <header className="z-20 flex shrink-0 items-center justify-between border-b border-[#e1c9b6] bg-[#fffaf5]/90 h-[70px] px-6 py-[13px] shadow-sm backdrop-blur dark:border-[#343b59] dark:bg-[#171b2b]/90">
          <div className="flex items-center gap-4">
            <button className="rounded-lg p-1 text-stone-600 hover:bg-stone-100 dark:text-stone-300 dark:hover:bg-stone-800 md:hidden" onClick={() => setSidebarOpen(true)} aria-label="Open navigation">
              <Menu size={22} />
            </button>
            <div>
              <h2 className="text-base font-extrabold leading-tight tracking-[0.05em] text-stone-900 dark:text-stone-100 sm:text-[19px]">THE LIQUOR HOUSE</h2>
              <span className="font-mono text-xs text-stone-500">FY: 01/04/2026 - 31/03/2027 • CL-2 License</span>
            </div>
          </div>
          <div className="flex items-center gap-3">
            <button onClick={() => setDarkMode(!darkMode)} className="rounded-xl p-2 text-stone-500 transition-colors hover:bg-stone-100 dark:hover:bg-stone-800" title="Toggle Dark / Light Theme" aria-label="Toggle Dark / Light Theme">
              {darkMode ? <Sun size={18} /> : <Moon size={18} />}
            </button>
            <div className="flex items-center gap-2 border-l border-[#e1c9b6] pl-3 text-sm text-stone-700 dark:border-[#343b59] dark:text-stone-300">
              <div className="flex h-8 w-8 items-center justify-center rounded-full bg-linear-135 from-[#e8702a] to-[#e75b6f] text-xs font-bold text-white">AD</div>
              <span className="hidden text-xs font-medium sm:block">ADMIN</span>
            </div>
          </div>
        </header>

        <div className="flex h-[46px] shrink-0 items-center border-b border-[#e1c9b6] bg-[#f1e3d8]/80 px-5 dark:border-[#343b59] dark:bg-[#20253a]/80">
          <div className="flex h-8 items-center gap-2 rounded-full border border-[#e1c9b6] bg-[#fffaf5] px-[18px] text-xs font-bold text-[#b84716] shadow-[0_5px_16px_rgba(112,58,28,0.12)] dark:border-[#343b59] dark:bg-[#171b2b] dark:text-[#ffb07e]">
            <span className="h-2 w-2 rounded-full bg-[#e8702a]" />
            <span>{activeTab}</span>
          </div>
        </div>

        <main className="workspace-backdrop flex-1 overflow-auto bg-[#f6efe8] p-6 transition-colors duration-300 dark:bg-[#0e101b]">
          {renderContent()}
        </main>

        <footer className="flex h-10 shrink-0 items-center justify-between border-t border-[#e1c9b6] bg-[#fffaf5]/90 px-5 backdrop-blur font-mono text-[11px] text-stone-500 dark:border-[#343b59] dark:bg-[#171b2b]/90">
          <div className="flex items-center gap-3">
            <span className="flex items-center gap-1.5 font-semibold text-[#b84716]"><span className="h-1.5 w-1.5 rounded-full bg-[#e8702a]" />Ready</span>
            <span className="hidden sm:inline">DB: vLQMSLHOUSE</span>
            <span className="hidden md:inline">Login: ADMIN</span>
          </div>
          <div className="flex items-center gap-4">
            <span className="hidden font-semibold text-stone-700 dark:text-stone-300 sm:inline">Licensed Copy: 827106637</span>
            <span className="hidden md:inline">Total Bills: 142</span>
            <span className="font-bold text-[#b84716] dark:text-[#ffb07e]">Today's Sales: ₹ 1,84,650.00</span>
          </div>
        </footer>
      </div>
    </div>
  );
}
