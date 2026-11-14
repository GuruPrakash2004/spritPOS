






 

// Each entry is one legacy report screen (frmRpt*), served by backend/app/routers/reports.py.
export const REPORTS = {
  'Stock Ledger': {
    title: 'Stock Ledger', subtitle: 'Purchases, sales and adjustments for one item, with running balance.',
    endpoint: '/reports/stock-ledger', filters: ['item', 'range'],
    columns: [
      { key: 'date', label: 'Date' }, { key: 'particulars', label: 'Particulars' },
      { key: 'in', label: 'In', kind: 'number', total: true }, { key: 'out', label: 'Out', kind: 'number', total: true },
      { key: 'balance', label: 'Balance', kind: 'number' },
    ],
  },
  'Stock Ageing': {
    title: 'Stock Ageing', subtitle: 'Stock on hand by months since the item was last received.',
    endpoint: '/reports/stock-ageing', filters: ['on', 'months', 'ageing'],
    columns: [
      { key: 'item_code', label: 'Code' }, { key: 'item_name', label: 'Item' }, { key: 'last_purchase', label: 'Last Received' },
      { key: 'age_days', label: 'Age (days)', kind: 'number' }, { key: 'bucket', label: 'Bucket' },
      { key: 'qoh', label: 'QOH', kind: 'number', total: true }, { key: 'value', label: 'Value (₹)', kind: 'money', total: true },
    ],
  },
  'Profit Margin': {
    title: 'Profit Margin', subtitle: 'Sales value against purchase value per item.',
    endpoint: '/reports/profit-margin', filters: ['range'],
    columns: [
      { key: 'item_code', label: 'Code' }, { key: 'item_name', label: 'Item' },
      { key: 'sales_qty', label: 'Sales Qty', kind: 'number', total: true }, { key: 'purchase_rate', label: 'Pur. Rate', kind: 'money' },
      { key: 'purchase_value', label: 'Purchase Value', kind: 'money', total: true }, { key: 'sales_value', label: 'Sales Value', kind: 'money', total: true },
      { key: 'profit', label: 'Profit', kind: 'money', total: true },
    ],
  },
  'Hourly Sales': {
    title: 'Hourly Sales', subtitle: 'Bills and sales value by hour of the day.',
    endpoint: '/reports/sales/hourly', filters: ['on'],
    columns: [
      { key: 'hour', label: 'Hour' }, { key: 'bills', label: 'Bills', kind: 'number', total: true },
      { key: 'qty', label: 'Qty', kind: 'number', total: true }, { key: 'amount', label: 'Amount (₹)', kind: 'money', total: true },
    ],
  },
  'Daily Sales': {
    title: 'Daily Sales', subtitle: 'Bills, bottles, litres and value per day.',
    endpoint: '/reports/sales/daily', filters: ['range'],
    columns: [
      { key: 'date', label: 'Date' }, { key: 'bills', label: 'Bills', kind: 'number', total: true },
      { key: 'qty', label: 'Qty', kind: 'number', total: true }, { key: 'litres', label: 'Litres', kind: 'number', total: true },
      { key: 'amount', label: 'Amount (₹)', kind: 'money', total: true },
    ],
  },
  'Package-wise Sales': {
    title: 'Package-wise Sales', subtitle: 'Sales by bottle size, most litres first.',
    endpoint: '/reports/sales/package-wise', filters: ['range'],
    columns: [
      { key: 'package', label: 'Package' }, { key: 'qty', label: 'Bottles', kind: 'number', total: true },
      { key: 'litres', label: 'Litres', kind: 'number', total: true }, { key: 'amount', label: 'Amount (₹)', kind: 'money', total: true },
    ],
  },
  'Package-wise Purchase': {
    title: 'Package-wise Purchase', subtitle: 'Purchases (less purchase returns) by bottle size.',
    endpoint: '/reports/purchase/package-wise', filters: ['range'],
    columns: [
      { key: 'package', label: 'Package' }, { key: 'qty', label: 'Bottles', kind: 'number', total: true },
      { key: 'litres', label: 'Litres', kind: 'number', total: true }, { key: 'amount', label: 'Amount (₹)', kind: 'money', total: true },
    ],
  },
  'Loyalty Report': {
    title: 'Loyalty Report', subtitle: 'Points earned and redeemed in the period, and each customer\'s balance now.',
    endpoint: '/reports/loyalty', filters: ['range'],
    columns: [
      { key: 'customer', label: 'Customer' }, { key: 'name', label: 'Name' }, { key: 'mobile', label: 'Mobile' }, { key: 'card_no', label: 'Card No' },
      { key: 'earned', label: 'Earned', kind: 'number', total: true }, { key: 'redeemed', label: 'Redeemed', kind: 'number', total: true },
      { key: 'balance', label: 'Balance', kind: 'number', total: true },
    ],
  },
  'Loyalty Detail': {
    title: 'Loyalty Detail', subtitle: 'Every saved bill in the period that added or redeemed loyalty points.',
    endpoint: '/reports/loyalty/detail', filters: ['range'],
    columns: [
      { key: 'customer', label: 'Customer' }, { key: 'name', label: 'Name' }, { key: 'mobile', label: 'Phone No' }, { key: 'card_no', label: 'Card No' },
      { key: 'doc_date', label: 'Doc Date' }, { key: 'doc_type', label: 'Doc Type' }, { key: 'doc_ref', label: 'Doc Ref' },
      { key: 'added', label: 'Points Added', kind: 'number', total: true }, { key: 'redeemed', label: 'Redeemed', kind: 'number', total: true },
    ],
  },
  'Item Rate History': {
    title: 'Item Rate History', subtitle: 'Every MRP and purchase-rate change in the period, from the shop\'s own rate-change log (who, when, from which PC).',
    endpoint: '/reports/item-rate-history', filters: ['range', 'item'],
    columns: [
      { key: 'changed_on', label: 'Changed On' }, { key: 'item_code', label: 'Item Code' }, { key: 'item_name', label: 'Item' }, { key: 'source', label: 'Source' },
      { key: 'old_mrp', label: 'Old MRP', kind: 'money' }, { key: 'new_mrp', label: 'New MRP', kind: 'money' },
      { key: 'old_pur_rate', label: 'Old Pur Rate', kind: 'money' }, { key: 'new_pur_rate', label: 'New Pur Rate', kind: 'money' },
      { key: 'changed_by', label: 'By' }, { key: 'system', label: 'PC' },
    ],
  },
  'Audit Trail': {
    title: 'Audit Trail', subtitle: 'Who added, changed or deleted which document, and when (the desktop\'s audit log of actions). Administrator only.',
    endpoint: '/reports/audit-trail', filters: ['range'],
    columns: [
      { key: 'at', label: 'When' }, { key: 'document', label: 'Document' }, { key: 'doc_no', label: 'Doc No' }, { key: 'action', label: 'Action' },
      { key: 'user', label: 'User' }, { key: 'system', label: 'PC' }, { key: 'ip', label: 'IP Address' },
    ],
  },
  'Excise Stock': {
    title: 'Excise Stock', subtitle: 'Closing stock in bottles and litres by liquor type and bottle size.',
    endpoint: '/reports/excise-stock', filters: ['on'],
    columns: [
      { key: 'liqtype', label: 'Liquor Type' }, { key: 'package', label: 'Package' },
      { key: 'qty', label: 'Bottles', kind: 'number', total: true }, { key: 'litres', label: 'Litres', kind: 'number', total: true },
    ],
  },
  'Drops Sales Summary': {
    title: 'Drops - Sales Summary', subtitle: 'One day: liquor, each other group, round off, payment modes and total sales.',
    endpoint: '/reports/drops/sales-summary', filters: ['on'],
    columns: [
      { key: 'particulars', label: 'Group' }, { key: 'qty', label: 'Qty', kind: 'number' },
      { key: 'amount', label: 'Amount (₹)', kind: 'money' },
    ],
  },
  'Drops Consolidated': {
    title: 'Drops - Consolidated Report', subtitle: 'Sales, petty cash, purchase and group-wise stock value for the period (stock at purchase rate).',
    endpoint: '/reports/drops/consolidated', filters: ['range'],
    columns: [
      { key: 'a', label: 'Particulars' }, { key: 'b', label: 'Qty / Expense / Supplier' },
      { key: 'c', label: 'Amount / Receipt / Opening' }, { key: 'd', label: 'Amount / Closing' },
    ],
  },
  'Drops Purchase': {
    title: 'Drops - Purchase', subtitle: 'One day: each purchase bill (supplier invoice no. - date) and the total.',
    endpoint: '/reports/drops/purchase', filters: ['on'],
    columns: [
      { key: 'group', label: 'Group' }, { key: 'supplier', label: 'Supplier' }, { key: 'bill_no', label: 'Bill No' },
      { key: 'amount', label: 'Amount (₹)', kind: 'money' },
    ],
  },
  'Drops Petty Cash': {
    title: 'Drops - Petty Cash', subtitle: 'One day: opening balance, expense by ledger, closing balance.',
    endpoint: '/reports/drops/petty-cash', filters: ['on'],
    columns: [
      { key: 'particulars', label: 'Particulars' }, { key: 'expense', label: 'Expense (₹)', kind: 'money' },
      { key: 'receipt', label: 'Receipt / Balance (₹)', kind: 'money' },
    ],
  },
  'Brand-wise Sales': {
    // frmRptBrandWise: Purchase / Sale / Purchase Vs Sale per item with brand totals; the older /sales/brand-wise summary stays on the server.
    title: 'Brand Wise Report', subtitle: 'Purchase and sales per brand and item, with brand totals.',
    endpoint: '/reports/brand-wise', filters: ['range', 'brandwise'],
    columns: [
      { key: 'brand', label: 'Brand' }, { key: 'group', label: 'ItemGroup' }, { key: 'item_code', label: 'ItemCode' },
      { key: 'item_name', label: 'ItemName' }, { key: 'uom', label: 'Uom' },
      { key: 'pur_qty', label: 'Purchase Qty', kind: 'number', side: 'pur' },
      { key: 'pur_rate', label: 'Purchase Rate', kind: 'money', side: 'pur', valued: true },
      { key: 'pur_amt', label: 'Purchase Value', kind: 'money', side: 'pur', valued: true },
      { key: 'sal_qty', label: 'Sales Qty', kind: 'number', side: 'sal' },
      { key: 'sal_rate', label: 'Sales Rate', kind: 'money', side: 'sal', valued: true },
      { key: 'sal_amt', label: 'Sales Value', kind: 'money', side: 'sal', valued: true },
      { key: 'cases', label: 'No of Case', kind: 'number', cases: true },
      { key: 'bottles', label: 'No of Bottles', kind: 'number', cases: true },
    ],
  },
  'Day-wise Litres': {
    title: 'Day-wise Litres Sold', subtitle: 'Litres sold per day by liquor type, for excise returns.',
    endpoint: '/reports/litres/day', filters: ['range'],
    columns: [
      { key: 'date', label: 'Date' }, { key: 'IML', label: 'IML (L)', kind: 'number', total: true },
      { key: 'Beer', label: 'Beer (L)', kind: 'number', total: true }, { key: 'Wine', label: 'Wine (L)', kind: 'number', total: true },
      { key: 'Others', label: 'Others (L)', kind: 'number', total: true }, { key: 'total', label: 'Total (L)', kind: 'number', total: true },
    ],
  },
  'Stock in Litres': {
    title: 'Stock in Litres', subtitle: 'Closing stock converted to litres using Package Master.',
    endpoint: '/reports/litres/stock', filters: ['on'],
    columns: [
      { key: 'item_code', label: 'Code' }, { key: 'item_name', label: 'Item' }, { key: 'package', label: 'Pkg' },
      { key: 'liqtype', label: 'LIQTYPE' }, { key: 'cl_qty', label: 'Closing Qty', kind: 'number', total: true },
      { key: 'cl_litres', label: 'Litres', kind: 'number', total: true },
    ],
  },
  // frmRptStkLevel: Below Min Level / Below Re-Order Level / Above Max Level, from the stock list as on the date (the view is chosen with the 'level' filter).
  'Stock Level / Reorder': {
    title: 'Stock Level', subtitle: 'Items below their Min Level or Re-Order Level, or above their Max Level, as on the date.',
    endpoint: '/reports/stock-level', filters: ['on', 'level', 'stocklevel'],
    columns: [
      { key: 'group', label: 'Item Group Name' }, { key: 'item_name', label: 'Item Name' }, { key: 'level', label: 'Level', kind: 'number' },
      { key: 'closing_qty', label: 'Closing Quantity', kind: 'number', total: true, ltr: false }, { key: 'closing_ltr', label: 'Closing Quantity-Ltr', kind: 'number', total: true, ltr: true },
    ],
  },
};
