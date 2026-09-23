// Every module a role can be given, grouped as in the sidebar. Names are the routing keys used by
// App.tsx and Sidebar.tsx, and are what the backend stores in a role's rights.
export const MODULE_GROUPS: { group: string; modules: string[] }[] = [
  { group: 'General', modules: ['Dashboard'] },
  {
    group: 'Masters',
    modules: [
      'Company Master', 'Item Master', 'Price List Maintenance', 'Item Group Master', 'UOM Master', 'Supplier Master',
      'Customer Master', 'Ledger Master', 'Ledger Group Master', 'Sales Counter Master', 'Package Master', 'Brand Master',
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
    ],
  },
];

export const ADMIN_MASTER = 'Admin Master';
export const ADMIN_PATH = '/adminMaster';
export const ADMIN_ROLE = 'Administrator';

// "*" (the Administrator role) grants everything; Admin Master itself is Administrator-only.
export const canOpen = (modules: string[], role: string, name: string) =>
  name === ADMIN_MASTER ? role === ADMIN_ROLE : modules.includes('*') || modules.includes(name);

// Extra search words per module: legacy abbreviations and everyday terms, so "SINV", "breakage" or
// "reorder" finds the right screen.
export const MODULE_KEYWORDS: Record<string, string> = {
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
  'Brand-wise Sales': 'brand sales',
  'Day-wise Litres': 'litres liters excise day',
  'Stock in Litres': 'litres liters excise stock',
  'Stock Level / Reorder': 'reorder rol roq low stock',
  'Reports Dashboard': 'customised custom reports',
  'Admin Master': 'admin users roles rights permissions password security',
};
