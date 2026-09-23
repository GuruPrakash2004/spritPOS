---
paths:
  - "frontend/src/**"
---

# React app (`frontend/src/`)

## Shell and routing
`main.tsx` → `App.tsx` is the whole shell. There is **no router**: `App` holds `activeTab: string` in `useState`, and `renderContent()` is a `switch` over that string returning the module component.

**The module name string is the routing key.** It must match exactly across the component's `case` in `App.tsx` and its entry in `components/Sidebar.tsx`. Unmatched names fall through to a "Module X is active" placeholder rather than erroring, so typos fail silently. Use the `add-module` skill for the full checklist.

`App` shows `Login` until a session exists. Role rights (`modules.ts` → `canOpen`) hide sidebar entries and block modules with a No-access panel; `MODULE_GROUPS` there must list every module name. Admin Master is the only module with its own URL, `/adminMaster` (pushState + popstate in `App`); Administrator-only, and the backend `/admin/*` routes check the `X-User` header that `api.ts` sends. Report names found in `reports/reportDefs.ts` (`REPORTS`) are routed to `ApiReport` before the switch, so a new API report only needs a `REPORTS` entry and a sidebar name.

Known routing gap: the `'Customised Reports'` case is unreachable; the sidebar's "Customised Reports" label routes to `'Reports Dashboard'`.

## Component layout (`components/`)
- `OperationalModules.tsx`: Dashboard (KPIs from the DSR) and Stock Balance Report.
- `api.ts`: `api<T>()` fetch wrapper (Vite proxies `/api` to FastAPI on :8000), response types, `qs()`. `ui.ts`: shared class strings and `money()`. `Modal.tsx`: portalled dialog (same reason as Toast). `csv.ts`: CSV parse/export/download.
- `MasterCrud.tsx` + `masterConfigs.ts`: one config-driven list+form screen for simple masters (`Masters.tsx`, `ItemMaster.tsx`).
- `ItemLinesEntry.tsx`: shared cases/bottles grid behind Purchase Indent, Sales Return, Stock Deduction (`StockTransactions.tsx`).
- `reports/`: `ReportShell` (filters, table, totals, Print, Export CSV) + `reportDefs.ts`.
- `CompanyMaster.tsx`, `SupplierMaster.tsx`, `ReportsDashboard.tsx`, `PurchaseBill/` (index + modals + data), `SalesCounterMaster.tsx`, `SalesInvoiceSetup.tsx`, `SalesInvoice.tsx`: modules large enough to warrant their own file.
- `Toast.tsx`: the shared, portalled notification. Use it for every save/error message.
- `stockData.ts`: the shared `SAMPLE_STOCK_DATA` fixture (`as const`). Every stock/report screen derives from it; Sales Invoice templates reference items by index.
- `salesCounters.ts`: counter/invoice types, `PARTIES`, `PAYMENT_TYPES` and bill-number helpers. It is a separate file because of the only-export-components lint rule.
- `formatters.ts`: `numberToWords()` (Indian lakh/crore).
- `images/`: reference screenshots of the bill layouts to match (`purchase_bill.png`, `sales_invoice_bill.png`). Nothing imports them.
- `App.css` and `frontend/README.md` are unused Vite leftovers.

## Sales Invoice flow
Sales Invoice is two steps:
1. `SalesInvoiceSetup.tsx` collects the counter fields plus Invoice No / Payment Mode / Customer, all required. It submits into `App`'s `invoiceDraft` and sets `billOpen`.
2. `SalesInvoice.tsx` is the POS bill built from that draft. Its Close returns to the setup form.

Save Master in Sales Counter Master pre-fills the setup with that counter via `openInvoiceSetup`.

Layout of the bill, top to bottom:
- header strip (date, bill no, previous bill);
- party / sales person / counter;
- scan row, which holds the **Quick Selection** toggle;
- the full-width item grid;
- the totals row (Amounts | Payment | Invoice Value + collected/change);
- the summary bar, which holds the held-bill recall chips;
- the function-key buttons.

Quick Selection starts closed. When open, it docks as a 230px left column beside the item grid.

## Theming internals
- **Dark mode:** `App` toggles `.dark` on the root `div`. `index.css` declares `@custom-variant dark (&:where(.dark, .dark *));`.
- **Neutral scales:** `index.css` redefines the `stone-*` and `slate-*` scales in `@theme inline` as `--ms-50…950`. `:root` sets them to warm clay tints and `.dark` to midnight-navy tints. `inline` is required: without it the variables resolve at `:root` and the `.dark` override never applies.
- **Backdrop:** `.workspace-backdrop` (on `<main>`) draws the sunset glow plus a cursor aura. `App.tsx` drives the aura through `--cursor-x`/`--cursor-y` from a `pointermove` listener. The class sets `isolation: isolate` and gives direct children `z-index: 1`, which is why overlays must portal out (see `Toast.tsx`).
- **Card rule:** `div.rounded-2xl.shadow-sm` gets the Version5 card look and hover lift. Motion is `prefers-reduced-motion` guarded.
