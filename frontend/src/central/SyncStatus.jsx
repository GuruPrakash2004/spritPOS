// Central site → Shops: every shop you can see, when it last sent its day and its stock, and a warning when
// it hasn't sent anything for STALE_MINUTES (its PC is off, its internet is down, or Web Sync isn't set up).

import { RefreshCw } from 'lucide-react';
import { badge, pageHeader, pageSub, pageTitle, panel, secondaryButton, tableBody, tableHead, tableRow } from '../lib/styles';
import { STALE_MINUTES, isStale, when, } from './types';

export default function SyncStatus({ shops, onRefresh }) {
  return (
    <div className="flex flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Shops</h1>
          <p className={pageSub}>Each shop's backend sends its day and stock every few minutes. A shop is flagged after {STALE_MINUTES} minutes without a sync.</p>
        </div>
        <button className={secondaryButton} onClick={onRefresh}><RefreshCw size={14} />Refresh</button>
      </div>
      <section className={`${panel} overflow-hidden`}>
        <table className="min-w-full text-xs">
          <thead><tr>{['Shop', 'Name', 'Last day sent', 'Day sent at', 'Stock sent at', 'Status'].map(h => <th key={h} className={`${tableHead} px-3 py-1.5`}>{h}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {shops.length === 0 && <tr><td colSpan={6} className="px-3 py-3 text-stone-500">No shops yet. The central administrator adds them (tools/central.py add-client).</td></tr>}
            {shops.map(s => (
              <tr key={s.code} className={tableRow}>
                <td className="px-3 py-1.5 font-medium">{s.code}</td>
                <td className="px-3 py-1.5">{s.name}</td>
                <td className="px-3 py-1.5">{s.last_day ?? '–'}</td>
                <td className="px-3 py-1.5">{when(s.last_sync)}</td>
                <td className="px-3 py-1.5">{when(s.last_stock_sync)}</td>
                <td className="px-3 py-1.5">
                  {!s.active ? <span className={badge('neutral')}>Disabled</span>
                    : isStale(s.last_sync) ? <span className={badge('danger')}>Not syncing</span>
                    : <span className={badge('success')}>Syncing</span>}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}
