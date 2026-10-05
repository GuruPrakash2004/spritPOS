// Response shapes of the central site's API (backend/app/central/main.py). Column names inside the day's
// tables are the desktop's (RptDSRSmry, RptDSRCtrSales, …), as the shops push them (backend/app/websync.py).

export type CentralSession = { user: string; role: 'Admin' | 'Owner'; must_change: boolean; access_token?: string };

export type CentralShop = {
  id: number; code: string; name: string; active: boolean;
  last_sync: string | null; last_stock_sync: string | null; last_day: string | null;
};

export type DsrSummary = Record<string, number | string | null>;
export type DsrRow = Record<string, number | string | null>;

export type CentralDsr = {
  day: string;
  client: string | null;
  summary: DsrSummary | null;
  shops: ({ client: string; name: string; last_sync: string | null; synced: boolean } & Partial<Record<
    'TotBills' | 'TotSalesAmt' | 'SalesProfitAmt' | 'ClStkQty' | 'CLStkValue' | 'TotCashAmt' | 'ExpenseAmt' | 'CashBalanceAmt', number>>)[];
  tables: Record<string, DsrRow[]>;
};

export type CentralStock = {
  shops: { client: string; name: string; last_stock_sync: string | null }[];
  items: {
    item_code: string; item_name: string; group: string; brand: string; package: string; mrp: number; total_qoh: number;
    shops: { client: string; qoh: number; min_level: number; short: boolean }[];
  }[];
};

export type CentralIndent = {
  client: string; PINO: string; PIDate: string | null; SUPPName: string | null; GRNDTOT: number; TOTAMT: number;
  lines: { SlNo: number; ItemCode: string; ITEMName: string; PIQty: number; PIRate: number; PIAmt: number; MRP: number }[];
};

/** A shop that hasn't sent anything for this long is shown as not syncing. */
export const STALE_MINUTES = 30;
export const isStale = (iso: string | null) => !iso || Date.now() - new Date(iso).getTime() > STALE_MINUTES * 60_000;
export const when = (iso: string | null) => (iso ? new Date(iso).toLocaleString() : 'Never');
