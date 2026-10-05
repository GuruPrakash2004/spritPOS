// Left navigation. Lists every module explicitly (one item(...) line each, grouped by heading) and hides
// the ones the user's role can't open. Reports from reportDefs are listed via REPORT_ITEMS, which must be
// updated by hand when a report is added. On small screens it slides in over the page.

import VlqmsLogo from './VlqmsLogo';
import DevelopedBy from './DevelopedBy';
import {
  BarChart2, Building, CreditCard, Database, FileText, LayoutDashboard,
  Monitor, Package, PackageMinus, RotateCcw, Scale, ShoppingCart, Tag, Truck,
  Users, ClipboardList, X, Banknote, Boxes, CalendarCheck, ClipboardCheck,
  Droplets, FolderTree, IndianRupee, ShieldCheck, SquareTerminal, RefreshCcwDot, SlidersHorizontal, FileX2, Replace, GitMerge, GitCompare, History, Info, CloudUpload, UserCheck,
} from 'lucide-react';
import type { ReactNode } from 'react';
import { ADMIN_MASTER, MODULE_GROUPS } from '../../lib/modules';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';

interface SidebarProps {
  isOpen: boolean;
  activeTab: string;
  onClose: () => void;
  onOpenModule: (moduleName: string) => void;
  user: string;
  canOpen: (moduleName: string) => boolean;
}

const SidebarItem = ({ icon, label, onClick, active }: { icon: ReactNode; label: string; onClick: () => void; active: boolean }) => (
  <button
    onClick={onClick}
    aria-current={active ? 'page' : undefined}
    className={`flex w-full items-center gap-2.5 rounded-lg px-2.5 py-2 text-left text-[13px] transition-all duration-200 ${
      active
        ? 'bg-[#64283A] text-white font-semibold shadow-[0_4px_12px_rgba(100,40,58,0.14)] dark:bg-[#D4A847]/12 dark:text-[#E8C36A] dark:shadow-[0_0_12px_rgba(212,168,71,0.06)]'
        : 'text-[#70675F] hover:bg-[#E8DFD2] hover:text-[#29231F] dark:text-stone-400 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-200'
    }`}
  >
    <span className={`shrink-0 transition-colors ${
      active
        ? 'text-[#D3BB89] dark:text-[#D4A847]'
        : 'text-[#8E847A] dark:text-stone-500'
    }`}>{icon}</span>
    <span className="truncate">{label}</span>
  </button>
);

const REPORT_ITEMS = ['Stock Ledger', 'Stock Ageing', 'Profit Margin', 'Hourly Sales', 'Daily Sales', 'Brand-wise Sales', 'Day-wise Litres', 'Stock in Litres', 'Stock Level / Reorder'];

export default function Sidebar({ isOpen, activeTab, onClose, onOpenModule, user, canOpen }: SidebarProps) {
  const { company, isOnline } = useCompanyProfile();
  const item = (icon: ReactNode, name: string, label = name) => (canOpen(name)
    ? <SidebarItem key={name} icon={icon} label={label} onClick={() => onOpenModule(name)} active={activeTab === name} />
    : null);
  const heading = (text: string, names: string[]) => (names.some(canOpen)
    ? <div className="px-2.5 pb-1 pt-5 text-[10px] font-bold tracking-[0.15em] text-[#B08A45] dark:text-stone-500 uppercase">{text}</div>
    : null);

  return (
    <>
      {isOpen && <div className="fixed inset-0 z-30 bg-overlay md:hidden print:hidden" onClick={onClose} />}

      <aside aria-label="Main navigation" className={`fixed inset-y-0 left-0 z-40 flex w-60 transform flex-col
        border-r bg-[#F0EBE2] text-[#29231F] border-[#E2D8CC]
        dark:border-[#1E1E1E] dark:bg-[#0E0E0E] dark:text-stone-100
        transition-transform duration-200 ease-out ${isOpen ? 'translate-x-0 shadow-pop' : '-translate-x-full'}
        md:relative md:translate-x-0 md:shadow-none print:hidden`}>

        {/* Brand */}
        <div className="flex h-16 shrink-0 items-center justify-between border-b border-[#E2D8CC] dark:border-[#1E1E1E] px-4">
          <div className="flex items-center gap-2.5 p-[4px]">
            <VlqmsLogo size="sm" textOnly />
          </div>
          <button className="rounded-lg p-1.5 text-[#8E847A] hover:bg-[#E8DFD2] hover:text-[#29231F] md:hidden dark:text-stone-500 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-50" onClick={onClose} aria-label="Close navigation">
            <X size={18} />
          </button>
        </div>

        <nav className="flex-1 space-y-0.5 overflow-y-auto px-3 py-3 text-xs">
          {item(<LayoutDashboard size={16} />, 'Dashboard')}
          {item(<Info size={16} />, 'About')}
          {heading('Masters', MODULE_GROUPS.find(g => g.group === 'Masters')!.modules)}
          {item(<Building size={16} />, 'Company Master')}
          {item(<Package size={16} />, 'Item Master')}
          {item(<IndianRupee size={16} />, 'Price List Maintenance')}
          {item(<Database size={16} />, 'Item Group Master')}
          {item(<Scale size={16} />, 'UOM Master')}
          {item(<Truck size={16} />, 'Supplier Master')}
          {item(<Users size={16} />, 'Customer Master')}
          {item(<FileText size={16} />, 'Ledger Master')}
          {item(<FolderTree size={16} />, 'Ledger Group Master')}
          {item(<Monitor size={16} />, 'Sales Counter Master')}
          {item(<UserCheck size={16} />, 'Sales Person Master')}
          {item(<Package size={16} />, 'Package Master')}
          {item(<Tag size={16} />, 'Brand Master')}
          {heading('Transactions', MODULE_GROUPS.find(g => g.group === 'Transactions')!.modules)}
          {item(<ShoppingCart size={16} />, 'Purchase Bill')}
          {item(<FileText size={16} />, 'Sales Invoice')}
          {item(<CreditCard size={16} />, 'Expense Entry')}
          {item(<PackageMinus size={16} />, 'Stock Deduction')}
          {item(<ClipboardList size={16} />, 'Purchase Indent')}
          {item(<RotateCcw size={16} />, 'Sales Return')}
          {item(<ClipboardCheck size={16} />, 'Closing Stock Entry')}
          {item(<Banknote size={16} />, 'Cash Denomination')}
          {heading('Reports', MODULE_GROUPS.find(g => g.group === 'Reports')!.modules)}
          {item(<CalendarCheck size={16} />, 'Daily Summary Report')}
          {item(<BarChart2 size={16} />, 'Stock Balance Report')}
          {REPORT_ITEMS.map(name => item(name.includes('Litres') ? <Droplets size={16} /> : name.startsWith('Stock') ? <Boxes size={16} /> : <BarChart2 size={16} />, name))}
          {item(<BarChart2 size={16} />, 'Reports Dashboard', 'Customised Reports')}
          {heading('Tools', MODULE_GROUPS.find(g => g.group === 'Tools')!.modules)}
          {item(<SquareTerminal size={16} />, 'SQL Query Execution')}
          {item(<RefreshCcwDot size={16} />, 'Re Write Stock')}
          {heading('Utilities', MODULE_GROUPS.find(g => g.group === 'Utilities')!.modules)}
          {item(<SlidersHorizontal size={16} />, 'Global Settings')}
          {item(<FileX2 size={16} />, 'Remove Duplicate Invoice')}
          {item(<Replace size={16} />, 'Replace and Remove Duplicate items')}
          {item(<GitMerge size={16} />, 'Merge Items')}
          {item(<GitCompare size={16} />, 'Compare Stock')}
          {item(<History size={16} />, 'ADMIN Audit Log')}
          {item(<CloudUpload size={16} />, 'Web Sync')}
          {heading('Administration', [ADMIN_MASTER])}
          {item(<ShieldCheck size={16} />, ADMIN_MASTER, 'Admin Master (Security)')}
        </nav>

        <div className="shrink-0 border-t border-[#E2D8CC] dark:border-[#1E1E1E] px-4 py-3 text-[11px] text-[#70675F] dark:text-stone-500">
          <div className="flex items-center justify-between">
            <span className="truncate">{company?.name || 'e-vLQMS ERP'}</span>
            <span className={`flex items-center gap-1.5 ${isOnline ? 'text-emerald-700 dark:text-emerald-400' : 'text-[#9A4B45] dark:text-red-400'}`}>
              <span className={`h-1.5 w-1.5 rounded-full ${isOnline ? 'bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.8)]' : 'bg-[#9A4B45]'}`} />
              {isOnline === null ? 'Checking' : isOnline ? 'Online' : 'Offline'}
            </span>
          </div>
          <div className="mt-0.5 truncate">Signed in as <span className="font-medium text-[#64283A] dark:text-[#D4A847]">{user}</span></div>
          <DevelopedBy className="mt-3 border-t border-[#E2D8CC] pt-3 dark:border-[#1E1E1E]" />
        </div>
      </aside>
    </>
  );
}
