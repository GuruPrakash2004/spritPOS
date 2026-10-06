// HTTP client for the FastAPI backend, plus the TypeScript shapes of its responses.
//
// - Every call goes to /api/<path>; Vite proxies that to http://localhost:8001 (see vite.config.ts).
// - After login, setAuthUser() stores the JWT (sessionStorage `vlqms_jwt_token`: it survives a page
//   refresh but not closing the tab, which suits a shared shop counter; with "Remember me", offered when
//   Login Settings allow it, it is kept in localStorage `vlqms_jwt_remember` until sign-out); each request then sends
//   `Authorization: Bearer <token>`. The backend needs it on every endpoint except login, the health
//   check, GET /masters/company and company image content.
// - Failed requests throw ApiError with FastAPI's `detail` text; show it with errorText(e).
// - The Api* types mirror backend/app/schemas.py and the router responses, so they stay snake_case.

export class ApiError extends Error {}

// The logged-in user and JWT token.
const TOKEN_KEY = 'vlqms_jwt_token';
const REMEMBER_KEY = 'vlqms_jwt_remember';
const readToken = () => {
  try {
    localStorage.removeItem(TOKEN_KEY);   // older builds kept the token here, surviving browser restarts
    return sessionStorage.getItem(TOKEN_KEY) || localStorage.getItem(REMEMBER_KEY) || '';
  } catch {
    return '';
  }
};
let rememberSession = (() => { try { return !!localStorage.getItem(REMEMBER_KEY); } catch { return false; } })();
/** "Remember me" at sign-in: keep this device signed in after the browser closes (until Sign out). */
export const setRememberSession = (on: boolean) => { rememberSession = on; };
let authUser = '';
let authRole = '';
let authToken = readToken();

export const setAuthToken = (token: string) => {
  authToken = token;
  try {
    if (token) sessionStorage.setItem(TOKEN_KEY, token);
    else sessionStorage.removeItem(TOKEN_KEY);
    if (token && rememberSession) localStorage.setItem(REMEMBER_KEY, token);
    else localStorage.removeItem(REMEMBER_KEY);
    if (!token) rememberSession = false;
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

// Field Level Rights of the signed-in user's role (Admin Master (Security)): master path -> field -> mode.
// MasterCrud hides "hidden" fields and locks "readonly" ones; the backend enforces the same.
let authFieldRights: ApiFieldRights = {};
export const setFieldRights = (rights: ApiFieldRights | undefined) => { authFieldRights = rights ?? {}; };
export const fieldRight = (master: string, field: string): 'hidden' | 'readonly' | undefined => authFieldRights[master]?.[field];

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
  if (!res.ok) failed(res, path, data);
  return data as T;
}

// Throw the server's message as ApiError (and end the session on a 401).
function failed(res: Response, path: string, data: { detail?: unknown } | null): never {
  // FastAPI sends { detail: string } for HTTPException and { detail: [{ msg }] } for validation errors.
  const detail = data?.detail;
  const message = Array.isArray(detail) ? detail.map((d: { msg: string }) => d.msg).join('; ') : detail as string | undefined;
  if (res.status === 401 && path !== '/auth/login' && authToken) onSessionEnded?.(message || 'Please log in again.');
  throw new ApiError(message || `Request failed (${res.status})`);
}

// Download a file the backend builds (e.g. '/admin/backup'): fetched with the login token, then handed
// to the browser's download under the server's file name. Returns that name.
export async function apiDownload(path: string, fallbackName: string): Promise<string> {
  let res: Response;
  try {
    res = await fetch(`/api${path}`, { headers: authToken ? { Authorization: `Bearer ${authToken}` } : {} });
  } catch {
    throw new ApiError('Backend is not reachable. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload --port 8001');
  }
  if (!res.ok) failed(res, path, await res.json().catch(() => null));
  const name = /filename="([^"]+)"/.exec(res.headers.get('Content-Disposition') ?? '')?.[1] ?? fallbackName;
  const url = URL.createObjectURL(await res.blob());
  const a = document.createElement('a');
  a.href = url;
  a.download = name;
  document.body.appendChild(a);
  a.click();
  a.remove();
  setTimeout(() => URL.revokeObjectURL(url), 1000);   // revoking at once can cancel the download in Safari
  return name;
}

// A fresh key for one draft document (see api's idempotencyKey).
export const newIdempotencyKey = () =>
  globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`;

// Message text for any caught error, ready for a toast.
export const errorText = (e: unknown) => (e instanceof Error ? e.message : String(e));

// Today as YYYY-MM-DD, the date format every backend date parameter expects. The PC's own date, not UTC's
// (toISOString would give yesterday's date in India before 05:30).
export const todayIso = () => {
  const d = new Date();
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
};

// Shapes returned by the backend (snake_case, as FastAPI sends them).
export type ApiCounter = { code: string; name: string; system_name: string; prefix: string; inactive: boolean };

export type ApiItem = {
  code: string; name: string; hsn_code: string; group: string; brand: string; liqtype: string; uom: string;
  package: string; bottles_per_case: number; mrp: number; purchase_rate: number | null; qoh: number; active?: boolean;
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

export type ApiLine = { item_code: string; item_name: string; cases: number; bottles: number; qty: number; rate: number; amount: number; litres: number; cgst?: number; sgst?: number };

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

// GET /transactions/next-number: the number the next document of a kind will most likely get (a preview).
export type ApiNextNumber = { doc_no: string; fy_prefix: string; prefix: string; number: number };

export type ApiCompany = {
  name: string; email: string; address: string; phone: string; fax: string; mobile: string; website: string;
  gstin: string; pan: string; cin: string; excise_phone: string; ksbcl_code: string;
  licence_type: string; state: string; fy_start: string; fy_end: string;
  fin_years?: { start: string; end: string; active: boolean }[];   // from GET /masters/company (Web_MstCompFinYear)
  place_id?: string;   // the shop on Google Maps, picked in Company Master (the Dashboard's Store Open / Closed)
};

// Add/Edit/Delete ticks per module (Admin Master). A module with no entry has full rights.
export type ApiRights = Record<string, { add: boolean; edit: boolean; delete: boolean }>;

export type ApiFieldRights = Record<string, Record<string, 'hidden' | 'readonly'>>;

export type ApiUser = {
  user: string;
  role: string;
  modules: string[];
  rights?: ApiRights;
  field_rights?: ApiFieldRights;
  must_change_password?: boolean;
  company: ApiCompany;
  data_mode?: DataMode;
  access_token?: string;
  token?: string;
  token_type?: string;
};

export type ApiAdminUser = {
  user: string; role: string; active: boolean; full_name?: string; email?: string; mobile?: string; department?: string;
  dob?: string | null; password_reset_on?: string | null; inactive_reason?: string; password_expired?: boolean;
};
export type ApiLoginSettings = {
  min_length: number; require_mix: boolean; require_upper: boolean; require_lower: boolean; require_digit: boolean;
  require_special: boolean; password_expiry_days: number; show_remember_me: boolean;
};
export type ApiFieldRightsAdmin = {
  masters: Record<string, string[]>; roles: Record<string, ApiFieldRights>;
  captions?: Record<string, Record<string, string>>;   // e.g. the Dashboard panels' names
};
export type ApiMasterApproval = {
  masters: Record<string, string>; required: string[];
  pending: { master: string; screen: string; code: string; name: string; changed_by: string | null }[];
};
export type ApiReportRights = { reports: string[]; rights: Record<string, string[]>; users: string[] };

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

// Tools → SQL Query Execution (backend/app/routers/tools.py): the vLQMS SQL Server (changes only to
// dbo.Web_* tables). "target" is kept in the requests; SQL Server is the only one.
export type ApiSqlTarget = 'sqlserver';
export type ApiSqlStatus = {
  db_connected: boolean; database: string | null; driver: string | null;
};
export type ApiSqlObject = { kind: 'Tables' | 'Views' | 'Procedures'; schema: string; name: string };
export type ApiSqlResult = {
  kind?: 'read' | 'write';
  table?: string;           // write: the table changed
  rows_affected?: number;   // write: "N row(s) affected"
  columns: string[];
  rows: (string | number | boolean | null)[][];
  row_count: number;
  truncated: boolean;
  elapsed_ms: number;
};
export type ApiSqlTop = ApiSqlResult & {
  object: { schema: string; name: string; type: string }; total_rows: number;
  writable: boolean;        // a web table: its rows can be changed with an UPDATE / DELETE query
};

// Tools → Re Write Stock (backend/app/stock_rewrite.py). On the "ledger" tab the two quantities are
// ledger_stock (base + movements) and qoh; on the others trn_stock (documents) and sys_stock (movements).
export type ApiStockDiffRow = {
  item_code: string;
  item_name: string;
  trn_stock?: number;
  sys_stock?: number;
  ledger_stock?: number;
  qoh?: number;
  diff: number;
};
export type ApiStockTab = { key: string; caption: string; rows: ApiStockDiffRow[]; error?: string | null };
export type ApiStockCheck = { tabs: ApiStockTab[]; rewrite_required: boolean };
export type ApiRewriteCheck = { web: ApiStockCheck; desktop: ApiStockCheck | null; db_connected: boolean };
export type ApiRewriteResult = {
  rewritten: boolean;
  message: string;
  items_changed?: number;
  movements?: number;
  check: ApiStockCheck;
};

// ─── Utilities (backend app/routers/utilities.py) ──────────────────────────────
export type ApiSettingKind = 'yn' | 'number' | 'text' | 'date';
export type ApiSetting = {
  code: string; group: string; caption: string; value: string; kind: ApiSettingKind; remarks: string;
  effect: string | null; hidden: boolean; source: 'web' | 'custom' | 'env'; readonly: boolean;
};
export type ApiAuditItem = { code: string | null; name: string | null; status: string | null; details: string | null };
export type ApiAuditRow = {
  id: number | string; at: string; screen: string; action: string; main_fld: string; user_code: string | null;
  earlier: string; changed: string; items: ApiAuditItem[];
};
// GET /company/hours: Open / Closed from the shop's opening hours on Google Maps (open_now null when unknown).
export type ApiShopHours = { configured: boolean; open_now: boolean | null; today: string; place: string; maps_url: string; error: string | null };

// What the screens show: the shop's SQL Server ("live"), the sample data of the Test data switch ("test"), or nothing.
export type DataMode = 'live' | 'test' | 'none';
export type ApiAbout = {
  product: string; software_version: string; api_version: string; company: string; db_connected: boolean; server_time: string;
  db_configured: boolean; db_disconnected_for_testing: boolean; data_mode: DataMode;
};
export type ApiDupInvoice = {
  doc_no: string; date: string; time: string; added_by: string; sales_type: string; bill_system: 'Web' | 'Desktop';
  qty: number; value: number; counter: string; removable: boolean; duplicate_of: string | null; why_not: string | null;
};
/** Utilities → Web Sync: whether it is set up, the central site (host only) and how the last push went. */
export type ApiWebSync = {
  configured: boolean; central: string; every_minutes: number; last_ok: string | null; last_day: string | null;
  last_stock_ok: string | null; last_error: string | null; last_error_at: string | null;
};
export type ApiMergeSide = { code: string; name: string; qoh: number; active: boolean };
export type ApiMergePreview = {
  from: ApiMergeSide; to: ApiMergeSide; lines: Record<string, number>; movements: number; qoh_after: number; message?: string;
};
export type ApiStockSheet = { basis: string; rows: { code: string; name: string; qty: number; value: number }[] };
