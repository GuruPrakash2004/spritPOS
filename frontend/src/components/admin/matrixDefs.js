import { SIMPLE_MASTERS } from '../../lib/simpleMasters';
















export const MATRIX_SCREENS = [
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
  { id: 'Sales Person Master', name: 'Sales Person Master', group: 'MASTERS', sub: 'Sales persons chosen on each bill', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Ledger Group Master', name: 'Ledger Group Master', group: 'MASTERS', sub: 'Groups for ledgers (expenses, cash, bank…)', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Sales Counter Master', name: 'Sales Counter Master', group: 'MASTERS', sub: 'POS counter terminals and bill prefixes', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Package Master', name: 'Package Master', group: 'MASTERS', sub: 'Bottle sizes and litres per bottle for excise totals', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  { id: 'Brand Master', name: 'Brand Master', group: 'MASTERS', sub: 'Distillery brands used in sales reports', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
  ...SIMPLE_MASTERS.map(m => ({ id: m.title, name: m.title, group: 'MASTERS' , sub: m.subtitle, hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true })),

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
  { id: 'Package-wise Sales', name: 'Package-wise Sales', group: 'REPORTS', sub: 'Sales by bottle size', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Package-wise Purchase', name: 'Package-wise Purchase', group: 'REPORTS', sub: 'Purchases by bottle size', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Loyalty Report', name: 'Loyalty Report', group: 'REPORTS', sub: 'Loyalty points earned, redeemed and balance', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Excise Stock', name: 'Excise Stock', group: 'REPORTS', sub: 'Closing stock in litres by liquor type and size', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Brand-wise Sales', name: 'Brand-wise Sales', group: 'REPORTS', sub: 'Excise sales volume breakdown by liquor brand', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Day-wise Litres', name: 'Day-wise Litres', group: 'REPORTS', sub: 'Official Karnataka excise litres audit log', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock in Litres', name: 'Stock in Litres', group: 'REPORTS', sub: 'Warehouse inventory stock expressed in bulk litres', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Stock Level / Reorder', name: 'Stock Level / Reorder', group: 'REPORTS', sub: 'Reorder levels (ROL) and minimum order quantities', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Reports Dashboard', name: 'Reports Dashboard', group: 'REPORTS', sub: 'Customised MIS reports across inventory and ledgers', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },

  // Tools (SQL Query Execution is Administrator-only, so it has no tick here)
  { id: 'Re Write Stock', name: 'Re Write Stock', group: 'TOOLS', sub: 'Check stock against documents and rebuild QOH', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },

  // Utilities (Global Settings and the ADMIN Audit Log are Administrator-only, so they have no tick here)
  { id: 'Remove Duplicate Invoice', name: 'Remove Duplicate Invoice', group: 'UTILITIES', sub: 'Remove bills saved twice (stock and points go back)', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Replace and Remove Duplicate items', name: 'Replace and Remove Duplicate items', group: 'UTILITIES', sub: 'Replace an item created twice with the right one', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Merge Items', name: 'Merge Items', group: 'UTILITIES', sub: 'Fold one item into another (bills, stock, QOH)', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Bulk Master Update', name: 'Bulk Master Update', group: 'UTILITIES', sub: 'Set one Item Master field on many items at once', hasOpen: true, hasAdd: false, hasEdit: true, hasDelete: false },
  { id: 'Label Printing', name: 'Label Printing', group: 'UTILITIES', sub: 'Item barcode labels', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Web Sync', name: 'Web Sync', group: 'UTILITIES', sub: "Send the day's summary and stock to the central site", hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },
  { id: 'Compare Stock', name: 'Compare Stock', group: 'UTILITIES', sub: 'Compare two stock sheets item by item', hasOpen: true, hasAdd: false, hasEdit: false, hasDelete: false },

  // Administration
  { id: 'Admin Master', name: 'Admin Master', group: 'ADMINISTRATION', sub: 'Users, roles, structured item permissions and passwords', hasOpen: true, hasAdd: true, hasEdit: true, hasDelete: true },
];

export const GROUPS = ['GENERAL', 'MASTERS', 'TRANSACTIONS', 'REPORTS', 'TOOLS', 'UTILITIES', 'ADMINISTRATION'] ;







export function parseRoleToDraft(r) {
  const isAll = r.modules.includes('*');
  const rights = r.rights ?? {};
  const nextMods = new Set();
  const nextActs = new Set();

  // A screen's Add/Edit/Delete tick is on unless the role's saved rights switch it off.
  MATRIX_SCREENS.forEach(s => {
    if (!(isAll || r.modules.includes(s.id))) return;
    nextMods.add(s.id);
    if (s.hasAdd && rights[s.id]?.add !== false) nextActs.add(`${s.id}:add`);
    if (s.hasEdit && rights[s.id]?.edit !== false) nextActs.add(`${s.id}:edit`);
    if (s.hasDelete && rights[s.id]?.delete !== false) nextActs.add(`${s.id}:delete`);
  });

  return { name: r.name, modules: nextMods, actions: nextActs };
}

// The draft's ticks as the backend's `rights` (only for opened screens; actions a screen doesn't
// have count as allowed).
export function draftRights(draft) {
  const out = {};
  MATRIX_SCREENS.forEach(s => {
    if (!draft.modules.has(s.id)) return;
    out[s.id] = {
      add: !s.hasAdd || draft.actions.has(`${s.id}:add`),
      edit: !s.hasEdit || draft.actions.has(`${s.id}:edit`),
      delete: !s.hasDelete || draft.actions.has(`${s.id}:delete`),
    };
  });
  return out;
}

// Tick changes on a role draft (Admin Master (Security) → Roles & rights).
// Ticking Open gives the screen's actions (Cashiers don't get Delete by default); unticking it removes them.
export function toggleOpenDraft(draft, screenId) {
  const modules = new Set(draft.modules);
  const actions = new Set(draft.actions);
  if (modules.has(screenId)) {
    modules.delete(screenId);
    ['add', 'edit', 'delete'].forEach(a => actions.delete(`${screenId}:${a}`));
  } else {
    modules.add(screenId);
    const s = MATRIX_SCREENS.find(x => x.id === screenId);
    if (s?.hasAdd) actions.add(`${screenId}:add`);
    if (s?.hasEdit) actions.add(`${screenId}:edit`);
    if (s?.hasDelete && draft.name !== 'Cashier') actions.add(`${screenId}:delete`);
  }
  return { ...draft, modules, actions };
}

export function toggleActionDraft(draft, screenId, action) {
  const key = `${screenId}:${action}`;
  const modules = new Set(draft.modules);
  const actions = new Set(draft.actions);
  if (actions.has(key)) actions.delete(key);
  else { actions.add(key); modules.add(screenId); }   // an action needs the screen open
  return { ...draft, modules, actions };
}

export function toggleGroupDraft(draft, group, col) {
  const screens = MATRIX_SCREENS.filter(s => s.group === group);
  const modules = new Set(draft.modules);
  const actions = new Set(draft.actions);
  if (col === 'open') {
    const allOpen = screens.every(s => modules.has(s.id));
    screens.forEach(s => {
      if (allOpen) {
        modules.delete(s.id);
        ['add', 'edit', 'delete'].forEach(a => actions.delete(`${s.id}:${a}`));
      } else {
        modules.add(s.id);
        if (s.hasAdd) actions.add(`${s.id}:add`);
        if (s.hasEdit) actions.add(`${s.id}:edit`);
        if (s.hasDelete && draft.name !== 'Cashier') actions.add(`${s.id}:delete`);
      }
    });
  } else {
    const applicable = screens.filter(s => (col === 'add' ? s.hasAdd : col === 'edit' ? s.hasEdit : s.hasDelete));
    const allOn = applicable.every(s => actions.has(`${s.id}:${col}`));
    applicable.forEach(s => {
      if (allOn) actions.delete(`${s.id}:${col}`);
      else { actions.add(`${s.id}:${col}`); modules.add(s.id); }
    });
  }
  return { ...draft, modules, actions };
}
