import { useEffect, useState } from 'react';
import { Menu, Settings, Moon, Sun, LogOut, ShieldAlert } from 'lucide-react';
import AdminMaster from './components/AdminMaster';
import { api, setAuthUser, todayIso, type ApiDsr, type ApiUser } from './components/api';
import { ADMIN_MASTER, ADMIN_PATH, ADMIN_ROLE, canOpen } from './components/modules';
import CashDenomination from './components/CashDenomination';
import ClosingStockEntry from './components/ClosingStockEntry';
import CompanyMaster from './components/CompanyMaster';
import DailySummaryReport from './components/DailySummaryReport';
import ExpenseEntry from './components/ExpenseEntry';
import ItemMaster from './components/ItemMaster';
import Login from './components/Login';
import ModuleSearch from './components/ModuleSearch';
import {
  BrandMaster, CustomerMaster, ItemGroupMaster, LedgerGroupMaster, LedgerMaster, PackageMaster, UOMMaster,
} from './components/Masters';
import PriceListMaintenance from './components/PriceListMaintenance';
import ApiReport from './components/reports/ApiReport';
import { REPORTS } from './components/reports/reportDefs';
import { PurchaseIndent, SalesReturn, StockDeduction } from './components/StockTransactions';
import PurchaseBill from './components/PurchaseBill';
import ReportsDashboard from './components/ReportsDashboard';
import SalesCounterMaster from './components/SalesCounterMaster';
import { DEFAULT_COUNTER, FIRST_BILL_SEQ, billNumber, type InvoiceSetup, type SalesCounter } from './components/salesCounters';
import SalesInvoice from './components/SalesInvoice';
import SalesInvoiceSetup from './components/SalesInvoiceSetup';
import Sidebar from './components/Sidebar';
import SupplierMaster from './components/SupplierMaster';
import { Dashboard, StockBalanceReport } from './components/OperationalModules';

export default function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);
  // /adminMaster deep-links to Admin Master; every other module lives at /.
  const [activeTab, setActiveTab] = useState(() => (window.location.pathname === ADMIN_PATH ? ADMIN_MASTER : 'Dashboard'));
  const [darkMode, setDarkMode] = useState(false);
  const [invoiceDraft, setInvoiceDraft] = useState<InvoiceSetup>({
    counter: DEFAULT_COUNTER, invoiceNo: billNumber(DEFAULT_COUNTER.prefix, FIRST_BILL_SEQ), paymentType: 'CASH', customer: '',
  });
  const [billOpen, setBillOpen] = useState(false);
  const [dsr, setDsr] = useState<ApiDsr | null>(null);
  const [session, setSession] = useState<{ user: ApiUser; finYear: string } | null>(null);

  // Refresh the footer's bill count / today's sales from the backend DSR whenever the screen changes.
  useEffect(() => {
    api<ApiDsr>(`/reports/dsr?on=${todayIso()}`).then(setDsr).catch(() => setDsr(null));
  }, [activeTab, billOpen]);

  useEffect(() => {
    const handlePointerMove = (event: PointerEvent) => {
      document.documentElement.style.setProperty('--cursor-x', `${event.clientX}px`);
      document.documentElement.style.setProperty('--cursor-y', `${event.clientY}px`);
    };

    window.addEventListener('pointermove', handlePointerMove);
    return () => window.removeEventListener('pointermove', handlePointerMove);
  }, []);

  useEffect(() => {
    const onPop = () => setActiveTab(window.location.pathname === ADMIN_PATH ? ADMIN_MASTER : 'Dashboard');
    window.addEventListener('popstate', onPop);
    return () => window.removeEventListener('popstate', onPop);
  }, []);

  const openModule = (moduleName: string) => {
    const path = moduleName === ADMIN_MASTER ? ADMIN_PATH : '/';
    if (window.location.pathname !== path) window.history.pushState(null, '', path);
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
    if (session && !canOpen(session.user.modules, session.user.role, activeTab)) return (
      <div className="flex h-full flex-col items-center justify-center rounded-2xl border border-dashed border-[#e1c9b6] bg-[#fffaf5] p-8 text-center dark:border-[#343b59] dark:bg-[#171b2b]">
        <ShieldAlert size={44} className="mb-3 text-red-500" />
        <h3 className="text-lg font-bold text-stone-800 dark:text-stone-100">No access to {activeTab}</h3>
        <p className="mt-1 text-sm text-stone-500">Your role ({session.user.role}) doesn't include this module. Ask an Administrator to change it in Admin Master.</p>
        <button onClick={() => openModule('Dashboard')} className="mt-4 rounded-lg bg-[#e8702a] px-4 py-2 text-xs font-bold text-white hover:bg-[#b84716]">Go to Dashboard</button>
      </div>
    );
    if (activeTab === ADMIN_MASTER) return <AdminMaster currentUser={session?.user.user ?? ''} />;
    if (REPORTS[activeTab]) return <ApiReport name={activeTab} />;
    switch (activeTab) {
      case 'Dashboard': return <Dashboard />;
      case 'Company Master': return <CompanyMaster canEditFiles={session?.user.role === ADMIN_ROLE} />;
      case 'Item Master': return <ItemMaster />;
      case 'Item Group Master': return <ItemGroupMaster />;
      case 'UOM Master': return <UOMMaster />;
      case 'Supplier Master': return <SupplierMaster />;
      case 'Customer Master': return <CustomerMaster />;
      case 'Ledger Master': return <LedgerMaster />;
      case 'Ledger Group Master': return <LedgerGroupMaster />;
      case 'Sales Counter Master': return <SalesCounterMaster onSave={openInvoiceSetup} />;
      case 'Package Master': return <PackageMaster />;
      case 'Price List Maintenance': return <PriceListMaintenance />;
      case 'Brand Master': return <BrandMaster />;
      case 'Purchase Bill': return <PurchaseBill />;
      case 'Sales Invoice': return billOpen
        ? <SalesInvoice setup={invoiceDraft} onClose={() => setBillOpen(false)} />
        : <SalesInvoiceSetup initial={invoiceDraft} onSubmit={setup => { setInvoiceDraft(setup); setBillOpen(true); }} />;
      case 'Expense Entry': return <ExpenseEntry />;
      case 'Stock Deduction': return <StockDeduction />;
      case 'Purchase Indent': return <PurchaseIndent />;
      case 'Sales Return': return <SalesReturn />;
      case 'Closing Stock Entry': return <ClosingStockEntry />;
      case 'Cash Denomination': return <CashDenomination />;
      case 'Daily Summary Report': return <DailySummaryReport />;
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

  // After login, keep a /adminMaster deep link; otherwise start on the Dashboard.
  if (!session) return <Login onLogin={(user, finYear) => {
    setAuthUser(user.user);
    setSession({ user, finYear });
    setActiveTab(window.location.pathname === ADMIN_PATH ? ADMIN_MASTER : 'Dashboard');
  }} />;
  const { user, finYear } = session;

  return (
    <div className={`app-shell flex h-screen overflow-hidden font-sans transition-colors duration-300 ${darkMode ? 'dark bg-[#0e101b] text-stone-100' : 'bg-[#f6efe8] text-stone-800'}`}>
      <Sidebar isOpen={isSidebarOpen} activeTab={activeTab} onClose={() => setSidebarOpen(false)} onOpenModule={openModule} user={user.user} canOpen={name => canOpen(user.modules, user.role, name)} />

      <div className="flex min-w-0 flex-1 flex-col overflow-hidden">
        <header className="z-20 flex shrink-0 items-center justify-between border-b border-[#e1c9b6] bg-[#fffaf5]/90 min-h-[70px] gap-2 px-3 py-2 shadow-sm sm:px-6 sm:py-[13px] backdrop-blur dark:border-[#343b59] dark:bg-[#171b2b]/90">
          <div className="flex min-w-0 items-center gap-2 sm:gap-4">
            <button className="shrink-0 rounded-lg p-1 text-stone-600 hover:bg-stone-100 dark:text-stone-300 dark:hover:bg-stone-800 md:hidden" onClick={() => setSidebarOpen(true)} aria-label="Open navigation">
              <Menu size={22} />
            </button>
            {/* One line each on phones, so the header never grows into the active-module bar below. */}
            <div className="min-w-0">
              <h2 className="truncate text-base font-extrabold leading-tight tracking-[0.05em] text-stone-900 dark:text-stone-100 sm:text-[19px]">THE LIQUOR HOUSE</h2>
              <span className="block truncate font-mono text-[11px] text-stone-500 sm:text-xs">FY: {finYear}<span className="hidden sm:inline"> • CL-2 License</span></span>
            </div>
          </div>
          <div className="flex shrink-0 items-center gap-1 sm:gap-3">
            <button onClick={() => setDarkMode(!darkMode)} className="rounded-xl p-2 text-stone-500 transition-colors hover:bg-stone-100 dark:hover:bg-stone-800" title="Toggle Dark / Light Theme" aria-label="Toggle Dark / Light Theme">
              {darkMode ? <Sun size={18} /> : <Moon size={18} />}
            </button>
            <div className="flex items-center gap-2 border-l border-[#e1c9b6] pl-3 text-sm text-stone-700 dark:border-[#343b59] dark:text-stone-300">
              <div className="flex h-8 w-8 items-center justify-center rounded-full bg-linear-135 from-[#e8702a] to-[#e75b6f] text-xs font-bold text-white">{user.user.slice(0, 2)}</div>
              <span className="hidden text-xs font-medium sm:block">{user.user} <span className="text-stone-400">• {user.role}</span></span>
              <button onClick={() => { setSession(null); setAuthUser(''); setBillOpen(false); }} className="rounded-lg p-1.5 text-stone-500 hover:bg-stone-100 hover:text-red-600 dark:hover:bg-stone-800" title="Log out" aria-label="Log out"><LogOut size={16} /></button>
            </div>
          </div>
        </header>

        {/* z-10 so the search results drop down over the workspace (a separate stacking context). */}
        <div className="relative z-10 flex h-[46px] shrink-0 items-center gap-3 border-b border-[#e1c9b6] bg-[#f1e3d8]/80 px-3 dark:border-[#343b59] dark:bg-[#20253a]/80 sm:px-5">
          <div className="flex h-8 min-w-0 shrink items-center gap-2 rounded-full border border-[#e1c9b6] bg-[#fffaf5] px-[18px] text-xs font-bold text-[#b84716] shadow-[0_5px_16px_rgba(112,58,28,0.12)] dark:border-[#343b59] dark:bg-[#171b2b] dark:text-[#ffb07e]">
            <span className="h-2 w-2 rounded-full bg-[#e8702a]" />
            <span className="truncate">{activeTab}</span>
          </div>
          <ModuleSearch canOpen={name => canOpen(user.modules, user.role, name)} onOpen={openModule} />
        </div>

        <main className="workspace-backdrop flex-1 overflow-auto bg-[#f6efe8] p-6 transition-colors duration-300 dark:bg-[#0e101b]">
          {renderContent()}
        </main>

        <footer className="flex h-10 shrink-0 items-center justify-between border-t border-[#e1c9b6] bg-[#fffaf5]/90 px-5 backdrop-blur font-mono text-[11px] text-stone-500 dark:border-[#343b59] dark:bg-[#171b2b]/90">
          <div className="flex items-center gap-3">
            <span className="flex items-center gap-1.5 font-semibold text-[#b84716]"><span className="h-1.5 w-1.5 rounded-full bg-[#e8702a]" />Ready</span>
            <span className="hidden sm:inline">{dsr ? 'API: connected' : 'API: offline'}</span>
            <span className="hidden md:inline">Login: {user.user}</span>
          </div>
          <div className="flex items-center gap-4">
            <span className="hidden font-semibold text-stone-700 dark:text-stone-300 sm:inline">Licensed Copy: 827106637</span>
            <span className="hidden md:inline">Total Bills: {dsr?.total_bills ?? '—'}</span>
            <span className="font-bold text-[#b84716] dark:text-[#ffb07e]">Today's Sales: ₹ {(dsr?.sales_incl_gst ?? 0).toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
          </div>
        </footer>
      </div>
    </div>
  );
}
