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

import { OPEN_MODULE } from './lib/nav';
import { SnackbarContainer } from './components/common/Toast';
import DesignerNotice from './components/common/DesignerNotice';
import { Suspense, useEffect, useState } from 'react';
import { ChevronRight, Menu, Settings, ShieldAlert, Keyboard } from 'lucide-react';
import ChangePasswordDialog from './components/auth/ChangePasswordDialog';
import VlqmsLogo from './components/layout/VlqmsLogo';
import ScreenBoundary from './components/common/ScreenBoundary';
import KeyboardShortcutsModal from './components/common/KeyboardShortcutsModal';
import CalculatorDialog from './components/common/CalculatorDialog';
import { getAuthToken, getDsr, getMe, getTodayTotals, requestTokenFromOtherTabs, setAuthRights, setAuthUser, setFieldRights, setSessionEndedHandler, todayIso, } from './lib/api';
import { ADMIN_MASTER, ADMIN_ROLE, MODULE_GROUPS, canOpen } from './lib/modules';
import { moduleForPath, pathForModule } from './lib/routes';
import { iconButton, primaryButton } from './lib/styles';
import { setExportCompany } from './lib/xlsExport';
import UserMenu from './components/layout/UserMenu';
import { SIMPLE_MASTERS } from './lib/simpleMasters';
import { isTypingTarget, useAdvanceAfterSelect, useShortcuts } from './lib/keys';
import { withViewTransition } from './lib/viewTransition';
import { useConfirm } from './components/common/ConfirmDialog';
import CashDenomination from './components/transactions/CashDenomination';
import ClosingStockEntry from './components/transactions/ClosingStockEntry';
import VoucherEntry from './components/accounting/VoucherEntry';
import PhysicalStockEntry from './components/transactions/PhysicalStockEntry';
import CompanyMaster from './components/masters/CompanyMaster';
import ExpenseEntry from './components/transactions/ExpenseEntry';
import ItemMaster from './components/masters/ItemMaster';
import Login from './components/auth/Login';
import ModuleSearch from './components/layout/ModuleSearch';
import {
  SimpleMasterScreen, BrandMaster, CustomerMaster, ItemGroupMaster, LedgerGroupMaster, LedgerMaster, PackageMaster, SalesPersonMaster, UOMMaster,
} from './components/masters/Masters';
import ApiReport from './components/reports/ApiReport';
import { REPORTS } from './components/reports/reportDefs';
import { PurchaseIndent, SalesReturn, StockDeduction } from './components/transactions/StockTransactions';
import PurchaseBill from './components/transactions/PurchaseBill';
import SalesCounterMaster from './components/masters/SalesCounterMaster';
import { NO_COUNTER, } from './data/salesCounters';
import { finYearLabel } from './lib/formatters';
import FinYearSwitch from './components/layout/FinYearSwitch';
import SalesInvoice from './components/transactions/SalesInvoice';
import SalesInvoiceSetup from './components/transactions/SalesInvoiceSetup';
import Sidebar from './components/layout/Sidebar';
import SupplierMaster from './components/masters/SupplierMaster';
import { Dashboard } from './components/dashboard/Dashboard';
import { useCompanyProfile } from './hooks/useCompanyProfile';
import { lazyScreen, prefetchAllWhenIdle } from './lib/screenLoaders';
import { ScreenSkeleton } from './components/common/Skeleton';

// Rarely used screens load on demand (lib/screenLoaders.js); the rest are in the main bundle.
const AdminMaster = lazyScreen('Admin Master');
const SqlQueryExecution = lazyScreen('SQL Query Execution');
const ReWriteStock = lazyScreen('Re Write Stock');
const GlobalSettings = lazyScreen('Global Settings');
const AdminAuditLog = lazyScreen('ADMIN Audit Log');
const KeyGenerator = lazyScreen('Key Generation');
const About = lazyScreen('About');
const RemoveDuplicateInvoice = lazyScreen('Remove Duplicate Invoice');
const BulkMasterUpdate = lazyScreen('Bulk Master Update');
const LabelPrinting = lazyScreen('Label Printing');
const MergeItems = lazyScreen('Merge Items');
const CompareStock = lazyScreen('Compare Stock');
const UpdateItemStock = lazyScreen('Update Item Stock');
const ImportItemMasterFields = lazyScreen('Import Item Master Fields');
const ItemVsBrand = lazyScreen('Item Vs Brand');
const ImportItemData = lazyScreen('Import Item Data From Excel');
const SmsScreen = lazyScreen('SMS');
const CustomerVsItem = lazyScreen('Customer Vs Item Master');
const SupplierVsItem = lazyScreen('Supplier Vs Item Master');
const PdfGenerator = lazyScreen('PDF Generator');
const Quotation = lazyScreen('Quotation');
const GeneralLedger = lazyScreen('General Ledger');
const PartyOutstanding = lazyScreen('Party wise OutStanding');
const BillWiseOutstanding = lazyScreen('Bill wise OutStanding');
const BillWiseAgeing = lazyScreen('Bill wise Ageing');
const SharedDocuments = lazyScreen('Shared Documents');
const DeleteNonLiquorItems = lazyScreen('Delete Non-Liquor Items');
const ReportsDashboard = lazyScreen('Reports Dashboard');
const DailySummaryReport = lazyScreen('Daily Summary Report');
const StockBalanceReport = lazyScreen('Stock Balance Report');
const PriceListMaintenance = lazyScreen('Price List Maintenance');

// Per-tab memory for a page refresh (sessionStorage; cleared when the tab closes).
const FY_KEY = 'vlqms_fin_year';
// Screens whose unsaved rows Alt+B would throw away.
const ENTRY_SCREENS = ['Payment Voucher', 'Receipt Voucher', 'Credit Note', 'Debit Note', 'Contra Voucher', 'Journal Voucher', 'Quotation', 'Purchase Bill', 'Expense Entry', 'Stock Deduction', 'Purchase Indent', 'Sales Return', 'Closing Stock Entry', 'Physical Stock Entry', 'Cash Denomination'];
const SESSION_CHECK_MS = 60_000;
// Whether the left navigation is a narrow icon rail (a per-browser convenience, so localStorage).
const NAV_COLLAPSED_KEY = 'vlqms_nav_collapsed';
// Billing screens start with the rail so the item grid gets the width.
const BILLING_SCREENS = ['Sales Invoice', 'Purchase Bill'];
// Screens that open with no transition: the user starts typing at once (billing and the other entry screens).
const QUIET_SCREENS = [...BILLING_SCREENS, 'Payment Voucher', 'Receipt Voucher', 'Credit Note', 'Debit Note', 'Contra Voucher', 'Journal Voucher', 'Quotation', 'Expense Entry', 'Stock Deduction', 'Purchase Indent', 'Sales Return', 'Closing Stock Entry', 'Physical Stock Entry', 'Cash Denomination'];
const readNavCollapsed = () => { try { return localStorage.getItem(NAV_COLLAPSED_KEY) === '1'; } catch { return false; } };
const writeNavCollapsed = (on) => { try { localStorage.setItem(NAV_COLLAPSED_KEY, on ? '1' : '0'); } catch { /* storage blocked */ } };
const readSaved = (key) => { try { return sessionStorage.getItem(key) || ''; } catch { return ''; } };
const writeSaved = (key, value) => { try { sessionStorage.setItem(key, value); } catch { /* storage blocked */ } };

export default function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);
  const [navCollapsed, setNavCollapsed] = useState(() => readNavCollapsed() || BILLING_SCREENS.includes(moduleForPath(window.location.pathname)));
  // The rail widens / narrows through a view transition (the sidebar morphs on the GPU instead of animating its width).
  const toggleNav = () => withViewTransition(() => setNavCollapsed(c => { writeNavCollapsed(!c); return !c; }));
  const [activeTab, setActiveTab] = useState(() => moduleForPath(window.location.pathname));
  const [darkMode, setDarkMode] = useState(false);
  const [invoiceDraft, setInvoiceDraft] = useState({
    counter: NO_COUNTER, invoiceNo: '', paymentType: 'CASH', customer: '',
  });
  const [billOpen, setBillOpen] = useState(false);
  const [newCounter, setNewCounter] = useState(false);   // Sales Counter Master opened from Sales Invoice to add the first counter
  const [showShortcuts, setShowShortcuts] = useState(false);
  const [showCalc, setShowCalc] = useState(false);
  const { confirm, dialog: confirmDialog } = useConfirm();
  const [changingPassword, setChangingPassword] = useState(false);
  const [dsr, setDsr] = useState(null);
  const [totals, setTotals] = useState(null);   // the footer's Total Bills / Sales, read from the database
  const [session, setSession] = useState(null);
  const { company } = useCompanyProfile();
  useEffect(() => { setExportCompany(company?.name || session?.user?.company?.name); }, [company?.name, session]);   // first title row of every Excel export
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
    const load = () => {
      getDsr(todayIso()).then(setDsr).catch(() => setDsr(null));
      getTodayTotals().then(setTotals).catch(() => setTotals(null));
    };
    load();
    // The footer's Total Bills / Sales follow a bill saved or cancelled here at once, and a bill made in the Windows vLQMS (or another PC)
    // within 15 seconds or when this window gets the focus back.
    window.addEventListener('vlqms:bills-changed', load);
    window.addEventListener('focus', load);
    const timer = window.setInterval(load, 15000);
    return () => { window.removeEventListener('vlqms:bills-changed', load); window.removeEventListener('focus', load); window.clearInterval(timer); };
  }, [session, activeTab, billOpen]);

  // Dialogs and toasts are portalled to <body>, so the theme class lives on <html> as well.
  useEffect(() => {
    document.documentElement.classList.toggle('dark', darkMode);
    document.body.classList.toggle('dark', darkMode);
  }, [darkMode]);

  useEffect(() => {
    const onPop = () => {
      const target = moduleForPath(window.location.pathname);
      withViewTransition(() => { setActiveTab(target); setBillOpen(false); }, !QUIET_SCREENS.includes(target));
    };
    window.addEventListener('popstate', onPop);
    return () => window.removeEventListener('popstate', onPop);
  }, []);

  // Once signed in and idle, fetch the code of the on-demand screens so opening one is instant.
  const signedIn = !!session;
  useEffect(() => (signedIn && import.meta.env.MODE !== 'test' ? prefetchAllWhenIdle() : undefined), [signedIn]);

  const openModule = (moduleName) => {
    const path = pathForModule(moduleName);
    if (window.location.pathname !== path) window.history.pushState(null, '', path);
    withViewTransition(() => {
      setActiveTab(moduleName);
      setNewCounter(false);
      setBillOpen(false);
      if (BILLING_SCREENS.includes(moduleName)) setNavCollapsed(true);   // not saved: only the toggle sets the preference
      if (window.innerWidth < 768) setSidebarOpen(false);
    }, !QUIET_SCREENS.includes(moduleName));
  };
  // Screens (the action bar's Close, ...) ask to open another screen through lib/nav.js.
  useEffect(() => {
    const onOpen = (e) => openModule(String(e.detail));
    window.addEventListener(OPEN_MODULE, onOpen);
    return () => window.removeEventListener(OPEN_MODULE, onOpen);
  });

  // App-wide keys: `?` shows the shortcuts sheet (not while typing); Alt+B starts a new sales bill from any screen,
  // asking first when it would leave an entry screen (its unsaved rows would be lost).
  useAdvanceAfterSelect();
  useShortcuts({
    'Mod+B': e => { e.preventDefault(); toggleNav(); },
    'Shift+?': e => { if (!isTypingTarget(e.target)) setShowShortcuts(true); },
    '?': e => { if (!isTypingTarget(e.target)) setShowShortcuts(true); },
    'Alt+K': () => setShowCalc(true),   // the desktop's calculator
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
    setNewCounter(false);
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
      case 'Sales Counter Master': return <SalesCounterMaster onSave={openInvoiceSetup} startNew={newCounter} />;
      case 'Package Master': return <PackageMaster />;
      case 'Price List Maintenance': return <PriceListMaintenance />;
      case 'Brand Master': return <BrandMaster />;
      case 'Purchase Bill': return <PurchaseBill />;
      case 'Sales Invoice': return billOpen
        ? <SalesInvoice setup={invoiceDraft} onClose={() => setBillOpen(false)} />
        : <SalesInvoiceSetup initial={invoiceDraft} onCreateCounter={() => { setNewCounter(true); setActiveTab('Sales Counter Master'); }} onSubmit={setup => { setInvoiceDraft(setup); setBillOpen(true); }} />;
      case 'Expense Entry': return <ExpenseEntry />;
      case 'Stock Deduction': return <StockDeduction />;
      case 'Purchase Indent': return <PurchaseIndent />;
      case 'Sales Return': return <SalesReturn />;
      case 'Closing Stock Entry': return <ClosingStockEntry />;
      case 'Physical Stock Entry': return <PhysicalStockEntry />;
      case 'Cash Denomination': return <CashDenomination />;
      case 'Daily Summary Report': return <DailySummaryReport />;
      case 'Stock Balance Report': return <StockBalanceReport />;
      case 'Reports Dashboard':
      case 'Customised Reports': return <ReportsDashboard />;
      case 'SQL Query Execution': return <SqlQueryExecution />;
      case 'Re Write Stock': return <ReWriteStock />;
      case 'Global Settings': return <GlobalSettings />;
      case 'ADMIN Audit Log': return <AdminAuditLog />;
      case 'Key Generation': return <KeyGenerator />;
      case 'About': return <About />;
      case 'Remove Duplicate Invoice': return <RemoveDuplicateInvoice />;
      case 'Replace and Remove Duplicate items': return <MergeItems key="replace" mode="replace" />;
      case 'Merge Items': return <MergeItems key="merge" mode="merge" />;
      case 'Compare Stock': return <CompareStock />;
      case 'Update Item Stock': return <UpdateItemStock />;
      case 'Import Item Master Fields': return <ImportItemMasterFields />;
      case 'Item Vs Brand': return <ItemVsBrand />;
      case 'Import Item Data From Excel': return <ImportItemData />;
      case 'SMS': return <SmsScreen />;
      case 'Customer Vs Item Master': return <CustomerVsItem />;
      case 'Supplier Vs Item Master': return <SupplierVsItem />;
      case 'PDF Generator': return <PdfGenerator />;
      case 'Quotation': return <Quotation />;
      case 'General Ledger': return <GeneralLedger />;
      case 'Party wise OutStanding': return <PartyOutstanding />;
      case 'Bill wise OutStanding': return <BillWiseOutstanding />;
      case 'Bill wise Ageing': return <BillWiseAgeing />;
      case 'Payment Voucher': return <VoucherEntry kind="payment" title="Payment Voucher" />;
      case 'Receipt Voucher': return <VoucherEntry kind="receipt" title="Receipt Voucher" />;
      case 'Credit Note': return <VoucherEntry kind="credit-note" title="Credit Note" />;
      case 'Debit Note': return <VoucherEntry kind="debit-note" title="Debit Note" />;
      case 'Contra Voucher': return <VoucherEntry kind="contra" title="Contra Voucher" />;
      case 'Journal Voucher': return <VoucherEntry kind="journal" title="Journal Voucher" />;
      case 'Shared Documents': return <SharedDocuments />;
      case 'Delete Non-Liquor Items': return <DeleteNonLiquorItems />;
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
    return (
      <div className="flex min-h-screen flex-col items-center justify-center gap-4 bg-[#F5F1E9] dark:bg-[#0A0A0A]">
        <VlqmsLogo size="md" />
        <p className="text-sm text-stone-500 dark:text-stone-400">Signing you back in…</p>
      </div>
    );
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
      <Sidebar isOpen={isSidebarOpen} collapsed={navCollapsed} onToggleCollapse={toggleNav} activeTab={activeTab} onClose={() => setSidebarOpen(false)} onOpenModule={openModule} user={user.user} canOpen={name => canOpen(user.modules, user.role, name)} />

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
                className="hidden lg:inline-flex h-8 w-8 items-center justify-center rounded-full border border-[#E3D9CD] bg-[#FFFCF7] text-xs font-medium text-[#70675F] hover:bg-[#F3EEE6] hover:text-[#29231F] transition duration-150 active:scale-[0.98] dark:border-[#2A2A2A] dark:bg-[#141414] dark:text-stone-300 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-100 shrink-0 shadow-2xs"
              title="Keyboard Shortcuts (Press ?)"
              aria-label="Show keyboard shortcuts"
            >
              <Keyboard size={14} className="text-[#64283A] dark:text-[#D4A847]" />
            </button>
          </div>

          <div className="flex shrink-0 items-center gap-1 sm:gap-2">
            <FinYearSwitch label={finYear || finYearLabel(company?.fy_start, company?.fy_end)} years={company?.fin_years ?? []} canChange={user.role === ADMIN_ROLE}
              onChanged={label => { writeSaved(FY_KEY, label); setSession(s => (s ? { ...s, finYear: label } : s)); }} />
            <UserMenu user={user.user} role={user.role} isAdmin={user.role === ADMIN_ROLE} darkMode={darkMode}
              onToggleDark={() => setDarkMode(!darkMode)}
              onShortcuts={() => setShowShortcuts(true)}
              onChangePassword={() => setChangingPassword(true)}
              onSignOut={async () => { if (!(await confirm(billOpen ? 'A sales bill is open. Are you sure you want to log out?' : 'Are you sure you want to log out?'))) return; setSession(null); setAuthUser('', '', ''); setBillOpen(false); setActiveTab('Dashboard'); window.history.replaceState(null, '', '/'); }} />
          </div>
        </header>

        <main className="flex-1 overflow-auto p-4 pb-0 sm:p-6 sm:pb-0 bg-[#F5F1E9] dark:bg-[#0A0A0A]">
          <div key={activeTab} className={QUIET_SCREENS.includes(activeTab) ? undefined : "screen-enter"}>
            <ScreenBoundary onHome={() => openModule('Dashboard')}><Suspense fallback={<ScreenSkeleton />}>{renderContent()}</Suspense></ScreenBoundary>
          </div>
        </main>

        <footer className="flex h-9 shrink-0 items-center justify-between gap-4 border-t px-4 text-[11px] sm:px-6 print:hidden
                           bg-[#FFFCF7] border-[#E3D9CD] text-[#70675F]
                           dark:border-[#1E1E1E] dark:bg-[#0E0E0E] dark:text-stone-500">
         <div className="flex items-center gap-2 sm:gap-3">
            <span className="flex items-center gap-1.5 font-medium">
              <span className={`h-1.5 w-1.5 rounded-full ${dsr ? 'bg-[#55745A] dark:bg-emerald-500 dark:shadow-[0_0_6px_rgba(16,185,129,0.5)]' : 'bg-[#9A4B45]'}`} />
              {dsr ? 'Licensed' : 'Offline'}
            </span>
            <span className="text-[#A4998E] dark:text-stone-600">·</span>
            <span>Licence No. 827106637</span>
          </div>
          <div className="flex items-center gap-4 tabular-nums">
            {/* The desktop's status bar: today's saved bills (cancelled / deleted / held ones left out) and their value. */}
            <span className="font-semibold text-[#29231F] dark:text-stone-200">Total Bills: {totals?.total_bills ?? '—'}</span>
            <span className="font-semibold text-[#29231F] dark:text-stone-200">Sales: {totals ? Math.round(Number(totals.sales) || 0) : '—'}</span>
          </div>
        </footer>
      </div>
      <SnackbarContainer />
      <DesignerNotice />
      {changingPassword && !user.must_change_password && <ChangePasswordDialog user={user.user} onClose={() => setChangingPassword(false)} />}
      {user.must_change_password && <ChangePasswordDialog forced user={user.user}
        onClose={() => setSession(s => (s ? { ...s, user: { ...s.user, must_change_password: false } } : s))} />}
      {showCalc && <CalculatorDialog onClose={() => setShowCalc(false)} />}
      {showShortcuts && <KeyboardShortcutsModal onClose={() => setShowShortcuts(false)} />}
      {confirmDialog}
    </div>
  );
}
