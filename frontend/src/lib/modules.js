// Module registry: the list of every screen ("module"), grouped as in the sidebar.
//
// A module name is a key used in four places, so it must be spelled identically everywhere:
// App.jsx (switch), components/layout/Sidebar.jsx, components/admin/matrixDefs.js and
// backend/app/store.py ROLES (a role's rights are a list of these names; "*" means all).
// MODULE_KEYWORDS adds search words (legacy abbreviations such as "SINV", "DSR") for ModuleSearch.

import { SIMPLE_MASTERS, SIMPLE_TITLES } from './simpleMasters';

export const MODULE_GROUPS = [
  { group: 'General', modules: ['Dashboard', 'About'] },
  {
    group: 'Masters',
    modules: [
      'Company Master', 'Item Master', 'Price List Maintenance', 'Item Group Master', 'UOM Master', 'Supplier Master',
      'Customer Master', 'Ledger Master', 'Ledger Group Master', 'Sales Counter Master', 'Sales Person Master', 'Package Master', 'Brand Master',
      ...SIMPLE_TITLES,
    ],
  },
  {
    group: 'Transactions',
    modules: [
      'Purchase Bill', 'Sales Invoice', 'Expense Entry', 'Stock Deduction', 'Purchase Indent', 'Sales Return',
      'Closing Stock Entry', 'Cash Denomination',
    ],
  },
  {
    group: 'Reports',
    modules: [
      'Daily Summary Report', 'Stock Balance Report', 'Stock Ledger', 'Stock Ageing', 'Profit Margin', 'Hourly Sales',
      'Daily Sales', 'Brand-wise Sales', 'Day-wise Litres', 'Stock in Litres', 'Stock Level / Reorder', 'Reports Dashboard',
      'Package-wise Sales', 'Package-wise Purchase', 'Loyalty Report', 'Excise Stock',
    ],
  },
  // The desktop app's TOOLS menu (MstMenu type 6), with its captions. Download Latest Exe is left out on purpose.
  { group: 'Tools', modules: ['SQL Query Execution', 'Re Write Stock', 'Screen Preview'] },
  // The desktop app's UTILITIES menu (MstMenu type 7), with its captions; screens are added as they are built.
  // Its Security item is Admin Master, shown as "Admin Master (Security)" under Administration.
  { group: 'Utilities', modules: ['Global Settings', 'Remove Duplicate Invoice', 'Replace and Remove Duplicate items', 'Merge Items', 'Compare Stock', 'ADMIN Audit Log', 'Web Sync', 'Bulk Master Update', 'Label Printing'] },
];

// The sidebar's tree, laid out like the desktop's menu (INVENTORY > MASTERS > ITEM RELATED > Item Master, TRANSACTIONS, REPORTS,
// TOOLS, UTILITIES ...). A string is a module name; { name, label } shows another caption; { label, children } is a branch.
// MODULE_GROUPS above stays the flat list (routes, rights, search); a module missing here still shows, under "More"
// (Sidebar.jsx), so a new module is never lost from the menu.
export const NAV_TREE = [
  'Dashboard',
  {
    label: 'Inventory',
    children: [
      {
        label: 'Masters',
        children: [
          { label: 'Company Details', children: ['Company Master'] },
          { label: 'Item Related', children: ['Item Master', 'Item Group Master', 'UOM Master', 'Price List Maintenance', 'Package Master', 'Brand Master'] },
          { label: 'Customer Related', children: ['Customer Master', 'Customer Group Master'] },
          { label: 'Supplier Related', children: ['Supplier Master', 'Supplier Group Master'] },
          {
            label: 'Others',
            children: ['Ledger Master', 'Ledger Group Master', 'Sales Counter Master', 'Sales Person Master', 'State Master', 'Currency Master',
              'Location Master', 'Tax Master', 'Bank Master', 'Holiday Master'],
          },
        ],
      },
      {
        label: 'Transactions',
        children: ['Sales Invoice', 'Purchase Bill', 'Expense Entry', 'Stock Deduction', 'Purchase Indent', 'Sales Return', 'Closing Stock Entry', 'Cash Denomination'],
      },
      {
        label: 'Reports',
        children: ['Daily Summary Report', 'Stock Balance Report', 'Stock Ledger', 'Stock Ageing', 'Profit Margin', 'Hourly Sales', 'Daily Sales', 'Brand-wise Sales',
          'Day-wise Litres', 'Stock in Litres', 'Stock Level / Reorder', 'Package-wise Sales', 'Package-wise Purchase', 'Loyalty Report', 'Excise Stock',
          { name: 'Reports Dashboard', label: 'Customised Reports' }],
      },
    ],
  },
  { label: 'Tools', children: ['SQL Query Execution', 'Re Write Stock', 'Screen Preview'] },
  {
    label: 'Utilities',
    children: ['Global Settings', 'Remove Duplicate Invoice', 'Replace and Remove Duplicate items', 'Merge Items', 'Compare Stock', 'ADMIN Audit Log', 'Web Sync',
      'Bulk Master Update', 'Label Printing'],
  },
  { name: 'Admin Master', label: 'Admin Master (Security)' },
  'About',
];

export const ADMIN_MASTER = 'Admin Master';
export const ADMIN_PATH = '/adminMaster';
export const ADMIN_ROLE = 'Administrator';
// Screens only an Administrator may open, whatever the role's module list says (the server checks too).
export const ADMIN_ONLY = new Set([ADMIN_MASTER, 'SQL Query Execution', 'Screen Preview', 'Global Settings', 'ADMIN Audit Log']);

// Screens every signed-in user may open, whatever their role.
export const ALWAYS_OPEN = new Set(['About']);

// "*" (the Administrator role) grants everything; Admin Master itself is Administrator-only.
export const canOpen = (modules, role, name) =>
  ALWAYS_OPEN.has(name) || (ADMIN_ONLY.has(name) ? role === ADMIN_ROLE : modules.includes('*') || modules.includes(name));

// Extra search words per module: legacy abbreviations and everyday terms, so "SINV", "breakage" or
// "reorder" finds the right screen.
export const MODULE_KEYWORDS = {
  ...Object.fromEntries(SIMPLE_MASTERS.map(m => [m.title, m.keywords])),
  'Dashboard': 'home kpi today summary',
  'Company Master': 'company licence fy financial year backup',
  'Item Master': 'item product brand mrp bottle import excel ksbcl code',
  'Price List Maintenance': 'price rate mrp update bulk',
  'Item Group Master': 'group category loyalty liquor',
  'UOM Master': 'unit uom measure',
  'Supplier Master': 'supplier vendor distributor ksbcl gst',
  'Customer Master': 'customer party loyalty card points sms mobile',
  'Ledger Master': 'ledger account expense head',
  'Ledger Group Master': 'ledger group account',
  'Sales Person Master': 'salesman salesperson staff sp',
  'Sales Counter Master': 'counter pos terminal prefix',
  'Package Master': 'package bottle size litres ml',
  'Brand Master': 'brand',
  'Purchase Bill': 'pb purchase bill grn receipt supplier invoice stock in copy from',
  'Sales Invoice': 'sinv sale bill pos billing counter cash card upi hold reprint cancel',
  'Expense Entry': 'expense payment cash paid',
  'Stock Deduction': 'deduction breakage leakage damage sample wastage',
  'Purchase Indent': 'indent order ksbcl po',
  'Sales Return': 'return refund',
  'Closing Stock Entry': 'cse closing stock physical count day end',
  'Cash Denomination': 'cash count denomination notes drawer day end',
  'Daily Summary Report': 'dsr daily summary day report',
  'Stock Balance Report': 'stock balance qoh closing',
  'Stock Ledger': 'stock ledger item movement history',
  'Stock Ageing': 'ageing aging old stock',
  'Profit Margin': 'profit margin',
  'Hourly Sales': 'hourly sales hour',
  'Daily Sales': 'daily sales day',
  'Bulk Master Update': 'bulk update change many items hsn brand mrp rol roq liqtype',
  'Label Printing': 'label barcode print item code sticker',
  'Brand-wise Sales': 'brand sales',
  'Package-wise Sales': 'package size bottle ml sales litres',
  'Package-wise Purchase': 'package size bottle ml purchase litres',
  'Loyalty Report': 'loyalty points earned redeemed customer card',
  'Excise Stock': 'excise stock litres liqtype statement',
  'Day-wise Litres': 'litres liters excise day',
  'Stock in Litres': 'litres liters excise stock',
  'Stock Level / Reorder': 'reorder rol roq low stock',
  'Reports Dashboard': 'customised custom reports',
  'Admin Master': '700 admin users user roles rights assign rights permissions password security',
  // Legacy menu shortcut codes (600, 604) find the tools as they did in the desktop app.
  'SQL Query Execution': '600 sql query select table view procedure database tools',
  'Screen Preview': 'screen preview doc designer fields layout database columns check problems tools',
  'Re Write Stock': '604 rewrite re-write stock qoh mismatch ledger repair tools',
  // Utilities, with the desktop menu codes (702, 711-714).
  'Remove Duplicate Invoice': '711 remove duplicate invoice bill delete twice double utilities',
  'Replace and Remove Duplicate items': '712 replace remove duplicate item items merge utilities',
  'Merge Items': '713 merge items item combine duplicate utilities',
  'Compare Stock': '714 compare stock sheet excel difference mismatch closing utilities',
  'Global Settings': '702 ctrl+g global settings setting configuration freeze date salesperson loyalty points utilities print settings printer preview direct email prompt after save other settings transaction settings doc prefix document number menu sequence shortcut',
  'ADMIN Audit Log': 'admin audit log history changes who changed edited deleted trail utilities',
  'Web Sync': 'web sync central head office owner upload push dsr summary branches shops utilities',
  'About': 'about version softvent help',
};
