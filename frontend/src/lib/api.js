













import { noteDataVersion } from './dataVersion';

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
    const res = await fetch(`${base}${path}`, {
      method: init.method ?? 'GET',
      headers,
      body: init.body === undefined ? undefined : JSON.stringify(init.body),
    });
    noteDataVersion(res.headers?.get?.('X-Data-Version'));   // the master data changed outside the app: open screens read again
    return res;
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
/** Connect a shared printer (\\computer\printer) for the Windows user the server runs as; answers like getPrintPrinter(). */
export const addSharedPrinter = (share) => request('/setup/print-printer/add', { method: 'POST', body: { share } });
/** The e-mail (SMTP) settings (no password: only whether one is saved), saved, and a connection test (nothing is sent). */
export const getSmtp = () => request('/setup/smtp');
export const saveSmtp = (body) => request('/setup/smtp', { method: 'POST', body });
export const testSmtp = (body) => request('/setup/smtp/test', { method: 'POST', body });
// A master's code inside a URL path. Any character may be in a code: "/" can't travel in a path (the server reads %2F as a slash before routing),
// so "~" becomes "~7E" and "/" becomes "~2F" first (backend app/codeseg.py gives it back); the rest is ordinary URL-encoding.
export const seg = (code) => encodeURIComponent(String(code).replace(/~/g, '~7E').replace(/[/]/g, '~2F'));
/** Every SQL Server instance found (any version). `host` limits it to one server. */
export const getSetupInstances = (host = '') => request(`/setup/instances${host ? `?host=${encodeURIComponent(host)}` : ''}`);
/** Try the login on every instance of one server and list each one's databases. */
export const setupScan = (body) => request('/setup/scan', { method: 'POST', body });
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
  request(`/masters/${path}/${seg(code)}`, { method: 'PUT', body });
export const deleteMaster = (path, code) => request(`/masters/${path}/${seg(code)}`, { method: 'DELETE' });

export const getItems = () => listMaster('items');
export const getBrands = () => listMaster('brands');
export const getItemGroups = () => listMaster('item-groups');
export const getCustomers = () => listMaster('customers');
export const getSuppliers = () => listMaster('suppliers');
export const getSalesCounters = () => listMaster('sales-counters');
export const getLedgers = () => listMaster('ledgers');
export const getSalesPersons = () => request('/sales-persons');
// Item Master options (frmMstItem): items with a similar code, the Items-DataLink Problem list,
// Update Item Master Fields (field list, load rows, import) and Update Item Rates.
export const getSimilarMasters = (path, code, exclude = '') => request(`/masters/${path}/similar${qs({ code, exclude })}`);   // -> [{ code, name }]
export const getInvalidItems = () => request('/masters/items/invalid');   // -> { columns, rows, note }
export const getBulkFieldDefs = () => request('/masters/items/bulk-fields');   // -> [{ code, datatype }]
export const loadBulkFields = (body) => request('/masters/items/bulk-fields/load', { method: 'POST', body });   // -> { columns, rows }
export const importBulkFields = (rows) => request('/masters/items/bulk-fields/import', { method: 'POST', body: { rows } });   // -> { updated }
export const recalculateItemRates = () => request('/masters/items/recalculate-rates', { method: 'POST' });   // -> { fields, message }
// Arrange Group Slno / Arrange Pkg Slno: [{ id, slno, code, litres? }] and the new order (the IDs in order).
export const getMasterOrder = (path, sort = '') => request(`/masters/${path}/order${qs({ sort })}`);
export const saveMasterOrder = (path, ids) => request(`/masters/${path}/order`, { method: 'POST', body: { ids } });
// Import Item Data From Excel: POST the sheet's rows. { dryRun: true } only checks them -> { rows: [{ error }] } (one per row); otherwise
// -> { added, duplicates }. `key` makes a repeated send of the same sheet a no-op.
export const importItems = (body, { dryRun = false, overwrite = false, key } = {}) =>
  request(`/masters/items/import${qs({ dry_run: dryRun ? 'true' : '', overwrite: overwrite ? 'true' : '' })}`, { method: 'POST', body, idempotencyKey: key });
export const bulkUpdateItems = (body) =>
  request('/masters/items/bulk-update', { method: 'POST', body });
export const updatePriceList = (body) => request('/masters/items/price-list', { method: 'POST', body });
// The Item Rate Columns of Price List Maintenance (the Item Master's numeric designer fields): [{ key, caption, typed }].
export const getRateColumns = () => request('/masters/items/rate-columns');
// Update to DB: [{ item_code, values: { <column key>: new value } }] the new rates themselves -> { items, changed }.
export const setItemRates = (rows) => request('/masters/items/price-set', { method: 'POST', body: { rows } });

export const getCompany = () => request('/masters/company');
export const saveCompany = (body) => request('/masters/company', { method: 'PUT', body });
/** The dates offered for a new financial year (a year after the last one). */
/** Company Master: BACKUP DATABASE into the folder set in Global Settings (runs in the background; poll the status). */
export const startBackup = () => request('/utilities/backup', { method: 'POST', body: {} });
export const getBackupStatus = () => request('/utilities/backup');
export const updateDashboardLink = (link) => request('/masters/company/dashboard-link', { method: 'PUT', body: { link } });
/** A customer's or supplier's contact persons / addresses / alert messages / notes (`kind`: contacts, addresses, alerts, activities). */
export const getPartyRecords = (party, code, kind) => request(`/masters/${party}/${seg(code)}/${kind}`);
export const savePartyRecords = (party, code, kind, rows) => request(`/masters/${party}/${seg(code)}/${kind}`, { method: 'PUT', body: { rows } });
/** Customer / Supplier Vs Item: the exe's item pick list (active, authorised items, one line per unit) and the items attached per party code. */
export const searchPartyItems = (party, q) => request(`/masters/party-item-search/${party}${qs({ q })}`);
export const getPartyItemCounts = (party) => request(`/masters/party-item-counts/${party}`);
export const addPartyNote = (party, code, note) => request(`/masters/${party}/${seg(code)}/activities`, { method: 'POST', body: { rows: [{ Notes: note }] } });
export const getNextFinYear = () => request('/masters/company/fin-years/next');
export const createFinYear = (body) => request('/masters/company/fin-years', { method: 'POST', body });
export const activateFinYear = (start) => request(`/masters/company/fin-years/${start}/activate`, { method: 'PUT' });   // makes that year the active one; returns the company profile
export const removeFinYear =(start) => request(`/masters/company/fin-years/${start}`, { method: 'DELETE' });
// Stock carry forward into a year: the previous year's closing stock becomes its opening stock (Administrator).
// Preview: {from_year, to_year, items, lines, qty, value}; carry forward body {admin_password} (encrypted for "admin"): {from_year, to_year, lines, items, qty}.
export const previewStockCarryForward = (start) => request(`/masters/company/fin-years/${start}/carry-forward`);
export const carryForwardStock = (start, body) => request(`/masters/company/fin-years/${start}/carry-forward`, { method: 'POST', body });
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
// Quick Selection (the desktop's MstQuickSelItems): named sets of item codes. { sets: [{ name, codes: [...] }] }
export const getQuickSelections = () => request('/quick-selection');
export const saveQuickSelection = (name, codes) => request(`/quick-selection/${encodeURIComponent(name)}`, { method: 'PUT', body: { codes } });
export const deleteQuickSelection = (name) => request(`/quick-selection/${encodeURIComponent(name)}`, { method: 'DELETE' });
// PDF Generator: the chosen documents as one ZIP of PDFs (a Blob).
export const downloadBulkPdf = async (kind, docNos, option = '') => (await requestRaw('/print/bulk-pdf', { method: 'POST', body: { kind, doc_nos: docNos, option } })).blob();
// Desktop rights (Admin Master): the shop's MstUserRights / MstRoleRights. Overview -> { by, owners:[{id,name,detail}], menu:[{id,caption,type}], flags }.
export const getDesktopRightsOverview = () => request('/admin/desktop-rights');
export const getDesktopRights = (ownerId) => request(`/admin/desktop-rights/${ownerId}`);
export const saveDesktopRights = (ownerId, rows) => request(`/admin/desktop-rights/${ownerId}`, { method: 'PUT', body: { rows } });
// File Attachment on masters (master = the /masters path, e.g. 'items'): files kept by the web server.
export const listAttachments = (master, code) => request(`/attachments/${master}${qs({ code })}`);
export const uploadAttachment = (master, code, body) => request(`/attachments/${master}${qs({ code })}`, { method: 'POST', body });
export const deleteAttachment = (master, id) => request(`/attachments/${master}/${id}`, { method: 'DELETE' });
export const fetchAttachment = async (master, id, download = false) => (await requestRaw(`/attachments/${master}/${id}/content${qs({ download: download || undefined })}`)).blob();
// SMS (UtlMsgSettings / TrnSMS): providers + whether the shop has SMS on, sent history, and sending.
export const getSmsProviders = () => request('/sms/providers');
export const getSmsHistory = (from, to) => request(`/sms/history${qs({ from, to })}`);
// Admin Master → Login Settings, the desktop's own settings (UtlMisc): { available, policy, rules, never_expires, expiry_days, min_*, login_required, user_based }.
export const getDesktopLoginSettings = () => request('/admin/desktop-login-settings');
export const saveDesktopLoginSettings = (body) => request('/admin/desktop-login-settings', { method: 'PUT', body });
// Utilities → Key Generation (the vendor's tool): { available } and { reg_no, key } ("-" when the Reg. No. has no key). Administrator only.
export const getKeyGeneratorAvailable = () => request('/utilities/key-generator/available');
export const getLicenceKey = (regNo) => request(`/utilities/key-generator${qs({ reg_no: regNo })}`);
// Global Settings → Other Settings → Invoice Settings (frmUtlInvNumberFmt; UtlGSettings 'Invoice' rows and UtlMisc), Administrator only:
// { prefix: ''|'M'|'D', counter_reset, months: [12 letters], format: [15 cells], f12, ctrl_f12, alt_f12 (Save | Save and Print | Save and Preview | ''),
//   sms: { on_sales, on_cancel, on_delete, sales_value, sales_msg, cancel_msg, delete_msg }, update_rate, sms_available, sample: { prefix, counter, ... } }
export const getInvoiceKeys = () => request('/transactions/invoice-keys');   // { f12, ctrl_f12, alt_f12 }: 'S' save, 'SP' save and print, 'SPP' save and preview
export const getInvoiceSettings = () => request('/utilities/invoice-settings');
export const saveInvoiceSettings = (body) => request('/utilities/invoice-settings', { method: 'PUT', body });
// Global Settings → Other Settings → Delete Linkage Updation (UtlDeletion, Administrator only). A rule row: { table, column, display, caption }.
export const getLinkageTables = () => request('/utilities/delete-linkage/tables');
export const getLinkageColumns = (table) => request(`/utilities/delete-linkage/columns${qs({ table })}`);
export const getLinkageGroups = () => request('/utilities/delete-linkage/groups');   // [{ table, name }]
export const getLinkageGroup = (table) => request(`/utilities/delete-linkage/group${qs({ table })}`);   // { table, doc_name, doc_field, rows }
export const searchLinkage = (table, field) => request('/utilities/delete-linkage/search', { method: 'POST', body: { table, field } });
export const saveLinkageGroup = (table, body) => request(`/utilities/delete-linkage/group${qs({ table })}`, { method: 'PUT', body });   // body: { doc_name, doc_field, rows }
export const deleteLinkageGroup = (table) => request(`/utilities/delete-linkage/group${qs({ table })}`, { method: 'DELETE' });
export const sendSms = (body) => request('/sms/send', { method: 'POST', body });
// SMS settings (Administrator): the providers with their URLs and the invoice SMS settings (UtlMsgSettings / UtlMisc).
export const getSmsProviderRows = () => request('/sms/settings/providers');
export const saveSmsProvider = (id, body) => (id ? request(`/sms/settings/providers/${id}`, { method: 'PUT', body }) : request('/sms/settings/providers', { method: 'POST', body }));
export const deleteSmsProvider = (id) => request(`/sms/settings/providers/${id}`, { method: 'DELETE' });
export const testSmsProvider = (id, mobile) => request(`/sms/settings/providers/${id}/test`, { method: 'POST', body: { mobile } });
export const getSmsInvoiceSettings = () => request('/sms/settings/invoice');
export const saveSmsInvoiceSettings = (body) => request('/sms/settings/invoice', { method: 'PUT', body });
// The note and coin values of the cash count (UtlDenom): { values: ['500', …] }.
export const getDenominationConfig = () => request('/transactions/denominations/config');
// Item templates (MstItemTemp, the desktop's F6 Load Template): the same shape as the quick selections.
export const getItemTemplates = () => request('/item-templates');
export const saveItemTemplate = (name, codes) => request(`/item-templates/${encodeURIComponent(name)}`, { method: 'PUT', body: { codes } });
export const deleteItemTemplate = (name) => request(`/item-templates/${encodeURIComponent(name)}`, { method: 'DELETE' });
export const listSalesInvoices =(status, from_date, to_date) => request(`/transactions/sales-invoice${qs({ status, from_date, to_date })}`);
// The footer's Total Bills / Sales (App.jsx) listens for this and reloads after a bill is saved, changed or cancelled.
const billsChanged = (result) => { try { window.dispatchEvent(new Event('vlqms:bills-changed')); } catch { /* no window (tests) */ } return result; };
export const saveSalesInvoice = (body, idempotencyKey) =>
  request('/transactions/sales-invoice', { method: 'POST', body, idempotencyKey }).then(billsChanged);
export const reprintSalesInvoice = (docNo) => request(`${salesInvoicePath(docNo)}/reprint`);
export const cancelSalesInvoice = (docNo, remarks) =>
  request(`${salesInvoicePath(docNo)}/cancel`, { method: 'POST', body: { remarks } }).then(billsChanged);
/** Cancel a saved purchase bill, indent, sales return or stock deduction (the exe's deleteDoc, CANCELLED): its stock effect is taken back and it is marked cancelled. */
const CANCEL_PATHS = { closing_stock: 'closing-stock', expense: 'expense', purchase_bill: 'purchase-bill', purchase_indent: 'purchase-indent', sales_return: 'sales-return', stock_deduction: 'stock-deduction' };
export const cancelSavedDocument = (kind, docNo) => request(`/transactions/${CANCEL_PATHS[kind]}/${docNo}/cancel`, { method: 'POST' }).then(billsChanged);
export const deleteSavedSalesInvoice = (docNo) =>
  request(`${salesInvoicePath(docNo)}/delete`, { method: 'POST' }).then(billsChanged);
/** Remove a held bill (recalling it deletes the hold). */
export const deleteSalesInvoice = (docNo) => request(salesInvoicePath(docNo), { method: 'DELETE' });

/** The documents of one kind (sales_invoice, purchase_bill, purchase_indent, sales_return, stock_deduction, expense, closing_stock). */
export const getSalesInvoice = (docNo) => request(`/transactions/sales-invoice/${docNo}`);   // one saved bill, whole (as the screen edits it)
export const listDocuments =(kind, params = {}) => request(`/transactions/${kind.replaceAll('_', '-')}${qs(params)}`);
export const getPurchaseIndents = (from_date, to_date) => request(`/transactions/purchase-indent${qs({ from_date, to_date })}`);
export const listPurchaseBills = (from_date, to_date) => request(`/transactions/purchase-bill${qs({ from_date, to_date })}`);
/** Change a saved sales invoice / purchase bill in place (same number); the body is the one a new bill takes. */
export const updateSalesInvoice = (docNo, body) => request(`/transactions/sales-invoice/${docNo}`, { method: 'PUT', body }).then(billsChanged);
export const updatePurchaseBill = (docNo, body) => request(`/transactions/purchase-bill/${docNo}`, { method: 'PUT', body });
export const savePurchaseBill = (body, idempotencyKey) =>
  request('/transactions/purchase-bill', { method: 'POST', body, idempotencyKey });
/** Save a stock document (Purchase Indent, Stock Deduction, Sales Return) at its own endpoint, e.g. '/transactions/stock-deduction'. */
export const saveStockDocument = (endpoint, body) => request(endpoint, { method: 'POST', body });
export const saveExpense = (body) => request('/transactions/expense', { method: 'POST', body });
/** Change a saved Purchase Indent / Sales Return / Stock Deduction / Expense: `path` is its endpoint plus the document number. */
export const updateDocument = (path, body) => request(path, { method: 'PUT', body });

export const loadClosingStock = (asOn) => request(`/transactions/closing-stock/load${qs({ as_on: asOn })}`);
export const saveClosingStock = (body) => request('/transactions/closing-stock', { method: 'POST', body });
export const updateClosingStock = (docNo, body) => request(`/transactions/closing-stock/${docNo}`, { method: 'PUT', body }).then(billsChanged);
export const generateClosingStockInvoice = (docNo, body) =>
  request(`/transactions/closing-stock/${docNo}/generate-invoice`, { method: 'POST', body });

// Physical Stock Entry (desktop frmTrnPSE). load: active items with system_qty for groups / packages / items (comma lists);
// save posts only the changed rows: the excess becomes a purchase bill, the shortage a stock deduction (or a sales invoice).
export const loadPhysicalStock = (params) => request(`/transactions/physical-stock/load${qs(params)}`);
export const getPhysicalStockEntry = (docNo) => request(`/transactions/physical-stock/${docNo}`);
export const savePhysicalStock = (body, key) => request('/transactions/physical-stock', { method: 'POST', body, idempotencyKey: key });
// Accounting (desktop frmTrnVch and the FrmRptAcc* reports), read from and written to the shop's own tables.
// kind: payment | receipt | credit-note | debit-note | contra | journal. A voucher line is { ledger_id, side: 'Dr'|'Cr', amount }.
export const getAccountingLedgers = () => request('/accounting/ledgers');
export const listVouchers = (kind, from_date, to_date) => request(`/accounting/vouchers/${kind}${qs({ from_date, to_date })}`);
export const getVoucher = (kind, id) => request(`/accounting/vouchers/${kind}/${id}`);
export const saveVoucher = (kind, body) => request(`/accounting/vouchers/${kind}`, { method: 'POST', body });
export const updateVoucher = (kind, id, body) => request(`/accounting/vouchers/${kind}/${id}`, { method: 'PUT', body });
export const cancelVoucher = (kind, id) => request(`/accounting/vouchers/${kind}/${id}/cancel`, { method: 'POST' });
export const getGeneralLedger = (ledger_id, from_date, to_date) => request(`/accounting/general-ledger${qs({ ledger_id, from_date, to_date })}`);
export const getPartyOutstanding = (party) => request(`/accounting/outstanding${qs({ party })}`);
export const getBillWise = (ac_type) => request(`/accounting/bill-wise${qs({ ac_type })}`);
export const getBillAgeing = (ac_type, as_on) => request(`/accounting/ageing${qs({ ac_type, as_on })}`);
// Quotation (desktop document 1282, tables TrnHdrQTN / TrnDtlQTN). A line is { item_code, qty, rate? } (rate defaults to the MRP).
export const listQuotations = (from_date, to_date) => request(`/transactions/quotation${qs({ from_date, to_date })}`);
export const getQuotation = (id) => request(`/transactions/quotation/${id}`);
export const saveQuotation = (body) => request('/transactions/quotation', { method: 'POST', body });
export const updateQuotation = (id, body) => request(`/transactions/quotation/${id}`, { method: 'PUT', body });
export const cancelQuotation = (id) => request(`/transactions/quotation/${id}/cancel`, { method: 'POST' });
// Shared Documents (desktop frmUtlSharedDocuments, table UtlSharedDocs): [{ id, group, file, remarks }], read only.
export const getSharedDocuments = () => request('/utilities/shared-documents');
// Item Vs Brand (desktop frmUtlItemVsBrand): assignments [{item_code, brand}], an empty brand unlinks. -> { changed }
export const assignItemBrands = (assignments) => request('/masters/items/brands', { method: 'POST', body: { assignments } });
// Update Item Stock (desktop frmUpdateItemStock): rows [{item_code, stock, rate}]. dry_run true checks each row
// ({ ok, rows: [{row, error}], importable }); false posts one purchase bill of opening stock ({ doc_no, imported }).
export const updateItemStock = (body, idempotencyKey) => request('/utilities/update-item-stock', { method: 'POST', body, idempotencyKey });
// Expense Entry "Day's OP-Balance": { date, amount } cash in hand at the start of a day (desktop TrnExpOPBal).
export const getExpenseOpening = (on) => request(`/transactions/expense/opening-balance${qs({ on })}`);
export const saveExpenseOpening = (body) => request('/transactions/expense/opening-balance', { method: 'POST', body });

// ─── Reports (routers/reports.py, routers/custom_reports.py) ───────────────────────────────────────

/** The footer's Total Bills / Sales: today's bills in the database that are not cancelled or deleted, { total_bills, sales }. */
export const getTodayTotals = () => request('/transactions/today-totals');
export const getDsr = (on = todayIso()) => request(`/reports/dsr${qs({ on })}`);
export const getDailySales = (fromDate, toDate) => request(`/reports/sales/daily${qs({ from_date: fromDate, to_date: toDate })}`);
export const getStockMovement = (on = todayIso()) => request(`/reports/stock-movement${qs({ on })}`);
export const getRestock = (days, limit) => request(`/reports/restock${qs({ days, limit })}`);
/** The exe's Stock Balance views (closing / detailed / package / group): { columns: [{key, label, kind, total}], rows, brand_required }. */
export const getStockBalanceView = (params) => request(`/reports/stock-balance/view${qs(params)}`);
export const getStockBalance = (date) => request(`/reports/stock-balance${qs({ from_date: date, to_date: date })}`);
/** Any table report: `endpoint` and its filters come from components/reports/reportDefs.js. */
export const getReport = (endpoint, params = {}) => request(`${endpoint}${qs(params)}`);
export const getRptFiles = (category) => request(`/reports/rpt-files${qs({ category })}`);

export const getCustomReportRights = () => request('/reports/custom/allowed');
/** A Customised Report through the Crystal file in its definition: the PDF (with a printer: printed there, JSON back). */
export const renderCustomReportPrint = (body, printer = '') =>
  requestRaw('/print/custom-report', { method: 'POST', body: { ...body, printer } });
/** `query` is the already-built query string of the filters (without the '?'). */
/** The filter fields a Customised Report offers (its definition's FilterReq fields) and whether it takes a period. */
export const getCustomReportFilters = (name) => request(`/reports/custom/filters${qs({ name })}`);
/** The values to pick from for one of those fields. */
export const getCustomReportFilterValues = (name, field) => request(`/reports/custom/filter-values${qs({ name, field })}`);
export const runCustomReport = (query) => request(`/reports/custom?${query}`);
export const emailCustomReport = (body) => request('/reports/custom/email', { method: 'POST', body });

// ─── Printing (routers/printing.py) ────────────────────────────────────────────────────────────────

export const getPrintOptions = (kind) => request(`/print/options${qs({ kind })}`);
/** The PDF (or, for a direct print, JSON) the server built from the document's Crystal .rpt format. */
export const renderPrint = (body) => requestRaw('/print/render', { method: 'POST', body });
/** The Print Agent for a counter PC as a ZIP (a Blob), preset for this page's address. */
export const downloadPrintAgent = async (origin) => (await requestRaw(`/print/agent${qs({ origin })}`)).blob();
/** A document's report as page pictures {pages:[{png (base64), w, h (points)}], copies, name} for the Print Agent on this PC (lib/localPrint.js). */
export const renderPrintPages = (body) => request('/print/pages', { method: 'POST', body });
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
/** The barcode label formats (every RPTBC*.rpt) that are in the report folder. */
export const getBarcodeFormats = () => request('/print/barcode-formats');
/** Labels [{code, copies, qty, mrp, barcode}] laid out by the desktop's RPTBC*.rpt (with a printer: printed there, JSON back). */
export const renderBarcodePrint = (items, rpt, printer = '') => requestRaw('/print/barcode', { method: 'POST', body: { items, rpt, printer } });
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
// Add / Delete / Alter Order of a document's print options (the desktop's Print Settings buttons).
export const addPrintOption = (docId, body) => request(`/utilities/print-settings/${docId}`, { method: 'POST', body });
export const deletePrintOption = (docId, order) => request(`/utilities/print-settings/${docId}/${order}`, { method: 'DELETE' });
export const orderPrintOptions = (docId, names) => request(`/utilities/print-settings/${docId}`, { method: 'PUT', body: { names } });

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
  request(`/admin/master-approval/${encodeURIComponent(master)}/${seg(code)}`, { method: 'POST' });
export const getReportRights = () => request('/admin/report-rights');
export const saveReportRights = (role, body) => request(`/admin/report-rights/${encodeURIComponent(role)}`, { method: 'PUT', body });

// ─── Utilities and settings (routers/utilities.py) ─────────────────────────────────────────────────

export const getAbout = () => request('/utilities/about');
export const setDatabaseConnection = (action) => request(`/utilities/database/${action}`, { method: 'POST' });
export const refreshDocDesigner = () => request('/designer/refresh', { method: 'POST' });
// The top bar's "Refresh from Doc Designer" (Administrator): designer tables, column lists and masters' extra columns read again now. → {version, build}
export const reloadFromDocDesigner = () => request('/designer/reload', { method: 'POST' });

// ─── Screens drawn from the database (routers/designer.py) ─────────────────────────────────────────
/** The screens the Doc Designer defines: [{ doc_id, name, type, short_form, header_table, detail_table, master_path }]. */
export const getDesignerDocs = () => request('/designer/docs');
/** One screen: { doc_id, name, type, header_table, detail_table, master_path, version, fields: [{ code, caption, section,
 *  sub_section, order, type, width, decimals, readonly, hidden, mandatory, options, combo, default, tooltip, hide_when,
 *  mandatory_when, readonly_when, standard, modelled, ... }] }. */
export const getScreenDef = (docId) => request(`/designer/screen/${docId}`);
/** A value that changes when a table or a designer row changes (poll it to know when to read the screen again). */
export const getSchemaVersion = () => request('/designer/version');
export const getDesignerMenu = () => request('/designer/menu');
/** The value a designer pick-up field takes after another field changed (`values`: the other fields by code). */
export const getDesignerPickup = (docId, code, values) => request(`/designer/pickup/${docId}/${encodeURIComponent(code)}?values=${encodeURIComponent(JSON.stringify(values))}`);
/** The next number and code of a code series (a master whose codes are generated). */
export const getDesignerNextCode = (docId, series) => request(`/designer/next-code/${docId}?series=${encodeURIComponent(series)}`);
export const getDesignerLookup = (docId, code, q = '') => request(`/designer/lookup/${docId}/${encodeURIComponent(code)}${q ? `?q=${encodeURIComponent(q)}` : ''}`);
/** The structure-change stream (server-sent events read with fetch, because EventSource can't send the token): calls
 *  `onState({version, build})` for every message; resolves when the stream ends, rejects when it can't be opened. */
export async function streamDesignerEvents(onState, signal) {
  const res = await requestRaw('/designer/events', { signal });
  const reader = res.body.getReader();
  const decoder = new TextDecoder();
  let buffer = '';
  for (;;) {
    const { done, value } = await reader.read();
    if (done) return;
    buffer += decoder.decode(value, { stream: true });
    let cut;
    while ((cut = buffer.indexOf('\n\n')) >= 0) {
      const block = buffer.slice(0, cut);
      buffer = buffer.slice(cut + 2);
      const line = block.split('\n').find(l => l.startsWith('data:'));
      if (line) { try { onState(JSON.parse(line.slice(5))); } catch { /* a malformed message is skipped */ } }
    }
  }
}
export const getAuditLog = (params) =>
  request(`/utilities/audit${qs(params)}`);
export const getAuditScreens = () => request('/utilities/audit/screens');

export const listDuplicateInvoices = (params) =>
  request(`/utilities/duplicate-invoices${qs(params)}`);
export const removeDuplicateInvoices = (body) =>
  request('/utilities/duplicate-invoices/remove', { method: 'POST', body });

// Delete Non-Liquor Items (desktop frmUtlDeleteNonLiqItems), Administrator only. preview -> { group, items, counts, blockers, can_delete };
// delete body { group, confirm, admin_password (encrypted for "admin") } -> { deleted_items, sales_bills, ... }.
export const previewDeleteNonLiquor = (group) => request(`/utilities/delete-non-liquor/preview${qs({ group })}`);
export const deleteNonLiquorItems = (body) => request('/utilities/delete-non-liquor', { method: 'POST', body });
// Carry Over tab: groups to pick, a preview of what goes and what is carried, and the run (Administrator; needs the ADMIN password).
export const getCarryGroups = () => request('/utilities/delete-non-liquor/carry-groups');
export const previewCarryOver = (groups) => request(`/utilities/delete-non-liquor/carry-preview${qs({ groups: groups.join(',') })}`);
export const carryOverItems = (body) => request('/utilities/delete-non-liquor/carry-over', { method: 'POST', body });

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

// The web app's own check answers at once; the desktop database's checks scan the whole bill history (tens of seconds on a
// big shop), so the screen asks for them one by one: getRewriteStockDesktopTab(key, fresh) → {key, caption, error, rows}.
export const getRewriteStock = () => request('/tools/rewrite-stock?desktop=false');
export const getRewriteStockDesktopTab = (key, fresh = false) => request(`/tools/rewrite-stock/desktop/${encodeURIComponent(key)}${fresh ? '?fresh=true' : ''}`);
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
 























































































































































































































// Transaction Settings: the other UtlTransactions settings of the desktop's window (routers/utilities.py: /utilities/trn-settings).
// getTrnSettings() -> { docs: [{doc_id, name, type, kind, values: {column: value}}], fields: [{column, caption, kind}] }
export const getTrnSettings = () => request('/utilities/trn-settings');
export const saveTrnSettings = (docId, values) => request(`/utilities/trn-settings/${docId}`, { method: 'PUT', body: { values } });
// getBulkImportFields() -> { fields: [{name, datatype}] (the table UtlItemBulkUpdate), columns: [MstItem column names], types: ['Text','Integer','Date','Numeric'] }
export const getBulkImportFields = () => request('/utilities/bulk-import-fields');
// saveBulkImportFields([{name, datatype}]) replaces the whole list -> { message, fields }
export const saveBulkImportFields = (fields) => request('/utilities/bulk-import-fields', { method: 'PUT', body: { fields } });
