// HTTP client for the FastAPI backend, plus the TypeScript shapes of its responses.
//
// - Every call goes to /api/<path>; Vite proxies that to http://localhost:8001 (see vite.config.ts).
// - After login, setAuthUser() stores the JWT (sessionStorage `vlqms_jwt_token`: it survives a page
//   refresh but not closing the tab, which suits a shared shop counter); each request then sends
//   `Authorization: Bearer <token>`. The backend needs it on every endpoint except login, the health
//   check, GET /masters/company and company image content.
// - Failed requests throw ApiError with FastAPI's `detail` text; show it with errorText(e).
// - The Api* types mirror backend/app/schemas.py and the router responses, so they stay snake_case.

export class ApiError extends Error {}

// The logged-in user and JWT token.
const TOKEN_KEY = 'vlqms_jwt_token';
const readToken = () => {
  try {
    localStorage.removeItem(TOKEN_KEY);   // older builds kept the token here, surviving browser restarts
    return sessionStorage.getItem(TOKEN_KEY) || '';
  } catch {
    return '';
  }
};
let authUser = '';
let authRole = '';
let authToken = readToken();

export const setAuthToken = (token: string) => {
  authToken = token;
  try {
    if (token) sessionStorage.setItem(TOKEN_KEY, token);
    else sessionStorage.removeItem(TOKEN_KEY);
  } catch {
    // Storage blocked (private mode): the session just won't survive a refresh.
  }
};

export const getAuthToken = () => authToken;

// Called by App.tsx after login (and with empty strings on logout). Every later api() call sends these.
export const setAuthUser = (user: string, role: string = '', token?: string) => {
  authUser = user;
  authRole = role;
  if (token !== undefined) {
    setAuthToken(token);
  }
};
export const getAuthUser = () => authUser;

// The logged-in role's modules and Add/Edit/Delete ticks (set by App.tsx at login), for hiding buttons.
// The backend enforces the same rights; this only keeps the screen honest.
let authModules: string[] = [];
let authRights: ApiRights = {};
export const setAuthRights = (modules: string[], rights: ApiRights = {}) => { authModules = modules; authRights = rights; };
// Same list as backend auth.COST_MODULES: roles with one of these see purchase costs and profit.
const COST_MODULES = ['Purchase Bill', 'Purchase Indent', 'Profit Margin', 'Price List Maintenance', 'Stock Ageing', 'Reports Dashboard'];
export const canSeeCost = () => authModules.includes('*') || COST_MODULES.some(m => authModules.includes(m));
export const canDo = (module: string, action: 'add' | 'edit' | 'delete') =>
  (authModules.includes('*') || authModules.includes(module)) && authRights[module]?.[action] !== false;
export const getAuthRole = () => authRole;

// App.tsx registers this: a 401 from any call except login means the session is over (expired token,
// password changed, user deactivated), so the app returns to the sign-in page with the server's reason.
let onSessionEnded: ((reason: string) => void) | null = null;
export const setSessionEndedHandler = (handler: ((reason: string) => void) | null) => { onSessionEnded = handler; };

// Call the backend: `await api<ApiItem[]>('/masters/items')`, or
// `await api('/masters/items', { method: 'POST', body: item })`. `path` is the backend route without /api.
// Returns the parsed JSON (undefined for 204) or throws ApiError with the server's message.
// A save sent with the same idempotencyKey again (double-click, retry after a dropped reply) gets the
// first reply back instead of creating a second document (backend main.py). Use one key per draft.
export async function api<T>(path: string, init?: { method?: string; body?: unknown; idempotencyKey?: string }): Promise<T> {
  let res: Response;
  try {
    const headers: Record<string, string> = {
      ...(init?.body === undefined ? {} : { 'Content-Type': 'application/json' }),
      ...(authToken ? { Authorization: `Bearer ${authToken}` } : {}),
      ...(init?.idempotencyKey ? { 'Idempotency-Key': init.idempotencyKey } : {}),
    };
    res = await fetch(`/api${path}`, {
      method: init?.method ?? 'GET',
      headers,
      body: init?.body === undefined ? undefined : JSON.stringify(init.body),
    });
  } catch {
    throw new ApiError('Backend is not reachable. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload --port 8001');
  }
  if (res.status === 204) return undefined as T;
  const data = await res.json().catch(() => null);
  if (!res.ok) {
    // FastAPI sends { detail: string } for HTTPException and { detail: [{ msg }] } for validation errors.
    const detail = data?.detail;
    const message = Array.isArray(detail) ? detail.map((d: { msg: string }) => d.msg).join('; ') : detail;
    if (res.status === 401 && path !== '/auth/login' && authToken) onSessionEnded?.(message || 'Please log in again.');
    throw new ApiError(message || `Request failed (${res.status})`);
  }
  return data as T;
}

// A fresh key for one draft document (see api's idempotencyKey).
export const newIdempotencyKey = () =>
  globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`;

// Message text for any caught error, ready for a toast.
export const errorText = (e: unknown) => (e instanceof Error ? e.message : String(e));

// Today as YYYY-MM-DD, the date format every backend date parameter expects.
export const todayIso = () => new Date().toISOString().slice(0, 10);

// Shapes returned by the backend (snake_case, as FastAPI sends them).
export type ApiCounter = { code: string; name: string; system_name: string; prefix: string; inactive: boolean };

export type ApiItem = {
  code: string; name: string; hsn_code: string; group: string; brand: string; liqtype: string; uom: string;
  package: string; bottles_per_case: number; mrp: number; purchase_rate: number | null; qoh: number;
};

export type ApiStockRow = {
  item_code: string; item_name: string; group: string; package: string;
  op_qty: number; rec_qty: number; iss_qty: number; cl_qty: number; cl_litres: number; cl_value: number | null;
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

export type ApiSupplier = {
  code: string; name: string; alias: string; address: string; city: string; pin: string; state: string;
  country: string; phone: string; gstin: string; currency: string; active: boolean;
};

export type ApiLedger = { code: string; name: string; group: string };

export type ApiCompany = {
  name: string; email: string; address: string; phone: string; fax: string; mobile: string; website: string;
  gstin: string; pan: string; cin: string; excise_phone: string; ksbcl_code: string;
  licence_type: string; state: string; fy_start: string; fy_end: string;
};

// Add/Edit/Delete ticks per module (Admin Master). A module with no entry has full rights.
export type ApiRights = Record<string, { add: boolean; edit: boolean; delete: boolean }>;

export type ApiUser = {
  user: string;
  role: string;
  modules: string[];
  rights?: ApiRights;
  company: ApiCompany;
  access_token?: string;
  token?: string;
  token_type?: string;
};

export type ApiAdminUser = { user: string; role: string; active: boolean };

export type ApiRole = { name: string; modules: string[]; rights?: ApiRights; users: number };

export type ApiFullDsr = ApiDsr & {
  date: string; liquor_basic: number; non_liquor_basic: number; profit_of_the_day: number | null;
  opening_stock: { qty: number; value: number | null }; closing_stock: { qty: number; value: number | null; litres: number };
  payment_modes: Record<string, number>; purchase: { basic: number | null; incl_gst: number | null };
  sales_returns: number; expenses: number; cash_balance: number; redemption: number;
  counter_wise: Record<string, number>; salesperson_wise: Record<string, number>;
  group_wise_sales: Record<string, number>; expense_ledger_wise: Record<string, number>; cancelled_invoices: string[];
};

export type ApiClosingLoad = { item_code: string; item_name: string; package: string; op_qty: number; mrp: number };

export type ApiDenomination = { date: string; counts: Record<string, number>; total: number };

// GET /reports/restock: items ranked by restock priority (fast sellers that are running out first).
export type ApiRestockStatus = 'OUT_OF_STOCK' | 'CRITICAL' | 'LOW' | 'OK';
export type ApiRestockRow = {
  item_code: string; item_name: string; group: string; brand: string; mrp: number;
  qoh: number; rol: number; roq: number;
  sold_qty: number; avg_daily: number; days_left: number | null; last_sold: string | null;
  status: ApiRestockStatus; needs_restock: boolean; priority: number; suggested_qty: number;
  // The sales window: the last N days up to today; window_days = days the shop has actually been selling.
  sales_from: string; sales_to: string; window_days: number;
  source: 'live' | 'memory'; // live = read from SQL Server just now; memory = in-app data
};

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
