import {
  BarChart2,
  Building,
  CreditCard,
  Database,
  FileText,
  LayoutDashboard,
  Monitor,
  Package,
  PackageMinus,
  RotateCcw,
  Scale,
  ShoppingCart,
  Tag,
  Truck,
  Users,
  ClipboardList,
  X,
  Banknote,
  Boxes,
  CalendarCheck,
  ClipboardCheck,
  Droplets,
  FolderTree,
  IndianRupee,
  ShieldCheck,
} from 'lucide-react';
import type { ReactNode } from 'react';
import { ADMIN_MASTER, MODULE_GROUPS } from './modules';

interface SidebarProps {
  isOpen: boolean;
  activeTab: string;
  onClose: () => void;
  onOpenModule: (moduleName: string) => void;
  user: string;
  canOpen: (moduleName: string) => boolean; // role rights from Admin Master
}

interface SidebarItemProps {
  icon: ReactNode;
  label: string;
  onClick: () => void;
  active: boolean;
}

const SidebarItem = ({ icon, label, onClick, active }: SidebarItemProps) => (
  <button
    onClick={onClick}
    className={`w-full flex items-center gap-2.5 px-3.5 py-2.5 rounded-full text-xs transition-colors ${
      active
        ? 'bg-[#e8702a]/15 text-[#b84716] dark:text-[#ffb07e] font-bold border-l-2 border-[#e8702a] shadow-[0_6px_18px_rgba(232,112,42,0.12)]'
        : 'text-stone-600 dark:text-stone-300 hover:bg-[#f1e3d8] dark:hover:bg-[#20253a] hover:text-stone-950 dark:hover:text-white'
    }`}
  >
    <span>{icon}</span>
    <span className="truncate">{label}</span>
  </button>
);

// Report entries are routed through ApiReport; names must match reports/reportDefs.ts.
const REPORT_ITEMS = ['Stock Ledger', 'Stock Ageing', 'Profit Margin', 'Hourly Sales', 'Daily Sales', 'Brand-wise Sales', 'Day-wise Litres', 'Stock in Litres', 'Stock Level / Reorder'];

export default function Sidebar({ isOpen, activeTab, onClose, onOpenModule, user, canOpen }: SidebarProps) {
  // Modules the user's role can't open are left out of the menu entirely.
  const item = (icon: ReactNode, name: string, label = name) => (canOpen(name)
    ? <SidebarItem key={name} icon={icon} label={label} onClick={() => onOpenModule(name)} active={activeTab === name} />
    : null);
  const heading = (text: string, names: string[]) => (names.some(canOpen)
    ? <div className="px-[11px] pb-1.5 pt-5 text-[10px] font-bold uppercase tracking-wider text-stone-400 dark:text-stone-500">{text}</div>
    : null);

  return (
    <>
      {isOpen && <div className="fixed inset-0 bg-black/50 z-30 md:hidden" onClick={onClose} />}

      <aside className={`fixed inset-y-0 left-0 z-40 w-[15.6rem] transform border-r border-[#e1c9b6] bg-linear-160 from-[#fffaf5] via-[#f1e3d8] via-70% to-[#ecd2c4] text-stone-800 shadow-2xl transition-transform duration-300 ease-in-out dark:border-[#343b59] dark:from-[#171b2b] dark:via-[#20253a] dark:to-[#3a2432] dark:text-white ${isOpen ? 'translate-x-0' : '-translate-x-full'} flex flex-col md:relative md:translate-x-0`}>
        <div className="flex shrink-0 items-center justify-between border-b border-[#e1c9b6] px-[17px] py-[19px] dark:border-[#343b59]">
          <div className="flex items-center gap-3">
            <div className="flex h-[38px] w-[38px] items-center justify-center rounded-full bg-linear-135 from-[#e8702a] to-[#e75b6f] shadow-[0_0_22px_rgba(232,112,42,0.3)] text-white font-bold text-sm">V</div>
            <div>
              <h1 className="text-sm font-bold tracking-wide text-stone-900 dark:text-white">Softvent vLQMS</h1>
              <div className="text-[10px] font-medium text-[#b84716] dark:text-[#ffb07e]">Cloud ERP Net 8.0</div>
            </div>
          </div>
          <button className="text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-white md:hidden" onClick={onClose}>
            <X size={20} />
          </button>
        </div>

        <nav className="flex-1 px-[13px] py-[15px] space-y-1 overflow-y-auto pb-20 custom-scrollbar text-xs">
          {item(<LayoutDashboard size={16} />, 'Dashboard')}
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
          {heading('Administration', [ADMIN_MASTER])}
          {item(<ShieldCheck size={16} />, ADMIN_MASTER)}
        </nav>

        <div className="shrink-0 space-y-1 border-t border-[#e1c9b6] bg-[#f1e3d8] p-3 text-[11px] text-stone-500 dark:border-[#343b59] dark:bg-[#20253a] dark:text-stone-400">
          <div className="flex items-center justify-between">
            <span>DB: vLQMSLHOUSE</span>
            <span className="flex items-center gap-1 text-[#b84716] dark:text-[#ffb07e]"><div className="h-1.5 w-1.5 rounded-full bg-[#e8702a]" /> Online</span>
          </div>
          <div>Login: {user} • Bangalore-92</div>
        </div>
      </aside>
    </>
  );
}
