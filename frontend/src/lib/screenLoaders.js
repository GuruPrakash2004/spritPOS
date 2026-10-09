// Rarely used screens are loaded on demand, so the first page load carries only the Dashboard, billing and the
// masters. `lazyScreen(name)` is what App.jsx renders (inside <Suspense>); `prefetchScreen(name)` starts the download
// early: when the pointer or keyboard reaches the sidebar link, and for all of them once the app has been idle after
// sign-in, so by the time someone clicks the code is usually already here.

import { lazy } from 'react';

export const SCREEN_LOADERS = {
  'Admin Master': () => import('../components/admin/AdminMaster'),
  'SQL Query Execution': () => import('../components/tools/SqlQueryExecution'),
  'Screen Preview': () => import('../components/tools/ScreenPreview'),
  'Re Write Stock': () => import('../components/tools/ReWriteStock'),
  'Global Settings': () => import('../components/utilities/GlobalSettings'),
  'ADMIN Audit Log': () => import('../components/utilities/AdminAuditLog'),
  'About': () => import('../components/utilities/About'),
  'Remove Duplicate Invoice': () => import('../components/utilities/RemoveDuplicateInvoice'),
  'Web Sync': () => import('../components/utilities/WebSync'),
  'Bulk Master Update': () => import('../components/utilities/BulkMasterUpdate'),
  'Label Printing': () => import('../components/utilities/LabelPrinting'),
  'Merge Items': () => import('../components/utilities/MergeItems'),
  'Compare Stock': () => import('../components/utilities/CompareStock'),
  'Reports Dashboard': () => import('../components/reports/ReportsDashboard'),
  'Daily Summary Report': () => import('../components/reports/DailySummaryReport'),
  'Stock Balance Report': () => import('../components/reports/StockBalanceReport'),
  'Price List Maintenance': () => import('../components/masters/PriceListMaintenance'),
};

const made = {};
/** The lazy component for a screen (the same one every time, so React keeps its state). */
export const lazyScreen = (name) => (made[name] ??= lazy(SCREEN_LOADERS[name]));

/** Start downloading a screen's code; a failed download is ignored here (the real open reports it). */
export const prefetchScreen = (name) => { SCREEN_LOADERS[name]?.().catch(() => undefined); };

/** Prefetch every lazy screen when the browser is idle. Returns a function that cancels it. */
export function prefetchAllWhenIdle() {
  const run = () => Object.keys(SCREEN_LOADERS).forEach(prefetchScreen);
  if (typeof window.requestIdleCallback === 'function') {
    const id = window.requestIdleCallback(run, { timeout: 8000 });
    return () => window.cancelIdleCallback?.(id);
  }
  const t = setTimeout(run, 4000);
  return () => clearTimeout(t);
}
