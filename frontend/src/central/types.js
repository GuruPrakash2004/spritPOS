// Response shapes of the central site's API (backend/app/central/main.py). Column names inside the day's
// tables are the desktop's (RptDSRSmry, RptDSRCtrSales, …), as the shops push them (backend/app/websync.py).

 































/** A shop that hasn't sent anything for this long is shown as not syncing. */
export const STALE_MINUTES = 30;
export const isStale = (iso) => !iso || Date.now() - new Date(iso).getTime() > STALE_MINUTES * 60_000;
export const when = (iso) => (iso ? new Date(iso).toLocaleString() : 'Never');
