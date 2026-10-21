// Left navigation, a collapsible tree laid out like the desktop's menu (Inventory > Masters > Item Related > Item Master,
// Transactions, Reports, Tools, Utilities ...). The tree itself is NAV_TREE in lib/modules.js; here each module gets its
// icon (ICONS) and is hidden when the user's role can't open it, and a branch with nothing to show is hidden too.
// A module that is in MODULE_GROUPS but not in NAV_TREE is listed under "More", so none is ever lost.
// Branches (each with its own icon and an arrow) open and close with a click, Enter/Space, or ←/→; ↑/↓ move through what is visible. The open branches are
// remembered (localStorage), and the branch holding the current screen opens when it changes.
// When the rail is collapsed (md and up) only the icons of the modules show, with the name as a tooltip.
// On small screens the sidebar slides in over the page.

import { useState } from 'react';
import VlqmsLogo from './VlqmsLogo';
import DevelopedBy from './DevelopedBy';
import {
  BarChart2, Building, CreditCard, Database, FileText, LayoutDashboard,
  Monitor, Package, PackageMinus, RotateCcw, Scale, ShoppingCart, Tag, Truck,
  Users, ClipboardList, X, Banknote, Boxes, CalendarCheck, ClipboardCheck,
  Droplets, FolderTree, IndianRupee, ShieldCheck, SquareTerminal, RefreshCcwDot, SlidersHorizontal, FileX2, Replace, GitMerge, GitCompare, History, Info, CloudUpload, UserCheck,
  PanelLeftClose, PanelLeftOpen, ChevronDown, ChevronRight, Warehouse, ArrowLeftRight, Wrench,
} from 'lucide-react';

import { useMenuHidden } from '../../hooks/useMenuHidden';
import { ADMIN_MASTER, MODULE_GROUPS, NAV_TREE } from '../../lib/modules';
import { isPlainClick, pathForModule } from '../../lib/routes';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import { prefetchScreen } from '../../lib/screenLoaders';

const ICONS = {
  'Dashboard': LayoutDashboard, 'About': Info, 'Company Master': Building, 'Item Master': Package, 'Price List Maintenance': IndianRupee,
  'Item Group Master': Database, 'UOM Master': Scale, 'Supplier Master': Truck, 'Customer Master': Users, 'Ledger Master': FileText,
  'Ledger Group Master': FolderTree, 'Sales Counter Master': Monitor, 'Sales Person Master': UserCheck, 'Package Master': Package, 'Brand Master': Tag,
  'Purchase Bill': ShoppingCart, 'Sales Invoice': FileText, 'Expense Entry': CreditCard, 'Stock Deduction': PackageMinus, 'Purchase Indent': ClipboardList,
  'Sales Return': RotateCcw, 'Closing Stock Entry': ClipboardCheck, 'Cash Denomination': Banknote, 'Daily Summary Report': CalendarCheck,
  'Stock Balance Report': BarChart2, 'SQL Query Execution': SquareTerminal, 'Screen Preview': Info, 'Re Write Stock': RefreshCcwDot, 'Global Settings': SlidersHorizontal,
  'Remove Duplicate Invoice': FileX2, 'Replace and Remove Duplicate items': Replace, 'Merge Items': GitMerge, 'Compare Stock': GitCompare,
  'ADMIN Audit Log': History, 'Web Sync': CloudUpload, 'Bulk Master Update': Database, 'Label Printing': Tag, [ADMIN_MASTER]: ShieldCheck,
};
// Icons of the branch headers (Inventory, Masters, Item Related ...).
const BRANCH_ICONS = {
  'Inventory': Warehouse, 'Masters': Database, 'Company Details': Building, 'Item Related': Package, 'Customer Related': Users,
  'Supplier Related': Truck, 'Others': FolderTree, 'Transactions': ArrowLeftRight, 'Reports': BarChart2, 'Tools': Wrench,
  'Utilities': SlidersHorizontal, 'More': FolderTree,
};
const iconFor = (name) => ICONS[name] ?? (name.includes('Litres') ? Droplets : name.startsWith('Stock') ? Boxes : /Report|Sales|Purchase|Loyalty|Excise|Profit|Ageing|Reports/.test(name) ? BarChart2 : FolderTree);

const STORE = 'vlqms_nav_open';
const DEFAULT_OPEN = ['Inventory', 'Inventory/Masters', 'Inventory/Transactions'];
const readOpen = () => {
  try { const saved = JSON.parse(localStorage.getItem(STORE) ?? 'null'); if (Array.isArray(saved)) return new Set(saved); } catch { /* use the default */ }
  return new Set(DEFAULT_OPEN);
};
const writeOpen = (set) => { try { localStorage.setItem(STORE, JSON.stringify([...set])); } catch { /* just not remembered */ } };

const leafName = (n) => (typeof n === 'string' ? n : n.name);
const isBranch = (n) => typeof n !== 'string' && Array.isArray(n.children);
// The branch keys ("Inventory", "Inventory/Masters", ...) that lead to a module.
function branchesOf(nodes, name, parent = '') {
  for (const n of nodes) {
    if (isBranch(n)) {
      const key = parent ? `${parent}/${n.label}` : n.label;
      const found = branchesOf(n.children, name, key);
      if (found) return [key, ...found];
    } else if (leafName(n) === name) return [];
  }
  return null;
}
const namesIn = (nodes) => nodes.flatMap(n => (isBranch(n) ? namesIn(n.children) : [leafName(n)]));

// A real link: a plain click switches screen in this tab; middle-click or Ctrl/Cmd-click opens it in a new tab.
const SidebarItem = ({ icon, label, module, href, onClick, active, collapsed, depth = 0 }) => (
  <a
    href={href}
    title={collapsed ? label : undefined}
    data-nav
    onClick={e => { if (isPlainClick(e)) { e.preventDefault(); onClick(); } }}
    onMouseEnter={() => prefetchScreen(module ?? label)}
    onFocus={() => prefetchScreen(module ?? label)}
    aria-current={active ? 'page' : undefined}
    style={collapsed ? undefined : { paddingLeft: `${8 + depth * 14}px` }}
    className={`flex w-full items-center gap-2 rounded-md py-1 pr-2 text-left text-[12px] leading-5 transition-colors ${collapsed ? 'md:justify-center md:px-0 md:py-2.5' : ''} ${
      active
        ? 'bg-[#1B2B4B] text-white font-semibold dark:bg-[#D4A847]/12 dark:text-[#E8C36A]'
        : 'text-[#5F6B80] hover:bg-[#E1E7F0] hover:text-[#1A2236] dark:text-stone-400 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-200'
    }`}
  >
    <span className={`shrink-0 ${active ? 'text-[#F0CF8A] dark:text-[#D4A847]' : 'text-[#7B879B] dark:text-stone-500'}`}>{icon}</span>
    <span className={`truncate ${collapsed ? 'md:sr-only' : ''}`}>{label}</span>
  </a>
);

export default function Sidebar({ isOpen, activeTab, onClose, onOpenModule, user, canOpen: mayOpen, collapsed = false, onToggleCollapse }) {
  // The desktop's menu (MstMenu) decides which screens are listed; the screen that is open always is.
  const menuHidden = useMenuHidden();
  const canOpen = (name) => mayOpen(name) && (name === activeTab || !menuHidden.has(String(name).toLowerCase()));
  const { company, isOnline } = useCompanyProfile();
  const [open, setOpenState] = useState(() => {
    const s = readOpen();
    branchesOf(NAV_TREE, activeTab)?.forEach(k => s.add(k));   // the branch of the screen already open
    return s;
  });
  // Opens the branch that holds the screen just opened (adjusting state while rendering, the React way for "when X changes").
  const [seenTab, setSeenTab] = useState(activeTab);
  if (seenTab !== activeTab) {
    setSeenTab(activeTab);
    const trail = branchesOf(NAV_TREE, activeTab);
    if (trail?.some(k => !open.has(k))) setOpenState(new Set([...open, ...trail]));
  }

  const tree = (() => {
    const placed = new Set(namesIn(NAV_TREE));
    const more = MODULE_GROUPS.flatMap(g => g.modules).filter(n => !placed.has(n) && n !== ADMIN_MASTER);
    return more.length ? [...NAV_TREE, { label: 'More', children: more }] : NAV_TREE;
  })();

  const toggle = (key, to) => {
    const next = new Set(open);
    if (to ?? !next.has(key)) next.add(key); else next.delete(key);
    writeOpen(next);
    setOpenState(next);
  };

  const leaf = (n, depth) => {
    const name = leafName(n);
    if (!canOpen(name)) return null;
    const Icon = iconFor(name);
    return <SidebarItem key={name} icon={<Icon size={collapsed ? 20 : 14} />} label={typeof n === 'string' ? name : n.label} module={name} href={pathForModule(name)}
      onClick={() => onOpenModule(name)} active={activeTab === name} collapsed={collapsed} depth={depth} />;
  };

  const branch = (n, depth, parent) => {
    const key = parent ? `${parent}/${n.label}` : n.label;
    const kids = n.children.map(c => (isBranch(c) ? branch(c, depth + 1, key) : leaf(c, depth + 1))).filter(Boolean);
    if (!kids.length) return null;
    const expanded = open.has(key);
    return (
      <div key={key} role="group" aria-label={n.label}>
        <button type="button" data-nav aria-expanded={expanded} onClick={() => toggle(key)}
          onKeyDown={e => {
            if (e.key === 'ArrowRight') { e.preventDefault(); toggle(key, true); }
            if (e.key === 'ArrowLeft') { e.preventDefault(); toggle(key, false); }
          }}
          style={{ paddingLeft: `${8 + depth * 14}px` }}
          className={`flex w-full items-center gap-2 rounded-md py-1 pr-2 text-left text-[12px] leading-5 hover:bg-[#E1E7F0] dark:hover:bg-[#1C1C1C] ${depth === 0 ? 'font-bold uppercase tracking-wide text-[#1A2236] dark:text-stone-100' : 'font-semibold text-[#3D352F] dark:text-stone-200'}`}>
          {(() => { const Icon = BRANCH_ICONS[n.label] ?? FolderTree; return <Icon size={15} className="shrink-0 text-[#1B2B4B] dark:text-[#D4A847]" aria-hidden="true" />; })()}
          <span className="min-w-0 flex-1 truncate">{n.label}</span>
          {expanded ? <ChevronDown size={13} className="shrink-0 text-[#7B879B]" aria-hidden="true" /> : <ChevronRight size={13} className="shrink-0 text-[#7B879B]" aria-hidden="true" />}
        </button>
        {expanded && <div>{kids}</div>}
      </div>
    );
  };

  // Collapsed rail: only the main sections (Dashboard, Masters, Transactions, Reports, Tools, Utilities, Admin, About), one icon each.
  // A section's icon opens the full menu on that section; a single screen (Dashboard, About, Admin) opens as usual.
  const SECTION_ICONS = { Masters: Database, Transactions: FileText, Reports: BarChart2, Tools: SquareTerminal, Utilities: SlidersHorizontal, More: FolderTree };
  const sections = tree.flatMap(n => (isBranch(n) && n.label === 'Inventory' ? n.children.map(c => ({ node: c, key: `Inventory/${c.label}` })) : [{ node: n, key: isBranch(n) ? n.label : '' }]));
  const flat = () => sections.map(({ node, key }) => {
    if (!isBranch(node)) return leaf(node, 0);
    if (!namesIn(node.children).some(canOpen)) return null;
    const Icon = SECTION_ICONS[node.label] ?? FolderTree;
    return (
      <button key={key} type="button" title={node.label} aria-label={node.label}
        onClick={() => { const next = new Set(open); (branchesOf(NAV_TREE, namesIn(node.children)[0]) ?? [key]).forEach(k => next.add(k)); writeOpen(next); setOpenState(next); onToggleCollapse?.(); }}
        className="flex w-full items-center justify-center rounded-md py-2.5 text-[#7B879B] hover:bg-[#E1E7F0] hover:text-[#1A2236] dark:text-stone-500 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-200">
        <Icon size={21} />
      </button>
    );
  });

  // ↑/↓ move between the visible links and branch buttons.
  const navKeys = (e) => {
    if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return;
    const all = [...e.currentTarget.querySelectorAll('[data-nav]')];
    const at = all.indexOf(document.activeElement);
    if (at === -1) return;
    e.preventDefault();
    all[Math.max(0, Math.min(all.length - 1, at + (e.key === 'ArrowDown' ? 1 : -1)))]?.focus();
  };

  return (
    <>
      {isOpen && <div className="fixed inset-0 z-30 bg-overlay md:hidden print:hidden" onClick={onClose} />}

      <aside aria-label="Main navigation" className={`fixed inset-y-0 left-0 z-40 flex w-56 transform flex-col
        border-r bg-[#F0EBE2] text-[#1A2236] border-[#E2D8CC]
        dark:border-[#1E1E1E] dark:bg-[#0E0E0E] dark:text-stone-100
        transition-transform duration-250 ease-out ${isOpen ? 'translate-x-0 shadow-pop' : '-translate-x-full'}
        ${collapsed ? 'md:w-[76px]' : ''}
        md:relative md:translate-x-0 md:shadow-none print:hidden`}>

        {/* Brand */}
        <div className={`flex h-14 shrink-0 items-center justify-between border-b border-[#E2D8CC] dark:border-[#1E1E1E] px-4 ${collapsed ? 'md:justify-center md:px-0' : ''}`}>
          <a href={pathForModule('Dashboard')} title="Go to Dashboard" aria-label="Go to Dashboard"
            onClick={e => { if (isPlainClick(e)) { e.preventDefault(); onOpenModule('Dashboard'); onClose?.(); } }}
            className={`flex items-center gap-2.5 rounded-md p-[4px] ${collapsed ? 'md:hidden' : ''}`}>
            <VlqmsLogo size="sm" textOnly />
          </a>
          {onToggleCollapse && (
            <button
              className="hidden rounded-lg p-1.5 text-[#7B879B] hover:bg-[#E1E7F0] hover:text-[#1A2236] md:block dark:text-stone-500 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-50"
              onClick={onToggleCollapse}
              aria-label={collapsed ? 'Expand navigation' : 'Collapse navigation'}
              aria-expanded={!collapsed}
              title={`${collapsed ? 'Expand' : 'Collapse'} navigation (Ctrl+B)`}
            >
              {collapsed ? <PanelLeftOpen size={18} /> : <PanelLeftClose size={18} />}
            </button>
          )}
          <button className="rounded-lg p-1.5 text-[#7B879B] hover:bg-[#E1E7F0] hover:text-[#1A2236] md:hidden dark:text-stone-500 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-50" onClick={onClose} aria-label="Close navigation">
            <X size={18} />
          </button>
        </div>

        <nav onKeyDown={navKeys} className={`flex-1 space-y-px overflow-y-auto px-2 py-2 text-xs ${collapsed ? 'md:space-y-1 md:px-2.5' : ''}`}>
          {collapsed
            ? <>
                <div className="md:hidden">{tree.map(n => (isBranch(n) ? branch(n, 0) : leaf(n, 0)))}</div>
                <div className="hidden md:block">{flat()}</div>
              </>
            : tree.map(n => (isBranch(n) ? branch(n, 0) : leaf(n, 0)))}
        </nav>

        <div className={`shrink-0 border-t border-[#E2D8CC] dark:border-[#1E1E1E] px-4 py-3 text-[11px] text-[#5F6B80] dark:text-stone-500 ${collapsed ? 'md:hidden' : ''}`}>
          <div className="flex items-center justify-between">
            <span className="truncate">{company?.name || 'SpiritPOS'}</span>
            <span className={`flex items-center gap-1.5 ${isOnline ? 'text-emerald-700 dark:text-emerald-400' : 'text-[#9A4B45] dark:text-red-400'}`}>
              <span className={`h-1.5 w-1.5 rounded-full ${isOnline ? 'bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.8)]' : 'bg-[#9A4B45]'}`} />
              {isOnline === null ? 'Checking' : isOnline ? 'Online' : 'Offline'}
            </span>
          </div>
          <div className="mt-0.5 truncate">Signed in as <span className="font-medium text-[#1B2B4B] dark:text-[#D4A847]">{user}</span></div>
          <DevelopedBy className="mt-3 border-t border-[#E2D8CC] pt-3 dark:border-[#1E1E1E]" />
        </div>
      </aside>
    </>
  );
}
