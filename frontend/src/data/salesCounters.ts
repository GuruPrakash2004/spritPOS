// Sales-invoice types and constants shared by the invoice screens.
//
// Counters, customers and sales persons come from the API (the shop's SQL Server); only the payment
// types are fixed (the backend accepts exactly these). Bill numbers are given by the backend when a bill
// is saved; GET /transactions/next-number previews the next one: `<FY>/<counter prefix>-<7-digit seq>`.

import type { ApiCounter } from '../lib/api';
export type { ApiNextNumber as NextNumber } from '../lib/api';

export type SalesCounter = { code: string; name: string; systemName: string; prefix: string; inactive: boolean };

export const NO_COUNTER: SalesCounter = { code: '', name: '', systemName: '', prefix: '', inactive: false };

export type PaymentType = 'CASH' | 'CARD' | 'UPI' | 'SPLIT';

export const PAYMENT_TYPES: PaymentType[] = ['CASH', 'CARD', 'UPI', 'SPLIT'];

// Everything the Sales Invoice setup form collects before the bill screen opens.
export type InvoiceSetup = { counter: SalesCounter; invoiceNo: string; paymentType: PaymentType; customer: string };

export const counterFromApi = (c: ApiCounter): SalesCounter => ({ code: c.code, name: c.name, systemName: c.system_name, prefix: c.prefix, inactive: c.inactive });
