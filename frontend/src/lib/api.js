













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
export const setRememberSession = (on) => { rememberSession = on; };
let authUser = '';
let authRole = '';
let authToken = readToken();

export const setAuthToken = (token) => {
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
export const requestTokenFromOtherTabs = (waitMs = 400) => new Promise(resolve => {
  if (!TAB_CHANNEL) return resolve('');
  const done = (token) => { clearTimeout(timer); TAB_CHANNEL.removeEventListener('message', onMsg); resolve(token); };
  const onMsg = (e) => { if (e.data?.type === 'token' && e.data.token) done(String(e.data.token)); };
  const timer = setTimeout(() => done(''), waitMs);
  TAB_CHANNEL.addEventListener('message', onMsg);
  TAB_CHANNEL.postMessage({ type: 'need-token' });
});

// Called by App.jsx after login (and with empty strings on logout). Every later api() call sends these.
export const setAuthUser = (user, role = '', token) => {
  authUser = user;
  authRole = role;
  if (token !== undefined) {
    setAuthToken(token);
  }
};
export const getAuthUser = () => authUser;

// The logged-in role's modules and Add/Edit/Delete ticks (set by App.jsx at login), for hiding buttons.
// The backend enforces the same rights; this only keeps the screen honest.
let authModules = [];
let authRights = {};
export const setAuthRights = (modules, rights = {}) => { authModules = modules; authRights = rights; };
// Same list as backend auth.COST_MODULES: roles with one of these see purchase costs and profit.
const COST_MODULES = ['Purchase Bill', 'Purchase Indent', 'Profit Margin', 'Price List Maintenance', 'Stock Ageing', 'Reports Dashboard'];
export const canSeeCost = () => authModules.includes('*') || COST_MODULES.some(m => authModules.includes(m));
export const canDo = (module, action) =>
  (authModules.includes('*') || authModules.includes(module)) && authRights[module]?.[action] !== false;
export const getAuthRole = () => authRole;

// Field Level Rights of the signed-in user's role (Admin Master (Security)): master path -> field -> mode.
// MasterCrud hides "hidden" fields and locks "readonly" ones; the backend enforces the same.
let authFieldRights = {};
export const setFieldRights = (rights) => { authFieldRights = rights ?? {}; };
export const fieldRight = (master, field) => authFieldRights[master]?.[field];

// App.jsx registers this: a 401 from any call except login means the session is over (expired token,
// password changed, user deactivated), so the app returns to the sign-in page with the server's reason.
let onSessionEnded = null;
export const setSessionEndedHandler = (handler) => { onSessionEnded = handler; };

// ─── Where the backend is, and how a request is sent ──────────────────────────────────────────────

// The backend's address: VITE_API_URL in frontend/.env (e.g. http://localhost:8001). The backend allows every origin
// (CORS), so the browser calls it directly. Without it, every call goes to /api/<path> on this page's own address
// (Vite's dev proxy, or the backend serving the built frontend).
const cleanBase = (v) => String(v ?? '').trim().replace(/\/+$/, '');
export const API_BASE = cleanBase(import.meta.env.VITE_API_URL) || '/api';
// The hosted central site (central.html) has its own backend: VITE_CENTRAL_API_URL, else the same rule as above.
export const CENTRAL_API_BASE = cleanBase(import.meta.env.VITE_CENTRAL_API_URL) || '/api';



const unreachable = (base) =>
  new ApiError(`Backend is not reachable at ${base.startsWith('http') ? base : 'this site\'s /api'}. Start it with: cd backend && .venv/bin/uvicorn app.main:app --reload --port 8001`);

async function send(path, init) {
  const base = init.base ?? API_BASE;
  try {
    const headers = {
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
async function request(path, init = {}) {
  const res = await send(path, init);
  if (res.status === 204) { announceDocChange(path, init, res); return undefined; }
  const data = await res.json().catch(() => null);
  if (!res.ok) failed(res, path, data);
  announceDocChange(path, init, res);
  return data ;
}

/** Sent on `window` after a document is saved, cancelled or deleted (any write under /transactions/), so lists such as the
 *  screens' History column can refresh themselves. */
export const DOCS_CHANGED = 'vlqms-docs-changed';
function announceDocChange(path, init, res) {
  if (res.ok && init.method && init.method !== 'GET' && path.startsWith('/transactions/') && typeof window !== 'undefined') {
    window.dispatchEvent(new Event(DOCS_CHANGED));
  }
}

// A request whose answer is not always JSON (a PDF for a print preview, or JSON when it was sent to a printer).
// The caller reads the Response; a failure is thrown as ApiError like request().
async function requestRaw(path, init = {}) {
  const res = await send(path, init);
  if (!res.ok) failed(res, path, await res.json().catch(() => null));
  return res;
}

// Throw the server's message as ApiError (and end the session on a 401).
function failed(res, path, data) {
  // FastAPI sends { detail: string } for HTTPException and { detail: [{ msg }] } for validation errors.
  const detail = data?.detail;
  const message = Array.isArray(detail) ? detail.map((d) => d.msg).join('; ') : detail ;
  if (res.status === 401 && path !== '/auth/login' && authToken) onSessionEnded?.(message || 'Please log in again.');
  throw new ApiError(message || `Request failed (${res.status})`);
}

// A fresh key for one draft document (see request's idempotencyKey).
export const newIdempotencyKey = () =>
  globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`;

// Message text for any caught error, ready for a toast.
export const errorText = (e) => (e instanceof Error ? e.message : String(e));

// Today as YYYY-MM-DD, the date format every backend date parameter expects. The PC's own date, not UTC's
// (toISOString would give yesterday's date in India before 05:30).
export const todayIso = () => {
  const d = new Date();
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
};

// Build a query string, dropping empty values.
export const qs = (params) => {
  const p = new URLSearchParams();
  Object.entries(params).forEach(([k, v]) => { if (v !== undefined && v !== '') p.set(k, String(v)); });
  const s = p.toString();
  return s ? `?${s}` : '';
};

// ═══ The backend's functions ═════════════════════════════════════════════════════════════════════════
// One function per backend endpoint (backend/app/main.py and app/routers/*). Screens import these and never build
// a URL themselves. The response types are at the end of this file.

// ─── Sign-in, session and Database setup (routers/auth.py, routers/setup.py) ───────────────────────

export const login = (body) => request('/auth/login', { method: 'POST', body });
export const getMe = () => request('/auth/me');
export const changePassword = (body) =>
  request('/auth/change-password', { method: 'POST', body });
/** The server's RSA public key every password is encrypted with (lib/secureField.js). */
export const getTransportKey = () => request('/auth/transport-key');
/** Password rules and "Remember me" for the sign-in page (public). */
export const getPublicLoginSettings = () => request('/auth/login-settings');

export const getSetupStatus = () => request('/setup/status');
/** The connection settings in use (no password), to fill the Database settings form. */
 
export const getSetupCurrent = () => request('/setup/current');
/** Where the .rpt print formats are read from (RPT_FOLDER), and what is in that folder. Same access as the database settings. */
export const getRptFolder = () => request('/setup/rpt-folder');
/** Save the .rpt folder on the server (blank = the default, backend/Rpts); it is used at once. */
export const saveRptFolder = (folder) => request('/setup/rpt-folder', { method: 'POST', body: { folder } });
/** The printer all direct prints use on the server (blank = each document's own), and the printers the server can see. */
export const getPrintPrinter = () => request('/setup/print-printer');
export const savePrintPrinter = (printer) => request('/setup/print-printer', { method: 'POST', body: { printer } });
/** The e-mail (SMTP) settings (no password: only whether one is saved), saved, and a connection test (nothing is sent). */
export const getSmtp = () => request('/setup/smtp');
export const saveSmtp = (body) => request('/setup/smtp', { method: 'POST', body });
export const testSmtp = (body) => request('/setup/smtp/test', { method: 'POST', body });
export const setupTest = (body) =>
  request('/setup/test', { method: 'POST', body });
export const setupConnect = (body) =>
  request('/setup/connect', { method: 'POST', body });
export const createAdmin = (body) =>
  request('/setup/admin', { method: 'POST', body });

// ─── Masters (routers/masters.py) ──────────────────────────────────────────────────────────────────

/** Every row of a master: `path` is the master's route, e.g. 'items', 'sales-counters'. */
export const listMaster = (path) => request(`/masters/${path}`);
export const createMaster = (path, body) => request(`/masters/${path}`, { method: 'POST', body });
export const updateMaster = (path, code, body) =>
  request(`/masters/${path}/${encodeURIComponent(code)}`, { method: 'PUT', body });
export const deleteMaster = (path, code) => request(`/masters/${path}/${encodeURIComponent(code)}`, { method: 'DELETE' });

export const getItems = () => listMaster('items');
export const getBrands = () => listMaster('brands');
export const getItemGroups = () => listMaster('item-groups');
export const getCustomers = () => listMaster('customers');
export const getSuppliers = () => listMaster('suppliers');
export const getSalesCounters = () => listMaster('sales-counters');
export const getLedgers = () => listMaster('ledgers');
export const getSalesPersons = () => request('/sales-persons');
export const importItems = (body) => request('/masters/items/import', { method: 'POST', body });
export const bulkUpdateItems = (body) =>
  request('/masters/items/bulk-update', { method: 'POST', body });
export const updatePriceList = (body) => request('/masters/items/price-list', { method: 'POST', body });

export const getCompany = () => request('/masters/company');
export const saveCompany = (body) => request('/masters/company', { method: 'PUT', body });
export const getCompanyHours = () => request('/company/hours');

// Company Master's pictures and documents (routers/company_files.py).
export const listCompanyFiles = (kind) => request(`/company/files${qs({ kind })}`);
export const uploadCompanyFile = (body) => request('/company/files', { method: 'POST', body });
export const updateCompanyFile = (id, body) => request(`/company/files/${id}`, { method: 'PUT', body });
export const deleteCompanyFile = (id) => request(`/company/files/${id}`, { method: 'DELETE' });
/** Where a company file's content is served, for <img src> and links (the browser can't send the token there). */
export const companyFileUrl = (id, download = false) => `${API_BASE}/company/files/${id}/content${download ? '?download=true' : ''}`;

// ─── Documents (routers/transactions.py) ───────────────────────────────────────────────────────────

/** The number the next document of a kind will most likely get (a preview; the backend numbers on save). */
export const getNextNumber = (kind, counter) => request(`/transactions/next-number${qs({ kind, counter })}`);

const salesInvoicePath = (docNo) => `/transactions/sales-invoice/${docNo}`;
export const listSalesInvoices = (status) => request(`/transactions/sales-invoice${qs({ status })}`);
export const saveSalesInvoice = (body, idempotencyKey) =>
  request('/transactions/sales-invoice', { method: 'POST', body, idempotencyKey });
export const reprintSalesInvoice = (docNo) => request(`${salesInvoicePath(docNo)}/reprint`);
export const cancelSalesInvoice = (docNo, remarks) =>
  request(`${salesInvoicePath(docNo)}/cancel`, { method: 'POST', body: { remarks } });
/** Remove a held bill (recalling it deletes the hold). */
export const deleteSalesInvoice = (docNo) => request(salesInvoicePath(docNo), { method: 'DELETE' });

/** The documents of one kind (sales_invoice, purchase_bill, purchase_indent, sales_return, stock_deduction, expense, closing_stock). */
export const listDocuments = (kind, params = {}) => request(`/transactions/${kind.replaceAll('_', '-')}${qs(params)}`);
export const getPurchaseIndents = () => request('/transactions/purchase-indent');
export const savePurchaseBill = (body, idempotencyKey) =>
  request('/transactions/purchase-bill', { method: 'POST', body, idempotencyKey });
/** Save a stock document (Purchase Indent, Stock Deduction, Sales Return) at its own endpoint, e.g. '/transactions/stock-deduction'. */
export const saveStockDocument = (endpoint, body) => request(endpoint, { method: 'POST', body });
export const saveExpense = (body) => request('/transactions/expense', { method: 'POST', body });

export const loadClosingStock = (asOn) => request(`/transactions/closing-stock/load${qs({ as_on: asOn })}`);
export const saveClosingStock = (body) => request('/transactions/closing-stock', { method: 'POST', body });
export const generateClosingStockInvoice = (docNo, body) =>
  request(`/transactions/closing-stock/${docNo}/generate-invoice`, { method: 'POST', body });

export const getDenomination = (on) => request(`/transactions/denomination${qs({ on })}`);
export const saveDenomination = (body) => request('/transactions/denomination', { method: 'POST', body });

// ─── Reports (routers/reports.py, routers/custom_reports.py) ───────────────────────────────────────

export const getDsr = (on = todayIso()) => request(`/reports/dsr${qs({ on })}`);
export const getDailySales = (fromDate, toDate) => request(`/reports/sales/daily${qs({ from_date: fromDate, to_date: toDate })}`);
export const getStockMovement = (on = todayIso()) => request(`/reports/stock-movement${qs({ on })}`);
export const getRestock = (days, limit) => request(`/reports/restock${qs({ days, limit })}`);
export const getStockBalance = (date) => request(`/reports/stock-balance${qs({ from_date: date, to_date: date })}`);
/** Any table report: `endpoint` and its filters come from components/reports/reportDefs.js. */
export const getReport = (endpoint, params = {}) => request(`${endpoint}${qs(params)}`);
export const getRptFiles = (category) => request(`/reports/rpt-files${qs({ category })}`);

export const getCustomReportRights = () => request('/reports/custom/allowed');
/** A Customised Report through the Crystal file in its definition: the PDF (with a printer: printed there, JSON back). */
export const renderCustomReportPrint = (body, printer = '') =>
  requestRaw('/print/custom-report', { method: 'POST', body: { ...body, printer } });
/** `query` is the already-built query string of the filters (without the '?'). */
export const runCustomReport = (query) => request(`/reports/custom?${query}`);
export const emailCustomReport = (body) => request('/reports/custom/email', { method: 'POST', body });

// ─── Printing (routers/printing.py) ────────────────────────────────────────────────────────────────

export const getPrintOptions = (kind) => request(`/print/options${qs({ kind })}`);
/** The PDF (or, for a direct print, JSON) the server built from the document's Crystal .rpt format. */
export const renderPrint = (body) => requestRaw('/print/render', { method: 'POST', body });
/** Remember the print option this user picked for a document. */
export const setPrintPreference = (kind, option) => request('/print/preference', { method: 'PUT', body: { kind, option } });
/** The Daily Summary Report's .rpt formats that are in the report folder. */
export const getDsrFormats = () => request('/print/dsr-formats');
/** The Daily Summary Report of a day as a PDF laid out by the desktop's RptDSR.rpt (rpt empty = RptDSR.rpt). */
export const renderDsrPrint = (on, rpt, printer = '') => requestRaw('/print/dsr', { method: 'POST', body: { on, rpt, printer } });
/** The Stock Balance .rpt formats that are in the report folder. */
export const getStockBalanceFormats = () => request('/print/stock-balance-formats');
/** The Stock Balance report of a period as a PDF laid out by the desktop's RptStkBal*.rpt (with a printer: printed there, JSON back). */
export const renderStockBalancePrint = (from_date, to_date, group, rpt, printer = '') =>
  requestRaw('/print/stock-balance', { method: 'POST', body: { from_date, to_date, group: group || null, rpt, printer } });
/** Is the Crystal Reports bridge set up and answering, and the printers of the PC it runs on. */
export const getPrintBridge = () => request('/print/bridge');
export const emailPrint = (body, to) => request('/print/email', { method: 'POST', body: { ...body, to } });

// ─── Other Settings (routers/utilities.py: /utilities/doc-settings, /numbering, /menu) ───────────────

 





export const getDocSettingsAdmin = () => request('/utilities/doc-settings');
export const saveDocSettings = (kind, body) => request(`/utilities/doc-settings/${kind}`, { method: 'PUT', body });
export const getNumbering = () => request('/utilities/numbering');
export const saveNumbering = (kind, body) => request(`/utilities/numbering/${kind}`, { method: 'PUT', body });
export const getMenuAdmin = () => request('/utilities/menu');
export const saveMenuAdmin = (body) => request('/utilities/menu', { method: 'PUT', body });

// ─── Print Settings (routers/utilities.py: /utilities/print-settings) ───────────────────────────────

 







export const getPrintSettings = () => request('/utilities/print-settings');
export const savePrintOption = (docId, order, body) =>
  request(`/utilities/print-settings/${docId}/${order}`, { method: 'PUT', body });

// ─── Admin Master (routers/auth.py: /admin/*) ──────────────────────────────────────────────────────

export const getAdminUsers = () => request('/admin/users');
export const createAdminUser = (body) => request('/admin/users', { method: 'POST', body });
export const updateAdminUser = (user, body) => request(`/admin/users/${encodeURIComponent(user)}`, { method: 'PUT', body });
export const deleteAdminUser = (user) => request(`/admin/users/${encodeURIComponent(user)}`, { method: 'DELETE' });

export const getRoles = () => request('/admin/roles');
export const saveAdminRole = (name, body) => request(`/admin/roles/${encodeURIComponent(name)}`, { method: 'PUT', body });
export const deleteAdminRole = (name) => request(`/admin/roles/${encodeURIComponent(name)}`, { method: 'DELETE' });

export const getLoginSettings = () => request('/admin/login-settings');
export const saveLoginSettings = (body) => request('/admin/login-settings', { method: 'PUT', body });
export const getSecurity = () => request('/admin/security');
export const setAdditionalAdminPassword = (body) => request('/admin/adnl-admin-password', { method: 'PUT', body });

export const getFieldRights = () => request('/admin/field-rights');
export const saveFieldRights = (role, body) => request(`/admin/field-rights/${encodeURIComponent(role)}`, { method: 'PUT', body });
export const getMasterApprovals = () => request('/admin/master-approval');
export const saveMasterApprovals = (body) => request('/admin/master-approval', { method: 'PUT', body });
export const approveMasterRow = (master, code) =>
  request(`/admin/master-approval/${encodeURIComponent(master)}/${encodeURIComponent(code)}`, { method: 'POST' });
export const getReportRights = () => request('/admin/report-rights');
export const saveReportRights = (role, body) => request(`/admin/report-rights/${encodeURIComponent(role)}`, { method: 'PUT', body });

// ─── Utilities and settings (routers/utilities.py) ─────────────────────────────────────────────────

export const getAbout = () => request('/utilities/about');
export const setDatabaseConnection = (action) => request(`/utilities/database/${action}`, { method: 'POST' });
export const refreshDocDesigner = () => request('/designer/refresh', { method: 'POST' });

export const getWebSync = () => request('/utilities/web-sync');
export const runWebSync = (body) => request('/utilities/web-sync', { method: 'POST', body });

export const getAuditLog = (params) =>
  request(`/utilities/audit${qs(params)}`);
export const getAuditScreens = () => request('/utilities/audit/screens');

export const listDuplicateInvoices = (params) =>
  request(`/utilities/duplicate-invoices${qs(params)}`);
export const removeDuplicateInvoices = (body) =>
  request('/utilities/duplicate-invoices/remove', { method: 'POST', body });

/** `mode` is 'merge' or 'replace' (Merge Items / Replace and Remove Duplicate items). */
export const previewMergeItems = (mode, fromItem, toItem) =>
  request(`/utilities/merge-items/${mode}/preview${qs({ from_item: fromItem, to_item: toItem })}`);
export const mergeItems = (mode, body) => request(`/utilities/merge-items/${mode}`, { method: 'POST', body });

export const getGlobalSettings = (all = false) => request(`/utilities/settings${all ? '?all=true' : ''}`);
export const saveGlobalSetting = (code, body) => request(`/utilities/settings/${encodeURIComponent(code)}`, { method: 'PUT', body });
export const addGlobalSetting = (body) => request('/utilities/settings', { method: 'POST', body });
export const getCurrentStock = () => request('/utilities/compare-stock/current');
export const getMenuLayout = () => request('/settings/menu');
/** Global Settings the screens act on, by setting code. */
export const getSettingValues = () => request('/settings/values');
export const getDocumentSettings = () => request('/settings/documents');

// ─── Tools (routers/tools.py) ──────────────────────────────────────────────────────────────────────

export const getSqlStatus = () => request('/tools/sql/status');
export const getSqlObjects = (search, target) => request(`/tools/sql/objects${qs({ search, target })}`);
export const executeSql = (sql, target) => request('/tools/sql/execute', { method: 'POST', body: { sql, target } });
export const getSqlDefinition = (obj, target) =>
  request(`/tools/sql/definition${qs({ name: obj.name, schema: obj.schema, target })}`);
export const getSqlScript = (obj, kind, target) =>
  request(`/tools/sql/script${qs({ name: obj.name, schema: obj.schema, kind, target })}`);
export const getSqlTop = (obj, n, target) =>
  request(`/tools/sql/top${qs({ name: obj.name, schema: obj.schema, n, target })}`);

export const getRewriteStock = () => request('/tools/rewrite-stock');
export const runRewriteStock = () => request('/tools/rewrite-stock', { method: 'POST' });

// ─── The hosted central site (backend/app/central, central.html): its own backend, see CENTRAL_API_BASE ─

const central = { base: CENTRAL_API_BASE };
export const centralLogin = (body) => request('/auth/login', { method: 'POST', body, ...central });
export const centralMe = () => request('/auth/me', central);
export const centralChangePassword = (body) => request('/auth/change-password', { method: 'POST', body, ...central });
export const centralShops = () => request('/clients', central);
export const centralStock = (params) => request(`/stock?${params}`, central);
export const centralDsr = (params) => request(`/dsr?${params}`, central);
export const centralIndents = (params) => request(`/indents?${params}`, central);

// Shapes returned by the backend (snake_case, as FastAPI sends them).
 






















































































































































































































