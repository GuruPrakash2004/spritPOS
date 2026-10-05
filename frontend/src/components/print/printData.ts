// Empty shapes the print layouts fall back to when a field is missing. There is no sample data: a blank
// company field prints blank, and a document or report with no lines prints with no lines.

import type { ApiCompany, ApiFullDsr, ApiInvoice } from '../../lib/api';
import type { AnalyticalReportData } from './layouts/AnalyticalReportLayout';
import type { DocumentCategory } from './rptRegistry';

export const BLANK_COMPANY: ApiCompany = {
  name: '', email: '', address: '', phone: '', fax: '', mobile: '', website: '', gstin: '', pan: '', cin: '',
  excise_phone: '', ksbcl_code: '', licence_type: '', state: '', fy_start: '', fy_end: '',
};

export const EMPTY_DOC: ApiInvoice = {
  doc_no: '', date: '', time: '', counter: '', customer: '', salesperson: '', status: 'SAVED', lines: [],
  totals: { qty: 0, litres: 0, basic: 0, cgst: 0, sgst: 0, discount: 0, points_redeemed: 0, points_earned: 0, net: 0 },
  payment: { cash: 0, card: 0, upi: 0, credit: 0 },
};

export const EMPTY_DSR: ApiFullDsr = {
  date: '', total_bills: 0, sales_incl_gst: 0, liquor_basic: 0, non_liquor_basic: 0, profit_of_the_day: null,
  opening_stock: { qty: 0, value: null }, closing_stock: { qty: 0, litres: 0, value: null },
  purchase: { basic: null, incl_gst: null }, sales_returns: 0, expenses: 0, cash_balance: 0, redemption: 0,
  payment_modes: {}, counter_wise: {}, salesperson_wise: {}, group_wise_sales: {}, expense_ledger_wise: {},
  cancelled_invoices: [],
};

export const EMPTY_REPORT: AnalyticalReportData = { title: '', columns: [], rows: [] };

/** The data as given, with the empty shape of its category filled in underneath (never sample values). */
export function printData(category: DocumentCategory, raw: unknown): unknown {
  const obj = raw && typeof raw === 'object' && !Array.isArray(raw) ? raw as Record<string, unknown> : null;
  if (category === 'STOCK_BALANCE') return Array.isArray(raw) ? raw : [];
  if (category === 'DSR') return { ...EMPTY_DSR, ...obj };
  if (category === 'GENERIC_REPORT') return { ...EMPTY_REPORT, ...obj };
  return { ...EMPTY_DOC, ...obj, totals: { ...EMPTY_DOC.totals, ...(obj?.totals as object) }, payment: { ...EMPTY_DOC.payment, ...(obj?.payment as object) } };
}
