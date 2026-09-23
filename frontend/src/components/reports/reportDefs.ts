import type { ReportColumn, ReportFilter } from './ReportShell';

export type ReportDef = { title: string; subtitle: string; endpoint: string; filters: ReportFilter[]; columns: ReportColumn[] };

// Each entry is one legacy report screen (frmRpt*), served by backend/app/routers/reports.py.
export const REPORTS: Record<string, ReportDef> = {
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
    endpoint: '/reports/stock-ageing', filters: ['on', 'months'],
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
  'Brand-wise Sales': {
    title: 'Brand-wise Sales', subtitle: 'Sales by brand, highest first.',
    endpoint: '/reports/sales/brand-wise', filters: ['range'],
    columns: [
      { key: 'brand', label: 'Brand' }, { key: 'qty', label: 'Qty', kind: 'number', total: true },
      { key: 'litres', label: 'Litres', kind: 'number', total: true }, { key: 'amount', label: 'Amount (₹)', kind: 'money', total: true },
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
  'Stock Level / Reorder': {
    title: 'Stock Level / Reorder', subtitle: 'Items at or below their reorder level, with the suggested reorder quantity.',
    endpoint: '/reports/reorder', filters: [],
    columns: [
      { key: 'item_code', label: 'Code' }, { key: 'item_name', label: 'Item' },
      { key: 'qoh', label: 'QOH', kind: 'number' }, { key: 'rol', label: 'ROL', kind: 'number' }, { key: 'roq', label: 'Suggested ROQ', kind: 'number', total: true },
    ],
  },
};
