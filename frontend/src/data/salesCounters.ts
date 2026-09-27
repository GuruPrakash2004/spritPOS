// Sales-invoice types and constants shared by the invoice screens.
//
// Real data (counters, customers) comes from the API; DEFAULT_COUNTER and PARTIES are prototype
// fallbacks still used by the invoice setup. billNumber() builds a doc number the same way the
// backend does: `<FY>/<counter prefix>-<7-digit seq>`, e.g. 26/c10-0083718.

export type SalesCounter = { code: string; name: string; systemName: string; prefix: string; inactive: boolean };

export const DEFAULT_COUNTER: SalesCounter = { code: 'counter10', name: 'counter10', systemName: 'PRADEEP', prefix: 'c10', inactive: false };

export type PaymentType = 'CASH' | 'CARD' | 'UPI' | 'SPLIT';

export const PAYMENT_TYPES: PaymentType[] = ['CASH', 'CARD', 'UPI', 'SPLIT'];

export const PARTIES = ['CASH CUSTOMER', 'SRI RAM BAR & RESTAURANT', 'HOTEL AMBASSADOR', 'RAJU (CREDIT)'];

// Everything the Sales Invoice setup form collects before the bill screen opens.
export type InvoiceSetup = { counter: SalesCounter; invoiceNo: string; paymentType: PaymentType; customer: string };

export const FY_PREFIX = '26';
export const FIRST_BILL_SEQ = 83718;
export const billNumber = (prefix: string, seq: number) => `${FY_PREFIX}/${prefix}-${String(seq).padStart(7, '0')}`;

export const SALES_PERSONS = ['GURU', 'PRADEEP', 'RAVI', 'ADMIN'];
