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
} from 'lucide-react';
import type { ReactNode } from 'react';

interface SidebarProps {
  isOpen: boolean;
  activeTab: string;
  onClose: () => void;
  onOpenModule: (moduleName: string) => void;
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

export default function Sidebar({ isOpen, activeTab, onClose, onOpenModule }: SidebarProps) {
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
          <SidebarItem icon={<LayoutDashboard size={16} />} label="Dashboard" onClick={() => onOpenModule('Dashboard')} active={activeTab === 'Dashboard'} />
          <div className="px-[11px] pb-1.5 pt-5 text-[10px] font-bold uppercase tracking-wider text-stone-400 dark:text-stone-500">Masters</div>
          <SidebarItem icon={<Building size={16} />} label="Company Master" onClick={() => onOpenModule('Company Master')} active={activeTab === 'Company Master'} />
          <SidebarItem icon={<Package size={16} />} label="Item Master" onClick={() => onOpenModule('Item Master')} active={activeTab === 'Item Master'} />
          <SidebarItem icon={<Database size={16} />} label="Item Group Master" onClick={() => onOpenModule('Item Group Master')} active={activeTab === 'Item Group Master'} />
          <SidebarItem icon={<Scale size={16} />} label="UOM Master" onClick={() => onOpenModule('UOM Master')} active={activeTab === 'UOM Master'} />
          <SidebarItem icon={<Truck size={16} />} label="Supplier Master" onClick={() => onOpenModule('Supplier Master')} active={activeTab === 'Supplier Master'} />
          <SidebarItem icon={<Users size={16} />} label="Customer Master" onClick={() => onOpenModule('Customer Master')} active={activeTab === 'Customer Master'} />
          <SidebarItem icon={<FileText size={16} />} label="Ledger Master" onClick={() => onOpenModule('Ledger Master')} active={activeTab === 'Ledger Master'} />
          <SidebarItem icon={<Monitor size={16} />} label="Sales Counter Master" onClick={() => onOpenModule('Sales Counter Master')} active={activeTab === 'Sales Counter Master'} />
          <SidebarItem icon={<Package size={16} />} label="Package Master" onClick={() => onOpenModule('Package Master')} active={activeTab === 'Package Master'} />
          <SidebarItem icon={<Tag size={16} />} label="Brand Master" onClick={() => onOpenModule('Brand Master')} active={activeTab === 'Brand Master'} />
          <div className="px-[11px] pb-1.5 pt-5 text-[10px] font-bold uppercase tracking-wider text-stone-400 dark:text-stone-500">Transactions</div>
          <SidebarItem icon={<ShoppingCart size={16} />} label="Purchase Bill" onClick={() => onOpenModule('Purchase Bill')} active={activeTab === 'Purchase Bill'} />
          <SidebarItem icon={<FileText size={16} />} label="Sales Invoice" onClick={() => onOpenModule('Sales Invoice')} active={activeTab === 'Sales Invoice'} />
          <SidebarItem icon={<CreditCard size={16} />} label="Expense Entry" onClick={() => onOpenModule('Expense Entry')} active={activeTab === 'Expense Entry'} />
          <SidebarItem icon={<PackageMinus size={16} />} label="Stock Deduction" onClick={() => onOpenModule('Stock Deduction')} active={activeTab === 'Stock Deduction'} />
          <SidebarItem icon={<ClipboardList size={16} />} label="Purchase Indent" onClick={() => onOpenModule('Purchase Indent')} active={activeTab === 'Purchase Indent'} />
          <SidebarItem icon={<RotateCcw size={16} />} label="Sales Return" onClick={() => onOpenModule('Sales Return')} active={activeTab === 'Sales Return'} />
          <div className="px-[11px] pb-1.5 pt-5 text-[10px] font-bold uppercase tracking-wider text-stone-400 dark:text-stone-500">Reports</div>
          <SidebarItem icon={<BarChart2 size={16} />} label="Stock Balance Report" onClick={() => onOpenModule('Stock Balance Report')} active={activeTab === 'Stock Balance Report'} />
          <SidebarItem icon={<BarChart2 size={16} />} label="Customised Reports" onClick={() => onOpenModule('Reports Dashboard')} active={activeTab === 'Reports Dashboard'} />
        </nav>

        <div className="shrink-0 space-y-1 border-t border-[#e1c9b6] bg-[#f1e3d8] p-3 text-[11px] text-stone-500 dark:border-[#343b59] dark:bg-[#20253a] dark:text-stone-400">
          <div className="flex items-center justify-between">
            <span>DB: vLQMSLHOUSE</span>
            <span className="flex items-center gap-1 text-[#b84716] dark:text-[#ffb07e]"><div className="h-1.5 w-1.5 rounded-full bg-[#e8702a]" /> Online</span>
          </div>
          <div>Login: ADMIN • Bangalore-92</div>
        </div>
      </aside>
    </>
  );
}
