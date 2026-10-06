// The one place that talks to the FastAPI backend: its address, the signed-in session, one named function per
// backend endpoint, and the TypeScript shapes of the responses. Screens import the functions they need from here;
// nothing else builds a URL or calls fetch.
//
// - The backend's address is VITE_API_URL in frontend/.env (http://localhost:8001); the backend allows every origin
//   (CORS). Without it, calls go to /api/<path> on the page's own address (Vite proxies it, see vite.config.ts).
// - After login, setAuthUser() stores the JWT (sessionStorage `vlqms_jwt_token`: it survives a page
//   refresh but not closing the tab, which suits a shared shop counter; with "Remember me", offered when
//   Login Settings allow it, it is kept in localStorage `vlqms_jwt_remember` until sign-out); each request then sends
//   `Authorization: Bearer <token>`. The backend needs it on every endpoint except login, the health
//   check, GET /masters/company and company image content.
// - Failed requests throw ApiError with FastAPI's `detail` text; show it with errorText(e).
// - The Api* types mirror backend/app/schemas.py and the router responses, so they stay snake_case.
import type { PrintKind, PrintOptions, PrintRequest } from './printing';
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

// A screen opened in a new browser tab starts with an empty sessionStorage. The tabs that are already signed in
// hand their token over through a BroadcastChannel (same browser and site only), so the biller does not sign in
// again for every tab. The token is never written anywhere else, so closing all tabs still ends the session.
const TAB_CHANNEL = typeof BroadcastChannel === 'undefined' ? null : new BroadcastChannel('vlqms_session');
if (TAB_CHANNEL) {
  TAB_CHANNEL.onmessage = e => {
    if (e.data?.type === 'need-token' && authToken) TAB_CHANNEL.postMessage({ type: 'token', token: authToken });
  };
}
/** Ask the other open tabs for their sign-in token (resolves with '' when none answers in time). */
export const requestTokenFromOtherTabs = (waitMs = 400): Promise<string> => new Promise(resolve => {
  if (!TAB_CHANNEL) return resolve('');
  const done = (token: string) => { clearTimeout(timer); TAB_CHANNEL.removeEventListener('message', onMsg); resolve(token); };
  const onMsg = (e: MessageEvent) => { if (e.data?.type === 'token' && e.data.token) done(String(e.data.token)); };
  const timer = setTimeout(() => done(''), waitMs);
  TAB_CHANNEL.addEventListener('message', onMsg);
  TAB_CHANNEL.postMessage({ type: 'need-token' });
});

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

// ─── Where the backend is, and how a request is sent ──────────────────────────────────────────────

// The backend's address: VITE_API_URL in frontend/.env (e.g. http://localhost:8001). The backend allows every origin
// (CORS), so the browser calls it directly. Without it, every call goes to /api/<path> on this page's own address
// (Vite's dev proxy, or the backend serving the built frontend).
const cleanBase = (v: unknown) => String(v ?? '').trim().replace(/\/+$/, '');
export const API_BASE = cleanBase(import.meta.env.VITE_API_URL) || '/api';
// The hosted central site (central.html) has its own backend: VITE_CENTRAL_API_URL, else the same rule as above.
export const CENTRAL_API_BASE = cleanBase(import.meta.env.VITE_CENTRAL_API_URL) || '/api';

type Init = { method?: string; body?: unknown; idempotencyKey?: string; base?: string };

const unreachable = (base: string) =>
  new ApiError(`Backend is not reachable at ${base.startsWith('http') ? base : 'this site\'s /api'}. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload --port 8001`);

async function send(path: string, init: Init): Promise<Response> {
  const base = init.base ?? API_BASE;
  try {
    const headers: Record<string, string> = {
      ...(init.body === undefined ? {} : { 'Content-Type': 'application/json' }),
      ...(authToken ? { Authorization: `Bearer ${authToken}` } : {}),
      ...(init.idempotencyKey ? { 'Idempotency-Key': init.idempotencyKey } : {}),
    };
    return await fetch(`${base}${path}`, {
      method: init.method ?? 'GET',
      headers,
      body: init.body === undefined ? undefined : JSON.stringify(init.body),
    });
  } catch {
    throw unreachable(base);
  }
}

// Every JSON call goes through here (private: screens use the named functions below). Returns the parsed JSON
// (undefined for 204) or throws ApiError with the server's message. A save sent with the same idempotencyKey
// again (double-click, retry after a dropped reply) gets the first reply back instead of creating a second
// document (backend main.py). Use one key per draft.
async function request<T>(path: string, init: Init = {}): Promise<T> {
  const res = await send(path, init);
  if (res.status === 204) return undefined as T;
  const data = await res.json().catch(() => null);
  if (!res.ok) failed(res, path, data);
  return data as T;
}

// A request whose answer is not always JSON (a PDF for a print preview, or JSON when it was sent to a printer).
// The caller reads the Response; a failure is thrown as ApiError like request().
async function requestRaw(path: string, init: Init = {}): Promise<Response> {
  const res = await send(path, init);
  if (!res.ok) failed(res, path, await res.json().catch(() => null));
  return res;
}

// Throw the server's message as ApiError (and end the session on a 401).
function failed(res: Response, path: string, data: { detail?: unknown } | null): never {
  // FastAPI sends { detail: string } for HTTPException and { detail: [{ msg }] } for validation errors.
  const detail = data?.detail;
  const message = Array.isArray(detail) ? detail.map((d: { msg: string }) => d.msg).join('; ') : detail as string | undefined;
  if (res.status === 401 && path !== '/auth/login' && authToken) onSessionEnded?.(message || 'Please log in again.');
  throw new ApiError(message || `Request failed (${res.status})`);
}

// A fresh key for one draft document (see request's idempotencyKey).
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

// Build a query string, dropping empty values.
export const qs = (params: Record<string, string | number | undefined>) => {
  const p = new URLSearchParams();
  Object.entries(params).forEach(([k, v]) => { if (v !== undefined && v !== '') p.set(k, String(v)); });
  const s = p.toString();
  return s ? `?${s}` : '';
};

// ═══ The backend's functions ═════════════════════════════════════════════════════════════════════════
// One function per backend endpoint (backend/app/main.py and app/routers/*). Screens import these and never build
// a URL themselves. The response types are at the end of this file.

// ─── Sign-in, session and Database setup (routers/auth.py, routers/setup.py) ───────────────────────

export const login = (body: { user: string; password: string }) => request<ApiUser>('/auth/login', { method: 'POST', body });
export const getMe = () => request<ApiUser>('/auth/me');
export const changePassword = (body: { current_password: string; new_password: string }) =>
  request<{ access_token: string }>('/auth/change-password', { method: 'POST', body });
/** The server's RSA public key every password is encrypted with (lib/secureField.ts). */
export const getTransportKey = () => request<ApiTransportKey>('/auth/transport-key');
/** Password rules and "Remember me" for the sign-in page (public). */
export const getPublicLoginSettings = () => request<ApiLoginSettings>('/auth/login-settings');

export const getSetupStatus = () => request<ApiSetupStatus>('/setup/status');
/** The connection settings in use (no password), to fill the Database settings form. */
export type ApiSetupCurrent = { host: string; port: string; database: string; auth: 'sql'; user: string; has_password: boolean; connected: boolean };
export const getSetupCurrent = () => request<ApiSetupCurrent>('/setup/current');
export const setupTest = (body: ApiSetupServer) =>
  request<{ ok: boolean; databases?: string[]; error?: string; driver?: string }>('/setup/test', { method: 'POST', body });
export const setupConnect = (body: ApiSetupServer & { database: string }) =>
  request<{ database: string; edition: string; ready: boolean }>('/setup/connect', { method: 'POST', body });
export const createAdmin = (body: { user: string; password: string }) =>
  request<{ user: string; role: string }>('/setup/admin', { method: 'POST', body });

// ─── Masters (routers/masters.py) ──────────────────────────────────────────────────────────────────

/** Every row of a master: `path` is the master's route, e.g. 'items', 'sales-counters'. */
export const listMaster = <T>(path: string) => request<T[]>(`/masters/${path}`);
export const createMaster = <T = unknown>(path: string, body: unknown) => request<T>(`/masters/${path}`, { method: 'POST', body });
export const updateMaster = <T = unknown>(path: string, code: string, body: unknown) =>
  request<T>(`/masters/${path}/${encodeURIComponent(code)}`, { method: 'PUT', body });
export const deleteMaster = (path: string, code: string) => request<void>(`/masters/${path}/${encodeURIComponent(code)}`, { method: 'DELETE' });

export const getItems = () => listMaster<ApiItemFull>('items');
export const getItemGroups = () => listMaster<{ code: string }>('item-groups');
export const getCustomers = () => listMaster<ApiCustomer>('customers');
export const getSuppliers = () => listMaster<ApiSupplier>('suppliers');
export const getSalesCounters = () => listMaster<ApiCounter>('sales-counters');
export const getLedgers = () => listMaster<ApiLedger>('ledgers');
export const getSalesPersons = () => request<string[]>('/sales-persons');
export const importItems = (body: unknown) => request<{ added: string[]; duplicates: string[] }>('/masters/items/import', { method: 'POST', body });
export const updatePriceList = (body: unknown) => request<ApiItemFull[]>('/masters/items/price-list', { method: 'POST', body });

export const getCompany = () => request<ApiCompany>('/masters/company');
export const saveCompany = (body: unknown) => request<ApiCompany>('/masters/company', { method: 'PUT', body });
export const getCompanyHours = () => request<ApiShopHours>('/company/hours');

// Company Master's pictures and documents (routers/company_files.py).
export const listCompanyFiles = <T>(kind: string) => request<T[]>(`/company/files${qs({ kind })}`);
export const uploadCompanyFile = (body: unknown) => request<unknown>('/company/files', { method: 'POST', body });
export const updateCompanyFile = (id: number | string, body: unknown) => request<unknown>(`/company/files/${id}`, { method: 'PUT', body });
export const deleteCompanyFile = (id: number | string) => request<void>(`/company/files/${id}`, { method: 'DELETE' });
/** Where a company file's content is served, for <img src> and links (the browser can't send the token there). */
export const companyFileUrl = (id: number | string, download = false) => `${API_BASE}/company/files/${id}/content${download ? '?download=true' : ''}`;

// ─── Documents (routers/transactions.py) ───────────────────────────────────────────────────────────

/** The number the next document of a kind will most likely get (a preview; the backend numbers on save). */
export const getNextNumber = (kind: string, counter?: string) => request<ApiNextNumber>(`/transactions/next-number${qs({ kind, counter })}`);

const salesInvoicePath = (docNo: string) => `/transactions/sales-invoice/${docNo}`;
export const listSalesInvoices = (status?: string) => request<ApiInvoice[]>(`/transactions/sales-invoice${qs({ status })}`);
export const saveSalesInvoice = (body: unknown, idempotencyKey: string) =>
  request<ApiInvoice>('/transactions/sales-invoice', { method: 'POST', body, idempotencyKey });
export const reprintSalesInvoice = (docNo: string) => request<{ invoice: ApiInvoice }>(`${salesInvoicePath(docNo)}/reprint`);
export const cancelSalesInvoice = (docNo: string, remarks: string) =>
  request<ApiInvoice>(`${salesInvoicePath(docNo)}/cancel`, { method: 'POST', body: { remarks } });
/** Remove a held bill (recalling it deletes the hold). */
export const deleteSalesInvoice = (docNo: string) => request<void>(salesInvoicePath(docNo), { method: 'DELETE' });

export const getPurchaseIndents = () => request<ApiIndent[]>('/transactions/purchase-indent');
export const savePurchaseBill = (body: unknown, idempotencyKey: string) =>
  request<ApiDoc>('/transactions/purchase-bill', { method: 'POST', body, idempotencyKey });
/** Save a stock document (Purchase Indent, Stock Deduction, Sales Return) at its own endpoint, e.g. '/transactions/stock-deduction'. */
export const saveStockDocument = (endpoint: string, body: unknown) => request<ApiDoc>(endpoint, { method: 'POST', body });
export const saveExpense = (body: unknown) => request<{ doc_no: string; total: number }>('/transactions/expense', { method: 'POST', body });

export const loadClosingStock = (asOn: string) => request<ApiClosingLoad[]>(`/transactions/closing-stock/load${qs({ as_on: asOn })}`);
export const saveClosingStock = (body: unknown) => request<{ doc_no: string }>('/transactions/closing-stock', { method: 'POST', body });
export const generateClosingStockInvoice = (docNo: string, body: { counter: string; salesperson: string }) =>
  request<ApiDoc>(`/transactions/closing-stock/${docNo}/generate-invoice`, { method: 'POST', body });

export const getDenomination = (on: string) => request<ApiDenomination | null>(`/transactions/denomination${qs({ on })}`);
export const saveDenomination = (body: unknown) => request<ApiDenomination>('/transactions/denomination', { method: 'POST', body });

// ─── Reports (routers/reports.py, routers/custom_reports.py) ───────────────────────────────────────

export const getDsr = (on: string = todayIso()) => request<ApiFullDsr>(`/reports/dsr${qs({ on })}`);
export const getDailySales = (fromDate: string, toDate: string) => request<ApiDailySale[]>(`/reports/sales/daily${qs({ from_date: fromDate, to_date: toDate })}`);
export const getStockMovement = (on: string = todayIso()) => request<ApiStockMovement>(`/reports/stock-movement${qs({ on })}`);
export const getRestock = (days: number, limit: number) => request<ApiRestockRow[]>(`/reports/restock${qs({ days, limit })}`);
export const getStockBalance = (date: string) => request<ApiStockRow[]>(`/reports/stock-balance${qs({ from_date: date, to_date: date })}`);
/** Any table report: `endpoint` and its filters come from components/reports/reportDefs.ts. */
export const getReport = <T>(endpoint: string, params: Record<string, string | number | undefined> = {}) => request<T[]>(`${endpoint}${qs(params)}`);
export const getRptFiles = <T>(category: string) => request<T[]>(`/reports/rpt-files${qs({ category })}`);

export const getCustomReportRights = () => request<{ known: string[]; allowed: string[]; groups?: Record<string, string> }>('/reports/custom/allowed');
/** A Customised Report through the Crystal file in its definition: the PDF (with a printer: printed there, JSON back). */
export const renderCustomReportPrint = (body: { name: string; from_date: string; to_date: string; ledger: string[]; supplier: string[]; group: string[] }, printer = '') =>
  requestRaw('/print/custom-report', { method: 'POST', body: { ...body, printer } });
/** `query` is the already-built query string of the filters (without the '?'). */
export const runCustomReport = <T>(query: string) => request<T>(`/reports/custom?${query}`);
export const emailCustomReport = (body: unknown) => request<{ sent_to: string[]; rows: number }>('/reports/custom/email', { method: 'POST', body });

// ─── Printing (routers/printing.py) ────────────────────────────────────────────────────────────────

export const getPrintOptions = (kind: PrintKind) => request<PrintOptions>(`/print/options${qs({ kind })}`);
/** The PDF (or, for a direct print, JSON) the server built from the document's Crystal .rpt format. */
export const renderPrint = (body: PrintRequest) => requestRaw('/print/render', { method: 'POST', body });
/** Remember the print option this user picked for a document. */
export const setPrintPreference = (kind: PrintKind, option: string) => request<{ kind: string; option: string }>('/print/preference', { method: 'PUT', body: { kind, option } });
/** The Daily Summary Report's .rpt formats that are in the report folder. */
export const getDsrFormats = () => request<{ formats: string[]; bridge_configured: boolean }>('/print/dsr-formats');
/** The Daily Summary Report of a day as a PDF laid out by the desktop's RptDSR.rpt (rpt empty = RptDSR.rpt). */
export const renderDsrPrint = (on: string, rpt: string, printer = '') => requestRaw('/print/dsr', { method: 'POST', body: { on, rpt, printer } });
/** The Stock Balance .rpt formats that are in the report folder. */
export const getStockBalanceFormats = () => request<{ formats: string[]; bridge_configured: boolean }>('/print/stock-balance-formats');
/** The Stock Balance report of a period as a PDF laid out by the desktop's RptStkBal*.rpt (with a printer: printed there, JSON back). */
export const renderStockBalancePrint = (from_date: string, to_date: string, group: string, rpt: string, printer = '') =>
  requestRaw('/print/stock-balance', { method: 'POST', body: { from_date, to_date, group: group || null, rpt, printer } });
/** Is the Crystal Reports bridge set up and answering, and the printers of the PC it runs on. */
export const getPrintBridge = () => request<{ configured: boolean; ok: boolean; message: string; version?: string; printers?: string[] }>('/print/bridge');
export const emailPrint = (body: PrintRequest, to: string[]) => request<{ sent: boolean; to: string[] }>('/print/email', { method: 'POST', body: { ...body, to } });

// ─── Other Settings (routers/utilities.py: /utilities/doc-settings, /numbering, /menu) ───────────────

export type ApiDocSettings = {
  after_save: 'none' | 'prompt' | 'preview'; default_format: string; allow_duplicate_items: boolean; allow_edit_date: boolean;
  edit_days: number; allow_copy_from: boolean; update_rate_from_pb: boolean; screen: string; prints: boolean; affects_stock: boolean;
};
export type ApiNumbering = { kind: string; screen: string; prefix: string | null; default_prefix: string | null; next_number: number; example: string; from_counter: boolean };
export type ApiMenuLayout = { hidden: string[]; codes: Record<string, string> };
export const getDocSettingsAdmin = () => request<Record<string, ApiDocSettings>>('/utilities/doc-settings');
export const saveDocSettings = (kind: string, body: Partial<ApiDocSettings>) => request<ApiDocSettings>(`/utilities/doc-settings/${kind}`, { method: 'PUT', body });
export const getNumbering = () => request<ApiNumbering[]>('/utilities/numbering');
export const saveNumbering = (kind: string, body: { prefix?: string; next_number?: number }) => request<ApiNumbering>(`/utilities/numbering/${kind}`, { method: 'PUT', body });
export const getMenuAdmin = () => request<ApiMenuLayout>('/utilities/menu');
export const saveMenuAdmin = (body: ApiMenuLayout) => request<ApiMenuLayout>('/utilities/menu', { method: 'PUT', body });

// ─── Print Settings (routers/utilities.py: /utilities/print-settings) ───────────────────────────────

export type ApiPrintOption = {
  doc_id: number; order: number; name: string; rpt: string; selection: string; copies: number; default: boolean; inactive: boolean;
  click: string; ctrl_click: string; ctrl_alt_click: string; after_save: string; printer: string; multi_rpt: boolean;
};
export type ApiPrintSettings = {
  documents: { doc_id: number; name: string; short_form: string; options: ApiPrintOption[] }[];
  clicks: Record<string, string>; after_save: Record<string, string>;
};
export const getPrintSettings = () => request<ApiPrintSettings>('/utilities/print-settings');
export const savePrintOption = (docId: number, order: number, body: Partial<Omit<ApiPrintOption, 'doc_id' | 'order' | 'rpt' | 'selection' | 'multi_rpt'>>) =>
  request<ApiPrintOption>(`/utilities/print-settings/${docId}/${order}`, { method: 'PUT', body });

// ─── Admin Master (routers/auth.py: /admin/*) ──────────────────────────────────────────────────────

export const getAdminUsers = () => request<ApiAdminUser[]>('/admin/users');
export const createAdminUser = (body: unknown) => request<unknown>('/admin/users', { method: 'POST', body });
export const updateAdminUser = (user: string, body: unknown) => request<unknown>(`/admin/users/${encodeURIComponent(user)}`, { method: 'PUT', body });
export const deleteAdminUser = (user: string) => request<void>(`/admin/users/${encodeURIComponent(user)}`, { method: 'DELETE' });

export const getRoles = () => request<ApiRole[]>('/admin/roles');
export const saveAdminRole = (name: string, body: unknown) => request<unknown>(`/admin/roles/${encodeURIComponent(name)}`, { method: 'PUT', body });
export const deleteAdminRole = (name: string) => request<void>(`/admin/roles/${encodeURIComponent(name)}`, { method: 'DELETE' });

export const getLoginSettings = () => request<ApiLoginSettings>('/admin/login-settings');
export const saveLoginSettings = (body: unknown) => request<ApiLoginSettings>('/admin/login-settings', { method: 'PUT', body });
export const getSecurity = () => request<{ adnl_admin_set: boolean }>('/admin/security');
export const setAdditionalAdminPassword = (body: unknown) => request<{ adnl_admin_set: boolean }>('/admin/adnl-admin-password', { method: 'PUT', body });

export const getFieldRights = () => request<ApiFieldRightsAdmin>('/admin/field-rights');
export const saveFieldRights = (role: string, body: unknown) => request<unknown>(`/admin/field-rights/${encodeURIComponent(role)}`, { method: 'PUT', body });
export const getMasterApprovals = () => request<ApiMasterApproval>('/admin/master-approval');
export const saveMasterApprovals = (body: unknown) => request<ApiMasterApproval>('/admin/master-approval', { method: 'PUT', body });
export const approveMasterRow = (master: string, code: string) =>
  request<unknown>(`/admin/master-approval/${encodeURIComponent(master)}/${encodeURIComponent(code)}`, { method: 'POST' });
export const getReportRights = () => request<ApiReportRights>('/admin/report-rights');
export const saveReportRights = (role: string, body: unknown) => request<unknown>(`/admin/report-rights/${encodeURIComponent(role)}`, { method: 'PUT', body });

// ─── Utilities and settings (routers/utilities.py) ─────────────────────────────────────────────────

export const getAbout = () => request<ApiAbout>('/utilities/about');
export const setDatabaseConnection = (action: 'disconnect' | 'reconnect') => request<ApiAbout>(`/utilities/database/${action}`, { method: 'POST' });
export const refreshDocDesigner = () => request<unknown>('/designer/refresh', { method: 'POST' });

export const getWebSync = () => request<ApiWebSync>('/utilities/web-sync');
export const runWebSync = (body: unknown) => request<ApiWebSync>('/utilities/web-sync', { method: 'POST', body });

export const getAuditLog = (params: { from_date: string; to_date: string; screen?: string }) =>
  request<ApiAuditRow[]>(`/utilities/audit${qs(params)}`);
export const getAuditScreens = () => request<string[]>('/utilities/audit/screens');

export const listDuplicateInvoices = (params: Record<string, string | number | undefined>) =>
  request<ApiDupInvoice[]>(`/utilities/duplicate-invoices${qs(params)}`);
export const removeDuplicateInvoices = (body: { doc_nos: string[]; admin_password?: string }) =>
  request<{ removed: string[]; message: string }>('/utilities/duplicate-invoices/remove', { method: 'POST', body });

/** `mode` is 'merge' or 'replace' (Merge Items / Replace and Remove Duplicate items). */
export const previewMergeItems = (mode: string, fromItem: string, toItem: string) =>
  request<ApiMergePreview>(`/utilities/merge-items/${mode}/preview${qs({ from_item: fromItem, to_item: toItem })}`);
export const mergeItems = (mode: string, body: unknown) => request<ApiMergePreview>(`/utilities/merge-items/${mode}`, { method: 'POST', body });

export const getGlobalSettings = (all = false) => request<ApiSetting[]>(`/utilities/settings${all ? '?all=true' : ''}`);
export const saveGlobalSetting = (code: string, body: unknown) => request<unknown>(`/utilities/settings/${encodeURIComponent(code)}`, { method: 'PUT', body });
export const addGlobalSetting = (body: unknown) => request<ApiSetting>('/utilities/settings', { method: 'POST', body });
export const getCurrentStock = () => request<ApiStockSheet>('/utilities/compare-stock/current');
export const getMenuLayout = <T>() => request<T>('/settings/menu');
/** Global Settings the screens act on, by setting code. */
export const getSettingValues = () => request<Record<string, string>>('/settings/values');
export const getDocumentSettings = <T>() => request<Record<string, T>>('/settings/documents');

// ─── Tools (routers/tools.py) ──────────────────────────────────────────────────────────────────────

export const getSqlStatus = () => request<ApiSqlStatus>('/tools/sql/status');
export const getSqlObjects = (search: string, target: ApiSqlTarget) => request<ApiSqlObject[]>(`/tools/sql/objects${qs({ search, target })}`);
export const executeSql = (sql: string, target: ApiSqlTarget) => request<ApiSqlResult>('/tools/sql/execute', { method: 'POST', body: { sql, target } });
export const getSqlDefinition = (obj: { name: string; schema: string }, target: ApiSqlTarget) =>
  request<{ definition: string }>(`/tools/sql/definition${qs({ name: obj.name, schema: obj.schema, target })}`);
export const getSqlScript = (obj: { name: string; schema: string }, kind: string, target: ApiSqlTarget) =>
  request<{ script: string }>(`/tools/sql/script${qs({ name: obj.name, schema: obj.schema, kind, target })}`);
export const getSqlTop = (obj: { name: string; schema: string }, n: number, target: ApiSqlTarget) =>
  request<ApiSqlTop>(`/tools/sql/top${qs({ name: obj.name, schema: obj.schema, n, target })}`);

export const getRewriteStock = () => request<ApiRewriteCheck>('/tools/rewrite-stock');
export const runRewriteStock = () => request<ApiRewriteResult>('/tools/rewrite-stock', { method: 'POST' });

// ─── The hosted central site (backend/app/central, central.html): its own backend, see CENTRAL_API_BASE ─

const central = { base: CENTRAL_API_BASE };
export const centralLogin = <T>(body: { user: string; password: string }) => request<T>('/auth/login', { method: 'POST', body, ...central });
export const centralMe = <T>() => request<T>('/auth/me', central);
export const centralChangePassword = <T>(body: unknown) => request<T>('/auth/change-password', { method: 'POST', body, ...central });
export const centralShops = <T>() => request<T[]>('/clients', central);
export const centralStock = <T>(params: URLSearchParams) => request<T>(`/stock?${params}`, central);
export const centralDsr = <T>(params: URLSearchParams) => request<T>(`/dsr?${params}`, central);
export const centralIndents = <T>(params: URLSearchParams) => request<T[]>(`/indents?${params}`, central);

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
  fin_years?: { start: string; end: string; active: boolean }[];   // from GET /masters/company (MstCompFinYear)
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

// Tools → SQL Query Execution (backend/app/routers/tools.py): the vLQMS SQL Server (changes only to
// tables). "target" is kept in the requests; SQL Server is the only one.
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

export type ApiAbout = {
  product: string; software_version: string; api_version: string; company: string; db_connected: boolean; server_time: string;
  db_configured: boolean; db_disconnected_for_testing: boolean;
  designer: { screens: number; print_options: number; fields: number }; rpt_bridge: boolean; rpt_files: number;
  /** The database in use; shown to everyone. */
  database: string;
  /** Server, port, login and driver: only an Administrator gets them (never the password). */
  connection: { server: string; port: string; database: string; auth: string; user: string; driver: string; edition: string; version: string } | null;
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

// The sign-in page's Database setup (backend routers/setup.py).
export type ApiSetupStatus = { configured: boolean; connected: boolean; needs_setup: boolean; needs_admin: boolean };
export type ApiSetupServer = { host: string; port: string; auth: 'sql'; user: string; password: string };
// GET /auth/transport-key: the RSA public key (SPKI, base64) passwords are encrypted with, and the server's clock.
export type ApiTransportKey = { alg: string; spki: string; kid: string; server_time: number };
