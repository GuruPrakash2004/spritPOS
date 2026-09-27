// Sample documents for print previews, used when a screen opens the print dialog without real data.

import type { ApiCompany, ApiInvoice, ApiFullDsr } from '../../lib/api';
import type { AnalyticalReportData } from './layouts/AnalyticalReportLayout';
import type { DocumentCategory } from './rptRegistry';

export const SAMPLE_COMPANY: ApiCompany = {
  name: 'THE LIQUOR HOUSE',
  address: 'No. 42, Brigade Road, Civil Station, Bangalore - 560001, Karnataka',
  phone: '+91 98450 12345',
  mobile: '+91 98450 67890',
  email: 'accounts@theliquorhouse.in',
  fax: '+91 80 2558 0000',
  website: 'www.theliquorhouse.in',
  licence_type: 'CL-2 (Retail Vend of Liquor)',
  gstin: '29AAAAA0000A1Z5',
  pan: 'AAAAA0000A',
  cin: 'U15510KA2020PTC123456',
  excise_phone: '+91 80 2221 0000',
  ksbcl_code: 'RET-BLR-0842',
  state: 'Karnataka',
  fy_start: '2026-04-01',
  fy_end: '2027-03-31',
};

export const SAMPLE_INVOICE_DATA: ApiInvoice = {
  doc_no: '26/c10-0083718',
  date: new Date().toISOString().slice(0, 10),
  time: '14:32:10',
  counter: 'Counter 1 (Main Cash)',
  customer: 'Retail Cash Customer',
  salesperson: 'Prakash R.',
  status: 'SAVED',
  lines: [
    { item_code: 'BEER01', item_name: 'Kingfisher Premium Lager Beer 650ml', cases: 0, bottles: 2, qty: 2, rate: 180, amount: 360, litres: 1.3 },
    { item_code: 'WKY02', item_name: 'Royal Challenge Select Premium Whisky 750ml', cases: 0, bottles: 1, qty: 1, rate: 850, amount: 850, litres: 0.75 },
    { item_code: 'RUM03', item_name: 'Old Monk XXX Very Old Vatted Rum 750ml', cases: 0, bottles: 1, qty: 1, rate: 540, amount: 540, litres: 0.75 },
    { item_code: 'WKY04', item_name: 'Signature Rare Aged Grain Whisky 750ml', cases: 0, bottles: 1, qty: 1, rate: 920, amount: 920, litres: 0.75 },
  ],
  totals: {
    qty: 5,
    litres: 3.55,
    basic: 2262.71,
    cgst: 203.64,
    sgst: 203.65,
    discount: 0,
    points_redeemed: 0,
    points_earned: 26,
    net: 2670,
  },
  payment: { cash: 2670, card: 0, upi: 0, credit: 0 },
};

export const SAMPLE_PURCHASE_BILL_DATA: ApiInvoice = {
  doc_no: 'PB-2026-00412',
  date: new Date().toISOString().slice(0, 10),
  time: '11:15:00',
  counter: 'Main Godown',
  customer: 'KSBCL Depot #14 (Bangalore West)',
  salesperson: 'Store Manager',
  status: 'SAVED',
  lines: [
    { item_code: 'BEER01', item_name: 'Kingfisher Premium Lager Beer 650ml', cases: 10, bottles: 120, qty: 120, rate: 145, amount: 17400, litres: 78.0 },
    { item_code: 'WKY02', item_name: 'Royal Challenge Select Premium Whisky 750ml', cases: 5, bottles: 60, qty: 60, rate: 680, amount: 40800, litres: 45.0 },
    { item_code: 'RUM03', item_name: 'Old Monk XXX Very Old Vatted Rum 750ml', cases: 5, bottles: 60, qty: 60, rate: 430, amount: 25800, litres: 45.0 },
  ],
  totals: {
    qty: 240,
    litres: 168.0,
    basic: 84000,
    cgst: 0,
    sgst: 0,
    discount: 0,
    points_redeemed: 0,
    points_earned: 0,
    net: 84000,
  },
  payment: { cash: 0, card: 0, upi: 0, credit: 84000 },
};

export const SAMPLE_PURCHASE_INDENT_DATA: ApiInvoice = {
  doc_no: 'IND-KSBCL-2026-089',
  date: new Date().toISOString().slice(0, 10),
  time: '10:00:00',
  counter: 'Indent Desk',
  customer: 'KSBCL Wholesale Depot',
  salesperson: 'Purchase Dept',
  status: 'SAVED',
  lines: [
    { item_code: 'BEER01', item_name: 'Kingfisher Premium Lager Beer 650ml', cases: 25, bottles: 300, qty: 300, rate: 145, amount: 43500, litres: 195.0 },
    { item_code: 'WKY01', item_name: "McDowell's No.1 Reserve Whisky 750ml", cases: 15, bottles: 180, qty: 180, rate: 490, amount: 88200, litres: 135.0 },
    { item_code: 'WKY02', item_name: 'Royal Challenge Select Premium Whisky 750ml', cases: 10, bottles: 120, qty: 120, rate: 680, amount: 81600, litres: 90.0 },
  ],
  totals: {
    qty: 600,
    litres: 420.0,
    basic: 213300,
    cgst: 0,
    sgst: 0,
    discount: 0,
    points_redeemed: 0,
    points_earned: 0,
    net: 213300,
  },
  payment: { cash: 0, card: 0, upi: 0, credit: 213300 },
};

export const SAMPLE_STOCK_DEDUCTION_DATA: ApiInvoice = {
  doc_no: 'STKDED-2026-0034',
  date: new Date().toISOString().slice(0, 10),
  time: '18:45:00',
  counter: 'Godown / Rack 4',
  customer: 'Transit Breakage & Wastage Loss',
  salesperson: 'Supervisor',
  status: 'SAVED',
  lines: [
    { item_code: 'BEER01', item_name: 'Kingfisher Premium Lager Beer 650ml (Bottle Breakage)', cases: 0, bottles: 3, qty: 3, rate: 180, amount: 540, litres: 1.95 },
    { item_code: 'WKY01', item_name: "McDowell's No.1 Reserve Whisky 750ml (Seal Damaged)", cases: 0, bottles: 1, qty: 1, rate: 620, amount: 620, litres: 0.75 },
  ],
  totals: {
    qty: 4,
    litres: 2.7,
    basic: 1160,
    cgst: 0,
    sgst: 0,
    discount: 0,
    points_redeemed: 0,
    points_earned: 0,
    net: 1160,
  },
  payment: { cash: 0, card: 0, upi: 0, credit: 0 },
};

export const SAMPLE_DSR_DATA: ApiFullDsr = {
  date: new Date().toISOString().slice(0, 10),
  total_bills: 54,
  sales_incl_gst: 198450,
  liquor_basic: 168000,
  non_liquor_basic: 0,
  profit_of_the_day: 39690,
  opening_stock: { qty: 1428, value: 856800 },
  closing_stock: { qty: 1374, litres: 1030.5, value: 824400 },
  purchase: { basic: 0, incl_gst: 0 },
  sales_returns: 0,
  expenses: 1450,
  cash_balance: 111000,
  redemption: 0,
  payment_modes: {
    Cash: 112450,
    'Card / POS': 58000,
    'UPI / QR': 28000,
  },
  counter_wise: {
    'Counter 1 (Main Cash)': 124800,
    'Counter 2 (Express)': 73650,
  },
  salesperson_wise: {
    'Prakash R.': 124800,
    'Suresh K.': 73650,
  },
  group_wise_sales: {
    Beer: 48600,
    Whisky: 112400,
    Rum: 24500,
    Scotch: 12950,
  },
  expense_ledger_wise: {
    'Tea & Refreshment': 450,
    'Packing & Bags': 1000,
  },
  cancelled_invoices: ['26/c10-0083692', '26/c10-0083705'],
};

export const SAMPLE_STOCK_BALANCE_DATA = [
  { group: 'Beer', brand: 'United Breweries', name: 'Kingfisher Premium Lager Beer', pkg: '650ml', opQty: 120, recQty: 24, issQty: 18, clQty: 126, rate: 180, value: 22680 },
  { group: 'Beer', brand: 'United Breweries', name: 'Kingfisher Strong Premium Beer', pkg: '650ml', opQty: 96, recQty: 24, issQty: 30, clQty: 90, rate: 190, value: 17100 },
  { group: 'Whisky', brand: 'Diageo / USL', name: 'Royal Challenge Select Premium Whisky', pkg: '750ml', opQty: 85, recQty: 12, issQty: 9, clQty: 88, rate: 850, value: 74800 },
  { group: 'Whisky', brand: 'Diageo / USL', name: "McDowell's No.1 Reserve Whisky", pkg: '750ml', opQty: 110, recQty: 36, issQty: 28, clQty: 118, rate: 620, value: 73160 },
  { group: 'Whisky', brand: 'Pernod Ricard', name: 'Blenders Pride Rare Premium Whisky', pkg: '750ml', opQty: 60, recQty: 12, issQty: 8, clQty: 64, rate: 980, value: 62720 },
  { group: 'Rum', brand: 'Mohan Meakin', name: 'Old Monk XXX Very Old Vatted Rum', pkg: '750ml', opQty: 94, recQty: 0, issQty: 14, clQty: 80, rate: 540, value: 43200 },
  { group: 'Vodka', brand: 'Radico Khaitan', name: 'Magic Moments Pure Grain Vodka', pkg: '750ml', opQty: 40, recQty: 12, issQty: 6, clQty: 46, rate: 680, value: 31280 },
  { group: 'Scotch', brand: 'Diageo', name: 'Johnnie Walker Red Label Blended Scotch', pkg: '750ml', opQty: 24, recQty: 6, issQty: 3, clQty: 27, rate: 2150, value: 58050 },
];

export const SAMPLE_GENERIC_REPORT_DATA: AnalyticalReportData = {
  title: 'Daily Sales & Excise MIS Register',
  subtitle: 'CL-2 Retail Operations • Softvent vLQMS Audit Report',
  columns: [
    { key: 'doc_no', label: 'Bill No', kind: 'text' },
    { key: 'time', label: 'Time', kind: 'text' },
    { key: 'counter', label: 'Counter', kind: 'text' },
    { key: 'mode', label: 'Payment Mode', kind: 'text' },
    { key: 'qty', label: 'Qty (Btl)', kind: 'number', total: true },
    { key: 'litres', label: 'Litres', kind: 'number', total: true },
    { key: 'amount', label: 'Net Amount (₹)', kind: 'money', total: true },
  ],
  rows: [
    { doc_no: '26/c10-0083715', time: '13:45', counter: 'Counter 1', mode: 'Cash', qty: 3, litres: 2.1, amount: 1650 },
    { doc_no: '26/c10-0083716', time: '14:02', counter: 'Counter 2', mode: 'UPI', qty: 2, litres: 1.3, amount: 360 },
    { doc_no: '26/c10-0083717', time: '14:15', counter: 'Counter 1', mode: 'Card', qty: 1, litres: 0.75, amount: 850 },
    { doc_no: '26/c10-0083718', time: '14:32', counter: 'Counter 1', mode: 'Cash', qty: 5, litres: 3.55, amount: 2670 },
    { doc_no: '26/c10-0083719', time: '14:40', counter: 'Counter 2', mode: 'UPI', qty: 4, litres: 2.6, amount: 2120 },
  ],
};

/**
 * Returns effective printable data for any category.
 * If live data is empty or missing line items, automatically merges or provides realistic sample data.
 */
export function getEffectivePrintData(category: DocumentCategory, rawData: unknown): unknown {
  if (category === 'SALES_INVOICE') {
    if (!rawData) return SAMPLE_INVOICE_DATA;
    const inv = rawData as ApiInvoice;
    if (!inv.lines || inv.lines.length === 0) {
      return { ...inv, lines: SAMPLE_INVOICE_DATA.lines, totals: inv.totals?.net ? inv.totals : SAMPLE_INVOICE_DATA.totals };
    }
    return inv;
  }

  if (category === 'PURCHASE_BILL') {
    if (!rawData) return SAMPLE_PURCHASE_BILL_DATA;
    const inv = rawData as ApiInvoice;
    if (!inv.lines || inv.lines.length === 0) {
      return { ...inv, lines: SAMPLE_PURCHASE_BILL_DATA.lines, totals: inv.totals?.net ? inv.totals : SAMPLE_PURCHASE_BILL_DATA.totals };
    }
    return inv;
  }

  if (category === 'PURCHASE_INDENT') {
    if (!rawData) return SAMPLE_PURCHASE_INDENT_DATA;
    const inv = rawData as ApiInvoice;
    if (!inv.lines || inv.lines.length === 0) {
      return { ...inv, lines: SAMPLE_PURCHASE_INDENT_DATA.lines, totals: inv.totals?.net ? inv.totals : SAMPLE_PURCHASE_INDENT_DATA.totals };
    }
    return inv;
  }

  if (category === 'STOCK_DEDUCTION') {
    if (!rawData) return SAMPLE_STOCK_DEDUCTION_DATA;
    const inv = rawData as ApiInvoice;
    if (!inv.lines || inv.lines.length === 0) {
      return { ...inv, lines: SAMPLE_STOCK_DEDUCTION_DATA.lines, totals: inv.totals?.net ? inv.totals : SAMPLE_STOCK_DEDUCTION_DATA.totals };
    }
    return inv;
  }

  if (category === 'DSR') {
    if (!rawData) return SAMPLE_DSR_DATA;
    const dsr = rawData as ApiFullDsr;
    if (!dsr.total_bills && !dsr.sales_incl_gst) return SAMPLE_DSR_DATA;
    return dsr;
  }

  if (category === 'STOCK_BALANCE') {
    if (!rawData || !Array.isArray(rawData) || rawData.length === 0) {
      return SAMPLE_STOCK_BALANCE_DATA;
    }
    return rawData;
  }

  if (category === 'GENERIC_REPORT') {
    if (!rawData) return SAMPLE_GENERIC_REPORT_DATA;
    const rep = rawData as AnalyticalReportData;
    if (!rep.rows || rep.rows.length === 0 || !rep.columns || rep.columns.length === 0) {
      return SAMPLE_GENERIC_REPORT_DATA;
    }
    return rep;
  }

  return rawData;
}
