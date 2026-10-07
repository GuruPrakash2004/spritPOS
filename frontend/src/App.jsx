// App shell and screen router.
//
// - Shows <Login /> until a user signs in, then the header, <Sidebar /> and the active screen.
// - There is no router library: `activeTab` holds a module name (see lib/modules.js) and
//   renderContent() switches on it. Every module has its own URL (lib/routes.js: /sales-invoice, /item-master, ...),
//   kept in step with `activeTab` through the History API, so a screen can be opened in another browser tab.
// - A module name that is a key of REPORTS (reports/reportDefs.js) renders through <ApiReport />.
// - Sales Invoice is two steps: <SalesInvoiceSetup /> fills `invoiceDraft`, then <SalesInvoice /> opens.
// - To add a screen: add its name to MODULE_GROUPS/MODULE_KEYWORDS, the Sidebar, MATRIX_SCREENS,
//   backend ROLES, and a `case` below.

import { SnackbarContainer } from './components/common/Toast';
import { useEffect, useState } from 'react';
import { ChevronRight, Menu, Settings, Moon, Sun, LogOut, ShieldAlert, Keyboard, KeyRound } from 'lucide-react';
import ChangePasswordDialog from './components/auth/ChangePasswordDialog';
import ScreenBoundary from './components/common/ScreenBoundary';
import KeyboardShortcutsModal from './components/common/KeyboardShortcutsModal';
import AdminMaster from './components/admin/AdminMaster';
import { getAuthToken, getDsr, getMe, requestTokenFromOtherTabs, setAuthRights, setAuthUser, setFieldRights, setSessionEndedHandler, todayIso, } from './lib/api';
import { ADMIN_MASTER, ADMIN_ROLE, MODULE_GROUPS, canOpen } from './lib/modules';
import { moduleForPath, pathForModule } from './lib/routes';
import { iconButton, primaryButton } from './lib/styles';
import { SIMPLE_MASTERS } from './lib/simpleMasters';
import { isTypingTarget, useAdvanceAfterSelect, useShortcuts } from './lib/keys';
import { useConfirm } from './components/common/ConfirmDialog';
import CashDenomination from './components/transactions/CashDenomination';
import ClosingStockEntry from './components/transactions/ClosingStockEntry';
import CompanyMaster from './components/masters/CompanyMaster';
import DailySummaryReport from './components/reports/DailySummaryReport';
import ExpenseEntry from './components/transactions/ExpenseEntry';
import ItemMaster from './components/masters/ItemMaster';
import Login from './components/auth/Login';
import ModuleSearch from './components/layout/ModuleSearch';
import {
  SimpleMasterScreen, BrandMaster, CustomerMaster, ItemGroupMaster, LedgerGroupMaster, LedgerMaster, PackageMaster, SalesPersonMaster, UOMMaster,
} from './components/masters/Masters';
import PriceListMaintenance from './components/masters/PriceListMaintenance';
import ApiReport from './components/reports/ApiReport';
import { REPORTS } from './components/reports/reportDefs';
import { PurchaseIndent, SalesReturn, StockDeduction } from './components/transactions/StockTransactions';
import PurchaseBill from './components/transactions/PurchaseBill';
import ReportsDashboard from './components/reports/ReportsDashboard';
import SalesCounterMaster from './components/masters/SalesCounterMaster';
import { NO_COUNTER, } from './data/salesCounters';
import { finYearLabel } from './lib/formatters';
import SalesInvoice from './components/transactions/SalesInvoice';
import SalesInvoiceSetup from './components/transactions/SalesInvoiceSetup';
import Sidebar from './components/layout/Sidebar';
import SupplierMaster from './components/masters/SupplierMaster';
import { Dashboard } from './components/dashboard/Dashboard';
import StockBalanceReport from './components/reports/StockBalanceReport';
import SqlQueryExecution from './components/tools/SqlQueryExecution';
import ReWriteStock from './components/tools/ReWriteStock';
import GlobalSettings from './components/utilities/GlobalSettings';
import AdminAuditLog from './components/utilities/AdminAuditLog';
import About from './components/utilities/About';
import RemoveDuplicateInvoice from './components/utilities/RemoveDuplicateInvoice';
import WebSync from './components/utilities/WebSync';
import BulkMasterUpdate from './components/utilities/BulkMasterUpdate';
import LabelPrinting from './components/utilities/LabelPrinting';
import MergeItems from './components/utilities/MergeItems';
import CompareStock from './components/utilities/CompareStock';
import { useCompanyProfile } from './hooks/useCompanyProfile';

// Per-tab memory for a page refresh (sessionStorage; cleared when the tab closes).
const FY_KEY = 'vlqms_fin_year';
// Screens whose unsaved rows Alt+B would throw away.
const ENTRY_SCREENS = ['Purchase Bill', 'Expense Entry', 'Stock Deduction', 'Purchase Indent', 'Sales Return', 'Closing Stock Entry', 'Cash Denomination'];
const SESSION_CHECK_MS = 60_000;
const readSaved = (key) => { try { return sessionStorage.getItem(key) || ''; } catch { return ''; } };
const writeSaved = (key, value) => { try { sessionStorage.setItem(key, value); } catch { /* storage blocked */ } };

export default function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);
  const [activeTab, setActiveTab] = useState(() => moduleForPath(window.location.pathname));
  const [darkMode, setDarkMode] = useState(false);
  const [invoiceDraft, setInvoiceDraft] = useState({
    counter: NO_COUNTER, invoiceNo: '', paymentType: 'CASH', customer: '',
  });
  const [billOpen, setBillOpen] = useState(false);
  const [showShortcuts, setShowShortcuts] = useState(false);
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [changingPassword, setChangingPassword] = useState(false);
  const [dsr, setDsr] = useState(null);
  const [session, setSession] = useState(null);
  const { company } = useCompanyProfile();
  // Why the last session ended (shown on the sign-in page), e.g. "Your session has expired".
  const [sessionNotice, setSessionNotice] = useState('');

  // Any 401 from the backend ends the session: forget the token and go back to the sign-in page.
  useEffect(() => {
    setSessionEndedHandler(reason => {
      setAuthUser('', '', '');
      setSession(null);
      setBillOpen(false);
      setSessionNotice(reason);
    });
    return () => setSessionEndedHandler(null);
  }, []);

  // After a page refresh the tab still holds the login token: check it with the server and restore
  // the session (and the screen the user was on) instead of showing the sign-in page again.
  const [restoring, setRestoring] = useState(true);
  useEffect(() => {
    // A new tab has no token yet: ask the tabs that are already open before showing the sign-in page.
    const token = getAuthToken() ? Promise.resolve('') : requestTokenFromOtherTabs();
    token.then(t => {
      if (t) setAuthUser('', '', t);
      if (!getAuthToken()) { setRestoring(false); return; }
      return getMe()
      .then(me => {
        setAuthUser(me.user, me.role);
        setAuthRights(me.modules, me.rights);
        setFieldRights(me.field_rights);
        setSession({ user: me, finYear: readSaved(FY_KEY) });
      })
      .catch(() => undefined)   // a 401 already sent us to the sign-in page with the reason
      .finally(() => setRestoring(false));
    });
  }, []);

  // Session check every minute and when the window regains focus: an expired token, a password change
  // or a deactivated user ends the session within a minute (the 401 handler above shows the reason),
  // and role changes made in Admin Master reach the sidebar and buttons without logging out.
  useEffect(() => {
    if (!session) return;
    const check = () => {
      getMe()
        .then(me => {
          setAuthRights(me.modules, me.rights);
          setFieldRights(me.field_rights);
          setSession(s => (s && (s.user.role !== me.role || s.user.modules.join() !== me.modules.join()
            || JSON.stringify(s.user.rights ?? {}) !== JSON.stringify(me.rights ?? {})
            || !!s.user.must_change_password !== !!me.must_change_password)
            ? { ...s, user: { ...s.user, role: me.role, modules: me.modules, rights: me.rights, must_change_password: me.must_change_password } } : s));
        })
        .catch(() => undefined);   // offline: try again next time; a 401 is handled by the session handler
    };
    const timer = setInterval(check, SESSION_CHECK_MS);
    const onFocus = () => check();
    window.addEventListener('focus', onFocus);
    return () => { clearInterval(timer); window.removeEventListener('focus', onFocus); };
  }, [session]);

  // Refresh the footer's bill count / today's sales from the backend DSR after login and whenever the
  // screen changes. The DSR needs a login token, so nothing is fetched on the sign-in page.
  useEffect(() => {
    if (!session) return;
    getDsr(todayIso()).then(setDsr).catch(() => setDsr(null));
  }, [session, activeTab, billOpen]);

  // Dialogs and toasts are portalled to <body>, so the theme class lives on <html> as well.
  useEffect(() => {
    document.documentElement.classList.toggle('dark', darkMode);
    document.body.classList.toggle('dark', darkMode);
  }, [darkMode]);

  useEffect(() => {
    const onPop = () => { setActiveTab(moduleForPath(window.location.pathname)); setBillOpen(false); };
    window.addEventListener('popstate', onPop);
    return () => window.removeEventListener('popstate', onPop);
  }, []);

  const openModule = (moduleName) => {
    const path = pathForModule(moduleName);
    if (window.location.pathname !== path) window.history.pushState(null, '', path);
    setActiveTab(moduleName);
    setBillOpen(false);
    if (window.innerWidth < 768) setSidebarOpen(false);
  };

  // App-wide keys: `?` shows the shortcuts sheet (not while typing); Alt+B starts a new sales bill from any screen,
  // asking first when it would leave an entry screen (its unsaved rows would be lost).
  useAdvanceAfterSelect();
  useShortcuts({
    'Shift+?': e => { if (!isTypingTarget(e.target)) setShowShortcuts(true); },
    '?': e => { if (!isTypingTarget(e.target)) setShowShortcuts(true); },
    'Alt+B': () => {
      if (!session || !canOpen(session.user.modules, session.user.role, 'Sales Invoice')) return;
      if (activeTab === 'Sales Invoice' && billOpen) return;   // already billing: F7 starts a new bill
      if (!ENTRY_SCREENS.includes(activeTab)) return openModule('Sales Invoice');
      confirm(`Leave ${activeTab} for a new sales bill? Anything not saved there is lost.`)
        .then(ok => { if (ok) openModule('Sales Invoice'); });
    },
  }, !!session);

  // Sales Invoice is two steps: the setup form collects counter + invoice details, then the bill screen opens.
  const openInvoiceSetup = (counter) => {
    setInvoiceDraft(draft => ({ ...draft, counter, invoiceNo: '' }));
    setBillOpen(false);
    setActiveTab('Sales Invoice');
  };

  const renderContent = () => {
    if (session && !canOpen(session.user.modules, session.user.role, activeTab)) return (
      <div className="flex h-full flex-col items-center justify-center rounded-xl border border-dashed border-[#D4C8BA] bg-[#FFFCF7] p-8 text-center dark:border-line-strong dark:bg-surface">
        <span className="mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-[#9A4B45]/10 text-[#9A4B45] dark:text-red-400"><ShieldAlert size={22} /></span>
        <h3 className="text-base font-semibold text-[#29231F] dark:text-stone-50">You don't have access to {activeTab}</h3>
        <p className="mt-1 max-w-md text-sm text-[#70675F] dark:text-stone-400">Your role ({session.user.role}) doesn't include this module. Ask an Administrator to grant it in Admin Master.</p>
        <button onClick={() => openModule('Dashboard')} className={`${primaryButton} mt-5`}>Go to Dashboard</button>
      </div>
    );
    if (activeTab === ADMIN_MASTER) return <AdminMaster currentUser={session?.user.user ?? ''} />;
    if (REPORTS[activeTab]) return <ApiReport name={activeTab} />;
    const simple = SIMPLE_MASTERS.find(m => m.title === activeTab);
    if (simple) return <SimpleMasterScreen spec={simple} />;
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
      case 'Sales Person Master': return <SalesPersonMaster />;
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
      case 'SQL Query Execution': return <SqlQueryExecution />;
      case 'Re Write Stock': return <ReWriteStock />;
      case 'Global Settings': return <GlobalSettings />;
      case 'ADMIN Audit Log': return <AdminAuditLog />;
      case 'About': return <About />;
      case 'Remove Duplicate Invoice': return <RemoveDuplicateInvoice />;
      case 'Replace and Remove Duplicate items': return <MergeItems key="replace" mode="replace" />;
      case 'Merge Items': return <MergeItems key="merge" mode="merge" />;
      case 'Compare Stock': return <CompareStock />;
      case 'Web Sync': return <WebSync />;
      case 'Bulk Master Update': return <BulkMasterUpdate />;
      case 'Label Printing': return <LabelPrinting />;
      default: return (
        <div className="flex h-full flex-col items-center justify-center rounded-xl border border-dashed border-[#D4C8BA] bg-[#FFFCF7] p-8 text-center dark:border-line-strong dark:bg-surface">
          <span className="mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-muted text-stone-500"><Settings size={22} /></span>
          <h3 className="text-base font-semibold text-[#29231F] dark:text-stone-50">{activeTab}</h3>
          <p className="mt-1 text-sm text-[#70675F] dark:text-stone-400">This screen isn't available in the web edition yet.</p>
        </div>
      );
    }
  };

  // After login, always start on Dashboard first and reset path to /
  if (restoring) {
    return <div className="flex min-h-screen items-center justify-center bg-[#FFFCF7] text-sm text-stone-500 dark:bg-[#0B0B0B] dark:text-stone-400">Signing you back in…</div>;
  }
  if (!session) return <Login notice={sessionNotice} onLogin={(user, finYear) => {
    writeSaved(FY_KEY, finYear);
    setSessionNotice('');
    setAuthUser(user.user, user.role, user.access_token || user.token);
    setAuthRights(user.modules, user.rights);
    setFieldRights(user.field_rights);
    setSession({ user, finYear });
    // Land on the screen the address names (a bookmark or a new tab) when this user may open it, else the Dashboard.
    const wanted = moduleForPath(window.location.pathname);
    const first = canOpen(user.modules, user.role, wanted) ? wanted : 'Dashboard';
    window.history.replaceState(null, '', pathForModule(first));
    setActiveTab(first);
  }} />;
  const { user, finYear } = session;
  const moduleGroup = activeTab === ADMIN_MASTER ? 'Administration'
    : REPORTS[activeTab] ? 'Reports'
    : MODULE_GROUPS.find(g => g.modules.includes(activeTab))?.group ?? 'General';
  const moduleLabel = activeTab === 'Reports Dashboard' ? 'Customised Reports'
    : activeTab === ADMIN_MASTER ? 'Admin Master (Security)' : activeTab;

  return (
    <div className={`app-shell flex h-screen overflow-hidden font-sans ${darkMode ? 'dark bg-[#0A0A0A] text-stone-100' : 'bg-[#F5F1E9] text-[#29231F]'}`}>
      <Sidebar isOpen={isSidebarOpen} activeTab={activeTab} onClose={() => setSidebarOpen(false)} onOpenModule={openModule} user={user.user} canOpen={name => canOpen(user.modules, user.role, name)} />

      <div className="flex min-w-0 flex-1 flex-col overflow-hidden">
        {/* z-20 so the search results drop down over the workspace. */}
        <header className="relative z-20 flex shrink-0 flex-wrap items-center gap-x-3 gap-y-2 border-b px-3 py-2.5 sm:h-16 sm:flex-nowrap sm:gap-4 sm:px-6 sm:py-0
                           bg-[#FFFCF7] border-[#E3D9CD]
                           dark:border-[#1E1E1E] dark:bg-[#0E0E0E]/95 dark:backdrop-blur-xl">
          <button className={`${iconButton} shrink-0 md:hidden print:hidden`} onClick={() => setSidebarOpen(true)} aria-label="Open navigation">
            <Menu size={20} />
          </button>
          <div className="min-w-0 flex-1 sm:flex-none sm:shrink">
            <nav aria-label="Breadcrumb" className="flex items-center gap-1 text-xs text-[#70675F] dark:text-stone-400">
              <span className="hidden truncate sm:inline">{company?.name || user.company.name}</span>
              <ChevronRight size={12} className="hidden shrink-0 sm:block" />
              <span className="truncate">{moduleGroup}</span>
            </nav>
            <h2 className="truncate text-[15px] font-semibold leading-tight text-[#29231F] dark:text-stone-50">{moduleLabel}</h2>
          </div>

          <div className="order-last w-full sm:order-0 sm:ml-auto sm:w-auto sm:flex-1 sm:max-w-md flex items-center gap-2">
            <div className="flex-1 min-w-0">
              <ModuleSearch canOpen={name => canOpen(user.modules, user.role, name)} onOpen={openModule} />
            </div>
            <button
              onClick={() => setShowShortcuts(true)}
                className="hidden lg:inline-flex h-8 w-8 items-center justify-center rounded-full border border-[#E3D9CD] bg-[#FFFCF7] text-xs font-medium text-[#70675F] hover:bg-[#F3EEE6] hover:text-[#29231F] transition-all duration-180 active:scale-[0.98] dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-300 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-100 shrink-0 shadow-2xs"
              title="Keyboard Shortcuts (Press ?)"
              aria-label="Show keyboard shortcuts"
            >
              <Keyboard size={14} className="text-[#64283A] dark:text-[#D4A847]" />
            </button>
          </div>

          <div className="flex shrink-0 items-center gap-1 sm:gap-2">
            <span className="hidden rounded-md border px-2 py-1 text-[11px] font-medium tabular-nums lg:inline
                             border-[#E3D9CD] text-[#64283A]
                             dark:border-[#2A2A2A] dark:text-[#D4A847]" title="Financial year">FY {finYear || finYearLabel(company?.fy_start, company?.fy_end) || '—'}</span>
            <button onClick={() => setDarkMode(!darkMode)} className={`${iconButton} print:hidden`} title={darkMode ? 'Switch to light theme' : 'Switch to dark theme'} aria-label={darkMode ? 'Switch to light theme' : 'Switch to dark theme'}>
              {darkMode ? <Sun size={17} /> : <Moon size={17} />}
            </button>
            <div className="ml-1 flex items-center gap-2 border-l pl-3 border-[#E3D9CD] dark:border-[#2A2A2A]">
              <div className="flex h-8 w-8 items-center justify-center rounded-full text-[11px] font-semibold
                              bg-[#64283A]/10 text-[#64283A]
                              dark:bg-linear-to-br dark:from-[#D4A847]/20 dark:to-[#C8952E]/10 dark:text-[#D4A847] dark:ring-1 dark:ring-[#D4A847]/20" aria-hidden="true">{user.user.slice(0, 2)}</div>
              <div className="hidden leading-tight sm:block">
                <div className="text-xs font-medium text-[#29231F] dark:text-stone-50">{user.user}</div>
                <div className="text-[11px] text-[#70675F] dark:text-stone-400">{user.role}</div>
              </div>
              <button onClick={() => setChangingPassword(true)} className={`${iconButton} print:hidden`} title="Change my password" aria-label="Change my password"><KeyRound size={16} /></button>
              <button onClick={() => { setSession(null); setAuthUser('', '', ''); setBillOpen(false); setActiveTab('Dashboard'); window.history.replaceState(null, '', '/'); }} className={`${iconButton} hover:text-[#9A4B45] print:hidden dark:hover:text-red-400`} title="Log out" aria-label="Log out"><LogOut size={16} /></button>
            </div>
          </div>
        </header>

        <main className="flex-1 overflow-auto p-4 sm:p-6 bg-[#F5F1E9] dark:bg-[#0A0A0A]">
          <ScreenBoundary key={activeTab} onHome={() => openModule('Dashboard')}>{renderContent()}</ScreenBoundary>
        </main>

        <footer className="flex h-9 shrink-0 items-center justify-between gap-4 border-t px-4 text-[11px] sm:px-6 print:hidden
                           bg-[#FFFCF7] border-[#E3D9CD] text-[#70675F]
                           dark:border-[#1E1E1E] dark:bg-[#0E0E0E] dark:text-stone-500">
         <div className="flex items-center gap-2 sm:gap-3">
            <span className="flex items-center gap-1.5 font-medium">
              <span className={`h-1.5 w-1.5 rounded-full ${dsr ? 'bg-[#55745A] dark:bg-emerald-500 dark:shadow-[0_0_6px_rgba(16,185,129,0.5)]' : 'bg-[#9A4B45]'}`} />
              {dsr ? 'Connected' : 'Offline'}
            </span>
            <span className="text-[#A4998E] dark:text-stone-600">·</span>
            <span>Licence 827106637</span>
          </div>
          <div className="flex items-center gap-4 tabular-nums">
            <span className="hidden sm:inline">Bills today <span className="font-medium text-[#29231F] dark:text-stone-200">{dsr?.total_bills ?? '—'}</span></span>
            <span>Sales today <span className="font-semibold text-[#64283A] dark:text-[#D4A847]">₹ {(dsr?.sales_incl_gst ?? 0).toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span></span>
          </div>
        </footer>
      </div>
      <SnackbarContainer />
      {changingPassword && !user.must_change_password && <ChangePasswordDialog user={user.user} onClose={() => setChangingPassword(false)} />}
      {user.must_change_password && <ChangePasswordDialog forced user={user.user}
        onClose={() => setSession(s => (s ? { ...s, user: { ...s.user, must_change_password: false } } : s))} />}
      {showShortcuts && <KeyboardShortcutsModal onClose={() => setShowShortcuts(false)} />}
      {confirmDialog}
    </div>
  );
}
