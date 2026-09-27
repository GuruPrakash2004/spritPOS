// Thin client for the FastAPI backend. Vite proxies /api to localhost:8001 (see vite.config.ts).

export class ApiError extends Error {}

// The logged-in user and JWT token.
let authUser = '';
let authRole = '';
let authToken = localStorage.getItem('vlqms_jwt_token') || '';

export const setAuthToken = (token: string) => {
  authToken = token;
  if (token) {
    localStorage.setItem('vlqms_jwt_token', token);
  } else {
    localStorage.removeItem('vlqms_jwt_token');
  }
};

export const getAuthToken = () => authToken;

export const setAuthUser = (user: string, role: string = '', token?: string) => {
  authUser = user;
  authRole = role;
  if (token !== undefined) {
    setAuthToken(token);
  }
};
export const getAuthUser = () => authUser;
export const getAuthRole = () => authRole;

export async function api<T>(path: string, init?: { method?: string; body?: unknown }): Promise<T> {
  let res: Response;
  try {
    const headers: Record<string, string> = {
      ...(init?.body === undefined ? {} : { 'Content-Type': 'application/json' }),
      ...(authToken ? { Authorization: `Bearer ${authToken}` } : {}),
      ...(authUser ? { 'X-User': authUser } : {}),
    };
    res = await fetch(`/api${path}`, {
      method: init?.method ?? 'GET',
      headers,
      body: init?.body === undefined ? undefined : JSON.stringify(init.body),
    });
  } catch {
    throw new ApiError('Backend is not reachable. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload');
  }
  if (res.status === 204) return undefined as T;
  const data = await res.json().catch(() => null);
  if (!res.ok) {
    // FastAPI sends { detail: string } for HTTPException and { detail: [{ msg }] } for validation errors.
    const detail = data?.detail;
    const message = Array.isArray(detail) ? detail.map((d: { msg: string }) => d.msg).join('; ') : detail;
    throw new ApiError(message || `Request failed (${res.status})`);
  }
  return data as T;
}

export const errorText = (e: unknown) => (e instanceof Error ? e.message : String(e));

export const todayIso = () => new Date().toISOString().slice(0, 10);

// Shapes returned by the backend (snake_case, as FastAPI sends them).
export type ApiCounter = { code: string; name: string; system_name: string; prefix: string; inactive: boolean };

export type ApiItem = {
  code: string; name: string; hsn_code: string; group: string; brand: string; liqtype: string; uom: string;
  package: string; bottles_per_case: number; mrp: number; purchase_rate: number; qoh: number;
};

export type ApiStockRow = {
  item_code: string; item_name: string; group: string; package: string;
  op_qty: number; rec_qty: number; iss_qty: number; cl_qty: number; cl_litres: number; cl_value: number;
};

export type ApiDoc = { doc_no: string; totals: { net: number; qty: number; litres: number } };

export type ApiDsr = { total_bills: number; sales_incl_gst: number };

export type ApiDailySale = { date: string; day?: string; bills: number; qty: number; litres: number; amount: number };

export type ApiStockMovementItem = { label: string; value: string; pct: number; color: string };

export type ApiStockMovement = {
  date: string;
  opening_stock: number;
  received: number;
  issued: number;
  closing_stock: number;
  flow: ApiStockMovementItem[];
};

export type ApiLine = { item_code: string; item_name: string; cases: number; bottles: number; qty: number; rate: number; amount: number; litres: number };

export type ApiInvoice = {
  doc_no: string; date: string; time?: string; counter: string; customer: string; salesperson: string | null;
  status: 'SAVED' | 'HOLD' | 'CANCELLED'; lines: ApiLine[]; cancel_remarks?: string;
  totals: { qty: number; litres: number; basic: number; cgst: number; sgst: number; discount: number; points_redeemed: number; points_earned: number; net: number };
  payment: { cash: number; card: number; upi: number; credit: number };
};

export type ApiIndent = { doc_no: string; date: string; supplier: string; lines: ApiLine[]; totals: { qty: number; net: number } };

export type ApiCustomer = { code: string; name: string; mobile: string; loyalty_card_no: string; send_sms: boolean; points: number };

export type ApiSupplier = { code: string; name: string; gstin: string; city: string; active: boolean };

export type ApiLedger = { code: string; name: string; group: string };

export type ApiCompany = {
  name: string; email: string; address: string; phone: string; fax: string; mobile: string; website: string;
  gstin: string; pan: string; cin: string; excise_phone: string; ksbcl_code: string;
  licence_type: string; state: string; fy_start: string; fy_end: string;
};

export type ApiUser = {
  user: string;
  role: string;
  modules: string[];
  company: ApiCompany;
  access_token?: string;
  token?: string;
  token_type?: string;
};

export type ApiAdminUser = { user: string; role: string; active: boolean };

export type ApiRole = { name: string; modules: string[]; users: number };

export type ApiFullDsr = ApiDsr & {
  date: string; liquor_basic: number; non_liquor_basic: number; profit_of_the_day: number;
  opening_stock: { qty: number; value: number }; closing_stock: { qty: number; value: number; litres: number };
  payment_modes: Record<string, number>; purchase: { basic: number; incl_gst: number };
  sales_returns: number; expenses: number; cash_balance: number; redemption: number;
  counter_wise: Record<string, number>; salesperson_wise: Record<string, number>;
  group_wise_sales: Record<string, number>; expense_ledger_wise: Record<string, number>; cancelled_invoices: string[];
};

export type ApiClosingLoad = { item_code: string; item_name: string; package: string; op_qty: number; mrp: number };

export type ApiDenomination = { date: string; counts: Record<string, number>; total: number };

// Full Item Master row, as the backend's Item schema defines it.
export type ApiItemFull = ApiItem & {
  short_name: string; cgst_per: number; sgst_per: number; rol: number; roq: number; active: boolean;
};

// Build a query string, dropping empty values.
export const qs = (params: Record<string, string | number | undefined>) => {
  const p = new URLSearchParams();
  Object.entries(params).forEach(([k, v]) => { if (v !== undefined && v !== '') p.set(k, String(v)); });
  const s = p.toString();
  return s ? `?${s}` : '';
};
