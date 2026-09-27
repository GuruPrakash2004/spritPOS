export interface MatrixScreen {
  id: string;
  name: string;
  group: 'GENERAL' | 'MASTERS' | 'TRANSACTIONS' | 'REPORTS' | 'ADMINISTRATION';
  sub: string;
  hasOpen: boolean;
  hasAdd: boolean;
  hasEdit: boolean;
  hasDelete: boolean;
}

export const MATRIX_SCREENS: MatrixScreen[] = [
  // General
  { id: 'Dashboard', name: 'Dashboard', group: 'GENERAL', sub: "Today's sales, bills and cash at a glance", hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },

  // Masters
  { id: 'Company Master', name: 'Company Master', group: 'MASTERS', sub: 'Store name, licence, address, financial year', hasOpen: true, hasAdd: false, hasEdit: true, hasDelete: false },
  { id: 'Item Master', name: 'Item Master', group: 'MASTERS', sub: 'Products: code, MRP, bottles per case; CSV import', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Price List Maintenance', name: 'Price List Maintenance', group: 'MASTERS', sub: 'Change MRP or purchase rate for many items at once', hasOpen: true, hasAdd: false, hasEdit: true, hasDelete: false },
  { id: 'Item Group Master', name: 'Item Group Master', group: 'MASTERS', sub: 'Categories (IML, Beer…), liquor and loyalty flags', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'UOM Master', name: 'UOM Master', group: 'MASTERS', sub: 'Units of measure', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Supplier Master', name: 'Supplier Master', group: 'MASTERS', sub: 'KSBCL and other suppliers, GST details', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Customer Master', name: 'Customer Master', group: 'MASTERS', sub: 'Customers, loyalty cards and points', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Ledger Master', name: 'Ledger Master', group: 'MASTERS', sub: 'Account heads used by Expense Entry', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Ledger Group Master', name: 'Ledger Group Master', group: 'MASTERS', sub: 'Groups for ledgers (expenses, cash, bank…)', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Sales Counter Master', name: 'Sales Counter Master', group: 'MASTERS', sub: 'POS counter terminals and bill prefixes', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Package Master', name: 'Package Master', group: 'MASTERS', sub: 'Bottle sizes and litres per bottle for excise totals', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Brand Master', name: 'Brand Master', group: 'MASTERS', sub: 'Distillery brands used in sales reports', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },

  // Transactions
  { id: 'Purchase Bill', name: 'Purchase Bill', group: 'TRANSACTIONS', sub: 'Supplier goods receipt note (GRN) and invoice intake', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Sales Invoice', name: 'Sales Invoice', group: 'TRANSACTIONS', sub: 'Counter POS billing, receipts and payment settlements', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Expense Entry', name: 'Expense Entry', group: 'TRANSACTIONS', sub: 'Voucher postings against ledger expense heads', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Stock Deduction', name: 'Stock Deduction', group: 'TRANSACTIONS', sub: 'Damages, breakages, leakages and wastage logging', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Purchase Indent', name: 'Purchase Indent', group: 'TRANSACTIONS', sub: 'Depot procurement purchase orders', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Sales Return', name: 'Sales Return', group: 'TRANSACTIONS', sub: 'Customer bill returns, credit notes and refunds', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Closing Stock Entry', name: 'Closing Stock Entry', group: 'TRANSACTIONS', sub: 'Physical day-end inventory counts and verification', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: false },
  { id: 'Cash Denomination', name: 'Cash Denomination', group: 'TRANSACTIONS', sub: 'End-of-day drawer note and coin tally', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: false },

  // Reports
  { id: 'Daily Summary Report', name: 'Daily Summary Report', group: 'REPORTS', sub: 'Day-end audit: bills, payments, excise litres, profit', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock Balance Report', name: 'Stock Balance Report', group: 'REPORTS', sub: 'Current quantity on hand (QOH) and closing valuation', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock Ledger', name: 'Stock Ledger', group: 'REPORTS', sub: 'Item movement history: receipts, sales, balance', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock Ageing', name: 'Stock Ageing', group: 'REPORTS', sub: 'Aging analysis by last purchase receipt date', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Profit Margin', name: 'Profit Margin', group: 'REPORTS', sub: 'Realized gross margin per item and category', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Hourly Sales', name: 'Hourly Sales', group: 'REPORTS', sub: 'Counter bill traffic and peak sales volume', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Daily Sales', name: 'Daily Sales', group: 'REPORTS', sub: 'Daily revenue trend across financial periods', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Brand-wise Sales', name: 'Brand-wise Sales', group: 'REPORTS', sub: 'Excise sales volume breakdown by liquor brand', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Day-wise Litres', name: 'Day-wise Litres', group: 'REPORTS', sub: 'Official Karnataka excise litres audit log', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock in Litres', name: 'Stock in Litres', group: 'REPORTS', sub: 'Warehouse inventory stock expressed in bulk litres', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock Level / Reorder', name: 'Stock Level / Reorder', group: 'REPORTS', sub: 'Reorder levels (ROL) and minimum order quantities', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Reports Dashboard', name: 'Reports Dashboard', group: 'REPORTS', sub: 'Customised MIS reports across inventory and ledgers', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },

  // Administration
  { id: 'Admin Master', name: 'Admin Master', group: 'ADMINISTRATION', sub: 'Users, roles, structured item permissions and passwords', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
];

export const GROUPS = ['GENERAL', 'MASTERS', 'TRANSACTIONS', 'REPORTS', 'ADMINISTRATION'] as const;

export interface RoleDraft {
  name: string;
  modules: Set<string>;
  actions: Set<string>;
}

export function parseRoleToDraft(r: { name: string; modules: string[] }): RoleDraft {
  const isAll = r.modules.includes('*');
  const isCashier = r.name === 'Cashier';

  const nextMods = new Set<string>();
  const nextActs = new Set<string>();

  MATRIX_SCREENS.forEach(s => {
    if (isAll || r.modules.includes(s.id)) {
      nextMods.add(s.id);
      if (isCashier) {
        if (s.id === 'Item Master') {
          nextActs.add(`${s.id}:add`);
          nextActs.add(`${s.id}:edit`);
          // NO delete for cashier!
        } else {
          if (s.hasAdd) nextActs.add(`${s.id}:add`);
          if (s.hasEdit) nextActs.add(`${s.id}:edit`);
        }
      } else {
        if (s.hasAdd) nextActs.add(`${s.id}:add`);
        if (s.hasEdit) nextActs.add(`${s.id}:edit`);
        if (s.hasDelete) nextActs.add(`${s.id}:delete`);
      }
    }
  });

  return { name: r.name, modules: nextMods, actions: nextActs };
}
